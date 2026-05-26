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
	// class associated with syntax rule <declaration part>
	//

	public class	_declaration_part_ : SyntaxTreeBase
	{
		#region enumerated production(s) of syntax rule <declaration part>
		public enum production_kind : ushort	// enumerated production(s) of syntax rule <declaration part>
		{
			g__declaration_part__1 = 1,	// <attribute list optional> '%declarations' <identifier> '%{' <anglr definition list optional> '%}'

		};
		#endregion
		#region production markers associated with the syntax rule <declaration part>

		// markers associated with production: <declaration part> -> <attribute list optional> '%declarations' <identifier> '%{' <anglr definition list optional> '%}'

		public enum production_marker_1 : ushort
		{
			m__attribute_list_optional_,
			m__anglr_definition_list_optional_,
			final
		};
		#endregion
		#region Constructors and destructors associated with the syntax rule <declaration part>

		// Constructor declaration(s) associated with production(s) of syntax rule <declaration part>

		//
		// Constructor associated with the following production(s)
		// <declaration part> -> <attribute list optional> '%declarations' <identifier> '%{' <anglr definition list optional> '%}'

		//

		public _declaration_part_ (_attribute_list_optional_ p__attribute_list_optional_, SyntaxTreeToken p_token, SyntaxTreeToken p_token_1, SyntaxTreeToken p_token_2, _anglr_definition_list_optional_ p__anglr_definition_list_optional_, SyntaxTreeToken p_token_3) : base ((uint) ProductionID.__declaration_part__ID, (uint) production_kind.g__declaration_part__1)
		{
			++g_counter;
			_init ();
			children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 6);
			children[0] = m__attribute_list_optional_ = p__attribute_list_optional_;
			children[1] = m__declarations_ = p_token;
			children[2] = m__identifier_ = p_token_1;
			children[3] = m__left_part_bracket_ = p_token_2;
			children[4] = m__anglr_definition_list_optional_ = p__anglr_definition_list_optional_;
			children[5] = m__right_part_bracket_ = p_token_3;
		}

		// Copy constructor

		public _declaration_part_ (_declaration_part_ p__declaration_part_) : base (p__declaration_part_.id, p__declaration_part_.kind)
		{
			++g_counter;
			_init ();
			switch ((production_kind) this.kind)
			{
			case production_kind.g__declaration_part__1:
				children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 6);
				children[0] = m__attribute_list_optional_ = p__declaration_part_.m__attribute_list_optional_;
				children[1] = m__declarations_ = p__declaration_part_.m__declarations_;
				children[2] = m__identifier_ = p__declaration_part_.m__identifier_;
				children[3] = m__left_part_bracket_ = p__declaration_part_.m__left_part_bracket_;
				children[4] = m__anglr_definition_list_optional_ = p__declaration_part_.m__anglr_definition_list_optional_;
				children[5] = m__right_part_bracket_ = p__declaration_part_.m__right_part_bracket_;
				break;
			default:
				string[] args = new string[] { "_declaration_part_" };
				throw new SyntaxTreeError (SyntaxTreeError.InvalidKindError, args);
			}
		}

		// Clone node - wrapper of copy constructor

		public override SyntaxTreeBase Clone () { return (SyntaxTreeBase) new _declaration_part_ (this); }

		// Destructor

		public new void Dispose ()
		{
			base.Dispose ();
			--g_counter;

			m__attribute_list_optional_?.Dispose ();
			m__declarations_?.Dispose ();
			m__identifier_?.Dispose ();
			m__left_part_bracket_?.Dispose ();
			m__anglr_definition_list_optional_?.Dispose ();
			m__right_part_bracket_?.Dispose ();

			_init ();
		}
		#endregion
		#region Content changing and inclusion checking methods associated with the syntax rule <declaration part>

		// Content changing function(s) associated with production(s) of syntax rule <declaration part>

		//
		// Content changing function associated with following production(s)
		// <declaration part> -> <attribute list optional> '%declarations' <identifier> '%{' <anglr definition list optional> '%}'

		//

		public void change(_attribute_list_optional_ p__attribute_list_optional_, SyntaxTreeToken p_token, SyntaxTreeToken p_token_1, SyntaxTreeToken p_token_2, _anglr_definition_list_optional_ p__anglr_definition_list_optional_, SyntaxTreeToken p_token_3)
		{
			_init ();
			this.kind = (uint) production_kind.g__declaration_part__1;
			children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 6);
			children [0] = m__attribute_list_optional_ = p__attribute_list_optional_;
			children [1] = m__declarations_ = p_token;
			children [2] = m__identifier_ = p_token_1;
			children [3] = m__left_part_bracket_ = p_token_2;
			children [4] = m__anglr_definition_list_optional_ = p__anglr_definition_list_optional_;
			children [5] = m__right_part_bracket_ = p_token_3;
			reparent (parent);
		}

		//
		// check if node 'element' is included within syntax tree associated with syntax rule <declaration part>
		//

		public new bool checkInclusion (SyntaxTreeBase element)
		{
			return
				(element == this) ||
				(m__attribute_list_optional_ != null) && m__attribute_list_optional_.checkInclusion (element) ||
				(m__declarations_ != null) && m__declarations_.checkInclusion (element) ||
				(m__identifier_ != null) && m__identifier_.checkInclusion (element) ||
				(m__left_part_bracket_ != null) && m__left_part_bracket_.checkInclusion (element) ||
				(m__anglr_definition_list_optional_ != null) && m__anglr_definition_list_optional_.checkInclusion (element) ||
				(m__right_part_bracket_ != null) && m__right_part_bracket_.checkInclusion (element) ||
				false;
		}
		#endregion
		#region Syntax tree emit methods associated with the syntax rule <declaration part>

		//
		// emit production tree node associated with any production of syntax rule <declaration part>
		//

		public override string Emit (int depth)
		{
			string s = "";
			if (depth != 0)
			switch ((production_kind) kind)
			{
				case production_kind.g__declaration_part__1:
					// emit syntax tree node associated with production
					// <declaration part>: <attribute list optional> '%declarations' <identifier> '%{' <anglr definition list optional> '%}'

					s += m__attribute_list_optional_.Emit (depth - 1);
					s += " ";
					s += m__declarations_.Emit (depth - 1);
					s += " ";
					s += m__identifier_.Emit (depth - 1);
					s += " ";
					s += m__left_part_bracket_.Emit (depth - 1);
					s += " ";
					s += m__anglr_definition_list_optional_.Emit (depth - 1);
					s += " ";
					s += m__right_part_bracket_.Emit (depth - 1);
				break;
			}
			return s.Trim ();
		}

		//
		// emit production tree node associated with any production of syntax rule <declaration part>
		//

		public override string EmitProductionTree (int depth)
		{
			string s = "_declaration_part_ (";
			if (depth != 0)
				switch ((production_kind) kind)
				{
				case production_kind.g__declaration_part__1:
					// emit syntax tree node associated with production
					// <declaration part>: <attribute list optional> '%declarations' <identifier> '%{' <anglr definition list optional> '%}'

					s += m__attribute_list_optional_.EmitProductionTree (depth - 1);
					s += ' ';
					s += "_declarations_";
					s += ' ';
					s += "_identifier_";
					s += ' ';
					s += "_left_part_bracket_";
					s += ' ';
					s += m__anglr_definition_list_optional_.EmitProductionTree (depth - 1);
					s += ' ';
					s += "_right_part_bracket_";
				break;
			}
			s += ")";
			return s;
		}
		#endregion
		#region Syntax tree traversal,  reperrent, init and iterator methods associated with the syntax rule <declaration part>

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
			m__declarations_?.reparent (this);
			m__identifier_?.reparent (this);
			m__left_part_bracket_?.reparent (this);
			m__anglr_definition_list_optional_?.reparent (this);
			m__right_part_bracket_?.reparent (this);
		}

		//
		// initialize object associated with syntax rule <declaration part>
		//

		public void _init ()
		{
			m__attribute_list_optional_ = null;
			m__declarations_ = null;
			m__identifier_ = null;
			m__left_part_bracket_ = null;
			m__anglr_definition_list_optional_ = null;
			m__right_part_bracket_ = null;
		}
		#endregion
		#region Fields and properties associated with the syntax rule <declaration part>

		// counter of all nodes associated with syntax rule <declaration part>
		public static int g_counter;

		// objects associated with terminal and non-terminal symbols within production(s) of syntax rule <declaration part>
		public _attribute_list_optional_ m__attribute_list_optional_ { get; private set; }
		public SyntaxTreeToken m__declarations_ { get; private set; }
		public SyntaxTreeToken m__identifier_ { get; private set; }
		public SyntaxTreeToken m__left_part_bracket_ { get; private set; }
		public _anglr_definition_list_optional_ m__anglr_definition_list_optional_ { get; private set; }
		public SyntaxTreeToken m__right_part_bracket_ { get; private set; }
		#endregion

	};

	public partial class SyntaxTreeWalker
	{
		#region delegates and events associated with the syntax rule <declaration part>

		// delegate function (callback) prototype associated with syntax rule <declaration part>
		public delegate bool _declaration_part__Callback (SyntaxTreeCallbackReason reason, _declaration_part_.production_kind kind, _declaration_part_ p__declaration_part_);

		// event associated with syntax rule <declaration part>
		public event _declaration_part__Callback _declaration_part__Event;

		// event trigger associated with syntax rule <declaration part>
		public bool Raise__declaration_part__Event (SyntaxTreeCallbackReason reason, _declaration_part_.production_kind kind, _declaration_part_ p__declaration_part_)
		{
			bool? status = _declaration_part__Event?.Invoke (reason, kind, p__declaration_part_);
			return (status == null) || status.Value;
		}
		#endregion
		#region syntax tree traversal methods associated with the syntax rule <declaration part>
		//
		// traverse syntax tree node associated with any production of syntax rule <declaration part>
		//

		public void Traverse (_declaration_part_ p__declaration_part_)
		{
			if (p__declaration_part_.isLocked())
				return;
			p__declaration_part_.dolock();
			_declaration_part_.production_kind kind = (_declaration_part_.production_kind) p__declaration_part_.kind;
			p__declaration_part_.turn_reset ();
			if (Raise__declaration_part__Event (SyntaxTreeCallbackReason.TraversalPrologueCallbackReason, kind, p__declaration_part_))
			switch (kind)
			{
				case _declaration_part_.production_kind.g__declaration_part__1:
					// traverse syntax tree node associated with production
					// <declaration part>: <attribute list optional> '%declarations' <identifier> '%{' <anglr definition list optional> '%}'

					if (Raise__declaration_part__Event (SyntaxTreeCallbackReason.TraversalMidTermCallbackReason, kind, p__declaration_part_))
						Traverse (p__declaration_part_.m__attribute_list_optional_);
					p__declaration_part_.turn_inc ();
					if (Raise__declaration_part__Event (SyntaxTreeCallbackReason.TraversalMidTermCallbackReason, kind, p__declaration_part_))
						Traverse (p__declaration_part_.m__anglr_definition_list_optional_);
					p__declaration_part_.turn_inc ();
					Raise__declaration_part__Event (SyntaxTreeCallbackReason.TraversalMidTermCallbackReason, kind, p__declaration_part_);
				break;
			}
			Raise__declaration_part__Event (SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason, kind, p__declaration_part_);
			p__declaration_part_.unlock();
		}

		//
		// traverse syntax tree node associated with any production of syntax rule <declaration part>
		//

		public void TraverseCommon (_declaration_part_ p__declaration_part_)
		{
			_declaration_part_.production_kind kind = (_declaration_part_.production_kind) p__declaration_part_.kind;
			if (Raise_Common_Event (SyntaxTreeCallbackReason.TraversalPrologueCallbackReason, (int) kind, p__declaration_part_))
			switch (kind)
			{
				case _declaration_part_.production_kind.g__declaration_part__1:
					// traverse syntax tree node associated with production
					// <declaration part>: <attribute list optional> '%declarations' <identifier> '%{' <anglr definition list optional> '%}'

						TraverseCommon (p__declaration_part_.m__attribute_list_optional_);
						TraverseCommon (p__declaration_part_.m__declarations_);
						TraverseCommon (p__declaration_part_.m__identifier_);
						TraverseCommon (p__declaration_part_.m__left_part_bracket_);
						TraverseCommon (p__declaration_part_.m__anglr_definition_list_optional_);
						TraverseCommon (p__declaration_part_.m__right_part_bracket_);
				break;
			}
			Raise_Common_Event (SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason, (int) kind, p__declaration_part_);
		}
	};
		#endregion

	public partial class SyntaxTreeBuilder : SyntaxTreeWalker
	{
		#region syntax tree builder methods associated with the syntax rule <declaration part>

		//
		// create syntax tree node associated with production
		// <declaration part>: <attribute list optional> '%declarations' <identifier> '%{' <anglr definition list optional> '%}'

		//

		public _declaration_part_ _declaration_part__1 (_attribute_list_optional_ p__attribute_list_optional_, SyntaxTreeToken p_token, SyntaxTreeToken p_token_1, SyntaxTreeToken p_token_2, _anglr_definition_list_optional_ p__anglr_definition_list_optional_, SyntaxTreeToken p_token_3)
		{
			_declaration_part_ p__declaration_part__ref = new _declaration_part_(p__attribute_list_optional_, p_token, p_token_1, p_token_2, p__anglr_definition_list_optional_, p_token_3);
			p__attribute_list_optional_.parent = p__declaration_part__ref;
			p_token.parent = p__declaration_part__ref;
			p_token_1.parent = p__declaration_part__ref;
			p_token_2.parent = p__declaration_part__ref;
			p__anglr_definition_list_optional_.parent = p__declaration_part__ref;
			p_token_3.parent = p__declaration_part__ref;
			Raise__declaration_part__Event (SyntaxTreeCallbackReason.BuilderCallbackReason, _declaration_part_.production_kind.g__declaration_part__1, p__declaration_part__ref);
			return p__declaration_part__ref;
		}
		#endregion
	};
}
