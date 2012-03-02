all: parse_and_analyze analyze

analyze: analyzer.lisp wrap-analyzer.lisp
	sbcl --load wrap-analyzer.lisp

local-install: parse_and_analyze analyze CommonBuildFormat.pm
	cp parse_and_analyze ~/bin/
	cp analyze ~/bin/
	cp CommonBuildFormat.pm /usr/local/lib/site_perl/

clean:
	rm build.html build.png analyze

test:
	perl t/*.t
