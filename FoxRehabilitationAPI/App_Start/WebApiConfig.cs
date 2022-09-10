using System.Web.Http;
using Microsoft.Owin.Security.OAuth;
using Unity;
using FoxRehabilitationAPI.App_Start;
using FOX.BusinessOperations.OriginalQueueService;
using FOX.BusinessOperations.CompleteQueueService;
using FOX.BusinessOperations.UnAssignedQueueService;
using FOX.BusinessOperations.AccountService;
using FOX.BusinessOperations.GeneralNotesService;
using FOX.BusinessOperations.TaskServices;
using FOX.BusinessOperations.CaseServices;
using System.Web.Http.Cors;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using FoxRehabilitationAPI.Filters;

namespace FoxRehabilitationAPI
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.DependencyResolver = new UnityResolver(FoxRehabilitationAPI.App_Start.DIContainer.GetContainer());

            // Web API configuration and services
            // Configure Web API to use only bearer token authentication.
            config.SuppressDefaultHostAuthentication();
            config.Filters.Add(new HostAuthenticationFilter(OAuthDefaults.AuthenticationType));
            config.Filters.Add(new AuthorizationHandlerAttribute());

            string Origins = @"
                                http://172.16.0.207:15187,
                                https://ccremote.carecloud.com,
                                https://uatfox.mtbc.com,
                                https://stagingfox.mtbc.com,
                                https://stagingfox2.mtbc.com,
                                https://fox.mtbc.com
                             ";
            var corsAttr = new EnableCorsAttribute(Origins, "*", "*");
            //var corsAttr = new EnableCorsAttribute("*", "*", "*");
            config.EnableCors(corsAttr);
            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
            config.Routes.MapHttpRoute(
               name: "ActionBased",
               routeTemplate: "api/{controller}/{action}/{id}",
               defaults: new { id = RouteParameter.Optional }
           );
            config.MessageHandlers.Add(new WorkAroundForOperationCancelledException());
        }
    }

 
}
