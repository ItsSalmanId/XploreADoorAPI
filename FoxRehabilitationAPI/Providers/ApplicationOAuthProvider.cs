using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;
using FoxRehabilitationAPI.Models;
using FOX.DataModels.Models.Security;
using FOX.BusinessOperations.Security;
using System.Text;
using System.Net.Http;
using Newtonsoft.Json;
using FOX.BusinessOperations.CommonServices;
using FOX.DataModels.Models.GoogleRecaptcha;
using System.Net;
using static FOX.BusinessOperations.CommonServices.AppConfiguration.ActiveDirectoryViewModel;
using FOX.BusinessOperations.CommonService;
using FoxRehabilitationAPI.Filters;

namespace FoxRehabilitationAPI.Providers
{
    public class ApplicationOAuthProvider : OAuthAuthorizationServerProvider
    {
        private readonly string _publicClientId;
        int invalidAttempts = 0;

        public ApplicationOAuthProvider(string publicClientId)
        {
            if (publicClientId == null)
            {
                throw new ArgumentNullException("publicClientId");
            }
            _publicClientId = publicClientId;
        }

        public static string GetCookie(HttpRequestMessage request, string cookieName)
        {
            System.Net.Http.Headers.CookieHeaderValue cookie = request.Headers.GetCookies(cookieName).FirstOrDefault();
            if (cookie != null)
                return cookie[cookieName].Value;

            return null;
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            try
            {
            //string hostname = FOX.BusinessOperations.CommonService.Helper.GetHostName();
            //string h = context.OwinContext.Request.Host.Value;
            context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { "*" });
            var form = await context.Request.ReadFormAsync();

                //var cookie = context.Request.Cookies["count"].FirstOrDefault();
                //if (cookie != null)
                //{
                //    return cookie[]
                //}
                //System.Net.Http.Headers.CookieHeaderValue cookie = System.Net.Http.

                if (invalidAttempts >= 5)
                {
                    var response = await ValidateCaptcha(form["encryptedCode"]);
                    if (!response.Success)
                    {
                        context.SetError("Bad request", "Captcha verification is required.");
                        return;
                    }
                }

                Tuple<ApplicationUser, UserProfile> tuple = null;
            var userManager = context.OwinContext.GetUserManager<ApplicationUserManager>();
            userManager = context.OwinContext.GetUserManager<ApplicationUserManager>();

            if (!userManager.CheckUserExistingLoginAttempts(context.UserName))
            {
                context.SetError("invalid_grant", "Your account has been temporarily suspended. Please contact system administrator.");
                return;
            }

            tuple = await userManager.FindProfileAsync(context.UserName, context.Password);

            if (tuple.Item1 == null || tuple.Item2 == null)
            {
                userManager.AddInvalidLoginAttempt(context.UserName);
                if (context.UserName.Contains("@"))
                {
                    ADDetail _ADDetail = null;
                    _ADDetail = ADDetailList.FirstOrDefault(t => t.DomainForSearch.Equals(context.UserName.Split('@')[1].ToLower()));
                    if (_ADDetail  != null)
                    {
                        context.SetError("invalid_grant", "We are unable to sign into your account, please contact your network administrator at FOX Rehab at  ");
                        invalidAttempts++;
                        return;
                    }
                }
                context.SetError("invalid_grant", "The user name or password is incorrect.");
                invalidAttempts++;
                return;
            }
            else
            {
                userManager.AddValidLoginAttempt(context.UserName);
                invalidAttempts = 0;
            }
            tuple.Item1.UserName = tuple.Item1.USER_NAME = tuple.Item2.UserName;
            ClaimsIdentity oAuthIdentity;
            try
            {
                oAuthIdentity = await tuple.Item1.GenerateUserIdentityAsync(userManager, OAuthDefaults.AuthenticationType, tuple.Item2);

            }
            catch (Exception ex)
            {
                throw;
            }

            ClaimsIdentity cookiesIdentity = await tuple.Item1.GenerateUserIdentityAsync(userManager, CookieAuthenticationDefaults.AuthenticationType);
            AuthenticationProperties properties = CreateProperties(tuple.Item2);
            AuthenticationTicket ticket = new AuthenticationTicket(oAuthIdentity, properties);
            context.Validated(ticket);
            context.Request.Context.Authentication.SignIn(cookiesIdentity);
        }
        catch (Exception ex)
        {
                Helper.CustomExceptionLog(ex);
        }
    }

        private async Task<GoogleRecaptchaResponse> ValidateCaptcha(string encodedCode)
        {
            string code = "03AGdBq27ecw_BPYm99zZCfLD9MwVmjf3D4wN_UMbRBUkqJbtm-RhULsw4TEXps8_EqWTl9Or3UPsAvbaXBAuwoOdEbgULmZOxMlb0ODd7oF112JZPHXxIDbG2QZhPHCc2SoKn5oxO-bpnc9NvjEMlmE1rqF2LN3lUhjsMC8h8r3gJQOdS_NIko3E4xJatTkRkV99zYsxyuPyhGPGORthvYHMPrWeasS-xGLHXp5bgPyQUzOSMSYIjmUpMKlnEsIgQLKyRUh-j1JC2RxIw0FTGN89KPJe51x0XYR6D5HhvVV9MtbblUpbw3WoMPyuRozjoQhyPsvzG7r0JzDlezt66SPWjeWwROyoQuwMFlVTPA3QyZVKMhKqCet0lZe-BJTRWzQO0xKpUFiLbFMmSU-ZWiZ4m37Z1PIA_qSc2lr_1ADa_jXCbE9JZGCcAvf5V_zpEpJZzaDIA6cjlogr3vSfiJ28HRI4xyfaBQg";
            string url = $"https://www.google.com/recaptcha/api/siteverify?secret={AppConfiguration.SecretKey}&response={code}";
            using (var client = new HttpClient())
            {
                try
                {
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                    return JsonConvert.DeserializeObject<GoogleRecaptchaResponse>(await client.GetStringAsync(url));
                }
                catch (Exception ex)
                {
                    //throw ex;
                    return new GoogleRecaptchaResponse()
                    {
                        Success = false,
                        ErrorCodes = new string[] { ex.ToString() },
                    };
                }
            }

        }
        public override Task TokenEndpoint(OAuthTokenEndpointContext context)
        {
            try
            {
                foreach (KeyValuePair<string, string> property in context.Properties.Dictionary)
                {
                    context.AdditionalResponseParameters.Add(property.Key, property.Value);
                }

                return Task.FromResult<object>(null);
            }
            catch (Exception ex)
            {
                Helper.CustomExceptionLog(ex);
                return Task.FromResult<object>(null);
            }
        }

        public override Task TokenEndpointResponse(OAuthTokenEndpointResponseContext context)
        {
            try
            {
                ITokenService tokenService = new TokenService();
                var userProfile = ClaimsModel.GetUserProfile(context.Identity as System.Security.Claims.ClaimsIdentity);
                var result = tokenService.SaveTokenWithProfile(userProfile, context.AccessToken);
                return base.TokenEndpointResponse(context);
            }
            catch (Exception ex)
            {
                Helper.CustomExceptionLog(ex);
                return base.TokenEndpointResponse(context);
            }
        }

        public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            try
            {
                // Resource owner password credentials does not provide a client ID.
                
                if (context.ClientId == null)
                {
                    context.Validated();
                }

                return Task.FromResult<object>(null);
            }
            catch (Exception ex)
            {
                Helper.CustomExceptionLog(ex);
                return Task.FromResult<object>(null);
            }
        }

        public override Task ValidateClientRedirectUri(OAuthValidateClientRedirectUriContext context)
        {
            if (context.ClientId == _publicClientId)
            {
                Uri expectedRootUri = new Uri(context.Request.Uri, "/");

                if (expectedRootUri.AbsoluteUri == context.RedirectUri)
                {
                    context.Validated();
                }
            }

            return Task.FromResult<object>(null);
        }

        public static AuthenticationProperties CreateProperties(UserProfile user)
        {
            IDictionary<string, string> data = new Dictionary<string, string>
            {
                { "Profile", Newtonsoft.Json.JsonConvert.SerializeObject(user) }

            };
            return new AuthenticationProperties(data);
        }
        public static AuthenticationProperties CreateProperties(string userName)
        {
            IDictionary<string, string> data = new Dictionary<string, string>
            {
                { "userName", userName }
            };
            return new AuthenticationProperties(data);
        }
    }
}
