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
	// class associated with syntax rule <actions optional>
	//

	public class	_actions_optional_ : SyntaxTreeBase
	{
		#region enumerated production(s) of syntax rule <actions optional>
		public enum production_kind : ushort	// enumerated production(s) of syntax rule <actions optional>
		{
			g__actions_optional__1 = 1,	// %empty

			g__actions_optional__2 = 2,	// <actions>

		};
		#endregion
		#region production markers associated with the syntax rule <actions optional>

		// markers associated with production: <actions optional> -> %empty

		public enum production_marker_1 : ushort
		{
			final
		};

		// markers associated with production: <actions optional> -> <actions>

		public enum production_marker_2 : ushort
		{
			m__actions_,
			final
		};
		#endregion
		#region Constructors and destructors associated with the syntax rule <actions optional>

		// Constructor declaration(s) associated with production(s) of syntax rule <actions optional>

		//
		// Constructor associated with the following production(s)
		// <actions optional> -> %empty

		//

		public _actions_optional_ () : base ((uint) ProductionID.__actions_optional__ID, (uint) production_kind.g__actions_optional__1)
		{
			++g_counter;
			_init ();
			children = Array.Empty <SyntaxTreeBase> ();
		}

		//
		// Constructor associated with the following production(s)
		// <actions optional> -> <actions>

		//

		public _actions_optional_ (_actions_ p__actions_) : base ((uint) ProductionID.__actions_optional__ID, (uint) production_kind.g__actions_optional__2)
		{
			++g_counter;
			_init ();
			children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 1);
			children[0] = m__actions_ = p__actions_;
		}

		// Copy constructor

		public _actions_optional_ (_actions_optional_ p__actions_optional_) : base (p__actions_optional_.id, p__actions_optional_.kind)
		{
			++g_counter;
			_init ();
			switch ((production_kind) p__actions_optional_.kind)
			{
				case production_kind.g__actions_optional__1:
					children = Array.Empty <SyntaxTreeBase> ();
					break;
				case production_kind.g__actions_optional__2:
					children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 1);
					if ((children [0] = m__actions_ = (p__actions_optional_.m__actions_ != null) ? new _actions_ (p__actions_optional_.m__actions_) : null) != null) m__actions_.parent = this;
					break;
				default:
				{
					string[] args = new string[] { "_actions_optional_" };
					throw new SyntaxTreeError (SyntaxTreeError.InvalidKindError, args);
				}
			}
		}

		// Clone node - wrapper of copy constructor

		public override SyntaxTreeBase Clone () { return (SyntaxTreeBase) new _actions_optional_ (this); }

		// Destructor

		public new void Dispose ()
		{
			base.Dispose ();
			--g_counter;

			m__actions_?.Dispose ();

			_init ();
		}
		#endregion
		#region Content changing and inclusion checking methods associated with the syntax rule <actions optional>

		// Content changing function(s) associated with production(s) of syntax rule <actions optional>

		//
		// Content changing function associated with following production(s)
		// <actions optional> -> %empty

		//

		public void change()
		{
			_init ();
			this.kind = (uint) production_kind.g__actions_optional__1;
			children = Array.Empty <SyntaxTreeBase> ();
			reparent (parent);
		}

		//
		// Content changing function associated with following production(s)
		// <actions optional> -> <actions>

		//

		public void change(_actions_ p__actions_)
		{
			_init ();
			this.kind = (uint) production_kind.g__actions_optional__2;
			children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 1);
			children [0] = m__actions_ = p__actions_;
			reparent (parent);
		}

		//
		// check if node 'element' is included within syntax tree associated with syntax rule <actions optional>
		//

		public new bool checkInclusion (SyntaxTreeBase element)
		{
			return
				(element == this) ||
				(m__actions_ != null) && m__actions_.checkInclusion (element) ||
				false;
		}
		#endregion
		#region Syntax tree emit methods associated with the syntax rule <actions optional>

		//
		// emit production tree node associated with any production of syntax rule <actions optional>
		//

		public override string Emit (int depth)
		{
			string s = "";
			if (depth != 0)
			switch ((production_kind) kind)
			{
				case production_kind.g__actions_optional__1:
					// emit syntax tree node associated with production
					// <actions optional>: %empty

				break;
				case production_kind.g__actions_optional__2:
					// emit syntax tree node associated with production
					// <actions optional>: <actions>

					s += m__actions_.Emit (depth - 1);
				break;
			}
			return s.Trim ();
		}

		//
		// emit production tree node associated with any production of syntax rule <actions optional>
		//

		public override string EmitProductionTree (int depth)
		{
			string s = "_actions_optional_ (";
			if (depth != 0)
				switch ((production_kind) kind)
				{
				case production_kind.g__actions_optional__1:
					// emit syntax tree node associated with production
					// <actions optional>: %empty

				break;
				case production_kind.g__actions_optional__2:
					// emit syntax tree node associated with production
					// <actions optional>: <actions>

					s += m__actions_.EmitProductionTree (depth - 1);
				break;
			}
			s += ")";
			return s;
		}
		#endregion
		#region Syntax tree traversal,  reperrent, init and iterator methods associated with the syntax rule <actions optional>

		//
		// traverse sub-tree rooted in this node using selected syntax tree walker
		//

		public override void InvokeTraverse (SyntaxTreeWalkerCore syntaxTreeWalker) => ((SyntaxTreeWalker) syntaxTreeWalker).Traverse (this);
		public override void InvokeTraverseCommon (SyntaxTreeWalkerCore syntaxTreeWalker) => ((SyntaxTreeWalker) syntaxTreeWalker).TraverseCommon (this);

		// reparent sub-tree

		public override void reparent (SyntaxTreeBase parent)
		{
			this.parent = parent;

			m__actions_?.reparent (this);
		}

		//
		// initialize object associated with syntax rule <actions optional>
		//

		public void _init ()
		{
			m__actions_ = null;
		}
		#endregion
		#region Fields and properties associated with the syntax rule <actions optional>

		// counter of all nodes associated with syntax rule <actions optional>
		public static int g_counter;

		// objects associated with terminal and non-terminal symbols within production(s) of syntax rule <actions optional>
		public _actions_ m__actions_ { get; private set; }
		#endregion

	};

	public partial class SyntaxTreeWalker
	{
		#region delegates and events associated with the syntax rule <actions optional>

		// delegate function (callback) prototype associated with syntax rule <actions optional>
		public delegate bool _actions_optional__Callback (SyntaxTreeCallbackReason reason, _actions_optional_.production_kind kind, _actions_optional_ p__actions_optional_);

		// event associated with syntax rule <actions optional>
		public event _actions_optional__Callback _actions_optional__Event;

		// event trigger associated with syntax rule <actions optional>
		public bool Raise__actions_optional__Event (SyntaxTreeCallbackReason reason, _actions_optional_.production_kind kind, _actions_optional_ p__actions_optional_)
		{
			bool? status = _actions_optional__Event?.Invoke (reason, kind, p__actions_optional_);
			return (status == null) || status.Value;
		}
		#endregion
		#region syntax tree traversal methods associated with the syntax rule <actions optional>
		//
		// traverse syntax tree node associated with any production of syntax rule <actions optional>
		//

		public void Traverse (_actions_optional_ p__actions_optional_)
		{
			if (p__actions_optional_.isLocked())
				return;
			p__actions_optional_.dolock();
			_actions_optional_.production_kind kind = (_actions_optional_.production_kind) p__actions_optional_.kind;
			p__actions_optional_.turn_reset ();
			if (Raise__actions_optional__Event (SyntaxTreeCallbackReason.TraversalPrologueCallbackReason, kind, p__actions_optional_))
			switch (kind)
			{
				case _actions_optional_.production_kind.g__actions_optional__1:
					// traverse syntax tree node associated with production
					// <actions optional>: %empty

				break;
				case _actions_optional_.production_kind.g__actions_optional__2:
					// traverse syntax tree node associated with production
					// <actions optional>: <actions>

					if (Raise__actions_optional__Event (SyntaxTreeCallbackReason.TraversalMidTermCallbackReason, kind, p__actions_optional_))
						Traverse (p__actions_optional_.m__actions_);
					p__actions_optional_.turn_inc ();
					Raise__actions_optional__Event (SyntaxTreeCallbackReason.TraversalMidTermCallbackReason, kind, p__actions_optional_);
				break;
			}
			Raise__actions_optional__Event (SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason, kind, p__actions_optional_);
			p__actions_optional_.unlock();
		}

		//
		// traverse syntax tree node associated with any production of syntax rule <actions optional>
		//

		public void TraverseCommon (_actions_optional_ p__actions_optional_)
		{
			_actions_optional_.production_kind kind = (_actions_optional_.production_kind) p__actions_optional_.kind;
			if (Raise_Common_Event (SyntaxTreeCallbackReason.TraversalPrologueCallbackReason, (int) kind, p__actions_optional_))
			switch (kind)
			{
				case _actions_optional_.production_kind.g__actions_optional__1:
					// traverse syntax tree node associated with production
					// <actions optional>: %empty

				break;
				case _actions_optional_.production_kind.g__actions_optional__2:
					// traverse syntax tree node associated with production
					// <actions optional>: <actions>

						TraverseCommon (p__actions_optional_.m__actions_);
				break;
			}
			Raise_Common_Event (SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason, (int) kind, p__actions_optional_);
		}
	};
		#endregion

	public partial class SyntaxTreeBuilder : SyntaxTreeWalker
	{
		#region syntax tree builder methods associated with the syntax rule <actions optional>

		//
		// create syntax tree node associated with production
		// <actions optional>: %empty

		//

		public _actions_optional_ _actions_optional__1 ()
		{
			_actions_optional_ p__actions_optional__ref = new _actions_optional_();
			Raise__actions_optional__Event (SyntaxTreeCallbackReason.BuilderCallbackReason, _actions_optional_.production_kind.g__actions_optional__1, p__actions_optional__ref);
			return p__actions_optional__ref;
		}

		//
		// create syntax tree node associated with production
		// <actions optional>: <actions>

		//

		public _actions_optional_ _actions_optional__2 (_actions_ p__actions_)
		{
			_actions_optional_ p__actions_optional__ref = new _actions_optional_(p__actions_);
			p__actions_.parent = p__actions_optional__ref;
			Raise__actions_optional__Event (SyntaxTreeCallbackReason.BuilderCallbackReason, _actions_optional_.production_kind.g__actions_optional__2, p__actions_optional__ref);
			return p__actions_optional__ref;
		}
		#endregion
	};
}
