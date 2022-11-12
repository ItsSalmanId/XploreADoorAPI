using FOX.DataModels.Models.CommonModel;
using FOX.DataModels.Models.IndexInfo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FOX.DataModels.Models.ExternalUserModel
{
    public class UserDetailsByNPIResponseModel : ResponseModel
    {
        public UserDetailByNPIModel userDetailByNPIModel { get; set; }
    }
    public class UserDetailByNPIModel
    {
        public int result_count { get; set; }
        public IList<Result> results { get; set; }
    }

    public class Taxonomy
    {
        public string state { get; set; }
        public string code { get; set; }
        public bool primary { get; set; }
        public string license { get; set; }
        public string desc { get; set; }
    }

    public class Address
    {
        public string city { get; set; }
        public string address_2 { get; set; }
        public string telephone_number { get; set; }
        public string fax_number { get; set; }
        public string state { get; set; }
        public string postal_code { get; set; }
        public string address_1 { get; set; }
        public string country_code { get; set; }
        public string country_name { get; set; }
        public string address_type { get; set; }
        public string address_purpose { get; set; }
    }

    public class Basic
    {
        public string status { get; set; }
        public string credential { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string middle_name { get; set; }
        public string name { get; set; }
        public string gender { get; set; }
        public string sole_proprietor { get; set; }
        public string last_updated { get; set; }
        public string enumeration_date { get; set; }
    }

    public class Result
    {
        public IList<Taxonomy> taxonomies { get; set; }
        public IList<Address> addresses { get; set; }
        public string created_epoch { get; set; }
        public IList<Identifier> identifiers { get; set; }
        public IList<OtherName> other_names { get; set; }
        public string number { get; set; }
        public string last_updated_epoch { get; set; }
        public Basic basic { get; set; }
        public string enumeration_type { get; set; }
    }
    public class Identifier
    {
        public string code { get; set; }
        public string issuer { get; set; }
        public string state { get; set; }
        public string identifier { get; set; }
        public string desc { get; set; }
    }

    public class OtherName
    {
        public string organization_name { get; set; }
        public string code { get; set; }
        public string type { get; set; }
    }


}