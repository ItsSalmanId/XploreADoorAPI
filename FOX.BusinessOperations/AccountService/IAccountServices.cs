using FOX.DataModels.Models.CasesModel;
using FOX.DataModels.Models.CommonModel;
using FOX.DataModels.Models.ExternalUserModel;
using FOX.DataModels.Models.Security;
using FOX.DataModels.Models.Settings.RoleAndRights;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace FOX.BusinessOperations.AccountService
{
    public interface IAccountServices
    {
        ResponseGetSenderTypesModel getSenderTypes();
        UserDetailsByNPIResponseModel getUserDetailByNPI(UserDetailsByNPIRequestModel model);
        CityDetailByZipCodeResponseModel getCityDetailByZipCode(CityDetailByZipCodeRequestModel model);
        bool CheckIfEmailAlreadyInUse(EmailExist model);
        string UploadSignature(HttpPostedFile file);
        FoxTBLPracticeOrganizationResponseModel getPractices(SmartSearchRequest model);
        SmartSpecialitySearchResponseModel getSmartSpecialities(SmartSearchRequest model);
        List<SmartIdentifierRes> getSmartIdentifier(SmartSearchRequest obj);
        ExternalUserSignupResponseModel CreateExternalUser(DataModels.Models.Security.User user);
        bool CheckForDublicateNPI(string npi);

        void SavePasswordHistory(dynamic user);
        void EmailToAdmin(dynamic user);
        void ClearOpenedByinPatientforUser(string UserName);
        ResponseModel SignOut(LogoutModal logout, UserProfile profile);
        void InsertLogs(dynamic user, string encryptedPassword, string detectedBrowser, string  requestType);
        bool IpConfig(GetUserIP data);
    }
}
