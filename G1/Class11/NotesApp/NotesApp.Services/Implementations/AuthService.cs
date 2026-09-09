using Microsoft.Extensions.Options;
using NotesApp.DataAccess.Interfaces;
using NotesApp.Domain.Models;
using NotesApp.Dtos;
using NotesApp.Mappers;
using NotesApp.Services.Configuration;
using NotesApp.Services.CustomExceptions;
using NotesApp.Services.Interfaces;

namespace NotesApp.Services.Implementations;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly JwtSettings _jwtSettings;

    public AuthService(
        IUserRepository userRepository,
        IOptions<JwtSettings> jwtSettings)
    {
        _userRepository = userRepository;
        _jwtSettings = jwtSettings.Value;
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
        // The hashing algorithm used is BCrypt, which is a widely used and secure hashing algorithm for passwords.
        // It automatically handles salting and is designed to be slow to mitigate brute-force attacks.
        // Same password twice => two different hashes, thanks to the random salt generated for each hash.
        // "SuperSecret123" => $2a$11$IL1LwfXd72Z/Vw6vPxpghO/.h/ZTauAf75DGVuY1LuMka/iRW3Ezy

        // In relation to SHA algorithms, BCrypt is generally considered more secure for password hashing because it is specifically designed for that purpose. SHA algorithms (like SHA-256) are fast and not suitable for password hashing as they can be brute-forced more easily. BCrypt's slowness and built-in salting make it a better choice for securely storing passwords.

        // 4) Map to User
        User newUser = registerDto.ToUser(passwordHash);

        // 5) Save the new User
        await _userRepository.AddAsync(newUser);

        // 6) Return the UserDto
        return newUser.ToUserDto();
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
