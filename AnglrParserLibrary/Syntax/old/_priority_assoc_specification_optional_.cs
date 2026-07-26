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
	// class associated with syntax rule <priority assoc specification optional>
	//

	public class	_priority_assoc_specification_optional_ : SyntaxTreeBase
	{
		#region enumerated production(s) of syntax rule <priority assoc specification optional>
		public enum production_kind : ushort	// enumerated production(s) of syntax rule <priority assoc specification optional>
		{
			g__priority_assoc_specification_optional__1 = 1,	// %empty

			g__priority_assoc_specification_optional__2 = 2,	// <priority assoc specification>

		};
		#endregion
		#region production markers associated with the syntax rule <priority assoc specification optional>

		// markers associated with production: <priority assoc specification optional> -> %empty

		public enum production_marker_1 : ushort
		{
			final
		};

		// markers associated with production: <priority assoc specification optional> -> <priority assoc specification>

		public enum production_marker_2 : ushort
		{
			m__priority_assoc_specification_,
			final
		};
		#endregion
		#region Constructors and destructors associated with the syntax rule <priority assoc specification optional>

		// Constructor declaration(s) associated with production(s) of syntax rule <priority assoc specification optional>

		//
		// Constructor associated with the following production(s)
		// <priority assoc specification optional> -> %empty

		//

		public _priority_assoc_specification_optional_ () : base ((uint) ProductionID.__priority_assoc_specification_optional__ID, (uint) production_kind.g__priority_assoc_specification_optional__1)
		{
			++g_counter;
			_init ();
			children = Array.Empty <SyntaxTreeBase> ();
		}

		//
		// Constructor associated with the following production(s)
		// <priority assoc specification optional> -> <priority assoc specification>

		//

		public _priority_assoc_specification_optional_ (_priority_assoc_specification_ p__priority_assoc_specification_) : base ((uint) ProductionID.__priority_assoc_specification_optional__ID, (uint) production_kind.g__priority_assoc_specification_optional__2)
		{
			++g_counter;
			_init ();
			children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 1);
			children[0] = m__priority_assoc_specification_ = p__priority_assoc_specification_;
		}

		// Copy constructor

		public _priority_assoc_specification_optional_ (_priority_assoc_specification_optional_ p__priority_assoc_specification_optional_) : base (p__priority_assoc_specification_optional_.id, p__priority_assoc_specification_optional_.kind)
		{
			++g_counter;
			_init ();
			switch ((production_kind) p__priority_assoc_specification_optional_.kind)
			{
				case production_kind.g__priority_assoc_specification_optional__1:
					children = Array.Empty <SyntaxTreeBase> ();
					break;
				case production_kind.g__priority_assoc_specification_optional__2:
					children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 1);
					if ((children [0] = m__priority_assoc_specification_ = (p__priority_assoc_specification_optional_.m__priority_assoc_specification_ != null) ? new _priority_assoc_specification_ (p__priority_assoc_specification_optional_.m__priority_assoc_specification_) : null) != null) m__priority_assoc_specification_.parent = this;
					break;
				default:
				{
					string[] args = new string[] { "_priority_assoc_specification_optional_" };
					throw new SyntaxTreeError (SyntaxTreeError.InvalidKindError, args);
				}
			}
		}

		// Clone node - wrapper of copy constructor

		public override SyntaxTreeBase Clone () { return (SyntaxTreeBase) new _priority_assoc_specification_optional_ (this); }

		// Destructor

		public new void Dispose ()
		{
			base.Dispose ();
			--g_counter;

			m__priority_assoc_specification_?.Dispose ();

			_init ();
		}
		#endregion
		#region Content changing and inclusion checking methods associated with the syntax rule <priority assoc specification optional>

		// Content changing function(s) associated with production(s) of syntax rule <priority assoc specification optional>

		//
		// Content changing function associated with following production(s)
		// <priority assoc specification optional> -> %empty

		//

		public void change()
		{
			_init ();
			this.kind = (uint) production_kind.g__priority_assoc_specification_optional__1;
			children = Array.Empty <SyntaxTreeBase> ();
			reparent (parent);
		}

		//
		// Content changing function associated with following production(s)
		// <priority assoc specification optional> -> <priority assoc specification>

		//

		public void change(_priority_assoc_specification_ p__priority_assoc_specification_)
		{
			_init ();
			this.kind = (uint) production_kind.g__priority_assoc_specification_optional__2;
			children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 1);
			children [0] = m__priority_assoc_specification_ = p__priority_assoc_specification_;
			reparent (parent);
		}

		//
		// check if node 'element' is included within syntax tree associated with syntax rule <priority assoc specification optional>
		//

		public new bool checkInclusion (SyntaxTreeBase element)
		{
			return
				(element == this) ||
				(m__priority_assoc_specification_ != null) && m__priority_assoc_specification_.checkInclusion (element) ||
				false;
		}
		#endregion
		#region Syntax tree emit methods associated with the syntax rule <priority assoc specification optional>

		//
		// emit production tree node associated with any production of syntax rule <priority assoc specification optional>
		//

		public override string Emit (int depth)
		{
			string s = "";
			if (depth != 0)
			switch ((production_kind) kind)
			{
				case production_kind.g__priority_assoc_specification_optional__1:
					// emit syntax tree node associated with production
					// <priority assoc specification optional>: %empty

				break;
				case production_kind.g__priority_assoc_specification_optional__2:
					// emit syntax tree node associated with production
					// <priority assoc specification optional>: <priority assoc specification>

					s += m__priority_assoc_specification_.Emit (depth - 1);
				break;
			}
			return s.Trim ();
		}

		//
		// emit production tree node associated with any production of syntax rule <priority assoc specification optional>
		//

		public override string EmitProductionTree (int depth)
		{
			string s = "_priority_assoc_specification_optional_ (";
			if (depth != 0)
				switch ((production_kind) kind)
				{
				case production_kind.g__priority_assoc_specification_optional__1:
					// emit syntax tree node associated with production
					// <priority assoc specification optional>: %empty

				break;
				case production_kind.g__priority_assoc_specification_optional__2:
					// emit syntax tree node associated with production
					// <priority assoc specification optional>: <priority assoc specification>

					s += m__priority_assoc_specification_.EmitProductionTree (depth - 1);
				break;
			}
			s += ")";
			return s;
		}
		#endregion
		#region Syntax tree traversal,  reperrent, init and iterator methods associated with the syntax rule <priority assoc specification optional>

		//
		// traverse sub-tree rooted in this node using selected syntax tree walker
		//

		public override void InvokeTraverse (SyntaxTreeWalkerCore syntaxTreeWalker) => ((SyntaxTreeWalker) syntaxTreeWalker).Traverse (this);
		public override void InvokeTraverseCommon (SyntaxTreeWalkerCore syntaxTreeWalker) => ((SyntaxTreeWalker) syntaxTreeWalker).TraverseCommon (this);

		// reparent sub-tree

		public override void reparent (SyntaxTreeBase parent)
		{
			this.parent = parent;

			m__priority_assoc_specification_?.reparent (this);
		}

		//
		// initialize object associated with syntax rule <priority assoc specification optional>
		//

		public void _init ()
		{
			m__priority_assoc_specification_ = null;
		}
		#endregion
		#region Fields and properties associated with the syntax rule <priority assoc specification optional>

		// counter of all nodes associated with syntax rule <priority assoc specification optional>
		public static int g_counter;

		// objects associated with terminal and non-terminal symbols within production(s) of syntax rule <priority assoc specification optional>
		public _priority_assoc_specification_ m__priority_assoc_specification_ { get; private set; }
		#endregion

	};

	public partial class SyntaxTreeWalker
	{
		#region delegates and events associated with the syntax rule <priority assoc specification optional>

		// delegate function (callback) prototype associated with syntax rule <priority assoc specification optional>
		public delegate bool _priority_assoc_specification_optional__Callback (SyntaxTreeCallbackReason reason, _priority_assoc_specification_optional_.production_kind kind, _priority_assoc_specification_optional_ p__priority_assoc_specification_optional_);

		// event associated with syntax rule <priority assoc specification optional>
		public event _priority_assoc_specification_optional__Callback _priority_assoc_specification_optional__Event;

		// event trigger associated with syntax rule <priority assoc specification optional>
		public bool Raise__priority_assoc_specification_optional__Event (SyntaxTreeCallbackReason reason, _priority_assoc_specification_optional_.production_kind kind, _priority_assoc_specification_optional_ p__priority_assoc_specification_optional_)
		{
			bool? status = _priority_assoc_specification_optional__Event?.Invoke (reason, kind, p__priority_assoc_specification_optional_);
			return (status == null) || status.Value;
		}
		#endregion
		#region syntax tree traversal methods associated with the syntax rule <priority assoc specification optional>
		//
		// traverse syntax tree node associated with any production of syntax rule <priority assoc specification optional>
		//

		public void Traverse (_priority_assoc_specification_optional_ p__priority_assoc_specification_optional_)
		{
			if (p__priority_assoc_specification_optional_.isLocked())
				return;
			p__priority_assoc_specification_optional_.dolock();
			_priority_assoc_specification_optional_.production_kind kind = (_priority_assoc_specification_optional_.production_kind) p__priority_assoc_specification_optional_.kind;
			p__priority_assoc_specification_optional_.turn_reset ();
			if (Raise__priority_assoc_specification_optional__Event (SyntaxTreeCallbackReason.TraversalPrologueCallbackReason, kind, p__priority_assoc_specification_optional_))
			switch (kind)
			{
				case _priority_assoc_specification_optional_.production_kind.g__priority_assoc_specification_optional__1:
					// traverse syntax tree node associated with production
					// <priority assoc specification optional>: %empty

				break;
				case _priority_assoc_specification_optional_.production_kind.g__priority_assoc_specification_optional__2:
					// traverse syntax tree node associated with production
					// <priority assoc specification optional>: <priority assoc specification>

					if (Raise__priority_assoc_specification_optional__Event (SyntaxTreeCallbackReason.TraversalMidTermCallbackReason, kind, p__priority_assoc_specification_optional_))
						Traverse (p__priority_assoc_specification_optional_.m__priority_assoc_specification_);
					p__priority_assoc_specification_optional_.turn_inc ();
					Raise__priority_assoc_specification_optional__Event (SyntaxTreeCallbackReason.TraversalMidTermCallbackReason, kind, p__priority_assoc_specification_optional_);
				break;
			}
			Raise__priority_assoc_specification_optional__Event (SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason, kind, p__priority_assoc_specification_optional_);
			p__priority_assoc_specification_optional_.unlock();
		}

		//
		// traverse syntax tree node associated with any production of syntax rule <priority assoc specification optional>
		//

		public void TraverseCommon (_priority_assoc_specification_optional_ p__priority_assoc_specification_optional_)
		{
			_priority_assoc_specification_optional_.production_kind kind = (_priority_assoc_specification_optional_.production_kind) p__priority_assoc_specification_optional_.kind;
			if (Raise_Common_Event (SyntaxTreeCallbackReason.TraversalPrologueCallbackReason, (int) kind, p__priority_assoc_specification_optional_))
			switch (kind)
			{
				case _priority_assoc_specification_optional_.production_kind.g__priority_assoc_specification_optional__1:
					// traverse syntax tree node associated with production
					// <priority assoc specification optional>: %empty

				break;
				case _priority_assoc_specification_optional_.production_kind.g__priority_assoc_specification_optional__2:
					// traverse syntax tree node associated with production
					// <priority assoc specification optional>: <priority assoc specification>

						TraverseCommon (p__priority_assoc_specification_optional_.m__priority_assoc_specification_);
				break;
			}
			Raise_Common_Event (SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason, (int) kind, p__priority_assoc_specification_optional_);
		}
	};
		#endregion

	public partial class SyntaxTreeBuilder : SyntaxTreeWalker
	{
		#region syntax tree builder methods associated with the syntax rule <priority assoc specification optional>

		//
		// create syntax tree node associated with production
		// <priority assoc specification optional>: %empty

		//

		public _priority_assoc_specification_optional_ _priority_assoc_specification_optional__1 ()
		{
			_priority_assoc_specification_optional_ p__priority_assoc_specification_optional__ref = new _priority_assoc_specification_optional_();
			Raise__priority_assoc_specification_optional__Event (SyntaxTreeCallbackReason.BuilderCallbackReason, _priority_assoc_specification_optional_.production_kind.g__priority_assoc_specification_optional__1, p__priority_assoc_specification_optional__ref);
			return p__priority_assoc_specification_optional__ref;
		}

		//
		// create syntax tree node associated with production
		// <priority assoc specification optional>: <priority assoc specification>

		//

		public _priority_assoc_specification_optional_ _priority_assoc_specification_optional__2 (_priority_assoc_specification_ p__priority_assoc_specification_)
		{
			_priority_assoc_specification_optional_ p__priority_assoc_specification_optional__ref = new _priority_assoc_specification_optional_(p__priority_assoc_specification_);
			p__priority_assoc_specification_.parent = p__priority_assoc_specification_optional__ref;
			Raise__priority_assoc_specification_optional__Event (SyntaxTreeCallbackReason.BuilderCallbackReason, _priority_assoc_specification_optional_.production_kind.g__priority_assoc_specification_optional__2, p__priority_assoc_specification_optional__ref);
			return p__priority_assoc_specification_optional__ref;
		}
		#endregion
	};
}
