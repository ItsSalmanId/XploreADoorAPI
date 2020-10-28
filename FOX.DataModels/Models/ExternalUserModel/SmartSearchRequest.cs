using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FOX.DataModels.Models.ExternalUserModel
{
    public class SmartSearchRequest : BaseModel
    {
        public string Keyword { get; set; }
        public long PRACTICE_CODE { get; set; }
        public string TYPE { get; set; }
        public string SEARCHVALUE { get; set; }
        public long PracticeCode { get; set; }
    }
}