using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using TestLibrary.Settings;
using TestLibrary.Users.Interfaces;
using TestLibrary.Users.Model;
using TestLibrary.Users.Service;

namespace TestAPI.Extensions
{
    public static class ServiceCollectionExtention
    {
        public static void SetupDependencies(this IServiceCollection services)
        {
            services.AddScoped<IUserService, UserService>();
            services.AddTransient<IJWTService, JWTService>();
            services.AddTransient<IClaimsService, ClaimsService>();
        }

        public static void SetupDBs(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<TestDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("TestDB")));
        }

        public static void SetupSettings(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<JWTSettings>(configuration.GetSection("Jwt"));
        }

        public static void SetupAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddIdentity<User, UserRole>(options =>
            {
                options.User.RequireUniqueEmail = true;
                //options.SignIn.RequireConfirmedEmail = true;
                //options.SignIn.RequireConfirmedAccount = true;
                //options.Lockout.MaxFailedAccessAttempts = 5;
                options.Password.RequireNonAlphanumeric = false;

            })
                .AddEntityFrameworkStores<TestDbContext>()
                .AddDefaultTokenProviders();

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;

            }).AddJwtBearer(o =>
            {
                o.SaveToken = true;
                o.RequireHttpsMetadata = false;
                o.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidIssuer = configuration["JWT:ValidIssuer"],
                    ValidAudience = configuration["JWT:ValidAudience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:SecretKey"])),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true
                };
            });
        }

        public static void SetupSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("V1", new OpenApiInfo
                {
                    Version = "V1",
                    Title = "TestAPI",
                    Description = "Test Application"
                });

                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Name = "Authorization",
                    Description = "Bearer Authentication with JWT Token",
                    Type = SecuritySchemeType.Http
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Id = "Bearer",
                                Type = ReferenceType.SecurityScheme
                            }
                        },
                        new List<string>()
                    }
                });

            });
        }
    }
}
