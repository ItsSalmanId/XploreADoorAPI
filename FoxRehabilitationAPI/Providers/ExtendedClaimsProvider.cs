using FoxRehabilitationAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;

namespace FoxRehabilitationAPI.Providers
{
    public class ExtendedClaimsProvider
    {
        public static IEnumerable<Claim> GetClaims(ApplicationUser user)
        {

            List<Claim> claims = new List<Claim>();
            //claims.Add(CreateClaim("FirstName", user.FIRST_NAME));
            //var daysInWork = (DateTime.Now.Date - user.JoinDate).TotalDays;

            //if (daysInWork > 90)
            //{
            //    claims.Add(CreateClaim("FTE", "1"));

            //}
            //else
            //{
            //    claims.Add(CreateClaim("FTE", "0"));
            //}

            return claims;
        }

        public static Claim CreateClaim(string type, string value)
        {
            return new Claim(type, value, ClaimValueTypes.String);
        }

    }
}