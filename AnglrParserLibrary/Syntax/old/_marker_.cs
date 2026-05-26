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
	// class associated with syntax rule <marker>
	//

	public class	_marker_ : SyntaxTreeBase
	{
		#region enumerated production(s) of syntax rule <marker>
		public enum production_kind : ushort	// enumerated production(s) of syntax rule <marker>
		{
			g__marker__1 = 1,	// '@' <identifier>

		};
		#endregion
		#region production markers associated with the syntax rule <marker>

		// markers associated with production: <marker> -> '@' <identifier>

		public enum production_marker_1 : ushort
		{
			final
		};
		#endregion
		#region Constructors and destructors associated with the syntax rule <marker>

		// Constructor declaration(s) associated with production(s) of syntax rule <marker>

		//
		// Constructor associated with the following production(s)
		// <marker> -> '@' <identifier>

		//

		public _marker_ (SyntaxTreeToken p_token, SyntaxTreeToken p_token_1) : base ((uint) ProductionID.__marker__ID, (uint) production_kind.g__marker__1)
		{
			++g_counter;
			_init ();
			children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 2);
			children[0] = m__at_sign_ = p_token;
			children[1] = m__identifier_ = p_token_1;
		}

		// Copy constructor

		public _marker_ (_marker_ p__marker_) : base (p__marker_.id, p__marker_.kind)
		{
			++g_counter;
			_init ();
			switch ((production_kind) this.kind)
			{
			case production_kind.g__marker__1:
				children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 2);
				children[0] = m__at_sign_ = p__marker_.m__at_sign_;
				children[1] = m__identifier_ = p__marker_.m__identifier_;
				break;
			default:
				string[] args = new string[] { "_marker_" };
				throw new SyntaxTreeError (SyntaxTreeError.InvalidKindError, args);
			}
		}

		// Clone node - wrapper of copy constructor

		public override SyntaxTreeBase Clone () { return (SyntaxTreeBase) new _marker_ (this); }

		// Destructor

		public new void Dispose ()
		{
			base.Dispose ();
			--g_counter;

			m__at_sign_?.Dispose ();
			m__identifier_?.Dispose ();

			_init ();
		}
		#endregion
		#region Content changing and inclusion checking methods associated with the syntax rule <marker>

		// Content changing function(s) associated with production(s) of syntax rule <marker>

		//
		// Content changing function associated with following production(s)
		// <marker> -> '@' <identifier>

		//

		public void change(SyntaxTreeToken p_token, SyntaxTreeToken p_token_1)
		{
			_init ();
			this.kind = (uint) production_kind.g__marker__1;
			children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 2);
			children [0] = m__at_sign_ = p_token;
			children [1] = m__identifier_ = p_token_1;
			reparent (parent);
		}

		//
		// check if node 'element' is included within syntax tree associated with syntax rule <marker>
		//

		public new bool checkInclusion (SyntaxTreeBase element)
		{
			return
				(element == this) ||
				(m__at_sign_ != null) && m__at_sign_.checkInclusion (element) ||
				(m__identifier_ != null) && m__identifier_.checkInclusion (element) ||
				false;
		}
		#endregion
		#region Syntax tree emit methods associated with the syntax rule <marker>

		//
		// emit production tree node associated with any production of syntax rule <marker>
		//

		public override string Emit (int depth)
		{
			string s = "";
			if (depth != 0)
			switch ((production_kind) kind)
			{
				case production_kind.g__marker__1:
					// emit syntax tree node associated with production
					// <marker>: '@' <identifier>

					s += m__at_sign_.Emit (depth - 1);
					s += " ";
					s += m__identifier_.Emit (depth - 1);
				break;
			}
			return s.Trim ();
		}

		//
		// emit production tree node associated with any production of syntax rule <marker>
		//

		public override string EmitProductionTree (int depth)
		{
			string s = "_marker_ (";
			if (depth != 0)
				switch ((production_kind) kind)
				{
				case production_kind.g__marker__1:
					// emit syntax tree node associated with production
					// <marker>: '@' <identifier>

					s += "_at_sign_";
					s += ' ';
					s += "_identifier_";
				break;
			}
			s += ")";
			return s;
		}
		#endregion
		#region Syntax tree traversal,  reperrent, init and iterator methods associated with the syntax rule <marker>

		//
		// traverse sub-tree rooted in this node using selected syntax tree walker
		//

		public override void InvokeTraverse (SyntaxTreeWalkerCore syntaxTreeWalker) => ((SyntaxTreeWalker) syntaxTreeWalker).Traverse (this);
		public override void InvokeTraverseCommon (SyntaxTreeWalkerCore syntaxTreeWalker) => ((SyntaxTreeWalker) syntaxTreeWalker).TraverseCommon (this);

		// reparent sub-tree

		public override void reparent (SyntaxTreeBase parent)
		{
			this.parent = parent;

			m__at_sign_?.reparent (this);
			m__identifier_?.reparent (this);
		}

		//
		// initialize object associated with syntax rule <marker>
		//

		public void _init ()
		{
			m__at_sign_ = null;
			m__identifier_ = null;
		}
		#endregion
		#region Fields and properties associated with the syntax rule <marker>

		// counter of all nodes associated with syntax rule <marker>
		public static int g_counter;

		// objects associated with terminal and non-terminal symbols within production(s) of syntax rule <marker>
		public SyntaxTreeToken m__at_sign_ { get; private set; }
		public SyntaxTreeToken m__identifier_ { get; private set; }
		#endregion

	};

	public partial class SyntaxTreeWalker
	{
		#region delegates and events associated with the syntax rule <marker>

		// delegate function (callback) prototype associated with syntax rule <marker>
		public delegate bool _marker__Callback (SyntaxTreeCallbackReason reason, _marker_.production_kind kind, _marker_ p__marker_);

		// event associated with syntax rule <marker>
		public event _marker__Callback _marker__Event;

		// event trigger associated with syntax rule <marker>
		public bool Raise__marker__Event (SyntaxTreeCallbackReason reason, _marker_.production_kind kind, _marker_ p__marker_)
		{
			bool? status = _marker__Event?.Invoke (reason, kind, p__marker_);
			return (status == null) || status.Value;
		}
		#endregion
		#region syntax tree traversal methods associated with the syntax rule <marker>
		//
		// traverse syntax tree node associated with any production of syntax rule <marker>
		//

		public void Traverse (_marker_ p__marker_)
		{
			if (p__marker_.isLocked())
				return;
			p__marker_.dolock();
			_marker_.production_kind kind = (_marker_.production_kind) p__marker_.kind;
			p__marker_.turn_reset ();
			if (Raise__marker__Event (SyntaxTreeCallbackReason.TraversalPrologueCallbackReason, kind, p__marker_))
			switch (kind)
			{
				case _marker_.production_kind.g__marker__1:
					// traverse syntax tree node associated with production
					// <marker>: '@' <identifier>

				break;
			}
			Raise__marker__Event (SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason, kind, p__marker_);
			p__marker_.unlock();
		}

		//
		// traverse syntax tree node associated with any production of syntax rule <marker>
		//

		public void TraverseCommon (_marker_ p__marker_)
		{
			_marker_.production_kind kind = (_marker_.production_kind) p__marker_.kind;
			if (Raise_Common_Event (SyntaxTreeCallbackReason.TraversalPrologueCallbackReason, (int) kind, p__marker_))
			switch (kind)
			{
				case _marker_.production_kind.g__marker__1:
					// traverse syntax tree node associated with production
					// <marker>: '@' <identifier>

						TraverseCommon (p__marker_.m__at_sign_);
						TraverseCommon (p__marker_.m__identifier_);
				break;
			}
			Raise_Common_Event (SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason, (int) kind, p__marker_);
		}
	};
		#endregion

	public partial class SyntaxTreeBuilder : SyntaxTreeWalker
	{
		#region syntax tree builder methods associated with the syntax rule <marker>

		//
		// create syntax tree node associated with production
		// <marker>: '@' <identifier>

		//

		public _marker_ _marker__1 (SyntaxTreeToken p_token, SyntaxTreeToken p_token_1)
		{
			_marker_ p__marker__ref = new _marker_(p_token, p_token_1);
			p_token.parent = p__marker__ref;
			p_token_1.parent = p__marker__ref;
			Raise__marker__Event (SyntaxTreeCallbackReason.BuilderCallbackReason, _marker_.production_kind.g__marker__1, p__marker__ref);
			return p__marker__ref;
		}
		#endregion
	};
}
