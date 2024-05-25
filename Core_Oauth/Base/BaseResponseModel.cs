using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core_Oauth.Base
{
    public class BaseResponseModel
    {
        public bool isSuccess { get; set; } 
        public string Message { get; set; }
        public object Data { get; set; }    
    }
}
