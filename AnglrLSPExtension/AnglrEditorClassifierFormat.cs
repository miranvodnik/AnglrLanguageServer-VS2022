using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Utilities;
using System.ComponentModel.Composition;
using System.Windows.Media;

namespace AnglrLSPExtension
{
	/// <summary>
	/// Defines an editor format for the AnglrEditorClassifier type that has a purple background
	/// and is underlined.
	/// </summary>

	[Export (typeof (EditorFormatDefinition))]
	[ClassificationType (ClassificationTypeNames = "AnglrEditorClassifier.Undefined")]
	[Name ("AnglrEditorClassifier.Undefined")]
	[UserVisible (true)] // This should be visible to the end user
	[Order (Before = Priority.Default)] // Set the priority to be after the default classifiers
	internal class AnglrUndefinedClassificationFormat : ClassificationFormatDefinition
	{
		AnglrUndefinedClassificationFormat ()
		{
			this.ForegroundColor = Colors.DarkRed;
			this.TextDecorations = System.Windows.TextDecorations.Underline;
		}
	}


	[Export (typeof (EditorFormatDefinition))]
	[ClassificationType (ClassificationTypeNames = "AnglrEditorClassifier.ReservedWord")]
	[Name ("AnglrEditorClassifier.ReservedWord")]
	internal class AnglrReservedWordClassificationFormat : ClassificationFormatDefinition
	{
		AnglrReservedWordClassificationFormat ()
		{
			this.ForegroundColor = Colors.Green;
		}
	}


	[Export (typeof (EditorFormatDefinition))]
	[ClassificationType (ClassificationTypeNames = "AnglrEditorClassifier.GeneralPartName")]
	[Name ("AnglrEditorClassifier.GeneralPartName")]
	internal class AnglrGeneralPartNameClassificationFormat : ClassificationFormatDefinition
	{
		AnglrGeneralPartNameClassificationFormat ()
		{
			this.ForegroundColor = Colors.Blue;
		}
	}


	[Export (typeof (EditorFormatDefinition))]
	[ClassificationType (ClassificationTypeNames = "AnglrEditorClassifier.DeclarationsPartName")]
	[Name ("AnglrEditorClassifier.DeclarationsPartName")]
	internal class AnglrDeclarationsPartNameClassificationFormat : ClassificationFormatDefinition
	{
		AnglrDeclarationsPartNameClassificationFormat ()
		{
			this.ForegroundColor = Colors.DarkRed;
		}
	}


	[Export (typeof (EditorFormatDefinition))]
	[ClassificationType (ClassificationTypeNames = "AnglrEditorClassifier.ScannerPartName")]
	[Name ("AnglrEditorClassifier.ScannerPartName")]
	internal class AnglrScannerPartNameClassificationFormat : ClassificationFormatDefinition
	{
		AnglrScannerPartNameClassificationFormat ()
		{
			this.ForegroundColor = Colors.DarkGreen;
		}
	}


    [Export (typeof (EditorFormatDefinition))]
    [ClassificationType (ClassificationTypeNames = "AnglrEditorClassifier.LexerPartName")]
    [Name ("AnglrEditorClassifier.LexerPartName")]
    internal class AnglrLexerPartNameClassificationFormat : ClassificationFormatDefinition
    {
        AnglrLexerPartNameClassificationFormat ()
        {
            this.ForegroundColor = Colors.DarkBlue;
        }
    }


    [Export (typeof (EditorFormatDefinition))]
    [ClassificationType (ClassificationTypeNames = "AnglrEditorClassifier.ParserPartName")]
    [Name ("AnglrEditorClassifier.ParserPartName")]
    internal class AnglrParserPartNameClassificationFormat : ClassificationFormatDefinition
    {
        AnglrParserPartNameClassificationFormat ()
        {
            this.ForegroundColor = Colors.DarkBlue;
        }
    }


    [Export (typeof (EditorFormatDefinition))]
	[ClassificationType (ClassificationTypeNames = "AnglrEditorClassifier.PartBracket")]
	[Name ("AnglrEditorClassifier.PartBracket")]
	internal class AnglrPartBracketClassificationFormat : ClassificationFormatDefinition
	{
		AnglrPartBracketClassificationFormat ()
		{
			this.ForegroundColor = Colors.OrangeRed;
		}
	}


	[Export (typeof (EditorFormatDefinition))]
	[ClassificationType (ClassificationTypeNames = "AnglrEditorClassifier.Bracket")]
	[Name ("AnglrEditorClassifier.Bracket")]
	internal class AnglrBracketClassificationFormat : ClassificationFormatDefinition
	{
		AnglrBracketClassificationFormat ()
		{
			this.ForegroundColor = Colors.Gray;
		}
	}


	[Export (typeof (EditorFormatDefinition))]
	[ClassificationType (ClassificationTypeNames = "AnglrEditorClassifier.RegularExpression")]
	[Name ("AnglrEditorClassifier.RegularExpression")]
	internal class AnglrRegularExpressionClassificationFormat : ClassificationFormatDefinition
	{
		AnglrRegularExpressionClassificationFormat ()
		{
			this.ForegroundColor = Colors.Brown;
		}
	}


	[Export (typeof (EditorFormatDefinition))]
	[ClassificationType (ClassificationTypeNames = "AnglrEditorClassifier.AttributeName")]
	[Name ("AnglrEditorClassifier.AttributeName")]
	internal class AnglrAttributeNameClassificationFormat : ClassificationFormatDefinition
	{
		AnglrAttributeNameClassificationFormat ()
		{
			this.ForegroundColor = Colors.Cyan;
		}
	}


	[Export (typeof (EditorFormatDefinition))]
	[ClassificationType (ClassificationTypeNames = "AnglrEditorClassifier.PropertyName")]
	[Name ("AnglrEditorClassifier.PropertyName")]
	internal class AnglrPropertyNameClassificationFormat : ClassificationFormatDefinition
	{
		AnglrPropertyNameClassificationFormat ()
		{
			this.ForegroundColor = Colors.DarkCyan;
		}
	}


	[Export (typeof (EditorFormatDefinition))]
	[ClassificationType (ClassificationTypeNames = "AnglrEditorClassifier.OperatorSignature")]
	[Name ("AnglrEditorClassifier.OperatorSignature")]
	internal class AnglrOperatorSignatureClassificationFormat : ClassificationFormatDefinition
	{
		AnglrOperatorSignatureClassificationFormat ()
		{
			this.ForegroundColor = Colors.Red;
		}
	}


	[Export (typeof (EditorFormatDefinition))]
	[ClassificationType (ClassificationTypeNames = "AnglrEditorClassifier.PropertyValue")]
	[Name ("AnglrEditorClassifier.PropertyValue")]
	internal class AnglrPropertyValueClassificationFormat : ClassificationFormatDefinition
	{
		AnglrPropertyValueClassificationFormat ()
		{
			this.ForegroundColor = Colors.Gray;
		}
	}


	[Export (typeof (EditorFormatDefinition))]
	[ClassificationType (ClassificationTypeNames = "AnglrEditorClassifier.NonTerminalName")]
	[Name ("AnglrEditorClassifier.NonTerminalName")]
	internal class AnglrNonTerminalNameClassificationFormat : ClassificationFormatDefinition
	{
		AnglrNonTerminalNameClassificationFormat ()
		{
			this.ForegroundColor = Colors.BlueViolet;
		}
	}


	[Export (typeof (EditorFormatDefinition))]
	[ClassificationType (ClassificationTypeNames = "AnglrEditorClassifier.NonTerminalNameDef")]
	[Name ("AnglrEditorClassifier.NonTerminalNameDef")]
	internal class AnglrNonTerminalNameDefClassificationFormat : ClassificationFormatDefinition
	{
		AnglrNonTerminalNameDefClassificationFormat ()
		{
			this.ForegroundColor = Colors.Red;
		}
	}


	[Export (typeof (EditorFormatDefinition))]
	[ClassificationType (ClassificationTypeNames = "AnglrEditorClassifier.NonTerminalNameRef")]
	[Name ("AnglrEditorClassifier.NonTerminalNameRef")]
	internal class AnglrNonTerminalNameRefClassificationFormat : ClassificationFormatDefinition
	{
		AnglrNonTerminalNameRefClassificationFormat ()
		{
			this.ForegroundColor = Colors.IndianRed;
		}
	}


	[Export (typeof (EditorFormatDefinition))]
	[ClassificationType (ClassificationTypeNames = "AnglrEditorClassifier.RegexName")]
	[Name ("AnglrEditorClassifier.RegexName")]
	internal class AnglrRegexNameClassificationFormat : ClassificationFormatDefinition
	{
		AnglrRegexNameClassificationFormat ()
		{
			this.ForegroundColor = Colors.Green;
		}
	}


	[Export (typeof (EditorFormatDefinition))]
	[ClassificationType (ClassificationTypeNames = "AnglrEditorClassifier.TerminalName")]
	[Name ("AnglrEditorClassifier.TerminalName")]
	internal class AnglrTerminalNameClassificationFormat : ClassificationFormatDefinition
	{
		AnglrTerminalNameClassificationFormat ()
		{
			this.ForegroundColor = Colors.Brown;
		}
	}


	[Export (typeof (EditorFormatDefinition))]
	[ClassificationType (ClassificationTypeNames = "AnglrEditorClassifier.TerminalNameDef")]
	[Name ("AnglrEditorClassifier.TerminalNameDef")]
	internal class AnglrTerminalNameDefClassificationFormat : ClassificationFormatDefinition
	{
		AnglrTerminalNameDefClassificationFormat ()
		{
			this.ForegroundColor = Colors.Red;
		}
	}


	[Export (typeof (EditorFormatDefinition))]
	[ClassificationType (ClassificationTypeNames = "AnglrEditorClassifier.TerminalNameRef")]
	[Name ("AnglrEditorClassifier.TerminalNameRef")]
	internal class AnglrTerminalNameRefClassificationFormat : ClassificationFormatDefinition
	{
		AnglrTerminalNameRefClassificationFormat ()
		{
			this.ForegroundColor = Colors.Green;
		}
	}


	[Export (typeof (EditorFormatDefinition))]
	[ClassificationType (ClassificationTypeNames = "AnglrEditorClassifier.MarkerName")]
	[Name ("AnglrEditorClassifier.MarkerName")]
	internal class AnglrMarkerNameClassificationFormat : ClassificationFormatDefinition
	{
		AnglrMarkerNameClassificationFormat ()
		{
			this.ForegroundColor = Colors.Green;
		}
	}


	[Export (typeof (EditorFormatDefinition))]
	[ClassificationType (ClassificationTypeNames = "AnglrEditorClassifier.ProductionName")]
	[Name ("AnglrEditorClassifier.ProductionName")]
	internal class AnglrProductionNameClassificationFormat : ClassificationFormatDefinition
	{
		AnglrProductionNameClassificationFormat ()
		{
			this.ForegroundColor = Colors.Green;
		}
	}


	[Export (typeof (EditorFormatDefinition))]
	[ClassificationType (ClassificationTypeNames = "AnglrEditorClassifier.Literal")]
	[Name ("AnglrEditorClassifier.Literal")]
	internal class AnglrLiteralClassificationFormat : ClassificationFormatDefinition
	{
		AnglrLiteralClassificationFormat ()
		{
			this.ForegroundColor = Colors.Blue;
		}
	}


	[Export (typeof (EditorFormatDefinition))]
	[ClassificationType (ClassificationTypeNames = "AnglrEditorClassifier.StringLiteral")]
	[Name ("AnglrEditorClassifier.StringLiteral")]
	internal class AnglrStringLiteralClassificationFormat : ClassificationFormatDefinition
	{
		AnglrStringLiteralClassificationFormat ()
		{
			this.ForegroundColor = Colors.Red;
		}
	}


	[Export (typeof (EditorFormatDefinition))]
	[ClassificationType (ClassificationTypeNames = "AnglrEditorClassifier.NumericalLiteral")]
	[Name ("AnglrEditorClassifier.NumericalLiteral")]
	internal class AnglrNumericalLiteralClassificationFormat : ClassificationFormatDefinition
	{
		AnglrNumericalLiteralClassificationFormat ()
		{
			this.ForegroundColor = Colors.Green;
		}
	}


	[Export (typeof (EditorFormatDefinition))]
	[ClassificationType (ClassificationTypeNames = "AnglrEditorClassifier.Code")]
	[Name ("AnglrEditorClassifier.Code")]
	internal class AnglrCodeClassificationFormat : ClassificationFormatDefinition
	{
		AnglrCodeClassificationFormat ()
		{
			this.ForegroundColor = Colors.Blue;
		}
	}


    [Export (typeof (EditorFormatDefinition))]
    [ClassificationType (ClassificationTypeNames = "AnglrEditorClassifier.GroupName")]
    [Name ("AnglrEditorClassifier.GroupName")]
    internal class AnglrGroupNameClassificationFormat : ClassificationFormatDefinition
    {
        AnglrGroupNameClassificationFormat ()
        {
            this.ForegroundColor = Colors.Blue;
        }
    }


    [Export (typeof (EditorFormatDefinition))]
    [ClassificationType (ClassificationTypeNames = "AnglrEditorClassifier.AttributeList")]
    [Name ("AnglrEditorClassifier.AttributeList")]
    internal class AnglrAttributeListClassificationFormat : ClassificationFormatDefinition
    {
        AnglrAttributeListClassificationFormat ()
        {
            this.ForegroundColor = Colors.Green;
        }
    }


    [Export (typeof (EditorFormatDefinition))]
    [ClassificationType (ClassificationTypeNames = "AnglrEditorClassifier.ScannerAction")]
    [Name ("AnglrEditorClassifier.ScannerAction")]
    internal class AnglrScannerActionClassificationFormat : ClassificationFormatDefinition
    {
        AnglrScannerActionClassificationFormat ()
        {
            this.ForegroundColor = Colors.Red;
        }
    }


    [Export (typeof (EditorFormatDefinition))]
    [ClassificationType (ClassificationTypeNames = "AnglrEditorClassifier.EventName")]
    [Name ("AnglrEditorClassifier.EventName")]
    internal class AnglrEventNameClassificationFormat : ClassificationFormatDefinition
    {
        AnglrEventNameClassificationFormat ()
        {
            this.ForegroundColor = Colors.LightGoldenrodYellow;
        }
    }
}
