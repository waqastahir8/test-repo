using AmeriCorps.Data;
using AmeriCorps.Users.Data.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace AmeriCorps.Users.Data;

public interface IOrganizationRepository
{
    Task<Organization?> GetOrgByCodeAsync(string orgCode);

    Task<List<Organization>?> GetOrgListAsync();

    Task<T> SaveAsync<T>(T entity) where T : Entity;
}

public sealed partial class OrganizationRepository(
    ILogger<OrganizationRepository> logger,
    IContextFactory contextFactory,
    IOptions<UserContextOptions> options
) : RepositoryBase<RepositoryContext, UserContextOptions>(logger, contextFactory, options),
    IOrganizationRepository
{
    public async Task<Organization?> GetOrgByCodeAsync(string orgCode) =>
        await ExecuteAsync(async context => await context.Organizations.FirstOrDefaultAsync(o => o.OrgCode == orgCode));

    public async Task<List<Organization>?> GetOrgListAsync() =>
        await ExecuteAsync(async context => await context.Organizations.ToListAsync());

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