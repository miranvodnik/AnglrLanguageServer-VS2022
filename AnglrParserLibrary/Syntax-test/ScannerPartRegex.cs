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

namespace Anglr.RegexLib
{
	internal class ScannerPartRegex : Regex, RegexInterface
	{
		public ScannerPartRegex (LexerBase scanner) : base (@"(?<g1>\/\*)|(?<g2>\[\[)|(?<g3>\/\/[^\n]*)|(?<g4>\%\{)|(?<g5>\%\})|(?<g6>skip)|(?<g7>terminal)|(?<g8>event)|(?<g9>push)|(?<g10>pop)|(?<g11>[a-zA-Z_]([a-zA-Z_]|[0-9]|[-\.])*|<[a-zA-Z_]([a-zA-Z_]|[0-9]|[-\. ])*>)|(?<g12>[ \t]+)|(?<g13>.+)|(?<g14>[\n\r])", RegexOptions.ExplicitCapture)
		{
			Scanner = scanner;
		}

		public LexerBase Scanner { get; private set; }
		public static string Id { get; private set; } = "scanner_part_ctx";

		public delegate int scannerCallback (ScannerPartRegex regex, LexerBase scanner);



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
				Scanner.pushScanner (CommentRegex.Id, text);
				result = 0;
				break;
			case 2:
				Scanner.pushScanner (AttributeRegex.Id, text);
				result = 0;
				break;
			case 3:
				result = 0;
				break;
			case 4:
				result = AnglrDeclarations.tokens._left_part_bracket_;
				break;
			case 5:
				Scanner.popScanner (text);
				Scanner.popScanner (text);
				result = AnglrDeclarations.tokens._right_part_bracket_;
				break;
			case 6:
				result = AnglrDeclarations.tokens._skip_;
				break;
			case 7:
				result = AnglrDeclarations.tokens._ttoken_;
				break;
			case 8:
				result = AnglrDeclarations.tokens._event_;
				break;
			case 9:
				result = AnglrDeclarations.tokens._push_;
				break;
			case 10:
				result = AnglrDeclarations.tokens._pop_;
				break;
			case 11:
				result = AnglrDeclarations.tokens._identifier_;
				break;
			case 12:
				result = 0;
				break;
			case 13:
				result = AnglrDeclarations.tokens._regular_expression_;
				break;
			case 14:
				result = 0;
				break;

			}
			return (result != null) ? (result.Value, matchLength) : (0, matchLength);
		}
	}
}

