using Microsoft.VisualStudio.LanguageServer.Protocol;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace AnglrJsonRpcMethods
{
    /// <summary>
    /// AnglrGetClassificationSpans Mehod Parameters
    /// </summary>
    [Obfuscation (Exclude = true)]
    [DataContract]
    public class AnglrGetClassificationSpansParams
    {
        [DataMember (Name = "textDocument")]
        public TextDocumentIdentifier TextDocument { get; set; }
        [DataMember (Name = "position")]
        public Position Position { get; set; }
    }

    /// <summary>
    /// AnglrGetClassificationSpans Method Results
    /// </summary>
    [Obfuscation (Exclude = true)]
    [DataContract]
    public class AnglrGetClassificationSpansResult
    {
        [DataMember (Name = "position")]
        public Position Position { get; set; }
        [DataMember (Name = "classifications")]
        public List<(int column, int line, int classification)> ClassificationSpanInfo { get; set; }
    }

    /// <summary>
    /// AnglrGetGetHierarchyItem Mehod Parameters
    /// </summary>
    [Obfuscation (Exclude = true)]
    [DataContract]
    public class AnglrGetGetHierarchyItemParams
    {
        [DataMember (Name = "textDocument")] public TextDocumentIdentifier TextDocument { get; set; }
        [DataMember (Name = "itemId")] public string ItemId { get; set; }
    }

    /// <summary>
    /// AnglrGetGetHierarchyItem Method Results
    /// </summary>
    [Obfuscation (Exclude = true)]
    [DataContract]
    public class AnglrGetGetHierarchyItemData
    {
        [DataMember (Name = "itemId")] public string ItemId { get; set; }
        [DataMember (Name = "itemName")] public string ItemName { get; set; }
        [DataMember (Name = "cookie")] public uint Cookie { get; set; }
        [DataMember (Name = "specie")] public uint Specie { get; set; }
    }

    /// <summary>
    /// AnglrGetGetHierarchyItem Method Results
    /// </summary>
    [Obfuscation (Exclude = true)]
    [DataContract]
    public class AnglrGetGetHierarchyItemResult
    {
        [DataMember (Name = "nodeCategory")] public int NodeCategory { get; set; }
        [DataMember (Name = "nodeSubCategory")] public int NodeSubCategory { get; set; }
        [DataMember (Name = "nodeName")] public string NodeName { get; set; }
        [DataMember (Name = "htmlText")] public string HtmlText { get; set; }
        [DataMember (Name = "items")] public AnglrGetGetHierarchyItemData [] Items { get; set; }
    }

    /// <summary>
    /// AnglrGetDictionaryItem Mehod Parameters
    /// </summary>
    [Obfuscation (Exclude = true)]
    [DataContract]
    public class AnglrGetDictionaryItemParams
    {
        [DataMember (Name = "textDocument")] public TextDocumentIdentifier TextDocument { get; set; }
        [DataMember (Name = "itemId")] public string ItemId { get; set; }
    }

    /// <summary>
    /// AnglrGetDictionaryItem Method Results
    /// </summary>
    [Obfuscation (Exclude = true)]
    [DataContract]
    public class AnglrGetDictionaryItemData
    {
        [DataMember (Name = "itemId")] public string ItemId { get; set; }
        [DataMember (Name = "itemName")] public string ItemName { get; set; }
        [DataMember (Name = "cookie")] public uint Cookie { get; set; }
        [DataMember (Name = "specie")] public uint Specie { get; set; }
    }

    /// <summary>
    /// AnglrGetDictionaryItem Method Results
    /// </summary>
    [Obfuscation (Exclude = true)]
    [DataContract]
    public class AnglrGetDictionaryItemResult
    {
        [DataMember (Name = "nodeCategory")] public int NodeCategory { get; set; }
        [DataMember (Name = "nodeSubCategory")] public int NodeSubCategory { get; set; }
        [DataMember (Name = "nodeName")] public string NodeName { get; set; }
        [DataMember (Name = "htmlText")] public string HtmlText { get; set; }
        [DataMember (Name = "items")] public AnglrGetDictionaryItemData [] Items { get; set; }
    }

    /// <summary>
    /// AnglrGetParserStateItem Mehod Parameters
    /// </summary>
    [Obfuscation (Exclude = true)]
    [DataContract]
    public class AnglrGetParserStateItemParams
    {
        [DataMember (Name = "textDocument")] public TextDocumentIdentifier TextDocument { get; set; }
        [DataMember (Name = "stateNr")] public int StateNr { get; set; }
    }

    /// <summary>
    /// AnglrGetParserStateItem Method Results
    /// </summary>
    [Obfuscation (Exclude = true)]
    [DataContract]
    public class AnglrGetParserStateItemResult
    {
        [DataMember (Name = "stateNumber")] public int StateNumber { get; set; }
        [DataMember (Name = "symbolToken")] public AnglrGetParserStateSymbolTokenData SymbolToken { get; set; }
        [DataMember (Name = "coreSet")] public AnglrGetParserStateCoreData [] CoreSet { get; set; }
        [DataMember (Name = "closureSet")] public AnglrGetParserStateClosureData [] ClosureSet { get; set; }
        [DataMember (Name = "shiftSet")] public AnglrGetParserStateTransitionData [] ShiftSet { get; set; }
        [DataMember (Name = "gotoSet")] public AnglrGetParserStateTransitionData [] GotoSet { get; set; }
        [DataMember (Name = "reductionsSet")] public AnglrGetParserStateReductionsData [] ReductionsSet { get; set; }
        [DataMember (Name = "refByStates")] public int [] RefByStates { get; set; }
    }

    /// <summary>
    /// AnglrGetParserStateItem Method Results
    /// </summary>
    [Obfuscation (Exclude = true)]
    [DataContract]
    public class AnglrGetParserStateSymbolTokenData
    {
        [DataMember (Name = "name")] public string Name { get; set; }
        [DataMember (Name = "synonym")] public string Synonym { get; set; }
        [DataMember (Name = "declarator")] public uint Declarator { get; set; }
        [DataMember (Name = "id")] public int Id { get; set; }
    }

    /// <summary>
    /// AnglrGetParserStateItem Method Results
    /// </summary>
    [Obfuscation (Exclude = true)]
    [DataContract]
    public class AnglrGetParserStateCoreData
    {
        [DataMember (Name = "production")] public AnglrGetParserStateProductionData Production { get; set; }
        [DataMember (Name = "position")] public int Position { get; set; }
        [DataMember (Name = "followSet")] public string [] FollowSet { get; set; }
    }

    /// <summary>
    /// AnglrGetParserStateItem Method Results
    /// </summary>
    [Obfuscation (Exclude = true)]
    [DataContract]
    public class AnglrGetParserStateClosureData
    {
        [DataMember (Name = "productionNode")] public AnglrGetParserStateProductionNodeData ProductionNode { get; set; }
        [DataMember (Name = "followSet")] public string [] FollowSet { get; set; }
    }

    /// <summary>
    /// AnglrGetParserStateItem Method Results
    /// </summary>
    [Obfuscation (Exclude = true)]
    [DataContract]
    public class AnglrGetParserStateTransitionData
    {
        [DataMember (Name = "state")] public int State { get; set; }
        [DataMember (Name = "conflicts")] public uint Conflicts { get; set; }
        [DataMember (Name = "token")] public string Token { get; set; }
    }

    /// <summary>
    /// AnglrGetParserStateItem Method Results
    /// </summary>
    [Obfuscation (Exclude = true)]
    [DataContract]
    public class AnglrGetParserStateReductionsData
    {
        [DataMember (Name = "production")] public AnglrGetParserStateProductionData Production { get; set; }
        [DataMember (Name = "followSet")] public string [] FollowSet { get; set; }
    }

    /// <summary>
    /// AnglrGetParserStateItem Method Results
    /// </summary>
    [Obfuscation (Exclude = true)]
    [DataContract]
    public class AnglrGetParserStateProductionData : INotifyPropertyChanged
    {
        [DataMember (Name = "productionNumber")] public int ProductionNumber { get; set; }
        [DataMember (Name = "productionName")] public string ProductionName { get; set; }
        [DataMember (Name = "rhsNodeSet")] public AnglrGetParserStateSymbolTokenData [] RhsNodeSet { get; set; }
        [DataMember (Name = "breakPoint")] public bool BreakPoint
        {
            get => _breakPoint;
            set
            {
                if (_breakPoint != value)
                {
                    _breakPoint = value;
                    OnPropertyChanged (nameof (BreakPoint));
                }
            }
        }
        private bool _breakPoint;

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged (string propertyName) =>
            PropertyChanged?.Invoke (this, new PropertyChangedEventArgs (propertyName));
    }

    /// <summary>
    /// AnglrGetParserStateItem Method Results
    /// </summary>
    [Obfuscation (Exclude = true)]
    [DataContract]
    public class AnglrGetParserStateProductionNodeData
    {
        [DataMember (Name = "productionName")] public string ProductionName { get; set; }
        [DataMember (Name = "productionSet")] public AnglrGetParserStateProductionData [] ProductionSet { get; set; }
    }

    /// <summary>
    /// AnglrGetParserStatesInfo Mehod Parameters
    /// </summary>
    [Obfuscation (Exclude = true)]
    [DataContract]
    public class AnglrGetParserStatesInfoParams
    {
        [DataMember (Name = "textDocument")] public TextDocumentIdentifier TextDocument { get; set; }
    }

    [Obfuscation (Exclude = true)]
    public class AnglrGetParserStateItemResultSet : Dictionary<int, AnglrGetParserStateItemResult> { }

    /// <summary>
    /// AnglrGetParserStatesInfo Mehod Result
    /// </summary>
    [Obfuscation (Exclude = true)]
    [DataContract]
    public class AnglrGetParserStatesInfoResult
    {
        [DataMember (Name = "parserStatesSet")] public AnglrGetParserStateItemResultSet ParserStatesSet { get; set; }
    }

    /// <summary>
    /// AnglrGetParserStateLink Mehod Parameters
    /// </summary>
    [Obfuscation (Exclude = true)]
    [DataContract]
    public class AnglrGetParserStateLinkParams
    {
        [DataMember (Name = "textDocument")] public TextDocumentIdentifier TextDocument { get; set; }
        [DataMember (Name = "stateNr")] public int StateNr { get; set; }
    }

    /// <summary>
    /// AnglrGetParserStateLink Method Results
    /// </summary>
    [Obfuscation (Exclude = true)]
    [DataContract]
    public class AnglrGetParserStateLinkResult
    {
        [DataMember (Name = "stateLinks")] public int [] StateLinks { get; set; }
    }

    /// <summary>
    /// AnglrGetParserMagicNumber Mehod Parameters
    /// </summary>
    [Obfuscation (Exclude = true)]
    [DataContract]
    public class AnglrGetParserMagicNumberParams
    {
        [DataMember (Name = "textDocument")] public TextDocumentIdentifier TextDocument { get; set; }
    }

    /// <summary>
    /// AnglrGetParserMagicNumber Method Results
    /// </summary>
    [Obfuscation (Exclude = true)]
    [DataContract]
    public class AnglrGetParserMagicNumberResult
    {
        [DataMember (Name = "magicNumber")] public int? MagicNumber { get; set; }
    }

    /// <summary>
    /// AnglrGetParserSyntaxRule Mehod Parameters
    /// </summary>
    [Obfuscation (Exclude = true)]
    [DataContract]
    public class AnglrGetParserSyntaxRuleParams
    {
        [DataMember (Name = "textDocument")] public TextDocumentIdentifier TextDocument { get; set; }
        [DataMember (Name = "ruleName")] public string RuleName { get; set; }
    }

    /// <summary>
    /// AnglrGetParserSyntaxRule Method Results
    /// </summary>
    [Obfuscation (Exclude = true)]
    [DataContract]
    public class AnglrGetParserSyntaxRuleResult
    {
        [DataMember (Name = "syntaxRule")] public AnglrGetParserSyntaxRuleData SyntaxRule { get; set; }
    }

    /// <summary>
    /// AnglrGetParserSyntaxRules Mehod Parameters
    /// </summary>
    [Obfuscation (Exclude = true)]
    [DataContract]
    public class AnglrGetParserSyntaxRulesParams
    {
        [DataMember (Name = "textDocument")] public TextDocumentIdentifier TextDocument { get; set; }
    }

    /// <summary>
    /// AnglrGetParserSyntaxRules Method Results
    /// </summary>
    [Obfuscation (Exclude = true)]
    [DataContract]
    public class AnglrGetParserSyntaxRulesResult
    {
        [DataMember (Name = "syntaxRuleList")] public AnglrGetParserSyntaxRuleData [] SyntaxRuleList { get; set; }
    }

    /// <summary>
    /// AnglrGetParserSyntaxRuleData Data
    /// </summary>
    [Obfuscation (Exclude = true)]
    [DataContract]
    public class AnglrGetParserSyntaxRuleData
    {
        [DataMember (Name = "syntaxRuleName")] public AnglrGetParserStateSymbolTokenData SyntaxRuleName { get; set; }
        [DataMember (Name = "productions")] public AnglrGetParserStateProductionData [] Productions { get; set; }
    }

    /// <summary>
    /// AnglrGetItemNavigationInfoRequest
    /// </summary>
    [Obfuscation (Exclude = true)]
    [DataContract]
    public class AnglrGetItemNavigationInfoRequest
    {
        [DataMember (Name = "textDocument")] public TextDocumentIdentifier TextDocument { get; set; }
        [DataMember (Name = "itemId")] public string ItemId { get; set; }
    }

    /// <summary>
    /// AnglrGetItemNavigationInfoResponse
    /// </summary>
    [Obfuscation (Exclude = true)]
    [DataContract]
    public class AnglrGetItemNavigationInfoResponse
    {
        [DataMember (Name = "itemLineno")] public int ItemLineno { get; set; }
        [DataMember (Name = "itemColumn")] public int ItemColumn { get; set; }
    }

    /// <summary>
    /// AnglrGetCompileFragmentRequest
    /// </summary>
    [Obfuscation (Exclude = true)]
    [DataContract]
    public class AnglrGetCompileFragmentRequest
    {
        [DataMember (Name = "textDocument")] public TextDocumentIdentifier TextDocument { get; set; }
        [DataMember (Name = "itemId")] public string ItemId { get; set; }
    }

    /// <summary>
    /// AnglrGetCompileFragmentResponse
    /// </summary>
    [Obfuscation (Exclude = true)]
    [DataContract]
    public class AnglrGetCompileFragmentResponse
    {
        [DataMember (Name = "result")] public int Result { get; set; }
        [DataMember (Name = "fragment")] public string Fragment { get; set; }
    }

    [Obfuscation (Exclude = true)]
    [DataContract]
    public class AnglrLspLogMessageNotification : EventArgs
    {
        [DataMember (Name = "logLevel")] public int LogLevel { get; set; }
        [DataMember (Name = "message")] public string Message { get; set; }
        [DataMember (Name = "flags")] public uint Flags { get; set; }
    }

    /// <summary>
    /// Method Names and Prototypes
    /// </summary>
    [Obfuscation (Exclude = true)]
    public static class AnglrMethods
    {
        /// <summary>
        /// Method Names
        /// </summary>
        public const string AnglrGetClassificationSpansName = "AnglrGetClassificationSpans";
        public const string AnglrGetGetHierarchyItemName = "AnglrGetGetHierarchyItem";
        public const string AnglrGetDictionaryItemName = "AnglrGetDictionaryItem";
        public const string AnglrGetParserStateItemName = "AnglrGetParserStateItem";
        public const string AnglrGetParserStatesInfoName = "AnglrGetParserStatesInfo";
        public const string AnglrGetParserStateLinkName = "AnglrGetParserStateLink";
        public const string AnglrGetParserSyntaxRuleName = "AnglrGetParserSyntaxRule";
        public const string AnglrGetParserSyntaxRulesName = "AnglrGetParserSyntaxRules";
        public const string AnglrGetParserMagicNumberName = "AnglrGetParserMagicNumber";
        public const string AnglrGetItemNavigationInfoName = "AnglrGetItemNavigationInfo";
        public const string AnglrGetCompileFragmentName = "AnglrGetCompileFragment";
        public const string AnglrLspLogMessageName = "AnglrLspLogMessage";
        /// <summary>
        /// Method Prototypes
        /// </summary>
        public static readonly LspRequest<AnglrGetClassificationSpansParams, AnglrGetClassificationSpansResult> AnglrGetClassificationSpans =
            new LspRequest<AnglrGetClassificationSpansParams, AnglrGetClassificationSpansResult> (AnglrGetClassificationSpansName);
        public static readonly LspRequest<AnglrGetGetHierarchyItemParams, AnglrGetGetHierarchyItemResult> AnglrGetGetHierarchyItem =
            new LspRequest<AnglrGetGetHierarchyItemParams, AnglrGetGetHierarchyItemResult> (AnglrGetGetHierarchyItemName);
        public static readonly LspRequest<AnglrGetDictionaryItemParams, AnglrGetDictionaryItemResult> AnglrGetDictionaryItem =
            new LspRequest<AnglrGetDictionaryItemParams, AnglrGetDictionaryItemResult> (AnglrGetDictionaryItemName);
        public static readonly LspRequest<AnglrGetParserStateItemParams, AnglrGetParserStateItemResult> AnglrGetParserStateItem =
            new LspRequest<AnglrGetParserStateItemParams, AnglrGetParserStateItemResult> (AnglrGetParserStateItemName);
        public static readonly LspRequest<AnglrGetParserStatesInfoParams, AnglrGetParserStatesInfoResult> AnglrGetParserStatesInfo =
            new LspRequest<AnglrGetParserStatesInfoParams, AnglrGetParserStatesInfoResult> (AnglrGetParserStatesInfoName);
        public static readonly LspRequest<AnglrGetParserStateLinkParams, AnglrGetParserStateLinkResult> AnglrGetParserStateLink =
            new LspRequest<AnglrGetParserStateLinkParams, AnglrGetParserStateLinkResult> (AnglrGetParserStateLinkName);
        public static readonly LspRequest<AnglrGetParserMagicNumberParams, AnglrGetParserMagicNumberResult> AnglrGetParserMagicNumber =
            new LspRequest<AnglrGetParserMagicNumberParams, AnglrGetParserMagicNumberResult> (AnglrGetParserMagicNumberName);
        public static readonly LspRequest<AnglrGetParserSyntaxRuleParams, AnglrGetParserSyntaxRuleResult> AnglrGetParserSyntaxRule =
            new LspRequest<AnglrGetParserSyntaxRuleParams, AnglrGetParserSyntaxRuleResult> (AnglrGetParserSyntaxRuleName);
        public static readonly LspRequest<AnglrGetParserSyntaxRulesParams, AnglrGetParserSyntaxRulesResult> AnglrGetParserSyntaxRules =
            new LspRequest<AnglrGetParserSyntaxRulesParams, AnglrGetParserSyntaxRulesResult> (AnglrGetParserSyntaxRulesName);
        public static readonly LspRequest<AnglrGetItemNavigationInfoRequest, AnglrGetItemNavigationInfoResponse> AnglrGetItemNavigationInfo =
            new LspRequest<AnglrGetItemNavigationInfoRequest, AnglrGetItemNavigationInfoResponse> (AnglrGetItemNavigationInfoName);
        public static readonly LspRequest<AnglrGetCompileFragmentRequest, AnglrGetCompileFragmentResponse> AnglrGetCompileFragment =
            new LspRequest<AnglrGetCompileFragmentRequest, AnglrGetCompileFragmentResponse> (AnglrGetCompileFragmentName);
        public static readonly LspNotification<AnglrLspLogMessageNotification> AnglrLspLogMessage =
            new LspNotification<AnglrLspLogMessageNotification> (AnglrLspLogMessageName);
    }
}
