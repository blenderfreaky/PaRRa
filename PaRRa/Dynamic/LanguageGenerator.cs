using PaRRa.Generator;
using PaRRa.Parser;
using PaRRa.Syntax.Lexer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace PaRRa.Dynamic
{
    public sealed class LanguageGenerator
    {
        internal Dictionary<string, Regex> tokenTypes;
        internal Dictionary<string, GrammaticalStructureGenerator> grammaticalStructures;
        internal string startingGrammaticalStructure;

        public LanguageGenerator()
        {
            tokenTypes = new Dictionary<string, Regex>();
            grammaticalStructures = new Dictionary<string, GrammaticalStructureGenerator>();
            startingGrammaticalStructure = string.Empty;
        }

        public LanguageGenerator AddTokenType(string name, string regex)
        {
            tokenTypes[name] = TokenType.CreateRegex(regex);

            return this;
        }

        public LanguageGenerator AddGrammaticalStructure(string name, Func<GrammaticalStructureGenerator, GrammaticalStructureGenerator> structure)
        {
            grammaticalStructures[name] = structure(new GrammaticalStructureGenerator());

            return this;
        }

        public LanguageGenerator SetStartingGrammaticalStructure(string name)
        {
            startingGrammaticalStructure = name;

            return this;
        }

        public Language Build()
        {
            List<TokenType> tokenList = new List<TokenType>();
            Dictionary<string, Terminal> terminals = tokenTypes
                .ToDictionary(x => x.Key, x => new Terminal(tokenList, x.Key, x.Value));
            List<(GrammaticalStructureGenerator generator, GrammaticalStructure nonTerminal)> nonTerminals = this.grammaticalStructures
                .Select(x => (x.Value, new GrammaticalStructure(x.Key))).ToList();

            Dictionary<string, GrammaticalStructure> grammaticalStructures = nonTerminals
                .ToDictionary(x => x.nonTerminal.name, x => x.nonTerminal)
                .Concat(terminals.ToDictionary(x => x.Key, x => (GrammaticalStructure)x.Value))
                .ToDictionary(x => x.Key, x => x.Value);

            nonTerminals.ForEach(x => x.generator.BuildOnto(x.nonTerminal, grammaticalStructures));

            return new Language(tokenList, grammaticalStructures[startingGrammaticalStructure]);
        }
    }
}
