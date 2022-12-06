using FOX.BusinessOperations.CommonService;
using FOX.BusinessOperations.CommonServices;
using FOX.BusinessOperations.PatientServices;
using FOX.BusinessOperations.RequestForOrder.UploadOrderImages;
using FOX.DataModels.Context;
using FOX.DataModels.GenericRepository;
using FOX.DataModels.Models.CommonModel;
using FOX.DataModels.Models.FrictionlessReferral.SupportStaff;
using FOX.DataModels.Models.IndexInfo;
using FOX.DataModels.Models.OriginalQueueModel;
using FOX.DataModels.Models.Patient;
using FOX.DataModels.Models.RequestForOrder;
using FOX.DataModels.Models.Security;
using FOX.DataModels.Models.ServiceConfiguration;
using FOX.DataModels.Models.UploadWorkOrderFiles;
using FOX.ExternalServices;
using Newtonsoft.Json;
using SautinSoft;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Web;
using System.Web.Configuration;
using HtmlAgilityPack;
using SelectPdf;
using FOX.BusinessOperations.FaxServices;
using ZXing;
using System.Net;

namespace FOX.BusinessOperations.FrictionlessReferral.SupportStaff
{
    public class SupportStaffService : ISupportStaffService
    {
        private readonly IFaxService _IFaxService = new FaxService();

        #region PROPERTIES
        // DB Context Objects
        private readonly DbContextFrictionless _dbContextFrictionLess = new DbContextFrictionless();
        private readonly GenericRepository<OriginalQueue> _QueueRepository;
        private readonly DbContextPatient _dbContextPatient = new DbContextPatient();
        private readonly DbContextCommon _dbContextCommon = new DbContextCommon();
        private readonly DBContextQueue _QueueContext = new DBContextQueue();
        private readonly GenericRepository<FOX_TBL_NOTES_HISTORY> _NotesRepository;
        private readonly DbContextIndexinfo _IndexinfoContext = new DbContextIndexinfo();
        private readonly GenericRepository<FoxDocumentType> _foxdocumenttypeRepository;
        private readonly GenericRepository<FoxInsurancePayers> _foxInsurancePayersRepository;
        private readonly GenericRepository<TherapyTreatmentRequestForm> _fox_tbl_TherapyTreatmentRequestForm;
        private readonly DbContextPatient _PatientContext = new DbContextPatient();

        // Generic Repository Objects
        private readonly GenericRepository<FoxInsurancePayers> _insurancePayerRepository;
        private readonly GenericRepository<FrictionlessReferralForm> _frictionlessReferralWorkReposistory;
        private readonly GenericRepository<PHR> _phrRepository;
        private readonly GenericRepository<Provider> _providerRepository;
        private readonly GenericRepository<FrictionLessReferral> _frictionlessReferralRepository;
        private readonly GenericRepository<OriginalQueueFiles> _OriginalQueueFiles;
        private static List<Thread> threadsList = new List<Thread>();
        // Class Objects
        PatientService patientServices = new PatientService();
        ConvertPDFToImages _convertPDFToImages;
        #endregion

        #region CONSTRUCTOR
        public SupportStaffService()
        {
            _insurancePayerRepository = new GenericRepository<FoxInsurancePayers>(_dbContextFrictionLess);
            _frictionlessReferralWorkReposistory = new GenericRepository<FrictionlessReferralForm>(_dbContextFrictionLess);
            _phrRepository = new GenericRepository<PHR>(_dbContextPatient);
            _providerRepository = new GenericRepository<Provider>(_dbContextCommon);
            _frictionlessReferralRepository = new GenericRepository<FrictionLessReferral>(_dbContextFrictionLess);
            _OriginalQueueFiles = new GenericRepository<OriginalQueueFiles>(_QueueContext);
            _QueueRepository = new GenericRepository<OriginalQueue>(_QueueContext);
            _NotesRepository = new GenericRepository<FOX_TBL_NOTES_HISTORY>(_IndexinfoContext);
            _convertPDFToImages = new ConvertPDFToImages();
            _foxdocumenttypeRepository = new GenericRepository<FoxDocumentType>(_IndexinfoContext);
            _foxInsurancePayersRepository = new GenericRepository<FoxInsurancePayers>(_PatientContext);
            _fox_tbl_TherapyTreatmentRequestForm = new GenericRepository<TherapyTreatmentRequestForm>(_QueueContext);
        }
        #endregion

        #region FUNCTIONS
        // Description: This function is used to get practice code from WebConfig.
        public long GetPracticeCode()
        {
            long practiceCode = Convert.ToInt64(WebConfigurationManager.AppSettings?["GetPracticeCode"]);
            return practiceCode;
        }
        // Description: This function is used to get insurance payer names.
        public List<InsurancePayer> GetInsurancePayers()
        {
            List<FoxInsurancePayers> insurancePayers = new List<FoxInsurancePayers>();
            List<InsurancePayer> selectedInsurancePayer = new List<InsurancePayer>();
            long practiceCode = GetPracticeCode();
            insurancePayers = _insurancePayerRepository.GetMany(x => x.PRACTICE_CODE == practiceCode);
            if (insurancePayers != null && insurancePayers.Count > 0)
            {
                selectedInsurancePayer = insurancePayers.Select(x => new InsurancePayer
                {
                    FoxTblInsurance_Id = x.FOX_TBL_INSURANCE_ID,
                    InsurancePayersId = x.INSURANCE_PAYERS_ID,
                    InsuranceName = x.INSURANCE_NAME
                }).ToList();
            }
            return selectedInsurancePayer;
        }
        // Description: This function send email invite to patient for Patient Portal.
        public ResponseModel SendInviteToPatientPortal(PatientDetail patientDetails)
        {
            ResponseModel responseModel = new ResponseModel();
            PHR phrInvite = new PHR();
            bool emailStatus = false;
            long practiceCode = GetPracticeCode();
            if (patientDetails != null && (!string.IsNullOrEmpty(patientDetails.EmailAddress) || !string.IsNullOrEmpty(patientDetails.MobilePhone)))
            {
                phrInvite = _phrRepository.GetFirst(e => e.EMAIL_ADDRESS == patientDetails.EmailAddress && e.USER_PHONE == patientDetails.MobilePhone && e.PRACTICE_CODE == practiceCode && !(e.DELETED.HasValue ? e.DELETED.Value : false));
                if (phrInvite == null)
                {
                    int pinCode = Helper.GetRandomPin();
                    string urlLink = GetEncryptedUrlLink(patientDetails.EmailAddress, patientDetails.MobilePhone, "", pinCode.ToString());
                    if (!string.IsNullOrEmpty(urlLink))
                    {
                        emailStatus = SendEmailToPHR(patientDetails.EmailAddress, pinCode.ToString(), urlLink, patientDetails.FirstName, patientDetails.LastName);
                        if (emailStatus == true)
                        {
                            phrInvite = new PHR();
                            CommonServices.EncryptionDecryption encrypt = new CommonServices.EncryptionDecryption();
                            long maxId = Helper.getMaximumId("FOX_PHR_USER_ID");
                            phrInvite.USER_ID = maxId;
                            phrInvite.USER_NAME = "FP_" + patientDetails.FirstName + "_" + maxId;
                            phrInvite.USER_PHONE = patientDetails.MobilePhone;
                            phrInvite.EMAIL_ADDRESS = patientDetails.EmailAddress;
                            phrInvite.TEMP_PASSWORD = encrypt.Encrypt(pinCode.ToString());
                            phrInvite.IS_BLOCK = false;
                            phrInvite.INVITE_STATUS = "Response Awaited";
                            phrInvite.PRACTICE_CODE = GetPracticeCode();
                            phrInvite.CREATED_DATE = Helper.GetCurrentDate();
                            phrInvite.MODIFIED_DATE = Helper.GetCurrentDate();
                            phrInvite.CREATED_BY = phrInvite.MODIFIED_BY = "FOX_PORTAL";
                            phrInvite.DELETED = false;
                            _phrRepository.Insert(phrInvite);
                            _phrRepository.Save();
                            responseModel = new ResponseModel()
                            {
                                Message = "Invitation sent to patient successfully.",
                                Success = true
                            };
                        }
                        else
                        {
                            responseModel = new ResponseModel()
                            {
                                Success = false,
                                Message = "Unable to send request to patient.",
                            };
                        }
                    }
                }
                else
                {
                    string msg = String.Empty;
                    var phoneCheck = _phrRepository.Get(e => e.USER_PHONE == patientDetails.MobilePhone && e.PRACTICE_CODE == GetPracticeCode() && !(e.DELETED.HasValue ? e.DELETED.Value : false) && e.USER_PHONE != null);
                    var emailCheck = _phrRepository.Get(e => e.EMAIL_ADDRESS == patientDetails.EmailAddress && e.PRACTICE_CODE == GetPracticeCode() && !(e.DELETED.HasValue ? e.DELETED.Value : false) && e.EMAIL_ADDRESS != null);
                    if (phoneCheck != null && emailCheck != null)
                    {
                        msg = "A user already been registered with this email address | cell number";
                    }
                    else if (phoneCheck != null)
                    {
                        msg = "A user already been registered with this cell number";
                    }
                    else
                    {
                        msg = "A user already been registered with this email address";
                    }
                    responseModel = new ResponseModel()
                    {
                        ErrorMessage = "",
                        Message = msg,
                        Success = false
                    };
                }
            }
            else
            {
                phrInvite = new PHR();
                responseModel = new ResponseModel()
                {
                    Success = false,
                    Message = "Email Address or Patient Account is missing.",
                };
            }
            return responseModel;
        }
        // Description: This function is used to generate encryption link of Patient Portal PHR.
        private string GetEncryptedUrlLink(string email, string phone, string patientAccount, string pinCode)
        {
            EncryptionDecryption encrypt = new EncryptionDecryption();
            string queryString = encrypt.Encrypt(email + "|" + phone + "|" + patientAccount + "|" + pinCode).Replace('+', '!');
            string link = WebConfigurationManager.AppSettings["PHRPortalURL"].ToString() + queryString;
            return link;
        }
        // Description: This Fucntion is used to send email for PHR Patient.
        private bool SendEmailToPHR(string emailAddress, string pin, string link, string firstName, string lastName)
        {
            string body = patientServices.GetEmailOrFaxToSenderTemplate(firstName: firstName, LastName: lastName, link: link, pin: pin, practiceName: "");
            List<string> BCC = new List<string>();
            BCC.Add("muhammadarslan3@carecloud.com");
            bool sent = Helper.Email(to: emailAddress, subject: "Fox Patient Portal", body: body, profile: null, CC: null, BCC: BCC);
            return sent;
        }
        // Description: This function is used to send invite of PHR on Mobile.
        public ResponseModel SendInviteOnMobile(PatientDetail patientDetails)
        {
            ResponseModel responseModel = new ResponseModel();
            int pinCode = Helper.GetRandomPin();
            if (!string.IsNullOrEmpty(patientDetails.EmailAddress) || !string.IsNullOrEmpty(patientDetails.MobilePhone))
            {
                string urlLink = GetEncryptedUrlLink(patientDetails.EmailAddress, patientDetails.MobilePhone, "", pinCode.ToString());
                if (!string.IsNullOrEmpty(urlLink))
                {
                    var status = SmsService.SMSTwilio(patientDetails.MobilePhone, urlLink);
                    if (status != null)
                    {
                        responseModel = new ResponseModel()
                        {
                            Success = true,
                            Message = "Patient Portal invite is send successfully on mobile phone.",
                        };
                    }
                }
            }
            else
            {
                responseModel = new ResponseModel()
                {
                    Success = false,
                    Message = "Email Address or Patient Account is missing.",
                };
            }
            return responseModel;
        }
        // Description: This function is used to get provider details from db first then npiregistry.
        public List<ProviderReferralSourceResponse> GetProviderReferralSources(ProviderReferralSourceRequest obj)
        {
            List<Provider> providerResponse = new List<Provider>();
            List<ProviderReferralSourceResponse> ProviderReferralSourceInfo = new List<ProviderReferralSourceResponse>();
            long practiceCode = GetPracticeCode();
            if (practiceCode != 0 && !string.IsNullOrEmpty(obj.ProviderNpi))
            {
                providerResponse = _providerRepository.GetMany(x => x.INDIVIDUAL_NPI == obj.ProviderNpi && x.PRACTICE_CODE == practiceCode && !(x.DELETED.HasValue ? x.DELETED.Value : false));
                // Search on NPPES
                if (providerResponse.Count == 0)
                {
                    string url = AppConfiguration.NPPESNPIRegistry + "&number=" + obj.ProviderNpi;
                    ProviderReferralSourceInfo = GetNPPESNPIResponse(url);
                    return ProviderReferralSourceInfo;
                }
            }
            else if (practiceCode != 0 && !string.IsNullOrEmpty(obj.ProviderFirstName.Trim()) && !string.IsNullOrEmpty(obj.ProviderLastName.Trim()) && !string.IsNullOrEmpty(obj.ProviderState.Trim()))
            {
                providerResponse = _providerRepository.GetMany(x => x.FIRST_NAME == obj.ProviderFirstName && x.LAST_NAME == obj.ProviderLastName && x.STATE == obj.ProviderState && x.PRACTICE_CODE == practiceCode && !(x.DELETED.HasValue ? x.DELETED.Value : false));
                // Search on NPPES
                if (providerResponse.Count == 0)
                {
                    string url = AppConfiguration.NPPESNPIRegistry + "&first_name=" + obj.ProviderFirstName.Trim() + "&last_name=" + obj.ProviderLastName.Trim() + "&state=" + obj.ProviderState.Trim();
                    ProviderReferralSourceInfo = GetNPPESNPIResponse(url);
                    return ProviderReferralSourceInfo;
                }
            }
            if (providerResponse != null && ProviderReferralSourceInfo.Count == 0)
            {
                ProviderReferralSourceInfo = providerResponse.Select(x => new ProviderReferralSourceResponse
                {
                    ProviderNpi = x.INDIVIDUAL_NPI,
                    ProviderFirstName = x.FIRST_NAME,
                    ProviderLastName = x.LAST_NAME,
                    ProviderAddress = x.ADDRESS,
                    ProviderCity = x.CITY,
                    ProviderState = x.STATE,
                    ProviderZipCode = x.ZIP,
                    ProviderRegion = x.REGION_NAME,
                    ProviderRegionCode = x.REGION_CODE,
                    ProviderFax = x.FAX,
                    isNPPES = false,
                    Success = true,
                }).ToList();
            }
            return ProviderReferralSourceInfo;
        }
        // Description: This function is trigger to fetch details of provider from NPPES NPI Registry.
        public List<ProviderReferralSourceResponse> GetNPPESNPIResponse(string url)
        {
            NPPESRegistryRequest registryRequest = new NPPESRegistryRequest();
            List<ProviderReferralSourceResponse> referralSourceResponse = new List<ProviderReferralSourceResponse>();
            using (HttpClient http = new HttpClient())
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                var response = http.GetAsync(url).Result;
                if (response != null && response.IsSuccessStatusCode)
                {
                    var stringResponse = response.Content.ReadAsStringAsync().Result;
                    if (stringResponse != null && !stringResponse.Contains("NPI must be 10 digits"))
                    {
                        registryRequest = JsonConvert.DeserializeObject<NPPESRegistryRequest>(stringResponse);
                        if (registryRequest != null && registryRequest?.results?.Count >= 1)
                        {
                            foreach (var item in registryRequest.results)
                            {
                                referralSourceResponse.Add(new ProviderReferralSourceResponse()
                                {
                                    ProviderNpi = item.number,
                                    ProviderFirstName = item.basic.first_name,
                                    ProviderLastName = item.basic.last_name,
                                    ProviderAddress = item.addresses[1].address_1,
                                    ProviderCity = item.addresses[1].city,
                                    ProviderState = item.addresses[1].state,
                                    ProviderZipCode = item.addresses[1].postal_code,
                                    ProviderFax = item.addresses[1].fax_number,
                                    isNPPES = true,
                                    Success = true,
                                });
                            }
                        }
                    }
                    else
                    {
                        referralSourceResponse = new List<ProviderReferralSourceResponse>()
                        {
                            new ProviderReferralSourceResponse
                            {
                                Message = "NPI must be 10 digits",
                                Success = false,
                            }
                        };
                    }
                }
            }
            return referralSourceResponse;
        }
        // Description: This function is used to get details of Provider Referral Source on the basics of NPI, First Name, Last Name and State.
        public List<ProviderReferralSourceResponse> GetOrderingReferralSource(ProviderReferralSourceRequest obj)
        {
            List<ProviderReferralSourceResponse> ProviderReferralSourceInfo;
            if (obj != null)
            {
                ProviderReferralSourceInfo = GetProviderReferralSources(obj);
            }
            else
            {
                ProviderReferralSourceInfo = new List<ProviderReferralSourceResponse>()
                {
                    new ProviderReferralSourceResponse
                    {
                        Success = false,
                        Message = "Please use NPPES API."
                    }
                };
            }
            return ProviderReferralSourceInfo;
        }
        // Description: This function is trigger to get details of frictionless referral.
        public FrictionLessReferral GetFrictionLessReferralDetails(long referralId)
        {
            FrictionLessReferral frictionLessReferral = new FrictionLessReferral();
            long practiceCode = GetPracticeCode();
            if (referralId != 0 && practiceCode != 0)
            {
                frictionLessReferral = _frictionlessReferralRepository.GetFirst(f => f.FRICTIONLESS_REFERRAL_ID == referralId && f.PRACTICE_CODE == practiceCode && f.DELETED == false);
                if (frictionLessReferral.PATIENT_DOB != null)
                {
                    var date = Convert.ToDateTime(frictionLessReferral.PATIENT_DOB);
                    frictionLessReferral.PATIENT_DOB_STRING = date.ToShortDateString();
                }
                return frictionLessReferral ?? new FrictionLessReferral();
            }
            return frictionLessReferral;
        }
        // Description: This function is trigger to get details of frictionless referral.
        public FrictionLessReferral GetFrictionLessReferralDetailsByWorkID(long workId)
        {
            FrictionLessReferral frictionLessReferral = new FrictionLessReferral();
            long practiceCode = GetPracticeCode();
            if (workId != 0 && practiceCode != 0)
            {
                frictionLessReferral = _frictionlessReferralRepository.GetFirst(f => f.WORK_ID == workId && f.PRACTICE_CODE == practiceCode && f.DELETED == false);
                return frictionLessReferral ?? new FrictionLessReferral();
            }
            return frictionLessReferral;
        }
        // Description: This function is used to save the record of referral form in frictionless table.
        public FrictionLessReferralResponse SaveFrictionLessReferralDetails(FrictionLessReferral frictionLessReferralObj)
        {
            ResponseModel responseModel = new ResponseModel();
            FrictionLessReferralResponse frictionLessReferralResponse = new FrictionLessReferralResponse();
            long practiceCode = GetPracticeCode();
            if (frictionLessReferralObj != null)
            {
                if (frictionLessReferralObj.PATIENT_DISCIPLINE_ID != null && frictionLessReferralObj.PATIENT_DISCIPLINE_ID.StartsWith(","))
                {
                    frictionLessReferralObj.PATIENT_DISCIPLINE_ID = frictionLessReferralObj.PATIENT_DISCIPLINE_ID.Remove(0, 1);
                }
                var existingFrictionReferral = _frictionlessReferralRepository.GetFirst(f => f.FRICTIONLESS_REFERRAL_ID == frictionLessReferralObj.FRICTIONLESS_REFERRAL_ID && f.PRACTICE_CODE == practiceCode && f.DELETED == false);
                if ((frictionLessReferralObj.FILE_NAME_LIST != null && frictionLessReferralObj.FILE_NAME_LIST.Count > 0) || frictionLessReferralObj.IS_SUBMIT_CHECK == true)
                {
                    var WorkID = new SqlParameter("WORK_ID", SqlDbType.BigInt) { Value = frictionLessReferralObj.WORK_ID };
                    var updatedWorkFiles = SpRepository<OriginalQueueFiles>.GetSingleObjectWithStoreProcedure(@"exec FOX_PROC_DELETE_FRICTIONLESS_WORK_FILE @WORK_ID", WorkID);
                    UserProfile userProfile = new UserProfile();
                    userProfile.PracticeCode = GetPracticeCode();
                    userProfile.UserName = existingFrictionReferral.SUBMITTER_LAST_NAME;
                    var result = new ResSaveUploadWorkOrderFiles();
                    frictionLessReferralObj.WORK_ID = AddUpdateWorkOrder(frictionLessReferralObj, userProfile);
                }
                if (!string.IsNullOrEmpty(frictionLessReferralObj.PATIENT_DOB_STRING))
                {
                    frictionLessReferralObj.PATIENT_DOB = Convert.ToDateTime(frictionLessReferralObj.PATIENT_DOB_STRING);
                }
                else
                {
                    frictionLessReferralObj.PATIENT_DOB = null;
                }
                if (frictionLessReferralObj.PROVIDER_FAX != null)
                {
                    frictionLessReferralObj.PROVIDER_FAX = frictionLessReferralObj.PROVIDER_FAX.Replace("-", "");
                }
                if (existingFrictionReferral == null)
                {
                    frictionLessReferralObj.FRICTIONLESS_REFERRAL_ID = Helper.getMaximumId("FRICTIONLESS_REFERRAL_ID");
                    frictionLessReferralObj.PRACTICE_CODE = GetPracticeCode();
                    frictionLessReferralObj.CREATED_BY = frictionLessReferralObj.MODIFIED_BY = !string.IsNullOrEmpty(frictionLessReferralObj.SUBMITTER_LAST_NAME) ? frictionLessReferralObj.SUBMITTER_LAST_NAME : "FOX_TEAM";
                    frictionLessReferralObj.CREATED_DATE = frictionLessReferralObj.MODIFIED_DATE = Helper.GetCurrentDate();
                    frictionLessReferralObj.DELETED = false;
                    _frictionlessReferralRepository.Insert(frictionLessReferralObj);
                    frictionLessReferralResponse.Message = "Record Inserted Successfully.";
                    frictionLessReferralResponse.Success = true;
                    frictionLessReferralResponse.FrictionLessReferralObj = frictionLessReferralObj;
                }
                else
                {
                    existingFrictionReferral.USER_TYPE = frictionLessReferralObj.USER_TYPE;
                    existingFrictionReferral.IS_SIGNED_REFERRAL = frictionLessReferralObj.IS_SIGNED_REFERRAL;
                    existingFrictionReferral.SUBMITER_FIRST_NAME = frictionLessReferralObj.SUBMITER_FIRST_NAME;
                    existingFrictionReferral.SUBMITTER_LAST_NAME = frictionLessReferralObj.SUBMITTER_LAST_NAME;
                    existingFrictionReferral.SUBMITTER_PHONE = frictionLessReferralObj.SUBMITTER_PHONE;
                    existingFrictionReferral.SUBMITTER_EMAIL = frictionLessReferralObj.SUBMITTER_EMAIL;
                    existingFrictionReferral.PROVIDER_NPI = frictionLessReferralObj.PROVIDER_NPI;
                    existingFrictionReferral.PROVIDER_FIRST_NAME = frictionLessReferralObj.PROVIDER_FIRST_NAME;
                    existingFrictionReferral.PATIENT_LAST_NAME = frictionLessReferralObj.PATIENT_LAST_NAME;
                    existingFrictionReferral.PROVIDER_ADDRESS = frictionLessReferralObj.PROVIDER_ADDRESS;
                    existingFrictionReferral.PROVIDER_CITY = frictionLessReferralObj.PROVIDER_CITY;
                    existingFrictionReferral.PROVIDER_STATE = frictionLessReferralObj.PROVIDER_STATE;
                    existingFrictionReferral.PROVIDER_ZIP_CODE = frictionLessReferralObj.PROVIDER_ZIP_CODE;
                    existingFrictionReferral.PROVIDER_REGION = frictionLessReferralObj.PROVIDER_REGION;
                    existingFrictionReferral.PROVIDER_REGION_CODE = frictionLessReferralObj.PROVIDER_REGION_CODE;
                    existingFrictionReferral.PROVIDER_FAX = frictionLessReferralObj.PROVIDER_FAX;
                    existingFrictionReferral.PATIENT_FIRST_NAME = frictionLessReferralObj.PATIENT_FIRST_NAME;
                    existingFrictionReferral.PATIENT_LAST_NAME = frictionLessReferralObj.PATIENT_LAST_NAME;
                    existingFrictionReferral.PATIENT_DOB = frictionLessReferralObj.PATIENT_DOB;
                    existingFrictionReferral.PATIENT_MOBILE_NO = frictionLessReferralObj.PATIENT_MOBILE_NO;
                    existingFrictionReferral.PATIENT_EMAIL = frictionLessReferralObj.PATIENT_EMAIL;
                    existingFrictionReferral.PATIENT_SUBSCRIBER_ID = frictionLessReferralObj.PATIENT_SUBSCRIBER_ID;
                    existingFrictionReferral.PATIENT_INSURANCE_PAYER_ID = frictionLessReferralObj.PATIENT_INSURANCE_PAYER_ID;
                    existingFrictionReferral.IS_CHECK_ELIGIBILITY = frictionLessReferralObj.IS_CHECK_ELIGIBILITY;
                    existingFrictionReferral.PATIENT_DISCIPLINE_ID = frictionLessReferralObj.PATIENT_DISCIPLINE_ID;
                    existingFrictionReferral.PATIENT_REFERRAL_NOTES = frictionLessReferralObj.PATIENT_REFERRAL_NOTES;
                    existingFrictionReferral.WORK_ID = frictionLessReferralObj.WORK_ID;
                    existingFrictionReferral.MODIFIED_DATE = Helper.GetCurrentDate();
                    _frictionlessReferralRepository.Update(existingFrictionReferral);
                    frictionLessReferralResponse.Message = "Record Updated Successfully.";
                    frictionLessReferralResponse.Success = true;
                    frictionLessReferralResponse.FrictionLessReferralObj = frictionLessReferralObj;
                }
                _frictionlessReferralRepository.Save();
            }
            if (frictionLessReferralResponse.FrictionLessReferralObj.PATIENT_DOB != null)
            {
                var date = Convert.ToDateTime(frictionLessReferralResponse.FrictionLessReferralObj.PATIENT_DOB);
                frictionLessReferralResponse.FrictionLessReferralObj.PATIENT_DOB_STRING = date.ToShortDateString();
            }
            

            return frictionLessReferralResponse;
        }
        public ResponseModel DeleteWorkOrder(RequestDeleteWorkOrder requestDeleteWorkOrder)
        {
            try
            {
                OriginalQueue originalQueue = _QueueRepository.Get(t => t.WORK_ID == requestDeleteWorkOrder?.WorkId && !t.DELETED);
                if (originalQueue != null)
                {
                    originalQueue.DELETED = true;
                    originalQueue.MODIFIED_DATE = DateTime.Now;
                    _QueueRepository.Update(originalQueue);
                    _QueueRepository.Save();
                    return new ResponseModel() { Message = "Delete work order successfully.", ErrorMessage = "", Success = true };
                }
                else
                    return new ResponseModel() { Message = "Work order not found.", ErrorMessage = "", Success = false };
            }
            catch (Exception exception)
            {
                return new ResponseModel() { Message = "We encountered an error while processing your request.", ErrorMessage = exception.ToString(), Success = false };
            }
        }
        private void SavePdfToImages(string PdfPath, ServiceConfiguration config, long workId, int noOfPages, int pageCounter, out long pageCounterOut, FrictionLessReferral frictionLessReferralObj)
        {
            List<int> threadCounter = new List<int>();
            if (!Directory.Exists(config.IMAGES_PATH_SERVER))
            {
                Directory.CreateDirectory(config.IMAGES_PATH_SERVER);
            }
            if (System.IO.File.Exists(PdfPath))
            {
                for (int i = 0; i < noOfPages; i++, pageCounter++)
                {
                    Thread myThread = new Thread(() => this.newThreadImplementaion(ref threadCounter, PdfPath, i, config, workId, pageCounter));
                    myThread.Start();
                    threadsList.Add(myThread);
                    var imgPath = config.IMAGES_PATH_DB + "\\" + workId + "_" + pageCounter + ".jpg";
                    var logoImgPath = config.IMAGES_PATH_DB + "\\Logo_" + workId + "_" + pageCounter + ".jpg";
                    if (!frictionLessReferralObj.IS_SIGNED_REFERRAL)
                    {
                        AddFrictionlessFilesToDatabase(imgPath, workId, logoImgPath);
                    }
                    else
                    {
                        AddFilesToDatabase(imgPath, workId, logoImgPath);
                    }
                }
                while (noOfPages > threadCounter.Count)
                {
                    //loop untill record complete
                }

                foreach (var thread in threadsList)
                {
                    thread.Abort();
                }
                //AddToDatabase(PdfPath, noOfPages, workId);
            }
            pageCounterOut = pageCounter;
        }
        private int getNumberOfPagesOfPDF(string PdfPath)
        {
            iTextSharp.text.pdf.PdfReader pdfReader = new iTextSharp.text.pdf.PdfReader(PdfPath);
            return pdfReader.NumberOfPages;
        }
        public void newThreadImplementaion(ref List<int> threadCounter, string PdfPath, int i, ServiceConfiguration config, long workId, int pageCounter)
        {
            try
            {
                System.Drawing.Image img;
                PdfFocus f = new PdfFocus();
                f.Serial = "10261435399";
                f.OpenPdf(PdfPath);
                if (f.PageCount > 0)
                {
                    //Save all PDF pages to jpeg images
                    f.ImageOptions.Dpi = 120;
                    f.ImageOptions.ImageFormat = ImageFormat.Jpeg;
                    var image = f.ToImage(i + 1);
                    //Next manipulate with Jpeg in memory or save to HDD, open in a viewer

                    using (var ms = new MemoryStream(image))
                    {
                        img = System.Drawing.Image.FromStream(ms);
                        img.Save(config.IMAGES_PATH_SERVER + "\\" + workId + "_" + pageCounter + ".jpg", ImageFormat.Jpeg);
                        Bitmap bmp = new Bitmap(img);
                        img.Dispose();
                        ConvertPDFToImages ctp = new ConvertPDFToImages();
                        ctp.SaveWithNewDimention(bmp, 115, 150, 100, config.IMAGES_PATH_SERVER + "\\Logo_" + workId + "_" + pageCounter + ".jpg");
                        bmp.Dispose();
                    }
                    threadCounter.Add(1);
                }
            }
            catch (Exception)
            {
                threadCounter.Add(1);
            }

        }

        private void AddFilesToDatabase(string filePath, long workId, string logoPath)
        {
            try
            {
                long iD = Helper.getMaximumId("FOXREHAB_FILE_ID");
                var fileId = new SqlParameter("FILE_ID", SqlDbType.BigInt) { Value = iD };
                var parmWorkID = new SqlParameter("WORKID", SqlDbType.BigInt) { Value = workId };
                var parmFilePath = new SqlParameter("FILEPATH", SqlDbType.VarChar) { Value = filePath };
                var parmLogoPath = new SqlParameter("LOGOPATH", SqlDbType.VarChar) { Value = logoPath };
                var _isFromIndexInfo = new SqlParameter("IS_FROM_INDEX_INFO", SqlDbType.Bit) { Value = false };
                var result = SpRepository<OriginalQueueFiles>.GetSingleObjectWithStoreProcedure(@"exec FOX_PROC_AD_FILES_TO_DB_FROM_RFO @FILE_ID, @WORKID, @FILEPATH, @LOGOPATH, @IS_FROM_INDEX_INFO",
                    fileId, parmWorkID, parmFilePath, parmLogoPath, _isFromIndexInfo);
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }
        public void InsertNotesHistory(FOX_TBL_NOTES_HISTORY obj, UserProfile profile)
        {
            var notesDetail = _NotesRepository.GetByID(obj.NOTE_ID);
            if (notesDetail != null)
            {
                notesDetail.WORK_ID = obj.WORK_ID;
                notesDetail.NOTE_DESC = obj.NOTE_DESC;
                notesDetail.DELETED = obj.DELETED;
                notesDetail.MODIFIED_DATE = Helper.GetCurrentDate();
                notesDetail.MODIFIED_BY = profile.UserName;
                _NotesRepository.Update(notesDetail);
                _NotesRepository.Save();
            }
            else
            {
                obj.NOTE_ID = Helper.getMaximumId("NOTE_ID");
                obj.CREATED_BY = profile.UserName;
                obj.CREATED_DATE = Helper.GetCurrentDate().ToString();
                obj.DELETED = obj.DELETED;
                obj.MODIFIED_DATE = Helper.GetCurrentDate();
                obj.MODIFIED_BY = profile.UserName;
                obj.PRACTICE_CODE = profile.PracticeCode;
                _NotesRepository.Insert(obj);
                _NotesRepository.Save();
            }

            //Log Changes
            string logMsg = string.Format("ID: {0} A new Note(s) has been added.", obj.WORK_ID);
            string user = !string.IsNullOrEmpty(profile.FirstName) ? profile.FirstName + " " + profile.LastName : profile.UserName;
            Helper.LogSingleWorkOrderChange(obj.WORK_ID, obj.WORK_ID.ToString(), logMsg, user);
        }
        private ResponseHTMLToPDF HTMLToPDF(ServiceConfiguration config, string htmlString, string fileName, string type, string linkMessage = null)
        {
            try
            {
                HtmlDocument htmlDoc = new HtmlDocument();
                htmlDoc.LoadHtml(htmlString);
                htmlDoc.DocumentNode.SelectSingleNode("//*[contains(@id,'print-footer')]")?.Remove();
                if (!string.IsNullOrWhiteSpace(linkMessage))
                {
                    var htmlNode_link = htmlDoc.DocumentNode.SelectSingleNode("//*[contains(@id,'link')]");
                    if (htmlNode_link != null)
                    {
                        htmlNode_link.InnerHtml = linkMessage;
                    }
                }
                HtmlToPdf converter = new HtmlToPdf();
                converter.Options.PdfPageSize = PdfPageSize.A4;
                converter.Options.MarginBottom = 10;
                converter.Options.MarginTop = 10;
                converter.Options.MarginLeft = 10;
                converter.Options.MarginRight = 10;
                converter.Options.DisplayHeader = false;
                converter.Options.WebPageWidth = 768;
                PdfDocument doc = converter.ConvertHtmlString(htmlDoc.DocumentNode.OuterHtml);
                string pdfPath = config.ORIGINAL_FILES_PATH_SERVER;
                if (!Directory.Exists(pdfPath))
                {
                    Directory.CreateDirectory(pdfPath);
                }
                fileName += DateTime.Now.Ticks + ".pdf";
                string pdfFilePath = pdfPath + fileName;
                // save pdf document
                doc.Save(pdfFilePath);
                // close pdf document
                doc.Close();
                return new ResponseHTMLToPDF() { FileName = fileName, FilePath = pdfPath, Success = true, ErrorMessage = "" };
            }
            catch (Exception exception)
            {
                return new ResponseHTMLToPDF() { FileName = "", FilePath = "", Success = false, ErrorMessage = exception.ToString() };
            }
        }
        public ResponseModel DownloadPdf(RequestDownloadPdfFrictionlessModel requestDownloadPdfModel)
        {
            try
            {
                var config = Helper.GetServiceConfiguration(GetPracticeCode());
                if (config.PRACTICE_CODE != null
                    && !string.IsNullOrWhiteSpace(config.ORIGINAL_FILES_PATH_DB) && !string.IsNullOrWhiteSpace(config.ORIGINAL_FILES_PATH_SERVER)
                    && !string.IsNullOrWhiteSpace(config.IMAGES_PATH_DB) && !string.IsNullOrWhiteSpace(config.IMAGES_PATH_SERVER))
                {
                    ResponseHTMLToPDF responseHTMLToPDF = HTMLToPDF(config, requestDownloadPdfModel.AttachmentHTML, requestDownloadPdfModel.FileName.Replace(' ', '_'), "fax");
                    //return new ResponseModel() { Message = @"FoxDocumentDirectory\RequestForOrderPDF\" + responseHTMLToPDF.FileName, ErrorMessage = "", Success = true };
                    return new ResponseModel() { Message = config.ORIGINAL_FILES_PATH_DB + responseHTMLToPDF.FileName, ErrorMessage = "", Success = true };

                }
                else
                {
                    return new ResponseModel() { Message = "We encountered an error while processing your request.", ErrorMessage = "DB configuration for file paths not found. See service configuration.", Success = false };
                }
            }
            catch (Exception exception)
            {
                return new ResponseModel() { Message = "We encountered an error while processing your request.", ErrorMessage = exception.ToString(), Success = false };
            }
        }

        public ResponseModel SendFAX(FrictionLessRequestSendFAXModel requestSendFAXModel)
        {
            try
            {
                string htmlstring = "";
                var config = Helper.GetServiceConfiguration(GetPracticeCode());
                UserProfile Profile = new UserProfile();
                Profile.PracticeCode = GetPracticeCode();
                Profile.UserName = "FOX TEAM";
                if (config.PRACTICE_CODE != null
                    && !string.IsNullOrWhiteSpace(config.ORIGINAL_FILES_PATH_DB) && !string.IsNullOrWhiteSpace(config.ORIGINAL_FILES_PATH_SERVER)
                    && !string.IsNullOrWhiteSpace(config.IMAGES_PATH_DB) && !string.IsNullOrWhiteSpace(config.IMAGES_PATH_SERVER))
                {
                    ResponseHTMLToPDF responseHTMLToPDF = HTMLToPDF(config, requestSendFAXModel.AttachmentHTML, requestSendFAXModel.FileName, "fax");

                    if (responseHTMLToPDF != null && (responseHTMLToPDF?.Success ?? false))
                    {
                        string filePath = responseHTMLToPDF?.FilePath + responseHTMLToPDF?.FileName;
                        int numberOfPages = getNumberOfPagesOfPDF(filePath);

                        SavePdfToImages(filePath, config, requestSendFAXModel.WorkId, numberOfPages, "Fax", requestSendFAXModel.ReceipientFaxNumber, Profile.UserName, requestSendFAXModel._isFromIndexInfo);

                        //SavePdfToImages(deliveryfilePath, config, requestSendFAXModel.WorkId, 1, "DR:Fax", requestSendFAXModel.ReceipientFaxNumber, Profile.UserName, requestSendFAXModel._isFromIndexInfo);

                        var commonService = new CommonServices.CommonServices();
                        AttachmentData attachmentPath = commonService.GeneratePdfForSupportedDoc(config, requestSendFAXModel.WorkId.ToString(), Profile);

                        if (!attachmentPath.FILE_PATH.EndsWith("\\"))
                        {
                            attachmentPath.FILE_PATH = attachmentPath.FILE_PATH + "\\";
                        }

                        var resultfax = _IFaxService.SendFax(new string[] { requestSendFAXModel.ReceipientFaxNumber }, new string[] { "" }, null, attachmentPath.FILE_NAME, attachmentPath.FILE_PATH, requestSendFAXModel.Subject, false, Profile);


                        if (resultfax == "failed")
                        {
                            htmlstring = "<html><body><h2>Delivery Report</h2><p>Subject:" + requestSendFAXModel.Subject + "</p><p>From:" + requestSendFAXModel.SenderName + "</p><p>To:" + requestSendFAXModel.ReceipientFaxNumber + "</p><p>Sent:" + DateTime.Now + "</p><br/><div style='padding:10px;background-color:#ff9999;width: 50%;'><p>Delivery report for:" + requestSendFAXModel.ReceipientFaxNumber + "</p><p>Failed:</p><p>Message failed to deliver </p></div></body></html>";
                        }
                        else
                        {
                            htmlstring = "<html><body><h2>Delivery Report</h2><p>Subject:" + requestSendFAXModel.Subject + "</p><p>From:" + requestSendFAXModel.SenderName + "</p><p>To:" + requestSendFAXModel.ReceipientFaxNumber + "</p><p>Sent:" + DateTime.Now + "</p><br/><div style='padding:10px;width: 50%;'><p>Delivery report for:" + requestSendFAXModel.ReceipientFaxNumber + "</p><p>Delivered Successfully:</p><p>Message delivered to recipient. </p></div></body></html>";
                        }
                        //hl
                        ResponseHTMLToPDF responseHTMLToPDF2 = HTMLToPDF2(config, htmlstring, "tempdfdelivery");

                        string deliveryfilePath = responseHTMLToPDF2?.FilePath + responseHTMLToPDF2?.FileName;

                        SavePdfToImages(deliveryfilePath, config, requestSendFAXModel.WorkId, 1, "DR:Fax", requestSendFAXModel.ReceipientFaxNumber, Profile.UserName, requestSendFAXModel._isFromIndexInfo);

                        return new ResponseModel() { Message = "Fax sent successfully, our admission team is processing your referral.", ErrorMessage = "", Success = true };
                    }
                    else
                    {
                        return new ResponseModel() { Message = "Fax sent successfully, our admission team is processing your referral.", ErrorMessage = responseHTMLToPDF?.ErrorMessage, Success = false };
                    }
                }
                else
                {
                    return new ResponseModel() { Message = "Fax could not be sent.", ErrorMessage = "DB configuration for file paths not found. See service configuration.", Success = false };
                }
            }
            catch (Exception exception)
            {
                return new ResponseModel() { Message = "Fax sent successfully, our admission team is processing your referral.", ErrorMessage = exception.ToString(), Success = false };
            }
        }
        private void SavePdfToImages(string PdfPath, ServiceConfiguration config, long workId, int noOfPages, string sorcetype, string sorceName, string userName, bool _isFromIndexInfo)
        {
            List<int> threadCounter = new List<int>();
            var originalQueueFilesCount = _OriginalQueueFiles.GetMany(t => t.WORK_ID == workId && !t.deleted)?.Count() ?? 0;
            long pageCounter = originalQueueFilesCount;
            if (!Directory.Exists(config.IMAGES_PATH_SERVER))
            {
                Directory.CreateDirectory(config.IMAGES_PATH_SERVER);
            }
            if (System.IO.File.Exists(PdfPath))
            {
                for (int i = 0; i < noOfPages; i++, pageCounter++)
                {
                    var imgPath = "";
                    var logoImgPath = "";
                    var imgPathServer = "";
                    var logoImgPathServer = "";
                    string deliveryReportId = "";
                    Random random = new Random();

                    if (sorcetype.Split(':')?[0] == "DR")
                    {
                        deliveryReportId = Convert.ToString(workId) + DateTime.Now.Ticks;
                        var randomString = random.Next();
                        imgPath = config.IMAGES_PATH_DB + "\\" + deliveryReportId + "_" + i + "_" + randomString + ".jpg";
                        imgPathServer = config.IMAGES_PATH_SERVER + "\\" + deliveryReportId + "_" + i + "_" + randomString + ".jpg";
                        var randomStringlogo = random.Next();
                        logoImgPath = config.IMAGES_PATH_DB + "\\Logo_" + deliveryReportId + "_" + i + ".jpg";
                        logoImgPathServer = config.IMAGES_PATH_SERVER + "\\Logo_" + deliveryReportId + "_" + i + ".jpg";
                    }
                    else
                    {
                        if (pageCounter != 0 && _isFromIndexInfo == false && sorcetype.ToLower() == "fax")
                        {
                            imgPath = config.IMAGES_PATH_DB + "\\" + workId + "_" + pageCounter + ".jpg";
                            imgPathServer = config.IMAGES_PATH_SERVER + "\\" + workId + "_" + pageCounter + ".jpg";
                        }
                        else
                        {
                            var randomString = random.Next();
                            imgPath = config.IMAGES_PATH_DB + "\\" + workId + "_" + i + "_" + randomString + ".jpg";
                            imgPathServer = config.IMAGES_PATH_SERVER + "\\" + workId + "_" + i + "_" + randomString + ".jpg";
                        }
                        if (pageCounter != 0 && _isFromIndexInfo == false && sorcetype.ToLower() == "fax")
                        {
                            logoImgPath = config.IMAGES_PATH_DB + "\\Logo_" + workId + "_" + pageCounter + ".jpg";
                            logoImgPathServer = config.IMAGES_PATH_SERVER + "\\Logo_" + workId + "_" + pageCounter + ".jpg";
                        }
                        else
                        {
                            var randomString = random.Next();
                            logoImgPath = config.IMAGES_PATH_DB + "\\Logo_" + workId + "_" + i + "_" + randomString + ".jpg";
                            logoImgPathServer = config.IMAGES_PATH_SERVER + "\\Logo_" + workId + "_" + i + "_" + randomString + ".jpg";
                        }
                    }
                    Thread myThread = new Thread(() => this.newThreadImplementaion(ref threadCounter, PdfPath, i, imgPathServer, logoImgPathServer));
                    myThread.Start();
                    threadsList.Add(myThread);
                    AddFilesToDatabase(imgPath, workId, logoImgPath);
                }
                while (noOfPages > threadCounter.Count)
                {
                    //loop untill record complete
                }
                foreach (var thread in threadsList)
                {
                    thread.Abort();
                }
                noOfPages = _OriginalQueueFiles.GetMany(t => t.WORK_ID == workId && !t.deleted)?.Count() ?? 0;
                AddToDatabase(PdfPath, noOfPages, workId, sorcetype, sorceName, userName, config.PRACTICE_CODE, _isFromIndexInfo);
            }
        }
        private void AddToDatabase(string filePath, int noOfPages, long workId, string sorcetype, string sorceName, string userName, long? practice_code, bool fromindexinf)
        {
            try
            {
                var PracticeCode = new SqlParameter { ParameterName = "@PRACTICE_CODE", SqlDbType = SqlDbType.BigInt, Value = practice_code };
                var workid = new SqlParameter { ParameterName = "@WORK_ID", SqlDbType = SqlDbType.BigInt, Value = workId };
                var username = new SqlParameter { ParameterName = "@USER_NAME", SqlDbType = SqlDbType.VarChar, Value = userName };
                var noofpages = new SqlParameter { ParameterName = "@NO_OF_PAGES", SqlDbType = SqlDbType.Int, Value = noOfPages };
                var fromindexinfo = new SqlParameter { ParameterName = "@FROM_INDEXINFO", SqlDbType = SqlDbType.Int, Value = fromindexinf };
                var result = SpRepository<OriginalQueue>.GetListWithStoreProcedure(@"exec FOX_PROC_ADD_TO_DB_FROM_RFO @PRACTICE_CODE,@WORK_ID,@USER_NAME,@NO_OF_PAGES,@FROM_INDEXINFO",
                    PracticeCode, workid, username, noofpages, fromindexinfo);
            }
            catch (Exception)
            {
                //throw exception;
            }
        }
        public void newThreadImplementaion(ref List<int> threadCounter, string PdfPath, int i, string imgPath, string logoImgPath)
        {
            try
            {
                System.Drawing.Image img;
                PdfFocus f = new PdfFocus();
                f.Serial = "10261435399";
                f.OpenPdf(PdfPath);

                if (f.PageCount > 0)
                {
                    //Save all PDF pages to jpeg images
                    f.ImageOptions.Dpi = 120;
                    f.ImageOptions.ImageFormat = ImageFormat.Jpeg;

                    var image = f.ToImage(i + 1);
                    //Next manipulate with Jpeg in memory or save to HDD, open in a viewer
                    using (var ms = new MemoryStream(image))
                    {
                        img = System.Drawing.Image.FromStream(ms);
                        img.Save(imgPath, ImageFormat.Jpeg);
                        Bitmap bmp = new Bitmap(img);
                        img.Dispose();
                        ConvertPDFToImages ctp = new ConvertPDFToImages();
                        ctp.SaveWithNewDimention(bmp, 115, 150, 100, logoImgPath);
                        bmp.Dispose();
                    }
                }
                threadCounter.Add(1);
            }
            catch (Exception)
            {
                threadCounter.Add(1);
            }
        }
        public static ResponseHTMLToPDF HTMLToPDF2(ServiceConfiguration config, string htmlString, string fileName, string linkMessage = null)
        {
            try
            {
                HtmlDocument htmlDoc = new HtmlDocument();
                htmlDoc.LoadHtml(htmlString);
                HtmlToPdf converter = new HtmlToPdf();
                converter.Options.PdfPageSize = PdfPageSize.A4;
                converter.Options.MarginBottom = 10;
                converter.Options.MarginTop = 10;
                converter.Options.MarginLeft = 10;
                converter.Options.MarginRight = 10;
                converter.Options.DisplayHeader = false;
                converter.Options.WebPageWidth = 768;
                PdfDocument doc = converter.ConvertHtmlString(htmlDoc.DocumentNode.OuterHtml);
                //string pdfPath = HttpContext.Current.Server.MapPath("~/" + @"FoxDocumentDirectory\RequestForOrderPDF\");
                string pdfpath = config.ORIGINAL_FILES_PATH_SERVER;
                if (!Directory.Exists(pdfpath))
                {
                    Directory.CreateDirectory(pdfpath);
                }
                fileName = fileName + DateTime.Now.Ticks + ".pdf";
                string pdfFilePath = pdfpath + "\\" + fileName;
                // save pdf document
                doc.Save(pdfFilePath);

                // close pdf document
                doc.Close();
                return new ResponseHTMLToPDF() { FileName = fileName, FilePath = pdfpath, Success = true, ErrorMessage = "" };
            }
            catch (Exception exception)
            {
                return new ResponseHTMLToPDF() { FileName = "", FilePath = "", Success = true, ErrorMessage = exception.ToString() };
            }
        }

        public QRCodeModel GenerateQRCode(QRCodeModel obj)
        {
            Bitmap result = null;
            string base64Image = "";
            try
            {
                var writer = new BarcodeWriter();
                writer.Format = BarcodeFormat.QR_CODE;
                writer.Options.Height = 70;
                writer.Options.Width = 70;
                writer.Options.Margin = 0;

                result = writer.Write(obj.WORK_ID.ToString());
            }
            catch (Exception ex)
            {
                throw new Exception("QRcode is not generated by library", ex);
            }
            if (!Directory.Exists(obj.AbsolutePath))
            {
                Directory.CreateDirectory(obj.AbsolutePath);
            }

            using (var bitmap = new Bitmap(result))
            {
                result.Dispose();
                string curtime = obj.AbsolutePath + obj.WORK_ID + "_" + DateTime.Now.Ticks.ToString() + ".jpg";
                Bitmap cimage = (Bitmap)bitmap;
                cimage.Save(curtime, System.Drawing.Imaging.ImageFormat.Jpeg);
                bitmap.Dispose();
                cimage.Dispose();
                base64Image = Convert.ToBase64String(File.ReadAllBytes(curtime)); //Get Base64
            }
            string src = "data:image/png;base64," + base64Image;
            string encodedString = HttpUtility.UrlEncode(src);
            obj.ENCODED_IMAGE_BYTES = encodedString;
            if (!File.Exists(System.Web.HttpContext.Current.Server.MapPath("~/" + obj.SignPath)))
            {
                obj.SignPath = "";
            }
            return obj;
        }

        public ResponseModel SendEmail(RequestSendEmailModel requestSendEmailModel)
        {
            UserProfile userProfile = new UserProfile();
            userProfile.PracticeCode = GetPracticeCode();
            userProfile.UserName = "FRICTIONLESS_REFERRAL_SOURCE";
            var frictionlessReferralTempFiles = _frictionlessReferralWorkReposistory.GetMany(t => t.WORK_ID == requestSendEmailModel.WorkId && t.DELETED == false);
            if (frictionlessReferralTempFiles != null && frictionlessReferralTempFiles.Count != 0)
            {
                foreach (var item in frictionlessReferralTempFiles)
                {
                    AddFilesToDatabase(item.FILE_PATH1, item.WORK_ID ?? 0, item.FILE_PATH);
                }
            }
            return SendEmail(requestSendEmailModel, userProfile);
        }

        public ResponseUploadFilesModel UploadFiles(RequestUploadFilesModel requestUploadFilesModel)
        {

            ResponseUploadFilesModel responseUploadFilesModel = new ResponseUploadFilesModel();
            string message = "Please upload file of type " + String.Join(", ", requestUploadFilesModel?.AllowedFileExtensions) + ".";
            try
            {
                foreach (string file in requestUploadFilesModel?.Files)
                {
                    var postedFile = requestUploadFilesModel?.Files[file];
                    if (postedFile != null && postedFile.ContentLength > 0)
                    {
                        //int MaxContentLength = 1024 * 1024 * 5; //Size = 5 MB  
                        //IList<string> AllowedFileExtensions = new List<string> { ".pdf", ".png", ".jpg", ".JPG", ".jpeg", ".tiff", ".tif", ".docx" };

                        string fileName = Path.GetFileNameWithoutExtension(postedFile.FileName);
                        if (fileName?.Length > 30)
                            fileName = fileName.Substring(0, 30);

                        string fileExtension = Path.GetExtension(postedFile.FileName);

                        if (!(requestUploadFilesModel?.AllowedFileExtensions.Contains(fileExtension?.ToLower()) ?? false))
                        {
                            responseUploadFilesModel.FilePath = "";
                            responseUploadFilesModel.Message = message;
                            responseUploadFilesModel.Success = true;
                            responseUploadFilesModel.ErrorMessage = "";
                            return responseUploadFilesModel;
                        }
                        else
                        {
                            string uploadFilesPath = requestUploadFilesModel?.UploadFilesPath;
                            if (!Directory.Exists(uploadFilesPath))
                            {
                                Directory.CreateDirectory(uploadFilesPath);
                            }
                            fileName += "_" + DateTime.Now.Ticks + fileExtension;
                            string filePath = uploadFilesPath + @"\" + fileName;

                            responseUploadFilesModel.FilePath = fileName;
                            responseUploadFilesModel.FileName = filePath;
                            responseUploadFilesModel.Message = "File Uploaded Successfully.";
                            responseUploadFilesModel.Success = true;
                            responseUploadFilesModel.ErrorMessage = "";
                            postedFile.SaveAs(filePath);
                        }
                    }
                    return responseUploadFilesModel;
                }
                responseUploadFilesModel.Message = message;
                responseUploadFilesModel.Success = true;
                responseUploadFilesModel.ErrorMessage = "";
                responseUploadFilesModel.FilePath = "";
                return responseUploadFilesModel;
            }
            catch (Exception exception)
            {
                responseUploadFilesModel.Message = "We encountered an error while processing your request.";
                responseUploadFilesModel.Success = false;
                responseUploadFilesModel.ErrorMessage = exception.ToString();
                responseUploadFilesModel.FilePath = "";
                return responseUploadFilesModel;
            }

        }
        public void GenerateAndSaveImagesOfUploadedFiles(long workId, FrictionLessReferral frictionLessReferralObj, UserProfile profile, int originalQueueFilesCount = 0)
        {
            var config = Helper.GetServiceConfiguration(profile.PracticeCode);
            if (config.PRACTICE_CODE != null
                && !string.IsNullOrWhiteSpace(config.ORIGINAL_FILES_PATH_DB) && !string.IsNullOrWhiteSpace(config.ORIGINAL_FILES_PATH_SERVER)
                && !string.IsNullOrWhiteSpace(config.IMAGES_PATH_DB) && !string.IsNullOrWhiteSpace(config.IMAGES_PATH_SERVER))
            {
                int totalPages = 0;
                long pageCounter = originalQueueFilesCount;
                foreach (var filePath1 in frictionLessReferralObj.FILE_NAME_LIST)
                {
                    string filePath = HttpContext.Current.Server.MapPath("~/" + AppConfiguration.RequestForOrderUploadImages + @"\" + filePath1);
                    var ext = Path.GetExtension(filePath).ToLower();
                    if (!Directory.Exists(config.ORIGINAL_FILES_PATH_SERVER))
                    {
                        Directory.CreateDirectory(config.ORIGINAL_FILES_PATH_SERVER);
                    }
                    if (ext == ".tiff" || ext == ".tif" || ext == ".png" || ext == ".jpg" || ext == ".jpeg" || ext == ".gif" || ext == ".bmp")
                    {
                        int numberOfPages = tifToImage(filePath, config.IMAGES_PATH_SERVER, workId, pageCounter, config.IMAGES_PATH_DB, out pageCounter, true, frictionLessReferralObj);
                        totalPages += numberOfPages;
                    }
                    else
                    {
                        int numberOfPages = getNumberOfPagesOfPDF(filePath);
                        SavePdfToImages(filePath, config, workId, numberOfPages, Convert.ToInt32(pageCounter), out pageCounter, frictionLessReferralObj);
                        totalPages += numberOfPages;
                    }
                }

                AddToDatabase("", totalPages + originalQueueFilesCount, profile.UserName, workId, config.PRACTICE_CODE);
            }
            else
            {
                throw new Exception("DB configuration for file paths not found. See service configuration.");
            }
        }
        private void AddToDatabase(string filePath, int noOfPages, string userName, long workId, long? practiceCode)
        {
            try
            {
                var PracticeCode = new SqlParameter { ParameterName = "@PRACTICE_CODE", SqlDbType = SqlDbType.BigInt, Value = practiceCode };
                var workid = new SqlParameter { ParameterName = "@WORK_ID", SqlDbType = SqlDbType.BigInt, Value = workId };
                var username = new SqlParameter { ParameterName = "@USER_NAME", SqlDbType = SqlDbType.VarChar, Value = userName };
                var filePaths = new SqlParameter { ParameterName = "@FILE_PATH", SqlDbType = SqlDbType.VarChar, Value = filePath };
                var noofpages = new SqlParameter { ParameterName = "@NO_OF_PAGES", SqlDbType = SqlDbType.Int, Value = noOfPages };

                var result = SpRepository<OriginalQueue>.GetListWithStoreProcedure(@"exec FOX_PROC_ADD_TO_DB_FROM_UPLOAD_ORDER_IMAGES @PRACTICE_CODE, @WORK_ID, @USER_NAME, @FILE_PATH, @NO_OF_PAGES",
                    PracticeCode, workid, username, filePaths, noofpages);
            }
            catch (Exception)
            {
                //throw exception;
            }
        }
        public int tifToImage(string tifImagePath, string imagePath, long workId, long pageCounter, string ImgDirPath, out long pageCounterOut, bool _isStoreToDB, FrictionLessReferral frictionLessReferralObj)
        {
            if (!Directory.Exists(imagePath))
                Directory.CreateDirectory(imagePath);
            var imgCount = _convertPDFToImages.CountTiffFileImages(tifImagePath);
            for (int i = 0; i < imgCount; i++, pageCounter++)
            {
                using (
                var bitmap = _convertPDFToImages.ExtractImageFromTiffFile(tifImagePath, i, imagePath + "\\Logo_" + workId + "_" + pageCounter + ".jpg"))
                {
                    var size = bitmap.Size.Width * bitmap.Size.Height;
                    Bitmap bmp = new Bitmap(bitmap);
                    if (_isStoreToDB == false)
                    {
                        bitmap.Dispose();
                        bmp.Save(imagePath + "\\" + workId + "_" + pageCounter + "_" + i + ".jpg", ImageFormat.Jpeg);
                    }
                    if (_isStoreToDB == true)
                    {
                        bitmap.Dispose();
                        bmp.Save(imagePath + "\\" + workId + "_" + pageCounter + ".jpg", ImageFormat.Jpeg);
                        var imgPath = ImgDirPath + "\\" + workId + "_" + pageCounter + ".jpg";
                        var logoImgPath = ImgDirPath + "\\Logo_" + workId + "_" + pageCounter + ".jpg";

                        if (!frictionLessReferralObj.IS_SIGNED_REFERRAL)
                        {
                            AddFrictionlessFilesToDatabase(imgPath, workId, logoImgPath);
                        }
                        else
                        {
                            AddFilesToDatabase(imgPath, workId, logoImgPath);
                        }

                    }
                    bmp.Dispose();
                    // encoderParameters.Dispose();
                }
            }
            pageCounterOut = pageCounter;
            return imgCount;
        }
        public void AddFrictionlessFilesToDatabase(string filePath, long workId, string logoPath)
        {
            try
            {
                long frictionlessRefID = Helper.getMaximumId("FRICTIONLESS_REFERRAL_FILE_ID");
                var frictionlessRefId = new SqlParameter("FRICTIONLESS_ID", SqlDbType.BigInt) { Value = frictionlessRefID };
                var parmWorkID = new SqlParameter("WORKID", SqlDbType.BigInt) { Value = workId };
                var parmFilePath = new SqlParameter("FILEPATH", SqlDbType.VarChar) { Value = filePath };
                var parmLogoPath = new SqlParameter("LOGOPATH", SqlDbType.VarChar) { Value = logoPath };
                var result = SpRepository<OriginalQueueFiles>.GetSingleObjectWithStoreProcedure(@"exec FOX_PROC_ADD_FRICTIONLESS_FILES_TO_DB_FROM_RFO @FRICTIONLESS_ID,  @WORKID, @FILEPATH, @LOGOPATH",
                    frictionlessRefId, parmWorkID, parmFilePath, parmLogoPath);
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }
        public ResponseModel SendEmail(RequestSendEmailModel requestSendEmailModel, UserProfile Profile)
        {
            try
            {
                var frictionLessReferralData = _frictionlessReferralRepository.GetFirst(t => t.DELETED == false && t.WORK_ID == requestSendEmailModel.WorkId);
                var config = Helper.GetServiceConfiguration(Profile.PracticeCode);
                if (config.PRACTICE_CODE != null
                    && !string.IsNullOrWhiteSpace(config.ORIGINAL_FILES_PATH_DB) && !string.IsNullOrWhiteSpace(config.ORIGINAL_FILES_PATH_SERVER)
                    && !string.IsNullOrWhiteSpace(config.IMAGES_PATH_DB) && !string.IsNullOrWhiteSpace(config.IMAGES_PATH_SERVER))
                {
                    var encryptedWorkId = requestSendEmailModel.WorkId.ToString();
                    string link = "";
                    link = AppConfiguration.ClientURL + @"#/VerifyWorkOrder?value=" + HttpUtility.UrlEncode(encryptedWorkId);
                    link += "&name=" + requestSendEmailModel.EmailAddress;
                    link += "&isFrictionLess=" + true;
                    string linkMessage = @"<p>Please <a href='" + link + @"'>click here for signing</a> to confirm that you have reviewed and are an agreement of this request.   Once you click, the document will electronically be signed by you with the current date and time.  Thank you for your confidence in our practice. ";
                    ResponseHTMLToPDF responseHTMLToPDF = HTMLToPDF(config, requestSendEmailModel.AttachmentHTML, requestSendEmailModel.FileName.Replace(' ', '_'), "email", linkMessage);
                    AddHtmlToDB(requestSendEmailModel.AttachmentHTML, Profile.UserName, frictionLessReferralData);
                    if (responseHTMLToPDF != null && (responseHTMLToPDF?.Success ?? false))
                    {
                        string attachmentPath = "";
                        List<string> _bccList = new List<string>() { "abdulsattar@carecloud.com" };
                        string _body = String.Empty;
                        _body = @"
                        <table style='width:100%; padding:0px;background:#fff;font-family: sans-serif !important;' cellpadding='0' cellspacing='0'>
						<tr>
							<td style='height:15px;width:100%;'></td>
						</tr>
						<tr>
							<td align='center' style='padding: 0px 20px;'>
								<table style='width:100%;margin:0 auto;background:#fff;' cellpadding='0' cellspacing='0'>
									<tr>
										<td align='left'>
											<img src='https://fox.mtbc.com/assets/images/email-logo.png' alt='Fox logo' />
										</td>
									</tr>
								</table>
							</td>
						</tr>
						<tr>
							<td style='height:5px;width:100%;background-color:#ff671f; '></td>
						</tr>
						<tr>
							<td style='height:15px;width:100%;'></td>
						</tr>
						<tr>
							<td style='background:#fff;'>
								<table style='width:100%;margin:0 auto;background:transparent;font-family: sans-serif !important;' cellpadding='0' cellspacing='0'>
									<tr>
										<td style='padding:0px 20px;'>
											<table style='width:100%;margin:0 auto;font-family: sans-serif !important;' cellpadding='0' cellspacing='0'>
												<tr>
													<td style='width:100%;font-size:14px;font-family: sans-serif !important;line-height: 1.5;'>
                                                       ***Important Message regarding your patient’s care***<br><br>
                                                       Please login to see the request for referral from Fox Rehabilitation
													</td>
                                                </tr>
                                                <tr>
                                                    <td>&nbsp;</td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <table style='width:250px;font-family: sans-serif !important;' cellpadding='0' cellspacing='0'>
                                                            <tr>
                                                                <td style='text-align: center;background-color:#ff671f; border:1px solid #ff671f; vertical-align:middle; line-height:normal; padding:5px 15px 5px 15px;'>
                                                                    <a style='color:#fff; font-size:16px;text-decoration:none; outline:none;background-color:#ff671f;' target='_blank' href='" + link + @"'>Login To Fox Rehab Portal</a>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                    
                                                </tr>
                                                <tr>
                                                        <td>&nbsp;</td>
                                                    </tr>
                                                <tr>
                                                    <td style='line-height: 1.5'>
                                                        <strong>Rehabilitation </strong><br>
                                                        <strong>T</strong> +1 (877) 407 - 3422 <br>
                                                        <strong>F</strong> +1 (877) 407 - 4329 Main<br>
                                                        <strong>F</strong> +1 (800) 597 - 0848 Patient Referral<br>
                                                        <strong>E</strong> clientservices@foxrehab.org<br><br>
                                                            
                                                        <strong>Checkout our NEW website</strong> - <a href='www.foxrehab.org' target='_blank'>www.foxrehab.org</a> <br><br>
                                                            
                                                        Physical, Occupational & Speech Therapy.<br>
                                                        <strong>FOX Rehabilitates Lives.</strong>                                                             
                                                    </td>
                                                </tr>
											</table>
										</td>
									</tr>
								</table>
							</td>
						</tr>
					</table>
                            ";
                        bool emailStatus = false;
                        emailStatus = Helper.Email(requestSendEmailModel.EmailAddress, requestSendEmailModel.Subject, _body, Profile, requestSendEmailModel.WorkId, null, _bccList, new List<string>() { attachmentPath });
                        var queueResult = _QueueRepository.GetFirst(s => s.WORK_ID == requestSendEmailModel.WorkId && s.DELETED == false);
                        if (frictionLessReferralData != null)
                        {
                            queueResult.REFERRAL_EMAIL_SENT_TO = requestSendEmailModel.EmailAddress;
                            _QueueRepository.Save();
                        }
                        string filePath = responseHTMLToPDF?.FilePath + responseHTMLToPDF?.FileName;
                        int numberOfPages = getNumberOfPagesOfPDF(filePath);
                        SavePdfToImages(filePath, config, requestSendEmailModel.WorkId, numberOfPages, "Email", requestSendEmailModel.EmailAddress, Profile.UserName, requestSendEmailModel._isFromIndexInfo);
                        return new ResponseModel() { Message = "Email sent successfully, our admission team is processing your referral", ErrorMessage = "", Success = true };
                    }
                    else
                    {
                        return new ResponseModel() { Message = "We encountered an error while processing your request.", ErrorMessage = responseHTMLToPDF?.ErrorMessage, Success = false };
                    }
                }
                else
                {
                    return new ResponseModel() { Message = "Email could not be sent.", ErrorMessage = "DB configuration for file paths not found. See service configuration.", Success = false };
                }
            }
            catch (Exception exception)
            {
                return new ResponseModel() { Message = "We encountered an error while processing your request.", ErrorMessage = exception.ToString(), Success = false };
            }
        }
        public void AddHtmlToDB(string htmll, string userName, FrictionLessReferral FrictionLessReferralObj)
        {
            var work_order = _QueueRepository.GetFirst(t => t.WORK_ID == FrictionLessReferralObj.WORK_ID && t.DELETED == false);
            var practiceCode = work_order.PRACTICE_CODE.Value.ToString();
            var documentType = _foxdocumenttypeRepository.GetFirst(t => t.DOCUMENT_TYPE_ID == work_order.DOCUMENT_TYPE).NAME ?? "";
            var pri_insurance = "";
            var ins_name = "";
            if (!string.IsNullOrEmpty(FrictionLessReferralObj.PATIENT_INSURANCE_PAYER_ID))
            {
                ins_name = _foxInsurancePayersRepository.GetFirst(t => t.DELETED == false && t.INSURANCE_PAYERS_ID == FrictionLessReferralObj.PATIENT_INSURANCE_PAYER_ID).INSURANCE_NAME ?? "";
            }
            if (!String.IsNullOrWhiteSpace(ins_name))
            {
                pri_insurance = ins_name;
            }
            var file_name = FrictionLessReferralObj.PATIENT_LAST_NAME + "_" + documentType;
            var fcClass = new FinancialClass();
            var discipline = "";
            if (work_order != null)
            {
                if (!string.IsNullOrEmpty(work_order?.DEPARTMENT_ID))
                {
                    if (work_order.DEPARTMENT_ID.Contains("1"))
                    {
                        discipline = discipline + " Occupational Therapy (OT), ";
                    }
                    if (work_order.DEPARTMENT_ID.Contains("2"))
                    {
                        discipline = discipline + " Physical Therapy (PT), ";
                    }
                    if (work_order.DEPARTMENT_ID.Contains("3"))
                    {
                        discipline = discipline + " Speech Therapy (ST), ";
                    }
                    if (work_order.DEPARTMENT_ID == "4")
                    {
                        discipline = discipline + "Physical/Occupational/Speech Therapy(PT/OT/ST)";
                    }
                    if (work_order.DEPARTMENT_ID == "5")
                    {
                        discipline = discipline + "Physical/Occupational Therapy(PT/OT)";
                    }
                    if (work_order.DEPARTMENT_ID == "6")
                    {
                        discipline = discipline + "Physical/Speech Therapy(PT/ST)";
                    }
                    if (work_order.DEPARTMENT_ID == "7")
                    {
                        discipline = discipline + "Occupational/Speech Therapy(OT/ST)";
                    }
                    if (work_order.DEPARTMENT_ID.Contains("8"))
                    {
                        discipline = discipline + " Unknown, ";
                    }
                    if (work_order.DEPARTMENT_ID.Contains("9"))
                    {
                        discipline = discipline + " Exercise Physiology (EP), ";
                    }
                }
                else
                {
                    discipline = "";
                }
                if (discipline.Substring(discipline.Length - 2) == ",")
                {
                    discipline = discipline.TrimEnd(discipline[discipline.Length - 1]);
                }
            }
            QRCodeModel qr = new QRCodeModel();
            qr.AbsolutePath = System.Web.HttpContext.Current.Server.MapPath("~/" + AppConfiguration.QRCodeTempPath);
            qr.WORK_ID = FrictionLessReferralObj.WORK_ID;
            var qrCode = GenerateQRCode(qr);
            string body = string.Empty;
            string templatePathOfSenderEmail = HttpContext.Current.Server.MapPath(@"~/HtmlTemplates/print-send-submit-order.html");
            if (File.Exists(templatePathOfSenderEmail))
            {
                string receivedDate = string.Empty;
                string receivedTime = string.Empty;
                body = File.ReadAllText(templatePathOfSenderEmail);
                HtmlDocument htmldoc = new HtmlDocument();
                htmldoc.LoadHtml(body);

                body = htmldoc.DocumentNode.OuterHtml;

                body = body.Replace("[[PATIENT_NAME]]", FrictionLessReferralObj.PATIENT_LAST_NAME + ", " + FrictionLessReferralObj.PATIENT_FIRST_NAME);
                //      body = body.Replace("[[PATIENT_DOB]]", FrictionLessReferralObj.PATIENT_DOB_STRING.ToString() ?? "");
                body = body.Replace("[[PATIENT_PRI_INS]]", pri_insurance ?? "");
                if (qrCode != null)
                {
                    body = body.Replace("[[QRCode]]", qrCode.ENCODED_IMAGE_BYTES ?? "");
                }
                body = body.Replace("[[DOCUMENT_TYPE]]", documentType ?? "");
                if (!string.IsNullOrEmpty(FrictionLessReferralObj.PROVIDER_FIRST_NAME) && !string.IsNullOrEmpty(FrictionLessReferralObj.PROVIDER_LAST_NAME))
                {
                    body = body.Replace("[[ORS]]", FrictionLessReferralObj.PROVIDER_LAST_NAME + ", " + FrictionLessReferralObj.PROVIDER_FIRST_NAME ?? "");
                }
                body = body.Replace("[[SENDER]]", string.IsNullOrEmpty(FrictionLessReferralObj.SUBMITER_FIRST_NAME) ? "" : FrictionLessReferralObj.SUBMITTER_LAST_NAME + ", " + FrictionLessReferralObj.SUBMITER_FIRST_NAME ?? "");
                body = body.Replace("[[discipline]]", discipline ?? "");
                body = body.Replace("[[additional_notes]]", FrictionLessReferralObj.PATIENT_REFERRAL_NOTES ?? "");
                if (!string.IsNullOrEmpty(FrictionLessReferralObj.PROVIDER_NPI))
                {
                    body = body.Replace("[[provider_name]]", FrictionLessReferralObj.PROVIDER_LAST_NAME + ", " + FrictionLessReferralObj.PROVIDER_FIRST_NAME + " " + FrictionLessReferralObj.PROVIDER_REGION);
                    body = body.Replace("[[provider_NPI]]", FrictionLessReferralObj.PROVIDER_NPI ?? "");
                    body = body.Replace("[[provider_fax]]", DataModels.HelperClasses.StringHelper.ApplyPhoneMask(FrictionLessReferralObj.PROVIDER_FAX) ?? "");
                    body = body.Replace("[[provider_date]]", Helper.GetCurrentDate().ToShortDateString() ?? "");
                }
                else
                {
                    body = body.Replace("[[provider_name]]", "");
                    body = body.Replace("[[provider_NPI]]", "");
                    body = body.Replace("[[provider_phone]]", "");
                    body = body.Replace("[[provider_fax]]", "");
                    body = body.Replace("[[provider_date]]", "");
                }
                body = body.Replace("<img style=\"width:30%; height: 60px;margin:6px;\" src=\"[[Signature]]\" alt=\"Signature\">", "{{SignaturePath}}");
                body = body.Replace("[[current_Date]]", "{{TodayDate}}");
            }
            long Pid = Helper.getMaximumId("THERAPY_TREATMENT_REFERRAL_REQUEST_FORM_ID");
            TherapyTreatmentRequestForm obj = new TherapyTreatmentRequestForm();
            obj.THERAPY_TREATMENT_REFERRAL_REQUEST_FORM_ID = Pid;
            obj.WORK_ID = FrictionLessReferralObj.WORK_ID;
            obj.THERAPY_TREATMENT_REFERRAL_REQUEST_HTML = body;
            obj.CREATED_BY = userName;
            obj.CREATED_DATE = Helper.GetCurrentDate();
            _fox_tbl_TherapyTreatmentRequestForm.Insert(obj);
            _fox_tbl_TherapyTreatmentRequestForm.Save();
        }
        public long AddUpdateWorkOrder(FrictionLessReferral frictionLessReferralObj, UserProfile Profile)
        {
            long workId = 0;
            var workOrder = _QueueRepository.GetFirst(x => x.WORK_ID == frictionLessReferralObj.WORK_ID && !x.DELETED);
            if (workOrder != null)
            {
                workId = workOrder.WORK_ID;
                if (frictionLessReferralObj.IS_SIGNED_REFERRAL)
                {
                    int document_type = _foxdocumenttypeRepository.GetFirst(x => x.NAME == "Signed Order" && !x.DELETED && x.IS_ACTIVE == true).DOCUMENT_TYPE_ID;
                    if(document_type != 0)
                    {
                        workOrder.DOCUMENT_TYPE = document_type;
                    }                
                }
                else
                {
                    int document_type = _foxdocumenttypeRepository.GetFirst(x => x.NAME == "Unsigned Order" && !x.DELETED && x.IS_ACTIVE == true).DOCUMENT_TYPE_ID;
                    if (document_type != 0)
                    {
                        workOrder.DOCUMENT_TYPE = document_type;
                    }
                }
                workOrder.CREATED_BY = workOrder.MODIFIED_BY = Profile.UserName;
                workOrder.MODIFIED_DATE = DateTime.Now;
                workOrder.RFO_Type = frictionLessReferralObj.USER_TYPE;
                workOrder.IsSigned = frictionLessReferralObj.IS_SIGNED_REFERRAL;
                workOrder.SORCE_NAME = frictionLessReferralObj.SUBMITTER_EMAIL;
                workOrder.DEPARTMENT_ID = frictionLessReferralObj.PATIENT_DISCIPLINE_ID;
                workOrder.REASON_FOR_VISIT = frictionLessReferralObj.PATIENT_REFERRAL_NOTES;

                _QueueRepository.Update(workOrder);
                _QueueRepository.Save();
                GenerateAndSaveImagesOfUploadedFiles(workOrder.WORK_ID, frictionLessReferralObj, Profile);
            }
            else
            {
                OriginalQueue originalQueue = new OriginalQueue();
                workId = Helper.getMaximumId("WORK_ID");
                if (frictionLessReferralObj.IS_SIGNED_REFERRAL)
                {
                    int document_type = _foxdocumenttypeRepository.GetFirst(x => x.NAME == "Signed Order" && !x.DELETED && x.IS_ACTIVE == true).DOCUMENT_TYPE_ID;
                    if (document_type != 0)
                    {
                        originalQueue.DOCUMENT_TYPE = document_type;
                    }
                }
                else
                {
                    int document_type = _foxdocumenttypeRepository.GetFirst(x => x.NAME == "Unsigned Order" && !x.DELETED && x.IS_ACTIVE == true).DOCUMENT_TYPE_ID;
                    if (document_type != 0)
                    {
                        originalQueue.DOCUMENT_TYPE = document_type;
                    }
                }
                originalQueue.WORK_ID = workId;
                originalQueue.UNIQUE_ID = workId.ToString();
                originalQueue.PRACTICE_CODE = Profile.PracticeCode;
                originalQueue.CREATED_BY = originalQueue.MODIFIED_BY = Profile.UserName;
                originalQueue.CREATED_DATE = originalQueue.MODIFIED_DATE = DateTime.Now;
                originalQueue.IS_EMERGENCY_ORDER = false;
                originalQueue.supervisor_status = false;
                originalQueue.DELETED = false;
                originalQueue.RECEIVE_DATE = originalQueue.CREATED_DATE;
                originalQueue.SORCE_TYPE = "Email";
                originalQueue.RFO_Type = frictionLessReferralObj.USER_TYPE;
                originalQueue.IsSigned = frictionLessReferralObj.IS_SIGNED_REFERRAL;
                originalQueue.SORCE_NAME = frictionLessReferralObj.SUBMITTER_EMAIL;
                originalQueue.WORK_STATUS = "Created";
                originalQueue.DEPARTMENT_ID = frictionLessReferralObj.PATIENT_DISCIPLINE_ID;
                originalQueue.REASON_FOR_VISIT = frictionLessReferralObj.PATIENT_REFERRAL_NOTES;
                originalQueue.ASSIGNED_TO = null;
                originalQueue.ASSIGNED_BY = null;
                originalQueue.ASSIGNED_DATE = null;
                _QueueRepository.Insert(originalQueue);
                _QueueRepository.Save();
                GenerateAndSaveImagesOfUploadedFiles(workId, frictionLessReferralObj, Profile);
            }
            return workId;
        }
    }
    #endregion
}
