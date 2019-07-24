using System;

namespace Parra.Common
{
    // Use wrapper around int to prevent using an index accidentally
    public readonly struct TokenIndex : IEquatable<TokenIndex>
    {
        public TokenIndex(int value)
        {
            if (value < 0) throw new ArgumentException("TokenIndicies must be larger or equal to 0", nameof(value));

            Value = value;
        }

        public int Value { get; }

        public override bool Equals(object obj) => obj is TokenIndex tokenIndex && Equals(tokenIndex);
        public bool Equals(TokenIndex other) => Value == other.Value;
        public static bool operator ==(TokenIndex first, TokenIndex second) => first.Value == second.Value;
        public static bool operator !=(TokenIndex first, TokenIndex second) => first.Value != second.Value;

        public override int GetHashCode() => Value.GetHashCode();
        public override string ToString() => Value.ToString();

        public static explicit operator int(TokenIndex index) => index.Value;
        public static explicit operator TokenIndex(int index) => new TokenIndex(index);
    }
}