using System.Web.Mvc;

namespace PPVR.WebApp
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            // The RequireHttps requires that all access to the web app be through HTTPS.
            filters.Add(new RequireHttpsAttribute());
        }
    }
}