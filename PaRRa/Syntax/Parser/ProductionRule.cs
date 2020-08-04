using System;
using System.Collections;
using System.Collections.Generic;

namespace PaRRa.Parser
{
    public class ProductionRule : IEnumerable<GrammaticalStructure>
    {
        public GrammaticalStructure[] decomposition;
        public string name;
        public Func<ParseTreeNode[], object, object> _eval;

        public GrammaticalStructure this[int index]
        {
            get => decomposition[index];
            set => decomposition[index] = value;
        }
        public int Length => decomposition.Length;

        public ProductionRule(string name, Func<ParseTreeNode[], object, object> eval, params GrammaticalStructure[] grammaticalStructures)
        {
            this.decomposition = grammaticalStructures;
            this.name = name;
            _eval = eval;
        }

        public object Eval(ParseTreeNode[] nodes, object state = null)
        {
            if (nodes.Length != decomposition.Length) throw new ArgumentException("Input does not match pattern");
            for (int i = 0; i < nodes.Length; i++) if (nodes[i].grammaticalStructure != decomposition[i]) throw new ArgumentException("Input does not match pattern");

            return _eval(nodes, state);
        }

        public IEnumerator<GrammaticalStructure> GetEnumerator() => ((IEnumerable<GrammaticalStructure>)decomposition).GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable<GrammaticalStructure>)decomposition).GetEnumerator();
    }
}
