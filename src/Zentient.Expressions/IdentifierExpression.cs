// <copyright file="IdentifierExpression.cs" authors="Zentient Framework Team">
// Copyright © 2025 Zentient Framework Team. All rights reserved.
// </copyright>

using Zentient.Abstractions.Expressions;

namespace Zentient.Expressions
{
    /// <summary>
    /// Represents an identifier (variable or parameter name).
    /// </summary>
    internal sealed class IdentifierExpression : ExpressionBase
    {
        /// <summary>
        /// Gets the identifier name.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Initializes a new instance of <see cref="IdentifierExpression"/>.
        /// </summary>
        /// <param name="name">The identifier name.</param>
        public IdentifierExpression(string name) => Name = name;

        /// <inheritdoc />
        public override ExpressionKind Kind => ExpressionKind.Identifier;

        /// <inheritdoc />
        public override IReadOnlyList<IExpression> Operands => Array.Empty<IExpression>();

        /// <inheritdoc />
        public override string Canonical => Name;
    }
}
