
using Cgmail.Common.Exceptions;
using Cgmail.Common.Extensions;
using Cgmail.Common.Middlewares;
using Cgmail.Common.Model;
using Cmail.Users.Application;
using Cmail.Users.Domain.Data;
using System.Reflection;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;

builder.Services.CustomAddControllers();

configuration.AddEnvironmentVariables("CLONE_");

var jwtSettings = builder.Configuration.GetSection("JwtSettings").Get<JwtSettings>();

var key = Encoding.UTF8.GetBytes(jwtSettings!.Key ?? string.Empty);

string connectionString = configuration.GetValue<string>("AuthConnectionString") ?? string.Empty;

string? assemblyName = Assembly.GetExecutingAssembly().GetName().Name ?? string.Empty;

builder.Services.CustomAddCors(configuration);

builder.Services.AddUsersApplicationExtension(connectionString, assemblyName);

builder.CustomAddSeriLog(configuration, connectionString);

builder.Services.CustomAddRedisCache(configuration, "Authentication:");

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

builder.Services.AddJwtAuthentication(builder.Configuration);

builder.Services.AddAuthorization();

var app = builder.Build();

app.MigrateDatabase<UsersContext>();

app.CustomUseSwagger();

app.UseMiddleware<ErrorHandlerMiddleware>();

app.UseHttpsRedirection();

app.CustomUseForwardedHeaders();

app.UseCors();

app.UseAuthentication();

app.UseAuthorization();

app.UseMiddleware<EncryptionMiddleware>();

app.MapControllers();

app.Run();

//migration CLI - Add-Migration YourMigrationName -Project Cmail.Users.Api