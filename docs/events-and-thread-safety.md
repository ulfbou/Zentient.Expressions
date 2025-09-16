Events and thread-safety

Registry events

- OnParsed: fired after a successful parse; provides the parsed expression.
- OnEvaluated: fired after evaluation; provides the expression and result.

Thread-safety guarantees

- All registry registration methods and event subscription/unsubscription are thread-safe.
- Event invocation occurs after the operation completes to avoid exposing partially-constructed state.
- Expression instances are immutable and can be safely shared between threads without additional synchronization.

Guidance for implementers

- Implementations should use internal locks or concurrent collections to manage registry state.
- Avoid invoking external code while holding internal locks to prevent deadlocks.
