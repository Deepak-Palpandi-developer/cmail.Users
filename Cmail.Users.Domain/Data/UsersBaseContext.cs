using Cmail.Users.Domain.Entities.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cmail.Users.Domain.Data;

public class UsersBaseContext<T> : DbContext where T : DbContext
{
    public UsersBaseContext(DbContextOptions<T> options) : base(options)
    { }

    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(ConfigureUserEntity);
    }

    private static void ConfigureUserEntity(EntityTypeBuilder<User> entity)
    {
        entity.HasKey(u => u.Id);
        entity.HasIndex(u => u.Email).IsUnique();
        entity.Property(u => u.Email).HasMaxLength(200);
        entity.Property(u => u.FirstName).HasMaxLength(100);
        entity.Property(u => u.LastName).HasMaxLength(100);
        entity.Property(u => u.Gender).HasMaxLength(20);
        entity.Property(u => u.Phone).HasMaxLength(15);
        entity.Property(u => u.PrefixPhone).HasMaxLength(5);
    }

}


public class UsersContext : UsersBaseContext<UsersContext>
{
    public UsersContext(DbContextOptions<UsersContext> options) : base(options)
    {
    }
}
