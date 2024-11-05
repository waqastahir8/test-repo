using AmeriCorps.Data;
using AmeriCorps.Users.Data.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace AmeriCorps.Users.Data;

public interface IAccessRepository
{

    Task<Access?> GetAsync(int id);

    Task<Access?> GetAccessByNameAsync(string accessName);

    Task<List<Access>?> GetAccessListByTypeAsync(string accessType);

    Task<List<Access>?> GetAccessListAsync();

    Task<T> SaveAsync<T>(T entity) where T : Entity;

}

public sealed partial class AccessRepository(
    ILogger<AccessRepository> logger,
    IContextFactory contextFactory,
    IOptions<UserContextOptions> options
) : RepositoryBase<RepositoryContext, UserContextOptions>(logger, contextFactory, options),
    IAccessRepository
{

    public async Task<Access?> GetAsync(int id) =>
        await ExecuteAsync(async context => await context.Access.FirstOrDefaultAsync(a => a.Id == id));

    public async Task<Access?> GetAccessByNameAsync(string accessName) =>
        await ExecuteAsync(async context => await context.Access.FirstOrDefaultAsync(a => a.AccessName == accessName));

    public async Task<List<Access>?> GetAccessListByTypeAsync(string accessType) =>
        await ExecuteAsync(async context => await context.Access.Where(o => o.AccessType == accessType).ToListAsync());

    public async Task<List<Access>?> GetAccessListAsync() =>
        await ExecuteAsync(async context => await context.Access.ToListAsync());

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

}