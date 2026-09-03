namespace Domain;

public interface IRefreshTokenRepository
{
    Task<RefreshToken?> GetByTokenHashAsync(string tokenHash, CancellationToken ct);
    Task AddTokenAsync (RefreshToken refreshToken, CancellationToken ct);
    Task RevokedAllForUserAsync(UserId userId, string revokedByIp, CancellationToken ct);
}