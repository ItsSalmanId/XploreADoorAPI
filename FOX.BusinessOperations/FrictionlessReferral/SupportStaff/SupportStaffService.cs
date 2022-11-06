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
using FOX.DataModels.Models.RequestForOrder.UploadOrderImages;
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
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Web;
using System.Web.Configuration;

namespace FOX.BusinessOperations.FrictionlessReferral.SupportStaff
{
    public class SupportStaffService : ISupportStaffService 
    {
        private readonly IUploadOrderImagesService _IUploadOrderImagesService;

        #region PROPERTIES
        // DB Context Objects
        private readonly DbContextFrictionless _dbContextFrictionLess = new DbContextFrictionless();
        private readonly GenericRepository<OriginalQueue> _QueueRepository;
        private readonly DbContextPatient _dbContextPatient = new DbContextPatient();
        private readonly DbContextCommon _dbContextCommon = new DbContextCommon();
        private readonly DBContextQueue _QueueContext = new DBContextQueue();
        private readonly GenericRepository<FOX_TBL_NOTES_HISTORY> _NotesRepository;
        private readonly UploadOrderImagesService _uploadOrderImagesService;
        private readonly DbContextIndexinfo _IndexinfoContext = new DbContextIndexinfo();

        // Generic Repository Objects
        private readonly GenericRepository<FoxInsurancePayers> _insurancePayerRepository;
        private readonly GenericRepository<PHR> _phrRepository;
        private readonly GenericRepository<Provider> _providerRepository;
        private readonly GenericRepository<FrictionLessReferral> _frictionlessReferralRepository;
        private readonly GenericRepository<OriginalQueueFiles> _OriginalQueueFiles;
        private static List<Thread> threadsList = new List<Thread>();
        // Class Objects
        PatientService patientServices = new PatientService();
        #endregion
        #region CONSTRUCTOR
        public SupportStaffService(IUploadOrderImagesService IUploadOrderImagesService)
        {
            _insurancePayerRepository = new GenericRepository<FoxInsurancePayers>(_dbContextFrictionLess);
            _phrRepository = new GenericRepository<PHR>(_dbContextPatient);
            _providerRepository = new GenericRepository<Provider>(_dbContextCommon);
            _frictionlessReferralRepository = new GenericRepository<FrictionLessReferral>(_dbContextFrictionLess);
            _OriginalQueueFiles = new GenericRepository<OriginalQueueFiles>(_QueueContext);
            _IUploadOrderImagesService = IUploadOrderImagesService;
            _QueueRepository = new GenericRepository<OriginalQueue>(_QueueContext);
            _uploadOrderImagesService = new UploadOrderImagesService();
            _NotesRepository = new GenericRepository<FOX_TBL_NOTES_HISTORY>(_IndexinfoContext);
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
            if (patientDetails != null && !string.IsNullOrEmpty(patientDetails.EmailAddress) && !string.IsNullOrEmpty(patientDetails.MobilePhone))
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
            if (!string.IsNullOrEmpty(patientDetails.EmailAddress) && !string.IsNullOrEmpty(patientDetails.MobilePhone))
            {
                string urlLink = GetEncryptedUrlLink(patientDetails.EmailAddress, patientDetails.MobilePhone, "", pinCode.ToString());
                if (!string.IsNullOrEmpty(urlLink))
                {
                    var status = SmsService.SMSTwilio(patientDetails.MobilePhone, urlLink);
                    if (status != null && status == "Success")
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
            else if (practiceCode != 0 && !string.IsNullOrEmpty(obj.ProviderFirstName) && !string.IsNullOrEmpty(obj.ProviderLastName) && !string.IsNullOrEmpty(obj.ProviderState))
            {
                providerResponse = _providerRepository.GetMany(x => x.FIRST_NAME == obj.ProviderFirstName && x.LAST_NAME == obj.ProviderLastName && x.STATE == obj.ProviderState && x.PRACTICE_CODE == practiceCode && !(x.DELETED.HasValue ? x.DELETED.Value : false));
                // Search on NPPES
                if (providerResponse.Count == 0)
                {
                    string url = AppConfiguration.NPPESNPIRegistry + "&first_name=" + obj.ProviderFirstName + "&last_name=" + obj.ProviderLastName + "&state=" + obj.ProviderState;
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
                if (frictionLessReferralObj.PATIENT_DISCIPLINE_ID.StartsWith(",") && frictionLessReferralObj.PATIENT_DISCIPLINE_ID != null)
                {
                    frictionLessReferralObj.PATIENT_DISCIPLINE_ID = frictionLessReferralObj.PATIENT_DISCIPLINE_ID.Remove(0, 1);
                }
                var existingFrictionReferral = _frictionlessReferralRepository.GetFirst(f => f.FRICTIONLESS_REFERRAL_ID == frictionLessReferralObj.FRICTIONLESS_REFERRAL_ID && f.PRACTICE_CODE == practiceCode && f.DELETED == false);
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
                    existingFrictionReferral.PATIENT_DOB = Convert.ToDateTime(frictionLessReferralObj.PATIENT_DOB_STRING);
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
            var x = frictionLessReferralResponse.FrictionLessReferralObj.PATIENT_DOB.ToString();
            var s = x.Split();
            var xxx = s[0];
            frictionLessReferralResponse.FrictionLessReferralObj.PATIENT_DOB_STRING = xxx;
            if (frictionLessReferralObj.FILE_NAME_LIST.Count != 0) {

                UserProfile userProfile = new UserProfile();
                userProfile.PracticeCode = GetPracticeCode();
                userProfile.UserName = "FOX TEAM";
                var result = new ResSaveUploadWorkOrderFiles();
                // long workId = Helper.getMaximumId("WORK_ID");
                //==========================================================
                SubmitUploadOrderImages(frictionLessReferralObj, userProfile);
                //=========================================================
            }
            //var originalQueueFilesCount = 0;
            //Helper.TokenTaskCancellationExceptionLog("UploadWorkOrderFiles: In Function  SaveUploadWorkOrderFiles > GenerateAndSaveImagesOfUploadedFiles || Start Time of Function GenerateAndSaveImagesOfUploadedFiles" + Helper.GetCurrentDate().ToLocalTime());
            //_IUploadOrderImagesService.GenerateAndSaveImagesOfUploadedFiles(workId, frictionLessReferralObj.FILE_NAME_LIST, userProfile, originalQueueFilesCount);
            //Helper.TokenTaskCancellationExceptionLog("UploadWorkOrderFiles: In Function  SaveUploadWorkOrderFiles > GenerateAndSaveImagesOfUploadedFiles || End Time of Function GenerateAndSaveImagesOfUploadedFiles" + Helper.GetCurrentDate().ToLocalTime());

            //result.WORK_ID = workId;
            //result.FilePaths = SpRepository<FilePath>.GetListWithStoreProcedure(@"exec FOX_GET_File_PAGES  @WORK_ID", new SqlParameter("WORK_ID ", SqlDbType.BigInt) { Value = workId });
            //result.Message = $"Upload Work Order Files Successfully. WorkId = { workId }";
            //result.ErrorMessage = "";
            //result.Success = true;

            //decimal size = 0;
            //decimal byteCount = 0;
            //foreach (var list in result.FilePaths.ToList())
            //{
            //    string virtualPath = @"/" + list.file_path1;
            //    string orignalPath = HttpContext.Current.Server.MapPath("~" + virtualPath);
            //    FileInfo file = new FileInfo(orignalPath);
            //    bool exists = file.Exists;
            //    if (file.Exists)
            //    {
            //        byteCount = file.Length;
            //        size += byteCount;
            //    }
            //}
            //result.fileSize = Convert.ToDecimal(string.Format("{0:0.00}", size / 1048576));



            return frictionLessReferralResponse;
        }
        #endregion


        public ResSubmitUploadOrderImagesModel SubmitUploadOrderImages(FrictionLessReferral frictionLessReferralObj, UserProfile Profile)
        {
            //try
            //{54819524
            var workId = Helper.getMaximumId("WORK_ID");
            Helper.TokenTaskCancellationExceptionLog("UploadOrderImages: In Function  SubmitUploadOrderImages Work_ID (" + workId + ") || Start Time of Function SubmitUploadOrderImages" + Helper.GetCurrentDate().ToLocalTime());
            OriginalQueue originalQueue = new OriginalQueue();

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
      //    originalQueue.DOCUMENT_TYPE = reqSubmitUploadOrderImagesModel.DOCUMENT_TYPE;
            originalQueue.DEPARTMENT_ID = frictionLessReferralObj.PATIENT_DISCIPLINE_ID;
            originalQueue.REASON_FOR_VISIT = frictionLessReferralObj.PATIENT_REFERRAL_NOTES;
            originalQueue.ASSIGNED_TO = null;
            originalQueue.ASSIGNED_BY = null;
            originalQueue.ASSIGNED_DATE = null;

            _QueueRepository.Insert(originalQueue);
            _QueueRepository.Save();

            Helper.TokenTaskCancellationExceptionLog("UploadOrderImages: In Function  SubmitUploadOrderImages > GenerateAndSaveImagesOfUploadedFiles || Start Time of Function GenerateAndSaveImagesOfUploadedFiles" + Helper.GetCurrentDate().ToLocalTime());
            _IUploadOrderImagesService.GenerateAndSaveImagesOfUploadedFiles(workId, frictionLessReferralObj.FILE_NAME_LIST, Profile);
            Helper.TokenTaskCancellationExceptionLog("UploadOrderImages: In Function  SubmitUploadOrderImages > GenerateAndSaveImagesOfUploadedFiles || End Time of Function GenerateAndSaveImagesOfUploadedFiles" + Helper.GetCurrentDate().ToLocalTime());

           // if (reqSubmitUploadOrderImagesModel.Is_Manual_ORS)
            if (true)
            {
                string body = string.Empty;
                string template_html = HttpContext.Current.Server.MapPath(@"~/HtmlTemplates/ORS_info_Template.html");
                Profile.PracticeCode = 1011163;
                var config = Helper.GetServiceConfiguration(Profile.PracticeCode);
                //body = File.ReadAllText(template_html);
                //body = body.Replace("[[provider_name]]", reqSubmitUploadOrderImagesModel.ORS_NAME ?? "");
                //body = body.Replace("[[provider_NPI]]", reqSubmitUploadOrderImagesModel.ORS_NPI ?? "");
                //body = body.Replace("[[provider_phone]]", DataModels.HelperClasses.StringHelper.ApplyPhoneMask(reqSubmitUploadOrderImagesModel.ORS_PHONE) ?? "");
                //body = body.Replace("[[provider_fax]]", DataModels.HelperClasses.StringHelper.ApplyPhoneMask(reqSubmitUploadOrderImagesModel.ORS_FAX) ?? "");
                long pageCounter = 1;
                ResponseHTMLToPDF responseHTMLToPDF2 = RequestForOrder.RequestForOrderService.HTMLToPDF2(config, body, "orsInfo");
                string coverfilePath = responseHTMLToPDF2?.FilePath + responseHTMLToPDF2?.FileName;
                var ext = Path.GetExtension(coverfilePath).ToLower();
                int numberOfPages = getNumberOfPagesOfPDF(coverfilePath);

                Helper.TokenTaskCancellationExceptionLog("UploadOrderImages: In Function  SubmitUploadOrderImages > SavePdfToImages || Start Time of Function SavePdfToImages" + Helper.GetCurrentDate().ToLocalTime());
               SavePdfToImages(coverfilePath, config, workId, numberOfPages, Convert.ToInt32(pageCounter), out pageCounter);
                Helper.TokenTaskCancellationExceptionLog("UploadOrderImages: In Function  SubmitUploadOrderImages > SavePdfToImages || End Time of Function SavePdfToImages" + Helper.GetCurrentDate().ToLocalTime());

                FOX_TBL_NOTES_HISTORY notes = new FOX_TBL_NOTES_HISTORY();
                notes.NOTE_ID = Helper.getMaximumId("NOTE_ID");

                notes.CREATED_BY = Profile.UserName;
                notes.CREATED_DATE = Helper.GetCurrentDate().ToString();
                notes.DELETED = false;
                notes.MODIFIED_DATE = Helper.GetCurrentDate();
                notes.MODIFIED_BY = Profile.UserName;
                notes.PRACTICE_CODE = Profile.PracticeCode;
                _NotesRepository.Insert(notes);
                _NotesRepository.Save();

                var newObj = new FOX_TBL_NOTES_HISTORY()
                {
                    WORK_ID = workId,
                    NOTE_DESC = "Custom ordering referral source is added by the user. See the attached referral for details"
                };
                InsertNotesHistory(newObj, Profile);
            }
            //if (!String.IsNullOrWhiteSpace(reqSubmitUploadOrderImagesModel.NOTE_DESC))
            //{
            //    var newObj = new FOX_TBL_NOTES_HISTORY()
            //    {
            //        WORK_ID = workId,
            //        NOTE_DESC = reqSubmitUploadOrderImagesModel.NOTE_DESC
            //    };
            //    InsertNotesHistory(newObj, Profile);
            //}
            //if (!string.IsNullOrWhiteSpace(reqSubmitUploadOrderImagesModel?.SPECIALITY_PROGRAM))
            //{
            //    var procedureDetail = InsertUpdateSpecialty(reqSubmitUploadOrderImagesModel, Profile, originalQueue, reqSubmitUploadOrderImagesModel.PATIENT_ACCOUNT);
            //}
            Helper.TokenTaskCancellationExceptionLog("UploadOrderImages: In Function  SubmitUploadOrderImages || End Time of Function SubmitUploadOrderImages" + Helper.GetCurrentDate().ToLocalTime());
            return new ResSubmitUploadOrderImagesModel() { Message = "Work Order Created Successfully. workId = " + workId, ErrorMessage = "", Success = true };
            //}
            //catch (Exception exception)
            //{
            //    //TO DO Log exception here
            //    //throw exception;
            //    return new ResSubmitUploadOrderImagesModel() { Message = "Work Order Created Successfully.", ErrorMessage = exception.ToString(), Success = false };
            //    //return new ResSubmitUploadOrderImagesModel() { Message = "We encountered an error while processing your request.", ErrorMessage = exception.ToString(), Success = false };
            //}

        }

        private void SavePdfToImages(string PdfPath, ServiceConfiguration config, long workId, int noOfPages, int pageCounter, out long pageCounterOut)
        {
            List<int> threadCounter = new List<int>();
            Helper.TokenTaskCancellationExceptionLog("UploadOrderImages: In Function  SavePdfToImages > Checking Time of Directory Create || Start Time of Function SavePdfToImages > Checking Time of Directory Create" + Helper.GetCurrentDate().ToLocalTime());
            if (!Directory.Exists(config.IMAGES_PATH_SERVER))
            {
                Directory.CreateDirectory(config.IMAGES_PATH_SERVER);
            }
            Helper.TokenTaskCancellationExceptionLog("UploadOrderImages: In Function  SavePdfToImages > Checking Time of Directory Create || End Time of Function SavePdfToImages > Checking Time of Directory Create" + Helper.GetCurrentDate().ToLocalTime());
            if (System.IO.File.Exists(PdfPath))
            {
                Helper.TokenTaskCancellationExceptionLog("UploadOrderImages: In Function  SavePdfToImages > Checking Time of Threading || Start Time of Function SavePdfToImages > Checking Time of Threading" + Helper.GetCurrentDate().ToLocalTime());
                for (int i = 0; i < noOfPages; i++, pageCounter++)
                {
                    Thread myThread = new Thread(() => this.newThreadImplementaion(ref threadCounter, PdfPath, i, config, workId, pageCounter));
                    myThread.Start();
                    threadsList.Add(myThread);
                    var imgPath = config.IMAGES_PATH_DB + "\\" + workId + "_" + pageCounter + ".jpg";
                    var logoImgPath = config.IMAGES_PATH_DB + "\\Logo_" + workId + "_" + pageCounter + ".jpg";
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
                Helper.TokenTaskCancellationExceptionLog("UploadOrderImages: In Function  SavePdfToImages > Checking Time of Threading || End Time of Function SavePdfToImages > Checking Time of Threading" + Helper.GetCurrentDate().ToLocalTime());
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
                //OriginalQueueFiles originalQueueFiles = _OriginalQueueFiles.GetFirst(t => t.WORK_ID == workId && !t.deleted && t.FILE_PATH1.Equals(filePath) && t.FILE_PATH.Equals(logoPath));

                //if (originalQueueFiles == null)
                //{
                //    //If Work Order files is deleted
                //    originalQueueFiles = _OriginalQueueFiles.Get(t => t.WORK_ID == workId && t.deleted && t.FILE_PATH1.Equals(filePath) && t.FILE_PATH.Equals(logoPath));
                //    if (originalQueueFiles == null)
                //    {
                //        originalQueueFiles = new OriginalQueueFiles();

                //        originalQueueFiles.FILE_ID = Helper.getMaximumId("FOXREHAB_FILE_ID");
                //        originalQueueFiles.WORK_ID = workId;
                //        originalQueueFiles.UNIQUE_ID = workId.ToString();
                //        originalQueueFiles.FILE_PATH1 = filePath;
                //        originalQueueFiles.FILE_PATH = logoPath;
                //        originalQueueFiles.deleted = false;

                //        //_OriginalQueueFiles.Insert(originalQueueFiles);
                //        //_OriginalQueueFiles.Save();
                //    }
                //}

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
    }
}
