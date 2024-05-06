using AmeriCorps.Data;
using AmeriCorps.Users.Data.Core;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace AmeriCorps.Users.Data;

public interface IUserRepository
{
    Task<User?> GetAsync(int id);

    Task<User?> GetByExternalAcctId(string ExternalAccountId);
    Task<List<SavedSearch>?> GetUserSearchesAsync(int id);
    Task<List<Reference>?> GetUserReferencesAsync(int id);
    Task<bool> ExistsAsync<T>(Expression<Func<T, bool>> predicate = null) where T : Entity;
    Task<T> SaveAsync<T>(T entity) where T : Entity;
    Task DeleteAsync<T>(int id) where T : Entity;
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
    public async Task<User?> GetByExternalAcctId(string externalAccountId) =>
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
                                .FirstOrDefaultAsync(x => x.ExternalAccountId == externalAccountId));

    public async Task<List<SavedSearch>?> GetUserSearchesAsync(int id)
    {
        var user = await ExecuteAsync(async context => await context.Users
                                                    .Include(u => u.SavedSearches)
                                                    .FirstOrDefaultAsync(u => u.Id == id));

        return user != null ? user.SavedSearches : null;
    }
    public async Task<List<Reference>?> GetUserReferencesAsync(int id)
    {
        var user = await ExecuteAsync(async context => await context.Users
                                                    .Include(u => u.References)
                                                    .FirstOrDefaultAsync(u => u.Id == id));

        return user != null ? user.References : null;
    }

    public async Task<bool> ExistsAsync<T>(
            Expression<Func<T, bool>> predicate = null) where T : Entity =>
                await ExecuteAsync(async context =>
                {
                    IQueryable<T> data = context.Set<T>();
                    return await data.AnyAsync(predicate);
                });

    public async Task<T> SaveAsync<T>(T entity) where T : Entity =>
        await ExecuteAsync(async context =>
        {
            var entry = context.Entry(entity);

            entry.State = entity.Id == 0 ?
                                EntityState.Detached :
                                EntityState.Modified;

            if (entity.Id == 0)
            {
                entry.State = EntityState.Detached;
                await context.Set<T>().AddAsync(entity);
            }
            else
            {
                entry.State = EntityState.Modified;
                context.Set<T>().Update(entity);
            }

            await context.SaveChangesAsync();
            return entity;
        });

    public async Task DeleteAsync<T>(int id) where T : Entity =>
        await ExecuteAsync(async context =>
        {
            T e = await context.Set<T>().FindAsync(id);

            if (e != null)
            {
                context.Set<T>().Remove(e);
                await context.SaveChangesAsync();
                return true;
            }

            return false;
        });
}