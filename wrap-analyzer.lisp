(load "analyzer.lisp")

(defun analyze-wrapper ()
  (analyze (cadr *posix-argv*))) ;usage: ./analyze cbf.xml

(sb-ext:save-lisp-and-die "analyze" :executable t :toplevel 'analyze-wrapper)
