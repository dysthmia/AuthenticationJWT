using Domain;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure;

public class UserRepository : IUserRepository
{
    private readonly AuthDbContext _db;
    public UserRepository(AuthDbContext db) => _db = db;

    public async Task AddUserAsync(User user, CancellationToken ct)
    {
        await _db.AddAsync(user, ct);
    }

    public Task<bool> ExistsByIdAsync(UserId id, CancellationToken ct)
    {
        return _db.Users.AnyAsync(u => u.Id == id, ct);
    }

    public Task<bool> ExistsByMailAsync(string email, CancellationToken ct)
    {
        return _db.Users.AnyAsync(x => x.PersonalData.Email == email, ct);
    }

    public Task<User?> GetByEmailAsync(string email, CancellationToken ct)
    {
        return _db.Users.FirstOrDefaultAsync(x => x.PersonalData.Email == email, ct);
    }

    public Task<User?> GetByIdAsync(UserId id, CancellationToken ct)
    {
        return _db.Users.FirstOrDefaultAsync(u => u.Id == id, ct);
    }

    public async Task UpdateUser(User user)
    {
        _db.Update(user);
    }
}