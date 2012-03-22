package CommonBuildFormat;
use Makefile::Parser::GmakeDB;
use XML::TreePP;
use GraphViz;                           # dirty! DIRTY!
use Moose; # turns on strict and warnings, too! :)

# AUTHORS:      Mike Heise
# SYNOPSIS:     Class encapsulating the common build format

# nb. that this module still somewhat suffers from unwarranted familiarity of
# subcomponents - eg. for the 2.0 release, the GraphViz code will be refactored
# out of the the helper to the constructor to its own separate home, and the XML
# tree code will be brought in from outside (since internally we're working with
# a nested hash structure that only makes sense in the context of being fed to
# XML::TreePP)

# true instance vars
has 'ast' => (is => 'rw', isa => 'Makefile::AST', required => 1);
has 'tree' => (is => 'rw', isa => 'HashRef', default => sub { {buildsystem => {}} });
has 'image' => (is => 'ro', isa => 'Str', default => 'build.png');

# housekeeping vars, will eventually be refactored out
has 'edges' => (is => 'ro', isa => 'HashRef', default => sub { {} });

sub BUILD {
    # custom section of the constructor, gets called automatically by the ctor
    # nb 'all' is the default target for plain calls to 'make', and thus the
    # root of the build tree we care about, and also nb. the hack here to put
    # everything inside a <build> element
    my $self = shift;
    _traverse($self, $self->ast, $self->tree->{buildsystem}, 'all');
}

sub _traverse {
    # helper method for ctor - trees are inherently recursive data structures,
    # and we don't want to call our constructor recursively (it would work, but
    # it's self-evidently vile)
    #
    # the tree this builds in order to eventually feed to XML::TreePP is
    # demonstrated in auxiliary/xml_gen.pl for reference, although the basic
    # nested hash is pretty simple
    my ($self, $ast, $tree, $nodename) = @_;
    my ($node) = grep {$_->target eq $nodename} @{$ast->explicit_rules};
    $tree->{file} = [] unless exists $tree->{file};

    if (defined $node) {
        my $parent = $node->target;
        push @{$tree->{file}}, {"-name" => $parent, dep => []};
        #consider keeping track of this ref instead of grepping for it later
        for my $child (@{$node->{normal_prereqs}}, @{$node->{ordered_prereqs}}) {
            if (! $self->edges->{$parent}{$child}) {
                $self->edges->{$parent}{$child} = 1;
                my $elem = (grep {$_->{"-name"} eq $parent}
                    @{$tree->{file}})[0];
                push @{$elem->{dep}}, $child;
                my $cur = _traverse($self, $ast, $tree, $child);
            }
        }
    }
}

1; # ending with true value -> successful 'use' statement
