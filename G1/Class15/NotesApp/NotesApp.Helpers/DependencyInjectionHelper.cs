using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using NotesApp.DataAccess.Implementations.EntityFramework;
using NotesApp.DataAccess.Interfaces;
using NotesApp.Services.Configuration;
using NotesApp.Services.Implementations;
using NotesApp.Services.Interfaces;
using System.Text;

namespace NotesApp.Helpers;

public static class DependencyInjectionHelper
{
    public static void AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<INoteRepository, NoteRepository>(); // EF Core
        //services.AddScoped<INoteRepository, NoteRepositoryAdoNet>(); // ADO.NET
        //services.AddScoped<INoteRepository, NoteRepositoryDapper>(); // Dapper 
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<ITagRepository, TagRepository>();
    }

    public static void AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<INoteService, NoteService>();
        services.AddScoped<IAuthService, AuthService>();
    }

    public static void AddJwtAuthentication(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // 1) Bind the "JwtSettings" section, so we can inject IOptions<JwtSettings> in our services
        IConfigurationSection jwtSection = configuration.GetSection("JwtSettings");
        services.Configure<JwtSettings>(jwtSection);

        // 2) Read it here to use it for JWT authentication configuration
        JwtSettings jwtSettings = jwtSection.Get<JwtSettings>()!;

        // 3) Configure JWT authentication
        // When someone says [Authorize], without specifying a scheme, it will use this configuration by default to validate the JWT token
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                // A) Is this our signature ?
                // This is the most important part, because it ensures that the token was signed with our secret key, and not some other key
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SecretKey)),

                // B) Did we issue it ?
                // This ensures that the token was issued by our application
                ValidateIssuer = true,
                ValidIssuer = jwtSettings.Issuer,
                ValidateAudience = true,
                ValidAudience = jwtSettings.Audience,

                // C) Is it expired ?
                // This ensures that the token is still valid and has not expired
                ValidateLifetime = true,

                // The default allows for a 5 minute clock skew when validating the token's expiration. We don't want that, so we set it to zero.
                ClockSkew = TimeSpan.Zero
            };
        });
    }

    public static void AddSwaggerWithJwt(this IServiceCollection services)
    {
        services.AddSwaggerGen(options =>
        {
            // 1) Describe the scheme: a bearer token in the Authorization header.
            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                Scheme = "bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "Paste the token only - Swagger adds the 'Bearer ' prefix itself."
            });

            // 2) Apply it to every endpoint, so the padlocks appear and the token is
            // actually sent. Without this, the button shows but no header goes out.
            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    Array.Empty<string>()
                }
            });
        });
    }
}
