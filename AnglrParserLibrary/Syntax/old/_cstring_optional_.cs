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
	// class associated with syntax rule <cstring optional>
	//

	public class	_cstring_optional_ : SyntaxTreeBase
	{
		#region enumerated production(s) of syntax rule <cstring optional>
		public enum production_kind : ushort	// enumerated production(s) of syntax rule <cstring optional>
		{
			g__cstring_optional__1 = 1,	// %empty

			g__cstring_optional__2 = 2,	// <cstring>

		};
		#endregion
		#region production markers associated with the syntax rule <cstring optional>

		// markers associated with production: <cstring optional> -> %empty

		public enum production_marker_1 : ushort
		{
			final
		};

		// markers associated with production: <cstring optional> -> <cstring>

		public enum production_marker_2 : ushort
		{
			final
		};
		#endregion
		#region Constructors and destructors associated with the syntax rule <cstring optional>

		// Constructor declaration(s) associated with production(s) of syntax rule <cstring optional>

		//
		// Constructor associated with the following production(s)
		// <cstring optional> -> %empty

		//

		public _cstring_optional_ () : base ((uint) ProductionID.__cstring_optional__ID, (uint) production_kind.g__cstring_optional__1)
		{
			++g_counter;
			_init ();
			children = Array.Empty <SyntaxTreeBase> ();
		}

		//
		// Constructor associated with the following production(s)
		// <cstring optional> -> <cstring>

		//

		public _cstring_optional_ (SyntaxTreeToken p_token) : base ((uint) ProductionID.__cstring_optional__ID, (uint) production_kind.g__cstring_optional__2)
		{
			++g_counter;
			_init ();
			children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 1);
			children[0] = m__cstring_ = p_token;
		}

		// Copy constructor

		public _cstring_optional_ (_cstring_optional_ p__cstring_optional_) : base (p__cstring_optional_.id, p__cstring_optional_.kind)
		{
			++g_counter;
			_init ();
			switch ((production_kind) this.kind)
			{
			case production_kind.g__cstring_optional__1:
				children = Array.Empty <SyntaxTreeBase> ();
				break;
			case production_kind.g__cstring_optional__2:
				children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 1);
				children[0] = m__cstring_ = p__cstring_optional_.m__cstring_;
				break;
			default:
				string[] args = new string[] { "_cstring_optional_" };
				throw new SyntaxTreeError (SyntaxTreeError.InvalidKindError, args);
			}
		}

		// Clone node - wrapper of copy constructor

		public override SyntaxTreeBase Clone () { return (SyntaxTreeBase) new _cstring_optional_ (this); }

		// Destructor

		public new void Dispose ()
		{
			base.Dispose ();
			--g_counter;

			m__cstring_?.Dispose ();

			_init ();
		}
		#endregion
		#region Content changing and inclusion checking methods associated with the syntax rule <cstring optional>

		// Content changing function(s) associated with production(s) of syntax rule <cstring optional>

		//
		// Content changing function associated with following production(s)
		// <cstring optional> -> %empty

		//

		public void change()
		{
			_init ();
			this.kind = (uint) production_kind.g__cstring_optional__1;
			children = Array.Empty <SyntaxTreeBase> ();
			reparent (parent);
		}

		//
		// Content changing function associated with following production(s)
		// <cstring optional> -> <cstring>

		//

		public void change(SyntaxTreeToken p_token)
		{
			_init ();
			this.kind = (uint) production_kind.g__cstring_optional__2;
			children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 1);
			children [0] = m__cstring_ = p_token;
			reparent (parent);
		}

		//
		// check if node 'element' is included within syntax tree associated with syntax rule <cstring optional>
		//

		public new bool checkInclusion (SyntaxTreeBase element)
		{
			return
				(element == this) ||
				(m__cstring_ != null) && m__cstring_.checkInclusion (element) ||
				false;
		}
		#endregion
		#region Syntax tree emit methods associated with the syntax rule <cstring optional>

		//
		// emit production tree node associated with any production of syntax rule <cstring optional>
		//

		public override string Emit (int depth)
		{
			string s = "";
			if (depth != 0)
			switch ((production_kind) kind)
			{
				case production_kind.g__cstring_optional__1:
					// emit syntax tree node associated with production
					// <cstring optional>: %empty

				break;
				case production_kind.g__cstring_optional__2:
					// emit syntax tree node associated with production
					// <cstring optional>: <cstring>

					s += m__cstring_.Emit (depth - 1);
				break;
			}
			return s.Trim ();
		}

		//
		// emit production tree node associated with any production of syntax rule <cstring optional>
		//

		public override string EmitProductionTree (int depth)
		{
			string s = "_cstring_optional_ (";
			if (depth != 0)
				switch ((production_kind) kind)
				{
				case production_kind.g__cstring_optional__1:
					// emit syntax tree node associated with production
					// <cstring optional>: %empty

				break;
				case production_kind.g__cstring_optional__2:
					// emit syntax tree node associated with production
					// <cstring optional>: <cstring>

					s += "_cstring_";
				break;
			}
			s += ")";
			return s;
		}
		#endregion
		#region Syntax tree traversal,  reperrent, init and iterator methods associated with the syntax rule <cstring optional>

		//
		// traverse sub-tree rooted in this node using selected syntax tree walker
		//

		public override void InvokeTraverse (SyntaxTreeWalkerCore syntaxTreeWalker) => ((SyntaxTreeWalker) syntaxTreeWalker).Traverse (this);
		public override void InvokeTraverseCommon (SyntaxTreeWalkerCore syntaxTreeWalker) => ((SyntaxTreeWalker) syntaxTreeWalker).TraverseCommon (this);

		// reparent sub-tree

		public override void reparent (SyntaxTreeBase parent)
		{
			this.parent = parent;

			m__cstring_?.reparent (this);
		}

		//
		// initialize object associated with syntax rule <cstring optional>
		//

		public void _init ()
		{
			m__cstring_ = null;
		}
		#endregion
		#region Fields and properties associated with the syntax rule <cstring optional>

		// counter of all nodes associated with syntax rule <cstring optional>
		public static int g_counter;

		// objects associated with terminal and non-terminal symbols within production(s) of syntax rule <cstring optional>
		public SyntaxTreeToken m__cstring_ { get; private set; }
		#endregion

	};

	public partial class SyntaxTreeWalker
	{
		#region delegates and events associated with the syntax rule <cstring optional>

		// delegate function (callback) prototype associated with syntax rule <cstring optional>
		public delegate bool _cstring_optional__Callback (SyntaxTreeCallbackReason reason, _cstring_optional_.production_kind kind, _cstring_optional_ p__cstring_optional_);

		// event associated with syntax rule <cstring optional>
		public event _cstring_optional__Callback _cstring_optional__Event;

		// event trigger associated with syntax rule <cstring optional>
		public bool Raise__cstring_optional__Event (SyntaxTreeCallbackReason reason, _cstring_optional_.production_kind kind, _cstring_optional_ p__cstring_optional_)
		{
			bool? status = _cstring_optional__Event?.Invoke (reason, kind, p__cstring_optional_);
			return (status == null) || status.Value;
		}
		#endregion
		#region syntax tree traversal methods associated with the syntax rule <cstring optional>
		//
		// traverse syntax tree node associated with any production of syntax rule <cstring optional>
		//

		public void Traverse (_cstring_optional_ p__cstring_optional_)
		{
			if (p__cstring_optional_.isLocked())
				return;
			p__cstring_optional_.dolock();
			_cstring_optional_.production_kind kind = (_cstring_optional_.production_kind) p__cstring_optional_.kind;
			p__cstring_optional_.turn_reset ();
			if (Raise__cstring_optional__Event (SyntaxTreeCallbackReason.TraversalPrologueCallbackReason, kind, p__cstring_optional_))
			switch (kind)
			{
				case _cstring_optional_.production_kind.g__cstring_optional__1:
					// traverse syntax tree node associated with production
					// <cstring optional>: %empty

				break;
				case _cstring_optional_.production_kind.g__cstring_optional__2:
					// traverse syntax tree node associated with production
					// <cstring optional>: <cstring>

				break;
			}
			Raise__cstring_optional__Event (SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason, kind, p__cstring_optional_);
			p__cstring_optional_.unlock();
		}

		//
		// traverse syntax tree node associated with any production of syntax rule <cstring optional>
		//

		public void TraverseCommon (_cstring_optional_ p__cstring_optional_)
		{
			_cstring_optional_.production_kind kind = (_cstring_optional_.production_kind) p__cstring_optional_.kind;
			if (Raise_Common_Event (SyntaxTreeCallbackReason.TraversalPrologueCallbackReason, (int) kind, p__cstring_optional_))
			switch (kind)
			{
				case _cstring_optional_.production_kind.g__cstring_optional__1:
					// traverse syntax tree node associated with production
					// <cstring optional>: %empty

				break;
				case _cstring_optional_.production_kind.g__cstring_optional__2:
					// traverse syntax tree node associated with production
					// <cstring optional>: <cstring>

						TraverseCommon (p__cstring_optional_.m__cstring_);
				break;
			}
			Raise_Common_Event (SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason, (int) kind, p__cstring_optional_);
		}
	};
		#endregion

	public partial class SyntaxTreeBuilder : SyntaxTreeWalker
	{
		#region syntax tree builder methods associated with the syntax rule <cstring optional>

		//
		// create syntax tree node associated with production
		// <cstring optional>: %empty

		//

		public _cstring_optional_ _cstring_optional__1 ()
		{
			_cstring_optional_ p__cstring_optional__ref = new _cstring_optional_();
			Raise__cstring_optional__Event (SyntaxTreeCallbackReason.BuilderCallbackReason, _cstring_optional_.production_kind.g__cstring_optional__1, p__cstring_optional__ref);
			return p__cstring_optional__ref;
		}

		//
		// create syntax tree node associated with production
		// <cstring optional>: <cstring>

		//

		public _cstring_optional_ _cstring_optional__2 (SyntaxTreeToken p_token)
		{
			_cstring_optional_ p__cstring_optional__ref = new _cstring_optional_(p_token);
			p_token.parent = p__cstring_optional__ref;
			Raise__cstring_optional__Event (SyntaxTreeCallbackReason.BuilderCallbackReason, _cstring_optional_.production_kind.g__cstring_optional__2, p__cstring_optional__ref);
			return p__cstring_optional__ref;
		}
		#endregion
	};
}
