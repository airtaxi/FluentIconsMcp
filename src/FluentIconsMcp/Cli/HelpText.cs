namespace FluentIconsMcp.Cli;

public static class HelpText
{
    public static string Create()
    {
        return """
        fluent-icons-mcp

        Runs a stdio MCP server that searches Windows Fluent Icons from WinUI Gallery metadata.

        Usage:
          fluent-icons-mcp
          fluent-icons-mcp help
          fluent-icons-mcp mcp-install --provider codex|claude|copilot|gemini|all [--name fluent-icons]
          fluent-icons-mcp mcp-install --config <mcp.json> [--name fluent-icons]
          fluent-icons-mcp mcp-remove --provider codex|claude|copilot|gemini|all [--name fluent-icons]
          fluent-icons-mcp mcp-uninstall --provider codex|claude|copilot|gemini|all [--name fluent-icons]

        Providers:
          codex    Uses: codex mcp add/remove
          claude   Uses: claude mcp add/remove
          copilot  Uses: copilot mcp add/remove
          gemini   Uses: gemini mcp add/remove
          all      Runs every supported provider CLI and prints a result summary

        Options:
          --provider <name>  Install or remove through a supported provider CLI.
          --config <path>    Edit an arbitrary mcp.json directly for other providers.
          --name <name>      MCP server name. Defaults to fluent-icons.

        Examples:
          fluent-icons-mcp mcp-install --provider codex
          fluent-icons-mcp mcp-install --provider all
          fluent-icons-mcp mcp-install --config C:\path\to\mcp.json --name fluent-icons
          fluent-icons-mcp mcp-remove --provider claude
        """;
    }
}
