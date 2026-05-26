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
	// class associated with syntax rule <anglr nested rule>
	//

	public class	_anglr_nested_rule_ : SyntaxTreeBase
	{
		#region enumerated production(s) of syntax rule <anglr nested rule>
		public enum production_kind : ushort	// enumerated production(s) of syntax rule <anglr nested rule>
		{
			g__anglr_nested_rule__1 = 1,	// <anglr syntax production list name optional> <anglr syntax production list>

		};
		#endregion
		#region production markers associated with the syntax rule <anglr nested rule>

		// markers associated with production: <anglr nested rule> -> <anglr syntax production list name optional> <anglr syntax production list>

		public enum production_marker_1 : ushort
		{
			m__anglr_syntax_production_list_name_optional_,
			m__anglr_syntax_production_list_,
			final
		};
		#endregion
		#region Constructors and destructors associated with the syntax rule <anglr nested rule>

		// Constructor declaration(s) associated with production(s) of syntax rule <anglr nested rule>

		//
		// Constructor associated with the following production(s)
		// <anglr nested rule> -> <anglr syntax production list name optional> <anglr syntax production list>

		//

		public _anglr_nested_rule_ (_anglr_syntax_production_list_name_optional_ p__anglr_syntax_production_list_name_optional_, _anglr_syntax_production_list_ p__anglr_syntax_production_list_) : base ((uint) ProductionID.__anglr_nested_rule__ID, (uint) production_kind.g__anglr_nested_rule__1)
		{
			++g_counter;
			_init ();
			children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 2);
			children[0] = m__anglr_syntax_production_list_name_optional_ = p__anglr_syntax_production_list_name_optional_;
			children[1] = m__anglr_syntax_production_list_ = p__anglr_syntax_production_list_;
		}

		// Copy constructor

		public _anglr_nested_rule_ (_anglr_nested_rule_ p__anglr_nested_rule_) : base (p__anglr_nested_rule_.id, p__anglr_nested_rule_.kind)
		{
			++g_counter;
			_init ();
			switch ((production_kind) this.kind)
			{
			case production_kind.g__anglr_nested_rule__1:
				children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 2);
				children[0] = m__anglr_syntax_production_list_name_optional_ = p__anglr_nested_rule_.m__anglr_syntax_production_list_name_optional_;
				children[1] = m__anglr_syntax_production_list_ = p__anglr_nested_rule_.m__anglr_syntax_production_list_;
				break;
			default:
				string[] args = new string[] { "_anglr_nested_rule_" };
				throw new SyntaxTreeError (SyntaxTreeError.InvalidKindError, args);
			}
		}

		// Clone node - wrapper of copy constructor

		public override SyntaxTreeBase Clone () { return (SyntaxTreeBase) new _anglr_nested_rule_ (this); }

		// Destructor

		public new void Dispose ()
		{
			base.Dispose ();
			--g_counter;

			m__anglr_syntax_production_list_name_optional_?.Dispose ();
			m__anglr_syntax_production_list_?.Dispose ();

			_init ();
		}
		#endregion
		#region Content changing and inclusion checking methods associated with the syntax rule <anglr nested rule>

		// Content changing function(s) associated with production(s) of syntax rule <anglr nested rule>

		//
		// Content changing function associated with following production(s)
		// <anglr nested rule> -> <anglr syntax production list name optional> <anglr syntax production list>

		//

		public void change(_anglr_syntax_production_list_name_optional_ p__anglr_syntax_production_list_name_optional_, _anglr_syntax_production_list_ p__anglr_syntax_production_list_)
		{
			_init ();
			this.kind = (uint) production_kind.g__anglr_nested_rule__1;
			children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 2);
			children [0] = m__anglr_syntax_production_list_name_optional_ = p__anglr_syntax_production_list_name_optional_;
			children [1] = m__anglr_syntax_production_list_ = p__anglr_syntax_production_list_;
			reparent (parent);
		}

		//
		// check if node 'element' is included within syntax tree associated with syntax rule <anglr nested rule>
		//

		public new bool checkInclusion (SyntaxTreeBase element)
		{
			return
				(element == this) ||
				(m__anglr_syntax_production_list_name_optional_ != null) && m__anglr_syntax_production_list_name_optional_.checkInclusion (element) ||
				(m__anglr_syntax_production_list_ != null) && m__anglr_syntax_production_list_.checkInclusion (element) ||
				false;
		}
		#endregion
		#region Syntax tree emit methods associated with the syntax rule <anglr nested rule>

		//
		// emit production tree node associated with any production of syntax rule <anglr nested rule>
		//

		public override string Emit (int depth)
		{
			string s = "";
			if (depth != 0)
			switch ((production_kind) kind)
			{
				case production_kind.g__anglr_nested_rule__1:
					// emit syntax tree node associated with production
					// <anglr nested rule>: <anglr syntax production list name optional> <anglr syntax production list>

					s += m__anglr_syntax_production_list_name_optional_.Emit (depth - 1);
					s += " ";
					s += m__anglr_syntax_production_list_.Emit (depth - 1);
				break;
			}
			return s.Trim ();
		}

		//
		// emit production tree node associated with any production of syntax rule <anglr nested rule>
		//

		public override string EmitProductionTree (int depth)
		{
			string s = "_anglr_nested_rule_ (";
			if (depth != 0)
				switch ((production_kind) kind)
				{
				case production_kind.g__anglr_nested_rule__1:
					// emit syntax tree node associated with production
					// <anglr nested rule>: <anglr syntax production list name optional> <anglr syntax production list>

					s += m__anglr_syntax_production_list_name_optional_.EmitProductionTree (depth - 1);
					s += ' ';
					s += m__anglr_syntax_production_list_.EmitProductionTree (depth - 1);
				break;
			}
			s += ")";
			return s;
		}
		#endregion
		#region Syntax tree traversal,  reperrent, init and iterator methods associated with the syntax rule <anglr nested rule>

		//
		// traverse sub-tree rooted in this node using selected syntax tree walker
		//

		public override void InvokeTraverse (SyntaxTreeWalkerCore syntaxTreeWalker) => ((SyntaxTreeWalker) syntaxTreeWalker).Traverse (this);
		public override void InvokeTraverseCommon (SyntaxTreeWalkerCore syntaxTreeWalker) => ((SyntaxTreeWalker) syntaxTreeWalker).TraverseCommon (this);

		// reparent sub-tree

		public override void reparent (SyntaxTreeBase parent)
		{
			this.parent = parent;

			m__anglr_syntax_production_list_name_optional_?.reparent (this);
			m__anglr_syntax_production_list_?.reparent (this);
		}

		//
		// initialize object associated with syntax rule <anglr nested rule>
		//

		public void _init ()
		{
			m__anglr_syntax_production_list_name_optional_ = null;
			m__anglr_syntax_production_list_ = null;
		}
		#endregion
		#region Fields and properties associated with the syntax rule <anglr nested rule>

		// counter of all nodes associated with syntax rule <anglr nested rule>
		public static int g_counter;

		// objects associated with terminal and non-terminal symbols within production(s) of syntax rule <anglr nested rule>
		public _anglr_syntax_production_list_name_optional_ m__anglr_syntax_production_list_name_optional_ { get; private set; }
		public _anglr_syntax_production_list_ m__anglr_syntax_production_list_ { get; private set; }
		#endregion

	};

	public partial class SyntaxTreeWalker
	{
		#region delegates and events associated with the syntax rule <anglr nested rule>

		// delegate function (callback) prototype associated with syntax rule <anglr nested rule>
		public delegate bool _anglr_nested_rule__Callback (SyntaxTreeCallbackReason reason, _anglr_nested_rule_.production_kind kind, _anglr_nested_rule_ p__anglr_nested_rule_);

		// event associated with syntax rule <anglr nested rule>
		public event _anglr_nested_rule__Callback _anglr_nested_rule__Event;

		// event trigger associated with syntax rule <anglr nested rule>
		public bool Raise__anglr_nested_rule__Event (SyntaxTreeCallbackReason reason, _anglr_nested_rule_.production_kind kind, _anglr_nested_rule_ p__anglr_nested_rule_)
		{
			bool? status = _anglr_nested_rule__Event?.Invoke (reason, kind, p__anglr_nested_rule_);
			return (status == null) || status.Value;
		}
		#endregion
		#region syntax tree traversal methods associated with the syntax rule <anglr nested rule>
		//
		// traverse syntax tree node associated with any production of syntax rule <anglr nested rule>
		//

		public void Traverse (_anglr_nested_rule_ p__anglr_nested_rule_)
		{
			if (p__anglr_nested_rule_.isLocked())
				return;
			p__anglr_nested_rule_.dolock();
			_anglr_nested_rule_.production_kind kind = (_anglr_nested_rule_.production_kind) p__anglr_nested_rule_.kind;
			p__anglr_nested_rule_.turn_reset ();
			if (Raise__anglr_nested_rule__Event (SyntaxTreeCallbackReason.TraversalPrologueCallbackReason, kind, p__anglr_nested_rule_))
			switch (kind)
			{
				case _anglr_nested_rule_.production_kind.g__anglr_nested_rule__1:
					// traverse syntax tree node associated with production
					// <anglr nested rule>: <anglr syntax production list name optional> <anglr syntax production list>

					if (Raise__anglr_nested_rule__Event (SyntaxTreeCallbackReason.TraversalMidTermCallbackReason, kind, p__anglr_nested_rule_))
						Traverse (p__anglr_nested_rule_.m__anglr_syntax_production_list_name_optional_);
					p__anglr_nested_rule_.turn_inc ();
					if (Raise__anglr_nested_rule__Event (SyntaxTreeCallbackReason.TraversalMidTermCallbackReason, kind, p__anglr_nested_rule_))
						Traverse (p__anglr_nested_rule_.m__anglr_syntax_production_list_);
					p__anglr_nested_rule_.turn_inc ();
					Raise__anglr_nested_rule__Event (SyntaxTreeCallbackReason.TraversalMidTermCallbackReason, kind, p__anglr_nested_rule_);
				break;
			}
			Raise__anglr_nested_rule__Event (SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason, kind, p__anglr_nested_rule_);
			p__anglr_nested_rule_.unlock();
		}

		//
		// traverse syntax tree node associated with any production of syntax rule <anglr nested rule>
		//

		public void TraverseCommon (_anglr_nested_rule_ p__anglr_nested_rule_)
		{
			_anglr_nested_rule_.production_kind kind = (_anglr_nested_rule_.production_kind) p__anglr_nested_rule_.kind;
			if (Raise_Common_Event (SyntaxTreeCallbackReason.TraversalPrologueCallbackReason, (int) kind, p__anglr_nested_rule_))
			switch (kind)
			{
				case _anglr_nested_rule_.production_kind.g__anglr_nested_rule__1:
					// traverse syntax tree node associated with production
					// <anglr nested rule>: <anglr syntax production list name optional> <anglr syntax production list>

						TraverseCommon (p__anglr_nested_rule_.m__anglr_syntax_production_list_name_optional_);
						TraverseCommon (p__anglr_nested_rule_.m__anglr_syntax_production_list_);
				break;
			}
			Raise_Common_Event (SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason, (int) kind, p__anglr_nested_rule_);
		}
	};
		#endregion

	public partial class SyntaxTreeBuilder : SyntaxTreeWalker
	{
		#region syntax tree builder methods associated with the syntax rule <anglr nested rule>

		//
		// create syntax tree node associated with production
		// <anglr nested rule>: <anglr syntax production list name optional> <anglr syntax production list>

		//

		public _anglr_nested_rule_ _anglr_nested_rule__1 (_anglr_syntax_production_list_name_optional_ p__anglr_syntax_production_list_name_optional_, _anglr_syntax_production_list_ p__anglr_syntax_production_list_)
		{
			_anglr_nested_rule_ p__anglr_nested_rule__ref = new _anglr_nested_rule_(p__anglr_syntax_production_list_name_optional_, p__anglr_syntax_production_list_);
			p__anglr_syntax_production_list_name_optional_.parent = p__anglr_nested_rule__ref;
			p__anglr_syntax_production_list_.parent = p__anglr_nested_rule__ref;
			Raise__anglr_nested_rule__Event (SyntaxTreeCallbackReason.BuilderCallbackReason, _anglr_nested_rule_.production_kind.g__anglr_nested_rule__1, p__anglr_nested_rule__ref);
			return p__anglr_nested_rule__ref;
		}
		#endregion
	};
}
