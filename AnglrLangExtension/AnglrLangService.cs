using Anglr.Parser;
using AnglrJsonRpcMethods;
using AnglrLogLibrary;
using EnvDTE;
using EnvDTE80;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.ComponentModelHost;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Text.Outlining;
using Microsoft.VisualStudio.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.ComponentModel.Composition;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using IAsyncServiceProvider = Microsoft.VisualStudio.Shell.IAsyncServiceProvider;

namespace AnglrLangExtension
{
    public enum AnglrItemOperation
    {
        None,
        Get,
        Add,
        Remove,
    }

    public delegate Task AnglrProjectItemCallback (string fileName, AnglrItemOperation operation);

    [Export(typeof(IAnglrLangService))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public sealed class AnglrLangService : SAnglrLangService, IAnglrLangService
    {
        private HashSet<string> openAnglrFiles = new HashSet<string> ();
        private AsyncPackage _serviceProvider = null;
        public IAnglrLogService AnglrLogService { get; private set; }

        public IComponentModel ComponentModel { get; }
        public ITextEditorFactoryService TextEditorFactoryService { get; }
        public ITextBufferFactoryService TextBufferFactoryService { get; }
        public IContentTypeRegistryService ContentTypeRegistryService { get; }
        public IOutliningManagerService OutliningManagerService { get; }

        public IAnglrLogger AnglrLogger { get; private set; }

        private DTE2 dTE = null;

        public event AnglrProjectItemCallback anglrProjectItemEvent;

        public AnglrLangService (AsyncPackage serviceProvider, IAnglrLogService anglrLogService)
        {
            _serviceProvider = serviceProvider;
            AnglrLogService = anglrLogService;
            AnglrLogger = AnglrLogService?.Logger ?? new VoidAnglrLogger ();
            //AnglrLogService = serviceProvider.GetServiceAsync (typeof (AnglrLogService)) as IAnglrLogService;
            dTE = Package.GetGlobalService (typeof (DTE)) as DTE2;
            ComponentModel = (IComponentModel) Package.GetGlobalService (typeof (SComponentModel));
            TextEditorFactoryService = ComponentModel?.GetService<ITextEditorFactoryService> ();
            TextBufferFactoryService = ComponentModel?.GetService<ITextBufferFactoryService> ();
            ContentTypeRegistryService = ComponentModel.GetService<IContentTypeRegistryService> ();
            OutliningManagerService = ComponentModel.GetService<IOutliningManagerService> ();
        }

        public async Task InitializeAsync (CancellationToken cancellationToken)
        {
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync (cancellationToken);
        }

        public void Log (AnglrLogLevel logLevel, string message, string indent = null)
        {
            AnglrLogService?.Logger?.Log (logLevel, message);
        }

        public AnglrDetailsWindow CreateDetailsWindowInstance (UIElement content, string caption, Func<AnglrDetailsWindow, UIElement, IAnglrLangService, bool> init)
        {
            AnglrDetailsWindow window = null;
            for (int i = 0; ; i++)
            {
                if (_serviceProvider.FindToolWindow (typeof (AnglrDetailsWindow), i, false) != null)
                    continue;
                window = _serviceProvider.JoinableTaskFactory.Run (() =>
                    _serviceProvider.ShowToolWindowAsync (typeof (AnglrDetailsWindow), i, true, _serviceProvider.DisposalToken)) as AnglrDetailsWindow;
                if ((null == window) || (null == window.Frame))
                {
                    throw new NotSupportedException ("Cannot create details window");
                }
                window.Caption = caption;
                break;
            }
            if (content != null)
                ((AnglrDetailsWindowControl) window.Content).Content = content;
            init (window, content, this);
            return window;
        }

        public async Task<AnglrGetGetHierarchyItemResult> InvokeGetHierarchy (AnglrGetGetHierarchyItemParams anglrGetGetHierarchyItemParams)
        {
            return await AnglrLspClientAccessPoint.Instance.InvokeAsync<AnglrGetGetHierarchyItemResult>
                (AnglrMethods.AnglrGetGetHierarchyItemName, anglrGetGetHierarchyItemParams);
        }

        public async Task<AnglrGetDictionaryItemResult> InvokeGetDictionary (AnglrGetDictionaryItemParams anglrGetDictionaryItemParams)
        {
            return await AnglrLspClientAccessPoint.Instance.InvokeAsync<AnglrGetDictionaryItemResult> (AnglrMethods.AnglrGetDictionaryItemName, anglrGetDictionaryItemParams);
        }

        public async Task<AnglrGetParserStateItemResult> InvokeGetParserState (AnglrGetParserStateItemParams anglrGetParserStateItemParams)
        {
            return await AnglrLspClientAccessPoint.Instance.InvokeAsync<AnglrGetParserStateItemResult> (AnglrMethods.AnglrGetParserStateItemName, anglrGetParserStateItemParams);
        }

        public async Task<AnglrGetParserStatesInfoResult> InvokeGetParserStatesInfo (AnglrGetParserStatesInfoParams anglrGetParserStatesInfoParams)
        {
            return await AnglrLspClientAccessPoint.Instance.InvokeAsync<AnglrGetParserStatesInfoResult> (AnglrMethods.AnglrGetParserStatesInfoName, anglrGetParserStatesInfoParams);
        }

        public async Task<AnglrGetParserStateLinkResult> InvokeGetParserStateLink (AnglrGetParserStateLinkParams anglrGetParserStateLinkParams)
        {
            return await AnglrLspClientAccessPoint.Instance.InvokeAsync<AnglrGetParserStateLinkResult> (AnglrMethods.AnglrGetParserStateLinkName, anglrGetParserStateLinkParams);
        }

        public async Task<AnglrGetParserMagicNumberResult> InvokeGetParserMagicNumber (AnglrGetParserMagicNumberParams anglrGetParserMagicNumberParams)
        {
            return await AnglrLspClientAccessPoint.Instance.InvokeAsync<AnglrGetParserMagicNumberResult> (AnglrMethods.AnglrGetParserMagicNumberName, anglrGetParserMagicNumberParams);
        }

        public async Task<AnglrGetSyntaxTreeResult> InvokeGetSyntaxTree (AnglrGetSyntaxTreeParams anglrGetSyntaxTreeParams)
        {
            return await AnglrLspClientAccessPoint.Instance.InvokeAsync<AnglrGetSyntaxTreeResult> (AnglrMethods.AnglrGetSyntaxTreeName, anglrGetSyntaxTreeParams);
        }

        public async Task<AnglrGetParserSyntaxRuleResult> InvokeGetParserSyntaxRule (AnglrGetParserSyntaxRuleParams anglrGetParserSyntaxRuleParams)
        {
            return await AnglrLspClientAccessPoint.Instance.InvokeAsync<AnglrGetParserSyntaxRuleResult> (AnglrMethods.AnglrGetParserSyntaxRuleName, anglrGetParserSyntaxRuleParams);
        }

        public async Task<AnglrGetParserSyntaxRulesResult> InvokeGetParserSyntaxRules (AnglrGetParserSyntaxRulesParams anglrGetParserSyntaxRulesParams)
        {
            return await AnglrLspClientAccessPoint.Instance.InvokeAsync<AnglrGetParserSyntaxRulesResult> (AnglrMethods.AnglrGetParserSyntaxRulesName, anglrGetParserSyntaxRulesParams);
        }

        public async Task<AnglrGetItemNavigationInfoResponse> InvokeGetItemNavigationInfo (AnglrGetItemNavigationInfoRequest anglrGetItemNavigationInfoRequest)
        {
            return await AnglrLspClientAccessPoint.Instance.InvokeAsync<AnglrGetItemNavigationInfoResponse> (AnglrMethods.AnglrGetItemNavigationInfoName, anglrGetItemNavigationInfoRequest);
        }

        public async Task<AnglrGetCompileFragmentResponse> InvokeGetCompileFragment (AnglrGetCompileFragmentRequest anglrGetCompileFragmentRequest)
        {
            return await AnglrLspClientAccessPoint.Instance.InvokeAsync<AnglrGetCompileFragmentResponse> (AnglrMethods.AnglrGetCompileFragmentName, anglrGetCompileFragmentRequest);
        }

        public void FindAllDocumentsInSolution (AnglrProjectItemCallback func, string extension)
        {
            FindAllDocumentsInSolution (dTE, func, extension);
        }

        private void FindAllDocumentsInSolution (DTE2 dte, AnglrProjectItemCallback func, string extension)
        {
            ThreadHelper.ThrowIfNotOnUIThread ();
            Solution solution = dte.Solution;
            Projects projects = solution.Projects;

            foreach (Project project in projects)
            {
                FindAllProjectItems (project.ProjectItems, func, extension);
            }
        }

        private void FindAllProjectItems (ProjectItems projectItems, AnglrProjectItemCallback func, string extension)
        {
            ThreadHelper.ThrowIfNotOnUIThread ();
            if (projectItems == null)
                return;
            try
            {
                foreach (ProjectItem document in projectItems)
                {
                    switch (document.Kind)
                    {
                        case VSConstants.ItemTypeGuid.PhysicalFile_string:
                        {
                            string fileName = document.FileNames [0];
                            string extstr = Path.GetExtension (fileName);
                            if ((extstr != null) && (extstr == extension))
                                func (fileName, AnglrItemOperation.Get);
                        }
                        break;
                        case VSConstants.ItemTypeGuid.PhysicalFolder_string:
                            FindAllProjectItems (document.ProjectItems, func, extension);
                            break;
                        case VSConstants.CLSID.MiscellaneousFilesProject_string:
                            break;
                        default:
                            break;
                    }
                }
            }
            catch (Exception)
            {
                for (int i = 0; i < projectItems.Count; i++)
                {
                    try
                    {
                        ProjectItem item = projectItems.Item (i);
                        Document document = item.Document;
                        string fileName = item.FileNames [1];
                        string extstr = Path.GetExtension (fileName);
                        if ((extstr != null) && (extstr == extension))
                            func (fileName, AnglrItemOperation.Get);
                    }
                    catch (Exception)
                    {
                    }
                }
            }
        }

        public void NavigateAnglrFile (string fileName, int lineno, int column)
        {
            ThreadHelper.ThrowIfNotOnUIThread ();
            dTE.ItemOperations.OpenFile (fileName);
            TextSelection selection = dTE.ActiveDocument.Selection as TextSelection;
            selection.MoveToLineAndOffset (lineno, column);
        }

        public void RegisterAnglrFileEventHandler (AnglrProjectItemCallback func)
        {
            anglrProjectItemEvent += func;
        }

        public async Task OpenAnglrFileAsync (string fileName)
        {
            if (openAnglrFiles.Contains (fileName))
                return;
            openAnglrFiles.Add (fileName);
            await anglrProjectItemEvent?.Invoke (fileName, AnglrItemOperation.Add);
        }

        public async Task CloseAnglrFileAsync (string fileName)
        {
            openAnglrFiles.Remove (fileName);
            await anglrProjectItemEvent?.Invoke (fileName, AnglrItemOperation.Remove);
        }

        public async Task GetOpenAnglrFilesAsync ()
        {
            foreach (var file in openAnglrFiles)
                await anglrProjectItemEvent?.Invoke (file, AnglrItemOperation.Get);
        }
    }

    public interface SAnglrLangService
    {
    }

    public interface IAnglrLangService
    {
        IComponentModel ComponentModel { get; }
        ITextEditorFactoryService TextEditorFactoryService { get; }
        ITextBufferFactoryService TextBufferFactoryService { get; }
        IContentTypeRegistryService ContentTypeRegistryService { get; }
        IOutliningManagerService OutliningManagerService { get; }
        IAnglrLogger AnglrLogger { get; }
        AnglrDetailsWindow CreateDetailsWindowInstance (UIElement content, string caption, Func<AnglrDetailsWindow, UIElement, IAnglrLangService, bool> init);
        Task<AnglrGetGetHierarchyItemResult> InvokeGetHierarchy (AnglrGetGetHierarchyItemParams request);
        Task<AnglrGetDictionaryItemResult> InvokeGetDictionary (AnglrGetDictionaryItemParams request);
        Task<AnglrGetParserStateItemResult> InvokeGetParserState (AnglrGetParserStateItemParams request);
        Task<AnglrGetParserStatesInfoResult> InvokeGetParserStatesInfo (AnglrGetParserStatesInfoParams request);
        Task<AnglrGetParserStateLinkResult> InvokeGetParserStateLink (AnglrGetParserStateLinkParams request);
        Task<AnglrGetParserMagicNumberResult> InvokeGetParserMagicNumber (AnglrGetParserMagicNumberParams request);
        Task<AnglrGetSyntaxTreeResult> InvokeGetSyntaxTree (AnglrGetSyntaxTreeParams request);
        Task<AnglrGetParserSyntaxRuleResult> InvokeGetParserSyntaxRule (AnglrGetParserSyntaxRuleParams request);
        Task<AnglrGetParserSyntaxRulesResult> InvokeGetParserSyntaxRules (AnglrGetParserSyntaxRulesParams request);
        Task<AnglrGetItemNavigationInfoResponse> InvokeGetItemNavigationInfo (AnglrGetItemNavigationInfoRequest request);
        Task<AnglrGetCompileFragmentResponse> InvokeGetCompileFragment (AnglrGetCompileFragmentRequest request);
        void RegisterAnglrFileEventHandler (AnglrProjectItemCallback func);
        Task OpenAnglrFileAsync (string fileName);
        Task CloseAnglrFileAsync (string fileName);
        Task GetOpenAnglrFilesAsync ();
        void FindAllDocumentsInSolution (AnglrProjectItemCallback func, string extension);
        void NavigateAnglrFile (string fileName, int lineno, int column);
    }

    public class AnglrLogService : SAnglrLogService, IAnglrLogService
    {
        private IAsyncServiceProvider _serviceProvider = null;
        private DTE2 dTE = null;
        public IAnglrLogger Logger { get; private set; }

        public AnglrLogService (IAsyncServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            Logger = new VsAnglrLogger (CreatePaneDTE ("Anglr Extension"));
        }

        private OutputWindowPane CreatePaneDTE (string title)
        {
            ThreadHelper.ThrowIfNotOnUIThread ();
            dTE = Package.GetGlobalService (typeof (DTE)) as DTE2;
            OutputWindowPanes panes = dTE.ToolWindows.OutputWindow.OutputWindowPanes;
            OutputWindowPane pane = null;
            try
            {
                pane = panes.Item (title);
            }
            catch (ArgumentException)
            {
                pane = panes.Add (title);
            }
            pane?.Activate ();
            return pane;
        }

        public async Task InitializeAsync (CancellationToken cancellationToken)
        {
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync (cancellationToken);
        }

        public void Log (AnglrLogLevel logLevel, string message, Exception exception = null)
        {
            _ = _LogAsync (logLevel, message, exception);
        }

        private async Task<int> _LogAsync (AnglrLogLevel logLevel, string message, Exception exception = null)
        {
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync ();
            Logger?.Log (logLevel, message);
            return 0;
        }
    }

    public interface SAnglrLogService
    {
    }

    public interface IAnglrLogService
    {
        IAnglrLogger Logger { get; }
    }

    public class VsAnglrLogger : AnglrLoggerBase
    {
        private readonly OutputWindowPane _pane;

        public VsAnglrLogger (OutputWindowPane pane)
        {
            _pane = pane;
        }

        protected override void Write (AnglrLogLevel level, string message, uint flags = 0)
        {
            //await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync ();
            try
            {
                if ((flags & WriteOnlyMsg) != 0)
                    _pane.OutputString ($"{message}\n");
                else
                    _pane.OutputString ($"{DateTime.Now.ToString ("yyyy-MM-dd HH:mm:ss.fff")} - {level} - {message}\n");
            }
            catch (Exception e)
            {
            }
        }

        protected override void WriteLine (AnglrLogLevel level, string message, uint flags = 0) => Write (level, message, flags);
    }
}
