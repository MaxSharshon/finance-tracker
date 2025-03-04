using FinanceTracker.BusinessLogic.Validators;
using FinanceTracker.DataAccess;
using FinanceTracker.DataAccess.Repositories;
using FinanceTracker.DataAccess.Repositories.Interfaces;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<FinanceTrackerContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("FinanceTrackerDb")));
builder.Services.AddScoped<IFinancialOperationRepository, FinancialOperationRepository>();
builder.Services.AddScoped<IBalanceChangeRepository, BalanceChangeRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddValidatorsFromAssemblyContaining<FinancialOperationValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<BalanceChangeValidator>();

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();