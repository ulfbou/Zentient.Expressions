# Zentient.Expressions — Canonical Specification (First Release)

**Version:** 1.0.0  
**Audience:** Application developers, library authors, test engineers, maintainers

---

## 1. Purpose and Scope

**Mission:**  
Provide a unified, type-safe, and extensible foundation for parsing, evaluating, composing, transforming, and serializing expressions across the Zentient ecosystem.

**Key Goals:**
- Unify all expression categories (arithmetic, logical, predicate, lambda, etc.)
- Standardize lifecycle: parsing → validation → evaluation → transformation → serialization
- Enable fluent, composable, immutable APIs
- Integrate seamlessly with Zentient.Validation, Zentient.Formatters, and Zentient.Common
- Protect API boundaries: only entry/extension points are public—all else internal

---

## 2. Design Principles

- **Contract-First:** All surface APIs defined in Zentient.Abstractions.Expressions
- **Immutability:** All expression objects are immutable and thread-safe
- **Composable:** Expressions can be nested and transformed fluently
- **DX-First:** Intellisense-driven discovery, fluent entry points, zero boilerplate
- **Extensible:** New parsers, evaluators, composers can be registered via entry hooks
- **Observability:** Events and diagnostics for parsing, evaluation, transformation
- **Strict Encapsulation:** Concrete implementations remain internal

---

## 3. Public API: Entry & Extension Points

### 3.1 Core Contracts (in Zentient.Abstractions.Expressions)

- `IExpression`  
  - `ExpressionKind Kind { get; }`
  - `string Canonical { get; }`
  - `IReadOnlyList<IExpression> Operands { get; }`

- `ITypedExpression<T> : IExpression`  
  - `T Evaluate(object? context = null)`

- `IExpressionParser`  
  - `IExpression Parse(string expressionText)`
  - `bool TryParse(string expressionText, out IExpression? expression, out IReadOnlyList<string> errors)`

- `IExpressionEvaluator`  
  - `object? Evaluate(IExpression expression, object? context = null)`

- `IExpressionComposer`  
  - `IExpression Compose(ExpressionKind kind, params IExpression[] operands)`

- `IExpressionTransformer`  
  - `IExpression Transform(IExpression root, Func<IExpression, IExpression> visitor)`

- `IExpressionSerializer`  
  - `string Serialize(IExpression expression, ExpressionSerializationFormat format = Canonical)`
  - `IExpression Deserialize(string text, ExpressionSerializationFormat format = Canonical)`

- **Enumerations:**  
  - `ExpressionKind` {Arithmetic, Logical, Predicate, Lambda, Custom}
  - `ExpressionSerializationFormat` {Canonical, Infix, Prefix, Postfix}

---

### 3.2 Static Entry Points & Hooks (in Zentient.Expressions)

- **Static registry:**  
  - `ExpressionRegistry.DefaultParser`
  - `ExpressionRegistry.DefaultEvaluator`
  - `ExpressionRegistry.DefaultComposer`
  - `ExpressionRegistry.DefaultSerializer`

- **Events:**  
  - `event EventHandler<IExpression> OnParsed`
  - `event EventHandler<(IExpression Expression, object? Result)> OnEvaluated`

- **Registration API:**  
  - `RegisterParser(IExpressionParser parser)`
  - `RegisterEvaluator(IExpressionEvaluator evaluator)`
  - `RegisterComposer(IExpressionComposer composer)`
  - `RegisterSerializer(IExpressionSerializer serializer)`

- **Thread Safety:**  
  - All hooks and registration APIs are thread-safe and support safe unsubscription/unregistration.

---

### 3.3 Extension Methods (in Zentient.Extensions.Expressions)

- **Convenience:**  
  - `EvaluateExpression(this string expressionText, object? context = null)`
  - `ToCanonicalString(this IExpression expr)`
  - `ToPrefixString(this IExpression expr)`
  - `ToInfixString(this IExpression expr)`
  - `ToPostfixString(this IExpression expr)`

- **Transformation:**  
  - `Simplify(this IExpression expr)`
  - `Flatten(this IExpression expr)`
  - `Walk(this IExpression expr, Action<IExpression> visitor)`

- **Validation & Diagnostics:**  
  - `IsValid<T>(this ITypedExpression<T> expr)`
  - `GetDiagnostics(this IExpression expr)`

---

## 4. Internal-Only Members

- All concrete implementations (parsers, evaluators, composers, transformers, serializer)
- All registry/data structures
- Operator precedence, grammar rules, evaluation context
- Any caching/optimization

---

## 5. Enumerations

- `ExpressionKind` (see above)
- `ExpressionSerializationFormat` (see above)

---

## 6. Package Structure

- `Zentient.Abstractions.Expressions` — interfaces/enums only (contracts)
- `Zentient.Expressions` — implementation, registry, and static entry points (public surface only as described)
- `Zentient.Extensions.Expressions` — extension methods

All other implementations/internal details are hidden.

---

## 7. Example Usage

```csharp
// Parse & evaluate
var expr = ExpressionRegistry.DefaultParser.Parse("2 + 2 * (3 - 1)");
var result = ExpressionRegistry.DefaultEvaluator.Evaluate(expr);

// Subscribe to events
ExpressionRegistry.OnParsed += (s, e) => Logger.Log(e.Canonical);
ExpressionRegistry.OnEvaluated += (s, e) => Logger.Log($"Evaluated: {e.Expression} = {e.Result}");

// Extension convenience
var value = "a && b".EvaluateExpression(new { a = true, b = false });

// Composition & simplification
var composed = ExpressionRegistry.DefaultComposer.Compose(ExpressionKind.LogicalAnd, expr1, expr2);
var simplified = composed.Simplify();
```

---

## 8. Error Handling

- **Throw:** Exceptions only for illegal API usage (null/malformed params)
- **Diagnostics:** All user/parse/eval errors returned via diagnostics, not exceptions

---

## 9. Testing & Quality Gates

- Unit tests for each contract, round-trip parse/serialize, and property-based invariants
- Integration with Zentient.Common and Zentient.Validation
- CI: Build → Test → Static Analysis → Package Validation

---

## 10. Developer Experience

- Full API documentation for all public entry and extension points
- Samples for parsing, evaluation, eventing, registry usage
- Events observable and safe; no hidden state leaks
- All entry/extension points discoverable via Intellisense

---

## 11. Changelog (First Release)

- Canonical IExpression and ITypedExpression<T> contracts
- Registry, events, and registration hooks
- Canonical extension methods for DX-first use
- All types not listed above are internal and sealed
