using AutoMapper;
using CsvHelper;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using ToDo_Common.Exceptions;
using ToDo_Common.Helper;
using ToDo_Core.Manager.Interface;
using ToDo_MainProject.Model;
using ToDo_ModelView.ModelView;

namespace ToDo_Core.Manager
{
    public class UserManager : IUserManager
    {
        #region public

        private DB_ToDoContext _DB;
        private IMapper _mapper;

        public UserManager(DB_ToDoContext _DB, IMapper mapper)
        {
            this._DB = _DB;
            _mapper = mapper;
        }


        public LoginUserResponse Login(LoginModelView userReg)
        {
            var user = _DB.Users
                                .FirstOrDefault(a => a.Email.ToLower().Equals(userReg.Email.ToLower()));

            if (user == null || !VerifyHashPassword(userReg.Password, user.Password))
            {
                throw new ServiceValidationException(300, "Invalid user name or password received");
            }

            var res = _mapper.Map<LoginUserResponse>(user);
            res.Token = $"Bearer {GenerateJWTToken(user)}";
            return res;
        }


        public LoginUserResponse SignUp(UserRegistrationModel userReg)
        {
            if (_DB.Users.Any(a => a.Email.ToLower().Equals(userReg.Email.ToLower())))
            {
                throw new ServiceValidationException("User already exist");
            }

            var hashedPassword = HashPassword(userReg.Password);

            var user = _DB.Users.Add(new User
            {
                FirstName = userReg.FirstName,
                LastName = userReg.LastName,
                Email = userReg.Email.ToLower(),
                Password = hashedPassword,
                ConfirmPassword = hashedPassword,
                Images = string.Empty
            }).Entity;

            _DB.SaveChanges();

            var res = _mapper.Map<LoginUserResponse>(user);
            res.Token = $"Bearer {GenerateJWTToken(user)}";

            return res;

        }

        public UserModel UpdateProfile(UserModel currentUser, UserUpdatedModel request)
        {
            var user = _DB.Users
                                .FirstOrDefault(a => a.Id == currentUser.Id)
                                 ?? throw new ServiceValidationException("User not found");

            var url = "";

            if (!string.IsNullOrWhiteSpace(request.ImageString))
            {
                url = Helper.SaveImage(request.ImageString, "profileimages");
            }

            user.FirstName = request.FirstName;
            user.LastName = request.LastName;

            if (!string.IsNullOrWhiteSpace(url))
            {
                var baseURL = "https://localhost:44309/";
                user.Images = @$"{baseURL}/api/v1/user/fileretrive/profilepic?filename={url}";
            }

            _DB.SaveChanges();
            return _mapper.Map<UserModel>(user);
        }


        public void DeleteUser(UserModel currentUser, int id)
        {
            if (currentUser.Id == id)
            {
                throw new ServiceValidationException("You have no access to delete your self");
            }

            var user = _DB.Users
                          .FirstOrDefault(a => a.Id == id)
                          ?? throw new ServiceValidationException("User not found");
            // for soft delete
            user.Archived = 1;
            _DB.SaveChanges();
        }


        public List<UserModel> GetAll()
        {
            return _mapper.Map<List<UserModel>>(_DB.Users.ToList());
        }


        #endregion public

        #region private

        private static string HashPassword(string password)
            {
                string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);

                return hashedPassword;
            }

            private static bool VerifyHashPassword(string password, string HashedPasword)
            {
                return BCrypt.Net.BCrypt.Verify(password, HashedPasword);
            }

            private string GenerateJWTToken(User user)
            {
                var jwtKey = "#test.key*&^vanthis%$^&*()$%^@#$@!@#%$#^%&*%^*";
                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
                var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

                var claims = new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, $"{user.FirstName} {user.LastName}"),
                    new Claim(JwtRegisteredClaimNames.Email, user.Email),
                    new Claim("Id", user.Id.ToString()),
                    new Claim("FirstName", user.FirstName),
                    new Claim("DateOfJoining", user.CreatedDate.ToString("yyyy-MM-dd")),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                };

                var issuer = "test.com";

                var token = new JwtSecurityToken(
                            issuer,
                            issuer,
                            claims,
                            expires: DateTime.Now.AddDays(20),
                            signingCredentials: credentials);

                return new JwtSecurityTokenHandler().WriteToken(token);
            }


        #endregion private



    }
}
