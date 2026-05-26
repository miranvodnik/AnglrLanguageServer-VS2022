using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using System;
using System.ComponentModel.Design;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using Task = System.Threading.Tasks.Task;
using System.Windows.Forms;

namespace AnglrLangExtension
{
    /// <summary>
    /// Command handler
    /// </summary>
    internal sealed class AnglrLangWindowCommand
    {
        /// <summary>
        /// Command ID.
        /// </summary>
        public const int CommandId = 0x0100;

        /// <summary>
        /// Command menu group (command set GUID).
        /// </summary>
        public static readonly Guid CommandSet = new Guid ("ff536f65-200c-4ffb-95c9-97e156a39e2a");

        /// <summary>
        /// VS Package that provides this command, not null.
        /// </summary>
        private readonly AsyncPackage package;

        public const string guidAnglrLangExtensionPackageCmdSet = "ff536f65-200c-4ffb-95c9-97e156a39e2a";  // get the GUID from the .vsct file
        public const uint cmdidWindowsMedia = 0x100;
        public const int cmdidWindowsMediaOpen = 0x132;
        public const int ToolbarID = 0x1000;
        public AnglrLangWindow window;

        /// <summary>
        /// Initializes a new instance of the <see cref="AnglrLangWindowCommand"/> class.
        /// Adds our command handlers for menu (commands must exist in the command table file)
        /// </summary>
        /// <param name="package">Owner package, not null.</param>
        /// <param name="commandService">Command service to add command to, not null.</param>
        private AnglrLangWindowCommand (AsyncPackage package, OleMenuCommandService commandService)
        {
            this.package = package ?? throw new ArgumentNullException (nameof (package));
            commandService = commandService ?? throw new ArgumentNullException (nameof (commandService));

            var menuCommandID = new CommandID (CommandSet, CommandId);
            var menuItem = new MenuCommand (this.Execute, menuCommandID);
            commandService.AddCommand (menuItem);
        }

        /// <summary>
        /// Gets the instance of the command.
        /// </summary>
        public static AnglrLangWindowCommand Instance
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the service provider from the owner package.
        /// </summary>
        private Microsoft.VisualStudio.Shell.IAsyncServiceProvider ServiceProvider
        {
            get
            {
                return this.package;
            }
        }

        /// <summary>
        /// Initializes the singleton instance of the command.
        /// </summary>
        /// <param name="package">Owner package, not null.</param>
        public static async Task InitializeAsync (AsyncPackage package)
        {
            // Switch to the main thread - the call to AddCommand in AnglrLangWindowCommand's constructor requires
            // the UI thread.
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync (package.DisposalToken);

            OleMenuCommandService commandService = await package.GetServiceAsync ((typeof (IMenuCommandService))) as OleMenuCommandService;
            Instance = new AnglrLangWindowCommand (package, commandService);
        }

        /// <summary>
        /// Shows the tool window when the menu item is clicked.
        /// </summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event args.</param>
        private void Execute (object sender, EventArgs e)
        {
            this.package.JoinableTaskFactory.RunAsync (async delegate
            {
                window = (AnglrLangWindow) await this.package.ShowToolWindowAsync (typeof (AnglrLangWindow), 0, true, this.package.DisposalToken);
                if ((null == window) || (null == window.Frame))
                {
                    throw new NotSupportedException ("Cannot create tool window");
                }
                window.AnglrLangControl.package = package;

                // Create the handles for the toolbar command.
                var mcs = await this.ServiceProvider.GetServiceAsync (typeof (IMenuCommandService)) as OleMenuCommandService;
                var toolbarbtnCmdID = new CommandID (new Guid (AnglrLangWindowCommand.guidAnglrLangExtensionPackageCmdSet), AnglrLangWindowCommand.cmdidWindowsMediaOpen);
                var menuItem = new MenuCommand (new EventHandler (ButtonHandler), toolbarbtnCmdID);
                mcs.AddCommand (menuItem);
            });
        }

        private void ButtonHandler (object sender, EventArgs arguments)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog ();
            DialogResult result = openFileDialog.ShowDialog ();
        }

        public void Dispose () => window?.Dispose ();
    }
}
