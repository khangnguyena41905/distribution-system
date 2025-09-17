namespace GATEWAY.API.Extensions;
using Microsoft.Extensions.Configuration;
using System.Text.Json;

public static class ConfigurationBuilderExtensions
{
    public static IConfigurationBuilder AddOcelotConfigFolder(
        this IConfigurationBuilder configuration,
        string folderPath)
    {
        var fullPath = Path.GetFullPath(folderPath);

        if (!Directory.Exists(fullPath))
            throw new DirectoryNotFoundException($"Không tìm thấy thư mục cấu hình: {fullPath}");

        var jsonFiles = Directory.GetFiles(fullPath, "ocelot.*.json", SearchOption.TopDirectoryOnly);

        foreach (var file in jsonFiles)
        {
            configuration.AddJsonFile(file, optional: false, reloadOnChange: true);
        }

        return configuration;
    }
}