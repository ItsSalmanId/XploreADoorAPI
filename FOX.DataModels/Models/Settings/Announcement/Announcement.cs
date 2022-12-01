using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FOX.DataModels.Models.Settings.Announcement
{
    [Table("FOX_TBL_ANNOUNCEMENT")]
    public class Announcements
    {
        [Key]
        public long ANNOUNCEMENT_ID { get; set; }
        public string ANNOUNCEMENT_TITLE { get; set; }
        public DateTime? ANNOUNCEMENT_DATE_FROM { get; set; }
        [NotMapped]
        public string ANNOUNCEMENT_DATE_FROM_STR { get; set; }
        public DateTime? ANNOUNCEMENT_DATE_TO { get; set; }
        [NotMapped]
        public string ANNOUNCEMENT_DATE_TO_STR { get; set; }
        public string ANNOUNCEMENT_DETAILS { get; set; }
        [NotMapped]
        public List<AnnouncementRoles> AnnouncementRoles { get; set; }
        [NotMapped]
        public List<FoxRoleRequest> RoleRequest { get; set; }
        [NotMapped]
        public string EditSelectedRolesID { get; set; }
        public long PRACTICE_CODE { get; set; }
        public string CREATED_BY { get; set; }
        public DateTime CREATED_DATE { get; set; }
        public string MODIFIED_BY { get; set; }
        [NotMapped]
        public string ROLE_ID { get; set; }
        [NotMapped]
        public string ROLE_NAME { get; set; }
        public DateTime MODIFIED_DATE { get; set; }
        public bool DELETED { get; set; }
    }
    [Table("FOX_TBL_ANNOUNCEMENT_ROLE")]
    public class AnnouncementRoles
    {
        [Key]
        public long ANNOUNCEMENT_ROLE_ID { get; set; }
        public long ROLE_ID { get; set; }
        public string ROLE_NAME { get; set; }
        public long ANNOUNCEMENT_ID { get; set; }
        public long PRACTICE_CODE { get; set; }
        public string CREATED_BY { get; set; }
        public DateTime CREATED_DATE { get; set; }
        public string MODIFIED_BY { get; set; }
        public DateTime MODIFIED_DATE { get; set; }
        public bool DELETED { get; set; }
    }
    [Table("FOX_TBL_ROLE")]
    public class FoxRoles
    {
        [Key]
        public long ROLE_ID { get; set; }
        public string ROLE_NAME { get; set; }
        public long? PRACTICE_CODE { get; set; }
        public string CREATED_BY { get; set; }
        public DateTime CREATED_DATE { get; set; }
        public string MODIFIED_BY { get; set; }
        public DateTime MODIFIED_DATE { get; set; }
        public bool DELETED { get; set; }
    }

    public class FoxRoleRequest
    {
        public long ROLE_ID { get; set; }
        public string ROLE_NAME { get; set; }
    }

    public  class AddEditFoxAnnouncement
    {
        public long ANNOUNCEMENT_ID { get; set; }
        public DateTime? ANNOUNCEMENT_DATE_FROM { get; set; }
        public string ANNOUNCEMENT_DATE_FROM_STR { get; set; }
        public DateTime? ANNOUNCEMENT_DATE_TO { get; set; }
        public string ANNOUNCEMENT_DATE_TO_STR { get; set; }
        public string ANNOUNCEMENT_TITLE { get; set; }
        public string ANNOUNCEMENT_DETAILS { get; set; }
        public List<FoxRoleRequest> RoleRequest { get; set; }
        public long PRACTICE_CODE { get; set; }
        public string ROLE_ID { get; set; }
        public string EditSelectedRolesID { get; set; }
        public List<AnnouncementRoles> AnnouncementRoles { get; set; }
    }
}
