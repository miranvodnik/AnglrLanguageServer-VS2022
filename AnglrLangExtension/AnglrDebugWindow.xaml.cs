using AnglrDebuggerBridge;
using AnglrDebuggerJsonRpcMessages;
using AnglrLogLibrary;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Threading;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using StreamJsonRpc;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.IO.Packaging;
using System.IO.Pipes;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
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
using System.Windows.Threading;
using MessageBox = System.Windows.Forms.MessageBox;
using UserControl = System.Windows.Controls.UserControl;

namespace AnglrLangExtension
{
    /// <summary>
    /// Interaction logic for AnglrDebugWindow.xaml
    /// </summary>
    public partial class AnglrDebugWindow : UserControl, IDisposable
    {
        private IAnglrLogger logger = null;
        private IAnglrLangService anglrLangService = null;

        private AsyncPackage _package = null;

        public AsyncPackage package
        {
            get => _package;
            set
            {
                _package = value;
                if (anglrLangService == null)
                {
                    anglrLangService = ThreadHelper.JoinableTaskFactory.Run (() => value.GetServiceAsync (typeof (SAnglrLangService))) as IAnglrLangService;
                    logger = anglrLangService?.AnglrLogger ?? new VoidAnglrLogger ();
                }
            }
        }

        public AnglrDebuggerServerBridgeSet AnglrDebuggers { get; private set; }
        private AnglrDebuggerServerBridge anglrDebuggerServerBridge = null;
        private string anglrLocalDirName;
        private string anglrDebugWindowSettingsFileName;

        public AnglrDebugWindow ()
        {
            InitializeComponent ();
            DataContext = this;
            anglrLocalDirName = System.IO.Path.Combine (Environment.GetFolderPath (Environment.SpecialFolder.LocalApplicationData), ".anglr");
            anglrDebugWindowSettingsFileName = System.IO.Path.Combine (anglrLocalDirName, "AnglrDebugWindow.json");
            LoadSettings ();
        }

        public void Dispose () => SaveSettings ();

        private void SaveSettings (bool reset = true)
        {
            try
            {
                if(reset)
                foreach (var item in AnglrDebuggers)
                    item.Reset ();
                logger?.DebugLine ($"SaveSettings; check directory {anglrLocalDirName}");
                if (!Directory.Exists (anglrLocalDirName))
                {
                    logger?.DebugLine ($"SaveSettings; create directory {anglrLocalDirName}");
                    Directory.CreateDirectory (anglrLocalDirName);
                }
                logger?.DebugLine ($"SaveSettings; rewrite file {anglrDebugWindowSettingsFileName}");
                File.WriteAllText (anglrDebugWindowSettingsFileName, JsonConvert.SerializeObject (AnglrDebuggers));
            }
            catch (Exception ex)
            {
                LogException (ex);
            }
        }

        private void LoadSettings ()
        {
            if (Directory.Exists (anglrLocalDirName))
                try
                {
                    AnglrDebuggers = JsonConvert.DeserializeObject<AnglrDebuggerServerBridgeSet> (File.ReadAllText (anglrDebugWindowSettingsFileName));
                    return;
                }
                catch (Exception ex)
                {
                    LogException (ex);
                }
            AnglrDebuggers = new AnglrDebuggerServerBridgeSet ();
        }

        private void startProgramPathBrowser_Click (object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog ();
            openFileDialog.Filter = "Executable (*.exe)|*.exe|Library (*.dll)|*.dll|All Assemblies (*.exe;*.dll)|*.exe;*.dll|All Files (*.*)|*.*";
            openFileDialog.InitialDirectory = startProgramPathValue.Text;
            if (openFileDialog.ShowDialog () != DialogResult.OK)
                return;
            this.startProgramPathValue.Text = openFileDialog.FileName;
        }

        private void workingDirectoryPathBrowser_Click (object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog ();
            folderBrowserDialog.Description = "Select Working Directory Folder for Start Program";
            folderBrowserDialog.ShowNewFolderButton = false;
            folderBrowserDialog.SelectedPath = workingDirectoryPathValue.Text;
            if (folderBrowserDialog.ShowDialog () != DialogResult.OK)
                return;
            workingDirectoryPathValue.Text = folderBrowserDialog.SelectedPath;
        }

        private void clearButton_Click (object sender, RoutedEventArgs e)
        {

        }

        private void editButton_Click (object sender, RoutedEventArgs e)
        {

        }

        private void startButton_Click (object sender, RoutedEventArgs e)
        {
            try
            {
                AnglrDebuggerServerBridge anglrDebuggerServerBridge = anglrDebugWindowGrid.SelectedItem as AnglrDebuggerServerBridge;
                if (anglrDebuggerServerBridge == null)
                {
                    logger?.DebugLine ($"cannot start debugger, selected item is null");
                    return;
                }
                {
                    string selectedAnglrFile = anglrDebuggerServerBridge.FileName;
                    logger?.DebugLine ($"<AnglrDebugWindow>: BTN");
                    if (selectedAnglrFile == null)
                    {
                        logger?.DebugLine ($"<AnglrDebugWindow>: SEL");
                        return;
                    }
                    if (anglrDebuggerServerBridge.IsRunning)
                    {
                        logger?.DebugLine ($"<AnglrDebugWindow>: RET");
                        return;
                    }

                    AnglrDebugPanelWindow window = (AnglrDebugPanelWindow) package.JoinableTaskFactory.Run (() =>
                        package.ShowToolWindowAsync (typeof (AnglrDebugPanelWindow), 0, true, package.DisposalToken));
                    if ((null == window) || (null == window.Frame))
                    {
                        throw new NotSupportedException ("Cannot create tool window");
                    }
                    window.AnglrDebugPanelControl.package = package;
                    logger?.DebugLine ($"Add debugger tab, 1: {selectedAnglrFile}");
                    AnglrDebugPanelTab anglrDebugPanelTab = window.AnglrDebugPanelControl.AddDebugPanelTab (selectedAnglrFile, anglrDebuggerServerBridge);
                    logger?.DebugLine ($"Add debugger tab, 2: {selectedAnglrFile}");

                    logger?.DebugLine ($"<AnglrDebugWindow>: AWAIT START");
                    _ = anglrDebuggerServerBridge.DebuggedProcessCtrlAsync (anglrDebugPanelTab);
                    logger?.DebugLine ($"<AnglrDebugWindow>: AWAIT END");
                }
            }
            catch (Exception ex)
            {
                LogException (ex);
            }
        }

        private void deleteButton_Click (object sender, RoutedEventArgs e)
        {

        }

        private void addDebugEntry_Click (object sender, RoutedEventArgs e)
        {
            if (startProgramPathValue.Text == default)
                return;
            try
            {
                AnglrDebuggers.Add (new AnglrDebuggerServerBridge (startProgramPathValue.Text, commandLineParametersValue.Text, workingDirectoryPathValue.Text));
                SaveSettings (false);
            }
            catch (Exception ex)
            {
                LogException (ex);
            }
        }

        private void anglrDebugWindowGrid_SelectionChanged (object sender, SelectionChangedEventArgs e)
        {
            try
            {
                AnglrDebuggerServerBridge anglrDebuggerServerBridge = anglrDebugWindowGrid.SelectedItem as AnglrDebuggerServerBridge;
                if (anglrDebuggerServerBridge == null)
                {
                    logger?.DebugLine ($"cannot start debugger, selected item is null");
                    return;
                }
                startProgramPathValue.Text = anglrDebuggerServerBridge.FileName;
                commandLineParametersValue.Text = anglrDebuggerServerBridge.Arguments;
                workingDirectoryPathValue.Text = anglrDebuggerServerBridge.WorkingDirectory;
            }
            catch (Exception ex)
            {
                LogException (ex);
            }
        }

        private void LogException (Exception exception)
        {
            StackTrace stackTrace = new StackTrace ();
            if (stackTrace.FrameCount < 2)
                return;
            StackFrame frame = stackTrace.GetFrame (1);
            MethodBase method = frame.GetMethod ();
            int depth = 0;
            logger?.DebugLine ($"{method.DeclaringType.Name}.{method.Name} exception:");
            while (exception != null)
            {
                logger?.DebugLine ($"\t{depth}: {exception.Message}");
                logger?.DebugLine ($"\t{depth}: {exception.StackTrace}");
                exception = exception.InnerException;
                depth++;
            }
        }
    }
}
