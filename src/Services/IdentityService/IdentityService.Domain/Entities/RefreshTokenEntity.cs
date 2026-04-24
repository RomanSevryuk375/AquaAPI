using Contracts.Abstractions;

namespace IdentityService.Domain.Entities;

public class RefreshTokenEntity : IEntity
{
    private RefreshTokenEntity(
        Guid id,
        Guid userId,
        string token,
        bool isUsed,
        bool isRevoked,
        DateTime expiredAt,
        DateTime createdAt)
    {
        Id = id;
        UserId = userId;
        Token = token;
        IsUsed = isUsed;
        IsRevoked = isRevoked;
        ExpiredAt = expiredAt;
        CreatedAt = createdAt;
    }

    public Guid Id { get; private set; }
    public Guid UserId { get; private set; }
    public string Token { get; private set; }
    public bool IsUsed { get; private set; }
    public bool IsRevoked { get; private set; }
    public DateTime ExpiredAt { get; private set; }
    public DateTime CreatedAt { get; private set; }

    public static RefreshTokenEntity Create(Guid userId, string token)
    {
        return new RefreshTokenEntity(
            Guid.NewGuid(),
            userId,
            token,
            false,
            false,
            DateTime.UtcNow.AddDays(30),
            DateTime.UtcNow);
    }

    public void MarkAsUsed()
    {
        IsUsed = true;
    }

    public void MarkAsRevoked()
    {
        IsRevoked = true;
    }
}
