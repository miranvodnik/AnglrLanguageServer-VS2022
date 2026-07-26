//
//	This file was generated with ANGLR compiler
//
using System;

using Anglr.Parser.Core;
using Anglr.Parser.SyntaxTree;
using Anglr.Parser.Walker;
namespace Anglr.Parser
{
	//
	// class associated with syntax rule <regex definition>
	//

	public class	_regex_definition_ : SyntaxTreeBase
	{
		#region enumerated production(s) of syntax rule <regex definition>
		public enum production_kind : ushort	// enumerated production(s) of syntax rule <regex definition>
		{
			g__regex_definition__1 = 1,	// <identifier> <regular expression>

		};
		#endregion
		#region production markers associated with the syntax rule <regex definition>

		// markers associated with production: <regex definition> -> <identifier> <regular expression>

		public enum production_marker_1 : ushort
		{
			final
		};
		#endregion
		#region Constructors and destructors associated with the syntax rule <regex definition>

		// Constructor declaration(s) associated with production(s) of syntax rule <regex definition>

		//
		// Constructor associated with the following production(s)
		// <regex definition> -> <identifier> <regular expression>

		//

		public _regex_definition_ (SyntaxTreeToken p_token, SyntaxTreeToken p_token_1) : base ((uint) ProductionID.__regex_definition__ID, (uint) production_kind.g__regex_definition__1)
		{
			++g_counter;
			_init ();
			children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 2);
			children[0] = m__identifier_ = p_token;
			children[1] = m__regular_expression_ = p_token_1;
		}

		// Copy constructor

		public _regex_definition_ (_regex_definition_ p__regex_definition_) : base (p__regex_definition_.id, p__regex_definition_.kind)
		{
			++g_counter;
			_init ();
			switch ((production_kind) p__regex_definition_.kind)
			{
				case production_kind.g__regex_definition__1:
					children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 2);
					if ((children [0] = m__identifier_ = (p__regex_definition_.m__identifier_ != null) ? new SyntaxTreeToken (p__regex_definition_.m__identifier_) : null) != null) m__identifier_.parent = this;
					if ((children [1] = m__regular_expression_ = (p__regex_definition_.m__regular_expression_ != null) ? new SyntaxTreeToken (p__regex_definition_.m__regular_expression_) : null) != null) m__regular_expression_.parent = this;
					break;
				default:
				{
					string[] args = new string[] { "_regex_definition_" };
					throw new SyntaxTreeError (SyntaxTreeError.InvalidKindError, args);
				}
			}
		}

		// Clone node - wrapper of copy constructor

		public override SyntaxTreeBase Clone () { return (SyntaxTreeBase) new _regex_definition_ (this); }

		// Destructor

		public new void Dispose ()
		{
			base.Dispose ();
			--g_counter;

			m__identifier_?.Dispose ();
			m__regular_expression_?.Dispose ();

			_init ();
		}
		#endregion
		#region Content changing and inclusion checking methods associated with the syntax rule <regex definition>

		// Content changing function(s) associated with production(s) of syntax rule <regex definition>

		//
		// Content changing function associated with following production(s)
		// <regex definition> -> <identifier> <regular expression>

		//

		public void change(SyntaxTreeToken p_token, SyntaxTreeToken p_token_1)
		{
			_init ();
			this.kind = (uint) production_kind.g__regex_definition__1;
			children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 2);
			children [0] = m__identifier_ = p_token;
			children [1] = m__regular_expression_ = p_token_1;
			reparent (parent);
		}

		//
		// check if node 'element' is included within syntax tree associated with syntax rule <regex definition>
		//

		public new bool checkInclusion (SyntaxTreeBase element)
		{
			return
				(element == this) ||
				(m__identifier_ != null) && m__identifier_.checkInclusion (element) ||
				(m__regular_expression_ != null) && m__regular_expression_.checkInclusion (element) ||
				false;
		}
		#endregion
		#region Syntax tree emit methods associated with the syntax rule <regex definition>

		//
		// emit production tree node associated with any production of syntax rule <regex definition>
		//

		public override string Emit (int depth)
		{
			string s = "";
			if (depth != 0)
			switch ((production_kind) kind)
			{
				case production_kind.g__regex_definition__1:
					// emit syntax tree node associated with production
					// <regex definition>: <identifier> <regular expression>

					s += m__identifier_.Emit (depth - 1);
					s += " ";
					s += m__regular_expression_.Emit (depth - 1);
				break;
			}
			return s.Trim ();
		}

		//
		// emit production tree node associated with any production of syntax rule <regex definition>
		//

		public override string EmitProductionTree (int depth)
		{
			string s = "_regex_definition_ (";
			if (depth != 0)
				switch ((production_kind) kind)
				{
				case production_kind.g__regex_definition__1:
					// emit syntax tree node associated with production
					// <regex definition>: <identifier> <regular expression>

					s += "_identifier_";
					s += ' ';
					s += "_regular_expression_";
				break;
			}
			s += ")";
			return s;
		}
		#endregion
		#region Syntax tree traversal,  reperrent, init and iterator methods associated with the syntax rule <regex definition>

		//
		// traverse sub-tree rooted in this node using selected syntax tree walker
		//

		public override void InvokeTraverse (SyntaxTreeWalkerCore syntaxTreeWalker) => ((SyntaxTreeWalker) syntaxTreeWalker).Traverse (this);
		public override void InvokeTraverseCommon (SyntaxTreeWalkerCore syntaxTreeWalker) => ((SyntaxTreeWalker) syntaxTreeWalker).TraverseCommon (this);

		// reparent sub-tree

		public override void reparent (SyntaxTreeBase parent)
		{
			this.parent = parent;

			m__identifier_?.reparent (this);
			m__regular_expression_?.reparent (this);
		}

		//
		// initialize object associated with syntax rule <regex definition>
		//

		public void _init ()
		{
			m__identifier_ = null;
			m__regular_expression_ = null;
		}
		#endregion
		#region Fields and properties associated with the syntax rule <regex definition>

		// counter of all nodes associated with syntax rule <regex definition>
		public static int g_counter;

		// objects associated with terminal and non-terminal symbols within production(s) of syntax rule <regex definition>
		public SyntaxTreeToken m__identifier_ { get; private set; }
		public SyntaxTreeToken m__regular_expression_ { get; private set; }
		#endregion

	};

	public partial class SyntaxTreeWalker
	{
		#region delegates and events associated with the syntax rule <regex definition>

		// delegate function (callback) prototype associated with syntax rule <regex definition>
		public delegate bool _regex_definition__Callback (SyntaxTreeCallbackReason reason, _regex_definition_.production_kind kind, _regex_definition_ p__regex_definition_);

		// event associated with syntax rule <regex definition>
		public event _regex_definition__Callback _regex_definition__Event;

		// event trigger associated with syntax rule <regex definition>
		public bool Raise__regex_definition__Event (SyntaxTreeCallbackReason reason, _regex_definition_.production_kind kind, _regex_definition_ p__regex_definition_)
		{
			bool? status = _regex_definition__Event?.Invoke (reason, kind, p__regex_definition_);
			return (status == null) || status.Value;
		}
		#endregion
		#region syntax tree traversal methods associated with the syntax rule <regex definition>
		//
		// traverse syntax tree node associated with any production of syntax rule <regex definition>
		//

		public void Traverse (_regex_definition_ p__regex_definition_)
		{
			if (p__regex_definition_.isLocked())
				return;
			p__regex_definition_.dolock();
			_regex_definition_.production_kind kind = (_regex_definition_.production_kind) p__regex_definition_.kind;
			p__regex_definition_.turn_reset ();
			if (Raise__regex_definition__Event (SyntaxTreeCallbackReason.TraversalPrologueCallbackReason, kind, p__regex_definition_))
			switch (kind)
			{
				case _regex_definition_.production_kind.g__regex_definition__1:
					// traverse syntax tree node associated with production
					// <regex definition>: <identifier> <regular expression>

				break;
			}
			Raise__regex_definition__Event (SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason, kind, p__regex_definition_);
			p__regex_definition_.unlock();
		}

		//
		// traverse syntax tree node associated with any production of syntax rule <regex definition>
		//

		public void TraverseCommon (_regex_definition_ p__regex_definition_)
		{
			_regex_definition_.production_kind kind = (_regex_definition_.production_kind) p__regex_definition_.kind;
			if (Raise_Common_Event (SyntaxTreeCallbackReason.TraversalPrologueCallbackReason, (int) kind, p__regex_definition_))
			switch (kind)
			{
				case _regex_definition_.production_kind.g__regex_definition__1:
					// traverse syntax tree node associated with production
					// <regex definition>: <identifier> <regular expression>

						TraverseCommon (p__regex_definition_.m__identifier_);
						TraverseCommon (p__regex_definition_.m__regular_expression_);
				break;
			}
			Raise_Common_Event (SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason, (int) kind, p__regex_definition_);
		}
	};
		#endregion

	public partial class SyntaxTreeBuilder : SyntaxTreeWalker
	{
		#region syntax tree builder methods associated with the syntax rule <regex definition>

		//
		// create syntax tree node associated with production
		// <regex definition>: <identifier> <regular expression>

		//

		public _regex_definition_ _regex_definition__1 (SyntaxTreeToken p_token, SyntaxTreeToken p_token_1)
		{
			_regex_definition_ p__regex_definition__ref = new _regex_definition_(p_token, p_token_1);
			p_token.parent = p__regex_definition__ref;
			p_token_1.parent = p__regex_definition__ref;
			Raise__regex_definition__Event (SyntaxTreeCallbackReason.BuilderCallbackReason, _regex_definition_.production_kind.g__regex_definition__1, p__regex_definition__ref);
			return p__regex_definition__ref;
		}
		#endregion
	};
}
