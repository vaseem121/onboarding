using smartTechAuthenticator.Services.Comman.CustomFilters;
using System.Web;
using System.Web.Mvc;

namespace smartTechAuthenticator
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute()); 
        }
    }
}
