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
	// class associated with syntax rule tree-element-set
	//

	[DataContract]
	public class	tree_element_set : SyntaxTreeBase
	{
		#region enumerated production(s) of syntax rule tree-element-set
		public enum production_kind : ushort	// enumerated production(s) of syntax rule tree-element-set
		{
			g_tree_element_set_1 = 1,	// tree-element

			g_tree_element_set_2 = 2,	// tree-element-set ',' tree-element Left

		};
		#endregion
		#region production markers associated with the syntax rule tree-element-set

		// markers associated with production: tree-element-set -> tree-element

		public enum production_marker_1 : ushort
		{
			m_tree_element,
			final
		};

		// markers associated with production: tree-element-set -> tree-element-set ',' tree-element Left

		public enum production_marker_2 : ushort
		{
			m_tree_element_set,
			m_tree_element,
			final
		};
		#endregion
		#region Constructors and destructors associated with the syntax rule tree-element-set

		// Constructor declaration(s) associated with production(s) of syntax rule tree-element-set

		//
		// Constructor associated with the following production(s)
		// tree-element-set -> tree-element

		//

		public tree_element_set (tree_element p_tree_element) : base ((uint) ProductionID._tree_element_set_ID, (uint) production_kind.g_tree_element_set_1)
		{
			++g_counter;
			_init ();
			children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 1);
			children[0] = m_tree_element = p_tree_element;
		}

		//
		// Constructor associated with the following production(s)
		// tree-element-set -> tree-element-set ',' tree-element Left

		//

		public tree_element_set (tree_element_set p_tree_element_set, SyntaxTreeToken p_token, tree_element p_tree_element) : base ((uint) ProductionID._tree_element_set_ID, (uint) production_kind.g_tree_element_set_2)
		{
			++g_counter;
			_init ();
			children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 3);
			children[0] = m_tree_element_set = p_tree_element_set;
			children[1] = m__coma_ = p_token;
			children[2] = m_tree_element = p_tree_element;
		}

		public tree_element_set () { }

		// Copy constructor

		public tree_element_set (tree_element_set p_tree_element_set) : base (p_tree_element_set.id, p_tree_element_set.kind)
		{
			++g_counter;
			_init ();
			appInfo = p_tree_element_set.appInfo;
			switch ((production_kind) p_tree_element_set.kind)
			{
				case production_kind.g_tree_element_set_1:
					children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 1);
					if ((children [0] = m_tree_element = (p_tree_element_set.m_tree_element != null) ? new tree_element (p_tree_element_set.m_tree_element) : null) != null) m_tree_element.parent = this;
					break;
				case production_kind.g_tree_element_set_2:
					children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 3);
					if ((children [0] = m_tree_element_set = (p_tree_element_set.m_tree_element_set != null) ? new tree_element_set (p_tree_element_set.m_tree_element_set) : null) != null) m_tree_element_set.parent = this;
					if ((children [1] = m__coma_ = (p_tree_element_set.m__coma_ != null) ? new SyntaxTreeToken (p_tree_element_set.m__coma_) : null) != null) m__coma_.parent = this;
					if ((children [2] = m_tree_element = (p_tree_element_set.m_tree_element != null) ? new tree_element (p_tree_element_set.m_tree_element) : null) != null) m_tree_element.parent = this;
					break;
				default:
				{
					string[] args = new string[] { "tree_element_set" };
					throw new SyntaxTreeError (SyntaxTreeError.InvalidKindError, args);
				}
			}
		}

		// Clone node - wrapper of copy constructor

		public override SyntaxTreeBase Clone () { return (SyntaxTreeBase) new tree_element_set (this); }

		// Destructor

		public new void Dispose ()
		{
			Iterate (null, (node, appData) =>
			{
				--g_counter;

				node.m_tree_element?.Dispose ();
				node.m__coma_?.Dispose ();

				node._init ();

				return null;
			});
		}
		#endregion
		#region Content changing and inclusion checking methods associated with the syntax rule tree-element-set

		// Content changing function(s) associated with production(s) of syntax rule tree-element-set

		//
		// Content changing function associated with following production(s)
		// tree-element-set -> tree-element

		//

		public void change(tree_element p_tree_element)
		{
			_init ();
			this.kind = (uint) production_kind.g_tree_element_set_1;
			children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 1);
			children [0] = m_tree_element = p_tree_element;
			reparent (parent);
		}

		//
		// Content changing function associated with following production(s)
		// tree-element-set -> tree-element-set ',' tree-element Left

		//

		public void change(tree_element_set p_tree_element_set, SyntaxTreeToken p_token, tree_element p_tree_element)
		{
			_init ();
			this.kind = (uint) production_kind.g_tree_element_set_2;
			children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 3);
			children [0] = m_tree_element_set = p_tree_element_set;
			children [1] = m__coma_ = p_token;
			children [2] = m_tree_element = p_tree_element;
			reparent (parent);
		}

		//
		// check if node 'element' is included within syntax tree associated with syntax rule tree-element-set
		//

		public new bool checkInclusion (SyntaxTreeBase element)
		{
			return (element == this) || (bool) Iterate (false, (node, appData) =>
			{
				return
					(bool) appData ||
					(node.m_tree_element != null) && node.m_tree_element.checkInclusion (element) ||
					(node.m__coma_ != null) && node.m__coma_.checkInclusion (element) ||
					false;
			});
		}
		#endregion
		#region Syntax tree emit methods associated with the syntax rule tree-element-set

		//
		// emit production tree node associated with any production of syntax rule tree-element-set
		//

		public override string Emit (int depth)
		{
			string s = "";
			if (depth != 0)
				Iterate (null, (node, appData) =>
				{
					switch ((production_kind) node.kind)
					{
					case production_kind.g_tree_element_set_1:
						// emit syntax tree node associated with production
						// tree-element-set: tree-element

						s += " " + node.m_tree_element.Emit (depth - 1);
						break;
					case production_kind.g_tree_element_set_2:
						// emit syntax tree node associated with production
						// tree-element-set: tree-element-set ',' tree-element Left

						s += " " + node.m__coma_.Emit (depth - 1);
						s += " " + node.m_tree_element.Emit (depth - 1);
						break;
					}
					return null;
				});
			return s.Trim ();
		}

		//
		// emit production tree node associated with any production of syntax rule tree-element-set
		//

		public override string EmitProductionTree (int depth)
		{
			return (depth != 0) ?
				(string) Iterate ("", (node, appData) =>
				{
					string str = "tree_element_set (" + (string) appData;
					switch ((production_kind) node.kind)
					{
					case production_kind.g_tree_element_set_1:
						// emit syntax tree node associated with production
						// tree-element-set: tree-element

						str += node.m_tree_element.EmitProductionTree (depth - 1);
						break;
					case production_kind.g_tree_element_set_2:
						// emit syntax tree node associated with production
						// tree-element-set: tree-element-set ',' tree-element Left

						str += ' ';
						str += "_coma_";
						str += ' ';
						str += node.m_tree_element.EmitProductionTree (depth - 1);
						break;
					}
					return str + ")";
				}) : "";
		}
		#endregion
		#region Syntax tree traversal,  reperrent, init and iterator methods associated with the syntax rule tree-element-set

		//
		// traverse sub-tree rooted in this node using selected syntax tree walker
		//

		public override void InvokeTraverse (SyntaxTreeWalkerCore syntaxTreeWalker) => ((SyntaxTreeWalker) syntaxTreeWalker).Traverse (this);
		public override void InvokeTraverseCommon (SyntaxTreeWalkerCore syntaxTreeWalker) => ((SyntaxTreeWalker) syntaxTreeWalker).TraverseCommon (this);

		//
		// traverse syntax tree node associated with any production of syntax rule tree-element-set with specified delegate
		//

		public override void Visit (Func<SyntaxTreeCallbackReason, int, SyntaxTreeBase, object, bool> visitor, object appData)
		{
			if (visitor (SyntaxTreeCallbackReason.IterationPrologueCallbackReason, (int) this.kind, this, appData))
				Iterate (null, (node, data) =>
				{
					tree_element_set.production_kind kind = (tree_element_set.production_kind) node.kind;
					if (visitor (SyntaxTreeCallbackReason.TraversalPrologueCallbackReason, (int) kind, node, appData))
						switch (kind)
						{
						case tree_element_set.production_kind.g_tree_element_set_1:
							// traverse syntax tree node associated with production
							// tree-element-set: tree-element

							node.m_tree_element.Visit (visitor, appData);
							break;
						case tree_element_set.production_kind.g_tree_element_set_2:
							// traverse syntax tree node associated with production
							// tree-element-set: tree-element-set ',' tree-element Left

							node.m__coma_.Visit (visitor, appData);
							node.m_tree_element.Visit (visitor, appData);
							break;
						}
					visitor (SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason, (int) kind, node, appData);
					return null;
				});
			visitor (SyntaxTreeCallbackReason.IterationEpilogueCallbackReason, (int) this.kind, this, appData);
		}

		// reparent sub-tree

		public override void reparent (SyntaxTreeBase parent)
		{
			for (tree_element_set node = this; node != null; node = node.m_tree_element_set)
			{
				node.parent = parent;
				node.m_tree_element?.reparent (parent);
				node.m__coma_?.reparent (parent);
				parent = node;
			}
		}

		//
		// initialize object associated with syntax rule tree-element-set
		//

		public void _init ()
		{
			m_tree_element = null;
			m_tree_element_set = null;
			m__coma_ = null;
		}

		public delegate object IteratorDelegate (tree_element_set p_tree_element_set, object appData);

		public object Iterate (object appData, IteratorDelegate iteratorDelegate)
		{
			tree_element_set p_tree_element_set;
			for (p_tree_element_set = this; p_tree_element_set.m_tree_element_set != null; p_tree_element_set = p_tree_element_set.m_tree_element_set);
			for (SyntaxTreeBase parent = p_tree_element_set; (parent != null) && (parent is tree_element_set); parent = parent.parent)
				appData = iteratorDelegate ((tree_element_set) parent, appData);
			return appData;
		}
		#endregion
		#region Fields and properties associated with the syntax rule tree-element-set

		// counter of all nodes associated with syntax rule tree-element-set
		public static int g_counter;

		// objects associated with terminal and non-terminal symbols within production(s) of syntax rule tree-element-set
		[DataMember (Name = "m_tree_element")] public tree_element m_tree_element { get; private set; }
		[DataMember (Name = "m_tree_element_set")] public tree_element_set m_tree_element_set { get; private set; }
		[DataMember (Name = "m__coma_")] public SyntaxTreeToken m__coma_ { get; private set; }
		#endregion

	};

	public partial class SyntaxTreeWalker
	{
		#region delegates and events associated with the syntax rule tree-element-set

		// delegate function (callback) prototype associated with syntax rule tree-element-set
		public delegate bool tree_element_set_Callback (SyntaxTreeCallbackReason reason, tree_element_set.production_kind kind, tree_element_set p_tree_element_set);

		// event associated with syntax rule tree-element-set
		public event tree_element_set_Callback tree_element_set_Event;

		// event trigger associated with syntax rule tree-element-set
		public bool Raise_tree_element_set_Event (SyntaxTreeCallbackReason reason, tree_element_set.production_kind kind, tree_element_set p_tree_element_set)
		{
			bool? status = tree_element_set_Event?.Invoke (reason, kind, p_tree_element_set);
			return (status == null) || status.Value;
		}
		#endregion
		#region syntax tree traversal methods associated with the syntax rule tree-element-set
		//
		// traverse syntax tree node associated with any production of syntax rule tree-element-set
		//

		public void Traverse (tree_element_set p_tree_element_set)
		{
			if (p_tree_element_set.isLocked())
				return;
			if (Raise_tree_element_set_Event (SyntaxTreeCallbackReason.IterationPrologueCallbackReason, (tree_element_set.production_kind) p_tree_element_set.kind, p_tree_element_set))
				p_tree_element_set.Iterate (null, (node, appData) =>
				{
					if (node.isLocked())
						return null;
					node.dolock();
					tree_element_set.production_kind kind = (tree_element_set.production_kind) node.kind;
					node.turn_reset ();
					if (Raise_tree_element_set_Event (SyntaxTreeCallbackReason.TraversalPrologueCallbackReason, kind, node))
						switch (kind)
						{
							case tree_element_set.production_kind.g_tree_element_set_1:
								// traverse syntax tree node associated with production
								// tree-element-set: tree-element

								if (Raise_tree_element_set_Event (SyntaxTreeCallbackReason.TraversalMidTermCallbackReason, kind, node))
									Traverse (node.m_tree_element);
								node.turn_inc ();
								Raise_tree_element_set_Event (SyntaxTreeCallbackReason.TraversalMidTermCallbackReason, kind, node);
							break;
							case tree_element_set.production_kind.g_tree_element_set_2:
								// traverse syntax tree node associated with production
								// tree-element-set: tree-element-set ',' tree-element Left

								if (Raise_tree_element_set_Event (SyntaxTreeCallbackReason.TraversalMidTermCallbackReason, kind, node))
									;
								node.turn_inc ();
								if (Raise_tree_element_set_Event (SyntaxTreeCallbackReason.TraversalMidTermCallbackReason, kind, node))
									Traverse (node.m_tree_element);
								node.turn_inc ();
								Raise_tree_element_set_Event (SyntaxTreeCallbackReason.TraversalMidTermCallbackReason, kind, node);
							break;
						}
					Raise_tree_element_set_Event (SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason, kind, node);
					node.unlock();
					return null;
				});
			Raise_tree_element_set_Event (SyntaxTreeCallbackReason.IterationEpilogueCallbackReason, (tree_element_set.production_kind) p_tree_element_set.kind, p_tree_element_set);
		}

		//
		// traverse syntax tree node associated with any production of syntax rule tree-element-set
		//

		public void TraverseCommon (tree_element_set p_tree_element_set)
		{
			if (Raise_Common_Event (SyntaxTreeCallbackReason.IterationPrologueCallbackReason, (int) p_tree_element_set.kind, p_tree_element_set))
				p_tree_element_set.Iterate (null, (node, appData) =>
				{
					tree_element_set.production_kind kind = (tree_element_set.production_kind) node.kind;
					if (Raise_Common_Event (SyntaxTreeCallbackReason.TraversalPrologueCallbackReason, (int) kind, node))
						switch (kind)
						{
						case tree_element_set.production_kind.g_tree_element_set_1:
							// traverse syntax tree node associated with production
							// tree-element-set: tree-element

							TraverseCommon (node.m_tree_element);
							break;
						case tree_element_set.production_kind.g_tree_element_set_2:
							// traverse syntax tree node associated with production
							// tree-element-set: tree-element-set ',' tree-element Left

							TraverseCommon (node.m__coma_);
							TraverseCommon (node.m_tree_element);
							break;
						}
					Raise_Common_Event (SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason, (int) kind, node);
					return null;
				});
			Raise_Common_Event (SyntaxTreeCallbackReason.IterationEpilogueCallbackReason, (int) p_tree_element_set.kind, p_tree_element_set);
		}
	};
		#endregion

	public partial class SyntaxTreeBuilder : SyntaxTreeWalker
	{
		#region syntax tree builder methods associated with the syntax rule tree-element-set

		//
		// create syntax tree node associated with production
		// tree-element-set: tree-element

		//

		public tree_element_set tree_element_set_1 (tree_element p_tree_element)
		{
			tree_element_set p_tree_element_set_ref = new tree_element_set(p_tree_element);
			p_tree_element.parent = p_tree_element_set_ref;
			Raise_tree_element_set_Event (SyntaxTreeCallbackReason.BuilderCallbackReason, tree_element_set.production_kind.g_tree_element_set_1, p_tree_element_set_ref);
			return p_tree_element_set_ref;
		}

		//
		// create syntax tree node associated with production
		// tree-element-set: tree-element-set ',' tree-element Left

		//

		public tree_element_set tree_element_set_2 (tree_element_set p_tree_element_set, SyntaxTreeToken p_token, tree_element p_tree_element)
		{
			tree_element_set p_tree_element_set_ref = new tree_element_set(p_tree_element_set, p_token, p_tree_element);
			p_tree_element_set.parent = p_tree_element_set_ref;
			p_token.parent = p_tree_element_set_ref;
			p_tree_element.parent = p_tree_element_set_ref;
			Raise_tree_element_set_Event (SyntaxTreeCallbackReason.BuilderCallbackReason, tree_element_set.production_kind.g_tree_element_set_2, p_tree_element_set_ref);
			return p_tree_element_set_ref;
		}
		#endregion
	};
}
