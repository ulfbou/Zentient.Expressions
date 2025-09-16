Architecture and package boundaries

This section explains the package layout, responsibilities and public vs internal boundaries.

Packages

- Zentient.Abstractions.Expressions — public contracts (interfaces, enums). This package defines the canonical public surface for expressions and is intended to be stable across releases.

- Zentient.Expressions — the runtime implementation, registry, and static entry points. The package exposes only the public contracts and registry entry points; concrete implementation types are internal.

- Zentient.Extensions.Expressions — convenience extension methods for common DX patterns (EvaluateExpression, ToCanonicalString, Simplify, Walk). These are lightweight helpers that operate only on public contracts.

Runtime components

- Parser: converts text into an IExpression tree and produces diagnostics for user errors.
- Evaluator: evaluates an IExpression against a context and returns a result value.
- Composer: programmatically constructs expression trees.
- Transformer: applies tree transformations using visitor-style functions.
- Serializer: converts expression trees to/from textual representations.

Event model and registry

A central ExpressionRegistry exposes default instances for parser, evaluator, composer and serializer along with events for OnParsed and OnEvaluated. The registry provides thread-safe registration and safe event invocation semantics.