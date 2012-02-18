(ql:quickload "xmls")                   ;TODO: add quicklisp as a dependency

(defun analyze (input-file)
  (with-open-file (xml-str input-file :direction :input)
    (let ((xml-sexpr (xmls:parse xml-str)))
      (princ xml-sexpr)
      (princ #\newline))))
