// <copyright file="MethodCallExpression.cs" authors="Zentient Framework Team">
// Copyright © 2025 Zentient Framework Team. All rights reserved.
// </copyright>

using Zentient.Abstractions.Expressions;

namespace Zentient.Expressions
{
    /// <summary>
    /// Represents a method call expression on a target with arguments.
    /// </summary>
    internal sealed class MethodCallExpression : ExpressionBase
    {
        /// <summary>
        /// Gets the target expression on which the method is invoked.
        /// </summary>
        public IExpression Target { get; }

        /// <summary>
        /// Gets the method name to invoke.
        /// </summary>
        public string MethodName { get; }

        /// <summary>
        /// Gets the call arguments.
        /// </summary>
        public IReadOnlyList<IExpression> Arguments { get; }

        /// <summary>
        /// Initializes a new instance of <see cref="MethodCallExpression"/>.
        /// </summary>
        /// <param name="target">The target expression.</param>
        /// <param name="methodName">The method name.</param>
        /// <param name="args">The arguments to the method.</param>
        public MethodCallExpression(
            IExpression target,
            string methodName,
            IEnumerable<IExpression> args)
        {
            Target = target;
            MethodName = methodName;
            Arguments = args.ToArray();
        }

        /// <inheritdoc />
        public override ExpressionKind Kind => ExpressionKind.MethodCall;

        /// <inheritdoc />
        public override IReadOnlyList<IExpression> Operands
            => new[] { Target }.Concat(Arguments).ToArray();

        /// <inheritdoc />
        public override string Canonical
            => $"{Target.Canonical}.{MethodName}({string.Join(", ", Arguments.Select(a => a.Canonical))})";
    }
}
