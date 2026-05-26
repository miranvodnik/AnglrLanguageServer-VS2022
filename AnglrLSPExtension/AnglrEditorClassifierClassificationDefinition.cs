using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Utilities;
using System.ComponentModel.Composition;

namespace AnglrLSPExtension
{
	/// <summary>
	/// Classification type definition export for AnglrEditorClassifier
	/// </summary>
	internal static class AnglrEditorClassifierClassificationDefinition
	{
		// This disables "The field is never used" compiler's warning. Justification: the field is used by MEF.
#pragma warning disable 169

		/// <summary>
		/// Defines the "AnglrEditorClassifier" classification type.
		/// </summary>
		[Export(typeof(ClassificationTypeDefinition))]
		[Name("AnglrEditorClassifier.Undefined")]
		private static ClassificationTypeDefinition anglrUndefinedClassificationTypeDefinition;

		[Export(typeof(ClassificationTypeDefinition))]
		[Name("AnglrEditorClassifier.ReservedWord")]
		private static ClassificationTypeDefinition anglrReservedWordClassificationTypeDefinition;

		[Export(typeof(ClassificationTypeDefinition))]
		[Name("AnglrEditorClassifier.GeneralPartName")]
		private static ClassificationTypeDefinition anglrGeneralPartNameClassificationTypeDefinition;

		[Export(typeof(ClassificationTypeDefinition))]
		[Name("AnglrEditorClassifier.DeclarationsPartName")]
		private static ClassificationTypeDefinition anglrDeclarationsPartNameClassificationTypeDefinition;

		[Export(typeof(ClassificationTypeDefinition))]
		[Name("AnglrEditorClassifier.ScannerPartName")]
		private static ClassificationTypeDefinition anglrScannerPartNameClassificationTypeDefinition;

        [Export (typeof (ClassificationTypeDefinition))]
        [Name ("AnglrEditorClassifier.LexerPartName")]
        private static ClassificationTypeDefinition anglrLexerPartNameClassificationTypeDefinition;

        [Export (typeof (ClassificationTypeDefinition))]
        [Name ("AnglrEditorClassifier.ParserPartName")]
        private static ClassificationTypeDefinition anglrParserPartNameClassificationTypeDefinition;

        [Export (typeof(ClassificationTypeDefinition))]
		[Name("AnglrEditorClassifier.PartBracket")]
		private static ClassificationTypeDefinition anglrPartBracketClassificationTypeDefinition;

		[Export(typeof(ClassificationTypeDefinition))]
		[Name("AnglrEditorClassifier.Bracket")]
		private static ClassificationTypeDefinition anglrBracketClassificationTypeDefinition;

		[Export(typeof(ClassificationTypeDefinition))]
		[Name("AnglrEditorClassifier.RegularExpression")]
		private static ClassificationTypeDefinition anglrRegularExpressionClassificationTypeDefinition;

		[Export(typeof(ClassificationTypeDefinition))]
		[Name("AnglrEditorClassifier.AttributeName")]
		private static ClassificationTypeDefinition anglrAttributeNameClassificationTypeDefinition;

		[Export(typeof(ClassificationTypeDefinition))]
		[Name("AnglrEditorClassifier.PropertyName")]
		private static ClassificationTypeDefinition anglrPropertyNameClassificationTypeDefinition;

		[Export(typeof(ClassificationTypeDefinition))]
		[Name("AnglrEditorClassifier.OperatorSignature")]
		private static ClassificationTypeDefinition anglrOperatorSignatureClassificationTypeDefinition;

		[Export(typeof(ClassificationTypeDefinition))]
		[Name("AnglrEditorClassifier.PropertyValue")]
		private static ClassificationTypeDefinition anglrPropertyValueClassificationTypeDefinition;

		[Export(typeof(ClassificationTypeDefinition))]
		[Name("AnglrEditorClassifier.NonTerminalName")]
		private static ClassificationTypeDefinition anglrNonTerminalNameClassificationTypeDefinition;

		[Export(typeof(ClassificationTypeDefinition))]
		[Name("AnglrEditorClassifier.NonTerminalNameDef")]
		private static ClassificationTypeDefinition anglrNonTerminalNameDefClassificationTypeDefinition;

		[Export(typeof(ClassificationTypeDefinition))]
		[Name("AnglrEditorClassifier.NonTerminalNameRef")]
		private static ClassificationTypeDefinition anglrNonTerminalNameRefClassificationTypeDefinition;

		[Export(typeof(ClassificationTypeDefinition))]
		[Name("AnglrEditorClassifier.RegexName")]
		private static ClassificationTypeDefinition anglrRegexNameClassificationTypeDefinition;

		[Export(typeof(ClassificationTypeDefinition))]
		[Name("AnglrEditorClassifier.TerminalName")]
		private static ClassificationTypeDefinition anglrTerminalNameClassificationTypeDefinition;

		[Export(typeof(ClassificationTypeDefinition))]
		[Name("AnglrEditorClassifier.TerminalNameDef")]
		private static ClassificationTypeDefinition anglrTerminalNameDefClassificationTypeDefinition;

		[Export (typeof (ClassificationTypeDefinition))]
		[Name ("AnglrEditorClassifier.TerminalNameRef")]
		private static ClassificationTypeDefinition anglrTerminalNameRefClassificationTypeDefinition;

		[Export (typeof (ClassificationTypeDefinition))]
		[Name ("AnglrEditorClassifier.MarkerName")]
		private static ClassificationTypeDefinition anglrMarkerNameClassificationTypeDefinition;

		[Export (typeof (ClassificationTypeDefinition))]
		[Name ("AnglrEditorClassifier.ProductionName")]
		private static ClassificationTypeDefinition anglrProductionNameClassificationTypeDefinition;

		[Export (typeof(ClassificationTypeDefinition))]
		[Name("AnglrEditorClassifier.Literal")]
		private static ClassificationTypeDefinition anglrLiteralClassificationTypeDefinition;

		[Export(typeof(ClassificationTypeDefinition))]
		[Name("AnglrEditorClassifier.StringLiteral")]
		private static ClassificationTypeDefinition anglrStringLiteralClassificationTypeDefinition;

		[Export(typeof(ClassificationTypeDefinition))]
		[Name("AnglrEditorClassifier.NumericalLiteral")]
		private static ClassificationTypeDefinition anglrNumericalLiteralClassificationTypeDefinition;

		[Export(typeof(ClassificationTypeDefinition))]
		[Name("AnglrEditorClassifier.Code")]
		private static ClassificationTypeDefinition anglrCodeClassificationTypeDefinition;

        [Export (typeof (ClassificationTypeDefinition))]
        [Name ("AnglrEditorClassifier.GroupName")]
        private static ClassificationTypeDefinition anglrGroupNameClassificationTypeDefinition;

        [Export (typeof (ClassificationTypeDefinition))]
        [Name ("AnglrEditorClassifier.AttributeList")]
        private static ClassificationTypeDefinition anglrAttributeListClassificationTypeDefinition;

        [Export (typeof (ClassificationTypeDefinition))]
        [Name ("AnglrEditorClassifier.ScannerAction")]
        private static ClassificationTypeDefinition anglrScannerActionClassificationTypeDefinition;

        [Export (typeof (ClassificationTypeDefinition))]
        [Name ("AnglrEditorClassifier.EventName")]
        private static ClassificationTypeDefinition anglrEventNameClassificationTypeDefinition;

#pragma warning restore 169
    }
}
