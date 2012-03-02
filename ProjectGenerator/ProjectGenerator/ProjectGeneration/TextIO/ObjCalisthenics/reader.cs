using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace ProjectGenerator
{
    public class reader
    {
        public KeyValuePair<Graph<string>, Dictionary<string, BuildElement>> readbuildfile(string path)
        {
            XmlTextReader reader = new XmlTextReader(path);
            Graph<string> graph = new Graph<string>();
            Dictionary<string, BuildElement> elements = new Dictionary<string, BuildElement>();
            string name = "";
            string extension = "";
            string instruction = "";
            string source = "";
            string target = "";

            readerhelper.read(reader, source, target, name, extension, instruction, graph);

            return new KeyValuePair<Graph<string>, Dictionary<string, BuildElement>>(graph, elements);
        }

    }

}
