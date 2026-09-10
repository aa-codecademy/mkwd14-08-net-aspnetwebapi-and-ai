using NotesApp.Dtos;

namespace NotesApp.Services.Interfaces;

public interface IAuthService
{
    Task<UserDto> RegisterAsync(RegisterDto registerDto);
    Task<LoginResponseDto> LoginAsync(LoginDto loginDto);
}
