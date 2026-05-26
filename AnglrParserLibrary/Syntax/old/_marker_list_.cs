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
	// class associated with syntax rule <marker list>
	//

	public class	_marker_list_ : SyntaxTreeBase
	{
		#region enumerated production(s) of syntax rule <marker list>
		public enum production_kind : ushort	// enumerated production(s) of syntax rule <marker list>
		{
			g__marker_list__1 = 1,	// <marker>

			g__marker_list__2 = 2,	// <marker list> <marker>

		};
		#endregion
		#region production markers associated with the syntax rule <marker list>

		// markers associated with production: <marker list> -> <marker>

		public enum production_marker_1 : ushort
		{
			m__marker_,
			final
		};

		// markers associated with production: <marker list> -> <marker list> <marker>

		public enum production_marker_2 : ushort
		{
			m__marker_list_,
			m__marker_,
			final
		};
		#endregion
		#region Constructors and destructors associated with the syntax rule <marker list>

		// Constructor declaration(s) associated with production(s) of syntax rule <marker list>

		//
		// Constructor associated with the following production(s)
		// <marker list> -> <marker>

		//

		public _marker_list_ (_marker_ p__marker_) : base ((uint) ProductionID.__marker_list__ID, (uint) production_kind.g__marker_list__1)
		{
			++g_counter;
			_init ();
			children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 1);
			children[0] = m__marker_ = p__marker_;
		}

		//
		// Constructor associated with the following production(s)
		// <marker list> -> <marker list> <marker>

		//

		public _marker_list_ (_marker_list_ p__marker_list_, _marker_ p__marker_) : base ((uint) ProductionID.__marker_list__ID, (uint) production_kind.g__marker_list__2)
		{
			++g_counter;
			_init ();
			children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 2);
			children[0] = m__marker_list_ = p__marker_list_;
			children[1] = m__marker_ = p__marker_;
		}

		// Copy constructor

		public _marker_list_ (_marker_list_ p__marker_list_) : base (p__marker_list_.id, p__marker_list_.kind)
		{
			++g_counter;
			_init ();
			switch ((production_kind) this.kind)
			{
			case production_kind.g__marker_list__1:
				children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 1);
				children[0] = m__marker_ = p__marker_list_.m__marker_;
				break;
			case production_kind.g__marker_list__2:
				children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 2);
				children[0] = m__marker_list_ = p__marker_list_.m__marker_list_;
				children[1] = m__marker_ = p__marker_list_.m__marker_;
				break;
			default:
				string[] args = new string[] { "_marker_list_" };
				throw new SyntaxTreeError (SyntaxTreeError.InvalidKindError, args);
			}
		}

		// Clone node - wrapper of copy constructor

		public override SyntaxTreeBase Clone () { return (SyntaxTreeBase) new _marker_list_ (this); }

		// Destructor

		public new void Dispose ()
		{
			Iterate (null, (node, appData) =>
			{
				--g_counter;

				node.m__marker_?.Dispose ();

				node._init ();

				return null;
			});
		}
		#endregion
		#region Content changing and inclusion checking methods associated with the syntax rule <marker list>

		// Content changing function(s) associated with production(s) of syntax rule <marker list>

		//
		// Content changing function associated with following production(s)
		// <marker list> -> <marker>

		//

		public void change(_marker_ p__marker_)
		{
			_init ();
			this.kind = (uint) production_kind.g__marker_list__1;
			children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 1);
			children [0] = m__marker_ = p__marker_;
			reparent (parent);
		}

		//
		// Content changing function associated with following production(s)
		// <marker list> -> <marker list> <marker>

		//

		public void change(_marker_list_ p__marker_list_, _marker_ p__marker_)
		{
			_init ();
			this.kind = (uint) production_kind.g__marker_list__2;
			children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 2);
			children [0] = m__marker_list_ = p__marker_list_;
			children [1] = m__marker_ = p__marker_;
			reparent (parent);
		}

		//
		// check if node 'element' is included within syntax tree associated with syntax rule <marker list>
		//

		public new bool checkInclusion (SyntaxTreeBase element)
		{
			return (element == this) || (bool) Iterate (false, (node, appData) =>
			{
				return
					(bool) appData ||
					(node.m__marker_ != null) && node.m__marker_.checkInclusion (element) ||
					false;
			});
		}
		#endregion
		#region Syntax tree emit methods associated with the syntax rule <marker list>

		//
		// emit production tree node associated with any production of syntax rule <marker list>
		//

		public override string Emit (int depth)
		{
			string s = "";
			if (depth != 0)
				Iterate (null, (node, appData) =>
				{
					switch ((production_kind) node.kind)
					{
					case production_kind.g__marker_list__1:
						// emit syntax tree node associated with production
						// <marker list>: <marker>

						s += " " + node.m__marker_.Emit (depth - 1);
						break;
					case production_kind.g__marker_list__2:
						// emit syntax tree node associated with production
						// <marker list>: <marker list> <marker>

						s += " " + node.m__marker_.Emit (depth - 1);
						break;
					}
					return null;
				});
			return s.Trim ();
		}

		//
		// emit production tree node associated with any production of syntax rule <marker list>
		//

		public override string EmitProductionTree (int depth)
		{
			return (depth != 0) ?
				(string) Iterate ("", (node, appData) =>
				{
					string str = "_marker_list_ (" + (string) appData;
					switch ((production_kind) node.kind)
					{
					case production_kind.g__marker_list__1:
						// emit syntax tree node associated with production
						// <marker list>: <marker>

						str += node.m__marker_.EmitProductionTree (depth - 1);
						break;
					case production_kind.g__marker_list__2:
						// emit syntax tree node associated with production
						// <marker list>: <marker list> <marker>

						str += ' ';
						str += node.m__marker_.EmitProductionTree (depth - 1);
						break;
					}
					return str + ")";
				}) : "";
		}
		#endregion
		#region Syntax tree traversal,  reperrent, init and iterator methods associated with the syntax rule <marker list>

		//
		// traverse sub-tree rooted in this node using selected syntax tree walker
		//

		public override void InvokeTraverse (SyntaxTreeWalkerCore syntaxTreeWalker) => ((SyntaxTreeWalker) syntaxTreeWalker).Traverse (this);
		public override void InvokeTraverseCommon (SyntaxTreeWalkerCore syntaxTreeWalker) => ((SyntaxTreeWalker) syntaxTreeWalker).TraverseCommon (this);

		// reparent sub-tree

		public override void reparent (SyntaxTreeBase parent)
		{
			for (_marker_list_ node = this; node != null; node = node.m__marker_list_)
			{
				node.parent = parent;
				node.m__marker_?.reparent (parent);
				parent = node;
			}
		}

		//
		// initialize object associated with syntax rule <marker list>
		//

		public void _init ()
		{
			m__marker_ = null;
			m__marker_list_ = null;
		}

		public delegate object IteratorDelegate (_marker_list_ p__marker_list_, object appData);

		public object Iterate (object appData, IteratorDelegate iteratorDelegate)
		{
			_marker_list_ p__marker_list_;
			for (p__marker_list_ = this; p__marker_list_.m__marker_list_ != null; p__marker_list_ = p__marker_list_.m__marker_list_);
			for (SyntaxTreeBase parent = p__marker_list_; (parent != null) && (parent is _marker_list_); parent = parent.parent)
				appData = iteratorDelegate ((_marker_list_) parent, appData);
			return appData;
		}
		#endregion
		#region Fields and properties associated with the syntax rule <marker list>

		// counter of all nodes associated with syntax rule <marker list>
		public static int g_counter;

		// objects associated with terminal and non-terminal symbols within production(s) of syntax rule <marker list>
		public _marker_ m__marker_ { get; private set; }
		public _marker_list_ m__marker_list_ { get; private set; }
		#endregion

	};

	public partial class SyntaxTreeWalker
	{
		#region delegates and events associated with the syntax rule <marker list>

		// delegate function (callback) prototype associated with syntax rule <marker list>
		public delegate bool _marker_list__Callback (SyntaxTreeCallbackReason reason, _marker_list_.production_kind kind, _marker_list_ p__marker_list_);

		// event associated with syntax rule <marker list>
		public event _marker_list__Callback _marker_list__Event;

		// event trigger associated with syntax rule <marker list>
		public bool Raise__marker_list__Event (SyntaxTreeCallbackReason reason, _marker_list_.production_kind kind, _marker_list_ p__marker_list_)
		{
			bool? status = _marker_list__Event?.Invoke (reason, kind, p__marker_list_);
			return (status == null) || status.Value;
		}
		#endregion
		#region syntax tree traversal methods associated with the syntax rule <marker list>
		//
		// traverse syntax tree node associated with any production of syntax rule <marker list>
		//

		public void Traverse (_marker_list_ p__marker_list_)
		{
			if (p__marker_list_.isLocked())
				return;
			if (Raise__marker_list__Event (SyntaxTreeCallbackReason.IterationPrologueCallbackReason, (_marker_list_.production_kind) p__marker_list_.kind, p__marker_list_))
				p__marker_list_.Iterate (null, (node, appData) =>
				{
					if (node.isLocked())
						return null;
					node.dolock();
					_marker_list_.production_kind kind = (_marker_list_.production_kind) node.kind;
					node.turn_reset ();
					if (Raise__marker_list__Event (SyntaxTreeCallbackReason.TraversalPrologueCallbackReason, kind, node))
						switch (kind)
						{
							case _marker_list_.production_kind.g__marker_list__1:
								// traverse syntax tree node associated with production
								// <marker list>: <marker>

								if (Raise__marker_list__Event (SyntaxTreeCallbackReason.TraversalMidTermCallbackReason, kind, node))
									Traverse (node.m__marker_);
								node.turn_inc ();
								Raise__marker_list__Event (SyntaxTreeCallbackReason.TraversalMidTermCallbackReason, kind, node);
							break;
							case _marker_list_.production_kind.g__marker_list__2:
								// traverse syntax tree node associated with production
								// <marker list>: <marker list> <marker>

								if (Raise__marker_list__Event (SyntaxTreeCallbackReason.TraversalMidTermCallbackReason, kind, node))
									;
								node.turn_inc ();
								if (Raise__marker_list__Event (SyntaxTreeCallbackReason.TraversalMidTermCallbackReason, kind, node))
									Traverse (node.m__marker_);
								node.turn_inc ();
								Raise__marker_list__Event (SyntaxTreeCallbackReason.TraversalMidTermCallbackReason, kind, node);
							break;
						}
					Raise__marker_list__Event (SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason, kind, node);
					node.unlock();
					return null;
				});
			Raise__marker_list__Event (SyntaxTreeCallbackReason.IterationEpilogueCallbackReason, (_marker_list_.production_kind) p__marker_list_.kind, p__marker_list_);
		}

		//
		// traverse syntax tree node associated with any production of syntax rule <marker list>
		//

		public void TraverseCommon (_marker_list_ p__marker_list_)
		{
			if (Raise_Common_Event (SyntaxTreeCallbackReason.IterationPrologueCallbackReason, (int) p__marker_list_.kind, p__marker_list_))
				p__marker_list_.Iterate (null, (node, appData) =>
				{
					_marker_list_.production_kind kind = (_marker_list_.production_kind) node.kind;
					if (Raise_Common_Event (SyntaxTreeCallbackReason.TraversalPrologueCallbackReason, (int) kind, node))
						switch (kind)
						{
						case _marker_list_.production_kind.g__marker_list__1:
							// traverse syntax tree node associated with production
							// <marker list>: <marker>

							TraverseCommon (node.m__marker_);
							break;
						case _marker_list_.production_kind.g__marker_list__2:
							// traverse syntax tree node associated with production
							// <marker list>: <marker list> <marker>

							TraverseCommon (node.m__marker_);
							break;
						}
					Raise_Common_Event (SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason, (int) kind, node);
					return null;
				});
			Raise_Common_Event (SyntaxTreeCallbackReason.IterationEpilogueCallbackReason, (int) p__marker_list_.kind, p__marker_list_);
		}
	};
		#endregion

	public partial class SyntaxTreeBuilder : SyntaxTreeWalker
	{
		#region syntax tree builder methods associated with the syntax rule <marker list>

		//
		// create syntax tree node associated with production
		// <marker list>: <marker>

		//

		public _marker_list_ _marker_list__1 (_marker_ p__marker_)
		{
			_marker_list_ p__marker_list__ref = new _marker_list_(p__marker_);
			p__marker_.parent = p__marker_list__ref;
			Raise__marker_list__Event (SyntaxTreeCallbackReason.BuilderCallbackReason, _marker_list_.production_kind.g__marker_list__1, p__marker_list__ref);
			return p__marker_list__ref;
		}

		//
		// create syntax tree node associated with production
		// <marker list>: <marker list> <marker>

		//

		public _marker_list_ _marker_list__2 (_marker_list_ p__marker_list_, _marker_ p__marker_)
		{
			_marker_list_ p__marker_list__ref = new _marker_list_(p__marker_list_, p__marker_);
			p__marker_list_.parent = p__marker_list__ref;
			p__marker_.parent = p__marker_list__ref;
			Raise__marker_list__Event (SyntaxTreeCallbackReason.BuilderCallbackReason, _marker_list_.production_kind.g__marker_list__2, p__marker_list__ref);
			return p__marker_list__ref;
		}
		#endregion
	};
}
