# CI/CD Staged Release Strategy

This repository implements a staged release strategy with four distinct release stages, each mapped to specific branches and with automated CI/CD workflows.

## Branching Strategy

### Release Stages and Branch Mapping

| Stage | Branch | Version Format | Purpose |
|-------|--------|----------------|---------|
| **Alfa** | `develop` | `1.0.0-alfa.{build-number}` | Early development builds with latest features |
| **Beta** | `release/beta` | `1.0.0-beta.{build-number}` | Feature-complete builds for testing |
| **Release Candidate** | `release/rc` | `1.0.0-rc.{build-number}` | Production-ready candidates for final validation |
| **Stable** | `main` | `1.0.0` | Stable releases for production use |

### Workflow Triggers

The CI/CD pipeline is triggered on:
- **Push** to any of the release branches
- **Pull Requests** to any of the release branches  
- **Tags** starting with `v` (e.g., `v1.0.0`)

## Release Process

### 1. Development (Alfa Releases)
- All active development happens on the `develop` branch
- Every push to `develop` triggers an alfa build
- Alfa packages are published with version suffix `alfa.{build-number}`

### 2. Beta Releases
- Create `release/beta` branch from `develop` when features are complete
- Push to `release/beta` triggers beta builds
- Beta packages are published with version suffix `beta.{build-number}`

### 3. Release Candidate
- Create `release/rc` branch from `release/beta` when beta is stable
- Push to `release/rc` triggers RC builds
- RC packages are published with version suffix `rc.{build-number}`

### 4. Stable Release
- Merge `release/rc` to `main` when RC is validated
- Tag the release with `v{version}` (e.g., `v1.0.0`)
- Stable packages are published without version suffix
- GitHub release is automatically created

## Pipeline Jobs

### 1. Detect Release Stage
- Determines the release stage based on branch/tag
- Sets version suffix and publishing flags
- Outputs used by subsequent jobs

### 2. Build and Test
- Restores dependencies
- Builds the project with appropriate versioning
- Runs all unit tests
- Creates NuGet packages
- Uploads package artifacts

### 3. Publish
- Downloads package artifacts
- Publishes to NuGet.org based on release stage
- Uses GitHub environments for deployment protection
- Requires appropriate secrets (NUGET_API_KEY)

### 4. Create Release (Stable only)
- Creates GitHub release for stable versions
- Attaches release notes
- Only runs for tagged releases on main branch

## Environment Configuration

The workflow uses GitHub environments for each release stage:
- `alfa` - For alfa releases from develop
- `beta` - For beta releases from release/beta  
- `rc` - For release candidate builds from release/rc
- `stable` - For stable releases from main

### Required Secrets

Configure these secrets in your repository settings:
- `NUGET_API_KEY` - Your NuGet.org API key for package publishing

### Environment Protection Rules (Recommended)

Set up environment protection rules in GitHub:
- **stable**: Require review from maintainers
- **rc**: Require review from team leads
- **beta**: Automatic deployment
- **alfa**: Automatic deployment

## Usage Examples

### Creating a Beta Release
```bash
# From develop branch
git checkout develop
git pull origin develop

# Create beta branch
git checkout -b release/beta
git push origin release/beta
```

### Creating a Stable Release
```bash
# From release/rc branch
git checkout main
git merge release/rc
git tag v1.0.0
git push origin main --tags
```

### Hotfix Process
```bash
# Create hotfix from main
git checkout main
git checkout -b hotfix/fix-critical-issue
# Make fixes
git checkout main
git merge hotfix/fix-critical-issue
git tag v1.0.1
git push origin main --tags
```

## Package Consumption

Consumers can install different release stages:

```bash
# Stable release
dotnet add package Zentient.Expressions

# Pre-release versions
dotnet add package Zentient.Expressions --version 1.0.0-beta.123
dotnet add package Zentient.Expressions --version 1.0.0-rc.456
dotnet add package Zentient.Expressions --version 1.0.0-alfa.789
```

## Monitoring and Troubleshooting

- Monitor workflow runs in the **Actions** tab
- Check environment deployment status
- Review package publishing in NuGet.org
- Use GitHub releases for stable version tracking

## Benefits

1. **Clear Release Pipeline**: Each stage has a specific purpose and quality gate
2. **Automated Versioning**: Consistent version naming across all stages
3. **Quality Gates**: Environment protection rules ensure proper review
4. **Traceability**: Full audit trail from commit to package publication
5. **Flexibility**: Support for hotfixes and parallel development
6. **Rollback Safety**: Easy to revert or skip problematic releases