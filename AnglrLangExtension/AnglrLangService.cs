using EnvDTE80;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using Microsoft.VisualStudio.Shell;
using IAsyncServiceProvider = Microsoft.VisualStudio.Shell.IAsyncServiceProvider;
using System.Threading;
using Microsoft.VisualStudio.Shell.Interop;
using EnvDTE;
using Anglr.Parser;
using Microsoft.VisualStudio;
using AnglrJsonRpcMethods;
using AnglrLogLibrary;
using System.Runtime.CompilerServices;

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

    public class AnglrLangService : SAnglrLangService, IAnglrLangService
    {
        private HashSet<string> openAnglrFiles = new HashSet<string> ();
        private IAsyncServiceProvider _serviceProvider = null;
        public IAnglrLogService AnglrLogService { get; private set; }

        public IAnglrLogger AnglrLogger { get; private set; }

        private DTE2 dTE = null;

        public event AnglrProjectItemCallback anglrProjectItemEvent;

        public AnglrLangService (IAsyncServiceProvider serviceProvider, IAnglrLogService anglrLogService)
        {
            _serviceProvider = serviceProvider;
            AnglrLogService = anglrLogService;
            AnglrLogger = AnglrLogService?.Logger ?? new VoidAnglrLogger ();
            //AnglrLogService = serviceProvider.GetServiceAsync (typeof (AnglrLogService)) as IAnglrLogService;
            dTE = Package.GetGlobalService (typeof (DTE)) as DTE2;
        }

        public async Task InitializeAsync (CancellationToken cancellationToken)
        {
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync (cancellationToken);
        }

        public void Log (AnglrLogLevel logLevel, string message, string indent = null)
        {
            AnglrLogService?.Logger?.Log (logLevel, message);
        }

        public AnglrGetGetHierarchyItemResult InvokeGetHierarchy (AnglrGetGetHierarchyItemParams anglrGetGetHierarchyItemParams)
        {
            return
                    ThreadHelper.JoinableTaskFactory.Run
                    (
                        () => AnglrLspClientAccessPoint.Instance.InvokeAsync<AnglrGetGetHierarchyItemResult> (AnglrMethods.AnglrGetGetHierarchyItemName, anglrGetGetHierarchyItemParams)
                    );
        }

        public AnglrGetDictionaryItemResult InvokeGetDictionary (AnglrGetDictionaryItemParams anglrGetDictionaryItemParams)
        {
            return
                    ThreadHelper.JoinableTaskFactory.Run
                    (
                        () => AnglrLspClientAccessPoint.Instance.InvokeAsync<AnglrGetDictionaryItemResult> (AnglrMethods.AnglrGetDictionaryItemName, anglrGetDictionaryItemParams)
                    );
        }

        public AnglrGetParserStateItemResult InvokeGetParserState (AnglrGetParserStateItemParams anglrGetParserStateItemParams)
        {
            return
                    ThreadHelper.JoinableTaskFactory.Run
                    (
                        () => AnglrLspClientAccessPoint.Instance.InvokeAsync<AnglrGetParserStateItemResult> (AnglrMethods.AnglrGetParserStateItemName, anglrGetParserStateItemParams)
                    );
        }

        public AnglrGetParserStatesInfoResult InvokeGetParserStatesInfo (AnglrGetParserStatesInfoParams anglrGetParserStatesInfoParams)
        {
            return
                    ThreadHelper.JoinableTaskFactory.Run
                    (
                        () => AnglrLspClientAccessPoint.Instance.InvokeAsync<AnglrGetParserStatesInfoResult> (AnglrMethods.AnglrGetParserStatesInfoName, anglrGetParserStatesInfoParams)
                    );
        }

        public AnglrGetParserStateLinkResult InvokeGetParserStateLink (AnglrGetParserStateLinkParams anglrGetParserStateLinkParams)
        {
            return
                    ThreadHelper.JoinableTaskFactory.Run
                    (
                        () => AnglrLspClientAccessPoint.Instance.InvokeAsync<AnglrGetParserStateLinkResult> (AnglrMethods.AnglrGetParserStateLinkName, anglrGetParserStateLinkParams)
                    );
        }

        public AnglrGetParserMagicNumberResult InvokeGetParserMagicNumber (AnglrGetParserMagicNumberParams anglrGetParserMagicNumberParams)
        {
            return
                    ThreadHelper.JoinableTaskFactory.Run
                    (
                        () => AnglrLspClientAccessPoint.Instance.InvokeAsync<AnglrGetParserMagicNumberResult> (AnglrMethods.AnglrGetParserMagicNumberName, anglrGetParserMagicNumberParams)
                    );
        }

        public AnglrGetParserSyntaxRuleResult InvokeGetParserSyntaxRule (AnglrGetParserSyntaxRuleParams anglrGetParserSyntaxRuleParams)
        {
            return
                    ThreadHelper.JoinableTaskFactory.Run
                    (
                        () => AnglrLspClientAccessPoint.Instance.InvokeAsync<AnglrGetParserSyntaxRuleResult> (AnglrMethods.AnglrGetParserSyntaxRuleName, anglrGetParserSyntaxRuleParams)
                    );
        }

        public AnglrGetParserSyntaxRulesResult InvokeGetParserSyntaxRules (AnglrGetParserSyntaxRulesParams anglrGetParserSyntaxRulesParams)
        {
            return
                    ThreadHelper.JoinableTaskFactory.Run
                    (
                        () => AnglrLspClientAccessPoint.Instance.InvokeAsync<AnglrGetParserSyntaxRulesResult> (AnglrMethods.AnglrGetParserSyntaxRulesName, anglrGetParserSyntaxRulesParams)
                    );
        }

        public AnglrGetItemNavigationInfoResponse InvokeGetItemNavigationInfo (AnglrGetItemNavigationInfoRequest anglrGetItemNavigationInfoRequest)
        {
            return
                    ThreadHelper.JoinableTaskFactory.Run
                    (
                        () => AnglrLspClientAccessPoint.Instance.InvokeAsync<AnglrGetItemNavigationInfoResponse> (AnglrMethods.AnglrGetItemNavigationInfoName, anglrGetItemNavigationInfoRequest)
                    );
        }

        public AnglrGetCompileFragmentResponse InvokeGetCompileFragment (AnglrGetCompileFragmentRequest anglrGetCompileFragmentRequest)
        {
            return
                    ThreadHelper.JoinableTaskFactory.Run
                    (
                        () => AnglrLspClientAccessPoint.Instance.InvokeAsync<AnglrGetCompileFragmentResponse> (AnglrMethods.AnglrGetCompileFragmentName, anglrGetCompileFragmentRequest)
                    );
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
        IAnglrLogger AnglrLogger { get; }
        AnglrGetGetHierarchyItemResult InvokeGetHierarchy (AnglrGetGetHierarchyItemParams request);
        AnglrGetDictionaryItemResult InvokeGetDictionary (AnglrGetDictionaryItemParams request);
        AnglrGetParserStateItemResult InvokeGetParserState (AnglrGetParserStateItemParams request);
        AnglrGetParserStatesInfoResult InvokeGetParserStatesInfo (AnglrGetParserStatesInfoParams request);
        AnglrGetParserStateLinkResult InvokeGetParserStateLink (AnglrGetParserStateLinkParams request);
        AnglrGetParserMagicNumberResult InvokeGetParserMagicNumber (AnglrGetParserMagicNumberParams request);
        AnglrGetParserSyntaxRuleResult InvokeGetParserSyntaxRule (AnglrGetParserSyntaxRuleParams request);
        AnglrGetParserSyntaxRulesResult InvokeGetParserSyntaxRules (AnglrGetParserSyntaxRulesParams request);
        AnglrGetItemNavigationInfoResponse InvokeGetItemNavigationInfo (AnglrGetItemNavigationInfoRequest request);
        AnglrGetCompileFragmentResponse InvokeGetCompileFragment (AnglrGetCompileFragmentRequest request);
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

        protected override async void Write (AnglrLogLevel level, string message, uint flags = 0)
        {
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync ();
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
