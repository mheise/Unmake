all: unmake 

unmake: unmake.pl parse analyze
	perl -c unmake.pl
	cp unmake.pl unmake

parse: parser/parser.pl parser/CommonBuildFormat.pm
	perl -c parser/CommonBuildFormat.pm
	perl -c parser/parser.pl
	cp parser/parser.pl ./parse

analyze: analyzer/analyzer.lisp analyzer/wrap-analyzer.lisp
	sbcl --load analyzer/wrap-analyzer.lisp

local-install: unmake parse analyze parser/CommonBuildFormat.pm
	cp parse ~/bin/parse_build_system
	cp analyze ~/bin/analyze_build_system
	cp parser/CommonBuildFormat.pm /usr/local/lib/site_perl/
	cp unmake ~/bin/unmake

install: unmake parse analyze parser/CommonBuildFormat.pm
	cp parse /usr/bin/parse_build_system
	cp analyze /usr/bin/analyze_build_system
	cp parser/CommonBuildFormat.pm /usr/local/lib/site_perl/
	cp unmake /usr/bin/unmake
	chmod +x /usr/bin/unmake

clean:
	rm analyze parse unmake

test:
	prove
