CC=g++
CFLAGS=-Wall -Wextra -O2

all: hello

hello: main.o factorial.o hello.o
	$(CC) $(CFLAGS) main.o factorial.o hello.o -o hello

main.o: main.cpp
	$(CC) $(CFLAGS) -c main.cpp

factorial.o: factorial.cpp
	$(CC) $(CFLAGS) -c factorial.cpp

hello.o: hello.cpp
	$(CC) $(CFLAGS) -c hello.cpp

clean:
	rm -rf *o hello
