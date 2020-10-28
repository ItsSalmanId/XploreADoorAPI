using FOX.DataModels.Models.IndexInfo;
using FOX.DataModels.Models.Settings.FacilityLocation;
using FOX.DataModels.Models.Settings.ReferralSource;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FOX.DataModels.Models.RequestForOrder.IndexInformation
{
    public class FacilityReferralSource
    {
        public FacilityLocation FACILITY_LOCATION { get; set; } = new FacilityLocation();
        public ReferralSource REFERRAL_SOURCE { get; set; } = new ReferralSource();
    }
}