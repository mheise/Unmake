#!/usr/bin/env perl

use XML::TreePP;

my $tpp = XML::TreePP->new;
$tpp->set(indent => 4);

my $tree = {
    'rule' => [
    { 
        '-name' => 'foo',
        'dep' => [
        'bar',
        'baz',
        'fred'
        ]
    },
    { 
        '-name' => 'bar',
        'dep' => [
        'baz',
        'quux'
        ]
    },
    { 
        '-name' => 'baz',
        'dep' => [
        'quux',
        'baz.c'
        ]
    },
    { 
        '-name' => 'fred',
        'dep' => [
        'fred.c'
        ]
    },
    { 
        '-name' => 'quux',
        'dep' => [
        'quux.c'
        ]
    }
    ]
};

print $tpp->write($tree);
