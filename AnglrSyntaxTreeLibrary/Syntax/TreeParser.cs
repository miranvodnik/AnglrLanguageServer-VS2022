//
//	This file was generated with ANGLR compiler
//
using System;
using System.Runtime.Serialization;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.IO;
using Tree.Declarations;
using Tree.Lexer;

using Anglr.Parser.Core;
using Anglr.Parser.SyntaxTree;
using AnglrLogLibrary;
#if ANGLR_DEBUGGER
using AnglrDebuggerBridge;
#endif

namespace Tree.Parser
{

	public class TreeParser : SyntaxTreeBuilder, ParserInterface
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
		public stackset parserStacks => stackSet;

		public bool FragmentParser { get; set; } = false;
		public int ProductionID { get; set; } = 0;
		public int InitialState { get; set; } = 0;
		public int LastToken { get; set; } = 0;
		public string LastTokenName { get; set; } = "";
#if ANGLR_DEBUGGER
		public AnglrDebuggerServerBridge AnglrDebugBridgeObj { get; private set; }
#endif

		public TreeParser (string fragmentName = null, IAnglrLogger anglrLogger = null) : base ()
		{
			AnglrLogger = anglrLogger;
			(string fragmentName, ProductionID productionId, int initialState, int finalTokenId, string finalTokenName) fragmentInfo = AnglrFragments.GetFragmentInfo (fragmentName);
			if (fragmentInfo.productionId != 0)
			{
				FragmentParser = true;
				ProductionID = (int) fragmentInfo.productionId;
				InitialState = fragmentInfo.initialState;
				LastToken = fragmentInfo.finalTokenId;
				LastTokenName = fragmentInfo.finalTokenName;
			}
			token = new ParserToken (0, 0, null);
			stackSet = new stackset (new cmpstackid ());
			parseList = new parselist ();
			stackSet.Add (new ParserStack (this));
		}

		public TreeParser (ProductionID productionId, IAnglrLogger anglrLogger = null) : base ()
		{
			AnglrLogger = anglrLogger;
			(string fragmentName, ProductionID productionId, int initialState, int finalTokenId, string finalTokenName) fragmentInfo = AnglrFragments.GetFragmentInfo (productionId);
			if (fragmentInfo.productionId != 0)
			{
				FragmentParser = true;
				ProductionID = (int) fragmentInfo.productionId;
				InitialState = fragmentInfo.initialState;
				LastToken = fragmentInfo.finalTokenId;
				LastTokenName = fragmentInfo.finalTokenName;
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
			this.AnglrDebugBridgeObj = new AnglrDebuggerServerBridge (this);
		}
		private void DisposeDebuggerConnection ()
		{
			this.AnglrDebugBridgeObj?.Dispose ();
		}
#endif

		public int parse (TreeLexer scanner)
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
			case 2:
				currentValue = tree_1 ((SyntaxTreeToken) valueStack [-3], (tree_element_set) valueStack [-2], (SyntaxTreeToken) valueStack [-1]);
				break;
			case 3:
				currentValue = tree_element_1 ((element_name) valueStack [-3], (SyntaxTreeToken) valueStack [-2], (element_value) valueStack [-1]);
				break;
			case 4:
				currentValue = element_name_1 ((SyntaxTreeToken) valueStack [-1]);
				break;
			case 5:
				currentValue = element_value_1 ((SyntaxTreeToken) valueStack [-1]);
				break;
			case 6:
				currentValue = element_value_2 ((SyntaxTreeToken) valueStack [-1]);
				break;
			case 7:
				currentValue = element_value_3 ((SyntaxTreeToken) valueStack [-1]);
				break;
			case 8:
				currentValue = element_value_4 ((tree) valueStack [-1]);
				break;
			case 9:
				currentValue = tree_element_set_1 ((tree_element) valueStack [-1]);
				break;
			case 10:
				currentValue = tree_element_set_2 ((tree_element_set) valueStack [-3], (SyntaxTreeToken) valueStack [-2], (tree_element) valueStack [-1]);
				break;
			case 11:
			break;
			}
			return currentValue;
		}

		void ReportClassCounters ()
		{
			if (tree.g_counter != 0)
				AnglrLogger?.DebugRawLine ("tree.g_counter = " + tree.g_counter);
			if (tree_element.g_counter != 0)
				AnglrLogger?.DebugRawLine ("tree_element.g_counter = " + tree_element.g_counter);
			if (element_name.g_counter != 0)
				AnglrLogger?.DebugRawLine ("element_name.g_counter = " + element_name.g_counter);
			if (element_value.g_counter != 0)
				AnglrLogger?.DebugRawLine ("element_value.g_counter = " + element_value.g_counter);
			if (tree_element_set.g_counter != 0)
				AnglrLogger?.DebugRawLine ("tree_element_set.g_counter = " + tree_element_set.g_counter);
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

		internal readonly static int g_magicNumber = 152842;

		internal readonly static int g_minTerminalCode = 258;
		internal readonly static int[] g_terminalCodes = 
		{
			   258,    259,    260,    261,    262,    263,    264, 
		};

		internal readonly static string[] g_terminalNames = 
		{
			"<left curly bracket>",
			"<right curly bracket>",
			"<colon>",
			"<coma>",
			"<cstring>",
			"<number>",
			"<null>",
		};

		internal readonly static int g_minNonTerminalCode = 265;
		internal readonly static int[] g_nonTerminalCodes = 
		{
			   265,    266,    267,    268,    269, 
		};

		internal readonly static string[] g_nonTerminalNames = 
		{
			"tree",
			"tree-element",
			"element-name",
			"element-value",
			"tree-element-set",
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
			     0,      0,      0,      0,      1,      0,      0,      0,      0,      0, 
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
			     0,      0,      0,      0,      0,      0,      0,      0,      0,    258, 
			     0,      0,      0,      0,    262,    259,      0,    261,    260,    258, 
			     0,      0,      0,    262,    263,    264,    266,    265, 
		};

		internal readonly static int[] g_state =
		{
			     0,      0,      0,      0,      7,      0,      0,      0,      0,      0, 
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
			     0,      0,      0,      0,      0,      0,      0,      0,      0,      1, 
			     0,      0,      0,      0,      3,      8,      0,      9,     10,      1, 
			     0,      0,      0,     12,     13,     14,     11,     16, 
		};

		internal readonly static int[] g_shiftDelta =
		{
			     1,      2,      3,      0,      6,      0,      8,      0,      0,      2, 
			    11,      0,      0,      0,      0,      0,      0, 
		};

		internal readonly static int[] g_gotoDelta =
		{
			     5,      7,      0,      0,      0,      0,      0,      0,      0,     10, 
			    12,      0,      0,      0,      0,      0,      0, 
		};

		internal readonly static int[] g_productionLengths = 
		{
			     0,      0,      3,      3,      1,      1,      1,      1,      1,      1, 
			     3,      2, 
		};

		internal readonly static int[] g_productionRules = 
		{
			     0,      0,    265,    266,    267,    268,    268,    268,    268,    269, 
			   269,      0, 
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
			     0,      0,      0,      0,      0,      2,      5,      6,     15,      4, 
		};

		internal readonly static int[] g_rcheck =
		{
			0
		};

		internal readonly static int[] g_rstate =
		{
			0
		};

		internal readonly static int[] g_reductions =
		{
			     0,      0,      0,      4,      0,      9,      0,     11,      2,      0, 
			     0,     10,      5,      6,      7,      3,      8, 
		};

	}

	public partial class SyntaxTreeWalker : Anglr.Parser.Walker.SyntaxTreeWalkerCore { }

	public enum ProductionID
	{
		InvalidProductionID = 0,
		_tree_ID = 265,
		_tree_element_ID = 266,
		_element_name_ID = 267,
		_element_value_ID = 268,
		_tree_element_set_ID = 269,
		LastProductionID
	};

	public static class AnglrFragments
	{
		public static readonly (string fragmentName, ProductionID productionId, int initialState, int finalTokenId, string finalTokenName) [] FragmentsInfo =
		{
			("tree", ProductionID._tree_ID, 0, 1, "_eof_token"),
			("tree-element-set", ProductionID._tree_element_set_ID, 1, 259, "_right_curly_bracket_"),
			("tree-element", ProductionID._tree_element_ID, 9, 259, "_right_curly_bracket_"),
			("element-name", ProductionID._element_name_ID, 9, 260, "_colon_"),
			("element-value", ProductionID._element_value_ID, 10, 259, "_right_curly_bracket_"),
		};

		public static Dictionary<string, (string fragmentName, ProductionID productionId, int initialState, int finalTokenId, string finalTokenName)> _FragmentsInfoDictionaryByName = FragmentsInfo.ToDictionary (x => x.fragmentName, x => (x.fragmentName, x.productionId, x.initialState, x.finalTokenId, x.finalTokenName));
		public static Dictionary<ProductionID, (string fragmentName, ProductionID productionId, int initialState, int finalTokenId, string finalTokenName)> _FragmentsInfoDictionaryById = FragmentsInfo.ToDictionary (x => x.productionId, x => (x.fragmentName, x.productionId, x.initialState, x.finalTokenId, x.finalTokenName));

		public static (string fragmentName, ProductionID productionId, int initialState, int finalTokenId, string finalTokenName) GetFragmentInfo (string fragmentName) =>
			_FragmentsInfoDictionaryByName.TryGetValue (fragmentName ?? "", out var fragment) ?
			fragment :
			("", 0, -1, -1, "");

		public static (string fragmentName, ProductionID productionId, int initialState, int finalTokenId, string finalTokenName) GetFragmentInfo (ProductionID productionId) =>
			_FragmentsInfoDictionaryById.TryGetValue (productionId, out var fragment) ?
			fragment :
			("", 0, -1, -1, "");
	}

	public class TreeParser_TEST : SyntaxTreeWalker
	{
		public TreeParser_TEST ()
		{
			//
			// event registration templates
			//
			Common_Event += Invoke_Common_Callback;
			tree_Event += Invoke_tree_Callback;
			tree_element_Event += Invoke_tree_element_Callback;
			element_name_Event += Invoke_element_name_Callback;
			element_value_Event += Invoke_element_value_Callback;
			tree_element_set_Event += Invoke_tree_element_set_Callback;
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

		private bool Invoke_tree_Callback (SyntaxTreeCallbackReason reason, tree.production_kind kind, tree p_tree)
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

		private bool Invoke_tree_element_Callback (SyntaxTreeCallbackReason reason, tree_element.production_kind kind, tree_element p_tree_element)
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

		private bool Invoke_element_name_Callback (SyntaxTreeCallbackReason reason, element_name.production_kind kind, element_name p_element_name)
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

		private bool Invoke_element_value_Callback (SyntaxTreeCallbackReason reason, element_value.production_kind kind, element_value p_element_value)
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

		private bool Invoke_tree_element_set_Callback (SyntaxTreeCallbackReason reason, tree_element_set.production_kind kind, tree_element_set p_tree_element_set)
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

		public void InvokeTest (tree p_tree)
		{
			Traverse (p_tree);
			TraverseCommon (p_tree);
		}

		public void InvokeTest (tree_element p_tree_element)
		{
			Traverse (p_tree_element);
			TraverseCommon (p_tree_element);
		}

		public void InvokeTest (element_name p_element_name)
		{
			Traverse (p_element_name);
			TraverseCommon (p_element_name);
		}

		public void InvokeTest (element_value p_element_value)
		{
			Traverse (p_element_value);
			TraverseCommon (p_element_value);
		}

		public void InvokeTest (tree_element_set p_tree_element_set)
		{
			Traverse (p_tree_element_set);
			TraverseCommon (p_tree_element_set);
		}
	}

	internal class TreeParserExample
	{
		static void Main (string [] args)
		{
			// global flags should be set before TreeParser object creation
			TreeParser.createParseTree = true;
			TreeParser.loopDetection = true;

			// parse every source file named in command line
			foreach (string arg in args)
			{
				TreeParser parser = new TreeParser ();
				TreeLexer lexer = new TreeLexer (new StreamReader (arg));

				// register method reporting syntax errors
				parser.Error_Event += (int lineno, int column, int token, string tokenString) =>
				{
					Console.Error.WriteLine ($"Syntax error: file ({arg}), line ({lineno}), column ({column}), token ({token} - {tokenString}");
					return true;	// continue parsing
				};


				// invoke parser
				if (parser.parse (lexer) != 0)
					continue;	// errors, skip current file

				// visit every node of every syntax tree generated for current source file
				foreach (var syntaxTree in parser.parseList)
				{
					// establish clear parent-child relationships
					syntaxTree.reparent (null);
					// create walker object
					TreeParser_TEST testWalker = new TreeParser_TEST ();
					// visit all nodes of syntax tree
					testWalker.Traverse (syntaxTree as tree);
				}
			}
		}
	}
}
