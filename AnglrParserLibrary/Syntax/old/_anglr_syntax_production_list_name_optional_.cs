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
	// class associated with syntax rule <anglr syntax production list name optional>
	//

	public class	_anglr_syntax_production_list_name_optional_ : SyntaxTreeBase
	{
		#region enumerated production(s) of syntax rule <anglr syntax production list name optional>
		public enum production_kind : ushort	// enumerated production(s) of syntax rule <anglr syntax production list name optional>
		{
			g__anglr_syntax_production_list_name_optional__1 = 1,	// %empty

			g__anglr_syntax_production_list_name_optional__2 = 2,	// <anglr syntax production list name>

		};
		#endregion
		#region production markers associated with the syntax rule <anglr syntax production list name optional>

		// markers associated with production: <anglr syntax production list name optional> -> %empty

		public enum production_marker_1 : ushort
		{
			final
		};

		// markers associated with production: <anglr syntax production list name optional> -> <anglr syntax production list name>

		public enum production_marker_2 : ushort
		{
			m__anglr_syntax_production_list_name_,
			final
		};
		#endregion
		#region Constructors and destructors associated with the syntax rule <anglr syntax production list name optional>

		// Constructor declaration(s) associated with production(s) of syntax rule <anglr syntax production list name optional>

		//
		// Constructor associated with the following production(s)
		// <anglr syntax production list name optional> -> %empty

		//

		public _anglr_syntax_production_list_name_optional_ () : base ((uint) ProductionID.__anglr_syntax_production_list_name_optional__ID, (uint) production_kind.g__anglr_syntax_production_list_name_optional__1)
		{
			++g_counter;
			_init ();
			children = Array.Empty <SyntaxTreeBase> ();
		}

		//
		// Constructor associated with the following production(s)
		// <anglr syntax production list name optional> -> <anglr syntax production list name>

		//

		public _anglr_syntax_production_list_name_optional_ (_anglr_syntax_production_list_name_ p__anglr_syntax_production_list_name_) : base ((uint) ProductionID.__anglr_syntax_production_list_name_optional__ID, (uint) production_kind.g__anglr_syntax_production_list_name_optional__2)
		{
			++g_counter;
			_init ();
			children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 1);
			children[0] = m__anglr_syntax_production_list_name_ = p__anglr_syntax_production_list_name_;
		}

		// Copy constructor

		public _anglr_syntax_production_list_name_optional_ (_anglr_syntax_production_list_name_optional_ p__anglr_syntax_production_list_name_optional_) : base (p__anglr_syntax_production_list_name_optional_.id, p__anglr_syntax_production_list_name_optional_.kind)
		{
			++g_counter;
			_init ();
			switch ((production_kind) p__anglr_syntax_production_list_name_optional_.kind)
			{
				case production_kind.g__anglr_syntax_production_list_name_optional__1:
					children = Array.Empty <SyntaxTreeBase> ();
					break;
				case production_kind.g__anglr_syntax_production_list_name_optional__2:
					children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 1);
					if ((children [0] = m__anglr_syntax_production_list_name_ = (p__anglr_syntax_production_list_name_optional_.m__anglr_syntax_production_list_name_ != null) ? new _anglr_syntax_production_list_name_ (p__anglr_syntax_production_list_name_optional_.m__anglr_syntax_production_list_name_) : null) != null) m__anglr_syntax_production_list_name_.parent = this;
					break;
				default:
				{
					string[] args = new string[] { "_anglr_syntax_production_list_name_optional_" };
					throw new SyntaxTreeError (SyntaxTreeError.InvalidKindError, args);
				}
			}
		}

		// Clone node - wrapper of copy constructor

		public override SyntaxTreeBase Clone () { return (SyntaxTreeBase) new _anglr_syntax_production_list_name_optional_ (this); }

		// Destructor

		public new void Dispose ()
		{
			base.Dispose ();
			--g_counter;

			m__anglr_syntax_production_list_name_?.Dispose ();

			_init ();
		}
		#endregion
		#region Content changing and inclusion checking methods associated with the syntax rule <anglr syntax production list name optional>

		// Content changing function(s) associated with production(s) of syntax rule <anglr syntax production list name optional>

		//
		// Content changing function associated with following production(s)
		// <anglr syntax production list name optional> -> %empty

		//

		public void change()
		{
			_init ();
			this.kind = (uint) production_kind.g__anglr_syntax_production_list_name_optional__1;
			children = Array.Empty <SyntaxTreeBase> ();
			reparent (parent);
		}

		//
		// Content changing function associated with following production(s)
		// <anglr syntax production list name optional> -> <anglr syntax production list name>

		//

		public void change(_anglr_syntax_production_list_name_ p__anglr_syntax_production_list_name_)
		{
			_init ();
			this.kind = (uint) production_kind.g__anglr_syntax_production_list_name_optional__2;
			children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 1);
			children [0] = m__anglr_syntax_production_list_name_ = p__anglr_syntax_production_list_name_;
			reparent (parent);
		}

		//
		// check if node 'element' is included within syntax tree associated with syntax rule <anglr syntax production list name optional>
		//

		public new bool checkInclusion (SyntaxTreeBase element)
		{
			return
				(element == this) ||
				(m__anglr_syntax_production_list_name_ != null) && m__anglr_syntax_production_list_name_.checkInclusion (element) ||
				false;
		}
		#endregion
		#region Syntax tree emit methods associated with the syntax rule <anglr syntax production list name optional>

		//
		// emit production tree node associated with any production of syntax rule <anglr syntax production list name optional>
		//

		public override string Emit (int depth)
		{
			string s = "";
			if (depth != 0)
			switch ((production_kind) kind)
			{
				case production_kind.g__anglr_syntax_production_list_name_optional__1:
					// emit syntax tree node associated with production
					// <anglr syntax production list name optional>: %empty

				break;
				case production_kind.g__anglr_syntax_production_list_name_optional__2:
					// emit syntax tree node associated with production
					// <anglr syntax production list name optional>: <anglr syntax production list name>

					s += m__anglr_syntax_production_list_name_.Emit (depth - 1);
				break;
			}
			return s.Trim ();
		}

		//
		// emit production tree node associated with any production of syntax rule <anglr syntax production list name optional>
		//

		public override string EmitProductionTree (int depth)
		{
			string s = "_anglr_syntax_production_list_name_optional_ (";
			if (depth != 0)
				switch ((production_kind) kind)
				{
				case production_kind.g__anglr_syntax_production_list_name_optional__1:
					// emit syntax tree node associated with production
					// <anglr syntax production list name optional>: %empty

				break;
				case production_kind.g__anglr_syntax_production_list_name_optional__2:
					// emit syntax tree node associated with production
					// <anglr syntax production list name optional>: <anglr syntax production list name>

					s += m__anglr_syntax_production_list_name_.EmitProductionTree (depth - 1);
				break;
			}
			s += ")";
			return s;
		}
		#endregion
		#region Syntax tree traversal,  reperrent, init and iterator methods associated with the syntax rule <anglr syntax production list name optional>

		//
		// traverse sub-tree rooted in this node using selected syntax tree walker
		//

		public override void InvokeTraverse (SyntaxTreeWalkerCore syntaxTreeWalker) => ((SyntaxTreeWalker) syntaxTreeWalker).Traverse (this);
		public override void InvokeTraverseCommon (SyntaxTreeWalkerCore syntaxTreeWalker) => ((SyntaxTreeWalker) syntaxTreeWalker).TraverseCommon (this);

		// reparent sub-tree

		public override void reparent (SyntaxTreeBase parent)
		{
			this.parent = parent;

			m__anglr_syntax_production_list_name_?.reparent (this);
		}

		//
		// initialize object associated with syntax rule <anglr syntax production list name optional>
		//

		public void _init ()
		{
			m__anglr_syntax_production_list_name_ = null;
		}
		#endregion
		#region Fields and properties associated with the syntax rule <anglr syntax production list name optional>

		// counter of all nodes associated with syntax rule <anglr syntax production list name optional>
		public static int g_counter;

		// objects associated with terminal and non-terminal symbols within production(s) of syntax rule <anglr syntax production list name optional>
		public _anglr_syntax_production_list_name_ m__anglr_syntax_production_list_name_ { get; private set; }
		#endregion

	};

	public partial class SyntaxTreeWalker
	{
		#region delegates and events associated with the syntax rule <anglr syntax production list name optional>

		// delegate function (callback) prototype associated with syntax rule <anglr syntax production list name optional>
		public delegate bool _anglr_syntax_production_list_name_optional__Callback (SyntaxTreeCallbackReason reason, _anglr_syntax_production_list_name_optional_.production_kind kind, _anglr_syntax_production_list_name_optional_ p__anglr_syntax_production_list_name_optional_);

		// event associated with syntax rule <anglr syntax production list name optional>
		public event _anglr_syntax_production_list_name_optional__Callback _anglr_syntax_production_list_name_optional__Event;

		// event trigger associated with syntax rule <anglr syntax production list name optional>
		public bool Raise__anglr_syntax_production_list_name_optional__Event (SyntaxTreeCallbackReason reason, _anglr_syntax_production_list_name_optional_.production_kind kind, _anglr_syntax_production_list_name_optional_ p__anglr_syntax_production_list_name_optional_)
		{
			bool? status = _anglr_syntax_production_list_name_optional__Event?.Invoke (reason, kind, p__anglr_syntax_production_list_name_optional_);
			return (status == null) || status.Value;
		}
		#endregion
		#region syntax tree traversal methods associated with the syntax rule <anglr syntax production list name optional>
		//
		// traverse syntax tree node associated with any production of syntax rule <anglr syntax production list name optional>
		//

		public void Traverse (_anglr_syntax_production_list_name_optional_ p__anglr_syntax_production_list_name_optional_)
		{
			if (p__anglr_syntax_production_list_name_optional_.isLocked())
				return;
			p__anglr_syntax_production_list_name_optional_.dolock();
			_anglr_syntax_production_list_name_optional_.production_kind kind = (_anglr_syntax_production_list_name_optional_.production_kind) p__anglr_syntax_production_list_name_optional_.kind;
			p__anglr_syntax_production_list_name_optional_.turn_reset ();
			if (Raise__anglr_syntax_production_list_name_optional__Event (SyntaxTreeCallbackReason.TraversalPrologueCallbackReason, kind, p__anglr_syntax_production_list_name_optional_))
			switch (kind)
			{
				case _anglr_syntax_production_list_name_optional_.production_kind.g__anglr_syntax_production_list_name_optional__1:
					// traverse syntax tree node associated with production
					// <anglr syntax production list name optional>: %empty

				break;
				case _anglr_syntax_production_list_name_optional_.production_kind.g__anglr_syntax_production_list_name_optional__2:
					// traverse syntax tree node associated with production
					// <anglr syntax production list name optional>: <anglr syntax production list name>

					if (Raise__anglr_syntax_production_list_name_optional__Event (SyntaxTreeCallbackReason.TraversalMidTermCallbackReason, kind, p__anglr_syntax_production_list_name_optional_))
						Traverse (p__anglr_syntax_production_list_name_optional_.m__anglr_syntax_production_list_name_);
					p__anglr_syntax_production_list_name_optional_.turn_inc ();
					Raise__anglr_syntax_production_list_name_optional__Event (SyntaxTreeCallbackReason.TraversalMidTermCallbackReason, kind, p__anglr_syntax_production_list_name_optional_);
				break;
			}
			Raise__anglr_syntax_production_list_name_optional__Event (SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason, kind, p__anglr_syntax_production_list_name_optional_);
			p__anglr_syntax_production_list_name_optional_.unlock();
		}

		//
		// traverse syntax tree node associated with any production of syntax rule <anglr syntax production list name optional>
		//

		public void TraverseCommon (_anglr_syntax_production_list_name_optional_ p__anglr_syntax_production_list_name_optional_)
		{
			_anglr_syntax_production_list_name_optional_.production_kind kind = (_anglr_syntax_production_list_name_optional_.production_kind) p__anglr_syntax_production_list_name_optional_.kind;
			if (Raise_Common_Event (SyntaxTreeCallbackReason.TraversalPrologueCallbackReason, (int) kind, p__anglr_syntax_production_list_name_optional_))
			switch (kind)
			{
				case _anglr_syntax_production_list_name_optional_.production_kind.g__anglr_syntax_production_list_name_optional__1:
					// traverse syntax tree node associated with production
					// <anglr syntax production list name optional>: %empty

				break;
				case _anglr_syntax_production_list_name_optional_.production_kind.g__anglr_syntax_production_list_name_optional__2:
					// traverse syntax tree node associated with production
					// <anglr syntax production list name optional>: <anglr syntax production list name>

						TraverseCommon (p__anglr_syntax_production_list_name_optional_.m__anglr_syntax_production_list_name_);
				break;
			}
			Raise_Common_Event (SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason, (int) kind, p__anglr_syntax_production_list_name_optional_);
		}
	};
		#endregion

	public partial class SyntaxTreeBuilder : SyntaxTreeWalker
	{
		#region syntax tree builder methods associated with the syntax rule <anglr syntax production list name optional>

		//
		// create syntax tree node associated with production
		// <anglr syntax production list name optional>: %empty

		//

		public _anglr_syntax_production_list_name_optional_ _anglr_syntax_production_list_name_optional__1 ()
		{
			_anglr_syntax_production_list_name_optional_ p__anglr_syntax_production_list_name_optional__ref = new _anglr_syntax_production_list_name_optional_();
			Raise__anglr_syntax_production_list_name_optional__Event (SyntaxTreeCallbackReason.BuilderCallbackReason, _anglr_syntax_production_list_name_optional_.production_kind.g__anglr_syntax_production_list_name_optional__1, p__anglr_syntax_production_list_name_optional__ref);
			return p__anglr_syntax_production_list_name_optional__ref;
		}

		//
		// create syntax tree node associated with production
		// <anglr syntax production list name optional>: <anglr syntax production list name>

		//

		public _anglr_syntax_production_list_name_optional_ _anglr_syntax_production_list_name_optional__2 (_anglr_syntax_production_list_name_ p__anglr_syntax_production_list_name_)
		{
			_anglr_syntax_production_list_name_optional_ p__anglr_syntax_production_list_name_optional__ref = new _anglr_syntax_production_list_name_optional_(p__anglr_syntax_production_list_name_);
			p__anglr_syntax_production_list_name_.parent = p__anglr_syntax_production_list_name_optional__ref;
			Raise__anglr_syntax_production_list_name_optional__Event (SyntaxTreeCallbackReason.BuilderCallbackReason, _anglr_syntax_production_list_name_optional_.production_kind.g__anglr_syntax_production_list_name_optional__2, p__anglr_syntax_production_list_name_optional__ref);
			return p__anglr_syntax_production_list_name_optional__ref;
		}
		#endregion
	};
}
