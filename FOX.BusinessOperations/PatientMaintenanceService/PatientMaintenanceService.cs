//System Namespaces
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

//App Namespaces
using FOX.BusinessOperations.CommonService;
using FOX.DataModels.Context;
using FOX.DataModels.GenericRepository;
using FOX.DataModels.Models.IndexInfo;
using FOX.DataModels.Models.Patient;
using FOX.DataModels.Models.Security;

namespace FOX.BusinessOperations.PatientMaintenanceService
{
    public class PatientMaintenanceService : IPatientMaintenanceService
    {
        private readonly DbContextPatient _patientContext = new DbContextPatient();
        private readonly GenericRepository<Patient> _patientRepository;

        public PatientMaintenanceService()
        {
            _patientRepository = new GenericRepository<Patient>(_patientContext);
        }

        public Patient GetPatientByAccountNo(long patientAccount)
        {
            return _patientRepository.GetByID(patientAccount);
        }

        public List<IndexPatRes> SearchPatients(getPatientReq patientReq, UserProfile Profile)
        {
            if (patientReq.Last_Name == null)
                patientReq.Last_Name = "";
            if (patientReq.First_Name == null)
                patientReq.First_Name = "";
            if (patientReq.Middle_Name == null)
                patientReq.Middle_Name = "";
            if (patientReq.SSN == null)
                patientReq.SSN = "";
            if (patientReq.Gender == null)
                patientReq.Gender = "";
            if (patientReq.Chart_Id == null)
                patientReq.Chart_Id = "";
            patientReq.Practice_Code = Profile.PracticeCode;
            if (string.IsNullOrEmpty(patientReq.Date_Of_Birth_In_String))
            {
                patientReq.Date_Of_Birth_In_String = null;
            }

            var _lastName = new SqlParameter("Last_Name", SqlDbType.VarChar) { Value = !string.IsNullOrWhiteSpace(patientReq.Last_Name) ? patientReq.Last_Name.Trim() : patientReq.Last_Name };
            var _firstName = new SqlParameter("First_Name", SqlDbType.VarChar) { Value = !string.IsNullOrWhiteSpace(patientReq.First_Name) ? patientReq.First_Name.Trim() : patientReq.First_Name };
            var _middleName = new SqlParameter("Middle_Name", SqlDbType.VarChar) { Value = patientReq.Middle_Name };
            var _ssn = new SqlParameter("SSN", SqlDbType.VarChar) { Value = patientReq.SSN };
            var _dob = Helper.getDBNullOrValue("Date_Of_Birth", patientReq.Date_Of_Birth_In_String == null ? null : Convert.ToDateTime(patientReq.Date_Of_Birth_In_String).ToShortDateString());
            var _gender = new SqlParameter("Gender", SqlDbType.VarChar) { Value = patientReq.Gender };
            var _chartId = new SqlParameter("@Chart_Id", SqlDbType.VarChar) { Value = patientReq.Chart_Id };
            var _practiceCode = new SqlParameter("PRACTICE_CODE", SqlDbType.BigInt) { Value = patientReq.Practice_Code };
            var _practicOrgId = new SqlParameter("@PRACTICE_ORGANIZATION_ID", SqlDbType.BigInt) { Value = Profile.PRACTICE_ORGANIZATION_ID ?? 0 };

            var result = SpRepository<IndexPatRes>.GetListWithStoreProcedure(@"exec Fox_Get_Patient_Info
                             @Last_Name, @First_Name, @Middle_Name, @SSN, @Gender, @Date_Of_Birth, @Chart_Id, @PRACTICE_CODE, @PRACTICE_ORGANIZATION_ID",
                         _lastName, _firstName, _middleName, _ssn, _gender, _dob, _chartId, _practiceCode, _practicOrgId);
            if (result.Any()) { return result; }
            else
            {
                return new List<IndexPatRes>();
            }
        }
    }
}
