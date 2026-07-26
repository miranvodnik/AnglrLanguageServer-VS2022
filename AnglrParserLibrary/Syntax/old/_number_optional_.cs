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
	// class associated with syntax rule <number optional>
	//

	public class	_number_optional_ : SyntaxTreeBase
	{
		#region enumerated production(s) of syntax rule <number optional>
		public enum production_kind : ushort	// enumerated production(s) of syntax rule <number optional>
		{
			g__number_optional__1 = 1,	// %empty

			g__number_optional__2 = 2,	// <number>

		};
		#endregion
		#region production markers associated with the syntax rule <number optional>

		// markers associated with production: <number optional> -> %empty

		public enum production_marker_1 : ushort
		{
			final
		};

		// markers associated with production: <number optional> -> <number>

		public enum production_marker_2 : ushort
		{
			final
		};
		#endregion
		#region Constructors and destructors associated with the syntax rule <number optional>

		// Constructor declaration(s) associated with production(s) of syntax rule <number optional>

		//
		// Constructor associated with the following production(s)
		// <number optional> -> %empty

		//

		public _number_optional_ () : base ((uint) ProductionID.__number_optional__ID, (uint) production_kind.g__number_optional__1)
		{
			++g_counter;
			_init ();
			children = Array.Empty <SyntaxTreeBase> ();
		}

		//
		// Constructor associated with the following production(s)
		// <number optional> -> <number>

		//

		public _number_optional_ (SyntaxTreeToken p_token) : base ((uint) ProductionID.__number_optional__ID, (uint) production_kind.g__number_optional__2)
		{
			++g_counter;
			_init ();
			children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 1);
			children[0] = m__number_ = p_token;
		}

		// Copy constructor

		public _number_optional_ (_number_optional_ p__number_optional_) : base (p__number_optional_.id, p__number_optional_.kind)
		{
			++g_counter;
			_init ();
			switch ((production_kind) p__number_optional_.kind)
			{
				case production_kind.g__number_optional__1:
					children = Array.Empty <SyntaxTreeBase> ();
					break;
				case production_kind.g__number_optional__2:
					children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 1);
					if ((children [0] = m__number_ = (p__number_optional_.m__number_ != null) ? new SyntaxTreeToken (p__number_optional_.m__number_) : null) != null) m__number_.parent = this;
					break;
				default:
				{
					string[] args = new string[] { "_number_optional_" };
					throw new SyntaxTreeError (SyntaxTreeError.InvalidKindError, args);
				}
			}
		}

		// Clone node - wrapper of copy constructor

		public override SyntaxTreeBase Clone () { return (SyntaxTreeBase) new _number_optional_ (this); }

		// Destructor

		public new void Dispose ()
		{
			base.Dispose ();
			--g_counter;

			m__number_?.Dispose ();

			_init ();
		}
		#endregion
		#region Content changing and inclusion checking methods associated with the syntax rule <number optional>

		// Content changing function(s) associated with production(s) of syntax rule <number optional>

		//
		// Content changing function associated with following production(s)
		// <number optional> -> %empty

		//

		public void change()
		{
			_init ();
			this.kind = (uint) production_kind.g__number_optional__1;
			children = Array.Empty <SyntaxTreeBase> ();
			reparent (parent);
		}

		//
		// Content changing function associated with following production(s)
		// <number optional> -> <number>

		//

		public void change(SyntaxTreeToken p_token)
		{
			_init ();
			this.kind = (uint) production_kind.g__number_optional__2;
			children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 1);
			children [0] = m__number_ = p_token;
			reparent (parent);
		}

		//
		// check if node 'element' is included within syntax tree associated with syntax rule <number optional>
		//

		public new bool checkInclusion (SyntaxTreeBase element)
		{
			return
				(element == this) ||
				(m__number_ != null) && m__number_.checkInclusion (element) ||
				false;
		}
		#endregion
		#region Syntax tree emit methods associated with the syntax rule <number optional>

		//
		// emit production tree node associated with any production of syntax rule <number optional>
		//

		public override string Emit (int depth)
		{
			string s = "";
			if (depth != 0)
			switch ((production_kind) kind)
			{
				case production_kind.g__number_optional__1:
					// emit syntax tree node associated with production
					// <number optional>: %empty

				break;
				case production_kind.g__number_optional__2:
					// emit syntax tree node associated with production
					// <number optional>: <number>

					s += m__number_.Emit (depth - 1);
				break;
			}
			return s.Trim ();
		}

		//
		// emit production tree node associated with any production of syntax rule <number optional>
		//

		public override string EmitProductionTree (int depth)
		{
			string s = "_number_optional_ (";
			if (depth != 0)
				switch ((production_kind) kind)
				{
				case production_kind.g__number_optional__1:
					// emit syntax tree node associated with production
					// <number optional>: %empty

				break;
				case production_kind.g__number_optional__2:
					// emit syntax tree node associated with production
					// <number optional>: <number>

					s += "_number_";
				break;
			}
			s += ")";
			return s;
		}
		#endregion
		#region Syntax tree traversal,  reperrent, init and iterator methods associated with the syntax rule <number optional>

		//
		// traverse sub-tree rooted in this node using selected syntax tree walker
		//

		public override void InvokeTraverse (SyntaxTreeWalkerCore syntaxTreeWalker) => ((SyntaxTreeWalker) syntaxTreeWalker).Traverse (this);
		public override void InvokeTraverseCommon (SyntaxTreeWalkerCore syntaxTreeWalker) => ((SyntaxTreeWalker) syntaxTreeWalker).TraverseCommon (this);

		// reparent sub-tree

		public override void reparent (SyntaxTreeBase parent)
		{
			this.parent = parent;

			m__number_?.reparent (this);
		}

		//
		// initialize object associated with syntax rule <number optional>
		//

		public void _init ()
		{
			m__number_ = null;
		}
		#endregion
		#region Fields and properties associated with the syntax rule <number optional>

		// counter of all nodes associated with syntax rule <number optional>
		public static int g_counter;

		// objects associated with terminal and non-terminal symbols within production(s) of syntax rule <number optional>
		public SyntaxTreeToken m__number_ { get; private set; }
		#endregion

	};

	public partial class SyntaxTreeWalker
	{
		#region delegates and events associated with the syntax rule <number optional>

		// delegate function (callback) prototype associated with syntax rule <number optional>
		public delegate bool _number_optional__Callback (SyntaxTreeCallbackReason reason, _number_optional_.production_kind kind, _number_optional_ p__number_optional_);

		// event associated with syntax rule <number optional>
		public event _number_optional__Callback _number_optional__Event;

		// event trigger associated with syntax rule <number optional>
		public bool Raise__number_optional__Event (SyntaxTreeCallbackReason reason, _number_optional_.production_kind kind, _number_optional_ p__number_optional_)
		{
			bool? status = _number_optional__Event?.Invoke (reason, kind, p__number_optional_);
			return (status == null) || status.Value;
		}
		#endregion
		#region syntax tree traversal methods associated with the syntax rule <number optional>
		//
		// traverse syntax tree node associated with any production of syntax rule <number optional>
		//

		public void Traverse (_number_optional_ p__number_optional_)
		{
			if (p__number_optional_.isLocked())
				return;
			p__number_optional_.dolock();
			_number_optional_.production_kind kind = (_number_optional_.production_kind) p__number_optional_.kind;
			p__number_optional_.turn_reset ();
			if (Raise__number_optional__Event (SyntaxTreeCallbackReason.TraversalPrologueCallbackReason, kind, p__number_optional_))
			switch (kind)
			{
				case _number_optional_.production_kind.g__number_optional__1:
					// traverse syntax tree node associated with production
					// <number optional>: %empty

				break;
				case _number_optional_.production_kind.g__number_optional__2:
					// traverse syntax tree node associated with production
					// <number optional>: <number>

				break;
			}
			Raise__number_optional__Event (SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason, kind, p__number_optional_);
			p__number_optional_.unlock();
		}

		//
		// traverse syntax tree node associated with any production of syntax rule <number optional>
		//

		public void TraverseCommon (_number_optional_ p__number_optional_)
		{
			_number_optional_.production_kind kind = (_number_optional_.production_kind) p__number_optional_.kind;
			if (Raise_Common_Event (SyntaxTreeCallbackReason.TraversalPrologueCallbackReason, (int) kind, p__number_optional_))
			switch (kind)
			{
				case _number_optional_.production_kind.g__number_optional__1:
					// traverse syntax tree node associated with production
					// <number optional>: %empty

				break;
				case _number_optional_.production_kind.g__number_optional__2:
					// traverse syntax tree node associated with production
					// <number optional>: <number>

						TraverseCommon (p__number_optional_.m__number_);
				break;
			}
			Raise_Common_Event (SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason, (int) kind, p__number_optional_);
		}
	};
		#endregion

	public partial class SyntaxTreeBuilder : SyntaxTreeWalker
	{
		#region syntax tree builder methods associated with the syntax rule <number optional>

		//
		// create syntax tree node associated with production
		// <number optional>: %empty

		//

		public _number_optional_ _number_optional__1 ()
		{
			_number_optional_ p__number_optional__ref = new _number_optional_();
			Raise__number_optional__Event (SyntaxTreeCallbackReason.BuilderCallbackReason, _number_optional_.production_kind.g__number_optional__1, p__number_optional__ref);
			return p__number_optional__ref;
		}

		//
		// create syntax tree node associated with production
		// <number optional>: <number>

		//

		public _number_optional_ _number_optional__2 (SyntaxTreeToken p_token)
		{
			_number_optional_ p__number_optional__ref = new _number_optional_(p_token);
			p_token.parent = p__number_optional__ref;
			Raise__number_optional__Event (SyntaxTreeCallbackReason.BuilderCallbackReason, _number_optional_.production_kind.g__number_optional__2, p__number_optional__ref);
			return p__number_optional__ref;
		}
		#endregion
	};
}
