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
	// class associated with syntax rule <anglr syntax rule>
	//

	public class	_anglr_syntax_rule_ : SyntaxTreeBase
	{
		#region enumerated production(s) of syntax rule <anglr syntax rule>
		public enum production_kind : ushort	// enumerated production(s) of syntax rule <anglr syntax rule>
		{
			g__anglr_syntax_rule__1 = 1,	// <attribute list optional> <identifier> ':' <anglr syntax production list> ';'

			g__anglr_syntax_rule__2 = 2,	// <attribute list optional> <identifier> '{' <anglr syntax rule list optional> '}'

		};
		#endregion
		#region production markers associated with the syntax rule <anglr syntax rule>

		// markers associated with production: <anglr syntax rule> -> <attribute list optional> <identifier> ':' <anglr syntax production list> ';'

		public enum production_marker_1 : ushort
		{
			m__attribute_list_optional_,
			m__anglr_syntax_production_list_,
			final
		};

		// markers associated with production: <anglr syntax rule> -> <attribute list optional> <identifier> '{' <anglr syntax rule list optional> '}'

		public enum production_marker_2 : ushort
		{
			m__attribute_list_optional_,
			m__anglr_syntax_rule_list_optional_,
			final
		};
		#endregion
		#region Constructors and destructors associated with the syntax rule <anglr syntax rule>

		// Constructor declaration(s) associated with production(s) of syntax rule <anglr syntax rule>

		//
		// Constructor associated with the following production(s)
		// <anglr syntax rule> -> <attribute list optional> <identifier> ':' <anglr syntax production list> ';'

		//

		public _anglr_syntax_rule_ (_attribute_list_optional_ p__attribute_list_optional_, SyntaxTreeToken p_token, SyntaxTreeToken p_token_1, _anglr_syntax_production_list_ p__anglr_syntax_production_list_, SyntaxTreeToken p_token_2) : base ((uint) ProductionID.__anglr_syntax_rule__ID, (uint) production_kind.g__anglr_syntax_rule__1)
		{
			++g_counter;
			_init ();
			children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 5);
			children[0] = m__attribute_list_optional_ = p__attribute_list_optional_;
			children[1] = m__identifier_ = p_token;
			children[2] = m__colon_ = p_token_1;
			children[3] = m__anglr_syntax_production_list_ = p__anglr_syntax_production_list_;
			children[4] = m__semicolon_ = p_token_2;
		}

		//
		// Constructor associated with the following production(s)
		// <anglr syntax rule> -> <attribute list optional> <identifier> '{' <anglr syntax rule list optional> '}'

		//

		public _anglr_syntax_rule_ (_attribute_list_optional_ p__attribute_list_optional_, SyntaxTreeToken p_token, SyntaxTreeToken p_token_1, _anglr_syntax_rule_list_optional_ p__anglr_syntax_rule_list_optional_, SyntaxTreeToken p_token_2) : base ((uint) ProductionID.__anglr_syntax_rule__ID, (uint) production_kind.g__anglr_syntax_rule__2)
		{
			++g_counter;
			_init ();
			children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 5);
			children[0] = m__attribute_list_optional_ = p__attribute_list_optional_;
			children[1] = m__identifier_ = p_token;
			children[2] = m__left_curly_bracket_ = p_token_1;
			children[3] = m__anglr_syntax_rule_list_optional_ = p__anglr_syntax_rule_list_optional_;
			children[4] = m__right_curly_bracket_ = p_token_2;
		}

		// Copy constructor

		public _anglr_syntax_rule_ (_anglr_syntax_rule_ p__anglr_syntax_rule_) : base (p__anglr_syntax_rule_.id, p__anglr_syntax_rule_.kind)
		{
			++g_counter;
			_init ();
			switch ((production_kind) this.kind)
			{
			case production_kind.g__anglr_syntax_rule__1:
				children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 5);
				children[0] = m__attribute_list_optional_ = p__anglr_syntax_rule_.m__attribute_list_optional_;
				children[1] = m__identifier_ = p__anglr_syntax_rule_.m__identifier_;
				children[2] = m__colon_ = p__anglr_syntax_rule_.m__colon_;
				children[3] = m__anglr_syntax_production_list_ = p__anglr_syntax_rule_.m__anglr_syntax_production_list_;
				children[4] = m__semicolon_ = p__anglr_syntax_rule_.m__semicolon_;
				break;
			case production_kind.g__anglr_syntax_rule__2:
				children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 5);
				children[0] = m__attribute_list_optional_ = p__anglr_syntax_rule_.m__attribute_list_optional_;
				children[1] = m__identifier_ = p__anglr_syntax_rule_.m__identifier_;
				children[2] = m__left_curly_bracket_ = p__anglr_syntax_rule_.m__left_curly_bracket_;
				children[3] = m__anglr_syntax_rule_list_optional_ = p__anglr_syntax_rule_.m__anglr_syntax_rule_list_optional_;
				children[4] = m__right_curly_bracket_ = p__anglr_syntax_rule_.m__right_curly_bracket_;
				break;
			default:
				string[] args = new string[] { "_anglr_syntax_rule_" };
				throw new SyntaxTreeError (SyntaxTreeError.InvalidKindError, args);
			}
		}

		// Clone node - wrapper of copy constructor

		public override SyntaxTreeBase Clone () { return (SyntaxTreeBase) new _anglr_syntax_rule_ (this); }

		// Destructor

		public new void Dispose ()
		{
			base.Dispose ();
			--g_counter;

			m__attribute_list_optional_?.Dispose ();
			m__identifier_?.Dispose ();
			m__colon_?.Dispose ();
			m__anglr_syntax_production_list_?.Dispose ();
			m__semicolon_?.Dispose ();
			m__left_curly_bracket_?.Dispose ();
			m__anglr_syntax_rule_list_optional_?.Dispose ();
			m__right_curly_bracket_?.Dispose ();

			_init ();
		}
		#endregion
		#region Content changing and inclusion checking methods associated with the syntax rule <anglr syntax rule>

		// Content changing function(s) associated with production(s) of syntax rule <anglr syntax rule>

		//
		// Content changing function associated with following production(s)
		// <anglr syntax rule> -> <attribute list optional> <identifier> ':' <anglr syntax production list> ';'

		//

		public void change(_attribute_list_optional_ p__attribute_list_optional_, SyntaxTreeToken p_token, SyntaxTreeToken p_token_1, _anglr_syntax_production_list_ p__anglr_syntax_production_list_, SyntaxTreeToken p_token_2)
		{
			_init ();
			this.kind = (uint) production_kind.g__anglr_syntax_rule__1;
			children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 5);
			children [0] = m__attribute_list_optional_ = p__attribute_list_optional_;
			children [1] = m__identifier_ = p_token;
			children [2] = m__colon_ = p_token_1;
			children [3] = m__anglr_syntax_production_list_ = p__anglr_syntax_production_list_;
			children [4] = m__semicolon_ = p_token_2;
			reparent (parent);
		}

		//
		// Content changing function associated with following production(s)
		// <anglr syntax rule> -> <attribute list optional> <identifier> '{' <anglr syntax rule list optional> '}'

		//

		public void change(_attribute_list_optional_ p__attribute_list_optional_, SyntaxTreeToken p_token, SyntaxTreeToken p_token_1, _anglr_syntax_rule_list_optional_ p__anglr_syntax_rule_list_optional_, SyntaxTreeToken p_token_2)
		{
			_init ();
			this.kind = (uint) production_kind.g__anglr_syntax_rule__2;
			children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 5);
			children [0] = m__attribute_list_optional_ = p__attribute_list_optional_;
			children [1] = m__identifier_ = p_token;
			children [2] = m__left_curly_bracket_ = p_token_1;
			children [3] = m__anglr_syntax_rule_list_optional_ = p__anglr_syntax_rule_list_optional_;
			children [4] = m__right_curly_bracket_ = p_token_2;
			reparent (parent);
		}

		//
		// check if node 'element' is included within syntax tree associated with syntax rule <anglr syntax rule>
		//

		public new bool checkInclusion (SyntaxTreeBase element)
		{
			return
				(element == this) ||
				(m__attribute_list_optional_ != null) && m__attribute_list_optional_.checkInclusion (element) ||
				(m__identifier_ != null) && m__identifier_.checkInclusion (element) ||
				(m__colon_ != null) && m__colon_.checkInclusion (element) ||
				(m__anglr_syntax_production_list_ != null) && m__anglr_syntax_production_list_.checkInclusion (element) ||
				(m__semicolon_ != null) && m__semicolon_.checkInclusion (element) ||
				(m__left_curly_bracket_ != null) && m__left_curly_bracket_.checkInclusion (element) ||
				(m__anglr_syntax_rule_list_optional_ != null) && m__anglr_syntax_rule_list_optional_.checkInclusion (element) ||
				(m__right_curly_bracket_ != null) && m__right_curly_bracket_.checkInclusion (element) ||
				false;
		}
		#endregion
		#region Syntax tree emit methods associated with the syntax rule <anglr syntax rule>

		//
		// emit production tree node associated with any production of syntax rule <anglr syntax rule>
		//

		public override string Emit (int depth)
		{
			string s = "";
			if (depth != 0)
			switch ((production_kind) kind)
			{
				case production_kind.g__anglr_syntax_rule__1:
					// emit syntax tree node associated with production
					// <anglr syntax rule>: <attribute list optional> <identifier> ':' <anglr syntax production list> ';'

					s += m__attribute_list_optional_.Emit (depth - 1);
					s += " ";
					s += m__identifier_.Emit (depth - 1);
					s += " ";
					s += m__colon_.Emit (depth - 1);
					s += " ";
					s += m__anglr_syntax_production_list_.Emit (depth - 1);
					s += " ";
					s += m__semicolon_.Emit (depth - 1);
				break;
				case production_kind.g__anglr_syntax_rule__2:
					// emit syntax tree node associated with production
					// <anglr syntax rule>: <attribute list optional> <identifier> '{' <anglr syntax rule list optional> '}'

					s += m__attribute_list_optional_.Emit (depth - 1);
					s += " ";
					s += m__identifier_.Emit (depth - 1);
					s += " ";
					s += m__left_curly_bracket_.Emit (depth - 1);
					s += " ";
					s += m__anglr_syntax_rule_list_optional_.Emit (depth - 1);
					s += " ";
					s += m__right_curly_bracket_.Emit (depth - 1);
				break;
			}
			return s.Trim ();
		}

		//
		// emit production tree node associated with any production of syntax rule <anglr syntax rule>
		//

		public override string EmitProductionTree (int depth)
		{
			string s = "_anglr_syntax_rule_ (";
			if (depth != 0)
				switch ((production_kind) kind)
				{
				case production_kind.g__anglr_syntax_rule__1:
					// emit syntax tree node associated with production
					// <anglr syntax rule>: <attribute list optional> <identifier> ':' <anglr syntax production list> ';'

					s += m__attribute_list_optional_.EmitProductionTree (depth - 1);
					s += ' ';
					s += "_identifier_";
					s += ' ';
					s += "_colon_";
					s += ' ';
					s += m__anglr_syntax_production_list_.EmitProductionTree (depth - 1);
					s += ' ';
					s += "_semicolon_";
				break;
				case production_kind.g__anglr_syntax_rule__2:
					// emit syntax tree node associated with production
					// <anglr syntax rule>: <attribute list optional> <identifier> '{' <anglr syntax rule list optional> '}'

					s += m__attribute_list_optional_.EmitProductionTree (depth - 1);
					s += ' ';
					s += "_identifier_";
					s += ' ';
					s += "_left_curly_bracket_";
					s += ' ';
					s += m__anglr_syntax_rule_list_optional_.EmitProductionTree (depth - 1);
					s += ' ';
					s += "_right_curly_bracket_";
				break;
			}
			s += ")";
			return s;
		}
		#endregion
		#region Syntax tree traversal,  reperrent, init and iterator methods associated with the syntax rule <anglr syntax rule>

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
			m__identifier_?.reparent (this);
			m__colon_?.reparent (this);
			m__anglr_syntax_production_list_?.reparent (this);
			m__semicolon_?.reparent (this);
			m__left_curly_bracket_?.reparent (this);
			m__anglr_syntax_rule_list_optional_?.reparent (this);
			m__right_curly_bracket_?.reparent (this);
		}

		//
		// initialize object associated with syntax rule <anglr syntax rule>
		//

		public void _init ()
		{
			m__attribute_list_optional_ = null;
			m__identifier_ = null;
			m__colon_ = null;
			m__anglr_syntax_production_list_ = null;
			m__semicolon_ = null;
			m__left_curly_bracket_ = null;
			m__anglr_syntax_rule_list_optional_ = null;
			m__right_curly_bracket_ = null;
		}
		#endregion
		#region Fields and properties associated with the syntax rule <anglr syntax rule>

		// counter of all nodes associated with syntax rule <anglr syntax rule>
		public static int g_counter;

		// objects associated with terminal and non-terminal symbols within production(s) of syntax rule <anglr syntax rule>
		public _attribute_list_optional_ m__attribute_list_optional_ { get; private set; }
		public SyntaxTreeToken m__identifier_ { get; private set; }
		public SyntaxTreeToken m__colon_ { get; private set; }
		public _anglr_syntax_production_list_ m__anglr_syntax_production_list_ { get; private set; }
		public SyntaxTreeToken m__semicolon_ { get; private set; }
		public SyntaxTreeToken m__left_curly_bracket_ { get; private set; }
		public _anglr_syntax_rule_list_optional_ m__anglr_syntax_rule_list_optional_ { get; private set; }
		public SyntaxTreeToken m__right_curly_bracket_ { get; private set; }
		#endregion

	};

	public partial class SyntaxTreeWalker
	{
		#region delegates and events associated with the syntax rule <anglr syntax rule>

		// delegate function (callback) prototype associated with syntax rule <anglr syntax rule>
		public delegate bool _anglr_syntax_rule__Callback (SyntaxTreeCallbackReason reason, _anglr_syntax_rule_.production_kind kind, _anglr_syntax_rule_ p__anglr_syntax_rule_);

		// event associated with syntax rule <anglr syntax rule>
		public event _anglr_syntax_rule__Callback _anglr_syntax_rule__Event;

		// event trigger associated with syntax rule <anglr syntax rule>
		public bool Raise__anglr_syntax_rule__Event (SyntaxTreeCallbackReason reason, _anglr_syntax_rule_.production_kind kind, _anglr_syntax_rule_ p__anglr_syntax_rule_)
		{
			bool? status = _anglr_syntax_rule__Event?.Invoke (reason, kind, p__anglr_syntax_rule_);
			return (status == null) || status.Value;
		}
		#endregion
		#region syntax tree traversal methods associated with the syntax rule <anglr syntax rule>
		//
		// traverse syntax tree node associated with any production of syntax rule <anglr syntax rule>
		//

		public void Traverse (_anglr_syntax_rule_ p__anglr_syntax_rule_)
		{
			if (p__anglr_syntax_rule_.isLocked())
				return;
			p__anglr_syntax_rule_.dolock();
			_anglr_syntax_rule_.production_kind kind = (_anglr_syntax_rule_.production_kind) p__anglr_syntax_rule_.kind;
			p__anglr_syntax_rule_.turn_reset ();
			if (Raise__anglr_syntax_rule__Event (SyntaxTreeCallbackReason.TraversalPrologueCallbackReason, kind, p__anglr_syntax_rule_))
			switch (kind)
			{
				case _anglr_syntax_rule_.production_kind.g__anglr_syntax_rule__1:
					// traverse syntax tree node associated with production
					// <anglr syntax rule>: <attribute list optional> <identifier> ':' <anglr syntax production list> ';'

					if (Raise__anglr_syntax_rule__Event (SyntaxTreeCallbackReason.TraversalMidTermCallbackReason, kind, p__anglr_syntax_rule_))
						Traverse (p__anglr_syntax_rule_.m__attribute_list_optional_);
					p__anglr_syntax_rule_.turn_inc ();
					if (Raise__anglr_syntax_rule__Event (SyntaxTreeCallbackReason.TraversalMidTermCallbackReason, kind, p__anglr_syntax_rule_))
						Traverse (p__anglr_syntax_rule_.m__anglr_syntax_production_list_);
					p__anglr_syntax_rule_.turn_inc ();
					Raise__anglr_syntax_rule__Event (SyntaxTreeCallbackReason.TraversalMidTermCallbackReason, kind, p__anglr_syntax_rule_);
				break;
				case _anglr_syntax_rule_.production_kind.g__anglr_syntax_rule__2:
					// traverse syntax tree node associated with production
					// <anglr syntax rule>: <attribute list optional> <identifier> '{' <anglr syntax rule list optional> '}'

					if (Raise__anglr_syntax_rule__Event (SyntaxTreeCallbackReason.TraversalMidTermCallbackReason, kind, p__anglr_syntax_rule_))
						Traverse (p__anglr_syntax_rule_.m__attribute_list_optional_);
					p__anglr_syntax_rule_.turn_inc ();
					if (Raise__anglr_syntax_rule__Event (SyntaxTreeCallbackReason.TraversalMidTermCallbackReason, kind, p__anglr_syntax_rule_))
						Traverse (p__anglr_syntax_rule_.m__anglr_syntax_rule_list_optional_);
					p__anglr_syntax_rule_.turn_inc ();
					Raise__anglr_syntax_rule__Event (SyntaxTreeCallbackReason.TraversalMidTermCallbackReason, kind, p__anglr_syntax_rule_);
				break;
			}
			Raise__anglr_syntax_rule__Event (SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason, kind, p__anglr_syntax_rule_);
			p__anglr_syntax_rule_.unlock();
		}

		//
		// traverse syntax tree node associated with any production of syntax rule <anglr syntax rule>
		//

		public void TraverseCommon (_anglr_syntax_rule_ p__anglr_syntax_rule_)
		{
			_anglr_syntax_rule_.production_kind kind = (_anglr_syntax_rule_.production_kind) p__anglr_syntax_rule_.kind;
			if (Raise_Common_Event (SyntaxTreeCallbackReason.TraversalPrologueCallbackReason, (int) kind, p__anglr_syntax_rule_))
			switch (kind)
			{
				case _anglr_syntax_rule_.production_kind.g__anglr_syntax_rule__1:
					// traverse syntax tree node associated with production
					// <anglr syntax rule>: <attribute list optional> <identifier> ':' <anglr syntax production list> ';'

						TraverseCommon (p__anglr_syntax_rule_.m__attribute_list_optional_);
						TraverseCommon (p__anglr_syntax_rule_.m__identifier_);
						TraverseCommon (p__anglr_syntax_rule_.m__colon_);
						TraverseCommon (p__anglr_syntax_rule_.m__anglr_syntax_production_list_);
						TraverseCommon (p__anglr_syntax_rule_.m__semicolon_);
				break;
				case _anglr_syntax_rule_.production_kind.g__anglr_syntax_rule__2:
					// traverse syntax tree node associated with production
					// <anglr syntax rule>: <attribute list optional> <identifier> '{' <anglr syntax rule list optional> '}'

						TraverseCommon (p__anglr_syntax_rule_.m__attribute_list_optional_);
						TraverseCommon (p__anglr_syntax_rule_.m__identifier_);
						TraverseCommon (p__anglr_syntax_rule_.m__left_curly_bracket_);
						TraverseCommon (p__anglr_syntax_rule_.m__anglr_syntax_rule_list_optional_);
						TraverseCommon (p__anglr_syntax_rule_.m__right_curly_bracket_);
				break;
			}
			Raise_Common_Event (SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason, (int) kind, p__anglr_syntax_rule_);
		}
	};
		#endregion

	public partial class SyntaxTreeBuilder : SyntaxTreeWalker
	{
		#region syntax tree builder methods associated with the syntax rule <anglr syntax rule>

		//
		// create syntax tree node associated with production
		// <anglr syntax rule>: <attribute list optional> <identifier> ':' <anglr syntax production list> ';'

		//

		public _anglr_syntax_rule_ _anglr_syntax_rule__1 (_attribute_list_optional_ p__attribute_list_optional_, SyntaxTreeToken p_token, SyntaxTreeToken p_token_1, _anglr_syntax_production_list_ p__anglr_syntax_production_list_, SyntaxTreeToken p_token_2)
		{
			_anglr_syntax_rule_ p__anglr_syntax_rule__ref = new _anglr_syntax_rule_(p__attribute_list_optional_, p_token, p_token_1, p__anglr_syntax_production_list_, p_token_2);
			p__attribute_list_optional_.parent = p__anglr_syntax_rule__ref;
			p_token.parent = p__anglr_syntax_rule__ref;
			p_token_1.parent = p__anglr_syntax_rule__ref;
			p__anglr_syntax_production_list_.parent = p__anglr_syntax_rule__ref;
			p_token_2.parent = p__anglr_syntax_rule__ref;
			Raise__anglr_syntax_rule__Event (SyntaxTreeCallbackReason.BuilderCallbackReason, _anglr_syntax_rule_.production_kind.g__anglr_syntax_rule__1, p__anglr_syntax_rule__ref);
			return p__anglr_syntax_rule__ref;
		}

		//
		// create syntax tree node associated with production
		// <anglr syntax rule>: <attribute list optional> <identifier> '{' <anglr syntax rule list optional> '}'

		//

		public _anglr_syntax_rule_ _anglr_syntax_rule__2 (_attribute_list_optional_ p__attribute_list_optional_, SyntaxTreeToken p_token, SyntaxTreeToken p_token_1, _anglr_syntax_rule_list_optional_ p__anglr_syntax_rule_list_optional_, SyntaxTreeToken p_token_2)
		{
			_anglr_syntax_rule_ p__anglr_syntax_rule__ref = new _anglr_syntax_rule_(p__attribute_list_optional_, p_token, p_token_1, p__anglr_syntax_rule_list_optional_, p_token_2);
			p__attribute_list_optional_.parent = p__anglr_syntax_rule__ref;
			p_token.parent = p__anglr_syntax_rule__ref;
			p_token_1.parent = p__anglr_syntax_rule__ref;
			p__anglr_syntax_rule_list_optional_.parent = p__anglr_syntax_rule__ref;
			p_token_2.parent = p__anglr_syntax_rule__ref;
			Raise__anglr_syntax_rule__Event (SyntaxTreeCallbackReason.BuilderCallbackReason, _anglr_syntax_rule_.production_kind.g__anglr_syntax_rule__2, p__anglr_syntax_rule__ref);
			return p__anglr_syntax_rule__ref;
		}
		#endregion
	};
}
