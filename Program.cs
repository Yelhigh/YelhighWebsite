using System.Text.Json.Serialization.Metadata;
using YelhighWebsite.Components;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Application services
builder.Services.AddSingleton<YelhighWebsite.Services.INavigationService, YelhighWebsite.Services.NavigationService>();
builder.Services.AddSingleton<YelhighWebsite.Services.ILocalizationService, YelhighWebsite.Services.LocalizationService>();

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

// Localization endpoint (returns translated texts)
app.MapGet("/localization/{lang}", (string lang, YelhighWebsite.Services.ILocalizationService localizationService) =>
{
    var texts = localizationService.GetTexts(lang);
    return Results.Json(texts);
});

// Bulk translate endpoint for arbitrary text nodes
app.MapPost("/localization/translate/{lang}", async (string lang, HttpRequest request, YelhighWebsite.Services.ILocalizationService localizationService) =>
{
    var body = await System.Text.Json.JsonSerializer.DeserializeAsync<IEnumerable<string>>(request.Body);
    var result = localizationService.TranslateTexts(lang, body ?? Array.Empty<string>());
    return Results.Json(result);
});

app.Run();
