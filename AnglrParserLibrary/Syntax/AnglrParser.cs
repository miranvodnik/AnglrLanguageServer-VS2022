//
//	This file was generated with ANGLR compiler
//
using System;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.IO;
using Anglr.Declarations;
using Anglr.Lexer;

using Anglr.Parser.Core;
using Anglr.Parser.SyntaxTree;
using AnglrLogLibrary;
#if ANGLR_DEBUGGER
using AnglrDebuggerBridge;
#endif

namespace Anglr.Parser
{

	public class AnglrParser : SyntaxTreeBuilder, ParserInterface
	{
		public IAnglrLogger AnglrLogger { get; private set; }
		public event StartParserDelegate StartParserEvent;
		public event SyntaxErrorDelegate SyntaxErrorEvent;
		public event ShiftStepDelegate ShiftStepEvent;
		public event ReduceStepDelegate ReduceStepEvent;
		public event SplitStepDelegate SplitStepEvent;
		public event LoopStepDelegate LoopStepEvent;
		public event JoinDelegate JoinEvent;
		public event FinalStepDelegate FinalStepEvent;
		public event StopParserDelegate StopParserEvent;

		public void StartParserReport (int stackNr, object [] info = null) => StartParserEvent?.Invoke (stackNr, info);
		public void SyntaxErrorReport (int stackNr, int state) => SyntaxErrorEvent?.Invoke (stackNr, state);
		public void ShiftStepReport (int stackNr, int state, int tokenValue, string tokenName, string tokenText, bool conflict) => ShiftStepEvent?.Invoke (stackNr, state, tokenValue, tokenName, tokenText, conflict);
		public void ReduceStepReport (int stackNr, int prodNr, int ruleNr, string ruleName, int prodLen, int fallingState, int bottomState, int risingState, SyntaxTreeBase currentValue, bool bottom) => ReduceStepEvent?.Invoke (stackNr, prodNr, ruleNr, ruleName, prodLen, fallingState, bottomState, risingState, currentValue, bottom);
		public void SplitStepReport (int stackNr, int state, bool begin) => SplitStepEvent?.Invoke (stackNr, state, begin);
		public void LoopStepReport (int stackNr, int state) => LoopStepEvent?.Invoke (stackNr, state);
		public void JoinReport (int stackNr, int joinNr) => JoinEvent?.Invoke (stackNr, joinNr);
		public void FinalStepReport (int stackNr) => FinalStepEvent?.Invoke (stackNr);
		public void StopParserReport () => StopParserEvent?.Invoke ();
		public string [] GetStackText (int stackNr) => stackSet.GetStackText (stackNr);

		public bool FragmentParser { get; set; } = false;
		public int ProductionID { get; set; } = 0;
		public int InitialState { get; set; } = 0;
		public int LastToken { get; set; } = 0;
		public string LastTokenName { get; set; } = "";
#if ANGLR_DEBUGGER
		public AnglrDebuggerClientBridge AnglrDebugBridgeObj { get; private set; }
#endif

		public AnglrParser (string fragmentName = null, IAnglrLogger anglrLogger = null) : base ()
		{
			AnglrLogger = anglrLogger;
			(int id, int state, int token, string tokenName) fragmentInfo = AnglrFragments.GetFragmentInfo (fragmentName);
			if (fragmentInfo.id != 0)
			{
				FragmentParser = true;
				ProductionID = fragmentInfo.id;
				InitialState = fragmentInfo.state;
				LastToken = fragmentInfo.token;
				LastTokenName = fragmentInfo.tokenName;
			}
			token = new ParserToken (0, 0, null);
			stackSet = new stackset (new cmpstackid ());
			parseList = new parselist ();
			stackSet.Add (new ParserStack (this));
		}

		public void reset ()
		{
			token = new ParserToken (0, 0, null);
			stackSet = new stackset (new cmpstackid ());
			parseList = new parselist ();
			stackSet.Add (new ParserStack (this));
		}

		public bool CheckTokenInStack (ParserStack parserStack, int token)
		{
			int stackDepth = parserStack.stackDepth;
			while (stackDepth-- > 0)
			{
				if (!parserStack.CheckTokenInState (token, parserStack.stateStack[stackDepth]))
					continue;
				return true;
			}
			return false;
		}

#if ANGLR_DEBUGGER
		private void CreateDebuggerConnection ()
		{
			this.AnglrDebugBridgeObj = new AnglrDebuggerClientBridge (this);
		}
		private void DisposeDebuggerConnection ()
		{
			this.AnglrDebugBridgeObj?.Dispose ();
		}
#endif

		public int parse (AnglrLexer scanner)
		{
			int step = 0;
			int finalCount = 0;
			int syntaxCnt = 0;
			int tokenCnt = 0;
			int shiftCnt = 0;
			int reduceCnt = 0;
			int splitCnt = 0;
			int joinCnt = 0;
			bool eofCondition = false;
			bool errCondition = false;

			try
			{
#if ANGLR_DEBUGGER
			CreateDebuggerConnection ();
#endif

			StartParserReport (ParserStack.g_stackCounter, scanner.Info);

			while (true)
			{
				++step;
				AnglrLogger?.DebugRawLine ("STEP   " + step + ", " + stackSet.Count);
				int tokenRequest = 0;
				int shiftRequest = 0;
				stacklist delStackList = new stacklist ();
				stacklist insStackList = new stacklist ();
				stacklist errStackList = new stacklist ();

				foreach (ParserStack set in stackSet)
				{
					while (true)
					{
						switch (set.Step (insStackList, delStackList))
						{
						case StepOutcome.NoStep:
							AnglrLogger?.DebugRawLine ("NOSTEP " + set.stackCounter);
							delStackList.Add (set);
							++syntaxCnt;
							break;
						case StepOutcome.ResizeFailed:
							AnglrLogger?.DebugRawLine ("RESIZE " + set.stackCounter);
							delStackList.Add (set);
							++syntaxCnt;
							break;
						case StepOutcome.SyntaxError:
							AnglrLogger?.DebugRawLine ("SYNTAX " + set.stackCounter + " '" + set.tokenText + "' (" + set.tokenValue + ")");
							errStackList.Add (set);
							++syntaxCnt;
							break;
						case StepOutcome.LoopStep:
							AnglrLogger?.DebugRawLine ("LOOP   " + set.stackCounter);
							delStackList.Add (set);
							++syntaxCnt;
							break;
						case StepOutcome.TokenUnavailable:
							++tokenRequest;
							++tokenCnt;
							break;
						case StepOutcome.ShiftStep:
							++shiftRequest;
							++shiftCnt;
							break;
						case StepOutcome.ReduceStep:
							++reduceCnt;
							continue;
							//break;
						case StepOutcome.SplitStep:
							delStackList.Add (set);
							++splitCnt;
							break;
						case StepOutcome.FinalStep:
							++finalCount;
							set.valueStack.Pop ();
							parseList.Add (set.valueStack.Peek());
							delStackList.Add (set);
							break;
						}
						break;
					}
				}

				foreach (ParserStack set in delStackList)
					stackSet.Remove (set);

				foreach (ParserStack set in insStackList)
					stackSet.Add (set);

				int size;
				if ((size = stackSet.Count) > 1)
				{
					int count = 0;
					stackset smap = new stackset (new cmpstackinfo ());
					foreach (ParserStack set in stackSet)
					{
						if (!smap.Contains (set))
							smap.Add (set);
						else
						{
							AnglrLogger?.DebugRawLine ("JOIN   " + set.stackCounter + ", " + set.m_equivalent.stackCounter);
							set.Join (set.m_equivalent);
							JoinReport (set.stackCounter, set.m_equivalent.stackCounter);
							errStackList.Remove (set);
							++joinCnt;
						}
					}
					if (smap.Count < stackSet.Count)
					{
						stackSet.Clear ();
						foreach (ParserStack set in smap)
							stackSet.Add (set);
					}
					if ((size = stackSet.Count) < 2)
						++count;
				}
				if (size == 0)
				{
					if (errStackList.Count == 0)
						break;
				}
				else
				{
					if (errStackList.Count < size)
					{
						foreach (ParserStack set in errStackList)
						{
							SyntaxErrorReport (set.stackCounter, -1);
							stackSet.Remove (set);
						}
						errStackList.Clear ();
					}
				}
				errCondition = (errStackList.Count > 0) && (errStackList.Count >= size);
				foreach (ParserStack set in errStackList)
					set.sequenceNr = token.sequenceNr;
				bool getToken = false;
				foreach (ParserStack set in stackSet)
				{
					if (set.sequenceNr < token.sequenceNr)
					{
						getToken = true;
						break;
					}
				}
				if (!getToken)
				{
					if (errCondition)
					{
						if (Raise_Error_Event (this.token.lineno, this.token.column - this.token.tokenText.Length, this.token.token, this.token.tokenText))
							break;
						if (eofCondition)
							break;
					}
					int token = scanner.scan ();
					int secondary = scanner.secondary;
					int lineno = scanner.lineno;
					int column = scanner.column;
					string tokenText = scanner.text;
					if (token < 0)
					{
						if (LastToken > 0)
						{
							token = LastToken;
							tokenText = LastTokenName;
						}
						LastToken = 0;
					}
					this.token.Load ((eofCondition = (token <= 0)) ? (int) TerminalCodes.token__eof_token : token, secondary, lineno, column, tokenText);
					AnglrLogger?.DebugRawLine ("TOKEN  '" + tokenText + "' (" + token + ")");
					foreach (ParserStack set in stackSet)
						set.LoadToken ();
					if (secondary > 0)
					{
						stackset parserStacks = new stackset (new cmpstackid ());
						foreach (ParserStack set in stackSet)
						{
							ParserStack pset;
							parserStacks.Add (pset = new ParserStack (set));
							SplitStepReport (set.stackCounter, pset.stackCounter, true);
							pset.LoadSecondary ();
						}
						foreach (ParserStack set in parserStacks)
							stackSet.Add (set);
					}
				}
			}
			StopParserReport ();
			if (finalCount > 0)
				AnglrLogger?.DebugRawLine ("       OK\n" +
					"         final =  " + finalCount + '\n' +
					"         syntax = " + syntaxCnt + '\n' +
					"         token =  " + tokenCnt + '\n' +
					"         shift =  " + shiftCnt + '\n' +
					"         reduce = " + reduceCnt + '\n' +
					"         split =  " + splitCnt + '\n' +
					"         join =   " + joinCnt + '\n' +
					"         steps =  " + step);
			else
				AnglrLogger?.DebugRawLine ("       NOK");
			return (finalCount > 0) ? 0 : -1;

			}
			finally
			{
#if ANGLR_DEBUGGER
				DisposeDebuggerConnection ();
#endif
			}
		}

		SyntaxTreeBase ParserInterface.Build (ParserStack stack, int reductionNr, int prodLen)
		{
			valstack valueStack = stack.valueStack;
			SyntaxTreeBase currentValue = null;

			switch (reductionNr)
			{
			case 1:
				currentValue = _anglr_file_fragment__1 ((SyntaxTreeToken) valueStack [-2], (_attribute_list_) valueStack [-1]);
				break;
			case 2:
				currentValue = _anglr_file_fragment__2 ((SyntaxTreeToken) valueStack [-2], (_attribute_) valueStack [-1]);
				break;
			case 3:
				currentValue = _anglr_file_fragment__3 ((SyntaxTreeToken) valueStack [-2], (_name_value_list_) valueStack [-1]);
				break;
			case 4:
				currentValue = _anglr_file_fragment__4 ((SyntaxTreeToken) valueStack [-2], (_name_value_pair_) valueStack [-1]);
				break;
			case 5:
				currentValue = _anglr_file_fragment__5 ((SyntaxTreeToken) valueStack [-2], (_anglr_file_) valueStack [-1]);
				break;
			case 6:
				currentValue = _anglr_file_fragment__6 ((SyntaxTreeToken) valueStack [-2], (_anglr_file_part_list_) valueStack [-1]);
				break;
			case 7:
				currentValue = _anglr_file_fragment__7 ((SyntaxTreeToken) valueStack [-2], (_anglr_file_part_) valueStack [-1]);
				break;
			case 8:
				currentValue = _anglr_file_fragment__8 ((SyntaxTreeToken) valueStack [-2], (_general_part_) valueStack [-1]);
				break;
			case 9:
				currentValue = _anglr_file_fragment__9 ((SyntaxTreeToken) valueStack [-2], (_declaration_part_) valueStack [-1]);
				break;
			case 10:
				currentValue = _anglr_file_fragment__10 ((SyntaxTreeToken) valueStack [-2], (_anglr_definition_list_) valueStack [-1]);
				break;
			case 11:
				currentValue = _anglr_file_fragment__11 ((SyntaxTreeToken) valueStack [-2], (_anglr_definition_with_attribute_) valueStack [-1]);
				break;
			case 12:
				currentValue = _anglr_file_fragment__12 ((SyntaxTreeToken) valueStack [-2], (_anglr_definition_) valueStack [-1]);
				break;
			case 13:
				currentValue = _anglr_file_fragment__13 ((SyntaxTreeToken) valueStack [-2], (_single_terminal_definition_) valueStack [-1]);
				break;
			case 14:
				currentValue = _anglr_file_fragment__14 ((SyntaxTreeToken) valueStack [-2], (_single_regex_definition_) valueStack [-1]);
				break;
			case 15:
				currentValue = _anglr_file_fragment__15 ((SyntaxTreeToken) valueStack [-2], (_block_of_terminal_definitions_) valueStack [-1]);
				break;
			case 16:
				currentValue = _anglr_file_fragment__16 ((SyntaxTreeToken) valueStack [-2], (_block_of_regex_definitions_) valueStack [-1]);
				break;
			case 17:
				currentValue = _anglr_file_fragment__17 ((SyntaxTreeToken) valueStack [-2], (_terminal_definition_) valueStack [-1]);
				break;
			case 18:
				currentValue = _anglr_file_fragment__18 ((SyntaxTreeToken) valueStack [-2], (_regex_definition_) valueStack [-1]);
				break;
			case 19:
				currentValue = _anglr_file_fragment__19 ((SyntaxTreeToken) valueStack [-2], (_block_terminal_definitions_) valueStack [-1]);
				break;
			case 20:
				currentValue = _anglr_file_fragment__20 ((SyntaxTreeToken) valueStack [-2], (_block_terminal_definition_) valueStack [-1]);
				break;
			case 21:
				currentValue = _anglr_file_fragment__21 ((SyntaxTreeToken) valueStack [-2], (_block_regex_definitions_) valueStack [-1]);
				break;
			case 22:
				currentValue = _anglr_file_fragment__22 ((SyntaxTreeToken) valueStack [-2], (_block_regex_definition_) valueStack [-1]);
				break;
			case 23:
				currentValue = _anglr_file_fragment__23 ((SyntaxTreeToken) valueStack [-2], (_scanner_part_) valueStack [-1]);
				break;
			case 24:
				currentValue = _anglr_file_fragment__24 ((SyntaxTreeToken) valueStack [-2], (_regular_expression_list_) valueStack [-1]);
				break;
			case 25:
				currentValue = _anglr_file_fragment__25 ((SyntaxTreeToken) valueStack [-2], (_regular_expression_usage_) valueStack [-1]);
				break;
			case 26:
				currentValue = _anglr_file_fragment__26 ((SyntaxTreeToken) valueStack [-2], (_actions_) valueStack [-1]);
				break;
			case 27:
				currentValue = _anglr_file_fragment__27 ((SyntaxTreeToken) valueStack [-2], (_action_) valueStack [-1]);
				break;
			case 28:
				currentValue = _anglr_file_fragment__28 ((SyntaxTreeToken) valueStack [-2], (_skip_action_) valueStack [-1]);
				break;
			case 29:
				currentValue = _anglr_file_fragment__29 ((SyntaxTreeToken) valueStack [-2], (_terminal_action_) valueStack [-1]);
				break;
			case 30:
				currentValue = _anglr_file_fragment__30 ((SyntaxTreeToken) valueStack [-2], (_event_action_) valueStack [-1]);
				break;
			case 31:
				currentValue = _anglr_file_fragment__31 ((SyntaxTreeToken) valueStack [-2], (_push_action_) valueStack [-1]);
				break;
			case 32:
				currentValue = _anglr_file_fragment__32 ((SyntaxTreeToken) valueStack [-2], (_pop_action_) valueStack [-1]);
				break;
			case 33:
				currentValue = _anglr_file_fragment__33 ((SyntaxTreeToken) valueStack [-2], (_lexer_part_) valueStack [-1]);
				break;
			case 34:
				currentValue = _anglr_file_fragment__34 ((SyntaxTreeToken) valueStack [-2], (_parser_part_) valueStack [-1]);
				break;
			case 35:
				currentValue = _anglr_file_fragment__35 ((SyntaxTreeToken) valueStack [-2], (_anglr_syntax_rule_list_) valueStack [-1]);
				break;
			case 36:
				currentValue = _anglr_file_fragment__36 ((SyntaxTreeToken) valueStack [-2], (_anglr_syntax_rule_) valueStack [-1]);
				break;
			case 37:
				currentValue = _anglr_file_fragment__37 ((SyntaxTreeToken) valueStack [-2], (_anglr_nested_rule_) valueStack [-1]);
				break;
			case 38:
				currentValue = _anglr_file_fragment__38 ((SyntaxTreeToken) valueStack [-2], (_anglr_syntax_production_list_name_) valueStack [-1]);
				break;
			case 39:
				currentValue = _anglr_file_fragment__39 ((SyntaxTreeToken) valueStack [-2], (_anglr_syntax_production_list_) valueStack [-1]);
				break;
			case 40:
				currentValue = _anglr_file_fragment__40 ((SyntaxTreeToken) valueStack [-2], (_anglr_syntax_production_) valueStack [-1]);
				break;
			case 41:
				currentValue = _anglr_file_fragment__41 ((SyntaxTreeToken) valueStack [-2], (_priority_assoc_specification_) valueStack [-1]);
				break;
			case 42:
				currentValue = _anglr_file_fragment__42 ((SyntaxTreeToken) valueStack [-2], (_priority_specification_) valueStack [-1]);
				break;
			case 43:
				currentValue = _anglr_file_fragment__43 ((SyntaxTreeToken) valueStack [-2], (_associativity_specification_) valueStack [-1]);
				break;
			case 44:
				currentValue = _anglr_file_fragment__44 ((SyntaxTreeToken) valueStack [-2], (_production_name_) valueStack [-1]);
				break;
			case 45:
				currentValue = _anglr_file_fragment__45 ((SyntaxTreeToken) valueStack [-2], (_name_list_) valueStack [-1]);
				break;
			case 46:
				currentValue = _anglr_file_fragment__46 ((SyntaxTreeToken) valueStack [-2], (_marker_list_) valueStack [-1]);
				break;
			case 47:
				currentValue = _anglr_file_fragment__47 ((SyntaxTreeToken) valueStack [-2], (_marker_) valueStack [-1]);
				break;
			case 48:
				currentValue = _anglr_file_fragment__48 ((SyntaxTreeToken) valueStack [-2], (_g_name_) valueStack [-1]);
				break;
			case 49:
				currentValue = _anglr_file_fragment__49 ((SyntaxTreeToken) valueStack [-2], (_name_) valueStack [-1]);
				break;
			case 50:
				currentValue = _anglr_file_fragment__50 ((SyntaxTreeToken) valueStack [-2], (_cardinality_delimiter_) valueStack [-1]);
				break;
			case 51:
				currentValue = _anglr_file_fragment__51 ((SyntaxTreeToken) valueStack [-2], (_cardinality_) valueStack [-1]);
				break;
			case 52:
				currentValue = _anglr_file_fragment__52 ((SyntaxTreeToken) valueStack [-2], (_delimiter_) valueStack [-1]);
				break;
			case 53:
				currentValue = _anglr_file_fragment__53 ((SyntaxTreeToken) valueStack [-2], (_attribute_list_optional_) valueStack [-1]);
				break;
			case 54:
				currentValue = _anglr_file_fragment__54 ((SyntaxTreeToken) valueStack [-2], (_name_value_list_optional_) valueStack [-1]);
				break;
			case 55:
				currentValue = _anglr_file_fragment__55 ((SyntaxTreeToken) valueStack [-2], (_anglr_file_part_list_optional_) valueStack [-1]);
				break;
			case 56:
				currentValue = _anglr_file_fragment__56 ((SyntaxTreeToken) valueStack [-2], (_anglr_definition_list_optional_) valueStack [-1]);
				break;
			case 57:
				currentValue = _anglr_file_fragment__57 ((SyntaxTreeToken) valueStack [-2], (_block_terminal_definitions_optional_) valueStack [-1]);
				break;
			case 58:
				currentValue = _anglr_file_fragment__58 ((SyntaxTreeToken) valueStack [-2], (_block_regex_definitions_optional_) valueStack [-1]);
				break;
			case 59:
				currentValue = _anglr_file_fragment__59 ((SyntaxTreeToken) valueStack [-2], (_regular_expression_list_optional_) valueStack [-1]);
				break;
			case 60:
				currentValue = _anglr_file_fragment__60 ((SyntaxTreeToken) valueStack [-2], (_actions_optional_) valueStack [-1]);
				break;
			case 61:
				currentValue = _anglr_file_fragment__61 ((SyntaxTreeToken) valueStack [-2], (_anglr_syntax_rule_list_optional_) valueStack [-1]);
				break;
			case 62:
				currentValue = _anglr_file_fragment__62 ((SyntaxTreeToken) valueStack [-2], (_anglr_syntax_production_list_name_optional_) valueStack [-1]);
				break;
			case 63:
				currentValue = _anglr_file_fragment__63 ((SyntaxTreeToken) valueStack [-2], (_production_name_optional_) valueStack [-1]);
				break;
			case 64:
				currentValue = _anglr_file_fragment__64 ((SyntaxTreeToken) valueStack [-2], (_priority_assoc_specification_optional_) valueStack [-1]);
				break;
			case 65:
				currentValue = _anglr_file_fragment__65 ((SyntaxTreeToken) valueStack [-2], (_marker_list_optional_) valueStack [-1]);
				break;
			case 66:
				currentValue = _anglr_file_fragment__66 ((SyntaxTreeToken) valueStack [-2], (_delimiter_optional_) valueStack [-1]);
				break;
			case 67:
				currentValue = _anglr_file_fragment__67 ((SyntaxTreeToken) valueStack [-2], (_cstring_optional_) valueStack [-1]);
				break;
			case 68:
				currentValue = _anglr_file_fragment__68 ((SyntaxTreeToken) valueStack [-2], (_number_optional_) valueStack [-1]);
				break;
			case 70:
				currentValue = _attribute__1 ((SyntaxTreeToken) valueStack [-4], (SyntaxTreeToken) valueStack [-3], (_name_value_list_optional_) valueStack [-2], (SyntaxTreeToken) valueStack [-1]);
				break;
			case 72:
				currentValue = _name_value_pair__1 ((SyntaxTreeToken) valueStack [-3], (SyntaxTreeToken) valueStack [-2], (SyntaxTreeToken) valueStack [-1]);
				break;
			case 75:
				currentValue = _anglr_file_part__1 ((_general_part_) valueStack [-1]);
				break;
			case 76:
				currentValue = _anglr_file_part__2 ((_declaration_part_) valueStack [-1]);
				break;
			case 77:
				currentValue = _anglr_file_part__3 ((_scanner_part_) valueStack [-1]);
				break;
			case 78:
				currentValue = _anglr_file_part__4 ((_lexer_part_) valueStack [-1]);
				break;
			case 79:
				currentValue = _anglr_file_part__5 ((_parser_part_) valueStack [-1]);
				break;
			case 80:
				currentValue = _general_part__1 ((_attribute_list_optional_) valueStack [-6], (SyntaxTreeToken) valueStack [-5], (SyntaxTreeToken) valueStack [-4], (SyntaxTreeToken) valueStack [-3], (_attribute_list_optional_) valueStack [-2], (SyntaxTreeToken) valueStack [-1]);
				break;
			case 81:
				currentValue = _declaration_part__1 ((_attribute_list_optional_) valueStack [-6], (SyntaxTreeToken) valueStack [-5], (SyntaxTreeToken) valueStack [-4], (SyntaxTreeToken) valueStack [-3], (_anglr_definition_list_optional_) valueStack [-2], (SyntaxTreeToken) valueStack [-1]);
				break;
			case 83:
				currentValue = _anglr_definition_with_attribute__1 ((_attribute_list_optional_) valueStack [-2], (_anglr_definition_) valueStack [-1]);
				break;
			case 84:
				currentValue = _anglr_definition__1 ((_single_terminal_definition_) valueStack [-1]);
				break;
			case 85:
				currentValue = _anglr_definition__2 ((_single_regex_definition_) valueStack [-1]);
				break;
			case 86:
				currentValue = _anglr_definition__3 ((_block_of_terminal_definitions_) valueStack [-1]);
				break;
			case 87:
				currentValue = _anglr_definition__4 ((_block_of_regex_definitions_) valueStack [-1]);
				break;
			case 88:
				currentValue = _single_terminal_definition__1 ((SyntaxTreeToken) valueStack [-2], (_terminal_definition_) valueStack [-1]);
				break;
			case 89:
				currentValue = _single_regex_definition__1 ((SyntaxTreeToken) valueStack [-2], (_regex_definition_) valueStack [-1]);
				break;
			case 90:
				currentValue = _block_of_terminal_definitions__1 ((SyntaxTreeToken) valueStack [-4], (SyntaxTreeToken) valueStack [-3], (_block_terminal_definitions_optional_) valueStack [-2], (SyntaxTreeToken) valueStack [-1]);
				break;
			case 91:
				currentValue = _block_of_regex_definitions__1 ((SyntaxTreeToken) valueStack [-4], (SyntaxTreeToken) valueStack [-3], (_block_regex_definitions_optional_) valueStack [-2], (SyntaxTreeToken) valueStack [-1]);
				break;
			case 92:
				currentValue = _terminal_definition__1 ((SyntaxTreeToken) valueStack [-2], (_cstring_optional_) valueStack [-1]);
				break;
			case 93:
				currentValue = _regex_definition__1 ((SyntaxTreeToken) valueStack [-2], (SyntaxTreeToken) valueStack [-1]);
				break;
			case 95:
				currentValue = _block_terminal_definition__1 ((_attribute_list_optional_) valueStack [-2], (_terminal_definition_) valueStack [-1]);
				break;
			case 97:
				currentValue = _block_regex_definition__1 ((_attribute_list_optional_) valueStack [-2], (_regex_definition_) valueStack [-1]);
				break;
			case 98:
				currentValue = _scanner_part__1 ((_attribute_list_optional_) valueStack [-6], (SyntaxTreeToken) valueStack [-5], (SyntaxTreeToken) valueStack [-4], (SyntaxTreeToken) valueStack [-3], (_regular_expression_list_optional_) valueStack [-2], (SyntaxTreeToken) valueStack [-1]);
				break;
			case 100:
				currentValue = _regular_expression_usage__1 ((SyntaxTreeToken) valueStack [-2], (_actions_optional_) valueStack [-1]);
				break;
			case 102:
				currentValue = _action__1 ((_skip_action_) valueStack [-1]);
				break;
			case 103:
				currentValue = _action__2 ((_terminal_action_) valueStack [-1]);
				break;
			case 104:
				currentValue = _action__3 ((_event_action_) valueStack [-1]);
				break;
			case 105:
				currentValue = _action__4 ((_push_action_) valueStack [-1]);
				break;
			case 106:
				currentValue = _action__5 ((_pop_action_) valueStack [-1]);
				break;
			case 107:
				currentValue = _skip_action__1 ((SyntaxTreeToken) valueStack [-1]);
				break;
			case 108:
				currentValue = _terminal_action__1 ((SyntaxTreeToken) valueStack [-2], (SyntaxTreeToken) valueStack [-1]);
				break;
			case 109:
				currentValue = _event_action__1 ((SyntaxTreeToken) valueStack [-2], (SyntaxTreeToken) valueStack [-1]);
				break;
			case 110:
				currentValue = _push_action__1 ((SyntaxTreeToken) valueStack [-2], (SyntaxTreeToken) valueStack [-1]);
				break;
			case 111:
				currentValue = _pop_action__1 ((SyntaxTreeToken) valueStack [-1]);
				break;
			case 112:
				currentValue = _lexer_part__1 ((_attribute_list_optional_) valueStack [-6], (SyntaxTreeToken) valueStack [-5], (SyntaxTreeToken) valueStack [-4], (SyntaxTreeToken) valueStack [-3], (_attribute_list_optional_) valueStack [-2], (SyntaxTreeToken) valueStack [-1]);
				break;
			case 113:
				currentValue = _parser_part__1 ((_attribute_list_optional_) valueStack [-6], (SyntaxTreeToken) valueStack [-5], (SyntaxTreeToken) valueStack [-4], (SyntaxTreeToken) valueStack [-3], (_anglr_syntax_rule_list_optional_) valueStack [-2], (SyntaxTreeToken) valueStack [-1]);
				break;
			case 115:
				currentValue = _anglr_syntax_rule__1 ((_attribute_list_optional_) valueStack [-5], (SyntaxTreeToken) valueStack [-4], (SyntaxTreeToken) valueStack [-3], (_anglr_syntax_production_list_) valueStack [-2], (SyntaxTreeToken) valueStack [-1]);
				break;
			case 116:
				currentValue = _anglr_syntax_rule__2 ((_attribute_list_optional_) valueStack [-5], (SyntaxTreeToken) valueStack [-4], (SyntaxTreeToken) valueStack [-3], (_anglr_syntax_rule_list_optional_) valueStack [-2], (SyntaxTreeToken) valueStack [-1]);
				break;
			case 117:
				currentValue = _anglr_nested_rule__1 ((_anglr_syntax_production_list_name_optional_) valueStack [-2], (_anglr_syntax_production_list_) valueStack [-1]);
				break;
			case 118:
				currentValue = _anglr_syntax_production_list_name__1 ((SyntaxTreeToken) valueStack [-3], (SyntaxTreeToken) valueStack [-2], (SyntaxTreeToken) valueStack [-1]);
				break;
			case 121:
				currentValue = _anglr_syntax_production__1 ((_production_name_optional_) valueStack [-3], (_name_list_) valueStack [-2], (_priority_assoc_specification_optional_) valueStack [-1]);
				break;
			case 122:
				currentValue = _anglr_syntax_production__2 ((SyntaxTreeToken) valueStack [-1]);
				break;
			case 123:
				currentValue = _production_name__1 ((SyntaxTreeToken) valueStack [-2], (SyntaxTreeToken) valueStack [-1]);
				break;
			case 124:
				currentValue = _priority_assoc_specification__1 ((_priority_specification_) valueStack [-2], (_associativity_specification_) valueStack [-1]);
				break;
			case 125:
				currentValue = _priority_assoc_specification__2 ((_associativity_specification_) valueStack [-2], (_priority_specification_) valueStack [-1]);
				break;
			case 126:
				currentValue = _priority_assoc_specification__3 ((_priority_specification_) valueStack [-1]);
				break;
			case 127:
				currentValue = _priority_assoc_specification__4 ((_associativity_specification_) valueStack [-1]);
				break;
			case 128:
				currentValue = _priority_assoc_specification__5 ((_priority_specification_) valueStack [-3], (SyntaxTreeToken) valueStack [-2], (_associativity_specification_) valueStack [-1]);
				break;
			case 129:
				currentValue = _priority_assoc_specification__6 ((_associativity_specification_) valueStack [-3], (SyntaxTreeToken) valueStack [-2], (_priority_specification_) valueStack [-1]);
				break;
			case 130:
				currentValue = _priority_specification__1 ((SyntaxTreeToken) valueStack [-2], (SyntaxTreeToken) valueStack [-1]);
				break;
			case 131:
				currentValue = _priority_specification__2 ((SyntaxTreeToken) valueStack [-3], (SyntaxTreeToken) valueStack [-2], (SyntaxTreeToken) valueStack [-1]);
				break;
			case 132:
				currentValue = _associativity_specification__1 ((SyntaxTreeToken) valueStack [-2], (SyntaxTreeToken) valueStack [-1]);
				break;
			case 133:
				currentValue = _associativity_specification__2 ((SyntaxTreeToken) valueStack [-3], (SyntaxTreeToken) valueStack [-2], (SyntaxTreeToken) valueStack [-1]);
				break;
			case 134:
				currentValue = _associativity_specification__3 ((SyntaxTreeToken) valueStack [-2], (SyntaxTreeToken) valueStack [-1]);
				break;
			case 135:
				currentValue = _associativity_specification__4 ((SyntaxTreeToken) valueStack [-3], (SyntaxTreeToken) valueStack [-2], (SyntaxTreeToken) valueStack [-1]);
				break;
			case 139:
				currentValue = _marker__1 ((SyntaxTreeToken) valueStack [-2], (SyntaxTreeToken) valueStack [-1]);
				break;
			case 140:
				currentValue = _g_name__1 ((_name_) valueStack [-1]);
				break;
			case 141:
				currentValue = _g_name__2 ((SyntaxTreeToken) valueStack [-3], (_anglr_nested_rule_) valueStack [-2], (SyntaxTreeToken) valueStack [-1]);
				break;
			case 142:
				currentValue = _g_name__3 ((_g_name_) valueStack [-2], (_cardinality_delimiter_) valueStack [-1]);
				break;
			case 143:
				currentValue = _name__1 ((SyntaxTreeToken) valueStack [-1]);
				break;
			case 144:
				currentValue = _name__2 ((SyntaxTreeToken) valueStack [-1]);
				break;
			case 145:
				currentValue = _name__3 ((SyntaxTreeToken) valueStack [-1]);
				break;
			case 146:
				currentValue = _cardinality_delimiter__1 ((_cardinality_) valueStack [-2], (_delimiter_optional_) valueStack [-1]);
				break;
			case 147:
				currentValue = _cardinality__1 ((SyntaxTreeToken) valueStack [-1]);
				break;
			case 148:
				currentValue = _cardinality__2 ((SyntaxTreeToken) valueStack [-1]);
				break;
			case 149:
				currentValue = _cardinality__3 ((SyntaxTreeToken) valueStack [-1]);
				break;
			case 150:
				currentValue = _cardinality__4 ((SyntaxTreeToken) valueStack [-1]);
				break;
			case 151:
				currentValue = _cardinality__5 ((SyntaxTreeToken) valueStack [-1]);
				break;
			case 152:
				currentValue = _cardinality__6 ((SyntaxTreeToken) valueStack [-1]);
				break;
			case 153:
				currentValue = _cardinality__7 ((SyntaxTreeToken) valueStack [-1]);
				break;
			case 154:
				currentValue = _cardinality__8 ((SyntaxTreeToken) valueStack [-1]);
				break;
			case 155:
				currentValue = _cardinality__9 ((SyntaxTreeToken) valueStack [-1]);
				break;
			case 156:
				currentValue = _cardinality__10 ((SyntaxTreeToken) valueStack [-5], (_number_optional_) valueStack [-4], (SyntaxTreeToken) valueStack [-3], (_number_optional_) valueStack [-2], (SyntaxTreeToken) valueStack [-1]);
				break;
			case 157:
				currentValue = _delimiter__1 ((SyntaxTreeToken) valueStack [-3], (_anglr_nested_rule_) valueStack [-2], (SyntaxTreeToken) valueStack [-1]);
				break;
			case 158:
				currentValue = _attribute_list_optional__1 ();
				break;
			case 159:
				currentValue = _attribute_list_optional__2 ((_attribute_list_) valueStack [-1]);
				break;
			case 160:
				currentValue = _name_value_list_optional__1 ();
				break;
			case 161:
				currentValue = _name_value_list_optional__2 ((_name_value_list_) valueStack [-1]);
				break;
			case 162:
				currentValue = _anglr_file_part_list_optional__1 ();
				break;
			case 163:
				currentValue = _anglr_file_part_list_optional__2 ((_anglr_file_part_list_) valueStack [-1]);
				break;
			case 164:
				currentValue = _anglr_definition_list_optional__1 ();
				break;
			case 165:
				currentValue = _anglr_definition_list_optional__2 ((_anglr_definition_list_) valueStack [-1]);
				break;
			case 166:
				currentValue = _block_terminal_definitions_optional__1 ();
				break;
			case 167:
				currentValue = _block_terminal_definitions_optional__2 ((_block_terminal_definitions_) valueStack [-1]);
				break;
			case 168:
				currentValue = _block_regex_definitions_optional__1 ();
				break;
			case 169:
				currentValue = _block_regex_definitions_optional__2 ((_block_regex_definitions_) valueStack [-1]);
				break;
			case 170:
				currentValue = _regular_expression_list_optional__1 ();
				break;
			case 171:
				currentValue = _regular_expression_list_optional__2 ((_regular_expression_list_) valueStack [-1]);
				break;
			case 172:
				currentValue = _actions_optional__1 ();
				break;
			case 173:
				currentValue = _actions_optional__2 ((_actions_) valueStack [-1]);
				break;
			case 174:
				currentValue = _anglr_syntax_rule_list_optional__1 ();
				break;
			case 175:
				currentValue = _anglr_syntax_rule_list_optional__2 ((_anglr_syntax_rule_list_) valueStack [-1]);
				break;
			case 176:
				currentValue = _anglr_syntax_production_list_name_optional__1 ();
				break;
			case 177:
				currentValue = _anglr_syntax_production_list_name_optional__2 ((_anglr_syntax_production_list_name_) valueStack [-1]);
				break;
			case 178:
				currentValue = _production_name_optional__1 ();
				break;
			case 179:
				currentValue = _production_name_optional__2 ((_production_name_) valueStack [-1]);
				break;
			case 180:
				currentValue = _priority_assoc_specification_optional__1 ();
				break;
			case 181:
				currentValue = _priority_assoc_specification_optional__2 ((_priority_assoc_specification_) valueStack [-1]);
				break;
			case 182:
				currentValue = _marker_list_optional__1 ();
				break;
			case 183:
				currentValue = _marker_list_optional__2 ((_marker_list_) valueStack [-1]);
				break;
			case 184:
				currentValue = _delimiter_optional__1 ();
				break;
			case 185:
				currentValue = _delimiter_optional__2 ((_delimiter_) valueStack [-1]);
				break;
			case 186:
				currentValue = _cstring_optional__1 ();
				break;
			case 187:
				currentValue = _cstring_optional__2 ((SyntaxTreeToken) valueStack [-1]);
				break;
			case 188:
				currentValue = _number_optional__1 ();
				break;
			case 189:
				currentValue = _number_optional__2 ((SyntaxTreeToken) valueStack [-1]);
				break;
			case 190:
				currentValue = _attribute_list__1 ((_attribute_) valueStack [-1]);
				break;
			case 191:
				currentValue = _attribute_list__2 ((_attribute_list_) valueStack [-2], (_attribute_) valueStack [-1]);
				break;
			case 194:
				currentValue = _name_value_list__1 ((_name_value_pair_) valueStack [-1]);
				break;
			case 195:
				currentValue = _name_value_list__2 ((_name_value_list_) valueStack [-2], (_name_value_pair_) valueStack [-1]);
				break;
			case 196:
				currentValue = _anglr_file__1 ();
				break;
			case 197:
				currentValue = _anglr_file__2 ((_anglr_file_part_list_) valueStack [-1]);
				break;
			case 198:
				currentValue = _anglr_file_part_list__1 ((_anglr_file_part_) valueStack [-1]);
				break;
			case 199:
				currentValue = _anglr_file_part_list__2 ((_anglr_file_part_list_) valueStack [-2], (_anglr_file_part_) valueStack [-1]);
				break;
			case 208:
				currentValue = _anglr_definition_list__1 ((_anglr_definition_with_attribute_) valueStack [-1]);
				break;
			case 209:
				currentValue = _anglr_definition_list__2 ((_anglr_definition_list_) valueStack [-2], (_anglr_definition_with_attribute_) valueStack [-1]);
				break;
			case 218:
				currentValue = _block_terminal_definitions__1 ((_block_terminal_definition_) valueStack [-1]);
				break;
			case 219:
				currentValue = _block_terminal_definitions__2 ((_block_terminal_definitions_) valueStack [-2], (_block_terminal_definition_) valueStack [-1]);
				break;
			case 222:
				currentValue = _block_regex_definitions__1 ((_block_regex_definition_) valueStack [-1]);
				break;
			case 223:
				currentValue = _block_regex_definitions__2 ((_block_regex_definitions_) valueStack [-2], (_block_regex_definition_) valueStack [-1]);
				break;
			case 230:
				currentValue = _regular_expression_list__1 ((_regular_expression_usage_) valueStack [-1]);
				break;
			case 231:
				currentValue = _regular_expression_list__2 ((_regular_expression_list_) valueStack [-2], (_regular_expression_usage_) valueStack [-1]);
				break;
			case 234:
				currentValue = _actions__1 ((_action_) valueStack [-1]);
				break;
			case 235:
				currentValue = _actions__2 ((_actions_) valueStack [-2], (_action_) valueStack [-1]);
				break;
			case 244:
				currentValue = _anglr_syntax_rule_list__1 ((_anglr_syntax_rule_) valueStack [-1]);
				break;
			case 245:
				currentValue = _anglr_syntax_rule_list__2 ((_anglr_syntax_rule_list_) valueStack [-2], (_anglr_syntax_rule_) valueStack [-1]);
				break;
			case 254:
				currentValue = _anglr_syntax_production_list__1 ((_anglr_syntax_production_) valueStack [-1]);
				break;
			case 255:
				currentValue = _anglr_syntax_production_list__2 ((_anglr_syntax_production_list_) valueStack [-3], (SyntaxTreeToken) valueStack [-2], (_anglr_syntax_production_) valueStack [-1]);
				break;
			case 262:
				currentValue = _name_list__1 ((_marker_list_optional_) valueStack [-1]);
				break;
			case 263:
				currentValue = _name_list__2 ((_name_list_) valueStack [-3], (_g_name_) valueStack [-2], (_marker_list_optional_) valueStack [-1]);
				break;
			case 264:
				currentValue = _marker_list__1 ((_marker_) valueStack [-1]);
				break;
			case 265:
				currentValue = _marker_list__2 ((_marker_list_) valueStack [-2], (_marker_) valueStack [-1]);
				break;
			case 272:
			break;
			}
			return currentValue;
		}

		void ReportClassCounters ()
		{
			if (_anglr_file_fragment_.g_counter != 0)
				AnglrLogger?.DebugRawLine ("_anglr_file_fragment_.g_counter = " + _anglr_file_fragment_.g_counter);
			if (_attribute_list_.g_counter != 0)
				AnglrLogger?.DebugRawLine ("_attribute_list_.g_counter = " + _attribute_list_.g_counter);
			if (_attribute_.g_counter != 0)
				AnglrLogger?.DebugRawLine ("_attribute_.g_counter = " + _attribute_.g_counter);
			if (_name_value_list_.g_counter != 0)
				AnglrLogger?.DebugRawLine ("_name_value_list_.g_counter = " + _name_value_list_.g_counter);
			if (_name_value_pair_.g_counter != 0)
				AnglrLogger?.DebugRawLine ("_name_value_pair_.g_counter = " + _name_value_pair_.g_counter);
			if (_anglr_file_.g_counter != 0)
				AnglrLogger?.DebugRawLine ("_anglr_file_.g_counter = " + _anglr_file_.g_counter);
			if (_anglr_file_part_list_.g_counter != 0)
				AnglrLogger?.DebugRawLine ("_anglr_file_part_list_.g_counter = " + _anglr_file_part_list_.g_counter);
			if (_anglr_file_part_.g_counter != 0)
				AnglrLogger?.DebugRawLine ("_anglr_file_part_.g_counter = " + _anglr_file_part_.g_counter);
			if (_general_part_.g_counter != 0)
				AnglrLogger?.DebugRawLine ("_general_part_.g_counter = " + _general_part_.g_counter);
			if (_declaration_part_.g_counter != 0)
				AnglrLogger?.DebugRawLine ("_declaration_part_.g_counter = " + _declaration_part_.g_counter);
			if (_anglr_definition_list_.g_counter != 0)
				AnglrLogger?.DebugRawLine ("_anglr_definition_list_.g_counter = " + _anglr_definition_list_.g_counter);
			if (_anglr_definition_with_attribute_.g_counter != 0)
				AnglrLogger?.DebugRawLine ("_anglr_definition_with_attribute_.g_counter = " + _anglr_definition_with_attribute_.g_counter);
			if (_anglr_definition_.g_counter != 0)
				AnglrLogger?.DebugRawLine ("_anglr_definition_.g_counter = " + _anglr_definition_.g_counter);
			if (_single_terminal_definition_.g_counter != 0)
				AnglrLogger?.DebugRawLine ("_single_terminal_definition_.g_counter = " + _single_terminal_definition_.g_counter);
			if (_single_regex_definition_.g_counter != 0)
				AnglrLogger?.DebugRawLine ("_single_regex_definition_.g_counter = " + _single_regex_definition_.g_counter);
			if (_block_of_terminal_definitions_.g_counter != 0)
				AnglrLogger?.DebugRawLine ("_block_of_terminal_definitions_.g_counter = " + _block_of_terminal_definitions_.g_counter);
			if (_block_of_regex_definitions_.g_counter != 0)
				AnglrLogger?.DebugRawLine ("_block_of_regex_definitions_.g_counter = " + _block_of_regex_definitions_.g_counter);
			if (_terminal_definition_.g_counter != 0)
				AnglrLogger?.DebugRawLine ("_terminal_definition_.g_counter = " + _terminal_definition_.g_counter);
			if (_regex_definition_.g_counter != 0)
				AnglrLogger?.DebugRawLine ("_regex_definition_.g_counter = " + _regex_definition_.g_counter);
			if (_block_terminal_definitions_.g_counter != 0)
				AnglrLogger?.DebugRawLine ("_block_terminal_definitions_.g_counter = " + _block_terminal_definitions_.g_counter);
			if (_block_terminal_definition_.g_counter != 0)
				AnglrLogger?.DebugRawLine ("_block_terminal_definition_.g_counter = " + _block_terminal_definition_.g_counter);
			if (_block_regex_definitions_.g_counter != 0)
				AnglrLogger?.DebugRawLine ("_block_regex_definitions_.g_counter = " + _block_regex_definitions_.g_counter);
			if (_block_regex_definition_.g_counter != 0)
				AnglrLogger?.DebugRawLine ("_block_regex_definition_.g_counter = " + _block_regex_definition_.g_counter);
			if (_scanner_part_.g_counter != 0)
				AnglrLogger?.DebugRawLine ("_scanner_part_.g_counter = " + _scanner_part_.g_counter);
			if (_regular_expression_list_.g_counter != 0)
				AnglrLogger?.DebugRawLine ("_regular_expression_list_.g_counter = " + _regular_expression_list_.g_counter);
			if (_regular_expression_usage_.g_counter != 0)
				AnglrLogger?.DebugRawLine ("_regular_expression_usage_.g_counter = " + _regular_expression_usage_.g_counter);
			if (_actions_.g_counter != 0)
				AnglrLogger?.DebugRawLine ("_actions_.g_counter = " + _actions_.g_counter);
			if (_action_.g_counter != 0)
				AnglrLogger?.DebugRawLine ("_action_.g_counter = " + _action_.g_counter);
			if (_skip_action_.g_counter != 0)
				AnglrLogger?.DebugRawLine ("_skip_action_.g_counter = " + _skip_action_.g_counter);
			if (_terminal_action_.g_counter != 0)
				AnglrLogger?.DebugRawLine ("_terminal_action_.g_counter = " + _terminal_action_.g_counter);
			if (_event_action_.g_counter != 0)
				AnglrLogger?.DebugRawLine ("_event_action_.g_counter = " + _event_action_.g_counter);
			if (_push_action_.g_counter != 0)
				AnglrLogger?.DebugRawLine ("_push_action_.g_counter = " + _push_action_.g_counter);
			if (_pop_action_.g_counter != 0)
				AnglrLogger?.DebugRawLine ("_pop_action_.g_counter = " + _pop_action_.g_counter);
			if (_lexer_part_.g_counter != 0)
				AnglrLogger?.DebugRawLine ("_lexer_part_.g_counter = " + _lexer_part_.g_counter);
			if (_parser_part_.g_counter != 0)
				AnglrLogger?.DebugRawLine ("_parser_part_.g_counter = " + _parser_part_.g_counter);
			if (_anglr_syntax_rule_list_.g_counter != 0)
				AnglrLogger?.DebugRawLine ("_anglr_syntax_rule_list_.g_counter = " + _anglr_syntax_rule_list_.g_counter);
			if (_anglr_syntax_rule_.g_counter != 0)
				AnglrLogger?.DebugRawLine ("_anglr_syntax_rule_.g_counter = " + _anglr_syntax_rule_.g_counter);
			if (_anglr_nested_rule_.g_counter != 0)
				AnglrLogger?.DebugRawLine ("_anglr_nested_rule_.g_counter = " + _anglr_nested_rule_.g_counter);
			if (_anglr_syntax_production_list_name_.g_counter != 0)
				AnglrLogger?.DebugRawLine ("_anglr_syntax_production_list_name_.g_counter = " + _anglr_syntax_production_list_name_.g_counter);
			if (_anglr_syntax_production_list_.g_counter != 0)
				AnglrLogger?.DebugRawLine ("_anglr_syntax_production_list_.g_counter = " + _anglr_syntax_production_list_.g_counter);
			if (_anglr_syntax_production_.g_counter != 0)
				AnglrLogger?.DebugRawLine ("_anglr_syntax_production_.g_counter = " + _anglr_syntax_production_.g_counter);
			if (_production_name_.g_counter != 0)
				AnglrLogger?.DebugRawLine ("_production_name_.g_counter = " + _production_name_.g_counter);
			if (_priority_assoc_specification_.g_counter != 0)
				AnglrLogger?.DebugRawLine ("_priority_assoc_specification_.g_counter = " + _priority_assoc_specification_.g_counter);
			if (_priority_specification_.g_counter != 0)
				AnglrLogger?.DebugRawLine ("_priority_specification_.g_counter = " + _priority_specification_.g_counter);
			if (_associativity_specification_.g_counter != 0)
				AnglrLogger?.DebugRawLine ("_associativity_specification_.g_counter = " + _associativity_specification_.g_counter);
			if (_name_list_.g_counter != 0)
				AnglrLogger?.DebugRawLine ("_name_list_.g_counter = " + _name_list_.g_counter);
			if (_marker_list_.g_counter != 0)
				AnglrLogger?.DebugRawLine ("_marker_list_.g_counter = " + _marker_list_.g_counter);
			if (_marker_.g_counter != 0)
				AnglrLogger?.DebugRawLine ("_marker_.g_counter = " + _marker_.g_counter);
			if (_g_name_.g_counter != 0)
				AnglrLogger?.DebugRawLine ("_g_name_.g_counter = " + _g_name_.g_counter);
			if (_name_.g_counter != 0)
				AnglrLogger?.DebugRawLine ("_name_.g_counter = " + _name_.g_counter);
			if (_cardinality_delimiter_.g_counter != 0)
				AnglrLogger?.DebugRawLine ("_cardinality_delimiter_.g_counter = " + _cardinality_delimiter_.g_counter);
			if (_cardinality_.g_counter != 0)
				AnglrLogger?.DebugRawLine ("_cardinality_.g_counter = " + _cardinality_.g_counter);
			if (_delimiter_.g_counter != 0)
				AnglrLogger?.DebugRawLine ("_delimiter_.g_counter = " + _delimiter_.g_counter);
			if (_attribute_list_optional_.g_counter != 0)
				AnglrLogger?.DebugRawLine ("_attribute_list_optional_.g_counter = " + _attribute_list_optional_.g_counter);
			if (_name_value_list_optional_.g_counter != 0)
				AnglrLogger?.DebugRawLine ("_name_value_list_optional_.g_counter = " + _name_value_list_optional_.g_counter);
			if (_anglr_file_part_list_optional_.g_counter != 0)
				AnglrLogger?.DebugRawLine ("_anglr_file_part_list_optional_.g_counter = " + _anglr_file_part_list_optional_.g_counter);
			if (_anglr_definition_list_optional_.g_counter != 0)
				AnglrLogger?.DebugRawLine ("_anglr_definition_list_optional_.g_counter = " + _anglr_definition_list_optional_.g_counter);
			if (_block_terminal_definitions_optional_.g_counter != 0)
				AnglrLogger?.DebugRawLine ("_block_terminal_definitions_optional_.g_counter = " + _block_terminal_definitions_optional_.g_counter);
			if (_block_regex_definitions_optional_.g_counter != 0)
				AnglrLogger?.DebugRawLine ("_block_regex_definitions_optional_.g_counter = " + _block_regex_definitions_optional_.g_counter);
			if (_regular_expression_list_optional_.g_counter != 0)
				AnglrLogger?.DebugRawLine ("_regular_expression_list_optional_.g_counter = " + _regular_expression_list_optional_.g_counter);
			if (_actions_optional_.g_counter != 0)
				AnglrLogger?.DebugRawLine ("_actions_optional_.g_counter = " + _actions_optional_.g_counter);
			if (_anglr_syntax_rule_list_optional_.g_counter != 0)
				AnglrLogger?.DebugRawLine ("_anglr_syntax_rule_list_optional_.g_counter = " + _anglr_syntax_rule_list_optional_.g_counter);
			if (_anglr_syntax_production_list_name_optional_.g_counter != 0)
				AnglrLogger?.DebugRawLine ("_anglr_syntax_production_list_name_optional_.g_counter = " + _anglr_syntax_production_list_name_optional_.g_counter);
			if (_production_name_optional_.g_counter != 0)
				AnglrLogger?.DebugRawLine ("_production_name_optional_.g_counter = " + _production_name_optional_.g_counter);
			if (_priority_assoc_specification_optional_.g_counter != 0)
				AnglrLogger?.DebugRawLine ("_priority_assoc_specification_optional_.g_counter = " + _priority_assoc_specification_optional_.g_counter);
			if (_marker_list_optional_.g_counter != 0)
				AnglrLogger?.DebugRawLine ("_marker_list_optional_.g_counter = " + _marker_list_optional_.g_counter);
			if (_delimiter_optional_.g_counter != 0)
				AnglrLogger?.DebugRawLine ("_delimiter_optional_.g_counter = " + _delimiter_optional_.g_counter);
			if (_cstring_optional_.g_counter != 0)
				AnglrLogger?.DebugRawLine ("_cstring_optional_.g_counter = " + _cstring_optional_.g_counter);
			if (_number_optional_.g_counter != 0)
				AnglrLogger?.DebugRawLine ("_number_optional_.g_counter = " + _number_optional_.g_counter);
			if (SyntaxTreeToken.g_counter != 0)
				AnglrLogger?.DebugRawLine ("SyntaxTreeToken.g_counter = " + SyntaxTreeToken.g_counter);
		}

		bool ParserInterface.checkLoopDetection () => loopDetection;
		bool ParserInterface.checkDebug () => debug;
		bool ParserInterface.checkCreateParseTree () => createParseTree;
		ParserToken ParserInterface.getToken () => token;
		int ParserInterface.magicNumber () => g_magicNumber;
		int ParserInterface.minTerminalCode () => g_minTerminalCode;
		int [] ParserInterface.terminalCodes () => g_terminalCodes;
		string [] ParserInterface.terminalNames () => g_terminalNames;
		int ParserInterface.minNonTerminalCode () => g_minNonTerminalCode;
		int [] ParserInterface.nonTerminalCodes () => g_nonTerminalCodes;
		string [] ParserInterface.nonTerminalNames () => g_nonTerminalNames;
		int [] ParserInterface.glrcheck () => g_glrcheck;
		int [] ParserInterface.glrstate () => g_glrstate;
		int [] ParserInterface.glrcells () => g_glrcells;
		int [] ParserInterface.check () => g_check;
		int [] ParserInterface.state () => g_state;
		int [] ParserInterface.shiftDelta () => g_shiftDelta;
		int [] ParserInterface.gotoDelta () => g_gotoDelta;
		int [] ParserInterface.productionLengths () => g_productionLengths;
		int [] ParserInterface.productionRules () => g_productionRules;
		int [] ParserInterface.defaultGoto () => g_defaultGoto;
		int [] ParserInterface.rcheck () => g_rcheck;
		int [] ParserInterface.rstate () => g_rstate;
		int [] ParserInterface.reductions () => g_reductions;

		public static bool debug { get; set; } = false;
		public static bool createParseTree { get; set; } = false;
		public static bool loopDetection { get; set; } = false;

		private static bool g_debug = false;
		private static bool g_createParseTree = false;
		private static bool g_loopDetection = false;

		public ParserToken token { get; private set; }
		public stackset stackSet { get; private set; }
		public parselist parseList { get; private set; }

		internal readonly static int g_magicNumber = 2629197;

		internal readonly static int g_minTerminalCode = 258;
		internal readonly static int[] g_terminalCodes = 
		{
			   258,    259,    260,    261,    262,    263,    264,    265,    266,    267, 
			   268,    269,    270,    271,    272,    273,    274,    275,    276,    277, 
			   278,    279,    280,    281,    282,    283,    284,    285,    286,    287, 
			   288,    289,    290,    291,    292,    293,    294,    295,    296,    297, 
			   298,    299,    300,    301,    302,    303,    304,    305,    306,    307, 
			   308,    309,    310,    311,    312,    313,    314,    315,    316,    317, 
			   318,    319,    320,    321,    322,    323,    324,    325,    326,    327, 
			   328,    329,    330,    331,    332,    333,    334,    335,    336,    337, 
			   338,    339,    340,    341,    342,    343,    344,    345,    346,    347, 
			   348,    349,    350,    351,    352,    353,    354,    355,    356,    357, 
			   358,    359,    360,    361,    362,    363,    364,    365,    366,    367, 
			   368,    369, 
		};

		internal readonly static string[] g_terminalNames = 
		{
			"<vertical bar>",
			"<comma>",
			"<left curly bracket>",
			"<right curly bracket>",
			"<left bracket>",
			"<right bracket>",
			"<left part bracket>",
			"<right part bracket>",
			"<left square bracket>",
			"<right square bracket>",
			"<double at sign>",
			"<at sign>",
			"<equals sign>",
			"<colon>",
			"<semicolon>",
			"<question mark>",
			"<plus sign>",
			"<minus sign>",
			"<asterisk>",
			"<slash>",
			"<inv plus sign>",
			"<inv minus sign>",
			"<inv asterisk>",
			"<inv slash>",
			"<any>",
			"<cstring>",
			"<empty>",
			"<identifier>",
			"<terminal>",
			"<general>",
			"<declarations>",
			"<regex>",
			"<scanner>",
			"<lexer>",
			"<parser>",
			"<priority>",
			"<associativity>",
			"<regular expression>",
			"<number>",
			"<skip>",
			"<ttoken>",
			"<event>",
			"<push>",
			"<pop>",
			"<anglr file terminal>",
			"<anglr file part list terminal>",
			"<anglr file part terminal>",
			"<general part terminal>",
			"<declaration part terminal>",
			"<scanner part terminal>",
			"<regular expression list terminal>",
			"<regular expression usage terminal>",
			"<actions terminal>",
			"<action terminal>",
			"<skip action terminal>",
			"<terminal action terminal>",
			"<event action terminal>",
			"<push action terminal>",
			"<pop action terminal>",
			"<lexer part terminal>",
			"<parser part terminal>",
			"<attribute list terminal>",
			"<attribute terminal>",
			"<name value list terminal>",
			"<name value pair terminal>",
			"<anglr definition list terminal>",
			"<anglr definition with attribute list terminal>",
			"<anglr definition terminal>",
			"<single terminal definition terminal>",
			"<single regex definition terminal>",
			"<block of terminal definitions terminal>",
			"<block of regex definitions terminal>",
			"<terminal definition terminal>",
			"<regex definition terminal>",
			"<block terminal definitions terminal>",
			"<block terminal definition terminal>",
			"<block regex definitions terminal>",
			"<block regex definition terminal>",
			"<anglr syntax rule list terminal>",
			"<anglr syntax rule terminal>",
			"<anglr syntax production list terminal>",
			"<anglr syntax production terminal>",
			"<priority assoc specification terminal>",
			"<priority specification terminal>",
			"<associativity specification terminal>",
			"<anglr nested rule terminal>",
			"<anglr syntax production list name terminal>",
			"<name list terminal>",
			"<production name terminal>",
			"<marker list terminal>",
			"<marker terminal>",
			"<g name terminal>",
			"<name terminal>",
			"<cardinality delimiter terminal>",
			"<cardinality terminal>",
			"<delimiter terminal>",
			"<attribute list optional terminal>",
			"<name value list optional terminal>",
			"<anglr file part list optional terminal>",
			"<anglr definition list optional terminal>",
			"<block terminal definitions optional terminal>",
			"<block regex definitions optional terminal>",
			"<regular expression list optional terminal>",
			"<actions optional terminal>",
			"<anglr syntax rule list optional terminal>",
			"<anglr syntax production list name optional terminal>",
			"<production name optional terminal>",
			"<priority assoc specification optional terminal>",
			"<marker list optional terminal>",
			"<delimiter optional terminal>",
			"<cstring optional terminal>",
			"<number optional terminal>",
		};

		internal readonly static int g_minNonTerminalCode = 370;
		internal readonly static int[] g_nonTerminalCodes = 
		{
			   370,    371,    372,    373,    374,    375,    376,    377,    378,    379, 
			   380,    381,    382,    383,    384,    385,    386,    387,    388,    389, 
			   390,    391,    392,    393,    394,    395,    396,    397,    398,    399, 
			   400,    401,    402,    403,    404,    405,    406,    407,    408,    409, 
			   410,    411,    412,    413,    414,    415,    416,    417,    418,    419, 
			   420,    421,    422,    423,    424,    425,    426,    427,    428,    429, 
			   430,    431,    432,    433,    434,    435,    436,    437,    438, 
		};

		internal readonly static string[] g_nonTerminalNames = 
		{
			"<anglr file fragment>",
			"<attribute list>",
			"<attribute>",
			"<name value list>",
			"<name value pair>",
			"<anglr file>",
			"<anglr file part list>",
			"<anglr file part>",
			"<general part>",
			"<declaration part>",
			"<anglr definition list>",
			"<anglr definition with attribute>",
			"<anglr definition>",
			"<single terminal definition>",
			"<single regex definition>",
			"<block of terminal definitions>",
			"<block of regex definitions>",
			"<terminal definition>",
			"<regex definition>",
			"<block terminal definitions>",
			"<block terminal definition>",
			"<block regex definitions>",
			"<block regex definition>",
			"<scanner part>",
			"<regular expression list>",
			"<regular expression usage>",
			"<actions>",
			"<action>",
			"<skip action>",
			"<terminal action>",
			"<event action>",
			"<push action>",
			"<pop action>",
			"<lexer part>",
			"<parser part>",
			"<anglr syntax rule list>",
			"<anglr syntax rule>",
			"<anglr nested rule>",
			"<anglr syntax production list name>",
			"<anglr syntax production list>",
			"<anglr syntax production>",
			"<production name>",
			"<priority assoc specification>",
			"<priority specification>",
			"<associativity specification>",
			"<name list>",
			"<marker list>",
			"<marker>",
			"<g name>",
			"<name>",
			"<cardinality delimiter>",
			"<cardinality>",
			"<delimiter>",
			"<attribute list optional>",
			"<name value list optional>",
			"<anglr file part list optional>",
			"<anglr definition list optional>",
			"<block terminal definitions optional>",
			"<block regex definitions optional>",
			"<regular expression list optional>",
			"<actions optional>",
			"<anglr syntax rule list optional>",
			"<anglr syntax production list name optional>",
			"<production name optional>",
			"<priority assoc specification optional>",
			"<marker list optional>",
			"<delimiter optional>",
			"<cstring optional>",
			"<number optional>",
		};

		internal readonly static int[] g_glrcheck =
		{
		};

		internal readonly static int[] g_glrstate =
		{
		};

		internal readonly static int[] g_glrcells =
		{
		};

		internal readonly static int[] g_check =
		{
			     0,      0,      0,      0,      0,      0,      0,      0,      0,      0, 
			     0,      0,      0,      0,      0,      1,      0,      0,      0,      0, 
			     0,      0,      0,      0,      0,      0,      0,      0,      0,      0, 
			     0,      0,      0,      0,      0,      0,      0,      0,      0,      0, 
			     0,      0,      0,      0,      0,      0,      0,      0,      0,      0, 
			     0,      0,      0,      0,      0,      0,      0,      0,      0,      0, 
			     0,      0,      0,      0,      0,      0,      0,      0,      0,      0, 
			     0,      0,      0,      0,      0,      0,      0,      0,      0,      0, 
			     0,      0,      0,      0,      0,      0,      0,      0,      0,      0, 
			     0,      0,      0,      0,      0,      0,      0,      0,      0,      0, 
			     0,      0,      0,      0,      0,      0,      0,      0,      0,      0, 
			     0,      0,      0,      0,      0,      0,      0,      0,      0,      0, 
			     0,      0,      0,      0,      0,      0,      0,      0,      0,      0, 
			     0,      0,      0,      0,      0,      0,      0,      0,      0,      0, 
			     0,      0,      0,      0,      0,      0,      0,      0,      0,      0, 
			     0,      0,      0,      0,      0,      0,      0,      0,      0,      0, 
			     0,      0,      0,      0,      0,      0,      0,      0,      0,      0, 
			     0,      0,      0,      0,      0,      0,      0,      0,      0,      0, 
			     0,      0,      0,      0,      0,      0,      0,      0,      0,      0, 
			     0,      0,      0,      0,      0,      0,      0,      0,      0,      0, 
			     0,      0,      0,      0,      0,      0,      0,      0,      0,      0, 
			     0,      0,      0,      0,      0,      0,      0,      0,      0,      0, 
			     0,      0,      0,      0,      0,      0,      0,      0,      0,      0, 
			     0,      0,      0,      0,      0,      0,      0,      0,      0,      0, 
			     0,      0,      0,      0,      0,      0,      0,      0,      0,      0, 
			     0,      0,      0,      0,      0,      0,      0,      0,      0,      0, 
			     0,      0,      0,      0,      0,      0,      0,      0,    266,      0, 
			     0,      0,      0,      0,      0,      0,      0,    266,    268,    259, 
			   271,    260,    260,    268,    258,    269,    263,      0,    285,    270, 
			   286,    286,    267,    289,    286,    289,    289,    285,    285,    284, 
			   283,    271,    267,    302,    303,    304,    305,    306,    307,    308, 
			   309,    310,    311,    312,    313,    314,    315,    316,    317,    318, 
			   319,    320,    321,    322,    323,    324,    325,    326,    327,    328, 
			   329,    330,    331,    332,    333,    334,    335,    336,    337,    338, 
			   339,    340,    341,    342,    343,    344,    345,    346,    347,    348, 
			   349,    350,    351,    352,    353,    354,    355,    356,    357,    358, 
			   359,    360,    361,    362,    363,    364,    365,    366,    367,    368, 
			   369,    295,    297,    298,    299,    300,    301,    297,    299,    298, 
			   293,    294,    300,    293,    301,    294,    262,    282,    283,    296, 
			   285,    285,    288,    295,    287,    287,    288,    260,    290,    291, 
			   292,    260,    260,    290,    285,    285,    282,    283,    285,    285, 
			   273,    274,    275,    276,    277,    278,    279,    280,    281,    291, 
			   285,    292,    270,    285,    285,    270,    285,    285,    259,    285, 
			   259,    283,    285,    260,    285,    285,    285,    262,    283,    285, 
			   285,    296,    264,    283,    271,    285,    264,    264,    296,    264, 
			   264,    261,    261,    258,    261,    261,    371,    282,    283,    373, 
			   285,    260,    265,    294,    293,    265,    372,    272,    293,    294, 
			   269,    265,    265,    265,    273,    274,    275,    276,    277,    278, 
			   279,    280,    281,    374,    381,    383,    376,    386,    377,    389, 
			   378,    392,    395,    379,    398,    380,    401,    405,    410,    421, 
			   384,    412,    385,    419,    432,    387,    433,    437,      0,    388, 
			   436,    434,    390,      0,      0,    423,    391,      0,      0,    393, 
			     0,    394,    423,    423,      0,      0,    423,    396,      0,    397, 
			   400,      0,    399,      0,      0,    423,    402,    423,    423,    403, 
			     0,    376,      0,      0,    404,    423,    372,      0,    423,    423, 
			   374,    408,    406,      0,    377,      0,      0,      0,      0,    423, 
			   381,      0,    382,    423,      0,      0,    413,      0,    411,    423, 
			   414,    390,    388,      0,    416,    387,    417,      0,      0,    418, 
			     0,    392,    420,      0,      0,    422,      0,    423,      0,    423, 
			   423,    423,      0,    423,    395,    397,      0,      0,    406,      0, 
			     0,      0,    423,      0,    423,    409,      0,    435,      0,      0, 
			     0,      0,    423,      0,      0,    423,    414,    430,    415,    407, 
			   407,    417,    413,      0,    413,    410,      0,      0,    409,      0, 
			     0,    414,      0,      0,    424,      0,      0,    423,      0,    423, 
			     0,    427,      0,    438,    428,    423,    423,    423,    423,      0, 
			   426,    423,      0,    435,    431,    429,      0,      0,    438,    431, 
		};

		internal readonly static int[] g_state =
		{
			     0,      0,      0,      0,      0,      0,      0,      0,      0,      0, 
			     0,      0,      0,      0,      0,    234,      0,      0,      0,      0, 
			     0,      0,      0,      0,      0,      0,      0,      0,      0,      0, 
			     0,      0,      0,      0,      0,      0,      0,      0,      0,      0, 
			     0,      0,      0,      0,      0,      0,      0,      0,      0,      0, 
			     0,      0,      0,      0,      0,      0,      0,      0,      0,      0, 
			     0,      0,      0,      0,      0,      0,      0,      0,      0,      0, 
			     0,      0,      0,      0,      0,      0,      0,      0,      0,      0, 
			     0,      0,      0,      0,      0,      0,      0,      0,      0,      0, 
			     0,      0,      0,      0,      0,      0,      0,      0,      0,      0, 
			     0,      0,      0,      0,      0,      0,      0,      0,      0,      0, 
			     0,      0,      0,      0,      0,      0,      0,      0,      0,      0, 
			     0,      0,      0,      0,      0,      0,      0,      0,      0,      0, 
			     0,      0,      0,      0,      0,      0,      0,      0,      0,      0, 
			     0,      0,      0,      0,      0,      0,      0,      0,      0,      0, 
			     0,      0,      0,      0,      0,      0,      0,      0,      0,      0, 
			     0,      0,      0,      0,      0,      0,      0,      0,      0,      0, 
			     0,      0,      0,      0,      0,      0,      0,      0,      0,      0, 
			     0,      0,      0,      0,      0,      0,      0,      0,      0,      0, 
			     0,      0,      0,      0,      0,      0,      0,      0,      0,      0, 
			     0,      0,      0,      0,      0,      0,      0,      0,      0,      0, 
			     0,      0,      0,      0,      0,      0,      0,      0,      0,      0, 
			     0,      0,      0,      0,      0,      0,      0,      0,      0,      0, 
			     0,      0,      0,      0,      0,      0,      0,      0,      0,      0, 
			     0,      0,      0,      0,      0,      0,      0,      0,      0,      0, 
			     0,      0,      0,      0,      0,      0,      0,      0,      0,      0, 
			     0,      0,      0,      0,      0,      0,      0,      0,     70,      0, 
			     0,      0,      0,      0,      0,      0,      0,    204,    163,    308, 
			   157,    247,    249,    163,    268,    177,    307,      0,     74,    237, 
			    98,    105,    309,     99,    109,    107,    111,    113,    115,    162, 
			   230,    298,    310,      5,      6,      7,      8,      9,     23,     24, 
			    25,     26,     27,     28,     29,     30,     31,     32,     33,     34, 
			     1,      2,      3,      4,     10,     11,     12,     13,     14,     15, 
			    16,     17,     18,     19,     20,     21,     22,     35,     36,     39, 
			    40,     41,     42,     43,     37,     38,     45,     44,     46,     47, 
			    48,     49,     50,     51,     52,     53,     54,     55,     56,     57, 
			    58,     59,     60,     61,     62,     63,     64,     65,     66,     67, 
			    68,    127,    131,    132,    133,    134,    135,    131,    133,    132, 
			   169,    170,    134,    169,    135,    170,    184,    185,    186,    232, 
			   187,    235,    241,    252,    240,    240,    241,    200,    242,    243, 
			   244,    247,    249,    242,    259,    260,    185,    186,    261,    187, 
			   191,    192,    193,    194,    195,    196,    197,    198,    199,    243, 
			   264,    244,    271,    265,    267,    273,    113,    115,    275,    279, 
			   277,    288,    289,    297,    290,    291,    292,    184,    272,    293, 
			   274,    301,    311,    302,    296,    303,    312,    313,    270,    314, 
			   315,    316,    317,    268,    327,    328,     71,    185,    186,     75, 
			   187,    200,    329,    170,    169,    330,     73,    326,    169,    170, 
			   177,    331,    332,    333,    191,    192,    193,    194,    195,    196, 
			   197,    198,    199,     77,     97,    106,     88,    112,     89,    117, 
			    90,    124,    130,     92,    144,     94,    147,    153,    168,    203, 
			   108,    171,    110,    190,    223,    114,    224,    251,      0,    116, 
			   285,    300,    120,      0,      0,    155,    121,      0,      0,    125, 
			     0,    128,    123,    119,      0,      0,     96,    136,      0,    143, 
			   146,      0,    145,      0,      0,     91,    148,     93,     96,    149, 
			     0,    210,      0,      0,    151,    119,    236,      0,    123,    126, 
			   238,    161,    156,      0,    239,      0,      0,      0,      0,    150, 
			   245,      0,    246,    152,      0,      0,    174,      0,    176,    155, 
			   175,    253,    256,      0,    182,    254,    183,      0,      0,    188, 
			     0,    255,    201,      0,      0,    205,      0,    206,      0,     96, 
			   119,    123,      0,    155,    258,    262,      0,      0,    263,      0, 
			     0,      0,     96,      0,    119,    266,      0,    227,      0,      0, 
			     0,      0,    123,      0,      0,    155,    276,    257,    269,    282, 
			   286,    281,    278,      0,    305,    299,      0,      0,    318,      0, 
			     0,    304,      0,      0,    287,      0,      0,    119,      0,    123, 
			     0,    294,      0,    284,    295,    321,    155,     96,    324,      0, 
			   322,    155,      0,    306,    319,    323,      0,      0,    320,    325, 
		};

		internal readonly static int[] g_shiftDelta =
		{
			     1,      2,      2,      3,      3,      2,      2,      2,      2,      2, 
			     2,      2,      4,      5,      6,      8,      7,     12,     13,      2, 
			     2,      2,      2,      2,     76,     76,     75,     75,     80,     81, 
			    79,     82,     83,      2,      2,      2,      2,      9,      9,     15, 
			    15,     87,     90,     91,     10,     16,     16,     16,    124,    105, 
			   137,    137,     11,      2,      3,      2,      2,      2,      2,     76, 
			    75,      2,      9,     10,     87,     16,     11,     17,     93,     14, 
			   106,      2,      0,      0,     19,      3,      0,      0,      0,      2, 
			     0,      0,      0,      0,      0,      0,    108,      2,      2,      0, 
			     0,    107,      0,    104,      2,      0,      4,      0,    141,    142, 
			     0,      0,      0,      0,      0,     12,      0,     13,      0,     21, 
			     0,     22,      0,     17,      0,     98,      0,      2,      0,     12, 
			     0,      2,      0,     13,      0,      0,    113,     75,     76,      0, 
			     0,      0,    119,    120,    123,      0,     75,      0,      0,      0, 
			     0,      0,      0,      0,      0,      0,      0,      0,      0,      0, 
			   128,      0,    129,      2,      0,    135,      0,    138,      0,     15, 
			     0,      0,      0,    139,     26,      0,     16,      0,      0,    152, 
			   155,      0,    169,    171,      0,      0,      0,    144,    124,      0, 
			    16,      0,     16,      0,      9,      0,      0,      0,    137,      0, 
			     0,      0,      0,      0,      0,      0,      0,      0,      0,      0, 
			    93,      0,     11,      0,      9,      0,      0,      0,      3,      0, 
			     2,      0,      2,      0,      2,      0,      2,      0,     76,      0, 
			    75,      0,      2,      0,      0,      0,      0,      0,      0,      0, 
			     0,      0,      0,      0,      0,      3,      0,    148,      0,      0, 
			   147,    149,    150,    151,    154,      0,      0,      2,      0,      2, 
			     0,      0,      0,      0,      0,      0,      0,      0,      0,      0, 
			     0,      0,      0,      0,    173,     30,     26,      0,     15,    175, 
			     0,    145,      0,    160,      0,     91,      0,     90,      0,      0, 
			   201,      0,     23,      0,     20,      0,     25,     35,      0,    178, 
			   182,    183,    185,    186,    190,    191,     15,      2,      0,      0, 
			     0,      0,      0,      0,      0,      0,      0,      0,     93,      0, 
			     0,      2,      2,     76,      2,      2,      0,      0,    195,    193, 
			   194,    197,    200,    206,    207,    208,      0,      0,      0,      0, 
			     0,      0,      0,      0, 
		};

		internal readonly static int[] g_gotoDelta =
		{
			    24,     85,     94,     86,    109,     27,    110,    111,    112,    114, 
			   115,    103,     28,    102,    116,    117,    101,    118,    121,    100, 
			   122,    125,     99,    126,    127,     97,    131,    132,     96,    133, 
			   130,     95,    134,    136,    140,     92,    146,     29,    143,     31, 
			    88,     89,    153,    156,    157,     32,    158,    159,    161,     84, 
			   162,     78,    163,    164,     33,    165,    166,    167,    168,     34, 
			    36,    170,     72,     73,     37,    172,     38,     39,     40,      0, 
			     0,    174,      0,      0,      0,    176,      0,      0,      0,    177, 
			     0,      0,      0,      0,      0,      0,      0,    174,    177,      0, 
			     0,      0,      0,      0,    179,      0,    180,      0,     42,     43, 
			     0,      0,      0,      0,      0,     42,      0,     43,      0,      0, 
			     0,      0,      0,     70,      0,      0,      0,    181,      0,    188, 
			     0,    189,      0,    184,      0,      0,      0,    187,    199,      0, 
			     0,      0,      0,      0,      0,      0,    198,      0,      0,      0, 
			     0,      0,      0,      0,      0,      0,      0,      0,      0,      0, 
			     0,      0,      0,    192,      0,      0,      0,      0,      0,    196, 
			     0,      0,      0,      0,      0,      0,    203,      0,      0,      0, 
			     0,      0,    202,    209,      0,      0,      0,      0,     44,      0, 
			   204,      0,    204,      0,    212,      0,      0,      0,     45,      0, 
			     0,      0,      0,      0,      0,      0,      0,      0,      0,      0, 
			   205,      0,     74,      0,    213,      0,      0,      0,    176,      0, 
			   177,      0,    179,      0,    181,      0,    189,      0,    199,      0, 
			   198,      0,    192,      0,      0,      0,      0,      0,      0,      0, 
			     0,      0,      0,      0,      0,    210,      0,      0,      0,      0, 
			     0,      0,      0,      0,      0,      0,      0,    214,      0,    216, 
			     0,      0,      0,      0,      0,      0,      0,      0,      0,      0, 
			     0,      0,      0,      0,      0,      0,      0,      0,    215,     77, 
			     0,      0,      0,      0,      0,    217,      0,    211,      0,      0, 
			   218,      0,      0,      0,      0,      0,      0,      0,      0,      0, 
			     0,      0,      0,      0,      0,      0,    219,    223,      0,      0, 
			     0,      0,      0,      0,      0,      0,      0,      0,    220,      0, 
			     0,    222,    224,    226,    225,    228,      0,      0,      0,      0, 
			     0,      0,      0,      0,      0,      0,      0,      0,      0,      0, 
			     0,      0,      0,      0, 
		};

		internal readonly static int[] g_productionLengths = 
		{
			     0,      2,      2,      2,      2,      2,      2,      2,      2,      2, 
			     2,      2,      2,      2,      2,      2,      2,      2,      2,      2, 
			     2,      2,      2,      2,      2,      2,      2,      2,      2,      2, 
			     2,      2,      2,      2,      2,      2,      2,      2,      2,      2, 
			     2,      2,      2,      2,      2,      2,      2,      2,      2,      2, 
			     2,      2,      2,      2,      2,      2,      2,      2,      2,      2, 
			     2,      2,      2,      2,      2,      2,      2,      2,      2,      0, 
			     4,      0,      3,      0,      0,      1,      1,      1,      1,      1, 
			     6,      6,      0,      2,      1,      1,      1,      1,      2,      2, 
			     4,      4,      2,      2,      0,      2,      0,      2,      6,      0, 
			     2,      0,      1,      1,      1,      1,      1,      1,      2,      2, 
			     2,      1,      6,      6,      0,      5,      5,      2,      3,      0, 
			     0,      3,      1,      2,      2,      2,      1,      1,      3,      3, 
			     2,      3,      2,      3,      2,      3,      0,      0,      0,      2, 
			     1,      3,      2,      1,      1,      1,      2,      1,      1,      1, 
			     1,      1,      1,      1,      1,      1,      5,      3,      0,      1, 
			     0,      1,      0,      1,      0,      1,      0,      1,      0,      1, 
			     0,      1,      0,      1,      0,      1,      0,      1,      0,      1, 
			     0,      1,      0,      1,      0,      1,      0,      1,      0,      1, 
			     1,      2,      0,      0,      1,      2,      0,      1,      1,      2, 
			     0,      0,      0,      0,      0,      0,      0,      0,      1,      2, 
			     0,      0,      0,      0,      0,      0,      0,      0,      1,      2, 
			     0,      0,      1,      2,      0,      0,      0,      0,      0,      0, 
			     1,      2,      0,      0,      1,      2,      0,      0,      0,      0, 
			     0,      0,      0,      0,      1,      2,      0,      0,      0,      0, 
			     0,      0,      0,      0,      1,      3,      0,      0,      0,      0, 
			     0,      0,      1,      3,      1,      2,      0,      0,      0,      0, 
			     0,      0,      2, 
		};

		internal readonly static int[] g_productionRules = 
		{
			     0,    370,    370,    370,    370,    370,    370,    370,    370,    370, 
			   370,    370,    370,    370,    370,    370,    370,    370,    370,    370, 
			   370,    370,    370,    370,    370,    370,    370,    370,    370,    370, 
			   370,    370,    370,    370,    370,    370,    370,    370,    370,    370, 
			   370,    370,    370,    370,    370,    370,    370,    370,    370,    370, 
			   370,    370,    370,    370,    370,    370,    370,    370,    370,    370, 
			   370,    370,    370,    370,    370,    370,    370,    370,    370,      0, 
			   372,      0,    374,      0,      0,    377,    377,    377,    377,    377, 
			   378,    379,      0,    381,    382,    382,    382,    382,    383,    384, 
			   385,    386,    387,    388,      0,    390,      0,    392,    393,      0, 
			   395,      0,    397,    397,    397,    397,    397,    398,    399,    400, 
			   401,    402,    403,    404,      0,    406,    406,    407,    408,      0, 
			     0,    410,    410,    411,    412,    412,    412,    412,    412,    412, 
			   413,    413,    414,    414,    414,    414,      0,      0,      0,    417, 
			   418,    418,    418,    419,    419,    419,    420,    421,    421,    421, 
			   421,    421,    421,    421,    421,    421,    421,    422,    423,    423, 
			   424,    424,    425,    425,    426,    426,    427,    427,    428,    428, 
			   429,    429,    430,    430,    431,    431,    432,    432,    433,    433, 
			   434,    434,    435,    435,    436,    436,    437,    437,    438,    438, 
			   371,    371,      0,      0,    373,    373,    375,    375,    376,    376, 
			     0,      0,      0,      0,      0,      0,      0,      0,    380,    380, 
			     0,      0,      0,      0,      0,      0,      0,      0,    389,    389, 
			     0,      0,    391,    391,      0,      0,      0,      0,      0,      0, 
			   394,    394,      0,      0,    396,    396,      0,      0,      0,      0, 
			     0,      0,      0,      0,    405,    405,      0,      0,      0,      0, 
			     0,      0,      0,      0,    409,    409,      0,      0,      0,      0, 
			     0,      0,    415,    415,    416,    416,      0,      0,      0,      0, 
			     0,      0,      0, 
		};

		internal readonly static int[] g_defaultGoto = 
		{
			    -1,      0,      0,      0,      0,      0,      0,      0,      0,      0, 
			     0,      0,      0,      0,      0,      0,      0,      0,      0,      0, 
			     0,      0,      0,      0,      0,      0,      0,      0,      0,      0, 
			     0,      0,      0,      0,      0,      0,      0,      0,      0,      0, 
			     0,      0,      0,      0,      0,      0,      0,      0,      0,      0, 
			     0,      0,      0,      0,      0,      0,      0,      0,      0,      0, 
			     0,      0,      0,      0,      0,      0,      0,      0,      0,      0, 
			     0,      0,      0,      0,      0,      0,      0,      0,      0,      0, 
			     0,      0,      0,      0,      0,      0,      0,      0,      0,      0, 
			     0,      0,      0,      0,      0,      0,      0,      0,      0,      0, 
			     0,      0,      0,      0,      0,      0,      0,      0,      0,      0, 
			     0,      0,      0,      0,      0,      0,      0,      0,      0,      0, 
			     0,      0,      0,      0,      0,      0,      0,      0,      0,      0, 
			     0,      0,      0,      0,      0,      0,      0,      0,      0,      0, 
			     0,      0,      0,      0,      0,      0,      0,      0,      0,      0, 
			     0,      0,      0,      0,      0,      0,      0,      0,      0,      0, 
			     0,      0,      0,      0,      0,      0,      0,      0,      0,      0, 
			     0,      0,      0,      0,      0,      0,      0,      0,      0,      0, 
			     0,      0,      0,      0,      0,      0,      0,      0,      0,      0, 
			     0,      0,      0,      0,      0,      0,      0,      0,      0,      0, 
			     0,      0,      0,      0,      0,      0,      0,      0,      0,      0, 
			     0,      0,      0,      0,      0,      0,      0,      0,      0,      0, 
			     0,      0,      0,      0,      0,      0,      0,      0,      0,      0, 
			     0,      0,      0,      0,      0,      0,      0,      0,      0,      0, 
			     0,      0,      0,      0,      0,      0,      0,      0,      0,      0, 
			     0,      0,      0,      0,      0,      0,      0,      0,      0,      0, 
			     0,      0,      0,      0,      0,      0,      0,      0,      0,      0, 
			     0,      0,      0,      0,      0,      0,      0,      0,      0,      0, 
			     0,      0,      0,      0,      0,      0,      0,      0,      0,      0, 
			     0,      0,      0,      0,      0,      0,      0,      0,      0,      0, 
			     0,      0,      0,      0,      0,      0,      0,      0,      0,      0, 
			     0,      0,      0,      0,      0,      0,      0,      0,      0,      0, 
			     0,      0,      0,      0,      0,      0,      0,      0,      0,      0, 
			     0,      0,      0,      0,      0,      0,      0,      0,      0,      0, 
			     0,      0,      0,      0,      0,      0,      0,      0,      0,      0, 
			     0,      0,      0,      0,      0,      0,      0,      0,      0,      0, 
			     0,      0,      0,      0,      0,      0,      0,      0,      0,      0, 
			    69,     87,     72,    208,     76,     78,     79,     80,     81,     82, 
			   212,     95,    100,    101,    102,    103,    104,    248,    250,    214, 
			   118,    216,    122,     83,    218,    129,    220,    137,    138,    139, 
			   140,    141,    142,     84,     85,    222,    154,    158,    160,    164, 
			   165,    167,    226,    172,    173,    178,    180,    181,    280,    189, 
			   283,    202,    229,     86,    207,    209,    211,    213,    215,    217, 
			   219,    221,    159,    166,    225,    179,    228,    231,    233, 
		};

		internal readonly static int[] g_rcheck =
		{
			     0,      0,      1,      1,      0,      0,      1,      0,      1,      0, 
			     0,      0,      1,      0,      0,      1,      0,      1,      1,      0, 
			     0,      0,      1,      0,      1,      0,      0,      0,      1,      0, 
			     0,      1,      0,      0,      1,      0,      0,      0,      0,      0, 
			     0,      0,      1,      0,      0,      0,      0,      0,      0,      0, 
			     0,      0,      0,      1,      0,      0,      0,      0,      1,      0, 
			     0,      0,      0,      0,      0,      0,      0,      1,      0,      0, 
			     0,      0,      0,      0,      0,      0,      0,      0,      0,      0, 
			     0,      0,      0,      0,      0,      0,      0,      0,      0,      0, 
			     0,      0,      0,      0,      0,      0,      0,      0,      0,      0, 
			     0,      0,      0,      0,      0,      0,      0,      0,      0,      0, 
			     0,      0,      0,      0,      0,      0,      0,      0,      0,      0, 
			     0,      0,      0,      0,      0,      0,      0,      0,      0,      0, 
			     0,      0,      0,      0,      0,      0,      0,      0,      0,      0, 
			     0,      0,      0,      0,      0,      0,      0,      0,      0,      0, 
			     0,      0,      0,      0,      0,      0,      0,      0,      0,      0, 
			     0,      0,      0,      0,      0,      0,      0,      0,      0,      0, 
			     0,      0,      0,      0,      0,      0,      0,      0,      0,      0, 
			     0,      0,      0,      0,      0,      0,      0,      0,      0,      0, 
			     0,      0,      0,      0,      0,      0,      0,      0,      0,      0, 
			     0,      0,      0,      0,      0,      0,      0,      0,      0,      0, 
			     0,      0,      0,      0,      0,      0,      0,      0,      0,      0, 
			     0,      0,      0,      0,      0,      0,      0,      0,      0,      0, 
			     0,      0,      0,      0,      0,      0,      0,      0,      0,      0, 
			     0,      0,      0,      0,      0,      0,      0,      0,      0,      0, 
			     0,      0,      0,      0,      0,      0,      0,      0,      0,      0, 
			     0,      0,      0,      0,      0,      0,      0,      0,      0,      0, 
			     0,      0,      0,      0,      0,      0,      0,      0,      0,      0, 
			     0,      0,      0,      0,      0,      0,      0,    285,    287,    288, 
			   285,    290,    291,    292,    287,    288,    285,    290,    291,    292, 
			   286,    285,    261,    289,    287,    288,    285,    290,    291,    292, 
			   287,    288,    285,    290,    291,    292,    286,    265,    261,    289, 
			   287,    288,    261,    290,    291,    292,    285,    261,    261,    261, 
			   265,    265,      0,      0,    265,      0,      0,      0,    286,      0, 
			     0,    289,    285,      0,      0,      0,    285,      0,      0,      0, 
			   285,    285,    285,    285,      0,    286,      0,      0,    289, 
		};

		internal readonly static int[] g_rstate =
		{
			     0,      0,    196,    166,      0,      0,    168,      0,    162,      0, 
			     0,      0,    174,      0,      0,    164,      0,     19,    197,      0, 
			     0,      0,     21,      0,      6,      0,      0,      0,     35,      0, 
			     0,     10,      0,      0,    163,      0,      0,      0,      0,      0, 
			     0,      0,    167,      0,      0,      0,      0,      0,      0,      0, 
			     0,      0,      0,    165,      0,      0,      0,      0,    169,      0, 
			     0,      0,      0,      0,      0,      0,      0,    175,      0,      0, 
			     0,      0,      0,      0,      0,      0,      0,      0,      0,      0, 
			     0,      0,      0,      0,      0,      0,      0,      0,      0,      0, 
			     0,      0,      0,      0,      0,      0,      0,      0,      0,      0, 
			     0,      0,      0,      0,      0,      0,      0,      0,      0,      0, 
			     0,      0,      0,      0,      0,      0,      0,      0,      0,      0, 
			     0,      0,      0,      0,      0,      0,      0,      0,      0,      0, 
			     0,      0,      0,      0,      0,      0,      0,      0,      0,      0, 
			     0,      0,      0,      0,      0,      0,      0,      0,      0,      0, 
			     0,      0,      0,      0,      0,      0,      0,      0,      0,      0, 
			     0,      0,      0,      0,      0,      0,      0,      0,      0,      0, 
			     0,      0,      0,      0,      0,      0,      0,      0,      0,      0, 
			     0,      0,      0,      0,      0,      0,      0,      0,      0,      0, 
			     0,      0,      0,      0,      0,      0,      0,      0,      0,      0, 
			     0,      0,      0,      0,      0,      0,      0,      0,      0,      0, 
			     0,      0,      0,      0,      0,      0,      0,      0,      0,      0, 
			     0,      0,      0,      0,      0,      0,      0,      0,      0,      0, 
			     0,      0,      0,      0,      0,      0,      0,      0,      0,      0, 
			     0,      0,      0,      0,      0,      0,      0,      0,      0,      0, 
			     0,      0,      0,      0,      0,      0,      0,      0,      0,      0, 
			     0,      0,      0,      0,      0,      0,      0,      0,      0,      0, 
			     0,      0,      0,      0,      0,      0,      0,      0,      0,      0, 
			     0,      0,      0,      0,      0,      0,      0,    158,    158,    158, 
			   158,    158,    158,    158,    158,    158,    158,    158,    158,    158, 
			   158,    158,    167,    158,    158,    158,    158,    158,    158,    158, 
			   158,    158,    158,    158,    158,    158,    158,    165,    169,    158, 
			   158,    158,    166,    158,    158,    158,    158,    175,    168,    174, 
			   174,    175,      0,      0,    164,      0,      0,      0,    158,      0, 
			     0,    158,    158,      0,      0,      0,    158,      0,      0,      0, 
			   158,    158,    158,    158,      0,    158,      0,      0,    158, 
		};

		internal readonly static int[] g_reductions =
		{
			     0,      0,      0,      0,      0,     -1,    158,    158,    158,    158, 
			   158,    158,      0,      0,      0,      0,      0,      0,      0,    158, 
			   158,    158,    158,    158,      0,      0,      0,      0,      0,      0, 
			     0,      0,      0,    158,    158,    158,    158,    176,      0,    178, 
			   178,      0,      0,      0,      0,    182,      0,      0,      0,      0, 
			     0,      0,      0,    158,    160,     -7,    -14,     -2,     -5,    170, 
			   172,    -11,    176,    178,    180,    182,    184,    186,    188,      0, 
			     0,      1,    190,      2,      0,      3,    194,      4,      5,    -17, 
			   198,     75,     76,     77,     78,     79,      0,    159,    -23,      7, 
			     8,      0,      9,      0,    -30,    208,      0,     11,      0,      0, 
			    12,     84,     85,     86,     87,      0,     13,      0,     14,      0, 
			    15,      0,     16,    186,     17,      0,     18,    -16,    218,      0, 
			    20,    -21,    222,      0,     22,     23,      0,    172,     24,    230, 
			    25,    107,      0,      0,      0,    111,     26,    234,    102,    103, 
			   104,    105,    106,     27,     28,     29,     30,     31,     32,     33, 
			     0,     34,      0,    -27,    244,      0,     36,      0,     37,    178, 
			   177,     38,    122,      0,     39,    254,    182,    179,     40,      0, 
			     0,     41,    126,    127,     42,     43,     44,      0,     45,    262, 
			   183,    264,     46,     47,    176,    143,    144,    145,     48,    140, 
			    49,    147,    148,    149,    150,    151,    152,    153,    154,    155, 
			   188,     50,    184,     51,    176,     52,     53,     54,    161,     55, 
			   -33,     56,    -52,     57,    -41,     58,    -57,     59,    171,     60, 
			   173,     61,    -66,     62,     63,     64,    181,     65,     66,    185, 
			   187,     67,    189,     68,    272,    160,    191,      0,    195,    199, 
			     0,      0,      0,      0,      0,    209,     83,    -61,     88,    -67, 
			    89,     92,     93,    219,     95,    223,     97,    100,    231,    108, 
			   109,    110,    235,    245,      0,      0,    117,    123,    178,    180, 
			   130,      0,    132,      0,    134,      0,    124,      0,    125,    139, 
			   182,    265,      0,    142,      0,    146,      0,      0,     72,      0, 
			     0,      0,      0,      0,      0,      0,    178,    -68,    118,    255, 
			   121,    131,    133,    135,    128,    129,    263,    141,    188,    157, 
			    70,    158,    -69,    170,    158,    -65,     90,     91,      0,      0, 
			     0,      0,      0,      0,      0,      0,    115,    116,    156,     80, 
			    81,     98,    112,    113, 
		};

	}

	public partial class SyntaxTreeWalker : Anglr.Parser.Walker.SyntaxTreeWalkerCore { }

	public enum ProductionID
	{
		InvalidProductionID = 0,
		__anglr_file_fragment__ID = 370,
		__attribute_list__ID = 371,
		__attribute__ID = 372,
		__name_value_list__ID = 373,
		__name_value_pair__ID = 374,
		__anglr_file__ID = 375,
		__anglr_file_part_list__ID = 376,
		__anglr_file_part__ID = 377,
		__general_part__ID = 378,
		__declaration_part__ID = 379,
		__anglr_definition_list__ID = 380,
		__anglr_definition_with_attribute__ID = 381,
		__anglr_definition__ID = 382,
		__single_terminal_definition__ID = 383,
		__single_regex_definition__ID = 384,
		__block_of_terminal_definitions__ID = 385,
		__block_of_regex_definitions__ID = 386,
		__terminal_definition__ID = 387,
		__regex_definition__ID = 388,
		__block_terminal_definitions__ID = 389,
		__block_terminal_definition__ID = 390,
		__block_regex_definitions__ID = 391,
		__block_regex_definition__ID = 392,
		__scanner_part__ID = 393,
		__regular_expression_list__ID = 394,
		__regular_expression_usage__ID = 395,
		__actions__ID = 396,
		__action__ID = 397,
		__skip_action__ID = 398,
		__terminal_action__ID = 399,
		__event_action__ID = 400,
		__push_action__ID = 401,
		__pop_action__ID = 402,
		__lexer_part__ID = 403,
		__parser_part__ID = 404,
		__anglr_syntax_rule_list__ID = 405,
		__anglr_syntax_rule__ID = 406,
		__anglr_nested_rule__ID = 407,
		__anglr_syntax_production_list_name__ID = 408,
		__anglr_syntax_production_list__ID = 409,
		__anglr_syntax_production__ID = 410,
		__production_name__ID = 411,
		__priority_assoc_specification__ID = 412,
		__priority_specification__ID = 413,
		__associativity_specification__ID = 414,
		__name_list__ID = 415,
		__marker_list__ID = 416,
		__marker__ID = 417,
		__g_name__ID = 418,
		__name__ID = 419,
		__cardinality_delimiter__ID = 420,
		__cardinality__ID = 421,
		__delimiter__ID = 422,
		__attribute_list_optional__ID = 423,
		__name_value_list_optional__ID = 424,
		__anglr_file_part_list_optional__ID = 425,
		__anglr_definition_list_optional__ID = 426,
		__block_terminal_definitions_optional__ID = 427,
		__block_regex_definitions_optional__ID = 428,
		__regular_expression_list_optional__ID = 429,
		__actions_optional__ID = 430,
		__anglr_syntax_rule_list_optional__ID = 431,
		__anglr_syntax_production_list_name_optional__ID = 432,
		__production_name_optional__ID = 433,
		__priority_assoc_specification_optional__ID = 434,
		__marker_list_optional__ID = 435,
		__delimiter_optional__ID = 436,
		__cstring_optional__ID = 437,
		__number_optional__ID = 438,
		LastProductionID
	};

	public static class AnglrFragments
	{
		public static readonly (string, ProductionID, int, int, string) [] FragmentsInfo =
		{
			("<anglr file fragment>", ProductionID.__anglr_file_fragment__ID, 0, 1, "_eof_token"),
			("<attribute list>", ProductionID.__attribute_list__ID, 1, 266, "_left_square_bracket_"),
			("<attribute>", ProductionID.__attribute__ID, 2, 1, "_eof_token"),
			("<name value list>", ProductionID.__name_value_list__ID, 3, 285, "_identifier_"),
			("<name value pair>", ProductionID.__name_value_pair__ID, 4, 1, "_eof_token"),
			("<anglr file>", ProductionID.__anglr_file__ID, 5, 1, "_eof_token"),
			("<anglr file part list>", ProductionID.__anglr_file_part_list__ID, 6, 287, "_general_"),
			("<anglr file part>", ProductionID.__anglr_file_part__ID, 7, 1, "_eof_token"),
			("<general part>", ProductionID.__general_part__ID, 8, 1, "_eof_token"),
			("<declaration part>", ProductionID.__declaration_part__ID, 9, 1, "_eof_token"),
			("<scanner part>", ProductionID.__scanner_part__ID, 23, 1, "_eof_token"),
			("<lexer part>", ProductionID.__lexer_part__ID, 33, 1, "_eof_token"),
			("<parser part>", ProductionID.__parser_part__ID, 34, 1, "_eof_token"),
			("<attribute list optional>", ProductionID.__attribute_list_optional__ID, 53, 1, "_eof_token"),
			("<anglr definition list>", ProductionID.__anglr_definition_list__ID, 10, 266, "_left_square_bracket_"),
			("<anglr definition with attribute>", ProductionID.__anglr_definition_with_attribute__ID, 11, 1, "_eof_token"),
			("<anglr definition>", ProductionID.__anglr_definition__ID, 12, 1, "_eof_token"),
			("<single terminal definition>", ProductionID.__single_terminal_definition__ID, 13, 1, "_eof_token"),
			("<single regex definition>", ProductionID.__single_regex_definition__ID, 14, 1, "_eof_token"),
			("<block of terminal definitions>", ProductionID.__block_of_terminal_definitions__ID, 15, 1, "_eof_token"),
			("<block of regex definitions>", ProductionID.__block_of_regex_definitions__ID, 16, 1, "_eof_token"),
			("<terminal definition>", ProductionID.__terminal_definition__ID, 17, 1, "_eof_token"),
			("<regex definition>", ProductionID.__regex_definition__ID, 18, 1, "_eof_token"),
			("<block terminal definitions>", ProductionID.__block_terminal_definitions__ID, 19, 285, "_identifier_"),
			("<block terminal definition>", ProductionID.__block_terminal_definition__ID, 20, 1, "_eof_token"),
			("<block regex definitions>", ProductionID.__block_regex_definitions__ID, 21, 285, "_identifier_"),
			("<block regex definition>", ProductionID.__block_regex_definition__ID, 22, 1, "_eof_token"),
			("<regular expression list>", ProductionID.__regular_expression_list__ID, 24, 295, "_regular_expression_"),
			("<regular expression usage>", ProductionID.__regular_expression_usage__ID, 25, 1, "_eof_token"),
			("<actions>", ProductionID.__actions__ID, 26, 297, "_skip_"),
			("<action>", ProductionID.__action__ID, 27, 1, "_eof_token"),
			("<skip action>", ProductionID.__skip_action__ID, 28, 1, "_eof_token"),
			("<terminal action>", ProductionID.__terminal_action__ID, 29, 1, "_eof_token"),
			("<event action>", ProductionID.__event_action__ID, 30, 1, "_eof_token"),
			("<push action>", ProductionID.__push_action__ID, 31, 1, "_eof_token"),
			("<pop action>", ProductionID.__pop_action__ID, 32, 1, "_eof_token"),
			("<anglr syntax rule list>", ProductionID.__anglr_syntax_rule_list__ID, 35, 285, "_identifier_"),
			("<anglr syntax rule>", ProductionID.__anglr_syntax_rule__ID, 36, 1, "_eof_token"),
			("<anglr nested rule>", ProductionID.__anglr_nested_rule__ID, 37, 1, "_eof_token"),
			("<anglr syntax production list name optional>", ProductionID.__anglr_syntax_production_list_name_optional__ID, 62, 1, "_eof_token"),
			("<anglr syntax production list name>", ProductionID.__anglr_syntax_production_list_name__ID, 38, 1, "_eof_token"),
			("<anglr syntax production list>", ProductionID.__anglr_syntax_production_list__ID, 39, 258, "_vertical_bar_"),
			("<anglr syntax production>", ProductionID.__anglr_syntax_production__ID, 40, 1, "_eof_token"),
			("<production name optional>", ProductionID.__production_name_optional__ID, 63, 1, "_eof_token"),
			("<production name>", ProductionID.__production_name__ID, 44, 1, "_eof_token"),
			("<priority assoc specification>", ProductionID.__priority_assoc_specification__ID, 41, 1, "_eof_token"),
			("<priority specification>", ProductionID.__priority_specification__ID, 42, 1, "_eof_token"),
			("<associativity specification>", ProductionID.__associativity_specification__ID, 43, 1, "_eof_token"),
			("<name list>", ProductionID.__name_list__ID, 45, 262, "_left_bracket_"),
			("<marker list optional>", ProductionID.__marker_list_optional__ID, 65, 1, "_eof_token"),
			("<marker list>", ProductionID.__marker_list__ID, 46, 269, "_at_sign_"),
			("<marker>", ProductionID.__marker__ID, 47, 1, "_eof_token"),
			("<g name>", ProductionID.__g_name__ID, 48, 273, "_question_mark_"),
			("<name>", ProductionID.__name__ID, 49, 1, "_eof_token"),
			("<cardinality delimiter>", ProductionID.__cardinality_delimiter__ID, 50, 1, "_eof_token"),
			("<cardinality>", ProductionID.__cardinality__ID, 51, 1, "_eof_token"),
			("<delimiter>", ProductionID.__delimiter__ID, 52, 1, "_eof_token"),
			("<name value list optional>", ProductionID.__name_value_list_optional__ID, 54, 1, "_eof_token"),
			("<anglr file part list optional>", ProductionID.__anglr_file_part_list_optional__ID, 55, 1, "_eof_token"),
			("<anglr definition list optional>", ProductionID.__anglr_definition_list_optional__ID, 56, 1, "_eof_token"),
			("<block terminal definitions optional>", ProductionID.__block_terminal_definitions_optional__ID, 57, 1, "_eof_token"),
			("<block regex definitions optional>", ProductionID.__block_regex_definitions_optional__ID, 58, 1, "_eof_token"),
			("<regular expression list optional>", ProductionID.__regular_expression_list_optional__ID, 59, 1, "_eof_token"),
			("<actions optional>", ProductionID.__actions_optional__ID, 60, 1, "_eof_token"),
			("<anglr syntax rule list optional>", ProductionID.__anglr_syntax_rule_list_optional__ID, 61, 1, "_eof_token"),
			("<priority assoc specification optional>", ProductionID.__priority_assoc_specification_optional__ID, 64, 1, "_eof_token"),
			("<delimiter optional>", ProductionID.__delimiter_optional__ID, 66, 1, "_eof_token"),
			("<cstring optional>", ProductionID.__cstring_optional__ID, 67, 1, "_eof_token"),
			("<number optional>", ProductionID.__number_optional__ID, 68, 1, "_eof_token"),
		};

		public static Dictionary<string, (int, int, int, string)> _FragmentsInfoDictionary = FragmentsInfo.ToDictionary (x => x.Item1, x => ((int) x.Item2, x.Item3, x.Item4, x.Item5));

		public static (int, int, int, string) GetFragmentInfo (string fragmentName) =>
			_FragmentsInfoDictionary.TryGetValue (fragmentName ?? "", out var fragment) ?
			fragment :
			(0, -1, -1, "");
	}

	public class AnglrParser_TEST : SyntaxTreeWalker
	{
		public AnglrParser_TEST ()
		{
			//
			// event registration templates
			//
			Common_Event += Invoke_Common_Callback;
			_anglr_file_fragment__Event += Invoke__anglr_file_fragment__Callback;
			_attribute_list__Event += Invoke__attribute_list__Callback;
			_attribute__Event += Invoke__attribute__Callback;
			_name_value_list__Event += Invoke__name_value_list__Callback;
			_name_value_pair__Event += Invoke__name_value_pair__Callback;
			_anglr_file__Event += Invoke__anglr_file__Callback;
			_anglr_file_part_list__Event += Invoke__anglr_file_part_list__Callback;
			_anglr_file_part__Event += Invoke__anglr_file_part__Callback;
			_general_part__Event += Invoke__general_part__Callback;
			_declaration_part__Event += Invoke__declaration_part__Callback;
			_anglr_definition_list__Event += Invoke__anglr_definition_list__Callback;
			_anglr_definition_with_attribute__Event += Invoke__anglr_definition_with_attribute__Callback;
			_anglr_definition__Event += Invoke__anglr_definition__Callback;
			_single_terminal_definition__Event += Invoke__single_terminal_definition__Callback;
			_single_regex_definition__Event += Invoke__single_regex_definition__Callback;
			_block_of_terminal_definitions__Event += Invoke__block_of_terminal_definitions__Callback;
			_block_of_regex_definitions__Event += Invoke__block_of_regex_definitions__Callback;
			_terminal_definition__Event += Invoke__terminal_definition__Callback;
			_regex_definition__Event += Invoke__regex_definition__Callback;
			_block_terminal_definitions__Event += Invoke__block_terminal_definitions__Callback;
			_block_terminal_definition__Event += Invoke__block_terminal_definition__Callback;
			_block_regex_definitions__Event += Invoke__block_regex_definitions__Callback;
			_block_regex_definition__Event += Invoke__block_regex_definition__Callback;
			_scanner_part__Event += Invoke__scanner_part__Callback;
			_regular_expression_list__Event += Invoke__regular_expression_list__Callback;
			_regular_expression_usage__Event += Invoke__regular_expression_usage__Callback;
			_actions__Event += Invoke__actions__Callback;
			_action__Event += Invoke__action__Callback;
			_skip_action__Event += Invoke__skip_action__Callback;
			_terminal_action__Event += Invoke__terminal_action__Callback;
			_event_action__Event += Invoke__event_action__Callback;
			_push_action__Event += Invoke__push_action__Callback;
			_pop_action__Event += Invoke__pop_action__Callback;
			_lexer_part__Event += Invoke__lexer_part__Callback;
			_parser_part__Event += Invoke__parser_part__Callback;
			_anglr_syntax_rule_list__Event += Invoke__anglr_syntax_rule_list__Callback;
			_anglr_syntax_rule__Event += Invoke__anglr_syntax_rule__Callback;
			_anglr_nested_rule__Event += Invoke__anglr_nested_rule__Callback;
			_anglr_syntax_production_list_name__Event += Invoke__anglr_syntax_production_list_name__Callback;
			_anglr_syntax_production_list__Event += Invoke__anglr_syntax_production_list__Callback;
			_anglr_syntax_production__Event += Invoke__anglr_syntax_production__Callback;
			_production_name__Event += Invoke__production_name__Callback;
			_priority_assoc_specification__Event += Invoke__priority_assoc_specification__Callback;
			_priority_specification__Event += Invoke__priority_specification__Callback;
			_associativity_specification__Event += Invoke__associativity_specification__Callback;
			_name_list__Event += Invoke__name_list__Callback;
			_marker_list__Event += Invoke__marker_list__Callback;
			_marker__Event += Invoke__marker__Callback;
			_g_name__Event += Invoke__g_name__Callback;
			_name__Event += Invoke__name__Callback;
			_cardinality_delimiter__Event += Invoke__cardinality_delimiter__Callback;
			_cardinality__Event += Invoke__cardinality__Callback;
			_delimiter__Event += Invoke__delimiter__Callback;
			_attribute_list_optional__Event += Invoke__attribute_list_optional__Callback;
			_name_value_list_optional__Event += Invoke__name_value_list_optional__Callback;
			_anglr_file_part_list_optional__Event += Invoke__anglr_file_part_list_optional__Callback;
			_anglr_definition_list_optional__Event += Invoke__anglr_definition_list_optional__Callback;
			_block_terminal_definitions_optional__Event += Invoke__block_terminal_definitions_optional__Callback;
			_block_regex_definitions_optional__Event += Invoke__block_regex_definitions_optional__Callback;
			_regular_expression_list_optional__Event += Invoke__regular_expression_list_optional__Callback;
			_actions_optional__Event += Invoke__actions_optional__Callback;
			_anglr_syntax_rule_list_optional__Event += Invoke__anglr_syntax_rule_list_optional__Callback;
			_anglr_syntax_production_list_name_optional__Event += Invoke__anglr_syntax_production_list_name_optional__Callback;
			_production_name_optional__Event += Invoke__production_name_optional__Callback;
			_priority_assoc_specification_optional__Event += Invoke__priority_assoc_specification_optional__Callback;
			_marker_list_optional__Event += Invoke__marker_list_optional__Callback;
			_delimiter_optional__Event += Invoke__delimiter_optional__Callback;
			_cstring_optional__Event += Invoke__cstring_optional__Callback;
			_number_optional__Event += Invoke__number_optional__Callback;
		}

		//
		// event handler templates
		//

		private bool Invoke_Common_Callback (SyntaxTreeCallbackReason reason, int kind, SyntaxTreeBase p_node)
		{
			bool status = true;
			switch (reason)
			{
			case SyntaxTreeCallbackReason.BuilderCallbackReason:
				break;
			case SyntaxTreeCallbackReason.TraversalPrologueCallbackReason:
				break;
			case SyntaxTreeCallbackReason.TraversalMidTermCallbackReason:
				break;
			case SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason:
				break;
			}
			return status;
		}

		private bool Invoke__anglr_file_fragment__Callback (SyntaxTreeCallbackReason reason, _anglr_file_fragment_.production_kind kind, _anglr_file_fragment_ p__anglr_file_fragment_)
		{
			bool status = true;
			switch (reason)
			{
			case SyntaxTreeCallbackReason.BuilderCallbackReason:
				break;
			case SyntaxTreeCallbackReason.TraversalPrologueCallbackReason:
				break;
			case SyntaxTreeCallbackReason.TraversalMidTermCallbackReason:
				break;
			case SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason:
				break;
			}
			return status;
		}

		private bool Invoke__attribute_list__Callback (SyntaxTreeCallbackReason reason, _attribute_list_.production_kind kind, _attribute_list_ p__attribute_list_)
		{
			bool status = true;
			switch (reason)
			{
			case SyntaxTreeCallbackReason.BuilderCallbackReason:
				break;
			case SyntaxTreeCallbackReason.TraversalPrologueCallbackReason:
				break;
			case SyntaxTreeCallbackReason.TraversalMidTermCallbackReason:
				break;
			case SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason:
				break;
			}
			return status;
		}

		private bool Invoke__attribute__Callback (SyntaxTreeCallbackReason reason, _attribute_.production_kind kind, _attribute_ p__attribute_)
		{
			bool status = true;
			switch (reason)
			{
			case SyntaxTreeCallbackReason.BuilderCallbackReason:
				break;
			case SyntaxTreeCallbackReason.TraversalPrologueCallbackReason:
				break;
			case SyntaxTreeCallbackReason.TraversalMidTermCallbackReason:
				break;
			case SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason:
				break;
			}
			return status;
		}

		private bool Invoke__name_value_list__Callback (SyntaxTreeCallbackReason reason, _name_value_list_.production_kind kind, _name_value_list_ p__name_value_list_)
		{
			bool status = true;
			switch (reason)
			{
			case SyntaxTreeCallbackReason.BuilderCallbackReason:
				break;
			case SyntaxTreeCallbackReason.TraversalPrologueCallbackReason:
				break;
			case SyntaxTreeCallbackReason.TraversalMidTermCallbackReason:
				break;
			case SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason:
				break;
			}
			return status;
		}

		private bool Invoke__name_value_pair__Callback (SyntaxTreeCallbackReason reason, _name_value_pair_.production_kind kind, _name_value_pair_ p__name_value_pair_)
		{
			bool status = true;
			switch (reason)
			{
			case SyntaxTreeCallbackReason.BuilderCallbackReason:
				break;
			case SyntaxTreeCallbackReason.TraversalPrologueCallbackReason:
				break;
			case SyntaxTreeCallbackReason.TraversalMidTermCallbackReason:
				break;
			case SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason:
				break;
			}
			return status;
		}

		private bool Invoke__anglr_file__Callback (SyntaxTreeCallbackReason reason, _anglr_file_.production_kind kind, _anglr_file_ p__anglr_file_)
		{
			bool status = true;
			switch (reason)
			{
			case SyntaxTreeCallbackReason.BuilderCallbackReason:
				break;
			case SyntaxTreeCallbackReason.TraversalPrologueCallbackReason:
				break;
			case SyntaxTreeCallbackReason.TraversalMidTermCallbackReason:
				break;
			case SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason:
				break;
			}
			return status;
		}

		private bool Invoke__anglr_file_part_list__Callback (SyntaxTreeCallbackReason reason, _anglr_file_part_list_.production_kind kind, _anglr_file_part_list_ p__anglr_file_part_list_)
		{
			bool status = true;
			switch (reason)
			{
			case SyntaxTreeCallbackReason.BuilderCallbackReason:
				break;
			case SyntaxTreeCallbackReason.TraversalPrologueCallbackReason:
				break;
			case SyntaxTreeCallbackReason.TraversalMidTermCallbackReason:
				break;
			case SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason:
				break;
			}
			return status;
		}

		private bool Invoke__anglr_file_part__Callback (SyntaxTreeCallbackReason reason, _anglr_file_part_.production_kind kind, _anglr_file_part_ p__anglr_file_part_)
		{
			bool status = true;
			switch (reason)
			{
			case SyntaxTreeCallbackReason.BuilderCallbackReason:
				break;
			case SyntaxTreeCallbackReason.TraversalPrologueCallbackReason:
				break;
			case SyntaxTreeCallbackReason.TraversalMidTermCallbackReason:
				break;
			case SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason:
				break;
			}
			return status;
		}

		private bool Invoke__general_part__Callback (SyntaxTreeCallbackReason reason, _general_part_.production_kind kind, _general_part_ p__general_part_)
		{
			bool status = true;
			switch (reason)
			{
			case SyntaxTreeCallbackReason.BuilderCallbackReason:
				break;
			case SyntaxTreeCallbackReason.TraversalPrologueCallbackReason:
				break;
			case SyntaxTreeCallbackReason.TraversalMidTermCallbackReason:
				break;
			case SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason:
				break;
			}
			return status;
		}

		private bool Invoke__declaration_part__Callback (SyntaxTreeCallbackReason reason, _declaration_part_.production_kind kind, _declaration_part_ p__declaration_part_)
		{
			bool status = true;
			switch (reason)
			{
			case SyntaxTreeCallbackReason.BuilderCallbackReason:
				break;
			case SyntaxTreeCallbackReason.TraversalPrologueCallbackReason:
				break;
			case SyntaxTreeCallbackReason.TraversalMidTermCallbackReason:
				break;
			case SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason:
				break;
			}
			return status;
		}

		private bool Invoke__anglr_definition_list__Callback (SyntaxTreeCallbackReason reason, _anglr_definition_list_.production_kind kind, _anglr_definition_list_ p__anglr_definition_list_)
		{
			bool status = true;
			switch (reason)
			{
			case SyntaxTreeCallbackReason.BuilderCallbackReason:
				break;
			case SyntaxTreeCallbackReason.TraversalPrologueCallbackReason:
				break;
			case SyntaxTreeCallbackReason.TraversalMidTermCallbackReason:
				break;
			case SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason:
				break;
			}
			return status;
		}

		private bool Invoke__anglr_definition_with_attribute__Callback (SyntaxTreeCallbackReason reason, _anglr_definition_with_attribute_.production_kind kind, _anglr_definition_with_attribute_ p__anglr_definition_with_attribute_)
		{
			bool status = true;
			switch (reason)
			{
			case SyntaxTreeCallbackReason.BuilderCallbackReason:
				break;
			case SyntaxTreeCallbackReason.TraversalPrologueCallbackReason:
				break;
			case SyntaxTreeCallbackReason.TraversalMidTermCallbackReason:
				break;
			case SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason:
				break;
			}
			return status;
		}

		private bool Invoke__anglr_definition__Callback (SyntaxTreeCallbackReason reason, _anglr_definition_.production_kind kind, _anglr_definition_ p__anglr_definition_)
		{
			bool status = true;
			switch (reason)
			{
			case SyntaxTreeCallbackReason.BuilderCallbackReason:
				break;
			case SyntaxTreeCallbackReason.TraversalPrologueCallbackReason:
				break;
			case SyntaxTreeCallbackReason.TraversalMidTermCallbackReason:
				break;
			case SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason:
				break;
			}
			return status;
		}

		private bool Invoke__single_terminal_definition__Callback (SyntaxTreeCallbackReason reason, _single_terminal_definition_.production_kind kind, _single_terminal_definition_ p__single_terminal_definition_)
		{
			bool status = true;
			switch (reason)
			{
			case SyntaxTreeCallbackReason.BuilderCallbackReason:
				break;
			case SyntaxTreeCallbackReason.TraversalPrologueCallbackReason:
				break;
			case SyntaxTreeCallbackReason.TraversalMidTermCallbackReason:
				break;
			case SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason:
				break;
			}
			return status;
		}

		private bool Invoke__single_regex_definition__Callback (SyntaxTreeCallbackReason reason, _single_regex_definition_.production_kind kind, _single_regex_definition_ p__single_regex_definition_)
		{
			bool status = true;
			switch (reason)
			{
			case SyntaxTreeCallbackReason.BuilderCallbackReason:
				break;
			case SyntaxTreeCallbackReason.TraversalPrologueCallbackReason:
				break;
			case SyntaxTreeCallbackReason.TraversalMidTermCallbackReason:
				break;
			case SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason:
				break;
			}
			return status;
		}

		private bool Invoke__block_of_terminal_definitions__Callback (SyntaxTreeCallbackReason reason, _block_of_terminal_definitions_.production_kind kind, _block_of_terminal_definitions_ p__block_of_terminal_definitions_)
		{
			bool status = true;
			switch (reason)
			{
			case SyntaxTreeCallbackReason.BuilderCallbackReason:
				break;
			case SyntaxTreeCallbackReason.TraversalPrologueCallbackReason:
				break;
			case SyntaxTreeCallbackReason.TraversalMidTermCallbackReason:
				break;
			case SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason:
				break;
			}
			return status;
		}

		private bool Invoke__block_of_regex_definitions__Callback (SyntaxTreeCallbackReason reason, _block_of_regex_definitions_.production_kind kind, _block_of_regex_definitions_ p__block_of_regex_definitions_)
		{
			bool status = true;
			switch (reason)
			{
			case SyntaxTreeCallbackReason.BuilderCallbackReason:
				break;
			case SyntaxTreeCallbackReason.TraversalPrologueCallbackReason:
				break;
			case SyntaxTreeCallbackReason.TraversalMidTermCallbackReason:
				break;
			case SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason:
				break;
			}
			return status;
		}

		private bool Invoke__terminal_definition__Callback (SyntaxTreeCallbackReason reason, _terminal_definition_.production_kind kind, _terminal_definition_ p__terminal_definition_)
		{
			bool status = true;
			switch (reason)
			{
			case SyntaxTreeCallbackReason.BuilderCallbackReason:
				break;
			case SyntaxTreeCallbackReason.TraversalPrologueCallbackReason:
				break;
			case SyntaxTreeCallbackReason.TraversalMidTermCallbackReason:
				break;
			case SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason:
				break;
			}
			return status;
		}

		private bool Invoke__regex_definition__Callback (SyntaxTreeCallbackReason reason, _regex_definition_.production_kind kind, _regex_definition_ p__regex_definition_)
		{
			bool status = true;
			switch (reason)
			{
			case SyntaxTreeCallbackReason.BuilderCallbackReason:
				break;
			case SyntaxTreeCallbackReason.TraversalPrologueCallbackReason:
				break;
			case SyntaxTreeCallbackReason.TraversalMidTermCallbackReason:
				break;
			case SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason:
				break;
			}
			return status;
		}

		private bool Invoke__block_terminal_definitions__Callback (SyntaxTreeCallbackReason reason, _block_terminal_definitions_.production_kind kind, _block_terminal_definitions_ p__block_terminal_definitions_)
		{
			bool status = true;
			switch (reason)
			{
			case SyntaxTreeCallbackReason.BuilderCallbackReason:
				break;
			case SyntaxTreeCallbackReason.TraversalPrologueCallbackReason:
				break;
			case SyntaxTreeCallbackReason.TraversalMidTermCallbackReason:
				break;
			case SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason:
				break;
			}
			return status;
		}

		private bool Invoke__block_terminal_definition__Callback (SyntaxTreeCallbackReason reason, _block_terminal_definition_.production_kind kind, _block_terminal_definition_ p__block_terminal_definition_)
		{
			bool status = true;
			switch (reason)
			{
			case SyntaxTreeCallbackReason.BuilderCallbackReason:
				break;
			case SyntaxTreeCallbackReason.TraversalPrologueCallbackReason:
				break;
			case SyntaxTreeCallbackReason.TraversalMidTermCallbackReason:
				break;
			case SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason:
				break;
			}
			return status;
		}

		private bool Invoke__block_regex_definitions__Callback (SyntaxTreeCallbackReason reason, _block_regex_definitions_.production_kind kind, _block_regex_definitions_ p__block_regex_definitions_)
		{
			bool status = true;
			switch (reason)
			{
			case SyntaxTreeCallbackReason.BuilderCallbackReason:
				break;
			case SyntaxTreeCallbackReason.TraversalPrologueCallbackReason:
				break;
			case SyntaxTreeCallbackReason.TraversalMidTermCallbackReason:
				break;
			case SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason:
				break;
			}
			return status;
		}

		private bool Invoke__block_regex_definition__Callback (SyntaxTreeCallbackReason reason, _block_regex_definition_.production_kind kind, _block_regex_definition_ p__block_regex_definition_)
		{
			bool status = true;
			switch (reason)
			{
			case SyntaxTreeCallbackReason.BuilderCallbackReason:
				break;
			case SyntaxTreeCallbackReason.TraversalPrologueCallbackReason:
				break;
			case SyntaxTreeCallbackReason.TraversalMidTermCallbackReason:
				break;
			case SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason:
				break;
			}
			return status;
		}

		private bool Invoke__scanner_part__Callback (SyntaxTreeCallbackReason reason, _scanner_part_.production_kind kind, _scanner_part_ p__scanner_part_)
		{
			bool status = true;
			switch (reason)
			{
			case SyntaxTreeCallbackReason.BuilderCallbackReason:
				break;
			case SyntaxTreeCallbackReason.TraversalPrologueCallbackReason:
				break;
			case SyntaxTreeCallbackReason.TraversalMidTermCallbackReason:
				break;
			case SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason:
				break;
			}
			return status;
		}

		private bool Invoke__regular_expression_list__Callback (SyntaxTreeCallbackReason reason, _regular_expression_list_.production_kind kind, _regular_expression_list_ p__regular_expression_list_)
		{
			bool status = true;
			switch (reason)
			{
			case SyntaxTreeCallbackReason.BuilderCallbackReason:
				break;
			case SyntaxTreeCallbackReason.TraversalPrologueCallbackReason:
				break;
			case SyntaxTreeCallbackReason.TraversalMidTermCallbackReason:
				break;
			case SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason:
				break;
			}
			return status;
		}

		private bool Invoke__regular_expression_usage__Callback (SyntaxTreeCallbackReason reason, _regular_expression_usage_.production_kind kind, _regular_expression_usage_ p__regular_expression_usage_)
		{
			bool status = true;
			switch (reason)
			{
			case SyntaxTreeCallbackReason.BuilderCallbackReason:
				break;
			case SyntaxTreeCallbackReason.TraversalPrologueCallbackReason:
				break;
			case SyntaxTreeCallbackReason.TraversalMidTermCallbackReason:
				break;
			case SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason:
				break;
			}
			return status;
		}

		private bool Invoke__actions__Callback (SyntaxTreeCallbackReason reason, _actions_.production_kind kind, _actions_ p__actions_)
		{
			bool status = true;
			switch (reason)
			{
			case SyntaxTreeCallbackReason.BuilderCallbackReason:
				break;
			case SyntaxTreeCallbackReason.TraversalPrologueCallbackReason:
				break;
			case SyntaxTreeCallbackReason.TraversalMidTermCallbackReason:
				break;
			case SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason:
				break;
			}
			return status;
		}

		private bool Invoke__action__Callback (SyntaxTreeCallbackReason reason, _action_.production_kind kind, _action_ p__action_)
		{
			bool status = true;
			switch (reason)
			{
			case SyntaxTreeCallbackReason.BuilderCallbackReason:
				break;
			case SyntaxTreeCallbackReason.TraversalPrologueCallbackReason:
				break;
			case SyntaxTreeCallbackReason.TraversalMidTermCallbackReason:
				break;
			case SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason:
				break;
			}
			return status;
		}

		private bool Invoke__skip_action__Callback (SyntaxTreeCallbackReason reason, _skip_action_.production_kind kind, _skip_action_ p__skip_action_)
		{
			bool status = true;
			switch (reason)
			{
			case SyntaxTreeCallbackReason.BuilderCallbackReason:
				break;
			case SyntaxTreeCallbackReason.TraversalPrologueCallbackReason:
				break;
			case SyntaxTreeCallbackReason.TraversalMidTermCallbackReason:
				break;
			case SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason:
				break;
			}
			return status;
		}

		private bool Invoke__terminal_action__Callback (SyntaxTreeCallbackReason reason, _terminal_action_.production_kind kind, _terminal_action_ p__terminal_action_)
		{
			bool status = true;
			switch (reason)
			{
			case SyntaxTreeCallbackReason.BuilderCallbackReason:
				break;
			case SyntaxTreeCallbackReason.TraversalPrologueCallbackReason:
				break;
			case SyntaxTreeCallbackReason.TraversalMidTermCallbackReason:
				break;
			case SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason:
				break;
			}
			return status;
		}

		private bool Invoke__event_action__Callback (SyntaxTreeCallbackReason reason, _event_action_.production_kind kind, _event_action_ p__event_action_)
		{
			bool status = true;
			switch (reason)
			{
			case SyntaxTreeCallbackReason.BuilderCallbackReason:
				break;
			case SyntaxTreeCallbackReason.TraversalPrologueCallbackReason:
				break;
			case SyntaxTreeCallbackReason.TraversalMidTermCallbackReason:
				break;
			case SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason:
				break;
			}
			return status;
		}

		private bool Invoke__push_action__Callback (SyntaxTreeCallbackReason reason, _push_action_.production_kind kind, _push_action_ p__push_action_)
		{
			bool status = true;
			switch (reason)
			{
			case SyntaxTreeCallbackReason.BuilderCallbackReason:
				break;
			case SyntaxTreeCallbackReason.TraversalPrologueCallbackReason:
				break;
			case SyntaxTreeCallbackReason.TraversalMidTermCallbackReason:
				break;
			case SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason:
				break;
			}
			return status;
		}

		private bool Invoke__pop_action__Callback (SyntaxTreeCallbackReason reason, _pop_action_.production_kind kind, _pop_action_ p__pop_action_)
		{
			bool status = true;
			switch (reason)
			{
			case SyntaxTreeCallbackReason.BuilderCallbackReason:
				break;
			case SyntaxTreeCallbackReason.TraversalPrologueCallbackReason:
				break;
			case SyntaxTreeCallbackReason.TraversalMidTermCallbackReason:
				break;
			case SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason:
				break;
			}
			return status;
		}

		private bool Invoke__lexer_part__Callback (SyntaxTreeCallbackReason reason, _lexer_part_.production_kind kind, _lexer_part_ p__lexer_part_)
		{
			bool status = true;
			switch (reason)
			{
			case SyntaxTreeCallbackReason.BuilderCallbackReason:
				break;
			case SyntaxTreeCallbackReason.TraversalPrologueCallbackReason:
				break;
			case SyntaxTreeCallbackReason.TraversalMidTermCallbackReason:
				break;
			case SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason:
				break;
			}
			return status;
		}

		private bool Invoke__parser_part__Callback (SyntaxTreeCallbackReason reason, _parser_part_.production_kind kind, _parser_part_ p__parser_part_)
		{
			bool status = true;
			switch (reason)
			{
			case SyntaxTreeCallbackReason.BuilderCallbackReason:
				break;
			case SyntaxTreeCallbackReason.TraversalPrologueCallbackReason:
				break;
			case SyntaxTreeCallbackReason.TraversalMidTermCallbackReason:
				break;
			case SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason:
				break;
			}
			return status;
		}

		private bool Invoke__anglr_syntax_rule_list__Callback (SyntaxTreeCallbackReason reason, _anglr_syntax_rule_list_.production_kind kind, _anglr_syntax_rule_list_ p__anglr_syntax_rule_list_)
		{
			bool status = true;
			switch (reason)
			{
			case SyntaxTreeCallbackReason.BuilderCallbackReason:
				break;
			case SyntaxTreeCallbackReason.TraversalPrologueCallbackReason:
				break;
			case SyntaxTreeCallbackReason.TraversalMidTermCallbackReason:
				break;
			case SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason:
				break;
			}
			return status;
		}

		private bool Invoke__anglr_syntax_rule__Callback (SyntaxTreeCallbackReason reason, _anglr_syntax_rule_.production_kind kind, _anglr_syntax_rule_ p__anglr_syntax_rule_)
		{
			bool status = true;
			switch (reason)
			{
			case SyntaxTreeCallbackReason.BuilderCallbackReason:
				break;
			case SyntaxTreeCallbackReason.TraversalPrologueCallbackReason:
				break;
			case SyntaxTreeCallbackReason.TraversalMidTermCallbackReason:
				break;
			case SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason:
				break;
			}
			return status;
		}

		private bool Invoke__anglr_nested_rule__Callback (SyntaxTreeCallbackReason reason, _anglr_nested_rule_.production_kind kind, _anglr_nested_rule_ p__anglr_nested_rule_)
		{
			bool status = true;
			switch (reason)
			{
			case SyntaxTreeCallbackReason.BuilderCallbackReason:
				break;
			case SyntaxTreeCallbackReason.TraversalPrologueCallbackReason:
				break;
			case SyntaxTreeCallbackReason.TraversalMidTermCallbackReason:
				break;
			case SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason:
				break;
			}
			return status;
		}

		private bool Invoke__anglr_syntax_production_list_name__Callback (SyntaxTreeCallbackReason reason, _anglr_syntax_production_list_name_.production_kind kind, _anglr_syntax_production_list_name_ p__anglr_syntax_production_list_name_)
		{
			bool status = true;
			switch (reason)
			{
			case SyntaxTreeCallbackReason.BuilderCallbackReason:
				break;
			case SyntaxTreeCallbackReason.TraversalPrologueCallbackReason:
				break;
			case SyntaxTreeCallbackReason.TraversalMidTermCallbackReason:
				break;
			case SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason:
				break;
			}
			return status;
		}

		private bool Invoke__anglr_syntax_production_list__Callback (SyntaxTreeCallbackReason reason, _anglr_syntax_production_list_.production_kind kind, _anglr_syntax_production_list_ p__anglr_syntax_production_list_)
		{
			bool status = true;
			switch (reason)
			{
			case SyntaxTreeCallbackReason.BuilderCallbackReason:
				break;
			case SyntaxTreeCallbackReason.TraversalPrologueCallbackReason:
				break;
			case SyntaxTreeCallbackReason.TraversalMidTermCallbackReason:
				break;
			case SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason:
				break;
			}
			return status;
		}

		private bool Invoke__anglr_syntax_production__Callback (SyntaxTreeCallbackReason reason, _anglr_syntax_production_.production_kind kind, _anglr_syntax_production_ p__anglr_syntax_production_)
		{
			bool status = true;
			switch (reason)
			{
			case SyntaxTreeCallbackReason.BuilderCallbackReason:
				break;
			case SyntaxTreeCallbackReason.TraversalPrologueCallbackReason:
				break;
			case SyntaxTreeCallbackReason.TraversalMidTermCallbackReason:
				break;
			case SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason:
				break;
			}
			return status;
		}

		private bool Invoke__production_name__Callback (SyntaxTreeCallbackReason reason, _production_name_.production_kind kind, _production_name_ p__production_name_)
		{
			bool status = true;
			switch (reason)
			{
			case SyntaxTreeCallbackReason.BuilderCallbackReason:
				break;
			case SyntaxTreeCallbackReason.TraversalPrologueCallbackReason:
				break;
			case SyntaxTreeCallbackReason.TraversalMidTermCallbackReason:
				break;
			case SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason:
				break;
			}
			return status;
		}

		private bool Invoke__priority_assoc_specification__Callback (SyntaxTreeCallbackReason reason, _priority_assoc_specification_.production_kind kind, _priority_assoc_specification_ p__priority_assoc_specification_)
		{
			bool status = true;
			switch (reason)
			{
			case SyntaxTreeCallbackReason.BuilderCallbackReason:
				break;
			case SyntaxTreeCallbackReason.TraversalPrologueCallbackReason:
				break;
			case SyntaxTreeCallbackReason.TraversalMidTermCallbackReason:
				break;
			case SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason:
				break;
			}
			return status;
		}

		private bool Invoke__priority_specification__Callback (SyntaxTreeCallbackReason reason, _priority_specification_.production_kind kind, _priority_specification_ p__priority_specification_)
		{
			bool status = true;
			switch (reason)
			{
			case SyntaxTreeCallbackReason.BuilderCallbackReason:
				break;
			case SyntaxTreeCallbackReason.TraversalPrologueCallbackReason:
				break;
			case SyntaxTreeCallbackReason.TraversalMidTermCallbackReason:
				break;
			case SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason:
				break;
			}
			return status;
		}

		private bool Invoke__associativity_specification__Callback (SyntaxTreeCallbackReason reason, _associativity_specification_.production_kind kind, _associativity_specification_ p__associativity_specification_)
		{
			bool status = true;
			switch (reason)
			{
			case SyntaxTreeCallbackReason.BuilderCallbackReason:
				break;
			case SyntaxTreeCallbackReason.TraversalPrologueCallbackReason:
				break;
			case SyntaxTreeCallbackReason.TraversalMidTermCallbackReason:
				break;
			case SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason:
				break;
			}
			return status;
		}

		private bool Invoke__name_list__Callback (SyntaxTreeCallbackReason reason, _name_list_.production_kind kind, _name_list_ p__name_list_)
		{
			bool status = true;
			switch (reason)
			{
			case SyntaxTreeCallbackReason.BuilderCallbackReason:
				break;
			case SyntaxTreeCallbackReason.TraversalPrologueCallbackReason:
				break;
			case SyntaxTreeCallbackReason.TraversalMidTermCallbackReason:
				break;
			case SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason:
				break;
			}
			return status;
		}

		private bool Invoke__marker_list__Callback (SyntaxTreeCallbackReason reason, _marker_list_.production_kind kind, _marker_list_ p__marker_list_)
		{
			bool status = true;
			switch (reason)
			{
			case SyntaxTreeCallbackReason.BuilderCallbackReason:
				break;
			case SyntaxTreeCallbackReason.TraversalPrologueCallbackReason:
				break;
			case SyntaxTreeCallbackReason.TraversalMidTermCallbackReason:
				break;
			case SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason:
				break;
			}
			return status;
		}

		private bool Invoke__marker__Callback (SyntaxTreeCallbackReason reason, _marker_.production_kind kind, _marker_ p__marker_)
		{
			bool status = true;
			switch (reason)
			{
			case SyntaxTreeCallbackReason.BuilderCallbackReason:
				break;
			case SyntaxTreeCallbackReason.TraversalPrologueCallbackReason:
				break;
			case SyntaxTreeCallbackReason.TraversalMidTermCallbackReason:
				break;
			case SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason:
				break;
			}
			return status;
		}

		private bool Invoke__g_name__Callback (SyntaxTreeCallbackReason reason, _g_name_.production_kind kind, _g_name_ p__g_name_)
		{
			bool status = true;
			switch (reason)
			{
			case SyntaxTreeCallbackReason.BuilderCallbackReason:
				break;
			case SyntaxTreeCallbackReason.TraversalPrologueCallbackReason:
				break;
			case SyntaxTreeCallbackReason.TraversalMidTermCallbackReason:
				break;
			case SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason:
				break;
			}
			return status;
		}

		private bool Invoke__name__Callback (SyntaxTreeCallbackReason reason, _name_.production_kind kind, _name_ p__name_)
		{
			bool status = true;
			switch (reason)
			{
			case SyntaxTreeCallbackReason.BuilderCallbackReason:
				break;
			case SyntaxTreeCallbackReason.TraversalPrologueCallbackReason:
				break;
			case SyntaxTreeCallbackReason.TraversalMidTermCallbackReason:
				break;
			case SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason:
				break;
			}
			return status;
		}

		private bool Invoke__cardinality_delimiter__Callback (SyntaxTreeCallbackReason reason, _cardinality_delimiter_.production_kind kind, _cardinality_delimiter_ p__cardinality_delimiter_)
		{
			bool status = true;
			switch (reason)
			{
			case SyntaxTreeCallbackReason.BuilderCallbackReason:
				break;
			case SyntaxTreeCallbackReason.TraversalPrologueCallbackReason:
				break;
			case SyntaxTreeCallbackReason.TraversalMidTermCallbackReason:
				break;
			case SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason:
				break;
			}
			return status;
		}

		private bool Invoke__cardinality__Callback (SyntaxTreeCallbackReason reason, _cardinality_.production_kind kind, _cardinality_ p__cardinality_)
		{
			bool status = true;
			switch (reason)
			{
			case SyntaxTreeCallbackReason.BuilderCallbackReason:
				break;
			case SyntaxTreeCallbackReason.TraversalPrologueCallbackReason:
				break;
			case SyntaxTreeCallbackReason.TraversalMidTermCallbackReason:
				break;
			case SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason:
				break;
			}
			return status;
		}

		private bool Invoke__delimiter__Callback (SyntaxTreeCallbackReason reason, _delimiter_.production_kind kind, _delimiter_ p__delimiter_)
		{
			bool status = true;
			switch (reason)
			{
			case SyntaxTreeCallbackReason.BuilderCallbackReason:
				break;
			case SyntaxTreeCallbackReason.TraversalPrologueCallbackReason:
				break;
			case SyntaxTreeCallbackReason.TraversalMidTermCallbackReason:
				break;
			case SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason:
				break;
			}
			return status;
		}

		private bool Invoke__attribute_list_optional__Callback (SyntaxTreeCallbackReason reason, _attribute_list_optional_.production_kind kind, _attribute_list_optional_ p__attribute_list_optional_)
		{
			bool status = true;
			switch (reason)
			{
			case SyntaxTreeCallbackReason.BuilderCallbackReason:
				break;
			case SyntaxTreeCallbackReason.TraversalPrologueCallbackReason:
				break;
			case SyntaxTreeCallbackReason.TraversalMidTermCallbackReason:
				break;
			case SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason:
				break;
			}
			return status;
		}

		private bool Invoke__name_value_list_optional__Callback (SyntaxTreeCallbackReason reason, _name_value_list_optional_.production_kind kind, _name_value_list_optional_ p__name_value_list_optional_)
		{
			bool status = true;
			switch (reason)
			{
			case SyntaxTreeCallbackReason.BuilderCallbackReason:
				break;
			case SyntaxTreeCallbackReason.TraversalPrologueCallbackReason:
				break;
			case SyntaxTreeCallbackReason.TraversalMidTermCallbackReason:
				break;
			case SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason:
				break;
			}
			return status;
		}

		private bool Invoke__anglr_file_part_list_optional__Callback (SyntaxTreeCallbackReason reason, _anglr_file_part_list_optional_.production_kind kind, _anglr_file_part_list_optional_ p__anglr_file_part_list_optional_)
		{
			bool status = true;
			switch (reason)
			{
			case SyntaxTreeCallbackReason.BuilderCallbackReason:
				break;
			case SyntaxTreeCallbackReason.TraversalPrologueCallbackReason:
				break;
			case SyntaxTreeCallbackReason.TraversalMidTermCallbackReason:
				break;
			case SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason:
				break;
			}
			return status;
		}

		private bool Invoke__anglr_definition_list_optional__Callback (SyntaxTreeCallbackReason reason, _anglr_definition_list_optional_.production_kind kind, _anglr_definition_list_optional_ p__anglr_definition_list_optional_)
		{
			bool status = true;
			switch (reason)
			{
			case SyntaxTreeCallbackReason.BuilderCallbackReason:
				break;
			case SyntaxTreeCallbackReason.TraversalPrologueCallbackReason:
				break;
			case SyntaxTreeCallbackReason.TraversalMidTermCallbackReason:
				break;
			case SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason:
				break;
			}
			return status;
		}

		private bool Invoke__block_terminal_definitions_optional__Callback (SyntaxTreeCallbackReason reason, _block_terminal_definitions_optional_.production_kind kind, _block_terminal_definitions_optional_ p__block_terminal_definitions_optional_)
		{
			bool status = true;
			switch (reason)
			{
			case SyntaxTreeCallbackReason.BuilderCallbackReason:
				break;
			case SyntaxTreeCallbackReason.TraversalPrologueCallbackReason:
				break;
			case SyntaxTreeCallbackReason.TraversalMidTermCallbackReason:
				break;
			case SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason:
				break;
			}
			return status;
		}

		private bool Invoke__block_regex_definitions_optional__Callback (SyntaxTreeCallbackReason reason, _block_regex_definitions_optional_.production_kind kind, _block_regex_definitions_optional_ p__block_regex_definitions_optional_)
		{
			bool status = true;
			switch (reason)
			{
			case SyntaxTreeCallbackReason.BuilderCallbackReason:
				break;
			case SyntaxTreeCallbackReason.TraversalPrologueCallbackReason:
				break;
			case SyntaxTreeCallbackReason.TraversalMidTermCallbackReason:
				break;
			case SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason:
				break;
			}
			return status;
		}

		private bool Invoke__regular_expression_list_optional__Callback (SyntaxTreeCallbackReason reason, _regular_expression_list_optional_.production_kind kind, _regular_expression_list_optional_ p__regular_expression_list_optional_)
		{
			bool status = true;
			switch (reason)
			{
			case SyntaxTreeCallbackReason.BuilderCallbackReason:
				break;
			case SyntaxTreeCallbackReason.TraversalPrologueCallbackReason:
				break;
			case SyntaxTreeCallbackReason.TraversalMidTermCallbackReason:
				break;
			case SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason:
				break;
			}
			return status;
		}

		private bool Invoke__actions_optional__Callback (SyntaxTreeCallbackReason reason, _actions_optional_.production_kind kind, _actions_optional_ p__actions_optional_)
		{
			bool status = true;
			switch (reason)
			{
			case SyntaxTreeCallbackReason.BuilderCallbackReason:
				break;
			case SyntaxTreeCallbackReason.TraversalPrologueCallbackReason:
				break;
			case SyntaxTreeCallbackReason.TraversalMidTermCallbackReason:
				break;
			case SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason:
				break;
			}
			return status;
		}

		private bool Invoke__anglr_syntax_rule_list_optional__Callback (SyntaxTreeCallbackReason reason, _anglr_syntax_rule_list_optional_.production_kind kind, _anglr_syntax_rule_list_optional_ p__anglr_syntax_rule_list_optional_)
		{
			bool status = true;
			switch (reason)
			{
			case SyntaxTreeCallbackReason.BuilderCallbackReason:
				break;
			case SyntaxTreeCallbackReason.TraversalPrologueCallbackReason:
				break;
			case SyntaxTreeCallbackReason.TraversalMidTermCallbackReason:
				break;
			case SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason:
				break;
			}
			return status;
		}

		private bool Invoke__anglr_syntax_production_list_name_optional__Callback (SyntaxTreeCallbackReason reason, _anglr_syntax_production_list_name_optional_.production_kind kind, _anglr_syntax_production_list_name_optional_ p__anglr_syntax_production_list_name_optional_)
		{
			bool status = true;
			switch (reason)
			{
			case SyntaxTreeCallbackReason.BuilderCallbackReason:
				break;
			case SyntaxTreeCallbackReason.TraversalPrologueCallbackReason:
				break;
			case SyntaxTreeCallbackReason.TraversalMidTermCallbackReason:
				break;
			case SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason:
				break;
			}
			return status;
		}

		private bool Invoke__production_name_optional__Callback (SyntaxTreeCallbackReason reason, _production_name_optional_.production_kind kind, _production_name_optional_ p__production_name_optional_)
		{
			bool status = true;
			switch (reason)
			{
			case SyntaxTreeCallbackReason.BuilderCallbackReason:
				break;
			case SyntaxTreeCallbackReason.TraversalPrologueCallbackReason:
				break;
			case SyntaxTreeCallbackReason.TraversalMidTermCallbackReason:
				break;
			case SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason:
				break;
			}
			return status;
		}

		private bool Invoke__priority_assoc_specification_optional__Callback (SyntaxTreeCallbackReason reason, _priority_assoc_specification_optional_.production_kind kind, _priority_assoc_specification_optional_ p__priority_assoc_specification_optional_)
		{
			bool status = true;
			switch (reason)
			{
			case SyntaxTreeCallbackReason.BuilderCallbackReason:
				break;
			case SyntaxTreeCallbackReason.TraversalPrologueCallbackReason:
				break;
			case SyntaxTreeCallbackReason.TraversalMidTermCallbackReason:
				break;
			case SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason:
				break;
			}
			return status;
		}

		private bool Invoke__marker_list_optional__Callback (SyntaxTreeCallbackReason reason, _marker_list_optional_.production_kind kind, _marker_list_optional_ p__marker_list_optional_)
		{
			bool status = true;
			switch (reason)
			{
			case SyntaxTreeCallbackReason.BuilderCallbackReason:
				break;
			case SyntaxTreeCallbackReason.TraversalPrologueCallbackReason:
				break;
			case SyntaxTreeCallbackReason.TraversalMidTermCallbackReason:
				break;
			case SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason:
				break;
			}
			return status;
		}

		private bool Invoke__delimiter_optional__Callback (SyntaxTreeCallbackReason reason, _delimiter_optional_.production_kind kind, _delimiter_optional_ p__delimiter_optional_)
		{
			bool status = true;
			switch (reason)
			{
			case SyntaxTreeCallbackReason.BuilderCallbackReason:
				break;
			case SyntaxTreeCallbackReason.TraversalPrologueCallbackReason:
				break;
			case SyntaxTreeCallbackReason.TraversalMidTermCallbackReason:
				break;
			case SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason:
				break;
			}
			return status;
		}

		private bool Invoke__cstring_optional__Callback (SyntaxTreeCallbackReason reason, _cstring_optional_.production_kind kind, _cstring_optional_ p__cstring_optional_)
		{
			bool status = true;
			switch (reason)
			{
			case SyntaxTreeCallbackReason.BuilderCallbackReason:
				break;
			case SyntaxTreeCallbackReason.TraversalPrologueCallbackReason:
				break;
			case SyntaxTreeCallbackReason.TraversalMidTermCallbackReason:
				break;
			case SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason:
				break;
			}
			return status;
		}

		private bool Invoke__number_optional__Callback (SyntaxTreeCallbackReason reason, _number_optional_.production_kind kind, _number_optional_ p__number_optional_)
		{
			bool status = true;
			switch (reason)
			{
			case SyntaxTreeCallbackReason.BuilderCallbackReason:
				break;
			case SyntaxTreeCallbackReason.TraversalPrologueCallbackReason:
				break;
			case SyntaxTreeCallbackReason.TraversalMidTermCallbackReason:
				break;
			case SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason:
				break;
			}
			return status;
		}

		//
		// event fire templates
		//

		public void InvokeTest (_anglr_file_fragment_ p__anglr_file_fragment_)
		{
			Traverse (p__anglr_file_fragment_);
			TraverseCommon (p__anglr_file_fragment_);
		}

		public void InvokeTest (_attribute_list_ p__attribute_list_)
		{
			Traverse (p__attribute_list_);
			TraverseCommon (p__attribute_list_);
		}

		public void InvokeTest (_attribute_ p__attribute_)
		{
			Traverse (p__attribute_);
			TraverseCommon (p__attribute_);
		}

		public void InvokeTest (_name_value_list_ p__name_value_list_)
		{
			Traverse (p__name_value_list_);
			TraverseCommon (p__name_value_list_);
		}

		public void InvokeTest (_name_value_pair_ p__name_value_pair_)
		{
			Traverse (p__name_value_pair_);
			TraverseCommon (p__name_value_pair_);
		}

		public void InvokeTest (_anglr_file_ p__anglr_file_)
		{
			Traverse (p__anglr_file_);
			TraverseCommon (p__anglr_file_);
		}

		public void InvokeTest (_anglr_file_part_list_ p__anglr_file_part_list_)
		{
			Traverse (p__anglr_file_part_list_);
			TraverseCommon (p__anglr_file_part_list_);
		}

		public void InvokeTest (_anglr_file_part_ p__anglr_file_part_)
		{
			Traverse (p__anglr_file_part_);
			TraverseCommon (p__anglr_file_part_);
		}

		public void InvokeTest (_general_part_ p__general_part_)
		{
			Traverse (p__general_part_);
			TraverseCommon (p__general_part_);
		}

		public void InvokeTest (_declaration_part_ p__declaration_part_)
		{
			Traverse (p__declaration_part_);
			TraverseCommon (p__declaration_part_);
		}

		public void InvokeTest (_anglr_definition_list_ p__anglr_definition_list_)
		{
			Traverse (p__anglr_definition_list_);
			TraverseCommon (p__anglr_definition_list_);
		}

		public void InvokeTest (_anglr_definition_with_attribute_ p__anglr_definition_with_attribute_)
		{
			Traverse (p__anglr_definition_with_attribute_);
			TraverseCommon (p__anglr_definition_with_attribute_);
		}

		public void InvokeTest (_anglr_definition_ p__anglr_definition_)
		{
			Traverse (p__anglr_definition_);
			TraverseCommon (p__anglr_definition_);
		}

		public void InvokeTest (_single_terminal_definition_ p__single_terminal_definition_)
		{
			Traverse (p__single_terminal_definition_);
			TraverseCommon (p__single_terminal_definition_);
		}

		public void InvokeTest (_single_regex_definition_ p__single_regex_definition_)
		{
			Traverse (p__single_regex_definition_);
			TraverseCommon (p__single_regex_definition_);
		}

		public void InvokeTest (_block_of_terminal_definitions_ p__block_of_terminal_definitions_)
		{
			Traverse (p__block_of_terminal_definitions_);
			TraverseCommon (p__block_of_terminal_definitions_);
		}

		public void InvokeTest (_block_of_regex_definitions_ p__block_of_regex_definitions_)
		{
			Traverse (p__block_of_regex_definitions_);
			TraverseCommon (p__block_of_regex_definitions_);
		}

		public void InvokeTest (_terminal_definition_ p__terminal_definition_)
		{
			Traverse (p__terminal_definition_);
			TraverseCommon (p__terminal_definition_);
		}

		public void InvokeTest (_regex_definition_ p__regex_definition_)
		{
			Traverse (p__regex_definition_);
			TraverseCommon (p__regex_definition_);
		}

		public void InvokeTest (_block_terminal_definitions_ p__block_terminal_definitions_)
		{
			Traverse (p__block_terminal_definitions_);
			TraverseCommon (p__block_terminal_definitions_);
		}

		public void InvokeTest (_block_terminal_definition_ p__block_terminal_definition_)
		{
			Traverse (p__block_terminal_definition_);
			TraverseCommon (p__block_terminal_definition_);
		}

		public void InvokeTest (_block_regex_definitions_ p__block_regex_definitions_)
		{
			Traverse (p__block_regex_definitions_);
			TraverseCommon (p__block_regex_definitions_);
		}

		public void InvokeTest (_block_regex_definition_ p__block_regex_definition_)
		{
			Traverse (p__block_regex_definition_);
			TraverseCommon (p__block_regex_definition_);
		}

		public void InvokeTest (_scanner_part_ p__scanner_part_)
		{
			Traverse (p__scanner_part_);
			TraverseCommon (p__scanner_part_);
		}

		public void InvokeTest (_regular_expression_list_ p__regular_expression_list_)
		{
			Traverse (p__regular_expression_list_);
			TraverseCommon (p__regular_expression_list_);
		}

		public void InvokeTest (_regular_expression_usage_ p__regular_expression_usage_)
		{
			Traverse (p__regular_expression_usage_);
			TraverseCommon (p__regular_expression_usage_);
		}

		public void InvokeTest (_actions_ p__actions_)
		{
			Traverse (p__actions_);
			TraverseCommon (p__actions_);
		}

		public void InvokeTest (_action_ p__action_)
		{
			Traverse (p__action_);
			TraverseCommon (p__action_);
		}

		public void InvokeTest (_skip_action_ p__skip_action_)
		{
			Traverse (p__skip_action_);
			TraverseCommon (p__skip_action_);
		}

		public void InvokeTest (_terminal_action_ p__terminal_action_)
		{
			Traverse (p__terminal_action_);
			TraverseCommon (p__terminal_action_);
		}

		public void InvokeTest (_event_action_ p__event_action_)
		{
			Traverse (p__event_action_);
			TraverseCommon (p__event_action_);
		}

		public void InvokeTest (_push_action_ p__push_action_)
		{
			Traverse (p__push_action_);
			TraverseCommon (p__push_action_);
		}

		public void InvokeTest (_pop_action_ p__pop_action_)
		{
			Traverse (p__pop_action_);
			TraverseCommon (p__pop_action_);
		}

		public void InvokeTest (_lexer_part_ p__lexer_part_)
		{
			Traverse (p__lexer_part_);
			TraverseCommon (p__lexer_part_);
		}

		public void InvokeTest (_parser_part_ p__parser_part_)
		{
			Traverse (p__parser_part_);
			TraverseCommon (p__parser_part_);
		}

		public void InvokeTest (_anglr_syntax_rule_list_ p__anglr_syntax_rule_list_)
		{
			Traverse (p__anglr_syntax_rule_list_);
			TraverseCommon (p__anglr_syntax_rule_list_);
		}

		public void InvokeTest (_anglr_syntax_rule_ p__anglr_syntax_rule_)
		{
			Traverse (p__anglr_syntax_rule_);
			TraverseCommon (p__anglr_syntax_rule_);
		}

		public void InvokeTest (_anglr_nested_rule_ p__anglr_nested_rule_)
		{
			Traverse (p__anglr_nested_rule_);
			TraverseCommon (p__anglr_nested_rule_);
		}

		public void InvokeTest (_anglr_syntax_production_list_name_ p__anglr_syntax_production_list_name_)
		{
			Traverse (p__anglr_syntax_production_list_name_);
			TraverseCommon (p__anglr_syntax_production_list_name_);
		}

		public void InvokeTest (_anglr_syntax_production_list_ p__anglr_syntax_production_list_)
		{
			Traverse (p__anglr_syntax_production_list_);
			TraverseCommon (p__anglr_syntax_production_list_);
		}

		public void InvokeTest (_anglr_syntax_production_ p__anglr_syntax_production_)
		{
			Traverse (p__anglr_syntax_production_);
			TraverseCommon (p__anglr_syntax_production_);
		}

		public void InvokeTest (_production_name_ p__production_name_)
		{
			Traverse (p__production_name_);
			TraverseCommon (p__production_name_);
		}

		public void InvokeTest (_priority_assoc_specification_ p__priority_assoc_specification_)
		{
			Traverse (p__priority_assoc_specification_);
			TraverseCommon (p__priority_assoc_specification_);
		}

		public void InvokeTest (_priority_specification_ p__priority_specification_)
		{
			Traverse (p__priority_specification_);
			TraverseCommon (p__priority_specification_);
		}

		public void InvokeTest (_associativity_specification_ p__associativity_specification_)
		{
			Traverse (p__associativity_specification_);
			TraverseCommon (p__associativity_specification_);
		}

		public void InvokeTest (_name_list_ p__name_list_)
		{
			Traverse (p__name_list_);
			TraverseCommon (p__name_list_);
		}

		public void InvokeTest (_marker_list_ p__marker_list_)
		{
			Traverse (p__marker_list_);
			TraverseCommon (p__marker_list_);
		}

		public void InvokeTest (_marker_ p__marker_)
		{
			Traverse (p__marker_);
			TraverseCommon (p__marker_);
		}

		public void InvokeTest (_g_name_ p__g_name_)
		{
			Traverse (p__g_name_);
			TraverseCommon (p__g_name_);
		}

		public void InvokeTest (_name_ p__name_)
		{
			Traverse (p__name_);
			TraverseCommon (p__name_);
		}

		public void InvokeTest (_cardinality_delimiter_ p__cardinality_delimiter_)
		{
			Traverse (p__cardinality_delimiter_);
			TraverseCommon (p__cardinality_delimiter_);
		}

		public void InvokeTest (_cardinality_ p__cardinality_)
		{
			Traverse (p__cardinality_);
			TraverseCommon (p__cardinality_);
		}

		public void InvokeTest (_delimiter_ p__delimiter_)
		{
			Traverse (p__delimiter_);
			TraverseCommon (p__delimiter_);
		}

		public void InvokeTest (_attribute_list_optional_ p__attribute_list_optional_)
		{
			Traverse (p__attribute_list_optional_);
			TraverseCommon (p__attribute_list_optional_);
		}

		public void InvokeTest (_name_value_list_optional_ p__name_value_list_optional_)
		{
			Traverse (p__name_value_list_optional_);
			TraverseCommon (p__name_value_list_optional_);
		}

		public void InvokeTest (_anglr_file_part_list_optional_ p__anglr_file_part_list_optional_)
		{
			Traverse (p__anglr_file_part_list_optional_);
			TraverseCommon (p__anglr_file_part_list_optional_);
		}

		public void InvokeTest (_anglr_definition_list_optional_ p__anglr_definition_list_optional_)
		{
			Traverse (p__anglr_definition_list_optional_);
			TraverseCommon (p__anglr_definition_list_optional_);
		}

		public void InvokeTest (_block_terminal_definitions_optional_ p__block_terminal_definitions_optional_)
		{
			Traverse (p__block_terminal_definitions_optional_);
			TraverseCommon (p__block_terminal_definitions_optional_);
		}

		public void InvokeTest (_block_regex_definitions_optional_ p__block_regex_definitions_optional_)
		{
			Traverse (p__block_regex_definitions_optional_);
			TraverseCommon (p__block_regex_definitions_optional_);
		}

		public void InvokeTest (_regular_expression_list_optional_ p__regular_expression_list_optional_)
		{
			Traverse (p__regular_expression_list_optional_);
			TraverseCommon (p__regular_expression_list_optional_);
		}

		public void InvokeTest (_actions_optional_ p__actions_optional_)
		{
			Traverse (p__actions_optional_);
			TraverseCommon (p__actions_optional_);
		}

		public void InvokeTest (_anglr_syntax_rule_list_optional_ p__anglr_syntax_rule_list_optional_)
		{
			Traverse (p__anglr_syntax_rule_list_optional_);
			TraverseCommon (p__anglr_syntax_rule_list_optional_);
		}

		public void InvokeTest (_anglr_syntax_production_list_name_optional_ p__anglr_syntax_production_list_name_optional_)
		{
			Traverse (p__anglr_syntax_production_list_name_optional_);
			TraverseCommon (p__anglr_syntax_production_list_name_optional_);
		}

		public void InvokeTest (_production_name_optional_ p__production_name_optional_)
		{
			Traverse (p__production_name_optional_);
			TraverseCommon (p__production_name_optional_);
		}

		public void InvokeTest (_priority_assoc_specification_optional_ p__priority_assoc_specification_optional_)
		{
			Traverse (p__priority_assoc_specification_optional_);
			TraverseCommon (p__priority_assoc_specification_optional_);
		}

		public void InvokeTest (_marker_list_optional_ p__marker_list_optional_)
		{
			Traverse (p__marker_list_optional_);
			TraverseCommon (p__marker_list_optional_);
		}

		public void InvokeTest (_delimiter_optional_ p__delimiter_optional_)
		{
			Traverse (p__delimiter_optional_);
			TraverseCommon (p__delimiter_optional_);
		}

		public void InvokeTest (_cstring_optional_ p__cstring_optional_)
		{
			Traverse (p__cstring_optional_);
			TraverseCommon (p__cstring_optional_);
		}

		public void InvokeTest (_number_optional_ p__number_optional_)
		{
			Traverse (p__number_optional_);
			TraverseCommon (p__number_optional_);
		}
	}

	internal class AnglrParserExample
	{
		static void Main (string [] args)
		{
			// parse every source file named in command line
			foreach (string arg in args)
			{
				AnglrParser parser = new AnglrParser ();
				AnglrLexer lexer = new AnglrLexer (new StreamReader (arg));

				// register method reporting syntax errors
				parser.Error_Event += (int lineno, int column, int token, string tokenString) =>
				{
					Console.Error.WriteLine ($"Syntax error: file ({arg}), line ({lineno}), column ({column}), token ({token} - {tokenString}");
					return true;	// continue parsing
				};


				// invoke parser
				AnglrParser.createParseTree = true;
				if (parser.parse (lexer) != 0)
					continue;	// errors, skip current file

				// visit every node of every syntax tree generated for current source file
				foreach (var syntaxTree in parser.parseList)
				{
					AnglrParser_TEST testWalker = new AnglrParser_TEST ();
					testWalker.Traverse (syntaxTree as _anglr_file_fragment_);
				}
			}
		}
	}
}
