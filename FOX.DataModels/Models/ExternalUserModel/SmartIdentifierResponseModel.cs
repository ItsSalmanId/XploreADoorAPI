using FOX.DataModels.Models.CasesModel;
using FOX.DataModels.Models.CommonModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FOX.DataModels.Models.ExternalUserModel
{
    public class SmartIdentifierResponseModel : ResponseModel
    {
        public List<FOX_TBL_IDENTIFIER> fox_tbl_identifier { get; set; }
    }
}