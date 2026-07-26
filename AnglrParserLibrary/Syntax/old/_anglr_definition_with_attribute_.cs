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
	// class associated with syntax rule <anglr definition with attribute>
	//

	public class	_anglr_definition_with_attribute_ : SyntaxTreeBase
	{
		#region enumerated production(s) of syntax rule <anglr definition with attribute>
		public enum production_kind : ushort	// enumerated production(s) of syntax rule <anglr definition with attribute>
		{
			g__anglr_definition_with_attribute__1 = 1,	// <attribute list optional> <anglr definition>

		};
		#endregion
		#region production markers associated with the syntax rule <anglr definition with attribute>

		// markers associated with production: <anglr definition with attribute> -> <attribute list optional> <anglr definition>

		public enum production_marker_1 : ushort
		{
			m__attribute_list_optional_,
			m__anglr_definition_,
			final
		};
		#endregion
		#region Constructors and destructors associated with the syntax rule <anglr definition with attribute>

		// Constructor declaration(s) associated with production(s) of syntax rule <anglr definition with attribute>

		//
		// Constructor associated with the following production(s)
		// <anglr definition with attribute> -> <attribute list optional> <anglr definition>

		//

		public _anglr_definition_with_attribute_ (_attribute_list_optional_ p__attribute_list_optional_, _anglr_definition_ p__anglr_definition_) : base ((uint) ProductionID.__anglr_definition_with_attribute__ID, (uint) production_kind.g__anglr_definition_with_attribute__1)
		{
			++g_counter;
			_init ();
			children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 2);
			children[0] = m__attribute_list_optional_ = p__attribute_list_optional_;
			children[1] = m__anglr_definition_ = p__anglr_definition_;
		}

		// Copy constructor

		public _anglr_definition_with_attribute_ (_anglr_definition_with_attribute_ p__anglr_definition_with_attribute_) : base (p__anglr_definition_with_attribute_.id, p__anglr_definition_with_attribute_.kind)
		{
			++g_counter;
			_init ();
			switch ((production_kind) p__anglr_definition_with_attribute_.kind)
			{
				case production_kind.g__anglr_definition_with_attribute__1:
					children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 2);
					if ((children [0] = m__attribute_list_optional_ = (p__anglr_definition_with_attribute_.m__attribute_list_optional_ != null) ? new _attribute_list_optional_ (p__anglr_definition_with_attribute_.m__attribute_list_optional_) : null) != null) m__attribute_list_optional_.parent = this;
					if ((children [1] = m__anglr_definition_ = (p__anglr_definition_with_attribute_.m__anglr_definition_ != null) ? new _anglr_definition_ (p__anglr_definition_with_attribute_.m__anglr_definition_) : null) != null) m__anglr_definition_.parent = this;
					break;
				default:
				{
					string[] args = new string[] { "_anglr_definition_with_attribute_" };
					throw new SyntaxTreeError (SyntaxTreeError.InvalidKindError, args);
				}
			}
		}

		// Clone node - wrapper of copy constructor

		public override SyntaxTreeBase Clone () { return (SyntaxTreeBase) new _anglr_definition_with_attribute_ (this); }

		// Destructor

		public new void Dispose ()
		{
			base.Dispose ();
			--g_counter;

			m__attribute_list_optional_?.Dispose ();
			m__anglr_definition_?.Dispose ();

			_init ();
		}
		#endregion
		#region Content changing and inclusion checking methods associated with the syntax rule <anglr definition with attribute>

		// Content changing function(s) associated with production(s) of syntax rule <anglr definition with attribute>

		//
		// Content changing function associated with following production(s)
		// <anglr definition with attribute> -> <attribute list optional> <anglr definition>

		//

		public void change(_attribute_list_optional_ p__attribute_list_optional_, _anglr_definition_ p__anglr_definition_)
		{
			_init ();
			this.kind = (uint) production_kind.g__anglr_definition_with_attribute__1;
			children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 2);
			children [0] = m__attribute_list_optional_ = p__attribute_list_optional_;
			children [1] = m__anglr_definition_ = p__anglr_definition_;
			reparent (parent);
		}

		//
		// check if node 'element' is included within syntax tree associated with syntax rule <anglr definition with attribute>
		//

		public new bool checkInclusion (SyntaxTreeBase element)
		{
			return
				(element == this) ||
				(m__attribute_list_optional_ != null) && m__attribute_list_optional_.checkInclusion (element) ||
				(m__anglr_definition_ != null) && m__anglr_definition_.checkInclusion (element) ||
				false;
		}
		#endregion
		#region Syntax tree emit methods associated with the syntax rule <anglr definition with attribute>

		//
		// emit production tree node associated with any production of syntax rule <anglr definition with attribute>
		//

		public override string Emit (int depth)
		{
			string s = "";
			if (depth != 0)
			switch ((production_kind) kind)
			{
				case production_kind.g__anglr_definition_with_attribute__1:
					// emit syntax tree node associated with production
					// <anglr definition with attribute>: <attribute list optional> <anglr definition>

					s += m__attribute_list_optional_.Emit (depth - 1);
					s += " ";
					s += m__anglr_definition_.Emit (depth - 1);
				break;
			}
			return s.Trim ();
		}

		//
		// emit production tree node associated with any production of syntax rule <anglr definition with attribute>
		//

		public override string EmitProductionTree (int depth)
		{
			string s = "_anglr_definition_with_attribute_ (";
			if (depth != 0)
				switch ((production_kind) kind)
				{
				case production_kind.g__anglr_definition_with_attribute__1:
					// emit syntax tree node associated with production
					// <anglr definition with attribute>: <attribute list optional> <anglr definition>

					s += m__attribute_list_optional_.EmitProductionTree (depth - 1);
					s += ' ';
					s += m__anglr_definition_.EmitProductionTree (depth - 1);
				break;
			}
			s += ")";
			return s;
		}
		#endregion
		#region Syntax tree traversal,  reperrent, init and iterator methods associated with the syntax rule <anglr definition with attribute>

		//
		// traverse sub-tree rooted in this node using selected syntax tree walker
		//

		public override void InvokeTraverse (SyntaxTreeWalkerCore syntaxTreeWalker) => ((SyntaxTreeWalker) syntaxTreeWalker).Traverse (this);
		public override void InvokeTraverseCommon (SyntaxTreeWalkerCore syntaxTreeWalker) => ((SyntaxTreeWalker) syntaxTreeWalker).TraverseCommon (this);

		// reparent sub-tree

		public override void reparent (SyntaxTreeBase parent)
		{
			this.parent = parent;

			m__attribute_list_optional_?.reparent (this);
			m__anglr_definition_?.reparent (this);
		}

		//
		// initialize object associated with syntax rule <anglr definition with attribute>
		//

		public void _init ()
		{
			m__attribute_list_optional_ = null;
			m__anglr_definition_ = null;
		}
		#endregion
		#region Fields and properties associated with the syntax rule <anglr definition with attribute>

		// counter of all nodes associated with syntax rule <anglr definition with attribute>
		public static int g_counter;

		// objects associated with terminal and non-terminal symbols within production(s) of syntax rule <anglr definition with attribute>
		public _attribute_list_optional_ m__attribute_list_optional_ { get; private set; }
		public _anglr_definition_ m__anglr_definition_ { get; private set; }
		#endregion

	};

	public partial class SyntaxTreeWalker
	{
		#region delegates and events associated with the syntax rule <anglr definition with attribute>

		// delegate function (callback) prototype associated with syntax rule <anglr definition with attribute>
		public delegate bool _anglr_definition_with_attribute__Callback (SyntaxTreeCallbackReason reason, _anglr_definition_with_attribute_.production_kind kind, _anglr_definition_with_attribute_ p__anglr_definition_with_attribute_);

		// event associated with syntax rule <anglr definition with attribute>
		public event _anglr_definition_with_attribute__Callback _anglr_definition_with_attribute__Event;

		// event trigger associated with syntax rule <anglr definition with attribute>
		public bool Raise__anglr_definition_with_attribute__Event (SyntaxTreeCallbackReason reason, _anglr_definition_with_attribute_.production_kind kind, _anglr_definition_with_attribute_ p__anglr_definition_with_attribute_)
		{
			bool? status = _anglr_definition_with_attribute__Event?.Invoke (reason, kind, p__anglr_definition_with_attribute_);
			return (status == null) || status.Value;
		}
		#endregion
		#region syntax tree traversal methods associated with the syntax rule <anglr definition with attribute>
		//
		// traverse syntax tree node associated with any production of syntax rule <anglr definition with attribute>
		//

		public void Traverse (_anglr_definition_with_attribute_ p__anglr_definition_with_attribute_)
		{
			if (p__anglr_definition_with_attribute_.isLocked())
				return;
			p__anglr_definition_with_attribute_.dolock();
			_anglr_definition_with_attribute_.production_kind kind = (_anglr_definition_with_attribute_.production_kind) p__anglr_definition_with_attribute_.kind;
			p__anglr_definition_with_attribute_.turn_reset ();
			if (Raise__anglr_definition_with_attribute__Event (SyntaxTreeCallbackReason.TraversalPrologueCallbackReason, kind, p__anglr_definition_with_attribute_))
			switch (kind)
			{
				case _anglr_definition_with_attribute_.production_kind.g__anglr_definition_with_attribute__1:
					// traverse syntax tree node associated with production
					// <anglr definition with attribute>: <attribute list optional> <anglr definition>

					if (Raise__anglr_definition_with_attribute__Event (SyntaxTreeCallbackReason.TraversalMidTermCallbackReason, kind, p__anglr_definition_with_attribute_))
						Traverse (p__anglr_definition_with_attribute_.m__attribute_list_optional_);
					p__anglr_definition_with_attribute_.turn_inc ();
					if (Raise__anglr_definition_with_attribute__Event (SyntaxTreeCallbackReason.TraversalMidTermCallbackReason, kind, p__anglr_definition_with_attribute_))
						Traverse (p__anglr_definition_with_attribute_.m__anglr_definition_);
					p__anglr_definition_with_attribute_.turn_inc ();
					Raise__anglr_definition_with_attribute__Event (SyntaxTreeCallbackReason.TraversalMidTermCallbackReason, kind, p__anglr_definition_with_attribute_);
				break;
			}
			Raise__anglr_definition_with_attribute__Event (SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason, kind, p__anglr_definition_with_attribute_);
			p__anglr_definition_with_attribute_.unlock();
		}

		//
		// traverse syntax tree node associated with any production of syntax rule <anglr definition with attribute>
		//

		public void TraverseCommon (_anglr_definition_with_attribute_ p__anglr_definition_with_attribute_)
		{
			_anglr_definition_with_attribute_.production_kind kind = (_anglr_definition_with_attribute_.production_kind) p__anglr_definition_with_attribute_.kind;
			if (Raise_Common_Event (SyntaxTreeCallbackReason.TraversalPrologueCallbackReason, (int) kind, p__anglr_definition_with_attribute_))
			switch (kind)
			{
				case _anglr_definition_with_attribute_.production_kind.g__anglr_definition_with_attribute__1:
					// traverse syntax tree node associated with production
					// <anglr definition with attribute>: <attribute list optional> <anglr definition>

						TraverseCommon (p__anglr_definition_with_attribute_.m__attribute_list_optional_);
						TraverseCommon (p__anglr_definition_with_attribute_.m__anglr_definition_);
				break;
			}
			Raise_Common_Event (SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason, (int) kind, p__anglr_definition_with_attribute_);
		}
	};
		#endregion

	public partial class SyntaxTreeBuilder : SyntaxTreeWalker
	{
		#region syntax tree builder methods associated with the syntax rule <anglr definition with attribute>

		//
		// create syntax tree node associated with production
		// <anglr definition with attribute>: <attribute list optional> <anglr definition>

		//

		public _anglr_definition_with_attribute_ _anglr_definition_with_attribute__1 (_attribute_list_optional_ p__attribute_list_optional_, _anglr_definition_ p__anglr_definition_)
		{
			_anglr_definition_with_attribute_ p__anglr_definition_with_attribute__ref = new _anglr_definition_with_attribute_(p__attribute_list_optional_, p__anglr_definition_);
			p__attribute_list_optional_.parent = p__anglr_definition_with_attribute__ref;
			p__anglr_definition_.parent = p__anglr_definition_with_attribute__ref;
			Raise__anglr_definition_with_attribute__Event (SyntaxTreeCallbackReason.BuilderCallbackReason, _anglr_definition_with_attribute_.production_kind.g__anglr_definition_with_attribute__1, p__anglr_definition_with_attribute__ref);
			return p__anglr_definition_with_attribute__ref;
		}
		#endregion
	};
}
