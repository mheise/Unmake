#!/usr/bin/env perl
use warnings;
use strict;
use 5.010;

use HTML::Template;                     # kind of dirty :(
use CommonBuildFormat;

# AUTHORS:      Mike Heise
# SYNOPSIS:     Driver for parsing and analysis of makefile
# driver program for parsing and analysis. originally only the parsing was done
# in Perl and the analysis was all done in Lisp, with the Common Build Format
# file as an intermediary, but the libraries for dealing with XML in Lisp were
# difficult to get to install on the system this was built on, so the analysis
# was brought back to Perl in the interest of getting a working product. 
# This is in the process of being rectified, and real analysis is now being done
# in analyzer/analyzer.lisp, but the generation of the graphviz visualization is
# still here, as well as the (currently disabled) html template stuff.  The end
# goal is that eventually this parser will do nothing but generate the CBF file.

# options
die "USAGE: $0 [makefile]\n"
    if $#ARGV > 0;
my $target = $ARGV[0] // 'Makefile';

# parse. i love leveraging existing libraries!
my $db_listing = `make --print-data-base -pqRrs -f $target`;
my $ast = Makefile::Parser::GmakeDB->parse(\$db_listing);

# walk our AST an analyze it
my $cbf = CommonBuildFormat->new(ast => $ast, graph => GraphViz->new);

my $tpp = XML::TreePP->new;
$tpp->set(indent => 4, output_encoding => 'UTF-8');
my $xml = $tpp->write($cbf->tree);
print $xml;

my @ordered_deps = sort {$cbf->deps->{$b} <=> $cbf->deps->{$a}}
                        keys %{$cbf->deps};

#output the graphviz image
print STDERR "Saving build system graph image as @{[$cbf->image]} ...\n";
open my $imgfh, '>', $cbf->image;
print $imgfh $cbf->graph->as_png;

=begin
make_template(  depth => $cbf->depth,
                achilles => $ordered_deps[0] // undef,
                broken => (defined $ordered_deps[0] ? undef : "make couldn't read makefile sensibly"),
                bad => ($cbf->deps->{$ordered_deps[0]} > 2 ? 'chokepoints exist' : 0),
                image => $cbf->image,
);
=cut

# End of procedural section, beginning of helper functions
sub make_template {
    my %args = @_;
    open my $htmlfh, '>', 'build.html';

    my $template = HTML::Template->new(filename => 'report.tmpl');
    $template->param(DEPTH => $args{depth});
    $template->param(ACHILLES => $args{achilles} // 'nothing');
    $template->param(BROKEN => $args{broken} // 'none :)');
    $template->param(BAD => $args{bad} // 'none :)');
    $template->param(IMAGE => $args{image});

    print $htmlfh $template->output;
}
