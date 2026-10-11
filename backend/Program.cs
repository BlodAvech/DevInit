using System.Diagnostics;
using System.Security.Claims;
using backend.Extensions;
using backend.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.EntityFrameworkCore;
using FluentValidation;
using Scalar.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

DotNetEnv.Env.Load();

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

string connectionString = builder.Configuration.GetConnectionString("DefaultConnection")!;
builder.Services.AddDbContext<DevInitContext>(options => options.UseNpgsql(connectionString));

if(!string.IsNullOrEmpty(Environment.GetEnvironmentVariable("GOOGLE_CLIENT_ID")))
{
    builder.Services.AddAuthentication()
    .AddGoogle(options =>
    {
        options.ClientId = Environment.GetEnvironmentVariable("GOOGLE_CLIENT_ID")!;
        options.ClientSecret = Environment.GetEnvironmentVariable("GOOGLE_CLIENT_SECRET")!;
        options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        
        options.Events.OnCreatingTicket = OAuth.OnCreatingTicket;
    });
}

if(!string.IsNullOrEmpty(Environment.GetEnvironmentVariable("GITHUB_CLIENT_ID")))
{
    builder.Services.AddAuthentication()
    .AddGitHub(options =>
    {
        options.ClientId = Environment.GetEnvironmentVariable("GITHUB_CLIENT_ID")!;
        options.ClientSecret = Environment.GetEnvironmentVariable("GITHUB_CLIENT_SECRET")!;
        options.Scope.Add("user:email");
        options.Events.OnCreatingTicket = OAuth.OnCreatingTicket;
    });
}

builder.Services.AddValidatorsFromAssemblyContaining<Program>();

builder.Services.AddScoped<JWTService>();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(builder.Configuration["JWT:Secret"]!)),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });

builder.Services.AddAuthorization();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    app.MapScalarApiReference();
}

app.UseHttpsRedirection();


app.MapAuthRoute();

// app.MapGet("/" , () => Results.Ok("ok"));

// app.MapGet("/google" , async () =>
// {
//     return Results.Challenge(
//         new AuthenticationProperties { RedirectUri = "/" },
//         new[] { "Google"}
//     );
// });

app.Run();