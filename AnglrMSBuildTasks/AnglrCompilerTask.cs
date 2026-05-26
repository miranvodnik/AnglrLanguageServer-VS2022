using Anglr.Compiler;
using Anglr.Declarations;
using Anglr.Parser;
using Anglr.Parser.SyntaxTree;
using AnglrLibrary;
using AnglrLogLibrary;
using AnglrParserLibrary;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using System;
using System.Collections;
using System.Reflection;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace AnglrMSBuildTasks
{
    using Task = Microsoft.Build.Utilities.Task;

    [Obfuscation(Exclude = true)]
    internal class AnglrMSBuildLogger : AnglrLoggerBase
    {
        public TaskLoggingHelper Logger { get; private set; }

        internal AnglrMSBuildLogger (TaskLoggingHelper logger)
        {
            Logger = logger;
        }

        protected override void Write (AnglrLogLevel level, string message, uint flags = 0)
        {
            Logger.LogMessage (MessageImportance.High, message);
        }

        protected override void WriteLine (AnglrLogLevel level, string message, uint flags = 0)
        {
            Logger.LogMessage (MessageImportance.High, message);
        }
    }

    [Obfuscation(Exclude = true)]
    public class Anglr : Task
    {
        public Anglr ()
        {
            Logger = new AnglrMSBuildLogger (Log);
        }

        internal AnglrMSBuildLogger Logger { get; private set; }

        /// <summary>
        /// debug flag - when set, anglr will report additional information
        /// </summary>
        [Obfuscation (Exclude = true)]
        public bool Debug { get; set; } = false;

        /// <summary>
        /// generate syntax tree flag - when set, anglr will create syntax tree
        /// </summary>
        [Obfuscation (Exclude = true)]
        public bool Tree { get; set; } = false;

        /// <summary>
        /// loop detection flag - when set, anglr will detect (and correct) looping conditions in grammar rules
        /// </summary>
        [Obfuscation (Exclude = true)]
        public bool Loop { get; set; } = true;

        /// <summary>
        /// iterator flag - when set, anglr will create iterators for recursive rules
        /// </summary>
        [Obfuscation (Exclude = true)]
        public bool Iterator { get; set; } = false;

        /// <summary>
        /// precedence flag - when set, anglr will try to deduce precedence grammar from specially related syntax rules
        /// </summary>
        [Obfuscation (Exclude = true)]
        public bool Precedence { get; set; } = false;

        /// <summary>
        /// name of output directory
        /// </summary>
        [Obfuscation (Exclude = true)]
        public string OutputDir { get; set; } = @"Syntax\";

        /// <summary>
        /// source file names of anglr files
        /// </summary>
        [Obfuscation (Exclude = true)]
        [Required]
        public ITaskItem [] SourceFile { get; set; }

        /// <summary>
        /// output language indicator: c# is default output language
        /// </summary>
        [Obfuscation (Exclude = true)]
        public string Code { get; set; } = "cs";

        /// <summary>
        /// names of generated files
        /// </summary>
        [Obfuscation (Exclude = true)]
        [Output]
        public ITaskItem [] OutputSourceFileList { get; private set; }

        /// <summary>
        /// names of generated library files
        /// </summary>
        [Obfuscation (Exclude = true)]
        [Output]
        public ITaskItem [] OutputLibraryFileList { get; private set; }

        /// <summary>
        /// output language indicator: c# is default output language
        /// </summary>
        [Obfuscation (Exclude = true)]
        [Output]
        public string TestString { get; private set; } = "TEST STRING";

        private int errorCount = 0;
        private string currentSourceFile;

        public override bool Execute ()
        {
            Logger.InfoLine ($"ANGLR project: '{BuildEngine.ProjectFileOfTaskNode}'");

            int errorCount = 0;
            foreach (ITaskItem taskItem in SourceFile)
            {
                currentSourceFile = taskItem.ItemSpec;
                try { Debug = bool.Parse (taskItem.GetMetadata ("Debug")); } catch (Exception) { }
                try { Tree = bool.Parse (taskItem.GetMetadata ("Tree")); } catch (Exception) { }
                try { Loop = bool.Parse (taskItem.GetMetadata ("Loop")); } catch (Exception) { }
                try { Iterator = bool.Parse (taskItem.GetMetadata ("Iterator")); } catch (Exception) { }
                try { Precedence = bool.Parse (taskItem.GetMetadata ("Precedence")); } catch (Exception) { }
                OutputDir = taskItem.GetMetadata ("OutputDir");
                Code = taskItem.GetMetadata ("Code");

                Logger.InfoLine ($"ANGLR: compile parser specification file: '{currentSourceFile}'");
                try
                {
                    if (Debug)
                        AnglrParser.debug = true;
                    if (Tree)
                        AnglrParser.createParseTree = true;
                    if (Loop)
                        AnglrParser.loopDetection = true;
                    if (Iterator)
                        anglrCompiler.createIterators = true;
                    if (Precedence)
                        anglrCompiler.createPrecedenceGrammar = true;

                    anglrCompiler parser = new anglrCompiler ();
                    parser.Error_Event += Invoke_Error_Callback;
                    if ((parser.Parse (currentSourceFile, AnglrDeclarations.tokens._anglr_file_terminal_, new object [] { Path.GetFileNameWithoutExtension (currentSourceFile), "File Name", currentSourceFile }) != 0) || (errorCount > 0))
                    {
                        Logger.InfoLine ($"ANGLR: compile failed, file: '{currentSourceFile}'");
                        ++errorCount;
                        continue;
                            }
                    foreach (SyntaxTreeBase node in parser.parseList)
                    {
                        if (node == null)
                            continue;
                        switch (Code)
                        {
                            case "cs":
                            {
                                CSharpGenerator cSharpGenerator = new CSharpGenerator ((_anglr_file_fragment_) node, OutputDir, parser);
                                cSharpGenerator.GenerateCsCode (parser.startSyntaxRule);
                                foreach (KeyValuePair<string, GeneralPart> member in cSharpGenerator.generalParts)
                                {
                                    string partName = member.Key;
                                    GeneralPart part = member.Value;
                                }
                                foreach (KeyValuePair<string, DeclarationsPart> member in cSharpGenerator.declarationParts)
                                {
                                    string partName = member.Key;
                                    DeclarationsPart part = member.Value;
                                }
                                foreach (KeyValuePair<string, ScannerPart> member in cSharpGenerator.scannerParts)
                                {
                                    string partName = member.Key;
                                    ScannerPart part = member.Value;
                                }
                                foreach (KeyValuePair<string, LexerPart> member in cSharpGenerator.lexerParts)
                                {
                                    string partName = member.Key;
                                    LexerPart part = member.Value;
                                }
                                foreach (KeyValuePair<string, ParserPart> member in cSharpGenerator.parserParts)
                                {
                                    string partName = member.Key;
                                    ParserPart part = member.Value;
                                }
                                try
                                {
                                    OutputSourceFileList = (ITaskItem []) cSharpGenerator.SourceFileList.ToArray (typeof (ITaskItem));
                                    foreach (ITaskItem taskItem1 in OutputSourceFileList)
                                        Logger.InfoLine ($"generated source file: {taskItem1.ItemSpec}'");
                                }
                                catch (Exception e)
                                {
                                    Logger.InfoLine (e, $"ANGLR: exception formating source list");
                                }
                                try
                                {
                                    OutputLibraryFileList = (ITaskItem []) cSharpGenerator.LibraryFileList.ToArray (typeof (ITaskItem));
                                    foreach (ITaskItem taskItem1 in OutputLibraryFileList)
                                        Logger.InfoLine ($"generated library file: {taskItem1.ItemSpec}'");
                                }
                                catch (Exception e)
                                {
                                    Logger.InfoLine (e, $"ANGLR: exception formating library list");
                                }
                            }
                            break;
                            default:
                            {
                                if (Code.Length > 0)
                                {
                                    Logger.InfoLine ($"ANGLR: invalid code generator specifier: '{Code}'");
                                    continue;
                                }
                                Logger.InfoLine ($"ANGLR: no code generator specified, using default: cs'");
                                CSharpGenerator p_CSGenerator = new CSharpGenerator ((_anglr_file_fragment_) node, OutputDir, parser);
                                p_CSGenerator.GenerateCsCode (parser.startSyntaxRule);
                            }
                            break;
                        }
                    }
                }
                catch (Exception e)
                {
                    Logger.InfoLine (e, $"ANGLR: exception");
                    ++errorCount;
                    continue;
                }
            }

            return errorCount == 0;
        }

        /// <summary>
        /// Error reporting function. It is executed by Error_Event event, which is in turn fired when parser detects syntax errors in source file
        /// </summary>
        /// <param name="lineno">line number in which the error occured</param>
        /// <param name="column">column number in which the error occured</param>
        /// <param name="terminal">terminal code that causes syntax error</param>
        /// <param name="terminalString">text, which is located in the line with the number lineno and in the column with the number colnum</param>
        /// <returns>always false, meaning to continue parsing source file</returns>
        private bool Invoke_Error_Callback (int lineno, int column, int terminal, string terminalString)
        {
            ++errorCount;
            column += terminalString.Length;
            Log.LogError ("Parser", "E001", "ANGLR", currentSourceFile, lineno, column, lineno, column + terminalString.Length, "Syntax Error", null);
            return false;
        }
    }
}
