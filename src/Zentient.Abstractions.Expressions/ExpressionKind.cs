// <copyright file="ExpressionKind.cs" authors="Zentient Framework Team">
// Copyright © 2025 Zentient Framework Team. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Globalization;

namespace Zentient.Abstractions.Expressions
{
    /// <summary>
    /// Specifies the kind of expression.
    /// </summary>
    public enum ExpressionKind
    {
        /// <summary>
        /// An expression representing a constant value.
        /// </summary>
        Constant,

        /// <summary>
        /// An expression representing an identifier.
        /// </summary>
        Identifier,

        /// <summary>
        /// An expression representing member access.
        /// </summary>
        MemberAccess,

        /// <summary>
        /// An expression representing a method call.
        /// </summary>
        MethodCall,

        /// <summary>
        /// An expression representing a lambda expression.
        /// </summary>
        Lambda
    }
}
