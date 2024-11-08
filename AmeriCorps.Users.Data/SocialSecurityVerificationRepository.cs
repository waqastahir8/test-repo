using AmeriCorps.Data;
using AmeriCorps.Users.Data.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace AmeriCorps.Users.Data;

public interface ISocialSecurityVerificationRepository
{
    Task<SocialSecurityVerification?> GetVerificationByUserAsync(int userId);

    Task<T> SaveAsync<T>(T entity) where T : Entity;
}

public sealed partial class SocialSecurityVerificationRepository(
    ILogger<SocialSecurityVerificationRepository> logger,
    IContextFactory contextFactory,
    IOptions<UserContextOptions> options
) : RepositoryBase<RepositoryContext, UserContextOptions>(logger, contextFactory, options),
    ISocialSecurityVerificationRepository
{
    public async Task<SocialSecurityVerification?> GetVerificationByUserAsync(int userId) =>
        await ExecuteAsync(async context => await context.SocialSecurityVerification.FirstOrDefaultAsync(s => s.UserId == userId));

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