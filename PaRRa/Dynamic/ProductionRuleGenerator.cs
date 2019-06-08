using PaRRa.Parser;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PaRRa.Generator
{
    public sealed class ProductionRuleGenerator
    {
        internal string name;
        internal Func<ParseTreeNode[], object> _eval;
        internal string[] decomposition;

        public ProductionRuleGenerator()
        {
            name = string.Empty;
            _eval = nodes => throw new NotImplementedException();
            decomposition = new string[0];
        }

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