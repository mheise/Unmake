#!/usr/bin/env perl

use XML::TreePP;
use Data::Dumper;

my $tpp = XML::TreePP->new();
my $tree = $tpp->parsefile( $ARGV[0] );
$Data::Dumper::Varname = 'tree';
my $text = Dumper( $tree );

print $text;
