// <copyright file="Extensions.cs" authors="Zentient Framework Team">
// Copyright © 2025 Zentient Framework Team. All rights reserved.
// </copyright>

using System.Collections.Generic;
using Zentient.Abstractions.Expressions;

namespace Zentient.Extensions.Expressions
{
    /// <summary>
    /// Ergonomic extension methods for common expression workflows.
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// Parses and evaluates the provided expression text using the default registry parser and evaluator.
        /// Returns the evaluation result or throws when parsing fails.
        /// </summary>
        public static object? EvaluateExpression(this string expressionText, object? context = null)
        {
            if (expressionText is null) throw new System.ArgumentNullException(nameof(expressionText));

            var parser = Zentient.Expressions.ExpressionRegistry.DefaultParser;
            if (!parser.TryParse(expressionText, out var expr, out var diags))
            {
                var msg = string.Join(';', System.Linq.Enumerable.Select(diags, d => d.Message));
                throw new System.ArgumentException($"Failed to parse expression: {msg}");
            }

            return Zentient.Expressions.ExpressionRegistry.DefaultEvaluator.Evaluate(expr!, context);
        }

        /// <summary>
        /// Returns the canonical form of an expression.
        /// </summary>
        public static string ToCanonicalString(this IExpression expr)
            => expr?.Canonical ?? string.Empty;

        /// <summary>
        /// Returns a brief debug string describing the expression kind, operand count, and canonical form.
        /// </summary>
        public static string ToDebugString(this IExpression expr)
            => $"Kind={expr.Kind}; Operands={expr.Operands?.Count ?? 0}; Canonical={expr.Canonical}";
    }
}
