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
	// class associated with syntax rule <classifier named value>
	//

	[DataContract]
	public class	_classifier_named_value_ : SyntaxTreeBase
	{
		#region enumerated production(s) of syntax rule <classifier named value>
		public enum production_kind : ushort	// enumerated production(s) of syntax rule <classifier named value>
		{
			g__classifier_named_value__1 = 1,	// <identifier> '=' <cstring>

		};
		#endregion
		#region production markers associated with the syntax rule <classifier named value>

		// markers associated with production: <classifier named value> -> <identifier> '=' <cstring>

		public enum production_marker_1 : ushort
		{
			final
		};
		#endregion
		#region Constructors and destructors associated with the syntax rule <classifier named value>

		// Constructor declaration(s) associated with production(s) of syntax rule <classifier named value>

		//
		// Constructor associated with the following production(s)
		// <classifier named value> -> <identifier> '=' <cstring>

		//

		public _classifier_named_value_ (SyntaxTreeToken p_token, SyntaxTreeToken p_token_1, SyntaxTreeToken p_token_2) : base ((uint) ProductionID.__classifier_named_value__ID, (uint) production_kind.g__classifier_named_value__1)
		{
			++g_counter;
			_init ();
			children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 3);
			children[0] = m__identifier_ = p_token;
			children[1] = m__equals_sign_ = p_token_1;
			children[2] = m__cstring_ = p_token_2;
		}

		public _classifier_named_value_ () { }

		// Copy constructor

		public _classifier_named_value_ (_classifier_named_value_ p__classifier_named_value_) : base (p__classifier_named_value_.id, p__classifier_named_value_.kind)
		{
			++g_counter;
			_init ();
			appInfo = p__classifier_named_value_.appInfo;
			switch ((production_kind) p__classifier_named_value_.kind)
			{
				case production_kind.g__classifier_named_value__1:
					children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 3);
					if ((children [0] = m__identifier_ = (p__classifier_named_value_.m__identifier_ != null) ? new SyntaxTreeToken (p__classifier_named_value_.m__identifier_) : null) != null) m__identifier_.parent = this;
					if ((children [1] = m__equals_sign_ = (p__classifier_named_value_.m__equals_sign_ != null) ? new SyntaxTreeToken (p__classifier_named_value_.m__equals_sign_) : null) != null) m__equals_sign_.parent = this;
					if ((children [2] = m__cstring_ = (p__classifier_named_value_.m__cstring_ != null) ? new SyntaxTreeToken (p__classifier_named_value_.m__cstring_) : null) != null) m__cstring_.parent = this;
					break;
				default:
				{
					string[] args = new string[] { "_classifier_named_value_" };
					throw new SyntaxTreeError (SyntaxTreeError.InvalidKindError, args);
				}
			}
		}

		// Clone node - wrapper of copy constructor

		public override SyntaxTreeBase Clone () { return (SyntaxTreeBase) new _classifier_named_value_ (this); }

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
		#region Content changing and inclusion checking methods associated with the syntax rule <classifier named value>

		// Content changing function(s) associated with production(s) of syntax rule <classifier named value>

		//
		// Content changing function associated with following production(s)
		// <classifier named value> -> <identifier> '=' <cstring>

		//

		public void change(SyntaxTreeToken p_token, SyntaxTreeToken p_token_1, SyntaxTreeToken p_token_2)
		{
			_init ();
			this.kind = (uint) production_kind.g__classifier_named_value__1;
			children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 3);
			children [0] = m__identifier_ = p_token;
			children [1] = m__equals_sign_ = p_token_1;
			children [2] = m__cstring_ = p_token_2;
			reparent (parent);
		}

		//
		// check if node 'element' is included within syntax tree associated with syntax rule <classifier named value>
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
		#region Syntax tree emit methods associated with the syntax rule <classifier named value>

		//
		// emit production tree node associated with any production of syntax rule <classifier named value>
		//

		public override string Emit (int depth)
		{
			string s = "";
			if (depth != 0)
			switch ((production_kind) kind)
			{
				case production_kind.g__classifier_named_value__1:
					// emit syntax tree node associated with production
					// <classifier named value>: <identifier> '=' <cstring>

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
		// emit production tree node associated with any production of syntax rule <classifier named value>
		//

		public override string EmitProductionTree (int depth)
		{
			string s = "_classifier_named_value_ (";
			if (depth != 0)
				switch ((production_kind) kind)
				{
				case production_kind.g__classifier_named_value__1:
					// emit syntax tree node associated with production
					// <classifier named value>: <identifier> '=' <cstring>

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
		#region Syntax tree traversal,  reperrent, init and iterator methods associated with the syntax rule <classifier named value>

		//
		// traverse sub-tree rooted in this node using selected syntax tree walker
		//

		public override void InvokeTraverse (SyntaxTreeWalkerCore syntaxTreeWalker) => ((SyntaxTreeWalker) syntaxTreeWalker).Traverse (this);
		public override void InvokeTraverseCommon (SyntaxTreeWalkerCore syntaxTreeWalker) => ((SyntaxTreeWalker) syntaxTreeWalker).TraverseCommon (this);

		//
		// traverse syntax tree node associated with any production of syntax rule <classifier named value> with specified delegate
		//

		public override void Visit (Func<SyntaxTreeCallbackReason, int, SyntaxTreeBase, object, bool> visitor, object appData)
		{
			_classifier_named_value_.production_kind kind = (_classifier_named_value_.production_kind) this.kind;
			if (visitor (SyntaxTreeCallbackReason.TraversalPrologueCallbackReason, (int) kind, this, appData))
			switch (kind)
			{
				case _classifier_named_value_.production_kind.g__classifier_named_value__1:
					// traverse syntax tree node associated with production
					// <classifier named value>: <identifier> '=' <cstring>

						m__identifier_.Visit (visitor, appData);
						m__equals_sign_.Visit (visitor, appData);
						m__cstring_.Visit (visitor, appData);
				break;
			}
			visitor (SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason, (int) kind, this, appData);
		}

		// reparent sub-tree

		public override void reparent (SyntaxTreeBase parent)
		{
			this.parent = parent;

			m__identifier_?.reparent (this);
			m__equals_sign_?.reparent (this);
			m__cstring_?.reparent (this);
		}

		//
		// initialize object associated with syntax rule <classifier named value>
		//

		public void _init ()
		{
			m__identifier_ = null;
			m__equals_sign_ = null;
			m__cstring_ = null;
		}
		#endregion
		#region Fields and properties associated with the syntax rule <classifier named value>

		// counter of all nodes associated with syntax rule <classifier named value>
		public static int g_counter;

		// objects associated with terminal and non-terminal symbols within production(s) of syntax rule <classifier named value>
		[DataMember (Name = "m__identifier_")] public SyntaxTreeToken m__identifier_ { get; private set; }
		[DataMember (Name = "m__equals_sign_")] public SyntaxTreeToken m__equals_sign_ { get; private set; }
		[DataMember (Name = "m__cstring_")] public SyntaxTreeToken m__cstring_ { get; private set; }
		#endregion

	};

	public partial class SyntaxTreeWalker
	{
		#region delegates and events associated with the syntax rule <classifier named value>

		// delegate function (callback) prototype associated with syntax rule <classifier named value>
		public delegate bool _classifier_named_value__Callback (SyntaxTreeCallbackReason reason, _classifier_named_value_.production_kind kind, _classifier_named_value_ p__classifier_named_value_);

		// event associated with syntax rule <classifier named value>
		public event _classifier_named_value__Callback _classifier_named_value__Event;

		// event trigger associated with syntax rule <classifier named value>
		public bool Raise__classifier_named_value__Event (SyntaxTreeCallbackReason reason, _classifier_named_value_.production_kind kind, _classifier_named_value_ p__classifier_named_value_)
		{
			bool? status = _classifier_named_value__Event?.Invoke (reason, kind, p__classifier_named_value_);
			return (status == null) || status.Value;
		}
		#endregion
		#region syntax tree traversal methods associated with the syntax rule <classifier named value>
		//
		// traverse syntax tree node associated with any production of syntax rule <classifier named value>
		//

		public void Traverse (_classifier_named_value_ p__classifier_named_value_)
		{
			if (p__classifier_named_value_.isLocked())
				return;
			p__classifier_named_value_.dolock();
			_classifier_named_value_.production_kind kind = (_classifier_named_value_.production_kind) p__classifier_named_value_.kind;
			p__classifier_named_value_.turn_reset ();
			if (Raise__classifier_named_value__Event (SyntaxTreeCallbackReason.TraversalPrologueCallbackReason, kind, p__classifier_named_value_))
			switch (kind)
			{
				case _classifier_named_value_.production_kind.g__classifier_named_value__1:
					// traverse syntax tree node associated with production
					// <classifier named value>: <identifier> '=' <cstring>

				break;
			}
			Raise__classifier_named_value__Event (SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason, kind, p__classifier_named_value_);
			p__classifier_named_value_.unlock();
		}

		//
		// traverse syntax tree node associated with any production of syntax rule <classifier named value>
		//

		public void TraverseCommon (_classifier_named_value_ p__classifier_named_value_)
		{
			_classifier_named_value_.production_kind kind = (_classifier_named_value_.production_kind) p__classifier_named_value_.kind;
			if (Raise_Common_Event (SyntaxTreeCallbackReason.TraversalPrologueCallbackReason, (int) kind, p__classifier_named_value_))
			switch (kind)
			{
				case _classifier_named_value_.production_kind.g__classifier_named_value__1:
					// traverse syntax tree node associated with production
					// <classifier named value>: <identifier> '=' <cstring>

						TraverseCommon (p__classifier_named_value_.m__identifier_);
						TraverseCommon (p__classifier_named_value_.m__equals_sign_);
						TraverseCommon (p__classifier_named_value_.m__cstring_);
				break;
			}
			Raise_Common_Event (SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason, (int) kind, p__classifier_named_value_);
		}
	};
		#endregion

	public partial class SyntaxTreeBuilder : SyntaxTreeWalker
	{
		#region syntax tree builder methods associated with the syntax rule <classifier named value>

		//
		// create syntax tree node associated with production
		// <classifier named value>: <identifier> '=' <cstring>

		//

		public _classifier_named_value_ _classifier_named_value__1 (SyntaxTreeToken p_token, SyntaxTreeToken p_token_1, SyntaxTreeToken p_token_2)
		{
			_classifier_named_value_ p__classifier_named_value__ref = new _classifier_named_value_(p_token, p_token_1, p_token_2);
			p_token.parent = p__classifier_named_value__ref;
			p_token_1.parent = p__classifier_named_value__ref;
			p_token_2.parent = p__classifier_named_value__ref;
			Raise__classifier_named_value__Event (SyntaxTreeCallbackReason.BuilderCallbackReason, _classifier_named_value_.production_kind.g__classifier_named_value__1, p__classifier_named_value__ref);
			return p__classifier_named_value__ref;
		}
		#endregion
	};
}
