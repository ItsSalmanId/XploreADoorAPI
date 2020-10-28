using FOX.DataModels.Models.CommonModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FOX.DataModels.Models.ExternalUserModel
{
    public class CityDetailByZipCodeResponseModel : ResponseModel
    {
        public List<Zip_City_State> zip_city_state { get; set; }
    }
}