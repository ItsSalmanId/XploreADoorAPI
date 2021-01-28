using FOX.DataModels.GenericRepository;
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
                if (accessedTokenFromRequst != null)
                {
                    var accessToken = new SqlParameter("TOKEN", SqlDbType.VarChar) { Value = accessedTokenFromRequst ?? "0" };
                    var ExpiredToken = SpRepository<ProfileTokensSecurity>.GetSingleObjectWithStoreProcedure(@"EXEC FOX_PROC_CHECK_EXPIRED_TOKEN  @TOKEN", accessToken);
                    if (ExpiredToken != null)
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