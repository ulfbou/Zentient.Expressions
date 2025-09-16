// <copyright file="StubTypedExpression.cs" authors="Zentient Framework Team">
// Copyright © 2025 Zentient Framework Team. All rights reserved.
// </copyright>

using Zentient.Abstractions.Expressions;

namespace Zentient.Expressions
{
    /// <summary>
    /// A lightweight wrapper implementing <see cref="ITypedExpression{T}"/> around an existing <see cref="IExpression"/>.
    /// The evaluation uses the internal <see cref="StubEvaluator"/> and not a full evaluator.
    /// </summary>
    /// <typeparam name="T">The target CLR type for evaluation.</typeparam>
    internal sealed class StubTypedExpression<T> : ITypedExpression<T>
    {
        private readonly IExpression _inner;

        /// <summary>
        /// Initializes a new instance that wraps <paramref name="inner"/>.
        /// </summary>
        /// <param name="inner">The inner expression to evaluate.</param>
        public StubTypedExpression(IExpression inner) => _inner = inner;

        /// <inheritdoc />
        public ExpressionKind Kind => _inner.Kind;

        /// <inheritdoc />
        public string Canonical => _inner.Canonical;

        /// <inheritdoc />
        public IReadOnlyList<IExpression> Operands => _inner.Operands;

        /// <summary>
        /// Evaluates the wrapped expression using the internal stub evaluator and returns a value of type <typeparamref name="T"/>.
        /// </summary>
        /// <param name="context">An optional context object passed to the evaluator.</param>
        /// <returns>The evaluated value converted to <typeparamref name="T"/> when possible; otherwise the default of <typeparamref name="T"/>.</returns>
        public T Evaluate(object? context = null)
        {
            var result = StubEvaluator.Evaluate(_inner, context);
            ExpressionRegistry.RaiseEvaluated(_inner, result);
            return result is T t ? t : default!;
        }
    }
}
