(ql:quickload "xmls")                   ;TODO: add quicklisp as a dependency

(defun to-sexpr (input-file)
  (with-open-file (xml-str input-file :direction :input)
    (let ((xml-sexpr (xmls:parse xml-str)))
      (cddr xml-sexpr)
      )))
