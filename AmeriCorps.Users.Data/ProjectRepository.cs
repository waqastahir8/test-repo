using AmeriCorps.Data;
using AmeriCorps.Users.Data.Core;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;


namespace AmeriCorps.Users.Data;

public interface IProjectRepository
{


    Task<Project?> GetAsync(int id);

    Task<Project?> GetProjectByCodeAsync(string projCode);

    Task<List<Project>?> GetProjectListByOrgAsync(string orgCode);

    Task<T> SaveAsync<T>(T entity) where T : Entity;

    Task<Project?> UpdateProjectAsync(Project entity);

    Task<OperatingSite?> GetOperatingSiteByIdAsync(int opSiteId);

}

public sealed partial class ProjectRepository(
    ILogger<ProjectRepository> logger,
    IContextFactory contextFactory,
    IOptions<UserContextOptions> options
) : RepositoryBase<RepositoryContext, UserContextOptions>(logger, contextFactory, options),
    IProjectRepository
{
    public async Task<Project?> GetProjectByCodeAsync(string projCode) =>
        await ExecuteAsync(async context => await context.Projects.Include(u => u.OperatingSites).FirstOrDefaultAsync(p => p.ProjectCode == projCode));


    public async Task<Project?> GetAsync(int id) =>
        await ExecuteAsync(async context => await context.Projects.Include(p => p.OperatingSites).FirstOrDefaultAsync(p =>p.Id == id));


    public async Task<List<Project>?> GetProjectListByOrgAsync(string orgCode) =>
        await ExecuteAsync(async context => await context.Projects.Include(p => p.OperatingSites).Where(o =>o.ProjectOrg == orgCode).ToListAsync());

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

    public async Task<OperatingSite?> GetOperatingSiteByIdAsync(int opSiteId) =>
        await ExecuteAsync(async context => await context.OperatingSites.FirstOrDefaultAsync(o => o.Id == opSiteId));



    public async Task<Project?> UpdateProjectAsync(Project project)
    {
        Project? upatedProject = null;
        await ExecuteAsync(async context =>
        {
            await using var transaction = await context.Database.BeginTransactionAsync();
            try
            {
                context.Attach(project);
                context.Entry(project).State = EntityState.Modified;
                upatedProject = UpdateProject(project, context);
                await transaction.CommitAsync();
                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                logger.LogWarning($"Could not save changes to repo: {ex}.  Rolling back.");
                await transaction.RollbackAsync();
            }
        });

        return upatedProject;
    }

    private static Project? UpdateProject(Project project, DbContext context)
    {
        var id = project.Id;
        UpdateEntities(project.OperatingSites, context, id);
        return project;
    }

    private static void UpdateEntities<T>(List<T> entities, DbContext context, int id) where T : Entity
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
            .Where(e => e.Id == id && !entityIds.Contains(e.Id));

        foreach (var entity in entitiesToDelete)
        {
            context.Entry(entity).State = EntityState.Deleted;
        }
    }
}