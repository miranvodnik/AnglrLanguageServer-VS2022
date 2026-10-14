//
//	This file was generated with ANGLR compiler
//
using System;
using System.Runtime.Serialization;

using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using Anglr.Lexer.Core;
using Tree.ScannerLib;

namespace Tree.Lexer
{

	public class TreeLexer : LexerBase
	{
		public TreeScanner TreeScanner { get { return (TreeScanner) regarray [treeScanner]; } }
		public object [] Info { get; private set; }
		public TreeLexer (TextReader textReader, object [] info = null)
		{
			Info = info;
			Init ();
			pushInput (textReader);
			pushScanner (treeScanner, "");
		}

		public TreeLexer (string [] lines, object [] info = null)
		{
			Info = info;
			Init ();
			pushInput (lines);
			pushScanner (treeScanner, "");
		}

		public TreeLexer (string line, object [] info = null)
		{
			Info = info;
			Init ();
			pushInput (line);
			pushScanner (treeScanner, "");
		}

		public void Init ()
		{
			regarray = new RegexInterface []
			{
				new TreeScanner (this),
			};
		}

		public override int ScannerNameToId (string name) => scannerIds [name];
		public override RegexInterface GetRegexInterface (int index) => regarray [index];
		// array of regular expression scanners
		private RegexInterface [] regarray = null;
		private Dictionary<string, int> scannerIds = new Dictionary<string, int>
		{
			{
				TreeScanner.Id,
				treeScanner
			},
		};

		// scanner codes
		public const int treeScanner = 0;
	}
}

