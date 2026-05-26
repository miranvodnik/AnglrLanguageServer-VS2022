//
//	This file was generated with ANGLR compiler
//
using Anglr.Parser.Core;
using Anglr.Parser.SyntaxTree;
namespace Anglr.Parser.Walker
{
	public partial class SyntaxTreeWalkerCore
	{
		public SyntaxTreeWalkerCore () { }

		public delegate bool SyntaxTreeToken_Callback (SyntaxTreeToken syntaxTreeToken);
		public delegate bool Common_Callback (SyntaxTreeCallbackReason reason, int kind, SyntaxTreeBase p_node);
		public delegate bool Error_Callback (int lineno, int column, int token, string tokenString);

		public event SyntaxTreeToken_Callback syntaxTreeToken_Event;
		public event Common_Callback Common_Event;
		public event Error_Callback Error_Event;

		public bool Raise_SyntaxTreeToken_Callback (SyntaxTreeToken syntaxTreeToken)
		{
			bool? status = syntaxTreeToken_Event?.Invoke (syntaxTreeToken);
			return (status == null) || status.Value;
		}

		public bool Raise_Common_Event (SyntaxTreeCallbackReason reason, int kind, SyntaxTreeBase p_node)
		{
			bool? status = Common_Event?.Invoke (reason, kind, p_node);
			return (status == null) || status.Value;
		}

		public bool Raise_Error_Event (int lineno, int column, int token, string tokenString)
		{
			bool? status = Error_Event?.Invoke (lineno, column, token, tokenString);
			return (status != null) && status.Value;
		}

		public void Traverse (SyntaxTreeToken syntaxTreeToken)
		{
			Raise_SyntaxTreeToken_Callback (syntaxTreeToken);
		}

		public void TraverseCommon (SyntaxTreeToken p_token)
		{
			Raise_Common_Event (SyntaxTreeCallbackReason.TraversalPrologueCallbackReason, 0, p_token);
			Raise_Common_Event (SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason, 0, p_token);
		}

		public SyntaxTreeBase GetParentById (SyntaxTreeBase node, uint id)
		{
			SyntaxTreeBase parent;
			for (parent = node; (parent != null) && (parent.id != id); parent = parent.parent);
			return parent;
		}
	}
}
