//
//	This file was generated with ANGLR compiler
//
using System;
using System.Runtime.Serialization;
using System.Collections.Generic;

using Anglr.Parser.Core;
using Anglr.Parser.SyntaxTree;
using Anglr.Parser.Walker;
namespace Anglr.Parser
{
	//
	// class associated with syntax rule <classifier list>
	//

	[DataContract]
	public class	_classifier_list_ : SyntaxTreeBase
	{
		#region enumerated production(s) of syntax rule <classifier list>
		public enum production_kind : ushort	// enumerated production(s) of syntax rule <classifier list>
		{
			g__classifier_list__1 = 1,	// '(' <classifier info set> ')'

		};
		#endregion
		#region production markers associated with the syntax rule <classifier list>

		// markers associated with production: <classifier list> -> '(' <classifier info set> ')'

		public enum production_marker_1 : ushort
		{
			m__classifier_info_set_,
			final
		};
		#endregion
		#region Constructors and destructors associated with the syntax rule <classifier list>

		// Constructor declaration(s) associated with production(s) of syntax rule <classifier list>

		//
		// Constructor associated with the following production(s)
		// <classifier list> -> '(' <classifier info set> ')'

		//

		public _classifier_list_ (SyntaxTreeToken p_token, _classifier_info_set_ p__classifier_info_set_, SyntaxTreeToken p_token_1) : base ((uint) ProductionID.__classifier_list__ID, (uint) production_kind.g__classifier_list__1)
		{
			++g_counter;
			_init ();
			children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 3);
			children[0] = m__left_bracket_ = p_token;
			children[1] = m__classifier_info_set_ = p__classifier_info_set_;
			children[2] = m__right_bracket_ = p_token_1;
		}

		public _classifier_list_ () { }

		// Copy constructor

		public _classifier_list_ (_classifier_list_ p__classifier_list_) : base (p__classifier_list_.id, p__classifier_list_.kind)
		{
			++g_counter;
			_init ();
			appInfo = p__classifier_list_.appInfo;
			switch ((production_kind) p__classifier_list_.kind)
			{
				case production_kind.g__classifier_list__1:
					children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 3);
					if ((children [0] = m__left_bracket_ = (p__classifier_list_.m__left_bracket_ != null) ? new SyntaxTreeToken (p__classifier_list_.m__left_bracket_) : null) != null) m__left_bracket_.parent = this;
					if ((children [1] = m__classifier_info_set_ = (p__classifier_list_.m__classifier_info_set_ != null) ? new _classifier_info_set_ (p__classifier_list_.m__classifier_info_set_) : null) != null) m__classifier_info_set_.parent = this;
					if ((children [2] = m__right_bracket_ = (p__classifier_list_.m__right_bracket_ != null) ? new SyntaxTreeToken (p__classifier_list_.m__right_bracket_) : null) != null) m__right_bracket_.parent = this;
					break;
				default:
				{
					string[] args = new string[] { "_classifier_list_" };
					throw new SyntaxTreeError (SyntaxTreeError.InvalidKindError, args);
				}
			}
		}

		// Clone node - wrapper of copy constructor

		public override SyntaxTreeBase Clone () { return (SyntaxTreeBase) new _classifier_list_ (this); }

		// Destructor

		public new void Dispose ()
		{
			base.Dispose ();
			--g_counter;

			m__left_bracket_?.Dispose ();
			m__classifier_info_set_?.Dispose ();
			m__right_bracket_?.Dispose ();

			_init ();
		}
		#endregion
		#region Content changing and inclusion checking methods associated with the syntax rule <classifier list>

		// Content changing function(s) associated with production(s) of syntax rule <classifier list>

		//
		// Content changing function associated with following production(s)
		// <classifier list> -> '(' <classifier info set> ')'

		//

		public void change(SyntaxTreeToken p_token, _classifier_info_set_ p__classifier_info_set_, SyntaxTreeToken p_token_1)
		{
			_init ();
			this.kind = (uint) production_kind.g__classifier_list__1;
			children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 3);
			children [0] = m__left_bracket_ = p_token;
			children [1] = m__classifier_info_set_ = p__classifier_info_set_;
			children [2] = m__right_bracket_ = p_token_1;
			reparent (parent);
		}

		//
		// check if node 'element' is included within syntax tree associated with syntax rule <classifier list>
		//

		public new bool checkInclusion (SyntaxTreeBase element)
		{
			return
				(element == this) ||
				(m__left_bracket_ != null) && m__left_bracket_.checkInclusion (element) ||
				(m__classifier_info_set_ != null) && m__classifier_info_set_.checkInclusion (element) ||
				(m__right_bracket_ != null) && m__right_bracket_.checkInclusion (element) ||
				false;
		}
		#endregion
		#region Syntax tree emit methods associated with the syntax rule <classifier list>

		//
		// emit production tree node associated with any production of syntax rule <classifier list>
		//

		public override string Emit (int depth)
		{
			string s = "";
			if (depth != 0)
			switch ((production_kind) kind)
			{
				case production_kind.g__classifier_list__1:
					// emit syntax tree node associated with production
					// <classifier list>: '(' <classifier info set> ')'

					s += m__left_bracket_.Emit (depth - 1);
					s += " ";
					s += m__classifier_info_set_.Emit (depth - 1);
					s += " ";
					s += m__right_bracket_.Emit (depth - 1);
				break;
			}
			return s.Trim ();
		}

		//
		// emit production tree node associated with any production of syntax rule <classifier list>
		//

		public override string EmitProductionTree (int depth)
		{
			string s = "_classifier_list_ (";
			if (depth != 0)
				switch ((production_kind) kind)
				{
				case production_kind.g__classifier_list__1:
					// emit syntax tree node associated with production
					// <classifier list>: '(' <classifier info set> ')'

					s += "_left_bracket_";
					s += ' ';
					s += m__classifier_info_set_.EmitProductionTree (depth - 1);
					s += ' ';
					s += "_right_bracket_";
				break;
			}
			s += ")";
			return s;
		}
		#endregion
		#region Syntax tree traversal,  reperrent, init and iterator methods associated with the syntax rule <classifier list>

		//
		// traverse sub-tree rooted in this node using selected syntax tree walker
		//

		public override void InvokeTraverse (SyntaxTreeWalkerCore syntaxTreeWalker) => ((SyntaxTreeWalker) syntaxTreeWalker).Traverse (this);
		public override void InvokeTraverseCommon (SyntaxTreeWalkerCore syntaxTreeWalker) => ((SyntaxTreeWalker) syntaxTreeWalker).TraverseCommon (this);

		//
		// traverse syntax tree node associated with any production of syntax rule <classifier list> with specified delegate
		//

		public override void Visit (Func<SyntaxTreeCallbackReason, int, SyntaxTreeBase, object, bool> visitor, object appData)
		{
			_classifier_list_.production_kind kind = (_classifier_list_.production_kind) this.kind;
			if (visitor (SyntaxTreeCallbackReason.TraversalPrologueCallbackReason, (int) kind, this, appData))
			switch (kind)
			{
				case _classifier_list_.production_kind.g__classifier_list__1:
					// traverse syntax tree node associated with production
					// <classifier list>: '(' <classifier info set> ')'

						m__left_bracket_.Visit (visitor, appData);
						m__classifier_info_set_.Visit (visitor, appData);
						m__right_bracket_.Visit (visitor, appData);
				break;
			}
			visitor (SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason, (int) kind, this, appData);
		}

		// reparent sub-tree

		public override void reparent (SyntaxTreeBase parent)
		{
			this.parent = parent;

			m__left_bracket_?.reparent (this);
			m__classifier_info_set_?.reparent (this);
			m__right_bracket_?.reparent (this);
		}

		//
		// initialize object associated with syntax rule <classifier list>
		//

		public void _init ()
		{
			m__left_bracket_ = null;
			m__classifier_info_set_ = null;
			m__right_bracket_ = null;
		}
		#endregion
		#region Fields and properties associated with the syntax rule <classifier list>

		// counter of all nodes associated with syntax rule <classifier list>
		public static int g_counter;

		// objects associated with terminal and non-terminal symbols within production(s) of syntax rule <classifier list>
		[DataMember (Name = "m__left_bracket_")] public SyntaxTreeToken m__left_bracket_ { get; private set; }
		[DataMember (Name = "m__classifier_info_set_")] public _classifier_info_set_ m__classifier_info_set_ { get; private set; }
		[DataMember (Name = "m__right_bracket_")] public SyntaxTreeToken m__right_bracket_ { get; private set; }
		#endregion

	};

	public partial class SyntaxTreeWalker
	{
		#region delegates and events associated with the syntax rule <classifier list>

		// delegate function (callback) prototype associated with syntax rule <classifier list>
		public delegate bool _classifier_list__Callback (SyntaxTreeCallbackReason reason, _classifier_list_.production_kind kind, _classifier_list_ p__classifier_list_);

		// event associated with syntax rule <classifier list>
		public event _classifier_list__Callback _classifier_list__Event;

		// event trigger associated with syntax rule <classifier list>
		public bool Raise__classifier_list__Event (SyntaxTreeCallbackReason reason, _classifier_list_.production_kind kind, _classifier_list_ p__classifier_list_)
		{
			bool? status = _classifier_list__Event?.Invoke (reason, kind, p__classifier_list_);
			return (status == null) || status.Value;
		}
		#endregion
		#region syntax tree traversal methods associated with the syntax rule <classifier list>
		//
		// traverse syntax tree node associated with any production of syntax rule <classifier list>
		//

		public void Traverse (_classifier_list_ p__classifier_list_)
		{
			if (p__classifier_list_.isLocked())
				return;
			p__classifier_list_.dolock();
			_classifier_list_.production_kind kind = (_classifier_list_.production_kind) p__classifier_list_.kind;
			p__classifier_list_.turn_reset ();
			if (Raise__classifier_list__Event (SyntaxTreeCallbackReason.TraversalPrologueCallbackReason, kind, p__classifier_list_))
			switch (kind)
			{
				case _classifier_list_.production_kind.g__classifier_list__1:
					// traverse syntax tree node associated with production
					// <classifier list>: '(' <classifier info set> ')'

					if (Raise__classifier_list__Event (SyntaxTreeCallbackReason.TraversalMidTermCallbackReason, kind, p__classifier_list_))
						Traverse (p__classifier_list_.m__classifier_info_set_);
					p__classifier_list_.turn_inc ();
					Raise__classifier_list__Event (SyntaxTreeCallbackReason.TraversalMidTermCallbackReason, kind, p__classifier_list_);
				break;
			}
			Raise__classifier_list__Event (SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason, kind, p__classifier_list_);
			p__classifier_list_.unlock();
		}

		//
		// traverse syntax tree node associated with any production of syntax rule <classifier list>
		//

		public void TraverseCommon (_classifier_list_ p__classifier_list_)
		{
			_classifier_list_.production_kind kind = (_classifier_list_.production_kind) p__classifier_list_.kind;
			if (Raise_Common_Event (SyntaxTreeCallbackReason.TraversalPrologueCallbackReason, (int) kind, p__classifier_list_))
			switch (kind)
			{
				case _classifier_list_.production_kind.g__classifier_list__1:
					// traverse syntax tree node associated with production
					// <classifier list>: '(' <classifier info set> ')'

						TraverseCommon (p__classifier_list_.m__left_bracket_);
						TraverseCommon (p__classifier_list_.m__classifier_info_set_);
						TraverseCommon (p__classifier_list_.m__right_bracket_);
				break;
			}
			Raise_Common_Event (SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason, (int) kind, p__classifier_list_);
		}
	};
		#endregion

	public partial class SyntaxTreeBuilder : SyntaxTreeWalker
	{
		#region syntax tree builder methods associated with the syntax rule <classifier list>

		//
		// create syntax tree node associated with production
		// <classifier list>: '(' <classifier info set> ')'

		//

		public _classifier_list_ _classifier_list__1 (SyntaxTreeToken p_token, _classifier_info_set_ p__classifier_info_set_, SyntaxTreeToken p_token_1)
		{
			_classifier_list_ p__classifier_list__ref = new _classifier_list_(p_token, p__classifier_info_set_, p_token_1);
			p_token.parent = p__classifier_list__ref;
			p__classifier_info_set_.parent = p__classifier_list__ref;
			p_token_1.parent = p__classifier_list__ref;
			Raise__classifier_list__Event (SyntaxTreeCallbackReason.BuilderCallbackReason, _classifier_list_.production_kind.g__classifier_list__1, p__classifier_list__ref);
			return p__classifier_list__ref;
		}
		#endregion
	};
}
