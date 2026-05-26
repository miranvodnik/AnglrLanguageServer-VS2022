//
//	This file was generated with ANGLR compiler
//
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Anglr.Parser.SyntaxTree;
using AnglrLogLibrary;

namespace Anglr.Parser.Core
{
    public class AnglrBitField
    {
        public byte [] BitField { get; private set; }
        public uint Size { get; private set; }
        public AnglrBitField (uint size)
        {
            Size = size;
            BitField = new byte [(size / 8) + 1];
            for (int i = 0; i < BitField.Length; i++)
                BitField [i] = 0;
        }
        public void Set (uint bit) => BitField [bit / 8] |= (byte) (1 << (int) (bit & 0x7));
        public void Clear (uint bit) => BitField [bit / 8] &= (byte) ~(1 << (int) (bit & 0x7));
        public byte Check (uint bit) => (byte) (BitField [bit / 8] & (1 << (int) (bit & 0x7)));
    }

    public enum SyntaxTreeCallbackReason
    {
        BuilderCallbackReason = 1,
        IterationPrologueCallbackReason = 2,
        TraversalPrologueCallbackReason = 3,
        TraversalMidTermCallbackReason = 4,
        TraversalEpilogueCallbackReason = 5,
        IterationEpilogueCallbackReason = 6
    };

    // Typedefs

    public class stacklist : List<ParserStack> { }
    public class valstack : ValueStack<SyntaxTreeBase>
    {
        public valstack () : base () { }
        public valstack (valstack val) : base (val) { }
        public string [] texts
        {
            get
            {
                string [] t = (stackDepth > 0) ? new string [stackDepth] : Array.Empty<string> ();
                int index = 0;
                foreach (var val in this)
                {
                    int lineno = -1;
                    int column = 0;
                    bool started = true;
                    t [index++] = val.Traverse<StringBuilder>
                    (
                        (l, n, s) =>
                        {
                            if (!(l && (n is SyntaxTreeToken)))
                                return s;

                            SyntaxTreeToken token = n as SyntaxTreeToken;
                            int tokenlineno = token.lineno;
                            int tokenColumn = token.column;
                            string tokenText = token.text;
                            int tokenLength = tokenText.Length;

                            if (started)
                            {
                                lineno = tokenlineno;
                                started = false;
                            }
                            else
                            {
                                while (tokenlineno > lineno)
                                {
                                    s.AppendLine ();
                                    ++lineno;
                                    column = 0;
                                }
                            }

                            if (column < tokenColumn)
                            {
                                s.Append (' ', tokenColumn - column);
                                column = tokenColumn;
                            }

                            try
                            {
                                s.Append (tokenText);
                                column += tokenLength;
                            }
                            catch (Exception)
                            {
                            }

                            return s;
                        },
                        new StringBuilder ()
                    ).ToString ();
                }
                return t;
            }
        }
    }
    public class intstack : ValueStack<int>
    {
        public intstack () : base () { }
        public intstack (intstack val) : base (val) { }
    }
    public class statestack : Stack<StateStack>
    {
        public statestack () : base () { }
        public statestack (statestack val) : base (val) { }
    }
    public class stackset : SortedSet<ParserStack>
    {
        public stackset () : base () { }
        public stackset (stackset val) : base (val) { }
        public stackset (cmpstackid val) : base (val) { }
        public stackset (cmpstackinfo val) : base (val) { }
        public string [] GetStackText (int stackNr)
        {
            foreach (var stack in this)
            {
                if (stack.stackCounter != stackNr)
                    continue;
                return stack.valueStack.texts;
            }
            return null;
        }
    }
    public class stackque : Queue<ParserStack> { }
    public class parselist : List<SyntaxTreeBase> { }

    public class ValueStack<T> : IEnumerator<T>
    {
        public IEnumerator<T> GetEnumerator () => this;

        public T Current => m_values [m_currentPosition];

        object IEnumerator.Current => Current;

        public void Dispose () { }

        public bool MoveNext () => m_currentPosition-- > 0;

        public void Reset () => m_currentPosition = m_stackTop;

        public ValueStack ()
        {
            m_stackSize = 0;
            m_stackTop = 0;
            m_values = null;
        }

        public ValueStack (ValueStack<T> stack) : this ()
        {
            if (stack.m_values != null)
            {
                m_stackSize = stack.m_stackSize;
                m_stackTop = stack.m_stackTop;
                m_values = new T [m_stackSize = stack.m_stackSize];
                Array.Copy (stack.m_values, m_values, m_stackTop);
            }
        }

        public void Resize ()
        {
            if (m_stackTop < m_stackSize)
                return;
            m_stackSize = ((m_stackSize >> 6) + 1) << 6;
            if (m_values == null)
                m_values = new T [m_stackSize];
            else
                Array.Resize<T> (ref m_values, m_stackSize);
        }

        public void Reduce (int size)
        {
            if (size > m_stackTop)
                throw new Exception ("reduction size overflow");
            m_stackTop -= size;
        }

        public void Push (T value)
        {
            Resize ();
            m_values [m_stackTop++] = value;
        }

        public T Pop ()
        {
            if (m_stackTop <= 0)
                throw new Exception ("try to pop empty stack");
            return m_values [--m_stackTop];
        }

        public T Peek ()
        {
            if (m_stackTop <= 0)
                throw new Exception ("try to peek empty stack");
            return m_values [m_stackTop - 1];
        }

        public T this [int i]
        {
            get
            {
                int index = m_stackTop + i;
                if ((index < 0) || (index >= m_stackTop))
                    throw new Exception ("index out of range");
                return m_values [index];
            }
        }

        public int stackDepth { get => m_stackTop; }

        private int m_currentPosition = 0;
        private int m_stackSize = 0;
        private int m_stackTop = 0;
        private T [] m_values = null;
    }

    public enum StepOutcome
    {
        NoStep,
        ResizeFailed,
        SyntaxError,
        TokenUnavailable,
        ShiftStep,
        ReduceStep,
        SplitStep,
        LoopStep,
        FinalStep
    }

    public class TerminalCodes
    {
        public static readonly uint token_error = 2;
        public static readonly uint token__eof_token = 1;
    }

    public struct StateStack
    {
        public int m_state;
        public int m_ruleNr;
        public int m_depth;
    };

    public class ParserToken
    {
        public ParserToken (int token, int secondary, string tokenText)
        {
            this.sequenceNr = 0;
            this.token = token;
            this.secondary = secondary;
            this.lineno = -1;
            this.column = -1;
            this.tokenText = tokenText;
        }
        public void Load (int token, int secondary, int lineno, int column, string tokenText)
        {
            ++this.sequenceNr;
            this.token = token;
            this.secondary = secondary;
            this.lineno = lineno;
            this.column = column;
            this.tokenText = tokenText;
        }

        public int sequenceNr { get; private set; }
        public int token { get; private set; }
        public int secondary { get; private set; }
        public int lineno { get; private set; }
        public int column { get; private set; }
        public string tokenText { get; private set; }
    }

    public class ParserStack
    {
        public ParserStack (ParserInterface p_AnglrParser)
        {
            AnglrParserObj = p_AnglrParser;
            AnglrLogger = p_AnglrParser?.AnglrLogger ?? new VoidAnglrLogger ();
            stackCounter = ++g_stackCounter;
            valueStack = new valstack ();
            stateStack = new intstack ();
            if (AnglrParserObj.checkLoopDetection ())
                loopStack = new statestack ();
            currentState = p_AnglrParser.InitialState;
            currentValue = null;
            token = AnglrParserObj.getToken ();
            sequenceNr = 0;
            tokenValue = 0;
            SaveState ();
            AnglrLogger?.DebugRawLine ("CREATE " + stackCounter);
            m_minTerminalCode = AnglrParserObj.minTerminalCode ();
            m_terminalCodes = AnglrParserObj.terminalCodes ();
            m_terminalNames = AnglrParserObj.terminalNames ();
            m_minNonTerminalCode = AnglrParserObj.minNonTerminalCode ();
            m_nonTerminalCodes = AnglrParserObj.nonTerminalCodes ();
            m_nonTerminalNames = AnglrParserObj.nonTerminalNames ();
            m_glrcheck = AnglrParserObj.glrcheck ();
            m_glrstate = AnglrParserObj.glrstate ();
            m_glrcells = AnglrParserObj.glrcells ();
            m_check = AnglrParserObj.check ();
            m_state = AnglrParserObj.state ();
            m_shiftDelta = AnglrParserObj.shiftDelta ();
            m_gotoDelta = AnglrParserObj.gotoDelta ();
            m_productionLengths = AnglrParserObj.productionLengths ();
            m_productionRules = AnglrParserObj.productionRules ();
            m_defaultGoto = AnglrParserObj.defaultGoto ();
            m_rcheck = AnglrParserObj.rcheck ();
            m_rstate = AnglrParserObj.rstate ();
            m_reductions = AnglrParserObj.reductions ();
        }

        public ParserStack (ParserStack p_ParserStack)
        {
            AnglrParserObj = p_ParserStack.AnglrParserObj;
            stackCounter = ++g_stackCounter;
            stateStack = new intstack (p_ParserStack.stateStack);
            valueStack = new valstack (p_ParserStack.valueStack);
            currentState = stateStack.Peek ();
            currentValue = valueStack.Peek ();
            if (AnglrParserObj.checkLoopDetection ())
                loopStack = new statestack (p_ParserStack.loopStack);
            token = p_ParserStack.token;
            sequenceNr = p_ParserStack.sequenceNr;
            tokenValue = p_ParserStack.tokenValue;
            tokenText = p_ParserStack.tokenText;
            AnglrLogger?.DebugRawLine ("CREATE " + stackCounter);
            m_minTerminalCode = AnglrParserObj.minTerminalCode ();
            m_terminalCodes = AnglrParserObj.terminalCodes ();
            m_terminalNames = AnglrParserObj.terminalNames ();
            m_minNonTerminalCode = AnglrParserObj.minNonTerminalCode ();
            m_nonTerminalCodes = AnglrParserObj.nonTerminalCodes ();
            m_nonTerminalNames = AnglrParserObj.nonTerminalNames ();
            m_glrcheck = AnglrParserObj.glrcheck ();
            m_glrstate = AnglrParserObj.glrstate ();
            m_glrcells = AnglrParserObj.glrcells ();
            m_check = AnglrParserObj.check ();
            m_state = AnglrParserObj.state ();
            m_shiftDelta = AnglrParserObj.shiftDelta ();
            m_gotoDelta = AnglrParserObj.gotoDelta ();
            m_productionLengths = AnglrParserObj.productionLengths ();
            m_productionRules = AnglrParserObj.productionRules ();
            m_defaultGoto = AnglrParserObj.defaultGoto ();
            m_rcheck = AnglrParserObj.rcheck ();
            m_rstate = AnglrParserObj.rstate ();
            m_reductions = AnglrParserObj.reductions ();
        }

        public bool CheckTokenInState (int token, int state)
        {
            int stateDelta = m_shiftDelta [state];
            if (stateDelta < 0)
            {
                stateDelta = -stateDelta;
                int index = stateDelta + token;
                if ((index < 0) || (index >= m_glrcheck.Length) || (m_glrcheck [index] != token))
                    return false;
                return true;
            }

            if (stateDelta != 0)
            {
                int index = stateDelta + token;
                if (!((index < 0) || (index >= m_check.Length) || (m_check [index] != token)))
                    return true;
            }

            int productionNr = m_reductions [currentState];
            if (productionNr != 0)
            {
                if (productionNr < 0)
                {
                    int index = token - productionNr;
                    if ((index < 0) || (index >= m_rcheck.Length) || (m_rcheck [index] != token))
                        return false;
                }
                return true;
            }
            return false;
        }

        public StepOutcome Step (stacklist insStackList, stacklist delStackList)
        {
            int token;

            int stateDelta = m_shiftDelta [currentState];
            if (stateDelta < 0)
            {
                stateDelta = -stateDelta;
                if ((token = tokenValue) == 0)

                    return StepOutcome.TokenUnavailable;

                int index = stateDelta + token;
                if ((index < 0) || (index >= m_glrcheck.Length) || (m_glrcheck [index] != token))
                {
                    return StepOutcome.SyntaxError;
                }

                int delta = m_glrstate [index];
                int size = m_glrcells [delta];
                int state = m_glrcells [delta + 1];

                if (size < 3)
                {
                    AnglrParserObj.ShiftStepReport (this.stackCounter, state, tokenValue, m_terminalNames [tokenValue - m_minTerminalCode], tokenText, false);
                    Shift (state);
                    return StepOutcome.ShiftStep;
                }
                else if ((size == 3) && (state < 0))
                    return Reduce (m_glrcells [delta + 2]);

                AnglrLogger?.DebugRawLine ("SPLIT  " + stackCounter);
                if (state >= 0)
                {
                    ParserStack stack = new ParserStack (this); //m_anglrParser.allocateStack (this);
                    AnglrParserObj.SplitStepReport (this.stackCounter, stack.stackCounter, true);
                    AnglrParserObj.ShiftStepReport (stack.stackCounter, state, tokenValue, m_terminalNames [tokenValue - m_minTerminalCode], tokenText, true);
                    stack.Shift (state);
                    insStackList.Add (stack);
                }
                for (int count = 2; count < size; ++count)
                {
                    ParserStack stack = new ParserStack (this); //m_anglrParser.allocateStack (this);
                    AnglrParserObj.SplitStepReport (this.stackCounter, stack.stackCounter, true);
                    switch (stack.Reduce (m_glrcells [delta + count]))
                    {
                        case StepOutcome.ReduceStep:
                            insStackList.Add (stack);
                            break;
                        case StepOutcome.LoopStep:
                            AnglrParserObj.LoopStepReport (this.stackCounter, m_glrcells [delta + count]);
                            delStackList.Add (stack);
                            break;
                        default:
                            stack = null;
                            break;
                    }
                }
                AnglrLogger?.DebugRawLine ("SPLIT  END");
                AnglrParserObj.SplitStepReport (this.stackCounter, currentState, false);
                return StepOutcome.SplitStep;
            }

            if (stateDelta != 0)
            {
                if ((token = tokenValue) == 0)
                    return StepOutcome.TokenUnavailable;
                int index = stateDelta + token;
                if (!((index < 0) || (index >= m_check.Length) || (m_check [index] != token)))
                {
                    int state = m_state [index];
                    if ((tokenValue < m_minTerminalCode))
                        AnglrParserObj.ShiftStepReport (this.stackCounter, state, tokenValue, "", tokenText, false);
                    else
                        AnglrParserObj.ShiftStepReport (this.stackCounter, state, tokenValue, m_terminalNames [tokenValue - m_minTerminalCode], tokenText, false);
                    Shift (state);
                    return StepOutcome.ShiftStep;
                }
            }

            int productionNr = m_reductions [currentState];
            if (productionNr != 0)
            {
                if (productionNr < 0)
                {
                    if ((token = tokenValue) == 0)
                        return StepOutcome.TokenUnavailable;
                    int index = token - productionNr;
                    if ((index < 0) || (index >= m_rcheck.Length) || (m_rcheck [index] != token))
                    {
                        return StepOutcome.SyntaxError;
                    }
                    productionNr = m_rstate [index];

                }

                return Reduce (productionNr);
            }
            else
            {
                return StepOutcome.SyntaxError;
            }

            //return StepOutcome.NoStep;
        }

        public SyntaxTreeBase Build (int reductionNr, int prodLen) => AnglrParserObj.Build (this, reductionNr, prodLen);

        public void Shift (int state)
        {
            AnglrLogger?.DebugRawLine ("SHIFT  " + stackCounter + " '" + ((tokenText != null) ? tokenText : "") + "' (" + tokenValue + ") : " + currentState + " --> " + state);
            currentState = state;
            if (AnglrParserObj.checkCreateParseTree ())
                currentValue = new SyntaxTreeToken (tokenValue, token.lineno, token.column, tokenText);
            tokenValue = 0;
            tokenText = "";
            if (AnglrParserObj.checkLoopDetection ())
                loopStack.Clear ();
            ++sequenceNr;
            SaveState ();
        }

        public StepOutcome Reduce (int productionNr)
        {
            int fallingState = currentState;
            int ruleNr = m_productionRules [productionNr];
            int prodLen = m_productionLengths [productionNr];
            if ((prodLen >= stackDepth) || (ruleNr <= 0))
            {
                AnglrLogger?.DebugRawLine ("ACCEPT " + stackCounter + " '" + ((tokenText != null) ? tokenText : "") + "' (" + tokenValue + ") <" + productionNr + ", " + ruleNr + ", " + prodLen + "> : " + currentState);
                AnglrParserObj.FinalStepReport (this.stackCounter);
                return StepOutcome.FinalStep;
            }
            if (AnglrParserObj.checkCreateParseTree ())
                currentValue = Build (productionNr, prodLen);
            stateStack.Reduce (prodLen);
            int state = stateStack.Peek ();
            int bottomState = state;
            int idx = ruleNr + m_gotoDelta [state];
            if ((idx < 0) || (idx >= m_check.Length) || (m_check [idx] != ruleNr))
                currentState = m_defaultGoto [ruleNr];
            else
                currentState = m_state [idx];
            int risingState = currentState;
            AnglrLogger?.DebugRawLine ($"REDUCE {stackCounter} '{((tokenText != null) ? tokenText : "")}' ({tokenValue}) <{productionNr}, {ruleNr}, {prodLen}> : {fallingState} --> {bottomState} --> {risingState}");
            if (currentState < 0)
            {
                AnglrParserObj.FinalStepReport (this.stackCounter);
                return StepOutcome.FinalStep;
            }

            bool loop = false;
            if (AnglrParserObj.checkLoopDetection ())
            {
                foreach (StateStack cstate in loopStack)

                {
                    if ((cstate.m_state == state) && (cstate.m_ruleNr == productionNr) && (cstate.m_depth == stackDepth))
                    {
                        loop = true;
                        break;
                    }
                }

                StateStack item = new StateStack ();
                item.m_state = state;
                item.m_ruleNr = productionNr;
                item.m_depth = stackDepth;
                loopStack.Push (item);
            }

            valueStack.Reduce (prodLen);
            AnglrParserObj.ReduceStepReport (this.stackCounter, productionNr, ruleNr, m_nonTerminalNames [ruleNr - m_minNonTerminalCode], prodLen, fallingState, bottomState, risingState, currentValue, valueStack.stackDepth <= 1);
            SaveState ();
            return loop ? StepOutcome.LoopStep : StepOutcome.ReduceStep;
        }

        public void LoadToken ()
        {
            tokenValue = token.token;
            tokenText = token.tokenText;
        }

        public void LoadSecondary ()
        {
            tokenValue = token.secondary;
            tokenText = token.tokenText;
        }

        public int Join (ParserStack stack)
        {
            int cnt = 0;
            if (stack.stackDepth != stackDepth)
                return cnt;
            valstack valStack = stack.valueStack;
            for (int i = -stackDepth; i < 0; ++i)
            {
                SyntaxTreeBase lvalue = valueStack [i];
                SyntaxTreeBase rvalue = valStack [i];
                if (lvalue != rvalue)
                {
                    lvalue.join (rvalue);
                    ++cnt;
                }
            }
            return cnt;
        }

        private void SaveState ()
        {
            stateStack.Push (currentState);
            valueStack.Push (currentValue);
        }

        public IAnglrLogger AnglrLogger;
        public static int g_stackCounter;
        public int stackCounter { get; private set; }
        public ParserInterface AnglrParserObj { get; private set; }
        public valstack valueStack { get; private set; }
        public intstack stateStack { get; private set; }
        public int stackDepth { get { return stateStack.stackDepth; } }
        public statestack loopStack { get; private set; }
        public int currentState { get; private set; }
        public SyntaxTreeBase currentValue { get; private set; }
        public ParserToken token { get; private set; }
        public int tokenValue { get; private set; }
        public string tokenText { get; private set; }
        public int sequenceNr { get; set; }
        public ParserStack m_equivalent { get; set; }

        internal int m_minTerminalCode;
        internal int [] m_terminalCodes;
        internal string [] m_terminalNames;
        internal int m_minNonTerminalCode;
        internal int [] m_nonTerminalCodes;
        internal string [] m_nonTerminalNames;
        internal int [] m_glrcheck;
        internal int [] m_glrstate;
        internal int [] m_glrcells;
        internal int [] m_check;
        internal int [] m_state;
        internal int [] m_shiftDelta;
        internal int [] m_gotoDelta;
        internal int [] m_productionLengths;
        internal int [] m_productionRules;
        internal int [] m_defaultGoto;
        internal int [] m_rcheck;
        internal int [] m_rstate;
        internal int [] m_reductions;
    }

    public class cmpstackinfo : IComparer<ParserStack>
    {
        public int Compare (ParserStack x, ParserStack y)
        {
            if (x.tokenValue < y.tokenValue)
                return -1;
            if (y.tokenValue < x.tokenValue)
                return 1;
            int xsize = x.stackDepth;
            int ysize = y.stackDepth;
            if (xsize < ysize)
                return -1;
            if (ysize < xsize)
                return 1;
            intstack xstack = x.stateStack;
            intstack ystack = y.stateStack;
            for (int i = -xstack.stackDepth; i < 0; ++i)
            {
                int xval = xstack [i];
                int yval = ystack [i];
                if (xval < yval)
                    return -1;
                if (xval > yval)
                    return 1;
            }
            x.m_equivalent = y;
            y.m_equivalent = x;
            return 0;

        }
    }

    public class cmpstackid : IComparer<ParserStack>
    {
        public int Compare (ParserStack x, ParserStack y)
        {
            return x.stackCounter - y.stackCounter;
        }
    }

    public delegate void StartParserDelegate (int stackNr, object [] info = null);
    public delegate void SyntaxErrorDelegate (int stackNr, int state);
    public delegate void ShiftStepDelegate (int stackNr, int state, int tokenValue, string tokenName, string tokenText, bool conflict);
    public delegate void ReduceStepDelegate (int stackNr, int prodNr, int ruleNr, string ruleName, int prodLen, int fallingState, int bottomState, int risingState, SyntaxTreeBase currentValue, bool conflict);
    public delegate void SplitStepDelegate (int stackNr, int state, bool begin);
    public delegate void LoopStepDelegate (int stackNr, int state);
    public delegate void JoinDelegate (int stackNr, int joinNr);
    public delegate void FinalStepDelegate (int stackNr);
    public delegate void StopParserDelegate ();

    public interface ParserInterface
    {
        IAnglrLogger AnglrLogger { get; }
        event StartParserDelegate StartParserEvent;
        event SyntaxErrorDelegate SyntaxErrorEvent;
        event ShiftStepDelegate ShiftStepEvent;
        event ReduceStepDelegate ReduceStepEvent;
        event SplitStepDelegate SplitStepEvent;
        event LoopStepDelegate LoopStepEvent;
        event JoinDelegate JoinEvent;
        event FinalStepDelegate FinalStepEvent;
        event StopParserDelegate StopParserEvent;

        SyntaxTreeBase Build (ParserStack stack, int reductionNr, int prodLen);
        void StartParserReport (int stackNr, object [] info = null);
        void SyntaxErrorReport (int stackNr, int state);
        void ShiftStepReport (int stackNr, int state, int tokenValue, string tokenName, string tokenText, bool conflict);
        void ReduceStepReport (int stackNr, int prodNr, int ruleNr, string ruleName, int prodLen, int fallingState, int bottomState, int risingState, SyntaxTreeBase currentValue, bool bottom);
        void SplitStepReport (int stackNr, int state, bool begin);
        void LoopStepReport (int stackNr, int state);
        void JoinReport (int stackNr, int joinNr);
        void FinalStepReport (int stackNr);
        string [] GetStackText (int stackNr);
        void StopParserReport ();
        ParserToken getToken ();
        bool checkLoopDetection ();
        bool checkDebug ();
        bool checkCreateParseTree ();
        bool FragmentParser { get; set; }
        int ProductionID { get; set; }
        int InitialState { get; set; }
        int LastToken { get; set; }
        string LastTokenName { get; set; }
        int magicNumber ();
        int minTerminalCode ();
        int [] terminalCodes ();
        string [] terminalNames ();
        int minNonTerminalCode ();
        int [] nonTerminalCodes ();
        string [] nonTerminalNames ();
        int [] glrcheck ();
        int [] glrstate ();
        int [] glrcells ();
        int [] check ();
        int [] state ();
        int [] shiftDelta ();
        int [] gotoDelta ();
        int [] productionLengths ();
        int [] productionRules ();
        int [] defaultGoto ();
        int [] rcheck ();
        int [] rstate ();
        int [] reductions ();
    }
}
