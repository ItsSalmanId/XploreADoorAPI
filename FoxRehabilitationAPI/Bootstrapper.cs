using System.Web.Mvc;
using Microsoft.Practices.Unity;
using Unity.Mvc3;
using FOX.Resolver;
using System.Web.Http;
namespace FoxRehabilitationAPI
{
    public static class Bootstrapper
    {
        public static void Initialise()
        {
            var container = BuildUnityContainer();

            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
            GlobalConfiguration.Configuration.DependencyResolver = new Unity.WebApi.UnityDependencyResolver(container);
        }

        private static IUnityContainer BuildUnityContainer()
        {
            var container = new UnityContainer();

            // register all your components with the container here
            // it is NOT necessary to register your controllers

            // e.g. container.RegisterType<ITestService, TestService>();            
            RegisterTypes(container);
            return container;
        }

        public static void RegisterTypes(IUnityContainer container)
        {

            //Component initialization via MEF
            ComponentLoader.LoadContainer(container, ".\\bin", "FoxRehabilitationAPI.dll");
            ComponentLoader.LoadContainer(container, ".\\bin", "FOX.BusinessOperations.dll");

        }
    }
}