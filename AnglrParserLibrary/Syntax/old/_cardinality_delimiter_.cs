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
	// class associated with syntax rule <cardinality delimiter>
	//

	public class	_cardinality_delimiter_ : SyntaxTreeBase
	{
		#region enumerated production(s) of syntax rule <cardinality delimiter>
		public enum production_kind : ushort	// enumerated production(s) of syntax rule <cardinality delimiter>
		{
			g__cardinality_delimiter__1 = 1,	// <cardinality> <delimiter optional>

		};
		#endregion
		#region production markers associated with the syntax rule <cardinality delimiter>

		// markers associated with production: <cardinality delimiter> -> <cardinality> <delimiter optional>

		public enum production_marker_1 : ushort
		{
			m__cardinality_,
			m__delimiter_optional_,
			final
		};
		#endregion
		#region Constructors and destructors associated with the syntax rule <cardinality delimiter>

		// Constructor declaration(s) associated with production(s) of syntax rule <cardinality delimiter>

		//
		// Constructor associated with the following production(s)
		// <cardinality delimiter> -> <cardinality> <delimiter optional>

		//

		public _cardinality_delimiter_ (_cardinality_ p__cardinality_, _delimiter_optional_ p__delimiter_optional_) : base ((uint) ProductionID.__cardinality_delimiter__ID, (uint) production_kind.g__cardinality_delimiter__1)
		{
			++g_counter;
			_init ();
			children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 2);
			children[0] = m__cardinality_ = p__cardinality_;
			children[1] = m__delimiter_optional_ = p__delimiter_optional_;
		}

		// Copy constructor

		public _cardinality_delimiter_ (_cardinality_delimiter_ p__cardinality_delimiter_) : base (p__cardinality_delimiter_.id, p__cardinality_delimiter_.kind)
		{
			++g_counter;
			_init ();
			switch ((production_kind) this.kind)
			{
			case production_kind.g__cardinality_delimiter__1:
				children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 2);
				children[0] = m__cardinality_ = p__cardinality_delimiter_.m__cardinality_;
				children[1] = m__delimiter_optional_ = p__cardinality_delimiter_.m__delimiter_optional_;
				break;
			default:
				string[] args = new string[] { "_cardinality_delimiter_" };
				throw new SyntaxTreeError (SyntaxTreeError.InvalidKindError, args);
			}
		}

		// Clone node - wrapper of copy constructor

		public override SyntaxTreeBase Clone () { return (SyntaxTreeBase) new _cardinality_delimiter_ (this); }

		// Destructor

		public new void Dispose ()
		{
			base.Dispose ();
			--g_counter;

			m__cardinality_?.Dispose ();
			m__delimiter_optional_?.Dispose ();

			_init ();
		}
		#endregion
		#region Content changing and inclusion checking methods associated with the syntax rule <cardinality delimiter>

		// Content changing function(s) associated with production(s) of syntax rule <cardinality delimiter>

		//
		// Content changing function associated with following production(s)
		// <cardinality delimiter> -> <cardinality> <delimiter optional>

		//

		public void change(_cardinality_ p__cardinality_, _delimiter_optional_ p__delimiter_optional_)
		{
			_init ();
			this.kind = (uint) production_kind.g__cardinality_delimiter__1;
			children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 2);
			children [0] = m__cardinality_ = p__cardinality_;
			children [1] = m__delimiter_optional_ = p__delimiter_optional_;
			reparent (parent);
		}

		//
		// check if node 'element' is included within syntax tree associated with syntax rule <cardinality delimiter>
		//

		public new bool checkInclusion (SyntaxTreeBase element)
		{
			return
				(element == this) ||
				(m__cardinality_ != null) && m__cardinality_.checkInclusion (element) ||
				(m__delimiter_optional_ != null) && m__delimiter_optional_.checkInclusion (element) ||
				false;
		}
		#endregion
		#region Syntax tree emit methods associated with the syntax rule <cardinality delimiter>

		//
		// emit production tree node associated with any production of syntax rule <cardinality delimiter>
		//

		public override string Emit (int depth)
		{
			string s = "";
			if (depth != 0)
			switch ((production_kind) kind)
			{
				case production_kind.g__cardinality_delimiter__1:
					// emit syntax tree node associated with production
					// <cardinality delimiter>: <cardinality> <delimiter optional>

					s += m__cardinality_.Emit (depth - 1);
					s += " ";
					s += m__delimiter_optional_.Emit (depth - 1);
				break;
			}
			return s.Trim ();
		}

		//
		// emit production tree node associated with any production of syntax rule <cardinality delimiter>
		//

		public override string EmitProductionTree (int depth)
		{
			string s = "_cardinality_delimiter_ (";
			if (depth != 0)
				switch ((production_kind) kind)
				{
				case production_kind.g__cardinality_delimiter__1:
					// emit syntax tree node associated with production
					// <cardinality delimiter>: <cardinality> <delimiter optional>

					s += m__cardinality_.EmitProductionTree (depth - 1);
					s += ' ';
					s += m__delimiter_optional_.EmitProductionTree (depth - 1);
				break;
			}
			s += ")";
			return s;
		}
		#endregion
		#region Syntax tree traversal,  reperrent, init and iterator methods associated with the syntax rule <cardinality delimiter>

		//
		// traverse sub-tree rooted in this node using selected syntax tree walker
		//

		public override void InvokeTraverse (SyntaxTreeWalkerCore syntaxTreeWalker) => ((SyntaxTreeWalker) syntaxTreeWalker).Traverse (this);
		public override void InvokeTraverseCommon (SyntaxTreeWalkerCore syntaxTreeWalker) => ((SyntaxTreeWalker) syntaxTreeWalker).TraverseCommon (this);

		// reparent sub-tree

		public override void reparent (SyntaxTreeBase parent)
		{
			this.parent = parent;

			m__cardinality_?.reparent (this);
			m__delimiter_optional_?.reparent (this);
		}

		//
		// initialize object associated with syntax rule <cardinality delimiter>
		//

		public void _init ()
		{
			m__cardinality_ = null;
			m__delimiter_optional_ = null;
		}
		#endregion
		#region Fields and properties associated with the syntax rule <cardinality delimiter>

		// counter of all nodes associated with syntax rule <cardinality delimiter>
		public static int g_counter;

		// objects associated with terminal and non-terminal symbols within production(s) of syntax rule <cardinality delimiter>
		public _cardinality_ m__cardinality_ { get; private set; }
		public _delimiter_optional_ m__delimiter_optional_ { get; private set; }
		#endregion

	};

	public partial class SyntaxTreeWalker
	{
		#region delegates and events associated with the syntax rule <cardinality delimiter>

		// delegate function (callback) prototype associated with syntax rule <cardinality delimiter>
		public delegate bool _cardinality_delimiter__Callback (SyntaxTreeCallbackReason reason, _cardinality_delimiter_.production_kind kind, _cardinality_delimiter_ p__cardinality_delimiter_);

		// event associated with syntax rule <cardinality delimiter>
		public event _cardinality_delimiter__Callback _cardinality_delimiter__Event;

		// event trigger associated with syntax rule <cardinality delimiter>
		public bool Raise__cardinality_delimiter__Event (SyntaxTreeCallbackReason reason, _cardinality_delimiter_.production_kind kind, _cardinality_delimiter_ p__cardinality_delimiter_)
		{
			bool? status = _cardinality_delimiter__Event?.Invoke (reason, kind, p__cardinality_delimiter_);
			return (status == null) || status.Value;
		}
		#endregion
		#region syntax tree traversal methods associated with the syntax rule <cardinality delimiter>
		//
		// traverse syntax tree node associated with any production of syntax rule <cardinality delimiter>
		//

		public void Traverse (_cardinality_delimiter_ p__cardinality_delimiter_)
		{
			if (p__cardinality_delimiter_.isLocked())
				return;
			p__cardinality_delimiter_.dolock();
			_cardinality_delimiter_.production_kind kind = (_cardinality_delimiter_.production_kind) p__cardinality_delimiter_.kind;
			p__cardinality_delimiter_.turn_reset ();
			if (Raise__cardinality_delimiter__Event (SyntaxTreeCallbackReason.TraversalPrologueCallbackReason, kind, p__cardinality_delimiter_))
			switch (kind)
			{
				case _cardinality_delimiter_.production_kind.g__cardinality_delimiter__1:
					// traverse syntax tree node associated with production
					// <cardinality delimiter>: <cardinality> <delimiter optional>

					if (Raise__cardinality_delimiter__Event (SyntaxTreeCallbackReason.TraversalMidTermCallbackReason, kind, p__cardinality_delimiter_))
						Traverse (p__cardinality_delimiter_.m__cardinality_);
					p__cardinality_delimiter_.turn_inc ();
					if (Raise__cardinality_delimiter__Event (SyntaxTreeCallbackReason.TraversalMidTermCallbackReason, kind, p__cardinality_delimiter_))
						Traverse (p__cardinality_delimiter_.m__delimiter_optional_);
					p__cardinality_delimiter_.turn_inc ();
					Raise__cardinality_delimiter__Event (SyntaxTreeCallbackReason.TraversalMidTermCallbackReason, kind, p__cardinality_delimiter_);
				break;
			}
			Raise__cardinality_delimiter__Event (SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason, kind, p__cardinality_delimiter_);
			p__cardinality_delimiter_.unlock();
		}

		//
		// traverse syntax tree node associated with any production of syntax rule <cardinality delimiter>
		//

		public void TraverseCommon (_cardinality_delimiter_ p__cardinality_delimiter_)
		{
			_cardinality_delimiter_.production_kind kind = (_cardinality_delimiter_.production_kind) p__cardinality_delimiter_.kind;
			if (Raise_Common_Event (SyntaxTreeCallbackReason.TraversalPrologueCallbackReason, (int) kind, p__cardinality_delimiter_))
			switch (kind)
			{
				case _cardinality_delimiter_.production_kind.g__cardinality_delimiter__1:
					// traverse syntax tree node associated with production
					// <cardinality delimiter>: <cardinality> <delimiter optional>

						TraverseCommon (p__cardinality_delimiter_.m__cardinality_);
						TraverseCommon (p__cardinality_delimiter_.m__delimiter_optional_);
				break;
			}
			Raise_Common_Event (SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason, (int) kind, p__cardinality_delimiter_);
		}
	};
		#endregion

	public partial class SyntaxTreeBuilder : SyntaxTreeWalker
	{
		#region syntax tree builder methods associated with the syntax rule <cardinality delimiter>

		//
		// create syntax tree node associated with production
		// <cardinality delimiter>: <cardinality> <delimiter optional>

		//

		public _cardinality_delimiter_ _cardinality_delimiter__1 (_cardinality_ p__cardinality_, _delimiter_optional_ p__delimiter_optional_)
		{
			_cardinality_delimiter_ p__cardinality_delimiter__ref = new _cardinality_delimiter_(p__cardinality_, p__delimiter_optional_);
			p__cardinality_.parent = p__cardinality_delimiter__ref;
			p__delimiter_optional_.parent = p__cardinality_delimiter__ref;
			Raise__cardinality_delimiter__Event (SyntaxTreeCallbackReason.BuilderCallbackReason, _cardinality_delimiter_.production_kind.g__cardinality_delimiter__1, p__cardinality_delimiter__ref);
			return p__cardinality_delimiter__ref;
		}
		#endregion
	};
}
