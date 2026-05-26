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
	// class associated with syntax rule <terminal action>
	//

	public class	_terminal_action_ : SyntaxTreeBase
	{
		#region enumerated production(s) of syntax rule <terminal action>
		public enum production_kind : ushort	// enumerated production(s) of syntax rule <terminal action>
		{
			g__terminal_action__1 = 1,	// 'terminal' <identifier>

		};
		#endregion
		#region production markers associated with the syntax rule <terminal action>

		// markers associated with production: <terminal action> -> 'terminal' <identifier>

		public enum production_marker_1 : ushort
		{
			final
		};
		#endregion
		#region Constructors and destructors associated with the syntax rule <terminal action>

		// Constructor declaration(s) associated with production(s) of syntax rule <terminal action>

		//
		// Constructor associated with the following production(s)
		// <terminal action> -> 'terminal' <identifier>

		//

		public _terminal_action_ (SyntaxTreeToken p_token, SyntaxTreeToken p_token_1) : base ((uint) ProductionID.__terminal_action__ID, (uint) production_kind.g__terminal_action__1)
		{
			++g_counter;
			_init ();
			children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 2);
			children[0] = m__ttoken_ = p_token;
			children[1] = m__identifier_ = p_token_1;
		}

		// Copy constructor

		public _terminal_action_ (_terminal_action_ p__terminal_action_) : base (p__terminal_action_.id, p__terminal_action_.kind)
		{
			++g_counter;
			_init ();
			switch ((production_kind) this.kind)
			{
			case production_kind.g__terminal_action__1:
				children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 2);
				children[0] = m__ttoken_ = p__terminal_action_.m__ttoken_;
				children[1] = m__identifier_ = p__terminal_action_.m__identifier_;
				break;
			default:
				string[] args = new string[] { "_terminal_action_" };
				throw new SyntaxTreeError (SyntaxTreeError.InvalidKindError, args);
			}
		}

		// Clone node - wrapper of copy constructor

		public override SyntaxTreeBase Clone () { return (SyntaxTreeBase) new _terminal_action_ (this); }

		// Destructor

		public new void Dispose ()
		{
			base.Dispose ();
			--g_counter;

			m__ttoken_?.Dispose ();
			m__identifier_?.Dispose ();

			_init ();
		}
		#endregion
		#region Content changing and inclusion checking methods associated with the syntax rule <terminal action>

		// Content changing function(s) associated with production(s) of syntax rule <terminal action>

		//
		// Content changing function associated with following production(s)
		// <terminal action> -> 'terminal' <identifier>

		//

		public void change(SyntaxTreeToken p_token, SyntaxTreeToken p_token_1)
		{
			_init ();
			this.kind = (uint) production_kind.g__terminal_action__1;
			children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 2);
			children [0] = m__ttoken_ = p_token;
			children [1] = m__identifier_ = p_token_1;
			reparent (parent);
		}

		//
		// check if node 'element' is included within syntax tree associated with syntax rule <terminal action>
		//

		public new bool checkInclusion (SyntaxTreeBase element)
		{
			return
				(element == this) ||
				(m__ttoken_ != null) && m__ttoken_.checkInclusion (element) ||
				(m__identifier_ != null) && m__identifier_.checkInclusion (element) ||
				false;
		}
		#endregion
		#region Syntax tree emit methods associated with the syntax rule <terminal action>

		//
		// emit production tree node associated with any production of syntax rule <terminal action>
		//

		public override string Emit (int depth)
		{
			string s = "";
			if (depth != 0)
			switch ((production_kind) kind)
			{
				case production_kind.g__terminal_action__1:
					// emit syntax tree node associated with production
					// <terminal action>: 'terminal' <identifier>

					s += m__ttoken_.Emit (depth - 1);
					s += " ";
					s += m__identifier_.Emit (depth - 1);
				break;
			}
			return s.Trim ();
		}

		//
		// emit production tree node associated with any production of syntax rule <terminal action>
		//

		public override string EmitProductionTree (int depth)
		{
			string s = "_terminal_action_ (";
			if (depth != 0)
				switch ((production_kind) kind)
				{
				case production_kind.g__terminal_action__1:
					// emit syntax tree node associated with production
					// <terminal action>: 'terminal' <identifier>

					s += "_ttoken_";
					s += ' ';
					s += "_identifier_";
				break;
			}
			s += ")";
			return s;
		}
		#endregion
		#region Syntax tree traversal,  reperrent, init and iterator methods associated with the syntax rule <terminal action>

		//
		// traverse sub-tree rooted in this node using selected syntax tree walker
		//

		public override void InvokeTraverse (SyntaxTreeWalkerCore syntaxTreeWalker) => ((SyntaxTreeWalker) syntaxTreeWalker).Traverse (this);
		public override void InvokeTraverseCommon (SyntaxTreeWalkerCore syntaxTreeWalker) => ((SyntaxTreeWalker) syntaxTreeWalker).TraverseCommon (this);

		// reparent sub-tree

		public override void reparent (SyntaxTreeBase parent)
		{
			this.parent = parent;

			m__ttoken_?.reparent (this);
			m__identifier_?.reparent (this);
		}

		//
		// initialize object associated with syntax rule <terminal action>
		//

		public void _init ()
		{
			m__ttoken_ = null;
			m__identifier_ = null;
		}
		#endregion
		#region Fields and properties associated with the syntax rule <terminal action>

		// counter of all nodes associated with syntax rule <terminal action>
		public static int g_counter;

		// objects associated with terminal and non-terminal symbols within production(s) of syntax rule <terminal action>
		public SyntaxTreeToken m__ttoken_ { get; private set; }
		public SyntaxTreeToken m__identifier_ { get; private set; }
		#endregion

	};

	public partial class SyntaxTreeWalker
	{
		#region delegates and events associated with the syntax rule <terminal action>

		// delegate function (callback) prototype associated with syntax rule <terminal action>
		public delegate bool _terminal_action__Callback (SyntaxTreeCallbackReason reason, _terminal_action_.production_kind kind, _terminal_action_ p__terminal_action_);

		// event associated with syntax rule <terminal action>
		public event _terminal_action__Callback _terminal_action__Event;

		// event trigger associated with syntax rule <terminal action>
		public bool Raise__terminal_action__Event (SyntaxTreeCallbackReason reason, _terminal_action_.production_kind kind, _terminal_action_ p__terminal_action_)
		{
			bool? status = _terminal_action__Event?.Invoke (reason, kind, p__terminal_action_);
			return (status == null) || status.Value;
		}
		#endregion
		#region syntax tree traversal methods associated with the syntax rule <terminal action>
		//
		// traverse syntax tree node associated with any production of syntax rule <terminal action>
		//

		public void Traverse (_terminal_action_ p__terminal_action_)
		{
			if (p__terminal_action_.isLocked())
				return;
			p__terminal_action_.dolock();
			_terminal_action_.production_kind kind = (_terminal_action_.production_kind) p__terminal_action_.kind;
			p__terminal_action_.turn_reset ();
			if (Raise__terminal_action__Event (SyntaxTreeCallbackReason.TraversalPrologueCallbackReason, kind, p__terminal_action_))
			switch (kind)
			{
				case _terminal_action_.production_kind.g__terminal_action__1:
					// traverse syntax tree node associated with production
					// <terminal action>: 'terminal' <identifier>

				break;
			}
			Raise__terminal_action__Event (SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason, kind, p__terminal_action_);
			p__terminal_action_.unlock();
		}

		//
		// traverse syntax tree node associated with any production of syntax rule <terminal action>
		//

		public void TraverseCommon (_terminal_action_ p__terminal_action_)
		{
			_terminal_action_.production_kind kind = (_terminal_action_.production_kind) p__terminal_action_.kind;
			if (Raise_Common_Event (SyntaxTreeCallbackReason.TraversalPrologueCallbackReason, (int) kind, p__terminal_action_))
			switch (kind)
			{
				case _terminal_action_.production_kind.g__terminal_action__1:
					// traverse syntax tree node associated with production
					// <terminal action>: 'terminal' <identifier>

						TraverseCommon (p__terminal_action_.m__ttoken_);
						TraverseCommon (p__terminal_action_.m__identifier_);
				break;
			}
			Raise_Common_Event (SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason, (int) kind, p__terminal_action_);
		}
	};
		#endregion

	public partial class SyntaxTreeBuilder : SyntaxTreeWalker
	{
		#region syntax tree builder methods associated with the syntax rule <terminal action>

		//
		// create syntax tree node associated with production
		// <terminal action>: 'terminal' <identifier>

		//

		public _terminal_action_ _terminal_action__1 (SyntaxTreeToken p_token, SyntaxTreeToken p_token_1)
		{
			_terminal_action_ p__terminal_action__ref = new _terminal_action_(p_token, p_token_1);
			p_token.parent = p__terminal_action__ref;
			p_token_1.parent = p__terminal_action__ref;
			Raise__terminal_action__Event (SyntaxTreeCallbackReason.BuilderCallbackReason, _terminal_action_.production_kind.g__terminal_action__1, p__terminal_action__ref);
			return p__terminal_action__ref;
		}
		#endregion
	};
}
