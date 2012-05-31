/*
|| NAME:	XmlValidator.cs
||
||
||
|| AUTHORS:	Frank Kotarski
||
|| SYNOPSIS:  Validate an XML file against a given schema
||
|| MODULE:	Project generator
||
|| MODULE DATA: XML files
||		
|| NOTES:       courtesy of aharon @ StackOverflow  http://stackoverflow.com/questions/4584080/schema-validation-xml
 *              and kubben @ Code Project http://www.codeproject.com/Articles/10444/Simple-code-to-validate-an-XML-file-against-a-sche
||
*/


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System.Xml;
using System.Xml.Schema;

namespace ProjectGenerator
{
    //Singleton- one XmlReader per application. This fits because we don't ever need to have more than 1 reader. Access the reader through the use of
    // "instance" to get the available instance of the reader.
    public class XmlValidator
    {
        private static XmlValidator validatorinstance;
        private bool m_Success = false;
        public static XmlValidator Instance
        {
            get
            {
                if (validatorinstance == null)
                {
                    validatorinstance = new XmlValidator();
                }
                return validatorinstance;
            }
        }

        //C#
        public bool validateXml(String xmlfile, string schemafile)
        {
            //First we create the xmltextreader
            XmlTextReader xmlr = new XmlTextReader(infile);
            XmlSchema schema = getSchema(schemafile);
            //We pass the xmltextreader into the xmlvalidatingreader
            //This will validate the xml doc with the schema file
            //NOTE the xml file it self points to the schema file
           
            XmlValidatingReader xmlvread = new XmlValidatingReader(xmlr);

            // Set the validation event handler
            xmlvread.ValidationEventHandler +=
                new ValidationEventHandler(ValidationCallBack);
            m_Success = true; //make sure to reset the success var

            // Read XML data
            while (xmlvread.Read()) { }

            //Close the reader.
            xmlvread.Close();

            //The validationeventhandler is the only thing that would set 
            //m_Success to false
            return m_Success;
        }

        private void ValidationCallBack(Object sender, ValidationEventArgs args)
        {
            //Display the validation error.  This is only called on error
            m_Success = false; //Validation failed
            Console.WriteLine("Validation error: " + args.Message);

        }

        private XmlSchema getSchema(String infilename)
        {
            //this function will validate the schema file (xsd)
            XmlSchema myschema;
            m_Success = true; //make sure to reset the success var
            StreamReader sr = new StreamReader(infilename);
            try
            {
                myschema = XmlSchema.Read(sr,
                    new ValidationEventHandler(ValidationCallBack));
                //This compile statement is what ususally catches the errors
                myschema.Compile(new ValidationEventHandler(ValidationCallBack));
            }
            finally
            {
                sr.Close();
            }
            return myschema;
        }
        /*
        private void ValidationCallBack(object sender, ValidationEventArgs e)
        { throw new Exception(); }

        public bool validate(string sxml, string xschem)
        {
            try
            {
                XmlDocument xmld = new XmlDocument();
                xmld.LoadXml(sxml);
                xmld.Schemas.Add(null, xschem);
                xmld.Validate(ValidationCallBack);
                return true;
            }
            catch
            {
                return false;
            }
        }
         * */
    }
}
