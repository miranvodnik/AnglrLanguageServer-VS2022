//
//	This file was generated with ANGLR compiler
//
using System;
using System.Runtime.Serialization;
using System.Collections.Generic;

using Anglr.Parser.Core;
using Anglr.Parser.SyntaxTree;
using Anglr.Parser.Walker;
namespace Anglr.Parser
{
	//
	// class associated with syntax rule <classifier named value set>
	//

	[DataContract]
	public class	_classifier_named_value_set_ : SyntaxTreeBase
	{
		#region enumerated production(s) of syntax rule <classifier named value set>
		public enum production_kind : ushort	// enumerated production(s) of syntax rule <classifier named value set>
		{
			g__classifier_named_value_set__1 = 1,	// <classifier named value>

			g__classifier_named_value_set__2 = 2,	// <classifier named value set> <classifier named value>

		};
		#endregion
		#region production markers associated with the syntax rule <classifier named value set>

		// markers associated with production: <classifier named value set> -> <classifier named value>

		public enum production_marker_1 : ushort
		{
			m__classifier_named_value_,
			final
		};

		// markers associated with production: <classifier named value set> -> <classifier named value set> <classifier named value>

		public enum production_marker_2 : ushort
		{
			m__classifier_named_value_set_,
			m__classifier_named_value_,
			final
		};
		#endregion
		#region Constructors and destructors associated with the syntax rule <classifier named value set>

		// Constructor declaration(s) associated with production(s) of syntax rule <classifier named value set>

		//
		// Constructor associated with the following production(s)
		// <classifier named value set> -> <classifier named value>

		//

		public _classifier_named_value_set_ (_classifier_named_value_ p__classifier_named_value_) : base ((uint) ProductionID.__classifier_named_value_set__ID, (uint) production_kind.g__classifier_named_value_set__1)
		{
			++g_counter;
			_init ();
			children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 1);
			children[0] = m__classifier_named_value_ = p__classifier_named_value_;
		}

		//
		// Constructor associated with the following production(s)
		// <classifier named value set> -> <classifier named value set> <classifier named value>

		//

		public _classifier_named_value_set_ (_classifier_named_value_set_ p__classifier_named_value_set_, _classifier_named_value_ p__classifier_named_value_) : base ((uint) ProductionID.__classifier_named_value_set__ID, (uint) production_kind.g__classifier_named_value_set__2)
		{
			++g_counter;
			_init ();
			children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 2);
			children[0] = m__classifier_named_value_set_ = p__classifier_named_value_set_;
			children[1] = m__classifier_named_value_ = p__classifier_named_value_;
		}

		public _classifier_named_value_set_ () { }

		// Copy constructor

		public _classifier_named_value_set_ (_classifier_named_value_set_ p__classifier_named_value_set_) : base (p__classifier_named_value_set_.id, p__classifier_named_value_set_.kind)
		{
			++g_counter;
			_init ();
			appInfo = p__classifier_named_value_set_.appInfo;
			switch ((production_kind) p__classifier_named_value_set_.kind)
			{
				case production_kind.g__classifier_named_value_set__1:
					children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 1);
					if ((children [0] = m__classifier_named_value_ = (p__classifier_named_value_set_.m__classifier_named_value_ != null) ? new _classifier_named_value_ (p__classifier_named_value_set_.m__classifier_named_value_) : null) != null) m__classifier_named_value_.parent = this;
					break;
				case production_kind.g__classifier_named_value_set__2:
					children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 2);
					if ((children [0] = m__classifier_named_value_set_ = (p__classifier_named_value_set_.m__classifier_named_value_set_ != null) ? new _classifier_named_value_set_ (p__classifier_named_value_set_.m__classifier_named_value_set_) : null) != null) m__classifier_named_value_set_.parent = this;
					if ((children [1] = m__classifier_named_value_ = (p__classifier_named_value_set_.m__classifier_named_value_ != null) ? new _classifier_named_value_ (p__classifier_named_value_set_.m__classifier_named_value_) : null) != null) m__classifier_named_value_.parent = this;
					break;
				default:
				{
					string[] args = new string[] { "_classifier_named_value_set_" };
					throw new SyntaxTreeError (SyntaxTreeError.InvalidKindError, args);
				}
			}
		}

		// Clone node - wrapper of copy constructor

		public override SyntaxTreeBase Clone () { return (SyntaxTreeBase) new _classifier_named_value_set_ (this); }

		// Destructor

		public new void Dispose ()
		{
			Iterate (null, (node, appData) =>
			{
				--g_counter;

				node.m__classifier_named_value_?.Dispose ();

				node._init ();

				return null;
			});
		}
		#endregion
		#region Content changing and inclusion checking methods associated with the syntax rule <classifier named value set>

		// Content changing function(s) associated with production(s) of syntax rule <classifier named value set>

		//
		// Content changing function associated with following production(s)
		// <classifier named value set> -> <classifier named value>

		//

		public void change(_classifier_named_value_ p__classifier_named_value_)
		{
			_init ();
			this.kind = (uint) production_kind.g__classifier_named_value_set__1;
			children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 1);
			children [0] = m__classifier_named_value_ = p__classifier_named_value_;
			reparent (parent);
		}

		//
		// Content changing function associated with following production(s)
		// <classifier named value set> -> <classifier named value set> <classifier named value>

		//

		public void change(_classifier_named_value_set_ p__classifier_named_value_set_, _classifier_named_value_ p__classifier_named_value_)
		{
			_init ();
			this.kind = (uint) production_kind.g__classifier_named_value_set__2;
			children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 2);
			children [0] = m__classifier_named_value_set_ = p__classifier_named_value_set_;
			children [1] = m__classifier_named_value_ = p__classifier_named_value_;
			reparent (parent);
		}

		//
		// check if node 'element' is included within syntax tree associated with syntax rule <classifier named value set>
		//

		public new bool checkInclusion (SyntaxTreeBase element)
		{
			return (element == this) || (bool) Iterate (false, (node, appData) =>
			{
				return
					(bool) appData ||
					(node.m__classifier_named_value_ != null) && node.m__classifier_named_value_.checkInclusion (element) ||
					false;
			});
		}
		#endregion
		#region Syntax tree emit methods associated with the syntax rule <classifier named value set>

		//
		// emit production tree node associated with any production of syntax rule <classifier named value set>
		//

		public override string Emit (int depth)
		{
			string s = "";
			if (depth != 0)
				Iterate (null, (node, appData) =>
				{
					switch ((production_kind) node.kind)
					{
					case production_kind.g__classifier_named_value_set__1:
						// emit syntax tree node associated with production
						// <classifier named value set>: <classifier named value>

						s += " " + node.m__classifier_named_value_.Emit (depth - 1);
						break;
					case production_kind.g__classifier_named_value_set__2:
						// emit syntax tree node associated with production
						// <classifier named value set>: <classifier named value set> <classifier named value>

						s += " " + node.m__classifier_named_value_.Emit (depth - 1);
						break;
					}
					return null;
				});
			return s.Trim ();
		}

		//
		// emit production tree node associated with any production of syntax rule <classifier named value set>
		//

		public override string EmitProductionTree (int depth)
		{
			return (depth != 0) ?
				(string) Iterate ("", (node, appData) =>
				{
					string str = "_classifier_named_value_set_ (" + (string) appData;
					switch ((production_kind) node.kind)
					{
					case production_kind.g__classifier_named_value_set__1:
						// emit syntax tree node associated with production
						// <classifier named value set>: <classifier named value>

						str += node.m__classifier_named_value_.EmitProductionTree (depth - 1);
						break;
					case production_kind.g__classifier_named_value_set__2:
						// emit syntax tree node associated with production
						// <classifier named value set>: <classifier named value set> <classifier named value>

						str += ' ';
						str += node.m__classifier_named_value_.EmitProductionTree (depth - 1);
						break;
					}
					return str + ")";
				}) : "";
		}
		#endregion
		#region Syntax tree traversal,  reperrent, init and iterator methods associated with the syntax rule <classifier named value set>

		//
		// traverse sub-tree rooted in this node using selected syntax tree walker
		//

		public override void InvokeTraverse (SyntaxTreeWalkerCore syntaxTreeWalker) => ((SyntaxTreeWalker) syntaxTreeWalker).Traverse (this);
		public override void InvokeTraverseCommon (SyntaxTreeWalkerCore syntaxTreeWalker) => ((SyntaxTreeWalker) syntaxTreeWalker).TraverseCommon (this);

		//
		// traverse syntax tree node associated with any production of syntax rule <classifier named value set> with specified delegate
		//

		public override void Visit (Func<SyntaxTreeCallbackReason, int, SyntaxTreeBase, object, bool> visitor, object appData)
		{
			if (visitor (SyntaxTreeCallbackReason.IterationPrologueCallbackReason, (int) this.kind, this, appData))
				Iterate (null, (node, data) =>
				{
					_classifier_named_value_set_.production_kind kind = (_classifier_named_value_set_.production_kind) node.kind;
					if (visitor (SyntaxTreeCallbackReason.TraversalPrologueCallbackReason, (int) kind, node, appData))
						switch (kind)
						{
						case _classifier_named_value_set_.production_kind.g__classifier_named_value_set__1:
							// traverse syntax tree node associated with production
							// <classifier named value set>: <classifier named value>

							node.m__classifier_named_value_.Visit (visitor, appData);
							break;
						case _classifier_named_value_set_.production_kind.g__classifier_named_value_set__2:
							// traverse syntax tree node associated with production
							// <classifier named value set>: <classifier named value set> <classifier named value>

							node.m__classifier_named_value_.Visit (visitor, appData);
							break;
						}
					visitor (SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason, (int) kind, node, appData);
					return null;
				});
			visitor (SyntaxTreeCallbackReason.IterationEpilogueCallbackReason, (int) this.kind, this, appData);
		}

		// reparent sub-tree

		public override void reparent (SyntaxTreeBase parent)
		{
			for (_classifier_named_value_set_ node = this; node != null; node = node.m__classifier_named_value_set_)
			{
				node.parent = parent;
				node.m__classifier_named_value_?.reparent (parent);
				parent = node;
			}
		}

		//
		// initialize object associated with syntax rule <classifier named value set>
		//

		public void _init ()
		{
			m__classifier_named_value_ = null;
			m__classifier_named_value_set_ = null;
		}

		public delegate object IteratorDelegate (_classifier_named_value_set_ p__classifier_named_value_set_, object appData);

		public object Iterate (object appData, IteratorDelegate iteratorDelegate)
		{
			Stack<_classifier_named_value_set_> stack = new Stack<_classifier_named_value_set_> ();
			_classifier_named_value_set_ p__classifier_named_value_set_;
			for (p__classifier_named_value_set_ = this; p__classifier_named_value_set_ != null; p__classifier_named_value_set_ = p__classifier_named_value_set_.m__classifier_named_value_set_)
				stack.Push (p__classifier_named_value_set_);
			foreach (var node in stack)
				appData = iteratorDelegate (node, appData);
			return appData;
		}
		#endregion
		#region Fields and properties associated with the syntax rule <classifier named value set>

		// counter of all nodes associated with syntax rule <classifier named value set>
		public static int g_counter;

		// objects associated with terminal and non-terminal symbols within production(s) of syntax rule <classifier named value set>
		[DataMember (Name = "m__classifier_named_value_")] public _classifier_named_value_ m__classifier_named_value_ { get; private set; }
		[DataMember (Name = "m__classifier_named_value_set_")] public _classifier_named_value_set_ m__classifier_named_value_set_ { get; private set; }
		#endregion

	};

	public partial class SyntaxTreeWalker
	{
		#region delegates and events associated with the syntax rule <classifier named value set>

		// delegate function (callback) prototype associated with syntax rule <classifier named value set>
		public delegate bool _classifier_named_value_set__Callback (SyntaxTreeCallbackReason reason, _classifier_named_value_set_.production_kind kind, _classifier_named_value_set_ p__classifier_named_value_set_);

		// event associated with syntax rule <classifier named value set>
		public event _classifier_named_value_set__Callback _classifier_named_value_set__Event;

		// event trigger associated with syntax rule <classifier named value set>
		public bool Raise__classifier_named_value_set__Event (SyntaxTreeCallbackReason reason, _classifier_named_value_set_.production_kind kind, _classifier_named_value_set_ p__classifier_named_value_set_)
		{
			bool? status = _classifier_named_value_set__Event?.Invoke (reason, kind, p__classifier_named_value_set_);
			return (status == null) || status.Value;
		}
		#endregion
		#region syntax tree traversal methods associated with the syntax rule <classifier named value set>
		//
		// traverse syntax tree node associated with any production of syntax rule <classifier named value set>
		//

		public void Traverse (_classifier_named_value_set_ p__classifier_named_value_set_)
		{
			if (p__classifier_named_value_set_.isLocked())
				return;
			if (Raise__classifier_named_value_set__Event (SyntaxTreeCallbackReason.IterationPrologueCallbackReason, (_classifier_named_value_set_.production_kind) p__classifier_named_value_set_.kind, p__classifier_named_value_set_))
				p__classifier_named_value_set_.Iterate (null, (node, appData) =>
				{
					if (node.isLocked())
						return null;
					node.dolock();
					_classifier_named_value_set_.production_kind kind = (_classifier_named_value_set_.production_kind) node.kind;
					node.turn_reset ();
					if (Raise__classifier_named_value_set__Event (SyntaxTreeCallbackReason.TraversalPrologueCallbackReason, kind, node))
						switch (kind)
						{
							case _classifier_named_value_set_.production_kind.g__classifier_named_value_set__1:
								// traverse syntax tree node associated with production
								// <classifier named value set>: <classifier named value>

								if (Raise__classifier_named_value_set__Event (SyntaxTreeCallbackReason.TraversalMidTermCallbackReason, kind, node))
									Traverse (node.m__classifier_named_value_);
								node.turn_inc ();
								Raise__classifier_named_value_set__Event (SyntaxTreeCallbackReason.TraversalMidTermCallbackReason, kind, node);
							break;
							case _classifier_named_value_set_.production_kind.g__classifier_named_value_set__2:
								// traverse syntax tree node associated with production
								// <classifier named value set>: <classifier named value set> <classifier named value>

								if (Raise__classifier_named_value_set__Event (SyntaxTreeCallbackReason.TraversalMidTermCallbackReason, kind, node))
									;
								node.turn_inc ();
								if (Raise__classifier_named_value_set__Event (SyntaxTreeCallbackReason.TraversalMidTermCallbackReason, kind, node))
									Traverse (node.m__classifier_named_value_);
								node.turn_inc ();
								Raise__classifier_named_value_set__Event (SyntaxTreeCallbackReason.TraversalMidTermCallbackReason, kind, node);
							break;
						}
					Raise__classifier_named_value_set__Event (SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason, kind, node);
					node.unlock();
					return null;
				});
			Raise__classifier_named_value_set__Event (SyntaxTreeCallbackReason.IterationEpilogueCallbackReason, (_classifier_named_value_set_.production_kind) p__classifier_named_value_set_.kind, p__classifier_named_value_set_);
		}

		//
		// traverse syntax tree node associated with any production of syntax rule <classifier named value set>
		//

		public void TraverseCommon (_classifier_named_value_set_ p__classifier_named_value_set_)
		{
			if (Raise_Common_Event (SyntaxTreeCallbackReason.IterationPrologueCallbackReason, (int) p__classifier_named_value_set_.kind, p__classifier_named_value_set_))
				p__classifier_named_value_set_.Iterate (null, (node, appData) =>
				{
					_classifier_named_value_set_.production_kind kind = (_classifier_named_value_set_.production_kind) node.kind;
					if (Raise_Common_Event (SyntaxTreeCallbackReason.TraversalPrologueCallbackReason, (int) kind, node))
						switch (kind)
						{
						case _classifier_named_value_set_.production_kind.g__classifier_named_value_set__1:
							// traverse syntax tree node associated with production
							// <classifier named value set>: <classifier named value>

							TraverseCommon (node.m__classifier_named_value_);
							break;
						case _classifier_named_value_set_.production_kind.g__classifier_named_value_set__2:
							// traverse syntax tree node associated with production
							// <classifier named value set>: <classifier named value set> <classifier named value>

							TraverseCommon (node.m__classifier_named_value_);
							break;
						}
					Raise_Common_Event (SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason, (int) kind, node);
					return null;
				});
			Raise_Common_Event (SyntaxTreeCallbackReason.IterationEpilogueCallbackReason, (int) p__classifier_named_value_set_.kind, p__classifier_named_value_set_);
		}
	};
		#endregion

	public partial class SyntaxTreeBuilder : SyntaxTreeWalker
	{
		#region syntax tree builder methods associated with the syntax rule <classifier named value set>

		//
		// create syntax tree node associated with production
		// <classifier named value set>: <classifier named value>

		//

		public _classifier_named_value_set_ _classifier_named_value_set__1 (_classifier_named_value_ p__classifier_named_value_)
		{
			_classifier_named_value_set_ p__classifier_named_value_set__ref = new _classifier_named_value_set_(p__classifier_named_value_);
			p__classifier_named_value_.parent = p__classifier_named_value_set__ref;
			Raise__classifier_named_value_set__Event (SyntaxTreeCallbackReason.BuilderCallbackReason, _classifier_named_value_set_.production_kind.g__classifier_named_value_set__1, p__classifier_named_value_set__ref);
			return p__classifier_named_value_set__ref;
		}

		//
		// create syntax tree node associated with production
		// <classifier named value set>: <classifier named value set> <classifier named value>

		//

		public _classifier_named_value_set_ _classifier_named_value_set__2 (_classifier_named_value_set_ p__classifier_named_value_set_, _classifier_named_value_ p__classifier_named_value_)
		{
			_classifier_named_value_set_ p__classifier_named_value_set__ref = new _classifier_named_value_set_(p__classifier_named_value_set_, p__classifier_named_value_);
			p__classifier_named_value_set_.parent = p__classifier_named_value_set__ref;
			p__classifier_named_value_.parent = p__classifier_named_value_set__ref;
			Raise__classifier_named_value_set__Event (SyntaxTreeCallbackReason.BuilderCallbackReason, _classifier_named_value_set_.production_kind.g__classifier_named_value_set__2, p__classifier_named_value_set__ref);
			return p__classifier_named_value_set__ref;
		}
		#endregion
	};
}
