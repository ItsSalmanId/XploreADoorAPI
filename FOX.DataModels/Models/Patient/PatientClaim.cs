using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.UI.WebControls;

namespace FOX.DataModels.Models.Patient
{
    public class ClaimInsuranceSearchReq
    {
        public string Patient_Account { get; set; }
        public List<long> Patient_Insurance_Ids { get; set; }
        public string Effective_Date { get; set; }
        public string Termination_Date { get; set; }
    }

    public class ClaimInsuranceViewModel
    {
        public long PATIENT_ACCOUNT { get; set; }
        public string PATIENT_ACCOUNT_STR
        {
            get
            {
                return PATIENT_ACCOUNT.ToString();
            }
        }
        public long PATIENT_INSURANCE_ID { get; set; }
        public long CLAIM_INSURANCE_ID { get; set; }
        public long INSURANCE_ID { get; set; }
        public DateTime? DOS { get; set; }
        public long? CLAIM_NO { get; set; }
        public long? CASE_NO { get; set; }
        public decimal? AMOUNT_BILLED { get; set; }
        public decimal? AMOUNT_PAID { get; set; }
        public decimal? AMOUNT_DUE { get; set; }
        public string STATUS { get; set; }
        public bool CHECKED { get; set; }
    }

    //[Table("Claims")]
    //public class Claims
    //{
    //    [Key]
    //    public long Claim_No { get; set; }
    //    public long? Patient_Account { get; set; }
    //    //public DateTime? BILL_DATE { get; set; }
    //    public DateTime? DOS { get; set; }
    //    public long? Location_Code { get; set; }
    //    public long? Attending_Physician { get; set; }
    //    public long? Billing_Physician { get; set; }
    //    public long? Supervising_Physician { get; set; }
    //    public long? Referring_Physician { get; set; }
    //    public string PA_NUMBER { get; set; }
    //    public string Referral_Number { get; set; }
    //    public string ICN_Number { get; set; }
    //    public long? Facility_Code { get; set; }
    //    public DateTime? Hospital_From { get; set; }
    //    public DateTime? Hospital_To { get; set; }
    //    public string PRI_STATUS { get; set; }
    //    public string SEC_STATUS { get; set; }
    //    public string OTH_STATUS { get; set; }
    //    public string PAT_STATUS { get; set; }
    //    public long? Attach_Type_Code { get; set; }
    //    public string Claim_Status { get; set; }
    //    public DateTime? Claim_Status_Date { get; set; }
    //    public string Current_Visit { get; set; }
    //    public string Allowed_Visit { get; set; }
    //    public bool? Accident_Auto { get; set; }
    //    public bool? Accident_Other { get; set; }
    //    public bool? Employment { get; set; }
    //    public bool? Accident_Emergency { get; set; }
    //    public bool? Accident_Time { get; set; }
    //    public DateTime? Accident_Date { get; set; }
    //    public string Accident_State { get; set; }
    //    public string Spinal_Manipulation_Condition_Code { get; set; }
    //    public string Spinal_Manipulation_Description { get; set; }
    //    public bool? Spinal_Manipulation_Xray_Availability { get; set; }
    //    public string Phy_Exam_Code { get; set; }
    //    public string Phy_Exam_Desc { get; set; }
    //    public DateTime? Start_Care_Date { get; set; }
    //    public DateTime? Last_Seen_Date { get; set; }
    //    public DateTime? Current_Illness_Date { get; set; }
    //    public DateTime? X_Ray_Date { get; set; }
    //    public decimal? Pri_Ins_Payment { get; set; }
    //    public decimal? Sec_Ins_Payment { get; set; }
    //    public decimal? Oth_Ins_Payment { get; set; }
    //    public decimal? Patient_Payment { get; set; }
    //    public decimal? Adjustment { get; set; }
    //    public decimal? Amt_Due { get; set; }
    //    public decimal? Amt_Paid { get; set; }
    //    public decimal? Claim_Total { get; set; }
    //    public string DX_Code1 { get; set; }
    //    public string DX_Code2 { get; set; }
    //    public string DX_Code3 { get; set; }
    //    public string DX_Code4 { get; set; }
    //    public string DX_Code5 { get; set; }
    //    public string DX_Code6 { get; set; }
    //    public string DX_Code7 { get; set; }
    //    public string DX_Code8 { get; set; }
    //    public byte? AA { get; set; }
    //    public string BLOCK1213 { get; set; }
    //    public string POS { get; set; }
    //    public DateTime? REBILL_DATE { get; set; }
    //    public bool? PTL_Status { get; set; }
    //    public string DELAY_REASON_CODE { get; set; }
    //    public DateTime? REF_DATE { get; set; }
    //    public bool? Add_CLIA_Number { get; set; }
    //    public string Special_Program_Code { get; set; }
    //    public string Name_Dummy { get; set; }
    //    public bool? Print_Center { get; set; }
    //    public DateTime? Injury_Date { get; set; }
    //    public DateTime? Injury_Time { get; set; }
    //    public string Epsdt_Services { get; set; }
    //    public long? Hl7claim_No { get; set; }
    //    public string HCFA_Note { get; set; }
    //    public bool? Patient_Payment_Plan { get; set; }
    //    public bool? Patient_Statement { get; set; }
    //    public bool? Include_In_Sdf { get; set; }
    //    public bool? Is_Self_Pay { get; set; }
    //    public string Eligibility_Status { get; set; }
    //    public DateTime? Eligibility_Inquiry_Date { get; set; }
    //    public bool? Deleted { get; set; }
    //    public string Created_By { get; set; }
    //    public DateTime? Created_Date { get; set; }
    //    public string Modified_By { get; set; }
    //    public DateTime? Modified_Date { get; set; }
    //    public string scan_no { get; set; }
    //    public bool? resolve { get; set; }
    //    public DateTime Sync_Date { get; set; }
    //    public string reference_number { get; set; }
    //    public DateTime? Scan_Date { get; set; }
    //    public long? Ordering_Physician { get; set; }
    //    public byte? Response_Code { get; set; }
    //    public string Condition_Code { get; set; }
    //    public string Reference_Claim_No { get; set; }
    //    public DateTime? Emr_Bill_Date { get; set; }
    //    public DateTime? Web_Resolve_Date { get; set; }
    //    public DateTime? Web_Modified_Date { get; set; }
    //    public Guid rowguid { get; set; }
    //    public string Map_Claim_History { get; set; }
    //    public DateTime? LMP_Date { get; set; }
    //    public DateTime? SCAN_DATE_PTL { get; set; }
    //    public string PAGE_NO { get; set; }
    //    public string Weight { get; set; }
    //    public string Transport_Distance { get; set; }
    //    public string Transportation_Reason_Code { get; set; }
    //    public string Transportation_Condition_Code { get; set; }
    //    public string Transport_Code { get; set; }
    //    public string Condition_Indicator { get; set; }
    //    public bool? STOP_BRC { get; set; }
    //    public DateTime? EDC_date { get; set; }
    //    public string Institution_Condition_Code { get; set; }
    //    public string SpecialProgramCode { get; set; }
    //    public string ServiceAuthExceptionCode { get; set; }
    //    public bool? Itemized_Statement { get; set; }
    //    public bool? Archive { get; set; }
    //    public bool? Arbitration { get; set; }
    //    public long? Last_Seen_Physician { get; set; }
    //    public bool? BRC { get; set; }
    //    public bool? IS_DRAFT { get; set; }
    //    public long? DRAFT_PATIENT_ACCOUNT { get; set; }
    //    public bool? Copay_Owed { get; set; }
    //    public bool? Copay_Waived { get; set; }
    //    public DateTime? Manifestation_Date { get; set; }
    //    public DateTime? ASSUMED_CARE_DATE { get; set; }
    //    public DateTime? RELINQISHED_CARE_DATE { get; set; }
    //    public long? Claims_Status_Code { get; set; }
    //    public string Coded_By { get; set; }
    //    public DateTime? TCM_Cal_Dos { get; set; }
    //    public bool? Stop_Submission { get; set; }
    //    public bool? Additional_Estatement { get; set; }
    //    public DateTime? Last_statement_Sent_date { get; set; }
    //    public string DX_Code9 { get; set; }
    //    public string DX_Code10 { get; set; }
    //    public string DX_Code11 { get; set; }
    //    public string DX_Code12 { get; set; }
    //    public DateTime? Last_Work_Date { get; set; }
    //    public bool? SEND_E_STATEMENT { get; set; }
    //    public string FACILITY_REF_NO { get; set; }
    //    public string Sign_off { get; set; }
    //    public string Sign_off_reasons { get; set; }
    //    public string Sign_by { get; set; }
    //    public DateTime? Signoff_Date { get; set; }
    //    public string BATCH_NO { get; set; }
    //    public string SIGN_OFF_REMARKS { get; set; }
    //    public DateTime? BATCH_DATE { get; set; }
    //    public long? Plan_Code { get; set; }
    //    public string Linked_Claims { get; set; }
    //    public bool? Authorization_Req { get; set; }
    //    public string PROMISED_AMT_WAIVE { get; set; }
    //    public DateTime? PROMISED_AMT_WAIVE_DATE { get; set; }
    //    public string PROMISED_AMT_WAIVE_BY { get; set; }
    //    public decimal? PROMISED_AMT { get; set; }
    //    public DateTime? PROMISED_AMT_DATE { get; set; }
    //    public string PROMISED_AMT_ENTERED_BY { get; set; }
    //    public long? DWC_ID { get; set; }
    //    public long? DWC_DETAIL_ID { get; set; }
    //    public bool? Advance_Pat_Payment { get; set; }
    //    public string created_From { get; set; }
    //    public string modified_from { get; set; }
    //    public long? PA_TRACKING_ID { get; set; }
    //    public long? PRIMARY_CARE_PHYSICIAN { get; set; }
    //}

    [Table("Claim_Insurance")]
    public class ClaimInsurance
    {
        [Key]
        public long Claim_Insurance_Id { get; set; }
        public long Patient_Account { get; set; }
        public long Claim_No { get; set; }
        public long Insurance_Id { get; set; }
        public string Pri_Sec_Oth_Type { get; set; }
        //public decimal? Co_Payment { get; set; }
        //public decimal? Deductions { get; set; }
        //public string Policy_Number { get; set; }
        //public string Group_Number { get; set; }
        //public DateTime? Effective_Date { get; set; }
        //public DateTime? Termination_Date { get; set; }
        //public long? Subscriber { get; set; }
        //public string Relationship { get; set; }
        //public string Eligibility_Status { get; set; }
        //public long? Eligibility_S_No { get; set; }
        //public DateTime? Eligibility_Enquiry_Date { get; set; }
        //public string Access_Carolina_Number { get; set; }
        //public bool? Is_Capitated_Claim { get; set; }
        //public int? Allowed_Visits { get; set; }
        //public int? Remaining_Visits { get; set; }
        //public DateTime? Visits_Start_Date { get; set; }
        //public DateTime? Visits_End_Date { get; set; }
        //public long? HL7PatInsurance_Id { get; set; }
        public bool? Deleted { get; set; }
        public string Created_By { get; set; }
        public DateTime? Created_Date { get; set; }
        public string Modified_By { get; set; }
        public DateTime? Modified_Date { get; set; }
        //public DateTime Sync_Date { get; set; }
        //public string CCN { get; set; }
        //public Guid rowguid { get; set; }
        //public string Group_Name { get; set; }
        //public bool? Print_Center { get; set; }
        //public bool? Corrected_Claim { get; set; }
        //public string ICN { get; set; }
        //public bool? Late_Filing { get; set; }
        //public string Late_Filing_Reason { get; set; }
        //public string MCR_SEC_Payer { get; set; }
        //public long? MCR_SEC_Payer_Code { get; set; }
        //public string Notes { get; set; }
        //public bool? Send_notes { get; set; }
        //public string certification_number { get; set; }
        //public string certification_action { get; set; }
        //public DateTime? certification_issue_date { get; set; }
        //public DateTime? certification_expiry_date { get; set; }
        //public string rej_code { get; set; }
        //public DateTime? response_date { get; set; }
        //public bool? Send_Appeal { get; set; }
        //public string Admission_Type_Code { get; set; }
        //public string Admission_Source_Code { get; set; }
        //public string Patient_Status_Code { get; set; }
        //public string Eligibility_Difference { get; set; }
        //public string Filing_Indicator_Code { get; set; }
        //public string Filing_Indicator { get; set; }
        //public string Plan_type { get; set; }
        //public string coverage_description { get; set; }
        //public bool? RECONSIDERATION { get; set; }
        //public bool? MEDICAL_NOTES { get; set; }
        //public bool? CLAIM_STATUS_REQUEST { get; set; }
        //public int? CO_PAYMENT_PER { get; set; }
        //public string Plan_Name { get; set; }
        //public string PLAN_NAME_TYPE { get; set; }
        //public string Co_Insurance { get; set; }
        //public bool? RETURNED_HCFA { get; set; }
        //public bool? Appeal_Required { get; set; }
        //public string created_From { get; set; }
        //public string modified_from { get; set; }
        //public long? PLAN_ID { get; set; }
    }

    [Table("CLAIM_NOTES")]
    public class ClaimNotes
    {
        [Key]
        public long Claim_Notes_Id { get; set; }
        public long Claim_No { get; set; }
        public long Note_Id { get; set; }
        public string Note_Detail { get; set; }
        //public string Scan_No { get; set; }
        //public string Note_State { get; set; }
        //public string No_of_Days { get; set; }
        public bool? Deleted { get; set; }
        public string Created_By { get; set; }
        public DateTime Created_Date { get; set; }
        public string Modified_By { get; set; }
        public DateTime? Modified_Date { get; set; }
        //public DateTime? Scan_Date { get; set; }
        //public int? page_no { get; set; }
        //public long? Call_ID { get; set; }
        //public string Case_Number { get; set; }
        //public long? SENT_APPEAL_ID { get; set; }
        //public decimal? PROPOSAL_AMOUNT { get; set; }
        public string created_From { get; set; }
        public string modified_from { get; set; }
        //public long? DEPOSITSLIP_ID { get; set; }
        //public string User_id { get; set; }
        //public Guid? rowguid { get; set; }
    }
}
