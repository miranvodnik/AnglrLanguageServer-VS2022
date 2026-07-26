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
	// class associated with syntax rule <general part>
	//

	public class	_general_part_ : SyntaxTreeBase
	{
		#region enumerated production(s) of syntax rule <general part>
		public enum production_kind : ushort	// enumerated production(s) of syntax rule <general part>
		{
			g__general_part__1 = 1,	// <attribute list optional> '%general' <identifier> '%{' <attribute list optional> '%}'

		};
		#endregion
		#region production markers associated with the syntax rule <general part>

		// markers associated with production: <general part> -> <attribute list optional> '%general' <identifier> '%{' <attribute list optional> '%}'

		public enum production_marker_1 : ushort
		{
			m__attribute_list_optional_,
			m__attribute_list_optional__1,
			final
		};
		#endregion
		#region Constructors and destructors associated with the syntax rule <general part>

		// Constructor declaration(s) associated with production(s) of syntax rule <general part>

		//
		// Constructor associated with the following production(s)
		// <general part> -> <attribute list optional> '%general' <identifier> '%{' <attribute list optional> '%}'

		//

		public _general_part_ (_attribute_list_optional_ p__attribute_list_optional_, SyntaxTreeToken p_token, SyntaxTreeToken p_token_1, SyntaxTreeToken p_token_2, _attribute_list_optional_ p__attribute_list_optional__1, SyntaxTreeToken p_token_3) : base ((uint) ProductionID.__general_part__ID, (uint) production_kind.g__general_part__1)
		{
			++g_counter;
			_init ();
			children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 6);
			children[0] = m__attribute_list_optional_ = p__attribute_list_optional_;
			children[1] = m__general_ = p_token;
			children[2] = m__identifier_ = p_token_1;
			children[3] = m__left_part_bracket_ = p_token_2;
			children[4] = m__attribute_list_optional__1 = p__attribute_list_optional__1;
			children[5] = m__right_part_bracket_ = p_token_3;
		}

		// Copy constructor

		public _general_part_ (_general_part_ p__general_part_) : base (p__general_part_.id, p__general_part_.kind)
		{
			++g_counter;
			_init ();
			switch ((production_kind) p__general_part_.kind)
			{
				case production_kind.g__general_part__1:
					children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 6);
					if ((children [0] = m__attribute_list_optional_ = (p__general_part_.m__attribute_list_optional_ != null) ? new _attribute_list_optional_ (p__general_part_.m__attribute_list_optional_) : null) != null) m__attribute_list_optional_.parent = this;
					if ((children [1] = m__general_ = (p__general_part_.m__general_ != null) ? new SyntaxTreeToken (p__general_part_.m__general_) : null) != null) m__general_.parent = this;
					if ((children [2] = m__identifier_ = (p__general_part_.m__identifier_ != null) ? new SyntaxTreeToken (p__general_part_.m__identifier_) : null) != null) m__identifier_.parent = this;
					if ((children [3] = m__left_part_bracket_ = (p__general_part_.m__left_part_bracket_ != null) ? new SyntaxTreeToken (p__general_part_.m__left_part_bracket_) : null) != null) m__left_part_bracket_.parent = this;
					if ((children [4] = m__attribute_list_optional__1 = (p__general_part_.m__attribute_list_optional__1 != null) ? new _attribute_list_optional_ (p__general_part_.m__attribute_list_optional__1) : null) != null) m__attribute_list_optional__1.parent = this;
					if ((children [5] = m__right_part_bracket_ = (p__general_part_.m__right_part_bracket_ != null) ? new SyntaxTreeToken (p__general_part_.m__right_part_bracket_) : null) != null) m__right_part_bracket_.parent = this;
					break;
				default:
				{
					string[] args = new string[] { "_general_part_" };
					throw new SyntaxTreeError (SyntaxTreeError.InvalidKindError, args);
				}
			}
		}

		// Clone node - wrapper of copy constructor

		public override SyntaxTreeBase Clone () { return (SyntaxTreeBase) new _general_part_ (this); }

		// Destructor

		public new void Dispose ()
		{
			base.Dispose ();
			--g_counter;

			m__attribute_list_optional_?.Dispose ();
			m__attribute_list_optional__1?.Dispose ();
			m__general_?.Dispose ();
			m__identifier_?.Dispose ();
			m__left_part_bracket_?.Dispose ();
			m__right_part_bracket_?.Dispose ();

			_init ();
		}
		#endregion
		#region Content changing and inclusion checking methods associated with the syntax rule <general part>

		// Content changing function(s) associated with production(s) of syntax rule <general part>

		//
		// Content changing function associated with following production(s)
		// <general part> -> <attribute list optional> '%general' <identifier> '%{' <attribute list optional> '%}'

		//

		public void change(_attribute_list_optional_ p__attribute_list_optional_, SyntaxTreeToken p_token, SyntaxTreeToken p_token_1, SyntaxTreeToken p_token_2, _attribute_list_optional_ p__attribute_list_optional__1, SyntaxTreeToken p_token_3)
		{
			_init ();
			this.kind = (uint) production_kind.g__general_part__1;
			children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 6);
			children [0] = m__attribute_list_optional_ = p__attribute_list_optional_;
			children [1] = m__general_ = p_token;
			children [2] = m__identifier_ = p_token_1;
			children [3] = m__left_part_bracket_ = p_token_2;
			children [4] = m__attribute_list_optional__1 = p__attribute_list_optional__1;
			children [5] = m__right_part_bracket_ = p_token_3;
			reparent (parent);
		}

		//
		// check if node 'element' is included within syntax tree associated with syntax rule <general part>
		//

		public new bool checkInclusion (SyntaxTreeBase element)
		{
			return
				(element == this) ||
				(m__attribute_list_optional_ != null) && m__attribute_list_optional_.checkInclusion (element) ||
				(m__attribute_list_optional__1 != null) && m__attribute_list_optional__1.checkInclusion (element) ||
				(m__general_ != null) && m__general_.checkInclusion (element) ||
				(m__identifier_ != null) && m__identifier_.checkInclusion (element) ||
				(m__left_part_bracket_ != null) && m__left_part_bracket_.checkInclusion (element) ||
				(m__right_part_bracket_ != null) && m__right_part_bracket_.checkInclusion (element) ||
				false;
		}
		#endregion
		#region Syntax tree emit methods associated with the syntax rule <general part>

		//
		// emit production tree node associated with any production of syntax rule <general part>
		//

		public override string Emit (int depth)
		{
			string s = "";
			if (depth != 0)
			switch ((production_kind) kind)
			{
				case production_kind.g__general_part__1:
					// emit syntax tree node associated with production
					// <general part>: <attribute list optional> '%general' <identifier> '%{' <attribute list optional> '%}'

					s += m__attribute_list_optional_.Emit (depth - 1);
					s += " ";
					s += m__general_.Emit (depth - 1);
					s += " ";
					s += m__identifier_.Emit (depth - 1);
					s += " ";
					s += m__left_part_bracket_.Emit (depth - 1);
					s += " ";
					s += m__attribute_list_optional__1.Emit (depth - 1);
					s += " ";
					s += m__right_part_bracket_.Emit (depth - 1);
				break;
			}
			return s.Trim ();
		}

		//
		// emit production tree node associated with any production of syntax rule <general part>
		//

		public override string EmitProductionTree (int depth)
		{
			string s = "_general_part_ (";
			if (depth != 0)
				switch ((production_kind) kind)
				{
				case production_kind.g__general_part__1:
					// emit syntax tree node associated with production
					// <general part>: <attribute list optional> '%general' <identifier> '%{' <attribute list optional> '%}'

					s += m__attribute_list_optional_.EmitProductionTree (depth - 1);
					s += ' ';
					s += "_general_";
					s += ' ';
					s += "_identifier_";
					s += ' ';
					s += "_left_part_bracket_";
					s += ' ';
					s += m__attribute_list_optional__1.EmitProductionTree (depth - 1);
					s += ' ';
					s += "_right_part_bracket_";
				break;
			}
			s += ")";
			return s;
		}
		#endregion
		#region Syntax tree traversal,  reperrent, init and iterator methods associated with the syntax rule <general part>

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
			m__attribute_list_optional__1?.reparent (this);

			m__general_?.reparent (this);
			m__identifier_?.reparent (this);
			m__left_part_bracket_?.reparent (this);
			m__right_part_bracket_?.reparent (this);
		}

		//
		// initialize object associated with syntax rule <general part>
		//

		public void _init ()
		{
			m__attribute_list_optional_ = null;
			m__attribute_list_optional__1 = null;
			m__general_ = null;
			m__identifier_ = null;
			m__left_part_bracket_ = null;
			m__right_part_bracket_ = null;
		}
		#endregion
		#region Fields and properties associated with the syntax rule <general part>

		// counter of all nodes associated with syntax rule <general part>
		public static int g_counter;

		// objects associated with terminal and non-terminal symbols within production(s) of syntax rule <general part>
		public _attribute_list_optional_ m__attribute_list_optional_ { get; private set; }
		public _attribute_list_optional_ m__attribute_list_optional__1 { get; private set; }
		public SyntaxTreeToken m__general_ { get; private set; }
		public SyntaxTreeToken m__identifier_ { get; private set; }
		public SyntaxTreeToken m__left_part_bracket_ { get; private set; }
		public SyntaxTreeToken m__right_part_bracket_ { get; private set; }
		#endregion

	};

	public partial class SyntaxTreeWalker
	{
		#region delegates and events associated with the syntax rule <general part>

		// delegate function (callback) prototype associated with syntax rule <general part>
		public delegate bool _general_part__Callback (SyntaxTreeCallbackReason reason, _general_part_.production_kind kind, _general_part_ p__general_part_);

		// event associated with syntax rule <general part>
		public event _general_part__Callback _general_part__Event;

		// event trigger associated with syntax rule <general part>
		public bool Raise__general_part__Event (SyntaxTreeCallbackReason reason, _general_part_.production_kind kind, _general_part_ p__general_part_)
		{
			bool? status = _general_part__Event?.Invoke (reason, kind, p__general_part_);
			return (status == null) || status.Value;
		}
		#endregion
		#region syntax tree traversal methods associated with the syntax rule <general part>
		//
		// traverse syntax tree node associated with any production of syntax rule <general part>
		//

		public void Traverse (_general_part_ p__general_part_)
		{
			if (p__general_part_.isLocked())
				return;
			p__general_part_.dolock();
			_general_part_.production_kind kind = (_general_part_.production_kind) p__general_part_.kind;
			p__general_part_.turn_reset ();
			if (Raise__general_part__Event (SyntaxTreeCallbackReason.TraversalPrologueCallbackReason, kind, p__general_part_))
			switch (kind)
			{
				case _general_part_.production_kind.g__general_part__1:
					// traverse syntax tree node associated with production
					// <general part>: <attribute list optional> '%general' <identifier> '%{' <attribute list optional> '%}'

					if (Raise__general_part__Event (SyntaxTreeCallbackReason.TraversalMidTermCallbackReason, kind, p__general_part_))
						Traverse (p__general_part_.m__attribute_list_optional_);
					p__general_part_.turn_inc ();
					if (Raise__general_part__Event (SyntaxTreeCallbackReason.TraversalMidTermCallbackReason, kind, p__general_part_))
						Traverse (p__general_part_.m__attribute_list_optional__1);
					p__general_part_.turn_inc ();
					Raise__general_part__Event (SyntaxTreeCallbackReason.TraversalMidTermCallbackReason, kind, p__general_part_);
				break;
			}
			Raise__general_part__Event (SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason, kind, p__general_part_);
			p__general_part_.unlock();
		}

		//
		// traverse syntax tree node associated with any production of syntax rule <general part>
		//

		public void TraverseCommon (_general_part_ p__general_part_)
		{
			_general_part_.production_kind kind = (_general_part_.production_kind) p__general_part_.kind;
			if (Raise_Common_Event (SyntaxTreeCallbackReason.TraversalPrologueCallbackReason, (int) kind, p__general_part_))
			switch (kind)
			{
				case _general_part_.production_kind.g__general_part__1:
					// traverse syntax tree node associated with production
					// <general part>: <attribute list optional> '%general' <identifier> '%{' <attribute list optional> '%}'

						TraverseCommon (p__general_part_.m__attribute_list_optional_);
						TraverseCommon (p__general_part_.m__general_);
						TraverseCommon (p__general_part_.m__identifier_);
						TraverseCommon (p__general_part_.m__left_part_bracket_);
						TraverseCommon (p__general_part_.m__attribute_list_optional__1);
						TraverseCommon (p__general_part_.m__right_part_bracket_);
				break;
			}
			Raise_Common_Event (SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason, (int) kind, p__general_part_);
		}
	};
		#endregion

	public partial class SyntaxTreeBuilder : SyntaxTreeWalker
	{
		#region syntax tree builder methods associated with the syntax rule <general part>

		//
		// create syntax tree node associated with production
		// <general part>: <attribute list optional> '%general' <identifier> '%{' <attribute list optional> '%}'

		//

		public _general_part_ _general_part__1 (_attribute_list_optional_ p__attribute_list_optional_, SyntaxTreeToken p_token, SyntaxTreeToken p_token_1, SyntaxTreeToken p_token_2, _attribute_list_optional_ p__attribute_list_optional__1, SyntaxTreeToken p_token_3)
		{
			_general_part_ p__general_part__ref = new _general_part_(p__attribute_list_optional_, p_token, p_token_1, p_token_2, p__attribute_list_optional__1, p_token_3);
			p__attribute_list_optional_.parent = p__general_part__ref;
			p_token.parent = p__general_part__ref;
			p_token_1.parent = p__general_part__ref;
			p_token_2.parent = p__general_part__ref;
			p__attribute_list_optional__1.parent = p__general_part__ref;
			p_token_3.parent = p__general_part__ref;
			Raise__general_part__Event (SyntaxTreeCallbackReason.BuilderCallbackReason, _general_part_.production_kind.g__general_part__1, p__general_part__ref);
			return p__general_part__ref;
		}
		#endregion
	};
}
