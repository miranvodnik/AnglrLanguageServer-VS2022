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
	// class associated with syntax rule <scanner part>
	//

	public class	_scanner_part_ : SyntaxTreeBase
	{
		#region enumerated production(s) of syntax rule <scanner part>
		public enum production_kind : ushort	// enumerated production(s) of syntax rule <scanner part>
		{
			g__scanner_part__1 = 1,	// <attribute list optional> '%scanner' <identifier> '%{' <regular expression list optional> '%}'

		};
		#endregion
		#region production markers associated with the syntax rule <scanner part>

		// markers associated with production: <scanner part> -> <attribute list optional> '%scanner' <identifier> '%{' <regular expression list optional> '%}'

		public enum production_marker_1 : ushort
		{
			m__attribute_list_optional_,
			m__regular_expression_list_optional_,
			final
		};
		#endregion
		#region Constructors and destructors associated with the syntax rule <scanner part>

		// Constructor declaration(s) associated with production(s) of syntax rule <scanner part>

		//
		// Constructor associated with the following production(s)
		// <scanner part> -> <attribute list optional> '%scanner' <identifier> '%{' <regular expression list optional> '%}'

		//

		public _scanner_part_ (_attribute_list_optional_ p__attribute_list_optional_, SyntaxTreeToken p_token, SyntaxTreeToken p_token_1, SyntaxTreeToken p_token_2, _regular_expression_list_optional_ p__regular_expression_list_optional_, SyntaxTreeToken p_token_3) : base ((uint) ProductionID.__scanner_part__ID, (uint) production_kind.g__scanner_part__1)
		{
			++g_counter;
			_init ();
			children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 6);
			children[0] = m__attribute_list_optional_ = p__attribute_list_optional_;
			children[1] = m__scanner_ = p_token;
			children[2] = m__identifier_ = p_token_1;
			children[3] = m__left_part_bracket_ = p_token_2;
			children[4] = m__regular_expression_list_optional_ = p__regular_expression_list_optional_;
			children[5] = m__right_part_bracket_ = p_token_3;
		}

		// Copy constructor

		public _scanner_part_ (_scanner_part_ p__scanner_part_) : base (p__scanner_part_.id, p__scanner_part_.kind)
		{
			++g_counter;
			_init ();
			switch ((production_kind) this.kind)
			{
			case production_kind.g__scanner_part__1:
				children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 6);
				children[0] = m__attribute_list_optional_ = p__scanner_part_.m__attribute_list_optional_;
				children[1] = m__scanner_ = p__scanner_part_.m__scanner_;
				children[2] = m__identifier_ = p__scanner_part_.m__identifier_;
				children[3] = m__left_part_bracket_ = p__scanner_part_.m__left_part_bracket_;
				children[4] = m__regular_expression_list_optional_ = p__scanner_part_.m__regular_expression_list_optional_;
				children[5] = m__right_part_bracket_ = p__scanner_part_.m__right_part_bracket_;
				break;
			default:
				string[] args = new string[] { "_scanner_part_" };
				throw new SyntaxTreeError (SyntaxTreeError.InvalidKindError, args);
			}
		}

		// Clone node - wrapper of copy constructor

		public override SyntaxTreeBase Clone () { return (SyntaxTreeBase) new _scanner_part_ (this); }

		// Destructor

		public new void Dispose ()
		{
			base.Dispose ();
			--g_counter;

			m__attribute_list_optional_?.Dispose ();
			m__scanner_?.Dispose ();
			m__identifier_?.Dispose ();
			m__left_part_bracket_?.Dispose ();
			m__regular_expression_list_optional_?.Dispose ();
			m__right_part_bracket_?.Dispose ();

			_init ();
		}
		#endregion
		#region Content changing and inclusion checking methods associated with the syntax rule <scanner part>

		// Content changing function(s) associated with production(s) of syntax rule <scanner part>

		//
		// Content changing function associated with following production(s)
		// <scanner part> -> <attribute list optional> '%scanner' <identifier> '%{' <regular expression list optional> '%}'

		//

		public void change(_attribute_list_optional_ p__attribute_list_optional_, SyntaxTreeToken p_token, SyntaxTreeToken p_token_1, SyntaxTreeToken p_token_2, _regular_expression_list_optional_ p__regular_expression_list_optional_, SyntaxTreeToken p_token_3)
		{
			_init ();
			this.kind = (uint) production_kind.g__scanner_part__1;
			children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 6);
			children [0] = m__attribute_list_optional_ = p__attribute_list_optional_;
			children [1] = m__scanner_ = p_token;
			children [2] = m__identifier_ = p_token_1;
			children [3] = m__left_part_bracket_ = p_token_2;
			children [4] = m__regular_expression_list_optional_ = p__regular_expression_list_optional_;
			children [5] = m__right_part_bracket_ = p_token_3;
			reparent (parent);
		}

		//
		// check if node 'element' is included within syntax tree associated with syntax rule <scanner part>
		//

		public new bool checkInclusion (SyntaxTreeBase element)
		{
			return
				(element == this) ||
				(m__attribute_list_optional_ != null) && m__attribute_list_optional_.checkInclusion (element) ||
				(m__scanner_ != null) && m__scanner_.checkInclusion (element) ||
				(m__identifier_ != null) && m__identifier_.checkInclusion (element) ||
				(m__left_part_bracket_ != null) && m__left_part_bracket_.checkInclusion (element) ||
				(m__regular_expression_list_optional_ != null) && m__regular_expression_list_optional_.checkInclusion (element) ||
				(m__right_part_bracket_ != null) && m__right_part_bracket_.checkInclusion (element) ||
				false;
		}
		#endregion
		#region Syntax tree emit methods associated with the syntax rule <scanner part>

		//
		// emit production tree node associated with any production of syntax rule <scanner part>
		//

		public override string Emit (int depth)
		{
			string s = "";
			if (depth != 0)
			switch ((production_kind) kind)
			{
				case production_kind.g__scanner_part__1:
					// emit syntax tree node associated with production
					// <scanner part>: <attribute list optional> '%scanner' <identifier> '%{' <regular expression list optional> '%}'

					s += m__attribute_list_optional_.Emit (depth - 1);
					s += " ";
					s += m__scanner_.Emit (depth - 1);
					s += " ";
					s += m__identifier_.Emit (depth - 1);
					s += " ";
					s += m__left_part_bracket_.Emit (depth - 1);
					s += " ";
					s += m__regular_expression_list_optional_.Emit (depth - 1);
					s += " ";
					s += m__right_part_bracket_.Emit (depth - 1);
				break;
			}
			return s.Trim ();
		}

		//
		// emit production tree node associated with any production of syntax rule <scanner part>
		//

		public override string EmitProductionTree (int depth)
		{
			string s = "_scanner_part_ (";
			if (depth != 0)
				switch ((production_kind) kind)
				{
				case production_kind.g__scanner_part__1:
					// emit syntax tree node associated with production
					// <scanner part>: <attribute list optional> '%scanner' <identifier> '%{' <regular expression list optional> '%}'

					s += m__attribute_list_optional_.EmitProductionTree (depth - 1);
					s += ' ';
					s += "_scanner_";
					s += ' ';
					s += "_identifier_";
					s += ' ';
					s += "_left_part_bracket_";
					s += ' ';
					s += m__regular_expression_list_optional_.EmitProductionTree (depth - 1);
					s += ' ';
					s += "_right_part_bracket_";
				break;
			}
			s += ")";
			return s;
		}
		#endregion
		#region Syntax tree traversal,  reperrent, init and iterator methods associated with the syntax rule <scanner part>

		//
		// traverse sub-tree rooted in this node using selected syntax tree walker
		//

		public override void InvokeTraverse (SyntaxTreeWalkerCore syntaxTreeWalker) => ((SyntaxTreeWalker) syntaxTreeWalker).Traverse (this);
		public override void InvokeTraverseCommon (SyntaxTreeWalkerCore syntaxTreeWalker) => ((SyntaxTreeWalker) syntaxTreeWalker).TraverseCommon (this);

		// reparent sub-tree

		public override void reparent (SyntaxTreeBase parent)
		{
			this.parent = parent;

			m__attribute_list_optional_?.reparent (this);
			m__scanner_?.reparent (this);
			m__identifier_?.reparent (this);
			m__left_part_bracket_?.reparent (this);
			m__regular_expression_list_optional_?.reparent (this);
			m__right_part_bracket_?.reparent (this);
		}

		//
		// initialize object associated with syntax rule <scanner part>
		//

		public void _init ()
		{
			m__attribute_list_optional_ = null;
			m__scanner_ = null;
			m__identifier_ = null;
			m__left_part_bracket_ = null;
			m__regular_expression_list_optional_ = null;
			m__right_part_bracket_ = null;
		}
		#endregion
		#region Fields and properties associated with the syntax rule <scanner part>

		// counter of all nodes associated with syntax rule <scanner part>
		public static int g_counter;

		// objects associated with terminal and non-terminal symbols within production(s) of syntax rule <scanner part>
		public _attribute_list_optional_ m__attribute_list_optional_ { get; private set; }
		public SyntaxTreeToken m__scanner_ { get; private set; }
		public SyntaxTreeToken m__identifier_ { get; private set; }
		public SyntaxTreeToken m__left_part_bracket_ { get; private set; }
		public _regular_expression_list_optional_ m__regular_expression_list_optional_ { get; private set; }
		public SyntaxTreeToken m__right_part_bracket_ { get; private set; }
		#endregion

	};

	public partial class SyntaxTreeWalker
	{
		#region delegates and events associated with the syntax rule <scanner part>

		// delegate function (callback) prototype associated with syntax rule <scanner part>
		public delegate bool _scanner_part__Callback (SyntaxTreeCallbackReason reason, _scanner_part_.production_kind kind, _scanner_part_ p__scanner_part_);

		// event associated with syntax rule <scanner part>
		public event _scanner_part__Callback _scanner_part__Event;

		// event trigger associated with syntax rule <scanner part>
		public bool Raise__scanner_part__Event (SyntaxTreeCallbackReason reason, _scanner_part_.production_kind kind, _scanner_part_ p__scanner_part_)
		{
			bool? status = _scanner_part__Event?.Invoke (reason, kind, p__scanner_part_);
			return (status == null) || status.Value;
		}
		#endregion
		#region syntax tree traversal methods associated with the syntax rule <scanner part>
		//
		// traverse syntax tree node associated with any production of syntax rule <scanner part>
		//

		public void Traverse (_scanner_part_ p__scanner_part_)
		{
			if (p__scanner_part_.isLocked())
				return;
			p__scanner_part_.dolock();
			_scanner_part_.production_kind kind = (_scanner_part_.production_kind) p__scanner_part_.kind;
			p__scanner_part_.turn_reset ();
			if (Raise__scanner_part__Event (SyntaxTreeCallbackReason.TraversalPrologueCallbackReason, kind, p__scanner_part_))
			switch (kind)
			{
				case _scanner_part_.production_kind.g__scanner_part__1:
					// traverse syntax tree node associated with production
					// <scanner part>: <attribute list optional> '%scanner' <identifier> '%{' <regular expression list optional> '%}'

					if (Raise__scanner_part__Event (SyntaxTreeCallbackReason.TraversalMidTermCallbackReason, kind, p__scanner_part_))
						Traverse (p__scanner_part_.m__attribute_list_optional_);
					p__scanner_part_.turn_inc ();
					if (Raise__scanner_part__Event (SyntaxTreeCallbackReason.TraversalMidTermCallbackReason, kind, p__scanner_part_))
						Traverse (p__scanner_part_.m__regular_expression_list_optional_);
					p__scanner_part_.turn_inc ();
					Raise__scanner_part__Event (SyntaxTreeCallbackReason.TraversalMidTermCallbackReason, kind, p__scanner_part_);
				break;
			}
			Raise__scanner_part__Event (SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason, kind, p__scanner_part_);
			p__scanner_part_.unlock();
		}

		//
		// traverse syntax tree node associated with any production of syntax rule <scanner part>
		//

		public void TraverseCommon (_scanner_part_ p__scanner_part_)
		{
			_scanner_part_.production_kind kind = (_scanner_part_.production_kind) p__scanner_part_.kind;
			if (Raise_Common_Event (SyntaxTreeCallbackReason.TraversalPrologueCallbackReason, (int) kind, p__scanner_part_))
			switch (kind)
			{
				case _scanner_part_.production_kind.g__scanner_part__1:
					// traverse syntax tree node associated with production
					// <scanner part>: <attribute list optional> '%scanner' <identifier> '%{' <regular expression list optional> '%}'

						TraverseCommon (p__scanner_part_.m__attribute_list_optional_);
						TraverseCommon (p__scanner_part_.m__scanner_);
						TraverseCommon (p__scanner_part_.m__identifier_);
						TraverseCommon (p__scanner_part_.m__left_part_bracket_);
						TraverseCommon (p__scanner_part_.m__regular_expression_list_optional_);
						TraverseCommon (p__scanner_part_.m__right_part_bracket_);
				break;
			}
			Raise_Common_Event (SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason, (int) kind, p__scanner_part_);
		}
	};
		#endregion

	public partial class SyntaxTreeBuilder : SyntaxTreeWalker
	{
		#region syntax tree builder methods associated with the syntax rule <scanner part>

		//
		// create syntax tree node associated with production
		// <scanner part>: <attribute list optional> '%scanner' <identifier> '%{' <regular expression list optional> '%}'

		//

		public _scanner_part_ _scanner_part__1 (_attribute_list_optional_ p__attribute_list_optional_, SyntaxTreeToken p_token, SyntaxTreeToken p_token_1, SyntaxTreeToken p_token_2, _regular_expression_list_optional_ p__regular_expression_list_optional_, SyntaxTreeToken p_token_3)
		{
			_scanner_part_ p__scanner_part__ref = new _scanner_part_(p__attribute_list_optional_, p_token, p_token_1, p_token_2, p__regular_expression_list_optional_, p_token_3);
			p__attribute_list_optional_.parent = p__scanner_part__ref;
			p_token.parent = p__scanner_part__ref;
			p_token_1.parent = p__scanner_part__ref;
			p_token_2.parent = p__scanner_part__ref;
			p__regular_expression_list_optional_.parent = p__scanner_part__ref;
			p_token_3.parent = p__scanner_part__ref;
			Raise__scanner_part__Event (SyntaxTreeCallbackReason.BuilderCallbackReason, _scanner_part_.production_kind.g__scanner_part__1, p__scanner_part__ref);
			return p__scanner_part__ref;
		}
		#endregion
	};
}
