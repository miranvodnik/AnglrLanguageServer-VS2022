//
//	This file was generated with ANGLR compiler
//
using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace Anglr.Lexer.Core
{
	public interface RegexInterface
	{
		(int, int) match (string currentLine, int currentPosition);
	}

    public interface InputDriver
	{
		string ReadLine ();
	}

    public class DefaultDriver : InputDriver
	{
		public string ReadLine ()
		{
			return null;
		}
	}

    public class FileDriver : InputDriver
	{
		public FileDriver (TextReader textReader)
		{
			this.textReader = textReader;
		}
		public string ReadLine ()
		{
			return textReader.ReadLine ();
		}
		private TextReader textReader;
	}

    public class StringArrayDriver : InputDriver
	{
		public StringArrayDriver (string [] lines)
		{
			this.lines = lines;
			lineCounter = 0;
		}
		public string ReadLine ()
		{
			if (lineCounter < lines.Length)
				return lines [lineCounter++];
			return null;
		}
		private int lineCounter;
		private string [] lines;
	}

    public class StringDriver : InputDriver
	{
		public StringDriver(string line)
		{
			this.line = line;
			ind = false;
		}
		public string ReadLine ()
		{
			if (ind)
				return null;
			ind = true;
			return line;
		}
		private bool ind;
		private string line;
	}

	public class LexerBase
	{
		public virtual int ScannerNameToId (string name) => -1;
        public virtual RegexInterface GetRegexInterface (int index) => null;

		public int scan ()
		{
			token = secondary = -1;
			try
			{
				return token = _scan ();
			}
			finally
			{
				scannerLeaveEvent?.Invoke (token);
			}
		}

		private int _scan ()
		{
			text = "";
			while (true)
			{
				while (currentPosition >= currentLength)
				{
					if ((currentLine = inputDriver.ReadLine ()) == null)
					{
						if (popInput ())
							continue;
						return -1;
					}
					lineno = ++internalLineno;
					internalColumn = 0;
					currentLine += '\n';
					currentPosition = 0;
					currentLength = currentLine.Length;
				}
				column = internalColumn;
				int? token = scannerEnterEvent?.Invoke ();
				if ((token != null) && (token.Value > 0))
					return token.Value;
				(int token, int length) result = regex.match (currentLine, currentPosition);
				int length = result.length;
				scase = result.token;
				text = currentLine.Substring (currentPosition, length);
				if (length <= 0)
					length = 1;
				internalColumn += length;
				currentPosition += length;
				scannerTokenEvent?.Invoke (regexIndex, result.token, text);
				if (scase <= 0)
					continue;
				return result.token;
			}
		}

		public void pushScanner (string regexName, string text) => pushScanner (ScannerNameToId (regexName), text);

		public void pushScanner (int regexIndex, string text)
		{
			scannerPushEvent?.Invoke (this.regexIndex, regexIndex, text);
			regqueue.Push (this.regexIndex = regexIndex);
			this.regex = GetRegexInterface (regexIndex);
		}

		public void popScanner (string text)
		{
			int index = regexIndex;
			regqueue.Pop ();
			regex = GetRegexInterface (regexIndex = regqueue.Peek ());
			scannerPopEvent?.Invoke (index, regexIndex, text);
		}

		public void pushInput (TextReader textReader)
		{
			inputDrivers.Push ((inputDriver, currentLine, currentPosition, currentLength, internalLineno, internalColumn));
			inputDriver = new FileDriver (textReader);
			currentLine = "";
			currentPosition = 0;
			currentLength = 0;
			internalLineno = 0;
			internalColumn = 0;
		}

		public void pushInput (string [] lines)
		{
			inputDrivers.Push ((inputDriver, currentLine, currentPosition, currentLength, internalLineno, internalColumn));
			inputDriver = new StringArrayDriver (lines);
			currentLine = "";
			currentPosition = 0;
			currentLength = 0;
			internalLineno = 0;
			internalColumn = 0;
		}

		public void pushInput (string line)
		{
			inputDrivers.Push ((inputDriver, currentLine, currentPosition, currentLength, internalLineno, internalColumn));
			inputDriver = new StringDriver (line);
			currentLine = "";
			currentPosition = 0;
			currentLength = 0;
			internalLineno = 0;
			internalColumn = 0;
		}

		public bool popInput ()
		{
			if (inputDrivers.Count <= 0)
				return false;
			(inputDriver, currentLine, currentPosition, currentLength, internalLineno, internalColumn) = inputDrivers.Peek ();
			inputDrivers.Pop ();
			return inputDrivers.Count > 0;
		}

		public delegate int scannerEnterCallback ();
		public delegate void scannerLeaveCallback (int token);
		public delegate void scannerPushCallback (int oldCtx, int newCtx, string text);
		public delegate void scannerPopCallback (int oldCtx, int newCtx, string text);
		public delegate void scannerTokenCallback (int ctx, int token, string text);

		public event scannerEnterCallback scannerEnterEvent;
		public event scannerLeaveCallback scannerLeaveEvent;
		public event scannerPushCallback scannerPushEvent;
		public event scannerPopCallback scannerPopEvent;
		public event scannerTokenCallback scannerTokenEvent;

		private InputDriver inputDriver = new DefaultDriver ();
		private string currentLine = "";
		private int currentPosition = 0;
		private int currentLength = 0;
		private int internalLineno = 0;
		private int internalColumn = 0;
		private Stack<int> regqueue = new Stack<int> ();
		private Stack<(InputDriver, string, int, int, int, int)> inputDrivers = new Stack<(InputDriver, string, int, int, int, int)> ();
		private RegexInterface regex = null;
		private Regex anreg = null;
		private bool anregInd = false;

		public int regexIndex { get; private set; } = -1;
		public int scase { get; private set; } = -1;
		public int lineno { get; private set; } = 0;
		public int column { get; private set; } = 0;
		public int token { get; private set; } = -1;
		public int secondary { get; set; } = -1;
		public string text { get; set; } = "";
	}
}
