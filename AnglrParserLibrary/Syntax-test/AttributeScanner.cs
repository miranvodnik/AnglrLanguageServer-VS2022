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
	internal class AttributeScanner : Regex, RegexInterface
	{
		public AttributeScanner (LexerBase scanner) : base (@"(?<g1>\]\])|(?<g2>[a-zA-Z_]([a-zA-Z_]|[0-9]|[-\.])*|<[a-zA-Z_]([a-zA-Z_]|[0-9]|[-\. ])*>)|(?<g3>\=)|(?<g4>\""([^""]|\""\"")*\""|\'([^']|\'\')*\')|(?<g5>[ \t\n\r])", RegexOptions.ExplicitCapture)
		{
			Scanner = scanner;
		}

		public LexerBase Scanner { get; private set; }
		public static string Id { get; private set; } = "attribute_scanner";

		public delegate int scannerCallback (AttributeScanner regex, LexerBase scanner);



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
				Scanner.popScanner (text);
				result = 0;
				break;
			case 2:
				result = 0;
				break;
			case 3:
				result = 0;
				break;
			case 4:
				result = 0;
				break;
			case 5:
				result = 0;
				break;

			}
			return (result != null) ? (result.Value, matchLength) : (0, matchLength);
		}
	}
}

