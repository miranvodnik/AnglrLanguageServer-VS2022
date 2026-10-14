//
//	This file was generated with ANGLR compiler
//
using System;
using System.Runtime.Serialization;

﻿using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using Anglr.Lexer.Core;
using Tree.Declarations;

namespace Tree.ScannerLib
{
	public class TreeScanner : Regex, RegexInterface
	{
		public TreeScanner (LexerBase scanner) : base (@"(?<g1>\{)|(?<g2>\})|(?<g3>\:)|(?<g4>\,)|(?<g5>\""([^""]|\""\"")*\""|\'([^']|\'\')*\')|(?<g6>[0-9]+)|(?<g7>null)|(?<g8>[ \t\n\r])", RegexOptions.ExplicitCapture)
		{
			Scanner = scanner;
		}

		public LexerBase Scanner { get; private set; }
		public static string Id { get; private set; } = "treeScanner";

		public delegate int scannerCallback (TreeScanner regex, LexerBase scanner);



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
				result = TreeDeclarations.tokens._left_curly_bracket_;
				break;
			case 2:
				result = TreeDeclarations.tokens._right_curly_bracket_;
				break;
			case 3:
				result = TreeDeclarations.tokens._colon_;
				break;
			case 4:
				result = TreeDeclarations.tokens._coma_;
				break;
			case 5:
				result = TreeDeclarations.tokens._cstring_;
				break;
			case 6:
				result = TreeDeclarations.tokens._number_;
				break;
			case 7:
				result = TreeDeclarations.tokens._null_;
				break;
			case 8:
				result = 0;
				break;

			}
			return (result != null) ? (result.Value, matchLength) : (0, matchLength);
		}
	}
}

