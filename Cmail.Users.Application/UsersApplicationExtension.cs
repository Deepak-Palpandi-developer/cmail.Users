using Cgmail.Common.Extensions;
using Cgmail.Common.Middlewares;
using Cmail.Users.Application.Services.Auth;
using Cmail.Users.Application.Services.Users;
using Cmail.Users.Domain.Data;
using Cmail.Users.Domain.Repository.Users;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Clients.ActiveDirectory;

namespace Cmail.Users.Application;

public static class UsersApplicationExtension
{
    public static IServiceCollection AddUsersApplicationExtension(this IServiceCollection services,
       string connectionString,
       string assemblyName)
    {
        services.CustomAddDBContext<UsersContext>(connectionString, assemblyName);

        services.AddScoped<IHmacService, HmacService>();

        services.AddScoped<IUserRepository, UserRepository>();

        services.AddScoped<IUserService, UserService>();

        services.AddScoped<IAuthService, AuthService>();

        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

        return services;
    }
}
