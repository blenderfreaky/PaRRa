using System;
using System.Collections.Generic;
using System.Linq;

namespace PaRRa.Generator
{
    public sealed class ProductionRuleGenerator
    {
        public string name;
        public Func<ParseTreeNode[], object> _eval;
        public string[] decomposition;

        public ProductionRuleGenerator SetName(string name)
        {
            this.name = name;

            return this;
        }

        public ProductionRuleGenerator SetEval(Func<ParseTreeNode[], object> eval)
        {
            _eval = eval;
            
            return this;
        }

        public ProductionRuleGenerator SetDecomposition(params string[] decomposition)
        {
            this.decomposition = decomposition;

            return this;
        }

        public ProductionRule Build(Dictionary<string, GrammaticalStructure> grammaticalStructures) =>
            new ProductionRule(name, _eval, decomposition.Select(x => grammaticalStructures[x]).ToArray());
    }
}