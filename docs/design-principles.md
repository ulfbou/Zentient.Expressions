Design principles

This document summarizes the principles that guide the design and evolution of the public API for Zentient.Expressions.

Contract-first

- Public interfaces define the library surface. Implementations remain internal to enable refactoring without breaking consumers.

Immutability

- Expression instances are immutable and safe for concurrent consumption. Wherever possible, objects are value-like and do not expose mutable collections.

DX-first

- Registry entry points and extension methods prioritize ergonomics for common workflows: parsing, evaluating, serializing, and transforming.

Observability

- The library exposes events for parsing and evaluation to enable lightweight instrumentation. Events are safe to subscribe/unsubscribe concurrently.

Extensibility

- Consumers may register alternate parser/evaluator/composer/serializer implementations via the registry. Implementations must adhere to contract expectations (thread-safety, immutability).

Minimal public surface

- Keep only necessary entry points and types public. Concrete parser/evaluator types and internal helpers remain internal to preserve flexibility and enable implementation changes across minor releases.