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
	// class associated with syntax rule <delimiter>
	//

	public class	_delimiter_ : SyntaxTreeBase
	{
		#region enumerated production(s) of syntax rule <delimiter>
		public enum production_kind : ushort	// enumerated production(s) of syntax rule <delimiter>
		{
			g__delimiter__1 = 1,	// '[' <anglr nested rule> ']'

		};
		#endregion
		#region production markers associated with the syntax rule <delimiter>

		// markers associated with production: <delimiter> -> '[' <anglr nested rule> ']'

		public enum production_marker_1 : ushort
		{
			m__anglr_nested_rule_,
			final
		};
		#endregion
		#region Constructors and destructors associated with the syntax rule <delimiter>

		// Constructor declaration(s) associated with production(s) of syntax rule <delimiter>

		//
		// Constructor associated with the following production(s)
		// <delimiter> -> '[' <anglr nested rule> ']'

		//

		public _delimiter_ (SyntaxTreeToken p_token, _anglr_nested_rule_ p__anglr_nested_rule_, SyntaxTreeToken p_token_1) : base ((uint) ProductionID.__delimiter__ID, (uint) production_kind.g__delimiter__1)
		{
			++g_counter;
			_init ();
			children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 3);
			children[0] = m__left_square_bracket_ = p_token;
			children[1] = m__anglr_nested_rule_ = p__anglr_nested_rule_;
			children[2] = m__right_square_bracket_ = p_token_1;
		}

		// Copy constructor

		public _delimiter_ (_delimiter_ p__delimiter_) : base (p__delimiter_.id, p__delimiter_.kind)
		{
			++g_counter;
			_init ();
			switch ((production_kind) this.kind)
			{
			case production_kind.g__delimiter__1:
				children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 3);
				children[0] = m__left_square_bracket_ = p__delimiter_.m__left_square_bracket_;
				children[1] = m__anglr_nested_rule_ = p__delimiter_.m__anglr_nested_rule_;
				children[2] = m__right_square_bracket_ = p__delimiter_.m__right_square_bracket_;
				break;
			default:
				string[] args = new string[] { "_delimiter_" };
				throw new SyntaxTreeError (SyntaxTreeError.InvalidKindError, args);
			}
		}

		// Clone node - wrapper of copy constructor

		public override SyntaxTreeBase Clone () { return (SyntaxTreeBase) new _delimiter_ (this); }

		// Destructor

		public new void Dispose ()
		{
			base.Dispose ();
			--g_counter;

			m__left_square_bracket_?.Dispose ();
			m__anglr_nested_rule_?.Dispose ();
			m__right_square_bracket_?.Dispose ();

			_init ();
		}
		#endregion
		#region Content changing and inclusion checking methods associated with the syntax rule <delimiter>

		// Content changing function(s) associated with production(s) of syntax rule <delimiter>

		//
		// Content changing function associated with following production(s)
		// <delimiter> -> '[' <anglr nested rule> ']'

		//

		public void change(SyntaxTreeToken p_token, _anglr_nested_rule_ p__anglr_nested_rule_, SyntaxTreeToken p_token_1)
		{
			_init ();
			this.kind = (uint) production_kind.g__delimiter__1;
			children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 3);
			children [0] = m__left_square_bracket_ = p_token;
			children [1] = m__anglr_nested_rule_ = p__anglr_nested_rule_;
			children [2] = m__right_square_bracket_ = p_token_1;
			reparent (parent);
		}

		//
		// check if node 'element' is included within syntax tree associated with syntax rule <delimiter>
		//

		public new bool checkInclusion (SyntaxTreeBase element)
		{
			return
				(element == this) ||
				(m__left_square_bracket_ != null) && m__left_square_bracket_.checkInclusion (element) ||
				(m__anglr_nested_rule_ != null) && m__anglr_nested_rule_.checkInclusion (element) ||
				(m__right_square_bracket_ != null) && m__right_square_bracket_.checkInclusion (element) ||
				false;
		}
		#endregion
		#region Syntax tree emit methods associated with the syntax rule <delimiter>

		//
		// emit production tree node associated with any production of syntax rule <delimiter>
		//

		public override string Emit (int depth)
		{
			string s = "";
			if (depth != 0)
			switch ((production_kind) kind)
			{
				case production_kind.g__delimiter__1:
					// emit syntax tree node associated with production
					// <delimiter>: '[' <anglr nested rule> ']'

					s += m__left_square_bracket_.Emit (depth - 1);
					s += " ";
					s += m__anglr_nested_rule_.Emit (depth - 1);
					s += " ";
					s += m__right_square_bracket_.Emit (depth - 1);
				break;
			}
			return s.Trim ();
		}

		//
		// emit production tree node associated with any production of syntax rule <delimiter>
		//

		public override string EmitProductionTree (int depth)
		{
			string s = "_delimiter_ (";
			if (depth != 0)
				switch ((production_kind) kind)
				{
				case production_kind.g__delimiter__1:
					// emit syntax tree node associated with production
					// <delimiter>: '[' <anglr nested rule> ']'

					s += "_left_square_bracket_";
					s += ' ';
					s += m__anglr_nested_rule_.EmitProductionTree (depth - 1);
					s += ' ';
					s += "_right_square_bracket_";
				break;
			}
			s += ")";
			return s;
		}
		#endregion
		#region Syntax tree traversal,  reperrent, init and iterator methods associated with the syntax rule <delimiter>

		//
		// traverse sub-tree rooted in this node using selected syntax tree walker
		//

		public override void InvokeTraverse (SyntaxTreeWalkerCore syntaxTreeWalker) => ((SyntaxTreeWalker) syntaxTreeWalker).Traverse (this);
		public override void InvokeTraverseCommon (SyntaxTreeWalkerCore syntaxTreeWalker) => ((SyntaxTreeWalker) syntaxTreeWalker).TraverseCommon (this);

		// reparent sub-tree

		public override void reparent (SyntaxTreeBase parent)
		{
			this.parent = parent;

			m__left_square_bracket_?.reparent (this);
			m__anglr_nested_rule_?.reparent (this);
			m__right_square_bracket_?.reparent (this);
		}

		//
		// initialize object associated with syntax rule <delimiter>
		//

		public void _init ()
		{
			m__left_square_bracket_ = null;
			m__anglr_nested_rule_ = null;
			m__right_square_bracket_ = null;
		}
		#endregion
		#region Fields and properties associated with the syntax rule <delimiter>

		// counter of all nodes associated with syntax rule <delimiter>
		public static int g_counter;

		// objects associated with terminal and non-terminal symbols within production(s) of syntax rule <delimiter>
		public SyntaxTreeToken m__left_square_bracket_ { get; private set; }
		public _anglr_nested_rule_ m__anglr_nested_rule_ { get; private set; }
		public SyntaxTreeToken m__right_square_bracket_ { get; private set; }
		#endregion

	};

	public partial class SyntaxTreeWalker
	{
		#region delegates and events associated with the syntax rule <delimiter>

		// delegate function (callback) prototype associated with syntax rule <delimiter>
		public delegate bool _delimiter__Callback (SyntaxTreeCallbackReason reason, _delimiter_.production_kind kind, _delimiter_ p__delimiter_);

		// event associated with syntax rule <delimiter>
		public event _delimiter__Callback _delimiter__Event;

		// event trigger associated with syntax rule <delimiter>
		public bool Raise__delimiter__Event (SyntaxTreeCallbackReason reason, _delimiter_.production_kind kind, _delimiter_ p__delimiter_)
		{
			bool? status = _delimiter__Event?.Invoke (reason, kind, p__delimiter_);
			return (status == null) || status.Value;
		}
		#endregion
		#region syntax tree traversal methods associated with the syntax rule <delimiter>
		//
		// traverse syntax tree node associated with any production of syntax rule <delimiter>
		//

		public void Traverse (_delimiter_ p__delimiter_)
		{
			if (p__delimiter_.isLocked())
				return;
			p__delimiter_.dolock();
			_delimiter_.production_kind kind = (_delimiter_.production_kind) p__delimiter_.kind;
			p__delimiter_.turn_reset ();
			if (Raise__delimiter__Event (SyntaxTreeCallbackReason.TraversalPrologueCallbackReason, kind, p__delimiter_))
			switch (kind)
			{
				case _delimiter_.production_kind.g__delimiter__1:
					// traverse syntax tree node associated with production
					// <delimiter>: '[' <anglr nested rule> ']'

					if (Raise__delimiter__Event (SyntaxTreeCallbackReason.TraversalMidTermCallbackReason, kind, p__delimiter_))
						Traverse (p__delimiter_.m__anglr_nested_rule_);
					p__delimiter_.turn_inc ();
					Raise__delimiter__Event (SyntaxTreeCallbackReason.TraversalMidTermCallbackReason, kind, p__delimiter_);
				break;
			}
			Raise__delimiter__Event (SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason, kind, p__delimiter_);
			p__delimiter_.unlock();
		}

		//
		// traverse syntax tree node associated with any production of syntax rule <delimiter>
		//

		public void TraverseCommon (_delimiter_ p__delimiter_)
		{
			_delimiter_.production_kind kind = (_delimiter_.production_kind) p__delimiter_.kind;
			if (Raise_Common_Event (SyntaxTreeCallbackReason.TraversalPrologueCallbackReason, (int) kind, p__delimiter_))
			switch (kind)
			{
				case _delimiter_.production_kind.g__delimiter__1:
					// traverse syntax tree node associated with production
					// <delimiter>: '[' <anglr nested rule> ']'

						TraverseCommon (p__delimiter_.m__left_square_bracket_);
						TraverseCommon (p__delimiter_.m__anglr_nested_rule_);
						TraverseCommon (p__delimiter_.m__right_square_bracket_);
				break;
			}
			Raise_Common_Event (SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason, (int) kind, p__delimiter_);
		}
	};
		#endregion

	public partial class SyntaxTreeBuilder : SyntaxTreeWalker
	{
		#region syntax tree builder methods associated with the syntax rule <delimiter>

		//
		// create syntax tree node associated with production
		// <delimiter>: '[' <anglr nested rule> ']'

		//

		public _delimiter_ _delimiter__1 (SyntaxTreeToken p_token, _anglr_nested_rule_ p__anglr_nested_rule_, SyntaxTreeToken p_token_1)
		{
			_delimiter_ p__delimiter__ref = new _delimiter_(p_token, p__anglr_nested_rule_, p_token_1);
			p_token.parent = p__delimiter__ref;
			p__anglr_nested_rule_.parent = p__delimiter__ref;
			p_token_1.parent = p__delimiter__ref;
			Raise__delimiter__Event (SyntaxTreeCallbackReason.BuilderCallbackReason, _delimiter_.production_kind.g__delimiter__1, p__delimiter__ref);
			return p__delimiter__ref;
		}
		#endregion
	};
}
