using Microsoft.VisualStudio.Editor;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.TextManager.Interop;
using Microsoft.VisualStudio.Utilities;
using Microsoft.VisualStudio.LanguageServer.Client;
using System;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.Text;

namespace AnglrLSPExtension
{
	[Export (typeof (IVsTextViewCreationListener))]
	[ContentType ("anglr")]
	[TextViewRole (PredefinedTextViewRoles.Editable)]
	class AnglrViewCreationlistener : IVsTextViewCreationListener
	{
        [Import]
        internal ITextDocumentFactoryService TextDocumentFactory = null;

		[Import]
		internal IVsEditorAdaptersFactoryService AdaptersFactory = null;

		[Import]
		internal IContentTypeRegistryService ContentTypeRegistryService { get; set; }

        [Import]
        internal ILanguageClient LanguageClient { get; }

		public void VsTextViewCreated (IVsTextView textViewAdapter)
		{
            try
            {
                if (!(AdaptersFactory.GetWpfTextView (textViewAdapter) is IWpfTextView view))
                    return;

                Microsoft.VisualStudio.Text.ITextBuffer buffer = view.TextBuffer;
                if (buffer == null)
                    return;

                if (!TextDocumentFactory.TryGetTextDocument (buffer, out var document))
                    return;

                if (document == null)
                    return;

                document.UpdateDirtyState (true, DateTime.Now);

                string fileName = document.FilePath;
                if (fileName == null)
                    return;

                if (LanguageClient is AnglrLspClientAccessPoint)
                    ((AnglrLspClientAccessPoint) LanguageClient).Log ($"text view created for: {fileName}");
            }
            catch (Exception)
            {
            }
        }
    }
}
