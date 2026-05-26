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
	// class associated with syntax rule <block terminal definitions>
	//

	public class	_block_terminal_definitions_ : SyntaxTreeBase
	{
		#region enumerated production(s) of syntax rule <block terminal definitions>
		public enum production_kind : ushort	// enumerated production(s) of syntax rule <block terminal definitions>
		{
			g__block_terminal_definitions__1 = 1,	// <block terminal definition>

			g__block_terminal_definitions__2 = 2,	// <block terminal definitions> <block terminal definition>

		};
		#endregion
		#region production markers associated with the syntax rule <block terminal definitions>

		// markers associated with production: <block terminal definitions> -> <block terminal definition>

		public enum production_marker_1 : ushort
		{
			m__block_terminal_definition_,
			final
		};

		// markers associated with production: <block terminal definitions> -> <block terminal definitions> <block terminal definition>

		public enum production_marker_2 : ushort
		{
			m__block_terminal_definitions_,
			m__block_terminal_definition_,
			final
		};
		#endregion
		#region Constructors and destructors associated with the syntax rule <block terminal definitions>

		// Constructor declaration(s) associated with production(s) of syntax rule <block terminal definitions>

		//
		// Constructor associated with the following production(s)
		// <block terminal definitions> -> <block terminal definition>

		//

		public _block_terminal_definitions_ (_block_terminal_definition_ p__block_terminal_definition_) : base ((uint) ProductionID.__block_terminal_definitions__ID, (uint) production_kind.g__block_terminal_definitions__1)
		{
			++g_counter;
			_init ();
			children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 1);
			children[0] = m__block_terminal_definition_ = p__block_terminal_definition_;
		}

		//
		// Constructor associated with the following production(s)
		// <block terminal definitions> -> <block terminal definitions> <block terminal definition>

		//

		public _block_terminal_definitions_ (_block_terminal_definitions_ p__block_terminal_definitions_, _block_terminal_definition_ p__block_terminal_definition_) : base ((uint) ProductionID.__block_terminal_definitions__ID, (uint) production_kind.g__block_terminal_definitions__2)
		{
			++g_counter;
			_init ();
			children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 2);
			children[0] = m__block_terminal_definitions_ = p__block_terminal_definitions_;
			children[1] = m__block_terminal_definition_ = p__block_terminal_definition_;
		}

		// Copy constructor

		public _block_terminal_definitions_ (_block_terminal_definitions_ p__block_terminal_definitions_) : base (p__block_terminal_definitions_.id, p__block_terminal_definitions_.kind)
		{
			++g_counter;
			_init ();
			switch ((production_kind) this.kind)
			{
			case production_kind.g__block_terminal_definitions__1:
				children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 1);
				children[0] = m__block_terminal_definition_ = p__block_terminal_definitions_.m__block_terminal_definition_;
				break;
			case production_kind.g__block_terminal_definitions__2:
				children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 2);
				children[0] = m__block_terminal_definitions_ = p__block_terminal_definitions_.m__block_terminal_definitions_;
				children[1] = m__block_terminal_definition_ = p__block_terminal_definitions_.m__block_terminal_definition_;
				break;
			default:
				string[] args = new string[] { "_block_terminal_definitions_" };
				throw new SyntaxTreeError (SyntaxTreeError.InvalidKindError, args);
			}
		}

		// Clone node - wrapper of copy constructor

		public override SyntaxTreeBase Clone () { return (SyntaxTreeBase) new _block_terminal_definitions_ (this); }

		// Destructor

		public new void Dispose ()
		{
			Iterate (null, (node, appData) =>
			{
				--g_counter;

				node.m__block_terminal_definition_?.Dispose ();

				node._init ();

				return null;
			});
		}
		#endregion
		#region Content changing and inclusion checking methods associated with the syntax rule <block terminal definitions>

		// Content changing function(s) associated with production(s) of syntax rule <block terminal definitions>

		//
		// Content changing function associated with following production(s)
		// <block terminal definitions> -> <block terminal definition>

		//

		public void change(_block_terminal_definition_ p__block_terminal_definition_)
		{
			_init ();
			this.kind = (uint) production_kind.g__block_terminal_definitions__1;
			children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 1);
			children [0] = m__block_terminal_definition_ = p__block_terminal_definition_;
			reparent (parent);
		}

		//
		// Content changing function associated with following production(s)
		// <block terminal definitions> -> <block terminal definitions> <block terminal definition>

		//

		public void change(_block_terminal_definitions_ p__block_terminal_definitions_, _block_terminal_definition_ p__block_terminal_definition_)
		{
			_init ();
			this.kind = (uint) production_kind.g__block_terminal_definitions__2;
			children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 2);
			children [0] = m__block_terminal_definitions_ = p__block_terminal_definitions_;
			children [1] = m__block_terminal_definition_ = p__block_terminal_definition_;
			reparent (parent);
		}

		//
		// check if node 'element' is included within syntax tree associated with syntax rule <block terminal definitions>
		//

		public new bool checkInclusion (SyntaxTreeBase element)
		{
			return (element == this) || (bool) Iterate (false, (node, appData) =>
			{
				return
					(bool) appData ||
					(node.m__block_terminal_definition_ != null) && node.m__block_terminal_definition_.checkInclusion (element) ||
					false;
			});
		}
		#endregion
		#region Syntax tree emit methods associated with the syntax rule <block terminal definitions>

		//
		// emit production tree node associated with any production of syntax rule <block terminal definitions>
		//

		public override string Emit (int depth)
		{
			string s = "";
			if (depth != 0)
				Iterate (null, (node, appData) =>
				{
					switch ((production_kind) node.kind)
					{
					case production_kind.g__block_terminal_definitions__1:
						// emit syntax tree node associated with production
						// <block terminal definitions>: <block terminal definition>

						s += " " + node.m__block_terminal_definition_.Emit (depth - 1);
						break;
					case production_kind.g__block_terminal_definitions__2:
						// emit syntax tree node associated with production
						// <block terminal definitions>: <block terminal definitions> <block terminal definition>

						s += " " + node.m__block_terminal_definition_.Emit (depth - 1);
						break;
					}
					return null;
				});
			return s.Trim ();
		}

		//
		// emit production tree node associated with any production of syntax rule <block terminal definitions>
		//

		public override string EmitProductionTree (int depth)
		{
			return (depth != 0) ?
				(string) Iterate ("", (node, appData) =>
				{
					string str = "_block_terminal_definitions_ (" + (string) appData;
					switch ((production_kind) node.kind)
					{
					case production_kind.g__block_terminal_definitions__1:
						// emit syntax tree node associated with production
						// <block terminal definitions>: <block terminal definition>

						str += node.m__block_terminal_definition_.EmitProductionTree (depth - 1);
						break;
					case production_kind.g__block_terminal_definitions__2:
						// emit syntax tree node associated with production
						// <block terminal definitions>: <block terminal definitions> <block terminal definition>

						str += ' ';
						str += node.m__block_terminal_definition_.EmitProductionTree (depth - 1);
						break;
					}
					return str + ")";
				}) : "";
		}
		#endregion
		#region Syntax tree traversal,  reperrent, init and iterator methods associated with the syntax rule <block terminal definitions>

		//
		// traverse sub-tree rooted in this node using selected syntax tree walker
		//

		public override void InvokeTraverse (SyntaxTreeWalkerCore syntaxTreeWalker) => ((SyntaxTreeWalker) syntaxTreeWalker).Traverse (this);
		public override void InvokeTraverseCommon (SyntaxTreeWalkerCore syntaxTreeWalker) => ((SyntaxTreeWalker) syntaxTreeWalker).TraverseCommon (this);

		// reparent sub-tree

		public override void reparent (SyntaxTreeBase parent)
		{
			for (_block_terminal_definitions_ node = this; node != null; node = node.m__block_terminal_definitions_)
			{
				node.parent = parent;
				node.m__block_terminal_definition_?.reparent (parent);
				parent = node;
			}
		}

		//
		// initialize object associated with syntax rule <block terminal definitions>
		//

		public void _init ()
		{
			m__block_terminal_definition_ = null;
			m__block_terminal_definitions_ = null;
		}

		public delegate object IteratorDelegate (_block_terminal_definitions_ p__block_terminal_definitions_, object appData);

		public object Iterate (object appData, IteratorDelegate iteratorDelegate)
		{
			_block_terminal_definitions_ p__block_terminal_definitions_;
			for (p__block_terminal_definitions_ = this; p__block_terminal_definitions_.m__block_terminal_definitions_ != null; p__block_terminal_definitions_ = p__block_terminal_definitions_.m__block_terminal_definitions_);
			for (SyntaxTreeBase parent = p__block_terminal_definitions_; (parent != null) && (parent is _block_terminal_definitions_); parent = parent.parent)
				appData = iteratorDelegate ((_block_terminal_definitions_) parent, appData);
			return appData;
		}
		#endregion
		#region Fields and properties associated with the syntax rule <block terminal definitions>

		// counter of all nodes associated with syntax rule <block terminal definitions>
		public static int g_counter;

		// objects associated with terminal and non-terminal symbols within production(s) of syntax rule <block terminal definitions>
		public _block_terminal_definition_ m__block_terminal_definition_ { get; private set; }
		public _block_terminal_definitions_ m__block_terminal_definitions_ { get; private set; }
		#endregion

	};

	public partial class SyntaxTreeWalker
	{
		#region delegates and events associated with the syntax rule <block terminal definitions>

		// delegate function (callback) prototype associated with syntax rule <block terminal definitions>
		public delegate bool _block_terminal_definitions__Callback (SyntaxTreeCallbackReason reason, _block_terminal_definitions_.production_kind kind, _block_terminal_definitions_ p__block_terminal_definitions_);

		// event associated with syntax rule <block terminal definitions>
		public event _block_terminal_definitions__Callback _block_terminal_definitions__Event;

		// event trigger associated with syntax rule <block terminal definitions>
		public bool Raise__block_terminal_definitions__Event (SyntaxTreeCallbackReason reason, _block_terminal_definitions_.production_kind kind, _block_terminal_definitions_ p__block_terminal_definitions_)
		{
			bool? status = _block_terminal_definitions__Event?.Invoke (reason, kind, p__block_terminal_definitions_);
			return (status == null) || status.Value;
		}
		#endregion
		#region syntax tree traversal methods associated with the syntax rule <block terminal definitions>
		//
		// traverse syntax tree node associated with any production of syntax rule <block terminal definitions>
		//

		public void Traverse (_block_terminal_definitions_ p__block_terminal_definitions_)
		{
			if (p__block_terminal_definitions_.isLocked())
				return;
			if (Raise__block_terminal_definitions__Event (SyntaxTreeCallbackReason.IterationPrologueCallbackReason, (_block_terminal_definitions_.production_kind) p__block_terminal_definitions_.kind, p__block_terminal_definitions_))
				p__block_terminal_definitions_.Iterate (null, (node, appData) =>
				{
					if (node.isLocked())
						return null;
					node.dolock();
					_block_terminal_definitions_.production_kind kind = (_block_terminal_definitions_.production_kind) node.kind;
					node.turn_reset ();
					if (Raise__block_terminal_definitions__Event (SyntaxTreeCallbackReason.TraversalPrologueCallbackReason, kind, node))
						switch (kind)
						{
							case _block_terminal_definitions_.production_kind.g__block_terminal_definitions__1:
								// traverse syntax tree node associated with production
								// <block terminal definitions>: <block terminal definition>

								if (Raise__block_terminal_definitions__Event (SyntaxTreeCallbackReason.TraversalMidTermCallbackReason, kind, node))
									Traverse (node.m__block_terminal_definition_);
								node.turn_inc ();
								Raise__block_terminal_definitions__Event (SyntaxTreeCallbackReason.TraversalMidTermCallbackReason, kind, node);
							break;
							case _block_terminal_definitions_.production_kind.g__block_terminal_definitions__2:
								// traverse syntax tree node associated with production
								// <block terminal definitions>: <block terminal definitions> <block terminal definition>

								if (Raise__block_terminal_definitions__Event (SyntaxTreeCallbackReason.TraversalMidTermCallbackReason, kind, node))
									;
								node.turn_inc ();
								if (Raise__block_terminal_definitions__Event (SyntaxTreeCallbackReason.TraversalMidTermCallbackReason, kind, node))
									Traverse (node.m__block_terminal_definition_);
								node.turn_inc ();
								Raise__block_terminal_definitions__Event (SyntaxTreeCallbackReason.TraversalMidTermCallbackReason, kind, node);
							break;
						}
					Raise__block_terminal_definitions__Event (SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason, kind, node);
					node.unlock();
					return null;
				});
			Raise__block_terminal_definitions__Event (SyntaxTreeCallbackReason.IterationEpilogueCallbackReason, (_block_terminal_definitions_.production_kind) p__block_terminal_definitions_.kind, p__block_terminal_definitions_);
		}

		//
		// traverse syntax tree node associated with any production of syntax rule <block terminal definitions>
		//

		public void TraverseCommon (_block_terminal_definitions_ p__block_terminal_definitions_)
		{
			if (Raise_Common_Event (SyntaxTreeCallbackReason.IterationPrologueCallbackReason, (int) p__block_terminal_definitions_.kind, p__block_terminal_definitions_))
				p__block_terminal_definitions_.Iterate (null, (node, appData) =>
				{
					_block_terminal_definitions_.production_kind kind = (_block_terminal_definitions_.production_kind) node.kind;
					if (Raise_Common_Event (SyntaxTreeCallbackReason.TraversalPrologueCallbackReason, (int) kind, node))
						switch (kind)
						{
						case _block_terminal_definitions_.production_kind.g__block_terminal_definitions__1:
							// traverse syntax tree node associated with production
							// <block terminal definitions>: <block terminal definition>

							TraverseCommon (node.m__block_terminal_definition_);
							break;
						case _block_terminal_definitions_.production_kind.g__block_terminal_definitions__2:
							// traverse syntax tree node associated with production
							// <block terminal definitions>: <block terminal definitions> <block terminal definition>

							TraverseCommon (node.m__block_terminal_definition_);
							break;
						}
					Raise_Common_Event (SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason, (int) kind, node);
					return null;
				});
			Raise_Common_Event (SyntaxTreeCallbackReason.IterationEpilogueCallbackReason, (int) p__block_terminal_definitions_.kind, p__block_terminal_definitions_);
		}
	};
		#endregion

	public partial class SyntaxTreeBuilder : SyntaxTreeWalker
	{
		#region syntax tree builder methods associated with the syntax rule <block terminal definitions>

		//
		// create syntax tree node associated with production
		// <block terminal definitions>: <block terminal definition>

		//

		public _block_terminal_definitions_ _block_terminal_definitions__1 (_block_terminal_definition_ p__block_terminal_definition_)
		{
			_block_terminal_definitions_ p__block_terminal_definitions__ref = new _block_terminal_definitions_(p__block_terminal_definition_);
			p__block_terminal_definition_.parent = p__block_terminal_definitions__ref;
			Raise__block_terminal_definitions__Event (SyntaxTreeCallbackReason.BuilderCallbackReason, _block_terminal_definitions_.production_kind.g__block_terminal_definitions__1, p__block_terminal_definitions__ref);
			return p__block_terminal_definitions__ref;
		}

		//
		// create syntax tree node associated with production
		// <block terminal definitions>: <block terminal definitions> <block terminal definition>

		//

		public _block_terminal_definitions_ _block_terminal_definitions__2 (_block_terminal_definitions_ p__block_terminal_definitions_, _block_terminal_definition_ p__block_terminal_definition_)
		{
			_block_terminal_definitions_ p__block_terminal_definitions__ref = new _block_terminal_definitions_(p__block_terminal_definitions_, p__block_terminal_definition_);
			p__block_terminal_definitions_.parent = p__block_terminal_definitions__ref;
			p__block_terminal_definition_.parent = p__block_terminal_definitions__ref;
			Raise__block_terminal_definitions__Event (SyntaxTreeCallbackReason.BuilderCallbackReason, _block_terminal_definitions_.production_kind.g__block_terminal_definitions__2, p__block_terminal_definitions__ref);
			return p__block_terminal_definitions__ref;
		}
		#endregion
	};
}
