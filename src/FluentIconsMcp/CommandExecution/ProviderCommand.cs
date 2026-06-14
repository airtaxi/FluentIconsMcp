namespace FluentIconsMcp.CommandExecution;

public sealed record ProviderCommand(string FileName, IReadOnlyList<string> Arguments);
