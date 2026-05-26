using Anglr.Parser;
using AnglrJsonRpcMethods;
using AnglrLogLibrary;
using Microsoft.VisualStudio.LanguageServer.Protocol;
using Microsoft.VisualStudio.Shell;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace AnglrLangExtension
{
    using static System.Windows.Forms.AxHost;
    using UserControl = System.Windows.Controls.UserControl;
    /// <summary>
    /// Interaction logic for AnglrLangWindowControl.
    /// </summary>
    public static class DependencyObjectExtensions
    {
        public static T GetParentOfType<T> (this DependencyObject element) where T : DependencyObject
        {
            if (element == null)
                return null;

            DependencyObject parent = VisualTreeHelper.GetParent (element);
            while (parent != null && !(parent is T))
            {
                parent = VisualTreeHelper.GetParent (parent);
            }

            return parent as T;
        }
    }

    public class SRConflictList : List<(int, AnglrGetParserStateReductionsData)> { }
    public class SRConflictSet : Dictionary<string, SRConflictList> { }
    public class RRConflictList : List<(AnglrGetParserStateReductionsData, AnglrGetParserStateReductionsData)> { }
    public class RRConflictSet : Dictionary<string, RRConflictList> { }
    public class TreeViewItemSet : Dictionary<int, AnglrStateItem> { }
    public class ViablePrefix : List<AnglrStateItem>
    {
        public string GetPath ()
        {
            string str = "";
            foreach (var part in this)
                str += $"/{part.Name}";
            return str;
        }

        public string GetHtmlPath ()
        {
            string str = "";
            foreach (var part in this)
            {
                string htmlClass = part.IsShift ? "terminal-symbol" : "non-terminal-symbol";
                str += $"/<span class={htmlClass}>{WebUtility.HtmlEncode ((part.State > 0) ? part.Token : "$")}</span>";
            }
            return str;
        }
    }

    public class AnglrLangDictionary : Dictionary<int, (AnglrLangItem, AnglrStateItem, AnglrGetParserSyntaxRulesResult)>
    {
        private static AnglrLangDictionary AnglrLangRepo = new AnglrLangDictionary ();
        public static bool HasItem (int id) => AnglrLangRepo.TryGetValue (id, out _);
        public static (AnglrLangItem, AnglrStateItem, AnglrGetParserSyntaxRulesResult) AddItem (int id, (AnglrLangItem, AnglrStateItem, AnglrGetParserSyntaxRulesResult) item) => HasItem (id) ? default : AnglrLangRepo [id] = item;
        public static (AnglrLangItem, AnglrStateItem, AnglrGetParserSyntaxRulesResult) GetItem (int id) => AnglrLangRepo.TryGetValue (id, out var item) ? item : default;
    }

    public partial class AnglrLangWindowControl : UserControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AnglrLangWindowControl"/> class.
        /// </summary>

        public static readonly string anglrHtmlPrologue =
            "<html><head><style>\r\n" +
            ".anglr-doc-index {\r\n" +
            " font-size: smaller;\r\n" +
            "}\r\n" +
            "\r\n" +
            ".anglr-page-index {\r\n" +
            " font-size: small;\r\n" +
            "}\r\n" +
            "\r\n" +
            ".anglr-content {\r\n" +
            " background-image: url('../images/anglr-parser-part.png');\r\n" +
            " background-repeat:no-repeat;\r\n" +
            " background-size:cover;\r\n" +
            " height:100vh;\r\n" +
            "}\r\n" +
            "\r\n" +
            ".part-token {\r\n" +
            " color: red;\r\n" +
            " font-weight: bold;\r\n" +
            "}\r\n" +
            "\r\n" +
            ".part-name {\r\n" +
            " color: green;\r\n" +
            "}\r\n" +
            "\r\n" +
            ".part-brace {\r\n" +
            " color: black;\r\n" +
            " font-weight: bold;\r\n" +
            "}\r\n" +
            "\r\n" +
            ".regex-ref {\r\n" +
            " color: darkred;\r\n" +
            " font-weight: bold;\r\n" +
            "}\r\n" +
            "\r\n" +
            ".action-token {\r\n" +
            " color: red;\r\n" +
            "}\r\n" +
            "\r\n" +
            ".action-identifier {\r\n" +
            " color: green;\r\n" +
            " font-weight: bold;\r\n" +
            "}\r\n" +
            "\r\n" +
            ".attribute-brace {\r\n" +
            " color: black;\r\n" +
            " font-weight: bold;\r\n" +
            "}\r\n" +
            "\r\n" +
            ".attribute-name {\r\n" +
            " color: blue;\r\n" +
            "}\r\n" +
            "\r\n" +
            ".attribute-val-name {\r\n" +
            " color: orange;\r\n" +
            "}\r\n" +
            "\r\n" +
            ".attribute-val-value {\r\n" +
            " color: black;\r\n" +
            "}\r\n" +
            "\r\n" +
            ".reserved-word {\r\n" +
            " color: red;\r\n" +
            " font-weight: bold;\r\n" +
            "}\r\n" +
            "\r\n" +
            ".regex-name {\r\n" +
            " color: green;\r\n" +
            "}\r\n" +
            "\r\n" +
            ".regex-def {\r\n" +
            " color: blue;\r\n" +
            "}\r\n" +
            "\r\n" +
            ".def-brace {\r\n" +
            " color: black;\r\n" +
            " font-weight: bold;\r\n" +
            "}\r\n" +
            "\r\n" +
            ".def-separator {\r\n" +
            " color: black;\r\n" +
            " font-weight: bold;\r\n" +
            "}\r\n" +
            "\r\n" +
            ".token-name {\r\n" +
            " color: green;\r\n" +
            "}\r\n" +
            "\r\n" +
            ".token-value {\r\n" +
            " color: blue;\r\n" +
            "}\r\n" +
            "\r\n" +
            ".rule-name {\r\n" +
            " color: red;\r\n" +
            " font-weight: bold;\r\n" +
            "}\r\n" +
            "\r\n" +
            ".rule-ref {\r\n" +
            " color: indianred;\r\n" +
            " font-weight: bold;\r\n" +
            "}\r\n" +
            "\r\n" +
            ".group-name {\r\n" +
            " color: mediumblue;\r\n" +
            " font-weight: bold;\r\n" +
            "}\r\n" +
            "\r\n" +
            ".production-name {\r\n" +
            " color: blue;\r\n" +
            "}\r\n" +
            "\r\n" +
            ".marker-name {\r\n" +
            " color: lightblue;\r\n" +
            "}\r\n" +
            "\r\n" +
            ".string-name {\r\n" +
            " color: darkred;\r\n" +
            "}\r\n" +
            "\r\n" +
            ".identifier-name {\r\n" +
            " color: blue;\r\n" +
            "}\r\n" +
            "</style></head><body>";
        public static readonly string anglrHtmlEpilogue = "</body></html>";

        public static readonly string [] anglrLangWindowIcons =
        {
            "Resources/CSApplication.png",
            "Resources/CSAssembyInfoFile.png",
            "Resources/CSBDCModel.png",
            "Resources/CSBlankApplication.png",
            "Resources/CSBlankFile.png",
            "Resources/CSBlankPhone.png",
            "Resources/CSBlankWebSite.png",
            "Resources/CSClassCollection.png",
            "Resources/CSClassFile.png",
            "Resources/CSClassLibrary.png",
            "Resources/CSCloudBusinessApplication.png",
            "Resources/CSCodeTest.png",
            "Resources/CSColumn.png",
            "Resources/CSConsole.png",
            "Resources/CSConsoleTest.png",
            "Resources/CSContentType.png",
            "Resources/CSDeploymentModule.png",
            "Resources/CSDeviceTest.png",
            "Resources/CSDynamicWebSite.png",
            "Resources/CSEventReceiver.png",
            "Resources/CSExtension.png",
            "Resources/CSFile.png",
            "Resources/CSFileNode.png",
            "Resources/CSFixedLayoutApplication.png",
            "Resources/CSGridApplication.png",
            "Resources/CSHubApplication.png",
            "Resources/CSInteractiveWindow.png",
            "Resources/CSInterface.png",
            "Resources/CSInterfaceCollection.png",
            "Resources/CSLibrary.png",
            "Resources/CSLightswitch.png",
            "Resources/CSLightswitchLibrary.png",
            "Resources/CSListDefinition.png",
            "Resources/CSMenuItemCustomAction.png",
            "Resources/CSNavigationApplication.png",
            "Resources/CSPackage.png",
            "Resources/CSPhone.png",
            "Resources/CSPhoneHub.png",
            "Resources/CSPhoneLibrary.png",
            "Resources/CSPhonePivot.png",
            "Resources/CSPhoneTest.png",
            "Resources/CSPhoneWebApplication.png",
            "Resources/CSPhoneWebSite.png",
            "Resources/CSProjectNode.png",
            "Resources/CSRazorFile.png",
            "Resources/CSReport.png",
            "Resources/CSRibbonCustomAction.png",
            "Resources/CSSClass.png",
            "Resources/CSSElement.png",
            "Resources/CSSLink.png",
            "Resources/CSSQLLibrary.png",
            "Resources/CSSStyleError.png",
            "Resources/CSServiceBusWorker.png",
            "Resources/CSSharedProject.png",
            "Resources/CSSilverlight.png",
            "Resources/CSSilverlightLibrary.png",
            "Resources/CSSilverlightPhone.png",
            "Resources/CSSilverlightTest.png",
            "Resources/CSSilverlightWebSite.png",
            "Resources/CSSiteDefinition.png",
            "Resources/CSSourceFile.png",
            "Resources/CSSplitApplication.png",
            "Resources/CSTablet.png",
            "Resources/CSTestApplication.png",
            "Resources/NN.png",
            "Resources/CN.png",
            "Resources/NC.png",
            "Resources/CC.png",
        };

        public static string [] [] anglrNodeTypes =
        {
            new string [] { "", "" },
            new string [] { "list of attributes", "" },
            new string [] { "attribute", "" },
            new string [] { "", "" },
            new string [] { "attribute named value", "" },
            new string [] { "anglr file", "" },
            new string [] { "", "" },
            new string [] { "", "" },
            new string [] { "general part of anglr file", "" },
            new string [] { "declaration part of anglr file", "" },
            new string [] { "", "" },
            new string [] { "definition", "" },
            new string [] { "", "" },
            new string [] { "single terminal definition", "" },
            new string [] { "single regular expression definition", "" },
            new string [] { "block of terminals", "" },
            new string [] { "block of regular expressions", "" },
            new string [] { "terminal definition", "" },
            new string [] { "regular expression definition", "" },
            new string [] { "", "" },
            new string [] { "", "" },
            new string [] { "", "" },
            new string [] { "", "" },
            new string [] { "scanner part of anglr file", "" },
            new string [] { "", "" },
            new string [] { "regular expression usage", "" },
            new string [] { "", "" },
            new string [] { "", "" },
            new string [] { "skip action", "" },
            new string [] { "terminal action", "" },
            new string [] { "event action", "" },
            new string [] { "push action", "" },
            new string [] { "pop action", "" },
            new string [] { "lexer part of anglr file", "" },
            new string [] { "parser part of anglr file", "" },
            new string [] { "", "" },
            new string [] { "syntax rule", "list of syntax rules" },
            new string [] { "nested syntax rule", "" },
            new string [] { "nested syntax rule name", "" },
            new string [] { "", "" },
            new string [] { "production", "" },
            new string [] { "production name", "" },
            new string [] { "", "" },
            new string [] { "priority specification", "" },
            new string [] { "associativity specification", "" },
            new string [] { "name list", "" },
            new string [] { "marker list", "" },
            new string [] { "marker", "" },
            new string [] { "", "" },
            new string [] { "name", "" },
            new string [] { "cardinality and delimiter", "" },
            new string [] { "cardinality", "" },
            new string [] { "delimiter", "" },
        };

        private IAnglrLogger logger;
        private IAnglrLangService anglrLangService = null;
        private object rightButtonSource = null;
        private AsyncPackage _package = null;

        public AsyncPackage package
        {
            get => _package;
            set
            {
                _package = value;
                if (anglrLangService == null)
                {
                    anglrDebugWindow.package = _package;
                    anglrLangService = ThreadHelper.JoinableTaskFactory.Run (() => value.GetServiceAsync (typeof (SAnglrLangService))) as IAnglrLangService;
                    logger = anglrLangService?.AnglrLogger ?? new VoidAnglrLogger ();
                    anglrLangService.RegisterAnglrFileEventHandler (AnglrProjectItemEventHandlerAsync);
                    ThreadHelper.JoinableTaskFactory.Run (() => anglrLangService.GetOpenAnglrFilesAsync ());
                }
            }
        }

        ViablePrefix viablePrefix = null;

        public void Dispose () => anglrDebugWindow?.Dispose ();

        public async System.Threading.Tasks.Task AnglrProjectItemEventHandlerAsync (string fileName, AnglrItemOperation operation)
        {
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync ();
            switch (operation)
            {
                case AnglrItemOperation.None:
                    break;
                case AnglrItemOperation.Get:
                    AddAnglrItem (fileName);
                    break;
                case AnglrItemOperation.Add:
                    AddAnglrItem (fileName);
                    break;
                case AnglrItemOperation.Remove:
                    RemoveSyntaxTreeElement (fileName, $"{(int) ProductionID.__anglr_file__ID}-1");
                    RemoveParserStatesElement (fileName, $"{(int) ProductionID.__anglr_file__ID}-1");
                    RemoveDebugPanelElement (fileName, $"{(int) ProductionID.__anglr_file__ID}-1");
                    break;
            }
        }

        public AnglrLangWindowControl ()
        {
            this.InitializeComponent ();
            anglrSyntaxTree.Items.IsLiveSorting = true;
            _ = AnglrLangExtensionPackage.Instance.RetrieveAnglrFilesAsync ();
        }

        public void AddAnglrItem (string name)
        {
            try
            {
                AnglrGetParserMagicNumberResult anglrGetParserMagicNumberResult = anglrLangService.InvokeGetParserMagicNumber
                (
                    new AnglrGetParserMagicNumberParams ()
                    {
                        TextDocument = new TextDocumentIdentifier ()
                        {
                            Uri = new Uri (name)
                        }
                    }
                );
                int? magicNr = anglrGetParserMagicNumberResult?.MagicNumber;

                AnglrGetParserSyntaxRulesResult anglrGetParserSyntaxRulesResult = anglrLangService.InvokeGetParserSyntaxRules
                (
                    new AnglrGetParserSyntaxRulesParams ()
                    {
                        TextDocument = new TextDocumentIdentifier ()
                        {
                            Uri = new Uri (name)
                        }
                    }
                );

                AnglrLangItem anglrLangItem = new AnglrLangItem (null, (int) ProductionID.__anglr_file__ID, name, $"{(int) ProductionID.__anglr_file__ID}-1");
                AnglrStateItem anglrStateItem = new AnglrStateItem (null, true, name, 0, 0, new TreeViewItemSet ());
                AnglrDetailViewItemWindow anglrDetailViewItemWindow = new AnglrDetailViewItemWindow ();
                anglrDetailViewItemWindow.FileName = name;
                anglrDetailViewItemWindow.AnglrGetParserSyntaxRuleDatas = new AnglrGetParserSyntaxRuleDataCollection (anglrGetParserSyntaxRulesResult?.SyntaxRuleList);

                if (magicNr.HasValue)
                    AnglrLangDictionary.AddItem (magicNr.Value, (anglrLangItem, anglrStateItem, anglrGetParserSyntaxRulesResult));

                anglrSyntaxTree.Items.Add (anglrLangItem);
                anglrParserStates.Items.Add (anglrStateItem);
                anglrDetailsWindow.Items.Add (anglrDetailViewItemWindow);
            }
            catch (Exception ex)
            {
                while (ex != null)
                {
                    logger?.DebugLine ($"<AddAnglrItem> exception: {ex.Message}");
                    logger?.DebugLine ($"<AddAnglrItem> stack trace: {ex.StackTrace}");
                    ex = ex.InnerException;
                }
            }
        }

        public void RemoveSyntaxTreeElement (string name, string itemId)
        {
            AnglrLangItem anglrLangItem = new AnglrLangItem (null, (int) ProductionID.__anglr_file__ID, name, itemId);
            List<AnglrLangItem> anglrLangItems = new List<AnglrLangItem> ();
            foreach (var item in anglrSyntaxTree.Items)
            {
                if (anglrLangItem.Equal ((AnglrLangItem) item))
                    anglrLangItems.Add ((AnglrLangItem) item);
            }
            foreach (var item in anglrLangItems)
            {
                var tvi = anglrSyntaxTree.ItemContainerGenerator.ContainerFromItem (item) as TreeViewItem;
                if (tvi != null)
                    tvi.IsSelected = false;
                anglrSyntaxTree.Items.Remove (item);
            }
        }

        public void RemoveParserStatesElement (string name, string itemId)
        {
            try
            {
                logger?.DebugLine ($"<RemoveParserStatesElement>: trying to remove states tree: {name}");
                AnglrStateItem anglrStateItem = new AnglrStateItem (null, false, name, 0, 0, new TreeViewItemSet ());
                List<AnglrStateItem> anglrLangItems = new List<AnglrStateItem> ();
                foreach (var item in anglrParserStates.Items)
                {
                    if (anglrStateItem.Equal ((AnglrStateItem) item))
                        anglrLangItems.Add ((AnglrStateItem) item);
                }
                logger?.DebugLine ($"<RemoveParserStatesElement>: found {anglrParserStates.Items.Count} items");
                foreach (var item in anglrLangItems)
                {
                    var tvi = anglrParserStates.ItemContainerGenerator.ContainerFromItem (item) as TreeViewItem;
                    if (tvi != null)
                        tvi.IsSelected = false;
                    logger?.DebugLine ($"<RemoveParserStatesElement>: remove {item.Name}");
                    anglrParserStates.Items.Remove (item);
                }
            }
            catch (Exception ex)
            {
                for (; ex != null; ex = ex.InnerException)
                {
                    logger?.DebugLine ($"<RemoveParserStatesElement>: exception: {ex.Message}");
                    logger?.DebugLine ($"<RemoveParserStatesElement>: trace: {ex.StackTrace}");
                }
            }
        }

        public void RemoveDebugPanelElement (string name, string itemId)
        {
        }

        private void anglrSyntaxTree_SelectedItemChanged (object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            try
            {
                AnglrLangItem selectedViewItem = e.NewValue as AnglrLangItem;
                AnglrLangItem treeViewItem = e.NewValue as AnglrLangItem;
                string itemId = (string) treeViewItem.Id;
                while (treeViewItem.Parent != null)
                    treeViewItem = treeViewItem.Parent;

                AnglrGetGetHierarchyItemParams anglrGetGetHierarchyItemParams = new AnglrGetGetHierarchyItemParams ()
                {
                    ItemId = itemId,
                    TextDocument = new TextDocumentIdentifier ()
                    {
                        Uri = new System.Uri ((string) treeViewItem.Name)
                    }
                };

                AnglrGetGetHierarchyItemResult anglrGetGetHierarchyItemResult = anglrLangService.InvokeGetHierarchy (anglrGetGetHierarchyItemParams);
                //anglrLangService.Log ($"SELECTED ITEM CHANGED\n{JsonConvert.SerializeObject (anglrGetGetHierarchyItemResult)}");

                string nodeType = AnglrLangWindowControl.anglrNodeTypes
                    [anglrGetGetHierarchyItemResult.NodeCategory - (int) ProductionID.__anglr_file_fragment__ID]
                    [anglrGetGetHierarchyItemResult.NodeSubCategory];
                nodeName.Content = $"{nodeType}: {anglrGetGetHierarchyItemResult.NodeName}";
                webBrowser.NavigateToString ($"{anglrHtmlPrologue}{anglrGetGetHierarchyItemResult.HtmlText}{anglrHtmlEpilogue}");

                foreach (var item in anglrGetGetHierarchyItemResult.Items)
                {
                    bool found = false;
                    foreach (AnglrLangItem anglrLangItem in selectedViewItem.anglrLangItems)
                    {
                        if (anglrLangItem.Id == item.ItemId)
                        {
                            found = true;
                            break;
                        }
                    }
                    if (found)
                        continue;
                    selectedViewItem.anglrLangItems.Add (new AnglrLangItem (selectedViewItem, (int) item.Specie, item.ItemName, item.ItemId));
                }
            }
            catch (Exception exc)
            {
                logger?.DebugLine ($"anglrHierarchy_SelectedItemChanged throws exception:");
                logger?.DebugLine ($"\t{exc.Message}");
                logger?.DebugLine ($"\t{exc.StackTrace}");
            }
        }

        private void Edit_MenuItem_Click (object sender, RoutedEventArgs e)
        {
            if (((rightButtonSource == null) || !(rightButtonSource is AnglrLangItem)) && ((rightButtonSource = anglrSyntaxTree.SelectedItem) == null))
                return;
            try
            {
                AnglrLangItem anglrLangItem = (AnglrLangItem) rightButtonSource;
                string itemId = anglrLangItem.Id;
                anglrLangItem = anglrLangItem.Root;
                AnglrGetItemNavigationInfoRequest request = new AnglrGetItemNavigationInfoRequest ()
                {
                    ItemId = itemId,
                    TextDocument = new TextDocumentIdentifier ()
                    {
                        Uri = new System.Uri ((string) anglrLangItem.Name)
                    }
                };
                AnglrGetItemNavigationInfoResponse anglrGetItemNavigationInfoResponse = anglrLangService.InvokeGetItemNavigationInfo (request);
                anglrLangService.NavigateAnglrFile (anglrLangItem.Name, anglrGetItemNavigationInfoResponse.ItemLineno, anglrGetItemNavigationInfoResponse.ItemColumn + 1);
            }
            catch (Exception exc)
            {
                logger?.DebugLine ($"Edit_MenuItem_Click throws exception:");
                logger?.DebugLine ($"\t{exc.Message}");
                logger?.DebugLine ($"\t{exc.StackTrace}");
            }
            rightButtonSource = null;
        }

        private void Compile_MenuItem_Click (object sender, RoutedEventArgs e)
        {
            if (((rightButtonSource == null) || !(rightButtonSource is AnglrLangItem)) && ((rightButtonSource = anglrSyntaxTree.SelectedItem) == null))
                return;
            try
            {
                AnglrLangItem anglrLangItem = (AnglrLangItem) rightButtonSource;
                string itemId = anglrLangItem.Id;
                anglrLangItem = anglrLangItem.Root;
                AnglrGetCompileFragmentRequest request = new AnglrGetCompileFragmentRequest ()
                {
                    ItemId = itemId,
                    TextDocument = new TextDocumentIdentifier ()
                    {
                        Uri = new System.Uri ((string) anglrLangItem.Name)
                    }
                };
                AnglrGetCompileFragmentResponse anglrGetItemNavigationInfoResponse = anglrLangService.InvokeGetCompileFragment (request);
                logger?.InfoLine ($"Compile fragment returns: {anglrGetItemNavigationInfoResponse.Result}");
                logger?.InfoLine ($"{anglrGetItemNavigationInfoResponse.Fragment}");
            }
            catch (Exception exc)
            {
                logger?.DebugLine ($"Compile_MenuItem_Click throws exception:");
                logger?.DebugLine ($"\t{exc.Message}");
                logger?.DebugLine ($"\t{exc.StackTrace}");
            }
            rightButtonSource = null;
        }

        private void Debug_MenuItem_Click (object sender, RoutedEventArgs e)
        {
            if (((rightButtonSource == null) || !(rightButtonSource is AnglrLangItem)) && ((rightButtonSource = anglrSyntaxTree.SelectedItem) == null))
                return;
            UserControl userControl = null;
            try
            {
                AnglrLangItem anglrLangItem = (AnglrLangItem) rightButtonSource;
                AnglrDetailsWindow window = null;
                switch ((ProductionID) anglrLangItem.Specie)
                {
                    case ProductionID.__anglr_file_fragment__ID:
                        break;
                    case ProductionID.__attribute_list__ID:
                        break;
                    case ProductionID.__attribute__ID:
                        break;
                    case ProductionID.__name_value_list__ID:
                        break;
                    case ProductionID.__name_value_pair__ID:
                        break;
                    case ProductionID.__anglr_file__ID:
                        break;
                    case ProductionID.__anglr_file_part_list__ID:
                        break;
                    case ProductionID.__anglr_file_part__ID:
                        break;
                    case ProductionID.__general_part__ID:
                        break;
                    case ProductionID.__declaration_part__ID:
                        break;
                    case ProductionID.__anglr_definition_list__ID:
                        break;
                    case ProductionID.__anglr_definition_with_attribute__ID:
                        break;
                    case ProductionID.__anglr_definition__ID:
                        break;
                    case ProductionID.__single_terminal_definition__ID:
                        break;
                    case ProductionID.__single_regex_definition__ID:
                        break;
                    case ProductionID.__block_of_terminal_definitions__ID:
                        break;
                    case ProductionID.__block_of_regex_definitions__ID:
                        break;
                    case ProductionID.__terminal_definition__ID:
                        break;
                    case ProductionID.__regex_definition__ID:
                        break;
                    case ProductionID.__block_terminal_definitions__ID:
                        break;
                    case ProductionID.__block_terminal_definition__ID:
                        break;
                    case ProductionID.__block_regex_definitions__ID:
                        break;
                    case ProductionID.__block_regex_definition__ID:
                        break;
                    case ProductionID.__scanner_part__ID:
                        break;
                    case ProductionID.__regular_expression_list__ID:
                        break;
                    case ProductionID.__regular_expression_usage__ID:
                        break;
                    case ProductionID.__actions__ID:
                        break;
                    case ProductionID.__action__ID:
                        break;
                    case ProductionID.__skip_action__ID:
                        break;
                    case ProductionID.__terminal_action__ID:
                        break;
                    case ProductionID.__event_action__ID:
                        break;
                    case ProductionID.__push_action__ID:
                        break;
                    case ProductionID.__pop_action__ID:
                        break;
                    case ProductionID.__lexer_part__ID:
                        break;
                    case ProductionID.__parser_part__ID:
                        break;
                    case ProductionID.__anglr_syntax_rule_list__ID:
                        break;
                    case ProductionID.__anglr_syntax_rule__ID:
                    case ProductionID.__anglr_nested_rule__ID:
                    {
                        userControl = new AnglrSyntaxRuleDetailsWindow (package, anglrLangService, anglrLangItem);
                    }
                    break;
                    case ProductionID.__anglr_syntax_production_list_name__ID:
                        break;
                    case ProductionID.__anglr_syntax_production_list__ID:
                        break;
                    case ProductionID.__anglr_syntax_production__ID:
                        break;
                    case ProductionID.__production_name__ID:
                        break;
                    case ProductionID.__priority_assoc_specification__ID:
                        break;
                    case ProductionID.__priority_specification__ID:
                        break;
                    case ProductionID.__associativity_specification__ID:
                        break;
                    case ProductionID.__name_list__ID:
                        break;
                    case ProductionID.__marker_list__ID:
                        break;
                    case ProductionID.__marker__ID:
                        break;
                    case ProductionID.__g_name__ID:
                        break;
                    case ProductionID.__name__ID:
                        break;
                    case ProductionID.__cardinality_delimiter__ID:
                        break;
                    case ProductionID.__cardinality__ID:
                        break;
                    case ProductionID.__delimiter__ID:
                        break;
                    case ProductionID.__attribute_list_optional__ID:
                        break;
                    case ProductionID.__name_value_list_optional__ID:
                        break;
                    case ProductionID.__anglr_file_part_list_optional__ID:
                        break;
                    case ProductionID.__anglr_definition_list_optional__ID:
                        break;
                    case ProductionID.__block_terminal_definitions_optional__ID:
                        break;
                    case ProductionID.__block_regex_definitions_optional__ID:
                        break;
                    case ProductionID.__regular_expression_list_optional__ID:
                        break;
                    case ProductionID.__actions_optional__ID:
                        break;
                    case ProductionID.__anglr_syntax_rule_list_optional__ID:
                        break;
                    case ProductionID.__anglr_syntax_production_list_name_optional__ID:
                        break;
                    case ProductionID.__production_name_optional__ID:
                        break;
                    case ProductionID.__priority_assoc_specification_optional__ID:
                        break;
                    case ProductionID.__marker_list_optional__ID:
                        break;
                    case ProductionID.__delimiter_optional__ID:
                        break;
                    case ProductionID.__cstring_optional__ID:
                        break;
                    case ProductionID.__number_optional__ID:
                        break;
                    default:
                        break;
                }
                if (userControl == null)
                    return;
                for (int i = 0; ; i++)
                {
                    if (this.package.FindToolWindow (typeof (AnglrDetailsWindow), i, false) != null)
                        continue;
                    window = package.JoinableTaskFactory.Run (() =>
                        package.ShowToolWindowAsync (typeof (AnglrDetailsWindow), i, true, package.DisposalToken)) as AnglrDetailsWindow;
                    if ((null == window) || (null == window.Frame))
                    {
                        throw new NotSupportedException ("Cannot create details window");
                    }
                    window.Caption = anglrLangItem.Name;
                    break;
                }
                if (userControl != null)
                    ((AnglrDetailsWindowControl) window.Content).Content = userControl;
            }
            catch (Exception exc)
            {
                logger?.DebugLine ($"Debug_MenuItem_Click throws exception:");
                logger?.DebugLine ($"\t{exc.Message}");
                logger?.DebugLine ($"\t{exc.StackTrace}");
            }
            rightButtonSource = null;
        }

        private void anglrSyntaxTree_MouseRightButtonDown (object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            rightButtonSource = e.Source;
        }

        private void anglrSyntaxTree_KeyDown (object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key != System.Windows.Input.Key.Enter)
                return;

            if (anglrSyntaxTree.SelectedItem == null)
                return;

            try
            {
                AnglrLangItem anglrLangItem = anglrSyntaxTree.SelectedItem as AnglrLangItem;
                string itemId = anglrLangItem.Id;
                while (anglrLangItem.Parent != null)
                    anglrLangItem = anglrLangItem.Parent;
                AnglrGetItemNavigationInfoRequest request = new AnglrGetItemNavigationInfoRequest ()
                {
                    ItemId = itemId,
                    TextDocument = new TextDocumentIdentifier ()
                    {
                        Uri = new System.Uri ((string) anglrLangItem.Name)
                    }
                };
                AnglrGetItemNavigationInfoResponse anglrGetItemNavigationInfoResponse = anglrLangService.InvokeGetItemNavigationInfo (request);
                anglrLangService.NavigateAnglrFile (anglrLangItem.Name, anglrGetItemNavigationInfoResponse.ItemLineno, anglrGetItemNavigationInfoResponse.ItemColumn + 1);
            }
            catch (Exception exc)
            {
                logger?.DebugLine ($"Edit_MenuItem_Click throws exception:");
                logger?.DebugLine ($"\t{exc.Message}");
                logger?.DebugLine ($"\t{exc.StackTrace}");
            }
            rightButtonSource = null;
        }

        private void InspectPath (TreeView treeView, ViablePrefix path)
        {
            ItemsControl current = treeView;

            foreach (var part in path)
            {
                ItemContainerGenerator itemContainer = current.ItemContainerGenerator;
                TreeViewItem treeViewItem = null;

                logger?.DebugLine ($"\ttry path {part.Name}");
                foreach (var item in current.Items)
                {
                    AnglrStateItem anglrStateItem = item as AnglrStateItem;
                    if (anglrStateItem != null)
                        logger?.DebugLine ($"\t\tcheck item {anglrStateItem.State}");
                    var tvi = itemContainer.ContainerFromItem (item);
                    if (tvi == null)
                    {
                        logger?.DebugLine ($"\t\titem {anglrStateItem.State} has no container for {part.Name}");
                        continue;
                    }
                    if (!(tvi is TreeViewItem))
                    {
                        logger?.DebugLine ($"\t\titem {anglrStateItem.State} has container for {part.Name}, but is not TreeViewItem");
                        continue;
                    }
                    treeViewItem = tvi as TreeViewItem;
                    logger?.DebugLine ($"\t\titem {anglrStateItem.State} has TreeViewItem");
                    break;
                }

                if ((current = treeViewItem) == null)
                {
                    logger?.DebugLine ($"\tpath {part.Name} has no items with TreeViewItem");
                    return;
                }
            }

            logger?.DebugLine ($"\t\tpath {path.GetPath ()} is correct");
            return;
        }

        private void anglrParserStates_SelectedItemChanged (object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            try
            {
                AnglrStateItem selectedViewItem = e.NewValue as AnglrStateItem;
                AnglrStateItem anglrStateItem = selectedViewItem.TreeViewItemSet [selectedViewItem.State];

                if (!anglrStateItem.IsLoaded)
                {
                    AnglrGetParserStateItemParams anglrGetParserStateItemParams = new AnglrGetParserStateItemParams ()
                    {
                        StateNr = anglrStateItem.State,
                        TextDocument = new TextDocumentIdentifier ()
                        {
                            Uri = new System.Uri (anglrStateItem.RootItem.Name)
                        }
                    };
                    AnglrGetParserStateItemResult anglrGetParserStateItemResult = anglrLangService.InvokeGetParserState (anglrGetParserStateItemParams);
                    anglrStateItem.DefineText (anglrGetParserStateItemResult);

                    foreach (var item in anglrGetParserStateItemResult.ShiftSet)
                        anglrStateItem.anglrStateItems.Add
                            (
                                new AnglrStateItem
                                (
                                    anglrStateItem,
                                    true,
                                    item.Token,
                                    item.State,
                                    item.Conflicts,
                                    anglrStateItem.TreeViewItemSet
                                )
                            );
                    foreach (var item in anglrGetParserStateItemResult.GotoSet)
                        anglrStateItem.anglrStateItems.Add
                            (
                                new AnglrStateItem
                                (
                                    anglrStateItem,
                                    false,
                                    item.Token,
                                    item.State,
                                    item.Conflicts,
                                    anglrStateItem.TreeViewItemSet
                                )
                            );
                }

                if (selectedViewItem != anglrStateItem)
                {
                    if (!selectedViewItem.IsLoaded)
                    {
                        selectedViewItem.Copy (anglrStateItem);
                    }
                }

                stateTab.Tag = selectedViewItem;
                stateName.Content = $"State {selectedViewItem.State}";
                stateBrowser.NavigateToString (selectedViewItem.Text);
                //anglrLangService.Log ($"HTML START");
                //anglrLangService.Log (selectedViewItem.Text);
                //anglrLangService.Log ($"HTML STOP");
            }
            catch (Exception exc)
            {
                logger?.DebugLine ($"anglrParserStates_SelectedItemChanged throws exception:");
                logger?.DebugLine ($"\t{exc.Message}");
                logger?.DebugLine ($"\t{exc.StackTrace}");
            }
        }

        private void anglrParserStates_MouseRightButtonDown (object sender, System.Windows.Input.MouseButtonEventArgs e)
        {

        }

        private void anglrParserStates_KeyDown (object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key != System.Windows.Input.Key.Enter)
                return;

            logger?.DebugLine ($"KeyDown: find selected item");
            // operate on previously selected item
            AnglrStateItem stateItem = anglrParserStates.SelectedItem as AnglrStateItem;
            if (stateItem == null)
            {
                logger?.DebugLine ($"KeyDown: no selected items");
                return;
            }

            logger?.DebugLine ($"KeyDown: locate view item: {stateItem.State}");
            // try to get linked AnglrStateItem object
            stateItem = stateItem.TreeViewItemSet [stateItem.State];
            if (stateItem == null)
            {
                logger?.DebugLine ($"KeyDown: selected item is not registered");
                return;
            }

            logger?.DebugLine ($"KeyDown: traverse path to view item: {stateItem.State}");
            TreeViewItem treeViewItem;
            // try to get TreeViewItem associated with linked item
            if ((treeViewItem = TraversePath (anglrParserStates, stateItem.CreatePath ())) == null)
            {
                logger?.DebugLine ($"KeyDown: cannot traverse path");
                return;
            }

            logger?.DebugLine ($"KeyDown: bring into view item: {stateItem.State}");
            // bring linked TreeViewItem into  view
            treeViewItem.IsSelected = true;
            treeViewItem.BringIntoView ();
        }

        private void anglrParserStates_MouseDoubleClick (object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            // try to find TreeViewItem parent of item being clicked
            TreeViewItem treeViewItem = (e.OriginalSource as DependencyObject)?.GetParentOfType<TreeViewItem> ();
            if (treeViewItem == null)
                return;

            // try to get AnglrStateItem object associated with TreeViewItem being clicked
            AnglrStateItem stateItem = treeViewItem.Header as AnglrStateItem;
            if (stateItem == null)
                return;

            // try to get linked AnglrStateItem object
            stateItem = stateItem.TreeViewItemSet [stateItem.State];
            if (stateItem == null)
                return;

            // try to get TreeViewItem associated with linked item
            if ((treeViewItem = TraversePath (anglrParserStates, stateItem.CreatePath ())) == null)
                return;

            // bring linked TreeViewItem into  view
            treeViewItem.IsSelected = true;
            treeViewItem.BringIntoView ();
        }

        private void stateBrowser_Navigating (object sender, System.Windows.Navigation.NavigatingCancelEventArgs e)
        {
            try
            {
                // try to get clicked link
                string uri = ((e.Uri != null) && (e.Uri.OriginalString != null)) ? e.Uri.OriginalString : null;
                if (uri == null)
                    return;
                // link is something like that: about-blank:#state_NR,
                // where NR is number of some state in pushdown automata
                int position = uri.IndexOf ('_');
                if (position < 0)
                    return;
                // read state number
                int stateNumber = int.Parse (uri.Substring (position + 1));

                // try to get AnglrStateItem object assocoated with clicked state number
                AnglrStateItem stateItem = stateTab.Tag as AnglrStateItem;
                if (stateItem == null)
                    return;

                viablePrefix = stateItem.CreatePath ();

                //anglrLangService.Log ($"navigate to {stateNumber}");

                // now that we have the number of some pushdown automata state,
                // acquire states associated with viable prefix leading from
                // the beginning of pushdown automata (state 0) to the state being
                // clicked in browser window
                AnglrGetParserStateLinkParams anglrGetParserStateLinkParams = new AnglrGetParserStateLinkParams ()
                {
                    StateNr = stateNumber,
                    TextDocument = new TextDocumentIdentifier ()
                    {
                        Uri = new System.Uri (stateItem.RootItem.Name)
                    }
                };
                AnglrGetParserStateLinkResult anglrGetParserStateLinkResult = anglrLangService.InvokeGetParserStateLink (anglrGetParserStateLinkParams);

                // for each state in viable prefix leading from the beginning of pushdown
                // automata (state 0) to the selected state: realize that state if it has not
                // been realized yet and than display it.
                foreach (int stateNr in anglrGetParserStateLinkResult.StateLinks)
                {
                    if (stateItem.TreeViewItemSet.ContainsKey (stateNr))
                    {
                        stateItem = stateItem.TreeViewItemSet [stateNr];
                        // ensure that realized state is visible
                        if (stateItem.IsLoaded)
                        {
                            TreeViewItem treeViewItem;
                            if ((treeViewItem = TraversePath (anglrParserStates, stateItem.CreatePath ())) == null)
                            {
                                //anglrLangService.Log ($"cannot display {stateNr}", "\t");
                                continue;
                            }

                            //anglrLangService.Log ($"display {stateNr}", "\t");
                            // bring linked TreeViewItem into  view
                            treeViewItem.IsSelected = true;
                            treeViewItem.IsExpanded = true;
                            treeViewItem.BringIntoView ();
                            continue;
                        }
                    }

                    // if state has not been realized yet, first acquire info about that state
                    AnglrGetParserStateItemParams anglrGetParserStateItemParams = new AnglrGetParserStateItemParams ()
                    {
                        StateNr = stateItem.State,
                        TextDocument = new TextDocumentIdentifier ()
                        {
                            Uri = new System.Uri (stateItem.RootItem.Name)
                        }
                    };
                    AnglrGetParserStateItemResult anglrGetParserStateItemResult = anglrLangService.InvokeGetParserState (anglrGetParserStateItemParams);

                    //anglrLangService.Log ($"expand {stateNr}", "\t");

                    stateNumber = stateItem.State;
                    AnglrStateItem selectedStateItem = null;
                    AnglrStateItem anglrStateItem = null;

                    // add all shift transitions - transitions through terminal symbols
                    foreach (var item in anglrGetParserStateItemResult.ShiftSet)
                    {
                        if (!stateItem.TreeViewItemSet.TryGetValue (item.State, out anglrStateItem))
                            //anglrLangService.Log ($"shift {stateNumber} --> {item.State}", "\t");
                            stateItem.anglrStateItems.Add
                            (
                                anglrStateItem = new AnglrStateItem
                                (
                                    stateItem,
                                    true,
                                    item.Token,
                                    item.State,
                                    item.Conflicts,
                                    stateItem.TreeViewItemSet
                                )
                            );
                        if (item.State == stateNr)
                            selectedStateItem = anglrStateItem;
                    }

                    // add all goto transitions - transitions through non-terminal symbols
                    foreach (var item in anglrGetParserStateItemResult.GotoSet)
                    {
                        if (!stateItem.TreeViewItemSet.TryGetValue (item.State, out anglrStateItem))
                            //anglrLangService.Log ($"goto {stateNumber} --> {item.State}", "\t");
                            stateItem.anglrStateItems.Add
                            (
                                anglrStateItem = new AnglrStateItem
                                (
                                    stateItem,
                                    false,
                                    item.Token,
                                    item.State,
                                    item.Conflicts,
                                    stateItem.TreeViewItemSet
                                )
                            );
                        if (item.State == stateNr)
                            selectedStateItem = anglrStateItem;
                    }

                    // one and only one of transitions should be positioned on viable prefix path
                    if (selectedStateItem == null)
                    {
                        //anglrLangService.Log ($"undefined item for state {stateNr}", "\t");
                        return;
                    }

                    // display tree view node associated with viable prefix path transition
                    stateItem = selectedStateItem;
                    {
                        TreeViewItem treeViewItem;
                        if ((treeViewItem = TraversePath (anglrParserStates, stateItem.CreatePath ())) == null)
                        {
                            //anglrLangService.Log ($"cannot display {stateNr}", "\t");
                            continue;
                        }

                        //anglrLangService.Log ($"display {stateNr}", "\t");
                        // bring linked TreeViewItem into  view
                        treeViewItem.IsSelected = true;
                        treeViewItem.IsExpanded = true;
                        treeViewItem.BringIntoView ();
                    }
                }
            }
            catch (Exception ex)
            {
                logger?.DebugLine ($"EXCEPTION: {ex.Message}");
                logger?.DebugLine ($"\t{ex.StackTrace}");
            }
        }

        private void openState_Click (object sender, RoutedEventArgs e)
        {
            AnglrStateItem anglrStateItem = anglrParserStates.SelectedItem as AnglrStateItem;
            if (anglrStateItem == null)
                return;
            package.JoinableTaskFactory.RunAsync (async delegate
            {
                try
                {
                    AnglrStatesWindow window = (AnglrStatesWindow) await package.ShowToolWindowAsync (typeof (AnglrStatesWindow), 0, true, package.DisposalToken);
                    if ((null == window) || (null == window.Frame))
                    {
                        throw new NotSupportedException ("Cannot create tool window");
                    }
                    logger?.DebugLine ($"Add state tab, 1: {anglrStateItem.State}");
                    window.AnglrStatesControl.package = package;
                    window.AnglrStatesControl.AddStateTab (anglrStateItem);
                    logger?.DebugLine ($"Add state tab, 2: {anglrStateItem.State}");
                }
                catch (Exception ex)
                {
                    logger?.DebugLine ($"EXCEPTION: {ex.Message}");
                    logger?.DebugLine ($"\t{ex.StackTrace}");
                }
            });
        }

        public void CheckConflicts (AnglrGetParserStateItemResult stateItemResult)
        {
            foreach (AnglrGetParserStateReductionsData reductionsData in stateItemResult.ReductionsSet)
            {
                foreach (AnglrGetParserStateTransitionData stateTransitionData in stateItemResult.ShiftSet)
                {
                    foreach (string symbol in reductionsData.FollowSet)
                        if (symbol == stateTransitionData.Token)
                        {
                            // shift-reduce conflict
                        }
                }
            }
            int counterVer = 0;
            foreach (AnglrGetParserStateReductionsData reductionsDataVer in stateItemResult.ReductionsSet)
            {
                int counterHor = 0;
                foreach (AnglrGetParserStateReductionsData reductionsDataHor in stateItemResult.ReductionsSet)
                {
                    if (counterHor > counterVer)
                        foreach (string symbolHor in reductionsDataHor.FollowSet)
                        {
                            foreach (string symbolVer in reductionsDataVer.FollowSet)
                            {
                                if (symbolHor == symbolVer)
                                {
                                    // reduce-reduce conflict
                                }
                            }
                        }
                    ++counterHor;
                }
                ++counterVer;
            }
        }

        public TreeViewItem TraversePath (TreeView treeView, ViablePrefix path)
        {
            int count = path.Count;
            ItemsControl current = treeView;

            //Log ($"TraversePath: path = {path.GetPath ()}");
            foreach (var part in path)
            {
                --count;
                //Log ($"TraversePath: part = {part.Name}");
                ItemContainerGenerator itemContainer = current.ItemContainerGenerator;
                TreeViewItem treeViewItem = null;

                foreach (var item in current.Items)
                {
                    var tvi = itemContainer.ContainerFromItem (item);
                    if (tvi == null)
                    {
                        //Log ($"TraversePath: tvi is null");
                        continue;
                    }
                    if (!(tvi is TreeViewItem))
                    {
                        //Log ($"TraversePath: tvi is not TreeViewItem");
                        continue;
                    }
                    treeViewItem = tvi as TreeViewItem;
                    AnglrStateItem anglrStateItem = treeViewItem.Header as AnglrStateItem;
                    //Log ($"TraversePath: check item = {treeViewItem.Name}");
                    if (anglrStateItem == null)
                    {
                        //Log ($"TraversePath: item header is null");
                        continue;
                    }
                    if (anglrStateItem.Token != part.Token)
                        continue;
                    if (count != 0)
                        treeViewItem.IsExpanded = true;
                    //Log ($"TraversePath: found stateItem = {anglrStateItem.Name}");
                    break;
                }

                if ((current = treeViewItem) == null)
                {
                    //Log ($"TraversePath: part tree-view-item is null");
                    return null;
                }
            }

            {
                TreeViewItem treeViewItem = current as TreeViewItem;
                if (treeViewItem != null)
                {
                    treeViewItem.IsSelected = true;
                    treeViewItem.BringIntoView ();
                    //Log ($"TraversePath: resulting item = {treeViewItem.Name}");
                }
                else
                    ;//Log ($"TraversePath: resulting item = null");
                return current as TreeViewItem;
            }
        }

        private void startProgramPathBrowser_Click (object sender, RoutedEventArgs e)
        {

        }

        private void workingDirectoryPathBrowser_Click (object sender, RoutedEventArgs e)
        {

        }
    }

    public class AnglrLangItem
    {
        public AnglrLangItem (AnglrLangItem parent, int specie, string name, string id)
        {
            Specie = specie;
            specie -= (int) ProductionID.__anglr_file_fragment__ID;
            Parent = parent;
            ImagePath = (specie >= AnglrLangWindowControl.anglrLangWindowIcons.Length) ? "" : AnglrLangWindowControl.anglrLangWindowIcons [specie];
            Name = name;
            Id = id;
            anglrLangItems = new ObservableCollection<AnglrLangItem> ();
            Root = (parent != null) ? parent.Root : this;
        }

        public bool Equal (AnglrLangItem other)
        {
            if (other.Name.CompareTo (Name) != 0)
                return false;
            if (other.Id.CompareTo (Id) != 0)
                return false;
            return true;
        }

        public AnglrLangItem Parent { get; private set; }
        public AnglrLangItem Root { get; private set; }
        public string ImagePath { get; private set; }
        public string Name { get; private set; }
        public string Id { get; private set; }
        public int Specie { get; private set; }
        public ObservableCollection<AnglrLangItem> anglrLangItems { get; set; }
    }

    public class AnglrStateItem
    {
        private static uint _id = 0;
        public uint Id { get; private set; }
        public AnglrStateItem ParentItem { get; private set; }
        public bool IsShift { get; private set; }
        public string Token { get; private set; }
        public int State { get; private set; }
        internal TreeViewItemSet TreeViewItemSet { get; private set; }
        public ViablePrefix StateViablePrefix { get; private set; }

        public string Name { get; private set; }
        public bool IsLink { get; private set; }
        public ObservableCollection<AnglrStateItem> anglrStateItems { get; private set; }
        public AnglrStateItem RootItem { get; private set; }
        public string ImagePath { get; private set; }
        public string LinkImagePath { get; private set; }
        public string KindImagePath { get; private set; }
        public string Text { get; private set; }
        internal AnglrGetParserStateItemResult getParserStateItemResult { get; private set; }
        public bool IsLoaded { get => getParserStateItemResult != null; }
        public SRConflictSet SRConflicts { get; private set; }
        public RRConflictSet RRConflicts { get; private set; }

        private static string stateHtmlPrologue =
            "<html><head>\r\n" +
            "<style>\r\n" +
            "th, td {\n\r" +
            " padding-right: 0.5em;\n\r" +
            " text-align: left;\n\r" +
            "}\n\r" +
            "tr {\n\r" +
            " border-bottom: 1px solid #DCDCDC;\n\r" +
            "}\n\r" +
            //"tr:nth-child(even) {\n\r" +
            //" background-color: #d6eeee;\n\r" +
            //"}\n\r" +
            ".terminal-symbol {\n\r" +
            " color: blue;\r\n" +
            " background-color: lightgray;\r\n" +
            " white-space: nowrap;\r\n" +
            "}\n\r" +
            ".non-terminal-symbol {\n\r" +
            " font-weight: bold;\r\n" +
            " font-style: italic;\r\n" +
            " color: orange;\r\n" +
            " background-color: libhtblue;\r\n" +
            " white-space: nowrap;\r\n" +
            "}\n\r" +
            ".core-caption {\n\r" +
            " font-size: 1.5em;\r\n" +
            " font-weight: bold;\r\n" +
            " text-align: left;\r\n" +
            " white-space: nowrap;\r\n" +
            "}\n\r" +
            ".closure-caption {\n\r" +
            " font-size: 1.5em;\r\n" +
            " font-weight: bold;\r\n" +
            " text-align: left;\r\n" +
            " white-space: nowrap;\r\n" +
            "}\n\r" +
            ".shift-caption {\n\r" +
            " font-size: 1.5em;\r\n" +
            " font-weight: bold;\r\n" +
            " text-align: left;\r\n" +
            " white-space: nowrap;\r\n" +
            "}\n\r" +
            ".goto-caption {\n\r" +
            " font-size: 1.5em;\r\n" +
            " font-weight: bold;\r\n" +
            " text-align: left;\r\n" +
            " white-space: nowrap;\r\n" +
            "}\n\r" +
            ".reductions-caption {\n\r" +
            " font-size: 1.5em;\r\n" +
            " font-weight: bold;\r\n" +
            " text-align: left;\r\n" +
            " white-space: nowrap;\r\n" +
            "}\n\r" +
            ".srconflict-caption {\n\r" +
            " font-size: 1.5em;\r\n" +
            " font-weight: bold;\r\n" +
            " text-align: left;\r\n" +
            " white-space: nowrap;\r\n" +
            "}\n\r" +
            ".rrconflict-caption {\n\r" +
            " font-size: 1.5em;\r\n" +
            " font-weight: bold;\r\n" +
            " text-align: left;\r\n" +
            " white-space: nowrap;\r\n" +
            "}\n\r" +
            ".core-table {\n\r" +
            " border-collapse: collapse;\n\r" +
            "}\n\r" +
            ".closure-table {\n\r" +
            " border-collapse: collapse;\n\r" +
            "}\n\r" +
            ".shift-table {\n\r" +
            " border-collapse: collapse;\n\r" +
            "}\n\r" +
            ".goto-table {\n\r" +
            " border-collapse: collapse;\n\r" +
            "}\n\r" +
            ".reductions-table {\n\r" +
            " border-collapse: collapse;\n\r" +
            "}\n\r" +
            ".srconflict-table {\n\r" +
            " border-collapse: collapse;\n\r" +
            "}\n\r" +
            ".rrconflict-table {\n\r" +
            " border-collapse: collapse;\n\r" +
            "}\n\r" +
            "</style>" +
            "</head><body>";
        private static string stateHtmlEpilogue =
            "</body></html>";

        public AnglrStateItem (AnglrStateItem parentItem, bool isShift, string name, int state, uint conflicts, TreeViewItemSet treeViewItemSet)
        {
            Id = ++_id;
            ParentItem = parentItem;
            IsShift = isShift;
            Token = name;
            State = state;
            TreeViewItemSet = treeViewItemSet;
            StateViablePrefix = CreatePath ();

            if (IsLink = TreeViewItemSet.ContainsKey (State))
                LinkImagePath = "Resources/LS.png";
            else
            {
                LinkImagePath = "Resources/ON.png";
                TreeViewItemSet [State] = this;
            }

            if (isShift)
                KindImagePath = "Resources/ST.png";
            else
                KindImagePath = "Resources/GT.png";

            switch (conflicts)
            {
                case 0:
                    ImagePath = "Resources/NN.png";
                    break;
                case 1:
                    ImagePath = "Resources/CN.png";
                    break;
                case 2:
                    ImagePath = "Resources/NC.png";
                    break;
                case 3:
                    ImagePath = "Resources/CC.png";
                    break;
                default:
                    ImagePath = "Resources/NN.png";
                    break;
            }

            anglrStateItems = new ObservableCollection<AnglrStateItem> ();
            RootItem = (parentItem != null) ? parentItem.RootItem : this;
            getParserStateItemResult = null;
            SRConflicts = new SRConflictSet ();
            RRConflicts = new RRConflictSet ();

            Name = (State != 0) ? isShift ? $"{Token} shift to {State}" : $"{Token} go to {State}" : Token;
            Text = $"state {state}";
        }

        public bool Equal (AnglrStateItem other)
        {
            if (other.Name.CompareTo (Name) != 0)
                return false;
            if (other.State != State)
                return false;
            return true;
        }

        public void DefineText (AnglrGetParserStateItemResult getParserStateItemResult)
        {
            if (getParserStateItemResult == null)
                return;

            CheckConflicts (this.getParserStateItemResult = getParserStateItemResult);

            Text = stateHtmlPrologue;
            Text += $"<br/><hr/>";
            Text += $"<table>";
            Text += $"<caption class=\"core-caption\">State {getParserStateItemResult.StateNumber}</caption>";

            if (IsLink)
                Text += $"<tr><td>Type</td><td>link</td></tr>";
            else
                Text += $"<tr><td>Type</td><td>original</td></tr>";

            if (IsShift)
                Text += $"<tr><td>Transition</td><td>shift</td></tr>";
            else
                Text += $"<tr><td>Transition</td><td>goto</td></tr>";

            AnglrGetParserStateSymbolTokenData anglrGetParserStateSymbolTokenData = getParserStateItemResult.SymbolToken;
            if (anglrGetParserStateSymbolTokenData.Declarator == 18)
                Text += $"<tr><td>Symbol</td><td><span class=\"terminal-symbol\">{WebUtility.HtmlEncode ((anglrGetParserStateSymbolTokenData.Synonym != null) ? anglrGetParserStateSymbolTokenData.Synonym : anglrGetParserStateSymbolTokenData.Name)}</td></tr></span>";
            else
                Text += $"<tr><td>Symbol</td><td><span class=\"non-terminal-symbol\">{WebUtility.HtmlEncode (anglrGetParserStateSymbolTokenData.Name)}</td></tr></span>";

            Text += $"<tr><td>Viable Prefix</td><td>{StateViablePrefix.GetHtmlPath ()}</td></tr>";

            if ((SRConflicts.Count > 0) || (RRConflicts.Count > 0))
                Text += $"<tr><td>Conflicts</td><td><span class=\"conflicts-info\">{SRConflicts.Count} shift/reduce, {RRConflicts.Count} reduce/reduce</span></td></tr>";

            Text += $"</table>";

            {
                AnglrGetParserStateCoreData [] anglrGetParserStateCoreData = getParserStateItemResult.CoreSet;
                if (anglrGetParserStateCoreData.Length > 0)
                {
                    Text += $"<br/><hr/>";
                    Text += $"<table class=\"core-table\">";
                    Text += $"<caption class=\"core-caption\">Core Productions</caption>";
                    Text += $"<tr><th>Production Nr.</th><th>Production Name</th><th>Production</th></tr>";
                    foreach (AnglrGetParserStateCoreData coreData in anglrGetParserStateCoreData)
                    {
                        int position = coreData.Position;
                        int index = 0;
                        AnglrGetParserStateProductionData productionData = coreData.Production;
                        Text += $"<tr>";
                        Text += $"<td>{productionData.ProductionNumber}</td>";
                        Text += $"<td><span class=\"non-terminal-symbol\">{WebUtility.HtmlEncode (productionData.ProductionName)}</span></td>";
                        Text += $"<td>";
                        foreach (AnglrGetParserStateSymbolTokenData symbolTokenData in productionData.RhsNodeSet)
                        {
                            if (index++ == position)
                                Text += $" &bull;";
                            if (symbolTokenData.Declarator == 18)
                                Text += $" <span class=\"terminal-symbol\">{WebUtility.HtmlEncode ((symbolTokenData.Synonym != null) ? symbolTokenData.Synonym : symbolTokenData.Name)}</span>";
                            else
                                Text += $" <span class=\"non-terminal-symbol\">{WebUtility.HtmlEncode (symbolTokenData.Name)}</span>";
                        }
                        if (index++ == position)
                            Text += $" &bull;";
                        Text += $"</td>";
                        Text += $"</tr>";
                    }
                    Text += $"</table>";
                }
                AnglrGetParserStateClosureData [] anglrGetParserStateClosureData = getParserStateItemResult.ClosureSet;
                if (anglrGetParserStateClosureData.Length > 0)
                {
                    Text += $"<br/><hr/>";
                    Text += $"<table class=\"closure-table\">";
                    Text += $"<caption class=\"closure-caption\">Closure Productions</caption>";
                    Text += $"<tr><th>Production Nr.</th><th>Production Name</th><th>Production</th></tr>";
                    foreach (AnglrGetParserStateClosureData closureData in anglrGetParserStateClosureData)
                    {
                        AnglrGetParserStateProductionNodeData productionNodeData = closureData.ProductionNode;
                        foreach (AnglrGetParserStateProductionData stateProductionData in productionNodeData.ProductionSet)
                        {
                            Text += $"<tr>";
                            Text += $"<td>{stateProductionData.ProductionNumber}</td>";
                            Text += $"<td><span class=\"non-terminal-symbol\">{WebUtility.HtmlEncode (stateProductionData.ProductionName)}</span></td>";
                            Text += $"<td>&bull;";
                            foreach (AnglrGetParserStateSymbolTokenData symbolTokenData in stateProductionData.RhsNodeSet)
                            {
                                if (symbolTokenData.Declarator == 18)
                                    Text += $" <span class=\"terminal-symbol\">{WebUtility.HtmlEncode ((symbolTokenData.Synonym != null) ? symbolTokenData.Synonym : symbolTokenData.Name)}</span>";
                                else
                                    Text += $" <span class=\"non-terminal-symbol\">{WebUtility.HtmlEncode (symbolTokenData.Name)}</span>";
                            }
                            Text += $"</td>";
                            Text += $"</tr>";
                        }
                    }
                    Text += $"</table>";
                }
                AnglrGetParserStateTransitionData [] shiftSet = getParserStateItemResult.ShiftSet;
                if (shiftSet.Length > 0)
                {
                    Text += $"<br/><hr/>";
                    Text += $"<table class=\"shift-table\">";
                    Text += $"<caption class=\"shift-caption\">Shift States</caption>";
                    Text += $"<tr><th>Symbol</th><th>State</th></tr>";
                    foreach (AnglrGetParserStateTransitionData shiftData in shiftSet)
                    {
                        Text += $"<tr>";
                        Text += $"<td><span class=\"terminal-symbol\">{WebUtility.HtmlEncode (shiftData.Token)}</span></td>";
                        Text += $"<a href=\"#state_{shiftData.State}\"><td>{shiftData.State}</td></a>";
                        Text += $"</tr>";
                    }
                    Text += $"</table>";
                }
                AnglrGetParserStateTransitionData [] gotoSet = getParserStateItemResult.GotoSet;
                if (gotoSet.Length > 0)
                {
                    Text += $"<br/><hr/>";
                    Text += $"<table class=\"goto-table\">";
                    Text += $"<caption class=\"goto-caption\">Goto States</caption>";
                    Text += $"<tr><th>Symbol</th><th>State</th></tr>";
                    foreach (AnglrGetParserStateTransitionData gotoData in gotoSet)
                    {
                        Text += $"<tr>";
                        Text += $"<td><span class=\"non-terminal-symbol\"> {WebUtility.HtmlEncode (gotoData.Token)} </span></td>";
                        Text += $"<a href=\"#state_{gotoData.State}\"><td>{gotoData.State}</td></a>";
                        Text += $"</tr>";
                    }
                    Text += $"</table>";
                }
                AnglrGetParserStateReductionsData [] reductionsSet = getParserStateItemResult.ReductionsSet;
                if (reductionsSet.Length > 0)
                {
                    Text += $"<br/><hr/>";
                    Text += $"<table class=\"reductions-table\">";
                    Text += $"<caption class=\"reductions-caption\">Reductions</caption>";
                    Text += $"<tr><th>Production Nr.</th><th>Production Name</th><th>Production</th></tr>";
                    foreach (AnglrGetParserStateReductionsData reductionData in reductionsSet)
                    {
                        AnglrGetParserStateProductionData productionData = reductionData.Production;
                        Text += $"<tr>";
                        Text += $"<td>{productionData.ProductionNumber}</td>";
                        Text += $"<td><span class=\"non-terminal-symbol\">{WebUtility.HtmlEncode (productionData.ProductionName)}</span></td>";
                        Text += $"<td>";
                        foreach (AnglrGetParserStateSymbolTokenData symbolTokenData in productionData.RhsNodeSet)
                        {
                            if (symbolTokenData.Declarator == 18)
                                Text += $" <span class=\"terminal-symbol\">{WebUtility.HtmlEncode ((symbolTokenData.Synonym != null) ? symbolTokenData.Synonym : symbolTokenData.Name)}</span>";
                            else
                                Text += $" <span class=\"non-terminal-symbol\">{WebUtility.HtmlEncode (symbolTokenData.Name)}</span>";
                        }
                        Text += $"</td>";
                        Text += $"</tr>";
                    }
                    Text += $"</table>";
                }
                if (SRConflicts.Count > 0)
                {
                    Text += $"<br/><hr/>";
                    Text += $"<table class=\"srconflict-table\">";
                    Text += $"<caption class=\"srconflict-caption\">Shift-Reduce Conflicts</caption>";
                    Text += $"<tr><th>Symbol</th><th>State</th><th>Production Name</th></tr>";
                    foreach (KeyValuePair<string, SRConflictList> pair in SRConflicts)
                    {
                        bool ind = true;
                        string symbol = pair.Key;
                        SRConflictList conflicts = pair.Value;
                        foreach ((int, AnglrGetParserStateReductionsData) reductionsData in conflicts)
                        {
                            Text += $"<tr>";
                            if (ind)
                            {
                                ind = false;
                                Text += $"<td><span class=\"terminal-symbol\">{symbol}</span></td>";
                            }
                            else
                                Text += $"<td></td>";
                            Text += $"<td>{reductionsData.Item1}</td>";
                            Text += $"<td><span class=\"non-terminal-symbol\">{WebUtility.HtmlEncode (reductionsData.Item2.Production.ProductionName)}</span></td>";
                            Text += $"</tr>";
                        }
                    }
                    Text += $"</table>";
                }
                if (RRConflicts.Count > 0)
                {
                    Text += $"<br/><hr/>";
                    Text += $"<table class=\"rrconflict-table\">";
                    Text += $"<caption class=\"rrconflict-caption\">Reduce-Reduce Conflicts</caption>";
                    Text += $"<tr><th>Symbol</th><th>Production Name</th><th>Production Name</th></tr>";
                    foreach (KeyValuePair<string, RRConflictList> pair in RRConflicts)
                    {
                        bool ind = true;
                        string symbolName = "";
                        string symbol = pair.Key;
                        RRConflictList conflicts = pair.Value;
                        foreach ((AnglrGetParserStateReductionsData, AnglrGetParserStateReductionsData) reductionsData in conflicts)
                        {
                            Text += $"<tr>";
                            if (ind)
                            {
                                ind = false;
                                Text += $"<td><span class=\"terminal-symbol\">{symbol}</span></td>";
                            }
                            else
                                Text += $"<td></td>";
                            if (symbolName != reductionsData.Item1.Production.ProductionName)
                            {
                                Text += $"<td><span class=\"non-terminal-symbol\">{WebUtility.HtmlEncode (reductionsData.Item1.Production.ProductionName)}</span></td>";
                                symbolName = reductionsData.Item1.Production.ProductionName;
                            }
                            else
                                Text += $"<td></td>";
                            Text += $"<td><span class=\"non-terminal-symbol\">{WebUtility.HtmlEncode (reductionsData.Item2.Production.ProductionName)}</span></td>";
                            Text += $"</tr>";
                        }
                    }
                    Text += $"</table>";
                }
            }
            Text += stateHtmlEpilogue;
        }

        internal void Copy (AnglrStateItem anglrStateItem)
        {
            Name = (State != 0) ? IsShift ? $"{Token} shift to {State}" : $"{Token} go to {State}" : Token;
            DefineText (anglrStateItem.getParserStateItemResult);
        }

        public void CheckConflicts (AnglrGetParserStateItemResult stateItemResult)
        {
            foreach (AnglrGetParserStateReductionsData reductionsData in stateItemResult.ReductionsSet)
            {
                foreach (AnglrGetParserStateTransitionData stateTransitionData in stateItemResult.ShiftSet)
                {
                    foreach (string symbol in reductionsData.FollowSet)
                        if (symbol == stateTransitionData.Token)
                        {
                            SRConflictList srConflictList = null;
                            if (!SRConflicts.ContainsKey (symbol))
                                SRConflicts.Add (symbol, srConflictList = new SRConflictList ());
                            srConflictList = SRConflicts [symbol];
                            srConflictList.Add ((stateTransitionData.State, reductionsData));
                        }
                }
            }
            int counterVer = 0;
            foreach (AnglrGetParserStateReductionsData reductionsDataVer in stateItemResult.ReductionsSet)
            {
                int counterHor = 0;
                foreach (AnglrGetParserStateReductionsData reductionsDataHor in stateItemResult.ReductionsSet)
                {
                    if (counterHor > counterVer)
                        foreach (string symbolHor in reductionsDataHor.FollowSet)
                        {
                            foreach (string symbolVer in reductionsDataVer.FollowSet)
                            {
                                if (symbolHor == symbolVer)
                                {
                                    RRConflictList rrConflictList = null;
                                    if (!RRConflicts.ContainsKey (symbolHor))
                                        RRConflicts.Add (symbolHor, rrConflictList = new RRConflictList ());
                                    rrConflictList = RRConflicts [symbolHor];
                                    rrConflictList.Add ((reductionsDataVer, reductionsDataHor));
                                }
                            }
                        }
                    ++counterHor;
                }
                ++counterVer;
            }
        }

        public ViablePrefix CreatePath ()
        {
            ViablePrefix path;
            if (ParentItem != null)
                path = ParentItem.CreatePath ();
            else
                path = new ViablePrefix ();
            path.Add (this);
            return path;
        }
    }

    public class AnglrDebugPanelItem
    {
        public AnglrDebugPanelItem (string anglrFilePathValue)
        {
            this.anglrFilePathValue = anglrFilePathValue;
            startProgramPathValue = "";
            commandLineParametersValue = "";
            workingDirectoryPathValue = "";
        }
        public string anglrFilePathValue { get; private set; }
        public string startProgramPathValue { get; set; }
        public string commandLineParametersValue { get; set; }
        public string workingDirectoryPathValue { get; set; }
    }
}
