// <copyright file="IExpressionParser.cs" authors="Zentient Framework Team">
// Copyright © 2025 Zentient Framework Team. All rights reserved.
// </copyright>

namespace Zentient.Abstractions.Expressions
{
    /// <summary>
    /// Parses expressions from strings.
    /// </summary>
    public interface IExpressionParser
    {
        /// <summary>
        /// Attempts to parse the input string as an expression.
        /// </summary>
        /// <param name="input">The input string.</param>
        /// <param name="expression">When this method returns, contains the parsed expression if the parse was successful, or <c>null</c> otherwise.</param>
        /// <param name="diagnostics">When this method returns, contains the list of diagnostics produced during parsing.</param>
        /// <returns><c>true</c> if the parse was successful; otherwise, <c>false</c>.</returns>
        bool TryParse(string input, out IExpression? expression, out IReadOnlyList<ParseDiagnostic> diagnostics);

        /// <summary>
        /// Parses the input string as an expression.
        /// </summary>
        /// <param name="input">The input string.</param>
        /// <returns>The parsed expression.</returns>
        /// <exception cref="ParseException">Thrown if the input string cannot be parsed as an expression.</exception>
        IExpression Parse(string input);
    }
}
