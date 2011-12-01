#!/usr/bin/env perl
use warnings;
use strict;
use 5.010;

use Makefile::Parser::GmakeDB;
use GraphViz;                           # dirty! DIRTY!
use HTML::Template;                     # ditto :(
use XML::TreePP;

# parse
my $db_listing = `make --print-data-base -pqRrs -f Makefile`;
my $ast = Makefile::Parser::GmakeDB->parse(\$db_listing);

# walk our AST an analyze it
my ($image, $depth, $achilles, $broken) = ('build.png', 0, undef, undef);
our $g = GraphViz->new; # blarg
our %deps;
our %edges;

my $tree = {};
$depth = traverse($ast, $tree, 'all', 0);
my $tpp = XML::TreePP->new;
my $xml = $tpp->write($tree);
print $xml;

my @ordered_deps = sort {$deps{$b} <=> $deps{$a}} keys %deps;
make_template(  depth => $depth,
                achilles => $ordered_deps[0],
                broken => $broken,
                bad => ($deps{$ordered_deps[0]} > 5 ? 'chokepoints exist' : 0),
                image => $image,
);
open my $imgfh, '>', $image;
print $imgfh $g->as_png;

# =============================================================================
# Here endeth the procedural section, and beginneth function defs
# ============================================================================

sub traverse {
    my ($ast, $tree, $nodename, $depth) = @_;
    my ($node) = grep {$_->target eq $nodename} @{$ast->explicit_rules};
    my $greatest = $depth;

    if (defined $node) {
        my $parent = $node->target;
        $g->add_node($parent);
        $tree->{$parent} = {dep => []};
        for my $child (@{$node->{normal_prereqs}}, @{$node->{ordered_prereqs}}) {
            if (! $edges{$parent}{$child}) {
                $edges{$parent}{$child} = 1;
                $g->add_edge($parent, $child);
                push @{$tree->{$parent}->{dep}}, $child;
                my $cur = traverse($ast, $tree, $child, $depth + 1);
                $greatest = ($cur > $greatest ? $cur : $greatest);
            }
            $deps{$child} = (defined $deps{$child} ? $deps{$child}+1 : 1);
        }
    }

    return ($depth > $greatest ? $depth : $greatest);
}

sub make_template {
    my %args = @_;
    open my $htmlfh, '>', 'build.html';

    my $template = HTML::Template->new(filename => 'analysis.tmpl');
    $template->param(DEPTH => $args{depth});
    $template->param(ACHILLES => $args{achilles} // 'nothing');
    $template->param(BROKEN => $args{broken} // 'none :)');
    $template->param(BAD => $args{bad} // 'none :)');
    $template->param(IMAGE => $args{image});

    print $htmlfh $template->output;
}

