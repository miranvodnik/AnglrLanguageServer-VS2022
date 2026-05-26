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
	// class associated with syntax rule <cardinality>
	//

	public class	_cardinality_ : SyntaxTreeBase
	{
		#region enumerated production(s) of syntax rule <cardinality>
		public enum production_kind : ushort	// enumerated production(s) of syntax rule <cardinality>
		{
			g__cardinality__1 = 1,	// '?'

			g__cardinality__2 = 2,	// '+'

			g__cardinality__3 = 3,	// '-'

			g__cardinality__4 = 4,	// '*'

			g__cardinality__5 = 5,	// '/'

			g__cardinality__6 = 6,	// '~+'

			g__cardinality__7 = 7,	// '~-'

			g__cardinality__8 = 8,	// '~*'

			g__cardinality__9 = 9,	// '~/'

			g__cardinality__10 = 10,	// '{' <number optional> ',' <number optional> '}'

		};
		#endregion
		#region production markers associated with the syntax rule <cardinality>

		// markers associated with production: <cardinality> -> '?'

		public enum production_marker_1 : ushort
		{
			final
		};

		// markers associated with production: <cardinality> -> '+'

		public enum production_marker_2 : ushort
		{
			final
		};

		// markers associated with production: <cardinality> -> '-'

		public enum production_marker_3 : ushort
		{
			final
		};

		// markers associated with production: <cardinality> -> '*'

		public enum production_marker_4 : ushort
		{
			final
		};

		// markers associated with production: <cardinality> -> '/'

		public enum production_marker_5 : ushort
		{
			final
		};

		// markers associated with production: <cardinality> -> '~+'

		public enum production_marker_6 : ushort
		{
			final
		};

		// markers associated with production: <cardinality> -> '~-'

		public enum production_marker_7 : ushort
		{
			final
		};

		// markers associated with production: <cardinality> -> '~*'

		public enum production_marker_8 : ushort
		{
			final
		};

		// markers associated with production: <cardinality> -> '~/'

		public enum production_marker_9 : ushort
		{
			final
		};

		// markers associated with production: <cardinality> -> '{' <number optional> ',' <number optional> '}'

		public enum production_marker_10 : ushort
		{
			m__number_optional_,
			m__number_optional__1,
			final
		};
		#endregion
		#region Constructors and destructors associated with the syntax rule <cardinality>

		// Constructor declaration(s) associated with production(s) of syntax rule <cardinality>

		//
		// Constructor associated with the following production(s)
		// <cardinality> -> '?'

		// <cardinality> -> '+'

		// <cardinality> -> '-'

		// <cardinality> -> '*'

		// <cardinality> -> '/'

		// <cardinality> -> '~+'

		// <cardinality> -> '~-'

		// <cardinality> -> '~*'

		// <cardinality> -> '~/'

		//

		public _cardinality_ (SyntaxTreeToken p_token, int kind) : base ((uint) ProductionID.__cardinality__ID, (uint) kind)
		{
			++g_counter;
			_init ();
			switch ((production_kind) this.kind)
			{
			case production_kind.g__cardinality__1:
				children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 1);
				children[0] = m__question_mark_ = p_token;
				break;
			case production_kind.g__cardinality__2:
				children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 1);
				children[0] = m__plus_sign_ = p_token;
				break;
			case production_kind.g__cardinality__3:
				children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 1);
				children[0] = m__minus_sign_ = p_token;
				break;
			case production_kind.g__cardinality__4:
				children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 1);
				children[0] = m__asterisk_ = p_token;
				break;
			case production_kind.g__cardinality__5:
				children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 1);
				children[0] = m__slash_ = p_token;
				break;
			case production_kind.g__cardinality__6:
				children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 1);
				children[0] = m__inv_plus_sign_ = p_token;
				break;
			case production_kind.g__cardinality__7:
				children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 1);
				children[0] = m__inv_minus_sign_ = p_token;
				break;
			case production_kind.g__cardinality__8:
				children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 1);
				children[0] = m__inv_asterisk_ = p_token;
				break;
			case production_kind.g__cardinality__9:
				children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 1);
				children[0] = m__inv_slash_ = p_token;
				break;
			default:
				{
					string[] args = new string[] { "_cardinality_" };
					throw new SyntaxTreeError (SyntaxTreeError.InvalidKindError, args);
				}
			}
		}

		//
		// Constructor associated with the following production(s)
		// <cardinality> -> '{' <number optional> ',' <number optional> '}'

		//

		public _cardinality_ (SyntaxTreeToken p_token, _number_optional_ p__number_optional_, SyntaxTreeToken p_token_1, _number_optional_ p__number_optional__1, SyntaxTreeToken p_token_2) : base ((uint) ProductionID.__cardinality__ID, (uint) production_kind.g__cardinality__10)
		{
			++g_counter;
			_init ();
			children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 5);
			children[0] = m__left_curly_bracket_ = p_token;
			children[1] = m__number_optional_ = p__number_optional_;
			children[2] = m__comma_ = p_token_1;
			children[3] = m__number_optional__1 = p__number_optional__1;
			children[4] = m__right_curly_bracket_ = p_token_2;
		}

		// Copy constructor

		public _cardinality_ (_cardinality_ p__cardinality_) : base (p__cardinality_.id, p__cardinality_.kind)
		{
			++g_counter;
			_init ();
			switch ((production_kind) this.kind)
			{
			case production_kind.g__cardinality__1:
				children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 1);
				children[0] = m__question_mark_ = p__cardinality_.m__question_mark_;
				break;
			case production_kind.g__cardinality__2:
				children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 5);
				children[0] = m__left_curly_bracket_ = p__cardinality_.m__left_curly_bracket_;
				children[1] = m__number_optional_ = p__cardinality_.m__number_optional_;
				children[2] = m__comma_ = p__cardinality_.m__comma_;
				children[3] = m__number_optional__1 = p__cardinality_.m__number_optional__1;
				children[4] = m__right_curly_bracket_ = p__cardinality_.m__right_curly_bracket_;
				break;
			default:
				string[] args = new string[] { "_cardinality_" };
				throw new SyntaxTreeError (SyntaxTreeError.InvalidKindError, args);
			}
		}

		// Clone node - wrapper of copy constructor

		public override SyntaxTreeBase Clone () { return (SyntaxTreeBase) new _cardinality_ (this); }

		// Destructor

		public new void Dispose ()
		{
			base.Dispose ();
			--g_counter;

			m__question_mark_?.Dispose ();
			m__plus_sign_?.Dispose ();
			m__minus_sign_?.Dispose ();
			m__asterisk_?.Dispose ();
			m__slash_?.Dispose ();
			m__inv_plus_sign_?.Dispose ();
			m__inv_minus_sign_?.Dispose ();
			m__inv_asterisk_?.Dispose ();
			m__inv_slash_?.Dispose ();
			m__left_curly_bracket_?.Dispose ();
			m__number_optional_?.Dispose ();
			m__number_optional__1?.Dispose ();
			m__comma_?.Dispose ();
			m__right_curly_bracket_?.Dispose ();

			_init ();
		}
		#endregion
		#region Content changing and inclusion checking methods associated with the syntax rule <cardinality>

		// Content changing function(s) associated with production(s) of syntax rule <cardinality>

		//
		// Content changing function associated with following production(s)
		// <cardinality> -> '?'

		// <cardinality> -> '+'

		// <cardinality> -> '-'

		// <cardinality> -> '*'

		// <cardinality> -> '/'

		// <cardinality> -> '~+'

		// <cardinality> -> '~-'

		// <cardinality> -> '~*'

		// <cardinality> -> '~/'

		//

		public void change(SyntaxTreeToken p_token, int kind)
		{
			_init ();
			this.kind = (uint) kind;
			switch ((production_kind) this.kind)
			{
			case production_kind.g__cardinality__1:
				children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 1);
				children [0] = m__question_mark_ = p_token;
				break;
			case production_kind.g__cardinality__2:
				children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 1);
				children [0] = m__plus_sign_ = p_token;
				break;
			case production_kind.g__cardinality__3:
				children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 1);
				children [0] = m__minus_sign_ = p_token;
				break;
			case production_kind.g__cardinality__4:
				children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 1);
				children [0] = m__asterisk_ = p_token;
				break;
			case production_kind.g__cardinality__5:
				children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 1);
				children [0] = m__slash_ = p_token;
				break;
			case production_kind.g__cardinality__6:
				children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 1);
				children [0] = m__inv_plus_sign_ = p_token;
				break;
			case production_kind.g__cardinality__7:
				children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 1);
				children [0] = m__inv_minus_sign_ = p_token;
				break;
			case production_kind.g__cardinality__8:
				children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 1);
				children [0] = m__inv_asterisk_ = p_token;
				break;
			case production_kind.g__cardinality__9:
				children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 1);
				children [0] = m__inv_slash_ = p_token;
				break;
			default:
				{
					string[] args = new string[] { "_cardinality_" };
					throw new SyntaxTreeError (SyntaxTreeError.InvalidKindError, args);
				}
			}
			reparent (parent);
		}

		//
		// Content changing function associated with following production(s)
		// <cardinality> -> '{' <number optional> ',' <number optional> '}'

		//

		public void change(SyntaxTreeToken p_token, _number_optional_ p__number_optional_, SyntaxTreeToken p_token_1, _number_optional_ p__number_optional__1, SyntaxTreeToken p_token_2)
		{
			_init ();
			this.kind = (uint) production_kind.g__cardinality__10;
			children = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), 5);
			children [0] = m__left_curly_bracket_ = p_token;
			children [1] = m__number_optional_ = p__number_optional_;
			children [2] = m__comma_ = p_token_1;
			children [3] = m__number_optional__1 = p__number_optional__1;
			children [4] = m__right_curly_bracket_ = p_token_2;
			reparent (parent);
		}

		//
		// check if node 'element' is included within syntax tree associated with syntax rule <cardinality>
		//

		public new bool checkInclusion (SyntaxTreeBase element)
		{
			return
				(element == this) ||
				(m__question_mark_ != null) && m__question_mark_.checkInclusion (element) ||
				(m__plus_sign_ != null) && m__plus_sign_.checkInclusion (element) ||
				(m__minus_sign_ != null) && m__minus_sign_.checkInclusion (element) ||
				(m__asterisk_ != null) && m__asterisk_.checkInclusion (element) ||
				(m__slash_ != null) && m__slash_.checkInclusion (element) ||
				(m__inv_plus_sign_ != null) && m__inv_plus_sign_.checkInclusion (element) ||
				(m__inv_minus_sign_ != null) && m__inv_minus_sign_.checkInclusion (element) ||
				(m__inv_asterisk_ != null) && m__inv_asterisk_.checkInclusion (element) ||
				(m__inv_slash_ != null) && m__inv_slash_.checkInclusion (element) ||
				(m__left_curly_bracket_ != null) && m__left_curly_bracket_.checkInclusion (element) ||
				(m__number_optional_ != null) && m__number_optional_.checkInclusion (element) ||
				(m__number_optional__1 != null) && m__number_optional__1.checkInclusion (element) ||
				(m__comma_ != null) && m__comma_.checkInclusion (element) ||
				(m__right_curly_bracket_ != null) && m__right_curly_bracket_.checkInclusion (element) ||
				false;
		}
		#endregion
		#region Syntax tree emit methods associated with the syntax rule <cardinality>

		//
		// emit production tree node associated with any production of syntax rule <cardinality>
		//

		public override string Emit (int depth)
		{
			string s = "";
			if (depth != 0)
			switch ((production_kind) kind)
			{
				case production_kind.g__cardinality__1:
					// emit syntax tree node associated with production
					// <cardinality>: '?'

					s += m__question_mark_.Emit (depth - 1);
				break;
				case production_kind.g__cardinality__2:
					// emit syntax tree node associated with production
					// <cardinality>: '+'

					s += m__plus_sign_.Emit (depth - 1);
				break;
				case production_kind.g__cardinality__3:
					// emit syntax tree node associated with production
					// <cardinality>: '-'

					s += m__minus_sign_.Emit (depth - 1);
				break;
				case production_kind.g__cardinality__4:
					// emit syntax tree node associated with production
					// <cardinality>: '*'

					s += m__asterisk_.Emit (depth - 1);
				break;
				case production_kind.g__cardinality__5:
					// emit syntax tree node associated with production
					// <cardinality>: '/'

					s += m__slash_.Emit (depth - 1);
				break;
				case production_kind.g__cardinality__6:
					// emit syntax tree node associated with production
					// <cardinality>: '~+'

					s += m__inv_plus_sign_.Emit (depth - 1);
				break;
				case production_kind.g__cardinality__7:
					// emit syntax tree node associated with production
					// <cardinality>: '~-'

					s += m__inv_minus_sign_.Emit (depth - 1);
				break;
				case production_kind.g__cardinality__8:
					// emit syntax tree node associated with production
					// <cardinality>: '~*'

					s += m__inv_asterisk_.Emit (depth - 1);
				break;
				case production_kind.g__cardinality__9:
					// emit syntax tree node associated with production
					// <cardinality>: '~/'

					s += m__inv_slash_.Emit (depth - 1);
				break;
				case production_kind.g__cardinality__10:
					// emit syntax tree node associated with production
					// <cardinality>: '{' <number optional> ',' <number optional> '}'

					s += m__left_curly_bracket_.Emit (depth - 1);
					s += " ";
					s += m__number_optional_.Emit (depth - 1);
					s += " ";
					s += m__comma_.Emit (depth - 1);
					s += " ";
					s += m__number_optional__1.Emit (depth - 1);
					s += " ";
					s += m__right_curly_bracket_.Emit (depth - 1);
				break;
			}
			return s.Trim ();
		}

		//
		// emit production tree node associated with any production of syntax rule <cardinality>
		//

		public override string EmitProductionTree (int depth)
		{
			string s = "_cardinality_ (";
			if (depth != 0)
				switch ((production_kind) kind)
				{
				case production_kind.g__cardinality__1:
					// emit syntax tree node associated with production
					// <cardinality>: '?'

					s += "_question_mark_";
				break;
				case production_kind.g__cardinality__2:
					// emit syntax tree node associated with production
					// <cardinality>: '+'

					s += "_plus_sign_";
				break;
				case production_kind.g__cardinality__3:
					// emit syntax tree node associated with production
					// <cardinality>: '-'

					s += "_minus_sign_";
				break;
				case production_kind.g__cardinality__4:
					// emit syntax tree node associated with production
					// <cardinality>: '*'

					s += "_asterisk_";
				break;
				case production_kind.g__cardinality__5:
					// emit syntax tree node associated with production
					// <cardinality>: '/'

					s += "_slash_";
				break;
				case production_kind.g__cardinality__6:
					// emit syntax tree node associated with production
					// <cardinality>: '~+'

					s += "_inv_plus_sign_";
				break;
				case production_kind.g__cardinality__7:
					// emit syntax tree node associated with production
					// <cardinality>: '~-'

					s += "_inv_minus_sign_";
				break;
				case production_kind.g__cardinality__8:
					// emit syntax tree node associated with production
					// <cardinality>: '~*'

					s += "_inv_asterisk_";
				break;
				case production_kind.g__cardinality__9:
					// emit syntax tree node associated with production
					// <cardinality>: '~/'

					s += "_inv_slash_";
				break;
				case production_kind.g__cardinality__10:
					// emit syntax tree node associated with production
					// <cardinality>: '{' <number optional> ',' <number optional> '}'

					s += "_left_curly_bracket_";
					s += ' ';
					s += m__number_optional_.EmitProductionTree (depth - 1);
					s += ' ';
					s += "_comma_";
					s += ' ';
					s += m__number_optional__1.EmitProductionTree (depth - 1);
					s += ' ';
					s += "_right_curly_bracket_";
				break;
			}
			s += ")";
			return s;
		}
		#endregion
		#region Syntax tree traversal,  reperrent, init and iterator methods associated with the syntax rule <cardinality>

		//
		// traverse sub-tree rooted in this node using selected syntax tree walker
		//

		public override void InvokeTraverse (SyntaxTreeWalkerCore syntaxTreeWalker) => ((SyntaxTreeWalker) syntaxTreeWalker).Traverse (this);
		public override void InvokeTraverseCommon (SyntaxTreeWalkerCore syntaxTreeWalker) => ((SyntaxTreeWalker) syntaxTreeWalker).TraverseCommon (this);

		// reparent sub-tree

		public override void reparent (SyntaxTreeBase parent)
		{
			this.parent = parent;

			m__question_mark_?.reparent (this);
			m__plus_sign_?.reparent (this);
			m__minus_sign_?.reparent (this);
			m__asterisk_?.reparent (this);
			m__slash_?.reparent (this);
			m__inv_plus_sign_?.reparent (this);
			m__inv_minus_sign_?.reparent (this);
			m__inv_asterisk_?.reparent (this);
			m__inv_slash_?.reparent (this);
			m__left_curly_bracket_?.reparent (this);
			m__number_optional_?.reparent (this);
			m__number_optional__1?.reparent (this);

			m__comma_?.reparent (this);
			m__right_curly_bracket_?.reparent (this);
		}

		//
		// initialize object associated with syntax rule <cardinality>
		//

		public void _init ()
		{
			m__question_mark_ = null;
			m__plus_sign_ = null;
			m__minus_sign_ = null;
			m__asterisk_ = null;
			m__slash_ = null;
			m__inv_plus_sign_ = null;
			m__inv_minus_sign_ = null;
			m__inv_asterisk_ = null;
			m__inv_slash_ = null;
			m__left_curly_bracket_ = null;
			m__number_optional_ = null;
			m__number_optional__1 = null;
			m__comma_ = null;
			m__right_curly_bracket_ = null;
		}
		#endregion
		#region Fields and properties associated with the syntax rule <cardinality>

		// counter of all nodes associated with syntax rule <cardinality>
		public static int g_counter;

		// objects associated with terminal and non-terminal symbols within production(s) of syntax rule <cardinality>
		public SyntaxTreeToken m__question_mark_ { get; private set; }
		public SyntaxTreeToken m__plus_sign_ { get; private set; }
		public SyntaxTreeToken m__minus_sign_ { get; private set; }
		public SyntaxTreeToken m__asterisk_ { get; private set; }
		public SyntaxTreeToken m__slash_ { get; private set; }
		public SyntaxTreeToken m__inv_plus_sign_ { get; private set; }
		public SyntaxTreeToken m__inv_minus_sign_ { get; private set; }
		public SyntaxTreeToken m__inv_asterisk_ { get; private set; }
		public SyntaxTreeToken m__inv_slash_ { get; private set; }
		public SyntaxTreeToken m__left_curly_bracket_ { get; private set; }
		public _number_optional_ m__number_optional_ { get; private set; }
		public _number_optional_ m__number_optional__1 { get; private set; }
		public SyntaxTreeToken m__comma_ { get; private set; }
		public SyntaxTreeToken m__right_curly_bracket_ { get; private set; }
		#endregion

	};

	public partial class SyntaxTreeWalker
	{
		#region delegates and events associated with the syntax rule <cardinality>

		// delegate function (callback) prototype associated with syntax rule <cardinality>
		public delegate bool _cardinality__Callback (SyntaxTreeCallbackReason reason, _cardinality_.production_kind kind, _cardinality_ p__cardinality_);

		// event associated with syntax rule <cardinality>
		public event _cardinality__Callback _cardinality__Event;

		// event trigger associated with syntax rule <cardinality>
		public bool Raise__cardinality__Event (SyntaxTreeCallbackReason reason, _cardinality_.production_kind kind, _cardinality_ p__cardinality_)
		{
			bool? status = _cardinality__Event?.Invoke (reason, kind, p__cardinality_);
			return (status == null) || status.Value;
		}
		#endregion
		#region syntax tree traversal methods associated with the syntax rule <cardinality>
		//
		// traverse syntax tree node associated with any production of syntax rule <cardinality>
		//

		public void Traverse (_cardinality_ p__cardinality_)
		{
			if (p__cardinality_.isLocked())
				return;
			p__cardinality_.dolock();
			_cardinality_.production_kind kind = (_cardinality_.production_kind) p__cardinality_.kind;
			p__cardinality_.turn_reset ();
			if (Raise__cardinality__Event (SyntaxTreeCallbackReason.TraversalPrologueCallbackReason, kind, p__cardinality_))
			switch (kind)
			{
				case _cardinality_.production_kind.g__cardinality__1:
					// traverse syntax tree node associated with production
					// <cardinality>: '?'

				break;
				case _cardinality_.production_kind.g__cardinality__2:
					// traverse syntax tree node associated with production
					// <cardinality>: '+'

				break;
				case _cardinality_.production_kind.g__cardinality__3:
					// traverse syntax tree node associated with production
					// <cardinality>: '-'

				break;
				case _cardinality_.production_kind.g__cardinality__4:
					// traverse syntax tree node associated with production
					// <cardinality>: '*'

				break;
				case _cardinality_.production_kind.g__cardinality__5:
					// traverse syntax tree node associated with production
					// <cardinality>: '/'

				break;
				case _cardinality_.production_kind.g__cardinality__6:
					// traverse syntax tree node associated with production
					// <cardinality>: '~+'

				break;
				case _cardinality_.production_kind.g__cardinality__7:
					// traverse syntax tree node associated with production
					// <cardinality>: '~-'

				break;
				case _cardinality_.production_kind.g__cardinality__8:
					// traverse syntax tree node associated with production
					// <cardinality>: '~*'

				break;
				case _cardinality_.production_kind.g__cardinality__9:
					// traverse syntax tree node associated with production
					// <cardinality>: '~/'

				break;
				case _cardinality_.production_kind.g__cardinality__10:
					// traverse syntax tree node associated with production
					// <cardinality>: '{' <number optional> ',' <number optional> '}'

					if (Raise__cardinality__Event (SyntaxTreeCallbackReason.TraversalMidTermCallbackReason, kind, p__cardinality_))
						Traverse (p__cardinality_.m__number_optional_);
					p__cardinality_.turn_inc ();
					if (Raise__cardinality__Event (SyntaxTreeCallbackReason.TraversalMidTermCallbackReason, kind, p__cardinality_))
						Traverse (p__cardinality_.m__number_optional__1);
					p__cardinality_.turn_inc ();
					Raise__cardinality__Event (SyntaxTreeCallbackReason.TraversalMidTermCallbackReason, kind, p__cardinality_);
				break;
			}
			Raise__cardinality__Event (SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason, kind, p__cardinality_);
			p__cardinality_.unlock();
		}

		//
		// traverse syntax tree node associated with any production of syntax rule <cardinality>
		//

		public void TraverseCommon (_cardinality_ p__cardinality_)
		{
			_cardinality_.production_kind kind = (_cardinality_.production_kind) p__cardinality_.kind;
			if (Raise_Common_Event (SyntaxTreeCallbackReason.TraversalPrologueCallbackReason, (int) kind, p__cardinality_))
			switch (kind)
			{
				case _cardinality_.production_kind.g__cardinality__1:
					// traverse syntax tree node associated with production
					// <cardinality>: '?'

						TraverseCommon (p__cardinality_.m__question_mark_);
				break;
				case _cardinality_.production_kind.g__cardinality__2:
					// traverse syntax tree node associated with production
					// <cardinality>: '+'

						TraverseCommon (p__cardinality_.m__plus_sign_);
				break;
				case _cardinality_.production_kind.g__cardinality__3:
					// traverse syntax tree node associated with production
					// <cardinality>: '-'

						TraverseCommon (p__cardinality_.m__minus_sign_);
				break;
				case _cardinality_.production_kind.g__cardinality__4:
					// traverse syntax tree node associated with production
					// <cardinality>: '*'

						TraverseCommon (p__cardinality_.m__asterisk_);
				break;
				case _cardinality_.production_kind.g__cardinality__5:
					// traverse syntax tree node associated with production
					// <cardinality>: '/'

						TraverseCommon (p__cardinality_.m__slash_);
				break;
				case _cardinality_.production_kind.g__cardinality__6:
					// traverse syntax tree node associated with production
					// <cardinality>: '~+'

						TraverseCommon (p__cardinality_.m__inv_plus_sign_);
				break;
				case _cardinality_.production_kind.g__cardinality__7:
					// traverse syntax tree node associated with production
					// <cardinality>: '~-'

						TraverseCommon (p__cardinality_.m__inv_minus_sign_);
				break;
				case _cardinality_.production_kind.g__cardinality__8:
					// traverse syntax tree node associated with production
					// <cardinality>: '~*'

						TraverseCommon (p__cardinality_.m__inv_asterisk_);
				break;
				case _cardinality_.production_kind.g__cardinality__9:
					// traverse syntax tree node associated with production
					// <cardinality>: '~/'

						TraverseCommon (p__cardinality_.m__inv_slash_);
				break;
				case _cardinality_.production_kind.g__cardinality__10:
					// traverse syntax tree node associated with production
					// <cardinality>: '{' <number optional> ',' <number optional> '}'

						TraverseCommon (p__cardinality_.m__left_curly_bracket_);
						TraverseCommon (p__cardinality_.m__number_optional_);
						TraverseCommon (p__cardinality_.m__comma_);
						TraverseCommon (p__cardinality_.m__number_optional__1);
						TraverseCommon (p__cardinality_.m__right_curly_bracket_);
				break;
			}
			Raise_Common_Event (SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason, (int) kind, p__cardinality_);
		}
	};
		#endregion

	public partial class SyntaxTreeBuilder : SyntaxTreeWalker
	{
		#region syntax tree builder methods associated with the syntax rule <cardinality>

		//
		// create syntax tree node associated with production
		// <cardinality>: '?'

		//

		public _cardinality_ _cardinality__1 (SyntaxTreeToken p_token)
		{
			_cardinality_ p__cardinality__ref = new _cardinality_(p_token, 1);
			p_token.parent = p__cardinality__ref;
			Raise__cardinality__Event (SyntaxTreeCallbackReason.BuilderCallbackReason, _cardinality_.production_kind.g__cardinality__1, p__cardinality__ref);
			return p__cardinality__ref;
		}

		//
		// create syntax tree node associated with production
		// <cardinality>: '+'

		//

		public _cardinality_ _cardinality__2 (SyntaxTreeToken p_token)
		{
			_cardinality_ p__cardinality__ref = new _cardinality_(p_token, 2);
			p_token.parent = p__cardinality__ref;
			Raise__cardinality__Event (SyntaxTreeCallbackReason.BuilderCallbackReason, _cardinality_.production_kind.g__cardinality__2, p__cardinality__ref);
			return p__cardinality__ref;
		}

		//
		// create syntax tree node associated with production
		// <cardinality>: '-'

		//

		public _cardinality_ _cardinality__3 (SyntaxTreeToken p_token)
		{
			_cardinality_ p__cardinality__ref = new _cardinality_(p_token, 3);
			p_token.parent = p__cardinality__ref;
			Raise__cardinality__Event (SyntaxTreeCallbackReason.BuilderCallbackReason, _cardinality_.production_kind.g__cardinality__3, p__cardinality__ref);
			return p__cardinality__ref;
		}

		//
		// create syntax tree node associated with production
		// <cardinality>: '*'

		//

		public _cardinality_ _cardinality__4 (SyntaxTreeToken p_token)
		{
			_cardinality_ p__cardinality__ref = new _cardinality_(p_token, 4);
			p_token.parent = p__cardinality__ref;
			Raise__cardinality__Event (SyntaxTreeCallbackReason.BuilderCallbackReason, _cardinality_.production_kind.g__cardinality__4, p__cardinality__ref);
			return p__cardinality__ref;
		}

		//
		// create syntax tree node associated with production
		// <cardinality>: '/'

		//

		public _cardinality_ _cardinality__5 (SyntaxTreeToken p_token)
		{
			_cardinality_ p__cardinality__ref = new _cardinality_(p_token, 5);
			p_token.parent = p__cardinality__ref;
			Raise__cardinality__Event (SyntaxTreeCallbackReason.BuilderCallbackReason, _cardinality_.production_kind.g__cardinality__5, p__cardinality__ref);
			return p__cardinality__ref;
		}

		//
		// create syntax tree node associated with production
		// <cardinality>: '~+'

		//

		public _cardinality_ _cardinality__6 (SyntaxTreeToken p_token)
		{
			_cardinality_ p__cardinality__ref = new _cardinality_(p_token, 6);
			p_token.parent = p__cardinality__ref;
			Raise__cardinality__Event (SyntaxTreeCallbackReason.BuilderCallbackReason, _cardinality_.production_kind.g__cardinality__6, p__cardinality__ref);
			return p__cardinality__ref;
		}

		//
		// create syntax tree node associated with production
		// <cardinality>: '~-'

		//

		public _cardinality_ _cardinality__7 (SyntaxTreeToken p_token)
		{
			_cardinality_ p__cardinality__ref = new _cardinality_(p_token, 7);
			p_token.parent = p__cardinality__ref;
			Raise__cardinality__Event (SyntaxTreeCallbackReason.BuilderCallbackReason, _cardinality_.production_kind.g__cardinality__7, p__cardinality__ref);
			return p__cardinality__ref;
		}

		//
		// create syntax tree node associated with production
		// <cardinality>: '~*'

		//

		public _cardinality_ _cardinality__8 (SyntaxTreeToken p_token)
		{
			_cardinality_ p__cardinality__ref = new _cardinality_(p_token, 8);
			p_token.parent = p__cardinality__ref;
			Raise__cardinality__Event (SyntaxTreeCallbackReason.BuilderCallbackReason, _cardinality_.production_kind.g__cardinality__8, p__cardinality__ref);
			return p__cardinality__ref;
		}

		//
		// create syntax tree node associated with production
		// <cardinality>: '~/'

		//

		public _cardinality_ _cardinality__9 (SyntaxTreeToken p_token)
		{
			_cardinality_ p__cardinality__ref = new _cardinality_(p_token, 9);
			p_token.parent = p__cardinality__ref;
			Raise__cardinality__Event (SyntaxTreeCallbackReason.BuilderCallbackReason, _cardinality_.production_kind.g__cardinality__9, p__cardinality__ref);
			return p__cardinality__ref;
		}

		//
		// create syntax tree node associated with production
		// <cardinality>: '{' <number optional> ',' <number optional> '}'

		//

		public _cardinality_ _cardinality__10 (SyntaxTreeToken p_token, _number_optional_ p__number_optional_, SyntaxTreeToken p_token_1, _number_optional_ p__number_optional__1, SyntaxTreeToken p_token_2)
		{
			_cardinality_ p__cardinality__ref = new _cardinality_(p_token, p__number_optional_, p_token_1, p__number_optional__1, p_token_2);
			p_token.parent = p__cardinality__ref;
			p__number_optional_.parent = p__cardinality__ref;
			p_token_1.parent = p__cardinality__ref;
			p__number_optional__1.parent = p__cardinality__ref;
			p_token_2.parent = p__cardinality__ref;
			Raise__cardinality__Event (SyntaxTreeCallbackReason.BuilderCallbackReason, _cardinality_.production_kind.g__cardinality__10, p__cardinality__ref);
			return p__cardinality__ref;
		}
		#endregion
	};
}
