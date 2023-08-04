using System.Web.Mvc;
using Unity;
using Unity.Mvc5;
using smartTechAuthenticator.Services.Account;
using smartTechAuthenticator.Services.Comman;
using smartTechAuthenticator.Services.Customers;
using smartTechAuthenticator.Services.Mall;
using smartTechAuthenticator.Services.Tickets;

namespace smartTechAuthenticator
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
			var container = new UnityContainer();

            // register all your components with the container here
            // it is NOT necessary to register your controllers

            // e.g. container.RegisterType<ITestService, TestService>();
            container.RegisterType<IAccountRepo, AccountRepo>();
            container.RegisterType<ICommanServices, CommanServices>();
            container.RegisterType<ICustomersService, CustomersService>();
            container.RegisterType<IMallService, MallService>();
            container.RegisterType<ITicketService, TicketService>();
            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }
    }
}