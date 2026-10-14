using Tree.Lexer;
using Tree.Parser;
using Anglr.Parser.Core;
using System.Text;

namespace AnglrSyntaxTreeTest
{
    internal class TextSourceInfoComparer : IComparer<(int line, int column)>
    {
        public int Compare ((int line, int column) x, (int line, int column) y)
        {
            if (x.line != y.line)
                return x.line - y.line;
            if (x.column != y.column)
                return x.column - y.column;
            return 0;
        }
    }
    internal class TextSourceInfo : SortedDictionary<(int line, int column), string>
    {
        public TextSourceInfo () : base (new TextSourceInfoComparer ()) { }
    }
    internal class TreeAnalyzer : SyntaxTreeWalker
    {
        private TextSourceInfo sourceInfo;
        public TreeAnalyzer ()
        {
            sourceInfo = new TextSourceInfo ();

            tree_Event += TreeAnalyzer_tree_Event;
        }
        private bool TreeAnalyzer_tree_Event (SyntaxTreeCallbackReason reason, tree.production_kind kind, tree p_tree)
        {
            switch (reason)
            {
                case SyntaxTreeCallbackReason.TraversalPrologueCallbackReason:
                    break;
                case SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason:
                {
                    string text = null;
                    int column = -1;
                    int lineno = -1;
                    p_tree.m_tree_element_set.Iterate
                        (
                        null,
                        (element, appData) =>
                        {
                            tree_element tree_Element = element.m_tree_element;
                            element_name element_Name = tree_Element.m_element_name;
                            element_value element_Value = tree_Element.m_element_value;
                            switch (element_Name.m__cstring_.text)
                            {
                                case @"""text""":
                                    text = element_Value.m__cstring_.text.Substring (1, element_Value.m__cstring_.text.Length - 2);
                                    break;
                                case @"""column""":
                                    column = int.Parse (element_Value.m__number_.text);
                                    break;
                                case @"""lineno""":
                                    lineno = int.Parse (element_Value.m__number_.text);
                                    break;
                            }
                            return appData;
                        });
                    if ((text == null) || (column < 0) || (lineno < 0))
                        break;
                    sourceInfo [(lineno, column)] = text;
                }
                break;
            }
            return true;
        }
        public string CreateSourceText ()
        {
            bool start = true;
            int currentLine = 0;
            int currentColumn = 0;
            StringBuilder sb = new StringBuilder ();
            foreach (var element in sourceInfo)
            {
                int lineno = element.Key.line;
                int column = element.Key.column;
                string text = element.Value;
                while (currentLine < lineno)
                {
                    if (start)
                    {
                        start = false;
                        currentLine = lineno;
                        break;
                    }
                    sb.AppendLine ();
                    ++currentLine;
                    currentColumn = 0;
                }
                while (currentColumn < column)
                {
                    sb.Append (' ');
                    ++currentColumn;
                }
                sb.Append (text);
                currentColumn += text.Length;
            }
            return sb.ToString ();
        }
    }

    internal class Program
    {
        static void Main (string [] args)
        {
            // global flags should be set before TreeParser object creation
            TreeParser.createParseTree = true;
            TreeParser.loopDetection = true;

            // parse every source file named in command line
            foreach (string arg in args)
            {
                TreeParser parser = new TreeParser ();
                TreeLexer lexer = new TreeLexer (new StreamReader (arg));

                // register method reporting syntax errors
                parser.Error_Event += (int lineno, int column, int token, string tokenString) =>
                {
                    Console.Error.WriteLine ($"Syntax error: file ({arg}), line ({lineno}), column ({column}), token ({token} - {tokenString}");
                    return true;    // continue parsing
                };


                // invoke parser
                if (parser.parse (lexer) != 0)
                    continue;   // errors, skip current file

                // visit every node of every syntax tree generated for current source file
                foreach (var syntaxTree in parser.parseList)
                {
                    // establish clear parent-child relationships
                    syntaxTree.reparent (null);
                    // create walker object
                    TreeAnalyzer treeAnalyzer = new TreeAnalyzer ();
                    // visit all nodes of syntax tree
                    treeAnalyzer.Traverse (syntaxTree as tree);
                    Console.Write (treeAnalyzer.CreateSourceText ());
                }
            }
        }
    }
}
