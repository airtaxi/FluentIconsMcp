using System.ComponentModel;
using System.Text.Json;
using FluentIconsMcp.Models;
using FluentIconsMcp.Services;
using ModelContextProtocol.Server;

namespace FluentIconsMcp.Mcp;

[McpServerToolType]
public static class FluentIconsTools
{
    private static readonly JsonSerializerOptions s_jsonSerializerOptions = new(JsonSerializerDefaults.Web) { WriteIndented = true };

    [McpServerTool(Name = "search_fluent_icons", Title = "Search Fluent Icons", ReadOnly = true, Destructive = false), Description("Search Windows Fluent Icons by name, tags, or code. Returns matching icons with their code, name, and tags.")]
    public static string SearchFluentIcons(FluentIconsDataService iconDataService, [Description("Optional name filter. Matches icon names containing this value (case-insensitive).")] string? name = null, [Description("Optional tags filter. Supports multiple tag phrases separated by comma or semicolon. Each phrase is matched partially against icon tags (case-insensitive), and all phrases must match.")] string? tags = null, [Description("Optional code filter. Matches the exact icon code, e.g. 'E700', 'U+E700', or '0xE700'.")] string? code = null, [Description("Maximum number of results to return. Default is 50.")] int maxResults = 50)
    {
        var icons = iconDataService.Icons;
        var queryName = name?.Trim();
        var queryCode = NormalizeIconCode(code);
        var queryTags = ParseTags(tags);

        var matchingIcons = icons.Where(icon => IsMatch(icon, queryName, queryTags, queryCode)).ToList();
        var resultLimit = Math.Clamp(maxResults, 1, 200);
        var response = new FluentIconsSearchResponse(matchingIcons.Count, Math.Min(matchingIcons.Count, resultLimit), iconDataService.CacheFilePath, matchingIcons.Take(resultLimit).ToList());

        return JsonSerializer.Serialize(response, s_jsonSerializerOptions);
    }

    private static List<string> ParseTags(string? tags)
    {
        if (string.IsNullOrWhiteSpace(tags)) return [];

        return tags.Split([',', ';', '\n', '\r'], StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .Select(tag => tag.ToUpperInvariant())
            .Distinct()
            .ToList();
    }

    private static string? NormalizeIconCode(string? code)
    {
        if (string.IsNullOrWhiteSpace(code)) return null;

        var normalizedCode = code.Trim().ToUpperInvariant();
        if (normalizedCode.StartsWith("U+", StringComparison.OrdinalIgnoreCase)) normalizedCode = normalizedCode[2..];
        if (normalizedCode.StartsWith("0X", StringComparison.OrdinalIgnoreCase)) normalizedCode = normalizedCode[2..];
        if (normalizedCode.StartsWith("\\U", StringComparison.OrdinalIgnoreCase)) normalizedCode = normalizedCode[2..];
        return normalizedCode;
    }

    private static bool IsMatch(FluentIcons icon, string? queryName, IReadOnlyList<string> queryTags, string? queryCode)
    {
        if (!string.IsNullOrEmpty(queryCode) && !string.Equals(icon.Code, queryCode, StringComparison.OrdinalIgnoreCase)) return false;
        if (!string.IsNullOrEmpty(queryName) && !icon.Name.Contains(queryName, StringComparison.OrdinalIgnoreCase)) return false;
        if (queryTags.Count > 0 && !queryTags.All(queryTag => icon.Tags.Any(iconTag => iconTag.Contains(queryTag, StringComparison.OrdinalIgnoreCase)))) return false;
        return true;
    }

    private sealed record FluentIconsSearchResponse(int TotalMatches, int ReturnedCount, string CacheFilePath, IReadOnlyList<FluentIcons> Icons);
}
