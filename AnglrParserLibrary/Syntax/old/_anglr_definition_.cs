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
	// class associated with syntax rule <anglr definition>
	//

	public class	_anglr_definition_ : SyntaxTreeBase
	{
		#region enumerated production(s) of syntax rule <anglr definition>
		public enum production_kind : ushort	// enumerated production(s) of syntax rule <anglr definition>
		{
			g__anglr_definition__1 = 1,	// <single terminal definition>

			g__anglr_definition__2 = 2,	// <single regex definition>

			g__anglr_definition__3 = 3,	// <block of terminal definitions>

			g__anglr_definition__4 = 4,	// <block of regex definitions>

		};
		#endregion
		#region production markers associated with the syntax rule <anglr definition>

		// markers associated with production: <anglr definition> -> <single terminal definition>

		public enum production_marker_1 : ushort
		{
			m__single_terminal_definition_,
			final
		};

		// markers associated with production: <anglr definition> -> <single regex definition>

		public enum production_marker_2 : ushort
		{
			m__single_regex_definition_,
			final
		};

		// markers associated with production: <anglr definition> -> <block of terminal definitions>

		public enum production_marker_3 : ushort
		{
			m__block_of_terminal_definitions_,
			final
		};

		// markers associated with production: <anglr definition> -> <block of regex definitions>

		public enum production_marker_4 : ushort
		{
			m__block_of_regex_definitions_,
			final
		};
		#endregion
		#region Constructors and destructors associated with the syntax rule <anglr definition>

		// Constructor declaration(s) associated with production(s) of syntax rule <anglr definition>

		//
		// Constructor associated with the following production(s)
		// <anglr definition> -> <single terminal definition>

		//

		public _anglr_definition_ (_single_terminal_definition_ p__single_terminal_definition_) : base ((uint) ProductionID.__anglr_definition__ID, (uint) production_kind.g__anglr_definition__1)
		{
			++g_counter;
			_init ();
			children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 1);
			children[0] = m__single_terminal_definition_ = p__single_terminal_definition_;
		}

		//
		// Constructor associated with the following production(s)
		// <anglr definition> -> <single regex definition>

		//

		public _anglr_definition_ (_single_regex_definition_ p__single_regex_definition_) : base ((uint) ProductionID.__anglr_definition__ID, (uint) production_kind.g__anglr_definition__2)
		{
			++g_counter;
			_init ();
			children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 1);
			children[0] = m__single_regex_definition_ = p__single_regex_definition_;
		}

		//
		// Constructor associated with the following production(s)
		// <anglr definition> -> <block of terminal definitions>

		//

		public _anglr_definition_ (_block_of_terminal_definitions_ p__block_of_terminal_definitions_) : base ((uint) ProductionID.__anglr_definition__ID, (uint) production_kind.g__anglr_definition__3)
		{
			++g_counter;
			_init ();
			children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 1);
			children[0] = m__block_of_terminal_definitions_ = p__block_of_terminal_definitions_;
		}

		//
		// Constructor associated with the following production(s)
		// <anglr definition> -> <block of regex definitions>

		//

		public _anglr_definition_ (_block_of_regex_definitions_ p__block_of_regex_definitions_) : base ((uint) ProductionID.__anglr_definition__ID, (uint) production_kind.g__anglr_definition__4)
		{
			++g_counter;
			_init ();
			children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 1);
			children[0] = m__block_of_regex_definitions_ = p__block_of_regex_definitions_;
		}

		// Copy constructor

		public _anglr_definition_ (_anglr_definition_ p__anglr_definition_) : base (p__anglr_definition_.id, p__anglr_definition_.kind)
		{
			++g_counter;
			_init ();
			switch ((production_kind) p__anglr_definition_.kind)
			{
				case production_kind.g__anglr_definition__1:
					children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 1);
					if ((children [0] = m__single_terminal_definition_ = (p__anglr_definition_.m__single_terminal_definition_ != null) ? new _single_terminal_definition_ (p__anglr_definition_.m__single_terminal_definition_) : null) != null) m__single_terminal_definition_.parent = this;
					break;
				case production_kind.g__anglr_definition__2:
					children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 1);
					if ((children [0] = m__single_regex_definition_ = (p__anglr_definition_.m__single_regex_definition_ != null) ? new _single_regex_definition_ (p__anglr_definition_.m__single_regex_definition_) : null) != null) m__single_regex_definition_.parent = this;
					break;
				case production_kind.g__anglr_definition__3:
					children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 1);
					if ((children [0] = m__block_of_terminal_definitions_ = (p__anglr_definition_.m__block_of_terminal_definitions_ != null) ? new _block_of_terminal_definitions_ (p__anglr_definition_.m__block_of_terminal_definitions_) : null) != null) m__block_of_terminal_definitions_.parent = this;
					break;
				case production_kind.g__anglr_definition__4:
					children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 1);
					if ((children [0] = m__block_of_regex_definitions_ = (p__anglr_definition_.m__block_of_regex_definitions_ != null) ? new _block_of_regex_definitions_ (p__anglr_definition_.m__block_of_regex_definitions_) : null) != null) m__block_of_regex_definitions_.parent = this;
					break;
				default:
				{
					string[] args = new string[] { "_anglr_definition_" };
					throw new SyntaxTreeError (SyntaxTreeError.InvalidKindError, args);
				}
			}
		}

		// Clone node - wrapper of copy constructor

		public override SyntaxTreeBase Clone () { return (SyntaxTreeBase) new _anglr_definition_ (this); }

		// Destructor

		public new void Dispose ()
		{
			base.Dispose ();
			--g_counter;

			m__single_terminal_definition_?.Dispose ();
			m__single_regex_definition_?.Dispose ();
			m__block_of_terminal_definitions_?.Dispose ();
			m__block_of_regex_definitions_?.Dispose ();

			_init ();
		}
		#endregion
		#region Content changing and inclusion checking methods associated with the syntax rule <anglr definition>

		// Content changing function(s) associated with production(s) of syntax rule <anglr definition>

		//
		// Content changing function associated with following production(s)
		// <anglr definition> -> <single terminal definition>

		//

		public void change(_single_terminal_definition_ p__single_terminal_definition_)
		{
			_init ();
			this.kind = (uint) production_kind.g__anglr_definition__1;
			children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 1);
			children [0] = m__single_terminal_definition_ = p__single_terminal_definition_;
			reparent (parent);
		}

		//
		// Content changing function associated with following production(s)
		// <anglr definition> -> <single regex definition>

		//

		public void change(_single_regex_definition_ p__single_regex_definition_)
		{
			_init ();
			this.kind = (uint) production_kind.g__anglr_definition__2;
			children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 1);
			children [0] = m__single_regex_definition_ = p__single_regex_definition_;
			reparent (parent);
		}

		//
		// Content changing function associated with following production(s)
		// <anglr definition> -> <block of terminal definitions>

		//

		public void change(_block_of_terminal_definitions_ p__block_of_terminal_definitions_)
		{
			_init ();
			this.kind = (uint) production_kind.g__anglr_definition__3;
			children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 1);
			children [0] = m__block_of_terminal_definitions_ = p__block_of_terminal_definitions_;
			reparent (parent);
		}

		//
		// Content changing function associated with following production(s)
		// <anglr definition> -> <block of regex definitions>

		//

		public void change(_block_of_regex_definitions_ p__block_of_regex_definitions_)
		{
			_init ();
			this.kind = (uint) production_kind.g__anglr_definition__4;
			children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 1);
			children [0] = m__block_of_regex_definitions_ = p__block_of_regex_definitions_;
			reparent (parent);
		}

		//
		// check if node 'element' is included within syntax tree associated with syntax rule <anglr definition>
		//

		public new bool checkInclusion (SyntaxTreeBase element)
		{
			return
				(element == this) ||
				(m__single_terminal_definition_ != null) && m__single_terminal_definition_.checkInclusion (element) ||
				(m__single_regex_definition_ != null) && m__single_regex_definition_.checkInclusion (element) ||
				(m__block_of_terminal_definitions_ != null) && m__block_of_terminal_definitions_.checkInclusion (element) ||
				(m__block_of_regex_definitions_ != null) && m__block_of_regex_definitions_.checkInclusion (element) ||
				false;
		}
		#endregion
		#region Syntax tree emit methods associated with the syntax rule <anglr definition>

		//
		// emit production tree node associated with any production of syntax rule <anglr definition>
		//

		public override string Emit (int depth)
		{
			string s = "";
			if (depth != 0)
			switch ((production_kind) kind)
			{
				case production_kind.g__anglr_definition__1:
					// emit syntax tree node associated with production
					// <anglr definition>: <single terminal definition>

					s += m__single_terminal_definition_.Emit (depth - 1);
				break;
				case production_kind.g__anglr_definition__2:
					// emit syntax tree node associated with production
					// <anglr definition>: <single regex definition>

					s += m__single_regex_definition_.Emit (depth - 1);
				break;
				case production_kind.g__anglr_definition__3:
					// emit syntax tree node associated with production
					// <anglr definition>: <block of terminal definitions>

					s += m__block_of_terminal_definitions_.Emit (depth - 1);
				break;
				case production_kind.g__anglr_definition__4:
					// emit syntax tree node associated with production
					// <anglr definition>: <block of regex definitions>

					s += m__block_of_regex_definitions_.Emit (depth - 1);
				break;
			}
			return s.Trim ();
		}

		//
		// emit production tree node associated with any production of syntax rule <anglr definition>
		//

		public override string EmitProductionTree (int depth)
		{
			string s = "_anglr_definition_ (";
			if (depth != 0)
				switch ((production_kind) kind)
				{
				case production_kind.g__anglr_definition__1:
					// emit syntax tree node associated with production
					// <anglr definition>: <single terminal definition>

					s += m__single_terminal_definition_.EmitProductionTree (depth - 1);
				break;
				case production_kind.g__anglr_definition__2:
					// emit syntax tree node associated with production
					// <anglr definition>: <single regex definition>

					s += m__single_regex_definition_.EmitProductionTree (depth - 1);
				break;
				case production_kind.g__anglr_definition__3:
					// emit syntax tree node associated with production
					// <anglr definition>: <block of terminal definitions>

					s += m__block_of_terminal_definitions_.EmitProductionTree (depth - 1);
				break;
				case production_kind.g__anglr_definition__4:
					// emit syntax tree node associated with production
					// <anglr definition>: <block of regex definitions>

					s += m__block_of_regex_definitions_.EmitProductionTree (depth - 1);
				break;
			}
			s += ")";
			return s;
		}
		#endregion
		#region Syntax tree traversal,  reperrent, init and iterator methods associated with the syntax rule <anglr definition>

		//
		// traverse sub-tree rooted in this node using selected syntax tree walker
		//

		public override void InvokeTraverse (SyntaxTreeWalkerCore syntaxTreeWalker) => ((SyntaxTreeWalker) syntaxTreeWalker).Traverse (this);
		public override void InvokeTraverseCommon (SyntaxTreeWalkerCore syntaxTreeWalker) => ((SyntaxTreeWalker) syntaxTreeWalker).TraverseCommon (this);

		// reparent sub-tree

		public override void reparent (SyntaxTreeBase parent)
		{
			this.parent = parent;

			m__single_terminal_definition_?.reparent (this);
			m__single_regex_definition_?.reparent (this);
			m__block_of_terminal_definitions_?.reparent (this);
			m__block_of_regex_definitions_?.reparent (this);
		}

		//
		// initialize object associated with syntax rule <anglr definition>
		//

		public void _init ()
		{
			m__single_terminal_definition_ = null;
			m__single_regex_definition_ = null;
			m__block_of_terminal_definitions_ = null;
			m__block_of_regex_definitions_ = null;
		}
		#endregion
		#region Fields and properties associated with the syntax rule <anglr definition>

		// counter of all nodes associated with syntax rule <anglr definition>
		public static int g_counter;

		// objects associated with terminal and non-terminal symbols within production(s) of syntax rule <anglr definition>
		public _single_terminal_definition_ m__single_terminal_definition_ { get; private set; }
		public _single_regex_definition_ m__single_regex_definition_ { get; private set; }
		public _block_of_terminal_definitions_ m__block_of_terminal_definitions_ { get; private set; }
		public _block_of_regex_definitions_ m__block_of_regex_definitions_ { get; private set; }
		#endregion

	};

	public partial class SyntaxTreeWalker
	{
		#region delegates and events associated with the syntax rule <anglr definition>

		// delegate function (callback) prototype associated with syntax rule <anglr definition>
		public delegate bool _anglr_definition__Callback (SyntaxTreeCallbackReason reason, _anglr_definition_.production_kind kind, _anglr_definition_ p__anglr_definition_);

		// event associated with syntax rule <anglr definition>
		public event _anglr_definition__Callback _anglr_definition__Event;

		// event trigger associated with syntax rule <anglr definition>
		public bool Raise__anglr_definition__Event (SyntaxTreeCallbackReason reason, _anglr_definition_.production_kind kind, _anglr_definition_ p__anglr_definition_)
		{
			bool? status = _anglr_definition__Event?.Invoke (reason, kind, p__anglr_definition_);
			return (status == null) || status.Value;
		}
		#endregion
		#region syntax tree traversal methods associated with the syntax rule <anglr definition>
		//
		// traverse syntax tree node associated with any production of syntax rule <anglr definition>
		//

		public void Traverse (_anglr_definition_ p__anglr_definition_)
		{
			if (p__anglr_definition_.isLocked())
				return;
			p__anglr_definition_.dolock();
			_anglr_definition_.production_kind kind = (_anglr_definition_.production_kind) p__anglr_definition_.kind;
			p__anglr_definition_.turn_reset ();
			if (Raise__anglr_definition__Event (SyntaxTreeCallbackReason.TraversalPrologueCallbackReason, kind, p__anglr_definition_))
			switch (kind)
			{
				case _anglr_definition_.production_kind.g__anglr_definition__1:
					// traverse syntax tree node associated with production
					// <anglr definition>: <single terminal definition>

					if (Raise__anglr_definition__Event (SyntaxTreeCallbackReason.TraversalMidTermCallbackReason, kind, p__anglr_definition_))
						Traverse (p__anglr_definition_.m__single_terminal_definition_);
					p__anglr_definition_.turn_inc ();
					Raise__anglr_definition__Event (SyntaxTreeCallbackReason.TraversalMidTermCallbackReason, kind, p__anglr_definition_);
				break;
				case _anglr_definition_.production_kind.g__anglr_definition__2:
					// traverse syntax tree node associated with production
					// <anglr definition>: <single regex definition>

					if (Raise__anglr_definition__Event (SyntaxTreeCallbackReason.TraversalMidTermCallbackReason, kind, p__anglr_definition_))
						Traverse (p__anglr_definition_.m__single_regex_definition_);
					p__anglr_definition_.turn_inc ();
					Raise__anglr_definition__Event (SyntaxTreeCallbackReason.TraversalMidTermCallbackReason, kind, p__anglr_definition_);
				break;
				case _anglr_definition_.production_kind.g__anglr_definition__3:
					// traverse syntax tree node associated with production
					// <anglr definition>: <block of terminal definitions>

					if (Raise__anglr_definition__Event (SyntaxTreeCallbackReason.TraversalMidTermCallbackReason, kind, p__anglr_definition_))
						Traverse (p__anglr_definition_.m__block_of_terminal_definitions_);
					p__anglr_definition_.turn_inc ();
					Raise__anglr_definition__Event (SyntaxTreeCallbackReason.TraversalMidTermCallbackReason, kind, p__anglr_definition_);
				break;
				case _anglr_definition_.production_kind.g__anglr_definition__4:
					// traverse syntax tree node associated with production
					// <anglr definition>: <block of regex definitions>

					if (Raise__anglr_definition__Event (SyntaxTreeCallbackReason.TraversalMidTermCallbackReason, kind, p__anglr_definition_))
						Traverse (p__anglr_definition_.m__block_of_regex_definitions_);
					p__anglr_definition_.turn_inc ();
					Raise__anglr_definition__Event (SyntaxTreeCallbackReason.TraversalMidTermCallbackReason, kind, p__anglr_definition_);
				break;
			}
			Raise__anglr_definition__Event (SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason, kind, p__anglr_definition_);
			p__anglr_definition_.unlock();
		}

		//
		// traverse syntax tree node associated with any production of syntax rule <anglr definition>
		//

		public void TraverseCommon (_anglr_definition_ p__anglr_definition_)
		{
			_anglr_definition_.production_kind kind = (_anglr_definition_.production_kind) p__anglr_definition_.kind;
			if (Raise_Common_Event (SyntaxTreeCallbackReason.TraversalPrologueCallbackReason, (int) kind, p__anglr_definition_))
			switch (kind)
			{
				case _anglr_definition_.production_kind.g__anglr_definition__1:
					// traverse syntax tree node associated with production
					// <anglr definition>: <single terminal definition>

						TraverseCommon (p__anglr_definition_.m__single_terminal_definition_);
				break;
				case _anglr_definition_.production_kind.g__anglr_definition__2:
					// traverse syntax tree node associated with production
					// <anglr definition>: <single regex definition>

						TraverseCommon (p__anglr_definition_.m__single_regex_definition_);
				break;
				case _anglr_definition_.production_kind.g__anglr_definition__3:
					// traverse syntax tree node associated with production
					// <anglr definition>: <block of terminal definitions>

						TraverseCommon (p__anglr_definition_.m__block_of_terminal_definitions_);
				break;
				case _anglr_definition_.production_kind.g__anglr_definition__4:
					// traverse syntax tree node associated with production
					// <anglr definition>: <block of regex definitions>

						TraverseCommon (p__anglr_definition_.m__block_of_regex_definitions_);
				break;
			}
			Raise_Common_Event (SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason, (int) kind, p__anglr_definition_);
		}
	};
		#endregion

	public partial class SyntaxTreeBuilder : SyntaxTreeWalker
	{
		#region syntax tree builder methods associated with the syntax rule <anglr definition>

		//
		// create syntax tree node associated with production
		// <anglr definition>: <single terminal definition>

		//

		public _anglr_definition_ _anglr_definition__1 (_single_terminal_definition_ p__single_terminal_definition_)
		{
			_anglr_definition_ p__anglr_definition__ref = new _anglr_definition_(p__single_terminal_definition_);
			p__single_terminal_definition_.parent = p__anglr_definition__ref;
			Raise__anglr_definition__Event (SyntaxTreeCallbackReason.BuilderCallbackReason, _anglr_definition_.production_kind.g__anglr_definition__1, p__anglr_definition__ref);
			return p__anglr_definition__ref;
		}

		//
		// create syntax tree node associated with production
		// <anglr definition>: <single regex definition>

		//

		public _anglr_definition_ _anglr_definition__2 (_single_regex_definition_ p__single_regex_definition_)
		{
			_anglr_definition_ p__anglr_definition__ref = new _anglr_definition_(p__single_regex_definition_);
			p__single_regex_definition_.parent = p__anglr_definition__ref;
			Raise__anglr_definition__Event (SyntaxTreeCallbackReason.BuilderCallbackReason, _anglr_definition_.production_kind.g__anglr_definition__2, p__anglr_definition__ref);
			return p__anglr_definition__ref;
		}

		//
		// create syntax tree node associated with production
		// <anglr definition>: <block of terminal definitions>

		//

		public _anglr_definition_ _anglr_definition__3 (_block_of_terminal_definitions_ p__block_of_terminal_definitions_)
		{
			_anglr_definition_ p__anglr_definition__ref = new _anglr_definition_(p__block_of_terminal_definitions_);
			p__block_of_terminal_definitions_.parent = p__anglr_definition__ref;
			Raise__anglr_definition__Event (SyntaxTreeCallbackReason.BuilderCallbackReason, _anglr_definition_.production_kind.g__anglr_definition__3, p__anglr_definition__ref);
			return p__anglr_definition__ref;
		}

		//
		// create syntax tree node associated with production
		// <anglr definition>: <block of regex definitions>

		//

		public _anglr_definition_ _anglr_definition__4 (_block_of_regex_definitions_ p__block_of_regex_definitions_)
		{
			_anglr_definition_ p__anglr_definition__ref = new _anglr_definition_(p__block_of_regex_definitions_);
			p__block_of_regex_definitions_.parent = p__anglr_definition__ref;
			Raise__anglr_definition__Event (SyntaxTreeCallbackReason.BuilderCallbackReason, _anglr_definition_.production_kind.g__anglr_definition__4, p__anglr_definition__ref);
			return p__anglr_definition__ref;
		}
		#endregion
	};
}
