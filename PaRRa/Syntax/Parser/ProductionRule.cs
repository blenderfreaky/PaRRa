using System;
using System.Collections;
using System.Collections.Generic;

namespace PaRRa.Parser
{
    public class ProductionRule
    {
        public GrammaticalStructure[] decomposition;
        public string name;

        public GrammaticalStructure this[int index]
        {
            get => decomposition[index];
            set => decomposition[index] = value;
        }
        public int Length => decomposition.Length;

        public ProductionRule(string name, params GrammaticalStructure[] decomposition)
        {
            this.name = name;
            this.decomposition = decomposition;
        }
    }
}
