using BusinessLayer.Services;
using ModelLayer.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Interfaces
{
    public interface IUserBL
    {
        Task<bool> UserRegister(UserRegisteration UserCredentials);
        Task<string> UserLogin(UserLoginModel UserCredentials);
        Task<string> ForgotPassword(string Email);
        Task<bool> ResetPassword(PasswordResetModel Validations);
    }
}
