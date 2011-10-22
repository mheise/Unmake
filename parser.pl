#!/usr/bin/env perl
use warnings;
use strict;
use 5.010;

use Makefile::Parser::GmakeDB;

my $db_listing = `make --print-data-base -pqRrs -f Makefile`;
my $ast = Makefile::Parser::GmakeDB->parse(\$db_listing);

# print phony targets
my @phonies = ( keys %{$ast->phony_targets});
say for @phonies;
