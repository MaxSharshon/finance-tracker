using FinanceTracker.UI.Components;
using FinanceTracker.UI.Services;
using FinanceTracker.UI.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

var apiBaseAddress = GetApiBaseAddress(builder.Configuration);

RegisterRazorComponents(builder.Services);
RegisterHttpClients(builder.Services, apiBaseAddress);
RegisterClientServices(builder.Services);

var app = builder.Build();

ConfigureRequestPipeline(app);

app.Run();
return;

static Uri GetApiBaseAddress(IConfiguration configuration)
{
    var apiBaseAddress = configuration["ApiSettings:LocalBaseAddress"];

    if (string.IsNullOrWhiteSpace(apiBaseAddress))
    {
        throw new InvalidOperationException("API base address is not configured.");
    }

    return new Uri(apiBaseAddress);
}

static void RegisterRazorComponents(IServiceCollection services)
{
    services.AddRazorComponents()
        .AddInteractiveServerComponents();
}

static void RegisterHttpClients(IServiceCollection services, Uri apiBaseAddress)
{
    services.AddScoped(_ => new HttpClient
    {
        BaseAddress = apiBaseAddress
    });
}

static void RegisterClientServices(IServiceCollection services)
{
    services.AddScoped<IFinancialOperationService, FinancialOperationService>();
    services.AddScoped<IAuthClientService, AuthClientService>();
    services.AddScoped<ICategoryService, CategoryService>();
    services.AddScoped<IBudgetService, BudgetService>();
    services.AddScoped<ITagService, TagService>();
    services.AddScoped<INotificationClientService, NotificationClientService>();
    services.AddScoped<INotificationRefreshService, NotificationRefreshService>();
    services.AddScoped<IReportsService, ReportsService>();
}

static void ConfigureRequestPipeline(WebApplication app)
{
    if (!app.Environment.IsDevelopment())
    {
        app.UseExceptionHandler("/Error", createScopeForErrors: true);
        app.UseHsts();
    }

    app.UseHttpsRedirection();

    app.UseAntiforgery();

    app.MapStaticAssets();
    app.MapRazorComponents<App>()
        .AddInteractiveServerRenderMode();
}
