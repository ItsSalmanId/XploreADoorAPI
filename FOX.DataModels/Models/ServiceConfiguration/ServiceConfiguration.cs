using System.ComponentModel.DataAnnotations.Schema;

namespace FOX.DataModels.Models.ServiceConfiguration
{
    [Table("FOX_TBL_FAX_SERVICE_CONFIG")]
    public class ServiceConfiguration
    {
        public long? PRACTICE_CODE { get; set; }
        public string ENVIRONMENT_NAME { get; set; }
        public string EMERGENCY_EMAIL { get; set; }
        public string FAX_USERNAME { get; set; }
        public string EMAIL_PASSWORD { get; set; }
        public string EMAIL_USERNAME { get; set; }
        public string IMAGES_PATH_DB { get; set; }
        public string ORIGINAL_FILES_PATH_DB { get; set; }
        public string IMAGES_PATH_SERVER { get; set; }
        public string ORIGINAL_FILES_PATH_SERVER { get; set; }
        public bool LOAD_FAXES { get; set; }
        public bool LOAD_EMAILS { get; set; }
    }
}