// <copyright file="MemberAccessExpression.cs" authors="Zentient Framework Team">
// Copyright © 2025 Zentient Framework Team. All rights reserved.
// </copyright>

using Zentient.Abstractions.Expressions;

namespace Zentient.Expressions
{
    /// <summary>
    /// Represents accessing a member on a target expression (for example: target.Member).
    /// </summary>
    internal sealed class MemberAccessExpression : ExpressionBase
    {
        /// <summary>
        /// Gets the target expression whose member is accessed.
        /// </summary>
        public IExpression Target { get; }

        /// <summary>
        /// Gets the member name being accessed.
        /// </summary>
        public string MemberName { get; }

        /// <summary>
        /// Initializes a new instance of <see cref="MemberAccessExpression"/>.
        /// </summary>
        /// <param name="target">The target expression.</param>
        /// <param name="memberName">The member name.</param>
        public MemberAccessExpression(IExpression target, string memberName)
            => (Target, MemberName) = (target, memberName);

        /// <inheritdoc />
        public override ExpressionKind Kind => ExpressionKind.MemberAccess;

        /// <inheritdoc />
        public override IReadOnlyList<IExpression> Operands => new[] { Target };

        /// <inheritdoc />
        public override string Canonical => $"{Target.Canonical}.{MemberName}";
    }
}
