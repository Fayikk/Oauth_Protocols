using Core_Oauth.Base;
using Core_Oauth.DTOs;
using Core_Oauth.Models;

namespace Core_Oauth.Abstraction
{
    public interface IUserRepository
    {
        Task<BaseResponseModel> Register(RegisterRequestDTO request);
        Task<BaseResponseModel> Login(LoginRequestDTO request);

        Task<BaseResponseModel> GoogleOAuthLogin(GoogleRequestModel requestModel);
        Task<BaseResponseModel> GithubOAuthLogin(GithubRequestModel requestModel);
        Task<BaseResponseModel> FacebookOAuthLogin(FacebookRequestModel requestModel);
    }
}
