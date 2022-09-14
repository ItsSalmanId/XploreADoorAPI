using FOX.BusinessOperations.PatientServices;
using FOX.DataModels.GenericRepository;
using FOX.DataModels.Models.IndexInfo;
using FOX.DataModels.Models.Patient;
using FOX.DataModels.Models.Security;
using FOX.DataModels.Models.Settings.FacilityLocation;
using FOX.DataModels.Models.TasksModel;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


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

        }
    [   Test]
        [TestCase( 1011163,true)]
        [TestCase(1011163, false)]
        public void GetPatientList_PatientListModel_ReturnData(long practiceCode,bool isTalkRehab)
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

           //Act
           var result = _patientService.GetPatientList(_patientSearchRequest,_userProfile);

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
        public void GetRegionByZip_RegionByZipModel_ReturnData(string zipCode,long PracticeCode)
        {
            //Arrange
            _userProfile.PracticeCode = PracticeCode;

            //Act
            var result = _patientService.GetRegionByZip(zipCode,_userProfile);

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
        public void GetInsurancePayers_InsurancePayersModel_ReturnData(string Insurance_Name)
        {
            //Arrange
            _smartSearchInsuranceReq.Insurance_Name = Insurance_Name;
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
        public void GetRestOfPatientData_ReturnPatientDetails_ReturnData(long Patient_Account)
        {
            //Arrange
            _patient.Patient_Account = Patient_Account;

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
        public void SetFlags_Flags_ReturnData(bool Flag)
        {
            //Arrange
            _patientContact.Flag_Financially_Responsible_Party = Flag;
            _patientContact.Flag_Emergency_Contact = Flag;
            _patientContact.Flag_Lives_In_Household_SLC = Flag;
            _patientContact.Flag_Power_Of_Attorney = Flag;
            _patientContact.Flag_Power_Of_Attorney_Financial = Flag;
            _patientContact.Flag_Power_Of_Attorney_Medical = Flag;
            _patientContact.Flag_Preferred_Contact = Flag;
            _patientContact.Flag_Service_Location = Flag;

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
        [TestCase(1011163,true)]
        [TestCase(1011163, false)]
        [TestCase(102, false)]
        public void GetAllPatientContactTypes_AllPatientContactTypesModel_ReturnData(long practiceCode,bool isTalkRehab)
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
        public void SsnExists_SsnExistsModel_ReturnData(long PracticeCode,string Patient_Account)
        {
            //Arrange
            _userProfile.PracticeCode = PracticeCode;
            _ssnExist.Patient_Account = Patient_Account;

            //Act
            var result = _patientService.SSNExists(_ssnExist,_userProfile);

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
        public void PatientExists_PatientExistsModel_ReturnData(long PracticeCode, string Patient_Account)
        {
            //Arrange
            _userProfile.PracticeCode = PracticeCode;
            _patientExist.Patient_Account = Patient_Account;
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
        public void PatientDemographicsExists_PatientDemographicsExistsModel_ReturnData(long PracticeCode, string Patient_Account)
        {
            //Arrange
            _userProfile.PracticeCode = PracticeCode;
            _patientExist.Patient_Account = Patient_Account;
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
        public void GetFoxPayorName_ReturnFoxPayorName_ReturnData(long insurance_Id)
        {
            //Arrange
            //Act
            var result = _patientService.GetFoxPayorName(insurance_Id);

            //Assert
            if (result != null)
            {
                Assert.IsTrue(true);
            }
        }
        [Test]
        [TestCase(101116354815798)]
        [TestCase(101116354813344)]
        public void GetPrimaryInsurance_PrimaryInsuranceName_ReturnData(long patient_Account)
        {
            //Arrange
            //Act
            var result = _patientService.GetPrimaryInsurance(patient_Account);

            //Assert
            if (result != null)
            {
                Assert.IsTrue(true);
            }
        }
        [Test]
        [TestCase(101116354815798)]
        [TestCase(101116354813344)]
        public void GetLatestPrimaryInsurance_LatestPrimaryInsuranceName_ReturnData(long patient_Account)
        {
            //Arrange
            //Act
            var result = _patientService.GetLatestPrimaryInsurance(patient_Account);

            //Assert
            if (result != null)
            {
                Assert.IsTrue(true);
            }
        }
        [Test]
        [TestCase(1011163,"")]
        [TestCase(1011163,"test")]
        public void GetSubscribers_SubscribersModel_ReturnData(long PracticeCode,string SEARCHVALUE)
        {
            //Arrange
            _userProfile.PracticeCode = PracticeCode;
            _subscriberSearchReq.SEARCHVALUE = SEARCHVALUE;

            //Act
            var result = _patientService.GetSubscribers(_subscriberSearchReq,_userProfile);

            //Assert
            if (result != null)
            {
                Assert.IsTrue(true);
            }
        }
        [Test]
        [TestCase(1011163, "")]
        [TestCase(1011163, "test")]
        public void GetEmployers_EmployersModel_ReturnData(long PracticeCode, string SEARCHVALUE)
        {
            //Arrange
            _userProfile.PracticeCode = PracticeCode;
            _employerSearchReq.SEARCHVALUE = SEARCHVALUE;

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
        public void GetPatientCasesForDD_PatientCasesForDDModel_ReturnData(long patient_Account)
        {
            //Arrange
            //Act
            var result = _patientService.GetPatientCasesForDD(patient_Account);

            //Assert
            if (result != null)
            {
                Assert.IsTrue(true);
            }
        }
        [Test]
        [TestCase(101116354815798)]
        [TestCase(101116354813344)]
        public void GetPatientCasesForPR_PatientCasesForPRModel_ReturnData(long patient_Account)
        {
            //Arrange
            //Act
            var result = _patientService.GetPatientCasesForPR(patient_Account);

            //Assert
            if (result != null)
            {
                Assert.IsTrue(true);
            }
        }
        [Test]
        [TestCase("P",true)]
        [TestCase("S", true)]
        [TestCase("T", true)]
        [TestCase("Q", true)]
        [TestCase("PR", true)]
        [TestCase("PR", false)]
        [TestCase("default", false)]
        public void GetInsuraneTypeName_InsuraneTypeNameRModel_ReturnData(string pri_Sec_Oth_Type, bool is_PP)
        {
            //Arrange
            //Act
            var result = _patientService.GetInsuraneTypeName(pri_Sec_Oth_Type, is_PP);

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
        public void GetFinancialClassDDValues_InsurancePayersForAdvanceSearchModel_ReturnData(long practiceCode,string Insurance_Address, string Insurance_City, string Insurance_State, string Insurance_Zip)
        {
            //Arrange
            _advanceInsuranceSearch.Practice_Code = practiceCode;
            _advanceInsuranceSearch.CurrentPage = 1;
            _advanceInsuranceSearch.RecordPerPage = 10;
            _advanceInsuranceSearch.Insurance_Address = Insurance_Address;
            _advanceInsuranceSearch.Insurance_City = Insurance_City;
            _advanceInsuranceSearch.Insurance_State = Insurance_State;
            _advanceInsuranceSearch.Insurance_Zip = Insurance_Zip;
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
        public void GetPatientsForAdvanceSearch_PatientsForAdvanceSearchModel_ReturnData(long practiceCode, string First_Name, string Last_Name)
        {
            //Arrange
            _userProfile.PracticeCode = practiceCode;
            _advancePatientSearch.CurrentPage = 1;
            _advancePatientSearch.RecordPerPage = 10;
            _advancePatientSearch.First_Name = First_Name;
            _advancePatientSearch.Last_Name = Last_Name;
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
        public void GetPatientDetail_PatientDetailModel_ReturnData(long patient_Account)
        {
            //Arrange
            //Act
            var result = _patientService.GetPatientDetail(patient_Account);

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
        [TestCase(1011163,"")]
        [TestCase(1011163, "Test")]
        public void GetSmartPatient_SmartPatientModel_ReturnData(long PracticeCode, string searchText)
        {
            //Arrange
            _userProfile.PracticeCode = PracticeCode;

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
        public void GetSmartPatientForTask_SmartPatientForTaskModel_ReturnData(long PracticeCode, string searchText)
        {
            //Arrange
            _userProfile.PracticeCode = PracticeCode;

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
        public void GetSmartModifiedBy_SmartModifiedByModel_ReturnData(long PracticeCode, string searchText)
        {
            //Arrange
            _userProfile.PracticeCode = PracticeCode;

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
        public void GetDefaultPrimaryInsurance_EmptyModel_ReturnData(long PracticeCode, string ZIP, string State)
        {
            //Arrange
            _defaultInsuranceParameters.ZIP = ZIP; 
            _defaultInsuranceParameters.State = State; 
            _defaultInsuranceParameters.patientAccount = 101116354816561;

            //Act
            var result = _patientService.GetDefaultPrimaryInsurance(_defaultInsuranceParameters, PracticeCode);

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
        public void GetSmartInsurancePayers_SmartInsurancePayersModel_ReturnData(string patientAccount, long PracticeCode, string Insurance_Name)
        {
            //Arrange
            _userProfile.PracticeCode = PracticeCode;
            _smartSearchInsuranceReq.Insurance_Name = Insurance_Name;
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
        [TestCase( 1011163)]
        public void GetAutoPopulateInsurance_GetAutoPopulateInsuranceModel_ReturnData( long PracticeCode)
        {
            //Arrange
            _userProfile.PracticeCode = PracticeCode;
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
        public void GetSuggestedMCPayer_SuggestedMCPayerModel_ReturnData(long PracticeCode,string Zip,string State)
        {
            //Arrange
            _userProfile.PracticeCode = PracticeCode;
            _suggestedMCPayer.Zip = Zip;
            _suggestedMCPayer.State = State;
   
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
        public void GetPatientInsurancesInIndexInfo_PatientInsurancesInIndexInfoModel_ReturnData(long PracticeCode, string patientAccountStr)
        {
            //Arrange
            _userProfile.PracticeCode = PracticeCode;

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
        public void GetWorkOrderDocs_WorkOrderDocsModel_ReturnData(long PracticeCode, string patientAccountStr)
        {
            //Arrange
            _userProfile.PracticeCode = PracticeCode;

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
        public void GetPatientInviteStatus_PatientInviteStatusModel_ReturnData(long PracticeCode, string PatientAccount)
        {
            //Arrange
            _userProfile.PracticeCode = PracticeCode;

            //Act
            var result = _patientService.GetPatientInviteStatus(PatientAccount, _userProfile);

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
        public void GetPatientAliasListForSpecificPatient_PatientAliasListForSpecificPatientModel_ReturnData(long patient_Account)
        {
            //Arrange
            //Act
            var result = _patientService.GetPatientAliasListForSpecificPatient(patient_Account);

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
        public void GetCountries_CountriesModel_ReturnData(string SEARCHVALUE)
        {
            //Arrange
            _smartSearchCountriesReq.SEARCHVALUE = SEARCHVALUE;

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
        public void GetInsuranc_InsurancModel_ReturnData(long ID)
        {
            //Arrange
            //Act
            var result = _patientService.GetInsuranc(ID,_userProfile);

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
        public void AddUpdatePatient_AddToPatientTable_ReturnData(long PracticeCode, long Patient_Account )
        {
            //Arrange
           
            _userProfile.PracticeCode = PracticeCode;
            _userProfile.UserName = "1163testing";
            _patient.PCP = 544100;
            _patient.PCP_Name = "Test";
            _patient.Date_Of_Birth_In_String = "8/12/2000";
            _patient.Expiry_Date_In_Str = "8/12/2024";
            _patient.Patient_Account = Patient_Account;
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
            var result = _patientService.GetPatientInsurance(patientAccount,_userProfile);

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
        [TestCase("101116354816562",null)]
        public void CheckNecessaryDataForLoadEligibility_DataEligibilityCheck_ReturnData(string patientAccount, long Patient_Insurance_id)
        {
            //Arrange
            _patientEligibilitySearchModel.Patient_Account_Str = patientAccount;
            _patientEligibilitySearchModel.Patient_Insurance_id = Patient_Insurance_id;
            
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
        public void RemoveStyleNodeFromHtmlForMVP_RemoveStyle_ReturnData(string HTML)
        {
            //Arrange
            //Act
            var result = _patientService.RemoveStyleNodeFromHtmlForMVP(HTML);

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
        public void UpdatePayerNameInHTML_UpdatePayerNameInHTML_ReturnData(string HTML)
        {
            //Arrange
            //Act
            var result = _patientService.UpdatePayerNameInHTML(HTML, _patientInsuranceInformation);

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
        public void GetCoordinates_Coordinates_ReturnData(long LOC_ID)
        {
            //Arrange
            _userProfile.PracticeCode=1011163;
            _userProfile.UserName = "1163testing";
            _facilityLocation.LOC_ID = LOC_ID;

            //Act
            var result = _patientService.AddFacilityLocation(_facilityLocation , _userProfile);

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
        public void SaveContact_Contact_ReturnData(long Contact_ID)
        {
            //Arrange
            _userProfile.PracticeCode = 1011163;
            _userProfile.UserName = "1163testing";
            _patientContact.Start_Date_In_String = "8/12/2000";
            _patientContact.End_Date_In_String = "8/12/2024";
            _patientContact.Patient_Account_Str = "";
            _patientContact.STATEMENT_ADDRESS_MARKED = false;
            _patientContact.Flag_Financially_Responsible_Party = false;
            _patientContact.Contact_ID = Contact_ID;
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
        public void GetPatientContacts_PatientContactsModel_ReturnData(long patient_Account)
        {
            //Arrange
            //Act
            var result = _patientService.GetPatientContacts(patient_Account);

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
        public void GetPatientContactsForInsurance_PatientContactsForInsuranceModel_ReturnData(long patient_Account)
        {
            //Arrange
            //Act
            var result = _patientService.GetPatientContactsForInsurance(patient_Account);

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
        public void GetCurrentPatientInsurances_CurrentPatientInsurancesModel_ReturnData(long patient_Account)
        {
            //Arrange
            //Act
            var result = _patientService.GetCurrentPatientInsurances(patient_Account,_userProfile);

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
        public void CreateUpdateInsuranceInMTBC_UpdateModel_ReturnData(long patient_Account)
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
            var result = _patientService.CreateUpdateInsuranceInMTBC(_patientInsurance, patient_Account, _userProfile);

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
        [TestCase(true, 548101, "101116351010069" , "")]
        [TestCase(true, 548101, "101116351010069", "Q")]
        [TestCase(true , 548101 , "101116354816562" , "Q")]
        public void SaveInsuranceAndEligibilityDetails_SaveModel_ReturnData(bool FromIndexInfo , long Patient_Insurance_Id , string Patient_Account_Str , string Pri_Sec_Oth_Type)
        {
            //Arrange
            _userProfile.PracticeCode = 1011163;
            _userProfile.UserName = "1163testing";
            _patientInsuranceEligibilityDetail = new PatientInsuranceEligibilityDetail()
            {
                InsuranceToCreateUpdate = new PatientInsurance()
                {
                    Patient_Insurance_Id = Patient_Insurance_Id,
                    Pri_Sec_Oth_Type = "",
                    Effective_Date_In_String = Convert.ToString(DateTime.Today),
                    Termination_Date_In_String = Convert.ToString(DateTime.Today),
                    SUPRESS_BILLING_UNTIL_DATE_IN_STRING = "8/12/2020",
                    DED_AMT_VERIFIED_ON_DATE_IN_STRING = "8/12/2020",
                    DED_MET_AS_OF_IN_STRING = "8/12/2020",
                    BENEFIT_AMT_VERIFIED_ON_DATE_IN_STRING = "8/12/2020",
                    Deceased_Date_In_String = "8/12/2020",
                    Policy_Number = "123456789",
                    Plan_Name  = "Test",
                    Relationship   = "Brother",

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
            _patientInsuranceEligibilityDetail.Patient_Account_Str = Patient_Account_Str;

            //Act
            var result = _patientService.SaveInsuranceAndEligibilityDetails(_patientInsuranceEligibilityDetail, _userProfile, FromIndexInfo);

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
        [TestCase(544100 , "ABN")]
        public void CreateNewLimit_NewLimit_ReturnData(long? old_Lim_ID, string lim_Type)
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
            var result = _patientService.CreateNewLimit(old_Lim_ID, _medicareLimit, lim_Type, _userProfile);

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
        public void ABN_MedicareLimitDataChanged_DataChanged_ReturnData(long? newCaseId , string MEDICARE_LIMIT_TYPE_NAME)
        {
            //Arrange

            _medicareLimit.MEDICARE_LIMIT_TYPE_NAME = MEDICARE_LIMIT_TYPE_NAME;
            _medicareLimit.CASE_ID = 544100 ;
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
            var result = _patientService.GetMedicareLimitHistory(_medicareLimitHistorySearchReq , _userProfile);

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
        public void Getsplitauthorizations_splitauthorization_ReturnData(long parent_id)
        {
            //Arrange 
            _userProfile.PracticeCode = 1011163;

            //Act
            var result = _patientService.getsplitauthorization(parent_id, _userProfile);

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
            var result = _patientService.GetInvitedPatient(_phr , _userProfile);

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
        public void UnBlockPatientFromPHR_UnBlockPatient_ReturnData(long USER_ID)
        {
            //Arrange 
            _phr.EMAIL_ADDRESS = "babarazam@mailinator.com";
            _phr.PRACTICE_CODE = 1011163;
            _userProfile.PracticeCode = 1011163;
            _phr.USER_ID = USER_ID;
            _userProfile.userID = USER_ID;
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
        public void BlockPatientFromPHR_BlockPatient_ReturnData(long USER_ID)
        {
            //Arrange 
            _phr.EMAIL_ADDRESS = "babarazam@mailinator.com";
            _phr.PRACTICE_CODE = 1011163;
            _userProfile.PracticeCode = 1011163;
            _phr.USER_ID = USER_ID;
            _userProfile.userID = USER_ID;
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
        public void CancelPatientRequestFromPHR_CancelRequest_ReturnData(long USER_ID)
        {
            //Arrange 
            _phr.EMAIL_ADDRESS = "babarazam@mailinator.com";
            _phr.PRACTICE_CODE = 1011163;
            _userProfile.PracticeCode = 1011163;
            _phr.USER_ID = USER_ID;
            _userProfile.userID = USER_ID;
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
        public void AddUpdateTask_TaskAddToDb_ReturnData(string Eligibility_MSP_Data)
        {
            //Arrange 
            _userProfile.PracticeCode = 1011163;
            _userProfile.UserName = "1163testing";
            _fOX_TBL_TASK.PATIENT_ACCOUNT_STR = "1012714";
            _fOX_TBL_TASK.PATIENT_ACCOUNT = 1012714 ;
            _fOX_TBL_TASK.TASK_ID = 123456;
 
            //Act
            var result = _patientService.AddUpdateTask(_fOX_TBL_TASK , _userProfile , Eligibility_MSP_Data);

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
        public void SavePatientAlias_AddData_ReturnData(long PATIENT_ALIAS_ID )
        {
            //Arrange 
            _userProfile.PracticeCode = 1011163;
            _userProfile.UserName = "1163testing";
            _patientAlias.PATIENT_ACCOUNT_STR = "101116354814556";
            _patientAlias.PATIENT_ALIAS_ID = PATIENT_ALIAS_ID;

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
        [TestCase(null)]
        [TestCase(548100)]
        public void CheckDuplicatePatients_CheckDuplicate_ReturnData(long PATIENT_ALIAS_ID)
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

