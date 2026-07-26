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
	// class associated with syntax rule <name value pair>
	//

	public class	_name_value_pair_ : SyntaxTreeBase
	{
		#region enumerated production(s) of syntax rule <name value pair>
		public enum production_kind : ushort	// enumerated production(s) of syntax rule <name value pair>
		{
			g__name_value_pair__1 = 1,	// <identifier> '=' <cstring>

		};
		#endregion
		#region production markers associated with the syntax rule <name value pair>

		// markers associated with production: <name value pair> -> <identifier> '=' <cstring>

		public enum production_marker_1 : ushort
		{
			final
		};
		#endregion
		#region Constructors and destructors associated with the syntax rule <name value pair>

		// Constructor declaration(s) associated with production(s) of syntax rule <name value pair>

		//
		// Constructor associated with the following production(s)
		// <name value pair> -> <identifier> '=' <cstring>

		//

		public _name_value_pair_ (SyntaxTreeToken p_token, SyntaxTreeToken p_token_1, SyntaxTreeToken p_token_2) : base ((uint) ProductionID.__name_value_pair__ID, (uint) production_kind.g__name_value_pair__1)
		{
			++g_counter;
			_init ();
			children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 3);
			children[0] = m__identifier_ = p_token;
			children[1] = m__equals_sign_ = p_token_1;
			children[2] = m__cstring_ = p_token_2;
		}

		// Copy constructor

		public _name_value_pair_ (_name_value_pair_ p__name_value_pair_) : base (p__name_value_pair_.id, p__name_value_pair_.kind)
		{
			++g_counter;
			_init ();
			switch ((production_kind) p__name_value_pair_.kind)
			{
				case production_kind.g__name_value_pair__1:
					children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 3);
					if ((children [0] = m__identifier_ = (p__name_value_pair_.m__identifier_ != null) ? new SyntaxTreeToken (p__name_value_pair_.m__identifier_) : null) != null) m__identifier_.parent = this;
					if ((children [1] = m__equals_sign_ = (p__name_value_pair_.m__equals_sign_ != null) ? new SyntaxTreeToken (p__name_value_pair_.m__equals_sign_) : null) != null) m__equals_sign_.parent = this;
					if ((children [2] = m__cstring_ = (p__name_value_pair_.m__cstring_ != null) ? new SyntaxTreeToken (p__name_value_pair_.m__cstring_) : null) != null) m__cstring_.parent = this;
					break;
				default:
				{
					string[] args = new string[] { "_name_value_pair_" };
					throw new SyntaxTreeError (SyntaxTreeError.InvalidKindError, args);
				}
			}
		}

		// Clone node - wrapper of copy constructor

		public override SyntaxTreeBase Clone () { return (SyntaxTreeBase) new _name_value_pair_ (this); }

		// Destructor

		public new void Dispose ()
		{
			base.Dispose ();
			--g_counter;

			m__identifier_?.Dispose ();
			m__equals_sign_?.Dispose ();
			m__cstring_?.Dispose ();

			_init ();
		}
		#endregion
		#region Content changing and inclusion checking methods associated with the syntax rule <name value pair>

		// Content changing function(s) associated with production(s) of syntax rule <name value pair>

		//
		// Content changing function associated with following production(s)
		// <name value pair> -> <identifier> '=' <cstring>

		//

		public void change(SyntaxTreeToken p_token, SyntaxTreeToken p_token_1, SyntaxTreeToken p_token_2)
		{
			_init ();
			this.kind = (uint) production_kind.g__name_value_pair__1;
			children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 3);
			children [0] = m__identifier_ = p_token;
			children [1] = m__equals_sign_ = p_token_1;
			children [2] = m__cstring_ = p_token_2;
			reparent (parent);
		}

		//
		// check if node 'element' is included within syntax tree associated with syntax rule <name value pair>
		//

		public new bool checkInclusion (SyntaxTreeBase element)
		{
			return
				(element == this) ||
				(m__identifier_ != null) && m__identifier_.checkInclusion (element) ||
				(m__equals_sign_ != null) && m__equals_sign_.checkInclusion (element) ||
				(m__cstring_ != null) && m__cstring_.checkInclusion (element) ||
				false;
		}
		#endregion
		#region Syntax tree emit methods associated with the syntax rule <name value pair>

		//
		// emit production tree node associated with any production of syntax rule <name value pair>
		//

		public override string Emit (int depth)
		{
			string s = "";
			if (depth != 0)
			switch ((production_kind) kind)
			{
				case production_kind.g__name_value_pair__1:
					// emit syntax tree node associated with production
					// <name value pair>: <identifier> '=' <cstring>

					s += m__identifier_.Emit (depth - 1);
					s += " ";
					s += m__equals_sign_.Emit (depth - 1);
					s += " ";
					s += m__cstring_.Emit (depth - 1);
				break;
			}
			return s.Trim ();
		}

		//
		// emit production tree node associated with any production of syntax rule <name value pair>
		//

		public override string EmitProductionTree (int depth)
		{
			string s = "_name_value_pair_ (";
			if (depth != 0)
				switch ((production_kind) kind)
				{
				case production_kind.g__name_value_pair__1:
					// emit syntax tree node associated with production
					// <name value pair>: <identifier> '=' <cstring>

					s += "_identifier_";
					s += ' ';
					s += "_equals_sign_";
					s += ' ';
					s += "_cstring_";
				break;
			}
			s += ")";
			return s;
		}
		#endregion
		#region Syntax tree traversal,  reperrent, init and iterator methods associated with the syntax rule <name value pair>

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
			m__equals_sign_?.reparent (this);
			m__cstring_?.reparent (this);
		}

		//
		// initialize object associated with syntax rule <name value pair>
		//

		public void _init ()
		{
			m__identifier_ = null;
			m__equals_sign_ = null;
			m__cstring_ = null;
		}
		#endregion
		#region Fields and properties associated with the syntax rule <name value pair>

		// counter of all nodes associated with syntax rule <name value pair>
		public static int g_counter;

		// objects associated with terminal and non-terminal symbols within production(s) of syntax rule <name value pair>
		public SyntaxTreeToken m__identifier_ { get; private set; }
		public SyntaxTreeToken m__equals_sign_ { get; private set; }
		public SyntaxTreeToken m__cstring_ { get; private set; }
		#endregion

	};

	public partial class SyntaxTreeWalker
	{
		#region delegates and events associated with the syntax rule <name value pair>

		// delegate function (callback) prototype associated with syntax rule <name value pair>
		public delegate bool _name_value_pair__Callback (SyntaxTreeCallbackReason reason, _name_value_pair_.production_kind kind, _name_value_pair_ p__name_value_pair_);

		// event associated with syntax rule <name value pair>
		public event _name_value_pair__Callback _name_value_pair__Event;

		// event trigger associated with syntax rule <name value pair>
		public bool Raise__name_value_pair__Event (SyntaxTreeCallbackReason reason, _name_value_pair_.production_kind kind, _name_value_pair_ p__name_value_pair_)
		{
			bool? status = _name_value_pair__Event?.Invoke (reason, kind, p__name_value_pair_);
			return (status == null) || status.Value;
		}
		#endregion
		#region syntax tree traversal methods associated with the syntax rule <name value pair>
		//
		// traverse syntax tree node associated with any production of syntax rule <name value pair>
		//

		public void Traverse (_name_value_pair_ p__name_value_pair_)
		{
			if (p__name_value_pair_.isLocked())
				return;
			p__name_value_pair_.dolock();
			_name_value_pair_.production_kind kind = (_name_value_pair_.production_kind) p__name_value_pair_.kind;
			p__name_value_pair_.turn_reset ();
			if (Raise__name_value_pair__Event (SyntaxTreeCallbackReason.TraversalPrologueCallbackReason, kind, p__name_value_pair_))
			switch (kind)
			{
				case _name_value_pair_.production_kind.g__name_value_pair__1:
					// traverse syntax tree node associated with production
					// <name value pair>: <identifier> '=' <cstring>

				break;
			}
			Raise__name_value_pair__Event (SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason, kind, p__name_value_pair_);
			p__name_value_pair_.unlock();
		}

		//
		// traverse syntax tree node associated with any production of syntax rule <name value pair>
		//

		public void TraverseCommon (_name_value_pair_ p__name_value_pair_)
		{
			_name_value_pair_.production_kind kind = (_name_value_pair_.production_kind) p__name_value_pair_.kind;
			if (Raise_Common_Event (SyntaxTreeCallbackReason.TraversalPrologueCallbackReason, (int) kind, p__name_value_pair_))
			switch (kind)
			{
				case _name_value_pair_.production_kind.g__name_value_pair__1:
					// traverse syntax tree node associated with production
					// <name value pair>: <identifier> '=' <cstring>

						TraverseCommon (p__name_value_pair_.m__identifier_);
						TraverseCommon (p__name_value_pair_.m__equals_sign_);
						TraverseCommon (p__name_value_pair_.m__cstring_);
				break;
			}
			Raise_Common_Event (SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason, (int) kind, p__name_value_pair_);
		}
	};
		#endregion

	public partial class SyntaxTreeBuilder : SyntaxTreeWalker
	{
		#region syntax tree builder methods associated with the syntax rule <name value pair>

		//
		// create syntax tree node associated with production
		// <name value pair>: <identifier> '=' <cstring>

		//

		public _name_value_pair_ _name_value_pair__1 (SyntaxTreeToken p_token, SyntaxTreeToken p_token_1, SyntaxTreeToken p_token_2)
		{
			_name_value_pair_ p__name_value_pair__ref = new _name_value_pair_(p_token, p_token_1, p_token_2);
			p_token.parent = p__name_value_pair__ref;
			p_token_1.parent = p__name_value_pair__ref;
			p_token_2.parent = p__name_value_pair__ref;
			Raise__name_value_pair__Event (SyntaxTreeCallbackReason.BuilderCallbackReason, _name_value_pair_.production_kind.g__name_value_pair__1, p__name_value_pair__ref);
			return p__name_value_pair__ref;
		}
		#endregion
	};
}
