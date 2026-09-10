using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using NotesApp.DataAccess.Interfaces;
using NotesApp.Domain.Models;
using NotesApp.Dtos;
using NotesApp.Mappers;
using NotesApp.Services.Configuration;
using NotesApp.Services.CustomExceptions;
using NotesApp.Services.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

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

    public async Task<LoginResponseDto> LoginAsync(LoginDto loginDto)
    {
        // 1) Validate the input
        if (string.IsNullOrWhiteSpace(loginDto.Username) ||
            string.IsNullOrWhiteSpace(loginDto.Password))
        {
            throw new UserDataException("Username and password are required fields.");
        }

        // 2) Find the user by Username
        User? userDb = await _userRepository.GetByUsernameAsync(loginDto.Username);

        // 3) Verify the password
        // BCrypt automatically handles the salt and the hashing algorithm, so we just need to call Verify with the plain password and the hashed password from the database.
        if (userDb is null || !BCrypt.Net.BCrypt.Verify(loginDto.Password, userDb.Password))
        {
            throw new InvalidCredentialsException("Username or password is incorrect.");
        }

        // 4) Generate token, once the credentials are verified.
        string token = GenerateToken(userDb);

        // The token will be used for authentication in subsequent requests so we return it to the client in the response.
        return new LoginResponseDto
        {
            Token = token
        };
    }

    private string GenerateToken(User user)
    {
        // Here we generate a JWT (JSON Web Token) for the authenticated user.
        // The token will contain claims about the user and will be signed to ensure its integrity and authenticity.

        // 1) Defining the Claims
        // Claims are pieces of information about the user that we want to include in the token.
        // They can be used for authorization and other purposes.
        // NameIdentifier: A unique identifier for the user (usually the user's ID).
        // Name: The username of the user.
        // Custom claims can also be added, such as the user's full name. They typically contain information about the user that might be useful for the application, such as roles, permissions, or other attributes.
        List<Claim> claims = new()
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Username),
            new Claim("fullName", user.FullName)
        };

        // 2) The signing key, from configuration.
        // Must be at least 32 characters long for HMACSHA256 algorithm. The key should be kept secret and not hard-coded in the source code.
        // It is recommended to store it in a secure configuration file or environment variable.
        SymmetricSecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey));

        // 3) Key + algorithm = the credentials that produce the SIGNATURE of the token
        // The signing credentials are used to sign the token, ensuring that it cannot be tampered with.
        // The algorithm used here is HMACSHA256, which is a secure hashing algorithm.
        // HMAC = Hash-based Message Authentication Code, which combines a cryptographic hash function with a secret key to provide data integrity and authenticity.
        // SHA256 = Secure Hash Algorithm 256-bit, which is a widely used cryptographic hash function that produces a fixed-size output (256 bits) from input data of any size.
        SigningCredentials signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        // 4) When the token expires
        // The expiration time is set to a certain number of minutes from the current UTC time, as specified in the configuration.
        // This ensures that the token is only valid for a limited time, enhancing security.
        DateTime expiresAtUtc = DateTime.UtcNow.AddMinutes(_jwtSettings.ExpirationInMinutes);

        // 5) Describe the token
        // The SecurityTokenDescriptor contains all the information needed to create the token, including the issuer, audience, claims, expiration time, and signing credentials.
        SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
        {
            Issuer = _jwtSettings.Issuer,
            Audience = _jwtSettings.Audience,
            Subject = new ClaimsIdentity(claims),
            Expires = expiresAtUtc,
            SigningCredentials = signingCredentials
        };

        // 6) Create token and serialize it to the 'Header.Payload.Signature' string
        // TokenHandler => responsible for creating and validating JWT tokens.
        JwtSecurityTokenHandler tokenHandler = new();
        // CreateToken => creates a new JWT token based on the provided token descriptor.
        SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);
        // WriteToken => serializes the token to a string in the standard JWT format (Header.Payload.Signature).
        string tokenString = tokenHandler.WriteToken(token);

        return tokenString;
    }
}
