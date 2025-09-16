// Pseudocode plan:
// - Add XML documentation comments to all public types and members in this file.
// - For enum ExpressionKind: provide summary and describe each member.
// - For ParseDiagnostic record: document purpose and parameters, note position semantics.
// - For IExpression: document interface purpose and each property.
// - For ITypedExpression<T>: document generic contract and Evaluate method.
// - For IExpressionParser: document TryParse and Parse semantics and parameters.
// - For IExpressionEvaluator: document Evaluate method behavior and return value.
// - Preserve original code structure and behavior; only add comments.
// - Remove duplicate using directives for cleanliness but keep semantics unchanged.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Zentient.Abstractions.Expressions
{
    /// <summary>
    /// Specifies the different kinds of expressions supported by the parser and evaluator.
    /// </summary>
    public enum ExpressionKind
    {
        /// <summary>
        /// A literal constant value (numbers, strings, booleans, etc.).
        /// </summary>
        Constant,

        /// <summary>
        /// An identifier such as a variable or parameter name.
        /// </summary>
        Identifier,

        /// <summary>
        /// A member access expression (for example, object.Member).
        /// </summary>
        MemberAccess,

        /// <summary>
        /// A method or function call expression.
        /// </summary>
        MethodCall,

        /// <summary>
        /// A lambda expression (anonymous function).
        /// </summary>
        Lambda
    }

    /// <summary>
    /// Represents a diagnostic produced while parsing an expression.
    /// </summary>
    /// <param name="Position">Zero-based character position in the input where the diagnostic applies.</param>
    /// <param name="Message">A human-readable diagnostic message describing the issue.</param>
    public record ParseDiagnostic(int Position, string Message);

    /// <summary>
    /// Represents a parsed expression node in the expression model.
    /// </summary>
    public interface IExpression
    {
        /// <summary>
        /// Gets the kind of this expression node.
        /// </summary>
        ExpressionKind Kind { get; }

        /// <summary>
        /// Gets a canonical, language-independent string representation of the expression.
        /// Implementations should provide a stable representation suitable for comparison or debugging.
        /// </summary>
        string Canonical { get; }

        /// <summary>
        /// Gets the operands (child expressions) of this expression node.
        /// For leaf nodes this collection is typically empty.
        /// </summary>
        IReadOnlyList<IExpression> Operands { get; }
    }

    /// <summary>
    /// Represents an expression that can be evaluated to a strongly typed value.
    /// </summary>
    /// <typeparam name="T">The CLR type that the expression evaluates to.</typeparam>
    public interface ITypedExpression<T> : IExpression
    {
        /// <summary>
        /// Evaluates the expression against an optional evaluation context and returns a value of type <typeparamref name="T"/>.
        /// </summary>
        /// <param name="context">An optional context object (for example, a model or variable scope) used during evaluation.</param>
        /// <returns>The value of the expression converted to <typeparamref name="T"/>.</returns>
        T Evaluate(object? context = null);
    }

    /// <summary>
    /// Parses textual expressions into an expression model.
    /// </summary>
    public interface IExpressionParser
    {
        /// <summary>
        /// Attempts to parse the specified input string into an <see cref="IExpression"/>.
        /// </summary>
        /// <param name="input">The expression text to parse.</param>
        /// <param name="expression">When this method returns, contains the parsed expression if parsing succeeded; otherwise, <c>null</c>.</param>
        /// <param name="diagnostics">When this method returns, contains a read-only list of diagnostics produced while parsing. The list may be empty if parsing succeeded without issues.</param>
        /// <returns><c>true</c> if parsing succeeded and a valid <see cref="IExpression"/> was produced; otherwise, <c>false</c>.</returns>
        bool TryParse(string input, out IExpression? expression, out IReadOnlyList<ParseDiagnostic> diagnostics);

        /// <summary>
        /// Parses the specified input string into an <see cref="IExpression"/>, throwing an exception on fatal parse errors.
        /// </summary>
        /// <param name="input">The expression text to parse.</param>
        /// <returns>The parsed <see cref="IExpression"/>.</returns>
        /// <remarks>
        /// Implementations may throw an exception for unrecoverable parse errors; callers who want diagnostics without exceptions
        /// should use <see cref="TryParse(string,out IExpression?,out IReadOnlyList{ParseDiagnostic})"/>.
        /// </remarks>
        IExpression Parse(string input);
    }

    /// <summary>
    /// Evaluates an <see cref="IExpression"/> against an optional evaluation context and returns the resulting value.
    /// </summary>
    public interface IExpressionEvaluator
    {
        /// <summary>
        /// Evaluates the provided expression and returns the resulting value.
        /// </summary>
        /// <param name="expression">The expression to evaluate.</param>
        /// <param name="context">An optional context object (for example, a model or variable scope) used during evaluation.</param>
        /// <returns>The result of evaluating the expression; may be <c>null</c> if the expression produces no value.</returns>
        object? Evaluate(IExpression expression, object? context = null);
    }
}
