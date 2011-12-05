/*
|| NAME:	VSSolution.cs
||
||
||
|| AUTHORS:	Frank Kotarski
||
|| SYNOPSIS:    Wrap around a COM visual studio Project. 
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
using Microsoft.VisualStudio.VCProjectEngine;

namespace ProjectGenerator
{
    class VSProject
    {
        private VCProject vcproj;
        private String absfilepath;
        private String relfilepath;

        public VSProject(VCProject project, String afilepath, String rfilepath)
        {
            vcproj = project;
            absfilepath = afilepath;
            relfilepath = rfilepath;
        }
    }
}
