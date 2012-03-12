using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace ProjectGenerator.InstructionParsing
{
    /* These are actions that may be taken on a BuildElement, depending on its targets, dependencies, and instructions.
     * 
     * */
    public enum CustomAction
    {
        Unknown,                     //Unrecognized instructions
        ExecAsBatchFile,             //Create as a batch file and execute immediately.
        ConvertToProject,            //Convert the custom rule to VS2008 project using our generator. It must then be placed correctly in the dependency tree.
        ModifyComponent,             //Add the relevant files to a pre-existing component.
        AttachAsPreBuildEvent,       //Attach to a pre-existing component as a prebuild event.
        AttachAsPostBuildEvent,      //Attach to a pre-existing component as a postbuild event.
        AttachAsPreLinkEvent,        //Attach to a pre-existing component as a prelink event.
        AttachAsCustomBuildStep,     //Attach to a pre-existing project as a custom build step.
        Ignore,                      //Do not take action
    }

    /*
     * Given:
     *   A CustomComponent
     * Output:
     *   Based on targets,
     *   An appropriate action to take
     *  
     * */
    class CustomActionsBare
    {
        public static CustomAction determineAction(BuildElement rule)
        {
            CustomAction ret = CustomAction.Unknown;
            string noExtension = "";
            string extension = "";
            foreach (string target in rule.dependencies)
            {
                extension = Path.GetExtension(target).Replace(".", "");
                noExtension = Path.GetFileNameWithoutExtension(target);

                if (string.IsNullOrWhiteSpace(target))
                    continue;
                if (target.ToLower().Contains("clean"))
                    return CustomAction.Ignore;
                if (target.Contains("stdafx"))
                    return CustomAction.Ignore;

                if (extension.Equals(""))
                {
                    //Phony target
                    return CustomAction.AttachAsPostBuildEvent;
                }

                if (rule.dependencies.Count > 0)
                {
                    if (rule.dependencies[0].Equals("stdafx.cpp") || rule.dependencies[0].Equals("stdafx.h"))
                    {
                        return CustomAction.Ignore;
                    }
                }
                //this isn't exactly correct
                if (extension.Equals("o"))
                {
                    ret = CustomAction.ConvertToProject;
                }

                if (extension.Equals("cpp"))
                {
                    ret = CustomAction.ModifyComponent;
                }
                if (extension.Equals("dll"))
                {
                    ret = CustomAction.ConvertToProject;
                }
                else if (extension.Equals("exe"))
                {
                    ret = CustomAction.ConvertToProject;
                }
                else if (extension.Equals("ocx"))
                {
                    ret = CustomAction.ConvertToProject;
                }
                else if (extension.Equals("hlp"))
                {
                    ret = CustomAction.Ignore;
                }
                else if (extension.Equals("cab"))
                {
                    ret = CustomAction.Unknown;
                }
                else if (extension.Equals("obj"))
                {
                    ret = CustomAction.ModifyComponent;
                }
                else if (extension.Equals("h"))
                {
                    ret = CustomAction.AttachAsPreBuildEvent;
                }
                else if (extension.Equals("c"))
                {
                    ret = CustomAction.AttachAsPreBuildEvent;
                }
                else if (extension.Equals("lib"))
                {
                    ret = CustomAction.ConvertToProject;
                }
                else if (extension.Equals("dat"))
                {
                    ret = CustomAction.Unknown;
                }
                else if (extension.Equals("idt"))
                {
                    ret = CustomAction.Unknown;
                }
                else if (extension.Equals("res"))
                {
                    ret = CustomAction.ModifyComponent;
                }
                else if (extension.Equals("tlb"))
                {
                    //handled during makefile processing
                    ret = CustomAction.Ignore;
                }
                else if (extension.Equals("def"))
                {
                    ret = CustomAction.Unknown;
                }
            }
            return ret;
        }
    }
}
