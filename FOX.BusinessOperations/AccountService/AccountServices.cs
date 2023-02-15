using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FOX.DataModels.GenericRepository;
using FOX.DataModels.Context;
using FOX.DataModels.Models.CommonModel;
using FOX.DataModels.Models.ExternalUserModel;
using FOX.DataModels.Models.Security;
using FOX.DataModels.Models.CasesModel;
using FOX.BusinessOperations.CommonService;
using FOX.BusinessOperations.Security;
using FOX.DataModels.Models.Settings.RoleAndRights;
using System.IO;
using FOX.BusinessOperations.CommonServices;
using System.Data.SqlClient;
using System.Data;
using System.Net.Http;
using Newtonsoft.Json;
using System.Net;
using FOX.DataModels.Models.Settings.ReferralSource;
using FOX.DataModels.Models.Settings.Practice;
using FOX.DataModels.Models.SenderName;
using FOX.DataModels.Models.Patient;
using FOX.DataModels.Models.Authorization;
using System.Xml;
using System.Collections.Specialized;
using static FOX.DataModels.Models.Security.ProfileToken;
using FOX.DataModels;
namespace FOX.BusinessOperations.AccountService
{
    public class AccountServices : IAccountServices
    {

        private readonly GenericRepository<DataModels.Models.SenderType.FOX_TBL_SENDER_TYPE> _fox_tbl_sender_type;
        private readonly GenericRepository<ReferralSource> _fox_tbl_ordering_ref_source;
        private readonly GenericRepository<PracticeOrganization> _fox_tbl_practice_organization;
        private readonly GenericRepository<FOX_TBL_IDENTIFIER> _fox_tbl_identifier;
        private readonly GenericRepository<Speciality> _speciality;
        private readonly GenericRepository<PasswordHistory> _passwordHistoryRepository;
        private readonly GenericRepository<FOX_TBL_SENDER_NAME> _fox_tbl_sender_name;
        private readonly GenericRepository<User> _userRepository;
        private readonly GenericRepository<Zip_City_State> _zipCitiesStatesRepository;
        private readonly GenericRepository<FOX_TBL_PATIENT> _FoxTblPatientRepository;
        private readonly GenericRepository<ProfileToken> profileToken;
        private readonly GenericRepository<WS_TBL_FOX_Login_LOGS> _loginLogsRepository;
        private readonly GenericRepository<ProfileTokensSecurity> _profileTokenSecurityRepository;
        //DbContextCases
        private readonly DbContextCases _DbContextCases = new DbContextCases();
        private readonly DbContextIndexinfo _IndexinfoContext = new DbContextIndexinfo();
        private readonly DbContextCommon _DbContextCommon = new DbContextCommon();
        private readonly DbContextSecurity security = new DbContextSecurity();
        private readonly DbContextPatient _PatientContext = new DbContextPatient();

        public AccountServices()
        {
            _fox_tbl_sender_type = new GenericRepository<FOX.DataModels.Models.SenderType.FOX_TBL_SENDER_TYPE>(_DbContextCommon);
            _fox_tbl_ordering_ref_source = new GenericRepository<ReferralSource>(_IndexinfoContext);
            _fox_tbl_practice_organization = new GenericRepository<PracticeOrganization>(_DbContextCommon);
            _userRepository = new GenericRepository<User>(_DbContextCommon);
            _fox_tbl_identifier = new GenericRepository<FOX_TBL_IDENTIFIER>(_DbContextCases);
            _speciality = new GenericRepository<Speciality>(_DbContextCases);
            _passwordHistoryRepository = new GenericRepository<PasswordHistory>(security);
            _fox_tbl_sender_name = new GenericRepository<FOX_TBL_SENDER_NAME>(_DbContextCommon);
            _zipCitiesStatesRepository = new GenericRepository<Zip_City_State>(_DbContextCommon);
            _FoxTblPatientRepository = new GenericRepository<FOX_TBL_PATIENT>(_PatientContext);
            profileToken = new GenericRepository<ProfileToken>(security);
            _loginLogsRepository = new GenericRepository<WS_TBL_FOX_Login_LOGS>(security);
            _profileTokenSecurityRepository = new GenericRepository<ProfileTokensSecurity>(security);
        }
        public ResponseGetSenderTypesModel getSenderTypes()
        {
            var practiceCode = AppConfiguration.GetPracticeCode;
            try
            {
                var senderTypeList = _fox_tbl_sender_type.GetMany(t => !t.DELETED && t.DISPLAY_ORDER != null && t.PRACTICE_CODE == practiceCode)
                    .OrderBy(t => t.DISPLAY_ORDER)
                    .ToList();
                return new ResponseGetSenderTypesModel() { SenderTypeList = senderTypeList, ErrorMessage = "", Message = "Get Sender Types List Successfully.", Success = true };
            }
            catch (Exception exception)
            {
                return new ResponseGetSenderTypesModel() { SenderTypeList = null, ErrorMessage = exception.ToString(), Message = "We encountered an error while processing your request.", Success = false };
            }
        }

        public UserDetailsByNPIResponseModel getUserDetailByNPI(UserDetailsByNPIRequestModel model)
        {
            try
            {
                var NPIalreadyInUse = this.CheckForDublicateNPI(model.NPI);
                if (NPIalreadyInUse)
                {
                    return new UserDetailsByNPIResponseModel() { userDetailByNPIModel = null, ErrorMessage = "NPI already in use", Message = "The NPI is already in use.", Success = true };
                }
                else
                {
                    var response = GetProviderDetailByNpi(model.NPI);
                    if (response != null && response.results[0].enumeration_type == "NPI-1")
                    {
                        if (response.results.Count > 0 && response.results[0].addresses.Count > 0 && response.results[0].addresses[0].postal_code.Length > 5 && response.results[0].addresses[0].postal_code.Length == 9)
                        {
                            response.results[0].addresses[0].postal_code = response.results[0].addresses[0].postal_code.Insert(5, "-");
                        }

                        return new UserDetailsByNPIResponseModel() { userDetailByNPIModel = response, ErrorMessage = "", Message = "User details has been retrived by NPI", Success = true };
                    }
                    else
                    {
                        return new UserDetailsByNPIResponseModel() { userDetailByNPIModel = null, ErrorMessage = "", Message = "User details didn't found against provided NPI", Success = true };
                    }
                }
            }
            catch (Exception ex)
            {
                return new UserDetailsByNPIResponseModel() { userDetailByNPIModel = null, ErrorMessage = ex.ToString(), Message = ex.Message, Success = false };
            }

        }
        public UserDetailByNPIModel GetProviderDetailByNpi(string npi)
        {
            string url = AppConfiguration.NPIRegistryURL + "&number=" + npi;
            using (HttpClient client = new HttpClient())
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                var response = client.GetAsync(url).Result;
                if (response.IsSuccessStatusCode)
                {
                    var responseContent = response.Content;
                    var r = responseContent.ReadAsStringAsync().Result;
                    return JsonConvert.DeserializeObject<UserDetailByNPIModel>(r);
                }
                else
                {
                    return null;
                }
            }
        }

        public CityDetailByZipCodeResponseModel getCityDetailByZipCode(CityDetailByZipCodeRequestModel model)
        {
            try
            {
                var zip = model.ZipCode.Replace("-", string.Empty);
                zip = zip.Substring(0, 5);
                var varResult = (from z in _DbContextCommon.ZipCitiesStates
                                 where z.ZIP_Code == zip
                                 && (z.Deleted != true)
                                 orderby z.City_Name
                                 select new
                                 {
                                     z.City_Name,
                                     z.State_Code,
                                     z.ZIP_Code,
                                     z.Time_Zone
                                 }).ToList();
                List<Zip_City_State> list = new List<Zip_City_State>();
                foreach (var r in varResult)
                {
                    list.Add(new Zip_City_State()
                    {

                        ZIP_Code = model.ZipCode,
                        State_Code = r.State_Code,
                        City_Name = r.City_Name,
                        Time_Zone = r.Time_Zone
                    });
                }
                if (varResult.Count > 0)
                {
                    return new CityDetailByZipCodeResponseModel() { zip_city_state = list, ErrorMessage = "", Message = "Detail retrived successfully", Success = true };
                }
                else
                {
                    return new CityDetailByZipCodeResponseModel() { zip_city_state = null, ErrorMessage = "", Message = "No details found", Success = false };
                }
            }
            catch (Exception ex)
            {
                return new CityDetailByZipCodeResponseModel() { zip_city_state = null, ErrorMessage = ex.Message, Message = ex.Message, Success = false };
            }
        }
        public bool CheckIfEmailAlreadyInUse(EmailExist model)
        {
            var practiceCode = AppConfiguration.GetPracticeCode;
            var user = _userRepository.GetFirst(t => t.EMAIL == model.EMAIL && !t.DELETED && t.PRACTICE_CODE == practiceCode);
            return user == null ? false : true;
        }

        public string UploadSignature(HttpPostedFile signature)
        {
            var directoryPath = "";
            var newFileName = "";
            var fileExtension = "";
            var pathToWriteImage = "";
            var pathToStoreIndb = "";
            fileExtension = Path.GetExtension(signature.FileName);
            if (fileExtension.ToLower() == ".jpeg" || fileExtension.ToLower() == ".jpg" || fileExtension.ToLower() == ".png" || fileExtension.ToLower() == ".bmp")
            {
                directoryPath = HttpContext.Current.Server.MapPath(@"~/FoxDocumentDirectory/Fox/Signatures");
                newFileName = new string(Path.GetFileNameWithoutExtension(signature.FileName).Take(10).ToArray()).Replace(" ", "-");
                newFileName = newFileName + DateTime.Now.ToString("yyyyMMddHHmmssffff") + fileExtension.ToLower();
                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }
                pathToWriteImage = Path.Combine(directoryPath, newFileName);
                pathToStoreIndb = "FoxDocumentDirectory\\Fox\\Signatures\\" + newFileName;
                try
                {
                    signature.SaveAs(pathToWriteImage);
                    return pathToStoreIndb;
                }
                catch (Exception)
                {
                    return "";
                }
            }
            else
            {
                return "";
            }
        }
        public FoxTBLPracticeOrganizationResponseModel getPractices(SmartSearchRequest model)
        {
            var practiceCode = AppConfiguration.GetPracticeCode;
            try
            {
                var practicesList = _DbContextCommon.PracticeOrganizations.Where(t => !t.DELETED && t.NAME.StartsWith(model.Keyword) && t.PRACTICE_CODE == practiceCode).OrderBy(t => t.NAME).ToList();
                if (practicesList.Count > 0)
                {
                    return new FoxTBLPracticeOrganizationResponseModel() { fox_tbl_practice_organization = practicesList, ErrorMessage = "", Message = "Practices retrived successfully", Success = true };
                }
                else
                {
                    return new FoxTBLPracticeOrganizationResponseModel() { fox_tbl_practice_organization = null, ErrorMessage = "No practice exist", Message = "No practice exist", Success = true };
                }
            }
            catch (Exception ex)
            {
                return new FoxTBLPracticeOrganizationResponseModel() { fox_tbl_practice_organization = null, ErrorMessage = ex.Message, Message = ex.Message, Success = false };
            }
        }
        public List<SmartIdentifierRes> getSmartIdentifier(SmartSearchRequest obj)
        {
            var practiceCode = AppConfiguration.GetPracticeCode;
            try
            {
                if (obj.SEARCHVALUE == null)
                    obj.SEARCHVALUE = "";
                var pRACTICE_CODE = new SqlParameter("PRACTICE_CODE", SqlDbType.BigInt) { Value = practiceCode };
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

        public SmartSpecialitySearchResponseModel getSmartSpecialities(SmartSearchRequest model)
        {
            var practiceCode = AppConfiguration.GetPracticeCode;
            try
            {
                var specialitiesList = _speciality.GetMany(i => !i.DELETED && i.NAME.StartsWith(model.Keyword) && i.PRACTICE_CODE == practiceCode).OrderBy(i => i.NAME).ToList();
                if (specialitiesList.Count > 0)
                {
                    return new SmartSpecialitySearchResponseModel() { specialities = specialitiesList, ErrorMessage = "", Message = "Specialties retrived successfully", Success = true };
                }
                else
                {
                    return new SmartSpecialitySearchResponseModel() { specialities = null, ErrorMessage = "No Specialties exist", Message = "No Specialties exist", Success = true };
                }
            }
            catch (Exception ex)
            {
                return new SmartSpecialitySearchResponseModel() { specialities = null, ErrorMessage = ex.Message, Message = ex.Message, Success = false };
            }
        }

        public ExternalUserSignupResponseModel CreateExternalUser(User user)
        {
            var usr = _userRepository.GetFirst(u => u.EMAIL == user.EMAIL && !u.DELETED);
            if (usr != null)
            {
                usr.FIRST_NAME = user.FIRST_NAME;
                usr.LAST_NAME = user.LAST_NAME;
                usr.EMAIL = user.EMAIL;
                usr.COMMENTS = user.COMMENTS;
                usr.PHONE_NO = user.PHONE_NO;
                usr.ACO = user.ACO;
                usr.SPECIALITY = user.SPECIALITY;
                usr.SNF = user.SNF;
                usr.HOSPITAL = user.HOSPITAL;
                usr.HHH = user.HHH;
                usr.THIRD_PARTY_REFERRAL_SOURCE = user.THIRD_PARTY_REFERRAL_SOURCE;
                usr.PRACTICE_ORGANIZATION_ID = user.PRACTICE_ORGANIZATION_ID;
                //usr.PRACTICE_NAME = user.PRACTICE_NAME;
                usr.ADDRESS_1 = user.ADDRESS_1;
                usr.CITY = user.CITY;
                usr.STATE = user.STATE;
                usr.ZIP = user.ZIP.Substring(0, 5);
                usr.NPI = user.NPI;
                //usr.SENDER_TYPE = user.SENDER_TYPE;
                usr.FOX_TBL_SENDER_TYPE_ID = user.FOX_TBL_SENDER_TYPE_ID;
                usr.GENDER = user.GENDER;
                usr.TIME_ZONE = user.TIME_ZONE;
                usr.WORK_PHONE = user.WORK_PHONE;
                usr.PASSWORD = Encrypt.getEncryptedCode(user.PASSWORD);
                usr.PRACTICE_CODE = AppConfiguration.GetPracticeCode;
                usr.CREATED_BY = "self";
                usr.CREATED_DATE = Helper.GetCurrentDate();
                usr.MODIFIED_BY = "self";
                usr.MODIFIED_DATE = Helper.GetCurrentDate();
                usr.IS_APPROVED = false;
                usr.USER_TYPE = "External";
                usr.IS_ACTIVE = false;
                usr.DELETED = false;
                usr.FAX = user.FAX;
                usr.MOBILE_PHONE = user.MOBILE_PHONE;
                usr.ROLE_ID = 0;
                usr.SIGNATURE_PATH = user.SIGNATURE_PATH;
                //additional
                usr.PASSWORD_CHANGED_DATE = null;
                usr.MIDDLE_NAME = null;
                usr.USER_DISPLAY_NAME = null;
                usr.DESIGNATION = null;
                usr.DATE_OF_BIRTH = null;
                usr.RESET_PASS = null;
                usr.SECURITY_QUESTION = null;
                usr.SECURITY_QUESTION_ANSWER = null;
                usr.LOCKEDBY = null;
                usr.LAST_LOGIN_DATE = null;
                usr.FAILED_PASSWORD_ATTEMPT_COUNT = 0;
                usr.IS_LOCKED_OUT = false;
                usr.PASS_RESET_CODE = null;
                usr.ACTIVATION_CODE = null;
                usr.ADDRESS_2 = null;
                usr.CREATED_BY = "self";
                usr.CREATED_DATE = Helper.GetCurrentDate();
                usr.MODIFIED_BY = "self";
                usr.MODIFIED_DATE = Helper.GetCurrentDate();
                usr.MANAGER_ID = null;
                usr.LANGUAGE = null;
                usr.PASSWORD_RESET_TICKS = null;
                try
                {
                    _userRepository.Update(usr);
                    _userRepository.Save();
                    #region email to admin
                    string bodyOfAdminEmail = string.Empty;
                    string templatePathOfAdminEmail = HttpContext.Current.Server.MapPath("~/HtmlTemplates/external_user_signup_email_to_admin.html");
                    if (File.Exists(templatePathOfAdminEmail))
                    {
                        var formatedMobileNumber = "";
                        if (!string.IsNullOrEmpty(user.MOBILE_PHONE) && user.MOBILE_PHONE.Length == 10)
                        {
                            var part1 = user.MOBILE_PHONE.Substring(0, 3);
                            var part2 = user.MOBILE_PHONE.Substring(3, 3);
                            var part3 = user.MOBILE_PHONE.Substring(6, 4);
                            formatedMobileNumber = "(" + part1 + ") " + part2 + "-" + part3;
                        }

                        string senderType = _fox_tbl_sender_type.Get(t => !t.DELETED && t.PRACTICE_CODE == AppConfiguration.GetPracticeCode && t.FOX_TBL_SENDER_TYPE_ID == user.FOX_TBL_SENDER_TYPE_ID)?.SENDER_TYPE_NAME ?? "";
                        string practiceOrganizationName = _fox_tbl_practice_organization.Get(t => !t.DELETED && t.PRACTICE_CODE == AppConfiguration.GetPracticeCode && t.PRACTICE_ORGANIZATION_ID == user.PRACTICE_ORGANIZATION_ID)?.NAME ?? "";

                        bodyOfAdminEmail = File.ReadAllText(templatePathOfAdminEmail);
                        bodyOfAdminEmail = bodyOfAdminEmail.Replace("[[NPI]]", user.NPI)
                                           .Replace("[[FIRST_NAME]]", user.FIRST_NAME)
                                           .Replace("[[MOBILE_NO]]", formatedMobileNumber)
                                           .Replace("[[SENDER_TYPE]]", senderType)
                                           .Replace("[[LAST_NAME]]", user.LAST_NAME)
                                           .Replace("[[EMAIL]]", user.EMAIL)
                                           .Replace("[[PRACTICE_NAME]]", practiceOrganizationName)
                                           .Replace("[[ACO]]", user.ACO_NAME)
                                           .Replace("[[SPECIALITY]]", user.SPECIALITY_NAME)
                                           .Replace("[[SNF]]", user.SNF_NAME)
                                           .Replace("[[HOSPITAL]]", user.HOSPITAL_NAME)
                                           .Replace("[[HHH]]", user.HHH_NAME)
                                           .Replace("[[COMMENTS]]", user.COMMENTS);
                    }
                    string subjectOfAdminEmail = "New signup request: " + user.LAST_NAME + ", " + user.FIRST_NAME + ", " + user.USER_TYPE;
                    string sendToOfAdminEmail = string.Empty;
                    string firstName = string.IsNullOrEmpty(user.FIRST_NAME) ? string.Empty : user.FIRST_NAME;
                    string lastName = string.IsNullOrEmpty(user.LAST_NAME) ? string.Empty : user.LAST_NAME;
                    if (!Helper.IsTestUser(firstName.ToLower(), lastName.ToLower()) && AppConfiguration.ClientURL.Contains("https://fox.mtbc.com/"))
                    {
                        sendToOfAdminEmail = AppConfiguration.SendEmailToAdminOnExternalUserSignUp_CC;
                    }
                    else
                    {
                        sendToOfAdminEmail = AppConfiguration.SendEmailToQAOnExternalUserSignUp_To;
                    }
                    List<string> _bccListOfAdminEmail = new List<string>();
                    if (string.IsNullOrEmpty(sendToOfAdminEmail))
                    {
                        Helper.SendEmail(sendToOfAdminEmail, subjectOfAdminEmail, bodyOfAdminEmail, null, null, null, _bccListOfAdminEmail);
                    }

                    #endregion
                    PasswordHistory ph = new PasswordHistory()
                    {
                        PASSWORD_ID = Helper.getMaximumId("PASSWORD_ID"),
                        DELETED = false,
                        USER_ID = user.USER_ID,
                        PASSWORD = user.PASSWORD,
                        CREATED_BY = "self",
                        CREATED_DATE = Helper.GetCurrentDate(),
                        MODIFIED_BY = "self",
                        MODIFIED_DATE = Helper.GetCurrentDate(),
                        PRACTICE_CODE = user.PRACTICE_CODE
                    };
                    _passwordHistoryRepository.Insert(ph);
                    _passwordHistoryRepository.Save();
                    //var senderName = _fox_tbl_sender_name.GetFirst(t => t.SENDER_NAME_CODE.Equals(user.USER_NAME));
                    //if (senderName == null)
                    //{
                    //    senderName = new FOX_TBL_SENDER_NAME();
                    //    senderName.FOX_TBL_SENDER_NAME_ID = Helper.getMaximumId("FOX_TBL_SENDER_NAME_ID");
                    //    senderName.FOX_TBL_SENDER_TYPE_ID = null;
                    //    senderName.PRACTICE_CODE = 1011163;
                    //    senderName.SENDER_NAME_CODE = user.USER_NAME;
                    //    senderName.SENDER_NAME_DESCRIPTION = user.USER_DISPLAY_NAME;
                    //    senderName.CREATED_BY = senderName.MODIFIED_BY = user.USER_NAME;
                    //    senderName.CREATED_DATE = senderName.MODIFIED_DATE = Helper.GetCurrentDate();
                    //    senderName.DELETED = false;

                    //    _fox_tbl_sender_name.Insert(senderName);
                    //    _DbContextCommon.SaveChanges();
                    //    _fox_tbl_sender_name.Save();

                    //}
                    return new ExternalUserSignupResponseModel() { status = 1, ErrorMessage = "Account registration request submitted successfully. You'll be notified by an SMS or email once your request is processed. ", Message = "Account registration request submitted successfully. You'll be notified by an SMS or email once your request is processed.", Success = true };
                }
                catch (Exception ex)
                {
                    return new ExternalUserSignupResponseModel() { status = 0, ErrorMessage = ex.Message, Message = ex.Message, Success = false };
                }
            }
            else
            {
                user.USER_ID = Helper.getMaximumId("USER_ID");
                user.USER_NAME = user.LAST_NAME + "_" + user.USER_ID.ToString();
                user.PASSWORD = Encrypt.getEncryptedCode(user.PASSWORD);
                user.PRACTICE_CODE = 1011163;
                user.CREATED_BY = "self";
                user.CREATED_DATE = Helper.GetCurrentDate();
                user.MODIFIED_BY = "self";
                user.MODIFIED_DATE = Helper.GetCurrentDate();
                user.IS_APPROVED = false;
                user.USER_TYPE = "External";
                user.IS_ACTIVE = false;
                try
                {
                    _userRepository.Insert(user);
                    _userRepository.Save();

                    #region email to admin

                    string bodyOfAdminEmail = string.Empty;
                    string sendToOfAdminEmail = string.Empty;
                    string templatePathOfAdminEmail = HttpContext.Current.Server.MapPath("~/HtmlTemplates/external_user_signup_email_to_admin.html");
                    if (File.Exists(templatePathOfAdminEmail))
                    {
                        var formatedMobileNumber = "";
                        if (!string.IsNullOrEmpty(user.MOBILE_PHONE) && user.MOBILE_PHONE.Length == 10)
                        {
                            var part1 = user.MOBILE_PHONE.Substring(0, 3);
                            var part2 = user.MOBILE_PHONE.Substring(3, 3);
                            var part3 = user.MOBILE_PHONE.Substring(6, 4);
                            formatedMobileNumber = "(" + part1 + ") " + part2 + "-" + part3;
                        }

                        string senderType = _fox_tbl_sender_type.Get(t => !t.DELETED && t.PRACTICE_CODE == AppConfiguration.GetPracticeCode && t.FOX_TBL_SENDER_TYPE_ID == user.FOX_TBL_SENDER_TYPE_ID)?.SENDER_TYPE_NAME ?? "";
                        string practiceOrganizationName = _fox_tbl_practice_organization.Get(t => !t.DELETED && t.PRACTICE_CODE == AppConfiguration.GetPracticeCode && t.PRACTICE_ORGANIZATION_ID == user.PRACTICE_ORGANIZATION_ID)?.NAME ?? "";

                        bodyOfAdminEmail = File.ReadAllText(templatePathOfAdminEmail);
                        bodyOfAdminEmail = bodyOfAdminEmail.Replace("[[NPI]]", user.NPI)
                                           .Replace("[[FIRST_NAME]]", user.FIRST_NAME)
                                           .Replace("[[MOBILE_NO]]", formatedMobileNumber)
                                           .Replace("[[SENDER_TYPE]]", senderType)
                                           .Replace("[[LAST_NAME]]", user.LAST_NAME)
                                           .Replace("[[EMAIL]]", user.EMAIL)
                                           .Replace("[[PRACTICE_NAME]]", practiceOrganizationName)
                                           .Replace("[[ACO]]", user.ACO_NAME)
                                           .Replace("[[SPECIALITY]]", user.SPECIALITY_NAME)
                                           .Replace("[[SNF]]", user.SNF_NAME)
                                           .Replace("[[HOSPITAL]]", user.HOSPITAL_NAME)
                                           .Replace("[[HHH]]", user.HHH_NAME)
                                           .Replace("[[COMMENTS]]", user.COMMENTS);
                    }
                    string subjectOfAdminEmail = "New signup request: " + user.LAST_NAME + ", " + user.FIRST_NAME + ", " + user.USER_TYPE;
                    if (AppConfiguration.ClientURL.Contains("https://fox.mtbc.com/"))
                    {
                        sendToOfAdminEmail = "support@foxrehab.org";
                    }
                    List<string> _bccListOfAdminEmail = new List<string>();
                    Helper.SendEmail(sendToOfAdminEmail, subjectOfAdminEmail, bodyOfAdminEmail, null, null, null, _bccListOfAdminEmail);

                    #endregion
                    PasswordHistory ph = new PasswordHistory()
                    {
                        PASSWORD_ID = Helper.getMaximumId("PASSWORD_ID"),
                        DELETED = false,
                        USER_ID = user.USER_ID,
                        PASSWORD = user.PASSWORD,
                        CREATED_BY = "self",
                        CREATED_DATE = Helper.GetCurrentDate(),
                        MODIFIED_BY = "self",
                        MODIFIED_DATE = Helper.GetCurrentDate(),
                        PRACTICE_CODE = user.PRACTICE_CODE
                    };
                    _passwordHistoryRepository.Insert(ph);
                    _passwordHistoryRepository.Save();
                    //var senderName = _fox_tbl_sender_name.GetFirst(t => t.SENDER_NAME_CODE.Equals(user.USER_NAME));
                    //if (senderName == null)
                    //{
                    //    senderName = new FOX_TBL_SENDER_NAME();
                    //    senderName.FOX_TBL_SENDER_NAME_ID = Helper.getMaximumId("FOX_TBL_SENDER_NAME_ID");
                    //    senderName.FOX_TBL_SENDER_TYPE_ID = null;
                    //    senderName.PRACTICE_CODE = 1011163;
                    //    senderName.SENDER_NAME_CODE = user.USER_NAME;
                    //    senderName.SENDER_NAME_DESCRIPTION = user.USER_DISPLAY_NAME;
                    //    senderName.CREATED_BY = senderName.MODIFIED_BY = user.USER_NAME;
                    //    senderName.CREATED_DATE = senderName.MODIFIED_DATE = Helper.GetCurrentDate();
                    //    senderName.DELETED = false;

                    //    _fox_tbl_sender_name.Insert(senderName);
                    //    _DbContextCommon.SaveChanges();
                    //    _fox_tbl_sender_name.Save();

                    //}
                    ////Create Referral Source for external user
                    if (EntityHelper.isTalkRehab)
                    {
                        CreateExternalUserOrdRefSource(user.USER_ID);
                    }
                    return new ExternalUserSignupResponseModel() { status = 1, ErrorMessage = "Account registration request submitted successfully. You'll be notified by an SMS or email once your request is processed. ", Message = "Account registration request submitted successfully. You'll be notified by an SMS or email once your request is processed.", Success = true };
                }
                catch (Exception ex)
                {
                    return new ExternalUserSignupResponseModel() { status = 0, ErrorMessage = ex.Message, Message = ex.Message, Success = false };
                }
            }
        }

        public ReferralSource CreateExternalUserOrdRefSource(long userId)
        {
            try
            {
                var usr = _userRepository.GetFirst(e => e.USER_ID == userId && e.PRACTICE_CODE == AppConfiguration.GetPracticeCode && !e.DELETED);
                if (usr != null)
                {
                    var dbReferralSource = new ReferralSource();
                    dbReferralSource.FIRST_NAME = usr.FIRST_NAME;
                    dbReferralSource.LAST_NAME = usr.LAST_NAME;
                    dbReferralSource.ADDRESS = usr.ADDRESS_1;
                    dbReferralSource.ADDRESS_2 = usr.ADDRESS_2;
                    dbReferralSource.ZIP = usr.ZIP;
                    dbReferralSource.CITY = usr.CITY;
                    dbReferralSource.STATE = usr.STATE;
                    dbReferralSource.PHONE = !string.IsNullOrEmpty(usr.WORK_PHONE) ? usr.WORK_PHONE : usr.MOBILE_PHONE;
                    dbReferralSource.FAX = usr.FAX;
                    dbReferralSource.DELETED = false;
                    dbReferralSource.CREATED_BY = "CareCloud Remote";
                    dbReferralSource.CREATED_DATE = Helper.GetCurrentDate();
                    dbReferralSource.MODIFIED_BY = "CareCloud Remote";
                    dbReferralSource.MODIFIED_DATE = Helper.GetCurrentDate();
                    _fox_tbl_ordering_ref_source.Insert(dbReferralSource);
                    _fox_tbl_ordering_ref_source.Save();

                    return dbReferralSource;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //public FOX_TBL_ORDERING_REF_SOURCE CreateExternalUserOrdRefSource(long userId)
        //{
        //    try
        //    {
        //        var usr = _userRepository.GetFirst(e => e.USER_ID == userId && e.PRACTICE_CODE == AppConfiguration.GetPracticeCode && !e.DELETED);
        //        if (usr != null)
        //        {
        //            var dbReferralSource = new FOX_TBL_ORDERING_REF_SOURCE();
        //            dbReferralSource.FIRST_NAME = usr.FIRST_NAME;
        //            dbReferralSource.LAST_NAME = usr.LAST_NAME;
        //            dbReferralSource.ADDRESS = usr.ADDRESS_1;
        //            dbReferralSource.ADDRESS_2 = usr.ADDRESS_2;
        //            dbReferralSource.ZIP = usr.ZIP;
        //            dbReferralSource.CITY = usr.CITY;
        //            dbReferralSource.STATE = usr.STATE;
        //            dbReferralSource.PHONE = !string.IsNullOrEmpty(usr.WORK_PHONE) ? usr.WORK_PHONE : usr.MOBILE_PHONE;
        //            dbReferralSource.FAX = usr.FAX;
        //            dbReferralSource.DELETED = false;

        //            dbReferralSource.CREATED_BY = "FOX TEAM";
        //            dbReferralSource.CREATED_DATE = Helper.GetCurrentDate();
        //            dbReferralSource.MODIFIED_BY = "FOX TEAM";
        //            dbReferralSource.MODIFIED_DATE = Helper.GetCurrentDate();
        //            _fox_tbl_ordering_ref_source.Insert(dbReferralSource);
        //            _fox_tbl_ordering_ref_source.Save();

        //            return dbReferralSource;
        //        }
        //        else {
        //            return null;
        //        }
        //    }
        //    catch (Exception ex) {
        //        throw ex;
        //    }
        //}
        public bool CheckForDublicateNPI(string npi)
        {
            try
            {
                var user = _userRepository.GetMany(u => u.NPI == npi).FirstOrDefault();
                if (user != null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        public void EmailToAdmin(dynamic user)
        {
            string bodyOfAdminEmail = string.Empty;
            string templatePathOfAdminEmail = HttpContext.Current.Server.MapPath("~/HtmlTemplates/external_user_signup_email_to_admin.html");
            if (File.Exists(templatePathOfAdminEmail))
            {
                var formatedMobileNumber = "";
                if (!string.IsNullOrEmpty(user.MOBILE_PHONE) && user.MOBILE_PHONE.Length == 10)
                {
                    var part1 = user.MOBILE_PHONE.Substring(0, 3);
                    var part2 = user.MOBILE_PHONE.Substring(3, 3);
                    var part3 = user.MOBILE_PHONE.Substring(6, 4);
                    formatedMobileNumber = "(" + part1 + ") " + part2 + "-" + part3;
                }

                string senderType = _fox_tbl_sender_type.Get(t => !t.DELETED && t.PRACTICE_CODE == AppConfiguration.GetPracticeCode && t.FOX_TBL_SENDER_TYPE_ID == user.FOX_TBL_SENDER_TYPE_ID)?.SENDER_TYPE_NAME ?? "";
                string practiceOrganizationName = _fox_tbl_practice_organization.Get(t => !t.DELETED && t.PRACTICE_CODE == AppConfiguration.GetPracticeCode && t.PRACTICE_ORGANIZATION_ID == user.PRACTICE_ORGANIZATION_ID)?.NAME ?? "";

                bodyOfAdminEmail = File.ReadAllText(templatePathOfAdminEmail);
                bodyOfAdminEmail = bodyOfAdminEmail.Replace("[[NPI]]", user.NPI)
                                   .Replace("[[FIRST_NAME]]", user.FIRST_NAME)
                                   .Replace("[[MOBILE_NO]]", formatedMobileNumber)
                                   .Replace("[[SENDER_TYPE]]", senderType)
                                   .Replace("[[LAST_NAME]]", user.LAST_NAME)
                                   .Replace("[[EMAIL]]", user.EMAIL)
                                   .Replace("[[PRACTICE_NAME]]", user.PRACTICE_ORGANIZATION_TEXT)
                                   .Replace("[[ACO]]", user.ACO_TEXT)
                                   .Replace("[[SPECIALITY]]", user.SPECIALITY_TEXT)
                                   .Replace("[[SNF]]", user.SNF_TEXT)
                                   .Replace("[[HOSPITAL]]", user.HOSPITAL_TEXT)
                                   .Replace("[[HHH]]", user.HHH_TEXT)
                                   .Replace("[[COMMENTS]]", user.COMMENTS);
            }
            string subjectOfAdminEmail = "New signup request: " + user.LAST_NAME + ", " + user.FIRST_NAME + ", " + user.USER_TYPE;
            //string sendTo = "support@foxrehab.org";            
            List<string> _bccListOfAdminEmail = new List<string>();
            List<string> _ccListOfAdminEmail = new List<string>();
            string sendToOfAdminEmail = string.Empty;
            string firstName = string.IsNullOrEmpty(user.FIRST_NAME) ? string.Empty : user.FIRST_NAME;
            string lastName = string.IsNullOrEmpty(user.LAST_NAME) ? string.Empty : user.LAST_NAME;

            if (!Helper.IsTestUser(firstName.ToLower(), lastName.ToLower()) && (AppConfiguration.ClientURL.Contains("https://fox.mtbc.com/") || AppConfiguration.ClientURL.Contains("https://fox2.mtbc.com/")))
            {
                sendToOfAdminEmail = AppConfiguration.SendEmailToAdminOnExternalUserSignUp_To;
                _ccListOfAdminEmail.Add(AppConfiguration.SendEmailToAdminOnExternalUserSignUp_CC);
            }
            else
            {
                sendToOfAdminEmail = AppConfiguration.SendEmailToQAOnExternalUserSignUp_To;
            }
            if (!string.IsNullOrEmpty(sendToOfAdminEmail))
            {
                Helper.SendEmail(sendToOfAdminEmail, subjectOfAdminEmail, bodyOfAdminEmail, null, null, _ccListOfAdminEmail, _bccListOfAdminEmail);
            }
        }

        public void SavePasswordHistory(dynamic user)
        {
            PasswordHistory ph = new PasswordHistory()
            {
                PASSWORD_ID = Helper.getMaximumId("PASSWORD_ID"),
                DELETED = false,
                USER_ID = user.USER_ID,
                PASSWORD = user.PasswordHash,
                CREATED_BY = "self",
                CREATED_DATE = Helper.GetCurrentDate(),
                MODIFIED_BY = "self",
                MODIFIED_DATE = Helper.GetCurrentDate(),
                PRACTICE_CODE = user.PRACTICE_CODE
            };
            _passwordHistoryRepository.Insert(ph);
            _passwordHistoryRepository.Save();
        }

        public void ClearOpenedByinPatientforUser(string UserName)
        {
            var patient = _FoxTblPatientRepository.GetMany(t => t.Is_Opened_By == UserName && t.DELETED == false);
            if (patient != null && patient.Count > 0)
            {
                foreach (var item in patient)
                {
                    item.Is_Opened_By = null;
                    _FoxTblPatientRepository.Update(item);
                    _FoxTblPatientRepository.Save();
                }
            }
        }

        /// <summary>Match current machine IP with USA IP </summary>
        /// <author>Muhammad Arqam </author> 
        /// <param>userName </param>
        /// <return> boolean </return>
        public bool IpConfig(GetUserIP data)
        {
            bool _isValidUser = false;
            string serverName = System.Web.HttpContext.Current?.Request?.Url?.Host;
            string showUrl = HttpContext.Current?.Request?.Url?.AbsoluteUri;
            if (data.userIP != null && data.userIP != "undefined")
            {
                data.userIP = Encrypt.DecrypStringEncryptedInClient(data.userIP);
            }
            else
            {
                data.userIP = "";
            }
            if (data.userName.ToLower().Contains("@foxrehab.org"))
            {
                _isValidUser = true;
                return _isValidUser;
            }
            else
            {
                if (serverName != "localhost" && !showUrl.Contains("172.16.0.207"))
                {
                    if (string.IsNullOrEmpty(data.userIP))
                    {
                        _isValidUser = true;
                        return _isValidUser;
                    }
                    if (!string.IsNullOrEmpty(data.userIP) && data.userIP.Contains(":"))
                    {
                        string getIpV6 = GetUserCountryByIp(data.userIP);
                        if (!string.IsNullOrEmpty(getIpV6) && getIpV6.ToLower().Contains("united states"))
                        {
                            _isValidUser = true;
                        }
                        else
                        {
                            _isValidUser = false;
                        }
                        return _isValidUser;
                    }
                    var checkipObj = CheckIP(data.userName, data.userIP);
                    if (checkipObj.Count <= 0)
                    {
                        _isValidUser = false;
                    }
                    else if (checkipObj.Any() && !string.IsNullOrEmpty(checkipObj[0].VALIDIP.ToString()) && checkipObj[0].VALIDIP.ToString().Trim() != "1")
                    {
                        _isValidUser = false;
                    }
                    else if (checkipObj.Any() && !string.IsNullOrEmpty(checkipObj[0].VALIDIP.ToString()) && checkipObj[0].VALIDIP.ToString().Trim() == "1")
                    {
                        _isValidUser = true;
                    }
                    string directory = System.Web.HttpContext.Current.Server.MapPath("\\FOXUserIpInformation");
                    if (!Directory.Exists(directory))
                    {
                        Directory.CreateDirectory(directory);
                    }
                    string filePath = directory + "\\ipdetail_" + DateTime.Now.Date.ToString("MM-dd-yyyy") + ".txt";
                    using (StreamWriter writer = new StreamWriter(filePath, true))
                    {
                        writer.WriteLine("ServerName: " + serverName + Environment.NewLine + Environment.NewLine + "URI: " + showUrl + Environment.NewLine + Environment.NewLine + "Login Machine IP :" + data.userIP + Environment.NewLine + Environment.NewLine + "User Name :" + data.userName + Environment.NewLine + Environment.NewLine + "Valid User: " + _isValidUser);
                        writer.Close();
                    }
                }
                else
                {
                    _isValidUser = true;
                }
            }

            return _isValidUser;

        }

        /// <summary>Get IPV6 information </summary>
        /// <author>Muhammad Arqam </author> 
        /// <param>ipAddress(Get machine IP), </param>
        public string GetUserCountryByIp(string ipAddress)
        {
            string strReturnVal;
            string ipResponse = IPRequestHelper("http://ip-api.com/xml/" + ipAddress);
            //return ipResponse;
            XmlDocument ipInfoXML = new XmlDocument();
            ipInfoXML.LoadXml(ipResponse);
            XmlNodeList responseXML = ipInfoXML.GetElementsByTagName("query");
            NameValueCollection dataXML = new NameValueCollection();
            dataXML.Add(responseXML.Item(0).ChildNodes[2].InnerText, responseXML.Item(0).ChildNodes[2].Value);
            strReturnVal = responseXML.Item(0).ChildNodes[1].InnerText.ToString(); // Contry
            return strReturnVal;
        }

        /// <summary>Get IPV6 information </summary>
        /// <author>Muhammad Arqam </author> 
        /// <param>url(Pass url), </param>
        public string IPRequestHelper(string url)
        {

            HttpWebRequest objRequest = (HttpWebRequest)WebRequest.Create(url);
            HttpWebResponse objResponse = (HttpWebResponse)objRequest.GetResponse();

            StreamReader responseStream = new StreamReader(objResponse.GetResponseStream());
            string responseRead = responseStream.ReadToEnd();

            responseStream.Close();
            responseStream.Dispose();

            return responseRead;
        }
        /// <summary>Match current machine IP with USA IP through SP </summary>
        /// <author>Muhammad Arqam </author> 
        /// <param>userName(Who is login), ipAddress(Get machine IP), </param>
        public List<WEB_PROC_ADI_VALIDATEUSERIPResult> CheckIP(string userName, string ipAddress)
        {
            var User_Name = Helper.getDBNullOrValue("@USERNAME", userName.ToString());
            var User_IP = Helper.getDBNullOrValue("@USERIP", ipAddress.ToString());
            var result = SpRepository<WEB_PROC_ADI_VALIDATEUSERIPResult>.GetListWithStoreProcedure(@"exec WEB_PROC_ADI_VALIDATEUSERIP @USERNAME, @USERIP", User_Name, User_IP);
            if (result == null && result.Count == 0)
            {
                result[0].VALIDIP = 0;
                return result;
            }
            if (result.Any() && !string.IsNullOrEmpty(result[0].VALIDIP.ToString()) && result[0].VALIDIP.ToString().Trim() == "1")
            {
                var UserName = Helper.getDBNullOrValue("@USERNAME", userName.ToString());
                var UserIPP = Helper.getDBNullOrValue("@USERIP", ipAddress.ToString());
                var ISBresult = SpRepository<WEB_PROC_ADI_VALIDATEUSERIPResult>.GetListWithStoreProcedure(@"exec WEB_PROC_ONLYISB_VALIDATEUSERIP @USERNAME, @USERIP", UserName, UserIPP);
                if (ISBresult.Any() && !string.IsNullOrEmpty(ISBresult[0].VALIDIP.ToString()) && ISBresult[0].VALIDIP.ToString().Trim() == "1")
                {
                    result[0].VALIDIP = 1;
                    return result;
                }
                else if (ISBresult.Any() && !string.IsNullOrEmpty(ISBresult[0].VALIDIP.ToString()) && ISBresult[0].VALIDIP.ToString().Trim() != "1")
                {
                    var UserIP = Helper.getDBNullOrValue("@USERIP", ipAddress.ToString());
                    var USresult = SpRepository<WEB_PROC_ADI_VALIDATEUSERIPResult>.GetListWithStoreProcedure(@"exec Web_Proc_GetCountryLong @USERIP", UserIP);
                    if (USresult.Any() && !string.IsNullOrEmpty(USresult[0].countryLONG) && USresult[0].countryLONG.ToLower().Contains("united states"))
                    {
                        result[0].VALIDIP = 1;
                        return result;
                    }
                    else
                    {
                        result[0].VALIDIP = 0;
                        return result;
                    }
                }
            }
            else if (result.Any() && !string.IsNullOrEmpty(result[0].VALIDIP.ToString()) && result[0].VALIDIP.ToString().Trim() != "1")
            {
                result[0].VALIDIP = 0;
                return result;
            }
            return result;
        }

        public ResponseModel SignOut(LogoutModal obj, UserProfile profile)
        {
            try
            {
                ResponseModel resp = new ResponseModel();
                ProfileToken token = new ProfileToken();
                if (obj.token != null)
                {
                    var paramAuthToken = new SqlParameter { ParameterName = "AUTHTOKEN", SqlDbType = SqlDbType.NVarChar, Value = obj.token };
                    var paramUserID = new SqlParameter { ParameterName = "USERID", SqlDbType = SqlDbType.NVarChar, Value = profile.userID };
                    token = SpRepository<ProfileToken>.GetSingleObjectWithStoreProcedure(@"EXEC FOX_PROC_GET_PROFILE_TOKEN @AUTHTOKEN, @USERID", paramAuthToken, paramUserID);

                    if (token != null)
                    {
                        var tokenSecurityID = Helper.getMaximumId("Fox_TokenSecurityID");
                        var paramSecurityID = new SqlParameter { ParameterName = "TokenSecurityID", SqlDbType = SqlDbType.BigInt, Value = tokenSecurityID };
                        var paramtokenAuthToken = new SqlParameter { ParameterName = "AUTHTOKEN", SqlDbType = SqlDbType.NVarChar, Value = token.AuthToken };
                        var paramtokenIssuedOn = new SqlParameter { ParameterName = "ISSUEDON", SqlDbType = SqlDbType.NVarChar, Value = token.IssuedOn };
                        var paramtokenUserName = new SqlParameter { ParameterName = "USER_NAME", SqlDbType = SqlDbType.NVarChar, Value = profile.UserName };
                        var profileTokenResponse = SpRepository<ProfileTokensSecurity>.GetSingleObjectWithStoreProcedure(@"EXEC FOX_PROC_INSERT_TOKEN_ON_LOGOUT @TokenSecurityID, @AUTHTOKEN, @ISSUEDON, @USER_NAME",
                            paramSecurityID, paramtokenAuthToken, paramtokenIssuedOn, paramtokenUserName);

                        token.ExpiresOn = DateTime.Now;
                        profileToken.Update(token);
                        profileToken.Save();
                        resp.Success = true;
                        return resp;
                    }
                }
                if (token == null && profile.isTalkRehab)
                {
                    resp.Success = true;
                    return resp;
                }
                resp.Success = false;
                return resp;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void InsertLogs(dynamic user, string encryptedPassword, string detectedBrowser, string requestType)
        {
            try
            {
                if (user != null && !string.IsNullOrWhiteSpace(encryptedPassword) && !string.IsNullOrWhiteSpace(detectedBrowser) && !string.IsNullOrWhiteSpace(requestType))
                {
                    //Creating Logs For Invalid Login Request of Internal User
                    if (requestType.Contains("Invalid Internal Login") && (user.EMAIL.ToLower().Contains("@foxrehab.org") || user.EMAIL.ToLower().Contains("@rmb.com") || user.EMAIL.ToLower().Contains("@gulfcoastbilling.com")))
                    {
                        WS_TBL_FOX_Login_LOGS insertLogs = new WS_TBL_FOX_Login_LOGS()
                        {
                            UserName = user.EMAIL,
                            Password = encryptedPassword,
                            DeviceInfo = (EntityHelper.isTalkRehab) ? "CC_Remote_Portal. App_Name: MTBC CC Remote. Browser: " + detectedBrowser : "Fox_Portal. App_Name: MTBC Fox Portal. Browser: " + detectedBrowser,
                            AdResponse = string.Empty,
                            ServiceResponse = string.Empty,
                            CreatedDate = Helper.GetCurrentDate(),
                            CreatedBy = (EntityHelper.isTalkRehab) ? "CC_REMOTE" : "Fox_Portal",
                            FirstName = user.FIRST_NAME,
                            LastName = user.LAST_NAME,
                            UserType = "Internal User",
                            AdLoginResponse = "Invalid Credential"
                        };
                        _loginLogsRepository.Insert(insertLogs);
                        _loginLogsRepository.Save();
                    }
                    //Creating Logs for Valid Login Request of Internal User
                    else if (requestType.Contains("Valid Internal Login") && user.ApplicationUserRoles != null && (user.Email.ToLower().Contains("@foxrehab.org") || user.Email.ToLower().Contains("@rmb.com") || user.Email.ToLower().Contains("@gulfcoastbilling.com")))
                    {
                        WS_TBL_FOX_Login_LOGS insertLogs = new WS_TBL_FOX_Login_LOGS()
                        {
                            UserName = user.Email,
                            Password = encryptedPassword,
                            DeviceInfo = (EntityHelper.isTalkRehab) ? "CC_Remote_Portal. App_Name: MTBC CC Remote. Browser: " + detectedBrowser : "Fox_Portal. App_Name: MTBC Fox Portal. Browser: " + detectedBrowser,
                            AdResponse = string.Empty,
                            ServiceResponse = string.Empty,
                            CreatedDate = Helper.GetCurrentDate(),
                            CreatedBy = (EntityHelper.isTalkRehab) ? "CC_REMOTE" : "Fox_Portal",
                            FirstName = user.FIRST_NAME,
                            LastName = user.LAST_NAME,
                            UserType = "Internal User",
                            AdLoginResponse = "User Login Successfully"
                        };
                        _loginLogsRepository.Insert(insertLogs);
                        _loginLogsRepository.Save();
                    }


                    //Creating Logs For a Valid Login Request of External User
                    else if (requestType.Contains("Login") && user.ApplicationUserRoles != null)
                    {
                        WS_TBL_FOX_Login_LOGS insertLogs = new WS_TBL_FOX_Login_LOGS()
                        {
                            UserName = user.UserEmailAddress,
                            Password = encryptedPassword,
                            DeviceInfo = (EntityHelper.isTalkRehab) ? "CC_Remote_Portal. App_Name: MTBC CC Remote. Browser: " + detectedBrowser : "Fox_Portal. App_Name: MTBC Fox Portal. Browser: " + detectedBrowser,
                            AdResponse = string.Empty,
                            ServiceResponse = string.Empty,
                            CreatedDate = Helper.GetCurrentDate(),
                            CreatedBy = (EntityHelper.isTalkRehab) ? "CC_REMOTE" : "Fox_Portal",
                            FirstName = user.FirstName,
                            LastName = user.LastName,
                            UserType = "External User",
                            AdLoginResponse = "User Login Successfully"
                        };
                        _loginLogsRepository.Insert(insertLogs);
                        _loginLogsRepository.Save();
                    }

                    //Creating Logs For Invalid Login Request of External User
                    else if (requestType.Contains("Login") && user.ApplicationUserRoles == null)
                    {
                        WS_TBL_FOX_Login_LOGS insertLogs = new WS_TBL_FOX_Login_LOGS()
                        {
                            UserName = user.UserEmailAddress,
                            Password = encryptedPassword,
                            DeviceInfo = (EntityHelper.isTalkRehab) ? "CC_Remote_Portal. App_Name: MTBC CC Remote. Browser: " + detectedBrowser : "Fox_Portal. App_Name: MTBC Fox Portal. Browser: " + detectedBrowser,
                            AdResponse = string.Empty,
                            ServiceResponse = string.Empty,
                            CreatedDate = Helper.GetCurrentDate(),
                            CreatedBy = (EntityHelper.isTalkRehab) ? "CC_REMOTE" : "Fox_Portal",
                            FirstName = user.FirstName,
                            LastName = user.LastName,
                            UserType = "External User",
                            AdLoginResponse = "Invalid Credential"
                        };
                        _loginLogsRepository.Insert(insertLogs);
                        _loginLogsRepository.Save();
                    }

                    //Creating Logs for Update Request of External User User
                    else if (requestType.Contains("Update User"))
                    {
                        WS_TBL_FOX_Login_LOGS insertLogs = new WS_TBL_FOX_Login_LOGS()
                        {
                            UserName = user.Email,
                            Password = encryptedPassword,
                            DeviceInfo = (EntityHelper.isTalkRehab) ? "CC_Remote_Portal. App_Name: MTBC CC Remote. Browser: " + detectedBrowser : "Fox_Portal. App_Name: MTBC Fox Portal. Browser: " + detectedBrowser,
                            AdResponse = string.Empty,
                            ServiceResponse = string.Empty,
                            CreatedDate = Helper.GetCurrentDate(),
                            CreatedBy = (EntityHelper.isTalkRehab) ? "CC_REMOTE" : "Fox_Portal",
                            FirstName = user.FirstName,
                            LastName = user.LastName,
                            UserType = "External User",
                            AdLoginResponse = "User Login Successfully"
                        };
                        _loginLogsRepository.Insert(insertLogs);
                        _loginLogsRepository.Save();
                    }

                    //Creating Logs For User Registration Request of External User
                    else
                    {
                        WS_TBL_FOX_Login_LOGS insertLogs = new WS_TBL_FOX_Login_LOGS()
                        {
                            UserName = user.Email,
                            Password = encryptedPassword,
                            DeviceInfo = (EntityHelper.isTalkRehab) ? "CC_Remote_Portal. App_Name: MTBC CC Remote. Browser: " + detectedBrowser : "Fox_Portal. App_Name: MTBC Fox Portal. Browser: " + detectedBrowser,
                            AdResponse = string.Empty,
                            ServiceResponse = string.Empty,
                            CreatedDate = Helper.GetCurrentDate(),
                            CreatedBy = (EntityHelper.isTalkRehab) ? "CC_REMOTE" : "Fox_Portal",
                            FirstName = user.FIRST_NAME,
                            LastName = user.LAST_NAME,
                            UserType = "External User",
                            AdLoginResponse = "User Login Successfully"
                        };
                        _loginLogsRepository.Insert(insertLogs);
                        _loginLogsRepository.Save();
                    }
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

    }
}