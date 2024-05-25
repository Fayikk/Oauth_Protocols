using Core_Oauth.Models;
using Domain_Oauth.Entities;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Infrastructure_Oauth.Validator
{
    public static class HandleTokenValidator
    {
        public static async Task<TokenModel> HandleToken(IList<string> roles,User user)
        {
           
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();   
            byte[] key = Encoding.ASCII.GetBytes("mysupersecret_jwtkey_asasasasasasasasassas");
            SecurityTokenDescriptor tokenDescriptor = new()
            {
                Subject = new System.Security.Claims.ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id),
                    new Claim(ClaimTypes.Email, user.Email != null ? user.Email : "" ),
                    new Claim(ClaimTypes.GivenName, user.FullName != null ? user.FullName : ""),
                    new Claim("ImageIrl",user.ImageUrl != null ? user.ImageUrl : "")


                }),
                Expires = DateTime.Now.AddDays(7),
              
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);
            TokenModel tokenModel = new()
            {
                AccessToken = tokenHandler.WriteToken(token),
                Expiration = DateTime.Now.AddDays(7),
            };
            return tokenModel;  
        }
    }
}
