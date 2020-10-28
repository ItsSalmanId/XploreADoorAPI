using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace FOX.DataModels.Models.SearchOrderModel
{
    public class SearchOrder
    {
        public long WORK_ID { get; set; }
        [NotMapped]
        public int ROW { get; set; }
        public string UNIQUE_ID { get; set; }
        public long? Patient_Account { get; set; }
        public string MRN { get; set; }
        public string First_Name { get; set; }
        public string Last_Name { get; set; }
        public string SSN { get; set; }
        public DateTime? Date_Of_Birth { get; set; }
        [NotMapped]
        public string Date_of_Birth_Str { get; set; }
        public string SENDER_FIRST_NAME { get; set; }
        public string SENDER_LAST_NAME { get; set; }
        public string SOURCE_NAME { get; set; }
        public string DOCUMENT_TYPE { get; set; }
        public string REGION_NAME { get; set; }
        public DateTime? RECEIVE_DATE { get; set; }
        [NotMapped]
        public string Received_Date_Str { get; set; }
        public string WORK_STATUS { get; set; }
        public DateTime? COMPLETED_DATE { get; set; }
        [NotMapped]
        public string Completed_Date_Str { get; set; }
        public double TOTAL_ROCORD_PAGES { get; set; }
        public int TOTAL_RECORDS { get; set; }
    }

    public class SearchOrderRequest: BaseModel
    {
        public string Patient_Account { get; set; }
        public string MRN { get; set; }
        public string First_Name { get; set; }
        public string Last_Name { get; set; }
        public string SSN { get; set; }
        public DateTime? Date_Of_Birth { get; set; }
        public string Date_Of_Birth_In_String { get; set; }
        public string SOURCE_NAME { get; set; }
        public string DOCUMENT_TYPE { get; set; }
        public string REGION_CODE { get; set; }
        public string SENDER_FIRST_NAME { get; set; }
        public string SENDER_LAST_NAME { get; set; }
        public DateTime? RECEIVE_FROM { get; set; }
        public DateTime? RECEIVE_TO { get; set; }
        public string RECEIVE_FROM_In_String { get; set; }
        public string RECEIVE_TO_In_String { get; set; }


        public DateTime? RECEIVE_FROM1 { get; set; }
        public DateTime? RECEIVE_TO1 { get; set; }
        public DateTime? RECEIVE_FROM_TIME { get; set; }
        public DateTime? RECEIVE_TO_TIME { get; set; }
        public string RECEIVE_FROM_Time_In_String { get; set; }
        public string RECEIVE_TO_Time_In_String { get; set; }


        public string SearchText { get; set; }
        public int RecordPerPage { get; set; }
        public int CurrentPage { get; set; }

        public string SortBy { get; set; }
        public string SortOrder { get; set; }
    }
}