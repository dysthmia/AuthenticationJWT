namespace Domain;

public interface IUserRepository
{
    Task<User?> GetByIdAsync (UserId id, CancellationToken ct);
    Task<User?> GetByEmailAsync (string email, CancellationToken ct);

    Task<bool> ExistsByIdAsync (UserId id, CancellationToken ct);
    Task<bool> ExistsByMailAsync (string email, CancellationToken ct);
    
    Task AddUserAsync(User user, CancellationToken ct);
    Task UpdateUser (User user);
}