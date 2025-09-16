# Changelog

## [Unreleased]

### Added
- Initial alfa release artifacts for Zentient.Expressions
  - Contract-first public API in Zentient.Abstractions.Expressions
  - Default parser, evaluator, composer, and registry with events
  - Parser TryParse with diagnostics and lexer
  - Stub evaluator with dictionary-based identifier/member lookup and delegate invocation
  - Extension helpers EvaluateExpression, ToCanonicalString, ToDebugString
  - CI workflow updated to support release/* prerelease branches

### Tests
- Unit tests for parsing, evaluation, registry events, extensions and concurrency added

### Docs
- Comprehensive public documentation under docs/ and release notes for alfa
