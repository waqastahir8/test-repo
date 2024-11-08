using AmeriCorps.Users.Configuration;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new DateOnlyJsonConverter());
});
builder.Services.AddApiVersioning();
builder.Services.AddApplicationInsightsTelemetry();
builder.Services
    .AddSingleton<IValidator, Validator>()
    .AddScoped<IContextFactory, DefaultContextFactory>()
    .AddScoped<IUserRepository, UserRepository>()
    .AddScoped<IRoleRepository, RoleRepository>()
    .AddScoped<IOrganizationRepository, OrganizationRepository>()
    .AddScoped<IProjectRepository, ProjectRepository>()
    .AddScoped<IAccessRepository, AccessRepository>()
    .AddScoped<IRequestMapper, RequestMapper>()
    .AddScoped<IResponseMapper, ResponseMapper>()
    .AddScoped<IUsersControllerService, UsersControllerService>()
    .AddScoped<IRolesControllerService, RolesControllerService>()
    .AddScoped<IOrgControllerService, OrgControllerService>()
    .AddScoped<IProjectControllerService, ProjectControllerService>()
    .AddScoped<IAccessControllerService, AccessControllerService>()
    .AddScoped<IUserHelperService, UserHelperService>()
    .AddScoped<IEmailTemplatesService, EmailTemplatesService>()
    .AddScoped<IEncryptionService, EncryptionService>()
    .AddScoped<ISsaControllerService, SsaControllerService>()
    .AddScoped<ISocialSecurityVerificationRepository, SocialSecurityVerificationRepository>();

builder.Services.AddHttpClient()
            .AddTransient<INotificationApiClient, NotificationApiClient>();
var keyVaultUri = builder.Configuration["KeyVaultOptions:KeyVaultUri"]!;
var tenantId = builder.Configuration["KeyVaultOptions:TenantId"];
var clientId = builder.Configuration["KeyVaultOptions:ClientId"];
var clientSecret = builder.Configuration["KeyVaultOptions:ClientSecret"];

builder.Configuration
    .AddJsonFile("appsettings.local.json", optional: true)
    .AddJsonFile("appsettings.json", optional: false)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true);

if (!string.IsNullOrEmpty(keyVaultUri) &&
    !string.IsNullOrEmpty(tenantId) &&
    !string.IsNullOrEmpty(clientId) &&
    !string.IsNullOrEmpty(clientSecret))
{
    builder.Configuration
        .AddAzureKeyVault(new Uri(keyVaultUri),
            new ClientSecretCredential(
                tenantId,
                clientId,
                clientSecret));
}
else if (!string.IsNullOrEmpty(keyVaultUri))
{
    builder.Configuration
        .AddAzureKeyVault(new Uri(keyVaultUri),
            new DefaultAzureCredential());
}

builder.Configuration
    .AddJsonFile("appsettings.local.json", optional: true);

builder.Services.Configure<UserContextOptions>(
    builder.Configuration.GetSection(nameof(UserContextOptions)));

builder.Services.Configure<NotificationOptions>(
    builder.Configuration.GetSection(nameof(NotificationOptions)));

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();
app.UseRouting();
app.MapControllers();

await app.RunAsync();