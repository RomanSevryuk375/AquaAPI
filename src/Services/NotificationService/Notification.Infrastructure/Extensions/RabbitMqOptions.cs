namespace Notification.Infrastructure.Extensions;

public record RabbitMqOptions
{
    public const string SectionName = "EmailOptions";
    public string Host { get; init; } = string.Empty;
    public string UserName { get; init; } = string.Empty;
    public string Password { get; init; } = string.Empty;
}
