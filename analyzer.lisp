(ql:quickload "xmls")                   ;TODO: document quicklisp as a dependency

(defpackage :unmake
  (:use :common-lisp))
(in-package :unmake)

(defun to-sexpr (input-file)
  "Parse a file in the Common Build Format and return an sexpr representing
  the dependency tree contained therein."
  (with-open-file (xml-str input-file :direction :input)
    (let ((xml-sexpr (xmls:parse xml-str)))
      (cddr xml-sexpr)
      )))

(defun count-rules (xml-sexpr)
  (if (eq xml-sexpr nil) 0
    (+ 1 (count-rules (cdr xml-sexpr)))))
