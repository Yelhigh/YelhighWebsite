using YelhighWebsite.Components;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Application services
builder.Services.AddSingleton<YelhighWebsite.Services.INavigationService, YelhighWebsite.Services.NavigationService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

// Navigation endpoint (returns URL based on key)
app.MapGet("/navigate/{key}", (string key, YelhighWebsite.Services.INavigationService navigationService) =>
{
    var url = navigationService.GetTargetUrl(key);
    return Results.Json(new { url });
});

app.Run();
