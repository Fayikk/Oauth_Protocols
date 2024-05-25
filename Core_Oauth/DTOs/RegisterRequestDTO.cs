using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core_Oauth.DTOs
{
    public class RegisterRequestDTO
    {
        public string UserName { get; set; }    
        public string FullName { get; set; }    
        public string Email { get; set; }
        public string Password { get; set; }

        public string ConfirmPassword { get; set; } 
    }
}
