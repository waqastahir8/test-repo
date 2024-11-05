using AmeriCorps.Data;
using AmeriCorps.Users.Data.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace AmeriCorps.Users.Data;

public sealed partial class RoleRepository(
    ILogger<RoleRepository> logger,
    IContextFactory contextFactory,
    IOptions<UserContextOptions> options
) : RepositoryBase<RepositoryContext>(logger, contextFactory, options), IRoleRepository
{
    public async Task<Role?> GetAsync(int id) =>
        await ExecuteAsync(async context => await context.Roles.FindAsync(id));

    public async Task<List<Role>?> GetRoleListByTypeAsync(string roleType) =>
        await ExecuteAsync(async context => await context.Roles.Where(o => o.RoleType == roleType).ToListAsync());

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

    public async Task<Role?> UpdateRoleAsync(Role role)
    {
        Role? updateRole = null;
        await ExecuteAsync(async context =>
        {
            await using var transaction = await context.Database.BeginTransactionAsync();
            try
            {
                context.Attach(role);
                context.Entry(role).State = EntityState.Modified;
                updateRole = UpdateRole(role, context);
                await transaction.CommitAsync();
                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                logger.LogWarning($"Could not save changes to repo: {ex}.  Rolling back.");
                await transaction.RollbackAsync();
            }
        });

        return updateRole;
    }

    private Role? UpdateRole(Role role, RepositoryContext context)

    {
        var roleId = role.Id;

        return role;
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

    public async Task<Role?> GetRoleByNameAsync(string roleName) =>
        await ExecuteAsync(async context => await context.Roles.FirstOrDefaultAsync(r => r.RoleName == roleName));
}