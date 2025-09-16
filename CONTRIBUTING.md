# Contribution Guidelines

Thank you for your interest in contributing. This repository targets .NET 8 and .NET 9.

## Prerequisites

- .NET 8.0 SDK and/or .NET 9.0 SDK
- Git

## Quick setup

1. Clone the repository
2. Install Dependencies
   ```bash
   dotnet restore
   ```
3. Verify Setup
   ```bash
   dotnet build
   dotnet test
   ```

## Branching & Releases

- `main`: stable releases
- `develop`: integration
- `feature/*`, `bugfix/*`, `hotfix/*` as usual
- `release/*` branches are used to produce prerelease packages (alpha, beta, rc). The CI will generate prerelease versions that include the release label and CI metadata.

## Packaging

- Ensure projects intended for packaging include `<IsPackable>true</IsPackable>` in their `.csproj`.
- Test projects should include `<IsPackable>false</IsPackable>`.

## Testing

- Add unit tests alongside features
- Run `dotnet test` locally before PR

## Code style

- Follow .NET/C# conventions

## Pull Requests

- Provide tests for bug fixes and features
- Update CHANGELOG.md when relevant

For more details see README.md and project docs in the docs/ directory.
