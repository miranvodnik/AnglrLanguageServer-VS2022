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
	// class associated with syntax rule <push action>
	//

	public class	_push_action_ : SyntaxTreeBase
	{
		#region enumerated production(s) of syntax rule <push action>
		public enum production_kind : ushort	// enumerated production(s) of syntax rule <push action>
		{
			g__push_action__1 = 1,	// 'push' <identifier>

		};
		#endregion
		#region production markers associated with the syntax rule <push action>

		// markers associated with production: <push action> -> 'push' <identifier>

		public enum production_marker_1 : ushort
		{
			final
		};
		#endregion
		#region Constructors and destructors associated with the syntax rule <push action>

		// Constructor declaration(s) associated with production(s) of syntax rule <push action>

		//
		// Constructor associated with the following production(s)
		// <push action> -> 'push' <identifier>

		//

		public _push_action_ (SyntaxTreeToken p_token, SyntaxTreeToken p_token_1) : base ((uint) ProductionID.__push_action__ID, (uint) production_kind.g__push_action__1)
		{
			++g_counter;
			_init ();
			children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 2);
			children[0] = m__push_ = p_token;
			children[1] = m__identifier_ = p_token_1;
		}

		// Copy constructor

		public _push_action_ (_push_action_ p__push_action_) : base (p__push_action_.id, p__push_action_.kind)
		{
			++g_counter;
			_init ();
			switch ((production_kind) this.kind)
			{
			case production_kind.g__push_action__1:
				children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 2);
				children[0] = m__push_ = p__push_action_.m__push_;
				children[1] = m__identifier_ = p__push_action_.m__identifier_;
				break;
			default:
				string[] args = new string[] { "_push_action_" };
				throw new SyntaxTreeError (SyntaxTreeError.InvalidKindError, args);
			}
		}

		// Clone node - wrapper of copy constructor

		public override SyntaxTreeBase Clone () { return (SyntaxTreeBase) new _push_action_ (this); }

		// Destructor

		public new void Dispose ()
		{
			base.Dispose ();
			--g_counter;

			m__push_?.Dispose ();
			m__identifier_?.Dispose ();

			_init ();
		}
		#endregion
		#region Content changing and inclusion checking methods associated with the syntax rule <push action>

		// Content changing function(s) associated with production(s) of syntax rule <push action>

		//
		// Content changing function associated with following production(s)
		// <push action> -> 'push' <identifier>

		//

		public void change(SyntaxTreeToken p_token, SyntaxTreeToken p_token_1)
		{
			_init ();
			this.kind = (uint) production_kind.g__push_action__1;
			children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 2);
			children [0] = m__push_ = p_token;
			children [1] = m__identifier_ = p_token_1;
			reparent (parent);
		}

		//
		// check if node 'element' is included within syntax tree associated with syntax rule <push action>
		//

		public new bool checkInclusion (SyntaxTreeBase element)
		{
			return
				(element == this) ||
				(m__push_ != null) && m__push_.checkInclusion (element) ||
				(m__identifier_ != null) && m__identifier_.checkInclusion (element) ||
				false;
		}
		#endregion
		#region Syntax tree emit methods associated with the syntax rule <push action>

		//
		// emit production tree node associated with any production of syntax rule <push action>
		//

		public override string Emit (int depth)
		{
			string s = "";
			if (depth != 0)
			switch ((production_kind) kind)
			{
				case production_kind.g__push_action__1:
					// emit syntax tree node associated with production
					// <push action>: 'push' <identifier>

					s += m__push_.Emit (depth - 1);
					s += " ";
					s += m__identifier_.Emit (depth - 1);
				break;
			}
			return s.Trim ();
		}

		//
		// emit production tree node associated with any production of syntax rule <push action>
		//

		public override string EmitProductionTree (int depth)
		{
			string s = "_push_action_ (";
			if (depth != 0)
				switch ((production_kind) kind)
				{
				case production_kind.g__push_action__1:
					// emit syntax tree node associated with production
					// <push action>: 'push' <identifier>

					s += "_push_";
					s += ' ';
					s += "_identifier_";
				break;
			}
			s += ")";
			return s;
		}
		#endregion
		#region Syntax tree traversal,  reperrent, init and iterator methods associated with the syntax rule <push action>

		//
		// traverse sub-tree rooted in this node using selected syntax tree walker
		//

		public override void InvokeTraverse (SyntaxTreeWalkerCore syntaxTreeWalker) => ((SyntaxTreeWalker) syntaxTreeWalker).Traverse (this);
		public override void InvokeTraverseCommon (SyntaxTreeWalkerCore syntaxTreeWalker) => ((SyntaxTreeWalker) syntaxTreeWalker).TraverseCommon (this);

		// reparent sub-tree

		public override void reparent (SyntaxTreeBase parent)
		{
			this.parent = parent;

			m__push_?.reparent (this);
			m__identifier_?.reparent (this);
		}

		//
		// initialize object associated with syntax rule <push action>
		//

		public void _init ()
		{
			m__push_ = null;
			m__identifier_ = null;
		}
		#endregion
		#region Fields and properties associated with the syntax rule <push action>

		// counter of all nodes associated with syntax rule <push action>
		public static int g_counter;

		// objects associated with terminal and non-terminal symbols within production(s) of syntax rule <push action>
		public SyntaxTreeToken m__push_ { get; private set; }
		public SyntaxTreeToken m__identifier_ { get; private set; }
		#endregion

	};

	public partial class SyntaxTreeWalker
	{
		#region delegates and events associated with the syntax rule <push action>

		// delegate function (callback) prototype associated with syntax rule <push action>
		public delegate bool _push_action__Callback (SyntaxTreeCallbackReason reason, _push_action_.production_kind kind, _push_action_ p__push_action_);

		// event associated with syntax rule <push action>
		public event _push_action__Callback _push_action__Event;

		// event trigger associated with syntax rule <push action>
		public bool Raise__push_action__Event (SyntaxTreeCallbackReason reason, _push_action_.production_kind kind, _push_action_ p__push_action_)
		{
			bool? status = _push_action__Event?.Invoke (reason, kind, p__push_action_);
			return (status == null) || status.Value;
		}
		#endregion
		#region syntax tree traversal methods associated with the syntax rule <push action>
		//
		// traverse syntax tree node associated with any production of syntax rule <push action>
		//

		public void Traverse (_push_action_ p__push_action_)
		{
			if (p__push_action_.isLocked())
				return;
			p__push_action_.dolock();
			_push_action_.production_kind kind = (_push_action_.production_kind) p__push_action_.kind;
			p__push_action_.turn_reset ();
			if (Raise__push_action__Event (SyntaxTreeCallbackReason.TraversalPrologueCallbackReason, kind, p__push_action_))
			switch (kind)
			{
				case _push_action_.production_kind.g__push_action__1:
					// traverse syntax tree node associated with production
					// <push action>: 'push' <identifier>

				break;
			}
			Raise__push_action__Event (SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason, kind, p__push_action_);
			p__push_action_.unlock();
		}

		//
		// traverse syntax tree node associated with any production of syntax rule <push action>
		//

		public void TraverseCommon (_push_action_ p__push_action_)
		{
			_push_action_.production_kind kind = (_push_action_.production_kind) p__push_action_.kind;
			if (Raise_Common_Event (SyntaxTreeCallbackReason.TraversalPrologueCallbackReason, (int) kind, p__push_action_))
			switch (kind)
			{
				case _push_action_.production_kind.g__push_action__1:
					// traverse syntax tree node associated with production
					// <push action>: 'push' <identifier>

						TraverseCommon (p__push_action_.m__push_);
						TraverseCommon (p__push_action_.m__identifier_);
				break;
			}
			Raise_Common_Event (SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason, (int) kind, p__push_action_);
		}
	};
		#endregion

	public partial class SyntaxTreeBuilder : SyntaxTreeWalker
	{
		#region syntax tree builder methods associated with the syntax rule <push action>

		//
		// create syntax tree node associated with production
		// <push action>: 'push' <identifier>

		//

		public _push_action_ _push_action__1 (SyntaxTreeToken p_token, SyntaxTreeToken p_token_1)
		{
			_push_action_ p__push_action__ref = new _push_action_(p_token, p_token_1);
			p_token.parent = p__push_action__ref;
			p_token_1.parent = p__push_action__ref;
			Raise__push_action__Event (SyntaxTreeCallbackReason.BuilderCallbackReason, _push_action_.production_kind.g__push_action__1, p__push_action__ref);
			return p__push_action__ref;
		}
		#endregion
	};
}
