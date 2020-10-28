using FOX.DataModels.Models.CommonModel;
using FOX.DataModels.Models.PatientSurvey;
using FOX.DataModels.Models.Security;
using System.Collections.Generic;

namespace FOX.BusinessOperations.PatientSurveyService.UploadDataService
{
    public interface IUploadDataService
    {
        List<PatientSurvey> GetLastUpload(long practiceCode);
        ResponseModel ReadExcel(string filePath, long practiceCode, string userName);
    }
}
