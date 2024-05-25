using Core_Oauth.Abstraction;
using Core_Oauth.DTOs;
using Core_Oauth.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api_Oauth.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }


        [HttpPost("SignIn")]
        public async Task<IActionResult> SignIn(LoginRequestDTO request)
        {
            var result = await _userRepository.Login(request);
            return Ok(result);
        }


        [HttpPost("SignUp")]  
        public async Task<IActionResult> SignUp(RegisterRequestDTO request)
        {
            var result = await _userRepository.Register(request);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> GoogleOAuthLogin(GoogleRequestModel request)
        {
            var result = await _userRepository.GoogleOAuthLogin(request);
            return Ok(result);  
        }

        [HttpPost("GithubLogin")]
        public async Task<IActionResult> GithubOAuthLogin([FromBody]GithubRequestModel request)
        {
            var result = await _userRepository.GithubOAuthLogin(request);
            return Ok(result);
        }

        [HttpPost("FacebookLogin")]
        public async Task<IActionResult> FacebookLogin([FromBody]FacebookRequestModel request)
        {
            var result = await _userRepository.FacebookOAuthLogin(request);
            return Ok(result);
        }
    }
}
