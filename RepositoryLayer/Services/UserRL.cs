using Dapper;
using Microsoft.Extensions.Configuration;
using ModelLayer.DTO;
using RepositoryLayer.Database;
using RepositoryLayer.Interfaces;
using RepositoryLayer.JWT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Services
{
    public class UserRL:IUserRL
    {
        private readonly DBContext _dbContext;
        private readonly IConfiguration _configuration;

        public UserRL(DBContext dbContext,IConfiguration configuration)
        {
            _dbContext = dbContext;
            _configuration = configuration;
        }

        public async Task<bool> UserRegister(UserRegisteration UserCredentials)
        {
            using (var connect = _dbContext.CreateConnection())
            {
                string query = "Insert into Customer(Name,Email,Phone,Password,Role) values(@Name,@Email,@Phone,@Password,@Role)";

                //DynamicParameters parameters = new DynamicParameters();
                //parameters.Add("")
                var res = await connect.ExecuteAsync(query,
                    new {
                        Name=UserCredentials.Name, 
                        Email = UserCredentials.Email, 
                        Phone = UserCredentials.Phone, 
                        Password= BCrypt.Net.BCrypt.HashPassword(UserCredentials.Password), 
                        Role = UserCredentials.Role
                    });

                if (res > 0)
                {
                    return true;
                }
            }
            return false;
        }

        public async Task<string> UserLogin(UserLoginModel UserCredentials)
        {
            using(var connect = _dbContext.CreateConnection())
            {
                var query = "Select * from Customer where Email=@Email";

                var res = await connect.QueryFirstOrDefaultAsync<UserVerifyModel>(query,
                    new
                    {
                        Email = UserCredentials.Email
                        
                    });
                if(res!=null && BCrypt.Net.BCrypt.Verify(UserCredentials.Password, res.Password))
                {
                    TokenGenerator Token = new TokenGenerator(_configuration);
                    var data = Token.GenerateJwtToken(res.Customer_Id, res.Email, res.Role);
                    return data;

                }
            }
            return null;
        }
    }
}
