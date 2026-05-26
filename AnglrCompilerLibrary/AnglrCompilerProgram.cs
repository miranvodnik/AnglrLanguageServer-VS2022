using Anglr.Compiler;
using Anglr.Declarations;
using Anglr.Parser;
using Anglr.Parser.Core;
using Anglr.Parser.SyntaxTree;
using AnglrJsonRpcMethods;
using AnglrLibrary;
using AnglrParserLibrary;
using AnglrLogLibrary;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using System.Xml.Linq;

namespace AnglrCompilerLibrary
{
    public class AnglrCompilerProgram
    {
        public IAnglrLogger Logger { get; private set; }

        public AnglrCompilerProgram ()
        {
            Logger = new ConsoleAnglrLogger ();
            Logger.MinimumLevel = AnglrLogLevel.Debug;
        }

        private void MainTask (string [] args)
        {
            string fname = "";
            string genLang = "";
            int res = -1;

            bool genLangInd = false;

            foreach (string arg in args)
            {
                if (arg == "-d")
                {
                    AnglrParser.debug = !AnglrParser.debug;
                    continue;
                }
                if (arg == "-t")
                {
                    AnglrParser.createParseTree = !AnglrParser.createParseTree;
                    continue;
                }
                if (arg == "-l")
                {
                    AnglrParser.loopDetection = !AnglrParser.loopDetection;
                    continue;
                }
                if (arg == "-p")
                {
                    anglrCompiler.createPrecedenceGrammar = !anglrCompiler.createPrecedenceGrammar;
                    continue;
                }
                if (arg == "-i")
                {
                    anglrCompiler.createIterators = !anglrCompiler.createIterators;
                    continue;
                }
                if (arg == "-gen")
                {
                    genLangInd = true;
                    continue;
                }

                if (genLangInd)
                {
                    genLang = arg;
                    genLangInd = false;
                    continue;
                }

                fname = arg;
                anglrCompiler parser = new anglrCompiler (null, Logger);
                parser.Error_Event += Invoke_Error_Callback;
                Logger.InfoLine ($"Parse grammar file '{fname}'");
                res = parser.Parse (fname, AnglrDeclarations.tokens._anglr_file_terminal_, new object [] { Path.GetFileNameWithoutExtension (fname), "File Name", fname });
                if (res == 0)
                {
                    parselist p_synlist = parser.parseList;
                    foreach (SyntaxTreeBase node in parser.parseList)
                    {
                        if (node == null)
                            continue;

                        switch (genLang)
                        {
                            case "cs":
                            {
                                CSharpGenerator cSharpGenerator = new CSharpGenerator ((_anglr_file_fragment_) node, "", parser);
                                cSharpGenerator.GenerateCsCode (parser.startSyntaxRule);
                                ((_anglr_file_fragment_) node).Dispose ();
                            }
                            break;
                            default:
                                if (genLang.Length > 0)
                                    continue;
                                break;
                        }
                    }
                }
            }
        }

        public static async Task MainTaskAsync (string [] args)
        {
            foreach ((string, ProductionID, int, int, string) elt in AnglrFragments.FragmentsInfo)
            {
                Console.WriteLine (elt);
            }

            AnglrCompilerProgram program = null;
            IAnglrLogger logger = null;
            try
            {
                program = new AnglrCompilerProgram ();
                logger = program.Logger;
                program.MainTask (args);
            }
            catch (Exception ex)
            {
                {
                    int i = 0;
                    while (ex != null)
                    {
                        logger?.ErrorLine ($"{i} exception:   {ex.Message}");
                        logger?.ErrorLine ($"{i} stack trace: {ex.StackTrace}");
                        ex = ex.InnerException;
                        ++i;
                    }
                }
            }
        }

        public bool Invoke_Error_Callback (int lineno, int column, int token, string tokenString)
        {
            Logger.ErrorLine ($"SYNTAX ERROR: line = {lineno}, column = {column}, token = {token}, text = '{tokenString}'");
            return false;
        }
    }
}
