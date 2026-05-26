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
	// class associated with syntax rule <block regex definitions optional>
	//

	public class	_block_regex_definitions_optional_ : SyntaxTreeBase
	{
		#region enumerated production(s) of syntax rule <block regex definitions optional>
		public enum production_kind : ushort	// enumerated production(s) of syntax rule <block regex definitions optional>
		{
			g__block_regex_definitions_optional__1 = 1,	// %empty

			g__block_regex_definitions_optional__2 = 2,	// <block regex definitions>

		};
		#endregion
		#region production markers associated with the syntax rule <block regex definitions optional>

		// markers associated with production: <block regex definitions optional> -> %empty

		public enum production_marker_1 : ushort
		{
			final
		};

		// markers associated with production: <block regex definitions optional> -> <block regex definitions>

		public enum production_marker_2 : ushort
		{
			m__block_regex_definitions_,
			final
		};
		#endregion
		#region Constructors and destructors associated with the syntax rule <block regex definitions optional>

		// Constructor declaration(s) associated with production(s) of syntax rule <block regex definitions optional>

		//
		// Constructor associated with the following production(s)
		// <block regex definitions optional> -> %empty

		//

		public _block_regex_definitions_optional_ () : base ((uint) ProductionID.__block_regex_definitions_optional__ID, (uint) production_kind.g__block_regex_definitions_optional__1)
		{
			++g_counter;
			_init ();
			children = Array.Empty <SyntaxTreeBase> ();
		}

		//
		// Constructor associated with the following production(s)
		// <block regex definitions optional> -> <block regex definitions>

		//

		public _block_regex_definitions_optional_ (_block_regex_definitions_ p__block_regex_definitions_) : base ((uint) ProductionID.__block_regex_definitions_optional__ID, (uint) production_kind.g__block_regex_definitions_optional__2)
		{
			++g_counter;
			_init ();
			children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 1);
			children[0] = m__block_regex_definitions_ = p__block_regex_definitions_;
		}

		// Copy constructor

		public _block_regex_definitions_optional_ (_block_regex_definitions_optional_ p__block_regex_definitions_optional_) : base (p__block_regex_definitions_optional_.id, p__block_regex_definitions_optional_.kind)
		{
			++g_counter;
			_init ();
			switch ((production_kind) this.kind)
			{
			case production_kind.g__block_regex_definitions_optional__1:
				children = Array.Empty <SyntaxTreeBase> ();
				break;
			case production_kind.g__block_regex_definitions_optional__2:
				children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 1);
				children[0] = m__block_regex_definitions_ = p__block_regex_definitions_optional_.m__block_regex_definitions_;
				break;
			default:
				string[] args = new string[] { "_block_regex_definitions_optional_" };
				throw new SyntaxTreeError (SyntaxTreeError.InvalidKindError, args);
			}
		}

		// Clone node - wrapper of copy constructor

		public override SyntaxTreeBase Clone () { return (SyntaxTreeBase) new _block_regex_definitions_optional_ (this); }

		// Destructor

		public new void Dispose ()
		{
			base.Dispose ();
			--g_counter;

			m__block_regex_definitions_?.Dispose ();

			_init ();
		}
		#endregion
		#region Content changing and inclusion checking methods associated with the syntax rule <block regex definitions optional>

		// Content changing function(s) associated with production(s) of syntax rule <block regex definitions optional>

		//
		// Content changing function associated with following production(s)
		// <block regex definitions optional> -> %empty

		//

		public void change()
		{
			_init ();
			this.kind = (uint) production_kind.g__block_regex_definitions_optional__1;
			children = Array.Empty <SyntaxTreeBase> ();
			reparent (parent);
		}

		//
		// Content changing function associated with following production(s)
		// <block regex definitions optional> -> <block regex definitions>

		//

		public void change(_block_regex_definitions_ p__block_regex_definitions_)
		{
			_init ();
			this.kind = (uint) production_kind.g__block_regex_definitions_optional__2;
			children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 1);
			children [0] = m__block_regex_definitions_ = p__block_regex_definitions_;
			reparent (parent);
		}

		//
		// check if node 'element' is included within syntax tree associated with syntax rule <block regex definitions optional>
		//

		public new bool checkInclusion (SyntaxTreeBase element)
		{
			return
				(element == this) ||
				(m__block_regex_definitions_ != null) && m__block_regex_definitions_.checkInclusion (element) ||
				false;
		}
		#endregion
		#region Syntax tree emit methods associated with the syntax rule <block regex definitions optional>

		//
		// emit production tree node associated with any production of syntax rule <block regex definitions optional>
		//

		public override string Emit (int depth)
		{
			string s = "";
			if (depth != 0)
			switch ((production_kind) kind)
			{
				case production_kind.g__block_regex_definitions_optional__1:
					// emit syntax tree node associated with production
					// <block regex definitions optional>: %empty

				break;
				case production_kind.g__block_regex_definitions_optional__2:
					// emit syntax tree node associated with production
					// <block regex definitions optional>: <block regex definitions>

					s += m__block_regex_definitions_.Emit (depth - 1);
				break;
			}
			return s.Trim ();
		}

		//
		// emit production tree node associated with any production of syntax rule <block regex definitions optional>
		//

		public override string EmitProductionTree (int depth)
		{
			string s = "_block_regex_definitions_optional_ (";
			if (depth != 0)
				switch ((production_kind) kind)
				{
				case production_kind.g__block_regex_definitions_optional__1:
					// emit syntax tree node associated with production
					// <block regex definitions optional>: %empty

				break;
				case production_kind.g__block_regex_definitions_optional__2:
					// emit syntax tree node associated with production
					// <block regex definitions optional>: <block regex definitions>

					s += m__block_regex_definitions_.EmitProductionTree (depth - 1);
				break;
			}
			s += ")";
			return s;
		}
		#endregion
		#region Syntax tree traversal,  reperrent, init and iterator methods associated with the syntax rule <block regex definitions optional>

		//
		// traverse sub-tree rooted in this node using selected syntax tree walker
		//

		public override void InvokeTraverse (SyntaxTreeWalkerCore syntaxTreeWalker) => ((SyntaxTreeWalker) syntaxTreeWalker).Traverse (this);
		public override void InvokeTraverseCommon (SyntaxTreeWalkerCore syntaxTreeWalker) => ((SyntaxTreeWalker) syntaxTreeWalker).TraverseCommon (this);

		// reparent sub-tree

		public override void reparent (SyntaxTreeBase parent)
		{
			this.parent = parent;

			m__block_regex_definitions_?.reparent (this);
		}

		//
		// initialize object associated with syntax rule <block regex definitions optional>
		//

		public void _init ()
		{
			m__block_regex_definitions_ = null;
		}
		#endregion
		#region Fields and properties associated with the syntax rule <block regex definitions optional>

		// counter of all nodes associated with syntax rule <block regex definitions optional>
		public static int g_counter;

		// objects associated with terminal and non-terminal symbols within production(s) of syntax rule <block regex definitions optional>
		public _block_regex_definitions_ m__block_regex_definitions_ { get; private set; }
		#endregion

	};

	public partial class SyntaxTreeWalker
	{
		#region delegates and events associated with the syntax rule <block regex definitions optional>

		// delegate function (callback) prototype associated with syntax rule <block regex definitions optional>
		public delegate bool _block_regex_definitions_optional__Callback (SyntaxTreeCallbackReason reason, _block_regex_definitions_optional_.production_kind kind, _block_regex_definitions_optional_ p__block_regex_definitions_optional_);

		// event associated with syntax rule <block regex definitions optional>
		public event _block_regex_definitions_optional__Callback _block_regex_definitions_optional__Event;

		// event trigger associated with syntax rule <block regex definitions optional>
		public bool Raise__block_regex_definitions_optional__Event (SyntaxTreeCallbackReason reason, _block_regex_definitions_optional_.production_kind kind, _block_regex_definitions_optional_ p__block_regex_definitions_optional_)
		{
			bool? status = _block_regex_definitions_optional__Event?.Invoke (reason, kind, p__block_regex_definitions_optional_);
			return (status == null) || status.Value;
		}
		#endregion
		#region syntax tree traversal methods associated with the syntax rule <block regex definitions optional>
		//
		// traverse syntax tree node associated with any production of syntax rule <block regex definitions optional>
		//

		public void Traverse (_block_regex_definitions_optional_ p__block_regex_definitions_optional_)
		{
			if (p__block_regex_definitions_optional_.isLocked())
				return;
			p__block_regex_definitions_optional_.dolock();
			_block_regex_definitions_optional_.production_kind kind = (_block_regex_definitions_optional_.production_kind) p__block_regex_definitions_optional_.kind;
			p__block_regex_definitions_optional_.turn_reset ();
			if (Raise__block_regex_definitions_optional__Event (SyntaxTreeCallbackReason.TraversalPrologueCallbackReason, kind, p__block_regex_definitions_optional_))
			switch (kind)
			{
				case _block_regex_definitions_optional_.production_kind.g__block_regex_definitions_optional__1:
					// traverse syntax tree node associated with production
					// <block regex definitions optional>: %empty

				break;
				case _block_regex_definitions_optional_.production_kind.g__block_regex_definitions_optional__2:
					// traverse syntax tree node associated with production
					// <block regex definitions optional>: <block regex definitions>

					if (Raise__block_regex_definitions_optional__Event (SyntaxTreeCallbackReason.TraversalMidTermCallbackReason, kind, p__block_regex_definitions_optional_))
						Traverse (p__block_regex_definitions_optional_.m__block_regex_definitions_);
					p__block_regex_definitions_optional_.turn_inc ();
					Raise__block_regex_definitions_optional__Event (SyntaxTreeCallbackReason.TraversalMidTermCallbackReason, kind, p__block_regex_definitions_optional_);
				break;
			}
			Raise__block_regex_definitions_optional__Event (SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason, kind, p__block_regex_definitions_optional_);
			p__block_regex_definitions_optional_.unlock();
		}

		//
		// traverse syntax tree node associated with any production of syntax rule <block regex definitions optional>
		//

		public void TraverseCommon (_block_regex_definitions_optional_ p__block_regex_definitions_optional_)
		{
			_block_regex_definitions_optional_.production_kind kind = (_block_regex_definitions_optional_.production_kind) p__block_regex_definitions_optional_.kind;
			if (Raise_Common_Event (SyntaxTreeCallbackReason.TraversalPrologueCallbackReason, (int) kind, p__block_regex_definitions_optional_))
			switch (kind)
			{
				case _block_regex_definitions_optional_.production_kind.g__block_regex_definitions_optional__1:
					// traverse syntax tree node associated with production
					// <block regex definitions optional>: %empty

				break;
				case _block_regex_definitions_optional_.production_kind.g__block_regex_definitions_optional__2:
					// traverse syntax tree node associated with production
					// <block regex definitions optional>: <block regex definitions>

						TraverseCommon (p__block_regex_definitions_optional_.m__block_regex_definitions_);
				break;
			}
			Raise_Common_Event (SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason, (int) kind, p__block_regex_definitions_optional_);
		}
	};
		#endregion

	public partial class SyntaxTreeBuilder : SyntaxTreeWalker
	{
		#region syntax tree builder methods associated with the syntax rule <block regex definitions optional>

		//
		// create syntax tree node associated with production
		// <block regex definitions optional>: %empty

		//

		public _block_regex_definitions_optional_ _block_regex_definitions_optional__1 ()
		{
			_block_regex_definitions_optional_ p__block_regex_definitions_optional__ref = new _block_regex_definitions_optional_();
			Raise__block_regex_definitions_optional__Event (SyntaxTreeCallbackReason.BuilderCallbackReason, _block_regex_definitions_optional_.production_kind.g__block_regex_definitions_optional__1, p__block_regex_definitions_optional__ref);
			return p__block_regex_definitions_optional__ref;
		}

		//
		// create syntax tree node associated with production
		// <block regex definitions optional>: <block regex definitions>

		//

		public _block_regex_definitions_optional_ _block_regex_definitions_optional__2 (_block_regex_definitions_ p__block_regex_definitions_)
		{
			_block_regex_definitions_optional_ p__block_regex_definitions_optional__ref = new _block_regex_definitions_optional_(p__block_regex_definitions_);
			p__block_regex_definitions_.parent = p__block_regex_definitions_optional__ref;
			Raise__block_regex_definitions_optional__Event (SyntaxTreeCallbackReason.BuilderCallbackReason, _block_regex_definitions_optional_.production_kind.g__block_regex_definitions_optional__2, p__block_regex_definitions_optional__ref);
			return p__block_regex_definitions_optional__ref;
		}
		#endregion
	};
}
