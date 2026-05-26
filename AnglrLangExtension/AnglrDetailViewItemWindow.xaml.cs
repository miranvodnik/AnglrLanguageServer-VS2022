using AnglrJsonRpcMethods;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AnglrLangExtension
{
    public class AnglrGetParserSyntaxRuleDataCollection : ObservableCollection<AnglrGetParserSyntaxRuleData>
    {
        public AnglrGetParserSyntaxRuleDataCollection () : base () { }

        public AnglrGetParserSyntaxRuleDataCollection (AnglrGetParserSyntaxRuleData [] data) : base (data) { }
    }

    /// <summary>
    /// Interaction logic for AnglrDetailViewItemWindow.xaml
    /// </summary>
    public partial class AnglrDetailViewItemWindow : UserControl
    {
        public string FileName { get; set; }
        public AnglrGetParserSyntaxRuleDataCollection AnglrGetParserSyntaxRuleDatas { get; set; }

        public AnglrDetailViewItemWindow ()
        {
            InitializeComponent ();
        }

        private void anglrGetParserSyntaxRuleDatas_SelectionChanged (object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
