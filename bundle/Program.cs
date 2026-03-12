// This project exists solely to bundle FrejaOrgId and its NuGet dependencies
// into a single DLL for use in PowerShell modules.
//
// Build this project to produce dist/FrejaOrgId.dll:
//   dotnet build bundle/Bundle.csproj
//
// The output will NOT include .NET runtime files — PowerShell 7 provides those.

// Minimal entry point required by the SDK — not used at runtime.
static class Program { static void Main() { } }
