using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace PaRRa
{
    public class GrammaticalStructure : IEnumerable<ProductionRule>
    {
        public bool IsTerminal => ProductionRules == null;
        protected virtual List<ProductionRule> ProductionRules { get; }
        public string name;

        public GrammaticalStructure(List<ProductionRule> productionRules, string name = "")
        {
            ProductionRules = productionRules;
            this.name = name;
        }
        public GrammaticalStructure(string name = "") : this(new List<ProductionRule>(), name) { }

        public void Add(string name, Func<Node[], object> eval, params GrammaticalStructure[] grammaticalStructures) => ProductionRules.Add(new ProductionRule(name, eval, grammaticalStructures.Select(x => x ?? this).ToArray()));
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
    }

    public class ProductionRule : IEnumerable<GrammaticalStructure>
    {
        public GrammaticalStructure[] grammaticalStructures;
        public string name;
        public Func<Node[], object> _eval;

        public GrammaticalStructure this[int index]
        {
            get => grammaticalStructures[index];
            set => grammaticalStructures[index] = value;
        }
        public int Length => grammaticalStructures.Length;

        public ProductionRule(string name, Func<Node[], object> eval, params GrammaticalStructure[] grammaticalStructures)
        {
            this.grammaticalStructures = grammaticalStructures;
            this.name = name;
            _eval = eval;
        }

        public object Eval(Node[] nodes)
        {
            if (nodes.Length != grammaticalStructures.Length) throw new ArgumentException("Input does not match pattern");
            for  (int  i =  0; i < nodes.Length; i++) if (nodes[i].grammaticalStructure != grammaticalStructures[i]) throw new ArgumentException("Input does not match pattern");

            return _eval(nodes);
        }

        public IEnumerator<GrammaticalStructure> GetEnumerator() => ((IEnumerable<GrammaticalStructure>)grammaticalStructures).GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable<GrammaticalStructure>)grammaticalStructures).GetEnumerator();
    }

    public class GrammaticalPlaceholder : GrammaticalStructure
    { }

    public class Terminal : GrammaticalStructure
    {
        protected override List<ProductionRule> ProductionRules => null;
        public TokenType Token { get; }

        public Terminal(TokenType token)
        {
            name = token.name;
            Token = token;
        }
        public Terminal(string name, Regex regex) => Token = new TokenType(this.name = name, regex);
        public Terminal(out TokenType token, string name, Regex regex) : this(name, regex) => Token = token = new TokenType(name, regex);
        public Terminal(List<TokenType> tokenList, string name, Regex regex) : this(name, regex) => tokenList?.Add(Token);
        public Terminal(List<TokenType> tokenList, out TokenType token, string name, Regex regex) : this(out token, name, regex) => tokenList?.Add(Token);

        public Terminal(string name, string regex) => Token = new TokenType(this.name = name, regex);
        public Terminal(out TokenType token, string name, string regex) : this(name, regex) => Token = token = new TokenType(name, regex);
        public Terminal(List<TokenType> tokenList, string name, string regex) : this(name, regex) => tokenList?.Add(Token);
        public Terminal(List<TokenType> tokenList, out TokenType token, string name, string regex) : this(out token, name, regex) => tokenList?.Add(Token);
    }
}
