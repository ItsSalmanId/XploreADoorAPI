using FOX.BusinessOperations.PatientServices;
using FOX.DataModels.Models.Authorization;
using FOX.DataModels.Models.CasesModel;
using FOX.DataModels.Models.Patient;
using FOX.DataModels.Models.Settings.FacilityLocation;
using FoxRehabilitationAPI.Filters;
using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace FoxRehabilitationAPI.Controllers
{
    [Authorize]
    [ExceptionHandlingFilter]
    public class PatientController : BaseApiController
    {
        private readonly IPatientService _patientServices;

        public PatientController(IPatientService userServices)
        {
            _patientServices = userServices;
        }

        [HttpPost]
        public Patient AddUpdatePatient([FromBody] Patient patient)
        {
            return _patientServices.AddUpdatePatient(patient, GetProfile());
        }
       [HttpPost]

        public HttpResponseMessage GetPatientList(PatientSearchRequest patientSearchRequest)
        {
            var getProfile = GetProfile();
            var getUserRight = getProfile.ApplicationUserRoles.Find(x => x.RIGHT_NAME.ToLower() == "patient maintenance");
            if (getUserRight != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, _patientServices.GetPatientList(patientSearchRequest, GetProfile()));
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Access Denied");
            }
        }

        [HttpPost]
        public HttpResponseMessage ExportToExcel(PatientSearchRequest patientSearchRequest)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _patientServices.ExportPatientListToExcel(patientSearchRequest, GetProfile()));
        }

        [HttpGet]
        public HttpResponseMessage GetPatientAddress(string patientAccount)
        {
            var PatientAddressList = _patientServices.GetPatientAddress(long.Parse(patientAccount));
            var response = Request.CreateResponse(HttpStatusCode.OK, PatientAddressList);
            return response;
        }

        [HttpGet]
        public HttpResponseMessage GetPatientAddressesIncludingPOS(string patientAccount)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _patientServices.GetPatientAddressesIncludingPOS(long.Parse(patientAccount)));
        }

        [HttpGet]
        public HttpResponseMessage GetPatientAddressesToDisplay(string patientAccount)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _patientServices.GetPatientAddressesToDisplay(long.Parse(patientAccount)));
        }

        [HttpGet]
        public HttpResponseMessage GetPatientInsurance(string patientAccount)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _patientServices.GetPatientInsurance(long.Parse(patientAccount), GetProfile()));
        }

        [HttpPost]
        public HttpResponseMessage GetPatientUpdateHistoryList(PatientUpdateHistory patientUpdateHistory)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _patientServices.GetPatientUpdateHistory(patientUpdateHistory));
        }

        [HttpGet]
        public HttpResponseMessage GetCityStateByZipCode(string zipCode)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _patientServices.GetCityStateByZip(zipCode));
        }

        [HttpGet]
        public HttpResponseMessage SearchCityStateAddressByAPI(string zipCode, string address)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _patientServices.SearchCityStateAddressByAPI(zipCode, address));
        }
        [HttpGet]
        public HttpResponseMessage GetRegionByZip(string zipCode)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _patientServices.GetRegionByZip(zipCode, GetProfile()));
        }

        [HttpPost]
        public HttpResponseMessage GetInsurances(SmartSearchInsuranceReq obj)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _patientServices.GetInsurancePayers(obj));
        }

        [HttpGet]
        public HttpResponseMessage CheckNecessaryDataForEligibility(string patientAccount)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _patientServices.CheckNecessaryDataForEligibility(long.Parse(patientAccount)));
        }

        [HttpPost]
        public HttpResponseMessage CheckNecessaryDataForLoadEligibility(PatientEligibilitySearchModel searchReq)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _patientServices.CheckNecessaryDataForLoadEligibility(searchReq));
        }

        

        //[HttpGet]
        //public HttpResponseMessage InsuranceEligibility(string patientAccount)
        //{
        //    var profile = GetProfile();
        //    return Request.CreateResponse(HttpStatusCode.OK, _patientServices.CheckPatientInsuranceEligibility(long.Parse(patientAccount), profile.userID.ToString()));
        //}

        [HttpPost]
        public HttpResponseMessage InsuranceEligibility(PatientEligibilitySearchModel patientEligibilitySearchModel)
        {
            var profile = GetProfile();
            return Request.CreateResponse(HttpStatusCode.OK, _patientServices.CheckPatientInsuranceEligibility(patientEligibilitySearchModel, profile.userID.ToString()));
        }

        [HttpGet]
        public HttpResponseMessage GetCurrentPatientDemographics(string patientAccount)
        {
            var profile = GetProfile();
            return Request.CreateResponse(HttpStatusCode.OK, _patientServices.GetCurrentPatientDemographics(long.Parse(patientAccount), GetProfile()));
        }

        [HttpPost]
        public HttpResponseMessage AddUpdatePatientPOSLocation([FromBody] PatientPOSLocation patientPOSData)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _patientServices.AddUpdatePatientPOSLocation(patientPOSData, GetProfile()));
        }

        [HttpGet]
        public HttpResponseMessage GetPatientContactTypes()
        {
            var profile = GetProfile();
            return Request.CreateResponse(HttpStatusCode.OK, _patientServices.GetPatientContactTypes(profile.PracticeCode));
        }
        [HttpGet]
        public HttpResponseMessage GetPatientBestTimeToCall()
        {
            var profile = GetProfile();
            return Request.CreateResponse(HttpStatusCode.OK, _patientServices.GetPatientBestTimeToCall(profile.PracticeCode));
        }
        [HttpGet]
        public HttpResponseMessage GetAllPatientContactTypes()
        {
            var profile = GetProfile();
            return Request.CreateResponse(HttpStatusCode.OK, _patientServices.GetAllPatientContactTypes(profile.PracticeCode));
        }
        [HttpGet]
        public HttpResponseMessage GetPatientContactDetails(string contactid)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _patientServices.GetPatientContactDetails(long.Parse(contactid)));
        }

        [HttpPost]
        public HttpResponseMessage SaveContact([FromBody] PatientContact contact)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _patientServices.SaveContact(contact, GetProfile()));
        }

        [HttpPost]
        public HttpResponseMessage SSNExists(SSNExist request)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _patientServices.SSNExists(request, GetProfile()));
        }

        [HttpPost]
        public HttpResponseMessage PatientExists(PatientExist request)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _patientServices.PatientExists(request, GetProfile()));
        }

        [HttpPost]
        public HttpResponseMessage DemographicsPatientExists(PatientExist request)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _patientServices.PatientDemographicsExists(request, GetProfile()));
        }

        [HttpGet]
        public HttpResponseMessage GetPatientContacts(string patientAccount)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _patientServices.GetPatientContacts(long.Parse(patientAccount)));
        }

        [HttpGet]
        public HttpResponseMessage GetPatientContactsForInsurance(string patientAccount)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _patientServices.GetPatientContactsForInsurance(long.Parse(patientAccount)));
        }

        [HttpGet]
        public HttpResponseMessage GetPrimaryInsurance(string patientAccount)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _patientServices.GetPrimaryInsurance(long.Parse(patientAccount)));
        }

        [HttpGet]
        public HttpResponseMessage GetLatestPrimaryInsurance(string patientAccount)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _patientServices.GetLatestPrimaryInsurance(long.Parse(patientAccount)));
        }

        [HttpGet]
        public HttpResponseMessage GetCurrentPatientInsurances(string patientAccount)
        {
            var profile = GetProfile();
            return Request.CreateResponse(HttpStatusCode.OK, _patientServices.GetCurrentPatientInsurances(long.Parse(patientAccount), profile));
        }

        [HttpPost]
        public HttpResponseMessage GetSubscribers(SubscriberSearchReq obj)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _patientServices.GetSubscribers(obj, GetProfile()));
        }

        [HttpPost]
        public HttpResponseMessage GetEmployers(EmployerSearchReq obj)
        {
            var subs = _patientServices.GetEmployers(obj, GetProfile());
            var response = Request.CreateResponse(HttpStatusCode.OK, subs);
            return response;
        }

        [HttpPost]
        public HttpResponseMessage SaveInsuranceAndEligibilityDetails([FromBody] PatientInsuranceEligibilityDetail patientInsAndEligDetails)
        {
            var Patient = _patientServices.SaveInsuranceAndEligibilityDetails(patientInsAndEligDetails, GetProfile(),false);
            return Request.CreateResponse(HttpStatusCode.OK, Patient);
        }

        [HttpPost]
        public HttpResponseMessage SaveDynamicPatientResponsibilityInsurance([FromBody] string pat_Account)
        {
            var Patient = _patientServices.SaveDynamicPatientResponsibilityInsurance(pat_Account, GetProfile());
            return Request.CreateResponse(HttpStatusCode.OK, Patient);

        }

        [HttpPost]
        public HttpResponseMessage FetchEligibilityRecords([FromBody] PatientEligibilitySearchModel patientEligibilitySearchModel)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _patientServices.FetchEligibilityRecords(patientEligibilitySearchModel, GetProfile()));
        }
        [HttpPost]
        public HttpResponseMessage FetchReconcileEligibilityRecords([FromBody] PatientEligibilitySearchModel patientEligibilitySearchModel)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _patientServices.GetLatestEligibilityRecords(patientEligibilitySearchModel, GetProfile()));
        }
        [HttpGet]
        public HttpResponseMessage GetCurrentPatientAuthorizations(string patientAccount)
        {
            var profile = GetProfile();
            return Request.CreateResponse(HttpStatusCode.OK, _patientServices.GetCurrentPatientAuthorizations(long.Parse(patientAccount), profile));
        }

        [HttpPost]
        public HttpResponseMessage SaveAuthDetails([FromBody] PatientInsuranceAuthDetails patientAuthDetails)
        {
            var Patient = _patientServices.SaveAuthDetails(patientAuthDetails, GetProfile());
            return Request.CreateResponse(HttpStatusCode.OK, "saved");
        }

        [HttpPost]
        public HttpResponseMessage GetMedicareLimitHistory([FromBody] MedicareLimitHistorySearchReq req)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _patientServices.GetMedicareLimitHistory(req, GetProfile()));
        }

        [HttpGet]
        public HttpResponseMessage getsplitauthorization(string parent_id)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _patientServices.getsplitauthorization(long.Parse(parent_id), GetProfile()));
        }

        [HttpPost]
        public HttpResponseMessage DeletePatPos([FromBody] PatientPOSLocation obj)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _patientServices.DeletePatPos(obj, GetProfile()));
        }

        [HttpGet]
        public HttpResponseMessage GetFinancialClassDDValues()
        {
            var profile = GetProfile();
            return Request.CreateResponse(HttpStatusCode.OK, _patientServices.GetFinancialClassDDValues(profile.PracticeCode.ToString()));
        }

        [HttpPost]
        public HttpResponseMessage GetInsurancePayersForAdvanceSearch(AdvanceInsuranceSearch obj)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _patientServices.GetInsurancePayersForAdvanceSearch(obj));
        }

        [HttpPost]
        public HttpResponseMessage GetPatientsForAdvanceSearch(AdvancePatientSearch obj)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _patientServices.GetPatientsForAdvanceSearch(obj, GetProfile()));
        }

        [HttpGet]
        public HttpResponseMessage GetPatientDetail(string patientaccount)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _patientServices.GetPatientDetail(Convert.ToInt64(patientaccount)));
        }

        [HttpGet]
        public HttpResponseMessage GetSmartPatient(string searchText)
        {
            var result = _patientServices.GetSmartPatient(searchText, GetProfile());
            var response = Request.CreateResponse(HttpStatusCode.OK, result);
            return response;
        }

        [HttpGet]
        public HttpResponseMessage GetSmartPatientFortask(string searchText)
        {
            var result = _patientServices.GetSmartPatientForTask(searchText, GetProfile());
            var response = Request.CreateResponse(HttpStatusCode.OK, result);
            return response;
        }
        [HttpGet]
        public HttpResponseMessage getSmartModifiedBy(string searchText)
        {
            var result = _patientServices.getSmartModifiedBy(searchText, GetProfile());
            var response = Request.CreateResponse(HttpStatusCode.OK, result);
            return response;
        }

        [HttpPost]
        public HttpResponseMessage GetDefaultPrimaryInsurance(DefaultInsuranceParameters defaultInsuranceParameters)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _patientServices.GetDefaultPrimaryInsurance(defaultInsuranceParameters, GetProfile().PracticeCode));
        }

        [HttpPost]
        public HttpResponseMessage GetSubscriberInfo(SubscriberInfoRequest subscriberinforequest)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _patientServices.GetSubscriberInfo(subscriberinforequest));
        }
        [HttpGet]
        public HttpResponseMessage GetPatientPrivateHomes(string patientAccount, string stateCode)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _patientServices.GetPatientPrivateHomes(patientAccount, stateCode, GetProfile().PracticeCode));
        }

        [HttpPost]
        public HttpResponseMessage GetSmartInsurancePayers(SmartSearchInsuranceReq obj)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _patientServices.GetSmartInsurancePayers(obj, GetProfile()));
        }

        [HttpGet]
        public HttpResponseMessage GetPrivateHomeFacilityByCode(string code)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _patientServices.GetPrivateHomeFacilityByCode(code, GetProfile()));
        }


        [HttpPost]
        public HttpResponseMessage GetSuggestedMCPayer(SuggestedMCPayer suggestedMCPayer)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _patientServices.GetSuggestedMCPayer(suggestedMCPayer, GetProfile()));
        }


        [HttpGet]
        public HttpResponseMessage GetPatientInsurancesInIndexInfo(string patientAccountStr)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _patientServices.GetPatientInsurancesInIndexInfo(patientAccountStr, GetProfile()));
        }

        [HttpPost]
        public HttpResponseMessage SendPatientInvite(PHR obj)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _patientServices.SendInviteToPatient(obj, GetProfile()));
        }
        [HttpPost]
        public HttpResponseMessage GetInvitedPatient(PHR obj)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _patientServices.GetInvitedPatient(obj, GetProfile()));
        }

        [HttpGet]
        public HttpResponseMessage GetPatientInviteStatus(string PatientAccount)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _patientServices.GetPatientInviteStatus(PatientAccount, GetProfile()));
        }

        [HttpPost]
        public HttpResponseMessage BlockPatientFromPHR(PHR obj)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _patientServices.BlockPatientFromPHR(obj, GetProfile()));
        }

        [HttpPost]
        public HttpResponseMessage UnBlockPatientFromPHR(PHR obj)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _patientServices.UnBlockPatientFromPHR(obj, GetProfile()));
        }

        [HttpPost]
        public HttpResponseMessage CancelPatientFromPHR(PHR obj)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _patientServices.CancelPatientRequestFromPHR(obj, GetProfile()));
        }

        [HttpPost]
        public HttpResponseMessage ResendRequestToPatient(PHR obj)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _patientServices.ResendRequestforPHRToPatient(obj, GetProfile()));
        }

        [HttpGet]
        public HttpResponseMessage GetWorkOrderDocs(string patientAccountStr)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _patientServices.GetWorkOrderDocs(patientAccountStr, GetProfile()));
        }
        [HttpPost]
        public HttpResponseMessage GetAutoPopulateInsurance(AutoPopulateModel obj)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _patientServices.GetAutoPopulateInsurance(obj, GetProfile()));
        }

        [HttpPost]
        public HttpResponseMessage GetWorkOrderInfo(WORK_ORDER_INFO_REQ obj)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _patientServices.GetWorkOrderInfo(obj, GetProfile()));
        }
        [HttpGet]
        public HttpResponseMessage checkPatientisInterfaced(long Patient_Account)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _patientServices.checkPatientisInterfaced(Patient_Account, GetProfile()));
        }

        //[HttpGet]
        //public HttpResponseMessage GetPatientDeceaseDate(string patientAccountStr)
        //{
        //    return Request.CreateResponse(HttpStatusCode.OK, _patientServices.GetPatientDeceaseDate(patientAccountStr, GetProfile()));
        //}

        [HttpGet]
        public HttpResponseMessage GetPatientCasesForDD(string patientAccountStr)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _patientServices.GetPatientCasesForDD(long.Parse(patientAccountStr)));
        }

        [HttpPost]
        public HttpResponseMessage SaveInsuranceEligibilityFromIndexInfo(PatientInsurance insuranceToCreateUpdate)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _patientServices.SaveInsuranceEligibilityFromIndexInfo(insuranceToCreateUpdate, GetProfile()));
        }
        
        [HttpGet]
        public HttpResponseMessage PrivateHOMExists(string state_code)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _patientServices.PrivateHOMExists(state_code, GetProfile()));
        }

        [HttpPost]
        public HttpResponseMessage DeleteInsuranceInformation(PatientInsuranceDetail req)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _patientServices.DeleteInsuranceInformation(req, GetProfile()));
        }

        [HttpGet]
        public HttpResponseMessage GetPatientAliasListForSpecificPatient(string patientAccountStr)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _patientServices.GetPatientAliasListForSpecificPatient(long.Parse(patientAccountStr)));
        }

        [HttpPost]
        public HttpResponseMessage SavePatientAlias(PatientAlias aliasToCreateUpdate)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _patientServices.SavePatientAlias(aliasToCreateUpdate, GetProfile()));
        }

        [HttpPost]
        public HttpResponseMessage getCountries(SmartSearchCountriesReq obj)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _patientServices.getCountries(obj));
        }
        [HttpGet]
        public HttpResponseMessage GetAllCountries()
        {
            var profile = GetProfile();
            return Request.CreateResponse(HttpStatusCode.OK, _patientServices.GetAllCountries(profile.PracticeCode));
        }

        [HttpPost]
        public HttpResponseMessage SaveEligibiltyDocument(DocumentSaveEligibility documentSaveEligibility )
        {
            var profile = GetProfile();
            return Request.CreateResponse(HttpStatusCode.OK, _patientServices.SaveEligibiltyDocument(documentSaveEligibility, profile));
        }

        [HttpPost]
        public HttpResponseMessage CheckDuplicatePatients(CheckDuplicatePatientsReq CheckDuplicatePatientsReq)
        {
            var profile = GetProfile();
            return Request.CreateResponse(HttpStatusCode.OK, _patientServices.CheckDuplicatePatients(CheckDuplicatePatientsReq, profile));
        }

        [HttpGet]
        public HttpResponseMessage GetInsuranc(long ID)
        {
            var profile = GetProfile();
            return Request.CreateResponse(HttpStatusCode.OK, _patientServices.GetInsuranc(ID, profile));
        }
        [HttpPost]
        public HttpResponseMessage ResetCordinate(FacilityLocation loc)
        {
            var profile = GetProfile();
            return Request.CreateResponse(HttpStatusCode.OK, _patientServices.ResetCoordinates(loc, GetProfile()));
        }
    }
}
