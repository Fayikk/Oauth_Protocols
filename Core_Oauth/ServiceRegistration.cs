using Core_Oauth.Base;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Core_Oauth
{
    public static class ServiceRegistration
    {
        public static void AddCoreServices(this IServiceCollection serviceCollection,IConfiguration configuration = null)
        {
            serviceCollection.AddScoped(typeof(BaseResponseModel));
        }
    }
}
