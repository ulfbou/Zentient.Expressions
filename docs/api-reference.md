API Reference — Zentient.Expressions (Summary)

Note: This file is a high-level reference of the public contracts and common usage patterns intended for library consumers.

Core interfaces

IExpression
- ExpressionKind Kind { get; }
- string Canonical { get; }
- IReadOnlyList<IExpression> Operands { get; }

ITypedExpression<T> : IExpression
- T Evaluate(object? context = null)

IExpressionParser
- IExpression Parse(string expressionText)
- bool TryParse(string expressionText, out IExpression? expression, out IReadOnlyList<string> errors)

IExpressionEvaluator
- object? Evaluate(IExpression expression, object? context = null)

IExpressionComposer
- IExpression Compose(ExpressionKind kind, params IExpression[] operands)

IExpressionTransformer
- IExpression Transform(IExpression root, Func<IExpression, IExpression> visitor)

IExpressionSerializer
- string Serialize(IExpression expression, ExpressionSerializationFormat format = ExpressionSerializationFormat.Canonical)
- IExpression Deserialize(string text, ExpressionSerializationFormat format = ExpressionSerializationFormat.Canonical)

Factory and registry

ExpressionRegistry
- static IExpressionParser DefaultParser { get; }
- static IExpressionEvaluator DefaultEvaluator { get; }
- static IExpressionComposer DefaultComposer { get; }
- static IExpressionSerializer DefaultSerializer { get; }
- static event EventHandler<IExpression> OnParsed
- static event EventHandler<(IExpression Expression, object? Result)> OnEvaluated
- static void RegisterParser(IExpressionParser parser)
- static void RegisterEvaluator(IExpressionEvaluator evaluator)
- static void RegisterComposer(IExpressionComposer composer)
- static void RegisterSerializer(IExpressionSerializer serializer)

Enums

ExpressionKind { Arithmetic, Logical, Predicate, Lambda, Custom }
ExpressionSerializationFormat { Canonical, Infix, Prefix, Postfix }

Extension methods

- EvaluateExpression(this string expressionText, object? context = null)
- ToCanonicalString(this IExpression expr)
- Simplify(this IExpression expr)

Guidelines

- Implementations should be immutable and thread-safe.
- Parsers should return diagnostics via TryParse rather than throwing for user input errors.
- Expose only the explicitly public contracts; keep concrete types internal to allow future refactoring.
