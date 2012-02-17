#!/usr/bin/sbcl --script
(load "xmls.lisp")
(with-open-file (xml-str "cbf.xml" :direction :input)
  (let ((xml-sexpr (xmls:parse xml-str)))
    (princ xml-sexpr)
    (princ #\newline)))
