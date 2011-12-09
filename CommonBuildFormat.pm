package CommonBuildFormat;
use Makefile::Parser::GmakeDB;
use XML::TreePP;
use GraphViz;                           # dirty! DIRTY!
use Moose; # turns on strict and warnings, too! :)

# NAME:         CommonBuildFormat.pm
# AUTHORS:      Mike Heise
# SYNOPSIS:     Class encapsulating the common build format
# MODULE:       Parser
# MODULE DATA:  None
# Notes:        See below

# nb. that this module still somewhat suffers from unwarranted familiarity of
# subcomponents - eg. for the 2.0 release, the GraphViz code will be refactored
# out of the the helper to the constructor to its own separate home, and the XML
# tree code will be brought in from outside (since internally we're working with
# a nested hash structure that only makes sense in the context of being fed to
# XML::TreePP)

# true instance vars
has 'ast' => (is => 'rw', isa => 'Makefile::AST', required => 1);
has 'tree' => (is => 'rw', isa => 'HashRef', default => sub { {} });
has 'graph' => (is => 'ro', isa => 'GraphViz');
has 'image' => (is => 'ro', isa => 'Str', default => 'build.png');
has 'depth' => (is => 'rw', isa => 'Int', default => 0);
has 'achilles' => (is => 'rw', isa => 'Str');
has 'broken' => (is => 'rw', isa => 'Str');

# housekeeping vars, will eventually be refactored out
has 'deps' => (is => 'ro', isa => 'HashRef', default => sub { {} });
has 'edges' => (is => 'ro', isa => 'HashRef', default => sub { {} });

sub BUILD {
    # custom section of the constructor, gets called automatically by the ctor
    # nb 'all' is the default target for plain calls to 'make', and thus the
    # root of the build tree we care about
    my $self = shift;
    $self->depth(_traverse($self, $self->ast, $self->tree, 'all', 0));
}

sub _traverse {
    # helper method for ctor - trees are inherently recursive data structures,
    # and we don't want to call our constructor recursively (it would work, but
    # it's self-evidently vile)
    #
    # the tree this builds in order to eventually feed to XML::TreePP is
    # demonstrated in auxiliary/xml_gen.pl for reference, although the basic
    # nested hash is pretty simple
    my ($self, $ast, $tree, $nodename, $depth) = @_;
    my ($node) = grep {$_->target eq $nodename} @{$ast->explicit_rules};
    my $greatest = $depth;
    $tree->{rule} = [] unless exists $tree->{rule};

    if (defined $node) {
        my $parent = $node->target;
        $self->graph->add_node($parent);
        push @{$tree->{rule}}, {"-name" => $parent, dep => []};
        #consider keeping track of this ref instead of grepping for it later
        for my $child (@{$node->{normal_prereqs}}, @{$node->{ordered_prereqs}}) {
            if (! $self->edges->{$parent}{$child}) {
                $self->edges->{$parent}{$child} = 1;
                $self->graph->add_edge($parent, $child);
                my $elem = (grep {$_->{"-name"} eq $parent}
                    @{$tree->{rule}})[0];
                push @{$elem->{dep}}, $child;
                my $cur = _traverse($self, $ast, $tree, $child, $depth + 1);
                $greatest = ($cur > $greatest ? $cur : $greatest);
            }
            $self->deps->{$child} = (defined $self->deps->{$child} ?
                $self->deps->{$child}+1 : 1);
        }
    }

    return ($depth > $greatest ? $depth : $greatest);
}

1; # ending with true value -> successful 'use' statement
