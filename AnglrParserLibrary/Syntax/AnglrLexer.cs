//
//	This file was generated with ANGLR compiler
//
using System;

using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using Anglr.Lexer.Core;
using Anglr.ScannerLib;

namespace Anglr.Lexer
{

	public class AnglrLexer : LexerBase
	{
		internal CommentScanner CommentScanner { get { return (CommentScanner) regarray [comment_scanner]; } }
		internal LineCommentScanner LineCommentScanner { get { return (LineCommentScanner) regarray [line_comment_scanner]; } }
		internal AttributeScanner AttributeScanner { get { return (AttributeScanner) regarray [attribute_scanner]; } }
		internal ScannerIdScanner ScannerIdScanner { get { return (ScannerIdScanner) regarray [scanner_id_scanner]; } }
		internal ScannerPartScanner ScannerPartScanner { get { return (ScannerPartScanner) regarray [scanner_part_scanner]; } }
		internal RegexIdScanner RegexIdScanner { get { return (RegexIdScanner) regarray [regex_id_scanner]; } }
		internal RegexPartScanner RegexPartScanner { get { return (RegexPartScanner) regarray [regex_part_scanner]; } }
		internal RegexBlockScanner RegexBlockScanner { get { return (RegexBlockScanner) regarray [regex_block_scanner]; } }
		internal RegexBlockPartScanner RegexBlockPartScanner { get { return (RegexBlockPartScanner) regarray [regex_block_part_scanner]; } }
		internal AnglrScanner AnglrScanner { get { return (AnglrScanner) regarray [anglrScanner]; } }
		public object [] Info { get; private set; }
		public AnglrLexer (TextReader textReader, object [] info = null)
		{
			Info = info;
			Init ();
			pushInput (textReader);
			pushScanner (anglrScanner, "");
		}

		public AnglrLexer (string [] lines, object [] info = null)
		{
			Info = info;
			Init ();
			pushInput (lines);
			pushScanner (anglrScanner, "");
		}

		public AnglrLexer (string line, object [] info = null)
		{
			Info = info;
			Init ();
			pushInput (line);
			pushScanner (anglrScanner, "");
		}

		public void Init ()
		{
			regarray = new RegexInterface []
			{
				new CommentScanner (this),
				new LineCommentScanner (this),
				new AttributeScanner (this),
				new ScannerIdScanner (this),
				new ScannerPartScanner (this),
				new RegexIdScanner (this),
				new RegexPartScanner (this),
				new RegexBlockScanner (this),
				new RegexBlockPartScanner (this),
				new AnglrScanner (this),
			};
		}

		public override int ScannerNameToId (string name) => scannerIds [name];
		public override RegexInterface GetRegexInterface (int index) => regarray [index];
		// array of regular expression scanners
		private RegexInterface [] regarray = null;
		private Dictionary<string, int> scannerIds = new Dictionary<string, int>
		{
			{
				CommentScanner.Id,
				comment_scanner
			},
			{
				LineCommentScanner.Id,
				line_comment_scanner
			},
			{
				AttributeScanner.Id,
				attribute_scanner
			},
			{
				ScannerIdScanner.Id,
				scanner_id_scanner
			},
			{
				ScannerPartScanner.Id,
				scanner_part_scanner
			},
			{
				RegexIdScanner.Id,
				regex_id_scanner
			},
			{
				RegexPartScanner.Id,
				regex_part_scanner
			},
			{
				RegexBlockScanner.Id,
				regex_block_scanner
			},
			{
				RegexBlockPartScanner.Id,
				regex_block_part_scanner
			},
			{
				AnglrScanner.Id,
				anglrScanner
			},
		};

		// scanner codes
		public const int comment_scanner = 0;
		public const int line_comment_scanner = 1;
		public const int attribute_scanner = 2;
		public const int scanner_id_scanner = 3;
		public const int scanner_part_scanner = 4;
		public const int regex_id_scanner = 5;
		public const int regex_part_scanner = 6;
		public const int regex_block_scanner = 7;
		public const int regex_block_part_scanner = 8;
		public const int anglrScanner = 9;
	}
}

