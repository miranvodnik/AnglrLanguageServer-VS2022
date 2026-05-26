using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.LanguageServer.Protocol;
using Microsoft.VisualStudio.Threading;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.IO;
using AnglrJsonRpcMethods;

namespace AnglrLSPExtension
{
	/// <summary>
	/// Classifier that classifies all text as an instance of the "AnglrEditorClassifier" classification type.
	/// </summary>
	internal class AnglrEditorClassifier : IClassifier
	{
		/// <summary>
		/// Classification type.
		/// </summary>
		private readonly IClassificationType anglrUndefinedClassificationTypeType;
		private readonly IClassificationType anglrReservedWordClassificationTypeType;
		private readonly IClassificationType anglrGeneralPartNameClassificationTypeType;
		private readonly IClassificationType anglrDeclarationsPartNameClassificationTypeType;
		private readonly IClassificationType anglrScannerPartNameClassificationTypeType;
        private readonly IClassificationType anglrLexerPartNameClassificationTypeType;
        private readonly IClassificationType anglrParserPartNameClassificationTypeType;
        private readonly IClassificationType anglrPartBracketClassificationTypeType;
		private readonly IClassificationType anglrBracketClassificationTypeType;
		private readonly IClassificationType anglrRegularExpressionClassificationTypeType;
		private readonly IClassificationType anglrAttributeNameClassificationTypeType;
		private readonly IClassificationType anglrPropertyNameClassificationTypeType;
		private readonly IClassificationType anglrOperatorSignatureClassificationTypeType;
		private readonly IClassificationType anglrPropertyValueClassificationTypeType;
		private readonly IClassificationType anglrNonTerminalNameClassificationTypeType;
		private readonly IClassificationType anglrNonTerminalNameDefClassificationTypeType;
		private readonly IClassificationType anglrNonTerminalNameRefClassificationTypeType;
		private readonly IClassificationType anglrRegexNameClassificationTypeType;
		private readonly IClassificationType anglrTerminalNameClassificationTypeType;
		private readonly IClassificationType anglrTerminalNameDefClassificationTypeType;
		private readonly IClassificationType anglrTerminalNameRefClassificationTypeType;
		private readonly IClassificationType anglrMarkerNameClassificationTypeType;
		private readonly IClassificationType anglrProductionNameClassificationTypeType;
		private readonly IClassificationType anglrLiteralClassificationTypeType;
		private readonly IClassificationType anglrStringLiteralClassificationTypeType;
		private readonly IClassificationType anglrNumericalLiteralClassificationTypeType;
		private readonly IClassificationType anglrCodeClassificationTypeType;
        private readonly IClassificationType anglrGroupNameClassificationTypeType;
        private readonly IClassificationType anglrAttributeListClassificationTypeType;
        private readonly IClassificationType anglrScannerActionClassificationTypeType;
        private readonly IClassificationType anglrEventNameClassificationTypeType;

        private IClassificationType [] classificationTypes = null;

		private readonly IClassificationTypeRegistryService registry = null;
		private readonly string fileName = "";

		private readonly StreamWriter streamWriter = null;
		private readonly bool debug = false;

		/// <summary>
		/// Initializes a new instance of the <see cref="AnglrEditorClassifier"/> class.
		/// </summary>
		/// <param name="registry">Classification registry.</param>
		internal AnglrEditorClassifier (IClassificationTypeRegistryService registry, string fileName)
		{
			this.registry = registry;
			this.fileName = fileName;

			try
			{
				if (debug)
					streamWriter = new StreamWriter (@"D:\Users\Miran\source\repos\test-classification\test\classification.txt");
			}
			catch (Exception e)
			{

			}

			List<IClassificationType> classificationTypeList = new List<IClassificationType> ();

			if ((anglrUndefinedClassificationTypeType = registry.GetClassificationType ("AnglrEditorClassifier.Undefined")) == null)
				Log ("AnglrEditorClassifier.Undefined classifier type is undefined");
			else
				classificationTypeList.Add (anglrUndefinedClassificationTypeType);

			if ((anglrReservedWordClassificationTypeType = registry.GetClassificationType ("AnglrEditorClassifier.ReservedWord")) == null)
				Log ("AnglrEditorClassifier.ReservedWord classifier type is undefined");
			else
				classificationTypeList.Add (anglrReservedWordClassificationTypeType);

			if ((anglrGeneralPartNameClassificationTypeType = registry.GetClassificationType ("AnglrEditorClassifier.GeneralPartName")) == null)
				Log ("AnglrEditorClassifier.GeneralPartName classifier type is undefined");
			else
				classificationTypeList.Add (anglrGeneralPartNameClassificationTypeType);

			if ((anglrDeclarationsPartNameClassificationTypeType = registry.GetClassificationType ("AnglrEditorClassifier.DeclarationsPartName")) == null)
				Log ("AnglrEditorClassifier.DeclarationsPartName classifier type is undefined");
			else
				classificationTypeList.Add (anglrDeclarationsPartNameClassificationTypeType);

			if ((anglrScannerPartNameClassificationTypeType = registry.GetClassificationType ("AnglrEditorClassifier.ScannerPartName")) == null)
				Log ("AnglrEditorClassifier.ScannerPartName classifier type is undefined");
			else
				classificationTypeList.Add (anglrScannerPartNameClassificationTypeType);

            if ((anglrLexerPartNameClassificationTypeType = registry.GetClassificationType ("AnglrEditorClassifier.LexerPartName")) == null)
                Log ("AnglrEditorClassifier.LexerPartName classifier type is undefined");
            else
                classificationTypeList.Add (anglrLexerPartNameClassificationTypeType);

            if ((anglrParserPartNameClassificationTypeType = registry.GetClassificationType ("AnglrEditorClassifier.ParserPartName")) == null)
                Log ("AnglrEditorClassifier.ParserPartName classifier type is undefined");
            else
                classificationTypeList.Add (anglrParserPartNameClassificationTypeType);

            if ((anglrPartBracketClassificationTypeType = registry.GetClassificationType ("AnglrEditorClassifier.PartBracket")) == null)
				Log ("AnglrEditorClassifier.PartBracket classifier type is undefined");
			else
				classificationTypeList.Add (anglrPartBracketClassificationTypeType);

			if ((anglrBracketClassificationTypeType = registry.GetClassificationType ("AnglrEditorClassifier.Bracket")) == null)
				Log ("AnglrEditorClassifier.Bracket classifier type is undefined");
			else
				classificationTypeList.Add (anglrBracketClassificationTypeType);

			if ((anglrRegularExpressionClassificationTypeType = registry.GetClassificationType ("AnglrEditorClassifier.RegularExpression")) == null)
				Log ("AnglrEditorClassifier.RegularExpression classifier type is undefined");
			else
				classificationTypeList.Add (anglrRegularExpressionClassificationTypeType);

			if ((anglrAttributeNameClassificationTypeType = registry.GetClassificationType ("AnglrEditorClassifier.AttributeName")) == null)
				Log ("AnglrEditorClassifier.AttributeName classifier type is undefined");
			else
				classificationTypeList.Add (anglrAttributeNameClassificationTypeType);

			if ((anglrPropertyNameClassificationTypeType = registry.GetClassificationType ("AnglrEditorClassifier.PropertyName")) == null)
				Log ("AnglrEditorClassifier.PropertyName classifier type is undefined");
			else
				classificationTypeList.Add (anglrPropertyNameClassificationTypeType);

			if ((anglrOperatorSignatureClassificationTypeType = registry.GetClassificationType ("AnglrEditorClassifier.OperatorSignature")) == null)
				Log ("AnglrEditorClassifier.OperatorSignature classifier type is undefined");
			else
				classificationTypeList.Add (anglrOperatorSignatureClassificationTypeType);

			if ((anglrPropertyValueClassificationTypeType = registry.GetClassificationType ("AnglrEditorClassifier.PropertyValue")) == null)
				Log ("AnglrEditorClassifier.PropertyValue classifier type is undefined");
			else
				classificationTypeList.Add (anglrPropertyValueClassificationTypeType);

			if ((anglrNonTerminalNameClassificationTypeType = registry.GetClassificationType ("AnglrEditorClassifier.NonTerminalName")) == null)
				Log ("AnglrEditorClassifier.NonTerminalName classifier type is undefined");
			else
				classificationTypeList.Add (anglrNonTerminalNameClassificationTypeType);

			if ((anglrNonTerminalNameDefClassificationTypeType = registry.GetClassificationType ("AnglrEditorClassifier.NonTerminalNameDef")) == null)
				Log ("AnglrEditorClassifier.NonTerminalNameDef classifier type is undefined");
			else
				classificationTypeList.Add (anglrNonTerminalNameDefClassificationTypeType);

			if ((anglrNonTerminalNameRefClassificationTypeType = registry.GetClassificationType ("AnglrEditorClassifier.NonTerminalNameRef")) == null)
				Log ("AnglrEditorClassifier.NonTerminalNameRef classifier type is undefined");
			else
				classificationTypeList.Add (anglrNonTerminalNameRefClassificationTypeType);

			if ((anglrRegexNameClassificationTypeType = registry.GetClassificationType ("AnglrEditorClassifier.RegexName")) == null)
				Log ("AnglrEditorClassifier.RegexName classifier type is undefined");
			else
				classificationTypeList.Add (anglrRegexNameClassificationTypeType);

			if ((anglrTerminalNameClassificationTypeType = registry.GetClassificationType ("AnglrEditorClassifier.TerminalName")) == null)
				Log ("AnglrEditorClassifier.TerminalName classifier type is undefined");
			else
				classificationTypeList.Add (anglrTerminalNameClassificationTypeType);

			if ((anglrTerminalNameDefClassificationTypeType = registry.GetClassificationType ("AnglrEditorClassifier.TerminalNameDef")) == null)
				Log ("AnglrEditorClassifier.TerminalNameDef classifier type is undefined");
			else
				classificationTypeList.Add (anglrTerminalNameDefClassificationTypeType);

			if ((anglrTerminalNameRefClassificationTypeType = registry.GetClassificationType ("AnglrEditorClassifier.TerminalNameRef")) == null)
				Log ("AnglrEditorClassifier.TerminalNameRef classifier type is undefined");
			else
				classificationTypeList.Add (anglrTerminalNameRefClassificationTypeType);

			if ((anglrMarkerNameClassificationTypeType = registry.GetClassificationType ("AnglrEditorClassifier.MarkerName")) == null)
				Log ("AnglrEditorClassifier.MarkerName classifier type is undefined");
			else
				classificationTypeList.Add (anglrMarkerNameClassificationTypeType);

			if ((anglrProductionNameClassificationTypeType = registry.GetClassificationType ("AnglrEditorClassifier.ProductionName")) == null)
				Log ("AnglrEditorClassifier.ProductionName classifier type is undefined");
			else
				classificationTypeList.Add (anglrProductionNameClassificationTypeType);

			if ((anglrLiteralClassificationTypeType = registry.GetClassificationType ("AnglrEditorClassifier.Literal")) == null)
				Log ("AnglrEditorClassifier.Literal classifier type is undefined");
			else
				classificationTypeList.Add (anglrLiteralClassificationTypeType);

			if ((anglrStringLiteralClassificationTypeType = registry.GetClassificationType ("AnglrEditorClassifier.StringLiteral")) == null)
				Log ("AnglrEditorClassifier.StringLiteral classifier type is undefined");
			else
				classificationTypeList.Add (anglrStringLiteralClassificationTypeType);

			if ((anglrNumericalLiteralClassificationTypeType = registry.GetClassificationType ("AnglrEditorClassifier.NumericalLiteral")) == null)
				Log ("AnglrEditorClassifier.NumericalLiteral classifier type is undefined");
			else
				classificationTypeList.Add (anglrNumericalLiteralClassificationTypeType);

			if ((anglrCodeClassificationTypeType = registry.GetClassificationType ("AnglrEditorClassifier.Code")) == null)
				Log ("AnglrEditorClassifier.Code classifier type is undefined");
			else
				classificationTypeList.Add (anglrCodeClassificationTypeType);

            if ((anglrGroupNameClassificationTypeType = registry.GetClassificationType ("AnglrEditorClassifier.GroupName")) == null)
                Log ("AnglrEditorClassifier.GroupName classifier type is undefined");
            else
                classificationTypeList.Add (anglrGroupNameClassificationTypeType);

            if ((anglrAttributeListClassificationTypeType = registry.GetClassificationType ("AnglrEditorClassifier.AttributeList")) == null)
                Log ("AnglrEditorClassifier.AttributeList classifier type is undefined");
            else
                classificationTypeList.Add (anglrAttributeListClassificationTypeType);

            if ((anglrScannerActionClassificationTypeType = registry.GetClassificationType ("AnglrEditorClassifier.ScannerAction")) == null)
                Log ("AnglrEditorClassifier.ScannerAction classifier type is undefined");
            else
                classificationTypeList.Add (anglrScannerActionClassificationTypeType);

            if ((anglrEventNameClassificationTypeType = registry.GetClassificationType ("AnglrEditorClassifier.EventName")) == null)
                Log ("AnglrEditorClassifier.EventName classifier type is undefined");
            else
                classificationTypeList.Add (anglrEventNameClassificationTypeType);

            classificationTypes = classificationTypeList.ToArray ();

			Log ($"created AnglrEditorClassifier for: {fileName}");
		}

		internal void Log (string message)
		{
			if (!debug)
				return;
			if (streamWriter == null)
				return;
			streamWriter.WriteLine (message);
			streamWriter.Flush ();
		}

		#region IClassifier

#pragma warning disable 67

		/// <summary>
		/// An event that occurs when the classification of a span of text has changed.
		/// </summary>
		/// <remarks>
		/// This event gets raised if a non-text change would affect the classification in some way,
		/// for example typing /* would cause the classification to change in C# without directly
		/// affecting the span.
		/// </remarks>
		public event EventHandler<ClassificationChangedEventArgs> ClassificationChanged;

#pragma warning restore 67

		/// <summary>
		/// Gets all the <see cref="ClassificationSpan"/> objects that intersect with the given range of text.
		/// </summary>
		/// <remarks>
		/// This method scans the given SnapshotSpan for potential matches for this classification.
		/// In this instance, it classifies everything and returns each span as a new ClassificationSpan.
		/// </remarks>
		/// <param name="span">The span currently being classified.</param>
		/// <returns>A list of ClassificationSpans that represent spans identified to be of this classification.</returns>
		public IList<ClassificationSpan> GetClassificationSpans (SnapshotSpan span)
		{
			AnglrLspClientAccessPoint anglrLspClientAccessPoint = AnglrLspClientAccessPoint.Instance;
			ITextSnapshot textSnapshot = span.Snapshot;
			int startLineNumber = textSnapshot.GetLineNumberFromPosition (span.Start.Position);
			int endLineNumber = textSnapshot.GetLineNumberFromPosition (span.End.Position);
			var result = new List<ClassificationSpan> ();

			if (anglrLspClientAccessPoint != null)
            {
				try
				{
					AnglrGetClassificationSpansParams anglrGetClassificationSpansParams = new AnglrGetClassificationSpansParams()
					{
						TextDocument = new TextDocumentIdentifier ()
						{
							Uri = new Uri(fileName)
						},
						Position = new Position ()
						{
							Line = startLineNumber,
							Character = 0
						}
					};

					AnglrGetClassificationSpansResult anglrGetClassificationSpansResult =
						ThreadHelper.JoinableTaskFactory.Run
						(
							() => anglrLspClientAccessPoint.InvokeAsync<AnglrGetClassificationSpansResult> (AnglrMethods.AnglrGetClassificationSpansName, anglrGetClassificationSpansParams)
						);
					if (debug)
					{
						string spans = "";
						if (anglrGetClassificationSpansResult != null)
							foreach ((int col, int len, int cls) elt in anglrGetClassificationSpansResult.ClassificationSpanInfo)
							{
								spans += $"({elt.col}, {elt.len}, {elt.cls}) ";
							}
						Log ($"method {AnglrMethods.AnglrGetClassificationSpansName} returned ({spans})");
					}

					if (anglrGetClassificationSpansResult != null)
						foreach ((int col, int len, int cls) elt in anglrGetClassificationSpansResult.ClassificationSpanInfo)
						{
							IClassificationType classType;

							try
							{
								classType = classificationTypes[elt.cls];
							}
							catch
							{
								classType = classificationTypes[0];
							}

							result.Add (new ClassificationSpan (new SnapshotSpan (textSnapshot, span.Start.Position + elt.col, elt.len), classType));
						}
				}
				catch (Exception e)
				{
					Log ($"{e.Message}");
				}
				Log ($"snapshot (Y): ({startLineNumber}, {endLineNumber})");
			}
			else
				Log ($"snapshot (N): ({startLineNumber}, {endLineNumber})");

			return result;
		}

		#endregion
	}
}
