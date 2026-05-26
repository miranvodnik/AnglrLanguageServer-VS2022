using AnglrLogLibrary;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.TextManager.Interop;
using Microsoft.VisualStudio.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;

namespace AnglrLangExtension
{
	/// <summary>
	/// Classifier provider. It adds the classifier to the set of classifiers.
	/// </summary>
	[Export (typeof (IClassifierProvider))]
	[ContentType ("anglr")] // This classifier applies to all text files.
	internal class AnglrEditorClassifierProvider : IClassifierProvider
	{
		// Disable "Field is never assigned to..." compiler's warning. Justification: the field is assigned by MEF.
#pragma warning disable 649

		/// <summary>
		/// Classification registry to be used for getting a reference
		/// to the custom classification type later.
		/// </summary>
		[Import]
		private IClassificationTypeRegistryService classificationRegistry;
		private bool debug = true;

        private IAnglrLangService anglrLangService = null;
		private IAnglrLogger anglrLogger = null;

		internal void Log (AnglrLogLevel logLevel, string message) => anglrLogger?.Log (logLevel, message);

		public string GetFFN (ITextBuffer buffer)
		{
			Microsoft.VisualStudio.Shell.ThreadHelper.ThrowIfNotOnUIThread ();
			if (buffer == null)
			{
				return null;
			}

			buffer.Properties.TryGetProperty (typeof (Microsoft.VisualStudio.TextManager.Interop.IVsTextBuffer), out Microsoft.VisualStudio.TextManager.Interop.IVsTextBuffer bufferAdapter);
			if (bufferAdapter != null)
			{
				Microsoft.VisualStudio.Shell.Interop.IPersistFileFormat persistFileFormat = bufferAdapter as Microsoft.VisualStudio.Shell.Interop.IPersistFileFormat;
				string ppzsFilename = null;
				if (persistFileFormat != null)
				{
					persistFileFormat.GetCurFile (out ppzsFilename, out uint _);
				}

				return ppzsFilename;
			}
			return null;
		}

#pragma warning restore 649

		#region IClassifierProvider

		/// <summary>
		/// Gets a classifier for the given text buffer.
		/// </summary>
		/// <param name="buffer">The <see cref="ITextBuffer"/> to classify.</param>
		/// <returns>A classifier for the text buffer, or null if the provider cannot do so in its current state.</returns>
		public IClassifier GetClassifier (ITextBuffer buffer)
		{
			try
			{
                anglrLangService = anglrLangService ?? ThreadHelper.JoinableTaskFactory.Run (() => AnglrLangExtensionPackage.Instance.GetServiceAsync (typeof (SAnglrLangService))) as IAnglrLangService;
				anglrLogger = anglrLogger ?? anglrLangService?.AnglrLogger ?? new VoidAnglrLogger ();

            }
			catch (Exception e)
			{

			}
			string fileName = GetFFN (buffer);
			if (fileName != null)
				anglrLogger?.InfoLine ($"opening file: {fileName}");
			else
                anglrLogger?.ErrorLine ("cannot get file name");
			return buffer.Properties.GetOrCreateSingletonProperty<AnglrEditorClassifier> (creator: () => new AnglrEditorClassifier (this.classificationRegistry, fileName));
		}

		#endregion
	}
}
