/*
|| NAME:	BuildElement.cs
||
||
||
|| AUTHORS:	Frank Kotarski
||
|| SYNOPSIS:    Encapsulates the information found in a common build file. A "build element" may be a source file, an exe, etc.
||
|| MODULE:	Project generator
||
|| MODULE DATA: 
||		
|| NOTES:       
||
*/

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
        private string buildinstruction; //the literal instruction associated with building the file.
        private List<string> prebuildinstructions; //the literal prebuild instructions associated with building the file.
        private List<string> prelinkinstructions; //the literal prelink instructions associated with building the file.
        private List<string> postbuildinstructions; //the literal postbuild instructions associated with building the file.
        private List<BuildElement> dependencies;

        public BuildElement(string vname, string vext, string vinstr)
        {
            this.name = vname;
            this.extension = vext;
            this.buildinstruction = vinstr;
        }
        public override bool Equals(System.Object obj)
        {
            // If parameter is null return false.
            if (obj == null)
            {
                return false;
            }

            // If parameter cannot be cast to Point return false.
            BuildElement p = obj as BuildElement;
            if ((System.Object)p == null)
            {
                return false;
            }

            return name == p.name;
        }
        //Filename
        //Extension
        //Type
        //Instructions to create
        //prebuild, postbuild, prelink
    }
}
