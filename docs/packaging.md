# Packaging and CI

This project uses GitHub Actions to build, test and pack NuGet packages.

Key points

- CI runs on pushes to develop and release/* branches, and on PRs targeting main.
- Tag-based releases (refs/tags/v*.*.*) produce final packages and a GitHub Release.
- Branch-based prereleases (refs/heads/release/*) produce prerelease PackageVersion values that include the branch label and CI metadata. The pipeline will create a prerelease GitHub Release and may push prerelease packages to NuGet if NUGET_API_KEY is configured.

Ensure you have the following in .csproj for packages you want to publish:

<PropertyGroup>
  <IsPackable>true</IsPackable>
  <VersionPrefix>0.1.0</VersionPrefix>
</PropertyGroup>

Test projects should set IsPackable to false.