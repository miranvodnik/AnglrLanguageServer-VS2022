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
	internal class RegexBlockScanner : Regex, RegexInterface
	{
		public RegexBlockScanner (LexerBase scanner) : base (@"(?<g1>\/\*)|(?<g2>\[\[)|(?<g3>\/\/)|(?<g4>[ \t\n]+)|(?<g5>\})|(?<g6>[a-zA-Z_]([a-zA-Z_]|[0-9]|[-\.])*|<[a-zA-Z_]([a-zA-Z_]|[0-9]|[-\. ])*>)", RegexOptions.ExplicitCapture)
		{
			Scanner = scanner;
		}

		public LexerBase Scanner { get; private set; }
		public static string Id { get; private set; } = "regex_block_scanner";

		public delegate int scannerCallback (RegexBlockScanner regex, LexerBase scanner);



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
				result = 0;
				break;
			case 5:
				Scanner.popScanner (text);
				Scanner.popScanner (text);
				result = AnglrDeclarations.tokens._right_curly_bracket_;
				break;
			case 6:
				Scanner.pushScanner (RegexBlockPartScanner.Id, text);
				result = AnglrDeclarations.tokens._identifier_;
				break;

			}
			return (result != null) ? (result.Value, matchLength) : (0, matchLength);
		}
	}
}

