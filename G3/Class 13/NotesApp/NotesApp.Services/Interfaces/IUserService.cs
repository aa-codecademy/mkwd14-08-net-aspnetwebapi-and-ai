using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NotesApp.DTOs;

namespace NotesApp.Services.Interfaces
{
    public interface IUserService
    {
        void RegisterUser(RegisterUserDto registerUserDto);

        string LoginUser(LoginUserDto loginUserDto); //the result of the login process is the generated JWT token
    }
}
