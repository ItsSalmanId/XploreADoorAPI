using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace FOX.DataModels.Models.Security
{
    [Serializable]
    [Table("FOX_TBL_PROFILE_TOKENS")]
    public class ProfileToken //: Alachisoft.NCache.Runtime.Serialization.ICompactSerializable
    {
        [Key]
        public long TokenId { get; set; }
        public long UserId { get; set; }
        public string AuthToken { get; set; }
        public System.DateTime IssuedOn { get; set; }
        public System.DateTime ExpiresOn { get; set; }
        public string Profile { get; set; }

        [NotMapped]
        public UserProfile userProfile
        {
            get
            {
                UserProfile UF = new UserProfile();
                UF = JsonConvert.DeserializeObject<UserProfile>(this.Profile);
                return UF;
            }
            set { this.Profile = JsonConvert.SerializeObject(value); }
        }
        [NotMapped]
        public int AUTO_LOCK_TIMESPAN { get; set; }

        [Table("FOX_TBL_PROFILE_TOKENS_SECURITY")]
        public class ProfileTokensSecurity //: Alachisoft.NCache.Runtime.Serialization.ICompactSerializable
        {
            [Key]
            public long TokenSecurityID { get; set; }
            public bool isLogOut { get; set; }
            public string AuthToken { get; set; }
            public System.DateTime IssuedOn { get; set; }
            public System.DateTime ExpiresOn { get; set; }
        }
    }
}