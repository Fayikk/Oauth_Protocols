using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Infrastructure_Oauth
{
    public static class ServiceRegistration
    {
        public static void AddInfrastructureServices(this IServiceCollection serviceCollection,IConfiguration configuration = null)
        {
            var key = configuration.GetValue<string>("SecretKey:jwtKey");
            serviceCollection.AddAuthentication(u =>
            {
                u.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                u.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(u =>
            {
                u.RequireHttpsMetadata = false;
                u.SaveToken = true;
                u.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key)),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero,

                };
            });








        }
    }
}
