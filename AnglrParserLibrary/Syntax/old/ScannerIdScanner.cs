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
	public class ScannerIdScanner : Regex, RegexInterface
	{
		public ScannerIdScanner (LexerBase scanner) : base (@"(?<g1>\/\*)|(?<g2>\[\[)|(?<g3>\/\/)|(?<g4>[a-zA-Z_]([a-zA-Z_]|[0-9]|[-\.])*|<[a-zA-Z_]([a-zA-Z_]|[0-9]|[-\. ])*>)|(?<g5>[ \t]+)|(?<g6>[\n\r])", RegexOptions.ExplicitCapture)
		{
			Scanner = scanner;
		}

		public LexerBase Scanner { get; private set; }
		public static string Id { get; private set; } = "scanner_id_scanner";

		public delegate int scannerCallback (ScannerIdScanner regex, LexerBase scanner);



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
				Scanner.pushScanner (ScannerPartScanner.Id, text);
				result = AnglrDeclarations.tokens._identifier_;
				break;
			case 5:
				result = 0;
				break;
			case 6:
				result = 0;
				break;

			}
			return (result != null) ? (result.Value, matchLength) : (0, matchLength);
		}
	}
}

