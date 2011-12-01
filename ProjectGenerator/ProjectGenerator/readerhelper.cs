using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
namespace ProjectGenerator
{
    class readerhelper
    {
        static public void read(XmlTextReader reader, string source, string target, string name, string extension, string instruction, Graph<string> graph)
        {
            try{
                while (reader.Read()){
                    switch (reader.NodeType){
                        case XmlNodeType.Element: // The node is an element.
                            Console.Write("<" + reader.Name);
                            if (reader.Name == "node")
                            {
                                while (reader.MoveToNextAttribute()){// Read the attributes.
                                    if (reader.Name == "name"){
                                        name = reader.Value;
                                        extension = System.IO.Path.GetExtension(reader.Value);
                                    }

                                }
                            }
                            if (reader.Name == "edge"){
                                while (reader.MoveToNextAttribute()){
                                    if (reader.Name == "source"){
                                        source = reader.Value;
                                    }
                                    else if (reader.Name == "target"){
                                        target = reader.Value;
                                    }
                                }
                                graph.AddDirectedEdge(new GraphNode<string>(source), new GraphNode<string>(target), 1);
                                source = "";
                                target = "";
                            }

                            break;
                    }
                }
            }
            catch (Exception e) { }
            return new KeyValuePair<Graph<string>, Dictionary<string, BuildElement>>(graph, elements);
        }
    }
}
