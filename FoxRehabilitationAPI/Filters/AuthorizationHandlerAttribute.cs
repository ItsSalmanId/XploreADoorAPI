using FOX.BusinessOperations.CommonService;
using FOX.BusinessOperations.Security;
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
                    var accessToken = new SqlParameter("TOKEN", SqlDbType.VarChar) { Value = accessedTokenFromRequst ?? "0" };
                    var ExpiredToken = SpRepository<ProfileToken>.GetSingleObjectWithStoreProcedure(@"EXEC FOX_PROC_CHECK_EXPIRED_TOKEN  @TOKEN", accessToken);
                    if (HttpContext.Current.User != null && HttpContext.Current.User.Identity != null)
                    {
                        profile = ClaimsModel.GetUserProfile(HttpContext.Current.User.Identity as System.Security.Claims.ClaimsIdentity) ?? new UserProfile();
                    }
                    if (ExpiredToken == null)
                    {
                        if (profile?.UserName == "6455testing" || profile?.UserName == "1163TESTING")
                        {
                            Helper.TokenTaskCancellationExceptionLog("ExpiredToken null for User: " + profile?.UserName + " and Token: " + accessedTokenFromRequst, profile?.isTalkRehab == true ? "CCR" : "Fox");
                        }
                        base.HandleUnauthorizedRequest(actionContext);
                    }
                    else if (ExpiredToken.isLogOut == true)
                    {
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