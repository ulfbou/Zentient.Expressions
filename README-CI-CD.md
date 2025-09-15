# Zentient.Expressions CI/CD Setup

This document provides a quick overview of the CI/CD setup and validation commands.

## Project Structure
```
Zentient.Expressions/
├── .github/workflows/ci-cd.yml      # Main CI/CD workflow
├── .gitignore                       # .NET gitignore
├── Zentient.Expressions.sln         # Solution file
├── Zentient.Expressions.csproj      # Main library project
├── ExpressionParser.cs              # Sample library code
├── tests/
│   └── Zentient.Expressions.Tests/
│       ├── Zentient.Expressions.Tests.csproj
│       └── ExpressionParserTests.cs
└── docs/
    └── CI-CD-Strategy.md            # Detailed workflow documentation
```

## Local Development Commands

### Build and Test
```bash
# Restore dependencies
dotnet restore

# Build the solution
dotnet build --configuration Release

# Run tests
dotnet test tests/Zentient.Expressions.Tests/Zentient.Expressions.Tests.csproj --configuration Release

# Pack with version suffix (for testing)
dotnet pack --configuration Release --output ./artifacts --version-suffix alfa.1
```

### Validate Workflow
```bash
# Check YAML syntax
python3 -c "import yaml; yaml.safe_load(open('.github/workflows/ci-cd.yml').read()); print('Valid')"
```

## Branching Strategy

| Branch | Release Stage | Version Format | Auto-Deploy |
|--------|---------------|----------------|-------------|
| develop | Alfa | 1.0.0-alfa.{build} | Yes |
| release/beta | Beta | 1.0.0-beta.{build} | Yes |
| release/rc | RC | 1.0.0-rc.{build} | Yes |
| main | Stable | 1.0.0 | Yes |

## Required Repository Setup

1. **Secrets**: Add `NUGET_API_KEY` to repository secrets
2. **Environments**: Create environments (alfa, beta, rc, stable) with appropriate protection rules
3. **Branches**: Set up branch protection for main and release branches

See `docs/CI-CD-Strategy.md` for complete documentation.