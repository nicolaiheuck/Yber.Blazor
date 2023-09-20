using Microsoft.OpenApi.Models;

namespace Yber.API.Extensions;

public static class SwaggerExtensions
{
    public static IServiceCollection AddSwaggerWithOAuth(this IServiceCollection services)
    {
        return services.AddSwaggerGen(options =>
        {
            options.AddSecurityDefinition("OAuth2", new OpenApiSecurityScheme()
            {
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.OpenIdConnect,
                OpenIdConnectUrl = new Uri("https://tved-it.eu.auth0.com/.well-known/openid-configuration"),
                BearerFormat = "JWT",
                Scheme = "Bearer",
            });

            options.AddSecurityRequirement(new OpenApiSecurityRequirement()
            {
                {
                    new OpenApiSecurityScheme()
                    {
                        Reference = new OpenApiReference()
                        {
                            Id = "OAuth2",
                            Type = ReferenceType.SecurityScheme
                        }
                    },
                    new List<string>()
                }
            });
        });
    }

    public static WebApplication UseSwaggerWithOAuth(this WebApplication app)
    {
        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            options.OAuthAdditionalQueryStringParams(new() { { "audience", "https://uber.travel" } });
            options.OAuthClientId("BwlvqkEGoYmw4gzrmgxoWt4zqQRx5i9c"); // TODO HIDE SECRET
            options.OAuthClientSecret("NuYGvyNB8lyYn8PL4as9ek-eafhOO8OnFZxB3jg0n9UP50jFb8CZgF8xvzpDGO9O"); // TODO HIDE SECRET
        });

        return app;
    }
}