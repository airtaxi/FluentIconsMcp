namespace FluentIconsMcp.Models;

public sealed class FluentIcons
{
    public string Code { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    public List<string> Tags { get; set; } = [];

    public bool IsSegoeFluentOnly { get; set; }
}
