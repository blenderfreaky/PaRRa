using PaRRa.Syntax.Lexer;
using System.Collections.Generic;
using System.Linq;
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

        public override List<Terminal> FirstSet => this.Yield().ToList();
    }
}
