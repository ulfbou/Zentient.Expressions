// <copyright file="IExpression.cs" authors="Zentient Framework Team">
// Copyright © 2025 Zentient Framework Team. All rights reserved.
// </copyright>

namespace Zentient.Abstractions.Expressions
{
    /// <summary>
    /// Represents an expression in the Zentient abstraction.
    /// </summary>
    public interface IExpression
    {
        /// <summary>
        /// Gets the kind of the expression.
        /// </summary>
        ExpressionKind Kind { get; }

        /// <summary>
        /// Gets the canonical string representation of the expression.
        /// </summary>
        string Canonical { get; }

        /// <summary>
        /// Gets the operands of the expression.
        /// </summary>
        IReadOnlyList<IExpression> Operands { get; }
    }
}
