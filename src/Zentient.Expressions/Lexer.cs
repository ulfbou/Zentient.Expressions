// <copyright file="Lexer.cs" authors="Zentient Framework Team">
// Copyright © 2025 Zentient Framework Team. All rights reserved.
// </copyright>

namespace Zentient.Expressions
{
    /// <summary>
    /// Simple lexer that converts an input source string into a stream of <see cref="Token"/> instances.
    /// </summary>
    internal sealed class Lexer
    {
        private readonly string src;
        private int pos;

        /// <summary>
        /// Initializes a new instance of the <see cref="Lexer"/> for the specified source.
        /// </summary>
        /// <param name="src">The input text to tokenize.</param>
        public Lexer(string src) => this.src = src;

        /// <summary>
        /// Reads and returns the next token from the input. Whitespace is skipped.
        /// Returns a <see cref="TokenType.End"/> token when the end of input is reached.
        /// </summary>
        /// <returns>The next <see cref="Token"/> read from the input stream.</returns>
        public Token Next()
        {
            SkipWhitespace();
            if (pos >= src.Length) return new Token(TokenType.End, "", pos);

            int start = pos;
            char c = src[pos];

            if (char.IsLetter(c) || c == '_')
                return ReadIdentifier(start);

            if (char.IsDigit(c))
                return ReadNumber(start);

            if (c == '"')
                return ReadString(start);

            // handle multi-character token '=>' first
            if (c == '=' && pos + 1 < src.Length && src[pos + 1] == '>')
            {
                pos += 2;
                return new Token(TokenType.Arrow, "=>", start);
            }

            // single-character punctuation
            pos++;
            return c switch
            {
                '.' => new Token(TokenType.Dot, ".", start),
                ',' => new Token(TokenType.Comma, ",", start),
                '(' => new Token(TokenType.LParen, "(", start),
                ')' => new Token(TokenType.RParen, ")", start),
                _ => new Token(TokenType.End, "", start)
            };
        }

        /// <summary>
        /// Reads an identifier token beginning at <paramref name="start"/>.
        /// Identifiers may contain letters, digits and underscore.
        /// </summary>
        /// <param name="start">Start index in the source where the identifier begins.</param>
        /// <returns>A token of type <see cref="TokenType.Identifier"/>.</returns>
        private Token ReadIdentifier(int start)
        {
            while (pos < src.Length && (char.IsLetterOrDigit(src[pos]) || src[pos] == '_'))
                pos++;
            string text = src[start..pos];
            return new Token(TokenType.Identifier, text, start);
        }

        /// <summary>
        /// Reads a numeric token (digits and optional decimal point).
        /// The lexer performs only lexical recognition; numeric validation occurs during parsing.
        /// </summary>
        /// <param name="start">Start index in the source where the number begins.</param>
        /// <returns>A token of type <see cref="TokenType.Number"/>.</returns>
        private Token ReadNumber(int start)
        {
            while (pos < src.Length && (char.IsDigit(src[pos]) || src[pos] == '.'))
                pos++;
            string text = src[start..pos];
            return new Token(TokenType.Number, text, start);
        }

        /// <summary>
        /// Reads a double-quoted string literal and returns its unquoted content as the token text.
        /// Supports common escape sequences: \\, \" , \n, \r, \t. Reports unterminated strings by returning a token with IsComplete=false.
        /// </summary>
        /// <param name="start">Start index pointing at the opening quote.</param>
        /// <returns>A token of type <see cref="TokenType.String"/> containing the unquoted string content.</returns>
        private Token ReadString(int start)
        {
            pos++; // skip opening "
            var sb = new System.Text.StringBuilder();
            bool terminated = false;
            while (pos < src.Length)
            {
                char ch = src[pos];
                if (ch == '"')
                {
                    terminated = true;
                    pos++; // consume closing quote
                    break;
                }

                if (ch == '\\' && pos + 1 < src.Length)
                {
                    // handle escape sequences
                    char next = src[pos + 1];
                    switch (next)
                    {
                        case '\\': sb.Append('\\'); break;
                        case '"': sb.Append('"'); break;
                        case 'n': sb.Append('\n'); break;
                        case 'r': sb.Append('\r'); break;
                        case 't': sb.Append('\t'); break;
                        default:
                            // unknown escape: preserve the character as-is
                            sb.Append(next);
                            break;
                    }
                    pos += 2;
                    continue;
                }

                sb.Append(ch);
                pos++;
            }

            // If not terminated, capture the remainder as text and mark incomplete
            if (!terminated)
            {
                string textRem = src.Substring(start + 1);
                pos = src.Length;
                return new Token(TokenType.String, textRem, start, isComplete: false);
            }

            return new Token(TokenType.String, sb.ToString(), start, isComplete: true);
        }

        /// <summary>
        /// Peeks one character ahead of the current position, returning '\0' when at or past the end.
        /// Used for recognizing multi-character tokens like "=>".
        /// </summary>
        /// <returns>The next character or '\0' if none.</returns>
        private char Peek() => pos + 1 < src.Length ? src[pos + 1] : '\0';

        /// <summary>
        /// Advances the internal cursor past any contiguous whitespace characters.
        /// </summary>
        private void SkipWhitespace()
        {
            while (pos < src.Length && char.IsWhiteSpace(src[pos]))
                pos++;
        }
    }
}
