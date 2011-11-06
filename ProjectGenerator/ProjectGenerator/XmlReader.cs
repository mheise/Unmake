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
        public Graph<BuildElement> readbuildfile(string absfilepath)
        {
            XmlTextReader reader = new XmlTextReader(absfilepath);
            Graph<BuildElement> buildgraph = new Graph<BuildElement>();
            string name;
            string extension;
            string instruction;

            BuildElement element;
            try
            {
                while (reader.Read())
                {
                    switch (reader.NodeType)
                    {
                        case XmlNodeType.Element: // The node is an element.
                            Console.Write("<" + reader.Name);
                            if (reader.Name=="node")
                            {
                                name = reader.Value;
                                extension = System.IO.Path.GetExtension(reader.Value);                
                            }
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
            return buildgraph;
        }

    }

}
