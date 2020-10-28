using FOX.DataModels.Models.CommonModel;
using FOX.DataModels.Models.Settings.Practice;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FOX.DataModels.Models.ExternalUserModel
{
    public class FoxTBLPracticeOrganizationResponseModel : ResponseModel
    {
        public List<PracticeOrganization> fox_tbl_practice_organization { get; set; }
    }
}