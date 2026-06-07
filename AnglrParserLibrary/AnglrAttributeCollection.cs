using Anglr.Compiler;
using Anglr.Parser;
using Anglr.Parser.Core;
using Anglr.Parser.SyntaxTree;
using AnglrLibrary;
using AnglrLogLibrary;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace AnglrParserLibrary
{
    static public class AnglrExtensions
    {
        public static string Correct (this string name)
        {
            string correctName = "";
            foreach (char c in name)
                correctName += (char.IsLetterOrDigit (c)) ? c : '_';
            return correctName;
        }
    }

    public class GeneralPart
    {
        public anglrCompiler Compiler { get; private set; }
        public SymbolTable SymbolTable { get; private set; }
        public IAnglrLogger AnglrLogger { get; private set; }
        public _general_part_ part { get; private set; } = null;
        public int counter { get; private set; } = -1;
        public SymbolToken generalPartSymbol { get; private set; } = null;

        public string defaultNameSpace { get; private set; } = "";
        public string codeDir { get; private set; } = @".\";

        public GeneralPart (anglrCompiler compiler, _general_part_ p__general_part_, int index, SymbolToken generalPartSymbol)
        {
            Compiler = compiler;
            SymbolTable = Compiler?.symbolTable;
            AnglrLogger = Compiler?.AnglrLogger ?? new VoidAnglrLogger ();

            defaultNameSpace = Path.GetFileNameWithoutExtension (Compiler?.sourceFileName).Correct ();
            codeDir = @".\";

            this.part = p__general_part_;
            this.counter = index;
            this.generalPartSymbol = generalPartSymbol;

            p__general_part_.m__attribute_list_optional__1.m__attribute_list_?.Iterate (null, (node, appData) =>
            {
                _attribute_ attribute = node.m__attribute_;
                if (attribute != null)
                    switch (attribute.m__identifier_.text)
                    {
                        case "CompilationInfo":
                            ReadGeneralCompilationInfoAttribute (attribute);
                            break;
                        default:
                            AnglrLogger?.WarnLine ("Unknown general attribute: " + attribute.m__identifier_.text);
                            break;
                    }
                return null;
            });
        }

        private void ReadGeneralCompilationInfoAttribute (_attribute_ attribute)
        {
            attribute.m__name_value_list_optional_.m__name_value_list_?.Iterate (null, (node, appData) =>
            {
                _name_value_pair_ name_Value_Pair_ = node.m__name_value_pair_;
                switch (name_Value_Pair_.m__identifier_.text)
                {
                    case "NameSpace":
                        defaultNameSpace = name_Value_Pair_.m__cstring_.text.Substring (1, name_Value_Pair_.m__cstring_.text.Length - 2);
                        break;
                    case "CodeDir":
                        codeDir = name_Value_Pair_.m__cstring_.text.Substring (1, name_Value_Pair_.m__cstring_.text.Length - 2);
                        break;
                    default:
                        AnglrLogger?.WarnLine ("Unknown name-value pair for " + attribute.m__identifier_.text + " attribute :" + name_Value_Pair_.Emit (-1));
                        break;
                }
                return null;
            });
        }
    }

    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class GeneralParts : Dictionary<string, GeneralPart>
    {
    }

    public class DeclarationsPart
    {
        public anglrCompiler Compiler { get; private set; }
        public SymbolTable SymbolTable { get; private set; }
        public IAnglrLogger AnglrLogger { get; private set; }
        public _declaration_part_ part { get; private set; } = null;
        public int counter { get; private set; } = -1;
        public SymbolToken declarationPartSymbol { get; private set; } = null;

        public string declarationsClassName { get; private set; } = "";
        public string declarationsNameSpace { get; private set; } = "";
        public string declarationsAccess { get; private set; } = "public";
        public string codeDir { get; private set; } = @".\";
        public string tokenPrefix { get; private set; } = "";
        public RegexDictionary regexparts { get; private set; } = new RegexDictionary ();

        private Regex regparts = new Regex ("{([a-zA-Z_][a-zA-Z0-9_.-]*|<[a-zA-Z0-9_ .-]+>)}");

        public DeclarationsPart (anglrCompiler compiler, _declaration_part_ p__declaration_part_, int index, SymbolToken declSymbol)
        {
            Compiler = compiler;
            SymbolTable = Compiler?.symbolTable;
            AnglrLogger = Compiler?.AnglrLogger ?? new VoidAnglrLogger ();

            this.part = p__declaration_part_;
            this.counter = index;
            this.declarationPartSymbol = declSymbol;

            declarationsNameSpace = Path.GetFileNameWithoutExtension (Compiler?.sourceFileName).Correct ();
            declarationsClassName = declSymbol?.correctName;

            p__declaration_part_.m__attribute_list_optional_.m__attribute_list_?.Iterate (null, (node, appData) =>
            {
                _attribute_ attribute = node.m__attribute_;
                if (attribute != null)
                    switch (attribute.m__identifier_.text)
                    {
                        case "CompilationInfo":
                            ReadCompilationInfo (attribute);
                            break;
                        default:
                            AnglrLogger?.WarnLine ("Unknown general attribute: " + attribute.m__identifier_.text);
                            break;
                    }
                return null;
            });
        }

        private void ReadCompilationInfo (_attribute_ attribute)
        {
            attribute.m__name_value_list_optional_.m__name_value_list_?.Iterate (null, (node, appData) =>
            {
                _name_value_pair_ name_Value_Pair_ = node.m__name_value_pair_;
                switch (name_Value_Pair_.m__identifier_.text)
                {
                    case "NameSpace":
                        declarationsNameSpace = name_Value_Pair_.m__cstring_.text.Substring (1, name_Value_Pair_.m__cstring_.text.Length - 2);
                        break;
                    case "ClassName":
                        declarationsClassName = name_Value_Pair_.m__cstring_.text.Substring (1, name_Value_Pair_.m__cstring_.text.Length - 2);
                        break;
                    case "Access":
                        declarationsAccess = name_Value_Pair_.m__cstring_.text.Substring (1, name_Value_Pair_.m__cstring_.text.Length - 2);
                        break;
                    case "CodeDir":
                        codeDir = name_Value_Pair_.m__cstring_.text.Substring (1, name_Value_Pair_.m__cstring_.text.Length - 2);
                        break;
                    case "TokenPrefix":
                        tokenPrefix = name_Value_Pair_.m__cstring_.text.Substring (1, name_Value_Pair_.m__cstring_.text.Length - 2);
                        break;
                    default:
                        AnglrLogger?.WarnLine ("Unknown name-value pair for " + attribute.m__identifier_.text + " attribute :" + name_Value_Pair_.Emit (-1));
                        break;
                }
                return null;
            });
        }

        public void RegexAdd (_regex_definition_ p__regex_definition_)
        {
            {
                SyntaxTreeToken p_identifier = p__regex_definition_.m__identifier_;
                SyntaxTreeToken p_regex = p__regex_definition_.m__regular_expression_;
                SymbolToken symbolToken = new SymbolToken (p_identifier.text, (uint) AnglrClassificationType.RegexName, null, declarationPartSymbol);
                SymbolToken symbolTokenRef = SymbolTable.insert (symbolToken);
                regstr regval = null;
                if (!regexparts.TryGetValue (symbolTokenRef, out regval))
                {
                    string text = p_regex.text.Trim ();
                    regexparts [symbolTokenRef] = new regstr (text);
                    AnglrLogger?.DebugLine ("Regular expression part " + p_identifier.text + " = '" + text + "'");
                }
                else
                {
                    AnglrLogger?.WarnLine ("Redefinition of regular expression part: '" + p_identifier.text + "', old value: '" + regval.str + "', new value: '" + p_regex.text + "'");
                }
            }
        }

        public string RegexResolve (SyntaxTreeToken p_regex, ref bool expanded)
        {
            expanded = false;
            string text = p_regex.text.Trim ();
            foreach (Match match in regparts.Matches (p_regex.text))
            {
                string partname = match.Value.Substring (1, match.Value.Length - 2);
                SymbolToken symbolTokenRef = SymbolTable.find (new SymbolToken (partname, (uint) AnglrClassificationType.RegexName, null, declarationPartSymbol));
                if (symbolTokenRef == null)
                {
                    AnglrLogger?.WarnLine ("Undefined substitution '" + partname + "' in " + p_regex.text);
                    continue;
                }
                regstr partvalue = null;
                if (!regexparts.TryGetValue (symbolTokenRef, out partvalue))
                {
                    AnglrLogger?.WarnLine ("Undefined substitution '" + partname + "' in " + p_regex.text);
                    continue;
                }
                text = text.Replace (match.Value, partvalue.str);
                expanded = true;
            }
            return text;
        }

        public void RegexResolveRefs ()
        {
            while (true)
            {
                int changes = 0;
                foreach (KeyValuePair<SymbolToken, regstr> pair in regexparts)
                {
                    string regName = pair.Key.name;
                    regstr regValue = pair.Value;
                    string text = regValue.str;
                    foreach (Match match in regparts.Matches (text))
                    {
                        string partname = match.Value.Substring (1, match.Value.Length - 2);
                        SymbolToken symbolToken = SymbolTable.find (new SymbolToken (partname, (uint) AnglrClassificationType.RegexName, null, declarationPartSymbol));
                        regstr partvalue = null;
                        if (symbolToken == null)
                        {
                            AnglrLogger?.WarnLine ("Undefined substitution '" + partname + "' in " + regName);
                            continue;
                        }
                        if (!regexparts.TryGetValue (symbolToken, out partvalue))
                        {
                            AnglrLogger?.WarnLine ("Undefined substitution '" + partname + "' in " + regName);
                            continue;
                        }
                        text = text.Replace (match.Value, partvalue.str);
                    }
                    if (regValue.str == text)
                        continue;
                    regValue.str = text;
                    ++changes;
                }
                if (changes == 0)
                    break;
            }
        }

        public StringBuilder Generate ()
        {
            RegexResolveRefs ();

            StringBuilder stringBuilder = new StringBuilder ();
            SymbolToken contextSymbol = (SymbolToken) ((AppInfo) part.m__identifier_.appInfo) [AppInfoType.SymbolToken];

            stringBuilder.AppendLine ($"namespace {declarationsNameSpace}");
            stringBuilder.AppendLine ($"{{");
            stringBuilder.AppendLine ($"\tpublic class {declarationsClassName}");
            stringBuilder.AppendLine ($"\t{{");
            int index = 258;
            stringBuilder.AppendLine ($"\t\t// values of terminal symbols");
            stringBuilder.AppendLine ($"\t\tpublic class tokens");
            stringBuilder.AppendLine ($"\t\t{{");
            for (symtab.Enumerator enumerator = SymbolTable.enumerator; enumerator.MoveNext ();)
            {
                SymbolToken symbolToken = enumerator.Current.Value;
                if (symbolToken.context != contextSymbol)
                    continue;
                if (symbolToken.AliasFlag)
                    continue;
                if (symbolToken.declarator != (uint) AnglrClassificationType.TerminalName)
                    continue;
                stringBuilder.AppendLine ($"\t\t\tpublic const int {tokenPrefix}{symbolToken.correctName} = {index++};");
            }
            stringBuilder.AppendLine ($"\t\t}}");
            stringBuilder.AppendLine ();
            stringBuilder.AppendLine ($"\t\t// values of regular expressions");
            stringBuilder.AppendLine ($"\t\tpublic class regex");
            stringBuilder.AppendLine ($"\t\t{{");
            for (symtab.Enumerator enumerator = SymbolTable.enumerator; enumerator.MoveNext ();)
            {
                SymbolToken symbolToken = enumerator.Current.Value;
                if (symbolToken.context != contextSymbol)
                    continue;
                if (symbolToken.AliasFlag)
                    continue;
                if (symbolToken.declarator != (uint) AnglrClassificationType.RegexName)
                    continue;
                if (!regexparts.TryGetValue (symbolToken, out var regexp))
                    continue;
                stringBuilder.AppendLine ($"\t\t\tpublic const string {symbolToken.correctName} = @\"{regexp.str.Replace ("\"", "\"\"")}\";");
            }
            stringBuilder.AppendLine ($"\t\t}}");
            stringBuilder.AppendLine ($"\t}}");
            stringBuilder.AppendLine ($"}}");

            return stringBuilder;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class DeclarationParts : Dictionary<string, DeclarationsPart>
    {
    }

    public class ScannerPart
    {
        public anglrCompiler Compiler { get; private set; }
        public SymbolTable SymbolTable { get; private set; }
        public IAnglrLogger AnglrLogger { get; private set; }
        public _scanner_part_ part { get; private set; } = null;
        public int counter { get; private set; } = -1;
        public SymbolToken scannerPartSymbol { get; private set; } = null;

        public SymbolToken declarationPartSymbol { get; private set; } = null;

        public string declarationsId { get; private set; } = "";
        public string regexNameSpace { get; private set; } = "";
        public string regexClassName { get; private set; } = "";
        public string regexAccess { get; private set; } = "public";
        public string codeDir { get; private set; } = @".\";
        public List<string> lexRegSet { get; private set; } = new List<string> ();
        public string matchCode { get; private set; } = "";
        public string eventCode { get; private set; } = "";
        public string eventExample { get; private set; } = "";
        public string scannCode { get; private set; } = "";

        private SortedSet<string> nameSpaces = new SortedSet<string> ();

        public static readonly string g_UsingDeclarations = "$(UsingDeclarations)";
        public static readonly string g_ScannerNamespace = "$(ScannerNamespace)";
        public static readonly string g_regexClassName = "$(regexClassName)";
        public static readonly string g_regexScannerClassName = "$(regexScannerClassName)";
        public static readonly string g_regexScannerId = "$(regexScannerId)";
        public static readonly string g_regexLexStr = "$(regexLexStr)";
        public static readonly string g_regexEventsCode = "$(regexEventsCode)";
        public static readonly string g_regexScannerCode = "$(regexScannerCode)";
        public static readonly string [] g_RegexClassSourceStr =
        {
            "﻿using System;",
            "using System.IO;",
            "using System.Text.RegularExpressions;",
            "using System.Collections.Generic;",
            "using Anglr.Lexer.Core;",
            "$(UsingDeclarations)",
            "",
            "$(ScannerNamespace)",
            "{",
            "\tpublic class $(regexClassName) : Regex, RegexInterface\n\t{",
            "\t\tpublic $(regexClassName) ($(regexScannerClassName) scanner) : base (@\"$(regexLexStr)\", RegexOptions.ExplicitCapture)",
            "\t\t{",
            "\t\t\tScanner = scanner;",
            "\t\t}",
            "",
            "\t\tpublic $(regexScannerClassName) Scanner { get; private set; }",
            "\t\tpublic static string Id { get; private set; } = \"$(regexScannerId)\";",
            "",
            "\t\tpublic delegate int scannerCallback ($(regexClassName) regex, $(regexScannerClassName) scanner);",
            "",
            "$(regexEventsCode)",
            "",
            "\t\tpublic string text { get; private set; }",
            "",
            "\t\tpublic (int, int) match (string currentLine, int currentPosition)",
            "\t\t{",
            "\t\t\tint matchIndex = 0;",
            "\t\t\tint matchLength = 0;",
            "\t\t\ttry",
            "\t\t\t{",
            "\t\t\t\ttext = \"\";",
            "\t\t\t\tMatch match = Match (currentLine, currentPosition);",
            "\t\t\t\tif (!match.Success)",
            "\t\t\t\t\treturn (-1, 0);",
            "\t\t\t\t\tint index = 0;",
            "\t\t\t\tforeach (Group group in match.Groups)",
            "\t\t\t\t{",
            "\t\t\t\t\tif (index++ == 0)",
            "\t\t\t\t\t\tcontinue;",
            "\t\t\t\t\tif (!group.Success)",
            "\t\t\t\t\t\tcontinue;",
            "\t\t\t\t\ttry",
            "\t\t\t\t\t{",
            "\t\t\t\t\t\tmatchLength = match.Value.Length;",
            "\t\t\t\t\t\tmatchIndex = index - 1;",
            "\t\t\t\t\t\ttext = currentLine.Substring (currentPosition, matchLength);",
            "\t\t\t\t\t}",
            "\t\t\t\t\tcatch (Exception)",
            "\t\t\t\t\t{",
            "\t\t\t\t\t\tcontinue;",
            "\t\t\t\t\t}",
            "\t\t\t\t\tbreak;",
            "\t\t\t\t}",
            "\t\t\t}",
            "\t\t\tcatch (Exception e)",
            "\t\t\t{",
            "\t\t\t\treturn (-2, 0);",
            "\t\t\t}",
            "",
            "\t\t\tint? result = null;",
            "",
            "\t\t\tswitch (matchIndex)",
            "\t\t\t{",
            "\t\t\t$(regexScannerCode)",
            "\t\t\t}",
            "\t\t\treturn (result != null) ? (result.Value, matchLength) : (0, matchLength);",
            "\t\t}",
            "\t}",
            "}",
        };

        public ScannerPart (anglrCompiler compiler, _scanner_part_ p__scanner_part_, int index, SymbolToken scannerPartSymbol)
        {
            Compiler = compiler;
            SymbolTable = Compiler?.symbolTable;
            AnglrLogger = Compiler?.AnglrLogger ?? new VoidAnglrLogger ();

            this.part = p__scanner_part_;
            this.counter = index;
            this.scannerPartSymbol = scannerPartSymbol;

            regexNameSpace = $"{Path.GetFileNameWithoutExtension (Compiler?.sourceFileName).Correct()}.Scanners";
            regexClassName = scannerPartSymbol?.correctName;

            p__scanner_part_.m__attribute_list_optional_.m__attribute_list_?.Iterate (null, (node, appData) =>
            {
                _attribute_ attribute = node.m__attribute_;
                if (attribute != null)
                    switch (attribute.m__identifier_.text)
                    {
                        case "Declarations":
                            ReadDeclarations (attribute);
                            break;
                        case "CompilationInfo":
                            ReadCompilationInfo (attribute);
                            break;
                        default:
                            AnglrLogger?.WarnLine ("Unknown general attribute: " + attribute.m__identifier_.text);
                            break;
                    }
                return null;
            });
        }

        private void ReadDeclarations (_attribute_ attribute)
        {
            attribute.m__name_value_list_optional_.m__name_value_list_?.Iterate (null, (node, appData) =>
            {
                _name_value_pair_ name_Value_Pair_ = node.m__name_value_pair_;
                switch (name_Value_Pair_.m__identifier_.text)
                {
                    case "Id":
                        declarationsId = name_Value_Pair_.m__cstring_.text.Substring (1, name_Value_Pair_.m__cstring_.text.Length - 2);
                        declarationPartSymbol = SymbolTable.find (new SymbolToken (declarationsId, (uint) AnglrClassificationType.DeclarationsPartName));
                        if (!nameSpaces.Contains (declarationsId))
                            nameSpaces.Add (declarationsId);
                        break;
                    default:
                        AnglrLogger?.WarnLine ("Unknown name-value pair for " + attribute.m__identifier_.text + " attribute :" + name_Value_Pair_.Emit (-1));
                        break;
                }
                return null;
            });
        }

        private void ReadCompilationInfo (_attribute_ attribute)
        {
            attribute.m__name_value_list_optional_.m__name_value_list_?.Iterate (null, (node, appData) =>
            {
                _name_value_pair_ name_Value_Pair_ = node.m__name_value_pair_;
                switch (name_Value_Pair_.m__identifier_.text)
                {
                    case "NameSpace":
                        regexNameSpace = name_Value_Pair_.m__cstring_.text.Substring (1, name_Value_Pair_.m__cstring_.text.Length - 2);
                        break;
                    case "ClassName":
                        regexClassName = name_Value_Pair_.m__cstring_.text.Substring (1, name_Value_Pair_.m__cstring_.text.Length - 2);
                        break;
                    case "Access":
                        regexAccess = name_Value_Pair_.m__cstring_.text.Substring (1, name_Value_Pair_.m__cstring_.text.Length - 2);
                        break;
                    case "CodeDir":
                        codeDir = name_Value_Pair_.m__cstring_.text.Substring (1, name_Value_Pair_.m__cstring_.text.Length - 2);
                        break;
                    default:
                        AnglrLogger?.WarnLine ("Unknown name-value pair for " + attribute.m__identifier_.text + " attribute :" + name_Value_Pair_.Emit (-1));
                        break;
                }
                return null;
            });
        }

        public void LoadData (StringBuilder matchCode, StringBuilder eventCode, StringBuilder eventExample, StringBuilder scannerCode)
        {
            this.matchCode = matchCode.ToString ();
            this.eventCode = eventCode.ToString ();
            this.scannCode = scannerCode.ToString ();
            if (eventExample.Length > 0)
            {
                this.eventExample += $"\t\t\t\t// implement all {regexClassName}'s events otherwise they will throw exceptions\n";
                this.eventExample += $"\t\t\t\t// or they will mimic 'skip' scanner operation if you delete them\n";
                this.eventExample += $"\t\t\t\t{{\n";
                this.eventExample += $"\t\t\t\t\t{regexNameSpace}.{regexClassName} scanner = lexer.{regexClassName};\n";
                this.eventExample += eventExample.ToString ();
                this.eventExample += $"\t\t\t\t}}";
            }
        }

        public string Generate (AnglrAttributeCollection attributeCollection)
        {
            StringBuilder sb = new StringBuilder ();
            string className = regexClassName;
            string scannerClassName = $"LexerBase";
            string lexStr = matchCode.Replace ("\"", "\"\"");
            string usingDecls = "";
            foreach(string ns in nameSpaces)
            {
                if (!attributeCollection.declarationParts.ContainsKey (ns))
                    continue;
                usingDecls += $"using {attributeCollection.declarationParts[ns].declarationsNameSpace};";
            }
            foreach (string line in g_RegexClassSourceStr)
            {
                sb.AppendLine
                    (
                    line
                    .Replace (g_UsingDeclarations, usingDecls)
                    .Replace (g_ScannerNamespace, $"namespace {regexNameSpace}")
                    .Replace (g_regexClassName, className)
                    .Replace (g_regexScannerClassName, scannerClassName)
                    .Replace (g_regexScannerId, part.m__identifier_.text)
                    .Replace (g_regexLexStr, lexStr)
                    .Replace (g_regexEventsCode, eventCode)
                    .Replace (g_regexScannerCode, scannCode)
                    );
            }
            return sb.ToString ();
        }
    }

    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class ScannerParts : Dictionary<string, ScannerPart>
    {
    }

    public class LexerPart
    {
        public anglrCompiler Compiler { get; private set; }
        public SymbolTable SymbolTable { get; private set; }
        public IAnglrLogger AnglrLogger { get; private set; }
        public _lexer_part_ part { get; private set; } = null;
        public int counter { get; private set; } = -1;
        public SymbolToken lexerPartSymbol { get; private set; } = null;

        public SymbolTable scannerSymbols { get; private set; } = new SymbolTable ();
        public SymbolToken initialScanner { get; private set; } = null;
        public string lexerClassName { get; private set; } = "";
        public string lexerNameSpace { get; private set; } = "";
        public string lexerAccess { get; private set; } = "public";
        public string codeDir { get; private set; } = @".\";

        private SymbolToken firstScanner = null;

        public static readonly string g_UsingDeclarations = "$(UsingDeclarations)";
        public static readonly string g_ScannerNamespace = "$(ScannerNamespace)";
        public static readonly string g_ScannerClassDef = "$(ScannerClassDef)";
        public static readonly string g_ScannerRegexGetters = "$(ScannerRegexGetters)";
        public static readonly string g_ScannerClassTextReader = "$(ScannerClassTextReader)";
        public static readonly string g_ScannerClassStringArray = "$(ScannerClassStringArray)";
        public static readonly string g_ScannerClassString = "$(ScannerClassString)";
        public static readonly string g_ScannerCodes = "$(ScannerCodes)";
        public static readonly string g_InitialScanner = "$(InitialScanner)";
        public static readonly string g_ScannerInitCode = "$(ScannerInitCode)";
        public static readonly string g_ScannerIds = "$(ScannerIds)";
        public static readonly string g_MatchScanner = "$(MatchScanner)";

        public static readonly string [] g_ScannerSkeleton =
        {
            "using System;",
            "using System.IO;",
            "using System.Text.RegularExpressions;",
            "using System.Collections.Generic;",
            "using Anglr.Lexer.Core;",
            "$(UsingDeclarations)",
            "",
            "$(ScannerNamespace)",
            "{",
            "",
            "$(ScannerClassDef)",
            "\t{",
            "$(ScannerRegexGetters)",
            "\t\tpublic object [] Info { get; private set; }",
            "$(ScannerClassTextReader)",
            "\t\t{",
            "\t\t\tInfo = info;",
            "\t\t\tInit ();",
            "\t\t\tpushInput (textReader);",
            "$(InitialScanner)",
            "\t\t}",
            "",
            "$(ScannerClassStringArray)",
            "\t\t{",
            "\t\t\tInfo = info;",
            "\t\t\tInit ();",
            "\t\t\tpushInput (lines);",
            "$(InitialScanner)",
            "\t\t}",
            "",
            "$(ScannerClassString)",
            "\t\t{",
            "\t\t\tInfo = info;",
            "\t\t\tInit ();",
            "\t\t\tpushInput (line);",
            "$(InitialScanner)",
            "\t\t}",
            "",
            "\t\tpublic void Init ()",
            "\t\t{",
            "\t\t\tregarray = new RegexInterface []",
            "\t\t\t{",
            "$(ScannerInitCode)",
            "\t\t\t};",
            "\t\t}",
            "",
            "\t\tpublic override int ScannerNameToId (string name) => scannerIds [name];",
            "\t\tpublic override RegexInterface GetRegexInterface (int index) => regarray [index];",
            "\t\t// array of regular expression scanners",
            "\t\tprivate RegexInterface [] regarray = null;",
            "\t\tprivate Dictionary<string, int> scannerIds = new Dictionary<string, int>",
            "\t\t{",
            "$(ScannerIds)",
            "\t\t};",
            "",
            "\t\t// scanner codes",
            "$(ScannerCodes)",
            "\t}",
            "}",
        };

        public LexerPart (anglrCompiler compiler, _lexer_part_ p__lexer_part_, int index, SymbolToken lexerPartSymbol)
        {
            Compiler = compiler;
            SymbolTable = Compiler?.symbolTable;
            AnglrLogger = Compiler?.AnglrLogger ?? new VoidAnglrLogger ();

            lexerNameSpace = $"{Path.GetFileNameWithoutExtension (Compiler?.sourceFileName).Correct()}.Lexers";
            lexerClassName = lexerPartSymbol.correctName;

            this.part = p__lexer_part_;
            this.counter = index;
            this.lexerPartSymbol = lexerPartSymbol;

            p__lexer_part_.m__attribute_list_optional_.m__attribute_list_?.Iterate (null, (node, appData) =>
            {
                _attribute_ attribute = node.m__attribute_;
                if (attribute != null)
                    switch (attribute.m__identifier_.text)
                    {
                        case "UseScanner":
                            ReadScanner (attribute);
                            break;
                        case "CompilationInfo":
                            ReadCompilationInfo (attribute);
                            break;
                        default:
                            AnglrLogger?.WarnLine ("Unknown lexer attribute: " + attribute.m__identifier_.text);
                            break;
                    }
                return null;
            });

            if (initialScanner == null)
            {
                if (firstScanner == null)
                    AnglrLogger?.WarnLine ($"At least one scanner id should be used to define lexer");
                initialScanner = firstScanner;
            }
        }

        private void ReadScanner (_attribute_ attribute)
        {
            attribute.m__name_value_list_optional_.m__name_value_list_?.Iterate (null, (node, appData) =>
            {
                _name_value_pair_ name_Value_Pair_ = node.m__name_value_pair_;
                switch (name_Value_Pair_.m__identifier_.text)
                {
                    case "ScannerId":
                    {
                        string scannerId = name_Value_Pair_.m__cstring_.text.Substring (1, name_Value_Pair_.m__cstring_.text.Length - 2);
                        SymbolToken scannerSymbol = SymbolTable.find (new SymbolToken (scannerId, (uint) AnglrClassificationType.ScannerPartName));
                        if (scannerSymbol == null)
                        {
                            AnglrLogger?.WarnLine ($"symbol {scannerId} does not represent scanner ID");
                            break;
                        }
                        if (scannerSymbols.find (scannerSymbol) != null)
                        {
                            AnglrLogger?.WarnLine ($"redefined scanner ID {scannerId}");
                            break;
                        }
                        if (firstScanner == null)
                            firstScanner = scannerSymbol;
                        scannerSymbols.insert (scannerSymbol);
                    }
                    break;
                    case "InitialScanner":
                    {
                        string scannerId = name_Value_Pair_.m__cstring_.text.Substring (1, name_Value_Pair_.m__cstring_.text.Length - 2);
                        SymbolToken scannerSymbol = SymbolTable.find (new SymbolToken (scannerId, (uint) AnglrClassificationType.ScannerPartName));
                        if (scannerSymbol == null)
                        {
                            AnglrLogger?.WarnLine ($"symbol {scannerId} does not represent scanner ID");
                            break;
                        }
                        if (initialScanner != null)
                        {
                            AnglrLogger?.WarnLine ($"Redefinition of initial scanner: {initialScanner.name} to {scannerId}");
                            break;
                        }
                        scannerSymbols.insert (initialScanner = scannerSymbol);
                    }
                    break;
                    default:
                        AnglrLogger?.WarnLine ("Unknown name-value pair for " + attribute.m__identifier_.text + " attribute :" + name_Value_Pair_.Emit (-1));
                        break;
                }
                return null;
            });
        }

        private void ReadCompilationInfo (_attribute_ attribute)
        {
            attribute.m__name_value_list_optional_.m__name_value_list_?.Iterate (null, (node, appData) =>
            {
                _name_value_pair_ name_Value_Pair_ = node.m__name_value_pair_;
                switch (name_Value_Pair_.m__identifier_.text)
                {
                    case "NameSpace":
                        lexerNameSpace = name_Value_Pair_.m__cstring_.text.Substring (1, name_Value_Pair_.m__cstring_.text.Length - 2);
                        break;
                    case "ClassName":
                        lexerClassName = name_Value_Pair_.m__cstring_.text.Substring (1, name_Value_Pair_.m__cstring_.text.Length - 2);
                        break;
                    case "Access":
                        lexerAccess = name_Value_Pair_.m__cstring_.text.Substring (1, name_Value_Pair_.m__cstring_.text.Length - 2);
                        break;
                    case "CodeDir":
                        codeDir = name_Value_Pair_.m__cstring_.text.Substring (1, name_Value_Pair_.m__cstring_.text.Length - 2);
                        break;
                    default:
                        AnglrLogger?.WarnLine ("Unknown name-value pair for " + attribute.m__identifier_.text + " attribute :" + name_Value_Pair_.Emit (-1));
                        break;
                }
                return null;
            });
        }

        public string Generate (AnglrAttributeCollection attributeCollection)
        {
            SortedSet<string> nameSpaces = new SortedSet<string> ();
            ScannerParts scannerParts = new ScannerParts ();

            for (symtab.Enumerator enumerator = scannerSymbols.enumerator; enumerator.MoveNext ();)
            {
                SymbolToken symbolToken = enumerator.Current.Value;
                ScannerPart scannerPart = attributeCollection.scannerParts [symbolToken.name];
                nameSpaces.Add (scannerPart.regexNameSpace);
                scannerParts [symbolToken.name] = scannerPart;
            }

            StringBuilder sb = new StringBuilder ();

            try
            {
                foreach (string line in g_ScannerSkeleton)
                {
                    if ((line == "") || (line [0] != '$'))
                        sb.AppendLine (line);
                    else if (line == g_UsingDeclarations)
                    {
                        foreach (string name in nameSpaces)
                            sb.AppendLine ($"using {name};");
                    }
                    else if (line == g_ScannerNamespace)
                        sb.AppendLine ($"namespace {lexerNameSpace}");
                    else if (line == g_ScannerClassDef)
                        sb.AppendLine ($"\tpublic class {lexerClassName} : LexerBase");
                    else if (line == g_ScannerRegexGetters)
                    {
                        foreach (ScannerPart part in scannerParts.Values)
                        {
                            string regexClassName = part.regexClassName;
                            string regexClassIndex = ((SymbolToken) ((AppInfo) part.part.m__identifier_.appInfo) [AppInfoType.SymbolToken]).correctName;
                            sb.AppendLine ($"\t\tpublic {regexClassName} {regexClassName} {{ get {{ return ({regexClassName}) regarray [{regexClassIndex}]; }} }}");
                        }
                    }
                    else if (line == g_ScannerClassTextReader)
                        sb.AppendLine ($"\t\tpublic {lexerClassName} (TextReader textReader, object [] info = null)");
                    else if (line == g_ScannerClassStringArray)
                        sb.AppendLine ($"\t\tpublic {lexerClassName} (string [] lines, object [] info = null)");
                    else if (line == g_ScannerClassString)
                        sb.AppendLine ($"\t\tpublic {lexerClassName} (string line, object [] info = null)");
                    else if (line == g_InitialScanner)
                        sb.AppendLine ($"\t\t\tpushScanner ({initialScanner.name}, \"\");");
                    else if (line == g_ScannerInitCode)
                    {
                        foreach (ScannerPart part in scannerParts.Values)
                        {
                            sb.AppendLine ($"\t\t\t\tnew {part.regexClassName} (this),");
                        }
                    }
                    else if (line == g_ScannerIds)
                    {
                        foreach (KeyValuePair<string, ScannerPart> p in scannerParts)
                        {
                            sb.AppendLine ($"\t\t\t{{");
                            sb.AppendLine ($"\t\t\t\t{p.Value.regexClassName}.Id,");
                            sb.AppendLine ($"\t\t\t\t{p.Key}");
                            sb.AppendLine ($"\t\t\t}},");
                        }
                    }
                    else if (line == g_MatchScanner)
                        sb.AppendLine ($"\t\t\t\tMatch match = regex.Match (currentLine);");
                    else if (line == g_ScannerCodes)
                    {
                        foreach (ScannerPart scanner_Part_ in scannerParts.Values)
                            sb.AppendLine ($"\t\tpublic const int {((SymbolToken) ((AppInfo) scanner_Part_.part.m__identifier_.appInfo) [AppInfoType.SymbolToken]).correctName} = {scanner_Part_.counter};");
                    }
                }
            }
            catch (Exception e)
            {
                throw;
            }
            return sb.ToString ();
        }
    }

    public class LexerParts : Dictionary<string, LexerPart>
    {
    }

    public class ParserPart
    {
        public anglrCompiler Compiler { get; private set; }
        public SymbolTable SymbolTable { get; private set; }
        public IAnglrLogger AnglrLogger { get; private set; }

        public _parser_part_ part { get; private set; } = null;
        public int counter { get; private set; } = -1;
        public SymbolToken parserPartSymbol { get; private set; } = null;

        public SymbolToken declarationPartSymbol { get; private set; } = null;
        public SymbolToken lexerPartSymbol { get; private set; } = null;

        public string lexerId { get; private set; } = "";
        public string declarationsId { get; private set; } = "";
        public string parserClassName { get; private set; } = "";
        public string parserNameSpace { get; private set; } = "";
        public string parserAccess { get; private set; } = "";
        public string codeDir { get; private set; } = @".\";

        public ParserPart (anglrCompiler compiler, _parser_part_ p__parser_part_, int index, SymbolToken parserPartSymbol)
        {
            Compiler = compiler;
            SymbolTable = Compiler?.symbolTable;
            AnglrLogger = Compiler?.AnglrLogger ?? new VoidAnglrLogger ();

            this.part = p__parser_part_;
            this.counter = index;
            this.parserPartSymbol = parserPartSymbol;

            parserNameSpace = $"{Path.GetFileNameWithoutExtension (Compiler?.sourceFileName).Correct()}.Parsers";
            parserClassName = parserPartSymbol.correctName;

            p__parser_part_.m__attribute_list_optional_.m__attribute_list_?.Iterate (null, (node, appData) =>
            {
                _attribute_ attribute = node.m__attribute_;
                if (attribute != null)
                    switch (attribute.m__identifier_.text)
                    {
                        case "Lexer":
                            ReadLexer (attribute);
                            break;
                        case "Declarations":
                            ReadDeclarations (attribute);
                            break;
                        case "CompilationInfo":
                            ReadCompilationInfo (attribute);
                            break;
                        default:
                            AnglrLogger?.WarnLine ("Unknown general attribute: " + attribute.m__identifier_.text);
                            break;
                    }
                return null;
            });
        }

        private void ReadLexer (_attribute_ attribute)
        {
            attribute.m__name_value_list_optional_.m__name_value_list_?.Iterate (null, (node, appData) =>
            {
                _name_value_pair_ name_Value_Pair_ = node.m__name_value_pair_;
                switch (name_Value_Pair_.m__identifier_.text)
                {
                    case "Id":
                        lexerId = name_Value_Pair_.m__cstring_.text.Substring (1, name_Value_Pair_.m__cstring_.text.Length - 2);
                        lexerPartSymbol = SymbolTable.find (new SymbolToken (lexerId, (uint) AnglrClassificationType.LexerPartName));
                        break;
                    default:
                        AnglrLogger?.WarnLine ("Unknown name-value pair for " + attribute.m__identifier_.text + " attribute :" + name_Value_Pair_.Emit (-1));
                        break;
                }
                return null;
            });
        }

        private void ReadDeclarations (_attribute_ attribute)
        {
            attribute.m__name_value_list_optional_.m__name_value_list_?.Iterate (null, (node, appData) =>
            {
                _name_value_pair_ name_Value_Pair_ = node.m__name_value_pair_;
                switch (name_Value_Pair_.m__identifier_.text)
                {
                    case "Id":
                        declarationsId = name_Value_Pair_.m__cstring_.text.Substring (1, name_Value_Pair_.m__cstring_.text.Length - 2);
                        declarationPartSymbol = SymbolTable.find (new SymbolToken (declarationsId, (uint) AnglrClassificationType.DeclarationsPartName));
                        break;
                    default:
                        AnglrLogger?.WarnLine ("Unknown name-value pair for " + attribute.m__identifier_.text + " attribute :" + name_Value_Pair_.Emit (-1));
                        break;
                }
                return null;
            });
        }

        private void ReadCompilationInfo (_attribute_ attribute)
        {
            attribute.m__name_value_list_optional_.m__name_value_list_?.Iterate (null, (node, appData) =>
            {
                _name_value_pair_ name_Value_Pair_ = node.m__name_value_pair_;
                switch (name_Value_Pair_.m__identifier_.text)
                {
                    case "NameSpace":
                        parserNameSpace = name_Value_Pair_.m__cstring_.text.Substring (1, name_Value_Pair_.m__cstring_.text.Length - 2);
                        break;
                    case "ClassName":
                        parserClassName = name_Value_Pair_.m__cstring_.text.Substring (1, name_Value_Pair_.m__cstring_.text.Length - 2);
                        break;
                    case "Access":
                        parserAccess = name_Value_Pair_.m__cstring_.text.Substring (1, name_Value_Pair_.m__cstring_.text.Length - 2);
                        break;
                    case "CodeDir":
                        codeDir = name_Value_Pair_.m__cstring_.text.Substring (1, name_Value_Pair_.m__cstring_.text.Length - 2);
                        break;
                    default:
                        AnglrLogger?.WarnLine ("Unknown name-value pair for " + attribute.m__identifier_.text + " attribute :" + name_Value_Pair_.Emit (-1));
                        break;
                }
                return null;
            });
        }
    }

    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class ParserParts : Dictionary<string, ParserPart>
    {
    }

    public class AnglrAttributeCollection : SyntaxTreeWalker
    {
        public anglrCompiler Compiler { get; private set; }
        public SymbolTable SymbolTable { get; private set; }
        public IAnglrLogger AnglrLogger { get; private set; }

        public GeneralParts generalParts { get; private set; } = new GeneralParts ();
        public DeclarationParts declarationParts { get; private set; } = new DeclarationParts ();
        public ScannerParts scannerParts { get; private set; } = new ScannerParts ();
        public LexerParts lexerParts { get; private set; } = new LexerParts ();
        public ParserParts parserParts { get; private set; } = new ParserParts ();

        private int generalPartCounter = 0;
        private int declarationPartCounter = 0;
        private int scannerPartCounter = 0;
        private int lexerPartCounter = 0;
        private int parserPartCounter = 0;

        public AnglrAttributeCollection (anglrCompiler compiler)
        {
            Compiler = compiler;
            SymbolTable = Compiler?.symbolTable;
            AnglrLogger = Compiler?.AnglrLogger ?? new VoidAnglrLogger ();

            _general_part__Event += AnglrAttributeCollection__general_part__Event;
            _declaration_part__Event += AnglrAttributeCollection__declaration_part__Event;
            _scanner_part__Event += AnglrAttributeCollection__scanner_part__Event;
            _lexer_part__Event += AnglrAttributeCollection__lexer_part__Event;
            _parser_part__Event += AnglrAttributeCollection__parser_part__Event;
        }

        private bool AnglrAttributeCollection__general_part__Event (SyntaxTreeCallbackReason reason, _general_part_.production_kind kind, _general_part_ p__general_part_)
        {
            if (reason != SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason)
                return false;
            string generalPartName = p__general_part_.m__identifier_.text;
            SymbolToken symbol = new SymbolToken (generalPartName, (uint) AnglrClassificationType.GeneralPartName);
            SymbolToken symbolRef = SymbolTable.find (symbol);
            if (symbolRef == null)
            {
                AnglrLogger?.ErrorLine ($"IMPLEMENTATION ERROR: general part name not found: {generalPartName}");
                return false;
            }
            generalParts [generalPartName] = new GeneralPart (Compiler, p__general_part_, generalPartCounter++, symbolRef);
            return false;
        }

        private bool AnglrAttributeCollection__declaration_part__Event (SyntaxTreeCallbackReason reason, _declaration_part_.production_kind kind, _declaration_part_ p__declaration_part_)
        {
            if (reason != SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason)
                return false;
            string declarationPartName = p__declaration_part_.m__identifier_.text;
            SymbolToken symbol = new SymbolToken (declarationPartName, (uint) AnglrClassificationType.DeclarationsPartName);
            SymbolToken symbolRef = SymbolTable.find (symbol);
            if (symbolRef == null)
            {
                AnglrLogger?.ErrorLine ($"IMPLEMENTATION ERROR: declaration part name not found: {declarationPartName}");
                return false;
            }
            declarationParts [declarationPartName] = new DeclarationsPart (Compiler, p__declaration_part_, declarationPartCounter++, symbolRef);
            return false;
        }

        private bool AnglrAttributeCollection__scanner_part__Event (SyntaxTreeCallbackReason reason, _scanner_part_.production_kind kind, _scanner_part_ p__scanner_part_)
        {
            if (reason != SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason)
                return false;
            string scannerPartName = p__scanner_part_.m__identifier_.text;
            SymbolToken symbol = new SymbolToken (scannerPartName, (uint) AnglrClassificationType.ScannerPartName);
            SymbolToken symbolRef = SymbolTable.find (symbol);
            if (symbolRef == null)
            {
                AnglrLogger?.ErrorLine ($"IMPLEMENTATION ERROR: scanner part name not found: {scannerPartName}");
                return false;
            }
            scannerParts [scannerPartName] = new ScannerPart (Compiler, p__scanner_part_, scannerPartCounter++, symbolRef);
            return false;
        }

        private bool AnglrAttributeCollection__lexer_part__Event (SyntaxTreeCallbackReason reason, _lexer_part_.production_kind kind, _lexer_part_ p__lexer_part_)
        {
            if (reason != SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason)
                return false;
            string lexerPartName = p__lexer_part_.m__identifier_.text;
            SymbolToken symbol = new SymbolToken (lexerPartName, (uint) AnglrClassificationType.LexerPartName);
            SymbolToken symbolRef = SymbolTable.find (symbol);
            if (symbolRef == null)
            {
                AnglrLogger?.ErrorLine ($"IMPLEMENTATION ERROR: lexer part name not found: {lexerPartName}");
                return false;
            }
            lexerParts [lexerPartName] = new LexerPart (Compiler, p__lexer_part_, lexerPartCounter++, symbolRef);
            return false;
        }

        private bool AnglrAttributeCollection__parser_part__Event (SyntaxTreeCallbackReason reason, _parser_part_.production_kind kind, _parser_part_ p__parser_part_)
        {
            if (reason != SyntaxTreeCallbackReason.TraversalEpilogueCallbackReason)
                return false;
            string parserPartName = p__parser_part_.m__identifier_.text;
            SymbolToken symbol = new SymbolToken (parserPartName, (uint) AnglrClassificationType.ParserPartName);
            SymbolToken symbolRef = SymbolTable.find (symbol);
            if (symbolRef == null)
            {
                AnglrLogger?.ErrorLine ($"IMPLEMENTATION ERROR: parser part name not found: {parserPartName}");
                return false;
            }
            parserParts [parserPartName] = new ParserPart (Compiler, p__parser_part_, parserPartCounter++, symbolRef);
            return false;
        }
    }
}
