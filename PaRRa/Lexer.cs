using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace PaRRa
{
    public class Lexer
    {
        public string text;
        public List<TokenType> tokens;

        public Lexer(string text, List<TokenType> tokens)
        {
            this.text = text;
            this.tokens = tokens;
        }

        public IEnumerable<(TokenType token, string text)> GetTokens()
        {
            for (int i = 0; i < text.Length;)
            {
                Dictionary<TokenType, (int length, string text)> matches = new Dictionary<TokenType, (int length, string text)>();

                foreach (TokenType token in tokens)
                {
                    Match match = token.regex.Match(text, i);
                    if (match.Success)
                    {
                        matches[token] = (match.Length, match.Value);
                    }
                }

                if (matches.Count == 0) { i++; continue; }
                KeyValuePair<TokenType, (int length, string text)> max = matches.First();
                foreach (KeyValuePair<TokenType, (int length, string text)> match in matches)
                {
                    if (match.Value.length >= max.Value.length) max = match;
                }
                i += max.Value.length;
                yield return (max.Key, max.Value.text);
            }
        }
    }
}
