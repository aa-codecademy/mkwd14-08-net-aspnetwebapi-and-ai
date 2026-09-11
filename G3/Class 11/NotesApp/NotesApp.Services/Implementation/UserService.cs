using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using NotesApp.DataAccess.Interfaces;
using NotesApp.Domain.Enums;
using NotesApp.Domain.Models;
using NotesApp.DTOs;
using NotesApp.Services.Interfaces;

namespace NotesApp.Services.Implementation
{
    public class UserService : IUserService
    {
        private IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public string LoginUser(LoginUserDto loginUserDto)
        {
            //validations
            if (loginUserDto == null)
            {
                throw new ArgumentNullException("Model cannot be null");
            }
            if (string.IsNullOrEmpty(loginUserDto.Username) || string.IsNullOrEmpty(loginUserDto.Password))
            {
                throw new ArgumentNullException("Username and password are required fields");
            }

            //we need to check if a user with this username and password exists in our db
            //in our db we saved the password as a hashed string, so we need to hash this password and compare the hash strings

            var hashedPassword = GenerateHash(loginUserDto.Password);
            var userDb = _userRepository.GetAll().FirstOrDefault(x => x.Username == loginUserDto.Username && x.Password == hashedPassword);

            if (userDb == null) {

                throw new NullReferenceException("Wrong username or password");
            }

            //generarte JWT token that we will later on use in our http requests for authentication/authorization

            JwtSecurityTokenHandler jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            var secretKey = Encoding.ASCII.GetBytes("Our secret secret secret secret secret secret secret secret key");
            var role = userDb.Username == "petko" ? RoleEnum.Admin : RoleEnum.StandardUser;

            SecurityTokenDescriptor securityTokenDescriptor = new SecurityTokenDescriptor
            {
                Expires = DateTime.UtcNow.AddHours(2),
                Subject = new System.Security.Claims.ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, loginUserDto.Username),
                    new Claim(ClaimTypes.Role, role.ToString()),
                    new Claim("id", userDb.Id.ToString()),
                    new Claim("userFullname", $"{userDb.FirstName} {userDb.LastName}")
                }),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(secretKey), SecurityAlgorithms.HmacSha256)
            };

            SecurityToken token = jwtSecurityTokenHandler.CreateToken(securityTokenDescriptor);
            string tokenString = jwtSecurityTokenHandler.WriteToken(token);

            return tokenString;
        }

        public void RegisterUser(RegisterUserDto registerUserDto)
        {
            //validation
            if (registerUserDto == null)
            {
                throw new ArgumentNullException("Model cannot be null");
            }
            if (string.IsNullOrEmpty(registerUserDto.Firstname) || string.IsNullOrEmpty(registerUserDto.Lastname))
            {
                throw new ArgumentNullException("Firstname and lastname are required fields");
            }
            if (string.IsNullOrEmpty(registerUserDto.Username))
            {
                throw new ArgumentNullException("Username is a required field");
            }
            if (string.IsNullOrEmpty(registerUserDto.Password))
            {
                throw new ArgumentNullException("Password is a required field");
            }
            if (registerUserDto.Password != registerUserDto.ConfirmPassword)
            {
                throw new ArgumentNullException("Passwords must match");
            }
            if (_userRepository.GetUserByUsername(registerUserDto.Username) != null)
            {
                throw new ArgumentException("User with that username already exists");
            }

            //create new user
            User user = new User
            {
                FirstName = registerUserDto.Firstname,
                LastName = registerUserDto.Lastname,
                Username = registerUserDto.Username,
                Password = GenerateHash(registerUserDto.Password) //we need to hash our password before saving it to the db
            };

            _userRepository.Add(user);
        }

        private string GenerateHash(string password)
        {
            //here we use md5 for our simple example, but for bigger apps a safer algorithm should be used
            using var md5 = MD5.Create();
            var passwordBytes = Encoding.ASCII.GetBytes(password);
            var hashBytes = md5.ComputeHash(passwordBytes);
            var hashedString = Encoding.ASCII.GetString(hashBytes);

            return hashedString;
        }
    }
}
