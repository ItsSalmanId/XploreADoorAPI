using FOX.DataModels.Context;
using FOX.DataModels.GenericRepository;
using FOX.DataModels.HelperClasses;
//using FOX.DataModels.Models.Exceptions;
using FOX.DataModels.Models.Patient;
using FOX.DataModels.Models.Security;
using FOX.DataModels.Models.WorkOrderHistoryModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Transactions;
using System.Web;
using FOX.DataModels.Models.ServiceConfiguration;
using FOX.BusinessOperations.CommonServices;
using FOX.DataModels.Models.CommonModel;
using FOX.DataModels.Models.OriginalQueueModel;
using FOX.DataModels.Models.IndexInfo;
using System.Web.Configuration;

namespace FOX.BusinessOperations.CommonService
{
    public static class Helper
    {
        private static DbContextCommon _DbContextSP;
        private static DbContextIndexinfo _IndexinfoContext;
        //private static readonly DBContextExceptionLog _DbContextExceptionLog = new DBContextExceptionLog();
        private static GenericRepository<Maintenance_Counter> _MaintenanceCounterRepositry;
        //private static readonly GenericRepository<WorkOrderHistory> _WorkHistoryRepository = new GenericRepository<WorkOrderHistory>(_DbContextSP);
        private static GenericRepository<User> _UserRepository;
        private static GenericRepository<Patient> _PatientRepository;
        //private static readonly GenericRepository<FOX_TBL_SENDER> _SenderRepository = new GenericRepository<FOX_TBL_SENDER>(_DbContextSP);
        //private static readonly GenericRepository<FOX_TBL_EXCEPTION_LOG> _ExceptionLogRepository = new GenericRepository<FOX_TBL_EXCEPTION_LOG>(_DbContextExceptionLog);
        //private static readonly GenericRepository<EmailFaxLog> _emailfaxlogRepository = new GenericRepository<EmailFaxLog>(_DbContextSP);
        private static GenericRepository<OriginalQueue> _InsertSourceAddRepository;
        private static GenericRepository<FoxDocumentType> _foxdocumenttypeRepository;
        
        static Helper()
        {
            _DbContextSP = new DbContextCommon();
            _IndexinfoContext = new DbContextIndexinfo();
            _MaintenanceCounterRepositry = new GenericRepository<Maintenance_Counter>(_DbContextSP);
            _UserRepository = new GenericRepository<User>(_DbContextSP);
            _PatientRepository = new GenericRepository<Patient>(_DbContextSP);
            _InsertSourceAddRepository = new GenericRepository<OriginalQueue>(_DbContextSP);
            _foxdocumenttypeRepository = new GenericRepository<FoxDocumentType>(_IndexinfoContext);
        }
        public static void InitilizeUpdatedValues()
        {
            _DbContextSP = new DbContextCommon();
            _IndexinfoContext = new DbContextIndexinfo();
            _MaintenanceCounterRepositry = new GenericRepository<Maintenance_Counter>(_DbContextSP);
            _UserRepository = new GenericRepository<User>(_DbContextSP);
            _PatientRepository = new GenericRepository<Patient>(_DbContextSP);
            _InsertSourceAddRepository = new GenericRepository<OriginalQueue>(_DbContextSP);
            _foxdocumenttypeRepository = new GenericRepository<FoxDocumentType>(_IndexinfoContext);
        }

        public static long getMaximumId(string columnName)
        {

            var columnNamePar = new SqlParameter("Col_Name", SqlDbType.VarChar) { Value = columnName };
            var maxColumn = SpRepository<Web_GetMaxColumnID>.GetListWithStoreProcedure(@"exec Web_GetMaxColumnID @Col_Name", columnNamePar).SingleOrDefault();
            if (maxColumn == null || maxColumn.MaxID == null)
            {
                InsertMaxColumn(columnName);
                var columnNamePar2 = new SqlParameter("Col_Name", SqlDbType.VarChar) { Value = columnName };
                maxColumn = SpRepository<Web_GetMaxColumnID>.GetListWithStoreProcedure(@"exec Web_GetMaxColumnID @Col_Name", columnNamePar2).SingleOrDefault();
            }
            return Convert.ToInt64(maxColumn.MaxID);
        }

        public static long getMaximumLogId(string columnName)
        {
            var columnNamePar = new SqlParameter("Col_Name", SqlDbType.VarChar) { Value = columnName };
            var maxColumn = SpRepository<Web_GetMaxColumnID>.GetListWithStoreProcedure(@"exec FOX_PROC_GETMAXCOLUMNID @Col_Name", columnNamePar).SingleOrDefault();
            if (maxColumn == null || maxColumn.MaxID == null)
            {
                InsertMaxColumn(columnName);
                var columnNamePar2 = new SqlParameter("Col_Name", SqlDbType.VarChar) { Value = columnName };
                maxColumn = SpRepository<Web_GetMaxColumnID>.GetListWithStoreProcedure(@"exec FOX_PROC_GETMAXCOLUMNID @Col_Name", columnNamePar2).SingleOrDefault();
            }
            return Convert.ToInt64(maxColumn.MaxID);
        }

        public static long getMaximumIdForPatientAndAppointment(string tableName, string columnName, string practiceCode)
        {
            var tableNameParam = new SqlParameter("TableName", SqlDbType.VarChar) { Value = tableName };
            var columnNameParam = new SqlParameter("colName", SqlDbType.VarChar) { Value = columnName };
            var practiceCodeParam = new SqlParameter("Practice_code", SqlDbType.VarChar) { Value = practiceCode };
            var maxColumn = SpRepository<PatAppMaxColumnID>.GetListWithStoreProcedure(@"exec Web_Proc_GetMaxID @TableName,@colName,@Practice_code", tableNameParam, columnNameParam, practiceCodeParam).SingleOrDefault();
            return Convert.ToInt64(maxColumn.MaxColumnID);
        }


        private static void InsertMaxColumn(string columnName)
        {
            using (var scope = new TransactionScope())
            {
                var maintenanceCounter = new Maintenance_Counter
                {
                    Col_Name = columnName,
                    Col_Counter = 100
                    //,
                    //SUPPORT_CALL_ID = 0,
                    //SUPPORT_TRAINING_ID = 0
                };
                _MaintenanceCounterRepositry.Insert(maintenanceCounter);
                _MaintenanceCounterRepositry.Save();
                scope.Complete();
            }
        }

        public static string GetIpAddress()
        {
            return HttpContext.Current.Request.UserHostAddress;
        }

        public static DateTime GetCurrentDate()
        {
            return DateTime.Now;
        }
        public static bool isCode(string refSearch)
        {
            if (!string.IsNullOrEmpty(refSearch) && refSearch.Length > 2)
            {
                if (!char.IsDigit(refSearch[0]) && char.IsDigit(refSearch[1]) && char.IsDigit(refSearch[2]))
                {
                    return true;
                }
                else if (char.IsDigit(refSearch[0]) && char.IsDigit(refSearch[1]) && char.IsDigit(refSearch[2]))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
        public static bool SendEmail(string messageTo, string subject, string body,long? WORK_ID = null, UserProfile profile = null, List<string> CC = null, List<string> BCC = null, string senderEmail = "foxrehab@carecloud.com")
        {
            var bodyHTML = "";
            bodyHTML += "<body>";
            bodyHTML += body;
            bodyHTML += "</body>";
            SmtpClient client = new SmtpClient();
            MailMessage msg = new MailMessage(senderEmail, messageTo);
            if (CC != null)
            {
                foreach (var item in CC)
                {
                    if(!String.IsNullOrWhiteSpace(item))
                        msg.CC.Add(item);
                }
            }

            if (BCC != null)
            {
                foreach (var item in BCC)
                {
                    if (!String.IsNullOrWhiteSpace(item))
                        msg.Bcc.Add(item);
                }
            }

            try
            {
                msg.Subject = subject;
                msg.BodyEncoding = System.Text.Encoding.UTF8;
                msg.Body = bodyHTML;
                msg.IsBodyHtml = true;
                msg.Priority = MailPriority.Normal;
                client.Credentials = new System.Net.NetworkCredential(WebConfigurationManager.AppSettings["FoxRehabUserName"], WebConfigurationManager.AppSettings["FoxRehabPassword"]);
                client.Send(msg);
                LogEmailData(messageTo,"Success",profile,CC,BCC, senderEmail, null, WORK_ID, null);
                return true;
            }
            catch (Exception ex)
            {
                LogEmailData(messageTo, "Failed", profile, CC, BCC, senderEmail, ex, WORK_ID, null);
                return false;
            }
        }

        //public static bool Email(string from, string to, string subject, string body, List<string> CC = null, List<string> BCC = null, List<string> AttachmentFilePaths = null)
        public static bool Email(string to, string subject, string body,UserProfile profile =null, long? WORK_ID=null, List<string> CC = null, List<string> BCC = null, List<string> AttachmentFilePaths = null, string from = "foxrehab@carecloud.com")
        {
            bool IsMailSent = false;
            try
            {

                using (SmtpClient smtp = new SmtpClient())
                {
                    using (MailMessage mail = new MailMessage())
                    {
                        mail.From = new MailAddress(from);
                        mail.To.Add(new MailAddress(to));
                        mail.Subject = subject;
                        mail.Body = body;
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
                        if (AttachmentFilePaths != null && AttachmentFilePaths.Count > 0)
                        {
                            foreach (string filePth in AttachmentFilePaths)
                            {
                                if (File.Exists(filePth)) { mail.Attachments.Add(new Attachment(filePth)); }
                            }
                        }
                        smtp.Credentials = new System.Net.NetworkCredential(WebConfigurationManager.AppSettings["FoxRehabUserName"], WebConfigurationManager.AppSettings["FoxRehabPassword"]);
                        smtp.Send(mail);
                        LogEmailData(to,"Success",profile,CC,BCC, from,null,WORK_ID, AttachmentFilePaths);
                        IsMailSent = true;
                    }
                }

                /////////////////////////////////////
                //SmtpClient smtp = new SmtpClient();
                //MailMessage mail = new MailMessage();
                //mail.From = new MailAddress(from);
                //mail.To.Add(new MailAddress(to));
                //mail.Subject = subject;
                //mail.Body = body;
                //mail.IsBodyHtml = true;
                //mail.SubjectEncoding = Encoding.UTF8;
                //if (CC != null && CC.Count > 0)
                //{
                //    foreach (var item in CC) { mail.CC.Add(item); }
                //}
                //if (BCC != null && BCC.Count > 0)
                //{
                //    foreach (var item in BCC) { mail.Bcc.Add(item); }
                //}
                //if (AttachmentFilePaths != null && AttachmentFilePaths.Count > 0)
                //{
                //    foreach (string filePth in AttachmentFilePaths)
                //    {
                //        if (File.Exists(filePth)) { mail.Attachments.Add(new Attachment(filePth)); }
                //    }
                //}
                //smtp.Send(mail);
                //LogEmailData(to,"Success",profile,CC,BCC, from,null,WORK_ID, AttachmentFilePaths);
                //IsMailSent = true;
            }
            catch (Exception ex)
            {
                LogEmailData(to, "Failed", profile, CC, BCC, from, ex, WORK_ID, AttachmentFilePaths);
                IsMailSent = false;
            }
            return IsMailSent;
        }
        public static string GetAgeInWeekAndDayFormat(string Age)
        {

            //"1Y,11M,0D";
            var FormattedAge = "";
            int IntYear = 0; int IntMonth = 0; int IntDays = 0;
            var CleanStr = Regex.Replace(Age, "[^,0-9 ]", "");
            int.TryParse(CleanStr.Split(',')[0], out IntYear);
            int.TryParse(CleanStr.Split(',')[1], out IntMonth);
            int.TryParse(CleanStr.Split(',')[2], out IntDays);

            #region For 2 + years
            //For children 2 + years à age in years(Yrs)
            if (IntYear >= 2)
            {
                FormattedAge = string.Format("{0} yrs", IntYear);
            }
            #endregion
            #region For children up to 1 month
            //For children up to 1 month à age in weeks(wks and days)
            else if (IntYear == 0 && IntMonth == 0 && IntDays > 0)
            {
                // age>=7 Days (weeks and days Calculation)
                if (IntDays >= 7)
                {
                    var weeks = int.Parse((IntDays / 7).ToString());
                    var Days = int.Parse((IntDays % 7).ToString());
                    FormattedAge = string.Format("{0} wks and {1} days", weeks, Days);
                }
                else // child's age < a week
                {
                    FormattedAge = IntDays + " days";
                }

            }
            #endregion
            else if (IntYear == 0 && IntMonth == 0 && IntDays == 0)
            {
                FormattedAge = "1 day";
            }
            #region For children 1 month to 23 months
            //For children 1 month to 23 months à age in months(mos)
            else if ((IntYear >= 0 && IntYear < 2) || (IntMonth >= 1))
            {
                var Months = (IntYear * 12) + IntMonth;
                FormattedAge = string.Format("{0} mos", Months);
            }
            #endregion

            return FormattedAge;
        }
        //public static void LogException(Exception ex)
        //{
        //    if (!string.IsNullOrEmpty(ex.Message))
        //    {
        //        string directory = @"e:\\Errors";
        //        if (!Directory.Exists(directory))
        //        {
        //            Directory.CreateDirectory(directory);
        //        }
        //        string filePath = directory + "\\Fox.txt";
        //        using (StreamWriter writer = new StreamWriter(filePath, true))
        //        {
        //            writer.WriteLine("Message :" + ex.Message + "<br/>" + Environment.NewLine + "StackTrace :" + ex.StackTrace + Environment.NewLine + Environment.NewLine +
        //                "///------------------Inner Exception------------------///" + Environment.NewLine + ((ex.InnerException != null && ex.InnerException.Message != null) ? ex.InnerException.Message : "NULL") + Environment.NewLine +
        //                "Date :" + DateTime.Now.ToString() + Environment.NewLine + Environment.NewLine + "-------------------------------------------------------||||||||||||---End Current Exception---||||||||||||||||-------------------------------------------------------" + Environment.NewLine);
        //        }
        //    }
        //}

        public static string LogADException(Exception ex)
        {
            string textToType = string.Empty;
            if (ex != null)
            {
                var excpMsg = ex.Message;
                var excpStackTrace = ex.StackTrace;
                var excpInnerMessage = ((ex.InnerException != null && ex.InnerException.Message != null) ? (ex.InnerException.Message.ToLower().Contains("inner exception") ? ex.InnerException.InnerException.Message : ex.InnerException.Message) : "NULL");
                string directoryOther = System.Web.HttpContext.Current.Server.MapPath("\\FoxCriticalErrors");
                if (!Directory.Exists(directoryOther))
                {
                    Directory.CreateDirectory(directoryOther);
                }
                string filePathOther = directoryOther + "\\errors_" + DateTime.Now.Date.ToString("MM-dd-yyyy") + ".txt";
                textToType = "Message: " + excpMsg + Environment.NewLine + Environment.NewLine + "StackTrace: " + excpStackTrace + Environment.NewLine + Environment.NewLine +
                       "///------------------Inner Exception------------------///" + Environment.NewLine + excpInnerMessage + "" + Environment.NewLine +
                       "Date: " + DateTime.Now.ToString() + Environment.NewLine + Environment.NewLine + "-------------------------------------------------------||||||||||||---End Current Exception---||||||||||||||||-------------------------------------------------------" + Environment.NewLine;
                using (StreamWriter writer = new StreamWriter(filePathOther, true))
                {
                    writer.WriteLine(textToType);
                }
            }
            return textToType.Replace("\n", "<br />");
        }

        public static async void LogADLoginStatus(string UserName, string Status, string FailedReason)
        {
            string directory = System.Web.HttpContext.Current.Server.MapPath(AppConfiguration.LogADUserStatus);
            string ServerName = System.Web.HttpContext.Current.Server.MachineName + " ";
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
            string filePath = directory + "\\" + ServerName + DateTime.Now.Date.ToString("MM-dd-yyyy") + ".csv";
            bool fileExists = true;
            //try
            //{
                if (!File.Exists(filePath))
                {
                    fileExists = false;
                }
                using (StreamWriter writer = new StreamWriter(filePath, true))
                {
                    if (!fileExists)
                    {
                        await writer.WriteLineAsync("User Name, Status, Failed Reason, Date/Time");
                        fileExists = true;
                    }
                    await writer.WriteLineAsync(UserName + "," + Status + "," +  FailedReason + "," + DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss tt"));
                }
            //}
            //catch (Exception ex)
            //{

            //}
        }

        public static void LogADProperties(string Email, Dictionary<string, string> Properties)
        {
            string directory = System.Web.HttpContext.Current.Server.MapPath(AppConfiguration.LogADProperties);
            string FileName = Email;
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
            FileName = Email.Replace("/", " ").Replace(@"\", " ").Replace(":", " ").Replace("*", " ").Replace("?", " ").Replace(@"""", @" ").Replace("<", " ").Replace(">", " ").Replace("|", " ");
            StringBuilder values = new StringBuilder("============================= START OF PROPERTIES ================================================" + Environment.NewLine);

            foreach (KeyValuePair<string, string> property in Properties)
            {
                values.Append(property.Key + "\t\t" + property.Value + Environment.NewLine);
            }

            values.Append("============================= END OF PROPERTIES ================================================" + Environment.NewLine);
            string filePath = directory + "\\" + FileName + ".txt";

            using (StreamWriter writer = new StreamWriter(filePath, true))
            {
                writer.Write(values.ToString());
            }
        }

        public static string ValidStringValue(string value, string type)
        {
            if (type == "address")
            {
                value = value.Replace("'", "&#39;");
                value = value.Replace("{", "");
                value = value.Replace("}", "");
                value = value.Replace("%", "");
                value = value.Replace("^", "");
                value = value.Replace("~", "");
            }
            else
            {
                if (!string.IsNullOrEmpty(value))
                {
                    value = value.Replace("'", "&#39;");
                    value = value.Replace("{", "");
                    value = value.Replace("}", "");
                    value = value.Replace("#", "");
                    value = value.Replace("%", "");
                    value = value.Replace("^", "");
                    value = value.Replace("~", "");
                    value = value.Replace("!", "");
                    value = value.Replace("`", "&#39;");


                }
            }
            return value;
        }
        public static bool ValidateStringValue(string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                if (Regex.IsMatch(value, "^[a-zA-Z-_]+$"))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            return true;
        }
        public static bool CheckNumberValue(string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                long no;
                if (long.TryParse(value, out no))
                {
                    if (value.Length == 10)
                    {
                        return true;
                    }
                    return false;
                }
                else
                {
                    return false;
                }
            }
            return true;
        }
        public static bool CheckNumberValueExt(string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                long no;
                if (long.TryParse(value, out no))
                {
                    if (value.Length > 10)
                    {
                        return false;
                    }
                    return true;
                }
                else
                {
                    return false;
                }
            }
            return true;
        }
        public static string WriteErrorFile(string text, string practiceCode)
        {
            string Directorypath = HttpContext.Current.Server.MapPath(@"~/talkEHR/" + practiceCode + "/TempAttached");
            if (!Directory.Exists(Directorypath))
            {
                Directory.CreateDirectory(Directorypath);
            }
            var path = Directorypath + "/InvalidRecords.txt";
            if (!File.Exists(path))
            {
                //File.Create(path);
                //TextWriter tw = new StreamWriter(path);
                //tw.WriteLine(text);
                //tw.Close();

                // Create the file.
                using (FileStream fs = File.Create(path))
                {
                    Byte[] info = new UTF8Encoding(true).GetBytes(text + Environment.NewLine);
                    // Add some information to the file.
                    fs.Write(info, 0, info.Length);
                }
            }
            else
            {
                TextWriter tw = new StreamWriter(path, true);
                tw.WriteLine(Environment.NewLine + text);
                tw.Close();
            }
            return path;
        }
        public static void GetDateFromCritaria(string Criteria, ref string DateFrom, ref string DateTo)
        {
            if (Criteria == "monthtodate")
            {
                DateFrom = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).ToString("MM/dd/yyyy");
                DateTo = DateTime.Now.ToString("MM/dd/yyyy");
            }
            else if (Criteria == "last30days")
            {
                DateFrom = ((DateTime.Now).AddDays(-30)).ToString("MM/dd/yyyy");
                DateTo = DateTime.Now.ToString("MM/dd/yyyy");
            }
            else if (Criteria == "lastmonth")
            {
                var today = DateTime.Today;
                var month = new DateTime(today.Year, today.Month, 1);
                DateFrom = month.AddMonths(-1).ToString("MM/dd/yyyy");
                DateTo = month.AddDays(-1).ToString("MM/dd/yyyy");
            }
            else if (Criteria == "yeartodate")
            {
                DateFrom = new DateTime(DateTime.Now.Year, 1, 1).ToString("MM/dd/yyyy");
                DateTo = DateTime.Now.ToString("MM/dd/yyyy");
            }
            else if (Criteria == "lastyear")
            {
                DateFrom = new DateTime(DateTime.Now.Year - 1, 1, 1).ToString("MM/dd/yyyy");
                DateTo = new DateTime(DateTime.Now.Year - 1, 12, 31).ToString("MM/dd/yyyy");
            }
        }
        public static string getConnectionString()
        {
            return "FOXConnection";
        }

        public static string ConvertToUSFormatedPhone(string phoneNumber)
        {
            string UsFormatedNumber = "";
            if (!string.IsNullOrEmpty(phoneNumber) && phoneNumber.Length == 10)
            {
                UsFormatedNumber = "(" + phoneNumber.Substring(0, 3) + ") " + phoneNumber.Substring(3, 3) + "-" + phoneNumber.Substring(6, 4);
            }
            return UsFormatedNumber;
        }

        public static bool IsValidEmail(string email)
        {
            try
            {
                var addr = new MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        public static DateTime ChangeTime(this DateTime dateTime, int hours)
        {
            return new DateTime(
                dateTime.Year,
                dateTime.Month,
                dateTime.Day,
                hours,
                00,
                01,
                dateTime.Kind);
        }

        public static string ChangeStringToTitleCase(string str)
        {
            if (string.IsNullOrEmpty(str)) { return ""; }
            str = str.ToLower();
            System.Globalization.TextInfo strConverter = new System.Globalization.CultureInfo("en-US", false).TextInfo;
            str = strConverter.ToTitleCase(str);
            return str;
        }

        public static SqlParameter getDBNullOrValue(string parameterName, string value)
        {
            return string.IsNullOrWhiteSpace(value) ? (new SqlParameter(parameterName, SqlDbType.VarChar) { Value = DBNull.Value }) : (new SqlParameter(parameterName, SqlDbType.VarChar) { Value = value });
        }

        public static string DateFormateForInsuranceEligibility(DateTime dt)
        {
            try
            {
                return dt.Year.ToString() + ((dt.Month.ToString().Length == 1 ? ("0" + dt.Month).ToString() : dt.Month.ToString())) + (dt.Day.ToString().Length == 1 ? ("0" + dt.Day.ToString()) : dt.Day.ToString()).ToString();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public static void LogMultipleWorkOrderChanges(long workId, string uniqueId, List<string> logMessage, string user)
        {
            //foreach (var msg in logMessage)
            //{
            //    if (!string.IsNullOrEmpty(msg))
            //    {
            //DataTable dt = GetLogTable(logMessage);
            if (logMessage != null && logMessage.Count > 0)
            {
                foreach (var item in logMessage)
                {
                    List<string> temp = new List<string>();
                    temp.Add(item);
                    DataTable dt = GetLogTable(temp);

                    if (dt.Rows.Count > 0)
                    {
                        long Pid = Helper.getMaximumId("LOG_ID");
                        SqlParameter id = new SqlParameter("ID", Pid);
                        SqlParameter work_Id = new SqlParameter("WORK_ID", workId);
                        SqlParameter unique_Id = new SqlParameter("UNIQUE_ID", uniqueId);
                        SqlParameter MSG = new SqlParameter("ORIGNAL_QUEUE_HISTORY", SqlDbType.Structured);
                        MSG.TypeName = "ORIGNAL_QUEUE_HISTORY";
                        MSG.Value = dt;
                        SqlParameter user_Name = new SqlParameter("USER_NAME", user);
                        SpRepository<WorkOrderHistory>.GetSingleObjectWithStoreProcedure(@"FOX_PROC_INSERT_ORIGNAL_QUEUE_HISTORY @ORIGNAL_QUEUE_HISTORY, @WORK_ID,  @ID, @UNIQUE_ID,  @USER_NAME", MSG, work_Id, id, unique_Id, user_Name);
                    }
                }
            }
            //    }
            //}

            //DbContextCommon _DbContextSP_New = new DbContextCommon();
            //GenericRepository<WorkOrderHistory> _WorkHistoryRepository = new GenericRepository<WorkOrderHistory>(_DbContextSP_New);
            //foreach (var msg in logMessage)
            //{
            //    WorkOrderHistory workHistory = new WorkOrderHistory();
            //    //workHistory.LOG_ID = getMaximumLogId("FOXREHAB_LOGID");
            //    workHistory.LOG_ID = getMaximumLogId("LOG_ID");

            //    workHistory.WORK_ID = workId;
            //    workHistory.UNIQUE_ID = uniqueId;
            //    workHistory.LOG_MESSAGE = msg;
            //    workHistory.CREATED_BY = user;
            //    workHistory.CREATED_ON = GetCurrentDate();

            //    _WorkHistoryRepository.Insert(workHistory);
            //    _WorkHistoryRepository.Save();
            //}
        }

        private static DataTable GetLogTable(List<string> lst)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("MSG", typeof(string));
            foreach(string msg in lst)
            {
                if (!string.IsNullOrEmpty(msg))
                {
                    dt.Rows.Add(msg);
                }
            }
            return dt;

        }

        public static void LogSingleWorkOrderChange(long workId, string uniqueId, string logMessage, string user)
        {
            if (!string.IsNullOrEmpty(logMessage))
            {
                List<string> temp = new List<string>();
                temp.Add(logMessage);
                DataTable dt = GetLogTable(temp);

                long Pid = Helper.getMaximumId("LOG_ID");
                SqlParameter id = new SqlParameter("ID", Pid);
                SqlParameter work_Id = new SqlParameter("WORK_ID", workId);
                SqlParameter unique_Id = new SqlParameter("UNIQUE_ID", uniqueId);
                SqlParameter MSG = new SqlParameter("ORIGNAL_QUEUE_HISTORY", SqlDbType.Structured);
                MSG.TypeName = "ORIGNAL_QUEUE_HISTORY";
                MSG.Value = dt;
                SqlParameter user_Name = new SqlParameter("USER_NAME", user);
                SpRepository<WorkOrderHistory>.GetSingleObjectWithStoreProcedure(@"FOX_PROC_INSERT_ORIGNAL_QUEUE_HISTORY @ORIGNAL_QUEUE_HISTORY, @WORK_ID, @ID, @UNIQUE_ID,  @USER_NAME", MSG, work_Id, id, unique_Id, user_Name);

                //DbContextCommon _DbContextSP_New = new DbContextCommon();
                //GenericRepository<WorkOrderHistory> _WorkHistoryRepository = new GenericRepository<WorkOrderHistory>(_DbContextSP_New);
                //WorkOrderHistory workHistory = new WorkOrderHistory();

                ////workHistory.LOG_ID = getMaximumLogId("FOXREHAB_LOGID");
                //workHistory.LOG_ID = getMaximumLogId("LOG_ID");

                //workHistory.WORK_ID = workId;
                //workHistory.UNIQUE_ID = uniqueId;
                //workHistory.LOG_MESSAGE = logMessage;
                //workHistory.CREATED_BY = user;
                //workHistory.CREATED_ON = GetCurrentDate();

                //_WorkHistoryRepository.Insert(workHistory);
                //_WorkHistoryRepository.Save();
            }
        }

        public static string GetFullName(string userName)
        {
            if (!string.IsNullOrEmpty(userName))
            {
                var usr = _UserRepository.GetSingle(e => e.USER_NAME.Equals(userName));
                return usr.FIRST_NAME + " " + usr.LAST_NAME;
            }
            else
            {
                return "";
            }
        }

        public static string GetDocumentName(long? docType)
        {
            if (docType != null)
            {
                var DocumentType = _foxdocumenttypeRepository.GetFirst(d => !d.DELETED && d.DOCUMENT_TYPE_ID == docType);
                if (DocumentType != null)
                {
                    return DocumentType.NAME;
                }
                //if (docType.HasValue && docType.Value == 1)
                //{
                //    return "POC";
                //}
                //else if (docType.HasValue && docType.Value == 2)
                //{
                //    return "Refferal Order";
                //}
                //else if (docType.HasValue && docType.Value == 3)
                //{
                //    return "Other";
                //}
                else
                    return "Empty";
            }
            else
                return "Empty";
        }

        public static string GetPatientFullName(long? patAccount)
        {
            if (patAccount.HasValue)
            {
                var patient = _PatientRepository.GetFirst(e => e.Patient_Account == patAccount);
                return patient.First_Name + " " + patient.Last_Name;
            }
            else
                return "";
        }
        //public static string GetSenderName(long? senderId)
        //{
        //    if (senderId.HasValue)
        //    {
        //        var sender = _SenderRepository.GetSingle(e => e.SENDER_ID == senderId);
        //        return sender.SENDER_NAME;
        //    }
        //    else
        //        return "Empty";
        //}
        public static string GetDepartmentName(long? depId)
        {
            if (depId.HasValue && depId.Value == 1)
            {
                return "Occupational Therapy";
            }
            else if (depId.HasValue && depId.Value == 2)
            {
                return "Physical Therapy (PT)";
            }
            else if (depId.HasValue && depId.Value == 3)
            {
                return "Speech Therapy (ST)";
            }
            else if (depId.HasValue && depId.Value == 4)
            {
                return "Physical/Occupational/Speech Therapy(PT/OT/ST)";
            }
            else if (depId.HasValue && depId.Value == 5)
            {
                return "Physical/Occupational Therapy(PT/OT)";
            }
            else if (depId.HasValue && depId.Value == 6)
            {
                return "Physical/Speech Therapy(PT/ST)";
            }
            else if (depId.HasValue && depId.Value == 7)
            {
                return "Occupational/Speech Therapy(OT/ST)";
            }
            else
                return "Empty";
        }
        public static string GetDepartmentNames(string depId)
        {
            string Dis_str = string.Empty;
            if (!string.IsNullOrEmpty(depId))
            {
                if (depId.Contains("1"))
                {
                    Dis_str = Dis_str + " Occupational Therapy (OT), ";
                }
                if (depId.Contains("2"))
                {
                    Dis_str = Dis_str + " Physical Therapy (PT), ";
                }
                if (depId.Contains("3"))
                {
                    Dis_str = Dis_str + " Speech Therapy (ST), ";
                }
                if (depId == "4")
                {
                    Dis_str = Dis_str + "Physical/Occupational/Speech Therapy(PT/OT/ST)";
                }
                if (depId == "5")
                {
                    Dis_str = Dis_str + "Physical/Occupational Therapy(PT/OT)";
                }
                if (depId == "6")
                {
                    Dis_str = Dis_str + "Physical/Speech Therapy(PT/ST)";
                }
                if (depId == "7")
                {
                    Dis_str = Dis_str + "Occupational/Speech Therapy(OT/ST)";
                }
                if (depId.Contains("8"))
                {
                    Dis_str = Dis_str + " Unknown, ";
                }
                if (depId.Contains("9"))
                {
                    Dis_str = Dis_str + " Exercise Physiology (EP), ";
                }
            }
            else
            {
                Dis_str = "Empty";
            }
            if (Dis_str.Substring(Dis_str.Length - 2).Trim() == ",")
            {
                Dis_str = Dis_str.TrimEnd(Dis_str[Dis_str.Length - 1]);
            }
            return Dis_str;
        }

        static readonly Regex regexDecimal = new Regex(@"^\d*\.?\d*$"); //Only decimal
        //static readonly Regex regexDecimal = new Regex(@"^\D*?((-?(\d+(\.\d+)?))|(-?\.\d+)).*");
        static public decimal? ExtractAmountFromString(this string input)
        {
            //Match mx = regexDecimal.Match(input);
            ////Console.WriteLine("Input {0} - Digits {1} {2}", str, mx.Success, mx.Groups);
            //return mx.Success ? Convert.ToDecimal(mx.Groups[1].Value) : (decimal?)null;

            input = input.Substring(input.IndexOf("$")).Split(' ')[0];
            input = input.Replace("$", "").Replace(",", "").Replace(":", "");

            Match match = regexDecimal.Match(input.ToLower());
            if (match.Success)
            {
                decimal _dec;
                return decimal.TryParse(match.ToString(), out _dec) ? _dec : (decimal?)null;
            }
            return null;
        }
        //public static void LogException(Exception ex, UserProfile profile)
        //{
        //    try
        //    {
        //        var excepObj = new FOX_TBL_EXCEPTION_LOG();
        //        excepObj.PRACTICE_CODE = profile.PracticeCode;
        //        excepObj.SHORT_MSG = ex.Message;
        //        excepObj.LONG_MSG = ex.ToString();
        //        excepObj.LOG_DATE = DateTime.Now;
        //        excepObj.LOG_BY = profile.UserName;

        //        _ExceptionLogRepository.Update(excepObj);
        //        _ExceptionLogRepository.Save();
        //    }
        //    catch (Exception exception)
        //    {
        //        throw exception;
        //    }
        //}

        public static ServiceConfiguration GetServiceConfiguration(long practiceCode)
        {
            //practiceCode = AppConfiguration.GetPracticeCode;
            var config = new ServiceConfiguration();
            var configList = SpRepository<ServiceConfiguration>.GetListWithStoreProcedure(@"exec FOX_PROC_GET_SERVICE_CONFIGURATION");
            if (configList.Count() > 0)
            {
                config = configList.Where(e => e.PRACTICE_CODE.HasValue && e.PRACTICE_CODE.Value == practiceCode).FirstOrDefault();
            }
            return config;
        }

        public static DateTime? ConvertStingToDateTime(string _dateTimeString)
        {
            try
            {
                if (!string.IsNullOrEmpty(_dateTimeString))
                    return Convert.ToDateTime(_dateTimeString);
                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static int GetRandomPin()
        {
            Random rnd = new Random();
            return rnd.Next(10000, 99999);
        }

        public static void LogEmailData(string to, string status, UserProfile profile ,List<string> CC = null, List<string> BCC = null, string senderEmail = "foxrehab@carecloud.com", Exception ex = null, long? work_id = null, List<string> attachments = null)
        {
            DbContextCommon _DbContextSP_New = new DbContextCommon();
            GenericRepository<EmailFaxLog> _emailfaxlogRepository = new GenericRepository<EmailFaxLog>(_DbContextSP_New);

            EmailFaxLog logToInsert = new EmailFaxLog();
            logToInsert.FOX_EMAIL_FAX_LOG_ID = getMaximumId("FOX_EMAIL_FAX_LOG_ID");
            if(attachments!=null && attachments.Count>0)
            {
                logToInsert.ATTACHMENT_PATH = String.Join(" ", attachments.ToArray());
            }
            if (CC != null && CC.Count > 0)
            {
                logToInsert.CC = String.Join(" ", CC.ToArray());
            }
            if (BCC != null && BCC.Count > 0)
            {
                logToInsert.BCC = String.Join(" ", BCC.ToArray());
            }
            logToInsert.STATUS = status;
            if(ex!=null)
            {
            logToInsert.EXCEPTION_SHORT_MSG = ex.Message;
             logToInsert.EXCEPTION_TRACE = ex.StackTrace;
            }

            logToInsert.WORK_ID = work_id;
            if (logToInsert.WORK_ID != null)
            {
                var _workIdRes = new SqlParameter("WORK_ID", SqlDbType.BigInt) { Value = logToInsert.WORK_ID };
                var _practice_Code = new SqlParameter("PRACTICE_CODE", SqlDbType.BigInt) { Value = profile.PracticeCode };
                var work_order = SpRepository<WORK_ID_MODEL>.GetSingleObjectWithStoreProcedure(@"exec [FOX_TBL_GET_WORK_ID] @WORK_ID,@PRACTICE_CODE", _workIdRes, _practice_Code);
                if (work_order != null && work_order.WORK_STATUS != null)
                {
                    logToInsert.WORK_STATUS = work_order.WORK_STATUS;
                }
            }
            logToInsert.FROM = senderEmail;
            logToInsert.TO = to;
            if (profile != null)
            {
                logToInsert.CREATED_BY = profile.UserName;
                logToInsert.MODIFIED_BY = profile.UserName;
                logToInsert.PRACTICE_CODE = profile.PracticeCode;
            }
            logToInsert.CREATED_DATE = GetCurrentDate();
            logToInsert.MODIFIED_DATE = GetCurrentDate();
            logToInsert.DELETED = false;
            logToInsert.TYPE = "Email";
            if(profile !=null)
            {
                _emailfaxlogRepository.Insert(logToInsert);
                _emailfaxlogRepository.Save();
            }
              
            
        }

        public static bool IsTestUser(string firstName, string lastName)
        {
            if (firstName.Contains("mtbc") || lastName.Contains("mtbc"))
            {
                return true;
            }

            if ((firstName.Contains("demo") && lastName.Contains("test")) || (firstName.Contains("test") && lastName.Contains("demo")) || (firstName.Contains("test") && lastName.Contains("test")) || (firstName.Equals("demo") && lastName.Equals("demo")))
            {
                return true;
            }          

            return false;
        }

        public static void LogFaxData(string to, string status, UserProfile profile,  string ex = null, long? work_id = null, List<string> attachments = null, string senderFax = "1234567890")
        {
            DbContextCommon _DbContextSP_New = new DbContextCommon();
            GenericRepository<EmailFaxLog> _emailfaxlogRepository = new GenericRepository<EmailFaxLog>(_DbContextSP_New);

            EmailFaxLog logToInsert = new EmailFaxLog();
            logToInsert.FOX_EMAIL_FAX_LOG_ID = getMaximumId("FOX_EMAIL_FAX_LOG_ID");
            if (attachments != null && attachments.Count > 0)
            {
                logToInsert.ATTACHMENT_PATH = String.Join(" ", attachments.ToArray());
            }

            logToInsert.CC = string.Empty;
            
            
            logToInsert.BCC = string.Empty;
            
            logToInsert.STATUS = status;
            if (!string.IsNullOrEmpty(ex))
            {
                logToInsert.EXCEPTION_SHORT_MSG = ex;
                logToInsert.EXCEPTION_TRACE = string.Empty;
            }

            logToInsert.WORK_ID = work_id;
            if (logToInsert.WORK_ID != null)
            {
                var _workIdRes = new SqlParameter("WORK_ID", SqlDbType.BigInt) { Value = logToInsert.WORK_ID };
                var _practice_Code = new SqlParameter("PRACTICE_CODE", SqlDbType.BigInt) { Value = profile.PracticeCode };
                var work_order = SpRepository<WORK_ID_MODEL>.GetSingleObjectWithStoreProcedure(@"exec [FOX_TBL_GET_WORK_ID] @WORK_ID,@PRACTICE_CODE", _workIdRes, _practice_Code);
                if (work_order != null && work_order.WORK_STATUS != null)
                {
                    logToInsert.WORK_STATUS = work_order.WORK_STATUS;
                }
            }
            logToInsert.FROM = senderFax;
            logToInsert.TO = to;
            if (profile != null)
            {
                logToInsert.CREATED_BY = profile.UserName;
                logToInsert.MODIFIED_BY = profile.UserName;
                logToInsert.PRACTICE_CODE = profile.PracticeCode;
            }
            logToInsert.CREATED_DATE = GetCurrentDate();
            logToInsert.MODIFIED_DATE = GetCurrentDate();
            logToInsert.DELETED = false;
            logToInsert.TYPE = "FAX";
            if (profile != null)
            {
                _emailfaxlogRepository.Insert(logToInsert);
                _emailfaxlogRepository.Save();
            }


        }
        public static void CustomExceptionLog(Exception context)
        {

            //var excpParam = JsonConvert.SerializeObject(context.ActionContext.ActionArguments.Values);
            //var uri = context.ActionContext.Request.RequestUri.OriginalString;
            //var excpMsg = context.Exception.Message;
            //var excpStackTrace = context.Exception.StackTrace;
            //var excpInnerMessage = ((context.Exception.InnerException != null && context.Exception.InnerException.Message != null) ? (context.Exception.InnerException.Message.ToLower().Contains("inner exception") ? context.Exception.InnerException.InnerException.Message : context.Exception.InnerException.Message) : "NULL");


            //Log Critical errors
            //string directoryOther = System.Web.HttpContext.Current.Server.MapPath(AppConfiguration.ErrorLogPath + "\\FoxCriticalErrors");
            try
            {
                string directoryOther = System.Web.HttpContext.Current.Server.MapPath("\\FoxCriticalErrorsCustom");
            if (!Directory.Exists(directoryOther))
            {
                Directory.CreateDirectory(directoryOther);
            }
            string filePathOther = directoryOther + "\\errors_" + DateTime.Now.Date.ToString("MM-dd-yyyy") + ".txt";
            
                using (StreamWriter writer = new StreamWriter(filePathOther, true))
                {
                    writer.WriteLine("Message: " + context.Message + Environment.NewLine + Environment.NewLine + "URI:  " + context.ToString() + Environment.NewLine + Environment.NewLine + "Request parameters: " + Environment.NewLine + Environment.NewLine + "StackTrace: " + context.StackTrace + Environment.NewLine + Environment.NewLine +
                       "///------------------Inner Exception------------------///" + Environment.NewLine + context.InnerException + "" + Environment.NewLine +
                       "Date: " + DateTime.Now.ToString() + Environment.NewLine + Environment.NewLine + "-------------------------------------------------------||||||||||||---End Current Exception---||||||||||||||||-------------------------------------------------------" + Environment.NewLine);
                }
            }
            catch (Exception ex)
            {
                Helper.SendEmailOnException(ex.Message, ex.ToString(), "Exception occurred in Custom Exception Filter");
            }
        }

        public static void SendEmailOnException(string exceptionMsg = "", string exceptionDetails = "", string subject = "")
        {
            //bool IsMailSent = false;
            string from = "noreply@carecloud.com";
            //Live
            //QA
            string to = "abdulsattar@carecloud.com";
            //string subject = "Exception occurred in Exception Filter";
            List<string> cc = new List<string>();
            cc.Add("muhammadarslan3@carecloud.com");
            cc.Add("adnanshah3@carecloud.com");
            cc.Add("aftabkhan@carecloud.com");

            //string ccvalues = ConfigurationManager.AppSettings["CCListException"];
            //if (!string.IsNullOrWhiteSpace(ccvalues))
            //{
            //    cc = ccvalues.Split(',').ToList();
            //}

            var body = "";
            body += "<body>";
            //add exception Message
            body += "<h3 style='color: #1960a7;margin: 0px;'> Exception Message:" + "</h3><br />";
            body += "<p style='color: #34495e;margin: 0px;'>" + exceptionMsg + "</p><br />";

            //add exception Details
            body += "<h3 style='color: #1960a7;margin: 0px;'> Exception Details: " + "</h3><br />";
            body += "<p style='color: #34495e;margin: 0px;'>" + exceptionDetails + "</p><br />";

            ////add original mail info
            //body += "<h3 style='color: #1960a7;margin: 0px;'> To: </h3><p style='color: #34495e;margin: 0px;'>" + OrgMailSentTo + "</p><br />";
            //body += "<h3 style='color: #1960a7;margin: 0px;'> Subject: </h3><p style='color: #34495e;margin: 0px;'>" + OrgMailSubject + "</p><br />";
            //body += "<h3 style='color: #1960a7;margin: 0px;'> Date: </h3><p style='color: #34495e;margin: 0px;'>" + OrgMailDate + "</p><br />";

            body += "</body>";
            try
            {
                using (SmtpClient smtp = new SmtpClient())
                {
                    using (MailMessage mail = new MailMessage())
                    {
                        mail.From = new MailAddress(from);
                        mail.To.Add(new MailAddress(to));
                        mail.Subject = subject;
                        mail.Body = body;
                        mail.IsBodyHtml = true;
                        mail.SubjectEncoding = Encoding.UTF8;
                        if (cc != null && cc.Count > 0)
                        {
                            foreach (var item in cc) { mail.CC.Add(item); }
                        }
                        smtp.Credentials = new System.Net.NetworkCredential(WebConfigurationManager.AppSettings["NoReplyUserName"], WebConfigurationManager.AppSettings["NoReplyPassword"]);
                        smtp.Send(mail);
                    }
                }
            }
            catch (Exception ex)
            {
                 throw ex;
            }
        }

        public static void TokenTaskCancellationExceptionLog(string msg)
        {
           try
            {
                string directoryOther = System.Web.HttpContext.Current.Server.MapPath("\\FoxCriticalTokenExceptionLog");
            if (!Directory.Exists(directoryOther))
            {
                Directory.CreateDirectory(directoryOther);
            }
            string filePathOther = directoryOther + "\\errors_" + DateTime.Now.Date.ToString("MM-dd-yyyy") + ".txt";
            
                using (StreamWriter writer = new StreamWriter(filePathOther, true))
                {
                    writer.WriteLine(DateTime.Now.ToString() + "   Message: " + msg  + Environment.NewLine);
                }
            }
            catch (Exception ex)
            {
                Helper.SendEmailOnException(ex.Message, ex.ToString(), "Exception occurred in Token Exception Filter");
            }
        }
    }
}
