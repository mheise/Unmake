using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace ProjectGenerator
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            XmlReader reader = new XmlReader();
            reader.read("C:\\Users\\kotarf\\Documents\\Unmake\\Unmake\\ProjectGenerator\\Sample_Build_System\\cbf.xml");
            /*
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
             * */
        }
    }
}
