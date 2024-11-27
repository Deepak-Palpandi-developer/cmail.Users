using AutoMapper;
using Cmail.Users.Application.Dto.Users;
using Cmail.Users.Domain.Entities.Users;

namespace Cmail.Users.Application.Mappers;

public class UsersMapper : Profile
{
    public UsersMapper()
    {
        CreateMap<User, UserDto>().ReverseMap();
    }
}
