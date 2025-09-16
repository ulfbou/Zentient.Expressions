// <copyright file="ExpressionParser.cs" authors="Zentient Framework Team">
// Copyright © 2025 Zentient Framework Team. All rights reserved.
// </copyright>

using Zentient.Abstractions.Expressions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Zentient.Expressions
{
    /// <summary>
    /// Internal parsing helpers that produce an <see cref="IExpression"/> and diagnostics.
    /// </summary>
    internal static class ExpressionParser
    {
        /// <summary>
        /// Attempts to parse <paramref name="input"/> into an <see cref="IExpression"/> and collects diagnostics.
        /// </summary>
        /// <param name="input">The input expression text to parse.</param>
        /// <param name="expression">When this method returns, contains the parsed expression if parsing succeeded; otherwise <c>null</c>.</param>
        /// <param name="diagnostics">When this method returns, contains a read-only list of diagnostics observed during parsing.</param>
        /// <returns><c>true</c> when parsing succeeded with no diagnostics; otherwise <c>false</c>.</returns>
        public static bool TryParse(
            string input,
            out IExpression? expression,
            out IReadOnlyList<ParseDiagnostic> diagnostics)
        {
            var diags = new List<ParseDiagnostic>();

            if (string.IsNullOrWhiteSpace(input))
            {
                diags.Add(new ParseDiagnostic(0, "Expression is empty or whitespace."));
                expression = null;
                diagnostics = diags;
                return false;
            }

            var parser = new Parser(input);
            var expr = parser.ParseExpression();
            diags.AddRange(parser.Diagnostics);

            diagnostics = diags;

            if (expr is null || diags.Count > 0)
            {
                expression = expr;
                return false;
            }

            ExpressionRegistry.RaiseParsed(expr);
            expression = expr;
            diagnostics = diags;
            return true;
        }

        /// <summary>
        /// Parses the specified input and returns an <see cref="IExpression"/>.
        /// </summary>
        /// <param name="input">The input expression text to parse.</param>
        /// <returns>The parsed expression.</returns>
        /// <exception cref="ArgumentException">Thrown when parsing fails. The exception message contains concatenated diagnostic messages.</exception>
        public static IExpression Parse(string input)
        {
            if (!TryParse(input, out var expr, out var diags))
            {
                var message = string.Join("; ", diags.Select(d => d.Message));
                throw new ArgumentException($"Failed to parse expression: {message}");
            }

            return expr!;
        }
    }
}
