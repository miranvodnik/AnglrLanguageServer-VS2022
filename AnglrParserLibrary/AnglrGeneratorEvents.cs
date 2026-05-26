using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Anglr.Parser;

namespace AnglrLibrary
{
    public partial class AnglrGenerator : SyntaxTreeWalker
    {
        /// <summary>
        /// this function, when traversed through syntax tree, provides each node of this tree 
        /// whith an empty application info which should be used to serve application specific needs
        /// </summary>
        /// <param name="reason">call reason indicating traversal step</param>
        /// <param name="kind">unused - not important in this traversal function</param>
        /// <param name="p_node">current syntax tree node reference superclassed to SyntaxTreeBase</param>
        /// <returns></returns>
        internal bool Invoke_Common_Callback (SyntaxTreeCallbackReason reason, int kind, SyntaxTreeBase p_node)
        {
            if (reason == SyntaxTreeCallbackReason.TraversalPrologueCallbackReason)
                p_node.appInfo = new AppInfo ();
            return true;
        }

        /// <summary>
        /// syntax error reporting function which actually does nothing since it is used to skip these errors
        /// </summary>
        /// <param name="lineno">not used</param>
        /// <param name="column">not used</param>
        /// <param name="token">not used</param>
        /// <param name="tokenString">not used</param>
        /// <returns>false - skip error (true should break parsing algorithm on first error)</returns>
        private bool Invoke_Error_Callback (int lineno, int column, int token, string tokenString)
        {
            return false;
        }

        private bool Invoke__anglr_file_part_list__Callback (SyntaxTreeCallbackReason reason, _anglr_file_part_list_.production_kind kind, _anglr_file_part_list_ p__anglr_file_part_list_)
        {
            bool result = false;    // custom logic for compilation of different source file parts
            switch (reason)
            {
                case SyntaxTreeCallbackReason.BuilderCallbackReason:
                    break;
                case SyntaxTreeCallbackReason.TraversalPrologueCallbackReason:
                    break;
                case SyntaxTreeCallbackReason.TraversalMidTermCallbackReason:
                    break;
                case SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason:
                {
                    // 1st step: compile all general parts in order of appearance
                    p__anglr_file_part_list_.CustomIterate (null, (SyntaxTreeBase node, object appData) =>
                    {
                        _anglr_file_part_ p__anglr_file_part_ = (_anglr_file_part_) node;
                        if (p__anglr_file_part_.kind == (uint) _anglr_file_part_.production_kind.g__anglr_file_part__1)
                            Traverse (p__anglr_file_part_.m__general_part_);
                        return null;
                    });
                    // 2nd step: compile all declaration parts in order of appearance
                    p__anglr_file_part_list_.CustomIterate (null, (SyntaxTreeBase node, object appData) =>
                    {
                        _anglr_file_part_ p__anglr_file_part_ = (_anglr_file_part_) node;
                        if (p__anglr_file_part_.kind == (uint) _anglr_file_part_.production_kind.g__anglr_file_part__2)
                            Traverse (p__anglr_file_part_.m__declaration_part_);
                        return null;
                    });
                    // 3rd step: compile all scanner parts in order of appearance
                    p__anglr_file_part_list_.CustomIterate (null, (SyntaxTreeBase node, object appData) =>
                    {
                        _anglr_file_part_ p__anglr_file_part_ = (_anglr_file_part_) node;
                        if (p__anglr_file_part_.kind == (uint) _anglr_file_part_.production_kind.g__anglr_file_part__3)
                            Traverse (p__anglr_file_part_.m__scanner_part_);
                        return null;
                    });
                    // 4th step: compile all parser parts in order of appearance
                    p__anglr_file_part_list_.CustomIterate (null, (SyntaxTreeBase node, object appData) =>
                    {
                        _anglr_file_part_ p__anglr_file_part_ = (_anglr_file_part_) node;
                        if (p__anglr_file_part_.kind == (uint) _anglr_file_part_.production_kind.g__anglr_file_part__4)
                            Traverse (p__anglr_file_part_.m__parser_part_);
                        return null;
                    });
                }
                break;
            }
            return result;
        }

        /// <summary>
        /// &lt;general part> rule traversal function.
        /// It remembers general part rule name and its defining reference.
        /// It actually does this in the epilogue step of traversal.
        /// </summary>
        /// <param name="reason">traversal step - only epilogue is interested in this traversal function</param>
        /// <param name="kind">syntax rule production indicator - not important</param>
        /// <param name="p__general_part_">&lt;general part> node reference</param>
        /// <returns>true - don't skip traversal of underlying subtree</returns>
        private bool Invoke__general_part__Callback (SyntaxTreeCallbackReason reason, _general_part_.production_kind kind, _general_part_ p__general_part_)
        {
            bool result = true;
            switch (reason)
            {
                case SyntaxTreeCallbackReason.BuilderCallbackReason:
                    break;
                case SyntaxTreeCallbackReason.TraversalPrologueCallbackReason:
                    break;
                case SyntaxTreeCallbackReason.TraversalMidTermCallbackReason:
                    break;
                case SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason:
                {
                    SyntaxTreeToken p_identifier = p__general_part_.m__identifier_;
                    SymbolToken p_SymbolToken = new SymbolToken (p_identifier.text, (uint) AnglrClassificationType.GeneralPartName);
                    SymbolToken p_SymbolTokenRef = m_symtab.insert (p_SymbolToken);
                    if (p_SymbolTokenRef != p_SymbolToken)
                    {
                        if (p_SymbolTokenRef.declarator != (uint) AnglrClassificationType.GeneralPartName)
                            ;
                    }
                        ((AppInfo) p__general_part_.appInfo) [AppInfoType.SymbolToken] = p_SymbolTokenRef;
                    ((AppInfo) p_identifier.appInfo) [AppInfoType.SymbolToken] = p_SymbolTokenRef;
                    p_SymbolTokenRef.AddDefInfo (p_identifier.lineno, p_identifier.column, p_SymbolTokenRef.name.Length);
                }
                break;
            }
            return result;
        }

        /// <summary>
        /// &lt;declaration part> rule traversal function.
        /// It remembers declaration part rule name and its defining reference.
        /// It actually does this in the epilogue step of traversal.
        /// </summary>
        /// <param name="reason">traversal step - only epilogue step is interesting in this traversal function</param>
        /// <param name="kind">syntax rule production indicator - not important</param>
        /// <param name="p__declaration_part_">&lt;declaration part> node</param>
        /// <returns>true - don't skip traversal of underlying subtree</returns>
        private bool Invoke__declaration_part__Callback (SyntaxTreeCallbackReason reason, _declaration_part_.production_kind kind, _declaration_part_ p__declaration_part_)
        {
            bool result = true;
            switch (reason)
            {
                case SyntaxTreeCallbackReason.BuilderCallbackReason:
                    break;
                case SyntaxTreeCallbackReason.TraversalPrologueCallbackReason:
                    break;
                case SyntaxTreeCallbackReason.TraversalMidTermCallbackReason:
                    break;
                case SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason:
                {
                    SyntaxTreeToken p_identifier = p__declaration_part_.m__identifier_;
                    SymbolToken p_SymbolToken = new SymbolToken (p_identifier.text, (uint) AnglrClassificationType.DeclarationsPartName);
                    SymbolToken p_SymbolTokenRef = m_symtab.insert (p_SymbolToken);
                    if (p_SymbolTokenRef != p_SymbolToken)
                    {
                        if (p_SymbolTokenRef.declarator != (uint) AnglrClassificationType.DeclarationsPartName)
                            ;
                    }
                        ((AppInfo) p__declaration_part_.appInfo) [AppInfoType.SymbolToken] = p_SymbolTokenRef;
                    ((AppInfo) p_identifier.appInfo) [AppInfoType.SymbolToken] = p_SymbolTokenRef;
                    p_SymbolTokenRef.AddDefInfo (p_identifier.lineno, p_identifier.column, p_SymbolTokenRef.name.Length);
                }
                break;
            }
            return result;
        }

        private bool Invoke__scanner_part__Callback (SyntaxTreeCallbackReason reason, _scanner_part_.production_kind kind, _scanner_part_ p__scanner_part_)
        {
            bool result = true;
            switch (reason)
            {
                case SyntaxTreeCallbackReason.BuilderCallbackReason:
                    break;
                case SyntaxTreeCallbackReason.TraversalPrologueCallbackReason:
                    break;
                case SyntaxTreeCallbackReason.TraversalMidTermCallbackReason:
                    break;
                case SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason:
                {
                    SyntaxTreeToken p_identifier = p__scanner_part_.m__identifier_;
                    SymbolToken p_SymbolToken = new SymbolToken (p_identifier.text, (uint) AnglrClassificationType.ScannerPartName);
                    SymbolToken p_SymbolTokenRef = m_symtab.insert (p_SymbolToken);
                    if (p_SymbolTokenRef != p_SymbolToken)
                    {
                        if (p_SymbolTokenRef.declarator != (uint) AnglrClassificationType.ScannerPartName)
                            ;
                    }
                        ((AppInfo) p__scanner_part_.appInfo) [AppInfoType.SymbolToken] = p_SymbolTokenRef;
                    ((AppInfo) p_identifier.appInfo) [AppInfoType.SymbolToken] = p_SymbolTokenRef;
                    p_SymbolTokenRef.AddDefInfo (p_identifier.lineno, p_identifier.column, p_SymbolTokenRef.name.Length);
                }
                break;
            }
            return result;
        }

        private bool Invoke__regular_expression_list__Callback (SyntaxTreeCallbackReason reason, _regular_expression_list_.production_kind kind, _regular_expression_list_ p__regular_expression_list_)
        {
            bool result = false;    // prevent recursive calls of this callback since list of tokens should be very long
            switch (reason)
            {
                case SyntaxTreeCallbackReason.BuilderCallbackReason:
                    break;
                case SyntaxTreeCallbackReason.TraversalPrologueCallbackReason:
                    break;
                case SyntaxTreeCallbackReason.TraversalMidTermCallbackReason:
                    break;
                case SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason:
                {
                    p__regular_expression_list_.CustomIterate (null, (SyntaxTreeBase node, object appData) =>
                    {
                        Traverse ((_regular_expression_usage_) node);
                        return null;
                    });
                }
                break;
            }
            return result;
        }

        private bool Invoke__parser_part__Callback (SyntaxTreeCallbackReason reason, _parser_part_.production_kind kind, _parser_part_ p__parser_part_)
        {
            bool result = true;
            switch (reason)
            {
                case SyntaxTreeCallbackReason.BuilderCallbackReason:
                    break;
                case SyntaxTreeCallbackReason.TraversalPrologueCallbackReason:
                    break;
                case SyntaxTreeCallbackReason.TraversalMidTermCallbackReason:
                    break;
                case SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason:
                {
                    SyntaxTreeToken p_identifier = p__parser_part_.m__identifier_;
                    SymbolToken p_SymbolToken = new SymbolToken (p_identifier.text, (uint) AnglrClassificationType.ParserPartName);
                    SymbolToken p_SymbolTokenRef = m_symtab.insert (p_SymbolToken);
                    if (p_SymbolTokenRef != p_SymbolToken)
                    {
                        if (p_SymbolTokenRef.declarator != (uint) AnglrClassificationType.ParserPartName)
                            ;
                    }
                        ((AppInfo) p__parser_part_.appInfo) [AppInfoType.SymbolToken] = p_SymbolTokenRef;
                    ((AppInfo) p_identifier.appInfo) [AppInfoType.SymbolToken] = p_SymbolTokenRef;
                    p_SymbolTokenRef.AddDefInfo (p_identifier.lineno, p_identifier.column, p_SymbolTokenRef.name.Length);
                }
                break;
            }
            return result;
        }

        private bool Invoke__attribute__Callback (SyntaxTreeCallbackReason reason, _attribute_.production_kind kind, _attribute_ p__attribute_)
        {
            bool result = true;
            switch (reason)
            {
                case SyntaxTreeCallbackReason.BuilderCallbackReason:
                    break;
                case SyntaxTreeCallbackReason.TraversalPrologueCallbackReason:
                    break;
                case SyntaxTreeCallbackReason.TraversalMidTermCallbackReason:
                    break;
                case SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason:
                {
                    SyntaxTreeToken p_identifier = p__attribute_.m__identifier_;
                    SymbolToken p_SymbolToken = new SymbolToken (p_identifier.text, (uint) AnglrClassificationType.AttributeName);
                    SymbolToken p_SymbolTokenRef = m_symtab.insert (p_SymbolToken);
                    if (p_SymbolTokenRef != p_SymbolToken)
                    {
                        if (p_SymbolTokenRef.declarator != (uint) AnglrClassificationType.AttributeName)
                            ;
                    }
                        ((AppInfo) p__attribute_.appInfo) [AppInfoType.SymbolToken] = p_SymbolTokenRef;
                    ((AppInfo) p_identifier.appInfo) [AppInfoType.SymbolToken] = p_SymbolTokenRef;
                    p_SymbolTokenRef.AddDefInfo (p_identifier.lineno, p_identifier.column, p_SymbolTokenRef.name.Length);
                }
                break;
            }
            return result;
        }

        private bool Invoke__name_value_pair__Callback (SyntaxTreeCallbackReason reason, _name_value_pair_.production_kind kind, _name_value_pair_ p__name_value_pair_)
        {
            bool result = true;
            switch (reason)
            {
                case SyntaxTreeCallbackReason.BuilderCallbackReason:
                    break;
                case SyntaxTreeCallbackReason.TraversalPrologueCallbackReason:
                    break;
                case SyntaxTreeCallbackReason.TraversalMidTermCallbackReason:
                    break;
                case SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason:
                {
                    SyntaxTreeToken p_identifier = p__name_value_pair_.m__identifier_;
                    SymbolToken p_SymbolToken = new SymbolToken (p_identifier.text, (uint) AnglrClassificationType.PropertyName);
                    SymbolToken p_SymbolTokenRef = m_symtab.insert (p_SymbolToken);
                    if (p_SymbolTokenRef != p_SymbolToken)
                    {
                        if (p_SymbolTokenRef.declarator != (uint) AnglrClassificationType.PropertyName)
                            ;
                    }
                        ((AppInfo) p__name_value_pair_.appInfo) [AppInfoType.SymbolToken] = p_SymbolTokenRef;
                    ((AppInfo) p_identifier.appInfo) [AppInfoType.SymbolToken] = p_SymbolTokenRef;
                    p_SymbolTokenRef.AddDefInfo (p_identifier.lineno, p_identifier.column, p_SymbolTokenRef.name.Length);
                }
                break;
            }
            return result;
        }

        internal bool Invoke__anglr_definition_list__Callback (SyntaxTreeCallbackReason reason, _anglr_definition_list_.production_kind kind, _anglr_definition_list_ p__anglr_definition_list_)
        {
            bool result = false;    // prevent recursive calls of this callback since list of tokens should be very long
            switch (reason)
            {
                case SyntaxTreeCallbackReason.BuilderCallbackReason:
                    break;
                case SyntaxTreeCallbackReason.TraversalPrologueCallbackReason:
                    break;
                case SyntaxTreeCallbackReason.TraversalMidTermCallbackReason:
                    break;
                case SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason:  // iterate through list in reverse order
                {
                    p__anglr_definition_list_.CustomIterate (null, (SyntaxTreeBase node, object appData) =>
                    {
                        Traverse ((_anglr_definition_) node);
                        return null;
                    });
                }
                break;
            }
            return result;
        }

        internal bool Invoke__anglr_definition__Callback (SyntaxTreeCallbackReason reason, _anglr_definition_.production_kind kind, _anglr_definition_ p__anglr_definition_)
        {
            bool result = true;
            switch (reason)
            {
                case SyntaxTreeCallbackReason.BuilderCallbackReason:
                    break;
                case SyntaxTreeCallbackReason.TraversalPrologueCallbackReason:
                    break;
                case SyntaxTreeCallbackReason.TraversalMidTermCallbackReason:
                    break;
                case SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason:
                {
                    switch (kind)
                    {
                        case _anglr_definition_.production_kind.g__anglr_definition__1:
                            break;
                        case _anglr_definition_.production_kind.g__anglr_definition__2:
                        {
                            SyntaxTreeToken p_identifier = p__anglr_definition_.m__identifier_;
                            SymbolToken p_SymbolToken = new SymbolToken (p_identifier.text, (uint) AnglrClassificationType.RegexName);
                            SymbolToken p_SymbolTokenRef = m_symtab.insert (p_SymbolToken);
                            if (p_SymbolTokenRef != p_SymbolToken)
                            {
                                if (p_SymbolTokenRef.declarator == (uint) AnglrClassificationType.RegexName)
                                    ;
                            }
                                ((AppInfo) p__anglr_definition_.appInfo) [AppInfoType.SymbolToken] = p_SymbolTokenRef;
                            ((AppInfo) p_identifier.appInfo) [AppInfoType.SymbolToken] = p_SymbolTokenRef;
                            p_SymbolTokenRef.AddDefInfo (p_identifier.lineno, p_identifier.column, p_SymbolTokenRef.name.Length);
                        }
                        break;
                    }
                }
                break;
            }
            return result;
        }

        private bool Invoke__token_string__Callback (SyntaxTreeCallbackReason reason, _token_string_.production_kind kind, _token_string_ p__token_string_)
        {
            bool result = false;    // prevent recursive calls of this callback since list of tokens should be very long
            switch (reason)
            {
                case SyntaxTreeCallbackReason.BuilderCallbackReason:
                    break;
                case SyntaxTreeCallbackReason.TraversalPrologueCallbackReason:
                    break;
                case SyntaxTreeCallbackReason.TraversalMidTermCallbackReason:
                    break;
                case SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason:
                {
                    p__token_string_.CustomIterate (null, (SyntaxTreeBase node, object appData) =>
                    {
                        Traverse ((_token_definition_) node);
                        return null;
                    });
                }
                break;
            }
            return result;
        }

        internal bool Invoke__token_definition__Callback (SyntaxTreeCallbackReason reason, _token_definition_.production_kind kind, _token_definition_ p__token_definition_)
        {
            bool result = true;
            switch (reason)
            {
                case SyntaxTreeCallbackReason.BuilderCallbackReason:
                    break;
                case SyntaxTreeCallbackReason.TraversalPrologueCallbackReason:
                    break;
                case SyntaxTreeCallbackReason.TraversalMidTermCallbackReason:
                    break;
                case SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason:
                {
                    SyntaxTreeToken p__identifier_ = p__token_definition_.m__identifier_;
                    SyntaxTreeToken p__cstring_ = p__token_definition_.m__cstring_optional_.m__cstring_;
                    if (p__cstring_ == null)
                    {
                        SymbolToken p_SymbolToken = new SymbolToken (p__identifier_.text, (uint) AnglrClassificationType.TerminalName);
                        SymbolToken p_SymbolTokenRef = m_symtab.insert (p_SymbolToken);
                        if (p_SymbolTokenRef != p_SymbolToken)
                            p_SymbolToken.Dispose ();
                        ((AppInfo) p__token_definition_.appInfo) [AppInfoType.SymbolToken] = p_SymbolTokenRef;
                        ((AppInfo) p__identifier_.appInfo) [AppInfoType.SymbolToken] = p_SymbolTokenRef;
                        p_SymbolTokenRef.AddDefInfo (p__identifier_.lineno, p__identifier_.column, p_SymbolTokenRef.name.Length);
                    }
                    else
                    {
                        SymbolToken p_SymbolToken = new SymbolToken (p__identifier_.text, (uint) AnglrClassificationType.TerminalName);
                        SymbolToken p_SymbolTokenRef = m_symtab.insert (p_SymbolToken);
                        if (p_SymbolTokenRef != p_SymbolToken)
                            p_SymbolToken.Dispose ();
                        SymbolToken p_aliasToken = new SymbolToken (p__cstring_.text, (uint) AnglrClassificationType.TerminalName, null, null, false);
                        SymbolToken p_aliasTokenRef = m_symtab.insert (p_aliasToken);
                        if (p_aliasTokenRef != p_aliasToken)
                            p_aliasToken.Dispose ();
                        p_SymbolTokenRef.alias = p_aliasTokenRef;
                        p_aliasTokenRef.alias = p_SymbolTokenRef;
                        p_aliasTokenRef.setAlias ();
                        ((AppInfo) p__token_definition_.appInfo) [AppInfoType.SymbolToken] = p_SymbolTokenRef;
                        ((AppInfo) p__identifier_.appInfo) [AppInfoType.SymbolToken] = p_SymbolTokenRef;
                        ((AppInfo) p__cstring_.appInfo) [AppInfoType.SymbolToken] = p_aliasTokenRef;
                        p_SymbolTokenRef.AddDefInfo (p__identifier_.lineno, p__identifier_.column, p_SymbolTokenRef.name.Length);
                        p_aliasTokenRef.AddDefInfo (p__cstring_.lineno, p__cstring_.column, p_aliasTokenRef.name.Length);
                    }
                    break;
                }
            }
            return result;
        }

        internal bool Invoke__anglr_syntax_rule_list__Callback (SyntaxTreeCallbackReason reason, _anglr_syntax_rule_list_.production_kind kind, _anglr_syntax_rule_list_ p__anglr_syntax_rule_list_)
        {
            bool result = false;    // prevent recursive calls of this callback since list of syntax rules should be very long
            switch (reason)
            {
                case SyntaxTreeCallbackReason.BuilderCallbackReason:
                    break;
                case SyntaxTreeCallbackReason.TraversalPrologueCallbackReason:
                    break;
                case SyntaxTreeCallbackReason.TraversalMidTermCallbackReason:
                    break;
                case SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason:  // iterate through list in reverse order
                {
                    p__anglr_syntax_rule_list_.CustomIterate (null, (SyntaxTreeBase node, object appData) =>
                    {
                        Traverse ((_anglr_syntax_rule_) node);
                        return null;
                    });
                }
                break;
            }
            return result;
        }

        internal bool Invoke__anglr_syntax_rule__Callback (SyntaxTreeCallbackReason reason, _anglr_syntax_rule_.production_kind kind, _anglr_syntax_rule_ p__anglr_syntax_rule_)
        {
            bool result = true;
            switch (reason)
            {
                case SyntaxTreeCallbackReason.BuilderCallbackReason:
                    break;
                case SyntaxTreeCallbackReason.TraversalPrologueCallbackReason:
                {
                    if (kind != _anglr_syntax_rule_.production_kind.g__anglr_syntax_rule__1)
                        break;

                    SyntaxTreeToken p_identifier = p__anglr_syntax_rule_.m__identifier_;
                    SymbolToken p_SymbolToken = new SymbolToken (p_identifier.text);
                    SymbolToken p_SymbolTokenRef = m_symtab.insert (p_SymbolToken);
                    if (p_SymbolTokenRef != p_SymbolToken)
                    {
                        p_SymbolToken.Dispose ();
                        uint declarator = p_SymbolTokenRef.declarator;
                        if (declarator == (uint) AnglrClassificationType.TerminalName)
                            ; // Console.WriteLine (p_SymbolTokenRef.name + ": definition of rule previously defined as token");
                    }
                        ((AppInfo) p_identifier.appInfo) [AppInfoType.SymbolToken] = p_SymbolTokenRef;
                    p_SymbolTokenRef.declarator = (uint) AnglrClassificationType.NonTerminalName;
                    p_SymbolTokenRef.AddDefInfo (p_identifier.lineno, p_identifier.column, p_SymbolTokenRef.name.Length);
                }
                break;
                case SyntaxTreeCallbackReason.TraversalMidTermCallbackReason:
                    break;
                case SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason:
                    break;
            }
            return result;
        }

        private bool Invoke__anglr_syntax_production_list_name__Callback (SyntaxTreeCallbackReason reason, _anglr_syntax_production_list_name_.production_kind kind, _anglr_syntax_production_list_name_ p__anglr_syntax_production_list_name_)
        {
            bool result = true;
            switch (reason)
            {
                case SyntaxTreeCallbackReason.BuilderCallbackReason:
                    break;
                case SyntaxTreeCallbackReason.TraversalPrologueCallbackReason:
                {
                    SyntaxTreeToken p_identifier = p__anglr_syntax_production_list_name_.m__identifier_;
                    SymbolToken p_SymbolToken = new SymbolToken (p_identifier.text);
                    SymbolToken p_SymbolTokenRef = m_symtab.insert (p_SymbolToken);
                    if (p_SymbolTokenRef != p_SymbolToken)
                    {
                        p_SymbolToken.Dispose ();
                        uint declarator = p_SymbolTokenRef.declarator;
                        if (declarator == (uint) AnglrClassificationType.TerminalName)
                            ; // Console.WriteLine (p_SymbolTokenRef.name + ": definition of rule previously defined as token");
                    }
                        ((AppInfo) p_identifier.appInfo) [AppInfoType.SymbolToken] = p_SymbolTokenRef;
                    p_SymbolTokenRef.declarator = (uint) AnglrClassificationType.NonTerminalName;
                    p_SymbolTokenRef.AddDefInfo (p_identifier.lineno, p_identifier.column, p_SymbolTokenRef.name.Length);
                }
                break;
                case SyntaxTreeCallbackReason.TraversalMidTermCallbackReason:
                    break;
                case SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason:
                    break;
            }
            return result;
        }

        internal bool Invoke__anglr_syntax_production__Callback (SyntaxTreeCallbackReason reason, _anglr_syntax_production_.production_kind kind, _anglr_syntax_production_ p__anglr_syntax_production_)
        {
            bool result = true;
            switch (reason)
            {
                case SyntaxTreeCallbackReason.BuilderCallbackReason:
                    break;
                case SyntaxTreeCallbackReason.TraversalPrologueCallbackReason:
                    break;
                case SyntaxTreeCallbackReason.TraversalMidTermCallbackReason:
                    break;
                case SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason:
                {
                    if (kind != _anglr_syntax_production_.production_kind.g__anglr_syntax_production__1)
                        ;
                    p__anglr_syntax_production_.m__name_list_.CustomIterate (null, (SyntaxTreeBase node, object appData) =>
                    {
                        if (!(node is _g_name_))
                            return null;
                        _name_ p__name_ = ((_g_name_) node).m__name_;
                        if (p__name_ == null)
                            return null;
                        SymbolToken p_SymbolToken = (SymbolToken) ((AppInfo) p__name_.appInfo) [AppInfoType.SymbolToken];
                        if (p_SymbolToken.getAlias () != 0)
                            p_SymbolToken = p_SymbolToken.alias;
                        return null;
                    });
                }
                break;
            }
            return result;
        }

        internal bool Invoke__name__Callback (SyntaxTreeCallbackReason reason, _name_.production_kind kind, _name_ p__name_)
        {
            bool result = true;
            switch (reason)
            {
                case SyntaxTreeCallbackReason.BuilderCallbackReason:
                    break;
                case SyntaxTreeCallbackReason.TraversalPrologueCallbackReason:
                    break;
                case SyntaxTreeCallbackReason.TraversalMidTermCallbackReason:
                    break;
                case SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason:
                {
                    int lineno = -1;
                    int column = -1;
                    uint declarator = 0;
                    SymbolToken p_SymbolToken = null;
                    switch (kind)
                    {
                        case _name_.production_kind.g__name__1:
                            p_SymbolToken = new SymbolToken (p__name_.m__any_.text);
                            declarator = (uint) AnglrClassificationType.TerminalName;
                            lineno = p__name_.m__any_.lineno;
                            column = p__name_.m__any_.column;
                            break;
                        case _name_.production_kind.g__name__2:
                            p_SymbolToken = new SymbolToken (p__name_.m__cstring_.text);
                            declarator = (uint) AnglrClassificationType.TerminalName;
                            lineno = p__name_.m__cstring_.lineno;
                            column = p__name_.m__cstring_.column;
                            break;
                        case _name_.production_kind.g__name__3:
                            p_SymbolToken = new SymbolToken (p__name_.m__identifier_.text);
                            lineno = p__name_.m__identifier_.lineno;
                            column = p__name_.m__identifier_.column;
                            break;
                    }
                    SymbolToken p_SymbolTokenRef = m_symtab.insert (p_SymbolToken);
                    if (p_SymbolTokenRef != p_SymbolToken)
                        p_SymbolToken.Dispose ();
                    if ((declarator != 0) && (p_SymbolTokenRef.declarator == 0))
                        p_SymbolTokenRef.declarator = declarator;
                    ((AppInfo) p__name_.appInfo) [AppInfoType.SymbolToken] = p_SymbolTokenRef;
                    p_SymbolTokenRef.AddRefInfo (lineno, column, p_SymbolTokenRef.name.Length);
                }
                break;
            }
            return result;
        }
    }
}
