using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace PaRRa
{
    public static class Lexer
    {
        public static IEnumerable<Token> GetTokens(string text, ICollection<TokenType> tokenTypes)
        {
            for (int i = 0; i < text.Length;)
            {
                Dictionary<TokenType, (int length, string text)> matches = new Dictionary<TokenType, (int length, string text)>();

                foreach (TokenType tokenType in tokenTypes)
                {
                    Match match = tokenType.regex.Match(text, i);
                    if (match.Success)
                    {
                        matches[tokenType] = (match.Length, match.Value);
                    }
                }

                if (matches.Count == 0) { i++; continue; }
                KeyValuePair<TokenType, (int length, string text)> max = matches.First();
                foreach (KeyValuePair<TokenType, (int length, string text)> match in matches)
                {
                    if (match.Value.length >= max.Value.length) max = match;
                }
                i += max.Value.length;
                yield return new Token(max.Key, max.Value.text);
            }
        }
    }
}
