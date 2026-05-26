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
	// class associated with syntax rule <attribute list>
	//

	public class	_attribute_list_ : SyntaxTreeBase
	{
		#region enumerated production(s) of syntax rule <attribute list>
		public enum production_kind : ushort	// enumerated production(s) of syntax rule <attribute list>
		{
			g__attribute_list__1 = 1,	// <attribute>

			g__attribute_list__2 = 2,	// <attribute list> <attribute>

		};
		#endregion
		#region production markers associated with the syntax rule <attribute list>

		// markers associated with production: <attribute list> -> <attribute>

		public enum production_marker_1 : ushort
		{
			m__attribute_,
			final
		};

		// markers associated with production: <attribute list> -> <attribute list> <attribute>

		public enum production_marker_2 : ushort
		{
			m__attribute_list_,
			m__attribute_,
			final
		};
		#endregion
		#region Constructors and destructors associated with the syntax rule <attribute list>

		// Constructor declaration(s) associated with production(s) of syntax rule <attribute list>

		//
		// Constructor associated with the following production(s)
		// <attribute list> -> <attribute>

		//

		public _attribute_list_ (_attribute_ p__attribute_) : base ((uint) ProductionID.__attribute_list__ID, (uint) production_kind.g__attribute_list__1)
		{
			++g_counter;
			_init ();
			children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 1);
			children[0] = m__attribute_ = p__attribute_;
		}

		//
		// Constructor associated with the following production(s)
		// <attribute list> -> <attribute list> <attribute>

		//

		public _attribute_list_ (_attribute_list_ p__attribute_list_, _attribute_ p__attribute_) : base ((uint) ProductionID.__attribute_list__ID, (uint) production_kind.g__attribute_list__2)
		{
			++g_counter;
			_init ();
			children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 2);
			children[0] = m__attribute_list_ = p__attribute_list_;
			children[1] = m__attribute_ = p__attribute_;
		}

		// Copy constructor

		public _attribute_list_ (_attribute_list_ p__attribute_list_) : base (p__attribute_list_.id, p__attribute_list_.kind)
		{
			++g_counter;
			_init ();
			switch ((production_kind) this.kind)
			{
			case production_kind.g__attribute_list__1:
				children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 1);
				children[0] = m__attribute_ = p__attribute_list_.m__attribute_;
				break;
			case production_kind.g__attribute_list__2:
				children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 2);
				children[0] = m__attribute_list_ = p__attribute_list_.m__attribute_list_;
				children[1] = m__attribute_ = p__attribute_list_.m__attribute_;
				break;
			default:
				string[] args = new string[] { "_attribute_list_" };
				throw new SyntaxTreeError (SyntaxTreeError.InvalidKindError, args);
			}
		}

		// Clone node - wrapper of copy constructor

		public override SyntaxTreeBase Clone () { return (SyntaxTreeBase) new _attribute_list_ (this); }

		// Destructor

		public new void Dispose ()
		{
			Iterate (null, (node, appData) =>
			{
				--g_counter;

				node.m__attribute_?.Dispose ();

				node._init ();

				return null;
			});
		}
		#endregion
		#region Content changing and inclusion checking methods associated with the syntax rule <attribute list>

		// Content changing function(s) associated with production(s) of syntax rule <attribute list>

		//
		// Content changing function associated with following production(s)
		// <attribute list> -> <attribute>

		//

		public void change(_attribute_ p__attribute_)
		{
			_init ();
			this.kind = (uint) production_kind.g__attribute_list__1;
			children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 1);
			children [0] = m__attribute_ = p__attribute_;
			reparent (parent);
		}

		//
		// Content changing function associated with following production(s)
		// <attribute list> -> <attribute list> <attribute>

		//

		public void change(_attribute_list_ p__attribute_list_, _attribute_ p__attribute_)
		{
			_init ();
			this.kind = (uint) production_kind.g__attribute_list__2;
			children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 2);
			children [0] = m__attribute_list_ = p__attribute_list_;
			children [1] = m__attribute_ = p__attribute_;
			reparent (parent);
		}

		//
		// check if node 'element' is included within syntax tree associated with syntax rule <attribute list>
		//

		public new bool checkInclusion (SyntaxTreeBase element)
		{
			return (element == this) || (bool) Iterate (false, (node, appData) =>
			{
				return
					(bool) appData ||
					(node.m__attribute_ != null) && node.m__attribute_.checkInclusion (element) ||
					false;
			});
		}
		#endregion
		#region Syntax tree emit methods associated with the syntax rule <attribute list>

		//
		// emit production tree node associated with any production of syntax rule <attribute list>
		//

		public override string Emit (int depth)
		{
			string s = "";
			if (depth != 0)
				Iterate (null, (node, appData) =>
				{
					switch ((production_kind) node.kind)
					{
					case production_kind.g__attribute_list__1:
						// emit syntax tree node associated with production
						// <attribute list>: <attribute>

						s += " " + node.m__attribute_.Emit (depth - 1);
						break;
					case production_kind.g__attribute_list__2:
						// emit syntax tree node associated with production
						// <attribute list>: <attribute list> <attribute>

						s += " " + node.m__attribute_.Emit (depth - 1);
						break;
					}
					return null;
				});
			return s.Trim ();
		}

		//
		// emit production tree node associated with any production of syntax rule <attribute list>
		//

		public override string EmitProductionTree (int depth)
		{
			return (depth != 0) ?
				(string) Iterate ("", (node, appData) =>
				{
					string str = "_attribute_list_ (" + (string) appData;
					switch ((production_kind) node.kind)
					{
					case production_kind.g__attribute_list__1:
						// emit syntax tree node associated with production
						// <attribute list>: <attribute>

						str += node.m__attribute_.EmitProductionTree (depth - 1);
						break;
					case production_kind.g__attribute_list__2:
						// emit syntax tree node associated with production
						// <attribute list>: <attribute list> <attribute>

						str += ' ';
						str += node.m__attribute_.EmitProductionTree (depth - 1);
						break;
					}
					return str + ")";
				}) : "";
		}
		#endregion
		#region Syntax tree traversal,  reperrent, init and iterator methods associated with the syntax rule <attribute list>

		//
		// traverse sub-tree rooted in this node using selected syntax tree walker
		//

		public override void InvokeTraverse (SyntaxTreeWalkerCore syntaxTreeWalker) => ((SyntaxTreeWalker) syntaxTreeWalker).Traverse (this);
		public override void InvokeTraverseCommon (SyntaxTreeWalkerCore syntaxTreeWalker) => ((SyntaxTreeWalker) syntaxTreeWalker).TraverseCommon (this);

		// reparent sub-tree

		public override void reparent (SyntaxTreeBase parent)
		{
			for (_attribute_list_ node = this; node != null; node = node.m__attribute_list_)
			{
				node.parent = parent;
				node.m__attribute_?.reparent (parent);
				parent = node;
			}
		}

		//
		// initialize object associated with syntax rule <attribute list>
		//

		public void _init ()
		{
			m__attribute_ = null;
			m__attribute_list_ = null;
		}

		public delegate object IteratorDelegate (_attribute_list_ p__attribute_list_, object appData);

		public object Iterate (object appData, IteratorDelegate iteratorDelegate)
		{
			_attribute_list_ p__attribute_list_;
			for (p__attribute_list_ = this; p__attribute_list_.m__attribute_list_ != null; p__attribute_list_ = p__attribute_list_.m__attribute_list_);
			for (SyntaxTreeBase parent = p__attribute_list_; (parent != null) && (parent is _attribute_list_); parent = parent.parent)
				appData = iteratorDelegate ((_attribute_list_) parent, appData);
			return appData;
		}
		#endregion
		#region Fields and properties associated with the syntax rule <attribute list>

		// counter of all nodes associated with syntax rule <attribute list>
		public static int g_counter;

		// objects associated with terminal and non-terminal symbols within production(s) of syntax rule <attribute list>
		public _attribute_ m__attribute_ { get; private set; }
		public _attribute_list_ m__attribute_list_ { get; private set; }
		#endregion

	};

	public partial class SyntaxTreeWalker
	{
		#region delegates and events associated with the syntax rule <attribute list>

		// delegate function (callback) prototype associated with syntax rule <attribute list>
		public delegate bool _attribute_list__Callback (SyntaxTreeCallbackReason reason, _attribute_list_.production_kind kind, _attribute_list_ p__attribute_list_);

		// event associated with syntax rule <attribute list>
		public event _attribute_list__Callback _attribute_list__Event;

		// event trigger associated with syntax rule <attribute list>
		public bool Raise__attribute_list__Event (SyntaxTreeCallbackReason reason, _attribute_list_.production_kind kind, _attribute_list_ p__attribute_list_)
		{
			bool? status = _attribute_list__Event?.Invoke (reason, kind, p__attribute_list_);
			return (status == null) || status.Value;
		}
		#endregion
		#region syntax tree traversal methods associated with the syntax rule <attribute list>
		//
		// traverse syntax tree node associated with any production of syntax rule <attribute list>
		//

		public void Traverse (_attribute_list_ p__attribute_list_)
		{
			if (p__attribute_list_.isLocked())
				return;
			if (Raise__attribute_list__Event (SyntaxTreeCallbackReason.IterationPrologueCallbackReason, (_attribute_list_.production_kind) p__attribute_list_.kind, p__attribute_list_))
				p__attribute_list_.Iterate (null, (node, appData) =>
				{
					if (node.isLocked())
						return null;
					node.dolock();
					_attribute_list_.production_kind kind = (_attribute_list_.production_kind) node.kind;
					node.turn_reset ();
					if (Raise__attribute_list__Event (SyntaxTreeCallbackReason.TraversalPrologueCallbackReason, kind, node))
						switch (kind)
						{
							case _attribute_list_.production_kind.g__attribute_list__1:
								// traverse syntax tree node associated with production
								// <attribute list>: <attribute>

								if (Raise__attribute_list__Event (SyntaxTreeCallbackReason.TraversalMidTermCallbackReason, kind, node))
									Traverse (node.m__attribute_);
								node.turn_inc ();
								Raise__attribute_list__Event (SyntaxTreeCallbackReason.TraversalMidTermCallbackReason, kind, node);
							break;
							case _attribute_list_.production_kind.g__attribute_list__2:
								// traverse syntax tree node associated with production
								// <attribute list>: <attribute list> <attribute>

								if (Raise__attribute_list__Event (SyntaxTreeCallbackReason.TraversalMidTermCallbackReason, kind, node))
									;
								node.turn_inc ();
								if (Raise__attribute_list__Event (SyntaxTreeCallbackReason.TraversalMidTermCallbackReason, kind, node))
									Traverse (node.m__attribute_);
								node.turn_inc ();
								Raise__attribute_list__Event (SyntaxTreeCallbackReason.TraversalMidTermCallbackReason, kind, node);
							break;
						}
					Raise__attribute_list__Event (SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason, kind, node);
					node.unlock();
					return null;
				});
			Raise__attribute_list__Event (SyntaxTreeCallbackReason.IterationEpilogueCallbackReason, (_attribute_list_.production_kind) p__attribute_list_.kind, p__attribute_list_);
		}

		//
		// traverse syntax tree node associated with any production of syntax rule <attribute list>
		//

		public void TraverseCommon (_attribute_list_ p__attribute_list_)
		{
			if (Raise_Common_Event (SyntaxTreeCallbackReason.IterationPrologueCallbackReason, (int) p__attribute_list_.kind, p__attribute_list_))
				p__attribute_list_.Iterate (null, (node, appData) =>
				{
					_attribute_list_.production_kind kind = (_attribute_list_.production_kind) node.kind;
					if (Raise_Common_Event (SyntaxTreeCallbackReason.TraversalPrologueCallbackReason, (int) kind, node))
						switch (kind)
						{
						case _attribute_list_.production_kind.g__attribute_list__1:
							// traverse syntax tree node associated with production
							// <attribute list>: <attribute>

							TraverseCommon (node.m__attribute_);
							break;
						case _attribute_list_.production_kind.g__attribute_list__2:
							// traverse syntax tree node associated with production
							// <attribute list>: <attribute list> <attribute>

							TraverseCommon (node.m__attribute_);
							break;
						}
					Raise_Common_Event (SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason, (int) kind, node);
					return null;
				});
			Raise_Common_Event (SyntaxTreeCallbackReason.IterationEpilogueCallbackReason, (int) p__attribute_list_.kind, p__attribute_list_);
		}
	};
		#endregion

	public partial class SyntaxTreeBuilder : SyntaxTreeWalker
	{
		#region syntax tree builder methods associated with the syntax rule <attribute list>

		//
		// create syntax tree node associated with production
		// <attribute list>: <attribute>

		//

		public _attribute_list_ _attribute_list__1 (_attribute_ p__attribute_)
		{
			_attribute_list_ p__attribute_list__ref = new _attribute_list_(p__attribute_);
			p__attribute_.parent = p__attribute_list__ref;
			Raise__attribute_list__Event (SyntaxTreeCallbackReason.BuilderCallbackReason, _attribute_list_.production_kind.g__attribute_list__1, p__attribute_list__ref);
			return p__attribute_list__ref;
		}

		//
		// create syntax tree node associated with production
		// <attribute list>: <attribute list> <attribute>

		//

		public _attribute_list_ _attribute_list__2 (_attribute_list_ p__attribute_list_, _attribute_ p__attribute_)
		{
			_attribute_list_ p__attribute_list__ref = new _attribute_list_(p__attribute_list_, p__attribute_);
			p__attribute_list_.parent = p__attribute_list__ref;
			p__attribute_.parent = p__attribute_list__ref;
			Raise__attribute_list__Event (SyntaxTreeCallbackReason.BuilderCallbackReason, _attribute_list_.production_kind.g__attribute_list__2, p__attribute_list__ref);
			return p__attribute_list__ref;
		}
		#endregion
	};
}
