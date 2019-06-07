using System;
using System.Collections.Generic;
using System.Linq;

namespace PaRRa.Generator
{
    public sealed class GrammaticalStructureGenerator
    {
        public List<ProductionRuleGenerator> productionRules;

        public GrammaticalStructureGenerator AddProductionRule(string name, Func<ProductionRuleGenerator, ProductionRuleGenerator> productionRule)
        {
            productionRules.Add(productionRule(new ProductionRuleGenerator().SetName(name)));

            return this;
        }

        public GrammaticalStructure Build(string name, Dictionary<string, GrammaticalStructure> grammaticalStructures) =>
            new GrammaticalStructure(name, productionRules.Select(x => x.Build(grammaticalStructures)).ToList());
        public void BuildOnto(GrammaticalStructure structure, Dictionary<string, GrammaticalStructure> grammaticalStructures) =>
            productionRules.Select(x => x.Build(grammaticalStructures)).ToList().ForEach(structure.AddRule);
    }
}
