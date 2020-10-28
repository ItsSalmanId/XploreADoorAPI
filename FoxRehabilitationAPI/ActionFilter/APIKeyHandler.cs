

using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace FoxRehabilitationAPI.ActionFilter
{
    public class APIKeyHandler: AuthorizeAttribute
    {
        private const string ApiKey = "CASH-224-EHR-446-5002";

        protected override bool IsAuthorized(HttpActionContext context)
        {
            bool isValidAPIKey = false;
            IEnumerable<string> lsHeaders;
            var checkApiKeyExists = context.Request.Headers.TryGetValues("KEY", out lsHeaders);
            if (checkApiKeyExists)
            {
                if (lsHeaders.FirstOrDefault().Equals(ApiKey))
                {
                    isValidAPIKey = true;
                }
            }
            return isValidAPIKey;

        }
    }
}