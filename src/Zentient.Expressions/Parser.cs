// <copyright file="Parser.cs" authors="Zentient Framework Team">
// Copyright © 2025 Zentient Framework Team. All rights reserved.
// </copyright>

using System.Globalization;

using Zentient.Abstractions.Expressions;

namespace Zentient.Expressions
{
    /// <summary>
    /// Recursive-descent parser that converts a token stream into an <see cref="IExpression"/> AST and collects diagnostics.
    /// </summary>
    internal sealed class Parser
    {
        private readonly List<Token> tokens = new();
        private int idx;
        private readonly List<ParseDiagnostic> diagnostics = new();

        /// <summary>
        /// Gets the diagnostics produced while parsing. The collection may be empty when parsing succeeds.
        /// </summary>
        public IReadOnlyList<ParseDiagnostic> Diagnostics => diagnostics;

        /// <summary>
        /// Initializes a new instance of the <see cref="Parser"/> that tokenizes the provided source string.
        /// </summary>
        /// <param name="src">The expression source text to parse.</param>
        public Parser(string src)
        {
            var lexer = new Lexer(src);
            Token tok;
            do
            {
                tok = lexer.Next();
                tokens.Add(tok);
            } while (tok.Type != TokenType.End);
        }

        /// <summary>
        /// Parses an expression from the current token stream. When parsing fails, diagnostics are recorded.
        /// </summary>
        /// <returns>The parsed <see cref="IExpression"/> when successful; otherwise <c>null</c>.</returns>
        public IExpression? ParseExpression()
        {
            if (Peek().Type == TokenType.End)
            {
                diagnostics.Add(new ParseDiagnostic(0, "Empty expression"));
                return null;
            }

            var expr = ParseLambdaOrMemberOrCall();

            if (Peek().Type != TokenType.End)
            {
                var tok = Peek();
                diagnostics.Add(new ParseDiagnostic(tok.Position, $"Unexpected token '{tok.Text}'"));
            }

            return expr;
        }

        /// <summary>
        /// Parses either a lambda expression, a member access chain, or a method call starting at the current token.
        /// Handles primary expressions (identifiers, numbers, strings) and folds trailing member/access or invocation.
        /// </summary>
        /// <returns>The parsed expression or <c>null</c> on error.</returns>
        private IExpression? ParseLambdaOrMemberOrCall()
        {
            if (IsIdentifierList() && PeekNext().Type == TokenType.Arrow)
                return ParseLambda();

            IExpression? expr = Peek().Type switch
            {
                TokenType.Identifier => new IdentifierExpression(Consume().Text),
                TokenType.Number => TryParseNumber(),
                TokenType.String => TryParseString(),
                _ => null
            };

            if (expr is null)
            {
                var tok = Peek();
                diagnostics.Add(new ParseDiagnostic(tok.Position, $"Unexpected token '{tok.Text}'"));
                return null;
            }

            while (Peek().Type == TokenType.Dot)
            {
                Consume(); // '.'
                if (Peek().Type != TokenType.Identifier)
                {
                    var err = Peek();
                    diagnostics.Add(new ParseDiagnostic(err.Position, "Identifier expected after '.'"));
                    break;
                }

                var name = Consume().Text;
                expr = Peek().Type == TokenType.LParen
                    ? ParseMethodCall(expr, name)
                    : new MemberAccessExpression(expr, name);
            }

            return expr;
        }

        /// <summary>
        /// Determines whether the token sequence beginning at the current index represents
        /// a comma-separated identifier list suitable for lambda parameters (e.g. "x, y =>").
        /// </summary>
        /// <returns><c>true</c> when a comma-separated identifier list is followed by an arrow token.</returns>
        private bool IsIdentifierList()
        {
            int i = idx;
            if (tokens[i].Type != TokenType.Identifier) return false;
            i++;
            while (i < tokens.Count && tokens[i].Type == TokenType.Comma)
                i += 2;
            return i < tokens.Count && tokens[i].Type == TokenType.Arrow;
        }

        /// <summary>
        /// Parses a lambda expression in the form "param, ... => body".
        /// Parameters are consumed from the token stream and the body is parsed.
        /// </summary>
        /// <returns>A <see cref="LambdaExpression"/> representing the parsed lambda.</returns>
        private IExpression? ParseLambda()
        {
            var parameters = new List<string>();
            do
            {
                parameters.Add(Consume().Text);
            } while (Peek().Type == TokenType.Comma && Consume().Type == TokenType.Comma);

            Consume(); // Arrow
            var body = ParseLambdaOrMemberOrCall() ?? new ConstantExpression(null);
            return new LambdaExpression(parameters, body);
        }

        /// <summary>
        /// Parses a method call expression given a previously parsed target and method name.
        /// Expects to be positioned at the opening '(' when called.
        /// </summary>
        /// <param name="target">The target expression on which the method is invoked.</param>
        /// <param name="name">The method name.</param>
        /// <returns>A <see cref="MethodCallExpression"/> representing the invocation.</returns>
        private MethodCallExpression ParseMethodCall(IExpression target, string name)
        {
            Consume(); // LParen
            var args = new List<IExpression>();
            if (Peek().Type != TokenType.RParen)
            {
                do
                {
                    var arg = ParseLambdaOrMemberOrCall();
                    if (arg != null) args.Add(arg);
                } while (Peek().Type == TokenType.Comma && Consume().Type == TokenType.Comma);
            }
            Consume(); // RParen
            return new MethodCallExpression(target, name, args);
        }

        /// <summary>
        /// Attempts to parse the current numeric token into a numeric constant expression using invariant culture.
        /// When parsing fails a diagnostic is recorded and a <see cref="ConstantExpression"/> with a null value is returned.
        /// </summary>
        /// <returns>An <see cref="IExpression"/> representing the numeric constant (or null constant on error).</returns>
        private IExpression TryParseNumber()
        {
            var tok = Consume();
            if (double.TryParse(tok.Text, NumberStyles.Any, CultureInfo.InvariantCulture, out var d))
                return new ConstantExpression(d);

            diagnostics.Add(new ParseDiagnostic(tok.Position, $"Invalid number '{tok.Text}'"));
            return new ConstantExpression(null);
        }

        /// <summary>
        /// Consumes the current string token and returns a <see cref="ConstantExpression"/> containing its text.
        /// </summary>
        /// <returns>An <see cref="IExpression"/> representing the string constant.</returns>
        private IExpression TryParseString()
        {
            var tok = Consume();
            if (!tok.IsComplete)
            {
                diagnostics.Add(new ParseDiagnostic(tok.Position, "Unterminated string literal"));
                return new ConstantExpression(null);
            }

            return new ConstantExpression(tok.Text);
        }

        /// <summary>
        /// Returns the token at the current parser index without consuming it.
        /// </summary>
        /// <returns>The current <see cref="Token"/>.</returns>
        private Token Peek() => tokens[idx];

        /// <summary>
        /// Returns the token immediately after the current parser index without consuming it.
        /// If the lookahead is out of range the current token is returned.
        /// </summary>
        /// <returns>The next <see cref="Token"/> or the current token if lookahead is not available.</returns>
        private Token PeekNext() => idx + 1 < tokens.Count ? tokens[idx + 1] : Peek();

        /// <summary>
        /// Consumes and returns the token at the current index, advancing the parser position by one.
        /// </summary>
        /// <returns>The consumed <see cref="Token"/>.</returns>
        private Token Consume() => tokens[idx++];
    }
}
