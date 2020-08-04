using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace PaRRa.Parser
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

        public override int GetHashCode()
        {
            int hashCode = 11130958;
            hashCode = (hashCode * -1521134295) + EqualityComparer<string>.Default.GetHashCode(name);
            hashCode = (hashCode * -1521134295) + EqualityComparer<Regex>.Default.GetHashCode(regex);
            return hashCode;
        }

        public static bool operator ==(TokenType lhs, TokenType rhs) => lhs.GetHashCode() == rhs.GetHashCode()
            && lhs.name == rhs.name && EqualityComparer<Regex>.Default.Equals(lhs.regex, rhs.regex);

        public static bool operator !=(TokenType lhs, TokenType rhs) => lhs.GetHashCode() != rhs.GetHashCode()
            || lhs.name != rhs.name || !EqualityComparer<Regex>.Default.Equals(lhs.regex, rhs.regex);
    }
}
