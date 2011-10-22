#!/usr/bin/sbcl --script
(load "analysis/analysis.lisp")

(when (> (length *posix-argv*) 1)
  (format t "~a~&" (cdr *posix-argv*))) ; debug
