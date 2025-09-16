// <copyright file="LambdaExpression.cs" authors="Zentient Framework Team">
// Copyright © 2025 Zentient Framework Team. All rights reserved.
// </copyright>

using Zentient.Abstractions.Expressions;

namespace Zentient.Expressions
{
    /// <summary>
    /// Represents a lambda (anonymous function) expression with parameters and a body.
    /// </summary>
    internal sealed class LambdaExpression : ExpressionBase
    {
        /// <summary>
        /// Gets the lambda parameter names.
        /// </summary>
        public IReadOnlyList<string> Parameters { get; }

        /// <summary>
        /// Gets the lambda body expression.
        /// </summary>
        public IExpression Body { get; }

        /// <summary>
        /// Initializes a new instance of <see cref="LambdaExpression"/>.
        /// </summary>
        /// <param name="parameters">The parameter names used by the lambda.</param>
        /// <param name="body">The body expression.</param>
        public LambdaExpression(IEnumerable<string> parameters, IExpression body)
            => (Parameters, Body) = (parameters.ToArray(), body);

        /// <inheritdoc />
        public override ExpressionKind Kind => ExpressionKind.Lambda;

        /// <inheritdoc />
        public override IReadOnlyList<IExpression> Operands => new[] { Body };

        /// <inheritdoc />
        public override string Canonical
            => $"{string.Join(", ", Parameters)} => {Body.Canonical}";
    }
}
