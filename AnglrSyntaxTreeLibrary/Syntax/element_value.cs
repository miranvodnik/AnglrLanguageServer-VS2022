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
	// class associated with syntax rule element-value
	//

	[DataContract]
	public class	element_value : SyntaxTreeBase
	{
		#region enumerated production(s) of syntax rule element-value
		public enum production_kind : ushort	// enumerated production(s) of syntax rule element-value
		{
			g_element_value_1 = 1,	// <cstring>

			g_element_value_2 = 2,	// <number>

			g_element_value_3 = 3,	// <null>

			g_element_value_4 = 4,	// tree

		};
		#endregion
		#region production markers associated with the syntax rule element-value

		// markers associated with production: element-value -> <cstring>

		public enum production_marker_1 : ushort
		{
			final
		};

		// markers associated with production: element-value -> <number>

		public enum production_marker_2 : ushort
		{
			final
		};

		// markers associated with production: element-value -> <null>

		public enum production_marker_3 : ushort
		{
			final
		};

		// markers associated with production: element-value -> tree

		public enum production_marker_4 : ushort
		{
			m_tree,
			final
		};
		#endregion
		#region Constructors and destructors associated with the syntax rule element-value

		// Constructor declaration(s) associated with production(s) of syntax rule element-value

		//
		// Constructor associated with the following production(s)
		// element-value -> <cstring>

		// element-value -> <number>

		// element-value -> <null>

		//

		public element_value (SyntaxTreeToken p_token, int kind) : base ((uint) ProductionID._element_value_ID, (uint) kind)
		{
			++g_counter;
			_init ();
			switch ((production_kind) this.kind)
			{
			case production_kind.g_element_value_1:
				children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 1);
				children[0] = m__cstring_ = p_token;
				break;
			case production_kind.g_element_value_2:
				children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 1);
				children[0] = m__number_ = p_token;
				break;
			case production_kind.g_element_value_3:
				children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 1);
				children[0] = m__null_ = p_token;
				break;
			default:
				{
					string[] args = new string[] { "element_value" };
					throw new SyntaxTreeError (SyntaxTreeError.InvalidKindError, args);
				}
			}
		}

		//
		// Constructor associated with the following production(s)
		// element-value -> tree

		//

		public element_value (tree p_tree) : base ((uint) ProductionID._element_value_ID, (uint) production_kind.g_element_value_4)
		{
			++g_counter;
			_init ();
			children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 1);
			children[0] = m_tree = p_tree;
		}

		public element_value () { }

		// Copy constructor

		public element_value (element_value p_element_value) : base (p_element_value.id, p_element_value.kind)
		{
			++g_counter;
			_init ();
			appInfo = p_element_value.appInfo;
			switch ((production_kind) p_element_value.kind)
			{
				case production_kind.g_element_value_1:
					children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 1);
					if ((children [0] = m__cstring_ = (p_element_value.m__cstring_ != null) ? new SyntaxTreeToken (p_element_value.m__cstring_) : null) != null) m__cstring_.parent = this;
					break;
				case production_kind.g_element_value_2:
					children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 1);
					if ((children [0] = m__number_ = (p_element_value.m__number_ != null) ? new SyntaxTreeToken (p_element_value.m__number_) : null) != null) m__number_.parent = this;
					break;
				case production_kind.g_element_value_3:
					children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 1);
					if ((children [0] = m__null_ = (p_element_value.m__null_ != null) ? new SyntaxTreeToken (p_element_value.m__null_) : null) != null) m__null_.parent = this;
					break;
				case production_kind.g_element_value_4:
					children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 1);
					if ((children [0] = m_tree = (p_element_value.m_tree != null) ? new tree (p_element_value.m_tree) : null) != null) m_tree.parent = this;
					break;
				default:
				{
					string[] args = new string[] { "element_value" };
					throw new SyntaxTreeError (SyntaxTreeError.InvalidKindError, args);
				}
			}
		}

		// Clone node - wrapper of copy constructor

		public override SyntaxTreeBase Clone () { return (SyntaxTreeBase) new element_value (this); }

		// Destructor

		public new void Dispose ()
		{
			base.Dispose ();
			--g_counter;

			m__cstring_?.Dispose ();
			m__number_?.Dispose ();
			m__null_?.Dispose ();
			m_tree?.Dispose ();

			_init ();
		}
		#endregion
		#region Content changing and inclusion checking methods associated with the syntax rule element-value

		// Content changing function(s) associated with production(s) of syntax rule element-value

		//
		// Content changing function associated with following production(s)
		// element-value -> <cstring>

		// element-value -> <number>

		// element-value -> <null>

		//

		public void change(SyntaxTreeToken p_token, int kind)
		{
			_init ();
			this.kind = (uint) kind;
			switch ((production_kind) this.kind)
			{
			case production_kind.g_element_value_1:
				children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 1);
				children [0] = m__cstring_ = p_token;
				break;
			case production_kind.g_element_value_2:
				children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 1);
				children [0] = m__number_ = p_token;
				break;
			case production_kind.g_element_value_3:
				children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 1);
				children [0] = m__null_ = p_token;
				break;
			default:
				{
					string[] args = new string[] { "element_value" };
					throw new SyntaxTreeError (SyntaxTreeError.InvalidKindError, args);
				}
			}
			reparent (parent);
		}

		//
		// Content changing function associated with following production(s)
		// element-value -> tree

		//

		public void change(tree p_tree)
		{
			_init ();
			this.kind = (uint) production_kind.g_element_value_4;
			children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 1);
			children [0] = m_tree = p_tree;
			reparent (parent);
		}

		//
		// check if node 'element' is included within syntax tree associated with syntax rule element-value
		//

		public new bool checkInclusion (SyntaxTreeBase element)
		{
			return
				(element == this) ||
				(m__cstring_ != null) && m__cstring_.checkInclusion (element) ||
				(m__number_ != null) && m__number_.checkInclusion (element) ||
				(m__null_ != null) && m__null_.checkInclusion (element) ||
				(m_tree != null) && m_tree.checkInclusion (element) ||
				false;
		}
		#endregion
		#region Syntax tree emit methods associated with the syntax rule element-value

		//
		// emit production tree node associated with any production of syntax rule element-value
		//

		public override string Emit (int depth)
		{
			string s = "";
			if (depth != 0)
			switch ((production_kind) kind)
			{
				case production_kind.g_element_value_1:
					// emit syntax tree node associated with production
					// element-value: <cstring>

					s += m__cstring_.Emit (depth - 1);
				break;
				case production_kind.g_element_value_2:
					// emit syntax tree node associated with production
					// element-value: <number>

					s += m__number_.Emit (depth - 1);
				break;
				case production_kind.g_element_value_3:
					// emit syntax tree node associated with production
					// element-value: <null>

					s += m__null_.Emit (depth - 1);
				break;
				case production_kind.g_element_value_4:
					// emit syntax tree node associated with production
					// element-value: tree

					s += m_tree.Emit (depth - 1);
				break;
			}
			return s.Trim ();
		}

		//
		// emit production tree node associated with any production of syntax rule element-value
		//

		public override string EmitProductionTree (int depth)
		{
			string s = "element_value (";
			if (depth != 0)
				switch ((production_kind) kind)
				{
				case production_kind.g_element_value_1:
					// emit syntax tree node associated with production
					// element-value: <cstring>

					s += "_cstring_";
				break;
				case production_kind.g_element_value_2:
					// emit syntax tree node associated with production
					// element-value: <number>

					s += "_number_";
				break;
				case production_kind.g_element_value_3:
					// emit syntax tree node associated with production
					// element-value: <null>

					s += "_null_";
				break;
				case production_kind.g_element_value_4:
					// emit syntax tree node associated with production
					// element-value: tree

					s += m_tree.EmitProductionTree (depth - 1);
				break;
			}
			s += ")";
			return s;
		}
		#endregion
		#region Syntax tree traversal,  reperrent, init and iterator methods associated with the syntax rule element-value

		//
		// traverse sub-tree rooted in this node using selected syntax tree walker
		//

		public override void InvokeTraverse (SyntaxTreeWalkerCore syntaxTreeWalker) => ((SyntaxTreeWalker) syntaxTreeWalker).Traverse (this);
		public override void InvokeTraverseCommon (SyntaxTreeWalkerCore syntaxTreeWalker) => ((SyntaxTreeWalker) syntaxTreeWalker).TraverseCommon (this);

		// reparent sub-tree

		public override void reparent (SyntaxTreeBase parent)
		{
			this.parent = parent;

			m__cstring_?.reparent (this);
			m__number_?.reparent (this);
			m__null_?.reparent (this);
			m_tree?.reparent (this);
		}

		//
		// initialize object associated with syntax rule element-value
		//

		public void _init ()
		{
			m__cstring_ = null;
			m__number_ = null;
			m__null_ = null;
			m_tree = null;
		}
		#endregion
		#region Fields and properties associated with the syntax rule element-value

		// counter of all nodes associated with syntax rule element-value
		public static int g_counter;

		// objects associated with terminal and non-terminal symbols within production(s) of syntax rule element-value
		[DataMember (Name = "m__cstring_")] public SyntaxTreeToken m__cstring_ { get; private set; }
		[DataMember (Name = "m__number_")] public SyntaxTreeToken m__number_ { get; private set; }
		[DataMember (Name = "m__null_")] public SyntaxTreeToken m__null_ { get; private set; }
		[DataMember (Name = "m_tree")] public tree m_tree { get; private set; }
		#endregion

	};

	public partial class SyntaxTreeWalker
	{
		#region delegates and events associated with the syntax rule element-value

		// delegate function (callback) prototype associated with syntax rule element-value
		public delegate bool element_value_Callback (SyntaxTreeCallbackReason reason, element_value.production_kind kind, element_value p_element_value);

		// event associated with syntax rule element-value
		public event element_value_Callback element_value_Event;

		// event trigger associated with syntax rule element-value
		public bool Raise_element_value_Event (SyntaxTreeCallbackReason reason, element_value.production_kind kind, element_value p_element_value)
		{
			bool? status = element_value_Event?.Invoke (reason, kind, p_element_value);
			return (status == null) || status.Value;
		}
		#endregion
		#region syntax tree traversal methods associated with the syntax rule element-value
		//
		// traverse syntax tree node associated with any production of syntax rule element-value
		//

		public void Traverse (element_value p_element_value)
		{
			if (p_element_value.isLocked())
				return;
			p_element_value.dolock();
			element_value.production_kind kind = (element_value.production_kind) p_element_value.kind;
			p_element_value.turn_reset ();
			if (Raise_element_value_Event (SyntaxTreeCallbackReason.TraversalPrologueCallbackReason, kind, p_element_value))
			switch (kind)
			{
				case element_value.production_kind.g_element_value_1:
					// traverse syntax tree node associated with production
					// element-value: <cstring>

				break;
				case element_value.production_kind.g_element_value_2:
					// traverse syntax tree node associated with production
					// element-value: <number>

				break;
				case element_value.production_kind.g_element_value_3:
					// traverse syntax tree node associated with production
					// element-value: <null>

				break;
				case element_value.production_kind.g_element_value_4:
					// traverse syntax tree node associated with production
					// element-value: tree

					if (Raise_element_value_Event (SyntaxTreeCallbackReason.TraversalMidTermCallbackReason, kind, p_element_value))
						Traverse (p_element_value.m_tree);
					p_element_value.turn_inc ();
					Raise_element_value_Event (SyntaxTreeCallbackReason.TraversalMidTermCallbackReason, kind, p_element_value);
				break;
			}
			Raise_element_value_Event (SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason, kind, p_element_value);
			p_element_value.unlock();
		}

		//
		// traverse syntax tree node associated with any production of syntax rule element-value
		//

		public void TraverseCommon (element_value p_element_value)
		{
			element_value.production_kind kind = (element_value.production_kind) p_element_value.kind;
			if (Raise_Common_Event (SyntaxTreeCallbackReason.TraversalPrologueCallbackReason, (int) kind, p_element_value))
			switch (kind)
			{
				case element_value.production_kind.g_element_value_1:
					// traverse syntax tree node associated with production
					// element-value: <cstring>

						TraverseCommon (p_element_value.m__cstring_);
				break;
				case element_value.production_kind.g_element_value_2:
					// traverse syntax tree node associated with production
					// element-value: <number>

						TraverseCommon (p_element_value.m__number_);
				break;
				case element_value.production_kind.g_element_value_3:
					// traverse syntax tree node associated with production
					// element-value: <null>

						TraverseCommon (p_element_value.m__null_);
				break;
				case element_value.production_kind.g_element_value_4:
					// traverse syntax tree node associated with production
					// element-value: tree

						TraverseCommon (p_element_value.m_tree);
				break;
			}
			Raise_Common_Event (SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason, (int) kind, p_element_value);
		}
	};
		#endregion

	public partial class SyntaxTreeBuilder : SyntaxTreeWalker
	{
		#region syntax tree builder methods associated with the syntax rule element-value

		//
		// create syntax tree node associated with production
		// element-value: <cstring>

		//

		public element_value element_value_1 (SyntaxTreeToken p_token)
		{
			element_value p_element_value_ref = new element_value(p_token, 1);
			p_token.parent = p_element_value_ref;
			Raise_element_value_Event (SyntaxTreeCallbackReason.BuilderCallbackReason, element_value.production_kind.g_element_value_1, p_element_value_ref);
			return p_element_value_ref;
		}

		//
		// create syntax tree node associated with production
		// element-value: <number>

		//

		public element_value element_value_2 (SyntaxTreeToken p_token)
		{
			element_value p_element_value_ref = new element_value(p_token, 2);
			p_token.parent = p_element_value_ref;
			Raise_element_value_Event (SyntaxTreeCallbackReason.BuilderCallbackReason, element_value.production_kind.g_element_value_2, p_element_value_ref);
			return p_element_value_ref;
		}

		//
		// create syntax tree node associated with production
		// element-value: <null>

		//

		public element_value element_value_3 (SyntaxTreeToken p_token)
		{
			element_value p_element_value_ref = new element_value(p_token, 3);
			p_token.parent = p_element_value_ref;
			Raise_element_value_Event (SyntaxTreeCallbackReason.BuilderCallbackReason, element_value.production_kind.g_element_value_3, p_element_value_ref);
			return p_element_value_ref;
		}

		//
		// create syntax tree node associated with production
		// element-value: tree

		//

		public element_value element_value_4 (tree p_tree)
		{
			element_value p_element_value_ref = new element_value(p_tree);
			p_tree.parent = p_element_value_ref;
			Raise_element_value_Event (SyntaxTreeCallbackReason.BuilderCallbackReason, element_value.production_kind.g_element_value_4, p_element_value_ref);
			return p_element_value_ref;
		}
		#endregion
	};
}
