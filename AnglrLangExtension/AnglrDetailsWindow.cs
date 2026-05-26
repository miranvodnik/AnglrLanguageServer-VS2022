using Microsoft.VisualStudio.Shell;
using System;
using System.Runtime.InteropServices;

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
    [Guid ("e623ec18-a83d-4c9e-a5a1-e0d17d64f5ae")]
    public class AnglrDetailsWindow : ToolWindowPane
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AnglrDetailsWindow"/> class.
        /// </summary>
        public AnglrDetailsWindow () : base (null)
        {
            this.Caption = "AnglrDetailsWindow";

            // This is the user control hosted by the tool window; Note that, even if this class implements IDisposable,
            // we are not calling Dispose on this object. This is because ToolWindowPane calls Dispose on
            // the object returned by the Content property.
            this.Content = new AnglrDetailsWindowControl ();
        }
    }
}
