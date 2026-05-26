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
    [Guid ("7c6a5772-f0c7-40b2-89f1-7235e02e9cb4")]
    public class AnglrStatesWindow : ToolWindowPane
    {
        public AnglrStatesWindowControl AnglrStatesControl { get; private set; }
        /// <summary>
        /// Initializes a new instance of the <see cref="AnglrStatesWindow"/> class.
        /// </summary>
        public AnglrStatesWindow () : base (null)
        {
            this.Caption = "Anglr States Explorer";

            // This is the user control hosted by the tool window; Note that, even if this class implements IDisposable,
            // we are not calling Dispose on this object. This is because ToolWindowPane calls Dispose on
            // the object returned by the Content property.
            AnglrStatesControl = new AnglrStatesWindowControl ();
            base.Content = AnglrStatesControl;
        }
    }
}
