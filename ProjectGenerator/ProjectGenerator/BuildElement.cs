using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProjectGenerator
{
    public class BuildElement
    {
        private string name;       //the unqualified name of the file with extension (ie main.cpp)
        private string extension; // the extension of the file  //is the file generated or static
        private string instruction; //the literal instruction associated with building the file.
        public BuildElement(string vname, string vext, string vinstr)
        {
            this.name = vname;
            this.extension = vext;
            this.instruction = vinstr;
        }
        //Filename
        //Extension
        //Type
        //Instructions to create
        //prebuild, postbuild, prelink
    }
}
