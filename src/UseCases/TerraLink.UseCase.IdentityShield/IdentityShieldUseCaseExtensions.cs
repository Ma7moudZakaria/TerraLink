using LowCodeHub.MinimalEndpoints.Extensions;
using LowCodeHub.OpenApi;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using TerraLink.Domain.Constants;
using TerraLink.Domain.Entities;
using TerraLink.Domain.Persistence;
using TerraLink.UseCase.IdentityShield.Abstracts.Shield;
using TerraLink.UseCase.IdentityShield.Models;
using TerraLink.UseCase.IdentityShield.Implementation.Shield;

namespace TerraLink.UseCase.IdentityShield
{
    public static class IdentityShieldExtensions
    {
        public static IServiceCollection AddIdentityShieldUseCase(this IServiceCollection services, IConfiguration configuration)
        {
            // NOTE: JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear() 
            // must be called in Program.cs BEFORE this method to ensure custom claim types work correctly

            // Configure IdentityShield Settings - Single consolidated configuration
            IdentityShieldSettings identitySettings = configuration.GetSection("IdentityShield").Get<IdentityShieldSettings>() ?? throw new InvalidOperationException("IdentityShield configuration section is missing.");

            // Register the consolidated settings
            services.Configure<IdentityShieldSettings>(configuration.GetSection("IdentityShield"));

            // Configure Identity with settings from configuration
            services.AddIdentity<UserEntity, RoleEntity>(options =>
           {
               // Password settings from configuration
               options.Password.RequireDigit = identitySettings.Password.RequireDigit;
               options.Password.RequireLowercase = identitySettings.Password.RequireLowercase;
               options.Password.RequireUppercase = identitySettings.Password.RequireUppercase;
               options.Password.RequireNonAlphanumeric = identitySettings.Password.RequireNonAlphanumeric;
               options.Password.RequiredLength = identitySettings.Password.RequiredLength;
               options.Password.RequiredUniqueChars = identitySettings.Password.RequiredUniqueChars;

               // Lockout settings from configuration
               options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(identitySettings.Lockout.DefaultLockoutTimeSpanMinutes);
               options.Lockout.MaxFailedAccessAttempts = identitySettings.Lockout.MaxFailedAccessAttempts;
               options.Lockout.AllowedForNewUsers = identitySettings.Lockout.AllowedForNewUsers;

               // User settings from configuration
               options.User.RequireUniqueEmail = identitySettings.User.RequireUniqueEmail;
               options.SignIn.RequireConfirmedEmail = identitySettings.User.RequireConfirmedEmail;
               options.SignIn.RequireConfirmedPhoneNumber = identitySettings.User.RequireConfirmedPhoneNumber;

               // Configure claims identity to use custom claim types that match JWT token claims
               // This ensures ASP.NET Core Identity uses the same claim types as the JWT tokens
               options.ClaimsIdentity.RoleClaimType = "roles";
               options.ClaimsIdentity.UserNameClaimType = "unique_name";
               options.ClaimsIdentity.UserIdClaimType = "sub";
           })
            .AddEntityFrameworkStores<TerraLinkDbContext>()
            .AddDefaultTokenProviders()
            .AddTokenProvider<ShieldOTPTokenProvider<UserEntity>>(Constant.TokenProviders.OtpTokenProvider);

            // Register custom password hasher
            services.AddScoped<IPasswordHasher<UserEntity>, ShieldPasswordHasher>();

            // Configure JWT Authentication
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
          .AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.RequireHttpsMetadata = false; // Set to true in production
                options.MapInboundClaims = false; // Prevent default claim mapping to use custom claim types
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ClockSkew = TimeSpan.FromSeconds(15),
                    ValidIssuer = identitySettings.Jwt.Issuer,
                    ValidAudience = identitySettings.Jwt.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(identitySettings.Jwt.Secret)),
                    // Map "roles" claim to RoleClaimType so ASP.NET Core recognizes it for authorization
                    RoleClaimType = "roles",
                    NameClaimType = "unique_name"
                };
            });

            // Add HttpContextAccessor for accessing user from token
            services.AddHttpContextAccessor();

            services.AddOperations<IIdentityShieldScanner>();
            services.AddManualMapper<IIdentityShieldScanner>();

            services.AddScoped<IJwtTokenProvider, JwtTokenProvider>();
            services.AddScoped<IIdentityClaimMapper<UserEntity>, ShieldIdentityClaimMapper>();

            services.AddOpenApiDoc("identity-shield-module", config =>
            {
                config.WithTitle("Roles & Authentication API")
                      .WithVersion("v1")
                      .WithSecurity(sec => sec.AutoDetect())
                      .WithOperationSecurity()
                      .WithGroupName("identity-shield-module");
            });


            return services;
        }
    }
}
