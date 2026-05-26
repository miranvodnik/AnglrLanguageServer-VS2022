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
	// class associated with syntax rule <name>
	//

	public class	_name_ : SyntaxTreeBase
	{
		#region enumerated production(s) of syntax rule <name>
		public enum production_kind : ushort	// enumerated production(s) of syntax rule <name>
		{
			g__name__1 = 1,	// <any>

			g__name__2 = 2,	// <cstring>

			g__name__3 = 3,	// <identifier>

		};
		#endregion
		#region production markers associated with the syntax rule <name>

		// markers associated with production: <name> -> <any>

		public enum production_marker_1 : ushort
		{
			final
		};

		// markers associated with production: <name> -> <cstring>

		public enum production_marker_2 : ushort
		{
			final
		};

		// markers associated with production: <name> -> <identifier>

		public enum production_marker_3 : ushort
		{
			final
		};
		#endregion
		#region Constructors and destructors associated with the syntax rule <name>

		// Constructor declaration(s) associated with production(s) of syntax rule <name>

		//
		// Constructor associated with the following production(s)
		// <name> -> <any>

		// <name> -> <cstring>

		// <name> -> <identifier>

		//

		public _name_ (SyntaxTreeToken p_token, int kind) : base ((uint) ProductionID.__name__ID, (uint) kind)
		{
			++g_counter;
			_init ();
			switch ((production_kind) this.kind)
			{
			case production_kind.g__name__1:
				children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 1);
				children[0] = m__any_ = p_token;
				break;
			case production_kind.g__name__2:
				children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 1);
				children[0] = m__cstring_ = p_token;
				break;
			case production_kind.g__name__3:
				children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 1);
				children[0] = m__identifier_ = p_token;
				break;
			default:
				{
					string[] args = new string[] { "_name_" };
					throw new SyntaxTreeError (SyntaxTreeError.InvalidKindError, args);
				}
			}
		}

		// Copy constructor

		public _name_ (_name_ p__name_) : base (p__name_.id, p__name_.kind)
		{
			++g_counter;
			_init ();
			switch ((production_kind) this.kind)
			{
			case production_kind.g__name__1:
				children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 1);
				children[0] = m__any_ = p__name_.m__any_;
				break;
			default:
				string[] args = new string[] { "_name_" };
				throw new SyntaxTreeError (SyntaxTreeError.InvalidKindError, args);
			}
		}

		// Clone node - wrapper of copy constructor

		public override SyntaxTreeBase Clone () { return (SyntaxTreeBase) new _name_ (this); }

		// Destructor

		public new void Dispose ()
		{
			base.Dispose ();
			--g_counter;

			m__any_?.Dispose ();
			m__cstring_?.Dispose ();
			m__identifier_?.Dispose ();

			_init ();
		}
		#endregion
		#region Content changing and inclusion checking methods associated with the syntax rule <name>

		// Content changing function(s) associated with production(s) of syntax rule <name>

		//
		// Content changing function associated with following production(s)
		// <name> -> <any>

		// <name> -> <cstring>

		// <name> -> <identifier>

		//

		public void change(SyntaxTreeToken p_token, int kind)
		{
			_init ();
			this.kind = (uint) kind;
			switch ((production_kind) this.kind)
			{
			case production_kind.g__name__1:
				children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 1);
				children [0] = m__any_ = p_token;
				break;
			case production_kind.g__name__2:
				children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 1);
				children [0] = m__cstring_ = p_token;
				break;
			case production_kind.g__name__3:
				children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 1);
				children [0] = m__identifier_ = p_token;
				break;
			default:
				{
					string[] args = new string[] { "_name_" };
					throw new SyntaxTreeError (SyntaxTreeError.InvalidKindError, args);
				}
			}
			reparent (parent);
		}

		//
		// check if node 'element' is included within syntax tree associated with syntax rule <name>
		//

		public new bool checkInclusion (SyntaxTreeBase element)
		{
			return
				(element == this) ||
				(m__any_ != null) && m__any_.checkInclusion (element) ||
				(m__cstring_ != null) && m__cstring_.checkInclusion (element) ||
				(m__identifier_ != null) && m__identifier_.checkInclusion (element) ||
				false;
		}
		#endregion
		#region Syntax tree emit methods associated with the syntax rule <name>

		//
		// emit production tree node associated with any production of syntax rule <name>
		//

		public override string Emit (int depth)
		{
			string s = "";
			if (depth != 0)
			switch ((production_kind) kind)
			{
				case production_kind.g__name__1:
					// emit syntax tree node associated with production
					// <name>: <any>

					s += m__any_.Emit (depth - 1);
				break;
				case production_kind.g__name__2:
					// emit syntax tree node associated with production
					// <name>: <cstring>

					s += m__cstring_.Emit (depth - 1);
				break;
				case production_kind.g__name__3:
					// emit syntax tree node associated with production
					// <name>: <identifier>

					s += m__identifier_.Emit (depth - 1);
				break;
			}
			return s.Trim ();
		}

		//
		// emit production tree node associated with any production of syntax rule <name>
		//

		public override string EmitProductionTree (int depth)
		{
			string s = "_name_ (";
			if (depth != 0)
				switch ((production_kind) kind)
				{
				case production_kind.g__name__1:
					// emit syntax tree node associated with production
					// <name>: <any>

					s += "_any_";
				break;
				case production_kind.g__name__2:
					// emit syntax tree node associated with production
					// <name>: <cstring>

					s += "_cstring_";
				break;
				case production_kind.g__name__3:
					// emit syntax tree node associated with production
					// <name>: <identifier>

					s += "_identifier_";
				break;
			}
			s += ")";
			return s;
		}
		#endregion
		#region Syntax tree traversal,  reperrent, init and iterator methods associated with the syntax rule <name>

		//
		// traverse sub-tree rooted in this node using selected syntax tree walker
		//

		public override void InvokeTraverse (SyntaxTreeWalkerCore syntaxTreeWalker) => ((SyntaxTreeWalker) syntaxTreeWalker).Traverse (this);
		public override void InvokeTraverseCommon (SyntaxTreeWalkerCore syntaxTreeWalker) => ((SyntaxTreeWalker) syntaxTreeWalker).TraverseCommon (this);

		// reparent sub-tree

		public override void reparent (SyntaxTreeBase parent)
		{
			this.parent = parent;

			m__any_?.reparent (this);
			m__cstring_?.reparent (this);
			m__identifier_?.reparent (this);
		}

		//
		// initialize object associated with syntax rule <name>
		//

		public void _init ()
		{
			m__any_ = null;
			m__cstring_ = null;
			m__identifier_ = null;
		}
		#endregion
		#region Fields and properties associated with the syntax rule <name>

		// counter of all nodes associated with syntax rule <name>
		public static int g_counter;

		// objects associated with terminal and non-terminal symbols within production(s) of syntax rule <name>
		public SyntaxTreeToken m__any_ { get; private set; }
		public SyntaxTreeToken m__cstring_ { get; private set; }
		public SyntaxTreeToken m__identifier_ { get; private set; }
		#endregion

	};

	public partial class SyntaxTreeWalker
	{
		#region delegates and events associated with the syntax rule <name>

		// delegate function (callback) prototype associated with syntax rule <name>
		public delegate bool _name__Callback (SyntaxTreeCallbackReason reason, _name_.production_kind kind, _name_ p__name_);

		// event associated with syntax rule <name>
		public event _name__Callback _name__Event;

		// event trigger associated with syntax rule <name>
		public bool Raise__name__Event (SyntaxTreeCallbackReason reason, _name_.production_kind kind, _name_ p__name_)
		{
			bool? status = _name__Event?.Invoke (reason, kind, p__name_);
			return (status == null) || status.Value;
		}
		#endregion
		#region syntax tree traversal methods associated with the syntax rule <name>
		//
		// traverse syntax tree node associated with any production of syntax rule <name>
		//

		public void Traverse (_name_ p__name_)
		{
			if (p__name_.isLocked())
				return;
			p__name_.dolock();
			_name_.production_kind kind = (_name_.production_kind) p__name_.kind;
			p__name_.turn_reset ();
			if (Raise__name__Event (SyntaxTreeCallbackReason.TraversalPrologueCallbackReason, kind, p__name_))
			switch (kind)
			{
				case _name_.production_kind.g__name__1:
					// traverse syntax tree node associated with production
					// <name>: <any>

				break;
				case _name_.production_kind.g__name__2:
					// traverse syntax tree node associated with production
					// <name>: <cstring>

				break;
				case _name_.production_kind.g__name__3:
					// traverse syntax tree node associated with production
					// <name>: <identifier>

				break;
			}
			Raise__name__Event (SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason, kind, p__name_);
			p__name_.unlock();
		}

		//
		// traverse syntax tree node associated with any production of syntax rule <name>
		//

		public void TraverseCommon (_name_ p__name_)
		{
			_name_.production_kind kind = (_name_.production_kind) p__name_.kind;
			if (Raise_Common_Event (SyntaxTreeCallbackReason.TraversalPrologueCallbackReason, (int) kind, p__name_))
			switch (kind)
			{
				case _name_.production_kind.g__name__1:
					// traverse syntax tree node associated with production
					// <name>: <any>

						TraverseCommon (p__name_.m__any_);
				break;
				case _name_.production_kind.g__name__2:
					// traverse syntax tree node associated with production
					// <name>: <cstring>

						TraverseCommon (p__name_.m__cstring_);
				break;
				case _name_.production_kind.g__name__3:
					// traverse syntax tree node associated with production
					// <name>: <identifier>

						TraverseCommon (p__name_.m__identifier_);
				break;
			}
			Raise_Common_Event (SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason, (int) kind, p__name_);
		}
	};
		#endregion

	public partial class SyntaxTreeBuilder : SyntaxTreeWalker
	{
		#region syntax tree builder methods associated with the syntax rule <name>

		//
		// create syntax tree node associated with production
		// <name>: <any>

		//

		public _name_ _name__1 (SyntaxTreeToken p_token)
		{
			_name_ p__name__ref = new _name_(p_token, 1);
			p_token.parent = p__name__ref;
			Raise__name__Event (SyntaxTreeCallbackReason.BuilderCallbackReason, _name_.production_kind.g__name__1, p__name__ref);
			return p__name__ref;
		}

		//
		// create syntax tree node associated with production
		// <name>: <cstring>

		//

		public _name_ _name__2 (SyntaxTreeToken p_token)
		{
			_name_ p__name__ref = new _name_(p_token, 2);
			p_token.parent = p__name__ref;
			Raise__name__Event (SyntaxTreeCallbackReason.BuilderCallbackReason, _name_.production_kind.g__name__2, p__name__ref);
			return p__name__ref;
		}

		//
		// create syntax tree node associated with production
		// <name>: <identifier>

		//

		public _name_ _name__3 (SyntaxTreeToken p_token)
		{
			_name_ p__name__ref = new _name_(p_token, 3);
			p_token.parent = p__name__ref;
			Raise__name__Event (SyntaxTreeCallbackReason.BuilderCallbackReason, _name_.production_kind.g__name__3, p__name__ref);
			return p__name__ref;
		}
		#endregion
	};
}
