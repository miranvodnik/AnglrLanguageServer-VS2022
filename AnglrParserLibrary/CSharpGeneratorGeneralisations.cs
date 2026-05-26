using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Anglr.Parser;
using Anglr.Parser.SyntaxTree;
using Anglr.Compiler;
using Anglr.Declarations;

namespace AnglrLibrary
{
    public partial class CSharpBaseGenerator : SyntaxTreeWalker
    {
        public void InitGeneralizations ()
        {
            m_generatedRuleIndex = 1;
            m_discardProducton = false;
            m_simpleRuleName = null;
            m_simpleNodeName = null;
            m_generatedRuleList = new List<_anglr_syntax_rule_> ();
        }

        private void NormalizeCompoundName (_g_name_ p__g_name_)
        {
            if (p__g_name_.kind == (uint) _g_name_.production_kind.g__g_name__1)
                return;

            m_discardProducton = ((m_simpleRuleName = CheckSimpleRuleCondition (p__g_name_)) != null);
            m_simpleNodeName = CheckSimpleNodeCondition (p__g_name_);

            _name_ name_ = null;
            switch (p__g_name_.kind)
            {
                case (uint) _g_name_.production_kind.g__g_name__2:
                    name_ = NormalizeProdList (p__g_name_);
                    break;
                case (uint) _g_name_.production_kind.g__g_name__3:
                    name_ = NormalizeGNameWithCardinality (p__g_name_.m__g_name_, p__g_name_.m__cardinality_delimiter_);
                    break;
            }
            m_simpleRuleName = null;
            m_simpleNodeName = null;
            if (name_ == null)
                return;

            p__g_name_.change (name_);
        }

        private _name_ NormalizeGNameWithCardinality (_g_name_ p__g_name_, _cardinality_delimiter_ p__cardinality_delimiter_)
        {
            if (p__g_name_.kind != (uint) _g_name_.production_kind.g__g_name__1)
                return null;

            _name_ p__name_ = p__g_name_.m__name_;
            _name_ name_ = null;
            _cardinality_ cardinality_ = p__cardinality_delimiter_.m__cardinality_;
            _delimiter_ delimiter_ = p__cardinality_delimiter_.m__delimiter_optional_.m__delimiter_;

            switch ((_cardinality_.production_kind) cardinality_.kind)
            {
                case _cardinality_.production_kind.g__cardinality__1:
                    name_ = NormalizeNWCOption (p__name_, null);
                    break;
                case _cardinality_.production_kind.g__cardinality__2:
                    name_ = NormalizeNWCSet (p__name_, delimiter_, null);
                    break;
                case _cardinality_.production_kind.g__cardinality__3:
                    name_ = NormalizeINWCSet (p__name_, delimiter_, null);
                    break;
                case _cardinality_.production_kind.g__cardinality__4:
                    name_ = NormalizeNWCOptionalSet (p__name_, delimiter_, null);
                    break;
                case _cardinality_.production_kind.g__cardinality__5:
                    name_ = NormalizeINWCOptionalSet (p__name_, delimiter_, null);
                    break;
                case _cardinality_.production_kind.g__cardinality__6:
                    name_ = NormalizeLNWCSet (p__name_, delimiter_, null);
                    break;
                case _cardinality_.production_kind.g__cardinality__7:
                    name_ = NormalizeLINWCSet (p__name_, delimiter_, null);
                    break;
                case _cardinality_.production_kind.g__cardinality__8:
                    name_ = NormalizeLNWCOptionalSet (p__name_, delimiter_, null);
                    break;
                case _cardinality_.production_kind.g__cardinality__9:
                    name_ = NormalizeLINWCOptionalSet (p__name_, delimiter_, null);
                    break;
                case _cardinality_.production_kind.g__cardinality__10:
                    _number_optional_ minnr = cardinality_.m__number_optional_;
                    _number_optional_ maxnr = cardinality_.m__number_optional__1;
                    if (maxnr.m__number_ == null)
                    {
                        if (minnr.m__number_ == null)
                        {
                            name_ = NormalizeNWCOptionalSet (p__name_, delimiter_, null);
                        }
                        else
                        {
                            int number = int.Parse (minnr.m__number_.text);
                            if (number > 0)
                                name_ = NormalizeNWCSet (p__name_, delimiter_, null);
                            else
                                name_ = NormalizeNWCOptionalSet (p__name_, delimiter_, null);
                        }
                    }
                    else
                    {
                        if (minnr.m__number_ == null)
                        {
                            int number = int.Parse (maxnr.m__number_.text);
                            if (number > 0)
                                name_ = NormalizeNWCSet (p__name_, delimiter_, null);
                        }
                        else
                        {
                            int lband = int.Parse (minnr.m__number_.text);
                            int uband = int.Parse (maxnr.m__number_.text);
                            if (lband > uband)
                                ;
                            else if (lband > 0)
                            {
                                if (lband == uband)
                                    name_ = (_name_) p__name_.Clone ();
                                else
                                    name_ = NormalizeNWCSet (p__name_, delimiter_, null);
                            }
                            else
                            {
                                if (lband == uband)
                                    ;
                                else
                                    name_ = NormalizeNWCOptionalSet (p__name_, delimiter_, null);
                            }
                        }
                    }
                    break;
            }
            return name_;
        }

        private _name_ NormalizeNWCOption (_name_ p__name_, string dname)
        {
            string name = (m_simpleRuleName != null) ? m_simpleRuleName.text : (m_simpleNodeName != null) ? GenerateNodeName (m_simpleNodeName, new string [] { "optional" }, dname) : $"<generated-rule-{m_generatedRuleIndex++}>";
            string rule = $"{name}:%empty|{p__name_.Emit (-1)};";
            return CompileGeneratedRule (rule, false, new object [] { name, "Generated Rule: ", rule});
        }

        private _name_ NormalizeNWCSet (_name_ p__name_, _delimiter_ delimiter_, string dname)
        {
            string name = (m_simpleRuleName != null) ? m_simpleRuleName.text : (m_simpleNodeName != null) ? GenerateNodeName (m_simpleNodeName, new string [] { "set" }, dname) : $"<generated-rule-{m_generatedRuleIndex++}>";
            string rule;
            if (delimiter_ != null)
            {
                _anglr_nested_rule_ p__anglr_nested_rule_ = delimiter_.m__anglr_nested_rule_;
                _anglr_syntax_production_list_name_optional_ p__anglr_syntax_production_list_name_optional_ = p__anglr_nested_rule_.m__anglr_syntax_production_list_name_optional_;
                _anglr_syntax_production_list_name_ p__anglr_syntax_production_list_name_ = p__anglr_syntax_production_list_name_optional_.m__anglr_syntax_production_list_name_;
                if (p__anglr_syntax_production_list_name_ != null)
                    name = p__anglr_syntax_production_list_name_.m__identifier_.text;
                rule = $"{name}:{p__name_.Emit (-1)}";
                p__anglr_nested_rule_.m__anglr_syntax_production_list_?.Iterate (rule, (node, appData) =>
                    {
                        _anglr_syntax_production_ p__anglr_syntax_production_ = node.m__anglr_syntax_production_;
                        if (p__anglr_syntax_production_.kind != (uint) _anglr_syntax_production_.production_kind.g__anglr_syntax_production__1)
                            rule += $"|{name} {p__name_.Emit (-1)}";
                        else
                            rule += $"|{name} {node.m__anglr_syntax_production_.Emit (-1)} {p__name_.Emit (-1)}";
                        return null;
                    });
                rule += ";";
            }
            else
                rule = $"{name}:{p__name_.Emit (-1)}|{name} {p__name_.Emit (-1)};";
            return CompileGeneratedRule (rule, true, new object [] { name, "Generated Rule: ", rule });
        }

        private _name_ NormalizeINWCSet (_name_ p__name_, _delimiter_ delimiter_, string dname)
        {
            string name = (m_simpleRuleName != null) ? m_simpleRuleName.text : (m_simpleNodeName != null) ? GenerateNodeName (m_simpleNodeName, new string [] { "set" }, dname) : $"<generated-rule-{m_generatedRuleIndex++}>";
            string rule;
            if (delimiter_ != null)
            {
                _anglr_nested_rule_ p__anglr_nested_rule_ = delimiter_.m__anglr_nested_rule_;
                SyntaxTreeToken token = CheckSimpleRuleCondition (p__anglr_nested_rule_);
                if (token != null)
                    rule = $"{name}:{token.text}|{name} {p__name_.Emit (-1)} {token.text};";
                else
                {
                    _anglr_syntax_production_list_name_optional_ p__anglr_syntax_production_list_name_optional_ = p__anglr_nested_rule_.m__anglr_syntax_production_list_name_optional_;
                    _anglr_syntax_production_list_name_ p__anglr_syntax_production_list_name_ = p__anglr_syntax_production_list_name_optional_.m__anglr_syntax_production_list_name_;
                    if (p__anglr_syntax_production_list_name_ != null)
                        name = p__anglr_syntax_production_list_name_.m__identifier_.text;
                    rule = $"{name}:";
                    int index = 0;
                    p__anglr_nested_rule_.m__anglr_syntax_production_list_?.Iterate (null, (node, appInfo) =>
                        {
                            _anglr_syntax_production_ p__anglr_syntax_production_ = node.m__anglr_syntax_production_;
                            if (p__anglr_syntax_production_.kind != (uint) _anglr_syntax_production_.production_kind.g__anglr_syntax_production__1)
                                return null;
                            if (index++ != 0)
                                rule += "|";
                            rule += $"{node.m__anglr_syntax_production_.Emit (-1)}";
                            return null;
                        });
                    p__anglr_nested_rule_.m__anglr_syntax_production_list_?.Iterate (null, (node, appInfo) =>
                        {
                            _anglr_syntax_production_ p__anglr_syntax_production_ = node.m__anglr_syntax_production_;
                            if (p__anglr_syntax_production_.kind != (uint) _anglr_syntax_production_.production_kind.g__anglr_syntax_production__1)
                                return null;
                            else
                            if (index++ != 0)
                                rule += "|";
                            rule += $"{name} {p__name_.Emit (-1)} {node.m__anglr_syntax_production_.Emit (-1)}";
                            return null;
                        });
                    rule += ";";
                }
            }
            else
                rule = $"{name}:{p__name_.Emit (-1)}|{name} {p__name_.Emit (-1)};";
            return CompileGeneratedRule (rule, true, new object [] { name, "Generated Rule: ", rule });
        }

        private _name_ NormalizeNWCOptionalSet (_name_ p__name_, _delimiter_ delimiter_, string dname)
        {
            string name = (m_simpleRuleName != null) ? m_simpleRuleName.text : (m_simpleNodeName != null) ? GenerateNodeName (m_simpleNodeName, new string [] { "optional", "set" }, dname) : $"<generated-rule-{m_generatedRuleIndex++}>";
            string rule;
            if (delimiter_ != null)
            {
                string setname = (m_simpleRuleName != null) ? m_simpleRuleName.text : (m_simpleNodeName != null) ? GenerateNodeName (m_simpleNodeName, new string [] { "set" }, dname) : $"<generated-rule-{m_generatedRuleIndex++}>";
                string setrule;
                _anglr_nested_rule_ p__anglr_nested_rule_ = delimiter_.m__anglr_nested_rule_;
                _anglr_syntax_production_list_name_optional_ p__anglr_syntax_production_list_name_optional_ = p__anglr_nested_rule_.m__anglr_syntax_production_list_name_optional_;
                _anglr_syntax_production_list_name_ p__anglr_syntax_production_list_name_ = p__anglr_syntax_production_list_name_optional_.m__anglr_syntax_production_list_name_;
                if (p__anglr_syntax_production_list_name_ != null)
                    name = p__anglr_syntax_production_list_name_.m__identifier_.text;
                rule = $"{name}:%empty|{setname};";
                setrule = $"{setname}:{p__name_.Emit (-1)}";
                p__anglr_nested_rule_.m__anglr_syntax_production_list_?.Iterate (null, (node, appData) =>
                    {
                        _anglr_syntax_production_ p__anglr_syntax_production_ = node.m__anglr_syntax_production_;
                        if (p__anglr_syntax_production_.kind != (uint) _anglr_syntax_production_.production_kind.g__anglr_syntax_production__1)
                            setrule += $"|{setname} {p__name_.Emit (-1)}";
                        else
                            setrule += $"|{setname} {node.m__anglr_syntax_production_.Emit (-1)} {p__name_.Emit (-1)}";
                        return null;
                    });
                setrule += ";";
                CompileGeneratedRule (setrule, true, new object [] { name, "Generated Rule: ", rule });
            }
            else
                rule = $"{name}:%empty|{name} {p__name_.Emit (-1)};";
            return CompileGeneratedRule (rule, false, new object [] { name, "Generated Rule: ", rule });
        }

        private _name_ NormalizeINWCOptionalSet (_name_ p__name_, _delimiter_ delimiter_, string dname)
        {
            string name = (m_simpleRuleName != null) ? m_simpleRuleName.text : (m_simpleNodeName != null) ? GenerateNodeName (m_simpleNodeName, new string [] { "optional", "set" }, dname) : $"<generated-rule-{m_generatedRuleIndex++}>";
            string rule;
            if (delimiter_ != null)
            {
                SyntaxTreeToken token = CheckSimpleRuleCondition (delimiter_.m__anglr_nested_rule_);
                string setname = (m_simpleRuleName != null) ? m_simpleRuleName.text : (m_simpleNodeName != null) ? GenerateNodeName (m_simpleNodeName, new string [] { "set" }, dname) : $"<generated-rule-{m_generatedRuleIndex++}>";
                string setrule;
                _anglr_nested_rule_ p__anglr_nested_rule_ = delimiter_.m__anglr_nested_rule_;
                _anglr_syntax_production_list_name_optional_ p__anglr_syntax_production_list_name_optional_ = p__anglr_nested_rule_.m__anglr_syntax_production_list_name_optional_;
                _anglr_syntax_production_list_name_ p__anglr_syntax_production_list_name_ = p__anglr_syntax_production_list_name_optional_.m__anglr_syntax_production_list_name_;
                if (p__anglr_syntax_production_list_name_ != null)
                    name = p__anglr_syntax_production_list_name_.m__identifier_.text;
                rule = $"{name}:%empty|{setname};";
                if (token != null)
                    setrule = $"{name}:{token.text}|{name} {p__name_.Emit (-1)} {token.text};";
                else
                {
                    setrule = $"{setname}:";
                    int index = 0;
                    p__anglr_nested_rule_.m__anglr_syntax_production_list_?.Iterate (null, (node, appData) =>
                    {
                        _anglr_syntax_production_ p__anglr_syntax_production_ = node.m__anglr_syntax_production_;
                        if (p__anglr_syntax_production_.kind != (uint) _anglr_syntax_production_.production_kind.g__anglr_syntax_production__1)
                            return null;
                        if (index++ != 0)
                            setrule += "|";
                        setrule += $"{node.m__anglr_syntax_production_.Emit (-1)}";
                        return null;
                    });
                    p__anglr_nested_rule_.m__anglr_syntax_production_list_?.Iterate (null, (node, appData) =>
                    {
                        _anglr_syntax_production_ p__anglr_syntax_production_ = node.m__anglr_syntax_production_;
                        if (p__anglr_syntax_production_.kind != (uint) _anglr_syntax_production_.production_kind.g__anglr_syntax_production__1)
                            return null;
                        else
                        if (index++ != 0)
                            setrule += "|";
                        setrule += $"{setname} {p__name_.Emit (-1)} {node.m__anglr_syntax_production_.Emit (-1)}";
                        return null;
                    });
                    setrule += ";";
                }
                CompileGeneratedRule (setrule, true, new object [] { name, "Generated Rule: ", rule });
            }
            else
                rule = $"{name}:%empty|{name} {p__name_.Emit (-1)};";
            return CompileGeneratedRule (rule, false, new object [] { name, "Generated Rule: ", rule });
        }

        private _name_ NormalizeLNWCSet (_name_ p__name_, _delimiter_ delimiter_, string dname)
        {
            string name = (m_simpleRuleName != null) ? m_simpleRuleName.text : (m_simpleNodeName != null) ? GenerateNodeName (m_simpleNodeName, new string [] { "set" }, dname) : $"<generated-rule-{m_generatedRuleIndex++}>";
            string rule;
            if (delimiter_ != null)
            {
                _anglr_nested_rule_ p__anglr_nested_rule_ = delimiter_.m__anglr_nested_rule_;
                _anglr_syntax_production_list_name_optional_ p__anglr_syntax_production_list_name_optional_ = p__anglr_nested_rule_.m__anglr_syntax_production_list_name_optional_;
                _anglr_syntax_production_list_name_ p__anglr_syntax_production_list_name_ = p__anglr_syntax_production_list_name_optional_.m__anglr_syntax_production_list_name_;
                if (p__anglr_syntax_production_list_name_ != null)
                    name = p__anglr_syntax_production_list_name_.m__identifier_.text;
                rule = $"{name}:{p__name_.Emit (-1)}";
                p__anglr_nested_rule_.m__anglr_syntax_production_list_?.Iterate (null, (node, appData) =>
                {
                    _anglr_syntax_production_ p__anglr_syntax_production_ = node.m__anglr_syntax_production_;
                    if (p__anglr_syntax_production_.kind != (uint) _anglr_syntax_production_.production_kind.g__anglr_syntax_production__1)
                        rule += $"|{p__name_.Emit (-1)} {name}";
                    else
                        rule += $"|{p__name_.Emit (-1)} {node.m__anglr_syntax_production_.Emit (-1)} {name}";
                    return null;
                });
                rule += ";";
            }
            else
                rule = $"{name}:{p__name_.Emit (-1)}|{p__name_.Emit (-1)} {name};";
            return CompileGeneratedRule (rule, true, new object [] { name, "Generated Rule: ", rule });
        }

        private _name_ NormalizeLINWCSet (_name_ p__name_, _delimiter_ delimiter_, string dname)
        {
            string name = (m_simpleRuleName != null) ? m_simpleRuleName.text : (m_simpleNodeName != null) ? GenerateNodeName (m_simpleNodeName, new string [] { "set" }, dname) : $"<generated-rule-{m_generatedRuleIndex++}>";
            string rule;
            if (delimiter_ != null)
            {
                _anglr_nested_rule_ p__anglr_nested_rule_ = delimiter_.m__anglr_nested_rule_;
                SyntaxTreeToken token = CheckSimpleRuleCondition (p__anglr_nested_rule_);
                if (token != null)
                    rule = $"{name}:{token.text}|{token.text} {p__name_.Emit (-1)} {name};";
                else
                {
                    _anglr_syntax_production_list_name_optional_ p__anglr_syntax_production_list_name_optional_ = p__anglr_nested_rule_.m__anglr_syntax_production_list_name_optional_;
                    _anglr_syntax_production_list_name_ p__anglr_syntax_production_list_name_ = p__anglr_syntax_production_list_name_optional_.m__anglr_syntax_production_list_name_;
                    if (p__anglr_syntax_production_list_name_ != null)
                        name = p__anglr_syntax_production_list_name_.m__identifier_.text;
                    rule = $"{name}:";
                    int index = 0;
                    p__anglr_nested_rule_.m__anglr_syntax_production_list_?.Iterate (null, (node, appData) =>
                    {
                        _anglr_syntax_production_ p__anglr_syntax_production_ = node.m__anglr_syntax_production_;
                        if (p__anglr_syntax_production_.kind != (uint) _anglr_syntax_production_.production_kind.g__anglr_syntax_production__1)
                            return null;
                        if (index++ != 0)
                            rule += "|";
                        rule += $"{node.m__anglr_syntax_production_.Emit (-1)}";
                        return null;
                    });
                    p__anglr_nested_rule_.m__anglr_syntax_production_list_?.Iterate (null, (node, appData) =>
                    {
                        _anglr_syntax_production_ p__anglr_syntax_production_ = node.m__anglr_syntax_production_;
                        if (p__anglr_syntax_production_.kind != (uint) _anglr_syntax_production_.production_kind.g__anglr_syntax_production__1)
                            return null;
                        else
                        if (index++ != 0)
                            rule += "|";
                        rule += $"{node.m__anglr_syntax_production_.Emit (-1)} {p__name_.Emit (-1)} {name}";
                        return null;
                    });
                    rule += ";";
                }
            }
            else
                rule = $"{name}:{p__name_.Emit (-1)}|{p__name_.Emit (-1)} {name};";
            return CompileGeneratedRule (rule, true, new object [] { name, "Generated Rule: ", rule });
        }

        private _name_ NormalizeLNWCOptionalSet (_name_ p__name_, _delimiter_ delimiter_, string dname)
        {
            string name = (m_simpleRuleName != null) ? m_simpleRuleName.text : (m_simpleNodeName != null) ? GenerateNodeName (m_simpleNodeName, new string [] { "optional", "set" }, dname) : $"<generated-rule-{m_generatedRuleIndex++}>";
            string rule;
            if (delimiter_ != null)
            {
                string setname = (m_simpleRuleName != null) ? m_simpleRuleName.text : (m_simpleNodeName != null) ? GenerateNodeName (m_simpleNodeName, new string [] { "set" }, dname) : $"<generated-rule-{m_generatedRuleIndex++}>";
                string setrule;
                _anglr_nested_rule_ p__anglr_nested_rule_ = delimiter_.m__anglr_nested_rule_;
                _anglr_syntax_production_list_name_optional_ p__anglr_syntax_production_list_name_optional_ = p__anglr_nested_rule_.m__anglr_syntax_production_list_name_optional_;
                _anglr_syntax_production_list_name_ p__anglr_syntax_production_list_name_ = p__anglr_syntax_production_list_name_optional_.m__anglr_syntax_production_list_name_;
                if (p__anglr_syntax_production_list_name_ != null)
                    name = p__anglr_syntax_production_list_name_.m__identifier_.text;
                rule = $"{name}:%empty|{setname};";
                setrule = $"{setname}:{p__name_.Emit (-1)}";
                p__anglr_nested_rule_.m__anglr_syntax_production_list_?.Iterate (null, (node, appData) =>
                {
                    _anglr_syntax_production_ p__anglr_syntax_production_ = node.m__anglr_syntax_production_;
                    if (p__anglr_syntax_production_.kind != (uint) _anglr_syntax_production_.production_kind.g__anglr_syntax_production__1)
                        setrule += $"|{p__name_.Emit (-1)} {setname}";
                    else
                        setrule += $"|{p__name_.Emit (-1)} {node.m__anglr_syntax_production_.Emit (-1)} {setname}";
                    return null;
                });
                setrule += ";";
                CompileGeneratedRule (setrule, true, new object [] { name, "Generated Rule: ", rule });
            }
            else
                rule = $"{name}:%empty|{p__name_.Emit (-1)} {name};";
            return CompileGeneratedRule (rule, false, new object [] { name, "Generated Rule: ", rule });
        }

        private _name_ NormalizeLINWCOptionalSet (_name_ p__name_, _delimiter_ delimiter_, string dname)
        {
            string name = (m_simpleRuleName != null) ? m_simpleRuleName.text : (m_simpleNodeName != null) ? GenerateNodeName (m_simpleNodeName, new string [] { "optional", "set" }, dname) : $"<generated-rule-{m_generatedRuleIndex++}>";
            string rule;
            if (delimiter_ != null)
            {
                SyntaxTreeToken token = CheckSimpleRuleCondition (delimiter_.m__anglr_nested_rule_);
                string setname = (m_simpleRuleName != null) ? m_simpleRuleName.text : (m_simpleNodeName != null) ? GenerateNodeName (m_simpleNodeName, new string [] { "set" }, dname) : $"<generated-rule-{m_generatedRuleIndex++}>";
                string setrule;
                _anglr_nested_rule_ p__anglr_nested_rule_ = delimiter_.m__anglr_nested_rule_;
                _anglr_syntax_production_list_name_optional_ p__anglr_syntax_production_list_name_optional_ = p__anglr_nested_rule_.m__anglr_syntax_production_list_name_optional_;
                _anglr_syntax_production_list_name_ p__anglr_syntax_production_list_name_ = p__anglr_syntax_production_list_name_optional_.m__anglr_syntax_production_list_name_;
                if (p__anglr_syntax_production_list_name_ != null)
                    name = p__anglr_syntax_production_list_name_.m__identifier_.text;
                rule = $"{name}:%empty|{setname};";
                if (token != null)
                    setrule = $"{name}:{token.text}|{token.text} {p__name_.Emit (-1)} {name};";
                else
                {
                    setrule = $"{setname}:";
                    int index = 0;
                    p__anglr_nested_rule_.m__anglr_syntax_production_list_?.Iterate (null, (node, appData) =>
                    {
                        _anglr_syntax_production_ p__anglr_syntax_production_ = node.m__anglr_syntax_production_;
                        if (p__anglr_syntax_production_.kind != (uint) _anglr_syntax_production_.production_kind.g__anglr_syntax_production__1)
                            return null;
                        if (index++ != 0)
                            setrule += "|";
                        setrule += $"{node.m__anglr_syntax_production_.Emit (-1)}";
                        return null;
                    });
                    p__anglr_nested_rule_.m__anglr_syntax_production_list_?.Iterate (null, (node, appData) =>
                    {
                        _anglr_syntax_production_ p__anglr_syntax_production_ = node.m__anglr_syntax_production_;
                        if (p__anglr_syntax_production_.kind != (uint) _anglr_syntax_production_.production_kind.g__anglr_syntax_production__1)
                            return null;
                        else
                        if (index++ != 0)
                            setrule += "|";
                        setrule += $"{node.m__anglr_syntax_production_.Emit (-1)} {p__name_.Emit (-1)} {setname}";
                        return null;
                    });
                    setrule += ";";
                }
                CompileGeneratedRule (setrule, true, new object [] { name, "Generated Rule: ", rule });
            }
            else
                rule = $"{name}:%empty|{p__name_.Emit (-1)} {name};";
            return CompileGeneratedRule (rule, false, new object [] { name, "Generated Rule: ", rule });
        }

        private _name_ NormalizeProdList (_g_name_ p__gname_)
        {
            string name = (m_simpleRuleName != null) ? m_simpleRuleName.text : $"<generated-rule-{m_generatedRuleIndex++}>";
            _anglr_nested_rule_ p__anglr_nested_rule_ = p__gname_.m__anglr_nested_rule_;
            _anglr_syntax_production_list_name_optional_ p__anglr_syntax_production_list_name_optional_ = p__anglr_nested_rule_.m__anglr_syntax_production_list_name_optional_;
            _anglr_syntax_production_list_name_ p__anglr_syntax_production_list_name_ = p__anglr_syntax_production_list_name_optional_.m__anglr_syntax_production_list_name_;
            if (p__anglr_syntax_production_list_name_ != null)
                name = p__anglr_syntax_production_list_name_.m__identifier_.text;
            string rule = $"{name}:";
            _anglr_syntax_production_list_ p__anglr_syntax_production_list_ = p__anglr_nested_rule_.m__anglr_syntax_production_list_;
            if (p__anglr_syntax_production_list_ != null)
            {
                rule += p__anglr_syntax_production_list_.Emit (-1);
            }
            else
                rule += "%empty";
            rule += ";";
            return CompileGeneratedRule (rule, false, new object [] { name, "Generated Rule: ", rule });
        }

        private _name_ CompileGeneratedRule (string rule, bool iterator, object [] info = null)
        {
            AnglrLogger?.InfoRawLine ($"COMPILE RULE: {rule}");
            anglrCompiler compiler = new anglrCompiler ();
            int res = compiler.ParseString (rule, AnglrDeclarations.tokens._anglr_syntax_rule_terminal_, info);
            if (res != 0)
            {
                AnglrLogger?.ErrorLine ($"error in generated rule '{rule}'");
                return null;
            }
            _name_ name = null;
            foreach (SyntaxTreeBase rootNode in compiler.parseList)
            {
                _anglr_syntax_rule_ syntax_Rule_ = ((_anglr_file_fragment_) rootNode).m__anglr_syntax_rule_;
                (bool condition, string ruleName, string idName) result = CheckIdentityCondition (syntax_Rule_);
                if (result.condition)
                {
                    foreach (_anglr_syntax_rule_ syntax_Rule in m_generatedRuleList)
                    {
                        if (syntax_Rule.m__identifier_.text == result.idName)
                            syntax_Rule.m__identifier_.text = result.ruleName;
                        syntax_Rule.m__anglr_syntax_production_list_.Iterate (null, (node, appData) =>
                        {
                            node.m__anglr_syntax_production_.m__name_list_?.Iterate (null, (nameList, nameData) =>
                            {
                                if ((_name_list_.production_kind) nameList.kind != _name_list_.production_kind.g__name_list__2)
                                    return null;
                                _g_name_ gname = nameList.m__g_name_;
                                if ((_g_name_.production_kind) gname.kind != _g_name_.production_kind.g__g_name__1)
                                    return null;
                                _name_ name_ = gname.m__name_;
                                if ((_name_.production_kind) name_.kind != _name_.production_kind.g__name__3)
                                    return null;
                                if (name_.m__identifier_.text == result.idName)
                                    name_.m__identifier_.text = result.ruleName;
                                return null;
                            });
                            return null;
                        });
                    }
                }
                else
                {
                    m_generatedRuleList.Add (syntax_Rule_);
                }
                SymbolToken symbolToken = SymbolTable.insert (new SymbolToken (syntax_Rule_.m__identifier_.text, (uint) AnglrClassificationType.NonTerminalName, null, parserPartSymbol));
                if (iterator && anglrCompiler.createIterators)
                    symbolToken.IteratorFlag = true;
                name = new _name_ (new SyntaxTreeToken ((int) AnglrDeclarations.tokens._identifier_, -1, -1, syntax_Rule_.m__identifier_.text), (int) _name_.production_kind.g__name__3);
                TraverseCommon (name);
                Traverse (name);
            }
            return name;
        }

        private SyntaxTreeToken CheckSimpleRuleCondition (_g_name_ g_Name_)
        {
            SyntaxTreeBase node = g_Name_.parent;
            if (!(node is _name_list_))
                return null;
            if (((_name_list_) node).m__name_list_.m__name_list_ != null)
                return null;
            node = node.parent;
            if (!(node is _anglr_syntax_production_))
                return null;
            if (false)
            {
                node = node.parent;
                if (!(node is _anglr_syntax_production_list_))
                    return null;
                if ((_anglr_syntax_production_list_) node != ((_anglr_syntax_production_list_) node).m__anglr_syntax_production_list_)
                    return null;
                node = node.parent;
                if (node is _anglr_syntax_rule_)
                    return ((_anglr_syntax_rule_) node).m__identifier_;
            }
            for (node = node.parent; node is _anglr_syntax_production_list_; node = node.parent)
                ;
            if (node is _anglr_syntax_rule_)
                return ((_anglr_syntax_rule_) node).m__identifier_;
            return null;
        }

        private SyntaxTreeToken CheckSimpleRuleCondition (_anglr_nested_rule_ nested_Rule_)
        {
            if (nested_Rule_.m__anglr_syntax_production_list_name_optional_.m__anglr_syntax_production_list_name_ != null)
                return nested_Rule_.m__anglr_syntax_production_list_name_optional_.m__anglr_syntax_production_list_name_.m__identifier_;

            _anglr_syntax_production_list_ syntax_Production_List_ = nested_Rule_.m__anglr_syntax_production_list_;
            if (syntax_Production_List_.m__anglr_syntax_production_list_ != null)
                return null;
            _anglr_syntax_production_ syntax_Production_ = syntax_Production_List_.m__anglr_syntax_production_;
            if (syntax_Production_.kind != (uint) _anglr_syntax_production_.production_kind.g__anglr_syntax_production__1)
                return null;
            _g_name_ g_Name_ = null;
            for (_name_list_ name_List_ = syntax_Production_.m__name_list_; name_List_ != null; name_List_ = name_List_.m__name_list_)
            {
                if (name_List_.m__marker_list_optional_.m__marker_list_ != null)
                    return null;
                if (name_List_.m__g_name_ != null)
                {
                    if (g_Name_ != null)
                        return null;
                    g_Name_ = name_List_.m__g_name_;
                }
            }
            if (g_Name_ == null)
                return null;
            if (g_Name_.kind != (uint) _g_name_.production_kind.g__g_name__1)
                return null;
            _name_ name_ = g_Name_.m__name_;
            if (name_.kind != (uint) _name_.production_kind.g__name__3)
                return null;
            return name_.m__identifier_;
        }

        private SyntaxTreeToken CheckSimpleNodeCondition (_g_name_ g_Name_)
        {
            if (g_Name_.kind != (uint) _g_name_.production_kind.g__g_name__3)
                return null;
            g_Name_ = g_Name_.m__g_name_;
            if (g_Name_.kind != (uint) _g_name_.production_kind.g__g_name__1)
                return null;
            _name_ p__name_ = g_Name_.m__name_;
            if (p__name_.kind != (uint) _name_.production_kind.g__name__3)
                return null;
            return p__name_.m__identifier_;
        }

        private string GenerateNodeName (SyntaxTreeToken node, string [] suffix, string dname)
        {
            if (dname != null)
                return dname;

            string text = node.text;
            int index = text.LastIndexOf ('>');
            if (index < 0)
            {
                foreach (string str in suffix)
                    text += '-' + str;
            }
            else
            {
                foreach (string str in suffix)
                {
                    text = text.Insert (index, ' ' + str);
                    index += str.Length + 1;
                }
            }
            return text;
        }

        private (bool, string, string) CheckIdentityCondition (_anglr_syntax_rule_ syntax_Rule_)
        {
            _anglr_syntax_production_list_ syntax_Production_List_ = syntax_Rule_.m__anglr_syntax_production_list_;
            if (syntax_Production_List_.m__anglr_syntax_production_list_ != null)
                return (false, null, null);
            _anglr_syntax_production_ syntax_Production_ = syntax_Production_List_.m__anglr_syntax_production_;
            _name_list_ name_List_ = syntax_Production_.m__name_list_;
            if (name_List_ == null)
                return (false, null, null);
            if ((name_List_.m__name_list_ != null) && ((_name_list_.production_kind) name_List_.m__name_list_.kind != _name_list_.production_kind.g__name_list__1))
                return (false, null, null);
            return (false, syntax_Rule_.m__identifier_.text, name_List_.m__g_name_.m__name_.Emit (-1));
        }

        private int m_generatedRuleIndex = 1;
        private bool m_discardProducton = false;
        private SyntaxTreeToken m_simpleRuleName = null;
        private SyntaxTreeToken m_simpleNodeName = null;
        private List<_anglr_syntax_rule_> m_generatedRuleList = new List<_anglr_syntax_rule_> ();
    }

    public partial class AnglrParserStatesBaseGenerator : SyntaxTreeWalker
    {
        public void InitGeneralizations ()
        {
            m_generatedRuleIndex = 1;
            m_discardProducton = false;
            m_simpleRuleName = null;
            m_simpleNodeName = null;
            m_generatedRuleList = new List<_anglr_syntax_rule_> ();
        }

        private void NormalizeCompoundName (_g_name_ p__g_name_)
        {
            if (p__g_name_.kind == (uint) _g_name_.production_kind.g__g_name__1)
                return;

            m_discardProducton = ((m_simpleRuleName = CheckSimpleRuleCondition (p__g_name_)) != null);
            m_simpleNodeName = CheckSimpleNodeCondition (p__g_name_);

            _name_ name_ = null;
            switch (p__g_name_.kind)
            {
                case (uint) _g_name_.production_kind.g__g_name__2:
                    name_ = NormalizeProdList (p__g_name_);
                    break;
                case (uint) _g_name_.production_kind.g__g_name__3:
                    name_ = NormalizeGNameWithCardinality (p__g_name_.m__g_name_, p__g_name_.m__cardinality_delimiter_);
                    break;
            }
            m_simpleRuleName = null;
            m_simpleNodeName = null;
            if (name_ == null)
                return;

            p__g_name_.change (name_);
        }

        private _name_ NormalizeGNameWithCardinality (_g_name_ p__g_name_, _cardinality_delimiter_ p__cardinality_delimiter_)
        {
            if (p__g_name_.kind != (uint) _g_name_.production_kind.g__g_name__1)
                return null;

            _name_ p__name_ = p__g_name_.m__name_;
            _name_ name_ = null;
            _cardinality_ cardinality_ = p__cardinality_delimiter_.m__cardinality_;
            _delimiter_ delimiter_ = p__cardinality_delimiter_.m__delimiter_optional_.m__delimiter_;

            switch ((_cardinality_.production_kind) cardinality_.kind)
            {
                case _cardinality_.production_kind.g__cardinality__1:
                    name_ = NormalizeNWCOption (p__name_, null);
                    break;
                case _cardinality_.production_kind.g__cardinality__2:
                    name_ = NormalizeNWCSet (p__name_, delimiter_, null);
                    break;
                case _cardinality_.production_kind.g__cardinality__3:
                    name_ = NormalizeINWCSet (p__name_, delimiter_, null);
                    break;
                case _cardinality_.production_kind.g__cardinality__4:
                    name_ = NormalizeNWCOptionalSet (p__name_, delimiter_, null);
                    break;
                case _cardinality_.production_kind.g__cardinality__5:
                    name_ = NormalizeINWCOptionalSet (p__name_, delimiter_, null);
                    break;
                case _cardinality_.production_kind.g__cardinality__6:
                    name_ = NormalizeLNWCSet (p__name_, delimiter_, null);
                    break;
                case _cardinality_.production_kind.g__cardinality__7:
                    name_ = NormalizeLINWCSet (p__name_, delimiter_, null);
                    break;
                case _cardinality_.production_kind.g__cardinality__8:
                    name_ = NormalizeLNWCOptionalSet (p__name_, delimiter_, null);
                    break;
                case _cardinality_.production_kind.g__cardinality__9:
                    name_ = NormalizeLINWCOptionalSet (p__name_, delimiter_, null);
                    break;
                case _cardinality_.production_kind.g__cardinality__10:
                    _number_optional_ minnr = cardinality_.m__number_optional_;
                    _number_optional_ maxnr = cardinality_.m__number_optional__1;
                    if (maxnr.m__number_ == null)
                    {
                        if (minnr.m__number_ == null)
                        {
                            name_ = NormalizeNWCOptionalSet (p__name_, delimiter_, null);
                        }
                        else
                        {
                            int number = int.Parse (minnr.m__number_.text);
                            if (number > 0)
                                name_ = NormalizeNWCSet (p__name_, delimiter_, null);
                            else
                                name_ = NormalizeNWCOptionalSet (p__name_, delimiter_, null);
                        }
                    }
                    else
                    {
                        if (minnr.m__number_ == null)
                        {
                            int number = int.Parse (maxnr.m__number_.text);
                            if (number > 0)
                                name_ = NormalizeNWCSet (p__name_, delimiter_, null);
                        }
                        else
                        {
                            int lband = int.Parse (minnr.m__number_.text);
                            int uband = int.Parse (maxnr.m__number_.text);
                            if (lband > uband)
                                ;
                            else if (lband > 0)
                            {
                                if (lband == uband)
                                    name_ = (_name_) p__name_.Clone ();
                                else
                                    name_ = NormalizeNWCSet (p__name_, delimiter_, null);
                            }
                            else
                            {
                                if (lband == uband)
                                    ;
                                else
                                    name_ = NormalizeNWCOptionalSet (p__name_, delimiter_, null);
                            }
                        }
                    }
                    break;
            }
            return name_;
        }

        private _name_ NormalizeNWCOption (_name_ p__name_, string dname)
        {
            string name = (m_simpleRuleName != null) ? m_simpleRuleName.text : (m_simpleNodeName != null) ? GenerateNodeName (m_simpleNodeName, new string [] { "optional" }, dname) : $"<generated-rule-{m_generatedRuleIndex++}>";
            string rule = $"{name}:%empty|{p__name_.Emit (-1)};";
            return CompileGeneratedRule (rule, false, new object [] { name, "Generated Rule: ", rule });
        }

        private _name_ NormalizeNWCSet (_name_ p__name_, _delimiter_ delimiter_, string dname)
        {
            string name = (m_simpleRuleName != null) ? m_simpleRuleName.text : (m_simpleNodeName != null) ? GenerateNodeName (m_simpleNodeName, new string [] { "set" }, dname) : $"<generated-rule-{m_generatedRuleIndex++}>";
            string rule;
            if (delimiter_ != null)
            {
                _anglr_nested_rule_ p__anglr_nested_rule_ = delimiter_.m__anglr_nested_rule_;
                _anglr_syntax_production_list_name_optional_ p__anglr_syntax_production_list_name_optional_ = p__anglr_nested_rule_.m__anglr_syntax_production_list_name_optional_;
                _anglr_syntax_production_list_name_ p__anglr_syntax_production_list_name_ = p__anglr_syntax_production_list_name_optional_.m__anglr_syntax_production_list_name_;
                if (p__anglr_syntax_production_list_name_ != null)
                    name = p__anglr_syntax_production_list_name_.m__identifier_.text;
                rule = $"{name}:{p__name_.Emit (-1)}";
                p__anglr_nested_rule_.m__anglr_syntax_production_list_?.Iterate (rule, (node, appData) =>
                {
                    _anglr_syntax_production_ p__anglr_syntax_production_ = node.m__anglr_syntax_production_;
                    if (p__anglr_syntax_production_.kind != (uint) _anglr_syntax_production_.production_kind.g__anglr_syntax_production__1)
                        rule += $"|{name} {p__name_.Emit (-1)}";
                    else
                        rule += $"|{name} {node.m__anglr_syntax_production_.Emit (-1)} {p__name_.Emit (-1)}";
                    return null;
                });
                rule += ";";
            }
            else
                rule = $"{name}:{p__name_.Emit (-1)}|{name} {p__name_.Emit (-1)};";
            return CompileGeneratedRule (rule, true, new object [] { name, "Generated Rule: ", rule });
        }

        private _name_ NormalizeINWCSet (_name_ p__name_, _delimiter_ delimiter_, string dname)
        {
            string name = (m_simpleRuleName != null) ? m_simpleRuleName.text : (m_simpleNodeName != null) ? GenerateNodeName (m_simpleNodeName, new string [] { "set" }, dname) : $"<generated-rule-{m_generatedRuleIndex++}>";
            string rule;
            if (delimiter_ != null)
            {
                _anglr_nested_rule_ p__anglr_nested_rule_ = delimiter_.m__anglr_nested_rule_;
                SyntaxTreeToken token = CheckSimpleRuleCondition (p__anglr_nested_rule_);
                if (token != null)
                    rule = $"{name}:{token.text}|{name} {p__name_.Emit (-1)} {token.text};";
                else
                {
                    _anglr_syntax_production_list_name_optional_ p__anglr_syntax_production_list_name_optional_ = p__anglr_nested_rule_.m__anglr_syntax_production_list_name_optional_;
                    _anglr_syntax_production_list_name_ p__anglr_syntax_production_list_name_ = p__anglr_syntax_production_list_name_optional_.m__anglr_syntax_production_list_name_;
                    if (p__anglr_syntax_production_list_name_ != null)
                        name = p__anglr_syntax_production_list_name_.m__identifier_.text;
                    rule = $"{name}:";
                    int index = 0;
                    p__anglr_nested_rule_.m__anglr_syntax_production_list_?.Iterate (null, (node, appInfo) =>
                    {
                        _anglr_syntax_production_ p__anglr_syntax_production_ = node.m__anglr_syntax_production_;
                        if (p__anglr_syntax_production_.kind != (uint) _anglr_syntax_production_.production_kind.g__anglr_syntax_production__1)
                            return null;
                        if (index++ != 0)
                            rule += "|";
                        rule += $"{node.m__anglr_syntax_production_.Emit (-1)}";
                        return null;
                    });
                    p__anglr_nested_rule_.m__anglr_syntax_production_list_?.Iterate (null, (node, appInfo) =>
                    {
                        _anglr_syntax_production_ p__anglr_syntax_production_ = node.m__anglr_syntax_production_;
                        if (p__anglr_syntax_production_.kind != (uint) _anglr_syntax_production_.production_kind.g__anglr_syntax_production__1)
                            return null;
                        else
                        if (index++ != 0)
                            rule += "|";
                        rule += $"{name} {p__name_.Emit (-1)} {node.m__anglr_syntax_production_.Emit (-1)}";
                        return null;
                    });
                    rule += ";";
                }
            }
            else
                rule = $"{name}:{p__name_.Emit (-1)}|{name} {p__name_.Emit (-1)};";
            return CompileGeneratedRule (rule, true, new object [] { name, "Generated Rule: ", rule });
        }

        private _name_ NormalizeNWCOptionalSet (_name_ p__name_, _delimiter_ delimiter_, string dname)
        {
            string name = (m_simpleRuleName != null) ? m_simpleRuleName.text : (m_simpleNodeName != null) ? GenerateNodeName (m_simpleNodeName, new string [] { "optional", "set" }, dname) : $"<generated-rule-{m_generatedRuleIndex++}>";
            string rule;
            if (delimiter_ != null)
            {
                string setname = (m_simpleRuleName != null) ? m_simpleRuleName.text : (m_simpleNodeName != null) ? GenerateNodeName (m_simpleNodeName, new string [] { "set" }, dname) : $"<generated-rule-{m_generatedRuleIndex++}>";
                string setrule;
                _anglr_nested_rule_ p__anglr_nested_rule_ = delimiter_.m__anglr_nested_rule_;
                _anglr_syntax_production_list_name_optional_ p__anglr_syntax_production_list_name_optional_ = p__anglr_nested_rule_.m__anglr_syntax_production_list_name_optional_;
                _anglr_syntax_production_list_name_ p__anglr_syntax_production_list_name_ = p__anglr_syntax_production_list_name_optional_.m__anglr_syntax_production_list_name_;
                if (p__anglr_syntax_production_list_name_ != null)
                    name = p__anglr_syntax_production_list_name_.m__identifier_.text;
                rule = $"{name}:%empty|{setname};";
                setrule = $"{setname}:{p__name_.Emit (-1)}";
                p__anglr_nested_rule_.m__anglr_syntax_production_list_?.Iterate (null, (node, appData) =>
                {
                    _anglr_syntax_production_ p__anglr_syntax_production_ = node.m__anglr_syntax_production_;
                    if (p__anglr_syntax_production_.kind != (uint) _anglr_syntax_production_.production_kind.g__anglr_syntax_production__1)
                        setrule += $"|{setname} {p__name_.Emit (-1)}";
                    else
                        setrule += $"|{setname} {node.m__anglr_syntax_production_.Emit (-1)} {p__name_.Emit (-1)}";
                    return null;
                });
                setrule += ";";
                CompileGeneratedRule (setrule, true, new object [] { name, "Generated Rule: ", rule });
            }
            else
                rule = $"{name}:%empty|{name} {p__name_.Emit (-1)};";
            return CompileGeneratedRule (rule, false, new object [] { name, "Generated Rule: ", rule });
        }

        private _name_ NormalizeINWCOptionalSet (_name_ p__name_, _delimiter_ delimiter_, string dname)
        {
            string name = (m_simpleRuleName != null) ? m_simpleRuleName.text : (m_simpleNodeName != null) ? GenerateNodeName (m_simpleNodeName, new string [] { "optional", "set" }, dname) : $"<generated-rule-{m_generatedRuleIndex++}>";
            string rule;
            if (delimiter_ != null)
            {
                SyntaxTreeToken token = CheckSimpleRuleCondition (delimiter_.m__anglr_nested_rule_);
                string setname = (m_simpleRuleName != null) ? m_simpleRuleName.text : (m_simpleNodeName != null) ? GenerateNodeName (m_simpleNodeName, new string [] { "set" }, dname) : $"<generated-rule-{m_generatedRuleIndex++}>";
                string setrule;
                _anglr_nested_rule_ p__anglr_nested_rule_ = delimiter_.m__anglr_nested_rule_;
                _anglr_syntax_production_list_name_optional_ p__anglr_syntax_production_list_name_optional_ = p__anglr_nested_rule_.m__anglr_syntax_production_list_name_optional_;
                _anglr_syntax_production_list_name_ p__anglr_syntax_production_list_name_ = p__anglr_syntax_production_list_name_optional_.m__anglr_syntax_production_list_name_;
                if (p__anglr_syntax_production_list_name_ != null)
                    name = p__anglr_syntax_production_list_name_.m__identifier_.text;
                rule = $"{name}:%empty|{setname};";
                if (token != null)
                    setrule = $"{name}:{token.text}|{name} {p__name_.Emit (-1)} {token.text};";
                else
                {
                    setrule = $"{setname}:";
                    int index = 0;
                    p__anglr_nested_rule_.m__anglr_syntax_production_list_?.Iterate (null, (node, appData) =>
                    {
                        _anglr_syntax_production_ p__anglr_syntax_production_ = node.m__anglr_syntax_production_;
                        if (p__anglr_syntax_production_.kind != (uint) _anglr_syntax_production_.production_kind.g__anglr_syntax_production__1)
                            return null;
                        if (index++ != 0)
                            setrule += "|";
                        setrule += $"{node.m__anglr_syntax_production_.Emit (-1)}";
                        return null;
                    });
                    p__anglr_nested_rule_.m__anglr_syntax_production_list_?.Iterate (null, (node, appData) =>
                    {
                        _anglr_syntax_production_ p__anglr_syntax_production_ = node.m__anglr_syntax_production_;
                        if (p__anglr_syntax_production_.kind != (uint) _anglr_syntax_production_.production_kind.g__anglr_syntax_production__1)
                            return null;
                        else
                        if (index++ != 0)
                            setrule += "|";
                        setrule += $"{setname} {p__name_.Emit (-1)} {node.m__anglr_syntax_production_.Emit (-1)}";
                        return null;
                    });
                    setrule += ";";
                }
                CompileGeneratedRule (setrule, true, new object [] { name, "Generated Rule: ", rule });
            }
            else
                rule = $"{name}:%empty|{name} {p__name_.Emit (-1)};";
            return CompileGeneratedRule (rule, false, new object [] { name, "Generated Rule: ", rule });
        }

        private _name_ NormalizeLNWCSet (_name_ p__name_, _delimiter_ delimiter_, string dname)
        {
            string name = (m_simpleRuleName != null) ? m_simpleRuleName.text : (m_simpleNodeName != null) ? GenerateNodeName (m_simpleNodeName, new string [] { "set" }, dname) : $"<generated-rule-{m_generatedRuleIndex++}>";
            string rule;
            if (delimiter_ != null)
            {
                _anglr_nested_rule_ p__anglr_nested_rule_ = delimiter_.m__anglr_nested_rule_;
                _anglr_syntax_production_list_name_optional_ p__anglr_syntax_production_list_name_optional_ = p__anglr_nested_rule_.m__anglr_syntax_production_list_name_optional_;
                _anglr_syntax_production_list_name_ p__anglr_syntax_production_list_name_ = p__anglr_syntax_production_list_name_optional_.m__anglr_syntax_production_list_name_;
                if (p__anglr_syntax_production_list_name_ != null)
                    name = p__anglr_syntax_production_list_name_.m__identifier_.text;
                rule = $"{name}:{p__name_.Emit (-1)}";
                p__anglr_nested_rule_.m__anglr_syntax_production_list_?.Iterate (null, (node, appData) =>
                {
                    _anglr_syntax_production_ p__anglr_syntax_production_ = node.m__anglr_syntax_production_;
                    if (p__anglr_syntax_production_.kind != (uint) _anglr_syntax_production_.production_kind.g__anglr_syntax_production__1)
                        rule += $"|{p__name_.Emit (-1)} {name}";
                    else
                        rule += $"|{p__name_.Emit (-1)} {node.m__anglr_syntax_production_.Emit (-1)} {name}";
                    return null;
                });
                rule += ";";
            }
            else
                rule = $"{name}:{p__name_.Emit (-1)}|{p__name_.Emit (-1)} {name};";
            return CompileGeneratedRule (rule, true, new object [] { name, "Generated Rule: ", rule });
        }

        private _name_ NormalizeLINWCSet (_name_ p__name_, _delimiter_ delimiter_, string dname)
        {
            string name = (m_simpleRuleName != null) ? m_simpleRuleName.text : (m_simpleNodeName != null) ? GenerateNodeName (m_simpleNodeName, new string [] { "set" }, dname) : $"<generated-rule-{m_generatedRuleIndex++}>";
            string rule;
            if (delimiter_ != null)
            {
                _anglr_nested_rule_ p__anglr_nested_rule_ = delimiter_.m__anglr_nested_rule_;
                SyntaxTreeToken token = CheckSimpleRuleCondition (p__anglr_nested_rule_);
                if (token != null)
                    rule = $"{name}:{token.text}|{token.text} {p__name_.Emit (-1)} {name};";
                else
                {
                    _anglr_syntax_production_list_name_optional_ p__anglr_syntax_production_list_name_optional_ = p__anglr_nested_rule_.m__anglr_syntax_production_list_name_optional_;
                    _anglr_syntax_production_list_name_ p__anglr_syntax_production_list_name_ = p__anglr_syntax_production_list_name_optional_.m__anglr_syntax_production_list_name_;
                    if (p__anglr_syntax_production_list_name_ != null)
                        name = p__anglr_syntax_production_list_name_.m__identifier_.text;
                    rule = $"{name}:";
                    int index = 0;
                    p__anglr_nested_rule_.m__anglr_syntax_production_list_?.Iterate (null, (node, appData) =>
                    {
                        _anglr_syntax_production_ p__anglr_syntax_production_ = node.m__anglr_syntax_production_;
                        if (p__anglr_syntax_production_.kind != (uint) _anglr_syntax_production_.production_kind.g__anglr_syntax_production__1)
                            return null;
                        if (index++ != 0)
                            rule += "|";
                        rule += $"{node.m__anglr_syntax_production_.Emit (-1)}";
                        return null;
                    });
                    p__anglr_nested_rule_.m__anglr_syntax_production_list_?.Iterate (null, (node, appData) =>
                    {
                        _anglr_syntax_production_ p__anglr_syntax_production_ = node.m__anglr_syntax_production_;
                        if (p__anglr_syntax_production_.kind != (uint) _anglr_syntax_production_.production_kind.g__anglr_syntax_production__1)
                            return null;
                        else
                        if (index++ != 0)
                            rule += "|";
                        rule += $"{node.m__anglr_syntax_production_.Emit (-1)} {p__name_.Emit (-1)} {name}";
                        return null;
                    });
                    rule += ";";
                }
            }
            else
                rule = $"{name}:{p__name_.Emit (-1)}|{p__name_.Emit (-1)} {name};";
            return CompileGeneratedRule (rule, true, new object [] { name, "Generated Rule: ", rule });
        }

        private _name_ NormalizeLNWCOptionalSet (_name_ p__name_, _delimiter_ delimiter_, string dname)
        {
            string name = (m_simpleRuleName != null) ? m_simpleRuleName.text : (m_simpleNodeName != null) ? GenerateNodeName (m_simpleNodeName, new string [] { "optional", "set" }, dname) : $"<generated-rule-{m_generatedRuleIndex++}>";
            string rule;
            if (delimiter_ != null)
            {
                string setname = (m_simpleRuleName != null) ? m_simpleRuleName.text : (m_simpleNodeName != null) ? GenerateNodeName (m_simpleNodeName, new string [] { "set" }, dname) : $"<generated-rule-{m_generatedRuleIndex++}>";
                string setrule;
                _anglr_nested_rule_ p__anglr_nested_rule_ = delimiter_.m__anglr_nested_rule_;
                _anglr_syntax_production_list_name_optional_ p__anglr_syntax_production_list_name_optional_ = p__anglr_nested_rule_.m__anglr_syntax_production_list_name_optional_;
                _anglr_syntax_production_list_name_ p__anglr_syntax_production_list_name_ = p__anglr_syntax_production_list_name_optional_.m__anglr_syntax_production_list_name_;
                if (p__anglr_syntax_production_list_name_ != null)
                    name = p__anglr_syntax_production_list_name_.m__identifier_.text;
                rule = $"{name}:%empty|{setname};";
                setrule = $"{setname}:{p__name_.Emit (-1)}";
                p__anglr_nested_rule_.m__anglr_syntax_production_list_?.Iterate (null, (node, appData) =>
                {
                    _anglr_syntax_production_ p__anglr_syntax_production_ = node.m__anglr_syntax_production_;
                    if (p__anglr_syntax_production_.kind != (uint) _anglr_syntax_production_.production_kind.g__anglr_syntax_production__1)
                        setrule += $"|{p__name_.Emit (-1)} {setname}";
                    else
                        setrule += $"|{p__name_.Emit (-1)} {node.m__anglr_syntax_production_.Emit (-1)} {setname}";
                    return null;
                });
                setrule += ";";
                CompileGeneratedRule (setrule, true, new object [] { name, "Generated Rule: ", rule });
            }
            else
                rule = $"{name}:%empty|{p__name_.Emit (-1)} {name};";
            return CompileGeneratedRule (rule, false, new object [] { name, "Generated Rule: ", rule });
        }

        private _name_ NormalizeLINWCOptionalSet (_name_ p__name_, _delimiter_ delimiter_, string dname)
        {
            string name = (m_simpleRuleName != null) ? m_simpleRuleName.text : (m_simpleNodeName != null) ? GenerateNodeName (m_simpleNodeName, new string [] { "optional", "set" }, dname) : $"<generated-rule-{m_generatedRuleIndex++}>";
            string rule;
            if (delimiter_ != null)
            {
                SyntaxTreeToken token = CheckSimpleRuleCondition (delimiter_.m__anglr_nested_rule_);
                string setname = (m_simpleRuleName != null) ? m_simpleRuleName.text : (m_simpleNodeName != null) ? GenerateNodeName (m_simpleNodeName, new string [] { "set" }, dname) : $"<generated-rule-{m_generatedRuleIndex++}>";
                string setrule;
                _anglr_nested_rule_ p__anglr_nested_rule_ = delimiter_.m__anglr_nested_rule_;
                _anglr_syntax_production_list_name_optional_ p__anglr_syntax_production_list_name_optional_ = p__anglr_nested_rule_.m__anglr_syntax_production_list_name_optional_;
                _anglr_syntax_production_list_name_ p__anglr_syntax_production_list_name_ = p__anglr_syntax_production_list_name_optional_.m__anglr_syntax_production_list_name_;
                if (p__anglr_syntax_production_list_name_ != null)
                    name = p__anglr_syntax_production_list_name_.m__identifier_.text;
                rule = $"{name}:%empty|{setname};";
                if (token != null)
                    setrule = $"{name}:{token.text}|{token.text} {p__name_.Emit (-1)} {name};";
                else
                {
                    setrule = $"{setname}:";
                    int index = 0;
                    p__anglr_nested_rule_.m__anglr_syntax_production_list_?.Iterate (null, (node, appData) =>
                    {
                        _anglr_syntax_production_ p__anglr_syntax_production_ = node.m__anglr_syntax_production_;
                        if (p__anglr_syntax_production_.kind != (uint) _anglr_syntax_production_.production_kind.g__anglr_syntax_production__1)
                            return null;
                        if (index++ != 0)
                            setrule += "|";
                        setrule += $"{node.m__anglr_syntax_production_.Emit (-1)}";
                        return null;
                    });
                    p__anglr_nested_rule_.m__anglr_syntax_production_list_?.Iterate (null, (node, appData) =>
                    {
                        _anglr_syntax_production_ p__anglr_syntax_production_ = node.m__anglr_syntax_production_;
                        if (p__anglr_syntax_production_.kind != (uint) _anglr_syntax_production_.production_kind.g__anglr_syntax_production__1)
                            return null;
                        else
                        if (index++ != 0)
                            setrule += "|";
                        setrule += $"{node.m__anglr_syntax_production_.Emit (-1)} {p__name_.Emit (-1)} {setname}";
                        return null;
                    });
                    setrule += ";";
                }
                CompileGeneratedRule (setrule, true, new object [] { name, "Generated Rule: ", rule });
            }
            else
                rule = $"{name}:%empty|{p__name_.Emit (-1)} {name};";
            return CompileGeneratedRule (rule, false, new object [] { name, "Generated Rule: ", rule });
        }

        private _name_ NormalizeProdList (_g_name_ p__gname_)
        {
            string name = (m_simpleRuleName != null) ? m_simpleRuleName.text : $"<generated-rule-{m_generatedRuleIndex++}>";
            _anglr_nested_rule_ p__anglr_nested_rule_ = p__gname_.m__anglr_nested_rule_;
            _anglr_syntax_production_list_name_optional_ p__anglr_syntax_production_list_name_optional_ = p__anglr_nested_rule_.m__anglr_syntax_production_list_name_optional_;
            _anglr_syntax_production_list_name_ p__anglr_syntax_production_list_name_ = p__anglr_syntax_production_list_name_optional_.m__anglr_syntax_production_list_name_;
            if (p__anglr_syntax_production_list_name_ != null)
                name = p__anglr_syntax_production_list_name_.m__identifier_.text;
            string rule = $"{name}:";
            _anglr_syntax_production_list_ p__anglr_syntax_production_list_ = p__anglr_nested_rule_.m__anglr_syntax_production_list_;
            if (p__anglr_syntax_production_list_ != null)
            {
                rule += p__anglr_syntax_production_list_.Emit (-1);
            }
            else
                rule += "%empty";
            rule += ";";
            return CompileGeneratedRule (rule, false, new object [] { name, "Generated Rule: ", rule });
        }

        private _name_ CompileGeneratedRule (string rule, bool iterator, object [] info = null)
        {
            anglrCompiler compiler = new anglrCompiler ();
            int res = compiler.ParseString (rule, AnglrDeclarations.tokens._anglr_syntax_rule_terminal_, info);
            if (res != 0)
                return null;
            _name_ name = null;
            foreach (SyntaxTreeBase rootNode in compiler.parseList)
            {
                _anglr_syntax_rule_ syntax_Rule_ = ((_anglr_file_fragment_) rootNode).m__anglr_syntax_rule_;
                (bool condition, string ruleName, string idName) result = CheckIdentityCondition (syntax_Rule_);
                if (result.condition)
                {
                    foreach (_anglr_syntax_rule_ syntax_Rule in m_generatedRuleList)
                    {
                        if (syntax_Rule.m__identifier_.text == result.idName)
                            syntax_Rule.m__identifier_.text = result.ruleName;
                        syntax_Rule.m__anglr_syntax_production_list_.Iterate (null, (node, appData) =>
                        {
                            node.m__anglr_syntax_production_.m__name_list_?.Iterate (null, (nameList, nameData) =>
                            {
                                if ((_name_list_.production_kind) nameList.kind != _name_list_.production_kind.g__name_list__2)
                                    return null;
                                _g_name_ gname = nameList.m__g_name_;
                                if ((_g_name_.production_kind) gname.kind != _g_name_.production_kind.g__g_name__1)
                                    return null;
                                _name_ name_ = gname.m__name_;
                                if ((_name_.production_kind) name_.kind != _name_.production_kind.g__name__3)
                                    return null;
                                if (name_.m__identifier_.text == result.idName)
                                    name_.m__identifier_.text = result.ruleName;
                                return null;
                            });
                            return null;
                        });
                    }
                }
                else
                {
                    m_generatedRuleList.Add (syntax_Rule_);
                }
                SymbolToken symbolToken = SymbolTable.insert (new SymbolToken (syntax_Rule_.m__identifier_.text, (uint) AnglrClassificationType.NonTerminalName, null, parserPartSymbol));
                if (iterator && anglrCompiler.createIterators)
                    symbolToken.IteratorFlag = true;
                name = new _name_ (new SyntaxTreeToken ((int) AnglrDeclarations.tokens._identifier_, -1, -1, syntax_Rule_.m__identifier_.text), (int) _name_.production_kind.g__name__3);
                TraverseCommon (name);
                Traverse (name);
            }
            return name;
        }

        private SyntaxTreeToken CheckSimpleRuleCondition (_g_name_ g_Name_)
        {
            SyntaxTreeBase node = g_Name_.parent;
            if (!(node is _name_list_))
                return null;
            if (((_name_list_) node).m__name_list_.m__name_list_ != null)
                return null;
            node = node.parent;
            if (!(node is _anglr_syntax_production_))
                return null;
            if (false)
            {
                node = node.parent;
                if (!(node is _anglr_syntax_production_list_))
                    return null;
                if ((_anglr_syntax_production_list_) node != ((_anglr_syntax_production_list_) node).m__anglr_syntax_production_list_)
                    return null;
                node = node.parent;
                if (node is _anglr_syntax_rule_)
                    return ((_anglr_syntax_rule_) node).m__identifier_;
            }
            for (node = node.parent; node is _anglr_syntax_production_list_; node = node.parent)
                ;
            if (node is _anglr_syntax_rule_)
                return ((_anglr_syntax_rule_) node).m__identifier_;
            return null;
        }

        private SyntaxTreeToken CheckSimpleRuleCondition (_anglr_nested_rule_ nested_Rule_)
        {
            if (nested_Rule_.m__anglr_syntax_production_list_name_optional_.m__anglr_syntax_production_list_name_ != null)
                return nested_Rule_.m__anglr_syntax_production_list_name_optional_.m__anglr_syntax_production_list_name_.m__identifier_;

            _anglr_syntax_production_list_ syntax_Production_List_ = nested_Rule_.m__anglr_syntax_production_list_;
            if (syntax_Production_List_.m__anglr_syntax_production_list_ != null)
                return null;
            _anglr_syntax_production_ syntax_Production_ = syntax_Production_List_.m__anglr_syntax_production_;
            if (syntax_Production_.kind != (uint) _anglr_syntax_production_.production_kind.g__anglr_syntax_production__1)
                return null;
            _g_name_ g_Name_ = null;
            for (_name_list_ name_List_ = syntax_Production_.m__name_list_; name_List_ != null; name_List_ = name_List_.m__name_list_)
            {
                if (name_List_.m__marker_list_optional_.m__marker_list_ != null)
                    return null;
                if (name_List_.m__g_name_ != null)
                {
                    if (g_Name_ != null)
                        return null;
                    g_Name_ = name_List_.m__g_name_;
                }
            }
            if (g_Name_ == null)
                return null;
            if (g_Name_.kind != (uint) _g_name_.production_kind.g__g_name__1)
                return null;
            _name_ name_ = g_Name_.m__name_;
            if (name_.kind != (uint) _name_.production_kind.g__name__3)
                return null;
            return name_.m__identifier_;
        }

        private SyntaxTreeToken CheckSimpleNodeCondition (_g_name_ g_Name_)
        {
            if (g_Name_.kind != (uint) _g_name_.production_kind.g__g_name__3)
                return null;
            g_Name_ = g_Name_.m__g_name_;
            if (g_Name_.kind != (uint) _g_name_.production_kind.g__g_name__1)
                return null;
            _name_ p__name_ = g_Name_.m__name_;
            if (p__name_.kind != (uint) _name_.production_kind.g__name__3)
                return null;
            return p__name_.m__identifier_;
        }

        private string GenerateNodeName (SyntaxTreeToken node, string [] suffix, string dname)
        {
            if (dname != null)
                return dname;

            string text = node.text;
            int index = text.LastIndexOf ('>');
            if (index < 0)
            {
                foreach (string str in suffix)
                    text += '-' + str;
            }
            else
            {
                foreach (string str in suffix)
                {
                    text = text.Insert (index, ' ' + str);
                    index += str.Length + 1;
                }
            }
            return text;
        }

        private (bool, string, string) CheckIdentityCondition (_anglr_syntax_rule_ syntax_Rule_)
        {
            _anglr_syntax_production_list_ syntax_Production_List_ = syntax_Rule_.m__anglr_syntax_production_list_;
            if (syntax_Production_List_.m__anglr_syntax_production_list_ != null)
                return (false, null, null);
            _anglr_syntax_production_ syntax_Production_ = syntax_Production_List_.m__anglr_syntax_production_;
            _name_list_ name_List_ = syntax_Production_.m__name_list_;
            if (name_List_ == null)
                return (false, null, null);
            if ((name_List_.m__name_list_ != null) && ((_name_list_.production_kind) name_List_.m__name_list_.kind != _name_list_.production_kind.g__name_list__1))
                return (false, null, null);
            return (false, syntax_Rule_.m__identifier_.text, name_List_.m__g_name_.m__name_.Emit (-1));
        }

        private int m_generatedRuleIndex = 1;
        private bool m_discardProducton = false;
        private SyntaxTreeToken m_simpleRuleName = null;
        private SyntaxTreeToken m_simpleNodeName = null;
        private List<_anglr_syntax_rule_> m_generatedRuleList = new List<_anglr_syntax_rule_> ();
    }
}
