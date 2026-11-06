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
	// class associated with syntax rule <classifier reference>
	//

	[DataContract]
	public class	_classifier_reference_ : SyntaxTreeBase
	{
		#region enumerated production(s) of syntax rule <classifier reference>
		public enum production_kind : ushort	// enumerated production(s) of syntax rule <classifier reference>
		{
			g__classifier_reference__1 = 1,	// '%use' <identifier>

		};
		#endregion
		#region production markers associated with the syntax rule <classifier reference>

		// markers associated with production: <classifier reference> -> '%use' <identifier>

		public enum production_marker_1 : ushort
		{
			final
		};
		#endregion
		#region Constructors and destructors associated with the syntax rule <classifier reference>

		// Constructor declaration(s) associated with production(s) of syntax rule <classifier reference>

		//
		// Constructor associated with the following production(s)
		// <classifier reference> -> '%use' <identifier>

		//

		public _classifier_reference_ (SyntaxTreeToken p_token, SyntaxTreeToken p_token_1) : base ((uint) ProductionID.__classifier_reference__ID, (uint) production_kind.g__classifier_reference__1)
		{
			++g_counter;
			_init ();
			children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 2);
			children[0] = m__use_ = p_token;
			children[1] = m__identifier_ = p_token_1;
		}

		public _classifier_reference_ () { }

		// Copy constructor

		public _classifier_reference_ (_classifier_reference_ p__classifier_reference_) : base (p__classifier_reference_.id, p__classifier_reference_.kind)
		{
			++g_counter;
			_init ();
			appInfo = p__classifier_reference_.appInfo;
			switch ((production_kind) p__classifier_reference_.kind)
			{
				case production_kind.g__classifier_reference__1:
					children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 2);
					if ((children [0] = m__use_ = (p__classifier_reference_.m__use_ != null) ? new SyntaxTreeToken (p__classifier_reference_.m__use_) : null) != null) m__use_.parent = this;
					if ((children [1] = m__identifier_ = (p__classifier_reference_.m__identifier_ != null) ? new SyntaxTreeToken (p__classifier_reference_.m__identifier_) : null) != null) m__identifier_.parent = this;
					break;
				default:
				{
					string[] args = new string[] { "_classifier_reference_" };
					throw new SyntaxTreeError (SyntaxTreeError.InvalidKindError, args);
				}
			}
		}

		// Clone node - wrapper of copy constructor

		public override SyntaxTreeBase Clone () { return (SyntaxTreeBase) new _classifier_reference_ (this); }

		// Destructor

		public new void Dispose ()
		{
			base.Dispose ();
			--g_counter;

			m__use_?.Dispose ();
			m__identifier_?.Dispose ();

			_init ();
		}
		#endregion
		#region Content changing and inclusion checking methods associated with the syntax rule <classifier reference>

		// Content changing function(s) associated with production(s) of syntax rule <classifier reference>

		//
		// Content changing function associated with following production(s)
		// <classifier reference> -> '%use' <identifier>

		//

		public void change(SyntaxTreeToken p_token, SyntaxTreeToken p_token_1)
		{
			_init ();
			this.kind = (uint) production_kind.g__classifier_reference__1;
			children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 2);
			children [0] = m__use_ = p_token;
			children [1] = m__identifier_ = p_token_1;
			reparent (parent);
		}

		//
		// check if node 'element' is included within syntax tree associated with syntax rule <classifier reference>
		//

		public new bool checkInclusion (SyntaxTreeBase element)
		{
			return
				(element == this) ||
				(m__use_ != null) && m__use_.checkInclusion (element) ||
				(m__identifier_ != null) && m__identifier_.checkInclusion (element) ||
				false;
		}
		#endregion
		#region Syntax tree emit methods associated with the syntax rule <classifier reference>

		//
		// emit production tree node associated with any production of syntax rule <classifier reference>
		//

		public override string Emit (int depth)
		{
			string s = "";
			if (depth != 0)
			switch ((production_kind) kind)
			{
				case production_kind.g__classifier_reference__1:
					// emit syntax tree node associated with production
					// <classifier reference>: '%use' <identifier>

					s += m__use_.Emit (depth - 1);
					s += " ";
					s += m__identifier_.Emit (depth - 1);
				break;
			}
			return s.Trim ();
		}

		//
		// emit production tree node associated with any production of syntax rule <classifier reference>
		//

		public override string EmitProductionTree (int depth)
		{
			string s = "_classifier_reference_ (";
			if (depth != 0)
				switch ((production_kind) kind)
				{
				case production_kind.g__classifier_reference__1:
					// emit syntax tree node associated with production
					// <classifier reference>: '%use' <identifier>

					s += "_use_";
					s += ' ';
					s += "_identifier_";
				break;
			}
			s += ")";
			return s;
		}
		#endregion
		#region Syntax tree traversal,  reperrent, init and iterator methods associated with the syntax rule <classifier reference>

		//
		// traverse sub-tree rooted in this node using selected syntax tree walker
		//

		public override void InvokeTraverse (SyntaxTreeWalkerCore syntaxTreeWalker) => ((SyntaxTreeWalker) syntaxTreeWalker).Traverse (this);
		public override void InvokeTraverseCommon (SyntaxTreeWalkerCore syntaxTreeWalker) => ((SyntaxTreeWalker) syntaxTreeWalker).TraverseCommon (this);

		//
		// traverse syntax tree node associated with any production of syntax rule <classifier reference> with specified delegate
		//

		public override void Visit (Func<SyntaxTreeCallbackReason, int, SyntaxTreeBase, object, bool> visitor, object appData)
		{
			_classifier_reference_.production_kind kind = (_classifier_reference_.production_kind) this.kind;
			if (visitor (SyntaxTreeCallbackReason.TraversalPrologueCallbackReason, (int) kind, this, appData))
			switch (kind)
			{
				case _classifier_reference_.production_kind.g__classifier_reference__1:
					// traverse syntax tree node associated with production
					// <classifier reference>: '%use' <identifier>

						m__use_.Visit (visitor, appData);
						m__identifier_.Visit (visitor, appData);
				break;
			}
			visitor (SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason, (int) kind, this, appData);
		}

		// reparent sub-tree

		public override void reparent (SyntaxTreeBase parent)
		{
			this.parent = parent;

			m__use_?.reparent (this);
			m__identifier_?.reparent (this);
		}

		//
		// initialize object associated with syntax rule <classifier reference>
		//

		public void _init ()
		{
			m__use_ = null;
			m__identifier_ = null;
		}
		#endregion
		#region Fields and properties associated with the syntax rule <classifier reference>

		// counter of all nodes associated with syntax rule <classifier reference>
		public static int g_counter;

		// objects associated with terminal and non-terminal symbols within production(s) of syntax rule <classifier reference>
		[DataMember (Name = "m__use_")] public SyntaxTreeToken m__use_ { get; private set; }
		[DataMember (Name = "m__identifier_")] public SyntaxTreeToken m__identifier_ { get; private set; }
		#endregion

	};

	public partial class SyntaxTreeWalker
	{
		#region delegates and events associated with the syntax rule <classifier reference>

		// delegate function (callback) prototype associated with syntax rule <classifier reference>
		public delegate bool _classifier_reference__Callback (SyntaxTreeCallbackReason reason, _classifier_reference_.production_kind kind, _classifier_reference_ p__classifier_reference_);

		// event associated with syntax rule <classifier reference>
		public event _classifier_reference__Callback _classifier_reference__Event;

		// event trigger associated with syntax rule <classifier reference>
		public bool Raise__classifier_reference__Event (SyntaxTreeCallbackReason reason, _classifier_reference_.production_kind kind, _classifier_reference_ p__classifier_reference_)
		{
			bool? status = _classifier_reference__Event?.Invoke (reason, kind, p__classifier_reference_);
			return (status == null) || status.Value;
		}
		#endregion
		#region syntax tree traversal methods associated with the syntax rule <classifier reference>
		//
		// traverse syntax tree node associated with any production of syntax rule <classifier reference>
		//

		public void Traverse (_classifier_reference_ p__classifier_reference_)
		{
			if (p__classifier_reference_.isLocked())
				return;
			p__classifier_reference_.dolock();
			_classifier_reference_.production_kind kind = (_classifier_reference_.production_kind) p__classifier_reference_.kind;
			p__classifier_reference_.turn_reset ();
			if (Raise__classifier_reference__Event (SyntaxTreeCallbackReason.TraversalPrologueCallbackReason, kind, p__classifier_reference_))
			switch (kind)
			{
				case _classifier_reference_.production_kind.g__classifier_reference__1:
					// traverse syntax tree node associated with production
					// <classifier reference>: '%use' <identifier>

				break;
			}
			Raise__classifier_reference__Event (SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason, kind, p__classifier_reference_);
			p__classifier_reference_.unlock();
		}

		//
		// traverse syntax tree node associated with any production of syntax rule <classifier reference>
		//

		public void TraverseCommon (_classifier_reference_ p__classifier_reference_)
		{
			_classifier_reference_.production_kind kind = (_classifier_reference_.production_kind) p__classifier_reference_.kind;
			if (Raise_Common_Event (SyntaxTreeCallbackReason.TraversalPrologueCallbackReason, (int) kind, p__classifier_reference_))
			switch (kind)
			{
				case _classifier_reference_.production_kind.g__classifier_reference__1:
					// traverse syntax tree node associated with production
					// <classifier reference>: '%use' <identifier>

						TraverseCommon (p__classifier_reference_.m__use_);
						TraverseCommon (p__classifier_reference_.m__identifier_);
				break;
			}
			Raise_Common_Event (SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason, (int) kind, p__classifier_reference_);
		}
	};
		#endregion

	public partial class SyntaxTreeBuilder : SyntaxTreeWalker
	{
		#region syntax tree builder methods associated with the syntax rule <classifier reference>

		//
		// create syntax tree node associated with production
		// <classifier reference>: '%use' <identifier>

		//

		public _classifier_reference_ _classifier_reference__1 (SyntaxTreeToken p_token, SyntaxTreeToken p_token_1)
		{
			_classifier_reference_ p__classifier_reference__ref = new _classifier_reference_(p_token, p_token_1);
			p_token.parent = p__classifier_reference__ref;
			p_token_1.parent = p__classifier_reference__ref;
			Raise__classifier_reference__Event (SyntaxTreeCallbackReason.BuilderCallbackReason, _classifier_reference_.production_kind.g__classifier_reference__1, p__classifier_reference__ref);
			return p__classifier_reference__ref;
		}
		#endregion
	};
}
