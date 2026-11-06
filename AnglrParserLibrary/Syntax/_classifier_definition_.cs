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
	// class associated with syntax rule <classifier definition>
	//

	[DataContract]
	public class	_classifier_definition_ : SyntaxTreeBase
	{
		#region enumerated production(s) of syntax rule <classifier definition>
		public enum production_kind : ushort	// enumerated production(s) of syntax rule <classifier definition>
		{
			g__classifier_definition__1 = 1,	// '%classifier' <identifier> '=' <classifier named values>

		};
		#endregion
		#region production markers associated with the syntax rule <classifier definition>

		// markers associated with production: <classifier definition> -> '%classifier' <identifier> '=' <classifier named values>

		public enum production_marker_1 : ushort
		{
			m__classifier_named_values_,
			final
		};
		#endregion
		#region Constructors and destructors associated with the syntax rule <classifier definition>

		// Constructor declaration(s) associated with production(s) of syntax rule <classifier definition>

		//
		// Constructor associated with the following production(s)
		// <classifier definition> -> '%classifier' <identifier> '=' <classifier named values>

		//

		public _classifier_definition_ (SyntaxTreeToken p_token, SyntaxTreeToken p_token_1, SyntaxTreeToken p_token_2, _classifier_named_values_ p__classifier_named_values_) : base ((uint) ProductionID.__classifier_definition__ID, (uint) production_kind.g__classifier_definition__1)
		{
			++g_counter;
			_init ();
			children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 4);
			children[0] = m__classifier_ = p_token;
			children[1] = m__identifier_ = p_token_1;
			children[2] = m__equals_sign_ = p_token_2;
			children[3] = m__classifier_named_values_ = p__classifier_named_values_;
		}

		public _classifier_definition_ () { }

		// Copy constructor

		public _classifier_definition_ (_classifier_definition_ p__classifier_definition_) : base (p__classifier_definition_.id, p__classifier_definition_.kind)
		{
			++g_counter;
			_init ();
			appInfo = p__classifier_definition_.appInfo;
			switch ((production_kind) p__classifier_definition_.kind)
			{
				case production_kind.g__classifier_definition__1:
					children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 4);
					if ((children [0] = m__classifier_ = (p__classifier_definition_.m__classifier_ != null) ? new SyntaxTreeToken (p__classifier_definition_.m__classifier_) : null) != null) m__classifier_.parent = this;
					if ((children [1] = m__identifier_ = (p__classifier_definition_.m__identifier_ != null) ? new SyntaxTreeToken (p__classifier_definition_.m__identifier_) : null) != null) m__identifier_.parent = this;
					if ((children [2] = m__equals_sign_ = (p__classifier_definition_.m__equals_sign_ != null) ? new SyntaxTreeToken (p__classifier_definition_.m__equals_sign_) : null) != null) m__equals_sign_.parent = this;
					if ((children [3] = m__classifier_named_values_ = (p__classifier_definition_.m__classifier_named_values_ != null) ? new _classifier_named_values_ (p__classifier_definition_.m__classifier_named_values_) : null) != null) m__classifier_named_values_.parent = this;
					break;
				default:
				{
					string[] args = new string[] { "_classifier_definition_" };
					throw new SyntaxTreeError (SyntaxTreeError.InvalidKindError, args);
				}
			}
		}

		// Clone node - wrapper of copy constructor

		public override SyntaxTreeBase Clone () { return (SyntaxTreeBase) new _classifier_definition_ (this); }

		// Destructor

		public new void Dispose ()
		{
			base.Dispose ();
			--g_counter;

			m__classifier_?.Dispose ();
			m__identifier_?.Dispose ();
			m__equals_sign_?.Dispose ();
			m__classifier_named_values_?.Dispose ();

			_init ();
		}
		#endregion
		#region Content changing and inclusion checking methods associated with the syntax rule <classifier definition>

		// Content changing function(s) associated with production(s) of syntax rule <classifier definition>

		//
		// Content changing function associated with following production(s)
		// <classifier definition> -> '%classifier' <identifier> '=' <classifier named values>

		//

		public void change(SyntaxTreeToken p_token, SyntaxTreeToken p_token_1, SyntaxTreeToken p_token_2, _classifier_named_values_ p__classifier_named_values_)
		{
			_init ();
			this.kind = (uint) production_kind.g__classifier_definition__1;
			children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 4);
			children [0] = m__classifier_ = p_token;
			children [1] = m__identifier_ = p_token_1;
			children [2] = m__equals_sign_ = p_token_2;
			children [3] = m__classifier_named_values_ = p__classifier_named_values_;
			reparent (parent);
		}

		//
		// check if node 'element' is included within syntax tree associated with syntax rule <classifier definition>
		//

		public new bool checkInclusion (SyntaxTreeBase element)
		{
			return
				(element == this) ||
				(m__classifier_ != null) && m__classifier_.checkInclusion (element) ||
				(m__identifier_ != null) && m__identifier_.checkInclusion (element) ||
				(m__equals_sign_ != null) && m__equals_sign_.checkInclusion (element) ||
				(m__classifier_named_values_ != null) && m__classifier_named_values_.checkInclusion (element) ||
				false;
		}
		#endregion
		#region Syntax tree emit methods associated with the syntax rule <classifier definition>

		//
		// emit production tree node associated with any production of syntax rule <classifier definition>
		//

		public override string Emit (int depth)
		{
			string s = "";
			if (depth != 0)
			switch ((production_kind) kind)
			{
				case production_kind.g__classifier_definition__1:
					// emit syntax tree node associated with production
					// <classifier definition>: '%classifier' <identifier> '=' <classifier named values>

					s += m__classifier_.Emit (depth - 1);
					s += " ";
					s += m__identifier_.Emit (depth - 1);
					s += " ";
					s += m__equals_sign_.Emit (depth - 1);
					s += " ";
					s += m__classifier_named_values_.Emit (depth - 1);
				break;
			}
			return s.Trim ();
		}

		//
		// emit production tree node associated with any production of syntax rule <classifier definition>
		//

		public override string EmitProductionTree (int depth)
		{
			string s = "_classifier_definition_ (";
			if (depth != 0)
				switch ((production_kind) kind)
				{
				case production_kind.g__classifier_definition__1:
					// emit syntax tree node associated with production
					// <classifier definition>: '%classifier' <identifier> '=' <classifier named values>

					s += "_classifier_";
					s += ' ';
					s += "_identifier_";
					s += ' ';
					s += "_equals_sign_";
					s += ' ';
					s += m__classifier_named_values_.EmitProductionTree (depth - 1);
				break;
			}
			s += ")";
			return s;
		}
		#endregion
		#region Syntax tree traversal,  reperrent, init and iterator methods associated with the syntax rule <classifier definition>

		//
		// traverse sub-tree rooted in this node using selected syntax tree walker
		//

		public override void InvokeTraverse (SyntaxTreeWalkerCore syntaxTreeWalker) => ((SyntaxTreeWalker) syntaxTreeWalker).Traverse (this);
		public override void InvokeTraverseCommon (SyntaxTreeWalkerCore syntaxTreeWalker) => ((SyntaxTreeWalker) syntaxTreeWalker).TraverseCommon (this);

		//
		// traverse syntax tree node associated with any production of syntax rule <classifier definition> with specified delegate
		//

		public override void Visit (Func<SyntaxTreeCallbackReason, int, SyntaxTreeBase, object, bool> visitor, object appData)
		{
			_classifier_definition_.production_kind kind = (_classifier_definition_.production_kind) this.kind;
			if (visitor (SyntaxTreeCallbackReason.TraversalPrologueCallbackReason, (int) kind, this, appData))
			switch (kind)
			{
				case _classifier_definition_.production_kind.g__classifier_definition__1:
					// traverse syntax tree node associated with production
					// <classifier definition>: '%classifier' <identifier> '=' <classifier named values>

						m__classifier_.Visit (visitor, appData);
						m__identifier_.Visit (visitor, appData);
						m__equals_sign_.Visit (visitor, appData);
						m__classifier_named_values_.Visit (visitor, appData);
				break;
			}
			visitor (SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason, (int) kind, this, appData);
		}

		// reparent sub-tree

		public override void reparent (SyntaxTreeBase parent)
		{
			this.parent = parent;

			m__classifier_?.reparent (this);
			m__identifier_?.reparent (this);
			m__equals_sign_?.reparent (this);
			m__classifier_named_values_?.reparent (this);
		}

		//
		// initialize object associated with syntax rule <classifier definition>
		//

		public void _init ()
		{
			m__classifier_ = null;
			m__identifier_ = null;
			m__equals_sign_ = null;
			m__classifier_named_values_ = null;
		}
		#endregion
		#region Fields and properties associated with the syntax rule <classifier definition>

		// counter of all nodes associated with syntax rule <classifier definition>
		public static int g_counter;

		// objects associated with terminal and non-terminal symbols within production(s) of syntax rule <classifier definition>
		[DataMember (Name = "m__classifier_")] public SyntaxTreeToken m__classifier_ { get; private set; }
		[DataMember (Name = "m__identifier_")] public SyntaxTreeToken m__identifier_ { get; private set; }
		[DataMember (Name = "m__equals_sign_")] public SyntaxTreeToken m__equals_sign_ { get; private set; }
		[DataMember (Name = "m__classifier_named_values_")] public _classifier_named_values_ m__classifier_named_values_ { get; private set; }
		#endregion

	};

	public partial class SyntaxTreeWalker
	{
		#region delegates and events associated with the syntax rule <classifier definition>

		// delegate function (callback) prototype associated with syntax rule <classifier definition>
		public delegate bool _classifier_definition__Callback (SyntaxTreeCallbackReason reason, _classifier_definition_.production_kind kind, _classifier_definition_ p__classifier_definition_);

		// event associated with syntax rule <classifier definition>
		public event _classifier_definition__Callback _classifier_definition__Event;

		// event trigger associated with syntax rule <classifier definition>
		public bool Raise__classifier_definition__Event (SyntaxTreeCallbackReason reason, _classifier_definition_.production_kind kind, _classifier_definition_ p__classifier_definition_)
		{
			bool? status = _classifier_definition__Event?.Invoke (reason, kind, p__classifier_definition_);
			return (status == null) || status.Value;
		}
		#endregion
		#region syntax tree traversal methods associated with the syntax rule <classifier definition>
		//
		// traverse syntax tree node associated with any production of syntax rule <classifier definition>
		//

		public void Traverse (_classifier_definition_ p__classifier_definition_)
		{
			if (p__classifier_definition_.isLocked())
				return;
			p__classifier_definition_.dolock();
			_classifier_definition_.production_kind kind = (_classifier_definition_.production_kind) p__classifier_definition_.kind;
			p__classifier_definition_.turn_reset ();
			if (Raise__classifier_definition__Event (SyntaxTreeCallbackReason.TraversalPrologueCallbackReason, kind, p__classifier_definition_))
			switch (kind)
			{
				case _classifier_definition_.production_kind.g__classifier_definition__1:
					// traverse syntax tree node associated with production
					// <classifier definition>: '%classifier' <identifier> '=' <classifier named values>

					if (Raise__classifier_definition__Event (SyntaxTreeCallbackReason.TraversalMidTermCallbackReason, kind, p__classifier_definition_))
						Traverse (p__classifier_definition_.m__classifier_named_values_);
					p__classifier_definition_.turn_inc ();
					Raise__classifier_definition__Event (SyntaxTreeCallbackReason.TraversalMidTermCallbackReason, kind, p__classifier_definition_);
				break;
			}
			Raise__classifier_definition__Event (SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason, kind, p__classifier_definition_);
			p__classifier_definition_.unlock();
		}

		//
		// traverse syntax tree node associated with any production of syntax rule <classifier definition>
		//

		public void TraverseCommon (_classifier_definition_ p__classifier_definition_)
		{
			_classifier_definition_.production_kind kind = (_classifier_definition_.production_kind) p__classifier_definition_.kind;
			if (Raise_Common_Event (SyntaxTreeCallbackReason.TraversalPrologueCallbackReason, (int) kind, p__classifier_definition_))
			switch (kind)
			{
				case _classifier_definition_.production_kind.g__classifier_definition__1:
					// traverse syntax tree node associated with production
					// <classifier definition>: '%classifier' <identifier> '=' <classifier named values>

						TraverseCommon (p__classifier_definition_.m__classifier_);
						TraverseCommon (p__classifier_definition_.m__identifier_);
						TraverseCommon (p__classifier_definition_.m__equals_sign_);
						TraverseCommon (p__classifier_definition_.m__classifier_named_values_);
				break;
			}
			Raise_Common_Event (SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason, (int) kind, p__classifier_definition_);
		}
	};
		#endregion

	public partial class SyntaxTreeBuilder : SyntaxTreeWalker
	{
		#region syntax tree builder methods associated with the syntax rule <classifier definition>

		//
		// create syntax tree node associated with production
		// <classifier definition>: '%classifier' <identifier> '=' <classifier named values>

		//

		public _classifier_definition_ _classifier_definition__1 (SyntaxTreeToken p_token, SyntaxTreeToken p_token_1, SyntaxTreeToken p_token_2, _classifier_named_values_ p__classifier_named_values_)
		{
			_classifier_definition_ p__classifier_definition__ref = new _classifier_definition_(p_token, p_token_1, p_token_2, p__classifier_named_values_);
			p_token.parent = p__classifier_definition__ref;
			p_token_1.parent = p__classifier_definition__ref;
			p_token_2.parent = p__classifier_definition__ref;
			p__classifier_named_values_.parent = p__classifier_definition__ref;
			Raise__classifier_definition__Event (SyntaxTreeCallbackReason.BuilderCallbackReason, _classifier_definition_.production_kind.g__classifier_definition__1, p__classifier_definition__ref);
			return p__classifier_definition__ref;
		}
		#endregion
	};
}
