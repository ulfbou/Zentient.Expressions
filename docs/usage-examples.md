Usage examples and recipes

Parsing and evaluation

```csharp
var parser = ExpressionRegistry.DefaultParser;
var evaluator = ExpressionRegistry.DefaultEvaluator;

var expr = parser.Parse("2 + 2 * (3 - 1)");
var result = evaluator.Evaluate(expr);
```

Using extension methods

```csharp
using Zentient.Extensions.Expressions;

var value = "a && b".EvaluateExpression(new Dictionary<string, object?> { ["a"] = true, ["b"] = false });
```

Composing expressions programmatically

```csharp
var lit1 = ExpressionRegistry.DefaultComposer.Compose(ExpressionKind.Literal, "1");
var lit2 = ExpressionRegistry.DefaultComposer.Compose(ExpressionKind.Literal, "2");
var sum = ExpressionRegistry.DefaultComposer.Compose(ExpressionKind.ArithmeticAdd, lit1, lit2);
```

Transforming and simplifying

```csharp
var simplified = ExpressionRegistry.DefaultTransformer.Transform(sum, expr => {
    // Example: constant folding visitor
    return expr; // replace with transformed expression
});
```

Event subscription

```csharp
ExpressionRegistry.OnParsed += (s, e) => Console.WriteLine($"Parsed: {e.Canonical}");
ExpressionRegistry.OnEvaluated += (s, e) => Console.WriteLine($"Evaluated: {e.Expression.Canonical} => {e.Result}");
```

Diagnostics and TryParse

```csharp
if (!ExpressionRegistry.DefaultParser.TryParse(text, out var expression, out var errors)) {
    foreach (var err in errors) Console.WriteLine(err);
}
```
