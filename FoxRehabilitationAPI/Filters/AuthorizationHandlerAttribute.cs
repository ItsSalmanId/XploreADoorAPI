using FOX.BusinessOperations.CommonService;
using FOX.BusinessOperations.Security;
using FOX.DataModels;
using FOX.DataModels.GenericRepository;
using FOX.DataModels.Models.Security;
using FoxRehabilitationAPI.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using static FOX.DataModels.Models.Security.ProfileToken;

namespace FoxRehabilitationAPI.Filters
{
    public class AuthorizationHandlerAttribute : AuthorizeAttribute
    {
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            if (actionContext.Request.Headers.Authorization != null)
            {
                var accessedTokenFromRequst = actionContext?.Request?.Headers?.Authorization?.Parameter;
                if (accessedTokenFromRequst != null && accessedTokenFromRequst != "undefined" && accessedTokenFromRequst != "null")
                {
                    UserProfile profile = new UserProfile();
                    if (HttpContext.Current.User != null && HttpContext.Current.User.Identity != null)
                    {
                        profile = ClaimsModel.GetUserProfile(HttpContext.Current.User.Identity as System.Security.Claims.ClaimsIdentity) ?? new UserProfile();
                        EntityHelper.isTalkRehab = !string.IsNullOrEmpty(profile?.isTalkRehab.ToString()) && profile?.isTalkRehab.ToString().ToLower() == "true" ? true : false;
                    }
                    var accessToken = new SqlParameter("TOKEN", SqlDbType.VarChar) { Value = accessedTokenFromRequst ?? "0" };
                    var ExpiredToken = SpRepository<ProfileToken>.GetSingleObjectWithStoreProcedure(@"EXEC FOX_PROC_CHECK_EXPIRED_TOKEN  @TOKEN", accessToken);
                    if (ExpiredToken == null)
                    {
                        if (profile?.UserName == "6455testing" || profile?.UserName == "1163TESTING")
                        {
                            Helper.TokenTaskCancellationExceptionLog("ExpiredToken null for User: " + profile?.UserName + " and Token: " + accessedTokenFromRequst, profile?.isTalkRehab == true ? "CCR" : "Fox");

                        }
                        base.HandleUnauthorizedRequest(actionContext);
                    }
                    if (ExpiredToken.isMFAVerified == 0 && profile.MFA == true && profile.showMfaEanbleScreen == 1 && !actionContext.Request.RequestUri.OriginalString.Contains("Singout") && (actionContext.Request.RequestUri.OriginalString.Contains("GetOtp")))
                    {
                        ExpiredToken.isLogOut = false;
                        base.OnAuthorization(actionContext);

                    }
                    else if (ExpiredToken.isMFAVerified == 0 && profile.MFA == true && profile.showMfaEanbleScreen == 1 && actionContext.Request.RequestUri.OriginalString.Contains("UpdateOtpEnableDate"))
                    {
                        TokenService tokenUpdate = new TokenService();
                        bool isSecondCall = true;
                        tokenUpdate.UpdateToken(profile.UserName, ExpiredToken.AuthToken, isSecondCall);

                    }
                    else if (ExpiredToken.isMFAVerified == 0 && profile.MFA == true && profile.showMfaEanbleScreen == 1 && actionContext.Request.RequestUri.OriginalString.Contains("VerifyOTP"))
                    {
                        TokenService tokenUpdate = new TokenService();
                        bool isSecondCall = true;
                        tokenUpdate.UpdateToken(profile.UserName, ExpiredToken.AuthToken, isSecondCall);
                        }
                        base.HandleUnauthorizedRequest(actionContext);
                    }
                    else if (ExpiredToken.isLogOut == true)

                    {
                        if (actionContext.Request.RequestUri.OriginalString.Contains("Singout"))

                        {
                            base.HandleUnauthorizedRequest(actionContext);
                        }


                        if (profile?.UserName == "6455testing" || profile?.UserName == "1163TESTING")
                        {
                            Helper.TokenTaskCancellationExceptionLog("ExpiredToken null for User: " + profile?.UserName, profile?.isTalkRehab == true ? "CCR" : "Fox");
                        }
                        base.HandleUnauthorizedRequest(actionContext);
                    }
                    else if (Convert.ToInt64(ExpiredToken.UserId) != profile?.userID)
                    {
                        if (profile?.UserName == "6455testing" || profile?.UserName == "1163TESTING")
                        {
                            Helper.TokenTaskCancellationExceptionLog("UserId not equal ExpiredToken.UserId: " + ExpiredToken.UserId + " And profile.userID: " + profile?.userID, profile?.isTalkRehab == true ? "CCR" : "Fox");
                        }
                        base.HandleUnauthorizedRequest(actionContext);
                    }
                    else
                    {
                        base.OnAuthorization(actionContext);
                    }
                }
            }
        }
    }
}