using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnglrLibrary
{
	[Serializable]
	public class SymbolToken
	{
		public SymbolToken (string name, uint declarator = 0, SymbolToken tag = null, SymbolToken context = null, bool correctName = true, int lineno = -1, int colnum = -1)
		{
			this.lineno = lineno;
			this.colnum = colnum;
			m_name = name;
			m_declarator = declarator;
			m_tag = tag;
			m_id = ++g_id;
			this.context = context;

			if (correctName)
			{
				foreach (char c in m_name)
					m_correctName += (char.IsLetterOrDigit (c)) ? c : '_';
			}
			else
				m_correctName = m_name;
		}

		public void Display (TextWriter textWriter, string message)
		{
            textWriter.WriteLine ($"{message}, name = {name}, declarator = {declarator}");
            textWriter.WriteLine ($"\tline = {lineno}, column = {colnum}");
            textWriter.WriteLine ($"\tline = {lineno}, column = {colnum}");
            textWriter.Write ($"\tdefined:");
            foreach (SymbolReference symbolReference in m_deflist)
                textWriter.Write ($" ({symbolReference.lineno}, {symbolReference.column})");
            textWriter.WriteLine ();
            textWriter.Write ($"\tused:   ");
            foreach (SymbolReference symbolReference in m_reflist)
                textWriter.Write ($" ({symbolReference.lineno}, {symbolReference.column})");
            textWriter.WriteLine ();
			textWriter.Flush ();
        }

        public void Dispose ()
		{
		}

		public int hashCode
		{
			get
			{
				if (m_hashCode != -1)
					return m_hashCode;
				return m_hashCode = name.GetHashCode () + ((context != null) ? context.hashCode : 0);
			}
		}
		private int m_hashCode = -1;

		public int lineno { get; private set; } = -1;
		public int colnum { get; private set; } = -1;
		public bool displayed { get { return m_displayed; } set { m_displayed = value; } }
		public string name { get { return m_name; } }
		public string correctName { get { return m_correctName; } set { m_correctName = value; } }
		public uint declarator { get { return m_declarator; } set { m_declarator = value; } }
		public int id { get { return m_id; } set { if (value >= 0) m_id = value; } }
		public int index { get { return m_index; } set { if (value >= 0) m_index = value; } }
		public SymbolToken alias { get { return m_alias; } set { m_alias = value; } }
		public SymbolToken tag { get { return m_tag; } set { m_tag = value; } }
		public SymbolToken context { get; set; } = null;
		public uint precedence { get { return m_precedence; } set { m_precedence = value; } }
		public uint associativity { get { return m_associativity; } set { m_associativity = value; } }
		public string code { get { return m_code; } set { m_code = value; } }

		public bool AliasFlag
		{
			get => (m_flags & FlagAlias) != 0;
			set => _ = value ? m_flags |= FlagAlias : m_flags &= ~FlagAlias;
		}
        public bool TokenFlag
        {
            get => (m_flags & FlagToken) != 0;
            set => _ = value ? m_flags |= FlagToken : m_flags &= ~FlagToken;
        }
        public bool NonterminalFlag
        {
            get => (m_flags & FlagNonterminal) != 0;
            set => _ = value ? m_flags |= FlagNonterminal : m_flags &= ~FlagNonterminal;
        }
        public bool IteratorFlag
        {
            get => (m_flags & FlagIterator) != 0;
            set => _ = value ? m_flags |= FlagIterator : m_flags &= ~FlagIterator;
        }
        public bool IteratorAttributeFlag
        {
            get => (m_flags & FlagIteratorAttribute) != 0;
            set => _ = value ? m_flags |= FlagIteratorAttribute : m_flags &= ~FlagIteratorAttribute;
        }
        public bool IdentityFlag
        {
            get => (m_flags & FlagIdentity) != 0;
            set => _ = value ? m_flags |= FlagIdentity : m_flags &= ~FlagIdentity;
        }
        public bool CascadeFlag
        {
            get => (m_flags & FlagCascade) != 0;
            set => _ = value ? m_flags |= FlagCascade : m_flags &= ~FlagCascade;
        }
        public bool CascadeUsageFlag
        {
            get => (m_flags & FlagCascadeUsage) != 0;
            set => _ = value ? m_flags |= FlagCascadeUsage : m_flags &= ~FlagCascadeUsage;
        }
        public bool SymbolUsageFlag
        {
            get => (m_flags & FlagSymbolUsage) != 0;
            set => _ = value ? m_flags |= FlagSymbolUsage : m_flags &= ~FlagSymbolUsage;
        }

        public bool isDefined { get => TokenFlag || NonterminalFlag; }

		public void AddDefInfo (int lineno, int column, int length) => m_deflist.Add (new SymbolReference (lineno - 1, column, length, false));

		public void AddRefInfo (int lineno, int column, int length) => m_reflist.Add (new SymbolReference (lineno - 1, column, length, true));

		public const uint FlagAlias = (1 << 0);
		public const uint FlagToken = (1 << 1);
        public const uint FlagNonterminal = (1 << 2);
        public const uint FlagIterator = (1 << 3);
        public const uint FlagIteratorAttribute = (1 << 4);
        public const uint FlagIdentity = (1 << 5);
        public const uint FlagCascade = (1 << 6);
        public const uint FlagCascadeUsage = (1 << 7);
        public const uint FlagSymbolUsage = (1 << 8);

        string m_name;
		uint m_declarator;
		SymbolToken m_tag;
		int m_id;

		private static int g_id = 0;
		bool m_displayed = false;
		string m_correctName = "";
		uint m_flags = 0;
		int m_index = -1;
		SymbolToken m_alias = null;
		uint m_precedence = 0;
		uint m_associativity = 0;
		string  m_code = "";
		public reflist m_deflist { get; } = new reflist ();
		public reflist m_reflist { get; } = new reflist();
	}
}
