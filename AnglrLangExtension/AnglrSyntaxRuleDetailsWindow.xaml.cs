using AnglrJsonRpcMethods;
using AnglrLogLibrary;
using AnglrBreakPointDBLibrary;
using Microsoft.VisualStudio.LanguageServer.Protocol;
using Microsoft.VisualStudio.Shell;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AnglrLangExtension
{
    public class GetProductionText : IMultiValueConverter
    {
        public object Convert (object [] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length < 1)
                return "";
            AnglrGetParserStateSymbolTokenData [] symbolTokenDatas = values [0] as AnglrGetParserStateSymbolTokenData [];
            if (symbolTokenDatas == null)
                return "";
            string productionText = "";
            foreach (var symbolTokenData in symbolTokenDatas)
            {
                productionText += symbolTokenData.Name + ' ';
            }
            return productionText;
        }

        public object [] ConvertBack (object value, Type [] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException ();
        }
    }

    /// <summary>
    /// Interaction logic for AnglrSyntaxRuleDetailsWindow.xaml
    /// </summary>
    public partial class AnglrSyntaxRuleDetailsWindow : System.Windows.Controls.UserControl
    {
        public AsyncPackage Package { get; private set; }
        public IAnglrLangService AnglrLangService { get; private set; }
        public AnglrLangItem AnglrLangItem { get; private set; }
        public IAnglrLogger Logger { get; private set; }
        public ObservableCollection<AnglrGetParserStateProductionData> AnglrGetParserStateProductionDatas { get; private set; }
        public AnglrSyntaxRuleDetailsWindow (AsyncPackage package, IAnglrLangService anglrLangService, AnglrLangItem anglrLangItem)
        {
            InitializeComponent ();
            DataContext = this;

            Package = package;
            AnglrLangService = anglrLangService;
            AnglrLangItem = anglrLangItem;
            Logger = AnglrLangService?.AnglrLogger ?? new VoidAnglrLogger ();

            AnglrGetParserMagicNumberResult anglrGetParserMagicNumberResult = AnglrLangService.InvokeGetParserMagicNumber
            (
                new AnglrGetParserMagicNumberParams ()
                {
                    TextDocument = new TextDocumentIdentifier ()
                    {
                        Uri = new System.Uri ((string) AnglrLangItem.Root.Name)
                    }
                }
            );
            int? magicNr = anglrGetParserMagicNumberResult?.MagicNumber;
            if (!magicNr.HasValue)
                return;

            Logger?.DebugRawLine ($"try get BP chunk, magic nr.: {magicNr.Value}");
            AnglrBreakPointDBChunk dBChunk = null;
            AnglrBreakPointDB.Get (magicNr.Value, out dBChunk);
            if (dBChunk == null)
            {
                Logger?.DebugRawLine ($"create new BP chunk, magic nr.: {magicNr.Value}");
                dBChunk = AnglrBreakPointDB.Add (magicNr.Value);
            }

            AnglrGetGetHierarchyItemParams anglrGetGetHierarchyItemParams = new AnglrGetGetHierarchyItemParams ()
            {
                ItemId = AnglrLangItem.Id,
                TextDocument = new TextDocumentIdentifier ()
                {
                    Uri = new System.Uri ((string) AnglrLangItem.Root.Name)
                }
            };
            AnglrGetGetHierarchyItemResult anglrGetGetHierarchyItemResult = AnglrLangService.InvokeGetHierarchy (anglrGetGetHierarchyItemParams);
            webBrowser.NavigateToString ($"{AnglrLangWindowControl.anglrHtmlPrologue}{anglrGetGetHierarchyItemResult.HtmlText}{AnglrLangWindowControl.anglrHtmlEpilogue}");

            AnglrGetParserSyntaxRuleParams anglrGetParserSyntaxRuleParams = new AnglrGetParserSyntaxRuleParams ()
            {
                RuleName = anglrGetGetHierarchyItemResult.NodeName,
                TextDocument = new TextDocumentIdentifier ()
                {
                    Uri = new System.Uri ((string) AnglrLangItem.Root.Name)
                }
            };

            AnglrGetParserSyntaxRuleResult anglrGetParserSyntaxRuleResult = AnglrLangService.InvokeGetParserSyntaxRule (anglrGetParserSyntaxRuleParams);
            if (anglrGetParserSyntaxRuleResult == null)
                return;

            AnglrGetParserSyntaxRuleData syntaxRuleData = anglrGetParserSyntaxRuleResult.SyntaxRule;
            if (syntaxRuleData == null)
                return;

            AnglrGetParserStateSymbolTokenData symbolTokenData = syntaxRuleData.SyntaxRuleName;
            if (symbolTokenData == null)
                return;

            AnglrGetParserStateProductionData [] productionDatas = syntaxRuleData.Productions;
            if (productionDatas == null)
                return;

            foreach (var productionData in productionDatas)
            {
                AnglrReduceBP reduceBP = new AnglrReduceBP () { ProductionNumber = productionData.ProductionNumber };
                productionData.PropertyChanged +=
                    (sender, e) =>
                    {
                        if (productionData.BreakPoint)
                        {
                            Logger?.DebugRawLine ($"production {productionData.ProductionNumber} BP DB set");
                            dBChunk.AnglrReduceBPSet.Add (reduceBP);
                        }
                        else
                        {
                            Logger?.DebugRawLine ($"production {productionData.ProductionNumber} BP DB unset");
                            dBChunk.AnglrReduceBPSet.Remove (reduceBP);
                        }
                        dBChunk.Changed = true;
                    };
                Logger?.DebugRawLine ($"try get BP info, production nr.: {reduceBP.ProductionNumber}");
                if (!dBChunk.AnglrReduceBPSet.TryGetValue (reduceBP, out _))
                {
                    Logger?.DebugRawLine ($"cannot get BP info, production nr.: {reduceBP.ProductionNumber}");
                    continue;
                }
                Logger?.DebugRawLine ($"production {productionData.ProductionNumber} BP set");
                productionData.BreakPoint = true;
            }

            AnglrGetParserStateProductionDatas = new ObservableCollection<AnglrGetParserStateProductionData> (productionDatas);
        }

        private void DataGrid_CellEditEnding (object sender, DataGridCellEditEndingEventArgs e)
        {
            DataGridColumn dataGridColumn = e.Column;
            DataGridRow dataGridRow = e.Row;
        }
    }
}
