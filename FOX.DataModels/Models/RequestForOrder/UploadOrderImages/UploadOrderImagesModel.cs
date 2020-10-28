using FOX.DataModels.Models.IndexInfo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FOX.DataModels.Models.RequestForOrder.UploadOrderImages
{
    public class UploadOrderImagesModel
    {
    }
    public class ReqSubmitUploadOrderImagesModel: BaseModel
    {
        public long? PATIENT_ACCOUNT { get; set; }
        public long? FOX_TBL_SENDER_TYPE_ID { get; set; }
        public long? FOX_TBL_SENDER_NAME_ID { get; set; }
        public long? SENDER_ID { get; set; }
        public long? DOCUMENT_TYPE { get; set; }
        public string DEPARTMENT_ID { get; set; }
        public long? FACILITY_ID { get; set; }
        public string FACILITY_NAME { get; set; }
        public bool IS_EMERGENCY_ORDER { get; set; }
        public List<string> FileNameList { get; set; }
        public string NOTE_DESC { get; set; }
        public long WORK_ID { get; set; }
        public bool Is_Manual_ORS { get; set; }
        public string ORS_NAME { get; set; }
        public string ORS_PHONE { get; set; }
        public string ORS_FAX { get; set; }
        public string ORS_NPI { get; set; }

    }
    public class ResSubmitUploadOrderImagesModel
    {
        public string Message { get; set; }
        public string ErrorMessage { get; set; }
        public bool Success { get; set; }
    }
    public class ResGetSourceDataModel : ResSubmitUploadOrderImagesModel
    {
        public string SORCE_NAME { get; set; }
        public long? FOX_TBL_SENDER_TYPE_ID { get; set; }
        public List<FoxDocumentType> FoxDocumentTypeList { get; set; }
    }
}