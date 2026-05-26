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
	// class associated with syntax rule <skip action>
	//

	public class	_skip_action_ : SyntaxTreeBase
	{
		#region enumerated production(s) of syntax rule <skip action>
		public enum production_kind : ushort	// enumerated production(s) of syntax rule <skip action>
		{
			g__skip_action__1 = 1,	// 'skip'

		};
		#endregion
		#region production markers associated with the syntax rule <skip action>

		// markers associated with production: <skip action> -> 'skip'

		public enum production_marker_1 : ushort
		{
			final
		};
		#endregion
		#region Constructors and destructors associated with the syntax rule <skip action>

		// Constructor declaration(s) associated with production(s) of syntax rule <skip action>

		//
		// Constructor associated with the following production(s)
		// <skip action> -> 'skip'

		//

		public _skip_action_ (SyntaxTreeToken p_token) : base ((uint) ProductionID.__skip_action__ID, (uint) production_kind.g__skip_action__1)
		{
			++g_counter;
			_init ();
			children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 1);
			children[0] = m__skip_ = p_token;
		}

		// Copy constructor

		public _skip_action_ (_skip_action_ p__skip_action_) : base (p__skip_action_.id, p__skip_action_.kind)
		{
			++g_counter;
			_init ();
			switch ((production_kind) this.kind)
			{
			case production_kind.g__skip_action__1:
				children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 1);
				children[0] = m__skip_ = p__skip_action_.m__skip_;
				break;
			default:
				string[] args = new string[] { "_skip_action_" };
				throw new SyntaxTreeError (SyntaxTreeError.InvalidKindError, args);
			}
		}

		// Clone node - wrapper of copy constructor

		public override SyntaxTreeBase Clone () { return (SyntaxTreeBase) new _skip_action_ (this); }

		// Destructor

		public new void Dispose ()
		{
			base.Dispose ();
			--g_counter;

			m__skip_?.Dispose ();

			_init ();
		}
		#endregion
		#region Content changing and inclusion checking methods associated with the syntax rule <skip action>

		// Content changing function(s) associated with production(s) of syntax rule <skip action>

		//
		// Content changing function associated with following production(s)
		// <skip action> -> 'skip'

		//

		public void change(SyntaxTreeToken p_token)
		{
			_init ();
			this.kind = (uint) production_kind.g__skip_action__1;
			children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 1);
			children [0] = m__skip_ = p_token;
			reparent (parent);
		}

		//
		// check if node 'element' is included within syntax tree associated with syntax rule <skip action>
		//

		public new bool checkInclusion (SyntaxTreeBase element)
		{
			return
				(element == this) ||
				(m__skip_ != null) && m__skip_.checkInclusion (element) ||
				false;
		}
		#endregion
		#region Syntax tree emit methods associated with the syntax rule <skip action>

		//
		// emit production tree node associated with any production of syntax rule <skip action>
		//

		public override string Emit (int depth)
		{
			string s = "";
			if (depth != 0)
			switch ((production_kind) kind)
			{
				case production_kind.g__skip_action__1:
					// emit syntax tree node associated with production
					// <skip action>: 'skip'

					s += m__skip_.Emit (depth - 1);
				break;
			}
			return s.Trim ();
		}

		//
		// emit production tree node associated with any production of syntax rule <skip action>
		//

		public override string EmitProductionTree (int depth)
		{
			string s = "_skip_action_ (";
			if (depth != 0)
				switch ((production_kind) kind)
				{
				case production_kind.g__skip_action__1:
					// emit syntax tree node associated with production
					// <skip action>: 'skip'

					s += "_skip_";
				break;
			}
			s += ")";
			return s;
		}
		#endregion
		#region Syntax tree traversal,  reperrent, init and iterator methods associated with the syntax rule <skip action>

		//
		// traverse sub-tree rooted in this node using selected syntax tree walker
		//

		public override void InvokeTraverse (SyntaxTreeWalkerCore syntaxTreeWalker) => ((SyntaxTreeWalker) syntaxTreeWalker).Traverse (this);
		public override void InvokeTraverseCommon (SyntaxTreeWalkerCore syntaxTreeWalker) => ((SyntaxTreeWalker) syntaxTreeWalker).TraverseCommon (this);

		// reparent sub-tree

		public override void reparent (SyntaxTreeBase parent)
		{
			this.parent = parent;

			m__skip_?.reparent (this);
		}

		//
		// initialize object associated with syntax rule <skip action>
		//

		public void _init ()
		{
			m__skip_ = null;
		}
		#endregion
		#region Fields and properties associated with the syntax rule <skip action>

		// counter of all nodes associated with syntax rule <skip action>
		public static int g_counter;

		// objects associated with terminal and non-terminal symbols within production(s) of syntax rule <skip action>
		public SyntaxTreeToken m__skip_ { get; private set; }
		#endregion

	};

	public partial class SyntaxTreeWalker
	{
		#region delegates and events associated with the syntax rule <skip action>

		// delegate function (callback) prototype associated with syntax rule <skip action>
		public delegate bool _skip_action__Callback (SyntaxTreeCallbackReason reason, _skip_action_.production_kind kind, _skip_action_ p__skip_action_);

		// event associated with syntax rule <skip action>
		public event _skip_action__Callback _skip_action__Event;

		// event trigger associated with syntax rule <skip action>
		public bool Raise__skip_action__Event (SyntaxTreeCallbackReason reason, _skip_action_.production_kind kind, _skip_action_ p__skip_action_)
		{
			bool? status = _skip_action__Event?.Invoke (reason, kind, p__skip_action_);
			return (status == null) || status.Value;
		}
		#endregion
		#region syntax tree traversal methods associated with the syntax rule <skip action>
		//
		// traverse syntax tree node associated with any production of syntax rule <skip action>
		//

		public void Traverse (_skip_action_ p__skip_action_)
		{
			if (p__skip_action_.isLocked())
				return;
			p__skip_action_.dolock();
			_skip_action_.production_kind kind = (_skip_action_.production_kind) p__skip_action_.kind;
			p__skip_action_.turn_reset ();
			if (Raise__skip_action__Event (SyntaxTreeCallbackReason.TraversalPrologueCallbackReason, kind, p__skip_action_))
			switch (kind)
			{
				case _skip_action_.production_kind.g__skip_action__1:
					// traverse syntax tree node associated with production
					// <skip action>: 'skip'

				break;
			}
			Raise__skip_action__Event (SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason, kind, p__skip_action_);
			p__skip_action_.unlock();
		}

		//
		// traverse syntax tree node associated with any production of syntax rule <skip action>
		//

		public void TraverseCommon (_skip_action_ p__skip_action_)
		{
			_skip_action_.production_kind kind = (_skip_action_.production_kind) p__skip_action_.kind;
			if (Raise_Common_Event (SyntaxTreeCallbackReason.TraversalPrologueCallbackReason, (int) kind, p__skip_action_))
			switch (kind)
			{
				case _skip_action_.production_kind.g__skip_action__1:
					// traverse syntax tree node associated with production
					// <skip action>: 'skip'

						TraverseCommon (p__skip_action_.m__skip_);
				break;
			}
			Raise_Common_Event (SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason, (int) kind, p__skip_action_);
		}
	};
		#endregion

	public partial class SyntaxTreeBuilder : SyntaxTreeWalker
	{
		#region syntax tree builder methods associated with the syntax rule <skip action>

		//
		// create syntax tree node associated with production
		// <skip action>: 'skip'

		//

		public _skip_action_ _skip_action__1 (SyntaxTreeToken p_token)
		{
			_skip_action_ p__skip_action__ref = new _skip_action_(p_token);
			p_token.parent = p__skip_action__ref;
			Raise__skip_action__Event (SyntaxTreeCallbackReason.BuilderCallbackReason, _skip_action_.production_kind.g__skip_action__1, p__skip_action__ref);
			return p__skip_action__ref;
		}
		#endregion
	};
}
