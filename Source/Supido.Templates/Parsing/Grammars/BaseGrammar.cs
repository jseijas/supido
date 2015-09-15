using System;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Supido.Templates.Parsing.Grammars
{
    /// <summary>
    /// Base grammar, with the primitive rules.
    /// </summary>
    public class BaseGrammar
    {
        public static Rule Node(Rule rule)
        {
            return new NodeRule(rule);
        }

        public static Rule Recursive(Func<Rule> ruleFunction)
        {
            return new RecursiveRule(ruleFunction);
        }

        public static Rule At(Rule rule)
        {
            return new AtRule(rule);
        }

        public static Rule Sequence(params Rule[] rules)
        {
            return new SequenceRule(rules);
        }

        public static Rule Condition(params Rule[] rules)
        {
            return new IfRule(rules);
        }

        public static Rule End = new EndRule();

        public static Rule Not(Rule rule)
        {
            return new NotRule(rule);
        }

        public static Rule ZeroOrMore(Rule rule)
        {
            return new RepeatRule(rule);
        }

        public static Rule OneOrMore(Rule rule)
        {
            return new WhileRule(rule);
        }

        public static Rule Opt(Rule rule)
        {
            return new OptRule(rule);
        }

        public static Rule MatchString(string s)
        {
            return new StringRule(s);
        }

        public static Rule MatchChar(Predicate<char> charPredicate)
        {
            return new CharacterRule(charPredicate);
        }

        public static Rule MatchChar(char c)
        {
            return MatchChar(x => x == c).SetName(c.ToString());
        }

        public static Rule MatchRegex(Regex regex)
        {
            return new RegexRule(regex);
        }

        public static Rule CharSet(string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                throw new ArgumentException("The charset cannot be empty");
            }
            return MatchChar(c => s.Contains(c)).SetName(string.Format("[{0}]", s));
        }

        public static Rule CharRange(char a, char b)
        {
            return MatchChar(c => (c >= a) && (c <= b)).SetName(string.Format("[{0}..{1}]", a, b));
        }

        public static Rule ExceptCharSet(string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                throw new ArgumentException("The charset cannot be empty");
            }
            return MatchChar(c => !s.Contains(c)).SetName(String.Format("[{0}]", s));
        }

        public static Rule AnyChar = MatchChar(c => true).SetName(".");

        public static Rule AdvanceWhileNot(Rule rule)
        {
            if (rule == null)
            {
                throw new ArgumentNullException("The rule cannot be null");
            }
            return ZeroOrMore(Sequence(Not(rule), AnyChar));
        }

        public static Rule Pattern(string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                throw new ArgumentException("The pattern cannot be empty");
            }
            return MatchRegex(new Regex(s));
        }

        public static void InitGrammar(Type type)
        {
            foreach (FieldInfo field in type.GetFields())
            {
                if (field.FieldType.Equals(typeof(Rule)))
                {
                    Rule rule = field.GetValue(null) as Rule;
                    if (rule == null)
                        throw new Exception("Unexpected null rule");
                    rule.Name = field.Name;
                }
            }
        }

    }
}
