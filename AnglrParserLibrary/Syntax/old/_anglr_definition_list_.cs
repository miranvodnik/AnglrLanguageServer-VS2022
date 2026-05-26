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
	// class associated with syntax rule <anglr definition list>
	//

	public class	_anglr_definition_list_ : SyntaxTreeBase
	{
		#region enumerated production(s) of syntax rule <anglr definition list>
		public enum production_kind : ushort	// enumerated production(s) of syntax rule <anglr definition list>
		{
			g__anglr_definition_list__1 = 1,	// <anglr definition with attribute>

			g__anglr_definition_list__2 = 2,	// <anglr definition list> <anglr definition with attribute>

		};
		#endregion
		#region production markers associated with the syntax rule <anglr definition list>

		// markers associated with production: <anglr definition list> -> <anglr definition with attribute>

		public enum production_marker_1 : ushort
		{
			m__anglr_definition_with_attribute_,
			final
		};

		// markers associated with production: <anglr definition list> -> <anglr definition list> <anglr definition with attribute>

		public enum production_marker_2 : ushort
		{
			m__anglr_definition_list_,
			m__anglr_definition_with_attribute_,
			final
		};
		#endregion
		#region Constructors and destructors associated with the syntax rule <anglr definition list>

		// Constructor declaration(s) associated with production(s) of syntax rule <anglr definition list>

		//
		// Constructor associated with the following production(s)
		// <anglr definition list> -> <anglr definition with attribute>

		//

		public _anglr_definition_list_ (_anglr_definition_with_attribute_ p__anglr_definition_with_attribute_) : base ((uint) ProductionID.__anglr_definition_list__ID, (uint) production_kind.g__anglr_definition_list__1)
		{
			++g_counter;
			_init ();
			children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 1);
			children[0] = m__anglr_definition_with_attribute_ = p__anglr_definition_with_attribute_;
		}

		//
		// Constructor associated with the following production(s)
		// <anglr definition list> -> <anglr definition list> <anglr definition with attribute>

		//

		public _anglr_definition_list_ (_anglr_definition_list_ p__anglr_definition_list_, _anglr_definition_with_attribute_ p__anglr_definition_with_attribute_) : base ((uint) ProductionID.__anglr_definition_list__ID, (uint) production_kind.g__anglr_definition_list__2)
		{
			++g_counter;
			_init ();
			children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 2);
			children[0] = m__anglr_definition_list_ = p__anglr_definition_list_;
			children[1] = m__anglr_definition_with_attribute_ = p__anglr_definition_with_attribute_;
		}

		// Copy constructor

		public _anglr_definition_list_ (_anglr_definition_list_ p__anglr_definition_list_) : base (p__anglr_definition_list_.id, p__anglr_definition_list_.kind)
		{
			++g_counter;
			_init ();
			switch ((production_kind) this.kind)
			{
			case production_kind.g__anglr_definition_list__1:
				children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 1);
				children[0] = m__anglr_definition_with_attribute_ = p__anglr_definition_list_.m__anglr_definition_with_attribute_;
				break;
			case production_kind.g__anglr_definition_list__2:
				children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 2);
				children[0] = m__anglr_definition_list_ = p__anglr_definition_list_.m__anglr_definition_list_;
				children[1] = m__anglr_definition_with_attribute_ = p__anglr_definition_list_.m__anglr_definition_with_attribute_;
				break;
			default:
				string[] args = new string[] { "_anglr_definition_list_" };
				throw new SyntaxTreeError (SyntaxTreeError.InvalidKindError, args);
			}
		}

		// Clone node - wrapper of copy constructor

		public override SyntaxTreeBase Clone () { return (SyntaxTreeBase) new _anglr_definition_list_ (this); }

		// Destructor

		public new void Dispose ()
		{
			Iterate (null, (node, appData) =>
			{
				--g_counter;

				node.m__anglr_definition_with_attribute_?.Dispose ();

				node._init ();

				return null;
			});
		}
		#endregion
		#region Content changing and inclusion checking methods associated with the syntax rule <anglr definition list>

		// Content changing function(s) associated with production(s) of syntax rule <anglr definition list>

		//
		// Content changing function associated with following production(s)
		// <anglr definition list> -> <anglr definition with attribute>

		//

		public void change(_anglr_definition_with_attribute_ p__anglr_definition_with_attribute_)
		{
			_init ();
			this.kind = (uint) production_kind.g__anglr_definition_list__1;
			children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 1);
			children [0] = m__anglr_definition_with_attribute_ = p__anglr_definition_with_attribute_;
			reparent (parent);
		}

		//
		// Content changing function associated with following production(s)
		// <anglr definition list> -> <anglr definition list> <anglr definition with attribute>

		//

		public void change(_anglr_definition_list_ p__anglr_definition_list_, _anglr_definition_with_attribute_ p__anglr_definition_with_attribute_)
		{
			_init ();
			this.kind = (uint) production_kind.g__anglr_definition_list__2;
			children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 2);
			children [0] = m__anglr_definition_list_ = p__anglr_definition_list_;
			children [1] = m__anglr_definition_with_attribute_ = p__anglr_definition_with_attribute_;
			reparent (parent);
		}

		//
		// check if node 'element' is included within syntax tree associated with syntax rule <anglr definition list>
		//

		public new bool checkInclusion (SyntaxTreeBase element)
		{
			return (element == this) || (bool) Iterate (false, (node, appData) =>
			{
				return
					(bool) appData ||
					(node.m__anglr_definition_with_attribute_ != null) && node.m__anglr_definition_with_attribute_.checkInclusion (element) ||
					false;
			});
		}
		#endregion
		#region Syntax tree emit methods associated with the syntax rule <anglr definition list>

		//
		// emit production tree node associated with any production of syntax rule <anglr definition list>
		//

		public override string Emit (int depth)
		{
			string s = "";
			if (depth != 0)
				Iterate (null, (node, appData) =>
				{
					switch ((production_kind) node.kind)
					{
					case production_kind.g__anglr_definition_list__1:
						// emit syntax tree node associated with production
						// <anglr definition list>: <anglr definition with attribute>

						s += " " + node.m__anglr_definition_with_attribute_.Emit (depth - 1);
						break;
					case production_kind.g__anglr_definition_list__2:
						// emit syntax tree node associated with production
						// <anglr definition list>: <anglr definition list> <anglr definition with attribute>

						s += " " + node.m__anglr_definition_with_attribute_.Emit (depth - 1);
						break;
					}
					return null;
				});
			return s.Trim ();
		}

		//
		// emit production tree node associated with any production of syntax rule <anglr definition list>
		//

		public override string EmitProductionTree (int depth)
		{
			return (depth != 0) ?
				(string) Iterate ("", (node, appData) =>
				{
					string str = "_anglr_definition_list_ (" + (string) appData;
					switch ((production_kind) node.kind)
					{
					case production_kind.g__anglr_definition_list__1:
						// emit syntax tree node associated with production
						// <anglr definition list>: <anglr definition with attribute>

						str += node.m__anglr_definition_with_attribute_.EmitProductionTree (depth - 1);
						break;
					case production_kind.g__anglr_definition_list__2:
						// emit syntax tree node associated with production
						// <anglr definition list>: <anglr definition list> <anglr definition with attribute>

						str += ' ';
						str += node.m__anglr_definition_with_attribute_.EmitProductionTree (depth - 1);
						break;
					}
					return str + ")";
				}) : "";
		}
		#endregion
		#region Syntax tree traversal,  reperrent, init and iterator methods associated with the syntax rule <anglr definition list>

		//
		// traverse sub-tree rooted in this node using selected syntax tree walker
		//

		public override void InvokeTraverse (SyntaxTreeWalkerCore syntaxTreeWalker) => ((SyntaxTreeWalker) syntaxTreeWalker).Traverse (this);
		public override void InvokeTraverseCommon (SyntaxTreeWalkerCore syntaxTreeWalker) => ((SyntaxTreeWalker) syntaxTreeWalker).TraverseCommon (this);

		// reparent sub-tree

		public override void reparent (SyntaxTreeBase parent)
		{
			for (_anglr_definition_list_ node = this; node != null; node = node.m__anglr_definition_list_)
			{
				node.parent = parent;
				node.m__anglr_definition_with_attribute_?.reparent (parent);
				parent = node;
			}
		}

		//
		// initialize object associated with syntax rule <anglr definition list>
		//

		public void _init ()
		{
			m__anglr_definition_with_attribute_ = null;
			m__anglr_definition_list_ = null;
		}

		public delegate object IteratorDelegate (_anglr_definition_list_ p__anglr_definition_list_, object appData);

		public object Iterate (object appData, IteratorDelegate iteratorDelegate)
		{
			_anglr_definition_list_ p__anglr_definition_list_;
			for (p__anglr_definition_list_ = this; p__anglr_definition_list_.m__anglr_definition_list_ != null; p__anglr_definition_list_ = p__anglr_definition_list_.m__anglr_definition_list_);
			for (SyntaxTreeBase parent = p__anglr_definition_list_; (parent != null) && (parent is _anglr_definition_list_); parent = parent.parent)
				appData = iteratorDelegate ((_anglr_definition_list_) parent, appData);
			return appData;
		}
		#endregion
		#region Fields and properties associated with the syntax rule <anglr definition list>

		// counter of all nodes associated with syntax rule <anglr definition list>
		public static int g_counter;

		// objects associated with terminal and non-terminal symbols within production(s) of syntax rule <anglr definition list>
		public _anglr_definition_with_attribute_ m__anglr_definition_with_attribute_ { get; private set; }
		public _anglr_definition_list_ m__anglr_definition_list_ { get; private set; }
		#endregion

	};

	public partial class SyntaxTreeWalker
	{
		#region delegates and events associated with the syntax rule <anglr definition list>

		// delegate function (callback) prototype associated with syntax rule <anglr definition list>
		public delegate bool _anglr_definition_list__Callback (SyntaxTreeCallbackReason reason, _anglr_definition_list_.production_kind kind, _anglr_definition_list_ p__anglr_definition_list_);

		// event associated with syntax rule <anglr definition list>
		public event _anglr_definition_list__Callback _anglr_definition_list__Event;

		// event trigger associated with syntax rule <anglr definition list>
		public bool Raise__anglr_definition_list__Event (SyntaxTreeCallbackReason reason, _anglr_definition_list_.production_kind kind, _anglr_definition_list_ p__anglr_definition_list_)
		{
			bool? status = _anglr_definition_list__Event?.Invoke (reason, kind, p__anglr_definition_list_);
			return (status == null) || status.Value;
		}
		#endregion
		#region syntax tree traversal methods associated with the syntax rule <anglr definition list>
		//
		// traverse syntax tree node associated with any production of syntax rule <anglr definition list>
		//

		public void Traverse (_anglr_definition_list_ p__anglr_definition_list_)
		{
			if (p__anglr_definition_list_.isLocked())
				return;
			if (Raise__anglr_definition_list__Event (SyntaxTreeCallbackReason.IterationPrologueCallbackReason, (_anglr_definition_list_.production_kind) p__anglr_definition_list_.kind, p__anglr_definition_list_))
				p__anglr_definition_list_.Iterate (null, (node, appData) =>
				{
					if (node.isLocked())
						return null;
					node.dolock();
					_anglr_definition_list_.production_kind kind = (_anglr_definition_list_.production_kind) node.kind;
					node.turn_reset ();
					if (Raise__anglr_definition_list__Event (SyntaxTreeCallbackReason.TraversalPrologueCallbackReason, kind, node))
						switch (kind)
						{
							case _anglr_definition_list_.production_kind.g__anglr_definition_list__1:
								// traverse syntax tree node associated with production
								// <anglr definition list>: <anglr definition with attribute>

								if (Raise__anglr_definition_list__Event (SyntaxTreeCallbackReason.TraversalMidTermCallbackReason, kind, node))
									Traverse (node.m__anglr_definition_with_attribute_);
								node.turn_inc ();
								Raise__anglr_definition_list__Event (SyntaxTreeCallbackReason.TraversalMidTermCallbackReason, kind, node);
							break;
							case _anglr_definition_list_.production_kind.g__anglr_definition_list__2:
								// traverse syntax tree node associated with production
								// <anglr definition list>: <anglr definition list> <anglr definition with attribute>

								if (Raise__anglr_definition_list__Event (SyntaxTreeCallbackReason.TraversalMidTermCallbackReason, kind, node))
									;
								node.turn_inc ();
								if (Raise__anglr_definition_list__Event (SyntaxTreeCallbackReason.TraversalMidTermCallbackReason, kind, node))
									Traverse (node.m__anglr_definition_with_attribute_);
								node.turn_inc ();
								Raise__anglr_definition_list__Event (SyntaxTreeCallbackReason.TraversalMidTermCallbackReason, kind, node);
							break;
						}
					Raise__anglr_definition_list__Event (SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason, kind, node);
					node.unlock();
					return null;
				});
			Raise__anglr_definition_list__Event (SyntaxTreeCallbackReason.IterationEpilogueCallbackReason, (_anglr_definition_list_.production_kind) p__anglr_definition_list_.kind, p__anglr_definition_list_);
		}

		//
		// traverse syntax tree node associated with any production of syntax rule <anglr definition list>
		//

		public void TraverseCommon (_anglr_definition_list_ p__anglr_definition_list_)
		{
			if (Raise_Common_Event (SyntaxTreeCallbackReason.IterationPrologueCallbackReason, (int) p__anglr_definition_list_.kind, p__anglr_definition_list_))
				p__anglr_definition_list_.Iterate (null, (node, appData) =>
				{
					_anglr_definition_list_.production_kind kind = (_anglr_definition_list_.production_kind) node.kind;
					if (Raise_Common_Event (SyntaxTreeCallbackReason.TraversalPrologueCallbackReason, (int) kind, node))
						switch (kind)
						{
						case _anglr_definition_list_.production_kind.g__anglr_definition_list__1:
							// traverse syntax tree node associated with production
							// <anglr definition list>: <anglr definition with attribute>

							TraverseCommon (node.m__anglr_definition_with_attribute_);
							break;
						case _anglr_definition_list_.production_kind.g__anglr_definition_list__2:
							// traverse syntax tree node associated with production
							// <anglr definition list>: <anglr definition list> <anglr definition with attribute>

							TraverseCommon (node.m__anglr_definition_with_attribute_);
							break;
						}
					Raise_Common_Event (SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason, (int) kind, node);
					return null;
				});
			Raise_Common_Event (SyntaxTreeCallbackReason.IterationEpilogueCallbackReason, (int) p__anglr_definition_list_.kind, p__anglr_definition_list_);
		}
	};
		#endregion

	public partial class SyntaxTreeBuilder : SyntaxTreeWalker
	{
		#region syntax tree builder methods associated with the syntax rule <anglr definition list>

		//
		// create syntax tree node associated with production
		// <anglr definition list>: <anglr definition with attribute>

		//

		public _anglr_definition_list_ _anglr_definition_list__1 (_anglr_definition_with_attribute_ p__anglr_definition_with_attribute_)
		{
			_anglr_definition_list_ p__anglr_definition_list__ref = new _anglr_definition_list_(p__anglr_definition_with_attribute_);
			p__anglr_definition_with_attribute_.parent = p__anglr_definition_list__ref;
			Raise__anglr_definition_list__Event (SyntaxTreeCallbackReason.BuilderCallbackReason, _anglr_definition_list_.production_kind.g__anglr_definition_list__1, p__anglr_definition_list__ref);
			return p__anglr_definition_list__ref;
		}

		//
		// create syntax tree node associated with production
		// <anglr definition list>: <anglr definition list> <anglr definition with attribute>

		//

		public _anglr_definition_list_ _anglr_definition_list__2 (_anglr_definition_list_ p__anglr_definition_list_, _anglr_definition_with_attribute_ p__anglr_definition_with_attribute_)
		{
			_anglr_definition_list_ p__anglr_definition_list__ref = new _anglr_definition_list_(p__anglr_definition_list_, p__anglr_definition_with_attribute_);
			p__anglr_definition_list_.parent = p__anglr_definition_list__ref;
			p__anglr_definition_with_attribute_.parent = p__anglr_definition_list__ref;
			Raise__anglr_definition_list__Event (SyntaxTreeCallbackReason.BuilderCallbackReason, _anglr_definition_list_.production_kind.g__anglr_definition_list__2, p__anglr_definition_list__ref);
			return p__anglr_definition_list__ref;
		}
		#endregion
	};
}
