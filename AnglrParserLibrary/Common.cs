using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnglrLibrary
{

	[Serializable]
	internal class Constants
	{
		public const int TOKEN_ACCEPT = 0;
		public const int TOKEN_EOF = 1;
		public const int TOKEN_ERROR = 2;
		public const int TOKEN_START = 3;
	}

	/// <summary>
	/// comparer of SymbolToken instances
	/// </summary>
	[Serializable]
	internal class cmpsymbol : IEqualityComparer<SymbolToken>
	{
		/// <summary>
		/// SymbolToken instances comparer: they are equal iff their id-s are equal
		/// </summary>
		/// <param name="x">first SymbolToken instance</param>
		/// <param name="y">second SymbolToken instance</param>
		/// <returns>true - SymbolToken instance are equal (have equal id-s); false - otherwise</returns>
		public bool Equals (SymbolToken x, SymbolToken y)
		{
			return x.id == y.id;
		}

		/// <summary>
		/// This function is required by IEqualityComparer interface. It is actually
		/// wrappr aroud String's function GetHashCode() since it returns the value
		/// of SymbolToken instance property hashCode which actuallly does one shot
		/// call of GetHashCode() function
		/// </summary>
		/// <param name="obj">SymbolToken object reference</param>
		/// <returns>hash code of name property of given object</returns>
		public int GetHashCode (SymbolToken obj)
		{
			return obj.hashCode;
		}
	};

	/// <summary>
	/// dictionary of self referenced SymbolToken objects. Every object can be accessed
	/// by it's own reference. Dictionary cannot contain SymbolToken objects with equal
	/// id properties since this property is used as exclusion criteria in equality
	/// comparator class cmpsymbol which is used to construct this dictionary
	/// </summary>
	[Serializable]
	public class tokset : Dictionary <SymbolToken, SymbolToken>
	{
		/// <summary>
		/// dictionary constructor using cmpsymbol equality comparator
		/// </summary>
		internal tokset () : base (new cmpsymbol ()) { }
	}

	/// <summary>
	/// list of SymbolToken objects
	/// </summary>
	[Serializable]
	public class tokvec : List <SymbolToken>
	{

	}

	/// <summary>
	/// another equality comparator for SymbolToken objects
	/// </summary>
	[Serializable]
	internal class cmpsym : IEqualityComparer<SymbolToken>
	{
		/// <summary>
		/// this comparator is defined in the following way:
		/// two SymbolToken object are equal iff:
		///	- their names are equal
		///	- and their positions are not defined or are equal
		///	- and their declarators are not defined or are equal
		///	- and their context symbol definitions are undefined or are equal
		/// </summary>
		/// <param name="x">first SymbolToken instance</param>
		/// <param name="y">second SymbolToken instance</param>
		/// <returns>true - SymbolToken instance are equal (have equal name-s); false - otherwise</returns>
		public bool Equals (SymbolToken x, SymbolToken y)
		{
			return (x.name == y.name) &&	// names must be equal
				((x.lineno < 0) || (x.colnum < 0) || (y.lineno < 0) || (y.colnum < 0) || (x.lineno == y.lineno) && (x.colnum == y.colnum)) &&	// defined positions must be equal
				((x.declarator == 0) || (y.declarator == 0) || (x.declarator == y.declarator)) &&	// defined declarators must be equal
				(!((x.context != null) && (y.context != null)) || Equals (x.context, y.context));	// defined context symbols must be equal
		}

		/// <summary>
		/// This function is required by IEqualityComparer interface. It is actually
		/// wrappr aroud String's function GetHashCode() since it returns the value
		/// of SymbolToken instance property hashCode which actuallly does one shot
		/// call of GetHashCode() function
		/// </summary>
		/// <param name="obj">SymbolToken object reference</param>
		/// <returns>hash code of name property of given object</returns>
		public int GetHashCode (SymbolToken obj)
		{
			return obj.hashCode;
		}
	};

	/// <summary>
	/// another dictionar containing SymbolToken objects (with different name propertes)
	/// </summary>
	[Serializable]
	internal class symtab : Dictionary <SymbolToken, SymbolToken>
	{
		/// <summary>
		/// dictionary constructor using cmpsym equality comparator
		/// </summary>
		public symtab () : base (new cmpsym ()) { }
	}

	/// <summary>
	/// RhsNode list, representing single production of any syntax rule.
	/// Every symbol (terminal or nonterminal) in production of syntax 
	/// rule is associated with exactly one RhsNode object in this list
	/// </summary>
	[Serializable]
    public class rhslist : List<RhsNode>
	{

	}

	/// <summary>
	/// enumerator (or iterator) of rhslist (list of RhsNodes).
	/// It is used to define <c>position</c> within rhslist. This <c>position</c>
	/// implies the meaning of <c>current node</c> and the <c>end of list indicator</c>.
	/// </summary>
	[Serializable]
	public class rhsenumerator
	{
		/// <summary>
		/// enumerator constructor - given some RhsNode list it initializes it's
		/// <c>position</c>:
		/// <list type="bullet">
		/// <item>current node</item> - first RhsNode in the list or null if this list is empty
		/// <item>end of list indicator</item> - true if list is empty, false otherwise
		/// </list>
		/// </summary>
		/// <param name="p_rhslist">reference to RhsNode list</param>
		public rhsenumerator (rhslist p_rhslist)
		{
			rhslistRef = p_rhslist;
			rhslistEnum = p_rhslist.GetEnumerator ();
			atEnd = !rhslistEnum.MoveNext ();
			currentRhsNode = rhslistEnum.Current;
			position = 0;
		}

		/// <summary>
		/// copy constructor of enumerator - it takes the same RhsNode list 
		/// as original one and advances one step if it is not at the end of list
		/// </summary>
		/// <param name="p_rhsiterator">original enumerator</param>
		public rhsenumerator (rhsenumerator p_rhsiterator)
		{
			rhslistRef = p_rhsiterator.rhslistRef;
			rhslistEnum = p_rhsiterator.rhslistEnum;
			atEnd = p_rhsiterator.atEnd;
			position = p_rhsiterator.position;
			if (!atEnd)
			{
				atEnd = !rhslistEnum.MoveNext ();
				++position;
			}
			currentRhsNode = rhslistEnum.Current;
		}

		/// <summary>
		/// RhsNode list representing production of syntax rule
		/// </summary>
		public rhslist rhslistRef { get; private set; }
		/// <summary>
		/// rhslist (actually List) enumerator
		/// </summary>
		private rhslist.Enumerator rhslistEnum;
		/// <summary>
		/// end of list indicator: MoveNext() cannot advance
		/// </summary>
		public bool atEnd { get; private set; }
		/// <summary>
		/// <c>current</c> RhsNode
		/// </summary>
		public RhsNode currentRhsNode { get; private set; }
		/// <summary>
		/// index (numerical position starting with 0) of current node
		/// </summary>
		public int position { get; private set; }
	}

	/// <summary>
	/// list of RhsProduction-s. Syntax rule is represented with such list
	/// </summary>
	[Serializable]
	public class productions : List<RhsProduction>
	{

	}

	/// <summary>
	/// dictionary of integer numbers indexed with integers. This dictionary
	/// is used to count the number of 'goto' transitions in states of
	/// push-down automata representing generated parser. Keys in this
	/// dictionary represent states, Value-s represent number of 'goto'
	/// transitions in given state.
	/// </summary>
	[Serializable]
	internal class gotocnt : Dictionary< int, int >
	{

	}

	/// <summary>
	/// equality comparer of two arbitrary productions. Simply stated:
	/// two productions are equal if they are composed of pairwise
	/// equally named symbols: they have the same length; symbols at
	/// same position have equal names. This is strong definition of
	/// equality. But this comparer is slightly less accurate: it does
	/// not care about terminal symbols. This loosening of strong
	/// definition is meaningfull in traversal procedures of syntax 
	/// tree (both, procedurs and syntax tree, are generated by parser
	/// generator). Terminal codes are allways leaves of syntax tree
	/// and are never traversed by traversal procedures. Traversal procedures
	/// are provided only for non-terminal symbols, which represent
	/// syntax rules. In this sense, traversal procedures does not
	/// distinguish between two productions which differ only in
	/// terminal names. That is why the definition of this equality
	/// comparer makes sense. Parser generator uses this comparer to
	/// generate same traversal procedure for all productions equal
	/// in the sense of this comparator.
	/// </summary>
	[Serializable]
	internal class cmpProduction : IEqualityComparer<RhsProduction>
	{
		/// <summary>
		/// equality comparer for two productions, comparing them in the
		/// sense described in the description of cmpProduction class
		/// </summary>
		/// <param name="x">first production reference</param>
		/// <param name="y">second production reference</param>
		/// <returns>true - productions are equal in the described sense, false - otherwise</returns>
		public bool Equals (RhsProduction x, RhsProduction y)
		{
			rhslist xnodes = x.rhsNodes;
			rhslist ynodes = y.rhsNodes;

			if (xnodes.Count != ynodes.Count)
				return false;

			rhslist.Enumerator xenum = xnodes.GetEnumerator ();
			rhslist.Enumerator yenum = ynodes.GetEnumerator ();

			while (xenum.MoveNext () && yenum.MoveNext ())
			{
				SymbolToken xsym = xenum.Current.symbolToken;
				SymbolToken ysym = yenum.Current.symbolToken;
				uint declarator;
				if ((declarator = xsym.declarator) != ysym.declarator)
					return false;
				if (declarator != (uint) AnglrClassificationType.NonTerminalName)
					continue;
				if (xsym.id != ysym.id)
					return false;
			}
			return true;
		}

		public bool EqualsLiteraly (RhsProduction x, RhsProduction y)
		{
			rhslist xnodes = x.rhsNodes;
			rhslist ynodes = y.rhsNodes;

			if (xnodes.Count != ynodes.Count)
				return false;

			rhslist.Enumerator xenum = xnodes.GetEnumerator ();
			rhslist.Enumerator yenum = ynodes.GetEnumerator ();

			while (xenum.MoveNext () && yenum.MoveNext ())
			{
				SymbolToken xsym = xenum.Current.symbolToken;
				SymbolToken ysym = yenum.Current.symbolToken;
				if (!xsym.name.Equals(ysym.name))
					return false;
			}
			return true;
		}

		public int GetHashCode (RhsProduction obj)
		{
			return obj.hashCode;
		}
	}

	/// <summary>
	/// dictionary, containing numbers of equal productions in the
	/// sense of cmpProduction comparer, indexed by production reference.
	/// Simply stated: keys of this dictionary represent all different
	/// productions in the sense of cmpProduction comparer. Values
	/// associated to these keys represent number of productions
	/// which possibly differ only in terminal names.
	/// </summary>
	[Serializable]
	internal class protoset : Dictionary<RhsProduction, int>
	{
		public protoset () : base (new cmpProduction ()) { }
	}

	/// <summary>
	/// pair of possibly different type values;
	/// should be sometime replaced with c# tuple (T, V)
	/// </summary>
	/// <typeparam name="T">type of first value</typeparam>
	/// <typeparam name="V">type of second value</typeparam>
	[Serializable]
	internal class pair<T, V>
	{
		public pair (T t, V v)
		{
			first = t;
			second = v;
		}

		public T first { get; set; }
		public V second { get; set; }
	}

	/// <summary>
	/// pair (tuple) of two integer values
	/// </summary>
	[Serializable]
	internal class symindex : pair <int, int>
	{
		public symindex (int t, int v) : base (t, v) { }
	}

	/// <summary>
	/// dictionary of integer pairs indexed by SymbolToken references. This
	/// dictionary is used in class RhsProductionNode, which represents
	/// collection of syntax productions constituting some syntax rule.
	/// These productions are composed of different symbols. It is not
	/// uncommon, that are some symbols stated more than once in some
	/// productions. Code generator must distinguish symbols stated
	/// multiple times in single production. In fact, it must know the
	/// maximum number of occurences for any symbol in single production
	/// for all productions in given rule. This dictionary plays the
	/// major role in algorithms to compute this numbers and to generate
	/// code dependent of these numbers.
	/// </summary>
	[Serializable]
	internal class symset : Dictionary<SymbolToken, symindex>
	{
		public symset () : base (new cmpsymbol ()) { }
	}

	/// <summary>
	/// dictionary of push-down automata (PDA) states indexed by SymbolToken references.
	/// This dictionary is used to represent transition sets. Every PDA state has such
	/// set, even if it is empty. This set describes all possible transitions from given 
	/// PDA state to other PDA states. Key-Value pairs of this set represent event (Key)
	/// and PDA state (Value) in which PDA transitioned, when this event is fired in PDA
	/// state associated with that set. Transition set is, for practical reasons, divided
	/// in two sets: shift and goto set. The difference between these sets is in the
	/// nature of their keys. Shift set keys are terminal symbol references, while goto
	/// set keys are non-terminal symbol references. The distinction between them plays
	/// the key role in generating parser source code.
	/// </summary>
	[Serializable]
	internal class stateset : Dictionary<SymbolToken, RhsState>
	{
		public stateset () : base (new cmpsymbol ()) { }

		public int hashCode
		{
			get
			{
				if (m_hashCode != -1)
					return m_hashCode;
				foreach (SymbolToken p_SymbolToken in Keys)
					m_hashCode += p_SymbolToken.hashCode;
				return m_hashCode;
			}
		}
		private int m_hashCode = -1;
	}

	/// <summary>
	/// list of PDA states
	/// </summary>
	[Serializable]
	public class statearray : List <RhsState>
	{

	}

	/// <summary>
	/// dictionary of RHS configurations, indexed by production numbers -
	/// unique number attached to every production of syntax rule.
	/// RHS configuration is composed of two parts:
	/// <list type="bullet">
	/// <item>production</item> - list of RhsNodes (rhslist)
	/// <item>position</item> - transition node in production (rhsenumerator)
	/// </list>
	/// This dictionary is used for very special purpose. It is
	/// used to collect productions associated with its final position,
	/// position which cannot move any further since we have reached the end
	/// of production. Such configurations should be reduced. Simply stated:
	/// this dictionary (with restrictions described above) contains productions
	/// which should be reduced in given PDA state. The decision which production
	/// will be reduced (one, more or none of them) depends of the current
	/// lookahed symbol, provided by scanner. Reductions are implemented through
	/// push-down step of PDA, which reduces the size of PDA stack by the length
	/// of production being reduced. Therefore, it can be said that these
	/// dictionaries play a key role in the design of push-down mechanism.
	/// 
	/// RHS configurations which contain productions with nontrivial position
	/// are used to form shift and goto sets, depending on SymbolToken in RhsNode
	/// object representing transition node.
	/// </summary>
	[Serializable]
	public class prodset : Dictionary<int, RhsConfiguration>
	{

	}

	[Serializable]
	internal class stateSetInfo
	{
		public int m_delta { get; set; } = -1;
		//public int m_min { get; set; } = -1;
		//public int m_max { get; set; } = -1;
	}

	[Serializable]
	internal class statedelta : pair <stateset, stateSetInfo>
	{
		public statedelta (stateset t, stateSetInfo v) : base (t, v) { }
	}

	[Serializable]
	internal class sscmp : IEqualityComparer<statedelta>
	{
		public bool Equals (statedelta x, statedelta y)
		{
			stateset xset = x.first;
			stateset yset = y.first;

			if (xset.Count != yset.Count)
				return false;

			stateset.Enumerator xenum = xset.GetEnumerator ();
			stateset.Enumerator yenum = yset.GetEnumerator ();

			while (xenum.MoveNext() && yenum.MoveNext())
			{
				if (xenum.Current.Key.id != yenum.Current.Key.id)
					return false;
				if (xenum.Current.Value.m_id != yenum.Current.Value.m_id)
					return false;
			}
			return true;
		}

		public int GetHashCode (statedelta obj)
		{
			return obj.first.hashCode;
		}
	}

	[Serializable]
	internal class shiftset : Dictionary<statedelta, statedelta>
	{
		public shiftset () : base (new sscmp ()) { }
	}

	[Serializable]
	internal class gotoset : Dictionary<statedelta, statedelta>
	{
		public gotoset () : base (new sscmp ()) { }
	}

	[Serializable]
	internal class rdlist : List<RhsProduction>
	{

	}

	[Serializable]
	internal class GLRToken
	{
		public GLRToken (int state)
		{
			m_state = state;
		}
		public void Dispose ()
		{
			m_rdlist.Clear ();
		}
		public int hashCode
		{
			get
			{
				if (m_hashCode != -1)
					return m_hashCode;
				m_hashCode = 0;
				foreach (RhsProduction p_RhsProduction in m_rdlist)
					m_hashCode += p_RhsProduction.hashCode;
				return m_hashCode;
			}
		}
		private int m_hashCode = -1;
		public int position { get { return m_position; } set { m_position = value; } }
		public int state { get { return m_state; } }
		public void add (RhsProduction p_RhsProduction) { m_rdlist.Add (p_RhsProduction); }
		public rdlist getRdlist { get { return m_rdlist; } }

		private int m_state;
		private int m_position = 0;
		private rdlist m_rdlist = new rdlist ();
	}

	[Serializable]
	internal class glrset : Dictionary<SymbolToken, GLRToken>
	{
		public glrset () : base (new cmpsymbol ()) { }
	}

	[Serializable]
	internal class cmpglrtok : IEqualityComparer<GLRToken>
	{
		public bool Equals (GLRToken x, GLRToken y)
		{
			if (x.state != y.state)
				return false;

			rdlist xlist = x.getRdlist;
			rdlist ylist = y.getRdlist;

			if (xlist.Count != ylist.Count)
				return false;

			rdlist.Enumerator xenum = xlist.GetEnumerator ();
			rdlist.Enumerator yenum = ylist.GetEnumerator ();

			while (xenum.MoveNext() && yenum.MoveNext())
			{
				if (xenum.Current.productionNumber != yenum.Current.productionNumber)
					return false;
			}
			return true;
		}

		public int GetHashCode (GLRToken obj)
		{
			return obj.hashCode;
		}
	}

	[Serializable]
	internal class glrtokset : Dictionary<GLRToken, GLRToken>
	{
		public glrtokset () : base (new cmpglrtok ()) { }
	}

	[Serializable]
	internal class prodlist : Dictionary <SymbolToken, RhsProductionNode>
	{
		public prodlist () : base (new cmpsymbol ()) { }
	}

	[Serializable]
	internal class cmpcfg : IEqualityComparer<RhsConfiguration>
	{
		public bool Equals (RhsConfiguration x, RhsConfiguration y)
		{
			if (x.rhsProduction.productionNumber != y.rhsProduction.productionNumber)
				return false;

			rhsenumerator xenum = x.rhsIterator;
			rhsenumerator yenum = y.rhsIterator;

			bool atEnd;

			if ((atEnd = xenum.atEnd) != yenum.atEnd)
				return false;

			if (!atEnd && ((xenum.position != yenum.position) || (xenum.currentRhsNode.symbolToken.id != yenum.currentRhsNode.symbolToken.id)))
				return false;

			return true;
		}

		public int GetHashCode (RhsConfiguration obj)
		{
			return obj.hashCode;
		}
	}

	[Serializable]
	public class rhscfgset : Dictionary<RhsConfiguration, RhsConfiguration>
	{
		public rhscfgset () : base (new cmpcfg ()) { }
		public int hashCode
		{
			get
			{
				if (m_hashCode != -1)
					return m_hashCode;
				m_hashCode = 0;
				foreach (RhsConfiguration cfg in Values)
					m_hashCode += cfg.hashCode;
				return m_hashCode;
			}
		}
		private int m_hashCode = -1;
	}

	[Serializable]
    public class closurelist : Dictionary<SymbolToken, RhsClosureElt>
	{
		public closurelist () : base (new cmpsymbol ()) { }
	}

	[Serializable]
	internal class prodarray : List<RhsClosureElt>
	{

	}

	[Serializable]
	internal class proddict : SortedDictionary<int, RhsProduction>
	{

	}

	[Serializable]
	internal class statequeue : Queue<RhsState>
	{

	}

	[Serializable]
	internal class cmprhscore : IEqualityComparer<rhscfgset>
	{
		public bool Equals (rhscfgset x, rhscfgset y)
		{
			if (x.Count != y.Count)
				return false;

			rhscfgset.Enumerator xenum = x.GetEnumerator ();
			rhscfgset.Enumerator yenum = y.GetEnumerator ();
			cmpcfg cmp = new cmpcfg ();

			while (xenum.MoveNext() && yenum.MoveNext())
			{
				RhsConfiguration xcfg = xenum.Current.Value;
				RhsConfiguration ycfg = yenum.Current.Value;
				if (!cmp.Equals (xcfg, ycfg))
					return false;
			}

			return true;
		}

		public int GetHashCode (rhscfgset obj)
		{
			return obj.hashCode;
		}
	}

	[Serializable]
	internal class cmpset : IEqualityComparer<RhsState>
	{
		public bool Equals (RhsState x, RhsState y)
		{
			cmprhscore cmp = new cmprhscore ();
			return cmp.Equals (x.core, y.core);
		}

		public int GetHashCode (RhsState obj)
		{
			return obj.core.hashCode;
		}
	}

	[Serializable]
	internal class rhsstateset : Dictionary <RhsState, RhsState>
	{
		public rhsstateset () : base (new cmpset ()) { }
	}

	[Serializable]
	public class SymbolReference
	{
		public SymbolReference(int lineno, int column, int length, bool reference)
		{
			this.lineno = lineno;
			this.column = column;
			this.length = length;
			this.reference = reference;
		}
		public int lineno { get; private set; }
		public int column { get; private set; }
		public int length { get; private set; }
		public bool reference { get; private set; }
	}

	[Serializable]
	public class reflist : List <SymbolReference>
	{
	}

	[Serializable]
	internal class textpos
	{
		public textpos ((int lineno, int column, int length, bool reference) position)
		{
			this.position = position;
		}
		public (int lineno, int column, int length, bool reference) position { get; private set; }
	}

	[Serializable]
	public class cmpref : IComparer<(int lineno, int column, int length, bool reference)>
	{
		public int Compare ((int lineno, int column, int length, bool reference) x, (int lineno, int column, int length, bool reference) y)
		{
			if (x.lineno != y.lineno)
				return x.lineno - y.lineno;
			if (x.column + x.length < y.column)
				return -1;
			if (y.column + y.length < x.column)
				return 1;
			return 0;
		}
	}

	[Serializable]
	public class reftab : SortedDictionary < (int lineno, int column, int length, bool reference), (SymbolToken, object) >
	{
		public reftab () : base (new cmpref ()) { }
	}
}
