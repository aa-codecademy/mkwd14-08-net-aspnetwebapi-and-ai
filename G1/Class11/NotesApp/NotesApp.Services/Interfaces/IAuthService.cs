using NotesApp.Dtos;

namespace NotesApp.Services.Interfaces;

public interface IAuthService
{
    Task<UserDto> RegisterAsync(RegisterDto registerDto);
}
