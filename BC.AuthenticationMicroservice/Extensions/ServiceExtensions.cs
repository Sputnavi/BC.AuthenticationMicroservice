using BC.AuthenticationMicroservice.Models;
using BC.AuthenticationMicroservice.Repository;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace BC.AuthenticationMicroservice.Extensions
{
    public static class ServiceExtensions
    {
        public static void ConfigureCors(this IServiceCollection services)
        {
            services.AddCors(opt =>
            {
                opt.AddPolicy("CorsPolicy", builder =>
                    builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader());
            });
        }

        public static void AddBCMessaging(this IServiceCollection services, IConfiguration configuration, bool isDevelopment)
        {
            services.AddMassTransit(x =>
            {
                if (isDevelopment)
                {
                    var rabbitMqSection = configuration.GetSection("RabbitMQ");
                    string host = rabbitMqSection["host"];
                    string virtualHost = rabbitMqSection["virtualHost"];
                    string username = rabbitMqSection["username"];
                    string password = rabbitMqSection["password"];

                    x.UsingRabbitMq((context, config) =>
                    {
                        config.Host(host, virtualHost, h =>
                        {
                            h.Username(username);
                            h.Password(password);
                        });

                        config.ConfigureEndpoints(context);
                    });

                    return;
                }

                string azureServiceBusConnection = configuration.GetConnectionString("AzureServiceBusConnection");
                x.UsingAzureServiceBus((context, config) =>
                {
                    config.Host(azureServiceBusConnection);

                    config.ConfigureEndpoints(context);
                });
            });
        }

        public static void ConfigureIdentity(this IServiceCollection services)
        {
            var builder = services.AddIdentityCore<User>(o =>
            {
                o.Password.RequireDigit = true;
                o.Password.RequireLowercase = true;
                o.Password.RequireUppercase = true;
                o.Password.RequireNonAlphanumeric = true;
                o.Password.RequiredLength = 6;
                o.User.RequireUniqueEmail = true;
            });

            builder = new IdentityBuilder(builder.UserType, typeof(Role), builder.Services);
            builder.AddEntityFrameworkStores<ApplicationContext>()
                .AddDefaultTokenProviders()
                .AddRoles<Role>();
        }

        public static void ConfigureJWT(this IServiceCollection services, IConfiguration configuration)
        {
            var jwtSettings = configuration.GetSection("JwtSettings");
            var secretKey = jwtSettings.GetSection("secretKey").Value;

            services.AddAuthentication(opt =>
            {
                opt.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(opt =>
            {
                opt.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateAudience = true,
                    ValidateIssuer = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,

                    ValidAudience = jwtSettings.GetSection("validAudience").Value,
                    ValidIssuer = jwtSettings.GetSection("validIssuer").Value,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
                };
            });

        }
    }
}
