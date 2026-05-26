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
	// class associated with syntax rule <anglr syntax production>
	//

	public class	_anglr_syntax_production_ : SyntaxTreeBase
	{
		#region enumerated production(s) of syntax rule <anglr syntax production>
		public enum production_kind : ushort	// enumerated production(s) of syntax rule <anglr syntax production>
		{
			g__anglr_syntax_production__1 = 1,	// <production name optional> <name list> <priority assoc specification optional>

			g__anglr_syntax_production__2 = 2,	// '%empty'

		};
		#endregion
		#region production markers associated with the syntax rule <anglr syntax production>

		// markers associated with production: <anglr syntax production> -> <production name optional> <name list> <priority assoc specification optional>

		public enum production_marker_1 : ushort
		{
			m__production_name_optional_,
			m__name_list_,
			m__priority_assoc_specification_optional_,
			final
		};

		// markers associated with production: <anglr syntax production> -> '%empty'

		public enum production_marker_2 : ushort
		{
			final
		};
		#endregion
		#region Constructors and destructors associated with the syntax rule <anglr syntax production>

		// Constructor declaration(s) associated with production(s) of syntax rule <anglr syntax production>

		//
		// Constructor associated with the following production(s)
		// <anglr syntax production> -> <production name optional> <name list> <priority assoc specification optional>

		//

		public _anglr_syntax_production_ (_production_name_optional_ p__production_name_optional_, _name_list_ p__name_list_, _priority_assoc_specification_optional_ p__priority_assoc_specification_optional_) : base ((uint) ProductionID.__anglr_syntax_production__ID, (uint) production_kind.g__anglr_syntax_production__1)
		{
			++g_counter;
			_init ();
			children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 3);
			children[0] = m__production_name_optional_ = p__production_name_optional_;
			children[1] = m__name_list_ = p__name_list_;
			children[2] = m__priority_assoc_specification_optional_ = p__priority_assoc_specification_optional_;
		}

		//
		// Constructor associated with the following production(s)
		// <anglr syntax production> -> '%empty'

		//

		public _anglr_syntax_production_ (SyntaxTreeToken p_token) : base ((uint) ProductionID.__anglr_syntax_production__ID, (uint) production_kind.g__anglr_syntax_production__2)
		{
			++g_counter;
			_init ();
			children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 1);
			children[0] = m__empty_ = p_token;
		}

		// Copy constructor

		public _anglr_syntax_production_ (_anglr_syntax_production_ p__anglr_syntax_production_) : base (p__anglr_syntax_production_.id, p__anglr_syntax_production_.kind)
		{
			++g_counter;
			_init ();
			switch ((production_kind) p__anglr_syntax_production_.kind)
			{
				case production_kind.g__anglr_syntax_production__1:
					children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 3);
					if ((children [0] = m__production_name_optional_ = (p__anglr_syntax_production_.m__production_name_optional_ != null) ? new _production_name_optional_ (p__anglr_syntax_production_.m__production_name_optional_) : null) != null) m__production_name_optional_.parent = this;
					if ((children [1] = m__name_list_ = (p__anglr_syntax_production_.m__name_list_ != null) ? new _name_list_ (p__anglr_syntax_production_.m__name_list_) : null) != null) m__name_list_.parent = this;
					if ((children [2] = m__priority_assoc_specification_optional_ = (p__anglr_syntax_production_.m__priority_assoc_specification_optional_ != null) ? new _priority_assoc_specification_optional_ (p__anglr_syntax_production_.m__priority_assoc_specification_optional_) : null) != null) m__priority_assoc_specification_optional_.parent = this;
					break;
				case production_kind.g__anglr_syntax_production__2:
					children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 1);
					if ((children [0] = m__empty_ = (p__anglr_syntax_production_.m__empty_ != null) ? new SyntaxTreeToken (p__anglr_syntax_production_.m__empty_) : null) != null) m__empty_.parent = this;
					break;
				default:
				{
					string[] args = new string[] { "_anglr_syntax_production_" };
					throw new SyntaxTreeError (SyntaxTreeError.InvalidKindError, args);
				}
			}
		}

		// Clone node - wrapper of copy constructor

		public override SyntaxTreeBase Clone () { return (SyntaxTreeBase) new _anglr_syntax_production_ (this); }

		// Destructor

		public new void Dispose ()
		{
			base.Dispose ();
			--g_counter;

			m__production_name_optional_?.Dispose ();
			m__name_list_?.Dispose ();
			m__priority_assoc_specification_optional_?.Dispose ();
			m__empty_?.Dispose ();

			_init ();
		}
		#endregion
		#region Content changing and inclusion checking methods associated with the syntax rule <anglr syntax production>

		// Content changing function(s) associated with production(s) of syntax rule <anglr syntax production>

		//
		// Content changing function associated with following production(s)
		// <anglr syntax production> -> <production name optional> <name list> <priority assoc specification optional>

		//

		public void change(_production_name_optional_ p__production_name_optional_, _name_list_ p__name_list_, _priority_assoc_specification_optional_ p__priority_assoc_specification_optional_)
		{
			_init ();
			this.kind = (uint) production_kind.g__anglr_syntax_production__1;
			children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 3);
			children [0] = m__production_name_optional_ = p__production_name_optional_;
			children [1] = m__name_list_ = p__name_list_;
			children [2] = m__priority_assoc_specification_optional_ = p__priority_assoc_specification_optional_;
			reparent (parent);
		}

		//
		// Content changing function associated with following production(s)
		// <anglr syntax production> -> '%empty'

		//

		public void change(SyntaxTreeToken p_token)
		{
			_init ();
			this.kind = (uint) production_kind.g__anglr_syntax_production__2;
			children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 1);
			children [0] = m__empty_ = p_token;
			reparent (parent);
		}

		//
		// check if node 'element' is included within syntax tree associated with syntax rule <anglr syntax production>
		//

		public new bool checkInclusion (SyntaxTreeBase element)
		{
			return
				(element == this) ||
				(m__production_name_optional_ != null) && m__production_name_optional_.checkInclusion (element) ||
				(m__name_list_ != null) && m__name_list_.checkInclusion (element) ||
				(m__priority_assoc_specification_optional_ != null) && m__priority_assoc_specification_optional_.checkInclusion (element) ||
				(m__empty_ != null) && m__empty_.checkInclusion (element) ||
				false;
		}
		#endregion
		#region Syntax tree emit methods associated with the syntax rule <anglr syntax production>

		//
		// emit production tree node associated with any production of syntax rule <anglr syntax production>
		//

		public override string Emit (int depth)
		{
			string s = "";
			if (depth != 0)
			switch ((production_kind) kind)
			{
				case production_kind.g__anglr_syntax_production__1:
					// emit syntax tree node associated with production
					// <anglr syntax production>: <production name optional> <name list> <priority assoc specification optional>

					s += m__production_name_optional_.Emit (depth - 1);
					s += " ";
					s += m__name_list_.Emit (depth - 1);
					s += " ";
					s += m__priority_assoc_specification_optional_.Emit (depth - 1);
				break;
				case production_kind.g__anglr_syntax_production__2:
					// emit syntax tree node associated with production
					// <anglr syntax production>: '%empty'

					s += m__empty_.Emit (depth - 1);
				break;
			}
			return s.Trim ();
		}

		//
		// emit production tree node associated with any production of syntax rule <anglr syntax production>
		//

		public override string EmitProductionTree (int depth)
		{
			string s = "_anglr_syntax_production_ (";
			if (depth != 0)
				switch ((production_kind) kind)
				{
				case production_kind.g__anglr_syntax_production__1:
					// emit syntax tree node associated with production
					// <anglr syntax production>: <production name optional> <name list> <priority assoc specification optional>

					s += m__production_name_optional_.EmitProductionTree (depth - 1);
					s += ' ';
					s += m__name_list_.EmitProductionTree (depth - 1);
					s += ' ';
					s += m__priority_assoc_specification_optional_.EmitProductionTree (depth - 1);
				break;
				case production_kind.g__anglr_syntax_production__2:
					// emit syntax tree node associated with production
					// <anglr syntax production>: '%empty'

					s += "_empty_";
				break;
			}
			s += ")";
			return s;
		}
		#endregion
		#region Syntax tree traversal,  reperrent, init and iterator methods associated with the syntax rule <anglr syntax production>

		//
		// traverse sub-tree rooted in this node using selected syntax tree walker
		//

		public override void InvokeTraverse (SyntaxTreeWalkerCore syntaxTreeWalker) => ((SyntaxTreeWalker) syntaxTreeWalker).Traverse (this);
		public override void InvokeTraverseCommon (SyntaxTreeWalkerCore syntaxTreeWalker) => ((SyntaxTreeWalker) syntaxTreeWalker).TraverseCommon (this);

		// reparent sub-tree

		public override void reparent (SyntaxTreeBase parent)
		{
			this.parent = parent;

			m__production_name_optional_?.reparent (this);
			m__name_list_?.reparent (this);
			m__priority_assoc_specification_optional_?.reparent (this);
			m__empty_?.reparent (this);
		}

		//
		// initialize object associated with syntax rule <anglr syntax production>
		//

		public void _init ()
		{
			m__production_name_optional_ = null;
			m__name_list_ = null;
			m__priority_assoc_specification_optional_ = null;
			m__empty_ = null;
		}
		#endregion
		#region Fields and properties associated with the syntax rule <anglr syntax production>

		// counter of all nodes associated with syntax rule <anglr syntax production>
		public static int g_counter;

		// objects associated with terminal and non-terminal symbols within production(s) of syntax rule <anglr syntax production>
		public _production_name_optional_ m__production_name_optional_ { get; private set; }
		public _name_list_ m__name_list_ { get; private set; }
		public _priority_assoc_specification_optional_ m__priority_assoc_specification_optional_ { get; private set; }
		public SyntaxTreeToken m__empty_ { get; private set; }
		#endregion

	};

	public partial class SyntaxTreeWalker
	{
		#region delegates and events associated with the syntax rule <anglr syntax production>

		// delegate function (callback) prototype associated with syntax rule <anglr syntax production>
		public delegate bool _anglr_syntax_production__Callback (SyntaxTreeCallbackReason reason, _anglr_syntax_production_.production_kind kind, _anglr_syntax_production_ p__anglr_syntax_production_);

		// event associated with syntax rule <anglr syntax production>
		public event _anglr_syntax_production__Callback _anglr_syntax_production__Event;

		// event trigger associated with syntax rule <anglr syntax production>
		public bool Raise__anglr_syntax_production__Event (SyntaxTreeCallbackReason reason, _anglr_syntax_production_.production_kind kind, _anglr_syntax_production_ p__anglr_syntax_production_)
		{
			bool? status = _anglr_syntax_production__Event?.Invoke (reason, kind, p__anglr_syntax_production_);
			return (status == null) || status.Value;
		}
		#endregion
		#region syntax tree traversal methods associated with the syntax rule <anglr syntax production>
		//
		// traverse syntax tree node associated with any production of syntax rule <anglr syntax production>
		//

		public void Traverse (_anglr_syntax_production_ p__anglr_syntax_production_)
		{
			if (p__anglr_syntax_production_.isLocked())
				return;
			p__anglr_syntax_production_.dolock();
			_anglr_syntax_production_.production_kind kind = (_anglr_syntax_production_.production_kind) p__anglr_syntax_production_.kind;
			p__anglr_syntax_production_.turn_reset ();
			if (Raise__anglr_syntax_production__Event (SyntaxTreeCallbackReason.TraversalPrologueCallbackReason, kind, p__anglr_syntax_production_))
			switch (kind)
			{
				case _anglr_syntax_production_.production_kind.g__anglr_syntax_production__1:
					// traverse syntax tree node associated with production
					// <anglr syntax production>: <production name optional> <name list> <priority assoc specification optional>

					if (Raise__anglr_syntax_production__Event (SyntaxTreeCallbackReason.TraversalMidTermCallbackReason, kind, p__anglr_syntax_production_))
						Traverse (p__anglr_syntax_production_.m__production_name_optional_);
					p__anglr_syntax_production_.turn_inc ();
					if (Raise__anglr_syntax_production__Event (SyntaxTreeCallbackReason.TraversalMidTermCallbackReason, kind, p__anglr_syntax_production_))
						Traverse (p__anglr_syntax_production_.m__name_list_);
					p__anglr_syntax_production_.turn_inc ();
					if (Raise__anglr_syntax_production__Event (SyntaxTreeCallbackReason.TraversalMidTermCallbackReason, kind, p__anglr_syntax_production_))
						Traverse (p__anglr_syntax_production_.m__priority_assoc_specification_optional_);
					p__anglr_syntax_production_.turn_inc ();
					Raise__anglr_syntax_production__Event (SyntaxTreeCallbackReason.TraversalMidTermCallbackReason, kind, p__anglr_syntax_production_);
				break;
				case _anglr_syntax_production_.production_kind.g__anglr_syntax_production__2:
					// traverse syntax tree node associated with production
					// <anglr syntax production>: '%empty'

				break;
			}
			Raise__anglr_syntax_production__Event (SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason, kind, p__anglr_syntax_production_);
			p__anglr_syntax_production_.unlock();
		}

		//
		// traverse syntax tree node associated with any production of syntax rule <anglr syntax production>
		//

		public void TraverseCommon (_anglr_syntax_production_ p__anglr_syntax_production_)
		{
			_anglr_syntax_production_.production_kind kind = (_anglr_syntax_production_.production_kind) p__anglr_syntax_production_.kind;
			if (Raise_Common_Event (SyntaxTreeCallbackReason.TraversalPrologueCallbackReason, (int) kind, p__anglr_syntax_production_))
			switch (kind)
			{
				case _anglr_syntax_production_.production_kind.g__anglr_syntax_production__1:
					// traverse syntax tree node associated with production
					// <anglr syntax production>: <production name optional> <name list> <priority assoc specification optional>

						TraverseCommon (p__anglr_syntax_production_.m__production_name_optional_);
						TraverseCommon (p__anglr_syntax_production_.m__name_list_);
						TraverseCommon (p__anglr_syntax_production_.m__priority_assoc_specification_optional_);
				break;
				case _anglr_syntax_production_.production_kind.g__anglr_syntax_production__2:
					// traverse syntax tree node associated with production
					// <anglr syntax production>: '%empty'

						TraverseCommon (p__anglr_syntax_production_.m__empty_);
				break;
			}
			Raise_Common_Event (SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason, (int) kind, p__anglr_syntax_production_);
		}
	};
		#endregion

	public partial class SyntaxTreeBuilder : SyntaxTreeWalker
	{
		#region syntax tree builder methods associated with the syntax rule <anglr syntax production>

		//
		// create syntax tree node associated with production
		// <anglr syntax production>: <production name optional> <name list> <priority assoc specification optional>

		//

		public _anglr_syntax_production_ _anglr_syntax_production__1 (_production_name_optional_ p__production_name_optional_, _name_list_ p__name_list_, _priority_assoc_specification_optional_ p__priority_assoc_specification_optional_)
		{
			_anglr_syntax_production_ p__anglr_syntax_production__ref = new _anglr_syntax_production_(p__production_name_optional_, p__name_list_, p__priority_assoc_specification_optional_);
			p__production_name_optional_.parent = p__anglr_syntax_production__ref;
			p__name_list_.parent = p__anglr_syntax_production__ref;
			p__priority_assoc_specification_optional_.parent = p__anglr_syntax_production__ref;
			Raise__anglr_syntax_production__Event (SyntaxTreeCallbackReason.BuilderCallbackReason, _anglr_syntax_production_.production_kind.g__anglr_syntax_production__1, p__anglr_syntax_production__ref);
			return p__anglr_syntax_production__ref;
		}

		//
		// create syntax tree node associated with production
		// <anglr syntax production>: '%empty'

		//

		public _anglr_syntax_production_ _anglr_syntax_production__2 (SyntaxTreeToken p_token)
		{
			_anglr_syntax_production_ p__anglr_syntax_production__ref = new _anglr_syntax_production_(p_token);
			p_token.parent = p__anglr_syntax_production__ref;
			Raise__anglr_syntax_production__Event (SyntaxTreeCallbackReason.BuilderCallbackReason, _anglr_syntax_production_.production_kind.g__anglr_syntax_production__2, p__anglr_syntax_production__ref);
			return p__anglr_syntax_production__ref;
		}
		#endregion
	};
}
