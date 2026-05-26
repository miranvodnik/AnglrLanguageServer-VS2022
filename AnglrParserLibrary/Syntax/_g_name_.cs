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
	// class associated with syntax rule <g name>
	//

	public class	_g_name_ : SyntaxTreeBase
	{
		#region enumerated production(s) of syntax rule <g name>
		public enum production_kind : ushort	// enumerated production(s) of syntax rule <g name>
		{
			g__g_name__1 = 1,	// <name>

			g__g_name__2 = 2,	// '(' <anglr nested rule> ')'

			g__g_name__3 = 3,	// <g name> <cardinality delimiter>

		};
		#endregion
		#region production markers associated with the syntax rule <g name>

		// markers associated with production: <g name> -> <name>

		public enum production_marker_1 : ushort
		{
			m__name_,
			final
		};

		// markers associated with production: <g name> -> '(' <anglr nested rule> ')'

		public enum production_marker_2 : ushort
		{
			m__anglr_nested_rule_,
			final
		};

		// markers associated with production: <g name> -> <g name> <cardinality delimiter>

		public enum production_marker_3 : ushort
		{
			m__g_name_,
			m__cardinality_delimiter_,
			final
		};
		#endregion
		#region Constructors and destructors associated with the syntax rule <g name>

		// Constructor declaration(s) associated with production(s) of syntax rule <g name>

		//
		// Constructor associated with the following production(s)
		// <g name> -> <name>

		//

		public _g_name_ (_name_ p__name_) : base ((uint) ProductionID.__g_name__ID, (uint) production_kind.g__g_name__1)
		{
			++g_counter;
			_init ();
			children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 1);
			children[0] = m__name_ = p__name_;
		}

		//
		// Constructor associated with the following production(s)
		// <g name> -> '(' <anglr nested rule> ')'

		//

		public _g_name_ (SyntaxTreeToken p_token, _anglr_nested_rule_ p__anglr_nested_rule_, SyntaxTreeToken p_token_1) : base ((uint) ProductionID.__g_name__ID, (uint) production_kind.g__g_name__2)
		{
			++g_counter;
			_init ();
			children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 3);
			children[0] = m__left_bracket_ = p_token;
			children[1] = m__anglr_nested_rule_ = p__anglr_nested_rule_;
			children[2] = m__right_bracket_ = p_token_1;
		}

		//
		// Constructor associated with the following production(s)
		// <g name> -> <g name> <cardinality delimiter>

		//

		public _g_name_ (_g_name_ p__g_name_, _cardinality_delimiter_ p__cardinality_delimiter_) : base ((uint) ProductionID.__g_name__ID, (uint) production_kind.g__g_name__3)
		{
			++g_counter;
			_init ();
			children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 2);
			children[0] = m__g_name_ = p__g_name_;
			children[1] = m__cardinality_delimiter_ = p__cardinality_delimiter_;
		}

		// Copy constructor

		public _g_name_ (_g_name_ p__g_name_) : base (p__g_name_.id, p__g_name_.kind)
		{
			++g_counter;
			_init ();
			switch ((production_kind) p__g_name_.kind)
			{
				case production_kind.g__g_name__1:
					children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 1);
					if ((children [0] = m__name_ = (p__g_name_.m__name_ != null) ? new _name_ (p__g_name_.m__name_) : null) != null) m__name_.parent = this;
					break;
				case production_kind.g__g_name__2:
					children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 3);
					if ((children [0] = m__left_bracket_ = (p__g_name_.m__left_bracket_ != null) ? new SyntaxTreeToken (p__g_name_.m__left_bracket_) : null) != null) m__left_bracket_.parent = this;
					if ((children [1] = m__anglr_nested_rule_ = (p__g_name_.m__anglr_nested_rule_ != null) ? new _anglr_nested_rule_ (p__g_name_.m__anglr_nested_rule_) : null) != null) m__anglr_nested_rule_.parent = this;
					if ((children [2] = m__right_bracket_ = (p__g_name_.m__right_bracket_ != null) ? new SyntaxTreeToken (p__g_name_.m__right_bracket_) : null) != null) m__right_bracket_.parent = this;
					break;
				case production_kind.g__g_name__3:
					children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 2);
					if ((children [0] = m__g_name_ = (p__g_name_.m__g_name_ != null) ? new _g_name_ (p__g_name_.m__g_name_) : null) != null) m__g_name_.parent = this;
					if ((children [1] = m__cardinality_delimiter_ = (p__g_name_.m__cardinality_delimiter_ != null) ? new _cardinality_delimiter_ (p__g_name_.m__cardinality_delimiter_) : null) != null) m__cardinality_delimiter_.parent = this;
					break;
				default:
				{
					string[] args = new string[] { "_g_name_" };
					throw new SyntaxTreeError (SyntaxTreeError.InvalidKindError, args);
				}
			}
		}

		// Clone node - wrapper of copy constructor

		public override SyntaxTreeBase Clone () { return (SyntaxTreeBase) new _g_name_ (this); }

		// Destructor

		public new void Dispose ()
		{
			Iterate (null, (node, appData) =>
			{
				--g_counter;

				node.m__name_?.Dispose ();
				node.m__left_bracket_?.Dispose ();
				node.m__anglr_nested_rule_?.Dispose ();
				node.m__right_bracket_?.Dispose ();
				node.m__cardinality_delimiter_?.Dispose ();

				node._init ();

				return null;
			});
		}
		#endregion
		#region Content changing and inclusion checking methods associated with the syntax rule <g name>

		// Content changing function(s) associated with production(s) of syntax rule <g name>

		//
		// Content changing function associated with following production(s)
		// <g name> -> <name>

		//

		public void change(_name_ p__name_)
		{
			_init ();
			this.kind = (uint) production_kind.g__g_name__1;
			children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 1);
			children [0] = m__name_ = p__name_;
			reparent (parent);
		}

		//
		// Content changing function associated with following production(s)
		// <g name> -> '(' <anglr nested rule> ')'

		//

		public void change(SyntaxTreeToken p_token, _anglr_nested_rule_ p__anglr_nested_rule_, SyntaxTreeToken p_token_1)
		{
			_init ();
			this.kind = (uint) production_kind.g__g_name__2;
			children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 3);
			children [0] = m__left_bracket_ = p_token;
			children [1] = m__anglr_nested_rule_ = p__anglr_nested_rule_;
			children [2] = m__right_bracket_ = p_token_1;
			reparent (parent);
		}

		//
		// Content changing function associated with following production(s)
		// <g name> -> <g name> <cardinality delimiter>

		//

		public void change(_g_name_ p__g_name_, _cardinality_delimiter_ p__cardinality_delimiter_)
		{
			_init ();
			this.kind = (uint) production_kind.g__g_name__3;
			children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 2);
			children [0] = m__g_name_ = p__g_name_;
			children [1] = m__cardinality_delimiter_ = p__cardinality_delimiter_;
			reparent (parent);
		}

		//
		// check if node 'element' is included within syntax tree associated with syntax rule <g name>
		//

		public new bool checkInclusion (SyntaxTreeBase element)
		{
			return (element == this) || (bool) Iterate (false, (node, appData) =>
			{
				return
					(bool) appData ||
					(node.m__name_ != null) && node.m__name_.checkInclusion (element) ||
					(node.m__left_bracket_ != null) && node.m__left_bracket_.checkInclusion (element) ||
					(node.m__anglr_nested_rule_ != null) && node.m__anglr_nested_rule_.checkInclusion (element) ||
					(node.m__right_bracket_ != null) && node.m__right_bracket_.checkInclusion (element) ||
					(node.m__cardinality_delimiter_ != null) && node.m__cardinality_delimiter_.checkInclusion (element) ||
					false;
			});
		}
		#endregion
		#region Syntax tree emit methods associated with the syntax rule <g name>

		//
		// emit production tree node associated with any production of syntax rule <g name>
		//

		public override string Emit (int depth)
		{
			string s = "";
			if (depth != 0)
				Iterate (null, (node, appData) =>
				{
					switch ((production_kind) node.kind)
					{
					case production_kind.g__g_name__1:
						// emit syntax tree node associated with production
						// <g name>: <name>

						s += " " + node.m__name_.Emit (depth - 1);
						break;
					case production_kind.g__g_name__2:
						// emit syntax tree node associated with production
						// <g name>: '(' <anglr nested rule> ')'

						s += " " + node.m__left_bracket_.Emit (depth - 1);
						s += " " + node.m__anglr_nested_rule_.Emit (depth - 1);
						s += " " + node.m__right_bracket_.Emit (depth - 1);
						break;
					case production_kind.g__g_name__3:
						// emit syntax tree node associated with production
						// <g name>: <g name> <cardinality delimiter>

						s += " " + node.m__cardinality_delimiter_.Emit (depth - 1);
						break;
					}
					return null;
				});
			return s.Trim ();
		}

		//
		// emit production tree node associated with any production of syntax rule <g name>
		//

		public override string EmitProductionTree (int depth)
		{
			return (depth != 0) ?
				(string) Iterate ("", (node, appData) =>
				{
					string str = "_g_name_ (" + (string) appData;
					switch ((production_kind) node.kind)
					{
					case production_kind.g__g_name__1:
						// emit syntax tree node associated with production
						// <g name>: <name>

						str += node.m__name_.EmitProductionTree (depth - 1);
						break;
					case production_kind.g__g_name__2:
						// emit syntax tree node associated with production
						// <g name>: '(' <anglr nested rule> ')'

						str += "_left_bracket_";
						str += ' ';
						str += node.m__anglr_nested_rule_.EmitProductionTree (depth - 1);
						str += ' ';
						str += "_right_bracket_";
						break;
					case production_kind.g__g_name__3:
						// emit syntax tree node associated with production
						// <g name>: <g name> <cardinality delimiter>

						str += ' ';
						str += node.m__cardinality_delimiter_.EmitProductionTree (depth - 1);
						break;
					}
					return str + ")";
				}) : "";
		}
		#endregion
		#region Syntax tree traversal,  reperrent, init and iterator methods associated with the syntax rule <g name>

		//
		// traverse sub-tree rooted in this node using selected syntax tree walker
		//

		public override void InvokeTraverse (SyntaxTreeWalkerCore syntaxTreeWalker) => ((SyntaxTreeWalker) syntaxTreeWalker).Traverse (this);
		public override void InvokeTraverseCommon (SyntaxTreeWalkerCore syntaxTreeWalker) => ((SyntaxTreeWalker) syntaxTreeWalker).TraverseCommon (this);

		// reparent sub-tree

		public override void reparent (SyntaxTreeBase parent)
		{
			for (_g_name_ node = this; node != null; node = node.m__g_name_)
			{
				node.parent = parent;
				node.m__name_?.reparent (parent);
				node.m__left_bracket_?.reparent (parent);
				node.m__anglr_nested_rule_?.reparent (parent);
				node.m__right_bracket_?.reparent (parent);
				node.m__cardinality_delimiter_?.reparent (parent);
				parent = node;
			}
		}

		//
		// initialize object associated with syntax rule <g name>
		//

		public void _init ()
		{
			m__name_ = null;
			m__left_bracket_ = null;
			m__anglr_nested_rule_ = null;
			m__right_bracket_ = null;
			m__g_name_ = null;
			m__cardinality_delimiter_ = null;
		}

		public delegate object IteratorDelegate (_g_name_ p__g_name_, object appData);

		public object Iterate (object appData, IteratorDelegate iteratorDelegate)
		{
			_g_name_ p__g_name_;
			for (p__g_name_ = this; p__g_name_.m__g_name_ != null; p__g_name_ = p__g_name_.m__g_name_);
			for (SyntaxTreeBase parent = p__g_name_; (parent != null) && (parent is _g_name_); parent = parent.parent)
				appData = iteratorDelegate ((_g_name_) parent, appData);
			return appData;
		}
		#endregion
		#region Fields and properties associated with the syntax rule <g name>

		// counter of all nodes associated with syntax rule <g name>
		public static int g_counter;

		// objects associated with terminal and non-terminal symbols within production(s) of syntax rule <g name>
		public _name_ m__name_ { get; private set; }
		public SyntaxTreeToken m__left_bracket_ { get; private set; }
		public _anglr_nested_rule_ m__anglr_nested_rule_ { get; private set; }
		public SyntaxTreeToken m__right_bracket_ { get; private set; }
		public _g_name_ m__g_name_ { get; private set; }
		public _cardinality_delimiter_ m__cardinality_delimiter_ { get; private set; }
		#endregion

	};

	public partial class SyntaxTreeWalker
	{
		#region delegates and events associated with the syntax rule <g name>

		// delegate function (callback) prototype associated with syntax rule <g name>
		public delegate bool _g_name__Callback (SyntaxTreeCallbackReason reason, _g_name_.production_kind kind, _g_name_ p__g_name_);

		// event associated with syntax rule <g name>
		public event _g_name__Callback _g_name__Event;

		// event trigger associated with syntax rule <g name>
		public bool Raise__g_name__Event (SyntaxTreeCallbackReason reason, _g_name_.production_kind kind, _g_name_ p__g_name_)
		{
			bool? status = _g_name__Event?.Invoke (reason, kind, p__g_name_);
			return (status == null) || status.Value;
		}
		#endregion
		#region syntax tree traversal methods associated with the syntax rule <g name>
		//
		// traverse syntax tree node associated with any production of syntax rule <g name>
		//

		public void Traverse (_g_name_ p__g_name_)
		{
			if (p__g_name_.isLocked())
				return;
			if (Raise__g_name__Event (SyntaxTreeCallbackReason.IterationPrologueCallbackReason, (_g_name_.production_kind) p__g_name_.kind, p__g_name_))
				p__g_name_.Iterate (null, (node, appData) =>
				{
					if (node.isLocked())
						return null;
					node.dolock();
					_g_name_.production_kind kind = (_g_name_.production_kind) node.kind;
					node.turn_reset ();
					if (Raise__g_name__Event (SyntaxTreeCallbackReason.TraversalPrologueCallbackReason, kind, node))
						switch (kind)
						{
							case _g_name_.production_kind.g__g_name__1:
								// traverse syntax tree node associated with production
								// <g name>: <name>

								if (Raise__g_name__Event (SyntaxTreeCallbackReason.TraversalMidTermCallbackReason, kind, node))
									Traverse (node.m__name_);
								node.turn_inc ();
								Raise__g_name__Event (SyntaxTreeCallbackReason.TraversalMidTermCallbackReason, kind, node);
							break;
							case _g_name_.production_kind.g__g_name__2:
								// traverse syntax tree node associated with production
								// <g name>: '(' <anglr nested rule> ')'

								if (Raise__g_name__Event (SyntaxTreeCallbackReason.TraversalMidTermCallbackReason, kind, node))
									Traverse (node.m__anglr_nested_rule_);
								node.turn_inc ();
								Raise__g_name__Event (SyntaxTreeCallbackReason.TraversalMidTermCallbackReason, kind, node);
							break;
							case _g_name_.production_kind.g__g_name__3:
								// traverse syntax tree node associated with production
								// <g name>: <g name> <cardinality delimiter>

								if (Raise__g_name__Event (SyntaxTreeCallbackReason.TraversalMidTermCallbackReason, kind, node))
									;
								node.turn_inc ();
								if (Raise__g_name__Event (SyntaxTreeCallbackReason.TraversalMidTermCallbackReason, kind, node))
									Traverse (node.m__cardinality_delimiter_);
								node.turn_inc ();
								Raise__g_name__Event (SyntaxTreeCallbackReason.TraversalMidTermCallbackReason, kind, node);
							break;
						}
					Raise__g_name__Event (SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason, kind, node);
					node.unlock();
					return null;
				});
			Raise__g_name__Event (SyntaxTreeCallbackReason.IterationEpilogueCallbackReason, (_g_name_.production_kind) p__g_name_.kind, p__g_name_);
		}

		//
		// traverse syntax tree node associated with any production of syntax rule <g name>
		//

		public void TraverseCommon (_g_name_ p__g_name_)
		{
			if (Raise_Common_Event (SyntaxTreeCallbackReason.IterationPrologueCallbackReason, (int) p__g_name_.kind, p__g_name_))
				p__g_name_.Iterate (null, (node, appData) =>
				{
					_g_name_.production_kind kind = (_g_name_.production_kind) node.kind;
					if (Raise_Common_Event (SyntaxTreeCallbackReason.TraversalPrologueCallbackReason, (int) kind, node))
						switch (kind)
						{
						case _g_name_.production_kind.g__g_name__1:
							// traverse syntax tree node associated with production
							// <g name>: <name>

							TraverseCommon (node.m__name_);
							break;
						case _g_name_.production_kind.g__g_name__2:
							// traverse syntax tree node associated with production
							// <g name>: '(' <anglr nested rule> ')'

							TraverseCommon (node.m__left_bracket_);
							TraverseCommon (node.m__anglr_nested_rule_);
							TraverseCommon (node.m__right_bracket_);
							break;
						case _g_name_.production_kind.g__g_name__3:
							// traverse syntax tree node associated with production
							// <g name>: <g name> <cardinality delimiter>

							TraverseCommon (node.m__cardinality_delimiter_);
							break;
						}
					Raise_Common_Event (SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason, (int) kind, node);
					return null;
				});
			Raise_Common_Event (SyntaxTreeCallbackReason.IterationEpilogueCallbackReason, (int) p__g_name_.kind, p__g_name_);
		}
	};
		#endregion

	public partial class SyntaxTreeBuilder : SyntaxTreeWalker
	{
		#region syntax tree builder methods associated with the syntax rule <g name>

		//
		// create syntax tree node associated with production
		// <g name>: <name>

		//

		public _g_name_ _g_name__1 (_name_ p__name_)
		{
			_g_name_ p__g_name__ref = new _g_name_(p__name_);
			p__name_.parent = p__g_name__ref;
			Raise__g_name__Event (SyntaxTreeCallbackReason.BuilderCallbackReason, _g_name_.production_kind.g__g_name__1, p__g_name__ref);
			return p__g_name__ref;
		}

		//
		// create syntax tree node associated with production
		// <g name>: '(' <anglr nested rule> ')'

		//

		public _g_name_ _g_name__2 (SyntaxTreeToken p_token, _anglr_nested_rule_ p__anglr_nested_rule_, SyntaxTreeToken p_token_1)
		{
			_g_name_ p__g_name__ref = new _g_name_(p_token, p__anglr_nested_rule_, p_token_1);
			p_token.parent = p__g_name__ref;
			p__anglr_nested_rule_.parent = p__g_name__ref;
			p_token_1.parent = p__g_name__ref;
			Raise__g_name__Event (SyntaxTreeCallbackReason.BuilderCallbackReason, _g_name_.production_kind.g__g_name__2, p__g_name__ref);
			return p__g_name__ref;
		}

		//
		// create syntax tree node associated with production
		// <g name>: <g name> <cardinality delimiter>

		//

		public _g_name_ _g_name__3 (_g_name_ p__g_name_, _cardinality_delimiter_ p__cardinality_delimiter_)
		{
			_g_name_ p__g_name__ref = new _g_name_(p__g_name_, p__cardinality_delimiter_);
			p__g_name_.parent = p__g_name__ref;
			p__cardinality_delimiter_.parent = p__g_name__ref;
			Raise__g_name__Event (SyntaxTreeCallbackReason.BuilderCallbackReason, _g_name_.production_kind.g__g_name__3, p__g_name__ref);
			return p__g_name__ref;
		}
		#endregion
	};
}
