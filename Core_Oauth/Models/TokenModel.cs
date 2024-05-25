namespace Core_Oauth.Models
{
    public class TokenModel
    {
        public string AccessToken { get; set; } 
        public DateTime Expiration { get; set; }
    }
}
