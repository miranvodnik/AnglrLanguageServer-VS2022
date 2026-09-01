using Anglr.Compiler;
using Anglr.Declarations;
using Anglr.Parser;
using Anglr.Parser.SyntaxTree;
using AnglrLibrary;
using AnglrLogLibrary;
using System;
using System.Collections.Generic;
using System.IO;

namespace AnglrHtmlColorizerLibrary
{
    public class AnglrHtmlColorizerProgram
    {

        public static void MainTask (string [] args)
        {
            IAnglrLogger Logger = new ConsoleAnglrLogger ();

            if (args.Length < 1)
            {
                Logger.ErrorLine ("usage: colorizer ( [-t <fragment type>] <file>.anglr ) ...");
                return;
            }

            anglrCompiler.loopDetection = true;
            anglrCompiler.createParseTree = true;

            string fragmentName = null;
            int startToken = AnglrDeclarations.tokens._anglr_file_terminal_;
            bool changeFragmentType = false;

            foreach (string arg in args)
            {
                if (arg == "-t")
                {
                    changeFragmentType = true;
                    continue;
                }
                if (changeFragmentType)
                {
                    startToken = AnglrFragments.GetFragmentInfo (arg).tokenId;
                    fragmentName = arg;
                    changeFragmentType = false;
                    continue;
                }
                anglrCompiler compiler = new anglrCompiler (fragmentName, Logger);
                compiler.Error_Event += (int lineno, int column, int token, string tokenString) =>
                { 
                    return true;
                };
                if (compiler.Parse (arg, startToken, new object [] { Path.GetFileNameWithoutExtension (arg), "File Name", arg }) != 0)
                {
                    fragmentName = null;
                    startToken = AnglrDeclarations.tokens._anglr_file_terminal_;
                    continue;
                }
                AnglrHtmlColorizer anglrHtmlColorizer = new AnglrHtmlColorizer ();
                foreach (SyntaxTreeBase node in compiler.parseList)
                {
                    node.InvokeTraverse (anglrHtmlColorizer);
                    node.InvokeTraverse (anglrHtmlColorizer);
                    node.InvokeTraverseCommon (anglrHtmlColorizer);
                }
                fragmentName = null;
                startToken = AnglrDeclarations.tokens._anglr_file_terminal_;
            }
        }

        private static bool Compiler_Error_Event (int lineno, int column, int token, string tokenString)
        {
            throw new NotImplementedException ();
        }
    }
}
