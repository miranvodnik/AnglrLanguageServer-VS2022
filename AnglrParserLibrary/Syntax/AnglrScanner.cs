//
//	This file was generated with ANGLR compiler
//
using System;

﻿using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using Anglr.Lexer.Core;
using Anglr.Declarations;

namespace Anglr.ScannerLib
{
	public class AnglrScanner : Regex, RegexInterface
	{
		public AnglrScanner (LexerBase scanner) : base (@"(?<g1>\/\*)|(?<g2>\[\[)|(?<g3>\/\/)|(?<g4>\|)|(?<g5>\,)|(?<g6>\:)|(?<g7>\;)|(?<g8>\{)|(?<g9>\})|(?<g10>\()|(?<g11>\))|(?<g12>\%\{)|(?<g13>\%\})|(?<g14>\[)|(?<g15>\])|(?<g16>\=)|(?<g17>\@\@)|(?<g18>\@)|(?<g19>\?)|(?<g20>\+)|(?<g21>\-)|(?<g22>\*)|(?<g23>\/)|(?<g24>\~\+)|(?<g25>\~\-)|(?<g26>\~\*)|(?<g27>\~\/)|(?<g28>%[ \t]*empty)|(?<g29>%[ \t]*terminal)|(?<g30>%[ \t]*general)|(?<g31>%[ \t]*declarations)|(?<g32>%[ \t]*regex)|(?<g33>%[ \t]*scanner)|(?<g34>%[ \t]*lexer)|(?<g35>%[ \t]*parser)|(?<g36>%[ \t]*prio(rity)?)|(?<g37>%[ \t]*assoc(iativity)?)|(?<g38>[a-zA-Z_]([a-zA-Z_]|[0-9]|[-\.])*|<[a-zA-Z_]([a-zA-Z_]|[0-9]|[-\. ])*>)|(?<g39>\""([^""]|\""\"")*\""|\'([^']|\'\')*\')|(?<g40>[0-9]+)|(?<g41>[ \t]+)|(?<g42>[\n\r])|(?<g43>.)", RegexOptions.ExplicitCapture)
		{
			Scanner = scanner;
		}

		public LexerBase Scanner { get; private set; }
		public static string Id { get; private set; } = "anglrScanner";

		public delegate int scannerCallback (AnglrScanner regex, LexerBase scanner);



		public string text { get; private set; }

		public (int, int) match (string currentLine, int currentPosition)
		{
			int matchIndex = 0;
			int matchLength = 0;
			try
			{
				text = "";
				Match match = Match (currentLine, currentPosition);
				if (!match.Success)
					return (-1, 0);
					int index = 0;
				foreach (Group group in match.Groups)
				{
					if (index++ == 0)
						continue;
					if (!group.Success)
						continue;
					try
					{
						matchLength = match.Value.Length;
						matchIndex = index - 1;
						text = currentLine.Substring (currentPosition, matchLength);
					}
					catch (Exception)
					{
						continue;
					}
					break;
				}
			}
			catch (Exception e)
			{
				return (-2, 0);
			}

			int? result = null;

			switch (matchIndex)
			{
						case 1:
				Scanner.pushScanner (CommentScanner.Id, text);
				result = 0;
				break;
			case 2:
				Scanner.pushScanner (AttributeScanner.Id, text);
				result = 0;
				break;
			case 3:
				Scanner.pushScanner (LineCommentScanner.Id, text);
				result = 0;
				break;
			case 4:
				result = AnglrDeclarations.tokens._vertical_bar_;
				break;
			case 5:
				result = AnglrDeclarations.tokens._comma_;
				break;
			case 6:
				result = AnglrDeclarations.tokens._colon_;
				break;
			case 7:
				result = AnglrDeclarations.tokens._semicolon_;
				break;
			case 8:
				result = AnglrDeclarations.tokens._left_curly_bracket_;
				break;
			case 9:
				result = AnglrDeclarations.tokens._right_curly_bracket_;
				break;
			case 10:
				result = AnglrDeclarations.tokens._left_bracket_;
				break;
			case 11:
				result = AnglrDeclarations.tokens._right_bracket_;
				break;
			case 12:
				result = AnglrDeclarations.tokens._left_part_bracket_;
				break;
			case 13:
				result = AnglrDeclarations.tokens._right_part_bracket_;
				break;
			case 14:
				result = AnglrDeclarations.tokens._left_square_bracket_;
				break;
			case 15:
				result = AnglrDeclarations.tokens._right_square_bracket_;
				break;
			case 16:
				result = AnglrDeclarations.tokens._equals_sign_;
				break;
			case 17:
				result = AnglrDeclarations.tokens._double_at_sign_;
				break;
			case 18:
				result = AnglrDeclarations.tokens._at_sign_;
				break;
			case 19:
				result = AnglrDeclarations.tokens._question_mark_;
				break;
			case 20:
				result = AnglrDeclarations.tokens._plus_sign_;
				break;
			case 21:
				result = AnglrDeclarations.tokens._minus_sign_;
				break;
			case 22:
				result = AnglrDeclarations.tokens._asterisk_;
				break;
			case 23:
				result = AnglrDeclarations.tokens._slash_;
				break;
			case 24:
				result = AnglrDeclarations.tokens._inv_plus_sign_;
				break;
			case 25:
				result = AnglrDeclarations.tokens._inv_minus_sign_;
				break;
			case 26:
				result = AnglrDeclarations.tokens._inv_asterisk_;
				break;
			case 27:
				result = AnglrDeclarations.tokens._inv_slash_;
				break;
			case 28:
				result = AnglrDeclarations.tokens._empty_;
				break;
			case 29:
				result = AnglrDeclarations.tokens._terminal_;
				break;
			case 30:
				result = AnglrDeclarations.tokens._general_;
				break;
			case 31:
				result = AnglrDeclarations.tokens._declarations_;
				break;
			case 32:
				Scanner.pushScanner (RegexIdScanner.Id, text);
				result = AnglrDeclarations.tokens._regex_;
				break;
			case 33:
				Scanner.pushScanner (ScannerIdScanner.Id, text);
				result = AnglrDeclarations.tokens._scanner_;
				break;
			case 34:
				result = AnglrDeclarations.tokens._lexer_;
				break;
			case 35:
				result = AnglrDeclarations.tokens._parser_;
				break;
			case 36:
				result = AnglrDeclarations.tokens._priority_;
				break;
			case 37:
				result = AnglrDeclarations.tokens._associativity_;
				break;
			case 38:
				result = AnglrDeclarations.tokens._identifier_;
				break;
			case 39:
				result = AnglrDeclarations.tokens._cstring_;
				break;
			case 40:
				result = AnglrDeclarations.tokens._number_;
				break;
			case 41:
				result = 0;
				break;
			case 42:
				result = 0;
				break;
			case 43:
				result = 0;
				break;

			}
			return (result != null) ? (result.Value, matchLength) : (0, matchLength);
		}
	}
}

