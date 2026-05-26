using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Anglr.Parser;
using Anglr.Parser.Core;
using Anglr.Parser.SyntaxTree;
using Anglr.Compiler;
using AnglrParserLibrary;
using AnglrLogLibrary;
using System.ComponentModel;
using static Anglr.Declarations.AnglrDeclarations;
using AnglrLibrary;
using System.Xml.Linq;

namespace AnglrParserLibrary
{
    internal class AnglrReferenceNode
    {
        internal SyntaxTreeBase node { get; private set; }
        internal string name { get; private set; }
        internal string displayName { get; private set; }
        internal AnglrReferenceNode context { get; private set; }
        internal AnglrReferenceNode (SyntaxTreeBase node, string name, string displayName, AnglrReferenceNode context)
        {
            this.node = node;
            this.name = name;
            this.displayName = displayName;
            this.context = context;
        }
        internal string FullName => (context != null) ? $"{context.FullName}.{name}" : name;
    }
    internal class ReferenceComparer : IComparer<AnglrReferenceNode>
    {
        public int Compare (AnglrReferenceNode x, AnglrReferenceNode y)
        {
            for (; (x != null) && (y != null); x = x.context, y = y.context)
            {
                int diff = x.name.CompareTo (y.name);
                if (diff != 0)
                    return diff;
            }
            return (x != null) ? (y != null) ? x.name.CompareTo (y.name) : 1 : (y != null) ? -1 : 0;
        }
    }
    internal class AnglrReferenceDictionary : SortedDictionary<AnglrReferenceNode, AnglrReferenceNode>
    {
        internal AnglrReferenceDictionary () : base (new ReferenceComparer ()) { }
        internal void Display (IAnglrLogger logger)
        {
            foreach (AnglrReferenceNode node in Values)
            {
                logger?.DebugRawLine (node.FullName);
            }
        }
    }

    public class AnglrReferencesGenerator : SyntaxTreeWalker
    {
        private readonly AnglrReferenceDictionary _dictionary;
        private Stack<AnglrReferenceNode> _stack;
        private AnglrReferenceNode topReference;
        private AnglrReferenceNode filePartListHeader;
        private AnglrReferenceNode terminalListHeader;
        private AnglrReferenceNode regexListHeader;
        private AnglrReferenceNode nonTerminalListHeader;
        private anglrCompiler compiler;
        private int step;
        public AnglrReferencesGenerator (anglrCompiler compiler)
        {
            this.compiler = compiler;

            _dictionary = new AnglrReferenceDictionary ();
            _stack = new Stack<AnglrReferenceNode> ();

            topReference = new AnglrReferenceNode (null, "", "", null);
            filePartListHeader = new AnglrReferenceNode (null, "header 1", "Anglr File Parts", topReference);
            terminalListHeader = new AnglrReferenceNode (null, "header 2", "Terminal Symbols", topReference);
            regexListHeader = new AnglrReferenceNode (null, "header 3", "Regular Expressions", topReference);
            nonTerminalListHeader = new AnglrReferenceNode (null, "header 4", "NonTerminal Symbols", topReference);

            _dictionary [topReference] = topReference;
            _dictionary [filePartListHeader] = filePartListHeader;
            _dictionary [terminalListHeader] = terminalListHeader;
            _dictionary [regexListHeader] = regexListHeader;
            _dictionary [nonTerminalListHeader] = nonTerminalListHeader;
            _stack.Push (topReference);

            _attribute__Event += AnglrReferencesGenerator__attribute__Event;
            _name_value_pair__Event += AnglrReferencesGenerator__name_value_pair__Event;
            _general_part__Event += AnglrReferencesGenerator__general_part__Event;
            _declaration_part__Event += AnglrReferencesGenerator__declaration_part__Event;
            _terminal_definition__Event += AnglrReferencesGenerator__terminal_definition__Event;
            _regex_definition__Event += AnglrReferencesGenerator__regex_definition__Event;
            _scanner_part__Event += AnglrReferencesGenerator__scanner_part__Event;
            _terminal_action__Event += AnglrReferencesGenerator__terminal_action__Event;
            _event_action__Event += AnglrReferencesGenerator__event_action__Event;
            _push_action__Event += AnglrReferencesGenerator__push_action__Event;
            _lexer_part__Event += AnglrReferencesGenerator__lexer_part__Event;
            _parser_part__Event += AnglrReferencesGenerator__parser_part__Event;
            _anglr_syntax_rule__Event += AnglrReferencesGenerator__anglr_syntax_rule__Event;
            _anglr_syntax_production_list_name__Event += AnglrReferencesGenerator__anglr_syntax_production_list_name__Event;
            _production_name__Event += AnglrReferencesGenerator__production_name__Event;
            _associativity_specification__Event += AnglrReferencesGenerator__associativity_specification__Event;
            _marker__Event += AnglrReferencesGenerator__marker__Event;
            _name__Event += AnglrReferencesGenerator__name__Event;
        }

        private bool AnglrReferencesGenerator__attribute__Event (SyntaxTreeCallbackReason reason, _attribute_.production_kind kind, _attribute_ p__attribute_)
        {
            switch (reason)
            {
                case SyntaxTreeCallbackReason.TraversalPrologueCallbackReason:
                {
                    AnglrReferenceNode anglrReference = _stack.Peek ();
                    AnglrReferenceNode referenceNode = new AnglrReferenceNode (p__attribute_, p__attribute_.m__identifier_.text, null, _stack.Peek ());
                    if (step > 0)
                        referenceNode = _dictionary [referenceNode];
                    else
                        _dictionary [referenceNode] = referenceNode;
                    _stack.Push (referenceNode);
                }
                break;
                case SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason:
                    _stack.Pop ();
                    break;
            }
            return true;
        }

        private bool AnglrReferencesGenerator__name_value_pair__Event (SyntaxTreeCallbackReason reason, _name_value_pair_.production_kind kind, _name_value_pair_ p__name_value_pair_)
        {
            switch (reason)
            {
                case SyntaxTreeCallbackReason.TraversalPrologueCallbackReason:
                {
                    AnglrReferenceNode referenceNode = new AnglrReferenceNode (p__name_value_pair_, p__name_value_pair_.m__identifier_.text, null, _stack.Peek ());
                    if (step > 0)
                        referenceNode = _dictionary [referenceNode];
                    else
                        _dictionary [referenceNode] = referenceNode;
                    _stack.Push (referenceNode);
                }
                break;
                case SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason:
                    _stack.Pop ();
                    break;
            }
            return true;
        }

        private bool AnglrReferencesGenerator__general_part__Event (SyntaxTreeCallbackReason reason, _general_part_.production_kind kind, _general_part_ p__general_part_)
        {
            switch (reason)
            {
                case SyntaxTreeCallbackReason.TraversalPrologueCallbackReason:
                {
                    AnglrReferenceNode referenceNode = new AnglrReferenceNode (p__general_part_, p__general_part_.m__identifier_.text, null, filePartListHeader);
                    if (step > 0)
                        referenceNode = _dictionary [referenceNode];
                    else
                        _dictionary [referenceNode] = referenceNode;
                    _stack.Push (referenceNode);
                }
                break;
                case SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason:
                    _stack.Pop ();
                    break;
            }
            return true;
        }

        private bool AnglrReferencesGenerator__declaration_part__Event (SyntaxTreeCallbackReason reason, _declaration_part_.production_kind kind, _declaration_part_ p__declaration_part_)
        {
            switch (reason)
            {
                case SyntaxTreeCallbackReason.TraversalPrologueCallbackReason:
                {
                    AnglrReferenceNode referenceNode = new AnglrReferenceNode (p__declaration_part_, p__declaration_part_.m__identifier_.text, null, filePartListHeader);
                    if (step > 0)
                        referenceNode = _dictionary [referenceNode];
                    else
                        _dictionary [referenceNode] = referenceNode;
                    _stack.Push (referenceNode);
                }
                break;
                case SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason:
                    _stack.Pop ();
                    break;
            }
            return true;
        }

        private bool AnglrReferencesGenerator__terminal_definition__Event (SyntaxTreeCallbackReason reason, _terminal_definition_.production_kind kind, _terminal_definition_ p__terminal_definition_)
        {
            switch (reason)
            {
                case SyntaxTreeCallbackReason.TraversalPrologueCallbackReason:
                {
                    AnglrReferenceNode referenceNode = new AnglrReferenceNode (p__terminal_definition_, p__terminal_definition_.m__identifier_.text, null, terminalListHeader);
                    if (step > 0)
                        referenceNode = _dictionary [referenceNode];
                    else
                        _dictionary [referenceNode] = referenceNode;
                    _stack.Push (referenceNode);
                    SyntaxTreeToken token = p__terminal_definition_.m__cstring_optional_.m__cstring_;
                    if (token == null)
                        break;
                    referenceNode = new AnglrReferenceNode (token, token.text, null, terminalListHeader);
                    if (step <= 0)
                        _dictionary [referenceNode] = referenceNode;
                }
                break;
                case SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason:
                    _stack.Pop ();
                    break;
            }
            return true;
        }

        private bool AnglrReferencesGenerator__regex_definition__Event (SyntaxTreeCallbackReason reason, _regex_definition_.production_kind kind, _regex_definition_ p__regex_definition_)
        {
            switch (reason)
            {
                case SyntaxTreeCallbackReason.TraversalPrologueCallbackReason:
                {
                    AnglrReferenceNode referenceNode = new AnglrReferenceNode (p__regex_definition_, p__regex_definition_.m__identifier_.text, null, regexListHeader);
                    if (step > 0)
                        referenceNode = _dictionary [referenceNode];
                    else
                        _dictionary [referenceNode] = referenceNode;
                    _stack.Push (referenceNode);
                }
                break;
                case SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason:
                    _stack.Pop ();
                    break;
            }
            return true;
        }

        private bool AnglrReferencesGenerator__scanner_part__Event (SyntaxTreeCallbackReason reason, _scanner_part_.production_kind kind, _scanner_part_ p__scanner_part_)
        {
            switch (reason)
            {
                case SyntaxTreeCallbackReason.TraversalPrologueCallbackReason:
                {
                    AnglrReferenceNode referenceNode = new AnglrReferenceNode (p__scanner_part_, p__scanner_part_.m__identifier_.text, null, filePartListHeader);
                    if (step > 0)
                        referenceNode = _dictionary [referenceNode];
                    else
                        _dictionary [referenceNode] = referenceNode;
                    _stack.Push (referenceNode);
                }
                break;
                case SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason:
                    _stack.Pop ();
                    break;
            }
            return true;
        }

        private bool AnglrReferencesGenerator__terminal_action__Event (SyntaxTreeCallbackReason reason, _terminal_action_.production_kind kind, _terminal_action_ p__terminal_action_)
        {
            if (step <= 0)
                return true;
            switch (reason)
            {
                case SyntaxTreeCallbackReason.TraversalPrologueCallbackReason:
                {
                    AnglrReferenceNode referenceNode = new AnglrReferenceNode (p__terminal_action_, p__terminal_action_.m__identifier_.text, null, _stack.Peek ());
                    _dictionary [referenceNode] = referenceNode;
                }
                break;
                case SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason:
                    break;
            }
            return true;
        }

        private bool AnglrReferencesGenerator__event_action__Event (SyntaxTreeCallbackReason reason, _event_action_.production_kind kind, _event_action_ p__event_action_)
        {
            switch (reason)
            {
                case SyntaxTreeCallbackReason.TraversalPrologueCallbackReason:
                {
                    AnglrReferenceNode referenceNode = new AnglrReferenceNode (p__event_action_, p__event_action_.m__identifier_.text, null, _stack.Peek ());
                    if (step > 0)
                        referenceNode = _dictionary [referenceNode];
                    else
                        _dictionary [referenceNode] = referenceNode;
                    _stack.Push (referenceNode);
                }
                break;
                case SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason:
                    _stack.Pop ();
                    break;
            }
            return true;
        }

        private bool AnglrReferencesGenerator__push_action__Event (SyntaxTreeCallbackReason reason, _push_action_.production_kind kind, _push_action_ p__push_action_)
        {
            if (step <= 0)
                return true;
            switch (reason)
            {
                case SyntaxTreeCallbackReason.TraversalPrologueCallbackReason:
                {
                    AnglrReferenceNode referenceNode = new AnglrReferenceNode (p__push_action_, p__push_action_.m__identifier_.text, null, _stack.Peek ());
                    _dictionary [referenceNode] = referenceNode;
                }
                break;
                case SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason:
                    break;
            }
            return true;
        }

        private bool AnglrReferencesGenerator__lexer_part__Event (SyntaxTreeCallbackReason reason, _lexer_part_.production_kind kind, _lexer_part_ p__lexer_part_)
        {
            switch (reason)
            {
                case SyntaxTreeCallbackReason.TraversalPrologueCallbackReason:
                {
                    AnglrReferenceNode referenceNode = new AnglrReferenceNode (p__lexer_part_, p__lexer_part_.m__identifier_.text, null, filePartListHeader);
                    if (step > 0)
                        referenceNode = _dictionary [referenceNode];
                    else
                        _dictionary [referenceNode] = referenceNode;
                    _stack.Push (referenceNode);
                }
                break;
                case SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason:
                    _stack.Pop ();
                    break;
            }
            return true;
        }

        private bool AnglrReferencesGenerator__parser_part__Event (SyntaxTreeCallbackReason reason, _parser_part_.production_kind kind, _parser_part_ p__parser_part_)
        {
            switch (reason)
            {
                case SyntaxTreeCallbackReason.TraversalPrologueCallbackReason:
                {
                    AnglrReferenceNode referenceNode = new AnglrReferenceNode (p__parser_part_, p__parser_part_.m__identifier_.text, null, filePartListHeader);
                    if (step > 0)
                        referenceNode = _dictionary [referenceNode];
                    else
                        _dictionary [referenceNode] = referenceNode;
                    _stack.Push (referenceNode);
                }
                break;
                case SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason:
                    _stack.Pop ();
                    break;
            }
            return true;
        }

        private bool AnglrReferencesGenerator__anglr_syntax_rule__Event (SyntaxTreeCallbackReason reason, _anglr_syntax_rule_.production_kind kind, _anglr_syntax_rule_ p__anglr_syntax_rule_)
        {
            switch (reason)
            {
                case SyntaxTreeCallbackReason.TraversalPrologueCallbackReason:
                {
                    AnglrReferenceNode referenceNode = new AnglrReferenceNode (p__anglr_syntax_rule_, p__anglr_syntax_rule_.m__identifier_.text, null, nonTerminalListHeader);
                    if (step > 0)
                        referenceNode = _dictionary [referenceNode];
                    else
                        _dictionary [referenceNode] = referenceNode;
                    _stack.Push (referenceNode);
                }
                break;
                case SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason:
                    _stack.Pop ();
                    break;
            }
            return true;
        }

        private bool AnglrReferencesGenerator__anglr_syntax_production_list_name__Event (SyntaxTreeCallbackReason reason, _anglr_syntax_production_list_name_.production_kind kind, _anglr_syntax_production_list_name_ p__anglr_syntax_production_list_name_)
        {
            switch (reason)
            {
                case SyntaxTreeCallbackReason.TraversalPrologueCallbackReason:
                {
                    AnglrReferenceNode referenceNode = new AnglrReferenceNode (p__anglr_syntax_production_list_name_, p__anglr_syntax_production_list_name_.m__identifier_.text, null, _stack.Peek ());
                    if (step > 0)
                        referenceNode = _dictionary [referenceNode];
                    else
                        _dictionary [referenceNode] = referenceNode;
                    _stack.Push (referenceNode);
                }
                break;
                case SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason:
                    _stack.Pop ();
                    break;
            }
            return true;
        }

        private bool AnglrReferencesGenerator__production_name__Event (SyntaxTreeCallbackReason reason, _production_name_.production_kind kind, _production_name_ p__production_name_)
        {
            switch (reason)
            {
                case SyntaxTreeCallbackReason.TraversalPrologueCallbackReason:
                {
                    AnglrReferenceNode referenceNode = new AnglrReferenceNode (p__production_name_, p__production_name_.m__identifier_.text, null, _stack.Peek ());
                    if (step > 0)
                        referenceNode = _dictionary [referenceNode];
                    else
                        _dictionary [referenceNode] = referenceNode;
                    _stack.Push (referenceNode);
                }
                break;
                case SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason:
                    _stack.Pop ();
                    break;
            }
            return true;
        }

        private bool AnglrReferencesGenerator__associativity_specification__Event (SyntaxTreeCallbackReason reason, _associativity_specification_.production_kind kind, _associativity_specification_ p__associativity_specification_)
        {
            switch (reason)
            {
                case SyntaxTreeCallbackReason.TraversalPrologueCallbackReason:
                {
                    AnglrReferenceNode referenceNode = new AnglrReferenceNode (p__associativity_specification_, p__associativity_specification_.m__identifier_.text, null, _stack.Peek ());
                    if (step > 0)
                        referenceNode = _dictionary [referenceNode];
                    else
                        _dictionary [referenceNode] = referenceNode;
                    _stack.Push (referenceNode);
                }
                break;
                case SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason:
                    _stack.Pop ();
                    break;
            }
            return true;
        }

        private bool AnglrReferencesGenerator__marker__Event (SyntaxTreeCallbackReason reason, _marker_.production_kind kind, _marker_ p__marker_)
        {
            switch (reason)
            {
                case SyntaxTreeCallbackReason.TraversalPrologueCallbackReason:
                {
                    AnglrReferenceNode referenceNode = new AnglrReferenceNode (p__marker_, p__marker_.m__identifier_.text, null, _stack.Peek ());
                    if (step > 0)
                        referenceNode = _dictionary [referenceNode];
                    else
                        _dictionary [referenceNode] = referenceNode;
                    _stack.Push (referenceNode);
                }
                break;
                case SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason:
                    _stack.Pop ();
                    break;
            }
            return true;
        }

        private bool AnglrReferencesGenerator__name__Event (SyntaxTreeCallbackReason reason, _name_.production_kind kind, _name_ p__name_)
        {
            if (step <= 0)
                return true;
            switch (reason)
            {
                case SyntaxTreeCallbackReason.TraversalPrologueCallbackReason:
                {
                    AnglrReferenceNode referenceNode = null;
                    switch (kind)
                    {
                        case _name_.production_kind.g__name__1:
                            referenceNode = new AnglrReferenceNode (p__name_, p__name_.m__any_.text, null, terminalListHeader);
                            break;
                        case _name_.production_kind.g__name__2:
                            referenceNode = new AnglrReferenceNode (p__name_, p__name_.m__cstring_.text, null, terminalListHeader);
                            if (_dictionary.Keys.Contains (referenceNode))
                            {
                                AnglrReferenceNode anglrReference = _stack.Peek ();
                                AnglrReferenceNode anglrReferenceNode = new AnglrReferenceNode (anglrReference.node, anglrReference.name, null, referenceNode);
                                _dictionary [anglrReferenceNode] = anglrReferenceNode;
                                break;
                            }
                            break;
                        case _name_.production_kind.g__name__3:
                            referenceNode = new AnglrReferenceNode (p__name_, p__name_.m__identifier_.text, null, terminalListHeader);
                            if (_dictionary.Keys.Contains (referenceNode))
                            {
                                AnglrReferenceNode anglrReference = _stack.Peek ();
                                AnglrReferenceNode anglrReferenceNode = new AnglrReferenceNode (anglrReference.node, anglrReference.name, null, referenceNode);
                                _dictionary [anglrReferenceNode] = anglrReferenceNode;
                                break;
                            }
                            referenceNode = new AnglrReferenceNode (p__name_, p__name_.m__identifier_.text, null, nonTerminalListHeader);
                            if (_dictionary.Keys.Contains (referenceNode))
                            {
                                AnglrReferenceNode anglrReference = _stack.Peek ();
                                AnglrReferenceNode anglrReferenceNode = new AnglrReferenceNode (anglrReference.node, anglrReference.name, null, referenceNode);
                                _dictionary [anglrReferenceNode] = anglrReferenceNode;
                                break;
                            }
                            break;
                    }
                }
                break;
                case SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason:
                    break;
            }
            return true;
        }

        public void GenerateReferences ()
        {
            step = 0;
            foreach (SyntaxTreeBase node in compiler.parseList)
                node.InvokeTraverse (this);
            ++step;
            foreach (SyntaxTreeBase node in compiler.parseList)
                node.InvokeTraverse (this);
        }

        public void Display (IAnglrLogger logger) => _dictionary.Display (logger);
    }
}
