using Cmail.Users.Domain.Data;
using Cmail.Users.Domain.Entities.Users;
using Microsoft.EntityFrameworkCore;

namespace Cmail.Users.Domain.Repository.Users;

public interface IUserRepository
{
    Task<User?> GetUserDetailsAsync(Guid userId);
    Task<User?> GetUserDetailsAsync(string email);
    Task CreateUserAsync(User user);
    Task UpdateUserAsync(User user);
}

public class UserRepository : IUserRepository
{
    public readonly UsersContext _context;
    public UserRepository(UsersContext context)
    {
        _context = context;
    }

    public async Task<User?> GetUserDetailsAsync(Guid userId)
    {
        return await _context.Users.FirstOrDefaultAsync(x => x.Id == userId);
    }

    public async Task<User?> GetUserDetailsAsync(string email)
    {
        return await _context.Users.FirstOrDefaultAsync(x => x.Email == email);
    }

    public async Task CreateUserAsync(User user)
    {
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateUserAsync(User user)
    {
        _context.Users.Update(user);
        await _context.SaveChangesAsync();
    }
}
