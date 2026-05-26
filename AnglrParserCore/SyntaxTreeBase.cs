//
//	This file was generated with ANGLR compiler
//
using System;
using System.Collections.Generic;
using Anglr.Parser.Walker;

namespace Anglr.Parser.SyntaxTree
{
    using synlist = List<SyntaxTreeBase>;

    public class SyntaxTreeError : Exception
    {
        public SyntaxTreeError (uint errorCode, string [] errorStrings) { m_errorCode = errorCode; m_errorStrings = errorStrings; }
        public uint errorCode { get; private set; }
        public string errorString { get => (m_errorCode >= g_errorCount) ? null : g_errorStrings [m_errorCode]; private set => g_errorStrings [m_errorCode] = value; }

        public const uint InvalidKindError = 1;
        private static string [] g_errorStrings = new string [] { "error1", "error2", "error3", "error", "error5" };
        private static uint g_errorCount = 5;
        private uint m_errorCode;
        private string [] m_errorStrings;
    }

    public abstract class SyntaxTreeBase : IDisposable
    {
        public SyntaxTreeBase (uint id, uint kind)
        {
            ++g_created;
            this.id = id;
            this.kind = kind;
            this.locked = false;
            this.turn = 0;
            this.parent = null;
            this.children = null;
            this.appInfo = null;
        }

        SyntaxTreeBase (SyntaxTreeBase p_syn) : this (p_syn.id, p_syn.kind) { ++g_created; }

        public void Dispose ()
        {
            ++g_deleted;
            parent = null;
        }

        public T Traverse<T> (Func<bool, SyntaxTreeBase, T, T> f, T data)
        {
            T t = f (true, this, data);
            foreach (var child in children)
                t = child.Traverse<T> (f, t);
            t = f (false, this, t);
            return t;
        }

        public uint id { get; private set; }
        public uint kind { get; set; }
        public bool locked { get; private set; }
        public uint turn { get; private set; }
        public SyntaxTreeBase parent { get; set; }
        public SyntaxTreeBase [] children { get; set; }
        public object appInfo { get; set; }

        public void dolock () => _ = enableLocking && (locked = true);
        public void unlock () => _ = enableLocking && (locked = false);
        public bool isLocked () => enableLocking && locked;
        public void turn_reset () => turn = 0;
        public void turn_inc () => ++turn;
        public void join (SyntaxTreeBase node) => m_synlist.Add (node);
        public synlist joinList () => m_synlist;
        public bool checkInclusion (SyntaxTreeBase element) => element == this;

        public abstract SyntaxTreeBase Clone ();

        public abstract string Emit (int depth);

        public abstract string EmitProductionTree (int depth);

        public abstract void reparent (SyntaxTreeBase parent);

        public abstract void InvokeTraverse (SyntaxTreeWalkerCore syntaxTreeWalker);

        public abstract void InvokeTraverseCommon (SyntaxTreeWalkerCore syntaxTreeWalker);

        public static int g_created = 0;
        public static int g_deleted = 0;
        public static int g_traverse = 0;

        public static bool enableLocking { get; set; } = true;

        protected synlist m_synlist = new synlist ();
    }

    public class SyntaxTreeToken : SyntaxTreeBase
    {
        public SyntaxTreeToken (int token, int lineno, int column, string text) : base (0, 0)
        {
            ++g_tokens;
            ++g_counter;
            this.token = token;
            this.lineno = lineno;
            this.column = column;
            this.text = text;
            children = Array.Empty<SyntaxTreeBase> ();
        }
        public SyntaxTreeToken (SyntaxTreeToken p_token) : base (0, 0)
        {
            ++g_tokens;
            ++g_counter;
            this.token = p_token.token;
            this.lineno = p_token.lineno;
            this.column = p_token.column;
            this.text = p_token.text;
            children = Array.Empty<SyntaxTreeBase> ();
        }
        public new void Dispose ()
        {
            --g_counter;
        }
        public int token { get; private set; }
        public int lineno { get; private set; }
        public int column { get; private set; }
        public string text { get; set; }
        public new bool checkInclusion (SyntaxTreeBase element) => element == this;
        public override string Emit (int depth) => (depth != 0) ? text : "";
        public override string EmitProductionTree (int depth) => "";
        public override SyntaxTreeBase Clone () => new SyntaxTreeToken (this);
        public override void reparent (SyntaxTreeBase parent) => this.parent = parent;
        public override void InvokeTraverse (SyntaxTreeWalkerCore syntaxTreeWalker) => syntaxTreeWalker.Traverse (this);
        public override void InvokeTraverseCommon (SyntaxTreeWalkerCore syntaxTreeWalker) => syntaxTreeWalker.TraverseCommon (this);

        public static int g_tokens = 0;
        public static int g_counter = 0;
    }
}
