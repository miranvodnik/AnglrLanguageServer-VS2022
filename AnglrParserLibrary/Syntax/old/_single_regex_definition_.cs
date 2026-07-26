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
	// class associated with syntax rule <single regex definition>
	//

	public class	_single_regex_definition_ : SyntaxTreeBase
	{
		#region enumerated production(s) of syntax rule <single regex definition>
		public enum production_kind : ushort	// enumerated production(s) of syntax rule <single regex definition>
		{
			g__single_regex_definition__1 = 1,	// '%regex' <regex definition>

		};
		#endregion
		#region production markers associated with the syntax rule <single regex definition>

		// markers associated with production: <single regex definition> -> '%regex' <regex definition>

		public enum production_marker_1 : ushort
		{
			m__regex_definition_,
			final
		};
		#endregion
		#region Constructors and destructors associated with the syntax rule <single regex definition>

		// Constructor declaration(s) associated with production(s) of syntax rule <single regex definition>

		//
		// Constructor associated with the following production(s)
		// <single regex definition> -> '%regex' <regex definition>

		//

		public _single_regex_definition_ (SyntaxTreeToken p_token, _regex_definition_ p__regex_definition_) : base ((uint) ProductionID.__single_regex_definition__ID, (uint) production_kind.g__single_regex_definition__1)
		{
			++g_counter;
			_init ();
			children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 2);
			children[0] = m__regex_ = p_token;
			children[1] = m__regex_definition_ = p__regex_definition_;
		}

		// Copy constructor

		public _single_regex_definition_ (_single_regex_definition_ p__single_regex_definition_) : base (p__single_regex_definition_.id, p__single_regex_definition_.kind)
		{
			++g_counter;
			_init ();
			switch ((production_kind) p__single_regex_definition_.kind)
			{
				case production_kind.g__single_regex_definition__1:
					children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 2);
					if ((children [0] = m__regex_ = (p__single_regex_definition_.m__regex_ != null) ? new SyntaxTreeToken (p__single_regex_definition_.m__regex_) : null) != null) m__regex_.parent = this;
					if ((children [1] = m__regex_definition_ = (p__single_regex_definition_.m__regex_definition_ != null) ? new _regex_definition_ (p__single_regex_definition_.m__regex_definition_) : null) != null) m__regex_definition_.parent = this;
					break;
				default:
				{
					string[] args = new string[] { "_single_regex_definition_" };
					throw new SyntaxTreeError (SyntaxTreeError.InvalidKindError, args);
				}
			}
		}

		// Clone node - wrapper of copy constructor

		public override SyntaxTreeBase Clone () { return (SyntaxTreeBase) new _single_regex_definition_ (this); }

		// Destructor

		public new void Dispose ()
		{
			base.Dispose ();
			--g_counter;

			m__regex_?.Dispose ();
			m__regex_definition_?.Dispose ();

			_init ();
		}
		#endregion
		#region Content changing and inclusion checking methods associated with the syntax rule <single regex definition>

		// Content changing function(s) associated with production(s) of syntax rule <single regex definition>

		//
		// Content changing function associated with following production(s)
		// <single regex definition> -> '%regex' <regex definition>

		//

		public void change(SyntaxTreeToken p_token, _regex_definition_ p__regex_definition_)
		{
			_init ();
			this.kind = (uint) production_kind.g__single_regex_definition__1;
			children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 2);
			children [0] = m__regex_ = p_token;
			children [1] = m__regex_definition_ = p__regex_definition_;
			reparent (parent);
		}

		//
		// check if node 'element' is included within syntax tree associated with syntax rule <single regex definition>
		//

		public new bool checkInclusion (SyntaxTreeBase element)
		{
			return
				(element == this) ||
				(m__regex_ != null) && m__regex_.checkInclusion (element) ||
				(m__regex_definition_ != null) && m__regex_definition_.checkInclusion (element) ||
				false;
		}
		#endregion
		#region Syntax tree emit methods associated with the syntax rule <single regex definition>

		//
		// emit production tree node associated with any production of syntax rule <single regex definition>
		//

		public override string Emit (int depth)
		{
			string s = "";
			if (depth != 0)
			switch ((production_kind) kind)
			{
				case production_kind.g__single_regex_definition__1:
					// emit syntax tree node associated with production
					// <single regex definition>: '%regex' <regex definition>

					s += m__regex_.Emit (depth - 1);
					s += " ";
					s += m__regex_definition_.Emit (depth - 1);
				break;
			}
			return s.Trim ();
		}

		//
		// emit production tree node associated with any production of syntax rule <single regex definition>
		//

		public override string EmitProductionTree (int depth)
		{
			string s = "_single_regex_definition_ (";
			if (depth != 0)
				switch ((production_kind) kind)
				{
				case production_kind.g__single_regex_definition__1:
					// emit syntax tree node associated with production
					// <single regex definition>: '%regex' <regex definition>

					s += "_regex_";
					s += ' ';
					s += m__regex_definition_.EmitProductionTree (depth - 1);
				break;
			}
			s += ")";
			return s;
		}
		#endregion
		#region Syntax tree traversal,  reperrent, init and iterator methods associated with the syntax rule <single regex definition>

		//
		// traverse sub-tree rooted in this node using selected syntax tree walker
		//

		public override void InvokeTraverse (SyntaxTreeWalkerCore syntaxTreeWalker) => ((SyntaxTreeWalker) syntaxTreeWalker).Traverse (this);
		public override void InvokeTraverseCommon (SyntaxTreeWalkerCore syntaxTreeWalker) => ((SyntaxTreeWalker) syntaxTreeWalker).TraverseCommon (this);

		// reparent sub-tree

		public override void reparent (SyntaxTreeBase parent)
		{
			this.parent = parent;

			m__regex_?.reparent (this);
			m__regex_definition_?.reparent (this);
		}

		//
		// initialize object associated with syntax rule <single regex definition>
		//

		public void _init ()
		{
			m__regex_ = null;
			m__regex_definition_ = null;
		}
		#endregion
		#region Fields and properties associated with the syntax rule <single regex definition>

		// counter of all nodes associated with syntax rule <single regex definition>
		public static int g_counter;

		// objects associated with terminal and non-terminal symbols within production(s) of syntax rule <single regex definition>
		public SyntaxTreeToken m__regex_ { get; private set; }
		public _regex_definition_ m__regex_definition_ { get; private set; }
		#endregion

	};

	public partial class SyntaxTreeWalker
	{
		#region delegates and events associated with the syntax rule <single regex definition>

		// delegate function (callback) prototype associated with syntax rule <single regex definition>
		public delegate bool _single_regex_definition__Callback (SyntaxTreeCallbackReason reason, _single_regex_definition_.production_kind kind, _single_regex_definition_ p__single_regex_definition_);

		// event associated with syntax rule <single regex definition>
		public event _single_regex_definition__Callback _single_regex_definition__Event;

		// event trigger associated with syntax rule <single regex definition>
		public bool Raise__single_regex_definition__Event (SyntaxTreeCallbackReason reason, _single_regex_definition_.production_kind kind, _single_regex_definition_ p__single_regex_definition_)
		{
			bool? status = _single_regex_definition__Event?.Invoke (reason, kind, p__single_regex_definition_);
			return (status == null) || status.Value;
		}
		#endregion
		#region syntax tree traversal methods associated with the syntax rule <single regex definition>
		//
		// traverse syntax tree node associated with any production of syntax rule <single regex definition>
		//

		public void Traverse (_single_regex_definition_ p__single_regex_definition_)
		{
			if (p__single_regex_definition_.isLocked())
				return;
			p__single_regex_definition_.dolock();
			_single_regex_definition_.production_kind kind = (_single_regex_definition_.production_kind) p__single_regex_definition_.kind;
			p__single_regex_definition_.turn_reset ();
			if (Raise__single_regex_definition__Event (SyntaxTreeCallbackReason.TraversalPrologueCallbackReason, kind, p__single_regex_definition_))
			switch (kind)
			{
				case _single_regex_definition_.production_kind.g__single_regex_definition__1:
					// traverse syntax tree node associated with production
					// <single regex definition>: '%regex' <regex definition>

					if (Raise__single_regex_definition__Event (SyntaxTreeCallbackReason.TraversalMidTermCallbackReason, kind, p__single_regex_definition_))
						Traverse (p__single_regex_definition_.m__regex_definition_);
					p__single_regex_definition_.turn_inc ();
					Raise__single_regex_definition__Event (SyntaxTreeCallbackReason.TraversalMidTermCallbackReason, kind, p__single_regex_definition_);
				break;
			}
			Raise__single_regex_definition__Event (SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason, kind, p__single_regex_definition_);
			p__single_regex_definition_.unlock();
		}

		//
		// traverse syntax tree node associated with any production of syntax rule <single regex definition>
		//

		public void TraverseCommon (_single_regex_definition_ p__single_regex_definition_)
		{
			_single_regex_definition_.production_kind kind = (_single_regex_definition_.production_kind) p__single_regex_definition_.kind;
			if (Raise_Common_Event (SyntaxTreeCallbackReason.TraversalPrologueCallbackReason, (int) kind, p__single_regex_definition_))
			switch (kind)
			{
				case _single_regex_definition_.production_kind.g__single_regex_definition__1:
					// traverse syntax tree node associated with production
					// <single regex definition>: '%regex' <regex definition>

						TraverseCommon (p__single_regex_definition_.m__regex_);
						TraverseCommon (p__single_regex_definition_.m__regex_definition_);
				break;
			}
			Raise_Common_Event (SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason, (int) kind, p__single_regex_definition_);
		}
	};
		#endregion

	public partial class SyntaxTreeBuilder : SyntaxTreeWalker
	{
		#region syntax tree builder methods associated with the syntax rule <single regex definition>

		//
		// create syntax tree node associated with production
		// <single regex definition>: '%regex' <regex definition>

		//

		public _single_regex_definition_ _single_regex_definition__1 (SyntaxTreeToken p_token, _regex_definition_ p__regex_definition_)
		{
			_single_regex_definition_ p__single_regex_definition__ref = new _single_regex_definition_(p_token, p__regex_definition_);
			p_token.parent = p__single_regex_definition__ref;
			p__regex_definition_.parent = p__single_regex_definition__ref;
			Raise__single_regex_definition__Event (SyntaxTreeCallbackReason.BuilderCallbackReason, _single_regex_definition_.production_kind.g__single_regex_definition__1, p__single_regex_definition__ref);
			return p__single_regex_definition__ref;
		}
		#endregion
	};
}
