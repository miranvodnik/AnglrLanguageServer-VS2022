using Anglr.Parser;
using AnglrLogLibrary;
using System;
using System.Collections.Generic;
using System.Diagnostics.SymbolStore;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace AnglrLibrary
{
    [Serializable]
    public class RhsProductionNode
    {
        internal RhsProductionNode (SymbolToken p_productionName)
        {
            ProductionName = p_productionName;
            EpsilonCondition = false;
            Used = false;
            CascadeResolved = false;
        }

        internal void Dispose ()
        {
            foreach (RhsProduction p_RhsProduction in Productions)
                p_RhsProduction.Dispose ();
            StartSet.Dispose ();
        }

        internal void add (RhsProduction rhsProduction)
        {
            cmpProduction cmp = new cmpProduction ();
            foreach (RhsProduction production in Productions)
            {
                if (cmp.EqualsLiteraly (production, rhsProduction))
                    return;
            }
            Productions.Add (rhsProduction);
        }

        internal void markGotoCounter (int state)
        {
            if (m_gotocnt.Keys.Contains (state))
                m_gotocnt [state] += 1;
            else
                m_gotocnt [state] = 1;
        }

        internal void defineBestGotoCounter ()
        {
            int bestCounter = -1;

            foreach (KeyValuePair<int, int> keyval in m_gotocnt)
                if (keyval.Value > bestCounter)
                {
                    bestCounter = keyval.Value;
                    BestGoto = keyval.Key;
                }
        }

        internal void displayGotoCnt (IAnglrLogger logger)
        {
            logger?.DebugRawLine (ProductionName.name);
            foreach (KeyValuePair<int, int> keyval in m_gotocnt)
                logger?.DebugRawLine ("\tgoto " + keyval.Key + " = " + keyval.Value);
            logger?.DebugRawLine ("\tbest = " + BestGoto);
        }

        internal void PrepareASTData ()
        {
            int index = 1;
            foreach (RhsProduction p_RhsProduction in Productions)
            {
                p_RhsProduction.index = index;
                if (!m_protoset.Keys.Contains (p_RhsProduction))
                    m_protoset [p_RhsProduction] = 1;
                else
                    m_protoset [p_RhsProduction] += 1;
                foreach (RhsNode p_RhsNode in p_RhsProduction.rhsNodes)
                {
                    SymbolToken p_SymbolToken = p_RhsNode.symbolToken;
                    if (!m_symset.Keys.Contains (p_SymbolToken))
                        m_symset [p_SymbolToken] = new symindex (0, 0);
                }
                ++index;
            }

            foreach (RhsProduction p_RhsProduction in Productions)
            {
                foreach (KeyValuePair<SymbolToken, symindex> keyval in m_symset)
                    keyval.Value.first = 0;
                foreach (RhsNode p_RhsNode in p_RhsProduction.rhsNodes)
                {
                    SymbolToken p_SymbolToken = p_RhsNode.symbolToken;
                    if (!m_symset.Keys.Contains (p_SymbolToken))
                        continue;
                    symindex sindex = m_symset [p_SymbolToken];
                    p_RhsNode.index = sindex.first;
                    if (sindex.second < sindex.first)
                        sindex.second = sindex.first;
                    ++sindex.first;
                }
            }
        }

        internal void GenerateCppHeaderFile (TextWriter writer)
        {
            string correctName = ProductionName.correctName;

            writer.WriteLine ("#pragma once");
            writer.WriteLine ();
            writer.WriteLine ("#include \"SyntaxTreeBase.h\"");
            writer.WriteLine ("#include \"parse-syntax.h\"");
            writer.WriteLine ();
            writer.WriteLine ("//");
            writer.WriteLine ("// class associated with syntax rule " + ProductionName.name);
            writer.WriteLine ("//");
            writer.WriteLine ();
            writer.WriteLine ("class\t" + correctName + " : public SyntaxTreeBase");
            writer.WriteLine ("{");
            writer.WriteLine ("public:");
            writer.WriteLine ("\tenum production_kind\t// enumerated production(s) of syntax rule " + ProductionName.name);
            writer.WriteLine ("\t{");
            int cnt = 1;
            foreach (RhsProduction p_RhsProduction in Productions)
            {
                writer.Write ("\t\tg_" + correctName + "_" + cnt + " = " + cnt + ",\t//");
                p_RhsProduction.display (writer);
                writer.WriteLine ();
                ++cnt;
            }
            writer.WriteLine ("\t};");
            writer.WriteLine ();
            writer.WriteLine ("public:");
            writer.WriteLine ("\t// Constructor declaration(s) associated with production(s) of syntax rule " + ProductionName.name);
            foreach (KeyValuePair<RhsProduction, int> keyval in m_protoset)
            {
                writer.Write ("\t" + correctName + " (");
                RhsProduction p_RhsProduction = keyval.Key;
                string sep = "";
                int tokenIndex = 0;
                foreach (RhsNode p_RhsNode in p_RhsProduction.rhsNodes)
                {
                    writer.Write (sep);
                    SymbolToken p_SymbolToken = p_RhsNode.symbolToken;
                    if (p_SymbolToken.declarator == (uint) AnglrClassificationType.TerminalName)
                    {
                        writer.Write ("SyntaxTreeToken* p_token");
                        if (tokenIndex > 0)
                            writer.Write ("_" + tokenIndex);
                        ++tokenIndex;
                    }
                    else
                    {
                        writer.Write (p_SymbolToken.correctName + "* p_" + p_SymbolToken.correctName);
                        int index = p_RhsNode.index;
                        if (index > 0)
                            writer.Write ("_" + index);
                    }
                    sep = ", ";
                }
                if (keyval.Value > 1)
                    writer.Write (sep + "int kind");
                writer.WriteLine (");");
            }
            writer.WriteLine ("\t" + correctName + " (" + correctName + "& p_" + correctName + ");");
            writer.WriteLine ("\tinline virtual SyntaxTreeBase* Clone () { return (SyntaxTreeBase*) new " + correctName + " (*this); }");
            writer.WriteLine ("\tvirtual ~" + correctName + "();");
            writer.WriteLine ("\tvirtual bool checkInclusion (SyntaxTreeBase* element);");
            writer.WriteLine ("\tvirtual string Emit (int depth);");
            writer.WriteLine ("\tvirtual string EmitProductionTree (int depth);");
            writer.WriteLine ("\tvirtual void reparent (SyntaxTreeBase* parent);");
            writer.WriteLine ("private:");
            writer.WriteLine ("\tvoid _init ();");
            writer.WriteLine ();
            writer.WriteLine ("public:");
            writer.WriteLine ("\t// getters and setters associated with terminals (SyntaxTreeToken) and nonterminals (other than SyntaxTreeToken) within production(s) of syntax rule " + ProductionName.name);
            foreach (KeyValuePair<SymbolToken, symindex> keyval in m_symset)
            {
                SymbolToken p_SymbolToken = keyval.Key;
                string name = p_SymbolToken.correctName;
                int index = keyval.Value.second;
                if (p_SymbolToken.declarator == (uint) AnglrClassificationType.TerminalName)
                {
                    writer.WriteLine ("\tinline SyntaxTreeToken* get_" + name + " () { return m_" + name + "; }");
                    writer.WriteLine ("\tinline void set_" + name + " (SyntaxTreeToken* p_" + name + ") { m_" + name + " = p_" + name + "; }");
                    for (int i = 0; i < index; ++i)
                    {
                        writer.WriteLine ("\tinline SyntaxTreeToken* get_" + name + "_" + (i + 1) + " () { return m_" + name + "_" + (i + 1) + "; }");
                        writer.WriteLine ("\tinline void set_" + name + "_" + (i + 1) + " (SyntaxTreeToken* p_" + name + ") { m_" + name + "_" + (i + 1) + " = p_" + name + "; }");
                    }
                }
                else
                {
                    writer.WriteLine ("\tinline " + name + "* get_" + name + " () { return m_" + name + "; }");
                    writer.WriteLine ("\tinline void set_" + name + " ( " + name + "* p_" + name + ") { m_" + name + " = p_" + name + "; }");
                    for (int i = 0; i < index; ++i)
                    {
                        writer.WriteLine ("\tinline " + name + "* get_" + name + "_" + (i + 1) + " () { return m_" + name + "_" + (i + 1) + "; }");
                        writer.WriteLine ("\tinline void set_" + name + "_" + (i + 1) + " ( " + name + "* p_" + name + ") { m_" + name + "_" + (i + 1) + " = p_" + name + "; }");
                    }
                }
            }
            writer.WriteLine ();
            writer.WriteLine ("public:");
            writer.WriteLine ("\tstatic int g_counter;");
            writer.WriteLine ("private:");
            writer.WriteLine ("\t// objects associated with terminals (SyntaxTreeToken) and nonterminals (other than SyntaxTreeToken) within production(s) of syntax rule " + ProductionName.name);
            foreach (KeyValuePair<SymbolToken, symindex> keyval in m_symset)
            {
                SymbolToken p_SymbolToken = keyval.Key;
                string name = p_SymbolToken.correctName;
                int index = keyval.Value.second;
                if (p_SymbolToken.declarator == (uint) AnglrClassificationType.TerminalName)
                {
                    writer.WriteLine ("\tSyntaxTreeToken* m_" + name + ";");
                    for (int i = 0; i < index; ++i)
                        writer.WriteLine ("\tSyntaxTreeToken* m_" + name + "_" + (i + 1) + ";");
                }
                else
                {
                    writer.WriteLine ("\t" + name + "* m_" + name + ";");
                    for (int i = 0; i < index; ++i)
                        writer.WriteLine ("\t" + name + "* m_" + name + "_" + (i + 1) + ";");
                }
            }
            writer.WriteLine ();
            writer.WriteLine ("};");
        }

        internal void GenerateCppSourceFile (TextWriter writer)
        {
            string correctName = ProductionName.correctName;

            writer.WriteLine ("#include \"SyntaxTreeBuilder.h\"");
            writer.WriteLine ("#include \"" + correctName + ".h\"");

            writer.WriteLine ();
            writer.WriteLine ("int " + correctName + "::g_counter = 0;");
            writer.WriteLine ();
            foreach (KeyValuePair<RhsProduction, int> keyval in m_protoset)
            {
                RhsProduction p_RhsProduction = keyval.Key;
                writer.WriteLine ();
                writer.WriteLine ("//");
                writer.WriteLine ("// Constructor associated with the following production(s)");
                int variant = keyval.Value;
                if (variant > 1)
                {
                    cmpProduction cmp = new cmpProduction ();
                    foreach (RhsProduction p_RhsProductionRef in Productions)
                    {
                        if (!cmp.Equals (p_RhsProduction, p_RhsProductionRef))
                            continue;
                        writer.Write ("// " + ProductionName.name + " ->");
                        p_RhsProductionRef.display (writer);
                        writer.WriteLine ();
                    }
                }
                else
                {
                    writer.Write ("// " + ProductionName.name + " ->");
                    p_RhsProduction.display (writer);
                    writer.WriteLine ();
                }
                writer.WriteLine ("//");
                writer.WriteLine ();
                writer.Write (correctName + "::" + correctName + " (");
                string sep = "";
                int tokenIndex = 0;
                foreach (RhsNode p_RhsNode in p_RhsProduction.rhsNodes)
                {
                    SymbolToken p_SymbolToken = p_RhsNode.symbolToken;
                    string name = p_SymbolToken.correctName;
                    int index = p_RhsNode.index;
                    writer.Write (sep);
                    if (p_SymbolToken.declarator == (uint) AnglrClassificationType.TerminalName)
                    {
                        writer.Write ("SyntaxTreeToken* p_token");
                        if (tokenIndex > 0)
                            writer.Write ("_" + tokenIndex);
                        ++tokenIndex;
                    }
                    else
                    {
                        writer.Write (name + "* p_" + name);
                        if (index > 0)
                            writer.Write ("_" + index);
                    }
                    sep = ", ";
                }
                if (variant > 1)
                    writer.Write (sep + "int kind");
                writer.Write (") : SyntaxTreeBase (");
                writer.Write ("SyntaxTreeWalker::_" + correctName + "_ID, ");
                if (variant > 1)
                    writer.Write ("kind");
                else
                    writer.Write ("g_" + correctName + "_" + p_RhsProduction.index);
                writer.WriteLine (")");
                writer.WriteLine ("{");
                writer.WriteLine ("\t++g_counter;");
                writer.WriteLine ("\t_init ();");
                if (variant > 1)
                {
                    writer.WriteLine ("\tswitch (this->kind())");
                    writer.WriteLine ("\t{");
                    cmpProduction cmp = new cmpProduction ();
                    int caseIndex = 0;
                    foreach (RhsProduction p_RhsProductionRef in Productions)
                    {
                        ++caseIndex;
                        if (!cmp.Equals (p_RhsProduction, p_RhsProductionRef))
                            continue;
                        writer.WriteLine ("\tcase g_" + correctName + "_" + caseIndex + ":");
                        tokenIndex = 0;
                        foreach (RhsNode p_RhsNode in p_RhsProductionRef.rhsNodes)
                        {
                            SymbolToken p_SymbolToken = p_RhsNode.symbolToken;
                            string name = p_SymbolToken.correctName;
                            int index = p_RhsNode.index;
                            writer.Write ("\t\t(m_" + name);
                            if (index > 0)
                                writer.Write ($"_" + index);
                            writer.Write (" = ");
                            if (p_SymbolToken.declarator == (uint) AnglrClassificationType.TerminalName)
                            {
                                writer.Write ("p_token");
                                if (tokenIndex > 0)
                                    writer.Write ("_" + tokenIndex);
                                ++tokenIndex;
                            }
                            else
                            {
                                writer.Write ("p_" + name);
                                if (index > 0)
                                    writer.Write ($"_" + index);
                            }
                            writer.WriteLine (")->incUsage();");
                        }
                        writer.WriteLine ("\t\tbreak;");
                    }
                    writer.WriteLine ("\tdefault:");
                    writer.WriteLine ("\t\t{");
                    writer.WriteLine ("\t\t\tconst char* args[] = { \"" + correctName + "\", 0 };");
                    writer.WriteLine ("\t\t\tthrow new SyntaxTreeError (SyntaxTreeError::InvalidKindError, args);");
                    writer.WriteLine ("\t\t}");
                    writer.WriteLine ("\t}");
                }
                else
                {
                    tokenIndex = 0;
                    foreach (RhsNode p_RhsNode in p_RhsProduction.rhsNodes)
                    {
                        SymbolToken p_SymbolToken = p_RhsNode.symbolToken;
                        string name = p_SymbolToken.correctName;
                        int index = p_RhsNode.index;
                        writer.Write ("\t(m_" + name);
                        if (index > 0)
                            writer.Write ("_" + index);
                        writer.Write (" = ");
                        if (p_SymbolToken.declarator == (uint) AnglrClassificationType.TerminalName)
                        {
                            writer.Write ("p_token");
                            if (tokenIndex > 0)
                                writer.Write ("_" + tokenIndex);
                            ++tokenIndex;
                        }
                        else
                        {
                            writer.Write ("p_" + name);
                            if (index > 0)
                                writer.Write ("_" + index);
                        }
                        writer.WriteLine (")->incUsage();");
                    }
                }
                writer.WriteLine ("}");
            }
            writer.WriteLine ();
            writer.WriteLine ("// Copy constructor");
            writer.WriteLine ();
            writer.WriteLine (correctName + "::" + correctName + " (" + correctName + "& p_" + correctName + ") : SyntaxTreeBase (p_" + correctName + ".id(), p_" + correctName + ".kind())");
            writer.WriteLine ("{");
            writer.WriteLine ("\t++g_counter;");
            foreach (KeyValuePair<SymbolToken, symindex> keyval in m_symset)
            {
                SymbolToken p_SymbolToken = keyval.Key;
                string name = p_SymbolToken.correctName;
                int index = keyval.Value.second;
                if (p_SymbolToken.declarator == (uint) AnglrClassificationType.TerminalName)
                {
                    writer.WriteLine ("\tif ((m_" + name + " = (p_" + correctName + ".m_" + name + " != 0) ? new SyntaxTreeToken (*p_" + correctName + ".m_" + name + ") : 0) != 0) m_" + name + "->parent (this);");
                    for (int i = 0; i < index; ++i)
                    {
                        writer.WriteLine ("\tif ((m_" + name + "_" + (i + 1) + " = (p_" + correctName + ".m_" + name + "_" + (i + 1) + " != 0) ? new SyntaxTreeToken (*p_" + correctName + ".m_" + name + "_" + (i + 1) + ") : 0) != 0) m_" + name + "_" + (i + 1) + "->parent (this);");
                    }
                }
                else
                {
                    writer.WriteLine ("\tif ((m_" + name + " = (p_" + correctName + ".m_" + name + " != 0) ? new " + name + " (*p_" + correctName + ".m_" + name + ") : 0) != 0) m_" + name + "->parent (this);");
                    for (int i = 0; i < index; ++i)
                    {
                        writer.WriteLine ("\tif ((m_" + name + "_" + (i + 1) + " = (p_" + correctName + ".m_" + name + "_" + (i + 1) + " != 0) ? new " + name + " (*p_" + correctName + ".m_" + name + "_" + (i + 1) + ") : 0) != 0) m_" + name + "_" + (i + 1) + "->parent (this);");
                    }
                }
            }
            writer.WriteLine ("}");
            writer.WriteLine ();
            writer.WriteLine ("// Destructor");
            writer.WriteLine ();
            writer.WriteLine (correctName + "::~" + correctName + " ()");
            writer.WriteLine ("{");
            writer.WriteLine ("\t--g_counter;");
            foreach (KeyValuePair<SymbolToken, symindex> keyval in m_symset)
            {
                SymbolToken p_SymbolToken = keyval.Key;
                string name = p_SymbolToken.correctName;
                int index = keyval.Value.second;
                writer.WriteLine ("\tif (m_" + name + " != 0)");
                writer.WriteLine ("\tif (m_" + name + "->decUsage() <= 0)");
                writer.WriteLine ("\t{");
                writer.WriteLine ("\t\tdelete m_" + name + ";");
                writer.WriteLine ("\t\tm_" + name + " = 0;");
                writer.WriteLine ("\t}");
                writer.WriteLine ("\telse m_" + name + "->parent (0);");
                writer.WriteLine ();
                for (int i = 0; i < index; ++i)
                {
                    writer.WriteLine ("\tif (m_" + name + "_" + (i + 1) + " != 0)");
                    writer.WriteLine ("\tif (m_" + name + "_" + (i + 1) + "->decUsage() <= 0)");
                    writer.WriteLine ("\t{");
                    writer.WriteLine ("\t\tdelete m_" + name + "_" + (i + 1) + ";");
                    writer.WriteLine ("\t\tm_" + name + "_" + (i + 1) + " = 0;");
                    writer.WriteLine ("\t}");
                    writer.WriteLine ("\telse m_" + name + "_" + (i + 1) + "->parent (0);");
                    writer.WriteLine ();
                }
            }
            writer.WriteLine ("\t_init ();");
            writer.WriteLine ("}");
            writer.WriteLine ();
            writer.WriteLine ("// Consolidate parent nodes");
            writer.WriteLine ();
            writer.WriteLine ("void\t" + correctName + "::reparent" + " (SyntaxTreeBase* parent)");
            writer.WriteLine ("{");
            writer.WriteLine ("\tthis->parent (parent);");
            foreach (KeyValuePair<SymbolToken, symindex> keyval in m_symset)
            {
                SymbolToken p_SymbolToken = keyval.Key;
                string name = p_SymbolToken.correctName;
                int index = keyval.Value.second;
                writer.WriteLine ("\tif (m_" + name + " != 0)");
                writer.WriteLine ("\t\tm_" + name + "->reparent (this);");
                writer.WriteLine ();
                for (int i = 0; i < index; ++i)
                {
                    writer.WriteLine ("\tif (m_" + name + "_" + (i + 1) + " != 0)");
                    writer.WriteLine ("\t\tm_" + name + "_" + (i + 1) + "->reparent (this);");
                    writer.WriteLine ();
                }
            }
            writer.WriteLine ("}");
            writer.WriteLine ();
            writer.WriteLine ("//");
            writer.WriteLine ("// check if node 'element' is included within syntax tree associated with syntax rule " + ProductionName.name);
            writer.WriteLine ("//");
            writer.WriteLine ();
            writer.WriteLine ("bool " + correctName + "::checkInclusion (SyntaxTreeBase* element)");
            writer.WriteLine ("{");
            writer.WriteLine ("\treturn");
            writer.WriteLine ("\t\t(element == this) ||");
            foreach (KeyValuePair<SymbolToken, symindex> keyval in m_symset)
            {
                SymbolToken p_SymbolToken = keyval.Key;
                string name = p_SymbolToken.correctName;
                int index = keyval.Value.second;
                writer.WriteLine ("\t\t((m_" + name + " != 0) && m_" + name + "->checkInclusion (element)) ||");
                for (int i = 0; i < index; ++i)
                {
                    writer.WriteLine ("\t\t((m_" + name + "_" + (i + 1) + " != 0) && m_" + name + "_" + (i + 1) + "->checkInclusion (element)) ||");
                }
            }
            writer.WriteLine ("\t\tfalse;");
            writer.WriteLine ("}");
            writer.WriteLine ();
            writer.WriteLine ("//");
            writer.WriteLine ("// initialize object associated with syntax rule " + ProductionName.name);
            writer.WriteLine ("//");
            writer.WriteLine ();
            writer.WriteLine ("void " + correctName + "::_init ()");
            writer.WriteLine ("{");
            foreach (KeyValuePair<SymbolToken, symindex> keyval in m_symset)
            {
                SymbolToken p_SymbolToken = keyval.Key;
                string name = p_SymbolToken.correctName;
                int index = keyval.Value.second;
                writer.WriteLine ("\tm_" + name + " = 0;");
                for (int i = 0; i < index; ++i)
                    writer.WriteLine ("\tm_" + name + "_" + (i + 1) + " = 0;");
                ;
            }
            writer.WriteLine ("}");
            writer.WriteLine ();
            writer.WriteLine ("//");
            writer.WriteLine ("// traverse syntax tree node associated with any production of syntax rule " + ProductionName.name);
            writer.WriteLine ("//");
            writer.WriteLine ();
            writer.WriteLine ("void SyntaxTreeWalker::Traverse (" + correctName + "* p_" + correctName + ")");
            writer.WriteLine ("{");
            writer.WriteLine ("\tif (p_" + correctName + "->isLocked())");
            writer.WriteLine ("\t\treturn;");
            writer.WriteLine ("\tp_" + correctName + "->lock();");
            writer.WriteLine ("\t" + correctName + "::production_kind kind = (" + correctName + "::production_kind) p_" + correctName + "->kind();");
            writer.WriteLine ("\tp_" + correctName + "->turn_reset ();");
            writer.WriteLine ("\tif (Invoke_" + correctName + "_Callback (TraversalPrologueCallbackReason, kind, p_" + correctName + "))");
            writer.WriteLine ("\tswitch (kind)");
            writer.WriteLine ("\t{");

            foreach (RhsProduction p_RhsProduction in Productions)
            {
                writer.WriteLine ("\t\tcase " + correctName + "::g_" + correctName + "_" + p_RhsProduction.index + ":");
                writer.WriteLine ("\t\t// traverse syntax tree node associated with production");
                writer.Write ("\t\t// " + ProductionName.name + ":");
                p_RhsProduction.display (writer);
                writer.WriteLine ();
                int turn = 0;
                foreach (RhsNode p_RhsNode in p_RhsProduction.rhsNodes)
                {
                    SymbolToken p_SymbolToken = p_RhsNode.symbolToken;
                    if (p_SymbolToken.declarator != (uint) AnglrClassificationType.NonTerminalName)
                        continue;
                    if (turn > 0)
                        writer.WriteLine ("\t\t\tp_" + correctName + "->turn_inc ();");
                    writer.WriteLine ("\t\t\tif (Invoke_" + correctName + "_Callback (TraversalMidTermCallbackReason, kind, p_" + correctName + "))");
                    writer.Write ("\t\t\t\tTraverse (p_" + correctName + "->get_" + p_SymbolToken.correctName);
                    if (p_RhsNode.index > 0)
                        writer.Write ("_" + p_RhsNode.index);
                    writer.WriteLine ("());");
                    ++turn;
                }
                if (turn > 0)
                {
                    writer.WriteLine ("\t\t\tp_" + correctName + "->turn_inc ();");
                    writer.WriteLine ("\t\t\tInvoke_" + correctName + "_Callback (TraversalMidTermCallbackReason, kind, p_" + correctName + ");");
                }
                writer.WriteLine ("\t\tbreak;");
            }
            writer.WriteLine ("\t}");
            writer.WriteLine ("\tInvoke_" + correctName + "_Callback (TraversalEpilogueCallbackReason, kind, p_" + correctName + ");");
            writer.WriteLine ("\tsynlist& p_synlist = p_" + correctName + "->joinList();");
            writer.WriteLine ("\tif (false)");
            writer.WriteLine ("\tfor (synlist::iterator ptr = p_synlist.begin(); ptr != p_synlist.end(); ++ptr, ++SyntaxTreeBase::g_traverse)");
            writer.WriteLine ("\t\tTraverse ((" + correctName + "*) *ptr);");
            writer.WriteLine ("\tp_" + correctName + "->unlock();");
            writer.WriteLine ("}");
            writer.WriteLine ();
            writer.WriteLine ("//");
            writer.WriteLine ("// traverse syntax tree node associated with any production of syntax rule " + ProductionName.name);
            writer.WriteLine ("//");
            writer.WriteLine ();
            writer.WriteLine ("void SyntaxTreeWalker::TraverseCommon (" + correctName + "* p_" + correctName + ")");
            writer.WriteLine ("{");
            writer.WriteLine ("\t" + correctName + "::production_kind kind = (" + correctName + "::production_kind) p_" + correctName + "->kind();");
            writer.WriteLine ("\tif (Invoke__Common__Callback (TraversalPrologueCallbackReason, (int) kind, p_" + correctName + "))");
            writer.WriteLine ("\tswitch (kind)");
            writer.WriteLine ("\t{");

            foreach (RhsProduction p_RhsProduction in Productions)
            {
                writer.WriteLine ("\t\tcase " + correctName + "::g_" + correctName + "_" + p_RhsProduction.index + ":");
                writer.WriteLine ("\t\t// traverse syntax tree node associated with production");
                writer.Write ("\t\t// " + ProductionName.name + ":");
                p_RhsProduction.display (writer);
                writer.WriteLine ();
                int turn = -1;
                foreach (RhsNode p_RhsNode in p_RhsProduction.rhsNodes)
                {
                    ++turn;
                    SymbolToken p_SymbolToken = p_RhsNode.symbolToken;
                    writer.Write ("\t\t\tTraverseCommon (p_" + correctName + "->get_" + p_SymbolToken.correctName);
                    if (p_RhsNode.index > 0)
                        writer.Write ("_" + p_RhsNode.index);
                    writer.WriteLine ("());");
                }
                writer.WriteLine ("\t\tbreak;");
            }
            writer.WriteLine ("\t}");
            writer.WriteLine ("\tInvoke__Common__Callback (TraversalEpilogueCallbackReason, (int) kind, p_" + correctName + ");");
            writer.WriteLine ("}");
            writer.WriteLine ();
            writer.WriteLine ("//");
            writer.WriteLine ("// emit text string produced by syntax tree node associated with any production of syntax rule " + ProductionName.name);
            writer.WriteLine ("//");
            writer.WriteLine ();
            writer.WriteLine ("string " + correctName + "::Emit (int depth)");
            writer.WriteLine ("{");
            writer.WriteLine ("\tstring s = \"\";");
            writer.WriteLine ("\tif (depth != 0)");
            writer.WriteLine ("\tswitch (kind ())");
            writer.WriteLine ("\t{");

            foreach (RhsProduction p_RhsProduction in Productions)
            {
                writer.WriteLine ("\t\tcase g_" + correctName + "_" + p_RhsProduction.index + ":");
                writer.WriteLine ("\t\t// emit syntax tree node associated with production");
                writer.Write ("\t\t// " + ProductionName.name + ":");
                p_RhsProduction.display (writer);
                writer.WriteLine ();
                int turn = -1;
                foreach (RhsNode p_RhsNode in p_RhsProduction.rhsNodes)
                {
                    ++turn;
                    SymbolToken p_SymbolToken = p_RhsNode.symbolToken;
                    if (turn > 0)
                        writer.WriteLine ("\t\t\ts += ' ';");
                    writer.Write ("\t\t\ts += ");
                    if (p_SymbolToken.declarator == (uint) AnglrClassificationType.TerminalName)
                    {
                        writer.Write ("m_" + p_SymbolToken.correctName);
                        if (p_RhsNode.index > 0)
                            writer.Write ("_" + p_RhsNode.index);
                        writer.Write ("->Emit(depth - 1);");
                    }
                    else
                    {
                        writer.Write ("m_" + p_SymbolToken.correctName);
                        if (p_RhsNode.index > 0)
                            writer.Write ("_" + p_RhsNode.index);
                        writer.Write ("->Emit (depth - 1);");
                    }
                    writer.WriteLine ();
                }
                writer.WriteLine ("\t\tbreak;");
            }
            writer.WriteLine ("\t}");
            writer.WriteLine ("\treturn s;");
            writer.WriteLine ("}");

            writer.WriteLine ();
            writer.WriteLine ("//");
            writer.WriteLine ("// emit production tree node associated with any production of syntax rule " + ProductionName.name);
            writer.WriteLine ("//");
            writer.WriteLine ();
            writer.WriteLine ("string " + correctName + "::EmitProductionTree (int depth)");
            writer.WriteLine ("{");
            writer.WriteLine ("\tstring s = \"" + correctName + " (\";");
            writer.WriteLine ("\tif (depth != 0)");
            writer.WriteLine ("\tswitch (kind ())");
            writer.WriteLine ("\t{");

            foreach (RhsProduction p_RhsProduction in Productions)
            {
                writer.WriteLine ("\t\tcase g_" + correctName + "_" + p_RhsProduction.index + ":");
                writer.WriteLine ("\t\t// emit syntax tree node associated with production");
                writer.Write ("\t\t// " + ProductionName.name + ":");
                p_RhsProduction.display (writer);
                writer.WriteLine ();
                int turn = -1;
                foreach (RhsNode p_RhsNode in p_RhsProduction.rhsNodes)
                {
                    ++turn;
                    SymbolToken p_SymbolToken = p_RhsNode.symbolToken;
                    if (turn > 0)
                        writer.WriteLine ("\t\t\ts += ' ';");
                    writer.Write ("\t\t\ts += ");
                    if (p_SymbolToken.declarator == (uint) AnglrClassificationType.TerminalName)
                    {
                        writer.Write ("\"" + p_SymbolToken.correctName + "\";");
                    }
                    else
                    {
                        writer.Write ("m_" + p_SymbolToken.correctName);
                        if (p_RhsNode.index > 0)
                            writer.Write ("_" + p_RhsNode.index);
                        writer.Write ("->EmitProductionTree (depth - 1);");
                    }
                    writer.WriteLine ();
                }
                writer.WriteLine ("\t\tbreak;");
            }
            writer.WriteLine ("\t}");
            writer.WriteLine ("\ts += \")\";");
            writer.WriteLine ("\treturn s;");
            writer.WriteLine ("}");

            foreach (RhsProduction p_RhsProduction in Productions)
            {
                int variant = m_protoset [p_RhsProduction];
                int kind = p_RhsProduction.index;
                writer.WriteLine ();
                writer.WriteLine ("//");
                writer.WriteLine ("// create syntax tree node associated with production");
                writer.Write ("// " + ProductionName.name + ":");
                p_RhsProduction.display (writer);
                writer.WriteLine ();
                writer.WriteLine ("//");
                writer.WriteLine ();
                writer.Write (correctName + "* SyntaxTreeBuilder::" + correctName + "_" + kind + " (");
                string sep = "";
                int tokenIndex = 0;
                foreach (RhsNode p_RhsNode in p_RhsProduction.rhsNodes)
                {
                    SymbolToken p_SymbolToken = p_RhsNode.symbolToken;
                    string name = p_SymbolToken.correctName;
                    int index = p_RhsNode.index;
                    if (p_SymbolToken.declarator == (uint) AnglrClassificationType.TerminalName)
                    {
                        writer.Write (sep + "SyntaxTreeToken* p_token");
                        if (tokenIndex > 0)
                            writer.Write ("_" + tokenIndex);
                        ++tokenIndex;
                    }
                    else
                    {
                        writer.Write (sep + name + "* p_" + name);
                        if (index > 0)
                            writer.Write ("_" + index);
                    }
                    sep = ", ";
                }
                writer.WriteLine (")");
                writer.WriteLine ("{");
                writer.Write ("\t" + correctName + "* p_" + correctName + "_ref = new " + correctName + "(");
                sep = "";
                tokenIndex = 0;
                foreach (RhsNode p_RhsNode in p_RhsProduction.rhsNodes)
                {
                    SymbolToken p_SymbolToken = p_RhsNode.symbolToken;
                    string name = p_SymbolToken.correctName;
                    int index = p_RhsNode.index;
                    if (p_SymbolToken.declarator == (uint) AnglrClassificationType.TerminalName)
                    {
                        writer.Write (sep + "p_token");
                        if (tokenIndex > 0)
                            writer.Write ("_" + tokenIndex);
                        ++tokenIndex;
                    }
                    else
                    {
                        writer.Write (sep + "p_" + name);
                        if (index > 0)
                            writer.Write ("_" + index);
                    }
                    sep = ", ";
                }
                if (variant > 1)
                    writer.Write (sep + kind);
                writer.WriteLine (");");
                tokenIndex = 0;
                foreach (RhsNode p_RhsNode in p_RhsProduction.rhsNodes)
                {
                    SymbolToken p_SymbolToken = p_RhsNode.symbolToken;
                    string name = p_SymbolToken.correctName;
                    int index = p_RhsNode.index;
                    if (p_SymbolToken.declarator == (uint) AnglrClassificationType.TerminalName)
                    {
                        writer.Write ("\tp_token");
                        if (tokenIndex > 0)
                            writer.Write ("_" + tokenIndex);
                        ++tokenIndex;
                    }
                    else
                    {
                        writer.Write ("\tp_" + name);
                        if (index > 0)
                            writer.Write ("_" + index);
                    }
                    writer.WriteLine ("->parent (p_" + correctName + "_ref);");
                }
                writer.WriteLine ("\tInvoke_" + correctName + "_Callback (BuilderCallbackReason, " + correctName + "::g_" + correctName + "_" + kind + ", p_" + correctName + "_ref);");
                writer.WriteLine ("\treturn p_" + correctName + "_ref;");
                writer.WriteLine ("}");
            }
        }

        internal void GenerateCppCallbackPrototypes (TextWriter writer)
        {
            int cnt = 0;
            foreach (RhsProduction p_RhsProduction in Productions)
            {
                ++cnt;
                string correctName = ProductionName.correctName;
                writer.Write ("\t" + correctName + "* " + correctName + "_" + cnt + " (");
                string sep = "";
                int tokenIndex = 0;
                foreach (RhsNode p_RhsNode in p_RhsProduction.rhsNodes)
                {
                    SymbolToken p_SymbolToken = p_RhsNode.symbolToken;
                    string name = p_SymbolToken.correctName;
                    int index = p_RhsNode.index;
                    if (p_SymbolToken.declarator == (uint) AnglrClassificationType.TerminalName)
                    {
                        writer.Write (sep + "SyntaxTreeToken* p_token");
                        if (tokenIndex > 0)
                            writer.Write ("_" + tokenIndex);
                        ++tokenIndex;
                    }
                    else
                    {
                        writer.Write (sep + name + "* p_" + name);
                        if (index > 0)
                            writer.Write ("_" + index);
                    }
                    sep = ", ";
                }
                writer.WriteLine (");");
            }
        }

        internal void GenerateCsHeaderFile (TextWriter writer)
        {
            string correctName = ProductionName.correctName;

            GenerateCSClassHeader (writer, correctName);
            GenerateCSProductionsEnums (writer, correctName);
            writer.WriteLine ($"\t\t#region production markers associated with the syntax rule {ProductionName.name}");
            GenerateCSMarkers (writer, correctName);
            writer.WriteLine ($"\t\t#endregion");
            writer.WriteLine ($"\t\t#region Constructors and destructors associated with the syntax rule {ProductionName.name}");
            GenerateCSConstructors (writer, correctName);
            GenerateCSCopyConstructor (writer, correctName);
            GenerateCSCloneMethod (writer, correctName);
            GenerateCSDestructor (writer);
            writer.WriteLine ($"\t\t#endregion");
            writer.WriteLine ($"\t\t#region Content changing and inclusion checking methods associated with the syntax rule {ProductionName.name}");
            GenerateCSChangeMethod (writer, correctName);
            GenerateCSCheckInclusionMethod (writer);
            writer.WriteLine ($"\t\t#endregion");
            writer.WriteLine ($"\t\t#region Syntax tree emit methods associated with the syntax rule {ProductionName.name}");
            GenerateCSEmitMethod (writer);
            GenerateCSEmitProductionMethod (writer, correctName);
            writer.WriteLine ($"\t\t#endregion");
            writer.WriteLine ($"\t\t#region Syntax tree traversal,  reperrent, init and iterator methods associated with the syntax rule {ProductionName.name}");
            GenerateCSInvokeTraverseMethod (writer);
            GenerateCSReparentMethod (writer);
            GenerateCSInitMethod (writer, correctName);
            GenerateCSIteratorMethods (writer, correctName);
            writer.WriteLine ($"\t\t#endregion");
            writer.WriteLine ($"\t\t#region Fields and properties associated with the syntax rule {ProductionName.name}");
            GenerateCSClassMembers (writer, correctName);
            writer.WriteLine ($"\t\t#endregion");
            GenerateCSClassFooter (writer);
            GenerateCSSyntaxTreeWalkerClass (writer, correctName);
            GenerateCSSyntaxTreeBuilderClass (writer, correctName);
        }

        private void GenerateCSClassHeader (TextWriter writer, string correctName)
        {
            writer.WriteLine ("\t//");
            writer.WriteLine ("\t// class associated with syntax rule " + ProductionName.name);
            writer.WriteLine ("\t//");
            writer.WriteLine ();
            writer.WriteLine ("\tpublic class\t" + correctName + " : SyntaxTreeBase");
            writer.WriteLine ("\t{");
        }

        private void GenerateCSProductionsEnums (TextWriter writer, string correctName)
        {
            int cnt = 0;
            writer.WriteLine ($"\t\t#region enumerated production(s) of syntax rule {ProductionName.name}");
            writer.WriteLine ("\t\tpublic enum production_kind : ushort\t// enumerated production(s) of syntax rule " + ProductionName.name);
            writer.WriteLine ("\t\t{");
            foreach (RhsProduction p_RhsProduction in Productions)
            {
                ++cnt;
                writer.Write ("\t\t\tg_" + correctName + "_" + cnt + " = " + cnt + ",\t//");
                p_RhsProduction.display (writer);
                writer.WriteLine ();
            }
            writer.WriteLine ("\t\t};");
            writer.WriteLine ("\t\t#endregion");
        }

        private void GenerateCSMarkers (TextWriter writer, string correctName)
        {
            int index = 0;
            foreach (RhsProduction rhsProduction in Productions)
            {
                writer.WriteLine ();
                writer.Write ($"\t\t// markers associated with production: {ProductionName.name} ->");
                rhsProduction.display (writer);
                writer.WriteLine ();
                writer.WriteLine ($"\t\tpublic enum production_marker_{++index} : ushort");
                writer.WriteLine ($"\t\t{{");
                string sep = " ";
                foreach (RhsNode rhsNode in rhsProduction.rhsNodes)
                {
                    SymbolToken p_SymbolToken = rhsNode.symbolToken;
                    if (p_SymbolToken.declarator == (uint) AnglrClassificationType.TerminalName)
                        continue;
                    string name = p_SymbolToken.correctName;
                    int rhsIndex = rhsNode.index;
                    writer.Write ($"\t\t\tm_{name}");
                    if (rhsIndex > 0)
                        writer.Write ($"_{rhsIndex}");
                    writer.WriteLine ($",");
                }
                writer.WriteLine ($"\t\t\tfinal");
                writer.WriteLine ($"\t\t}};");
            }

            return;

            writer.WriteLine ();
            writer.WriteLine ($"\t\tpublic static readonly ushort [] [] markers = new ushort [] []");
            writer.WriteLine ($"\t\t{{");
            writer.WriteLine ($"\t\t\tnew ushort [] {{ }},");
            index = 0;
            foreach (RhsProduction rhsProduction in Productions)
            {
                ++index;
                writer.Write ($"\t\t\tnew ushort [] {{");
                foreach (RhsNode rhsNode in rhsProduction.rhsNodes)
                {
                    SymbolToken p_SymbolToken = rhsNode.symbolToken;
                    if (p_SymbolToken.declarator == (uint) AnglrClassificationType.TerminalName)
                        continue;
                    string name = p_SymbolToken.correctName;
                    int rhsIndex = rhsNode.index;
                    writer.Write ($" (ushort) p{index}.m_{name}");
                    if (rhsIndex > 0)
                        writer.Write ($"_{rhsIndex}");
                    writer.Write ($",");
                }
                writer.WriteLine ($" (ushort) p{index}.final }},");
            }
            writer.WriteLine ($"\t\t}};");
        }

        private void GenerateCSConstructors (TextWriter writer, string correctName)
        {
            writer.WriteLine ();
            writer.WriteLine ($"\t\t// Constructor declaration(s) associated with production(s) of syntax rule {ProductionName.name}");
            foreach (KeyValuePair<RhsProduction, int> keyval in m_protoset)
            {
                SymbolToken iteratorsymbol = null;
                RhsProduction p_RhsProduction = keyval.Key;
                writer.WriteLine ();
                writer.WriteLine ($"\t\t//");
                writer.WriteLine ($"\t\t// Constructor associated with the following production(s)");
                if (keyval.Value > 1)
                {
                    cmpProduction cmp = new cmpProduction ();
                    foreach (RhsProduction p_RhsProductionRef in Productions)
                    {
                        if (!cmp.Equals (p_RhsProduction, p_RhsProductionRef))
                            continue;
                        writer.Write ($"\t\t// {ProductionName.name} ->");
                        p_RhsProductionRef.display (writer);
                        writer.WriteLine ();
                    }
                }
                else
                {
                    writer.Write ($"\t\t// {ProductionName.name} ->");
                    p_RhsProduction.display (writer);
                    writer.WriteLine ();
                }
                writer.WriteLine ($"\t\t//");
                writer.WriteLine ();
                writer.Write ($"\t\tpublic {correctName} (");
                string sep = "";
                int tokenIndex = 0;
                foreach (RhsNode p_RhsNode in p_RhsProduction.rhsNodes)
                {
                    SymbolToken p_SymbolToken = p_RhsNode.symbolToken;
                    if ((p_SymbolToken.IteratorFlag) && (p_SymbolToken.name == ProductionName.name))
                        iteratorsymbol = p_SymbolToken;
                    string name = p_SymbolToken.correctName;
                    int index = p_RhsNode.index;
                    writer.Write (sep);
                    if (p_SymbolToken.declarator == (uint) AnglrClassificationType.TerminalName)
                    {
                        writer.Write ($"SyntaxTreeToken p_token");
                        if (tokenIndex > 0)
                            writer.Write ($"_{tokenIndex}");
                        ++tokenIndex;
                    }
                    else
                    {
                        writer.Write ($"{name} p_{name}");
                        if (index > 0)
                            writer.Write ($"_{index}");
                    }
                    sep = ", ";
                }
                if (keyval.Value > 1)
                    writer.Write ($"{sep}int kind");
                writer.Write ($") : base (");
                writer.Write ($"(uint) ProductionID._{correctName}_ID, (uint) ");
                if (keyval.Value > 1)
                    writer.Write ($"kind");
                else
                    writer.Write ($"production_kind.g_{correctName}_{p_RhsProduction.index}");
                writer.WriteLine ($")");
                writer.WriteLine ($"\t\t{{");
                writer.WriteLine ($"\t\t\t++g_counter;");
                writer.WriteLine ($"\t\t\t_init ();");
                if (keyval.Value > 1)
                {
                    writer.WriteLine ($"\t\t\tswitch ((production_kind) this.kind)");
                    writer.WriteLine ($"\t\t\t{{");
                    cmpProduction cmp = new cmpProduction ();
                    int caseIndex = 0;
                    foreach (RhsProduction p_RhsProductionRef in Productions)
                    {
                        ++caseIndex;
                        if (!cmp.Equals (p_RhsProduction, p_RhsProductionRef))
                            continue;
                        writer.WriteLine ($"\t\t\tcase production_kind.g_{correctName}_{caseIndex}:");
                        tokenIndex = 0;
                        int childCount = p_RhsProductionRef.rhsNodes.Count;
                        if (childCount > 0)
                            writer.WriteLine ($"\t\t\t\tchildren = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), {childCount});");
                        else
                            writer.WriteLine ($"\t\t\t\tchildren = Array.Empty <SyntaxTreeBase> ();");
                        int childCounter = 0;
                        foreach (RhsNode p_RhsNode in p_RhsProductionRef.rhsNodes)
                        {
                            SymbolToken p_SymbolToken = p_RhsNode.symbolToken;
                            string name = p_SymbolToken.correctName;
                            int index = p_RhsNode.index;
                            writer.Write ($"\t\t\t\tchildren[{childCounter++}] = m_{name}");
                            if (index > 0)
                                writer.Write ($"_{index}");
                            writer.Write ($" = ");
                            if (p_SymbolToken.declarator == (uint) AnglrClassificationType.TerminalName)
                            {
                                writer.Write ($"p_token");
                                if (tokenIndex > 0)
                                    writer.Write ($"_{tokenIndex}");
                                ++tokenIndex;
                            }
                            else
                            {
                                writer.Write ($"p_{name}");
                                if (index > 0)
                                    writer.Write ($"_{index}");
                            }
                            writer.WriteLine ($";");
                        }
                        writer.WriteLine ($"\t\t\t\tbreak;");
                    }
                    writer.WriteLine ($"\t\t\tdefault:");
                    writer.WriteLine ($"\t\t\t\t{{");
                    writer.WriteLine ($"\t\t\t\t\tstring[] args = new string[] {{ \"{correctName}\" }};");
                    writer.WriteLine ($"\t\t\t\t\tthrow new SyntaxTreeError (SyntaxTreeError.InvalidKindError, args);");
                    writer.WriteLine ($"\t\t\t\t}}");
                    writer.WriteLine ($"\t\t\t}}");
                }
                else
                {
                    tokenIndex = 0;
                    int childCount = p_RhsProduction.rhsNodes.Count;
                    if (childCount > 0)
                        writer.WriteLine ($"\t\t\tchildren = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), {childCount});");
                    else
                        writer.WriteLine ($"\t\t\tchildren = Array.Empty <SyntaxTreeBase> ();");
                    int childCounter = 0;
                    foreach (RhsNode p_RhsNode in p_RhsProduction.rhsNodes)
                    {
                        SymbolToken p_SymbolToken = p_RhsNode.symbolToken;
                        string name = p_SymbolToken.correctName;
                        int index = p_RhsNode.index;
                        writer.Write ($"\t\t\tchildren[{childCounter++}] = m_{name}");
                        if (index > 0)
                            writer.Write ($"_{index}");
                        writer.Write ($" = ");
                        if (p_SymbolToken.declarator == (uint) AnglrClassificationType.TerminalName)
                        {
                            writer.Write ($"p_token");
                            if (tokenIndex > 0)
                                writer.Write ($"_{tokenIndex}");
                            ++tokenIndex;
                        }
                        else
                        {
                            writer.Write ($"p_{name}");
                            if (index > 0)
                                writer.Write ($"_{index}");
                        }
                        writer.WriteLine ($";");
                    }
                }
                writer.WriteLine ($"\t\t}}");
            }
        }

        private void GenerateCSCopyConstructor (TextWriter writer, string correctName)
        {
            writer.WriteLine ();
            writer.WriteLine ($"\t\t// Copy constructor");
            writer.WriteLine ();
            writer.WriteLine ($"\t\tpublic {correctName} ({correctName} p_{correctName}) : base (p_{correctName}.id, p_{correctName}.kind)");
            writer.WriteLine ($"\t\t{{");
            writer.WriteLine ($"\t\t\t++g_counter;");
            writer.WriteLine ($"\t\t\t_init ();");
            writer.WriteLine ($"\t\t\tswitch ((production_kind) p_{correctName}.kind)");
            writer.WriteLine ($"\t\t\t{{");
            foreach (KeyValuePair<RhsProduction, int> keyval in m_protoset)
            {
                RhsProduction p_RhsProduction = keyval.Key;
                //if (keyval.Value > 1)
                {
                    cmpProduction cmp = new cmpProduction ();
                    int caseIndex = 0;
                    foreach (RhsProduction p_RhsProductionRef in Productions)
                    {
                        ++caseIndex;
                        if (!cmp.Equals (p_RhsProduction, p_RhsProductionRef))
                            continue;
                        writer.WriteLine ($"\t\t\t\tcase production_kind.g_{correctName}_{caseIndex}:");
                        int childCounter = 0;
                        int childCount = p_RhsProductionRef.rhsNodes.Count;
                        if (childCount > 0)
                            writer.WriteLine ($"\t\t\t\t\tchildren = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), {childCount});");
                        else
                            writer.WriteLine ($"\t\t\t\t\tchildren = Array.Empty <SyntaxTreeBase> ();");
                        foreach (RhsNode p_RhsNode in p_RhsProductionRef.rhsNodes)
                        {
                            SymbolToken p_SymbolToken = p_RhsNode.symbolToken;
                            string name = p_SymbolToken.correctName;
                            int index = p_RhsNode.index;
                            writer.Write ($"\t\t\t\t\tif ((children [{childCounter++}] = m_{name}");
                            if (index > 0)
                                writer.Write ($"_{index}");
                            writer.Write ($" = (p_{correctName}.m_{name}");
                            if (index > 0)
                                writer.Write ($"_{index}");
                            writer.Write ($" != null) ? new ");
                            if (p_SymbolToken.declarator != (uint) AnglrClassificationType.TerminalName)
                                writer.Write ($"{name}");
                            else
                                writer.Write ($"SyntaxTreeToken");
                            writer.Write ($" (p_{correctName}.m_{name}");
                            if (index > 0)
                                writer.Write ($"_{index}");
                            writer.Write ($") : null) != null) m_{name}");
                            if (index > 0)
                                writer.Write ($"_{index}");
                            writer.WriteLine ($".parent = this;");
                        }
                        writer.WriteLine ($"\t\t\t\t\tbreak;");
                    }
                }
            }
            writer.WriteLine ($"\t\t\t\tdefault:");
            writer.WriteLine ($"\t\t\t\t{{");
            writer.WriteLine ($"\t\t\t\t\tstring[] args = new string[] {{ \"{correctName}\" }};");
            writer.WriteLine ($"\t\t\t\t\tthrow new SyntaxTreeError (SyntaxTreeError.InvalidKindError, args);");
            writer.WriteLine ($"\t\t\t\t}}");
            writer.WriteLine ($"\t\t\t}}");
            writer.WriteLine ($"\t\t}}");
        }

        private static void GenerateCSCloneMethod (TextWriter writer, string correctName)
        {
            writer.WriteLine ();
            writer.WriteLine ("\t\t// Clone node - wrapper of copy constructor");
            writer.WriteLine ();
            writer.WriteLine ("\t\tpublic override SyntaxTreeBase Clone () {" + " return (SyntaxTreeBase) new " + correctName + " (this); }");
        }

        private void GenerateCSDestructor (TextWriter writer)
        {
            if (ProductionName.IteratorFlag)
            {
                writer.WriteLine ();
                writer.WriteLine ($"\t\t// Destructor");
                writer.WriteLine ();
                writer.WriteLine ($"\t\tpublic new void Dispose ()");
                writer.WriteLine ($"\t\t{{");
                writer.WriteLine ($"\t\t\tIterate (null, (node, appData) =>");
                writer.WriteLine ($"\t\t\t{{");
                writer.WriteLine ($"\t\t\t\t--g_counter;");
                writer.WriteLine ();
                foreach (KeyValuePair<SymbolToken, symindex> keyval in m_symset)
                {
                    SymbolToken p_SymbolToken = keyval.Key;
                    if ((p_SymbolToken.IteratorFlag) && (p_SymbolToken.name == ProductionName.name))
                        continue;
                    string name = p_SymbolToken.correctName;
                    int index = keyval.Value.second;
                    writer.WriteLine ($"\t\t\t\tnode.m_{name}?.Dispose ();");
                    for (int i = 0; i < index; ++i)
                    {
                        writer.WriteLine ($"\t\t\t\tnode.m_{name}_{(i + 1)}?.Dispose ();");
                    }
                }
                writer.WriteLine ();
                writer.WriteLine ($"\t\t\t\tnode._init ();");
                writer.WriteLine ();
                writer.WriteLine ($"\t\t\t\treturn null;");
                writer.WriteLine ($"\t\t\t}});");
                writer.WriteLine ($"\t\t}}");
            }
            else
            {
                writer.WriteLine ();
                writer.WriteLine ($"\t\t// Destructor");
                writer.WriteLine ();
                writer.WriteLine ($"\t\tpublic new void Dispose ()");
                writer.WriteLine ($"\t\t{{");
                writer.WriteLine ($"\t\t\tbase.Dispose ();");
                writer.WriteLine ($"\t\t\t--g_counter;");
                writer.WriteLine ();
                foreach (KeyValuePair<SymbolToken, symindex> keyval in m_symset)
                {
                    SymbolToken p_SymbolToken = keyval.Key;
                    string name = p_SymbolToken.correctName;
                    int index = keyval.Value.second;
                    writer.WriteLine ($"\t\t\tm_{name}?.Dispose ();");
                    for (int i = 0; i < index; ++i)
                    {
                        writer.WriteLine ($"\t\t\tm_{name}_{(i + 1)}?.Dispose ();");
                    }
                }
                writer.WriteLine ();
                writer.WriteLine ($"\t\t\t_init ();");
                writer.WriteLine ($"\t\t}}");
            }
        }

        private void GenerateCSChangeMethod (TextWriter writer, string correctName)
        {
            writer.WriteLine ();
            writer.WriteLine ("\t\t// Content changing function(s) associated with production(s) of syntax rule " + ProductionName.name);
            foreach (KeyValuePair<RhsProduction, int> keyval in m_protoset)
            {
                RhsProduction p_RhsProduction = keyval.Key;
                writer.WriteLine ();
                writer.WriteLine ("\t\t//");
                writer.WriteLine ("\t\t// Content changing function associated with following production(s)");
                if (keyval.Value > 1)
                {
                    cmpProduction cmp = new cmpProduction ();
                    foreach (RhsProduction p_RhsProductionRef in Productions)
                    {
                        if (!cmp.Equals (p_RhsProduction, p_RhsProductionRef))
                            continue;
                        writer.Write ("\t\t// " + ProductionName.name + " ->");
                        p_RhsProductionRef.display (writer);
                        writer.WriteLine ();
                    }
                }
                else
                {
                    writer.Write ("\t\t// " + ProductionName.name + " ->");
                    p_RhsProduction.display (writer);
                    writer.WriteLine ();
                }
                writer.WriteLine ("\t\t//");
                writer.WriteLine ();
                writer.Write ("\t\tpublic void change(");
                string sep = "";
                int tokenIndex = 0;
                foreach (RhsNode p_RhsNode in p_RhsProduction.rhsNodes)
                {
                    SymbolToken p_SymbolToken = p_RhsNode.symbolToken;
                    string name = p_SymbolToken.correctName;
                    int index = p_RhsNode.index;
                    writer.Write (sep);
                    if (p_SymbolToken.declarator == (uint) AnglrClassificationType.TerminalName)
                    {
                        writer.Write ("SyntaxTreeToken p_token");
                        if (tokenIndex > 0)
                            writer.Write ("_" + tokenIndex);
                        ++tokenIndex;
                    }
                    else
                    {
                        writer.Write (name + " p_" + name);
                        if (index > 0)
                            writer.Write ("_" + index);
                    }
                    sep = ", ";
                }
                if (keyval.Value > 1)
                    writer.Write (sep + "int kind");
                writer.WriteLine (")");
                writer.WriteLine ("\t\t{");
                writer.WriteLine ("\t\t\t_init ();");
                writer.Write ("\t\t\tthis.kind = (uint) ");
                if (keyval.Value > 1)
                    writer.Write ("kind");
                else
                    writer.Write ("production_kind.g_" + correctName + "_" + p_RhsProduction.index);
                writer.WriteLine (";");
                if (keyval.Value > 1)
                {
                    writer.WriteLine ("\t\t\tswitch ((production_kind) this.kind)");
                    writer.WriteLine ("\t\t\t{");
                    cmpProduction cmp = new cmpProduction ();
                    int caseIndex = 0;
                    foreach (RhsProduction p_RhsProductionRef in Productions)
                    {
                        ++caseIndex;
                        if (!cmp.Equals (p_RhsProduction, p_RhsProductionRef))
                            continue;
                        writer.WriteLine ("\t\t\tcase production_kind.g_" + correctName + "_" + caseIndex + ":");
                        tokenIndex = 0;
                        int childCounter = 0;
                        int childCount = p_RhsProductionRef.rhsNodes.Count;
                        if (childCount > 0)
                            writer.WriteLine ($"\t\t\t\tchildren = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), {childCount});");
                        else
                            writer.WriteLine ($"\t\t\t\tchildren = Array.Empty <SyntaxTreeBase> ();");
                        foreach (RhsNode p_RhsNode in p_RhsProductionRef.rhsNodes)
                        {
                            SymbolToken p_SymbolToken = p_RhsNode.symbolToken;
                            string name = p_SymbolToken.correctName;
                            int index = p_RhsNode.index;
                            writer.Write ($"\t\t\t\tchildren [{childCounter++}] = m_{name}");
                            if (index > 0)
                                writer.Write ("_" + index);
                            writer.Write (" = ");
                            if (p_SymbolToken.declarator == (uint) AnglrClassificationType.TerminalName)
                            {
                                writer.Write ("p_token");
                                if (tokenIndex > 0)
                                    writer.Write ("_" + tokenIndex);
                                ++tokenIndex;
                            }
                            else
                            {
                                writer.Write ("p_" + name);
                                if (index > 0)
                                    writer.Write ("_" + index);
                            }
                            writer.WriteLine (";");
                        }
                        writer.WriteLine ("\t\t\t\tbreak;");
                    }
                    writer.WriteLine ("\t\t\tdefault:");
                    writer.WriteLine ("\t\t\t\t{");
                    writer.WriteLine ("\t\t\t\t\tstring[] args = new string[] { \"" + correctName + "\" };");
                    writer.WriteLine ("\t\t\t\t\tthrow new SyntaxTreeError (SyntaxTreeError.InvalidKindError, args);");
                    writer.WriteLine ("\t\t\t\t}");
                    writer.WriteLine ("\t\t\t}");
                }
                else
                {
                    tokenIndex = 0;
                    int childCounter = 0;
                    int childCount = p_RhsProduction.rhsNodes.Count;
                    if (childCount > 0)
                        writer.WriteLine ($"\t\t\tchildren = (SyntaxTreeBase []) Array.CreateInstance (typeof (SyntaxTreeBase), {childCount});");
                    else
                        writer.WriteLine ($"\t\t\tchildren = Array.Empty <SyntaxTreeBase> ();");
                    foreach (RhsNode p_RhsNode in p_RhsProduction.rhsNodes)
                    {
                        SymbolToken p_SymbolToken = p_RhsNode.symbolToken;
                        string name = p_SymbolToken.correctName;
                        int index = p_RhsNode.index;
                        writer.Write ($"\t\t\tchildren [{childCounter++}] = m_{name}");
                        if (index > 0)
                            writer.Write ("_" + index);
                        writer.Write (" = ");
                        if (p_SymbolToken.declarator == (uint) AnglrClassificationType.TerminalName)
                        {
                            writer.Write ("p_token");
                            if (tokenIndex > 0)
                                writer.Write ("_" + tokenIndex);
                            ++tokenIndex;
                        }
                        else
                        {
                            writer.Write ("p_" + name);
                            if (index > 0)
                                writer.Write ("_" + index);
                        }
                        writer.WriteLine (";");
                    }
                }
                writer.WriteLine ("\t\t\treparent (parent);");
                writer.WriteLine ("\t\t}");
            }
        }

        private void GenerateCSCheckInclusionMethod (TextWriter writer)
        {
            if (ProductionName.IteratorFlag)
            {
                writer.WriteLine ();
                writer.WriteLine ($"\t\t//");
                writer.WriteLine ($"\t\t// check if node 'element' is included within syntax tree associated with syntax rule {ProductionName.name}");
                writer.WriteLine ($"\t\t//");
                writer.WriteLine ();
                writer.WriteLine ($"\t\tpublic new bool checkInclusion (SyntaxTreeBase element)");
                writer.WriteLine ($"\t\t{{");
                writer.WriteLine ($"\t\t\treturn (element == this) || (bool) Iterate (false, (node, appData) =>");
                writer.WriteLine ($"\t\t\t{{");
                writer.WriteLine ($"\t\t\t\treturn");
                writer.WriteLine ($"\t\t\t\t\t(bool) appData ||");
                foreach (KeyValuePair<SymbolToken, symindex> keyval in m_symset)
                {
                    SymbolToken p_SymbolToken = keyval.Key;
                    if ((p_SymbolToken.IteratorFlag) && (p_SymbolToken.name == ProductionName.name))
                        continue;
                    string name = p_SymbolToken.correctName;
                    int index = keyval.Value.second;
                    writer.WriteLine ($"\t\t\t\t\t(node.m_{name} != null) && node.m_{name}.checkInclusion (element) ||");
                    for (int i = 0; i < index; ++i)
                    {
                        writer.WriteLine ($"\t\t\t\t(node.m_{name}_{(i + 1)} != null) && node.m_{name}_{(i + 1)}.checkInclusion (element) ||");
                    }
                }
                writer.WriteLine ($"\t\t\t\t\tfalse;");
                writer.WriteLine ($"\t\t\t}});");
                writer.WriteLine ($"\t\t}}");
            }
            else
            {
                writer.WriteLine ();
                writer.WriteLine ($"\t\t//");
                writer.WriteLine ($"\t\t// check if node 'element' is included within syntax tree associated with syntax rule {ProductionName.name}");
                writer.WriteLine ($"\t\t//");
                writer.WriteLine ();
                writer.WriteLine ($"\t\tpublic new bool checkInclusion (SyntaxTreeBase element)");
                writer.WriteLine ($"\t\t{{");
                writer.WriteLine ($"\t\t\treturn");
                writer.WriteLine ($"\t\t\t\t(element == this) ||");
                foreach (KeyValuePair<SymbolToken, symindex> keyval in m_symset)
                {
                    SymbolToken p_SymbolToken = keyval.Key;
                    string name = p_SymbolToken.correctName;
                    int index = keyval.Value.second;
                    writer.WriteLine ($"\t\t\t\t(m_{name} != null) && m_{name}.checkInclusion (element) ||");
                    for (int i = 0; i < index; ++i)
                    {
                        writer.WriteLine ($"\t\t\t\t(m_{name}_{(i + 1)} != null) && m_{name}_{(i + 1)}.checkInclusion (element) ||");
                    }
                }
                writer.WriteLine ($"\t\t\t\tfalse;");
                writer.WriteLine ($"\t\t}}");
            }
        }

        private void GenerateCSEmitMethod (TextWriter writer)
        {
            writer.WriteLine ();
            writer.WriteLine ($"\t\t//");
            writer.WriteLine ($"\t\t// emit production tree node associated with any production of syntax rule {ProductionName.name}");
            writer.WriteLine ($"\t\t//");
            writer.WriteLine ();
            writer.WriteLine ($"\t\tpublic override string Emit (int depth)");
            if (ProductionName.IteratorFlag)
            //if (false)
            {
                writer.WriteLine ($"\t\t{{");
                writer.WriteLine ($"\t\t\tstring s = \"\";");
                writer.WriteLine ($"\t\t\tif (depth != 0)");
                writer.WriteLine ($"\t\t\t\tIterate (null, (node, appData) =>");
                writer.WriteLine ($"\t\t\t\t{{");
                writer.WriteLine ($"\t\t\t\t\tswitch ((production_kind) node.kind)");
                writer.WriteLine ($"\t\t\t\t\t{{");

                foreach (RhsProduction p_RhsProduction in Productions)
                {
                    writer.WriteLine ($"\t\t\t\t\tcase production_kind.g_{ProductionName.correctName}_{p_RhsProduction.index}:");
                    writer.WriteLine ($"\t\t\t\t\t\t// emit syntax tree node associated with production");
                    writer.Write ($"\t\t\t\t\t\t// {ProductionName.name}:");
                    p_RhsProduction.display (writer);
                    writer.WriteLine ();
                    foreach (RhsNode p_RhsNode in p_RhsProduction.rhsNodes)
                    {
                        SymbolToken p_SymbolToken = p_RhsNode.symbolToken;
                        if ((p_SymbolToken.IteratorFlag) && (p_SymbolToken.name == ProductionName.name))
                            continue;
                        writer.Write ($"\t\t\t\t\t\ts += \" \" + node.");
                        if (p_SymbolToken.declarator == (uint) AnglrClassificationType.TerminalName)
                        {
                            writer.Write ($"m_{p_SymbolToken.correctName}");
                            if (p_RhsNode.index > 0)
                                writer.Write ($"_{p_RhsNode.index}");
                            writer.Write ($".Emit (depth - 1);");
                        }
                        else
                        {
                            writer.Write ($"m_{p_SymbolToken.correctName}");
                            if (p_RhsNode.index > 0)
                                writer.Write ($"_{p_RhsNode.index}");
                            writer.Write ($".Emit (depth - 1);");
                        }
                        writer.WriteLine ();
                    }
                    writer.WriteLine ($"\t\t\t\t\t\tbreak;");
                }
                writer.WriteLine ($"\t\t\t\t\t}}");
                writer.WriteLine ($"\t\t\t\t\treturn null;");
                writer.WriteLine ($"\t\t\t\t}});");
                writer.WriteLine ($"\t\t\treturn s.Trim ();");
                writer.WriteLine ($"\t\t}}");
            }
            else
            {
                writer.WriteLine ($"\t\t{{");
                writer.WriteLine ($"\t\t\tstring s = \"\";");
                writer.WriteLine ($"\t\t\tif (depth != 0)");
                writer.WriteLine ($"\t\t\tswitch ((production_kind) kind)");
                writer.WriteLine ($"\t\t\t{{");

                foreach (RhsProduction p_RhsProduction in Productions)
                {
                    writer.WriteLine ($"\t\t\t\tcase production_kind.g_{ProductionName.correctName}_{p_RhsProduction.index}:");
                    writer.WriteLine ($"\t\t\t\t\t// emit syntax tree node associated with production");
                    writer.Write ($"\t\t\t\t\t// {ProductionName.name}:");
                    p_RhsProduction.display (writer);
                    writer.WriteLine ();
                    int index = 0;
                    foreach (RhsNode p_RhsNode in p_RhsProduction.rhsNodes)
                    {
                        SymbolToken p_SymbolToken = p_RhsNode.symbolToken;
                        if (index++ > 0)
                        {
                            writer.WriteLine ($"\t\t\t\t\ts += \" \";");
                        }
                        if (p_SymbolToken.declarator != (uint) AnglrClassificationType.TerminalName)
                        {
                            writer.Write ($"\t\t\t\t\ts += m_{p_SymbolToken.correctName}");
                            if (p_RhsNode.index > 0)
                                writer.Write ($"_{p_RhsNode.index}");
                            writer.WriteLine ($".Emit (depth - 1);");
                        }
                        else
                        {
                            writer.Write ($"\t\t\t\t\ts += m_{p_SymbolToken.correctName}");
                            if (p_RhsNode.index > 0)
                                writer.Write ($"_{p_RhsNode.index}");
                            writer.WriteLine ($".Emit (depth - 1);");
                        }
                    }
                    writer.WriteLine ($"\t\t\t\tbreak;");
                }
                writer.WriteLine ($"\t\t\t}}");
                writer.WriteLine ($"\t\t\treturn s.Trim ();");
                writer.WriteLine ($"\t\t}}");
            }
        }

        private void GenerateCSEmitProductionMethod (TextWriter writer, string correctName)
        {
            if (ProductionName.IteratorFlag)
            {
                writer.WriteLine ();
                writer.WriteLine ($"\t\t//");
                writer.WriteLine ($"\t\t// emit production tree node associated with any production of syntax rule {ProductionName.name}");
                writer.WriteLine ($"\t\t//");
                writer.WriteLine ();
                writer.WriteLine ($"\t\tpublic override string EmitProductionTree (int depth)");
                writer.WriteLine ($"\t\t{{");
                writer.WriteLine ($"\t\t\treturn (depth != 0) ?");
                writer.WriteLine ($"\t\t\t\t(string) Iterate (\"\", (node, appData) =>");
                writer.WriteLine ($"\t\t\t\t{{");
                writer.WriteLine ($"\t\t\t\t\tstring str = \"{correctName} (\" + (string) appData;");
                writer.WriteLine ($"\t\t\t\t\tswitch ((production_kind) node.kind)");
                writer.WriteLine ($"\t\t\t\t\t{{");

                foreach (RhsProduction p_RhsProduction in Productions)
                {
                    writer.WriteLine ($"\t\t\t\t\tcase production_kind.g_{ProductionName.correctName}_{p_RhsProduction.index}:");
                    writer.WriteLine ($"\t\t\t\t\t\t// emit syntax tree node associated with production");
                    writer.Write ($"\t\t\t\t\t\t// {ProductionName.name}:");
                    p_RhsProduction.display (writer);
                    writer.WriteLine ();
                    int turn = -1;
                    foreach (RhsNode p_RhsNode in p_RhsProduction.rhsNodes)
                    {
                        ++turn;
                        SymbolToken p_SymbolToken = p_RhsNode.symbolToken;
                        if (turn > 0)
                            writer.WriteLine ($"\t\t\t\t\t\tstr += ' ';");
                        if ((p_SymbolToken.IteratorFlag) && (p_SymbolToken.name == ProductionName.name))
                            continue;
                        writer.Write ($"\t\t\t\t\t\tstr += ");
                        if (p_SymbolToken.declarator == (uint) AnglrClassificationType.TerminalName)
                        {
                            writer.Write ($"\"{p_SymbolToken.correctName}\";");
                        }
                        else
                        {
                            writer.Write ($"node.m_{p_SymbolToken.correctName}");
                            if (p_RhsNode.index > 0)
                                writer.Write ($"_{p_RhsNode.index}");
                            writer.Write ($".EmitProductionTree (depth - 1);");
                        }
                        writer.WriteLine ();
                    }
                    writer.WriteLine ($"\t\t\t\t\t\tbreak;");
                }
                writer.WriteLine ($"\t\t\t\t\t}}");
                writer.WriteLine ($"\t\t\t\t\treturn str + \")\";");
                writer.WriteLine ($"\t\t\t\t}}) : \"\";");
                writer.WriteLine ($"\t\t}}");
            }
            else
            {
                writer.WriteLine ();
                writer.WriteLine ($"\t\t//");
                writer.WriteLine ($"\t\t// emit production tree node associated with any production of syntax rule {ProductionName.name}");
                writer.WriteLine ($"\t\t//");
                writer.WriteLine ();
                writer.WriteLine ($"\t\tpublic override string EmitProductionTree (int depth)");
                writer.WriteLine ($"\t\t{{");
                writer.WriteLine ($"\t\t\tstring s = \"{correctName} (\";");
                writer.WriteLine ($"\t\t\tif (depth != 0)");
                writer.WriteLine ($"\t\t\t\tswitch ((production_kind) kind)");
                writer.WriteLine ($"\t\t\t\t{{");

                foreach (RhsProduction p_RhsProduction in Productions)
                {
                    writer.WriteLine ($"\t\t\t\tcase production_kind.g_{ProductionName.correctName}_{p_RhsProduction.index}:");
                    writer.WriteLine ($"\t\t\t\t\t// emit syntax tree node associated with production");
                    writer.Write ($"\t\t\t\t\t// {ProductionName.name}:");
                    p_RhsProduction.display (writer);
                    writer.WriteLine ();
                    int turn = -1;
                    foreach (RhsNode p_RhsNode in p_RhsProduction.rhsNodes)
                    {
                        ++turn;
                        SymbolToken p_SymbolToken = p_RhsNode.symbolToken;
                        if (turn > 0)
                            writer.WriteLine ($"\t\t\t\t\ts += ' ';");
                        writer.Write ($"\t\t\t\t\ts += ");
                        if (p_SymbolToken.declarator == (uint) AnglrClassificationType.TerminalName)
                        {
                            writer.Write ($"\"{p_SymbolToken.correctName}\";");
                        }
                        else
                        {
                            writer.Write ($"m_{p_SymbolToken.correctName}");
                            if (p_RhsNode.index > 0)
                                writer.Write ($"_{p_RhsNode.index}");
                            writer.Write ($".EmitProductionTree (depth - 1);");
                        }
                        writer.WriteLine ();
                    }
                    writer.WriteLine ($"\t\t\t\tbreak;");
                }
                writer.WriteLine ($"\t\t\t}}");
                writer.WriteLine ($"\t\t\ts += \")\";");
                writer.WriteLine ($"\t\t\treturn s;");
                writer.WriteLine ($"\t\t}}");
            }
        }

        private static void GenerateCSInvokeTraverseMethod (TextWriter writer)
        {
            writer.WriteLine ();
            writer.WriteLine ("\t\t//");
            writer.WriteLine ("\t\t// traverse sub-tree rooted in this node using selected syntax tree walker");
            writer.WriteLine ("\t\t//");
            writer.WriteLine ();
            writer.WriteLine ("\t\tpublic override void InvokeTraverse (SyntaxTreeWalkerCore syntaxTreeWalker) => ((SyntaxTreeWalker) syntaxTreeWalker).Traverse (this);");
            writer.WriteLine ("\t\tpublic override void InvokeTraverseCommon (SyntaxTreeWalkerCore syntaxTreeWalker) => ((SyntaxTreeWalker) syntaxTreeWalker).TraverseCommon (this);");
        }

        private void GenerateCSReparentMethod (TextWriter writer)
        {
            if (ProductionName.IteratorFlag)
            {
                string correctName = ProductionName.correctName;
                writer.WriteLine ();
                writer.WriteLine ($"\t\t// reparent sub-tree");
                writer.WriteLine ();
                writer.WriteLine ($"\t\tpublic override void reparent (SyntaxTreeBase parent)");
                writer.WriteLine ($"\t\t{{");
                writer.WriteLine ($"\t\t\tfor ({correctName} node = this; node != null; node = node.m_{correctName})");
                writer.WriteLine ($"\t\t\t{{");
                writer.WriteLine ($"\t\t\t\tnode.parent = parent;");
                foreach (KeyValuePair<SymbolToken, symindex> keyval in m_symset)
                {
                    SymbolToken p_SymbolToken = keyval.Key;
                    if ((p_SymbolToken.IteratorFlag) && (p_SymbolToken.name == ProductionName.name))
                        continue;
                    string name = p_SymbolToken.correctName;
                    int index = keyval.Value.second;
                    writer.WriteLine ($"\t\t\t\tnode.m_{name}?.reparent (parent);");
                    for (int i = 0; i < index; ++i)
                    {
                        writer.WriteLine ($"\t\t\t\tnode.m_{name}_{(i + 1)}?.reparent (parent);");
                    }
                }
                writer.WriteLine ($"\t\t\t\tparent = node;");
                writer.WriteLine ($"\t\t\t}}");
                writer.WriteLine ($"\t\t}}");
            }
            else
            {
                writer.WriteLine ();
                writer.WriteLine ($"\t\t// reparent sub-tree");
                writer.WriteLine ();
                writer.WriteLine ($"\t\tpublic override void reparent (SyntaxTreeBase parent)");
                writer.WriteLine ($"\t\t{{");
                writer.WriteLine ($"\t\t\tthis.parent = parent;");
                writer.WriteLine ();
                foreach (KeyValuePair<SymbolToken, symindex> keyval in m_symset)
                {
                    SymbolToken p_SymbolToken = keyval.Key;
                    string name = p_SymbolToken.correctName;
                    int index = keyval.Value.second;
                    writer.WriteLine ($"\t\t\tm_{name}?.reparent (this);");
                    for (int i = 0; i < index; ++i)
                    {
                        writer.WriteLine ($"\t\t\tm_{name}_{(i + 1)}?.reparent (this);");
                        writer.WriteLine ();
                    }
                }
                writer.WriteLine ($"\t\t}}");
            }
        }

        private void GenerateCSInitMethod (TextWriter writer, string correctName)
        {
            writer.WriteLine ();
            writer.WriteLine ("\t\t//");
            writer.WriteLine ("\t\t// initialize object associated with syntax rule " + ProductionName.name);
            writer.WriteLine ("\t\t//");
            writer.WriteLine ();
            writer.WriteLine ("\t\tpublic void _init ()");
            writer.WriteLine ("\t\t{");
            foreach (KeyValuePair<SymbolToken, symindex> keyval in m_symset)
            {
                SymbolToken p_SymbolToken = keyval.Key;
                string name = p_SymbolToken.correctName;
                int index = keyval.Value.second;
                writer.WriteLine ("\t\t\tm_" + name + " = null;");
                for (int i = 0; i < index; ++i)
                    writer.WriteLine ("\t\t\tm_" + name + "_" + (i + 1) + " = null;");
                ;
            }
            writer.WriteLine ("\t\t}");
        }

        private void GenerateCSIteratorMethods (TextWriter writer, string correctName)
        {
            if (!ProductionName.IteratorFlag)
                return;
            writer.WriteLine ();
            writer.WriteLine ($"\t\tpublic delegate object IteratorDelegate ({correctName} p_{correctName}, object appData);");
            writer.WriteLine ();
            writer.WriteLine ($"\t\tpublic object Iterate (object appData, IteratorDelegate iteratorDelegate)");
            writer.WriteLine ($"\t\t{{");
            writer.WriteLine ($"\t\t\t{correctName} p_{correctName};");
            writer.WriteLine ($"\t\t\tfor (p_{correctName} = this; p_{correctName}.m_{correctName} != null; p_{correctName} = p_{correctName}.m_{correctName});");
            writer.WriteLine ($"\t\t\tfor (SyntaxTreeBase parent = p_{correctName}; (parent != null) && (parent is {correctName}); parent = parent.parent)");
            writer.WriteLine ($"\t\t\t\tappData = iteratorDelegate (({correctName}) parent, appData);");
            writer.WriteLine ($"\t\t\treturn appData;");
            writer.WriteLine ($"\t\t}}");
            if (false)
            {
                writer.WriteLine ();
                writer.WriteLine ($"\t\tpublic object IterateLeft (object appData, IteratorDelegate iteratorDelegate)");
                writer.WriteLine ($"\t\t{{");
                writer.WriteLine ($"\t\t\tfor ({correctName} p_{correctName} = m_{correctName}; p_{correctName} != null; p_{correctName} = p_{correctName}.m_{correctName})");
                writer.WriteLine ($"\t\t\t\tappData = iteratorDelegate (p_{correctName}, appData);");
                writer.WriteLine ($"\t\t\treturn appData;");
                writer.WriteLine ($"\t\t}}");
            }
        }

        private void GenerateCSClassMembers (TextWriter writer, string correctName)
        {
            int cnt;
            writer.WriteLine ();
            writer.WriteLine ("\t\t// counter of all nodes associated with syntax rule " + ProductionName.name);
            writer.WriteLine ("\t\tpublic static int g_counter;");
            writer.WriteLine ();
            writer.WriteLine ("\t\t// objects associated with terminal and non-terminal symbols within production(s) of syntax rule " + ProductionName.name);
            foreach (KeyValuePair<SymbolToken, symindex> keyval in m_symset)
            {
                SymbolToken p_SymbolToken = keyval.Key;
                string name = p_SymbolToken.correctName;
                int index = keyval.Value.second;
                if (p_SymbolToken.declarator == (uint) AnglrClassificationType.TerminalName)
                {
                    writer.WriteLine ("\t\tpublic SyntaxTreeToken m_" + name + " { get; private set; }");
                    for (int i = 0; i < index; ++i)
                        writer.WriteLine ("\t\tpublic SyntaxTreeToken m_" + name + "_" + (i + 1) + " { get; private set; }");
                }
                else
                {
                    writer.WriteLine ("\t\tpublic " + name + " m_" + name + " { get; private set; }");
                    for (int i = 0; i < index; ++i)
                        writer.WriteLine ("\t\tpublic " + name + " m_" + name + "_" + (i + 1) + " { get; private set; }");
                }
            }
            cnt = 0;
            foreach (RhsProduction p_RhsProduction in Productions)
            {
                ++cnt;
                MarkerByPosition markers = p_RhsProduction.markersByPosition;
                if (markers.Count == 0)
                    continue;
                string enumName;
                SymbolToken productionName = p_RhsProduction.productionName;
                if (productionName.declarator != (uint) AnglrClassificationType.Literal)
                    enumName = $"{productionName.correctName}_markers";
                else
                    enumName = $"{correctName}_markers_{cnt}";
                writer.WriteLine ();
                writer.WriteLine ($"\t\tpublic enum {enumName}");
                writer.WriteLine ($"\t\t{{");
                foreach ((int markerNumber, string markerName) in markers.Keys)
                {
                    SymbolToken markerSymbol = markers [(markerNumber, markerName)];
                    writer.WriteLine ($"\t\t\t{markerSymbol.correctName} = {markerNumber},");
                }
                writer.WriteLine ($"\t\t}};");
            }
        }

        private static void GenerateCSClassFooter (TextWriter writer)
        {
            writer.WriteLine ();
            writer.WriteLine ("\t};");
        }

        private void GenerateCSSyntaxTreeWalkerClass (TextWriter writer, string correctName)
        {
            GenerateCSSyntaxTreeWalkerHeader (writer);
            writer.WriteLine ($"\t\t#region delegates and events associated with the syntax rule {ProductionName.name}");
            GenerateCSSyntaxTreeWalkerEvents (writer, correctName);
            writer.WriteLine ($"\t\t#endregion");
            writer.WriteLine ($"\t\t#region syntax tree traversal methods associated with the syntax rule {ProductionName.name}");
            GenerateCSSyntaxTreeWalkerTraverseMethod (writer, correctName);
            //GenerateCSSyntaxTreeWalkerTraverseIteratorMethod (writer, correctName);
            GenerateCSSyntaxTreeWalkerTraverseCommonMethod (writer, correctName);
            //GenerateCSSyntaxTreeWalkersIterators (writer, correctName);
            GenerateCSSyntaxTreeWalkerFooter (writer);
            writer.WriteLine ($"\t\t#endregion");
        }

        private static void GenerateCSSyntaxTreeWalkerHeader (TextWriter writer)
        {
            writer.WriteLine ();
            writer.WriteLine ("\tpublic partial class SyntaxTreeWalker");
            writer.WriteLine ("\t{");
        }

        private void GenerateCSSyntaxTreeWalkerEvents (TextWriter writer, string correctName)
        {
            writer.WriteLine ();
            writer.WriteLine ($"\t\t// delegate function (callback) prototype associated with syntax rule {ProductionName.name}");
            writer.WriteLine ($"\t\tpublic delegate bool {correctName}_Callback (SyntaxTreeCallbackReason reason, {correctName}.production_kind kind, {correctName} p_{correctName});");
            writer.WriteLine ();
            writer.WriteLine ($"\t\t// event associated with syntax rule {ProductionName.name}");
            writer.WriteLine ($"\t\tpublic event {correctName}_Callback {correctName}_Event;");
            writer.WriteLine ();
            writer.WriteLine ($"\t\t// event trigger associated with syntax rule {ProductionName.name}");
            writer.WriteLine ($"\t\tpublic bool Raise_{correctName}_Event (SyntaxTreeCallbackReason reason, {correctName}.production_kind kind, {correctName} p_{correctName})");
            writer.WriteLine ($"\t\t{{");
            writer.WriteLine ($"\t\t\tbool? status = {correctName}_Event?.Invoke (reason, kind, p_{correctName});");
            writer.WriteLine ($"\t\t\treturn (status == null) || status.Value;");
            writer.WriteLine ($"\t\t}}");
        }

        private void GenerateCSSyntaxTreeWalkerTraverseMethod (TextWriter writer, string correctName)
        {
            writer.WriteLine ($"\t\t//");
            writer.WriteLine ($"\t\t// traverse syntax tree node associated with any production of syntax rule {ProductionName.name}");
            writer.WriteLine ($"\t\t//");
            writer.WriteLine ();
            writer.WriteLine ($"\t\tpublic void Traverse ({correctName} p_{correctName})");
            writer.WriteLine ($"\t\t{{");
            if (ProductionName.IteratorFlag)
            {
                writer.WriteLine ($"\t\t\tif (p_{correctName}.isLocked())");
                writer.WriteLine ($"\t\t\t\treturn;");
                writer.WriteLine ($"\t\t\tif (Raise_{correctName}_Event (SyntaxTreeCallbackReason.IterationPrologueCallbackReason, ({correctName}.production_kind) p_{correctName}.kind, p_{correctName}))");
                writer.WriteLine ($"\t\t\t\tp_{correctName}.Iterate (null, (node, appData) =>");
                writer.WriteLine ($"\t\t\t\t{{");
                writer.WriteLine ($"\t\t\t\t\tif (node.isLocked())");
                writer.WriteLine ($"\t\t\t\t\t\treturn null;");
                writer.WriteLine ($"\t\t\t\t\tnode.dolock();");
                writer.WriteLine ($"\t\t\t\t\t{correctName}.production_kind kind = ({correctName}.production_kind) node.kind;");
                writer.WriteLine ($"\t\t\t\t\tnode.turn_reset ();");
                writer.WriteLine ($"\t\t\t\t\tif (Raise_{correctName}_Event (SyntaxTreeCallbackReason.TraversalPrologueCallbackReason, kind, node))");
                writer.WriteLine ($"\t\t\t\t\t\tswitch (kind)");
                writer.WriteLine ($"\t\t\t\t\t\t{{");

                foreach (RhsProduction p_RhsProduction in Productions)
                {
                    writer.WriteLine ($"\t\t\t\t\t\t\tcase {correctName}.production_kind.g_{ProductionName.correctName}_{p_RhsProduction.index}:");
                    writer.WriteLine ($"\t\t\t\t\t\t\t\t// traverse syntax tree node associated with production");
                    writer.Write ($"\t\t\t\t\t\t\t\t// {ProductionName.name}:");
                    p_RhsProduction.display (writer);
                    writer.WriteLine ();
                    int turn = 0;
                    foreach (RhsNode p_RhsNode in p_RhsProduction.rhsNodes)
                    {
                        SymbolToken p_SymbolToken = p_RhsNode.symbolToken;
                        if (p_SymbolToken.declarator != (uint) AnglrClassificationType.NonTerminalName)
                            continue;
                        if (turn > 0)
                            writer.WriteLine ($"\t\t\t\t\t\t\t\tnode.turn_inc ();");
                        writer.WriteLine ($"\t\t\t\t\t\t\t\tif (Raise_{correctName}_Event (SyntaxTreeCallbackReason.TraversalMidTermCallbackReason, kind, node))");
                        if ((p_SymbolToken.IteratorFlag) && (p_SymbolToken.name == ProductionName.name) && (p_RhsNode.index == 0))
                            writer.WriteLine ($"\t\t\t\t\t\t\t\t\t;");
                        else
                        {
                            writer.Write ($"\t\t\t\t\t\t\t\t\tTraverse (node.m_{p_SymbolToken.correctName}");
                            if (p_RhsNode.index > 0)
                                writer.Write ($"_{p_RhsNode.index}");
                            writer.WriteLine ($");");
                        }
                        ++turn;
                    }
                    if (turn > 0)
                    {
                        writer.WriteLine ($"\t\t\t\t\t\t\t\tnode.turn_inc ();");
                        writer.WriteLine ($"\t\t\t\t\t\t\t\tRaise_{correctName}_Event (SyntaxTreeCallbackReason.TraversalMidTermCallbackReason, kind, node);");
                    }
                    writer.WriteLine ($"\t\t\t\t\t\t\tbreak;");
                }
                writer.WriteLine ($"\t\t\t\t\t\t}}");
                writer.WriteLine ($"\t\t\t\t\tRaise_{correctName}_Event (SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason, kind, node);");
                writer.WriteLine ($"\t\t\t\t\tnode.unlock();");
                writer.WriteLine ($"\t\t\t\t\treturn null;");
                writer.WriteLine ($"\t\t\t\t}});");
                writer.WriteLine ($"\t\t\tRaise_{correctName}_Event (SyntaxTreeCallbackReason.IterationEpilogueCallbackReason, ({correctName}.production_kind) p_{correctName}.kind, p_{correctName});");
            }
            else
            {
                writer.WriteLine ("\t\t\tif (p_" + correctName + ".isLocked())");
                writer.WriteLine ("\t\t\t\treturn;");
                writer.WriteLine ("\t\t\tp_" + correctName + ".dolock();");
                writer.WriteLine ("\t\t\t" + correctName + ".production_kind kind = (" + correctName + ".production_kind) p_" + correctName + ".kind;");
                writer.WriteLine ("\t\t\tp_" + correctName + ".turn_reset ();");
                writer.WriteLine ("\t\t\tif (Raise_" + correctName + "_Event (SyntaxTreeCallbackReason.TraversalPrologueCallbackReason, kind, p_" + correctName + "))");
                writer.WriteLine ("\t\t\tswitch (kind)");
                writer.WriteLine ("\t\t\t{");

                foreach (RhsProduction p_RhsProduction in Productions)
                {
                    writer.WriteLine ("\t\t\t\tcase " + correctName + ".production_kind.g_" + ProductionName.correctName + "_" + p_RhsProduction.index + ":");
                    writer.WriteLine ("\t\t\t\t\t// traverse syntax tree node associated with production");
                    writer.Write ("\t\t\t\t\t// " + ProductionName.name + ":");
                    p_RhsProduction.display (writer);
                    writer.WriteLine ();
                    int turn = 0;
                    foreach (RhsNode p_RhsNode in p_RhsProduction.rhsNodes)
                    {
                        SymbolToken p_SymbolToken = p_RhsNode.symbolToken;
                        if (p_SymbolToken.declarator != (uint) AnglrClassificationType.NonTerminalName)
                            continue;
                        if (turn > 0)
                            writer.WriteLine ("\t\t\t\t\tp_" + correctName + ".turn_inc ();");
                        writer.WriteLine ("\t\t\t\t\tif (Raise_" + correctName + "_Event (SyntaxTreeCallbackReason.TraversalMidTermCallbackReason, kind, p_" + correctName + "))");
                        writer.Write ("\t\t\t\t\t\tTraverse (p_" + correctName + ".m_" + p_SymbolToken.correctName);
                        if (p_RhsNode.index > 0)
                            writer.Write ("_" + p_RhsNode.index);
                        writer.WriteLine (");");
                        ++turn;
                    }
                    if (turn > 0)
                    {
                        writer.WriteLine ("\t\t\t\t\tp_" + correctName + ".turn_inc ();");
                        writer.WriteLine ("\t\t\t\t\tRaise_" + correctName + "_Event (SyntaxTreeCallbackReason.TraversalMidTermCallbackReason, kind, p_" + correctName + ");");
                    }
                    writer.WriteLine ("\t\t\t\tbreak;");
                }
                writer.WriteLine ("\t\t\t}");
                writer.WriteLine ("\t\t\tRaise_" + correctName + "_Event (SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason, kind, p_" + correctName + ");");
                writer.WriteLine ("\t\t\tp_" + correctName + ".unlock();");
            }
            writer.WriteLine ("\t\t}");
        }

        private void GenerateCSSyntaxTreeWalkerTraverseIteratorMethod (TextWriter writer, string correctName)
        {
            if (!ProductionName.IteratorFlag)
                return;
            writer.WriteLine ();
            writer.WriteLine ("\t\t//");
            writer.WriteLine ("\t\t// traverse syntax tree node associated with any production of syntax rule " + ProductionName.name);
            writer.WriteLine ("\t\t//");
            writer.WriteLine ();
            writer.WriteLine ("\t\tpublic void TraverseIterator (" + correctName + " p_" + correctName + ")");
            writer.WriteLine ("\t\t{");
            writer.WriteLine ("\t\t\tif (p_" + correctName + ".isLocked())");
            writer.WriteLine ("\t\t\t\treturn;");
            writer.WriteLine ("\t\t\tp_" + correctName + ".dolock();");
            writer.WriteLine ("\t\t\t" + correctName + ".production_kind kind = (" + correctName + ".production_kind) p_" + correctName + ".kind;");
            writer.WriteLine ("\t\t\tp_" + correctName + ".turn_reset ();");
            writer.WriteLine ("\t\t\tif (Raise_" + correctName + "_Event (SyntaxTreeCallbackReason.TraversalPrologueCallbackReason, kind, p_" + correctName + "))");
            writer.WriteLine ("\t\t\tswitch (kind)");
            writer.WriteLine ("\t\t\t{");

            foreach (RhsProduction p_RhsProduction in Productions)
            {
                writer.WriteLine ("\t\t\t\tcase " + correctName + ".production_kind.g_" + ProductionName.correctName + "_" + p_RhsProduction.index + ":");
                writer.WriteLine ("\t\t\t\t\t// traverse syntax tree node associated with production");
                writer.Write ("\t\t\t\t\t// " + ProductionName.name + ":");
                p_RhsProduction.display (writer);
                writer.WriteLine ();
                int turn = 0;
                foreach (RhsNode p_RhsNode in p_RhsProduction.rhsNodes)
                {
                    SymbolToken p_SymbolToken = p_RhsNode.symbolToken;
                    if (p_SymbolToken.declarator != (uint) AnglrClassificationType.NonTerminalName)
                        continue;
                    if (turn > 0)
                        writer.WriteLine ("\t\t\t\t\tp_" + correctName + ".turn_inc ();");
                    writer.WriteLine ("\t\t\t\t\tif (Raise_" + correctName + "_Event (SyntaxTreeCallbackReason.TraversalMidTermCallbackReason, kind, p_" + correctName + "))");
                    if (p_RhsNode.symbolToken.IteratorFlag)
                        writer.WriteLine ("\t\t\t\t\t\t;");
                    else
                    {
                        writer.Write ("\t\t\t\t\t\tTraverse (p_" + correctName + ".m_" + p_SymbolToken.correctName);
                        if (p_RhsNode.index > 0)
                            writer.Write ("_" + p_RhsNode.index);
                        writer.WriteLine (");");
                    }
                    ++turn;
                }
                if (turn > 0)
                {
                    writer.WriteLine ("\t\t\t\t\tp_" + correctName + ".turn_inc ();");
                    writer.WriteLine ("\t\t\t\t\tRaise_" + correctName + "_Event (SyntaxTreeCallbackReason.TraversalMidTermCallbackReason, kind, p_" + correctName + ");");
                }
                writer.WriteLine ("\t\t\t\tbreak;");
            }
            writer.WriteLine ("\t\t\t}");
            writer.WriteLine ("\t\t\tRaise_" + correctName + "_Event (SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason, kind, p_" + correctName + ");");
            writer.WriteLine ("\t\t\tp_" + correctName + ".unlock();");
            writer.WriteLine ("\t\t}");
        }

        private void GenerateCSSyntaxTreeWalkerTraverseCommonMethod (TextWriter writer, string correctName)
        {
            writer.WriteLine ();
            writer.WriteLine ($"\t\t//");
            writer.WriteLine ($"\t\t// traverse syntax tree node associated with any production of syntax rule {ProductionName.name}");
            writer.WriteLine ($"\t\t//");
            writer.WriteLine ();
            writer.WriteLine ($"\t\tpublic void TraverseCommon ({correctName} p_{correctName})");
            writer.WriteLine ($"\t\t{{");
            if (ProductionName.IteratorFlag)
            {
                writer.WriteLine ($"\t\t\tif (Raise_Common_Event (SyntaxTreeCallbackReason.IterationPrologueCallbackReason, (int) p_{correctName}.kind, p_{correctName}))");
                writer.WriteLine ($"\t\t\t\tp_{correctName}.Iterate (null, (node, appData) =>");
                writer.WriteLine ($"\t\t\t\t{{");
                writer.WriteLine ($"\t\t\t\t\t{correctName}.production_kind kind = ({correctName}.production_kind) node.kind;");
                writer.WriteLine ($"\t\t\t\t\tif (Raise_Common_Event (SyntaxTreeCallbackReason.TraversalPrologueCallbackReason, (int) kind, node))");
                writer.WriteLine ($"\t\t\t\t\t\tswitch (kind)");
                writer.WriteLine ($"\t\t\t\t\t\t{{");

                foreach (RhsProduction p_RhsProduction in Productions)
                {
                    writer.WriteLine ($"\t\t\t\t\t\tcase {correctName}.production_kind.g_{ProductionName.correctName}_{p_RhsProduction.index}:");
                    writer.WriteLine ($"\t\t\t\t\t\t\t// traverse syntax tree node associated with production");
                    writer.Write ($"\t\t\t\t\t\t\t// {ProductionName.name}:");
                    p_RhsProduction.display (writer);
                    writer.WriteLine ();
                    int turn = -1;
                    foreach (RhsNode p_RhsNode in p_RhsProduction.rhsNodes)
                    {
                        ++turn;
                        SymbolToken p_SymbolToken = p_RhsNode.symbolToken;
                        if (!(p_SymbolToken.name == ProductionName.name))
                        {
                            writer.Write ($"\t\t\t\t\t\t\tTraverseCommon (node.m_{p_SymbolToken.correctName}");
                            if (p_RhsNode.index > 0)
                                writer.Write ($"_{p_RhsNode.index}");
                            writer.WriteLine (");");
                        }
                    }
                    writer.WriteLine ($"\t\t\t\t\t\t\tbreak;");
                }
                writer.WriteLine ($"\t\t\t\t\t\t}}");
                writer.WriteLine ($"\t\t\t\t\tRaise_Common_Event (SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason, (int) kind, node);");
                writer.WriteLine ($"\t\t\t\t\treturn null;");
                writer.WriteLine ($"\t\t\t\t}});");
                writer.WriteLine ($"\t\t\tRaise_Common_Event (SyntaxTreeCallbackReason.IterationEpilogueCallbackReason, (int) p_{correctName}.kind, p_{correctName});");
            }
            else
            {
                writer.WriteLine ($"\t\t\t{correctName}.production_kind kind = ({correctName}.production_kind) p_{correctName}.kind;");
                writer.WriteLine ($"\t\t\tif (Raise_Common_Event (SyntaxTreeCallbackReason.TraversalPrologueCallbackReason, (int) kind, p_{correctName}))");
                writer.WriteLine ($"\t\t\tswitch (kind)");
                writer.WriteLine ($"\t\t\t{{");

                foreach (RhsProduction p_RhsProduction in Productions)
                {
                    writer.WriteLine ($"\t\t\t\tcase {correctName}.production_kind.g_{ProductionName.correctName}_{p_RhsProduction.index}:");
                    writer.WriteLine ($"\t\t\t\t\t// traverse syntax tree node associated with production");
                    writer.Write ($"\t\t\t\t\t// {ProductionName.name}:");
                    p_RhsProduction.display (writer);
                    writer.WriteLine ();
                    int turn = -1;
                    foreach (RhsNode p_RhsNode in p_RhsProduction.rhsNodes)
                    {
                        ++turn;
                        SymbolToken p_SymbolToken = p_RhsNode.symbolToken;
                        writer.Write ($"\t\t\t\t\t\tTraverseCommon (p_{correctName}.m_{p_SymbolToken.correctName}");
                        if (p_RhsNode.index > 0)
                            writer.Write ($"_{p_RhsNode.index}");
                        writer.WriteLine ($");");
                    }
                    writer.WriteLine ($"\t\t\t\tbreak;");
                }
                writer.WriteLine ($"\t\t\t}}");
                writer.WriteLine ($"\t\t\tRaise_Common_Event (SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason, (int) kind, p_{correctName});");
            }
            writer.WriteLine ($"\t\t}}");
        }

        private void GenerateCSSyntaxTreeWalkerFooter (TextWriter writer)
        {
            writer.WriteLine ("\t};");
        }

        private void GenerateCSSyntaxTreeBuilderClass (TextWriter writer, string correctName)
        {
            GenerateCSSyntaxTreeBuilderHeader (writer);
            writer.WriteLine ($"\t\t#region syntax tree builder methods associated with the syntax rule {ProductionName.name}");
            GenerateCSSyntaxTreeBuilderBuilders (writer, correctName);
            writer.WriteLine ($"\t\t#endregion");
            GenerateCSSyntaxTreeBuilderFooter (writer);
        }

        private static void GenerateCSSyntaxTreeBuilderHeader (TextWriter writer)
        {
            writer.WriteLine ();
            writer.WriteLine ("\tpublic partial class SyntaxTreeBuilder : SyntaxTreeWalker");
            writer.WriteLine ("\t{");
        }

        private void GenerateCSSyntaxTreeBuilderBuilders (TextWriter writer, string correctName)
        {
            foreach (RhsProduction p_RhsProduction in Productions)
            {
                int kind = p_RhsProduction.index;
                writer.WriteLine ();
                writer.WriteLine ("\t\t//");
                writer.WriteLine ("\t\t// create syntax tree node associated with production");
                writer.Write ("\t\t// " + ProductionName.name + ":");
                p_RhsProduction.display (writer);
                writer.WriteLine ();
                writer.WriteLine ("\t\t//");
                writer.WriteLine ();
                writer.Write ("\t\tpublic " + correctName + " " + correctName + "_" + kind + " (");
                string sep = "";
                int tokenIndex = 0;
                foreach (RhsNode p_RhsNode in p_RhsProduction.rhsNodes)
                {
                    SymbolToken p_SymbolToken = p_RhsNode.symbolToken;
                    string name = p_SymbolToken.correctName;
                    int index = p_RhsNode.index;
                    if (p_SymbolToken.declarator == (uint) AnglrClassificationType.TerminalName)
                    {
                        writer.Write (sep + "SyntaxTreeToken p_token");
                        if (tokenIndex > 0)
                            writer.Write ("_" + tokenIndex);
                        ++tokenIndex;
                    }
                    else
                    {
                        writer.Write (sep + name + " p_" + name);
                        if (index > 0)
                            writer.Write ("_" + index);
                    }
                    sep = ", ";
                }
                writer.WriteLine (")");
                writer.WriteLine ("\t\t{");
                writer.Write ("\t\t\t" + correctName + " p_" + correctName + "_ref = new " + correctName + "(");
                sep = "";
                tokenIndex = 0;
                foreach (RhsNode p_RhsNode in p_RhsProduction.rhsNodes)
                {
                    SymbolToken p_SymbolToken = p_RhsNode.symbolToken;
                    string name = p_SymbolToken.correctName;
                    int index = p_RhsNode.index;
                    if (p_SymbolToken.declarator == (uint) AnglrClassificationType.TerminalName)
                    {
                        writer.Write (sep + "p_token");
                        if (tokenIndex > 0)
                            writer.Write ("_" + tokenIndex);
                        ++tokenIndex;
                    }
                    else
                    {
                        writer.Write (sep + "p_" + name);
                        if (index > 0)
                            writer.Write ("_" + index);
                    }
                    sep = ", ";
                }
                if (m_protoset [p_RhsProduction] > 1)
                    writer.Write (sep + kind);
                writer.WriteLine (");");
                tokenIndex = 0;
                foreach (RhsNode p_RhsNode in p_RhsProduction.rhsNodes)
                {
                    SymbolToken p_SymbolToken = p_RhsNode.symbolToken;
                    string name = p_SymbolToken.correctName;
                    int index = p_RhsNode.index;
                    if (p_SymbolToken.declarator == (uint) AnglrClassificationType.TerminalName)
                    {
                        writer.Write ("\t\t\tp_token");
                        if (tokenIndex > 0)
                            writer.Write ("_" + tokenIndex);
                        ++tokenIndex;
                    }
                    else
                    {
                        writer.Write ("\t\t\tp_" + name);
                        if (index > 0)
                            writer.Write ("_" + index);
                    }
                    writer.WriteLine (".parent = p_" + correctName + "_ref;");
                }
                writer.WriteLine ("\t\t\tRaise_" + correctName + "_Event (SyntaxTreeCallbackReason.BuilderCallbackReason, " + correctName + ".production_kind.g_" + correctName + "_" + kind + ", p_" + correctName + "_ref);");
                writer.WriteLine ("\t\t\treturn p_" + correctName + "_ref;");
                writer.WriteLine ("\t\t}");
            }
        }

        private static void GenerateCSSyntaxTreeBuilderFooter (TextWriter writer)
        {
            writer.WriteLine ("\t};");
        }

        internal void GenerateCsSourceFile (TextWriter writer)
        {
        }

        internal void GenerateCsCallbackPrototypes (TextWriter writer)
        {
            int cnt = 0;
            foreach (RhsProduction p_RhsProduction in Productions)
            {
                ++cnt;
                string correctName = ProductionName.correctName;
                writer.Write ("\t" + correctName + "* " + correctName + "_" + cnt + " (");
                string sep = "";
                int tokenIndex = 0;
                foreach (RhsNode p_RhsNode in p_RhsProduction.rhsNodes)
                {
                    SymbolToken p_SymbolToken = p_RhsNode.symbolToken;
                    string name = p_SymbolToken.correctName;
                    int index = p_RhsNode.index;
                    if (p_SymbolToken.declarator == (uint) AnglrClassificationType.TerminalName)
                    {
                        writer.Write (sep + "SyntaxTreeToken* p_token");
                        if (tokenIndex > 0)
                            writer.Write ("_" + tokenIndex);
                        ++tokenIndex;
                    }
                    else
                    {
                        writer.Write (sep + name + "* p_" + name);
                        if (index > 0)
                            writer.Write ("_" + index);
                    }
                    sep = ", ";
                }
                writer.WriteLine (");");
            }
        }

        internal void EliminateTautologies ()
        {
            string name = this.ProductionName.name;
            productions tautologyList = new productions ();
            foreach (RhsProduction rhsProduction in Productions)
            {
                bool isTautology = false;
                bool brokenTest = false;
                foreach (RhsNode p_rhsNode in rhsProduction.rhsNodes)
                {
                    if (p_rhsNode.symbolToken.name != name)
                    {
                        brokenTest = true;
                        break;
                    }
                    isTautology = true;
                }
                if (brokenTest || !isTautology)
                    continue;
                tautologyList.Add (rhsProduction);
            }
            foreach (RhsProduction rhsProduction in tautologyList)
            {
                Productions.Remove (rhsProduction);
            }
        }

        internal void CheckIterator ()
        {
            bool iterator = false;
            foreach (RhsProduction rhsProduction in Productions)
            {
                int index = -1;
                int counter = 0;
                foreach (RhsNode p_rhsNode in rhsProduction.rhsNodes)
                {
                    if (ProductionName.name.Equals (p_rhsNode.symbolToken.name))
                    {
                        index = counter;
                    }
                    counter++;
                }
                if (index > 1)
                {
                    iterator = false;
                    break;
                }
                if (index < 0)
                    continue;
                iterator = true;
            }
            if (ProductionName.IteratorAttributeFlag)
                ;
            if (iterator && ProductionName.IteratorAttributeFlag)
                ProductionName.IteratorFlag = true;
        }

        internal SymbolToken CheckIdentity ()
        {
            if (Productions.Count > 1)
                return null;
            RhsProduction rhsProduction = Productions [0];
            if (rhsProduction.rhsNodes.Count != 1)
                return null;
            RhsNode rhsNode = rhsProduction.rhsNodes [0];
            return rhsNode.symbolToken;
        }

        internal (bool, SymbolToken) CheckCascade ()
        {
            if (Productions.Count < 2)
            {
                if (Productions.Count == 0)
                    return (false, null);
                RhsProduction rhsProduction = Productions [0];
                if (rhsProduction.rhsNodes.Count != 1)
                    return (false, null);
                RhsNode rhsNode = rhsProduction.rhsNodes [0];
                return (true, rhsNode.symbolToken);
            }

            RhsProduction cascadeBase = null;
            foreach (RhsProduction rhsProduction in Productions)
            {
                int rlen = 0;
                foreach (RhsNode p_rhsNode in rhsProduction.rhsNodes)
                {
                    ++rlen;
                }
                switch (rlen)
                {
                    case 1:
                        if (cascadeBase != null)
                            return (false, null);
                        cascadeBase = rhsProduction;
                        continue;
                    case 3:
                        continue;
                    default:
                        return (false, null);
                }
            }
            if (cascadeBase == null)
                return (false, null);
            string baseName = cascadeBase.rhsNodes [0].symbolToken.name;
            string cascName = ProductionName.name;
            foreach (RhsProduction rhsProduction in Productions)
            {
                if (rhsProduction == cascadeBase)
                    continue;
                int rlen = 0;
                RhsNode cascNode = null;
                RhsNode opNode = null;
                RhsNode baseNode = null;
                foreach (RhsNode p_rhsNode in rhsProduction.rhsNodes)
                {
                    switch (rlen++)
                    {
                        case 0:
                            cascNode = p_rhsNode;
                            break;
                        case 1:
                            opNode = p_rhsNode;
                            break;
                        case 2:
                            baseNode = p_rhsNode;
                            break;
                    }
                }
                if ((cascNode.symbolToken.name == cascName) && (baseNode.symbolToken.name == baseName))
                {
                    rhsProduction.associativity = ProductionAssociativity.Left;
                    continue;
                }
                if ((cascNode.symbolToken.name == baseName) && (baseNode.symbolToken.name == cascName))
                {
                    rhsProduction.associativity = ProductionAssociativity.Right;
                    continue;
                }
                return (false, null);
            }
            return (false, cascadeBase.rhsNodes [0].symbolToken);
        }

        internal productions GenerateCascade ()
        {
            if (CascadeResolved)
                return Productions;
            CascadeResolved = true;
            if (IdentityNode != null)
            {
                IdentityNode.GenerateCascade ();
                return Productions;
            }
            else if (CascadeNode != null)
            {
                if ((CascadeNode.IdentityNode != null) || (CascadeNode.CascadeNode != null))
                    Productions = GenerateCascadeProductions (CascadeNode);
                else
                {
                    foreach (RhsProduction rhsProduction in Productions)
                        rhsProduction.priority = 0;
                }
                return Productions;
            }
            else /*if (ProductionName.getCascade () != 0)*/
                return Productions;
        }

        internal productions GenerateCascadeProductions (RhsProductionNode cascadeNode)
        {
            SymbolToken cascadeName = cascadeNode.ProductionName;
            SymbolToken productionName = ProductionName;
            productions rhsProductions = cascadeNode.GenerateCascade ();
            productions productions = new productions ();

            int priority = -1;
            rhslist rhsNodes = new rhslist ();
            foreach (RhsProduction rhsProduction in rhsProductions)
            {
                RhsProduction production = new RhsProduction (productionName);
                if (rhsProduction.priority > priority)
                    priority = rhsProduction.priority;
                int rlen = 0;
                production.priority = rhsProduction.priority;
                production.associativity = rhsProduction.associativity;
                switch (rhsProduction.rhsNodes.Count)
                {
                    case 1:
                    {
                        production.add (new RhsNode (rhsProduction.rhsNodes [0].symbolToken));
                    }
                    break;
                    case 3:
                    {
                        foreach (RhsNode rhsNode in rhsProduction.rhsNodes)
                        {
                            switch (rlen++)
                            {
                                case 0:
                                    production.add (new RhsNode (productionName));
                                    break;
                                case 1:
                                    production.add (new RhsNode (rhsNode.symbolToken));
                                    break;
                                case 2:
                                    production.add (new RhsNode (productionName));
                                    break;
                            }
                        }
                    }
                    break;
                    default:
                        continue;
                }
                productions.Add (production);
            }

            ++priority;
            foreach (RhsProduction rhsProduction in Productions)
            {
                RhsProduction production = new RhsProduction (productionName);
                production.priority = priority;
                production.associativity = rhsProduction.associativity;
                int rlen = 0;
                switch (rhsProduction.rhsNodes.Count)
                {
                    case 3:
                    {
                        foreach (RhsNode rhsNode in rhsProduction.rhsNodes)
                        {
                            switch (rlen++)
                            {
                                case 0:
                                    production.add (new RhsNode (productionName));
                                    break;
                                case 1:
                                    production.add (new RhsNode (rhsNode.symbolToken));
                                    break;
                                case 2:
                                    production.add (new RhsNode (productionName));
                                    break;
                            }
                        }
                    }
                    break;
                    default:
                        continue;
                }
                productions.Add (production);
            }
            return productions;
        }

        internal bool insertStartElement (SymbolToken p_SymbolToken) { return StartSet.insert (p_SymbolToken); }
        internal bool checkStartElement (SymbolToken p_SymbolToken) { return StartSet.check (p_SymbolToken); }
        internal bool insertStartSet (TokenSet p_tokset) { return StartSet.makeUnion (p_tokset); }

        public  SymbolToken ProductionName { get; private set; }
        public  productions Productions { get; set; } = new productions ();
        internal RhsProductionNode IdentityNode { get; set; } = null;
        internal RhsProductionNode CascadeNode { get; set; } = null;
        internal TokenSet StartSet { get; } = new TokenSet ();
        internal int BestGoto { get; private set; } = -1;
        internal int round { get; set; } = 0;
        internal bool EpsilonCondition
        {
            get => (m_flags & EpsilonConditionFlag) != 0;
            set => _ = (value) ? m_flags |= EpsilonConditionFlag : m_flags &= ~EpsilonConditionFlag;
        }
        internal bool Used
        {
            get => (m_flags & UsedFlag) != 0;
            set => _ = (value) ? m_flags |= UsedFlag : m_flags &= ~UsedFlag;
        }
        internal bool CascadeResolved
        {
            get => (m_flags & CascadeResolvedFlag) != 0;
            set => _ = (value) ? m_flags |= CascadeResolvedFlag : m_flags &= ~CascadeResolvedFlag;
        }

        private const uint EpsilonConditionFlag = (1 << 0);
        private const uint UsedFlag = (1 << 1);
        private const uint CascadeResolvedFlag = (1 << 2);

        private uint m_flags = 0;
        private gotocnt m_gotocnt = new gotocnt ();
        private protoset m_protoset = new protoset ();
        private symset m_symset = new symset ();
    }
}
