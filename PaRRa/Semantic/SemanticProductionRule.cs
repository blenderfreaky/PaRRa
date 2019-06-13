using PaRRa.Parser;
using System;
using System.Collections.Generic;
using System.Text;

namespace PaRRa.Semantic
{
    public class SemanticProductionRule
    {
        public Func<SemanticParseTreeNode, SemanticTypeError> TypeCheck;
        public Func<SemanticParseTreeNode, SemanticObject> Execute;

        public SemanticProductionRule[][] GrammaticalStructures { get; set; }

        public SemanticProductionRule(string name, Func<ParseTreeNode[], object> eval, params GrammaticalStructure[] grammaticalStructures) { }


    }
}
