using FOX.BusinessOperations.CommonService;
using FOX.BusinessOperations.Security;
using FOX.BusinessOperations.SettingsService.FacilityLocationService;
using FOX.BusinessOperations.SettingsService.UserMangementService;
using FOX.BusinessOperations.StoreProcedure.SettingsService.ReferralSourceService;
using FOX.DataModels.Models.ExternalUserModel;
using FOX.DataModels.Models.Security;
using FOX.DataModels.Models.Settings.FacilityLocation;
using FOX.DataModels.Models.Settings.ReferralSource;
using FOX.DataModels.Models.Settings.RoleAndRights;
using FOX.DataModels.Models.Settings.User;
using FoxRehabilitationAPI.Filters;
using FoxRehabilitationAPI.Models;
using Microsoft.AspNet.Identity;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using FOX.BusinessOperations.AccountService;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace FoxRehabilitationAPI.Controllers
{
    [Authorize]
    [ExceptionHandlingFilter]
    public class SettingsController : BaseApiController
    {
        private readonly IUserManagementService _userServices;
        private readonly UserManagementService _userServicess;
        private readonly IReferralSourceService _referralSourceService;
        private readonly IFacilityLocationService _facilityLocationService;
        private readonly IPasswordHistoryService _passwordHistoryService;
        private readonly IAccountServices _accountServices = new AccountServices();

        public SettingsController(IUserManagementService userServices, UserManagementService userServicess, IReferralSourceService referralSourceService, IFacilityLocationService facilityLocationService,
            IPasswordHistoryService passwordHistoryService)
        {
            _userServices = userServices;
            _userServicess = userServicess;
            _referralSourceService = referralSourceService;
            _facilityLocationService = facilityLocationService;
            _passwordHistoryService = passwordHistoryService;
        }

        [HttpPost]
        public async Task<HttpResponseMessage> CreateUser([FromBody] ApplicationUser user)
        {
            var profile = GetProfile();
            var userToUpdate = _userServices.IsUserAlreadyExist(user.USER_ID);
            if (userToUpdate != null)
            {
                userToUpdate.Is_Blocked = user.Is_Blocked.HasValue ? user.Is_Blocked.Value : false;
            }
            bool canUpdateUser = _userServices.CanUserUpdateUser(profile);
            if (userToUpdate == null && !_accountServices.CheckIfEmailAlreadyInUse(new EmailExist() { EMAIL = user.EMAIL }))
            {
                string password = HttpUtility.UrlDecode(user.PASSWORD);

                user.USER_ID = Helper.getMaximumId("USER_ID");
                user.USER_NAME = $"{user.LAST_NAME.Trim()}_{user.USER_ID}";
                user.CREATED_BY = profile.UserName;
                user.CREATED_DATE = Helper.GetCurrentDate();
                user.MODIFIED_BY = profile.UserName;
                user.MODIFIED_DATE = Helper.GetCurrentDate();
                user.PRACTICE_CODE = profile.PracticeCode;
                user.IS_APPROVED = true;
                user.PASSWORD = Encrypt.getEncryptedCode(password);
                user.IS_AD_USER = false;
                IdentityResult result = await UserManager.CreateAsync(user, password);
                if (result.Succeeded)
                {
                    _userServices.SavePasswordHisotry(user, profile);
                    _userServices.SaveSenderName(user, profile);
                }

                //if (user.USER_TYPE.ToLower().Contains("external") && (user.IS_APPROVED ?? false) && (user.ROLE_ID.HasValue && user.ROLE_ID == 108 || user.ROLE_ID.HasValue && user.ROLE_ID == 109))
                //{
                //    _userServicess.AddUpdateReferralSourceInfo(user.USER_NAME, profile);

                //}
                return Request.CreateResponse(HttpStatusCode.OK, user);
            }
            else
            {
                if (_userServices.UpdateUser(userToUpdate, user, profile, canUpdateUser))
                {

                    //if (!string.IsNullOrWhiteSpace(user.USER_TYPE) && user.USER_TYPE.ToLower().Contains("external") && (user.IS_APPROVED ?? false) && (user.ROLE_ID.HasValue && user.ROLE_ID == 108 || user.ROLE_ID.HasValue && user.ROLE_ID == 109))
                    //{
                    //    _userServicess.AddUpdateReferralSourceInfo(userToUpdate.USER_NAME, profile);
                    //}

                    return Request.CreateResponse(HttpStatusCode.OK, profile);
                }
                return Request.CreateResponse(HttpStatusCode.BadRequest, new UserProfile());
            }
        }
        [HttpPost]
        public HttpResponseMessage DiscardAndDeleteExternalUserRequest([FromBody] User user)
        {
            return Request.CreateErrorResponse(HttpStatusCode.OK, _userServices.DiscardAndDeleteExternalUser(user, GetProfile()).ToString());

        }
        [HttpPost]
        public HttpResponseMessage GetUsers(UserRequest req)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _userServices.GetUsers(req, GetProfile()));
        }
        //export to excel 08-07-2021
        [HttpPost]
        public HttpResponseMessage ExportToExcelUsersReport(UserRequest req)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _userServices.ExportToExcelUsersReport(req, GetProfile()));
        }

        //close
        [HttpGet]
        public HttpResponseMessage GetSingleUser(string username)
        {
            var user = _userServices.GetSingleUser(username, GetProfile());
            var response = Request.CreateResponse(HttpStatusCode.OK, user);
            return response;
        }
        [HttpGet]
        public HttpResponseMessage GetDataForRefferalReagions(string username)
        {
            var user = _userServices.GetReferralReagions(username, GetProfile());
            var response = Request.CreateResponse(HttpStatusCode.OK, user);
            return response;
        }

        public HttpResponseMessage GetUsersForSupervisor()
        {
            var profile = GetProfile();
            return Request.CreateResponse(HttpStatusCode.OK, _userServices.GetUsersForSupervisorDD(profile.PracticeCode));
        }

        [HttpPost]
        public HttpResponseMessage UploadSignature()
        {
            var fileAttachments = HttpContext.Current.Request.Files;
            string username = HttpContext.Current.Request.Params["username"];
            var result = _userServices.UploadSignatures(fileAttachments, username, GetProfile());
            var response = Request.CreateResponse(HttpStatusCode.OK, result);
            return response;
        }

        //Role And Rights Section

        public HttpResponseMessage GetRoles()
        {
            var profile = GetProfile();
            return Request.CreateResponse(HttpStatusCode.OK, _userServices.GetRoles(profile.PracticeCode));
        }

        [HttpGet]
        public HttpResponseMessage CheckForAtleastOneRight(long roleId)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _userServices.CheckForAtleastOneRight(roleId, GetProfile().PracticeCode));
        }

        public HttpResponseMessage SetRole(Role role)
        {
            _userServices.SetRole(role, GetProfile());
            var response = Request.CreateResponse(HttpStatusCode.OK, _userServices.GetRoles(GetProfile().PracticeCode));
            return response;
        }

        public HttpResponseMessage GetPracticeRole()
        {
            var profile = GetProfile();
            return Request.CreateResponse(HttpStatusCode.OK, _userServices.GetPracticeRole(profile.PracticeCode));
        }
        [HttpPost]
        public HttpResponseMessage UpdatePassword(PasswordChangeRequest request)
        {
            request.PasswordHash = UserManager.PasswordHasher.HashPassword(HttpUtility.UrlDecode(request.Password));
            _userServices.UpdatePassword(request, GetProfile());
            var response = Request.CreateResponse(HttpStatusCode.OK, "Updated");
            return response;

        }

        [HttpPost]
        public HttpResponseMessage AddRole(RoleToAdd request)
        {
            _userServices.AddRole(request, GetProfile());
            var response = Request.CreateResponse(HttpStatusCode.OK, "Updated");
            return response;

        }

        [HttpPost]
        public HttpResponseMessage DeleteRole(Role request)
        {
            _userServices.DeleteRole(request, GetProfile());
            var response = Request.CreateResponse(HttpStatusCode.OK, "Deleted");
            return response;
        }

        [HttpGet]
        public HttpResponseMessage GetCurrentUserRigts()
        {
            var profile = GetProfile();
            var data = _userServices.GetCurrentUserRights(profile.RoleId, profile.PracticeCode);
            var response = Request.CreateResponse(HttpStatusCode.OK, data);
            return response;

        }
        [HttpPost]
        public HttpResponseMessage CheckPreviousUsedPassword(PreviousPasswordCheck password)
        {
            password.PasswordHash = UserManager.PasswordHasher.HashPassword(password.password);
            var res = _passwordHistoryService.CheckIfPasswordIsPreviouslyUser(password, GetProfile());
            return Request.CreateResponse(HttpStatusCode.OK, res);

        }
        [HttpPost]
        public HttpResponseMessage EmailExists(FOX.DataModels.Models.Settings.RoleAndRights.EmailExist request)
        {
            var res = _userServices.EmailExists(request);
            var response = Request.CreateResponse(HttpStatusCode.OK, res);
            return response;
        }

        [HttpPost]
        public HttpResponseMessage AddUpdateReferralRegion([FromBody] ReferralRegion referralRegion)
        {
            _userServices.AddUpdateReferralRegion(referralRegion, GetProfile());
            var response = Request.CreateResponse(HttpStatusCode.OK, "Created/Updated");
            return response;
        }

        [HttpPost]
        public HttpResponseMessage GetReferralRegions(ReferralRegionSearch referralRegionSearch)
        {
            var referralRegions = _userServices.GetReferralRegionList(referralRegionSearch, GetProfile());
            var response = Request.CreateResponse(HttpStatusCode.OK, referralRegions);
            return response;
        }
        [HttpPost]
        public HttpResponseMessage ExportToExcelReferralRegion(ReferralRegionSearch referralRegionSearch)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _userServices.ExportToExcelReferralRegion(referralRegionSearch, GetProfile()));
        }
        [HttpPost]
        public HttpResponseMessage GetReferralRegion(ReferralRegionSearch referralRegionSearch)
        {
            var referralRegions = _userServices.GetReferralRegion(referralRegionSearch, GetProfile());
            var response = Request.CreateResponse(HttpStatusCode.OK, referralRegions);
            return response;
        }

        [HttpPost]
        public HttpResponseMessage AddUpdateReferralSource([FromBody] ReferralSource referralSource)
        {
            var result = _referralSourceService.AddUpdateReferralSource(referralSource, GetProfile());
            var response = Request.CreateResponse(HttpStatusCode.OK, result);
            return response;
        }

        [HttpGet]
        public HttpResponseMessage GetReferralSourceBySourceID(long sourceId)
        {
            var referralRegion = _referralSourceService.GetReferralSourceBySourceID(sourceId);
            var response = Request.CreateResponse(HttpStatusCode.OK, referralRegion);
            return response;
        }

        [HttpPost]
        public HttpResponseMessage GetReferralSourceList(ReferralSourceSearch referralSourceSearch)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _referralSourceService.GetReferralSourceList(referralSourceSearch, GetProfile()));
        }
        [HttpPost]
        public HttpResponseMessage ExportToExcelReferralSource(ReferralSourceSearch referralRegionSearch)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _referralSourceService.ExportToExcelReferralSource(referralRegionSearch, GetProfile()));
        }
        //AUTOMATION
        public HttpResponseMessage GetRolesCheckBit()
        {
            var profile = GetProfile();
            var users = _userServices.GetRolesCheckBit(profile.PracticeCode, profile.UserName);
            var response = Request.CreateResponse(HttpStatusCode.OK, users);
            return response;
        }

        [HttpPost]
        public HttpResponseMessage AddUpdateFacilityLocation([FromBody] FacilityLocation facilityLocation)
        {
            var _facilityLocation = _facilityLocationService.AddUpdateFacilityLocation(facilityLocation, GetProfile());
            var response = Request.CreateResponse(HttpStatusCode.OK, _facilityLocation);
            return response;
        }

        [HttpPost]
        public HttpResponseMessage GetFacilityLocationList(FacilityLocationSearch facilityLocationSearch)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _facilityLocationService.GetFacilityLocationList(facilityLocationSearch, GetProfile()));
        }
        [HttpPost]
        public HttpResponseMessage ExportToExcelFacilityCreation(FacilityLocationSearch facilityLocationSearch)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _facilityLocationService.ExportToExcelFacilityCreation(facilityLocationSearch, GetProfile()));
        }
        [HttpPost]
        public HttpResponseMessage ExportToExcel(FacilityLocationSearch facilityLocationSearch)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _facilityLocationService.Export(facilityLocationSearch, GetProfile()));
        }

        [HttpPost]
        public HttpResponseMessage GetFacilityLocationByLocationId(LocationPatientAccount location)
        {
            var profile = GetProfile();
            var facilityLocation = _facilityLocationService.GetFacilityLocationById(location, profile.PracticeCode.ToString());
            var response = Request.CreateResponse(HttpStatusCode.OK, facilityLocation);
            return response;
        }

        [HttpGet]
        public HttpResponseMessage GetProviderNamesList(string searchText)
        {
            var profile = GetProfile();
            return Request.CreateResponse(HttpStatusCode.OK, _facilityLocationService.GetProviderNamesList(searchText, profile.PracticeCode));
        }

        [HttpGet]
        public HttpResponseMessage GetProviderCode(string state)
        {
            var profile = GetProfile();
            var providercode = _facilityLocationService.GetProviderCode(state, profile.PracticeCode);
            var response = Request.CreateResponse(HttpStatusCode.OK, providercode);
            return response;
        }

        [HttpPost]
        [ActionName("FetchPractices")]
        public HttpResponseMessage FetchPractices(SmartSearchRequest model)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _userServices.getPractices(model, GetProfile()));
        }

        [HttpPost]
        [ActionName("GetSmartIdentifier")]
        public HttpResponseMessage GetSmartIdentifier(SmartSearchRequest model)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _userServices.GetSmartIdentifier(model, GetProfile()));
        }

        [HttpPost]
        [ActionName("FetchSpecialities")]
        public HttpResponseMessage FetchSpecialities(SmartSearchRequest model)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _userServices.getSmartSpecialities(model, GetProfile()));
        }

        [HttpGet]
        public HttpResponseMessage AddUpdateUserExtension(string extension, bool? isActive)
        {
            var profile = GetProfile();
            _userServices.AddUpdateUserExtension(profile.userID, extension, isActive);
            var response = Request.CreateResponse(HttpStatusCode.OK, "Record Updated Successfully.");
            return response;
        }
        [HttpGet]
        public HttpResponseMessage GetSmartUsersOfSpecificRoleName(string searchText, string roleName)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _userServices.GetSmartUsersOfSpecificRoleName(searchText: searchText, roleName: roleName, userProfile: GetProfile()));
        }
        [HttpGet]
        public HttpResponseMessage GetAlternateAccountManger(string roleName)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _userServices.GetAlternateAccountManger(roleName: roleName, userProfile: GetProfile()));
        }
        [HttpGet]
        public HttpResponseMessage GetSmartStates(string searchText)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _userServices.GetSmartStates(searchText));
        }
        [HttpGet]
        public HttpResponseMessage GetFacilityTypeById(long facilityTypeId)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _facilityLocationService.GetFacilityTypeById(facilityTypeId, GetProfile().PracticeCode));
        }
        [HttpGet]
        public HttpResponseMessage GetSmartReferralRegions(string searchText)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _facilityLocationService.GetSmartReferralRegions(searchText, GetProfile().PracticeCode));
        }
        [HttpPost]
        public HttpResponseMessage GetGroupIdentifierList(GroupIdentifierSearch groupIdentifierSearch)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _facilityLocationService.GetGroupIdentifierList(groupIdentifierSearch, GetProfile().PracticeCode));
        }
        [HttpPost]
        public HttpResponseMessage ExportToExcelGetGroupIdentifier(GroupIdentifierSearch groupIdentifierSearch)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _facilityLocationService.ExportToExcelGetGroupIdentifier(groupIdentifierSearch, GetProfile()));
        }
        [HttpPost]
        public HttpResponseMessage LocationCorporationList(LocationCorporationSearch locationCorporationSearch)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _facilityLocationService.LocationCorporationList(locationCorporationSearch, GetProfile().PracticeCode));
        }
        [HttpPost]
        public HttpResponseMessage ExportToExcelLocationCorporation(LocationCorporationSearch locationCorporationSearch)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _facilityLocationService.ExportToExcelLocationCorporation(locationCorporationSearch, GetProfile()));
        }
        [HttpPost]
        public HttpResponseMessage GetIdentifierList(IdentifierSearch identifierSearch)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _facilityLocationService.GetIdentifierList(identifierSearch, GetProfile().PracticeCode));
        }
        [HttpPost]
        public HttpResponseMessage ExportToExcelIdentifier(IdentifierSearch identifierSearch)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _facilityLocationService.ExportToExcelIdentifier(identifierSearch, GetProfile()));
        }
        [HttpPost]
        public HttpResponseMessage GetAuthStatusList(AuthStatusSearch authStatusSearch)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _facilityLocationService.GetAuthStatusList(authStatusSearch, GetProfile().PracticeCode));
        }
        [HttpPost]
        public HttpResponseMessage ExportToExcelAuthStatus(AuthStatusSearch authStatusSearch)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _facilityLocationService.ExportToExcelAuthStatus(authStatusSearch, GetProfile()));
        }
        [HttpPost]
        public HttpResponseMessage GetTaskTypeList(TaskTpyeSearch taskTpyeSearch)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _facilityLocationService.GetTaskTypeList(taskTpyeSearch, GetProfile().PracticeCode));
        }
        [HttpPost]
        public HttpResponseMessage ExportToExcelTaskType(TaskTpyeSearch taskTpyeSearch)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _facilityLocationService.ExportToExcelTaskType(taskTpyeSearch, GetProfile()));
        }
        [HttpPost]
        public HttpResponseMessage GetOrderStatusList(OrderStatusSearch orderStatusSearch)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _facilityLocationService.GetOrderStatusList(orderStatusSearch, GetProfile().PracticeCode));
        }
        [HttpPost]
        public HttpResponseMessage ExportToExcelOrderStatus(OrderStatusSearch orderStatusSearch)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _facilityLocationService.ExportToExcelOrderStatus(orderStatusSearch, GetProfile()));
        }
        [HttpPost]
        public HttpResponseMessage GetSourceofReferralList(SourceOfreferralSearch sourceOfreferralSearch)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _facilityLocationService.GetSourceofReferralList(sourceOfreferralSearch, GetProfile().PracticeCode));
        }
        [HttpPost]
        public HttpResponseMessage ExportToExcelSourceOfReferral(SourceOfreferralSearch sourceOfreferralSearch)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _facilityLocationService.ExportToExcelSourceOfReferral(sourceOfreferralSearch, GetProfile()));
        }
        [HttpPost]
        public HttpResponseMessage GetAlertTypeList(AlertTypeSearch alertTypeSearch)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _facilityLocationService.GetAlertTypeList(alertTypeSearch, GetProfile().PracticeCode));
        }
        [HttpPost]
        public HttpResponseMessage ExportToExcelAlertType(AlertTypeSearch alertTypeSearch)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _facilityLocationService.ExportToExcelAlertType(alertTypeSearch, GetProfile()));
        }
        [HttpPost]
        public HttpResponseMessage GetDocumentTypeList(DocumentTypeSearch documentTypeSearch)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _facilityLocationService.GetDocumentTypeList(documentTypeSearch));
        }
        [HttpPost]
        public HttpResponseMessage ExportToExcelDocumentType(DocumentTypeSearch documentTypeSearch)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _facilityLocationService.ExportToExcelDocumentType(documentTypeSearch, GetProfile()));
        }
        [HttpPost]
        public HttpResponseMessage GetPatientContactTypeList(PatientContactTypeSearch patientContactTypeSearch)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _facilityLocationService.GetPatientContactTypeList(patientContactTypeSearch, GetProfile().PracticeCode));
        }
        [HttpPost]
        public HttpResponseMessage ExportToExcelContactType(PatientContactTypeSearch patientContactTypeSearch)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _facilityLocationService.ExportToExcelContactType(patientContactTypeSearch, GetProfile()));
        }
        [HttpGet]
        public HttpResponseMessage GetIdentifierTypes()
        {
            var profile = GetProfile();
            return Request.CreateResponse(HttpStatusCode.OK, _facilityLocationService.GetIdentifierTypes(profile));
        }
        [HttpGet]
        public HttpResponseMessage RedirecToTalkEhr()
        {
            return Request.CreateResponse(HttpStatusCode.OK, _userServices.RedirecToTalkEhr(GetProfile()));
        }
        [HttpGet]
        [AllowAnonymous]
        public HttpResponseMessage CheckisTalkrehab(string practiceCode)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _userServices.CheckisTalkrehab(practiceCode));
        }
        [HttpGet]
        [AllowAnonymous]
        public HttpResponseMessage GetTalkrehabDisabedModules()
        {
            return Request.CreateResponse(HttpStatusCode.OK, _userServices.GetTalkrehabDisabedModules());
        }
        [HttpGet]
        public HttpResponseMessage SetAutoLockTime(int AutoLockTime)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _userServices.SetAutoLockTimeSetup(AutoLockTime, GetProfile()));
        }
        [HttpPost]
        public HttpResponseMessage DeleteUser(DeleteUserModel res)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _userServices.DeleteUser(res, GetProfile()));
        }
        [HttpPost]
        public HttpResponseMessage GetActiveIndexers(ActiveIndexer req)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _userServices.GetActiveIndexers(req, GetProfile()));
        }
        [HttpPost]
        public HttpResponseMessage UpdateActiveIndexers(List<ActiveIndexer> res)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _userServices.UpdateActiveIndexers(res, GetProfile()));
        }
        [HttpPost]
        public HttpResponseMessage GetActiveIndexersLogs(ActiveIndexerLogs res)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _userServices.GetActiveIndexersLogs(res, GetProfile()));
        }
        [HttpPost]
        public HttpResponseMessage GetActiveIndexersHistory(ActiveIndexerHistory req)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _userServices.GetActiveIndexersHistory(req, GetProfile()));
        }
        [HttpPost]
        public HttpResponseMessage ExportToExcelHistory(ActiveIndexerHistory req)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _userServices.ExportToExcelHistory(req, GetProfile()));
        }
        [HttpGet]
        public HttpResponseMessage CheckActiveStatus()
        {
            return Request.CreateResponse(HttpStatusCode.OK, _userServices.CheckActiveStatus(GetProfile()));
        }
        [HttpGet]
        public HttpResponseMessage AddUserTeam(string userTeamModel)
        {
            var userTeamList = JsonConvert.DeserializeObject<List<UserTeamModel>>(userTeamModel);
            if (userTeamList.Count > 0)
            {
                return Request.CreateResponse(HttpStatusCode.OK, _userServices.AddUserTeam(userTeamList, GetProfile()));
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "User List is Empty");
            }
        }
    }
}


