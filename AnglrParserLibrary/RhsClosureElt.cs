using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnglrLibrary
{
	[Serializable]
    public class RhsClosureElt
	{
        internal RhsClosureElt (RhsProductionNode p_rhsProductionNode)
		{
			(m_rhsProductionNode = p_rhsProductionNode).Used = true;
			m_followSet = new TokenSet ();
		}

        internal void Dispose ()
		{
			m_followSet.Dispose ();
		}

        public	RhsProductionNode rhsProductionNode { get { return m_rhsProductionNode; } }
        public	TokenSet getFollowSet { get { return m_followSet; } }
        internal bool unionFollowSet (TokenSet p_tokset) { return m_followSet.makeUnion (p_tokset); }

		private RhsProductionNode m_rhsProductionNode;
		private TokenSet m_followSet;
	}
}
