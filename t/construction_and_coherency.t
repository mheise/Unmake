use Test::More;
use Makefile::Parser::GmakeDB;
use CommonBuildFormat;

my $mock_contents = do {local $/; <DATA>}; # see __END__ section
my $ast = Makefile::Parser::GmakeDB->parse(\$mock_contents);

# first check that our constructor returned an object of the right type
my $cbf = CommonBuildFormat->new(ast => $ast, graph => GraphViz->new);
isa_ok($cbf, 'CommonBuildFormat');

# now make sure our instance objects were constructed sensibly
isa_ok($cbf->ast, 'Makefile::AST');
isa_ok($cbf->graph, 'GraphViz');

# now, test actual business logic to make sure our traversal is right. nb that
# these values correspond the the mocked makefile output contained in output.txt
# in this directory.

like(ref $cbf->deps, qr/HASH/, "Internal dependency tracking correctly set up");
like(ref $cbf->edges, qr/HASH/, "Internal edge tracking correctly set up");

like(ref $cbf->tree, qr/HASH/, "Internal tree correctly set up");
like($cbf->tree->{rule}, qr/ARRAY/, "Internal tree sane");
ok($cbf->depth == 3, "Depth calculated correctly");

done_testing();


# now comes a giant heredoc so that we don't have to ship the mock make db
# output separately and deal with the issues that come with that
__END__
# GNU Make 3.81
# Copyright (C) 2006  Free Software Foundation, Inc.
# This is free software; see the source for copying conditions.
# There is NO warranty; not even for MERCHANTABILITY or FITNESS FOR A
# PARTICULAR PURPOSE.

# This program built for i486-pc-linux-gnu

# Make data base, printed on Fri Dec  9 08:01:21 2011

# Variables

# automatic
<D = $(patsubst %/,%,$(dir $<))
# automatic
?F = $(notdir $?)
# automatic
?D = $(patsubst %/,%,$(dir $?))
# makefile (from `Makefile', line 21)
INSTALL = /usr/local/bin/install -c
# makefile (from `Makefile', line 75)
DEFS = -DSIGTYPE=int -DDIRENT -DSTRSTR_MISSING -DVPRINTF_MISSING -DBSD42
# automatic
@D = $(patsubst %/,%,$(dir $@))
# automatic
@F = $(notdir $@)
# makefile
CURDIR := /home/mike/src/unmake/tar_test
# makefile (from `Makefile', line 9)
SHELL = /bin/sh
# environment
_ = /usr/bin/make
# makefile (from `Makefile', line 85)
CFLAGS = $(CDEBUG) -I. -I$(srcdir) $(DEFS) -DDEF_AR_FILE=\"$(DEF_AR_FILE)\" -DDEFBLOCKING=$(DEFBLOCKING)
# makefile (from `Makefile', line 1)
MAKEFILE_LIST :=  Makefile
# makefile (from `Makefile', line 103)
SRC1 = tar.c create.c extract.c buffer.c getoldopt.c update.c gnu.c mangle.c
# environment
HISTCONTROL = ignoredups
# makefile (from `Makefile', line 114)
OBJS = $(OBJ1) $(OBJ2) $(OBJ3)
# makefile (from `Makefile', line 84)
CDEBUG = -g
# environment
LESSOPEN = | /usr/bin/lesspipe %s
# environment
GIT_EDITOR = vim
# environment
SSH_CONNECTION = 192.168.254.1 54047 192.168.254.129 22
# environment
PATH = /usr/local/sbin:/usr/local/bin:/usr/sbin:/usr/bin:/sbin:/bin:/usr/games
# makefile (from `Makefile', line 81)
DEF_AR_FILE = /dev/rmt8
# makefile (from `Makefile', line 109)
OBJ1 = tar.o create.o extract.o buffer.o getoldopt.o update.o gnu.o mangle.o
# makefile (from `Makefile', line 20)
YACC = bison -y
# makefile (from `Makefile', line 105)
SRC2 = version.c list.c names.c diffarch.c port.c wildmat.c getopt.c
# environment
SSH_TTY = /dev/pts/0
# makefile (from `Makefile', line 88)
LDFLAGS = -g
# default
.FEATURES := target-specific order-only second-expansion else-if archives jobserver check-symlink
# makefile (from `Makefile', line 79)
RTAPELIB = rtapelib.o
# environment
LS_COLORS = rs=0:di=01;34:ln=01;36:hl=44;37:pi=40;33:so=01;35:do=01;35:bd=40;33;01:cd=40;33;01:or=40;31;01:su=37;41:sg=30;43:ca=30;41:tw=30;42:ow=34;42:st=37;44:ex=01;32:*.tar=01;31:*.tgz=01;31:*.arj=01;31:*.taz=01;31:*.lzh=01;31:*.lzma=01;31:*.zip=01;31:*.z=01;31:*.Z=01;31:*.dz=01;31:*.gz=01;31:*.bz2=01;31:*.bz=01;31:*.tbz2=01;31:*.tz=01;31:*.deb=01;31:*.rpm=01;31:*.jar=01;31:*.rar=01;31:*.ace=01;31:*.zoo=01;31:*.cpio=01;31:*.7z=01;31:*.rz=01;31:*.jpg=01;35:*.jpeg=01;35:*.gif=01;35:*.bmp=01;35:*.pbm=01;35:*.pgm=01;35:*.ppm=01;35:*.tga=01;35:*.xbm=01;35:*.xpm=01;35:*.tif=01;35:*.tiff=01;35:*.png=01;35:*.svg=01;35:*.svgz=01;35:*.mng=01;35:*.pcx=01;35:*.mov=01;35:*.mpg=01;35:*.mpeg=01;35:*.m2v=01;35:*.mkv=01;35:*.ogm=01;35:*.mp4=01;35:*.m4v=01;35:*.mp4v=01;35:*.vob=01;35:*.qt=01;35:*.nuv=01;35:*.wmv=01;35:*.asf=01;35:*.rm=01;35:*.rmvb=01;35:*.flc=01;35:*.avi=01;35:*.fli=01;35:*.flv=01;35:*.gl=01;35:*.dl=01;35:*.xcf=01;35:*.xwd=01;35:*.yuv=01;35:*.axv=01;35:*.anx=01;35:*.ogv=01;35:*.ogx=01;35:*.aac=00;36:*.au=00;36:*.flac=00;36:*.mid=00;36:*.midi=00;36:*.mka=00;36:*.mp3=00;36:*.mpc=00;36:*.ogg=00;36:*.ra=00;36:*.wav=00;36:*.axa=00;36:*.oga=00;36:*.spx=00;36:*.xspf=00;36:
# makefile (from `Makefile', line 115)
AUX = README COPYING ChangeLog Makefile.in makefile.pc configure configure.in tar.texinfo tar.info* texinfo.tex tar.h port.h open3.h getopt.h regex.h rmt.h rmt.c rtapelib.c alloca.c msd_dir.h msd_dir.c tcexparg.c level-0 level-1 backup-specs testpad.c
# automatic
%F = $(notdir $%)
# makefile (from `Makefile', line 13)
srcdir = .
# environment
PWD = /home/mike/src/unmake/tar_test
# makefile (from `Makefile', line 96)
bindir = $(prefix)/bin
# automatic
*D = $(patsubst %/,%,$(dir $*))
# environment
HOME = /home/mike
# environment
LESSCLOSE = /usr/bin/lesspipe %s %s
# environment
LOGNAME = mike
# automatic
^D = $(patsubst %/,%,$(dir $^))
# makefile (from `Makefile', line 107)
SRC3 = getopt1.c regex.c getdate.y
# default
MAKE = $(MAKE_COMMAND)
# makefile (from `Makefile', line 108)
SRCS = $(SRC1) $(SRC2) $(SRC3)
# environment
SHLVL = 2
# makefile (from `Makefile', line 19)
CC = gcc -O
# default
MAKE_VERSION := 3.81
# environment
USER = mike
# makefile
.DEFAULT_GOAL := all
# makefile (from `Makefile', line 22)
INSTALLDATA = /usr/local/bin/install -c -m 644
# automatic
%D = $(patsubst %/,%,$(dir $%))
# makefile (from `Makefile', line 111)
OBJ2 = version.o list.o names.o diffarch.o port.o wildmat.o getopt.o
# makefile (from `Makefile', line 82)
DEFBLOCKING = 20
# default
MAKE_COMMAND := make
# makefile (from `Makefile', line 90)
prefix = /usr/local
# default
.VARIABLES := 
# automatic
*F = $(notdir $*)
# environment
LANGUAGE = en_US:en
# makefile
MAKEFLAGS = sRrqp
# environment
MFLAGS = -sRrqp
# environment
SSH_CLIENT = 192.168.254.1 54047 22
# environment
MAIL = /var/mail/mike
# automatic
+D = $(patsubst %/,%,$(dir $+))
# automatic
+F = $(notdir $+)
# makefile (from `Makefile', line 99)
infodir = $(prefix)/info
# makefile (from `Makefile', line 93)
binprefix = 
# default
MAKEFILES := 
# automatic
<F = $(notdir $<)
# environment
MYPATH = /bin|/usr/bin|/usr/local/bin|/sbin|/usr/sbin|/usr/local/sbin|.
# environment
EDITOR = vim
# makefile (from `Makefile', line 80)
LIBS = 
# automatic
^F = $(notdir $^)
# default
SUFFIXES := 
# default
.INCLUDE_DIRS = /usr/include /usr/local/include /usr/include
# makefile (from `Makefile', line 113)
OBJ3 = getopt1.o regex.o getdate.o $(RTAPELIB)
# environment
MAKELEVEL := 0
# environment
LANG = en_US.UTF-8
# environment
TERM = xterm
# variable set hash-table stats:
# Load=76/1024=7%, Rehash=0, Collisions=12/139=9%

# Pattern-specific Variable Values

# No pattern-specific variable values.

# Directories

# . (device 64256, inode 271451): 7 files, no impossibilities.

# 7 files, no impossibilities in 1 directories.

# Implicit Rules

# No implicit rules.

# Files

wildmat.o: wildmat.c
#  Implicit rule search has not been done.
#  Implicit/static pattern stem: `wildmat'
#  Modification time never checked.
#  File has not been updated.

# Not a target:
backup-specs:
#  Implicit rule search has not been done.
#  Modification time never checked.
#  File has not been updated.

getdate.o: getdate.c
#  Implicit rule search has not been done.
#  Implicit/static pattern stem: `getdate'
#  Modification time never checked.
#  File has not been updated.

tar.c:
#  Implicit rule search has been done.
#  File does not exist.
#  File has been updated.
#  Successfully updated.
# variable set hash-table stats:
# Load=0/32=0%, Rehash=0, Collisions=0/0=0%

getdate.y:
#  Implicit rule search has not been done.
#  Modification time never checked.
#  File has not been updated.

# Not a target:
open3.h:
#  Implicit rule search has not been done.
#  Modification time never checked.
#  File has not been updated.

list.c:
#  Implicit rule search has not been done.
#  Modification time never checked.
#  File has not been updated.

all: tar rmt tar.info
#  Implicit rule search has been done.
#  File does not exist.
#  File has been updated.
#  Needs to be updated (-q is set).
# variable set hash-table stats:
# Load=0/32=0%, Rehash=0, Collisions=0/2=0%

getopt.c:
#  Implicit rule search has not been done.
#  Modification time never checked.
#  File has not been updated.

regex.c:
#  Implicit rule search has not been done.
#  Modification time never checked.
#  File has not been updated.

rtapelib.o: rtapelib.c
#  Implicit rule search has not been done.
#  Implicit/static pattern stem: `rtapelib'
#  Modification time never checked.
#  File has not been updated.

.PHONY: clean distclean realclean
#  Implicit rule search has not been done.
#  Modification time never checked.
#  File has not been updated.

# Not a target:
msd_dir.h:
#  Implicit rule search has not been done.
#  Modification time never checked.
#  File has not been updated.

testpad: testpad.o
#  Implicit rule search has not been done.
#  Modification time never checked.
#  File has not been updated.
#  commands to execute (from `Makefile', line 146):
	$(CC) -o $@ testpad.o
	

# Not a target:
rmt.c:
#  Implicit rule search has not been done.
#  Modification time never checked.
#  File has not been updated.

# Not a target:
testpad.o:
#  Implicit rule search has not been done.
#  Modification time never checked.
#  File has not been updated.

# Not a target:
tar.texinfo:
#  Implicit rule search has not been done.
#  Modification time never checked.
#  File has not been updated.

# Not a target:
README:
#  Implicit rule search has not been done.
#  Modification time never checked.
#  File has not been updated.

distclean: clean
#  Phony target (prerequisite of .PHONY).
#  Implicit rule search has not been done.
#  File does not exist.
#  File has not been updated.
#  commands to execute (from `Makefile', line 155):
	rm -f TAGS Makefile config.status
	

# Not a target:
tar.h:
#  Implicit rule search has not been done.
#  Modification time never checked.
#  File has not been updated.

extract.o: extract.c
#  Implicit rule search has not been done.
#  Implicit/static pattern stem: `extract'
#  Modification time never checked.
#  File has not been updated.

# Not a target:
getopt.h:
#  Implicit rule search has not been done.
#  Modification time never checked.
#  File has not been updated.

regex.h:
#  Implicit rule search has been done.
#  File does not exist.
#  File has been updated.
#  Successfully updated.
# variable set hash-table stats:
# Load=0/32=0%, Rehash=0, Collisions=0/0=0%

# Not a target:
.SUFFIXES:
#  Implicit rule search has not been done.
#  Modification time never checked.
#  File has not been updated.

diffarch.o: diffarch.c
#  Implicit rule search has not been done.
#  Implicit/static pattern stem: `diffarch'
#  Modification time never checked.
#  File has not been updated.

# Not a target:
Makefile:
#  Implicit rule search has been done.
#  Last modified 2011-12-07 08:15:45.939523267
#  File has been updated.
#  Successfully updated.
# variable set hash-table stats:
# Load=0/32=0%, Rehash=0, Collisions=0/0=0%

realclean: distclean
#  Phony target (prerequisite of .PHONY).
#  Implicit rule search has not been done.
#  File does not exist.
#  File has not been updated.
#  commands to execute (from `Makefile', line 158):
	rm -f tar.info*
	

# Not a target:
Makefile.in:
#  Implicit rule search has not been done.
#  Modification time never checked.
#  File has not been updated.

# Not a target:
rmt.h:
#  Implicit rule search has not been done.
#  Modification time never checked.
#  File has not been updated.

update.o: update.c
#  Implicit rule search has not been done.
#  Implicit/static pattern stem: `update'
#  Modification time never checked.
#  File has not been updated.

port.o: port.c
#  Implicit rule search has not been done.
#  Implicit/static pattern stem: `port'
#  Modification time never checked.
#  File has not been updated.

# Not a target:
configure:
#  Implicit rule search has not been done.
#  Modification time never checked.
#  File has not been updated.

getopt1.c:
#  Implicit rule search has not been done.
#  Modification time never checked.
#  File has not been updated.

# Not a target:
configure.in:
#  Implicit rule search has not been done.
#  Modification time never checked.
#  File has not been updated.

wildmat.c:
#  Implicit rule search has not been done.
#  Modification time never checked.
#  File has not been updated.

create.o: create.c
#  Implicit rule search has not been done.
#  Implicit/static pattern stem: `create'
#  Modification time never checked.
#  File has not been updated.

# Not a target:
COPYING:
#  Implicit rule search has not been done.
#  Modification time never checked.
#  File has not been updated.

# Not a target:
getdate.c:
#  Implicit rule search has not been done.
#  Modification time never checked.
#  File has not been updated.

# Not a target:
port.h:
#  Implicit rule search has not been done.
#  Modification time never checked.
#  File has not been updated.

# Not a target:
makefile.pc:
#  Implicit rule search has not been done.
#  Modification time never checked.
#  File has not been updated.

buffer.o: regex.h buffer.c
#  Implicit rule search has not been done.
#  Implicit/static pattern stem: `buffer'
#  Modification time never checked.
#  File has not been updated.
#  commands to execute (from `Makefile', line 143):
	# getdate.y has 8 shift/reduce conflicts.
	

shar: tar.c create.c extract.c buffer.c getoldopt.c update.c gnu.c mangle.c version.c list.c names.c diffarch.c port.c wildmat.c getopt.c getopt1.c regex.c getdate.y README COPYING ChangeLog Makefile.in makefile.pc configure configure.in tar.texinfo tar.info* texinfo.tex tar.h port.h open3.h getopt.h regex.h rmt.h rmt.c rtapelib.c alloca.c msd_dir.h msd_dir.c tcexparg.c level-0 level-1 backup-specs testpad.c
#  Implicit rule search has not been done.
#  Modification time never checked.
#  File has not been updated.
#  commands to execute (from `Makefile', line 161):
	shar $(SRCS) $(AUX) | compress \
	> tar-`sed -e '/version_string/!d' \
	-e 's/[^0-9.]*\([0-9.]*\).*/\1/' \
	-e q 
	version.c`.shar.Z
	

mangle.o: mangle.c
#  Implicit rule search has not been done.
#  Implicit/static pattern stem: `mangle'
#  Modification time never checked.
#  File has not been updated.

getoldopt.o: getoldopt.c
#  Implicit rule search has not been done.
#  Implicit/static pattern stem: `getoldopt'
#  Modification time never checked.
#  File has not been updated.

# Not a target:
texinfo.tex:
#  Implicit rule search has not been done.
#  Modification time never checked.
#  File has not been updated.

# Not a target:
level-0:
#  Implicit rule search has not been done.
#  Modification time never checked.
#  File has not been updated.

# Not a target:
rtapelib.c:
#  Implicit rule search has not been done.
#  Modification time never checked.
#  File has not been updated.

# Not a target:
level-1:
#  Implicit rule search has not been done.
#  Modification time never checked.
#  File has not been updated.

# Not a target:
testpad.c:
#  Implicit rule search has not been done.
#  Modification time never checked.
#  File has not been updated.

# Not a target:
alloca.c:
#  Implicit rule search has not been done.
#  Modification time never checked.
#  File has not been updated.

names.o: names.c
#  Implicit rule search has not been done.
#  Implicit/static pattern stem: `names'
#  Modification time never checked.
#  File has not been updated.

# Not a target:
tar.info*:
#  Implicit rule search has not been done.
#  Modification time never checked.
#  File has not been updated.

gnu.o: gnu.c
#  Implicit rule search has not been done.
#  Implicit/static pattern stem: `gnu'
#  Modification time never checked.
#  File has not been updated.

extract.c:
#  Implicit rule search has not been done.
#  Modification time never checked.
#  File has not been updated.

version.o: version.c
#  Implicit rule search has not been done.
#  Implicit/static pattern stem: `version'
#  Modification time never checked.
#  File has not been updated.

tar.info: tar.texinfo
#  Implicit rule search has not been done.
#  Modification time never checked.
#  File has not been updated.
#  commands to execute (from `Makefile', line 132):
	makeinfo tar.texinfo
	

# Not a target:
.DEFAULT:
#  Implicit rule search has not been done.
#  Modification time never checked.
#  File has not been updated.

diffarch.c:
#  Implicit rule search has not been done.
#  Modification time never checked.
#  File has not been updated.

# Not a target:
tcexparg.c:
#  Implicit rule search has not been done.
#  Modification time never checked.
#  File has not been updated.

update.c:
#  Implicit rule search has not been done.
#  Modification time never checked.
#  File has not been updated.

port.c:
#  Implicit rule search has not been done.
#  Modification time never checked.
#  File has not been updated.

clean:
#  Phony target (prerequisite of .PHONY).
#  Implicit rule search has not been done.
#  File does not exist.
#  File has not been updated.
#  commands to execute (from `Makefile', line 152):
	rm -f *.o tar rmt testpad testpad.h core
	

tar.o: regex.h tar.c
#  Implicit rule search has not been done.
#  Implicit/static pattern stem: `tar'
#  File does not exist.
#  File has been updated.
#  Needs to be updated (-q is set).
# automatic
# @ := tar.o
# automatic
# % := 
# automatic
# * := tar
# automatic
# + := regex.h tar.c
# automatic
# | := 
# automatic
# < := regex.h
# automatic
# ^ := regex.h tar.c
# automatic
# ? := regex.h tar.c
# variable set hash-table stats:
# Load=8/32=25%, Rehash=0, Collisions=1/10=10%
#  commands to execute (from `Makefile', line 143):
	# getdate.y has 8 shift/reduce conflicts.
	

tar: tar.o create.o extract.o buffer.o getoldopt.o update.o gnu.o mangle.o version.o list.o names.o diffarch.o port.o wildmat.o getopt.o getopt1.o regex.o getdate.o rtapelib.o
#  Implicit rule search has not been done.
#  File does not exist.
#  File has been updated.
#  Needs to be updated (-q is set).
# variable set hash-table stats:
# Load=0/32=0%, Rehash=0, Collisions=0/2=0%
#  commands to execute (from `Makefile', line 126):
	$(CC) $(LDFLAGS) -o $@ $(OBJS) $(LIBS)
	

list.o: list.c
#  Implicit rule search has not been done.
#  Implicit/static pattern stem: `list'
#  Modification time never checked.
#  File has not been updated.

dist: tar.c create.c extract.c buffer.c getoldopt.c update.c gnu.c mangle.c version.c list.c names.c diffarch.c port.c wildmat.c getopt.c getopt1.c regex.c getdate.y README COPYING ChangeLog Makefile.in makefile.pc configure configure.in tar.texinfo tar.info* texinfo.tex tar.h port.h open3.h getopt.h regex.h rmt.h rmt.c rtapelib.c alloca.c msd_dir.h msd_dir.c tcexparg.c level-0 level-1 backup-specs testpad.c
#  Implicit rule search has not been done.
#  Modification time never checked.
#  File has not been updated.
#  commands to execute (from `Makefile', line 168):
	echo tar-`sed \
	-e '/version_string/!d' \
	-e 's/[^0-9.]*\([0-9.]*\).*/\1/' \
	-e q 
	version.c` > .fname
	-rm -rf `cat .fname`
	mkdir `cat .fname`
	ln $(SRCS) $(AUX) `cat .fname`
	-rm -rf `cat .fname` .fname
	tar chZf `cat .fname`.tar.Z `cat .fname`
	

create.c:
#  Implicit rule search has not been done.
#  Modification time never checked.
#  File has not been updated.

getopt.o: getopt.c
#  Implicit rule search has not been done.
#  Implicit/static pattern stem: `getopt'
#  Modification time never checked.
#  File has not been updated.

regex.o: regex.h regex.c
#  Implicit rule search has not been done.
#  Implicit/static pattern stem: `regex'
#  Modification time never checked.
#  File has not been updated.
#  commands to execute (from `Makefile', line 143):
	# getdate.y has 8 shift/reduce conflicts.
	

buffer.c:
#  Implicit rule search has not been done.
#  Modification time never checked.
#  File has not been updated.

mangle.c:
#  Implicit rule search has not been done.
#  Modification time never checked.
#  File has not been updated.

getoldopt.c:
#  Implicit rule search has not been done.
#  Modification time never checked.
#  File has not been updated.

rmt: rmt.c
#  Implicit rule search has not been done.
#  Modification time never checked.
#  File has not been updated.
#  commands to execute (from `Makefile', line 129):
	$(CC) $(CFLAGS) $(LDFLAGS) -o $@ rmt.c
	

tar.zoo: tar.c create.c extract.c buffer.c getoldopt.c update.c gnu.c mangle.c version.c list.c names.c diffarch.c port.c wildmat.c getopt.c getopt1.c regex.c getdate.y README COPYING ChangeLog Makefile.in makefile.pc configure configure.in tar.texinfo tar.info* texinfo.tex tar.h port.h open3.h getopt.h regex.h rmt.h rmt.c rtapelib.c alloca.c msd_dir.h msd_dir.c tcexparg.c level-0 level-1 backup-specs testpad.c
#  Implicit rule search has not been done.
#  Modification time never checked.
#  File has not been updated.
#  commands to execute (from `Makefile', line 180):
	-rm -rf tmp.dir
	-mkdir tmp.dir
	-rm tar.zoo
	for X in $(SRCS) $(AUX) ; do \
	echo $$X ; \
	sed 's/$$/^M/' $$X \
	> tmp.dir/$$X ; done
	cd tmp.dir ; zoo aM ../tar.zoo *
	-rm -rf tmp.dir
	

names.c:
#  Implicit rule search has not been done.
#  Modification time never checked.
#  File has not been updated.

gnu.c:
#  Implicit rule search has not been done.
#  Modification time never checked.
#  File has not been updated.

# Not a target:
ChangeLog:
#  Implicit rule search has not been done.
#  Modification time never checked.
#  File has not been updated.

TAGS: tar.c create.c extract.c buffer.c getoldopt.c update.c gnu.c mangle.c version.c list.c names.c diffarch.c port.c wildmat.c getopt.c getopt1.c regex.c getdate.y
#  Implicit rule search has not been done.
#  Modification time never checked.
#  File has not been updated.
#  commands to execute (from `Makefile', line 149):
	etags $(SRCS)
	

version.c:
#  Implicit rule search has not been done.
#  Modification time never checked.
#  File has not been updated.

install: all
#  Implicit rule search has not been done.
#  Modification time never checked.
#  File has not been updated.
#  commands to execute (from `Makefile', line 135):
	$(INSTALL) tar $(bindir)/$(binprefix)tar
	-test ! -f rmt || $(INSTALL) rmt /etc/rmt
	$(INSTALLDATA) $(srcdir)/tar.info* $(infodir)
	

getopt1.o: getopt1.c
#  Implicit rule search has not been done.
#  Implicit/static pattern stem: `getopt1'
#  Modification time never checked.
#  File has not been updated.

# Not a target:
msd_dir.c:
#  Implicit rule search has not been done.
#  Modification time never checked.
#  File has not been updated.

# files hash-table stats:
# Load=82/1024=8%, Rehash=0, Collisions=14/299=5%
# VPATH Search Paths

# No `vpath' search paths.

# No general (`VPATH' variable) search path.

# # of strings in strcache: 1
# # of strcache buffers: 1
# strcache size: total = 4096 / max = 4096 / min = 4096 / avg = 4096
# strcache free: total = 4087 / max = 4087 / min = 4087 / avg = 4087

# Finished Make data base on Fri Dec  9 08:01:21 2011

