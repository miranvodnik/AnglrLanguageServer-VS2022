using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnglrLibrary
{
	[Serializable]
	public class RhsConfiguration
	{
        internal RhsConfiguration (RhsProduction p_rhsProduction, rhsenumerator p_rhsIterator, bool create = true)
		{
			m_rhsProduction = p_rhsProduction;
			m_rhsIterator = p_rhsIterator;
			m_followSet = create ? new TokenSet () : null;
		}

        internal void Dispose ()
		{
			m_followSet.Dispose ();
		}

        internal int hashCode
		{
			get
			{
				if (m_hashCode != -1)
					return m_hashCode;
				m_hashCode = m_rhsProduction.hashCode;
				if (m_rhsIterator.atEnd)
					m_hashCode += 1;
				else
					m_hashCode *= m_rhsIterator.currentRhsNode.symbolToken.hashCode;
				return m_hashCode;
			}
		}
		private int m_hashCode = -1;
        public RhsProduction rhsProduction { get { return m_rhsProduction; } }
        public rhsenumerator rhsIterator { get { return m_rhsIterator; } }
        public TokenSet getFollowSet { get { return m_followSet; } }
        internal bool addFollowSet (SymbolToken p_symbolToken) { return m_followSet.insert (p_symbolToken); }
        internal bool unionFollowSet (TokenSet p_tokset) { return m_followSet.makeUnion (p_tokset); }

		private RhsProduction m_rhsProduction;
		private rhsenumerator m_rhsIterator;
		private TokenSet m_followSet;
}
}
