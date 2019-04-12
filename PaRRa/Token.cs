using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace PaRRa
{
    public class TokenType
    {
        public string name;
        public Regex regex;

        public TokenType(string name, Regex regex)
        {
            this.name = name;
            this.regex = regex;
        }
        public TokenType(string name, string regex) : this(name, new Regex("\\G" + regex, RegexOptions.Compiled | RegexOptions.CultureInvariant)) { }
    }
}
