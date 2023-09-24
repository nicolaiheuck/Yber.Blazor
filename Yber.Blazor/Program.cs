using System.Net;
using Blazored.Modal;
using Blazored.Toast;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Localization;
using Radzen;
using Toolbelt.Blazor.Extensions.DependencyInjection;
using Yber.Blazor.Extensions;
using Azure.Identity;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.UI;
using Yber.Repositories.DBContext;

var builder = WebApplication.CreateBuilder(args);

// Add authentication and authorization for API endpoints
builder.Services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
				.AddMicrosoftIdentityWebApi(builder.Configuration.GetSection("AzureAd"));

// Add authentication and authorization for Blazor Server app
builder.Services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
				.AddMicrosoftIdentityWebApp(builder.Configuration.GetSection("AzureAd"));

builder.Services.AddControllersWithViews()
	.AddMicrosoftIdentityUI();

builder.Services.AddAuthorization(options =>
{
	// By default, all incoming requests will be authorized according to the default policy
	options.FallbackPolicy = options.DefaultPolicy;
});

var keyVaultEndpoint = new Uri(builder.Configuration["VaultUri"]);
builder.Configuration.AddAzureKeyVault(keyVaultEndpoint, new DefaultAzureCredential());


// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddDbContext<YberContext>();
builder.Services.AddHttpClient();

// configure cultures
builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    var supportedCultures = new[] { "en", "dk" };
    options.DefaultRequestCulture = new RequestCulture("en");
    options.AddSupportedCultures(supportedCultures);
    options.AddSupportedUICultures(supportedCultures);
});

// UI services
builder.Services.AddI18nText();
builder.Services.AddBlazoredModal();
builder.Services.AddBlazoredToast();
builder.Services.AddHotKeys2();
builder.Services.AddScoped<DialogService>();
builder.Services.AddScoped<NotificationService>();
builder.Services.AddScoped<TooltipService>();
builder.Services.AddScoped<ContextMenuService>();
builder.RegisterDependencies();
builder.Services.AddLogging(loggingBuilder =>
{
	loggingBuilder.AddSeq(builder.Configuration.GetSection("Seq"));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
