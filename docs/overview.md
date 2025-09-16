Zentient.Expressions — Library Overview

Zentient.Expressions provides a compact, composable, and type-safe expression framework for .NET applications. It defines a stable set of contracts for representing expressions (typed and untyped), parsing textual expressions, evaluating against a context, composing expressions programmatically, transforming expression trees, and serializing expressions for transport and storage.

Primary goals

- Contract-first design: public-facing interfaces define the library surface so multiple implementations can coexist.
- Immutability and thread-safety: expression instances are immutable and safe for concurrent use.
- DX-first entry points: simple static registry and a small set of extension methods for common developer workflows.
- Minimal public surface: most implementation details are intentionally internal to preserve future flexibility.

What this library provides

- Core contracts (interfaces and enums) that define expression shapes and behaviors.
- A registry with default parser, evaluator, composer and serializer entry points, and safe event hooks for observability.
- Extension methods that simplify common tasks such as quick evaluation and canonical rendering.
- Packaging and CI/Release readiness: packs and prerelease workflows produce predictable prerelease package versions for early adopters.

Intended users

- Application developers who need a small, reliable expression language for business logic or configuration.
- Library authors who want to implement alternate parsers or evaluators that interoperate with the registry.
- Test engineers who need deterministic expression parsing and evaluation for testing scenarios.
