using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace AnglrLibrary
{
    internal class MarkerByPosition : Dictionary<(int, string), SymbolToken> { }
    internal class MarkerByName : Dictionary<string, SymbolToken> { }

    internal enum ProductionAssociativity
    {
        None,
        Left,
        Right
    }

    [Serializable]
    public class RhsProduction
    {
        internal static void Reset ()
        {
            productionCounter = 0;
        }

        internal RhsProduction (SymbolToken p_symbolToken)
        {
            symbolToken = p_symbolToken;
            productionNumber = ++productionCounter;
        }

        internal void Dispose ()
        {
            foreach (RhsNode p_RhsNode in rhsNodes)
                p_RhsNode.Dispose ();
        }

        internal int hashCode
        {
            get
            {
                if (m_hashCode != -1)
                    return m_hashCode;
                m_hashCode = 0;
                foreach (RhsNode p_RhsNode in rhsNodes)
                    m_hashCode += (p_RhsNode.symbolToken.declarator != (uint) AnglrClassificationType.TerminalName) ? p_RhsNode.symbolToken.hashCode : 1;
                return m_hashCode;
            }
        }
        private int m_hashCode = -1;

        internal void display (TextWriter writer)
        {
            if (rhsNodes.Count > 0)
                foreach (RhsNode p_node in rhsNodes)
                {
                    SymbolToken p_nodeSymbol = p_node.symbolToken;
                    if (p_nodeSymbol.alias != null)
                        p_nodeSymbol = p_nodeSymbol.alias;
                    writer.Write ($" {p_nodeSymbol.name}");
                }
            else
                writer.Write (" %empty");
            if (priority > 0)
            {
                writer.Write ($" {priority}");
                if (associativity != ProductionAssociativity.None)
                    writer.Write ($" {associativity}");
            }
            else
            {
                if (associativity != ProductionAssociativity.None)
                    writer.Write ($" {associativity}");
            }
            writer.WriteLine ();
        }

        internal void add (RhsNode node) { rhsNodes.Add (node); }
        public SymbolToken symbolToken { get; set; }
        public  int productionNumber { get; private set; }
        internal int index { get; set; }
        internal bool empty { get; set; }
        internal int priority { get; set; } = -1;
        internal ProductionAssociativity associativity { get; set; } = ProductionAssociativity.None;
        internal SymbolToken mergeTag { get; set; }
        //inline rhslist::iterator getIterator () { return rhsNodes.begin (); }
        //inline rhslist::iterator incIterator (rhslist::iterator it) { if (it != rhsNodes.end ()) ++it; return it; }
        //inline bool endIterator (rhslist::iterator it) { return it == rhsNodes.end (); }
        internal int length { get { return rhsNodes.Count; } }
        public rhslist rhsNodes { get; } = new rhslist ();
        internal MarkerByPosition markersByPosition { get; private set; } = new MarkerByPosition ();
        internal MarkerByName markersByName { get; private set; } = new MarkerByName ();
        public  SymbolToken productionName { get; set; } = null;
        internal static int productionCounter { get; private set; }
    }
}
