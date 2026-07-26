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
	// class associated with syntax rule <priority assoc specification>
	//

	public class	_priority_assoc_specification_ : SyntaxTreeBase
	{
		#region enumerated production(s) of syntax rule <priority assoc specification>
		public enum production_kind : ushort	// enumerated production(s) of syntax rule <priority assoc specification>
		{
			g__priority_assoc_specification__1 = 1,	// <priority specification> <associativity specification>

			g__priority_assoc_specification__2 = 2,	// <associativity specification> <priority specification>

			g__priority_assoc_specification__3 = 3,	// <priority specification>

			g__priority_assoc_specification__4 = 4,	// <associativity specification>

			g__priority_assoc_specification__5 = 5,	// <priority specification> ',' <associativity specification>

			g__priority_assoc_specification__6 = 6,	// <associativity specification> ',' <priority specification>

		};
		#endregion
		#region production markers associated with the syntax rule <priority assoc specification>

		// markers associated with production: <priority assoc specification> -> <priority specification> <associativity specification>

		public enum production_marker_1 : ushort
		{
			m__priority_specification_,
			m__associativity_specification_,
			final
		};

		// markers associated with production: <priority assoc specification> -> <associativity specification> <priority specification>

		public enum production_marker_2 : ushort
		{
			m__associativity_specification_,
			m__priority_specification_,
			final
		};

		// markers associated with production: <priority assoc specification> -> <priority specification>

		public enum production_marker_3 : ushort
		{
			m__priority_specification_,
			final
		};

		// markers associated with production: <priority assoc specification> -> <associativity specification>

		public enum production_marker_4 : ushort
		{
			m__associativity_specification_,
			final
		};

		// markers associated with production: <priority assoc specification> -> <priority specification> ',' <associativity specification>

		public enum production_marker_5 : ushort
		{
			m__priority_specification_,
			m__associativity_specification_,
			final
		};

		// markers associated with production: <priority assoc specification> -> <associativity specification> ',' <priority specification>

		public enum production_marker_6 : ushort
		{
			m__associativity_specification_,
			m__priority_specification_,
			final
		};
		#endregion
		#region Constructors and destructors associated with the syntax rule <priority assoc specification>

		// Constructor declaration(s) associated with production(s) of syntax rule <priority assoc specification>

		//
		// Constructor associated with the following production(s)
		// <priority assoc specification> -> <priority specification> <associativity specification>

		//

		public _priority_assoc_specification_ (_priority_specification_ p__priority_specification_, _associativity_specification_ p__associativity_specification_) : base ((uint) ProductionID.__priority_assoc_specification__ID, (uint) production_kind.g__priority_assoc_specification__1)
		{
			++g_counter;
			_init ();
			children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 2);
			children[0] = m__priority_specification_ = p__priority_specification_;
			children[1] = m__associativity_specification_ = p__associativity_specification_;
		}

		//
		// Constructor associated with the following production(s)
		// <priority assoc specification> -> <associativity specification> <priority specification>

		//

		public _priority_assoc_specification_ (_associativity_specification_ p__associativity_specification_, _priority_specification_ p__priority_specification_) : base ((uint) ProductionID.__priority_assoc_specification__ID, (uint) production_kind.g__priority_assoc_specification__2)
		{
			++g_counter;
			_init ();
			children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 2);
			children[0] = m__associativity_specification_ = p__associativity_specification_;
			children[1] = m__priority_specification_ = p__priority_specification_;
		}

		//
		// Constructor associated with the following production(s)
		// <priority assoc specification> -> <priority specification>

		//

		public _priority_assoc_specification_ (_priority_specification_ p__priority_specification_) : base ((uint) ProductionID.__priority_assoc_specification__ID, (uint) production_kind.g__priority_assoc_specification__3)
		{
			++g_counter;
			_init ();
			children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 1);
			children[0] = m__priority_specification_ = p__priority_specification_;
		}

		//
		// Constructor associated with the following production(s)
		// <priority assoc specification> -> <associativity specification>

		//

		public _priority_assoc_specification_ (_associativity_specification_ p__associativity_specification_) : base ((uint) ProductionID.__priority_assoc_specification__ID, (uint) production_kind.g__priority_assoc_specification__4)
		{
			++g_counter;
			_init ();
			children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 1);
			children[0] = m__associativity_specification_ = p__associativity_specification_;
		}

		//
		// Constructor associated with the following production(s)
		// <priority assoc specification> -> <priority specification> ',' <associativity specification>

		//

		public _priority_assoc_specification_ (_priority_specification_ p__priority_specification_, SyntaxTreeToken p_token, _associativity_specification_ p__associativity_specification_) : base ((uint) ProductionID.__priority_assoc_specification__ID, (uint) production_kind.g__priority_assoc_specification__5)
		{
			++g_counter;
			_init ();
			children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 3);
			children[0] = m__priority_specification_ = p__priority_specification_;
			children[1] = m__comma_ = p_token;
			children[2] = m__associativity_specification_ = p__associativity_specification_;
		}

		//
		// Constructor associated with the following production(s)
		// <priority assoc specification> -> <associativity specification> ',' <priority specification>

		//

		public _priority_assoc_specification_ (_associativity_specification_ p__associativity_specification_, SyntaxTreeToken p_token, _priority_specification_ p__priority_specification_) : base ((uint) ProductionID.__priority_assoc_specification__ID, (uint) production_kind.g__priority_assoc_specification__6)
		{
			++g_counter;
			_init ();
			children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 3);
			children[0] = m__associativity_specification_ = p__associativity_specification_;
			children[1] = m__comma_ = p_token;
			children[2] = m__priority_specification_ = p__priority_specification_;
		}

		// Copy constructor

		public _priority_assoc_specification_ (_priority_assoc_specification_ p__priority_assoc_specification_) : base (p__priority_assoc_specification_.id, p__priority_assoc_specification_.kind)
		{
			++g_counter;
			_init ();
			switch ((production_kind) p__priority_assoc_specification_.kind)
			{
				case production_kind.g__priority_assoc_specification__1:
					children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 2);
					if ((children [0] = m__priority_specification_ = (p__priority_assoc_specification_.m__priority_specification_ != null) ? new _priority_specification_ (p__priority_assoc_specification_.m__priority_specification_) : null) != null) m__priority_specification_.parent = this;
					if ((children [1] = m__associativity_specification_ = (p__priority_assoc_specification_.m__associativity_specification_ != null) ? new _associativity_specification_ (p__priority_assoc_specification_.m__associativity_specification_) : null) != null) m__associativity_specification_.parent = this;
					break;
				case production_kind.g__priority_assoc_specification__2:
					children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 2);
					if ((children [0] = m__associativity_specification_ = (p__priority_assoc_specification_.m__associativity_specification_ != null) ? new _associativity_specification_ (p__priority_assoc_specification_.m__associativity_specification_) : null) != null) m__associativity_specification_.parent = this;
					if ((children [1] = m__priority_specification_ = (p__priority_assoc_specification_.m__priority_specification_ != null) ? new _priority_specification_ (p__priority_assoc_specification_.m__priority_specification_) : null) != null) m__priority_specification_.parent = this;
					break;
				case production_kind.g__priority_assoc_specification__3:
					children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 1);
					if ((children [0] = m__priority_specification_ = (p__priority_assoc_specification_.m__priority_specification_ != null) ? new _priority_specification_ (p__priority_assoc_specification_.m__priority_specification_) : null) != null) m__priority_specification_.parent = this;
					break;
				case production_kind.g__priority_assoc_specification__4:
					children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 1);
					if ((children [0] = m__associativity_specification_ = (p__priority_assoc_specification_.m__associativity_specification_ != null) ? new _associativity_specification_ (p__priority_assoc_specification_.m__associativity_specification_) : null) != null) m__associativity_specification_.parent = this;
					break;
				case production_kind.g__priority_assoc_specification__5:
					children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 3);
					if ((children [0] = m__priority_specification_ = (p__priority_assoc_specification_.m__priority_specification_ != null) ? new _priority_specification_ (p__priority_assoc_specification_.m__priority_specification_) : null) != null) m__priority_specification_.parent = this;
					if ((children [1] = m__comma_ = (p__priority_assoc_specification_.m__comma_ != null) ? new SyntaxTreeToken (p__priority_assoc_specification_.m__comma_) : null) != null) m__comma_.parent = this;
					if ((children [2] = m__associativity_specification_ = (p__priority_assoc_specification_.m__associativity_specification_ != null) ? new _associativity_specification_ (p__priority_assoc_specification_.m__associativity_specification_) : null) != null) m__associativity_specification_.parent = this;
					break;
				case production_kind.g__priority_assoc_specification__6:
					children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 3);
					if ((children [0] = m__associativity_specification_ = (p__priority_assoc_specification_.m__associativity_specification_ != null) ? new _associativity_specification_ (p__priority_assoc_specification_.m__associativity_specification_) : null) != null) m__associativity_specification_.parent = this;
					if ((children [1] = m__comma_ = (p__priority_assoc_specification_.m__comma_ != null) ? new SyntaxTreeToken (p__priority_assoc_specification_.m__comma_) : null) != null) m__comma_.parent = this;
					if ((children [2] = m__priority_specification_ = (p__priority_assoc_specification_.m__priority_specification_ != null) ? new _priority_specification_ (p__priority_assoc_specification_.m__priority_specification_) : null) != null) m__priority_specification_.parent = this;
					break;
				default:
				{
					string[] args = new string[] { "_priority_assoc_specification_" };
					throw new SyntaxTreeError (SyntaxTreeError.InvalidKindError, args);
				}
			}
		}

		// Clone node - wrapper of copy constructor

		public override SyntaxTreeBase Clone () { return (SyntaxTreeBase) new _priority_assoc_specification_ (this); }

		// Destructor

		public new void Dispose ()
		{
			base.Dispose ();
			--g_counter;

			m__priority_specification_?.Dispose ();
			m__associativity_specification_?.Dispose ();
			m__comma_?.Dispose ();

			_init ();
		}
		#endregion
		#region Content changing and inclusion checking methods associated with the syntax rule <priority assoc specification>

		// Content changing function(s) associated with production(s) of syntax rule <priority assoc specification>

		//
		// Content changing function associated with following production(s)
		// <priority assoc specification> -> <priority specification> <associativity specification>

		//

		public void change(_priority_specification_ p__priority_specification_, _associativity_specification_ p__associativity_specification_)
		{
			_init ();
			this.kind = (uint) production_kind.g__priority_assoc_specification__1;
			children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 2);
			children [0] = m__priority_specification_ = p__priority_specification_;
			children [1] = m__associativity_specification_ = p__associativity_specification_;
			reparent (parent);
		}

		//
		// Content changing function associated with following production(s)
		// <priority assoc specification> -> <associativity specification> <priority specification>

		//

		public void change(_associativity_specification_ p__associativity_specification_, _priority_specification_ p__priority_specification_)
		{
			_init ();
			this.kind = (uint) production_kind.g__priority_assoc_specification__2;
			children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 2);
			children [0] = m__associativity_specification_ = p__associativity_specification_;
			children [1] = m__priority_specification_ = p__priority_specification_;
			reparent (parent);
		}

		//
		// Content changing function associated with following production(s)
		// <priority assoc specification> -> <priority specification>

		//

		public void change(_priority_specification_ p__priority_specification_)
		{
			_init ();
			this.kind = (uint) production_kind.g__priority_assoc_specification__3;
			children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 1);
			children [0] = m__priority_specification_ = p__priority_specification_;
			reparent (parent);
		}

		//
		// Content changing function associated with following production(s)
		// <priority assoc specification> -> <associativity specification>

		//

		public void change(_associativity_specification_ p__associativity_specification_)
		{
			_init ();
			this.kind = (uint) production_kind.g__priority_assoc_specification__4;
			children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 1);
			children [0] = m__associativity_specification_ = p__associativity_specification_;
			reparent (parent);
		}

		//
		// Content changing function associated with following production(s)
		// <priority assoc specification> -> <priority specification> ',' <associativity specification>

		//

		public void change(_priority_specification_ p__priority_specification_, SyntaxTreeToken p_token, _associativity_specification_ p__associativity_specification_)
		{
			_init ();
			this.kind = (uint) production_kind.g__priority_assoc_specification__5;
			children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 3);
			children [0] = m__priority_specification_ = p__priority_specification_;
			children [1] = m__comma_ = p_token;
			children [2] = m__associativity_specification_ = p__associativity_specification_;
			reparent (parent);
		}

		//
		// Content changing function associated with following production(s)
		// <priority assoc specification> -> <associativity specification> ',' <priority specification>

		//

		public void change(_associativity_specification_ p__associativity_specification_, SyntaxTreeToken p_token, _priority_specification_ p__priority_specification_)
		{
			_init ();
			this.kind = (uint) production_kind.g__priority_assoc_specification__6;
			children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 3);
			children [0] = m__associativity_specification_ = p__associativity_specification_;
			children [1] = m__comma_ = p_token;
			children [2] = m__priority_specification_ = p__priority_specification_;
			reparent (parent);
		}

		//
		// check if node 'element' is included within syntax tree associated with syntax rule <priority assoc specification>
		//

		public new bool checkInclusion (SyntaxTreeBase element)
		{
			return
				(element == this) ||
				(m__priority_specification_ != null) && m__priority_specification_.checkInclusion (element) ||
				(m__associativity_specification_ != null) && m__associativity_specification_.checkInclusion (element) ||
				(m__comma_ != null) && m__comma_.checkInclusion (element) ||
				false;
		}
		#endregion
		#region Syntax tree emit methods associated with the syntax rule <priority assoc specification>

		//
		// emit production tree node associated with any production of syntax rule <priority assoc specification>
		//

		public override string Emit (int depth)
		{
			string s = "";
			if (depth != 0)
			switch ((production_kind) kind)
			{
				case production_kind.g__priority_assoc_specification__1:
					// emit syntax tree node associated with production
					// <priority assoc specification>: <priority specification> <associativity specification>

					s += m__priority_specification_.Emit (depth - 1);
					s += " ";
					s += m__associativity_specification_.Emit (depth - 1);
				break;
				case production_kind.g__priority_assoc_specification__2:
					// emit syntax tree node associated with production
					// <priority assoc specification>: <associativity specification> <priority specification>

					s += m__associativity_specification_.Emit (depth - 1);
					s += " ";
					s += m__priority_specification_.Emit (depth - 1);
				break;
				case production_kind.g__priority_assoc_specification__3:
					// emit syntax tree node associated with production
					// <priority assoc specification>: <priority specification>

					s += m__priority_specification_.Emit (depth - 1);
				break;
				case production_kind.g__priority_assoc_specification__4:
					// emit syntax tree node associated with production
					// <priority assoc specification>: <associativity specification>

					s += m__associativity_specification_.Emit (depth - 1);
				break;
				case production_kind.g__priority_assoc_specification__5:
					// emit syntax tree node associated with production
					// <priority assoc specification>: <priority specification> ',' <associativity specification>

					s += m__priority_specification_.Emit (depth - 1);
					s += " ";
					s += m__comma_.Emit (depth - 1);
					s += " ";
					s += m__associativity_specification_.Emit (depth - 1);
				break;
				case production_kind.g__priority_assoc_specification__6:
					// emit syntax tree node associated with production
					// <priority assoc specification>: <associativity specification> ',' <priority specification>

					s += m__associativity_specification_.Emit (depth - 1);
					s += " ";
					s += m__comma_.Emit (depth - 1);
					s += " ";
					s += m__priority_specification_.Emit (depth - 1);
				break;
			}
			return s.Trim ();
		}

		//
		// emit production tree node associated with any production of syntax rule <priority assoc specification>
		//

		public override string EmitProductionTree (int depth)
		{
			string s = "_priority_assoc_specification_ (";
			if (depth != 0)
				switch ((production_kind) kind)
				{
				case production_kind.g__priority_assoc_specification__1:
					// emit syntax tree node associated with production
					// <priority assoc specification>: <priority specification> <associativity specification>

					s += m__priority_specification_.EmitProductionTree (depth - 1);
					s += ' ';
					s += m__associativity_specification_.EmitProductionTree (depth - 1);
				break;
				case production_kind.g__priority_assoc_specification__2:
					// emit syntax tree node associated with production
					// <priority assoc specification>: <associativity specification> <priority specification>

					s += m__associativity_specification_.EmitProductionTree (depth - 1);
					s += ' ';
					s += m__priority_specification_.EmitProductionTree (depth - 1);
				break;
				case production_kind.g__priority_assoc_specification__3:
					// emit syntax tree node associated with production
					// <priority assoc specification>: <priority specification>

					s += m__priority_specification_.EmitProductionTree (depth - 1);
				break;
				case production_kind.g__priority_assoc_specification__4:
					// emit syntax tree node associated with production
					// <priority assoc specification>: <associativity specification>

					s += m__associativity_specification_.EmitProductionTree (depth - 1);
				break;
				case production_kind.g__priority_assoc_specification__5:
					// emit syntax tree node associated with production
					// <priority assoc specification>: <priority specification> ',' <associativity specification>

					s += m__priority_specification_.EmitProductionTree (depth - 1);
					s += ' ';
					s += "_comma_";
					s += ' ';
					s += m__associativity_specification_.EmitProductionTree (depth - 1);
				break;
				case production_kind.g__priority_assoc_specification__6:
					// emit syntax tree node associated with production
					// <priority assoc specification>: <associativity specification> ',' <priority specification>

					s += m__associativity_specification_.EmitProductionTree (depth - 1);
					s += ' ';
					s += "_comma_";
					s += ' ';
					s += m__priority_specification_.EmitProductionTree (depth - 1);
				break;
			}
			s += ")";
			return s;
		}
		#endregion
		#region Syntax tree traversal,  reperrent, init and iterator methods associated with the syntax rule <priority assoc specification>

		//
		// traverse sub-tree rooted in this node using selected syntax tree walker
		//

		public override void InvokeTraverse (SyntaxTreeWalkerCore syntaxTreeWalker) => ((SyntaxTreeWalker) syntaxTreeWalker).Traverse (this);
		public override void InvokeTraverseCommon (SyntaxTreeWalkerCore syntaxTreeWalker) => ((SyntaxTreeWalker) syntaxTreeWalker).TraverseCommon (this);

		// reparent sub-tree

		public override void reparent (SyntaxTreeBase parent)
		{
			this.parent = parent;

			m__priority_specification_?.reparent (this);
			m__associativity_specification_?.reparent (this);
			m__comma_?.reparent (this);
		}

		//
		// initialize object associated with syntax rule <priority assoc specification>
		//

		public void _init ()
		{
			m__priority_specification_ = null;
			m__associativity_specification_ = null;
			m__comma_ = null;
		}
		#endregion
		#region Fields and properties associated with the syntax rule <priority assoc specification>

		// counter of all nodes associated with syntax rule <priority assoc specification>
		public static int g_counter;

		// objects associated with terminal and non-terminal symbols within production(s) of syntax rule <priority assoc specification>
		public _priority_specification_ m__priority_specification_ { get; private set; }
		public _associativity_specification_ m__associativity_specification_ { get; private set; }
		public SyntaxTreeToken m__comma_ { get; private set; }
		#endregion

	};

	public partial class SyntaxTreeWalker
	{
		#region delegates and events associated with the syntax rule <priority assoc specification>

		// delegate function (callback) prototype associated with syntax rule <priority assoc specification>
		public delegate bool _priority_assoc_specification__Callback (SyntaxTreeCallbackReason reason, _priority_assoc_specification_.production_kind kind, _priority_assoc_specification_ p__priority_assoc_specification_);

		// event associated with syntax rule <priority assoc specification>
		public event _priority_assoc_specification__Callback _priority_assoc_specification__Event;

		// event trigger associated with syntax rule <priority assoc specification>
		public bool Raise__priority_assoc_specification__Event (SyntaxTreeCallbackReason reason, _priority_assoc_specification_.production_kind kind, _priority_assoc_specification_ p__priority_assoc_specification_)
		{
			bool? status = _priority_assoc_specification__Event?.Invoke (reason, kind, p__priority_assoc_specification_);
			return (status == null) || status.Value;
		}
		#endregion
		#region syntax tree traversal methods associated with the syntax rule <priority assoc specification>
		//
		// traverse syntax tree node associated with any production of syntax rule <priority assoc specification>
		//

		public void Traverse (_priority_assoc_specification_ p__priority_assoc_specification_)
		{
			if (p__priority_assoc_specification_.isLocked())
				return;
			p__priority_assoc_specification_.dolock();
			_priority_assoc_specification_.production_kind kind = (_priority_assoc_specification_.production_kind) p__priority_assoc_specification_.kind;
			p__priority_assoc_specification_.turn_reset ();
			if (Raise__priority_assoc_specification__Event (SyntaxTreeCallbackReason.TraversalPrologueCallbackReason, kind, p__priority_assoc_specification_))
			switch (kind)
			{
				case _priority_assoc_specification_.production_kind.g__priority_assoc_specification__1:
					// traverse syntax tree node associated with production
					// <priority assoc specification>: <priority specification> <associativity specification>

					if (Raise__priority_assoc_specification__Event (SyntaxTreeCallbackReason.TraversalMidTermCallbackReason, kind, p__priority_assoc_specification_))
						Traverse (p__priority_assoc_specification_.m__priority_specification_);
					p__priority_assoc_specification_.turn_inc ();
					if (Raise__priority_assoc_specification__Event (SyntaxTreeCallbackReason.TraversalMidTermCallbackReason, kind, p__priority_assoc_specification_))
						Traverse (p__priority_assoc_specification_.m__associativity_specification_);
					p__priority_assoc_specification_.turn_inc ();
					Raise__priority_assoc_specification__Event (SyntaxTreeCallbackReason.TraversalMidTermCallbackReason, kind, p__priority_assoc_specification_);
				break;
				case _priority_assoc_specification_.production_kind.g__priority_assoc_specification__2:
					// traverse syntax tree node associated with production
					// <priority assoc specification>: <associativity specification> <priority specification>

					if (Raise__priority_assoc_specification__Event (SyntaxTreeCallbackReason.TraversalMidTermCallbackReason, kind, p__priority_assoc_specification_))
						Traverse (p__priority_assoc_specification_.m__associativity_specification_);
					p__priority_assoc_specification_.turn_inc ();
					if (Raise__priority_assoc_specification__Event (SyntaxTreeCallbackReason.TraversalMidTermCallbackReason, kind, p__priority_assoc_specification_))
						Traverse (p__priority_assoc_specification_.m__priority_specification_);
					p__priority_assoc_specification_.turn_inc ();
					Raise__priority_assoc_specification__Event (SyntaxTreeCallbackReason.TraversalMidTermCallbackReason, kind, p__priority_assoc_specification_);
				break;
				case _priority_assoc_specification_.production_kind.g__priority_assoc_specification__3:
					// traverse syntax tree node associated with production
					// <priority assoc specification>: <priority specification>

					if (Raise__priority_assoc_specification__Event (SyntaxTreeCallbackReason.TraversalMidTermCallbackReason, kind, p__priority_assoc_specification_))
						Traverse (p__priority_assoc_specification_.m__priority_specification_);
					p__priority_assoc_specification_.turn_inc ();
					Raise__priority_assoc_specification__Event (SyntaxTreeCallbackReason.TraversalMidTermCallbackReason, kind, p__priority_assoc_specification_);
				break;
				case _priority_assoc_specification_.production_kind.g__priority_assoc_specification__4:
					// traverse syntax tree node associated with production
					// <priority assoc specification>: <associativity specification>

					if (Raise__priority_assoc_specification__Event (SyntaxTreeCallbackReason.TraversalMidTermCallbackReason, kind, p__priority_assoc_specification_))
						Traverse (p__priority_assoc_specification_.m__associativity_specification_);
					p__priority_assoc_specification_.turn_inc ();
					Raise__priority_assoc_specification__Event (SyntaxTreeCallbackReason.TraversalMidTermCallbackReason, kind, p__priority_assoc_specification_);
				break;
				case _priority_assoc_specification_.production_kind.g__priority_assoc_specification__5:
					// traverse syntax tree node associated with production
					// <priority assoc specification>: <priority specification> ',' <associativity specification>

					if (Raise__priority_assoc_specification__Event (SyntaxTreeCallbackReason.TraversalMidTermCallbackReason, kind, p__priority_assoc_specification_))
						Traverse (p__priority_assoc_specification_.m__priority_specification_);
					p__priority_assoc_specification_.turn_inc ();
					if (Raise__priority_assoc_specification__Event (SyntaxTreeCallbackReason.TraversalMidTermCallbackReason, kind, p__priority_assoc_specification_))
						Traverse (p__priority_assoc_specification_.m__associativity_specification_);
					p__priority_assoc_specification_.turn_inc ();
					Raise__priority_assoc_specification__Event (SyntaxTreeCallbackReason.TraversalMidTermCallbackReason, kind, p__priority_assoc_specification_);
				break;
				case _priority_assoc_specification_.production_kind.g__priority_assoc_specification__6:
					// traverse syntax tree node associated with production
					// <priority assoc specification>: <associativity specification> ',' <priority specification>

					if (Raise__priority_assoc_specification__Event (SyntaxTreeCallbackReason.TraversalMidTermCallbackReason, kind, p__priority_assoc_specification_))
						Traverse (p__priority_assoc_specification_.m__associativity_specification_);
					p__priority_assoc_specification_.turn_inc ();
					if (Raise__priority_assoc_specification__Event (SyntaxTreeCallbackReason.TraversalMidTermCallbackReason, kind, p__priority_assoc_specification_))
						Traverse (p__priority_assoc_specification_.m__priority_specification_);
					p__priority_assoc_specification_.turn_inc ();
					Raise__priority_assoc_specification__Event (SyntaxTreeCallbackReason.TraversalMidTermCallbackReason, kind, p__priority_assoc_specification_);
				break;
			}
			Raise__priority_assoc_specification__Event (SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason, kind, p__priority_assoc_specification_);
			p__priority_assoc_specification_.unlock();
		}

		//
		// traverse syntax tree node associated with any production of syntax rule <priority assoc specification>
		//

		public void TraverseCommon (_priority_assoc_specification_ p__priority_assoc_specification_)
		{
			_priority_assoc_specification_.production_kind kind = (_priority_assoc_specification_.production_kind) p__priority_assoc_specification_.kind;
			if (Raise_Common_Event (SyntaxTreeCallbackReason.TraversalPrologueCallbackReason, (int) kind, p__priority_assoc_specification_))
			switch (kind)
			{
				case _priority_assoc_specification_.production_kind.g__priority_assoc_specification__1:
					// traverse syntax tree node associated with production
					// <priority assoc specification>: <priority specification> <associativity specification>

						TraverseCommon (p__priority_assoc_specification_.m__priority_specification_);
						TraverseCommon (p__priority_assoc_specification_.m__associativity_specification_);
				break;
				case _priority_assoc_specification_.production_kind.g__priority_assoc_specification__2:
					// traverse syntax tree node associated with production
					// <priority assoc specification>: <associativity specification> <priority specification>

						TraverseCommon (p__priority_assoc_specification_.m__associativity_specification_);
						TraverseCommon (p__priority_assoc_specification_.m__priority_specification_);
				break;
				case _priority_assoc_specification_.production_kind.g__priority_assoc_specification__3:
					// traverse syntax tree node associated with production
					// <priority assoc specification>: <priority specification>

						TraverseCommon (p__priority_assoc_specification_.m__priority_specification_);
				break;
				case _priority_assoc_specification_.production_kind.g__priority_assoc_specification__4:
					// traverse syntax tree node associated with production
					// <priority assoc specification>: <associativity specification>

						TraverseCommon (p__priority_assoc_specification_.m__associativity_specification_);
				break;
				case _priority_assoc_specification_.production_kind.g__priority_assoc_specification__5:
					// traverse syntax tree node associated with production
					// <priority assoc specification>: <priority specification> ',' <associativity specification>

						TraverseCommon (p__priority_assoc_specification_.m__priority_specification_);
						TraverseCommon (p__priority_assoc_specification_.m__comma_);
						TraverseCommon (p__priority_assoc_specification_.m__associativity_specification_);
				break;
				case _priority_assoc_specification_.production_kind.g__priority_assoc_specification__6:
					// traverse syntax tree node associated with production
					// <priority assoc specification>: <associativity specification> ',' <priority specification>

						TraverseCommon (p__priority_assoc_specification_.m__associativity_specification_);
						TraverseCommon (p__priority_assoc_specification_.m__comma_);
						TraverseCommon (p__priority_assoc_specification_.m__priority_specification_);
				break;
			}
			Raise_Common_Event (SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason, (int) kind, p__priority_assoc_specification_);
		}
	};
		#endregion

	public partial class SyntaxTreeBuilder : SyntaxTreeWalker
	{
		#region syntax tree builder methods associated with the syntax rule <priority assoc specification>

		//
		// create syntax tree node associated with production
		// <priority assoc specification>: <priority specification> <associativity specification>

		//

		public _priority_assoc_specification_ _priority_assoc_specification__1 (_priority_specification_ p__priority_specification_, _associativity_specification_ p__associativity_specification_)
		{
			_priority_assoc_specification_ p__priority_assoc_specification__ref = new _priority_assoc_specification_(p__priority_specification_, p__associativity_specification_);
			p__priority_specification_.parent = p__priority_assoc_specification__ref;
			p__associativity_specification_.parent = p__priority_assoc_specification__ref;
			Raise__priority_assoc_specification__Event (SyntaxTreeCallbackReason.BuilderCallbackReason, _priority_assoc_specification_.production_kind.g__priority_assoc_specification__1, p__priority_assoc_specification__ref);
			return p__priority_assoc_specification__ref;
		}

		//
		// create syntax tree node associated with production
		// <priority assoc specification>: <associativity specification> <priority specification>

		//

		public _priority_assoc_specification_ _priority_assoc_specification__2 (_associativity_specification_ p__associativity_specification_, _priority_specification_ p__priority_specification_)
		{
			_priority_assoc_specification_ p__priority_assoc_specification__ref = new _priority_assoc_specification_(p__associativity_specification_, p__priority_specification_);
			p__associativity_specification_.parent = p__priority_assoc_specification__ref;
			p__priority_specification_.parent = p__priority_assoc_specification__ref;
			Raise__priority_assoc_specification__Event (SyntaxTreeCallbackReason.BuilderCallbackReason, _priority_assoc_specification_.production_kind.g__priority_assoc_specification__2, p__priority_assoc_specification__ref);
			return p__priority_assoc_specification__ref;
		}

		//
		// create syntax tree node associated with production
		// <priority assoc specification>: <priority specification>

		//

		public _priority_assoc_specification_ _priority_assoc_specification__3 (_priority_specification_ p__priority_specification_)
		{
			_priority_assoc_specification_ p__priority_assoc_specification__ref = new _priority_assoc_specification_(p__priority_specification_);
			p__priority_specification_.parent = p__priority_assoc_specification__ref;
			Raise__priority_assoc_specification__Event (SyntaxTreeCallbackReason.BuilderCallbackReason, _priority_assoc_specification_.production_kind.g__priority_assoc_specification__3, p__priority_assoc_specification__ref);
			return p__priority_assoc_specification__ref;
		}

		//
		// create syntax tree node associated with production
		// <priority assoc specification>: <associativity specification>

		//

		public _priority_assoc_specification_ _priority_assoc_specification__4 (_associativity_specification_ p__associativity_specification_)
		{
			_priority_assoc_specification_ p__priority_assoc_specification__ref = new _priority_assoc_specification_(p__associativity_specification_);
			p__associativity_specification_.parent = p__priority_assoc_specification__ref;
			Raise__priority_assoc_specification__Event (SyntaxTreeCallbackReason.BuilderCallbackReason, _priority_assoc_specification_.production_kind.g__priority_assoc_specification__4, p__priority_assoc_specification__ref);
			return p__priority_assoc_specification__ref;
		}

		//
		// create syntax tree node associated with production
		// <priority assoc specification>: <priority specification> ',' <associativity specification>

		//

		public _priority_assoc_specification_ _priority_assoc_specification__5 (_priority_specification_ p__priority_specification_, SyntaxTreeToken p_token, _associativity_specification_ p__associativity_specification_)
		{
			_priority_assoc_specification_ p__priority_assoc_specification__ref = new _priority_assoc_specification_(p__priority_specification_, p_token, p__associativity_specification_);
			p__priority_specification_.parent = p__priority_assoc_specification__ref;
			p_token.parent = p__priority_assoc_specification__ref;
			p__associativity_specification_.parent = p__priority_assoc_specification__ref;
			Raise__priority_assoc_specification__Event (SyntaxTreeCallbackReason.BuilderCallbackReason, _priority_assoc_specification_.production_kind.g__priority_assoc_specification__5, p__priority_assoc_specification__ref);
			return p__priority_assoc_specification__ref;
		}

		//
		// create syntax tree node associated with production
		// <priority assoc specification>: <associativity specification> ',' <priority specification>

		//

		public _priority_assoc_specification_ _priority_assoc_specification__6 (_associativity_specification_ p__associativity_specification_, SyntaxTreeToken p_token, _priority_specification_ p__priority_specification_)
		{
			_priority_assoc_specification_ p__priority_assoc_specification__ref = new _priority_assoc_specification_(p__associativity_specification_, p_token, p__priority_specification_);
			p__associativity_specification_.parent = p__priority_assoc_specification__ref;
			p_token.parent = p__priority_assoc_specification__ref;
			p__priority_specification_.parent = p__priority_assoc_specification__ref;
			Raise__priority_assoc_specification__Event (SyntaxTreeCallbackReason.BuilderCallbackReason, _priority_assoc_specification_.production_kind.g__priority_assoc_specification__6, p__priority_assoc_specification__ref);
			return p__priority_assoc_specification__ref;
		}
		#endregion
	};
}
