using FedTec.Data;
using AmeriCorps.Users.Data.Core;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;

namespace AmeriCorps.Users.Data;

public interface IUserRepository
{
    Task<User?> GetAsync(int id);
    Task<List<SavedSearch>?> GetUserSearchesAsync(int id);
    Task<bool> ExistsAsync(int id);
    Task<User> CreateAsync(User user);
    Task<SavedSearch> SaveAsync(SavedSearch search);
    Task DeleteSearchAsync(int id);
}
public sealed class UserRepository(
    ILogger<UserRepository> logger,
    IContextFactory contextFactory,
    IOptions<UserContextOptions> options
) : RepositoryBase<RepositoryContext, UserContextOptions>(logger, contextFactory, options), IUserRepository
{
    public async Task<User?> GetAsync(int id) =>
        await ExecuteAsync(async context => await context.Users
                                        .Include(u => u.Attributes)
                                        .Include(u => u.Languages)
                                        .Include(u => u.Addresses)
                                        .Include(u => u.Education)
                                        .Include(u => u.Skills)
                                        .Include(u => u.MilitaryService)
                                        .Include(u => u.SavedSearches)
                                        .Include(u => u.Relatives)
                                        .Include(u => u.CommunicationMethods)
                                        .FirstOrDefaultAsync(x => x.Id == id));
    public async Task<List<SavedSearch>?> GetUserSearchesAsync(int id)
    {
        var user = await ExecuteAsync(async context => await context.Users
                                                    .Include(u => u.SavedSearches)
                                                    .FirstOrDefaultAsync(x => x.Id == id));

        return user != null ? user.SavedSearches : null;
    }
    public async Task<bool> ExistsAsync(int id) =>
            await ExecuteAsync(async context =>
                    await context.Users.AnyAsync(u => u.Id == id));
    public async Task<User> CreateAsync(User user)
    {
        await ExecuteAsync(async context =>
        {
            context.Users.Add(user);
            await context.SaveChangesAsync();
        });

        return user;
    }

    public async Task<SavedSearch> SaveAsync(SavedSearch search) =>
        await ExecuteAsync(async context =>
        {

            SavedSearch s;
            if (search.Id == default)
            {
                s = (await context.SavedSearch.AddAsync(search)).Entity;
            }
            else
            {
                context.Update(search);
                s = search;
            }

            await context.SaveChangesAsync();
            return s;
        });

    public async Task DeleteSearchAsync(int id) =>
        await ExecuteAsync(async context =>
        {
            SavedSearch? s = await context.SavedSearch.FindAsync(id);

            if (s != null)
            {
                context.Remove(s);
                await context.SaveChangesAsync();
                return true;
            }

            return false;
        });
}