# Release Notes — Alfa

This release marks the alfa (pre-release) state for Zentient.Expressions. It is intended for early validation and developer feedback.

Highlights

- Contract-first public APIs for expressions, parser and evaluator.
- Thread-safe registry with DefaultParser and DefaultEvaluator and observable events for parse/evaluate lifecycle.
- Parser and lexer implementing a small expression grammar with diagnostics reported via TryParse.
- Simple evaluator that supports constants, identifier/member lookup from dictionary contexts and delegate invocation via context.
- Canonical string representation with proper string escaping for special characters.

Known constraints

- This alfa release intentionally keeps implementation details internal; concrete parser/evaluator types are not part of the public surface.
- The companion package Zentient.Abstractions.Expressions is provisional and will be consolidated into a unified Zentient.Abstractions in a future version.

Guidance

- Pre-release packages are produced from release/* branches; stable releases are produced from semantic version tags.
- Pin to exact package versions in CI if using alfa packages to prevent surprises.
