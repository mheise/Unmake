/*
|| NAME:	TextWriter.cs
||
||
||
|| AUTHORS:	Frank Kotarski
||
|| SYNOPSIS:    Write text to a file
||
|| MODULE:	Project generator
||
|| MODULE DATA: Text files
||		
|| NOTES:       
||
*/


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProjectGenerator
{
    class TextWriter
    {
        static void writetest()
        {

            // These examples assume a "C:\Users\Public\TestFolder" folder on your machine.
            // You can modify the path if necessary.

            // Example #1: Write an array of strings to a file.
            // Create a string array that consists of three lines.
            string[] lines = {"First line", "Second line", "Third line"};
            System.IO.File.WriteAllLines(@"C:\Users\Public\TestFolder\WriteLines.txt", lines);


            // Example #2: Write one string to a text file.
            string text = "A class is the most powerful data type in C#. Like structures, " +
                           "a class defines the data and behavior of the data type. ";
            System.IO.File.WriteAllText(@"C:\Users\Public\TestFolder\WriteText.txt", text);

            // Example #3: Write only some strings in an array to a file.
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(@"C:\Users\Public\TestFolder\WriteLines2.txt"))
            {
                foreach (string line in lines)
                {
                    if (line.Contains("Second") == false)
                    {
                        file.WriteLine(line);
                    }
                }
            }

            // Example #4: Append new text to an existing file
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(@"C:\Users\Public\TestFolder\WriteLines2.txt", true))
            {
                file.WriteLine("Fourth line");
            }  
        }
        public void appendline(string absfilepath, string line, System.IO.StreamWriter file)
        {
            if(file!=null)
                file.WriteLine(line);  
        }
        public System.IO.StreamWriter createfile(string absfilepath)
        {
            if (!System.IO.File.Exists(absfilepath))
            {
                // Create a file to write to.
                System.IO.StreamWriter sw = System.IO.File.CreateText(absfilepath);
                return sw;
            }
            return null;
        }
    }

}
