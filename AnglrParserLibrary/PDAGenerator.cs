using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Anglr.Parser;
using Anglr.Compiler;
using AnglrParserLibrary;
using System.Runtime.InteropServices;

namespace AnglrLibrary
{
	public static class AnglrWin32Imports
	{
		[DllImport ("AnglrWin32Exports", CallingConvention = CallingConvention.Cdecl)]
		public static extern int GetParentProcessId (int processId);
		[DllImport ("AnglrWin32Exports", CallingConvention = CallingConvention.Cdecl)]
		public static extern int CreateRegexClass (string[] regexTextArray, int regexTextArraySize);
	}

	[Serializable]
	public partial class PDAGenerator : CSharpBaseGenerator
	{
		public PDAGenerator (_anglr_source_ p__anglr_source_, string outputDir, string className, anglrCompiler compiler) : base (p__anglr_source_, outputDir, className, compiler)
		{
			m_anglrFile = p__anglr_source_.m__anglr_file_;
			OutputDir = outputDir;
			m_className = className;

			try
			{
				if ((OutputDir != null) && (OutputDir != ""))
				{
					directoryInfo = Directory.CreateDirectory (OutputDir);
				}
			}
			catch (Exception)
			{

			}

			Common_Event += Invoke_Common_Callback;
			_anglr_file__Event += Invoke__anglr_file__Callback;
			_anglr_definition_list__Event += Invoke__anglr_definition_list__Callback;
			_anglr_file_part_list__Event += Invoke__anglr_file_part_list__Callback;
			_anglr_definition__Event += Invoke__anglr_definition__Callback;
			_token_string__Event += Invoke__token_string__Callback;
			_token_definition__Event += Invoke__token_definition__Callback;
			_anglr_syntax_rule_list__Event += Invoke__anglr_syntax_rule_list__Callback;
			_anglr_syntax_rule__Event += Invoke__anglr_syntax_rule__Callback;
			_anglr_syntax_production_list__Event += Invoke__anglr_syntax_production_list__Callback;
			_anglr_syntax_production__Event += Invoke__anglr_syntax_production__Callback;
			_name_list__Event += Invoke__name_list__Callback;
			_name__Event += Invoke__name__Callback;
			_g_name__Event += Invoke__g_name__Callback;
			_regular_expression_list__Event += Invoke__regular_expression_list__Callback;
			_attribute_list__Event += Invoke__attribute_list__Callback;
			_name_value_list__Event += Invoke__name_value_list__Callback;
			_general_part__Event += Invoke__general_part__Callback;
			_declaration_part__Event += Invoke__declaration_part__Callback;
			_scanner_part__Event += Invoke__scanner_part__Callback;
			_parser_part__Event += Invoke__parser_part__Callback;

			TraverseCommon (m_anglrFile);
		}

		public void Dispose () { }

		public void GeneratePDA ()
		{
			Console.Error.WriteLine ("Collect symbols");
			Traverse (m_anglrFile);
			Console.Error.WriteLine ("Display productions");
			DisplayProductions ();
			{
				SymbolToken[] symTab = new SymbolToken[1024];
				Console.Error.WriteLine ("Display hierarchical view");
				DisplayProductionsHierarchy (m_startProductionNode, symTab, 0, 0);
			}
			Console.Error.WriteLine ("Check productions");
			int result = CheckProductions ();
			if (result != 0)
				Console.Error.WriteLine ("production rules failure");
			else
			{
				Console.Error.WriteLine ("Make cannonical RHS sets");
				MakeCanonicalRhsSet ();
				Console.Error.WriteLine ("Compute epsilon conditions");
				ComputeEpsilonConditions ();
				Console.Error.WriteLine ("Compute start sets");
				ComputeStartSets ();
				Console.Error.WriteLine ("Compute transition sets");
				ComputeTransitionSets ();
				Console.Error.WriteLine ("Compute follow sets");
				ComputeFollowSets ();
				Console.Error.WriteLine ("Prepare table generator");
				PrepareTableGenerator ();
				GenerateParserSrc ();
				GenerateParserHdr ();
				GenerateTokenHeader ();
				GenerateLexer ();

				//int count = m_statearray[0]->stateWalker (m_statequeue, 100);
				//cout << "count = " << count << endl;
				//if (false)

				Console.Error.WriteLine ("Output state descriptions");
				Console.Error.WriteLine ("INFO: STATES");
				Console.Error.WriteLine ();
				foreach (RhsState p_state in m_statearray)
				{
					Console.WriteLine ("State " + p_state.stateNumber);
					if (p_state.coreSize > 0)
					{
						Console.WriteLine ();
						p_state.DisplayCore ();
					}
					if (p_state.closureSize > 0)
					{
						Console.WriteLine ();
						p_state.DisplayClosure ();
					}
					if (p_state.shiftSetSize > 0)
					{
						Console.WriteLine ();
						p_state.DisplayShiftTransitions ();
					}
					if (p_state.reductionsSetSize > 0)
					{
						Console.WriteLine ();
						//p_state->DisplayReductions ();
						p_state.checkConflicts ();
					}
					if (p_state.gotoSetSize > 0)
					{
						Console.WriteLine ();
						p_state.DisplayGotoTransitions ();
					}
					Console.WriteLine ();
				}
			}
		}

		private SymbolToken insertSymbol (string name, uint declarator, int index, bool reportDefined = true)
		{
			SymbolToken p_symbolToken = new SymbolToken (name, declarator);
			SymbolToken p_symbolReference = m_symtab.insert (p_symbolToken);
			if (p_symbolReference != p_symbolToken)
			{
				if (reportDefined)
					Console.WriteLine ("redefined symbol '" + p_symbolToken.name + "'");
			}
			p_symbolReference.index = index;
			return p_symbolReference;
		}

		private int CheckProductions ()
		{
			int result = 0;

			foreach (KeyValuePair<SymbolToken, RhsProductionNode> it in m_prodlist)
			{
				RhsProductionNode p_productionNode = it.Value;
				if (p_productionNode == null)
					continue;
				SymbolToken p_productionName = p_productionNode.productionName;
				if (p_productionName.declarator != (uint) AnglrClassificationType.NonTerminalName)
				{
					Console.WriteLine ("production name '" + p_productionName.name + "' must be non-terminal");
					++result;
				}
				int index = 1;
				foreach (RhsProduction p_production in p_productionNode.m_productions)
				{
					(m_proddict[p_production.productionNumber] = p_production).index = index++;
					foreach (RhsNode p_node in p_production.rhsNodes)
					{
						SymbolToken p_nodeSymbol = p_node.symbolToken;
						uint declarator = p_nodeSymbol.declarator;
						if (!((declarator == (uint) AnglrClassificationType.TerminalName) || (declarator == (uint) AnglrClassificationType.NonTerminalName)))
						{
							Console.WriteLine ("rhs symbol name '" + p_nodeSymbol.name + "' must be terminal (token) or non-terminal");
							++result;
							continue;
						}
						if (declarator == (uint) AnglrClassificationType.NonTerminalName)
						{
							if (!m_prodlist.Keys.Contains (p_nodeSymbol))
							{
								Console.WriteLine ("symbol '" + p_nodeSymbol.name + "' is used in production rules, but is not defined as token or non-terminal");
								++result;
							}
						}
					}
				}
			}
			return result;
		}

		private void DisplayProductions ()
		{
			if (!PDAGenerator.g_displayProductions)
				return;
			Console.WriteLine ("INFO: PRODUCTIONS");
			Console.WriteLine ();
			foreach (RhsProductionNode p_productionNode in m_prodlist.Values)
			{
				SymbolToken p_productionName = p_productionNode.productionName;
				Console.WriteLine (p_productionName.name);
				int i = -1;
				foreach (RhsProduction p_production in p_productionNode.m_productions)
				{
					++i;
					Console.Write ("\t");
					if (i != 0)
						Console.Write ("|\t");
					else
						Console.Write (":\t");
					foreach (RhsNode p_node in p_production.rhsNodes)
					{
						SymbolToken p_nodeSymbol = p_node.symbolToken;
						if (p_nodeSymbol.alias != null)
							p_nodeSymbol = p_nodeSymbol.alias;
						Console.Write (p_nodeSymbol.name + " ");
					}
					string code = p_production.code;
					if (code.Length != 0)
					{
						Console.WriteLine ();
						Console.Write ("\t" + code);
					}
					Console.WriteLine ();
				}
			}
		}

		private void DisplayProductionsHierarchy (RhsProductionNode prodNode, SymbolToken[] stack, int stackIndex, int displayDepth)
		{
			if (displayDepth == 0)
			{
				Console.WriteLine ("INFO: HIERARCHICALY VIEW OF PRODUCTIONS");
				Console.WriteLine ();
			}

			int stackTop = stackIndex;
			SymbolToken nodeSymbol = (prodNode != null) ? prodNode.productionName : null;
			if (nodeSymbol == null)
				return;
			if (nodeSymbol.displayed)
				return;
			nodeSymbol.displayed = true;
			stack[stackTop++] = nodeSymbol;
			for (int i = 0; i < displayDepth; ++i)
				Console.Write ("\t");
			Console.WriteLine (nodeSymbol.name);
			for (int i = 0; i < displayDepth; ++i)
				Console.Write ("\t");
			Console.WriteLine ("{");
			if (prodNode != null)
				foreach (RhsProduction p_RhsProduction in prodNode.m_productions)
				{
					foreach (RhsNode p_RhsNode in p_RhsProduction.rhsNodes)
					{
						SymbolToken p_SymbolToken = p_RhsNode.symbolToken;
						int index;
						for (index = stackIndex; (index < stackTop) && (p_SymbolToken.name != stack[index].name); ++index)
							;
						if (index < stackTop)
							continue;
						stack[stackTop++] = p_SymbolToken;
						bool isToken = false;
						if ((isToken = (p_SymbolToken.declarator == (uint) AnglrClassificationType.TerminalName)) || (p_SymbolToken.displayed == true))
						{
							for (int i = 0; i < displayDepth + 1; ++i)
								Console.Write ("\t");
							Console.WriteLine (p_SymbolToken.name + " " + (isToken ? "{T}" : "{N}"));
							continue;
						}
						DisplayProductionsHierarchy (m_prodlist[p_SymbolToken], stack, stackTop, displayDepth + 1);
					}
				}
			for (int i = 0; i < displayDepth; ++i)
				Console.Write ("\t");
			Console.WriteLine ("}");
		}

		private void MakeCanonicalRhsSet ()
		{
			int stateNumber = 0;
			int low = 0;
			int high = 0;

			if (!m_stateset.Keys.Contains (m_firstRhsState))
			{
				m_firstRhsState.stateNumber = stateNumber++;
				m_stateset[m_firstRhsState] = m_firstRhsState;
				m_statearray.Add (m_firstRhsState);
				++high;
			}
			else
			{
				Console.WriteLine ("internal error in MakeCanonicalRhsSet()");
				return;
			}
			while (low < high)
			{
				RhsState p_RhsState = m_statearray [low++];
				p_RhsState.makeClosure ();
				p_RhsState.makeStates ();
				int count = p_RhsState.m_shiftarray.Count;
				for (int index = 0; index < count; ++index)
				{
					RhsState p_state = p_RhsState.m_shiftarray[index];
					RhsState p_RhsStateRef = null;
					if (!m_stateset.TryGetValue (p_state, out p_RhsStateRef))
					{
						p_state.stateNumber = stateNumber++;
						m_stateset[p_state] = p_state;
						m_statearray.Add (p_state);
						++high;
					}
					else
					{
						p_RhsState.changeShiftState (p_RhsStateRef, index);
						p_state.Dispose ();
					}
				}
				count = p_RhsState.m_gotoarray.Count;
				for (int index = 0; index < count; ++index)
				{
					RhsState p_state = p_RhsState.m_gotoarray [index];
					RhsState p_RhsStateRef = null;
					if (!m_stateset.TryGetValue (p_state, out p_RhsStateRef))
					{
						p_state.stateNumber = stateNumber++;
						m_stateset[p_state] = p_state;
						m_statearray.Add (p_state);
						++high;
					}
					else
					{
						p_RhsState.changeGototState (p_RhsStateRef, index);
						p_state.Dispose ();
					}
				}
			}
			foreach (RhsProductionNode p_RhsProductionNode in m_prodlist.Values)
			{
				if (!p_RhsProductionNode.getUsed ())
					Console.WriteLine ("non-terminal symbol '" + p_RhsProductionNode.productionName.name + "' is not used");
			}
		}

		private void ComputeEpsilonConditions ()
		{
			while (true)
			{
				int count = 0;
				foreach (RhsProductionNode p_RhsProductionNode in m_prodlist.Values)
				{
					foreach (RhsProduction p_RhsProduction in p_RhsProductionNode.m_productions)
					{
						bool brokenLoop = false;
						foreach (RhsNode p_RhsNode in p_RhsProduction.rhsNodes)
						{
							RhsProductionNode p_RhsProductionNodeRef = null;
							SymbolToken p_SymbolToken = p_RhsNode.symbolToken;
							if (!m_prodlist.TryGetValue (p_SymbolToken, out p_RhsProductionNodeRef))
							{
								brokenLoop = true;
								break;
							}
							if (!p_RhsProductionNodeRef.epsilonCondition)
							{
								brokenLoop = true;
								break;
							}
						}
						if (!brokenLoop)
						{
							if (!p_RhsProductionNode.epsilonCondition)
							{
								p_RhsProductionNode.epsilonCondition = true;
								++count;
							}
							break;
						}
					}
				}
				if (count == 0)
					break;
			}
		}

		private void ComputeStartSets ()
		{
			while (true)
			{
				int count = 0;
				foreach (RhsProductionNode p_RhsProductionNode in m_prodlist.Values)
				{
					foreach (RhsProduction p_RhsProduction in p_RhsProductionNode.m_productions)
					{
						foreach (RhsNode p_RhsNode in p_RhsProduction.rhsNodes)
						{
							SymbolToken p_SymbolToken = p_RhsNode.symbolToken;
							if (p_SymbolToken.declarator == (uint) AnglrClassificationType.TerminalName)
							{
								if (p_RhsProductionNode.insertStartElement (p_SymbolToken))
									++count;
								break;
							}
							if (p_SymbolToken.declarator == (uint) AnglrClassificationType.NonTerminalName)
							{
								RhsProductionNode p_RhsProductionNodeRef = null;
								if (!m_prodlist.TryGetValue (p_SymbolToken, out p_RhsProductionNodeRef))
									break;
								if (p_RhsProductionNode.insertStartSet (p_RhsProductionNodeRef.startSet))
									++count;
								if (!p_RhsProductionNodeRef.epsilonCondition)
									break;
							}
						}
					}
				}
				if (count == 0)
					break;
			}
			if (!PDAGenerator.g_displayStartSets)
				return;
			Console.WriteLine ("Start sets:");
			foreach (KeyValuePair<SymbolToken, RhsProductionNode> keyval in m_prodlist)
			{
				Console.WriteLine (keyval.Key.name + ":");
				Console.Write ("{");
				keyval.Value.startSet.display ();
				Console.WriteLine ("}");
			}
		}

		private void ComputeTransitionSets ()
		{
			int count = 0;
			while (true)
			{
				count = 0;
				foreach (RhsProductionNode p_RhsProductionNode in m_prodlist.Values)
				{
					if (p_RhsProductionNode == null)
						continue;
					foreach (RhsProduction p_RhsProduction in p_RhsProductionNode.m_productions)
						count += ComputeTransitionSets (p_RhsProduction, new rhsenumerator (p_RhsProduction.rhsNodes));
				}
				if (count == 0)
					break;
			}
		}

		private int ComputeTransitionSets (RhsProduction p_RhsProduction, rhsenumerator rhsit)
		{
			int count = 0;

			if (rhsit.atEnd)
				return count;

			rhsenumerator rhsinc = new rhsenumerator (rhsit);
			count += ComputeTransitionSets (p_RhsProduction, rhsinc);

			RhsNode p_RhsNode = rhsit.currentRhsNode;
			if (rhsinc.atEnd)
			{
				p_RhsNode.opened = true;
				return count;
			}

			RhsNode p_RhsNodeInc = rhsinc.currentRhsNode;
			SymbolToken p_SymbolTokenInc = p_RhsNodeInc.symbolToken;
			if (p_SymbolTokenInc.declarator == (uint) AnglrClassificationType.TerminalName)
			{
				count += p_RhsNode.insertTransitionSet (p_SymbolTokenInc) ? 1 : 0;
				return count;
			}
			RhsProductionNode p_RhsProductionNodeInc = null;
			if (!m_prodlist.TryGetValue (p_SymbolTokenInc, out p_RhsProductionNodeInc))
				return count;
			if (p_RhsProductionNodeInc == null)
				return count;
			count += p_RhsNode.unionTransitionSet (p_RhsProductionNodeInc.startSet) ? 1 : 0;
			if (p_RhsProductionNodeInc.epsilonCondition)
			{
				count += p_RhsNode.unionTransitionSet (p_RhsNodeInc.getTransitionSet) ? 1 : 0;
				p_RhsNode.opened = p_RhsNodeInc.opened;
			}
			return count;
		}

		private void ComputeFollowSets ()
		{
			int count;
			do
			{
				count = 0;
				foreach (RhsState p_rhsState in m_stateset.Values)
					count += p_rhsState.computeFollowSets ();
			}
			while (count > 0);
		}

		private void PrepareTableGenerator ()
		{
			// mark most frequent goto transitions
			foreach (RhsState p_RhsState in m_stateset.Values)
				p_RhsState.markGotoCounters ();

			if (false)
			{
				int gotoCount = 0;
				foreach (KeyValuePair<SymbolToken, RhsProductionNode> keyval in m_prodlist)
					if (keyval.Value != null)
						keyval.Value.displayGotoCnt ();
				Console.WriteLine ("OPTIMIZED GOTO = " + gotoCount);
			}

			// create shift set, goto set collections, mark default reduction rules
			foreach (RhsState p_RhsState in m_stateset.Values)
			{
				p_RhsState.addToShiftSet (m_shiftset);
				p_RhsState.addToGotoSet (m_gotoset);
				p_RhsState.markDefaultReduction ();
				p_RhsState.checkGLRCondition ();
			}

			foreach (RhsProductionNode p_RhsProductionNode in m_prodlist.Values)
				if (p_RhsProductionNode != null)
					p_RhsProductionNode.defineBestGotoCounter ();

			//foreach (statedelta p_statedelta in m_shiftset.Values)
			//{
			//	stateset p_set = p_statedelta.first;
			//	stateSetInfo p_info = p_statedelta.second;
			//	int minToken = -1;
			//	int maxToken = -1;
			//	foreach (KeyValuePair<SymbolToken, RhsState> keyval in p_set)
			//	{
			//		SymbolToken p_SymbolToken = keyval.Key;
			//		int index = p_SymbolToken.index;
			//		if ((minToken < 0) || (minToken > index))
			//			minToken = index;
			//		if (maxToken < index)
			//			maxToken = index;
			//	}
			//	p_info.m_min = minToken;
			//	p_info.m_max = maxToken;
			//}

			//foreach (statedelta p_statedelta in m_gotoset.Values)
			//{
			//	stateset p_set = p_statedelta.first;
			//	stateSetInfo p_info = p_statedelta.second;
			//	int minToken = -1;
			//	int maxToken = -1;
			//	foreach (KeyValuePair < SymbolToken, RhsState > keyval in p_set)
			//	{
			//		SymbolToken p_SymbolToken = keyval.Key;
			//		RhsState p_RhsState = keyval.Value;
			//		RhsProductionNode p_RhsProductionNode = m_prodlist [p_SymbolToken];
			//		if (p_RhsProductionNode == null)
			//			continue;
			//		if (p_RhsState.stateNumber == p_RhsProductionNode.bestGoto)
			//			continue;
			//		int index = p_SymbolToken.index;
			//		if ((minToken < 0) || (minToken > index))
			//			minToken = index;
			//		if (maxToken < index)
			//			maxToken = index;
			//	}
			//	p_info.m_min = minToken;
			//	p_info.m_max = maxToken;
			//}
		}

		private void GenerateTables (TextWriter f)
		{
			GenerateTerminalTables (f);
			GenerateNonTerminalTables (f);
			GenerateGLRTables (f);
			GenerateTransitionTables (f);
			GenerateShiftDeltas (f);
			GenerateGotoDeltas (f);
			GenerateProductionTables (f);
			GenerateReductionTables (f);
			if (true)
				CheckPDA ();
			GenerateSemanticClasses ();
		}

		private void GenerateTerminalTables (TextWriter f)
		{
			Console.WriteLine ("INFO: TERMINALS");
			Console.WriteLine ();

			foreach (SymbolToken symbol in m_terminals)
			{
				Console.WriteLine ("#define\t" + symbol.correctName + "\t" + symbol.index);
			}
			Console.WriteLine ();

			Console.WriteLine ("#define\tminTerminalCode\t" + m_minTerminalNr);
			Console.WriteLine ("#define\tmaxTerminalCode\t" + m_maxTerminalNr);
			Console.WriteLine ();

			m_terminalCodes = new int[m_terminals.Count];

			int i = 0;
			f.WriteLine ("int " + m_className + "::g_terminalCodes[] = ");
			f.WriteLine ("{");
			foreach (SymbolToken symbol in m_terminals)
			{
				if ((i % 10) == 0)
					f.Write ("\t");
				f.Write ((m_terminalCodes[i] = symbol.index) + ", ");
				if ((++i % 10) == 0)
					f.WriteLine ();
			}
			if ((i % 10) != 0)
				f.WriteLine ();
			f.WriteLine ("};");
			f.WriteLine ();
		}

		private void GenerateNonTerminalTables (TextWriter f)
		{
			Console.WriteLine ("INFO: NONTERMINALS");
			Console.WriteLine ();

			foreach (SymbolToken symbol in m_nonterminals)
			{
				Console.WriteLine ("#define\t" + symbol.correctName + "\t" + symbol.index);
			}
			Console.WriteLine ();

			Console.WriteLine ("#define\tminNonTerminalCode\t" + m_minNonTerminalNr);
			Console.WriteLine ("#define\tmaxNonTerminalCode\t" + m_maxNonTerminalNr);
			Console.WriteLine ();

			m_nonTerminalCodes = new int[m_nonterminals.Count];

			int i = 0;
			f.WriteLine ("int " + m_className + "::g_nonTerminalCodes[] = ");
			f.WriteLine ("{");
			foreach (SymbolToken symbol in m_nonterminals)
			{
				if ((i % 10) == 0)
					f.Write ("\t");
				f.Write ((m_nonTerminalCodes[i] = symbol.index) + ", ");
				if ((++i % 10) == 0)
					f.WriteLine ();
			}
			if ((i % 10) != 0)
				f.WriteLine ();
			f.WriteLine ("};");
			f.WriteLine ();
		}

		private void GenerateTransitionTables (TextWriter f)
		{
			HashSet<int> deltaset = new HashSet<int> ();
			int i;
			int size = 1000;
			m_check = new int[size];
			m_state = new int[size];
			m_maxCheck = -1;

			foreach (statedelta shiftit in m_shiftset.Values)
			{
				stateset p_shiftset = shiftit.first;
				int delta = 1;
				while (true)
				{
					while (deltaset.Contains (delta))
						++delta;

					bool brokenLoop = false;
					foreach (SymbolToken p_SymbolToken in p_shiftset.Keys)
					{
						int index = delta + p_SymbolToken.index;
						if (index > m_maxCheck)
							m_maxCheck = index;
						if (index >= size)
							size = ResizeTransitionTables (size, 1000);
						if (m_check[index] != 0)
						{
							brokenLoop = true;
							break;
						}
					}
					if (!brokenLoop)
						break;
					++delta;
				}
				deltaset.Add (shiftit.second.m_delta = delta);
				foreach (KeyValuePair<SymbolToken, RhsState> stateit in p_shiftset)
				{
					SymbolToken p_SymbolToken = stateit.Key;
					int index = delta + p_SymbolToken.index;
					if (m_check[index] != 0)
						Console.WriteLine ("overwrite fiefd");
					m_check[index] = index - delta;
					m_state[index] = stateit.Value.stateNumber;
				}
			}

			foreach (statedelta gotoit in m_gotoset.Values)
			{
				stateset p_gotoset = gotoit.first;
				int delta = 1;
				while (true)
				{
					while (deltaset.Contains (delta))
						++delta;

					bool brokenLoop = false;
					foreach (KeyValuePair<SymbolToken, RhsState> stateit in p_gotoset)
					{
						SymbolToken p_SymbolToken = stateit.Key;
						RhsState p_RhsState = stateit.Value;
						RhsProductionNode p_RhsProductionNode = m_prodlist [p_SymbolToken];
						if (p_RhsProductionNode == null)
							continue;
						if (p_RhsState.stateNumber == p_RhsProductionNode.bestGoto)
							continue;
						int index = delta + p_SymbolToken.index;
						if (index > m_maxCheck)
							m_maxCheck = index;
						if (index >= size)
							size = ResizeTransitionTables (size, 1000);
						if (m_check[index] != 0)
						{
							brokenLoop = true;
							break;
						}
					}
					if (!brokenLoop)
						break;
					++delta;
				}
				deltaset.Add (gotoit.second.m_delta = delta);
				foreach (KeyValuePair<SymbolToken, RhsState> stateit in p_gotoset)
				{
					SymbolToken p_SymbolToken = stateit.Key;
					RhsState p_RhsState = stateit.Value;
					RhsProductionNode p_RhsProductionNode = m_prodlist [p_SymbolToken];
					if (p_RhsProductionNode == null)
						continue;
					if (p_RhsState.stateNumber == p_RhsProductionNode.bestGoto)
						continue;
					int index = delta + p_SymbolToken.index;
					if (m_check[index] != 0)
						Console.WriteLine ("overwrite fiefd");
					m_check[index] = index - delta;
					m_state[index] = stateit.Value.stateNumber /*- m_minNonTerminalNr*/;
				}
			}

			++m_maxCheck;

			f.WriteLine ("int " + m_className + "::g_check[] =");
			f.WriteLine ("{");
			for (i = 0; i < m_maxCheck;)
			{
				if ((i % 10) == 0)
					f.Write ("\t");
				f.Write (m_check[i] + ", ");
				if ((++i % 10) == 0)
					f.WriteLine ();
			}
			if ((i % 10) != 0)
				f.WriteLine ();
			f.WriteLine ("};");
			f.WriteLine ();

			f.WriteLine ("int " + m_className + "::g_state[] =");
			f.WriteLine ("{");
			for (i = 0; i < m_maxCheck;)
			{
				if ((i % 10) == 0)
					f.Write ("\t");
				f.Write (m_state[i] + ", ");
				if ((++i % 10) == 0)
					f.WriteLine ();
			}
			if ((i % 10) != 0)
				f.WriteLine ();
			f.WriteLine ("};");
			f.WriteLine ();
		}

		private void GenerateShiftDeltas (TextWriter f)
		{
			int size = m_stateset.Count;
			m_shiftDelta = new int[size];
			foreach (RhsState p_RhsState in m_stateset.Values)
			{
				int val = 0;
				statedelta sd = p_RhsState.findShiftSet (m_shiftset);
				if (sd.first.Count > 0)
					val = sd.second.m_delta;
				m_shiftDelta[p_RhsState.stateNumber] = p_RhsState.hasGLRCondition ? -p_RhsState.GLRDelta : val;
			}

			int i;
			f.WriteLine ("int " + m_className + "::g_shiftDelta[] =");
			f.WriteLine ("{");
			for (i = 0; i < size;)
			{
				if ((i % 10) == 0)
					f.Write ("\t");
				f.Write (m_shiftDelta[i] + ", ");
				if ((++i % 10) == 0)
					f.WriteLine ();
			}
			if ((i % 10) != 0)
				f.WriteLine ();
			f.WriteLine ("};");
			f.WriteLine ();
		}

		private void GenerateGotoDeltas (TextWriter f)
		{
			int size = m_stateset.Count;
			m_gotoDelta = new int[size];
			foreach (RhsState p_RhsState in m_stateset.Values)
			{
				int val = 0;
				statedelta sd = p_RhsState.findGotoSet (m_gotoset);
				if (sd.first.Count > 0)
					val = sd.second.m_delta;
				m_gotoDelta[p_RhsState.stateNumber] = val;
			}

			int i;
			f.WriteLine ("int " + m_className + "::g_gotoDelta[] =");
			f.WriteLine ("{");
			for (i = 0; i < size;)
			{
				if ((i % 10) == 0)
					f.Write ("\t");
				f.Write (m_gotoDelta[i] + ", ");
				if ((++i % 10) == 0)
					f.WriteLine ();
			}
			if ((i % 10) != 0)
				f.WriteLine ();
			f.WriteLine ("};");
			f.WriteLine ();
		}

		private void GenerateProductionTables (TextWriter f)
		{
			int i;

			int size = RhsProduction.productionCounter + 1;
			m_productionLengths = new int[size];
			m_productionLengths[0] = 0;

			foreach (RhsProductionNode p_RhsProductionNode in m_prodlist.Values)
			{
				if (p_RhsProductionNode == null)
					continue;
				foreach (RhsProduction p_RhsProduction in p_RhsProductionNode.m_productions)
				{
					m_productionLengths[p_RhsProduction.productionNumber] = p_RhsProduction.length;
				}
			}

			f.WriteLine ("int " + m_className + "::g_productionLengths[] = ");
			f.WriteLine ("{");
			for (i = 0; i < size;)
			{
				if ((i % 10) == 0)
					f.Write ("\t");
				f.Write (m_productionLengths[i] + ", ");
				if ((++i % 10) == 0)
					f.WriteLine ();
			}
			if ((i % 10) != 0)
				f.WriteLine ();
			f.WriteLine ("};");
			f.WriteLine ();

			m_productionRules = new int[size];
			m_productionRules[0] = 0;

			foreach (KeyValuePair<SymbolToken, RhsProductionNode> prodit in m_prodlist)
			{
				RhsProductionNode p_RhsProductionNode = prodit.Value;
				if (p_RhsProductionNode == null)
					continue;
				SymbolToken p_SymbolToken = prodit.Key;
				int index = p_SymbolToken.index;
				foreach (RhsProduction p_RhsProduction in p_RhsProductionNode.m_productions)
				{
					m_productionRules[p_RhsProduction.productionNumber] = index /*- m_minNonTerminalNr*/;
				}
			}

			f.WriteLine ("int " + m_className + "::g_productionRules[] = ");
			f.WriteLine ("{");
			for (i = 0; i < size;)
			{
				if ((i % 10) == 0)
					f.Write ("\t");
				f.Write (m_productionRules[i] + ", ");
				if ((++i % 10) == 0)
					f.WriteLine ();
			}
			if ((i % 10) != 0)
				f.WriteLine ();
			f.WriteLine ("};");
			f.WriteLine ();

			m_defaultGoto = new int[m_maxNonTerminalNr + 1];

			foreach (KeyValuePair<SymbolToken, RhsProductionNode> prodit in m_prodlist)
			{
				RhsProductionNode p_RhsProductionNode = prodit.Value;
				if (p_RhsProductionNode == null)
					continue;
				SymbolToken p_SymbolToken = prodit.Key;
				if (p_SymbolToken.declarator == (uint) AnglrClassificationType.TerminalName)
					continue;
				if (p_SymbolToken.index >= size)
					Console.WriteLine ("default goto " + p_SymbolToken.index + " > " + size);
				m_defaultGoto[p_SymbolToken.index /*- m_minNonTerminalNr*/] = p_RhsProductionNode.bestGoto;
			}

			f.WriteLine ("int " + m_className + "::g_defaultGoto[] = ");
			f.WriteLine ("{");
			for (i = 0; i < m_maxNonTerminalNr /*- m_minNonTerminalNr*/;)
			{
				if ((i % 10) == 0)
					f.Write ("\t");
				f.Write (m_defaultGoto[i] + ", ");
				if ((++i % 10) == 0)
					f.WriteLine ();
			}
			if ((i % 10) != 0)
				f.WriteLine ();
			f.WriteLine ("};");
			f.WriteLine ();
		}

		private void GenerateReductionTables (TextWriter f)
		{
			HashSet <int> deltaset = new HashSet<int> ();
			int i;
			int size = 1000;
			m_rcheck = new int[size];
			m_rstate = new int[size];
			m_maxRCheck = -1;

			foreach (RhsState p_RhsState in m_stateset.Values)
			{
				if (p_RhsState.reductionsSetSize < 2)
					continue;
				prodset p_reductions = p_RhsState.reductions;

				int delta = 1;
				while (true)
				{
					while (deltaset.Contains (delta))
						++delta;

					bool brokenLoop = false;
					foreach (RhsConfiguration p_RhsConfiguration in p_reductions.Values)
					{
						brokenLoop = false;
						TokenSet p_TokenSet = p_RhsConfiguration.getFollowSet;
						tokset p_tokset = p_TokenSet.set;
						foreach (SymbolToken symbol in p_tokset.Values)
						{
							int index = delta + symbol.index;
							if (index > m_maxRCheck)
								m_maxRCheck = index;
							if (index >= size)
								size = ResizeReductionTables (size, 1000);
							if (m_rcheck[index] != 0)
							{
								brokenLoop = true;
								break;
							}
						}
						if (brokenLoop)
							break;
					}
					if (!brokenLoop)
						break;
					++delta;
				}

				p_RhsState.reductionsDelta = delta;
				deltaset.Add (delta);

				foreach (KeyValuePair<int, RhsConfiguration> prodit in p_reductions)
				{
					RhsConfiguration p_RhsConfiguration = prodit.Value;
					TokenSet p_TokenSet = p_RhsConfiguration.getFollowSet;
					tokset p_tokset = p_TokenSet.set;
					foreach (SymbolToken p_SymbolToken in p_tokset.Values)
					{
						int index = p_SymbolToken.index;
						// reduce/reduce conflict
						if (m_rcheck[delta + index] != 0)
							; //Console.WriteLine ("reduce/reduce overwrite");
						m_rcheck[delta + index] = index;
						m_rstate[delta + index] = prodit.Key;
					}
				}
			}

			++m_maxRCheck;

			f.WriteLine ("int " + m_className + "::g_rcheck[] =");
			f.WriteLine ("{");
			if (m_maxRCheck == 0)
				f.WriteLine ("\t0");
			for (i = 0; i < m_maxRCheck;)
			{
				if ((i % 10) == 0)
					f.Write ("\t");
				f.Write (m_rcheck[i] + ", ");
				if ((++i % 10) == 0)
					f.WriteLine ();
			}
			if ((i % 10) != 0)
				f.WriteLine ();
			f.WriteLine ("};");
			f.WriteLine ();

			f.WriteLine ("int " + m_className + "::g_rstate[] =");
			f.WriteLine ("{");
			if (m_maxRCheck == 0)
				f.WriteLine ("\t0");
			for (i = 0; i < m_maxRCheck;)
			{
				if ((i % 10) == 0)
					f.Write ("\t");
				f.Write (m_rstate[i] + ", ");
				if ((++i % 10) == 0)
					f.WriteLine ();
			}
			if ((i % 10) != 0)
				f.WriteLine ();
			f.WriteLine ("};");
			f.WriteLine ();

			size = m_stateset.Count;
			m_reductions = new int[size];

			foreach (RhsState p_RhsState in m_stateset.Values)
				m_reductions[p_RhsState.stateNumber] = (p_RhsState.reductionsSetSize > 1) ? -p_RhsState.reductionsDelta : p_RhsState.defaultReduction;

			f.WriteLine ("int " + m_className + "::g_reductions[] =");
			f.WriteLine ("{");
			for (i = 0; i < size;)
			{
				if ((i % 10) == 0)
					f.Write ("\t");
				f.Write (m_reductions[i] + ", ");
				if ((++i % 10) == 0)
					f.WriteLine ();
			}
			if ((i % 10) != 0)
				f.WriteLine ();
			f.WriteLine ("};");
			f.WriteLine ();
		}

		private void GenerateGLRTables (TextWriter f)
		{
			int i;
			HashSet<int> deltaset = new HashSet<int> ();
			int size = 1000;
			m_glrcheck = new int[size];
			m_glrstate = new int[size];
			m_maxGLRCheck = -1;
			int glrsize = 0;
			glrtokset p_glrtokset = new glrtokset ();

			foreach (RhsState p_RhsState in m_stateset.Values)
			{
				if (!p_RhsState.hasGLRCondition)
					continue;
				glrset p_glrset = p_RhsState.getGLRSet;
				foreach (GLRToken p_GLRToken in p_glrset.Values)
				{
					GLRToken p_GLRTokenRef = null;
					if (p_glrtokset.TryGetValue (p_GLRToken, out p_GLRTokenRef))
					{
						p_GLRToken.position = p_GLRTokenRef.position;
						continue;
					}
					p_GLRToken.position = glrsize;
					p_glrtokset[p_GLRToken] = p_GLRToken;
					glrsize += 2 + p_GLRToken.getRdlist.Count;
				}
			}

			m_glrcells = new int[glrsize];

			foreach (GLRToken p_GLRToken in p_glrtokset.Values)
			{
				int position = p_GLRToken.position;
				rdlist p_rdlist = p_GLRToken.getRdlist;
				m_glrcells[position++] = 2 + p_rdlist.Count;
				m_glrcells[position++] = p_GLRToken.state;
				foreach (RhsProduction p_RhsProduction in p_rdlist)
					m_glrcells[position++] = p_RhsProduction.productionNumber;
			}

			foreach (RhsState p_RhsState in m_stateset.Values)
			{
				if (!p_RhsState.hasGLRCondition)
					continue;
				glrset p_glrset = p_RhsState.getGLRSet;
				int delta = 1;
				while (true)
				{
					while (deltaset.Contains (delta))
						++delta;

					bool brokenLoop = false;
					foreach (SymbolToken p_SymbolToken in p_glrset.Keys)
					{
						int index = delta + p_SymbolToken.index;
						if (index > m_maxGLRCheck)
							m_maxGLRCheck = index;
						if (index >= size)
							size = ResizeGLRTables (size, 1000);
						if (m_glrcheck[index] != 0)
						{
							brokenLoop = true;
							break;
						}
					}
					if (!brokenLoop)
						break;
					++delta;
				}
				p_RhsState.GLRDelta = delta;
				deltaset.Add (delta);
				foreach (KeyValuePair<SymbolToken, GLRToken> glrit in p_glrset)
				{
					SymbolToken p_SymbolToken = glrit.Key;
					int index = delta + p_SymbolToken.index;
					if (m_glrcheck[index] != 0)
						Console.WriteLine ("overwrite glr-check field, index = " + index);
					m_glrcheck[index] = index - delta;
					m_glrstate[index] = glrit.Value.position;
				}
			}

			++m_maxGLRCheck;

			f.WriteLine ("int " + m_className + "::g_glrcheck[] =");
			f.WriteLine ("{");
			for (i = 0; i < m_maxGLRCheck;)
			{
				if ((i % 10) == 0)
					f.Write ("\t");
				f.Write (m_glrcheck[i] + ", ");
				if ((++i % 10) == 0)
					f.WriteLine ();
			}
			if ((i % 10) != 0)
				f.WriteLine ();
			f.WriteLine ("};");
			f.WriteLine ();

			f.WriteLine ("int " + m_className + "::g_glrstate[] =");
			f.WriteLine ("{");
			for (i = 0; i < m_maxGLRCheck;)
			{
				if ((i % 10) == 0)
					f.Write ("\t");
				f.Write (m_glrstate[i] + ", ");
				if ((++i % 10) == 0)
					f.WriteLine ();
			}
			if ((i % 10) != 0)
				f.WriteLine ();
			f.WriteLine ("};");
			f.WriteLine ();

			f.WriteLine ("int " + m_className + "::g_glrcells[] =");
			f.WriteLine ("{");
			for (i = 0; i < glrsize;)
			{
				if ((i % 10) == 0)
					f.Write ("\t");
				f.Write (m_glrcells[i] + ", ");
				if ((++i % 10) == 0)
					f.WriteLine ();
			}
			if ((i % 10) != 0)
				f.WriteLine ();
			f.WriteLine ("};");
			f.WriteLine ();
		}

		private void GenerateParserSrc ()
		{
			string fileName = m_className + ".cpp";

			Console.Error.WriteLine ("Generate parser source file '" + fileName + "'");

			StreamWriter f = File.CreateText (fileName);
			//if (!f.is_open ())
			//{
			//	Console.WriteLine ("cannot open " + fileName);
			//	return;
			//}

			foreach (string line in g_ParserTemplateCppSrc)
			{
				if (line == g_CppTables)
					GenerateTables (f);
				else if (line == g_CppActions)
				{
					foreach (KeyValuePair<int, RhsProduction> it in m_proddict)
					{
						f.WriteLine ("\tcase " + it.Key + ":");
						RhsProduction p_RhsProduction = it.Value;
						int prodLen = p_RhsProduction.length;
						if (p_RhsProduction.symbolToken.name[0] == '$')
						{
							f.WriteLine ("\t\tbreak;");
							continue;
						}
						f.Write ("\t\tcurrentValue = m_" + m_className + "." + p_RhsProduction.symbolToken.correctName + "_" + p_RhsProduction.index + " (");
						string sep = "";
						int cnt = -1;
						foreach (RhsNode p_RhsNode in p_RhsProduction.rhsNodes)
						{
							++cnt;
							SymbolToken p_SymbolToken = p_RhsNode.symbolToken;
							f.Write (sep);
							if (p_SymbolToken.declarator == (uint) AnglrClassificationType.TerminalName)
								f.Write ("(SyntaxTreeToken*) m_valueStackTop[" + (cnt - prodLen) + "]");
							else
								f.Write ("(" + p_SymbolToken.correctName + "*) m_valueStackTop[" + (cnt - prodLen) + "]");
							sep = ", ";
						}
						f.WriteLine (");");
						f.WriteLine ("\t\tbreak;");
					}
				}
				else
				{
					f.WriteLine (line.Replace (g_CppClassname, m_className).Replace (g_ScannerClassRef, lexerClassName));
				}
			}

			f.WriteLine ();
			f.WriteLine ("void " + m_className + "::ReportClassCounters ()");
			f.WriteLine ("{");
			foreach (SymbolToken p_SymbolToken in m_prodlist.Keys)
			{
				if (p_SymbolToken.name[0] == '$')
					continue;
				f.WriteLine ("\tif (" + p_SymbolToken.correctName + "::g_counter != 0)");
				f.WriteLine ("\t\tcout << \"" + p_SymbolToken.correctName + "::g_counter = \" << " + p_SymbolToken.correctName + "::g_counter;");
			}
			f.WriteLine ("\tif (SyntaxTreeToken::g_counter != 0)");
			f.WriteLine ("\t\tcout << \"SyntaxTreeToken::g_counter = \" << SyntaxTreeToken::g_counter;");
			f.WriteLine ("}");
			f.Close ();
		}

		private void GenerateParserHdr ()
		{
			string fileName = m_className + ".h";
			Console.Error.WriteLine ("Generate parser header file '" + fileName + "'");
			StreamWriter f = File.CreateText (fileName);

			foreach (string line in g_ParserTemplateCppHdr)
			{
				f.WriteLine (line.Replace (g_CppClassname, m_className).Replace (g_ScannerClassRef, lexerClassName));
			}

			f.Close ();
		}

		private void CheckPDA ()
		{
			foreach (RhsState p_RhsState in m_stateset.Values)
			{
				int stateNumber = p_RhsState.stateNumber;

				int delta = m_shiftDelta [stateNumber];
				if (delta > 0)
					foreach (SymbolToken p_SymbolToken in m_terminals)
					{
						RhsState p_RhsStateRef = p_RhsState.checkShift (p_SymbolToken);
						int index = p_SymbolToken.index;
						if ((index + delta < 0) || (index + delta >= m_maxCheck) || (m_check[index + delta] != index))
						{
							if (p_RhsStateRef != null)
								Console.WriteLine ("state " + stateNumber + ", shift " + p_SymbolToken.name + " not handled");
						}
						else
						{
							if (p_RhsStateRef != null)
							{
								if (m_state[index + delta] != p_RhsStateRef.stateNumber)
									Console.WriteLine ("state " + stateNumber + ", shift " + p_SymbolToken.name + " to " + m_state[index + delta]);
							}
							else
								Console.WriteLine ("state " + stateNumber + ", shift " + p_SymbolToken.name + " handled");
						}
					}
				else
				{
					if (m_reductions[stateNumber] == 0)
						Console.WriteLine ("state " + stateNumber + ", no action");
				}

				delta = m_gotoDelta[stateNumber];
				if (delta > 0)
					foreach (SymbolToken p_SymbolToken in m_nonterminals)
					{
						RhsState p_RhsStateRef = p_RhsState.checkGoto (p_SymbolToken);
						int index = p_SymbolToken.index;
						if ((index + delta < 0) || (index + delta >= m_maxCheck) || (m_check[index + delta] != index))
						{
							if (p_RhsStateRef != null)
							{
								RhsProductionNode p_RhsProductionNode = null;
								if ((!m_prodlist.TryGetValue (p_SymbolToken, out p_RhsProductionNode)) || (p_RhsProductionNode == null) || (m_defaultGoto[p_SymbolToken.index /*- m_minNonTerminalNr*/] != p_RhsProductionNode.bestGoto))
									Console.WriteLine ("state " + stateNumber + ", goto " + p_SymbolToken.name + " not handled");
							}
						}
						else
						{
							if (p_RhsStateRef != null)
							{
								if (m_state[index + delta] != p_RhsStateRef.stateNumber)
								{
									RhsProductionNode p_RhsProductionNode = null;
									if ((!m_prodlist.TryGetValue (p_SymbolToken, out p_RhsProductionNode)) || (p_RhsProductionNode == null) || (m_defaultGoto[p_SymbolToken.index /*- m_minNonTerminalNr*/] != p_RhsProductionNode.bestGoto))
										Console.WriteLine ("state " + stateNumber + ", goto " + p_SymbolToken.name + " to " + m_state[index + delta]);
								}
							}
						}
					}
				else
				{
				}

				if ((p_RhsState.reductionsSetSize > 0) && (m_reductions[stateNumber] == 0))
					Console.WriteLine ("no reduction registered in state " + stateNumber);
			}
		}

		private void PrepareSyntaxTree ()
		{
			foreach (RhsProductionNode p_RhsProductionNode in m_prodlist.Values)
			{
				if (p_RhsProductionNode != null)
					p_RhsProductionNode.PrepareASTData ();
			}
		}

		private void GenerateParseHeaders ()
		{
			StreamWriter f = File.CreateText ("parse-syntax.h");

			f.WriteLine ("#pragma once");
			f.WriteLine ("");
			foreach (SymbolToken p_SymbolToken in m_prodlist.Keys)
			{
				if (p_SymbolToken.name[0] == '$')
					continue;
				f.WriteLine ("class\t" + p_SymbolToken.correctName + ";");
			}
			f.WriteLine ("");
			foreach (SymbolToken p_SymbolToken in m_prodlist.Keys)
			{
				if (p_SymbolToken.name[0] == '$')
					continue;
				f.WriteLine ("#include \"" + p_SymbolToken.correctName + ".h\"");
			}
			f.WriteLine ();
			f.WriteLine ("enum	SyntaxTreeCallbackReason");
			f.WriteLine ("{");
			f.WriteLine ("\tBuilderCallbackReason = 1,");
			f.WriteLine ("\tTraversalPrologueCallbackReason = 2,");
			f.WriteLine ("\tTraversalMidTermCallbackReason = 3,");
			f.WriteLine ("\tTraversalEpilogueCallbackReason = 4");
			f.WriteLine ("};");
			f.Close ();
		}

		private void GenerateTokenHeader ()
		{
			string fileName = m_className + "Tokens.h";
			Console.Error.WriteLine ("Generate tokens header file " + fileName + "'");
			StreamWriter f = File.CreateText (fileName);

			f.WriteLine ("#pragma once");
			f.WriteLine ();

			foreach (SymbolToken p_SymbolToken in m_terminals)
			{
				f.WriteLine ("#define\t" + p_SymbolToken.correctName + "\t" + p_SymbolToken.index);
			}

			f.WriteLine ();
			f.Close ();
		}

		private void GenerateSyntaxTreeBaseHdr ()
		{
			StreamWriter f = File.CreateText ("SyntaxTreeBase.h");

			foreach (string line in g_syntaxTreeBaseCppHdr)
			{
				f.WriteLine (line);
			}

			f.Close ();
		}

		private void GenerateSyntaxTreeBaseSrc ()
		{
			StreamWriter f = File.CreateText ("SyntaxTreeBase.cpp");

			foreach (string line in g_syntaxTreeBaseCppSrc)
			{
				f.WriteLine (line);
			}

			f.Close ();
		}

		private void GenerateLexerLibrary ()
		{
			//string fileName = OutputDir + regexClassName + ".h";
			//try
			//{
			//	Console.Error.WriteLine ($"Generate regex header file '{fileName}'");
			//	StreamWriter f = File.CreateText (fileName);
			//	f.WriteLine ($"# include <iostream>");
			//	f.WriteLine ($"# include <regex>");
			//	f.WriteLine ($"using namespace std;");

			//	f.WriteLine ($"");
			//	f.WriteLine ($"class anglrRegexScanner");
			//	f.WriteLine ($"{{");
			//	f.WriteLine ($"private:");
			//	f.WriteLine ($"\tregex* regarray;");
			//	f.WriteLine ($"\tint regsize;");
			//	f.WriteLine ($"public:");
			//	f.WriteLine ($"\tanglrRegexScanner (regex* regarray, int regsize);");
			//	f.WriteLine ($"\t~anglrRegexScanner ();");
			//	f.WriteLine ($"\tint scan (const char* line);");
			//	f.WriteLine ($"\tinline int token () {{ return _token; }}");
			//	f.WriteLine ($"\tinline const char* matchstr () {{ return _matchstr; }}");
			//	f.WriteLine ($"private:");
			//	f.WriteLine ($"\tint _token;");
			//	f.WriteLine ($"\tconst char* _matchstr;");
			//	f.WriteLine ($"}};");

			//	foreach (KeyValuePair<string, ScannerPart> keyValPair in scannerParts)
			//	{
			//		string scannerName = keyValPair.Value.regexClassName;
			//		try
			//		{
			//			f.WriteLine ($"");
			//			f.WriteLine ($"class {scannerName} : public anglrRegexScanner");
			//			f.WriteLine ($"{{");
			//			f.WriteLine ($"private:");
			//			f.WriteLine ($"\tstatic regex regarray [];");
			//			f.WriteLine ($"\tstatic int regsize;");
			//			f.WriteLine ($"public:");
			//			f.WriteLine ($"\tinline {scannerName} () : anglrRegexScanner (regarray, regsize) {{}}");
			//			f.WriteLine ($"}};");
			//		}
			//		catch (Exception e)
			//		{
			//			Console.Error.WriteLine ($"cannot get scanner description for {scannerName}, exception: {e.Message}");
			//		}
			//	}
			//	f.Close ();
			//}
			//catch (Exception e)
			//{
			//	Console.Error.WriteLine ($"Cannot create regex header file {fileName}, exception thrown: {e.Message}");
			//}

			//fileName = OutputDir + regexClassName + ".cpp";
			//try
			//{
			//	Console.Error.WriteLine ("Generate regex source file '" + fileName + "'");
			//	StreamWriter f = File.CreateText (fileName);
			//	f.WriteLine ($"#include \"{regexClassName}.h\"");

			//	f.WriteLine ();
			//	f.WriteLine ($"anglrRegexScanner::anglrRegexScanner (regex* regarray, int regsize)");
			//	f.WriteLine ($"{{");
			//	f.WriteLine ($"\tthis->regarray = regarray;");
			//	f.WriteLine ($"\tthis->regsize = regsize;");
			//	f.WriteLine ($"\t_matchstr = nullptr;");
			//	f.WriteLine ($"\t_token = 0;");
			//	f.WriteLine ($"}}");

			//	f.WriteLine ();
			//	f.WriteLine ($"anglrRegexScanner::~anglrRegexScanner ()");
			//	f.WriteLine ($"{{");
			//	f.WriteLine ($"\tif (_matchstr != nullptr)");
			//	f.WriteLine ($"\t\tfree ((void*) _matchstr);");
			//	f.WriteLine ($"\t_matchstr = nullptr;");
			//	f.WriteLine ($"}}");

			//	f.WriteLine ();
			//	f.WriteLine ($"int anglrRegexScanner::scan (const char* line)");
			//	f.WriteLine ($"{{");
			//	f.WriteLine ($"\tfor (int i = 0; i < regsize; ++i)");
			//	f.WriteLine ($"\t{{");
			//	f.WriteLine ($"\t\tcmatch cm;");
			//	f.WriteLine ($"\t\tif (!regex_search (line, cm, regarray [i]))");
			//	f.WriteLine ($"\t\t\tcontinue;");
			//	f.WriteLine ($"\t\tcsub_match sm = *cm.begin ();");
			//	f.WriteLine ($"\t\tif (_matchstr != nullptr)");
			//	f.WriteLine ($"\t\t\tfree ((void*) _matchstr);");
			//	f.WriteLine ($"\t\t_matchstr = _strdup (sm.str ().c_str ());");
			//	f.WriteLine ($"\t\treturn _token = i + 1;");
			//	f.WriteLine ($"\t}}");
			//	f.WriteLine ();
			//	f.WriteLine ($"\treturn 0;");
			//	f.WriteLine ($"}}");

			//	foreach (KeyValuePair<string, ScannerPart> keyValPair in scannerParts)
			//	{
			//		ScannerPart scannerValue = keyValPair.Value;
			//		string scannerName = scannerValue.regexClassName;
			//		try
			//		{

			//			f.WriteLine ();
			//			f.WriteLine ($"regex {scannerName}::regarray [] =");
			//			f.WriteLine ($"{{");
			//			foreach (string s in scannerValue.lexRegSet)
			//			{
			//				f.WriteLine ($"\tregex (\"^({s.Replace ("\\", "\\\\").Replace ("\"", "\\\"")})\"),");
			//			}
			//			f.WriteLine ($"}};");
			//			f.WriteLine ();
			//			f.WriteLine ($"int {scannerName}::regsize = sizeof {scannerName}::regarray / sizeof {scannerName}::regarray [0];");
			//			f.WriteLine ();
			//		}
			//		catch (Exception e)
			//		{
			//			Console.Error.WriteLine ($"cannot get scanner description for {scannerName}, exception: {e.Message}");
			//		}
			//	}
			//	f.Close ();
			//}
			//catch (Exception e)
			//{
			//	Console.Error.WriteLine ($"Cannot create regex header file {fileName}, exception thrown: {e.Message}");
			//}
		}

		private void GenerateLexerHdr ()
		{
			string fileName = OutputDir + lexerClassName + ".h";
			try
			{
				Console.Error.WriteLine ("Generate scanner header file '" + fileName + "'");
				StreamWriter f = File.CreateText (fileName);

				f.WriteLine ("#pragma once");
				f.WriteLine ("");
				if (lexerInheritedClassName != null)
					f.WriteLine ($"#include \"{lexerInheritedClassName}.h\"");

				foreach (string line in g_ScannerSkeletonHdr)
				{
					if (line == g_ScannerAnonCode)
					{
						if (anonymousTokens.Count > 0)
							f.WriteLine (anonymousSwitch);
					}
					else if (line == g_ScannerCodes)
					{
						foreach (ScannerPart scanner_Part_ in scannerParts.Values)
							f.WriteLine ("\tstatic const int " + scanner_Part_.part.m__identifier_.text + " = " + scanner_Part_.counter+ ";");
						if (anonymousTokens.Count > 0)
							f.WriteLine ("\tstatic const int anonymousPartIndex = " + scannerPartsCounter + ";");
					}
					else if (line == g_ScannerTokenCodes)
					{
						foreach (SymbolToken p_SymbolToken in m_terminals)
						{
							f.WriteLine ("\tstatic const int " + p_SymbolToken.correctName + " = " + p_SymbolToken.index + ";");
						}
					}
					else if (line == g_ScannerClassDef)
					{
						if (lexerInheritedClassName != null)
							f.WriteLine ("class " + lexerClassName + " : public " + lexerInheritedClassName);
						else
							f.WriteLine ("class " + lexerClassName);
					}
					else
					{
						//f.WriteLine (line.Replace (g_ScannerClassRef, scannerClassName).Replace (g_RegexClassName, regexClassName));
					}
				}

				f.Close ();
				SourceFileList.Add (fileName);
			}
			catch (Exception e)
			{
				Console.Error.WriteLine ($"Cannot create scanner source file {fileName}, exception thrown: {e.Message}");
			}
		}

		private void GenerateLexerSrc ()
		{
			string fileName = OutputDir + lexerClassName + ".cpp";
			try
			{
				Console.Error.WriteLine ("Generate scanner source file '" + fileName + "'");
				StreamWriter f = File.CreateText (fileName);

				foreach (string line in g_ScannerSkeletonSrc)
				{
					if (line == g_ScannerNamespace)
						f.WriteLine ("namespace " + lexerNameSpace);
					else if (line == g_ScannerClassRef)
					{
						if (lexerInheritedClassName != null)
							f.WriteLine ("\tpublic class " + lexerClassName + " : " + lexerInheritedClassName);
						else
							f.WriteLine ("\tpublic class " + lexerClassName);
					}
					else if (line == g_ScannerClassTextReader)
						f.WriteLine ("\t\tpublic " + lexerClassName + " (TextReader textReader)");
					else if (line == g_ScannerClassStringArray)
						f.WriteLine ("\t\tpublic " + lexerClassName + " (string [] lines)");
					else if (line == g_ScannerClassString)
						f.WriteLine ("\t\tpublic " + lexerClassName + " (string line)");
					else if (line == g_ScannerCode)
					{
						if (anonymousTokens.Count > 0)
						{
							f.WriteLine ("\t\t\t\tif (anregInd)");
							f.WriteLine ("\t\t\t\t\treturn anonymousCode [result.token];");
							f.WriteLine ("\t\t\t\telse");
							f.WriteLine ("\t\t\t\t{");
							f.WriteLine (lexerCode);
							f.WriteLine ("\t\t\t\t}");
						}
						else
						{
							f.WriteLine (lexerCode);
						}
					}
					else if (line == g_ScannerAnonCode)
					{
						if (anonymousTokens.Count > 0)
							f.WriteLine (anonymousSwitch);
					}
					else if (line == g_ScannerRegexList)
						f.WriteLine (regexCode);
					//"\t\t\t\tif (anreg != null)",
					else if (line == g_InitialScanner)
					{
						f.WriteLine ("\t\t\tpushScanner (" + scannerId + ");");
						if (anonymousTokens.Count > 0)
							f.WriteLine ("\t\t\tanreg = regarray [anonymousPartIndex];");
					}
					else if (line == g_MatchScanner)
					{
						if (anonymousTokens.Count > 0)
						{
							f.WriteLine ("\t\t\t\tMatch match = null;");
							f.WriteLine ("\t\t\t\tif (!(anregInd = ((regexIndex == " + scannerId + ") && (match = anreg.Match (currentLine)).Success)))");
							f.WriteLine ("\t\t\t\t\tmatch = regex.Match (currentLine);");
						}
						else
						{
							f.WriteLine ("\t\t\t\tMatch match = regex.Match (currentLine);");
						}
					}
					else if (line == g_ScannerCodes)
					{
						foreach (ScannerPart scanner_Part_ in scannerParts.Values)
							f.WriteLine ("\t\tpublic const int " + scanner_Part_.part.m__identifier_.text + " = " + scanner_Part_.counter + ";");
						if (anonymousTokens.Count > 0)
							f.WriteLine ("\t\tpublic const int anonymousPartIndex = " + scannerPartsCounter + ";");
					}
					else if (line == g_ScannerTokenCodes)
					{
						foreach (SymbolToken p_SymbolToken in m_terminals)
						{
							f.WriteLine ("\t\tpublic const int " + p_SymbolToken.correctName + " = " + p_SymbolToken.index + ";");
						}
					}
					else if (line == g_ScannerRegexList)
						f.WriteLine (regexCode);
					else
					{
						f.WriteLine (line.Replace (g_ScannerClassRef, lexerClassName));
					}
				}

				f.Close ();
				SourceFileList.Add (fileName);
			}
			catch (Exception e)
			{
				Console.Error.WriteLine ($"Cannot create scanner source file {fileName}, exception thrown: {e.Message}");
			}
		}

		private void GenerateLexer ()
		{
			GenerateLexerLibrary ();
			GenerateLexerHdr ();
			GenerateLexerSrc ();
		}

		private void GenerateSyntaxTreeBase ()
		{
			GenerateSyntaxTreeBaseHdr ();
			GenerateSyntaxTreeBaseSrc ();
		}

		private void GenerateSyntaxTreeCBHdr ()
		{
			StreamWriter f = File.CreateText ("SyntaxTreeWalker.h");

			f.WriteLine ("#pragma once");
			f.WriteLine ();
			f.WriteLine ("#include \"parse-syntax.h\"");
			f.WriteLine ();
			f.WriteLine ("class SyntaxTreeWalker");
			f.WriteLine ("{");
			f.WriteLine ("public:");
			f.WriteLine ("\tSyntaxTreeWalker () {}");
			f.WriteLine ("\tvirtual	~SyntaxTreeWalker () {}");
			f.WriteLine ("\tvirtual bool\tInvoke__Common__Callback (SyntaxTreeCallbackReason reason, int kind, SyntaxTreeBase* p_node) = 0;");
			foreach (SymbolToken p_SymbolToken in m_prodlist.Keys)
			{
				if (p_SymbolToken.name[0] == '$')
					continue;
				string productionName = p_SymbolToken.correctName;
				f.WriteLine ("\tvirtual bool\tInvoke_" + productionName + "_Callback (SyntaxTreeCallbackReason reason, " + productionName + "::production_kind kind, " + productionName + "* p_" + productionName + ") = 0;");
			}
			f.WriteLine ();
			foreach (SymbolToken p_SymbolToken in m_prodlist.Keys)
			{
				if (p_SymbolToken.name[0] == '$')
					continue;
				string productionName = p_SymbolToken.correctName;
				f.WriteLine ("\tvoid Traverse (" + productionName + "* p_" + productionName + ");");
			}
			f.WriteLine ();
			f.WriteLine ("\tvoid TraverseCommon (SyntaxTreeToken* p_token);");
			foreach (SymbolToken p_SymbolToken in m_prodlist.Keys)
			{
				if (p_SymbolToken.name[0] == '$')
					continue;
				string productionName = p_SymbolToken.correctName;
				f.WriteLine ("\tvoid TraverseCommon (" + productionName + "* p_" + productionName + ");");
			}
			f.WriteLine ();

			f.WriteLine ("\tSyntaxTreeBase* get_parent (SyntaxTreeBase* node, unsigned int id)");
			f.WriteLine ("\t {");
			f.WriteLine ("\t\tSyntaxTreeBase* parent;");
			f.WriteLine ("\t\tfor (parent = node; (parent != 0) && (parent->id () != id); parent = parent->parent ());");
			f.WriteLine ("\t\treturn parent;");
			f.WriteLine ("\t}");

			f.WriteLine ();
			f.WriteLine ("public:");
			f.WriteLine ("\tenum ProductionID");
			f.WriteLine ("\t{");
			f.WriteLine ("\t\tInvalidProductionID = 0,");
			foreach (SymbolToken p_SymbolToken in m_prodlist.Keys)
			{
				if (p_SymbolToken.name[0] == '$')
					continue;
				string productionName = p_SymbolToken.correctName;
				f.WriteLine ("\t\t_" + productionName + "_ID = " + p_SymbolToken.index + ",");
			}
			f.WriteLine ("\t\tLastProductionID");
			f.WriteLine ("\t};");
			f.WriteLine ("};");

			f.Close ();

			f = File.CreateText ("SyntaxTreeBuilder.h");

			f.WriteLine ("#pragma once");
			f.WriteLine ();
			f.WriteLine ("#include \"parse-syntax.h\"");
			f.WriteLine ("#include \"SyntaxTreeWalker.h\"");
			f.WriteLine ();
			f.WriteLine ("class SyntaxTreeBuilder : public SyntaxTreeWalker");
			f.WriteLine ("{");
			f.WriteLine ("public:");
			f.WriteLine ("\tSyntaxTreeBuilder () {}");
			f.WriteLine ("\tvirtual	~SyntaxTreeBuilder () {}");
			f.WriteLine ();
			foreach (KeyValuePair<SymbolToken, RhsProductionNode> keyval in m_prodlist)
			{
				SymbolToken p_SymbolToken = keyval.Key;
				RhsProductionNode p_RhsProductionNode = keyval.Value;
				if (p_SymbolToken.name[0] == '$')
					continue;
				if (p_RhsProductionNode != null)
					p_RhsProductionNode.GenerateCppCallbackPrototypes (f);
			}
			f.WriteLine ("};");

			f.Close ();
		}

		private void GenerateSyntaxTreeCBSrc ()
		{
			StreamWriter f = File.CreateText ("SyntaxTreeWalker.cpp");

			f.WriteLine ("#include \"SyntaxTreeWalker.h\"");
			f.WriteLine ();
			f.WriteLine ("void SyntaxTreeWalker::TraverseCommon (SyntaxTreeToken* p_token)");
			f.WriteLine ("{");
			f.WriteLine ("\tInvoke__Common__Callback (TraversalPrologueCallbackReason, (int) -1, p_token);");
			f.WriteLine ("\tInvoke__Common__Callback (TraversalEpilogueCallbackReason, (int) -1, p_token);");
			f.WriteLine ("}");
			f.Close ();

			f = File.CreateText ("SyntaxTreeBuilder.cpp");

			f.WriteLine ("#include \"SyntaxTreeBuilder.h\"");
			f.Close ();
		}

		private void GenerateSyntaxTreeCB ()
		{
			GenerateSyntaxTreeCBHdr ();
			GenerateSyntaxTreeCBSrc ();
		}

		private void GenerateSyntaxTreeClasses ()
		{
			foreach (KeyValuePair<SymbolToken, RhsProductionNode> keyval in m_prodlist)
			{
				SymbolToken p_SymbolToken = keyval.Key;
				RhsProductionNode p_RhsProductionNode = keyval.Value;

				if (p_SymbolToken.name[0] == '$')
					continue;

				string srcName = p_SymbolToken.correctName + ".cpp";
				string hdrName = p_SymbolToken.correctName + ".h";
				StreamWriter src = File.CreateText (srcName);
				StreamWriter hdr = File.CreateText (hdrName);

				if (p_RhsProductionNode != null)
				{
					Console.Error.WriteLine ("Generate header file '" + hdrName + "'");
					p_RhsProductionNode.GenerateCppHeaderFile (hdr);
					Console.Error.WriteLine ("Generate source file '" + srcName + "'");
					p_RhsProductionNode.GenerateCppSourceFile (src);
				}
				src.Close ();
				hdr.Close ();
			}
		}

		private void GenerateSyntaxTreeHelperHdr ()
		{
			StreamWriter f = File.CreateText ("SyntaxTreeCallbackDecls.h");

			f.WriteLine ("\tvirtual bool\tInvoke__Common__Callback (SyntaxTreeCallbackReason reason, int kind, SyntaxTreeBase* p_node);");
			foreach (SymbolToken p_SymbolToken in m_prodlist.Keys)
			{
				if (p_SymbolToken.name[0] == '$')
					continue;
				string productionName = p_SymbolToken.correctName;
				f.WriteLine ("\tvirtual bool\tInvoke_" + productionName + "_Callback (SyntaxTreeCallbackReason reason, " + productionName + "::production_kind kind, " + productionName + "* p_" + productionName + ");");
			}

			f.Close ();
		}

		private void GenerateSyntaxTreeHelperSrc ()
		{
			StreamWriter f = File.CreateText ("SyntaxTreeCallbackImpls.cpp");

			f.WriteLine ();
			f.WriteLine ("#if !defined(_Common__CallbackDefined)");
			f.WriteLine ("bool\tCLASSNAME::Invoke__Common__Callback (SyntaxTreeCallbackReason reason, int kind, SyntaxTreeBase* p_node)");
			f.WriteLine ("{");
			f.WriteLine ("\tbool result = true;");
			f.WriteLine ("\tswitch (reason)");
			f.WriteLine ("\t{");
			f.WriteLine ("\tcase\tBuilderCallbackReason:");
			f.WriteLine ("\t\tbreak;");
			f.WriteLine ("\tcase\tTraversalPrologueCallbackReason:");
			f.WriteLine ("\t\tbreak;");
			f.WriteLine ("\tcase\tTraversalMidTermCallbackReason:");
			f.WriteLine ("\t\tbreak;");
			f.WriteLine ("\tcase\tTraversalEpilogueCallbackReason:");
			f.WriteLine ("\t\tbreak;");
			f.WriteLine ("\t}");
			f.WriteLine ("\treturn result;");
			f.WriteLine ("}");
			f.WriteLine ("#endif");

			foreach (SymbolToken p_SymbolToken in m_prodlist.Keys)
			{
				if (p_SymbolToken.name[0] == '$')
					continue;
				string productionName = p_SymbolToken.correctName;
				f.WriteLine ();
				f.WriteLine ("#if !defined(" + productionName + "_CallbackDefined)");
				f.WriteLine ("bool\tCLASSNAME::Invoke_" + productionName + "_Callback (SyntaxTreeCallbackReason reason, " + productionName + "::production_kind kind, " + productionName + "* p_" + productionName + ")");
				f.WriteLine ("{");
				f.WriteLine ("\tbool result = true;");
				f.WriteLine ("\tswitch (reason)");
				f.WriteLine ("\t{");
				f.WriteLine ("\tcase\tBuilderCallbackReason:");
				f.WriteLine ("\t\tbreak;");
				f.WriteLine ("\tcase\tTraversalPrologueCallbackReason:");
				f.WriteLine ("\t\tbreak;");
				f.WriteLine ("\tcase\tTraversalMidTermCallbackReason:");
				f.WriteLine ("\t\tbreak;");
				f.WriteLine ("\tcase\tTraversalEpilogueCallbackReason:");
				f.WriteLine ("\t\tbreak;");
				f.WriteLine ("\t}");
				f.WriteLine ("\treturn result;");
				f.WriteLine ("}");
				f.WriteLine ("#endif");
			}

			f.Close ();
		}

		private void GenerateSyntaxTreeHelper ()
		{
			GenerateSyntaxTreeHelperHdr ();
			GenerateSyntaxTreeHelperSrc ();
		}

		private void GenerateSemanticClasses ()
		{
			PrepareSyntaxTree ();
			GenerateParseHeaders ();
			GenerateSyntaxTreeBase ();
			GenerateSyntaxTreeCB ();
			GenerateSyntaxTreeClasses ();
			GenerateSyntaxTreeHelper ();
		}

		private int ResizeTransitionTables (int size, int increment)
		{
			int newsize = size + increment;
			Array.Resize (ref m_check, newsize);
			Array.Resize (ref m_state, newsize);
			return newsize;
		}

		private int ResizeReductionTables (int size, int increment)
		{
			int newsize = size + increment;
			Array.Resize (ref m_rcheck, newsize);
			Array.Resize (ref m_rstate, newsize);
			return newsize;
		}

		private int ResizeGLRTables (int size, int increment)
		{
			int newsize = size + increment;
			Array.Resize (ref m_glrcheck, newsize);
			Array.Resize (ref m_glrstate, newsize);
			return newsize;
		}

		public static bool g_displayProductions = false;
		private static bool g_displayStartSets = false;
		private static bool g_displayEndSets = false;

		_anglr_file_ m_anglrFile;
		string m_className;

		rhsstateset m_stateset = new rhsstateset ();
		statearray m_statearray = new statearray ();

		shiftset m_shiftset = new shiftset ();
		gotoset m_gotoset = new gotoset ();

		int m_maxCheck = -1;
		int m_maxRCheck = -1;
		int m_maxGLRCheck = -1;

		int[] m_terminalCodes = null;
		int[] m_nonTerminalCodes = null;
		int[] m_check = null;
		int[] m_state = null;
		int[] m_shiftDelta = null;
		int[] m_gotoDelta = null;
		int[] m_productionLengths = null;
		int[] m_productionRules = null;
		int[] m_defaultGoto = null;
		int[] m_reductions = null;
		int[] m_rcheck = null;
		int[] m_rstate = null;
		int[] m_glrcheck = null;
		int[] m_glrstate = null;
		int[] m_glrcells = null;

		TextWriter m_hdrFile = null;

		private DirectoryInfo directoryInfo = null;
		public string OutputDir { get; private set; } = "";
		public ArrayList SourceFileList { get; private set; } = new ArrayList ();
		public ArrayList LibraryFileList { get; private set; } = new ArrayList ();
		private Regex regmatch = new Regex ("\\(", RegexOptions.ECMAScript);
	}
}
