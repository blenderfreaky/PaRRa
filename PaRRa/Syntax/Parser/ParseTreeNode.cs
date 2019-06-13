using PaRRa.Syntax.Lexer;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PaRRa.Parser
{
    public sealed class ASTNode
    {
        public GrammaticalStructure grammaticalStructure;
        public ProductionRule rule;
        public List<ASTNode> tree;
        public List<Token> tokens;

        internal ASTNode(GrammaticalStructure grammaticalStructure, ProductionRule rule, List<ASTNode> tree, List<Token> tokens)
        {
            this.grammaticalStructure = grammaticalStructure;
            this.rule = rule;
            this.tree = tree;
            this.tokens = tokens;
        }

        public static void Parse(IEnumerable<Token> tokens, GrammaticalStructure start, ICollection<GrammaticalStructure> grammaticalStructures)
        {
            var firstSets = new Dictionary<TokenType, List<GrammaticalStructure>>();
            grammaticalStructures
                .SelectMany(g => g.FirstSet.Select(first => (first, g)))
                .ForEach(x => firstSets.GetOrCreate(x.first.TokenType).AddIfNew(x.g));

            Stack<ASTNode> stack = new Stack<ASTNode>();

            foreach (Token token in tokens)
            {
                if (firstSets.TryGetValue(token.tokenType, out var options) && options.Any())
                {

                }
                else
                {
                    throw new ArgumentException();
                }
            }
        }
    }
}
