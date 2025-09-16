// <copyright file="ExpressionBase.cs" authors="Zentient Framework Team">
// Copyright © 2025 Zentient Framework Team. All rights reserved.
// </copyright>

using Zentient.Abstractions.Expressions;

namespace Zentient.Expressions
{
    /// <summary>
    /// Base implementation for expression AST nodes.
    /// </summary>
    internal abstract class ExpressionBase : IExpression
    {
        /// <summary>
        /// Gets the kind of this expression node.
        /// </summary>
        public abstract ExpressionKind Kind { get; }

        /// <summary>
        /// Gets a canonical, language-independent representation of this expression node.
        /// </summary>
        public abstract string Canonical { get; }

        /// <summary>
        /// Gets the child operands of this expression node. Leaf nodes typically return an empty collection.
        /// </summary>
        public virtual IReadOnlyList<IExpression> Operands => Array.Empty<IExpression>();
    }
}
