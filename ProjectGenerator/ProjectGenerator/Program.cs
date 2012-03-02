/*
|| NAME:	Program.cs
||
||
||
|| AUTHORS:	Frank Kotarski
||
|| SYNOPSIS:  Main entry point
||
|| MODULE:	Project generator
||
|| MODULE DATA: None
||		
|| NOTES:       
||
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace ProjectGenerator
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            //Initialize singleton generators
            //VSGen vcprojgen = new VSGen();
            //CmakeGen cgen = new CmakeGen();             
            //XmlReader reader = XmlReader.Instance;
            //KeyValuePair<Graph<string>,Dictionary<string,BuildElement> > buildinfo = new KeyValuePair<Graph<string>,Dictionary<string,BuildElement> >();
            //buildinfo = reader.readbuildfile("C:\\Users\\kotarf\\Documents\\Unmake\\Unmake\\ProjectGenerator\\Sample_Build_System_2\\cbf.xml");
            //vcprojgen.CreateTestSolution(@"C:\temp\FooBar", "Foo");
            //vcprojgen.CreateTestProject("C:\temp\FooBarFoo.sln", "myproj", VSGen.TestProjectType.Acceptance);
            //cgen.CreateCmakeList("C:\\Users\\kotarf\\Documents\\Unmake\\Unmake\\ProjectGenerator\\Sample_Build_System_2\\", "clists.txt", buildinfo.Value, buildinfo.Key);
            
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
             
        }
    }
}
