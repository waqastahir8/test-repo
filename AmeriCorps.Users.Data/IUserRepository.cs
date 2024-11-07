using AmeriCorps.Users.Data.Core;
using System.Linq.Expressions;

namespace AmeriCorps.Users.Data;

public interface IUserRepository
{
    Task<User?> GetAsync(int id);

    Task<IEnumerable<User>?> GetByAttributeAsync(string type, string value);

    Task<User?> GetByExternalAccountIdAsync(string externalAccountId);

    Task<int> GetUserIdByExternalAccountIdAsync(string externalAccountId);

    Task<List<SavedSearch>?> GetUserSearchesAsync(int id);

    Task<List<Reference>?> GetUserReferencesAsync(int id);

    Task<bool> ExistsAsync<T>(Expression<Func<T, bool>> predicate) where T : Entity;

    Task<T> SaveAsync<T>(T entity) where T : Entity;

    Task<User?> UpdateUserAsync(User entity);

    Task<bool> DeleteAsync<T>(int id) where T : Entity;

    Task<Collection?> SaveAsync(Collection collection);

    Task<List<Collection>?> GetCollectionAsync(Collection collection);

    Task<bool> DeleteCollectionAsync(List<Collection> collections);

    Task<Role?> GetRoleAsync(int roleId);

    Task<UserList> FetchUserListByOrgCodeAsync(string orgCode);

    Task<List<User>> FetchInvitedUsersForReminderAsync();

    Task<User?> FindInvitedUserInfo(string userEmail, string orgCode);

    Task<SocialSecurityVerification?> FindSocialSecurityVerificationByUserId(int userId);

    Task<List<User>> FetchFailedSSAChecksAsync();

    Task<List<User>> FetchVistaRecipientsAsync();

    Task<List<User>> FetchAsnRecipientsAsync();

    Task<List<User>> FetchNcccRecipientsAsync();

}