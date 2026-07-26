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
	// class associated with syntax rule <regular expression usage>
	//

	public class	_regular_expression_usage_ : SyntaxTreeBase
	{
		#region enumerated production(s) of syntax rule <regular expression usage>
		public enum production_kind : ushort	// enumerated production(s) of syntax rule <regular expression usage>
		{
			g__regular_expression_usage__1 = 1,	// <regular expression> <actions optional>

		};
		#endregion
		#region production markers associated with the syntax rule <regular expression usage>

		// markers associated with production: <regular expression usage> -> <regular expression> <actions optional>

		public enum production_marker_1 : ushort
		{
			m__actions_optional_,
			final
		};
		#endregion
		#region Constructors and destructors associated with the syntax rule <regular expression usage>

		// Constructor declaration(s) associated with production(s) of syntax rule <regular expression usage>

		//
		// Constructor associated with the following production(s)
		// <regular expression usage> -> <regular expression> <actions optional>

		//

		public _regular_expression_usage_ (SyntaxTreeToken p_token, _actions_optional_ p__actions_optional_) : base ((uint) ProductionID.__regular_expression_usage__ID, (uint) production_kind.g__regular_expression_usage__1)
		{
			++g_counter;
			_init ();
			children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 2);
			children[0] = m__regular_expression_ = p_token;
			children[1] = m__actions_optional_ = p__actions_optional_;
		}

		// Copy constructor

		public _regular_expression_usage_ (_regular_expression_usage_ p__regular_expression_usage_) : base (p__regular_expression_usage_.id, p__regular_expression_usage_.kind)
		{
			++g_counter;
			_init ();
			switch ((production_kind) p__regular_expression_usage_.kind)
			{
				case production_kind.g__regular_expression_usage__1:
					children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 2);
					if ((children [0] = m__regular_expression_ = (p__regular_expression_usage_.m__regular_expression_ != null) ? new SyntaxTreeToken (p__regular_expression_usage_.m__regular_expression_) : null) != null) m__regular_expression_.parent = this;
					if ((children [1] = m__actions_optional_ = (p__regular_expression_usage_.m__actions_optional_ != null) ? new _actions_optional_ (p__regular_expression_usage_.m__actions_optional_) : null) != null) m__actions_optional_.parent = this;
					break;
				default:
				{
					string[] args = new string[] { "_regular_expression_usage_" };
					throw new SyntaxTreeError (SyntaxTreeError.InvalidKindError, args);
				}
			}
		}

		// Clone node - wrapper of copy constructor

		public override SyntaxTreeBase Clone () { return (SyntaxTreeBase) new _regular_expression_usage_ (this); }

		// Destructor

		public new void Dispose ()
		{
			base.Dispose ();
			--g_counter;

			m__regular_expression_?.Dispose ();
			m__actions_optional_?.Dispose ();

			_init ();
		}
		#endregion
		#region Content changing and inclusion checking methods associated with the syntax rule <regular expression usage>

		// Content changing function(s) associated with production(s) of syntax rule <regular expression usage>

		//
		// Content changing function associated with following production(s)
		// <regular expression usage> -> <regular expression> <actions optional>

		//

		public void change(SyntaxTreeToken p_token, _actions_optional_ p__actions_optional_)
		{
			_init ();
			this.kind = (uint) production_kind.g__regular_expression_usage__1;
			children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 2);
			children [0] = m__regular_expression_ = p_token;
			children [1] = m__actions_optional_ = p__actions_optional_;
			reparent (parent);
		}

		//
		// check if node 'element' is included within syntax tree associated with syntax rule <regular expression usage>
		//

		public new bool checkInclusion (SyntaxTreeBase element)
		{
			return
				(element == this) ||
				(m__regular_expression_ != null) && m__regular_expression_.checkInclusion (element) ||
				(m__actions_optional_ != null) && m__actions_optional_.checkInclusion (element) ||
				false;
		}
		#endregion
		#region Syntax tree emit methods associated with the syntax rule <regular expression usage>

		//
		// emit production tree node associated with any production of syntax rule <regular expression usage>
		//

		public override string Emit (int depth)
		{
			string s = "";
			if (depth != 0)
			switch ((production_kind) kind)
			{
				case production_kind.g__regular_expression_usage__1:
					// emit syntax tree node associated with production
					// <regular expression usage>: <regular expression> <actions optional>

					s += m__regular_expression_.Emit (depth - 1);
					s += " ";
					s += m__actions_optional_.Emit (depth - 1);
				break;
			}
			return s.Trim ();
		}

		//
		// emit production tree node associated with any production of syntax rule <regular expression usage>
		//

		public override string EmitProductionTree (int depth)
		{
			string s = "_regular_expression_usage_ (";
			if (depth != 0)
				switch ((production_kind) kind)
				{
				case production_kind.g__regular_expression_usage__1:
					// emit syntax tree node associated with production
					// <regular expression usage>: <regular expression> <actions optional>

					s += "_regular_expression_";
					s += ' ';
					s += m__actions_optional_.EmitProductionTree (depth - 1);
				break;
			}
			s += ")";
			return s;
		}
		#endregion
		#region Syntax tree traversal,  reperrent, init and iterator methods associated with the syntax rule <regular expression usage>

		//
		// traverse sub-tree rooted in this node using selected syntax tree walker
		//

		public override void InvokeTraverse (SyntaxTreeWalkerCore syntaxTreeWalker) => ((SyntaxTreeWalker) syntaxTreeWalker).Traverse (this);
		public override void InvokeTraverseCommon (SyntaxTreeWalkerCore syntaxTreeWalker) => ((SyntaxTreeWalker) syntaxTreeWalker).TraverseCommon (this);

		// reparent sub-tree

		public override void reparent (SyntaxTreeBase parent)
		{
			this.parent = parent;

			m__regular_expression_?.reparent (this);
			m__actions_optional_?.reparent (this);
		}

		//
		// initialize object associated with syntax rule <regular expression usage>
		//

		public void _init ()
		{
			m__regular_expression_ = null;
			m__actions_optional_ = null;
		}
		#endregion
		#region Fields and properties associated with the syntax rule <regular expression usage>

		// counter of all nodes associated with syntax rule <regular expression usage>
		public static int g_counter;

		// objects associated with terminal and non-terminal symbols within production(s) of syntax rule <regular expression usage>
		public SyntaxTreeToken m__regular_expression_ { get; private set; }
		public _actions_optional_ m__actions_optional_ { get; private set; }
		#endregion

	};

	public partial class SyntaxTreeWalker
	{
		#region delegates and events associated with the syntax rule <regular expression usage>

		// delegate function (callback) prototype associated with syntax rule <regular expression usage>
		public delegate bool _regular_expression_usage__Callback (SyntaxTreeCallbackReason reason, _regular_expression_usage_.production_kind kind, _regular_expression_usage_ p__regular_expression_usage_);

		// event associated with syntax rule <regular expression usage>
		public event _regular_expression_usage__Callback _regular_expression_usage__Event;

		// event trigger associated with syntax rule <regular expression usage>
		public bool Raise__regular_expression_usage__Event (SyntaxTreeCallbackReason reason, _regular_expression_usage_.production_kind kind, _regular_expression_usage_ p__regular_expression_usage_)
		{
			bool? status = _regular_expression_usage__Event?.Invoke (reason, kind, p__regular_expression_usage_);
			return (status == null) || status.Value;
		}
		#endregion
		#region syntax tree traversal methods associated with the syntax rule <regular expression usage>
		//
		// traverse syntax tree node associated with any production of syntax rule <regular expression usage>
		//

		public void Traverse (_regular_expression_usage_ p__regular_expression_usage_)
		{
			if (p__regular_expression_usage_.isLocked())
				return;
			p__regular_expression_usage_.dolock();
			_regular_expression_usage_.production_kind kind = (_regular_expression_usage_.production_kind) p__regular_expression_usage_.kind;
			p__regular_expression_usage_.turn_reset ();
			if (Raise__regular_expression_usage__Event (SyntaxTreeCallbackReason.TraversalPrologueCallbackReason, kind, p__regular_expression_usage_))
			switch (kind)
			{
				case _regular_expression_usage_.production_kind.g__regular_expression_usage__1:
					// traverse syntax tree node associated with production
					// <regular expression usage>: <regular expression> <actions optional>

					if (Raise__regular_expression_usage__Event (SyntaxTreeCallbackReason.TraversalMidTermCallbackReason, kind, p__regular_expression_usage_))
						Traverse (p__regular_expression_usage_.m__actions_optional_);
					p__regular_expression_usage_.turn_inc ();
					Raise__regular_expression_usage__Event (SyntaxTreeCallbackReason.TraversalMidTermCallbackReason, kind, p__regular_expression_usage_);
				break;
			}
			Raise__regular_expression_usage__Event (SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason, kind, p__regular_expression_usage_);
			p__regular_expression_usage_.unlock();
		}

		//
		// traverse syntax tree node associated with any production of syntax rule <regular expression usage>
		//

		public void TraverseCommon (_regular_expression_usage_ p__regular_expression_usage_)
		{
			_regular_expression_usage_.production_kind kind = (_regular_expression_usage_.production_kind) p__regular_expression_usage_.kind;
			if (Raise_Common_Event (SyntaxTreeCallbackReason.TraversalPrologueCallbackReason, (int) kind, p__regular_expression_usage_))
			switch (kind)
			{
				case _regular_expression_usage_.production_kind.g__regular_expression_usage__1:
					// traverse syntax tree node associated with production
					// <regular expression usage>: <regular expression> <actions optional>

						TraverseCommon (p__regular_expression_usage_.m__regular_expression_);
						TraverseCommon (p__regular_expression_usage_.m__actions_optional_);
				break;
			}
			Raise_Common_Event (SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason, (int) kind, p__regular_expression_usage_);
		}
	};
		#endregion

	public partial class SyntaxTreeBuilder : SyntaxTreeWalker
	{
		#region syntax tree builder methods associated with the syntax rule <regular expression usage>

		//
		// create syntax tree node associated with production
		// <regular expression usage>: <regular expression> <actions optional>

		//

		public _regular_expression_usage_ _regular_expression_usage__1 (SyntaxTreeToken p_token, _actions_optional_ p__actions_optional_)
		{
			_regular_expression_usage_ p__regular_expression_usage__ref = new _regular_expression_usage_(p_token, p__actions_optional_);
			p_token.parent = p__regular_expression_usage__ref;
			p__actions_optional_.parent = p__regular_expression_usage__ref;
			Raise__regular_expression_usage__Event (SyntaxTreeCallbackReason.BuilderCallbackReason, _regular_expression_usage_.production_kind.g__regular_expression_usage__1, p__regular_expression_usage__ref);
			return p__regular_expression_usage__ref;
		}
		#endregion
	};
}
