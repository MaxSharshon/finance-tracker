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

// Add services to the container.
builder.Services.AddDbContext<FinanceTrackerContext>(options =>
    options.UseAzureSql(builder.Configuration.GetConnectionString("LocalFinanceTrackerDb")));

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<ITagRepository, TagRepository>();
builder.Services.AddScoped<IBudgetRepository, BudgetRepository>();
builder.Services.AddScoped<INotificationRepository, NotificationRepository>();
builder.Services.AddScoped<IFinancialOperationRepository, FinancialOperationRepository>();
builder.Services.AddScoped<IBalanceChangeRepository, BalanceChangeRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddScoped<IBalanceChangeService, BalanceChangeService>();
builder.Services.AddScoped<IFinancialOperationService, FinancialOperationService>();
builder.Services.AddScoped<IReportsService, ReportsService>();

builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection("Jwt"));
builder.Services.AddSingleton<JwtProvider>();
builder.Services.AddScoped<IAuthService, AuthService>();

builder.Services.AddValidatorsFromAssemblyContaining<FinancialOperationValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<BalanceChangeValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<RegisterRequestValidator>();

builder.Services.AddAutoMapper(typeof(ApiMapper), typeof(BusinessLogicMapper));

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Secret"]!))
        };
    });

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

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

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
