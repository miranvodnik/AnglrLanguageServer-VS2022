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
	// class associated with syntax rule <block of regex definitions>
	//

	public class	_block_of_regex_definitions_ : SyntaxTreeBase
	{
		#region enumerated production(s) of syntax rule <block of regex definitions>
		public enum production_kind : ushort	// enumerated production(s) of syntax rule <block of regex definitions>
		{
			g__block_of_regex_definitions__1 = 1,	// '%regex' '{' <block regex definitions optional> '}'

		};
		#endregion
		#region production markers associated with the syntax rule <block of regex definitions>

		// markers associated with production: <block of regex definitions> -> '%regex' '{' <block regex definitions optional> '}'

		public enum production_marker_1 : ushort
		{
			m__block_regex_definitions_optional_,
			final
		};
		#endregion
		#region Constructors and destructors associated with the syntax rule <block of regex definitions>

		// Constructor declaration(s) associated with production(s) of syntax rule <block of regex definitions>

		//
		// Constructor associated with the following production(s)
		// <block of regex definitions> -> '%regex' '{' <block regex definitions optional> '}'

		//

		public _block_of_regex_definitions_ (SyntaxTreeToken p_token, SyntaxTreeToken p_token_1, _block_regex_definitions_optional_ p__block_regex_definitions_optional_, SyntaxTreeToken p_token_2) : base ((uint) ProductionID.__block_of_regex_definitions__ID, (uint) production_kind.g__block_of_regex_definitions__1)
		{
			++g_counter;
			_init ();
			children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 4);
			children[0] = m__regex_ = p_token;
			children[1] = m__left_curly_bracket_ = p_token_1;
			children[2] = m__block_regex_definitions_optional_ = p__block_regex_definitions_optional_;
			children[3] = m__right_curly_bracket_ = p_token_2;
		}

		// Copy constructor

		public _block_of_regex_definitions_ (_block_of_regex_definitions_ p__block_of_regex_definitions_) : base (p__block_of_regex_definitions_.id, p__block_of_regex_definitions_.kind)
		{
			++g_counter;
			_init ();
			switch ((production_kind) this.kind)
			{
			case production_kind.g__block_of_regex_definitions__1:
				children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 4);
				children[0] = m__regex_ = p__block_of_regex_definitions_.m__regex_;
				children[1] = m__left_curly_bracket_ = p__block_of_regex_definitions_.m__left_curly_bracket_;
				children[2] = m__block_regex_definitions_optional_ = p__block_of_regex_definitions_.m__block_regex_definitions_optional_;
				children[3] = m__right_curly_bracket_ = p__block_of_regex_definitions_.m__right_curly_bracket_;
				break;
			default:
				string[] args = new string[] { "_block_of_regex_definitions_" };
				throw new SyntaxTreeError (SyntaxTreeError.InvalidKindError, args);
			}
		}

		// Clone node - wrapper of copy constructor

		public override SyntaxTreeBase Clone () { return (SyntaxTreeBase) new _block_of_regex_definitions_ (this); }

		// Destructor

		public new void Dispose ()
		{
			base.Dispose ();
			--g_counter;

			m__regex_?.Dispose ();
			m__left_curly_bracket_?.Dispose ();
			m__block_regex_definitions_optional_?.Dispose ();
			m__right_curly_bracket_?.Dispose ();

			_init ();
		}
		#endregion
		#region Content changing and inclusion checking methods associated with the syntax rule <block of regex definitions>

		// Content changing function(s) associated with production(s) of syntax rule <block of regex definitions>

		//
		// Content changing function associated with following production(s)
		// <block of regex definitions> -> '%regex' '{' <block regex definitions optional> '}'

		//

		public void change(SyntaxTreeToken p_token, SyntaxTreeToken p_token_1, _block_regex_definitions_optional_ p__block_regex_definitions_optional_, SyntaxTreeToken p_token_2)
		{
			_init ();
			this.kind = (uint) production_kind.g__block_of_regex_definitions__1;
			children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 4);
			children [0] = m__regex_ = p_token;
			children [1] = m__left_curly_bracket_ = p_token_1;
			children [2] = m__block_regex_definitions_optional_ = p__block_regex_definitions_optional_;
			children [3] = m__right_curly_bracket_ = p_token_2;
			reparent (parent);
		}

		//
		// check if node 'element' is included within syntax tree associated with syntax rule <block of regex definitions>
		//

		public new bool checkInclusion (SyntaxTreeBase element)
		{
			return
				(element == this) ||
				(m__regex_ != null) && m__regex_.checkInclusion (element) ||
				(m__left_curly_bracket_ != null) && m__left_curly_bracket_.checkInclusion (element) ||
				(m__block_regex_definitions_optional_ != null) && m__block_regex_definitions_optional_.checkInclusion (element) ||
				(m__right_curly_bracket_ != null) && m__right_curly_bracket_.checkInclusion (element) ||
				false;
		}
		#endregion
		#region Syntax tree emit methods associated with the syntax rule <block of regex definitions>

		//
		// emit production tree node associated with any production of syntax rule <block of regex definitions>
		//

		public override string Emit (int depth)
		{
			string s = "";
			if (depth != 0)
			switch ((production_kind) kind)
			{
				case production_kind.g__block_of_regex_definitions__1:
					// emit syntax tree node associated with production
					// <block of regex definitions>: '%regex' '{' <block regex definitions optional> '}'

					s += m__regex_.Emit (depth - 1);
					s += " ";
					s += m__left_curly_bracket_.Emit (depth - 1);
					s += " ";
					s += m__block_regex_definitions_optional_.Emit (depth - 1);
					s += " ";
					s += m__right_curly_bracket_.Emit (depth - 1);
				break;
			}
			return s.Trim ();
		}

		//
		// emit production tree node associated with any production of syntax rule <block of regex definitions>
		//

		public override string EmitProductionTree (int depth)
		{
			string s = "_block_of_regex_definitions_ (";
			if (depth != 0)
				switch ((production_kind) kind)
				{
				case production_kind.g__block_of_regex_definitions__1:
					// emit syntax tree node associated with production
					// <block of regex definitions>: '%regex' '{' <block regex definitions optional> '}'

					s += "_regex_";
					s += ' ';
					s += "_left_curly_bracket_";
					s += ' ';
					s += m__block_regex_definitions_optional_.EmitProductionTree (depth - 1);
					s += ' ';
					s += "_right_curly_bracket_";
				break;
			}
			s += ")";
			return s;
		}
		#endregion
		#region Syntax tree traversal,  reperrent, init and iterator methods associated with the syntax rule <block of regex definitions>

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
			m__left_curly_bracket_?.reparent (this);
			m__block_regex_definitions_optional_?.reparent (this);
			m__right_curly_bracket_?.reparent (this);
		}

		//
		// initialize object associated with syntax rule <block of regex definitions>
		//

		public void _init ()
		{
			m__regex_ = null;
			m__left_curly_bracket_ = null;
			m__block_regex_definitions_optional_ = null;
			m__right_curly_bracket_ = null;
		}
		#endregion
		#region Fields and properties associated with the syntax rule <block of regex definitions>

		// counter of all nodes associated with syntax rule <block of regex definitions>
		public static int g_counter;

		// objects associated with terminal and non-terminal symbols within production(s) of syntax rule <block of regex definitions>
		public SyntaxTreeToken m__regex_ { get; private set; }
		public SyntaxTreeToken m__left_curly_bracket_ { get; private set; }
		public _block_regex_definitions_optional_ m__block_regex_definitions_optional_ { get; private set; }
		public SyntaxTreeToken m__right_curly_bracket_ { get; private set; }
		#endregion

	};

	public partial class SyntaxTreeWalker
	{
		#region delegates and events associated with the syntax rule <block of regex definitions>

		// delegate function (callback) prototype associated with syntax rule <block of regex definitions>
		public delegate bool _block_of_regex_definitions__Callback (SyntaxTreeCallbackReason reason, _block_of_regex_definitions_.production_kind kind, _block_of_regex_definitions_ p__block_of_regex_definitions_);

		// event associated with syntax rule <block of regex definitions>
		public event _block_of_regex_definitions__Callback _block_of_regex_definitions__Event;

		// event trigger associated with syntax rule <block of regex definitions>
		public bool Raise__block_of_regex_definitions__Event (SyntaxTreeCallbackReason reason, _block_of_regex_definitions_.production_kind kind, _block_of_regex_definitions_ p__block_of_regex_definitions_)
		{
			bool? status = _block_of_regex_definitions__Event?.Invoke (reason, kind, p__block_of_regex_definitions_);
			return (status == null) || status.Value;
		}
		#endregion
		#region syntax tree traversal methods associated with the syntax rule <block of regex definitions>
		//
		// traverse syntax tree node associated with any production of syntax rule <block of regex definitions>
		//

		public void Traverse (_block_of_regex_definitions_ p__block_of_regex_definitions_)
		{
			if (p__block_of_regex_definitions_.isLocked())
				return;
			p__block_of_regex_definitions_.dolock();
			_block_of_regex_definitions_.production_kind kind = (_block_of_regex_definitions_.production_kind) p__block_of_regex_definitions_.kind;
			p__block_of_regex_definitions_.turn_reset ();
			if (Raise__block_of_regex_definitions__Event (SyntaxTreeCallbackReason.TraversalPrologueCallbackReason, kind, p__block_of_regex_definitions_))
			switch (kind)
			{
				case _block_of_regex_definitions_.production_kind.g__block_of_regex_definitions__1:
					// traverse syntax tree node associated with production
					// <block of regex definitions>: '%regex' '{' <block regex definitions optional> '}'

					if (Raise__block_of_regex_definitions__Event (SyntaxTreeCallbackReason.TraversalMidTermCallbackReason, kind, p__block_of_regex_definitions_))
						Traverse (p__block_of_regex_definitions_.m__block_regex_definitions_optional_);
					p__block_of_regex_definitions_.turn_inc ();
					Raise__block_of_regex_definitions__Event (SyntaxTreeCallbackReason.TraversalMidTermCallbackReason, kind, p__block_of_regex_definitions_);
				break;
			}
			Raise__block_of_regex_definitions__Event (SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason, kind, p__block_of_regex_definitions_);
			p__block_of_regex_definitions_.unlock();
		}

		//
		// traverse syntax tree node associated with any production of syntax rule <block of regex definitions>
		//

		public void TraverseCommon (_block_of_regex_definitions_ p__block_of_regex_definitions_)
		{
			_block_of_regex_definitions_.production_kind kind = (_block_of_regex_definitions_.production_kind) p__block_of_regex_definitions_.kind;
			if (Raise_Common_Event (SyntaxTreeCallbackReason.TraversalPrologueCallbackReason, (int) kind, p__block_of_regex_definitions_))
			switch (kind)
			{
				case _block_of_regex_definitions_.production_kind.g__block_of_regex_definitions__1:
					// traverse syntax tree node associated with production
					// <block of regex definitions>: '%regex' '{' <block regex definitions optional> '}'

						TraverseCommon (p__block_of_regex_definitions_.m__regex_);
						TraverseCommon (p__block_of_regex_definitions_.m__left_curly_bracket_);
						TraverseCommon (p__block_of_regex_definitions_.m__block_regex_definitions_optional_);
						TraverseCommon (p__block_of_regex_definitions_.m__right_curly_bracket_);
				break;
			}
			Raise_Common_Event (SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason, (int) kind, p__block_of_regex_definitions_);
		}
	};
		#endregion

	public partial class SyntaxTreeBuilder : SyntaxTreeWalker
	{
		#region syntax tree builder methods associated with the syntax rule <block of regex definitions>

		//
		// create syntax tree node associated with production
		// <block of regex definitions>: '%regex' '{' <block regex definitions optional> '}'

		//

		public _block_of_regex_definitions_ _block_of_regex_definitions__1 (SyntaxTreeToken p_token, SyntaxTreeToken p_token_1, _block_regex_definitions_optional_ p__block_regex_definitions_optional_, SyntaxTreeToken p_token_2)
		{
			_block_of_regex_definitions_ p__block_of_regex_definitions__ref = new _block_of_regex_definitions_(p_token, p_token_1, p__block_regex_definitions_optional_, p_token_2);
			p_token.parent = p__block_of_regex_definitions__ref;
			p_token_1.parent = p__block_of_regex_definitions__ref;
			p__block_regex_definitions_optional_.parent = p__block_of_regex_definitions__ref;
			p_token_2.parent = p__block_of_regex_definitions__ref;
			Raise__block_of_regex_definitions__Event (SyntaxTreeCallbackReason.BuilderCallbackReason, _block_of_regex_definitions_.production_kind.g__block_of_regex_definitions__1, p__block_of_regex_definitions__ref);
			return p__block_of_regex_definitions__ref;
		}
		#endregion
	};
}
