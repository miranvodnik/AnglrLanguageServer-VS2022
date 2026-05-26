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
	// class associated with syntax rule <anglr file>
	//

	public class	_anglr_file_ : SyntaxTreeBase
	{
		#region enumerated production(s) of syntax rule <anglr file>
		public enum production_kind : ushort	// enumerated production(s) of syntax rule <anglr file>
		{
			g__anglr_file__1 = 1,	// %empty

			g__anglr_file__2 = 2,	// <anglr file part list>

		};
		#endregion
		#region production markers associated with the syntax rule <anglr file>

		// markers associated with production: <anglr file> -> %empty

		public enum production_marker_1 : ushort
		{
			final
		};

		// markers associated with production: <anglr file> -> <anglr file part list>

		public enum production_marker_2 : ushort
		{
			m__anglr_file_part_list_,
			final
		};
		#endregion
		#region Constructors and destructors associated with the syntax rule <anglr file>

		// Constructor declaration(s) associated with production(s) of syntax rule <anglr file>

		//
		// Constructor associated with the following production(s)
		// <anglr file> -> %empty

		//

		public _anglr_file_ () : base ((uint) ProductionID.__anglr_file__ID, (uint) production_kind.g__anglr_file__1)
		{
			++g_counter;
			_init ();
			children = Array.Empty <SyntaxTreeBase> ();
		}

		//
		// Constructor associated with the following production(s)
		// <anglr file> -> <anglr file part list>

		//

		public _anglr_file_ (_anglr_file_part_list_ p__anglr_file_part_list_) : base ((uint) ProductionID.__anglr_file__ID, (uint) production_kind.g__anglr_file__2)
		{
			++g_counter;
			_init ();
			children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 1);
			children[0] = m__anglr_file_part_list_ = p__anglr_file_part_list_;
		}

		// Copy constructor

		public _anglr_file_ (_anglr_file_ p__anglr_file_) : base (p__anglr_file_.id, p__anglr_file_.kind)
		{
			++g_counter;
			_init ();
			switch ((production_kind) this.kind)
			{
			case production_kind.g__anglr_file__1:
				children = Array.Empty <SyntaxTreeBase> ();
				break;
			case production_kind.g__anglr_file__2:
				children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 1);
				children[0] = m__anglr_file_part_list_ = p__anglr_file_.m__anglr_file_part_list_;
				break;
			default:
				string[] args = new string[] { "_anglr_file_" };
				throw new SyntaxTreeError (SyntaxTreeError.InvalidKindError, args);
			}
		}

		// Clone node - wrapper of copy constructor

		public override SyntaxTreeBase Clone () { return (SyntaxTreeBase) new _anglr_file_ (this); }

		// Destructor

		public new void Dispose ()
		{
			base.Dispose ();
			--g_counter;

			m__anglr_file_part_list_?.Dispose ();

			_init ();
		}
		#endregion
		#region Content changing and inclusion checking methods associated with the syntax rule <anglr file>

		// Content changing function(s) associated with production(s) of syntax rule <anglr file>

		//
		// Content changing function associated with following production(s)
		// <anglr file> -> %empty

		//

		public void change()
		{
			_init ();
			this.kind = (uint) production_kind.g__anglr_file__1;
			children = Array.Empty <SyntaxTreeBase> ();
			reparent (parent);
		}

		//
		// Content changing function associated with following production(s)
		// <anglr file> -> <anglr file part list>

		//

		public void change(_anglr_file_part_list_ p__anglr_file_part_list_)
		{
			_init ();
			this.kind = (uint) production_kind.g__anglr_file__2;
			children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 1);
			children [0] = m__anglr_file_part_list_ = p__anglr_file_part_list_;
			reparent (parent);
		}

		//
		// check if node 'element' is included within syntax tree associated with syntax rule <anglr file>
		//

		public new bool checkInclusion (SyntaxTreeBase element)
		{
			return
				(element == this) ||
				(m__anglr_file_part_list_ != null) && m__anglr_file_part_list_.checkInclusion (element) ||
				false;
		}
		#endregion
		#region Syntax tree emit methods associated with the syntax rule <anglr file>

		//
		// emit production tree node associated with any production of syntax rule <anglr file>
		//

		public override string Emit (int depth)
		{
			string s = "";
			if (depth != 0)
			switch ((production_kind) kind)
			{
				case production_kind.g__anglr_file__1:
					// emit syntax tree node associated with production
					// <anglr file>: %empty

				break;
				case production_kind.g__anglr_file__2:
					// emit syntax tree node associated with production
					// <anglr file>: <anglr file part list>

					s += m__anglr_file_part_list_.Emit (depth - 1);
				break;
			}
			return s.Trim ();
		}

		//
		// emit production tree node associated with any production of syntax rule <anglr file>
		//

		public override string EmitProductionTree (int depth)
		{
			string s = "_anglr_file_ (";
			if (depth != 0)
				switch ((production_kind) kind)
				{
				case production_kind.g__anglr_file__1:
					// emit syntax tree node associated with production
					// <anglr file>: %empty

				break;
				case production_kind.g__anglr_file__2:
					// emit syntax tree node associated with production
					// <anglr file>: <anglr file part list>

					s += m__anglr_file_part_list_.EmitProductionTree (depth - 1);
				break;
			}
			s += ")";
			return s;
		}
		#endregion
		#region Syntax tree traversal,  reperrent, init and iterator methods associated with the syntax rule <anglr file>

		//
		// traverse sub-tree rooted in this node using selected syntax tree walker
		//

		public override void InvokeTraverse (SyntaxTreeWalkerCore syntaxTreeWalker) => ((SyntaxTreeWalker) syntaxTreeWalker).Traverse (this);
		public override void InvokeTraverseCommon (SyntaxTreeWalkerCore syntaxTreeWalker) => ((SyntaxTreeWalker) syntaxTreeWalker).TraverseCommon (this);

		// reparent sub-tree

		public override void reparent (SyntaxTreeBase parent)
		{
			this.parent = parent;

			m__anglr_file_part_list_?.reparent (this);
		}

		//
		// initialize object associated with syntax rule <anglr file>
		//

		public void _init ()
		{
			m__anglr_file_part_list_ = null;
		}
		#endregion
		#region Fields and properties associated with the syntax rule <anglr file>

		// counter of all nodes associated with syntax rule <anglr file>
		public static int g_counter;

		// objects associated with terminal and non-terminal symbols within production(s) of syntax rule <anglr file>
		public _anglr_file_part_list_ m__anglr_file_part_list_ { get; private set; }
		#endregion

	};

	public partial class SyntaxTreeWalker
	{
		#region delegates and events associated with the syntax rule <anglr file>

		// delegate function (callback) prototype associated with syntax rule <anglr file>
		public delegate bool _anglr_file__Callback (SyntaxTreeCallbackReason reason, _anglr_file_.production_kind kind, _anglr_file_ p__anglr_file_);

		// event associated with syntax rule <anglr file>
		public event _anglr_file__Callback _anglr_file__Event;

		// event trigger associated with syntax rule <anglr file>
		public bool Raise__anglr_file__Event (SyntaxTreeCallbackReason reason, _anglr_file_.production_kind kind, _anglr_file_ p__anglr_file_)
		{
			bool? status = _anglr_file__Event?.Invoke (reason, kind, p__anglr_file_);
			return (status == null) || status.Value;
		}
		#endregion
		#region syntax tree traversal methods associated with the syntax rule <anglr file>
		//
		// traverse syntax tree node associated with any production of syntax rule <anglr file>
		//

		public void Traverse (_anglr_file_ p__anglr_file_)
		{
			if (p__anglr_file_.isLocked())
				return;
			p__anglr_file_.dolock();
			_anglr_file_.production_kind kind = (_anglr_file_.production_kind) p__anglr_file_.kind;
			p__anglr_file_.turn_reset ();
			if (Raise__anglr_file__Event (SyntaxTreeCallbackReason.TraversalPrologueCallbackReason, kind, p__anglr_file_))
			switch (kind)
			{
				case _anglr_file_.production_kind.g__anglr_file__1:
					// traverse syntax tree node associated with production
					// <anglr file>: %empty

				break;
				case _anglr_file_.production_kind.g__anglr_file__2:
					// traverse syntax tree node associated with production
					// <anglr file>: <anglr file part list>

					if (Raise__anglr_file__Event (SyntaxTreeCallbackReason.TraversalMidTermCallbackReason, kind, p__anglr_file_))
						Traverse (p__anglr_file_.m__anglr_file_part_list_);
					p__anglr_file_.turn_inc ();
					Raise__anglr_file__Event (SyntaxTreeCallbackReason.TraversalMidTermCallbackReason, kind, p__anglr_file_);
				break;
			}
			Raise__anglr_file__Event (SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason, kind, p__anglr_file_);
			p__anglr_file_.unlock();
		}

		//
		// traverse syntax tree node associated with any production of syntax rule <anglr file>
		//

		public void TraverseCommon (_anglr_file_ p__anglr_file_)
		{
			_anglr_file_.production_kind kind = (_anglr_file_.production_kind) p__anglr_file_.kind;
			if (Raise_Common_Event (SyntaxTreeCallbackReason.TraversalPrologueCallbackReason, (int) kind, p__anglr_file_))
			switch (kind)
			{
				case _anglr_file_.production_kind.g__anglr_file__1:
					// traverse syntax tree node associated with production
					// <anglr file>: %empty

				break;
				case _anglr_file_.production_kind.g__anglr_file__2:
					// traverse syntax tree node associated with production
					// <anglr file>: <anglr file part list>

						TraverseCommon (p__anglr_file_.m__anglr_file_part_list_);
				break;
			}
			Raise_Common_Event (SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason, (int) kind, p__anglr_file_);
		}
	};
		#endregion

	public partial class SyntaxTreeBuilder : SyntaxTreeWalker
	{
		#region syntax tree builder methods associated with the syntax rule <anglr file>

		//
		// create syntax tree node associated with production
		// <anglr file>: %empty

		//

		public _anglr_file_ _anglr_file__1 ()
		{
			_anglr_file_ p__anglr_file__ref = new _anglr_file_();
			Raise__anglr_file__Event (SyntaxTreeCallbackReason.BuilderCallbackReason, _anglr_file_.production_kind.g__anglr_file__1, p__anglr_file__ref);
			return p__anglr_file__ref;
		}

		//
		// create syntax tree node associated with production
		// <anglr file>: <anglr file part list>

		//

		public _anglr_file_ _anglr_file__2 (_anglr_file_part_list_ p__anglr_file_part_list_)
		{
			_anglr_file_ p__anglr_file__ref = new _anglr_file_(p__anglr_file_part_list_);
			p__anglr_file_part_list_.parent = p__anglr_file__ref;
			Raise__anglr_file__Event (SyntaxTreeCallbackReason.BuilderCallbackReason, _anglr_file_.production_kind.g__anglr_file__2, p__anglr_file__ref);
			return p__anglr_file__ref;
		}
		#endregion
	};
}
