using Anglr.Compiler;
using Anglr.Parser;
using Anglr.Parser.Core;
using Anglr.Parser.SyntaxTree;
using Anglr.Parser.Walker;
using AnglrLogLibrary;
using AnglrParserLibrary;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.Metrics;
using System.Diagnostics.SymbolStore;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static AnglrLibrary.CSharpBaseGenerator;

/// <summary>
/// 
/// </summary>
namespace AnglrLibrary
{
    [Serializable]
    public enum AppInfoType
    {
        Invalid,
        SymbolToken,
        Children,
        HtmlText,
        IndexValue,
        NSCInfo
    }

    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class AppInfo : SortedDictionary<AppInfoType, object>
    {
    }

    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public partial class CSharpGenerator : CSharpBaseGenerator
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="p__anglr_file_fragment_"></param>
        /// <param name="className"></param>
        public CSharpGenerator (_anglr_file_fragment_ p__anglr_file_fragment_, string outputDir, anglrCompiler compiler) : base (p__anglr_file_fragment_, outputDir, compiler)
        {
            Logger = compiler?.AnglrLogger ?? new VoidAnglrLogger ();
            m_anglrFile = p__anglr_file_fragment_.m__anglr_file_;
            OutputDir = outputDir;

            try
            {
                if ((OutputDir != null) && (OutputDir != ""))
                {
                    directoryInfo = Directory.CreateDirectory (OutputDir);
                }
            }
            catch (Exception)
            {

            }

            Common_Event += Invoke_Common_Callback;
            _anglr_file__Event += Invoke__anglr_file__Callback;
            _terminal_definition__Event += Invoke__terminal_definition__Callback;
            _regex_definition__Event += Invoke__regex_definition__Callback;
            _anglr_syntax_rule__Event += Invoke__anglr_syntax_rule__Callback;
            _anglr_syntax_production__Event += Invoke__anglr_syntax_production__Callback;
            _priority_specification__Event += Invoke__priority_specification__Callback;
            _associativity_specification__Event += Invoke__associativity_specification__Callback;
            _production_name__Event += Invoke__production_name__Callback;
            _name__Event += Invoke__name__Callback;
            _g_name__Event += Invoke__g_name__Callback;
            _marker__Event += Invoke__marker__Callback;
            _regular_expression_usage__Event += Invoke__regular_expression_usage__Callback;
            _general_part__Event += Invoke__general_part__Callback;
            _declaration_part__Event += Invoke__declaration_part__Callback;
            _scanner_part__Event += Invoke__scanner_part__Callback;
            _lexer_part__Event += Invoke__lexer_part__Callback;
            _parser_part__Event += Invoke__parser_part__Callback;

            generateParserEvent += CSharpGenerator_generateParserEvent;
            generateLexerEvent += CSharpGenerator_generateLexerEvent;
            generateScannerEvent += CSharpGenerator_generateScannerEvent;

            TraverseCommon (m_anglrFile);
        }

        private void InitGenerator ()
        {
            m_stateset = new rhsstateset ();
            m_statearray = new statearray ();

            m_shiftset = new shiftset ();
            m_gotoset = new gotoset ();

            m_maxCheck = -1;
            m_maxRCheck = -1;
            m_maxGLRCheck = -1;

            m_terminalCodes = null;
            m_nonTerminalCodes = null;
            m_check = null;
            m_state = null;
            m_shiftDelta = null;
            m_gotoDelta = null;
            m_productionLengths = null;
            m_productionRules = null;
            m_defaultGoto = null;
            m_reductions = null;
            m_rcheck = null;
            m_rstate = null;
            m_glrcheck = null;
            m_glrstate = null;
            m_glrcells = null;
        }

        private void CSharpGenerator_generateScannerEvent (_scanner_part_ part)
        {
            ScannerPart scannerPart = scannerParts [part.m__identifier_.text];
            string codeDir = scannerPart.codeDir;
            if (!Directory.Exists (codeDir))
                Directory.CreateDirectory (codeDir);
            string currentDirectory = Directory.GetCurrentDirectory ();

            try
            {
                Directory.SetCurrentDirectory (codeDir);
                GenerateScanner (attributeCollection.scannerParts [part.m__identifier_.text]);
            }
            finally
            {
                Directory.SetCurrentDirectory (currentDirectory);
            }
        }

        private void CSharpGenerator_generateLexerEvent (_lexer_part_ part)
        {
            LexerPart lexerPart = lexerParts [part.m__identifier_.text];
            string codeDir = lexerPart.codeDir;
            if (!Directory.Exists (codeDir))
                Directory.CreateDirectory (codeDir);
            string currentDirectory = Directory.GetCurrentDirectory ();

            try
            {
                Directory.SetCurrentDirectory (codeDir);
                GenerateLexer (attributeCollection.lexerParts [part.m__identifier_.text]);
            }
            finally
            {
                Directory.SetCurrentDirectory (currentDirectory);
            }
        }

        private void CSharpGenerator_generateParserEvent (_parser_part_ part)
        {
            ParserPart parserPart = parserParts [part.m__identifier_.text];
            string codeDir = parserPart.codeDir;
            if (!Directory.Exists (codeDir))
                Directory.CreateDirectory (codeDir);
            string currentDirectory = Directory.GetCurrentDirectory ();

            try
            {
                Directory.SetCurrentDirectory (codeDir);
                InitGenerator ();
                Logger?.InfoRawLine ("Display symbol table");
                //m_symtab.print ();
                if (anglrCompiler.createPrecedenceGrammar)
                {
                    Logger?.InfoRawLine ("Eliminate tautologies");
                    EliminateTautologies ();
                    Logger?.InfoRawLine ("Check for iterator syntax rules");
                    CheckIterators ();
                    Logger?.InfoRawLine ("Check for cascaded syntax rules");
                    CheckCascades ();
                }
                //Logger?.InfoRawLine ("Resolve identities");
                //ResolveIdentities ();
                Logger?.InfoRawLine ("Display productions");
                DisplayProductions ();
                {
                    CheckProductionUsage (m_startProductionNode);
                    EliminateUnusedProductions ();
                    SymbolToken [] symTab = new SymbolToken [1024];
                    Logger?.InfoRawLine ("Display hierarchical view");
                    DisplayProductionsHierarchy (m_startProductionNode, symTab, 0, 0);
                    StringBuilder sb = new StringBuilder ();
                    DisplayProduction (sb, m_startProductionNode, 0, 1);
                    Logger?.InfoRawLine (sb.ToString ());
                }
                Logger?.InfoRawLine ("Check productions");
                int result = CheckProductions ();
                if (result != 0)
                    Logger?.ErrorLine ("production rules failure");
                else
                {
                    Logger?.InfoRawLine ("Make cannonical RHS sets");
                    MakeCanonicalRhsSet ();
                    Logger?.InfoRawLine ("Compute epsilon conditions");
                    ComputeEpsilonConditions ();
                    Logger?.InfoRawLine ("Compute start sets");
                    ComputeStartSets ();
                    Logger?.InfoRawLine ("Compute transition sets");
                    ComputeTransitionSets ();
                    Logger?.InfoRawLine ("Compute follow sets");
                    ComputeFollowSets ();
                    Logger?.InfoRawLine ("Prepare table generator");
                    PrepareTableGenerator ();
                    GenerateParserSrc (part);
                    GenerateParserHdr ();
                    GenerateTokenHeader ();

                    //int count = m_statearray[0].stateWalker (m_statequeue, 100);
                    //Logger?.InfoRawLine ("count = " << count);
                    //if (false)
                    Logger?.InfoRawLine ("Output state descriptions");
                    Logger?.InfoRawLine ("INFO: STATES");
                    Logger?.InfoRawLine ("");
                    foreach (RhsState p_state in m_statearray)
                    {
                        Logger?.InfoRawLine ("State " + p_state.stateNumber);
                        if (p_state.coreSize > 0)
                        {
                            StringBuilder sb = new StringBuilder ();
                            p_state.DisplayCore (sb);
                            Logger?.InfoRawLine (sb.ToString ());
                            Logger?.InfoRawLine ("");
                        }
                        if (p_state.closureSize > 0)
                        {
                            StringBuilder sb = new StringBuilder ();
                            p_state.DisplayClosure (sb);
                            Logger?.InfoRawLine (sb.ToString ());
                            Logger?.InfoRawLine ("");
                        }
                        if (p_state.shiftSetSize > 0)
                        {
                            p_state.DisplayShiftTransitions (Logger);
                            Logger?.InfoRawLine ("");
                        }
                        if (p_state.reductionsSetSize > 0)
                        {
                            //p_state.DisplayReductions ();
                            p_state.checkConflicts (Logger);
                            Logger?.InfoRawLine ("");
                        }
                        if (p_state.gotoSetSize > 0)
                        {
                            p_state.DisplayGotoTransitions (Logger);
                            Logger?.InfoRawLine ("");
                        }
                    }
                }
            }
            finally
            {
                RhsProduction.Reset ();
                Directory.SetCurrentDirectory (currentDirectory);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void Dispose ()
        {
            foreach (RhsState p_RhsState in m_statearray)
                p_RhsState.Dispose ();
            m_statearray.Clear ();
            m_stateset.Clear ();
            foreach (RhsProductionNode p_RhsProductionNode in m_prodlist.Values)
                p_RhsProductionNode.Dispose ();
        }

        /// <summary>
        /// 
        /// </summary>
        public void GenerateCsCode (_anglr_syntax_rule_ syntax_Rule)
        {
            startSyntaxRule = syntax_Rule;
            Logger?.InfoRawLine ("Collect symbols");
            Traverse (m_anglrFile);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private int CheckProductions ()
        {
            int result = 0;

            foreach (RhsProductionNode p_productionNode in m_prodlist.Values)
            {
                if (p_productionNode == null)
                    continue;
                SymbolToken p_productionName = p_productionNode.ProductionName;
                if (p_productionName.declarator != (uint) AnglrClassificationType.NonTerminalName)
                {
                    Logger?.WarnLine ("production name '" + p_productionName.name + "' must be non-terminal");
                    ++result;
                }
                int index = 1;
                foreach (RhsProduction p_production in p_productionNode.Productions)
                {
                    (m_proddict [p_production.productionNumber] = p_production).index = index++;
                    foreach (RhsNode p_node in p_production.rhsNodes)
                    {
                        SymbolToken p_nodeSymbol = p_node.symbolToken;
                        uint declarator = p_nodeSymbol.declarator;
                        if (!((declarator == (uint) AnglrClassificationType.TerminalName) || (declarator == (uint) AnglrClassificationType.NonTerminalName)))
                        {
                            Logger?.WarnLine ("rhs symbol name '" + p_nodeSymbol.name + "' must be terminal (token) or non-terminal");
                            ++result;
                            continue;
                        }
                        if (declarator == (uint) AnglrClassificationType.NonTerminalName)
                        {
                            if (!m_prodlist.Keys.Contains (p_nodeSymbol))
                            {
                                Logger?.WarnLine ("symbol '" + p_nodeSymbol.name + "' is used in production rules, but is not defined as terminal or non-terminal");
                                ++result;
                            }
                        }
                    }
                }
            }
            return result;
        }

        private void CheckProductionUsage (RhsProductionNode prodNode)
        {
            SymbolToken symbolToken = prodNode.ProductionName;
            if (symbolToken.SymbolUsageFlag)
                return;
            prodNode.ProductionName.SymbolUsageFlag = true;
            foreach (var production in prodNode.Productions)
            {
                foreach (var node in production.rhsNodes)
                {
                    SymbolToken nodeSymbol = node.symbolToken;
                    if (nodeSymbol.declarator != (uint) AnglrClassificationType.NonTerminalName)
                    {
                        nodeSymbol.SymbolUsageFlag = true;
                        continue;
                    }
                    if (!m_prodlist.TryGetValue (nodeSymbol, out var result))
                        continue;
                    CheckProductionUsage (result);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void DisplayProductions ()
        {
            if (!CSharpGenerator.DisplayProductionsFlag)
                return;
            Logger?.InfoRawLine ("INFO: CANONICAL PRODUCTIONS");
            Logger?.InfoRawLine ("");
            foreach (RhsProductionNode p_productionNode in m_prodlist.Values)
            {
                Logger?.InfoRawLine<RhsProductionNode>
                (
                    (productionNode) =>
                    {
                        StringBuilder sb = new StringBuilder ();
                        SymbolToken p_productionName = productionNode.ProductionName;
                        sb.AppendLine (p_productionName.name);
                        int i = -1;
                        foreach (RhsProduction p_production in productionNode.Productions)
                        {
                            ++i;
                            sb.Append ("\t");
                            if (i != 0)
                                sb.Append ("|\t");
                            else
                                sb.Append (":\t");
                            if (p_production.rhsNodes.Count > 0)
                                foreach (RhsNode p_node in p_production.rhsNodes)
                                {
                                    SymbolToken p_nodeSymbol = p_node.symbolToken;
                                    if (p_nodeSymbol.alias != null)
                                        p_nodeSymbol = p_nodeSymbol.alias;
                                    sb.Append (p_nodeSymbol.name + " ");
                                }
                            else
                                sb.Append ("%empty");
                            if (p_production.priority > 0)
                            {
                                sb.Append ($" {p_production.priority}");
                                if (p_production.associativity != ProductionAssociativity.None)
                                    sb.Append ($" {p_production.associativity}");
                            }
                            else
                            {
                                if (p_production.associativity != ProductionAssociativity.None)
                                    sb.Append ($" {p_production.associativity}");
                            }
                            sb.AppendLine ();
                        }
                        sb.AppendLine ("\t;");
                        sb.AppendLine ();
                        return sb.ToString ();
                    },
                    p_productionNode
                );
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="prodNode"></param>
        /// <param name="stack"></param>
        /// <param name="stackIndex"></param>
        /// <param name="displayDepth"></param>
        private void DisplayProductionsHierarchy (RhsProductionNode prodNode, SymbolToken [] stack, int stackIndex, int displayDepth)
        {
            if (displayDepth == 0)
            {
                Logger?.InfoRawLine ("INFO: HIERARCHICALY VIEW OF PRODUCTIONS");
                Logger?.InfoRawLine ("");
            }

            int stackTop = stackIndex;
            SymbolToken nodeSymbol = (prodNode != null) ? prodNode.ProductionName : null;
            if (nodeSymbol == null)
                return;
            if (nodeSymbol.displayed)
                return;
            nodeSymbol.displayed = true;
            stack [stackTop++] = nodeSymbol;
            Logger?.InfoRawLine<int>
            (
                (depth) =>
                {
                    StringBuilder sb = new StringBuilder ();
                    for (int i = 0; i < depth; ++i)
                        sb.Append ("\t");
                    sb.AppendLine ($"{nodeSymbol.name}");
                    return sb.ToString ();
                },
                displayDepth
            );
            Logger?.InfoRawLine<int>
            (
                (depth) =>
                {
                    StringBuilder sb = new StringBuilder ();
                    for (int i = 0; i < depth; ++i)
                        sb.Append ("\t");
                    sb.AppendLine ("{");
                    return sb.ToString ();
                },
                displayDepth
            );
            if (prodNode != null)
                foreach (RhsProduction p_RhsProduction in prodNode.Productions)
                {
                    foreach (RhsNode p_RhsNode in p_RhsProduction.rhsNodes)
                    {
                        SymbolToken p_SymbolToken = p_RhsNode.symbolToken;
                        int index;
                        for (index = stackIndex; (index < stackTop) && (p_SymbolToken.name != stack [index].name); ++index)
                            ;
                        if (index < stackTop)
                            continue;
                        stack [stackTop++] = p_SymbolToken;
                        bool isToken = false;
                        if ((isToken = (p_SymbolToken.declarator == (uint) AnglrClassificationType.TerminalName)) || (p_SymbolToken.displayed == true))
                        {
                            Logger?.InfoRawLine<int>
                            (
                                (depth) =>
                                {
                                    StringBuilder sb = new StringBuilder ();
                                    for (int i = 0; i < depth; ++i)
                                        sb.Append ("\t");
                                    string sign = isToken ? "T" : "N";
                                    sb.AppendLine ($"{p_SymbolToken.name} {sign}");
                                    return sb.ToString ();
                                },
                                displayDepth
                            );
                            continue;
                        }
                        if (m_prodlist.TryGetValue (p_SymbolToken, out _))
                            if (p_SymbolToken.declarator == (uint) AnglrClassificationType.NonTerminalName)
                                DisplayProductionsHierarchy (m_prodlist [p_SymbolToken], stack, stackTop, displayDepth + 1);
                    }
                    Logger?.InfoRawLine ("");
                }
            Logger?.InfoRawLine<int>
            (
                (depth) =>
                {
                    StringBuilder sb = new StringBuilder ();
                    for (int i = 0; i < depth; ++i)
                        sb.Append ("\t");
                    sb.AppendLine ("}");
                    return sb.ToString ();
                },
                displayDepth
            );
        }

        private void DisplayProduction (StringBuilder sb, RhsProductionNode node, int depth, int round)
        {
            string name = node.ProductionName.name;

            if (node.round != round)
            {
                bool first = true;
                node.round = round;
                sb.AppendLine ();
                for (int i = 0; i < depth; ++i)
                    sb.Append ("\t");
                sb.AppendLine ($"( : {name} :");
                foreach (RhsProduction p_RhsProduction in node.Productions)
                {
                    for (int i = 0; i < depth; ++i)
                        sb.Append ("\t");
                    if (first)
                        sb.Append ($"\t");
                    else
                        sb.Append ($"|\t");
                    first = false;
                    if (p_RhsProduction.rhsNodes.Count > 0)
                        foreach (RhsNode p_RhsNode in p_RhsProduction.rhsNodes)
                        {
                            SymbolToken symbol = p_RhsNode.symbolToken;
                            if ((symbol.declarator == (uint) AnglrClassificationType.TerminalName) || !m_prodlist.TryGetValue (symbol, out _))
                                sb.Append ($"{symbol.name} ");
                            else
                            {
                                DisplayProduction (sb, m_prodlist [symbol], depth + 1, round);
                            }
                        }
                    else
                        sb.Append ("%empty");
                    sb.AppendLine ();
                }
                for (int i = 0; i < depth; ++i)
                    sb.Append ("\t");
                sb.AppendLine ($")");
            }
            else
            {
                sb.Append ($"{name} ");
            }
        }

        private void EliminateTautologies ()
        {
            foreach (RhsProductionNode rhsProductionNode in m_prodlist.Values)
                rhsProductionNode.EliminateTautologies ();
        }

        private void CheckIterators ()
        {
            foreach (RhsProductionNode rhsProductionNode in m_prodlist.Values)
                rhsProductionNode.CheckIterator ();
        }

        private void CheckCascades ()
        {
            foreach (RhsProductionNode rhsProductionNode in m_prodlist.Values)
            {
                (bool identity, SymbolToken symbol) result = rhsProductionNode.CheckCascade ();
                if (result.symbol == null)
                    continue;
                if (result.symbol.declarator != (uint) AnglrClassificationType.NonTerminalName)
                    continue;
                if (result.identity)
                {
                    //Console.WriteLine ($"IDENTITY: {result.symbol.name} -- {rhsProductionNode.ProductionName.name}");
                    //rhsProductionNode.IdentityNode = m_prodlist [result.symbol];
                    //result.symbol.IdentityFlag = true;
                    //m_cascades.Remove (result.symbol);
                    //m_cascades [rhsProductionNode.ProductionName] = rhsProductionNode;
                }
                else
                {
                    Logger?.InfoRawLine ($"CASCADE : {result.symbol.name} -- {rhsProductionNode.ProductionName.name}");
                    rhsProductionNode.CascadeNode = m_prodlist [result.symbol];
                    result.symbol.CascadeFlag = true;
                    m_cascades.Remove (result.symbol);
                    m_cascades [rhsProductionNode.ProductionName] = rhsProductionNode;
                }
            }
            Logger?.InfoRawLine ("CASCADES:");
            foreach (RhsProductionNode productionNode in m_cascades.Values)
            {
                if (productionNode.ProductionName.CascadeFlag || productionNode.ProductionName.IdentityFlag)
                {
                    Logger?.InfoRawLine ($"\t({productionNode.ProductionName.name})");
                    continue;
                }
                Logger?.InfoRawLine<object>
                (
                    (_) =>
                    {
                        StringBuilder sb = new StringBuilder ();
                        sb.Append ("\t");
                        for (RhsProductionNode rhsProduction = productionNode; rhsProduction != null;)
                        {
                            sb.Append ($" {rhsProduction.ProductionName.name}");
                            if (rhsProduction.CascadeNode != null)
                                rhsProduction = rhsProduction.CascadeNode;
                            else if (rhsProduction.IdentityNode != null)
                                rhsProduction = rhsProduction.IdentityNode;
                            else
                                rhsProduction = null;
                        }
                        sb.AppendLine ();
                        return sb.ToString ();
                    },
                    null
                );
            }
            Logger?.InfoRawLine ("GENERATED CASCADE RULES:");
            foreach (KeyValuePair<SymbolToken, RhsProductionNode> pair in GenerateCascadeRules ())
            {
                Logger?.InfoRawLine ($"\t{pair.Key.name}");
                //m_prodlist [pair.Key] = pair.Value;
            }
        }

        private prodlist GenerateCascadeRules ()
        {
            prodlist productions = new prodlist ();
            //return productions;
            foreach (RhsProductionNode rhsProductionNode in m_cascades.Values)
            {
                if (rhsProductionNode.ProductionName.CascadeFlag || rhsProductionNode.ProductionName.IdentityFlag)
                    continue;
                Logger?.InfoRawLine ($"\t{rhsProductionNode.ProductionName.name}:");
                productions rhsProductions = /*rhsProductionNode.Productions = */ rhsProductionNode.GenerateCascade ();
                foreach (RhsProduction rhsProduction in rhsProductions)
                {
                    Logger?.InfoRawLine<RhsProduction>
                    (
                        (production) =>
                        {
                            StringBuilder sb = new StringBuilder ();
                            sb.Append ("\t\t");
                            foreach (RhsNode rhsNode in production.rhsNodes)
                                sb.Append ($"{rhsNode.symbolToken.name} ");
                            if (production.priority > 0)
                            {
                                sb.Append ($"{production.priority}");
                                if (production.associativity != ProductionAssociativity.None)
                                    sb.Append ($" {production.associativity}");
                            }
                            else
                            {
                                if (production.associativity != ProductionAssociativity.None)
                                    sb.Append ($"{production.associativity}");
                            }
                            sb.AppendLine ();
                            return sb.ToString ();
                        },
                        rhsProduction
                    );
                }
            }
            return productions;
        }

        private void ResolveIdentities ()
        {
            foreach (RhsProductionNode rhsProductionNode in m_prodlist.Values)
            {
                RhsProductionNode identityNode;
                for (identityNode = rhsProductionNode.IdentityNode; (identityNode != null) && (identityNode.IdentityNode != null); identityNode = identityNode.IdentityNode)
                    ;
                if (identityNode == null)
                    continue;
                ResolveIdentityNode (rhsProductionNode, identityNode);
            }
        }

        private void ResolveIdentityNode (RhsProductionNode rhsProductionNode, RhsProductionNode identityNode)
        {
            string rhsNodeName = rhsProductionNode.ProductionName.name;
            SymbolToken identityNodeName = identityNode.ProductionName;
            foreach (RhsProductionNode productionNode in m_prodlist.Values)
            {
                foreach (RhsProduction rhsProduction in productionNode.Productions)
                {
                    foreach (RhsNode rhsNode in rhsProduction.rhsNodes)
                    {
                        if (rhsNode.symbolToken.name == rhsNodeName)
                            rhsNode.symbolToken = identityNodeName;
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void MakeCanonicalRhsSet ()
        {
            int stateNumber = 0;
            int low = 0;
            int high = 0;

            if (!m_stateset.Keys.Contains (m_firstRhsState))
            {
                m_firstRhsState.stateNumber = stateNumber++;
                m_stateset [m_firstRhsState] = m_firstRhsState;
                m_statearray.Add (m_firstRhsState);
                ++high;
            }
            else
            {
                Logger?.ErrorLine ("internal error in MakeCanonicalRhsSet()");
                return;
            }
            while (low < high)
            {
                RhsState p_RhsState = m_statearray [low++];
                p_RhsState.makeClosure ();
                p_RhsState.makeStates (Logger);
                int count = p_RhsState.ShiftArray.Count;
                for (int index = 0; index < count; ++index)
                {
                    RhsState p_state = p_RhsState.ShiftArray [index];
                    RhsState p_RhsStateRef = null;
                    if (!m_stateset.TryGetValue (p_state, out p_RhsStateRef))
                    {
                        p_state.stateNumber = stateNumber++;
                        m_stateset [p_state] = p_state;
                        m_statearray.Add (p_state);
                        ++high;
                    }
                    else
                    {
                        p_RhsState.changeShiftState (p_RhsStateRef, index);
                        p_state.Dispose ();
                    }
                }
                count = p_RhsState.GotoArray.Count;
                for (int index = 0; index < count; ++index)
                {
                    RhsState p_state = p_RhsState.GotoArray [index];
                    RhsState p_RhsStateRef = null;
                    if (!m_stateset.TryGetValue (p_state, out p_RhsStateRef))
                    {
                        p_state.stateNumber = stateNumber++;
                        m_stateset [p_state] = p_state;
                        m_statearray.Add (p_state);
                        ++high;
                    }
                    else
                    {
                        p_RhsState.changeGototState (p_RhsStateRef, index);
                        p_state.Dispose ();
                    }
                }
            }
            foreach (RhsProductionNode p_RhsProductionNode in m_prodlist.Values)
            {
                if (!p_RhsProductionNode.Used)
                    Logger?.WarnLine ("non-terminal symbol '" + p_RhsProductionNode.ProductionName.name + "' is not used");
            }

            //{
            //	Stream stream = File.Create("stateset.bin");
            //	BinaryFormatter binaryFormatter = new BinaryFormatter();
            //	binaryFormatter.Serialize (stream, this);
            //	stream.Close ();
            //}
        }

        /// <summary>
        /// 
        /// </summary>
        private void ComputeEpsilonConditions ()
        {
            while (true)
            {
                int count = 0;
                foreach (RhsProductionNode p_RhsProductionNode in m_prodlist.Values)
                {
                    foreach (RhsProduction p_RhsProduction in p_RhsProductionNode.Productions)
                    {
                        bool brokenLoop = false;
                        foreach (RhsNode p_RhsNode in p_RhsProduction.rhsNodes)
                        {
                            RhsProductionNode p_RhsProductionNodeRef = null;
                            SymbolToken p_SymbolToken = p_RhsNode.symbolToken;
                            if (!m_prodlist.TryGetValue (p_SymbolToken, out p_RhsProductionNodeRef))
                            {
                                brokenLoop = true;
                                break;
                            }
                            if (!p_RhsProductionNodeRef.EpsilonCondition)
                            {
                                brokenLoop = true;
                                break;
                            }
                        }
                        if (!brokenLoop)
                        {
                            if (!p_RhsProductionNode.EpsilonCondition)
                            {
                                p_RhsProductionNode.EpsilonCondition = true;
                                ++count;
                            }
                            break;
                        }
                    }
                }
                if (count == 0)
                    break;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void ComputeStartSets ()
        {
            while (true)
            {
                int count = 0;
                foreach (RhsProductionNode p_RhsProductionNode in m_prodlist.Values)
                {
                    foreach (RhsProduction p_RhsProduction in p_RhsProductionNode.Productions)
                    {
                        foreach (RhsNode p_RhsNode in p_RhsProduction.rhsNodes)
                        {
                            SymbolToken p_SymbolToken = p_RhsNode.symbolToken;
                            if (p_SymbolToken.declarator == (uint) AnglrClassificationType.TerminalName)
                            {
                                if (p_RhsProductionNode.insertStartElement (p_SymbolToken))
                                    ++count;
                                break;
                            }
                            if (p_SymbolToken.declarator == (uint) AnglrClassificationType.NonTerminalName)
                            {
                                RhsProductionNode p_RhsProductionNodeRef = null;
                                if (!m_prodlist.TryGetValue (p_SymbolToken, out p_RhsProductionNodeRef))
                                    break;
                                if (p_RhsProductionNode.insertStartSet (p_RhsProductionNodeRef.StartSet))
                                    ++count;
                                if (!p_RhsProductionNodeRef.EpsilonCondition)
                                    break;
                            }
                        }
                    }
                }
                if (count == 0)
                    break;
            }
            if (!CSharpGenerator.DisplayStartSetsFlag)
                return;
            Logger?.InfoRawLine ("Start sets:");
            foreach (KeyValuePair<SymbolToken, RhsProductionNode> keyval in m_prodlist)
            {
                Logger?.InfoRawLine (keyval.Key.name + ":");
                Logger?.InfoRawLine<object>
                (
                    (_) =>
                    {
                        StringBuilder sb = new StringBuilder ();
                        sb.Append ("{");
                        keyval.Value.StartSet.display (sb);
                        sb.AppendLine ("}");
                        return sb.ToString ();
                    },
                    null
                );
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void ComputeTransitionSets ()
        {
            int count = 0;
            while (true)
            {
                count = 0;
                foreach (RhsProductionNode p_RhsProductionNode in m_prodlist.Values)
                {
                    if (p_RhsProductionNode == null)
                        continue;
                    foreach (RhsProduction p_RhsProduction in p_RhsProductionNode.Productions)
                        count += ComputeTransitionSets (p_RhsProduction, new rhsenumerator (p_RhsProduction.rhsNodes));
                }
                if (count == 0)
                    break;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="p_RhsProduction"></param>
        /// <param name="rhsit"></param>
        /// <returns></returns>
        private int ComputeTransitionSets (RhsProduction p_RhsProduction, rhsenumerator rhsit)
        {
            int count = 0;

            if (rhsit.atEnd)
                return count;

            rhsenumerator rhsinc = new rhsenumerator (rhsit);
            count += ComputeTransitionSets (p_RhsProduction, rhsinc);

            RhsNode p_RhsNode = rhsit.currentRhsNode;
            if (rhsinc.atEnd)
            {
                p_RhsNode.opened = true;
                return count;
            }

            RhsNode p_RhsNodeInc = rhsinc.currentRhsNode;
            SymbolToken p_SymbolTokenInc = p_RhsNodeInc.symbolToken;
            if (p_SymbolTokenInc.declarator == (uint) AnglrClassificationType.TerminalName)
            {
                count += p_RhsNode.insertTransitionSet (p_SymbolTokenInc) ? 1 : 0;
                return count;
            }
            RhsProductionNode p_RhsProductionNodeInc = null;
            if (!m_prodlist.TryGetValue (p_SymbolTokenInc, out p_RhsProductionNodeInc))
                return count;
            if (p_RhsProductionNodeInc == null)
                return count;
            count += p_RhsNode.unionTransitionSet (p_RhsProductionNodeInc.StartSet) ? 1 : 0;
            if (p_RhsProductionNodeInc.EpsilonCondition)
            {
                count += p_RhsNode.unionTransitionSet (p_RhsNodeInc.transitionSet) ? 1 : 0;
                p_RhsNode.opened = p_RhsNodeInc.opened;
            }
            return count;
        }

        /// <summary>
        /// 
        /// </summary>
        private void ComputeFollowSets ()
        {
            int count;
            do
            {
                count = 0;
                foreach (RhsState p_rhsState in m_stateset.Values)
                    count += p_rhsState.computeFollowSets (Logger);
            }
            while (count > 0);
        }

        /// <summary>
        /// 
        /// </summary>
        private void PrepareTableGenerator ()
        {
            // mark most frequent goto transitions
            foreach (RhsState p_RhsState in m_stateset.Values)
                p_RhsState.markGotoCounters ();

            if (false)
            {
                int gotoCount = 0;
                foreach (KeyValuePair<SymbolToken, RhsProductionNode> keyval in m_prodlist)
                    if (keyval.Value != null)
                        keyval.Value.displayGotoCnt (Logger);
                Logger?.InfoRawLine ("OPTIMIZED GOTO = " + gotoCount);
            }

            // create shift set, goto set collections, mark default reduction rules
            foreach (RhsState p_RhsState in m_stateset.Values)
            {
                p_RhsState.addToShiftSet (m_shiftset);
                p_RhsState.addToGotoSet (m_gotoset);
                p_RhsState.markDefaultReduction ();
                p_RhsState.checkGLRCondition ();
                p_RhsState.addToShiftSet (m_shiftset);
            }

            foreach (RhsProductionNode p_RhsProductionNode in m_prodlist.Values)
                if (p_RhsProductionNode != null)
                    p_RhsProductionNode.defineBestGotoCounter ();

            magicNr = 0;
            foreach (RhsState rhsState in m_stateset.Values)
            {
                foreach (RhsConfiguration rhsConfiguration in rhsState.m_core.Values)
                {
                    magicNr += rhsConfiguration.rhsIterator.position;
                    string name = rhsConfiguration.rhsProduction.symbolToken.name;
                    foreach (char c in name)
                        magicNr += c;
                    foreach (RhsNode rhsNode in rhsConfiguration.rhsProduction.rhsNodes)
                    {
                        name = rhsNode.symbolToken.name;
                        foreach (char c in name)
                            magicNr += c;
                    }
                }
            }

            //foreach (statedelta p_statedelta in m_shiftset.Values)
            //{
            //	stateset p_set = p_statedelta.first;
            //	stateSetInfo p_info = p_statedelta.second;
            //	int minToken = -1;
            //	int maxToken = -1;
            //	foreach (KeyValuePair<SymbolToken, RhsState> keyval in p_set)
            //	{
            //		SymbolToken p_SymbolToken = keyval.Key;
            //		int index = p_SymbolToken.index;
            //		if ((minToken < 0) || (minToken > index))
            //			minToken = index;
            //		if (maxToken < index)
            //			maxToken = index;
            //	}
            //	p_info.m_min = minToken;
            //	p_info.m_max = maxToken;
            //}

            //foreach (statedelta p_statedelta in m_gotoset.Values)
            //{
            //	stateset p_set = p_statedelta.first;
            //	stateSetInfo p_info = p_statedelta.second;
            //	int minToken = -1;
            //	int maxToken = -1;
            //	foreach (KeyValuePair<SymbolToken, RhsState> keyval in p_set)
            //	{
            //		SymbolToken p_SymbolToken = keyval.Key;
            //		RhsState p_RhsState = keyval.Value;
            //		RhsProductionNode p_RhsProductionNode = m_prodlist [p_SymbolToken];
            //		if (p_RhsProductionNode == null)
            //			continue;
            //		if (p_RhsState.stateNumber == p_RhsProductionNode.bestGoto)
            //			continue;
            //		int index = p_SymbolToken.index;
            //		if ((minToken < 0) || (minToken > index))
            //			minToken = index;
            //		if (maxToken < index)
            //			maxToken = index;
            //	}
            //	p_info.m_min = minToken;
            //	p_info.m_max = maxToken;
            //}
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="f"></param>
        private void GenerateTables (TextWriter f)
        {
            GenerateMagicNumber (f);
            GenerateTerminalTables (f);
            GenerateNonTerminalTables (f);
            GenerateGLRTables (f);
            GenerateTransitionTables (f);
            GenerateShiftDeltas (f);
            GenerateGotoDeltas (f);
            GenerateProductionTables (f);
            GenerateReductionTables (f);
            if (true)
                CheckPDA ();
            GenerateSemanticClasses ();
        }

        private void GenerateProdIds (TextWriter f)
        {
            foreach (SymbolToken p_SymbolToken in m_prodlist.Keys)
            {
                if (p_SymbolToken.name [0] == '$')
                    continue;
                string productionName = p_SymbolToken.correctName;
                f.WriteLine ("\t\t_" + productionName + "_ID = " + p_SymbolToken.index + ",");
            }
        }

        public void GenerateFragmenmts (TextWriter f)
        {
            Dictionary<SymbolToken, RhsState> fragments = new Dictionary<SymbolToken, RhsState> ();
            foreach (RhsState p_state in m_statearray)
            {
                int count = p_state.closure.Count;
                foreach (SymbolToken symbolToken in p_state.closure.Keys)
                {
                    if (fragments.Keys.Contains (symbolToken))
                    {
                        RhsState rhsState = fragments [symbolToken];
                        if (rhsState.closure.Count <= count)
                            continue;
                    }
                    fragments [symbolToken] = p_state;
                }
            }
            foreach (KeyValuePair<SymbolToken, RhsState> fragment in fragments)
            {
                RhsState rhsState = fragment.Value;
                closurelist closure = rhsState.closure;
                RhsClosureElt rhsClosureElt = null;
                if (!closure.TryGetValue (fragment.Key, out rhsClosureElt))
                    continue;
                TokenSet tokenSet = rhsClosureElt.getFollowSet;
                tokset tokens = tokenSet.m_tokset;
                SymbolToken firstSymbol = null;
                foreach (SymbolToken symbol in tokens.Keys)
                {
                    firstSymbol = symbol;
                    if (symbol.index == Constants.TOKEN_EOF)
                        continue;
                    break;
                }
                if (firstSymbol == null)
                    continue;
                f.WriteLine ($"\t\t\t(\"{fragment.Key.name}\", ProductionID._{fragment.Key.correctName}_ID, {fragment.Value.stateNumber}, {firstSymbol.index}, \"{firstSymbol.correctName}\"),");
            }
        }

        public void GenerateScannerEvents (TextWriter f)
        {
            foreach (var scannerPartPair in scannerParts)
            {
                string scannerPartName = scannerPartPair.Key;
                ScannerPart scannerPart = scannerPartPair.Value;
                if ((scannerPart.eventExample != null) && (scannerPart.eventExample.Length != 0))
                    f.WriteLine (scannerPart.eventExample);
            }
        }

        private void GenerateMagicNumber (TextWriter f)
        {
            f.WriteLine ();
            f.WriteLine ($"\t\tinternal readonly static int g_magicNumber = {magicNr};");
            f.WriteLine ();
        }

        private void GenerateTerminalTables (TextWriter f)
        {
            if (false)
            {
                Logger?.InfoRawLine ("INFO: TERMINALS");
                Logger?.InfoRawLine ("");

                foreach (SymbolToken p_SymbolToken in m_terminals)
                {
                    Logger?.InfoRawLine ("#define\t" + p_SymbolToken.correctName + "\t" + p_SymbolToken.index);
                }
                Logger?.InfoRawLine ("");

                Logger?.InfoRawLine ("#define\tminTerminalCode\t" + m_minTerminalNr);
                Logger?.InfoRawLine ("#define\tmaxTerminalCode\t" + m_maxTerminalNr);
                Logger?.InfoRawLine ("");
            }

            m_terminalCodes = new int [m_terminals.Count];
            int i = 0;
            f.WriteLine ($"\t\tinternal readonly static int g_minTerminalCode = {m_minTerminalNr};");
            f.WriteLine ("\t\tinternal readonly static int[] g_terminalCodes = ");
            f.WriteLine ("\t\t{");
            foreach (SymbolToken p_SymbolToken in m_terminals)
            {
                if ((i % 10) == 0)
                    f.Write ("\t\t\t");
                f.Write ("{0,6:d}, ", (m_terminalCodes [i] = p_SymbolToken.index));
                if ((++i % 10) == 0)
                    f.WriteLine ();
            }
            if ((i % 10) != 0)
                f.WriteLine ();
            f.WriteLine ("\t\t};");
            f.WriteLine ();

            f.WriteLine ("\t\tinternal readonly static string[] g_terminalNames = ");
            f.WriteLine ("\t\t{");
            foreach (SymbolToken p_SymbolToken in m_terminals)
            {
                f.WriteLine ($"\t\t\t\"{p_SymbolToken.name}\",");
            }
            f.WriteLine ("\t\t};");
            f.WriteLine ();
        }

        private void GenerateNonTerminalTables (TextWriter f)
        {
            if (false)
            {
                Logger?.InfoRawLine ("INFO: NONTERMINALS");
                Logger?.InfoRawLine ("");

                foreach (SymbolToken p_SymbolToken in m_nonterminals)
                {
                    Logger?.InfoRawLine ("#define\t" + p_SymbolToken.correctName + "\t" + p_SymbolToken.index);
                }
                Logger?.InfoRawLine ("");

                Logger?.InfoRawLine ("#define\tminNonTerminalCode\t" + m_minNonTerminalNr);
                Logger?.InfoRawLine ("#define\tmaxNonTerminalCode\t" + m_maxNonTerminalNr);
                Logger?.InfoRawLine ("");
            }

            m_nonTerminalCodes = new int [m_nonterminals.Count];
            int i = 0;
            f.WriteLine ($"\t\tinternal readonly static int g_minNonTerminalCode = {m_minNonTerminalNr};");
            f.WriteLine ("\t\tinternal readonly static int[] g_nonTerminalCodes = ");
            f.WriteLine ("\t\t{");
            foreach (SymbolToken p_SymbolToken in m_nonterminals)
            {
                if ((i % 10) == 0)
                    f.Write ("\t\t\t");
                f.Write ("{0,6:d}, ", (m_nonTerminalCodes [i] = p_SymbolToken.index));
                if ((++i % 10) == 0)
                    f.WriteLine ();
            }
            if ((i % 10) != 0)
                f.WriteLine ();
            f.WriteLine ("\t\t};");
            f.WriteLine ();

            f.WriteLine ("\t\tinternal readonly static string[] g_nonTerminalNames = ");
            f.WriteLine ("\t\t{");
            foreach (SymbolToken p_SymbolToken in m_nonterminals)
            {
                f.WriteLine ($"\t\t\t\"{p_SymbolToken.name}\",");
            }
            f.WriteLine ("\t\t};");
            f.WriteLine ();
        }

        private void GenerateTransitionTables (TextWriter f)
        {
            HashSet<int> deltaset = new HashSet<int> ();
            int i;
            int size = 1000;
            m_check = new int [size];
            m_state = new int [size];
            m_maxCheck = -1;

            foreach (statedelta shiftit in m_shiftset.Values)
            {
                stateset p_shiftset = shiftit.first;
                int delta = 1;
                while (true)
                {
                    while (deltaset.Contains (delta))
                        ++delta;

                    bool brokenLoop = false;
                    foreach (SymbolToken p_SymbolToken in p_shiftset.Keys)
                    {
                        int index = delta + p_SymbolToken.index;
                        if (index > m_maxCheck)
                            m_maxCheck = index;
                        if (index >= size)
                            size = ResizeTransitionTables (size, 1000);
                        if (m_check [index] != 0)
                        {
                            brokenLoop = true;
                            break;
                        }
                    }
                    if (!brokenLoop)
                        break;
                    ++delta;
                }
                deltaset.Add (shiftit.second.m_delta = delta);
                foreach (KeyValuePair<SymbolToken, RhsState> stateit in p_shiftset)
                {
                    SymbolToken p_SymbolToken = stateit.Key;
                    int index = delta + p_SymbolToken.index;
                    if (m_check [index] != 0)
                        Logger?.WarnLine ("overwrite fiefd");
                    m_check [index] = index - delta;
                    m_state [index] = stateit.Value.stateNumber;
                }
            }

            foreach (statedelta gotoit in m_gotoset.Values)
            {
                stateset p_gotoset = gotoit.first;
                int delta = 1;
                while (true)
                {
                    while (deltaset.Contains (delta))
                        ++delta;

                    bool brokenLoop = false;
                    foreach (KeyValuePair<SymbolToken, RhsState> stateit in p_gotoset)
                    {
                        SymbolToken p_SymbolToken = stateit.Key;
                        RhsState p_RhsState = stateit.Value;
                        RhsProductionNode p_RhsProductionNode = m_prodlist [p_SymbolToken];
                        if (p_RhsProductionNode == null)
                            continue;
                        if (p_RhsState.stateNumber == p_RhsProductionNode.BestGoto)
                            continue;
                        int index = delta + p_SymbolToken.index;
                        if (index > m_maxCheck)
                            m_maxCheck = index;
                        if (index >= size)
                            size = ResizeTransitionTables (size, 1000);
                        if (m_check [index] != 0)
                        {
                            brokenLoop = true;
                            break;
                        }
                    }
                    if (!brokenLoop)
                        break;
                    ++delta;
                }
                deltaset.Add (gotoit.second.m_delta = delta);
                foreach (KeyValuePair<SymbolToken, RhsState> stateit in p_gotoset)
                {
                    SymbolToken p_SymbolToken = stateit.Key;
                    RhsState p_RhsState = stateit.Value;
                    RhsProductionNode p_RhsProductionNode = m_prodlist [p_SymbolToken];
                    if (p_RhsProductionNode == null)
                        continue;
                    if (p_RhsState.stateNumber == p_RhsProductionNode.BestGoto)
                        continue;
                    int index = delta + p_SymbolToken.index;
                    if (m_check [index] != 0)
                        Logger?.WarnLine ("overwrite fiefd");
                    m_check [index] = index - delta;
                    m_state [index] = p_RhsState.stateNumber /*- m_minNonTerminalNr*/;
                }
            }

            ++m_maxCheck;

            f.WriteLine ("\t\tinternal readonly static int[] g_check =");
            f.WriteLine ("\t\t{");
            for (i = 0; i < m_maxCheck;)
            {
                if ((i % 10) == 0)
                    f.Write ("\t\t\t");
                f.Write ("{0,6:d}, ", m_check [i]);
                if ((++i % 10) == 0)
                    f.WriteLine ();
            }
            if ((i % 10) != 0)
                f.WriteLine ();
            f.WriteLine ("\t\t};");
            f.WriteLine ();

            f.WriteLine ("\t\tinternal readonly static int[] g_state =");
            f.WriteLine ("\t\t{");
            for (i = 0; i < m_maxCheck;)
            {
                if ((i % 10) == 0)
                    f.Write ("\t\t\t");
                f.Write ("{0,6:d}, ", m_state [i]);
                if ((++i % 10) == 0)
                    f.WriteLine ();
            }
            if ((i % 10) != 0)
                f.WriteLine ();
            f.WriteLine ("\t\t};");
            f.WriteLine ();
        }

        private void GenerateShiftDeltas (TextWriter f)
        {
            int size = m_stateset.Count;
            m_shiftDelta = new int [size];
            foreach (RhsState p_RhsState in m_stateset.Values)
            {
                int val = 0;
                statedelta sd = p_RhsState.findShiftSet (m_shiftset);
                if (sd.first.Count > 0)
                    val = sd.second.m_delta;
                m_shiftDelta [p_RhsState.stateNumber] = p_RhsState.hasGLRCondition ? -p_RhsState.GLRDelta : val;
            }

            int i;
            f.WriteLine ("\t\tinternal readonly static int[] g_shiftDelta =");
            f.WriteLine ("\t\t{");
            for (i = 0; i < size;)
            {
                if ((i % 10) == 0)
                    f.Write ("\t\t\t");
                f.Write ("{0,6:d}, ", m_shiftDelta [i]);
                if ((++i % 10) == 0)
                    f.WriteLine ();
            }
            if ((i % 10) != 0)
                f.WriteLine ();
            f.WriteLine ("\t\t};");
            f.WriteLine ();
        }

        private void GenerateGotoDeltas (TextWriter f)
        {
            int size = m_stateset.Count;
            m_gotoDelta = new int [size];
            foreach (RhsState p_RhsState in m_stateset.Values)
            {
                int val = 0;
                statedelta sd = p_RhsState.findGotoSet (m_gotoset);
                if (sd.first.Count > 0)
                    val = sd.second.m_delta;
                m_gotoDelta [p_RhsState.stateNumber] = val;
            }

            int i;
            f.WriteLine ("\t\tinternal readonly static int[] g_gotoDelta =");
            f.WriteLine ("\t\t{");
            for (i = 0; i < size;)
            {
                if ((i % 10) == 0)
                    f.Write ("\t\t\t");
                f.Write ("{0,6:d}, ", m_gotoDelta [i]);
                if ((++i % 10) == 0)
                    f.WriteLine ();
            }
            if ((i % 10) != 0)
                f.WriteLine ();
            f.WriteLine ("\t\t};");
            f.WriteLine ();
        }

        private void GenerateProductionTables (TextWriter f)
        {
            int i;

            int size = RhsProduction.productionCounter + 1;
            m_productionLengths = new int [size];
            m_productionLengths [0] = 0;

            foreach (RhsProductionNode p_RhsProductionNode in m_prodlist.Values)
            {
                if (p_RhsProductionNode == null)
                    continue;
                foreach (RhsProduction p_RhsProduction in p_RhsProductionNode.Productions)
                {
                    m_productionLengths [p_RhsProduction.productionNumber] = p_RhsProduction.length;
                }
            }

            f.WriteLine ("\t\tinternal readonly static int[] g_productionLengths = ");
            f.WriteLine ("\t\t{");
            for (i = 0; i < size;)
            {
                if ((i % 10) == 0)
                    f.Write ("\t\t\t");
                f.Write ("{0,6:d}, ", m_productionLengths [i]);
                if ((++i % 10) == 0)
                    f.WriteLine ();
            }
            if ((i % 10) != 0)
                f.WriteLine ();
            f.WriteLine ("\t\t};");
            f.WriteLine ();

            m_productionRules = new int [size];
            m_productionRules [0] = 0;

            foreach (KeyValuePair<SymbolToken, RhsProductionNode> prodit in m_prodlist)
            {
                RhsProductionNode p_RhsProductionNode = prodit.Value;
                if (p_RhsProductionNode == null)
                    continue;
                SymbolToken p_SymbolToken = prodit.Key;
                int index = p_SymbolToken.index;
                foreach (RhsProduction p_RhsProduction in p_RhsProductionNode.Productions)
                {
                    m_productionRules [p_RhsProduction.productionNumber] = index /*- m_minNonTerminalNr*/;
                }
            }

            f.WriteLine ("\t\tinternal readonly static int[] g_productionRules = ");
            f.WriteLine ("\t\t{");
            for (i = 0; i < size;)
            {
                if ((i % 10) == 0)
                    f.Write ("\t\t\t");
                f.Write ("{0,6:d}, ", m_productionRules [i]);
                if ((++i % 10) == 0)
                    f.WriteLine ();
            }
            if ((i % 10) != 0)
                f.WriteLine ();
            f.WriteLine ("\t\t};");
            f.WriteLine ();
            m_defaultGoto = new int [m_maxNonTerminalNr + 1];

            foreach (KeyValuePair<SymbolToken, RhsProductionNode> prodit in m_prodlist)
            {
                RhsProductionNode p_RhsProductionNode = prodit.Value;
                if (p_RhsProductionNode == null)
                    continue;
                SymbolToken p_SymbolToken = prodit.Key;
                if (p_SymbolToken.declarator == (uint) AnglrClassificationType.TerminalName)
                    continue;
                if (p_SymbolToken.index >= size)
                    Logger?.WarnLine ("default goto " + p_SymbolToken.index + " > " + size);
                m_defaultGoto [p_SymbolToken.index /*- m_minNonTerminalNr*/] = p_RhsProductionNode.BestGoto;
            }

            f.WriteLine ("\t\tinternal readonly static int[] g_defaultGoto = ");
            f.WriteLine ("\t\t{");
            for (i = 0; i < m_maxNonTerminalNr /*- m_minNonTerminalNr*/;)
            {
                if ((i % 10) == 0)
                    f.Write ("\t\t\t");
                f.Write ("{0,6:d}, ", m_defaultGoto [i]);
                if ((++i % 10) == 0)
                    f.WriteLine ();
            }
            if ((i % 10) != 0)
                f.WriteLine ();
            f.WriteLine ("\t\t};");
            f.WriteLine ();
        }

        private void GenerateReductionTables (TextWriter f)
        {
            HashSet<int> deltaset = new HashSet<int> ();
            int i;
            int size = 1000;
            m_rcheck = new int [size];
            m_rstate = new int [size];
            m_maxRCheck = -1;

            foreach (RhsState p_RhsState in m_stateset.Values)
            {
                if (p_RhsState.reductionsSetSize < 2)
                    continue;
                prodset p_reductions = p_RhsState.reductions;

                int delta = 1;
                while (true)
                {
                    while (deltaset.Contains (delta))
                        ++delta;

                    bool brokenLoop = false;
                    foreach (RhsConfiguration p_RhsConfiguration in p_reductions.Values)
                    {
                        brokenLoop = false;
                        TokenSet p_TokenSet = p_RhsConfiguration.getFollowSet;
                        tokset p_tokset = p_TokenSet.set;
                        foreach (SymbolToken p_SymbolToken in p_tokset.Values)
                        {
                            int index = delta + p_SymbolToken.index;
                            if (index > m_maxRCheck)
                                m_maxRCheck = index;
                            if (index >= size)
                                size = ResizeReductionTables (size, 1000);
                            if (m_rcheck [index] != 0)
                            {
                                brokenLoop = true;
                                break;
                            }
                        }
                        if (brokenLoop)
                            break;
                    }
                    if (!brokenLoop)
                        break;
                    ++delta;
                }

                p_RhsState.reductionsDelta = delta;
                deltaset.Add (delta);

                foreach (KeyValuePair<int, RhsConfiguration> prodit in p_reductions)
                {
                    RhsConfiguration p_RhsConfiguration = prodit.Value;
                    TokenSet p_TokenSet = p_RhsConfiguration.getFollowSet;
                    tokset p_tokset = p_TokenSet.set;
                    foreach (SymbolToken p_SymbolToken in p_tokset.Values)
                    {
                        int index = p_SymbolToken.index;
                        // reduce/reduce conflict
                        if (m_rcheck [delta + index] != 0)
                            ; //Logger?.WarnRawLine ("reduce/reduce overwrite");
                        m_rcheck [delta + index] = index;
                        m_rstate [delta + index] = prodit.Key;
                    }
                }
            }

            ++m_maxRCheck;

            f.WriteLine ("\t\tinternal readonly static int[] g_rcheck =");
            f.WriteLine ("\t\t{");
            if (m_maxRCheck == 0)
                f.WriteLine ("\t\t\t0");
            for (i = 0; i < m_maxRCheck;)
            {
                if ((i % 10) == 0)
                    f.Write ("\t\t\t");
                f.Write ("{0,6:d}, ", m_rcheck [i]);
                if ((++i % 10) == 0)
                    f.WriteLine ();
            }
            if ((i % 10) != 0)
                f.WriteLine ();
            f.WriteLine ("\t\t};");
            f.WriteLine ();

            f.WriteLine ("\t\tinternal readonly static int[] g_rstate =");
            f.WriteLine ("\t\t{");
            if (m_maxRCheck == 0)
                f.WriteLine ("\t\t\t0");
            for (i = 0; i < m_maxRCheck;)
            {
                if ((i % 10) == 0)
                    f.Write ("\t\t\t");
                f.Write ("{0,6:d}, ", m_rstate [i]);
                if ((++i % 10) == 0)
                    f.WriteLine ();
            }
            if ((i % 10) != 0)
                f.WriteLine ();
            f.WriteLine ("\t\t};");
            f.WriteLine ();

            size = m_stateset.Count;
            m_reductions = new int [size];

            foreach (RhsState p_RhsState in m_stateset.Values)
                m_reductions [p_RhsState.stateNumber] = (p_RhsState.reductionsSetSize > 1) ? -p_RhsState.reductionsDelta : p_RhsState.defaultReduction;

            f.WriteLine ("\t\tinternal readonly static int[] g_reductions =");
            f.WriteLine ("\t\t{");
            for (i = 0; i < size;)
            {
                if ((i % 10) == 0)
                    f.Write ("\t\t\t");
                f.Write ("{0,6:d}, ", m_reductions [i]);
                if ((++i % 10) == 0)
                    f.WriteLine ();
            }
            if ((i % 10) != 0)
                f.WriteLine ();
            f.WriteLine ("\t\t};");
            f.WriteLine ();
        }

        private void GenerateGLRTables (TextWriter f)
        {
            int i;
            HashSet<int> deltaset = new HashSet<int> ();
            int size = 1000;
            m_glrcheck = new int [size];
            m_glrstate = new int [size];
            m_maxGLRCheck = -1;
            int glrsize = 0;
            glrtokset p_glrtokset = new glrtokset ();

            foreach (RhsState p_RhsState in m_stateset.Values)
            {
                if (!p_RhsState.hasGLRCondition)
                    continue;
                glrset p_glrset = p_RhsState.getGLRSet;
                foreach (GLRToken p_GLRToken in p_glrset.Values)
                {
                    GLRToken p_GLRTokenRef = null;
                    if (p_glrtokset.TryGetValue (p_GLRToken, out p_GLRTokenRef))
                    {
                        p_GLRToken.position = p_GLRTokenRef.position;
                        continue;
                    }
                    p_GLRToken.position = glrsize;
                    p_glrtokset [p_GLRToken] = p_GLRToken;
                    glrsize += 2 + p_GLRToken.getRdlist.Count;
                }
            }

            m_glrcells = new int [glrsize];

            foreach (GLRToken p_GLRToken in p_glrtokset.Values)
            {
                int position = p_GLRToken.position;
                rdlist p_rdlist = p_GLRToken.getRdlist;
                m_glrcells [position++] = 2 + p_rdlist.Count;
                m_glrcells [position++] = p_GLRToken.state;
                foreach (RhsProduction p_RhsProduction in p_rdlist)
                    m_glrcells [position++] = p_RhsProduction.productionNumber;
            }

            foreach (RhsState p_RhsState in m_stateset.Values)
            {
                if (!p_RhsState.hasGLRCondition)
                    continue;
                glrset p_glrset = p_RhsState.getGLRSet;
                int delta = 1;
                while (true)
                {
                    while (deltaset.Contains (delta))
                        ++delta;

                    bool brokenLoop = false;
                    foreach (SymbolToken p_SymbolToken in p_glrset.Keys)
                    {
                        int index = delta + p_SymbolToken.index;
                        if (index > m_maxGLRCheck)
                            m_maxGLRCheck = index;
                        if (index >= size)
                            size = ResizeGLRTables (size, 1000);
                        if (m_glrcheck [index] != 0)
                        {
                            brokenLoop = true;
                            break;
                        }
                    }
                    if (!brokenLoop)
                        break;
                    ++delta;
                }
                p_RhsState.GLRDelta = delta;
                deltaset.Add (delta);
                foreach (KeyValuePair<SymbolToken, GLRToken> glrit in p_glrset)
                {
                    SymbolToken p_SymbolToken = glrit.Key;
                    int index = delta + p_SymbolToken.index;
                    if (m_glrcheck [index] != 0)
                        Logger?.WarnLine ("overwrite glr-check field, index = " + index);
                    m_glrcheck [index] = index - delta;
                    m_glrstate [index] = glrit.Value.position;
                }
            }

            ++m_maxGLRCheck;

            f.WriteLine ("\t\tinternal readonly static int[] g_glrcheck =");
            f.WriteLine ("\t\t{");
            for (i = 0; i < m_maxGLRCheck;)
            {
                if ((i % 10) == 0)
                    f.Write ("\t\t\t");
                f.Write ("{0,6:d}, ", m_glrcheck [i]);
                if ((++i % 10) == 0)
                    f.WriteLine ();
            }
            if ((i % 10) != 0)
                f.WriteLine ();
            f.WriteLine ("\t\t};");
            f.WriteLine ();

            f.WriteLine ("\t\tinternal readonly static int[] g_glrstate =");
            f.WriteLine ("\t\t{");
            for (i = 0; i < m_maxGLRCheck;)
            {
                if ((i % 10) == 0)
                    f.Write ("\t\t\t");
                f.Write ("{0,6:d}, ", m_glrstate [i]);
                if ((++i % 10) == 0)
                    f.WriteLine ();
            }
            if ((i % 10) != 0)
                f.WriteLine ();
            f.WriteLine ("\t\t};");
            f.WriteLine ();

            f.WriteLine ("\t\tinternal readonly static int[] g_glrcells =");
            f.WriteLine ("\t\t{");
            for (i = 0; i < glrsize;)
            {
                if ((i % 10) == 0)
                    f.Write ("\t\t\t");
                f.Write ("{0,6:d}, ", m_glrcells [i]);
                if ((++i % 10) == 0)
                    f.WriteLine ();
            }
            if ((i % 10) != 0)
                f.WriteLine ();
            f.WriteLine ("\t\t};");
            f.WriteLine ();
        }

        private void GenerateParserSrc (_parser_part_ part)
        {
            string fileName = OutputDir + parserClassName + ".cs";
            try
            {
                Logger?.InfoRawLine ("Generate parser source file '" + fileName + "'");
                StreamWriter f = File.CreateText (fileName);

                f.WriteLine ($"{m_generatedHeaderText}");

                foreach (string line in g_ParserTemplateCsSrc)
                {
                    if (line == g_CsTables)
                        GenerateTables (f);
                    else if (line == g_CsActions)
                    {
                        foreach (int key in m_proddict.Keys)
                        {
                            f.WriteLine ("\t\t\tcase " + key + ":");
                            RhsProduction p_RhsProduction = m_proddict [key];
                            int prodLen = p_RhsProduction.length;
                            if (p_RhsProduction.symbolToken.name [0] == '$')
                            {
                                f.WriteLine ("\t\t\tbreak;");
                                continue;
                            }
                            f.Write ("\t\t\t\tcurrentValue = " + p_RhsProduction.symbolToken.correctName + "_" + p_RhsProduction.index + " (");
                            string sep = "";
                            int cnt = 0;
                            foreach (RhsNode p_RhsNode in p_RhsProduction.rhsNodes)
                            {
                                SymbolToken p_SymbolToken = p_RhsNode.symbolToken;
                                f.Write (sep);
                                if (p_SymbolToken.declarator == (uint) AnglrClassificationType.TerminalName)
                                    f.Write ("(SyntaxTreeToken) valueStack [" + (cnt - prodLen) + "]");
                                else
                                    f.Write ("(" + p_SymbolToken.correctName + ") valueStack [" + (cnt - prodLen) + "]");
                                sep = ", ";
                                ++cnt;
                            }
                            f.WriteLine (");");
                            f.WriteLine ("\t\t\t\tbreak;");
                        }
                    }
                    else if (line == g_CsCounters)
                    {
                        foreach (SymbolToken p_SymbolToken in m_prodlist.Keys)
                        {
                            if (p_SymbolToken.name [0] == '$')
                                continue;
                            f.WriteLine ("\t\t\tif (" + p_SymbolToken.correctName + ".g_counter != 0)");
                            f.WriteLine ("\t\t\t\tAnglrLogger?.DebugRawLine (\"" + p_SymbolToken.correctName + ".g_counter = \" + " + p_SymbolToken.correctName + ".g_counter);");
                        }
                    }
                    else if (line == g_CSEventRegistration)
                    {
                        f.WriteLine ("\t\t\t//");
                        f.WriteLine ("\t\t\t// event registration templates");
                        f.WriteLine ("\t\t\t//");
                        f.WriteLine ("\t\t\tCommon_Event += Invoke_Common_Callback;");
                        foreach (SymbolToken p_SymbolToken in m_prodlist.Keys)
                        {
                            if (p_SymbolToken.name [0] == '$')
                                continue;
                            f.WriteLine ("\t\t\t" + p_SymbolToken.correctName + "_Event += Invoke_" + p_SymbolToken.correctName + "_Callback;");
                        }
                    }
                    else if (line == g_CSEventDeclaration)
                    {
                        f.WriteLine ();
                        f.WriteLine ("\t\t//");
                        f.WriteLine ("\t\t// event handler templates");
                        f.WriteLine ("\t\t//");
                        f.WriteLine ();
                        f.WriteLine ("\t\tprivate bool Invoke_Common_Callback (SyntaxTreeCallbackReason reason, int kind, SyntaxTreeBase p_node)");
                        foreach (string tline in g_EventTestBodyCs)
                            f.WriteLine (tline);
                        foreach (SymbolToken p_SymbolToken in m_prodlist.Keys)
                        {
                            if (p_SymbolToken.name [0] == '$')
                                continue;
                            f.WriteLine ();
                            f.WriteLine ("\t\tprivate bool Invoke_" + p_SymbolToken.correctName + "_Callback (SyntaxTreeCallbackReason reason, " + p_SymbolToken.correctName + ".production_kind kind, " + p_SymbolToken.correctName + " p_" + p_SymbolToken.correctName + ")");
                            foreach (string lptr in g_EventTestBodyCs)
                                f.WriteLine (lptr);
                        }
                        f.WriteLine ();
                        f.WriteLine ("\t\t//");
                        f.WriteLine ("\t\t// event fire templates");
                        f.WriteLine ("\t\t//");
                        foreach (SymbolToken p_SymbolToken in m_prodlist.Keys)
                        {
                            if (p_SymbolToken.name [0] == '$')
                                continue;
                            f.WriteLine ();
                            f.WriteLine ("\t\tpublic void InvokeTest (" + p_SymbolToken.correctName + " p_" + p_SymbolToken.correctName + ")");
                            f.WriteLine ("\t\t{");
                            f.WriteLine ("\t\t\tTraverse (p_" + p_SymbolToken.correctName + ");");
                            f.WriteLine ("\t\t\tTraverseCommon (p_" + p_SymbolToken.correctName + ");");
                            f.WriteLine ("\t\t}");
                        }
                    }
                    else if (line == g_ScannerNamespace)
                    {
                        SortedSet<string> nameSpaces = new SortedSet<string> ();
                        ParserPart parserPart = attributeCollection.parserParts [part.m__identifier_.text];
                        SymbolToken lexerPartSymbol = parserPart.lexerPartSymbol;
                        if (lexerPartSymbol != null)
                        {
                            LexerPart lexerPart = attributeCollection.lexerParts [lexerPartSymbol.name];
                            for (symtab.Enumerator enumerator = lexerPart.scannerSymbols.enumerator; enumerator.MoveNext ();)
                            {
                                SymbolToken symbolToken = enumerator.Current.Value;
                                ScannerPart scannerPart = attributeCollection.scannerParts [symbolToken.name];
                                SymbolToken declarationSymbol = scannerPart.declarationPartSymbol;
                                if (declarationSymbol == null)
                                    continue;
                                DeclarationsPart declarationsPart = null;
                                if (!attributeCollection.declarationParts.TryGetValue (declarationSymbol.name, out declarationsPart))
                                    continue;
                                nameSpaces.Add (declarationsPart.declarationsNameSpace);
                            }
                            if (lexerPart.lexerNameSpace != null)
                                nameSpaces.Add (lexerPart.lexerNameSpace);
                        }

                        foreach (string name in nameSpaces)
                        {
                            f.WriteLine ($"using {name};");
                        }
                        f.WriteLine ();
                    }
                    else if (line == g_prodIds)
                        GenerateProdIds (f);
                    else if (line == g_fragments)
                        GenerateFragmenmts (f);
                    else if (line == g_ScannerEvents)
                        GenerateScannerEvents (f);
                    else
                    {
                        ParserPart parserPart = parserParts [part.m__identifier_.text];
                        LexerPart lexerPart = lexerParts [parserPart.lexerId];
                        f.WriteLine
                        (
                            line.
                            Replace (g_CsClassname, parserPart.parserClassName).
                            Replace (g_CsNameSpace, parserPart.parserNameSpace).
                            Replace (g_ScannerClassDef, lexerPart.lexerClassName).
                            Replace (g_StartRule, m_startProductionNode.ProductionName.correctName)
                        );
                    }
                }
                f.Close ();
                SourceFileList.Add (fileName);
            }
            catch (Exception e)
            {
                Logger?.ErrorLine ($"Cannot create parser source file '{fileName}', exception thrown: {e.Message}");
            }
        }

        private void GenerateParserHdr ()
        {
            return;
#pragma warning disable CS0162
            string fileName = parserClassName + ".h";
            Logger?.InfoRawLine ("Generate parser header file '" + fileName + "'");
            StreamWriter f = File.CreateText (fileName);

            f.WriteLine ($"{m_generatedHeaderText}");

            foreach (string line in g_ParserTemplateCsHdr)
            {
                f.WriteLine (line.Replace (g_CsClassname, parserClassName));
            }

            f.Close ();
#pragma warning restore CS0162
        }

        private void CheckPDA ()
        {
            foreach (RhsState p_RhsState in m_stateset.Values)
            {
                int stateNumber = p_RhsState.stateNumber;

                int delta = m_shiftDelta [stateNumber];
                if (delta > 0)
                    foreach (SymbolToken p_SymbolToken in m_terminals)
                    {
                        RhsState p_RhsStateRef = p_RhsState.checkShift (p_SymbolToken);
                        int index = p_SymbolToken.index;
                        if ((index + delta < 0) || (index + delta >= m_maxCheck) || (m_check [index + delta] != index))
                        {
                            if (p_RhsStateRef != null)
                                Logger?.InfoRawLine ("state " + stateNumber + ", shift " + p_SymbolToken.name + " not handled");
                        }
                        else
                        {
                            if (p_RhsStateRef != null)
                            {
                                if (m_state [index + delta] != p_RhsStateRef.stateNumber)
                                    Logger?.InfoRawLine ("state " + stateNumber + ", shift " + p_SymbolToken.name + " to " + m_state [index + delta]);
                            }
                            else
                                Logger?.InfoRawLine ("state " + stateNumber + ", shift " + p_SymbolToken.name + " handled");
                        }
                    }
                else
                {
                    if (m_reductions [stateNumber] == 0)
                        Logger?.InfoRawLine ("state " + stateNumber + ", no action");
                }

                delta = m_gotoDelta [stateNumber];
                if (delta > 0)
                    foreach (SymbolToken p_SymbolToken in m_nonterminals)
                    {
                        RhsState p_RhsStateRef = p_RhsState.checkGoto (p_SymbolToken);
                        int index = p_SymbolToken.index;
                        if ((index + delta < 0) || (index + delta >= m_maxCheck) || (m_check [index + delta] != index))
                        {
                            if (p_RhsStateRef != null)
                            {
                                RhsProductionNode p_RhsProductionNode = null;
                                if ((!m_prodlist.TryGetValue (p_SymbolToken, out p_RhsProductionNode)) || (p_RhsProductionNode == null) || (m_defaultGoto [p_SymbolToken.index /*- m_minNonTerminalNr*/] != p_RhsProductionNode.BestGoto))
                                    Logger?.InfoRawLine ("state " + stateNumber + ", goto " + p_SymbolToken.name + " not handled");
                            }
                        }
                        else
                        {
                            if (p_RhsStateRef != null)
                            {
                                if (m_state [index + delta] != p_RhsStateRef.stateNumber)
                                {
                                    RhsProductionNode p_RhsProductionNode = null;
                                    if ((!m_prodlist.TryGetValue (p_SymbolToken, out p_RhsProductionNode)) || (p_RhsProductionNode == null) || (m_defaultGoto [p_SymbolToken.index /*- m_minNonTerminalNr*/] != p_RhsProductionNode.BestGoto))
                                        Logger?.InfoRawLine ("state " + stateNumber + ", goto " + p_SymbolToken.name + " to " + m_state [index + delta]);
                                }
                            }
                        }
                    }
                else
                {
                }

                if ((p_RhsState.reductionsSetSize > 0) && (m_reductions [stateNumber] == 0))
                    Logger?.InfoRawLine ("no reduction registered in state " + stateNumber);
            }
        }

        private void PrepareSyntaxTree ()
        {
            foreach (RhsProductionNode p_RhsProductionNode in m_prodlist.Values)
            {
                if (p_RhsProductionNode != null)
                    p_RhsProductionNode.PrepareASTData ();
            }
        }

        private void GenerateParseHeaders ()
        {
            return;
#pragma warning disable CS0162
            StreamWriter f = File.CreateText ("parse-syntax.h");

            f.WriteLine ($"{m_generatedHeaderText}");

            f.WriteLine ("#pragma once");
            f.WriteLine ("");
            foreach (SymbolToken p_SymbolToken in m_prodlist.Keys)
            {
                if (p_SymbolToken.name [0] == '$')
                    continue;
                f.WriteLine ("class\t" + p_SymbolToken.correctName + ";");
            }
            f.WriteLine ("");
            foreach (SymbolToken p_SymbolToken in m_prodlist.Keys)
            {
                if (p_SymbolToken.name [0] == '$')
                    continue;
                f.WriteLine ("#include \"" + p_SymbolToken.correctName + ".h\"");
            }
            f.WriteLine ();
            f.WriteLine ("enum	SyntaxTreeCallbackReason");
            f.WriteLine ("{");
            f.WriteLine ("\tBuilderCallbackReason = 1,");
            f.WriteLine ("\tTraversalPrologueCallbackReason = 2,");
            f.WriteLine ("\tTraversalMidTermCallbackReason = 3,");
            f.WriteLine ("\tTraversalEpilogueCallbackReason = 4");
            f.WriteLine ("};");
            f.Close ();
#pragma warning restore CS0162
        }

        private void GenerateTokenHeader ()
        {
            return;
        }

        private void GenerateScanner (ScannerPart scannerPart)
        {
            string scannerClassName = scannerPart.regexClassName;
            string fileName = OutputDir + scannerClassName + ".cs";
            try
            {
                Logger?.InfoRawLine ($"Generate scanner source file '{fileName}'");
                StreamWriter f = File.CreateText (fileName);
                f.WriteLine ($"{m_generatedHeaderText}");
                f.WriteLine (scannerPart.Generate (attributeCollection));
                f.Close ();
                SourceFileList.Add (fileName);
            }
            catch (Exception e)
            {
                Logger?.ErrorLine ($"Cannot create scanner source file {fileName}, exception thrown: {e.Message}");
            }
        }

        private void GenerateLexer (LexerPart lexerPart)
        {
            string lexerClassName = lexerPart.lexerClassName;
            string fileName = OutputDir + lexerClassName + ".cs";
            try
            {
                Logger?.InfoRawLine ($"Generate lexer source file '{fileName}'");
                StreamWriter f = File.CreateText (fileName);
                f.WriteLine ($"{m_generatedHeaderText}");
                f.WriteLine (lexerPart.Generate (attributeCollection));
                f.Close ();
                SourceFileList.Add (fileName);
            }
            catch (Exception e)
            {
                Logger?.ErrorLine ($"Cannot create lexer source file {fileName}, exception thrown: {e.Message}");
            }
        }

        private void GenerateSyntaxTreeClasses ()
        {
            foreach (KeyValuePair<SymbolToken, RhsProductionNode> prodit in m_prodlist)
            {
                SymbolToken p_SymbolToken = prodit.Key;
                RhsProductionNode p_RhsProductionNode = prodit.Value;

                if (p_SymbolToken.name [0] == '$')
                    continue;

                string srcName = OutputDir + p_SymbolToken.correctName + ".cs";
                try
                {
                    StreamWriter src = File.CreateText (srcName);

                    if (p_RhsProductionNode != null)
                    {
                        Logger?.InfoRawLine ($"Generate source file '{srcName}'");
                        src.WriteLine ($"{m_generatedHeaderText}");
                        src.WriteLine ($"using Anglr.Parser.Core;");
                        src.WriteLine ($"using Anglr.Parser.SyntaxTree;");
                        src.WriteLine ($"using Anglr.Parser.Walker;");
                        src.WriteLine ($"namespace {parserNameSpace}");
                        src.WriteLine ($"{{");
                        p_RhsProductionNode.GenerateCsHeaderFile (src);
                        p_RhsProductionNode.GenerateCsSourceFile (src);
                        src.WriteLine ($"}}");
                    }
                    src.Close ();
                    SourceFileList.Add (srcName);
                }
                catch (Exception e)
                {
                    Logger?.ErrorLine ($"Cannot create source file {srcName}, exception thrown: {e.Message}");
                }
            }
        }

        private void GenerateSemanticClasses ()
        {
            PrepareSyntaxTree ();
            GenerateParseHeaders ();
            GenerateSyntaxTreeClasses ();
        }

        private int ResizeTransitionTables (int size, int increment)
        {
            int newsize = size + increment;
            Array.Resize (ref m_check, newsize);
            Array.Resize (ref m_state, newsize);
            return newsize;
        }

        private int ResizeReductionTables (int size, int increment)
        {
            int newsize = size + increment;
            Array.Resize (ref m_rcheck, newsize);
            Array.Resize (ref m_rstate, newsize);
            return newsize;
        }

        private int ResizeGLRTables (int size, int increment)
        {
            int newsize = size + increment;
            Array.Resize (ref m_glrcheck, newsize);
            Array.Resize (ref m_glrstate, newsize);
            return newsize;
        }

        public IAnglrLogger Logger { get; private set; }
        public static bool DisplayProductionsFlag { get; set; } = true;
        public static bool DisplayStartSetsFlag { get; set; } = false;
        public static bool DisplayEndSetsFlag { get; set; } = false;

        private string m_generatedHeaderText = $"//{Environment.NewLine}//\tThis file was generated with ANGLR compiler{Environment.NewLine}//{Environment.NewLine}using System;{Environment.NewLine}";
        private _anglr_file_ m_anglrFile = null;

        private rhsstateset m_stateset = new rhsstateset ();
        private statearray m_statearray = new statearray ();

        private shiftset m_shiftset = new shiftset ();
        private gotoset m_gotoset = new gotoset ();

        public int magicNr { get; private set; }

        private int m_maxCheck = -1;
        private int m_maxRCheck = -1;
        private int m_maxGLRCheck = -1;

        private int [] m_terminalCodes = null;
        private int [] m_nonTerminalCodes = null;
        private int [] m_check = null;
        private int [] m_state = null;
        private int [] m_shiftDelta = null;
        private int [] m_gotoDelta = null;
        private int [] m_productionLengths = null;
        private int [] m_productionRules = null;
        private int [] m_defaultGoto = null;
        private int [] m_reductions = null;
        private int [] m_rcheck = null;
        private int [] m_rstate = null;
        private int [] m_glrcheck = null;
        private int [] m_glrstate = null;
        private int [] m_glrcells = null;

        private DirectoryInfo directoryInfo = null;
        public string OutputDir { get; private set; } = "";
        public ArrayList SourceFileList { get; private set; } = new ArrayList ();
        public ArrayList LibraryFileList { get; private set; } = new ArrayList ();

    }

    public partial class AnglrParserStatesGenerator : AnglrParserStatesBaseGenerator
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="p__anglr_file_fragment_"></param>
        /// <param name="className"></param>
        public AnglrParserStatesGenerator (anglrCompiler compiler) : base (compiler)
        {
            Logger = compiler?.AnglrLogger ?? new VoidAnglrLogger ();
            foreach (SyntaxTreeBase node in compiler.parseList)
            {
                if (node == null)
                    continue;
                m_anglrFileFragment = node.Clone () as _anglr_file_fragment_;
            }
            m_compiler = compiler;

            Common_Event += Invoke_Common_Callback;
            _anglr_file__Event += Invoke__anglr_file__Callback;
            _terminal_definition__Event += Invoke__terminal_definition__Callback;
            _regex_definition__Event += Invoke__regex_definition__Callback;
            _anglr_syntax_rule__Event += Invoke__anglr_syntax_rule__Callback;
            _anglr_syntax_production__Event += Invoke__anglr_syntax_production__Callback;
            _priority_specification__Event += Invoke__priority_specification__Callback;
            _associativity_specification__Event += Invoke__associativity_specification__Callback;
            _production_name__Event += Invoke__production_name__Callback;
            _name__Event += Invoke__name__Callback;
            _g_name__Event += Invoke__g_name__Callback;
            _marker__Event += Invoke__marker__Callback;
            _regular_expression_usage__Event += Invoke__regular_expression_usage__Callback;
            _general_part__Event += Invoke__general_part__Callback;
            _declaration_part__Event += Invoke__declaration_part__Callback;
            _scanner_part__Event += Invoke__scanner_part__Callback;
            _lexer_part__Event += Invoke__lexer_part__Callback;
            _parser_part__Event += Invoke__parser_part__Callback;

            generateParserEvent += CSharpGenerator_generateParserEvent;
            generateLexerEvent += CSharpGenerator_generateLexerEvent;
            generateScannerEvent += CSharpGenerator_generateScannerEvent;

            TraverseCommon (m_anglrFileFragment);
        }

        private void InitGenerator ()
        {
            m_stateset = new rhsstateset ();
            m_statearray = new statearray ();

            m_shiftset = new shiftset ();
            m_gotoset = new gotoset ();

            m_maxCheck = -1;
            m_maxRCheck = -1;
            m_maxGLRCheck = -1;

            m_terminalCodes = null;
            m_nonTerminalCodes = null;
            m_check = null;
            m_state = null;
            m_shiftDelta = null;
            m_gotoDelta = null;
            m_productionLengths = null;
            m_productionRules = null;
            m_defaultGoto = null;
            m_reductions = null;
            m_rcheck = null;
            m_rstate = null;
            m_glrcheck = null;
            m_glrstate = null;
            m_glrcells = null;
        }

        private void CSharpGenerator_generateScannerEvent (_scanner_part_ part)
        {
        }

        private void CSharpGenerator_generateLexerEvent (_lexer_part_ part)
        {
        }

        private void CSharpGenerator_generateParserEvent (_parser_part_ part)
        {
            InitGenerator ();
            if (anglrCompiler.createPrecedenceGrammar)
            {
                EliminateTautologies ();
                CheckIterators ();
                CheckCascades ();
            }
            int result = CheckProductions ();
            if (result != 0)
                ;
            else
            {
                MakeCanonicalRhsSet ();
                ComputeEpsilonConditions ();
                ComputeStartSets ();
                ComputeTransitionSets ();
                ComputeFollowSets ();
                PrepareTableGenerator ();
            }
            RhsProduction.Reset ();
        }

        /// <summary>
        /// 
        /// </summary>
        public void Dispose ()
        {
            foreach (RhsState p_RhsState in m_statearray)
                p_RhsState.Dispose ();
            m_statearray.Clear ();
            m_stateset.Clear ();
            foreach (RhsProductionNode p_RhsProductionNode in m_prodlist.Values)
                p_RhsProductionNode.Dispose ();
        }

        /// <summary>
        /// 
        /// </summary>
        public void GenerateCsCode (_anglr_syntax_rule_ syntax_Rule)
        {
            startSyntaxRule = syntax_Rule;
            Logger?.InfoRawLine ("Collect symbols");
            Traverse (m_anglrFileFragment);
        }

        public void GenerateParserStates ()
        {
            startSyntaxRule = m_compiler.startSyntaxRule;
            Traverse (m_anglrFileFragment);

            int counter = 0;
            do
            {
                counter = 0;
                foreach (RhsState rhsState in m_statearray)
                    counter += rhsState.ComputeConflicts ();
            }
            while (counter > 0);
        }

        public void ComputeConflicts ()
        {

        }

        public RhsState GetRhsState (int state) => m_statearray [state];

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private int CheckProductions ()
        {
            int result = 0;

            foreach (RhsProductionNode p_productionNode in m_prodlist.Values)
            {
                if (p_productionNode == null)
                    continue;
                SymbolToken p_productionName = p_productionNode.ProductionName;
                if (p_productionName.declarator != (uint) AnglrClassificationType.NonTerminalName)
                {
                    Logger?.WarnLine ("production name '" + p_productionName.name + "' must be non-terminal");
                    ++result;
                }
                int index = 1;
                foreach (RhsProduction p_production in p_productionNode.Productions)
                {
                    (m_proddict [p_production.productionNumber] = p_production).index = index++;
                    foreach (RhsNode p_node in p_production.rhsNodes)
                    {
                        SymbolToken p_nodeSymbol = p_node.symbolToken;
                        uint declarator = p_nodeSymbol.declarator;
                        if (!((declarator == (uint) AnglrClassificationType.TerminalName) || (declarator == (uint) AnglrClassificationType.NonTerminalName)))
                        {
                            Logger?.WarnLine ("rhs symbol name '" + p_nodeSymbol.name + "' must be terminal (token) or non-terminal");
                            ++result;
                            continue;
                        }
                        if (declarator == (uint) AnglrClassificationType.NonTerminalName)
                        {
                            if (!m_prodlist.Keys.Contains (p_nodeSymbol))
                            {
                                Logger?.WarnLine ("symbol '" + p_nodeSymbol.name + "' is used in production rules, but is not defined as terminal or non-terminal");
                                ++result;
                            }
                        }
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        private void DisplayProductions ()
        {
            if (!CSharpGenerator.DisplayProductionsFlag)
                return;
            Logger?.InfoRawLine ("INFO: CANONICAL PRODUCTIONS");
            Logger?.InfoRawLine ("");
            foreach (RhsProductionNode p_productionNode in m_prodlist.Values)
            {
                Logger?.InfoRawLine<RhsProductionNode>
                (
                    (productionNode) =>
                    {
                        StringBuilder sb = new StringBuilder ();
                        SymbolToken p_productionName = productionNode.ProductionName;
                        sb.AppendLine (p_productionName.name);
                        int i = -1;
                        foreach (RhsProduction p_production in productionNode.Productions)
                        {
                            ++i;
                            sb.Append ("\t");
                            if (i != 0)
                                sb.Append ("|\t");
                            else
                                sb.Append (":\t");
                            if (p_production.rhsNodes.Count > 0)
                                foreach (RhsNode p_node in p_production.rhsNodes)
                                {
                                    SymbolToken p_nodeSymbol = p_node.symbolToken;
                                    if (p_nodeSymbol.alias != null)
                                        p_nodeSymbol = p_nodeSymbol.alias;
                                    sb.Append (p_nodeSymbol.name + " ");
                                }
                            else
                                sb.Append ("%empty");
                            if (p_production.priority > 0)
                            {
                                sb.Append ($" {p_production.priority}");
                                if (p_production.associativity != ProductionAssociativity.None)
                                    sb.Append ($" {p_production.associativity}");
                            }
                            else
                            {
                                if (p_production.associativity != ProductionAssociativity.None)
                                    sb.Append ($" {p_production.associativity}");
                            }
                            sb.AppendLine ();
                        }
                        sb.AppendLine ("\t;");
                        sb.AppendLine ();
                        return sb.ToString ();
                    },
                    p_productionNode
                );
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="prodNode"></param>
        /// <param name="stack"></param>
        /// <param name="stackIndex"></param>
        /// <param name="displayDepth"></param>
        private void DisplayProductionsHierarchy (RhsProductionNode prodNode, SymbolToken [] stack, int stackIndex, int displayDepth)
        {
            if (displayDepth == 0)
            {
                Logger?.InfoRawLine ("INFO: HIERARCHICALY VIEW OF PRODUCTIONS");
                Logger?.InfoRawLine ("");
            }

            int stackTop = stackIndex;
            SymbolToken nodeSymbol = (prodNode != null) ? prodNode.ProductionName : null;
            if (nodeSymbol == null)
                return;
            if (nodeSymbol.displayed)
                return;
            nodeSymbol.displayed = true;
            stack [stackTop++] = nodeSymbol;
            Logger?.InfoRawLine<int>
            (
                (depth) =>
                {
                    StringBuilder sb = new StringBuilder ();
                    for (int i = 0; i < depth; ++i)
                        sb.Append ("\t");
                    sb.AppendLine ($"{nodeSymbol.name}");
                    return sb.ToString ();
                },
                displayDepth
            );
            Logger?.InfoRawLine<int>
            (
                (depth) =>
                {
                    StringBuilder sb = new StringBuilder ();
                    for (int i = 0; i < depth; ++i)
                        sb.Append ("\t");
                    sb.AppendLine ("{");
                    return sb.ToString ();
                },
                displayDepth
            );
            if (prodNode != null)
                foreach (RhsProduction p_RhsProduction in prodNode.Productions)
                {
                    foreach (RhsNode p_RhsNode in p_RhsProduction.rhsNodes)
                    {
                        SymbolToken p_SymbolToken = p_RhsNode.symbolToken;
                        int index;
                        for (index = stackIndex; (index < stackTop) && (p_SymbolToken.name != stack [index].name); ++index)
                            ;
                        if (index < stackTop)
                            continue;
                        stack [stackTop++] = p_SymbolToken;
                        bool isToken = false;
                        if ((isToken = (p_SymbolToken.declarator == (uint) AnglrClassificationType.TerminalName)) || (p_SymbolToken.displayed == true))
                        {
                            Logger?.InfoRawLine<int>
                            (
                                (depth) =>
                                {
                                    StringBuilder sb = new StringBuilder ();
                                    for (int i = 0; i < depth; ++i)
                                        sb.Append ("\t");
                                    string sign = isToken ? "T" : "N";
                                    sb.AppendLine ($"{p_SymbolToken.name} {sign}");
                                    return sb.ToString ();
                                },
                                displayDepth
                            );
                            continue;
                        }
                        if (m_prodlist.TryGetValue (p_SymbolToken, out _))
                            if (p_SymbolToken.declarator == (uint) AnglrClassificationType.NonTerminalName)
                                DisplayProductionsHierarchy (m_prodlist [p_SymbolToken], stack, stackTop, displayDepth + 1);
                    }
                    Logger?.InfoRawLine ("");
                }
            Logger?.InfoRawLine<int>
            (
                (depth) =>
                {
                    StringBuilder sb = new StringBuilder ();
                    for (int i = 0; i < depth; ++i)
                        sb.Append ("\t");
                    sb.AppendLine ("}");
                    return sb.ToString ();
                },
                displayDepth
            );
        }

        private void DisplayProduction (StringBuilder sb, RhsProductionNode node, int depth, int round)
        {
            string name = node.ProductionName.name;

            if (node.round != round)
            {
                bool first = true;
                node.round = round;
                sb.AppendLine ();
                for (int i = 0; i < depth; ++i)
                    sb.Append ("\t");
                sb.AppendLine ($"( : {name} :");
                foreach (RhsProduction p_RhsProduction in node.Productions)
                {
                    for (int i = 0; i < depth; ++i)
                        sb.Append ("\t");
                    if (first)
                        sb.Append ($"\t");
                    else
                        sb.Append ($"|\t");
                    first = false;
                    if (p_RhsProduction.rhsNodes.Count > 0)
                        foreach (RhsNode p_RhsNode in p_RhsProduction.rhsNodes)
                        {
                            SymbolToken symbol = p_RhsNode.symbolToken;
                            if ((symbol.declarator == (uint) AnglrClassificationType.TerminalName) || !m_prodlist.TryGetValue (symbol, out _))
                                sb.Append ($"{symbol.name} ");
                            else
                            {
                                DisplayProduction (sb, m_prodlist [symbol], depth + 1, round);
                            }
                        }
                    else
                        sb.Append ("%empty");
                    sb.AppendLine ();
                }
                for (int i = 0; i < depth; ++i)
                    sb.Append ("\t");
                sb.AppendLine ($")");
            }
            else
            {
                sb.Append ($"{name} ");
            }
        }

        private void EliminateTautologies ()
        {
            foreach (RhsProductionNode rhsProductionNode in m_prodlist.Values)
                rhsProductionNode.EliminateTautologies ();
        }

        private void CheckIterators ()
        {
            foreach (RhsProductionNode rhsProductionNode in m_prodlist.Values)
                rhsProductionNode.CheckIterator ();
        }

        private void CheckCascades ()
        {
            foreach (RhsProductionNode rhsProductionNode in m_prodlist.Values)
            {
                (bool identity, SymbolToken symbol) result = rhsProductionNode.CheckCascade ();
                if (result.symbol == null)
                    continue;
                if (result.symbol.declarator != (uint) AnglrClassificationType.NonTerminalName)
                    continue;
                if (result.identity)
                {
                    //Console.WriteLine ($"IDENTITY: {result.symbol.name} -- {rhsProductionNode.ProductionName.name}");
                    //rhsProductionNode.IdentityNode = m_prodlist [result.symbol];
                    //result.symbol.IdentityFlag = true;
                    //m_cascades.Remove (result.symbol);
                    //m_cascades [rhsProductionNode.ProductionName] = rhsProductionNode;
                }
                else
                {
                    Logger?.InfoRawLine ($"CASCADE : {result.symbol.name} -- {rhsProductionNode.ProductionName.name}");
                    rhsProductionNode.CascadeNode = m_prodlist [result.symbol];
                    result.symbol.CascadeFlag = true;
                    m_cascades.Remove (result.symbol);
                    m_cascades [rhsProductionNode.ProductionName] = rhsProductionNode;
                }
            }
            Logger?.InfoRawLine ("CASCADES:");
            foreach (RhsProductionNode productionNode in m_cascades.Values)
            {
                if (productionNode.ProductionName.CascadeFlag || productionNode.ProductionName.IdentityFlag)
                {
                    Logger?.InfoRawLine ($"\t({productionNode.ProductionName.name})");
                    continue;
                }
                Logger?.InfoRawLine<object>
                (
                    (_) =>
                    {
                        StringBuilder sb = new StringBuilder ();
                        sb.Append ("\t");
                        for (RhsProductionNode rhsProduction = productionNode; rhsProduction != null;)
                        {
                            sb.Append ($" {rhsProduction.ProductionName.name}");
                            if (rhsProduction.CascadeNode != null)
                                rhsProduction = rhsProduction.CascadeNode;
                            else if (rhsProduction.IdentityNode != null)
                                rhsProduction = rhsProduction.IdentityNode;
                            else
                                rhsProduction = null;
                        }
                        sb.AppendLine ();
                        return sb.ToString ();
                    },
                    null
                );
            }
            Logger?.InfoRawLine ("GENERATED CASCADE RULES:");
            foreach (KeyValuePair<SymbolToken, RhsProductionNode> pair in GenerateCascadeRules ())
            {
                Logger?.InfoRawLine ($"\t{pair.Key.name}");
                //m_prodlist [pair.Key] = pair.Value;
            }
        }

        private prodlist GenerateCascadeRules ()
        {
            prodlist productions = new prodlist ();
            //return productions;
            foreach (RhsProductionNode rhsProductionNode in m_cascades.Values)
            {
                if (rhsProductionNode.ProductionName.CascadeFlag || rhsProductionNode.ProductionName.IdentityFlag)
                    continue;
                Logger?.InfoRawLine ($"\t{rhsProductionNode.ProductionName.name}:");
                productions rhsProductions = /*rhsProductionNode.Productions = */ rhsProductionNode.GenerateCascade ();
                foreach (RhsProduction rhsProduction in rhsProductions)
                {
                    Logger?.InfoRawLine<RhsProduction>
                    (
                        (production) =>
                        {
                            StringBuilder sb = new StringBuilder ();
                            sb.Append ("\t\t");
                            foreach (RhsNode rhsNode in production.rhsNodes)
                                sb.Append ($"{rhsNode.symbolToken.name} ");
                            if (production.priority > 0)
                            {
                                sb.Append ($"{production.priority}");
                                if (production.associativity != ProductionAssociativity.None)
                                    sb.Append ($" {production.associativity}");
                            }
                            else
                            {
                                if (production.associativity != ProductionAssociativity.None)
                                    sb.Append ($"{production.associativity}");
                            }
                            sb.AppendLine ();
                            return sb.ToString ();
                        },
                        rhsProduction
                    );
                }
            }
            return productions;
        }

        private void ResolveIdentities ()
        {
            foreach (RhsProductionNode rhsProductionNode in m_prodlist.Values)
            {
                RhsProductionNode identityNode;
                for (identityNode = rhsProductionNode.IdentityNode; (identityNode != null) && (identityNode.IdentityNode != null); identityNode = identityNode.IdentityNode)
                    ;
                if (identityNode == null)
                    continue;
                ResolveIdentityNode (rhsProductionNode, identityNode);
            }
        }

        private void ResolveIdentityNode (RhsProductionNode rhsProductionNode, RhsProductionNode identityNode)
        {
            string rhsNodeName = rhsProductionNode.ProductionName.name;
            SymbolToken identityNodeName = identityNode.ProductionName;
            foreach (RhsProductionNode productionNode in m_prodlist.Values)
            {
                foreach (RhsProduction rhsProduction in productionNode.Productions)
                {
                    foreach (RhsNode rhsNode in rhsProduction.rhsNodes)
                    {
                        if (rhsNode.symbolToken.name == rhsNodeName)
                            rhsNode.symbolToken = identityNodeName;
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void MakeCanonicalRhsSet ()
        {
            int stateNumber = 0;
            int low = 0;
            int high = 0;

            if (!m_stateset.Keys.Contains (m_firstRhsState))
            {
                m_firstRhsState.stateNumber = stateNumber++;
                m_stateset [m_firstRhsState] = m_firstRhsState;
                m_statearray.Add (m_firstRhsState);
                ++high;
            }
            else
            {
                Logger?.ErrorLine ("internal error in MakeCanonicalRhsSet()");
                return;
            }
            while (low < high)
            {
                RhsState p_RhsState = m_statearray [low++];
                p_RhsState.makeClosure ();
                p_RhsState.makeStates (Logger);
                int count = p_RhsState.ShiftArray.Count;
                for (int index = 0; index < count; ++index)
                {
                    RhsState p_state = p_RhsState.ShiftArray [index];
                    RhsState p_RhsStateRef = null;
                    if (!m_stateset.TryGetValue (p_state, out p_RhsStateRef))
                    {
                        p_state.stateNumber = stateNumber++;
                        m_stateset [p_state] = p_state;
                        m_statearray.Add (p_state);
                        ++high;
                    }
                    else
                    {
                        p_RhsState.changeShiftState (p_RhsStateRef, index);
                        p_state.Dispose ();
                    }
                }
                count = p_RhsState.GotoArray.Count;
                for (int index = 0; index < count; ++index)
                {
                    RhsState p_state = p_RhsState.GotoArray [index];
                    RhsState p_RhsStateRef = null;
                    if (!m_stateset.TryGetValue (p_state, out p_RhsStateRef))
                    {
                        p_state.stateNumber = stateNumber++;
                        m_stateset [p_state] = p_state;
                        m_statearray.Add (p_state);
                        ++high;
                    }
                    else
                    {
                        p_RhsState.changeGototState (p_RhsStateRef, index);
                        p_state.Dispose ();
                    }
                }
            }
            foreach (RhsProductionNode p_RhsProductionNode in m_prodlist.Values)
            {
                if (!p_RhsProductionNode.Used)
                    Logger?.WarnLine ("non-terminal symbol '" + p_RhsProductionNode.ProductionName.name + "' is not used");
            }

            //{
            //	Stream stream = File.Create("stateset.bin");
            //	BinaryFormatter binaryFormatter = new BinaryFormatter();
            //	binaryFormatter.Serialize (stream, this);
            //	stream.Close ();
            //}
        }

        /// <summary>
        /// 
        /// </summary>
        private void ComputeEpsilonConditions ()
        {
            while (true)
            {
                int count = 0;
                foreach (RhsProductionNode p_RhsProductionNode in m_prodlist.Values)
                {
                    foreach (RhsProduction p_RhsProduction in p_RhsProductionNode.Productions)
                    {
                        bool brokenLoop = false;
                        foreach (RhsNode p_RhsNode in p_RhsProduction.rhsNodes)
                        {
                            RhsProductionNode p_RhsProductionNodeRef = null;
                            SymbolToken p_SymbolToken = p_RhsNode.symbolToken;
                            if (!m_prodlist.TryGetValue (p_SymbolToken, out p_RhsProductionNodeRef))
                            {
                                brokenLoop = true;
                                break;
                            }
                            if (!p_RhsProductionNodeRef.EpsilonCondition)
                            {
                                brokenLoop = true;
                                break;
                            }
                        }
                        if (!brokenLoop)
                        {
                            if (!p_RhsProductionNode.EpsilonCondition)
                            {
                                p_RhsProductionNode.EpsilonCondition = true;
                                ++count;
                            }
                            break;
                        }
                    }
                }
                if (count == 0)
                    break;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void ComputeStartSets ()
        {
            while (true)
            {
                int count = 0;
                foreach (RhsProductionNode p_RhsProductionNode in m_prodlist.Values)
                {
                    foreach (RhsProduction p_RhsProduction in p_RhsProductionNode.Productions)
                    {
                        foreach (RhsNode p_RhsNode in p_RhsProduction.rhsNodes)
                        {
                            SymbolToken p_SymbolToken = p_RhsNode.symbolToken;
                            if (p_SymbolToken.declarator == (uint) AnglrClassificationType.TerminalName)
                            {
                                if (p_RhsProductionNode.insertStartElement (p_SymbolToken))
                                    ++count;
                                break;
                            }
                            if (p_SymbolToken.declarator == (uint) AnglrClassificationType.NonTerminalName)
                            {
                                RhsProductionNode p_RhsProductionNodeRef = null;
                                if (!m_prodlist.TryGetValue (p_SymbolToken, out p_RhsProductionNodeRef))
                                    break;
                                if (p_RhsProductionNode.insertStartSet (p_RhsProductionNodeRef.StartSet))
                                    ++count;
                                if (!p_RhsProductionNodeRef.EpsilonCondition)
                                    break;
                            }
                        }
                    }
                }
                if (count == 0)
                    break;
            }
            if (!CSharpGenerator.DisplayStartSetsFlag)
                return;
            Logger?.InfoRawLine ("Start sets:");
            foreach (KeyValuePair<SymbolToken, RhsProductionNode> keyval in m_prodlist)
            {
                Logger?.InfoRawLine (keyval.Key.name + ":");
                Logger?.InfoRawLine<object>
                (
                    (_) =>
                    {
                        StringBuilder sb = new StringBuilder ();
                        sb.Append ("{");
                        keyval.Value.StartSet.display (sb);
                        sb.AppendLine ("}");
                        return sb.ToString ();
                    },
                    null
                );
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void ComputeTransitionSets ()
        {
            int count = 0;
            while (true)
            {
                count = 0;
                foreach (RhsProductionNode p_RhsProductionNode in m_prodlist.Values)
                {
                    if (p_RhsProductionNode == null)
                        continue;
                    foreach (RhsProduction p_RhsProduction in p_RhsProductionNode.Productions)
                        count += ComputeTransitionSets (p_RhsProduction, new rhsenumerator (p_RhsProduction.rhsNodes));
                }
                if (count == 0)
                    break;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="p_RhsProduction"></param>
        /// <param name="rhsit"></param>
        /// <returns></returns>
        private int ComputeTransitionSets (RhsProduction p_RhsProduction, rhsenumerator rhsit)
        {
            int count = 0;

            if (rhsit.atEnd)
                return count;

            rhsenumerator rhsinc = new rhsenumerator (rhsit);
            count += ComputeTransitionSets (p_RhsProduction, rhsinc);

            RhsNode p_RhsNode = rhsit.currentRhsNode;
            if (rhsinc.atEnd)
            {
                p_RhsNode.opened = true;
                return count;
            }

            RhsNode p_RhsNodeInc = rhsinc.currentRhsNode;
            SymbolToken p_SymbolTokenInc = p_RhsNodeInc.symbolToken;
            if (p_SymbolTokenInc.declarator == (uint) AnglrClassificationType.TerminalName)
            {
                count += p_RhsNode.insertTransitionSet (p_SymbolTokenInc) ? 1 : 0;
                return count;
            }
            RhsProductionNode p_RhsProductionNodeInc = null;
            if (!m_prodlist.TryGetValue (p_SymbolTokenInc, out p_RhsProductionNodeInc))
                return count;
            if (p_RhsProductionNodeInc == null)
                return count;
            count += p_RhsNode.unionTransitionSet (p_RhsProductionNodeInc.StartSet) ? 1 : 0;
            if (p_RhsProductionNodeInc.EpsilonCondition)
            {
                count += p_RhsNode.unionTransitionSet (p_RhsNodeInc.transitionSet) ? 1 : 0;
                p_RhsNode.opened = p_RhsNodeInc.opened;
            }
            return count;
        }

        /// <summary>
        /// 
        /// </summary>
        private void ComputeFollowSets ()
        {
            int count;
            do
            {
                count = 0;
                foreach (RhsState p_rhsState in m_stateset.Values)
                    count += p_rhsState.computeFollowSets (Logger);
            }
            while (count > 0);
        }

        /// <summary>
        /// 
        /// </summary>
        private void PrepareTableGenerator ()
        {
            // mark most frequent goto transitions
            foreach (RhsState p_RhsState in m_stateset.Values)
                p_RhsState.markGotoCounters ();

            if (false)
            {
                int gotoCount = 0;
                foreach (KeyValuePair<SymbolToken, RhsProductionNode> keyval in m_prodlist)
                    if (keyval.Value != null)
                        keyval.Value.displayGotoCnt (Logger);
                Logger?.InfoRawLine ("OPTIMIZED GOTO = " + gotoCount);
            }

            // create shift set, goto set collections, mark default reduction rules
            foreach (RhsState p_RhsState in m_stateset.Values)
            {
                p_RhsState.addToShiftSet (m_shiftset);
                p_RhsState.addToGotoSet (m_gotoset);
                p_RhsState.markDefaultReduction ();
                p_RhsState.checkGLRCondition ();
                p_RhsState.addToShiftSet (m_shiftset);
            }

            foreach (RhsProductionNode p_RhsProductionNode in m_prodlist.Values)
                if (p_RhsProductionNode != null)
                    p_RhsProductionNode.defineBestGotoCounter ();

            magicNr = 0;
            foreach (RhsState rhsState in m_stateset.Values)
            {
                foreach (RhsConfiguration rhsConfiguration in rhsState.m_core.Values)
                {
                    magicNr += rhsConfiguration.rhsIterator.position;
                    string name = rhsConfiguration.rhsProduction.symbolToken.name;
                    foreach (char c in name)
                        magicNr += c;
                    foreach (RhsNode rhsNode in rhsConfiguration.rhsProduction.rhsNodes)
                    {
                        name = rhsNode.symbolToken.name;
                        foreach (char c in name)
                            magicNr += c;
                    }
                }
            }

            //foreach (statedelta p_statedelta in m_shiftset.Values)
            //{
            //	stateset p_set = p_statedelta.first;
            //	stateSetInfo p_info = p_statedelta.second;
            //	int minToken = -1;
            //	int maxToken = -1;
            //	foreach (KeyValuePair<SymbolToken, RhsState> keyval in p_set)
            //	{
            //		SymbolToken p_SymbolToken = keyval.Key;
            //		int index = p_SymbolToken.index;
            //		if ((minToken < 0) || (minToken > index))
            //			minToken = index;
            //		if (maxToken < index)
            //			maxToken = index;
            //	}
            //	p_info.m_min = minToken;
            //	p_info.m_max = maxToken;
            //}

            //foreach (statedelta p_statedelta in m_gotoset.Values)
            //{
            //	stateset p_set = p_statedelta.first;
            //	stateSetInfo p_info = p_statedelta.second;
            //	int minToken = -1;
            //	int maxToken = -1;
            //	foreach (KeyValuePair<SymbolToken, RhsState> keyval in p_set)
            //	{
            //		SymbolToken p_SymbolToken = keyval.Key;
            //		RhsState p_RhsState = keyval.Value;
            //		RhsProductionNode p_RhsProductionNode = m_prodlist [p_SymbolToken];
            //		if (p_RhsProductionNode == null)
            //			continue;
            //		if (p_RhsState.stateNumber == p_RhsProductionNode.bestGoto)
            //			continue;
            //		int index = p_SymbolToken.index;
            //		if ((minToken < 0) || (minToken > index))
            //			minToken = index;
            //		if (maxToken < index)
            //			maxToken = index;
            //	}
            //	p_info.m_min = minToken;
            //	p_info.m_max = maxToken;
            //}
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="f"></param>
        private void GenerateTables (TextWriter f)
        {
            GenerateTerminalTables (f);
            GenerateNonTerminalTables (f);
            GenerateGLRTables (f);
            GenerateTransitionTables (f);
            GenerateShiftDeltas (f);
            GenerateGotoDeltas (f);
            GenerateProductionTables (f);
            GenerateReductionTables (f);
            if (true)
                CheckPDA ();
            GenerateSemanticClasses ();
        }

        private void GenerateProdIds (TextWriter f)
        {
            foreach (SymbolToken p_SymbolToken in m_prodlist.Keys)
            {
                if (p_SymbolToken.name [0] == '$')
                    continue;
                string productionName = p_SymbolToken.correctName;
                f.WriteLine ("\t\t_" + productionName + "_ID = " + p_SymbolToken.index + ",");
            }
        }

        public void GenerateFragmenmts (TextWriter f)
        {
            Dictionary<SymbolToken, RhsState> fragments = new Dictionary<SymbolToken, RhsState> ();
            foreach (RhsState p_state in m_statearray)
            {
                int count = p_state.closure.Count;
                foreach (SymbolToken symbolToken in p_state.closure.Keys)
                {
                    if (fragments.Keys.Contains (symbolToken))
                    {
                        RhsState rhsState = fragments [symbolToken];
                        if (rhsState.closure.Count <= count)
                            continue;
                    }
                    fragments [symbolToken] = p_state;
                }
            }
            foreach (KeyValuePair<SymbolToken, RhsState> fragment in fragments)
                f.WriteLine ($"\t\t\t(\"{fragment.Key.name}\", ProductionID._{fragment.Key.correctName}_ID, {fragment.Value.stateNumber}),");
        }

        public int GenerateViablePrefixes (Dictionary<int, List<string>> viablePrefixes)
        {
            int counter = 0;
            foreach (RhsState rhsState in m_statearray)
            {
                List<string> viables = viablePrefixes [rhsState.m_stateNumber];
                foreach (RhsState shiftState in rhsState.ShiftArray)
                {
                    if (shiftState.m_stateNumber == rhsState.m_stateNumber)
                        continue;
                    List<string> shiftViables = null;
                    if (!viablePrefixes.TryGetValue (shiftState.m_stateNumber, out shiftViables))
                        viablePrefixes [shiftState.m_stateNumber] = shiftViables = new List<string> ();
                    foreach (string viable in viables)
                    {
                        shiftViables.Add ($"{viable}.{shiftState.symbolToken.name}");
                        ++counter;
                    }
                }
                foreach (RhsState gotoState in rhsState.GotoArray)
                {
                    if (gotoState.m_stateNumber == rhsState.m_stateNumber)
                        continue;
                    List<string> gotoViables = null;
                    if (!viablePrefixes.TryGetValue (gotoState.m_stateNumber, out gotoViables))
                        viablePrefixes [gotoState.m_stateNumber] = gotoViables = new List<string> ();
                    foreach (string viable in viables)
                    {
                        gotoViables.Add ($"{viable}.{gotoState.symbolToken.name}");
                        ++counter;
                    }
                }
            }
            return counter;
        }

        private void GenerateTerminalTables (TextWriter f)
        {
            if (false)
            {
                Logger?.InfoRawLine ("INFO: TERMINALS");
                Logger?.InfoRawLine ("");

                foreach (SymbolToken p_SymbolToken in m_terminals)
                {
                    Logger?.InfoRawLine ("#define\t" + p_SymbolToken.correctName + "\t" + p_SymbolToken.index);
                }
                Logger?.InfoRawLine ("");

                Logger?.InfoRawLine ("#define\tminTerminalCode\t" + m_minTerminalNr);
                Logger?.InfoRawLine ("#define\tmaxTerminalCode\t" + m_maxTerminalNr);
                Logger?.InfoRawLine ("");
            }

            m_terminalCodes = new int [m_terminals.Count];
            int i = 0;
            f.WriteLine ($"\t\tinternal readonly static int g_minTerminalCode = {m_minTerminalNr};");
            f.WriteLine ("\t\tinternal readonly static int[] g_terminalCodes = ");
            f.WriteLine ("\t\t{");
            foreach (SymbolToken p_SymbolToken in m_terminals)
            {
                if ((i % 10) == 0)
                    f.Write ("\t\t\t");
                f.Write ("{0,6:d}, ", (m_terminalCodes [i] = p_SymbolToken.index));
                if ((++i % 10) == 0)
                    f.WriteLine ();
            }
            if ((i % 10) != 0)
                f.WriteLine ();
            f.WriteLine ("\t\t};");
            f.WriteLine ();

            f.WriteLine ("\t\tinternal readonly static string[] g_terminalNames = ");
            f.WriteLine ("\t\t{");
            foreach (SymbolToken p_SymbolToken in m_terminals)
            {
                f.WriteLine ($"\t\t\t\"{p_SymbolToken.name}\",");
            }
            f.WriteLine ("\t\t};");
            f.WriteLine ();
        }

        private void GenerateNonTerminalTables (TextWriter f)
        {
            if (false)
            {
                Logger?.InfoRawLine ("INFO: NONTERMINALS");
                Logger?.InfoRawLine ("");

                foreach (SymbolToken p_SymbolToken in m_nonterminals)
                {
                    Logger?.InfoRawLine ("#define\t" + p_SymbolToken.correctName + "\t" + p_SymbolToken.index);
                }
                Logger?.InfoRawLine ("");

                Logger?.InfoRawLine ("#define\tminNonTerminalCode\t" + m_minNonTerminalNr);
                Logger?.InfoRawLine ("#define\tmaxNonTerminalCode\t" + m_maxNonTerminalNr);
                Logger?.InfoRawLine ("");
            }

            m_nonTerminalCodes = new int [m_nonterminals.Count];
            int i = 0;
            f.WriteLine ($"\t\tinternal readonly static int g_minNonTerminalCode = {m_minNonTerminalNr};");
            f.WriteLine ("\t\tinternal readonly static int[] g_nonTerminalCodes = ");
            f.WriteLine ("\t\t{");
            foreach (SymbolToken p_SymbolToken in m_nonterminals)
            {
                if ((i % 10) == 0)
                    f.Write ("\t\t\t");
                f.Write ("{0,6:d}, ", (m_nonTerminalCodes [i] = p_SymbolToken.index));
                if ((++i % 10) == 0)
                    f.WriteLine ();
            }
            if ((i % 10) != 0)
                f.WriteLine ();
            f.WriteLine ("\t\t};");
            f.WriteLine ();

            f.WriteLine ("\t\tinternal readonly static string[] g_nonTerminalNames = ");
            f.WriteLine ("\t\t{");
            foreach (SymbolToken p_SymbolToken in m_nonterminals)
            {
                f.WriteLine ($"\t\t\t\"{p_SymbolToken.name}\",");
            }
            f.WriteLine ("\t\t};");
            f.WriteLine ();
        }

        private void GenerateTransitionTables (TextWriter f)
        {
            HashSet<int> deltaset = new HashSet<int> ();
            int i;
            int size = 1000;
            m_check = new int [size];
            m_state = new int [size];
            m_maxCheck = -1;

            foreach (statedelta shiftit in m_shiftset.Values)
            {
                stateset p_shiftset = shiftit.first;
                int delta = 1;
                while (true)
                {
                    while (deltaset.Contains (delta))
                        ++delta;

                    bool brokenLoop = false;
                    foreach (SymbolToken p_SymbolToken in p_shiftset.Keys)
                    {
                        int index = delta + p_SymbolToken.index;
                        if (index > m_maxCheck)
                            m_maxCheck = index;
                        if (index >= size)
                            size = ResizeTransitionTables (size, 1000);
                        if (m_check [index] != 0)
                        {
                            brokenLoop = true;
                            break;
                        }
                    }
                    if (!brokenLoop)
                        break;
                    ++delta;
                }
                deltaset.Add (shiftit.second.m_delta = delta);
                foreach (KeyValuePair<SymbolToken, RhsState> stateit in p_shiftset)
                {
                    SymbolToken p_SymbolToken = stateit.Key;
                    int index = delta + p_SymbolToken.index;
                    if (m_check [index] != 0)
                        Logger?.WarnLine ("overwrite fiefd");
                    m_check [index] = index - delta;
                    m_state [index] = stateit.Value.stateNumber;
                }
            }

            foreach (statedelta gotoit in m_gotoset.Values)
            {
                stateset p_gotoset = gotoit.first;
                int delta = 1;
                while (true)
                {
                    while (deltaset.Contains (delta))
                        ++delta;

                    bool brokenLoop = false;
                    foreach (KeyValuePair<SymbolToken, RhsState> stateit in p_gotoset)
                    {
                        SymbolToken p_SymbolToken = stateit.Key;
                        RhsState p_RhsState = stateit.Value;
                        RhsProductionNode p_RhsProductionNode = m_prodlist [p_SymbolToken];
                        if (p_RhsProductionNode == null)
                            continue;
                        if (p_RhsState.stateNumber == p_RhsProductionNode.BestGoto)
                            continue;
                        int index = delta + p_SymbolToken.index;
                        if (index > m_maxCheck)
                            m_maxCheck = index;
                        if (index >= size)
                            size = ResizeTransitionTables (size, 1000);
                        if (m_check [index] != 0)
                        {
                            brokenLoop = true;
                            break;
                        }
                    }
                    if (!brokenLoop)
                        break;
                    ++delta;
                }
                deltaset.Add (gotoit.second.m_delta = delta);
                foreach (KeyValuePair<SymbolToken, RhsState> stateit in p_gotoset)
                {
                    SymbolToken p_SymbolToken = stateit.Key;
                    RhsState p_RhsState = stateit.Value;
                    RhsProductionNode p_RhsProductionNode = m_prodlist [p_SymbolToken];
                    if (p_RhsProductionNode == null)
                        continue;
                    if (p_RhsState.stateNumber == p_RhsProductionNode.BestGoto)
                        continue;
                    int index = delta + p_SymbolToken.index;
                    if (m_check [index] != 0)
                        Logger?.WarnLine ("overwrite fiefd");
                    m_check [index] = index - delta;
                    m_state [index] = p_RhsState.stateNumber /*- m_minNonTerminalNr*/;
                }
            }

            ++m_maxCheck;

            f.WriteLine ("\t\tinternal readonly static int[] g_check =");
            f.WriteLine ("\t\t{");
            for (i = 0; i < m_maxCheck;)
            {
                if ((i % 10) == 0)
                    f.Write ("\t\t\t");
                f.Write ("{0,6:d}, ", m_check [i]);
                if ((++i % 10) == 0)
                    f.WriteLine ();
            }
            if ((i % 10) != 0)
                f.WriteLine ();
            f.WriteLine ("\t\t};");
            f.WriteLine ();

            f.WriteLine ("\t\tinternal readonly static int[] g_state =");
            f.WriteLine ("\t\t{");
            for (i = 0; i < m_maxCheck;)
            {
                if ((i % 10) == 0)
                    f.Write ("\t\t\t");
                f.Write ("{0,6:d}, ", m_state [i]);
                if ((++i % 10) == 0)
                    f.WriteLine ();
            }
            if ((i % 10) != 0)
                f.WriteLine ();
            f.WriteLine ("\t\t};");
            f.WriteLine ();
        }

        private void GenerateShiftDeltas (TextWriter f)
        {
            int size = m_stateset.Count;
            m_shiftDelta = new int [size];
            foreach (RhsState p_RhsState in m_stateset.Values)
            {
                int val = 0;
                statedelta sd = p_RhsState.findShiftSet (m_shiftset);
                if (sd.first.Count > 0)
                    val = sd.second.m_delta;
                m_shiftDelta [p_RhsState.stateNumber] = p_RhsState.hasGLRCondition ? -p_RhsState.GLRDelta : val;
            }

            int i;
            f.WriteLine ("\t\tinternal readonly static int[] g_shiftDelta =");
            f.WriteLine ("\t\t{");
            for (i = 0; i < size;)
            {
                if ((i % 10) == 0)
                    f.Write ("\t\t\t");
                f.Write ("{0,6:d}, ", m_shiftDelta [i]);
                if ((++i % 10) == 0)
                    f.WriteLine ();
            }
            if ((i % 10) != 0)
                f.WriteLine ();
            f.WriteLine ("\t\t};");
            f.WriteLine ();
        }

        private void GenerateGotoDeltas (TextWriter f)
        {
            int size = m_stateset.Count;
            m_gotoDelta = new int [size];
            foreach (RhsState p_RhsState in m_stateset.Values)
            {
                int val = 0;
                statedelta sd = p_RhsState.findGotoSet (m_gotoset);
                if (sd.first.Count > 0)
                    val = sd.second.m_delta;
                m_gotoDelta [p_RhsState.stateNumber] = val;
            }

            int i;
            f.WriteLine ("\t\tinternal readonly static int[] g_gotoDelta =");
            f.WriteLine ("\t\t{");
            for (i = 0; i < size;)
            {
                if ((i % 10) == 0)
                    f.Write ("\t\t\t");
                f.Write ("{0,6:d}, ", m_gotoDelta [i]);
                if ((++i % 10) == 0)
                    f.WriteLine ();
            }
            if ((i % 10) != 0)
                f.WriteLine ();
            f.WriteLine ("\t\t};");
            f.WriteLine ();
        }

        private void GenerateProductionTables (TextWriter f)
        {
            int i;

            int size = RhsProduction.productionCounter + 1;
            m_productionLengths = new int [size];
            m_productionLengths [0] = 0;

            foreach (RhsProductionNode p_RhsProductionNode in m_prodlist.Values)
            {
                if (p_RhsProductionNode == null)
                    continue;
                foreach (RhsProduction p_RhsProduction in p_RhsProductionNode.Productions)
                {
                    m_productionLengths [p_RhsProduction.productionNumber] = p_RhsProduction.length;
                }
            }

            f.WriteLine ("\t\tinternal readonly static int[] g_productionLengths = ");
            f.WriteLine ("\t\t{");
            for (i = 0; i < size;)
            {
                if ((i % 10) == 0)
                    f.Write ("\t\t\t");
                f.Write ("{0,6:d}, ", m_productionLengths [i]);
                if ((++i % 10) == 0)
                    f.WriteLine ();
            }
            if ((i % 10) != 0)
                f.WriteLine ();
            f.WriteLine ("\t\t};");
            f.WriteLine ();

            m_productionRules = new int [size];
            m_productionRules [0] = 0;

            foreach (KeyValuePair<SymbolToken, RhsProductionNode> prodit in m_prodlist)
            {
                RhsProductionNode p_RhsProductionNode = prodit.Value;
                if (p_RhsProductionNode == null)
                    continue;
                SymbolToken p_SymbolToken = prodit.Key;
                int index = p_SymbolToken.index;
                foreach (RhsProduction p_RhsProduction in p_RhsProductionNode.Productions)
                {
                    m_productionRules [p_RhsProduction.productionNumber] = index /*- m_minNonTerminalNr*/;
                }
            }

            f.WriteLine ("\t\tinternal readonly static int[] g_productionRules = ");
            f.WriteLine ("\t\t{");
            for (i = 0; i < size;)
            {
                if ((i % 10) == 0)
                    f.Write ("\t\t\t");
                f.Write ("{0,6:d}, ", m_productionRules [i]);
                if ((++i % 10) == 0)
                    f.WriteLine ();
            }
            if ((i % 10) != 0)
                f.WriteLine ();
            f.WriteLine ("\t\t};");
            f.WriteLine ();

            m_defaultGoto = new int [m_maxNonTerminalNr + 1];

            foreach (KeyValuePair<SymbolToken, RhsProductionNode> prodit in m_prodlist)
            {
                RhsProductionNode p_RhsProductionNode = prodit.Value;
                if (p_RhsProductionNode == null)
                    continue;
                SymbolToken p_SymbolToken = prodit.Key;
                if (p_SymbolToken.declarator == (uint) AnglrClassificationType.TerminalName)
                    continue;
                if (p_SymbolToken.index >= size)
                    Logger?.WarnLine ("default goto " + p_SymbolToken.index + " > " + size);
                m_defaultGoto [p_SymbolToken.index /*- m_minNonTerminalNr*/] = p_RhsProductionNode.BestGoto;
            }

            f.WriteLine ("\t\tinternal readonly static int[] g_defaultGoto = ");
            f.WriteLine ("\t\t{");
            for (i = 0; i < m_maxNonTerminalNr /*- m_minNonTerminalNr*/;)
            {
                if ((i % 10) == 0)
                    f.Write ("\t\t\t");
                f.Write ("{0,6:d}, ", m_defaultGoto [i]);
                if ((++i % 10) == 0)
                    f.WriteLine ();
            }
            if ((i % 10) != 0)
                f.WriteLine ();
            f.WriteLine ("\t\t};");
            f.WriteLine ();
        }

        private void GenerateReductionTables (TextWriter f)
        {
            HashSet<int> deltaset = new HashSet<int> ();
            int i;
            int size = 1000;
            m_rcheck = new int [size];
            m_rstate = new int [size];
            m_maxRCheck = -1;

            foreach (RhsState p_RhsState in m_stateset.Values)
            {
                if (p_RhsState.reductionsSetSize < 2)
                    continue;
                prodset p_reductions = p_RhsState.reductions;

                int delta = 1;
                while (true)
                {
                    while (deltaset.Contains (delta))
                        ++delta;

                    bool brokenLoop = false;
                    foreach (RhsConfiguration p_RhsConfiguration in p_reductions.Values)
                    {
                        brokenLoop = false;
                        TokenSet p_TokenSet = p_RhsConfiguration.getFollowSet;
                        tokset p_tokset = p_TokenSet.set;
                        foreach (SymbolToken p_SymbolToken in p_tokset.Values)
                        {
                            int index = delta + p_SymbolToken.index;
                            if (index > m_maxRCheck)
                                m_maxRCheck = index;
                            if (index >= size)
                                size = ResizeReductionTables (size, 1000);
                            if (m_rcheck [index] != 0)
                            {
                                brokenLoop = true;
                                break;
                            }
                        }
                        if (brokenLoop)
                            break;
                    }
                    if (!brokenLoop)
                        break;
                    ++delta;
                }

                p_RhsState.reductionsDelta = delta;
                deltaset.Add (delta);

                foreach (KeyValuePair<int, RhsConfiguration> prodit in p_reductions)
                {
                    RhsConfiguration p_RhsConfiguration = prodit.Value;
                    TokenSet p_TokenSet = p_RhsConfiguration.getFollowSet;
                    tokset p_tokset = p_TokenSet.set;
                    foreach (SymbolToken p_SymbolToken in p_tokset.Values)
                    {
                        int index = p_SymbolToken.index;
                        // reduce/reduce conflict
                        if (m_rcheck [delta + index] != 0)
                            ; //Logger?.WarnRawLine ("reduce/reduce overwrite");
                        m_rcheck [delta + index] = index;
                        m_rstate [delta + index] = prodit.Key;
                    }
                }
            }

            ++m_maxRCheck;

            f.WriteLine ("\t\tinternal readonly static int[] g_rcheck =");
            f.WriteLine ("\t\t{");
            if (m_maxRCheck == 0)
                f.WriteLine ("\t\t\t0");
            for (i = 0; i < m_maxRCheck;)
            {
                if ((i % 10) == 0)
                    f.Write ("\t\t\t");
                f.Write ("{0,6:d}, ", m_rcheck [i]);
                if ((++i % 10) == 0)
                    f.WriteLine ();
            }
            if ((i % 10) != 0)
                f.WriteLine ();
            f.WriteLine ("\t\t};");
            f.WriteLine ();

            f.WriteLine ("\t\tinternal readonly static int[] g_rstate =");
            f.WriteLine ("\t\t{");
            if (m_maxRCheck == 0)
                f.WriteLine ("\t\t\t0");
            for (i = 0; i < m_maxRCheck;)
            {
                if ((i % 10) == 0)
                    f.Write ("\t\t\t");
                f.Write ("{0,6:d}, ", m_rstate [i]);
                if ((++i % 10) == 0)
                    f.WriteLine ();
            }
            if ((i % 10) != 0)
                f.WriteLine ();
            f.WriteLine ("\t\t};");
            f.WriteLine ();

            size = m_stateset.Count;
            m_reductions = new int [size];

            foreach (RhsState p_RhsState in m_stateset.Values)
                m_reductions [p_RhsState.stateNumber] = (p_RhsState.reductionsSetSize > 1) ? -p_RhsState.reductionsDelta : p_RhsState.defaultReduction;

            f.WriteLine ("\t\tinternal readonly static int[] g_reductions =");
            f.WriteLine ("\t\t{");
            for (i = 0; i < size;)
            {
                if ((i % 10) == 0)
                    f.Write ("\t\t\t");
                f.Write ("{0,6:d}, ", m_reductions [i]);
                if ((++i % 10) == 0)
                    f.WriteLine ();
            }
            if ((i % 10) != 0)
                f.WriteLine ();
            f.WriteLine ("\t\t};");
            f.WriteLine ();
        }

        private void GenerateGLRTables (TextWriter f)
        {
            int i;
            HashSet<int> deltaset = new HashSet<int> ();
            int size = 1000;
            m_glrcheck = new int [size];
            m_glrstate = new int [size];
            m_maxGLRCheck = -1;
            int glrsize = 0;
            glrtokset p_glrtokset = new glrtokset ();

            foreach (RhsState p_RhsState in m_stateset.Values)
            {
                if (!p_RhsState.hasGLRCondition)
                    continue;
                glrset p_glrset = p_RhsState.getGLRSet;
                foreach (GLRToken p_GLRToken in p_glrset.Values)
                {
                    GLRToken p_GLRTokenRef = null;
                    if (p_glrtokset.TryGetValue (p_GLRToken, out p_GLRTokenRef))
                    {
                        p_GLRToken.position = p_GLRTokenRef.position;
                        continue;
                    }
                    p_GLRToken.position = glrsize;
                    p_glrtokset [p_GLRToken] = p_GLRToken;
                    glrsize += 2 + p_GLRToken.getRdlist.Count;
                }
            }

            m_glrcells = new int [glrsize];

            foreach (GLRToken p_GLRToken in p_glrtokset.Values)
            {
                int position = p_GLRToken.position;
                rdlist p_rdlist = p_GLRToken.getRdlist;
                m_glrcells [position++] = 2 + p_rdlist.Count;
                m_glrcells [position++] = p_GLRToken.state;
                foreach (RhsProduction p_RhsProduction in p_rdlist)
                    m_glrcells [position++] = p_RhsProduction.productionNumber;
            }

            foreach (RhsState p_RhsState in m_stateset.Values)
            {
                if (!p_RhsState.hasGLRCondition)
                    continue;
                glrset p_glrset = p_RhsState.getGLRSet;
                int delta = 1;
                while (true)
                {
                    while (deltaset.Contains (delta))
                        ++delta;

                    bool brokenLoop = false;
                    foreach (SymbolToken p_SymbolToken in p_glrset.Keys)
                    {
                        int index = delta + p_SymbolToken.index;
                        if (index > m_maxGLRCheck)
                            m_maxGLRCheck = index;
                        if (index >= size)
                            size = ResizeGLRTables (size, 1000);
                        if (m_glrcheck [index] != 0)
                        {
                            brokenLoop = true;
                            break;
                        }
                    }
                    if (!brokenLoop)
                        break;
                    ++delta;
                }
                p_RhsState.GLRDelta = delta;
                deltaset.Add (delta);
                foreach (KeyValuePair<SymbolToken, GLRToken> glrit in p_glrset)
                {
                    SymbolToken p_SymbolToken = glrit.Key;
                    int index = delta + p_SymbolToken.index;
                    if (m_glrcheck [index] != 0)
                        Logger?.WarnLine ("overwrite glr-check field, index = " + index);
                    m_glrcheck [index] = index - delta;
                    m_glrstate [index] = glrit.Value.position;
                }
            }

            ++m_maxGLRCheck;

            f.WriteLine ("\t\tinternal readonly static int[] g_glrcheck =");
            f.WriteLine ("\t\t{");
            for (i = 0; i < m_maxGLRCheck;)
            {
                if ((i % 10) == 0)
                    f.Write ("\t\t\t");
                f.Write ("{0,6:d}, ", m_glrcheck [i]);
                if ((++i % 10) == 0)
                    f.WriteLine ();
            }
            if ((i % 10) != 0)
                f.WriteLine ();
            f.WriteLine ("\t\t};");
            f.WriteLine ();

            f.WriteLine ("\t\tinternal readonly static int[] g_glrstate =");
            f.WriteLine ("\t\t{");
            for (i = 0; i < m_maxGLRCheck;)
            {
                if ((i % 10) == 0)
                    f.Write ("\t\t\t");
                f.Write ("{0,6:d}, ", m_glrstate [i]);
                if ((++i % 10) == 0)
                    f.WriteLine ();
            }
            if ((i % 10) != 0)
                f.WriteLine ();
            f.WriteLine ("\t\t};");
            f.WriteLine ();

            f.WriteLine ("\t\tinternal readonly static int[] g_glrcells =");
            f.WriteLine ("\t\t{");
            for (i = 0; i < glrsize;)
            {
                if ((i % 10) == 0)
                    f.Write ("\t\t\t");
                f.Write ("{0,6:d}, ", m_glrcells [i]);
                if ((++i % 10) == 0)
                    f.WriteLine ();
            }
            if ((i % 10) != 0)
                f.WriteLine ();
            f.WriteLine ("\t\t};");
            f.WriteLine ();
        }

        private void GenerateParserSrc (_parser_part_ part)
        {
            string fileName = OutputDir + parserClassName + ".cs";
            try
            {
                Logger?.InfoRawLine ("Generate parser source file '" + fileName + "'");
                StreamWriter f = File.CreateText (fileName);

                f.WriteLine ($"{m_generatedHeaderText}");

                foreach (string line in g_ParserTemplateCsSrc)
                {
                    if (line == g_CsTables)
                        GenerateTables (f);
                    else if (line == g_CsActions)
                    {
                        foreach (int key in m_proddict.Keys)
                        {
                            f.WriteLine ("\t\t\tcase " + key + ":");
                            RhsProduction p_RhsProduction = m_proddict [key];
                            int prodLen = p_RhsProduction.length;
                            if (p_RhsProduction.symbolToken.name [0] == '$')
                            {
                                f.WriteLine ("\t\t\tbreak;");
                                continue;
                            }
                            f.Write ("\t\t\t\tcurrentValue = " + p_RhsProduction.symbolToken.correctName + "_" + p_RhsProduction.index + " (");
                            string sep = "";
                            int cnt = 0;
                            foreach (RhsNode p_RhsNode in p_RhsProduction.rhsNodes)
                            {
                                SymbolToken p_SymbolToken = p_RhsNode.symbolToken;
                                f.Write (sep);
                                if (p_SymbolToken.declarator == (uint) AnglrClassificationType.TerminalName)
                                    f.Write ("(SyntaxTreeToken) valueStack [" + (cnt - prodLen) + "]");
                                else
                                    f.Write ("(" + p_SymbolToken.correctName + ") valueStack [" + (cnt - prodLen) + "]");
                                sep = ", ";
                                ++cnt;
                            }
                            f.WriteLine (");");
                            f.WriteLine ("\t\t\t\tbreak;");
                        }
                    }
                    else if (line == g_CsCounters)
                    {
                        foreach (SymbolToken p_SymbolToken in m_prodlist.Keys)
                        {
                            if (p_SymbolToken.name [0] == '$')
                                continue;
                            f.WriteLine ("\t\t\tif (" + p_SymbolToken.correctName + ".g_counter != 0)");
                            f.WriteLine ("\t\t\t\tAnglrLogger?.DebugRawLine (\"" + p_SymbolToken.correctName + ".g_counter = \" + " + p_SymbolToken.correctName + ".g_counter);");
                        }
                    }
                    else if (line == g_CSEventRegistration)
                    {
                        f.WriteLine ("\t\t\t//");
                        f.WriteLine ("\t\t\t// event registration templates");
                        f.WriteLine ("\t\t\t//");
                        f.WriteLine ("\t\t\tCommon_Event += Invoke_Common_Callback;");
                        foreach (SymbolToken p_SymbolToken in m_prodlist.Keys)
                        {
                            if (p_SymbolToken.name [0] == '$')
                                continue;
                            f.WriteLine ("\t\t\t" + p_SymbolToken.correctName + "_Event += Invoke_" + p_SymbolToken.correctName + "_Callback;");
                        }
                    }
                    else if (line == g_CSEventDeclaration)
                    {
                        f.WriteLine ();
                        f.WriteLine ("\t\t//");
                        f.WriteLine ("\t\t// event handler templates");
                        f.WriteLine ("\t\t//");
                        f.WriteLine ();
                        f.WriteLine ("\t\tprivate bool Invoke_Common_Callback (SyntaxTreeCallbackReason reason, int kind, SyntaxTreeBase p_node)");
                        foreach (string tline in g_EventTestBodyCs)
                            f.WriteLine (tline);
                        foreach (SymbolToken p_SymbolToken in m_prodlist.Keys)
                        {
                            if (p_SymbolToken.name [0] == '$')
                                continue;
                            f.WriteLine ();
                            f.WriteLine ("\t\tprivate bool Invoke_" + p_SymbolToken.correctName + "_Callback (SyntaxTreeCallbackReason reason, " + p_SymbolToken.correctName + ".production_kind kind, " + p_SymbolToken.correctName + " p_" + p_SymbolToken.correctName + ")");
                            foreach (string lptr in g_EventTestBodyCs)
                                f.WriteLine (lptr);
                        }
                        f.WriteLine ();
                        f.WriteLine ("\t\t//");
                        f.WriteLine ("\t\t// event fire templates");
                        f.WriteLine ("\t\t//");
                        foreach (SymbolToken p_SymbolToken in m_prodlist.Keys)
                        {
                            if (p_SymbolToken.name [0] == '$')
                                continue;
                            f.WriteLine ();
                            f.WriteLine ("\t\tpublic void InvokeTest (" + p_SymbolToken.correctName + " p_" + p_SymbolToken.correctName + ")");
                            f.WriteLine ("\t\t{");
                            f.WriteLine ("\t\t\tTraverse (p_" + p_SymbolToken.correctName + ");");
                            f.WriteLine ("\t\t\tTraverseCommon (p_" + p_SymbolToken.correctName + ");");
                            f.WriteLine ("\t\t}");
                        }
                    }
                    else if (line == g_ScannerNamespace)
                    {
                        SortedSet<string> nameSpaces = new SortedSet<string> ();
                        ParserPart parserPart = attributeCollection.parserParts [part.m__identifier_.text];
                        SymbolToken lexerPartSymbol = parserPart.lexerPartSymbol;
                        if (lexerPartSymbol != null)
                        {
                            LexerPart lexerPart = attributeCollection.lexerParts [lexerPartSymbol.name];
                            for (symtab.Enumerator enumerator = lexerPart.scannerSymbols.enumerator; enumerator.MoveNext ();)
                            {
                                SymbolToken symbolToken = enumerator.Current.Value;
                                ScannerPart scannerPart = attributeCollection.scannerParts [symbolToken.name];
                                SymbolToken declarationSymbol = scannerPart.declarationPartSymbol;
                                if (declarationSymbol == null)
                                    continue;
                                DeclarationsPart declarationsPart = null;
                                if (!attributeCollection.declarationParts.TryGetValue (declarationSymbol.name, out declarationsPart))
                                    continue;
                                nameSpaces.Add (declarationsPart.declarationsNameSpace);
                            }
                            if (lexerPart.lexerNameSpace != null)
                                nameSpaces.Add (lexerPart.lexerNameSpace);
                        }

                        foreach (string name in nameSpaces)
                        {
                            f.WriteLine ($"using {name};");
                        }
                        f.WriteLine ();
                    }
                    else if (line == g_prodIds)
                        GenerateProdIds (f);
                    else if (line == g_fragments)
                        GenerateFragmenmts (f);
                    else
                    {
                        ParserPart parserPart = parserParts [part.m__identifier_.text];
                        LexerPart lexerPart = lexerParts [parserPart.lexerId];
                        f.WriteLine
                        (
                            line.
                            Replace (g_CsClassname, parserPart.parserClassName).
                            Replace (g_CsNameSpace, parserPart.parserNameSpace).
                            Replace (g_ScannerClassDef, lexerPart.lexerClassName)
                        );
                    }
                }
                f.Close ();
                SourceFileList.Add (fileName);
            }
            catch (Exception e)
            {
                Logger?.ErrorLine ($"Cannot create parser source file '{fileName}', exception thrown: {e.Message}");
            }
        }

        private void GenerateParserHdr ()
        {
            return;
#pragma warning disable CS0162
            string fileName = parserClassName + ".h";
            Logger?.InfoRawLine ("Generate parser header file '" + fileName + "'");
            StreamWriter f = File.CreateText (fileName);

            f.WriteLine ($"{m_generatedHeaderText}");

            foreach (string line in g_ParserTemplateCsHdr)
            {
                f.WriteLine (line.Replace (g_CsClassname, parserClassName));
            }

            f.Close ();
#pragma warning restore CS0162
        }

        public void CheckPDA ()
        {
            foreach (RhsState p_RhsState in m_stateset.Values)
            {
                int stateNumber = p_RhsState.stateNumber;

                int delta = m_shiftDelta [stateNumber];
                if (delta > 0)
                    foreach (SymbolToken p_SymbolToken in m_terminals)
                    {
                        RhsState p_RhsStateRef = p_RhsState.checkShift (p_SymbolToken);
                        int index = p_SymbolToken.index;
                        if ((index + delta < 0) || (index + delta >= m_maxCheck) || (m_check [index + delta] != index))
                        {
                            if (p_RhsStateRef != null)
                                Logger?.InfoRawLine ("state " + stateNumber + ", shift " + p_SymbolToken.name + " not handled");
                        }
                        else
                        {
                            if (p_RhsStateRef != null)
                            {
                                if (m_state [index + delta] != p_RhsStateRef.stateNumber)
                                    Logger?.InfoRawLine ("state " + stateNumber + ", shift " + p_SymbolToken.name + " to " + m_state [index + delta]);
                            }
                            else
                                Logger?.InfoRawLine ("state " + stateNumber + ", shift " + p_SymbolToken.name + " handled");
                        }
                    }
                else
                {
                    if (m_reductions [stateNumber] == 0)
                        Logger?.InfoRawLine ("state " + stateNumber + ", no action");
                }

                delta = m_gotoDelta [stateNumber];
                if (delta > 0)
                    foreach (SymbolToken p_SymbolToken in m_nonterminals)
                    {
                        RhsState p_RhsStateRef = p_RhsState.checkGoto (p_SymbolToken);
                        int index = p_SymbolToken.index;
                        if ((index + delta < 0) || (index + delta >= m_maxCheck) || (m_check [index + delta] != index))
                        {
                            if (p_RhsStateRef != null)
                            {
                                RhsProductionNode p_RhsProductionNode = null;
                                if ((!m_prodlist.TryGetValue (p_SymbolToken, out p_RhsProductionNode)) || (p_RhsProductionNode == null) || (m_defaultGoto [p_SymbolToken.index /*- m_minNonTerminalNr*/] != p_RhsProductionNode.BestGoto))
                                    Logger?.InfoRawLine ("state " + stateNumber + ", goto " + p_SymbolToken.name + " not handled");
                            }
                        }
                        else
                        {
                            if (p_RhsStateRef != null)
                            {
                                if (m_state [index + delta] != p_RhsStateRef.stateNumber)
                                {
                                    RhsProductionNode p_RhsProductionNode = null;
                                    if ((!m_prodlist.TryGetValue (p_SymbolToken, out p_RhsProductionNode)) || (p_RhsProductionNode == null) || (m_defaultGoto [p_SymbolToken.index /*- m_minNonTerminalNr*/] != p_RhsProductionNode.BestGoto))
                                        Logger?.InfoRawLine ("state " + stateNumber + ", goto " + p_SymbolToken.name + " to " + m_state [index + delta]);
                                }
                            }
                        }
                    }
                else
                {
                }

                if ((p_RhsState.reductionsSetSize > 0) && (m_reductions [stateNumber] == 0))
                    Logger?.InfoRawLine ("no reduction registered in state " + stateNumber);
            }
        }

        //public bool CheckPDA (ParserInterface parserItf)
        //{
        //    foreach (RhsState p_RhsState in m_stateset.Values)
        //    {
        //        int stateNumber = p_RhsState.stateNumber;

        //        int delta = parserItf.shiftDelta () [stateNumber];
        //        if (delta > 0)
        //            foreach (SymbolToken p_SymbolToken in m_terminals)
        //            {
        //                RhsState p_RhsStateRef = p_RhsState.checkShift (p_SymbolToken);
        //                int index = p_SymbolToken.index;
        //                if ((index + delta < 0) || (index + delta >= parserItf.glrcheck ().Length) || (parserItf.check () [index + delta] != index))
        //                {
        //                    if (p_RhsStateRef != null)
        //                        return false;   // existing terminal symbol has no entry in transition table
        //                }
        //                else
        //                {
        //                    if (p_RhsStateRef != null)
        //                    {
        //                        if (parserItf.state () [index + delta] != p_RhsStateRef.stateNumber)
        //                            return false;   // existing terminal symbol does not shift into correct state
        //                    }
        //                    else
        //                        return false;   // there is no terminal symbol for an existing transition table entry
        //                }
        //            }
        //        else
        //        {
        //            if (parserItf.reductions () [stateNumber] == 0)
        //                return false;   // there is no default reduction for given terminal symbol
        //        }

        //        delta = parserItf.gotoDelta () [stateNumber];
        //        if (delta > 0)
        //            foreach (SymbolToken p_SymbolToken in m_nonterminals)
        //            {
        //                RhsState p_RhsStateRef = p_RhsState.checkGoto (p_SymbolToken);
        //                int index = p_SymbolToken.index;
        //                if ((index + delta < 0) || (index + delta >= parserItf.glrcheck ().Length) || (parserItf.check () [index + delta] != index))
        //                {
        //                    if (p_RhsStateRef != null)
        //                    {
        //                        RhsProductionNode p_RhsProductionNode = null;
        //                        if ((!m_prodlist.TryGetValue (p_SymbolToken, out p_RhsProductionNode)) || (p_RhsProductionNode == null) || (parserItf.defaultGoto () [p_SymbolToken.index /*- m_minNonTerminalNr*/] != p_RhsProductionNode.BestGoto))
        //                            return false;   // existing non-terminal symbol has no entry in transtion table
        //                    }
        //                }
        //                else
        //                {
        //                    if (p_RhsStateRef != null)
        //                    {
        //                        if (parserItf.state () [index + delta] != p_RhsStateRef.stateNumber)
        //                        {
        //                            RhsProductionNode p_RhsProductionNode = null;
        //                            if ((!m_prodlist.TryGetValue (p_SymbolToken, out p_RhsProductionNode)) || (p_RhsProductionNode == null) || (parserItf.defaultGoto () [p_SymbolToken.index /*- m_minNonTerminalNr*/] != p_RhsProductionNode.BestGoto))
        //                                return false;   // existing non-terminal symbol does not goto into correct state
        //                        }
        //                    }
        //                }
        //            }
        //        else
        //        {
        //        }

        //        if ((p_RhsState.reductionsSetSize > 0) && (parserItf.reductions () [stateNumber] == 0))
        //            return false;   // there is no default reduction for given non-terminal symbol
        //    }
        //    return true;    // parser interface is compatible with this PDA
        //}

        private void PrepareSyntaxTree ()
        {
            foreach (RhsProductionNode p_RhsProductionNode in m_prodlist.Values)
            {
                if (p_RhsProductionNode != null)
                    p_RhsProductionNode.PrepareASTData ();
            }
        }

        private void GenerateParseHeaders ()
        {
            return;
#pragma warning disable CS0162
            StreamWriter f = File.CreateText ("parse-syntax.h");

            f.WriteLine ($"{m_generatedHeaderText}");

            f.WriteLine ("#pragma once");
            f.WriteLine ("");
            foreach (SymbolToken p_SymbolToken in m_prodlist.Keys)
            {
                if (p_SymbolToken.name [0] == '$')
                    continue;
                f.WriteLine ("class\t" + p_SymbolToken.correctName + ";");
            }
            f.WriteLine ("");
            foreach (SymbolToken p_SymbolToken in m_prodlist.Keys)
            {
                if (p_SymbolToken.name [0] == '$')
                    continue;
                f.WriteLine ("#include \"" + p_SymbolToken.correctName + ".h\"");
            }
            f.WriteLine ();
            f.WriteLine ("enum	SyntaxTreeCallbackReason");
            f.WriteLine ("{");
            f.WriteLine ("\tBuilderCallbackReason = 1,");
            f.WriteLine ("\tTraversalPrologueCallbackReason = 2,");
            f.WriteLine ("\tTraversalMidTermCallbackReason = 3,");
            f.WriteLine ("\tTraversalEpilogueCallbackReason = 4");
            f.WriteLine ("};");
            f.Close ();
#pragma warning restore CS0162
        }

        private void GenerateTokenHeader ()
        {
            return;
        }

        private void GenerateScanner (ScannerPart scannerPart)
        {
            string scannerClassName = scannerPart.regexClassName;
            string fileName = OutputDir + scannerClassName + ".cs";
            try
            {
                Logger?.InfoRawLine ($"Generate scanner source file '{fileName}'");
                StreamWriter f = File.CreateText (fileName);
                f.WriteLine ($"{m_generatedHeaderText}");
                f.WriteLine (scannerPart.Generate (attributeCollection));
                f.Close ();
                SourceFileList.Add (fileName);
            }
            catch (Exception e)
            {
                Logger?.ErrorLine ($"Cannot create scanner source file {fileName}, exception thrown: {e.Message}");
            }
        }

        private void GenerateLexer (LexerPart lexerPart)
        {
            string lexerClassName = lexerPart.lexerClassName;
            string fileName = OutputDir + lexerClassName + ".cs";
            try
            {
                Logger?.InfoRawLine ($"Generate lexer source file '{fileName}'");
                StreamWriter f = File.CreateText (fileName);
                f.WriteLine ($"{m_generatedHeaderText}");
                f.WriteLine (lexerPart.Generate (attributeCollection));
                f.Close ();
                SourceFileList.Add (fileName);
            }
            catch (Exception e)
            {
                Logger?.ErrorLine ($"Cannot create lexer source file {fileName}, exception thrown: {e.Message}");
            }
        }

        private void GenerateSyntaxTreeClasses ()
        {
            foreach (KeyValuePair<SymbolToken, RhsProductionNode> prodit in m_prodlist)
            {
                SymbolToken p_SymbolToken = prodit.Key;
                RhsProductionNode p_RhsProductionNode = prodit.Value;

                if (p_SymbolToken.name [0] == '$')
                    continue;

                string srcName = OutputDir + p_SymbolToken.correctName + ".cs";
                try
                {
                    StreamWriter src = File.CreateText (srcName);

                    if (p_RhsProductionNode != null)
                    {
                        Logger?.InfoRawLine ($"Generate source file '{srcName}'");
                        src.WriteLine ($"{m_generatedHeaderText}");
                        src.WriteLine ($"using Anglr.Parser.Core;");
                        src.WriteLine ($"using Anglr.Parser.SyntaxTree;");
                        src.WriteLine ($"using Anglr.Parser.Walker;");
                        src.WriteLine ($"namespace {parserNameSpace}");
                        src.WriteLine ($"{{");
                        p_RhsProductionNode.GenerateCsHeaderFile (src);
                        p_RhsProductionNode.GenerateCsSourceFile (src);
                        src.WriteLine ($"}}");
                    }
                    src.Close ();
                    SourceFileList.Add (srcName);
                }
                catch (Exception e)
                {
                    Logger?.ErrorLine ($"Cannot create source file {srcName}, exception thrown: {e.Message}");
                }
            }
        }

        private void GenerateSemanticClasses ()
        {
            PrepareSyntaxTree ();
            GenerateParseHeaders ();
            GenerateSyntaxTreeClasses ();
        }

        private int ResizeTransitionTables (int size, int increment)
        {
            int newsize = size + increment;
            Array.Resize (ref m_check, newsize);
            Array.Resize (ref m_state, newsize);
            return newsize;
        }

        private int ResizeReductionTables (int size, int increment)
        {
            int newsize = size + increment;
            Array.Resize (ref m_rcheck, newsize);
            Array.Resize (ref m_rstate, newsize);
            return newsize;
        }

        private int ResizeGLRTables (int size, int increment)
        {
            int newsize = size + increment;
            Array.Resize (ref m_glrcheck, newsize);
            Array.Resize (ref m_glrstate, newsize);
            return newsize;
        }

        public void InvokeStatesIterator (Func<RhsState, int> func)
        {
            foreach (var state in m_statearray)
            {
                func (state);
            }
        }

        public IAnglrLogger Logger { get; private set; }
        public static bool DisplayProductionsFlag { get; set; } = true;
        public static bool DisplayStartSetsFlag { get; set; } = false;
        public static bool DisplayEndSetsFlag { get; set; } = false;

        private anglrCompiler m_compiler = null;
        private _anglr_file_fragment_ m_anglrFileFragment = null;
        private string m_generatedHeaderText = $"//{Environment.NewLine}//\tThis file was generated with ANGLR compiler{Environment.NewLine}//{Environment.NewLine}using System;{Environment.NewLine}";

        private rhsstateset m_stateset = new rhsstateset ();
        private statearray m_statearray = new statearray ();

        private shiftset m_shiftset = new shiftset ();
        private gotoset m_gotoset = new gotoset ();

        public int magicNr { get; private set; }

        private int m_maxCheck = -1;
        private int m_maxRCheck = -1;
        private int m_maxGLRCheck = -1;

        private int [] m_terminalCodes = null;
        private int [] m_nonTerminalCodes = null;
        private int [] m_check = null;
        private int [] m_state = null;
        private int [] m_shiftDelta = null;
        private int [] m_gotoDelta = null;
        private int [] m_productionLengths = null;
        private int [] m_productionRules = null;
        private int [] m_defaultGoto = null;
        private int [] m_reductions = null;
        private int [] m_rcheck = null;
        private int [] m_rstate = null;
        private int [] m_glrcheck = null;
        private int [] m_glrstate = null;
        private int [] m_glrcells = null;

        private DirectoryInfo directoryInfo = null;
        public string OutputDir { get; private set; } = "";
        public ArrayList SourceFileList { get; private set; } = new ArrayList ();
        public ArrayList LibraryFileList { get; private set; } = new ArrayList ();

    }
}
