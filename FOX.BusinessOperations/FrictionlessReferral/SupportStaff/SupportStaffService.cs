using FOX.BusinessOperations.CommonService;
using FOX.BusinessOperations.CommonServices;
using FOX.BusinessOperations.PatientServices;
using FOX.DataModels.Context;
using FOX.DataModels.GenericRepository;
using FOX.DataModels.Models.CommonModel;
using FOX.DataModels.Models.FrictionlessReferral.SupportStaff;
using FOX.DataModels.Models.Patient;
using FOX.ExternalServices;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Configuration;

namespace FOX.BusinessOperations.FrictionlessReferral.SupportStaff
{
    public class SupportStaffService : ISupportStaffService
    {
        #region PROPERTIES
        // DB Context Objects
        private readonly DbContextFrictionless _dbContextFrictionLess = new DbContextFrictionless();
        private readonly DbContextPatient _dbContextPatient = new DbContextPatient();
        private readonly DbContextCommon _dbContextCommon = new DbContextCommon();

        // Generic Repository Objects
        private readonly GenericRepository<FoxInsurancePayers> _insurancePayerRepository;
        private readonly GenericRepository<PHR> _phrRepository;
        private readonly GenericRepository<Provider> _providerRepository;
        private readonly GenericRepository<FrictionLessReferral> _frictionlessReferralRepository;

        // Class Objects
        PatientService patientServices = new PatientService();
        #endregion
        #region CONSTRUCTOR
        public SupportStaffService()
        {
            _insurancePayerRepository = new GenericRepository<FoxInsurancePayers>(_dbContextFrictionLess);
            _phrRepository = new GenericRepository<PHR>(_dbContextPatient);
            _providerRepository = new GenericRepository<Provider>(_dbContextCommon);
            _frictionlessReferralRepository = new GenericRepository<FrictionLessReferral>(_dbContextFrictionLess);
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
                var existingFrictionReferral = _frictionlessReferralRepository.GetFirst(f => f.FRICTIONLESS_REFERRAL_ID == frictionLessReferralObj.FRICTIONLESS_REFERRAL_ID && f.PRACTICE_CODE == practiceCode && f.DELETED == false);
                if (existingFrictionReferral == null)
                {
                    frictionLessReferralObj.FRICTIONLESS_REFERRAL_ID = Helper.getMaximumId("FRICTIONLESS_REFERRAL_ID");
                    frictionLessReferralObj.PRACTICE_CODE = GetPracticeCode();
                    frictionLessReferralObj.CREATED_BY = frictionLessReferralObj.MODIFIED_BY = frictionLessReferralObj.SUBMITTER_LAST_NAME;
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
            return frictionLessReferralResponse;
        }
        #endregion
    }
}
