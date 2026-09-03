namespace Domain;

public class RefreshToken
{
    public Guid TokenId { get; private set; }
    public UserId UserId { get; private set; }

    public string TokenHash { get; private set; }
    public string? ReplacedByTokenHash { get; private set; }

    public DateTimeOffset ExpiresAt { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }
    public DateTimeOffset? RevokedAt { get; private set; }

    public string CreatedByIp { get; private set; }
    public string? RevokedByIp { get; private set; }

    private RefreshToken(
        Guid tokenId,
        UserId userId,
        string tokenHash,
        DateTimeOffset expiresAt,
        DateTimeOffset createdAt,
        string createdByIp)
    {
        TokenId = tokenId;
        UserId = userId;
        TokenHash = tokenHash;
        ExpiresAt = expiresAt;
        CreatedAt = createdAt;
        CreatedByIp = createdByIp;
    }

    // возможно понадобится для EFcore
    // private RefreshToken()
    // {
    //     TokenHash = null!;
    //     CreatedByIp = null!;
    // }

    public static RefreshToken Create(
        UserId userId,
        string tokenHash,
        TimeSpan lifetime,
        string createdByIp)
    {
        if (string.IsNullOrWhiteSpace(tokenHash) ||
            string.IsNullOrWhiteSpace(createdByIp))
            throw new ArgumentException("createdByIp is wrong or null");

        if (lifetime <= TimeSpan.Zero)
            throw new ArgumentException("lifetime must be positive");

        return new RefreshToken(
            Guid.NewGuid(),
            userId,
            tokenHash,
            DateTimeOffset.UtcNow.Add(lifetime),
            DateTimeOffset.UtcNow,
            createdByIp);
    }

    // для восстановления из БД
    public static RefreshToken Restore(
        Guid tokenId,
        UserId userId,
        string tokenHash,
        string? replacedByTokenHash,
        DateTimeOffset expiresAt,
        DateTimeOffset createdAt,
        DateTimeOffset? revokedAt,
        string createdByIp,
        string? revokedByIp)
    {
        return new RefreshToken(tokenId, userId, tokenHash, expiresAt, createdAt, createdByIp)
        {
            ReplacedByTokenHash = replacedByTokenHash,
            RevokedAt = revokedAt,
            RevokedByIp = revokedByIp
        };
    }

    public bool IsExpired => DateTimeOffset.UtcNow >= ExpiresAt;
    public bool IsRevoked => RevokedAt is not null;
    public bool IsActive => !IsExpired && !IsRevoked;

    public void Revoke(string revokedByIp, string? replacedByTokenHash = null)
    {
        if (IsRevoked) return;

        if (string.IsNullOrWhiteSpace(revokedByIp))
            throw new ArgumentException("revokedByIp is required");

        RevokedAt = DateTimeOffset.UtcNow;
        RevokedByIp = revokedByIp;
        ReplacedByTokenHash = replacedByTokenHash;
    }
}