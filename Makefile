all: parse_and_analyze analyze

analyze: analyzer.lisp wrap-analyzer.lisp
	sbcl --load wrap-analyzer.lisp

local-install:
	cp parse_and_analyze ~/bin/
	cp analyze ~/bin/

clean:
	rm build.html build.png analyze

test:
	perl t/*.t
