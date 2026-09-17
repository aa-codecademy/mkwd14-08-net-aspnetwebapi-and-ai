using MoviesApi.DTOs;

namespace MoviesApi.Services.Interfaces
{
    public interface IUserService
    {
        void RegisterUser(RegisterUserDto registerUserDto);

        string LoginUser(LoginUserDto loginUserDto);
    }
}
