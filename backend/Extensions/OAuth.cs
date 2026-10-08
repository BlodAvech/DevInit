using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using backend.Models;
using Microsoft.AspNetCore.Authentication.OAuth;

namespace backend.Extensions
{
    public static class OAuth
    {
        public static async Task OnCreatingTicket(OAuthCreatingTicketContext ctx)
        {
            using var scope = ctx.HttpContext.RequestServices.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<DevInitContext>();

            var providerId = ctx.Principal!.FindFirstValue(ClaimTypes.NameIdentifier)!;
            var provider = ctx.Scheme.Name.ToLowerInvariant();

            var name = ctx.Principal.FindFirstValue(ClaimTypes.Name);
            var email = ctx.Principal.FindFirstValue(ClaimTypes.Email);

            var userOAuth = db.UserOAuths.FirstOrDefault(o => o.ProviderId == providerId);

            Guid userId;

            if(userOAuth == null)
            {
                var user = new User
                {
                    Name = name,
                    Email = email
                };

                user.OAuths.Add(new UserOAuth
                {
                    Provider = provider,
                    ProviderId = providerId
                });

                db.Users.Add(user);

                try
                {
                    await db.SaveChangesAsync();
                }catch(Exception ex)
                {
                    Console.WriteLine("DB ERROR" + ex.ToString());
                    throw;
                }

                userId = user.Id;
            }
            else
            {
                userId = userOAuth.UserId;
            }

            var identity = (ClaimsIdentity)ctx.Principal.Identity!;
            identity.AddClaim(new Claim("user_id" , userId.ToString()));
        }
    }
}