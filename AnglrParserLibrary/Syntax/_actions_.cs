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
	// class associated with syntax rule <actions>
	//

	public class	_actions_ : SyntaxTreeBase
	{
		#region enumerated production(s) of syntax rule <actions>
		public enum production_kind : ushort	// enumerated production(s) of syntax rule <actions>
		{
			g__actions__1 = 1,	// <action>

			g__actions__2 = 2,	// <actions> <action>

		};
		#endregion
		#region production markers associated with the syntax rule <actions>

		// markers associated with production: <actions> -> <action>

		public enum production_marker_1 : ushort
		{
			m__action_,
			final
		};

		// markers associated with production: <actions> -> <actions> <action>

		public enum production_marker_2 : ushort
		{
			m__actions_,
			m__action_,
			final
		};
		#endregion
		#region Constructors and destructors associated with the syntax rule <actions>

		// Constructor declaration(s) associated with production(s) of syntax rule <actions>

		//
		// Constructor associated with the following production(s)
		// <actions> -> <action>

		//

		public _actions_ (_action_ p__action_) : base ((uint) ProductionID.__actions__ID, (uint) production_kind.g__actions__1)
		{
			++g_counter;
			_init ();
			children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 1);
			children[0] = m__action_ = p__action_;
		}

		//
		// Constructor associated with the following production(s)
		// <actions> -> <actions> <action>

		//

		public _actions_ (_actions_ p__actions_, _action_ p__action_) : base ((uint) ProductionID.__actions__ID, (uint) production_kind.g__actions__2)
		{
			++g_counter;
			_init ();
			children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 2);
			children[0] = m__actions_ = p__actions_;
			children[1] = m__action_ = p__action_;
		}

		// Copy constructor

		public _actions_ (_actions_ p__actions_) : base (p__actions_.id, p__actions_.kind)
		{
			++g_counter;
			_init ();
			switch ((production_kind) p__actions_.kind)
			{
				case production_kind.g__actions__1:
					children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 1);
					if ((children [0] = m__action_ = (p__actions_.m__action_ != null) ? new _action_ (p__actions_.m__action_) : null) != null) m__action_.parent = this;
					break;
				case production_kind.g__actions__2:
					children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 2);
					if ((children [0] = m__actions_ = (p__actions_.m__actions_ != null) ? new _actions_ (p__actions_.m__actions_) : null) != null) m__actions_.parent = this;
					if ((children [1] = m__action_ = (p__actions_.m__action_ != null) ? new _action_ (p__actions_.m__action_) : null) != null) m__action_.parent = this;
					break;
				default:
				{
					string[] args = new string[] { "_actions_" };
					throw new SyntaxTreeError (SyntaxTreeError.InvalidKindError, args);
				}
			}
		}

		// Clone node - wrapper of copy constructor

		public override SyntaxTreeBase Clone () { return (SyntaxTreeBase) new _actions_ (this); }

		// Destructor

		public new void Dispose ()
		{
			Iterate (null, (node, appData) =>
			{
				--g_counter;

				node.m__action_?.Dispose ();

				node._init ();

				return null;
			});
		}
		#endregion
		#region Content changing and inclusion checking methods associated with the syntax rule <actions>

		// Content changing function(s) associated with production(s) of syntax rule <actions>

		//
		// Content changing function associated with following production(s)
		// <actions> -> <action>

		//

		public void change(_action_ p__action_)
		{
			_init ();
			this.kind = (uint) production_kind.g__actions__1;
			children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 1);
			children [0] = m__action_ = p__action_;
			reparent (parent);
		}

		//
		// Content changing function associated with following production(s)
		// <actions> -> <actions> <action>

		//

		public void change(_actions_ p__actions_, _action_ p__action_)
		{
			_init ();
			this.kind = (uint) production_kind.g__actions__2;
			children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 2);
			children [0] = m__actions_ = p__actions_;
			children [1] = m__action_ = p__action_;
			reparent (parent);
		}

		//
		// check if node 'element' is included within syntax tree associated with syntax rule <actions>
		//

		public new bool checkInclusion (SyntaxTreeBase element)
		{
			return (element == this) || (bool) Iterate (false, (node, appData) =>
			{
				return
					(bool) appData ||
					(node.m__action_ != null) && node.m__action_.checkInclusion (element) ||
					false;
			});
		}
		#endregion
		#region Syntax tree emit methods associated with the syntax rule <actions>

		//
		// emit production tree node associated with any production of syntax rule <actions>
		//

		public override string Emit (int depth)
		{
			string s = "";
			if (depth != 0)
				Iterate (null, (node, appData) =>
				{
					switch ((production_kind) node.kind)
					{
					case production_kind.g__actions__1:
						// emit syntax tree node associated with production
						// <actions>: <action>

						s += " " + node.m__action_.Emit (depth - 1);
						break;
					case production_kind.g__actions__2:
						// emit syntax tree node associated with production
						// <actions>: <actions> <action>

						s += " " + node.m__action_.Emit (depth - 1);
						break;
					}
					return null;
				});
			return s.Trim ();
		}

		//
		// emit production tree node associated with any production of syntax rule <actions>
		//

		public override string EmitProductionTree (int depth)
		{
			return (depth != 0) ?
				(string) Iterate ("", (node, appData) =>
				{
					string str = "_actions_ (" + (string) appData;
					switch ((production_kind) node.kind)
					{
					case production_kind.g__actions__1:
						// emit syntax tree node associated with production
						// <actions>: <action>

						str += node.m__action_.EmitProductionTree (depth - 1);
						break;
					case production_kind.g__actions__2:
						// emit syntax tree node associated with production
						// <actions>: <actions> <action>

						str += ' ';
						str += node.m__action_.EmitProductionTree (depth - 1);
						break;
					}
					return str + ")";
				}) : "";
		}
		#endregion
		#region Syntax tree traversal,  reperrent, init and iterator methods associated with the syntax rule <actions>

		//
		// traverse sub-tree rooted in this node using selected syntax tree walker
		//

		public override void InvokeTraverse (SyntaxTreeWalkerCore syntaxTreeWalker) => ((SyntaxTreeWalker) syntaxTreeWalker).Traverse (this);
		public override void InvokeTraverseCommon (SyntaxTreeWalkerCore syntaxTreeWalker) => ((SyntaxTreeWalker) syntaxTreeWalker).TraverseCommon (this);

		// reparent sub-tree

		public override void reparent (SyntaxTreeBase parent)
		{
			for (_actions_ node = this; node != null; node = node.m__actions_)
			{
				node.parent = parent;
				node.m__action_?.reparent (parent);
				parent = node;
			}
		}

		//
		// initialize object associated with syntax rule <actions>
		//

		public void _init ()
		{
			m__action_ = null;
			m__actions_ = null;
		}

		public delegate object IteratorDelegate (_actions_ p__actions_, object appData);

		public object Iterate (object appData, IteratorDelegate iteratorDelegate)
		{
			_actions_ p__actions_;
			for (p__actions_ = this; p__actions_.m__actions_ != null; p__actions_ = p__actions_.m__actions_);
			for (SyntaxTreeBase parent = p__actions_; (parent != null) && (parent is _actions_); parent = parent.parent)
				appData = iteratorDelegate ((_actions_) parent, appData);
			return appData;
		}
		#endregion
		#region Fields and properties associated with the syntax rule <actions>

		// counter of all nodes associated with syntax rule <actions>
		public static int g_counter;

		// objects associated with terminal and non-terminal symbols within production(s) of syntax rule <actions>
		public _action_ m__action_ { get; private set; }
		public _actions_ m__actions_ { get; private set; }
		#endregion

	};

	public partial class SyntaxTreeWalker
	{
		#region delegates and events associated with the syntax rule <actions>

		// delegate function (callback) prototype associated with syntax rule <actions>
		public delegate bool _actions__Callback (SyntaxTreeCallbackReason reason, _actions_.production_kind kind, _actions_ p__actions_);

		// event associated with syntax rule <actions>
		public event _actions__Callback _actions__Event;

		// event trigger associated with syntax rule <actions>
		public bool Raise__actions__Event (SyntaxTreeCallbackReason reason, _actions_.production_kind kind, _actions_ p__actions_)
		{
			bool? status = _actions__Event?.Invoke (reason, kind, p__actions_);
			return (status == null) || status.Value;
		}
		#endregion
		#region syntax tree traversal methods associated with the syntax rule <actions>
		//
		// traverse syntax tree node associated with any production of syntax rule <actions>
		//

		public void Traverse (_actions_ p__actions_)
		{
			if (p__actions_.isLocked())
				return;
			if (Raise__actions__Event (SyntaxTreeCallbackReason.IterationPrologueCallbackReason, (_actions_.production_kind) p__actions_.kind, p__actions_))
				p__actions_.Iterate (null, (node, appData) =>
				{
					if (node.isLocked())
						return null;
					node.dolock();
					_actions_.production_kind kind = (_actions_.production_kind) node.kind;
					node.turn_reset ();
					if (Raise__actions__Event (SyntaxTreeCallbackReason.TraversalPrologueCallbackReason, kind, node))
						switch (kind)
						{
							case _actions_.production_kind.g__actions__1:
								// traverse syntax tree node associated with production
								// <actions>: <action>

								if (Raise__actions__Event (SyntaxTreeCallbackReason.TraversalMidTermCallbackReason, kind, node))
									Traverse (node.m__action_);
								node.turn_inc ();
								Raise__actions__Event (SyntaxTreeCallbackReason.TraversalMidTermCallbackReason, kind, node);
							break;
							case _actions_.production_kind.g__actions__2:
								// traverse syntax tree node associated with production
								// <actions>: <actions> <action>

								if (Raise__actions__Event (SyntaxTreeCallbackReason.TraversalMidTermCallbackReason, kind, node))
									;
								node.turn_inc ();
								if (Raise__actions__Event (SyntaxTreeCallbackReason.TraversalMidTermCallbackReason, kind, node))
									Traverse (node.m__action_);
								node.turn_inc ();
								Raise__actions__Event (SyntaxTreeCallbackReason.TraversalMidTermCallbackReason, kind, node);
							break;
						}
					Raise__actions__Event (SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason, kind, node);
					node.unlock();
					return null;
				});
			Raise__actions__Event (SyntaxTreeCallbackReason.IterationEpilogueCallbackReason, (_actions_.production_kind) p__actions_.kind, p__actions_);
		}

		//
		// traverse syntax tree node associated with any production of syntax rule <actions>
		//

		public void TraverseCommon (_actions_ p__actions_)
		{
			if (Raise_Common_Event (SyntaxTreeCallbackReason.IterationPrologueCallbackReason, (int) p__actions_.kind, p__actions_))
				p__actions_.Iterate (null, (node, appData) =>
				{
					_actions_.production_kind kind = (_actions_.production_kind) node.kind;
					if (Raise_Common_Event (SyntaxTreeCallbackReason.TraversalPrologueCallbackReason, (int) kind, node))
						switch (kind)
						{
						case _actions_.production_kind.g__actions__1:
							// traverse syntax tree node associated with production
							// <actions>: <action>

							TraverseCommon (node.m__action_);
							break;
						case _actions_.production_kind.g__actions__2:
							// traverse syntax tree node associated with production
							// <actions>: <actions> <action>

							TraverseCommon (node.m__action_);
							break;
						}
					Raise_Common_Event (SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason, (int) kind, node);
					return null;
				});
			Raise_Common_Event (SyntaxTreeCallbackReason.IterationEpilogueCallbackReason, (int) p__actions_.kind, p__actions_);
		}
	};
		#endregion

	public partial class SyntaxTreeBuilder : SyntaxTreeWalker
	{
		#region syntax tree builder methods associated with the syntax rule <actions>

		//
		// create syntax tree node associated with production
		// <actions>: <action>

		//

		public _actions_ _actions__1 (_action_ p__action_)
		{
			_actions_ p__actions__ref = new _actions_(p__action_);
			p__action_.parent = p__actions__ref;
			Raise__actions__Event (SyntaxTreeCallbackReason.BuilderCallbackReason, _actions_.production_kind.g__actions__1, p__actions__ref);
			return p__actions__ref;
		}

		//
		// create syntax tree node associated with production
		// <actions>: <actions> <action>

		//

		public _actions_ _actions__2 (_actions_ p__actions_, _action_ p__action_)
		{
			_actions_ p__actions__ref = new _actions_(p__actions_, p__action_);
			p__actions_.parent = p__actions__ref;
			p__action_.parent = p__actions__ref;
			Raise__actions__Event (SyntaxTreeCallbackReason.BuilderCallbackReason, _actions_.production_kind.g__actions__2, p__actions__ref);
			return p__actions__ref;
		}
		#endregion
	};
}
