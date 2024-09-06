using AmeriCorps.Data;
using AmeriCorps.Users.Data.Core;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;


namespace AmeriCorps.Users.Data;

public interface IProjectRepository
{
    Task<Project?> GetProjectByCodeAsync(string projCode);

    Task<T> SaveAsync<T>(T entity) where T : Entity;


    UserProject FindExistingProject(string projectCode, User user);

}

public sealed partial class ProjectRepository(
    ILogger<ProjectRepository> logger,
    IContextFactory contextFactory,
    IOptions<UserContextOptions> options
) : RepositoryBase<RepositoryContext, UserContextOptions>(logger, contextFactory, options),
    IProjectRepository
{
    public async Task<Project?> GetProjectByCodeAsync(string projCode) =>
        await ExecuteAsync(async context => await context.Projects.FirstOrDefaultAsync(p => p.ProjectCode == projCode));

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


    public UserProject FindExistingProject(string projectCode, User user)
    {
        if(!String.IsNullOrEmpty(projectCode) && user != null){
            for (int i = 0; i < user.UserProjects.Count; i++) 
            {
                var existingProject =  user.UserProjects[i];
                if(existingProject.ProjectCode == projectCode && existingProject.Active){
                    return existingProject;
                }
            }
        }
        return null;
    }
}