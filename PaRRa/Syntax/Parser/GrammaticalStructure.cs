using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PaRRa.Parser
{
    public class GrammaticalStructure
    {
        public virtual bool IsTerminal => false;
        public virtual List<ProductionRule> ProductionRules { get; }

        public string name;

        public GrammaticalStructure(string name, List<ProductionRule> productionRules)
        {
            this.name = name;
            ProductionRules = productionRules ?? throw new ArgumentException("Production Rules can't be empty. Consider using the Terminal class");
        }
        internal GrammaticalStructure(string name = "")
        {
            this.name = name;
        }
    }
}
