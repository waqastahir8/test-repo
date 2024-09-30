using AmeriCorps.Data;
using AmeriCorps.Users.Data.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Linq.Expressions;

namespace AmeriCorps.Users.Data;

public sealed partial class UserRepository(
    ILogger<UserRepository> logger,
    IContextFactory contextFactory,
    IOptions<UserContextOptions> options
) : RepositoryBase<RepositoryContext, UserContextOptions>(logger, contextFactory, options),
    IUserRepository
{
    public async Task<User?> GetAsync(int id) =>
        await ExecuteAsync(async context =>
            await context.Users
                .AsNoTracking()
                .Include(u => u.Attributes)
                .Include(u => u.Languages)
                .Include(u => u.Addresses)
                .Include(u => u.Education)
                .Include(u => u.Skills)
                .Include(u => u.MilitaryService)
                .Include(u => u.SavedSearches)
                .Include(u => u.Relatives)
                .Include(u => u.CommunicationMethods)
                .Include(u => u.Roles)
                .Include(u => u.UserProjects).ThenInclude(p => p.ProjectRoles)
                .Include(u => u.UserProjects).ThenInclude(a => a.ProjectAccess)
                .FirstOrDefaultAsync(x => x.Id == id));

    public async Task<User?> GetByExternalAccountIdAsync(string externalAccountId) =>
        await ExecuteAsync(async context =>
            await context.Users
                .AsNoTracking()
                .Include(u => u.Attributes)
                .Include(u => u.Languages)
                .Include(u => u.Addresses)
                .Include(u => u.Education)
                .Include(u => u.Skills)
                .Include(u => u.MilitaryService)
                .Include(u => u.SavedSearches)
                .Include(u => u.Relatives)
                .Include(u => u.CommunicationMethods)
                .Include(u => u.Roles)
                .Include(u => u.UserProjects).ThenInclude(p => p.ProjectRoles)
                .Include(u => u.UserProjects).ThenInclude(a => a.ProjectAccess)
                .FirstOrDefaultAsync(x => x.ExternalAccountId == externalAccountId));

    public async Task<IEnumerable<User>?> GetByAttributeAsync(string type, string value) =>
        await ExecuteAsync(async context =>
            await context.Users
                .AsNoTracking()
                .Include(u => u.Attributes)
                .Include(u => u.Languages)
                .Include(u => u.Addresses)
                .Include(u => u.Education)
                .Include(u => u.Skills)
                .Include(u => u.MilitaryService)
                .Include(u => u.SavedSearches)
                .Include(u => u.Relatives)
                .Include(u => u.CommunicationMethods)
                .Include(u => u.Roles)
                .Include(u => u.UserProjects).ThenInclude(p => p.ProjectRoles)
                .Include(u => u.UserProjects).ThenInclude(a => a.ProjectAccess)
                .Where(x => x.Attributes.Any(x => x.Type == type && x.Value == value))
                .ToListAsync());

    public async Task<int> GetUserIdByExternalAccountIdAsync(string externalAccountId)
    {
        return await ExecuteAsync(async context =>
        {
            var userId = await context.Users
                .AsNoTracking()
                .Where(x => x.ExternalAccountId == externalAccountId)
                .Select(u => u.Id)
                .FirstOrDefaultAsync();

            return userId;
        });
    }

    public async Task<List<SavedSearch>?> GetUserSearchesAsync(int id)
    {
        var user = await ExecuteAsync(async context => await context.Users
                                                    .Include(u => u.SavedSearches)
                                                    .FirstOrDefaultAsync(u => u.Id == id));

        return user?.SavedSearches;
    }

    public async Task<List<Reference>?> GetUserReferencesAsync(int id)
    {
        var user = await ExecuteAsync(async context => await context.Users
                                                    .Include(u => u.References)
                                                    .FirstOrDefaultAsync(u => u.Id == id));

        return user?.References;
    }

    public async Task<bool> ExistsAsync<T>(
            Expression<Func<T, bool>> predicate) where T : Entity =>
                await ExecuteAsync(async context =>
                {
                    IQueryable<T> data = context.Set<T>();
                    return await data.AnyAsync(predicate);
                });

    public async Task<T> SaveAsync<T>(T entity) where T : Entity =>
        await ExecuteAsync(async context =>
        {
            var entry = context.Entry(entity);

            entry.State = entity.Id == 0 ? EntityState.Detached : EntityState.Modified;

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

    public async Task<User?> UpdateUserAsync(User user)
    {
        User? upatedUser = null;
        await ExecuteAsync(async context =>
        {
            await using var transaction = await context.Database.BeginTransactionAsync();
            try
            {
                context.Attach(user);
                context.Entry(user).State = EntityState.Modified;
                upatedUser = UpdateUser(user, context);
                await transaction.CommitAsync();
                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                logger.LogWarning($"Could not save changes to repo: {ex}.  Rolling back.");
                await transaction.RollbackAsync();
            }
        });

        return upatedUser;
    }

    public async Task<bool> DeleteAsync<T>(int id) where T : Entity =>
        await ExecuteAsync(async context =>
        {
            T? e = await context.Set<T>().FindAsync(id);

            if (e != null)
            {
                context.Set<T>().Remove(e);
                await context.SaveChangesAsync();
                return true;
            }

            return false;
        });

    private static User? UpdateUser(User user, DbContext context)

    {
        // Refactor this later
        var userId = user.Id;
        UpdateEntities(user.Education, context, userId);
        UpdateEntities(user.Addresses, context, userId);
        UpdateEntities(user.Attributes, context, userId);
        UpdateEntities(user.CommunicationMethods, context, userId);
        UpdateEntities(user.MilitaryService, context, userId);
        UpdateEntities(user.Relatives, context, userId);
        UpdateEntities(user.Languages, context, userId);
        UpdateEntities(user.Skills, context, userId);
        UpdateEntities(user.Roles, context, userId);
        UpdateEntities(user.UserProjects, context, userId);

        return user;
    }

    private static void UpdateEntities<T>(List<T> entities, DbContext context, int userId) where T : EntityWithUserId
    {
        var entityIds = new HashSet<int>(entities.Select(e => e.Id));

        entities.ForEach(item =>
        {
            context.Entry(item).State = item.Id switch
            {
                0 => EntityState.Added,
                > 0 => EntityState.Modified,
                _ => EntityState.Unchanged
            };
        });

        var entitiesToDelete = context.Set<T>()
            .Where(e => e.UserId == userId && !entityIds.Contains(e.Id));

        foreach (var entity in entitiesToDelete)
        {
            context.Entry(entity).State = EntityState.Deleted;
        }
    }

    public async Task<Role?> GetRoleAsync(int id) =>
       await ExecuteAsync(async context => await context.Roles.FindAsync(id));

    private Role? UpdateRole(Role role, RepositoryContext context)

    {
        var roleId = role.Id;

        return role;
    }

    public async Task<UserList> FetchUserListByOrgCodeAsync(string orgCode)
    {
        List<User> users = await ExecuteAsync(async context =>
                await context.Users
                    .AsNoTracking()
                    .Include(u => u.Attributes)
                    .Include(u => u.Languages)
                    .Include(u => u.Addresses)
                    .Include(u => u.Education)
                    .Include(u => u.Skills)
                    .Include(u => u.MilitaryService)
                    .Include(u => u.SavedSearches)
                    .Include(u => u.Relatives)
                    .Include(u => u.CommunicationMethods)
                    .Include(u => u.Roles)
                    .Include(u => u.UserProjects).ThenInclude(p => p.ProjectRoles)
                    .Include(u => u.UserProjects).ThenInclude(a => a.ProjectAccess)
                    .Where(x => x.OrgCode == orgCode).ToListAsync());

        UserList? userList = new UserList()
        {
            OrgCode = orgCode,
            Users = users
        };

        return userList;
    }

    public async Task<List<User>> FetchInvitedUsersForReminder() =>
        await ExecuteAsync(async context => await context.Users.Where(u => u.AccountStatus == "INVITED").ToListAsync());

    // ConstanstsService.Invited
    // public async Task<List<User> FetchInvitedUsersForReminder()
    // {
    //     return context.Users
    //                 .AsNoTracking()
    //                 .Include(u => u.Attributes)
    //                 .Include(u => u.Languages)
    //                 .Include(u => u.Addresses)
    //                 .Include(u => u.Education)
    //                 .Include(u => u.Skills)
    //                 .Include(u => u.MilitaryService)
    //                 .Include(u => u.SavedSearches)
    //                 .Include(u => u.Relatives)
    //                 .Include(u => u.CommunicationMethods)
    //                 .Include(u => u.Roles)
    //                 .Include(u => u.UserProjects).ThenInclude(p => p.ProjectRoles)
    //                 .Include(u => u.UserProjects).ThenInclude(a => a.ProjectAccess)
    //                 .Where(x => x.OrgCode == orgCode).ToListAsync());

    //     // return users;
    // }
}