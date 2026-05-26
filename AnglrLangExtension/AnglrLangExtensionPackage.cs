using AnglrLogLibrary;
using AnglrBreakPointDBLibrary;
using EnvDTE;
using EnvDTE80;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.OLE.Interop;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Events;
using Microsoft.VisualStudio.Shell.Interop;
using stdole;
using System;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using Constants = EnvDTE.Constants;
using Task = System.Threading.Tasks.Task;

namespace AnglrLangExtension
{
    /// <summary>
    /// This is the class that implements the package exposed by this assembly.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The minimum requirement for a class to be considered a valid package for Visual Studio
    /// is to implement the IVsPackage interface and register itself with the shell.
    /// This package uses the helper classes defined inside the Managed Package Framework (MPF)
    /// to do it: it derives from the Package class that provides the implementation of the
    /// IVsPackage interface and uses the registration attributes defined in the framework to
    /// register itself and its components with the shell. These attributes tell the pkgdef creation
    /// utility what data to put into .pkgdef file.
    /// </para>
    /// <para>
    /// To get loaded into VS, the package must be referred by &lt;Asset Type="Microsoft.VisualStudio.VsPackage" ...&gt; in .vsixmanifest file.
    /// </para>
    /// </remarks>

    using SolutionEvents = Microsoft.VisualStudio.Shell.Events.SolutionEvents;

    [PackageRegistration (UseManagedResourcesOnly = true, AllowsBackgroundLoading = true)]
    [Guid (AnglrLangExtensionPackage.PackageGuidString)]
    [ProvideMenuResource ("Menus.ctmenu", 1)]
    [ProvideToolWindow (typeof (AnglrLangWindow),
        Style = Microsoft.VisualStudio.Shell.VsDockStyle.Tabbed,
        Window = "3AE79031-E1BC-11D0-8F78-00A0C9110057")]
    [ProvideToolWindowVisibility (typeof (AnglrLangWindow), VSConstants.UICONTEXT.SolutionBuilding_string)]
    [ProvideToolWindow (typeof (AnglrStatesWindow),
        Style = Microsoft.VisualStudio.Shell.VsDockStyle.Tabbed,
        Window = "038CAAFD-E0FB-4F03-B929-802FFC634DB8")]
    [ProvideToolWindowVisibility (typeof (AnglrStatesWindow), VSConstants.UICONTEXT.SolutionBuilding_string)]
    [ProvideToolWindow (typeof (AnglrDebugPanelWindow),
        Style = Microsoft.VisualStudio.Shell.VsDockStyle.Tabbed,
        Window = "E3F738C9-8D10-49F8-A096-60B87F0452AA")]
    [ProvideToolWindowVisibility (typeof (AnglrDebugPanelWindow), VSConstants.UICONTEXT.SolutionBuilding_string)]
    [ProvideToolWindow (typeof (AnglrDetailsWindow),
        Style = Microsoft.VisualStudio.Shell.VsDockStyle.Tabbed,
        MultiInstances = true)]
    [ProvideToolWindowVisibility (typeof (AnglrDetailsWindow), VSConstants.UICONTEXT.SolutionBuilding_string)]
    [ProvideService (typeof (SAnglrLangService), IsAsyncQueryable = true)]
    [ProvideService (typeof (SAnglrLogService), IsAsyncQueryable = true)]

    public sealed class AnglrLangExtensionPackage : AsyncPackage
    {
        /// <summary>
        /// AnglrLangExtensionPackage GUID string.
        /// </summary>
        public const string PackageGuidString = "c2bf0e6f-dff2-4395-aa5f-5ac33cd059cb";
        public const string ErrorListGuid = "0D13BE59-1594-4EBF-9AE9-782DE2F6DA24";
        // {0834ADE6-C7EB-4CCA-BFEB-2521745249B0}
        public static AnglrLangExtensionPackage Instance { get; private set; } = null;
        public static bool debug { get; set; } = true;
        private IAnglrLogService anglrLogService = null;
        private IAnglrLangService anglrLangService = null;
        private IAnglrLogger Logger = null;

        #region Package Members

        /// <summary>
        /// Initialization of the package; this method is called right after the package is sited, so this is the place
        /// where you can put all the initialization code that rely on services provided by VisualStudio.
        /// </summary>
        /// <param name="cancellationToken">A cancellation token to monitor for initialization cancellation, which can occur when VS is shutting down.</param>
        /// <param name="progress">A provider for progress updates.</param>
        /// <returns>A task representing the async work of package initialization, or an already completed task if there is none. Do not return null from this method.</returns>
        protected override async Task InitializeAsync (CancellationToken cancellationToken, IProgress<ServiceProgressData> progress)
        {
            await base.InitializeAsync (cancellationToken, progress);

            await this.JoinableTaskFactory.SwitchToMainThreadAsync (cancellationToken);

            SolutionEvents.OnBeforeOpenSolution += SolutionEvents_OnBeforeOpenSolution;
            SolutionEvents.OnAfterOpenSolution += SolutionEvents_OnAfterOpenSolution;

            // When initialized asynchronously, the current thread may be a background thread at this point.
            // Do any initialization that requires the UI thread after switching to the UI thread.

            AddService (typeof (SAnglrLogService), CreateAnglrLogServiceAsync, true);
            anglrLogService = await GetServiceAsync (typeof (SAnglrLogService)) as IAnglrLogService;
            Logger = anglrLogService?.Logger ?? new VoidAnglrLogger ();
            Logger?.InfoLine ("add AnglrLangExtension service");
            AddService (typeof (SAnglrLangService), CreateAnglrLangServiceAsync, true);
            anglrLangService = await GetServiceAsync (typeof (SAnglrLangService)) as IAnglrLangService;
            Logger?.InfoLine ("added AnglrLangExtension service");

            await AnglrLangWindowCommand.InitializeAsync (this);
            await AnglrStatesWindowCommand.InitializeAsync (this);
            await AnglrDebugPanelWindowCommand.InitializeAsync (this);
            await AnglrDetailsWindowCommand.InitializeAsync (this);

            Instance = this;
            if (!AnglrBreakPointDB.Load ())
            {
                if (AnglrBreakPointDB.AnglrBreakPointDBException != null)
                    Logger?.InfoLine (AnglrBreakPointDB.AnglrBreakPointDBException, "cannot load AnglrBreakPointDB");
                else
                    Logger?.InfoLine ("cannot load AnglrBreakPointDB");
            }
            else
                Logger?.InfoLine ("loaded AnglrBreakPointDB");
        }

        protected override void Dispose (bool disposing)
        {
            base.Dispose (disposing);
            AnglrLangWindowCommand.Instance.Dispose ();
            AnglrBreakPointDB.Save ();
        }

        private void SolutionEvents_OnBeforeOpenSolution (object sender, BeforeOpenSolutionEventArgs e)
        {
        }

        private void SolutionEvents_OnAfterOpenSolution (object sender, OpenSolutionEventArgs e)
        {
        }

        private async System.Threading.Tasks.Task<object> CreateAnglrLangServiceAsync (IAsyncServiceContainer container, CancellationToken cancellationToken, Type serviceType)
        {
            AnglrLangService service = new AnglrLangService (this, anglrLogService);
            await service.InitializeAsync (cancellationToken);
            return service;
        }

        private async System.Threading.Tasks.Task<object> CreateAnglrLogServiceAsync (IAsyncServiceContainer container, CancellationToken cancellationToken, Type serviceType)
        {
            AnglrLogService service = new AnglrLogService (this);
            await service.InitializeAsync (cancellationToken);
            return service;
        }
        #endregion

        public void RetrieveMiscellaneousFiles ()
        {
            DTE dte = (DTE) System.Runtime.InteropServices.Marshal.GetActiveObject ("VisualStudio.DTE.16.0");
            var miscFiles = ((object []) dte.Solution.GetType ().InvokeMember ("GetMiscellaneousFiles", System.Reflection.BindingFlags.InvokeMethod, null, dte.Solution, null));
            foreach (string miscFile in miscFiles)
            {
                Logger?.DebugLine (miscFile);
            }
        }

        public async Task RetrieveAnglrFilesAsync ()
        {
            await this.JoinableTaskFactory.SwitchToMainThreadAsync ();

            IVsRunningDocumentTable vsRunningDocumentTable = ((IServiceContainer) this).GetService (typeof (SVsRunningDocumentTable)) as IVsRunningDocumentTable;
            if (vsRunningDocumentTable == null)
            {
                Logger?.WarnLine ($"service RDT not retrieved");
                return;
            }

            IEnumRunningDocuments enumRunningDocuments = null;
            if (vsRunningDocumentTable.GetRunningDocumentsEnum (out enumRunningDocuments) != VSConstants.S_OK)
            {
                Logger?.WarnLine ($"cannot enum RDT documents");
                return;
            }
            Logger?.DebugLine ($"RDT documents enumerated");

            uint [] cookies = new uint [1];
            uint cookiesRetrieved = 0;
            while (enumRunningDocuments.Next (1, cookies, out cookiesRetrieved) == VSConstants.S_OK)
            {
                enumRunningDocuments.Skip (1);
                if (cookiesRetrieved < 1)
                {
                    Logger?.DebugLine ($"number of cookies is {cookiesRetrieved}");
                    continue;
                }
                uint flags = 0;
                uint rdlock = 0;
                uint edlock = 0;
                string docPathName = "";
                if (vsRunningDocumentTable.GetDocumentInfo (cookies [0], out flags, out rdlock, out edlock, out docPathName, out _, out _, out _) != VSConstants.S_OK)
                {
                    Logger?.WarnLine ($"cannot get document info, cookie is {cookies [0]}");
                    continue;
                }
                Logger?.DebugLine ($"document: {docPathName}");
                Logger?.DebugLine ($"\tflags:      {flags:X08}");
                Logger?.DebugLine ($"\tread locks: {rdlock}");
                Logger?.DebugLine ($"\tedit locks: {edlock}");
            }
        }
    }
}
