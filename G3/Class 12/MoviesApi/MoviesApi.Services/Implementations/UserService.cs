using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.Identity.Client;
using Microsoft.IdentityModel.Tokens;
using MoviesApi.DataAccess.Interfaces;
using MoviesApi.Domain.Enums;
using MoviesApi.Domain.Models;
using MoviesApi.DTOs;
using MoviesApi.Services.Interfaces;

namespace MoviesApi.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public string LoginUser(LoginUserDto loginUserDto)
        {
            //validations
            if (loginUserDto == null) {

                throw new ArgumentNullException("Model cannot be null");
            }

            if(string.IsNullOrEmpty(loginUserDto.Username) || string.IsNullOrEmpty(loginUserDto.Password))
            {
                throw new ArgumentNullException("Username and password are required fields");
            }

            var hashedPassword = GenerateHash(loginUserDto.Password);
            var userDb = _userRepository.GetAll().FirstOrDefault(x => x.Username.ToLower() ==  loginUserDto.Username.ToLower() && x.Password == hashedPassword);

            if(userDb == null)
            {
                throw new NullReferenceException("Wrong username or password");
            }

            //generate JWT token that we will use for authentication/authorization
            JwtSecurityTokenHandler jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            var secretKey = Encoding.ASCII.GetBytes("Our secret secret secret secret secret secret secret secret key");
            var role = userDb.Username == "petko" ? Roles.Admin : Roles.User;

            SecurityTokenDescriptor securityTokenDescriptor = new SecurityTokenDescriptor
            {
                Expires = DateTime.Now.AddHours(2),
                Subject = new System.Security.Claims.ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, loginUserDto.Username),
                    new Claim(ClaimTypes.Role, role),
                    new Claim("id", userDb.Id.ToString()),
                    new Claim("userFullname", $"{userDb.Firstname} {userDb.Lastname}")
                }),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(secretKey), SecurityAlgorithms.HmacSha256Signature)
            };

            SecurityToken token = jwtSecurityTokenHandler.CreateToken(securityTokenDescriptor);
            string tokenString = jwtSecurityTokenHandler.WriteToken(token);
            return tokenString;
        }

        public void RegisterUser(RegisterUserDto registerUserDto)
        {
            //validations
            if (registerUserDto == null)
            {
                throw new ArgumentNullException("Model cannot be null");
            }

            if (string.IsNullOrEmpty(registerUserDto.Firstname) || string.IsNullOrEmpty(registerUserDto.Lastname))
            {

                throw new ArgumentNullException("Firstaname and lastname are required fields");
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
                throw new ArgumentException($"User with username {registerUserDto.Username} already exists");
            }

            string strongPasswordRegex = "^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[^a-zA-Z\\d]).{5,}$";

            if (!Regex.IsMatch(registerUserDto.Password, strongPasswordRegex))
            {

                throw new ArgumentException("Password is not a strong password");
            }

            //create the new user

            User user = new User
            {
                Firstname = registerUserDto.Firstname,
                Lastname = registerUserDto.Lastname,
                Username = registerUserDto.Username,
                Password = GenerateHash(registerUserDto.Password) //we need to hash our password before writing it in our db. We should never store our passwords as plain text
            };

            //add to the db
            _userRepository.Add(user);  
        }

        #region Helpers

        private string GenerateHash(string password)
        {
            using var md5Has = MD5.Create();
            var passwordBytes = Encoding.ASCII.GetBytes(password);
            var hashedBytes = md5Has.ComputeHash(passwordBytes);
            var hashedPassword = Encoding.ASCII.GetString(hashedBytes);

            return hashedPassword;
        }

        #endregion
    }
}

