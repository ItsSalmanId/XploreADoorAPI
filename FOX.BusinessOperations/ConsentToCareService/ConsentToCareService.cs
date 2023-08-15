using FOX.BusinessOperations.CommonService;
using FOX.BusinessOperations.CommonServices;
using FOX.BusinessOperations.FrictionlessReferral.SupportStaff;
using FOX.BusinessOperations.IndexInfoServices;
using FOX.DataModels.Context;
using FOX.DataModels.GenericRepository;
using FOX.DataModels.Models.CasesModel;
using FOX.DataModels.Models.OriginalQueueModel;
using FOX.DataModels.Models.Patient;
using FOX.DataModels.Models.RequestForOrder;
using FOX.DataModels.Models.Security;
using FOX.DataModels.Models.ServiceConfiguration;
using FOX.DataModels.Models.TasksModel;
using FOX.ExternalServices;
using SautinSoft;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Configuration;
using System.Windows.Forms;
using static FOX.DataModels.Models.ConsentToCare.ConsentToCare;

namespace FOX.BusinessOperations.ConsentToCareService
{
    public class ConsentToCareService : IConsentToCareService
    {

        #region PROPERTIES
        private readonly DBContextConsentToCare _consentToCareContext = new DBContextConsentToCare();
        private readonly DbContextTasks _TaskContext = new DbContextTasks();
        private readonly GenericRepository<FoxTblConsentToCare> _foxTblConsentToCareRepository;
        private readonly GenericRepository<ConsentToCareDocument> _consentToCareDocumentRepository;
        private readonly GenericRepository<FOX_TBL_TASK_TYPE> _taskTypeRepository;
        public static string SurveyMethod = string.Empty;
        private static List<Thread> threadsList = new List<Thread>();
        // To handle encryption/decryption Objective-C,C#
        private string passPhrase = "2657894562368456";
        #endregion

        #region CONSTRUCTOR
        public ConsentToCareService()
        {
            _foxTblConsentToCareRepository = new GenericRepository<FoxTblConsentToCare>(_consentToCareContext);
            _consentToCareDocumentRepository = new GenericRepository<ConsentToCareDocument>(_consentToCareContext);
            _taskTypeRepository = new GenericRepository<FOX_TBL_TASK_TYPE>(_TaskContext);
        }
        #endregion
        #region FUNCTIONS
        public FoxTblConsentToCare AddUpdateConsentToCare(FoxTblConsentToCare consentToCareObj, UserProfile profile)
        {
            if (consentToCareObj != null)
            {
                string concentToCareReceiverEmail = string.Empty;
                string concentToCareHomePhone = string.Empty;
                if (consentToCareObj.SEND_TO == "Patient")
                {
                    concentToCareReceiverEmail = consentToCareObj.PatientEmailAddress;
                     concentToCareHomePhone = consentToCareObj.PatientHomePhone;

                }
                else if (consentToCareObj.SEND_TO == "Financially Responsible Party")
                {
                     concentToCareReceiverEmail = consentToCareObj.FrpEmailAddress;
                     concentToCareHomePhone = consentToCareObj.FrpHomePhone;
                }
                if (consentToCareObj.SEND_TO == "Power of Attorney")
                {
                     concentToCareReceiverEmail = consentToCareObj.PoaEmailAddress;
                     concentToCareHomePhone = consentToCareObj.PoaHomePhone;
                }
                consentToCareObj.PATIENT_ACCOUNT = long.Parse(consentToCareObj.PATIENT_ACCOUNT_Str == null ? "0" : consentToCareObj.PATIENT_ACCOUNT_Str);
                profile.PracticeCode = GetPracticeCode();
                var config = Helper.GetServiceConfiguration(AppConfiguration.GetPracticeCode);
                var htmlTemplate = consentToCareObj.TEMPLATE_HTML;
                var consentToCareIdStr = consentToCareObj.CONSENT_TO_CARE_ID.ToString();
                var consentToCareId = new SqlParameter("@CONSENT_TO_CARE_ID", SqlDbType.BigInt) { Value = consentToCareObj.CONSENT_TO_CARE_ID };
                //var practiceCode = new SqlParameter("PRACTICE_CODE", SqlDbType.BigInt) { Value = GetPracticeCode() };
                var existingCaseId = consentToCareObj.CASE_ID;
                //var existingconsentToCareId = consentToCareObj.CONSENT_TO_CARE_ID;
                var caseId = new SqlParameter("@CASE_ID", SqlDbType.BigInt) { Value = consentToCareObj.CASE_ID };
                var practiceCode = new SqlParameter("@PRACTICE_CODE", SqlDbType.BigInt) { Value = GetPracticeCode() };
                var sendTo = new SqlParameter("@SEND_TO", SqlDbType.VarChar) { Value = consentToCareObj.SEND_TO };
                var existingInformation = SpRepository<FoxTblConsentToCare>.GetSingleObjectWithStoreProcedure(@"EXEC FOX_PROC_GET_CONSENT_TO_CARE_INFO_BY_CASE_ID_AND_SEND_TO @CASE_ID, @PRACTICE_CODE, @SEND_TO", caseId, practiceCode, sendTo);
                if (existingInformation == null)
                {
                    consentToCareObj.CONSENT_TO_CARE_ID = Helper.getMaximumId("CONSENT_TO_CARE_ID");
                    consentToCareObj.CREATED_DATE = DateTime.Now;
                    consentToCareObj.EXPIRY_DATE = DateTime.Now.AddDays(5);
                    consentToCareObj.CREATED_BY = profile.UserName;
                    consentToCareObj.PRACTICE_CODE = AppConfiguration.GetPracticeCode;
                    consentToCareObj.STATUS = "Sent";
                    consentToCareObj.STATUS_ID = 54100;
                    consentToCareObj.SOURCE_TYPE = "FOX TEAM";
                    _foxTblConsentToCareRepository.Insert(consentToCareObj);
                    _foxTblConsentToCareRepository.Save();
                    var currentConsentToCareId = consentToCareObj.CONSENT_TO_CARE_ID;
                    // Get List of CC Users
                    //var cc = GetEmailCCList();
                    // Get List of BCC Users
                    //var bcc = GetEmailBCCList();
                    //consentToCareId.
                    var encryptedEmailURL = GenerateEncryptedEmailURL(consentToCareObj);
                    var emailBody = EmailBody(consentToCareObj.PatientLastName, encryptedEmailURL);
                    var email = concentToCareReceiverEmail;
                    //var email = concentToCareReceiverEmail;
                    var number = concentToCareHomePhone;
                    //var number = concentToCareHomePhone;
                    var smsBody = SmsBody(consentToCareObj.PatientLastName);
                    if (!string.IsNullOrEmpty(concentToCareReceiverEmail))
                    {
                        bool sent = Helper.Email(to: email, subject: "Fox Patient Portal", body: emailBody, profile: profile, CC: null, BCC: null);
                    }
                    ///var sendEmail = Helper.consentToCareEmail(email, "Consent To Care", emailBody, CC: null, BCC: null);
                    if (!string.IsNullOrEmpty(number))
                    {
                        var status = SmsService.SMSTwilio(number, smsBody);
                    }
                    SupportStaffService supportStaffService = new SupportStaffService();
                    ResponseHTMLToPDF responseHTMLToPDF = supportStaffService.HTMLToPDF(config, htmlTemplate, consentToCareIdStr, "email", "");
                    var coverFilePath = responseHTMLToPDF.FilePath + "\\" + responseHTMLToPDF.FileName;
                    //var coverFilePath = HTMLToPDFSautinsoft(config, htmlTemplate, consentToCareIdStr);
                    //var coverFilePath = HTMLToPDFSautinsoft(config, htmlTemplate, consentToCareIdStr);
                    var consentToCareID = consentToCareObj.CONSENT_TO_CARE_ID;
                    var CASE_ID = consentToCareObj.CASE_ID;
                    SavePdfToImages(coverFilePath, config, currentConsentToCareId, CASE_ID.ToString(), CASE_ID, 1, "DR:Fax", "", profile.UserName, true);

                    //// task Implementation
                    string tasktypeHBR = "";
                    tasktypeHBR = "CONSENTTOCARE";
                    var currentDate = DateTime.Now.ToString();
                    IndexInfoService indexInfoServiceObj = new IndexInfoService();
                    var interfaceTaskHBR = setTaskData(profile, consentToCareObj.PATIENT_ACCOUNT, tasktypeHBR, currentDate);
                    interfaceTaskHBR.CASE_ID = consentToCareObj.CASE_ID;
                    OriginalQueue originalQueueObj = new OriginalQueue();
                    var taskInterfacedHBR = AddUpdateTask(interfaceTaskHBR, profile, originalQueueObj, consentToCareObj.SEND_TO);
                    InterfaceSynchModel interfaceSynch = new InterfaceSynchModel();
                    if (taskInterfacedHBR != null)
                    {
                        interfaceSynch.TASK_ID = taskInterfacedHBR.TASK_ID;
                        interfaceSynch.PATIENT_ACCOUNT = consentToCareObj.PATIENT_ACCOUNT;
                        interfaceSynch.CASE_ID = consentToCareObj.CASE_ID;

                        InsertInterfaceTeamData(interfaceSynch, profile);
                    }
                    //var taskInterfacedHBR = AddUpdateTask(interfaceTaskHBR, profile, obj);
                }
                else
                {
                    consentToCareObj = existingInformation;
                    consentToCareObj.EXPIRY_DATE = DateTime.Now.AddDays(5);
                    consentToCareObj.MODIFIED_BY = profile.UserName;
                    consentToCareObj.MODIFIED_DATE = DateTime.Now;
                    _foxTblConsentToCareRepository.Update(consentToCareObj);
                    _foxTblConsentToCareRepository.Save();
                    var existingconsentToCareId = consentToCareObj.CONSENT_TO_CARE_ID;
                    // send Email Or SMS
                    consentToCareObj.CASE_ID = existingCaseId;

                    var encryptedEmailURL = GenerateEncryptedEmailURL(consentToCareObj);
                    var emailBody = EmailBody(consentToCareObj.PatientLastName, encryptedEmailURL);
                    var email = concentToCareReceiverEmail;
                    //var email = concentToCareReceiverEmail;
                    var number = concentToCareHomePhone;
                    //var number = concentToCareHomePhone;
                    var smsBody = SmsBody(consentToCareObj.PatientLastName);
                    if (!string.IsNullOrEmpty(concentToCareReceiverEmail))
                    {
                        bool sent = Helper.Email(to: email, subject: "Fox Patient Portal", body: emailBody, profile: profile, CC: null, BCC: null);
                    }
                    ///var sendEmail = Helper.consentToCareEmail(email, "Consent To Care", emailBody, CC: null, BCC: null);
                    if(!string.IsNullOrEmpty(number))
                    {
                        var status = SmsService.SMSTwilio(number, smsBody);
                    }
                    SupportStaffService supportStaffService = new SupportStaffService();
                    ResponseHTMLToPDF responseHTMLToPDF = supportStaffService.HTMLToPDF(config, htmlTemplate, consentToCareIdStr, "email", "");
                    var coverFilePath = responseHTMLToPDF.FilePath + "\\" + responseHTMLToPDF.FileName;
                    //var coverFilePath = HTMLToPDFSautinsoft(config, htmlTemplate, consentToCareIdStr);
                    //var coverFilePath = HTMLToPDFSautinsoft(config, htmlTemplate, consentToCareIdStr);
                    var consentToCareID = consentToCareObj.CONSENT_TO_CARE_ID;
                    var CASE_ID = consentToCareObj.CASE_ID;
                    SavePdfToImages(coverFilePath, config, existingconsentToCareId, CASE_ID.ToString(), CASE_ID, 1, "DR:Fax", "", profile.UserName, true);

                    //var encryptedEmailURL = GenerateEncryptedEmailURL(consentToCareObj);
                    //var emailBody = EmailBody(consentToCareObj.PatientLastName, encryptedEmailURL);
                    //var email = "muhammadsalman7@carecloud.com";
                    //var number = "3040177646";
                    //var smsBody = SmsBody(consentToCareObj.PatientLastName);
                    ////bool sent = Helper.Email(to: email, subject: "Fox Patient Portal", body: emailBody, profile: profile, CC: null, BCC: null);
                    //var sendEmail = Helper.consentToCareEmail(email, "Consent To Care", emailBody, CC: null, BCC: null);
                    //var status = SmsService.SMSTwilio(number, smsBody);
                    //var coverFilePath = HTMLToPDFSautinsoft(config, htmlTemplate, consentToCareIdStr);
                    //var consentToCareID = consentToCareObj.CONSENT_TO_CARE_ID;
                    //SavePdfToImages(coverFilePath, config, existingconsentToCareId, consentToCareID.ToString(), consentToCareID, 1, "DR:Fax", "", profile.UserName, true);
                }
            }
            return consentToCareObj;
        }
        public int InsertInterfaceTeamData(InterfaceSynchModel obj, UserProfile Profile)
        {
            try
            {
                bool isSync = false;
                InterfaceSynchModel interfaceSynch = new InterfaceSynchModel();

                if (obj.PATIENT_ACCOUNT == null && obj.CASE_ID == null && obj.TASK_ID == null)
                {

                }
                else
                {
                    obj.APPLICATION = "PORTAL";
                    SqlParameter pracCode = new SqlParameter("@Practice_code", Profile.PracticeCode);
                    var response = SpRepository<string>.GetSingleObjectWithStoreProcedure(@"Exec Af_proc_is_talkrehab_practice @Practice_code", pracCode);
                    if (response == null)
                    {
                        isSync = false;
                    }
                    else
                    {
                        isSync = true;
                    }
                    long Pid = Helper.getMaximumId("FOX_INTERFACE_SYNCH_ID");
                    SqlParameter id = new SqlParameter("ID", Pid);

                    SqlParameter practiceCode = new SqlParameter("PRACTICE_CODE", Profile.PracticeCode);
                    SqlParameter caseId = new SqlParameter("CASE_ID", obj.CASE_ID ?? (object)DBNull.Value);
                    SqlParameter workId = new SqlParameter("Work_ID", obj.Work_ID ?? (object)DBNull.Value);
                    SqlParameter taskId = new SqlParameter("TASK_ID", obj.TASK_ID ?? (object)DBNull.Value);
                    SqlParameter patientAccount = new SqlParameter("PATIENT_ACCOUNT", obj.PATIENT_ACCOUNT);
                    SqlParameter userName = new SqlParameter("USER_NAME", Profile.UserName);
                    SqlParameter isSynced = new SqlParameter("IS_SYNCED", isSync);
                    SqlParameter application = new SqlParameter("APPLICATION", string.IsNullOrEmpty(obj.APPLICATION) ? string.Empty : obj.APPLICATION);
                    var result = SpRepository<InterfaceSynchModel>.GetListWithStoreProcedure(@"FOX_PROC_INSERT_INTERFACE_DATA @ID, @PRACTICE_CODE,@CASE_ID,@Work_ID,@TASK_ID,@PATIENT_ACCOUNT,@USER_NAME,@IS_SYNCED,@APPLICATION",
                                                                                                           id, practiceCode, caseId, workId, taskId, patientAccount, userName, isSynced, application);
                    return result.Count;
                }
                return 0;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }
        private FOX_TBL_TASK GetTask(long Practice_Code, long Task_Id)
        {
            SqlParameter practiceCode = new SqlParameter("PRACTICE_CODE", Practice_Code);
            SqlParameter TaskID = new SqlParameter("TASK_ID", Task_Id);
            return SpRepository<FOX_TBL_TASK>.GetSingleObjectWithStoreProcedure(@"FOX_PROC_GET_TASK @PRACTICE_CODE, @TASK_ID", practiceCode, TaskID);

        }
        public FOX_TBL_TASK AddUpdateTask(FOX_TBL_TASK task, UserProfile profile, OriginalQueue WORK_QUEUE, string sendTo)
        {

            if (!string.IsNullOrEmpty(task.PATIENT_ACCOUNT_STR))
            {
                task.PATIENT_ACCOUNT = Convert.ToInt64(task.PATIENT_ACCOUNT_STR);
            }
            if (task != null && profile != null)
            {
                FOX_TBL_TASK dbTask = GetTask(profile.PracticeCode, task.TASK_ID);
                if (dbTask == null)
                {

                    SqlParameter sendToId = new SqlParameter("SEND_TO_ID", task.SEND_TO_ID ?? (object)DBNull.Value);
                    long primaryKey = Helper.getMaximumId("FOX_TASK_ID");
                    SqlParameter id = new SqlParameter("ID", primaryKey);
                    SqlParameter practiceCode = new SqlParameter("PRACTICE_CODE", profile.PracticeCode);
                    SqlParameter patientAccount = new SqlParameter("PATIENT_ACCOUNT", task.PATIENT_ACCOUNT);
                    SqlParameter isCompletedInt = new SqlParameter("IS_COMPLETED_INT", SqlDbType.Int);
                    isCompletedInt.Value = 0;// new SqlParameter("IS_COMPLETED_INT", 0);/*0: Initiated 1:Sender Completed 2:Final Route Completed*/
                    SqlParameter userName = new SqlParameter("USER_NAME", profile.UserName);
                    SqlParameter isTemplate = new SqlParameter("IS_TEMPLATE", task.IS_TEMPLATE);
                    SqlParameter taskTypeId = new SqlParameter("TASK_TYPE_ID", task.TASK_TYPE_ID);
                    SqlParameter finalRouteId = new SqlParameter("FINAL_ROUTE_ID", task.FINAL_ROUTE_ID ?? (object)DBNull.Value);
                    SqlParameter priority = new SqlParameter("PRIORITY", string.IsNullOrEmpty(task.PRIORITY) ? string.Empty : task.PRIORITY);
                    SqlParameter dueDateTime = new SqlParameter("DUE_DATE_TIME", string.IsNullOrEmpty(task.DUE_DATE_TIME_str) ? (object)DBNull.Value : Helper.ConvertStingToDateTime(task.DUE_DATE_TIME_str) ?? Helper.GetCurrentDate());
                    task.DUE_DATE_TIME = Helper.ConvertStingToDateTime(task.DUE_DATE_TIME_str);
                    SqlParameter categoryID = new SqlParameter("CATEGORY_ID", task.CATEGORY_ID ?? (object)DBNull.Value);
                    SqlParameter isReqSignedoff = new SqlParameter("IS_REQ_SIGNOFF", task.IS_REQ_SIGNOFF ?? (object)DBNull.Value);
                    SqlParameter isSendingRouteDetails = new SqlParameter("IS_SENDING_ROUTE_DETAILS", task.IS_SENDING_ROUTE_DETAILS ?? (object)DBNull.Value);
                    SqlParameter sendContextId = new SqlParameter("SEND_CONTEXT_ID", task.SEND_CONTEXT_ID ?? (object)DBNull.Value);
                    SqlParameter contextInfo = new SqlParameter("CONTEXT_INFO", task.CONTEXT_INFO ?? (object)DBNull.Value);
                    SqlParameter deliveryId = new SqlParameter("DEVELIVERY_ID", task.DEVELIVERY_ID ?? (object)DBNull.Value);
                    SqlParameter destinations = new SqlParameter("DESTINATIONS", task.DESTINATIONS ?? (object)DBNull.Value);
                    SqlParameter loc_Id = new SqlParameter("LOC_ID", task.LOC_ID ?? (object)DBNull.Value);
                    SqlParameter provider_ID = new SqlParameter("PROVIDER_ID", task.PROVIDER_ID ?? (object)DBNull.Value);
                    SqlParameter isSendMailAuto = new SqlParameter("IS_SEND_EMAIL_AUTO", task.IS_SEND_EMAIL_AUTO ?? (object)DBNull.Value);
                    SqlParameter deleted = new SqlParameter("DELETED", task.DELETED);
                    SqlParameter isSendToUser = new SqlParameter("IS_SEND_TO_USER", task.IS_SEND_TO_USER);
                    SqlParameter isFinalRouteUser = new SqlParameter("IS_FINAL_ROUTE_USER", task.IS_FINAL_ROUTE_USER);
                    SqlParameter isFinalRouteMarkComplete = new SqlParameter("IS_FINALROUTE_MARK_COMPLETE", task.IS_FINALROUTE_MARK_COMPLETE);
                    SqlParameter isSendToMarkComplete = new SqlParameter("IS_SENDTO_MARK_COMPLETE", task.IS_SENDTO_MARK_COMPLETE);
                    SqlParameter caseID = new SqlParameter("CASE_ID", task.CASE_ID);

                    dbTask = SpRepository<FOX_TBL_TASK>.GetSingleObjectWithStoreProcedure(@"FOX_PROC_INSERT_CONSENT_TO_CARE_TASK @ID, @PRACTICE_CODE,@PATIENT_ACCOUNT ,@IS_COMPLETED_INT ,@USER_NAME,@IS_TEMPLATE,@TASK_TYPE_ID,@SEND_TO_ID,@FINAL_ROUTE_ID,@PRIORITY,@DUE_DATE_TIME,@CATEGORY_ID,@IS_REQ_SIGNOFF,@IS_SENDING_ROUTE_DETAILS,@SEND_CONTEXT_ID,@CONTEXT_INFO,@DEVELIVERY_ID,@DESTINATIONS,@LOC_ID,@PROVIDER_ID,@IS_SEND_EMAIL_AUTO,@DELETED,@IS_SEND_TO_USER,@IS_FINAL_ROUTE_USER,@IS_FINALROUTE_MARK_COMPLETE,@IS_SENDTO_MARK_COMPLETE, @CASE_ID",
                                                                                                                    id, practiceCode, patientAccount, isCompletedInt, userName, isTemplate, taskTypeId, sendToId, finalRouteId, priority, dueDateTime, categoryID, isReqSignedoff, isSendingRouteDetails, sendContextId, contextInfo, deliveryId, destinations, loc_Id, provider_ID, isSendMailAuto, deleted, isSendToUser, isFinalRouteUser, isFinalRouteMarkComplete, isSendToMarkComplete, caseID);
                    dbTask.dbChangeMsg = "TaskInsertSuccessed";
                    dbTask.CATEGORY_CODE = _taskTypeRepository.GetByID(dbTask?.TASK_TYPE_ID)?.CATEGORY_CODE;

                    List<TaskLog> taskLoglist = new List<TaskLog>();
                    List<string> consentTocarelogs = new List<string>();
                    StringBuilder consentTocarelogsString = new StringBuilder();
                    consentTocarelogs.Add("Consent To Care :" + Helper.GetCurrentDate());
                    consentTocarelogs.Add("Consent to care link has been send to : " + sendTo);
                    taskLoglist.Add(new TaskLog()
                    {
                        ACTION = "Consent To Care Logs",
                        ACTION_DETAIL = consentTocarelogsString.ToString()
                    }
                        );

                    if (taskLoglist.Count() > 0)
                    {
                        InsertTaskLog(dbTask.TASK_ID, taskLoglist, profile);
                    }

                }
                return dbTask;
            }
            return null;
        }
        private void InsertTaskLog(long? taskId, List<TaskLog> tasklog, UserProfile profile)
        {
                if (taskId != null && taskId.Value > 0)
                {
                    foreach (var item in tasklog)
                    {
                        List<TaskLog> lstTaskLog = new List<TaskLog>();
                        lstTaskLog.Add(item);
                        DataTable _dataTable = GetTaskLogTable(lstTaskLog);

                        if (_dataTable.Rows.Count > 0)
                        {
                            long primaryKey = Helper.getMaximumId("FOX_TASK_LOG_ID");
                            SqlParameter id = new SqlParameter("ID", primaryKey);
                            SqlParameter task_log = new SqlParameter("TASK_LOG", SqlDbType.Structured);
                            task_log.TypeName = "TASK_LOG_HISTORY";
                            task_log.Value = _dataTable;
                            SqlParameter practice_Code = new SqlParameter("PRACTICE_CODE", profile.PracticeCode);
                            SqlParameter Task_Id = new SqlParameter("TASK_ID", taskId);
                            SqlParameter user_Name = new SqlParameter("USER_NAME", profile.UserName);
                            SpRepository<TaskLog>.GetSingleObjectWithStoreProcedure(@"exec FOX_PROC_INSERT_TASK_LOG @ID, @PRACTICE_CODE, @TASK_ID, @TASK_LOG, @USER_NAME", id, practice_Code, Task_Id, task_log, user_Name);
                        }
                    }
                }
        }
        private DataTable GetTaskLogTable(List<TaskLog> lst)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("ACTION", typeof(string));
            dt.Columns.Add("ACTION_DETAIL", typeof(string));
            foreach (TaskLog task in lst)
            {
                dt.Rows.Add(task.ACTION, task.ACTION_DETAIL);
            }
            return dt;
        }
        private Patient GetPatient(long? patient_Account)
        {
            if (patient_Account != null && patient_Account != 0)
            {
                SqlParameter PatAccount = new SqlParameter("PATIENT_ACCOUNT", patient_Account ?? 0);
                return SpRepository<Patient>.GetSingleObjectWithStoreProcedure(@"FOX_PROC_GET_PATIENT  @PATIENT_ACCOUNT", PatAccount);
            }
            return null;
        }
        private FOX_TBL_TASK setTaskData(UserProfile profile, long? PATIENT_ACCOUNT, string tasktypeHBR, string CURRENT_DATE_STR)
        {
            var task = new FOX_TBL_TASK();
            SqlParameter pPracticeCode = new SqlParameter("PRACTICE_CODE", profile.PracticeCode);
            SqlParameter pTaskTypeName = new SqlParameter();
            pTaskTypeName.ParameterName = "NAME";
            pTaskTypeName.Value = "porta";
            var Task_type_Id = SpRepository<FOX_TBL_TASK_TYPE>.GetSingleObjectWithStoreProcedure(@"FOX_PROC_GET_TASK_ID @PRACTICE_CODE, @NAME", pPracticeCode, pTaskTypeName);
            task.TASK_TYPE_ID = Task_type_Id?.TASK_TYPE_ID ?? 0;
            task.PRACTICE_CODE = profile.PracticeCode;
            task.PATIENT_ACCOUNT = PATIENT_ACCOUNT;
            var PracticeCode = new SqlParameter { ParameterName = "PRACTICE_CODE", SqlDbType = SqlDbType.BigInt, Value = profile.PracticeCode };
            var _taskTId = new SqlParameter { ParameterName = "TASK_ID", SqlDbType = SqlDbType.BigInt, Value = -1 };
            var _taskTypeId = new SqlParameter { ParameterName = "TASK_TYPE_ID", SqlDbType = SqlDbType.Int, Value = task.TASK_TYPE_ID };
            var _isTemplate = new SqlParameter { ParameterName = "IS_TEMPLATE", SqlDbType = SqlDbType.Bit, Value = true };
            var taskTemplate = SpRepository<FOX_TBL_TASK>.GetSingleObjectWithStoreProcedure(@"exec FOX_PROC_GET_TASK_BY_TASK_TYPE_ID 
                               @PRACTICE_CODE, @TASK_ID, @TASK_TYPE_ID, @IS_TEMPLATE", PracticeCode, _taskTId, _taskTypeId, _isTemplate);
            if (taskTemplate != null)
            {
                task.SEND_TO_ID = taskTemplate.SEND_TO_ID;
            }
            task.IS_SEND_TO_USER = taskTemplate.IS_SEND_TO_USER;
            task.FINAL_ROUTE_ID = taskTemplate.FINAL_ROUTE_ID;
            task.IS_FINAL_ROUTE_USER = taskTemplate.IS_FINAL_ROUTE_USER;
            task.PRIORITY = taskTemplate.PRIORITY;
            if (task.DUE_DATE_TIME == null)
            {
                task.DUE_DATE_TIME_str = CURRENT_DATE_STR;
            }
            return task;
        }

        private void AddFilesToDatabase(string filePath, string caseID, long lworkid, string logoPath, long consentToCareId)
        {
            try
            {
                ConsentToCareDocument consentToCareDocumentObj = new ConsentToCareDocument();
                consentToCareDocumentObj.DOCUMENTS_ID = Helper.getMaximumId("DOCUMENTS_ID");
                consentToCareDocumentObj.IMAGE_PATH = filePath;
                consentToCareDocumentObj.LOGO_PATH = logoPath;
                consentToCareDocumentObj.CREATED_DATE = DateTime.Now;
                consentToCareDocumentObj.MODIFIED_DATE = DateTime.Now;
                consentToCareDocumentObj.PRACTICE_CODE = AppConfiguration.GetPracticeCode;
                consentToCareDocumentObj.CASE_ID = int.Parse(caseID);
                consentToCareDocumentObj.CONSENT_TO_CARE_ID = consentToCareId;
                _consentToCareDocumentRepository.Insert(consentToCareDocumentObj);
                _consentToCareDocumentRepository.Save();
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }
        public void SavePdfToImages(string PdfPath, ServiceConfiguration config, long consentToCareId, string workId, long lworkid, int noOfPages, string sorcetype, string sorceName, string userName, bool approval = true)
        {
            approval = false;
            var decline = false;
            List<int> threadCounter = new List<int>();
            if (!string.IsNullOrEmpty(PdfPath) && PdfPath.Contains("Signed"))
            {
                approval = true;
            }
            if (!string.IsNullOrEmpty(PdfPath) && PdfPath.Contains("Unsigned"))
            {
                decline = true;
            }
            if (!Directory.Exists(config.IMAGES_PATH_SERVER))
            {
                Directory.CreateDirectory(config.IMAGES_PATH_SERVER);
            }

            if (System.IO.File.Exists(PdfPath))
            {
                for (int i = 0; i < noOfPages; i++)
                {
                    //End
                    //string ImgDirPath = "FoxDocumentDirectory\\Fox\\Images";
                    var imgPath = "";
                    var logoImgPath = "";
                    var imgPathServer = "";
                    var logoImgPathServer = "";
                    Random random = new Random();

                    if (sorcetype.Split(':')?[0] == "DR")
                    {
                        var randomString = random.Next();
                        imgPath = config.IMAGES_PATH_DB + "\\" + workId + "_" + i + "_" + randomString + ".jpg";
                        imgPathServer = config.IMAGES_PATH_SERVER + "\\" + workId + "_" + i + "_" + randomString + ".jpg";

                        randomString = random.Next();
                        logoImgPath = config.IMAGES_PATH_DB + "\\Logo_" + workId + "_" + i + "_" + randomString + ".jpg";
                        logoImgPathServer = config.IMAGES_PATH_SERVER + "\\Logo_" + workId + "_" + i + "_" + randomString + ".jpg";
                    }
                    else
                    {
                        var randomString = random.Next();
                        imgPath = config.IMAGES_PATH_DB + "\\" + workId + "_" + i + "_" + randomString + ".jpg";
                        imgPathServer = config.IMAGES_PATH_SERVER + "\\" + workId + "_" + i + "_" + randomString + ".jpg";
                        randomString = random.Next();
                        logoImgPath = config.IMAGES_PATH_DB + "\\Logo_" + workId + "_" + i + "_" + randomString + ".jpg";
                        logoImgPathServer = config.IMAGES_PATH_SERVER + "\\Logo_" + workId + "_" + i + "_" + randomString + ".jpg"; logoImgPathServer = config.IMAGES_PATH_SERVER + "\\Logo_" + workId + "_" + i + "_" + randomString + ".jpg";
                    }

                    Thread myThread = new Thread(() => this.newThreadImplementaion(ref threadCounter, PdfPath, i, imgPathServer, logoImgPathServer));
                    myThread.Start();
                    if (workId != null && workId.Contains("_"))
                    {
                    }
                    threadsList.Add(myThread);
                    AddFilesToDatabase(imgPath, workId, lworkid, logoImgPath, consentToCareId);
                }
                while (noOfPages > threadCounter.Count)
                {
                    //loop untill record complete
                }
                foreach (var thread in threadsList)
                {
                    thread.Abort();
                }
                //noOfPages = noOfPages + 1;
                long ConvertedWorkID = Convert.ToInt64(workId);
                noOfPages = _consentToCareDocumentRepository.GetMany(t => t.CASE_ID == ConvertedWorkID && !t.DELETED)?.Count() ?? 0;
                ///AddToDatabase(PdfPath, noOfPages, workId, sorcetype, sorceName, userName, approval, config.PRACTICE_CODE, decline);
            }
        }
        public void AddToDatabase(string filePath, int noOfPages, string workId, string sorcetype, string sorceName, string userName, bool approval, long? practice_code, bool decline)
        {
            try
            {
                var PracticeCode = new SqlParameter { ParameterName = "@PRACTICE_CODE", SqlDbType = SqlDbType.BigInt, Value = practice_code };
                var workid = new SqlParameter { ParameterName = "@WORK_ID", SqlDbType = SqlDbType.BigInt, Value = workId };
                var username = new SqlParameter { ParameterName = "@USER_NAME", SqlDbType = SqlDbType.VarChar, Value = userName };
                var noofpages = new SqlParameter { ParameterName = "@NO_OF_PAGES", SqlDbType = SqlDbType.Int, Value = noOfPages };
                var approve = new SqlParameter { ParameterName = "@APPROVAL", SqlDbType = SqlDbType.Bit, Value = approval };
                var declined = new SqlParameter { ParameterName = "@DECLINE", SqlDbType = SqlDbType.Bit, Value = decline };

                var result = SpRepository<OriginalQueue>.GetListWithStoreProcedure(@"exec FOX_PROC_ADD_TODB_FROM_INDEXINFO @PRACTICE_CODE,@WORK_ID,@USER_NAME,@NO_OF_PAGES,@APPROVAL,@DECLINE",
                    PracticeCode, workid, username, noofpages, approve, declined);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public void newThreadImplementaion(ref List<int> threadCounter, string PdfPath, int i, string imgPath, string logoImgPath)
        {
            try
            {
                System.Drawing.Image img;
                PdfFocus f = new PdfFocus();
                // f.Serial = "10261435399";
                f.Serial = "80033727929";
                f.OpenPdf(PdfPath);

                if (f.PageCount > 0)
                {
                    //Save all PDF pages to jpeg images
                    f.ImageOptions.Dpi = 120;
                    f.ImageOptions.ImageFormat = ImageFormat.Jpeg;

                    var image = f.ToImage(i + 1);
                    //Next manipulate with Jpeg in memory or save to HDD, open in a viewer
                    using (var ms = new MemoryStream(image))
                    {
                        img = System.Drawing.Image.FromStream(ms);
                        img.Save(imgPath, ImageFormat.Jpeg);
                        Bitmap bmp = new Bitmap(img);
                        ConvertPDFToImages ctp = new ConvertPDFToImages();
                        img.Dispose();
                        ctp.SaveWithNewDimention(bmp, 115, 150, 100, logoImgPath);
                        bmp.Dispose();
                    }
                }
                threadCounter.Add(1);
            }
            catch (Exception)
            {
                threadCounter.Add(1);
            }
        }
        private string HTMLToPDFSautinsoft(ServiceConfiguration conf, string htmlString, string fileName, string linkMessage = null)
        {
            try
            {
                PdfMetamorphosis p = new PdfMetamorphosis();
                p.Serial = "10262870570";
                p.PageSettings.Size.A4();
                //p.PageSettings.Orientation = PdfMetamorphosis.PageSetting.Orientations.Portrait;
                p.PageSettings.Orientation = PdfMetamorphosis.PageSetting.Orientations.Landscape;
                p.PageSettings.MarginLeft.Inch(0.1f);
                p.PageSettings.MarginRight.Inch(0.1f);
                if (p != null)
                {
                    string pdfFilePath = Path.Combine(conf.DOCUMENTS_PATH_SERVER);
                    //string finalsetpath = conf.ORIGINAL_FILES_PATH_SERVER.Remove(conf.ORIGINAL_FILES_PATH_SERVER.Length - 1);
                    if (!Directory.Exists(pdfFilePath))
                    {
                        Directory.CreateDirectory(pdfFilePath);
                    }
                    fileName = fileName + DateTime.Now.Ticks + ".pdf";
                    string pdfFilePathnew = pdfFilePath + "\\" + fileName;
                    if (p.HtmlToPdfConvertStringToFile(htmlString, pdfFilePathnew) == 0)
                    {
                        return pdfFilePathnew;
                    }
                    else
                    {
                        var ex = p.TraceSettings.ExceptionList.Count > 0 ? p.TraceSettings.ExceptionList[0] : null;
                        var msg = ex != null ? ex.Message + Environment.NewLine + ex.StackTrace : "An error occured during converting HTML to PDF!";
                        return "";
                    }
                }
                return "";
            }
            catch (Exception)
            {
                return "";
            }
        }
        public ConsentToCareList GetConsentToCare(FoxTblConsentToCare consentToCareObj, UserProfile profile)
        {
            List<FoxTblConsentToCare> consentToCareList = new List<FoxTblConsentToCare>();
            ConsentToCareList response = new ConsentToCareList();
            if (consentToCareObj != null)
            {
                var caseId = new SqlParameter("@CASE_ID", SqlDbType.BigInt) { Value = consentToCareObj.CASE_ID };
                var practiceCode = new SqlParameter("PRACTICE_CODE", SqlDbType.BigInt) { Value = GetPracticeCode() };
                response.ConsentToCareDbList = SpRepository<FoxTblConsentToCare>.GetListWithStoreProcedure(@"EXEC FOX_PROC_GET_CONSENT_TO_CARE_INFO_BY_CASE_ID @CASE_ID, @PRACTICE_CODE", caseId, practiceCode);
            }
            return response;
        }
        public ConsentToCareList GetConsentToCareDocumentsInfo(FoxTblConsentToCare consentToCareObj, UserProfile profile)
        {
            List<FoxTblConsentToCare> consentToCareList = new List<FoxTblConsentToCare>();
            ConsentToCareList response = new ConsentToCareList();
            if (consentToCareObj != null)
            {
                var caseId = new SqlParameter("@CASE_ID", SqlDbType.BigInt) { Value = consentToCareObj.CASE_ID };
                var practiceCode = new SqlParameter("PRACTICE_CODE", SqlDbType.BigInt) { Value = GetPracticeCode() };
                response.ConsentToCareDbList = SpRepository<FoxTblConsentToCare>.GetListWithStoreProcedure(@"EXEC FOX_PROC_GET_CONSENT_TO_CARE_INFO_BY_CASE_ID @CASE_ID, @PRACTICE_CODE", caseId, practiceCode);
            }
            return response;
        }
        
        public long GetPracticeCode()
        {
            long practiceCode = Convert.ToInt64(WebConfigurationManager.AppSettings?["GetPracticeCode"]);
            return practiceCode;
        }
        public string GeneratePdfForConcentToCare(FoxTblConsentToCare consentToCareObj, string practiceDocumentDirectory)
        {
            try
            {
                consentToCareObj.PATIENT_ACCOUNT = long.Parse(consentToCareObj.PATIENT_ACCOUNT_Str == null ? "0" : consentToCareObj.PATIENT_ACCOUNT_Str);
                var practiceCode = GetPracticeCode();
                var userName = "FOX TEAM";
                var config = Helper.GetServiceConfiguration(practiceCode);
                var htmlTemplate = consentToCareObj.TEMPLATE_HTML;
                var consentToCareIdStr = consentToCareObj.CONSENT_TO_CARE_ID.ToString();
                var coverFilePath = HTMLToPDFSautinsoft(config, htmlTemplate, consentToCareIdStr);
                var consentToCareID = consentToCareObj.CONSENT_TO_CARE_ID;
                var CASE_ID = consentToCareObj.CASE_ID;
                var currentConsentToCareId = consentToCareObj.CONSENT_TO_CARE_ID;
                SavePdfToImages(coverFilePath, config, currentConsentToCareId, CASE_ID.ToString(), CASE_ID, 1, "DR:Fax", "", userName, true);
                return coverFilePath;
                //var queue = _consentToCareDocumentRepository.GetSingle(e => e.CASE_ID == consentToCareObj.CASE_ID && e.CONSENT_TO_CARE_ID == consentToCareObj.CONSENT_TO_CARE_ID);
                //var queue = _consentToCareDocumentRepository.GetSingleOrDefault(e => e.CASE_ID == consentToCareObj.CASE_ID);
                //if (queue != null)
                //{
                //    var localPath = practiceDocumentDirectory + "/" + queue.CASE_ID + " __" + DateTime.Now.Ticks + ".pdf";
                //    //var pathForPDF = @"\\10.10.30.165\\" + practiceDocumentDirectory + queue.CASE_ID + " __" + DateTime.Now.Ticks + ".pdf";
                //    var pathForPDF = Path.Combine(HttpContext.Current.Server.MapPath(@"~/" + practiceDocumentDirectory), queue.CASE_ID + " __" + DateTime.Now.Ticks + ".pdf");
                //    ImageHandler imgHandler = new ImageHandler();
                //    var imges = _consentToCareDocumentRepository.GetMany(x => x.CASE_ID == consentToCareObj.CASE_ID);
                //    if (imges != null && imges.Count > 0)
                //    {
                //        var imgPaths = (from x in imges select x.IMAGE_PATH).ToArray();
                //        imgHandler.ImagesToPdf(imgPaths, pathForPDF);
                //        return coverFilePath;
                //    }
                //}
                return "";
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static string EmailBody(string patientFirstName, string link)
        {
            string mailBody = string.Empty;
            string templatePathOfSenderEmail = AppDomain.CurrentDomain.BaseDirectory;
            templatePathOfSenderEmail = templatePathOfSenderEmail.Replace(@"\bin\Debug", "") + "HtmlTemplates\\Consent-To-Care-Email_Template.html";
            if (File.Exists(templatePathOfSenderEmail))
            {
                mailBody = File.ReadAllText(templatePathOfSenderEmail);
                mailBody = mailBody.Replace("[[PATIENT_FIRST_NAME]]", patientFirstName);
                mailBody = mailBody.Replace("[[LINK]]", link);
            }
            return mailBody ?? "";
        }
        public static string GenerateEncryptedEmailURL(FoxTblConsentToCare consentToCareObj)
        {
            string encryptedUrl = string.Empty;
            if (consentToCareObj != null)
            {
                //var modifiedSurveyId = ModifiedSurveyId(patientDetail.SURVEY_ID);
                //var timeSpan = TimeSpan();
                string environmentURL = GetClientURL() + "#/ConsentToCare";
                string conactURL = consentToCareObj.CASE_ID + "#";
                //encryptedUrl = Encrypt(consentToCareObj.CASE_ID.ToString(), "sblw-3hn8-sqoy19").ToString();
                encryptedUrl = EncryptTemp(consentToCareObj.CASE_ID.ToString());
                encryptedUrl = environmentURL + "/#" + encryptedUrl + "#";
            }
            return encryptedUrl;
        }
        public static string GetClientURL()
        {
            string url = ConfigurationManager.AppSettings?["ClientURL"].ToString();
            return url ?? "";
        }
        public static string EncryptTemp(string plainText)
        {
            try
            {
                // To handle encryption/decryption Objective-C,C#
               string passPhrase = "2657894562368456";
             var plainBytes = Encoding.UTF8.GetBytes(plainText);
                return Convert.ToBase64String(Encrypt(plainBytes, GetRijndaelManaged(passPhrase)));
            }
            catch (Exception ex)
            {
                return "exception : " + ex.Message;
            }


        }
        private static byte[] Encrypt(byte[] plainBytes, RijndaelManaged rijndaelManaged)
        {
            return rijndaelManaged.CreateEncryptor().TransformFinalBlock(plainBytes, 0, plainBytes.Length);
        }
        // To handle encryption/decryption java,C#
        private static RijndaelManaged GetRijndaelManaged(string secretKey)
        {
            var keyBytes = new byte[16];
            var secretKeyBytes = Encoding.UTF8.GetBytes(secretKey);
            Array.Copy(secretKeyBytes, keyBytes, Math.Min(keyBytes.Length, secretKeyBytes.Length));
            return new RijndaelManaged
            {
                Mode = CipherMode.CBC,
                Padding = PaddingMode.PKCS7,
                KeySize = 128,
                BlockSize = 128,
                Key = keyBytes,
                IV = keyBytes
            };
        }
        public static string Encrypt(string input, string key)
        {
            byte[] inputArray = UTF8Encoding.UTF8.GetBytes(input);
            TripleDESCryptoServiceProvider tripleDES = new TripleDESCryptoServiceProvider
            {
                Key = UTF8Encoding.UTF8.GetBytes(key),
                Mode = CipherMode.ECB,
                Padding = PaddingMode.PKCS7
            };
            ICryptoTransform cTransform = tripleDES.CreateEncryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(inputArray, 0, inputArray.Length);
            tripleDES.Clear();
            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }
        // Description: This function is get survey method details
        public static string GetSurveyMethod(string surveyMethodRemoveChar)
        {
            switch (surveyMethodRemoveChar)
            {
                case "1":
                    surveyMethodRemoveChar = "SMS";
                    break;
                case "2":
                    surveyMethodRemoveChar = "EMAIL";
                    break;
                case "3":
                    surveyMethodRemoveChar = "UnsubscribeSMS";
                    break;
                case "4":
                    surveyMethodRemoveChar = "UnsubscribeEmail";
                    break;
                default:
                    surveyMethodRemoveChar = "";
                    break;
            }
            return surveyMethodRemoveChar;
        }
        #endregion
        #region Email & SMS body 
        // Description: This function is used for Email body
        public static string EmailBody(string patientFirstName)
        {
            string mailBody = string.Empty;
            string templatePathOfSenderEmail = AppDomain.CurrentDomain.BaseDirectory;
            templatePathOfSenderEmail = templatePathOfSenderEmail.Replace(@"\bin\Debug", "") + "HtmlTemplates\\UnsubscribeAutomatedPatientSurveyEmailTemplate.html";
            if (File.Exists(templatePathOfSenderEmail))
            {
                mailBody = File.ReadAllText(templatePathOfSenderEmail);
                mailBody = mailBody.Replace("[[PATIENT_FIRST_NAME]]", patientFirstName);
            }
            return mailBody ?? "";
        }
        // Description: This function is used for send email
        public static bool SendEmail(string to, string subject, string body, List<string> CC = null, List<string> BCC = null, string AttachmentFilePaths = null, string from = "foxrehab@carecloud.com")
        {
            bool IsMailSent = false;
            var bodyHTML = "";
            bodyHTML += "<body>";
            bodyHTML += body;
            bodyHTML += "</body>";
            try
            {
                using (SmtpClient smtp = new SmtpClient())
                {
                    using (MailMessage mail = new MailMessage())
                    {
                        mail.From = new MailAddress(from);
                        mail.To.Add(new MailAddress(to));
                        mail.Subject = subject;
                        mail.Body = bodyHTML;
                        mail.IsBodyHtml = true;
                        mail.SubjectEncoding = Encoding.UTF8;
                        if (CC != null && CC.Count > 0)
                        {
                            foreach (var item in CC) { mail.CC.Add(item); }
                        }
                        if (BCC != null && BCC.Count > 0)
                        {
                            foreach (var item in BCC) { mail.Bcc.Add(item); }
                        }
                        if (AttachmentFilePaths != null)
                        {
                            if (File.Exists(AttachmentFilePaths)) { mail.Attachments.Add(new Attachment(AttachmentFilePaths)); }
                        }
                        smtp.Credentials = new System.Net.NetworkCredential(ConfigurationManager.AppSettings["FoxRehabUserName"], ConfigurationManager.AppSettings["FoxRehabPassword"]);
                        smtp.Send(mail);
                        IsMailSent = true;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return IsMailSent;
        }
        // Description: This function is used for get list of Email CC
        public static List<string> GetEmailCCList()
        {
            List<string> cc = new List<string>();
            var ccUsers = ConfigurationManager.AppSettings?["SurveyAutomationCCList"].ToString();
            if (!string.IsNullOrEmpty(ccUsers))
            {
                cc = ccUsers.Split(',').ToList();
            }
            return cc ?? null;
        }
        // Description: This function is used for get list of Email BCC
        public static List<string> GetEmailBCCList()
        {
            List<string> bcc = new List<string>();
            var ccUsers = ConfigurationManager.AppSettings?["SurveyAutomationBCCList"].ToString();
            if (!string.IsNullOrEmpty(ccUsers))
            {
                bcc = ccUsers.Split(',').ToList();
            }
            return bcc ?? null;
        }
        // Description: This function is used forcreate SMS body
        public static string SmsBody(string patientFirstName)
        {
            string smsBody = "Hello " + patientFirstName + "!\n \nYour request to unsubscribe from receiving patient surveys is received. You will not receive any messages with patient survey link in future.\n\nRegards\n\nFox Rehab Team ";
            return smsBody ?? "";
        }
        #endregion
    }
}
