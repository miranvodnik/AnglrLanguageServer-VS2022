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
	// class associated with syntax rule <associativity specification>
	//

	public class	_associativity_specification_ : SyntaxTreeBase
	{
		#region enumerated production(s) of syntax rule <associativity specification>
		public enum production_kind : ushort	// enumerated production(s) of syntax rule <associativity specification>
		{
			g__associativity_specification__1 = 1,	// '%associativity' <cstring>

			g__associativity_specification__2 = 2,	// '%associativity' '=' <cstring>

			g__associativity_specification__3 = 3,	// '%associativity' <identifier>

			g__associativity_specification__4 = 4,	// '%associativity' '=' <identifier>

		};
		#endregion
		#region production markers associated with the syntax rule <associativity specification>

		// markers associated with production: <associativity specification> -> '%associativity' <cstring>

		public enum production_marker_1 : ushort
		{
			final
		};

		// markers associated with production: <associativity specification> -> '%associativity' '=' <cstring>

		public enum production_marker_2 : ushort
		{
			final
		};

		// markers associated with production: <associativity specification> -> '%associativity' <identifier>

		public enum production_marker_3 : ushort
		{
			final
		};

		// markers associated with production: <associativity specification> -> '%associativity' '=' <identifier>

		public enum production_marker_4 : ushort
		{
			final
		};
		#endregion
		#region Constructors and destructors associated with the syntax rule <associativity specification>

		// Constructor declaration(s) associated with production(s) of syntax rule <associativity specification>

		//
		// Constructor associated with the following production(s)
		// <associativity specification> -> '%associativity' <cstring>

		// <associativity specification> -> '%associativity' <identifier>

		//

		public _associativity_specification_ (SyntaxTreeToken p_token, SyntaxTreeToken p_token_1, int kind) : base ((uint) ProductionID.__associativity_specification__ID, (uint) kind)
		{
			++g_counter;
			_init ();
			switch ((production_kind) this.kind)
			{
			case production_kind.g__associativity_specification__1:
				children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 2);
				children[0] = m__associativity_ = p_token;
				children[1] = m__cstring_ = p_token_1;
				break;
			case production_kind.g__associativity_specification__3:
				children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 2);
				children[0] = m__associativity_ = p_token;
				children[1] = m__identifier_ = p_token_1;
				break;
			default:
				{
					string[] args = new string[] { "_associativity_specification_" };
					throw new SyntaxTreeError (SyntaxTreeError.InvalidKindError, args);
				}
			}
		}

		//
		// Constructor associated with the following production(s)
		// <associativity specification> -> '%associativity' '=' <cstring>

		// <associativity specification> -> '%associativity' '=' <identifier>

		//

		public _associativity_specification_ (SyntaxTreeToken p_token, SyntaxTreeToken p_token_1, SyntaxTreeToken p_token_2, int kind) : base ((uint) ProductionID.__associativity_specification__ID, (uint) kind)
		{
			++g_counter;
			_init ();
			switch ((production_kind) this.kind)
			{
			case production_kind.g__associativity_specification__2:
				children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 3);
				children[0] = m__associativity_ = p_token;
				children[1] = m__equals_sign_ = p_token_1;
				children[2] = m__cstring_ = p_token_2;
				break;
			case production_kind.g__associativity_specification__4:
				children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 3);
				children[0] = m__associativity_ = p_token;
				children[1] = m__equals_sign_ = p_token_1;
				children[2] = m__identifier_ = p_token_2;
				break;
			default:
				{
					string[] args = new string[] { "_associativity_specification_" };
					throw new SyntaxTreeError (SyntaxTreeError.InvalidKindError, args);
				}
			}
		}

		// Copy constructor

		public _associativity_specification_ (_associativity_specification_ p__associativity_specification_) : base (p__associativity_specification_.id, p__associativity_specification_.kind)
		{
			++g_counter;
			_init ();
			switch ((production_kind) p__associativity_specification_.kind)
			{
				case production_kind.g__associativity_specification__1:
					children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 2);
					if ((children [0] = m__associativity_ = (p__associativity_specification_.m__associativity_ != null) ? new SyntaxTreeToken (p__associativity_specification_.m__associativity_) : null) != null) m__associativity_.parent = this;
					if ((children [1] = m__cstring_ = (p__associativity_specification_.m__cstring_ != null) ? new SyntaxTreeToken (p__associativity_specification_.m__cstring_) : null) != null) m__cstring_.parent = this;
					break;
				case production_kind.g__associativity_specification__3:
					children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 2);
					if ((children [0] = m__associativity_ = (p__associativity_specification_.m__associativity_ != null) ? new SyntaxTreeToken (p__associativity_specification_.m__associativity_) : null) != null) m__associativity_.parent = this;
					if ((children [1] = m__identifier_ = (p__associativity_specification_.m__identifier_ != null) ? new SyntaxTreeToken (p__associativity_specification_.m__identifier_) : null) != null) m__identifier_.parent = this;
					break;
				case production_kind.g__associativity_specification__2:
					children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 3);
					if ((children [0] = m__associativity_ = (p__associativity_specification_.m__associativity_ != null) ? new SyntaxTreeToken (p__associativity_specification_.m__associativity_) : null) != null) m__associativity_.parent = this;
					if ((children [1] = m__equals_sign_ = (p__associativity_specification_.m__equals_sign_ != null) ? new SyntaxTreeToken (p__associativity_specification_.m__equals_sign_) : null) != null) m__equals_sign_.parent = this;
					if ((children [2] = m__cstring_ = (p__associativity_specification_.m__cstring_ != null) ? new SyntaxTreeToken (p__associativity_specification_.m__cstring_) : null) != null) m__cstring_.parent = this;
					break;
				case production_kind.g__associativity_specification__4:
					children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 3);
					if ((children [0] = m__associativity_ = (p__associativity_specification_.m__associativity_ != null) ? new SyntaxTreeToken (p__associativity_specification_.m__associativity_) : null) != null) m__associativity_.parent = this;
					if ((children [1] = m__equals_sign_ = (p__associativity_specification_.m__equals_sign_ != null) ? new SyntaxTreeToken (p__associativity_specification_.m__equals_sign_) : null) != null) m__equals_sign_.parent = this;
					if ((children [2] = m__identifier_ = (p__associativity_specification_.m__identifier_ != null) ? new SyntaxTreeToken (p__associativity_specification_.m__identifier_) : null) != null) m__identifier_.parent = this;
					break;
				default:
				{
					string[] args = new string[] { "_associativity_specification_" };
					throw new SyntaxTreeError (SyntaxTreeError.InvalidKindError, args);
				}
			}
		}

		// Clone node - wrapper of copy constructor

		public override SyntaxTreeBase Clone () { return (SyntaxTreeBase) new _associativity_specification_ (this); }

		// Destructor

		public new void Dispose ()
		{
			base.Dispose ();
			--g_counter;

			m__associativity_?.Dispose ();
			m__cstring_?.Dispose ();
			m__equals_sign_?.Dispose ();
			m__identifier_?.Dispose ();

			_init ();
		}
		#endregion
		#region Content changing and inclusion checking methods associated with the syntax rule <associativity specification>

		// Content changing function(s) associated with production(s) of syntax rule <associativity specification>

		//
		// Content changing function associated with following production(s)
		// <associativity specification> -> '%associativity' <cstring>

		// <associativity specification> -> '%associativity' <identifier>

		//

		public void change(SyntaxTreeToken p_token, SyntaxTreeToken p_token_1, int kind)
		{
			_init ();
			this.kind = (uint) kind;
			switch ((production_kind) this.kind)
			{
			case production_kind.g__associativity_specification__1:
				children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 2);
				children [0] = m__associativity_ = p_token;
				children [1] = m__cstring_ = p_token_1;
				break;
			case production_kind.g__associativity_specification__3:
				children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 2);
				children [0] = m__associativity_ = p_token;
				children [1] = m__identifier_ = p_token_1;
				break;
			default:
				{
					string[] args = new string[] { "_associativity_specification_" };
					throw new SyntaxTreeError (SyntaxTreeError.InvalidKindError, args);
				}
			}
			reparent (parent);
		}

		//
		// Content changing function associated with following production(s)
		// <associativity specification> -> '%associativity' '=' <cstring>

		// <associativity specification> -> '%associativity' '=' <identifier>

		//

		public void change(SyntaxTreeToken p_token, SyntaxTreeToken p_token_1, SyntaxTreeToken p_token_2, int kind)
		{
			_init ();
			this.kind = (uint) kind;
			switch ((production_kind) this.kind)
			{
			case production_kind.g__associativity_specification__2:
				children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 3);
				children [0] = m__associativity_ = p_token;
				children [1] = m__equals_sign_ = p_token_1;
				children [2] = m__cstring_ = p_token_2;
				break;
			case production_kind.g__associativity_specification__4:
				children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 3);
				children [0] = m__associativity_ = p_token;
				children [1] = m__equals_sign_ = p_token_1;
				children [2] = m__identifier_ = p_token_2;
				break;
			default:
				{
					string[] args = new string[] { "_associativity_specification_" };
					throw new SyntaxTreeError (SyntaxTreeError.InvalidKindError, args);
				}
			}
			reparent (parent);
		}

		//
		// check if node 'element' is included within syntax tree associated with syntax rule <associativity specification>
		//

		public new bool checkInclusion (SyntaxTreeBase element)
		{
			return
				(element == this) ||
				(m__associativity_ != null) && m__associativity_.checkInclusion (element) ||
				(m__cstring_ != null) && m__cstring_.checkInclusion (element) ||
				(m__equals_sign_ != null) && m__equals_sign_.checkInclusion (element) ||
				(m__identifier_ != null) && m__identifier_.checkInclusion (element) ||
				false;
		}
		#endregion
		#region Syntax tree emit methods associated with the syntax rule <associativity specification>

		//
		// emit production tree node associated with any production of syntax rule <associativity specification>
		//

		public override string Emit (int depth)
		{
			string s = "";
			if (depth != 0)
			switch ((production_kind) kind)
			{
				case production_kind.g__associativity_specification__1:
					// emit syntax tree node associated with production
					// <associativity specification>: '%associativity' <cstring>

					s += m__associativity_.Emit (depth - 1);
					s += " ";
					s += m__cstring_.Emit (depth - 1);
				break;
				case production_kind.g__associativity_specification__2:
					// emit syntax tree node associated with production
					// <associativity specification>: '%associativity' '=' <cstring>

					s += m__associativity_.Emit (depth - 1);
					s += " ";
					s += m__equals_sign_.Emit (depth - 1);
					s += " ";
					s += m__cstring_.Emit (depth - 1);
				break;
				case production_kind.g__associativity_specification__3:
					// emit syntax tree node associated with production
					// <associativity specification>: '%associativity' <identifier>

					s += m__associativity_.Emit (depth - 1);
					s += " ";
					s += m__identifier_.Emit (depth - 1);
				break;
				case production_kind.g__associativity_specification__4:
					// emit syntax tree node associated with production
					// <associativity specification>: '%associativity' '=' <identifier>

					s += m__associativity_.Emit (depth - 1);
					s += " ";
					s += m__equals_sign_.Emit (depth - 1);
					s += " ";
					s += m__identifier_.Emit (depth - 1);
				break;
			}
			return s.Trim ();
		}

		//
		// emit production tree node associated with any production of syntax rule <associativity specification>
		//

		public override string EmitProductionTree (int depth)
		{
			string s = "_associativity_specification_ (";
			if (depth != 0)
				switch ((production_kind) kind)
				{
				case production_kind.g__associativity_specification__1:
					// emit syntax tree node associated with production
					// <associativity specification>: '%associativity' <cstring>

					s += "_associativity_";
					s += ' ';
					s += "_cstring_";
				break;
				case production_kind.g__associativity_specification__2:
					// emit syntax tree node associated with production
					// <associativity specification>: '%associativity' '=' <cstring>

					s += "_associativity_";
					s += ' ';
					s += "_equals_sign_";
					s += ' ';
					s += "_cstring_";
				break;
				case production_kind.g__associativity_specification__3:
					// emit syntax tree node associated with production
					// <associativity specification>: '%associativity' <identifier>

					s += "_associativity_";
					s += ' ';
					s += "_identifier_";
				break;
				case production_kind.g__associativity_specification__4:
					// emit syntax tree node associated with production
					// <associativity specification>: '%associativity' '=' <identifier>

					s += "_associativity_";
					s += ' ';
					s += "_equals_sign_";
					s += ' ';
					s += "_identifier_";
				break;
			}
			s += ")";
			return s;
		}
		#endregion
		#region Syntax tree traversal,  reperrent, init and iterator methods associated with the syntax rule <associativity specification>

		//
		// traverse sub-tree rooted in this node using selected syntax tree walker
		//

		public override void InvokeTraverse (SyntaxTreeWalkerCore syntaxTreeWalker) => ((SyntaxTreeWalker) syntaxTreeWalker).Traverse (this);
		public override void InvokeTraverseCommon (SyntaxTreeWalkerCore syntaxTreeWalker) => ((SyntaxTreeWalker) syntaxTreeWalker).TraverseCommon (this);

		// reparent sub-tree

		public override void reparent (SyntaxTreeBase parent)
		{
			this.parent = parent;

			m__associativity_?.reparent (this);
			m__cstring_?.reparent (this);
			m__equals_sign_?.reparent (this);
			m__identifier_?.reparent (this);
		}

		//
		// initialize object associated with syntax rule <associativity specification>
		//

		public void _init ()
		{
			m__associativity_ = null;
			m__cstring_ = null;
			m__equals_sign_ = null;
			m__identifier_ = null;
		}
		#endregion
		#region Fields and properties associated with the syntax rule <associativity specification>

		// counter of all nodes associated with syntax rule <associativity specification>
		public static int g_counter;

		// objects associated with terminal and non-terminal symbols within production(s) of syntax rule <associativity specification>
		public SyntaxTreeToken m__associativity_ { get; private set; }
		public SyntaxTreeToken m__cstring_ { get; private set; }
		public SyntaxTreeToken m__equals_sign_ { get; private set; }
		public SyntaxTreeToken m__identifier_ { get; private set; }
		#endregion

	};

	public partial class SyntaxTreeWalker
	{
		#region delegates and events associated with the syntax rule <associativity specification>

		// delegate function (callback) prototype associated with syntax rule <associativity specification>
		public delegate bool _associativity_specification__Callback (SyntaxTreeCallbackReason reason, _associativity_specification_.production_kind kind, _associativity_specification_ p__associativity_specification_);

		// event associated with syntax rule <associativity specification>
		public event _associativity_specification__Callback _associativity_specification__Event;

		// event trigger associated with syntax rule <associativity specification>
		public bool Raise__associativity_specification__Event (SyntaxTreeCallbackReason reason, _associativity_specification_.production_kind kind, _associativity_specification_ p__associativity_specification_)
		{
			bool? status = _associativity_specification__Event?.Invoke (reason, kind, p__associativity_specification_);
			return (status == null) || status.Value;
		}
		#endregion
		#region syntax tree traversal methods associated with the syntax rule <associativity specification>
		//
		// traverse syntax tree node associated with any production of syntax rule <associativity specification>
		//

		public void Traverse (_associativity_specification_ p__associativity_specification_)
		{
			if (p__associativity_specification_.isLocked())
				return;
			p__associativity_specification_.dolock();
			_associativity_specification_.production_kind kind = (_associativity_specification_.production_kind) p__associativity_specification_.kind;
			p__associativity_specification_.turn_reset ();
			if (Raise__associativity_specification__Event (SyntaxTreeCallbackReason.TraversalPrologueCallbackReason, kind, p__associativity_specification_))
			switch (kind)
			{
				case _associativity_specification_.production_kind.g__associativity_specification__1:
					// traverse syntax tree node associated with production
					// <associativity specification>: '%associativity' <cstring>

				break;
				case _associativity_specification_.production_kind.g__associativity_specification__2:
					// traverse syntax tree node associated with production
					// <associativity specification>: '%associativity' '=' <cstring>

				break;
				case _associativity_specification_.production_kind.g__associativity_specification__3:
					// traverse syntax tree node associated with production
					// <associativity specification>: '%associativity' <identifier>

				break;
				case _associativity_specification_.production_kind.g__associativity_specification__4:
					// traverse syntax tree node associated with production
					// <associativity specification>: '%associativity' '=' <identifier>

				break;
			}
			Raise__associativity_specification__Event (SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason, kind, p__associativity_specification_);
			p__associativity_specification_.unlock();
		}

		//
		// traverse syntax tree node associated with any production of syntax rule <associativity specification>
		//

		public void TraverseCommon (_associativity_specification_ p__associativity_specification_)
		{
			_associativity_specification_.production_kind kind = (_associativity_specification_.production_kind) p__associativity_specification_.kind;
			if (Raise_Common_Event (SyntaxTreeCallbackReason.TraversalPrologueCallbackReason, (int) kind, p__associativity_specification_))
			switch (kind)
			{
				case _associativity_specification_.production_kind.g__associativity_specification__1:
					// traverse syntax tree node associated with production
					// <associativity specification>: '%associativity' <cstring>

						TraverseCommon (p__associativity_specification_.m__associativity_);
						TraverseCommon (p__associativity_specification_.m__cstring_);
				break;
				case _associativity_specification_.production_kind.g__associativity_specification__2:
					// traverse syntax tree node associated with production
					// <associativity specification>: '%associativity' '=' <cstring>

						TraverseCommon (p__associativity_specification_.m__associativity_);
						TraverseCommon (p__associativity_specification_.m__equals_sign_);
						TraverseCommon (p__associativity_specification_.m__cstring_);
				break;
				case _associativity_specification_.production_kind.g__associativity_specification__3:
					// traverse syntax tree node associated with production
					// <associativity specification>: '%associativity' <identifier>

						TraverseCommon (p__associativity_specification_.m__associativity_);
						TraverseCommon (p__associativity_specification_.m__identifier_);
				break;
				case _associativity_specification_.production_kind.g__associativity_specification__4:
					// traverse syntax tree node associated with production
					// <associativity specification>: '%associativity' '=' <identifier>

						TraverseCommon (p__associativity_specification_.m__associativity_);
						TraverseCommon (p__associativity_specification_.m__equals_sign_);
						TraverseCommon (p__associativity_specification_.m__identifier_);
				break;
			}
			Raise_Common_Event (SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason, (int) kind, p__associativity_specification_);
		}
	};
		#endregion

	public partial class SyntaxTreeBuilder : SyntaxTreeWalker
	{
		#region syntax tree builder methods associated with the syntax rule <associativity specification>

		//
		// create syntax tree node associated with production
		// <associativity specification>: '%associativity' <cstring>

		//

		public _associativity_specification_ _associativity_specification__1 (SyntaxTreeToken p_token, SyntaxTreeToken p_token_1)
		{
			_associativity_specification_ p__associativity_specification__ref = new _associativity_specification_(p_token, p_token_1, 1);
			p_token.parent = p__associativity_specification__ref;
			p_token_1.parent = p__associativity_specification__ref;
			Raise__associativity_specification__Event (SyntaxTreeCallbackReason.BuilderCallbackReason, _associativity_specification_.production_kind.g__associativity_specification__1, p__associativity_specification__ref);
			return p__associativity_specification__ref;
		}

		//
		// create syntax tree node associated with production
		// <associativity specification>: '%associativity' '=' <cstring>

		//

		public _associativity_specification_ _associativity_specification__2 (SyntaxTreeToken p_token, SyntaxTreeToken p_token_1, SyntaxTreeToken p_token_2)
		{
			_associativity_specification_ p__associativity_specification__ref = new _associativity_specification_(p_token, p_token_1, p_token_2, 2);
			p_token.parent = p__associativity_specification__ref;
			p_token_1.parent = p__associativity_specification__ref;
			p_token_2.parent = p__associativity_specification__ref;
			Raise__associativity_specification__Event (SyntaxTreeCallbackReason.BuilderCallbackReason, _associativity_specification_.production_kind.g__associativity_specification__2, p__associativity_specification__ref);
			return p__associativity_specification__ref;
		}

		//
		// create syntax tree node associated with production
		// <associativity specification>: '%associativity' <identifier>

		//

		public _associativity_specification_ _associativity_specification__3 (SyntaxTreeToken p_token, SyntaxTreeToken p_token_1)
		{
			_associativity_specification_ p__associativity_specification__ref = new _associativity_specification_(p_token, p_token_1, 3);
			p_token.parent = p__associativity_specification__ref;
			p_token_1.parent = p__associativity_specification__ref;
			Raise__associativity_specification__Event (SyntaxTreeCallbackReason.BuilderCallbackReason, _associativity_specification_.production_kind.g__associativity_specification__3, p__associativity_specification__ref);
			return p__associativity_specification__ref;
		}

		//
		// create syntax tree node associated with production
		// <associativity specification>: '%associativity' '=' <identifier>

		//

		public _associativity_specification_ _associativity_specification__4 (SyntaxTreeToken p_token, SyntaxTreeToken p_token_1, SyntaxTreeToken p_token_2)
		{
			_associativity_specification_ p__associativity_specification__ref = new _associativity_specification_(p_token, p_token_1, p_token_2, 4);
			p_token.parent = p__associativity_specification__ref;
			p_token_1.parent = p__associativity_specification__ref;
			p_token_2.parent = p__associativity_specification__ref;
			Raise__associativity_specification__Event (SyntaxTreeCallbackReason.BuilderCallbackReason, _associativity_specification_.production_kind.g__associativity_specification__4, p__associativity_specification__ref);
			return p__associativity_specification__ref;
		}
		#endregion
	};
}
