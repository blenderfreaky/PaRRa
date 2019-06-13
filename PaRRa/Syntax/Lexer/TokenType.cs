using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace PaRRa.Syntax.Lexer
{
    public class TokenType
    {
        public string name;
        public Regex regex;
        public string[] keywords;
        public int precedence;

        public TokenType(string name, Regex regex, int precedence = 0, string[] keywords = null)
        {
            this.name = name;
            this.regex = regex;
            this.precedence = precedence;
            this.keywords = keywords;
        }
        public TokenType(string name, string regex, int precedence, string[] keywords) : this(name, CreateRegex(regex), precedence, keywords) { }

        public static Regex CreateRegex(string regex) => new Regex("\\G" + regex, RegexOptions.Compiled | RegexOptions.CultureInvariant);

        public override bool Equals(object obj) => obj is TokenType type &&
                   name == type.name &&
                   EqualityComparer<Regex>.Default.Equals(regex, type.regex);

        public override int GetHashCode() => HashCode.Combine(name, regex);

        public static bool operator ==(TokenType lhs, TokenType rhs) => lhs.GetHashCode() == rhs.GetHashCode()
            && lhs.name == rhs.name && EqualityComparer<Regex>.Default.Equals(lhs.regex, rhs.regex);

        public static bool operator !=(TokenType lhs, TokenType rhs) => lhs.GetHashCode() != rhs.GetHashCode()
            || lhs.name != rhs.name || !EqualityComparer<Regex>.Default.Equals(lhs.regex, rhs.regex);
    }
}
