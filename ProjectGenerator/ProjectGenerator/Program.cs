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
            VSGen vcprojgen = new VSGen();
            XmlReader reader = new XmlReader();
            reader.readbuildfile("C:\\Users\\kotarf\\Documents\\Unmake\\Unmake\\ProjectGenerator\\Sample_Build_System\\cbf.xml");
            vcprojgen.CreateTestSolution(@"C:\temp\FooBar", "Foo");
            vcprojgen.CreateTestProject(@"C:\temp\FooBarFoo.sln", "myproj", VSGen.TestProjectType.Acceptance);
           
            /*
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
             * */
        }
    }
}
