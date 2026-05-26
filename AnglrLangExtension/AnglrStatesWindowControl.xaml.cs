using Microsoft.VisualStudio.Shell;
using System.Diagnostics.CodeAnalysis;
using System.Windows;
using System.Windows.Controls;

namespace AnglrLangExtension
{
    /// <summary>
    /// Interaction logic for AnglrStatesWindowControl.
    /// </summary>
    public partial class AnglrStatesWindowControl : UserControl
    {

        public IAnglrLangService AnglrLangService { get; private set; }
        private AsyncPackage _package = null;

        public AsyncPackage package
        {
            get => _package;
            set
            {
                _package = value;
                AnglrLangService = AnglrLangService ?? ThreadHelper.JoinableTaskFactory.Run (() => value.GetServiceAsync (typeof (SAnglrLangService))) as IAnglrLangService;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AnglrStatesWindowControl"/> class.
        /// </summary>
        public AnglrStatesWindowControl ()
        {
            this.InitializeComponent ();
        }

        public void AddStateTab (AnglrStateItem anglrStateItem)
        {
            TabItem tabItem = new TabItem ();
            tabItem.Content = new AnglrStateTab (anglrStateItem);
            tabItem.Tag = anglrStateItem;
            tabItem.Header = $"State {anglrStateItem.State}";
            anglrStateTabs.Items.Add (tabItem);
        }
    }
}
