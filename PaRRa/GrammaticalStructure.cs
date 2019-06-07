using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace PaRRa
{
    public class GrammaticalStructure : IReadOnlyCollection<ProductionRule>
    {
        public bool IsTerminal => ProductionRules == null;
        protected virtual List<ProductionRule> ProductionRules { get; }

        public string name;

        public GrammaticalStructure(string name, List<ProductionRule> productionRules)
        {
            this.name = name;
            ProductionRules = productionRules;
        }
        public GrammaticalStructure(string name = "") : this(name, new List<ProductionRule>()) { }

        public void AddRule(ProductionRule productionRule) => ProductionRules.Add(productionRule);

        [Obsolete]
        public void Add(string name, Func<ParseTreeNode[], object> eval, params GrammaticalStructure[] grammaticalStructures) => ProductionRules.Add(new ProductionRule(name, eval, grammaticalStructures.Select(x => x ?? this).ToArray()));
        [Obsolete]
        public void Replace(GrammaticalStructure pattern, GrammaticalStructure result)
        {
            foreach (ProductionRule productionRule in ProductionRules)
            {
                for (int i = 0; i < productionRule.Length; i++)
                {
                    if (productionRule[i] == pattern) productionRule[i] = result;
                }
            }
        }

        public IEnumerator<ProductionRule> GetEnumerator() => ((IEnumerable<ProductionRule>)ProductionRules).GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable<ProductionRule>)ProductionRules).GetEnumerator();

        public int Count => ProductionRules.Count;
    }

    public class ProductionRule : IEnumerable<GrammaticalStructure>
    {
        public GrammaticalStructure[] decomposition;
        public string name;
        public Func<ParseTreeNode[], object> _eval;

        public GrammaticalStructure this[int index]
        {
            get => decomposition[index];
            set => decomposition[index] = value;
        }
        public int Length => decomposition.Length;

        public ProductionRule(string name, Func<ParseTreeNode[], object> eval, params GrammaticalStructure[] grammaticalStructures)
        {
            this.decomposition = grammaticalStructures;
            this.name = name;
            _eval = eval;
        }

        public object Eval(ParseTreeNode[] nodes)
        {
            if (nodes.Length != decomposition.Length) throw new ArgumentException("Input does not match pattern");
            for  (int  i =  0; i < nodes.Length; i++) if (nodes[i].grammaticalStructure != decomposition[i]) throw new ArgumentException("Input does not match pattern");

            return _eval(nodes);
        }

        public IEnumerator<GrammaticalStructure> GetEnumerator() => ((IEnumerable<GrammaticalStructure>)decomposition).GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable<GrammaticalStructure>)decomposition).GetEnumerator();
    }

    public sealed class GrammaticalPlaceholder : GrammaticalStructure
    { }

    public sealed class Terminal : GrammaticalStructure
    {
        protected override List<ProductionRule> ProductionRules => null;
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
