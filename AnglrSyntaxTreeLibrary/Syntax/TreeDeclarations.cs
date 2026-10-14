namespace Tree.Declarations
{
	public class TreeDeclarations
	{
		// values of terminal symbols
		public class tokens
		{
			public const int _left_curly_bracket_ = 258;
			public const int _right_curly_bracket_ = 259;
			public const int _colon_ = 260;
			public const int _coma_ = 261;
			public const int _cstring_ = 262;
			public const int _number_ = 263;
			public const int _null_ = 264;
		}

		// values of regular expressions
		public class regex
		{
			public const string colon = @"\:";
			public const string coma = @"\,";
			public const string left_curly_bracket = @"\{";
			public const string right_curly_bracket = @"\}";
			public const string digit = @"[0-9]";
			public const string cstring = @"\""([^""]|\""\"")*\""|\'([^']|\'\')*\'";
			public const string number = @"[0-9]+";
			public const string rx_null = @"null";
		}
	}
}

