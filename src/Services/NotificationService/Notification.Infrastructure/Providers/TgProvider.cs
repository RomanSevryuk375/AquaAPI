using Microsoft.Extensions.Configuration;
using Notification.Domain.Entities;
using Notification.Domain.Interfaces;

namespace Notification.Infrastructure.Providers;

public class TgProvider(
    HttpClient httpClient,
    IConfiguration configuration) : INotificationProvider
{
    public bool IsEnabled(UserEntity user)
    {
        if (user.TgEnable && user.TelegramChatId.HasValue)
        {
            return true;
        }

        return false;
    }

    public async Task<(bool Success, string Error)> SendAsync(
        UserEntity user, string message, CancellationToken cancellationToken)
    {
        var token = configuration["Telegram:BotToken"];
        var chatId = user.TelegramChatId!.Value.ToString();

        var url = $"https://api.telegram.org/bot{token}/sendMessage?chat_id={chatId}&text={Uri.EscapeDataString(message)}";

        try
        {
            var response = await httpClient.GetAsync(url, cancellationToken);
            return (response.IsSuccessStatusCode, "Ok");
        }
        catch (Exception ex)
        {
            return (false, $"{ex.Message} {ex.StackTrace}");
        }
    }
}
