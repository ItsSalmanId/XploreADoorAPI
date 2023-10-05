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
using FOX.DataModels;
using System.Globalization;

namespace FoxRehabilitationAPI.Providers
{
    public class ApplicationOAuthProvider : OAuthAuthorizationServerProvider
    {
        private readonly string _publicClientId;
        int invalidAttempts = 0;
        private string isTalkRehab;

        public ApplicationOAuthProvider(string publicClientId)
        {
            if (publicClientId == null)
            {
                throw new ArgumentNullException("publicClientId");
            }
            _publicClientId = publicClientId;
            isTalkRehab = string.Empty;
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
                isTalkRehab = form["isTalkRehab"];

                EntityHelper.isTalkRehab = (!string.IsNullOrWhiteSpace(isTalkRehab) && isTalkRehab.ToLower() == "true") ? true:false;
                //var cookie = context.Request.Cookies["count"].FirstOrDefault();
                //if (cookie != null)
                //{
                //    return cookie[]
                //}
                //System.Net.Http.Headers.CookieHeaderValue cookie = System.Net.Http.
                Tuple<ApplicationUser, UserProfile> tuple = null;
                var userManager = context.OwinContext.GetUserManager<ApplicationUserManager>();
                userManager = context.OwinContext.GetUserManager<ApplicationUserManager>();
                invalidAttempts = userManager.GetInvalidAttempts(context.UserName);
                if (invalidAttempts >= 5)
                {
                    if (!userManager.IsCheckedUserBlocked(context.UserName))
                    {
                        var response = await ValidateCaptcha(form["encryptedCode"]);
                        if (!response.Success)
                        {
                            context.SetError("Bad request", invalidAttempts + 1 + "Captcha verification is required.");
                            return;
                        }
                    }
                    else
                    {
                        context.SetError("invalid_grant", invalidAttempts + 1 + "Your account has been temporarily suspended. Please contact system administrator.");
                        return;
                    }
                }
                var invalidMFAAttempts = userManager.GetInvalidMFAAttempts(context.UserName);
                if (invalidMFAAttempts != null)
                {
                    DateTime currentDateTime = DateTime.UtcNow;
                    DateTime modifiedDateTime = invalidMFAAttempts.LAST_ATTEMPT_DATE_UTC;
                    TimeSpan timeDifference = modifiedDateTime - currentDateTime;
                    int differenceInMinutes = (int)timeDifference.TotalMinutes;
                    if (modifiedDateTime.Date < currentDateTime.Date || differenceInMinutes < 0)
                    {
                        userManager.AddMFAValidLoginAttempt(context.UserName);
                    }
                }
                var invalidMFAAttempt = userManager.GetInvalidMFAAttempts(context.UserName);
                if (invalidMFAAttempt.FAIL_ATTEMPT_COUNT >= 5)
                {
                    context.SetError("invalid_grant", invalidAttempts + 1 + "Your Account has been locked by Invalid MFA attempts, please try again in," + invalidMFAAttempts.LAST_ATTEMPT_DATE_UTC);
                    return;
                }
                tuple = await userManager.FindProfileAsync(context.UserName, context.Password);
                if (tuple.Item1 == null || tuple.Item2 == null)
                {
                    if (!userManager.CheckUserExistingLoginAttempts(context.UserName))
                    {
                        context.SetError("invalid_grant", invalidAttempts + 1 + "Your account has been temporarily suspended. Please contact system administrator.");
                        userManager.AddInvalidLoginAttempt(context.UserName);
                        return;
                    }
                    userManager.AddInvalidLoginAttempt(context.UserName);
                    if (context.UserName.Contains("@"))
                    {
                        ADDetail _ADDetail = null;
                        _ADDetail = ADDetailList.FirstOrDefault(t => t.DomainForSearch.Equals(context.UserName.Split('@')[1].ToLower()));
                        if (_ADDetail  != null)
                        {
                            context.SetError("invalid_grant", invalidAttempts + 1 + "We are unable to sign into your account, please contact your network administrator at FOX Rehab at  ");
                            invalidAttempts++;
                            return;
                        }
                    }
                    context.SetError("invalid_grant", invalidAttempts + 1 + "The user name or password is incorrect.");
                    invalidAttempts++;
                    return;
                }
                else
                {
                    if (userManager.IsCheckedUserBlocked(context.UserName))
                    {
                        context.SetError("invalid_grant", invalidAttempts + 1 + "Your account has been temporarily suspended. Please contact system administrator.");
                        return;
                    }
                    userManager.AddValidLoginAttempt(context.UserName);
                    invalidAttempts = 0;
                    userManager.AddMFAValidLoginAttempt(context.UserName);
                }
                tuple.Item1.UserName = tuple.Item1.USER_NAME = tuple.Item2.UserName;
                ClaimsIdentity oAuthIdentity;
                try
                {
                    if (!string.IsNullOrWhiteSpace(isTalkRehab) && isTalkRehab.ToLower() == "true")
                    {
                        tuple.Item2.isTalkRehab = true;
                    }
                    else
                    {
                        tuple.Item2.isTalkRehab = false;
                    }
                    oAuthIdentity = await tuple.Item1.GenerateUserIdentityAsync(userManager, OAuthDefaults.AuthenticationType, tuple.Item2);

                }
                catch (Exception)
                {
                    throw;
                }

                ClaimsIdentity cookiesIdentity = await tuple.Item1.GenerateUserIdentityAsync(userManager, CookieAuthenticationDefaults.AuthenticationType, tuple.Item2);
                AuthenticationProperties properties = CreateProperties(tuple.Item2);
                AuthenticationTicket ticket = new AuthenticationTicket(oAuthIdentity, properties);
                context.Validated(ticket);
                context.Request.Context.Authentication.SignIn(cookiesIdentity);
            }
            catch (Exception)
            {

            }
        }

        private async Task<GoogleRecaptchaResponse> ValidateCaptcha(string encodedCode)
        {

            string url = $"https://www.google.com/recaptcha/api/siteverify?secret={AppConfiguration.SecretKey}&response={encodedCode}";
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
            catch (Exception)
            {
                return Task.FromResult<object>(null);
            }
        }

        public override Task TokenEndpointResponse(OAuthTokenEndpointResponseContext context)
        {
            ITokenService tokenService = new TokenService();
            var userProfile = ClaimsModel.GetUserProfile(context.Identity as System.Security.Claims.ClaimsIdentity);
            var result = tokenService.SaveTokenWithProfile(userProfile, context.AccessToken);
            string encryptedToken = Encrypt.EncryptionForClient(context.AccessToken);
            context.AdditionalResponseParameters.Add("EncryptedAccessToken", encryptedToken);
            return base.TokenEndpointResponse(context);
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
            catch (Exception)
            {
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
