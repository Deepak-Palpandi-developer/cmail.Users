using Cgmail.Common;
using Cmail.Users.Domain.Data;

namespace Cmail.Users.Domain;

public interface IUsersUnitOfWork : IUnitOfWork
{ }

public class UsersUnitOfWork : UnitOfWork, IUsersUnitOfWork
{
    public UsersUnitOfWork(UsersContext context) : base(context) { }
}
