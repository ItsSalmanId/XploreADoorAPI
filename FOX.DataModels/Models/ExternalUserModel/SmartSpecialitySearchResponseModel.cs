using FOX.DataModels.Models.CommonModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FOX.DataModels.Models.ExternalUserModel
{
    public class SmartSpecialitySearchResponseModel:ResponseModel
    {
        public List<Speciality> specialities { get; set; }
    }
}