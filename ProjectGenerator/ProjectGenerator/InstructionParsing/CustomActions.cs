/*
|| NAME:	CustomActions.cs
||
||
||
|| AUTHORS:	Frank Kotarski
||
|| SYNOPSIS:    A library of functions for dealing with Makefile rules. Analyzes the extension and the instructions
||              to determine the best course of action (ie generate a new project, attach to an old project, ignore).
||
|| MODULE:	Project generator
||
|| MODULE DATA: 
||		
|| NOTES:       In Progress, Not Being Built
||
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;
using MakefileParser;

namespace ProjectGenerator
{
    /* This code is IN PROGRESS and is BEING TESTED...
     * 
     * */

    public static class CustomActions
    {
        /* These are actions that may be taken on a CustomComponent, depending on the targets, dependencies, and instructions.
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
        public static CustomAction determineAction(CustomComponent rule)
        {
            CustomAction ret = CustomAction.Unknown;
            string noExtension = "";
            string extension = "";
            foreach (string target in rule.CustomTargets)
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

                if (rule.CustomDependencies.Count > 0)
                {
                    if (rule.CustomDependencies[0].Equals("stdafx.cpp") || rule.CustomDependencies[0].Equals("stdafx.h"))
                    {
                        return CustomAction.Ignore;
                    }
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

        /*
         * Given:
         *   A CustomComponent that has generic instructions with no associated target.
         * Output:
         *   Create & execute a batch file with these instructions.
         *  
         * */
        public static void ExecAsBatchFile(CustomComponent rule)
        {
            try
            {
                using (FileStream fs = new FileStream("ExecInstr.bat", FileMode.Create))
                {
                    using (StreamWriter sw = new StreamWriter(fs))
                    {
                        //Write instructions to file
                        foreach (string instr in rule.CustomInstructions)
                        {
                            sw.WriteLine(instr);
                        }
                    }

                    Process.Start("ExecInstr.bat");
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
            }
        }

        /*
         * Given:
         *   A Link instruction- a single instruction that has been parsed before, and a CustomComponent that has this link instruction
         * Output:
         *   A Component that can be used to generate a project for this .dll
         *  Note that we want to point to source files (.c, .h) instead of linking against things that are not in our current project.
         * */
        public static Component resolveLinkInstruction(LinkInstruction linkInstr, CustomComponent rule, Makefile mkfile)
        {
            Component component = new Component(mkfile);
            string componentName = "";
            componentName = rule.Name;
            component.setName(componentName);
            linkInstr.convertToLink(mkfile, component);

            //If the link instruction shares objs with the makefile's, then use its cflags. Otherwise, we assume that the link instruction has an accompanying compile instruction somewhere
            if (rule.contextSensitiveProps.Contains("OBJ_LIST_C_OBJS"))
            {
                component.CompilerFlags = new CompileFlags(mkfile);
                component.CompilerFlags.ParseString(mkfile.ResolveVariables(mkfile.CompilerFlagsStr, Makefile.VarType.RegularVariable | Makefile.VarType.PropSheetVariable, false));
            }

            string extension = Path.GetExtension(rule.Output).Replace(".", "");
            component.Type = extension;
            component.IsCustomLinkStep = true;

            return component;
        }

        /*
         * Given:
         *   A Cl (command line compile) instruction with a /link flag- a single instruction that has been parsed before, and a CustomComponent that has this compile instruction
         * Output:
         *   A Component that can generate a project.
         *  Note that we want to point to source files (.c, .h) instead of linking against things that are not in our current project.
         * */
        public static Component resolveClInstructionIntoComponent(ClInstruction clInstruction, CustomComponent rule, Makefile mkfile)
        {
            Component result = new Component(rule.Output);
            result.IsCustomLinkStep = true;
            string target = "";
            //If the compile instruction has a /link flag, then it should ultimately become its own project.
            if (clInstruction.hasLinkFlags())
            {
                foreach (string file in clInstruction.clFiles)
                {
                    if (file.EndsWith(".c", StringComparison.CurrentCultureIgnoreCase) || file.EndsWith(".cpp", StringComparison.CurrentCultureIgnoreCase))
                    {
                        result.SourceFiles.Add(Path.GetFileNameWithoutExtension(file));
                    }
                    else if (file.Contains("lib"))
                    {
                        result.Link.Libs.Add(file);
                    }
                    else if (file.Contains("obj"))
                    {
                        result.Link.Objs.Add(file);
                        target = file;
                    }
                }
                CompileFlags flags = new CompileFlags(mkfile);
                flags.ParseString(clInstruction.clOptionsLiteral);
                result.CompilerFlags = flags;

                if (clInstruction.LinkOptionsLiteral_Words.Count > 0)
                {
                    MakefileUtil.ParseLinkFlags(mkfile, clInstruction.LinkOptionsLiteral_Words, result);
                }
                
            }

            string extension = Path.GetExtension(rule.Output).Replace(".", "");
            result.Type = extension;

            if (result.Name == "")
                result.setName(rule.Output);

            if (result.Link.Output == "")
                result.Link.Output = Path.GetFileName(rule.Output);
            return result;
        }

        /*
         * Input: A Component, and a CustomInstruction that needs to be attached to this Component.
         * Output: True if the instruction was identified and we attempted to take action, False if we do not know how to carry out the merge.
         * */
        public static bool MergeInstructionWithComponent(Component cp, CustomInstruction cinstr, Makefile mkfile)
        {
            if (cinstr.isCl())
            {
                ClInstruction clInstr = (ClInstruction)cinstr;
                string target = "";
                foreach (string clFile in clInstr.clFiles)
                {
                    if (clFile.EndsWith(".c", StringComparison.CurrentCultureIgnoreCase) || clFile.EndsWith(".cpp", StringComparison.CurrentCultureIgnoreCase))
                    {
                        cp.SourceFiles.Add(Path.GetFileNameWithoutExtension(clFile));
                    }
                    if (clFile.Contains(".obj"))
                        target = clFile;
                }
                foreach (string clLib in clInstr.clLibs)
                {
                    cp.Link.Libs.Add(clLib);
                }

                if (cp.CompilerFlags == null)
                    cp.CompilerFlags = new CompileFlags(mkfile);
                cp.CompilerFlags.ParseString(clInstr.clOptionsLiteral);

                foreach (string linkOption in clInstr.linkerOptions)
                {
                    cp.Link.Options.Add(new KeyValuePair<string, string>(target, linkOption));
                }

                mkfile.GenerateSourceFileNames(cp);
                return true;
            }
            else if (cinstr.isRc())
            {
                RcInstruction rcInstr = (RcInstruction)cinstr;
                cp.SourceFiles.Add(rcInstr.rcScript);
                RcFlags rcFlags = new RcFlags(mkfile);
                rcFlags.ParseString(rcInstr.instruction_literal);
                cp.CustomFlags.Add(rcInstr.rcScript, rcFlags);

                mkfile.GenerateSourceFileNames(cp);
            }
            else
            {
                //We cannot identify the instruction, we will assume that it must be added as a prebuild step to the component.
                cp.PostBuildInstructions.Add(cinstr);
            }
            return false;
        }

        /*
         * Given:
         *   A CustomComponent that creates a .dll. This CustomComponent should have a link instruction.
         * Output:
         *   A Component that can be used to generate a project for this .dll
         * 
         * */
        public static Component generateComponentFromRule(CustomComponent rule, Makefile mkfile)
        {

            Component result = new Component(rule.Name);
            result.Owner = mkfile;
            List<CustomInstruction> preBuildInstrs = new List<CustomInstruction>();
            List<CustomInstruction> postBuildInstrs = new List<CustomInstruction>();

            bool afterLinkInstruction = false;


            foreach (string instr in rule.CustomInstructions)
            {
                CustomInstruction cinstr = new CustomInstruction(instr);
                if (cinstr.isNmake())
                {
                    LinkInstruction linkInstr = new LinkInstruction(cinstr);
                    result = resolveLinkInstruction(linkInstr,rule,mkfile);
                    afterLinkInstruction = true;
                }
                else if (cinstr.isCl())
                {
                    ClInstruction compileInstruction = new ClInstruction(cinstr);
                    foreach (string sourceFile in compileInstruction.clFiles)
                        result.SourceFileNames.Add(sourceFile);

                }
                else if (!afterLinkInstruction)
                {
                    preBuildInstrs.Add(cinstr);
                }
                else
                {
                    postBuildInstrs.Add(cinstr);
                }

            }
            result.PreBuildInstructions = preBuildInstrs;
            result.PostBuildInstructions = postBuildInstrs;
            return result;
        }
    }
}
