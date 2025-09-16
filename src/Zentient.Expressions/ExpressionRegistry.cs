// <copyright file="ExpressionRegistry.cs" authors="Zentient Framework Team">
// Copyright © 2025 Zentient Framework Team. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

using Zentient.Abstractions.Expressions;

namespace Zentient.Expressions
{
    /// <summary>
    /// Central registry that exposes default expression parser and evaluator implementations
    /// and lifecycle events for parsed and evaluated expressions.
    /// </summary>
    /// <remarks>
    /// This static type is thread-safe for replacing the default parser/evaluator and for
    /// subscribing/unsubscribing to events. Consumers may replace the defaults or subscribe
    /// to <see cref="OnParsed"/> and <see cref="OnEvaluated"/> to receive notifications.
    /// </remarks>
    public static class ExpressionRegistry
    {
        private static readonly object _sync = new();
        private static IExpressionParser _defaultParser = new DefaultExpressionParser();

        /// <summary>
        /// Gets or sets the default <see cref="IExpressionParser"/> used by the library.
        /// </summary>
        /// <remarks>
        /// Setting the value is thread-safe. Attempting to assign <c>null</c> will throw
        /// <see cref="ArgumentNullException"/>.
        /// </remarks>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="value"/> is <c>null</c>.</exception>
        public static IExpressionParser DefaultParser
        {
            get => _defaultParser;
            set
            {
                if (value is null) throw new ArgumentNullException(nameof(value));
                lock (_sync) { _defaultParser = value; }
            }
        }

        private static IExpressionEvaluator _defaultEvaluator = new DefaultExpressionEvaluator();

        /// <summary>
        /// Gets or sets the default <see cref="IExpressionEvaluator"/> used by the library.
        /// </summary>
        /// <remarks>
        /// Setting the value is thread-safe. Attempting to assign <c>null</c> will throw
        /// <see cref="ArgumentNullException"/>.
        /// </remarks>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="value"/> is <c>null</c>.</exception>
        public static IExpressionEvaluator DefaultEvaluator
        {
            get => _defaultEvaluator;
            set
            {
                if (value is null) throw new ArgumentNullException(nameof(value));
                lock (_sync) { _defaultEvaluator = value; }
            }
        }

        private static Action<IExpression>? _onParsed;

        /// <summary>
        /// Event raised after an <see cref="IExpression"/> has been successfully parsed.
        /// </summary>
        /// <remarks>
        /// Event handlers will be invoked with the parsed expression. Subscription and
        /// unsubscription are performed under a lock to ensure thread-safety.
        /// </remarks>
        public static event Action<IExpression> OnParsed
        {
            add { lock (_sync) { _onParsed += value; } }
            remove { lock (_sync) { _onParsed -= value; } }
        }

        private static Action<IExpression, object?>? _onEvaluated;

        /// <summary>
        /// Event raised after an <see cref="IExpression"/> has been evaluated.
        /// </summary>
        /// <remarks>
        /// Handlers receive the expression and the evaluation result (which may be <c>null</c>).
        /// Subscription and unsubscription are thread-safe.
        /// </remarks>
        public static event Action<IExpression, object?> OnEvaluated
        {
            add { lock (_sync) { _onEvaluated += value; } }
            remove { lock (_sync) { _onEvaluated -= value; } }
        }

        /// <summary>
        /// Invokes the <see cref="OnParsed"/> event for a parsed expression.
        /// </summary>
        /// <param name="expr">The parsed expression to publish to subscribers.</param>
        internal static void RaiseParsed(IExpression expr)
            => _onParsed?.Invoke(expr);

        /// <summary>
        /// Invokes the <see cref="OnEvaluated"/> event for an evaluated expression.
        /// </summary>
        /// <param name="expr">The expression that was evaluated.</param>
        /// <param name="result">The result of the evaluation; may be <c>null</c>.</param>
        internal static void RaiseEvaluated(IExpression expr, object? result)
            => _onEvaluated?.Invoke(expr, result);

        // Adapter implementations for the public parser/evaluator

        /// <summary>
        /// Internal adapter that delegates parsing calls to the concrete parser implementation.
        /// </summary>
        private class DefaultExpressionParser : IExpressionParser
        {
            /// <inheritdoc />
            public bool TryParse(string input, out IExpression? expression, out IReadOnlyList<ParseDiagnostic> diagnostics)
                => ExpressionParser.TryParse(input, out expression, out diagnostics);

            /// <inheritdoc />
            public IExpression Parse(string input)
                => ExpressionParser.Parse(input);
        }

        /// <summary>
        /// Internal adapter that delegates evaluation calls to the concrete evaluator implementation.
        /// </summary>
        private class DefaultExpressionEvaluator : IExpressionEvaluator
        {
            /// <inheritdoc />
            public object? Evaluate(IExpression expression, object? context = null)
            {
                var result = StubEvaluator.Evaluate(expression, context);
                ExpressionRegistry.RaiseEvaluated(expression, result);
                return result;
            }
        }

        /// <summary>
        /// Creates a typed view over an existing expression using the registry's evaluator.
        /// </summary>
        /// <typeparam name="T">The expected result type of the expression.</typeparam>
        /// <param name="expr">The expression to wrap.</param>
        /// <returns>An <see cref="ITypedExpression{T}"/> that evaluates the underlying expression and casts the result to <typeparamref name="T"/>.</returns>
        public static ITypedExpression<T> AsTyped<T>(IExpression expr)
        {
            if (expr is null) throw new ArgumentNullException(nameof(expr));
            return new TypedExpression<T>(expr);
        }

        // Internal wrapper implementing the typed expression contract.
        private sealed class TypedExpression<T> : ITypedExpression<T>
        {
            private readonly IExpression _inner;

            public TypedExpression(IExpression inner) => _inner = inner ?? throw new ArgumentNullException(nameof(inner));

            public ExpressionKind Kind => _inner.Kind;

            public string Canonical => _inner.Canonical;

            public IReadOnlyList<IExpression> Operands => _inner.Operands;

            public T Evaluate(object? context = null)
            {
                var result = ExpressionRegistry.DefaultEvaluator.Evaluate(_inner, context);
                if (result is null && default(T) is null)
                    return (T)result!; // allow null for reference types
                if (result is T t) return t;

                // Try convert common primitive/numeric types
                try
                {
                    var converted = Convert.ChangeType(result, typeof(T));
                    return (T)converted!;
                }
                catch (Exception ex)
                {
                    throw new InvalidCastException($"Cannot cast evaluator result of type '{result?.GetType()}' to '{typeof(T)}'.", ex);
                }
            }
        }
    }
}
