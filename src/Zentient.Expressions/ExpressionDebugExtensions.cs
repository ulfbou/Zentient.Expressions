// <copyright file="ExpressionDebugExtensions.cs" authors="Zentient Framework Team">
// Copyright © 2025 Zentient Framework Team. All rights reserved.
// </copyright>

using Zentient.Abstractions.Expressions;

namespace Zentient.Expressions
{
    /// <summary>
    /// Debugging and utility extensions for expressions.
    /// </summary>
    public static class ExpressionDebugExtensions
    {
        /// <summary>
        /// Returns the canonical string representation of the specified expression.
        /// </summary>
        /// <param name="expr">The expression to obtain the canonical representation for.</param>
        /// <returns>A canonical, language-independent string for <paramref name="expr"/>.</returns>
        public static string ToCanonicalString(this IExpression expr)
            => expr.Canonical;
    }
}
