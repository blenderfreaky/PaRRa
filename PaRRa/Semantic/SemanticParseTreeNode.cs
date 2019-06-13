using System.Collections.Generic;
using PaRRa.Parser;
using PaRRa.Syntax.Lexer;

namespace PaRRa.Semantic
{
    public class SemanticParseTreeNode
    {
        public Dictionary<string, SemanticObject> scopedObjects;
        public Dictionary<string, SemanticType> scopedTypes;
        public SemanticProductionRule rule;

        public SemanticTypeError TypeCheck() => rule.TypeCheck(this);
        public SemanticObject Execute() => rule.Execute(this);
    }
}