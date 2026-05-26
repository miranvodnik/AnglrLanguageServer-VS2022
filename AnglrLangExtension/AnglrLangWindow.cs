using Microsoft.VisualStudio.Shell;
using System;
using System.Runtime.InteropServices;
using System.ComponentModel.Design;
using Microsoft.VisualStudio.Shell.Interop;

namespace AnglrLangExtension
{
    /// <summary>
    /// This class implements the tool window exposed by this package and hosts a user control.
    /// </summary>
    /// <remarks>
    /// In Visual Studio tool windows are composed of a frame (implemented by the shell) and a pane,
    /// usually implemented by the package implementer.
    /// <para>
    /// This class derives from the ToolWindowPane class provided from the MPF in order to use its
    /// implementation of the IVsUIElementPane interface.
    /// </para>
    /// </remarks>
    [Guid ("cafaac0d-76bb-4032-81ac-3514f71d7e79")]
    public class AnglrLangWindow : ToolWindowPane
    {
        public AnglrLangWindowControl AnglrLangControl { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="AnglrLangWindow"/> class.
        /// </summary>
        public AnglrLangWindow () : base (null)
        {
            this.Caption = "Anglr Explorer";

            // This is the user control hosted by the tool window; Note that, even if this class implements IDisposable,
            // we are not calling Dispose on this object. This is because ToolWindowPane calls Dispose on
            // the object returned by the Content property.
            this.BitmapResourceID = 301;
            this.BitmapIndex = 1;
            AnglrLangControl = new AnglrLangWindowControl ();
            base.Content = AnglrLangControl;
            this.ToolBar = new CommandID (new Guid (AnglrLangWindowCommand.guidAnglrLangExtensionPackageCmdSet), AnglrLangWindowCommand.ToolbarID);
            this.ToolBarLocation = (int) VSTWT_LOCATION.VSTWT_TOP;
        }

        public new void Dispose ()
        {
            base.Dispose (false);
            AnglrLangControl?.Dispose ();
        }
    }
}