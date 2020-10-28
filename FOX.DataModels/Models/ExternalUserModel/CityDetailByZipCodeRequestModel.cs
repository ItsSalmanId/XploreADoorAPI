using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FOX.DataModels.Models.ExternalUserModel
{
    public class CityDetailByZipCodeRequestModel
    {
        public string ZipCode { get; set; }
    }

    public class LogoutModal
    {
        public string token { get; set; }
    }
}