(load "analyzer.lisp")
(in-package :unmake)

(defun analyze-wrapper ()
  (analyze (cadr sb-ext:*posix-argv*))) ;usage: ./analyze cbf.xml

(sb-ext:save-lisp-and-die "analyze" :executable t :toplevel 'analyze-wrapper)
