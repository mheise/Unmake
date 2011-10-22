#!/usr/bin/env perl
use warnings;
use strict;

use Makefile::Parser::GmakeDB;

my $db = `make --print-data-base -pqRrs -f Makefile`;

