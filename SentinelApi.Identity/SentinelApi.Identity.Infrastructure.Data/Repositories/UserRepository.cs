using Microsoft.EntityFrameworkCore;
using SentinelApi.Identity.Application.Interfaces.Infrastructure;
using SentinelApi.Identity.Domain.Entities;
using SentinelApi.Identity.Infrastructure.Data.DbContexts;

namespace SentinelApi.Identity.Infrastructure.Data.Repositories;

public sealed class UserRepository(IdentityDbContext dbContext) : IUserRepository
{
    public Task<User?> FindByLoginAsync(string login, CancellationToken ct)
        => dbContext.Users.FirstOrDefaultAsync(u => u.Login == login, ct);

    public Task<bool> ExistsByLoginAsync(string login, CancellationToken ct)
        => dbContext.Users.AnyAsync(u => u.Login == login, ct);

    public async Task<IReadOnlyList<User>> GetAllAsync(CancellationToken ct)
        => await dbContext.Users.ToListAsync(ct);

    public async Task<IReadOnlyList<User>> GetAllWithActionLogsAsync(CancellationToken ct)
        => await dbContext.Users
            .Include(u => u.ActionLogsAsAuthor)
            .ToListAsync(ct);

    public async Task AddAsync(User user, CancellationToken ct)
    {
        dbContext.Users.Add(user);
        await dbContext.SaveChangesAsync(ct);
    }
}
