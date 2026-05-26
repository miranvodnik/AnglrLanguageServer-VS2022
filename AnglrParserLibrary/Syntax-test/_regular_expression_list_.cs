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
	// class associated with syntax rule <regular expression list>
	//

	public class	_regular_expression_list_ : SyntaxTreeBase
	{
		#region enumerated production(s) of syntax rule <regular expression list>
		public enum production_kind : ushort	// enumerated production(s) of syntax rule <regular expression list>
		{
			g__regular_expression_list__1 = 1,	// <regular expression usage>

			g__regular_expression_list__2 = 2,	// <regular expression list> <regular expression usage>

		};
		#endregion
		#region production markers associated with the syntax rule <regular expression list>

		// markers associated with production: <regular expression list> -> <regular expression usage>

		public enum production_marker_1 : ushort
		{
			m__regular_expression_usage_,
			final
		};

		// markers associated with production: <regular expression list> -> <regular expression list> <regular expression usage>

		public enum production_marker_2 : ushort
		{
			m__regular_expression_list_,
			m__regular_expression_usage_,
			final
		};
		#endregion
		#region Constructors and destructors associated with the syntax rule <regular expression list>

		// Constructor declaration(s) associated with production(s) of syntax rule <regular expression list>

		//
		// Constructor associated with the following production(s)
		// <regular expression list> -> <regular expression usage>

		//

		public _regular_expression_list_ (_regular_expression_usage_ p__regular_expression_usage_) : base ((uint) ProductionID.__regular_expression_list__ID, (uint) production_kind.g__regular_expression_list__1)
		{
			++g_counter;
			_init ();
			children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 1);
			children[0] = m__regular_expression_usage_ = p__regular_expression_usage_;
		}

		//
		// Constructor associated with the following production(s)
		// <regular expression list> -> <regular expression list> <regular expression usage>

		//

		public _regular_expression_list_ (_regular_expression_list_ p__regular_expression_list_, _regular_expression_usage_ p__regular_expression_usage_) : base ((uint) ProductionID.__regular_expression_list__ID, (uint) production_kind.g__regular_expression_list__2)
		{
			++g_counter;
			_init ();
			children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 2);
			children[0] = m__regular_expression_list_ = p__regular_expression_list_;
			children[1] = m__regular_expression_usage_ = p__regular_expression_usage_;
		}

		// Copy constructor

		public _regular_expression_list_ (_regular_expression_list_ p__regular_expression_list_) : base (p__regular_expression_list_.id, p__regular_expression_list_.kind)
		{
			++g_counter;
			_init ();
			switch ((production_kind) p__regular_expression_list_.kind)
			{
				case production_kind.g__regular_expression_list__1:
					children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 1);
					if ((children [0] = m__regular_expression_usage_ = (p__regular_expression_list_.m__regular_expression_usage_ != null) ? new _regular_expression_usage_ (p__regular_expression_list_.m__regular_expression_usage_) : null) != null) m__regular_expression_usage_.parent = this;
					break;
				case production_kind.g__regular_expression_list__2:
					children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 2);
					if ((children [0] = m__regular_expression_list_ = (p__regular_expression_list_.m__regular_expression_list_ != null) ? new _regular_expression_list_ (p__regular_expression_list_.m__regular_expression_list_) : null) != null) m__regular_expression_list_.parent = this;
					if ((children [1] = m__regular_expression_usage_ = (p__regular_expression_list_.m__regular_expression_usage_ != null) ? new _regular_expression_usage_ (p__regular_expression_list_.m__regular_expression_usage_) : null) != null) m__regular_expression_usage_.parent = this;
					break;
				default:
				{
					string[] args = new string[] { "_regular_expression_list_" };
					throw new SyntaxTreeError (SyntaxTreeError.InvalidKindError, args);
				}
			}
		}

		// Clone node - wrapper of copy constructor

		public override SyntaxTreeBase Clone () { return (SyntaxTreeBase) new _regular_expression_list_ (this); }

		// Destructor

		public new void Dispose ()
		{
			Iterate (null, (node, appData) =>
			{
				--g_counter;

				node.m__regular_expression_usage_?.Dispose ();

				node._init ();

				return null;
			});
		}
		#endregion
		#region Content changing and inclusion checking methods associated with the syntax rule <regular expression list>

		// Content changing function(s) associated with production(s) of syntax rule <regular expression list>

		//
		// Content changing function associated with following production(s)
		// <regular expression list> -> <regular expression usage>

		//

		public void change(_regular_expression_usage_ p__regular_expression_usage_)
		{
			_init ();
			this.kind = (uint) production_kind.g__regular_expression_list__1;
			children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 1);
			children [0] = m__regular_expression_usage_ = p__regular_expression_usage_;
			reparent (parent);
		}

		//
		// Content changing function associated with following production(s)
		// <regular expression list> -> <regular expression list> <regular expression usage>

		//

		public void change(_regular_expression_list_ p__regular_expression_list_, _regular_expression_usage_ p__regular_expression_usage_)
		{
			_init ();
			this.kind = (uint) production_kind.g__regular_expression_list__2;
			children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 2);
			children [0] = m__regular_expression_list_ = p__regular_expression_list_;
			children [1] = m__regular_expression_usage_ = p__regular_expression_usage_;
			reparent (parent);
		}

		//
		// check if node 'element' is included within syntax tree associated with syntax rule <regular expression list>
		//

		public new bool checkInclusion (SyntaxTreeBase element)
		{
			return (element == this) || (bool) Iterate (false, (node, appData) =>
			{
				return
					(bool) appData ||
					(node.m__regular_expression_usage_ != null) && node.m__regular_expression_usage_.checkInclusion (element) ||
					false;
			});
		}
		#endregion
		#region Syntax tree emit methods associated with the syntax rule <regular expression list>

		//
		// emit production tree node associated with any production of syntax rule <regular expression list>
		//

		public override string Emit (int depth)
		{
			string s = "";
			if (depth != 0)
				Iterate (null, (node, appData) =>
				{
					switch ((production_kind) node.kind)
					{
					case production_kind.g__regular_expression_list__1:
						// emit syntax tree node associated with production
						// <regular expression list>: <regular expression usage>

						s += " " + node.m__regular_expression_usage_.Emit (depth - 1);
						break;
					case production_kind.g__regular_expression_list__2:
						// emit syntax tree node associated with production
						// <regular expression list>: <regular expression list> <regular expression usage>

						s += " " + node.m__regular_expression_usage_.Emit (depth - 1);
						break;
					}
					return null;
				});
			return s.Trim ();
		}

		//
		// emit production tree node associated with any production of syntax rule <regular expression list>
		//

		public override string EmitProductionTree (int depth)
		{
			return (depth != 0) ?
				(string) Iterate ("", (node, appData) =>
				{
					string str = "_regular_expression_list_ (" + (string) appData;
					switch ((production_kind) node.kind)
					{
					case production_kind.g__regular_expression_list__1:
						// emit syntax tree node associated with production
						// <regular expression list>: <regular expression usage>

						str += node.m__regular_expression_usage_.EmitProductionTree (depth - 1);
						break;
					case production_kind.g__regular_expression_list__2:
						// emit syntax tree node associated with production
						// <regular expression list>: <regular expression list> <regular expression usage>

						str += ' ';
						str += node.m__regular_expression_usage_.EmitProductionTree (depth - 1);
						break;
					}
					return str + ")";
				}) : "";
		}
		#endregion
		#region Syntax tree traversal,  reperrent, init and iterator methods associated with the syntax rule <regular expression list>

		//
		// traverse sub-tree rooted in this node using selected syntax tree walker
		//

		public override void InvokeTraverse (SyntaxTreeWalkerCore syntaxTreeWalker) => ((SyntaxTreeWalker) syntaxTreeWalker).Traverse (this);
		public override void InvokeTraverseCommon (SyntaxTreeWalkerCore syntaxTreeWalker) => ((SyntaxTreeWalker) syntaxTreeWalker).TraverseCommon (this);

		// reparent sub-tree

		public override void reparent (SyntaxTreeBase parent)
		{
			for (_regular_expression_list_ node = this; node != null; node = node.m__regular_expression_list_)
			{
				node.parent = parent;
				node.m__regular_expression_usage_?.reparent (parent);
				parent = node;
			}
		}

		//
		// initialize object associated with syntax rule <regular expression list>
		//

		public void _init ()
		{
			m__regular_expression_usage_ = null;
			m__regular_expression_list_ = null;
		}

		public delegate object IteratorDelegate (_regular_expression_list_ p__regular_expression_list_, object appData);

		public object Iterate (object appData, IteratorDelegate iteratorDelegate)
		{
			_regular_expression_list_ p__regular_expression_list_;
			for (p__regular_expression_list_ = this; p__regular_expression_list_.m__regular_expression_list_ != null; p__regular_expression_list_ = p__regular_expression_list_.m__regular_expression_list_);
			for (SyntaxTreeBase parent = p__regular_expression_list_; (parent != null) && (parent is _regular_expression_list_); parent = parent.parent)
				appData = iteratorDelegate ((_regular_expression_list_) parent, appData);
			return appData;
		}
		#endregion
		#region Fields and properties associated with the syntax rule <regular expression list>

		// counter of all nodes associated with syntax rule <regular expression list>
		public static int g_counter;

		// objects associated with terminal and non-terminal symbols within production(s) of syntax rule <regular expression list>
		public _regular_expression_usage_ m__regular_expression_usage_ { get; private set; }
		public _regular_expression_list_ m__regular_expression_list_ { get; private set; }
		#endregion

	};

	public partial class SyntaxTreeWalker
	{
		#region delegates and events associated with the syntax rule <regular expression list>

		// delegate function (callback) prototype associated with syntax rule <regular expression list>
		public delegate bool _regular_expression_list__Callback (SyntaxTreeCallbackReason reason, _regular_expression_list_.production_kind kind, _regular_expression_list_ p__regular_expression_list_);

		// event associated with syntax rule <regular expression list>
		public event _regular_expression_list__Callback _regular_expression_list__Event;

		// event trigger associated with syntax rule <regular expression list>
		public bool Raise__regular_expression_list__Event (SyntaxTreeCallbackReason reason, _regular_expression_list_.production_kind kind, _regular_expression_list_ p__regular_expression_list_)
		{
			bool? status = _regular_expression_list__Event?.Invoke (reason, kind, p__regular_expression_list_);
			return (status == null) || status.Value;
		}
		#endregion
		#region syntax tree traversal methods associated with the syntax rule <regular expression list>
		//
		// traverse syntax tree node associated with any production of syntax rule <regular expression list>
		//

		public void Traverse (_regular_expression_list_ p__regular_expression_list_)
		{
			if (p__regular_expression_list_.isLocked())
				return;
			if (Raise__regular_expression_list__Event (SyntaxTreeCallbackReason.IterationPrologueCallbackReason, (_regular_expression_list_.production_kind) p__regular_expression_list_.kind, p__regular_expression_list_))
				p__regular_expression_list_.Iterate (null, (node, appData) =>
				{
					if (node.isLocked())
						return null;
					node.dolock();
					_regular_expression_list_.production_kind kind = (_regular_expression_list_.production_kind) node.kind;
					node.turn_reset ();
					if (Raise__regular_expression_list__Event (SyntaxTreeCallbackReason.TraversalPrologueCallbackReason, kind, node))
						switch (kind)
						{
							case _regular_expression_list_.production_kind.g__regular_expression_list__1:
								// traverse syntax tree node associated with production
								// <regular expression list>: <regular expression usage>

								if (Raise__regular_expression_list__Event (SyntaxTreeCallbackReason.TraversalMidTermCallbackReason, kind, node))
									Traverse (node.m__regular_expression_usage_);
								node.turn_inc ();
								Raise__regular_expression_list__Event (SyntaxTreeCallbackReason.TraversalMidTermCallbackReason, kind, node);
							break;
							case _regular_expression_list_.production_kind.g__regular_expression_list__2:
								// traverse syntax tree node associated with production
								// <regular expression list>: <regular expression list> <regular expression usage>

								if (Raise__regular_expression_list__Event (SyntaxTreeCallbackReason.TraversalMidTermCallbackReason, kind, node))
									;
								node.turn_inc ();
								if (Raise__regular_expression_list__Event (SyntaxTreeCallbackReason.TraversalMidTermCallbackReason, kind, node))
									Traverse (node.m__regular_expression_usage_);
								node.turn_inc ();
								Raise__regular_expression_list__Event (SyntaxTreeCallbackReason.TraversalMidTermCallbackReason, kind, node);
							break;
						}
					Raise__regular_expression_list__Event (SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason, kind, node);
					node.unlock();
					return null;
				});
			Raise__regular_expression_list__Event (SyntaxTreeCallbackReason.IterationEpilogueCallbackReason, (_regular_expression_list_.production_kind) p__regular_expression_list_.kind, p__regular_expression_list_);
		}

		//
		// traverse syntax tree node associated with any production of syntax rule <regular expression list>
		//

		public void TraverseCommon (_regular_expression_list_ p__regular_expression_list_)
		{
			if (Raise_Common_Event (SyntaxTreeCallbackReason.IterationPrologueCallbackReason, (int) p__regular_expression_list_.kind, p__regular_expression_list_))
				p__regular_expression_list_.Iterate (null, (node, appData) =>
				{
					_regular_expression_list_.production_kind kind = (_regular_expression_list_.production_kind) node.kind;
					if (Raise_Common_Event (SyntaxTreeCallbackReason.TraversalPrologueCallbackReason, (int) kind, node))
						switch (kind)
						{
						case _regular_expression_list_.production_kind.g__regular_expression_list__1:
							// traverse syntax tree node associated with production
							// <regular expression list>: <regular expression usage>

							TraverseCommon (node.m__regular_expression_usage_);
							break;
						case _regular_expression_list_.production_kind.g__regular_expression_list__2:
							// traverse syntax tree node associated with production
							// <regular expression list>: <regular expression list> <regular expression usage>

							TraverseCommon (node.m__regular_expression_usage_);
							break;
						}
					Raise_Common_Event (SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason, (int) kind, node);
					return null;
				});
			Raise_Common_Event (SyntaxTreeCallbackReason.IterationEpilogueCallbackReason, (int) p__regular_expression_list_.kind, p__regular_expression_list_);
		}
	};
		#endregion

	public partial class SyntaxTreeBuilder : SyntaxTreeWalker
	{
		#region syntax tree builder methods associated with the syntax rule <regular expression list>

		//
		// create syntax tree node associated with production
		// <regular expression list>: <regular expression usage>

		//

		public _regular_expression_list_ _regular_expression_list__1 (_regular_expression_usage_ p__regular_expression_usage_)
		{
			_regular_expression_list_ p__regular_expression_list__ref = new _regular_expression_list_(p__regular_expression_usage_);
			p__regular_expression_usage_.parent = p__regular_expression_list__ref;
			Raise__regular_expression_list__Event (SyntaxTreeCallbackReason.BuilderCallbackReason, _regular_expression_list_.production_kind.g__regular_expression_list__1, p__regular_expression_list__ref);
			return p__regular_expression_list__ref;
		}

		//
		// create syntax tree node associated with production
		// <regular expression list>: <regular expression list> <regular expression usage>

		//

		public _regular_expression_list_ _regular_expression_list__2 (_regular_expression_list_ p__regular_expression_list_, _regular_expression_usage_ p__regular_expression_usage_)
		{
			_regular_expression_list_ p__regular_expression_list__ref = new _regular_expression_list_(p__regular_expression_list_, p__regular_expression_usage_);
			p__regular_expression_list_.parent = p__regular_expression_list__ref;
			p__regular_expression_usage_.parent = p__regular_expression_list__ref;
			Raise__regular_expression_list__Event (SyntaxTreeCallbackReason.BuilderCallbackReason, _regular_expression_list_.production_kind.g__regular_expression_list__2, p__regular_expression_list__ref);
			return p__regular_expression_list__ref;
		}
		#endregion
	};
}
