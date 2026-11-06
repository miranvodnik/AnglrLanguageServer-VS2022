using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Utilities;
using System.ComponentModel.Composition;

namespace AnglrLangExtension
{
    [Export (typeof (IWpfTextViewCreationListener))]
    [ContentType ("anglr-dbg")]
    [TextViewRole (PredefinedTextViewRoles.Document)]
    internal class PdaCellAdornmentTextViewCreationListener : IWpfTextViewCreationListener
    {
        [Export (typeof (AdornmentLayerDefinition))]
        [Name ("PdaCellAdornment")]
        [Order (After = PredefinedAdornmentLayers.Selection, Before = PredefinedAdornmentLayers.Text)]
        private AdornmentLayerDefinition pdaCellAdornmentLayer;

        public void TextViewCreated (IWpfTextView textView)
        {
            new PdaCellAdornment (textView);
        }
    }
}
