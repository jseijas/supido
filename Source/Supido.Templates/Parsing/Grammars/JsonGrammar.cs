
namespace Supido.Templates.Parsing.Grammars
{
    /// <summary>
    /// Grammar for json object.
    /// </summary>
    public class JsonGrammar : CommonGrammar
    {
        new public static Rule Integer = Node(CommonGrammar.Integer);

        new public static Rule Float = Node(CommonGrammar.Float);

        public static Rule True = Node(MatchString("true"));

        public static Rule False = Node(MatchString("false"));

        public static Rule Null = Node(MatchString("null"));

        public static Rule UnicodeChar = MatchString("\\u") + HexDigit + HexDigit + HexDigit + HexDigit;

        public static Rule ControlChar = MatchChar('\\') + CharSet("\"\'\\/bfnt");

        public static Rule DoubleQuotedString = Node(MatchChar('"') + ZeroOrMore(UnicodeChar | ControlChar | ExceptCharSet("\"\\")) + MatchChar('"'));

        public static Rule SingleQuotedString = Node(MatchChar('\'') + ZeroOrMore(UnicodeChar | ControlChar | ExceptCharSet("'\\")) + MatchChar('\''));

        public static Rule String = Node(DoubleQuotedString | SingleQuotedString);

        public static Rule Number = Float | Integer;

        public static Rule Value = Node(Recursive(() => String | Number | Object | Array | True | False | Null));

        public static Rule Pair = Node(DoubleQuotedString + WS + CharToken(':') + Value + WS);

        public static Rule Array = Node(CharToken('[') + CommaDelimited(Value) + WS + CharToken(']'));

        public static Rule Object = Node(CharToken('{') + CommaDelimited(Pair) + WS + CharToken('}'));

        /// <summary>
        /// Initializes the <see cref="JsonGrammar"/> class.
        /// </summary>
        static JsonGrammar() 
        { 
            InitGrammar(typeof(JsonGrammar)); 
        }
    }
}
