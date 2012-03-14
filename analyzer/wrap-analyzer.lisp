(load "analyzer/analyzer.lisp")
(in-package :unmake)

(defun analyze-wrapper ()
  (analyze (cadr sb-ext:*posix-argv*))) ;usage: ./analyze <cbf-file.xml>

(sb-ext:save-lisp-and-die "analyze" :executable t :toplevel 'analyze-wrapper)
