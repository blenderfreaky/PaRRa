using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PaRRa.Parser
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
}
