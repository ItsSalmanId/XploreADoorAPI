using FOX.DataModels.Models.SenderName;
using FOX.DataModels.Models.SenderType;
using FOX.DataModels.Models.Settings.ReferralSource;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FOX.DataModels.Models.RequestForOrder
{
    public class RequestForOrderModel
    {
    }
    public class ResponseGeneratingWorkOrder
    {
        //public long WorkId { get; set; }
        public long WORK_ID { get; set; }
        public string UNIQUE_ID { get; set; }
        public bool? IS_EMERGENCY_ORDER { get; set; }
        public string SORCE_NAME { get; set; }
        public string SORCE_TYPE { get; set; }
        public long? FOX_TBL_SENDER_TYPE_ID { get; set; }
        public long? FOX_TBL_SENDER_NAME_ID { get; set; }
        public List<FOX_TBL_SENDER_TYPE> SenderTypeList { get; set; }
        public List<FOX_TBL_SENDER_NAME> SenderNameList { get; set; }
        public ReferralSource UserReferralSource { get; set; }
        public string SENDER_TYPE { get; set; }
    }
    public class RequestSendEmailModel : BaseModel
    {
        public string EmailAddress { get; set; }
        public string Subject { get; set; }
        public string AttachmentHTML { get; set; }
        public string FileName { get; set; }
        public long WorkId { get; set; }
        public bool _isFromIndexInfo { get; set; }
        public string Patient_Last_Name { get; set; }
        [NotMapped]
        public string uniqueWorkId { get; set; }

    }
    public class RequestSendFAXModel : BaseModel
    {
        public string SenderName { get; set; }
        public string SenderFax { get; set; }
        public string Subject { get; set; }
        public string ReceipientFaxNumber { get; set; }
        public string Notes { get; set; }
        public string AttachmentHTML { get; set; }
        public string FileName { get; set; }
        public long WorkId { get; set; }
        public bool _isFromIndexInfo { get; set; }
        [NotMapped]
        public string uniqueWorkId { get; set; }
    }
    //public class ResponseModel
    //{
    //    public string Message { get; set; }
    //    public string ErrorMessage { get; set; }
    //    public bool Success { get; set; }
    //}
    public class ResponseHTMLToPDF
    {
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public string ErrorMessage { get; set; }
        public bool Success { get; set; }
    }
    public class RequestDeleteWorkOrder : BaseModel
    {
        public long WorkId { get; set; }
    }
    public class RequestDownloadPdfModel : BaseModel
    {
        public string AttachmentHTML { get; set; }
        public string FileName { get; set; }
    }
    public class ReqAddDocument_SignOrder : BaseModel
    {
        public long WorkId { get; set; }
        public string FileName { get; set; }
        public string AttachmentHTML { get; set; }
    }
    [Table("FOX_TBL_THERAPY_TREATMENT_REFERRAL_REQUEST_FORM")]
    public class TherapyTreatmentRequestForm
    {
        [Key]
        public long THERAPY_TREATMENT_REFERRAL_REQUEST_FORM_ID { get; set; }
        public long WORK_ID { get; set; }
        public string THERAPY_TREATMENT_REFERRAL_REQUEST_HTML { get; set; }
        public string CREATED_BY { get; set; }
        public DateTime CREATED_DATE { get; set; }
    }
}