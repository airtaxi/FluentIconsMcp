using FluentIconsMcp.CommandExecution;

namespace FluentIconsMcp.Tests.CommandExecution;

public sealed class ProviderCommandBuilderTests
{
    [Fact]
    public void CreateInstallCommandCreatesCodexCommand()
    {
        var command = ProviderCommandBuilder.CreateInstallCommand(McpProvider.Codex, FluentIconsMcpConstants.DefaultServerName, FluentIconsMcpConstants.ToolCommandName);

        Assert.Equal("codex", command.FileName);
        Assert.Equal(["mcp", "add", "fluent-icons", "--", "fluent-icons-mcp"], command.Arguments);
    }

    [Fact]
    public void CreateRemoveCommandCreatesClaudeCommand()
    {
        var command = ProviderCommandBuilder.CreateRemoveCommand(McpProvider.Claude, FluentIconsMcpConstants.DefaultServerName);

        Assert.Equal("claude", command.FileName);
        Assert.Equal(["mcp", "remove", "--scope", "user", "fluent-icons"], command.Arguments);
    }
}
