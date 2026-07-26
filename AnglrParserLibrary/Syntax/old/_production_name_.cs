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
	// class associated with syntax rule <production name>
	//

	public class	_production_name_ : SyntaxTreeBase
	{
		#region enumerated production(s) of syntax rule <production name>
		public enum production_kind : ushort	// enumerated production(s) of syntax rule <production name>
		{
			g__production_name__1 = 1,	// '@@' <identifier>

		};
		#endregion
		#region production markers associated with the syntax rule <production name>

		// markers associated with production: <production name> -> '@@' <identifier>

		public enum production_marker_1 : ushort
		{
			final
		};
		#endregion
		#region Constructors and destructors associated with the syntax rule <production name>

		// Constructor declaration(s) associated with production(s) of syntax rule <production name>

		//
		// Constructor associated with the following production(s)
		// <production name> -> '@@' <identifier>

		//

		public _production_name_ (SyntaxTreeToken p_token, SyntaxTreeToken p_token_1) : base ((uint) ProductionID.__production_name__ID, (uint) production_kind.g__production_name__1)
		{
			++g_counter;
			_init ();
			children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 2);
			children[0] = m__double_at_sign_ = p_token;
			children[1] = m__identifier_ = p_token_1;
		}

		// Copy constructor

		public _production_name_ (_production_name_ p__production_name_) : base (p__production_name_.id, p__production_name_.kind)
		{
			++g_counter;
			_init ();
			switch ((production_kind) p__production_name_.kind)
			{
				case production_kind.g__production_name__1:
					children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 2);
					if ((children [0] = m__double_at_sign_ = (p__production_name_.m__double_at_sign_ != null) ? new SyntaxTreeToken (p__production_name_.m__double_at_sign_) : null) != null) m__double_at_sign_.parent = this;
					if ((children [1] = m__identifier_ = (p__production_name_.m__identifier_ != null) ? new SyntaxTreeToken (p__production_name_.m__identifier_) : null) != null) m__identifier_.parent = this;
					break;
				default:
				{
					string[] args = new string[] { "_production_name_" };
					throw new SyntaxTreeError (SyntaxTreeError.InvalidKindError, args);
				}
			}
		}

		// Clone node - wrapper of copy constructor

		public override SyntaxTreeBase Clone () { return (SyntaxTreeBase) new _production_name_ (this); }

		// Destructor

		public new void Dispose ()
		{
			base.Dispose ();
			--g_counter;

			m__double_at_sign_?.Dispose ();
			m__identifier_?.Dispose ();

			_init ();
		}
		#endregion
		#region Content changing and inclusion checking methods associated with the syntax rule <production name>

		// Content changing function(s) associated with production(s) of syntax rule <production name>

		//
		// Content changing function associated with following production(s)
		// <production name> -> '@@' <identifier>

		//

		public void change(SyntaxTreeToken p_token, SyntaxTreeToken p_token_1)
		{
			_init ();
			this.kind = (uint) production_kind.g__production_name__1;
			children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 2);
			children [0] = m__double_at_sign_ = p_token;
			children [1] = m__identifier_ = p_token_1;
			reparent (parent);
		}

		//
		// check if node 'element' is included within syntax tree associated with syntax rule <production name>
		//

		public new bool checkInclusion (SyntaxTreeBase element)
		{
			return
				(element == this) ||
				(m__double_at_sign_ != null) && m__double_at_sign_.checkInclusion (element) ||
				(m__identifier_ != null) && m__identifier_.checkInclusion (element) ||
				false;
		}
		#endregion
		#region Syntax tree emit methods associated with the syntax rule <production name>

		//
		// emit production tree node associated with any production of syntax rule <production name>
		//

		public override string Emit (int depth)
		{
			string s = "";
			if (depth != 0)
			switch ((production_kind) kind)
			{
				case production_kind.g__production_name__1:
					// emit syntax tree node associated with production
					// <production name>: '@@' <identifier>

					s += m__double_at_sign_.Emit (depth - 1);
					s += " ";
					s += m__identifier_.Emit (depth - 1);
				break;
			}
			return s.Trim ();
		}

		//
		// emit production tree node associated with any production of syntax rule <production name>
		//

		public override string EmitProductionTree (int depth)
		{
			string s = "_production_name_ (";
			if (depth != 0)
				switch ((production_kind) kind)
				{
				case production_kind.g__production_name__1:
					// emit syntax tree node associated with production
					// <production name>: '@@' <identifier>

					s += "_double_at_sign_";
					s += ' ';
					s += "_identifier_";
				break;
			}
			s += ")";
			return s;
		}
		#endregion
		#region Syntax tree traversal,  reperrent, init and iterator methods associated with the syntax rule <production name>

		//
		// traverse sub-tree rooted in this node using selected syntax tree walker
		//

		public override void InvokeTraverse (SyntaxTreeWalkerCore syntaxTreeWalker) => ((SyntaxTreeWalker) syntaxTreeWalker).Traverse (this);
		public override void InvokeTraverseCommon (SyntaxTreeWalkerCore syntaxTreeWalker) => ((SyntaxTreeWalker) syntaxTreeWalker).TraverseCommon (this);

		// reparent sub-tree

		public override void reparent (SyntaxTreeBase parent)
		{
			this.parent = parent;

			m__double_at_sign_?.reparent (this);
			m__identifier_?.reparent (this);
		}

		//
		// initialize object associated with syntax rule <production name>
		//

		public void _init ()
		{
			m__double_at_sign_ = null;
			m__identifier_ = null;
		}
		#endregion
		#region Fields and properties associated with the syntax rule <production name>

		// counter of all nodes associated with syntax rule <production name>
		public static int g_counter;

		// objects associated with terminal and non-terminal symbols within production(s) of syntax rule <production name>
		public SyntaxTreeToken m__double_at_sign_ { get; private set; }
		public SyntaxTreeToken m__identifier_ { get; private set; }
		#endregion

	};

	public partial class SyntaxTreeWalker
	{
		#region delegates and events associated with the syntax rule <production name>

		// delegate function (callback) prototype associated with syntax rule <production name>
		public delegate bool _production_name__Callback (SyntaxTreeCallbackReason reason, _production_name_.production_kind kind, _production_name_ p__production_name_);

		// event associated with syntax rule <production name>
		public event _production_name__Callback _production_name__Event;

		// event trigger associated with syntax rule <production name>
		public bool Raise__production_name__Event (SyntaxTreeCallbackReason reason, _production_name_.production_kind kind, _production_name_ p__production_name_)
		{
			bool? status = _production_name__Event?.Invoke (reason, kind, p__production_name_);
			return (status == null) || status.Value;
		}
		#endregion
		#region syntax tree traversal methods associated with the syntax rule <production name>
		//
		// traverse syntax tree node associated with any production of syntax rule <production name>
		//

		public void Traverse (_production_name_ p__production_name_)
		{
			if (p__production_name_.isLocked())
				return;
			p__production_name_.dolock();
			_production_name_.production_kind kind = (_production_name_.production_kind) p__production_name_.kind;
			p__production_name_.turn_reset ();
			if (Raise__production_name__Event (SyntaxTreeCallbackReason.TraversalPrologueCallbackReason, kind, p__production_name_))
			switch (kind)
			{
				case _production_name_.production_kind.g__production_name__1:
					// traverse syntax tree node associated with production
					// <production name>: '@@' <identifier>

				break;
			}
			Raise__production_name__Event (SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason, kind, p__production_name_);
			p__production_name_.unlock();
		}

		//
		// traverse syntax tree node associated with any production of syntax rule <production name>
		//

		public void TraverseCommon (_production_name_ p__production_name_)
		{
			_production_name_.production_kind kind = (_production_name_.production_kind) p__production_name_.kind;
			if (Raise_Common_Event (SyntaxTreeCallbackReason.TraversalPrologueCallbackReason, (int) kind, p__production_name_))
			switch (kind)
			{
				case _production_name_.production_kind.g__production_name__1:
					// traverse syntax tree node associated with production
					// <production name>: '@@' <identifier>

						TraverseCommon (p__production_name_.m__double_at_sign_);
						TraverseCommon (p__production_name_.m__identifier_);
				break;
			}
			Raise_Common_Event (SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason, (int) kind, p__production_name_);
		}
	};
		#endregion

	public partial class SyntaxTreeBuilder : SyntaxTreeWalker
	{
		#region syntax tree builder methods associated with the syntax rule <production name>

		//
		// create syntax tree node associated with production
		// <production name>: '@@' <identifier>

		//

		public _production_name_ _production_name__1 (SyntaxTreeToken p_token, SyntaxTreeToken p_token_1)
		{
			_production_name_ p__production_name__ref = new _production_name_(p_token, p_token_1);
			p_token.parent = p__production_name__ref;
			p_token_1.parent = p__production_name__ref;
			Raise__production_name__Event (SyntaxTreeCallbackReason.BuilderCallbackReason, _production_name_.production_kind.g__production_name__1, p__production_name__ref);
			return p__production_name__ref;
		}
		#endregion
	};
}
