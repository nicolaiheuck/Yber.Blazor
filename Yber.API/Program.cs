using Microsoft.AspNetCore.Authentication.JwtBearer;
using Yber.API.Extensions;
using Yber.Repositories.DBContext;
using Yber.Services.Interfaces;
using Yber.Services.Services;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.UI;

var builder = WebApplication.CreateBuilder(args);
//
// // Add authentication and authorization for API endpoints
// // builder.Services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
// //     .AddMicrosoftIdentityWebApi(builder.Configuration.GetSection("AzureAd"));
//
// // Add authentication and authorization for Blazor Server app
// builder.Services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
//     .AddMicrosoftIdentityWebApp(builder.Configuration.GetSection("AzureAd"));
//
// builder.Services.AddControllersWithViews()
//     .AddMicrosoftIdentityUI();
//
// builder.Services.AddAuthorization(options =>
// {
//     // By default, all incoming requests will be authorized according to the default policy
//     options.FallbackPolicy = options.DefaultPolicy;
// });
//
// var keyVaultEndpoint = new Uri(Environment.GetEnvironmentVariable("VaultUri"));
// var debug = new SecretClient(keyVaultEndpoint, new DefaultAzureCredential());
// builder.Configuration.AddAzureKeyVault(keyVaultEndpoint, new DefaultAzureCredential());
// // BUG AZURE KEYVAULT

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerWithOAuth(); // TODO REMOVE OAuth SWAGGER
builder.RegisterDependencies();
builder.Services.AddDbContext<YberContext>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwaggerWithOAuth();
}

app.UseHttpsRedirection();

app.UseAuthentication();
// app.UseAuthorization();

app.MapControllers();

app.Run();