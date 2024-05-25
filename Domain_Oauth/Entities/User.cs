using Microsoft.AspNetCore.Identity;

namespace Domain_Oauth.Entities
{
    public class User : IdentityUser
    {
        public string FullName { get; set; }  
        public string ImageUrl { get; set; }    
    }
}
