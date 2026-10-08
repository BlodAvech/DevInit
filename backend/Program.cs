using System.Diagnostics;
using System.Security.Claims;
using backend.Extensions;
using backend.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.EntityFrameworkCore;

DotNetEnv.Env.Load();

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

string connectionString = builder.Configuration.GetConnectionString("DefaultConnection")!;
builder.Services.AddDbContext<DevInitContext>(options => options.UseNpgsql(connectionString));

builder.Services.AddAuthentication(options =>
    {
        options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
    })
    .AddCookie()
    .AddGoogle(options =>
    {
        options.ClientId = Environment.GetEnvironmentVariable("GOOGLE_CLIENT_ID")!;
        options.ClientSecret = Environment.GetEnvironmentVariable("GOOGLE_CLIENT_SECRET")!;
        options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        
        options.Events.OnCreatingTicket = OAuth.OnCreatingTicket;
    })
    .AddGitHub(options =>
    {
        options.ClientId = Environment.GetEnvironmentVariable("GITHUB_CLIENT_ID")!;
        options.ClientSecret = Environment.GetEnvironmentVariable("GITHUB_CLIENT_SECRET")!;
        options.Scope.Add("user:email");
        options.Events.OnCreatingTicket = OAuth.OnCreatingTicket;
    });



var app = builder.Build();



if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();



app.MapGet("/" , async () =>
{
    return Results.Ok("все норм");
});

app.MapGet("/google" , async () =>
{
    return Results.Challenge(
        new AuthenticationProperties { RedirectUri = "/" },
        new[] { "Google"}
    );
});

app.MapGet("/github" , async () =>
{
    return Results.Challenge(
        new AuthenticationProperties { RedirectUri = "/" },
        new[] { "GitHub"}
    );
});

app.MapGet("/me" , async (ClaimsPrincipal user) =>
{
    return Results.Ok(new
    {
        id = user.FindFirstValue(ClaimTypes.NameIdentifier),
        name = user.FindFirstValue(ClaimTypes.Name),
        email = user.FindFirstValue(ClaimTypes.Email),
        given = user.FindFirstValue(ClaimTypes.GivenName),
        surname = user.FindFirstValue(ClaimTypes.Surname),
    });
});

app.Run();
