using FOX.DataModels.Context;
using FOX.DataModels.GenericRepository;
using FOX.DataModels.Models.Patient;
using FOX.DataModels.Models.Security;
using FOX.DataModels.Models.Settings.Announcement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FOX.BusinessOperations.SurveyAutomationService
{
    public class SurveyAutomationService : ISurveyAutomationService
    {
        private readonly DBContextSurveyAutomation _surveyAutomationContext = new DBContextSurveyAutomation();
        private readonly GenericRepository<Patient> _patientRepository;
        public SurveyAutomationService()
        {
            _patientRepository = new GenericRepository<Patient>(_surveyAutomationContext);

        }

        public List<FoxRoles> GetFoxRoles(UserProfile userProfile)
        {
            throw new NotImplementedException();
        }

        public List<Patient> GetPatientDetails()
        {
            List<Patient> patient = new List<Patient>();
           
            return patient;
        }
    }
}
