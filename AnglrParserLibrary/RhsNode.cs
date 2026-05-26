using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnglrLibrary
{
	[Serializable]
    public class RhsNode
	{
        internal RhsNode (SymbolToken p_symbolToken)
		{
			symbolToken = p_symbolToken;
			id = ++g_id;
			opened = false;
			index = 0;
            transitionSet = new TokenSet ();
        }
        internal void Dispose ()
		{
			transitionSet.Dispose ();
		}
        internal bool insertTransitionSet (SymbolToken p_symbolToken) { return transitionSet.insert (p_symbolToken); }
        internal bool unionTransitionSet (TokenSet p_tokset) { return transitionSet.makeUnion (p_tokset); }

        public SymbolToken symbolToken { get; set; }
        internal TokenSet transitionSet { get; }
        internal bool opened { get; set; }
        internal int index { get; set; }
        internal int id { get; }

        private static int g_id = 0;
	}
}
