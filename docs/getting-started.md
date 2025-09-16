Getting started — Zentient.Expressions

Installation

Install the package (pre-release packages may be available from NuGet with a prerelease label):

dotnet add package Zentient.Expressions --version 0.1.0-alfa

Basic usage

Parsing and evaluating a simple expression:

```csharp
using Zentient.Expressions;

var expr = ExpressionRegistry.DefaultParser.Parse("2 + 2 * 3");
var result = ExpressionRegistry.DefaultEvaluator.Evaluate(expr);
```

Convenience extension:

```csharp
using Zentient.Extensions.Expressions;

var value = "a && b".EvaluateExpression(new Dictionary<string, object?> { ["a"] = true, ["b"] = false });
```

Composing expressions programmatically:

```csharp
var left = ExpressionRegistry.DefaultComposer.Compose(ExpressionKind.Literal, "1");
var right = ExpressionRegistry.DefaultComposer.Compose(ExpressionKind.Literal, "2");
var add = ExpressionRegistry.DefaultComposer.Compose(ExpressionKind.ArithmeticAdd, left, right);
```

Serializing and deserializing:

```csharp
var canonical = ExpressionRegistry.DefaultSerializer.Serialize(add);
var round = ExpressionRegistry.DefaultSerializer.Deserialize(canonical);
```

Extending the library

Register a custom parser or evaluator at runtime:

```csharp
ExpressionRegistry.RegisterParser(myParserInstance);
ExpressionRegistry.RegisterEvaluator(myEvaluatorInstance);
```

Thread safety

All registry operations and events are thread-safe. Implementations should follow immutability and avoid exposing mutable internals.
