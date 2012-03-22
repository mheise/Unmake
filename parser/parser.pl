#!/usr/bin/env perl
use warnings;
use strict;
use 5.010;

use CommonBuildFormat;
use Makefile::Parser::GmakeDB;

# AUTHORS:  Mike Heise
# SYNOPSIS: Driver for parsing of makefile
# Pretty straightforward: we parse the makefile, using make itself to do an end
# run around any heavy duty true parsing ourselves.  This also gets us a huge
# amount of other work done 'for free', and relieves us from considering a whole
# host of corner cases.  Unfortunately, this doesn't give us a solution in the
# case where the build system is constructed from recursive makefiles, as the
# main make process doesn't have any sort of communication with the instances of
# make called to handle the recursive subdirectories.

# options
die "USAGE: $0 [makefile]\n"
    if $#ARGV > 0;
my $target = $ARGV[0] // 'Makefile';

# parse. i love leveraging existing libraries!
my $db_listing = `make --print-data-base -pqRrs -f $target`;
my $ast = Makefile::Parser::GmakeDB->parse(\$db_listing);

# walk our AST an analyze it
my $cbf = CommonBuildFormat->new(ast => $ast);

my $tpp = XML::TreePP->new;
$tpp->set(indent => 4, output_encoding => 'UTF-8');
my $xml = $tpp->write($cbf->tree);
print $xml;
