//
//	This file was generated with ANGLR compiler
//
using System;
using System.Runtime.Serialization;

using Anglr.Parser.Core;
using Anglr.Parser.SyntaxTree;
using Anglr.Parser.Walker;
namespace Tree.Parser
{
	//
	// class associated with syntax rule tree
	//

	[DataContract]
	public class	tree : SyntaxTreeBase
	{
		#region enumerated production(s) of syntax rule tree
		public enum production_kind : ushort	// enumerated production(s) of syntax rule tree
		{
			g_tree_1 = 1,	// '{' tree-element-set '}'

		};
		#endregion
		#region production markers associated with the syntax rule tree

		// markers associated with production: tree -> '{' tree-element-set '}'

		public enum production_marker_1 : ushort
		{
			m_tree_element_set,
			final
		};
		#endregion
		#region Constructors and destructors associated with the syntax rule tree

		// Constructor declaration(s) associated with production(s) of syntax rule tree

		//
		// Constructor associated with the following production(s)
		// tree -> '{' tree-element-set '}'

		//

		public tree (SyntaxTreeToken p_token, tree_element_set p_tree_element_set, SyntaxTreeToken p_token_1) : base ((uint) ProductionID._tree_ID, (uint) production_kind.g_tree_1)
		{
			++g_counter;
			_init ();
			children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 3);
			children[0] = m__left_curly_bracket_ = p_token;
			children[1] = m_tree_element_set = p_tree_element_set;
			children[2] = m__right_curly_bracket_ = p_token_1;
		}

		public tree () { }

		// Copy constructor

		public tree (tree p_tree) : base (p_tree.id, p_tree.kind)
		{
			++g_counter;
			_init ();
			appInfo = p_tree.appInfo;
			switch ((production_kind) p_tree.kind)
			{
				case production_kind.g_tree_1:
					children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 3);
					if ((children [0] = m__left_curly_bracket_ = (p_tree.m__left_curly_bracket_ != null) ? new SyntaxTreeToken (p_tree.m__left_curly_bracket_) : null) != null) m__left_curly_bracket_.parent = this;
					if ((children [1] = m_tree_element_set = (p_tree.m_tree_element_set != null) ? new tree_element_set (p_tree.m_tree_element_set) : null) != null) m_tree_element_set.parent = this;
					if ((children [2] = m__right_curly_bracket_ = (p_tree.m__right_curly_bracket_ != null) ? new SyntaxTreeToken (p_tree.m__right_curly_bracket_) : null) != null) m__right_curly_bracket_.parent = this;
					break;
				default:
				{
					string[] args = new string[] { "tree" };
					throw new SyntaxTreeError (SyntaxTreeError.InvalidKindError, args);
				}
			}
		}

		// Clone node - wrapper of copy constructor

		public override SyntaxTreeBase Clone () { return (SyntaxTreeBase) new tree (this); }

		// Destructor

		public new void Dispose ()
		{
			base.Dispose ();
			--g_counter;

			m__left_curly_bracket_?.Dispose ();
			m_tree_element_set?.Dispose ();
			m__right_curly_bracket_?.Dispose ();

			_init ();
		}
		#endregion
		#region Content changing and inclusion checking methods associated with the syntax rule tree

		// Content changing function(s) associated with production(s) of syntax rule tree

		//
		// Content changing function associated with following production(s)
		// tree -> '{' tree-element-set '}'

		//

		public void change(SyntaxTreeToken p_token, tree_element_set p_tree_element_set, SyntaxTreeToken p_token_1)
		{
			_init ();
			this.kind = (uint) production_kind.g_tree_1;
			children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 3);
			children [0] = m__left_curly_bracket_ = p_token;
			children [1] = m_tree_element_set = p_tree_element_set;
			children [2] = m__right_curly_bracket_ = p_token_1;
			reparent (parent);
		}

		//
		// check if node 'element' is included within syntax tree associated with syntax rule tree
		//

		public new bool checkInclusion (SyntaxTreeBase element)
		{
			return
				(element == this) ||
				(m__left_curly_bracket_ != null) && m__left_curly_bracket_.checkInclusion (element) ||
				(m_tree_element_set != null) && m_tree_element_set.checkInclusion (element) ||
				(m__right_curly_bracket_ != null) && m__right_curly_bracket_.checkInclusion (element) ||
				false;
		}
		#endregion
		#region Syntax tree emit methods associated with the syntax rule tree

		//
		// emit production tree node associated with any production of syntax rule tree
		//

		public override string Emit (int depth)
		{
			string s = "";
			if (depth != 0)
			switch ((production_kind) kind)
			{
				case production_kind.g_tree_1:
					// emit syntax tree node associated with production
					// tree: '{' tree-element-set '}'

					s += m__left_curly_bracket_.Emit (depth - 1);
					s += " ";
					s += m_tree_element_set.Emit (depth - 1);
					s += " ";
					s += m__right_curly_bracket_.Emit (depth - 1);
				break;
			}
			return s.Trim ();
		}

		//
		// emit production tree node associated with any production of syntax rule tree
		//

		public override string EmitProductionTree (int depth)
		{
			string s = "tree (";
			if (depth != 0)
				switch ((production_kind) kind)
				{
				case production_kind.g_tree_1:
					// emit syntax tree node associated with production
					// tree: '{' tree-element-set '}'

					s += "_left_curly_bracket_";
					s += ' ';
					s += m_tree_element_set.EmitProductionTree (depth - 1);
					s += ' ';
					s += "_right_curly_bracket_";
				break;
			}
			s += ")";
			return s;
		}
		#endregion
		#region Syntax tree traversal,  reperrent, init and iterator methods associated with the syntax rule tree

		//
		// traverse sub-tree rooted in this node using selected syntax tree walker
		//

		public override void InvokeTraverse (SyntaxTreeWalkerCore syntaxTreeWalker) => ((SyntaxTreeWalker) syntaxTreeWalker).Traverse (this);
		public override void InvokeTraverseCommon (SyntaxTreeWalkerCore syntaxTreeWalker) => ((SyntaxTreeWalker) syntaxTreeWalker).TraverseCommon (this);

		// reparent sub-tree

		public override void reparent (SyntaxTreeBase parent)
		{
			this.parent = parent;

			m__left_curly_bracket_?.reparent (this);
			m_tree_element_set?.reparent (this);
			m__right_curly_bracket_?.reparent (this);
		}

		//
		// initialize object associated with syntax rule tree
		//

		public void _init ()
		{
			m__left_curly_bracket_ = null;
			m_tree_element_set = null;
			m__right_curly_bracket_ = null;
		}
		#endregion
		#region Fields and properties associated with the syntax rule tree

		// counter of all nodes associated with syntax rule tree
		public static int g_counter;

		// objects associated with terminal and non-terminal symbols within production(s) of syntax rule tree
		[DataMember (Name = "m__left_curly_bracket_")] public SyntaxTreeToken m__left_curly_bracket_ { get; private set; }
		[DataMember (Name = "m_tree_element_set")] public tree_element_set m_tree_element_set { get; private set; }
		[DataMember (Name = "m__right_curly_bracket_")] public SyntaxTreeToken m__right_curly_bracket_ { get; private set; }
		#endregion

	};

	public partial class SyntaxTreeWalker
	{
		#region delegates and events associated with the syntax rule tree

		// delegate function (callback) prototype associated with syntax rule tree
		public delegate bool tree_Callback (SyntaxTreeCallbackReason reason, tree.production_kind kind, tree p_tree);

		// event associated with syntax rule tree
		public event tree_Callback tree_Event;

		// event trigger associated with syntax rule tree
		public bool Raise_tree_Event (SyntaxTreeCallbackReason reason, tree.production_kind kind, tree p_tree)
		{
			bool? status = tree_Event?.Invoke (reason, kind, p_tree);
			return (status == null) || status.Value;
		}
		#endregion
		#region syntax tree traversal methods associated with the syntax rule tree
		//
		// traverse syntax tree node associated with any production of syntax rule tree
		//

		public void Traverse (tree p_tree)
		{
			if (p_tree.isLocked())
				return;
			p_tree.dolock();
			tree.production_kind kind = (tree.production_kind) p_tree.kind;
			p_tree.turn_reset ();
			if (Raise_tree_Event (SyntaxTreeCallbackReason.TraversalPrologueCallbackReason, kind, p_tree))
			switch (kind)
			{
				case tree.production_kind.g_tree_1:
					// traverse syntax tree node associated with production
					// tree: '{' tree-element-set '}'

					if (Raise_tree_Event (SyntaxTreeCallbackReason.TraversalMidTermCallbackReason, kind, p_tree))
						Traverse (p_tree.m_tree_element_set);
					p_tree.turn_inc ();
					Raise_tree_Event (SyntaxTreeCallbackReason.TraversalMidTermCallbackReason, kind, p_tree);
				break;
			}
			Raise_tree_Event (SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason, kind, p_tree);
			p_tree.unlock();
		}

		//
		// traverse syntax tree node associated with any production of syntax rule tree
		//

		public void TraverseCommon (tree p_tree)
		{
			tree.production_kind kind = (tree.production_kind) p_tree.kind;
			if (Raise_Common_Event (SyntaxTreeCallbackReason.TraversalPrologueCallbackReason, (int) kind, p_tree))
			switch (kind)
			{
				case tree.production_kind.g_tree_1:
					// traverse syntax tree node associated with production
					// tree: '{' tree-element-set '}'

						TraverseCommon (p_tree.m__left_curly_bracket_);
						TraverseCommon (p_tree.m_tree_element_set);
						TraverseCommon (p_tree.m__right_curly_bracket_);
				break;
			}
			Raise_Common_Event (SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason, (int) kind, p_tree);
		}
	};
		#endregion

	public partial class SyntaxTreeBuilder : SyntaxTreeWalker
	{
		#region syntax tree builder methods associated with the syntax rule tree

		//
		// create syntax tree node associated with production
		// tree: '{' tree-element-set '}'

		//

		public tree tree_1 (SyntaxTreeToken p_token, tree_element_set p_tree_element_set, SyntaxTreeToken p_token_1)
		{
			tree p_tree_ref = new tree(p_token, p_tree_element_set, p_token_1);
			p_token.parent = p_tree_ref;
			p_tree_element_set.parent = p_tree_ref;
			p_token_1.parent = p_tree_ref;
			Raise_tree_Event (SyntaxTreeCallbackReason.BuilderCallbackReason, tree.production_kind.g_tree_1, p_tree_ref);
			return p_tree_ref;
		}
		#endregion
	};
}
