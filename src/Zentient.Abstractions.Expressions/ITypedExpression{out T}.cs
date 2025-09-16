// <copyright file="ITypedExpression{out T}.cs" authors="Zentient Framework Team">
// Copyright © 2025 Zentient Framework Team. All rights reserved.
// </copyright>

namespace Zentient.Abstractions.Expressions
{
    /// <summary>
    /// Represents a typed expression that can be evaluated to a value of type <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The type of the value that the expression evaluates to.</typeparam>
    public interface ITypedExpression<out T> : IExpression
    {
        /// <summary>
        /// Evaluates the expression in the specified context.
        /// </summary>
        /// <param name="context">The context in which to evaluate the expression, or <c>null</c> for no context.</param>
        /// <returns>The result of the evaluation.</returns>
        T Evaluate(object? context = null);
    }
}
