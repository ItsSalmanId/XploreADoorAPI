using FOX.BusinessOperations.Security;
using FOX.DataModels.Models.Security;
using System;
using System.Threading;
using System.Web.Http.Controllers;

namespace FoxRehabilitationAPI.Filters
{
    /// <summary>
    /// Custom Authentication Filter Extending basic Authentication
    /// </summary>
    public class APIAuthenticationFilter : GenericAuthenticationFilter
    {
        /// <summary>
        /// Default Authentication Constructor
        /// </summary>
        public APIAuthenticationFilter()
        {
        }

        /// <summary>
        /// AuthenticationFilter constructor with isActive parameter
        /// </summary>
        /// <param name="isActive"></param>
        public APIAuthenticationFilter(bool isActive)
            : base(isActive)
        {
        }

        /// <summary>
        /// Protected overriden method for authorizing user
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="actionContext"></param>
        /// <returns></returns>
        protected override bool OnAuthorizeUser(string username, string password, HttpActionContext actionContext)
        {
            try
            {
                var provider = actionContext.ControllerContext.Configuration
                                   .DependencyResolver.GetService(typeof(IUserServices)) as IUserServices;
                if (provider != null)
                {
                    var basicAuthenticationIdentity = Thread.CurrentPrincipal.Identity as BasicAuthenticationIdentity;
                    string userName = provider.Authenticate(username, password);
                    if (!string.IsNullOrEmpty(userName) && userName != "400" && userName != "201" && userName != "202" && userName != "203" && userName != "204")
                    {

                        if (basicAuthenticationIdentity != null)
                            basicAuthenticationIdentity.UserName = userName;

                        return true;
                    }
                }
                return false;
            }
            catch(Exception e) { return true; }
        }
    }
}