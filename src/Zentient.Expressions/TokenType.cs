// <copyright file="TokenType.cs" authors="Zentient Framework Team">
// Copyright © 2025 Zentient Framework Team. All rights reserved.
// </copyright>

namespace Zentient.Expressions
{
    /// <summary>
    /// Represents the kinds of lexical tokens produced by the expression lexer.
    /// </summary>
    internal enum TokenType
    {
        Identifier,
        Number,
        String,
        Dot,
        Comma,
        LParen,
        RParen,
        Arrow,
        End
    }
}
