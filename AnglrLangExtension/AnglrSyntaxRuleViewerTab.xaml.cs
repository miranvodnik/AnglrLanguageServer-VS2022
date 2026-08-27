using AnglrLogLibrary;
using System;
using System.Collections.Generic;
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
    /// <summary>
    /// Interaction logic for AnglrSyntaxRuleViewerTab.xaml
    /// </summary>
    public partial class AnglrSyntaxRuleViewerTab : UserControl
    {
        public IAnglrLangService LangService { get; }
        public IAnglrLogger Logger { get; }
        AnglrLangDictionaryItem LangDictionaryItem { get; }
        AnglrRawDrawingVisual DrawingVisual { get; }
        public AnglrSyntaxRuleViewerTab (IAnglrLangService anglrLangService, int magicNr, string fileName, AnglrRawDrawingVisual drawingVisual)
        {
            InitializeComponent ();

            LangService = anglrLangService;
            Logger = anglrLangService?.AnglrLogger ?? new VoidAnglrLogger ();
            if ((LangDictionaryItem = AnglrLangDictionary.GetItem (magicNr)) == null)
                Logger?.ErrorLine ($"Cannot retrieve syntax drawing for {fileName}, language service dictionary[{magicNr}] does not exist");
            if ((DrawingVisual = drawingVisual) == null)
                DrawingVisual = LangDictionaryItem.DrawingVisual;
            if (DrawingVisual != null)
            {
                syntaxRuleVisual.AddVisual (DrawingVisual);
                syntaxRuleVisual.Width = DrawingVisual.Width;
                syntaxRuleVisual.Height = DrawingVisual.Height;
            }
            else
                Logger?.ErrorLine ($"Cannot retrieve syntax drawing for {fileName}, drawing visual[{magicNr}] does not exist");
        }

        private void syntaxRuleVisual_MouseDown (object sender, MouseButtonEventArgs e)
        {

        }

        private void syntaxRuleVisual_MouseEnter (object sender, MouseEventArgs e)
        {

        }

        private void syntaxRuleVisual_MouseLeave (object sender, MouseEventArgs e)
        {

        }

        private void syntaxRuleVisual_MouseLeftButtonDown (object sender, MouseButtonEventArgs e)
        {

        }

        private void syntaxRuleVisual_MouseLeftButtonUp (object sender, MouseButtonEventArgs e)
        {

        }

        private void syntaxRuleVisual_MouseMove (object sender, MouseEventArgs e)
        {

        }

        private void syntaxRuleVisual_MouseRightButtonDown (object sender, MouseButtonEventArgs e)
        {

        }

        private void syntaxRuleVisual_MouseRightButtonUp (object sender, MouseButtonEventArgs e)
        {

        }

        private void syntaxRuleVisual_MouseUp (object sender, MouseButtonEventArgs e)
        {

        }

        private void syntaxRuleVisual_MouseWheel (object sender, MouseWheelEventArgs e)
        {

        }
    }
}
