(ql:quickload "xmls")                   ;TODO: document quicklisp as a dependency

(defpackage :unmake
  (:use :common-lisp))
(in-package :unmake)

(defun to-sexpr (cbf-file)
  "Parse a file in the Common Build Format and return an sexpr representing
  the dependency tree contained therein."
  (with-open-file (cbf-str cbf-file :direction :input)
    (let ((cbf-sexpr (xmls:parse cbf-str)))
      (cddr cbf-sexpr)
      )))

(defun count-rules (cbf-sexpr)
  "Returns the number of rules defined in the dependency tree represented by
  cbf-sexpr"
  (if (eq cbf-sexpr nil) 0
    (+ 1 (count-rules (cdr cbf-sexpr)))))

(defun analyze (cbf-file)
  "Analyze and report on the properties of a Common Build Format file"
  (format t "Report for the build system described in ~a: ~&" cbf-file)
  (format t "    Number of rules: ~d ~&" (count-rules (to-sexpr cbf-file))))
