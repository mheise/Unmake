clean:
	rm build.html build.png

test:
	perl t/*.t
	sbcl --load wrap-analyzer.lisp
