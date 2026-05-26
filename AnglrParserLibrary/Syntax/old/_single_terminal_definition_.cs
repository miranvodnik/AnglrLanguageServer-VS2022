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
	// class associated with syntax rule <single terminal definition>
	//

	public class	_single_terminal_definition_ : SyntaxTreeBase
	{
		#region enumerated production(s) of syntax rule <single terminal definition>
		public enum production_kind : ushort	// enumerated production(s) of syntax rule <single terminal definition>
		{
			g__single_terminal_definition__1 = 1,	// '%terminal' <terminal definition>

		};
		#endregion
		#region production markers associated with the syntax rule <single terminal definition>

		// markers associated with production: <single terminal definition> -> '%terminal' <terminal definition>

		public enum production_marker_1 : ushort
		{
			m__terminal_definition_,
			final
		};
		#endregion
		#region Constructors and destructors associated with the syntax rule <single terminal definition>

		// Constructor declaration(s) associated with production(s) of syntax rule <single terminal definition>

		//
		// Constructor associated with the following production(s)
		// <single terminal definition> -> '%terminal' <terminal definition>

		//

		public _single_terminal_definition_ (SyntaxTreeToken p_token, _terminal_definition_ p__terminal_definition_) : base ((uint) ProductionID.__single_terminal_definition__ID, (uint) production_kind.g__single_terminal_definition__1)
		{
			++g_counter;
			_init ();
			children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 2);
			children[0] = m__terminal_ = p_token;
			children[1] = m__terminal_definition_ = p__terminal_definition_;
		}

		// Copy constructor

		public _single_terminal_definition_ (_single_terminal_definition_ p__single_terminal_definition_) : base (p__single_terminal_definition_.id, p__single_terminal_definition_.kind)
		{
			++g_counter;
			_init ();
			switch ((production_kind) this.kind)
			{
			case production_kind.g__single_terminal_definition__1:
				children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 2);
				children[0] = m__terminal_ = p__single_terminal_definition_.m__terminal_;
				children[1] = m__terminal_definition_ = p__single_terminal_definition_.m__terminal_definition_;
				break;
			default:
				string[] args = new string[] { "_single_terminal_definition_" };
				throw new SyntaxTreeError (SyntaxTreeError.InvalidKindError, args);
			}
		}

		// Clone node - wrapper of copy constructor

		public override SyntaxTreeBase Clone () { return (SyntaxTreeBase) new _single_terminal_definition_ (this); }

		// Destructor

		public new void Dispose ()
		{
			base.Dispose ();
			--g_counter;

			m__terminal_?.Dispose ();
			m__terminal_definition_?.Dispose ();

			_init ();
		}
		#endregion
		#region Content changing and inclusion checking methods associated with the syntax rule <single terminal definition>

		// Content changing function(s) associated with production(s) of syntax rule <single terminal definition>

		//
		// Content changing function associated with following production(s)
		// <single terminal definition> -> '%terminal' <terminal definition>

		//

		public void change(SyntaxTreeToken p_token, _terminal_definition_ p__terminal_definition_)
		{
			_init ();
			this.kind = (uint) production_kind.g__single_terminal_definition__1;
			children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 2);
			children [0] = m__terminal_ = p_token;
			children [1] = m__terminal_definition_ = p__terminal_definition_;
			reparent (parent);
		}

		//
		// check if node 'element' is included within syntax tree associated with syntax rule <single terminal definition>
		//

		public new bool checkInclusion (SyntaxTreeBase element)
		{
			return
				(element == this) ||
				(m__terminal_ != null) && m__terminal_.checkInclusion (element) ||
				(m__terminal_definition_ != null) && m__terminal_definition_.checkInclusion (element) ||
				false;
		}
		#endregion
		#region Syntax tree emit methods associated with the syntax rule <single terminal definition>

		//
		// emit production tree node associated with any production of syntax rule <single terminal definition>
		//

		public override string Emit (int depth)
		{
			string s = "";
			if (depth != 0)
			switch ((production_kind) kind)
			{
				case production_kind.g__single_terminal_definition__1:
					// emit syntax tree node associated with production
					// <single terminal definition>: '%terminal' <terminal definition>

					s += m__terminal_.Emit (depth - 1);
					s += " ";
					s += m__terminal_definition_.Emit (depth - 1);
				break;
			}
			return s.Trim ();
		}

		//
		// emit production tree node associated with any production of syntax rule <single terminal definition>
		//

		public override string EmitProductionTree (int depth)
		{
			string s = "_single_terminal_definition_ (";
			if (depth != 0)
				switch ((production_kind) kind)
				{
				case production_kind.g__single_terminal_definition__1:
					// emit syntax tree node associated with production
					// <single terminal definition>: '%terminal' <terminal definition>

					s += "_terminal_";
					s += ' ';
					s += m__terminal_definition_.EmitProductionTree (depth - 1);
				break;
			}
			s += ")";
			return s;
		}
		#endregion
		#region Syntax tree traversal,  reperrent, init and iterator methods associated with the syntax rule <single terminal definition>

		//
		// traverse sub-tree rooted in this node using selected syntax tree walker
		//

		public override void InvokeTraverse (SyntaxTreeWalkerCore syntaxTreeWalker) => ((SyntaxTreeWalker) syntaxTreeWalker).Traverse (this);
		public override void InvokeTraverseCommon (SyntaxTreeWalkerCore syntaxTreeWalker) => ((SyntaxTreeWalker) syntaxTreeWalker).TraverseCommon (this);

		// reparent sub-tree

		public override void reparent (SyntaxTreeBase parent)
		{
			this.parent = parent;

			m__terminal_?.reparent (this);
			m__terminal_definition_?.reparent (this);
		}

		//
		// initialize object associated with syntax rule <single terminal definition>
		//

		public void _init ()
		{
			m__terminal_ = null;
			m__terminal_definition_ = null;
		}
		#endregion
		#region Fields and properties associated with the syntax rule <single terminal definition>

		// counter of all nodes associated with syntax rule <single terminal definition>
		public static int g_counter;

		// objects associated with terminal and non-terminal symbols within production(s) of syntax rule <single terminal definition>
		public SyntaxTreeToken m__terminal_ { get; private set; }
		public _terminal_definition_ m__terminal_definition_ { get; private set; }
		#endregion

	};

	public partial class SyntaxTreeWalker
	{
		#region delegates and events associated with the syntax rule <single terminal definition>

		// delegate function (callback) prototype associated with syntax rule <single terminal definition>
		public delegate bool _single_terminal_definition__Callback (SyntaxTreeCallbackReason reason, _single_terminal_definition_.production_kind kind, _single_terminal_definition_ p__single_terminal_definition_);

		// event associated with syntax rule <single terminal definition>
		public event _single_terminal_definition__Callback _single_terminal_definition__Event;

		// event trigger associated with syntax rule <single terminal definition>
		public bool Raise__single_terminal_definition__Event (SyntaxTreeCallbackReason reason, _single_terminal_definition_.production_kind kind, _single_terminal_definition_ p__single_terminal_definition_)
		{
			bool? status = _single_terminal_definition__Event?.Invoke (reason, kind, p__single_terminal_definition_);
			return (status == null) || status.Value;
		}
		#endregion
		#region syntax tree traversal methods associated with the syntax rule <single terminal definition>
		//
		// traverse syntax tree node associated with any production of syntax rule <single terminal definition>
		//

		public void Traverse (_single_terminal_definition_ p__single_terminal_definition_)
		{
			if (p__single_terminal_definition_.isLocked())
				return;
			p__single_terminal_definition_.dolock();
			_single_terminal_definition_.production_kind kind = (_single_terminal_definition_.production_kind) p__single_terminal_definition_.kind;
			p__single_terminal_definition_.turn_reset ();
			if (Raise__single_terminal_definition__Event (SyntaxTreeCallbackReason.TraversalPrologueCallbackReason, kind, p__single_terminal_definition_))
			switch (kind)
			{
				case _single_terminal_definition_.production_kind.g__single_terminal_definition__1:
					// traverse syntax tree node associated with production
					// <single terminal definition>: '%terminal' <terminal definition>

					if (Raise__single_terminal_definition__Event (SyntaxTreeCallbackReason.TraversalMidTermCallbackReason, kind, p__single_terminal_definition_))
						Traverse (p__single_terminal_definition_.m__terminal_definition_);
					p__single_terminal_definition_.turn_inc ();
					Raise__single_terminal_definition__Event (SyntaxTreeCallbackReason.TraversalMidTermCallbackReason, kind, p__single_terminal_definition_);
				break;
			}
			Raise__single_terminal_definition__Event (SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason, kind, p__single_terminal_definition_);
			p__single_terminal_definition_.unlock();
		}

		//
		// traverse syntax tree node associated with any production of syntax rule <single terminal definition>
		//

		public void TraverseCommon (_single_terminal_definition_ p__single_terminal_definition_)
		{
			_single_terminal_definition_.production_kind kind = (_single_terminal_definition_.production_kind) p__single_terminal_definition_.kind;
			if (Raise_Common_Event (SyntaxTreeCallbackReason.TraversalPrologueCallbackReason, (int) kind, p__single_terminal_definition_))
			switch (kind)
			{
				case _single_terminal_definition_.production_kind.g__single_terminal_definition__1:
					// traverse syntax tree node associated with production
					// <single terminal definition>: '%terminal' <terminal definition>

						TraverseCommon (p__single_terminal_definition_.m__terminal_);
						TraverseCommon (p__single_terminal_definition_.m__terminal_definition_);
				break;
			}
			Raise_Common_Event (SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason, (int) kind, p__single_terminal_definition_);
		}
	};
		#endregion

	public partial class SyntaxTreeBuilder : SyntaxTreeWalker
	{
		#region syntax tree builder methods associated with the syntax rule <single terminal definition>

		//
		// create syntax tree node associated with production
		// <single terminal definition>: '%terminal' <terminal definition>

		//

		public _single_terminal_definition_ _single_terminal_definition__1 (SyntaxTreeToken p_token, _terminal_definition_ p__terminal_definition_)
		{
			_single_terminal_definition_ p__single_terminal_definition__ref = new _single_terminal_definition_(p_token, p__terminal_definition_);
			p_token.parent = p__single_terminal_definition__ref;
			p__terminal_definition_.parent = p__single_terminal_definition__ref;
			Raise__single_terminal_definition__Event (SyntaxTreeCallbackReason.BuilderCallbackReason, _single_terminal_definition_.production_kind.g__single_terminal_definition__1, p__single_terminal_definition__ref);
			return p__single_terminal_definition__ref;
		}
		#endregion
	};
}
