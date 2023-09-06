using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using Tweetbook.Migrations;
using Tweetbook.Options;
using Tweetbook.Services;

namespace Tweetbook.Extensions
{
    public static class ConfigureServices
    {
        public static void ConfigureSwagger(this IServiceCollection services, IConfiguration configuration)
        {
            var jwtSettings = new JwtSettings();

            configuration.Bind(key: nameof(jwtSettings), jwtSettings);

            services.AddSingleton(jwtSettings);

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtSettings.Secret)),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    RequireExpirationTime = false,
                    ValidateLifetime = true
                };
            });

            services.AddSwaggerGen(x =>
            {
                x.SwaggerDoc("v1", new OpenApiInfo { Title = "Tweetbook", Version = "v1" });

                var sch = new Dictionary<string, IEnumerable<string>>
                {
                    {"Bearer" , new string[0] }
                };


                var sec = new OpenApiSecurityRequirement
                {
                   {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }

                        },new string[] {}
                   }
                };


                x.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the bearer scheme",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey
                });

                x.AddSecurityRequirement(sec);
            });
        }

        public static void ConfigureControllers(this IServiceCollection services)
        {
            services.AddControllers();
        }

        public static void ConfigurePostScoped(this IServiceCollection services)
        {
            services.AddScoped<IPostService, PostService>();
        }

        public static void ConfigureIdentity(this IServiceCollection services)
        {
            services.AddScoped<IIdentityService, IdentityService>();
        }
        
        public static void ConfigureDataContext(this IServiceCollection services)
        {
            services.AddDbContext<DataContext>();
        }
        
        
    }
}
