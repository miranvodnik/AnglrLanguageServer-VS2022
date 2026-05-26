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
	// class associated with syntax rule <name value list>
	//

	public class	_name_value_list_ : SyntaxTreeBase
	{
		#region enumerated production(s) of syntax rule <name value list>
		public enum production_kind : ushort	// enumerated production(s) of syntax rule <name value list>
		{
			g__name_value_list__1 = 1,	// <name value pair>

			g__name_value_list__2 = 2,	// <name value list> <name value pair>

		};
		#endregion
		#region production markers associated with the syntax rule <name value list>

		// markers associated with production: <name value list> -> <name value pair>

		public enum production_marker_1 : ushort
		{
			m__name_value_pair_,
			final
		};

		// markers associated with production: <name value list> -> <name value list> <name value pair>

		public enum production_marker_2 : ushort
		{
			m__name_value_list_,
			m__name_value_pair_,
			final
		};
		#endregion
		#region Constructors and destructors associated with the syntax rule <name value list>

		// Constructor declaration(s) associated with production(s) of syntax rule <name value list>

		//
		// Constructor associated with the following production(s)
		// <name value list> -> <name value pair>

		//

		public _name_value_list_ (_name_value_pair_ p__name_value_pair_) : base ((uint) ProductionID.__name_value_list__ID, (uint) production_kind.g__name_value_list__1)
		{
			++g_counter;
			_init ();
			children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 1);
			children[0] = m__name_value_pair_ = p__name_value_pair_;
		}

		//
		// Constructor associated with the following production(s)
		// <name value list> -> <name value list> <name value pair>

		//

		public _name_value_list_ (_name_value_list_ p__name_value_list_, _name_value_pair_ p__name_value_pair_) : base ((uint) ProductionID.__name_value_list__ID, (uint) production_kind.g__name_value_list__2)
		{
			++g_counter;
			_init ();
			children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 2);
			children[0] = m__name_value_list_ = p__name_value_list_;
			children[1] = m__name_value_pair_ = p__name_value_pair_;
		}

		// Copy constructor

		public _name_value_list_ (_name_value_list_ p__name_value_list_) : base (p__name_value_list_.id, p__name_value_list_.kind)
		{
			++g_counter;
			_init ();
			switch ((production_kind) this.kind)
			{
			case production_kind.g__name_value_list__1:
				children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 1);
				children[0] = m__name_value_pair_ = p__name_value_list_.m__name_value_pair_;
				break;
			case production_kind.g__name_value_list__2:
				children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 2);
				children[0] = m__name_value_list_ = p__name_value_list_.m__name_value_list_;
				children[1] = m__name_value_pair_ = p__name_value_list_.m__name_value_pair_;
				break;
			default:
				string[] args = new string[] { "_name_value_list_" };
				throw new SyntaxTreeError (SyntaxTreeError.InvalidKindError, args);
			}
		}

		// Clone node - wrapper of copy constructor

		public override SyntaxTreeBase Clone () { return (SyntaxTreeBase) new _name_value_list_ (this); }

		// Destructor

		public new void Dispose ()
		{
			Iterate (null, (node, appData) =>
			{
				--g_counter;

				node.m__name_value_pair_?.Dispose ();

				node._init ();

				return null;
			});
		}
		#endregion
		#region Content changing and inclusion checking methods associated with the syntax rule <name value list>

		// Content changing function(s) associated with production(s) of syntax rule <name value list>

		//
		// Content changing function associated with following production(s)
		// <name value list> -> <name value pair>

		//

		public void change(_name_value_pair_ p__name_value_pair_)
		{
			_init ();
			this.kind = (uint) production_kind.g__name_value_list__1;
			children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 1);
			children [0] = m__name_value_pair_ = p__name_value_pair_;
			reparent (parent);
		}

		//
		// Content changing function associated with following production(s)
		// <name value list> -> <name value list> <name value pair>

		//

		public void change(_name_value_list_ p__name_value_list_, _name_value_pair_ p__name_value_pair_)
		{
			_init ();
			this.kind = (uint) production_kind.g__name_value_list__2;
			children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 2);
			children [0] = m__name_value_list_ = p__name_value_list_;
			children [1] = m__name_value_pair_ = p__name_value_pair_;
			reparent (parent);
		}

		//
		// check if node 'element' is included within syntax tree associated with syntax rule <name value list>
		//

		public new bool checkInclusion (SyntaxTreeBase element)
		{
			return (element == this) || (bool) Iterate (false, (node, appData) =>
			{
				return
					(bool) appData ||
					(node.m__name_value_pair_ != null) && node.m__name_value_pair_.checkInclusion (element) ||
					false;
			});
		}
		#endregion
		#region Syntax tree emit methods associated with the syntax rule <name value list>

		//
		// emit production tree node associated with any production of syntax rule <name value list>
		//

		public override string Emit (int depth)
		{
			string s = "";
			if (depth != 0)
				Iterate (null, (node, appData) =>
				{
					switch ((production_kind) node.kind)
					{
					case production_kind.g__name_value_list__1:
						// emit syntax tree node associated with production
						// <name value list>: <name value pair>

						s += " " + node.m__name_value_pair_.Emit (depth - 1);
						break;
					case production_kind.g__name_value_list__2:
						// emit syntax tree node associated with production
						// <name value list>: <name value list> <name value pair>

						s += " " + node.m__name_value_pair_.Emit (depth - 1);
						break;
					}
					return null;
				});
			return s.Trim ();
		}

		//
		// emit production tree node associated with any production of syntax rule <name value list>
		//

		public override string EmitProductionTree (int depth)
		{
			return (depth != 0) ?
				(string) Iterate ("", (node, appData) =>
				{
					string str = "_name_value_list_ (" + (string) appData;
					switch ((production_kind) node.kind)
					{
					case production_kind.g__name_value_list__1:
						// emit syntax tree node associated with production
						// <name value list>: <name value pair>

						str += node.m__name_value_pair_.EmitProductionTree (depth - 1);
						break;
					case production_kind.g__name_value_list__2:
						// emit syntax tree node associated with production
						// <name value list>: <name value list> <name value pair>

						str += ' ';
						str += node.m__name_value_pair_.EmitProductionTree (depth - 1);
						break;
					}
					return str + ")";
				}) : "";
		}
		#endregion
		#region Syntax tree traversal,  reperrent, init and iterator methods associated with the syntax rule <name value list>

		//
		// traverse sub-tree rooted in this node using selected syntax tree walker
		//

		public override void InvokeTraverse (SyntaxTreeWalkerCore syntaxTreeWalker) => ((SyntaxTreeWalker) syntaxTreeWalker).Traverse (this);
		public override void InvokeTraverseCommon (SyntaxTreeWalkerCore syntaxTreeWalker) => ((SyntaxTreeWalker) syntaxTreeWalker).TraverseCommon (this);

		// reparent sub-tree

		public override void reparent (SyntaxTreeBase parent)
		{
			for (_name_value_list_ node = this; node != null; node = node.m__name_value_list_)
			{
				node.parent = parent;
				node.m__name_value_pair_?.reparent (parent);
				parent = node;
			}
		}

		//
		// initialize object associated with syntax rule <name value list>
		//

		public void _init ()
		{
			m__name_value_pair_ = null;
			m__name_value_list_ = null;
		}

		public delegate object IteratorDelegate (_name_value_list_ p__name_value_list_, object appData);

		public object Iterate (object appData, IteratorDelegate iteratorDelegate)
		{
			_name_value_list_ p__name_value_list_;
			for (p__name_value_list_ = this; p__name_value_list_.m__name_value_list_ != null; p__name_value_list_ = p__name_value_list_.m__name_value_list_);
			for (SyntaxTreeBase parent = p__name_value_list_; (parent != null) && (parent is _name_value_list_); parent = parent.parent)
				appData = iteratorDelegate ((_name_value_list_) parent, appData);
			return appData;
		}
		#endregion
		#region Fields and properties associated with the syntax rule <name value list>

		// counter of all nodes associated with syntax rule <name value list>
		public static int g_counter;

		// objects associated with terminal and non-terminal symbols within production(s) of syntax rule <name value list>
		public _name_value_pair_ m__name_value_pair_ { get; private set; }
		public _name_value_list_ m__name_value_list_ { get; private set; }
		#endregion

	};

	public partial class SyntaxTreeWalker
	{
		#region delegates and events associated with the syntax rule <name value list>

		// delegate function (callback) prototype associated with syntax rule <name value list>
		public delegate bool _name_value_list__Callback (SyntaxTreeCallbackReason reason, _name_value_list_.production_kind kind, _name_value_list_ p__name_value_list_);

		// event associated with syntax rule <name value list>
		public event _name_value_list__Callback _name_value_list__Event;

		// event trigger associated with syntax rule <name value list>
		public bool Raise__name_value_list__Event (SyntaxTreeCallbackReason reason, _name_value_list_.production_kind kind, _name_value_list_ p__name_value_list_)
		{
			bool? status = _name_value_list__Event?.Invoke (reason, kind, p__name_value_list_);
			return (status == null) || status.Value;
		}
		#endregion
		#region syntax tree traversal methods associated with the syntax rule <name value list>
		//
		// traverse syntax tree node associated with any production of syntax rule <name value list>
		//

		public void Traverse (_name_value_list_ p__name_value_list_)
		{
			if (p__name_value_list_.isLocked())
				return;
			if (Raise__name_value_list__Event (SyntaxTreeCallbackReason.IterationPrologueCallbackReason, (_name_value_list_.production_kind) p__name_value_list_.kind, p__name_value_list_))
				p__name_value_list_.Iterate (null, (node, appData) =>
				{
					if (node.isLocked())
						return null;
					node.dolock();
					_name_value_list_.production_kind kind = (_name_value_list_.production_kind) node.kind;
					node.turn_reset ();
					if (Raise__name_value_list__Event (SyntaxTreeCallbackReason.TraversalPrologueCallbackReason, kind, node))
						switch (kind)
						{
							case _name_value_list_.production_kind.g__name_value_list__1:
								// traverse syntax tree node associated with production
								// <name value list>: <name value pair>

								if (Raise__name_value_list__Event (SyntaxTreeCallbackReason.TraversalMidTermCallbackReason, kind, node))
									Traverse (node.m__name_value_pair_);
								node.turn_inc ();
								Raise__name_value_list__Event (SyntaxTreeCallbackReason.TraversalMidTermCallbackReason, kind, node);
							break;
							case _name_value_list_.production_kind.g__name_value_list__2:
								// traverse syntax tree node associated with production
								// <name value list>: <name value list> <name value pair>

								if (Raise__name_value_list__Event (SyntaxTreeCallbackReason.TraversalMidTermCallbackReason, kind, node))
									;
								node.turn_inc ();
								if (Raise__name_value_list__Event (SyntaxTreeCallbackReason.TraversalMidTermCallbackReason, kind, node))
									Traverse (node.m__name_value_pair_);
								node.turn_inc ();
								Raise__name_value_list__Event (SyntaxTreeCallbackReason.TraversalMidTermCallbackReason, kind, node);
							break;
						}
					Raise__name_value_list__Event (SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason, kind, node);
					node.unlock();
					return null;
				});
			Raise__name_value_list__Event (SyntaxTreeCallbackReason.IterationEpilogueCallbackReason, (_name_value_list_.production_kind) p__name_value_list_.kind, p__name_value_list_);
		}

		//
		// traverse syntax tree node associated with any production of syntax rule <name value list>
		//

		public void TraverseCommon (_name_value_list_ p__name_value_list_)
		{
			if (Raise_Common_Event (SyntaxTreeCallbackReason.IterationPrologueCallbackReason, (int) p__name_value_list_.kind, p__name_value_list_))
				p__name_value_list_.Iterate (null, (node, appData) =>
				{
					_name_value_list_.production_kind kind = (_name_value_list_.production_kind) node.kind;
					if (Raise_Common_Event (SyntaxTreeCallbackReason.TraversalPrologueCallbackReason, (int) kind, node))
						switch (kind)
						{
						case _name_value_list_.production_kind.g__name_value_list__1:
							// traverse syntax tree node associated with production
							// <name value list>: <name value pair>

							TraverseCommon (node.m__name_value_pair_);
							break;
						case _name_value_list_.production_kind.g__name_value_list__2:
							// traverse syntax tree node associated with production
							// <name value list>: <name value list> <name value pair>

							TraverseCommon (node.m__name_value_pair_);
							break;
						}
					Raise_Common_Event (SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason, (int) kind, node);
					return null;
				});
			Raise_Common_Event (SyntaxTreeCallbackReason.IterationEpilogueCallbackReason, (int) p__name_value_list_.kind, p__name_value_list_);
		}
	};
		#endregion

	public partial class SyntaxTreeBuilder : SyntaxTreeWalker
	{
		#region syntax tree builder methods associated with the syntax rule <name value list>

		//
		// create syntax tree node associated with production
		// <name value list>: <name value pair>

		//

		public _name_value_list_ _name_value_list__1 (_name_value_pair_ p__name_value_pair_)
		{
			_name_value_list_ p__name_value_list__ref = new _name_value_list_(p__name_value_pair_);
			p__name_value_pair_.parent = p__name_value_list__ref;
			Raise__name_value_list__Event (SyntaxTreeCallbackReason.BuilderCallbackReason, _name_value_list_.production_kind.g__name_value_list__1, p__name_value_list__ref);
			return p__name_value_list__ref;
		}

		//
		// create syntax tree node associated with production
		// <name value list>: <name value list> <name value pair>

		//

		public _name_value_list_ _name_value_list__2 (_name_value_list_ p__name_value_list_, _name_value_pair_ p__name_value_pair_)
		{
			_name_value_list_ p__name_value_list__ref = new _name_value_list_(p__name_value_list_, p__name_value_pair_);
			p__name_value_list_.parent = p__name_value_list__ref;
			p__name_value_pair_.parent = p__name_value_list__ref;
			Raise__name_value_list__Event (SyntaxTreeCallbackReason.BuilderCallbackReason, _name_value_list_.production_kind.g__name_value_list__2, p__name_value_list__ref);
			return p__name_value_list__ref;
		}
		#endregion
	};
}
