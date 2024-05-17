using Play.Users.Service.Dtos;
using Play.Users.Service.Entities;

namespace Play.Users.Service.Extensions
{
    public static class UserExtension
    {
        public static UserDto AsDto(this User user)
        {
            return new UserDto(
                user.Id,
                user.Name,
                user.Email,
                user.Role,
                user.CreatedDate);
        }
    }
}
