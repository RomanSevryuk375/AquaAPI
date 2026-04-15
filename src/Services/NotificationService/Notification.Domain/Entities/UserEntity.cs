using Notification.Domain.Interfaces;
using System.Text.RegularExpressions;

namespace Notification.Domain.Entities;

public class UserEntity : IEntity
{
    private UserEntity(
        Guid id,
        string email,
        string phonenumber,
        bool emailEnable,
        bool tgEnable,
        long? telegramChatId,
        bool enable,
        DateTime creayedAt)
    {
        Id = id;
        Email = email;
        PhoneNumber = phonenumber;
        EmailEnable = emailEnable;
        TgEnable = tgEnable;
        TelegramChatId = telegramChatId;
        IsNotifyEnable = enable;
        CreatedAt = creayedAt;
    }

    public Guid Id { get; private set; }
    public string Email { get; private set; } = string.Empty;
    public string PhoneNumber { get; private set; } = string.Empty;
    public bool EmailEnable { get; private set; }
    public bool TgEnable { get; private set; }
    public long? TelegramChatId { get; private set; }
    public bool IsNotifyEnable { get; private set; }
    public DateTime CreatedAt { get; private set; }

    public static (UserEntity? user, List<string> errors) Create(
        Guid id,
        string email,
        string phoneNumber,
        bool emailEnable,
        bool tgEnable,
        long? telegramChatId,
        bool enable,
        DateTime createdAt)
    {
        var errors = new List<string>();

        if (id == Guid.Empty)
        {
            errors.Add("id must not be empty.");
        }

        if (string.IsNullOrWhiteSpace(email))
        {
            errors.Add("email must not be empty.");
        }

        if (string.IsNullOrWhiteSpace(phoneNumber))
        {
            errors.Add("phonenumber must not be empty.");
        }

        if (!Regex.IsMatch(phoneNumber, @"^(\+375|80)(29|44|33|25)\d{7}$"))
        {
            errors.Add("Phone number should be in format +375XXXXXXXXX or 80XXXXXXXXX");
        }

        if (tgEnable is true && telegramChatId is null)
        {
            errors.Add("telegramChatId must not be empty if tgEnable is true.");
        }

        if (errors.Count > 0)
        {
            return (null, errors);
        }

        var user = new UserEntity(
            id, 
            email,
            phoneNumber,
            emailEnable,
            tgEnable,
            telegramChatId,
            enable,
            createdAt);

        return (user, errors);
    }

    public string? UpdateContacts(string email, string phoneNumber)
    {
        Email = email.Trim();

        if (!Regex.IsMatch(phoneNumber, @"^(\+375|80)(29|44|33|25)\d{7}$"))
        {
            return("Phone number should be in format +375XXXXXXXXX or 80XXXXXXXXX");
        }

        PhoneNumber = phoneNumber.Trim();

        return null;
    }

    public void SetNotificationPreferences(bool emailEnable, bool tgEnable, long? tgChatId)
    {
        EmailEnable = emailEnable;
        TgEnable = tgEnable;
        TelegramChatId = tgChatId;
    }
}
