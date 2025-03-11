using FinanceTracker.API.Mapping;
using FinanceTracker.BusinessLogic.Mapping;
using FinanceTracker.BusinessLogic.Services;
using FinanceTracker.BusinessLogic.Services.Interfaces;
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
builder.Services.AddScoped<IBalanceChangeService, BalanceChangeService>();
builder.Services.AddScoped<IFinancialOperationService, FinancialOperationService>();

builder.Services.AddValidatorsFromAssemblyContaining<FinancialOperationValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<BalanceChangeValidator>();

builder.Services.AddAutoMapper(typeof(ApiMapper), typeof(BusinessLogicMapper));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();