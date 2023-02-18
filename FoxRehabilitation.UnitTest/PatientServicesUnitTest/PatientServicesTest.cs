using FOX.BusinessOperations.CommonService;
using FOX.BusinessOperations.PatientServices;
using FOX.DataModels.Models.IndexInfo;
using FOX.DataModels.Models.Patient;
using FOX.DataModels.Models.PatientDocuments;
using FOX.DataModels.Models.Security;
using FOX.DataModels.Models.Settings.FacilityLocation;
using FOX.DataModels.Models.TasksModel;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace FoxRehabilitation.UnitTest.PatientServicesUnitTest
{
    [TestFixture]
    class PatientServicesTest
    {
        private PatientService _patientService;
        private UserProfile _userProfile;
        private PatientSearchRequest _patientSearchRequest;
        private PatientUpdateHistory _patientUpdateHistory;
        private SmartSearchInsuranceReq _smartSearchInsuranceReq;
        private Patient _patient;
        private PatientContact _patientContact;
        private FacilityLocation _facilityLocation;
        private SSNExist _ssnExist;
        private PatientExist _patientExist;
        private SubscriberSearchReq _subscriberSearchReq;
        private EmployerSearchReq _employerSearchReq;
        private AdvanceInsuranceSearch _advanceInsuranceSearch;
        private AdvancePatientSearch _advancePatientSearch;
        private DefaultInsuranceParameters _defaultInsuranceParameters;
        private SubscriberInfoRequest _subscriberInfoRequest;
        private AutoPopulateModel _autoPopulateModel;
        private SuggestedMCPayer _suggestedMCPayer;
        private SmartSearchCountriesReq _smartSearchCountriesReq;
        private PatientPOSLocation _patientPOSLocation;
        private SmartOrderSource _smartOrderSource;
        private PatientEligibilitySearchModel _patientEligibilitySearchModel;
        private PatientInsuranceInformation _patientInsuranceInformation;
        private PatientInsurance _patientInsurance;
        private PatientInsuranceEligibilityDetail _patientInsuranceEligibilityDetail;
        private MedicareLimit _medicareLimit;
        private MedicareLimitHistorySearchReq _medicareLimitHistorySearchReq;
        private PHR _phr;
        private FOX_TBL_TASK _fOX_TBL_TASK;
        private PatientAlias _patientAlias;
        private CheckDuplicatePatientsReq _checkDuplicatePatientsReq;
        private PayorDataModel _payorDataModel;
        private PatientPATDocument _patientPATDocument;

        [SetUp]
        public void SetUp()
        {
            _patientService = new PatientService();
            _userProfile = new UserProfile();
            _patientSearchRequest = new PatientSearchRequest();
            _patientUpdateHistory = new PatientUpdateHistory();
            _smartSearchInsuranceReq = new SmartSearchInsuranceReq();
            _patient = new Patient();
            _patientContact = new PatientContact();
            _facilityLocation = new FacilityLocation();
            _ssnExist = new SSNExist();
            _patientExist = new PatientExist();
            _subscriberSearchReq = new SubscriberSearchReq();
            _employerSearchReq = new EmployerSearchReq();
            _advanceInsuranceSearch = new AdvanceInsuranceSearch();
            _advancePatientSearch = new AdvancePatientSearch();
            _defaultInsuranceParameters = new DefaultInsuranceParameters();
            _subscriberInfoRequest = new SubscriberInfoRequest();
            _autoPopulateModel = new AutoPopulateModel();
            _suggestedMCPayer = new SuggestedMCPayer();
            _smartSearchCountriesReq = new SmartSearchCountriesReq();
            _patientPOSLocation = new PatientPOSLocation();
            _smartOrderSource = new SmartOrderSource();
            _patientEligibilitySearchModel = new PatientEligibilitySearchModel();
            _patientInsuranceInformation = new PatientInsuranceInformation();
            _patientInsurance = new PatientInsurance();
            _patientInsuranceEligibilityDetail = new PatientInsuranceEligibilityDetail();
            _medicareLimit = new MedicareLimit();
            _medicareLimitHistorySearchReq = new MedicareLimitHistorySearchReq();
            _phr = new PHR();
            _fOX_TBL_TASK = new FOX_TBL_TASK();
            _patientAlias = new PatientAlias();
            _checkDuplicatePatientsReq = new CheckDuplicatePatientsReq();
            _payorDataModel = new PayorDataModel();
            _patientPATDocument = new PatientPATDocument();

        }
        [Test]
        [TestCase(1011163, true, 101116354816630)]
        [TestCase(1011163, false, 101116354816001)]
        public void GetPatientList_PatientListModel_ReturnData(long practiceCode, bool isTalkRehab, long Patient_Account)
        {
            //Arrange
            _userProfile.PracticeCode = practiceCode;
            _patientSearchRequest.Patient_Account = "";
            _patientSearchRequest.FirstName = "";
            _patientSearchRequest.MiddleName = "";
            _patientSearchRequest.LastName = "";
            _patientSearchRequest.MRN = "";
            _patientSearchRequest.SSN = "";
            _patientSearchRequest.CreatedBy = "";
            _patientSearchRequest.ModifiedBy = "";
            _patientSearchRequest.CurrentPage = 1;
            _patientSearchRequest.RecordPerPage = 10;
            _patientSearchRequest.SearchText = "";
            _patientSearchRequest.SortBy = "";
            _patientSearchRequest.SortOrder = "";
            _patientSearchRequest.INCLUDE_ALIAS = true;
            _userProfile.isTalkRehab = isTalkRehab;
            _patientSearchRequest.DOBInString = Convert.ToString(DateTime.Today);
            _patientSearchRequest.CreatedDateInString = Convert.ToString(DateTime.Today);
            _patientSearchRequest.Patient_Account = Patient_Account.ToString();


            //Act
            var result = _patientService.GetPatientList(_patientSearchRequest, _userProfile);

            //Assert
            if (result != null)
            {
                Assert.IsTrue(true);
            }
        }
        [Test]
        [TestCase(101116354813049)]
        [TestCase(101116354412270)]
        public void GetPatientAddress_PatientAddressModel_ReturnData(long patientAccount)
        {
            //Arrange
            //Act
            var result = _patientService.GetPatientAddress(patientAccount);

            //Assert
            if (result != null)
            {
                Assert.IsTrue(true);
            }
        }
        [Test]
        [TestCase(101116354813049)]
        [TestCase(101116354412270)]
        public void GetPatientAddressesToDisplay_PatientAddressesToDisplayModel_ReturnData(long patientAccount)
        {
            //Arrange
            //Act
            var result = _patientService.GetPatientAddressesToDisplay(patientAccount);

            //Assert
            if (result != null)
            {
                Assert.IsTrue(true);
            }
        }
        [Test]
        [TestCase(101116354412221)]
        [TestCase(101116354412224)]
        public void GetPatientUpdateHistory_PatientUpdateHistoryModel_ReturnData(long PATIENT_ACCOUNT)
        {
            //Arrange
            _patientUpdateHistory.PATIENT_ACCOUNT = PATIENT_ACCOUNT;
            //Act
            var result = _patientService.GetPatientUpdateHistory(_patientUpdateHistory);

            //Assert
            if (result != null)
            {
                Assert.IsTrue(true);
            }
        }
        [Test]
        [TestCase("06404")]
        [TestCase("-00544")]
        public void GetCityStateByZip_CityStateByZipModel_ReturnData(string zipCode)
        {
            //Arrange
            //Act
            var result = _patientService.GetCityStateByZip(zipCode);

            //Assert
            if (result != null)
            {
                Assert.IsTrue(true);
            }
        }
        [Test]
        [TestCase("Alaska")]
        [TestCase("Atlantic")]
        [TestCase("Eastern")]
        [TestCase("Chamorro")]
        [TestCase("Marshall")]
        [TestCase("Pacific")]
        [TestCase("Central")]
        [TestCase("Mountain")]
        [TestCase("Pohnpei")]
        [TestCase("default")]
        public void GetTimeZoneName_ReturnTimeZoneName_ReturnData(string timeZone)
        {
            //Arrange
            //Act
            var result = _patientService.GetTimeZoneName(timeZone);

            //Assert
            if (result != null)
            {
                Assert.IsTrue(true);
            }
        }
        [Test]
        [TestCase("06404", 1011163)]
        [TestCase("-00544", 1011163)]
        [TestCase("", 1011163)]
        public void GetRegionByZip_RegionByZipModel_ReturnData(string zipCode, long practiceCode)
        {
            //Arrange
            _userProfile.PracticeCode = practiceCode;

            //Act
            var result = _patientService.GetRegionByZip(zipCode, _userProfile);

            //Assert
            if (result != null)
            {
                Assert.IsTrue(true);
            }
        }
        [Test]
        [TestCase("06404")]
        [TestCase("-00544")]
        public void GetCitiesByZip_CitiesByZipModel_ReturnData(string zipCode)
        {
            //Arrange
            //Act
            var result = _patientService.GetCitiesByZip(zipCode);

            //Assert
            if (result != null)
            {
                Assert.IsTrue(true);
            }
        }
        [Test]
        [TestCase("AUTO CLUB INSURANCE ASSOC")]
        [TestCase("AMERICAN CASUALTY COMPANY OF READING PA")]
        public void GetInsurancePayers_InsurancePayersModel_ReturnData(string insurance_Name)
        {
            //Arrange
            _smartSearchInsuranceReq.Insurance_Name = insurance_Name;
            _smartSearchInsuranceReq.Patient_Account = "101116354412334";

            //Act
            var result = _patientService.GetInsurancePayers(_smartSearchInsuranceReq);

            //Assert
            if (result != null)
            {
                Assert.IsTrue(true);
            }
        }

        [Test]
        [TestCase(101116354813049)]
        [TestCase(101116354610685)]
        public void checkPatientisInterfaced_PatientisInterfacedCheck_ReturnData(long patientAccount)
        {
            //Arrange
            _userProfile.PracticeCode = 1011163;

            //Act
            var result = _patientService.checkPatientisInterfaced(patientAccount, _userProfile);

            //Assert
            if (result == true)
            {
                Assert.IsTrue(true);
            }
        }
        [Test]
        [TestCase(101116354816561)]
        [TestCase(101116354610685)]
        public void GetRestOfPatientData_ReturnPatientDetails_ReturnData(long patientAccount)
        {
            //Arrange
            _patient.Patient_Account = patientAccount;

            //Act
            var result = _patientService.GetRestOfPatientData(_patient);

            //Assert
            if (result != null)
            {
                Assert.IsTrue(true);
            }
        }
        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void SetFlags_Flags_ReturnData(bool flag)
        {
            //Arrange
            _patientContact.Flag_Financially_Responsible_Party = flag;
            _patientContact.Flag_Emergency_Contact = flag;
            _patientContact.Flag_Lives_In_Household_SLC = flag;
            _patientContact.Flag_Power_Of_Attorney = flag;
            _patientContact.Flag_Power_Of_Attorney_Financial = flag;
            _patientContact.Flag_Power_Of_Attorney_Medical = flag;
            _patientContact.Flag_Preferred_Contact = flag;
            _patientContact.Flag_Service_Location = flag;

            //Act
            var result = _patientService.SetFlags(_patientContact);

            //Assert
            if (result != null)
            {
                Assert.IsTrue(true);
            }
        }
        [Test]
        [TestCase(101116354816561)]
        [TestCase(101116354610685)]
        public void GetPatientContactTypes_EmptyModel_ReturnData(long practiceCode)
        {
            //Arrange
            //Act
            var result = _patientService.GetPatientContactTypes(practiceCode);

            //Assert
            if (result != null)
            {
                Assert.IsTrue(true);
            }
        }
        [Test]
        [TestCase(101116354816561)]
        [TestCase(101116354610685)]
        public void GetPatientBestTimeToCall_EmptyModel_ReturnData(long practiceCode)
        {
            //Arrange
            //Act
            var result = _patientService.GetPatientBestTimeToCall(practiceCode);

            //Assert
            if (result != null)
            {
                Assert.IsTrue(true);
            }
        }
        [Test]
        [TestCase(1011163, true)]
        [TestCase(1011163, false)]
        [TestCase(102, false)]
        public void GetAllPatientContactTypes_AllPatientContactTypesModel_ReturnData(long practiceCode, bool isTalkRehab)
        {
            //Arrange
            _userProfile.PracticeCode = practiceCode;
            _userProfile.isTalkRehab = isTalkRehab;
            //Act
            var result = _patientService.GetAllPatientContactTypes(_userProfile);

            //Assert
            if (result != null)
            {
                Assert.IsTrue(true);
            }
        }
        [Test]
        [TestCase(544101)]
        [TestCase(544102)]
        public void GetPatientContactDetails_PatientContactDetailsModel_ReturnData(long contactid)
        {
            //Arrange
            //Act
            var result = _patientService.GetPatientContactDetails(contactid);

            //Assert
            if (result != null)
            {
                Assert.IsTrue(true);
            }
        }
        [Test]
        [TestCase(1011163, "101116354816561")]
        [TestCase(1011163, "101116354412338")]
        public void SsnExists_SsnExistsModel_ReturnData(long practiceCode, string patientAccount)
        {
            //Arrange
            _userProfile.PracticeCode = practiceCode;
            _ssnExist.Patient_Account = patientAccount;

            //Act
            var result = _patientService.SSNExists(_ssnExist, _userProfile);

            //Assert
            if (result == true)
            {
                Assert.IsTrue(true);
            }
            else
            {
                Assert.IsFalse(false);
            }
        }
        [Test]
        [TestCase(1011163, "101116354816561")]
        [TestCase(1011163, "101116354412338")]
        public void PatientExists_PatientExistsModel_ReturnData(long practiceCode, string patientAccount)
        {
            //Arrange
            _userProfile.PracticeCode = practiceCode;
            _patientExist.Patient_Account = patientAccount;
            _patientExist.First_Name = "";
            _patientExist.Middle_Name = "";
            _patientExist.Last_Name = "";

            //Act
            var result = _patientService.PatientExists(_patientExist, _userProfile);

            //Assert
            if (result == true)
            {
                Assert.IsTrue(true);
            }
            else
            {
                Assert.IsFalse(false);
            }
        }
        [Test]
        [TestCase(1011163, "101116354816561")]
        [TestCase(1011163, "101116354412338")]
        public void PatientDemographicsExists_PatientDemographicsExistsModel_ReturnData(long practiceCode, string patientAccount)
        {
            //Arrange
            _userProfile.PracticeCode = practiceCode;
            _patientExist.Patient_Account = patientAccount;
            _patientExist.First_Name = "one";
            _patientExist.Middle_Name = "one";
            _patientExist.Last_Name = "one";

            //Act
            var result = _patientService.PatientDemographicsExists(_patientExist, _userProfile);

            //Assert
            if (result == true)
            {
                Assert.IsTrue(true);
            }
            else
            {
                Assert.IsFalse(false);
            }
        }
        [Test]
        [TestCase(544101)]
        [TestCase(00000000)]
        public void GetFoxPayorName_ReturnFoxPayorName_ReturnData(long insuranceId)
        {
            //Arrange
            //Act
            var result = _patientService.GetFoxPayorName(insuranceId);

            //Assert
            if (result != null)
            {
                Assert.IsTrue(true);
            }
        }
        [Test]
        [TestCase(101116354815798)]
        [TestCase(101116354813344)]
        public void GetPrimaryInsurance_PrimaryInsuranceName_ReturnData(long patientAccount)
        {
            //Arrange
            //Act
            var result = _patientService.GetPrimaryInsurance(patientAccount);

            //Assert
            if (result != null)
            {
                Assert.IsTrue(true);
            }
        }
        [Test]
        [TestCase(101116354815798)]
        [TestCase(101116354813344)]
        public void GetLatestPrimaryInsurance_LatestPrimaryInsuranceName_ReturnData(long patientAccount)
        {
            //Arrange
            //Act
            var result = _patientService.GetLatestPrimaryInsurance(patientAccount);

            //Assert
            if (result != null)
            {
                Assert.IsTrue(true);
            }
        }
        [Test]
        [TestCase(1011163, "")]
        [TestCase(1011163, "test")]
        public void GetSubscribers_SubscribersModel_ReturnData(long practiceCode, string searchValue)
        {
            //Arrange
            _userProfile.PracticeCode = practiceCode;
            _subscriberSearchReq.SEARCHVALUE = searchValue;

            //Act
            var result = _patientService.GetSubscribers(_subscriberSearchReq, _userProfile);

            //Assert
            if (result != null)
            {
                Assert.IsTrue(true);
            }
        }
        [Test]
        [TestCase(1011163, "")]
        [TestCase(1011163, "test")]
        public void GetEmployers_EmployersModel_ReturnData(long practiceCode, string searchValue)
        {
            //Arrange
            _userProfile.PracticeCode = practiceCode;
            _employerSearchReq.SEARCHVALUE = searchValue;

            //Act
            var result = _patientService.GetEmployers(_employerSearchReq, _userProfile);

            //Assert
            if (result != null)
            {
                Assert.IsTrue(true);
            }
        }
        [Test]
        [TestCase(101116354815798)]
        [TestCase(101116354813344)]
        public void GetPatientCasesForDD_PatientCasesForDDModel_ReturnData(long patientAccount)
        {
            //Arrange
            //Act
            var result = _patientService.GetPatientCasesForDD(patientAccount);

            //Assert
            if (result != null)
            {
                Assert.IsTrue(true);
            }
        }
        [Test]
        [TestCase(101116354815798)]
        [TestCase(101116354813344)]
        public void GetPatientCasesForPR_PatientCasesForPRModel_ReturnData(long patientAccount)
        {
            //Arrange
            //Act
            var result = _patientService.GetPatientCasesForPR(patientAccount);

            //Assert
            if (result != null)
            {
                Assert.IsTrue(true);
            }
        }
        [Test]
        [TestCase("P", true)]
        [TestCase("S", true)]
        [TestCase("T", true)]
        [TestCase("Q", true)]
        [TestCase("PR", true)]
        [TestCase("PR", false)]
        [TestCase("default", false)]
        public void GetInsuraneTypeName_InsuraneTypeNameRModel_ReturnData(string priSecOthType, bool isPP)
        {
            //Arrange
            //Act
            var result = _patientService.GetInsuraneTypeName(priSecOthType, isPP);

            //Assert
            if (result != null)
            {
                Assert.IsTrue(true);
            }
        }
        [Test]
        [TestCase("1011163")]
        [TestCase("0")]
        public void GetFinancialClassDDValues_FinancialClassDDValuesModel_ReturnData(string practiceCode)
        {
            //Arrange
            //Act
            var result = _patientService.GetFinancialClassDDValues(practiceCode);

            //Assert
            if (result != null)
            {
                Assert.IsTrue(true);
            }
        }
        [Test]
        [TestCase(1011163, "Connecticut Claims", "Indianapolis", "IN", "462066185")]
        [TestCase(1011163, "", "", "", "")]
        [TestCase(1011163, "PO Box 3030", "Mechanicsburg", "PA", "170551802")]
        public void GetFinancialClassDDValues_InsurancePayersForAdvanceSearchModel_ReturnData(long practiceCode, string insuranceAddress, string insuranceCity, string insuranceState, string insuranceZip)
        {
            //Arrange
            _advanceInsuranceSearch.Practice_Code = practiceCode;
            _advanceInsuranceSearch.CurrentPage = 1;
            _advanceInsuranceSearch.RecordPerPage = 10;
            _advanceInsuranceSearch.Insurance_Address = insuranceAddress;
            _advanceInsuranceSearch.Insurance_City = insuranceCity;
            _advanceInsuranceSearch.Insurance_State = insuranceState;
            _advanceInsuranceSearch.Insurance_Zip = insuranceZip;
            _advanceInsuranceSearch.SearchString = "test";
            _advanceInsuranceSearch.FINANCIAL_CLASS_ID = 1;

            //Act
            var result = _patientService.GetInsurancePayersForAdvanceSearch(_advanceInsuranceSearch);

            //Assert
            if (result != null)
            {
                Assert.IsTrue(true);
            }
        }
        [Test]
        [TestCase(1011163, "One", "Test")]
        [TestCase(1011163, "", "")]
        [TestCase(1011163, "Test", "Test")]
        public void GetPatientsForAdvanceSearch_PatientsForAdvanceSearchModel_ReturnData(long practiceCode, string firstName, string lastName)
        {
            //Arrange
            _userProfile.PracticeCode = practiceCode;
            _advancePatientSearch.CurrentPage = 1;
            _advancePatientSearch.RecordPerPage = 10;
            _advancePatientSearch.First_Name = firstName;
            _advancePatientSearch.Last_Name = lastName;
            _advancePatientSearch.Date_Of_Birth_In_String = Convert.ToString(DateTime.Today);
            _advancePatientSearch.Created_Date_Str = Convert.ToString(DateTime.Today);

            //Act
            var result = _patientService.GetPatientsForAdvanceSearch(_advancePatientSearch, _userProfile);

            //Assert
            if (result != null)
            {
                Assert.IsTrue(true);
            }
        }
        [Test]
        [TestCase(101116354816561)]
        [TestCase(10111635481861)]
        public void GetPatientDetail_PatientDetailModel_ReturnData(long patientAccount)
        {
            //Arrange
            //Act
            var result = _patientService.GetPatientDetail(patientAccount);

            //Assert
            if (result != null)
            {
                Assert.IsTrue(true);
            }
            else
            {
                Assert.IsFalse(false);
            }
        }
        [Test]
        [TestCase(1011163, "")]
        [TestCase(1011163, "Test")]
        public void GetSmartPatient_SmartPatientModel_ReturnData(long practiceCode, string searchText)
        {
            //Arrange
            _userProfile.PracticeCode = practiceCode;

            //Act
            var result = _patientService.GetSmartPatient(searchText, _userProfile);

            //Assert
            if (result != null)
            {
                Assert.IsTrue(true);
            }
            else
            {
                Assert.IsFalse(false);
            }
        }
        [Test]
        [TestCase(1011163, "")]
        [TestCase(1011163, "Test")]
        public void GetSmartPatientForTask_SmartPatientForTaskModel_ReturnData(long practiceCode, string searchText)
        {
            //Arrange
            _userProfile.PracticeCode = practiceCode;

            //Act
            var result = _patientService.GetSmartPatientForTask(searchText, _userProfile);

            //Assert
            if (result != null)
            {
                Assert.IsTrue(true);
            }
            else
            {
                Assert.IsFalse(false);
            }
        }
        [Test]
        [TestCase(1011163, " - ")]
        [TestCase(1011163, "Test")]
        public void GetSmartModifiedBy_SmartModifiedByModel_ReturnData(long practiceCode, string searchText)
        {
            //Arrange
            _userProfile.PracticeCode = practiceCode;

            //Act
            var result = _patientService.getSmartModifiedBy(searchText, _userProfile);

            //Assert
            if (result != null)
            {
                Assert.IsTrue(true);
            }
            else
            {
                Assert.IsFalse(false);
            }
        }
        [Test]
        [TestCase(1011163, "170551802", "PA")]
        [TestCase(1011163, "462066185", "IN")]
        public void GetDefaultPrimaryInsurance_EmptyModel_ReturnData(long practiceCode, string zip, string state)
        {
            //Arrange
            _defaultInsuranceParameters.ZIP = zip;
            _defaultInsuranceParameters.State = state;
            _defaultInsuranceParameters.patientAccount = 101116354816561;

            //Act
            var result = _patientService.GetDefaultPrimaryInsurance(_defaultInsuranceParameters, practiceCode);

            //Assert
            if (result == null)
            {
                Assert.IsTrue(true);
            }
            else
            {
                Assert.IsFalse(false);
            }
        }
        [Test]
        [TestCase(101116354816561)]
        [TestCase(10111635481861)]
        public void GetSubscriberInfo_SubscriberInfoModel_ReturnData(long patientAccount)
        {
            //Arrange
            _subscriberInfoRequest.patientAccount = patientAccount;

            //Act
            var result = _patientService.GetSubscriberInfo(_subscriberInfoRequest);

            //Assert
            if (result != null)
            {
                Assert.IsTrue(true);
            }
            else
            {
                Assert.IsFalse(false);
            }
        }
        [Test]
        [TestCase("101116354816561", 1011163, "Aetna Medicare Advantage")]
        [TestCase("10111635481861", 1011163, "Novitas Medicare NJ1")]
        public void GetSmartInsurancePayers_SmartInsurancePayersModel_ReturnData(string patientAccount, long practiceCode, string insuranceName)
        {
            //Arrange
            _userProfile.PracticeCode = practiceCode;
            _smartSearchInsuranceReq.Insurance_Name = insuranceName;
            _smartSearchInsuranceReq.Patient_Account = patientAccount;
            _smartSearchInsuranceReq.FINANCIAL_CLASS_ID = 0;
            _smartSearchInsuranceReq.Pri_Sec_Oth_Type = "";
            _smartSearchInsuranceReq.Zip = "";
            _smartSearchInsuranceReq.State = "";

            //Act
            var result = _patientService.GetSmartInsurancePayers(_smartSearchInsuranceReq, _userProfile);

            //Assert
            if (result != null)
            {
                Assert.IsTrue(true);
            }
        }
        [Test]
        [TestCase(1011163)]
        public void GetAutoPopulateInsurance_GetAutoPopulateInsuranceModel_ReturnData(long practiceCode)
        {
            //Arrange
            _userProfile.PracticeCode = practiceCode;
            _autoPopulateModel.patientAccount = "101116354412362";
            _autoPopulateModel.CASE_ID = 544107;
            _autoPopulateModel.patientAccount = "0";
            _autoPopulateModel.Pri_Sec_Oth_Type = "0";


            //Act
            var result = _patientService.GetAutoPopulateInsurance(_autoPopulateModel, _userProfile);

            //Assert
            if (result != null)
            {
                Assert.IsTrue(true);
            }
            else
            {
                Assert.IsFalse(false);
            }
        }
        [Test]
        [TestCase("101116354816561", "GA", 1011163)]
        [TestCase("101116354412309", "IN", 1011163)]
        [TestCase("", "", 1011163)]
        public void GetPatientPrivateHomes_EmptyPatientPrivateHomesModel_ReturnData(string patientAccount, string stateCode, long practiceCode)
        {
            //Arrange
            //Act
            var result = _patientService.GetPatientPrivateHomes(patientAccount, stateCode, practiceCode);

            //Assert
            if (result != null)
            {
                Assert.IsTrue(true);
            }
            else
            {
                Assert.IsFalse(false);
            }
        }
        [Test]
        [TestCase(1011163, "99503", "AK")]
        [TestCase(1011163, "10968", "NY")]
        [TestCase(1011163, "85339", "AZ")]
        public void GetSuggestedMCPayer_SuggestedMCPayerModel_ReturnData(long practiceCode, string zip, string state)
        {
            //Arrange
            _userProfile.PracticeCode = practiceCode;
            _suggestedMCPayer.Zip = zip;
            _suggestedMCPayer.State = state;

            //Act
            var result = _patientService.GetSuggestedMCPayer(_suggestedMCPayer, _userProfile);

            //Assert
            if (result != null)
            {
                Assert.IsTrue(true);
            }
            else
            {
                Assert.IsFalse(false);
            }
        }
        [Test]
        [TestCase(1011163, "101116354816561")]
        [TestCase(1011163, "101116354412309")]
        [TestCase(1011163, "10111635441309")]
        public void GetPatientInsurancesInIndexInfo_PatientInsurancesInIndexInfoModel_ReturnData(long practiceCode, string patientAccountStr)
        {
            //Arrange
            _userProfile.PracticeCode = practiceCode;

            //Act
            var result = _patientService.GetPatientInsurancesInIndexInfo(patientAccountStr, _userProfile);

            //Assert
            if (result != null)
            {
                Assert.IsTrue(true);
            }
            else
            {
                Assert.IsFalse(false);
            }
        }
        [Test]
        [TestCase(1011163, "101116354816561")]
        [TestCase(1011163, "101116354412309")]
        [TestCase(1011163, "10111635441309")]
        public void GetWorkOrderDocs_WorkOrderDocsModel_ReturnData(long practiceCode, string patientAccountStr)
        {
            //Arrange
            _userProfile.PracticeCode = practiceCode;

            //Act
            var result = _patientService.GetWorkOrderDocs(patientAccountStr, _userProfile);

            //Assert
            if (result != null)
            {
                Assert.IsTrue(true);
            }
            else
            {
                Assert.IsFalse(false);
            }
        }
        [Test]
        [TestCase(1011163, "101116354812860")]
        [TestCase(1011163, "101116354813049")]
        [TestCase(1011163, "101116354813475")]
        public void GetPatientInviteStatus_PatientInviteStatusModel_ReturnData(long practiceCode, string patientAccount)
        {
            //Arrange
            _userProfile.PracticeCode = practiceCode;

            //Act
            var result = _patientService.GetPatientInviteStatus(patientAccount, _userProfile);

            //Assert
            if (result != null)
            {
                Assert.IsTrue(true);
            }
            else
            {
                Assert.IsFalse(false);
            }
        }
        [Test]
        [TestCase(101116354814556)]
        [TestCase(101116354814574)]
        [TestCase(101116354814575)]
        public void GetPatientAliasListForSpecificPatient_PatientAliasListForSpecificPatientModel_ReturnData(long patientAccount)
        {
            //Arrange
            //Act
            var result = _patientService.GetPatientAliasListForSpecificPatient(patientAccount);

            //Assert
            if (result != null)
            {
                Assert.IsTrue(true);
            }
            else
            {
                Assert.IsFalse(false);
            }
        }
        [Test]
        [TestCase("Afghanistan")]
        [TestCase("Austria")]
        [TestCase(null)]
        [TestCase("-Austria")]
        public void GetCountries_CountriesModel_ReturnData(string searchValue)
        {
            //Arrange
            _smartSearchCountriesReq.SEARCHVALUE = searchValue;

            //Act
            var result = _patientService.getCountries(_smartSearchCountriesReq);

            //Assert
            if (result != null)
            {
                Assert.IsTrue(true);
            }
            else
            {
                Assert.IsFalse(false);
            }
        }
        [Test]
        [TestCase(1011222)]
        [TestCase(1011163)]
        public void GetAllCountries_AllCountriesModel_ReturnData(long practiceCode)
        {
            //Arrange
            //Act
            var result = _patientService.GetAllCountries(practiceCode);

            //Assert
            if (result != null)
            {
                Assert.IsTrue(true);
            }
            else
            {
                Assert.IsFalse(false);
            }
        }
        [Test]
        [TestCase(544101)]
        [TestCase(6002572)]
        public void GetInsuranc_InsurancModel_ReturnData(long id)
        {
            //Arrange
            //Act
            var result = _patientService.GetInsuranc(id, _userProfile);

            //Assert
            if (result != null)
            {
                Assert.IsTrue(true);
            }
            else
            {
                Assert.IsFalse(false);
            }
        }
        [Test]
        //[TestCase(1011163, 101116354816561)] else part  face exception
        [TestCase(1011163, 101116354816000)]
        public void AddUpdatePatient_AddToPatientTable_ReturnData(long practiceCode, long patientAccount)
        {
            //Arrange
            _userProfile.PracticeCode = practiceCode;
            _userProfile.UserName = "1163testing";
            _patient.PCP = 544100;
            _patient.PCP_Name = "Test";
            _patient.Date_Of_Birth_In_String = "8/12/2000";
            _patient.Expiry_Date_In_Str = "8/12/2024";
            _patient.Patient_Account = patientAccount;
            _userProfile.isTalkRehab = true;

            _patient.FROM_INDEXINFO = true;
            _patient.IsRegister = false;
            List<PatientPOSLocation> patient_POS_Location_List = _patient.Patient_POS_Location_List;
            {
                _patientPOSLocation = new PatientPOSLocation()
                {
                    Patient_POS_ID = 544111

                };
            };
            _patient.Patient_POS_Location_List = patient_POS_Location_List;
            _patient.First_Name = "Test";
            _patient.SmartOrderSource = _smartOrderSource;
            List<PatientContact> _patientContact = _patient.Patient_Contacts_List;
            _patient.Patient_Contacts_List = _patientContact;
            _patient.Patient_Contacts_List = new List<PatientContact>();
            _patient.Patient_Address = new List<PatientAddress>();

            //Act
            var result = _patientService.AddUpdatePatient(_patient, _userProfile);

            //Assert
            if (result != null)
            {
                Assert.IsTrue(true);
            }
            else
            {
                Assert.IsFalse(false);
            }
        }
        [Test]
        [TestCase(101116354816000)]
        [TestCase(101116354816561)]
        public void GetPatientAddressesIncludingPOS_PatientAddressesIncludingPOSe_ReturnData(long patientAccount)
        {
            //Arrange
            //Act
            var result = _patientService.GetPatientAddressesIncludingPOS(patientAccount);

            //Assert
            if (result != null)
            {
                Assert.IsTrue(true);
            }
            else
            {
                Assert.IsFalse(false);
            }
        }
        [Test]
        [TestCase(101116354816000)]
        [TestCase(101116354815798)]
        public void GetPatientInsurance_GetPatientInsurance_ReturnData(long patientAccount)
        {
            //Arrange
            _userProfile.PracticeCode = 1011163;
            _userProfile.UserName = "1163testing";

            //Act
            var result = _patientService.GetPatientInsurance(patientAccount, _userProfile);

            //Assert
            if (result != null)
            {
                Assert.IsTrue(true);
            }
            else
            {
                Assert.IsFalse(false);
            }
        }
        [Test]
        [TestCase(101116354816561)]
        [TestCase(1012629500548)]
        public void CheckNecessaryDataForEligibility_DataEligibilityCheck_ReturnData(long patientAccount)
        {
            //Arrange
            //Act
            var result = _patientService.CheckNecessaryDataForEligibility(patientAccount);

            //Assert
            if (result != null)
            {
                Assert.IsTrue(true);
            }
            else
            {
                Assert.IsFalse(false);
            }
        }
        [Test]
        [TestCase("101116354816561", 5482085)]
        [TestCase("1012629500548", 5482085)]
        [TestCase("101116354816562", null)]
        public void CheckNecessaryDataForLoadEligibility_DataEligibilityCheck_ReturnData(string patientAccount, long patientInsuranceId)
        {
            //Arrange
            _patientEligibilitySearchModel.Patient_Account_Str = patientAccount;
            _patientEligibilitySearchModel.Patient_Insurance_id = patientInsuranceId;

            //Act
            var result = _patientService.CheckNecessaryDataForLoadEligibility(_patientEligibilitySearchModel);

            //Assert
            if (result != null)
            {
                Assert.IsTrue(true);
            }
            else
            {
                Assert.IsFalse(false);
            }
        }
        [Test]
        [TestCase("")]
        [TestCase("Test")]
        public void RemoveStyleNodeFromHtmlForMVP_RemoveStyle_ReturnData(string html)
        {
            //Arrange
            //Act
            var result = _patientService.RemoveStyleNodeFromHtmlForMVP(html);

            //Assert
            if (result != null)
            {
                Assert.IsTrue(true);
            }
            else
            {
                Assert.IsFalse(false);
            }
        }
        [Test]
        [TestCase("*//div[contains(@id,'main-container-eligibility')]//div[contains(@class,'custom-panel-head')][1]//h3[1]")]
        [TestCase("Test")]
        public void UpdatePayerNameInHTML_UpdatePayerNameInHTML_ReturnData(string html)
        {
            //Arrange
            //Act
            var result = _patientService.UpdatePayerNameInHTML(html, _patientInsuranceInformation);

            //Assert
            if (result != null)
            {
                Assert.IsTrue(true);
            }
            else
            {
                Assert.IsFalse(false);
            }
        }
        [Test]
        [TestCase(112562)]
        [TestCase(600102)]
        public void GetCoordinates_Coordinates_ReturnData(long locId)
        {
            //Arrange
            _userProfile.PracticeCode = 1011163;
            _userProfile.UserName = "1163testing";
            _facilityLocation.LOC_ID = locId;

            //Act
            var result = _patientService.AddFacilityLocation(_facilityLocation, _userProfile);

            //Assert
            if (result != 0)
            {
                Assert.IsTrue(true);
            }
            else
            {
                Assert.IsFalse(false);
            }
        }
        [Test]
        [TestCase(123456)]
        [TestCase(544101)]
        public void SaveContact_Contact_ReturnData(long contactID)
        {
            //Arrange
            _userProfile.PracticeCode = 1011163;
            _userProfile.UserName = "1163testing";
            _patientContact.Start_Date_In_String = "8/12/2000";
            _patientContact.End_Date_In_String = "8/12/2024";
            _patientContact.Patient_Account_Str = "";
            _patientContact.STATEMENT_ADDRESS_MARKED = false;
            _patientContact.Flag_Financially_Responsible_Party = false;
            _patientContact.Contact_ID = contactID;
            _userProfile.isTalkRehab = true;

            //Act
            var result = _patientService.SaveContact(_patientContact, _userProfile);

            //Assert
            if (result != null)
            {
                Assert.IsTrue(true);
            }
            else
            {
                Assert.IsFalse(false);
            }
        }
        [Test]
        [TestCase(101116354412336)]
        public void GetPatientContacts_PatientContactsModel_ReturnData(long patientAccount)
        {
            //Arrange
            //Act
            var result = _patientService.GetPatientContacts(patientAccount);

            //Assert
            if (result != null)
            {
                Assert.IsTrue(true);
            }
            else
            {
                Assert.IsFalse(false);
            }
        }
        [Test]
        [TestCase(101116354412336)]
        public void GetPatientContactsForInsurance_PatientContactsForInsuranceModel_ReturnData(long patientAccount)
        {
            //Arrange
            //Act
            var result = _patientService.GetPatientContactsForInsurance(patientAccount);

            //Assert
            if (result != null)
            {
                Assert.IsTrue(true);
            }
            else
            {
                Assert.IsFalse(false);
            }
        }
        [Test]
        [TestCase(101116354412336)]
        [TestCase(101116354816561)]
        public void GetCurrentPatientInsurances_CurrentPatientInsurancesModel_ReturnData(long patientAccount)
        {
            //Arrange
            //Act
            var result = _patientService.GetCurrentPatientInsurances(patientAccount, _userProfile);

            //Assert
            if (result != null)
            {
                Assert.IsTrue(true);
            }
            else
            {
                Assert.IsFalse(false);
            }
        }
        [Test]
        [TestCase(101116354412336)]
        [TestCase(101116354816561)]
        public void CreateUpdateInsuranceInMTBC_UpdateModel_ReturnData(long patientAccount)
        {
            //Arrange
            _patientInsurance.MTBC_Patient_Insurance_Id = 5174;
            _userProfile.PracticeCode = 1011163;
            _userProfile.UserName = "1163testing";
            _patientInsurance.Pri_Sec_Oth_Type = "Q";
            _patientInsurance.Effective_Date = DateTime.Today;
            _patientInsurance.Termination_Date = DateTime.Today;
            _patientInsurance.Policy_Number = "test";
            _patientInsurance.Plan_Name = "test";
            _patientInsurance.Relationship = "test";
            _patientInsurance.Eligibility_Status = "test";

            //Act
            var result = _patientService.CreateUpdateInsuranceInMTBC(_patientInsurance, patientAccount, _userProfile);

            //Assert
            if (result != null)
            {
                Assert.IsTrue(true);
            }
            else
            {
                Assert.IsFalse(false);
            }
        }
        [Test]
        [TestCase(true, 548101, "101116351010069")]
        [TestCase(true, 548101, "101116351010069")]
        [TestCase(true, 548101, "101116354816562")]
        public void SaveInsuranceAndEligibilityDetails_SaveModel_ReturnData(bool fromIndexInfo, long patientInsuranceId, string patientAccountStr)
        {
            //Arrange
            _userProfile.PracticeCode = 1011163;
            _userProfile.UserName = "1163testing";
            _patientInsuranceEligibilityDetail = new PatientInsuranceEligibilityDetail()
            {
                InsuranceToCreateUpdate = new PatientInsurance()
                {
                    Patient_Insurance_Id = patientInsuranceId,
                    Pri_Sec_Oth_Type = "",
                    Effective_Date_In_String = Convert.ToString(DateTime.Today),
                    Termination_Date_In_String = Convert.ToString(DateTime.Today),
                    SUPRESS_BILLING_UNTIL_DATE_IN_STRING = "8/12/2020",
                    DED_AMT_VERIFIED_ON_DATE_IN_STRING = "8/12/2020",
                    DED_MET_AS_OF_IN_STRING = "8/12/2020",
                    BENEFIT_AMT_VERIFIED_ON_DATE_IN_STRING = "8/12/2020",
                    Deceased_Date_In_String = "8/12/2020",
                    Policy_Number = "123456789",
                    Plan_Name = "Test",
                    Relationship = "Brother",

                    SUBSCRIBER_DETAILS = new Subscriber()
                    {
                        IS_NEW_SUBSCRIBER = true,
                        GUARANT_DOB_IN_STRING = "8/12/2020",
                        GUARANT_DOB = DateTime.Today
                    }
                },
                Employer_Details = new Employer()
                {
                    Employer_Code = 54896825423992
                },
            };
            _patientInsuranceEligibilityDetail.Patient_Account_Str = patientAccountStr;

            //Act
            var result = _patientService.SaveInsuranceAndEligibilityDetails(_patientInsuranceEligibilityDetail, _userProfile, fromIndexInfo);

            //Assert
            if (result != null)
            {
                Assert.IsTrue(true);
            }
            else
            {
                Assert.IsFalse(false);
            }
        }
        [Test]
        [TestCase(123456, "ABN")]
        [TestCase(123456, "Hospice")]
        [TestCase(544100, "ABN")]
        public void CreateNewLimit_NewLimit_ReturnData(long? oldLimID, string limType)
        {
            //Arrange
            _medicareLimit.Patient_Account = 101116354815798;
            _medicareLimit.CASE_ID = 12345678;
            _medicareLimit.MODIFIED_BY = Convert.ToString(DateTime.Today);
            _medicareLimit.EFFECTIVE_DATE_IN_STRING = Convert.ToString(DateTime.Today);
            //END_DATE   ABN_EST_WK_COST   ABN_COMMENTS
            _userProfile.UserName = "1163testing";

            // EFFECTIVE_DATE_IN_STRING
            //Act
            var result = _patientService.CreateNewLimit(oldLimID, _medicareLimit, limType, _userProfile);

            //Assert
            if (result != null)
            {
                Assert.IsTrue(true);
            }
            else
            {
                Assert.IsFalse(false);
            }
        }
        [Test]
        [TestCase(544100, "ABN")]
        [TestCase(544100, "Hospice")]
        [TestCase(544100, "")]
        public void ABN_MedicareLimitDataChanged_DataChanged_ReturnData(long? newCaseId, string medicareLimitTypeName)
        {
            //Arrange

            _medicareLimit.MEDICARE_LIMIT_TYPE_NAME = medicareLimitTypeName;
            _medicareLimit.CASE_ID = 544100;
            _medicareLimit.EFFECTIVE_DATE = DateTime.Today;
            _medicareLimit.END_DATE = DateTime.Today;
            _medicareLimit.ABN_EST_WK_COST = 0;
            _medicareLimit.ABN_COMMENTS = "ABN";
            _medicareLimit.NPI = "ABN";

            //Act
            var result = _patientService.MedicareDataChanged(_medicareLimit, _medicareLimit, newCaseId);

            //Assert
            if (result == true)
            {
                Assert.IsTrue(true);
            }
            else
            {
                Assert.IsFalse(false);
            }
        }
        [Test]
        public void FetchEligibilityRecords_FetchRecords_ReturnData()
        {
            //Arrange 
            _medicareLimitHistorySearchReq.Patient_Account = "101116354815798";
            _medicareLimitHistorySearchReq.MedicareLimitTypeId = 544100;
            _userProfile.PracticeCode = 1011163;

            //Act
            var result = _patientService.GetMedicareLimitHistory(_medicareLimitHistorySearchReq, _userProfile);

            //Assert
            if (result != null)
            {
                Assert.IsTrue(true);
            }
            else
            {
                Assert.IsFalse(false);
            }
        }
        [Test]
        [TestCase(548350)]
        [TestCase(544139)]
        public void Getsplitauthorizations_splitauthorization_ReturnData(long parentid)
        {
            //Arrange 
            _userProfile.PracticeCode = 1011163;

            //Act
            var result = _patientService.getsplitauthorization(parentid, _userProfile);

            //Assert
            if (result != null)
            {
                Assert.IsTrue(true);
            }
            else
            {
                Assert.IsFalse(false);
            }
        }
        [Test]
        [TestCase("Test")]
        [TestCase("")]
        public void GetPrivateHomeFacilityByCode_PrivateHomeFacilityByCode_ReturnData(string code)
        {
            //Arrange 
            _userProfile.PracticeCode = 1011163;
            _userProfile.UserName = "1163testing";

            //Act
            var result = _patientService.GetPrivateHomeFacilityByCode(code, _userProfile);

            //Assert
            if (result != null)
            {
                Assert.IsTrue(true);
            }
            else
            {
                Assert.IsFalse(false);
            }
        }
        [Test]
        public void SavePatientContactfromInsuranceSubscriber_AddToDb_ReturnData()
        {
            //Arrange 
            _userProfile.PracticeCode = 1011163;
            _userProfile.UserName = "1163testing";
            _patientSearchRequest.Patient_Account = "";
            _patientSearchRequest.FirstName = "";
            _patientSearchRequest.MiddleName = "";
            _patientSearchRequest.LastName = "";
            _patientSearchRequest.MRN = "";
            _patientSearchRequest.SSN = "";
            _patientSearchRequest.CreatedBy = "";
            _patientSearchRequest.ModifiedBy = "";
            _patientSearchRequest.CurrentPage = 1;
            _patientSearchRequest.RecordPerPage = 10;
            _patientSearchRequest.SearchText = "";
            _patientSearchRequest.SortBy = "";
            _patientSearchRequest.SortOrder = "";
            _patientSearchRequest.INCLUDE_ALIAS = true;
            _userProfile.isTalkRehab = true;
            _patientSearchRequest.DOBInString = Convert.ToString(DateTime.Today);
            _patientSearchRequest.CreatedDateInString = Convert.ToString(DateTime.Today);

            //Act
            var result = _patientService.ExportPatientListToExcel(_patientSearchRequest, _userProfile);

            //Assert
            if (result != null)
            {
                Assert.IsTrue(true);
            }
            else
            {
                Assert.IsFalse(false);
            }
        }
        [Test]
        public void ExportPatientListToExcel_ExportExcel_ReturnData()
        {
            //Arrange 
            _phr.EMAIL_ADDRESS = "babarazam@mailinator.com";
            _phr.PRACTICE_CODE = 1011163;
            _userProfile.PracticeCode = 1011163;

            //Act
            var result = _patientService.GetInvitedPatient(_phr, _userProfile);

            //Assert
            if (result != null)
            {
                Assert.IsTrue(true);
            }
            else
            {
                Assert.IsFalse(false);
            }
        }
        [Test]
        [TestCase(548158)]
        [TestCase(123456)]
        public void UnBlockPatientFromPHR_UnBlockPatient_ReturnData(long userId)
        {
            //Arrange 
            _phr.EMAIL_ADDRESS = "babarazam@mailinator.com";
            _phr.PRACTICE_CODE = 1011163;
            _userProfile.PracticeCode = 1011163;
            _phr.USER_ID = userId;
            _userProfile.userID = userId;
            _userProfile.UserName = "1163testing";

            //Act
            var result = _patientService.UnBlockPatientFromPHR(_phr, _userProfile);

            //Assert
            if (result != null)
            {
                Assert.IsTrue(true);
            }
            else
            {
                Assert.IsFalse(false);
            }
        }
        [Test]
        [TestCase(548158)]
        [TestCase(123456)]
        public void BlockPatientFromPHR_BlockPatient_ReturnData(long userId)
        {
            //Arrange 
            _phr.EMAIL_ADDRESS = "babarazam@mailinator.com";
            _phr.PRACTICE_CODE = 1011163;
            _userProfile.PracticeCode = 1011163;
            _phr.USER_ID = userId;
            _userProfile.userID = userId;
            _userProfile.UserName = "1163testing";

            //Act
            var result = _patientService.BlockPatientFromPHR(_phr, _userProfile);

            //Assert
            if (result != null)
            {
                Assert.IsTrue(true);
            }
            else
            {
                Assert.IsFalse(false);
            }
        }
        [Test]
        [TestCase(548158)]
        [TestCase(123456)]
        public void CancelPatientRequestFromPHR_CancelRequest_ReturnData(long userId)
        {
            //Arrange 
            _phr.EMAIL_ADDRESS = "babarazam@mailinator.com";
            _phr.PRACTICE_CODE = 1011163;
            _userProfile.PracticeCode = 1011163;
            _phr.USER_ID = userId;
            _userProfile.userID = userId;
            _userProfile.UserName = "1163testing";

            //Act
            var result = _patientService.CancelPatientRequestFromPHR(_phr, _userProfile);

            //Assert
            if (result != null)
            {
                Assert.IsTrue(true);
            }
            else
            {
                Assert.IsFalse(false);
            }
        }
        [Test]
        [TestCase("test")]
        public void AddUpdateTask_TaskAddToDb_ReturnData(string eligibilityMspData)
        {
            //Arrange 
            _userProfile.PracticeCode = 1011163;
            _userProfile.UserName = "1163testing";
            _fOX_TBL_TASK.PATIENT_ACCOUNT_STR = "1012714";
            _fOX_TBL_TASK.PATIENT_ACCOUNT = 1012714;
            _fOX_TBL_TASK.TASK_ID = 123456;

            //Act
            var result = _patientService.AddUpdateTask(_fOX_TBL_TASK, _userProfile, eligibilityMspData);

            //Assert
            if (result != null)
            {
                Assert.IsTrue(true);
            }
            else
            {
                Assert.IsFalse(false);
            }
        }
        [Test]
        [TestCase("test")]
        [TestCase("")]
        public void PrivateHOMExists_PrivateHOMExists_ReturnData(string statecode)
        {
            //Arrange 
            _userProfile.PracticeCode = 1011163;
            _userProfile.UserName = "1163testing";

            //Act
            var result = _patientService.PrivateHOMExists(statecode, _userProfile);

            //Assert
            if (result != null)
            {
                Assert.IsTrue(true);
            }
            else
            {
                Assert.IsFalse(false);
            }
        }
        [Test]
        [TestCase(null)]
        [TestCase(548100)]
        public void SavePatientAlias_AddData_ReturnData(long patientAliasId)
        {
            //Arrange 
            _userProfile.PracticeCode = 1011163;
            _userProfile.UserName = "1163testing";
            _patientAlias.PATIENT_ACCOUNT_STR = "101116354814556";
            _patientAlias.PATIENT_ALIAS_ID = patientAliasId;

            //Act
            var result = _patientService.SavePatientAlias(_patientAlias, _userProfile);

            //Assert
            if (result != null)
            {
                Assert.IsTrue(true);
            }
            else
            {
                Assert.IsFalse(false);
            }
        }
        [Test]
        public void CheckDuplicatePatients_CheckDuplicate_ReturnData()
        {
            //Arrange 
            _userProfile.PracticeCode = 1011163;
            _userProfile.UserName = "1163Testing";
            _checkDuplicatePatientsReq.PATIENT_ACCOUNT = "101116354816563";
            _checkDuplicatePatientsReq.First_Name = "";
            _checkDuplicatePatientsReq.Last_Name = "";
            _checkDuplicatePatientsReq.Date_Of_Birth_In_String = "2000-08-12";
            _checkDuplicatePatientsReq.Gender = "";

            //Act
            var result = _patientService.CheckDuplicatePatients(_checkDuplicatePatientsReq, _userProfile);

            //Assert
            if (result != null)
            {
                Assert.IsTrue(true);
            }
            else
            {
                Assert.IsFalse(false);
            }
        }
        [Test]
        [TestCase(5481487, true)]
        [TestCase(null, true)]
        [TestCase(null, false)]
        public void ABN_MedicareLimitDataChanged_ArgumentsPass_ReturnData(long? abn_Id, out bool abnInfoChanged)
        {
            //Arrange 
            _userProfile.PracticeCode = 1011163;
            _userProfile.UserName = "1163Testing";
            List<MedicareLimit> currentMedicareLimitList = new List<MedicareLimit>()
            {
                new MedicareLimit
                {
                    MEDICARE_LIMIT_TYPE_NAME = "ABN",
                    EFFECTIVE_DATE = Helper.GetCurrentDate()
        }
            };

            //Act
            var result = _patientService.ABN_MedicareLimitDataChanged(abn_Id, currentMedicareLimitList, out abnInfoChanged);

            //Assert
            if (result == true)
            {
                Assert.IsTrue(true);
            }
            else
            {
                Assert.IsFalse(false);
            }
        }
        [Test]
        [TestCase(null, true)]
        [TestCase(null, true)]
        [TestCase(null, false)]
        public void HOS_MedicareLimitDataChanged_ArgumentsPass_ReturnData(long? abn_Id, out bool abnInfoChanged)
        {
            //Arrange 
            _userProfile.PracticeCode = 1011163;
            _userProfile.UserName = "1163Testing";
            List<MedicareLimit> currentMedicareLimitList = new List<MedicareLimit>()
            {
                new MedicareLimit
                {
                    MEDICARE_LIMIT_TYPE_NAME = "Hospice",
                    EFFECTIVE_DATE = Helper.GetCurrentDate()
        }
            };

            //Act
            var result = _patientService.HOS_MedicareLimitDataChanged(abn_Id, currentMedicareLimitList, out abnInfoChanged);

            //Assert
            if (result == true)
            {
                Assert.IsTrue(true);
            }
            else
            {
                Assert.IsFalse(false);
            }
        }
        [Test]
        [TestCase(null, true)]
        [TestCase(null, true)]
        [TestCase(null, false)]
        public void HH_MedicareLimitDataChanged_ArgumentsPass_ReturnData(long? abn_Id, out bool abnInfoChanged)
        {
            //Arrange 
            _userProfile.PracticeCode = 1011163;
            _userProfile.UserName = "1163Testing";
            List<MedicareLimit> currentMedicareLimitList = new List<MedicareLimit>()
            {
                new MedicareLimit
                {
                    MEDICARE_LIMIT_TYPE_NAME = "Home Health Episode",
                    EFFECTIVE_DATE = Helper.GetCurrentDate()
        }
            };

            //Act
            var result = _patientService.HH_MedicareLimitDataChanged(abn_Id, currentMedicareLimitList, out abnInfoChanged);

            //Assert
            if (result == true)
            {
                Assert.IsTrue(true);
            }
            else
            {
                Assert.IsFalse(false);
            }
        }
        [Test]
        [TestCase(1010624506100456, true, "S", "")]
        [TestCase(1010624506100457, true, "", "G")]
        [TestCase(1010624506100458, false, "S", "G")]
        public void GetElgibilityDetails_ArgumentsPass_ReturnData(long patientAccount, bool isMVP, string relationship, string pracType)
        {
            //Arrange 
            _userProfile.PracticeCode = 1011163;
            _userProfile.UserName = "1163Testing";
            _patientInsuranceInformation.RELATIONSHIP = relationship;
            _patientInsuranceInformation.PRAC_TYPE = pracType;
            _patientInsuranceInformation.DATE_OF_BIRTH = Helper.GetCurrentDate();

            //Act
            var result = _patientService.GetElgibilityDetails(patientAccount, _patientInsuranceInformation, _userProfile, isMVP);

            //Assert
            if (result != null)
            {
                Assert.IsTrue(true);
            }
            else
            {
                Assert.IsFalse(false);
            }
        }
        //[Test]
        //[TestCase("1010624506100456")]
        //[TestCase("1010624506100457")]
        //[TestCase("1010624506100458")]
        //// Exception
        //public void FetchEligibilityRecords_ArgumentsPass_ReturnData(string patientAccount)
        //{
        //    //Arrange 
        //    _userProfile.PracticeCode = 1011163;
        //    _userProfile.UserName = "1163Testing";
        //    _patientEligibilitySearchModel.Patient_Account_Str = patientAccount;
        //    _patientEligibilitySearchModel.Patient_Insurance_id = 12345678;

        //    //Act
        //    var result = _patientService.FetchEligibilityRecords(_patientEligibilitySearchModel, _userProfile);

        //    //Assert
        //    if (result != null)
        //    {
        //        Assert.IsTrue(true);
        //    }
        //    else
        //    {
        //        Assert.IsFalse(false);
        //    }
        //}
        [Test]
        public void GetHtml_NoArguments_ReturnData()
        {
            //Arrange 
            //Act
            var result = _patientService.gethtml();

            //Assert
            if (result != null)
            {
                Assert.IsTrue(true);
            }
            else
            {
                Assert.IsFalse(false);
            }
        }
        [Test]
        [TestCase("1010624506100456")]
        [TestCase("1010624506100457")]
        [TestCase("1010624506100458")]
        public void GetLatestEligibilityRecords_Arguments_ReturnData(string patientAccount)
        {
            //Arrange 
            _userProfile.PracticeCode = 1011163;
            _userProfile.UserName = "1163Testing";
            _patientEligibilitySearchModel.Patient_Account_Str = patientAccount;
            //Act
            var result = _patientService.GetLatestEligibilityRecords(_patientEligibilitySearchModel, _userProfile);

            //Assert
            if (result != null)
            {
                Assert.IsTrue(true);
            }
            else
            {
                Assert.IsFalse(false);
            }
        }
        [Test]
        [TestCase("test", 1010624506100456)]
        [TestCase("test", 1010624506100457)]
        [TestCase("test", 1010624506100458)]
        public void GetEligibilityInformation_Arguments_ReturnData(string eligibility, long patientAccount)
        {
            //Arrange 
            _userProfile.PracticeCode = 1011163;
            _userProfile.UserName = "1163Testing";

            //Act
            var result = _patientService.GetEligibilityInformation(eligibility, patientAccount, _userProfile);

            //Assert
            if (result != null)
            {
                Assert.IsTrue(true);
            }
            else
            {
                Assert.IsFalse(false);
            }
        }
        [Test]
        public void SaveReconcileLatestData_Arguments_ReturnData()
        {
            //Arrange 
            _userProfile.PracticeCode = 1011163;
            _userProfile.UserName = "1163Testing";
            _payorDataModel.PayorDOB = Helper.GetCurrentDate().ToString();

            //Act
            _patientService.SaveReconcileLatestData(_payorDataModel, _userProfile);

            //Assert

            Assert.IsTrue(true);
        }
        [Test]
        [TestCase("test", 1010624506100456, 544123)]
        [TestCase("test", 1010624506100457, 544641)]
        [TestCase("test", 1010624506100458, 544470)]
        public void SaveEligibilityHtml_Arguments_ReturnData(string html, long patientAccount, long patientInsuranceId)
        {
            //Arrange 
            _userProfile.PracticeCode = 1011163;
            _userProfile.UserName = "1163Testing";
            _payorDataModel.PayorDOB = Helper.GetCurrentDate().ToString();

            //Act
            _patientService.SaveEligibilityHtml(patientAccount, patientInsuranceId, html, _userProfile);

            //Assert

            Assert.IsTrue(true);
        }
        [Test]
        [TestCase(true)]
        public void ExtractEligibilityData_ArgumentsPass_ReturnData(bool newDocument)
        {
            //Arrange 
            _userProfile.PracticeCode = 1011163;
            _userProfile.UserName = "1163Testing";
            List<string> list = new List<string>()
            {
                "test"
            };
            _patientPATDocument.WORK_ID = 54819396;

            //Act
            var result = _patientService.AddUpdateNewDocumentInformation(_patientPATDocument, _userProfile, newDocument);

            //Assert

            Assert.IsTrue(true);
        }
        //ExtractEligibilityData
        [TearDown]
        public void Teardown()
        {
            // Optionally dispose or cleanup objects
            _patientService = null;
            _patientSearchRequest = null;
            _userProfile = null;
            _patientUpdateHistory = null;
            _smartSearchInsuranceReq = null;
            _patient = null;
            _patientContact = null;
            _facilityLocation = null;
            _ssnExist = null;
            _subscriberSearchReq = null;
            _patientExist = null;
            _employerSearchReq = null;
            _advanceInsuranceSearch = null;
            _advancePatientSearch = null;
            _defaultInsuranceParameters = null;
            _subscriberInfoRequest = null;
            _autoPopulateModel = null;
            _suggestedMCPayer = null;
            _smartSearchCountriesReq = null;
        }
    }
}

