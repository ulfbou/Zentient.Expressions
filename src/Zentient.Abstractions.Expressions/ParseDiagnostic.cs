// <copyright file="ParseDiagnostic.cs" authors="Zentient Framework Team">
// Copyright © 2025 Zentient Framework Team. All rights reserved.
// </copyright>

namespace Zentient.Abstractions.Expressions
{
    /// <summary>
    /// Severity for parse diagnostics.
    /// </summary>
    public enum ParseDiagnosticSeverity
    {
        Error,
        Warning,
        Information
    }

    /// <summary>
    /// Represents a diagnostic message produced during parsing.
    /// </summary>
    /// <param name="Position">The position in the input where the diagnostic occurred.</param>
    /// <param name="Message">The diagnostic message.</param>
    /// <param name="Severity">The diagnostic severity (default: Error).</param>
    public record ParseDiagnostic(int Position, string Message, ParseDiagnosticSeverity Severity = ParseDiagnosticSeverity.Error);
}
