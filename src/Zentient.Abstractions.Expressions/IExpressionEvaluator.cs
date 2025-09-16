// <copyright file="IExpressionEvaluator.cs" authors="Zentient Framework Team">
// Copyright © 2025 Zentient Framework Team. All rights reserved.
// </copyright>

namespace Zentient.Abstractions.Expressions
{
    /// <summary>
    /// Evaluates expressions to obtain their numeric value.
    /// </summary>
    public interface IExpressionEvaluator
    {
        /// <summary>
        /// Evaluates the specified expression in the given context.
        /// </summary>
        /// <param name="expression">The expression to evaluate.</param>
        /// <param name="context">The context in which to evaluate the expression, or <c>null</c> for no context.</param>
        /// <returns>The numeric value of the expression.</returns>
        object? Evaluate(IExpression expression, object? context = null);
    }
}
