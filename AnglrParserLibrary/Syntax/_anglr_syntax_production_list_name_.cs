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
	// class associated with syntax rule <anglr syntax production list name>
	//

	public class	_anglr_syntax_production_list_name_ : SyntaxTreeBase
	{
		#region enumerated production(s) of syntax rule <anglr syntax production list name>
		public enum production_kind : ushort	// enumerated production(s) of syntax rule <anglr syntax production list name>
		{
			g__anglr_syntax_production_list_name__1 = 1,	// ':' <identifier> ':'

		};
		#endregion
		#region production markers associated with the syntax rule <anglr syntax production list name>

		// markers associated with production: <anglr syntax production list name> -> ':' <identifier> ':'

		public enum production_marker_1 : ushort
		{
			final
		};
		#endregion
		#region Constructors and destructors associated with the syntax rule <anglr syntax production list name>

		// Constructor declaration(s) associated with production(s) of syntax rule <anglr syntax production list name>

		//
		// Constructor associated with the following production(s)
		// <anglr syntax production list name> -> ':' <identifier> ':'

		//

		public _anglr_syntax_production_list_name_ (SyntaxTreeToken p_token, SyntaxTreeToken p_token_1, SyntaxTreeToken p_token_2) : base ((uint) ProductionID.__anglr_syntax_production_list_name__ID, (uint) production_kind.g__anglr_syntax_production_list_name__1)
		{
			++g_counter;
			_init ();
			children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 3);
			children[0] = m__colon_ = p_token;
			children[1] = m__identifier_ = p_token_1;
			children[2] = m__colon__1 = p_token_2;
		}

		// Copy constructor

		public _anglr_syntax_production_list_name_ (_anglr_syntax_production_list_name_ p__anglr_syntax_production_list_name_) : base (p__anglr_syntax_production_list_name_.id, p__anglr_syntax_production_list_name_.kind)
		{
			++g_counter;
			_init ();
			switch ((production_kind) p__anglr_syntax_production_list_name_.kind)
			{
				case production_kind.g__anglr_syntax_production_list_name__1:
					children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 3);
					if ((children [0] = m__colon_ = (p__anglr_syntax_production_list_name_.m__colon_ != null) ? new SyntaxTreeToken (p__anglr_syntax_production_list_name_.m__colon_) : null) != null) m__colon_.parent = this;
					if ((children [1] = m__identifier_ = (p__anglr_syntax_production_list_name_.m__identifier_ != null) ? new SyntaxTreeToken (p__anglr_syntax_production_list_name_.m__identifier_) : null) != null) m__identifier_.parent = this;
					if ((children [2] = m__colon__1 = (p__anglr_syntax_production_list_name_.m__colon__1 != null) ? new SyntaxTreeToken (p__anglr_syntax_production_list_name_.m__colon__1) : null) != null) m__colon__1.parent = this;
					break;
				default:
				{
					string[] args = new string[] { "_anglr_syntax_production_list_name_" };
					throw new SyntaxTreeError (SyntaxTreeError.InvalidKindError, args);
				}
			}
		}

		// Clone node - wrapper of copy constructor

		public override SyntaxTreeBase Clone () { return (SyntaxTreeBase) new _anglr_syntax_production_list_name_ (this); }

		// Destructor

		public new void Dispose ()
		{
			base.Dispose ();
			--g_counter;

			m__colon_?.Dispose ();
			m__colon__1?.Dispose ();
			m__identifier_?.Dispose ();

			_init ();
		}
		#endregion
		#region Content changing and inclusion checking methods associated with the syntax rule <anglr syntax production list name>

		// Content changing function(s) associated with production(s) of syntax rule <anglr syntax production list name>

		//
		// Content changing function associated with following production(s)
		// <anglr syntax production list name> -> ':' <identifier> ':'

		//

		public void change(SyntaxTreeToken p_token, SyntaxTreeToken p_token_1, SyntaxTreeToken p_token_2)
		{
			_init ();
			this.kind = (uint) production_kind.g__anglr_syntax_production_list_name__1;
			children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 3);
			children [0] = m__colon_ = p_token;
			children [1] = m__identifier_ = p_token_1;
			children [2] = m__colon__1 = p_token_2;
			reparent (parent);
		}

		//
		// check if node 'element' is included within syntax tree associated with syntax rule <anglr syntax production list name>
		//

		public new bool checkInclusion (SyntaxTreeBase element)
		{
			return
				(element == this) ||
				(m__colon_ != null) && m__colon_.checkInclusion (element) ||
				(m__colon__1 != null) && m__colon__1.checkInclusion (element) ||
				(m__identifier_ != null) && m__identifier_.checkInclusion (element) ||
				false;
		}
		#endregion
		#region Syntax tree emit methods associated with the syntax rule <anglr syntax production list name>

		//
		// emit production tree node associated with any production of syntax rule <anglr syntax production list name>
		//

		public override string Emit (int depth)
		{
			string s = "";
			if (depth != 0)
			switch ((production_kind) kind)
			{
				case production_kind.g__anglr_syntax_production_list_name__1:
					// emit syntax tree node associated with production
					// <anglr syntax production list name>: ':' <identifier> ':'

					s += m__colon_.Emit (depth - 1);
					s += " ";
					s += m__identifier_.Emit (depth - 1);
					s += " ";
					s += m__colon__1.Emit (depth - 1);
				break;
			}
			return s.Trim ();
		}

		//
		// emit production tree node associated with any production of syntax rule <anglr syntax production list name>
		//

		public override string EmitProductionTree (int depth)
		{
			string s = "_anglr_syntax_production_list_name_ (";
			if (depth != 0)
				switch ((production_kind) kind)
				{
				case production_kind.g__anglr_syntax_production_list_name__1:
					// emit syntax tree node associated with production
					// <anglr syntax production list name>: ':' <identifier> ':'

					s += "_colon_";
					s += ' ';
					s += "_identifier_";
					s += ' ';
					s += "_colon_";
				break;
			}
			s += ")";
			return s;
		}
		#endregion
		#region Syntax tree traversal,  reperrent, init and iterator methods associated with the syntax rule <anglr syntax production list name>

		//
		// traverse sub-tree rooted in this node using selected syntax tree walker
		//

		public override void InvokeTraverse (SyntaxTreeWalkerCore syntaxTreeWalker) => ((SyntaxTreeWalker) syntaxTreeWalker).Traverse (this);
		public override void InvokeTraverseCommon (SyntaxTreeWalkerCore syntaxTreeWalker) => ((SyntaxTreeWalker) syntaxTreeWalker).TraverseCommon (this);

		// reparent sub-tree

		public override void reparent (SyntaxTreeBase parent)
		{
			this.parent = parent;

			m__colon_?.reparent (this);
			m__colon__1?.reparent (this);

			m__identifier_?.reparent (this);
		}

		//
		// initialize object associated with syntax rule <anglr syntax production list name>
		//

		public void _init ()
		{
			m__colon_ = null;
			m__colon__1 = null;
			m__identifier_ = null;
		}
		#endregion
		#region Fields and properties associated with the syntax rule <anglr syntax production list name>

		// counter of all nodes associated with syntax rule <anglr syntax production list name>
		public static int g_counter;

		// objects associated with terminal and non-terminal symbols within production(s) of syntax rule <anglr syntax production list name>
		public SyntaxTreeToken m__colon_ { get; private set; }
		public SyntaxTreeToken m__colon__1 { get; private set; }
		public SyntaxTreeToken m__identifier_ { get; private set; }
		#endregion

	};

	public partial class SyntaxTreeWalker
	{
		#region delegates and events associated with the syntax rule <anglr syntax production list name>

		// delegate function (callback) prototype associated with syntax rule <anglr syntax production list name>
		public delegate bool _anglr_syntax_production_list_name__Callback (SyntaxTreeCallbackReason reason, _anglr_syntax_production_list_name_.production_kind kind, _anglr_syntax_production_list_name_ p__anglr_syntax_production_list_name_);

		// event associated with syntax rule <anglr syntax production list name>
		public event _anglr_syntax_production_list_name__Callback _anglr_syntax_production_list_name__Event;

		// event trigger associated with syntax rule <anglr syntax production list name>
		public bool Raise__anglr_syntax_production_list_name__Event (SyntaxTreeCallbackReason reason, _anglr_syntax_production_list_name_.production_kind kind, _anglr_syntax_production_list_name_ p__anglr_syntax_production_list_name_)
		{
			bool? status = _anglr_syntax_production_list_name__Event?.Invoke (reason, kind, p__anglr_syntax_production_list_name_);
			return (status == null) || status.Value;
		}
		#endregion
		#region syntax tree traversal methods associated with the syntax rule <anglr syntax production list name>
		//
		// traverse syntax tree node associated with any production of syntax rule <anglr syntax production list name>
		//

		public void Traverse (_anglr_syntax_production_list_name_ p__anglr_syntax_production_list_name_)
		{
			if (p__anglr_syntax_production_list_name_.isLocked())
				return;
			p__anglr_syntax_production_list_name_.dolock();
			_anglr_syntax_production_list_name_.production_kind kind = (_anglr_syntax_production_list_name_.production_kind) p__anglr_syntax_production_list_name_.kind;
			p__anglr_syntax_production_list_name_.turn_reset ();
			if (Raise__anglr_syntax_production_list_name__Event (SyntaxTreeCallbackReason.TraversalPrologueCallbackReason, kind, p__anglr_syntax_production_list_name_))
			switch (kind)
			{
				case _anglr_syntax_production_list_name_.production_kind.g__anglr_syntax_production_list_name__1:
					// traverse syntax tree node associated with production
					// <anglr syntax production list name>: ':' <identifier> ':'

				break;
			}
			Raise__anglr_syntax_production_list_name__Event (SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason, kind, p__anglr_syntax_production_list_name_);
			p__anglr_syntax_production_list_name_.unlock();
		}

		//
		// traverse syntax tree node associated with any production of syntax rule <anglr syntax production list name>
		//

		public void TraverseCommon (_anglr_syntax_production_list_name_ p__anglr_syntax_production_list_name_)
		{
			_anglr_syntax_production_list_name_.production_kind kind = (_anglr_syntax_production_list_name_.production_kind) p__anglr_syntax_production_list_name_.kind;
			if (Raise_Common_Event (SyntaxTreeCallbackReason.TraversalPrologueCallbackReason, (int) kind, p__anglr_syntax_production_list_name_))
			switch (kind)
			{
				case _anglr_syntax_production_list_name_.production_kind.g__anglr_syntax_production_list_name__1:
					// traverse syntax tree node associated with production
					// <anglr syntax production list name>: ':' <identifier> ':'

						TraverseCommon (p__anglr_syntax_production_list_name_.m__colon_);
						TraverseCommon (p__anglr_syntax_production_list_name_.m__identifier_);
						TraverseCommon (p__anglr_syntax_production_list_name_.m__colon__1);
				break;
			}
			Raise_Common_Event (SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason, (int) kind, p__anglr_syntax_production_list_name_);
		}
	};
		#endregion

	public partial class SyntaxTreeBuilder : SyntaxTreeWalker
	{
		#region syntax tree builder methods associated with the syntax rule <anglr syntax production list name>

		//
		// create syntax tree node associated with production
		// <anglr syntax production list name>: ':' <identifier> ':'

		//

		public _anglr_syntax_production_list_name_ _anglr_syntax_production_list_name__1 (SyntaxTreeToken p_token, SyntaxTreeToken p_token_1, SyntaxTreeToken p_token_2)
		{
			_anglr_syntax_production_list_name_ p__anglr_syntax_production_list_name__ref = new _anglr_syntax_production_list_name_(p_token, p_token_1, p_token_2);
			p_token.parent = p__anglr_syntax_production_list_name__ref;
			p_token_1.parent = p__anglr_syntax_production_list_name__ref;
			p_token_2.parent = p__anglr_syntax_production_list_name__ref;
			Raise__anglr_syntax_production_list_name__Event (SyntaxTreeCallbackReason.BuilderCallbackReason, _anglr_syntax_production_list_name_.production_kind.g__anglr_syntax_production_list_name__1, p__anglr_syntax_production_list_name__ref);
			return p__anglr_syntax_production_list_name__ref;
		}
		#endregion
	};
}
