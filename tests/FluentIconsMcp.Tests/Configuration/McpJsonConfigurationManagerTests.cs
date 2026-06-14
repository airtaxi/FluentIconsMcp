using System.Text.Json.Nodes;
using FluentIconsMcp.Configuration;

namespace FluentIconsMcp.Tests.Configuration;

public sealed class McpJsonConfigurationManagerTests
{
    [Fact]
    public void InstallCreatesServerConfiguration()
    {
        using var temporaryDirectory = new TemporaryDirectory();
        var configPath = Path.Combine(temporaryDirectory.Path, "mcp.json");
        var manager = new McpJsonConfigurationManager();

        var changed = manager.Install(configPath, FluentIconsMcpConstants.DefaultServerName, FluentIconsMcpConstants.ToolCommandName);

        Assert.True(changed);
        var rootObject = JsonNode.Parse(File.ReadAllText(configPath))!.AsObject();
        var serverObject = rootObject["mcpServers"]![FluentIconsMcpConstants.DefaultServerName]!.AsObject();
        Assert.Equal(FluentIconsMcpConstants.ToolCommandName, serverObject["command"]!.GetValue<string>());
        Assert.Empty(serverObject["args"]!.AsArray());
    }

    [Fact]
    public void RemoveDeletesServerConfiguration()
    {
        using var temporaryDirectory = new TemporaryDirectory();
        var configPath = Path.Combine(temporaryDirectory.Path, "mcp.json");
        var manager = new McpJsonConfigurationManager();
        manager.Install(configPath, FluentIconsMcpConstants.DefaultServerName, FluentIconsMcpConstants.ToolCommandName);

        var changed = manager.Remove(configPath, FluentIconsMcpConstants.DefaultServerName);

        Assert.True(changed);
        var rootObject = JsonNode.Parse(File.ReadAllText(configPath))!.AsObject();
        Assert.Null(rootObject["mcpServers"]![FluentIconsMcpConstants.DefaultServerName]);
    }
}
