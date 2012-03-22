#!/usr/bin/env perl
use warnings;
use strict;
use 5.010;

use HTML::Template;                     # kind of dirty :(
use CommonBuildFormat;

# AUTHORS:      Mike Heise
# SYNOPSIS:     Driver for parsing and analysis of makefile
# Originally only the parsing was done
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
print STDERR "make finished"; #XXX
my $ast = Makefile::Parser::GmakeDB->parse(\$db_listing);

# walk our AST an analyze it
my $cbf = CommonBuildFormat->new(ast => $ast);

my $tpp = XML::TreePP->new;
$tpp->set(indent => 4, output_encoding => 'UTF-8');
my $xml = $tpp->write($cbf->tree);
print $xml;
