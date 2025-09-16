Pull Request Checklist — Zentient.Expressions (Alpha 1.0.0)

Purpose:
Verify that this contribution meets the Alpha specification for Zentient.Expressions.
Alpha scope = minimal, unblock Zentient.Testing.Alpha, no DX sugar, no extended diagnostics, no context lookup.


---

Contracts

[ ] IExpression, ITypedExpression<T>, parser, and evaluator interfaces remain unchanged.

[ ] Public API surface matches the Public API Reference (Alpha spec) exactly.


ExpressionRegistry

[ ] DefaultParser and DefaultEvaluator are set with proper thread-safety (lock).

[ ] OnParsed fires after every successful parse.

[ ] OnEvaluated fires with the result value.


Diagnostics

[ ] TryParse reports empty/whitespace input as a diagnostic.

[ ] The parser and TryParse method collect and return diagnostics for all parsing failures.


Helpers

[ ] ToCanonicalString() returns the .Canonical property.


Encapsulation

[ ] ExpressionParser, Parser, and Lexer are marked internal.

[ ] All AST node implementations are internal sealed.



---

Contributor Notes

Keep changes minimal: no new helpers, no advanced diagnostics, no context binding.

Follow Alpha spec rigorously: this is a foundational build, not a feature-complete release.


Reviewer Sign-off

[ ] Specification adherence confirmed.

[ ] No out-of-scope features introduced.

[ ] API is stable for Beta/RC layering.


---





### Pull Request Checklist — Zentient.Expressions (Alpha 1.0.0)

Thank you for contributing! 🎉

This checklist ensures your changes align with the Alpha Specification. Please verify each item before requesting review.

---

#### Contracts

- [ ] IExpression, ITypedExpression<T>, parser, and evaluator interfaces remain unchanged.
- [ ] Public API surface matches the Public API Reference in the Alpha spec exactly.

---

#### ExpressionRegistry

- [ ] DefaultParser and DefaultEvaluator are set with proper thread-safety (lock).
- [ ] OnParsed fires after every successful parse.
- [ ] OnEvaluated fires with the result value, not the context.

---

#### Diagnostics

- [ ] TryParse reports empty/whitespace input as a diagnostic.
- [ ] ParseDiagnostic includes Severity.
- [ ] Parser uses Expect for hard requirements and TryConsumeIf for soft checks.

---

#### String Escaping

- [ ] ConstantExpression.EscapeString covers \\, ", \n, \r, \t.

---

#### Context Lookup

- [ ] IdentifierExpression resolves values from IDictionary<string, object?> in evaluator stub.

---

#### Helpers

- [ ] ExpressionRegistry.AsTyped<T>() factory exists.
- [ ] ToCanonicalString() returns the .Canonical property.
- [ ] ToDebugString() outputs Kind, operand count, and canonical form.

---

#### Encapsulation

- [ ] ExpressionParser, Parser, and Lexer are marked internal.
- [ ] All AST node implementations are internal sealed.

---

#### Contributor Notes

Please link to tests proving each feature/constraint.

If modifying internals, ensure no public surface leaks.

For new extension methods, confirm they live in Zentient.Extensions.Expressions.

---

#### Reviewer Sign-off

- [ ] Public API unchanged  
- [ ] Events and hooks verified  
- [ ] Diagnostics & helpers covered with tests  
- [ ] Encapsulation preserved  

Reviewer:   
Date:   
