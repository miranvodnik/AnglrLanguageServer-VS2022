using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Math.Parser;
using Math.Scanner;

namespace $rootnamespace$
{
	class Program
	{
		/// <summary>
		/// The program calculates simple arithmetic expressions that use operators '+', '-', '*' and '/'.
		/// The program takes into account the operator priority and the nesting with round brackets.
		/// </summary>
		/// <param name="args">every argument represents arithmetic expression.
		/// If argument list is empty an expression is read from command line</param>
		static void Main (string [] args)
		{
			MathParser.createParseTree = true;
			MathParser.loopDetection = true;
			if (args.Length>0)
			{
				// list of expressions
				foreach(string arg in args)
				{
					MathScanner mathScanner = new MathScanner (File.OpenText(arg));
					MathParser mathParser = new MathParser ();
					mathParser.Error_Event += Invoke_Error_Callback;
					mathParser.simple_expr_Event += Invoke_simple_expr_Callback;
					mathParser.plus_expr_Event += Invoke_plus_expr_Callback;
					mathParser.mul_expr_Event += Invoke_mul_expr_Callback;
					mathParser.expr_Event += Invoke_expr_Callback;
					if (mathParser.parse (mathScanner) != 0)
						Console.WriteLine ("Error in expression");
					else
					{
						Console.WriteLine ("OK");
						foreach (SyntaxTreeBase node in mathParser.parseList)
						{
							mathParser.Traverse ((expr) node);
							Console.WriteLine (((expr) node).Emit (-1) + " = " + (int) node.appInfo);
						}
					}
				}
			}
			else
			{
				// command line expressions
				while (true)
				{
					string line = Console.In.ReadLine ();
					if (line == null)
						break;
					MathScanner mathScanner = new MathScanner (line);
					MathParser mathParser = new MathParser ();
					mathParser.Error_Event += Invoke_Error_Callback;
					mathParser.simple_expr_Event += Invoke_simple_expr_Callback;
					mathParser.plus_expr_Event += Invoke_plus_expr_Callback;
					mathParser.mul_expr_Event += Invoke_mul_expr_Callback;
					mathParser.expr_Event += Invoke_expr_Callback;
					if (mathParser.parse (mathScanner) != 0)
						Console.WriteLine ("Error in expression");
					else
					{
						Console.WriteLine ("OK");
						foreach (SyntaxTreeBase node in mathParser.parseList)
						{
							mathParser.Traverse ((expr) node);
							Console.WriteLine (((expr) node).Emit (-1) + " = " + (int) node.appInfo);
						}
					}
				}
			}
		}

		/// <summary>
		/// an empty Common callback routine. Common callback is executed on all nodes of syntax tree
		/// </summary>
		/// <param name="reason">reflects step of syntax tree traversal</param>
		/// <param name="kind">syntax rule production number</param>
		/// <param name="p_node">syntax tree node reference</param>
		/// <returns>always true: continue traversal</returns>
		private static bool Invoke_Common_Callback (SyntaxTreeCallbackReason reason, int kind, SyntaxTreeBase p_node)
		{
			bool status = true;
			switch (reason)
			{
			case SyntaxTreeCallbackReason.BuilderCallbackReason:
				break;
			case SyntaxTreeCallbackReason.TraversalPrologueCallbackReason:
				break;
			case SyntaxTreeCallbackReason.TraversalMidTermCallbackReason:
				break;
			case SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason:
				break;
			}
			return status;
		}

		/// <summary>
		/// syntax tree traversal routine affecting syntax tree nodes associated with syntax rule 'expr'.
		/// It remembers the current value of expression associated with subtree underlying 'expr' node.
		/// It does that at the exit step of traversal, when the value of underlying tree is already computed.
		/// </summary>
		/// <param name="reason">reflects step of syntax tree traversal. Only exit step
		/// (TraversalEpilogueCallbackReason)is relevant in this case</param>
		/// <param name="kind">syntax rule production index relative to syntax rule 'expr'</param>
		/// <param name="p_expr">reference to an instance of syntax tree node associated with 'expr' syntax rule</param>
		/// <returns>always true: continue traversal</returns>
		private static bool Invoke_expr_Callback (SyntaxTreeCallbackReason reason, expr.production_kind kind, expr p_expr)
		{
			bool status = true;
			switch (reason)
			{
			case SyntaxTreeCallbackReason.BuilderCallbackReason:
				break;
			case SyntaxTreeCallbackReason.TraversalPrologueCallbackReason:
				break;
			case SyntaxTreeCallbackReason.TraversalMidTermCallbackReason:
				break;
			case SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason:
				{
					p_expr.appInfo = p_expr.m_plus_expr.appInfo;
				}
				break;
			}
			return status;
		}

		/// <summary>
		/// syntax tree traversal routine operating on syntax tree nodes associated with syntax tree rule 'plus-expr'
		/// This expression consists of three productions. First one represents arithmetic operations with higher
		/// priorities, others represent addition and subtraction. The result, stored in application info asociated
		/// with syntax tree node, reflects the fact described above
		/// </summary>
		/// <param name="reason">reflects step of syntax tree traversal</param>
		/// <param name="kind">syntax rule production index relative to syntax rule 'plus-expr'</param>
		/// <param name="p_plus_expr">reference to an instance of syntax tree node associated with 'plus-expr' syntax rule</param>
		/// <returns>always true: continue traversal</returns>
		private static bool Invoke_plus_expr_Callback (SyntaxTreeCallbackReason reason, plus_expr.production_kind kind, plus_expr p_plus_expr)
		{
			bool status = true;
			switch (reason)
			{
			case SyntaxTreeCallbackReason.BuilderCallbackReason:
				break;
			case SyntaxTreeCallbackReason.TraversalPrologueCallbackReason:
				break;
			case SyntaxTreeCallbackReason.TraversalMidTermCallbackReason:
				break;
			case SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason:
				{
					switch (kind)
					{
					case plus_expr.production_kind.g_plus_expr_1:
						p_plus_expr.appInfo = p_plus_expr.m_mul_expr.appInfo;
						break;
					case plus_expr.production_kind.g_plus_expr_2:
						p_plus_expr.appInfo = ((int) p_plus_expr.m_plus_expr.appInfo + (int) p_plus_expr.m_mul_expr.appInfo);
						break;
					case plus_expr.production_kind.g_plus_expr_3:
						p_plus_expr.appInfo = ((int) p_plus_expr.m_plus_expr.appInfo - (int) p_plus_expr.m_mul_expr.appInfo);
						break;
					}
				}
				break;
			}
			return status;
		}

		/// <summary>
		/// syntax tree traversal routine operating on syntax tree nodes associated with syntax tree rule 'mul-expr'
		/// This expression consists of three productions. First one represents arithmetic operations with constants
		/// or higher priorities, others represent multiplication and division. The result, stored in application info
		/// asociated with syntax tree node, reflects the fact described above.
		/// </summary>
		/// <param name="reason">reflects step of syntax tree traversal</param>
		/// <param name="kind">syntax rule production index relative to syntax rule 'mul-expr'</param>
		/// <param name="p_mul_expr">reference to an instance of syntax tree node associated with 'mul-expr' syntax rule</param>
		/// <returns>always true: continue traversal</returns>
		private static bool Invoke_mul_expr_Callback (SyntaxTreeCallbackReason reason, mul_expr.production_kind kind, mul_expr p_mul_expr)
		{
			bool status = true;
			switch (reason)
			{
			case SyntaxTreeCallbackReason.BuilderCallbackReason:
				break;
			case SyntaxTreeCallbackReason.TraversalPrologueCallbackReason:
				break;
			case SyntaxTreeCallbackReason.TraversalMidTermCallbackReason:
				break;
			case SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason:
				{
					switch (kind)
					{
					case mul_expr.production_kind.g_mul_expr_1:
						p_mul_expr.appInfo = p_mul_expr.m_simple_expr.appInfo;
						break;
					case mul_expr.production_kind.g_mul_expr_2:
						p_mul_expr.appInfo = ((int) p_mul_expr.m_mul_expr.appInfo * (int) p_mul_expr.m_simple_expr.appInfo);
						break;
					case mul_expr.production_kind.g_mul_expr_3:
						p_mul_expr.appInfo = ((int) p_mul_expr.m_mul_expr.appInfo / (int) p_mul_expr.m_simple_expr.appInfo);
						break;
					}
				}
				break;
			}
			return status;
		}

		/// <summary>
		/// syntax tree traversal routine operating on syntax tree nodes associated with syntax tree rule 'simple-expr'.
		/// This expression represents numerical constants (integers in this example) or higher order operations
		/// enclosed with round braces. Values of numerical constants are retrieved by invoking int.Parse() method
		/// on text property of syntax tree node associated with this constant, while the valuse of higher order
		/// expressions are simply copied from underlying subtree.
		/// </summary>
		/// <param name="reason">reflects step of syntax tree traversal</param>
		/// <param name="kind">syntax rule production index relative to syntax rule 'mul-expr'</param>
		/// <param name="p_simple_expr">reference to an instance of syntax tree node associated with 'simple-expr' syntax rule</param>
		/// <returns>always true: continue traversal</returns>
		private static bool Invoke_simple_expr_Callback (SyntaxTreeCallbackReason reason, simple_expr.production_kind kind, simple_expr p_simple_expr)
		{
			bool status = true;
			switch (reason)
			{
			case SyntaxTreeCallbackReason.BuilderCallbackReason:
				break;
			case SyntaxTreeCallbackReason.TraversalPrologueCallbackReason:
				break;
			case SyntaxTreeCallbackReason.TraversalMidTermCallbackReason:
				break;
			case SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason:
				{
					switch (kind)
					{
					case simple_expr.production_kind.g_simple_expr_1:
						p_simple_expr.appInfo = int.Parse (p_simple_expr.m_NUMBER.text);
						break;
					case simple_expr.production_kind.g_simple_expr_2:
						p_simple_expr.appInfo = p_simple_expr.m_expr.appInfo;
						break;
					}
				}
				break;
			}
			return status;
		}

    /// <summary>
    /// error callback routine. syntax errors are reported on console window
    /// </summary>
    /// <param name="lineno">line number of syntax error</param>
    /// <param name="column">column of syntax error</param>
    /// <param name="terminal">terminal representation of text</param>
    /// <param name="terminalString">text causing syntax error</param>
    /// <returns>always true: do not recover syntax errors</returns>
    private static bool Invoke_Error_Callback (int lineno, int column, int terminal, string terminalString)
		{
			Console.WriteLine ("ERROR, line: " + lineno + ", column: " + column + ", terminal: " + terminal + ", text: " + terminalString);
			return true;
		}
	}
}
