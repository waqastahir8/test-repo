using System.Linq.Expressions;
using AmeriCorps.Users.Data.Core;

namespace AmeriCorps.Users.Data;

public interface IUserRepository
{
    Task<User?> GetAsync(int id);

    Task<User?> GetByExternalAcctId(string ExternalAccountId);
    Task<int> GetUserIdByExternalAcctId(string externalAccountId);
    Task<List<SavedSearch>?> GetUserSearchesAsync(int id);
    Task<List<Reference>?> GetUserReferencesAsync(int id);
    Task<bool> ExistsAsync<T>(Expression<Func<T, bool>> predicate) where T : Entity;
    Task<T> SaveAsync<T>(T entity) where T : Entity;

    Task<User?> UpdateUserAsync(User entity);
    Task<bool> DeleteAsync<T>(int id) where T : Entity;
    Task<Collection?> SaveAsync(Collection collection);


    Task<List<Collection>?> GetCollectionAsync(Collection collection);

    Task<bool> DeleteCollectionAsync(List<Collection> collections);
}