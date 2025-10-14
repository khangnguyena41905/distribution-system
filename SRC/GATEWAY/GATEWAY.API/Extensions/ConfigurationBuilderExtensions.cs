namespace GATEWAY.API.Extensions;
using Microsoft.Extensions.Configuration;
using System.Text.Json;
public static class ConfigurationBuilderExtensions
{
    public static IConfigurationBuilder AddOcelotConfigFolder(this IConfigurationBuilder configuration, string folderPath)
    {
        var fullPath = Path.GetFullPath(folderPath);
        if (!Directory.Exists(fullPath))
            throw new DirectoryNotFoundException($"Không tìm thấy thư mục cấu hình: {fullPath}");

        var files = Directory.GetFiles(fullPath, "ocelot.*.json");

        var mergedRoutes = new List<JsonElement>();
        JsonElement? globalConfig = null;
        JsonElement? swaggerEndpoints = null;

        foreach (var file in files)
        {
            using var doc = JsonDocument.Parse(File.ReadAllText(file));
            var root = doc.RootElement;

            if (root.TryGetProperty("Routes", out var routes))
            {
                foreach (var route in routes.EnumerateArray())
                {
                    var clone = JsonDocument.Parse(route.GetRawText()).RootElement;
                    mergedRoutes.Add(clone);
                }
            }

            if (root.TryGetProperty("GlobalConfiguration", out var global))
            {
                globalConfig = JsonDocument.Parse(global.GetRawText()).RootElement;
            }

            if (root.TryGetProperty("SwaggerEndPoints", out var swagger))
            {
                swaggerEndpoints = JsonDocument.Parse(swagger.GetRawText()).RootElement;
            }
        }

        var resultObject = new Dictionary<string, object>
        {
            { "Routes", mergedRoutes }
        };

        if (globalConfig.HasValue)
            resultObject["GlobalConfiguration"] = globalConfig;

        if (swaggerEndpoints.HasValue)
            resultObject["SwaggerEndPoints"] = swaggerEndpoints;

        var jsonString = JsonSerializer.Serialize(resultObject);

        var memoryStream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(jsonString));
        configuration.AddJsonStream(memoryStream);

        return configuration;
    }
}
