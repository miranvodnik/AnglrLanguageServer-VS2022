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
	// class associated with syntax rule <anglr file part>
	//

	public class	_anglr_file_part_ : SyntaxTreeBase
	{
		#region enumerated production(s) of syntax rule <anglr file part>
		public enum production_kind : ushort	// enumerated production(s) of syntax rule <anglr file part>
		{
			g__anglr_file_part__1 = 1,	// <general part>

			g__anglr_file_part__2 = 2,	// <declaration part>

			g__anglr_file_part__3 = 3,	// <scanner part>

			g__anglr_file_part__4 = 4,	// <lexer part>

			g__anglr_file_part__5 = 5,	// <parser part>

		};
		#endregion
		#region production markers associated with the syntax rule <anglr file part>

		// markers associated with production: <anglr file part> -> <general part>

		public enum production_marker_1 : ushort
		{
			m__general_part_,
			final
		};

		// markers associated with production: <anglr file part> -> <declaration part>

		public enum production_marker_2 : ushort
		{
			m__declaration_part_,
			final
		};

		// markers associated with production: <anglr file part> -> <scanner part>

		public enum production_marker_3 : ushort
		{
			m__scanner_part_,
			final
		};

		// markers associated with production: <anglr file part> -> <lexer part>

		public enum production_marker_4 : ushort
		{
			m__lexer_part_,
			final
		};

		// markers associated with production: <anglr file part> -> <parser part>

		public enum production_marker_5 : ushort
		{
			m__parser_part_,
			final
		};
		#endregion
		#region Constructors and destructors associated with the syntax rule <anglr file part>

		// Constructor declaration(s) associated with production(s) of syntax rule <anglr file part>

		//
		// Constructor associated with the following production(s)
		// <anglr file part> -> <general part>

		//

		public _anglr_file_part_ (_general_part_ p__general_part_) : base ((uint) ProductionID.__anglr_file_part__ID, (uint) production_kind.g__anglr_file_part__1)
		{
			++g_counter;
			_init ();
			children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 1);
			children[0] = m__general_part_ = p__general_part_;
		}

		//
		// Constructor associated with the following production(s)
		// <anglr file part> -> <declaration part>

		//

		public _anglr_file_part_ (_declaration_part_ p__declaration_part_) : base ((uint) ProductionID.__anglr_file_part__ID, (uint) production_kind.g__anglr_file_part__2)
		{
			++g_counter;
			_init ();
			children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 1);
			children[0] = m__declaration_part_ = p__declaration_part_;
		}

		//
		// Constructor associated with the following production(s)
		// <anglr file part> -> <scanner part>

		//

		public _anglr_file_part_ (_scanner_part_ p__scanner_part_) : base ((uint) ProductionID.__anglr_file_part__ID, (uint) production_kind.g__anglr_file_part__3)
		{
			++g_counter;
			_init ();
			children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 1);
			children[0] = m__scanner_part_ = p__scanner_part_;
		}

		//
		// Constructor associated with the following production(s)
		// <anglr file part> -> <lexer part>

		//

		public _anglr_file_part_ (_lexer_part_ p__lexer_part_) : base ((uint) ProductionID.__anglr_file_part__ID, (uint) production_kind.g__anglr_file_part__4)
		{
			++g_counter;
			_init ();
			children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 1);
			children[0] = m__lexer_part_ = p__lexer_part_;
		}

		//
		// Constructor associated with the following production(s)
		// <anglr file part> -> <parser part>

		//

		public _anglr_file_part_ (_parser_part_ p__parser_part_) : base ((uint) ProductionID.__anglr_file_part__ID, (uint) production_kind.g__anglr_file_part__5)
		{
			++g_counter;
			_init ();
			children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 1);
			children[0] = m__parser_part_ = p__parser_part_;
		}

		// Copy constructor

		public _anglr_file_part_ (_anglr_file_part_ p__anglr_file_part_) : base (p__anglr_file_part_.id, p__anglr_file_part_.kind)
		{
			++g_counter;
			_init ();
			switch ((production_kind) p__anglr_file_part_.kind)
			{
				case production_kind.g__anglr_file_part__1:
					children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 1);
					if ((children [0] = m__general_part_ = (p__anglr_file_part_.m__general_part_ != null) ? new _general_part_ (p__anglr_file_part_.m__general_part_) : null) != null) m__general_part_.parent = this;
					break;
				case production_kind.g__anglr_file_part__2:
					children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 1);
					if ((children [0] = m__declaration_part_ = (p__anglr_file_part_.m__declaration_part_ != null) ? new _declaration_part_ (p__anglr_file_part_.m__declaration_part_) : null) != null) m__declaration_part_.parent = this;
					break;
				case production_kind.g__anglr_file_part__3:
					children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 1);
					if ((children [0] = m__scanner_part_ = (p__anglr_file_part_.m__scanner_part_ != null) ? new _scanner_part_ (p__anglr_file_part_.m__scanner_part_) : null) != null) m__scanner_part_.parent = this;
					break;
				case production_kind.g__anglr_file_part__4:
					children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 1);
					if ((children [0] = m__lexer_part_ = (p__anglr_file_part_.m__lexer_part_ != null) ? new _lexer_part_ (p__anglr_file_part_.m__lexer_part_) : null) != null) m__lexer_part_.parent = this;
					break;
				case production_kind.g__anglr_file_part__5:
					children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 1);
					if ((children [0] = m__parser_part_ = (p__anglr_file_part_.m__parser_part_ != null) ? new _parser_part_ (p__anglr_file_part_.m__parser_part_) : null) != null) m__parser_part_.parent = this;
					break;
				default:
				{
					string[] args = new string[] { "_anglr_file_part_" };
					throw new SyntaxTreeError (SyntaxTreeError.InvalidKindError, args);
				}
			}
		}

		// Clone node - wrapper of copy constructor

		public override SyntaxTreeBase Clone () { return (SyntaxTreeBase) new _anglr_file_part_ (this); }

		// Destructor

		public new void Dispose ()
		{
			base.Dispose ();
			--g_counter;

			m__general_part_?.Dispose ();
			m__declaration_part_?.Dispose ();
			m__scanner_part_?.Dispose ();
			m__lexer_part_?.Dispose ();
			m__parser_part_?.Dispose ();

			_init ();
		}
		#endregion
		#region Content changing and inclusion checking methods associated with the syntax rule <anglr file part>

		// Content changing function(s) associated with production(s) of syntax rule <anglr file part>

		//
		// Content changing function associated with following production(s)
		// <anglr file part> -> <general part>

		//

		public void change(_general_part_ p__general_part_)
		{
			_init ();
			this.kind = (uint) production_kind.g__anglr_file_part__1;
			children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 1);
			children [0] = m__general_part_ = p__general_part_;
			reparent (parent);
		}

		//
		// Content changing function associated with following production(s)
		// <anglr file part> -> <declaration part>

		//

		public void change(_declaration_part_ p__declaration_part_)
		{
			_init ();
			this.kind = (uint) production_kind.g__anglr_file_part__2;
			children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 1);
			children [0] = m__declaration_part_ = p__declaration_part_;
			reparent (parent);
		}

		//
		// Content changing function associated with following production(s)
		// <anglr file part> -> <scanner part>

		//

		public void change(_scanner_part_ p__scanner_part_)
		{
			_init ();
			this.kind = (uint) production_kind.g__anglr_file_part__3;
			children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 1);
			children [0] = m__scanner_part_ = p__scanner_part_;
			reparent (parent);
		}

		//
		// Content changing function associated with following production(s)
		// <anglr file part> -> <lexer part>

		//

		public void change(_lexer_part_ p__lexer_part_)
		{
			_init ();
			this.kind = (uint) production_kind.g__anglr_file_part__4;
			children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 1);
			children [0] = m__lexer_part_ = p__lexer_part_;
			reparent (parent);
		}

		//
		// Content changing function associated with following production(s)
		// <anglr file part> -> <parser part>

		//

		public void change(_parser_part_ p__parser_part_)
		{
			_init ();
			this.kind = (uint) production_kind.g__anglr_file_part__5;
			children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 1);
			children [0] = m__parser_part_ = p__parser_part_;
			reparent (parent);
		}

		//
		// check if node 'element' is included within syntax tree associated with syntax rule <anglr file part>
		//

		public new bool checkInclusion (SyntaxTreeBase element)
		{
			return
				(element == this) ||
				(m__general_part_ != null) && m__general_part_.checkInclusion (element) ||
				(m__declaration_part_ != null) && m__declaration_part_.checkInclusion (element) ||
				(m__scanner_part_ != null) && m__scanner_part_.checkInclusion (element) ||
				(m__lexer_part_ != null) && m__lexer_part_.checkInclusion (element) ||
				(m__parser_part_ != null) && m__parser_part_.checkInclusion (element) ||
				false;
		}
		#endregion
		#region Syntax tree emit methods associated with the syntax rule <anglr file part>

		//
		// emit production tree node associated with any production of syntax rule <anglr file part>
		//

		public override string Emit (int depth)
		{
			string s = "";
			if (depth != 0)
			switch ((production_kind) kind)
			{
				case production_kind.g__anglr_file_part__1:
					// emit syntax tree node associated with production
					// <anglr file part>: <general part>

					s += m__general_part_.Emit (depth - 1);
				break;
				case production_kind.g__anglr_file_part__2:
					// emit syntax tree node associated with production
					// <anglr file part>: <declaration part>

					s += m__declaration_part_.Emit (depth - 1);
				break;
				case production_kind.g__anglr_file_part__3:
					// emit syntax tree node associated with production
					// <anglr file part>: <scanner part>

					s += m__scanner_part_.Emit (depth - 1);
				break;
				case production_kind.g__anglr_file_part__4:
					// emit syntax tree node associated with production
					// <anglr file part>: <lexer part>

					s += m__lexer_part_.Emit (depth - 1);
				break;
				case production_kind.g__anglr_file_part__5:
					// emit syntax tree node associated with production
					// <anglr file part>: <parser part>

					s += m__parser_part_.Emit (depth - 1);
				break;
			}
			return s.Trim ();
		}

		//
		// emit production tree node associated with any production of syntax rule <anglr file part>
		//

		public override string EmitProductionTree (int depth)
		{
			string s = "_anglr_file_part_ (";
			if (depth != 0)
				switch ((production_kind) kind)
				{
				case production_kind.g__anglr_file_part__1:
					// emit syntax tree node associated with production
					// <anglr file part>: <general part>

					s += m__general_part_.EmitProductionTree (depth - 1);
				break;
				case production_kind.g__anglr_file_part__2:
					// emit syntax tree node associated with production
					// <anglr file part>: <declaration part>

					s += m__declaration_part_.EmitProductionTree (depth - 1);
				break;
				case production_kind.g__anglr_file_part__3:
					// emit syntax tree node associated with production
					// <anglr file part>: <scanner part>

					s += m__scanner_part_.EmitProductionTree (depth - 1);
				break;
				case production_kind.g__anglr_file_part__4:
					// emit syntax tree node associated with production
					// <anglr file part>: <lexer part>

					s += m__lexer_part_.EmitProductionTree (depth - 1);
				break;
				case production_kind.g__anglr_file_part__5:
					// emit syntax tree node associated with production
					// <anglr file part>: <parser part>

					s += m__parser_part_.EmitProductionTree (depth - 1);
				break;
			}
			s += ")";
			return s;
		}
		#endregion
		#region Syntax tree traversal,  reperrent, init and iterator methods associated with the syntax rule <anglr file part>

		//
		// traverse sub-tree rooted in this node using selected syntax tree walker
		//

		public override void InvokeTraverse (SyntaxTreeWalkerCore syntaxTreeWalker) => ((SyntaxTreeWalker) syntaxTreeWalker).Traverse (this);
		public override void InvokeTraverseCommon (SyntaxTreeWalkerCore syntaxTreeWalker) => ((SyntaxTreeWalker) syntaxTreeWalker).TraverseCommon (this);

		// reparent sub-tree

		public override void reparent (SyntaxTreeBase parent)
		{
			this.parent = parent;

			m__general_part_?.reparent (this);
			m__declaration_part_?.reparent (this);
			m__scanner_part_?.reparent (this);
			m__lexer_part_?.reparent (this);
			m__parser_part_?.reparent (this);
		}

		//
		// initialize object associated with syntax rule <anglr file part>
		//

		public void _init ()
		{
			m__general_part_ = null;
			m__declaration_part_ = null;
			m__scanner_part_ = null;
			m__lexer_part_ = null;
			m__parser_part_ = null;
		}
		#endregion
		#region Fields and properties associated with the syntax rule <anglr file part>

		// counter of all nodes associated with syntax rule <anglr file part>
		public static int g_counter;

		// objects associated with terminal and non-terminal symbols within production(s) of syntax rule <anglr file part>
		public _general_part_ m__general_part_ { get; private set; }
		public _declaration_part_ m__declaration_part_ { get; private set; }
		public _scanner_part_ m__scanner_part_ { get; private set; }
		public _lexer_part_ m__lexer_part_ { get; private set; }
		public _parser_part_ m__parser_part_ { get; private set; }
		#endregion

	};

	public partial class SyntaxTreeWalker
	{
		#region delegates and events associated with the syntax rule <anglr file part>

		// delegate function (callback) prototype associated with syntax rule <anglr file part>
		public delegate bool _anglr_file_part__Callback (SyntaxTreeCallbackReason reason, _anglr_file_part_.production_kind kind, _anglr_file_part_ p__anglr_file_part_);

		// event associated with syntax rule <anglr file part>
		public event _anglr_file_part__Callback _anglr_file_part__Event;

		// event trigger associated with syntax rule <anglr file part>
		public bool Raise__anglr_file_part__Event (SyntaxTreeCallbackReason reason, _anglr_file_part_.production_kind kind, _anglr_file_part_ p__anglr_file_part_)
		{
			bool? status = _anglr_file_part__Event?.Invoke (reason, kind, p__anglr_file_part_);
			return (status == null) || status.Value;
		}
		#endregion
		#region syntax tree traversal methods associated with the syntax rule <anglr file part>
		//
		// traverse syntax tree node associated with any production of syntax rule <anglr file part>
		//

		public void Traverse (_anglr_file_part_ p__anglr_file_part_)
		{
			if (p__anglr_file_part_.isLocked())
				return;
			p__anglr_file_part_.dolock();
			_anglr_file_part_.production_kind kind = (_anglr_file_part_.production_kind) p__anglr_file_part_.kind;
			p__anglr_file_part_.turn_reset ();
			if (Raise__anglr_file_part__Event (SyntaxTreeCallbackReason.TraversalPrologueCallbackReason, kind, p__anglr_file_part_))
			switch (kind)
			{
				case _anglr_file_part_.production_kind.g__anglr_file_part__1:
					// traverse syntax tree node associated with production
					// <anglr file part>: <general part>

					if (Raise__anglr_file_part__Event (SyntaxTreeCallbackReason.TraversalMidTermCallbackReason, kind, p__anglr_file_part_))
						Traverse (p__anglr_file_part_.m__general_part_);
					p__anglr_file_part_.turn_inc ();
					Raise__anglr_file_part__Event (SyntaxTreeCallbackReason.TraversalMidTermCallbackReason, kind, p__anglr_file_part_);
				break;
				case _anglr_file_part_.production_kind.g__anglr_file_part__2:
					// traverse syntax tree node associated with production
					// <anglr file part>: <declaration part>

					if (Raise__anglr_file_part__Event (SyntaxTreeCallbackReason.TraversalMidTermCallbackReason, kind, p__anglr_file_part_))
						Traverse (p__anglr_file_part_.m__declaration_part_);
					p__anglr_file_part_.turn_inc ();
					Raise__anglr_file_part__Event (SyntaxTreeCallbackReason.TraversalMidTermCallbackReason, kind, p__anglr_file_part_);
				break;
				case _anglr_file_part_.production_kind.g__anglr_file_part__3:
					// traverse syntax tree node associated with production
					// <anglr file part>: <scanner part>

					if (Raise__anglr_file_part__Event (SyntaxTreeCallbackReason.TraversalMidTermCallbackReason, kind, p__anglr_file_part_))
						Traverse (p__anglr_file_part_.m__scanner_part_);
					p__anglr_file_part_.turn_inc ();
					Raise__anglr_file_part__Event (SyntaxTreeCallbackReason.TraversalMidTermCallbackReason, kind, p__anglr_file_part_);
				break;
				case _anglr_file_part_.production_kind.g__anglr_file_part__4:
					// traverse syntax tree node associated with production
					// <anglr file part>: <lexer part>

					if (Raise__anglr_file_part__Event (SyntaxTreeCallbackReason.TraversalMidTermCallbackReason, kind, p__anglr_file_part_))
						Traverse (p__anglr_file_part_.m__lexer_part_);
					p__anglr_file_part_.turn_inc ();
					Raise__anglr_file_part__Event (SyntaxTreeCallbackReason.TraversalMidTermCallbackReason, kind, p__anglr_file_part_);
				break;
				case _anglr_file_part_.production_kind.g__anglr_file_part__5:
					// traverse syntax tree node associated with production
					// <anglr file part>: <parser part>

					if (Raise__anglr_file_part__Event (SyntaxTreeCallbackReason.TraversalMidTermCallbackReason, kind, p__anglr_file_part_))
						Traverse (p__anglr_file_part_.m__parser_part_);
					p__anglr_file_part_.turn_inc ();
					Raise__anglr_file_part__Event (SyntaxTreeCallbackReason.TraversalMidTermCallbackReason, kind, p__anglr_file_part_);
				break;
			}
			Raise__anglr_file_part__Event (SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason, kind, p__anglr_file_part_);
			p__anglr_file_part_.unlock();
		}

		//
		// traverse syntax tree node associated with any production of syntax rule <anglr file part>
		//

		public void TraverseCommon (_anglr_file_part_ p__anglr_file_part_)
		{
			_anglr_file_part_.production_kind kind = (_anglr_file_part_.production_kind) p__anglr_file_part_.kind;
			if (Raise_Common_Event (SyntaxTreeCallbackReason.TraversalPrologueCallbackReason, (int) kind, p__anglr_file_part_))
			switch (kind)
			{
				case _anglr_file_part_.production_kind.g__anglr_file_part__1:
					// traverse syntax tree node associated with production
					// <anglr file part>: <general part>

						TraverseCommon (p__anglr_file_part_.m__general_part_);
				break;
				case _anglr_file_part_.production_kind.g__anglr_file_part__2:
					// traverse syntax tree node associated with production
					// <anglr file part>: <declaration part>

						TraverseCommon (p__anglr_file_part_.m__declaration_part_);
				break;
				case _anglr_file_part_.production_kind.g__anglr_file_part__3:
					// traverse syntax tree node associated with production
					// <anglr file part>: <scanner part>

						TraverseCommon (p__anglr_file_part_.m__scanner_part_);
				break;
				case _anglr_file_part_.production_kind.g__anglr_file_part__4:
					// traverse syntax tree node associated with production
					// <anglr file part>: <lexer part>

						TraverseCommon (p__anglr_file_part_.m__lexer_part_);
				break;
				case _anglr_file_part_.production_kind.g__anglr_file_part__5:
					// traverse syntax tree node associated with production
					// <anglr file part>: <parser part>

						TraverseCommon (p__anglr_file_part_.m__parser_part_);
				break;
			}
			Raise_Common_Event (SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason, (int) kind, p__anglr_file_part_);
		}
	};
		#endregion

	public partial class SyntaxTreeBuilder : SyntaxTreeWalker
	{
		#region syntax tree builder methods associated with the syntax rule <anglr file part>

		//
		// create syntax tree node associated with production
		// <anglr file part>: <general part>

		//

		public _anglr_file_part_ _anglr_file_part__1 (_general_part_ p__general_part_)
		{
			_anglr_file_part_ p__anglr_file_part__ref = new _anglr_file_part_(p__general_part_);
			p__general_part_.parent = p__anglr_file_part__ref;
			Raise__anglr_file_part__Event (SyntaxTreeCallbackReason.BuilderCallbackReason, _anglr_file_part_.production_kind.g__anglr_file_part__1, p__anglr_file_part__ref);
			return p__anglr_file_part__ref;
		}

		//
		// create syntax tree node associated with production
		// <anglr file part>: <declaration part>

		//

		public _anglr_file_part_ _anglr_file_part__2 (_declaration_part_ p__declaration_part_)
		{
			_anglr_file_part_ p__anglr_file_part__ref = new _anglr_file_part_(p__declaration_part_);
			p__declaration_part_.parent = p__anglr_file_part__ref;
			Raise__anglr_file_part__Event (SyntaxTreeCallbackReason.BuilderCallbackReason, _anglr_file_part_.production_kind.g__anglr_file_part__2, p__anglr_file_part__ref);
			return p__anglr_file_part__ref;
		}

		//
		// create syntax tree node associated with production
		// <anglr file part>: <scanner part>

		//

		public _anglr_file_part_ _anglr_file_part__3 (_scanner_part_ p__scanner_part_)
		{
			_anglr_file_part_ p__anglr_file_part__ref = new _anglr_file_part_(p__scanner_part_);
			p__scanner_part_.parent = p__anglr_file_part__ref;
			Raise__anglr_file_part__Event (SyntaxTreeCallbackReason.BuilderCallbackReason, _anglr_file_part_.production_kind.g__anglr_file_part__3, p__anglr_file_part__ref);
			return p__anglr_file_part__ref;
		}

		//
		// create syntax tree node associated with production
		// <anglr file part>: <lexer part>

		//

		public _anglr_file_part_ _anglr_file_part__4 (_lexer_part_ p__lexer_part_)
		{
			_anglr_file_part_ p__anglr_file_part__ref = new _anglr_file_part_(p__lexer_part_);
			p__lexer_part_.parent = p__anglr_file_part__ref;
			Raise__anglr_file_part__Event (SyntaxTreeCallbackReason.BuilderCallbackReason, _anglr_file_part_.production_kind.g__anglr_file_part__4, p__anglr_file_part__ref);
			return p__anglr_file_part__ref;
		}

		//
		// create syntax tree node associated with production
		// <anglr file part>: <parser part>

		//

		public _anglr_file_part_ _anglr_file_part__5 (_parser_part_ p__parser_part_)
		{
			_anglr_file_part_ p__anglr_file_part__ref = new _anglr_file_part_(p__parser_part_);
			p__parser_part_.parent = p__anglr_file_part__ref;
			Raise__anglr_file_part__Event (SyntaxTreeCallbackReason.BuilderCallbackReason, _anglr_file_part_.production_kind.g__anglr_file_part__5, p__anglr_file_part__ref);
			return p__anglr_file_part__ref;
		}
		#endregion
	};
}
