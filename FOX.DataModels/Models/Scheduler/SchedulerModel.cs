using FOX.DataModels.Models.GeneralNotesModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FOX.DataModels.Models.Scheduler
{
    public class SchedulerModel
    {
        [Table("FOX_TBL_APPOINTMENT")]

        public class Appointment : BaseModel
        {
            [Key]
            public long APPOINTMENT_ID { get; set; }
            public DateTime? APPOINTMENT_DATE { get; set; }
            [NotMapped]
            public int? ROW { get; set; }
            [NotMapped]
            public string APPOINTMENT_DATE_STR { get; set; }
            public string TIME_FROM { get; set; }
            [NotMapped]
            public DateTime TIME_FROM_DATE { get; set; }
            [NotMapped]
            public DateTime TIME_TO_DATE { get; set; }
            [NotMapped]
            public int? LENGTH { get; set; }
            [NotMapped]
            public string STR_LENGTH { get; set; }
            [NotMapped]
            public string MRN { get; set; }
            public long? PATIENT_ACCOUNT { get; set; }
            [NotMapped]
            public string NAME { get; set; }
            [NotMapped]
            public string HOME_PHONE { get; set; }
            [NotMapped]
            public long? ADDRESS_ID { get; set; }
            [NotMapped]
            public string ADDRESS { get; set; }
            [NotMapped]
            public string REGION_NAME { get; set; }
            [NotMapped]
            public int? FC_ID { get; set; }
            [NotMapped]
            public string FC_CODE { get; set; }
            [NotMapped]
            public string FC_DESCRIPTION { get; set; }
            [NotMapped]
            public string RT_CASE_NO { get; set; }
            [NotMapped]
            public long? REASON_ID { get; set; }
            [NotMapped]
            public string REASON { get; set; }
            [NotMapped]
            public long? STATUS_ID { get; set; }
            [NotMapped]
            public string STATUS { get; set; }
            [NotMapped]
            public string PROVIDER { get; set; }
            [NotMapped]
            public string DESCRIPTION { get; set; }
            [NotMapped]
            public long? POS_ID { get; set; }
            [NotMapped]
            public string POS_CODE { get; set; }
            [NotMapped]
            public string POS_NAME { get; set; }
            [NotMapped]
            public string POS_ADDRESS { get; set; }
            [NotMapped]
            public string POS_CITY { get; set; }
            [NotMapped]
            public string POS_STATE { get; set; }
            [NotMapped]
            public string POS_ZIP { get; set; }
            [NotMapped]
            public long? REGION_ID { get; set; }
            [NotMapped]
            public string REGION_CODE { get; set; }
            public long? VISIT_TYPE_ID { get; set; }
            public string TIME_TO { get; set; }
            public long? PROVIDER_ID { get; set; }
            public long? CASE_ID { get; set; }
            public long? PRACTICE_CODE { get; set; }
            public long? APPOINTMENT_STATUS_ID { get; set; }
            public string SIGNED_BY { get; set; }
            public string REASON_FOR_SIGNING { get; set; }
            public string SIGNATURE_PATH { get; set; }
            public Double? LATITUDE { get; set; }
            public Double? LONGITUDE { get; set; }
            public string NOTES { get; set; }
            public DateTime? CREATED_DATE { get; set; }
            public string CREATED_BY { get; set; }
            public DateTime? MODIFIED_DATE { get; set; }
            public string MODIFIED_BY { get; set; }
            public bool DELETED { get; set; }
            public int? VISIT_FREQUENCY { get; set; }
            public string VISIT_CRITERIA { get; set; }
            public int? DURATION_DAYS { get; set; }
            public bool? IsSendForApproval { get; set; }
            public long? AssignToProviderID { get; set; }
            public long? PTOID { get; set; }
            public bool? IsClinicianAssignHisOwnAppt { get; set; }
            public bool? IsApproveRD { get; set; }
            public bool? IsDenyRD { get; set; }
            public bool? IsSend { get; set; }
            public long? SendToID { get; set; }
            public bool? IsSendToORS { get; set; }
            public DateTime? SendDate { get; set; }
            public bool? IsBlocked { get; set; }
            public string ReasonForBlocked { get; set; }
            [NotMapped]
            public int? TOTAL_RECORD_PAGES { get; set; }
            [NotMapped]
            public int? TOTAL_RECORDS { get; set; }
            [NotMapped]
            public bool IS_PATIENT_OVERLAPPING { get; set; }
            [NotMapped]
            public bool IS_PROVIDER_OVERLAPPING { get; set; }
            [NotMapped]
            public bool IS_PROVIDER_AVAILABLE { get; set; }
            [NotMapped]
            public bool IS_NEW { get; set; }
            [NotMapped]
            public bool IS_RECURSIVE { get; set; }
            [NotMapped]
            public List<NoteAlert> Alerts { get; set; }
            public long? CANCELLATION_REASON_ID { get; set; }
            [NotMapped]
            public string CANCELLATION_DESCRIPTION { get; set; }
            [NotMapped]
            public long? CANCELATIONREASON_ID { get; set; }
            [NotMapped]
            public string CANCELLATION_CODE { get; set; }
            [NotMapped]
            public double? PATIENTA_ADDRESS_LONGITUDE { get; set; }
            [NotMapped]
            public double? PATIENT_ADDRESS_LATITUDE { get; set; }
            [NotMapped]
            public double? AL_LONGITUDE { get; set; }
            [NotMapped]
            public double? AL_LATITUDE { get; set; }
            public double? ACCURACY { get; set; }
            [NotMapped]
            public string APPT_LATITUDE_STR { get; set; }
            [NotMapped]
            public string APPT_LONGITUDE_STR { get; set; }
            [NotMapped]
            public string PATIENTA_ADDRESS_LONGITUDE_STR { get; set; }
            [NotMapped]
            public string PATIENT_ADDRESS_LATITUDE_STR { get; set; }
            [NotMapped]
            public string AL_LONGITUDE_STR { get; set; }
            [NotMapped]
            public string AL_LATITUDE_STR { get; set; }
            [NotMapped]
            public string CITY { get; set; }
            [NotMapped]
            public string STATE { get; set; }
            [NotMapped]
            public string ZIP { get; set; }          
        }

        [Table("FOX_TBL_CANCELLATION_REASON")]
        public class CancellationReason :BaseModel
        {
            [Key]
            public long CANCELLATION_REASON_ID { get; set; }
            public  string CANCELLATION_CODE { get; set; }
            public string CANCELLATION_DESCRIPTION { get; set; }
            public string RT_CANCELLATION_CODE { get; set; }
            public DateTime? CREATED_DATE { get; set; }
            public string CREATED_BY { get; set; }
            public DateTime? MODIFIED_DATE { get; set; }
            public string MODIFIED_BY { get; set; }
            public bool DELETED { get; set; }
        }
        public class AppointmentSearchRequest : BaseModel
        {
            public string PATIENT_ACCOUNT { get; set; }
            public long? PRACTICE_CODE { get; set; }
            public string SEARCH_TEXT { get; set; }
            public string PROVIDER { get; set; }
            public string PROVIDER_ID { get; set; }
            public string REASON { get; set; }
            public string STATUS { get; set; }
            public string REGION { get; set; }
            public long LOCATION { get; set; }
            public string DISCIPLINE { get; set; }
            public DateTime? DATE_FROM { get; set; }
            public string DATE_FROM_STR { get; set; }
            public DateTime? DATE_TO { get; set; }
            public string DATE_TO_STR { get; set; }
            public int CURRENT_PAGE { get; set; }
            public int RECORD_PER_PAGE { get; set; }
            public string SORT_BY { get; set; }
            public string SORT_ORDER { get; set; }
            public int TIME_FRAME { get; set; }
            public string INSURANCE_ID { get; set; }
        }
        [Table("FOX_TBL_APPOINTMENT_STATUS")]
        public class AppointmentStatus : BaseModel
        {
            [Key]
            public long APPOINTMENT_STATUS_ID { get; set; }
            public string DESCRIPTION { get; set; }
            public DateTime? CREATED_DATE { get; set; }
            public string CREATED_BY { get; set; }
            public DateTime? MODIFIED_DATE { get; set; }
            public string MODIFIED_BY { get; set; }
            public bool? DELETED { get; set; }
        }

        [Table("FOX_TBL_VISIT_TYPE")]
        public class VisitType : BaseModel
        {
            [Key]
            public long VISIT_TYPE_ID { get; set; }
            public string DESCRIPTION { get; set; }
            public DateTime? CREATED_DATE { get; set; }
            public string CREATED_BY { get; set; }
            public DateTime? MODIFIED_DATE { get; set; }
            public string MODIFIED_BY { get; set; }
            public bool? DELETED { get; set; }
            public bool? SHOW_FOR_APPOINTMENT { get; set; }
        }
        public class ProviderList: BaseModel
        {
            public long FOX_PROVIDER_ID { get; set; }
            public string PROVIDER_NAME { get; set; }
        }
        public class RegionList : BaseModel
        {
            public long REFERRAL_REGION_ID { get; set; }
            public string REFERRAL_REGION_CODE { get; set; }
            public string REFERRAL_REGION_NAME { get; set; }
        }
        public class WeekAppointmentsWithDay
        {
            public List<Appointment> Monday { get; set; }
            public List<Appointment> Tuesday { get; set; }
            public List<Appointment> Wednesday { get; set; }
            public List<Appointment> Thursday { get; set; }
            public List<Appointment> Friday { get; set; }
            public List<Appointment> Saturday { get; set; }
            public List<Appointment> Sunday { get; set; }
            public string OfficeTimeStartFrom { get; set; }
            public string OfficeTimeEndAt { get; set; }
            public string AppointmentTimeLimit { get; set; }
        }
        ////This is for wekly Appointments
        public class SPGetLocations
        {
            public long LOCATION_CODE { get; set; }
            public string LOCATION_NAME { get; set; }
            public string LOCATION_ADDRESS { get; set; } 
            public string LOCATION_COMPLETE_ADDRESS { get; set; }
            public string PHONE_NUMBER { get; set; }
            public string FAX_NUMBER { get; set; }
        }
    }
}
