using FOX.BusinessOperations.CommonService;
using FOX.BusinessOperations.Security;
using FOX.DataModels.Models.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;

namespace FoxRehabilitationAPI.ControllerExtension
{
    public abstract class APIExtensionController : ApiController
    {
        [NonAction]
        protected UserProfile GetProfile()
        {
            //try
            //{
                object Token = string.Empty;
                //  Request.Properties.TryGetValue("Token", out Token);
                HttpHeaders c = Request.Headers;
                Token = c.GetValues("Token").FirstOrDefault();
                //get profile from business operations
                var provider = Configuration.DependencyResolver.GetService(typeof(ITokenService)) as ITokenService;
                return provider.GetProfile(Token.ToString()).userProfile;
            //}
            //catch(Exception ex)
            //{
            //    Helper.LogException(ex);
            //}
            //return null;
        }

    }
}
