using System.Linq.Expressions;
using AmeriCorps.Data;
using AmeriCorps.Users.Data.Core;
using AmeriCorps.Users.Data.Core.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

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
                .Include(u => u.CountryOfBirth)
                .Include(u => u.StateOfBirth)
                .Include(u => u.CityOfBirth)
                .Include(u => u.DateOfBirths)
                .Include(u => u.EncryptedSocialSecurityNumbers)
                .Include(u => u.DirectDeposits)
                .Include(u => u.TaxWithHoldings)
                .Include(u => u.SocialSecurityVerification)
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
                .Include(u => u.CountryOfBirth)
                .Include(u => u.StateOfBirth)
                .Include(u => u.CityOfBirth)
                .Include(u => u.DateOfBirths)
                .Include(u => u.EncryptedSocialSecurityNumbers)
                .Include(u => u.DirectDeposits)
                .Include(u => u.TaxWithHoldings)
                .Include(u => u.SocialSecurityVerification)
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
                    .Include(u => u.SocialSecurityVerification)
                    .Where(x => x.OrgCode == orgCode).ToListAsync());

        UserList? userList = new UserList()
        {
            OrgCode = orgCode,
            Users = users
        };

        return userList;
    }

    public async Task<List<User>> FetchInvitedUsersForReminderAsync()
    {
        var inviteCheckDate = DateTime.UtcNow.AddDays(-14);

        return await ExecuteAsync(async context => await context.Users
            .Where(u => u.UserAccountStatus == UserAccountStatus.INVITED
                && DateTime.Compare(u.InviteDate ?? DateTime.MaxValue, inviteCheckDate) <= 0).ToListAsync());
    }

    public async Task<User?> FindInvitedUserInfo(string userEmail, string orgCode) =>
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
                .Include(u => u.SocialSecurityVerification)
                .FirstOrDefaultAsync(x => x.UserName == userEmail && x.OrgCode == orgCode && (x.UserAccountStatus == UserAccountStatus.INVITED || x.UserAccountStatus == UserAccountStatus.PENDING)));


    public async Task<SocialSecurityVerification?> FindSocialSecurityVerificationByUserId(int userId) =>
        await ExecuteAsync(async context => await context.SocialSecurityVerification
            .FirstOrDefaultAsync(p => p.UserId == userId));


    public async Task<List<User>> FetchFailedSSAChecksAsync()
    {
        var updatedDate = DateTime.UtcNow.AddDays(-1);

        return await ExecuteAsync(async context => await context.Users
            .Include(u => u.SocialSecurityVerification)
            .Include(u => u.CommunicationMethods)
            .Include(u => u.UserProjects)
            .Where(x => x.SocialSecurityVerification != null && ((x.SocialSecurityVerification.CitizenshipStatus == VerificationStatus.Returned &&
                DateTime.Compare(x.SocialSecurityVerification.CitizenshipUpdatedDate ?? DateTime.MinValue, updatedDate) <= 0)
                || (x.SocialSecurityVerification.SocialSecurityStatus == VerificationStatus.Returned &&
                DateTime.Compare(x.SocialSecurityVerification.SocialSecurityUpdatedDate ?? DateTime.MinValue, updatedDate) >= 0))).ToListAsync());
    }

    public async Task<List<User>> FetchVistaRecipientsAsync()
    {
        //ORO and Vista
        return await ExecuteAsync(async context => await context.Users
            .Include(u => u.CommunicationMethods)
            .Include(u => u.Roles)
            .Include(u => u.UserProjects)
            .Where(x => x.Roles != null && x.Roles.Count > 0 &&
                ((x.UserProjects != null && x.UserProjects.Count > 0 && x.UserProjects.Where(project => project.ProjectType == "VISTA").ToList().Count > 0 &&
                x.Roles.Where(role => role.RoleName == "Program Staff").ToList().Count > 0) 
                || x.Roles.Where(role => role.RoleName == "VISTA Program Staff" || role.RoleName == "ORO").ToList().Count > 0)
                ).ToListAsync());
    }

    public async Task<List<User>> FetchAsnRecipientsAsync()
    {
        //Award Recipient
        return await ExecuteAsync(async context => await context.Users
            .Include(u => u.CommunicationMethods)
            .Include(u => u.Roles)
            .Include(u => u.UserProjects)
            .Where(x => x.Roles != null && x.Roles.Count > 0 && x.UserProjects != null && x.UserProjects.Count > 0 &&
                x.UserProjects.Where(project => project.ProjectType == "ASN").ToList().Count > 0 &&
                x.Roles.Where(role => role.RoleName == "Award Recipient").ToList().Count > 0
                ).ToListAsync());
    }

    public async Task<List<User>> FetchNcccRecipientsAsync()
    {
        //NCCC
        return await ExecuteAsync(async context => await context.Users
            .Include(u => u.CommunicationMethods)
            .Include(u => u.Roles)
            .Include(u => u.UserProjects)
            .Where(x => x.Roles != null && x.Roles.Count > 0 &&
                ((x.UserProjects != null && x.UserProjects.Count > 0 && x.UserProjects.Where(project => project.ProjectType == "NCCC").ToList().Count > 0 &&
                x.Roles.Where(role => role.RoleName == "Program Staff").ToList().Count > 0)
                || x.Roles.Where(role => role.RoleName == "NCCC Program Staff").ToList().Count > 0)
               ).ToListAsync());
    }

    public async Task<User?> FetchUserByEncryptedSSNAsync(string encryptedId) =>
        await ExecuteAsync(async context =>
            await context.Users
                .AsNoTracking()
                .Include(u => u.SocialSecurityVerification)
                .FirstOrDefaultAsync(x => x.EncryptedSocialSecurityNumber == encryptedId));

    public async Task<List<User>?> FetchPendingUsersForSSAVerificationAsync()
    {
        return await ExecuteAsync(async context => await context.Users
            .Include(u => u.SocialSecurityVerification)
            .Include(u => u.Attributes)
            .Where(x => x.SocialSecurityVerification != null && x.SocialSecurityVerification.FileStatus == SSAFileStatus.PendingToSend).ToListAsync());
    }

}