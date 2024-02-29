﻿using FedTec.Data;
using AmeriCorps.Users.Data.Core;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;

namespace AmeriCorps.Users.Data;

public interface IUserRepository 
{
       Task<User?> GetAsync(int id);
       Task<User> CreateAsync(User user);
}
public sealed class UserRepository(
    ILogger<UserRepository> logger,
    IContextFactory contextFactory,
    IOptions<UserContextOptions> options
): RepositoryBase<RepositoryContext, UserContextOptions>(logger, contextFactory, options),IUserRepository
{
    public async Task<User?> GetAsync(int id) =>
        await ExecuteAsync(async context => await context.Users.FirstOrDefaultAsync(x => x.Id == id));

    public async Task<User> CreateAsync(User user) {
        await ExecuteAsync(async context =>  {
                context.Users.Add(user);
                await context.SaveChangesAsync();
        });
       
        return user;
    }   
}