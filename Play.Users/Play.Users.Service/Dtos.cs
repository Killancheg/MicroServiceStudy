using System;
using System.ComponentModel.DataAnnotations;

namespace Play.Users.Service.Dtos
{
    public record UserDto(
        Guid Id,
        string Name,
        string Email,
        string Role,
        DateTimeOffset DateCreated);

    public record CreateUserDto(
        [Required] string Name,
        [Required] string Email,
        string Role);

    public record UpdateUserDto(
        [Required] string Name,
        [Required] string Email,
        string Role);
}