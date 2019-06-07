using System.Collections.Generic;
using System.Linq;

namespace PaRRa.Dynamic
{
    public sealed class Language
    {
        public readonly ICollection<TokenType> tokenTypes;
        public readonly GrammaticalStructure startingGrammaticalStructure;

        public Language(ICollection<TokenType> tokenTypes, GrammaticalStructure startingGrammaticalStructure)
        {
            this.tokenTypes = tokenTypes;
            this.startingGrammaticalStructure = startingGrammaticalStructure;
        }

        public ParseTreeNode Parse(string code)
        {
            IEnumerable<Token> tokens = Lexer.GetTokens(code, tokenTypes);

            ParseTreeNode startingNode = new ParseTreeNode(startingGrammaticalStructure, tokens.ToList());
            startingNode.Parse();

            return startingNode;
        }
    }
}
