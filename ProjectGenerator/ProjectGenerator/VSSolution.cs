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
        public enum TestProjectType
        {
            Unit,
            Acceptance,
            Integration
        }

        public static void CreateTestProject(string fullyQualifiedSolutionFileName, string projectName, TestProjectType testProjectType)
        {
            #region Argument Validation
            if (String.IsNullOrEmpty(fullyQualifiedSolutionFileName) || String.IsNullOrEmpty(fullyQualifiedSolutionFileName.Trim()))
            {
                throw new ArgumentNullException("fullyQualifiedSolutionFileName", "The solution file location is required.");
            }

            if (String.IsNullOrEmpty(projectName) || String.IsNullOrEmpty(projectName.Trim()))
            {
                throw new ArgumentNullException("projectName", "The project name is required.");
            }

            if (!System.IO.File.Exists(fullyQualifiedSolutionFileName))
            {
                throw new ArgumentException(String.Format("The file {0} specified does not exist.", fullyQualifiedSolutionFileName));
            }

            //if (testProjectType == null) testProjectType = TestProjectType.Unit;
            #endregion

            System.Type vsType = System.Type.GetTypeFromProgID("VisualStudio.DTE.8.0");
            Object vs = System.Activator.CreateInstance(vsType, true);
            EnvDTE80.DTE2 dte8Obj = (EnvDTE80.DTE2)vs;

            Solution2 vhaSolution = (Solution2)dte8Obj.Solution;
            vhaSolution.Open(fullyQualifiedSolutionFileName);

            //TODO: Externalize company name
            string cmpnyName = "Vha";
            string testProjectName = String.Format("{0}.{1}.{2}{3}", cmpnyName, projectName, testProjectType.ToString(), "Test");
            string testTemplateLocation = vhaSolution.GetProjectTemplate("TestProject.zip", "CSharp");
            System.IO.FileInfo rootSolutionFolder = new System.IO.FileInfo(fullyQualifiedSolutionFileName);

            //TODO: Externalize test directory name
            string testDirName = String.Format("{0}\\{1}\\{2}\\{3}", rootSolutionFolder.DirectoryName, "test", testProjectType.ToString(), testProjectName);

            if (!System.IO.Directory.Exists(testDirName))
            {
                //may throw an exception if the dir can't be created...
                System.IO.Directory.CreateDirectory(testDirName);
            }


            EnvDTE.Project vhaTestProj = vhaSolution.AddFromTemplate(testTemplateLocation, testDirName, testProjectName + ".proj", false);
            vhaTestProj.Save(String.Format("{0}\\{1}.proj", testDirName, testProjectName));

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
