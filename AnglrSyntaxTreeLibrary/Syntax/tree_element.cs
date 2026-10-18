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
	// class associated with syntax rule tree-element
	//

	[DataContract]
	public class	tree_element : SyntaxTreeBase
	{
		#region enumerated production(s) of syntax rule tree-element
		public enum production_kind : ushort	// enumerated production(s) of syntax rule tree-element
		{
			g_tree_element_1 = 1,	// element-name ':' element-value

		};
		#endregion
		#region production markers associated with the syntax rule tree-element

		// markers associated with production: tree-element -> element-name ':' element-value

		public enum production_marker_1 : ushort
		{
			m_element_name,
			m_element_value,
			final
		};
		#endregion
		#region Constructors and destructors associated with the syntax rule tree-element

		// Constructor declaration(s) associated with production(s) of syntax rule tree-element

		//
		// Constructor associated with the following production(s)
		// tree-element -> element-name ':' element-value

		//

		public tree_element (element_name p_element_name, SyntaxTreeToken p_token, element_value p_element_value) : base ((uint) ProductionID._tree_element_ID, (uint) production_kind.g_tree_element_1)
		{
			++g_counter;
			_init ();
			children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 3);
			children[0] = m_element_name = p_element_name;
			children[1] = m__colon_ = p_token;
			children[2] = m_element_value = p_element_value;
		}

		public tree_element () { }

		// Copy constructor

		public tree_element (tree_element p_tree_element) : base (p_tree_element.id, p_tree_element.kind)
		{
			++g_counter;
			_init ();
			appInfo = p_tree_element.appInfo;
			switch ((production_kind) p_tree_element.kind)
			{
				case production_kind.g_tree_element_1:
					children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 3);
					if ((children [0] = m_element_name = (p_tree_element.m_element_name != null) ? new element_name (p_tree_element.m_element_name) : null) != null) m_element_name.parent = this;
					if ((children [1] = m__colon_ = (p_tree_element.m__colon_ != null) ? new SyntaxTreeToken (p_tree_element.m__colon_) : null) != null) m__colon_.parent = this;
					if ((children [2] = m_element_value = (p_tree_element.m_element_value != null) ? new element_value (p_tree_element.m_element_value) : null) != null) m_element_value.parent = this;
					break;
				default:
				{
					string[] args = new string[] { "tree_element" };
					throw new SyntaxTreeError (SyntaxTreeError.InvalidKindError, args);
				}
			}
		}

		// Clone node - wrapper of copy constructor

		public override SyntaxTreeBase Clone () { return (SyntaxTreeBase) new tree_element (this); }

		// Destructor

		public new void Dispose ()
		{
			base.Dispose ();
			--g_counter;

			m_element_name?.Dispose ();
			m__colon_?.Dispose ();
			m_element_value?.Dispose ();

			_init ();
		}
		#endregion
		#region Content changing and inclusion checking methods associated with the syntax rule tree-element

		// Content changing function(s) associated with production(s) of syntax rule tree-element

		//
		// Content changing function associated with following production(s)
		// tree-element -> element-name ':' element-value

		//

		public void change(element_name p_element_name, SyntaxTreeToken p_token, element_value p_element_value)
		{
			_init ();
			this.kind = (uint) production_kind.g_tree_element_1;
			children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 3);
			children [0] = m_element_name = p_element_name;
			children [1] = m__colon_ = p_token;
			children [2] = m_element_value = p_element_value;
			reparent (parent);
		}

		//
		// check if node 'element' is included within syntax tree associated with syntax rule tree-element
		//

		public new bool checkInclusion (SyntaxTreeBase element)
		{
			return
				(element == this) ||
				(m_element_name != null) && m_element_name.checkInclusion (element) ||
				(m__colon_ != null) && m__colon_.checkInclusion (element) ||
				(m_element_value != null) && m_element_value.checkInclusion (element) ||
				false;
		}
		#endregion
		#region Syntax tree emit methods associated with the syntax rule tree-element

		//
		// emit production tree node associated with any production of syntax rule tree-element
		//

		public override string Emit (int depth)
		{
			string s = "";
			if (depth != 0)
			switch ((production_kind) kind)
			{
				case production_kind.g_tree_element_1:
					// emit syntax tree node associated with production
					// tree-element: element-name ':' element-value

					s += m_element_name.Emit (depth - 1);
					s += " ";
					s += m__colon_.Emit (depth - 1);
					s += " ";
					s += m_element_value.Emit (depth - 1);
				break;
			}
			return s.Trim ();
		}

		//
		// emit production tree node associated with any production of syntax rule tree-element
		//

		public override string EmitProductionTree (int depth)
		{
			string s = "tree_element (";
			if (depth != 0)
				switch ((production_kind) kind)
				{
				case production_kind.g_tree_element_1:
					// emit syntax tree node associated with production
					// tree-element: element-name ':' element-value

					s += m_element_name.EmitProductionTree (depth - 1);
					s += ' ';
					s += "_colon_";
					s += ' ';
					s += m_element_value.EmitProductionTree (depth - 1);
				break;
			}
			s += ")";
			return s;
		}
		#endregion
		#region Syntax tree traversal,  reperrent, init and iterator methods associated with the syntax rule tree-element

		//
		// traverse sub-tree rooted in this node using selected syntax tree walker
		//

		public override void InvokeTraverse (SyntaxTreeWalkerCore syntaxTreeWalker) => ((SyntaxTreeWalker) syntaxTreeWalker).Traverse (this);
		public override void InvokeTraverseCommon (SyntaxTreeWalkerCore syntaxTreeWalker) => ((SyntaxTreeWalker) syntaxTreeWalker).TraverseCommon (this);

		//
		// traverse syntax tree node associated with any production of syntax rule tree-element with specified delegate
		//

		public override void Visit (Func<SyntaxTreeCallbackReason, int, SyntaxTreeBase, object, bool> visitor, object appData)
		{
			tree_element.production_kind kind = (tree_element.production_kind) this.kind;
			if (visitor (SyntaxTreeCallbackReason.TraversalPrologueCallbackReason, (int) kind, this, appData))
			switch (kind)
			{
				case tree_element.production_kind.g_tree_element_1:
					// traverse syntax tree node associated with production
					// tree-element: element-name ':' element-value

						m_element_name.Visit (visitor, appData);
						m__colon_.Visit (visitor, appData);
						m_element_value.Visit (visitor, appData);
				break;
			}
			visitor (SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason, (int) kind, this, appData);
		}

		// reparent sub-tree

		public override void reparent (SyntaxTreeBase parent)
		{
			this.parent = parent;

			m_element_name?.reparent (this);
			m__colon_?.reparent (this);
			m_element_value?.reparent (this);
		}

		//
		// initialize object associated with syntax rule tree-element
		//

		public void _init ()
		{
			m_element_name = null;
			m__colon_ = null;
			m_element_value = null;
		}
		#endregion
		#region Fields and properties associated with the syntax rule tree-element

		// counter of all nodes associated with syntax rule tree-element
		public static int g_counter;

		// objects associated with terminal and non-terminal symbols within production(s) of syntax rule tree-element
		[DataMember (Name = "m_element_name")] public element_name m_element_name { get; private set; }
		[DataMember (Name = "m__colon_")] public SyntaxTreeToken m__colon_ { get; private set; }
		[DataMember (Name = "m_element_value")] public element_value m_element_value { get; private set; }
		#endregion

	};

	public partial class SyntaxTreeWalker
	{
		#region delegates and events associated with the syntax rule tree-element

		// delegate function (callback) prototype associated with syntax rule tree-element
		public delegate bool tree_element_Callback (SyntaxTreeCallbackReason reason, tree_element.production_kind kind, tree_element p_tree_element);

		// event associated with syntax rule tree-element
		public event tree_element_Callback tree_element_Event;

		// event trigger associated with syntax rule tree-element
		public bool Raise_tree_element_Event (SyntaxTreeCallbackReason reason, tree_element.production_kind kind, tree_element p_tree_element)
		{
			bool? status = tree_element_Event?.Invoke (reason, kind, p_tree_element);
			return (status == null) || status.Value;
		}
		#endregion
		#region syntax tree traversal methods associated with the syntax rule tree-element
		//
		// traverse syntax tree node associated with any production of syntax rule tree-element
		//

		public void Traverse (tree_element p_tree_element)
		{
			if (p_tree_element.isLocked())
				return;
			p_tree_element.dolock();
			tree_element.production_kind kind = (tree_element.production_kind) p_tree_element.kind;
			p_tree_element.turn_reset ();
			if (Raise_tree_element_Event (SyntaxTreeCallbackReason.TraversalPrologueCallbackReason, kind, p_tree_element))
			switch (kind)
			{
				case tree_element.production_kind.g_tree_element_1:
					// traverse syntax tree node associated with production
					// tree-element: element-name ':' element-value

					if (Raise_tree_element_Event (SyntaxTreeCallbackReason.TraversalMidTermCallbackReason, kind, p_tree_element))
						Traverse (p_tree_element.m_element_name);
					p_tree_element.turn_inc ();
					if (Raise_tree_element_Event (SyntaxTreeCallbackReason.TraversalMidTermCallbackReason, kind, p_tree_element))
						Traverse (p_tree_element.m_element_value);
					p_tree_element.turn_inc ();
					Raise_tree_element_Event (SyntaxTreeCallbackReason.TraversalMidTermCallbackReason, kind, p_tree_element);
				break;
			}
			Raise_tree_element_Event (SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason, kind, p_tree_element);
			p_tree_element.unlock();
		}

		//
		// traverse syntax tree node associated with any production of syntax rule tree-element
		//

		public void TraverseCommon (tree_element p_tree_element)
		{
			tree_element.production_kind kind = (tree_element.production_kind) p_tree_element.kind;
			if (Raise_Common_Event (SyntaxTreeCallbackReason.TraversalPrologueCallbackReason, (int) kind, p_tree_element))
			switch (kind)
			{
				case tree_element.production_kind.g_tree_element_1:
					// traverse syntax tree node associated with production
					// tree-element: element-name ':' element-value

						TraverseCommon (p_tree_element.m_element_name);
						TraverseCommon (p_tree_element.m__colon_);
						TraverseCommon (p_tree_element.m_element_value);
				break;
			}
			Raise_Common_Event (SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason, (int) kind, p_tree_element);
		}
	};
		#endregion

	public partial class SyntaxTreeBuilder : SyntaxTreeWalker
	{
		#region syntax tree builder methods associated with the syntax rule tree-element

		//
		// create syntax tree node associated with production
		// tree-element: element-name ':' element-value

		//

		public tree_element tree_element_1 (element_name p_element_name, SyntaxTreeToken p_token, element_value p_element_value)
		{
			tree_element p_tree_element_ref = new tree_element(p_element_name, p_token, p_element_value);
			p_element_name.parent = p_tree_element_ref;
			p_token.parent = p_tree_element_ref;
			p_element_value.parent = p_tree_element_ref;
			Raise_tree_element_Event (SyntaxTreeCallbackReason.BuilderCallbackReason, tree_element.production_kind.g_tree_element_1, p_tree_element_ref);
			return p_tree_element_ref;
		}
		#endregion
	};
}
