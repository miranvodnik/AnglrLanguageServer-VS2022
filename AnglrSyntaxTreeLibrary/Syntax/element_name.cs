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
	// class associated with syntax rule element-name
	//

	[DataContract]
	public class	element_name : SyntaxTreeBase
	{
		#region enumerated production(s) of syntax rule element-name
		public enum production_kind : ushort	// enumerated production(s) of syntax rule element-name
		{
			g_element_name_1 = 1,	// <cstring>

		};
		#endregion
		#region production markers associated with the syntax rule element-name

		// markers associated with production: element-name -> <cstring>

		public enum production_marker_1 : ushort
		{
			final
		};
		#endregion
		#region Constructors and destructors associated with the syntax rule element-name

		// Constructor declaration(s) associated with production(s) of syntax rule element-name

		//
		// Constructor associated with the following production(s)
		// element-name -> <cstring>

		//

		public element_name (SyntaxTreeToken p_token) : base ((uint) ProductionID._element_name_ID, (uint) production_kind.g_element_name_1)
		{
			++g_counter;
			_init ();
			children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 1);
			children[0] = m__cstring_ = p_token;
		}

		public element_name () { }

		// Copy constructor

		public element_name (element_name p_element_name) : base (p_element_name.id, p_element_name.kind)
		{
			++g_counter;
			_init ();
			appInfo = p_element_name.appInfo;
			switch ((production_kind) p_element_name.kind)
			{
				case production_kind.g_element_name_1:
					children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 1);
					if ((children [0] = m__cstring_ = (p_element_name.m__cstring_ != null) ? new SyntaxTreeToken (p_element_name.m__cstring_) : null) != null) m__cstring_.parent = this;
					break;
				default:
				{
					string[] args = new string[] { "element_name" };
					throw new SyntaxTreeError (SyntaxTreeError.InvalidKindError, args);
				}
			}
		}

		// Clone node - wrapper of copy constructor

		public override SyntaxTreeBase Clone () { return (SyntaxTreeBase) new element_name (this); }

		// Destructor

		public new void Dispose ()
		{
			base.Dispose ();
			--g_counter;

			m__cstring_?.Dispose ();

			_init ();
		}
		#endregion
		#region Content changing and inclusion checking methods associated with the syntax rule element-name

		// Content changing function(s) associated with production(s) of syntax rule element-name

		//
		// Content changing function associated with following production(s)
		// element-name -> <cstring>

		//

		public void change(SyntaxTreeToken p_token)
		{
			_init ();
			this.kind = (uint) production_kind.g_element_name_1;
			children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 1);
			children [0] = m__cstring_ = p_token;
			reparent (parent);
		}

		//
		// check if node 'element' is included within syntax tree associated with syntax rule element-name
		//

		public new bool checkInclusion (SyntaxTreeBase element)
		{
			return
				(element == this) ||
				(m__cstring_ != null) && m__cstring_.checkInclusion (element) ||
				false;
		}
		#endregion
		#region Syntax tree emit methods associated with the syntax rule element-name

		//
		// emit production tree node associated with any production of syntax rule element-name
		//

		public override string Emit (int depth)
		{
			string s = "";
			if (depth != 0)
			switch ((production_kind) kind)
			{
				case production_kind.g_element_name_1:
					// emit syntax tree node associated with production
					// element-name: <cstring>

					s += m__cstring_.Emit (depth - 1);
				break;
			}
			return s.Trim ();
		}

		//
		// emit production tree node associated with any production of syntax rule element-name
		//

		public override string EmitProductionTree (int depth)
		{
			string s = "element_name (";
			if (depth != 0)
				switch ((production_kind) kind)
				{
				case production_kind.g_element_name_1:
					// emit syntax tree node associated with production
					// element-name: <cstring>

					s += "_cstring_";
				break;
			}
			s += ")";
			return s;
		}
		#endregion
		#region Syntax tree traversal,  reperrent, init and iterator methods associated with the syntax rule element-name

		//
		// traverse sub-tree rooted in this node using selected syntax tree walker
		//

		public override void InvokeTraverse (SyntaxTreeWalkerCore syntaxTreeWalker) => ((SyntaxTreeWalker) syntaxTreeWalker).Traverse (this);
		public override void InvokeTraverseCommon (SyntaxTreeWalkerCore syntaxTreeWalker) => ((SyntaxTreeWalker) syntaxTreeWalker).TraverseCommon (this);

		//
		// traverse syntax tree node associated with any production of syntax rule element-name with specified delegate
		//

		public override void Visit (Func<SyntaxTreeCallbackReason, int, SyntaxTreeBase, object, bool> visitor, object appData)
		{
			element_name.production_kind kind = (element_name.production_kind) this.kind;
			if (visitor (SyntaxTreeCallbackReason.TraversalPrologueCallbackReason, (int) kind, this, appData))
			switch (kind)
			{
				case element_name.production_kind.g_element_name_1:
					// traverse syntax tree node associated with production
					// element-name: <cstring>

						m__cstring_.Visit (visitor, appData);
				break;
			}
			visitor (SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason, (int) kind, this, appData);
		}

		// reparent sub-tree

		public override void reparent (SyntaxTreeBase parent)
		{
			this.parent = parent;

			m__cstring_?.reparent (this);
		}

		//
		// initialize object associated with syntax rule element-name
		//

		public void _init ()
		{
			m__cstring_ = null;
		}
		#endregion
		#region Fields and properties associated with the syntax rule element-name

		// counter of all nodes associated with syntax rule element-name
		public static int g_counter;

		// objects associated with terminal and non-terminal symbols within production(s) of syntax rule element-name
		[DataMember (Name = "m__cstring_")] public SyntaxTreeToken m__cstring_ { get; private set; }
		#endregion

	};

	public partial class SyntaxTreeWalker
	{
		#region delegates and events associated with the syntax rule element-name

		// delegate function (callback) prototype associated with syntax rule element-name
		public delegate bool element_name_Callback (SyntaxTreeCallbackReason reason, element_name.production_kind kind, element_name p_element_name);

		// event associated with syntax rule element-name
		public event element_name_Callback element_name_Event;

		// event trigger associated with syntax rule element-name
		public bool Raise_element_name_Event (SyntaxTreeCallbackReason reason, element_name.production_kind kind, element_name p_element_name)
		{
			bool? status = element_name_Event?.Invoke (reason, kind, p_element_name);
			return (status == null) || status.Value;
		}
		#endregion
		#region syntax tree traversal methods associated with the syntax rule element-name
		//
		// traverse syntax tree node associated with any production of syntax rule element-name
		//

		public void Traverse (element_name p_element_name)
		{
			if (p_element_name.isLocked())
				return;
			p_element_name.dolock();
			element_name.production_kind kind = (element_name.production_kind) p_element_name.kind;
			p_element_name.turn_reset ();
			if (Raise_element_name_Event (SyntaxTreeCallbackReason.TraversalPrologueCallbackReason, kind, p_element_name))
			switch (kind)
			{
				case element_name.production_kind.g_element_name_1:
					// traverse syntax tree node associated with production
					// element-name: <cstring>

				break;
			}
			Raise_element_name_Event (SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason, kind, p_element_name);
			p_element_name.unlock();
		}

		//
		// traverse syntax tree node associated with any production of syntax rule element-name
		//

		public void TraverseCommon (element_name p_element_name)
		{
			element_name.production_kind kind = (element_name.production_kind) p_element_name.kind;
			if (Raise_Common_Event (SyntaxTreeCallbackReason.TraversalPrologueCallbackReason, (int) kind, p_element_name))
			switch (kind)
			{
				case element_name.production_kind.g_element_name_1:
					// traverse syntax tree node associated with production
					// element-name: <cstring>

						TraverseCommon (p_element_name.m__cstring_);
				break;
			}
			Raise_Common_Event (SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason, (int) kind, p_element_name);
		}
	};
		#endregion

	public partial class SyntaxTreeBuilder : SyntaxTreeWalker
	{
		#region syntax tree builder methods associated with the syntax rule element-name

		//
		// create syntax tree node associated with production
		// element-name: <cstring>

		//

		public element_name element_name_1 (SyntaxTreeToken p_token)
		{
			element_name p_element_name_ref = new element_name(p_token);
			p_token.parent = p_element_name_ref;
			Raise_element_name_Event (SyntaxTreeCallbackReason.BuilderCallbackReason, element_name.production_kind.g_element_name_1, p_element_name_ref);
			return p_element_name_ref;
		}
		#endregion
	};
}
