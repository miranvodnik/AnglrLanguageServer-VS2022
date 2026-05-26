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
	// class associated with syntax rule <action>
	//

	public class	_action_ : SyntaxTreeBase
	{
		#region enumerated production(s) of syntax rule <action>
		public enum production_kind : ushort	// enumerated production(s) of syntax rule <action>
		{
			g__action__1 = 1,	// <skip action>

			g__action__2 = 2,	// <terminal action>

			g__action__3 = 3,	// <event action>

			g__action__4 = 4,	// <push action>

			g__action__5 = 5,	// <pop action>

		};
		#endregion
		#region production markers associated with the syntax rule <action>

		// markers associated with production: <action> -> <skip action>

		public enum production_marker_1 : ushort
		{
			m__skip_action_,
			final
		};

		// markers associated with production: <action> -> <terminal action>

		public enum production_marker_2 : ushort
		{
			m__terminal_action_,
			final
		};

		// markers associated with production: <action> -> <event action>

		public enum production_marker_3 : ushort
		{
			m__event_action_,
			final
		};

		// markers associated with production: <action> -> <push action>

		public enum production_marker_4 : ushort
		{
			m__push_action_,
			final
		};

		// markers associated with production: <action> -> <pop action>

		public enum production_marker_5 : ushort
		{
			m__pop_action_,
			final
		};
		#endregion
		#region Constructors and destructors associated with the syntax rule <action>

		// Constructor declaration(s) associated with production(s) of syntax rule <action>

		//
		// Constructor associated with the following production(s)
		// <action> -> <skip action>

		//

		public _action_ (_skip_action_ p__skip_action_) : base ((uint) ProductionID.__action__ID, (uint) production_kind.g__action__1)
		{
			++g_counter;
			_init ();
			children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 1);
			children[0] = m__skip_action_ = p__skip_action_;
		}

		//
		// Constructor associated with the following production(s)
		// <action> -> <terminal action>

		//

		public _action_ (_terminal_action_ p__terminal_action_) : base ((uint) ProductionID.__action__ID, (uint) production_kind.g__action__2)
		{
			++g_counter;
			_init ();
			children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 1);
			children[0] = m__terminal_action_ = p__terminal_action_;
		}

		//
		// Constructor associated with the following production(s)
		// <action> -> <event action>

		//

		public _action_ (_event_action_ p__event_action_) : base ((uint) ProductionID.__action__ID, (uint) production_kind.g__action__3)
		{
			++g_counter;
			_init ();
			children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 1);
			children[0] = m__event_action_ = p__event_action_;
		}

		//
		// Constructor associated with the following production(s)
		// <action> -> <push action>

		//

		public _action_ (_push_action_ p__push_action_) : base ((uint) ProductionID.__action__ID, (uint) production_kind.g__action__4)
		{
			++g_counter;
			_init ();
			children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 1);
			children[0] = m__push_action_ = p__push_action_;
		}

		//
		// Constructor associated with the following production(s)
		// <action> -> <pop action>

		//

		public _action_ (_pop_action_ p__pop_action_) : base ((uint) ProductionID.__action__ID, (uint) production_kind.g__action__5)
		{
			++g_counter;
			_init ();
			children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 1);
			children[0] = m__pop_action_ = p__pop_action_;
		}

		// Copy constructor

		public _action_ (_action_ p__action_) : base (p__action_.id, p__action_.kind)
		{
			++g_counter;
			_init ();
			switch ((production_kind) p__action_.kind)
			{
				case production_kind.g__action__1:
					children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 1);
					if ((children [0] = m__skip_action_ = (p__action_.m__skip_action_ != null) ? new _skip_action_ (p__action_.m__skip_action_) : null) != null) m__skip_action_.parent = this;
					break;
				case production_kind.g__action__2:
					children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 1);
					if ((children [0] = m__terminal_action_ = (p__action_.m__terminal_action_ != null) ? new _terminal_action_ (p__action_.m__terminal_action_) : null) != null) m__terminal_action_.parent = this;
					break;
				case production_kind.g__action__3:
					children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 1);
					if ((children [0] = m__event_action_ = (p__action_.m__event_action_ != null) ? new _event_action_ (p__action_.m__event_action_) : null) != null) m__event_action_.parent = this;
					break;
				case production_kind.g__action__4:
					children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 1);
					if ((children [0] = m__push_action_ = (p__action_.m__push_action_ != null) ? new _push_action_ (p__action_.m__push_action_) : null) != null) m__push_action_.parent = this;
					break;
				case production_kind.g__action__5:
					children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 1);
					if ((children [0] = m__pop_action_ = (p__action_.m__pop_action_ != null) ? new _pop_action_ (p__action_.m__pop_action_) : null) != null) m__pop_action_.parent = this;
					break;
				default:
				{
					string[] args = new string[] { "_action_" };
					throw new SyntaxTreeError (SyntaxTreeError.InvalidKindError, args);
				}
			}
		}

		// Clone node - wrapper of copy constructor

		public override SyntaxTreeBase Clone () { return (SyntaxTreeBase) new _action_ (this); }

		// Destructor

		public new void Dispose ()
		{
			base.Dispose ();
			--g_counter;

			m__skip_action_?.Dispose ();
			m__terminal_action_?.Dispose ();
			m__event_action_?.Dispose ();
			m__push_action_?.Dispose ();
			m__pop_action_?.Dispose ();

			_init ();
		}
		#endregion
		#region Content changing and inclusion checking methods associated with the syntax rule <action>

		// Content changing function(s) associated with production(s) of syntax rule <action>

		//
		// Content changing function associated with following production(s)
		// <action> -> <skip action>

		//

		public void change(_skip_action_ p__skip_action_)
		{
			_init ();
			this.kind = (uint) production_kind.g__action__1;
			children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 1);
			children [0] = m__skip_action_ = p__skip_action_;
			reparent (parent);
		}

		//
		// Content changing function associated with following production(s)
		// <action> -> <terminal action>

		//

		public void change(_terminal_action_ p__terminal_action_)
		{
			_init ();
			this.kind = (uint) production_kind.g__action__2;
			children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 1);
			children [0] = m__terminal_action_ = p__terminal_action_;
			reparent (parent);
		}

		//
		// Content changing function associated with following production(s)
		// <action> -> <event action>

		//

		public void change(_event_action_ p__event_action_)
		{
			_init ();
			this.kind = (uint) production_kind.g__action__3;
			children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 1);
			children [0] = m__event_action_ = p__event_action_;
			reparent (parent);
		}

		//
		// Content changing function associated with following production(s)
		// <action> -> <push action>

		//

		public void change(_push_action_ p__push_action_)
		{
			_init ();
			this.kind = (uint) production_kind.g__action__4;
			children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 1);
			children [0] = m__push_action_ = p__push_action_;
			reparent (parent);
		}

		//
		// Content changing function associated with following production(s)
		// <action> -> <pop action>

		//

		public void change(_pop_action_ p__pop_action_)
		{
			_init ();
			this.kind = (uint) production_kind.g__action__5;
			children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 1);
			children [0] = m__pop_action_ = p__pop_action_;
			reparent (parent);
		}

		//
		// check if node 'element' is included within syntax tree associated with syntax rule <action>
		//

		public new bool checkInclusion (SyntaxTreeBase element)
		{
			return
				(element == this) ||
				(m__skip_action_ != null) && m__skip_action_.checkInclusion (element) ||
				(m__terminal_action_ != null) && m__terminal_action_.checkInclusion (element) ||
				(m__event_action_ != null) && m__event_action_.checkInclusion (element) ||
				(m__push_action_ != null) && m__push_action_.checkInclusion (element) ||
				(m__pop_action_ != null) && m__pop_action_.checkInclusion (element) ||
				false;
		}
		#endregion
		#region Syntax tree emit methods associated with the syntax rule <action>

		//
		// emit production tree node associated with any production of syntax rule <action>
		//

		public override string Emit (int depth)
		{
			string s = "";
			if (depth != 0)
			switch ((production_kind) kind)
			{
				case production_kind.g__action__1:
					// emit syntax tree node associated with production
					// <action>: <skip action>

					s += m__skip_action_.Emit (depth - 1);
				break;
				case production_kind.g__action__2:
					// emit syntax tree node associated with production
					// <action>: <terminal action>

					s += m__terminal_action_.Emit (depth - 1);
				break;
				case production_kind.g__action__3:
					// emit syntax tree node associated with production
					// <action>: <event action>

					s += m__event_action_.Emit (depth - 1);
				break;
				case production_kind.g__action__4:
					// emit syntax tree node associated with production
					// <action>: <push action>

					s += m__push_action_.Emit (depth - 1);
				break;
				case production_kind.g__action__5:
					// emit syntax tree node associated with production
					// <action>: <pop action>

					s += m__pop_action_.Emit (depth - 1);
				break;
			}
			return s.Trim ();
		}

		//
		// emit production tree node associated with any production of syntax rule <action>
		//

		public override string EmitProductionTree (int depth)
		{
			string s = "_action_ (";
			if (depth != 0)
				switch ((production_kind) kind)
				{
				case production_kind.g__action__1:
					// emit syntax tree node associated with production
					// <action>: <skip action>

					s += m__skip_action_.EmitProductionTree (depth - 1);
				break;
				case production_kind.g__action__2:
					// emit syntax tree node associated with production
					// <action>: <terminal action>

					s += m__terminal_action_.EmitProductionTree (depth - 1);
				break;
				case production_kind.g__action__3:
					// emit syntax tree node associated with production
					// <action>: <event action>

					s += m__event_action_.EmitProductionTree (depth - 1);
				break;
				case production_kind.g__action__4:
					// emit syntax tree node associated with production
					// <action>: <push action>

					s += m__push_action_.EmitProductionTree (depth - 1);
				break;
				case production_kind.g__action__5:
					// emit syntax tree node associated with production
					// <action>: <pop action>

					s += m__pop_action_.EmitProductionTree (depth - 1);
				break;
			}
			s += ")";
			return s;
		}
		#endregion
		#region Syntax tree traversal,  reperrent, init and iterator methods associated with the syntax rule <action>

		//
		// traverse sub-tree rooted in this node using selected syntax tree walker
		//

		public override void InvokeTraverse (SyntaxTreeWalkerCore syntaxTreeWalker) => ((SyntaxTreeWalker) syntaxTreeWalker).Traverse (this);
		public override void InvokeTraverseCommon (SyntaxTreeWalkerCore syntaxTreeWalker) => ((SyntaxTreeWalker) syntaxTreeWalker).TraverseCommon (this);

		// reparent sub-tree

		public override void reparent (SyntaxTreeBase parent)
		{
			this.parent = parent;

			m__skip_action_?.reparent (this);
			m__terminal_action_?.reparent (this);
			m__event_action_?.reparent (this);
			m__push_action_?.reparent (this);
			m__pop_action_?.reparent (this);
		}

		//
		// initialize object associated with syntax rule <action>
		//

		public void _init ()
		{
			m__skip_action_ = null;
			m__terminal_action_ = null;
			m__event_action_ = null;
			m__push_action_ = null;
			m__pop_action_ = null;
		}
		#endregion
		#region Fields and properties associated with the syntax rule <action>

		// counter of all nodes associated with syntax rule <action>
		public static int g_counter;

		// objects associated with terminal and non-terminal symbols within production(s) of syntax rule <action>
		public _skip_action_ m__skip_action_ { get; private set; }
		public _terminal_action_ m__terminal_action_ { get; private set; }
		public _event_action_ m__event_action_ { get; private set; }
		public _push_action_ m__push_action_ { get; private set; }
		public _pop_action_ m__pop_action_ { get; private set; }
		#endregion

	};

	public partial class SyntaxTreeWalker
	{
		#region delegates and events associated with the syntax rule <action>

		// delegate function (callback) prototype associated with syntax rule <action>
		public delegate bool _action__Callback (SyntaxTreeCallbackReason reason, _action_.production_kind kind, _action_ p__action_);

		// event associated with syntax rule <action>
		public event _action__Callback _action__Event;

		// event trigger associated with syntax rule <action>
		public bool Raise__action__Event (SyntaxTreeCallbackReason reason, _action_.production_kind kind, _action_ p__action_)
		{
			bool? status = _action__Event?.Invoke (reason, kind, p__action_);
			return (status == null) || status.Value;
		}
		#endregion
		#region syntax tree traversal methods associated with the syntax rule <action>
		//
		// traverse syntax tree node associated with any production of syntax rule <action>
		//

		public void Traverse (_action_ p__action_)
		{
			if (p__action_.isLocked())
				return;
			p__action_.dolock();
			_action_.production_kind kind = (_action_.production_kind) p__action_.kind;
			p__action_.turn_reset ();
			if (Raise__action__Event (SyntaxTreeCallbackReason.TraversalPrologueCallbackReason, kind, p__action_))
			switch (kind)
			{
				case _action_.production_kind.g__action__1:
					// traverse syntax tree node associated with production
					// <action>: <skip action>

					if (Raise__action__Event (SyntaxTreeCallbackReason.TraversalMidTermCallbackReason, kind, p__action_))
						Traverse (p__action_.m__skip_action_);
					p__action_.turn_inc ();
					Raise__action__Event (SyntaxTreeCallbackReason.TraversalMidTermCallbackReason, kind, p__action_);
				break;
				case _action_.production_kind.g__action__2:
					// traverse syntax tree node associated with production
					// <action>: <terminal action>

					if (Raise__action__Event (SyntaxTreeCallbackReason.TraversalMidTermCallbackReason, kind, p__action_))
						Traverse (p__action_.m__terminal_action_);
					p__action_.turn_inc ();
					Raise__action__Event (SyntaxTreeCallbackReason.TraversalMidTermCallbackReason, kind, p__action_);
				break;
				case _action_.production_kind.g__action__3:
					// traverse syntax tree node associated with production
					// <action>: <event action>

					if (Raise__action__Event (SyntaxTreeCallbackReason.TraversalMidTermCallbackReason, kind, p__action_))
						Traverse (p__action_.m__event_action_);
					p__action_.turn_inc ();
					Raise__action__Event (SyntaxTreeCallbackReason.TraversalMidTermCallbackReason, kind, p__action_);
				break;
				case _action_.production_kind.g__action__4:
					// traverse syntax tree node associated with production
					// <action>: <push action>

					if (Raise__action__Event (SyntaxTreeCallbackReason.TraversalMidTermCallbackReason, kind, p__action_))
						Traverse (p__action_.m__push_action_);
					p__action_.turn_inc ();
					Raise__action__Event (SyntaxTreeCallbackReason.TraversalMidTermCallbackReason, kind, p__action_);
				break;
				case _action_.production_kind.g__action__5:
					// traverse syntax tree node associated with production
					// <action>: <pop action>

					if (Raise__action__Event (SyntaxTreeCallbackReason.TraversalMidTermCallbackReason, kind, p__action_))
						Traverse (p__action_.m__pop_action_);
					p__action_.turn_inc ();
					Raise__action__Event (SyntaxTreeCallbackReason.TraversalMidTermCallbackReason, kind, p__action_);
				break;
			}
			Raise__action__Event (SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason, kind, p__action_);
			p__action_.unlock();
		}

		//
		// traverse syntax tree node associated with any production of syntax rule <action>
		//

		public void TraverseCommon (_action_ p__action_)
		{
			_action_.production_kind kind = (_action_.production_kind) p__action_.kind;
			if (Raise_Common_Event (SyntaxTreeCallbackReason.TraversalPrologueCallbackReason, (int) kind, p__action_))
			switch (kind)
			{
				case _action_.production_kind.g__action__1:
					// traverse syntax tree node associated with production
					// <action>: <skip action>

						TraverseCommon (p__action_.m__skip_action_);
				break;
				case _action_.production_kind.g__action__2:
					// traverse syntax tree node associated with production
					// <action>: <terminal action>

						TraverseCommon (p__action_.m__terminal_action_);
				break;
				case _action_.production_kind.g__action__3:
					// traverse syntax tree node associated with production
					// <action>: <event action>

						TraverseCommon (p__action_.m__event_action_);
				break;
				case _action_.production_kind.g__action__4:
					// traverse syntax tree node associated with production
					// <action>: <push action>

						TraverseCommon (p__action_.m__push_action_);
				break;
				case _action_.production_kind.g__action__5:
					// traverse syntax tree node associated with production
					// <action>: <pop action>

						TraverseCommon (p__action_.m__pop_action_);
				break;
			}
			Raise_Common_Event (SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason, (int) kind, p__action_);
		}
	};
		#endregion

	public partial class SyntaxTreeBuilder : SyntaxTreeWalker
	{
		#region syntax tree builder methods associated with the syntax rule <action>

		//
		// create syntax tree node associated with production
		// <action>: <skip action>

		//

		public _action_ _action__1 (_skip_action_ p__skip_action_)
		{
			_action_ p__action__ref = new _action_(p__skip_action_);
			p__skip_action_.parent = p__action__ref;
			Raise__action__Event (SyntaxTreeCallbackReason.BuilderCallbackReason, _action_.production_kind.g__action__1, p__action__ref);
			return p__action__ref;
		}

		//
		// create syntax tree node associated with production
		// <action>: <terminal action>

		//

		public _action_ _action__2 (_terminal_action_ p__terminal_action_)
		{
			_action_ p__action__ref = new _action_(p__terminal_action_);
			p__terminal_action_.parent = p__action__ref;
			Raise__action__Event (SyntaxTreeCallbackReason.BuilderCallbackReason, _action_.production_kind.g__action__2, p__action__ref);
			return p__action__ref;
		}

		//
		// create syntax tree node associated with production
		// <action>: <event action>

		//

		public _action_ _action__3 (_event_action_ p__event_action_)
		{
			_action_ p__action__ref = new _action_(p__event_action_);
			p__event_action_.parent = p__action__ref;
			Raise__action__Event (SyntaxTreeCallbackReason.BuilderCallbackReason, _action_.production_kind.g__action__3, p__action__ref);
			return p__action__ref;
		}

		//
		// create syntax tree node associated with production
		// <action>: <push action>

		//

		public _action_ _action__4 (_push_action_ p__push_action_)
		{
			_action_ p__action__ref = new _action_(p__push_action_);
			p__push_action_.parent = p__action__ref;
			Raise__action__Event (SyntaxTreeCallbackReason.BuilderCallbackReason, _action_.production_kind.g__action__4, p__action__ref);
			return p__action__ref;
		}

		//
		// create syntax tree node associated with production
		// <action>: <pop action>

		//

		public _action_ _action__5 (_pop_action_ p__pop_action_)
		{
			_action_ p__action__ref = new _action_(p__pop_action_);
			p__pop_action_.parent = p__action__ref;
			Raise__action__Event (SyntaxTreeCallbackReason.BuilderCallbackReason, _action_.production_kind.g__action__5, p__action__ref);
			return p__action__ref;
		}
		#endregion
	};
}
