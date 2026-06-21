using System.Text;
using FinanceTracker.API.Mapping;
using FinanceTracker.API.Services;
using FinanceTracker.API.Validators;
using FinanceTracker.BusinessLogic.Mapping;
using FinanceTracker.BusinessLogic.Services;
using FinanceTracker.BusinessLogic.Services.Interfaces;
using FinanceTracker.BusinessLogic.Validators;
using FinanceTracker.DataAccess;
using FinanceTracker.DataAccess.Repositories;
using FinanceTracker.DataAccess.Repositories.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

var apiSettings = GetApiSettings(builder.Configuration);

builder.Services.AddDbContext<FinanceTrackerContext>(options => 
    options.UseSqlServer(apiSettings.ConnectionString));

RegisterRepositories(builder.Services);
RegisterBusinessServices(builder.Services);
RegisterAuthServices(builder.Services, builder.Configuration);
RegisterValidationServices(builder.Services);
RegisterMappingServices(builder.Services);
RegisterJwtAuthentication(builder.Services, apiSettings);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

UseGlobalExceptionHandler(app);

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

await ApplyMigrationsIfEnabledAsync(app);

app.Run();
return;

static ApiSettings GetApiSettings(IConfiguration configuration)
{
    var connectionString = configuration.GetConnectionString("LocalFinanceTrackerDb")
                           ?? throw new InvalidOperationException("Connection string 'LocalFinanceTrackerDb' is not configured.");

    var jwtSecret = configuration["Jwt:Secret"];
    var jwtIssuer = configuration["Jwt:Issuer"];
    var jwtAudience = configuration["Jwt:Audience"];

    if (string.IsNullOrWhiteSpace(jwtSecret) || jwtSecret.Length < 32)
    {
        throw new InvalidOperationException("JWT secret is not configured or is shorter than 32 characters.");
    }

    if (string.IsNullOrWhiteSpace(jwtIssuer))
    {
        throw new InvalidOperationException("JWT issuer is not configured.");
    }

    if (string.IsNullOrWhiteSpace(jwtAudience))
    {
        throw new InvalidOperationException("JWT audience is not configured.");
    }

    return new ApiSettings(connectionString, jwtSecret, jwtIssuer, jwtAudience);
}

static void RegisterRepositories(IServiceCollection services)
{
    services.AddScoped<IUserRepository, UserRepository>();
    services.AddScoped<ICategoryRepository, CategoryRepository>();
    services.AddScoped<ITagRepository, TagRepository>();
    services.AddScoped<IBudgetRepository, BudgetRepository>();
    services.AddScoped<INotificationRepository, NotificationRepository>();
    services.AddScoped<IFinancialOperationRepository, FinancialOperationRepository>();
    services.AddScoped<IUnitOfWork, UnitOfWork>();
}

static void RegisterBusinessServices(IServiceCollection services)
{
    services.AddScoped<IFinancialOperationService, FinancialOperationService>();
    services.AddScoped<ICategoryService, CategoryService>();
    services.AddScoped<ITagService, TagService>();
    services.AddScoped<IBudgetService, BudgetService>();
    services.AddScoped<INotificationService, NotificationService>();
    services.AddScoped<IReportsService, ReportsService>();
}

static void RegisterAuthServices(IServiceCollection services, IConfiguration configuration)
{
    services.Configure<JwtOptions>(configuration.GetSection("Jwt"));
    services.AddSingleton<JwtProvider>();
    services.AddScoped<IAuthService, AuthService>();
}

static void RegisterValidationServices(IServiceCollection services)
{
    services.AddValidatorsFromAssemblyContaining<FinancialOperationValidator>();
    services.AddValidatorsFromAssemblyContaining<RegisterRequestValidator>();
}

static void RegisterMappingServices(IServiceCollection services)
{
    services.AddAutoMapper(
        typeof(FinancialOperationBusinessLogicMappingProfile),
        typeof(FinancialOperationApiMappingProfile));
}

static void RegisterJwtAuthentication(IServiceCollection services, ApiSettings apiSettings)
{
    services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = apiSettings.JwtIssuer,
                ValidAudience = apiSettings.JwtAudience,
                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(apiSettings.JwtSecret))
            };
        });
}

static void UseGlobalExceptionHandler(WebApplication app)
{
    app.UseExceptionHandler(errorApp =>
    {
        errorApp.Run(async context =>
        {
            var exception = context.Features.Get<IExceptionHandlerFeature>()?.Error;

            context.Response.ContentType = "application/json";

            var (statusCode, message) = exception switch
            {
                UnauthorizedAccessException => (
                    StatusCodes.Status401Unauthorized,
                    "Unauthorized"),

                KeyNotFoundException => (
                    StatusCodes.Status404NotFound,
                    exception.Message),

                InvalidOperationException => (
                    StatusCodes.Status409Conflict,
                    exception.Message),

                _ => (
                    StatusCodes.Status500InternalServerError,
                    "Internal server error")
            };

            context.Response.StatusCode = statusCode;

            await context.Response.WriteAsJsonAsync(new { message });
        });
    });
}

static async Task ApplyMigrationsIfEnabledAsync(WebApplication app)
{
    if (!app.Configuration.GetValue<bool>("Database:ApplyMigrations"))
    {
        return;
    }
    
    using var scope = app.Services.CreateScope();
    var dbContext = scope.ServiceProvider.GetRequiredService<FinanceTrackerContext>();
    await dbContext.Database.MigrateAsync();
}

internal sealed record ApiSettings(string ConnectionString, string JwtSecret, string JwtIssuer, string JwtAudience);
