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
	// class associated with syntax rule <parser part>
	//

	public class	_parser_part_ : SyntaxTreeBase
	{
		#region enumerated production(s) of syntax rule <parser part>
		public enum production_kind : ushort	// enumerated production(s) of syntax rule <parser part>
		{
			g__parser_part__1 = 1,	// <attribute list optional> '%parser' <identifier> '%{' <anglr syntax rule list optional> '%}'

		};
		#endregion
		#region production markers associated with the syntax rule <parser part>

		// markers associated with production: <parser part> -> <attribute list optional> '%parser' <identifier> '%{' <anglr syntax rule list optional> '%}'

		public enum production_marker_1 : ushort
		{
			m__attribute_list_optional_,
			m__anglr_syntax_rule_list_optional_,
			final
		};
		#endregion
		#region Constructors and destructors associated with the syntax rule <parser part>

		// Constructor declaration(s) associated with production(s) of syntax rule <parser part>

		//
		// Constructor associated with the following production(s)
		// <parser part> -> <attribute list optional> '%parser' <identifier> '%{' <anglr syntax rule list optional> '%}'

		//

		public _parser_part_ (_attribute_list_optional_ p__attribute_list_optional_, SyntaxTreeToken p_token, SyntaxTreeToken p_token_1, SyntaxTreeToken p_token_2, _anglr_syntax_rule_list_optional_ p__anglr_syntax_rule_list_optional_, SyntaxTreeToken p_token_3) : base ((uint) ProductionID.__parser_part__ID, (uint) production_kind.g__parser_part__1)
		{
			++g_counter;
			_init ();
			children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 6);
			children[0] = m__attribute_list_optional_ = p__attribute_list_optional_;
			children[1] = m__parser_ = p_token;
			children[2] = m__identifier_ = p_token_1;
			children[3] = m__left_part_bracket_ = p_token_2;
			children[4] = m__anglr_syntax_rule_list_optional_ = p__anglr_syntax_rule_list_optional_;
			children[5] = m__right_part_bracket_ = p_token_3;
		}

		// Copy constructor

		public _parser_part_ (_parser_part_ p__parser_part_) : base (p__parser_part_.id, p__parser_part_.kind)
		{
			++g_counter;
			_init ();
			switch ((production_kind) p__parser_part_.kind)
			{
				case production_kind.g__parser_part__1:
					children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 6);
					if ((children [0] = m__attribute_list_optional_ = (p__parser_part_.m__attribute_list_optional_ != null) ? new _attribute_list_optional_ (p__parser_part_.m__attribute_list_optional_) : null) != null) m__attribute_list_optional_.parent = this;
					if ((children [1] = m__parser_ = (p__parser_part_.m__parser_ != null) ? new SyntaxTreeToken (p__parser_part_.m__parser_) : null) != null) m__parser_.parent = this;
					if ((children [2] = m__identifier_ = (p__parser_part_.m__identifier_ != null) ? new SyntaxTreeToken (p__parser_part_.m__identifier_) : null) != null) m__identifier_.parent = this;
					if ((children [3] = m__left_part_bracket_ = (p__parser_part_.m__left_part_bracket_ != null) ? new SyntaxTreeToken (p__parser_part_.m__left_part_bracket_) : null) != null) m__left_part_bracket_.parent = this;
					if ((children [4] = m__anglr_syntax_rule_list_optional_ = (p__parser_part_.m__anglr_syntax_rule_list_optional_ != null) ? new _anglr_syntax_rule_list_optional_ (p__parser_part_.m__anglr_syntax_rule_list_optional_) : null) != null) m__anglr_syntax_rule_list_optional_.parent = this;
					if ((children [5] = m__right_part_bracket_ = (p__parser_part_.m__right_part_bracket_ != null) ? new SyntaxTreeToken (p__parser_part_.m__right_part_bracket_) : null) != null) m__right_part_bracket_.parent = this;
					break;
				default:
				{
					string[] args = new string[] { "_parser_part_" };
					throw new SyntaxTreeError (SyntaxTreeError.InvalidKindError, args);
				}
			}
		}

		// Clone node - wrapper of copy constructor

		public override SyntaxTreeBase Clone () { return (SyntaxTreeBase) new _parser_part_ (this); }

		// Destructor

		public new void Dispose ()
		{
			base.Dispose ();
			--g_counter;

			m__attribute_list_optional_?.Dispose ();
			m__parser_?.Dispose ();
			m__identifier_?.Dispose ();
			m__left_part_bracket_?.Dispose ();
			m__anglr_syntax_rule_list_optional_?.Dispose ();
			m__right_part_bracket_?.Dispose ();

			_init ();
		}
		#endregion
		#region Content changing and inclusion checking methods associated with the syntax rule <parser part>

		// Content changing function(s) associated with production(s) of syntax rule <parser part>

		//
		// Content changing function associated with following production(s)
		// <parser part> -> <attribute list optional> '%parser' <identifier> '%{' <anglr syntax rule list optional> '%}'

		//

		public void change(_attribute_list_optional_ p__attribute_list_optional_, SyntaxTreeToken p_token, SyntaxTreeToken p_token_1, SyntaxTreeToken p_token_2, _anglr_syntax_rule_list_optional_ p__anglr_syntax_rule_list_optional_, SyntaxTreeToken p_token_3)
		{
			_init ();
			this.kind = (uint) production_kind.g__parser_part__1;
			children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 6);
			children [0] = m__attribute_list_optional_ = p__attribute_list_optional_;
			children [1] = m__parser_ = p_token;
			children [2] = m__identifier_ = p_token_1;
			children [3] = m__left_part_bracket_ = p_token_2;
			children [4] = m__anglr_syntax_rule_list_optional_ = p__anglr_syntax_rule_list_optional_;
			children [5] = m__right_part_bracket_ = p_token_3;
			reparent (parent);
		}

		//
		// check if node 'element' is included within syntax tree associated with syntax rule <parser part>
		//

		public new bool checkInclusion (SyntaxTreeBase element)
		{
			return
				(element == this) ||
				(m__attribute_list_optional_ != null) && m__attribute_list_optional_.checkInclusion (element) ||
				(m__parser_ != null) && m__parser_.checkInclusion (element) ||
				(m__identifier_ != null) && m__identifier_.checkInclusion (element) ||
				(m__left_part_bracket_ != null) && m__left_part_bracket_.checkInclusion (element) ||
				(m__anglr_syntax_rule_list_optional_ != null) && m__anglr_syntax_rule_list_optional_.checkInclusion (element) ||
				(m__right_part_bracket_ != null) && m__right_part_bracket_.checkInclusion (element) ||
				false;
		}
		#endregion
		#region Syntax tree emit methods associated with the syntax rule <parser part>

		//
		// emit production tree node associated with any production of syntax rule <parser part>
		//

		public override string Emit (int depth)
		{
			string s = "";
			if (depth != 0)
			switch ((production_kind) kind)
			{
				case production_kind.g__parser_part__1:
					// emit syntax tree node associated with production
					// <parser part>: <attribute list optional> '%parser' <identifier> '%{' <anglr syntax rule list optional> '%}'

					s += m__attribute_list_optional_.Emit (depth - 1);
					s += " ";
					s += m__parser_.Emit (depth - 1);
					s += " ";
					s += m__identifier_.Emit (depth - 1);
					s += " ";
					s += m__left_part_bracket_.Emit (depth - 1);
					s += " ";
					s += m__anglr_syntax_rule_list_optional_.Emit (depth - 1);
					s += " ";
					s += m__right_part_bracket_.Emit (depth - 1);
				break;
			}
			return s.Trim ();
		}

		//
		// emit production tree node associated with any production of syntax rule <parser part>
		//

		public override string EmitProductionTree (int depth)
		{
			string s = "_parser_part_ (";
			if (depth != 0)
				switch ((production_kind) kind)
				{
				case production_kind.g__parser_part__1:
					// emit syntax tree node associated with production
					// <parser part>: <attribute list optional> '%parser' <identifier> '%{' <anglr syntax rule list optional> '%}'

					s += m__attribute_list_optional_.EmitProductionTree (depth - 1);
					s += ' ';
					s += "_parser_";
					s += ' ';
					s += "_identifier_";
					s += ' ';
					s += "_left_part_bracket_";
					s += ' ';
					s += m__anglr_syntax_rule_list_optional_.EmitProductionTree (depth - 1);
					s += ' ';
					s += "_right_part_bracket_";
				break;
			}
			s += ")";
			return s;
		}
		#endregion
		#region Syntax tree traversal,  reperrent, init and iterator methods associated with the syntax rule <parser part>

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
			m__parser_?.reparent (this);
			m__identifier_?.reparent (this);
			m__left_part_bracket_?.reparent (this);
			m__anglr_syntax_rule_list_optional_?.reparent (this);
			m__right_part_bracket_?.reparent (this);
		}

		//
		// initialize object associated with syntax rule <parser part>
		//

		public void _init ()
		{
			m__attribute_list_optional_ = null;
			m__parser_ = null;
			m__identifier_ = null;
			m__left_part_bracket_ = null;
			m__anglr_syntax_rule_list_optional_ = null;
			m__right_part_bracket_ = null;
		}
		#endregion
		#region Fields and properties associated with the syntax rule <parser part>

		// counter of all nodes associated with syntax rule <parser part>
		public static int g_counter;

		// objects associated with terminal and non-terminal symbols within production(s) of syntax rule <parser part>
		public _attribute_list_optional_ m__attribute_list_optional_ { get; private set; }
		public SyntaxTreeToken m__parser_ { get; private set; }
		public SyntaxTreeToken m__identifier_ { get; private set; }
		public SyntaxTreeToken m__left_part_bracket_ { get; private set; }
		public _anglr_syntax_rule_list_optional_ m__anglr_syntax_rule_list_optional_ { get; private set; }
		public SyntaxTreeToken m__right_part_bracket_ { get; private set; }
		#endregion

	};

	public partial class SyntaxTreeWalker
	{
		#region delegates and events associated with the syntax rule <parser part>

		// delegate function (callback) prototype associated with syntax rule <parser part>
		public delegate bool _parser_part__Callback (SyntaxTreeCallbackReason reason, _parser_part_.production_kind kind, _parser_part_ p__parser_part_);

		// event associated with syntax rule <parser part>
		public event _parser_part__Callback _parser_part__Event;

		// event trigger associated with syntax rule <parser part>
		public bool Raise__parser_part__Event (SyntaxTreeCallbackReason reason, _parser_part_.production_kind kind, _parser_part_ p__parser_part_)
		{
			bool? status = _parser_part__Event?.Invoke (reason, kind, p__parser_part_);
			return (status == null) || status.Value;
		}
		#endregion
		#region syntax tree traversal methods associated with the syntax rule <parser part>
		//
		// traverse syntax tree node associated with any production of syntax rule <parser part>
		//

		public void Traverse (_parser_part_ p__parser_part_)
		{
			if (p__parser_part_.isLocked())
				return;
			p__parser_part_.dolock();
			_parser_part_.production_kind kind = (_parser_part_.production_kind) p__parser_part_.kind;
			p__parser_part_.turn_reset ();
			if (Raise__parser_part__Event (SyntaxTreeCallbackReason.TraversalPrologueCallbackReason, kind, p__parser_part_))
			switch (kind)
			{
				case _parser_part_.production_kind.g__parser_part__1:
					// traverse syntax tree node associated with production
					// <parser part>: <attribute list optional> '%parser' <identifier> '%{' <anglr syntax rule list optional> '%}'

					if (Raise__parser_part__Event (SyntaxTreeCallbackReason.TraversalMidTermCallbackReason, kind, p__parser_part_))
						Traverse (p__parser_part_.m__attribute_list_optional_);
					p__parser_part_.turn_inc ();
					if (Raise__parser_part__Event (SyntaxTreeCallbackReason.TraversalMidTermCallbackReason, kind, p__parser_part_))
						Traverse (p__parser_part_.m__anglr_syntax_rule_list_optional_);
					p__parser_part_.turn_inc ();
					Raise__parser_part__Event (SyntaxTreeCallbackReason.TraversalMidTermCallbackReason, kind, p__parser_part_);
				break;
			}
			Raise__parser_part__Event (SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason, kind, p__parser_part_);
			p__parser_part_.unlock();
		}

		//
		// traverse syntax tree node associated with any production of syntax rule <parser part>
		//

		public void TraverseCommon (_parser_part_ p__parser_part_)
		{
			_parser_part_.production_kind kind = (_parser_part_.production_kind) p__parser_part_.kind;
			if (Raise_Common_Event (SyntaxTreeCallbackReason.TraversalPrologueCallbackReason, (int) kind, p__parser_part_))
			switch (kind)
			{
				case _parser_part_.production_kind.g__parser_part__1:
					// traverse syntax tree node associated with production
					// <parser part>: <attribute list optional> '%parser' <identifier> '%{' <anglr syntax rule list optional> '%}'

						TraverseCommon (p__parser_part_.m__attribute_list_optional_);
						TraverseCommon (p__parser_part_.m__parser_);
						TraverseCommon (p__parser_part_.m__identifier_);
						TraverseCommon (p__parser_part_.m__left_part_bracket_);
						TraverseCommon (p__parser_part_.m__anglr_syntax_rule_list_optional_);
						TraverseCommon (p__parser_part_.m__right_part_bracket_);
				break;
			}
			Raise_Common_Event (SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason, (int) kind, p__parser_part_);
		}
	};
		#endregion

	public partial class SyntaxTreeBuilder : SyntaxTreeWalker
	{
		#region syntax tree builder methods associated with the syntax rule <parser part>

		//
		// create syntax tree node associated with production
		// <parser part>: <attribute list optional> '%parser' <identifier> '%{' <anglr syntax rule list optional> '%}'

		//

		public _parser_part_ _parser_part__1 (_attribute_list_optional_ p__attribute_list_optional_, SyntaxTreeToken p_token, SyntaxTreeToken p_token_1, SyntaxTreeToken p_token_2, _anglr_syntax_rule_list_optional_ p__anglr_syntax_rule_list_optional_, SyntaxTreeToken p_token_3)
		{
			_parser_part_ p__parser_part__ref = new _parser_part_(p__attribute_list_optional_, p_token, p_token_1, p_token_2, p__anglr_syntax_rule_list_optional_, p_token_3);
			p__attribute_list_optional_.parent = p__parser_part__ref;
			p_token.parent = p__parser_part__ref;
			p_token_1.parent = p__parser_part__ref;
			p_token_2.parent = p__parser_part__ref;
			p__anglr_syntax_rule_list_optional_.parent = p__parser_part__ref;
			p_token_3.parent = p__parser_part__ref;
			Raise__parser_part__Event (SyntaxTreeCallbackReason.BuilderCallbackReason, _parser_part_.production_kind.g__parser_part__1, p__parser_part__ref);
			return p__parser_part__ref;
		}
		#endregion
	};
}
