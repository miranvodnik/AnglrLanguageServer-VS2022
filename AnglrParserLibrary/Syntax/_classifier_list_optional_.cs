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
	// class associated with syntax rule <classifier list optional>
	//

	[DataContract]
	public class	_classifier_list_optional_ : SyntaxTreeBase
	{
		#region enumerated production(s) of syntax rule <classifier list optional>
		public enum production_kind : ushort	// enumerated production(s) of syntax rule <classifier list optional>
		{
			g__classifier_list_optional__1 = 1,	// %empty

			g__classifier_list_optional__2 = 2,	// <classifier list>

		};
		#endregion
		#region production markers associated with the syntax rule <classifier list optional>

		// markers associated with production: <classifier list optional> -> %empty

		public enum production_marker_1 : ushort
		{
			final
		};

		// markers associated with production: <classifier list optional> -> <classifier list>

		public enum production_marker_2 : ushort
		{
			m__classifier_list_,
			final
		};
		#endregion
		#region Constructors and destructors associated with the syntax rule <classifier list optional>

		// Constructor declaration(s) associated with production(s) of syntax rule <classifier list optional>

		//
		// Constructor associated with the following production(s)
		// <classifier list optional> -> %empty

		//

		public _classifier_list_optional_ () : base ((uint) ProductionID.__classifier_list_optional__ID, (uint) production_kind.g__classifier_list_optional__1)
		{
			++g_counter;
			_init ();
			children = Array.Empty <SyntaxTreeBase> ();
		}

		//
		// Constructor associated with the following production(s)
		// <classifier list optional> -> <classifier list>

		//

		public _classifier_list_optional_ (_classifier_list_ p__classifier_list_) : base ((uint) ProductionID.__classifier_list_optional__ID, (uint) production_kind.g__classifier_list_optional__2)
		{
			++g_counter;
			_init ();
			children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 1);
			children[0] = m__classifier_list_ = p__classifier_list_;
		}

		// Copy constructor

		public _classifier_list_optional_ (_classifier_list_optional_ p__classifier_list_optional_) : base (p__classifier_list_optional_.id, p__classifier_list_optional_.kind)
		{
			++g_counter;
			_init ();
			appInfo = p__classifier_list_optional_.appInfo;
			switch ((production_kind) p__classifier_list_optional_.kind)
			{
				case production_kind.g__classifier_list_optional__1:
					children = Array.Empty <SyntaxTreeBase> ();
					break;
				case production_kind.g__classifier_list_optional__2:
					children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 1);
					if ((children [0] = m__classifier_list_ = (p__classifier_list_optional_.m__classifier_list_ != null) ? new _classifier_list_ (p__classifier_list_optional_.m__classifier_list_) : null) != null) m__classifier_list_.parent = this;
					break;
				default:
				{
					string[] args = new string[] { "_classifier_list_optional_" };
					throw new SyntaxTreeError (SyntaxTreeError.InvalidKindError, args);
				}
			}
		}

		// Clone node - wrapper of copy constructor

		public override SyntaxTreeBase Clone () { return (SyntaxTreeBase) new _classifier_list_optional_ (this); }

		// Destructor

		public new void Dispose ()
		{
			base.Dispose ();
			--g_counter;

			m__classifier_list_?.Dispose ();

			_init ();
		}
		#endregion
		#region Content changing and inclusion checking methods associated with the syntax rule <classifier list optional>

		// Content changing function(s) associated with production(s) of syntax rule <classifier list optional>

		//
		// Content changing function associated with following production(s)
		// <classifier list optional> -> %empty

		//

		public void change()
		{
			_init ();
			this.kind = (uint) production_kind.g__classifier_list_optional__1;
			children = Array.Empty <SyntaxTreeBase> ();
			reparent (parent);
		}

		//
		// Content changing function associated with following production(s)
		// <classifier list optional> -> <classifier list>

		//

		public void change(_classifier_list_ p__classifier_list_)
		{
			_init ();
			this.kind = (uint) production_kind.g__classifier_list_optional__2;
			children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 1);
			children [0] = m__classifier_list_ = p__classifier_list_;
			reparent (parent);
		}

		//
		// check if node 'element' is included within syntax tree associated with syntax rule <classifier list optional>
		//

		public new bool checkInclusion (SyntaxTreeBase element)
		{
			return
				(element == this) ||
				(m__classifier_list_ != null) && m__classifier_list_.checkInclusion (element) ||
				false;
		}
		#endregion
		#region Syntax tree emit methods associated with the syntax rule <classifier list optional>

		//
		// emit production tree node associated with any production of syntax rule <classifier list optional>
		//

		public override string Emit (int depth)
		{
			string s = "";
			if (depth != 0)
			switch ((production_kind) kind)
			{
				case production_kind.g__classifier_list_optional__1:
					// emit syntax tree node associated with production
					// <classifier list optional>: %empty

				break;
				case production_kind.g__classifier_list_optional__2:
					// emit syntax tree node associated with production
					// <classifier list optional>: <classifier list>

					s += m__classifier_list_.Emit (depth - 1);
				break;
			}
			return s.Trim ();
		}

		//
		// emit production tree node associated with any production of syntax rule <classifier list optional>
		//

		public override string EmitProductionTree (int depth)
		{
			string s = "_classifier_list_optional_ (";
			if (depth != 0)
				switch ((production_kind) kind)
				{
				case production_kind.g__classifier_list_optional__1:
					// emit syntax tree node associated with production
					// <classifier list optional>: %empty

				break;
				case production_kind.g__classifier_list_optional__2:
					// emit syntax tree node associated with production
					// <classifier list optional>: <classifier list>

					s += m__classifier_list_.EmitProductionTree (depth - 1);
				break;
			}
			s += ")";
			return s;
		}
		#endregion
		#region Syntax tree traversal,  reperrent, init and iterator methods associated with the syntax rule <classifier list optional>

		//
		// traverse sub-tree rooted in this node using selected syntax tree walker
		//

		public override void InvokeTraverse (SyntaxTreeWalkerCore syntaxTreeWalker) => ((SyntaxTreeWalker) syntaxTreeWalker).Traverse (this);
		public override void InvokeTraverseCommon (SyntaxTreeWalkerCore syntaxTreeWalker) => ((SyntaxTreeWalker) syntaxTreeWalker).TraverseCommon (this);

		//
		// traverse syntax tree node associated with any production of syntax rule <classifier list optional> with specified delegate
		//

		public override void Visit (Func<SyntaxTreeCallbackReason, int, SyntaxTreeBase, object, bool> visitor, object appData)
		{
			_classifier_list_optional_.production_kind kind = (_classifier_list_optional_.production_kind) this.kind;
			if (visitor (SyntaxTreeCallbackReason.TraversalPrologueCallbackReason, (int) kind, this, appData))
			switch (kind)
			{
				case _classifier_list_optional_.production_kind.g__classifier_list_optional__1:
					// traverse syntax tree node associated with production
					// <classifier list optional>: %empty

				break;
				case _classifier_list_optional_.production_kind.g__classifier_list_optional__2:
					// traverse syntax tree node associated with production
					// <classifier list optional>: <classifier list>

						m__classifier_list_.Visit (visitor, appData);
				break;
			}
			visitor (SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason, (int) kind, this, appData);
		}

		// reparent sub-tree

		public override void reparent (SyntaxTreeBase parent)
		{
			this.parent = parent;

			m__classifier_list_?.reparent (this);
		}

		//
		// initialize object associated with syntax rule <classifier list optional>
		//

		public void _init ()
		{
			m__classifier_list_ = null;
		}
		#endregion
		#region Fields and properties associated with the syntax rule <classifier list optional>

		// counter of all nodes associated with syntax rule <classifier list optional>
		public static int g_counter;

		// objects associated with terminal and non-terminal symbols within production(s) of syntax rule <classifier list optional>
		[DataMember (Name = "m__classifier_list_")] public _classifier_list_ m__classifier_list_ { get; private set; }
		#endregion

	};

	public partial class SyntaxTreeWalker
	{
		#region delegates and events associated with the syntax rule <classifier list optional>

		// delegate function (callback) prototype associated with syntax rule <classifier list optional>
		public delegate bool _classifier_list_optional__Callback (SyntaxTreeCallbackReason reason, _classifier_list_optional_.production_kind kind, _classifier_list_optional_ p__classifier_list_optional_);

		// event associated with syntax rule <classifier list optional>
		public event _classifier_list_optional__Callback _classifier_list_optional__Event;

		// event trigger associated with syntax rule <classifier list optional>
		public bool Raise__classifier_list_optional__Event (SyntaxTreeCallbackReason reason, _classifier_list_optional_.production_kind kind, _classifier_list_optional_ p__classifier_list_optional_)
		{
			bool? status = _classifier_list_optional__Event?.Invoke (reason, kind, p__classifier_list_optional_);
			return (status == null) || status.Value;
		}
		#endregion
		#region syntax tree traversal methods associated with the syntax rule <classifier list optional>
		//
		// traverse syntax tree node associated with any production of syntax rule <classifier list optional>
		//

		public void Traverse (_classifier_list_optional_ p__classifier_list_optional_)
		{
			if (p__classifier_list_optional_.isLocked())
				return;
			p__classifier_list_optional_.dolock();
			_classifier_list_optional_.production_kind kind = (_classifier_list_optional_.production_kind) p__classifier_list_optional_.kind;
			p__classifier_list_optional_.turn_reset ();
			if (Raise__classifier_list_optional__Event (SyntaxTreeCallbackReason.TraversalPrologueCallbackReason, kind, p__classifier_list_optional_))
			switch (kind)
			{
				case _classifier_list_optional_.production_kind.g__classifier_list_optional__1:
					// traverse syntax tree node associated with production
					// <classifier list optional>: %empty

				break;
				case _classifier_list_optional_.production_kind.g__classifier_list_optional__2:
					// traverse syntax tree node associated with production
					// <classifier list optional>: <classifier list>

					if (Raise__classifier_list_optional__Event (SyntaxTreeCallbackReason.TraversalMidTermCallbackReason, kind, p__classifier_list_optional_))
						Traverse (p__classifier_list_optional_.m__classifier_list_);
					p__classifier_list_optional_.turn_inc ();
					Raise__classifier_list_optional__Event (SyntaxTreeCallbackReason.TraversalMidTermCallbackReason, kind, p__classifier_list_optional_);
				break;
			}
			Raise__classifier_list_optional__Event (SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason, kind, p__classifier_list_optional_);
			p__classifier_list_optional_.unlock();
		}

		//
		// traverse syntax tree node associated with any production of syntax rule <classifier list optional>
		//

		public void TraverseCommon (_classifier_list_optional_ p__classifier_list_optional_)
		{
			_classifier_list_optional_.production_kind kind = (_classifier_list_optional_.production_kind) p__classifier_list_optional_.kind;
			if (Raise_Common_Event (SyntaxTreeCallbackReason.TraversalPrologueCallbackReason, (int) kind, p__classifier_list_optional_))
			switch (kind)
			{
				case _classifier_list_optional_.production_kind.g__classifier_list_optional__1:
					// traverse syntax tree node associated with production
					// <classifier list optional>: %empty

				break;
				case _classifier_list_optional_.production_kind.g__classifier_list_optional__2:
					// traverse syntax tree node associated with production
					// <classifier list optional>: <classifier list>

						TraverseCommon (p__classifier_list_optional_.m__classifier_list_);
				break;
			}
			Raise_Common_Event (SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason, (int) kind, p__classifier_list_optional_);
		}
	};
		#endregion

	public partial class SyntaxTreeBuilder : SyntaxTreeWalker
	{
		#region syntax tree builder methods associated with the syntax rule <classifier list optional>

		//
		// create syntax tree node associated with production
		// <classifier list optional>: %empty

		//

		public _classifier_list_optional_ _classifier_list_optional__1 ()
		{
			_classifier_list_optional_ p__classifier_list_optional__ref = new _classifier_list_optional_();
			Raise__classifier_list_optional__Event (SyntaxTreeCallbackReason.BuilderCallbackReason, _classifier_list_optional_.production_kind.g__classifier_list_optional__1, p__classifier_list_optional__ref);
			return p__classifier_list_optional__ref;
		}

		//
		// create syntax tree node associated with production
		// <classifier list optional>: <classifier list>

		//

		public _classifier_list_optional_ _classifier_list_optional__2 (_classifier_list_ p__classifier_list_)
		{
			_classifier_list_optional_ p__classifier_list_optional__ref = new _classifier_list_optional_(p__classifier_list_);
			p__classifier_list_.parent = p__classifier_list_optional__ref;
			Raise__classifier_list_optional__Event (SyntaxTreeCallbackReason.BuilderCallbackReason, _classifier_list_optional_.production_kind.g__classifier_list_optional__2, p__classifier_list_optional__ref);
			return p__classifier_list_optional__ref;
		}
		#endregion
	};
}
