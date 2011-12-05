/*
|| NAME:	CustomInstruction.cs
||
||
||
|| AUTHORS:	Frank Kotarski
||
|| SYNOPSIS:    Wrap around a batch instruction found in a makefile. Parse and analyze the nature of the instruction.
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
using System.Text.RegularExpressions;
using System.IO;

namespace MakefileParser
{
    //Custom instructions that may be found in the makefile. A CustomComponent may have one or more of these instructions with its associated targets.
    public class CustomInstruction
    {

        public string instruction_literal { get; set; }
        public List<string> words { get; set; }
        public SortedSet<string> commands { get; set; }
        public char[] delims = new char[1] { ' ' };
        private bool validInstruction;
        protected SortedSet<string> validCommands;

        public CustomInstruction() 
        {
            instruction_literal = "";
            words = new List<string>();
            commands = new SortedSet<string>();
            validCommands = new SortedSet<string>();
            populateValidCommands();
            validInstruction = false;
        }
        public CustomInstruction(string instr)
        {
            words = new List<string>();
            commands = new SortedSet<string>();
            instruction_literal = instr;
            instruction_literal = instruction_literal.Trim();
            validCommands = new SortedSet<string>();
            populateValidCommands();
            validInstruction = analyzeInstruction();
        }
        public CustomInstruction(CustomInstruction customInstr)
        {
            instruction_literal = customInstr.instruction_literal;
            words = customInstr.words;
            commands = customInstr.commands;
            delims = customInstr.delims;
            validInstruction = customInstr.validInstruction;
            validCommands = customInstr.validCommands;
        }

        //These are actual commands in an instruction that may be found in a makefile, and are command line / batch syntax
        #region Commands
        public const string Instruction_Copy = "copy";              //Copy a file
        public const string Instruction_Move = "move";              //Move a file
        public const string Instruction_Delete = "del";             //Delete a file
        public const string Instruction_Clean = "clean";            //Clean a directory
        public const string Instruction_Nmake = "nmake";            //Recursively call nmake
        public const string Instruction_If =    "if";               //Boolean if
        public const string Instruction_Exists = "exist";           //Does file exist
        public const string Instruction_Not =   "not";              //Negation
        public const string Instruction_MakeDir = "mkdir";          //Make a folder
        public const string Instruction_Link = "link";              //Creating executable
        public const string Instruction_Rmdir = "rmdir";            //Delete an empty directory
        public const string Instruction_Midl = "midl";              //Microsoft MIDL compiler
        public const string Instruction_Xcopy = "xcopy";            //More functional copy
        public const string Instruction_Devenv = "devenv";          //'Run' a project or a solution
        public const string Instruction_And = "&&";                 //Join commands
        public const string Instruction_AndB = "&";                 //Join commands
        public const string Instruction_Silent = ">nul";            //Hide output
        public const string Instruction_Comment = "#";              //Comment a line
        public const string Instruction_Cl = "cl";                  //Command line compiler
        public const string Instruction_Rc = "rc";                  //Microsoft resource generator
        public const string Instruction_Echo = "echo";              //Echo to output
        #endregion

		//Add the list of valid commands
        private void populateValidCommands()
        {
            validCommands.Add(Instruction_Copy);
            validCommands.Add(Instruction_Move);
            validCommands.Add(Instruction_Delete);
            validCommands.Add(Instruction_Nmake);
            validCommands.Add(Instruction_If);
            validCommands.Add(Instruction_Exists);
            validCommands.Add(Instruction_Not);
            validCommands.Add(Instruction_MakeDir);
            validCommands.Add(Instruction_Link);
            validCommands.Add(Instruction_Rmdir);
            validCommands.Add(Instruction_Midl);
            validCommands.Add(Instruction_Xcopy);
            validCommands.Add(Instruction_Devenv);
            validCommands.Add(Instruction_And);
            validCommands.Add(Instruction_AndB);
            validCommands.Add(Instruction_Silent);
            validCommands.Add(Instruction_Comment);
            validCommands.Add(Instruction_Cl);
            validCommands.Add(Instruction_Rc);
            validCommands.Add(Instruction_Echo);
        }
		//Make a multi-line instruction a single line
        public static string makeInstructionsSingleLine(List<CustomInstruction> cinstrs)
        {

            string preinstrs = "";
            foreach (CustomInstruction cinstr in cinstrs)
            {
                preinstrs += cinstr.instruction_literal + "\r\n";
            }
            return preinstrs;
        }
		//If an instruction is joined by &&, split it into multiple instructions
        public static string[] splitCombinedInstructions(string instr)
        {
            string[] delimiters = new string[] { "&&", "&" };
            string[] splitinstr = instr.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
            return splitinstr;
        }
        public string validateCommand(string str)
        {
            if (validCommands.Contains(str))
                return str;

            return "";
        }
        public bool isValidCommand(string str)
        {
            if (!validateCommand(str).Equals(""))
                return true;
            return false;
        }
        public virtual string[] parseInstruction(string instruction)
        {
            char[] delimiterChars = delims;

            string text = instruction_literal;
            string[] words = text.Split(delimiterChars);

            if (words[0] != null)
            {
                return words;
            }
            return null;
        }
        public string normalizeCommand(string str)
        {
            string normalizedCommand = str.ToLower();

            if (normalizedCommand.Contains(".") == true)
            {
                string extension = normalizedCommand.Substring(normalizedCommand.IndexOf("."));
                if (extension.Equals(".exe"))
                {
                    normalizedCommand = normalizedCommand.Substring(0, normalizedCommand.IndexOf("."));
                }
            }

            normalizedCommand = normalizedCommand.Replace("@", "");

            return normalizedCommand;
        }
        public virtual bool analyzeInstruction()
        {
            string[] instr_words = parseInstruction(instruction_literal);
            bool unknownCommandFound = false;
            if (instr_words.Length > 0)
            {
                string firstCommand = normalizeCommand(instr_words[0]);
                if (!validateCommand(firstCommand).Equals(""))
                    commands.Add(firstCommand);
                else
                    unknownCommandFound = true;

                foreach (string word in instr_words)
                {
                    string normalizedCommand = normalizeCommand(word);

                    if (!validateCommand(normalizedCommand).Equals(""))
                        commands.Add(normalizedCommand);

                    words.Add(word);
                }
            }
            return !unknownCommandFound;
        }
        public bool isValidInstruction()
        {
            return validInstruction;
        }
        public bool isNmake()
        {
            return commands.Contains(Instruction_Nmake);
        }
        public bool isLink()
        {
            return commands.Contains(Instruction_Link);
        }
        public bool isMidl()
        {
            return commands.Contains(Instruction_Midl);
        }
        public bool isCl()
        {
            return commands.Contains(Instruction_Cl);
        }
        public bool isRc()
        {
            return commands.Contains(Instruction_Rc);
        }
        public bool isCleanInstruction()
        {
            if (commands.Contains(Instruction_Delete) || commands.Contains(Instruction_Exists))
                return true;
            return false;
        }
        public bool containsCopyCommand()
        {
            return commands.Contains(Instruction_Copy);
        }
    }

    //A class representing a Link instruction in command-line form. A Link instruction will ultimately be converted to its own VS project.
    public sealed class LinkInstruction : CustomInstruction
    {
        //Link flags
        public const string flag_dll = "dll";            
        public const string flag_exe = "exe";
        public const string flag_out = "out";
        public const string flag_def = "def";
        public const string flag_entry = "entry";

        public List<string> Objs { get; set; }
        public List<string> Libs { get; set; }
        public List<KeyValuePair<string, string>> Options { get; set; }
        public string LinkOptionsLiteral { get; set; }
        public List<string> LinkOptionsLiteral_Words { get; set; }

        public string Output { get; set; }
        public string Def { get; set; }
        public string Entry { get; set; }

        bool isDll { get; set; }
        bool isExe { get; set; }

        public LinkInstruction() : base()
        {
            Objs = new List<string>();
            Libs = new List<string>();
            Options = new List<KeyValuePair<string, string>>();
            LinkOptionsLiteral = "";
            LinkOptionsLiteral_Words = new List<string>();
            Output = "";
            Def = "";
            Entry = "";
            isDll = false;
            isExe = false;
        }
        public LinkInstruction(string instr) : base(instr) 
        {
            Objs = new List<string>();
            Libs = new List<string>();
            Options = new List<KeyValuePair<string, string>>();
            LinkOptionsLiteral = "";
            LinkOptionsLiteral_Words = new List<string>();
            Output = "";
            Def = "";
            Entry = "";
            isDll = false;
            isExe = false;
            analyzeCommands();
        }
        public LinkInstruction(CustomInstruction customInstr)
            : base(customInstr)
        {
            Objs = new List<string>();
            Libs = new List<string>();
            Options = new List<KeyValuePair<string, string>>();
            LinkOptionsLiteral = "";
            LinkOptionsLiteral_Words = new List<string>();
            Output = "";
            Def = "";
            Entry = "";
            isDll = false;
            isExe = false;
            analyzeCommands();
        }

        public void analyzeCommands()
        {
            string currentFlag = "";
            bool processingFiles = false;
            foreach (string word in words)
            {
                if (word.StartsWith("/") || word.StartsWith("-"))
                {
                    if (word.Contains(flag_dll) || word.Contains(flag_dll.ToUpper()))
                    {
                        isDll = true;
                    }
                    else if (word.Contains(flag_exe) || word.Contains(flag_exe.ToUpper()))
                    {
                        isExe = true;
                    }
                    else if (word.Contains(flag_out) || word.Contains(flag_out.ToUpper()))
                    {
                        Output = word.Substring(flag_out.Length + 1);
                        Options.Add(new KeyValuePair<string, string>(flag_out, Output));
                        currentFlag = flag_out;
                    }
                    else if (word.Contains(flag_def) || word.Contains(flag_def.ToUpper()))
                    {
                        Def = word.Substring(flag_def.Length + 1);
                        Options.Add(new KeyValuePair<string, string>(flag_def, Def));
                        currentFlag = flag_def;
                    }
                    else if (word.Contains(flag_entry) || word.Contains(flag_entry.ToUpper()))
                    {
                        Entry = word.Substring(flag_entry.Length + 1);
                        Options.Add(new KeyValuePair<string, string>(flag_entry, Entry));
                        currentFlag = flag_entry;
                    }
                    else
                    {
                        Options.Add(new KeyValuePair<string, string>(word, Entry));
                    }   
                }
                else if (word.Contains(".obj"))
                {
                    Objs.Add(word);
                    processingFiles = true;
                }
                else if (word.Contains(".lib"))
                {
                    Libs.Add(word);
                    processingFiles = true;
                }
                else if (word.Contains(".res"))
                { }
                else if (currentFlag != "")
                {

                }
                else
                {
                    //Unrecognized command
                }

                if (word.ToLower().Equals("link") || word.ToLower().Equals("link.exe"))
                { }
                else if (!processingFiles)
                {
                    if (!word.Equals(""))
                    {
                        LinkOptionsLiteral += word + " ";
                        LinkOptionsLiteral_Words.Add(word);
                    }
                }

            }
            LinkOptionsLiteral = LinkOptionsLiteral.Trim();
        }

        public void convertToLink(Makefile mkfile, Component cp)
        {
            MakefileUtil.ParseLinkFlags(mkfile, words, cp);
        }
    }

    //A class representing an Nmake instruction, that is called recursively within a makefile
    public sealed class NmakeInstruction : CustomInstruction
    {
        public string target { get; set; }
        public string sourceDir { get; set; }
        public List<string> nmakeOptions { get; set; }
        //The nmake instruction was called recursively, so these are instructions before and after the call.
        public List<CustomInstruction> preInstructions { get; set; }
        public List<CustomInstruction> postInstructions { get; set; }

        public NmakeInstruction() : base()
        {
            target = "";
            sourceDir = "";
            nmakeOptions = new List<string>();
            preInstructions = new List<CustomInstruction>();
            postInstructions = new List<CustomInstruction>();
        }

        public NmakeInstruction(string instr, string sourceDirectory)
            : base(instr)
        {
            target = "";
            setSourceDirectory(sourceDirectory);
            nmakeOptions = new List<string>();
            parseNmakeCommand(instruction_literal);
            preInstructions = new List<CustomInstruction>();
            postInstructions = new List<CustomInstruction>();
        }
        public NmakeInstruction(CustomInstruction customInstr, string sourceDirectory)
            : base(customInstr)
        {
            target = "";
            setSourceDirectory(sourceDirectory);
            nmakeOptions = new List<string>();
            parseNmakeCommand(instruction_literal);
            preInstructions = new List<CustomInstruction>();
            postInstructions = new List<CustomInstruction>();
        }
        /*
         * NMAKE [option] [macros] [targets] [@CommandFile]
         * Nmake.exe options are preceded by either a slash mark (/) or a hyphen (-) and are not case-sensitive.
         * Options and targets need not be separated by white space
         * Supports only character-long flags
         * Does not support Macros or CommandFiles yet
         * */
        public bool parseNmakeCommand(string starget)
        {
            //Options and targets need not be separated by white space, so we need to dissect every word
            foreach (string word in words)
            {

                if (word.ToLower().Equals("nmake"))
                { }

                else
                {
                    for (int i = 0; i < word.Length; i++)
                    {
                        char c = word[i];
                        Console.WriteLine(word[i]);
                        if (c == '/' || c == '-')
                        {
                            string option = new string(word[i + 1], 1);
                            nmakeOptions.Add(option);
                            i++;
                        }
                        else
                        {
                            if (target == "")
                            {
                                target = word.Substring(i);
                                return true;
                            }
                        }

                    }
                }
 
            }

            return false;      
        }
        public void setSourceDirectory(string strDir)
        {
            string source = strDir;
            if (source.Length > 0)
            {
                if (source[source.Length - 1] != '\\')
                {
                    source += "\\";
                }
            }
            sourceDir = source;
        }
    }

    //A class representing a midl instruction.
    public sealed class MidlInstruction : CustomInstruction
    {
        public List<string> sourceFiles { get; set; }
        public string outputFile { get; set; }
        public List<string> MidlFlags { get; set; }
       
        public MidlInstruction() : base()
        {
            sourceFiles = new List<string>();
            outputFile = "";
            MidlFlags = new List<string>();
        }
        public MidlInstruction(string instr) : base(instr) 
        {
            sourceFiles = new List<string>();
            outputFile = "";
            MidlFlags = new List<string>();
            analyzeCommands();
        }
        public MidlInstruction(CustomInstruction customInstr)
            : base(customInstr)
        {
            sourceFiles = new List<string>();
            outputFile = "";
            MidlFlags = new List<string>();
            analyzeCommands();
        }
        public void analyzeCommands()
        {
            foreach (string word in words)
            {
                if (word.ToLower().Equals("midl") || word.ToLower().Equals("midl.exe"))
                { }
                else if (word.Contains(".c") || word.Contains(".h"))
                    sourceFiles.Add(word);
                else if(word.Contains(".idl"))
                    outputFile = word;
                else
                    MidlFlags.Add(word);
            }

        }
    }

    //A class representing a cl instruction.
    public sealed class ClInstruction : CustomInstruction
    {
        public List<string> clFiles { get; set; }
        public List<string> clLibs { get; set; }
        public List<string> clOptions { get; set; }
        public string clOptionsLiteral { get; set; }
        public string LinkOptionsLiteral { get; set; }
        public List<string> LinkOptionsLiteral_Words { get; set; }
        public SortedSet<string> linkerOptions { get; set; }
        public SortedSet<string> clDefines { get; set; }
        public List<string> unidentified { get; set; }
        bool containsLinkFlags { get; set; }
        public const string flag_Fo = "Fo";
        public ClInstruction()
            : base()
        {
            clFiles = new List<string>();
            clLibs = new List<string>();
            clOptions = new List<string>();
            linkerOptions = new SortedSet<string>();
            clDefines = new SortedSet<string>();
            unidentified = new List<string>();
            containsLinkFlags = false;
            clOptionsLiteral = "";
            LinkOptionsLiteral = "";
            LinkOptionsLiteral_Words = new List<string>();
        }
        public ClInstruction(string instr)
            : base(instr)
        {
            clFiles = new List<string>();
            clLibs = new List<string>();
            clOptions = new List<string>();
            linkerOptions = new SortedSet<string>();
            unidentified = new List<string>();
            clDefines = new SortedSet<string>();
            containsLinkFlags = false;
            clOptionsLiteral = "";
            LinkOptionsLiteral = "";
            LinkOptionsLiteral_Words = new List<string>();
            analyzeCommands();
        }
        public ClInstruction(CustomInstruction customInstr)
            : base(customInstr)
        {
            clFiles = new List<string>();
            clLibs = new List<string>();
            clOptions = new List<string>();
            linkerOptions = new SortedSet<string>();
            clDefines = new SortedSet<string>();
            unidentified = new List<string>();
            containsLinkFlags = false;
            clOptionsLiteral = "";
            LinkOptionsLiteral = "";
            LinkOptionsLiteral_Words = new List<string>();
            analyzeCommands();
        }
        public void analyzeCommands()
        {
            bool doneWithCompilerFlags = false;
            string word = "";
            for(int i = 0; i < words.Count; i++)
            {
                word = words[i];
                if (word.ToLower().Equals("cl") || word.ToLower().Equals("cl.exe"))
                { }
                else if (word.Equals(""))
                { }
                else if (word.Contains(".c") || word.Contains(".cpp") || word.Contains(".cxx") || word.Contains(".obj") || word.Contains(".lib") || word.Contains(".def"))
                {
                    doneWithCompilerFlags = true;

                    if (word.Contains(flag_Fo))
                    {
                        string delim = word[0].ToString();
                        string flag = delim + flag_Fo;
                        clOptionsLiteral += word + " ";
                        clOptions.Add(flag_Fo);
                        clFiles.Add(word.Substring(flag.Length));
                    }
                    else
                    {
                        clFiles.Add(word);
                    }
                }
                else if (word.Length > 0 && (word[0].Equals('/') || word[0].Equals('-')))
                {
                    if (!doneWithCompilerFlags)
                    {
                        if (word.Equals("/D")||word.Equals("-D"))
                        {
                            clDefines.Add(words[i + 1]);
                            i++;
                        }
                        else
                        {
                             clOptionsLiteral += word + " ";
                             clOptions.Add(word.Substring(1));
                        }
                    }
                    else
                    {
                        containsLinkFlags = true;
                        LinkOptionsLiteral += word + " ";
                        LinkOptionsLiteral_Words.Add(word);
                        linkerOptions.Add(word);
                    }
                }
                else
                {
                    unidentified.Add(word);
                }
            }
            clOptionsLiteral = clOptionsLiteral.Trim();
            LinkOptionsLiteral = LinkOptionsLiteral.Trim();
        }
        public bool compileWithoutLinking()
        {
            return clOptions.Contains("c");
        }
        public bool hasLinkFlags()
        {
            return containsLinkFlags;
        }
    }

    //A class representing an rc instruction.
    public sealed class RcInstruction : CustomInstruction
    {
        public string rcScript {get; set;}
        public string rcOutput { get; set; }
        public Dictionary<string,string> RcOptions { get; set; }
        public readonly HashSet<string> singleInputFlags = new HashSet<string>{ "fo", "fm", "j", "k", "l", "q" };

        public RcInstruction()
            : base()
        {
            rcScript = "";
            rcOutput = "";
            RcOptions = new Dictionary<string,string>();
        }
        public RcInstruction(string instr)
            : base(instr)
        {
            rcScript = "";
            rcOutput = "";
            RcOptions = new Dictionary<string,string>();

        }
        public RcInstruction(CustomInstruction customInstr)
            : base(customInstr)
        {
            rcScript = "";
            RcOptions = new Dictionary<string,string>();
            analyzeCommands();
        }
        public void analyzeCommands()
        {
            string arg = "";
            for(int i=0; i<words.Count; i++)
            {
                arg = words[i].ToLower().Trim();
                if (arg.Length > 0)
                {
                    if (arg.Equals("rc") || arg.Equals("rc.exe"))
                    { }

                    else if (arg[0].Equals('/') || arg[0].Equals('-'))
                    {
                        string flag = arg.Remove(0, 1);
                        if (singleInputFlags.Contains(flag))
                        {
                            RcOptions.Add(flag, words[i + 1]);
                            if (Path.GetExtension(words[i + 1]).Equals(".res"))
                                rcOutput = words[i + 1].ToLower().Trim();
                            i++;
                        }
                        else
                        {
                            RcOptions.Add(flag, "");
                        }
                    }
                    else if (arg.Contains("$"))
                    {}
                    else
                    {
                        rcScript = arg.ToLower().Trim();
                    }
                }
            }
        }
    }
}
