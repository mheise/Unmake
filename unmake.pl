#!/usr/bin/env perl
# unmake.pl - driver for other unmake subcomponents
# (c) 2012 Mike Heise

use warnings;
use strict;
use 5.010;

# quick & dirty argument processing

given ($ARGV[0]) {
    when (undef) {analyze(); projgen();}
    when (/^-parse/) {parse();}
    when (/^-analyze/) {analyze();}
    default {say STDERR "$_ usage: unmake [-p|a]"}
}

sub parse {
    my $redirflag = shift;
    my $ret;
    if ($redirflag) {
        $ret = system +("parse_build_system > cbf.xml");
    } else {
        $ret = system +("parse_build_system");
    }
    die "Couldn't call parser: $!" if $ret == -1;
}

sub analyze {
    parse('redirect to cbf.xml') unless -e 'Makefile.mks';

    my $ret = system "analyze_build_system";
    die "Couldn't call parser: $!" if $ret == -1;

    unlink 'Makefile.mks';
}

sub projgen {
    my $ret = system "projgen";
    die "Couldn't call parser: $!" if $ret == -1;
}
