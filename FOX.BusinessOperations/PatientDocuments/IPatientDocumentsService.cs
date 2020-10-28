using FOX.DataModels.Models.CommonModel;
using FOX.DataModels.Models.IndexInfo;
using FOX.DataModels.Models.PatientDocuments;
using FOX.DataModels.Models.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FOX.BusinessOperations.PatientDocumentsService
{
    public interface IPatientDocumentsService
    {
        DocumenttypeAndpatientcases getDocumentTypes(string patientAccount, UserProfile userProfile);
        List<PatientDocument> getDocumentsDataInformation(UserProfile userProfile, PatientDocumentRequest ObjPatientDocumentRequest);
        ResponseModel AddUpdateNewDocumentInformation(PatientPATDocument ObjPatientPATDocument, UserProfile profile);
        string ExportToExcelDocumentInformation(UserProfile userProfile, PatientDocumentRequest ObjPatientDocumentRequest);
        List<PatientDocument> getDocumentImagesInformation(UserProfile userProfile, PatientDocumentRequest ObjPatientDocumentRequest);
        ResponseModel DeleteDocumentFilesInformation(UserProfile userProfile, PatientDocument objPatientDocument);
        List<FoxDocumentType> GetAllDocumentTypes(UserProfile userProfile);
        List<FoxDocumentType> GetAllDocumentTypeswithInactive(UserProfile userProfile);
        List<FoxSpecialityProgram> GetAllSpecialityProgram(UserProfile userProfile);
    }
}
