using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using Microsoft.VisualStudio.VCProjectEngine;
using EnvDTE80;
namespace ProjectGenerator
{
    class VSSolution
    {
        private Solution2 vcsol;
        private String absfilepath;
        private String relfilepath;
        const String absdevenvpath = "devenv.exe";

        public VSSolution(Solution2 solution, String afilepath, String rfilepath)
        {
            vcsol = solution;
            absfilepath = afilepath;
            relfilepath = rfilepath;
        }

        protected void devenvLaunchSolution()
        {
            Process process = new Process();

            // Stop the process from opening a new window
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.CreateNoWindow = true;

            // Setup executable and parameters
            process.StartInfo.FileName = absdevenvpath;
            process.StartInfo.Arguments = absfilepath;

            // Go
            process.Start();
        }
    }
}
