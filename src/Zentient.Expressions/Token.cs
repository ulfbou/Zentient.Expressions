// <copyright file="Token.cs" authors="Zentient Framework Team">
// Copyright © 2025 Zentient Framework Team. All rights reserved.
// </copyright>

namespace Zentient.Expressions
{
    /// <summary>
    /// A single lexical token with its type, textual content and start position.
    /// </summary>
    internal readonly struct Token
    {
        /// <summary>Gets the token kind.</summary>
        public TokenType Type { get; }

        /// <summary>
        /// Gets the exact text for the token as it appeared in the source.
        /// For string tokens this value is the unquoted content.
        /// </summary>
        public string Text { get; }

        /// <summary>
        /// Gets the zero-based character index in the source where the token begins.
        /// </summary>
        public int Position { get; }

        /// <summary>
        /// For string tokens, indicates whether the closing quote was found.
        /// For other tokens this is always true.
        /// </summary>
        public bool IsComplete { get; }

        /// <summary>
        /// Initializes a new instance of <see cref="Token"/>.
        /// </summary>
        /// <param name="type">The token kind.</param>
        /// <param name="text">The token text.</param>
        /// <param name="position">Zero-based start position within the source.</param>
        /// <param name="isComplete">For string tokens, whether the closing quote was found. Defaults to true.</param>
        public Token(TokenType type, string text, int position, bool isComplete = true)
            => (Type, Text, Position, IsComplete) = (type, text, position, isComplete);
    }
}
