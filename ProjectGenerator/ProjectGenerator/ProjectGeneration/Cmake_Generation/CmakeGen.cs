using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public delegate double Delegate_Prod(int a, int b);
namespace ProjectGenerator.ProjectGeneration.Cmake_Generation
{
    class CmakeGen
    {
        public void CreateCmakeList(string fullyQualifiedPath, string unqualifiedName, Graph<BuildElement> buildsys)
        {
            TextWriter txtwriter = new TextWriter();
            foreach(BuildElement elem in buildsys)
            {


            }
        }
    }
}
