using FOX.BusinessOperations.Security;
using FOX.DataModels.GenericRepository;
using FOX.DataModels.Models.Security;
using FoxRehabilitationAPI.Models;
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
                if (accessedTokenFromRequst != null && accessedTokenFromRequst != "undefined" && accessedTokenFromRequst!="null")
                {
                    var accessToken = new SqlParameter("TOKEN", SqlDbType.VarChar) { Value = accessedTokenFromRequst ?? "0" };
                    var ExpiredToken = SpRepository<TokensUserInfo>.GetSingleObjectWithStoreProcedure(@"EXEC FOX_PROC_CHECK_EXPIRED_TOKEN  @TOKEN", accessToken);
                    if (HttpContext.Current.User != null && HttpContext.Current.User.Identity != null)
                    {
                        UserProfile profile = ClaimsModel.GetUserProfile(HttpContext.Current.User.Identity as System.Security.Claims.ClaimsIdentity) ?? new UserProfile();
                        if (Convert.ToInt64(ExpiredToken.UserId) != profile.userID)
                        {
                            base.HandleUnauthorizedRequest(actionContext);
                        }
                    }
                    if (ExpiredToken.TokenSecurityID != null && ExpiredToken.TokenSecurityID != "0")
                    {
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