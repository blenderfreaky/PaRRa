using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace PaRRa.Parser
{
    public sealed class Terminal : GrammaticalStructure
    {
        public override bool IsTerminal => true;
        public override List<ProductionRule> ProductionRules => null;
        public TokenType TokenType { get; }

        public Terminal(TokenType token)
        {
            name = token.name;
            TokenType = token;
        }
        public Terminal(string name, Regex regex) => TokenType = new TokenType(this.name = name, regex);
        public Terminal(out TokenType token, string name, Regex regex) : this(name, regex) => TokenType = token = new TokenType(name, regex);
        public Terminal(List<TokenType> tokenList, string name, Regex regex) : this(name, regex) => tokenList?.Add(TokenType);
        public Terminal(List<TokenType> tokenList, out TokenType token, string name, Regex regex) : this(out token, name, regex) => tokenList?.Add(TokenType);

        public Terminal(string name, string regex) => TokenType = new TokenType(this.name = name, regex);
        public Terminal(out TokenType token, string name, string regex) : this(name, regex) => TokenType = token = new TokenType(name, regex);
        public Terminal(List<TokenType> tokenList, string name, string regex) : this(name, regex) => tokenList?.Add(TokenType);
        public Terminal(List<TokenType> tokenList, out TokenType token, string name, string regex) : this(out token, name, regex) => tokenList?.Add(TokenType);
    }
}
