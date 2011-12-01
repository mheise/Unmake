using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace ProjectGenerator
{
    public class XmlReader
    {
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

            try
            {
                while (reader.Read())
                {

                    switch (reader.NodeType)
                    {
                        case XmlNodeType.Element: // The node is an element.
                            Console.Write("<" + reader.Name);
                            if (reader.Name == "node")
                            {
                                while (reader.MoveToNextAttribute())// Read the attributes.
                                {
                                    if (reader.Name == "name")
                                    {
                                        name = reader.Value;
                                        extension = System.IO.Path.GetExtension(reader.Value);
                                    }
                                    Console.Write(" " + reader.Name + "='" + reader.Value + "'");
                                }
                            }
                            if (reader.Name == "edge")
                            {
                                while (reader.MoveToNextAttribute())// Read the attributes.
                                {
                                    if (reader.Name == "source")
                                    {
                                        source = reader.Value;
                                    }
                                    else if (reader.Name == "target")
                                    {
                                        target = reader.Value;
                                    }

                                    //Console.Write(" " + reader.Name + "='" + reader.Value + "'");
                                }
                                buildgraph.AddDirectedEdge(new GraphNode<string>(source), new GraphNode<string>(target), 1);
                                source = "";
                                target = "";
                            }
    
                            
                            Console.WriteLine(">");
                            break;
                        case XmlNodeType.Text: //Display the text in each element.
                            instruction = reader.Value;
                            Console.WriteLine(reader.Value);
                            break;
                        case XmlNodeType.EndElement: //Display the end of the element.
                            if (reader.Name == "node")
                            {
                                buildgraph.AddNode(name);
                                buildelements.Add(name, new BuildElement(name, extension, instruction));
                                name = "";
                                extension = "";
                                instruction = "";
                            }
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
            return new KeyValuePair<Graph<string>, Dictionary<string, BuildElement>>(buildgraph, buildelements);
        }

    }

}
