using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core_Oauth.Models
{
    public class FacebookRequestModel
    {
        public string? Email { get; set; }   
        public string? ImageUrl { get; set; }    
        public string? FullName { get; set; }
    }
}
