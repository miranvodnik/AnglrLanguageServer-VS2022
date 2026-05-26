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
using AnglrLogLibrary;

namespace AnglrLangExtension
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

		private readonly bool debug = false;

		private IAnglrLogger logger = null;
		private IAnglrLangService anglrLangService = null;

		/// <summary>
		/// Initializes a new instance of the <see cref="AnglrEditorClassifier"/> class.
		/// </summary>
		/// <param name="registry">Classification registry.</param>
		internal AnglrEditorClassifier (IClassificationTypeRegistryService registry, string fileName)
		{
			this.registry = registry;
			this.fileName = fileName;

            anglrLangService = ThreadHelper.JoinableTaskFactory.Run (() => AnglrLangExtensionPackage.Instance.GetServiceAsync (typeof (SAnglrLangService))) as IAnglrLangService;
			logger = anglrLangService?.AnglrLogger ?? new VoidAnglrLogger ();

			List<IClassificationType> classificationTypeList = new List<IClassificationType> ();

			if ((anglrUndefinedClassificationTypeType = registry.GetClassificationType ("AnglrEditorClassifier.Undefined")) == null)
				logger?.DebugLine ("AnglrEditorClassifier.Undefined classifier type is undefined");
			else
				classificationTypeList.Add (anglrUndefinedClassificationTypeType);

			if ((anglrReservedWordClassificationTypeType = registry.GetClassificationType ("AnglrEditorClassifier.ReservedWord")) == null)
                logger?.DebugLine ("AnglrEditorClassifier.ReservedWord classifier type is undefined");
			else
				classificationTypeList.Add (anglrReservedWordClassificationTypeType);

			if ((anglrGeneralPartNameClassificationTypeType = registry.GetClassificationType ("AnglrEditorClassifier.GeneralPartName")) == null)
                logger?.DebugLine ("AnglrEditorClassifier.GeneralPartName classifier type is undefined");
			else
				classificationTypeList.Add (anglrGeneralPartNameClassificationTypeType);

			if ((anglrDeclarationsPartNameClassificationTypeType = registry.GetClassificationType ("AnglrEditorClassifier.DeclarationsPartName")) == null)
                logger?.DebugLine ("AnglrEditorClassifier.DeclarationsPartName classifier type is undefined");
			else
				classificationTypeList.Add (anglrDeclarationsPartNameClassificationTypeType);

			if ((anglrScannerPartNameClassificationTypeType = registry.GetClassificationType ("AnglrEditorClassifier.ScannerPartName")) == null)
                logger?.DebugLine ("AnglrEditorClassifier.ScannerPartName classifier type is undefined");
			else
				classificationTypeList.Add (anglrScannerPartNameClassificationTypeType);

            if ((anglrLexerPartNameClassificationTypeType = registry.GetClassificationType ("AnglrEditorClassifier.LexerPartName")) == null)
                logger?.DebugLine ("AnglrEditorClassifier.LexerPartName classifier type is undefined");
            else
                classificationTypeList.Add (anglrLexerPartNameClassificationTypeType);

            if ((anglrParserPartNameClassificationTypeType = registry.GetClassificationType ("AnglrEditorClassifier.ParserPartName")) == null)
                logger?.DebugLine ("AnglrEditorClassifier.ParserPartName classifier type is undefined");
            else
                classificationTypeList.Add (anglrParserPartNameClassificationTypeType);

            if ((anglrPartBracketClassificationTypeType = registry.GetClassificationType ("AnglrEditorClassifier.PartBracket")) == null)
                logger?.DebugLine ("AnglrEditorClassifier.PartBracket classifier type is undefined");
			else
				classificationTypeList.Add (anglrPartBracketClassificationTypeType);

			if ((anglrBracketClassificationTypeType = registry.GetClassificationType ("AnglrEditorClassifier.Bracket")) == null)
                logger?.DebugLine ("AnglrEditorClassifier.Bracket classifier type is undefined");
			else
				classificationTypeList.Add (anglrBracketClassificationTypeType);

			if ((anglrRegularExpressionClassificationTypeType = registry.GetClassificationType ("AnglrEditorClassifier.RegularExpression")) == null)
                logger?.DebugLine ("AnglrEditorClassifier.RegularExpression classifier type is undefined");
			else
				classificationTypeList.Add (anglrRegularExpressionClassificationTypeType);

			if ((anglrAttributeNameClassificationTypeType = registry.GetClassificationType ("AnglrEditorClassifier.AttributeName")) == null)
                logger?.DebugLine ("AnglrEditorClassifier.AttributeName classifier type is undefined");
			else
				classificationTypeList.Add (anglrAttributeNameClassificationTypeType);

			if ((anglrPropertyNameClassificationTypeType = registry.GetClassificationType ("AnglrEditorClassifier.PropertyName")) == null)
                logger?.DebugLine ("AnglrEditorClassifier.PropertyName classifier type is undefined");
			else
				classificationTypeList.Add (anglrPropertyNameClassificationTypeType);

			if ((anglrOperatorSignatureClassificationTypeType = registry.GetClassificationType ("AnglrEditorClassifier.OperatorSignature")) == null)
                logger?.DebugLine ("AnglrEditorClassifier.OperatorSignature classifier type is undefined");
			else
				classificationTypeList.Add (anglrOperatorSignatureClassificationTypeType);

			if ((anglrPropertyValueClassificationTypeType = registry.GetClassificationType ("AnglrEditorClassifier.PropertyValue")) == null)
                logger?.DebugLine ("AnglrEditorClassifier.PropertyValue classifier type is undefined");
			else
				classificationTypeList.Add (anglrPropertyValueClassificationTypeType);

			if ((anglrNonTerminalNameClassificationTypeType = registry.GetClassificationType ("AnglrEditorClassifier.NonTerminalName")) == null)
                logger?.DebugLine ("AnglrEditorClassifier.NonTerminalName classifier type is undefined");
			else
				classificationTypeList.Add (anglrNonTerminalNameClassificationTypeType);

			if ((anglrNonTerminalNameDefClassificationTypeType = registry.GetClassificationType ("AnglrEditorClassifier.NonTerminalNameDef")) == null)
                logger?.DebugLine ("AnglrEditorClassifier.NonTerminalNameDef classifier type is undefined");
			else
				classificationTypeList.Add (anglrNonTerminalNameDefClassificationTypeType);

			if ((anglrNonTerminalNameRefClassificationTypeType = registry.GetClassificationType ("AnglrEditorClassifier.NonTerminalNameRef")) == null)
                logger?.DebugLine ("AnglrEditorClassifier.NonTerminalNameRef classifier type is undefined");
			else
				classificationTypeList.Add (anglrNonTerminalNameRefClassificationTypeType);

			if ((anglrRegexNameClassificationTypeType = registry.GetClassificationType ("AnglrEditorClassifier.RegexName")) == null)
                logger?.DebugLine ("AnglrEditorClassifier.RegexName classifier type is undefined");
			else
				classificationTypeList.Add (anglrRegexNameClassificationTypeType);

			if ((anglrTerminalNameClassificationTypeType = registry.GetClassificationType ("AnglrEditorClassifier.TerminalName")) == null)
                logger?.DebugLine ("AnglrEditorClassifier.TerminalName classifier type is undefined");
			else
				classificationTypeList.Add (anglrTerminalNameClassificationTypeType);

			if ((anglrTerminalNameDefClassificationTypeType = registry.GetClassificationType ("AnglrEditorClassifier.TerminalNameDef")) == null)
                logger?.DebugLine ("AnglrEditorClassifier.TerminalNameDef classifier type is undefined");
			else
				classificationTypeList.Add (anglrTerminalNameDefClassificationTypeType);

			if ((anglrTerminalNameRefClassificationTypeType = registry.GetClassificationType ("AnglrEditorClassifier.TerminalNameRef")) == null)
                logger?.DebugLine ("AnglrEditorClassifier.TerminalNameRef classifier type is undefined");
			else
				classificationTypeList.Add (anglrTerminalNameRefClassificationTypeType);

			if ((anglrMarkerNameClassificationTypeType = registry.GetClassificationType ("AnglrEditorClassifier.MarkerName")) == null)
                logger?.DebugLine ("AnglrEditorClassifier.MarkerName classifier type is undefined");
			else
				classificationTypeList.Add (anglrMarkerNameClassificationTypeType);

			if ((anglrProductionNameClassificationTypeType = registry.GetClassificationType ("AnglrEditorClassifier.ProductionName")) == null)
                logger?.DebugLine ("AnglrEditorClassifier.ProductionName classifier type is undefined");
			else
				classificationTypeList.Add (anglrProductionNameClassificationTypeType);

			if ((anglrLiteralClassificationTypeType = registry.GetClassificationType ("AnglrEditorClassifier.Literal")) == null)
                logger?.DebugLine ("AnglrEditorClassifier.Literal classifier type is undefined");
			else
				classificationTypeList.Add (anglrLiteralClassificationTypeType);

			if ((anglrStringLiteralClassificationTypeType = registry.GetClassificationType ("AnglrEditorClassifier.StringLiteral")) == null)
                logger?.DebugLine ("AnglrEditorClassifier.StringLiteral classifier type is undefined");
			else
				classificationTypeList.Add (anglrStringLiteralClassificationTypeType);

			if ((anglrNumericalLiteralClassificationTypeType = registry.GetClassificationType ("AnglrEditorClassifier.NumericalLiteral")) == null)
                logger?.DebugLine ("AnglrEditorClassifier.NumericalLiteral classifier type is undefined");
			else
				classificationTypeList.Add (anglrNumericalLiteralClassificationTypeType);

			if ((anglrCodeClassificationTypeType = registry.GetClassificationType ("AnglrEditorClassifier.Code")) == null)
                logger?.DebugLine ("AnglrEditorClassifier.Code classifier type is undefined");
			else
				classificationTypeList.Add (anglrCodeClassificationTypeType);

            if ((anglrGroupNameClassificationTypeType = registry.GetClassificationType ("AnglrEditorClassifier.GroupName")) == null)
                logger?.DebugLine ("AnglrEditorClassifier.GroupName classifier type is undefined");
            else
                classificationTypeList.Add (anglrGroupNameClassificationTypeType);

            if ((anglrAttributeListClassificationTypeType = registry.GetClassificationType ("AnglrEditorClassifier.AttributeList")) == null)
                logger?.DebugLine ("AnglrEditorClassifier.AttributeList classifier type is undefined");
            else
                classificationTypeList.Add (anglrAttributeListClassificationTypeType);

            if ((anglrScannerActionClassificationTypeType = registry.GetClassificationType ("AnglrEditorClassifier.ScannerAction")) == null)
                logger?.DebugLine ("AnglrEditorClassifier.ScannerAction classifier type is undefined");
            else
                classificationTypeList.Add (anglrScannerActionClassificationTypeType);

            if ((anglrEventNameClassificationTypeType = registry.GetClassificationType ("AnglrEditorClassifier.EventName")) == null)
                logger?.DebugLine ("AnglrEditorClassifier.EventName classifier type is undefined");
            else
                classificationTypeList.Add (anglrEventNameClassificationTypeType);

            classificationTypes = classificationTypeList.ToArray ();

            logger?.InfoLine ($"created AnglrEditorClassifier for: {fileName}");
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
                        logger?.DebugLine ($"method {AnglrMethods.AnglrGetClassificationSpansName} returned ({spans})");
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
                    logger?.ErrorLine ($"{e.Message}");
				}
                logger?.DebugLine ($"snapshot (Y): ({startLineNumber}, {endLineNumber})");
			}
			else
                logger?.DebugLine ($"snapshot (N): ({startLineNumber}, {endLineNumber})");

			return result;
		}

		#endregion
	}
}
