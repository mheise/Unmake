all: parse analyze

parse: parser/parser.pl
	cp parser/parser.pl ./parse

analyze: analyzer/analyzer.lisp analyzer/wrap-analyzer.lisp
	sbcl --load analyzer/wrap-analyzer.lisp

local-install: parse analyze parser/CommonBuildFormat.pm
	cp parse ~/bin/parse_build_system
	cp analyze ~/bin/analyze_build_system
	cp parser/CommonBuildFormat.pm /usr/local/lib/site_perl/

clean:
	rm build.html build.png analyze parse

test:
	perl t/*.t
