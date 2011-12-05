/*
|| NAME:	VSGen.cs
||
||
||
|| AUTHORS:	Frank Kotarski
||
|| SYNOPSIS:    Using EnvDTE and COM wrappers, launch devenv to create a solution. Add projects to solution too.
||
|| MODULE:	Project generator
||
|| MODULE DATA: 
||		
|| NOTES:       
||
*/


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.VCProjectEngine;
using EnvDTE100;
using EnvDTE80;
using EnvDTE;

namespace ProjectGenerator
{
    class VSGen
    {
        public enum TestProjectType
        {
            Unit,
            Acceptance,
            Integration
        }
        public void CreateSolution(string fullyQualifiedPath, string unqualifiedName)
        {
            string visualStudioProgID = "VisualStudio.Solution.10.0";
            Type solutionObjectType = System.Type.GetTypeFromProgID(visualStudioProgID, true);
            object obj = System.Activator.CreateInstance(solutionObjectType, true);
            EnvDTE100.Solution4 solutionObject = (EnvDTE100.Solution4)obj;
            solutionObject.Create(fullyQualifiedPath, unqualifiedName);
            solutionObject.SaveAs(fullyQualifiedPath + unqualifiedName + ".sln");
        }

        public void CreateTestSolution(string fullyQualifiedPath, string unqualifiedName)
        {
            EnvDTE.Solution soln = System.Activator.CreateInstance(Type.GetTypeFromProgID("VisualStudio.Solution")) as EnvDTE.Solution;
            soln.DTE.MainWindow.Visible = true;
            EnvDTE80.Solution2 soln2 = soln as EnvDTE80.Solution2;
            soln2.Create(fullyQualifiedPath, unqualifiedName);
            string csTemplatePath = soln2.GetProjectTemplate("ConsoleApplication.zip", "CSharp");
            soln.AddFromTemplate(csTemplatePath, fullyQualifiedPath, unqualifiedName, false);
            int x = soln.AddIns.Count;
            soln2.SaveAs(fullyQualifiedPath + unqualifiedName + ".sln");
        }
        
        public void CreateTestProject(string fullyQualifiedSolutionFileName, string projectName, TestProjectType testProjectType)
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

            System.Type vsType = System.Type.GetTypeFromProgID("VisualStudio.DTE.10.0");
            Object vs = System.Activator.CreateInstance(vsType, true);

            EnvDTE80.DTE2 dte8Obj = (EnvDTE80.DTE2)vs;

            EnvDTE80.Solution2 vhaSolution = (EnvDTE80.Solution2)dte8Obj.Solution;
            //EnvDTE100.Solution4 vhaSolution = (EnvDTE100.Solution4)dte8Obj.Solution
            vhaSolution.Open(fullyQualifiedSolutionFileName);

            //TODO: Externalize company name
            string cmpnyName = "MCAF";
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

            string csTemplatePath = vhaSolution.GetProjectTemplate("ConsoleApplication.zip", "CSharp");

            vhaSolution.AddFromTemplate(csTemplatePath, testDirName, testProjectName + ".proj", false);

            //EnvDTE.Project vhaTestProj = vhaSolution.AddFromTemplate(testTemplateLocation, testDirName, testProjectName + ".proj", false);
            //vhaTestProj.Save(String.Format("{0}\\{1}.proj", testDirName, testProjectName));
            vhaSolution.SaveAs(fullyQualifiedSolutionFileName);
        }

        public static DTE2 GetActiveIDE()
        {
            // Get an instance of the currently running Visual Studio IDE.
            DTE2 dte2;
            dte2 = (DTE2)System.Runtime.InteropServices.Marshal.GetActiveObject("VisualStudio.DTE.10.0");
            return dte2;
        }


    }
}
