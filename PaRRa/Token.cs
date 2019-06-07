using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace PaRRa
{
    public struct TokenType
    {
        public string name;
        public Regex regex;

        public TokenType(string name, Regex regex)
        {
            this.name = name;
            this.regex = regex;
        }
        public TokenType(string name, string regex) : this(name, CreateRegex(regex)) { }

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

    public struct Token
    {
        public TokenType tokenType;
        public string text;

        public Token(TokenType tokenType, string text)
        {
            this.tokenType = tokenType;
            this.text = text;
        }
    }
}
