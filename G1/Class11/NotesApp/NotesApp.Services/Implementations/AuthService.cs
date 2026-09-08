using NotesApp.DataAccess.Interfaces;
using NotesApp.Domain.Models;
using NotesApp.Dtos;
using NotesApp.Services.CustomExceptions;
using NotesApp.Services.Interfaces;

namespace NotesApp.Services.Implementations;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;

    public AuthService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<UserDto> RegisterAsync(RegisterDto registerDto)
    {
        // 1) Validate the input data
        ValidateRegistration(registerDto);

        // 2) Validate Username
        bool usernameExists = await _userRepository.CheckUsernameExistsAsync(registerDto.Username);

        if (usernameExists)
        {
            throw new UserDataException($"Username '{registerDto.Username}' is already taken.");
        }

        // 3) Hash the password
        string passwordHash = BCrypt.Net.BCrypt.HashPassword(registerDto.Password);
        // "SuperSecret123" => $2a$11$IL1LwfXd72Z/Vw6vPxpghO/.h/ZTauAf75DGVuY1LuMka/iRW3Ezy

        // 4) Map to User
        User newUser = new User
        {
            FirstName = registerDto.FirstName,
            LastName = registerDto.LastName,
            Username = registerDto.Username,
            Password = passwordHash
        };

        // 5) Save the new User
        await _userRepository.AddAsync(newUser);

        // 6) Return the UserDto
        return new UserDto
        {
            Id = newUser.Id,
            FirstName = registerDto.FirstName,
            LastName = registerDto.LastName,
            Username = newUser.Username,
        };
    }

    private void ValidateRegistration(RegisterDto registerDto)
    {
        if (string.IsNullOrWhiteSpace(registerDto.FirstName) || registerDto.FirstName.Length > 100)
        {
            throw new UserDataException("First name is required and cannot exceed 100 characters.");
        }
        if (string.IsNullOrWhiteSpace(registerDto.LastName) || registerDto.LastName.Length > 100)
        {
            throw new UserDataException("Last name is required and cannot exceed 100 characters.");
        }
        if (string.IsNullOrWhiteSpace(registerDto.Username) || registerDto.Username.Length > 30)
        {
            throw new UserDataException("Username is required and cannot exceed 30 characters.");
        }

        if (registerDto.Password.Length < 8)
        {
            throw new UserDataException("Password must be at least 8 characters long.");
        }
        if (registerDto.Password != registerDto.ConfirmPassword)
        {
            throw new UserDataException("Passwords do not match.");
        }
    }
}
