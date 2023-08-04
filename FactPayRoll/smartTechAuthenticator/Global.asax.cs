
using smartTechAuthenticator.App_Start;
using smartTechAuthenticator.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace smartTechAuthenticator
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        { 
            AreaRegistration.RegisterAllAreas();
            UnityConfig.RegisterComponents();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            GlobalConfiguration.Configure(WebApiConfig.Register);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

        }
        protected void Application_AuthenticateRequest(object sender, EventArgs args)
        {
            if (Context.User != null)
            {
              //  IEnumerable<Role> roles = new UsersService.UsersClient().GetUserRoles(
                                                      //  Context.User.Identity.Name);


                string[] rolesArray = new string[Convert.ToInt32(Session["Role"])];
                //for (int i = 0; i < roles.Count(); i++)
                //{
                //    rolesArray[i] = roles.ElementAt(i).RoleName;
                //}

                GenericPrincipal gp = new GenericPrincipal(Context.User.Identity, rolesArray);
                Context.User = gp;
            }
        }
    }
}
