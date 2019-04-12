using PaRRa;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PaRRa_Test
{
    class Program
    {
        static void Main(string[] args)
        {
            List<TokenType> tokenList = new List<TokenType>();

            Terminal identifier = new Terminal(tokenList, "Identifier", "[a-zA-Z_][a-zA-Z_0-9]*");
            Terminal number = new Terminal(tokenList, "Number", "\\d*\\.?\\d+");

            Terminal add = new Terminal(tokenList, "Add", "\\+");
            Terminal subtract = new Terminal(tokenList, "Subtract", "-");
            Terminal multiply = new Terminal(tokenList, "Multiply", "\\*");
            Terminal divide = new Terminal(tokenList, "Divide", "/");
            Terminal power = new Terminal(tokenList, "Power", "\\*\\*");

            Terminal increment = new Terminal(tokenList, "Increment", "#");
            Terminal decrement = new Terminal(tokenList, "Decrement", "=");

            Terminal equality = new Terminal(tokenList, "Equality", "is");

            Terminal allocator = new Terminal(tokenList, "Allocator", "pls");
            Terminal assign = new Terminal(tokenList, "Assign", "as");

            Terminal semicolon = new Terminal(tokenList, "Semicolon", ";");
            Terminal lbraces = new Terminal(tokenList, "LBraces", "{");
            Terminal rbraces = new Terminal(tokenList, "RBraces", "}");

            Terminal separator = new Terminal(tokenList, "separator", "and");
            Terminal lparens = new Terminal(tokenList, "LParens", "\\(");
            Terminal rparens = new Terminal(tokenList, "RParens", "\\)");


            Lexer lexer = new Lexer($"frick as 5*4124-412*(42-124*4231)**2-5**(12+4*2);", tokenList);
            List<(TokenType token, string text)> tokens = lexer.GetTokens().ToList();
            tokens.ForEach(x => Console.WriteLine(x.token.name + ": " + x.text));
            Console.WriteLine();



            GrammaticalPlaceholder PLACEHOLDER = new GrammaticalPlaceholder();

            GrammaticalStructure POINTER;

            GrammaticalStructure ___EXPRESSION = new GrammaticalStructure("Expression'''")
            {
                { "Assignment", nodes => nodes[2].Eval(),
                    identifier , assign, PLACEHOLDER },
                { "Number", nodes => Convert.ToDouble(nodes[0].tokens[0].text),
                    number },
                { "Parenthesis", nodes => nodes[1].Eval(),
                    lparens, PLACEHOLDER, rparens },
                { "Method", nodes => nodes[2].Eval(),
                    identifier, lparens, PLACEHOLDER, rparens },
            };
            GrammaticalStructure __EXPRESSION = new GrammaticalStructure("Expression''")
            {
                { "BinaryPower", nodes => Math.Pow(Convert.ToDouble(nodes[0].Eval()), Convert.ToDouble(nodes[2].Eval())),
                    ___EXPRESSION, power, null },
                { "Fallthrough", nodes => nodes[0].Eval(),
                    ___EXPRESSION },
            };

            GrammaticalStructure _EXPRESSION = new GrammaticalStructure("Expression'")
            {
                { "BinaryMultiplication", nodes => Convert.ToDouble(nodes[0].Eval()) * Convert.ToDouble(nodes[2].Eval()),
                    __EXPRESSION, multiply, null },
                { "BinaryDivision", nodes => Convert.ToDouble(nodes[0].Eval()) / Convert.ToDouble(nodes[2].Eval()),
                    __EXPRESSION, divide, null },
                { "Fallthrough", nodes => nodes[0].Eval(),
                    __EXPRESSION },
            };

            GrammaticalStructure EXPRESSION = new GrammaticalStructure("Expression")
            {
                { "BinaryAddition", nodes => Convert.ToDouble(nodes[0].Eval()) + Convert.ToDouble(nodes[2].Eval()),
                    _EXPRESSION, add, null },
                { "BinarySubtraction", nodes => Convert.ToDouble(nodes[0].Eval()) - Convert.ToDouble(nodes[2].Eval()),
                    _EXPRESSION, subtract, null },
                { "UnaryPlus", nodes => Convert.ToDouble(nodes[1].Eval()),
                    add, null },
                { "UnaryMinus", nodes => -Convert.ToDouble(nodes[1].Eval()),
                    subtract, null },
                { "Brackets", nodes => nodes[1].Eval(),
                    lbraces, PLACEHOLDER, rbraces },
                {  "Fallthrough", nodes => nodes[0].Eval(),
                    _EXPRESSION },
            };
            ___EXPRESSION.Replace(PLACEHOLDER, EXPRESSION);

            GrammaticalStructure S = new GrammaticalStructure("Start")
            {
                { "ExpressionTerminal", nodes => nodes[0].Eval(),
                    EXPRESSION, semicolon },
                { "Expression", nodes => (nodes[0].Eval(), nodes[2].Eval()).Item1,
                    EXPRESSION, semicolon, null },
                { "BlockStatementTerminal", nodes =>
                    {
                        object output = null;
                        while (Convert.ToBoolean(nodes[0].Eval())) output = nodes[1].Eval();
                        return output;
                    },
                    EXPRESSION, EXPRESSION },
                { "BlockStatement", nodes =>
                    {
                        object output = null;
                        while (Convert.ToBoolean(nodes[0].Eval())) output = nodes[1].Eval();
                        nodes[2].Eval();
                        return output;
                    },
                    EXPRESSION, EXPRESSION, null },
            };
            EXPRESSION.Replace(PLACEHOLDER, S);

            Node node = new Node(S, tokens);
            node.rule = new ProductionRule("Start", nodes => nodes[0].Eval());
            node.Compile();

            char indenter = '|';
            string PrintNode(Node nodeToPrint, string indent = "") =>
                nodeToPrint.IsTerminal
                ? indent + "" + nodeToPrint.grammaticalStructure.name + "\n" +
                  indent + "" + nodeToPrint.tokens.First().token.name + ": " + nodeToPrint.tokens.First().text                  
                : indent + "" + nodeToPrint.grammaticalStructure.name + "\n" + 
                  string.Join("\n", nodeToPrint.tree.Select(x => PrintNode(x, indent + (x.tree?.Length < 2 ? ' ' : '|'))).ToArray());
            Console.WriteLine(PrintNode(node));

            Console.WriteLine(node.Eval());

            Console.ReadLine();
        }
    }
}
