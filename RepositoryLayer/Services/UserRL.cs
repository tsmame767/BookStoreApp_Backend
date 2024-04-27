using Dapper;
using Microsoft.Extensions.Configuration;
using ModelLayer.DTO;
using RepositoryLayer.Database;
using RepositoryLayer.Entity;
using RepositoryLayer.Interfaces;
using RepositoryLayer.JWT;
using System;
using System.Collections;
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
        private readonly ICacheServiceRL _cacheServiceRL;

        public UserRL(DBContext dbContext,IConfiguration configuration, ICacheServiceRL cacheService)
        {
            _dbContext = dbContext;
            _configuration = configuration;
            _cacheServiceRL = cacheService;
        }

        public async Task<bool> UserRegister(UserRegisteration UserCredentials)
        {
            using (var connect = _dbContext.CreateConnection())
            {
                string query = "Insert into [User](Name,Email,Phone,City,Password,Role) values(@Name,@Email,@Phone,@City,@Password,@Role)";

                string query2 = "Insert into ShoppingCart(user_Id) values(@userId)"; // Creating shopping cart of user registered

                //string query3 = "Insert into [Order](Customer_Id) values(@userId)"; // Creating Orders table of a user registered

                // Check if the email already exists in the database
                int checkmail = await connect.QueryFirstOrDefaultAsync<int>(
                    "select COUNT(*) from [User] where Email=@Email", new { Email = UserCredentials.Email });

                if (checkmail > 0)
                {
                    throw new Exception("Email Already Exists");
                }
                var res = await connect.ExecuteAsync(query,
                    new {
                        Name=UserCredentials.Name, 
                        Email = UserCredentials.Email, 
                        Phone = UserCredentials.Phone, 
                        City = UserCredentials.City,
                        Password= BCrypt.Net.BCrypt.HashPassword(UserCredentials.Password), 
                        Role = UserCredentials.Role
                    });

                if (res > 0)
                {
                    //take currently added customer id
                    if (UserCredentials.Role == "customer")
                    {

                        var CurrentRegisteredUserData = await connect.QueryFirstOrDefaultAsync<int>("select Top 1 user_id from [User] order by user_id desc");
                        await connect.ExecuteAsync(query2, new { userId = CurrentRegisteredUserData });
                        //await connect.ExecuteAsync(query3, new { userId = CurrentRegisteredUserData });
                    }
                    return true;
                }
            }
            return false;
        }

        public async Task<string> UserLogin(UserLoginModel UserCredentials)
        {
            using(var connect = _dbContext.CreateConnection())
            {
                var query = "Select * from [User] where Email=@Email";

                var res = await connect.QueryFirstOrDefaultAsync<UserVerifyModel>(query,
                    new
                    {
                        Email = UserCredentials.Email
                        
                    });
                if(res!=null && BCrypt.Net.BCrypt.Verify(UserCredentials.Password, res.Password))
                {
                    TokenGenerator Token = new TokenGenerator(_configuration);
                    var data = Token.GenerateJwtToken(res.User_Id, res.Email, res.Role);
                    return data;

                }
            }
            return null;
        }

        public async Task<string> ForgotPassword(string Email)
        {
            var otp = "" ;
            var query = "select Phone from [User] where Email=@Email";

            using(var connect=_dbContext.CreateConnection())
            {   var Value= await connect.QueryFirstOrDefaultAsync(query,new {Email = Email});
                var CacheKey = $"User_{Value.Phone}";
                int checkmail = await connect.QueryFirstOrDefaultAsync<int>(
                    "select COUNT(*) from [User] where Email=@Email", new { Email = Email });

                if (checkmail == 0)
                {
                    throw new Exception("User Doesn't Exists");
                }

                otp = new Random().Next(100000, 999999).ToString();
                _cacheServiceRL.SetData(CacheKey, otp, DateTimeOffset.Now.AddMinutes(5));
                return otp;
            }

        }

        public async Task<bool> ResetPassword(PasswordResetModel Validations)
        {
            var checkEmail = 0;
            var query = "select Phone from [User] where Email=@Email";
            var UpdatePassQuery= "update [User] set password=@password where email=@email";
            using (var connect = _dbContext.CreateConnection())
            {
                //Get CacheKey from Redis Cache
                var Value = await connect.QueryFirstOrDefaultAsync(query, new { Email = Validations.Email });
                var CacheKey = $"User_{Value.Phone}";

                //Check if the Entered Email is present in the DB
                checkEmail = await connect.QueryFirstOrDefaultAsync<int>(
                    "select COUNT(*) from [User] where Email=@Email", new { Email = Validations.Email });
                if(checkEmail == 0)
                {
                    throw new Exception("User Doesn't Exists");
                }

                //Check if OTP is valid
                var cachedOtp = _cacheServiceRL.GetData<string>(CacheKey);
                if(cachedOtp == null || cachedOtp!=Validations.OTP)
                {
                    throw new Exception("Invalid or Expired OTP!");
                }

                //Reset Password
                var res = await connect.ExecuteAsync(UpdatePassQuery, new { password = BCrypt.Net.BCrypt.HashPassword(Validations.NewPassword), email = Validations.Email });
                if (res == 0)
                {
                    return false;
                }
                _cacheServiceRL.RemoveData(CacheKey); //Removes the key once the otp is validated and used 
                return true;
            }
        }
    }
}
