using AnglrDebuggerBridge;
using AnglrDebuggerJsonRpcMessages;
using AnglrJsonRpcMethods;
using AnglrLogLibrary;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Threading;
using Newtonsoft.Json;
using StreamJsonRpc;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
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
    /// Interaction logic for AnglrDebugPanelTab.xaml
    /// </summary>
    public partial class AnglrDebugPanelTab : UserControl, IAnglrServerSideDebuggerInvoker
    {
        private int sessionCounter;
        private IAnglrLangService anglrLangService;
        public IAnglrLogger Logger { get; private set; }

        /// <summary>
        /// TabItem associated with specific process being debugged
        /// </summary>
        /// <param name="anglrLangService">
        /// reference to object implementing <see cref="IAnglrLangService"/>
        /// </param>
        /// <param name="fileName">
        /// path of executable being debugged
        /// </param>
        /// <param name="anglrDebuggerServerBridge">
        /// reference to object implementing JsonRpc between this TabItem and executable being debugged
        /// </param>
        public AnglrDebugPanelTab (IAnglrLangService anglrLangService, string fileName, AnglrDebuggerServerBridge anglrDebuggerServerBridge)
        {
            InitializeComponent ();

            sessionCounter = 0;
            this.anglrLangService = anglrLangService;
            Logger = anglrLangService?.AnglrLogger ?? new VoidAnglrLogger ();
            Logger?.DebugLine ($"AnglrDebugPanelTab created ");
        }

        /// <summary>
        /// Implementation of InvokeRpcSession method required by interface IAnglrServerSideDebuggerInvoker.
        /// This method is executed once for every parsing method invoked by debugged process.
        /// It starts detached task which will cover the entire debug session of single parsing procedure.
        /// For a single process being debugged, several such sessions can take place sequentially or in parallel.
        /// </summary>
        /// <param name="count">
        /// session counter for process being debugged
        /// </param>
        /// <param name="pipe">
        /// stream associated with named pipe between process being debugged and this TabItem
        /// </param>
        /// <param name="token">
        /// cancellation token which will destroy the session if something unexpected happened.
        /// </param>
        public void InvokeRpcSession (int count, Stream pipe, CancellationToken token)
        {
            AnglrDebugPanelTabSession anglrDebugPanelTabSession = null;
            Dispatcher.Invoke (() =>
            {
                anglrDebugPanelTabSession = new AnglrDebugPanelTabSession (anglrLangService);
                TabItem tabItem = new TabItem ();
                tabItem.Content = anglrDebugPanelTabSession;
                tabItem.Header = $"session {++sessionCounter}";
                anglrDebugPanelSessions.Items.Add (tabItem);
            });
            Logger?.InfoLine ($"starting session {sessionCounter}");
            // start detached task: this task will cover the entire debug session of single parsing procedure.
            // for a single process, several such sessions can take place sequentially or in parallel.
            _ = anglrDebugPanelTabSession.InvokeRpcSessionAsync (count, pipe, token);
            Logger?.InfoLine ($"session {sessionCounter} started");
        }
    }
}
