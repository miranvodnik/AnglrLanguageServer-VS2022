using AnglrLogLibrary;
using System;
using System.Collections.Generic;
using System.Diagnostics.SymbolStore;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnglrLibrary
{
    [Serializable]
    public class RhsState
    {
        internal RhsState (SymbolToken p_SymbolToken, prodlist p_prodset)
        {
            m_SymbolToken = p_SymbolToken;
            m_prodset = p_prodset;
            m_id = ++m_created;
        }

        internal void Dispose ()
        {
            ++m_deleted;
            foreach (KeyValuePair<int, RhsConfiguration> keyval in m_reductions)
            {
                RhsConfiguration p_RhsConfiguration = keyval.Value;
                if (!m_core.Keys.Contains (p_RhsConfiguration))
                    continue;
                p_RhsConfiguration.Dispose ();
            }
            m_reductions.Clear ();

            foreach (RhsConfiguration p_RhsConfiguration in m_core.Values)
                p_RhsConfiguration.Dispose ();
            m_core.Clear ();

            foreach (RhsClosureElt p_RhsClosureElt in m_closure.Values)
                p_RhsClosureElt.Dispose ();
            m_closure.Clear ();

            ShiftArray.Clear ();
            m_shiftset.Clear ();
            GotoArray.Clear ();
            m_gotoset.Clear ();
        }

        internal void makeClosure ()
        {
            int low = 0;
            int high = 0;
            prodarray closureArray = new prodarray ();

            m_closure.Clear ();
            foreach (KeyValuePair<RhsConfiguration, RhsConfiguration> keyval in m_core)
            {
                RhsConfiguration cfg = keyval.Value;
                RhsProduction p_rhsProduction = cfg.rhsProduction;
                rhsenumerator rhsit = cfg.rhsIterator;
                if (rhsit.atEnd)
                    continue;
                RhsNode p_RhsNode = rhsit.currentRhsNode;
                SymbolToken p_SymbolToken = p_RhsNode.symbolToken;
                if (p_SymbolToken.declarator != (uint) AnglrClassificationType.NonTerminalName)
                    continue;
                RhsProductionNode p_RhsProductionNode = null;
                if (!m_prodset.TryGetValue (p_SymbolToken, out p_RhsProductionNode))
                    continue;
                if (p_RhsProductionNode == null)
                    continue;
                RhsClosureElt p_pt = null;
                if (!m_closure.TryGetValue (p_SymbolToken, out p_pt))
                {
                    p_pt = new RhsClosureElt (p_RhsProductionNode);
                    m_closure [p_SymbolToken] = p_pt;
                    closureArray.Add (p_pt);
                    ++high;
                }
            }
            int lowcount = low;
            while (true)
            {
                int count = 0;
                while (low < high)
                {
                    RhsClosureElt pt = closureArray [low++];
                    RhsProductionNode p_RhsProductionNode = pt.rhsProductionNode;
                    SymbolToken p_SymbolToken = p_RhsProductionNode.ProductionName;
                    foreach (RhsProduction p_RhsProduction in p_RhsProductionNode.Productions)
                    {
                        if (p_RhsProduction.rhsNodes.Count <= 0)
                            continue;
                        RhsNode p_RhsNode = p_RhsProduction.rhsNodes [0];
                        SymbolToken p_SymbolTokenRef = p_RhsNode.symbolToken;
                        if (p_SymbolTokenRef.declarator != (uint) AnglrClassificationType.NonTerminalName)
                            continue;
                        RhsProductionNode p_RhsProductionNodeRef = null;
                        if (!m_prodset.TryGetValue (p_SymbolTokenRef, out p_RhsProductionNodeRef))
                            continue;
                        if (p_RhsProductionNodeRef == null)
                            continue;
                        RhsClosureElt p_pt = null;
                        if (!m_closure.TryGetValue (p_SymbolTokenRef, out p_pt))
                        {
                            p_pt = new RhsClosureElt (p_RhsProductionNodeRef);
                            m_closure [p_SymbolTokenRef] = p_pt;
                            closureArray.Add (p_pt);
                            ++high;
                        }
                    }
                }
                if (count == 0)
                    break;
                low = lowcount;
            }
        }

        internal void makeStates (IAnglrLogger logger)
        {
            foreach (KeyValuePair<RhsConfiguration, RhsConfiguration> keyval in m_core)
            {
                RhsConfiguration p_RhsConfiguration = keyval.Value;
                RhsProduction p_RhsProduction = p_RhsConfiguration.rhsProduction;
                rhsenumerator rhsit = p_RhsConfiguration.rhsIterator;
                if (rhsit.atEnd)
                {
                    int productionNumber = p_RhsProduction.productionNumber;
                    if (!m_reductions.Keys.Contains (productionNumber))
                        m_reductions [productionNumber] = p_RhsConfiguration;
                    else
                        logger?.WarnLine ("production already reduced = " + productionNumber);
                    continue;
                }
                RhsNode p_RhsNode = rhsit.currentRhsNode;
                SymbolToken p_SymbolToken = p_RhsNode.symbolToken;
                if (p_SymbolToken.declarator == (uint) AnglrClassificationType.TerminalName)
                {
                    RhsState p_RhsState = null;
                    if (!m_shiftset.TryGetValue (p_SymbolToken, out p_RhsState))
                    {
                        m_shiftset [p_SymbolToken] = p_RhsState = new RhsState (p_SymbolToken, m_prodset);
                        ShiftArray.Add (p_RhsState);
                    }
                    p_RhsState.add (new RhsConfiguration (p_RhsProduction, new rhsenumerator (rhsit)));
                    p_RhsState.AddRefState (this);
                }
                else if (p_SymbolToken.declarator == (uint) AnglrClassificationType.NonTerminalName)
                {
                    RhsState p_RhsState = null;
                    if (!m_gotoset.TryGetValue (p_SymbolToken, out p_RhsState))
                    {
                        m_gotoset [p_SymbolToken] = p_RhsState = new RhsState (p_SymbolToken, m_prodset);
                        GotoArray.Add (p_RhsState);
                    }
                    p_RhsState.add (new RhsConfiguration (p_RhsProduction, new rhsenumerator (rhsit)));
                    p_RhsState.AddRefState (this);
                }
                else
                {
                    logger?.WarnLine ("symbol '" + p_SymbolToken.name + "' should be token or non-terminal");
                    continue;
                }
            }
            foreach (KeyValuePair<SymbolToken, RhsClosureElt> keyval in m_closure)
            {
                RhsClosureElt pt = keyval.Value;
                RhsProductionNode p_RhsProductionNode = pt.rhsProductionNode;
                foreach (RhsProduction p_RhsProduction in p_RhsProductionNode.Productions)
                {
                    rhsenumerator rhsit = new rhsenumerator (p_RhsProduction.rhsNodes);
                    if (rhsit.atEnd)
                    {
                        int productionNumber = p_RhsProduction.productionNumber;
                        if (!m_reductions.Keys.Contains (productionNumber))
                            m_reductions [productionNumber] = new RhsConfiguration (p_RhsProduction, rhsit);
                        continue;
                    }
                    RhsNode p_RhsNode = rhsit.currentRhsNode;
                    SymbolToken p_SymbolToken = p_RhsNode.symbolToken;
                    if (p_SymbolToken.declarator == (uint) AnglrClassificationType.TerminalName)
                    {
                        RhsState p_RhsState = null;
                        if (!m_shiftset.TryGetValue (p_SymbolToken, out p_RhsState))
                        {
                            m_shiftset [p_SymbolToken] = p_RhsState = new RhsState (p_SymbolToken, m_prodset);
                            ShiftArray.Add (p_RhsState);
                        }
                        p_RhsState.add (new RhsConfiguration (p_RhsProduction, new rhsenumerator (rhsit)));
                        p_RhsState.AddRefState (this);
                    }
                    else if (p_SymbolToken.declarator == (uint) AnglrClassificationType.NonTerminalName)
                    {
                        RhsState p_RhsState = null;
                        if (!m_gotoset.TryGetValue (p_SymbolToken, out p_RhsState))
                        {
                            m_gotoset [p_SymbolToken] = p_RhsState = new RhsState (p_SymbolToken, m_prodset);
                            GotoArray.Add (p_RhsState);
                        }
                        p_RhsState.add (new RhsConfiguration (p_RhsProduction, new rhsenumerator (rhsit)));
                        p_RhsState.AddRefState (this);
                    }
                    else
                    {
                        logger?.WarnLine ("symbol '" + p_SymbolToken.name + "' should be token or non-terminal");
                        continue;
                    }
                }
            }
        }

        internal int computeFollowSets (IAnglrLogger logger)
        {
            int total = 0;
            int count;

            do
            {
                count = 0;
                foreach (RhsConfiguration p_RhsConfiguration in m_core.Values)
                {
                    RhsProduction p_RhsProduction = p_RhsConfiguration.rhsProduction;
                    rhsenumerator rhsit = p_RhsConfiguration.rhsIterator;
                    if (rhsit.atEnd)
                        continue;
                    RhsNode p_rhsNode = rhsit.currentRhsNode;
                    SymbolToken p_SymbolToken = p_rhsNode.symbolToken;
                    uint declarator = p_SymbolToken.declarator;
                    if (declarator == (uint) AnglrClassificationType.TerminalName)
                    {
                        RhsState p_RhsState = null;
                        if (!m_shiftset.TryGetValue (p_SymbolToken, out p_RhsState))
                            logger?.WarnLine ("missing shift " + p_SymbolToken.name);
                        else
                            count += p_RhsState.computeFollowSets (p_RhsProduction, new rhsenumerator (rhsit), p_RhsConfiguration.getFollowSet);
                    }
                    else    // declarator = PERCENT_NTERM
                    {
                        RhsState p_RhsState = null;
                        if (!m_gotoset.TryGetValue (p_SymbolToken, out p_RhsState))
                            logger?.WarnLine ("missing goto " + p_SymbolToken.name);
                        else
                            count += p_RhsState.computeFollowSets (p_RhsProduction, new rhsenumerator (rhsit), p_RhsConfiguration.getFollowSet);
                        RhsClosureElt p_RhsClosureElt = null;
                        if (!m_closure.TryGetValue (p_SymbolToken, out p_RhsClosureElt))
                            continue;
                        count += p_RhsClosureElt.unionFollowSet (p_rhsNode.transitionSet) ? 1 : 0;
                        if (p_rhsNode.opened)
                            count += p_RhsClosureElt.unionFollowSet (p_RhsConfiguration.getFollowSet) ? 1 : 0;
                    }
                }

                foreach (KeyValuePair<SymbolToken, RhsClosureElt> keyval in m_closure)
                {
                    RhsClosureElt p_prodtokpair = keyval.Value;
                    RhsProductionNode p_RhsProductionNode = p_prodtokpair.rhsProductionNode;
                    foreach (RhsProduction p_RhsProduction in p_RhsProductionNode.Productions)
                    {
                        rhsenumerator rhsit = new rhsenumerator (p_RhsProduction.rhsNodes);
                        if (rhsit.atEnd)
                        {
                            RhsConfiguration p_RhsConfiguration = m_reductions [p_RhsProduction.productionNumber];
                            if (p_RhsConfiguration != null)
                                p_RhsConfiguration.unionFollowSet (p_prodtokpair.getFollowSet);
                            continue;
                        }
                        RhsNode p_rhsNode = rhsit.currentRhsNode;
                        SymbolToken p_SymbolToken = p_rhsNode.symbolToken;
                        uint declarator = p_SymbolToken.declarator;
                        if (declarator == (uint) AnglrClassificationType.TerminalName)
                        {
                            RhsState p_RhsState = null;
                            if (!m_shiftset.TryGetValue (p_SymbolToken, out p_RhsState))
                                logger?.WarnLine ("missing shift " + p_SymbolToken.name);
                            else
                                count += p_RhsState.computeFollowSets (p_RhsProduction, new rhsenumerator (rhsit), p_prodtokpair.getFollowSet);
                        }
                        else    // declarator = PERCENT_NTERM
                        {
                            RhsState p_RhsState = null;
                            if (!m_gotoset.TryGetValue (p_SymbolToken, out p_RhsState))
                                logger?.WarnLine ("missing goto " + p_SymbolToken.name);
                            count += p_RhsState.computeFollowSets (p_RhsProduction, new rhsenumerator (rhsit), p_prodtokpair.getFollowSet);
                            RhsClosureElt p_RhsClosureElt = null;
                            if (!m_closure.TryGetValue (p_SymbolToken, out p_RhsClosureElt))
                                continue;
                            count += p_RhsClosureElt.unionFollowSet (p_rhsNode.transitionSet) ? 1 : 0;
                            if (p_rhsNode.opened)
                                count += p_RhsClosureElt.unionFollowSet (p_prodtokpair.getFollowSet) ? 1 : 0;
                        }
                    }
                }
                total += count;
            }
            while (count > 0);

            return total;
        }

        internal int computeFollowSets (RhsProduction p_rhsProduction, rhsenumerator rhsit, TokenSet p_tokset)
        {
            RhsConfiguration cfg = new RhsConfiguration (p_rhsProduction, rhsit, false);
            RhsConfiguration p_RhsConfiguration = null;
            if (!m_core.TryGetValue (cfg, out p_RhsConfiguration))
                return 0;
            return p_RhsConfiguration.unionFollowSet (p_tokset) ? 1 : 0;
        }

        internal statedelta findShiftSet (shiftset p_shiftset)
        {
            statedelta sd = new statedelta (m_shiftset, null);
            statedelta sdRef = null;
            if (p_shiftset.TryGetValue (sd, out sdRef))
                return sdRef;
            return sd;
        }

        internal statedelta findGotoSet (gotoset p_gotoset)
        {
            statedelta sd = new statedelta (m_gotoset, null);
            statedelta sdRef = null;
            if (p_gotoset.TryGetValue (sd, out sdRef))
                return sdRef;
            return sd;
        }

        internal void addToShiftSet (shiftset p_shiftset)
        {
            statedelta sd = new statedelta (m_shiftset, new stateSetInfo ());
            if (p_shiftset.Keys.Contains (sd))
                return;
            p_shiftset [sd] = sd;
        }

        internal void addToGotoSet (gotoset p_gotoset)
        {
            statedelta sd = new statedelta (m_gotoset, new stateSetInfo ());
            if (p_gotoset.Keys.Contains (sd))
                return;
            p_gotoset [sd] = sd;
        }

        internal void markGotoCounters ()
        {
            foreach (KeyValuePair<SymbolToken, RhsState> keyval in m_gotoset)
            {
                RhsProductionNode p_RhsProductionNode = null;
                if (!m_prodset.TryGetValue (keyval.Key, out p_RhsProductionNode) || (p_RhsProductionNode == null))
                    continue;
                p_RhsProductionNode.markGotoCounter (keyval.Value.stateNumber);
            }
        }

        internal void add (RhsConfiguration cfg)
        {
            if (!m_core.Keys.Contains (cfg))
                m_core [cfg] = cfg;
            else
                m_core [cfg].unionFollowSet (cfg.getFollowSet);
        }

        internal void DisplayCore (StringBuilder sb)
        {
            foreach (RhsConfiguration cfg in m_core.Values)
            {
                RhsProduction p_RhsProduction = cfg.rhsProduction;
                rhsenumerator rhsit = cfg.rhsIterator;
                RhsNode p_RhsNode = rhsit.atEnd ? null : rhsit.currentRhsNode;
                sb.Append ("\t" + p_RhsProduction.productionNumber + "\t" + p_RhsProduction.symbolToken.name + " :");
                foreach (RhsNode p_node in p_RhsProduction.rhsNodes)
                {
                    if (p_node == p_RhsNode)
                        sb.Append (" .");
                    SymbolToken p_SymbolToken = p_node.symbolToken;
                    if (p_SymbolToken.alias != null)
                        p_SymbolToken = p_SymbolToken.alias;
                    sb.Append (" " + p_SymbolToken.name);
                }
                if (null == p_RhsNode)
                    sb.Append (" .");
                sb.Append ("        FS = {");
                cfg.getFollowSet.display (sb);
                sb.Append ("}");
                sb.AppendLine ();
            }
        }

        internal void DisplayClosure (StringBuilder sb)
        {
            foreach (KeyValuePair<SymbolToken, RhsClosureElt> keyval in m_closure)
            {
                RhsClosureElt pt = keyval.Value;
                RhsProductionNode p_RhsProductionNode = pt.rhsProductionNode;
                int count = 0;
                string field = "";
                foreach (RhsProduction p_RhsProduction in p_RhsProductionNode.Productions)
                {
                    RhsNode p_RhsNode = (p_RhsProduction.rhsNodes.Count > 0) ? p_RhsProduction.rhsNodes [0] : null;
                    if (count == 0)
                    {
                        string name = p_RhsProduction.symbolToken.name;
                        field = new string (' ', name.Length);
                        sb.Append ("\t" + p_RhsProduction.productionNumber + "\t" + name + " :");
                    }
                    else
                        sb.Append ("\t" + p_RhsProduction.productionNumber + "\t" + field + " |");
                    ++count;
                    foreach (RhsNode p_node in p_RhsProduction.rhsNodes)
                    {
                        if (p_node == p_RhsNode)
                            sb.Append (" .");
                        SymbolToken p_SymbolToken = p_node.symbolToken;
                        if (p_SymbolToken.alias != null)
                            p_SymbolToken = p_SymbolToken.alias;
                        sb.Append (" " + p_SymbolToken.name);
                    }
                    if (null == p_RhsNode)
                        sb.Append (" .");
                    if (count == 1)
                    {
                        sb.Append ("        FS = {");
                        pt.getFollowSet.display (sb);
                        sb.Append ("}") /*<< ", address = " << (long*) pt->getFollowSet()*/;
                    }
                    sb.AppendLine ();
                }
            }
        }

        internal void DisplayShiftTransitions (IAnglrLogger logger)
        {
            foreach (RhsState p_RhsState in ShiftArray)
            {
                SymbolToken p_SymbolToken = p_RhsState.symbolToken;
                if (p_SymbolToken.alias != null)
                    logger?.DebugRawLine ("\t" + p_SymbolToken.alias.name + " shift " + p_RhsState.stateNumber);
                else
                    logger?.DebugRawLine ("\t" + p_SymbolToken.name + " shift " + p_RhsState.stateNumber);
            }
        }

        internal void DisplayReductions (IAnglrLogger logger)
        {
            if (m_reductions.Count > 1)
                logger?.WarnLine ("multiple reductions");
            foreach (KeyValuePair<int, RhsConfiguration> keyval in m_reductions)
            {
                RhsConfiguration p_rhsConfiguration = keyval.Value;
                RhsProduction p_rhsProduction = p_rhsConfiguration.rhsProduction;
                SymbolToken p_symbolToken = p_rhsProduction.symbolToken;
                TokenSet p_tokset = p_rhsConfiguration.getFollowSet;

                foreach (KeyValuePair<SymbolToken, RhsState> shift in m_shiftset)
                {
                    RhsState p_rhsState = shift.Value;
                    SymbolToken p_symbolTokenRef = shift.Key;
                    if (p_tokset.check (p_symbolTokenRef))
                        if (p_symbolTokenRef.alias != null)
                            logger?.WarnLine ("\tshift/reduce conflict: "
                            + p_symbolTokenRef.alias.name + " shift " + p_rhsState.stateNumber
                            + ", reduce " + p_symbolToken.name + " (" + keyval.Key + ")");
                        else
                            logger?.WarnLine ("\tshift/reduce conflict: "
                                + p_symbolTokenRef.name + " shift " + p_rhsState.stateNumber
                                + ", reduce " + p_symbolToken.name + " (" + keyval.Key + ")");
                }

                foreach (KeyValuePair<int, RhsConfiguration> redkeyval in m_reductions)
                {
                    RhsConfiguration p_rhsConfigurationRef = redkeyval.Value;
                    RhsProduction p_rhsProductionRef = p_rhsConfigurationRef.rhsProduction;
                    SymbolToken p_symbolTokenRef = p_rhsProductionRef.symbolToken;
                    TokenSet p_toksetRef = p_rhsConfigurationRef.getFollowSet;

                    if (p_rhsProductionRef.productionNumber == p_rhsProduction.productionNumber)
                        continue;
                    tokset tset = p_toksetRef.set;
                    foreach (SymbolToken symbol in tset.Values)
                    {
                        if (p_tokset.check (symbol))
                            if (symbol.alias != null)
                                logger?.WarnLine ("\treduce/reduce conflict: '"
                                + symbol.alias.name
                                + "', reduce " + p_symbolToken.name + " (" + keyval.Key + ")"
                                + ", reduce " + p_symbolTokenRef.name + " (" + redkeyval.Key + ")");
                            else
                                logger?.WarnLine ("\treduce/reduce conflict: "
                                    + symbol.name
                                    + ", reduce " + p_symbolToken.name + " (" + keyval.Key + ")"
                                    + ", reduce " + p_symbolTokenRef.name + " (" + redkeyval.Key + ")");
                    }
                }
                logger?.DebugRawLine ("\treduce " + p_symbolToken.name + " (" + keyval.Key + ")");
            }
        }

        internal void DisplayGotoTransitions (IAnglrLogger logger)
        {
            foreach (RhsState p_RhsState in GotoArray)
            {
                logger?.DebugRawLine ("\t" + p_RhsState.symbolToken.name + " goto " + p_RhsState.stateNumber);
            }
        }

        internal SymbolToken symbolToken { get { return m_SymbolToken; } }
        internal int stateNumber { get { return m_stateNumber; } set { if (m_stateNumber < 0) m_stateNumber = value; } }
        internal int reductionsDelta { get { return m_reductionsDelta; } set { m_reductionsDelta = value; } }
        internal int GLRDelta { get { return m_GLRDelta; } set { m_GLRDelta = value; } }
        internal rhscfgset core { get { return m_core; } }
        internal closurelist closure { get { return m_closure; } }
        //inline statearray::iterator getShiftIterator () { return m_shiftarray.begin (); }
        //inline statearray::iterator incShiftIterator (statearray::iterator it) { if (it != m_shiftarray.end ()) ++it; return it; }
        //inline bool endShiftIterator (statearray::iterator it) { return it == m_shiftarray.end (); }
        internal void changeShiftState (RhsState p_state, int index) { m_shiftset [p_state.symbolToken] = ShiftArray [index] = p_state; }
        //inline statearray::iterator getGotoIterator () { return m_gotoarray.begin (); }
        //inline statearray::iterator incGotoIterator (statearray::iterator it) { if (it != m_gotoarray.end ()) ++it; return it; }
        //inline bool endGotoIterator (statearray::iterator it) { return it == m_gotoarray.end (); }
        internal void changeGototState (RhsState p_state, int index) { m_gotoset [p_state.symbolToken] = GotoArray [index] = p_state; }

        internal RhsState checkShift (SymbolToken p_SymbolToken)
        {
            RhsState p_RhsState = null;
            return (m_shiftset.TryGetValue (p_SymbolToken, out p_RhsState)) ? p_RhsState : null;
        }

        internal RhsState checkGoto (SymbolToken p_SymbolToken)
        {
            RhsState p_RhsState = null;
            return (m_gotoset.TryGetValue (p_SymbolToken, out p_RhsState)) ? p_RhsState : null;
        }

        internal int coreSize { get { return m_core.Count; } }
        internal int closureSize { get { return m_closure.Count; } }
        internal int shiftSetSize { get { return m_shiftset.Count; } }
        internal int reductionsSetSize { get { return m_reductions.Count; } }
        internal int gotoSetSize { get { return m_gotoset.Count; } }

        internal int defaultReduction { get { return m_defaultReduction; } }
        internal int markDefaultReduction ()
        {
            int prodNr = 0;
            int maxFollowSize = -1;
            foreach (KeyValuePair<int, RhsConfiguration> keyval in m_reductions)
            {
                RhsConfiguration p_RhsConfiguration = keyval.Value;
                int size = p_RhsConfiguration.getFollowSet.set.Count;
                if (size > maxFollowSize)
                    maxFollowSize = size;
                prodNr = m_defaultReduction = keyval.Key;
            }
            return prodNr;
        }

        internal int checkConflicts (IAnglrLogger logger)
        {
            if (!hasGLRCondition)
                return -1;
            foreach (KeyValuePair<SymbolToken, GLRToken> git in m_glrset)
            {
                logger?.DebugRawLine<KeyValuePair<SymbolToken, GLRToken>>
                (
                    (vkp) => 
                    { 
                        StringBuilder sb = new StringBuilder ();
                        SymbolToken p_symbolToken = vkp.Key;
                        if (p_symbolToken.alias != null)
                            sb.Append ("\t'" + p_symbolToken.alias.name + "'");
                        else
                            sb.Append ("\t" + p_symbolToken.name);
                        sb.Append (" :");
                        GLRToken p_GLRToken = vkp.Value;
                        if (p_GLRToken.state >= 0)
                            sb.Append (" s-" + p_GLRToken.state);
                        rdlist p_rdlist = p_GLRToken.getRdlist;
                        foreach (RhsProduction rdit in p_rdlist)
                            sb.Append (" r-" + rdit.productionNumber);
                        sb.AppendLine ();
                        return sb.ToString (); 
                    }, 
                    git
                );
            }
            return 0;
        }

        internal int checkGLRCondition ()
        {
            int size = 0;

            if (!(((m_shiftset.Count > 0) && (m_reductions.Count > 0)) || (m_reductions.Count > 1)))
                return 0;

            if (m_reductions.Count == 1)
            {
                foreach (KeyValuePair<int, RhsConfiguration> prodit in m_reductions)
                {
                    RhsConfiguration rhsConfiguration1 = prodit.Value;
                    if (rhsConfiguration1.rhsProduction.priority >= 0)
                    {
                        TokenSet p_TokenSet = rhsConfiguration1.getFollowSet;
                        tokset p_set = p_TokenSet.set;
                        foreach (RhsConfiguration rhsConfiguration in m_core.Keys)
                        {
                            rhsenumerator rhsenumerator = rhsConfiguration.rhsIterator;
                            if (rhsenumerator.atEnd)
                                continue;
                            RhsNode rhsNode = rhsenumerator.currentRhsNode;
                            SymbolToken symbolToken = rhsNode.symbolToken;
                            if (symbolToken.declarator != (uint) AnglrClassificationType.TerminalName)
                                continue;
                            RhsProduction rhsProduction = rhsConfiguration.rhsProduction;
                            if (!p_set.TryGetValue (symbolToken, out _))
                                continue;
                            if (rhsProduction.associativity != ProductionAssociativity.Right)
                            {
                                if (rhsProduction.priority < rhsConfiguration1.rhsProduction.priority)
                                    p_set.Remove (symbolToken);
                                else
                                if (m_shiftset.TryGetValue (symbolToken, out _))
                                {
                                    ShiftArray.Remove (m_shiftset [symbolToken]);
                                    m_shiftset.Remove (symbolToken);
                                }
                            }
                            else
                            {
                                if (rhsProduction.priority >= rhsConfiguration1.rhsProduction.priority)
                                    p_set.Remove (symbolToken);
                                else
                                if (m_shiftset.TryGetValue (symbolToken, out _))
                                {
                                    ShiftArray.Remove (m_shiftset [symbolToken]);
                                    m_shiftset.Remove (symbolToken);
                                }
                            }
                        }
                    }
                }
            }

            foreach (KeyValuePair<SymbolToken, RhsState> sit in m_shiftset)
                m_glrset [sit.Key] = new GLRToken (sit.Value.stateNumber);
            foreach (KeyValuePair<int, RhsConfiguration> prodit in m_reductions)
            {
                TokenSet p_TokenSet = prodit.Value.getFollowSet;
                tokset p_set = p_TokenSet.set;
                foreach (SymbolToken p_SymbolToken in p_set.Values)
                {
                    GLRToken p_GLRToken = null;
                    if (!m_glrset.TryGetValue (p_SymbolToken, out p_GLRToken))
                        m_glrset [p_SymbolToken] = p_GLRToken = new GLRToken (-1);
                    p_GLRToken.add (prodit.Value.rhsProduction);
                }
            }

            bool glrCondition = false;
            foreach (KeyValuePair<SymbolToken, GLRToken> glrit in m_glrset)
            {
                GLRToken p_GLRToken = glrit.Value;
                rdlist p_rdlist = p_GLRToken.getRdlist;
                if ((p_rdlist.Count > 1) || ((p_rdlist.Count > 0) && (p_GLRToken.state >= 0)))
                    glrCondition = true;
                size += 2 + p_rdlist.Count;
            }

            if (!glrCondition)
            {
                foreach (KeyValuePair<SymbolToken, GLRToken> glrit in m_glrset)
                    glrit.Value.Dispose ();
                m_glrset.Clear ();
                size = 0;
            }
            else
                /*cout << "state " << stateNumber() << " meets GLR condition" << endl*/
                ;
            return size;
        }

        public int ComputeConflicts ()
        {
            int count = 0;

            foreach (RhsState rhsState in ShiftArray)
            {
                if ((ConflictFlags & HasSubConflicts) != 0)
                    break;
                if (rhsState.ConflictFlags != 0)
                {
                    ConflictFlags |= HasSubConflicts;
                    ++count;
                }
            }

            foreach (RhsState rhsState in GotoArray)
            {
                if ((ConflictFlags & HasSubConflicts) != 0)
                    break;
                if (rhsState.ConflictFlags != 0)
                {
                    ConflictFlags |= HasSubConflicts;
                    ++count;
                }
            }

            foreach (RhsState rhsState in ShiftArray)
            {
                if ((ConflictFlags & HasConflicts) != 0)
                    break;
                foreach (RhsConfiguration rhsConfiguration in m_reductions.Values)
                {
                    if ((ConflictFlags & HasConflicts) != 0)
                        break;
                    if (rhsConfiguration.getFollowSet.m_tokset.TryGetValue (rhsState.symbolToken, out _))
                    {
                        ConflictFlags |= HasConflicts;
                        ++count;
                    }
                }
            }

            int horIndex = 0;
            foreach (RhsConfiguration horRhsConfiguration in m_reductions.Values)
            {
                if ((ConflictFlags & HasConflicts) != 0)
                    break;
                int vrtIndex = 0;
                foreach (RhsConfiguration vrtRhsConfiguration in m_reductions.Values)
                {
                    if ((ConflictFlags & HasConflicts) != 0)
                        break;
                    if (vrtIndex++ <= horIndex)
                        continue;
                    foreach (SymbolToken symbolToken in vrtRhsConfiguration.getFollowSet.m_tokset.Values)
                    {
                        if ((ConflictFlags & HasConflicts) != 0)
                            break;
                        if (horRhsConfiguration.getFollowSet.m_tokset.TryGetValue (symbolToken, out _))
                        {
                            ConflictFlags |= HasConflicts;
                            ++count;
                        }
                    }
                }
                ++horIndex;
            }
            return count;
        }

        public void AddRefState (RhsState rhsState) => RefByStates.Add (rhsState);

        public static readonly uint HasConflicts = 1 << 0;
        public static readonly uint HasSubConflicts = 1 << 1;

        public uint ConflictFlags { get; private set; } = 0;

        public RhsState RhsStateTreeLink { get { return _RhsStateTreeLink; } set { if (_RhsStateTreeLink != null) return; _RhsStateTreeLink = value; } }
        private RhsState _RhsStateTreeLink = null;
        internal bool hasGLRCondition { get { return m_glrset.Count > 0; } }
        internal glrset getGLRSet { get { return m_glrset; } }

        internal int stateDummy { get { return m_stateDummy; } set { m_stateDummy = value; } }

        internal prodset reductions { get { return m_reductions; } }

        internal int stateWalker (Queue<RhsState> p_queue, int stateDummy) { return 0; }

        internal static int m_created = 0;
        internal static int m_deleted = 0;

        internal int m_id { get; private set; }
        public SymbolToken m_SymbolToken { get; private set; }
        private prodlist m_prodset;

        public int m_stateNumber { get; private set; } = -1;
        private int m_reductionsDelta = -1;
        private int m_GLRDelta = -1;
        public rhscfgset m_core { get; private set; } = new rhscfgset ();
        public closurelist m_closure = new closurelist ();
        private stateset m_shiftset = new stateset ();
        private stateset m_gotoset = new stateset ();
        public prodset m_reductions = new prodset ();
        private int m_defaultReduction = 0;
        public statearray ShiftArray { get; private set; } = new statearray ();
        public statearray GotoArray { get; private set; } = new statearray ();
        public statearray RefByStates { get; private set; } = new statearray ();

        private glrset m_glrset = new glrset ();

        private int m_stateDummy = 0;
    }
}
