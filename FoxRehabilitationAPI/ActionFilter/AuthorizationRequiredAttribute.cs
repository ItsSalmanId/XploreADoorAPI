﻿using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using FOX.BusinessOperations.Security;
using System.Collections.Generic;
using System;
using FOX.BusinessOperations.CommonService;

namespace FoxRehabilitationAPI.ActionFilter
{
    public class AuthorizationRequiredAttribute : ActionFilterAttribute
    {
        private const string Token = "Token";

        public override void OnActionExecuting(HttpActionContext filterContext)
        {
            //try
            //{
                //  Get API key provider
                var provider = filterContext.ControllerContext.Configuration
                .DependencyResolver.GetService(typeof(ITokenService)) as ITokenService;

                if (filterContext.Request.Headers.Contains(Token))
                {
                    var tokenValue = filterContext.Request.Headers.GetValues(Token).First();
                    //filterContext.RouteData.Values.Add("test", "TESTING");
                    filterContext.Request.Properties.Add(new KeyValuePair<string, object>("Token", tokenValue));
                    if (provider != null && !provider.ValidateToken(tokenValue, "41"))
                    {
                        var responseMessage = new HttpResponseMessage(HttpStatusCode.Unauthorized) { ReasonPhrase = "Invalid Request" };
                        filterContext.Response = responseMessage;
                    }
                }
                else
                {
                    filterContext.Response = new HttpResponseMessage(HttpStatusCode.Unauthorized);
                }

                base.OnActionExecuting(filterContext);
            //}
            //catch(Exception ex)
            //{
            //    Helper.LogException(ex);
            //}
        }
    }
}