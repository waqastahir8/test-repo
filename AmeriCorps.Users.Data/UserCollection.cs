using AmeriCorps.Users.Data.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AmeriCorps.Users.Data;

public partial class UserRepository
{
    public async Task<Collection?> SaveAsync(Collection collection) =>
        await ExecuteAsync(async context =>
        {
            var isCollectionExist = await IsCollectionExist(collection, context);
            if (isCollectionExist)
                throw new Exception("Collection already exists");

            Collection c;

            if (collection.Id == default)
            {
                c = (await context.Collection.AddAsync(collection)).Entity;
            }
            else
            {
                context.Update(collection);
                c = collection;
            }

            await context.SaveChangesAsync();
            return c;
        });

    private static async Task<bool> IsCollectionExist(Collection collection, RepositoryContext context)
    {
        var isCollectionExist = await context.Collection.AnyAsync(c =>
            c.Type == collection.Type && c.ListingId == collection.ListingId &&
            c.UserId == collection.UserId
        );
        return isCollectionExist;
    }
    public async Task<List<Collection>?> GetCollectionAsync(Collection collection)
    {
        var collectionResponse = await ExecuteAsync(
            async context =>
                await context.Collection.Where(c => collection.UserId == c.UserId && collection.Type == c.Type)
                    .ToListAsync());

        return collectionResponse;
    }
    public async Task<bool> DeleteCollectionAsync(List<Collection> collections)
    {
        var collectionsToDelete = new List<Collection>();
        var isDeleted = false;
        try
        {
            isDeleted = await ExecuteAsync(async context =>
            {
                foreach (var collection in collections)
                {
                    var collectionToDelete = await context.Collection.FirstOrDefaultAsync(c => c.UserId == collection.UserId
                        && c.ListingId == collection.ListingId && c.Type == collection.Type
                    );
                    if (collectionToDelete == null)
                    {
                        logger.LogError("Id provided in listing does not exist");
                        return false;
                    }
                    collectionsToDelete.Add(collectionToDelete);
                }

                context.Collection.RemoveRange(collectionsToDelete);

                await context.SaveChangesAsync();
                return true;
            });
        }
        catch (Exception ex)
        {
            logger.LogError($"Could not delete collection '{ex}'.");
            isDeleted = false;
        }

        return isDeleted;
    }

}