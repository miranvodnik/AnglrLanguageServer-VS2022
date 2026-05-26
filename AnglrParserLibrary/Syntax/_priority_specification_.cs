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
	// class associated with syntax rule <priority specification>
	//

	public class	_priority_specification_ : SyntaxTreeBase
	{
		#region enumerated production(s) of syntax rule <priority specification>
		public enum production_kind : ushort	// enumerated production(s) of syntax rule <priority specification>
		{
			g__priority_specification__1 = 1,	// '%priority' <number>

			g__priority_specification__2 = 2,	// '%priority' '=' <number>

		};
		#endregion
		#region production markers associated with the syntax rule <priority specification>

		// markers associated with production: <priority specification> -> '%priority' <number>

		public enum production_marker_1 : ushort
		{
			final
		};

		// markers associated with production: <priority specification> -> '%priority' '=' <number>

		public enum production_marker_2 : ushort
		{
			final
		};
		#endregion
		#region Constructors and destructors associated with the syntax rule <priority specification>

		// Constructor declaration(s) associated with production(s) of syntax rule <priority specification>

		//
		// Constructor associated with the following production(s)
		// <priority specification> -> '%priority' <number>

		//

		public _priority_specification_ (SyntaxTreeToken p_token, SyntaxTreeToken p_token_1) : base ((uint) ProductionID.__priority_specification__ID, (uint) production_kind.g__priority_specification__1)
		{
			++g_counter;
			_init ();
			children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 2);
			children[0] = m__priority_ = p_token;
			children[1] = m__number_ = p_token_1;
		}

		//
		// Constructor associated with the following production(s)
		// <priority specification> -> '%priority' '=' <number>

		//

		public _priority_specification_ (SyntaxTreeToken p_token, SyntaxTreeToken p_token_1, SyntaxTreeToken p_token_2) : base ((uint) ProductionID.__priority_specification__ID, (uint) production_kind.g__priority_specification__2)
		{
			++g_counter;
			_init ();
			children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 3);
			children[0] = m__priority_ = p_token;
			children[1] = m__equals_sign_ = p_token_1;
			children[2] = m__number_ = p_token_2;
		}

		// Copy constructor

		public _priority_specification_ (_priority_specification_ p__priority_specification_) : base (p__priority_specification_.id, p__priority_specification_.kind)
		{
			++g_counter;
			_init ();
			switch ((production_kind) p__priority_specification_.kind)
			{
				case production_kind.g__priority_specification__1:
					children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 2);
					if ((children [0] = m__priority_ = (p__priority_specification_.m__priority_ != null) ? new SyntaxTreeToken (p__priority_specification_.m__priority_) : null) != null) m__priority_.parent = this;
					if ((children [1] = m__number_ = (p__priority_specification_.m__number_ != null) ? new SyntaxTreeToken (p__priority_specification_.m__number_) : null) != null) m__number_.parent = this;
					break;
				case production_kind.g__priority_specification__2:
					children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 3);
					if ((children [0] = m__priority_ = (p__priority_specification_.m__priority_ != null) ? new SyntaxTreeToken (p__priority_specification_.m__priority_) : null) != null) m__priority_.parent = this;
					if ((children [1] = m__equals_sign_ = (p__priority_specification_.m__equals_sign_ != null) ? new SyntaxTreeToken (p__priority_specification_.m__equals_sign_) : null) != null) m__equals_sign_.parent = this;
					if ((children [2] = m__number_ = (p__priority_specification_.m__number_ != null) ? new SyntaxTreeToken (p__priority_specification_.m__number_) : null) != null) m__number_.parent = this;
					break;
				default:
				{
					string[] args = new string[] { "_priority_specification_" };
					throw new SyntaxTreeError (SyntaxTreeError.InvalidKindError, args);
				}
			}
		}

		// Clone node - wrapper of copy constructor

		public override SyntaxTreeBase Clone () { return (SyntaxTreeBase) new _priority_specification_ (this); }

		// Destructor

		public new void Dispose ()
		{
			base.Dispose ();
			--g_counter;

			m__priority_?.Dispose ();
			m__number_?.Dispose ();
			m__equals_sign_?.Dispose ();

			_init ();
		}
		#endregion
		#region Content changing and inclusion checking methods associated with the syntax rule <priority specification>

		// Content changing function(s) associated with production(s) of syntax rule <priority specification>

		//
		// Content changing function associated with following production(s)
		// <priority specification> -> '%priority' <number>

		//

		public void change(SyntaxTreeToken p_token, SyntaxTreeToken p_token_1)
		{
			_init ();
			this.kind = (uint) production_kind.g__priority_specification__1;
			children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 2);
			children [0] = m__priority_ = p_token;
			children [1] = m__number_ = p_token_1;
			reparent (parent);
		}

		//
		// Content changing function associated with following production(s)
		// <priority specification> -> '%priority' '=' <number>

		//

		public void change(SyntaxTreeToken p_token, SyntaxTreeToken p_token_1, SyntaxTreeToken p_token_2)
		{
			_init ();
			this.kind = (uint) production_kind.g__priority_specification__2;
			children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 3);
			children [0] = m__priority_ = p_token;
			children [1] = m__equals_sign_ = p_token_1;
			children [2] = m__number_ = p_token_2;
			reparent (parent);
		}

		//
		// check if node 'element' is included within syntax tree associated with syntax rule <priority specification>
		//

		public new bool checkInclusion (SyntaxTreeBase element)
		{
			return
				(element == this) ||
				(m__priority_ != null) && m__priority_.checkInclusion (element) ||
				(m__number_ != null) && m__number_.checkInclusion (element) ||
				(m__equals_sign_ != null) && m__equals_sign_.checkInclusion (element) ||
				false;
		}
		#endregion
		#region Syntax tree emit methods associated with the syntax rule <priority specification>

		//
		// emit production tree node associated with any production of syntax rule <priority specification>
		//

		public override string Emit (int depth)
		{
			string s = "";
			if (depth != 0)
			switch ((production_kind) kind)
			{
				case production_kind.g__priority_specification__1:
					// emit syntax tree node associated with production
					// <priority specification>: '%priority' <number>

					s += m__priority_.Emit (depth - 1);
					s += " ";
					s += m__number_.Emit (depth - 1);
				break;
				case production_kind.g__priority_specification__2:
					// emit syntax tree node associated with production
					// <priority specification>: '%priority' '=' <number>

					s += m__priority_.Emit (depth - 1);
					s += " ";
					s += m__equals_sign_.Emit (depth - 1);
					s += " ";
					s += m__number_.Emit (depth - 1);
				break;
			}
			return s.Trim ();
		}

		//
		// emit production tree node associated with any production of syntax rule <priority specification>
		//

		public override string EmitProductionTree (int depth)
		{
			string s = "_priority_specification_ (";
			if (depth != 0)
				switch ((production_kind) kind)
				{
				case production_kind.g__priority_specification__1:
					// emit syntax tree node associated with production
					// <priority specification>: '%priority' <number>

					s += "_priority_";
					s += ' ';
					s += "_number_";
				break;
				case production_kind.g__priority_specification__2:
					// emit syntax tree node associated with production
					// <priority specification>: '%priority' '=' <number>

					s += "_priority_";
					s += ' ';
					s += "_equals_sign_";
					s += ' ';
					s += "_number_";
				break;
			}
			s += ")";
			return s;
		}
		#endregion
		#region Syntax tree traversal,  reperrent, init and iterator methods associated with the syntax rule <priority specification>

		//
		// traverse sub-tree rooted in this node using selected syntax tree walker
		//

		public override void InvokeTraverse (SyntaxTreeWalkerCore syntaxTreeWalker) => ((SyntaxTreeWalker) syntaxTreeWalker).Traverse (this);
		public override void InvokeTraverseCommon (SyntaxTreeWalkerCore syntaxTreeWalker) => ((SyntaxTreeWalker) syntaxTreeWalker).TraverseCommon (this);

		// reparent sub-tree

		public override void reparent (SyntaxTreeBase parent)
		{
			this.parent = parent;

			m__priority_?.reparent (this);
			m__number_?.reparent (this);
			m__equals_sign_?.reparent (this);
		}

		//
		// initialize object associated with syntax rule <priority specification>
		//

		public void _init ()
		{
			m__priority_ = null;
			m__number_ = null;
			m__equals_sign_ = null;
		}
		#endregion
		#region Fields and properties associated with the syntax rule <priority specification>

		// counter of all nodes associated with syntax rule <priority specification>
		public static int g_counter;

		// objects associated with terminal and non-terminal symbols within production(s) of syntax rule <priority specification>
		public SyntaxTreeToken m__priority_ { get; private set; }
		public SyntaxTreeToken m__number_ { get; private set; }
		public SyntaxTreeToken m__equals_sign_ { get; private set; }
		#endregion

	};

	public partial class SyntaxTreeWalker
	{
		#region delegates and events associated with the syntax rule <priority specification>

		// delegate function (callback) prototype associated with syntax rule <priority specification>
		public delegate bool _priority_specification__Callback (SyntaxTreeCallbackReason reason, _priority_specification_.production_kind kind, _priority_specification_ p__priority_specification_);

		// event associated with syntax rule <priority specification>
		public event _priority_specification__Callback _priority_specification__Event;

		// event trigger associated with syntax rule <priority specification>
		public bool Raise__priority_specification__Event (SyntaxTreeCallbackReason reason, _priority_specification_.production_kind kind, _priority_specification_ p__priority_specification_)
		{
			bool? status = _priority_specification__Event?.Invoke (reason, kind, p__priority_specification_);
			return (status == null) || status.Value;
		}
		#endregion
		#region syntax tree traversal methods associated with the syntax rule <priority specification>
		//
		// traverse syntax tree node associated with any production of syntax rule <priority specification>
		//

		public void Traverse (_priority_specification_ p__priority_specification_)
		{
			if (p__priority_specification_.isLocked())
				return;
			p__priority_specification_.dolock();
			_priority_specification_.production_kind kind = (_priority_specification_.production_kind) p__priority_specification_.kind;
			p__priority_specification_.turn_reset ();
			if (Raise__priority_specification__Event (SyntaxTreeCallbackReason.TraversalPrologueCallbackReason, kind, p__priority_specification_))
			switch (kind)
			{
				case _priority_specification_.production_kind.g__priority_specification__1:
					// traverse syntax tree node associated with production
					// <priority specification>: '%priority' <number>

				break;
				case _priority_specification_.production_kind.g__priority_specification__2:
					// traverse syntax tree node associated with production
					// <priority specification>: '%priority' '=' <number>

				break;
			}
			Raise__priority_specification__Event (SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason, kind, p__priority_specification_);
			p__priority_specification_.unlock();
		}

		//
		// traverse syntax tree node associated with any production of syntax rule <priority specification>
		//

		public void TraverseCommon (_priority_specification_ p__priority_specification_)
		{
			_priority_specification_.production_kind kind = (_priority_specification_.production_kind) p__priority_specification_.kind;
			if (Raise_Common_Event (SyntaxTreeCallbackReason.TraversalPrologueCallbackReason, (int) kind, p__priority_specification_))
			switch (kind)
			{
				case _priority_specification_.production_kind.g__priority_specification__1:
					// traverse syntax tree node associated with production
					// <priority specification>: '%priority' <number>

						TraverseCommon (p__priority_specification_.m__priority_);
						TraverseCommon (p__priority_specification_.m__number_);
				break;
				case _priority_specification_.production_kind.g__priority_specification__2:
					// traverse syntax tree node associated with production
					// <priority specification>: '%priority' '=' <number>

						TraverseCommon (p__priority_specification_.m__priority_);
						TraverseCommon (p__priority_specification_.m__equals_sign_);
						TraverseCommon (p__priority_specification_.m__number_);
				break;
			}
			Raise_Common_Event (SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason, (int) kind, p__priority_specification_);
		}
	};
		#endregion

	public partial class SyntaxTreeBuilder : SyntaxTreeWalker
	{
		#region syntax tree builder methods associated with the syntax rule <priority specification>

		//
		// create syntax tree node associated with production
		// <priority specification>: '%priority' <number>

		//

		public _priority_specification_ _priority_specification__1 (SyntaxTreeToken p_token, SyntaxTreeToken p_token_1)
		{
			_priority_specification_ p__priority_specification__ref = new _priority_specification_(p_token, p_token_1);
			p_token.parent = p__priority_specification__ref;
			p_token_1.parent = p__priority_specification__ref;
			Raise__priority_specification__Event (SyntaxTreeCallbackReason.BuilderCallbackReason, _priority_specification_.production_kind.g__priority_specification__1, p__priority_specification__ref);
			return p__priority_specification__ref;
		}

		//
		// create syntax tree node associated with production
		// <priority specification>: '%priority' '=' <number>

		//

		public _priority_specification_ _priority_specification__2 (SyntaxTreeToken p_token, SyntaxTreeToken p_token_1, SyntaxTreeToken p_token_2)
		{
			_priority_specification_ p__priority_specification__ref = new _priority_specification_(p_token, p_token_1, p_token_2);
			p_token.parent = p__priority_specification__ref;
			p_token_1.parent = p__priority_specification__ref;
			p_token_2.parent = p__priority_specification__ref;
			Raise__priority_specification__Event (SyntaxTreeCallbackReason.BuilderCallbackReason, _priority_specification_.production_kind.g__priority_specification__2, p__priority_specification__ref);
			return p__priority_specification__ref;
		}
		#endregion
	};
}
