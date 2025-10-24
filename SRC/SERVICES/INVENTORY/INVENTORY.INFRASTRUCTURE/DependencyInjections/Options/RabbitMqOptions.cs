namespace INVENTORY.INFRASTRUCTURE.DependencyInjections.Options;

public class RabbitMqOptions
{
    public const string SectionName = "RabbitMQ";

    public string Host { get; set; } = default!;
    public int Port { get; set; } = 5672;
    public string VirtualHost { get; set; } = "/";
    public string Username { get; set; } = default!;
    public string Password { get; set; } = default!;
    public bool UseSsl { get; set; } = false;
}