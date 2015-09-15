
namespace Supido.Templates.Parsing.Grammars
{
    /// <summary>
    /// Grammar for Arithmetic operations.
    /// </summary>
    public class ArithmeticGrammar : CommonGrammar
    {
        public new static Rule Integer = Node(CommonGrammar.Integer);

        public new static Rule Float = Node(CommonGrammar.Float);

        public static Rule RecExpr = Recursive(() => Expression);

        public static Rule ParanExpr = Node(CharToken('(') + RecExpr + WS + CharToken(')'));

        public static Rule Number = (Integer | Float) + WS;

        public static Rule PrefixOp = Node(MatchStringSet("! - ~"));

        public static Rule PrefixExpr = Node(PrefixOp + Recursive(() => SimpleExpr));

        public static Rule SimpleExpr = PrefixExpr | Number | ParanExpr;

        public static Rule BinaryOp = Node(MatchStringSet("<= >= == != << >> && || < > & | + - * % / ^"));

        public static Rule Expression = Node(SimpleExpr + ZeroOrMore(BinaryOp + WS + SimpleExpr));

        /// <summary>
        /// Initializes the <see cref="ArithmeticGrammar"/> class.
        /// </summary>
        static ArithmeticGrammar() 
        { 
            InitGrammar(typeof(ArithmeticGrammar)); 
        }
    }
}
