using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnglrLibrary
{
	[Serializable]
	public class TokenSet
	{
		internal TokenSet ()
		{
			++g_setCount;
		}
        internal void Dispose ()
		{
			m_tokset.Clear ();
			--g_setCount;
		}

        internal bool insert (SymbolToken p_symbolToken)
		{
			if (m_tokset.Keys.Contains (p_symbolToken))
				return false;
			m_tokset [p_symbolToken] = p_symbolToken;
			return true;
		}

        internal bool check (SymbolToken p_symbolToken)
		{
			return m_tokset.Keys.Contains (p_symbolToken);
		}

        internal bool makeUnion (TokenSet p_tokenSet)
		{
			bool inserted = false;
			tokset p_tokset = p_tokenSet.set;
			foreach (SymbolToken p_SymbolToken in p_tokset.Keys)
			{
				if (m_tokset.Keys.Contains (p_SymbolToken))
					continue;
				m_tokset [p_SymbolToken] = p_SymbolToken;
				inserted = true;
			}
			return inserted;
		}

        internal void display (StringBuilder sb)
		{
			int count = 0;
			foreach (SymbolToken p_SymbolToken in m_tokset.Keys)
			{
				if (p_SymbolToken.alias != null)
					sb.Append (((count > 0) ? ", " : "") + p_SymbolToken.alias.name);
				else
                    sb.Append (((count > 0) ? ", " : "") + p_SymbolToken.name);
				++count;
			}
		}

        internal tokset set { get { return m_tokset; } }

		private static int g_setCount = 0;
		public tokset m_tokset { get; private set; } = new tokset ();
	}
}
