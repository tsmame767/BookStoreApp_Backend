using BusinessLayer.Interfaces;
using ModelLayer.DTO;
using RepositoryLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Services
{
    public class UserBL:IUserBL
    {
        private readonly IUserRL _registerService;

        public UserBL(IUserRL registerService)
        {
            _registerService = registerService;
        }
        public Task<bool> UserRegister(UserRegisteration UserCredentials)
        {
            return this._registerService.UserRegister(UserCredentials);
        }
        public Task<string> UserLogin(UserLoginModel UserCredentials)
        {
            return this._registerService.UserLogin(UserCredentials);
        }

    }
}
