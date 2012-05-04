/*
|| NAME:	XmlReader.cs
||
||
||
|| AUTHORS:	Frank Kotarski
||
|| SYNOPSIS:  Read XML file and place information into Graph
||
|| MODULE:	Project generator
||
|| MODULE DATA: XML files
||		
|| NOTES:       
||
*/


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace ProjectGenerator
{
    //Singleton- one XmlReader per application. This fits because we don't ever need to have more than 1 reader. Access the reader through the use of
    // "instance" to get the available instance of the reader.
    public class XmlReader
    {
        private static XmlReader instance;

        public static XmlReader Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new XmlReader();
                }
                return instance;
            }
        }

        public void read(string absfilepath)
        {
            XmlTextReader reader = new XmlTextReader(absfilepath);
            
            try
            {
                while (reader.Read())
                {
                    switch (reader.NodeType)
                    {
                        case XmlNodeType.Element: // The node is an element.
                            Console.Write("<" + reader.Name);

                            while (reader.MoveToNextAttribute()) // Read the attributes.
                                Console.Write(" " + reader.Name + "='" + reader.Value + "'");
                            Console.WriteLine(">");
                            break;
                        case XmlNodeType.Text: //Display the text in each element.
                            Console.WriteLine(reader.Value);
                            break;
                        case XmlNodeType.EndElement: //Display the end of the element.
                            Console.Write("</" + reader.Name);
                            Console.WriteLine(">");
                            break;
                    }
                }
            }

            catch (Exception e)
            {
                Console.WriteLine("Exception in XmlReader");
            }
        }
        public KeyValuePair<Graph<string>,Dictionary<string,BuildElement> > readbuildfile(string absfilepath)
        {
            XmlTextReader reader = new XmlTextReader(absfilepath);
            Graph<string> buildgraph = new Graph<string>();
            Dictionary<string, BuildElement> buildelements = new Dictionary<string, BuildElement>();
            string name = "";
            string extension = "";
            string instruction = "";
            string source = "";
            string target = "";
            bool dep = false;
            bool command = false;
            BuildElement element = null;
            try
            {
                while (reader.Read())
                {

                    switch (reader.NodeType)
                    {
                        case XmlNodeType.Element: // The node is an element.
                            Console.Write("<" + reader.Name);
                            if (reader.Name == "file")
                            {
                                element = new BuildElement();
                                while (reader.MoveToNextAttribute())// Read the attributes.
                                {
                                    if (reader.Name == "name")
                                    {
                                        name = reader.Value;
                                        source = reader.Value;
                                        extension = System.IO.Path.GetExtension(reader.Value);
                                        element.setName(name);
                                        element.setExtension(extension);
                                        buildelements.Add(name, element);
                                        //buildgraph.AddNode(name);
                                    }
                                    Console.Write(" " + reader.Name + "='" + reader.Value + "'");
                                }
                            }
                            if (reader.Name == "dep")
                            {
                                dep = true;
                            }
                            if (reader.Name == "command")
                            {
                                command = true;
                            }                         
    
                            
                            Console.WriteLine(">");
                            break;
                        case XmlNodeType.Text: //Display the text in each element.
                            if (dep)
                            {
                                target = reader.Value;
                                if (!buildgraph.Contains(source))
                                    buildgraph.AddNode(source);
                                if (!buildgraph.Contains(target))
                                    buildgraph.AddNode(target);

                                buildgraph.AddDirectedEdge((GraphNode<string>)buildgraph.GetNode(source), (GraphNode<string>)buildgraph.GetNode(target), 1);
                                buildelements[source].dependencies.Add(target);
                                target = "";
                                dep = false;
                            }
                            if (command)
                            {
                                buildelements[name].setBuildInstruction(reader.Value);
                                command = false;
                            }
                            Console.WriteLine(reader.Value);
                            break;
                        case XmlNodeType.EndElement: //Display the end of the element.
                            if (reader.Name == "file")
                            {
                                element = null;
                            }
                            Console.Write("</" + reader.Name);
                            Console.WriteLine(">");
                            break;
                    }
                }
            }

            catch (Exception e)
            {
                Console.WriteLine("Exception in XmlReader: "+e.Message);
            }
            return new KeyValuePair<Graph<string>, Dictionary<string, BuildElement>>(buildgraph, buildelements);
        }

    }

}
