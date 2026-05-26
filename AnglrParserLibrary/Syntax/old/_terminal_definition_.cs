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
	// class associated with syntax rule <terminal definition>
	//

	public class	_terminal_definition_ : SyntaxTreeBase
	{
		#region enumerated production(s) of syntax rule <terminal definition>
		public enum production_kind : ushort	// enumerated production(s) of syntax rule <terminal definition>
		{
			g__terminal_definition__1 = 1,	// <identifier> <cstring optional>

		};
		#endregion
		#region production markers associated with the syntax rule <terminal definition>

		// markers associated with production: <terminal definition> -> <identifier> <cstring optional>

		public enum production_marker_1 : ushort
		{
			m__cstring_optional_,
			final
		};
		#endregion
		#region Constructors and destructors associated with the syntax rule <terminal definition>

		// Constructor declaration(s) associated with production(s) of syntax rule <terminal definition>

		//
		// Constructor associated with the following production(s)
		// <terminal definition> -> <identifier> <cstring optional>

		//

		public _terminal_definition_ (SyntaxTreeToken p_token, _cstring_optional_ p__cstring_optional_) : base ((uint) ProductionID.__terminal_definition__ID, (uint) production_kind.g__terminal_definition__1)
		{
			++g_counter;
			_init ();
			children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 2);
			children[0] = m__identifier_ = p_token;
			children[1] = m__cstring_optional_ = p__cstring_optional_;
		}

		// Copy constructor

		public _terminal_definition_ (_terminal_definition_ p__terminal_definition_) : base (p__terminal_definition_.id, p__terminal_definition_.kind)
		{
			++g_counter;
			_init ();
			switch ((production_kind) this.kind)
			{
			case production_kind.g__terminal_definition__1:
				children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 2);
				children[0] = m__identifier_ = p__terminal_definition_.m__identifier_;
				children[1] = m__cstring_optional_ = p__terminal_definition_.m__cstring_optional_;
				break;
			default:
				string[] args = new string[] { "_terminal_definition_" };
				throw new SyntaxTreeError (SyntaxTreeError.InvalidKindError, args);
			}
		}

		// Clone node - wrapper of copy constructor

		public override SyntaxTreeBase Clone () { return (SyntaxTreeBase) new _terminal_definition_ (this); }

		// Destructor

		public new void Dispose ()
		{
			base.Dispose ();
			--g_counter;

			m__identifier_?.Dispose ();
			m__cstring_optional_?.Dispose ();

			_init ();
		}
		#endregion
		#region Content changing and inclusion checking methods associated with the syntax rule <terminal definition>

		// Content changing function(s) associated with production(s) of syntax rule <terminal definition>

		//
		// Content changing function associated with following production(s)
		// <terminal definition> -> <identifier> <cstring optional>

		//

		public void change(SyntaxTreeToken p_token, _cstring_optional_ p__cstring_optional_)
		{
			_init ();
			this.kind = (uint) production_kind.g__terminal_definition__1;
			children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 2);
			children [0] = m__identifier_ = p_token;
			children [1] = m__cstring_optional_ = p__cstring_optional_;
			reparent (parent);
		}

		//
		// check if node 'element' is included within syntax tree associated with syntax rule <terminal definition>
		//

		public new bool checkInclusion (SyntaxTreeBase element)
		{
			return
				(element == this) ||
				(m__identifier_ != null) && m__identifier_.checkInclusion (element) ||
				(m__cstring_optional_ != null) && m__cstring_optional_.checkInclusion (element) ||
				false;
		}
		#endregion
		#region Syntax tree emit methods associated with the syntax rule <terminal definition>

		//
		// emit production tree node associated with any production of syntax rule <terminal definition>
		//

		public override string Emit (int depth)
		{
			string s = "";
			if (depth != 0)
			switch ((production_kind) kind)
			{
				case production_kind.g__terminal_definition__1:
					// emit syntax tree node associated with production
					// <terminal definition>: <identifier> <cstring optional>

					s += m__identifier_.Emit (depth - 1);
					s += " ";
					s += m__cstring_optional_.Emit (depth - 1);
				break;
			}
			return s.Trim ();
		}

		//
		// emit production tree node associated with any production of syntax rule <terminal definition>
		//

		public override string EmitProductionTree (int depth)
		{
			string s = "_terminal_definition_ (";
			if (depth != 0)
				switch ((production_kind) kind)
				{
				case production_kind.g__terminal_definition__1:
					// emit syntax tree node associated with production
					// <terminal definition>: <identifier> <cstring optional>

					s += "_identifier_";
					s += ' ';
					s += m__cstring_optional_.EmitProductionTree (depth - 1);
				break;
			}
			s += ")";
			return s;
		}
		#endregion
		#region Syntax tree traversal,  reperrent, init and iterator methods associated with the syntax rule <terminal definition>

		//
		// traverse sub-tree rooted in this node using selected syntax tree walker
		//

		public override void InvokeTraverse (SyntaxTreeWalkerCore syntaxTreeWalker) => ((SyntaxTreeWalker) syntaxTreeWalker).Traverse (this);
		public override void InvokeTraverseCommon (SyntaxTreeWalkerCore syntaxTreeWalker) => ((SyntaxTreeWalker) syntaxTreeWalker).TraverseCommon (this);

		// reparent sub-tree

		public override void reparent (SyntaxTreeBase parent)
		{
			this.parent = parent;

			m__identifier_?.reparent (this);
			m__cstring_optional_?.reparent (this);
		}

		//
		// initialize object associated with syntax rule <terminal definition>
		//

		public void _init ()
		{
			m__identifier_ = null;
			m__cstring_optional_ = null;
		}
		#endregion
		#region Fields and properties associated with the syntax rule <terminal definition>

		// counter of all nodes associated with syntax rule <terminal definition>
		public static int g_counter;

		// objects associated with terminal and non-terminal symbols within production(s) of syntax rule <terminal definition>
		public SyntaxTreeToken m__identifier_ { get; private set; }
		public _cstring_optional_ m__cstring_optional_ { get; private set; }
		#endregion

	};

	public partial class SyntaxTreeWalker
	{
		#region delegates and events associated with the syntax rule <terminal definition>

		// delegate function (callback) prototype associated with syntax rule <terminal definition>
		public delegate bool _terminal_definition__Callback (SyntaxTreeCallbackReason reason, _terminal_definition_.production_kind kind, _terminal_definition_ p__terminal_definition_);

		// event associated with syntax rule <terminal definition>
		public event _terminal_definition__Callback _terminal_definition__Event;

		// event trigger associated with syntax rule <terminal definition>
		public bool Raise__terminal_definition__Event (SyntaxTreeCallbackReason reason, _terminal_definition_.production_kind kind, _terminal_definition_ p__terminal_definition_)
		{
			bool? status = _terminal_definition__Event?.Invoke (reason, kind, p__terminal_definition_);
			return (status == null) || status.Value;
		}
		#endregion
		#region syntax tree traversal methods associated with the syntax rule <terminal definition>
		//
		// traverse syntax tree node associated with any production of syntax rule <terminal definition>
		//

		public void Traverse (_terminal_definition_ p__terminal_definition_)
		{
			if (p__terminal_definition_.isLocked())
				return;
			p__terminal_definition_.dolock();
			_terminal_definition_.production_kind kind = (_terminal_definition_.production_kind) p__terminal_definition_.kind;
			p__terminal_definition_.turn_reset ();
			if (Raise__terminal_definition__Event (SyntaxTreeCallbackReason.TraversalPrologueCallbackReason, kind, p__terminal_definition_))
			switch (kind)
			{
				case _terminal_definition_.production_kind.g__terminal_definition__1:
					// traverse syntax tree node associated with production
					// <terminal definition>: <identifier> <cstring optional>

					if (Raise__terminal_definition__Event (SyntaxTreeCallbackReason.TraversalMidTermCallbackReason, kind, p__terminal_definition_))
						Traverse (p__terminal_definition_.m__cstring_optional_);
					p__terminal_definition_.turn_inc ();
					Raise__terminal_definition__Event (SyntaxTreeCallbackReason.TraversalMidTermCallbackReason, kind, p__terminal_definition_);
				break;
			}
			Raise__terminal_definition__Event (SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason, kind, p__terminal_definition_);
			p__terminal_definition_.unlock();
		}

		//
		// traverse syntax tree node associated with any production of syntax rule <terminal definition>
		//

		public void TraverseCommon (_terminal_definition_ p__terminal_definition_)
		{
			_terminal_definition_.production_kind kind = (_terminal_definition_.production_kind) p__terminal_definition_.kind;
			if (Raise_Common_Event (SyntaxTreeCallbackReason.TraversalPrologueCallbackReason, (int) kind, p__terminal_definition_))
			switch (kind)
			{
				case _terminal_definition_.production_kind.g__terminal_definition__1:
					// traverse syntax tree node associated with production
					// <terminal definition>: <identifier> <cstring optional>

						TraverseCommon (p__terminal_definition_.m__identifier_);
						TraverseCommon (p__terminal_definition_.m__cstring_optional_);
				break;
			}
			Raise_Common_Event (SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason, (int) kind, p__terminal_definition_);
		}
	};
		#endregion

	public partial class SyntaxTreeBuilder : SyntaxTreeWalker
	{
		#region syntax tree builder methods associated with the syntax rule <terminal definition>

		//
		// create syntax tree node associated with production
		// <terminal definition>: <identifier> <cstring optional>

		//

		public _terminal_definition_ _terminal_definition__1 (SyntaxTreeToken p_token, _cstring_optional_ p__cstring_optional_)
		{
			_terminal_definition_ p__terminal_definition__ref = new _terminal_definition_(p_token, p__cstring_optional_);
			p_token.parent = p__terminal_definition__ref;
			p__cstring_optional_.parent = p__terminal_definition__ref;
			Raise__terminal_definition__Event (SyntaxTreeCallbackReason.BuilderCallbackReason, _terminal_definition_.production_kind.g__terminal_definition__1, p__terminal_definition__ref);
			return p__terminal_definition__ref;
		}
		#endregion
	};
}
