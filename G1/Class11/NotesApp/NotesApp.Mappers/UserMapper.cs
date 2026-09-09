using NotesApp.Domain.Models;
using NotesApp.Dtos;

namespace NotesApp.Mappers;

public static class UserMapper
{
    public static UserDto ToUserDto(this User user)
    {
        return new UserDto
        {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Username = user.Username
        };
    }

    public static User ToUser(this RegisterDto registerDto, string passwordHash)
    {
        return new User
        {
            FirstName = registerDto.FirstName,
            LastName = registerDto.LastName,
            Username = registerDto.Username,
            Password = passwordHash
        };
    }

}
