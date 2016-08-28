using Microsoft.Owin.Security.OAuth;
using PPVR.WebApp.Configs.Filters;
using System.Web.Http;

namespace PPVR.WebApp
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            // Configure Web API to use only bearer token authentication.
            config.SuppressDefaultHostAuthentication();
            config.Filters.Add(new HostAuthenticationFilter(OAuthDefaults.AuthenticationType));

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                "DefaultApi",
                "api/{controller}/{id}",
                new {id = RouteParameter.Optional});

            // Enforcing SSL in a Web API Controller
            config.Filters.Add(new CustomRequireHttpsAttribute());
        }
    }
}