using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace ProjectGenerator
{
    class XmlReader
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
                        //trycatch

                        case XmlNodeType.Element: // The node is an element.
                            Console.Write("<" + reader.Name);
                            Console.WriteLine(">");
                            break;
                        case XmlNodeType.Text: //Display the text in each element.
                            Console.WriteLine(reader.Value);
                            break;
                        case XmlNodeType.EndElement: //Display the end of the element.
                            Console.Write("</" + reader.Name);
                            Console.WriteLine(">");
                            break;
                        case XmlNodeType.Attribute: // Display attribute text
                            Console.Write("Attribute!");
                            Console.Write(reader.Value);
                            break;
                    }
                }
                Console.ReadLine();
            }

            catch (Exception e)
            {
                Console.WriteLine("Exception in XmlReader");
            }
        }
      }
    
}
