using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PaRRa
{
    public class Node
    {
        public bool IsTerminal => tree == null || !tree.Any();
        public GrammaticalStructure grammaticalStructure;
        public ProductionRule rule;
        public List<(TokenType token, string text)> tokens;
        public Node[] tree;
        public Node parent;

        public Node(GrammaticalStructure grammaticalStructure, List<(TokenType token, string text)> tokens, Node parent = null)
        {
            this.grammaticalStructure = grammaticalStructure;
            this.tokens = tokens;
            this.parent = parent;
            tree = null;
        }

        public object Eval() => rule.Eval(tree);

        public int Compile()
        {
            if (!tokens.Any()) return 0;

            if (grammaticalStructure is Terminal terminal)
            {
                if (tokens.First().token == terminal.Token)
                {
                    tokens = new List<(TokenType token, string text)>(tokens.Take(1));
                    return 1;
                }
                return 0;
            }

            for (Node parent = this.parent?.parent; parent != null; parent = parent.parent)
            {
                if (parent.grammaticalStructure == grammaticalStructure && parent.tokens.SequenceEqual(tokens)) return 0;
            }

            List<(List<Node> tree, int length, ProductionRule rule)> options = new List<(List<Node> tree, int length, ProductionRule rule)>();

            foreach (ProductionRule productionRule in grammaticalStructure)
            {
                List<Node> recursiveTree = new List<Node>();
                IEnumerable<(TokenType token, string text)> recursiveTokens = tokens;
                int recursiveLength = 0;

                foreach (GrammaticalStructure grammaticalStructure in productionRule)
                {
                    Node node = new Node(grammaticalStructure, recursiveTokens.ToList(), this);
                    int length = node.Compile();
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
            (List<Node> tree, int length, ProductionRule rule) max = options.First();
            foreach ((List<Node> tree, int length, ProductionRule rule) option in options) if (option.length > max.length) max = option;
            tree = max.tree.ToArray();
            tokens = tokens.Take(max.length).ToList();
            rule = max.rule;
            return max.length;
        }
    }
}
