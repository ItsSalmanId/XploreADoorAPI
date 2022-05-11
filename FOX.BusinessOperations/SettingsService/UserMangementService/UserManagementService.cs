using FOX.BusinessOperations.CommonService;
using FOX.BusinessOperations.CommonServices;
using FOX.BusinessOperations.Security;
using FOX.DataModels.Context;
using FOX.DataModels.GenericRepository;
using FOX.DataModels.Models.CommonModel;
using FOX.DataModels.Models.Security;
using FOX.DataModels.Models.Settings.RoleAndRights;
using FOX.DataModels.Models.Settings.User;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using FOX.DataModels.Models.ExternalUserModel;
using FOX.DataModels.Models.CasesModel;
using FOX.ExternalServices;
using FOX.DataModels.Models.IndexInfo;
using FOX.DataModels.Models.Settings.ReferralSource;
using FOX.DataModels.Models.Settings.Practice;
using FOX.DataModels.Models.SenderName;
using System.Security.Cryptography;
using FOX.DataModels.Models.StatesModel;
using FOX.DataModels.Models.LDAPUser;
using System.Data.Entity;
using System.Globalization;
using System.Threading;
using System.Text;
using System.Configuration;
using FOX.DataModels.Models.Settings.ClinicianSetup;
using FOX.DataModels.Models.SenderType;

namespace FOX.BusinessOperations.SettingsService.UserMangementService
{
    public class UserManagementService : IUserManagementService
    {
        private readonly DbContextSecurity security = new DbContextSecurity();
        private readonly DbContextSettings _settings = new DbContextSettings();
        private readonly DbContextSettings _dbContextSettings = new DbContextSettings();
        private readonly GenericRepository<User> _UserRepository;
        private readonly GenericRepository<FOX_TBL_PRACTICE_ROLE_RIGHTS> _RoleRightRepository;
        private readonly GenericRepository<FOX_TBL_RIGHTS_OF_ROLE> _RightsOfRoleRepository;
        private readonly GenericRepository<RoleToAdd> _RoleRepository;
        private readonly GenericRepository<ReferralRegion> _ReferralRegionRepository;
        private readonly GenericRepository<RegionCoverLetter> _RegionCoverLetterRepository;
        private readonly DbContextCommon _DbContextCommon = new DbContextCommon();
        private readonly GenericRepository<FOX_TBL_SENDER_NAME> _FOX_TBL_SENDER_NAME;
        private readonly GenericRepository<FOX_TBL_SENDER_TYPE> _FOX_TBL_SENDER_TYPE;
        private readonly GenericRepository<PasswordHistory> _passwordHistoryRepository;
        private readonly GenericRepository<FOX_TBL_IDENTIFIER> _fox_tbl_identifier;
        private readonly GenericRepository<Speciality> _speciality;
        private readonly GenericRepository<PracticeOrganization> _fox_tbl_practice_organization;
        private readonly DbContextCases _DbContextCases = new DbContextCases();
        private readonly GenericRepository<ReferralSource> _ReferralSourceRepository;
        private readonly GenericRepository<REGION_ZIPCODE_DATA> _referralRegionZipCodeData;
        private readonly GenericRepository<REFERRAL_REGION_COUNTY> _referralRegionCounty;
        private readonly GenericRepository<ActiveDirectoryRole> _ADRole;
        private readonly GenericRepository<Referral_Physicians> _referral_physiciansRepository;
        private readonly GenericRepository<FOX_TBL_ZIP_STATE_COUNTY> _zipCityStateCountyRepository;
        private readonly GenericRepository<FOX_TBL_USER_RIGHTS> _rightsRepository;
        private readonly GenericRepository<FOX_TBL_USER_RIGHTS_TYPE> _rightsTypeRepository;
        private readonly GenericRepository<FOX_TBL_APP_USER_ADDITIONAL_INFO> _userAdditionalInfoRepository;
        private readonly GenericRepository<Valid_Login_Attempts> _validLoginAtttempts;
        private readonly GenericRepository<FOX_TBL_DASHBOARD_ACCESS> _dashBoardAccessRepository;
        private readonly GenericRepository<WS_TBL_FOX_Login_LOGS> _loginLogsRepository;
        private readonly GenericRepository<FoxProviderClass> _FoxProviderClassRepository;
        private readonly GenericRepository<ActiveIndexer> _ActiveIndexerRepository;
        private readonly GenericRepository<ActiveIndexerLogs> _ActiveIndexerLogsRepository;
        private readonly GenericRepository<ActiveIndexerHistory> _ActiveIndexerHistoryRepository;
        public UserManagementService()
        {
            _UserRepository = new GenericRepository<User>(security);
            _RoleRightRepository = new GenericRepository<FOX_TBL_PRACTICE_ROLE_RIGHTS>(security);
            _RoleRepository = new GenericRepository<RoleToAdd>(security);
            _RightsOfRoleRepository = new GenericRepository<FOX_TBL_RIGHTS_OF_ROLE>(security);
            _ReferralRegionRepository = new GenericRepository<ReferralRegion>(security);
            _RegionCoverLetterRepository = new GenericRepository<RegionCoverLetter>(security);
            _speciality = new GenericRepository<Speciality>(_DbContextCases);
            _FOX_TBL_SENDER_NAME = new GenericRepository<FOX_TBL_SENDER_NAME>(_DbContextCommon);
            _FOX_TBL_SENDER_TYPE = new GenericRepository<FOX_TBL_SENDER_TYPE>(_DbContextCommon);
            _passwordHistoryRepository = new GenericRepository<PasswordHistory>(security);
            _fox_tbl_identifier = new GenericRepository<FOX_TBL_IDENTIFIER>(_DbContextCases);
            _fox_tbl_practice_organization = new GenericRepository<PracticeOrganization>(_DbContextCommon);
            _ReferralSourceRepository = new GenericRepository<ReferralSource>(security);
            _referralRegionZipCodeData = new GenericRepository<REGION_ZIPCODE_DATA>(security);
            _referralRegionCounty = new GenericRepository<REFERRAL_REGION_COUNTY>(security);
            _ADRole = new GenericRepository<ActiveDirectoryRole>(_DbContextCommon);
            _referral_physiciansRepository = new GenericRepository<Referral_Physicians>(_settings);
            _zipCityStateCountyRepository = new GenericRepository<FOX_TBL_ZIP_STATE_COUNTY>(_settings);
            _rightsRepository = new GenericRepository<FOX_TBL_USER_RIGHTS>(security);
            _rightsTypeRepository = new GenericRepository<FOX_TBL_USER_RIGHTS_TYPE>(security);
            _userAdditionalInfoRepository = new GenericRepository<FOX_TBL_APP_USER_ADDITIONAL_INFO>(security);
            _validLoginAtttempts = new GenericRepository<Valid_Login_Attempts>(_DbContextCommon);
            _dashBoardAccessRepository = new GenericRepository<FOX_TBL_DASHBOARD_ACCESS>(_settings);
            _loginLogsRepository = new GenericRepository<WS_TBL_FOX_Login_LOGS>(security);
            _FoxProviderClassRepository = new GenericRepository<FoxProviderClass>(_dbContextSettings);
            _ActiveIndexerRepository = new GenericRepository<ActiveIndexer>(security);
            _ActiveIndexerLogsRepository = new GenericRepository<ActiveIndexerLogs>(security);
            _ActiveIndexerHistoryRepository = new GenericRepository<ActiveIndexerHistory>(security);
        }
        public bool CreateUser(User user, UserProfile profile)
        {
            var usr = _UserRepository.GetByID(user.USER_ID);
            if (usr == null && user != null && !string.IsNullOrWhiteSpace(user?.EMAIL ?? ""))
                usr = _UserRepository.Get(t => t.EMAIL != null && t.EMAIL.ToLower().Equals(user.EMAIL.ToLower()));

            if (usr != null)
            {
                usr.ACTIVATION_CODE = user.ACTIVATION_CODE;
                usr.ADDRESS_1 = user.ADDRESS_1;
                usr.ADDRESS_2 = user.ADDRESS_2;
                usr.CITY = user.CITY;
                usr.COMMENTS = user.COMMENTS;
                usr.DATE_OF_BIRTH = user.DATE_OF_BIRTH;
                usr.DELETED = user.DELETED;
                usr.DESIGNATION = user.DESIGNATION;
                usr.EMAIL = user.EMAIL;
                usr.FAILED_PASSWORD_ATTEMPT_COUNT = user.FAILED_PASSWORD_ATTEMPT_COUNT;
                usr.FIRST_NAME = user.FIRST_NAME;
                if (usr.IS_ACTIVE && !user.IS_ACTIVE)
                {
                    usr.TERMINATION_DATE = Helper.GetCurrentDate();
                }
                else
                {
                    usr.TERMINATION_DATE = null;
                }
                usr.IS_ACTIVE = user.IS_ACTIVE;
                usr.IS_ADMIN = user.IS_ADMIN;
                //  usr.IS_LOCKED_OUT = user.IS_LOCKED_OUT;
                //   usr.LAST_LOGIN_DATE = user.LAST_LOGIN_DATE;
                usr.LAST_NAME = user.LAST_NAME?.Trim();
                //    usr.LOCKEDBY = user.LOCKEDBY;
                usr.MIDDLE_NAME = user.MIDDLE_NAME?.Trim();
                //  usr.PASSWORD = Encrypt.getEncryptedCode(user.PASSWORD); 
                // usr.PASSWORD_CHANGED_DATE = user.PASSWORD_CHANGED_DATE;
                // usr.PASS_RESET_CODE = user.PASS_RESET_CODE;
                usr.PRACTICE_CODE = profile.PracticeCode;
                usr.RESET_PASS = user.RESET_PASS;
                usr.ROLE_ID = user.ROLE_ID;
                usr.SECURITY_QUESTION = user.SECURITY_QUESTION;
                usr.SECURITY_QUESTION_ANSWER = user.SECURITY_QUESTION_ANSWER;
                usr.STATE = user.STATE;
                usr.STATUS = user.STATUS;
                usr.USER_DISPLAY_NAME = user.USER_DISPLAY_NAME;
                usr.USER_ID = user.USER_ID;
                usr.USER_NAME = user.USER_NAME;
                usr.ZIP = user.ZIP;
                usr.MODIFIED_BY = profile.UserName;
                usr.MODIFIED_DATE = Helper.GetCurrentDate();
                usr.MANAGER_ID = user.MANAGER_ID;
                usr.USER_TYPE = user.USER_TYPE;
                //usr.PRACTICE_NAME = user.PRACTICE_NAME;
                usr.NPI = user.NPI;
                usr.MOBILE_PHONE = user.MOBILE_PHONE;
                usr.PHONE_NO = user.PHONE_NO;
                usr.FAX = user.FAX;
                usr.GENDER = user.GENDER;
                usr.LANGUAGE = user.LANGUAGE;
                usr.TIME_ZONE = user.TIME_ZONE;
                usr.SIGNATURE_PATH = usr.SIGNATURE_PATH;
                usr.FOX_TBL_SENDER_TYPE_ID = user.FOX_TBL_SENDER_TYPE_ID;
                usr.ACO = user.ACO;
                usr.SNF = user.SNF;
                usr.HHH = user.HHH;
                usr.PRACTICE_ORGANIZATION_ID = user.PRACTICE_ORGANIZATION_ID;
                usr.HOSPITAL = user.HOSPITAL;
                usr.SPECIALITY = user.SPECIALITY;
                usr.COMMENTS = user.COMMENTS;
                usr.ACO_TEXT = user.ACO_TEXT;
                usr.SPECIALITY_TEXT = user.SPECIALITY_TEXT;
                usr.HOSPITAL_TEXT = user.HOSPITAL_TEXT;
                usr.HHH_TEXT = user.HHH_TEXT;
                usr.PRACTICE_ORGANIZATION_TEXT = user.PRACTICE_ORGANIZATION_TEXT;
                usr.SNF_TEXT = user.SNF_TEXT;
                //usr.SENDER_TYPE = user.SENDER_TYPE;
                usr.IS_APPROVED = user.IS_APPROVED;
                try
                {
                    _UserRepository.Update(usr);
                    _UserRepository.Save();
                    #region Send Email In Case of Approved External User
                    //if (user.hasToSendEmail)
                    //{
                    //    #region email to user on approva;

                    //    string body = string.Empty;
                    //    string templatePathOfAdminEmail = HttpContext.Current.Server.MapPath("~/HtmlTemplates/external_user_email_template_on_request_approved.html");
                    //    if (File.Exists(templatePathOfAdminEmail))
                    //    {
                    //        body = File.ReadAllText(templatePathOfAdminEmail);
                    //        body = body.Replace("[[FIRST_NAME]]", user.FIRST_NAME);
                    //        body = body.Replace("[[LAST_NAME]]", user.LAST_NAME);
                    //    }
                    //    string subject = "Signup approved for FOX Rehab portal";
                    //    string sendTo = user.EMAIL;
                    //    //string sendTo = "usmanfarooq@mtbc.com";
                    //    //string sendTo = "asimshah4@mtbc.com";
                    //    List<string> _bccList = new List<string>();
                    //    //Helper.SendEmail(sendTo, subject, body, null, _bccList, "noreply@mtbc.com");
                    //    Helper.SendEmail(sendTo, subject, body, null, profile, _bccList);

                    //    #endregion
                    //}
                    #endregion
                    #region sms to user on approval
                    string recipient = "";
                    //string smsBody = @"Your signup request for Fox Rehab portal has been approved. You may now login to your account and start sending a patient referral right away! Download on the App Store: https://itunes.apple.com/us/app/mtbc-fox/id1384823410?mt=8";
                    //string smsBody = @"Dear " + user.LAST_NAME + ", " + user.FIRST_NAME + "\nYour request for access to Fox Rehabilitation's online referral portal has been approved. Also, if you agreed to receiving Electronic Plans of Care, please see your email for final security step.\nFox Rehabilitation,\nClient Services Team";
                    string smsBody = @"Dear " + user.LAST_NAME + ", " + user.FIRST_NAME + "\nYour request for access to Fox Rehabilitation online referral portal has been approved. \nFox Rehabilitation,\nClient Services Team";
                    if (user.MOBILE_PHONE != null)
                    {
                        recipient = user.MOBILE_PHONE;
                        SmsService.NJSmsService(recipient, smsBody);

                    }
                    #endregion
                }
                catch (Exception)
                {
                    return false;
                }
            }
            else
            {
                usr = new User();
                usr = user;
                usr.USER_ID = Helper.getMaximumId("USER_ID");
                usr.USER_NAME = usr.LAST_NAME + "_" + usr.USER_ID.ToString();
                usr.PASSWORD = Encrypt.getEncryptedCode(usr.PASSWORD);
                usr.PRACTICE_CODE = profile.PracticeCode;
                usr.CREATED_BY = profile.UserName;
                usr.CREATED_DATE = Helper.GetCurrentDate();
                usr.MODIFIED_BY = profile.UserName;
                usr.MODIFIED_DATE = Helper.GetCurrentDate();
                usr.IS_APPROVED = true;
                try
                {
                    _UserRepository.Insert(usr);
                    _UserRepository.Save();
                    PasswordHistory ph = new PasswordHistory()
                    {
                        PASSWORD_ID = Helper.getMaximumId("PASSWORD_ID"),
                        DELETED = false,
                        USER_ID = usr.USER_ID,
                        PASSWORD = usr.PASSWORD,
                        CREATED_BY = profile.UserName,
                        CREATED_DATE = Helper.GetCurrentDate(),
                        MODIFIED_BY = profile.UserName,
                        MODIFIED_DATE = Helper.GetCurrentDate(),
                        PRACTICE_CODE = usr.PRACTICE_CODE
                    };
                    _passwordHistoryRepository.Insert(ph);
                    _passwordHistoryRepository.Save();

                    var senderName = _FOX_TBL_SENDER_NAME.GetFirst(t => t.SENDER_NAME_CODE.Equals(usr.USER_NAME));
                    if (senderName == null)
                    {
                        senderName = new FOX_TBL_SENDER_NAME();
                        senderName.FOX_TBL_SENDER_NAME_ID = Helper.getMaximumId("FOX_TBL_SENDER_NAME_ID");
                        senderName.FOX_TBL_SENDER_TYPE_ID = null;
                        senderName.PRACTICE_CODE = profile.PracticeCode;
                        senderName.SENDER_NAME_CODE = usr.USER_NAME;
                        senderName.SENDER_NAME_DESCRIPTION = usr.USER_DISPLAY_NAME;
                        senderName.CREATED_BY = senderName.MODIFIED_BY = profile.UserName;
                        senderName.CREATED_DATE = senderName.MODIFIED_DATE = Helper.GetCurrentDate();
                        senderName.DELETED = false;

                        _FOX_TBL_SENDER_NAME.Insert(senderName);
                        _FOX_TBL_SENDER_NAME.Save();
                    }
                }
                catch (Exception ex)
                {
                    return false;
                }
            }

            if (usr.USER_TYPE.ToLower().Contains("external") && (usr.IS_APPROVED ?? false) && (usr.ROLE_ID.HasValue && usr.ROLE_ID == 108 || usr.ROLE_ID.HasValue && usr.ROLE_ID == 109))
            {
                AddUpdateReferralSourceInfo(usr.USER_NAME, profile);

            }

            return true;
        }
        public bool SetAutoLockTimeSetup(int time, UserProfile profile)
        {
            var user = new User();
            if (!string.IsNullOrEmpty(profile.UserName))
            {
                user = _UserRepository.Get(x => !string.IsNullOrEmpty(x.USER_NAME) && x.USER_NAME.ToLower().Equals(profile.UserName.ToLower()));
                try
                {
                    user.AUTO_LOCK_TIME_SPAN = time;
                    _UserRepository.Update(user);
                    _UserRepository.Save();
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
        public bool DiscardAndDeleteExternalUser(User user, UserProfile profile)
        {
            var usr = _UserRepository.GetByID(user.USER_ID);
            if (usr != null)
            {
                usr.DELETED = true;
                usr.IS_ACTIVE = false;
                usr.IS_APPROVED = false;
                try
                {
                    _UserRepository.Update(usr);
                    _UserRepository.Save();
                    #region email to user on declined
                    string body = string.Empty;
                    string templatePath = HttpContext.Current.Server.MapPath("~/HtmlTemplates/external_user_signup_email_to_user_on_request_declined.html");
                    if (File.Exists(templatePath))
                    {
                        body = File.ReadAllText(templatePath);
                        body = body.Replace("[[FIRST_NAME]]", user.FIRST_NAME);
                        body = body.Replace("[[LAST_NAME]]", user.LAST_NAME);
                    }
                    string subject = "Signup declined for FOX Rehab portal";
                    string sendTo = user.EMAIL;
                    List<string> _bccList = new List<string>();
                    Helper.SendEmail(sendTo, subject, body, null, profile, _bccList);

                    #endregion
                    #region sms to user on approval
                    string recipient = "";
                    string smsBody = @"Your signup request for Fox Rehab portal has been declined. You can contact us at 877-407-3422 for further assistance.\n FOX Rehab team.";
                    if (user.MOBILE_PHONE != null)
                    {
                        recipient = user.MOBILE_PHONE;
                        SmsService.NJSmsService(recipient, smsBody);

                    }
                    #endregion
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }
            else
            {
                return true;
            }
        }
        public List<User> GetUsers(UserRequest request, UserProfile profile)
        {
            if (request != null)
            {
                //   return _UserRepository.GetMany(x => x.PRACTICE_CODE == profile.PracticeCode);
                var parmPracticeCode = new SqlParameter("PRACTICE_CODE", SqlDbType.BigInt) { Value = profile.PracticeCode };
                var parmSearchText = Helper.getDBNullOrValue("SEARCH_TEXT", request.SearchText);
                //var CurrentPage = new SqlParameter { ParameterName = "CURRENT_PAGE", SqlDbType = SqlDbType.Int, Value = request.CurrentPage };
                //var RecordPerPage = new SqlParameter { ParameterName = "RECORD_PER_PAGE", SqlDbType = SqlDbType.Int, Value = request.RecordPerPage };
                var CurrentPage = new SqlParameter { ParameterName = "CURRENT_PAGE", SqlDbType = SqlDbType.Int, Value = request.CurrentPage };
                var RecordPerPage = new SqlParameter { ParameterName = "RECORD_PER_PAGE", SqlDbType = SqlDbType.Int, Value = request.RecordPerPage };
                var FilterIs_Approved = Helper.getDBNullOrValue("@FILTER_IS_APPROVED", request.FilterIs_Approved.ToString());
                try
                {
                    var users = SpRepository<User>.GetListWithStoreProcedure(@"exec [FOX_PROC_GET_PRACTICE_USERS] @PRACTICE_CODE,@SEARCH_TEXT, @RECORD_PER_PAGE, @CURRENT_PAGE,@FILTER_IS_APPROVED", parmPracticeCode, parmSearchText, RecordPerPage, CurrentPage, FilterIs_Approved);
                    return users;

                    //var users = SpRepository<User>.GetListWithStoreProcedure(@"exec [FOX_PROC_GET_PRACTICE_USERS_90365] @PRACTICE_CODE,@SEARCH_TEXT, @RECORD_PER_PAGE, @CURRENT_PAGE", parmPracticeCode, parmSearchText, RecordPerPage, CurrentPage);
                    //8 return users;
                }
                catch (Exception ex)
                {
                    return new List<User>();
                }
            }
            else
            {
                return new List<User>();
            }
        }
        //export to excel 08-07-2021
        public string ExportToExcelUsersReport(UserRequest request, UserProfile profile)
        {
            try
            {
                string fileName = "Users_Report_List";
                string exportPath = "";
                string path = string.Empty;
                bool exported;
                request.CurrentPage = 1;
                request.RecordPerPage = 0;
                var CalledFrom = "Users_Reports";
                string virtualPath = @"/" + profile.PracticeDocumentDirectory + "/" + "Fox/ExportedFiles/";
                exportPath = HttpContext.Current.Server.MapPath("~" + virtualPath);
                fileName = DocumentHelper.GenerateSignatureFileName(fileName) + ".xlsx";
                if (!Directory.Exists(exportPath))
                {
                    Directory.CreateDirectory(exportPath);
                }
                List<User> result = new List<User>();
                var pathtowriteFile = exportPath + "\\" + fileName;
                result = GetUsers(request, profile);
                for (int i = 0; i < result.Count(); i++)
                {
                    result[i].ROW = i + 1;

                }
                exported = ExportToExcel.CreateExcelDocument<User>(result, pathtowriteFile, CalledFrom.Replace(' ', '_'));
                return virtualPath + fileName;
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }

        //close



        public User GetSingleUser(string username, UserProfile profile)
        {
            try
            {
                string DB_PASSWORD = "";
                var user = _UserRepository.GetSingle(x => x.USER_NAME.Equals(username));
                //Source Email Validation Vulnerability
                user.SECURITY_QUESTION = null;
                user.SECURITY_QUESTION_ANSWER = null;
                var role = _RoleRepository.GetFirst(x => x.ROLE_ID.Equals(profile.RoleId));
                var userRole = _RoleRepository.GetFirst(x => x.ROLE_ID.Equals(user.ROLE_ID ?? 0) && !x.DELETED);
                if(userRole !=null && !string.IsNullOrWhiteSpace(userRole.ROLE_NAME))
                {
                    user.ROLE_NAME = userRole.ROLE_NAME.Trim();
                }
                Decripted_Password_Info respnse2 = new Decripted_Password_Info();

                if (!string.IsNullOrWhiteSpace(role.ROLE_NAME) && !string.IsNullOrWhiteSpace(user.EMAIL) && !string.IsNullOrWhiteSpace(profile.UserEmailAddress) && role.ROLE_NAME.ToLower().Contains("administrator") && user.EMAIL != profile.UserEmailAddress)
                {
                    var userPassword = SpRepository<WS_TBL_FOX_Login_LOGS>.GetListWithStoreProcedure(@"exec FOX_PROC_GET_PASSWORD_FROM_LOGS @EMAIL", new SqlParameter { ParameterName = "EMAIL", SqlDbType = SqlDbType.VarChar, Value = user.EMAIL });
                    var adminPassword = SpRepository<WS_TBL_FOX_Login_LOGS>.GetListWithStoreProcedure(@"exec FOX_PROC_GET_PASSWORD_FROM_LOGS @EMAIL", new SqlParameter { ParameterName = "EMAIL", SqlDbType = SqlDbType.VarChar, Value = profile.UserEmailAddress });
                    string encryptedPassword = "";
                    string encryptedAdminPassword = "";

                    foreach (var item in userPassword)
                    {
                        encryptedPassword = item.Password;
                    }
                    foreach(var item in adminPassword)
                    {
                        encryptedAdminPassword = item.Password;

                    }
                    if (!string.IsNullOrWhiteSpace(encryptedPassword) && !string.IsNullOrWhiteSpace(encryptedAdminPassword))
                    {
                        user.DB_PASSWORD = Encrypt.DecryptPassword(encryptedPassword);
                        user.ADMIN_PASSWORD = Encrypt.DecryptPassword(encryptedAdminPassword);

                        user.HIDE_EYE_ICON = false;
                    }
                    else
                    {
                        user.DB_PASSWORD = user.SHOW_TO_USER_PASSWORD = "";
                        user.HIDE_EYE_ICON = true;
                    }
                }
                #region Old Logic for Fox Rehab Users
                //else if(!string.IsNullOrWhiteSpace(role.ROLE_NAME) && role.ROLE_NAME.ToLower().Contains("administrator") && user.EMAIL.ToLower().Contains("@foxrehab.org") && user.EMAIL != profile.UserEmailAddress)
                //{
                //    user.HIDE_EYE_ICON = true;
                //}
                #endregion
                else if (!string.IsNullOrWhiteSpace(user.EMAIL) && !string.IsNullOrWhiteSpace(profile.UserEmailAddress) && user.EMAIL == profile.UserEmailAddress)
                {
                    var userPassword = SpRepository<WS_TBL_FOX_Login_LOGS>.GetListWithStoreProcedure(@"exec FOX_PROC_GET_PASSWORD_FROM_LOGS @EMAIL", new SqlParameter { ParameterName = "EMAIL", SqlDbType = SqlDbType.VarChar, Value = profile.UserEmailAddress });
                    string encryptedPassword = "";

                    foreach (var item in userPassword)
                    {
                        encryptedPassword = item.Password;
                    }
                    if (!string.IsNullOrWhiteSpace(encryptedPassword))
                    {
                        user.DB_PASSWORD = Encrypt.DecryptPassword(encryptedPassword);
                        user.PROFILE = true;
                    }
                    else
                    {
                        user.DB_PASSWORD = user.SHOW_TO_USER_PASSWORD = "";
                        user.HIDE_EYE_ICON = true;
                    }
                }
                else
                {
                    user.DB_PASSWORD = user.SHOW_TO_USER_PASSWORD = "";
                    user.HIDE_EYE_ICON = true;
                }


                if (user != null)
                {
                    if (user.REFERRAL_REGION_ID != null)
                    {
                        var reg_name = _ReferralRegionRepository.GetFirst(r => r.REFERRAL_REGION_ID == user.REFERRAL_REGION_ID && !r.DELETED);
                        if (reg_name != null && !string.IsNullOrEmpty(reg_name.REFERRAL_REGION_NAME))
                            user.REGION_NAME = reg_name.REFERRAL_REGION_NAME;
                    }
                    var aco = _fox_tbl_identifier.GetByID(user.ACO);
                    var snf = _fox_tbl_identifier.GetByID(user.SNF);
                    var hhh = _fox_tbl_identifier.GetByID(user.HHH);
                    var hospital = _fox_tbl_identifier.GetByID(user.HOSPITAL);
                    var speciality = _speciality.GetByID(user.SPECIALITY);
                    user.ACO_NAME = aco != null ? aco.NAME : "";
                    user.SNF_NAME = snf != null ? snf.NAME : "";
                    user.HHH_NAME = hhh != null ? hhh.NAME : "";
                    user.HOSPITAL_NAME = hospital != null ? hospital.NAME : "";
                    user.SPECIALITY_NAME = speciality != null ? speciality.NAME : "";
                    user.PRACTICE_NAME = _fox_tbl_practice_organization.Get(t => !t.DELETED && t.PRACTICE_CODE == AppConfiguration.GetPracticeCode && t.PRACTICE_ORGANIZATION_ID == user.PRACTICE_ORGANIZATION_ID)?.NAME ?? "";
                    /* user.Is_Electronic_POC = */
                    SqlDataAdapter result =   SpRepository<object>.getSpSqlDataAdapter("select Is_Electronic_POC from FOX_TBL_APP_USER_ADDITIONAL_INFO where deleted=0 and FOX_TBL_APPLICATION_USER_Id=" + user.USER_ID);

                    DataTable dt = new DataTable();
                    result.Fill(dt);
                    if(dt.Rows.Count>0)
                    {
                        bool Is_Electronic_POC = bool.Parse(dt.Rows[0][0].ToString());
                        user.Is_Electronic_POC = Is_Electronic_POC ;
                    }
                    else
                    {
                        user.Is_Electronic_POC = false;
                    }

                    user.Is_Blocked = false;
                    bool Is_AdUser = user.IS_AD_USER.HasValue ? user.IS_AD_USER.Value : false;
                    if (!Is_AdUser)
                    {
                        Valid_Login_Attempts invalidAttempts = _validLoginAtttempts.GetFirst(t => t.USER_NAME == user.USER_NAME || t.USER_NAME == user.EMAIL);
                        if (invalidAttempts != null && invalidAttempts.FAIL_ATTEMPT_COUNT >= AppConfiguration.InvalidAttemptsCountToBlockUser)
                        {
                            user.Is_Blocked = true;
                        }
                    }
                }
                return user;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public User_And_Regions_Data GetReferralReagions(string username, UserProfile profile)
        {
            try
            {

                User_And_Regions_Data response = new User_And_Regions_Data ();
                List<Decripted_Password_Info> list = new List<Decripted_Password_Info>();
                var user = _UserRepository.GetSingle(x => x.USER_NAME.Equals(username));
                string user_encryptedPassword = "";
                string admin_encryptedPassword = "";
                string decryptedPassword = "";
                string admin_decryptedPassword = "";
                if (user.ROLE_ID != null && user.ROLE_ID !=0)
                {
                    var role = _RoleRepository.GetFirst(x => x.ROLE_ID == user.ROLE_ID);
                    var PracticeCode = new SqlParameter { ParameterName = "PRACTICE_CODE", SqlDbType = SqlDbType.BigInt, Value = profile.PracticeCode };
                    var username1 = new SqlParameter { ParameterName = "USER_NAME", SqlDbType = SqlDbType.VarChar, Value = user.USER_NAME };
                    //var rt_userid= new SqlParameter { ParameterName = "RT_USER_ID", SqlDbType = SqlDbType.VarChar, Value = user.RT_USER_ID };
                    var role_name = new SqlParameter { ParameterName = "ROLE", SqlDbType = SqlDbType.VarChar, Value = role.ROLE_NAME };
                    var Email = new SqlParameter { ParameterName = "EMAIL", SqlDbType = SqlDbType.VarChar, Value = user.EMAIL };
                    var email = new SqlParameter { ParameterName = "EMAIL", SqlDbType = SqlDbType.VarChar, Value = user.EMAIL };
                    var admin_email = new SqlParameter { ParameterName = "EMAIL", SqlDbType = SqlDbType.VarChar, Value = profile.UserEmailAddress };
                    var ref_region = SpRepository<Clinician_And_Referral_Region_Data>.GetListWithStoreProcedure(@"exec FOX_PROC_GET_ASSOCIATED_REAGION_DATA @PRACTICE_CODE, @USER_NAME, @EMAIL, @ROLE", PracticeCode, username1, Email, role_name);
                    var login_log = SpRepository<Login_log_Data>.GetListWithStoreProcedure(@"exec FOX_PROC_GET_LOGIN_LOG  @EMAIL",  email);
                    var admin_log = SpRepository<Login_log_Data>.GetListWithStoreProcedure(@"exec FOX_PROC_GET_LOGIN_LOG  @EMAIL", admin_email);
                    foreach(var item1 in admin_log)
                    {
                        admin_encryptedPassword = item1.Password;
                        if (item1.CreatedBy.ToLower() == "fox web service team" && item1.AdLoginResponse.ToLower() == "user login successfully")
                        {
                            var decrypted = Encrypt.DecryptApp_password(admin_encryptedPassword);
                            admin_decryptedPassword = decrypted;
                        }
                        else if( item1.CreatedBy.ToLower() == "fox_portal" && item1.AdLoginResponse.ToLower() == "user login successfully")
                        {
                            var decrypted = Encrypt.DecryptPassword(admin_encryptedPassword);
                            admin_decryptedPassword = decrypted;
                        }
                    }
                    foreach (var item in login_log)
                    {
                        Decripted_Password_Info respnse2 = new Decripted_Password_Info();
                        user_encryptedPassword = item.Password;

                        if (item.CreatedBy.ToLower() == "fox web service team")
                        {
                            var decrypted = Encrypt.DecryptApp_password(user_encryptedPassword);
                            decryptedPassword = decrypted;
                        }
                        else
                        {
                            var decrypted = Encrypt.DecryptPassword(user_encryptedPassword);
                            decryptedPassword = decrypted;
                        }
                        respnse2.Decrypted_Passwords = decryptedPassword;
                        respnse2.Created_By = item.CreatedBy;
                        respnse2.Created_date = item.CreatedDate;
                        respnse2.Device_Info = item.DeviceInfo;
                        respnse2.AdLoginResponse = item.AdLoginResponse;
                        list.Add(respnse2);

                    }

                    response.Password_Info = list;
                    response.Reagions_info = ref_region;
                    response.LoginLog_Info = login_log;
                    response.Admin_Password = admin_decryptedPassword;
                }
                return response;
            }
            catch (Exception ex)
            {
                return null;
            }

        }

        //public List <Login_log_Data> GetLogInLogData (string username, UserProfile profile)
        //{ }
        public List<UsersForDropdown> GetUsersForSupervisorDD(long practiceCode)
        {
            return (from a in _UserRepository.GetMany(x => x.PRACTICE_CODE == practiceCode && x.ROLE_ID != 2) select new UsersForDropdown(a.FIRST_NAME, a.LAST_NAME, a.USER_ID, a.ROLE_NAME, a.ROLE_ID ?? 0)).ToList();
        }
        public List<right> GetRoles(long practiceCode)
        {
            var pc = new SqlParameter("PRACTICE_CODE", SqlDbType.BigInt) { Value = practiceCode };
            var roleAndList = SpRepository<RoleAndRights>.GetListWithStoreProcedure(@"exec FOX_PROC_GET_ROLE_RIGHTS @PRACTICE_CODE", pc);
            List<right> right = (from x in roleAndList select new right() { RIGHT_NAME = x.RIGHT_NAME, RIGHT_ID = x.RIGHT_ID, RIGHT_TYPE_NAME = x.RIGHT_TYPE_NAME, OrderId = x.OrderId }).OrderBy(e => e.OrderId ).GroupBy(p => p.RIGHT_ID).Select(g => g.First()).ToList();
            for (var i = 0; i < right.Count; i++)
            {
                var roles = (from x in roleAndList where x.RIGHT_ID == right[i].RIGHT_ID select new Role() { IS_CHECKED = x.IS_CHECKED, ROLE_ID = x.ROLE_ID, ROLE_NAME = x.ROLE_NAME, RIGHT_ID = x.RIGHT_ID, RIGHTS_OF_ROLE_ID = x.RIGHTS_OF_ROLE_ID }).OrderBy(t => t.ROLE_ID).ToList();
                //roles.Sort((x, y) => { return x.ROLE_ID < y.ROLE_ID ? 0 : 1; });
                right[i].Roles = roles;
            }
            return right;
        }
        public void SetRole(Role role, UserProfile profile)
        {
            bool IsDeleted = false;
            var usr = _RoleRightRepository.GetFirst(x => x.RIGHTS_OF_ROLE_ID == role.RIGHTS_OF_ROLE_ID && x.PRACTICE_CODE == profile.PracticeCode && x.DELETED == false);
            if (usr == null)
            {
                usr = _RoleRightRepository.GetFirst(x => x.RIGHTS_OF_ROLE_ID == role.RIGHTS_OF_ROLE_ID && x.PRACTICE_CODE == profile.PracticeCode && x.DELETED);
                IsDeleted = true;
            }

            if (usr != null)
            {
                FOX_TBL_PRACTICE_ROLE_RIGHTS RoleRightObj = new FOX_TBL_PRACTICE_ROLE_RIGHTS();
                var RightsData = security.Rights.Where(x => !x.DELETED).ToList();
                var RightsTypeData = security.RightsType.Where(x => !x.DELETED).ToList();
                var PatientManID = _rightsRepository.GetFirst(x => x.RIGHT_NAME == "PATIENT MAINTENANCE" && !x.DELETED).RIGHT_ID;
                var PatientManTypeID = _rightsTypeRepository.GetFirst(x => x.RIGHT_TYPE_NAME == "PATIENT MAINTENANCE" && !x.DELETED).RIGHT_TYPE_ID;
                var PatientTypeUsers = RightsData.Where(x => x.RIGHT_TYPE_ID == PatientManTypeID).ToList();
                if (PatientManID == role.RIGHT_ID && !role.IS_CHECKED)
                {
                    foreach (var i in PatientTypeUsers)
                    {
                        var rightsOfRoles = _RightsOfRoleRepository.GetFirst(x => x.ROLE_ID == role.ROLE_ID && x.RIGHT_ID == i.RIGHT_ID);
                        if (rightsOfRoles != null)
                        {
                            var ChangedUser = _RoleRightRepository.GetFirst(x => x.RIGHTS_OF_ROLE_ID == rightsOfRoles.RIGHTS_OF_ROLE_ID && x.CHECKED && x.PRACTICE_CODE == profile.PracticeCode && x.DELETED == false);
                            if (ChangedUser != null)
                            {
                                ChangedUser.CHECKED = false;
                                ChangedUser.MODIFIED_BY = profile.UserName;
                                ChangedUser.MODIFIED_DATE = Helper.GetCurrentDate();
                                _RoleRightRepository.Update(ChangedUser);
                            }
                        }
                    }
                }
                if (PatientManID != role.RIGHT_ID && role.IS_CHECKED)
                {
                    var RightTypeID = _rightsRepository.GetFirst(x => x.RIGHT_ID == role.RIGHT_ID && !x.DELETED);
                    if (RightTypeID != null)
                    {
                        if (RightTypeID.RIGHT_TYPE_ID == PatientManTypeID)
                        {
                            var rightsOfRoles = _RightsOfRoleRepository.GetFirst(x => x.ROLE_ID == role.ROLE_ID && x.RIGHT_ID == PatientManID);
                            if (rightsOfRoles != null)
                            {
                                var ChangedUser = _RoleRightRepository.GetFirst(x => x.RIGHTS_OF_ROLE_ID == rightsOfRoles.RIGHTS_OF_ROLE_ID && x.PRACTICE_CODE == profile.PracticeCode && !x.DELETED);
                                if (ChangedUser != null)
                                {
                                    ChangedUser.CHECKED = true;
                                    ChangedUser.MODIFIED_BY = profile.UserName;
                                    ChangedUser.MODIFIED_DATE = Helper.GetCurrentDate();
                                    _RoleRightRepository.Update(ChangedUser);
                                }
                            }
                        }
                    }
                }

                usr.MODIFIED_BY = profile.UserName;
                usr.MODIFIED_DATE = Helper.GetCurrentDate();
                usr.CHECKED = role.IS_CHECKED;
                if (IsDeleted)
                    usr.DELETED = false;
                _RoleRightRepository.Update(usr);
            }
            else
            {
                var user = new FOX_TBL_PRACTICE_ROLE_RIGHTS();
                user.RIGHTS_OF_ROLE_ID = role.RIGHTS_OF_ROLE_ID;
                user.CHECKED = role.IS_CHECKED;
                user.PRACTICE_CODE = profile.PracticeCode;
                user.CREATED_BY = profile.UserName;
                user.CREATED_DATE = Helper.GetCurrentDate();
                user.MODIFIED_BY = profile.UserName;
                user.MODIFIED_DATE = Helper.GetCurrentDate();
                _RoleRightRepository.Insert(user);
                _RoleRightRepository.Save();
                var PatientManID = _rightsRepository.GetFirst(x => x.RIGHT_NAME == "PATIENT MAINTENANCE" && !x.DELETED).RIGHT_ID;
                if (PatientManID != role.RIGHT_ID && role.IS_CHECKED)
                {


                    var rightsOfRoles = _RightsOfRoleRepository.GetFirst(x => x.ROLE_ID == role.ROLE_ID && x.RIGHT_ID == PatientManID);
                    if (rightsOfRoles != null)
                    {
                        var ChangedUser = _RoleRightRepository.GetFirst(x => x.RIGHTS_OF_ROLE_ID == rightsOfRoles.RIGHTS_OF_ROLE_ID && x.PRACTICE_CODE == profile.PracticeCode && !x.DELETED);
                        if (ChangedUser != null)
                        {
                            ChangedUser.CHECKED = true;
                            ChangedUser.MODIFIED_BY = profile.UserName;
                            ChangedUser.MODIFIED_DATE = Helper.GetCurrentDate();
                            _RoleRightRepository.Update(ChangedUser);
                        }
                        else
                        {
                            var users = new FOX_TBL_PRACTICE_ROLE_RIGHTS();
                            users.RIGHTS_OF_ROLE_ID = rightsOfRoles.RIGHTS_OF_ROLE_ID;
                            users.CHECKED = role.IS_CHECKED;
                            users.PRACTICE_CODE = profile.PracticeCode;
                            users.CREATED_BY = profile.UserName;
                            users.CREATED_DATE = Helper.GetCurrentDate();
                            users.MODIFIED_BY = profile.UserName;
                            users.MODIFIED_DATE = Helper.GetCurrentDate();
                            _RoleRightRepository.Insert(users);
                            _RoleRightRepository.Save();
                            var ChangedUsers = _RoleRightRepository.GetFirst(x => x.RIGHTS_OF_ROLE_ID == rightsOfRoles.RIGHTS_OF_ROLE_ID && x.PRACTICE_CODE == profile.PracticeCode && !x.DELETED);
                            if (ChangedUsers != null)
                            {
                                ChangedUsers.CHECKED = true;
                                ChangedUsers.MODIFIED_BY = profile.UserName;
                                ChangedUsers.MODIFIED_DATE = Helper.GetCurrentDate();
                                _RoleRightRepository.Update(ChangedUsers);
                            }
                        }
                    }
                }
            }
            _RoleRightRepository.Save();
        }
        public List<Role> GetPracticeRole(long practiceCode)
        {
            var parmPracticeCode = new SqlParameter("PRACTICE_CODE", SqlDbType.BigInt) { Value = practiceCode };
            var role = SpRepository<Role>.GetListWithStoreProcedure(@"exec FOX_PROC_GET_PRACTICE_ROLE @PRACTICE_CODE", parmPracticeCode);
            return role;
        }

        public int UpdatePassword(PasswordChangeRequest request, UserProfile profile)
        {
            var _user = _UserRepository.GetByID(request.User_id);
            if (_user != null)
            {
                _user.PasswordHash = request.PasswordHash;
                _user.PASSWORD = Encrypt.getEncryptedCode(System.Web.HttpUtility.UrlDecode(request.Password));
                _user.MODIFIED_BY = profile.UserName;
                _user.MODIFIED_DATE = Helper.GetCurrentDate();
                _UserRepository.Update(_user);
                _UserRepository.Save();
                PasswordHistory ph = new PasswordHistory()
                {
                    PASSWORD_ID = Helper.getMaximumId("PASSWORD_ID"),
                    DELETED = false,
                    USER_ID = _user.USER_ID,
                    PASSWORD = request.PasswordHash,
                    CREATED_BY = _user.USER_NAME,
                    CREATED_DATE = Helper.GetCurrentDate(),
                    MODIFIED_BY = _user.USER_NAME,
                    MODIFIED_DATE = Helper.GetCurrentDate(),
                    PRACTICE_CODE = _user.PRACTICE_CODE
                };
                _passwordHistoryRepository.Insert(ph);
                _passwordHistoryRepository.Save();
                return 1;
            }
            else
            {
                return 0;
            }
        }
        public int UpdateADUserPassword(string password, string hashpassword, UserProfile profile)
        {
            var _user = _UserRepository.GetByID(profile.userID);
            if (_user != null)
            {
                if (_user.PASSWORD != password)
                {
                    _user.PasswordHash = hashpassword;
                    _user.PASSWORD = password;
                    _user.MODIFIED_BY = "FOX Team";
                    _user.MODIFIED_DATE = Helper.GetCurrentDate();
                    _UserRepository.Update(_user);
                    _UserRepository.Save();
                    PasswordHistory ph = new PasswordHistory()
                    {
                        PASSWORD_ID = Helper.getMaximumId("PASSWORD_ID"),
                        DELETED = false,
                        USER_ID = _user.USER_ID,
                        PASSWORD = hashpassword,
                        CREATED_BY = "FOX Team",
                        CREATED_DATE = Helper.GetCurrentDate(),
                        MODIFIED_BY = "FOX Team",
                        MODIFIED_DATE = Helper.GetCurrentDate(),
                        PRACTICE_CODE = _user.PRACTICE_CODE
                    };
                    _passwordHistoryRepository.Insert(ph);
                    _passwordHistoryRepository.Save();
                    return 1;
                }
                else
                {
                    return 0;
                }
            }
            return 1;
        }
        public void AddRole(RoleToAdd request, UserProfile profile)
        {
            var role = _RoleRepository.GetByID(request.ROLE_ID);
            if (role != null)
            {
                role.ROLE_NAME = request.ROLE_NAME;
                role.MODIFIED_BY = profile.UserName;
                role.MODIFIED_DATE = Helper.GetCurrentDate();
                role.DELETED = false;
                _RoleRepository.Update(role);
                _RoleRepository.Save();
            }
            else
            {
                request.PRACTICE_CODE = profile.PracticeCode;
                request.ROLE_ID = Helper.getMaximumId("ROLE_ID");
                request.CREATED_DATE = Helper.GetCurrentDate();
                request.CREATED_BY = profile.UserName;
                request.MODIFIED_BY = profile.UserName;
                request.MODIFIED_DATE = Helper.GetCurrentDate();
                request.DELETED = false;
                _RoleRepository.Insert(request);
                _RoleRepository.Save();
                var parmPracticeCode = new SqlParameter("PRACTICE_CODE", SqlDbType.BigInt) { Value = profile.PracticeCode };
                var parmRoleId = new SqlParameter("ROLE_ID", SqlDbType.BigInt) { Value = request.ROLE_ID };
                var roleRight = SpRepository<Role>.GetListWithStoreProcedure(@"exec FOX_PROC_SET_PRACTICE_ROLE_RIGHTS @PRACTICE_CODE,@ROLE_ID", parmPracticeCode, parmRoleId);
            }

        }
        public void DeleteRole(Role req, UserProfile profile)
        {
            var role = _RoleRepository.GetByID(req.ROLE_ID);
            if (role != null)
            {
                //soft delete practice role rights (if any)
                var rights = _RoleRightRepository.GetMany(x => x.RIGHTS_OF_ROLE_ID == req.RIGHTS_OF_ROLE_ID && x.PRACTICE_CODE == profile.PracticeCode);
                foreach (var right in rights)
                {
                    right.MODIFIED_BY = profile.UserName;
                    right.MODIFIED_DATE = Helper.GetCurrentDate();
                    right.DELETED = true;
                    _RoleRightRepository.Update(right);
                    _RoleRightRepository.Save();
                }

                //soft delete rights of roles (if any)
                var rightsOfRoles = _RightsOfRoleRepository.GetMany(x => x.ROLE_ID == req.ROLE_ID);
                foreach (var rightOfRoles in rightsOfRoles)
                {
                    rightOfRoles.MODIFIED_BY = profile.UserName;
                    rightOfRoles.MODIFIED_DATE = Helper.GetCurrentDate();
                    rightOfRoles.DELETED = true;
                    _RightsOfRoleRepository.Update(rightOfRoles);
                    _RightsOfRoleRepository.Save();
                }

                //soft delete role
                role.MODIFIED_BY = profile.UserName;
                role.MODIFIED_DATE = Helper.GetCurrentDate();
                role.DELETED = true;
                _RoleRepository.Update(role);
                _RoleRepository.Save();
            }
        }
        public long GetADRole(long practiveCode, string roleName, long roleId)
        {
            if (!string.IsNullOrWhiteSpace(roleName))
            {
                ActiveDirectoryRole adRole = _ADRole.GetFirst(r => !r.DELETED && r.PRACTICE_CODE == practiveCode && r.AD_ROLE_NAME.ToLower().Equals(roleName.ToLower()));
                if (adRole == null)
                {
                    var role = _RoleRepository.Get(t => !t.DELETED && t.PRACTICE_CODE == practiveCode && t.ROLE_NAME.ToLower().Equals("default"));
                    return role?.ROLE_ID ?? roleId;
                }
                else
                {
                    var role = _RoleRepository.Get(t => !t.DELETED && (t.PRACTICE_CODE == practiveCode || t.PRACTICE_CODE == null) && t.ROLE_ID == adRole.ROLE_ID);
                    return role?.ROLE_ID ?? roleId;
                }
            }
            else
            {
                return roleId;
            }
        }
        public List<RoleAndRights> GetCurrentUserRights(long roleId, long practiceCode)
        {
            var roleID = new SqlParameter("ROLE_ID", SqlDbType.BigInt) { Value = roleId };
            var pc = new SqlParameter("PRACTICE_CODE", SqlDbType.BigInt) { Value = practiceCode };
            var roleAndList = SpRepository<RoleAndRights>.GetListWithStoreProcedure(@"exec FOX_PROC_GET_ROLE_RIGHTS_BY_ROLEID @PRACTICE_CODE,@ROLE_ID", pc, roleID);
            return roleAndList;
        }
        public bool EmailExists(EmailExist userEmail)
        {
            var usr = _UserRepository.GetFirst(x => x.EMAIL == userEmail.EMAIL);
            return usr != null ? true : false;
        }
        private List<FOX_TBL_ZIP_STATE_COUNTY> GetCountiesByReferralRegionId(long referralRegionId, long practiceCode)
        {
            var _paramsPracticeCode = new SqlParameter("PRACTICE_CODE", SqlDbType.BigInt) { Value = practiceCode };
            var _paramsReferralRegionId = new SqlParameter("REFERRAL_REGION_ID", SqlDbType.BigInt) { Value = referralRegionId };
            return SpRepository<FOX_TBL_ZIP_STATE_COUNTY>.GetListWithStoreProcedure(@"Exec [FOX_PROC_GET_COUNTIES_BY_REFERRAL_REGION_ID] @PRACTICE_CODE ,@REFERRAL_REGION_ID", _paramsPracticeCode, _paramsReferralRegionId);
        }
        private void UpdateCountiesByReferralRegionId(long? referralRegionId, long practiceCode, long zip_state_county_id)
        {
            var _paramsPracticeCode = new SqlParameter("PRACTICE_CODE", SqlDbType.BigInt) { Value = practiceCode };
            var _paramsReferralRegionId = new SqlParameter("REFERRAL_REGION_ID", SqlDbType.BigInt) { Value = referralRegionId };
            var _paramsReferralid = new SqlParameter("ZIP_STATE_COUNTY_ID", SqlDbType.BigInt) { Value = zip_state_county_id };

            SpRepository<FOX_TBL_ZIP_STATE_COUNTY>.GetSingleObjectWithStoreProcedure(@"Exec [FOX_PROC_DELETE_COUNTIES_REFERRAL_REGION] @PRACTICE_CODE ,@REFERRAL_REGION_ID, @ZIP_STATE_COUNTY_ID", _paramsPracticeCode, _paramsReferralRegionId, _paramsReferralid);
        }
        //private void DeleteStateCountyMapping(ReferralRegion referralRegion, UserProfile profile)
        //{
        //    var existingCounties = _referralRegionCounty.GetMany(c => !c.DELETED && c.PRACTICE_CODE == profile.PracticeCode && c.REFERRAL_REGION_ID == referralRegion.REFERRAL_REGION_ID);
        //    //////Commented code after removing county functaionality to procedure
        //    if (existingCounties != null && existingCounties.Count > 0)
        //    {
        //        foreach (var county in existingCounties)
        //        {
        //            county.DELETED = true;
        //            county.MODIFIED_BY = profile.UserName;
        //            county.MODIFIED_DATE = Helper.GetCurrentDate();
        //            _referralRegionCounty.Update(county);
        //        }
        //        _referralRegionCounty.Save();

        //        foreach (REFERRAL_REGION_COUNTY county in existingCounties)
        //        {
        //            UpdateCountiesByReferralRegionId(county.REFERRAL_REGION_ID, profile.PracticeCode, county.ZIP_STATE_COUNTY_ID.Value);
        //        }

        //    }
        //}
        private void DeleteStateCountyMapping(ReferralRegion referralRegion, UserProfile profile)
        {
            var existingCounties = _zipCityStateCountyRepository.GetMany(c => !c.DELETED && c.PRACTICE_CODE == profile.PracticeCode && c.REFERRAL_REGION_ID == referralRegion.REFERRAL_REGION_ID);
            //////Commented code after removing county functaionality to procedure
            if (existingCounties != null && existingCounties.Count > 0)
            {
                foreach (FOX_TBL_ZIP_STATE_COUNTY county in existingCounties)
                {
                    UpdateCountiesByReferralRegionId(county.REFERRAL_REGION_ID, profile.PracticeCode, county.ZIP_STATE_COUNTY_ID);
                }

            }
        }
        private void RemoveOldStateCounties(ReferralRegion referralRegion)
        {
            referralRegion.COUNTIES.RemoveAll(x => x.STATE != referralRegion.STATE_CODE);
        }
        //public void AddUpdateReferralRegion(ReferralRegion referralRegion, UserProfile profile)
        //{
        //    var refRegion = _ReferralRegionRepository.GetByID(referralRegion.REFERRAL_REGION_ID);
        //    bool isNewRegion = false;
        //    long Referral_RegionID = 0;
        //    string operation;
        //    bool isNewState = false;
        //    if (refRegion == null)
        //    {
        //        isNewRegion = true;
        //        Referral_RegionID = referralRegion.REFERRAL_REGION_ID = Helper.getMaximumId("REFERRAL_REGION_ID");
        //        if (referralRegion.IS_INACTIVE == null)
        //        {
        //            referralRegion.IS_INACTIVE = false;
        //            referralRegion.IS_ACTIVE = !(referralRegion.IS_INACTIVE ?? false);
        //        }
        //        referralRegion.PRACTICE_CODE = profile.PracticeCode;
        //        referralRegion.MODIFIED_BY = referralRegion.CREATED_BY = profile.UserName;
        //        referralRegion.MODIFIED_DATE = referralRegion.CREATED_DATE = Helper.GetCurrentDate();
        //        referralRegion.DELETED = false;
        //        if (!string.IsNullOrEmpty(referralRegion.IN_ACTIVEDATE_Str))
        //            referralRegion.IN_ACTIVEDATE = Convert.ToDateTime(referralRegion.IN_ACTIVEDATE_Str);
        //        _ReferralRegionRepository.Insert(referralRegion);
        //        operation = "add";
        //    }
        //    else
        //    {

        //        if (referralRegion.IS_INACTIVE == null)
        //        {
        //            referralRegion.IS_INACTIVE = false;
        //            referralRegion.IS_ACTIVE = !(referralRegion.IS_INACTIVE ?? false);
        //        }
        //        refRegion.REFERRAL_REGION_CODE = referralRegion.REFERRAL_REGION_CODE;
        //        refRegion.REFERRAL_REGION_NAME = referralRegion.REFERRAL_REGION_NAME;
        //        refRegion.ACCOUNT_MANAGER_EMAIL = referralRegion.ACCOUNT_MANAGER_EMAIL;
        //        refRegion.REGIONAL_DIRECTOR_ID = referralRegion.REGIONAL_DIRECTOR_ID;
        //        //////////////////////
        //        refRegion.ACCOUNT_MANAGER_ID = referralRegion.ACCOUNT_MANAGER_ID;
        //        //////////////////////
        //        refRegion.ACCOUNT_MANAGER = referralRegion.ACCOUNT_MANAGER;
        //        //refRegion.IS_ACTIVE = referralRegion.IS_ACTIVE;
        //        refRegion.IS_ACTIVE = !(referralRegion.IS_INACTIVE ?? false);
        //        refRegion.IS_INACTIVE = referralRegion.IS_INACTIVE;
        //        refRegion.MODIFIED_BY = profile.UserName;
        //        refRegion.MODIFIED_DATE = Helper.GetCurrentDate();
        //        refRegion.DELETED = referralRegion.DELETED;
        //        if (refRegion.STATE_CODE != referralRegion.STATE_CODE)
        //        {
        //            isNewState = true;
        //            DeleteStateCountyMapping(referralRegion, profile);
        //            referralRegion.COUNTIES.RemoveAll(x => x.STATE != referralRegion.STATE_CODE);
        //            //RemoveOldStateCounties(referralRegion);
        //            //referralRegion.COUNTIES[0].STATE
        //            //referralRegion.COUNTIES = null;
        //            //referralRegion.ZipStateCountyList = null;
        //        }
        //        refRegion.STATE_CODE = referralRegion.STATE_CODE;
        //        refRegion.ALTERNATE_REGION_ID = referralRegion.ALTERNATE_REGION_ID;
        //        if (!string.IsNullOrEmpty(referralRegion.IN_ACTIVEDATE_Str))
        //            refRegion.IN_ACTIVEDATE = Convert.ToDateTime(referralRegion.IN_ACTIVEDATE_Str);
        //        if (referralRegion.IN_ACTIVEDATE == null)
        //            refRegion.IN_ACTIVEDATE = referralRegion.IN_ACTIVEDATE;
        //        _ReferralRegionRepository.Update(refRegion);
        //        Referral_RegionID = referralRegion.REFERRAL_REGION_ID;
        //        operation = "update";
        //        var regionCounties = GetCountiesByReferralRegionId(refRegion.REFERRAL_REGION_ID, profile.PracticeCode).ToList();
        //        if (regionCounties != null && !isNewRegion)
        //        {

        //            foreach (var matchCounty in regionCounties)
        //            {
        //                foreach (var county in referralRegion.COUNTIES)
        //                {

        //                    if (county.COUNTY != matchCounty.COUNTY)
        //                    {
        //                        //practice_code Check
        //                        var result = _zipCityStateCountyRepository.GetMany(x => x.COUNTY == matchCounty.COUNTY && x.PRACTICE_CODE == profile.PracticeCode && !x.DELETED && x.IS_MAP == true && x.REFERRAL_REGION_ID == Referral_RegionID).ToList();
        //                        if (result != null)
        //                        {
        //                            foreach (var item in result)
        //                            {
        //                                //item.IS_MAP = true;
        //                                //item.REFERRAL_REGION_ID = null;
        //                                //_zipCityStateCountyRepository.Update(item);
        //                                //_zipCityStateCountyRepository.Save();
        //                            }
        //                        }


        //                    }
        //                }

        //            }
        //        }
        //    }

        //    _ReferralRegionRepository.Save();
        //    #region Changes made by Muhammad Imran

        //    if (operation == "add") // This is newly created region
        //    {
        //        foreach (var county in referralRegion.COUNTIES) // save all the counties with the region
        //        {
        //            var referralRegionCounty = new REFERRAL_REGION_COUNTY()
        //            {
        //                CREATED_BY = profile.UserName,
        //                CREATED_DATE = Helper.GetCurrentDate(),
        //                DELETED = false,
        //                MODIFIED_BY = profile.UserName,
        //                MODIFIED_DATE = Helper.GetCurrentDate(),
        //                PRACTICE_CODE = profile.PracticeCode,
        //                REFERRAL_REGION_COUNTY_ID = Helper.getMaximumId("FOX_REFERRAL_REGION_COUNTY_ID"),
        //                REFERRAL_REGION_ID = Referral_RegionID,
        //                ZIP_STATE_COUNTY_ID = county.ZIP_STATE_COUNTY_ID

        //            };
        //            _referralRegionCounty.Insert(referralRegionCounty);
        //        }
        //        _referralRegionCounty.Save();

        //        foreach (var county in referralRegion.COUNTIES) // save all the zip code in the counties against that region
        //        {
        //            List<FOX_TBL_ZIP_STATE_COUNTY> DBzipCityStateCounty = _zipCityStateCountyRepository.GetMany(x => x.COUNTY == county.COUNTY && x.STATE == county.STATE && x.REFERRAL_REGION_ID == null && !x.DELETED && x.PRACTICE_CODE == profile.PracticeCode);
        //            foreach (FOX_TBL_ZIP_STATE_COUNTY obj in DBzipCityStateCounty)
        //            {
        //                obj.IS_MAP = true;
        //                obj.REFERRAL_REGION_ID = Referral_RegionID;
        //                obj.MODIFIED_BY = profile.UserName;
        //                obj.MODIFIED_DATE = Helper.GetCurrentDate();
        //                _zipCityStateCountyRepository.Update(obj);
        //                _zipCityStateCountyRepository.Save();
        //            }
        //        }
        //    }
        //    else // Update the region
        //    {
        //        //First check if there is any no county attached with the region and check from database if there is already any mapped county
        //        //against that region then delete that county mapping
        //        if (referralRegion.COUNTIES == null || referralRegion.COUNTIES.Count == 0)
        //        {
        //            var existingCounties = _referralRegionCounty.GetMany(c => !c.DELETED && c.PRACTICE_CODE == profile.PracticeCode && c.REFERRAL_REGION_ID == Referral_RegionID);
        //            //////Commented code after removing county functaionality to procedure
        //            foreach (var county in existingCounties)
        //            {
        //                county.DELETED = true;
        //                county.MODIFIED_BY = profile.UserName;
        //                county.MODIFIED_DATE = Helper.GetCurrentDate();
        //                _referralRegionCounty.Update(county);
        //            }
        //            _referralRegionCounty.Save();
        //            //Delete the mapping of deleted counteis
        //            var existingCounties1 = _referralRegionCounty.GetMany(c => c.DELETED && c.PRACTICE_CODE == profile.PracticeCode && c.REFERRAL_REGION_ID == Referral_RegionID);
        //            if (existingCounties1 != null && existingCounties1.Count > 0)
        //            {
        //                foreach (REFERRAL_REGION_COUNTY county in existingCounties1)
        //                {
        //                    UpdateCountiesByReferralRegionId(county.REFERRAL_REGION_ID, profile.PracticeCode, county.ZIP_STATE_COUNTY_ID.Value);
        //                }
        //            }
        //        }
        //        else //counties are attached with the referral region
        //        {
        //            //DELETE ALL THE COUNTIES IN THE REGION
        //            var existingCounties = _referralRegionCounty.GetMany(c => !c.DELETED && c.PRACTICE_CODE == profile.PracticeCode && c.REFERRAL_REGION_ID == Referral_RegionID);
        //            foreach (var county in existingCounties)
        //            {
        //                county.DELETED = true;
        //                county.MODIFIED_BY = profile.UserName;
        //                county.MODIFIED_DATE = Helper.GetCurrentDate();
        //                _referralRegionCounty.Update(county);
        //                _referralRegionCounty.Save();
        //            }

        //            if (referralRegion.COUNTIES != null || referralRegion.COUNTIES.Count > 0)
        //            {

        //                foreach (var county in referralRegion.COUNTIES)
        //                {
        //                    //AGAIN MAP THE EXISTING COUNTIES   
        //                    var existingCounty = _referralRegionCounty.Get(c => c.DELETED && c.PRACTICE_CODE == profile.PracticeCode && c.REFERRAL_REGION_ID == Referral_RegionID && c.ZIP_STATE_COUNTY_ID == county.ZIP_STATE_COUNTY_ID);
        //                    if (existingCounty != null)
        //                    {
        //                        existingCounty.DELETED = false;
        //                        existingCounty.MODIFIED_BY = profile.UserName;
        //                        existingCounty.MODIFIED_DATE = Helper.GetCurrentDate();
        //                        _referralRegionCounty.Update(existingCounty);
        //                        _referralRegionCounty.Save();
        //                    }
        //                    else // NEW COUNTY
        //                    {
        //                        var referralRegionCounty = new REFERRAL_REGION_COUNTY()
        //                        {
        //                            CREATED_BY = profile.UserName,
        //                            CREATED_DATE = Helper.GetCurrentDate(),
        //                            DELETED = false,
        //                            MODIFIED_BY = profile.UserName,
        //                            MODIFIED_DATE = Helper.GetCurrentDate(),
        //                            PRACTICE_CODE = profile.PracticeCode,
        //                            REFERRAL_REGION_COUNTY_ID = Helper.getMaximumId("FOX_REFERRAL_REGION_COUNTY_ID"),
        //                            REFERRAL_REGION_ID = Referral_RegionID,
        //                            ZIP_STATE_COUNTY_ID = county.ZIP_STATE_COUNTY_ID
        //                            //REGION_ZIPCODE_DATA_ID = county.ZIP_STATE_COUNTY_ID

        //                        };
        //                        _referralRegionCounty.Insert(referralRegionCounty);
        //                        _referralRegionCounty.Save();
        //                        // MAP ALL ITS ZIP CODE IN COUNTY
        //                        //List<FOX_TBL_ZIP_STATE_COUNTY> DBzipCityStateCounty = _zipCityStateCountyRepository.GetMany(x => x.COUNTY == county.COUNTY && x.STATE == county.STATE && !x.DELETED && x.PRACTICE_CODE == profile.PracticeCode);
        //                        //foreach (FOX_TBL_ZIP_STATE_COUNTY obj in DBzipCityStateCounty)
        //                        //{
        //                        //    obj.IS_MAP = true;
        //                        //    obj.REFERRAL_REGION_ID = Referral_RegionID;
        //                        //    obj.MODIFIED_BY = profile.UserName;
        //                        //    obj.MODIFIED_DATE = Helper.GetCurrentDate();
        //                        //    _zipCityStateCountyRepository.Update(obj);
        //                        //    _zipCityStateCountyRepository.Save();
        //                        //}

        //                    }
        //                }
        //            }

        //            //Delete the mapping of deleted counteis                    
        //            var existingCounties1 = _referralRegionCounty.GetMany(c => c.DELETED && c.PRACTICE_CODE == profile.PracticeCode && c.REFERRAL_REGION_ID == Referral_RegionID);
        //            if (existingCounties1 != null && existingCounties1.Count > 0)
        //            {
        //                //foreach(REFERRAL_REGION_COUNTY county in existingCounties1)
        //                //{
        //                //    //UpdateCountiesByReferralRegionId(county.REFERRAL_REGION_ID, profile.PracticeCode, county.ZIP_STATE_COUNTY_ID.Value);                           

        //                //}
        //            }

        //            foreach (var county in referralRegion.COUNTIES)
        //            {
        //                if (county.Is_New_County) // if county is new then map all the county zip code against the region
        //                {
        //                    List<FOX_TBL_ZIP_STATE_COUNTY> DBzipCityStateCounty = _zipCityStateCountyRepository.GetMany(x => x.COUNTY == county.COUNTY && x.STATE == county.STATE && !x.DELETED && x.REFERRAL_REGION_ID == null && x.PRACTICE_CODE == profile.PracticeCode);
        //                    foreach (FOX_TBL_ZIP_STATE_COUNTY obj in DBzipCityStateCounty)
        //                    {
        //                        obj.IS_MAP = true;
        //                        obj.REFERRAL_REGION_ID = Referral_RegionID;
        //                        obj.MODIFIED_BY = profile.UserName;
        //                        obj.MODIFIED_DATE = Helper.GetCurrentDate();
        //                        _zipCityStateCountyRepository.Update(obj);
        //                        _zipCityStateCountyRepository.Save();
        //                    }
        //                }
        //            }
        //        }
        //    }

        //    #endregion Changes made by Muhammad Imran
        //    //    if (referralRegion.COUNTIES != null && referralRegion.COUNTIES.Count != 0)
        //    //    {
        //    //        foreach (var county in referralRegion.COUNTIES)
        //    //        {
        //    //            if (county.Sublist != null && county.Sublist.Count != 0 && !isNewRegion)
        //    //            {
        //    //                foreach (var subcounty in county.Sublist)
        //    //                {
        //    //                    var DBzipCityStateCounty = _zipCityStateCountyRepository.Get(x => x.ZIP_STATE_COUNTY_ID == subcounty.ZIP_STATE_COUNTY_ID && !x.DELETED && x.PRACTICE_CODE == profile.PracticeCode);
        //    //                    if (DBzipCityStateCounty != null)
        //    //                    {
        //    //                        DBzipCityStateCounty.MODIFIED_BY = profile.UserName;
        //    //                        DBzipCityStateCounty.MODIFIED_DATE = Helper.GetCurrentDate();
        //    //                        DBzipCityStateCounty.IS_MAP = true;
        //    //                        if (subcounty.IS_MAP == false && subcounty.REFERRAL_REGION_ID != null)
        //    //                        {
        //    //                            DBzipCityStateCounty.REFERRAL_REGION_ID = null;
        //    //                        }
        //    //                        else
        //    //                        {
        //    //                            DBzipCityStateCounty.REFERRAL_REGION_ID = Referral_RegionID;
        //    //                        }
        //    //                        _zipCityStateCountyRepository.Update(DBzipCityStateCounty);
        //    //                        _zipCityStateCountyRepository.Save();
        //    //                    }
        //    //                }
        //    //            }
        //    //            else
        //    //            {
        //    //                var DBzipCityStateCountyMain = _zipCityStateCountyRepository.Get(x => x.ZIP_STATE_COUNTY_ID == county.ZIP_STATE_COUNTY_ID && !x.DELETED && x.PRACTICE_CODE == profile.PracticeCode);
        //    //                if (DBzipCityStateCountyMain != null)
        //    //                {
        //    //                    DBzipCityStateCountyMain.MODIFIED_BY = profile.UserName;
        //    //                    DBzipCityStateCountyMain.MODIFIED_DATE = Helper.GetCurrentDate();
        //    //                    DBzipCityStateCountyMain.IS_MAP = true;
        //    //                    DBzipCityStateCountyMain.REFERRAL_REGION_ID = Referral_RegionID;
        //    //                    _zipCityStateCountyRepository.Update(DBzipCityStateCountyMain);
        //    //                    _zipCityStateCountyRepository.Save();
        //    //                }
        //    //            }
        //    //        }
        //    //    }
        //    //    else // if there is no county attached with region
        //    //    {
        //    //       var  data = _zipCityStateCountyRepository.GetMany(x => x.REFERRAL_REGION_ID == Referral_RegionID && !x.DELETED && x.PRACTICE_CODE == profile.PracticeCode);
        //    //        if (data != null && !isNewRegion)
        //    //        {
        //    //            foreach (var item in data)
        //    //            {
        //    //                item.MODIFIED_BY = profile.UserName;
        //    //                item.MODIFIED_DATE = Helper.GetCurrentDate();
        //    //                item.IS_MAP = false;
        //    //                //item.REFERRAL_REGION_ID = null;
        //    //                _zipCityStateCountyRepository.Update(item);
        //    //                _zipCityStateCountyRepository.Save();
        //    //            }
        //    //        }
        //    //    }

        //    //        //_UserRepository.Save();
        //    //        if (operation == "add")
        //    //        {
        //    //            foreach (var county in referralRegion.COUNTIES)
        //    //            {
        //    //            if (county.Sublist != null && county.Sublist.Count != 0 && !isNewRegion)
        //    //            {
        //    //                foreach (var i in county.Sublist)
        //    //                {
        //    //                    var existingCounty = _referralRegionCounty.GetFirst(c => !c.DELETED && c.PRACTICE_CODE == profile.PracticeCode  && c.ZIP_STATE_COUNTY_ID == i.ZIP_STATE_COUNTY_ID);
        //    //                    if (existingCounty != null)
        //    //                    {
        //    //                        existingCounty.CREATED_BY = profile.UserName;
        //    //                        existingCounty.CREATED_DATE = Helper.GetCurrentDate();
        //    //                        existingCounty.DELETED = false;
        //    //                        existingCounty.MODIFIED_BY = profile.UserName;
        //    //                        existingCounty.MODIFIED_DATE = Helper.GetCurrentDate();
        //    //                        existingCounty.PRACTICE_CODE = profile.PracticeCode;
        //    //                        //existingCounty.REFERRAL_REGION_COUNTY_ID = Helper.getMaximumId("FOX_REFERRAL_REGION_COUNTY_ID");
        //    //                        existingCounty.REFERRAL_REGION_ID = Referral_RegionID;
        //    //                        existingCounty.ZIP_STATE_COUNTY_ID = i.ZIP_STATE_COUNTY_ID;
        //    //                        //REGION_ZIPCODE_DATA_ID = county.ZIP_STATE_COUNTY_ID
        //    //                        _referralRegionCounty.Update(existingCounty);
        //    //                    }
        //    //                    else
        //    //                    {
        //    //                        var referralRegionCounty = new REFERRAL_REGION_COUNTY()
        //    //                        {
        //    //                            CREATED_BY = profile.UserName,
        //    //                            CREATED_DATE = Helper.GetCurrentDate(),
        //    //                            DELETED = false,
        //    //                            MODIFIED_BY = profile.UserName,
        //    //                            MODIFIED_DATE = Helper.GetCurrentDate(),
        //    //                            PRACTICE_CODE = profile.PracticeCode,
        //    //                            REFERRAL_REGION_COUNTY_ID = Helper.getMaximumId("FOX_REFERRAL_REGION_COUNTY_ID"),
        //    //                            REFERRAL_REGION_ID = Referral_RegionID,
        //    //                            ZIP_STATE_COUNTY_ID = i.ZIP_STATE_COUNTY_ID
        //    //                            //REGION_ZIPCODE_DATA_ID = county.ZIP_STATE_COUNTY_ID

        //    //                        };
        //    //                        _referralRegionCounty.Insert(referralRegionCounty);
        //    //                    }

        //    //                }
        //    //            }
        //    //            else
        //    //            {
        //    //                var referralRegionCounty = new REFERRAL_REGION_COUNTY()
        //    //                {
        //    //                    CREATED_BY = profile.UserName,
        //    //                    CREATED_DATE = Helper.GetCurrentDate(),
        //    //                    DELETED = false,
        //    //                    MODIFIED_BY = profile.UserName,
        //    //                    MODIFIED_DATE = Helper.GetCurrentDate(),
        //    //                    PRACTICE_CODE = profile.PracticeCode,
        //    //                    REFERRAL_REGION_COUNTY_ID = Helper.getMaximumId("FOX_REFERRAL_REGION_COUNTY_ID"),
        //    //                    REFERRAL_REGION_ID = Referral_RegionID,
        //    //                    ZIP_STATE_COUNTY_ID = county.ZIP_STATE_COUNTY_ID
        //    //                    //REGION_ZIPCODE_DATA_ID = county.ZIP_STATE_COUNTY_ID

        //    //                };
        //    //                _referralRegionCounty.Insert(referralRegionCounty);

        //    //            }
        //    //            }
        //    //        _referralRegionCounty.Save();
        //    //        }
        //    //        else // if region is being updated and county to remove
        //    //        {
        //    //            var existingCounties = _referralRegionCounty.GetMany(c => !c.DELETED && c.PRACTICE_CODE == profile.PracticeCode && c.REFERRAL_REGION_ID == Referral_RegionID);
        //    //            foreach (var county in existingCounties)
        //    //            {
        //    //                county.DELETED = true; 
        //    //                county.MODIFIED_BY = profile.UserName;
        //    //                county.MODIFIED_DATE = Helper.GetCurrentDate();
        //    //                _referralRegionCounty.Update(county);
        //    //           }

        //    //            _referralRegionCounty.Save();

        //    //       if (referralRegion.COUNTIES != null && referralRegion.COUNTIES.Count() != 0)

        //    //        {

        //    //        foreach (var county in referralRegion.COUNTIES)
        //    //        {
        //    //            var existingCounty = _referralRegionCounty.GetFirst(c => c.DELETED && c.PRACTICE_CODE == profile.PracticeCode && c.REFERRAL_REGION_ID == Referral_RegionID && c.ZIP_STATE_COUNTY_ID == county.ZIP_STATE_COUNTY_ID);
        //    //                if (existingCounty != null)
        //    //                {
        //    //                    existingCounty.DELETED = false;
        //    //                    existingCounty.MODIFIED_BY = profile.UserName;
        //    //                    existingCounty.MODIFIED_DATE = Helper.GetCurrentDate();
        //    //                    _referralRegionCounty.Update(existingCounty);
        //    //                }
        //    //                else
        //    //                {
        //    //                    var referralRegionCounty = new REFERRAL_REGION_COUNTY()
        //    //                    {
        //    //                        CREATED_BY = profile.UserName,
        //    //                        CREATED_DATE = Helper.GetCurrentDate(),
        //    //                        DELETED = false,
        //    //                        MODIFIED_BY = profile.UserName,
        //    //                        MODIFIED_DATE = Helper.GetCurrentDate(),
        //    //                        PRACTICE_CODE = profile.PracticeCode,
        //    //                        REFERRAL_REGION_COUNTY_ID = Helper.getMaximumId("FOX_REFERRAL_REGION_COUNTY_ID"),
        //    //                        REFERRAL_REGION_ID = Referral_RegionID,
        //    //                        ZIP_STATE_COUNTY_ID = county.ZIP_STATE_COUNTY_ID
        //    //                        //REGION_ZIPCODE_DATA_ID = county.ZIP_STATE_COUNTY_ID

        //    //                    };
        //    //                    _referralRegionCounty.Insert(referralRegionCounty);
        //    //                }
        //    //        }
        //    //    }
        //    //    _referralRegionCounty.Save();
        //    //}


        //}
        public void AddUpdateReferralRegion(ReferralRegion referralRegion, UserProfile profile)
        {
            var refRegion = _ReferralRegionRepository.GetByID(referralRegion.REFERRAL_REGION_ID);
            bool isNewRegion = false;
            long Referral_RegionID = 0;
            string operation;
            bool isNewState = false;
            if (refRegion == null)
            {
                isNewRegion = true;
                Referral_RegionID = referralRegion.REFERRAL_REGION_ID = Helper.getMaximumId("REFERRAL_REGION_ID");
                if (referralRegion.IS_INACTIVE == null)
                {
                    referralRegion.IS_INACTIVE = false;
                    referralRegion.IS_ACTIVE = !(referralRegion.IS_INACTIVE ?? false);
                }
                referralRegion.PRACTICE_CODE = profile.PracticeCode;
                referralRegion.MODIFIED_BY = referralRegion.CREATED_BY = profile.UserName;
                referralRegion.MODIFIED_DATE = referralRegion.CREATED_DATE = Helper.GetCurrentDate();
                referralRegion.DELETED = false;
                referralRegion.IS_ACTIVE = referralRegion.IS_ACTIVE;
                if (!string.IsNullOrEmpty(referralRegion.IN_ACTIVEDATE_Str))
                    referralRegion.IN_ACTIVEDATE = Convert.ToDateTime(referralRegion.IN_ACTIVEDATE_Str);
                _ReferralRegionRepository.Insert(referralRegion);
                operation = "add";
            }
            else
            {
                if (referralRegion.IS_INACTIVE == null)
                {
                    referralRegion.IS_INACTIVE = false;
                    referralRegion.IS_ACTIVE = !(referralRegion.IS_INACTIVE ?? false);
                }
                refRegion.REFERRAL_REGION_CODE = referralRegion.REFERRAL_REGION_CODE;
                refRegion.REFERRAL_REGION_NAME = referralRegion.REFERRAL_REGION_NAME;
                refRegion.ACCOUNT_MANAGER_EMAIL = referralRegion.ACCOUNT_MANAGER_EMAIL;
                refRegion.REGIONAL_DIRECTOR_ID = referralRegion.REGIONAL_DIRECTOR_ID;
                refRegion.REGIONAL_DIRECTOR_NAME = referralRegion.REGIONAL_DIRECTOR_NAME;
                refRegion.SENIOR_REGIONAL_DIRECTOR_ID = referralRegion.SENIOR_REGIONAL_DIRECTOR_ID;
                refRegion.SENIOR_REGIONAL_DIRECTOR_NAME = referralRegion.SENIOR_REGIONAL_DIRECTOR_NAME;
                //if(referralRegion.SENIOR_REGIONAL_DIRECTOR_ID!=null)
                //    refRegion.SENIOR_REGIONAL_DIRECTOR_ID = referralRegion.SENIOR_REGIONAL_DIRECTOR_ID;
                //else if(referralRegion.SENIOR_REGIONAL_DIRECTOR_ID==-1)
                //{
                //    referralRegion.SENIOR_REGIONAL_DIRECTOR_ID = null;
                //}
                //////////////////////
                refRegion.ACCOUNT_MANAGER_ID = referralRegion.ACCOUNT_MANAGER_ID;
                //////////////////////
                refRegion.ACCOUNT_MANAGER = referralRegion.ACCOUNT_MANAGER;
                //refRegion.IS_ACTIVE = referralRegion.IS_ACTIVE;
                refRegion.IS_ACTIVE = !(referralRegion.IS_INACTIVE ?? false);
                refRegion.IS_INACTIVE = referralRegion.IS_INACTIVE;
                refRegion.MODIFIED_BY = profile.UserName;
                refRegion.MODIFIED_DATE = Helper.GetCurrentDate();
                refRegion.DELETED = referralRegion.DELETED;
                if (refRegion.STATE_CODE != referralRegion.STATE_CODE)
                {
                    isNewState = true;
                    DeleteStateCountyMapping(referralRegion, profile);
                    //if (referralRegion?.COUNTIES != null && referralRegion?.COUNTIES.Count() > 0)
                    //{
                    //    referralRegion.COUNTIES.RemoveAll(x => x.STATE != referralRegion.STATE_CODE);
                    //}
                }
                refRegion.STATE_CODE = referralRegion.STATE_CODE;
                refRegion.ALTERNATE_REGION_ID = referralRegion.ALTERNATE_REGION_ID;
                if (!string.IsNullOrEmpty(referralRegion.IN_ACTIVEDATE_Str))
                    refRegion.IN_ACTIVEDATE = Convert.ToDateTime(referralRegion.IN_ACTIVEDATE_Str);
                if (referralRegion.IN_ACTIVEDATE == null)
                    refRegion.IN_ACTIVEDATE = referralRegion.IN_ACTIVEDATE;
                _ReferralRegionRepository.Update(refRegion);
                Referral_RegionID = referralRegion.REFERRAL_REGION_ID;
                operation = "update";
            }
            _ReferralRegionRepository.Save();
            if (operation == "add") // This is newly created region
            {
                if (referralRegion.COUNTIES != null || referralRegion.COUNTIES.Count > 0)
                {
                    foreach (var county in referralRegion.COUNTIES) // save all the zip code in the counties against that region
                    {
                        List<FOX_TBL_ZIP_STATE_COUNTY> DBzipCityStateCounty = _zipCityStateCountyRepository.GetMany(x => x.COUNTY.ToLower() == county.COUNTY.ToLower() && x.STATE == county.STATE && x.REFERRAL_REGION_ID == null && !x.DELETED && x.PRACTICE_CODE == profile.PracticeCode);
                        foreach (FOX_TBL_ZIP_STATE_COUNTY obj in DBzipCityStateCounty)
                        {
                            obj.IS_MAP = true;
                            obj.REFERRAL_REGION_ID = Referral_RegionID;
                            obj.MODIFIED_BY = profile.UserName;
                            obj.MODIFIED_DATE = Helper.GetCurrentDate();
                            _zipCityStateCountyRepository.Update(obj);
                            _zipCityStateCountyRepository.Save();
                        }
                    }
                }
            }
            else // Update the region
            {
                //First check if there is any no county attached with the region and check from database if there is already any mapped county
                //against that region then delete that county mapping
                if (referralRegion.COUNTIES == null || referralRegion.COUNTIES.Count == 0)
                {
                    //Delete the mapping of deleted counteis
                    var existingCounties1 = _zipCityStateCountyRepository.GetMany(c => !c.DELETED && c.PRACTICE_CODE == profile.PracticeCode && c.REFERRAL_REGION_ID == Referral_RegionID);
                    if (existingCounties1 != null && existingCounties1.Count > 0)
                    {
                        foreach (FOX_TBL_ZIP_STATE_COUNTY county in existingCounties1)
                        {
                            UpdateCountiesByReferralRegionId(county.REFERRAL_REGION_ID, profile.PracticeCode, county.ZIP_STATE_COUNTY_ID);
                        }
                    }
                }
                else //counties are attached with the referral region
                {
                    if (referralRegion.COUNTIES != null || referralRegion.COUNTIES.Count > 0)
                    {
                        foreach (var county in referralRegion.COUNTIES)
                        {
                            if (county.Is_New_County) // if county is new then map all the county zip code against the region
                            {
                                List<FOX_TBL_ZIP_STATE_COUNTY> DBzipCityStateCounty = _zipCityStateCountyRepository.GetMany(x => x.COUNTY.ToLower() == county.COUNTY.ToLower() && x.STATE == county.STATE && !x.DELETED && x.REFERRAL_REGION_ID == null && x.PRACTICE_CODE == profile.PracticeCode);
                                foreach (FOX_TBL_ZIP_STATE_COUNTY obj in DBzipCityStateCounty)
                                {
                                    obj.IS_MAP = true;
                                    obj.REFERRAL_REGION_ID = Referral_RegionID;
                                    obj.MODIFIED_BY = profile.UserName;
                                    obj.MODIFIED_DATE = Helper.GetCurrentDate();
                                    _zipCityStateCountyRepository.Update(obj);
                                    _zipCityStateCountyRepository.Save();
                                }
                            }
                            else
                            {

                            }
                        }
                    }
                }
            }
            if (referralRegion.Dashboard_Access != null)
            {
                if (referralRegion.Dashboard_AccessTemp != null)
                {
                    referralRegion.Dashboard_Access.AddRange(referralRegion.Dashboard_AccessTemp);
                }
                InsertReferralRegionDashBoardAccess(referralRegion.REFERRAL_REGION_ID, referralRegion.Dashboard_Access, profile);
            }
            if (referralRegion != null && referralRegion.IS_FAX_COVER_LETTER.HasValue)
            {
                var regionCoverLetterResponse = _RegionCoverLetterRepository.GetFirst(x => x.REFERRAL_REGION_ID == referralRegion.REFERRAL_REGION_ID && !x.DELETED);
                if(regionCoverLetterResponse == null)
                {
                    //Insert
                    RegionCoverLetter regionCover = new RegionCoverLetter();
                    regionCover.REGION_COVER_SHEET_ID = Helper.getMaximumId("REGION_COVER_SHEET_ID");
                    regionCover.REFERRAL_REGION_ID = referralRegion.REFERRAL_REGION_ID;
                    regionCover.REFERRAL_REGION_CODE = referralRegion.REFERRAL_REGION_CODE;
                    regionCover.IS_FAX_COVER_LETTER = referralRegion.IS_FAX_COVER_LETTER ?? false;
                    regionCover.CREATED_DATE = regionCover.MODIFIED_DATE = Helper.GetCurrentDate();
                    regionCover.CREATED_BY = profile.UserName;
                    regionCover.MODIFIED_BY = profile.UserName;
                    regionCover.FILE_PATH = referralRegion.FILE_PATH;
                    _RegionCoverLetterRepository.Insert(regionCover);
                    _RegionCoverLetterRepository.Save();
                }
                else
                {
                    //Update
                    regionCoverLetterResponse.IS_FAX_COVER_LETTER = referralRegion.IS_FAX_COVER_LETTER ?? false;
                    regionCoverLetterResponse.FILE_PATH = referralRegion.FILE_PATH;
                    regionCoverLetterResponse.MODIFIED_DATE = Helper.GetCurrentDate();
                    regionCoverLetterResponse.MODIFIED_BY = profile.UserName;
                    _RegionCoverLetterRepository.Update(regionCoverLetterResponse);
                    _RegionCoverLetterRepository.Save();
                }
            }
        }
        public ReferralRegion GetReferralRegion(ReferralRegionSearch referralRegionSearch, UserProfile profile)
        {
            if (referralRegionSearch.signal == 1)
            {
                return _ReferralRegionRepository.Get(x => !x.DELETED && x.PRACTICE_CODE == profile.PracticeCode && (x.REFERRAL_REGION_CODE?.ToLower() ?? "").Equals(referralRegionSearch.REFERRAL_REGION_CODE?.ToLower() ?? ""));
            }
            else if (referralRegionSearch.signal == 2)
            {
                return _ReferralRegionRepository.Get(x => !x.DELETED && x.PRACTICE_CODE == profile.PracticeCode && (x.REFERRAL_REGION_NAME?.ToLower() ?? "").Equals(referralRegionSearch.REFERRAL_REGION_NAME?.ToLower() ?? ""));
            }
            else return null;
        }
        public List<ReferralRegion> GetReferralRegionList(ReferralRegionSearch referralRegionSearch, UserProfile profile)
        {
            var parmPracticeCode = new SqlParameter("PRACTICE_CODE", SqlDbType.BigInt) { Value = profile.PracticeCode };
            var referralRegionCode = Helper.getDBNullOrValue("REFERRAL_REGION_CODE", referralRegionSearch.REFERRAL_REGION_CODE);
            var referralRegionName = Helper.getDBNullOrValue("REFERRAL_REGION_NAME", referralRegionSearch.REFERRAL_REGION_NAME);
            var searchString = new SqlParameter { ParameterName = "SEARCH_STRING", Value = referralRegionSearch.searchString };
            var CurrentPage = new SqlParameter { ParameterName = "CURRENT_PAGE", SqlDbType = SqlDbType.Int, Value = referralRegionSearch.CurrentPage };
            var RecordPerPage = new SqlParameter { ParameterName = "RECORD_PER_PAGE", SqlDbType = SqlDbType.Int, Value = referralRegionSearch.RecordPerPage };
            var SortBy = Helper.getDBNullOrValue("SORT_BY", referralRegionSearch.SortBy);
            var SortOrder = Helper.getDBNullOrValue("SORT_ORDER", referralRegionSearch.SortOrder);

            var referralRegionList = SpRepository<ReferralRegion>.GetListWithStoreProcedure(@"exec FOX_PROC_GET_REFERRAL_REGION_LIST @PRACTICE_CODE, @REFERRAL_REGION_CODE, @REFERRAL_REGION_NAME, @SEARCH_STRING, @CURRENT_PAGE, @RECORD_PER_PAGE, @SORT_BY, @SORT_ORDER",
               parmPracticeCode, referralRegionCode, referralRegionName, searchString, CurrentPage, RecordPerPage, SortBy, SortOrder);
            return referralRegionList;
        }
        public string ExportToExcelReferralRegion(ReferralRegionSearch referralRegionSearch, UserProfile profile)
        {
            try
            {
                string fileName = "Referral_Region_List";
                string exportPath = "";
                string path = string.Empty;
                bool exported;
                referralRegionSearch.CurrentPage = 1;
                referralRegionSearch.RecordPerPage = 0;
                var CalledFrom = "Referral_Region";
                string virtualPath = @"/" + profile.PracticeDocumentDirectory + "/" + "Fox/ExportedFiles/";
                exportPath = HttpContext.Current.Server.MapPath("~" + virtualPath);
                fileName = DocumentHelper.GenerateSignatureFileName(fileName) + ".xlsx";
                if (!Directory.Exists(exportPath))
                {
                    Directory.CreateDirectory(exportPath);
                }
                List<ReferralRegion> result = new List<ReferralRegion>();
                var pathtowriteFile = exportPath + "\\" + fileName;
                result = GetReferralRegionList(referralRegionSearch, profile);
                CultureInfo culture_info = Thread.CurrentThread.CurrentCulture;
                TextInfo text_info = culture_info.TextInfo;
                for (int i = 0; i < result.Count(); i++)
                {
                    result[i].REFERRAL_REGION_NAME = string.IsNullOrEmpty(result[i].REFERRAL_REGION_NAME) ? string.Empty : text_info.ToTitleCase(result[i].REFERRAL_REGION_NAME);
                    result[i].REGIONAL_DIRECTOR = string.IsNullOrEmpty(result[i].REGIONAL_DIRECTOR) ? string.Empty : text_info.ToTitleCase(result[i].REGIONAL_DIRECTOR);
                    result[i].SENIOR_REGIONAL_DIRECTOR = string.IsNullOrEmpty(result[i].SENIOR_REGIONAL_DIRECTOR) ? string.Empty : text_info.ToTitleCase(result[i].SENIOR_REGIONAL_DIRECTOR);
                    result[i].ACCOUNT_MANAGER = string.IsNullOrEmpty(result[i].ACCOUNT_MANAGER) ? string.Empty : text_info.ToTitleCase(result[i].ACCOUNT_MANAGER);
                    result[i].ROW = i + 1;

                }
                exported = ExportToExcel.CreateExcelDocument<ReferralRegion>(result, pathtowriteFile, CalledFrom.Replace(' ', '_'));
                return virtualPath + fileName;
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<right> GetRolesCheckBit(long practiceCode, string userName)
        {
            var pc = new SqlParameter("PRACTICE_CODE", SqlDbType.BigInt) { Value = practiceCode };
            var UserName = new SqlParameter("USER_NAME", SqlDbType.VarChar) { Value = userName };
            var roleAndList = SpRepository<RoleAndRights>.GetListWithStoreProcedure(@"exec [FOX_PROC_GET_ROLE_RIGHTS_USER] @PRACTICE_CODE,@USER_NAME", pc, UserName);
            List<right> right = (from x in roleAndList select new right() { RIGHT_NAME = x.RIGHT_NAME, RIGHT_ID = x.RIGHT_ID }).OrderBy(e => e.RIGHT_NAME).GroupBy(p => p.RIGHT_ID).Select(g => g.First()).ToList();
            for (var i = 0; i < right.Count; i++)
            {
                var roles = (from x in roleAndList where x.RIGHT_ID == right[i].RIGHT_ID select new Role() { IS_CHECKED = x.IS_CHECKED, ROLE_ID = x.ROLE_ID, ROLE_NAME = x.ROLE_NAME, RIGHT_ID = x.RIGHT_ID, RIGHTS_OF_ROLE_ID = x.RIGHTS_OF_ROLE_ID }).ToList();
                roles.Sort((x, y) => { return x.ROLE_ID < y.ROLE_ID ? 0 : 1; });
                right[i].Roles = roles;
            }
            return right;
        }
        public ResponseModel ValidateUserEmail(string email)
        {
            ResponseModel resp = new ResponseModel();
            try
            {
                var paramEmail = new SqlParameter("EMAIL", SqlDbType.VarChar) { Value = email };
                var user = SpRepository<User>.GetSingleObjectWithStoreProcedure(@"exec [FOX_SP_VALIDATEEMAIL] @EMAIL", paramEmail);
                if (user != null)
                {
                    if (user.IS_AD_USER ?? false)
                    {
                        resp.Success = true;
                        resp.Message = "";
                        resp.ErrorMessage = string.Empty;//"XaTvZZt85PPII";//"Please contact your network administrator at FOX Rehab for assistance.";
                        resp.AU = true;
                    }
                    else
                    {
                        resp.Success = true;
                        resp.Message = string.Empty;//"Password reset email has been sent.";
                        resp.ID = user.EMAIL;
                        resp.AU = false;
                    }
                }
                else
                {
                    resp.Success = true;
                    resp.Message = "";
                    resp.ErrorMessage = string.Empty;// "Wo78NHVTTlq65";//"Email address does not exist";
                    resp.AU = false;
                }
                return resp;
                //var result = _UserRepository.Get(t => !t.DELETED && t.IS_ACTIVE && (t.EMAIL.ToLower()).Equals(email.ToLower()));
                //if (result != null)
                //    return result.EMAIL;
                //return "";
            }
            catch (Exception exception)
            {
                //throw exception;
                throw;
            }
        }
        public int UpdatePassword(ResetPasswordViewModel data)
        {
            try
            {
                var _user = _UserRepository.Get(t => t.EMAIL.Equals(data.Email));
                DateTime tempDateTime = new DateTime(long.Parse(data.Ticks));
                tempDateTime = tempDateTime.AddHours(1);

                if (_user != null)
                {
                    if (_user.MODIFIED_DATE.AddHours(1) < tempDateTime && tempDateTime > DateTime.Now)
                    {
                        string newPassword = data.PasswordHash;
                        bool isValid = true;
                        List<PasswordHistory> previousPasswords = security.PasswordHistories.OrderByDescending(x => x.PASSWORD_ID).Where(u => u.USER_ID.Equals(_user.USER_ID) && u.DELETED == false).Take(5).ToList();
                        foreach (var password in previousPasswords)
                        {
                            if (VerifyHashedPassword(password.PASSWORD, data.NewPassword) || Equals(Encrypt.getEncryptedCode(data.NewPassword), password.PASSWORD))
                            {
                                isValid = false;
                            }
                        }
                        if (isValid)
                        {
                            _user.PasswordHash = data.PasswordHash;
                            _user.PASSWORD = Encrypt.getEncryptedCode(data.NewPassword);
                            _user.MODIFIED_BY = _user.USER_NAME;
                            _user.PASSWORD_RESET_TICKS = null;
                            _user.PASSWORD_CHANGED_DATE = Helper.GetCurrentDate();
                            _user.MODIFIED_DATE = Helper.GetCurrentDate();
                            _UserRepository.Update(_user);
                            _UserRepository.Save();
                            PasswordHistory ph = new PasswordHistory()
                            {
                                PASSWORD_ID = Helper.getMaximumId("PASSWORD_ID"),
                                DELETED = false,
                                USER_ID = _user.USER_ID,
                                PASSWORD = data.PasswordHash,
                                CREATED_BY = _user.USER_NAME,
                                CREATED_DATE = Helper.GetCurrentDate(),
                                MODIFIED_BY = _user.USER_NAME,
                                MODIFIED_DATE = Helper.GetCurrentDate(),
                                PRACTICE_CODE = _user.PRACTICE_CODE
                            };
                            _passwordHistoryRepository.Insert(ph);
                            _passwordHistoryRepository.Save();
                            //1 = Success
                            return 1;
                        }
                        else
                        {
                            // -1 Please use different password.
                            return -1;
                        }
                    }
                    //-2 link has been expired.
                    return -2;
                }
                //0 = User doesn't exist
                return 0;
            }
            catch (Exception)
            {
                return 0;
            }
        }
        public static bool VerifyHashedPassword(string hashedPassword, string password)
        {
            byte[] buffer4;
            if (hashedPassword == null)
            {
                return false;
            }
            if (password == null)
            {
                throw new ArgumentNullException("password");
            }
            byte[] src = Convert.FromBase64String(hashedPassword);
            if ((src.Length != 0x31) || (src[0] != 0))
            {
                return false;
            }
            byte[] dst = new byte[0x10];
            Buffer.BlockCopy(src, 1, dst, 0, 0x10);
            byte[] buffer3 = new byte[0x20];
            Buffer.BlockCopy(src, 0x11, buffer3, 0, 0x20);
            using (Rfc2898DeriveBytes bytes = new Rfc2898DeriveBytes(password, dst, 0x3e8))
            {
                buffer4 = bytes.GetBytes(0x20);
            }
            return buffer3.SequenceEqual(buffer4);
        }
        public List<FileRecieverResult> UploadSignatures(HttpFileCollection fileAttachments, string username, UserProfile profile)
        {
            List<FileRecieverResult> FileRecieverResultList = new List<FileRecieverResult>();
            string pathToWriteTiffImage = "";
            string directoryPath = "";
            string fileName = "";
            if (fileAttachments.Count > 0)
            {
                try
                {
                    for (int i = 0; i < fileAttachments.Count; i++)
                    {
                        if (fileAttachments[i] != null && fileAttachments[i].ContentLength > 0)
                        {
                            FileRecieverResult result = new FileRecieverResult();
                            var fileExtension = Path.GetExtension(fileAttachments[i].FileName);
                            if (fileExtension == ".jpeg" || fileExtension == ".jpg" || fileExtension == ".png" || fileExtension == ".tiff" || fileExtension == ".tif")
                            {
                                if (fileExtension.ToLower() == ".tif" || fileExtension.ToLower() == ".tiff")
                                {
                                    string directoryPathForTiff = HttpContext.Current.Server.MapPath(@"~/" + profile.PracticeDocumentDirectory + "/" + "Fox/Signatures/OriginalTiffImages/");
                                    if (!Directory.Exists(directoryPathForTiff))
                                    {
                                        Directory.CreateDirectory(directoryPathForTiff);
                                    }
                                    string fileNameForTiff = username + "_tif_" + DateTime.Now.ToString("ddMMyyyHHmmssffff") + fileExtension;
                                    pathToWriteTiffImage = directoryPathForTiff + "\\" + fileNameForTiff;
                                    fileAttachments[i].SaveAs(pathToWriteTiffImage);
                                }

                                directoryPath = HttpContext.Current.Server.MapPath(@"~/" + profile.PracticeDocumentDirectory + "/" + "Fox/Signatures/");
                                fileName = DocumentHelper.GenerateSignatureFileName(username);

                                if (!Directory.Exists(directoryPath))
                                {
                                    Directory.CreateDirectory(directoryPath);
                                }

                                var pathtowriteimage = directoryPath + "\\" + fileName;
                                if (fileExtension.ToLower() == ".tif" || fileExtension.ToLower() == ".tiff")
                                {
                                    fileExtension = ".png";
                                    System.Drawing.Bitmap.FromFile(pathToWriteTiffImage).Save(pathtowriteimage + fileExtension, System.Drawing.Imaging.ImageFormat.Png);
                                }
                                else
                                {
                                    fileAttachments[i].SaveAs(pathtowriteimage + fileExtension);
                                }
                                var pathtobesaveindb = profile.PracticeDocumentDirectory + "\\Fox\\Signatures\\" + fileName + fileExtension;
                                var savedindb = SaveSignaturesInDB(pathtobesaveindb, username, profile);
                                if (savedindb)
                                {
                                    result.FilePath = pathtobesaveindb;
                                    result.FileName = fileName;
                                    FileRecieverResultList.Add(result);
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    FileRecieverResultList = new List<FileRecieverResult>();
                }
            }
            return FileRecieverResultList;
        }
        public bool SaveSignaturesInDB(string path, string username, UserProfile profile)
        {
            try
            {
                if (!string.IsNullOrEmpty(username))
                {
                    var user = _UserRepository.Get(x => x.USER_NAME.Equals(username));
                    if (user != null)
                    {
                        user.SIGNATURE_PATH = path;
                        user.MODIFIED_BY = profile.UserName;
                        user.MODIFIED_DATE = Helper.GetCurrentDate();
                        _UserRepository.Update(user);
                        _UserRepository.Save();
                        return true;
                    }

                }
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public bool SavePasswordResetTicks(string ticks, string email)
        {
            var user = _UserRepository.Get(x => x.EMAIL.ToLower().Equals(email.ToLower()));
            try
            {
                user.PASSWORD_RESET_TICKS = ticks;
                _UserRepository.Update(user);
                _UserRepository.Save();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public string GetEmailByTick(string ticks)
        {
            var user = _UserRepository.Get(x => x.PASSWORD_RESET_TICKS == ticks);
            if (user != null)
                return user.EMAIL;
            return null;
        }
        public List<SmartIdentifierRes> GetSmartIdentifier(SmartSearchRequest obj, UserProfile Profile)
        {
            try
            {
                if (obj.SEARCHVALUE == null)
                    obj.SEARCHVALUE = "";
                var pRACTICE_CODE = new SqlParameter("PRACTICE_CODE", SqlDbType.BigInt) { Value = Profile.PracticeCode };
                var tYPE = new SqlParameter("TYPE", SqlDbType.VarChar) { Value = obj.TYPE };
                var smartvalue = new SqlParameter("SEARCHVALUE", SqlDbType.VarChar) { Value = obj.SEARCHVALUE };
                var result = SpRepository<SmartIdentifierRes>.GetListWithStoreProcedure(@"exec [FOX_GET_IDENTIFIER] @PRACTICE_CODE, @TYPE,@SEARCHVALUE",
                    pRACTICE_CODE, tYPE, smartvalue).ToList();
                if (result.Any())
                    return result;
                else
                    return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public SmartSpecialitySearchResponseModel getSmartSpecialities(SmartSearchRequest model, UserProfile Profile)
        {
            try
            {
                var specialitiesList = _speciality.GetMany(i => !i.DELETED && i.PRACTICE_CODE == Profile.PracticeCode && i.NAME.StartsWith(model.Keyword)).OrderBy(i => i.NAME).ToList();
                if (specialitiesList.Count > 0)
                {
                    return new SmartSpecialitySearchResponseModel() { specialities = specialitiesList, ErrorMessage = "", Message = "Specialties retrived successfully", Success = true };
                }
                else
                {
                    return new SmartSpecialitySearchResponseModel() { specialities = null, ErrorMessage = "No Specialties exist", Message = "No Specialties exist", Success = false };
                }
            }
            catch (Exception ex)
            {
                return new SmartSpecialitySearchResponseModel() { specialities = null, ErrorMessage = ex.ToString(), Message = ex.Message, Success = false };
            }
        }
        public FoxTBLPracticeOrganizationResponseModel getPractices(SmartSearchRequest model, UserProfile Profile)
        {
            try
            {
                var practicesList = _DbContextCommon.PracticeOrganizations.Where(t => !t.DELETED && t.PRACTICE_CODE == Profile.PracticeCode && t.NAME.StartsWith(model.Keyword)).OrderBy(t => t.NAME).ToList();
                if (practicesList.Count > 0)
                {
                    return new FoxTBLPracticeOrganizationResponseModel() { fox_tbl_practice_organization = practicesList, ErrorMessage = "", Message = "Practices retrived successfully", Success = true };
                }
                else
                {
                    return new FoxTBLPracticeOrganizationResponseModel() { fox_tbl_practice_organization = null, ErrorMessage = "No practice exist", Message = "No practice exist", Success = false };
                }
            }
            catch (Exception ex)
            {
                return new FoxTBLPracticeOrganizationResponseModel() { fox_tbl_practice_organization = null, ErrorMessage = ex.Message, Message = ex.Message, Success = false };
            }
        }
        public void AddUpdateUserExtension(long userId, string extension, bool? isActive)
        {
            var dbUsr = _UserRepository.GetByID(userId);
            dbUsr.EXTENSION = extension;
            dbUsr.IS_ACTIVE_EXTENSION = isActive;
            _UserRepository.Update(dbUsr);
            _UserRepository.Save();

        }
        public UserProfile UpdateProfile(string uSER_NAME)
        {
            //update profile
            var usrParmAuth = new SqlParameter("UserName", SqlDbType.VarChar) { Value = uSER_NAME };
            return SpRepository<UserProfile>.GetListWithStoreProcedure(@"exec FOX_PROC_GET_USER_PROFILING_DATA @UserName", usrParmAuth).FirstOrDefault();
        }
        public void AddUpdateUserAdditionalInfo(long UserId, bool isElectronicPOC,DateTime CreatedDate,string CreatedBy, DateTime ModifiedDate,string ModifiedBy,bool Deleted)
        {
            bool isEdit = true;
            var userAddInfo = _userAdditionalInfoRepository.GetFirst(e => e.FOX_TBL_APPLICATION_USER_ID == UserId && !e.DELETED);
            if (userAddInfo == null)
            {
                isEdit = false;
                userAddInfo = new FOX_TBL_APP_USER_ADDITIONAL_INFO();
                userAddInfo.FOX_USER_ADDITIONAL_INFO_ID = Helper.getMaximumId("FOX_USER_ADDITIONAL_INFO_ID");
                userAddInfo.CREATED_BY = CreatedBy;
                userAddInfo.CREATED_DATE = CreatedDate;
            }
            userAddInfo.FOX_TBL_APPLICATION_USER_ID = UserId;
            userAddInfo.IS_ELECTRONIC_POC = isElectronicPOC;
            userAddInfo.MODIFIED_BY = ModifiedBy;
            userAddInfo.MODIFIED_DATE = ModifiedDate;
            userAddInfo.DELETED = Deleted;
            if (isEdit)
            {
                _userAdditionalInfoRepository.Update(userAddInfo);
            }
            else {
                _userAdditionalInfoRepository.Insert(userAddInfo);
            }

            _userAdditionalInfoRepository.Save();
            //string query = @"exec SP_FOX_TBL_APP_USER_ADDITIONAL_INFO @USER_Id,@Is_Electronic_POC,@CREATED_DATE,@CREATED_BY,@MODIFIED_DATE,@MODIFIED_BY,@DELETED";
            ////params SqlParameter[] parameters = new 
            ////long id = Helper.getMaximumId("FOX_TBL_APPLICATION_USER_Id");
            ////SqlParameter Id = new SqlParameter("Id", SqlDbType.BigInt) { Value = id };
            //SqlParameter FOX_TBL_APPLICATION_USER_Id = new SqlParameter("@USER_Id", SqlDbType.BigInt) { Value = UserId };
            //SqlParameter Is_Electronic_POC = new SqlParameter("@Is_Electronic_POC", SqlDbType.Bit) { Value = isElectronicPOC };
            //SqlParameter CREATED_DATE = new SqlParameter("@CREATED_DATE", SqlDbType.DateTime) { Value = CreatedDate };
            //SqlParameter CREATED_BY = new SqlParameter("@CREATED_BY", SqlDbType.VarChar) { Value = CreatedBy };
            //SqlParameter MODIFIED_DATE = new SqlParameter("@MODIFIED_DATE", SqlDbType.DateTime) { Value = ModifiedDate };
            //SqlParameter MODIFIED_BY = new SqlParameter("@MODIFIED_BY", SqlDbType.VarChar) { Value = ModifiedBy };
            //SqlParameter DELETED = new SqlParameter("@DELETED", SqlDbType.Bit) { Value = Deleted };
            ////return  context.Database.ExecuteSqlCommand(query,list);
            //var result = SpRepository<object>.GetSingleObjectWithStoreProcedure(query, FOX_TBL_APPLICATION_USER_Id, Is_Electronic_POC, CREATED_DATE,
            //CREATED_BY, MODIFIED_DATE, MODIFIED_BY, DELETED);
            //return 1;
        }
        public void AddUpdateReferralSourceInfo(string userName, UserProfile profile)
        {
            try
            {
                bool isEdit = false;
                Referral_Physicians mtbcPhy = new Referral_Physicians();
                var user = _UserRepository.GetFirst(e => e.USER_NAME == userName);
                if (user != null)
                {

                    if (user.USR_REFERRAL_SOURCE_ID != null && user.USR_REFERRAL_SOURCE_ID != 0)
                    {
                        isEdit = true;
                    }
                    var referralSource = new ReferralSource();
                    var dbrefSrc = _ReferralSourceRepository.GetFirst(e => e.SOURCE_ID == user.USR_REFERRAL_SOURCE_ID && !e.DELETED);
                    if (dbrefSrc == null)
                    {
                        //long REFERRAL_CODE = InsertUpdateReferralPhy(referralSource, profile);
                        isEdit = false;
                        referralSource = new ReferralSource();
                        //By Johar removed Idenetity
                        referralSource.SOURCE_ID = Helper.getMaximumId("FOX_SOURCE_ID");
                        referralSource.PRACTICE_CODE = user.PRACTICE_CODE;
                        //referralSource.REFERRAL_CODE = REFERRAL_CODE;
                        referralSource.Email = user.EMAIL;
                        referralSource.CREATED_BY = "FOX TEAM";
                        referralSource.CREATED_DATE = Helper.GetCurrentDate();
                        referralSource.MODIFIED_BY = "FOX TEAM";
                        referralSource.MODIFIED_DATE = Helper.GetCurrentDate();
                        referralSource.DELETED = false;
                    }
                    else
                    {
                        //InsertUpdateReferralPhy(referralSource, profile);
                        referralSource = dbrefSrc;
                        referralSource.MODIFIED_BY = profile.UserName;
                        referralSource.MODIFIED_DATE = Helper.GetCurrentDate();
                        referralSource.Email = user.EMAIL;
                    }

                    referralSource.FIRST_NAME = user.FIRST_NAME;
                    referralSource.LAST_NAME = user.LAST_NAME;
                    referralSource.ADDRESS = user.ADDRESS_1;
                    referralSource.ADDRESS_2 = user.ADDRESS_2;
                    referralSource.ZIP = user.ZIP;
                    referralSource.CITY = user.CITY;
                    referralSource.STATE = user.STATE;
                    referralSource.PHONE = user.MOBILE_PHONE;
                    referralSource.FAX = user.FAX;
                    referralSource.NPI = user.NPI;
                    referralSource.PRACTICE_ORGANIZATION_ID = user.PRACTICE_ORGANIZATION_ID;
                    referralSource.ACO_ID = user.ACO;
                    ////Add name of ACO to existing names.
                    //if (isEdit && referralSource.ACO_ID != user.ACO)
                    //{
                    //    if (user.ACO.HasValue && user.ACO.Value != 0)
                    //    {
                    //        var acoData = _fox_tbl_identifier.GetFirst(e => e.IDENTIFIER_ID == user.ACO && !e.DELETED);
                    //        if (acoData != null)
                    //        {
                    //            referralSource.ACO = acoData.NAME;
                    //        }
                    //    }
                    //}
                    if (isEdit)
                        _ReferralSourceRepository.Update(referralSource);
                    else
                        _ReferralSourceRepository.Insert(referralSource);
                    _ReferralSourceRepository.Save();

                    user.USR_REFERRAL_SOURCE_ID = referralSource.SOURCE_ID;
                    _UserRepository.Update(user);
                    _UserRepository.Save();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void SaveSenderName(dynamic user, UserProfile profile)
        {
            string userName = user.USER_NAME;
            var senderName = _FOX_TBL_SENDER_NAME.GetFirst(t => t.SENDER_NAME_CODE.Equals(userName));
            if (senderName == null)
            {
                senderName = new DataModels.Models.SenderName.FOX_TBL_SENDER_NAME();
                senderName.FOX_TBL_SENDER_NAME_ID = Helper.getMaximumId("FOX_TBL_SENDER_NAME_ID");
                senderName.FOX_TBL_SENDER_TYPE_ID = null;
                senderName.PRACTICE_CODE = profile.PracticeCode;
                senderName.SENDER_NAME_CODE = user.USER_NAME;
                senderName.SENDER_NAME_DESCRIPTION = user.USER_DISPLAY_NAME;
                senderName.CREATED_BY = senderName.MODIFIED_BY = profile.UserName;
                senderName.CREATED_DATE = senderName.MODIFIED_DATE = Helper.GetCurrentDate();
                senderName.DELETED = false;

                _FOX_TBL_SENDER_NAME.Insert(senderName);
                _FOX_TBL_SENDER_NAME.Save();
            }
            if (user.ROLE_ID == 101 && user.IS_ACTIVE ==true)
            {
                var ActiveIUser = _ActiveIndexerRepository.GetFirst(t => t.INDEXER.Equals(userName));
                if (ActiveIUser == null)
                {
                    ActiveIUser = new ActiveIndexer();
                    ActiveIUser.INDEXER = user.USER_NAME;
                    ActiveIUser.DEFAULT_VALUE = "Regular Indexer";
                    ActiveIUser.IS_ACTIVE = false;
                    ActiveIUser.LAST_NAME = user.LAST_NAME;
                    ActiveIUser.MODIFIED_BY = profile.UserName;
                    ActiveIUser.PRACTICE_CODE = user.PRACTICE_CODE;
                    ActiveIUser.CREATED_BY = user.CREATED_BY;
                    ActiveIUser.CREATED_DATE = Helper.GetCurrentDate();
                    ActiveIUser.MODIFIED_DATE = Helper.GetCurrentDate();
                    ActiveIUser.ACTIVE_INDEXER_ID = Helper.getMaximumId("ACTIVE_INDEXER_ID");
                    _ActiveIndexerRepository.Insert(ActiveIUser);
                    _ActiveIndexerRepository.Save();
                }
            }
        }
        public void SaveSenderName(dynamic user, long practiceCode)
        {
            string userName = user.USER_NAME;
            var senderName = _FOX_TBL_SENDER_NAME.GetFirst(t => t.SENDER_NAME_CODE.Equals(userName));
            if (senderName == null)
            {
                senderName = new DataModels.Models.SenderName.FOX_TBL_SENDER_NAME();
                senderName.FOX_TBL_SENDER_NAME_ID = Helper.getMaximumId("FOX_TBL_SENDER_NAME_ID");
                senderName.FOX_TBL_SENDER_TYPE_ID = null;
                senderName.PRACTICE_CODE = practiceCode;
                senderName.SENDER_NAME_CODE = user.USER_NAME;
                senderName.SENDER_NAME_DESCRIPTION = user.USER_DISPLAY_NAME;
                senderName.CREATED_BY = senderName.MODIFIED_BY = user.UserName;
                senderName.CREATED_DATE = senderName.MODIFIED_DATE = Helper.GetCurrentDate();
                senderName.DELETED = false;

                _FOX_TBL_SENDER_NAME.Insert(senderName);
                _FOX_TBL_SENDER_NAME.Save();
            }
        }
        public void SavePasswordHisotry(dynamic user, UserProfile profile)
        {
            PasswordHistory ph = new PasswordHistory()
            {
                PASSWORD_ID = Helper.getMaximumId("PASSWORD_ID"),
                DELETED = false,
                USER_ID = user.USER_ID,
                PASSWORD = user.PasswordHash,
                CREATED_BY = profile.UserName,
                CREATED_DATE = Helper.GetCurrentDate(),
                MODIFIED_BY = profile.UserName,
                MODIFIED_DATE = Helper.GetCurrentDate(),
                PRACTICE_CODE = user.PRACTICE_CODE
            };
            _passwordHistoryRepository.Insert(ph);
            _passwordHistoryRepository.Save();
        }
        public void SavePasswordHisotry(dynamic user, long practiceCode)
        {
            PasswordHistory ph = new PasswordHistory()
            {
                PASSWORD_ID = Helper.getMaximumId("PASSWORD_ID"),
                DELETED = false,
                USER_ID = user.USER_ID,
                PASSWORD = user.PasswordHash,
                CREATED_BY = user.UserName,
                CREATED_DATE = Helper.GetCurrentDate(),
                MODIFIED_BY = user.UserName,
                MODIFIED_DATE = Helper.GetCurrentDate(),
                PRACTICE_CODE = user.PRACTICE_CODE
            };
            _passwordHistoryRepository.Insert(ph);
            _passwordHistoryRepository.Save();
        }
        public UserProfile GetUserProfileByName(string userName)
        {
            var usrParmAuth = new SqlParameter("UserName", SqlDbType.VarChar) { Value = userName };
            return SpRepository<UserProfile>.GetListWithStoreProcedure(@"exec FOX_PROC_GET_USER_PROFILING_DATA @UserName", usrParmAuth).FirstOrDefault();
        }
        public User IsUserAlreadyExist(long userId)
        {
            return _UserRepository.GetByID(userId);
        }
        public bool UpdateUser(User userToUpdate, dynamic user, UserProfile profile)
        {
            //if (string.IsNullOrWhiteSpace(user.SecurityStamp))
            //{
            //    userToUpdate.SecurityStamp = "EF7A457F-BD28-45C2-AD12-4AD93BEE540A";
            //}
            userToUpdate.ACTIVATION_CODE = user.ACTIVATION_CODE;
            userToUpdate.ADDRESS_1 = user.ADDRESS_1;
            userToUpdate.ADDRESS_2 = user.ADDRESS_2;
            userToUpdate.CITY = user.CITY;
            userToUpdate.COMMENTS = user.COMMENTS;
            userToUpdate.DATE_OF_BIRTH = user.DATE_OF_BIRTH;
            userToUpdate.DELETED = user.DELETED;
            userToUpdate.DESIGNATION = user.DESIGNATION;
            userToUpdate.EMAIL = user.EMAIL;
            userToUpdate.FAILED_PASSWORD_ATTEMPT_COUNT = user.FAILED_PASSWORD_ATTEMPT_COUNT;
            userToUpdate.FIRST_NAME = user.FIRST_NAME;
            userToUpdate.Is_Electronic_POC = user.Is_Electronic_POC;
            if (userToUpdate.IS_ACTIVE && !user.IS_ACTIVE)
            {
                userToUpdate.TERMINATION_DATE = Helper.GetCurrentDate();
            }
            else
            {
                userToUpdate.TERMINATION_DATE = null;
            }
            userToUpdate.IS_ACTIVE = user.IS_ACTIVE;
            userToUpdate.IS_ADMIN = user.IS_ADMIN;
            //  usr.IS_LOCKED_OUT = user.IS_LOCKED_OUT;
            //   usr.LAST_LOGIN_DATE = user.LAST_LOGIN_DATE;
            userToUpdate.LAST_NAME = user.LAST_NAME.Trim();
            //    usr.LOCKEDBY = user.LOCKEDBY;
            userToUpdate.MIDDLE_NAME = user.MIDDLE_NAME?.Trim();
            userToUpdate.PRACTICE_CODE = profile.PracticeCode;
            userToUpdate.RESET_PASS = user.RESET_PASS;
            userToUpdate.ROLE_ID = user.ROLE_ID;
            userToUpdate.SECURITY_QUESTION = user.SECURITY_QUESTION;
            userToUpdate.SECURITY_QUESTION_ANSWER = user.SECURITY_QUESTION_ANSWER;
            userToUpdate.STATE = user.STATE;
            userToUpdate.STATUS = user.STATUS;
            userToUpdate.USER_DISPLAY_NAME = user.USER_DISPLAY_NAME;
            userToUpdate.USER_ID = user.USER_ID;
            userToUpdate.USER_NAME = user.USER_NAME;
            userToUpdate.ZIP = user.ZIP;
            userToUpdate.MODIFIED_BY = profile.UserName;
            userToUpdate.MODIFIED_DATE = Helper.GetCurrentDate();
            userToUpdate.MANAGER_ID = user.MANAGER_ID;
            userToUpdate.USER_TYPE = user.USER_TYPE;
            //usr.PRACTICE_NAME = user.PRACTICE_NAME;
            userToUpdate.NPI = user.NPI;
            userToUpdate.MOBILE_PHONE = user.MOBILE_PHONE;
            userToUpdate.PHONE_NO = user.PHONE_NO;
            userToUpdate.FAX = user.FAX;
            userToUpdate.FAX_2 = user.FAX_2;
            userToUpdate.FAX_3 = user.FAX_3;
            userToUpdate.GENDER = user.GENDER;
            userToUpdate.LANGUAGE = user.LANGUAGE;
            userToUpdate.TIME_ZONE = user.TIME_ZONE;
            userToUpdate.SIGNATURE_PATH = userToUpdate.SIGNATURE_PATH;
            userToUpdate.FOX_TBL_SENDER_TYPE_ID = user.FOX_TBL_SENDER_TYPE_ID;
            userToUpdate.ACO = user.ACO;
            userToUpdate.SNF = user.SNF;
            userToUpdate.HHH = user.HHH;
            userToUpdate.PRACTICE_ORGANIZATION_ID = user.PRACTICE_ORGANIZATION_ID;
            userToUpdate.HOSPITAL = user.HOSPITAL;
            userToUpdate.SPECIALITY = user.SPECIALITY;
            userToUpdate.COMMENTS = user.COMMENTS;
            userToUpdate.ACO_TEXT = user.ACO_TEXT;
            userToUpdate.SPECIALITY_TEXT = user.SPECIALITY_TEXT;
            userToUpdate.HOSPITAL_TEXT = user.HOSPITAL_TEXT;
            userToUpdate.HHH_TEXT = user.HHH_TEXT;
            userToUpdate.PRACTICE_ORGANIZATION_TEXT = user.PRACTICE_ORGANIZATION_TEXT;
            userToUpdate.SNF_TEXT = user.SNF_TEXT;
            //usr.SENDER_TYPE = user.SENDER_TYPE;
            userToUpdate.IS_APPROVED = user.IS_APPROVED;
            userToUpdate.REFERRAL_REGION_ID = user.REFERRAL_REGION_ID;
            userToUpdate.FULL_ACCESS_OVER_APP = user.FULL_ACCESS_OVER_APP;

            AddUpdateUserAdditionalInfo(userToUpdate.USER_ID, userToUpdate.Is_Electronic_POC == null ? false : userToUpdate.Is_Electronic_POC.Value, userToUpdate.CREATED_DATE, userToUpdate.CREATED_BY, userToUpdate.MODIFIED_DATE, userToUpdate.MODIFIED_BY, userToUpdate.DELETED);

            try
            {
                _UserRepository.Update(userToUpdate);
                _UserRepository.Save();
                UnblockUser(userToUpdate);
                #region Send Email In Case of Approved External User
                //if (user.hasToSendEmail)
                //{
                //    #region email to user on approva;

                //    string body = string.Empty;
                //    string templatePathOfAdminEmail = HttpContext.Current.Server.MapPath("~/HtmlTemplates/external_user_email_template_on_request_approved.html");
                //    if (File.Exists(templatePathOfAdminEmail))
                //    {
                //        body = File.ReadAllText(templatePathOfAdminEmail);
                //        body = body.Replace("[[FIRST_NAME]]", user.FIRST_NAME);
                //        body = body.Replace("[[LAST_NAME]]", user.LAST_NAME);
                //    }
                //    string subject = "Signup approved for FOX Rehab portal";
                //    string sendTo = user.EMAIL;
                //    //string sendTo = "usmanfarooq@mtbc.com";
                //    //string sendTo = "asimshah4@mtbc.com";
                //    List<string> _bccList = new List<string>();
                //    //Helper.SendEmail(sendTo, subject, body, null, _bccList, "noreply@mtbc.com");
                //    //Helper.SendEmail(sendTo, subject, body, null, profile, null, _bccList);

                //    #endregion
                //}
                #endregion
                #region sms to user on approval
                string recipient = "";
                //string smsBody = @"Your signup request for Fox Rehab portal has been approved. You may now login to your account and start sending a patient referral right away! Download on the App Store: https://itunes.apple.com/us/app/mtbc-fox/id1384823410?mt=8";
                //string smsBody = @"Your signup request for Fox Rehab portal has been approved. You may now login to your account and start sending patient referral right away! Download our iOS App from App Store: https://itunes.apple.com/us/app/mtbc-fox/id1384823410?mt=8 and Android App from Play Store: https://play.google.com/store/apps/details?id=com.mtbc.fox";
                string smsBody = @"";
                if (user.MOBILE_PHONE != null)
                {
                    recipient = user.MOBILE_PHONE;
                    //smsBody = @"Dear "+userToUpdate.LAST_NAME+", "+userToUpdate.FIRST_NAME+"\nYour request for access to Fox Rehabilitation's online referral portal has been approved.\nFox Rehabilitation,\nClient Services Team";
                    smsBody = @"Dear " + userToUpdate.LAST_NAME + ", " + userToUpdate.FIRST_NAME + "\nYour request for access to Fox Rehabilitation online referral portal has been approved.\nFox Rehabilitation,\nClient Services Team";
                    SmsService.NJSmsService(recipient, smsBody);

                }
                #endregion
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public List<User> GetSmartUsersOfSpecificRoleName(string searchText, string roleName, UserProfile userProfile)
        {
            var _paramsPracticeCode = new SqlParameter("PRACTICE_CODE", SqlDbType.BigInt) { Value = userProfile.PracticeCode };
            var _searchText = new SqlParameter("SEARCH_TEXT", SqlDbType.VarChar) { Value = searchText };
            var _roleName = new SqlParameter("ROLE_NAME", SqlDbType.VarChar) { Value = roleName };
            return SpRepository<User>.GetListWithStoreProcedure(@"Exec [FOX_PROC_GET_SMART_USERS_OF_SPECIFIC_ROLE_BY_ROLE_NAME] @PRACTICE_CODE, @SEARCH_TEXT, @ROLE_NAME", _paramsPracticeCode, _searchText, _roleName);
        }
        public List<User> GetAlternateAccountManger(string roleName, UserProfile userProfile)
        {
            var _paramsPracticeCode = new SqlParameter("PRACTICE_CODE", SqlDbType.BigInt) { Value = userProfile.PracticeCode };
            var _roleName = new SqlParameter("ROLE_NAME", SqlDbType.VarChar) { Value = roleName };
            var mod = SpRepository<User>.GetListWithStoreProcedure(@"Exec [FOX_PROC_GET_SPECIFIC_ROLE_BY_ROLE_NAME] @PRACTICE_CODE, @ROLE_NAME", _paramsPracticeCode, _roleName);
            return mod;
        }
        public List<States> GetSmartStates(string searchText)
        {
            var _searchText = new SqlParameter("SEARCH_TEXT", SqlDbType.VarChar) { Value = searchText };
            return SpRepository<States>.GetListWithStoreProcedure(@"Exec [FOX_PROC_GET_SMART_STATES] @SEARCH_TEXT", _searchText);
        }
        public bool CheckForAtleastOneRight(long roleId, long practiceCode)
        {
            var _paramsRoleId = new SqlParameter("ROLE_ID", SqlDbType.BigInt) { Value = roleId };
            var _paramsPracticeCode = new SqlParameter("PRACTICE_CODE", SqlDbType.BigInt) { Value = practiceCode };
            return SpRepository<dynamic>.GetListWithStoreProcedure(@"EXEC [FOX_PROC_GET_CHECKED_RIGHTS_OF_ROLE] @PRACTICE_CODE,@ROLE_ID", _paramsPracticeCode, _paramsRoleId).Count() == 1 ? false : true;
        }
        //public long InsertUpdateReferralPhy(ReferralSource OrsObj, UserProfile profile)
        //{
        //    Referral_Physicians mtbcPhy = new Referral_Physicians();
        //    var dbReferralPhysn = _referral_physiciansRepository.GetFirst(t => t.REFERRAL_CODE == OrsObj.REFERRAL_CODE && !(t.DELETED ?? false));
        //    if (dbReferralPhysn != null)
        //    {
        //        dbReferralPhysn.REFERRAL_FNAME = OrsObj.FIRST_NAME;
        //        dbReferralPhysn.REFERRAL_LNAME = OrsObj.LAST_NAME;
        //        dbReferralPhysn.title = OrsObj.TITLE;
        //        dbReferralPhysn.REFERRAL_ADDRESS = OrsObj.ADDRESS;
        //        dbReferralPhysn.REFERRAL_CITY = OrsObj.CITY;
        //        dbReferralPhysn.REFERRAL_STATE = OrsObj.STATE;
        //        dbReferralPhysn.REFERRAL_ZIP = OrsObj.ZIP;
        //        dbReferralPhysn.REFERRAL_PHONE = OrsObj.PHONE;
        //        dbReferralPhysn.REFERRAL_FAX = OrsObj.FAX;
        //        dbReferralPhysn.NPI = OrsObj.NPI;
        //        if (OrsObj.INACTIVE_REASON_ID != null && OrsObj.INACTIVE_REASON_ID != 0)
        //        {
        //            dbReferralPhysn.IN_ACTIVE = true;
        //        }
        //        else
        //        {
        //            dbReferralPhysn.IN_ACTIVE = false;
        //        }

        //        dbReferralPhysn.Sync_Date = Helper.GetCurrentDate();
        //        dbReferralPhysn.MODIFIED_DATE = Helper.GetCurrentDate();
        //        dbReferralPhysn.MODIFIED_BY = profile.UserName;
        //        _referral_physiciansRepository.Update(dbReferralPhysn);
        //        _referral_physiciansRepository.Save();
        //        return dbReferralPhysn.REFERRAL_CODE;
        //    }
        //    else
        //    {
        //        mtbcPhy.REFERRAL_CODE = Helper.getMaximumId("REFERRAL_CODE");
        //        mtbcPhy.REFERRAL_FNAME = OrsObj.FIRST_NAME;
        //        mtbcPhy.REFERRAL_LNAME = OrsObj.LAST_NAME;
        //        mtbcPhy.title = OrsObj.TITLE;
        //        mtbcPhy.REFERRAL_ADDRESS = OrsObj.ADDRESS;
        //        mtbcPhy.REFERRAL_CITY = OrsObj.CITY;
        //        mtbcPhy.REFERRAL_STATE = OrsObj.STATE;
        //        mtbcPhy.REFERRAL_ZIP = OrsObj.ZIP;
        //        mtbcPhy.REFERRAL_PHONE = OrsObj.PHONE;
        //        mtbcPhy.REFERRAL_FAX = OrsObj.FAX;
        //        mtbcPhy.NPI = OrsObj.NPI;
        //        if (OrsObj.INACTIVE_REASON_ID != null && OrsObj.INACTIVE_REASON_ID != 0)
        //        {
        //            mtbcPhy.IN_ACTIVE = true;
        //        }
        //        else
        //        {
        //            mtbcPhy.IN_ACTIVE = false;

        //        }
        //        mtbcPhy.Sync_Date = Helper.GetCurrentDate();
        //        mtbcPhy.CREATED_DATE = Helper.GetCurrentDate();
        //        mtbcPhy.CREATED_BY = profile.UserName;
        //        _referral_physiciansRepository.Insert(mtbcPhy);
        //        _referral_physiciansRepository.Save();
        //        return mtbcPhy.REFERRAL_CODE;
        //    }


        //}
        public void SaveRegionalDirectorID(dynamic user, ReferralRegion referralRegion)
        {
            if (referralRegion.REGIONAL_DIRECTOR_ID == null || referralRegion.REGIONAL_DIRECTOR_ID == 0 || referralRegion.REGIONAL_DIRECTOR_ID == -1)
            {
                referralRegion.REGIONAL_DIRECTOR_ID = user.USER_ID;
                referralRegion.MODIFIED_BY = "FOX_AD";
                referralRegion.MODIFIED_DATE = user.MODIFIED_DATE;
                _ReferralRegionRepository.Update(referralRegion);
                _ReferralRegionRepository.Save();
            }
        }
        public void SaveAccountManagerID(dynamic user, ReferralRegion referralRegion)
        {
            if (referralRegion.ACCOUNT_MANAGER_ID == null || referralRegion.ACCOUNT_MANAGER_ID == 0 || referralRegion.ACCOUNT_MANAGER_ID == -1)
            {
                referralRegion.ACCOUNT_MANAGER_ID = user.USER_ID;
                referralRegion.ACCOUNT_MANAGER_EMAIL = user.EMAIL;
                referralRegion.ACCOUNT_MANAGER = user.FIRST_NAME + " " + user.LAST_NAME;
                referralRegion.MODIFIED_BY = "FOX_AD";
                referralRegion.MODIFIED_DATE = user.MODIFIED_DATE;
                _ReferralRegionRepository.Update(referralRegion);
                _ReferralRegionRepository.Save();
            }
        }
        public long GetRoleId(string RoleName, long practicecode)
        {
            var roleRecord = _RoleRepository.GetFirst(r => r.ROLE_NAME.ToLower() == RoleName && (r.PRACTICE_CODE == practicecode || r.PRACTICE_CODE == null) && !r.DELETED);
            if (roleRecord?.ROLE_ID != null)
            {
                return roleRecord.ROLE_ID;
            }
            return 0;
        }
        //sattar code 04/04/2019 end
        public bool CanUserUpdateUser(UserProfile profile)
        {
            List<RoleAndRights> userRights = GetCurrentUserRights(roleId: profile.RoleId, practiceCode: profile.PracticeCode);

            if (profile.RoleId == 103)
            {
                return true;
            }
            else if (userRights!= null && userRights.Count > 0)
            {
                RoleAndRights updateUserRight = userRights.Find(e => e.RIGHT_NAME.Contains("Create/Modify User"));
                if (updateUserRight == null)
                {
                    return false;
                }
                else
                {
                    return true;
                }

            }
            return false;
        }

        public bool UpdateUser(User userToUpdate, dynamic user, UserProfile profile, bool canUpdateUsers)
        {
            //if (string.IsNullOrWhiteSpace(user.SecurityStamp))
            //{
            //    userToUpdate.SecurityStamp = "EF7A457F-BD28-45C2-AD12-4AD93BEE540A";
            //}
            userToUpdate.ACTIVATION_CODE = user.ACTIVATION_CODE;
            userToUpdate.ADDRESS_1 = user.ADDRESS_1;
            userToUpdate.ADDRESS_2 = user.ADDRESS_2;
            userToUpdate.CITY = user.CITY;
            userToUpdate.COMMENTS = user.COMMENTS;
            userToUpdate.DATE_OF_BIRTH = user.DATE_OF_BIRTH;
            userToUpdate.DELETED = user.DELETED;
            userToUpdate.DESIGNATION = user.DESIGNATION;
            //if (canUpdateUsers)
            //{
            //    userToUpdate.EMAIL = user.EMAIL;
            //}
            userToUpdate.FAILED_PASSWORD_ATTEMPT_COUNT = user.FAILED_PASSWORD_ATTEMPT_COUNT;
            if (canUpdateUsers)
            {
                userToUpdate.FIRST_NAME = user.FIRST_NAME;
            }
            //if (canUpdateUsers || (userToUpdate.IS_ACTIVE && !user.IS_ACTIVE))
            //{
            //    userToUpdate.TERMINATION_DATE = Helper.GetCurrentDate();
            //}
            //else
            //{
            //    userToUpdate.TERMINATION_DATE = null;
            //}
            userToUpdate.IS_ACTIVE = user.IS_ACTIVE;
            if (canUpdateUsers)
            {
                userToUpdate.IS_ADMIN = user.IS_ADMIN;
            }
            //  usr.IS_LOCKED_OUT = user.IS_LOCKED_OUT;
            //   usr.LAST_LOGIN_DATE = user.LAST_LOGIN_DATE;
            if (canUpdateUsers)
            {
                userToUpdate.LAST_NAME = user.LAST_NAME.Trim();
            }
            //    usr.LOCKEDBY = user.LOCKEDBY;
            userToUpdate.MIDDLE_NAME = user.MIDDLE_NAME?.Trim();
            userToUpdate.PRACTICE_CODE = profile.PracticeCode;
            userToUpdate.RESET_PASS = user.RESET_PASS;
            if (canUpdateUsers)
            {
                userToUpdate.ROLE_ID = user.ROLE_ID;
            }
            userToUpdate.SECURITY_QUESTION = user.SECURITY_QUESTION;
            userToUpdate.SECURITY_QUESTION_ANSWER = user.SECURITY_QUESTION_ANSWER;
            userToUpdate.STATE = user.STATE;
            userToUpdate.STATUS = user.STATUS;
            userToUpdate.USER_DISPLAY_NAME = user.USER_DISPLAY_NAME;
            userToUpdate.USER_ID = user.USER_ID;
            userToUpdate.USER_NAME = user.USER_NAME;
            userToUpdate.ZIP = user.ZIP;
            userToUpdate.MODIFIED_BY = profile.UserName;
            userToUpdate.MODIFIED_DATE = Helper.GetCurrentDate();
            userToUpdate.MANAGER_ID = user.MANAGER_ID;
            //if (canUpdateUsers)
            //{
            //    userToUpdate.USER_TYPE = user.USER_TYPE;
            //}
            //usr.PRACTICE_NAME = user.PRACTICE_NAME;
            if (canUpdateUsers)
            {
                userToUpdate.NPI = user.NPI;
            }
            userToUpdate.MOBILE_PHONE = user.MOBILE_PHONE;
            userToUpdate.PHONE_NO = user.PHONE_NO;
            userToUpdate.FAX = user.FAX;
            userToUpdate.FAX_2 = user.FAX_2;
            userToUpdate.FAX_3 = user.FAX_3;
            userToUpdate.GENDER = user.GENDER;
            userToUpdate.LANGUAGE = user.LANGUAGE;
            userToUpdate.TIME_ZONE = user.TIME_ZONE;
            userToUpdate.SIGNATURE_PATH = user.SIGNATURE_PATH;
            if (canUpdateUsers)
            {
                userToUpdate.FOX_TBL_SENDER_TYPE_ID = user.FOX_TBL_SENDER_TYPE_ID;
            }
            userToUpdate.ACO = user.ACO;
            userToUpdate.SNF = user.SNF;
            userToUpdate.HHH = user.HHH;
            userToUpdate.PRACTICE_ORGANIZATION_ID = user.PRACTICE_ORGANIZATION_ID;
            userToUpdate.HOSPITAL = user.HOSPITAL;
            userToUpdate.SPECIALITY = user.SPECIALITY;
            userToUpdate.COMMENTS = user.COMMENTS;
            userToUpdate.ACO_TEXT = user.ACO_TEXT;
            userToUpdate.SPECIALITY_TEXT = user.SPECIALITY_TEXT;
            userToUpdate.HOSPITAL_TEXT = user.HOSPITAL_TEXT;
            userToUpdate.HHH_TEXT = user.HHH_TEXT;
            userToUpdate.PRACTICE_ORGANIZATION_TEXT = user.PRACTICE_ORGANIZATION_TEXT;
            userToUpdate.SNF_TEXT = user.SNF_TEXT;
            var senderName = _FOX_TBL_SENDER_TYPE.GetFirst(t => t.FOX_TBL_SENDER_TYPE_ID == userToUpdate.FOX_TBL_SENDER_TYPE_ID);
            userToUpdate.SENDER_TYPE = senderName.SENDER_TYPE_NAME;
            userToUpdate.IS_APPROVED = user.IS_APPROVED;
            userToUpdate.REFERRAL_REGION_ID = user.REFERRAL_REGION_ID;
            userToUpdate.FULL_ACCESS_OVER_APP = user.FULL_ACCESS_OVER_APP;
            userToUpdate.USER_TYPE = user.USER_TYPE;

            if (user.ROLE_ID == 101)
            {
                var ActiveIUser = _ActiveIndexerRepository.GetFirst(t => t.INDEXER.Equals(userToUpdate.USER_NAME));
                if (ActiveIUser == null)
                {
                    ActiveIUser = new ActiveIndexer();
                    ActiveIUser.INDEXER = user.USER_NAME;
                    ActiveIUser.DEFAULT_VALUE = "Regular Indexer";
                    ActiveIUser.IS_ACTIVE = false;
                    ActiveIUser.MODIFIED_BY = profile.UserName;
                    ActiveIUser.PRACTICE_CODE = user.PRACTICE_CODE;
                    ActiveIUser.CREATED_BY = user.CREATED_BY;
                    ActiveIUser.CREATED_DATE = Helper.GetCurrentDate();
                    ActiveIUser.MODIFIED_DATE = Helper.GetCurrentDate();
                    ActiveIUser.ACTIVE_INDEXER_ID = Helper.getMaximumId("ACTIVE_INDEXER_ID");
                    _ActiveIndexerRepository.Insert(ActiveIUser);
                    _ActiveIndexerRepository.Save();
                }
                if(ActiveIUser != null)
                {

                    ActiveIUser.INDEXER = ActiveIUser.INDEXER;
                    ActiveIUser.DEFAULT_VALUE = ActiveIUser.DEFAULT_VALUE;
                    ActiveIUser.IS_ACTIVE = false;
                    ActiveIUser.MODIFIED_BY = profile.UserName;
                    ActiveIUser.PRACTICE_CODE = user.PRACTICE_CODE;
                    ActiveIUser.CREATED_BY = ActiveIUser.CREATED_BY;
                    ActiveIUser.CREATED_DATE = ActiveIUser.CREATED_DATE;
                    ActiveIUser.MODIFIED_DATE = Helper.GetCurrentDate();
                    ActiveIUser.DELETED = false;
                    ActiveIUser.ACTIVE_INDEXER_ID = ActiveIUser.ACTIVE_INDEXER_ID;
                    _ActiveIndexerRepository.Update(ActiveIUser);
                    _ActiveIndexerRepository.Save();
                }
                if (ActiveIUser != null && user.IS_ACTIVE == false)
                {
                    ActiveIUser.DELETED = true;
                }
            }
            try
            {
                _UserRepository.Update(userToUpdate);
                _UserRepository.Save();
                //when user is is approved, then check that if it already blocked, if blocked then unblock it
                UnblockUser(userToUpdate);
                #region Send Email In Case of Approved External User
                //if (user.hasToSendEmail)
                //{
                //    #region email to user on approva;

                //    string body = string.Empty;
                //    string templatePathOfAdminEmail = HttpContext.Current.Server.MapPath("~/HtmlTemplates/external_user_email_template_on_request_approved.html");
                //    if (File.Exists(templatePathOfAdminEmail))
                //    {
                //        body = File.ReadAllText(templatePathOfAdminEmail);
                //        body = body.Replace("[[FIRST_NAME]]", user.FIRST_NAME);
                //        body = body.Replace("[[LAST_NAME]]", user.LAST_NAME);
                //    }
                //    string subject = "Signup approved for FOX Rehab portal";
                //    string sendTo = user.EMAIL;
                //    //string sendTo = "usmanfarooq@mtbc.com";
                //    List<string> _bccList = new List<string>();
                //    //Helper.SendEmail(sendTo, subject, body, null, _bccList, "noreply@mtbc.com");
                //    Helper.SendEmail(sendTo, subject, body, null, profile, null, _bccList);

                //    #endregion
                //}
                #endregion
                #region sms to user on approval
                string recipient = "";
                //string smsBody = @"Your signup request for Fox Rehab portal has been approved. You may now login to your account and start sending a patient referral right away! Download on the App Store: https://itunes.apple.com/us/app/mtbc-fox/id1384823410?mt=8";
                //string smsBody = @"Dear " + userToUpdate.LAST_NAME + ", " + userToUpdate.FIRST_NAME + "\nYour request for access to Fox Rehabilitation's online referral portal has been approved. Also, if you agreed to receiving Electronic Plans of Care, please see your email for final security step.\nFox Rehabilitation,\nClient Services Team";
                string smsBody = @"Dear " + userToUpdate.LAST_NAME + ", " + userToUpdate.FIRST_NAME + "\nYour request for access to Fox Rehabilitation online referral portal has been approved.\nFox Rehabilitation,\nClient Services Team";
                if ( user.MOBILE_PHONE != null)
                {
                    recipient = user.MOBILE_PHONE;
                    SmsService.NJSmsService(recipient, smsBody);

                }
                #endregion
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool CheckValidUserLoginAttempt(string userName)
        {
            User user = _UserRepository.GetFirst(x => (x.USER_NAME == userName || x.EMAIL == userName) && !x.DELETED && x.IS_ACTIVE);
            if (user != null)
            {
                if (user.IS_AD_USER.HasValue && user.IS_AD_USER.Value)
                {
                    return true;
                }
            }

            Valid_Login_Attempts invalidAttempts = _validLoginAtttempts.GetFirst(x => x.USER_NAME == userName);
            if (invalidAttempts != null && invalidAttempts.FAIL_ATTEMPT_COUNT >= AppConfiguration.InvalidAttemptsCountToBlockUser)
            {
                return false;
            }

            return true;
        }

        public bool IsUserBlocked(string userName)
        {
            User user = _UserRepository.GetFirst(x => (x.USER_NAME == userName || x.EMAIL == userName) && !x.DELETED && x.IS_ACTIVE);
            if (user != null)
            {
                if (user.IS_AD_USER.HasValue && user.IS_AD_USER.Value)
                {
                    return false;
                }
            }

            Valid_Login_Attempts invalidAttempts = _validLoginAtttempts.GetFirst(x => x.USER_NAME == userName);
            if (invalidAttempts != null && invalidAttempts.FAIL_ATTEMPT_COUNT >= AppConfiguration.InvalidAttemptsCountToBlockUser + 1)
            {
                return true;
            }
            return false;
        }

        public int GetInvalidAttempts(string userName)
        {
            Valid_Login_Attempts invalidAttempts = _validLoginAtttempts.GetFirst(x => x.USER_NAME == userName);
            if(invalidAttempts != null)
            {
                return Convert.ToInt32(invalidAttempts.FAIL_ATTEMPT_COUNT);
            }
            else
            {
                return 0;
            }
        }

        public bool AddUserValidLoginAttempt(string userName)
        {
            Valid_Login_Attempts validAttempts = _validLoginAtttempts.GetFirst(x => x.USER_NAME == userName);
            if (validAttempts != null)
            {
                validAttempts.FAIL_ATTEMPT_COUNT = 0;
                validAttempts.MODIFIED_DATE = Helper.GetCurrentDate();
                _validLoginAtttempts.Update(validAttempts);
                _validLoginAtttempts.Save();
            }
            return true;
        }

        public bool AddUserInvalidLoginAttempt(string userName)
        {
            SqlParameter uName = new SqlParameter("@USER_NAME", userName);

            Valid_Login_Attempts inValidLoginAttempts = SpRepository<Valid_Login_Attempts>.GetSingleObjectWithStoreProcedure(@"Exec FOX_PROC_SET_USER_INVALID_LOGIN_ATTEMPTS @USER_NAME", uName);

            return inValidLoginAttempts == null ? false : true;

            //Valid_Login_Attempts inValidLoginAttempt = _validLoginAtttempts.GetFirst(x => x.USER_NAME == userName);
            //long? maxId =  _validLoginAtttempts.GetAll().Max(x => x.INVALID_USER_COUNT_ID);

            //long? maxId1 = _validLoginAtttempts.Get().Max(x => x.INVALID_USER_COUNT_ID);

            //return true;
            //if (inValidLoginAttempts != null)
            //{
            //    inValidLoginAttempts.FAIL_ATTEMPT_COUNT = inValidLoginAttempts.FAIL_ATTEMPT_COUNT + 1;
            //    _validLoginAtttempts.Update(inValidLoginAttempts);
            //    _validLoginAtttempts.Save();
            //}
            //else
            //{
            //    Valid_Login_Attempts temp = new Valid_Login_Attempts();
            //    temp.FAIL_ATTEMPT_COUNT = 1;
            //    temp.USER_NAME = userName;
            //    temp.INVALID_USER_COUNT_ID = _validLoginAtttempts.GetSingle(x=> x.INVALID_USER_COUNT_ID)
            //}

            //return true;


        }

        private void UnblockUser(User userToUpdate)
        {
            if (userToUpdate.Is_Blocked != null && userToUpdate.Is_Blocked == false && userToUpdate.IS_APPROVED.HasValue && userToUpdate.IS_APPROVED.Value)
            {
                List<Valid_Login_Attempts> invalidAttempts = _validLoginAtttempts.GetMany(t => t.USER_NAME == userToUpdate.USER_NAME || t.USER_NAME == userToUpdate.EMAIL);
                if (invalidAttempts != null)
                {
                    foreach (Valid_Login_Attempts d in invalidAttempts)
                    {
                        d.FAIL_ATTEMPT_COUNT = 0;
                        d.MODIFIED_DATE = Helper.GetCurrentDate();
                        _validLoginAtttempts.Update(d);
                        _validLoginAtttempts.Save();
                    }
                }
            }
        }

        public void InsertReferralRegionDashBoardAccess(long referralRegionId, List<FOX_TBL_DASHBOARD_ACCESS> obj, UserProfile profile)
        {
            List<FOX_TBL_DASHBOARD_ACCESS> existingUsers = _dashBoardAccessRepository.GetMany(e => e.REFERRAL_REGION_ID == referralRegionId && (!e.DELETED.HasValue || !e.DELETED.Value));
            if (existingUsers != null && existingUsers.Count > 0 && (obj == null || obj.Count == 0)) // dash board users exists in db but deleted from front end, then mark every one deleted in db
            {
                foreach (FOX_TBL_DASHBOARD_ACCESS d in existingUsers)
                {
                    d.DELETED = true;
                    d.MODIFIED_BY = profile.UserName;
                    d.MODIFIED_DATE = Helper.GetCurrentDate();
                    _dashBoardAccessRepository.Update(d);
                    _dashBoardAccessRepository.Save();
                }

            }
            else if ((existingUsers == null || existingUsers.Count == 0) && obj != null && obj.Count > 0) // no mapping exists in db and user add some in front end now add every object in db
            {
                foreach (FOX_TBL_DASHBOARD_ACCESS o in obj)
                {
                    FOX_TBL_DASHBOARD_ACCESS temp = _dashBoardAccessRepository.Get(e => e.REFERRAL_REGION_ID == referralRegionId && e.USER_NAME == o.USER_NAME);
                    if (temp == null)
                    {
                        if(o.ROLE_NAME != null && o.ROLE_NAME.ToLower() == "account manager")
                        {
                            InsertAlternateManager(o.USER_NAME, referralRegionId, profile);
                        }
                        else if (o.ROLE_NAME != null)
                        {
                            InsertDashBoardUsers(o.USER_NAME, referralRegionId, profile);
                        }
                    }
                    else
                    {
                        temp.DELETED = false;
                        if (o.ROLE_NAME != null && o.ROLE_NAME.ToLower() == "account manager")
                        {
                            temp.SHOW_AS_ROLE = GetRegionalDirectorId();
                        }
                        else if (o.ROLE_NAME != null)
                        {
                            temp.SHOW_AS_ROLE = GetAlternateAccountManagerId();
                        }
                        temp.MODIFIED_BY = profile.UserName;
                        temp.MODIFIED_DATE = Helper.GetCurrentDate();
                        _dashBoardAccessRepository.Update(temp);
                        _dashBoardAccessRepository.Save();
                    }
                }
            }
            else
            {
                foreach (FOX_TBL_DASHBOARD_ACCESS o in existingUsers)
                {
                    if (o.DASHBOARD_ACCESS_ID > 0)
                    {
                        FOX_TBL_DASHBOARD_ACCESS t = obj.Find(e => e.DASHBOARD_ACCESS_ID == o.DASHBOARD_ACCESS_ID); // check that user is deleted from front end and now have to soft delete from db
                        if (t == null)
                        {
                            o.DELETED = true;
                            o.MODIFIED_DATE = Helper.GetCurrentDate();
                            o.MODIFIED_BY = profile.UserName;
                            _dashBoardAccessRepository.Update(o);
                            _dashBoardAccessRepository.Save();
                        }
                    }
                }
                foreach (FOX_TBL_DASHBOARD_ACCESS o in obj)
                {
                    FOX_TBL_DASHBOARD_ACCESS checkExisting = _dashBoardAccessRepository.Get(e => e.REFERRAL_REGION_ID == referralRegionId && e.USER_NAME == o.USER_NAME);
                    if (checkExisting != null && checkExisting.DELETED.HasValue && checkExisting.DELETED.Value) // already existing mapping of user and now have to change delete bit in db
                    {
                        checkExisting.DELETED = false;
                        if (o.ROLE_NAME != null && o.ROLE_NAME.ToLower() == "account manager")
                        {
                            checkExisting.SHOW_AS_ROLE = GetRegionalDirectorId();
                        }
                        else if (o.ROLE_NAME != null)
                        {
                            checkExisting.SHOW_AS_ROLE = GetAlternateAccountManagerId();
                        }
                        checkExisting.MODIFIED_BY = profile.UserName;
                        checkExisting.MODIFIED_DATE = Helper.GetCurrentDate();
                        _dashBoardAccessRepository.Update(checkExisting);
                        _dashBoardAccessRepository.Save();
                    }
                    else if (o.DASHBOARD_ACCESS_ID <= 0) //newly added user
                    {
                        if (checkExisting == null) // check for duplicate, insert incase of user mapping does not exist
                        {
                            if (o.ROLE_NAME != null && o.ROLE_NAME.ToLower() == "account manager")
                            {
                                InsertAlternateManager(o.USER_NAME, referralRegionId, profile);
                            }
                            else if (o.ROLE_NAME != null)
                            {
                                InsertDashBoardUsers(o.USER_NAME, referralRegionId, profile);
                            }
                        }
                        else
                        {
                            checkExisting.DELETED = false;
                            if (o.ROLE_NAME != null && o.ROLE_NAME.ToLower() == "account manager")
                            {
                                checkExisting.SHOW_AS_ROLE = GetRegionalDirectorId();
                            }
                            else if (o.ROLE_NAME != null)
                            {
                                checkExisting.SHOW_AS_ROLE = GetAlternateAccountManagerId();
                            }
                            checkExisting.MODIFIED_BY = profile.UserName;
                            checkExisting.MODIFIED_DATE = Helper.GetCurrentDate();
                            _dashBoardAccessRepository.Update(checkExisting);
                            _dashBoardAccessRepository.Save();
                        }
                    }

                }
            }
        }

        private void InsertDashBoardUsers(string userName, long referralRegionId, UserProfile profile)
        {
            FOX_TBL_DASHBOARD_ACCESS temp = new FOX_TBL_DASHBOARD_ACCESS();
            temp.DASHBOARD_ACCESS_ID = Helper.getMaximumId("DASH_BOARD_ACCESS_ID");
            temp.REFERRAL_REGION_ID = referralRegionId;
            temp.SHOW_AS_ROLE = GetRegionalDirectorId();
            temp.USER_NAME = userName;
            temp.DELETED = false;
            temp.CREATED_BY = profile.UserName;
            temp.MODIFIED_BY = profile.UserName;
            temp.MODIFIED_DATE = Helper.GetCurrentDate();
            temp.CREATED_ON = Helper.GetCurrentDate();
            _dashBoardAccessRepository.Insert(temp);
            _dashBoardAccessRepository.Save();
        }
        private void InsertAlternateManager(string userName, long referralRegionId, UserProfile profile)
        {
            FOX_TBL_DASHBOARD_ACCESS temp = new FOX_TBL_DASHBOARD_ACCESS();
            temp.DASHBOARD_ACCESS_ID = Helper.getMaximumId("DASH_BOARD_ACCESS_ID");
            temp.REFERRAL_REGION_ID = referralRegionId;
            temp.SHOW_AS_ROLE = GetAlternateAccountManagerId();
            temp.USER_NAME = userName;
            temp.DELETED = false;
            temp.CREATED_BY = profile.UserName;
            temp.MODIFIED_BY = profile.UserName;
            temp.MODIFIED_DATE = Helper.GetCurrentDate();
            temp.CREATED_ON = Helper.GetCurrentDate();
            _dashBoardAccessRepository.Insert(temp);
            _dashBoardAccessRepository.Save();
        }

        private long? GetRegionalDirectorId()
        {
            return _RoleRepository.GetFirst(e => e.ROLE_NAME.ToLower() == "regional director" && !e.DELETED)?.ROLE_ID ?? null;
        }
        private long? GetAlternateAccountManagerId()
        {
            return _RoleRepository.GetFirst(e => e.ROLE_NAME.ToLower() == "account manager" && !e.DELETED)?.ROLE_ID ?? null;
        }
        public string RedirecToTalkEhr(UserProfile profile)
        {
            try
            {
                User user = _UserRepository.GetFirst(x => (x.USER_NAME == profile.UserName || x.EMAIL == profile.UserEmailAddress) && !x.DELETED && x.IS_ACTIVE);
                var roleRecord = _RoleRepository.GetFirst(r => r.ROLE_ID == user.ROLE_ID && (r.PRACTICE_CODE == user.PRACTICE_CODE || r.PRACTICE_CODE == null) && !r.DELETED);
                var a = DateTime.Now.ToString();
                //string link = "https://testing.talkehr.com/Account/LoginWebsoft?"; //Testing
                string link = "https://secure.talkehr.com/Account/LoginWebsoft?"; //Live
                using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["TalkEhrDBConnection"].ToString()))
                {
                    var Loginlink = "";
                    connection.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandText = "FOX_PROC_CHECK_EXISTING_USER_FOX_TALKEHR_TELEHEALTH";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@EMAIL", user.EMAIL);
                    cmd.Connection = connection;
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        if (reader.HasRows)
                        {
                            var userName = reader["USER_NAME"].ToString();
                            var password = reader["PASSWORD"].ToString();
                            if(!string.IsNullOrWhiteSpace(userName) && !string.IsNullOrWhiteSpace(password))
                            {
                                Loginlink = link + "param=" + Encrypt.getEncryptedCodeTalkEhrRedirection(password + "~" + userName + "~" + DateTime.Now.ToString() + "~" + "MA359");
                            }
                        }
                        reader.Close();
                        connection.Close();
                        return Loginlink.ToString();
                    }
                    cmd.CommandText = "FOX_PROC_CREATE_USER_FOR_TALKEHR_TELEHEALTH";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@USER_ID", user.USER_ID);
                    cmd.Parameters.AddWithValue("@USER_NAME", user.USER_NAME);
                    cmd.Parameters.AddWithValue("@PASSWORD", user.PASSWORD);
                    cmd.Parameters.AddWithValue("@FIRST_NAME", user.FIRST_NAME);
                    cmd.Parameters.AddWithValue("@LAST_NAME", user.LAST_NAME);
                    cmd.Parameters.AddWithValue("@PRACTICE_CODE", user.PRACTICE_CODE);
                    cmd.Parameters.AddWithValue("@CREATED_DATE", Helper.GetCurrentDate());
                    cmd.Parameters.AddWithValue("@CREATED_BY", user.CREATED_BY);
                    cmd.Parameters.AddWithValue("@MODIFIED_DATE", Helper.GetCurrentDate());
                    cmd.Parameters.AddWithValue("@MODIFIED_BY", user.MODIFIED_BY);
                    cmd.Parameters.AddWithValue("@ROLE_NAME", roleRecord.ROLE_NAME);
                    cmd.Parameters.AddWithValue("@ROLE_ID", user.ROLE_ID);
                    cmd.Connection = connection;
                    reader.Close();
                    cmd.ExecuteNonQuery();
                    Loginlink = link + "param=" + Encrypt.getEncryptedCodeTalkEhrRedirection(user.PASSWORD + "~" + user.USER_NAME + "~" + DateTime.Now.ToString() + "~" + "MA359");
                    connection.Close();
                    return Loginlink.ToString();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public bool DeleteUser(DeleteUserModel res, UserProfile profile)
        {
            if (res != null && res.user != null)
            {
                var usr = _UserRepository.GetByID(res.user.USER_ID);
                if (usr != null)
                {
                    usr.DELETED = true;
                    usr.MODIFIED_BY = profile.UserName;
                    usr.MODIFIED_DATE = Helper.GetCurrentDate();
                    try
                    {
                        _UserRepository.Update(usr);
                        _UserRepository.Save();
                        #region email to Carey on Delete

                        string subject = "User profile deleted (" + usr.EMAIL + ")";
                        var body = "";
                        body += "<body>";
                        body += "<p style='margin: 0px;'>A user profile was deleted with the following specifics:</p><br />";
                        body += "<table width='500'>";
                        body += "<tr>";
                        body += "<td>Profile deleted: </td>";
                        body += "<td>" + res.user.FIRST_NAME + " " + res.user.LAST_NAME + ". " + res.user.EMAIL + "</td>";
                        body += "</tr>";
                        body += "<tr>";
                        body += "<td>By: </td>";
                        body += "<td>" + profile.FirstName + " " + profile.LastName + ". " + profile.UserEmailAddress + " </td>";
                        body += "</tr>";
                        body += "<tr>";
                        body += "<td>Date/Time: </td>";
                        body += "<td>" + Helper.GetCurrentDate().ToString("MM / dd / yyyy hh: mm tt") + "</td>";
                        body += "</tr>";
                        body += "<tr>";
                        body += "<td>Reason: </td>";
                        body += "<td>" + res.reason + "</td>";
                        body += "</tr></table><br /><br />";
                        body += "<p style='margin: 0px;'>Regards,</ p><br />";
                        body += "<p style='margin: 0px;'>MTBC Support team</ p><br />";
                        body += "</body>";
                        string sendTo = string.Empty;
                        List<string> _ccList = new List<string>();
                        if (profile.PracticeCode == 1012714 && AppConfiguration.ClientURL.Contains("https://fox.mtbc.com/"))
                        {
                            if (res._isADuser)
                            {
                                sendTo = "Carey.sambogna@foxrehab.org";
                                _ccList.Add("support@foxrehab.org");
                            }
                            else
                            {
                                sendTo = "Carey.sambogna@foxrehab.org,adnanshah3@carecloud.com";
                                _ccList.Add("foxsupport@carecloud.com");
                            }
                        }
                        else
                        {
                            sendTo = "adnanshah3@carecloud.com,muhammadarslan3@carecloud.com";
                            _ccList.Add("foxdev@carecloud.com");
                        }
                        Helper.SendEmail(sendTo, subject, body, null, profile, _ccList);
                        #endregion
                        return true;
                    }
                    catch (Exception ex)
                    {
                        return false;
                    }
                }
                else
                {
                    return true;
                }
            }
            else
            {
                return false;
            }
        }

        public bool CheckisTalkrehab(string practiceCode)
        {
            SqlParameter pracCode = new SqlParameter("@Practice_code", practiceCode);
            var response = SpRepository<string>.GetSingleObjectWithStoreProcedure(@"Exec Af_proc_is_talkrehab_practice @Practice_code", pracCode);
            if (response == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        public List<TalkRehabDisabledModules> GetTalkrehabDisabedModules()
        {
            List<TalkRehabDisabledModules> response = SpRepository<TalkRehabDisabledModules>.GetListWithStoreProcedure(@"Exec FOX_PROC_TALKREHAB_DISABLED_MODULES");
            return response;
        }
        public List<ActiveIndexer> GetActiveIndexers(ActiveIndexer req, UserProfile profile)
        {
            SqlParameter practiceCode = new SqlParameter("@PRACTICE_CODE", profile.PracticeCode);
            SqlParameter currentPage = new SqlParameter("@CURRENT_PAGE", req.CurrentPage);
            SqlParameter recordPerPage = new SqlParameter("@RECORD_PER_PAGE", req.RecordPerPage);
            SqlParameter searchText = new SqlParameter("@SEARCH_TEXT", req.SearchText);

            return  SpRepository<ActiveIndexer>.GetListWithStoreProcedure(@"Exec FOX_PROC_GET_INDEXERS
            @PRACTICE_CODE, @CURRENT_PAGE, @RECORD_PER_PAGE, @SEARCH_TEXT", practiceCode, currentPage, recordPerPage, searchText);
        }
        public bool UpdateActiveIndexers(List<ActiveIndexer> res, UserProfile profile)
        {
            foreach(var item in res)
            {
                var indexer = _ActiveIndexerRepository.GetFirst(x => x.INDEXER == item.INDEXER);
                var user = new User();
                if (indexer != null)
                {
                    user = _UserRepository.GetFirst(x => x.USER_NAME == indexer.INDEXER);
                }
                SqlParameter practiceCode = new SqlParameter("@PRACTICE_CODE", profile.PracticeCode);
                SqlParameter _indexer = new SqlParameter("@INDEXER", item.INDEXER);
                SqlParameter defaultValue = new SqlParameter("@DEFAULT_VALUE", item.DEFAULT_VALUE);
                SqlParameter isActive = new SqlParameter("@IS_ACTIVE", item.IS_ACTIVE);
                SqlParameter modifiedBy = new SqlParameter("@MODIFIED_BY", item.MODIFIED_BY);

                var response = SpRepository<ActiveIndexer>.GetSingleObjectWithStoreProcedure(@"Exec FOX_PROC_UPDATE_ACTIVE_INDEXER
            @PRACTICE_CODE, @INDEXER, @DEFAULT_VALUE, @IS_ACTIVE, @MODIFIED_BY", practiceCode, _indexer, defaultValue, isActive, modifiedBy);

                SaveActiveIndexersLogs(indexer, response, user, profile);
            }
            return true;
        }

        public void SaveActiveIndexersLogs(ActiveIndexer oldData, ActiveIndexer newData, User user, UserProfile profile)
        {
            try
            {

                if (oldData != null && newData != null)
                {
                    if(oldData.DEFAULT_VALUE != newData.DEFAULT_VALUE)
                    {
                        string message = String.Empty;
                        if (user != null)
                        {
                            if (newData.DEFAULT_VALUE.ToLower().Contains("regular"))
                            {
                                message = user.FIRST_NAME + " " + user.LAST_NAME + " default value set as Regular indexer";
                            }
                            if (newData.DEFAULT_VALUE.ToLower().Contains("poc"))
                            {
                                message = user.FIRST_NAME + " " + user.LAST_NAME + " default value set as POC indexer";
                            }
                            if (newData.DEFAULT_VALUE.ToLower().Contains("trainee"))
                            {
                                message = user.FIRST_NAME + " " + user.LAST_NAME + " default value set as Trainee indexer";
                            }
                        }

                        var pID = CommonService.Helper.getMaximumId("ACTIVE_INDEXER_LOG_ID");

                        SqlParameter id = new SqlParameter("@ID", pID);
                        SqlParameter practiceCode = new SqlParameter("@PRACTICE_CODE", profile.PracticeCode);
                        SqlParameter indexer = new SqlParameter("@INDEXER", newData.INDEXER);
                        SqlParameter logMessage = new SqlParameter("@LOG_MESSAGE", message);
                        SqlParameter createdBy = new SqlParameter("@CREATED_BY", profile.UserName);

                        var response = SpRepository<ActiveIndexerLogs>.GetSingleObjectWithStoreProcedure(@"Exec FOX_PROC_INSERT_ACTIVE_INDEXER_LOGS
                        @ID, @PRACTICE_CODE, @INDEXER, @LOG_MESSAGE, @CREATED_BY", id, practiceCode, indexer, logMessage, createdBy);
                    }
                    if (oldData.IS_ACTIVE != newData.IS_ACTIVE)
                    {
                        string message = String.Empty;
                        if (user != null)
                        {
                            if (newData.IS_ACTIVE == true)
                            {
                                message = user.FIRST_NAME + " " + user.LAST_NAME + " status changed to active";
                            }
                            else
                            {
                                message = user.FIRST_NAME + " " + user.LAST_NAME + " status changed to inactive";
                            }
                        }

                        var pID = CommonService.Helper.getMaximumId("ACTIVE_INDEXER_LOG_ID");

                        SqlParameter id = new SqlParameter("@ID", pID);
                        SqlParameter practiceCode = new SqlParameter("@PRACTICE_CODE", profile.PracticeCode);
                        SqlParameter indexer = new SqlParameter("@INDEXER", newData.INDEXER);
                        SqlParameter logMessage = new SqlParameter("@LOG_MESSAGE", message);
                        SqlParameter createdBy = new SqlParameter("@CREATED_BY", profile.UserName);

                        var response = SpRepository<ActiveIndexerLogs>.GetSingleObjectWithStoreProcedure(@"Exec FOX_PROC_INSERT_ACTIVE_INDEXER_LOGS
                        @ID, @PRACTICE_CODE, @INDEXER, @LOG_MESSAGE, @CREATED_BY", id, practiceCode, indexer, logMessage, createdBy);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<ActiveIndexerLogs> GetActiveIndexersLogs(ActiveIndexerLogs req, UserProfile profile)
        {
            SqlParameter indexer = new SqlParameter("@INDEXER", req.INDEXER);
            SqlParameter practiceCode = new SqlParameter("@PRACTICE_CODE", profile.PracticeCode);
            SqlParameter currentPage = new SqlParameter("@CURRENT_PAGE", req.CurrentPage);
            SqlParameter recordPerPage = new SqlParameter("@RECORD_PER_PAGE", req.RecordPerPage);

            return SpRepository<ActiveIndexerLogs>.GetListWithStoreProcedure(@"Exec FOX_PROC_GET_INDEXERS_LOGS
            @INDEXER, @PRACTICE_CODE, @CURRENT_PAGE, @RECORD_PER_PAGE", practiceCode, currentPage, recordPerPage, indexer);
        }
        public List<ActiveIndexerHistory> GetActiveIndexersHistory(ActiveIndexerHistory req, UserProfile profile)
        {
            if (!string.IsNullOrEmpty(req.CREATED_DATE_STR))
                req.CREATED_DATE = Convert.ToDateTime(req.CREATED_DATE_STR);

            SqlParameter practiceCode = new SqlParameter("@PRACTICE_CODE", profile.PracticeCode);
            SqlParameter currentPage = new SqlParameter("@CURRENT_PAGE", req.CurrentPage);
            SqlParameter recordPerPage = new SqlParameter("@RECORD_PER_PAGE", req.RecordPerPage);
            SqlParameter searchText = new SqlParameter("@SEARCH_TEXT", req.SearchText);
            SqlParameter date = new SqlParameter("@DATE", req.CREATED_DATE.ToString());

            var result =  SpRepository<ActiveIndexerHistory>.GetListWithStoreProcedure(@"Exec FOX_PROC_GET_INDEXERS_HISTORY
            @PRACTICE_CODE, @CURRENT_PAGE, @RECORD_PER_PAGE, @SEARCH_TEXT, @DATE", practiceCode, currentPage, recordPerPage, searchText, date);
            return result;
        }
        public string ExportToExcelHistory(ActiveIndexerHistory req, UserProfile profile)
        {
            try
            {
                string fileName = "Active_Indexer_History_List";
                string exportPath = "";
                string path = string.Empty;
                bool exported;
                req.CurrentPage = 1;
                req.RecordPerPage = 0;
                var CalledFrom = "Active_Indexer_History";
                string virtualPath = @"/" + profile.PracticeDocumentDirectory + "/" + "Fox/ExportedFiles/";
                exportPath = HttpContext.Current.Server.MapPath("~" + virtualPath);
                fileName = DocumentHelper.GenerateSignatureFileName(fileName) + ".xlsx";
                if (!Directory.Exists(exportPath))
                {
                    Directory.CreateDirectory(exportPath);
                }
                List<ActiveIndexerHistory> result = new List<ActiveIndexerHistory>();
                var pathtowriteFile = exportPath + "\\" + fileName;
                result = GetActiveIndexersHistory(req, profile);
                for (int i = 0; i < result.Count(); i++)
                {
                    result[i].ROW = i + 1;

                }
                exported = ExportToExcel.CreateExcelDocument<ActiveIndexerHistory>(result, pathtowriteFile, CalledFrom.Replace(' ', '_'));
                return virtualPath + fileName;
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool CheckActiveStatus(UserProfile profile)
        {
            var active = _ActiveIndexerRepository.GetFirst(x => (x.IS_ACTIVE ?? false) == true && x.PRACTICE_CODE == profile.PracticeCode && !(x.DELETED));

            if(active != null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        List<UserTeamModel> IUserManagementService.UpdateUserTeam(UserProfile profile, string callerUserID, string filter)
        {
            var result = new List<UserTeamModel>();
            string[] teamID = callerUserID.Split(',');
            long[] teamIDArray = new long[teamID.Length];
            int i = 0;
            foreach (string ID in teamID)
            {
                if(ID != "")
                {
                    //teamIDArray[i] = long.Parse(ID);
                    SqlParameter userTeamID = new SqlParameter { ParameterName = "USER_TEAM_ID", SqlDbType = SqlDbType.BigInt, Value = Helper.getMaximumId("USER_TEAM_ID") };
                    SqlParameter userID = new SqlParameter { ParameterName = "USER_ID", SqlDbType = SqlDbType.BigInt, Value = profile.userID };
                    SqlParameter phdCallScenareioID = new SqlParameter { ParameterName = "PHD_CALL_SCENARIO_ID", SqlDbType = SqlDbType.BigInt, Value = ID };
                    SqlParameter practiceCode = new SqlParameter { ParameterName = "PRACTICE_CODE", SqlDbType = SqlDbType.BigInt, Value = profile.PracticeCode };
                    SqlParameter filterForCheck = new SqlParameter { ParameterName = "FILTER", SqlDbType = SqlDbType.VarChar, Value = filter};
                    SqlParameter counter = new SqlParameter { ParameterName = "COUNTER", SqlDbType = SqlDbType.BigInt, Value = 0};
                    result = SpRepository<UserTeamModel>.GetListWithStoreProcedure(@"exec FOX_PROC_INSERT_USER_TEAM @USER_TEAM_ID, @USER_ID, @PHD_CALL_SCENARIO_ID ,@PRACTICE_CODE,@COUNTER,@FILTER", userTeamID, userID, phdCallScenareioID, practiceCode,counter, filterForCheck);
                }
                /*if (author.Trim() != "")
                    Console.WriteLine(author);*/
            }
            //int[] tokensArray = Array.ConvertAll(tokens, s => int.Parse(s));

            /*SqlParameter practiceCode = new SqlParameter { ParameterName = "PRACTICE_CODE", SqlDbType = SqlDbType.BigInt, Value = profile.PracticeCode };
            SqlParameter callAttendedBy = new SqlParameter { ParameterName = "CALL_ATTENDED_BY", SqlDbType = SqlDbType.BigInt, Value = tokensArray };
            var result = SpRepository<UserTeamModel>.GetListWithStoreProcedure(@"exec FOX_PROC_INSERT_USER_TEAM @PRACTICE_CODE, @CALL_ATTENDED_BY", practiceCode, callAttendedBy);
            *///IEnumerable<UserTeamModel> userTeamModel = null;
            //if (result != null)
            //{
            //    userTeamModel = result.Select(c => new UserTeamModel
            //    {
            //        PHD_CALL_SCENARIO_ID = c.PHD_CALL_SCENARIO_ID,
                   
            //    });
            //    if (userTeamModel != null)
            //    {
            //        result = userTeamModel.ToList();
            //    }
            //}
            //else
            //{
            //    return result = new List<UserTeamModel>();
            //}
            return result;
            throw new NotImplementedException();
        }
    }
}