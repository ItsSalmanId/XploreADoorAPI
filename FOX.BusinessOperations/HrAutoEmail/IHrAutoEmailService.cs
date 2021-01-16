using FOX.DataModels.Models.CommonModel;
using FOX.DataModels.Models.HrAutoEmail;
using FOX.DataModels.Models.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FOX.BusinessOperations.HrAutoEmail
{
    public interface IHrAutoEmailService
    {
        ResponseModel AddHrAutoEmailConfigure(HrAutoEmailConfigure hrAutoEmail, UserProfile userProfile);
        IEnumerable<HrAutoEmailConfigure> GetHrAutoEmailConfigureRecords(UserProfile userProfile);
        IEnumerable<HrEmailDocumentFileAll> GetHrAutoEmalById(int Id, UserProfile userProfile);
        ResponseModel SaveHrMTBCEMailDocumentFiles(List<HrEmailDocumentFileAll> hrEmailDocumentFileAll, UserProfile Profile);
        ResponseModel UpdateHrAutoEmailConfigure(HrAutoEmailConfigure hrAutoEmail, UserProfile userProfile);
        ResponseModel DeleteHrAutoEmailConfigure(HrAutoEmailConfigure hrAutoEmail, UserProfile userProfile);
        IEnumerable<GetDocumentFileDetails> GetMTBCDocumentFileDetails(UserProfile userProfile);
        ResponseModel EnableHrAutoEmailCertificate(HrAutoEmailConfigure hrAutoEmailConfigure, UserProfile userProfile);
        List<string> GetMTBCUnMappedCategory(UserProfile userProfile);
    }
}
