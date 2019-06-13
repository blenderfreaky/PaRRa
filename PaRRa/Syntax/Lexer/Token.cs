using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace PaRRa.Syntax.Lexer
{
    public struct Token
    {
        public TokenType tokenType;
        public string text;
        public bool keyword;

        public Token(TokenType tokenType, string text, bool keyword)
        {
            this.tokenType = tokenType;
            this.text = text;
            this.keyword = keyword;
        }

        public static IEnumerable<Token> GetTokens(string text, ICollection<TokenType> tokenTypes)
        {
            IOrderedEnumerable<TokenType> tokenTypesOrdered = tokenTypes.OrderByDescending(x => x.precedence);

            for (int i = 0; i < text.Length;)
            {
                //Dictionary<TokenType, (int length, string text)> matches = new Dictionary<TokenType, (int length, string text)>();

                foreach (TokenType tokenType in tokenTypesOrdered)
                {
                    Match match = tokenType.regex.Match(text, i);
                    if (match.Success)
                    {
                        yield return new Token(tokenType, match.Value, tokenType.keywords?.Any(x => x.Equals(match.Value)) ?? false);
                        i += match.Value.Length;
                        break;
                    }
                }

                /*if (matches.Count == 0) { i++; continue; }
                KeyValuePair<TokenType, (int length, string text)> max = matches.First();
                foreach (KeyValuePair<TokenType, (int length, string text)> match in matches)
                {
                    if (match.Value.length >= max.Value.length) max = match;
                }
                i += max.Value.length;
                yield return new Token(max.Key, max.Value.text);*/
            }
        }

        public override string ToString() => $"{tokenType.name}: {text}";
    }
}
