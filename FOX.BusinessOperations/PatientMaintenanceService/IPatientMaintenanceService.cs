using FOX.DataModels.Models.IndexInfo;
using FOX.DataModels.Models.Patient;
using FOX.DataModels.Models.Security;
using System.Collections.Generic;

namespace FOX.BusinessOperations.PatientMaintenanceService
{
    public interface IPatientMaintenanceService
    {
        Patient GetPatientByAccountNo(long patientAccount);
        List<IndexPatRes> SearchPatients(getPatientReq patientReq, UserProfile Profile);
    }
}
