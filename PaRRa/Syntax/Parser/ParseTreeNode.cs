using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PaRRa.Parser
{
    public sealed class ParseTreeNode
    {
        public bool IsTerminal => tree?.Any() != true;
        public GrammaticalStructure grammaticalStructure;
        public ProductionRule rule;
        public List<Token> tokens;
        public ParseTreeNode[] tree;
        public ParseTreeNode parent;

        public ParseTreeNode(GrammaticalStructure grammaticalStructure, List<Token> tokens, ParseTreeNode parent = null)
        {
            this.grammaticalStructure = grammaticalStructure;
            this.tokens = tokens;
            this.parent = parent;
            tree = null;
        }

        public object Eval(object state = null) => rule.Eval(tree, state);

        public int Parse()
        {
            if (tokens.Count == 0) return 0;

            if (grammaticalStructure is Terminal terminal)
            {
                if (tokens[0].tokenType == terminal.TokenType)
                {
                    tokens = new List<Token>(tokens.Take(1));
                    return 1;
                }
                return 0;
            }

            for (ParseTreeNode parent = this.parent?.parent; parent != null; parent = parent.parent)
            {
                if (parent.grammaticalStructure == grammaticalStructure && parent.tokens.SequenceEqual(tokens)) return 0;
            }

            List<(List<ParseTreeNode> tree, int length, ProductionRule rule)> options = new List<(List<ParseTreeNode> tree, int length, ProductionRule rule)>();

            foreach (ProductionRule productionRule in grammaticalStructure)
            {
                List<ParseTreeNode> recursiveTree = new List<ParseTreeNode>();
                IEnumerable<Token> recursiveTokens = tokens;
                int recursiveLength = 0;

                foreach (GrammaticalStructure grammaticalStructure in productionRule)
                {
                    ParseTreeNode node = new ParseTreeNode(grammaticalStructure, recursiveTokens.ToList(), this);
                    int length = node.Parse();
                    if (length > 0)
                    {
                        recursiveTree.Add(node);
                        recursiveTokens = recursiveTokens.Skip(length);
                        recursiveLength += length;
                    }
                    else
                    {
                        recursiveTree = null;
                        break;
                    }
                }

                if (recursiveTree != null)
                {
                    options.Add((recursiveTree, recursiveLength, productionRule));
                }
            }

            if (options.Count == 0) return 0;
            (List<ParseTreeNode> tree, int length, ProductionRule rule) max = options[0];
            foreach ((List<ParseTreeNode> tree, int length, ProductionRule rule) option in options) if (option.length > max.length) max = option;
            tree = max.tree.ToArray();
            tokens = tokens.Take(max.length).ToList();
            rule = max.rule;
            return max.length;
        }
    }
}
