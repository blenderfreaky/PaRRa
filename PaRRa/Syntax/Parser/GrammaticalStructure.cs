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

        public IEnumerator<ProductionRule> GetEnumerator() => ((IEnumerable<ProductionRule>)ProductionRules).GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable<ProductionRule>)ProductionRules).GetEnumerator();

        public int Count => ProductionRules.Count;
    }
}
