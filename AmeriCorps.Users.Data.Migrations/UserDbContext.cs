using Microsoft.EntityFrameworkCore;
using AmeriCorps.Users.Data.Core;

namespace AmeriCorps.Users.Data.Migrations;

public sealed class UserDbContext(DbContextOptions<UserDbContext> options) : NpgsqlContext(options) { }