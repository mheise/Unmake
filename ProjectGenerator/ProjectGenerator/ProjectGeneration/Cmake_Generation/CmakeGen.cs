using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProjectGenerator.InstructionParsing;

namespace ProjectGenerator
{
    class CmakeGen
    {
        public delegate void Resolve(BuildElement elem);
        public void CreateCmakeList(string fullyQualifiedPath, string unqualifiedName, Dictionary<string,BuildElement> buildsys, Graph<string> buildgraph)
        {
            /*NOTE: This is a totally hacked solution to try and have a demo for the presentation. It will get much cleaner over time.  */
            TextWriter txtwriter = new TextWriter();
            txtwriter.createfile(fullyQualifiedPath + unqualifiedName);
            BuildElement tmp;
            foreach(KeyValuePair<string,BuildElement> elem in buildsys)
            {
                CustomAction action = CustomActionsBare.determineAction(elem.Value);
                if(action == CustomAction.ConvertToProject)
                {
                    tmp = elem.Value;
                }
                    

                //System.Console.WriteLine("TEST: "+elem.Key);
               // System.Console.WriteLine(ProjectGenerator.InstructionParsing.CustomActionsBare.determineAction(elem.Value));
               
            
            }
        }
    }
}
