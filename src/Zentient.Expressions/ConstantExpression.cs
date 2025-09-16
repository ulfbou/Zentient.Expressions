// <copyright file="ConstantExpression.cs" authors="Zentient Framework Team">
// Copyright © 2025 Zentient Framework Team. All rights reserved.
// </copyright>

using System.Globalization;

using Zentient.Abstractions.Expressions;

namespace Zentient.Expressions
{
    /// <summary>
    /// Represents a literal constant expression.
    /// </summary>
    internal sealed class ConstantExpression : ExpressionBase
    {
        /// <summary>
        /// Gets the constant value represented by this node.
        /// </summary>
        public object? Value { get; }

        /// <summary>
        /// Creates a new <see cref="ConstantExpression"/> with the given value.
        /// </summary>
        /// <param name="value">The constant value (may be <c>null</c>).</param>
        public ConstantExpression(object? value) => Value = value;

        /// <inheritdoc />
        public override ExpressionKind Kind => ExpressionKind.Constant;

        /// <inheritdoc />
        public override IReadOnlyList<IExpression> Operands => Array.Empty<IExpression>();

        /// <summary>
        /// Produces a canonical string representation of the value.
        /// Strings are quoted and escape sequences are applied; <c>null</c> renders as "null".
        /// </summary>
        public override string Canonical
            => Value switch
            {
                string s => $"\"{EscapeString(s)}\"",
                null => "null",
                _ => Convert.ToString(Value, CultureInfo.InvariantCulture) ?? "null"
            };

        // Escape backslash first, then other characters
        private static string EscapeString(string s)
        {
            if (string.IsNullOrEmpty(s)) return s;
            var sb = new System.Text.StringBuilder();
            foreach (var ch in s)
            {
                switch (ch)
                {
                    case '\\': sb.Append("\\\\"); break; // backslash => \\\\ in string literal
                    case '"': sb.Append("\\\""); break;   // quote => \"
                    case '\n': sb.Append("\\n"); break;
                    case '\r': sb.Append("\\r"); break;
                    case '\t': sb.Append("\\t"); break;
                    default:
                        sb.Append(ch);
                        break;
                }
            }
            return sb.ToString();
        }
    }
}
