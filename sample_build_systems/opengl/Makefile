LIBPATH += 
-L"/System/Library/Frameworks/OpenGL.framework/Libraries"

FRAMEWORK = -framework GLUT
FRAMEWORK += -framework OpenGL 

COMPILERFLAGS = -Wall 
CC = g++ 
CFLAGS = $(COMPILERFLAGS) 
LIBRARIES = -lGL -lGLU -lm -lobjc -lstdc++ 

OBJECTS = GlutExample.o 
All: GlutExample 

GlutExample: GlutExample.o $(OBJECTS) 
    $(CC) $(FRAMEWORK) $(CFLAGS) -o $@ $(LIBPATH) $(OBJECTS)

$(LIBRARIES) 
       ./GlutExample