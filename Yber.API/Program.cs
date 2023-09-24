using System.Reflection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Yber.API.Extensions;
using Yber.Repositories.DBContext;
using Yber.Services.Interfaces;
using Yber.Services.Services;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerWithOAuth();
builder.RegisterDependencies();
builder.Services.AddLogging(loggingBuilder =>
{
    loggingBuilder.AddSeq(builder.Configuration.GetSection("Seq"));
});
builder.Services.AddDbContext<YberContext>();
// builder.Services.AddScoped<IYberService, YberService>();

// Authentication pipeline config - START
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.Authority = "https://tved-it.eu.auth0.com/";
    options.Audience = "https://uber.travel";
});
// Authentication pipeline config - END

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwaggerWithOAuth();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();