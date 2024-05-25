using Core_Oauth.Abstraction;
using Core_Oauth.Base;
using Core_Oauth.Constants;
using Core_Oauth.DTOs;
using Core_Oauth.Models;
using Domain_Oauth.Entities;
using Google.Apis.Auth;
using Infrastructure_Oauth.Validator;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Persistence_Oauth.Database;
using System.Diagnostics;

namespace Persistence_Oauth.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _context;
        private BaseResponseModel _responseModel;
        private string securityPass;
        public UserRepository(UserManager<User> userManager,BaseResponseModel responseModel,RoleManager<IdentityRole> roleManager,ApplicationDbContext context,IConfiguration configuration)
        {
            securityPass = configuration.GetValue<string>("PasswordOauth");
            _responseModel = responseModel;
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
        }

        public async Task<BaseResponseModel> FacebookOAuthLogin(FacebookRequestModel requestModel)
        {
            User userExist = await _context.Users.FirstOrDefaultAsync(x => x.Email == requestModel.Email && x.FullName.ToLower() == requestModel.FullName.ToLower());
            if (userExist is null)
            {
                _responseModel.isSuccess = true;
                User newUser = new()
                {
                    Email = requestModel.Email,
                    FullName = requestModel.FullName,
                    ImageUrl = requestModel.ImageUrl,
                    UserName = "GithubOAuthId"+Guid.NewGuid().ToString(),

                };
                var result = await _userManager.CreateAsync(newUser, securityPass);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(newUser, Roles.User.ToString());
                }
            }
            User userFromDb = await _context.Users.FirstOrDefaultAsync(x => x.FullName.ToLower() == requestModel.FullName.ToLower());
            if (userFromDb is not null)
            {
                var roles = await _userManager.GetRolesAsync(userFromDb);
                var model = await HandleTokenValidator.HandleToken(roles, userFromDb);
                await _context.SaveChangesAsync();
                _responseModel.isSuccess = true;
                _responseModel.Data = model;
                return _responseModel;
            }
            return null;
        }

        public async Task<BaseResponseModel> GithubOAuthLogin(GithubRequestModel requestModel)
        {
            User userExist = await _context.Users.FirstOrDefaultAsync(x => x.Email == requestModel.Email);
            if (userExist is null)
            {
                _responseModel.isSuccess = true;
                User newUser = new()
                {
                    UserName = requestModel.UserName,
                    Email = requestModel.Email,
                    FullName = requestModel.FullName,
                    ImageUrl = requestModel.ImageUrl

                };
                var result = await _userManager.CreateAsync(newUser, securityPass);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(newUser, Roles.User.ToString());
                }
            }
            User userFromDb = await _context.Users.FirstOrDefaultAsync(x => x.UserName.ToLower() == requestModel.UserName.ToLower());
            if (userFromDb is not null)
            {
                var roles = await _userManager.GetRolesAsync(userFromDb);
                var model = await HandleTokenValidator.HandleToken(roles, userFromDb);
                await _context.SaveChangesAsync();
                _responseModel.isSuccess = true;
                _responseModel.Data = model;
                return _responseModel;
            }
            return null;
        }

        public async Task<BaseResponseModel> GoogleOAuthLogin(GoogleRequestModel requestModel)
        {
            try
            {
                var payload = GoogleJsonWebSignature.ValidateAsync(requestModel.credentialToken, new GoogleJsonWebSignature.ValidationSettings()).Result;
                User userExist = await _context.Users.FirstOrDefaultAsync(x => x.Email == payload.Email);
                if (userExist is null)
                {
                    _responseModel.isSuccess = true;
                    User newUser = new()
                    {
                        UserName = payload.GivenName,
                        Email = payload.Email,
                        FullName = payload.GivenName + payload.FamilyName,

                    };
                    var result = await _userManager.CreateAsync(newUser, securityPass);
                    if (result.Succeeded)
                    {
                        await _userManager.AddToRoleAsync(newUser, Roles.User.ToString());
                    }
                }
                User userFromDb = await _context.Users.FirstOrDefaultAsync(x => x.Email.ToLower() == payload.Email.ToLower());
                if (userFromDb is not null)
                {
                    var roles = await _userManager.GetRolesAsync(userFromDb);
                    var model = await HandleTokenValidator.HandleToken(roles, userFromDb);
                    await _context.SaveChangesAsync();
                    _responseModel.isSuccess = true;
                    _responseModel.Data = model;
                    return _responseModel;
                }
                return null;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                throw ex;
            }
        }

        public async Task<BaseResponseModel> Login(LoginRequestDTO request)
        {
            User user = await _context.Users.FirstOrDefaultAsync(x => x.UserName.ToLower() == request.UserName.ToLower());
            if (user != null)
            {
                bool isValid = await _userManager.CheckPasswordAsync(user, request.Password);
                if (!isValid)
                {
                    _responseModel.isSuccess = false;
                    _responseModel.Message = "Is Not Correct Pass";
                    return _responseModel;  
                }

                var roles = await _userManager.GetRolesAsync(user);
                var token = await HandleTokenValidator.HandleToken(roles, user);
                _responseModel.Data = token;
                _responseModel.isSuccess = true;
                return _responseModel;

            }
            _responseModel.isSuccess = false;
            _responseModel.Message = "User is not found";
            return _responseModel;
        }

        public async Task<BaseResponseModel> Register(RegisterRequestDTO request)
        {


        User user = await _context.Users.FirstOrDefaultAsync(x => x.UserName.ToLower() == request.UserName.ToLower());
            if (user != null)
            {
                _responseModel.isSuccess = false;
                _responseModel.Message = "Username alreadyy exist";
                return _responseModel;
            }
            User newUser = new()
            {
                UserName = request.UserName,
                Email = request.Email,
                FullName = request.FullName,
            };

            var result = await _userManager.CreateAsync(newUser,request.Password);
            if (result.Succeeded)
            {
                if (!_roleManager.RoleExistsAsync(Roles.Admin).GetAwaiter().GetResult())
                {
                    await _roleManager.CreateAsync(new IdentityRole(Roles.Admin));
                    await _roleManager.CreateAsync(new IdentityRole(Roles.User));
                    await _userManager.AddToRoleAsync(newUser, Roles.Admin.ToString());
                }
                if (_roleManager.RoleExistsAsync(Roles.Admin).GetAwaiter().GetResult())
                {
                    await _userManager.AddToRoleAsync(newUser, Roles.User.ToString());
                }
                _responseModel.isSuccess = true;
                return _responseModel;
            }
            _responseModel.Message = "Oopps something wrong";
            return _responseModel;
        }
    }
}
