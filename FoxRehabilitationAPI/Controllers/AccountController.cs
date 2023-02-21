using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;
using FoxRehabilitationAPI.Models;
using FoxRehabilitationAPI.Providers;
using FoxRehabilitationAPI.Results;
using System.Linq;
using FOX.BusinessOperations.SettingsService.UserMangementService;
using FOX.BusinessOperations.AccountService;
using FOX.BusinessOperations.CommonService;
using FOX.BusinessOperations.RequestForOrder;
using System.Net;
using FOX.DataModels.Models.Security;
using FOX.DataModels.Models.ExternalUserModel;
using FOX.DataModels.Models.Settings.RoleAndRights;
using FoxRehabilitationAPI.Filters;
using FOX.BusinessOperations.CommonServices;
using FOX.BusinessOperations.Security;
using System.Text;
using System.IO;
using FOX.DataModels.Models.CommonModel;
using FOX.BusinessOperations.IndexInfoServices;
using System.Data.Entity;
using System.Data.SqlClient;
using FOX.DataModels.Context;
using System.Data;
using FOX.DataModels.GenericRepository;
using FOX.DataModels.Models.GoogleRecaptcha;
using Newtonsoft.Json;
using RestSharp;
using System.Web.Configuration;

namespace FoxRehabilitationAPI.Controllers
{
    [Authorize]
    [RoutePrefix("api/Account")]
    [ExceptionHandlingFilter]
    public class AccountController : ApiController
    {
        private readonly IUserManagementService _userManagementService = new UserManagementService();
        private readonly IRequestForOrderService _requestForOrderService = new RequestForOrderService();
        private readonly IIndexInfoService _IndexInfoService = new IndexInfoService();
        private readonly IAccountServices _accountServices = new AccountServices();
        private readonly DbContextSecurity security = new DbContextSecurity();
        private readonly GenericRepository<User> _UserRepository;
        private const string LocalLoginProvider = "Local";
        private ApplicationUserManager _userManager;
        private ApplicationRoleManager _roleManager;
        public AccountController()
        {
            _UserRepository = new GenericRepository<User>(security);
        }

        private UserProfile GetProfile()
        {
            return ClaimsModel.GetUserProfile(User.Identity as System.Security.Claims.ClaimsIdentity) ?? new UserProfile();
        }

        public AccountController(ApplicationUserManager userManager, ApplicationRoleManager roleManager, ISecureDataFormat<AuthenticationTicket> accessTokenFormat)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            AccessTokenFormat = accessTokenFormat;
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? Request?.GetOwinContext()?.GetUserManager<ApplicationUserManager>() ?? null;
            }
            private set
            {
                _userManager = value;
            }
        }
        protected ApplicationRoleManager AppRoleManager
        {
            get
            {
                return _roleManager ?? Request.GetOwinContext().GetUserManager<ApplicationRoleManager>();
            }
        }

        public ISecureDataFormat<AuthenticationTicket> AccessTokenFormat { get; private set; }

        // GET api/Account/UserInfo
        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [Route("UserInfo")]
        public UserInfoViewModel GetUserInfo()
        {
            ExternalLoginData externalLogin = ExternalLoginData.FromIdentity(User.Identity as ClaimsIdentity);

            return new UserInfoViewModel
            {
                Email = User.Identity.GetUserName(),
                HasRegistered = externalLogin == null,
                LoginProvider = externalLogin != null ? externalLogin.LoginProvider : null
            };
        }
        //[Route("Signout")]
        [HttpPost]
        public ResponseModel SignOut(LogoutModal obj)
        {
            return _accountServices.SignOut(obj, GetProfile());
            //Request.GetOwinContext().Response.Headers.
            //Request.GetOwinContext().Authentication.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            //return Ok();
        }
        // POST api/Account/Logout
        [Route("Logout")]
        public IHttpActionResult Logout()
        {
            Authentication.SignOut(CookieAuthenticationDefaults.AuthenticationType);
            return Ok();
        }

        // GET api/Account/ManageInfo?returnUrl=%2F&generateState=true
        [Route("ManageInfo")]
        public async Task<ManageInfoViewModel> GetManageInfo(string returnUrl, bool generateState = false)
        {
            IdentityUser user = await UserManager.FindByIdAsync(User.Identity.GetUserId());

            if (user == null)
            {
                return null;
            }

            List<UserLoginInfoViewModel> logins = new List<UserLoginInfoViewModel>();

            foreach (IdentityUserLogin linkedAccount in user.Logins)
            {
                logins.Add(new UserLoginInfoViewModel
                {
                    LoginProvider = linkedAccount.LoginProvider,
                    ProviderKey = linkedAccount.ProviderKey
                });
            }

            if (user.PasswordHash != null)
            {
                logins.Add(new UserLoginInfoViewModel
                {
                    LoginProvider = LocalLoginProvider,
                    ProviderKey = user.UserName,
                });
            }

            return new ManageInfoViewModel
            {
                LocalLoginProvider = LocalLoginProvider,
                Email = user.UserName,
                Logins = logins,
                ExternalLoginProviders = GetExternalLogins(returnUrl, generateState)
            };
        }

        // POST api/Account/ChangePassword
        [Route("ChangePassword")]
        public async Task<IHttpActionResult> ChangePassword(ChangePasswordBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            IdentityResult result = await UserManager.ChangePasswordAsync(User.Identity.GetUserId(), model.OldPassword,
                model.NewPassword);

            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }

            return Ok();
        }

        // POST api/Account/SetPassword
        [Route("SetPassword")]
        public async Task<IHttpActionResult> SetPassword(SetPasswordBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            IdentityResult result = await UserManager.AddPasswordAsync(User.Identity.GetUserId(), model.NewPassword);

            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }

            return Ok();
        }

        // POST api/Account/AddExternalLogin
        [Route("AddExternalLogin")]
        public async Task<IHttpActionResult> AddExternalLogin(AddExternalLoginBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Authentication.SignOut(DefaultAuthenticationTypes.ExternalCookie);

            AuthenticationTicket ticket = AccessTokenFormat.Unprotect(model.ExternalAccessToken);

            if (ticket == null || ticket.Identity == null || (ticket.Properties != null
                && ticket.Properties.ExpiresUtc.HasValue
                && ticket.Properties.ExpiresUtc.Value < DateTimeOffset.UtcNow))
            {
                return BadRequest("External login failure.");
            }

            ExternalLoginData externalData = ExternalLoginData.FromIdentity(ticket.Identity);

            if (externalData == null)
            {
                return BadRequest("The external login is already associated with an account.");
            }

            IdentityResult result = await UserManager.AddLoginAsync(User.Identity.GetUserId(),
                new UserLoginInfo(externalData.LoginProvider, externalData.ProviderKey));

            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }

            return Ok();
        }

        // POST api/Account/RemoveLogin
        [Route("RemoveLogin")]
        public async Task<IHttpActionResult> RemoveLogin(RemoveLoginBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            IdentityResult result;

            if (model.LoginProvider == LocalLoginProvider)
            {
                result = await UserManager.RemovePasswordAsync(User.Identity.GetUserId());
            }
            else
            {
                result = await UserManager.RemoveLoginAsync(User.Identity.GetUserId(),
                    new UserLoginInfo(model.LoginProvider, model.ProviderKey));
            }

            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }

            return Ok();
        }

        // GET api/Account/ExternalLogin
        [OverrideAuthentication]
        [HostAuthentication(DefaultAuthenticationTypes.ExternalCookie)]
        [AllowAnonymous]
        [Route("ExternalLogin", Name = "ExternalLogin")]
        public async Task<IHttpActionResult> GetExternalLogin(string provider, string error = null)
        {
            if (error != null)
            {
                return Redirect(Url.Content("~/") + "#error=" + Uri.EscapeDataString(error));
            }

            if (!User.Identity.IsAuthenticated)
            {
                return new ChallengeResult(provider, this);
            }

            ExternalLoginData externalLogin = ExternalLoginData.FromIdentity(User.Identity as ClaimsIdentity);

            if (externalLogin == null)
            {
                return InternalServerError();
            }

            if (externalLogin.LoginProvider != provider)
            {
                Authentication.SignOut(DefaultAuthenticationTypes.ExternalCookie);
                return new ChallengeResult(provider, this);
            }

            ApplicationUser user = await UserManager.FindAsync(new UserLoginInfo(externalLogin.LoginProvider,
                externalLogin.ProviderKey));

            bool hasRegistered = user != null;

            if (hasRegistered)
            {
                Authentication.SignOut(DefaultAuthenticationTypes.ExternalCookie);

                ClaimsIdentity oAuthIdentity = await user.GenerateUserIdentityAsync(UserManager,
                   OAuthDefaults.AuthenticationType);
                ClaimsIdentity cookieIdentity = await user.GenerateUserIdentityAsync(UserManager,
                    CookieAuthenticationDefaults.AuthenticationType);
                AuthenticationProperties properties = ApplicationOAuthProvider.CreateProperties(user.UserName);
                Authentication.SignIn(properties, oAuthIdentity, cookieIdentity);
            }
            else
            {
                IEnumerable<Claim> claims = externalLogin.GetClaims();
                ClaimsIdentity identity = new ClaimsIdentity(claims, OAuthDefaults.AuthenticationType);
                Authentication.SignIn(identity);
            }

            return Ok();
        }

        // GET api/Account/ExternalLogins?returnUrl=%2F&generateState=true
        [AllowAnonymous]
        [Route("ExternalLogins")]
        public IEnumerable<ExternalLoginViewModel> GetExternalLogins(string returnUrl, bool generateState = false)
        {
            IEnumerable<AuthenticationDescription> descriptions = Authentication.GetExternalAuthenticationTypes();
            List<ExternalLoginViewModel> logins = new List<ExternalLoginViewModel>();

            string state;

            if (generateState)
            {
                const int strengthInBits = 256;
                state = RandomOAuthStateGenerator.Generate(strengthInBits);
            }
            else
            {
                state = null;
            }

            foreach (AuthenticationDescription description in descriptions)
            {
                ExternalLoginViewModel login = new ExternalLoginViewModel
                {
                    Name = description.Caption,
                    Url = Url.Route("ExternalLogin", new
                    {
                        provider = description.AuthenticationType,
                        response_type = "token",
                        client_id = Startup.PublicClientId,
                        redirect_uri = new Uri(Request.RequestUri, returnUrl).AbsoluteUri,
                        state = state
                    }),
                    State = state
                };
                logins.Add(login);
            }

            return logins;
        }

        //private int insertUpdateFOX_TBL_APP_USER_ADDITIONAL_INFO(long UserId, bool isElectronicPOC, DateTime CreatedDate, string CreatedBy, DateTime ModifiedDate, string ModifiedBy, bool Deleted)
        //{
        //    string query = @"exec SP_FOX_TBL_APP_USER_ADDITIONAL_INFO @USER_Id,@Is_Electronic_POC,@CREATED_DATE,@CREATED_BY,@MODIFIED_DATE,@MODIFIED_BY,@DELETED";
        //        //params SqlParameter[] parameters = new 
        //        //long id = Helper.getMaximumId("FOX_TBL_APPLICATION_USER_Id");
        //        //SqlParameter Id = new SqlParameter("Id", SqlDbType.BigInt) { Value = id };
        //        SqlParameter FOX_TBL_APPLICATION_USER_Id = new SqlParameter("@USER_Id", SqlDbType.BigInt) { Value = UserId };
        //        SqlParameter Is_Electronic_POC = new SqlParameter("@Is_Electronic_POC", SqlDbType.Bit) { Value = isElectronicPOC };
        //        SqlParameter CREATED_DATE = new SqlParameter("@CREATED_DATE", SqlDbType.DateTime) { Value = CreatedDate };
        //        SqlParameter CREATED_BY = new SqlParameter("@CREATED_BY", SqlDbType.VarChar) { Value = CreatedBy };
        //        SqlParameter MODIFIED_DATE = new SqlParameter("@MODIFIED_DATE", SqlDbType.DateTime) { Value = ModifiedDate };
        //        SqlParameter MODIFIED_BY = new SqlParameter("@MODIFIED_BY", SqlDbType.VarChar) { Value = ModifiedBy };
        //        SqlParameter DELETED = new SqlParameter("@DELETED", SqlDbType.Bit) { Value = Deleted };
        //    //return context.Database.ExecuteSqlCommand(query, list);
        //    var result = SpRepository<object>.GetSingleObjectWithStoreProcedure(query, FOX_TBL_APPLICATION_USER_Id, Is_Electronic_POC, CREATED_DATE,
        //   CREATED_BY, MODIFIED_DATE, MODIFIED_BY, DELETED);
        //    return 1;
        //}
        [AllowAnonymous]
        [Route("ExternalUserSignUp")]
        public async Task<HttpResponseMessage> ExternalUserSignUp(ApplicationUser user)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Select(x => x.Value.Errors).Where(y => y.Count > 0).ToList();
                return Request.CreateResponse(new ExternalUserSignupResponseModel() { status = 0, ErrorMessage = string.Join(",", errors), Success = false });
            }
            string password = HttpUtility.UrlDecode(user.PASSWORD);


            if (!_accountServices.CheckIfEmailAlreadyInUse(new EmailExist() { EMAIL = user.EMAIL }))
            {

                user.USER_ID = Helper.getMaximumId("USER_ID");
                user.USER_NAME = $"{user.LAST_NAME.Trim()}_{user.USER_ID}";
                user.IS_APPROVED = false;
                user.IS_ACTIVE = false;
                user.MODIFIED_BY = user.CREATED_BY = "FOX_WEB";
                user.CREATED_DATE = Helper.GetCurrentDate();
                user.MODIFIED_DATE = Helper.GetCurrentDate();
                user.PRACTICE_CODE = AppConfiguration.GetPracticeCode;
                user.SIGNATURE_PATH = user.SIGNATURE_PATH;
                user.PASSWORD = Encrypt.getEncryptedCode(password);
                user.IS_AD_USER = false;
                string encryptedPassword = Encrypt.EncryptPassword(password);

                IdentityResult result = await UserManager.CreateAsync(user, password);
                if (!result.Succeeded)
                {
                    return Request.CreateResponse(new ExternalUserSignupResponseModel() { status = 200, ErrorMessage = string.Join(",", result.Errors), Success = true });
                }
                _accountServices.EmailToAdmin(user);
                _accountServices.SavePasswordHistory(user);
                _userManagementService.AddUpdateUserAdditionalInfo(user.USER_ID, user.Is_Electronic_POC == null ? false : true, user.CREATED_DATE, user.CREATED_BY, user.MODIFIED_DATE, user.MODIFIED_BY, user.DELETED); ;
                //Browser Detection
                string detectedBrowser = BrowserDetection();
                _accountServices.InsertLogs(user, encryptedPassword, detectedBrowser, "Registration");
                //object rows = insertUpdateFOX_TBL_APP_USER_ADDITIONAL_INFO(user.USER_ID, user.Is_Electronic_POC == null ? false : user.Is_Electronic_POC.Value, user.CREATED_DATE, user.CREATED_BY, user.MODIFIED_DATE, user.MODIFIED_BY, user.DELETED);
                return Request.CreateResponse(new ExternalUserSignupResponseModel() { status = 200, ErrorMessage = "Account registration request submitted successfully. You'll be notified by an SMS or email once your request is processed. ", Message = "Account registration request submitted successfully. You'll be notified by an SMS or email once your request is processed.", Success = true });

            }
            else { return Request.CreateResponse(new ExternalUserSignupResponseModel() { status = 200, ErrorMessage = "An error has occurred", Success = true }); }



            //else
            //{
            //    ApplicationUser existingUser = UserManager.Users.Where(u => u.Email == user.Email).FirstOrDefault();
            //    existingUser.USER_NAME = $"{existingUser.LAST_NAME}_{existingUser.USER_ID}";
            //    existingUser.NPI = user.NPI;
            //    existingUser.FIRST_NAME = user.FIRST_NAME;
            //    existingUser.LAST_NAME = user.LAST_NAME;
            //    existingUser.PASSWORD = Encrypt.getEncryptedCode(user.PASSWORD);
            //    existingUser.MOBILE_PHONE = user.MOBILE_PHONE;
            //    existingUser.FOX_TBL_SENDER_TYPE_ID = user.FOX_TBL_SENDER_TYPE_ID;
            //    existingUser.GENDER = user.GENDER;
            //    existingUser.ADDRESS_1 = user.ADDRESS_1;
            //    existingUser.ZIP = user.ZIP;
            //    existingUser.CITY = user.CITY;
            //    existingUser.STATE = user.STATE;
            //    existingUser.TIME_ZONE = user.TIME_ZONE;
            //    existingUser.WORK_PHONE = user.WORK_PHONE;
            //    existingUser.PRACTICE_ORGANIZATION_TEXT = user.PRACTICE_ORGANIZATION_TEXT;
            //    existingUser.HHH_TEXT = user.HHH_TEXT;
            //    existingUser.ACO_TEXT = user.ACO_TEXT;
            //    existingUser.SPECIALITY_TEXT = user.SPECIALITY_TEXT;
            //    existingUser.SNF_TEXT = user.SNF_TEXT;
            //    existingUser.HOSPITAL_TEXT = user.HOSPITAL_TEXT;
            //    existingUser.THIRD_PARTY_REFERRAL_SOURCE = user.THIRD_PARTY_REFERRAL_SOURCE;
            //    existingUser.COMMENT = user.COMMENT;
            //    existingUser.PRACTICE_CODE = AppConfiguration.GetPracticeCode;
            //    existingUser.IS_APPROVED = false;
            //    existingUser.IS_ACTIVE = false;
            //    existingUser.CREATED_BY = "FOX_WEB";
            //    existingUser.MODIFIED_BY = existingUser.USER_NAME;
            //    existingUser.CREATED_DATE = Helper.GetCurrentDate();
            //    existingUser.MODIFIED_DATE = Helper.GetCurrentDate();
            //    existingUser.SIGNATURE_PATH = user.SIGNATURE_PATH;
            //    existingUser.DELETED = false;
            //    existingUser.PasswordHash = UserManager.PasswordHasher.HashPassword(user.PASSWORD);
            //    existingUser.ROLE_ID = null;
            //    existingUser.Is_Electronic_POC = user.Is_Electronic_POC;
            //    IdentityResult result = await UserManager.UpdateAsync(existingUser);
            //    if (!result.Succeeded)
            //    {
            //        return Request.CreateResponse(new ExternalUserSignupResponseModel() { status = 0, ErrorMessage = string.Join(",", result.Errors), Success = false });
            //    }
            //    _accountServices.EmailToAdmin(user);
            //    _accountServices.SavePasswordHistory(user);
            //    _userManagementService.AddUpdateUserAdditionalInfo(existingUser.USER_ID, user.Is_Electronic_POC == null ? false : user.Is_Electronic_POC.Value, existingUser.CREATED_DATE, existingUser.CREATED_BY, existingUser.MODIFIED_DATE, existingUser.MODIFIED_BY, existingUser.DELETED); ;
            //    //object rows = insertUpdateFOX_TBL_APP_USER_ADDITIONAL_INFO(user.USER_ID, user.Is_Electronic_POC == null ? false : user.Is_Electronic_POC.Value, user.CREATED_DATE, user.CREATED_BY, user.MODIFIED_DATE, user.MODIFIED_BY, user.DELETED);
            //    return Request.CreateResponse(new ExternalUserSignupResponseModel() { status = 1, ErrorMessage = "Account registration request submitted successfully. You'll be notified by an SMS or email once your request is processed. ", Message = "Account registration request submitted successfully. You'll be notified by an SMS or email once your request is processed.", Success = true });
            //}
        }

        // POST api/Account/GetOtp
        [AllowAnonymous]
        [Route("GetOtp")]
        public HttpResponseMessage GetOtp(string email)
        {
            if (String.IsNullOrEmpty(email))
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Request with invalid parameters.");
            }
            else
            {
                IRestResponse response = GetOtpCode(email);
                if (!String.IsNullOrEmpty(response.Content))
                {
                    return Request.CreateResponse(HttpStatusCode.OK, response.Content);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "Error occured to get OTP code.");
                }
            }
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("VerifyOTP")]
        public HttpResponseMessage VerifyOTP(string otp, string otpIdentifier)
        {
            if (String.IsNullOrEmpty(otp) || String.IsNullOrEmpty(otpIdentifier))
            {
                if (String.IsNullOrEmpty(otp) && String.IsNullOrEmpty(otpIdentifier))
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "Request with invalid parameters otp and otpIdentifier .");
                }
                else if (String.IsNullOrEmpty(otp))
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "Request with invalid otp .");
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "Request with invalid otpIdentifier .");
                }
            }
            else
            {
                IRestResponse response = VerifyOtpCode(otp, otpIdentifier);
                if (response.Content != null)
                {
                    OtpModel obj=JsonConvert.DeserializeObject<OtpModel>(response.Content);
                    if (obj != null)
                    {
                        if (obj.status == true)
                        {
                            UserProfile profile = ClaimsModel.GetUserProfile(User.Identity as System.Security.Claims.ClaimsIdentity) ?? new UserProfile();
                            ResponseModel resp =_userManagementService.UpdateOtpEnableDate(profile.userID);
                            if (resp != null)
                            {
                                return Request.CreateResponse(HttpStatusCode.OK, response.Content);
                            }
                        }
                        else
                        {
                            if (obj.message.Contains("failed"))
                            {
                                obj.message = "OTP verification failed.";
                            }
                        }
                    }
                    var result = Newtonsoft.Json.JsonConvert.SerializeObject(obj);
                    return Request.CreateResponse(HttpStatusCode.OK, result);
                }
                else
                { 
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "Error occured to verify OTP code.");
                }
            }
        }


        [HttpGet]
        [AllowAnonymous]
        [Route("UpdateOtpEnableDate")]
        public HttpResponseMessage UpdateOtpEnableDate(string userId)
        {
            if (String.IsNullOrEmpty(userId))
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Request with invalid parameter.");
            }
            ResponseModel resp =_userManagementService.UpdateOtpEnableDate(Convert.ToInt64(userId));
            if (resp != null)
            { 
                return Request.CreateResponse(HttpStatusCode.OK, resp);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Error occured to update OTP enable date.");
            }

        }


        [HttpGet]
        [AllowAnonymous]
        [Route("UpdateMfaStatus")]
        public HttpResponseMessage UpdateMfaStatus(string userId)
        {
            if (String.IsNullOrEmpty(userId))
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Request with invalid parameter.");
            }
            ResponseModel resp =_userManagementService.UpdateMfaStatus(userId);
            if (resp != null)
            { 
                return Request.CreateResponse(HttpStatusCode.OK, resp);
            }
            else
            { 
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Error occured to update OTP enable date."); 
            }

        }
        //Browser Detection
        public static string BrowserDetection()
        {
            string browserName = "";
            string browserVersion = "";
            string plateForm = "";
            string browser = "";
            bool IsMobileDevice = false;
            if (HttpContext.Current.Request != null)
            {
                if (HttpContext.Current.Request.Browser != null)
                {
                    if (HttpContext.Current.Request.UserAgent.ToLower().Contains("edge") && HttpContext.Current.Request.Browser.Browser.ToLower() != "edge")
                    {
                        browserName = "Microsoft Edge";
                    }
                    else
                    {
                        browserName = HttpContext.Current.Request.Browser.Browser;
                        browserVersion = HttpContext.Current.Request.Browser.Version.ToString();
                    }
                    browser = HttpContext.Current.Request.Browser.ToString();
                    plateForm = HttpContext.Current.Request.Browser.Platform;
                    IsMobileDevice = HttpContext.Current.Request.Browser.IsMobileDevice;
                    if (IsMobileDevice)
                    {
                        browserName = HttpContext.Current.Request.Browser.Browser;

                    }

                }
            }
            if (!string.IsNullOrWhiteSpace(browserVersion))
            {
                return browserName + " " + browserVersion;
            }
            else
            {
                return browserName;
            }
        }


        // POST api/Account/RegisterExternal
        [OverrideAuthentication]
        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [Route("RegisterExternal")]
        public async Task<IHttpActionResult> RegisterExternal(RegisterExternalBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var info = await Authentication.GetExternalLoginInfoAsync();
            if (info == null)
            {
                return InternalServerError();
            }

            var user = new ApplicationUser() { UserName = model.Email, Email = model.Email };

            IdentityResult result = await UserManager.CreateAsync(user);
            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }

            result = await UserManager.AddLoginAsync(user.Id, info.Login);
            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }
            return Ok();
        }

        [AllowAnonymous]
        [Route("ValidateUserEmail")]
        public async Task<ResponseModel> ValidateUserEmail(EmailVerificationModel data)
        {
            //string link = "http://localhost:14479/#/account/resetpassword?value=";
            //string link = "http://172.16.0.207:6906/#/account/resetpassword?value=";
            //string link = "http://uatfox.mtbc.com/#/account/resetpassword?value=";
            string link = "https://fox.mtbc.com//#/account/resetpassword?value=";
            //string link = "http://172.16.0.207:7831/#/account/resetpassword?value=";
            //string link = "http://172.16.0.207:8195/#/account/resetpassword?value=";

            string _body = string.Empty;
            string _subject = "Reset Password for your FOX Portal";
            string sendTo = string.Empty;
            List<string> _bccList = new List<string>();
            sendTo = data.Email;
            EmailValidationRes obj = new EmailValidationRes();
            ResponseModel resp = new ResponseModel();
            GoogleRecaptchaResponse response = await ValidateCaptcha(data.EncodedResponse);
            if (!response.Success)
            {
                resp.Success = false;
                resp.ErrorMessage = "Please send a valid request.";
                return resp;
            }
            if (!string.IsNullOrEmpty(data.Email))
            {
                resp = _userManagementService.ValidateUserEmail(data.Email);
                if (resp.Success)
                {
                    string Email = resp.ID;
                    if (!string.IsNullOrWhiteSpace(Email))
                    {
                        //var ticks = DateTime.Now.Ticks.ToString();
                        obj.EncryptEmailWithTicks = Email + "^" + DateTime.Now.Ticks.ToString();
                        string[] splitArry = obj.EncryptEmailWithTicks.Split('^');
                        string ticks = string.Empty;
                        if (splitArry != null && splitArry.Length > 1)
                        {
                            ticks = splitArry.Length > 1 ? splitArry[1] : "";
                        }

                        FOX.BusinessOperations.CommonServices.EncryptionDecryption encrypt = new FOX.BusinessOperations.CommonServices.EncryptionDecryption();
                        obj.EncryptEmailWithTicks = encrypt.EncryptString(obj.EncryptEmailWithTicks.ToString());
                        //obj.EncryptEmailWithTicks = StringCipher.Encrypt(Email + "^" + DateTime.Now.Ticks.ToString());
                        _userManagementService.SavePasswordResetTicks(ticks, Email);
                        obj.IsValidate = true;
                    }
                    else
                    {
                        obj.IsValidate = false;
                    }
                }
                if (obj.IsValidate)
                {
                    var encodedstring = HttpUtility.UrlEncode(obj.EncryptEmailWithTicks);
                    _body = "<p><strong>*This is an automated email-Please do not reply</strong></p>" +
                          "<p> Hi,</p>" +
                          "<p> Please " + "<a href='" + link + encodedstring + @"' target='_blank'>click here</a>" + " to reset password of your FOX account.</p>" +
                          "<p><br/> If you believe that this request was erroneously sent, kindly ignore this email and promptly inform us at our tech support at (732) 873 - 5133 ext. 102 </p>";
                    Helper.SendEmail(sendTo, _subject, _body, null, null, null, _bccList);
                }
                //string EmailValidationResStr = Newtonsoft.Json.JsonConvert.SerializeObject(obj);
                //var response = Request.CreateResponse(HttpStatusCode.OK, EmailValidationResStr);
                resp.ID = string.Empty;
                return resp;
            }
            return null;
        }
        [AllowAnonymous]
        [Route("UpdatePassword")]
        public HttpResponseMessage UpdatePassword(ResetPasswordViewModel data)
        {
            UserProfile obj = GetProfile();
            var _user = _UserRepository.Get(t => t.EMAIL.Equals(data.Email));
            if (_user.USER_NAME != obj.UserName && obj.IsAdmin != true && obj.RoleId != 103)
            {
                var errorResponse = Request.CreateResponse(HttpStatusCode.BadRequest, "Error");
                return errorResponse;
            }
            this.ValidatePasswordResetKey(new ValidatePasswordResetKeyModel() { key = data.Key }, ref data);
            string _body = string.Empty;
            string _subject = "Reset Password Confirmation for your FOX Portal";
            string sendTo = string.Empty;
            List<string> _bccList = new List<string>();
            sendTo = _userManagementService.GetEmailByTick(data.Ticks);
            data.Email = sendTo;
            UpdatePasswordModel model = new UpdatePasswordModel();
            if (data.Email != null)
            {
                data.NewPassword = HttpUtility.UrlDecode(data.NewPassword);
                data.ConfirmPassword = HttpUtility.UrlDecode(data.ConfirmPassword);
                data.PasswordHash = UserManager.PasswordHasher.HashPassword(data.NewPassword);
                model.status = _userManagementService.UpdatePassword(data);
                if (model.status == 1)
                {
                    model.IsReset = true;
                    _body = "<p><strong>*This is an automated email-Please do not reply</strong></p>" +
                   "<p> Hi,</p>" +
                   "<p> Your password has been changed successfully.</p>" +
                   "<p><br/> If you believe that this request was erroneously sent, kindly ignore this email and promptly inform us at our tech support at (732) 873 - 5133 ext. 102 </p>";
                    Helper.SendEmail(sendTo, _subject, _body, null, null, null, _bccList);
                }
            }
            else
            {
                model.status = 0;
            }
            var response = Request.CreateResponse(HttpStatusCode.OK, model);
            return response;
        }
        public void ValidatePasswordResetKey(ValidatePasswordResetKeyModel key, ref ResetPasswordViewModel data)
        {
            //string decoded = HttpUtility.UrlDecode(key.key);
            FOX.BusinessOperations.CommonServices.EncryptionDecryption decrypt = new FOX.BusinessOperations.CommonServices.EncryptionDecryption();
            string decryptedVal = decrypt.DecryptString(key.key);
            string[] splitArry = decryptedVal.Split('^');
            string email = splitArry.Length > 0 ? splitArry[0] : "";
            string ticks = splitArry.Length > 1 ? splitArry[1] : "";
            data.Email = email;
            data.Ticks = ticks;
        }
        private const int Keysize = 256;

        // This constant determines the number of iterations for the password bytes generation function.
        private const int DerivationIterations = 1000;
        private static string passPhrase { get; set; } = "MTBCKey";
        public static string Decrypt(string cipherText)
        {
            // Get the complete stream of bytes that represent:
            // [32 bytes of Salt] + [32 bytes of IV] + [n bytes of CipherText]
            var cipherTextBytesWithSaltAndIv = Convert.FromBase64String(cipherText);
            // Get the saltbytes by extracting the first 32 bytes from the supplied cipherText bytes.
            var saltStringBytes = cipherTextBytesWithSaltAndIv.Take(Keysize / 8).ToArray();
            // Get the IV bytes by extracting the next 32 bytes from the supplied cipherText bytes.
            var ivStringBytes = cipherTextBytesWithSaltAndIv.Skip(Keysize / 8).Take(Keysize / 8).ToArray();
            // Get the actual cipher text bytes by removing the first 64 bytes from the cipherText string.
            var cipherTextBytes = cipherTextBytesWithSaltAndIv.Skip((Keysize / 8) * 2).Take(cipherTextBytesWithSaltAndIv.Length - ((Keysize / 8) * 2)).ToArray();

            using (var password = new Rfc2898DeriveBytes(passPhrase, saltStringBytes, DerivationIterations))
            {
                var keyBytes = password.GetBytes(Keysize / 8);
                using (var symmetricKey = new RijndaelManaged())
                {
                    symmetricKey.BlockSize = 256;
                    symmetricKey.Mode = CipherMode.CBC;
                    symmetricKey.Padding = PaddingMode.PKCS7;
                    using (var decryptor = symmetricKey.CreateDecryptor(keyBytes, ivStringBytes))
                    {
                        using (var memoryStream = new MemoryStream(cipherTextBytes))
                        {
                            using (var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                            {
                                var plainTextBytes = new byte[cipherTextBytes.Length];
                                var decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
                                memoryStream.Close();
                                cryptoStream.Close();
                                return Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount);
                            }
                        }
                    }
                }
            }
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing && UserManager != null)
            {
                UserManager.Dispose();
            }

            base.Dispose(disposing);
        }
        #region Helpers

        private IAuthenticationManager Authentication
        {
            get { return Request.GetOwinContext().Authentication; }
        }

        private IHttpActionResult GetErrorResult(IdentityResult result)
        {
            if (result == null)
            {
                return InternalServerError();
            }

            if (!result.Succeeded)
            {
                if (result.Errors != null)
                {
                    foreach (string error in result.Errors)
                    {
                        ModelState.AddModelError("", error);
                    }
                }

                if (ModelState.IsValid)
                {
                    // No ModelState errors are available to send, so just return an empty BadRequest.
                    return BadRequest();
                }

                return BadRequest(ModelState);
            }

            return null;
        }

        private class ExternalLoginData
        {
            public string LoginProvider { get; set; }
            public string ProviderKey { get; set; }
            public string UserName { get; set; }

            public IList<Claim> GetClaims()
            {
                IList<Claim> claims = new List<Claim>();
                claims.Add(new Claim(ClaimTypes.NameIdentifier, ProviderKey, null, LoginProvider));

                if (UserName != null)
                {
                    claims.Add(new Claim(ClaimTypes.Name, UserName, null, LoginProvider));
                }

                return claims;
            }

            public static ExternalLoginData FromIdentity(ClaimsIdentity identity)
            {
                if (identity == null)
                {
                    return null;
                }

                Claim providerKeyClaim = identity.FindFirst(ClaimTypes.NameIdentifier);

                if (providerKeyClaim == null || String.IsNullOrEmpty(providerKeyClaim.Issuer)
                    || String.IsNullOrEmpty(providerKeyClaim.Value))
                {
                    return null;
                }

                if (providerKeyClaim.Issuer == ClaimsIdentity.DefaultIssuer)
                {
                    return null;
                }

                return new ExternalLoginData
                {
                    LoginProvider = providerKeyClaim.Issuer,
                    ProviderKey = providerKeyClaim.Value,
                    UserName = identity.FindFirstValue(ClaimTypes.Name)
                };
            }
        }

        private static class RandomOAuthStateGenerator
        {
            private static RandomNumberGenerator _random = new RNGCryptoServiceProvider();

            public static string Generate(int strengthInBits)
            {
                const int bitsPerByte = 8;

                if (strengthInBits % bitsPerByte != 0)
                {
                    throw new ArgumentException("strengthInBits must be evenly divisible by 8.", "strengthInBits");
                }

                int strengthInBytes = strengthInBits / bitsPerByte;

                byte[] data = new byte[strengthInBytes];
                _random.GetBytes(data);
                return HttpServerUtility.UrlTokenEncode(data);
            }
        }

        #endregion

        #region copied from old
        //Generate RandomNo
        public int GenerateRandomNo()
        {
            int _min = 1000;
            int _max = 9999;
            Random _rdm = new Random();
            return _rdm.Next(_min, _max);
        }


        //[POST("/Account/VerifyWorkOrderByRecipient")]
        [HttpPost]
        //[AllowAnonymous]
        public HttpResponseMessage VerifyWorkOrderByRecipient(ReqVerifyWorkOrder model)
        {
            var responseVerifyWorkOrderByRecipient = _requestForOrderService.VerifyWorkOrderByRecipient(model.Value);
            var response = Request.CreateResponse(HttpStatusCode.OK, responseVerifyWorkOrderByRecipient);
            return response;
        }

        [AllowAnonymous]
        [HttpGet]
        public HttpResponseMessage FetchSenderTypes()
        {
            return Request.CreateResponse(HttpStatusCode.OK, _accountServices.getSenderTypes());
        }

        [HttpPost]
        [AllowAnonymous]
        public HttpResponseMessage FetchUserDetailsByNPI(UserDetailsByNPIRequestModel model)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _accountServices.getUserDetailByNPI(model));
        }
        [HttpPost]
        [AllowAnonymous]
        public HttpResponseMessage FetchCityDetailByZipCode(CityDetailByZipCodeRequestModel model)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _accountServices.getCityDetailByZipCode(model));
        }
        [HttpPost]
        [AllowAnonymous]
        public HttpResponseMessage CheckEmailAlreadyExist(EmailExist model)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _accountServices.CheckIfEmailAlreadyInUse(model));
        }
        [HttpPost]
        [AllowAnonymous]
        public HttpResponseMessage UploadSignature()
        {
            var file = HttpContext.Current.Request.Files.Get("Signatures");
            var fileName = _accountServices.UploadSignature(file);
            return Request.CreateResponse(HttpStatusCode.OK, fileName);
        }
        [HttpPost]
        [AllowAnonymous]
        public HttpResponseMessage FetchPractices(SmartSearchRequest model)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _accountServices.getPractices(model));
        }
        [HttpPost]
        [AllowAnonymous]
        public HttpResponseMessage FetchSpecialities(SmartSearchRequest model)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _accountServices.getSmartSpecialities(model));
        }
        [HttpPost]
        [AllowAnonymous]
        public HttpResponseMessage GetSmartIdentifier(SmartSearchRequest model)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _accountServices.getSmartIdentifier(model));
        }
        #endregion
        [HttpGet]
        [AllowAnonymous]
        public HttpResponseMessage ClearOpenedByinPatientforUser(string UserName)
        {
            _accountServices.ClearOpenedByinPatientforUser(UserName);
            return Request.CreateResponse(HttpStatusCode.OK, "Cleared");
        }

        /// <summary>Match current user machine IP with USA IP </summary>
        /// <author>Muhammad Arqam </author> 
        /// <param>userName </param>
        [HttpPost]
        [AllowAnonymous]
        public HttpResponseMessage IpConfig(GetUserIP data)
        {
            var result = _accountServices.IpConfig(data);
            var EncrptedResult = Encrypt.EncryptionForClient(result.ToString());//vulnerability fixation by irfan ullah
            var response = Request.CreateResponse(HttpStatusCode.OK, EncrptedResult);
            return response;
        }

        private async Task<GoogleRecaptchaResponse> ValidateCaptcha(string encodedCode)
        {

            string url = $"https://www.google.com/recaptcha/api/siteverify?secret={AppConfiguration.SecretKey}&response={encodedCode}";
            using (var client = new HttpClient())
            {
                try
                {
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                    return JsonConvert.DeserializeObject<GoogleRecaptchaResponse>(await client.GetStringAsync(url));
                }
                catch (Exception ex)
                {
                    //throw ex;
                    return new GoogleRecaptchaResponse()
                    {
                        Success = false,
                        ErrorCodes = new string[] { ex.ToString() },
                    };
                }
            }

        }


        //--method used to get otp code for MFA  
        private IRestResponse GetOtpCode(string email)
        {

            var client = new RestClient(WebConfigurationManager.AppSettings["GetOtpURL"]);
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            request.AddHeader("Content-Type", "application/json");
            var body = @"{" + "\n" +
                        @"  ""applicationName"": ""Fox""," + "\n" +
                        @"  ""userName"": """ + email + "\",\n" +
                        @"  ""deviceInfo"": ""12345""," + "\n" +
                        @"  ""appDisplayName"": ""FOX Portal""" + "\n" +
                        @"}";
            request.AddParameter("application/json", body, ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            return response;
        }
        //--method used to verify otp code for MFA  
        private IRestResponse VerifyOtpCode(string otp, string otpIdentifier)
        {
            var client = new RestClient(WebConfigurationManager.AppSettings["VerifyOtpURL"]);
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("Content-Type", "application/json");
            var body = @"{" + "\n" +
                        @"  ""deviceInfo"": ""12345""," + "\n" +
                        @"  ""notifier"": """ + otpIdentifier + "\",\n" +
                       @"  ""otp"": """ + otp + "\"\n" +
                        @"}";
            request.AddParameter("application/json", body, ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            return response;
        }
    }
}
