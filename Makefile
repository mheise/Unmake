all:
	sbcl --load wrap-analyzer.lisp

clean:
	rm build.html build.png analyze

test:
	perl t/*.t
