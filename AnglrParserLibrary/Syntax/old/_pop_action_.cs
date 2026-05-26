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
	// class associated with syntax rule <pop action>
	//

	public class	_pop_action_ : SyntaxTreeBase
	{
		#region enumerated production(s) of syntax rule <pop action>
		public enum production_kind : ushort	// enumerated production(s) of syntax rule <pop action>
		{
			g__pop_action__1 = 1,	// 'pop'

		};
		#endregion
		#region production markers associated with the syntax rule <pop action>

		// markers associated with production: <pop action> -> 'pop'

		public enum production_marker_1 : ushort
		{
			final
		};
		#endregion
		#region Constructors and destructors associated with the syntax rule <pop action>

		// Constructor declaration(s) associated with production(s) of syntax rule <pop action>

		//
		// Constructor associated with the following production(s)
		// <pop action> -> 'pop'

		//

		public _pop_action_ (SyntaxTreeToken p_token) : base ((uint) ProductionID.__pop_action__ID, (uint) production_kind.g__pop_action__1)
		{
			++g_counter;
			_init ();
			children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 1);
			children[0] = m__pop_ = p_token;
		}

		// Copy constructor

		public _pop_action_ (_pop_action_ p__pop_action_) : base (p__pop_action_.id, p__pop_action_.kind)
		{
			++g_counter;
			_init ();
			switch ((production_kind) this.kind)
			{
			case production_kind.g__pop_action__1:
				children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 1);
				children[0] = m__pop_ = p__pop_action_.m__pop_;
				break;
			default:
				string[] args = new string[] { "_pop_action_" };
				throw new SyntaxTreeError (SyntaxTreeError.InvalidKindError, args);
			}
		}

		// Clone node - wrapper of copy constructor

		public override SyntaxTreeBase Clone () { return (SyntaxTreeBase) new _pop_action_ (this); }

		// Destructor

		public new void Dispose ()
		{
			base.Dispose ();
			--g_counter;

			m__pop_?.Dispose ();

			_init ();
		}
		#endregion
		#region Content changing and inclusion checking methods associated with the syntax rule <pop action>

		// Content changing function(s) associated with production(s) of syntax rule <pop action>

		//
		// Content changing function associated with following production(s)
		// <pop action> -> 'pop'

		//

		public void change(SyntaxTreeToken p_token)
		{
			_init ();
			this.kind = (uint) production_kind.g__pop_action__1;
			children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 1);
			children [0] = m__pop_ = p_token;
			reparent (parent);
		}

		//
		// check if node 'element' is included within syntax tree associated with syntax rule <pop action>
		//

		public new bool checkInclusion (SyntaxTreeBase element)
		{
			return
				(element == this) ||
				(m__pop_ != null) && m__pop_.checkInclusion (element) ||
				false;
		}
		#endregion
		#region Syntax tree emit methods associated with the syntax rule <pop action>

		//
		// emit production tree node associated with any production of syntax rule <pop action>
		//

		public override string Emit (int depth)
		{
			string s = "";
			if (depth != 0)
			switch ((production_kind) kind)
			{
				case production_kind.g__pop_action__1:
					// emit syntax tree node associated with production
					// <pop action>: 'pop'

					s += m__pop_.Emit (depth - 1);
				break;
			}
			return s.Trim ();
		}

		//
		// emit production tree node associated with any production of syntax rule <pop action>
		//

		public override string EmitProductionTree (int depth)
		{
			string s = "_pop_action_ (";
			if (depth != 0)
				switch ((production_kind) kind)
				{
				case production_kind.g__pop_action__1:
					// emit syntax tree node associated with production
					// <pop action>: 'pop'

					s += "_pop_";
				break;
			}
			s += ")";
			return s;
		}
		#endregion
		#region Syntax tree traversal,  reperrent, init and iterator methods associated with the syntax rule <pop action>

		//
		// traverse sub-tree rooted in this node using selected syntax tree walker
		//

		public override void InvokeTraverse (SyntaxTreeWalkerCore syntaxTreeWalker) => ((SyntaxTreeWalker) syntaxTreeWalker).Traverse (this);
		public override void InvokeTraverseCommon (SyntaxTreeWalkerCore syntaxTreeWalker) => ((SyntaxTreeWalker) syntaxTreeWalker).TraverseCommon (this);

		// reparent sub-tree

		public override void reparent (SyntaxTreeBase parent)
		{
			this.parent = parent;

			m__pop_?.reparent (this);
		}

		//
		// initialize object associated with syntax rule <pop action>
		//

		public void _init ()
		{
			m__pop_ = null;
		}
		#endregion
		#region Fields and properties associated with the syntax rule <pop action>

		// counter of all nodes associated with syntax rule <pop action>
		public static int g_counter;

		// objects associated with terminal and non-terminal symbols within production(s) of syntax rule <pop action>
		public SyntaxTreeToken m__pop_ { get; private set; }
		#endregion

	};

	public partial class SyntaxTreeWalker
	{
		#region delegates and events associated with the syntax rule <pop action>

		// delegate function (callback) prototype associated with syntax rule <pop action>
		public delegate bool _pop_action__Callback (SyntaxTreeCallbackReason reason, _pop_action_.production_kind kind, _pop_action_ p__pop_action_);

		// event associated with syntax rule <pop action>
		public event _pop_action__Callback _pop_action__Event;

		// event trigger associated with syntax rule <pop action>
		public bool Raise__pop_action__Event (SyntaxTreeCallbackReason reason, _pop_action_.production_kind kind, _pop_action_ p__pop_action_)
		{
			bool? status = _pop_action__Event?.Invoke (reason, kind, p__pop_action_);
			return (status == null) || status.Value;
		}
		#endregion
		#region syntax tree traversal methods associated with the syntax rule <pop action>
		//
		// traverse syntax tree node associated with any production of syntax rule <pop action>
		//

		public void Traverse (_pop_action_ p__pop_action_)
		{
			if (p__pop_action_.isLocked())
				return;
			p__pop_action_.dolock();
			_pop_action_.production_kind kind = (_pop_action_.production_kind) p__pop_action_.kind;
			p__pop_action_.turn_reset ();
			if (Raise__pop_action__Event (SyntaxTreeCallbackReason.TraversalPrologueCallbackReason, kind, p__pop_action_))
			switch (kind)
			{
				case _pop_action_.production_kind.g__pop_action__1:
					// traverse syntax tree node associated with production
					// <pop action>: 'pop'

				break;
			}
			Raise__pop_action__Event (SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason, kind, p__pop_action_);
			p__pop_action_.unlock();
		}

		//
		// traverse syntax tree node associated with any production of syntax rule <pop action>
		//

		public void TraverseCommon (_pop_action_ p__pop_action_)
		{
			_pop_action_.production_kind kind = (_pop_action_.production_kind) p__pop_action_.kind;
			if (Raise_Common_Event (SyntaxTreeCallbackReason.TraversalPrologueCallbackReason, (int) kind, p__pop_action_))
			switch (kind)
			{
				case _pop_action_.production_kind.g__pop_action__1:
					// traverse syntax tree node associated with production
					// <pop action>: 'pop'

						TraverseCommon (p__pop_action_.m__pop_);
				break;
			}
			Raise_Common_Event (SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason, (int) kind, p__pop_action_);
		}
	};
		#endregion

	public partial class SyntaxTreeBuilder : SyntaxTreeWalker
	{
		#region syntax tree builder methods associated with the syntax rule <pop action>

		//
		// create syntax tree node associated with production
		// <pop action>: 'pop'

		//

		public _pop_action_ _pop_action__1 (SyntaxTreeToken p_token)
		{
			_pop_action_ p__pop_action__ref = new _pop_action_(p_token);
			p_token.parent = p__pop_action__ref;
			Raise__pop_action__Event (SyntaxTreeCallbackReason.BuilderCallbackReason, _pop_action_.production_kind.g__pop_action__1, p__pop_action__ref);
			return p__pop_action__ref;
		}
		#endregion
	};
}
