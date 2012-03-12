using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProjectGenerator.InstructionParsing;
using System.IO;

namespace ProjectGenerator
{
    class CmakeGen
    {
        public delegate void Resolve(BuildElement elem);
        public void CreateCmakeList(string fullyQualifiedPath, string unqualifiedName, Dictionary<string,BuildElement> buildsys, Graph<string> buildgraph)
        {
            /*NOTE: This is a totally hacked solution to try and have a demo for the presentation. It will get much cleaner over time.  */
            string fullyQualifiedFileName = fullyQualifiedPath + "\\" + unqualifiedName;
            TextWriter txtwriter = new TextWriter();
            System.IO.StreamWriter file = new System.IO.StreamWriter(fullyQualifiedFileName);
            BuildElement elem;
            string objects = "";
            string projectname = "";
            //brute forced just for the demo

            //this is not quite as brute forced, but pretty close
            foreach(KeyValuePair<string,BuildElement> keyval in buildsys)
            {
                elem = keyval.Value;
                CustomAction action = CustomActionsBare.determineAction(elem);
                if(action == CustomAction.ConvertToProject)
                {
                    projectname = elem.name;
                    for (int i = 0; i < elem.dependencies.Count; i++)
                    {
                        objects += elem.dependencies[i].Replace(".o", ".cpp") + " ";
                    }
                }

                //System.Console.WriteLine("TEST: "+elem.Key);
               // System.Console.WriteLine(ProjectGenerator.InstructionParsing.CustomActionsBare.determineAction(elem.Value));
             
            }
            file.WriteLine("cmake_minimum_required (VERSION 2.6)");
            file.WriteLine("project (" + projectname + ")");
            file.WriteLine("");
            file.WriteLine("add_executable("+projectname + " " + objects+")");
            file.Close();
        }
    }
}
