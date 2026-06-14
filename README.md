# Find Windows Fluent Icons from your agent.

[![NuGet](https://img.shields.io/nuget/v/fluent-icons-mcp.svg)](https://www.nuget.org/packages/fluent-icons-mcp)
[![NuGet downloads](https://img.shields.io/nuget/dt/fluent-icons-mcp.svg)](https://www.nuget.org/packages/fluent-icons-mcp)
[![Pack and Publish](https://github.com/airtaxi/FluentIconsMcp/actions/workflows/pack-and-publish.yml/badge.svg)](https://github.com/airtaxi/FluentIconsMcp/actions/workflows/pack-and-publish.yml)
[![License: MIT](https://img.shields.io/badge/license-MIT-blue.svg)](LICENSE.txt)

English | [한국어](README.ko.md)

fluent-icons-mcp is a .NET global tool that exposes Windows Fluent Icons metadata from [WinUI Gallery](https://github.com/microsoft/WinUI-Gallery) through a stdio MCP server. Local coding agents can call it to search icon codes by name, tags, or code.

## Highlights

- Search Windows Fluent Icons through MCP tools from Codex, Claude Code, GitHub Copilot CLI, Gemini CLI, or any provider that supports stdio MCP servers.
- Register or remove the server with supported provider CLIs using `mcp-install`, `mcp-remove`, or `mcp-uninstall`.
- Edit arbitrary `mcp.json` files directly for providers that do not have a built-in installer command.
- Download the latest WinUI Gallery `IconsData.json` when online and cache it for offline use.
- Return icon metadata with code, name, tags, and `isSegoeFluentOnly` information.

## Install

```powershell
dotnet tool install --global fluent-icons-mcp
```

Update an existing install:

```powershell
dotnet tool update --global fluent-icons-mcp
```

## Provider Setup

Use a supported provider CLI:

```powershell
fluent-icons-mcp mcp-install --provider codex
fluent-icons-mcp mcp-install --provider claude
fluent-icons-mcp mcp-install --provider copilot
fluent-icons-mcp mcp-install --provider gemini
fluent-icons-mcp mcp-install --provider all
```

Remove the registration:

```powershell
fluent-icons-mcp mcp-remove --provider codex
fluent-icons-mcp mcp-uninstall --provider claude
fluent-icons-mcp mcp-remove --provider all
```

The default MCP server name is `fluent-icons`. Override it when needed:

```powershell
fluent-icons-mcp mcp-install --provider codex --name fluent-icons
```

## Custom mcp.json

For other providers, pass the target MCP config file directly:

```powershell
fluent-icons-mcp mcp-install --config "C:\path\to\mcp.json"
```

This adds:

```json
{
  "mcpServers": {
    "fluent-icons": {
      "command": "fluent-icons-mcp",
      "args": []
    }
  }
}
```

The config editor accepts JSON comments and trailing commas when reading. It writes standard formatted JSON and creates a `.bak` file before modifying an existing config.

## MCP Tools

- `search_fluent_icons`: Searches Windows Fluent Icons by `name`, `tags`, or `code` and returns matching icon metadata.

The tool accepts optional `name`, `tags`, `code`, and `maxResults` parameters. Name and tag matching is case-insensitive and partial. Multiple tag phrases can be separated with commas or semicolons, and every phrase must match at least one icon tag.

The icon data is downloaded from WinUI Gallery when the server starts and cached at `%LOCALAPPDATA%\FluentIconsMcp\IconsData.json`. If the download fails, the server uses the cached file.

## Manual Server Command

Running without arguments starts the stdio MCP server:

```powershell
fluent-icons-mcp
```

Show CLI help:

```powershell
fluent-icons-mcp help
fluent-icons-mcp --help
```

## Package Development

```powershell
dotnet restore
dotnet build
dotnet test
dotnet pack
```

## License

fluent-icons-mcp is licensed under the [MIT License](LICENSE.txt).

Fluent Icons metadata is sourced from [microsoft/WinUI-Gallery](https://github.com/microsoft/WinUI-Gallery). See [THIRD-PARTY-NOTICES.md](THIRD-PARTY-NOTICES.md).

## Author

Created by [Howon Lee (airtaxi)](https://github.com/airtaxi).

Built with help from OpenAI Codex.
