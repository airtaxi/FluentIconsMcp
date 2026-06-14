using System.Net;
using System.Text.Json;
using FluentIconsMcp.Models;
using Microsoft.Extensions.Logging;

namespace FluentIconsMcp.Services;

public sealed class FluentIconsDataService(ILogger<FluentIconsDataService> logger)
{
    private const string RemoteDataUrl = "https://raw.githubusercontent.com/microsoft/WinUI-Gallery/refs/heads/main/WinUIGallery/Samples/Iconography/IconsData.json";
    private const string CacheDirectoryName = "FluentIconsMcp";
    private const string CacheFileName = "IconsData.json";

    private List<FluentIcons> _icons = [];

    public IReadOnlyList<FluentIcons> Icons => _icons;

    public string CacheFilePath => GetCacheFilePath();

    public async Task LoadIconsAsync(CancellationToken cancellationToken = default)
    {
        var cacheFilePath = GetCacheFilePath();
        var cacheDirectoryPath = Path.GetDirectoryName(cacheFilePath)!;

        if (!Directory.Exists(cacheDirectoryPath)) Directory.CreateDirectory(cacheDirectoryPath);

        var downloadedJson = await TryDownloadIconsJsonAsync(cancellationToken).ConfigureAwait(false);

        if (!string.IsNullOrWhiteSpace(downloadedJson))
        {
            if (TryParseIconsJson(downloadedJson, out var downloadedIcons))
            {
                try
                {
                    await File.WriteAllTextAsync(cacheFilePath, downloadedJson, cancellationToken).ConfigureAwait(false);
                    logger.LogInformation("Fluent icon cache updated at {CacheFilePath}", cacheFilePath);
                }
                catch (Exception exception) { logger.LogWarning(exception, "Failed to write icon cache to {CacheFilePath}", cacheFilePath); }

                _icons = downloadedIcons;
                return;
            }

            logger.LogWarning("Downloaded fluent icons data was invalid; falling back to cache");
        }

        if (File.Exists(cacheFilePath))
        {
            logger.LogInformation("Using cached fluent icons from {CacheFilePath}", cacheFilePath);
            var cachedJson = await File.ReadAllTextAsync(cacheFilePath, cancellationToken).ConfigureAwait(false);
            _icons = ParseIconsJson(cachedJson);
            return;
        }

        throw new InvalidOperationException($"Unable to download fluent icons data from {RemoteDataUrl} and no cache is available at {cacheFilePath}.");
    }

    private static string GetCacheFilePath()
    {
        var localApplicationDataPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        return Path.Combine(localApplicationDataPath, CacheDirectoryName, CacheFileName);
    }

    private async Task<string?> TryDownloadIconsJsonAsync(CancellationToken cancellationToken)
    {
        try
        {
            using var httpClient = new HttpClient();
            httpClient.Timeout = TimeSpan.FromSeconds(30);
            logger.LogInformation("Downloading fluent icons data from {RemoteDataUrl}", RemoteDataUrl);
            var json = await httpClient.GetStringAsync(RemoteDataUrl, cancellationToken).ConfigureAwait(false);
            logger.LogInformation("Fluent icon data downloaded successfully");
            return json;
        }
        catch (HttpRequestException exception) when (exception.StatusCode is null or >= HttpStatusCode.BadRequest) { logger.LogWarning(exception, "Failed to download fluent icons data; falling back to cache"); }
        catch (TaskCanceledException exception) { logger.LogWarning(exception, "Fluent icon data download timed out; falling back to cache"); }
        catch (Exception exception) { logger.LogWarning(exception, "Unexpected error while downloading fluent icons data; falling back to cache"); }

        return null;
    }

    private static List<FluentIcons> ParseIconsJson(string json)
    {
        var options = new JsonSerializerOptions(JsonSerializerDefaults.Web);
        var icons = JsonSerializer.Deserialize<List<FluentIcons>>(json, options);
        return icons ?? [];
    }

    private static bool TryParseIconsJson(string json, out List<FluentIcons> icons)
    {
        try
        {
            icons = ParseIconsJson(json);
            return true;
        }
        catch (JsonException)
        {
            icons = [];
            return false;
        }
    }
}
