using BC.AuthenticationMicroservice.Extensions;
using BC.AuthenticationMicroservice.Interfaces;
using BC.AuthenticationMicroservice.Profiles;
using BC.AuthenticationMicroservice.Repository;
using BC.AuthenticationMicroservice.Services;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using NLog;

var builder = WebApplication.CreateBuilder(args);

LogManager.LoadConfiguration(string.Concat(Directory.GetCurrentDirectory(), "/nlog.config"));
// Add services to the container.
var configuration = builder.Configuration;
var services = builder.Services;

services.AddDbContext<ApplicationContext>(options =>
    options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

services.AddAuthentication();
services.ConfigureIdentity();
services.ConfigureJWT(configuration);
services.AddScoped<IAuthenticationService, AuthenticationService>();

services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
services.AddSingleton<ILoggerManager, LoggerManager>();
services.AddScoped<IUserService, UserService>();
services.AddScoped<IRoleService, RoleService>();
services.AddAutoMapper(typeof(MappingProfile));
services.ConfigureCors();
services.AddMassTransit(x =>
    x.UsingRabbitMq((context, config) =>
    {
        config.Host("localhost", "/", h =>
        {
            h.Username("guest");
            h.Password("guest");
        });

        config.ConfigureEndpoints(context);
    })
);

services.AddControllers().AddNewtonsoftJson(options =>
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
);
services.AddEndpointsApiExplorer();
services.AddSwaggerGen();
var app = builder.Build();

app.UseMiddleware<ExceptionMiddleware>();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("CorsPolicy");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();