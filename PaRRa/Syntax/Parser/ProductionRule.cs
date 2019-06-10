using System;
using System.Collections;
using System.Collections.Generic;

namespace PaRRa.Parser
{
    public class ProductionRule
    {
        public GrammaticalStructure[] Decomposition { get; set; }
        public string name;

        public GrammaticalStructure this[int index]
        {
            get => Decomposition[index];
            set => Decomposition[index] = value;
        }
        public int Length => Decomposition.Length;

        public ProductionRule(string name, params GrammaticalStructure[] decomposition)
        {
            this.name = name;
            this.Decomposition = decomposition;
        }
    }
}
