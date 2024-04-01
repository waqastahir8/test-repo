var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();
builder.Services.AddApiVersioning();

builder.Services
    .AddSingleton<IValidator, Validator>()
    .AddScoped<IContextFactory, DefaultContextFactory>()
    .AddScoped<IUserRepository, UserRepository>()
    .AddScoped<IRequestMapper, RequestMapper>()
    .AddScoped<IResponseMapper, ResponseMapper>()
    .AddScoped<IUsersControllerService, UsersControllerService>();

var keyVaultUri = builder.Configuration["KeyVaultOptions:KeyVaultUri"]!;
var tenantId = builder.Configuration["KeyVaultOptions:TenantId"];
var clientId = builder.Configuration["KeyVaultOptions:ClientId"];
var clientSecret = builder.Configuration["KeyVaultOptions:ClientSecret"];

var configuration =
    builder.Configuration
    .AddJsonFile("appsettings.json", optional: false)
    .AddJsonFile("appsettings.{env.EnvironmentName}.json", optional: true)
    .AddAzureKeyVault(new Uri(keyVaultUri),
                    new ClientSecretCredential(
                        tenantId,
                        clientId,
                        clientSecret))
    .AddJsonFile("appsettings.local.json", optional: true)
    .Build();

builder.Services.Configure<UserContextOptions>(
    configuration.GetSection(nameof(UserContextOptions)));

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();
app.UseRouting();
app.MapControllers();

await app.RunAsync();