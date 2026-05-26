using AnglrLogLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnglrLibrary
{
    [Serializable]
    public class SymbolTable
    {
        public SymbolTable () { }
        private void Dispose ()
        {
            init ();
        }

        public void init ()
        {
            _init ();
        }

        public SymbolToken insert (SymbolToken symbol)
        {
            return _insert (symbol);
        }

        public SymbolToken find (SymbolToken symbol)
        {
            return _find (symbol);
        }

        public void print (IAnglrLogger logger)
        {
            _print (logger);
        }

        public int createSymbolList (tokvec p_tokvec, uint declarator, SymbolToken symbolToken = null)
        {
            return _createSymbolList (p_tokvec, declarator, symbolToken);
        }

        //public reftab getRefTab ()
        //{
        //	return _GetRefTab ();
        //}

        private void _init ()
        {
            m_symtab.Clear ();
        }

        private SymbolToken _insert (SymbolToken symbol)
        {
            SymbolToken symbolRef = null;
            return (m_symtab.TryGetValue (symbol, out symbolRef)) ? symbolRef : (m_symtab [symbol] = symbol);
        }

        private SymbolToken _find (SymbolToken symbol)
        {
            SymbolToken symbolRef = null;
            return (m_symtab.TryGetValue (symbol, out symbolRef)) ? symbolRef : null;
        }

        private void _print (IAnglrLogger logger)
        {
            foreach (SymbolToken p_SymbolToken in m_symtab.Keys)
            {
                if (p_SymbolToken.name.Length == 0)
                    continue;
                logger?.DebugRawLine<SymbolToken>
                (
                    (symbol) =>
                    {
                        StringBuilder sb = new StringBuilder ();
                        if (symbol.AliasFlag)
                            sb.Append ("alias = " + symbol.name);
                        else
                            sb.Append ("name = " + symbol.name);
                        if (symbol.id >= 0)
                            sb.Append (", id = " + symbol.id);
                        if (symbol.tag != null)
                            sb.Append (", tag = " + symbol.tag.name);
                        if (symbol.alias != null)
                        {
                            SymbolToken p_SymbolTokenRef = symbol.alias;
                            if (p_SymbolTokenRef.name != null)
                                if (p_SymbolTokenRef.AliasFlag)
                                    sb.Append (", alias = " + p_SymbolTokenRef.name);
                                else
                                    sb.Append (", name = " + p_SymbolTokenRef.name);
                        }
                        sb.AppendLine ();
                        return sb.ToString ();
                    },
                    p_SymbolToken
                );
                logger?.DebugRawLine<SymbolToken>
                (
                    (symbol) =>
                    {
                        StringBuilder sb = new StringBuilder ();
                        sb.Append ("\tdefinitions:");
                        foreach (SymbolReference p_SymbolReference in symbol.m_deflist)
                            sb.Append (" (" + p_SymbolReference.lineno + ", " + p_SymbolReference.column + ")");
                        sb.AppendLine ();
                        return sb.ToString ();
                    },
                    p_SymbolToken
                );
                logger?.DebugRawLine<SymbolToken>
                (
                    (symbol) =>
                    {
                        StringBuilder sb = new StringBuilder ();
                        sb.Append ("\treferences:");
                        foreach (SymbolReference p_SymbolReference in symbol.m_reflist)
                            sb.Append (" (" + p_SymbolReference.lineno + ", " + p_SymbolReference.column + ")");
                        sb.AppendLine ();
                        return sb.ToString ();
                    },
                    p_SymbolToken
                );
            }
        }

        private int _createSymbolList (tokvec p_tokvec, uint declarator, SymbolToken symbolToken)
        {
            int index = 0;
            foreach (SymbolToken p_SymbolToken in m_symtab.Keys)
            {
                if (p_SymbolToken.declarator != declarator)
                    continue;
                if (p_SymbolToken.AliasFlag)
                    continue;
                if ((symbolToken != null) && (p_SymbolToken.context != symbolToken))
                        continue;
                p_tokvec.Add (p_SymbolToken);
                ++index;
            }
            return index;
        }

        //private reftab _GetRefTab ()
        //{
        //	reftab keys = new reftab ();
        //	foreach (SymbolToken symbol in m_symtab.Keys)
        //	{
        //		foreach (SymbolReference reference in symbol.m_reflist)
        //			keys [(reference.lineno, reference.column, reference.length, true)] = symbol;
        //		foreach (SymbolReference reference in symbol.m_deflist)
        //			keys [(reference.lineno, reference.column, reference.length, false)] = symbol;
        //	}
        //	return keys;
        //}

        symtab m_symtab = new symtab ();
        public symtab.Enumerator enumerator { get { return m_symtab.GetEnumerator (); } }
    }
}
