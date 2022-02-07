using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;

namespace FOX.BusinessOperations.CommonServices
{
    public static class ExportToExcel
    {

        private static void RemoveExtraColumns(DataTable dt, string CalledFrom)
        {
            DataColumnCollection dc = dt.Columns;
            if (dt.TableName == "Advanced_region")
            {
                if (dc.Contains("TOTAL_RECORD_PAGES"))
                {
                    dc.Remove("TOTAL_RECORD_PAGES");
                }
                if (dc.Contains("TOTAL_RECORDS"))
                {
                    dc.Remove("TOTAL_RECORDS");
                }
            }

            if (CalledFrom.Equals("Users_Reports"))
            {
                if (dc.Contains("USER_ID"))
                {
                    dc.Remove("USER_ID");
                }
                if (dc.Contains("SENDER_TYPE"))
                {
                    dc.Remove("SENDER_TYPE");
                }

                if (dc.Contains("USER_NAME"))
                {
                    dc.Remove("USER_NAME");
                }
                if (dc.Contains("FIRST_NAME"))
                {
                    dc.Remove("FIRST_NAME");
                }
                if (dc.Contains("LAST_NAME"))
                {
                    dc.Remove("LAST_NAME");
                }
                if (dc.Contains("PRACTICE_CODE"))
                {
                    dc.Remove("PRACTICE_CODE");
                }
                if (dc.Contains("FOX_TBL_SENDER_TYPE_ID"))
                {
                    dc.Remove("FOX_TBL_SENDER_TYPE_ID");
                }
                if (dc.Contains("TOTAL_RECORD_PAGES"))
                {
                    dc.Remove("TOTAL_RECORD_PAGES");
                }
                if (dc.Contains("PASSWORD"))
                {
                    dc.Remove("PASSWORD");
                }
                if (dc.Contains("FAILED_PASSWORD_ATTEMPT_COUNT"))
                {
                    dc.Remove("FAILED_PASSWORD_ATTEMPT_COUNT");
                }
                if (dc.Contains("PASSWORD_CHANGED_DATE"))
                {
                    dc.Remove("PASSWORD_CHANGED_DATE");
                }
                if (dc.Contains("DESIGNATION"))
                {
                    dc.Remove("DESIGNATION");
                }
                if (dc.Contains("IS_ADMIN"))
                {
                    dc.Remove("IS_ADMIN");
                }
                if (dc.Contains("Status"))
                {
                    dc.Remove("Status");
                }
                if (dc.Contains("MIDDLE_NAME"))
                {
                    dc.Remove("MIDDLE_NAME");
                }
                if (dc.Contains("DATE_OF_BIRTH"))
                {
                    dc.Remove("DATE_OF_BIRTH");
                }
                if (dc.Contains("ZIP"))
                {
                    dc.Remove("ZIP");
                }
                if (dc.Contains("USER_DISPLAY_NAME"))
                {
                    dc.Remove("USER_DISPLAY_NAME");
                }
                if (dc.Contains("RESET_PASS"))
                {
                    dc.Remove("RESET_PASS");
                }
                if (dc.Contains("SECURITY_QUESTION"))
                {
                    dc.Remove("SECURITY_QUESTION");
                }
                if (dc.Contains("SECURITY_QUESTION_ANSWER"))
                {
                    dc.Remove("SECURITY_QUESTION_ANSWER");
                }
                if (dc.Contains("LOCKEDBY"))
                {
                    dc.Remove("LOCKEDBY");
                }
                if (dc.Contains("IS_LOCKED_OUT"))
                {
                    dc.Remove("IS_LOCKED_OUT");
                }
                if (dc.Contains("LAST_LOGIN_DATE"))
                {
                    dc.Remove("LAST_LOGIN_DATE");
                }
                if (dc.Contains("COMMENTS"))
                {
                    dc.Remove("COMMENTS");
                }
                if (dc.Contains("PASS_RESET_CODE"))
                {
                    dc.Remove("PASS_RESET_CODE");
                }
                if (dc.Contains("ACTIVATION_CODE"))
                {
                    dc.Remove("ACTIVATION_CODE");
                }
                if (dc.Contains("IS_ACTIVE"))
                {
                    dc.Remove("IS_ACTIVE");
                }
                if (dc.Contains("PHONE_NO"))
                {
                    dc.Remove("PHONE_NO");
                }
                if (dc.Contains("ROLE_ID"))
                {
                    dc.Remove("ROLE_ID");
                }
                if (dc.Contains("ADDRESS_1"))
                {
                    dc.Remove("ADDRESS_1");
                }
                if (dc.Contains("ADDRESS_2"))
                {
                    dc.Remove("ADDRESS_2");
                }
                if (dc.Contains("City"))
                {
                    dc.Remove("City");
                }
                if (dc.Contains("State"))
                {
                    dc.Remove("State");
                }
                if (dc.Contains("ZIP_Code"))
                {
                    dc.Remove("ZIP_Code");
                }
                if (dc.Contains("CREATED_BY"))
                {
                    dc.Remove("CREATED_BY");
                }
                if (dc.Contains("MODIFIED_BY"))
                {
                    dc.Remove("MODIFIED_BY");
                }
                if (dc.Contains("MODIFIED_DATE"))
                {
                    dc.Remove("MODIFIED_DATE");
                }
                if (dc.Contains("DELETED"))
                {
                    dc.Remove("DELETED");
                }
                if (dc.Contains("MANAGER_ID"))
                {
                    dc.Remove("MANAGER_ID");
                }
                if (dc.Contains("Assigned_Person_Role"))
                {
                    dc.Remove("Assigned_Person_Role");
                }
                if (dc.Contains("USER_TYPE"))
                {
                    dc.Remove("USER_TYPE");
                }
                if (dc.Contains("PRACTICE_NAME"))
                {
                    dc.Remove("PRACTICE_NAME");
                }
                if (dc.Contains("NPI"))
                {
                    dc.Remove("NPI");
                }
                if (dc.Contains("MOBILE_PHONE"))
                {
                    dc.Remove("MOBILE_PHONE");
                }
                if (dc.Contains("FAX"))
                {
                    dc.Remove("FAX");
                }
                if (dc.Contains("FAX_2"))
                {
                    dc.Remove("FAX_2");
                }
                if (dc.Contains("FAX_3"))
                {
                    dc.Remove("FAX_3");
                }
                if (dc.Contains("GENDER"))
                {
                    dc.Remove("GENDER");
                }
                if (dc.Contains("LANGUAGE"))
                {
                    dc.Remove("LANGUAGE");
                }
                if (dc.Contains("TIME_ZONE"))
                {
                    dc.Remove("TIME_ZONE");
                }
                if (dc.Contains("CREATED_DATE_STR"))
                {
                    dc.Remove("CREATED_DATE_STR");
                }
                if (dc.Contains("TERMINATION_DATE"))
                {
                    dc.Remove("TERMINATION_DATE");
                }
                if (dc.Contains("SIGNATURE_PATH"))
                {
                    dc.Remove("SIGNATURE_PATH");
                }
                if (dc.Contains("PASSWORD_RESET_TICKS"))
                {
                    dc.Remove("PASSWORD_RESET_TICKS");
                }
                if (dc.Contains("WORK_PHONE"))
                {
                    dc.Remove("WORK_PHONE");
                }
                if (dc.Contains("NOTE_ID"))
                {
                    dc.Remove("NOTE_ID");

                }
                if (dc.Contains("ACO"))
                {
                    dc.Remove("ACO");
                }
                if (dc.Contains("ACO_NAME"))
                {
                    dc.Remove("ACO_NAME");
                }
                if (dc.Contains("SPECIALITY"))
                {
                    dc.Remove("SPECIALITY");
                }
                if (dc.Contains("SPECIALITY_NAME"))
                {
                    dc.Remove("SPECIALITY_NAME");
                }
                if (dc.Contains("SNF"))
                {
                    dc.Remove("SNF");
                }
                if (dc.Contains("SNF_NAME"))
                {
                    dc.Remove("SNF_NAME");
                }
                if (dc.Contains("HOSPITAL"))
                {
                    dc.Remove("HOSPITAL");
                }
                if (dc.Contains("HOSPITAL_NAME"))
                {
                    dc.Remove("HOSPITAL_NAME");
                }
                if (dc.Contains("HHH"))
                {
                    dc.Remove("HHH");
                }
                if (dc.Contains("HHH_NAME"))
                {
                    dc.Remove("HHH_NAME");
                }
                if (dc.Contains("THIRD_PARTY_REFERRAL_SOURCE"))
                {
                    dc.Remove("THIRD_PARTY_REFERRAL_SOURCE");
                }
                if (dc.Contains("COMMENT"))
                {
                    dc.Remove("COMMENT");
                }
                if (dc.Contains("PRACTICE_ORGANIZATION_ID"))
                {
                    dc.Remove("PRACTICE_ORGANIZATION_ID");
                }
                if (dc.Contains("IS_APPROVED"))
                {
                    dc.Remove("IS_APPROVED");
                }
                if (dc.Contains("hasToSendEmail"))
                {
                    dc.Remove("hasToSendEmail");
                }
                if (dc.Contains("EXTENSION"))
                {
                    dc.Remove("EXTENSION");
                }
                if (dc.Contains("IS_ACTIVE_EXTENSION"))
                {
                    dc.Remove("IS_ACTIVE_EXTENSION");
                }
                if (dc.Contains("PRACTICE_ORGANIZATION_TEXT"))
                {
                    dc.Remove("PRACTICE_ORGANIZATION_TEXT");
                }
                if (dc.Contains("ACO_TEXT"))
                {
                    dc.Remove("ACO_TEXT");
                }
                if (dc.Contains("SPECIALITY_TEXT"))
                {
                    dc.Remove("SPECIALITY_TEXT");
                }
                if (dc.Contains("SNF_TEXT"))
                {
                    dc.Remove("SNF_TEXT");
                }
                if (dc.Contains("HOSPITAL_TEXT"))
                {
                    dc.Remove("HOSPITAL_TEXT");
                }
                if (dc.Contains("HHH_TEXT"))
                {
                    dc.Remove("HHH_TEXT");
                }
                if (dc.Contains("Template"))
                {
                    dc.Remove("Template");
                }
                if (dc.Contains("USR_REFERRAL_SOURCE_ID"))
                {
                    dc.Remove("USR_REFERRAL_SOURCE_ID");
                }
                if (dc.Contains("Discriminator"))
                {
                    dc.Remove("Discriminator");
                }
                if (dc.Contains("PasswordHash"))
                {
                    dc.Remove("PasswordHash");
                }
                if (dc.Contains("TOTAL_RECORDS"))
                {
                    dc.Remove("TOTAL_RECORDS");
                }
                if (dc.Contains("IS_AD_USER"))
                {
                    dc.Remove("IS_AD_USER");
                }
                if (dc.Contains("REFERRAL_REGION_ID"))
                {
                    dc.Remove("REFERRAL_REGION_ID");
                }
                if (dc.Contains("Region"))
                {
                    dc.Remove("Region");
                }
                if (dc.Contains("FULL_ACCESS_OVER_APP"))
                {
                    dc.Remove("FULL_ACCESS_OVER_APP");
                }
                if (dc.Contains("Is_Electronic_POC"))
                {
                    dc.Remove("Is_Electronic_POC");
                }
                if (dc.Contains("Is_Blocked"))
                {
                    dc.Remove("Is_Blocked");
                }
                if (dc.Contains("DB_PASSWORD"))
                {
                    dc.Remove("DB_PASSWORD");
                }
                if (dc.Contains("SELECTED_PASSWORD"))
                {
                    dc.Remove("SELECTED_PASSWORD");
                }
                if (dc.Contains("SHOW_TO_USER_PASSWORD"))
                {
                    dc.Remove("SHOW_TO_USER_PASSWORD");
                }
                if (dc.Contains("HIDE_EYE_ICON"))
                {
                    dc.Remove("HIDE_EYE_ICON");
                }
                if (dc.Contains("ADMIN_PASSWORD"))
                {
                    dc.Remove("ADMIN_PASSWORD");
                }
                if (dc.Contains("PROFILE"))
                {
                    dc.Remove("PROFILE");
                }
                if (dc.Contains("AUTO_LOCK_TIME_SPAN"))
                {
                    dc.Remove("AUTO_LOCK_TIME_SPAN");
                }
            }
            if (CalledFrom.Equals("Patient_Helpdesk"))
            {
                if (dc.Contains("FOX_PHD_CALL_DETAILS_ID"))
                {
                    dc.Remove("FOX_PHD_CALL_DETAILS_ID");
                }
                if (dc.Contains("PATIENT_ACCOUNT"))
                {
                    dc.Remove("PATIENT_ACCOUNT");
                }
                if (dc.Contains("DOS"))
                {
                    dc.Remove("DOS");
                }
                if (dc.Contains("CALL_DATE"))
                {
                    dc.Remove("CALL_DATE");
                }
                if (dc.Contains("CALL_TIME"))
                {
                    dc.Remove("CALL_TIME");
                }
                if (dc.Contains("PRACTICE_CODE"))
                {
                    dc.Remove("PRACTICE_CODE");
                }
                if (dc.Contains("CREATED_BY"))
                {
                    dc.Remove("CREATED_BY");
                }
                if (dc.Contains("CREATED_DATE"))
                {
                    dc.Remove("CREATED_DATE");
                }
                if (dc.Contains("MODIFIED_BY"))
                {
                    dc.Remove("MODIFIED_BY");
                }
                if (dc.Contains("MODIFIED_DATE"))
                {
                    dc.Remove("MODIFIED_DATE");
                }
                if (dc.Contains("DELETED"))
                {
                    dc.Remove("DELETED");
                }
                if (dc.Contains("CALL_SCENARIO"))
                {
                    dc.Remove("CALL_SCENARIO");
                }
                if (dc.Contains("CALL_REASON"))
                {
                    dc.Remove("CALL_REASON");
                }
                if (dc.Contains("REQUEST"))
                {
                    dc.Remove("REQUEST");
                }
                if (dc.Contains("CALL_ATTENDED_BY"))
                {
                    dc.Remove("CALL_ATTENDED_BY");
                }
                if (dc.Contains("IS_CALL_DETAIL_EDIT"))
                {
                    dc.Remove("IS_CALL_DETAIL_EDIT");
                }
                if (dc.Contains("TOTAL_RECORD_PAGES"))
                {
                    dc.Remove("TOTAL_RECORD_PAGES");
                }
                if (dc.Contains("TOTAL_RECORDS"))
                {
                    dc.Remove("TOTAL_RECORDS");
                }
                if (dc.Contains("CURRENT_EXTENSION"))
                {
                    dc.Remove("CURRENT_EXTENSION");
                }
                if (dc.Contains("CALL_RECORDING_PATH"))
                {
                    dc.Remove("CALL_RECORDING_PATH");
                }
                if (dc.Contains("GENERAL_NOTE_ID"))
                {
                    dc.Remove("GENERAL_NOTE_ID");
                }
                if (dc.Contains("REQUEST_NAME"))
                {
                    dc.Remove("REQUEST_NAME");
                }
                if (dc.Contains("IsRecordingMapped"))
                {
                    dc.Remove("IsRecordingMapped");
                }
                if (dc.Contains("CALL_CATEGORY_ID"))
                {
                    dc.Remove("CALL_CATEGORY_ID");
                }
                if (dc.Contains("AMOUNT"))
                {
                    dc.Remove("AMOUNT");
                }
                if (dc.Contains("FOLLOW_UP_DATE"))
                {
                    dc.Remove("FOLLOW_UP_DATE");
                }
                if (dc.Contains("ATTACHMENT"))
                {
                    dc.Remove("ATTACHMENT");
                }
                if (dc.Contains("IsNewPatient"))
                {
                    dc.Remove("IsNewPatient");
                }
                if (dc.Contains("CALLER_NAME"))
                {
                    dc.Remove("CALLER_NAME");
                }
                if (dc.Contains("IS_FOLLOW_UP"))
                {
                    dc.Remove("IS_FOLLOW_UP");
                }
                if (dc.Contains("firstName"))
                {
                    dc.Remove("firstName");
                }
                if (dc.Contains("lastName"))
                {
                    dc.Remove("lastName");
                }
                if (dc.Contains("ATTACHMENT_NAME"))
                {
                    dc.Remove("ATTACHMENT_NAME");
                }
                if (dc.Contains("DOCUMENT_TYPE"))
                {
                    dc.Remove("DOCUMENT_TYPE");
                }
                if (dc.Contains("ATTACHMENT_PATH"))
                {
                    dc.Remove("ATTACHMENT_PATH");
                }
                if (dc.Contains("_IsSSCM"))
                {
                    dc.Remove("_IsSSCM");
                }
                if (dc.Contains("PRIORITY"))
                {
                    dc.Remove("PRIORITY");
                }
                if(dc.Contains("CS_CASE_CATEGORY"))
                {
                    dc.Remove("CS_CASE_CATEGORY");
                }
                if (dc.Contains("FILE_PATH"))
                {
                    dc.Remove("FILE_PATH");
                }
            }
            if(CalledFrom.Equals("Advanced_Daily_Report"))
            {
                if(dc.Contains("CALL_USER_ID"))
                {
                    dc.Remove("CALL_USER_ID");
                }
                if (dc.Contains("AMOUNT"))
                {
                    dc.Remove("AMOUNT");
                }
            }
            if (CalledFrom.Equals("Task_User_Level"))
            {
                if (dc.Contains("CREATED_DATE"))
                {
                    dc.Remove("CREATED_DATE");
                }
                if (dc.Contains("PATIENT_ACCOUNT_STR"))
                {
                    dc.Remove("PATIENT_ACCOUNT_STR");
                }
                if (dc.Contains("DUE_DATE_TIME"))
                {
                    dc.Remove("DUE_DATE_TIME");
                }
                if (dc.Contains("DATE_OF_BIRTH"))
                {
                    dc.Remove("DATE_OF_BIRTH");
                }
                if (dc.Contains("GENDER"))
                {
                    dc.Remove("GENDER");
                }
                if (dc.Contains("CASE_ID"))
                {
                    dc.Remove("CASE_ID");
                }
                if (dc.Contains("TASK_ID"))
                {
                    dc.Remove("TASK_ID");
                }
                if (dc.Contains("CATEGORY_NAME"))
                {
                    dc.Remove("CATEGORY_NAME");
                }
                if (dc.Contains("SEND_TO_ID"))
                {
                    dc.Remove("SEND_TO_ID");
                }
                if (dc.Contains("TASK_TYPE_DESCRIPTION"))
                {
                    dc.Remove("TASK_TYPE_DESCRIPTION");
                }
                if (dc.Contains("CREATED_BY"))
                {
                    dc.Remove("CREATED_BY");
                }
                //if (dc.Contains("MODIFIED_DATE"))
                //{
                //    dc.Remove("MODIFIED_DATE");
                //}
                if (dc.Contains("TOTAL_RECORD_PAGES"))
                {
                    dc.Remove("TOTAL_RECORD_PAGES");
                }
                if (dc.Contains("TOTAL_RECORDS"))
                {
                    dc.Remove("TOTAL_RECORDS");
                }
                if (dc.Contains("CASE_NO"))
                {
                    dc.Remove("CASE_NO");
                }
                if (dc.Contains("MODIFIED_BY"))
                {
                    dc.Remove("MODIFIED_BY");
                }

            }
            if (CalledFrom.Equals("Task"))
            {
                if (dc.Contains("CREATED_DATE"))
                {
                    dc.Remove("CREATED_DATE");
                }
                if (dc.Contains("PATIENT_ACCOUNT_STR"))
                {
                    dc.Remove("PATIENT_ACCOUNT_STR");
                }
                if (dc.Contains("DUE_DATE_TIME"))
                {
                    dc.Remove("DUE_DATE_TIME");
                }
                if (dc.Contains("DATE_OF_BIRTH"))
                {
                    dc.Remove("DATE_OF_BIRTH");
                }
                if (dc.Contains("GENDER"))
                {
                    dc.Remove("GENDER");
                }
                if (dc.Contains("CASE_ID"))
                {
                    dc.Remove("CASE_ID");
                }
                if (dc.Contains("TASK_ID"))
                {
                    dc.Remove("TASK_ID");
                }
                if (dc.Contains("CATEGORY_NAME"))
                {
                    dc.Remove("CATEGORY_NAME");
                }
                if (dc.Contains("SEND_TO_ID"))
                {
                    dc.Remove("SEND_TO_ID");
                }
                if (dc.Contains("TASK_TYPE_DESCRIPTION"))
                {
                    dc.Remove("TASK_TYPE_DESCRIPTION");
                }
                if (dc.Contains("CREATED_BY"))
                {
                    dc.Remove("CREATED_BY");
                }
                //if (dc.Contains("MODIFIED_DATE"))
                //{
                //    dc.Remove("MODIFIED_DATE");
                //}
                if (dc.Contains("TOTAL_RECORD_PAGES"))
                {
                    dc.Remove("TOTAL_RECORD_PAGES");
                }
                if (dc.Contains("TOTAL_RECORDS"))
                {
                    dc.Remove("TOTAL_RECORDS");
                }
                //if (dc.Contains("PATIENT_ACCOUNT"))
                //{
                //    dc.Remove("PATIENT_ACCOUNT");
                //}
                //if (dc.Contains("PATIENT_FULL_NAME"))
                //{
                //    dc.Remove("PATIENT_FULL_NAME");
                //}
                if (dc.Contains("CASE_NO"))
                {
                    dc.Remove("CASE_NO");
                }
                if (dc.Contains("MODIFIED_BY"))
                {
                    dc.Remove("MODIFIED_BY");
                }
            }
            if (CalledFrom.Equals("Patient_Documents"))
            {
                if (dc.Contains("PATIENT_ACCOUNT"))
                {
                    dc.Remove("PATIENT_ACCOUNT");
                }
                if (dc.Contains("Patient_AccountStr"))
                {
                    dc.Remove("Patient_AccountStr");
                }
                if (dc.Contains("DOCUMENT_TYPE"))
                {
                    dc.Remove("DOCUMENT_TYPE");
                }
                if (dc.Contains("FILE_PATH"))
                {
                    dc.Remove("FILE_PATH");
                }
                if (dc.Contains("FILE_PATH1"))
                {
                    dc.Remove("FILE_PATH1");
                }
                if (dc.Contains("TOTAL_RECORDS"))
                {
                    dc.Remove("TOTAL_RECORDS");
                }
                if (dc.Contains("TOTAL_RECORD_PAGES"))
                {
                    dc.Remove("TOTAL_RECORD_PAGES");
                }
                if (dc.Contains("PAT_DOCUMENT_ID"))
                {
                    dc.Remove("PAT_DOCUMENT_ID");
                }
                if (dc.Contains("CASE_STATUS_ID"))
                {
                    dc.Remove("CASE_STATUS_ID");
                }
                if (dc.Contains("CASE_ID"))
                {
                    dc.Remove("CASE_ID");
                }
                if (dc.Contains("PARENT_DOCUMENT_ID"))
                {
                    dc.Remove("PARENT_DOCUMENT_ID");
                }
                if (dc.Contains("CASE_LIST"))
                {
                    dc.Remove("CASE_LIST");
                }
                if (dc.Contains("SHOW_ON_PATIENT_PORTAL"))
                {
                    dc.Remove("SHOW_ON_PATIENT_PORTAL");
                }
                if (dc.Contains("CREATED_BY"))
                {
                    dc.Remove("CREATED_BY");
                }
                if (dc.Contains("CREATED_DATE"))
                {
                    dc.Remove("CREATED_DATE");
                }
                if (dc.Contains("MODIFIED_BY"))
                {
                    dc.Remove("MODIFIED_BY");
                }
                if (dc.Contains("MODIFIED_DATE"))
                {
                    dc.Remove("MODIFIED_DATE");
                }
                if (dc.Contains("IMAGE_FILE_ID"))
                {
                    dc.Remove("IMAGE_FILE_ID");
                }
                if (dc.Contains("START_DATE"))
                {
                    dc.Remove("START_DATE");
                }
                if (dc.Contains("END_DATE"))
                {
                    dc.Remove("END_DATE");
                }
                if (dc.Contains("CASE_NO"))
                {
                    dc.Remove("CASE_NO");
                }
            }
            if (CalledFrom.Equals("Supervisor_Work"))
            {
                if (dc.Contains("Patient_Account"))
                {
                    dc.Remove("Patient_Account");
                }
                if (CalledFrom.Equals("TOTAL_ROCORD_PAGES"))
                {
                    dc.Remove("TOTAL_ROCORD_PAGES");
                }

                if (dc.Contains("WORK_ID"))
                {
                    dc.Remove("WORK_ID");
                }

                if (dc.Contains("ROW"))
                {
                    dc.Remove("ROW");
                }

                if (dc.Contains("PRACTICE_CODE"))
                {
                    dc.Remove("PRACTICE_CODE");
                }
                if (dc.Contains("SORCE_NAMEFormat"))
                {
                    dc.Remove("SORCE_NAMEFormat");
                }
                if (dc.Contains("COMPLETED_BY"))
                {
                    dc.Remove("COMPLETED_BY");
                }
                if (dc.Contains("COMPLETED_DATE"))
                {
                    dc.Remove("COMPLETED_DATE");
                }
                if (dc.Contains("Document_Type"))
                {
                    dc.Remove("Document_Type");
                }
                if (dc.Contains("SENDER_ID"))
                {
                    dc.Remove("SENDER_ID");
                }
                if (dc.Contains("FACILITY_NAME"))
                {
                    dc.Remove("FACILITY_NAME");
                }
                if (dc.Contains("DEPARTMENT_ID"))
                {

                    dc.Remove("DEPARTMENT_ID");
                }
                if (dc.Contains("IS_EMERGENCY_ORDER"))
                {
                    dc.Remove("IS_EMERGENCY_ORDER");
                }
                if (dc.Contains("REASON_FOR_VISIT"))
                {
                    dc.Remove("REASON_FOR_VISIT");
                }
                if (dc.Contains("ACCOUNT_NUMBER"))
                {
                    dc.Remove("ACCOUNT_NUMBER");
                }
                if (dc.Contains("UNIT_CASE_NO"))
                {
                    dc.Remove("UNIT_CASE_NO");
                }
                if (dc.Contains("CREATED_BY"))
                {
                    dc.Remove("CREATED_BY");
                }
                if (dc.Contains("CREATED_DATE"))
                {
                    dc.Remove("CREATED_DATE");
                }
                if (dc.Contains("MODIFIED_BY"))
                {
                    dc.Remove("MODIFIED_BY");
                }
                if (dc.Contains("MODIFIED_DATE"))
                {
                    dc.Remove("MODIFIED_DATE");
                }
                if (dc.Contains("DELETED"))
                {
                    dc.Remove("DELETED");
                }

                if (dc.Contains("IS_EMERGENCY"))
                {
                    dc.Remove("IS_EMERGENCY");
                }
                if (dc.Contains("IS_UNSIGNED"))
                {
                    dc.Remove("IS_UNSIGNED");
                }
                if (dc.Contains("NO_OF_SPLITS"))
                {
                    dc.Remove("NO_OF_SPLITS");
                }
                if (dc.Contains("FILE_PATH"))
                {
                    dc.Remove("FILE_PATH");
                }
                if (dc.Contains("ASSIGNED_TO"))
                {
                    dc.Remove("ASSIGNED_TO");
                }
                if (dc.Contains("TOTAL_ROCORD_PAGES"))
                {
                    dc.Remove("TOTAL_ROCORD_PAGES");
                }
                if (dc.Contains("TOTAL_RECORDS"))
                {
                    dc.Remove("TOTAL_RECORDS");
                }
            }
            if (CalledFrom.Equals("Facility_Creation"))
            {

                if (dc.Contains("MODIFIED_DATE"))
                {
                    dc.Remove("MODIFIED_DATE");
                }
                if (dc.Contains("AddressType"))
                {
                    dc.Remove("AddressType");
                }
                if (dc.Contains("Email_Address"))
                {
                    dc.Remove("Email_Address");
                }
                if (dc.Contains("Cell_Phone"))
                {
                    dc.Remove("Cell_Phone");
                }
                if (dc.Contains("Work_Phone"))
                {
                    dc.Remove("Work_Phone");
                }
                if (dc.Contains("PRACTICE_CODE"))
                {
                    dc.Remove("PRACTICE_CODE");
                }
                if (dc.Contains("TOTAL_RECORDS"))
                {
                    dc.Remove("TOTAL_RECORDS");
                }
                if (dc.Contains("TOTAL_RECORD_PAGES"))
                {
                    dc.Remove("TOTAL_RECORD_PAGES");
                }
                if (dc.Contains("IS_PRIVATE_HOME"))
                {
                    dc.Remove("IS_PRIVATE_HOME");
                }
                if (dc.Contains("DELETED"))
                {
                    dc.Remove("DELETED");
                }
                if (dc.Contains("MODIFIED_BY"))
                {
                    dc.Remove("MODIFIED_BY");
                }
                if (dc.Contains("CREATED_DATE"))
                {
                    dc.Remove("CREATED_DATE");
                }
                if (dc.Contains("CREATED_BY"))
                {
                    dc.Remove("CREATED_BY");
                }
                if (dc.Contains("Last_Update"))
                {
                    dc.Remove("Last_Update");
                }

                if (dc.Contains("ZIP"))
                {
                    dc.Remove("ZIP");
                }
                if (dc.Contains("State"))
                {
                    dc.Remove("State");
                }
                if (dc.Contains("City"))
                {
                    dc.Remove("City");
                }
                if (dc.Contains("LOC_ID"))
                {
                    dc.Remove("LOC_ID");
                }
                if (dc.Contains("Address"))
                {
                    dc.Remove("Address");
                }

                if (dc.Contains("REGION"))
                {
                    dc.Remove("REGION");
                }

                if (dc.Contains("Phone"))
                {
                    dc.Remove("Phone");
                }

                if (dc.Contains("Fax"))
                {
                    dc.Remove("Fax");
                }

                if (dc.Contains("FOL"))
                {
                    dc.Remove("FOL");
                }

            }
            if (CalledFrom.Equals("PHR_Users_Last_Login_Report"))
            {
                if (dc.Contains("TOTAL_RECORD_PAGES"))
                {
                    dc.Remove("TOTAL_RECORD_PAGES");
                }
                if (dc.Contains("USER_ID"))
                {
                    dc.Remove("USER_ID");
                }
                if (dc.Contains("PRACTICE_CODE"))
                {
                    dc.Remove("PRACTICE_CODE");
                }
                if (dc.Contains("TOTAL_RECORDS"))
                {
                    dc.Remove("TOTAL_RECORDS");
                }
            }
            if (CalledFrom.Equals("Practice_Organization"))
            {
                if (dc.Contains("PRACTICE_ORGANIZATION_ID"))
                {
                    dc.Remove("PRACTICE_ORGANIZATION_ID");
                }
                if (dc.Contains("PRACTICE_CODE"))
                {
                    dc.Remove("PRACTICE_CODE");
                }
                if (dc.Contains("DESCRIPTION"))
                {
                    dc.Remove("DESCRIPTION");
                }
                if (dc.Contains("CREATED_BY"))
                {
                    dc.Remove("CREATED_BY");
                }
                if (dc.Contains("CREATED_DATE"))
                {
                    dc.Remove("CREATED_DATE");
                }
                if (dc.Contains("MODIFIED_BY"))
                {
                    dc.Remove("MODIFIED_BY");
                }
                if (dc.Contains("MODIFIED_DATE"))
                {
                    dc.Remove("MODIFIED_DATE");
                }
                if (dc.Contains("DELETED"))
                {
                    dc.Remove("DELETED");
                }
                if (dc.Contains("TOTAL_RECORD_PAGES"))
                {
                    dc.Remove("TOTAL_RECORD_PAGES");
                }
                if (dc.Contains("TOTAL_RECORDS"))
                {
                    dc.Remove("TOTAL_RECORDS");
                }
                if (dc.Contains("Template"))
                {
                    dc.Remove("Template");
                }
                if (dc.Contains("IS_ACTIVE"))
                {
                    dc.Remove("IS_ACTIVE");
                }
            }
            if (CalledFrom.Equals("Corrected_Claim"))
            {
                if (dc.Contains("CORRECTED_CLAIM_ID"))
                {
                    dc.Remove("CORRECTED_CLAIM_ID");
                }
                if (dc.Contains("CHART_ID"))
                {
                    dc.Remove("CHART_ID");
                }
                if (dc.Contains("DOS_FROM"))
                {
                    dc.Remove("DOS_FROM");
                }
                if (dc.Contains("DOS_TO"))
                {
                    dc.Remove("DOS_TO");
                }
                if (dc.Contains("SOURCE_ID"))
                {
                    dc.Remove("SOURCE_ID");
                }
                if (dc.Contains("CORRECTED_CLAIM_TYPE_ID"))
                {
                    dc.Remove("CORRECTED_CLAIM_TYPE_ID");
                }
                if (dc.Contains("STATUS_ID"))
                {
                    dc.Remove("STATUS_ID");
                }
                if (dc.Contains("FOX_TBL_INSURANCE_ID"))
                {
                    dc.Remove("FOX_TBL_INSURANCE_ID");
                }
                if (dc.Contains("TOTAL_RECORD_PAGES"))
                {
                    dc.Remove("TOTAL_RECORD_PAGES");
                }
                if (dc.Contains("TOTAL_RECORDS"))
                {
                    dc.Remove("TOTAL_RECORDS");
                }
                if (dc.Contains("CHARGE_ENTRY_DATE"))
                {
                    dc.Remove("CHARGE_ENTRY_DATE");
                }
                if (dc.Contains("WORK_DATE"))
                {
                    dc.Remove("WORK_DATE");
                }
                if (dc.Contains("REQUESTED_DATE"))
                {
                    dc.Remove("REQUESTED_DATE");
                }
                if (dc.Contains("RESPONSE_DATE"))
                {
                    dc.Remove("RESPONSE_DATE");
                }
                if (dc.Contains("Message"))
                {
                    dc.Remove("Message");
                }
                if (dc.Contains("Message"))
                {
                    dc.Remove("Message");
                }

            }
            if (CalledFrom.Equals("Analysis_Report"))
            {
                if (dc.Contains("TOTAL_RECORDS"))
                {
                    dc.Remove("TOTAL_RECORDS");
                }
                if (dc.Contains("TOTAL_PAGE"))
                {
                    dc.Remove("TOTAL_PAGE");
                }
            }
            if (CalledFrom.Equals("Indexed_Queue"))
            {
                if (dc.Contains("PATIENT_ACCOUNT"))
                {
                    dc.Remove("PATIENT_ACCOUNT");
                }
                if (dc.Contains("PRACTICE_CODE"))
                {
                    dc.Remove("PRACTICE_CODE");
                }
                if (dc.Contains("SORCE_NAMEFormat"))
                {
                    dc.Remove("SORCE_NAMEFormat");
                }
                if (dc.Contains("FILE_PATH"))
                {
                    dc.Remove("FILE_PATH");
                }
                if (dc.Contains("RE_ASSIGNED_TO"))
                {
                    dc.Remove("RE_ASSIGNED_TO");
                }
                if (dc.Contains("TOTAL_ROCORD_PAGES"))
                {
                    dc.Remove("TOTAL_ROCORD_PAGES");
                }
                if (dc.Contains("NO_OF_COMMENTS"))
                {
                    dc.Remove("NO_OF_COMMENTS");
                }
                if (dc.Contains("INDEXED_DATE"))
                {
                    dc.Remove("INDEXED_DATE");
                }
                if (dc.Contains("file_path1"))
                {
                    dc.Remove("file_path1");
                }
                if (dc.Contains("Checked"))
                {
                    dc.Remove("Checked");
                }
                if (dc.Contains("TOTAL_RECORDS"))
                {
                    dc.Remove("TOTAL_RECORDS");
                }
                if (dc.Contains("IS_EMERGENCY"))
                {
                    dc.Remove("IS_EMERGENCY");
                }
                if (dc.Contains("ASSIGNTO_MEMBER"))
                {
                    dc.Remove("ASSIGNTO_MEMBER");
                }
                if (dc.Contains("WORK_ID"))
                {
                    dc.Remove("WORK_ID");
                }
            }
            if (CalledFrom.Equals("Task_Report"))
            {
                if (dc.Contains("TOTAL"))
                {
                    dc.Remove("TOTAL");
                }
                if (dc.Contains("TOTAL_RECORD_PAGES"))
                {
                    dc.Remove("TOTAL_RECORD_PAGES");
                }
                if (dc.Contains("TOTAL_RECORDS"))
                {
                    dc.Remove("TOTAL_RECORDS");
                }
                if (dc.Contains("ASSIGNED_TO"))
                {
                    dc.Remove("ASSIGNED_TO");
                }
            }
            if (CalledFrom.Equals("Clinician_Data"))
            {
                if (dc.Contains("FOX_PROVIDER_ID"))
                {
                    dc.Remove("FOX_PROVIDER_ID");
                }
                if (dc.Contains("PRACTICE_CODE"))
                {
                    dc.Remove("PRACTICE_CODE");
                }
                if (dc.Contains("PROVIDER_CODE"))
                {
                    dc.Remove("PROVIDER_CODE");
                }
                if (dc.Contains("ADDRESS"))
                {
                    dc.Remove("ADDRESS");
                }
                if (dc.Contains("SSN"))
                {
                    dc.Remove("SSN");
                }
                if (dc.Contains("DATE_OF_BIRTH"))
                {
                    dc.Remove("DATE_OF_BIRTH");
                }
                if (dc.Contains("IS_INACTIVE"))
                {
                    dc.Remove("IS_INACTIVE");
                }
                if (dc.Contains("REFERRAL_REGION_ID"))
                {
                    dc.Remove("REFERRAL_REGION_ID");
                }
                if (dc.Contains("TREATMENT_LOC_ID"))
                {
                    dc.Remove("TREATMENT_LOC_ID");
                }
                if (dc.Contains("DISCIPLINE_ID"))
                {
                    dc.Remove("DISCIPLINE_ID");
                }
                if (dc.Contains("VISIT_QOUTA_WEEK_ID"))
                {
                    dc.Remove("VISIT_QOUTA_WEEK_ID");
                }
                if (dc.Contains("CREATED_BY"))
                {
                    dc.Remove("CREATED_BY");
                }
                if (dc.Contains("CREATED_BYNAME"))
                {
                    dc.Remove("CREATED_BYNAME");
                }
                if (dc.Contains("MODIFIED_BY"))
                {
                    dc.Remove("MODIFIED_BY");
                }
                if (dc.Contains("MODIFIED_DATE"))
                {
                    dc.Remove("MODIFIED_DATE");
                }
                if (dc.Contains("DELETED"))
                {
                    dc.Remove("DELETED");
                }
                if (dc.Contains("DOB"))
                {
                    dc.Remove("DOB");
                }
                if (dc.Contains("CREATED_DATE"))
                {
                    dc.Remove("CREATED_DATE");
                }
                if (dc.Contains("FINANCE_REGION"))
                {
                    dc.Remove("FINANCE_REGION");
                }
                if (dc.Contains("TERMINATION_DATE"))
                {
                    dc.Remove("TERMINATION_DATE");
                }
                if (dc.Contains("ACTIVE_INACTIVE"))
                {
                    dc.Remove("ACTIVE_INACTIVE");
                }
                if (dc.Contains("SPECIALITY"))
                {
                    dc.Remove("SPECIALITY");
                }
                if (dc.Contains("TAXONOMY_CODE"))
                {
                    dc.Remove("TAXONOMY_CODE");
                }
                if (dc.Contains("TOTAL_RECORD_PAGES"))
                {
                    dc.Remove("TOTAL_RECORD_PAGES");
                }
                if (dc.Contains("TOTAL_RECORDS"))
                {
                    dc.Remove("TOTAL_RECORDS");
                }
                if (dc.Contains("PT_PROVIDER_ID"))
                {
                    dc.Remove("PT_PROVIDER_ID");
                }
                if (dc.Contains("OT_PROVIDER_ID"))
                {
                    dc.Remove("OT_PROVIDER_ID");
                }
                if (dc.Contains("ST_PROVIDER_ID"))
                {
                    dc.Remove("ST_PROVIDER_ID");
                }
                if (dc.Contains("EP_PROVIDER_ID"))
                {
                    dc.Remove("EP_PROVIDER_ID");
                }
                if (dc.Contains("REGION_CODE"))
                {
                    dc.Remove("REGION_CODE");
                }
                if (dc.Contains("REGION_NAME"))
                {
                    dc.Remove("REGION_NAME");
                }
                if (dc.Contains("PROVIDER_NAME"))
                {
                    dc.Remove("PROVIDER_NAME");
                }
                if (dc.Contains("CONTRACTED_QUOTA"))
                {
                    dc.Remove("CONTRACTED_QUOTA");
                }
                if (dc.Contains("EMPLOYEE_ID"))
                {
                    dc.Remove("EMPLOYEE_ID");
                }
                if (dc.Contains("ASHA_ID"))
                {
                    dc.Remove("ASHA_ID");
                }
                if (dc.Contains("UBER"))
                {
                    dc.Remove("UBER");
                }
                if (dc.Contains("CONTRACT"))
                {
                    dc.Remove("CONTRACT");
                }
                if (dc.Contains("HOT_SYNC_ENABLED"))
                {
                    dc.Remove("HOT_SYNC_ENABLED");
                }
                if (dc.Contains("HOT_SYNC_AUTO_LOCK"))
                {
                    dc.Remove("HOT_SYNC_AUTO_LOCK");
                }
                if (dc.Contains("LAST_LOGIN"))
                {
                    dc.Remove("LAST_LOGIN");
                }
                if (dc.Contains("LAST_LOGIN_DATE"))
                {
                    dc.Remove("LAST_LOGIN_DATE");
                }
                if (dc.Contains("PROGRAM"))
                {
                    dc.Remove("PROGRAM");
                }
                if (dc.Contains("MI"))
                {
                    dc.Remove("MI");
                }
                if (dc.Contains("CITY"))
                {
                    dc.Remove("CITY");
                }
                if (dc.Contains("ZIP"))
                {
                    dc.Remove("ZIP");
                }
                if (dc.Contains("PHONE"))
                {
                    dc.Remove("PHONE");
                }
                if (dc.Contains("FAX"))
                {
                    dc.Remove("FAX");
                }
                //if (dc.Contains("EMAIL"))
                //{
                //    dc.Remove("EMAIL");
                //}
                if (dc.Contains("GROUP_NPI"))
                {
                    dc.Remove("GROUP_NPI");
                }
                if (dc.Contains("LICENSE_NO"))
                {
                    dc.Remove("LICENSE_NO");
                }
                if (dc.Contains("TAX_ID"))
                {
                    dc.Remove("TAX_ID");
                }
                if (dc.Contains("GROUP_TAXONOMY_CODE"))
                {
                    dc.Remove("GROUP_TAXONOMY_CODE");
                }
                if (dc.Contains("HOLD_BILLING_UNTIL_DOS"))
                {
                    dc.Remove("HOLD_BILLING_UNTIL_DOS");
                }
                if (dc.Contains("JOB_DESC"))
                {
                    dc.Remove("JOB_DESC");
                }

            }
            if (CalledFrom.Equals("Reconciliation"))
            {
                if (dc.Contains("RECONCILIATION_CP_ID"))
                {
                    dc.Remove("RECONCILIATION_CP_ID");
                }
                if (dc.Contains("PRACTICE_CODE"))
                {
                    dc.Remove("PRACTICE_CODE");
                }
                if (dc.Contains("FOX_TBL_INSURANCE_ID"))
                {
                    dc.Remove("FOX_TBL_INSURANCE_ID");
                }
                if (dc.Contains("DEPOSIT_TYPE_ID"))
                {
                    dc.Remove("DEPOSIT_TYPE_ID");
                }
                if (dc.Contains("CATEGORY_ID"))
                {
                    dc.Remove("CATEGORY_ID");
                }
                if (dc.Contains("RECONCILIATION_STATUS_ID"))
                {
                    dc.Remove("RECONCILIATION_STATUS_ID");
                }
                if (dc.Contains("Assign_To"))
                {
                    dc.Remove("Assign_To");
                }
                if (dc.Contains("LEDGER_PATH"))
                {
                    dc.Remove("LEDGER_PATH");
                }
                if (dc.Contains("DELETED"))
                {
                    dc.Remove("DELETED");
                }
                if (dc.Contains("LEDGER_BASE64"))
                {
                    dc.Remove("LEDGER_BASE64");
                }
                if (dc.Contains("TOTAL_LEDGER_PAGES"))
                {
                    dc.Remove("TOTAL_LEDGER_PAGES");
                }
                if (dc.Contains("DEPOSIT_DATE_STR"))
                {
                    dc.Remove("DEPOSIT_DATE_STR");
                }
                if (dc.Contains("ASSIGNED_DATE_STR"))
                {
                    dc.Remove("ASSIGNED_DATE_STR");
                }
                if (dc.Contains("COMPLETED_DATE_STR"))
                {
                    dc.Remove("COMPLETED_DATE_STR");
                }
                if (dc.Contains("TOTAL_RECORDS"))
                {
                    dc.Remove("TOTAL_RECORDS");
                }
                if (dc.Contains("TOTAL_RECORD_PAGES"))
                {
                    dc.Remove("TOTAL_RECORD_PAGES");
                }
                if (dc.Contains("CREATED_BY"))
                {
                    dc.Remove("CREATED_BY");
                }
                if (dc.Contains("CREATED_DATE"))
                {
                    dc.Remove("CREATED_DATE");
                }
                if (dc.Contains("MODIFIED_BY"))
                {
                    dc.Remove("MODIFIED_BY");
                }
                if (dc.Contains("MODIFIED_DATE"))
                {
                    dc.Remove("MODIFIED_DATE");
                }
                if (dc.Contains("TOTAL_AMOUNT"))
                {
                    dc.Remove("TOTAL_AMOUNT");
                }
                if (dc.Contains("TOTAL_POSTED_AMOUNT"))
                {
                    dc.Remove("TOTAL_POSTED_AMOUNT");
                }
                if (dc.Contains("TOTAL_UNPOSTED_AMOUNT"))
                {
                    dc.Remove("TOTAL_UNPOSTED_AMOUNT");
                }
                if (dc.Contains("CREATED_BY_NAME"))
                {
                    dc.Remove("CREATED_BY_NAME");
                }
                if (dc.Contains("Assign_To"))
                {
                    dc.Remove("Assign_To");
                }
                if (dc.Contains("COMPLETED_BY"))
                {
                    dc.Remove("COMPLETED_BY");
                }
                if (dc.Contains("COMPLETED_BY_NAME"))
                {
                    dc.Remove("COMPLETED_BY_NAME");
                }
                if (dc.Contains("ASSIGNED_TO"))
                {
                    dc.Remove("ASSIGNED_TO");
                }

                //    if (CalledFrom.Equals("PHD_Call"))
                //    {
                //        if (dc.Contains("FOX_PHD_CALL_DETAILS_ID"))
                //        {
                //            dc.Remove("FOX_PHD_CALL_DETAILS_ID");
                //        }
                //        if (dc.Contains("PATIENT_ACCOUNT"))
                //        {
                //            dc.Remove("PATIENT_ACCOUNT");
                //        }
                //    }
                //}
                //if (CalledFrom.Equals("AgingReport"))
                //{
                //    if (dc.Contains("PRACTICE_CODE"))
                //    {
                //        dc.Remove("PRACTICE_CODE");
                //    }
                //    if (dc.Contains("totalPages"))
                //    {
                //        dc.Remove("totalPages");
                //    }
                //    if (dc.Contains("TotalRecords"))
                //    {
                //        dc.Remove("TotalRecords");
                //    }
            }
            if (CalledFrom.Equals("Contact_Type"))
            {
                if (dc.Contains("Contact_Type_ID"))
                {
                    dc.Remove("Contact_Type_ID");
                }
                if (dc.Contains("Practice_Code"))
                {
                    dc.Remove("Practice_Code");
                }
                if (dc.Contains("Display_Order"))
                {
                    dc.Remove("Display_Order");
                }
                if (dc.Contains("TOTAL_RECORDS"))
                {
                    dc.Remove("TOTAL_RECORDS");
                }
                if (dc.Contains("TOTAL_RECORD_PAGES"))
                {
                    dc.Remove("TOTAL_RECORD_PAGES");
                }
                if (dc.Contains("Created_By"))
                {
                    dc.Remove("Created_By");
                }
                if (dc.Contains("Deleted"))
                {
                    dc.Remove("Deleted");
                }
                if (dc.Contains("IS_ACTIVE"))
                {
                    dc.Remove("IS_ACTIVE");
                }
                if (dc.Contains("Created_On"))
                {
                    dc.Remove("Created_On");
                }
                if (dc.Contains("Modified_By"))
                {
                    dc.Remove("Modified_By");
                }
                if (dc.Contains("Modified_On"))
                {
                    dc.Remove("Modified_On");
                }

            }
            if (CalledFrom.Equals("Document_Type"))
            {
                if (dc.Contains("DOCUMENT_TYPE_ID"))
                {
                    dc.Remove("DOCUMENT_TYPE_ID");
                }
                if (dc.Contains("IS_ONLINE_ORDER_LIST"))
                {
                    dc.Remove("IS_ONLINE_ORDER_LIST");
                }
                if (dc.Contains("DESCRIPTION"))
                {
                    dc.Remove("DESCRIPTION");
                }
                if (dc.Contains("TOTAL_RECORDS"))
                {
                    dc.Remove("TOTAL_RECORDS");
                }
                if (dc.Contains("TOTAL_RECORD_PAGES"))
                {
                    dc.Remove("TOTAL_RECORD_PAGES");
                }
                if (dc.Contains("CREATED_BY"))
                {
                    dc.Remove("CREATED_BY");
                }
                if (dc.Contains("DELETED"))
                {
                    dc.Remove("DELETED");
                }
                if (dc.Contains("IS_ACTIVE"))
                {
                    dc.Remove("IS_ACTIVE");
                }
                if (dc.Contains("CREATED_DATE"))
                {
                    dc.Remove("CREATED_DATE");
                }
                if (dc.Contains("MODIFIED_BY"))
                {
                    dc.Remove("MODIFIED_BY");
                }
                if (dc.Contains("MODIFIED_DATE"))
                {
                    dc.Remove("MODIFIED_DATE");
                }

            }
            if (CalledFrom.Equals("Alert_Type"))
            {
                if (dc.Contains("ALERT_TYPE_ID"))
                {
                    dc.Remove("ALERT_TYPE_ID");
                }
                if (dc.Contains("PRACTICE_CODE"))
                {
                    dc.Remove("PRACTICE_CODE");
                }
                if (dc.Contains("TOTAL_RECORDS"))
                {
                    dc.Remove("TOTAL_RECORDS");
                }
                if (dc.Contains("TOTAL_RECORD_PAGES"))
                {
                    dc.Remove("TOTAL_RECORD_PAGES");
                }
                if (dc.Contains("CREATED_BY"))
                {
                    dc.Remove("CREATED_BY");
                }
                if (dc.Contains("DELETED"))
                {
                    dc.Remove("DELETED");
                }
                if (dc.Contains("IS_ACTIVE"))
                {
                    dc.Remove("IS_ACTIVE");
                }
                if (dc.Contains("CREATED_DATE"))
                {
                    dc.Remove("CREATED_DATE");
                }
                if (dc.Contains("MODIFIED_BY"))
                {
                    dc.Remove("MODIFIED_BY");
                }
                if (dc.Contains("MODIFIED_DATE"))
                {
                    dc.Remove("MODIFIED_DATE");
                }

            }
            if (CalledFrom.Equals("Source_of_Referral"))
            {
                if (dc.Contains("SOURCE_OF_REFERRAL_ID"))
                {
                    dc.Remove("SOURCE_OF_REFERRAL_ID");
                }
                if (dc.Contains("PRACTICE_CODE"))
                {
                    dc.Remove("PRACTICE_CODE");
                }
                if (dc.Contains("TOTAL_RECORDS"))
                {
                    dc.Remove("TOTAL_RECORDS");
                }
                if (dc.Contains("TOTAL_RECORD_PAGES"))
                {
                    dc.Remove("TOTAL_RECORD_PAGES");
                }
                if (dc.Contains("CREATED_BY"))
                {
                    dc.Remove("CREATED_BY");
                }
                if (dc.Contains("DELETED"))
                {
                    dc.Remove("DELETED");
                }
                if (dc.Contains("IS_ACTIVE"))
                {
                    dc.Remove("IS_ACTIVE");
                }
                if (dc.Contains("CREATED_DATE"))
                {
                    dc.Remove("CREATED_DATE");
                }
                if (dc.Contains("MODIFIED_BY"))
                {
                    dc.Remove("MODIFIED_BY");
                }
                if (dc.Contains("MODIFIED_DATE"))
                {
                    dc.Remove("MODIFIED_DATE");
                }

            }
            if (CalledFrom.Equals("Order_Status"))
            {
                if (dc.Contains("ORDER_STATUS_ID"))
                {
                    dc.Remove("ORDER_STATUS_ID");
                }
                if (dc.Contains("PRACTICE_CODE"))
                {
                    dc.Remove("PRACTICE_CODE");
                }
                if (dc.Contains("TOTAL_RECORDS"))
                {
                    dc.Remove("TOTAL_RECORDS");
                }
                if (dc.Contains("TOTAL_RECORD_PAGES"))
                {
                    dc.Remove("TOTAL_RECORD_PAGES");
                }
                if (dc.Contains("CREATED_BY"))
                {
                    dc.Remove("CREATED_BY");
                }
                if (dc.Contains("DELETED"))
                {
                    dc.Remove("DELETED");
                }
                if (dc.Contains("IS_ACTIVE"))
                {
                    dc.Remove("IS_ACTIVE");
                }
                if (dc.Contains("CREATED_DATE"))
                {
                    dc.Remove("CREATED_DATE");
                }
                if (dc.Contains("MODIFIED_BY"))
                {
                    dc.Remove("MODIFIED_BY");
                }
                if (dc.Contains("MODIFIED_DATE"))
                {
                    dc.Remove("MODIFIED_DATE");
                }
                if (dc.Contains("DAYS"))
                {
                    dc.Remove("DAYS");
                }

            }
            if (CalledFrom.Equals("Task_Type"))
            {
                if (dc.Contains("TASK_TYPE_ID"))
                {
                    dc.Remove("TASK_TYPE_ID");
                }
                if (dc.Contains("PRACTICE_CODE"))
                {
                    dc.Remove("PRACTICE_CODE");
                }
                if (dc.Contains("CASE_STATUS_ID"))
                {
                    dc.Remove("CASE_STATUS_ID");
                }
                if (dc.Contains("CATEGORY_CODE"))
                {
                    dc.Remove("CATEGORY_CODE");
                }
                if (dc.Contains("DESCRIPTION"))
                {
                    dc.Remove("DESCRIPTION");
                }
                if (dc.Contains("TOTAL_RECORDS"))
                {
                    dc.Remove("TOTAL_RECORDS");
                }
                if (dc.Contains("TOTAL_RECORD_PAGES"))
                {
                    dc.Remove("TOTAL_RECORD_PAGES");
                }
                if (dc.Contains("CREATED_BY"))
                {
                    dc.Remove("CREATED_BY");
                }
                if (dc.Contains("DELETED"))
                {
                    dc.Remove("DELETED");
                }
                if (dc.Contains("IS_ACTIVE"))
                {
                    dc.Remove("IS_ACTIVE");
                }
                if (dc.Contains("CREATED_DATE"))
                {
                    dc.Remove("CREATED_DATE");
                }
                if (dc.Contains("MODIFIED_BY"))
                {
                    dc.Remove("MODIFIED_BY");
                }
                if (dc.Contains("MODIFIED_DATE"))
                {
                    dc.Remove("MODIFIED_DATE");
                }

            }
            if (CalledFrom.Equals("Auth_Status"))
            {
                if (dc.Contains("AUTH_STATUS_ID"))
                {
                    dc.Remove("AUTH_STATUS_ID");
                }
                if (dc.Contains("PRACTICE_CODE"))
                {
                    dc.Remove("PRACTICE_CODE");
                }
                if (dc.Contains("TOTAL_RECORDS"))
                {
                    dc.Remove("TOTAL_RECORDS");
                }
                if (dc.Contains("TOTAL_RECORD_PAGES"))
                {
                    dc.Remove("TOTAL_RECORD_PAGES");
                }
                if (dc.Contains("CREATED_BY"))
                {
                    dc.Remove("CREATED_BY");
                }
                if (dc.Contains("DELETED"))
                {
                    dc.Remove("DELETED");
                }
                if (dc.Contains("IS_ACTIVE"))
                {
                    dc.Remove("IS_ACTIVE");
                }
                if (dc.Contains("CREATED_DATE"))
                {
                    dc.Remove("CREATED_DATE");
                }
                if (dc.Contains("MODIFIED_BY"))
                {
                    dc.Remove("MODIFIED_BY");
                }
                if (dc.Contains("MODIFIED_DATE"))
                {
                    dc.Remove("MODIFIED_DATE");
                }

            }
            if (CalledFrom.Equals("Identifier_Type"))
            {
                if (dc.Contains("IDENTIFIER_ID"))
                {
                    dc.Remove("IDENTIFIER_ID");
                }
                if (dc.Contains("PRACTICE_CODE"))
                {
                    dc.Remove("PRACTICE_CODE");
                }
                if (dc.Contains("IDENTIFIER_TYPE_ID"))
                {
                    dc.Remove("IDENTIFIER_TYPE_ID");
                }
                if (dc.Contains("CODE_NAME"))
                {
                    dc.Remove("CODE_NAME");
                }
                if (dc.Contains("DESCRIPTION"))
                {
                    dc.Remove("DESCRIPTION");
                }
                if (dc.Contains("TOTAL_RECORDS"))
                {
                    dc.Remove("TOTAL_RECORDS");
                }
                if (dc.Contains("TOTAL_RECORD_PAGES"))
                {
                    dc.Remove("TOTAL_RECORD_PAGES");
                }
                if (dc.Contains("CREATED_BY"))
                {
                    dc.Remove("CREATED_BY");
                }
                if (dc.Contains("DELETED"))
                {
                    dc.Remove("DELETED");
                }
                if (dc.Contains("IS_ACTIVE"))
                {
                    dc.Remove("IS_ACTIVE");
                }
                if (dc.Contains("CREATED_DATE"))
                {
                    dc.Remove("CREATED_DATE");
                }
                if (dc.Contains("MODIFIED_BY"))
                {
                    dc.Remove("MODIFIED_BY");
                }
                if (dc.Contains("MODIFIED_DATE"))
                {
                    dc.Remove("MODIFIED_DATE");
                }

            }
            if (CalledFrom.Equals("Location_Corporation"))
            {
                if (dc.Contains("LOCATION_CORPORATION_ID"))
                {
                    dc.Remove("LOCATION_CORPORATION_ID");
                }
                if (dc.Contains("PRACTICE_CODE"))
                {
                    dc.Remove("PRACTICE_CODE");
                }
                if (dc.Contains("TOTAL_RECORDS"))
                {
                    dc.Remove("TOTAL_RECORDS");
                }
                if (dc.Contains("TOTAL_RECORD_PAGES"))
                {
                    dc.Remove("TOTAL_RECORD_PAGES");
                }
                if (dc.Contains("CREATED_BY"))
                {
                    dc.Remove("CREATED_BY");
                }
                if (dc.Contains("DELETED"))
                {
                    dc.Remove("DELETED");
                }
                if (dc.Contains("IS_ACTIVE"))
                {
                    dc.Remove("IS_ACTIVE");
                }
                if (dc.Contains("CREATED_DATE"))
                {
                    dc.Remove("CREATED_DATE");
                }
                if (dc.Contains("MODIFIED_BY"))
                {
                    dc.Remove("MODIFIED_BY");
                }
                if (dc.Contains("MODIFIED_DATE"))
                {
                    dc.Remove("MODIFIED_DATE");
                }
                if (dc.Contains("START_DATE"))
                {
                    dc.Remove("START_DATE");
                }
                if (dc.Contains("END_DATE"))
                {
                    dc.Remove("END_DATE");
                }

            }
            if (CalledFrom.Equals("Group_Identifier"))
            {
                if (dc.Contains("GROUP_IDENTIFIER_ID"))
                {
                    dc.Remove("GROUP_IDENTIFIER_ID");
                }
                if (dc.Contains("PRACTICE_CODE"))
                {
                    dc.Remove("PRACTICE_CODE");
                }
                if (dc.Contains("TOTAL_RECORDS"))
                {
                    dc.Remove("TOTAL_RECORDS");
                }
                if (dc.Contains("TOTAL_RECORD_PAGES"))
                {
                    dc.Remove("TOTAL_RECORD_PAGES");
                }
                if (dc.Contains("CREATED_BY"))
                {
                    dc.Remove("CREATED_BY");
                }
                if (dc.Contains("DELETED"))
                {
                    dc.Remove("DELETED");
                }
                if (dc.Contains("IS_ACTIVE"))
                {
                    dc.Remove("IS_ACTIVE");
                }
                if (dc.Contains("CREATED_DATE"))
                {
                    dc.Remove("CREATED_DATE");
                }
                if (dc.Contains("MODIFIED_BY"))
                {
                    dc.Remove("MODIFIED_BY");
                }
                if (dc.Contains("MODIFIED_DATE"))
                {
                    dc.Remove("MODIFIED_DATE");
                }
            }
            if (CalledFrom.Equals("Facility_Setup"))
            {
                if (dc.Contains("LOC_ID"))
                {
                    dc.Remove("LOC_ID");
                }
                if (dc.Contains("PRACTICE_CODE"))
                {
                    dc.Remove("PRACTICE_CODE");
                }
                if (dc.Contains("Address"))
                {
                    dc.Remove("Address");
                }
                if (dc.Contains("REGION_NAME"))
                {
                    dc.Remove("REGION_NAME");
                }
                if (dc.Contains("PhoneFormat"))
                {
                    dc.Remove("PhoneFormat");
                }
                if (dc.Contains("FaxFormat"))
                {
                    dc.Remove("FaxFormat");
                }
                if (dc.Contains("FACILITY_TYPE_ID"))
                {
                    dc.Remove("FACILITY_TYPE_ID");
                }
                if (dc.Contains("Last_Update"))
                {
                    dc.Remove("Last_Update");
                }
                if (dc.Contains("CREATED_DATE_STRING"))
                {
                    dc.Remove("CREATED_DATE_STRING");
                }
                if (dc.Contains("MODIFIED_DATE_STRING"))
                {
                    dc.Remove("MODIFIED_DATE_STRING");
                }
                if (dc.Contains("FACILITY_TYPE_NAME"))
                {
                    dc.Remove("FACILITY_TYPE_NAME");
                }
                if (dc.Contains("Work_Phone"))
                {
                    dc.Remove("Work_Phone");
                }
                if (dc.Contains("Cell_Phone"))
                {
                    dc.Remove("Cell_Phone");
                }
                if (dc.Contains("Email_Address"))
                {
                    dc.Remove("Email_Address");
                }
                if (dc.Contains("AddressType"))
                {
                    dc.Remove("AddressType");
                }
                if (dc.Contains("PT_PROVIDER_ID"))
                {
                    dc.Remove("PT_PROVIDER_ID");
                }
                if (dc.Contains("EP_PROVIDER_ID"))
                {
                    dc.Remove("EP_PROVIDER_ID");
                }
                if (dc.Contains("OT_PROVIDER_ID"))
                {
                    dc.Remove("OT_PROVIDER_ID");
                }
                if (dc.Contains("ST_PROVIDER_ID"))
                {
                    dc.Remove("ST_PROVIDER_ID");
                }
                if (dc.Contains("City"))
                {
                    dc.Remove("City");
                }
                if (dc.Contains("State"))
                {
                    dc.Remove("State");
                }
                if (dc.Contains("Zip"))
                {
                    dc.Remove("Zip");
                }
                if (dc.Contains("LEAD_PROVIDER_ID"))
                {
                    dc.Remove("LEAD_PROVIDER_ID");
                }
                if (dc.Contains("PATIENT_POS_ID"))
                {
                    dc.Remove("PATIENT_POS_ID");
                }
                if (dc.Contains("Is_Void"))
                {
                    dc.Remove("Is_Void");
                }
                if (dc.Contains("Is_Default"))
                {
                    dc.Remove("Is_Default");
                }
                if (dc.Contains("Effective_From"))
                {
                    dc.Remove("Effective_From");
                }
                if (dc.Contains("Effective_To"))
                {
                    dc.Remove("Effective_To");
                }
                if (dc.Contains("TOTAL_RECORDS"))
                {
                    dc.Remove("TOTAL_RECORDS");
                }
                if (dc.Contains("TOTAL_RECORD_PAGES"))
                {
                    dc.Remove("TOTAL_RECORD_PAGES");
                }
                if (dc.Contains("CREATED_BY"))
                {
                    dc.Remove("CREATED_BY");
                }
                if (dc.Contains("DELETED"))
                {
                    dc.Remove("DELETED");
                }
                if (dc.Contains("IS_ACTIVE"))
                {
                    dc.Remove("IS_ACTIVE");
                }
                if (dc.Contains("CREATED_DATE"))
                {
                    dc.Remove("CREATED_DATE");
                }
                if (dc.Contains("MODIFIED_BY"))
                {
                    dc.Remove("MODIFIED_BY");
                }
                if (dc.Contains("MODIFIED_DATE"))
                {
                    dc.Remove("MODIFIED_DATE");
                }
                if (dc.Contains("Longitude"))
                {
                    dc.Remove("Longitude");
                }
                if (dc.Contains("Latitude"))
                {
                    dc.Remove("Latitude");
                }

            }
            if (CalledFrom.Equals("Referral_Region"))
            {
                if (dc.Contains("REFERRAL_REGION_ID"))
                {
                    dc.Remove("REFERRAL_REGION_ID");
                }
                if (dc.Contains("PRACTICE_CODE"))
                {
                    dc.Remove("PRACTICE_CODE");
                }
                if (dc.Contains("ZipStateCountyList"))
                {
                    dc.Remove("ZipStateCountyList");
                }
                if (dc.Contains("REFERRAL_REGION_NAME_Title"))
                {
                    dc.Remove("REFERRAL_REGION_NAME_Title");
                }
                if (dc.Contains("ACCOUNT_MANAGER_Title"))
                {
                    dc.Remove("ACCOUNT_MANAGER_Title");
                }
                if (dc.Contains("IS_INACTIVE"))
                {
                    dc.Remove("IS_INACTIVE");
                }
                if (dc.Contains("ALT_REGIONAL_DIRECTOR_NAME"))
                {
                    dc.Remove("ALT_REGIONAL_DIRECTOR_NAME");
                }
                if (dc.Contains("COUNTIES"))
                {
                    dc.Remove("COUNTIES");
                }
                if (dc.Contains("REGIONAL_DIRECTOR_NAME"))
                {
                    dc.Remove("REGIONAL_DIRECTOR_NAME");
                }
                if (dc.Contains("SENIOR_REGIONAL_DIRECTOR_NAME"))
                {
                    dc.Remove("SENIOR_REGIONAL_DIRECTOR_NAME");
                }
                if (dc.Contains("SENIOR_REGIONAL_DIRECTOR_ID"))
                {
                    dc.Remove("SENIOR_REGIONAL_DIRECTOR_ID");
                }
                if (dc.Contains("ACCOUNT_MANAGER_EMAIL"))
                {
                    dc.Remove("ACCOUNT_MANAGER_EMAIL");
                }
                if (dc.Contains("START_DATE"))
                {
                    dc.Remove("START_DATE");
                }
                if (dc.Contains("END_DATE"))
                {
                    dc.Remove("END_DATE");
                }
                if (dc.Contains("STATE_CODE"))
                {
                    dc.Remove("STATE_CODE");
                }
                if (dc.Contains("REGIONAL_DIRECTOR_ID"))
                {
                    dc.Remove("REGIONAL_DIRECTOR_ID");
                }
                if (dc.Contains("Name"))
                {
                    dc.Remove("Name");
                }
                if (dc.Contains("Template"))
                {
                    dc.Remove("Template");
                }
                if (dc.Contains("IN_ACTIVEDATE_Str"))
                {
                    dc.Remove("IN_ACTIVEDATE_Str");
                }
                if (dc.Contains("ALTERNATE_REGION_ID"))
                {
                    dc.Remove("ALTERNATE_REGION_ID");
                }
                if (dc.Contains("IN_ACTIVEDATE"))
                {
                    dc.Remove("IN_ACTIVEDATE");
                }
                if (dc.Contains("TOTAL_RECORDS"))
                {
                    dc.Remove("TOTAL_RECORDS");
                }
                if (dc.Contains("TOTAL_RECORD_PAGES"))
                {
                    dc.Remove("TOTAL_RECORD_PAGES");
                }
                if (dc.Contains("CREATED_BY"))
                {
                    dc.Remove("CREATED_BY");
                }
                if (dc.Contains("Created_Date_Str"))
                {
                    dc.Remove("Created_Date_Str");
                }
                if (dc.Contains("Modified_Date_Str"))
                {
                    dc.Remove("Modified_Date_Str");
                }
                if (dc.Contains("DELETED"))
                {
                    dc.Remove("DELETED");
                }
                if (dc.Contains("IS_ACTIVE"))
                {
                    dc.Remove("IS_ACTIVE");
                }
                if (dc.Contains("CREATED_DATE"))
                {
                    dc.Remove("CREATED_DATE");
                }
                if (dc.Contains("MODIFIED_BY"))
                {
                    dc.Remove("MODIFIED_BY");
                }
                if (dc.Contains("MODIFIED_DATE"))
                {
                    dc.Remove("MODIFIED_DATE");
                }
                if (dc.Contains("ACCOUNT_MANAGER_ID"))
                {
                    dc.Remove("ACCOUNT_MANAGER_ID");
                }
                if (dc.Contains("ACCOUNT_MANAGER_NAME"))
                {
                    dc.Remove("ACCOUNT_MANAGER_NAME");
                }
                if (dc.Contains("Dashboard_Access"))
                {
                    dc.Remove("Dashboard_Access");
                }
                if (dc.Contains("SENIOR_REGIONAL_DIRECTOR_NAME"))
                {
                    dc.Remove("SENIOR_REGIONAL_DIRECTOR_NAME");
                }
                if (dc.Contains("REGIONAL_DIRECTOR_NAME"))
                {
                    dc.Remove("REGIONAL_DIRECTOR_NAME");
                }

            }
            if (CalledFrom.Equals("Referral_Source"))
            {
                if (dc.Contains("SOURCE_ID"))
                {
                    dc.Remove("SOURCE_ID");
                }
                if (dc.Contains("PRACTICE_CODE"))
                {
                    dc.Remove("PRACTICE_CODE");
                }
                if (dc.Contains("FIRST_NAME_Title"))
                {
                    dc.Remove("FIRST_NAME_Title");
                }
                if (dc.Contains("LAST_NAME_Title"))
                {
                    dc.Remove("LAST_NAME_Title");
                }
                if (dc.Contains("PHONEFormat"))
                {
                    dc.Remove("PHONEFormat");
                }
                if (dc.Contains("FaxFormat"))
                {
                    dc.Remove("FaxFormat");
                }
                if (dc.Contains("REFERRAL_REGION_TITLE"))
                {
                    dc.Remove("REFERRAL_REGION_TITLE");
                }
                if (dc.Contains("ACO_TITLE"))
                {
                    dc.Remove("ACO_TITLE");
                }
                if (dc.Contains("ORGANIZATION_TITLE"))
                {
                    dc.Remove("ORGANIZATION_TITLE");
                }
                if (dc.Contains("ORGANIZATION"))
                {
                    dc.Remove("ORGANIZATION");
                }
                if (dc.Contains("PRACTICE_ORGANIZATION_ID"))
                {
                    dc.Remove("PRACTICE_ORGANIZATION_ID");
                }
                if (dc.Contains("INACTIVE_REASON_ID"))
                {
                    dc.Remove("INACTIVE_REASON_ID");
                }
                if (dc.Contains("INACTIVE_DATE"))
                {
                    dc.Remove("INACTIVE_DATE");
                }
                if (dc.Contains("INACTIVE_DATE_STR"))
                {
                    dc.Remove("INACTIVE_DATE_STR");
                }
                if (dc.Contains("SOURCE_DELIVERY_METHOD_ID"))
                {
                    dc.Remove("SOURCE_DELIVERY_METHOD_ID");
                }
                if (dc.Contains("SOURCE_DELIVERY_METHOD_NAME"))
                {
                    dc.Remove("SOURCE_DELIVERY_METHOD_NAME");
                }
                if (dc.Contains("NOTES"))
                {
                    dc.Remove("NOTES");
                }
                if (dc.Contains("TOTAL_RECORDS"))
                {
                    dc.Remove("TOTAL_RECORDS");
                }
                if (dc.Contains("TOTAL_RECORD_PAGES"))
                {
                    dc.Remove("TOTAL_RECORD_PAGES");
                }
                if (dc.Contains("CREATED_BY"))
                {
                    dc.Remove("CREATED_BY");
                }
                if (dc.Contains("CREATED_BY_Title"))
                {
                    dc.Remove("CREATED_BY_Title");
                }
                if (dc.Contains("MODIFIED_BY_Title"))
                {
                    dc.Remove("MODIFIED_BY_Title");
                }
                if (dc.Contains("DELETED"))
                {
                    dc.Remove("DELETED");
                }
                if (dc.Contains("Template"))
                {
                    dc.Remove("Template");
                }
                if (dc.Contains("REFERRAL_REGION_NAME"))
                {
                    dc.Remove("REFERRAL_REGION_NAME");
                }
                if (dc.Contains("ACO"))
                {
                    dc.Remove("ACO");
                }
                if (dc.Contains("ACO_ID"))
                {
                    dc.Remove("ACO_ID");
                }
                if (dc.Contains("MODIFIED_DATE"))
                {
                    dc.Remove("MODIFIED_DATE");
                }
                if (dc.Contains("Practice_Name"))
                {
                    dc.Remove("Practice_Name");
                }
                if (dc.Contains("REFERRAL_CODE"))
                {
                    dc.Remove("REFERRAL_CODE");
                }
                if (dc.Contains("CREATED_DATE"))
                {
                    dc.Remove("CREATED_DATE");
                }
                if (dc.Contains("MODIFIED_BY"))
                {
                    dc.Remove("MODIFIED_BY");
                }

            }
            if (CalledFrom.Equals("Original_Queue"))
            {
                if (dc.Contains("WORK_ID"))
                {
                    dc.Remove("WORK_ID");
                }
                if (dc.Contains("PRACTICE_CODE"))
                {
                    dc.Remove("PRACTICE_CODE");
                }
                if (dc.Contains("UNIQUE_ID"))
                {
                    dc.Remove("UNIQUE_ID");
                }
                if (dc.Contains("PATIENT_ACCOUNT"))
                {
                    dc.Remove("PATIENT_ACCOUNT");
                }
                if (dc.Contains("SORCE_NAME_FaxFormat"))
                {
                    dc.Remove("SORCE_NAME_FaxFormat");
                }
                if (dc.Contains("RECEIVE_DATE"))
                {
                    dc.Remove("RECEIVE_DATE");
                }
                if (dc.Contains("FILE_PATH"))
                {
                    dc.Remove("FILE_PATH");
                }
                if (dc.Contains("FILE_PATH_LOGO"))
                {
                    dc.Remove("FILE_PATH_LOGO");
                }
                if (dc.Contains("ASSIGNED_TO"))
                {
                    dc.Remove("ASSIGNED_TO");
                }
                if (dc.Contains("ASSIGNED_BY"))
                {
                    dc.Remove("ASSIGNED_BY");
                }
                if (dc.Contains("ASSIGNED_DATE"))
                {
                    dc.Remove("ASSIGNED_DATE");
                }
                if (dc.Contains("COMPLETED_BY"))
                {
                    dc.Remove("COMPLETED_BY");
                }
                if (dc.Contains("COMPLETED_DATE"))
                {
                    dc.Remove("COMPLETED_DATE");
                }
                if (dc.Contains("DOCUMENT_TYPE"))
                {
                    dc.Remove("DOCUMENT_TYPE");
                }
                if (dc.Contains("SENDER_ID"))
                {
                    dc.Remove("SENDER_ID");
                }
                if (dc.Contains("FACILITY_NAME"))
                {
                    dc.Remove("FACILITY_NAME");
                }
                if (dc.Contains("FACILITY_ID"))
                {
                    dc.Remove("FACILITY_ID");
                }
                if (dc.Contains("DEPARTMENT_ID"))
                {
                    dc.Remove("DEPARTMENT_ID");
                }
                if (dc.Contains("IS_EMERGENCY_ORDER"))
                {
                    dc.Remove("IS_EMERGENCY_ORDER");
                }
                if (dc.Contains("REASON_FOR_VISIT"))
                {
                    dc.Remove("REASON_FOR_VISIT");
                }
                if (dc.Contains("ACCOUNT_NUMBER"))
                {
                    dc.Remove("ACCOUNT_NUMBER");
                }
                if (dc.Contains("UNIT_CASE_NO"))
                {
                    dc.Remove("UNIT_CASE_NO");
                }
                if (dc.Contains("CREATED_BY"))
                {
                    dc.Remove("CREATED_BY");
                }
                if (dc.Contains("CREATED_DATE"))
                {
                    dc.Remove("CREATED_DATE");
                }
                if (dc.Contains("MODIFIED_BY"))
                {
                    dc.Remove("MODIFIED_BY");
                }
                if (dc.Contains("MODIFIED_DATE"))
                {
                    dc.Remove("MODIFIED_DATE");
                }
                if (dc.Contains("DELETED"))
                {
                    dc.Remove("DELETED");
                }
                if (dc.Contains("TOTAL_ROCORD_PAGES"))
                {
                    dc.Remove("TOTAL_ROCORD_PAGES");
                }
                if (dc.Contains("IsCompleted"))
                {
                    dc.Remove("IsCompleted");
                }
                if (dc.Contains("IsSaved"))
                {
                    dc.Remove("IsSaved");
                }
                if (dc.Contains("INDEXED_BY"))
                {
                    dc.Remove("INDEXED_BY");
                }
                if (dc.Contains("INDEXED_DATE"))
                {
                    dc.Remove("INDEXED_DATE");
                }
                if (dc.Contains("TOTAL_RECORDS"))
                {
                    dc.Remove("TOTAL_RECORDS");
                }
                if (dc.Contains("FAX_ID"))
                {
                    dc.Remove("FAX_ID");
                }
                if (dc.Contains("supervisor_status"))
                {
                    dc.Remove("supervisor_status");
                }
                if (dc.Contains("INDEXER_ASSIGN_DATE"))
                {
                    dc.Remove("INDEXER_ASSIGN_DATE");
                }
                if (dc.Contains("AGENT_ASSIGN_DATE"))
                {
                    dc.Remove("AGENT_ASSIGN_DATE");
                }
                if (dc.Contains("IS_VERIFIED_BY_RECIPIENT"))
                {
                    dc.Remove("IS_VERIFIED_BY_RECIPIENT");
                }
                if (dc.Contains("IsSigned"))
                {
                    dc.Remove("IsSigned");
                }
                if (dc.Contains("SignedBy"))
                {
                    dc.Remove("SignedBy");
                }
                if (dc.Contains("FOX_TBL_SENDER_TYPE_ID"))
                {
                    dc.Remove("FOX_TBL_SENDER_TYPE_ID");
                }
                if (dc.Contains("FOX_TBL_SENDER_NAME_ID"))
                {
                    dc.Remove("FOX_TBL_SENDER_NAME_ID");
                }
                if (dc.Contains("IS_UNSIGNED"))
                {
                    dc.Remove("IS_UNSIGNED");
                }
                if (dc.Contains("GuestID"))
                {
                    dc.Remove("GuestID");
                }
                if (dc.Contains("REASON_FOR_THE_URGENCY"))
                {
                    dc.Remove("REASON_FOR_THE_URGENCY");
                }
                if (dc.Contains("IS_POST_ACUTE"))
                {
                    dc.Remove("IS_POST_ACUTE");
                }
                if (dc.Contains("IsRequestForOrder"))
                {
                    dc.Remove("IsRequestForOrder");
                }
                if (dc.Contains("SPECIALITY_PROGRAM"))
                {
                    dc.Remove("SPECIALITY_PROGRAM");
                }
                if (dc.Contains("IS_EVALUATE_TREAT"))
                {
                    dc.Remove("IS_EVALUATE_TREAT");
                }
                if (dc.Contains("HEALTH_NAME"))
                {
                    dc.Remove("HEALTH_NAME");
                }
                if (dc.Contains("HEALTH_NUMBER"))
                {
                    dc.Remove("HEALTH_NUMBER");
                }
                if (dc.Contains("IS_VERBAL_ORDER"))
                {
                    dc.Remove("IS_VERBAL_ORDER");
                }
                if (dc.Contains("VO_ON_BEHALF_OF"))
                {
                    dc.Remove("VO_ON_BEHALF_OF");
                }
                if (dc.Contains("VO_RECIEVED_BY"))
                {
                    dc.Remove("VO_RECIEVED_BY");
                }
                if (dc.Contains("VO_DATE_TIME"))
                {
                    dc.Remove("VO_DATE_TIME");
                }
                if (dc.Contains("VO_DATE_TIME_STR"))
                {
                    dc.Remove("VO_DATE_TIME_STR");
                }
                if (dc.Contains("TRANSFER_DATE"))
                {
                    dc.Remove("TRANSFER_DATE");
                }
                if (dc.Contains("DIAGNOSIS"))
                {
                    dc.Remove("DIAGNOSIS");
                }
                if (dc.Contains("CURRENT_DATE_STR"))
                {
                    dc.Remove("CURRENT_DATE_STR");
                }
                if (dc.Contains("Patient_Account_Str"))
                {
                    dc.Remove("Patient_Account_Str");
                }
                if (dc.Contains("IS_TRASH_REFERRAL"))
                {
                    dc.Remove("IS_TRASH_REFERRAL");
                }
                if (dc.Contains("is_strategic_account"))
                {
                    dc.Remove("is_strategic_account");
                }
                if (dc.Contains("IS_STRATEGIC"))
                {
                    dc.Remove("IS_STRATEGIC");
                }
                if (dc.Contains("RFO_Type"))
                {
                    dc.Remove("RFO_Type");
                }
                if (dc.Contains("Is_Manual_ORS"))
                {
                    dc.Remove("Is_Manual_ORS");
                }
                if (dc.Contains("ORS_NAME"))
                {
                    dc.Remove("ORS_NAME");
                }
                if (dc.Contains("ORS_PHONE"))
                {
                    dc.Remove("ORS_PHONE");
                }
                if (dc.Contains("ORS_FAX"))
                {
                    dc.Remove("ORS_FAX");
                }
                if (dc.Contains("ORS_NPI"))
                {
                    dc.Remove("ORS_NPI");
                }
                if (dc.Contains("OCR_STATUS_ID"))
                {
                    dc.Remove("OCR_STATUS_ID");
                }
                if (dc.Contains("OCR_STATUS"))
                {
                    dc.Remove("OCR_STATUS");
                }
            }
            if (CalledFrom.Equals("Unassigned_Queue"))
            {
                if (dc.Contains("WORK_ID"))
                {
                    dc.Remove("WORK_ID");
                }
                if (dc.Contains("PRACTICE_CODE"))
                {
                    dc.Remove("PRACTICE_CODE");
                }
                if (dc.Contains("PATIENT_ACCOUNT"))
                {
                    dc.Remove("PATIENT_ACCOUNT");
                }
                if (dc.Contains("SORCE_NAMEFormat"))
                {
                    dc.Remove("SORCE_NAMEFormat");
                }
                if (dc.Contains("WORK_STATUS"))
                {
                    dc.Remove("WORK_STATUS");
                }
                if (dc.Contains("RECEIVE_DATE"))
                {
                    dc.Remove("RECEIVE_DATE");
                }
                if (dc.Contains("NO_OF_SPLITS"))
                {
                    dc.Remove("NO_OF_SPLITS");
                }
                if (dc.Contains("FILE_PATH"))
                {
                    dc.Remove("FILE_PATH");
                }
                if (dc.Contains("ASSIGNED_TO"))
                {
                    dc.Remove("ASSIGNED_TO");
                }
                if (dc.Contains("ASSIGNED_BY"))
                {
                    dc.Remove("ASSIGNED_BY");
                }
                if (dc.Contains("ASSIGNED_DATE"))
                {
                    dc.Remove("ASSIGNED_DATE");
                }
                if (dc.Contains("TOTAL_ROCORD_PAGES"))
                {
                    dc.Remove("TOTAL_ROCORD_PAGES");
                }
                if (dc.Contains("IS_EMRERGENCY"))
                {
                    dc.Remove("IS_EMRERGENCY");
                }
                if (dc.Contains("TOTAL_RECORDS"))
                {
                    dc.Remove("TOTAL_RECORDS");
                }
                if (dc.Contains("IS_UNSIGNED"))
                {
                    dc.Remove("IS_UNSIGNED");
                }
                if (dc.Contains("IS_STRATEGIC"))
                {
                    dc.Remove("IS_STRATEGIC");
                }
                if (dc.Contains("OCR_STATUS_ID"))
                {
                    dc.Remove("OCR_STATUS_ID");
                }
                if (dc.Contains("OCR_STATUS"))
                {
                    dc.Remove("OCR_STATUS");
                }
            }
            if (CalledFrom.Equals("Assigned_Queue"))
            {
                if (dc.Contains("WORK_ID"))
                {
                    dc.Remove("WORK_ID");
                }
                if (dc.Contains("SORCE_NAMEFormat"))
                {
                    dc.Remove("SORCE_NAMEFormat");
                }
                if (dc.Contains("RECEIVE_DATE"))
                {
                    dc.Remove("RECEIVE_DATE");
                }
                if (dc.Contains("SORCE_NAMEFormat"))
                {
                    dc.Remove("SORCE_NAMEFormat");
                }
                if (dc.Contains("ASSIGNED_DATE"))
                {
                    dc.Remove("ASSIGNED_DATE");
                }
                if (dc.Contains("MODIFIED_DATE"))
                {
                    dc.Remove("MODIFIED_DATE");
                }
                if (dc.Contains("PATIENT_ACCOUNT"))
                {
                    dc.Remove("PATIENT_ACCOUNT");
                }
                if (dc.Contains("FILE_PATH"))
                {
                    dc.Remove("FILE_PATH");
                }
                if (dc.Contains("PRACTICE_CODE"))
                {
                    dc.Remove("PRACTICE_CODE");
                }
                if (dc.Contains("RE_ASSIGNED_TO"))
                {
                    dc.Remove("RE_ASSIGNED_TO");
                }
                if (dc.Contains("INDEXED_BY"))
                {
                    dc.Remove("INDEXED_BY");
                }
                if (dc.Contains("TOTAL_ROCORD_PAGES"))
                {
                    dc.Remove("TOTAL_ROCORD_PAGES");
                }
                if (dc.Contains("INDEXED_DATE"))
                {
                    dc.Remove("INDEXED_DATE");
                }
                if (dc.Contains("TOTAL_RECORDS"))
                {
                    dc.Remove("TOTAL_RECORDS");
                }
                if (dc.Contains("IS_EMERGENCY"))
                {
                    dc.Remove("IS_EMERGENCY");
                }
                if (dc.Contains("IS_UNSIGNED"))
                {
                    dc.Remove("IS_UNSIGNED");
                }
                if (dc.Contains("IS_TRASH_REFERRAL"))
                {
                    dc.Remove("IS_TRASH_REFERRAL");
                }
                if (dc.Contains("IS_STRATEGIC"))
                {
                    dc.Remove("IS_STRATEGIC");
                }
                if (dc.Contains("OCR_STATUS_ID"))
                {
                    dc.Remove("OCR_STATUS_ID");
                }
                if (dc.Contains("OCR_STATUS"))
                {
                    dc.Remove("OCR_STATUS");
                }
                if (dc.Contains("IS_SYNC"))
                {
                    dc.Remove("IS_SYNC");
                }
                if (dc.Contains("IS_ERROR"))
                {
                    dc.Remove("IS_ERROR");
                }
                if (dc.Contains("ERROR_MSG"))
                {
                    dc.Remove("ERROR_MSG");
                }
            }
            if (CalledFrom.Equals("Indexed_Queue"))
            {
                if (dc.Contains("WORK_ID"))
                {
                    dc.Remove("WORK_ID");
                }
                if (dc.Contains("RECEIVE_DATE"))
                {
                    dc.Remove("RECEIVE_DATE");
                }
                if (dc.Contains("SORCE_NAMEFormat"))
                {
                    dc.Remove("SORCE_NAMEFormat");
                }
                if (dc.Contains("ASSIGNTO_MEMBER"))
                {
                    dc.Remove("ASSIGNTO_MEMBER");
                }
                if (dc.Contains("NO_OF_COMMENTS"))
                {
                    dc.Remove("NO_OF_COMMENTS");
                }
                if (dc.Contains("PATIENT_ACCOUNT"))
                {
                    dc.Remove("PATIENT_ACCOUNT");
                }
                if (dc.Contains("FILE_PATH"))
                {
                    dc.Remove("FILE_PATH");
                }
                if (dc.Contains("PRACTICE_CODE"))
                {
                    dc.Remove("PRACTICE_CODE");
                }
                if (dc.Contains("RE_ASSIGNED_TO"))
                {
                    dc.Remove("RE_ASSIGNED_TO");
                }
                if (dc.Contains("file_path1"))
                {
                    dc.Remove("file_path1");
                }
                if (dc.Contains("TOTAL_ROCORD_PAGES"))
                {
                    dc.Remove("TOTAL_ROCORD_PAGES");
                }
                if (dc.Contains("INDEXED_DATE"))
                {
                    dc.Remove("INDEXED_DATE");
                }
                if (dc.Contains("TOTAL_RECORDS"))
                {
                    dc.Remove("TOTAL_RECORDS");
                }
                if (dc.Contains("IS_EMERGENCY"))
                {
                    dc.Remove("IS_EMERGENCY");
                }
                if (dc.Contains("IS_UNSIGNED"))
                {
                    dc.Remove("IS_UNSIGNED");
                }
            }
            if (CalledFrom.Equals("Completed_Queue"))
            {
                if (dc.Contains("WORK_ID"))
                {
                    dc.Remove("WORK_ID");
                }
                if (dc.Contains("RECEIVE_DATE"))
                {
                    dc.Remove("RECEIVE_DATE");
                }
                if (dc.Contains("SORCE_NAMEFormat"))
                {
                    dc.Remove("SORCE_NAMEFormat");
                }
                if (dc.Contains("COMPLETED_DATE"))
                {
                    dc.Remove("COMPLETED_DATE");
                }
                if (dc.Contains("ASSIGNED_DATE"))
                {
                    dc.Remove("ASSIGNED_DATE");
                }
                if (dc.Contains("MODIFIED_DATE"))
                {
                    dc.Remove("MODIFIED_DATE");
                }
                if (dc.Contains("FILE_PATH"))
                {
                    dc.Remove("FILE_PATH");
                }
                if (dc.Contains("PRACTICE_CODE"))
                {
                    dc.Remove("PRACTICE_CODE");
                }
                if (dc.Contains("PATIENT_ACCOUNT"))
                {
                    dc.Remove("PATIENT_ACCOUNT");
                }
                if (dc.Contains("WORK_STATUS"))
                {
                    dc.Remove("WORK_STATUS");
                }
                if (dc.Contains("SSN"))
                {
                    dc.Remove("SSN");
                }
                if (dc.Contains("TOTAL_ROCORD_PAGES"))
                {
                    dc.Remove("TOTAL_ROCORD_PAGES");
                }
                if (dc.Contains("CREATED_BY"))
                {
                    dc.Remove("CREATED_BY");
                }
                if (dc.Contains("TOTAL_RECORDS"))
                {
                    dc.Remove("TOTAL_RECORDS");
                }
                if (dc.Contains("IS_EMERGENCY"))
                {
                    dc.Remove("IS_EMERGENCY");
                }
                if (dc.Contains("IS_UNSIGNED"))
                {
                    dc.Remove("IS_UNSIGNED");
                }
                if (dc.Contains("CREATED_DATE"))
                {
                    dc.Remove("CREATED_DATE");
                }
                if (dc.Contains("DELETED"))
                {
                    dc.Remove("DELETED");
                }
                if (dc.Contains("IS_STRATEGIC"))
                {
                    dc.Remove("IS_STRATEGIC");
                }
            }
            if (CalledFrom.Equals("Supervisor_Queue"))
            {
                if (dc.Contains("WORK_ID"))
                {
                    dc.Remove("WORK_ID");
                }
                if (dc.Contains("RECEIVE_DATE"))
                {
                    dc.Remove("RECEIVE_DATE");
                }
                if (dc.Contains("SORCE_NAMEFormat"))
                {
                    dc.Remove("SORCE_NAMEFormat");
                }
                if (dc.Contains("COMPLETED_DATE"))
                {
                    dc.Remove("COMPLETED_DATE");
                }
                if (dc.Contains("ASSIGNED_TO"))
                {
                    dc.Remove("ASSIGNED_TO");
                }
                if (dc.Contains("ASSIGNED_DATE"))
                {
                    dc.Remove("ASSIGNED_DATE");
                }
                if (dc.Contains("FILE_PATH"))
                {
                    dc.Remove("FILE_PATH");
                }
                if (dc.Contains("PRACTICE_CODE"))
                {
                    dc.Remove("PRACTICE_CODE");
                }
                if (dc.Contains("PATIENT_ACCOUNT"))
                {
                    dc.Remove("PATIENT_ACCOUNT");
                }
                if (dc.Contains("SENDER_ID"))
                {
                    dc.Remove("SENDER_ID");
                }
                if (dc.Contains("MODIFIED_BY"))
                {
                    dc.Remove("MODIFIED_BY");
                }
                if (dc.Contains("MODIFIED_DATE"))
                {
                    dc.Remove("MODIFIED_DATE");
                }
                if (dc.Contains("NO_OF_SPLITS"))
                {
                    dc.Remove("NO_OF_SPLITS");
                }
                if (dc.Contains("TOTAL_ROCORD_PAGES"))
                {
                    dc.Remove("TOTAL_ROCORD_PAGES");
                }
                if (dc.Contains("CREATED_BY"))
                {
                    dc.Remove("CREATED_BY");
                }
                if (dc.Contains("CREATED_DATE"))
                {
                    dc.Remove("CREATED_DATE");
                }
                if (dc.Contains("TOTAL_RECORDS"))
                {
                    dc.Remove("TOTAL_RECORDS");
                }
                if (dc.Contains("IS_EMERGENCY"))
                {
                    dc.Remove("IS_EMERGENCY");
                }
                if (dc.Contains("IS_UNSIGNED"))
                {
                    dc.Remove("IS_UNSIGNED");
                }
                if (dc.Contains("DELETED"))
                {
                    dc.Remove("DELETED");
                }
                if (dc.Contains("COMPLETED_BY"))
                {
                    dc.Remove("COMPLETED_BY");
                }
                if (dc.Contains("COMPLETED_DATE"))
                {
                    dc.Remove("COMPLETED_DATE");
                }
                if (dc.Contains("DOCUMENT_TYPE"))
                {
                    dc.Remove("DOCUMENT_TYPE");
                }
                if (dc.Contains("FACILITY_NAME"))
                {
                    dc.Remove("FACILITY_NAME");
                }
                if (dc.Contains("DEPARTMENT_ID"))
                {
                    dc.Remove("DEPARTMENT_ID");
                }
                if (dc.Contains("IS_EMERGENCY_ORDER"))
                {
                    dc.Remove("IS_EMERGENCY_ORDER");
                }
                if (dc.Contains("REASON_FOR_VISIT"))
                {
                    dc.Remove("REASON_FOR_VISIT");
                }
                if (dc.Contains("ACCOUNT_NUMBER"))
                {
                    dc.Remove("ACCOUNT_NUMBER");
                }
                if (dc.Contains("UNIT_CASE_NO"))
                {
                    dc.Remove("UNIT_CASE_NO");
                }
                if (dc.Contains("IS_STRATEGIC"))
                {
                    dc.Remove("IS_STRATEGIC");
                }
            }
            if (CalledFrom.Equals("Search_Order"))
            {
                if (dc.Contains("WORK_ID"))
                {
                    dc.Remove("WORK_ID");
                }
                if (dc.Contains("Date_Of_Birth"))
                {
                    dc.Remove("Date_Of_Birth");
                }
                if (dc.Contains("RECEIVE_DATE"))
                {
                    dc.Remove("RECEIVE_DATE");
                }
                if (dc.Contains("COMPLETED_DATE"))
                {
                    dc.Remove("COMPLETED_DATE");
                }
                if (dc.Contains("TOTAL_ROCORD_PAGES"))
                {
                    dc.Remove("TOTAL_ROCORD_PAGES");
                }
                if (dc.Contains("TOTAL_RECORDS"))
                {
                    dc.Remove("TOTAL_RECORDS");
                }
            }
            if (CalledFrom.Equals("Referral_Report"))
            {
                if (dc.Contains("UNIQUE_ID"))
                {
                    dc.Remove("UNIQUE_ID");
                }
                if (dc.Contains("FAX_ID"))
                {
                    dc.Remove("FAX_ID");
                }
                if (dc.Contains("INDEXED_DATE"))
                {
                    dc.Remove("INDEXED_DATE");
                }
                if (dc.Contains("RECEIVE_DATE"))
                {
                    dc.Remove("RECEIVE_DATE");
                }
                if (dc.Contains("COMPLETED_DATE"))
                {
                    dc.Remove("COMPLETED_DATE");
                }
                if (dc.Contains("TOTAL_ROCORD_PAGES"))
                {
                    dc.Remove("TOTAL_ROCORD_PAGES");
                }
                if (dc.Contains("TOTAL_RECORDS"))
                {
                    dc.Remove("TOTAL_RECORDS");
                }
            }
            if (CalledFrom.Equals("Detailed_Survey"))
            {
                //if (dc.Contains("ROW"))
                //{
                //    dc.Remove("ROW");
                //}
                if (dc.Contains("Is_exceptional_Str"))
                {
                    dc.Remove("Is_exceptional_Str");
                }
                if (dc.Contains("SURVEY_COMPLETED_DATE"))
                {
                    dc.Remove("SURVEY_COMPLETED_DATE");
                }
                if (dc.Contains("Modified_Date_Str"))
                {
                    dc.Remove("Modified_Date_Str");
                }
                if (dc.Contains("MODIFIED_DATE"))
                {
                    dc.Remove("MODIFIED_DATE");
                }
                if (dc.Contains("MONTH"))
                {
                    dc.Remove("MONTH");
                }
                if (dc.Contains("SURVEY_ID"))
                {
                    dc.Remove("SURVEY_ID");
                }
                if (dc.Contains("PRACTICE_CODE"))
                {
                    dc.Remove("PRACTICE_CODE");
                }
                if (dc.Contains("FACILITY_OR_CLIENT_ID"))
                {
                    dc.Remove("FACILITY_OR_CLIENT_ID");
                }
                if (dc.Contains("RESPONSIBLE_PARTY_LAST_NAME"))
                {
                    dc.Remove("RESPONSIBLE_PARTY_LAST_NAME");
                }
                if (dc.Contains("RESPONSIBLE_PARTY_FIRST_NAME"))
                {
                    dc.Remove("RESPONSIBLE_PARTY_FIRST_NAME");
                }
                if (dc.Contains("RESPONSIBLE_PARTY_MIDDLE_INITIAL"))
                {
                    dc.Remove("RESPONSIBLE_PARTY_MIDDLE_INITIAL");
                }
                if (dc.Contains("RESPONSIBLE_PARTY_ADDRESS"))
                {
                    dc.Remove("RESPONSIBLE_PARTY_ADDRESS");
                }
                if (dc.Contains("RESPONSIBLE_PARTY_CITY"))
                {
                    dc.Remove("RESPONSIBLE_PARTY_CITY");
                }
                if (dc.Contains("RESPONSIBLE_PARTY_STATE"))
                {
                    dc.Remove("RESPONSIBLE_PARTY_STATE");
                }
                if (dc.Contains("RESPONSIBLE_PARTY_ZIP_CODE"))
                {
                    dc.Remove("RESPONSIBLE_PARTY_ZIP_CODE");
                }
                if (dc.Contains("RESPONSIBLE_PARTY_TELEPHONE"))
                {
                    dc.Remove("RESPONSIBLE_PARTY_TELEPHONE");
                }
                if (dc.Contains("RESPONSIBLE_PARTY_SSN"))
                {
                    dc.Remove("RESPONSIBLE_PARTY_SSN");
                }
                if (dc.Contains("RESPONSIBLE_PARTY_SEX"))
                {
                    dc.Remove("RESPONSIBLE_PARTY_SEX");
                }
                if (dc.Contains("RESPONSIBLE_PARTY_DATE_OF_BIRTH"))
                {
                    dc.Remove("RESPONSIBLE_PARTY_DATE_OF_BIRTH");
                }
                if (dc.Contains("PATIENT_LAST_NAME"))
                {
                    dc.Remove("PATIENT_LAST_NAME");
                }
                if (dc.Contains("PATIENT_FIRST_NAME"))
                {
                    dc.Remove("PATIENT_FIRST_NAME");
                }
                if (dc.Contains("PATIENT_MIDDLE_INITIAL"))
                {
                    dc.Remove("PATIENT_MIDDLE_INITIAL");
                }
                if (dc.Contains("PATIENT_ADDRESS"))
                {
                    dc.Remove("PATIENT_ADDRESS");
                }
                if (dc.Contains("PATIENT_CITY"))
                {
                    dc.Remove("PATIENT_CITY");
                }
                if (dc.Contains("PATIENT_ZIP_CODE"))
                {
                    dc.Remove("PATIENT_ZIP_CODE");
                }
                if (dc.Contains("PATIENT_TELEPHONE_NUMBER"))
                {
                    dc.Remove("PATIENT_TELEPHONE_NUMBER");
                }
                if (dc.Contains("PATIENT_SOCIAL_SECURITY_NUMBER"))
                {
                    dc.Remove("PATIENT_SOCIAL_SECURITY_NUMBER");
                }
                if (dc.Contains("PATIENT_GENDER"))
                {
                    dc.Remove("PATIENT_GENDER");
                }
                if (dc.Contains("PATIENT_DATE_OF_BIRTH"))
                {
                    dc.Remove("PATIENT_DATE_OF_BIRTH");
                }
                if (dc.Contains("ALTERNATE_CONTACT_LAST_NAME"))
                {
                    dc.Remove("ALTERNATE_CONTACT_LAST_NAME");
                }
                if (dc.Contains("ALTERNATE_CONTACT_FIRST_NAME"))
                {
                    dc.Remove("ALTERNATE_CONTACT_FIRST_NAME");
                }
                if (dc.Contains("ALTERNATE_CONTACT_MIDDLE_INITIAL"))
                {
                    dc.Remove("ALTERNATE_CONTACT_MIDDLE_INITIAL");
                }
                if (dc.Contains("ALTERNATE_CONTACT_TELEPHONE"))
                {
                    dc.Remove("ALTERNATE_CONTACT_TELEPHONE");
                }
                if (dc.Contains("EMR_LOCATION_CODE"))
                {
                    dc.Remove("EMR_LOCATION_CODE");
                }
                if (dc.Contains("EMR_LOCATION_DESCRIPTION"))
                {
                    dc.Remove("EMR_LOCATION_DESCRIPTION");
                }
                if (dc.Contains("SERVICE_OR_PAYMENT_DESCRIPTION"))
                {
                    dc.Remove("SERVICE_OR_PAYMENT_DESCRIPTION");
                }
                if (dc.Contains("LAST_VISIT_DATE"))
                {
                    dc.Remove("LAST_VISIT_DATE");
                }
                if (dc.Contains("DISCHARGE_DATE"))
                {
                    dc.Remove("DISCHARGE_DATE");
                }
                //if (dc.Contains("ATTENDING_DOCTOR_NAME"))
                //{
                //    dc.Remove("ATTENDING_DOCTOR_NAME");
                //}
                if (dc.Contains("REFERRAL_DATE"))
                {
                    dc.Remove("REFERRAL_DATE");
                }
                if (dc.Contains("PROCEDURE_OR_TRAN_CODE"))
                {
                    dc.Remove("PROCEDURE_OR_TRAN_CODE");
                }
                if (dc.Contains("IS_IMPROVED_SETISFACTION"))
                {
                    dc.Remove("IS_IMPROVED_SETISFACTION");
                }
                if (dc.Contains("IS_REFERABLE"))
                {
                    dc.Remove("IS_REFERABLE");
                }
                if (dc.Contains("IS_CONTACT_HQ"))
                {
                    dc.Remove("IS_CONTACT_HQ");
                }
                if (dc.Contains("IS_RESPONSED_BY_HQ"))
                {
                    dc.Remove("IS_RESPONSED_BY_HQ");
                }
                if (dc.Contains("IS_QUESTION_ANSWERED"))
                {
                    dc.Remove("IS_QUESTION_ANSWERED");
                }
                if (dc.Contains("SERVICE_OR_PAYMENT_AMOUNT"))
                {
                    dc.Remove("SERVICE_OR_PAYMENT_AMOUNT");
                }
                if (dc.Contains("SURVEY_FORMAT_TYPE"))
                {
                    dc.Remove("SURVEY_FORMAT_TYPE");
                }
                if (dc.Contains("CREATED_BY"))
                {
                    dc.Remove("CREATED_BY");
                }
                if (dc.Contains("CREATED_DATE"))
                {
                    dc.Remove("CREATED_DATE");
                }
                if (dc.Contains("MODIFIED_BY"))
                {
                    dc.Remove("MODIFIED_BY");
                }
                if (dc.Contains("IS_SURVEYED"))
                {
                    dc.Remove("IS_SURVEYED");
                }
                if (dc.Contains("FILE_NAME"))
                {
                    dc.Remove("FILE_NAME");
                }
                if (dc.Contains("SHEET_NAME"))
                {
                    dc.Remove("SHEET_NAME");
                }
                if (dc.Contains("TOTAL_RECORD_IN_FILE"))
                {
                    dc.Remove("TOTAL_RECORD_IN_FILE");
                }
                if (dc.Contains("DELETED"))
                {
                    dc.Remove("DELETED");
                }
                if (dc.Contains("IN_PROGRESS"))
                {
                    dc.Remove("IN_PROGRESS");
                }
                if (dc.Contains("PATIENT_AGE"))
                {
                    dc.Remove("PATIENT_AGE");
                }
                if (dc.Contains("RESPONSIBLE_PARTY_AGE"))
                {
                    dc.Remove("RESPONSIBLE_PARTY_AGE");
                }
                if (dc.Contains("DAY_NAME"))
                {
                    dc.Remove("DAY_NAME");
                }
                if (dc.Contains("TOTAL_RECORD_PAGES"))
                {
                    dc.Remove("TOTAL_RECORD_PAGES");
                }
                if (dc.Contains("TOTAL_RECORDS"))
                {
                    dc.Remove("TOTAL_RECORDS");
                }
                if (dc.Contains("Surveyed_BY_LNAME"))
                {
                    dc.Remove("Surveyed_BY_LNAME");
                }
                if (dc.Contains("Surveyed_BY_FNAME"))
                {
                    dc.Remove("Surveyed_BY_FNAME");
                }
                if (dc.Contains("ACTIVE_FORMAT"))
                {
                    dc.Remove("ACTIVE_FORMAT");
                }
                if (dc.Contains("SURVEY_ID_Str"))
                {
                    dc.Remove("SURVEY_ID_Str");
                }
                if (dc.Contains("IS_EXCEPTIONAL"))
                {
                    dc.Remove("IS_EXCEPTIONAL");
                }
                if (dc.Contains("IS_PROTECTIVE_EQUIPMENT"))
                {
                    dc.Remove("IS_PROTECTIVE_EQUIPMENT");
                }
            }
            if (CalledFrom.Equals("Interface_Log_Report"))
            {
                if (dc.Contains("FOX_INTERFACE_LOG_ID"))
                {
                    dc.Remove("FOX_INTERFACE_LOG_ID");
                }
                //if (dc.Contains("ROW"))
                //{
                //    dc.Remove("ROW");
                //}
                if (dc.Contains("PATIENT_ACCOUNT_str"))
                {
                    dc.Remove("PATIENT_ACCOUNT_str");
                }
                if (dc.Contains("CREATED_BY"))
                {
                    dc.Remove("CREATED_BY");
                }
                if (dc.Contains("Created_Date_Str"))
                {
                    dc.Remove("Created_Date_Str");
                }
                if (dc.Contains("LOG_MESSAGE"))
                {
                    dc.Remove("LOG_MESSAGE");
                }
                if (dc.Contains("ACK"))
                {
                    dc.Remove("ACK");
                }
                if (dc.Contains("IS_INCOMMING"))
                {
                    dc.Remove("IS_INCOMMING");
                }
                if (dc.Contains("IS_OUTGOING"))
                {
                    dc.Remove("IS_OUTGOING");
                }
                if (dc.Contains("MODIFIED_BY"))
                {
                    dc.Remove("MODIFIED_BY");
                }
                if (dc.Contains("MODIFIED_DATE"))
                {
                    dc.Remove("MODIFIED_DATE");
                }
                if (dc.Contains("DELETED"))
                {
                    dc.Remove("DELETED");
                }
                if (dc.Contains("TOTAL_RECORD_PAGES"))
                {
                    dc.Remove("TOTAL_RECORD_PAGES");
                }
                if (dc.Contains("TOTAL_RECORDS"))
                {
                    dc.Remove("TOTAL_RECORDS");
                }
                if (dc.Contains("PRACTICE_CODE"))
                {
                    dc.Remove("PRACTICE_CODE");
                }
                if (dc.Contains("SUCCESSFUL_INTERFACED"))
                {
                    dc.Remove("SUCCESSFUL_INTERFACED");
                }
                if (dc.Contains("FAILED_INTERFACED"))
                {
                    dc.Remove("FAILED_INTERFACED");
                }
                if (dc.Contains("IS_ERROR"))
                {
                    dc.Remove("IS_ERROR");
                }
            }

            if (CalledFrom.Equals("PHR_Report"))
            {
                if (dc.Contains("USER_PHONE"))
                {
                    dc.Remove("USER_PHONE");
                }
                if (dc.Contains("USER_ID"))
                {
                    dc.Remove("USER_ID");
                }
                if (dc.Contains("PATIENT_ACCOUNT_str"))
                {
                    dc.Remove("PATIENT_ACCOUNT_str");
                }
                if (dc.Contains("DELETED"))
                {
                    dc.Remove("DELETED");
                }
                if (dc.Contains("Created_Date"))
                {
                    dc.Remove("Created_Date");
                }
                if (dc.Contains("TOTAL_RECORD_PAGES"))
                {
                    dc.Remove("TOTAL_RECORD_PAGES");
                }
                if (dc.Contains("TOTAL_RECORDS"))
                {
                    dc.Remove("TOTAL_RECORDS");
                }
                if (dc.Contains("Response_Awaited"))
                {
                    dc.Remove("Response_Awaited");
                }
                if (dc.Contains("Active"))
                {
                    dc.Remove("Active");
                }
                if (dc.Contains("IS_ACTIVE"))
                {
                    dc.Remove("IS_ACTIVE");
                }
            }

            if (CalledFrom.Equals("Detailed_Survey_old"))
            {
                if (dc.Contains("MODIFIED_DATE"))
                {
                    dc.Remove("MODIFIED_DATE");
                }
                if (dc.Contains("SURVEY_COMPLETED_DATE"))
                {
                    dc.Remove("SURVEY_COMPLETED_DATE");
                }
                if (dc.Contains("Modified_Date_Str"))
                {
                    dc.Remove("Modified_Date_Str");
                }
                if (dc.Contains("SURVEY_ID"))
                {
                    dc.Remove("SURVEY_ID");
                }
                if (dc.Contains("PRACTICE_CODE"))
                {
                    dc.Remove("PRACTICE_CODE");
                }
                if (dc.Contains("FACILITY_OR_CLIENT_ID"))
                {
                    dc.Remove("FACILITY_OR_CLIENT_ID");
                }
                if (dc.Contains("RESPONSIBLE_PARTY_LAST_NAME"))
                {
                    dc.Remove("RESPONSIBLE_PARTY_LAST_NAME");
                }
                if (dc.Contains("RESPONSIBLE_PARTY_FIRST_NAME"))
                {
                    dc.Remove("RESPONSIBLE_PARTY_FIRST_NAME");
                }
                if (dc.Contains("RESPONSIBLE_PARTY_MIDDLE_INITIAL"))
                {
                    dc.Remove("RESPONSIBLE_PARTY_MIDDLE_INITIAL");
                }
                if (dc.Contains("RESPONSIBLE_PARTY_ADDRESS"))
                {
                    dc.Remove("RESPONSIBLE_PARTY_ADDRESS");
                }
                if (dc.Contains("RESPONSIBLE_PARTY_CITY"))
                {
                    dc.Remove("RESPONSIBLE_PARTY_CITY");
                }
                if (dc.Contains("RESPONSIBLE_PARTY_STATE"))
                {
                    dc.Remove("RESPONSIBLE_PARTY_STATE");
                }
                if (dc.Contains("RESPONSIBLE_PARTY_ZIP_CODE"))
                {
                    dc.Remove("RESPONSIBLE_PARTY_ZIP_CODE");
                }
                if (dc.Contains("RESPONSIBLE_PARTY_TELEPHONE"))
                {
                    dc.Remove("RESPONSIBLE_PARTY_TELEPHONE");
                }
                if (dc.Contains("RESPONSIBLE_PARTY_SSN"))
                {
                    dc.Remove("RESPONSIBLE_PARTY_SSN");
                }
                if (dc.Contains("RESPONSIBLE_PARTY_SEX"))
                {
                    dc.Remove("RESPONSIBLE_PARTY_SEX");
                }
                if (dc.Contains("RESPONSIBLE_PARTY_DATE_OF_BIRTH"))
                {
                    dc.Remove("RESPONSIBLE_PARTY_DATE_OF_BIRTH");
                }
                if (dc.Contains("PATIENT_LAST_NAME"))
                {
                    dc.Remove("PATIENT_LAST_NAME");
                }
                if (dc.Contains("PATIENT_FIRST_NAME"))
                {
                    dc.Remove("PATIENT_FIRST_NAME");
                }
                if (dc.Contains("PATIENT_MIDDLE_INITIAL"))
                {
                    dc.Remove("PATIENT_MIDDLE_INITIAL");
                }
                if (dc.Contains("PATIENT_ADDRESS"))
                {
                    dc.Remove("PATIENT_ADDRESS");
                }
                if (dc.Contains("PATIENT_CITY"))
                {
                    dc.Remove("PATIENT_CITY");
                }
                if (dc.Contains("PATIENT_ZIP_CODE"))
                {
                    dc.Remove("PATIENT_ZIP_CODE");
                }
                if (dc.Contains("PATIENT_TELEPHONE_NUMBER"))
                {
                    dc.Remove("PATIENT_TELEPHONE_NUMBER");
                }
                if (dc.Contains("PATIENT_SOCIAL_SECURITY_NUMBER"))
                {
                    dc.Remove("PATIENT_SOCIAL_SECURITY_NUMBER");
                }
                if (dc.Contains("PATIENT_GENDER"))
                {
                    dc.Remove("PATIENT_GENDER");
                }
                if (dc.Contains("PATIENT_DATE_OF_BIRTH"))
                {
                    dc.Remove("PATIENT_DATE_OF_BIRTH");
                }
                if (dc.Contains("ALTERNATE_CONTACT_LAST_NAME"))
                {
                    dc.Remove("ALTERNATE_CONTACT_LAST_NAME");
                }
                if (dc.Contains("ALTERNATE_CONTACT_FIRST_NAME"))
                {
                    dc.Remove("ALTERNATE_CONTACT_FIRST_NAME");
                }
                if (dc.Contains("ALTERNATE_CONTACT_MIDDLE_INITIAL"))
                {
                    dc.Remove("ALTERNATE_CONTACT_MIDDLE_INITIAL");
                }
                if (dc.Contains("ALTERNATE_CONTACT_TELEPHONE"))
                {
                    dc.Remove("ALTERNATE_CONTACT_TELEPHONE");
                }
                if (dc.Contains("EMR_LOCATION_CODE"))
                {
                    dc.Remove("EMR_LOCATION_CODE");
                }
                if (dc.Contains("EMR_LOCATION_DESCRIPTION"))
                {
                    dc.Remove("EMR_LOCATION_DESCRIPTION");
                }
                if (dc.Contains("SERVICE_OR_PAYMENT_DESCRIPTION"))
                {
                    dc.Remove("SERVICE_OR_PAYMENT_DESCRIPTION");
                }
                if (dc.Contains("LAST_VISIT_DATE"))
                {
                    dc.Remove("LAST_VISIT_DATE");
                }
                if (dc.Contains("DISCHARGE_DATE"))
                {
                    dc.Remove("DISCHARGE_DATE");
                }
                //if (dc.Contains("ATTENDING_DOCTOR_NAME"))
                //{
                //    dc.Remove("ATTENDING_DOCTOR_NAME");
                //}
                if (dc.Contains("REFERRAL_DATE"))
                {
                    dc.Remove("REFERRAL_DATE");
                }
                if (dc.Contains("PROCEDURE_OR_TRAN_CODE"))
                {
                    dc.Remove("PROCEDURE_OR_TRAN_CODE");
                }
                if (dc.Contains("IS_IMPROVED_SETISFACTION"))
                {
                    dc.Remove("IS_IMPROVED_SETISFACTION");
                }
                if (dc.Contains("IS_REFERABLE"))
                {
                    dc.Remove("IS_REFERABLE");
                }
                if (dc.Contains("IS_CONTACT_HQ"))
                {
                    dc.Remove("IS_CONTACT_HQ");
                }
                if (dc.Contains("IS_RESPONSED_BY_HQ"))
                {
                    dc.Remove("IS_RESPONSED_BY_HQ");
                }
                if (dc.Contains("IS_QUESTION_ANSWERED"))
                {
                    dc.Remove("IS_QUESTION_ANSWERED");
                }
                if (dc.Contains("SERVICE_OR_PAYMENT_AMOUNT"))
                {
                    dc.Remove("SERVICE_OR_PAYMENT_AMOUNT");
                }
                if (dc.Contains("SURVEY_FLAG"))
                {
                    dc.Remove("SURVEY_FLAG");
                }
                if (dc.Contains("SURVEY_FORMAT_TYPE"))
                {
                    dc.Remove("SURVEY_FORMAT_TYPE");
                }
                if (dc.Contains("CREATED_BY"))
                {
                    dc.Remove("CREATED_BY");
                }
                if (dc.Contains("CREATED_DATE"))
                {
                    dc.Remove("CREATED_DATE");
                }
                if (dc.Contains("MODIFIED_BY"))
                {
                    dc.Remove("MODIFIED_BY");
                }
                if (dc.Contains("IS_SURVEYED"))
                {
                    dc.Remove("IS_SURVEYED");
                }
                if (dc.Contains("FILE_NAME"))
                {
                    dc.Remove("FILE_NAME");
                }
                if (dc.Contains("SHEET_NAME"))
                {
                    dc.Remove("SHEET_NAME");
                }
                if (dc.Contains("TOTAL_RECORD_IN_FILE"))
                {
                    dc.Remove("TOTAL_RECORD_IN_FILE");
                }
                if (dc.Contains("DELETED"))
                {
                    dc.Remove("DELETED");
                }
                if (dc.Contains("IN_PROGRESS"))
                {
                    dc.Remove("IN_PROGRESS");
                }
                if (dc.Contains("PATIENT_AGE"))
                {
                    dc.Remove("PATIENT_AGE");
                }
                if (dc.Contains("RESPONSIBLE_PARTY_AGE"))
                {
                    dc.Remove("RESPONSIBLE_PARTY_AGE");
                }
                if (dc.Contains("DAY_NAME"))
                {
                    dc.Remove("DAY_NAME");
                }
                if (dc.Contains("TOTAL_RECORD_PAGES"))
                {
                    dc.Remove("TOTAL_RECORD_PAGES");
                }
                if (dc.Contains("TOTAL_RECORDS"))
                {
                    dc.Remove("TOTAL_RECORDS");
                }
                if (dc.Contains("Surveyed_BY_LNAME"))
                {
                    dc.Remove("Surveyed_BY_LNAME");
                }
                if (dc.Contains("Surveyed_BY_FNAME"))
                {
                    dc.Remove("Surveyed_BY_FNAME");
                }
                if (dc.Contains("ACTIVE_FORMAT"))
                {
                    dc.Remove("ACTIVE_FORMAT");
                }
                if (dc.Contains("SURVEY_ID_Str"))
                {
                    dc.Remove("SURVEY_ID_Str");
                }
                if (dc.Contains("Is_Questioned_Answered_Str"))
                {
                    dc.Remove("Is_Questioned_Answered_Str");
                }
            }
            if (CalledFrom.Equals("Interface_Logs"))
            {
                if (dc.Contains("FOX_INTERFACE_LOG_ID"))
                {
                    dc.Remove("FOX_INTERFACE_LOG_ID");
                }
                if (dc.Contains("FOX_INTERFACE_SYNCH_ID"))
                {
                    dc.Remove("FOX_INTERFACE_SYNCH_ID");
                }
                if (dc.Contains("PRACTICE_CODE"))
                {
                    dc.Remove("PRACTICE_CODE");
                }
                if (dc.Contains("PATIENT_ACCOUNT_str"))
                {
                    dc.Remove("PATIENT_ACCOUNT_str");
                }
                if (dc.Contains("TOTAL_RECORD_PAGES"))
                {
                    dc.Remove("TOTAL_RECORD_PAGES");
                }
                if (dc.Contains("CREATED_DATE"))
                {
                    dc.Remove("CREATED_DATE");
                }
                if (dc.Contains("TOTAL_RECORDS"))
                {
                    dc.Remove("TOTAL_RECORDS");
                }
                if (dc.Contains("IS_ERROR"))
                {
                    dc.Remove("IS_ERROR");
                }
                if (dc.Contains("IS_INCOMMING"))
                {
                    dc.Remove("IS_INCOMMING");
                }
                if (dc.Contains("IS_OUTGOING"))
                {
                    dc.Remove("IS_OUTGOING");
                }
                if (dc.Contains("MODIFIED_BY"))
                {
                    dc.Remove("MODIFIED_BY");
                }
                if (dc.Contains("MODIFIED_DATE"))
                {
                    dc.Remove("MODIFIED_DATE");
                }
                if (dc.Contains("DELETED"))
                {
                    dc.Remove("DELETED");
                }
            }
            if (CalledFrom.Equals("Notes_History"))
            {
                if (dc.Contains("NOTE_ID"))
                {
                    dc.Remove("NOTE_ID");
                }
                if (dc.Contains("WORK_ID"))
                {
                    dc.Remove("WORK_ID");
                }
                if (dc.Contains("PRACTICE_CODE"))
                {
                    dc.Remove("PRACTICE_CODE");
                }
                if (dc.Contains("CREATED_DATE"))
                {
                    dc.Remove("CREATED_DATE");
                }
                if (dc.Contains("MODIFIED_BY"))
                {
                    dc.Remove("MODIFIED_BY");
                }
                if (dc.Contains("MODIFIED_DATE"))
                {
                    dc.Remove("MODIFIED_DATE");
                }
                if (dc.Contains("DELETED"))
                {
                    dc.Remove("DELETED");
                }
            }
            if (CalledFrom.Equals("Region_Survey"))
            {
                if (dc.Contains("RECOMMENDED_AVG"))
                {
                    dc.Remove("RECOMMENDED_AVG");
                }
                if (dc.Contains("NOT_RECOMMENDED_AVG"))
                {
                    dc.Remove("NOT_RECOMMENDED_AVG");
                }
            }
            if (CalledFrom.Equals("Region_Survey_Patient_Data"))
            {
                if (dc.Contains("DISCHARGE_DATE"))
                {
                    dc.Remove("DISCHARGE_DATE");
                }
                if (dc.Contains("REFERRAL_DATE"))
                {
                    dc.Remove("REFERRAL_DATE");
                }
                if (dc.Contains("TOTAL_RECORD_PAGES"))
                {
                    dc.Remove("TOTAL_RECORD_PAGES");
                }
                if (dc.Contains("TOTAL_RECORDS"))
                {
                    dc.Remove("TOTAL_RECORDS");
                }
            }
            if (CalledFrom.Equals("Question_Survey"))
            {
                if (dc.Contains("IS_CONTACT_HQ_YES_AVG"))
                {
                    dc.Remove("IS_CONTACT_HQ_YES_AVG");
                }
                if (dc.Contains("IS_CONTACT_HQ_NO_AVG"))
                {
                    dc.Remove("IS_CONTACT_HQ_NO_AVG");
                }
                if (dc.Contains("IS_RESPONSED_BY_HQ_YES_AVG"))
                {
                    dc.Remove("IS_RESPONSED_BY_HQ_YES_AVG");
                }
                if (dc.Contains("IS_RESPONSED_BY_HQ_NO_AVG"))
                {
                    dc.Remove("IS_RESPONSED_BY_HQ_NO_AVG");
                }
                if (dc.Contains("IS_QUESTION_ANSWERED_YES_AVG"))
                {
                    dc.Remove("IS_QUESTION_ANSWERED_YES_AVG");
                }
                if (dc.Contains("IS_QUESTION_ANSWERED_NO_AVG"))
                {
                    dc.Remove("IS_QUESTION_ANSWERED_NO_AVG");
                }
                if (dc.Contains("IS_REFERABLE_YES_AVG"))
                {
                    dc.Remove("IS_REFERABLE_YES_AVG");
                }
                if (dc.Contains("IS_REFERABLE_NO_AVG"))
                {
                    dc.Remove("IS_REFERABLE_NO_AVG");
                }
                if (dc.Contains("IS_IMPROVED_SETISFACTION_YES_AVG"))
                {
                    dc.Remove("IS_IMPROVED_SETISFACTION_YES_AVG");
                }
                if (dc.Contains("IS_IMPROVED_SETISFACTION_NO_AVG"))
                {
                    dc.Remove("IS_IMPROVED_SETISFACTION_NO_AVG");
                }
                if (dc.Contains("IS_PROTECTIVE_EQUIPMENT_YES_AVG"))
                {
                    dc.Remove("IS_PROTECTIVE_EQUIPMENT_YES_AVG");
                }
                if (dc.Contains("IS_PROTECTIVE_EQUIPMENT_NO_AVG"))
                {
                    dc.Remove("IS_PROTECTIVE_EQUIPMENT_NO_AVG");
                }
            }
            if (CalledFrom.Equals("Insurance_Setup"))
            {
                if (dc.Contains("FOX_TBL_INSURANCE_ID"))
                {
                    dc.Remove("FOX_TBL_INSURANCE_ID");
                }
                if (dc.Contains("PRACTICE_CODE"))
                {
                    dc.Remove("PRACTICE_CODE");
                }
                if (dc.Contains("PAYING_AGENCY"))
                {
                    dc.Remove("PAYING_AGENCY");
                }
                if (dc.Contains("PAYER_TYPE"))
                {
                    dc.Remove("PAYER_TYPE");
                }
                if (dc.Contains("PLAN_CODE"))
                {
                    dc.Remove("PLAN_CODE");
                }
                if (dc.Contains("INSURANCE_ID"))
                {
                    dc.Remove("INSURANCE_ID");
                }
                if (dc.Contains("COUNTY"))
                {
                    dc.Remove("COUNTY");
                }
                if (dc.Contains("FAX"))
                {
                    dc.Remove("FAX");
                }
                if (dc.Contains("FIELD_SET"))
                {
                    dc.Remove("FIELD_SET");
                }
                if (dc.Contains("ALLOW_CONCURRENT_THERAPY"))
                {
                    dc.Remove("ALLOW_CONCURRENT_THERAPY");
                }
                if (dc.Contains("ALLOW_CO_TREATMENT"))
                {
                    dc.Remove("ALLOW_CO_TREATMENT");
                }
                if (dc.Contains("CCI_VALIDATION"))
                {
                    dc.Remove("CCI_VALIDATION");
                }
                if (dc.Contains("MODIFIER_KX"))
                {
                    dc.Remove("MODIFIER_KX");
                }
                if (dc.Contains("MODIFIER_52_APPLIES"))
                {
                    dc.Remove("MODIFIER_52_APPLIES");
                }
                if (dc.Contains("CAP_TRACKING"))
                {
                    dc.Remove("CAP_TRACKING");
                }
                if (dc.Contains("MPPR_ADJUSTED"))
                {
                    dc.Remove("MPPR_ADJUSTED");
                }
                if (dc.Contains("PQRS"))
                {
                    dc.Remove("PQRS");
                }
                if (dc.Contains("FUNCTIONAL_LIMITATIONS_REPORTING"))
                {
                    dc.Remove("FUNCTIONAL_LIMITATIONS_REPORTING");
                }
                if (dc.Contains("EIGHT_MIN_RULE_CALCULATION_METHOD"))
                {
                    dc.Remove("EIGHT_MIN_RULE_CALCULATION_METHOD");
                }
                if (dc.Contains("BILL_FORM"))
                {
                    dc.Remove("BILL_FORM");
                }
                if (dc.Contains("ELECTRONIC_SUBMISSION"))
                {
                    dc.Remove("ELECTRONIC_SUBMISSION");
                }
                if (dc.Contains("CLAIM_FILING_INDICATOR_CODE"))
                {
                    dc.Remove("CLAIM_FILING_INDICATOR_CODE");
                }
                if (dc.Contains("SUBSEQUENT_ELECTRONIC_CLAIM"))
                {
                    dc.Remove("SUBSEQUENT_ELECTRONIC_CLAIM");
                }
                if (dc.Contains("SEPARATE_CLAIM_FOR_EACH_DISCIPLINE"))
                {
                    dc.Remove("SEPARATE_CLAIM_FOR_EACH_DISCIPLINE");
                }
                if (dc.Contains("REBILL_REQUIRES_DCN"))
                {
                    dc.Remove("REBILL_REQUIRES_DCN");
                }
                if (dc.Contains("INCLUDE_ZEROPOINT_ONE_CHARGES_ON_PROFESSIONAL_CLAIMS_SUBMITTED_AFTER_PRIMARY_INSTITUTIONAL_CLAIMS"))
                {
                    dc.Remove("INCLUDE_ZEROPOINT_ONE_CHARGES_ON_PROFESSIONAL_CLAIMS_SUBMITTED_AFTER_PRIMARY_INSTITUTIONAL_CLAIMS");
                }
                if (dc.Contains("APPLY_SEQUESTRATION_ADJUSTMENT"))
                {
                    dc.Remove("APPLY_SEQUESTRATION_ADJUSTMENT");
                }
                if (dc.Contains("SEPARATE_PROFESSIONAL_CLAIM_BY_RENDERING_THERAPIST_NPI"))
                {
                    dc.Remove("SEPARATE_PROFESSIONAL_CLAIM_BY_RENDERING_THERAPIST_NPI");
                }
                if (dc.Contains("RIM_HOLDS_WILL_PREVENT_CLAIMS_FROM_RELEASING"))
                {
                    dc.Remove("RIM_HOLDS_WILL_PREVENT_CLAIMS_FROM_RELEASING");
                }
                if (dc.Contains("PHYSICIAN_REQUIREMENT_NPI"))
                {
                    dc.Remove("PHYSICIAN_REQUIREMENT_NPI");
                }
                if (dc.Contains("RENDERING_PROVIDER_REQUIRES_NPI"))
                {
                    dc.Remove("RENDERING_PROVIDER_REQUIRES_NPI");
                }
                if (dc.Contains("RENDERING_PROVIDER_REQUIRES_STATE_LICENSE"))
                {
                    dc.Remove("RENDERING_PROVIDER_REQUIRES_STATE_LICENSE");
                }
                if (dc.Contains("RENDERING_PROVIDER_REQUIRES_TAXONOMY"))
                {
                    dc.Remove("RENDERING_PROVIDER_REQUIRES_TAXONOMY");
                }
                if (dc.Contains("REQUIRE_ADMITTING_DIAGNOSIS"))
                {
                    dc.Remove("REQUIRE_ADMITTING_DIAGNOSIS");
                }
                if (dc.Contains("PHONE_NUMBER_FOR_AUTHORIZATIONS"))
                {
                    dc.Remove("PHONE_NUMBER_FOR_AUTHORIZATIONS");
                }
                if (dc.Contains("TIMELY_FILING_LIMIT"))
                {
                    dc.Remove("TIMELY_FILING_LIMIT");
                }
                if (dc.Contains("OUTPATIENT_PRIOR_AUTHORIZATION_REQUIRED"))
                {
                    dc.Remove("OUTPATIENT_PRIOR_AUTHORIZATION_REQUIRED");
                }
                if (dc.Contains("OUTPATIENT_INCLUDE_KX_MODIFIER_ON_CLAIMS"))
                {
                    dc.Remove("OUTPATIENT_INCLUDE_KX_MODIFIER_ON_CLAIMS");
                }
                if (dc.Contains("OUTPATIENT_APPLY_KX_TO_ENTIRE_CLAIM_PERIOD"))
                {
                    dc.Remove("OUTPATIENT_APPLY_KX_TO_ENTIRE_CLAIM_PERIOD");
                }
                if (dc.Contains("OUTPATIENT_INCLUDE_59_MODIFIER_ON_CLAIMS"))
                {
                    dc.Remove("OUTPATIENT_INCLUDE_59_MODIFIER_ON_CLAIMS");
                }
                if (dc.Contains("RENDERING_PROVIDER_CREDENTIALING_REQUIREMENTS"))
                {
                    dc.Remove("RENDERING_PROVIDER_CREDENTIALING_REQUIREMENTS");
                }
                if (dc.Contains("CREATED_BY"))
                {
                    dc.Remove("CREATED_BY");
                }
                if (dc.Contains("CREATED_DATE"))
                {
                    dc.Remove("CREATED_DATE");
                }
                if (dc.Contains("MODIFIED_BY"))
                {
                    dc.Remove("MODIFIED_BY");
                }
                if (dc.Contains("MODIFIED_DATE"))
                {
                    dc.Remove("MODIFIED_DATE");
                }
                if (dc.Contains("DELETED"))
                {
                    dc.Remove("DELETED");
                }
                if (dc.Contains("IS_CCI_VALIDATION"))
                {
                    dc.Remove("IS_CCI_VALIDATION");
                }
                if (dc.Contains("IS_MODIFIER_KX"))
                {
                    dc.Remove("IS_MODIFIER_KX");
                }
                if (dc.Contains("IS_CAP_TRACKING"))
                {
                    dc.Remove("IS_CAP_TRACKING");
                }
                if (dc.Contains("IS_PQRS"))
                {
                    dc.Remove("IS_PQRS");
                }
                if (dc.Contains("IS_FUNCTIONAL_LIMITATIONS_REPORTING"))
                {
                    dc.Remove("IS_FUNCTIONAL_LIMITATIONS_REPORTING");
                }
                if (dc.Contains("FINANCIAL_CLASS_ID"))
                {
                    dc.Remove("FINANCIAL_CLASS_ID");
                }
                if (dc.Contains("Is_Authorization_Required"))
                {
                    dc.Remove("Is_Authorization_Required");
                }
                if (dc.Contains("TOTAL_RECORD_PAGES"))
                {
                    dc.Remove("TOTAL_RECORD_PAGES");
                }
                if (dc.Contains("TOTAL_RECORDS"))
                {
                    dc.Remove("TOTAL_RECORDS");
                }
                if (dc.Contains("FEE_REDIRECT"))
                {
                    dc.Remove("FEE_REDIRECT");
                }
            }
            if (CalledFrom.Equals("High_Balance_Report"))
            {
                if (dc.Contains("DOCUMENT_TYPE"))
                {
                    dc.Remove("DOCUMENT_TYPE");
                }
                if (dc.Contains("ORS"))
                {
                    dc.Remove("ORS");
                }
                if (dc.Contains("WORK_ORDER_SOURCE"))
                {
                    dc.Remove("WORK_ORDER_SOURCE");
                }
                if (dc.Contains("FACILITY_NAME"))
                {
                    dc.Remove("FACILITY_NAME");
                }
                if (dc.Contains("DISCIPLINE_NO"))
                {
                    dc.Remove("DISCIPLINE_NO");
                }
                if (dc.Contains("TOTAL_RECORDS"))
                {
                    dc.Remove("TOTAL_RECORDS");
                }
                if (dc.Contains("TOTAL_RECORD_PAGES"))
                {
                    dc.Remove("TOTAL_RECORD_PAGES");
                }
            }
            if (CalledFrom.Equals("QA_Report"))
            {
                if (dc.Contains("TOTAL_RECORDS"))
                {
                    dc.Remove("TOTAL_RECORDS");
                }
                if (dc.Contains("TOTAL_RECORD_PAGES"))
                {
                    dc.Remove("TOTAL_RECORD_PAGES");
                }
                if (dc.Contains("AGENT_NAME"))
                {
                    dc.Remove("AGENT_NAME");
                }
            }
            if (CalledFrom.Equals("QA_Report_AUD"))
            {
                if (dc.Contains("TOTAL_RECORDS"))
                {
                    dc.Remove("TOTAL_RECORDS");
                }
                if (dc.Contains("TOTAL_RECORD_PAGES"))
                {
                    dc.Remove("TOTAL_RECORD_PAGES");
                }
                if (dc.Contains("AGENT_NAME"))
                {
                    dc.Remove("AGENT_NAME");
                }
            }
            if (CalledFrom.Equals("QA_Report_Survey"))
            {
                if (dc.Contains("TOTAL_RECORDS"))
                {
                    dc.Remove("TOTAL_RECORDS");
                }
                if (dc.Contains("TOTAL_RECORD_PAGES"))
                {
                    dc.Remove("TOTAL_RECORD_PAGES");
                }
                if (dc.Contains("AGENT_NAME"))
                {
                    dc.Remove("AGENT_NAME");
                }
                if (dc.Contains("WOW_FACTOR"))
                {
                    dc.Remove("WOW_FACTOR");
                }
            }
            if (CalledFrom.Equals("QA_Report_AUD_Survey"))
            {
                if (dc.Contains("TOTAL_RECORDS"))
                {
                    dc.Remove("TOTAL_RECORDS");
                }
                if (dc.Contains("TOTAL_RECORD_PAGES"))
                {
                    dc.Remove("TOTAL_RECORD_PAGES");
                }
                if (dc.Contains("AGENT_NAME"))
                {
                    dc.Remove("AGENT_NAME");
                }
                if (dc.Contains("WOW_FACTOR"))
                {
                    dc.Remove("WOW_FACTOR");
                }
            }
            if (CalledFrom.Equals("Daily_Appointment_List"))
            {
                if (dc.Contains("APPOINTMENT_ID"))
                {
                    dc.Remove("APPOINTMENT_ID");
                }
                if (dc.Contains("Alerts"))
                {
                    dc.Remove("Alerts");
                }
                if (dc.Contains("ROW"))
                {
                    dc.Remove("ROW");
                }
                if (dc.Contains("LENGTH"))
                {
                    dc.Remove("LENGTH");
                }
                if (dc.Contains("ADDRESS_ID"))
                {
                    dc.Remove("ADDRESS_ID");
                }
                if (dc.Contains("FC_ID"))
                {
                    dc.Remove("FC_ID");
                }
                if (dc.Contains("FC_CODE"))
                {
                    dc.Remove("FC_CODE");
                }
                if (dc.Contains("FC_DESCRIPTION"))
                {
                    dc.Remove("FC_DESCRIPTION");
                }
                if (dc.Contains("REASON_ID"))
                {
                    dc.Remove("REASON_ID");
                }
                if (dc.Contains("STATUS_ID"))
                {
                    dc.Remove("STATUS_ID");
                }
                if (dc.Contains("POS_ID"))
                {
                    dc.Remove("POS_ID");
                }
                if (dc.Contains("POS_CODE"))
                {
                    dc.Remove("POS_CODE");
                }
                if (dc.Contains("POS_NAME"))
                {
                    dc.Remove("POS_NAME");
                }
                if (dc.Contains("POS_ADDRESS"))
                {
                    dc.Remove("POS_ADDRESS");
                }
                if (dc.Contains("POS_CITY"))
                {
                    dc.Remove("POS_CITY");
                }
                if (dc.Contains("POS_STATE"))
                {
                    dc.Remove("POS_STATE");
                }
                if (dc.Contains("POS_ZIP"))
                {
                    dc.Remove("POS_ZIP");
                }
                if (dc.Contains("REGION_ID"))
                {
                    dc.Remove("REGION_ID");
                }
                if (dc.Contains("REGION_CODE"))
                {
                    dc.Remove("REGION_CODE");
                }
                if (dc.Contains("STR_LENGTH"))
                {
                    dc.Remove("STR_LENGTH");
                }
                if (dc.Contains("HOME_PHONE"))
                {
                    dc.Remove("HOME_PHONE");
                }
                //if (dc.Contains("APPOINTMENT_DATE"))
                //{
                //    dc.Remove("APPOINTMENT_DATE");
                //}
                if (dc.Contains("VISIT_TYPE_ID"))
                {
                    dc.Remove("VISIT_TYPE_ID");
                }
                if (dc.Contains("TIME_TO"))
                {
                    dc.Remove("TIME_TO");
                }
                if (dc.Contains("PROVIDER_ID"))
                {
                    dc.Remove("PROVIDER_ID");
                }
                if (dc.Contains("CASE_ID"))
                {
                    dc.Remove("CASE_ID");
                }
                if (dc.Contains("PRACTICE_CODE"))
                {
                    dc.Remove("PRACTICE_CODE");
                }
                if (dc.Contains("PATIENT_ACCOUNT"))
                {
                    dc.Remove("PATIENT_ACCOUNT");
                }
                if (dc.Contains("APPOINTMENT_STATUS_ID"))
                {
                    dc.Remove("APPOINTMENT_STATUS_ID");
                }
                if (dc.Contains("SIGNED_BY"))
                {
                    dc.Remove("SIGNED_BY");
                }
                if (dc.Contains("REASON_FOR_SIGNING"))
                {
                    dc.Remove("REASON_FOR_SIGNING");
                }
                if (dc.Contains("SIGNATURE_PATH"))
                {
                    dc.Remove("SIGNATURE_PATH");
                }
                if (dc.Contains("LATITUDE"))
                {
                    dc.Remove("LATITUDE");
                }
                if (dc.Contains("LONGITUDE"))
                {
                    dc.Remove("LONGITUDE");
                }
                if (dc.Contains("NOTES"))
                {
                    dc.Remove("NOTES");
                }
                if (dc.Contains("CREATED_DATE"))
                {
                    dc.Remove("CREATED_DATE");
                }
                if (dc.Contains("CREATED_BY"))
                {
                    dc.Remove("CREATED_BY");
                }
                if (dc.Contains("MODIFIED_DATE"))
                {
                    dc.Remove("MODIFIED_DATE");
                }
                if (dc.Contains("MODIFIED_BY"))
                {
                    dc.Remove("MODIFIED_BY");
                }
                if (dc.Contains("DELETED"))
                {
                    dc.Remove("DELETED");
                }
                if (dc.Contains("VISIT_FREQUENCY"))
                {
                    dc.Remove("VISIT_FREQUENCY");
                }
                if (dc.Contains("VISIT_CRITERIA"))
                {
                    dc.Remove("VISIT_CRITERIA");
                }
                if (dc.Contains("DURATION_DAYS"))
                {
                    dc.Remove("DURATION_DAYS");
                }
                if (dc.Contains("IsSendForApproval"))
                {
                    dc.Remove("IsSendForApproval");
                }
                if (dc.Contains("AssignToProviderID"))
                {
                    dc.Remove("AssignToProviderID");
                }
                if (dc.Contains("PTOID"))
                {
                    dc.Remove("PTOID");
                }
                if (dc.Contains("IsClinicianAssignHisOwnAppt"))
                {
                    dc.Remove("IsClinicianAssignHisOwnAppt");
                }
                if (dc.Contains("IsApproveRD"))
                {
                    dc.Remove("IsApproveRD");
                }
                if (dc.Contains("IsDenyRD"))
                {
                    dc.Remove("IsDenyRD");
                }
                if (dc.Contains("IsSend"))
                {
                    dc.Remove("IsSend");
                }
                if (dc.Contains("SendToID"))
                {
                    dc.Remove("SendToID");
                }
                if (dc.Contains("IsSendToORS"))
                {
                    dc.Remove("IsSendToORS");
                }
                if (dc.Contains("SendDate"))
                {
                    dc.Remove("SendDate");
                }
                if (dc.Contains("IsBlocked"))
                {
                    dc.Remove("IsBlocked");
                }
                if (dc.Contains("ReasonForBlocked"))
                {
                    dc.Remove("ReasonForBlocked");
                }
                if (dc.Contains("TOTAL_RECORDS"))
                {
                    dc.Remove("TOTAL_RECORDS");
                }
                if (dc.Contains("TOTAL_RECORD_PAGES"))
                {
                    dc.Remove("TOTAL_RECORD_PAGES");
                }
                if (dc.Contains("IS_PATIENT_OVERLAPPING"))
                {
                    dc.Remove("IS_PATIENT_OVERLAPPING");
                }
                if (dc.Contains("IS_PROVIDER_OVERLAPPING"))
                {
                    dc.Remove("IS_PROVIDER_OVERLAPPING");
                }
                if (dc.Contains("IS_PROVIDER_AVAILABLE"))
                {
                    dc.Remove("IS_PROVIDER_AVAILABLE");
                }
                if (dc.Contains("APPOINTMENT_DATE"))
                {
                    dc.Remove("APPOINTMENT_DATE");
                }
                if (dc.Contains("TIME_FROM_DATE"))
                {
                    dc.Remove("TIME_FROM_DATE");
                }
                if (dc.Contains("TIME_TO_DATE"))
                {
                    dc.Remove("TIME_TO_DATE");
                }
                if (dc.Contains("IS_NEW"))
                {
                    dc.Remove("IS_NEW");
                }
                if (dc.Contains("IS_RECURSIVE"))
                {
                    dc.Remove("IS_RECURSIVE");
                }
                if (dc.Contains("CANCELLATION_REASON_ID"))
                {
                    dc.Remove("CANCELLATION_REASON_ID");
                }
                if (dc.Contains("CANCELLATION_DESCRIPTION"))
                {
                    dc.Remove("CANCELLATION_DESCRIPTION");
                }
                if (dc.Contains("CANCELATIONREASON_ID"))
                {
                    dc.Remove("CANCELATIONREASON_ID");
                }
                if (dc.Contains("CANCELLATION_CODE"))
                {
                    dc.Remove("CANCELLATION_CODE");
                }
                if (dc.Contains("PATIENTA_ADDRESS_LONGITUDE"))
                {
                    dc.Remove("PATIENTA_ADDRESS_LONGITUDE");
                }
                if (dc.Contains("PATIENT_ADDRESS_LATITUDE"))
                {
                    dc.Remove("PATIENT_ADDRESS_LATITUDE");
                }
                if (dc.Contains("AL_LONGITUDE"))
                {
                    dc.Remove("AL_LONGITUDE");
                }
                if (dc.Contains("AL_LATITUDE"))
                {
                    dc.Remove("AL_LATITUDE");
                }
                if (dc.Contains("ACCURACY"))
                {
                    dc.Remove("ACCURACY");
                }
                if (dc.Contains("APPT_LATITUDE_STR"))
                {
                    dc.Remove("APPT_LATITUDE_STR");
                }
                if (dc.Contains("APPT_LONGITUDE_STR"))
                {
                    dc.Remove("APPT_LONGITUDE_STR");
                }
                if (dc.Contains("PATIENTA_ADDRESS_LONGITUDE_STR"))
                {
                    dc.Remove("PATIENTA_ADDRESS_LONGITUDE_STR");
                }
                if (dc.Contains("PATIENT_ADDRESS_LATITUDE_STR"))
                {
                    dc.Remove("PATIENT_ADDRESS_LATITUDE_STR");
                }
                //if (dc.Contains("AL_LONGITUDE_STR"))
                //{
                //    dc.Remove("AL_LONGITUDE_STR");
                //}
                //if (dc.Contains("AL_LATITUDE_STR"))
                //{
                //    dc.Remove("AL_LATITUDE_STR");
                //}
                if (dc.Contains("CITY"))
                {
                    dc.Remove("CITY");
                }
                if (dc.Contains("STATE"))
                {
                    dc.Remove("STATE");
                }
                if (dc.Contains("ZIP"))
                {
                    dc.Remove("ZIP");
                }
                if (dc.Contains("APPOINTMENT_COMPLETE_DATE_TIME"))// missing Lines
                {
                    dc.Remove("APPOINTMENT_COMPLETE_DATE_TIME");
                }
            }
            if (CalledFrom.Equals("Patient_Scheduler_List"))
            {
                if (dc.Contains("APPOINTMENT_ID"))
                {
                    dc.Remove("APPOINTMENT_ID");
                }
                if (dc.Contains("MRN"))
                {
                    dc.Remove("MRN");
                }
                if (dc.Contains("Alerts"))
                {
                    dc.Remove("Alerts");
                }
                if (dc.Contains("ROW"))
                {
                    dc.Remove("ROW");
                }
                if (dc.Contains("LENGTH"))
                {
                    dc.Remove("LENGTH");
                }
                if (dc.Contains("ADDRESS_ID"))
                {
                    dc.Remove("ADDRESS_ID");
                }
                if (dc.Contains("FC_ID"))
                {
                    dc.Remove("FC_ID");
                }
                if (dc.Contains("FC_CODE"))
                {
                    dc.Remove("FC_CODE");
                }
                if (dc.Contains("FC_DESCRIPTION"))
                {
                    dc.Remove("FC_DESCRIPTION");
                }
                if (dc.Contains("REASON_ID"))
                {
                    dc.Remove("REASON_ID");
                }
                if (dc.Contains("STATUS_ID"))
                {
                    dc.Remove("STATUS_ID");
                }
                if (dc.Contains("POS_ID"))
                {
                    dc.Remove("POS_ID");
                }
                if (dc.Contains("POS_CODE"))
                {
                    dc.Remove("POS_CODE");
                }
                if (dc.Contains("POS_NAME"))
                {
                    dc.Remove("POS_NAME");
                }
                if (dc.Contains("POS_ADDRESS"))
                {
                    dc.Remove("POS_ADDRESS");
                }
                if (dc.Contains("POS_CITY"))
                {
                    dc.Remove("POS_CITY");
                }
                if (dc.Contains("POS_STATE"))
                {
                    dc.Remove("POS_STATE");
                }
                if (dc.Contains("POS_ZIP"))
                {
                    dc.Remove("POS_ZIP");
                }
                if (dc.Contains("REGION_ID"))
                {
                    dc.Remove("REGION_ID");
                }
                if (dc.Contains("REGION_CODE"))
                {
                    dc.Remove("REGION_CODE");
                }
                if (dc.Contains("REGION_NAME"))
                {
                    dc.Remove("REGION_NAME");
                }
                if (dc.Contains("HOME_PHONE"))
                {
                    dc.Remove("HOME_PHONE");
                }
                if (dc.Contains("APPOINTMENT_DATE"))
                {
                    dc.Remove("APPOINTMENT_DATE");
                }
                if (dc.Contains("VISIT_TYPE_ID"))
                {
                    dc.Remove("VISIT_TYPE_ID");
                }
                if (dc.Contains("TIME_TO"))
                {
                    dc.Remove("TIME_TO");
                }
                if (dc.Contains("PROVIDER_ID"))
                {
                    dc.Remove("PROVIDER_ID");
                }
                if (dc.Contains("CASE_ID"))
                {
                    dc.Remove("CASE_ID");
                }
                if (dc.Contains("PRACTICE_CODE"))
                {
                    dc.Remove("PRACTICE_CODE");
                }
                if (dc.Contains("PATIENT_ACCOUNT"))
                {
                    dc.Remove("PATIENT_ACCOUNT");
                }
                if (dc.Contains("APPOINTMENT_STATUS_ID"))
                {
                    dc.Remove("APPOINTMENT_STATUS_ID");
                }
                if (dc.Contains("SIGNED_BY"))
                {
                    dc.Remove("SIGNED_BY");
                }
                if (dc.Contains("REASON_FOR_SIGNING"))
                {
                    dc.Remove("REASON_FOR_SIGNING");
                }
                if (dc.Contains("SIGNATURE_PATH"))
                {
                    dc.Remove("SIGNATURE_PATH");
                }
                if (dc.Contains("LATITUDE"))
                {
                    dc.Remove("LATITUDE");
                }
                if (dc.Contains("LONGITUDE"))
                {
                    dc.Remove("LONGITUDE");
                }
                if (dc.Contains("NOTES"))
                {
                    dc.Remove("NOTES");
                }
                if (dc.Contains("CREATED_DATE"))
                {
                    dc.Remove("CREATED_DATE");
                }
                if (dc.Contains("CREATED_BY"))
                {
                    dc.Remove("CREATED_BY");
                }
                if (dc.Contains("MODIFIED_DATE"))
                {
                    dc.Remove("MODIFIED_DATE");
                }
                if (dc.Contains("MODIFIED_BY"))
                {
                    dc.Remove("MODIFIED_BY");
                }
                if (dc.Contains("DELETED"))
                {
                    dc.Remove("DELETED");
                }
                if (dc.Contains("VISIT_FREQUENCY"))
                {
                    dc.Remove("VISIT_FREQUENCY");
                }
                if (dc.Contains("VISIT_CRITERIA"))
                {
                    dc.Remove("VISIT_CRITERIA");
                }
                if (dc.Contains("DURATION_DAYS"))
                {
                    dc.Remove("DURATION_DAYS");
                }
                if (dc.Contains("IsSendForApproval"))
                {
                    dc.Remove("IsSendForApproval");
                }
                if (dc.Contains("AssignToProviderID"))
                {
                    dc.Remove("AssignToProviderID");
                }
                if (dc.Contains("PTOID"))
                {
                    dc.Remove("PTOID");
                }
                if (dc.Contains("IsClinicianAssignHisOwnAppt"))
                {
                    dc.Remove("IsClinicianAssignHisOwnAppt");
                }
                if (dc.Contains("IsApproveRD"))
                {
                    dc.Remove("IsApproveRD");
                }
                if (dc.Contains("IsDenyRD"))
                {
                    dc.Remove("IsDenyRD");
                }
                if (dc.Contains("IsSend"))
                {
                    dc.Remove("IsSend");
                }
                if (dc.Contains("SendToID"))
                {
                    dc.Remove("SendToID");
                }
                if (dc.Contains("IsSendToORS"))
                {
                    dc.Remove("IsSendToORS");
                }
                if (dc.Contains("SendDate"))
                {
                    dc.Remove("SendDate");
                }
                if (dc.Contains("IsBlocked"))
                {
                    dc.Remove("IsBlocked");
                }
                if (dc.Contains("ReasonForBlocked"))
                {
                    dc.Remove("ReasonForBlocked");
                }
                if (dc.Contains("TOTAL_RECORDS"))
                {
                    dc.Remove("TOTAL_RECORDS");
                }
                if (dc.Contains("TOTAL_RECORD_PAGES"))
                {
                    dc.Remove("TOTAL_RECORD_PAGES");
                }
                if (dc.Contains("IS_PATIENT_OVERLAPPING"))
                {
                    dc.Remove("IS_PATIENT_OVERLAPPING");
                }
                if (dc.Contains("IS_PROVIDER_OVERLAPPING"))
                {
                    dc.Remove("IS_PROVIDER_OVERLAPPING");
                }
                if (dc.Contains("IS_PROVIDER_AVAILABLE"))
                {
                    dc.Remove("IS_PROVIDER_AVAILABLE");
                }
                //if (dc.Contains("APPOINTMENT_DATE_STR"))
                //{
                //    dc.Remove("APPOINTMENT_DATE_STR");
                //}
                if (dc.Contains("TIME_FROM_DATE"))
                {
                    dc.Remove("TIME_FROM_DATE");
                }
                if (dc.Contains("TIME_TO_DATE"))
                {
                    dc.Remove("TIME_TO_DATE");
                }
                if (dc.Contains("IS_NEW"))
                {
                    dc.Remove("IS_NEW");
                }
                if (dc.Contains("IS_RECURSIVE"))
                {
                    dc.Remove("IS_RECURSIVE");
                }
                if (dc.Contains("NAME"))
                {
                    dc.Remove("NAME");
                }
                if (dc.Contains("CANCELLATION_REASON_ID"))
                {
                    dc.Remove("CANCELLATION_REASON_ID");
                }
                if (dc.Contains("CANCELLATION_DESCRIPTION"))
                {
                    dc.Remove("CANCELLATION_DESCRIPTION");
                }
                if (dc.Contains("CANCELATIONREASON_ID"))
                {
                    dc.Remove("CANCELATIONREASON_ID");
                }
                if (dc.Contains("CANCELLATION_CODE"))
                {
                    dc.Remove("CANCELLATION_CODE");
                }
                if (dc.Contains("PATIENTA_ADDRESS_LONGITUDE"))
                {
                    dc.Remove("PATIENTA_ADDRESS_LONGITUDE");
                }
                if (dc.Contains("PATIENT_ADDRESS_LATITUDE"))
                {
                    dc.Remove("PATIENT_ADDRESS_LATITUDE");
                }
                if (dc.Contains("AL_LONGITUDE"))
                {
                    dc.Remove("AL_LONGITUDE");
                }
                if (dc.Contains("AL_LATITUDE"))
                {
                    dc.Remove("AL_LATITUDE");
                }
                if (dc.Contains("ACCURACY"))
                {
                    dc.Remove("ACCURACY");
                }
                if (dc.Contains("APPT_LATITUDE_STR"))
                {
                    dc.Remove("APPT_LATITUDE_STR");
                }
                if (dc.Contains("APPT_LONGITUDE_STR"))
                {
                    dc.Remove("APPT_LONGITUDE_STR");
                }
                if (dc.Contains("PATIENTA_ADDRESS_LONGITUDE_STR"))
                {
                    dc.Remove("PATIENTA_ADDRESS_LONGITUDE_STR");
                }
                if (dc.Contains("PATIENT_ADDRESS_LATITUDE_STR"))
                {
                    dc.Remove("PATIENT_ADDRESS_LATITUDE_STR");
                }
                if (dc.Contains("AL_LONGITUDE_STR"))
                {
                    dc.Remove("AL_LONGITUDE_STR");
                }
                if (dc.Contains("AL_LATITUDE_STR"))
                {
                    dc.Remove("AL_LATITUDE_STR");
                }
                if (dc.Contains("CITY"))
                {
                    dc.Remove("CITY");
                }
                if (dc.Contains("STATE"))
                {
                    dc.Remove("STATE");
                }
                if (dc.Contains("ZIP"))
                {
                    dc.Remove("ZIP");
                }
            }
            if (CalledFrom.Equals("Active_Indexer_History"))
            {
                if (dc.Contains("ACTIVE_INDEXER_ID_HISTORY"))
                {
                    dc.Remove("ACTIVE_INDEXER_ID_HISTORY");
                }
                if (dc.Contains("PRACTICE_CODE"))
                {
                    dc.Remove("PRACTICE_CODE");
                }
                if (dc.Contains("INDEXER"))
                {
                    dc.Remove("INDEXER");
                }
                if (dc.Contains("CREATED_DATE"))
                {
                    dc.Remove("CREATED_DATE");
                }
                if (dc.Contains("CREATED_BY"))
                {
                    dc.Remove("CREATED_BY");
                }
                if (dc.Contains("MODIFIED_DATE"))
                {
                    dc.Remove("MODIFIED_DATE");
                }
                if (dc.Contains("MODIFIED_BY"))
                {
                    dc.Remove("MODIFIED_BY");
                }
                if (dc.Contains("DELETED"))
                {
                    dc.Remove("DELETED");
                }
                if (dc.Contains("FIRST_NAME"))
                {
                    dc.Remove("FIRST_NAME");
                }
                if (dc.Contains("LAST_NAME"))
                {
                    dc.Remove("LAST_NAME");
                }
                if (dc.Contains("CurrentPage"))
                {
                    dc.Remove("CurrentPage");
                }
                if (dc.Contains("RecordPerPage"))
                {
                    dc.Remove("RecordPerPage");
                }
                if (dc.Contains("SearchText"))
                {
                    dc.Remove("SearchText");
                }
                if (dc.Contains("TOTAL_RECORD_PAGES"))
                {
                    dc.Remove("TOTAL_RECORD_PAGES");
                }
                if (dc.Contains("TOTAL_RECORDS"))
                {
                    dc.Remove("TOTAL_RECORDS");
                }
            }
        }
        private static void SetAmounts(DataTable dt)
        {
            DataColumnCollection dtcol = dt.Columns;
            foreach (DataRow drr in dt.Rows)
            {
                #region Amounts Region
                if (dtcol.Contains("Billed_Amount"))
                {
                    if (drr["Billed_Amount"].ToString() != string.Empty)
                    {
                        decimal x = Convert.ToDecimal(drr["Billed_Amount"].ToString());
                        drr["Billed_Amount"] = x.ToString("#,##0.00");
                    }
                }
                if (dtcol.Contains("Due_Amount"))
                {
                    if (drr["Due_Amount"].ToString() != string.Empty)
                    {
                        decimal x = Convert.ToDecimal(drr["Due_Amount"].ToString());
                        drr["Due_Amount"] = x.ToString("#,##0.00");
                    }
                }
                if (dtcol.Contains("Claim_Total"))
                {
                    if (drr["Claim_Total"].ToString() != string.Empty)
                    {
                        decimal x = Convert.ToDecimal(drr["Claim_Total"].ToString());
                        drr["Claim_Total"] = x.ToString("#,##0.00");
                    }
                }

                if (dtcol.Contains("Amt_Due"))
                {
                    if (drr["Amt_Due"].ToString() != string.Empty)
                    {
                        decimal x = Convert.ToDecimal(drr["Amt_Due"].ToString());
                        drr["Amt_Due"] = x.ToString("#,##0.00");
                    }
                }
                if (dtcol.Contains("PaidAmount"))
                {
                    if (drr["PaidAmount"].ToString() != string.Empty)
                    {
                        decimal x = Convert.ToDecimal(drr["PaidAmount"].ToString());
                        drr["PaidAmount"] = x.ToString("#,##0.00");
                    }
                }
                if (dtcol.Contains("Amount Paid"))
                {
                    if (drr["Amount Paid"].ToString() != string.Empty)
                    {
                        decimal x = Convert.ToDecimal(drr["Amount Paid"].ToString());
                        drr["Amount Paid"] = x.ToString("#,##0.00");
                    }
                }
                //ERA Submission
                if (dtcol.Contains("Amount"))
                {
                    if (drr["Amount"].ToString() != string.Empty)
                    {
                        decimal x = Convert.ToDecimal(drr["Amount"].ToString());
                        drr["Amount"] = x.ToString("#,##0.00");
                    }
                }

                if (dtcol.Contains("TOTAL_AMOUNT"))
                {
                    if (drr["TOTAL_AMOUNT"].ToString() != string.Empty)
                    {
                        decimal x = Convert.ToDecimal(drr["TOTAL_AMOUNT"].ToString());
                        drr["TOTAL_AMOUNT"] = x.ToString("#,##0.00");
                    }
                }

                if (dtcol.Contains("AMOUNT_DUE"))
                {
                    if (drr["AMOUNT_DUE"].ToString() != string.Empty)
                    {
                        decimal x = Convert.ToDecimal(drr["AMOUNT_DUE"].ToString());
                        drr["AMOUNT_DUE"] = x.ToString("#,##0.00");
                    }
                }
                //ERA Submission
                if (dtcol.Contains("check_amount"))
                {
                    if (drr["check_amount"].ToString() != string.Empty)
                    {
                        decimal x = Convert.ToDecimal(drr["check_amount"].ToString());
                        drr["check_amount"] = x.ToString("#,##0.00");
                    }
                }
                if (dtcol.Contains("payments"))
                {
                    if (drr["payments"].ToString() != string.Empty)
                    {
                        decimal x = Convert.ToDecimal(drr["payments"].ToString());
                        drr["payments"] = x.ToString("#,##0.00");
                    }
                }
                if (dtcol.Contains("Days_0_30"))
                {
                    if (drr["Days_0_30"].ToString() != string.Empty)
                    {
                        decimal x = Convert.ToDecimal(drr["Days_0_30"].ToString());
                        drr["Days_0_30"] = x.ToString("#,##0.00");
                    }
                }


                #endregion
                #region Dates Region
                if (dtcol.Contains("DOS"))
                {
                    drr["DOS"] = string.Format("{0:MM/dd/yyyy}", drr["DOS"]);
                }
                if (dtcol.Contains("Bill_Date"))
                {
                    drr["Bill_Date"] = string.Format("{0:MM/dd/yyyy}", drr["Bill_Date"]);
                }
                #endregion
            }
        }

        private static void SetReconciliationHeaders(DataTable dt)
        {
            DataColumnCollection dtcol = dt.Columns;
            //For Reconciliation for Commercial Payer
            if (dt.TableName == "Reconciliations_CP")
            {
                if (dtcol.Contains("ROW"))
                {
                    dtcol["ROW"].ColumnName = "Sr. #";
                }
                if (dtcol.Contains("STATUS_NAME"))
                {
                    dtcol["STATUS_NAME"].ColumnName = "Status";
                }
                if (dtcol.Contains("INSURANCE_NAME"))
                {
                    dtcol["INSURANCE_NAME"].ColumnName = "Insurance";
                }
                if (dtcol.Contains("DEPOSIT_TYPE_NAME"))
                {
                    dtcol["DEPOSIT_TYPE_NAME"].ColumnName = "Deposit Type";
                }
                if (dtcol.Contains("STATE"))
                {
                    dtcol["STATE"].ColumnName = "State";
                }
                if (dtcol.Contains("CATEGORY_NAME"))
                {
                    dtcol["CATEGORY_NAME"].ColumnName = "Category/Account";
                }
                if (dtcol.Contains("CHECK_NO"))
                {
                    dtcol["CHECK_NO"].ColumnName = "Check #/ Batch #";
                }
                if (dtcol.Contains("ASSIGNED_TO_NAME"))
                {
                    dtcol["ASSIGNED_TO_NAME"].ColumnName = "Assigned To";
                }
                if (dtcol.Contains("LEDGER_NAME"))
                {
                    dtcol["LEDGER_NAME"].ColumnName = "Attachments";
                }
                if (dtcol.Contains("AMOUNT_POSTED"))
                {
                    dtcol["AMOUNT_POSTED"].ColumnName = "Total Posted";
                }
                if (dtcol.Contains("AMOUNT_NOT_POSTED"))
                {
                    dtcol["AMOUNT_NOT_POSTED"].ColumnName = "Not Posted Amount";
                }
                if (dtcol.Contains("COMPLETED_DATE"))
                {
                    dtcol["COMPLETED_DATE"].ColumnName = "Completed Date";
                }
                if (dtcol.Contains("Assigned_Date"))
                {
                    dtcol["Assigned_Date"].ColumnName = "Date Assigned";
                }
                if (dtcol.Contains("DEPOSIT_DATE"))
                {
                    dtcol["DEPOSIT_DATE"].ColumnName = "Deposit Date";
                }
                if (dtcol.Contains("AMOUNT"))
                {
                    dtcol["AMOUNT"].ColumnName = "Total Amount";
                }
                if (dtcol.Contains("AMOUNT"))
                {
                    dtcol["AMOUNT"].ColumnName = "Amount";
                }
                if (dtcol.Contains("DATE_POSTED"))
                {
                    dtcol["DATE_POSTED"].ColumnName = "Date Posted";
                }
                if (dtcol.Contains("REASON_NAME"))
                {
                    dtcol["REASON_NAME"].ColumnName = "Reason";
                }
                if (dtcol.Contains("ASSIGNED_GROUP"))
                {
                    dtcol["ASSIGNED_GROUP"].ColumnName = "Assigned Group";
                }
                if (dtcol.Contains("ASSIGNED_GROUP_DATE"))
                {
                    dtcol["ASSIGNED_GROUP_DATE"].ColumnName = "Assigned Group Date";
                }
            }
            else if (dt.TableName == "Reconciliation_CP_Logs")
            {
                if (dtcol.Contains("CREATED_DATE"))
                {
                    dtcol["CREATED_DATE"].ColumnName = "Created On";
                }
                if (dtcol.Contains("LOG_MESSAGE"))
                {
                    dtcol["LOG_MESSAGE"].ColumnName = "Log Message";
                }
                if (dtcol.Contains("CREATED_BY_NAME"))
                {
                    dtcol["CREATED_BY_NAME"].ColumnName = "Created By";
                }
            }


        }
        private static void SetHeaders(DataTable dt)
        {
            DataColumnCollection dtcol = dt.Columns;
            if (dt.TableName == "Patient_List")
            {
                if (dtcol.Contains("Patient_Account"))
                {
                    dtcol["Patient_Account"].ColumnName = "Account #";
                }
                if (dtcol.Contains("ROW"))
                {
                    dtcol["ROW"].ColumnName = "Sr. #";
                }
                if (dtcol.Contains("First_Name"))
                {
                    dtcol["First_Name"].ColumnName = "First Name";
                }
                if (dtcol.Contains("Last_Name"))
                {
                    dtcol["Last_Name"].ColumnName = "Last Name";
                }
                if (dtcol.Contains("FIRST_NAME_ALIAS"))
                {
                    dtcol["FIRST_NAME_ALIAS"].ColumnName = "Alias First Name";
                }
                if (dtcol.Contains("LAST_NAME_ALIAS"))
                {
                    dtcol["LAST_NAME_ALIAS"].ColumnName = "Alias Last Name";
                }
                if (dtcol.Contains("MIDDLE_INITIALS_ALIAS"))
                {
                    dtcol["MIDDLE_INITIALS_ALIAS"].ColumnName = "Alias MI";
                }
                if (dtcol.Contains("Created_By"))
                {
                    dtcol["Created_By"].ColumnName = "Created By";
                }
                if (dtcol.Contains("Created_Date_Time"))
                {
                    dtcol["Created_Date_Time"].ColumnName = "Created Date/Time";
                }
                if (dtcol.Contains("Updated_By"))
                {
                    dtcol["Updated_By"].ColumnName = "Updated By";
                }
                if (dtcol.Contains("Updated_By"))
                {
                    dtcol["Updated_By"].ColumnName = "Updated By";
                }
                if (dtcol.Contains("Update_Date_Time"))
                {
                    dtcol["Update_Date_Time"].ColumnName = "Updated Date/Time";
                }


            }
            else if (dt.TableName == "Adjustments")
            {
                if (dtcol.Contains("SERIAL_NUMBER"))
                {
                    dtcol["SERIAL_NUMBER"].ColumnName = "Sr. #";
                }
                if (dtcol.Contains("STATUS_NAME"))
                {
                    dtcol["STATUS_NAME"].ColumnName = "Status";
                }
                if (dtcol.Contains("PATIENT_NAME"))
                {
                    dtcol["PATIENT_NAME"].ColumnName = "Patient";
                }
                if (dtcol.Contains("DISCIPLINE_NAME"))
                {
                    dtcol["DISCIPLINE_NAME"].ColumnName = "Discipline(s)";
                }
                if (dtcol.Contains("ADJUSTMENT_AMOUNT"))
                {
                    dtcol["ADJUSTMENT_AMOUNT"].ColumnName = "Amount";
                }
                if (dtcol.Contains("REASON"))
                {
                    dtcol["REASON"].ColumnName = "Reason";
                }
                if (dtcol.Contains("REQUESTED_BY"))
                {
                    dtcol["REQUESTED_BY"].ColumnName = "Requested By";
                }
                if (dtcol.Contains("REQUESTED_DATE"))
                {
                    dtcol["REQUESTED_DATE"].ColumnName = "Requested Date";
                }
                if (dtcol.Contains("APPROVED_BY"))
                {
                    dtcol["APPROVED_BY"].ColumnName = "Approved By";
                }
                if (dtcol.Contains("APPROVED_DATE"))
                {
                    dtcol["APPROVED_DATE"].ColumnName = "Approved Date";
                }
                if (dtcol.Contains("ASSIGNED_TO"))
                {
                    dtcol["ASSIGNED_TO"].ColumnName = "Assigned To";
                }

            }
            else if (dt.TableName == "Users_Reports")
            {
                if (dtcol.Contains("ROW"))
                {
                    dtcol["ROW"].ColumnName = "Sr. #";
                }

                if (dtcol.Contains("Employee_name"))
                {
                    dtcol["Employee_name"].ColumnName = "Employee Name";
                }
                if (dtcol.Contains("EMAIL"))
                {
                    dtcol["EMAIL"].ColumnName = "Email Address";
                }
                if (dtcol.Contains("RT_USER_ID"))
                {
                    dtcol["RT_USER_ID"].ColumnName = "RT User ID";
                }
                if (dtcol.Contains("CREATED_DATE"))
                {
                    dtcol["CREATED_DATE"].ColumnName = "Effective From";
                }
                if (dtcol.Contains("REGION_NAME"))
                {
                    dtcol["REGION_NAME"].ColumnName = "Region";
                }
                if (dtcol.Contains("ROLE_NAME"))
                {
                    dtcol["ROLE_NAME"].ColumnName = "Role";
                }
                if (dtcol.Contains("SENDER_TYPE_NAME"))
                {
                    dtcol["SENDER_TYPE_NAME"].ColumnName = "Sender Type";
                }
            }
            else if (dt.TableName == "Advanced_Daily_Report")
            {
                if (dtcol.Contains("ROW"))
                {
                    dtcol["ROW"].ColumnName = "Sr. #";
                }
                if (dtcol.Contains("CALL_DATE_STR"))
                {
                    dtcol["CALL_DATE_STR"].ColumnName = "Date";
                }
                if (dtcol.Contains("PATIENT_ACCOUNT"))
                {
                    dtcol["PATIENT_ACCOUNT"].ColumnName = "Patient Account";
                }
                if (dtcol.Contains("NAME"))
                {
                    dtcol["NAME"].ColumnName = "Category";
                }
                if (dtcol.Contains("CALL_DETAILS"))
                {
                    dtcol["CALL_DETAILS"].ColumnName = "Remarks ";
                }
                if (dtcol.Contains("CurrencyAmount"))
                {
                    dtcol["CurrencyAmount"].ColumnName = "Amount Paid";
                }
            }
            else if (dt.TableName == "Patient_Helpdesk")
            {
                //PHD Call Module
                if (dtcol.Contains("ROW"))
                {
                    dtcol["ROW"].ColumnName = "Sr. #";
                }
                if (dtcol.Contains("PATIENT_ACCOUNT_STR"))
                {
                    dtcol["PATIENT_ACCOUNT_STR"].ColumnName = "Account #";
                }
                if (dtcol.Contains("DOS_STR"))
                {
                    dtcol["DOS_STR"].ColumnName = "Local Date";
                }
                if (dtcol.Contains("CALL_DATE_STR"))
                {
                    dtcol["CALL_DATE_STR"].ColumnName = "Date (ESD)";
                }
                if (dtcol.Contains("CALL_TIME_STR"))
                {
                    dtcol["CALL_TIME_STR"].ColumnName = "Time (EST)";
                }
                if (dtcol.Contains("CALL_DURATION"))
                {
                    dtcol["CALL_DURATION"].ColumnName = "Duration (Mins)";
                }
                if (dtcol.Contains("CALLER_NAME"))
                {
                    dtcol["CALLER_NAME"].ColumnName = "Caller Name";
                }
                if (dtcol.Contains("RELATIONSHIP"))
                {
                    dtcol["RELATIONSHIP"].ColumnName = "Relationship";
                }
                if (dtcol.Contains("CALL_DETAILS"))
                {
                    dtcol["CALL_DETAILS"].ColumnName = "Details";
                }
                if (dtcol.Contains("CALL_ATTENDED_BY"))
                {
                    dtcol["CALL_ATTENDED_BY"].ColumnName = "Attended By";
                }
                if (dtcol.Contains("INCOMING_CALL_NO"))
                {
                    dtcol["INCOMING_CALL_NO"].ColumnName = "Phone No.";
                }
                if (dtcol.Contains("PATIENT_EMAIL_ADDRESS"))
                {
                    dtcol["PATIENT_EMAIL_ADDRESS"].ColumnName = "Patient Email Address";
                }
                if (dtcol.Contains("CALL_SCENARIO_NAME"))
                {
                    dtcol["CALL_SCENARIO_NAME"].ColumnName = "Call Handling";
                }
                if (dtcol.Contains("CALL_REASON_NAME"))
                {
                    dtcol["CALL_REASON_NAME"].ColumnName = "Reason";
                }
                //if (dtcol.Contains("REQUEST_NAME"))
                //{
                //    dtcol["REQUEST_NAME"].ColumnName = "Request";
                //}
                if (dtcol.Contains("CALL_ATTENDED_BY_NAME"))
                {
                    dtcol["CALL_ATTENDED_BY_NAME"].ColumnName = "Attended By";
                }
                if (dtcol.Contains("AMOUNT"))
                {
                    dtcol["AMOUNT"].ColumnName = "Amount";
                }
                if (dtcol.Contains("CurrencyAmount"))
                {
                    dtcol["CurrencyAmount"].ColumnName = "Amount";
                }
                if (dtcol.Contains("SSCM_CASE_ID"))
                {
                    dtcol["SSCM_CASE_ID"].ColumnName = "SSCM Case";
                }
                if (dtcol.Contains("DOCUMENT_TYPE_NAME"))
                {
                    dtcol["DOCUMENT_TYPE_NAME"].ColumnName = "Document Type";
                }
                if (dtcol.Contains("FOLLOW_UP_DATE_STR"))
                {
                    dtcol["FOLLOW_UP_DATE_STR"].ColumnName = "Follow Up Date";
                }
                if (dtcol.Contains("CS_CASE_CATEGORY_NAME"))
                {
                    dtcol["CS_CASE_CATEGORY_NAME"].ColumnName = "Case Category";
                }
                if (dtcol.Contains("CS_Case_Status"))
                {
                    dtcol["CS_Case_Status"].ColumnName = "Case Status";
                }
            }
            else if (dt.TableName == "Users_Reports")
            {
                if (dtcol.Contains("ROW"))
                {
                    dtcol["ROW"].ColumnName = "Sr. #";
                }
                if (dtcol.Contains("FIRST_NAME"))
                {
                    dtcol["FIRST_NAME"].ColumnName = "First Name";
                }
                if (dtcol.Contains("LAST_NAME"))
                {
                    dtcol["LAST_NAME"].ColumnName = "Last Name";
                }
                if (dtcol.Contains("EMAIL"))
                {
                    dtcol["EMAIL"].ColumnName = "Email Address";
                }
            }
            else if (dt.TableName == "Interface_Log_Report")
            {
                if (dtcol.Contains("ROW"))
                {
                    dtcol["ROW"].ColumnName = "Sr. #";
                }
                if (dtcol.Contains("IS_ERROR"))
                {
                    dtcol["IS_ERROR"].ColumnName = "Error Status";
                }
                if (dtcol.Contains("ERROR"))
                {
                    dtcol["ERROR"].ColumnName = "Reason";
                }
                if (dtcol.Contains("PATIENT_ACCOUNT"))
                {
                    dtcol["PATIENT_ACCOUNT"].ColumnName = "Patient Account #";
                }
                if (dtcol.Contains("PATIENT_NAME"))
                {
                    dtcol["PATIENT_NAME"].ColumnName = "Patient Name";
                }

                if (dtcol.Contains("CREATED_DATE"))
                {
                    dtcol["CREATED_DATE"].ColumnName = "Date & Time";
                }
                if (dtcol.Contains("APPLICATION"))
                {
                    dtcol["APPLICATION"].ColumnName = "Application";
                }
                if (dtcol.Contains("TYPE"))
                {
                    dtcol["TYPE"].ColumnName = "Type";
                }

            }
            else if (dt.TableName == "PHR_Report")
            {
                if (dtcol.Contains("ROW"))
                {
                    dtcol["ROW"].ColumnName = "Sr. #";
                }
                if (dtcol.Contains("PATIENT_ACCOUNT"))
                {
                    dtcol["PATIENT_ACCOUNT"].ColumnName = "Patient Account #";
                }
                if (dtcol.Contains("FIRST_NAME"))
                {
                    dtcol["FIRST_NAME"].ColumnName = "First Name";
                }
                if (dtcol.Contains("LAST_NAME"))
                {
                    dtcol["LAST_NAME"].ColumnName = "Last Name";
                }
                if (dtcol.Contains("GENDER"))
                {
                    dtcol["GENDER"].ColumnName = "Gender";
                }
                if (dtcol.Contains("EMAIL"))
                {
                    dtcol["EMAIL"].ColumnName = "Email Address";
                }
                if (dtcol.Contains("REGION"))
                {
                    dtcol["REGION"].ColumnName = "Last Case Region";
                }
                if (dtcol.Contains("INVITATION_STATUS"))
                {
                    dtcol["INVITATION_STATUS"].ColumnName = "Invitation Status";
                }
                if (dtcol.Contains("PATIENT_STATUS"))
                {
                    dtcol["PATIENT_STATUS"].ColumnName = "Patient Status";
                }

            }
            else if (dt.TableName == "Task_User_Level")
            {
                if (dtcol.Contains("ROW"))
                {
                    dtcol["ROW"].ColumnName = "Sr. #";
                }
                if (dtcol.Contains("CREATED_DATE_STR"))
                {
                    dtcol["CREATED_DATE_STR"].ColumnName = "Created Date";
                }
                if (dtcol.Contains("CREATED_TIME_STR"))
                {
                    dtcol["CREATED_TIME_STR"].ColumnName = "Create Time";
                }
                if (dtcol.Contains("CREATED_BY_FULL_NAME"))
                {
                    dtcol["CREATED_BY_FULL_NAME"].ColumnName = "Created By";
                }
                if (dtcol.Contains("SENT_TO"))
                {
                    dtcol["SENT_TO"].ColumnName = "Sent To";
                }
                if (dtcol.Contains("FINAL_ROUTE"))
                {
                    dtcol["FINAL_ROUTE"].ColumnName = "Final Route";
                }
                if (dtcol.Contains("CATEGORY_CODE"))
                {
                    dtcol["CATEGORY_CODE"].ColumnName = "Category";
                }
                if (dtcol.Contains("TASK_TYPE_NAME"))
                {
                    dtcol["TASK_TYPE_NAME"].ColumnName = "Type";
                }
                if (dtcol.Contains("TASK_SUBTYPES"))
                {
                    dtcol["TASK_SUBTYPES"].ColumnName = "Type Subtypes";
                }
                if (dtcol.Contains("PATIENT_ACCOUNT"))
                {
                    dtcol["PATIENT_ACCOUNT"].ColumnName = "Account #";
                }

                if (dtcol.Contains("PATIENT_FULL_NAME"))
                {
                    dtcol["PATIENT_FULL_NAME"].ColumnName = "Patient Name";
                }
                //if (dtcol.Contains("CASE_NO"))
                //{
                //    dtcol["CASE_NO"].ColumnName = "Case";
                //}
                if (dtcol.Contains("RT_CASE_NO"))
                {
                    dtcol["RT_CASE_NO"].ColumnName = "Case #";
                }
                if (dtcol.Contains("PROVIDER_FULL_NAME"))
                {
                    dtcol["PROVIDER_FULL_NAME"].ColumnName = "Provider";
                }
                if (dtcol.Contains("LOCATION_NAME"))
                {
                    dtcol["LOCATION_NAME"].ColumnName = "Location";
                }
                if (dtcol.Contains("PRIORITY"))
                {
                    dtcol["PRIORITY"].ColumnName = "Priority";
                }
                if (dtcol.Contains("DUE_DATE_TIME_STR"))
                {
                    dtcol["DUE_DATE_TIME_STR"].ColumnName = "Due On";
                }
                if (dtcol.Contains("REGION"))
                {
                    dtcol["REGION"].ColumnName = "Region";
                }
                if (dtcol.Contains("STATE"))
                {
                    dtcol["STATE"].ColumnName = "State";
                }
                if (dtcol.Contains("NO_OF_TIMES_MODIFIED"))
                {
                    dtcol["NO_OF_TIMES_MODIFIED"].ColumnName = "No. of Times Modified";
                }
                if (dtcol.Contains("MODIFIED_DATE"))
                {
                    dtcol["MODIFIED_DATE"].ColumnName = "Modified On";
                }
                if (dtcol.Contains("MODIFIED_BY_FULL_NAME"))
                {
                    dtcol["MODIFIED_BY_FULL_NAME"].ColumnName = "Modified By";
                }
            }
            else if (dt.TableName == "Task")
            {
                if (dtcol.Contains("ROW"))
                {
                    dtcol["ROW"].ColumnName = "Sr. #";
                }
                if (dtcol.Contains("CREATED_DATE_STR"))
                {
                    dtcol["CREATED_DATE_STR"].ColumnName = "Created Date";
                }
                if (dtcol.Contains("CREATED_TIME_STR"))
                {
                    dtcol["CREATED_TIME_STR"].ColumnName = "Create Time";
                }
                if (dtcol.Contains("CREATED_BY_FULL_NAME"))
                {
                    dtcol["CREATED_BY_FULL_NAME"].ColumnName = "Created By";
                }
                if (dtcol.Contains("SENT_TO"))
                {
                    dtcol["SENT_TO"].ColumnName = "Sent To";
                }
                if (dtcol.Contains("FINAL_ROUTE"))
                {
                    dtcol["FINAL_ROUTE"].ColumnName = "Final Route";
                }
                if (dtcol.Contains("CATEGORY_CODE"))
                {
                    dtcol["CATEGORY_CODE"].ColumnName = "Category";
                }
                if (dtcol.Contains("TASK_TYPE_NAME"))
                {
                    dtcol["TASK_TYPE_NAME"].ColumnName = "Type";
                }
                if (dtcol.Contains("TASK_SUBTYPES"))
                {
                    dtcol["TASK_SUBTYPES"].ColumnName = "Type Subtypes";
                }
                if (dtcol.Contains("PATIENT_ACCOUNT"))
                {
                    dtcol["PATIENT_ACCOUNT"].ColumnName = "Account #";
                }
                if (dtcol.Contains("PATIENT_FULL_NAME"))
                {
                    dtcol["PATIENT_FULL_NAME"].ColumnName = "Patient Name";
                }
                //if (dtcol.Contains("CASE_NO"))
                //{
                //    dtcol["CASE_NO"].ColumnName = "Case";
                //}
                if (dtcol.Contains("RT_CASE_NO"))
                {
                    dtcol["RT_CASE_NO"].ColumnName = "Case #";
                }
                if (dtcol.Contains("PROVIDER_FULL_NAME"))
                {
                    dtcol["PROVIDER_FULL_NAME"].ColumnName = "Provider";
                }
                if (dtcol.Contains("LOCATION_NAME"))
                {
                    dtcol["LOCATION_NAME"].ColumnName = "Location";
                }
                if (dtcol.Contains("PRIORITY"))
                {
                    dtcol["PRIORITY"].ColumnName = "Priority";
                }
                if (dtcol.Contains("DUE_DATE_TIME_STR"))
                {
                    dtcol["DUE_DATE_TIME_STR"].ColumnName = "Due On";
                }
                if (dtcol.Contains("REGION"))
                {
                    dtcol["REGION"].ColumnName = "Region";
                }
                if (dtcol.Contains("STATE"))
                {
                    dtcol["STATE"].ColumnName = "State";
                }
                if (dtcol.Contains("NO_OF_TIMES_MODIFIED"))
                {
                    dtcol["NO_OF_TIMES_MODIFIED"].ColumnName = "No. of Times Modified";
                }
                if (dtcol.Contains("MODIFIED_DATE"))
                {
                    dtcol["MODIFIED_DATE"].ColumnName = "Modified On";
                }
                if (dtcol.Contains("MODIFIED_BY_FULL_NAME"))
                {
                    dtcol["MODIFIED_BY_FULL_NAME"].ColumnName = "Modified By";
                }
            }
            else if (dt.TableName == "Patient_Documents")
            {
                if (dtcol.Contains("ROW"))
                {
                    dtcol["ROW"].ColumnName = "Sr. #";
                }
                if (dtcol.Contains("CASE_STATUS"))
                {
                    dtcol["CASE_STATUS"].ColumnName = "Case Status";
                }
                if (dtcol.Contains("WORK_ID"))
                {
                    dtcol["WORK_ID"].ColumnName = "Order Id";
                }
                //if (dtcol.Contains("CASE_NO"))
                //{
                //    dtcol["CASE_NO"].ColumnName = "Case";
                //}
                if (dtcol.Contains("COMMENTS"))
                {
                    dtcol["COMMENTS"].ColumnName = "Comments";
                }
                if (dtcol.Contains("RT_CASE_NO"))
                {
                    dtcol["RT_CASE_NO"].ColumnName = "Case #";
                }
                if (dtcol.Contains("START_DATE_str"))
                {
                    dtcol["START_DATE_str"].ColumnName = "Content Start Date";
                }
                if (dtcol.Contains("END_DATE_str"))
                {
                    dtcol["END_DATE_str"].ColumnName = "Content End Date";
                }
            }
            else if (dt.TableName == "Contact_Type")
            {
                if (dtcol.Contains("ROW"))
                {
                    dtcol["ROW"].ColumnName = "Sr. #";
                }
                if (dtcol.Contains("CODE"))
                {
                    dtcol["CODE"].ColumnName = "Code";
                }
                if (dtcol.Contains("Type_Name"))
                {
                    dtcol["Type_Name"].ColumnName = "Contact Type";
                }
                if (dtcol.Contains("Created_Date_Str"))
                {
                    dtcol["Created_Date_Str"].ColumnName = "Created Date";
                }

                if (dtcol.Contains("Modified_Date_Str"))
                {
                    dtcol["Modified_Date_Str"].ColumnName = "Modified Date";
                }
                if (dtcol.Contains("Inactive"))
                {
                    dtcol["Inactive"].ColumnName = "Inactive";
                }
            }
            else if (dt.TableName == "Document_Type")
            {
                if (dtcol.Contains("ROW"))
                {
                    dtcol["ROW"].ColumnName = "Sr. #";
                }
                if (dtcol.Contains("RT_CODE"))
                {
                    dtcol["RT_CODE"].ColumnName = "Code";
                }
                if (dtcol.Contains("NAME"))
                {
                    dtcol["NAME"].ColumnName = "Document Type";
                }
                if (dtcol.Contains("Created_Date_Str"))
                {
                    dtcol["Created_Date_Str"].ColumnName = "Created Date";
                }

                if (dtcol.Contains("Modified_Date_Str"))
                {
                    dtcol["Modified_Date_Str"].ColumnName = "Modified Date";
                }
                if (dtcol.Contains("Inactive"))
                {
                    dtcol["Inactive"].ColumnName = "Inactive";
                }
            }
            else if (dt.TableName == "Alert_Type")
            {
                if (dtcol.Contains("ROW"))
                {
                    dtcol["ROW"].ColumnName = "Sr. #";
                }
                if (dtcol.Contains("CODE"))
                {
                    dtcol["CODE"].ColumnName = "Code";
                }
                if (dtcol.Contains("DESCRIPTION"))
                {
                    dtcol["DESCRIPTION"].ColumnName = "Description";
                }
                if (dtcol.Contains("Created_Date_Str"))
                {
                    dtcol["Created_Date_Str"].ColumnName = "Created Date";
                }

                if (dtcol.Contains("Modified_Date_Str"))
                {
                    dtcol["Modified_Date_Str"].ColumnName = "Modified Date";
                }
                if (dtcol.Contains("Inactive"))
                {
                    dtcol["Inactive"].ColumnName = "Inactive";
                }
            }
            else if (dt.TableName == "Source_of_Referral")
            {
                if (dtcol.Contains("ROW"))
                {
                    dtcol["ROW"].ColumnName = "Sr. #";
                }
                if (dtcol.Contains("CODE"))
                {
                    dtcol["CODE"].ColumnName = "Code";
                }
                if (dtcol.Contains("DESCRIPTION"))
                {
                    dtcol["DESCRIPTION"].ColumnName = "Description";
                }
                if (dtcol.Contains("Created_Date_Str"))
                {
                    dtcol["Created_Date_Str"].ColumnName = "Created Date";
                }

                if (dtcol.Contains("Modified_Date_Str"))
                {
                    dtcol["Modified_Date_Str"].ColumnName = "Modified Date";
                }
                if (dtcol.Contains("Inactive"))
                {
                    dtcol["Inactive"].ColumnName = "Inactive";
                }
            }
            else if (dt.TableName == "Order_Status")
            {
                if (dtcol.Contains("ROW"))
                {
                    dtcol["ROW"].ColumnName = "Sr. #";
                }
                if (dtcol.Contains("CODE"))
                {
                    dtcol["CODE"].ColumnName = "Code";
                }
                if (dtcol.Contains("DESCRIPTION"))
                {
                    dtcol["DESCRIPTION"].ColumnName = "Description";
                }
                if (dtcol.Contains("Created_Date_Str"))
                {
                    dtcol["Created_Date_Str"].ColumnName = "Created Date";
                }

                if (dtcol.Contains("Modified_Date_Str"))
                {
                    dtcol["Modified_Date_Str"].ColumnName = "Modified Date";
                }
                if (dtcol.Contains("Inactive"))
                {
                    dtcol["Inactive"].ColumnName = "Inactive";
                }
            }
            else if (dt.TableName == "Task_Type")
            {
                if (dtcol.Contains("ROW"))
                {
                    dtcol["ROW"].ColumnName = "Sr. #";
                }
                if (dtcol.Contains("RT_CODE"))
                {
                    dtcol["RT_CODE"].ColumnName = "Code";
                }
                if (dtcol.Contains("NAME"))
                {
                    dtcol["NAME"].ColumnName = "Name";
                }
                if (dtcol.Contains("Created_Date_Str"))
                {
                    dtcol["Created_Date_Str"].ColumnName = "Created Date";
                }

                if (dtcol.Contains("Modified_Date_Str"))
                {
                    dtcol["Modified_Date_Str"].ColumnName = "Modified Date";
                }
                if (dtcol.Contains("Inactive"))
                {
                    dtcol["Inactive"].ColumnName = "Inactive";
                }
            }
            else if (dt.TableName == "Auth_Status")
            {
                if (dtcol.Contains("ROW"))
                {
                    dtcol["ROW"].ColumnName = "Sr. #";
                }
                if (dtcol.Contains("CODE"))
                {
                    dtcol["CODE"].ColumnName = "Code";
                }
                if (dtcol.Contains("DESCRIPTION"))
                {
                    dtcol["DESCRIPTION"].ColumnName = "Authorization Type";
                }
                if (dtcol.Contains("Created_Date_Str"))
                {
                    dtcol["Created_Date_Str"].ColumnName = "Created Date";
                }

                if (dtcol.Contains("Modified_Date_Str"))
                {
                    dtcol["Modified_Date_Str"].ColumnName = "Modified Date";
                }
                if (dtcol.Contains("Inactive"))
                {
                    dtcol["Inactive"].ColumnName = "Inactive";
                }
            }
            else if (dt.TableName == "Identifier_Type")
            {
                if (dtcol.Contains("ROW"))
                {
                    dtcol["ROW"].ColumnName = "Sr. #";
                }
                if (dtcol.Contains("NAME"))
                {
                    dtcol["NAME"].ColumnName = "Name";
                }
                if (dtcol.Contains("CODE"))
                {
                    dtcol["CODE"].ColumnName = "Code";
                }
                if (dtcol.Contains("IDENTIFIER_TYPE_NAME"))
                {
                    dtcol["IDENTIFIER_TYPE_NAME"].ColumnName = "Identifier Type";
                }
                if (dtcol.Contains("Created_Date_Str"))
                {
                    dtcol["Created_Date_Str"].ColumnName = "Created Date";
                }

                if (dtcol.Contains("Modified_Date_Str"))
                {
                    dtcol["Modified_Date_Str"].ColumnName = "Modified Date";
                }
                if (dtcol.Contains("Inactive"))
                {
                    dtcol["Inactive"].ColumnName = "Inactive";
                }
            }
            else if (dt.TableName == "Location_Corporation")
            {
                if (dtcol.Contains("ROW"))
                {
                    dtcol["ROW"].ColumnName = "Sr. #";
                }
                if (dtcol.Contains("NAME"))
                {
                    dtcol["NAME"].ColumnName = "Corporation Name";
                }
                if (dtcol.Contains("CODE"))
                {
                    dtcol["CODE"].ColumnName = "Code";
                }
                if (dtcol.Contains("Created_Date_Str"))
                {
                    dtcol["Created_Date_Str"].ColumnName = "Created Date";
                }

                if (dtcol.Contains("Modified_Date_Str"))
                {
                    dtcol["Modified_Date_Str"].ColumnName = "Modified Date";
                }
                if (dtcol.Contains("Inactive"))
                {
                    dtcol["Inactive"].ColumnName = "Inactive";
                }
            }
            else if (dt.TableName == "Group_Identifier")
            {
                if (dtcol.Contains("ROW"))
                {
                    dtcol["ROW"].ColumnName = "Sr. #";
                }
                if (dtcol.Contains("NAME"))
                {
                    dtcol["NAME"].ColumnName = "Code";
                }
                if (dtcol.Contains("DESCRIPTION"))
                {
                    dtcol["DESCRIPTION"].ColumnName = "Identifier Name";
                }
                if (dtcol.Contains("Created_Date_Str"))
                {
                    dtcol["Created_Date_Str"].ColumnName = "Created Date";
                }

                if (dtcol.Contains("Modified_Date_Str"))
                {
                    dtcol["Modified_Date_Str"].ColumnName = "Modified Date";
                }
                if (dtcol.Contains("Inactive"))
                {
                    dtcol["Inactive"].ColumnName = "Inactive";
                }
            }
            else if (dt.TableName == "Facility_Setup")
            {
                if (dtcol.Contains("ROW"))
                {
                    dtcol["ROW"].ColumnName = "Sr. #";
                }
                if (dtcol.Contains("CODE"))
                {
                    dtcol["CODE"].ColumnName = "Code";
                }
                if (dtcol.Contains("NAME"))
                {
                    dtcol["NAME"].ColumnName = "Facility Name";
                }
                if (dtcol.Contains("COMPLETE_ADDRESS"))
                {
                    dtcol["COMPLETE_ADDRESS"].ColumnName = "Address";
                }
                if (dtcol.Contains("Country"))
                {
                    dtcol["Country"].ColumnName = "County";
                }
                if (dtcol.Contains("REGION"))
                {
                    dtcol["REGION"].ColumnName = "Region";
                }
                if (dtcol.Contains("Phone"))
                {
                    dtcol["Phone"].ColumnName = "Phone";
                }
                if (dtcol.Contains("Fax"))
                {
                    dtcol["Fax"].ColumnName = "Fax";
                }
                if (dtcol.Contains("POS_Code"))
                {
                    dtcol["POS_Code"].ColumnName = "POS";
                }
                if (dtcol.Contains("Capacity"))
                {
                    dtcol["Capacity"].ColumnName = "Capacity";
                }
                if (dtcol.Contains("Census"))
                {
                    dtcol["Census"].ColumnName = "Census";
                }
                if (dtcol.Contains("PT"))
                {
                    dtcol["PT"].ColumnName = "PT";
                }
                if (dtcol.Contains("OT"))
                {
                    dtcol["OT"].ColumnName = "OT";
                }
                if (dtcol.Contains("ST"))
                {
                    dtcol["ST"].ColumnName = "ST";
                }
                if (dtcol.Contains("EP"))
                {
                    dtcol["EP"].ColumnName = "EP";
                }
                if (dtcol.Contains("Lead"))
                {
                    dtcol["Lead"].ColumnName = "Lead";
                }
                if (dtcol.Contains("Parent"))
                {
                    dtcol["Parent"].ColumnName = "Parent Code";
                }
                if (dtcol.Contains("Description"))
                {
                    dtcol["Description"].ColumnName = "Parent Description";
                }
                if (dtcol.Contains("Created_Date_Str"))
                {
                    dtcol["Created_Date_Str"].ColumnName = "Created Date";
                }
                if (dtcol.Contains("Modified_Date_Str"))
                {
                    dtcol["Modified_Date_Str"].ColumnName = "Modified Date";
                }
                if (dtcol.Contains("Inactive"))
                {
                    dtcol["Inactive"].ColumnName = "Inactive";
                }
            }
            else if (dt.TableName == "Referral_Region")
            {
                if (dtcol.Contains("ROW"))
                {
                    dtcol["ROW"].ColumnName = "Sr. #";
                }
                if (dtcol.Contains("REFERRAL_REGION_CODE"))
                {
                    dtcol["REFERRAL_REGION_CODE"].ColumnName = "Region Code";
                }
                if (dtcol.Contains("REFERRAL_REGION_NAME"))
                {
                    dtcol["REFERRAL_REGION_NAME"].ColumnName = "Region Name";
                }
                if (dtcol.Contains("ACCOUNT_MANAGER"))
                {
                    dtcol["ACCOUNT_MANAGER"].ColumnName = "Account Manager";
                }
                if (dtcol.Contains("ACCOUNT_MANAGER_EMAIL"))
                {
                    dtcol["ACCOUNT_MANAGER_EMAIL"].ColumnName = "Account Manager Email";
                }
                //if (dtcol.Contains("REGIONAL_DIRECTOR"))
                //{
                //    dtcol["REGIONAL_DIRECTOR"].ColumnName = "Regional Director";
                //}
                //if (dtcol.Contains("SENIOR_REGIONAL_DIRECTOR"))
                //{
                //    dtcol["SENIOR_REGIONAL_DIRECTOR"].ColumnName = "Senior Regional Director";
                //}
                if (dtcol.Contains("Seleced_Counties"))
                {
                    dtcol["Seleced_Counties"].ColumnName = "Selected_Counties";
                }
                if (dtcol.Contains("Inactive"))
                {
                    dtcol["Inactive"].ColumnName = "Inactive";
                }
            }
            else if (dt.TableName == "Practice_Organization")
            {
                if (dtcol.Contains("ACTIVEROW"))
                {
                    dtcol["ACTIVEROW"].ColumnName = "Sr. #";
                }
                if (dtcol.Contains("CODE"))
                {
                    dtcol["CODE"].ColumnName = "Code";
                }
                if (dtcol.Contains("NAME"))
                {
                    dtcol["NAME"].ColumnName = "Name";
                }
                if (dtcol.Contains("ZIP"))
                {
                    dtcol["ZIP"].ColumnName = "ZIP Code";
                }
                if (dtcol.Contains("CITY"))
                {
                    dtcol["CITY"].ColumnName = "City";
                }
                if (dtcol.Contains("STATE"))
                {
                    dtcol["STATE"].ColumnName = "State";
                }
                if (dtcol.Contains("ADDRESS"))
                {
                    dtcol["ADDRESS"].ColumnName = "Address";
                }
                if (dtcol.Contains("Inactive"))
                {
                    dtcol["Inactive"].ColumnName = "Inactive";
                }
            }
            else if (dt.TableName == "Referral_Source")
            {
                if (dtcol.Contains("ROW"))
                {
                    dtcol["ROW"].ColumnName = "Sr. #";
                }
                if (dtcol.Contains("CODE"))
                {
                    dtcol["CODE"].ColumnName = "Source Code";
                }
                if (dtcol.Contains("FIRST_NAME"))
                {
                    dtcol["FIRST_NAME"].ColumnName = "First Name";
                }
                if (dtcol.Contains("LAST_NAME"))
                {
                    dtcol["LAST_NAME"].ColumnName = "Last Name";
                }
                if (dtcol.Contains("MI"))
                {
                    dtcol["MI"].ColumnName = "MI";
                }
                if (dtcol.Contains("EMAIL"))
                {
                    dtcol["EMAIL"].ColumnName = "Email";
                }
                if (dtcol.Contains("ADDRESS"))
                {
                    dtcol["ADDRESS"].ColumnName = "Address 1";
                }
                if (dtcol.Contains("ADDRESS_2"))
                {
                    dtcol["ADDRESS_2"].ColumnName = "Address 2";
                }
                if (dtcol.Contains("CITY"))
                {
                    dtcol["CITY"].ColumnName = "City";
                }
                if (dtcol.Contains("STATE"))
                {
                    dtcol["STATE"].ColumnName = "State";
                }
                if (dtcol.Contains("ZIP"))
                {
                    dtcol["ZIP"].ColumnName = "ZIP Code";
                }
                if (dtcol.Contains("Speciality"))
                {
                    dtcol["Speciality"].ColumnName = "Speciality";
                }
                if (dtcol.Contains("TITLE"))
                {
                    dtcol["TITLE"].ColumnName = "Credentials";
                }
                if (dtcol.Contains("REFERRAL_REGION"))
                {
                    dtcol["REFERRAL_REGION"].ColumnName = "Region";
                }
                if (dtcol.Contains("NPI"))
                {
                    dtcol["NPI"].ColumnName = "NPI";
                }
                if (dtcol.Contains("ACO_NAME"))
                {
                    dtcol["ACO_NAME"].ColumnName = "ACO";
                }
                if (dtcol.Contains("PRACTICE_ORGANIZATION_NAME"))
                {
                    dtcol["PRACTICE_ORGANIZATION_NAME"].ColumnName = "Organization";
                }
                if (dtcol.Contains("PHONE"))
                {
                    dtcol["PHONE"].ColumnName = "Phone";
                }
                if (dtcol.Contains("FAX"))
                {
                    dtcol["FAX"].ColumnName = "Fax";
                }
                if (dtcol.Contains("CELL"))
                {
                    dtcol["CELL"].ColumnName = "Cell";
                }
                if (dtcol.Contains("Created_Date_Str"))
                {
                    dtcol["Created_Date_Str"].ColumnName = "Created Date";
                }
                if (dtcol.Contains("Modified_Date_Str"))
                {
                    dtcol["Modified_Date_Str"].ColumnName = "Modified Date";
                }
                if (dtcol.Contains("INACTIVE_REASON"))
                {
                    dtcol["INACTIVE_REASON"].ColumnName = "Inactive Reason";
                }
                if (dtcol.Contains("Inactive"))
                {
                    dtcol["Inactive"].ColumnName = "Inactive";
                }
            }
            else if (dt.TableName == "Original_Queue")
            {
                if (dtcol.Contains("ROW"))
                {
                    dtcol["ROW"].ColumnName = "Sr. #";
                }
                if (dtcol.Contains("SORCE_TYPE"))
                {
                    dtcol["SORCE_TYPE"].ColumnName = "Source Type";
                }
                if (dtcol.Contains("SORCE_NAME"))
                {
                    dtcol["SORCE_NAME"].ColumnName = "Source";
                }
                if (dtcol.Contains("WORK_STATUS"))
                {
                    dtcol["WORK_STATUS"].ColumnName = "Status";
                }
                if (dtcol.Contains("Received_Date_Str"))
                {
                    dtcol["Received_Date_Str"].ColumnName = "Date/Time Received";
                }
                if (dtcol.Contains("TOTAL_PAGES"))
                {
                    dtcol["TOTAL_PAGES"].ColumnName = "Pages";
                }
                if (dtcol.Contains("NO_OF_SPLITS"))
                {
                    dtcol["NO_OF_SPLITS"].ColumnName = "No. of Splits";
                }
            }
            else if (dt.TableName == "Unassigned_Queue")
            {
                if (dtcol.Contains("ROW"))
                {
                    dtcol["ROW"].ColumnName = "Sr. #";
                }
                if (dtcol.Contains("UNIQUE_ID"))
                {
                    dtcol["UNIQUE_ID"].ColumnName = "ID";
                }
                if (dtcol.Contains("PATIENT_FIRST_NAME"))
                {
                    dtcol["PATIENT_FIRST_NAME"].ColumnName = "Patient First Name";
                }
                if (dtcol.Contains("PATIENT_LAST_NAME"))
                {
                    dtcol["PATIENT_LAST_NAME"].ColumnName = "Patient Last Name";
                }
                if (dtcol.Contains("MRN"))
                {
                    dtcol["MRN"].ColumnName = "MRN";
                }
                if (dtcol.Contains("SORCE_TYPE"))
                {
                    dtcol["SORCE_TYPE"].ColumnName = "Source Type";
                }
                if (dtcol.Contains("SORCE_NAME"))
                {
                    dtcol["SORCE_NAME"].ColumnName = "Source";
                }
                if (dtcol.Contains("Received_Date_Str"))
                {
                    dtcol["Received_Date_Str"].ColumnName = "Date/Time Received";
                }
                if (dtcol.Contains("ElapseTime"))
                {
                    dtcol["ElapseTime"].ColumnName = "Elapsed Unassigned Time (HH:MM)";
                }
                if (dtcol.Contains("TOTAL_PAGES"))
                {
                    dtcol["TOTAL_PAGES"].ColumnName = "Pages";
                }
            }
            else if (dt.TableName == "Assigned_Queue")
            {
                if (dtcol.Contains("ROW"))
                {
                    dtcol["ROW"].ColumnName = "Sr. #";
                }
                if (dtcol.Contains("UNIQUE_ID"))
                {
                    dtcol["UNIQUE_ID"].ColumnName = "ID";
                }
                if (dtcol.Contains("PATIENT_FIRST_NAME"))
                {
                    dtcol["PATIENT_FIRST_NAME"].ColumnName = "Patient First Name";
                }
                if (dtcol.Contains("PATIENT_LAST_NAME"))
                {
                    dtcol["PATIENT_LAST_NAME"].ColumnName = "Patient Last Name";
                }
                if (dtcol.Contains("MRN"))
                {
                    dtcol["MRN"].ColumnName = "MRN";
                }
                if (dtcol.Contains("ASSIGNED_TO"))
                {
                    dtcol["ASSIGNED_TO"].ColumnName = "Assigned To";
                }
                if (dtcol.Contains("SORCE_TYPE"))
                {
                    dtcol["SORCE_TYPE"].ColumnName = "Source Type";
                }
                if (dtcol.Contains("SORCE_NAME"))
                {
                    dtcol["SORCE_NAME"].ColumnName = "Source";
                }
                if (dtcol.Contains("Received_Date_Str"))
                {
                    dtcol["Received_Date_Str"].ColumnName = "Date/Time Received";
                }
                if (dtcol.Contains("TIME_ELASPE_IN_QUEUE"))
                {
                    dtcol["TIME_ELASPE_IN_QUEUE"].ColumnName = "Elapsed Queue Time (HH:MM)";
                }
                if (dtcol.Contains("TOTAL_PAGES"))
                {
                    dtcol["TOTAL_PAGES"].ColumnName = "Pages";
                }
                if (dtcol.Contains("ASSIGNED_BY"))
                {
                    dtcol["ASSIGNED_BY"].ColumnName = "Assigned By";
                }
                if (dtcol.Contains("MODIFIED_BY"))
                {
                    dtcol["MODIFIED_BY"].ColumnName = "Updated By";
                }
                if (dtcol.Contains("Modified_Date_Str"))
                {
                    dtcol["Modified_Date_Str"].ColumnName = "Date/Time Updated";
                }
                if (dtcol.Contains("Assigned_Date_Str"))
                {
                    dtcol["Assigned_Date_Str"].ColumnName = "Date/Time Assigned";
                }
                if (dtcol.Contains("WORK_STATUS"))
                {
                    dtcol["WORK_STATUS"].ColumnName = "Status";
                }
            }
            else if (dt.TableName == "Indexed_Queue")
            {
                if (dtcol.Contains("ROW"))
                {
                    dtcol["ROW"].ColumnName = "Sr. #";
                }
                if (dtcol.Contains("UNIQUE_ID"))
                {
                    dtcol["UNIQUE_ID"].ColumnName = "ID";
                }
                if (dtcol.Contains("INDEXED_BY"))
                {
                    dtcol["INDEXED_BY"].ColumnName = "Indexed By";
                }
                if (dtcol.Contains("ASSIGNED_TO"))
                {
                    dtcol["ASSIGNED_TO"].ColumnName = "Assign To";
                }
                if (dtcol.Contains("SORCE_TYPE"))
                {
                    dtcol["SORCE_TYPE"].ColumnName = "Source Type";
                }
                if (dtcol.Contains("SORCE_NAME"))
                {
                    dtcol["SORCE_NAME"].ColumnName = "Source Name";
                }
                if (dtcol.Contains("DOCUMENT_TYPE"))
                {
                    dtcol["DOCUMENT_TYPE"].ColumnName = "Document Type";
                }
                if (dtcol.Contains("Received_Date_Str"))
                {
                    dtcol["Received_Date_Str"].ColumnName = "Date/Time Received";
                }
                if (dtcol.Contains("TIME_ELASPE_IN_QUEUE"))
                {
                    dtcol["TIME_ELASPE_IN_QUEUE"].ColumnName = "Elapsed Queue Time (HH:MM)";
                }
                if (dtcol.Contains("INDEX_COMPLETION_TIME"))
                {
                    dtcol["INDEX_COMPLETION_TIME"].ColumnName = "Index Completion Time (HH:MM)";
                }
                if (dtcol.Contains("TOTAL_PAGES"))
                {
                    dtcol["TOTAL_PAGES"].ColumnName = "Pages";
                }
            }
            else if (dt.TableName == "Completed_Queue")
            {
                if (dtcol.Contains("ROW"))
                {
                    dtcol["ROW"].ColumnName = "Sr. #";
                }
                if (dtcol.Contains("UNIQUE_ID"))
                {
                    dtcol["UNIQUE_ID"].ColumnName = "ID";
                }
                if (dtcol.Contains("FIRST_NAME"))
                {
                    dtcol["FIRST_NAME"].ColumnName = "Patient First Name";
                }
                if (dtcol.Contains("LAST_NAME"))
                {
                    dtcol["LAST_NAME"].ColumnName = "Patient Last Name";
                }
                if (dtcol.Contains("Date_Of_Birth"))
                {
                    dtcol["Date_Of_Birth"].ColumnName = "DOB";
                }
                if (dtcol.Contains("MEDICAL_RECORD_NUMBER"))
                {
                    dtcol["MEDICAL_RECORD_NUMBER"].ColumnName = "MRN";
                }
                if (dtcol.Contains("ASSIGNED_TO"))
                {
                    dtcol["ASSIGNED_TO"].ColumnName = "Assigned To";
                }
                if (dtcol.Contains("SORCE_TYPE"))
                {
                    dtcol["SORCE_TYPE"].ColumnName = "Source Type";
                }
                if (dtcol.Contains("SORCE_NAME"))
                {
                    dtcol["SORCE_NAME"].ColumnName = "Source";
                }
                if (dtcol.Contains("ACCOUNT_NUMBER"))
                {
                    dtcol["ACCOUNT_NUMBER"].ColumnName = "Account #";
                }
                if (dtcol.Contains("UNIT_CASE_NO"))
                {
                    dtcol["UNIT_CASE_NO"].ColumnName = "Unit/Case #";
                }
                if (dtcol.Contains("DOCUMENT_TYPE"))
                {
                    dtcol["DOCUMENT_TYPE"].ColumnName = "Document Type";
                }
                if (dtcol.Contains("SENDER_NAME"))
                {
                    dtcol["SENDER_NAME"].ColumnName = "Sending Party";
                }
                if (dtcol.Contains("TOTAL_PAGES"))
                {
                    dtcol["TOTAL_PAGES"].ColumnName = "Pages";
                }
                if (dtcol.Contains("Received_Date_Str"))
                {
                    dtcol["Received_Date_Str"].ColumnName = "Date/Time Received";
                }
                if (dtcol.Contains("ElapseCompletionTime"))
                {
                    dtcol["ElapseCompletionTime"].ColumnName = "Elapsed Completion Time(HH:MM)";
                }
                if (dtcol.Contains("ElapseAssignTime"))
                {
                    dtcol["ElapseAssignTime"].ColumnName = "Elapsed Assigned Time(HH:MM)";
                }
                if (dtcol.Contains("COMPLETED_BY"))
                {
                    dtcol["COMPLETED_BY"].ColumnName = "Completed By";
                }
                if (dtcol.Contains("Completed_Date_Str"))
                {
                    dtcol["Completed_Date_Str"].ColumnName = "Completed Date/Time";
                }
                if (dtcol.Contains("ASSIGNED_BY"))
                {
                    dtcol["ASSIGNED_BY"].ColumnName = "Assigned By";
                }
                if (dtcol.Contains("Assigned_Date_Str"))
                {
                    dtcol["Assigned_Date_Str"].ColumnName = "Assigned Date/Time";
                }
                if (dtcol.Contains("MODIFIED_BY"))
                {
                    dtcol["MODIFIED_BY"].ColumnName = "Updated By";
                }
                if (dtcol.Contains("Modified_Date_Str"))
                {
                    dtcol["Modified_Date_Str"].ColumnName = " Updated Date/ Time";
                }
            }
            else if (dt.TableName == "Supervisor_Queue")
            {
                if (dtcol.Contains("ROW"))
                {
                    dtcol["ROW"].ColumnName = "Sr. #";
                }
                if (dtcol.Contains("UNIQUE_ID"))
                {
                    dtcol["UNIQUE_ID"].ColumnName = "ID";
                }
                if (dtcol.Contains("SUPERVISOR_NAME"))
                {
                    dtcol["SUPERVISOR_NAME"].ColumnName = "Supervisor Name";
                }
                if (dtcol.Contains("TRANSFER_REASON"))
                {
                    dtcol["TRANSFER_REASON"].ColumnName = "Transfer Reason";
                }
                if (dtcol.Contains("SORCE_TYPE"))
                {
                    dtcol["SORCE_TYPE"].ColumnName = "Source Type";
                }
                if (dtcol.Contains("SORCE_NAME"))
                {
                    dtcol["SORCE_NAME"].ColumnName = "Source";
                }
                if (dtcol.Contains("WORK_STATUS"))
                {
                    dtcol["WORK_STATUS"].ColumnName = "Status";
                }
                if (dtcol.Contains("ASSIGNED_BY"))
                {
                    dtcol["ASSIGNED_BY"].ColumnName = "Assigned By";
                }
                if (dtcol.Contains("Assigned_Date_Str"))
                {
                    dtcol["Assigned_Date_Str"].ColumnName = "Assigned Date";
                }
                if (dtcol.Contains("TOTAL_PAGES"))
                {
                    dtcol["TOTAL_PAGES"].ColumnName = "Pages";
                }
                if (dtcol.Contains("Received_Date_Str"))
                {
                    dtcol["Received_Date_Str"].ColumnName = "Recieved Date";
                }
                if (dtcol.Contains("ElapseTime"))
                {
                    dtcol["ElapseTime"].ColumnName = "Elapsed Queue";
                }
            }
            else if (dt.TableName == "Search_Order")
            {
                if (dtcol.Contains("ROW"))
                {
                    dtcol["ROW"].ColumnName = "Sr. #";
                }
                if (dtcol.Contains("UNIQUE_ID"))
                {
                    dtcol["UNIQUE_ID"].ColumnName = "ID";
                }
                if (dtcol.Contains("Patient_Account"))
                {
                    dtcol["Patient_Account"].ColumnName = "Account #";
                }
                if (dtcol.Contains("First_Name"))
                {
                    dtcol["First_Name"].ColumnName = "Patient First Name";
                }
                if (dtcol.Contains("Last_Name"))
                {
                    dtcol["Last_Name"].ColumnName = "Patient Last Name";
                }
                if (dtcol.Contains("SSN"))
                {
                    dtcol["SSN"].ColumnName = "SSN";
                }
                if (dtcol.Contains("Date_of_Birth_Str"))
                {
                    dtcol["Date_of_Birth_Str"].ColumnName = "Date of Birth";
                }
                if (dtcol.Contains("SENDER_FIRST_NAME"))
                {
                    dtcol["SENDER_FIRST_NAME"].ColumnName = "Sender First Name";
                }
                if (dtcol.Contains("SENDER_LAST_NAME"))
                {
                    dtcol["SENDER_LAST_NAME"].ColumnName = " Sender Last Name";
                }
                if (dtcol.Contains("SOURCE_NAME"))
                {
                    dtcol["SOURCE_NAME"].ColumnName = "Source Type";
                }
                if (dtcol.Contains("DOCUMENT_TYPE"))
                {
                    dtcol["DOCUMENT_TYPE"].ColumnName = "Document Type";
                }
                if (dtcol.Contains("REGION_NAME"))
                {
                    dtcol["REGION_NAME"].ColumnName = "Region";
                }
                if (dtcol.Contains("Received_Date_Str"))
                {
                    dtcol["Received_Date_Str"].ColumnName = "Date/Time Received";
                }
                if (dtcol.Contains("WORK_STATUS"))
                {
                    dtcol["WORK_STATUS"].ColumnName = "Status";
                }
                if (dtcol.Contains("Completed_Date_Str"))
                {
                    dtcol["Completed_Date_Str"].ColumnName = "Completed Date/Time ";
                }
            }
            else if (dt.TableName == "Referral_Report")
            {
                if (dtcol.Contains("ROW"))
                {
                    dtcol["ROW"].ColumnName = "Sr. #";
                }
                if (dtcol.Contains("WORK_ID"))
                {
                    dtcol["WORK_ID"].ColumnName = "Work ID";
                }
                if (dtcol.Contains("PRIORITY"))
                {
                    dtcol["PRIORITY"].ColumnName = "Priority";
                }
                if (dtcol.Contains("DOCUMENT_TYPE"))
                {
                    dtcol["DOCUMENT_TYPE"].ColumnName = "Document Type";
                }
                if (dtcol.Contains("INDEXING_STATUS"))
                {
                    dtcol["INDEXING_STATUS"].ColumnName = "Indexing Status";
                }
                if (dtcol.Contains("INDEXED_BY"))
                {
                    dtcol["INDEXED_BY"].ColumnName = "Indexed By";
                }
                if (dtcol.Contains("Indexed_Date_Str"))
                {
                    dtcol["Indexed_Date_Str"].ColumnName = "Indexed Date & Time";
                }
                if (dtcol.Contains("SOURCE_TYPE"))
                {
                    dtcol["SOURCE_TYPE"].ColumnName = "Source Type";
                }
                if (dtcol.Contains("Received_Date_Str"))
                {
                    dtcol["Received_Date_Str"].ColumnName = "Received Date & Time";
                }
                if (dtcol.Contains("ROLE_NAME"))
                {
                    dtcol["ROLE_NAME"].ColumnName = "Assigned Person Role";
                }
                if (dtcol.Contains("ASSIGNED_TO"))
                {
                    dtcol["ASSIGNED_TO"].ColumnName = "Assigned Person Name";
                }
                if (dtcol.Contains("WORK_STATUS"))
                {
                    dtcol["WORK_STATUS"].ColumnName = "Status";
                }
                if (dtcol.Contains("COMPLETED_BY"))
                {
                    dtcol["COMPLETED_BY"].ColumnName = "Completed By";
                }
                if (dtcol.Contains("Completed_Date_Str"))
                {
                    dtcol["Completed_Date_Str"].ColumnName = "Completed Date & Time ";
                }
            }
            else if (dt.TableName == "Detailed_Survey")
            {
                if (dtcol.Contains("MONTH"))
                {
                    dtcol["MONTH"].ColumnName = "Month";
                }
                if (dtcol.Contains("ROW"))
                {
                    dtcol["ROW"].ColumnName = "Sr. #";
                }
                if (dtcol.Contains("SURVEY_FLAG"))
                {
                    dtcol["SURVEY_FLAG"].ColumnName = "Flag";
                }
                if (dtcol.Contains("PATIENT_ACCOUNT_NUMBER"))
                {
                    dtcol["PATIENT_ACCOUNT_NUMBER"].ColumnName = "Patient Acc. No";
                }
                if (dtcol.Contains("PATIENT_FULL_NAME"))
                {
                    dtcol["PATIENT_FULL_NAME"].ColumnName = "Patient Name";
                }
                if (dtcol.Contains("PATIENT_STATE"))
                {
                    dtcol["PATIENT_STATE"].ColumnName = "State";
                }
                if (dtcol.Contains("PT_OT_SLP"))
                {
                    dtcol["PT_OT_SLP"].ColumnName = "Discipline";
                }
                if (dtcol.Contains("REGION"))
                {
                    dtcol["REGION"].ColumnName = "Region";
                }
                if (dtcol.Contains("PROVIDER"))
                {
                    dtcol["PROVIDER"].ColumnName = "Provider";
                }
                if (dtcol.Contains("ATTENDING_DOCTOR_NAME"))
                {
                    dtcol["ATTENDING_DOCTOR_NAME"].ColumnName = "Attending Doctor";
                }
                if (dtcol.Contains("SURVEY_STATUS_CHILD"))
                {
                    dtcol["SURVEY_STATUS_CHILD"].ColumnName = "Survey Status";
                }
                if (dtcol.Contains("FEEDBACK"))
                {
                    dtcol["FEEDBACK"].ColumnName = "Survey Comments";
                }
                if (dtcol.Contains("SURVEY_STATUS_BASE"))
                {
                    dtcol["SURVEY_STATUS_BASE"].ColumnName = "Survey Completion Status";
                }
                if (dtcol.Contains("SURVEY_COMPLETED_TIME_STR"))
                {
                    dtcol["SURVEY_COMPLETED_TIME_STR"].ColumnName = "Survey Time";
                }
                if (dtcol.Contains("SURVEY_COMPLETED_DATE_STR"))
                {
                    dtcol["SURVEY_COMPLETED_DATE_STR"].ColumnName = "Survey Date";
                }
                //if (dtcol.Contains("MODIFIED_DATE"))
                //{
                //    dtcol["MODIFIED_DATE"].ColumnName = "Pivot Date/Time";
                //}
                if (dtcol.Contains("Surveyed_BY_FULL_NAME"))
                {
                    dtcol["Surveyed_BY_FULL_NAME"].ColumnName = "Surveyed By";
                }
                if (dtcol.Contains("Is_improved_Satisfaction_Str"))
                {
                    dtcol["Is_improved_Satisfaction_Str"].ColumnName = "Is Improved Satisfaction";
                }
                if (dtcol.Contains("Is_Referrable_Str"))
                {
                    dtcol["Is_Referrable_Str"].ColumnName = "Is Referrable";
                }
                if (dtcol.Contains("Is_Contact_HQ_Str"))
                {
                    dtcol["Is_Contact_HQ_Str"].ColumnName = "Is Contact Headquarter";
                }
                if (dtcol.Contains("Is_Responsed_By_HQ_Str"))
                {
                    dtcol["Is_Responsed_By_HQ_Str"].ColumnName = "Is Responsed by Headquarter";
                }
                if (dtcol.Contains("Is_Questioned_Answered_Str"))
                {
                    dtcol["Is_Questioned_Answered_Str"].ColumnName = "Complaint Resolved";
                }
                if (dtcol.Contains("Is_protective_equipment_Str"))
                {
                    dtcol["Is_protective_equipment_Str"].ColumnName = "Protective Equipment";
                }
                if (dtcol.Contains("SURVEY_COMPLETED_DATE"))
                {
                    dtcol["SURVEY_COMPLETED_DATE"].ColumnName = "Survey Completed Date";
                }
            }
            else if (dt.TableName == "Detailed_Survey_old")
            {
                if (dtcol.Contains("ROW"))
                {
                    dtcol["ROW"].ColumnName = "Sr. #";
                }
                if (dtcol.Contains("PATIENT_ACCOUNT_NUMBER"))
                {
                    dtcol["PATIENT_ACCOUNT_NUMBER"].ColumnName = "Patient Acc. No";
                }
                if (dtcol.Contains("PATIENT_FULL_NAME"))
                {
                    dtcol["PATIENT_FULL_NAME"].ColumnName = "Patient Name";
                }
                if (dtcol.Contains("PATIENT_STATE"))
                {
                    dtcol["PATIENT_STATE"].ColumnName = "State";
                }
                if (dtcol.Contains("PT_OT_SLP"))
                {
                    dtcol["PT_OT_SLP"].ColumnName = "Discipline";
                }
                if (dtcol.Contains("REGION"))
                {
                    dtcol["REGION"].ColumnName = "Region";
                }
                if (dtcol.Contains("PROVIDER"))
                {
                    dtcol["PROVIDER"].ColumnName = "Provider";
                }
                if (dtcol.Contains("ATTENDING_DOCTOR_NAME"))
                {
                    dtcol["ATTENDING_DOCTOR_NAME"].ColumnName = "Attending Doctor";
                }
                if (dtcol.Contains("SURVEY_STATUS_CHILD"))
                {
                    dtcol["SURVEY_STATUS_CHILD"].ColumnName = "Survey Status";
                }
                if (dtcol.Contains("FEEDBACK"))
                {
                    dtcol["FEEDBACK"].ColumnName = "Survey Comments";
                }
                if (dtcol.Contains("SURVEY_STATUS_BASE"))
                {
                    dtcol["SURVEY_STATUS_BASE"].ColumnName = "Survey Completion Status";
                }
                if (dtcol.Contains("SURVEY_COMPLETED_TIME_STR"))
                {
                    dtcol["SURVEY_COMPLETED_TIME_STR"].ColumnName = "Survey Time";
                }
                if (dtcol.Contains("SURVEY_COMPLETED_DATE_STR"))
                {
                    dtcol["SURVEY_COMPLETED_DATE_STR"].ColumnName = "Survey Date";
                }
                if (dtcol.Contains("Surveyed_BY_FULL_NAME"))
                {
                    dtcol["Surveyed_BY_FULL_NAME"].ColumnName = "Surveyed By";
                }
                if (dtcol.Contains("Is_improved_Satisfaction_Str"))
                {
                    dtcol["Is_improved_Satisfaction_Str"].ColumnName = "Condition Improved";
                }
                if (dtcol.Contains("Is_Referrable_Str"))
                {
                    dtcol["Is_Referrable_Str"].ColumnName = "Recommend Our Practice";
                }
                if (dtcol.Contains("Is_Contact_HQ_Str"))
                {
                    dtcol["Is_Contact_HQ_Str"].ColumnName = "Call to HQ";
                }
                if (dtcol.Contains("Is_Responsed_By_HQ_Str"))
                {
                    dtcol["Is_Responsed_By_HQ_Str"].ColumnName = "Response from HQ";
                }
                if (dtcol.Contains("Is_protective_equipment_Str"))
                {
                    dtcol["Is_protective_equipment_Str"].ColumnName = "Protective Equipment";
                }
            }
            else if (dt.TableName == "Interface_Logs")
            {
                if (dtcol.Contains("ROW"))
                {
                    dtcol["ROW"].ColumnName = "Sr. #";
                }
                if (dtcol.Contains("CREATED_BY"))
                {
                    dtcol["CREATED_BY"].ColumnName = "Created By";
                }
                if (dtcol.Contains("Created_Date_Str"))
                {
                    dtcol["Created_Date_Str"].ColumnName = "Date & Time";
                }
                if (dtcol.Contains("ERROR"))
                {
                    dtcol["ERROR"].ColumnName = "Description";
                }
                if (dtcol.Contains("ACK"))
                {
                    dtcol["ACK"].ColumnName = "Acknowledge By";
                }
                if (dtcol.Contains("LOG_MESSAGE"))
                {
                    dtcol["LOG_MESSAGE"].ColumnName = "Message";
                }
                if (dtcol.Contains("Work_ID"))
                {
                    dtcol["Work_ID"].ColumnName = "Work Id";
                }
                if (dtcol.Contains("APPLICATION"))
                {
                    dtcol["APPLICATION"].ColumnName = "APPLICATION";
                }
            }
            else if (dt.TableName == "Notes_History")
            {
                if (dtcol.Contains("ROW"))
                {
                    dtcol["ROW"].ColumnName = "Sr. #";
                }
                if (dtcol.Contains("Created_Date_Str"))
                {
                    dtcol["Created_Date_Str"].ColumnName = "Date & Time";
                }
                if (dtcol.Contains("CREATED_BY"))
                {
                    dtcol["CREATED_BY"].ColumnName = "User";
                }
                if (dtcol.Contains("NOTE_DESC"))
                {
                    dtcol["NOTE_DESC"].ColumnName = "Notes";
                }
            }
            else if (dt.TableName == "Region_Survey")
            {
                if (dtcol.Contains("REGION"))
                {
                    dtcol["REGION"].ColumnName = "Region";
                }
                if (dtcol.Contains("RECOMMENDED"))
                {
                    dtcol["RECOMMENDED"].ColumnName = "Recommended";
                }
                if (dtcol.Contains("NOT_RECOMMENDED"))
                {
                    dtcol["NOT_RECOMMENDED"].ColumnName = "Not Recommended";
                }
            }
            else if (dt.TableName == "Region_Survey_Patient_Data")
            {
                if (dtcol.Contains("ROW"))
                {
                    dtcol["ROW"].ColumnName = "Sr. #";
                }
                if (dtcol.Contains("NAME"))
                {
                    dtcol["NAME"].ColumnName = "Patient's Name";
                }
                if (dtcol.Contains("SERVICE_OR_PAYMENT_DESCRIPTION"))
                {
                    dtcol["SERVICE_OR_PAYMENT_DESCRIPTION"].ColumnName = "Case #";
                }
                if (dtcol.Contains("FEEDBACK"))
                {
                    dtcol["FEEDBACK"].ColumnName = "Feedback / Comments";
                }
                if (dtcol.Contains("DISCHARGE_DATE_STR"))
                {
                    dtcol["DISCHARGE_DATE_STR"].ColumnName = "Discharge Date";
                }
                if (dtcol.Contains("REFERRAL_DATE_STR"))
                {
                    dtcol["REFERRAL_DATE_STR"].ColumnName = "Referral Date";
                }
                if (dtcol.Contains("PATIENT_STATE"))
                {
                    dtcol["PATIENT_STATE"].ColumnName = "State";
                }
                if (dtcol.Contains("REGION"))
                {
                    dtcol["REGION"].ColumnName = "Region";
                }
            }
            else if (dt.TableName == "Question_Survey")
            {
                if (dtcol.Contains("REGION"))
                {
                    dtcol["REGION"].ColumnName = "Region";
                }
                if (dtcol.Contains("IS_IMPROVED_SETISFACTION_YES"))
                {
                    dtcol["IS_IMPROVED_SETISFACTION_YES"].ColumnName = "Condition Improved(Yes)";
                }
                if (dtcol.Contains("IS_IMPROVED_SETISFACTION_NO"))
                {
                    dtcol["IS_IMPROVED_SETISFACTION_NO"].ColumnName = "Condition Improved(No)";
                }
                if (dtcol.Contains("IS_REFERABLE_YES"))
                {
                    dtcol["IS_REFERABLE_YES"].ColumnName = "Refer our Practice(Yes)";
                }
                if (dtcol.Contains("IS_REFERABLE_NO"))
                {
                    dtcol["IS_REFERABLE_NO"].ColumnName = "Refer our Practice(No)";
                }
                if (dtcol.Contains("IS_CONTACT_HQ_YES"))
                {
                    dtcol["IS_CONTACT_HQ_YES"].ColumnName = "Had to Call Home Office(Yes)";
                }
                if (dtcol.Contains("IS_CONTACT_HQ_NO"))
                {
                    dtcol["IS_CONTACT_HQ_NO"].ColumnName = "Had to Call Home Office(No)";
                }
                if (dtcol.Contains("IS_RESPONSED_BY_HQ_YES"))
                {
                    dtcol["IS_RESPONSED_BY_HQ_YES"].ColumnName = "Home Office Courteous(Yes)";
                }
                if (dtcol.Contains("IS_RESPONSED_BY_HQ_NO"))
                {
                    dtcol["IS_RESPONSED_BY_HQ_NO"].ColumnName = "Home Office Courteous(No)";
                }
                if (dtcol.Contains("IS_QUESTION_ANSWERED_YES"))
                {
                    dtcol["IS_QUESTION_ANSWERED_YES"].ColumnName = " Concern Resolved(Yes)";
                }
                if (dtcol.Contains("IS_QUESTION_ANSWERED_NO"))
                {
                    dtcol["IS_QUESTION_ANSWERED_NO"].ColumnName = "Concern Resolved(No)";
                }
                if (dtcol.Contains("IS_PROTECTIVE_EQUIPMENT_YES"))
                {
                    dtcol["IS_PROTECTIVE_EQUIPMENT_YES"].ColumnName = "Protective Equipment(Yes)";
                }
                if (dtcol.Contains("IS_PROTECTIVE_EQUIPMENT_NO"))
                {
                    dtcol["IS_PROTECTIVE_EQUIPMENT_NO"].ColumnName = "Protective Equipment(No)";
                }

            }
            else if (dt.TableName == "Insurance_Setup")
            {
                if (dtcol.Contains("ROW"))
                {
                    dtcol["ROW"].ColumnName = "Sr. #";
                }
                if (dtcol.Contains("INSURANCE_PAYERS_ID"))
                {
                    dtcol["INSURANCE_PAYERS_ID"].ColumnName = "Payer ID";
                }
                if (dtcol.Contains("INSURANCE_NAME"))
                {
                    dtcol["INSURANCE_NAME"].ColumnName = "Name";
                }
                if (dtcol.Contains("ADDRESS"))
                {
                    dtcol["ADDRESS"].ColumnName = "Address 1";
                }
                if (dtcol.Contains("ADDRESS_1"))
                {
                    dtcol["ADDRESS_1"].ColumnName = "Address 2";
                }
                if (dtcol.Contains("CITY"))
                {
                    dtcol["CITY"].ColumnName = "City";
                }
                if (dtcol.Contains("STATE"))
                {
                    dtcol["STATE"].ColumnName = "State";
                }
                if (dtcol.Contains("ZIP"))
                {
                    dtcol["ZIP"].ColumnName = "ZIP Code";
                }
                if (dtcol.Contains("PHONE"))
                {
                    dtcol["PHONE"].ColumnName = "Phone";
                }
                if (dtcol.Contains("FC_NAME"))
                {
                    dtcol["FC_NAME"].ColumnName = "Financial Class";
                }
                if (dtcol.Contains("Inactive"))
                {
                    dtcol["Inactive"].ColumnName = "Auth. Required";
                }
                if (dtcol.Contains("CARRIER_LOCALITY"))
                {
                    dtcol["CARRIER_LOCALITY"].ColumnName = "Carrier Locality";
                }
                if (dtcol.Contains("CARRIER"))
                {
                    dtcol["CARRIER"].ColumnName = "Carrier";
                }
                if (dtcol.Contains("CARRIER_STATE"))
                {
                    dtcol["CARRIER_STATE"].ColumnName = "Carrier State";
                }
                if (dtcol.Contains("FEE_PLAN_REDIRECT"))
                {
                    dtcol["FEE_PLAN_REDIRECT"].ColumnName = "Fee Plan Redirect To";
                }
                if (dtcol.Contains("Created_Date_Str"))
                {
                    dtcol["Created_Date_Str"].ColumnName = "Created Date";
                }
                if (dtcol.Contains("Modified_Date_Str"))
                {
                    dtcol["Modified_Date_Str"].ColumnName = "Modified Date";
                }
            }
            else if (dt.TableName == "High_Balance_Report")
            {
                if (dtcol.Contains("ROW"))
                {
                    dtcol["ROW"].ColumnName = "Sr. #";
                }
                if (dtcol.Contains("WORK_ID"))
                {
                    dtcol["WORK_ID"].ColumnName = "Work ID";
                }
                if (dtcol.Contains("Patient_Account"))
                {
                    dtcol["Patient_Account"].ColumnName = "Patient Account";
                }
                if (dtcol.Contains("PATIENT_NAME"))
                {
                    dtcol["PATIENT_NAME"].ColumnName = "Patient Name";
                }
                if (dtcol.Contains("PHONE"))
                {
                    dtcol["PHONE"].ColumnName = "Phone No.";
                }
                if (dtcol.Contains("Patient_Balance"))
                {
                    dtcol["Patient_Balance"].ColumnName = "Due Amount";
                }
                if (dtcol.Contains("DEFAULT_POS"))
                {
                    dtcol["DEFAULT_POS"].ColumnName = "Default POS";
                }
                if (dtcol.Contains("REGION"))
                {
                    dtcol["REGION"].ColumnName = "Region";
                }
                if (dtcol.Contains("PRIMARY_INSURANCE"))
                {
                    dtcol["PRIMARY_INSURANCE"].ColumnName = "Primary Insurance";
                }
                if (dtcol.Contains("SECONDARY_INSURANCE"))
                {
                    dtcol["SECONDARY_INSURANCE"].ColumnName = "Secondary Insurance";
                }
            }
            else if (dt.TableName == "QA_Report")
            {
                if (dtcol.Contains("ROW"))
                {
                    dtcol["ROW"].ColumnName = "Sr. #";
                }
                if (dtcol.Contains("NAME"))
                {
                    dtcol["NAME"].ColumnName = "Rep Name";
                }
                if (dtcol.Contains("CLIENT_EXPERIENCE_TOTAL"))
                {
                    dtcol["CLIENT_EXPERIENCE_TOTAL"].ColumnName = "Client Experience";
                }
                if (dtcol.Contains("SYSTEM_PROCESS_TOTAL"))
                {
                    dtcol["SYSTEM_PROCESS_TOTAL"].ColumnName = "System Product & Email Process";
                }
                if (dtcol.Contains("EVALUATIONS"))
                {
                    dtcol["EVALUATIONS"].ColumnName = "Evaluation";
                }
                if (dtcol.Contains("TOTAL_POINTS"))
                {
                    dtcol["TOTAL_POINTS"].ColumnName = "Overall Score";
                }
                if (dtcol.Contains("WOW_FACTOR"))
                {
                    dtcol["WOW_FACTOR"].ColumnName = "Wow Factor or Negative Feedback";
                }
            }
            else if (dt.TableName == "QA_Report_AUD")
            {
                if (dtcol.Contains("ROW"))
                {
                    dtcol["ROW"].ColumnName = "Sr. #";
                }
                if (dtcol.Contains("NAME"))
                {
                    dtcol["NAME"].ColumnName = "Rep Name";
                }
                if (dtcol.Contains("CLIENT_EXPERIENCE_TOTAL"))
                {
                    dtcol["CLIENT_EXPERIENCE_TOTAL"].ColumnName = "Client Experience Selected / Average";
                }
                if (dtcol.Contains("SYSTEM_PROCESS_TOTAL"))
                {
                    dtcol["SYSTEM_PROCESS_TOTAL"].ColumnName = "System Product & Email Process Selected / Average";
                }
                if (dtcol.Contains("EVALUATIONS"))
                {
                    dtcol["EVALUATIONS"].ColumnName = "Evaluation Selected / Average";
                }
                if (dtcol.Contains("TOTAL_POINTS"))
                {
                    dtcol["TOTAL_POINTS"].ColumnName = "Overall Score Selected / Average";
                }
                //if (dtcol.Contains("WOW_FACTOR"))
                //{
                //    dtcol["WOW_FACTOR"].ColumnName = "Wow Factor or Negative Feedback Selected / Average";
                //}
            }
            else if (dt.TableName == "QA_Report_Survey")
            {
                if (dtcol.Contains("ROW"))
                {
                    dtcol["ROW"].ColumnName = "Sr. #";
                }
                if (dtcol.Contains("NAME"))
                {
                    dtcol["NAME"].ColumnName = "Rep Name";
                }
                if (dtcol.Contains("CLIENT_EXPERIENCE_TOTAL"))
                {
                    dtcol["CLIENT_EXPERIENCE_TOTAL"].ColumnName = "Call Quality";
                }
                if (dtcol.Contains("SYSTEM_PROCESS_TOTAL"))
                {
                    dtcol["SYSTEM_PROCESS_TOTAL"].ColumnName = "System Usage";
                }
                if (dtcol.Contains("EVALUATIONS"))
                {
                    dtcol["EVALUATIONS"].ColumnName = "Evaluation";
                }
                if (dtcol.Contains("TOTAL_POINTS"))
                {
                    dtcol["TOTAL_POINTS"].ColumnName = "Average Total Score";
                }
                //if (dtcol.Contains("WOW_FACTOR"))
                //{
                //    dtcol["WOW_FACTOR"].ColumnName = "Wow Factor or Negative Feedback";
                //}
            }
            else if (dt.TableName == "QA_Report_AUD_Survey")
            {
                if (dtcol.Contains("ROW"))
                {
                    dtcol["ROW"].ColumnName = "Sr. #";
                }
                if (dtcol.Contains("NAME"))
                {
                    dtcol["NAME"].ColumnName = "Rep Name";
                }
                if (dtcol.Contains("CLIENT_EXPERIENCE_TOTAL"))
                {
                    dtcol["CLIENT_EXPERIENCE_TOTAL"].ColumnName = "Call Quality Selected / Average";
                }
                if (dtcol.Contains("SYSTEM_PROCESS_TOTAL"))
                {
                    dtcol["SYSTEM_PROCESS_TOTAL"].ColumnName = "System Usage Selected / Average";
                }
                if (dtcol.Contains("EVALUATIONS"))
                {
                    dtcol["EVALUATIONS"].ColumnName = "Evaluation Selected / Average";
                }
                if (dtcol.Contains("TOTAL_POINTS"))
                {
                    dtcol["TOTAL_POINTS"].ColumnName = "Average Total Score Selected / Average";
                }
                //if (dtcol.Contains("WOW_FACTOR"))
                //{
                //    dtcol["WOW_FACTOR"].ColumnName = "Wow Factor or Negative Feedback Selected / Average";
                //}
            }
            else if (dt.TableName == "Advanced_region")
            {
                if (dtcol.Contains("ROW_NUM"))
                {
                    dtcol["ROW_NUM"].ColumnName = "Sr. #";
                }
                if (dtcol.Contains("STATE_CODE"))
                {
                    dtcol["STATE_CODE"].ColumnName = "State";
                }
                if (dtcol.Contains("REFERRAL_REGION_CODE"))
                {
                    dtcol["REFERRAL_REGION_CODE"].ColumnName = "Region Code";
                }
                if (dtcol.Contains("REFERRAL_REGION_NAME"))
                {
                    dtcol["REFERRAL_REGION_NAME"].ColumnName = "Region Name";
                }
                if (dtcol.Contains("COUNTY"))
                {
                    dtcol["COUNTY"].ColumnName = "County";
                }
                if (dtcol.Contains("CITY"))
                {
                    dtcol["CITY"].ColumnName = "City";
                }
                if (dtcol.Contains("ZIP_CODE"))
                {
                    dtcol["ZIP_CODE"].ColumnName = "ZIP Code";
                }
            }
            else if (dt.TableName == "Daily_Appointment_List")
            {
                //if (dtcol.Contains("ROW"))
                //{
                //    dtcol["ROW"].ColumnName = "Sr. #";
                //}
                if (dtcol.Contains("APPOINTMENT_DATE_STR"))
                {
                    dtcol["APPOINTMENT_DATE_STR"].ColumnName = "Appointment Date";
                }
                if (dtcol.Contains("NAME"))
                {
                    dtcol["NAME"].ColumnName = "Patient";
                }
                if (dtcol.Contains("TIME_FROM"))
                {
                    dtcol["TIME_FROM"].ColumnName = "Time";
                }
                if (dtcol.Contains("STR_LENGTH"))
                {
                    dtcol["STR_LENGTH"].ColumnName = "Length";
                }
                if (dtcol.Contains("MRN"))
                {
                    dtcol["MRN"].ColumnName = "MRN #";
                }
                if (dtcol.Contains("ADDRESS"))
                {
                    dtcol["ADDRESS"].ColumnName = "POS Address";
                }
                if (dtcol.Contains("RT_CASE_NO"))
                {
                    dtcol["RT_CASE_NO"].ColumnName = "Case #";
                }
                if (dtcol.Contains("REASON"))
                {
                    dtcol["REASON"].ColumnName = "Appt. Type";
                }
                if (dtcol.Contains("STATUS"))
                {
                    dtcol["STATUS"].ColumnName = "Status";
                }
                if (dtcol.Contains("PROVIDER"))
                {
                    dtcol["PROVIDER"].ColumnName = "Provider";
                }
                if (dtcol.Contains("REGION_NAME"))
                {
                    dtcol["REGION_NAME"].ColumnName = "Region";
                }
                if (dtcol.Contains("DESCRIPTION"))
                {
                    dtcol["DESCRIPTION"].ColumnName = "Discipline";
                }
                if (dtcol.Contains("APPOINTMENT_COMPLETE_DATE_STR"))
                {
                    dtcol["APPOINTMENT_COMPLETE_DATE_STR"].ColumnName = "Appointment Completion Date";
                }
                if (dtcol.Contains("APPOINTMENT_COMPLETE_TIME_STR"))
                {
                    dtcol["APPOINTMENT_COMPLETE_TIME_STR"].ColumnName = "Appointment Completion Time";
                }
                if (dtcol.Contains("AL_LATITUDE_STR"))
                {
                    dtcol["AL_LATITUDE_STR"].ColumnName = "Latitude";
                }
                if (dtcol.Contains("AL_LONGITUDE_STR"))
                {
                    dtcol["AL_LONGITUDE_STR"].ColumnName = "Longitude";
                }
            }
            else if (dt.TableName == "Patient_Scheduler_List")
            {
                //if (dtcol.Contains("ROW"))
                //{
                //    dtcol["ROW"].ColumnName = "Sr. #";
                //}
                if (dtcol.Contains("APPOINTMENT_DATE_STR"))
                {
                    dtcol["APPOINTMENT_DATE_STR"].ColumnName = "Date";
                }
                if (dtcol.Contains("TIME_FROM"))
                {
                    dtcol["TIME_FROM"].ColumnName = "Time";
                }
                if (dtcol.Contains("STR_LENGTH"))
                {
                    dtcol["STR_LENGTH"].ColumnName = "Length";
                }
                //if (dtcol.Contains("MRN"))
                //{
                //    dtcol["MRN"].ColumnName = "MRN #";
                //}
                if (dtcol.Contains("ADDRESS"))
                {
                    dtcol["ADDRESS"].ColumnName = "POS Address";
                }

                if (dtcol.Contains("REGION_NAME"))
                {
                    dtcol["REGION_NAME"].ColumnName = "Region";
                }
                if (dtcol.Contains("RT_CASE_NO"))
                {
                    dtcol["RT_CASE_NO"].ColumnName = "Case #";
                }
                if (dtcol.Contains("REASON"))
                {
                    dtcol["REASON"].ColumnName = "Appt. Type";
                }
                if (dtcol.Contains("STATUS"))
                {
                    dtcol["STATUS"].ColumnName = "Status";
                }
                if (dtcol.Contains("PROVIDER"))
                {
                    dtcol["PROVIDER"].ColumnName = "Provider";
                }
            }
            else if (dt.TableName == "Active_Indexer_History")
            {
                if (dtcol.Contains("ROW"))
                {
                    dtcol["ROW"].ColumnName = "Sr. #";
                }
                if (dtcol.Contains("FULL_NAME"))
                {
                    dtcol["FULL_NAME"].ColumnName = "Assigned To";
                }
                if (dtcol.Contains("WORK_ID"))
                {
                    dtcol["WORK_ID"].ColumnName = "Work ID";
                }
                if (dtcol.Contains("CREATED_DATE_STR"))
                {
                    dtcol["CREATED_DATE_STR"].ColumnName = "Date";
                }
            }
            else if (dt.TableName == "PHR_Users_Last_Login_Report")
            {
                if (dtcol.Contains("ROW"))
                {
                    dtcol["ROW"].ColumnName = "Sr. #";
                }
                if (dtcol.Contains("Patient_Account"))
                {
                    dtcol["Patient_Account"].ColumnName = "Account #";
                }
                if (dtcol.Contains("FIRST_NAME"))
                {
                    dtcol["FIRST_NAME"].ColumnName = "First Name";
                }
                if (dtcol.Contains("LAST_NAME"))
                {
                    dtcol["LAST_NAME"].ColumnName = "Last Name";
                }
                if (dtcol.Contains("EMAIL"))
                {
                    dtcol["EMAIL"].ColumnName = "Email";
                }
                if (dtcol.Contains("LAST_LOGIN_DATE"))
                {
                    dtcol["LAST_LOGIN_DATE"].ColumnName = "Last Login Date & Time";
                }
            }
            else
            {
                //For Facility Creation

                if (dtcol.Contains("FOL_BOOL"))
                {
                    dtcol["FOL_BOOL"].ColumnName = "FOL";
                }
                if (dtcol.Contains("COMPLETE_ADDRESS"))
                {
                    dtcol["COMPLETE_ADDRESS"].ColumnName = "Address";
                }
                if (dtcol.Contains("Name"))
                {
                    dtcol["Name"].ColumnName = "Facility Description";
                }
                if (dtcol.Contains("REGION_NAME"))
                {
                    dtcol["REGION_NAME"].ColumnName = "Region";
                }
                if (dtcol.Contains("PhoneFormat"))
                {
                    dtcol["PhoneFormat"].ColumnName = "Phone";
                }
                if (dtcol.Contains("FaxFormat"))
                {
                    dtcol["FaxFormat"].ColumnName = "Fax";
                }
                if (dtcol.Contains("POS_Code"))
                {
                    dtcol["POS_Code"].ColumnName = "POS";
                }
                if (dtcol.Contains("Parent"))
                {
                    dtcol["Parent"].ColumnName = "Parent Code";
                }
                if (dtcol.Contains("Description"))
                {
                    dtcol["Description"].ColumnName = "Parent Description";
                }
                //for Practice Organization
                if (dtcol.Contains("CODE"))
                    dt.Columns["CODE"].ColumnName = "Code";
                if (dtcol.Contains("NAME"))
                    dt.Columns["NAME"].ColumnName = "Name";
                if (dtcol.Contains("ZIP"))
                    dt.Columns["ZIP"].ColumnName = "ZIP Code";
                if (dtcol.Contains("CITY"))
                    dt.Columns["CITY"].ColumnName = "City";
                if (dtcol.Contains("STATE"))
                    dt.Columns["STATE"].ColumnName = "State";
                if (dtcol.Contains("ADDRESS"))
                    dt.Columns["ADDRESS"].ColumnName = "Address";
                //for Practice Organization
                if (dtcol.Contains("ZEROTO_FIFTEEN"))
                    dt.Columns["ZEROTO_FIFTEEN"].ColumnName = "0-15";
                if (dtcol.Contains("SIXTEENTO_THIRTY"))
                    dt.Columns["SIXTEENTO_THIRTY"].ColumnName = "16-30";
                if (dtcol.Contains("THIRTYONETO_FOURTFIVE"))
                    dt.Columns["THIRTYONETO_FOURTFIVE"].ColumnName = "31-45";
                if (dtcol.Contains("FOURTYSIXTO_SIXTY"))
                    dt.Columns["FOURTYSIXTO_SIXTY"].ColumnName = "46-60";
                if (dtcol.Contains("GREATERTHAN_HOUR"))
                    dt.Columns["GREATERTHAN_HOUR"].ColumnName = "1-2Hour";
                if (dtcol.Contains("GREATERTHAN_TWOHOUR"))
                    dt.Columns["GREATERTHAN_TWOHOUR"].ColumnName = "2 > Hour";
                if (dtcol.Contains("INDEXER_ASSIGNMENT_TOTAL_TIME"))
                    dt.Columns["INDEXER_ASSIGNMENT_TOTAL_TIME"].ColumnName = "Avg. of Assignment Elapsed Time(indexer)";
                if (dtcol.Contains("GRANDTOTAL"))
                    dt.Columns["GRANDTOTAL"].ColumnName = "Grand Total";
                if (dtcol.Contains("INDEXER_TOTAL_TIME"))
                    dt.Columns["INDEXER_TOTAL_TIME"].ColumnName = "Avg. of Indexer Completion Time";
                if (dtcol.Contains("AGENT_TOTAL_TIME"))
                    dt.Columns["AGENT_TOTAL_TIME"].ColumnName = "Avg. of Agent Completion Time";
                if (dtcol.Contains("TOTALTIME_INSECONDS_TOCOMPLETE"))
                    dt.Columns["TOTALTIME_INSECONDS_TOCOMPLETE"].ColumnName = "Avg. of Time to Completion Time";
                if (dtcol.Contains("AGENT_TOTAL_TIME"))
                    dt.Columns["AGENT_TOTAL_TIME"].ColumnName = "Avg. of Agent Completion Time";
                if (dtcol.Contains("SUPERVISOR_TOTAL_TIME"))
                    dt.Columns["SUPERVISOR_TOTAL_TIME"].ColumnName = "Avg. of Supervisor Completion Time";

                if (dtcol.Contains("UNIQUE_ID"))
                    dt.Columns["UNIQUE_ID"].ColumnName = "ID";
                if (dtcol.Contains("INDEXED_BY"))
                    dt.Columns["INDEXED_BY"].ColumnName = "Indexed By";
                if (dtcol.Contains("ASSIGNED_TO"))
                    dt.Columns["ASSIGNED_TO"].ColumnName = "Assign To";
                if (dtcol.Contains("SORCE_TYPE"))
                    dt.Columns["SORCE_TYPE"].ColumnName = "Source Type";
                if (dtcol.Contains("SORCE_NAME"))
                    dt.Columns["SORCE_NAME"].ColumnName = "Source Name";
                if (dtcol.Contains("DOCUMENT_TYPE"))
                    dt.Columns["DOCUMENT_TYPE"].ColumnName = "Document Type";
                if (dtcol.Contains("RECEIVE_DATE"))
                    dt.Columns["RECEIVE_DATE"].ColumnName = "Date";
                if (dtcol.Contains("TIME_ELASPE_IN_QUEUE"))
                    dt.Columns["TIME_ELASPE_IN_QUEUE"].ColumnName = "Index Completion Time(HH: MM)";
                if (dtcol.Contains("INDEX_COMPLETION_TIME"))
                    dt.Columns["INDEX_COMPLETION_TIME"].ColumnName = "Elapsed Queue Time(HH: MM)";
                if (dtcol.Contains("ROLE_NAME"))
                    dt.Columns["ROLE_NAME"].ColumnName = "Assigned Person Role";
                if (dtcol.Contains("ASSIGNED"))
                    dt.Columns["ASSIGNED"].ColumnName = "No. of Assigned Orders";
                if (dtcol.Contains("PENDING"))
                    dt.Columns["PENDING"].ColumnName = "No. of Pending Orders";
                if (dtcol.Contains("COMPLETED"))
                    dt.Columns["COMPLETED"].ColumnName = "No. of Completed Orders";
                if (dtcol.Contains("AVERAGE_RECIEVETO_COMLETIONTIME"))
                    dt.Columns["AVERAGE_RECIEVETO_COMLETIONTIME"].ColumnName = "Average Completed Time";
                if (dtcol.Contains("ROW"))
                    dt.Columns["ROW"].ColumnName = "Sr. #";
                if (dtcol.Contains("ASSIGNED_USER"))
                    dt.Columns["ASSIGNED_USER"].ColumnName = "Assigned To";
                if (dtcol.Contains("STATUS_NAME"))
                    dt.Columns["STATUS_NAME"].ColumnName = "Status";
                if (dtcol.Contains("CHARGE_ENTRY_DATE_str"))
                    dt.Columns["CHARGE_ENTRY_DATE_str"].ColumnName = "Charges Entry Date";
                if (dtcol.Contains("PATIENT_ACCOUNT"))
                    dt.Columns["PATIENT_ACCOUNT"].ColumnName = "Patient Account #";
                if (dtcol.Contains("Patient_Name"))
                    dt.Columns["Patient_Name"].ColumnName = "Patient Name";
                if (dtcol.Contains("CORRECTED_CLAIMS_TYPE_DESC"))
                    dt.Columns["CORRECTED_CLAIMS_TYPE_DESC"].ColumnName = "Type";
                if (dtcol.Contains("Theripist"))
                    dt.Columns["Theripist"].ColumnName = "Therapist";
                if (dtcol.Contains("INSURANCE_NAME"))
                    dt.Columns["INSURANCE_NAME"].ColumnName = "Insurance";
                if (dtcol.Contains("CASE_NO"))
                    dt.Columns["CASE_NO"].ColumnName = "Case #";
                if (dtcol.Contains("REQUESTED_BY"))
                    dt.Columns["REQUESTED_BY"].ColumnName = "Requested By";
                if (dtcol.Contains("STATUS_NAME"))
                    dt.Columns["REQUESTED_DATE"].ColumnName = "Requested Date";
                if (dtcol.Contains("WORK_DATE_Str"))
                    dt.Columns["WORK_DATE_Str"].ColumnName = "Work Date";
                if (dtcol.Contains("REMARKS"))
                    dt.Columns["REMARKS"].ColumnName = "Remarks";
                if (dtcol.Contains("RESPONSE_BY"))
                    dt.Columns["RESPONSE_BY"].ColumnName = "Response By";
                if (dtcol.Contains("RESPONSE"))
                    dt.Columns["RESPONSE"].ColumnName = "Response";
                if (dtcol.Contains("RESPONSE_DATE_Str"))
                    dt.Columns["RESPONSE_DATE_Str"].ColumnName = "Response Date";
                if (dtcol.Contains("REQUESTED_DATE_Str"))
                    dt.Columns["REQUESTED_DATE_Str"].ColumnName = "Requested Date";

                //for worksupervisor 
                if (dtcol.Contains("SUPERVISOR_NAME"))
                    dt.Columns["SUPERVISOR_NAME"].ColumnName = "Supervisor Name";
                if (dtcol.Contains("WORK_STATUS"))
                    dt.Columns["WORK_STATUS"].ColumnName = "Status";
                if (dtcol.Contains("TRANSFER_REASON"))
                    dt.Columns["TRANSFER_REASON"].ColumnName = "Transfer Reason";
                if (dtcol.Contains("ASSIGNED_BY"))
                    dt.Columns["ASSIGNED_BY"].ColumnName = "Assigned By";
                if (dtcol.Contains("ASSIGNED_DATE"))
                    dt.Columns["ASSIGNED_DATE"].ColumnName = "Assigned Date";
                if (dtcol.Contains("ElapseTime"))
                    dt.Columns["ElapseTime"].ColumnName = "ElapseTime";
                if (dtcol.Contains("TOTAL_PAGES"))
                    dt.Columns["TOTAL_PAGES"].ColumnName = "Pages";

                //for clinician setup 
                if (dtcol.Contains("FOX_PROVIDER_CODE"))
                    dt.Columns["FOX_PROVIDER_CODE"].ColumnName = "RT Code";
                if (dtcol.Contains("FIRST_NAME"))
                    dt.Columns["FIRST_NAME"].ColumnName = "First Name";
                if (dtcol.Contains("LAST_NAME"))
                    dt.Columns["LAST_NAME"].ColumnName = "Last Name";
                if (dtcol.Contains("REFERRAL_REGION_NAME"))
                    dt.Columns["REFERRAL_REGION_NAME"].ColumnName = "Treating Region";
                if (dtcol.Contains("STATE"))
                    dt.Columns["STATE"].ColumnName = "State";
                if (dtcol.Contains("STATUS"))
                    dt.Columns["STATUS"].ColumnName = "Status";
                if (dtcol.Contains("SENIOR_REGIONAL_DIRECTOR"))
                    dt.Columns["SENIOR_REGIONAL_DIRECTOR"].ColumnName = "Senior Regional Director";
                if (dtcol.Contains("REGIONAL_DIRECTOR"))
                    dt.Columns["REGIONAL_DIRECTOR"].ColumnName = "Regional Director";
                if (dtcol.Contains("ACCOUNT_MANAGER"))
                    dt.Columns["ACCOUNT_MANAGER"].ColumnName = "Account Manager";
                if (dtcol.Contains("TREATMENT_LOCATION"))
                    dt.Columns["TREATMENT_LOCATION"].ColumnName = "Primary Treatment Location";
                if (dtcol.Contains("DISPLINE_NAME"))
                    dt.Columns["DISPLINE_NAME"].ColumnName = "Discipline/Speciality";
                if (dtcol.Contains("PRIMARY_POS_DISTANCE"))
                    dt.Columns["PRIMARY_POS_DISTANCE"].ColumnName = "Ideal Distance from Primary POS";
                if (dtcol.Contains("VISIT_QOUTA_WEEK"))
                    dt.Columns["VISIT_QOUTA_WEEK"].ColumnName = "Contract Quota";
                if (dtcol.Contains("CLR"))
                    dt.Columns["CLR"].ColumnName = "CLR %";
                if (dtcol.Contains("ACTIVE_CASES"))
                    dt.Columns["ACTIVE_CASES"].ColumnName = "Ideal no. of Active Cases";
                if (dtcol.Contains("PTO_HRS"))
                    dt.Columns["PTO_HRS"].ColumnName = "PTO Hours";
                //if (dtcol.Contains("CREATED_BYNAME"))
                //    dt.Columns["CREATED_BYNAME"].ColumnName = "Created By";
                if (dtcol.Contains("CREATED_DATE_STRING"))
                    dt.Columns["CREATED_DATE_STRING"].ColumnName = "Created Date";
                if (dtcol.Contains("MODIFIED_BYNAME"))
                    dt.Columns["MODIFIED_BYNAME"].ColumnName = "Modified By";
                if (dtcol.Contains("MODIFIED_DATE_STRING"))
                    dt.Columns["MODIFIED_DATE_STRING"].ColumnName = "Modified Date";
                if (dtcol.Contains("INDIVIDUAL_NPI"))
                    dt.Columns["INDIVIDUAL_NPI"].ColumnName = "NPI";
                if (dtcol.Contains("EMAIL"))
                    dt.Columns["EMAIL"].ColumnName = "Email";
            }

        }

        /// <summary>
        /// AS this Function is called after setting up the headers. So use column name that you replace in SetHeaders Section.
        /// Do not write any column that has text like 'date' in end. Function will auto handle those columns.
        /// </summary>
        /// <returns></returns>
        private static List<string> GenerateCenterAlignList()
        {
            List<string> str = new List<string>();
            str.Add("Pri. Status");
            str.Add("Sec. Status");
            str.Add("Oth. Status");
            str.Add("Pat. Status");
            str.Add("DOS");
            return str;
        }
        /// <summary>
        /// /// AS this Function is called after setting up the headers. So use column name that you replace in SetHeaders Section.
        /// Do not write any column that has text like 'date' in end. Function will auto handle those columns.
        /// </summary>
        /// <returns></returns>
        private static List<string> GenerateRightAlignList()
        {
            List<string> str = new List<string>();
            str.Add("Claim Total");
            str.Add("Paid Amount");
            str.Add("Amount Due");
            str.Add("Amount");
            str.Add("Amount Paid");
            return str;
        }


        #region Do not change functions of this region
        public static bool CreateExcelDocument<T>(List<T> list, string xlsxFilePath, string widgetName)
        {
            try
            {
                DataTable dt = ListToDataTable(list);
                dt.TableName = widgetName;
                RemoveExtraColumns(dt, widgetName);
                SetAmounts(dt);
                SetHeaders(dt);
                return CreateExcelDocument(dt, xlsxFilePath);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        #region HELPER_FUNCTIONS
        //  This function is adapated from: http://www.codeguru.com/forum/showthread.php?t=450171
        //  My thanks to Carl Quirion, for making it "nullable-friendly".
        public static DataTable ListToDataTable<T>(List<T> list)
        {
            DataTable dt = new DataTable();

            foreach (PropertyInfo info in typeof(T).GetProperties())
            {
                dt.Columns.Add(new DataColumn(info.Name, GetNullableType(info.PropertyType)));
            }
            foreach (T t in list)
            {
                DataRow row = dt.NewRow();
                foreach (PropertyInfo info in typeof(T).GetProperties())
                {
                    if (!IsNullableType(info.PropertyType))
                        row[info.Name] = info.GetValue(t, null);
                    else
                        row[info.Name] = (info.GetValue(t, null) ?? DBNull.Value);
                }
                dt.Rows.Add(row);
            }
            return dt;
        }
        private static Type GetNullableType(Type t)
        {
            Type returnType = t;
            if (t.IsGenericType && t.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
            {
                returnType = Nullable.GetUnderlyingType(t);
            }
            return returnType;
        }
        private static bool IsNullableType(Type type)
        {
            return (type == typeof(string) ||
                    type.IsArray ||
                    (type.IsGenericType &&
                     type.GetGenericTypeDefinition().Equals(typeof(Nullable<>))));
        }
        #endregion

        public static bool CreateExcelDocument(DataTable dt, string excelFilename)
        {
            try
            {
                if (File.Exists(excelFilename))
                {
                    File.Delete(excelFilename);
                }
                using (SpreadsheetDocument document = SpreadsheetDocument.Create(excelFilename, SpreadsheetDocumentType.Workbook))
                {
                    if (dt.TableName == "Reconciliations_CP")
                    {
                        SetReconciliationHeaders(dt);
                    }
                    else if (dt.TableName == "Reconciliation_CP_Logs")
                    {
                        SetReconciliationHeaders(dt);
                    }
                    else if (dt.TableName == "Adjustments")
                    {
                        SetHeaders(dt);
                    }
                    else if (dt.TableName == "Patient_List")
                    {
                        SetHeaders(dt);
                    }
                    //else if (dt.TableName == "Users_Reports")
                    //{
                    //    SetHeaders(dt);
                    //}
                    else if (dt.TableName == "Contact_Type")
                    {
                        SetHeaders(dt);
                    }
                    else if (dt.TableName == "Document_Type")
                    {
                        SetHeaders(dt);
                    }
                    else if (dt.TableName == "Alert_Type")
                    {
                        SetHeaders(dt);
                    }
                    else if (dt.TableName == "Source_of_Referral")
                    {
                        SetHeaders(dt);
                    }
                    else if (dt.TableName == "Order_Status")
                    {
                        SetHeaders(dt);
                    }
                    else if (dt.TableName == "Task_Type")
                    {
                        SetHeaders(dt);
                    }
                    else if (dt.TableName == "Auth_Status")
                    {
                        SetHeaders(dt);
                    }
                    else if (dt.TableName == "Identifier_Type")
                    {
                        SetHeaders(dt);
                    }
                    else if (dt.TableName == "Location_Corporation")
                    {
                        SetHeaders(dt);
                    }
                    else if (dt.TableName == "Group_Identifier")
                    {
                        SetHeaders(dt);
                    }
                    else if (dt.TableName == "Facility_Setup")
                    {
                        SetHeaders(dt);
                    }
                    else if (dt.TableName == "Referral_Region")
                    {
                        SetHeaders(dt);
                    }
                    else if (dt.TableName == "Referral_Source")
                    {
                        SetHeaders(dt);
                    }
                    else if (dt.TableName == "Original_Queue")
                    {
                        SetHeaders(dt);
                    }
                    else if (dt.TableName == "Unassigned_Queue")
                    {
                        SetHeaders(dt);
                    }
                    else if (dt.TableName == "Assigned_Queue")
                    {
                        SetHeaders(dt);
                    }
                    else if (dt.TableName == "Completed_Queue")
                    {
                        SetHeaders(dt);
                    }
                    else if (dt.TableName == "Supervisor_Queue")
                    {
                        SetHeaders(dt);
                    }
                    else if (dt.TableName == "Referral_Report")
                    {
                        SetHeaders(dt);
                    }
                    else if (dt.TableName == "Detailed_Survey")
                    {
                        SetHeaders(dt);
                    }
                    else if (dt.TableName == "Detailed_Survey_old")
                    {
                        SetHeaders(dt);
                    }
                    else if (dt.TableName == "Interface_Logs")
                    {
                        SetHeaders(dt);
                    }
                    else if (dt.TableName == "Notes_History")
                    {
                        SetHeaders(dt);
                    }
                    else if (dt.TableName == "Region_Survey")
                    {
                        SetHeaders(dt);
                    }
                    else if (dt.TableName == "Region_Survey_Patient_Data")
                    {
                        SetHeaders(dt);
                    }
                    else if (dt.TableName == "Question_Survey")
                    {
                        SetHeaders(dt);
                    }
                    else if (dt.TableName == "Insurance_Setup")
                    {
                        SetHeaders(dt);
                    }
                    else if (dt.TableName == "High_Balance_Report")
                    {
                        SetHeaders(dt);
                    }
                    else if (dt.TableName == "QA_Report")
                    {
                        SetHeaders(dt);
                    }
                    else if (dt.TableName == "Daily_Appointment_List")
                    {
                        SetHeaders(dt);
                    }
                    else if (dt.TableName == "Patient_Scheduler_List")
                    {
                        SetHeaders(dt);
                    }
                    else if (dt.TableName == "Active_Indexer_History")
                    {
                        SetHeaders(dt);
                    }
                    WriteExcelFile(dt, document);
                }
                //Trace.WriteLine("Successfully created: " + excelFilename);
                return true;
            }
            catch (Exception ex)
            {
                //Trace.WriteLine("Failed, exception thrown: " + ex.Message);
                //return false;
                throw ex;
            }
        }
        private static void WriteExcelFile(DataTable dt, SpreadsheetDocument spreadsheet)
        {
            try
            {
                //  Create the Excel file contents.  This function is used when creating an Excel file either writing 
                //  to a file, or writing to a MemoryStream.
                spreadsheet.AddWorkbookPart();
                spreadsheet.WorkbookPart.Workbook = new Workbook();

                // This line of code prevents crashes in Excel 2010
                spreadsheet.WorkbookPart.Workbook.Append(new BookViews(new WorkbookView()));

                //  If we don't add a "WorkbookStylesPart", OLEDB will refuse to connect to this .xlsx file !
                //WorkbookStylesPart workbookStylesPart = spreadsheet.WorkbookPart.AddNewPart<WorkbookStylesPart>("rIdStyles");
                //Stylesheet stylesheet = new Stylesheet();
                //workbookStylesPart.Stylesheet = stylesheet;

                WorkbookStylesPart workbookStylesPart = AddStyleSheet(spreadsheet);


                uint worksheetNumber = 1;

                //  For each worksheet you want to create
                string workSheetID = "rId" + worksheetNumber.ToString();
                string worksheetName = dt.TableName;

                WorksheetPart newWorksheetPart = spreadsheet.WorkbookPart.AddNewPart<WorksheetPart>();
                newWorksheetPart.Worksheet = new Worksheet();

                // create sheet data
                newWorksheetPart.Worksheet.AppendChild(new SheetData());

                // save worksheet
                WriteDataTableToExcelWorksheet(dt, newWorksheetPart);

                // create the worksheet to workbook relation
                if (worksheetNumber == 1)
                    spreadsheet.WorkbookPart.Workbook.AppendChild(new Sheets());

                spreadsheet.WorkbookPart.Workbook.GetFirstChild<Sheets>().AppendChild(new Sheet()
                {
                    Id = spreadsheet.WorkbookPart.GetIdOfPart(newWorksheetPart),
                    SheetId = (uint)worksheetNumber,
                    Name = dt.TableName
                });

                spreadsheet.WorkbookPart.Workbook.Save();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private static void WriteDataTableToExcelWorksheet(DataTable dt, WorksheetPart worksheetPart)
        {
            try
            {
                var worksheet = worksheetPart.Worksheet;
                var sheetData = worksheet.GetFirstChild<SheetData>();
                string cellValue = "";
                //  Create a Header Row in our Excel file, containing one header for each Column of data in our DataTable.
                //
                //  We'll also create an array, showing which type each column of data is (Text or Numeric), so when we come to write the actual
                //  cells of data, we'll know if to write Text values or Numeric cell values.
                int numberOfColumns = dt.Columns.Count;
                bool[] IsNumericColumn = new bool[numberOfColumns];
                bool[] IsCenterAlign = new bool[numberOfColumns];

                string[] excelColumnNames = new string[numberOfColumns];
                for (int n = 0; n < numberOfColumns; n++)
                    excelColumnNames[n] = GetExcelColumnName(n);

                //
                //  Create the Header row in our Excel Worksheet
                //
                List<string> CenterAlignList = GenerateCenterAlignList();
                List<string> RightAlignList = GenerateRightAlignList();

                uint rowIndex = 1;

                var headerRow = new Row { RowIndex = rowIndex };  // add a row at the top of spreadsheet
                sheetData.Append(headerRow);

                for (int colInx = 0; colInx < numberOfColumns; colInx++)
                {
                    DataColumn col = dt.Columns[colInx];
                    AppendTextCell(excelColumnNames[colInx] + "1", col.ColumnName.Replace('_', ' '), headerRow);

                    if (dt.TableName == "Reconciliations_CP")
                    {
                        IsNumericColumn[colInx] = (col.ColumnName.Equals("Total Amount") || col.ColumnName.Equals("Total Posted") || col.ColumnName.Equals("Not Posted Amount"));
                        IsCenterAlign[colInx] = (col.ColumnName.Contains("date") || col.ColumnName.Contains("DOS") || col.ColumnName.Contains("Date")
                                            || col.ColumnName.Contains("DATE") || CenterAlignList.Contains(col.ColumnName) || col.ColumnName.Contains("Sr. #"));
                    }
                    else if (dt.TableName == "Reconciliation_CP_Logs")
                    {
                        IsCenterAlign[colInx] = col.ColumnName.Contains("Created On");
                    }
                    else
                    {
                        IsNumericColumn[colInx] = ((col.DataType.FullName == "System.Decimal") || (col.DataType.FullName == "System.Int32") || RightAlignList.Contains(col.ColumnName))
                                               && !((col.ColumnName.Equals("Aging") || col.ColumnName.Equals("Sr. #") || col.ColumnName.Equals("Pages") || col.ColumnName.Equals("No. of Splits")));

                        IsCenterAlign[colInx] = (col.ColumnName.Contains("date") || col.ColumnName.Contains("DOS") || col.ColumnName.Contains("Date")
                                            || col.ColumnName.Contains("DATE") || CenterAlignList.Contains(col.ColumnName));
                    }
                }

                //
                //  Now, step through each row of data in our DataTable...
                //
                //double cellNumericValue = 0;
                foreach (DataRow dr in dt.Rows)
                {
                    // ...create a new row, and append a set of this row's data to it.
                    ++rowIndex;
                    var newExcelRow = new Row { RowIndex = rowIndex };  // add a row at the top of spreadsheet
                    sheetData.Append(newExcelRow);

                    for (int colInx = 0; colInx < numberOfColumns; colInx++)
                    {
                        cellValue = dr.ItemArray[colInx].ToString();

                        // Create cell with data
                        if (IsNumericColumn[colInx])
                        {
                            #region Old method for appending numeric values
                            //  For numeric cells, make sure our input data IS a number, then write it out to the Excel file.
                            //  If this numeric value is NULL, then don't write anything to the Excel file.
                            //cellNumericValue = 0;
                            //if (double.TryParse(cellValue, out cellNumericValue))
                            //{
                            //    cellValue = cellNumericValue.ToString();
                            //    AppendNumericCell(excelColumnNames[colInx] + rowIndex.ToString(), cellValue, newExcelRow);
                            //}
                            #endregion
                            AppendNumericCell(excelColumnNames[colInx] + rowIndex.ToString(), cellValue, newExcelRow);
                        }
                        else if (IsCenterAlign[colInx])
                        {
                            AppendDateCell(excelColumnNames[colInx] + rowIndex.ToString(), cellValue, newExcelRow);
                        }
                        else
                        {
                            //  For text cells, just write the input data straight out to the Excel file.
                            AppendTextCell(excelColumnNames[colInx] + rowIndex.ToString(), cellValue, newExcelRow);
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private static void AppendTextCell(string cellReference, string cellStringValue, Row excelRow)
        {
            //  Add a new Excel Cell to our Row 
            Cell cell;
            //if it is the header row then it should be bold and center aligned
            if (excelRow.RowIndex == 1)
            {
                cell = new Cell() { CellReference = cellReference, DataType = CellValues.String, StyleIndex = Convert.ToUInt32(1) };
            }
            else
            {
                cell = new Cell() { CellReference = cellReference, DataType = CellValues.String };
            }
            CellValue cellValue = new CellValue();
            cellValue.Text = cellStringValue;
            cell.Append(cellValue);

            excelRow.Append(cell);
        }
        private static void AppendDateCell(string cellReference, string cellStringValue, Row excelRow)
        {

            //  Add a new Excel Cell to our Row 
            if (cellStringValue.Equals("01/01/1900"))
            {
                cellStringValue = "";
            }
            Cell cell = new Cell() { CellReference = cellReference, DataType = CellValues.String, StyleIndex = Convert.ToUInt32(2) };
            CellValue cellValue = new CellValue();
            cellValue.Text = cellStringValue;
            cell.Append(cellValue);
            excelRow.Append(cell);
        }
        private static string GetExcelColumnName(int columnIndex)
        {
            //  Convert a zero-based column index into an Excel column reference  (A, B, C.. Y, Y, AA, AB, AC... AY, AZ, B1, B2..)
            //
            //  eg  GetExcelColumnName(0) should return "A"
            //      GetExcelColumnName(1) should return "B"
            //      GetExcelColumnName(25) should return "Z"
            //      GetExcelColumnName(26) should return "AA"
            //      GetExcelColumnName(27) should return "AB"
            //      ..etc..
            //
            if (columnIndex < 26)
                return ((char)('A' + columnIndex)).ToString();

            char firstChar = (char)('A' + (columnIndex / 26) - 1);
            char secondChar = (char)('A' + (columnIndex % 26));

            return string.Format("{0}{1}", firstChar, secondChar);
        }
        private static WorkbookStylesPart AddStyleSheet(SpreadsheetDocument spreadsheet)
        {
            WorkbookStylesPart stylesheet = spreadsheet.WorkbookPart.AddNewPart<WorkbookStylesPart>();

            Stylesheet workbookstylesheet = new Stylesheet();

            Font font0 = new Font();         // Default font

            Font font1 = new Font();         // Bold font
            Bold bold = new Bold();
            font1.Append(bold);

            Fonts fonts = new Fonts();      // <APENDING Fonts>
            fonts.Append(font0);
            fonts.Append(font1);

            // <Fills>
            Fill fill0 = new Fill();        // Default fill

            Fills fills = new Fills();      // <APENDING Fills>
            fills.Append(fill0);

            // <Borders>
            Border border0 = new Border();     // Defualt border

            Borders borders = new Borders();    // <APENDING Borders>
            borders.Append(border0);

            // <CellFormats>
            CellFormat cellformat0 = new CellFormat() { FontId = 0, FillId = 0, BorderId = 0 }; // Default style : Mandatory | Style ID =0

            CellFormat cellformat1 = new CellFormat(new Alignment() { Horizontal = HorizontalAlignmentValues.Center, Vertical = VerticalAlignmentValues.Center }) { FontId = 1 };  // Style with Bold text ; Style ID = 1

            CellFormat cellformat2 = new CellFormat(new Alignment() { Horizontal = HorizontalAlignmentValues.Center, Vertical = VerticalAlignmentValues.Center }) { FontId = 0 };  // Style with center align normal text; Style ID = 2

            CellFormat cellformat3 = new CellFormat(new Alignment() { Horizontal = HorizontalAlignmentValues.Right, Vertical = VerticalAlignmentValues.Center }) { FontId = 0 };  // Style with center align normal text; Style ID = 3


            // <APENDING CellFormats>
            CellFormats cellformats = new CellFormats();
            cellformats.Append(cellformat0);
            cellformats.Append(cellformat1);
            cellformats.Append(cellformat2);
            cellformats.Append(cellformat3);


            // Append FONTS, FILLS , BORDERS & CellFormats to stylesheet <Preserve the ORDER>
            workbookstylesheet.Append(fonts);
            workbookstylesheet.Append(fills);
            workbookstylesheet.Append(borders);
            workbookstylesheet.Append(cellformats);

            // Finalize
            stylesheet.Stylesheet = workbookstylesheet;
            stylesheet.Stylesheet.Save();

            return stylesheet;
        }
        private static void AppendNumericCell(string cellReference, string cellStringValue, Row excelRow)
        {

            //  Add a new Excel Cell to our Row 
            Cell cell = new Cell() { CellReference = cellReference, DataType = CellValues.String, StyleIndex = Convert.ToUInt32(3) };
            CellValue cellValue = new CellValue();
            cellValue.Text = cellStringValue;
            cell.Append(cellValue);
            excelRow.Append(cell);
        }
    }
    #endregion
}
