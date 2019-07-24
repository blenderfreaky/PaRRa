using PaRRa.Dynamic;
using PaRRa.Parser;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PaRRa_Test
{
    class Program
    {
        static void Main(string[] args)
        {
            var languageGenerator = new LanguageGenerator()
            .AddTokenType("identifier", "[a-zA-Z_][a-zA-Z_0-9]*")
            .AddTokenType("number", "\\d*\\.?\\d+")

            .AddTokenType("add", "\\+")
            .AddTokenType("subtract", "-")
            .AddTokenType("multiply", "\\*")
            .AddTokenType("divide", "/")
            .AddTokenType("power", "\\*\\*")

            .AddTokenType("increment", "#")
            .AddTokenType("decrement", "=")

            .AddTokenType("equality", "is")

            .AddTokenType("allocator", "pls")
            .AddTokenType("assign", "as")

            .AddTokenType("semicolon", ";")
            .AddTokenType("lbraces", "{")
            .AddTokenType("rbraces", "}")

            .AddTokenType("separator", "and")
            .AddTokenType("lparens", "\\(")
            .AddTokenType("rparens", "\\)");

            languageGenerator.AddGrammaticalStructure("Expression'''", structure => structure
                .AddProductionRule("Assignment", rule => rule
                    .SetDecomposition("identifier", "assign", "Expression")
                    .SetEval(nodes => nodes[2].Eval())
                )
                .AddProductionRule("Number", rule => rule
                    .SetDecomposition("number")
                    .SetEval(nodes => Convert.ToDouble(nodes[0].tokens[0].text))
                )
                .AddProductionRule("Parenthesis", rule => rule
                    .SetDecomposition("lparens", "Expression", "rparens")
                    .SetEval(nodes => nodes[1].Eval())
                )
                .AddProductionRule("Method", rule => rule
                    .SetDecomposition("identifier", "lparens", "Expression", "rparens")
                    .SetEval(nodes => nodes[2].Eval())
                )
            );


            languageGenerator.AddGrammaticalStructure("Expression''", structure => structure
                .AddProductionRule("BinaryPower", rule => rule
                     .SetDecomposition("Expression'''", "power", "Expression''")
                     .SetEval(nodes => Math.Pow(Convert.ToDouble(nodes[0].Eval()), Convert.ToDouble(nodes[2].Eval())))
                )
                .AddProductionRule("Fallthrough", rule => rule
                     .SetDecomposition("Expression'''")
                     .SetEval(nodes => nodes[0].Eval())
                )
            );


            languageGenerator.AddGrammaticalStructure("Expression'", structure => structure
                .AddProductionRule("BinaryMultiplication", rule => rule
                    .SetDecomposition("Expression''", "multiply", "Expression'")
                    .SetEval(nodes => Convert.ToDouble(nodes[0].Eval()) * Convert.ToDouble(nodes[2].Eval()))
                )
                .AddProductionRule("BinaryDivision", rule => rule
                    .SetDecomposition("Expression''", "divide", "Expression'")
                    .SetEval(nodes => Convert.ToDouble(nodes[0].Eval()) / Convert.ToDouble(nodes[2].Eval()))
                )
                .AddProductionRule("Fallthrough", rule => rule
                    .SetDecomposition("Expression''")
                    .SetEval(nodes => nodes[0].Eval())
                )
            );


            languageGenerator.AddGrammaticalStructure("Expression", structure => structure
                .AddProductionRule("BinaryAddition", rule => rule
                    .SetDecomposition("Expression'", "add", "Expression")
                    .SetEval(nodes => Convert.ToDouble(nodes[0].Eval()) + Convert.ToDouble(nodes[2].Eval()))
                )
                .AddProductionRule("BinarySubtraction", rule => rule
                    .SetDecomposition("Expression'", "subtract", "Expression")
                    .SetEval(nodes => Convert.ToDouble(nodes[0].Eval()) - Convert.ToDouble(nodes[2].Eval()))
                )
                .AddProductionRule("UnaryPlus", rule => rule
                    .SetDecomposition("add", "Expression")
                    .SetEval(nodes => Convert.ToDouble(nodes[1].Eval()))
                )
                .AddProductionRule("UnaryMinus", rule => rule
                    .SetDecomposition("subtract", "Expression")
                    .SetEval(nodes => -Convert.ToDouble(nodes[1].Eval()))
                )
                .AddProductionRule("Brackets", rule => rule
                    .SetDecomposition("lbraces", "Start", "rbraces")
                    .SetEval(nodes => nodes[1].Eval())
                )
                .AddProductionRule("Fallthrough", rule => rule
                    .SetDecomposition("Expression'")
                    .SetEval(nodes => nodes[0].Eval())
                )
            );


            languageGenerator.AddGrammaticalStructure("Start", structure => structure
                .AddProductionRule("ExpressionTerminal", rule => rule
                    .SetDecomposition("Expression", "semicolon")
                    .SetEval(nodes => nodes[0].Eval())
                )
                .AddProductionRule("Expression", rule => rule
                    .SetDecomposition("Expression", "semicolon", "Start")
                    .SetEval(nodes => (nodes[0].Eval(), nodes[2].Eval()).Item1)
                )
                .AddProductionRule("BlockStatementTerminal", rule => rule
                    .SetDecomposition("Expression", "Expression")
                    .SetEval(nodes =>
                    {
                        object output = null;
                        while (Convert.ToBoolean(nodes[0].Eval())) output = nodes[1].Eval();
                        return output;
                    })
                )
                .AddProductionRule("BlockStatement", rule => rule
                    .SetDecomposition("Expression", "Expression", "Start")
                    .SetEval(nodes =>
                    {
                        object output = null;
                        while (Convert.ToBoolean(nodes[0].Eval())) output = nodes[1].Eval();
                        nodes[2].Eval();
                        return output;
                    })
                )
            );

            languageGenerator.SetStartingGrammaticalStructure("Start");

            Language language = languageGenerator.Build();

            var parseTree = language.Parse("132+232/32*(123+42)-32**2;");
            var values = PrintParseTree(parseTree, "\t").ToArray();
            Console.WriteLine(string.Join(Environment.NewLine, values));
            //Console.WriteLine(parseTree.Eval());
        }

        public static IEnumerable<string> PrintParseTree(ParseTreeNode node, string indenter) => node.grammaticalStructure is Terminal
            ? new string[] { $"{node.tokens[0].tokenType.name}: {node.tokens[0].text}" }
            :
        new string[] { node.grammaticalStructure.name + ": " + node.rule?.name ?? "TERMINAL" }
        .Concat(node.tree == null
            ? Enumerable.Empty<string>()
            : node.tree?.SelectMany(x => PrintParseTree(x, indenter)).Select(x => indenter + x));
    }
}
