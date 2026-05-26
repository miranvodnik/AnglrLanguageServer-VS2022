using AnglrDebuggerBridge;
using AnglrLogLibrary;
using Microsoft.VisualStudio.Shell;
using Newtonsoft.Json.Linq;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace AnglrLangExtension
{
    /// <summary>
    /// Interaction logic for AnglrDebugPanelWindowControl.
    /// </summary>
    public partial class AnglrDebugPanelWindowControl : UserControl
    {
        public IAnglrLogger Logger { get; private set; }
        public IAnglrLangService AnglrLangService { get; private set; }
        private AsyncPackage _package = null;

        public AsyncPackage package
        {
            get => _package;
            set
            {
                _package = value;
                if (AnglrLangService == null)
                {
                    AnglrLangService = ThreadHelper.JoinableTaskFactory.Run (() => value.GetServiceAsync (typeof (SAnglrLangService))) as IAnglrLangService;
                    Logger = AnglrLangService?.AnglrLogger ?? new VoidAnglrLogger ();
                }
                Logger?.DebugLine ("Created new Anglr Debug Panel Window Control");
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AnglrDebugPanelWindowControl"/> class.
        /// </summary>
        public AnglrDebugPanelWindowControl ()
        {
            this.InitializeComponent ();
        }

        public AnglrDebugPanelTab AddDebugPanelTab (string fileName, AnglrDebuggerServerBridge anglrDebuggerServerBridge)
        {
            AnglrDebugPanelTab anglrDebugPanelTab = new AnglrDebugPanelTab (AnglrLangService, fileName, anglrDebuggerServerBridge);
            TabItem tabItem = new TabItem ();
            tabItem.Content = anglrDebugPanelTab;
            tabItem.Header = $"{Path.GetFileName(fileName)}";
            anglrDebugPanelTabs.Items.Add (tabItem);
            return anglrDebugPanelTab;
        }
    }
}
