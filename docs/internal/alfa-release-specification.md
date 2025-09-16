# Zentient.Errors — Alfa Release Specification

**Version:** Alfa 0.1  
**Status:** Pre-release / Development  
**Package:** `Zentient.Errors`  
**Abstractions Package:** `Zentient.Abstractions.Errors`  
**Extensions Package:** `Zentient.Extensions.Errors`  

---

## 1. Overview

`Zentient.Errors` provides a **structured, immutable, type-safe framework** for representing, building, and handling errors in .NET applications.  
The library enforces **explicit contracts, DX-first patterns, and robust validation**, aligning with other Zentient packages (Expressions, Testing, etc.).

- **Core goals:** Immutable error instances, fluent builder, event-driven registry, robust metadata, and extensibility via builders and extensions.
- **Alfa scope:** All abstractions and immutable implementations, registry with events, and ergonomic extensions.
- **Future scope:** DI integration, localization, telemetry, and hierarchical catalogs are deferred.

---

## 2. Package Structure

### 2.1 Zentient.Abstractions.Errors

Contains all frozen interfaces and enums:

| Interface / Type | Description |
|-----------------|-------------|
| `IErrorDefinition` | Defines intrinsic properties of an error type: `Severity`, `IsTransient`, `IsUserFacing`. |
| `ErrorSeverity` | Enum: `Informational`, `Warning`, `Error`, `Critical`. |
| `IErrorInfo<TErrorDefinition>` | Immutable error instance with definition, code, message, instance ID, inner errors, metadata. |
| `IErrorInfoBuilder<TErrorDefinition>` | Fluent builder for creating `IErrorInfo<T>` instances. |

**Extension Points:**  

- `Zentient.Abstractions.Errors.Builders` → Builder interfaces  
- `Zentient.Abstractions.Errors.Definitions` → Error type definitions

---

### 2.2 Zentient.Errors (Implementation)

#### 2.2.1 Core Classes

- `ErrorInfo<TErrorDefinition>` — immutable, sealed implementation of `IErrorInfo<T>`.
- `ErrorInfoBuilder<TErrorDefinition>` — fully guarded builder implementing `IErrorInfoBuilder<T>`.
- `ErrorRegistry` — static entry point for registry, builder factory, and lifecycle events.

#### 2.2.2 ErrorRegistry API

| Member | Type | Description | Notes |
|--------|------|-------------|-------|
| `DefaultBuilderFactory` | `Func<IErrorInfoBuilder<TErrorDefinition>>` | Factory delegate to create new builders | MUST be thread-safe |
| `OnBuilt` | `event Action<IErrorInfo<TErrorDefinition>>` | Fires when an error instance is successfully built | MUST be thread-safe |
| `OnDefinitionRegistered` | `event Action<IErrorDefinition>` | Fires when a new error definition is registered | MUST be thread-safe |

**Thread Safety:** All registry events and builder factory access MUST be protected via internal lock objects.

---

### 2.3 Zentient.Extensions.Errors

Contains **ergonomic extensions** only:

- `WithCode()`, `WithMetadata()`, `ToErrorInfo(Exception)`, `PrettyPrint()`  
- **No new abstractions** are introduced here.

**Purpose:** DX-first convenience for consuming code, without altering core contracts.

---

## 3. Builder Pattern

`IErrorInfoBuilder<TErrorDefinition>` MUST support:

1. `WithErrorDefinition(TErrorDefinition)` — REQUIRED  
2. `WithMessage(string)` — REQUIRED  
3. `WithInstanceId(string)` — OPTIONAL (auto-generate GUID if missing)  
4. `WithInnerError(IErrorInfo<TErrorDefinition>)` — OPTIONAL  
5. `WithInnerErrors(IEnumerable<IErrorInfo<TErrorDefinition>>?)` — OPTIONAL  
6. `WithMetadata(string, object?)` — OPTIONAL  
7. `WithMetadata(IMetadata?)` — OPTIONAL  
8. `Build()` — MUST validate REQUIRED properties and return immutable `IErrorInfo<T>`.

**Guards:**

- Null or empty values MUST throw `ArgumentNullException` or `ArgumentException`.
- `Build()` MUST throw `InvalidOperationException` if required fields are unset.

---

## 4. Validation & Error Handling

| Scenario | Action |
|----------|--------|
| Missing `ErrorDefinition` | Throw `InvalidOperationException` on `Build()` |
| Missing `Message` | Throw `InvalidOperationException` on `Build()` |
| Null inner error | Throw `ArgumentNullException` |
| Null/empty metadata key | Throw `ArgumentException` |

**Optionality Annotations:**

- **MUST**: Immutable error types, registry events, builder pattern, validation/guards.  
- **SHOULD**: PrettyPrint(), ToErrorInfo(Exception), WithCode() extensions.  
- **MAY**: Hierarchical catalogs, DI integration, localization, telemetry.

---

## 5. Usage Patterns

### 5.1 Constructing an Error

```csharp
var error = ErrorRegistry.DefaultBuilderFactory()
    .WithErrorDefinition(MyErrorDefinition.Instance)
    .WithMessage("Failed to process request")
    .WithMetadata("CorrelationId", correlationId)
    .Build();

5.2 Subscribing to Registry Events

ErrorRegistry.OnBuilt += e => Console.WriteLine($"Error built: {e.Canonical}");
ErrorRegistry.OnDefinitionRegistered += def => Console.WriteLine($"Definition registered: {def.Name}");

5.3 Using Extensions

string pretty = error.PrettyPrint();
var withCode = error.WithCode(MyErrorCodes.FailedProcessing);


---

6. Testing Guidelines (Xunit + FluentAssertions)

1. Error Construction

Verify builder sets properties correctly.

Verify immutability of resulting IErrorInfo<T>.



2. Validation

Null/empty argument guards.

Required properties enforced in Build().



3. Registry Events

OnBuilt fires after Build().

OnDefinitionRegistered fires on definition registration.

Thread safety tests for concurrent subscriptions.



4. Extensions

Verify .WithMetadata(), .WithCode(), .ToErrorInfo(), .PrettyPrint() return expected results.





---

7. Deliverables

Package	Content

Zentient.Abstractions.Errors	Interfaces, enums, builder contracts, definitions
Zentient.Errors	Immutable implementations, builders, registry, events
Zentient.Extensions.Errors	Fluent extensions, DX helpers
Tests	Xunit + FluentAssertions coverage of all above



---

8. Alfa Constraints

No DI integration in alfa — registry/event-first only.

Immutable only — all error info must be sealed and immutable.

Thread-safe registry — all public members and events must be synchronized.

No runtime exceptions beyond guard violations — illegal usage only.

Future-proofing — DI, telemetry, localization deferred.



---

9. References

Zentient.Expressions Alfa Spec

Zentient.Metadata Spec

DX-first and Builder Patterns Guide



---

End of Alfa Specification — Zentient.Errors


