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
	// class associated with syntax rule <attribute>
	//

	public class	_attribute_ : SyntaxTreeBase
	{
		#region enumerated production(s) of syntax rule <attribute>
		public enum production_kind : ushort	// enumerated production(s) of syntax rule <attribute>
		{
			g__attribute__1 = 1,	// '[' <identifier> <name value list optional> ']'

		};
		#endregion
		#region production markers associated with the syntax rule <attribute>

		// markers associated with production: <attribute> -> '[' <identifier> <name value list optional> ']'

		public enum production_marker_1 : ushort
		{
			m__name_value_list_optional_,
			final
		};
		#endregion
		#region Constructors and destructors associated with the syntax rule <attribute>

		// Constructor declaration(s) associated with production(s) of syntax rule <attribute>

		//
		// Constructor associated with the following production(s)
		// <attribute> -> '[' <identifier> <name value list optional> ']'

		//

		public _attribute_ (SyntaxTreeToken p_token, SyntaxTreeToken p_token_1, _name_value_list_optional_ p__name_value_list_optional_, SyntaxTreeToken p_token_2) : base ((uint) ProductionID.__attribute__ID, (uint) production_kind.g__attribute__1)
		{
			++g_counter;
			_init ();
			children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 4);
			children[0] = m__left_square_bracket_ = p_token;
			children[1] = m__identifier_ = p_token_1;
			children[2] = m__name_value_list_optional_ = p__name_value_list_optional_;
			children[3] = m__right_square_bracket_ = p_token_2;
		}

		// Copy constructor

		public _attribute_ (_attribute_ p__attribute_) : base (p__attribute_.id, p__attribute_.kind)
		{
			++g_counter;
			_init ();
			switch ((production_kind) this.kind)
			{
			case production_kind.g__attribute__1:
				children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 4);
				children[0] = m__left_square_bracket_ = p__attribute_.m__left_square_bracket_;
				children[1] = m__identifier_ = p__attribute_.m__identifier_;
				children[2] = m__name_value_list_optional_ = p__attribute_.m__name_value_list_optional_;
				children[3] = m__right_square_bracket_ = p__attribute_.m__right_square_bracket_;
				break;
			default:
				string[] args = new string[] { "_attribute_" };
				throw new SyntaxTreeError (SyntaxTreeError.InvalidKindError, args);
			}
		}

		// Clone node - wrapper of copy constructor

		public override SyntaxTreeBase Clone () { return (SyntaxTreeBase) new _attribute_ (this); }

		// Destructor

		public new void Dispose ()
		{
			base.Dispose ();
			--g_counter;

			m__left_square_bracket_?.Dispose ();
			m__identifier_?.Dispose ();
			m__name_value_list_optional_?.Dispose ();
			m__right_square_bracket_?.Dispose ();

			_init ();
		}
		#endregion
		#region Content changing and inclusion checking methods associated with the syntax rule <attribute>

		// Content changing function(s) associated with production(s) of syntax rule <attribute>

		//
		// Content changing function associated with following production(s)
		// <attribute> -> '[' <identifier> <name value list optional> ']'

		//

		public void change(SyntaxTreeToken p_token, SyntaxTreeToken p_token_1, _name_value_list_optional_ p__name_value_list_optional_, SyntaxTreeToken p_token_2)
		{
			_init ();
			this.kind = (uint) production_kind.g__attribute__1;
			children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 4);
			children [0] = m__left_square_bracket_ = p_token;
			children [1] = m__identifier_ = p_token_1;
			children [2] = m__name_value_list_optional_ = p__name_value_list_optional_;
			children [3] = m__right_square_bracket_ = p_token_2;
			reparent (parent);
		}

		//
		// check if node 'element' is included within syntax tree associated with syntax rule <attribute>
		//

		public new bool checkInclusion (SyntaxTreeBase element)
		{
			return
				(element == this) ||
				(m__left_square_bracket_ != null) && m__left_square_bracket_.checkInclusion (element) ||
				(m__identifier_ != null) && m__identifier_.checkInclusion (element) ||
				(m__name_value_list_optional_ != null) && m__name_value_list_optional_.checkInclusion (element) ||
				(m__right_square_bracket_ != null) && m__right_square_bracket_.checkInclusion (element) ||
				false;
		}
		#endregion
		#region Syntax tree emit methods associated with the syntax rule <attribute>

		//
		// emit production tree node associated with any production of syntax rule <attribute>
		//

		public override string Emit (int depth)
		{
			string s = "";
			if (depth != 0)
			switch ((production_kind) kind)
			{
				case production_kind.g__attribute__1:
					// emit syntax tree node associated with production
					// <attribute>: '[' <identifier> <name value list optional> ']'

					s += m__left_square_bracket_.Emit (depth - 1);
					s += " ";
					s += m__identifier_.Emit (depth - 1);
					s += " ";
					s += m__name_value_list_optional_.Emit (depth - 1);
					s += " ";
					s += m__right_square_bracket_.Emit (depth - 1);
				break;
			}
			return s.Trim ();
		}

		//
		// emit production tree node associated with any production of syntax rule <attribute>
		//

		public override string EmitProductionTree (int depth)
		{
			string s = "_attribute_ (";
			if (depth != 0)
				switch ((production_kind) kind)
				{
				case production_kind.g__attribute__1:
					// emit syntax tree node associated with production
					// <attribute>: '[' <identifier> <name value list optional> ']'

					s += "_left_square_bracket_";
					s += ' ';
					s += "_identifier_";
					s += ' ';
					s += m__name_value_list_optional_.EmitProductionTree (depth - 1);
					s += ' ';
					s += "_right_square_bracket_";
				break;
			}
			s += ")";
			return s;
		}
		#endregion
		#region Syntax tree traversal,  reperrent, init and iterator methods associated with the syntax rule <attribute>

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
			m__identifier_?.reparent (this);
			m__name_value_list_optional_?.reparent (this);
			m__right_square_bracket_?.reparent (this);
		}

		//
		// initialize object associated with syntax rule <attribute>
		//

		public void _init ()
		{
			m__left_square_bracket_ = null;
			m__identifier_ = null;
			m__name_value_list_optional_ = null;
			m__right_square_bracket_ = null;
		}
		#endregion
		#region Fields and properties associated with the syntax rule <attribute>

		// counter of all nodes associated with syntax rule <attribute>
		public static int g_counter;

		// objects associated with terminal and non-terminal symbols within production(s) of syntax rule <attribute>
		public SyntaxTreeToken m__left_square_bracket_ { get; private set; }
		public SyntaxTreeToken m__identifier_ { get; private set; }
		public _name_value_list_optional_ m__name_value_list_optional_ { get; private set; }
		public SyntaxTreeToken m__right_square_bracket_ { get; private set; }
		#endregion

	};

	public partial class SyntaxTreeWalker
	{
		#region delegates and events associated with the syntax rule <attribute>

		// delegate function (callback) prototype associated with syntax rule <attribute>
		public delegate bool _attribute__Callback (SyntaxTreeCallbackReason reason, _attribute_.production_kind kind, _attribute_ p__attribute_);

		// event associated with syntax rule <attribute>
		public event _attribute__Callback _attribute__Event;

		// event trigger associated with syntax rule <attribute>
		public bool Raise__attribute__Event (SyntaxTreeCallbackReason reason, _attribute_.production_kind kind, _attribute_ p__attribute_)
		{
			bool? status = _attribute__Event?.Invoke (reason, kind, p__attribute_);
			return (status == null) || status.Value;
		}
		#endregion
		#region syntax tree traversal methods associated with the syntax rule <attribute>
		//
		// traverse syntax tree node associated with any production of syntax rule <attribute>
		//

		public void Traverse (_attribute_ p__attribute_)
		{
			if (p__attribute_.isLocked())
				return;
			p__attribute_.dolock();
			_attribute_.production_kind kind = (_attribute_.production_kind) p__attribute_.kind;
			p__attribute_.turn_reset ();
			if (Raise__attribute__Event (SyntaxTreeCallbackReason.TraversalPrologueCallbackReason, kind, p__attribute_))
			switch (kind)
			{
				case _attribute_.production_kind.g__attribute__1:
					// traverse syntax tree node associated with production
					// <attribute>: '[' <identifier> <name value list optional> ']'

					if (Raise__attribute__Event (SyntaxTreeCallbackReason.TraversalMidTermCallbackReason, kind, p__attribute_))
						Traverse (p__attribute_.m__name_value_list_optional_);
					p__attribute_.turn_inc ();
					Raise__attribute__Event (SyntaxTreeCallbackReason.TraversalMidTermCallbackReason, kind, p__attribute_);
				break;
			}
			Raise__attribute__Event (SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason, kind, p__attribute_);
			p__attribute_.unlock();
		}

		//
		// traverse syntax tree node associated with any production of syntax rule <attribute>
		//

		public void TraverseCommon (_attribute_ p__attribute_)
		{
			_attribute_.production_kind kind = (_attribute_.production_kind) p__attribute_.kind;
			if (Raise_Common_Event (SyntaxTreeCallbackReason.TraversalPrologueCallbackReason, (int) kind, p__attribute_))
			switch (kind)
			{
				case _attribute_.production_kind.g__attribute__1:
					// traverse syntax tree node associated with production
					// <attribute>: '[' <identifier> <name value list optional> ']'

						TraverseCommon (p__attribute_.m__left_square_bracket_);
						TraverseCommon (p__attribute_.m__identifier_);
						TraverseCommon (p__attribute_.m__name_value_list_optional_);
						TraverseCommon (p__attribute_.m__right_square_bracket_);
				break;
			}
			Raise_Common_Event (SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason, (int) kind, p__attribute_);
		}
	};
		#endregion

	public partial class SyntaxTreeBuilder : SyntaxTreeWalker
	{
		#region syntax tree builder methods associated with the syntax rule <attribute>

		//
		// create syntax tree node associated with production
		// <attribute>: '[' <identifier> <name value list optional> ']'

		//

		public _attribute_ _attribute__1 (SyntaxTreeToken p_token, SyntaxTreeToken p_token_1, _name_value_list_optional_ p__name_value_list_optional_, SyntaxTreeToken p_token_2)
		{
			_attribute_ p__attribute__ref = new _attribute_(p_token, p_token_1, p__name_value_list_optional_, p_token_2);
			p_token.parent = p__attribute__ref;
			p_token_1.parent = p__attribute__ref;
			p__name_value_list_optional_.parent = p__attribute__ref;
			p_token_2.parent = p__attribute__ref;
			Raise__attribute__Event (SyntaxTreeCallbackReason.BuilderCallbackReason, _attribute_.production_kind.g__attribute__1, p__attribute__ref);
			return p__attribute__ref;
		}
		#endregion
	};
}
