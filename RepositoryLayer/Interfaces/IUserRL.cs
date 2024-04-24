using ModelLayer.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Interfaces
{
    public interface IUserRL
    {

        Task<bool> UserRegister(UserRegisteration UserCredentials);
        Task<string> UserLogin(UserLoginModel UserCredentials);
    }
}
