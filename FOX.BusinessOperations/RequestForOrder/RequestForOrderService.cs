using System;
using System.Collections.Generic;
using System.Web;
using FOX.DataModels.Models.RequestForOrder;
using FOX.DataModels.Models.Security;
using FOX.DataModels.Models.OriginalQueueModel;
using FOX.DataModels.GenericRepository;
using FOX.BusinessOperations.CommonService;
using FOX.DataModels.Context;
using System.IO;

using FOX.BusinessOperations.FaxServices;
using SautinSoft;
using System.Drawing.Imaging;
using System.Drawing;
using FOX.BusinessOperations.CommonServices;
using SelectPdf;
using HtmlAgilityPack;
using FOX.DataModels.Models.ServiceConfiguration;
using FOX.DataModels.Models.Settings.ReferralSource;
using FOX.DataModels.Models.CasesModel;
using FOX.DataModels.Models.Settings.Practice;
using FOX.DataModels.Models.CommonModel;
using System.Data.SqlClient;
using System.Data;
using FOX.DataModels.Models.Patient;
using System.Globalization;
using FOX.DataModels.Models.IndexInfo;
using ZXing;
using System.Linq;
using FOX.DataModels.Models.SenderType;
using System.Threading;
using System.Diagnostics;
using FOX.DataModels;

namespace FOX.BusinessOperations.RequestForOrder
{
    public class RequestForOrderService : IRequestForOrderService
    {
        private readonly DBContextQueue _QueueContext = new DBContextQueue();
        private readonly GenericRepository<OriginalQueue> _QueueRepository;
        private readonly GenericRepository<OriginalQueueFiles> _OriginalQueueFiles;
        private readonly DbContextPatient _PatientContext = new DbContextPatient();

        private readonly DbContextSecurity security = new DbContextSecurity();
        private readonly GenericRepository<User> _UserRepository;

        private readonly DbContextCommon _DbContextCommon = new DbContextCommon();
        private readonly GenericRepository<DataModels.Models.SenderName.FOX_TBL_SENDER_NAME> _FOX_TBL_SENDER_NAME;

        private readonly DbContextIndexinfo _IndexinfoContext = new DbContextIndexinfo();
        private readonly GenericRepository<ReferralSource> _InsertUpdateOrderingSourceRepository;
        private readonly GenericRepository<FOX_TBL_IDENTIFIER> _fox_tbl_identifier;
        private readonly GenericRepository<PracticeOrganization> _fox_tbl_practice_organization;
        private readonly GenericRepository<TherapyTreatmentRequestForm> _fox_tbl_TherapyTreatmentRequestForm;
        private readonly GenericRepository<Patient> _PatientRepository;
        private readonly GenericRepository<OriginalQueue> _InsertSourceAddRepository;
        private readonly GenericRepository<FoxDocumentType> _foxdocumenttypeRepository;
        private readonly GenericRepository<PatientAddress> _PatientAddressRepository;
        private readonly GenericRepository<FOX_TBL_PATIENT> _FoxTblPatientRepository;
        private readonly GenericRepository<FOX_TBL_PATIENT_DIAGNOSIS> _InsertDiagnosisRepository;
        private readonly GenericRepository<FOX_TBL_PATIENT_PROCEDURE> _InsertProceuresRepository;
        private readonly GenericRepository<PatientInsurance> _PatientInsuranceRepository;
        private readonly GenericRepository<FoxInsurancePayers> _foxInsurancePayersRepository;
        private readonly GenericRepository<FinancialClass> _financialClassRepository;
        private readonly GenericRepository<User> _User;
        private readonly GenericRepository<FOX_TBL_SENDER_TYPE> _SenderTypeRepository;
        private readonly IFaxService _IFaxService = new FaxService();
        private static List<Thread> threadsList = new List<Thread>();
        //string ImgDirPath = "FoxDocumentDirectory\\Fox\\Images";
        public RequestForOrderService()
        {
            _QueueRepository = new GenericRepository<OriginalQueue>(_QueueContext);
            _OriginalQueueFiles = new GenericRepository<OriginalQueueFiles>(_QueueContext);
            _QueueRepository = new GenericRepository<OriginalQueue>(_QueueContext);
            _UserRepository = new GenericRepository<User>(security);
            _FOX_TBL_SENDER_NAME = new GenericRepository<DataModels.Models.SenderName.FOX_TBL_SENDER_NAME>(_DbContextCommon);
            _InsertUpdateOrderingSourceRepository = new GenericRepository<ReferralSource>(_IndexinfoContext);
            _fox_tbl_identifier = new GenericRepository<FOX_TBL_IDENTIFIER>(_IndexinfoContext);
            _fox_tbl_practice_organization = new GenericRepository<PracticeOrganization>(_DbContextCommon);
            _fox_tbl_TherapyTreatmentRequestForm = new GenericRepository<TherapyTreatmentRequestForm>(_QueueContext);
            _PatientRepository = new GenericRepository<Patient>(_PatientContext);
            _InsertSourceAddRepository = new GenericRepository<DataModels.Models.OriginalQueueModel.OriginalQueue>(_IndexinfoContext);
            _foxdocumenttypeRepository = new GenericRepository<FoxDocumentType>(_IndexinfoContext);
            _PatientAddressRepository = new GenericRepository<PatientAddress>(_PatientContext);
            _FoxTblPatientRepository = new GenericRepository<FOX_TBL_PATIENT>(_PatientContext);
            _InsertDiagnosisRepository = new GenericRepository<FOX_TBL_PATIENT_DIAGNOSIS>(_IndexinfoContext);
            _InsertProceuresRepository = new GenericRepository<FOX_TBL_PATIENT_PROCEDURE>(_IndexinfoContext);
            _PatientInsuranceRepository = new GenericRepository<PatientInsurance>(_PatientContext);
            _foxInsurancePayersRepository = new GenericRepository<FoxInsurancePayers>(_PatientContext);
            _financialClassRepository = new GenericRepository<FinancialClass>(_PatientContext);
            _User = new GenericRepository<User>(security);
            _SenderTypeRepository = new GenericRepository<FOX_TBL_SENDER_TYPE>(_DbContextCommon);

        }
        public ResponseGeneratingWorkOrder GeneratingWorkOrder(long practiceCode, string userName, string email, long userId, UserProfile Profile)
        {
            OriginalQueue originalQueue = new OriginalQueue();
            //var workId = Helper.getMaximumId("WORK_ID");
            var workId = 0;
            originalQueue.WORK_ID = workId;
            originalQueue.UNIQUE_ID = workId.ToString();
            originalQueue.PRACTICE_CODE = practiceCode;
            originalQueue.CREATED_BY = originalQueue.MODIFIED_BY = userName;
            originalQueue.CREATED_DATE = originalQueue.MODIFIED_DATE = DateTime.Now;
            originalQueue.IS_EMERGENCY_ORDER = false;
            originalQueue.supervisor_status = false;
            originalQueue.DELETED = false;
            originalQueue.RECEIVE_DATE = originalQueue.CREATED_DATE;
            originalQueue.SORCE_NAME = email;
            originalQueue.SORCE_TYPE = "Email";

            var usr = _UserRepository.GetByID(userId);
            FOX_TBL_SENDER_TYPE senderType = new FOX_TBL_SENDER_TYPE();
            //if (Profile.isTalkRehab)
            //{
            //    senderType = _SenderTypeRepository.GetFirst(t => t.FOX_TBL_SENDER_TYPE_ID == usr.FOX_TBL_SENDER_TYPE_ID && !t.DELETED);
            //}
            //else
            //{
                senderType = _SenderTypeRepository.GetFirst(t => t.FOX_TBL_SENDER_TYPE_ID == usr.FOX_TBL_SENDER_TYPE_ID && (t.PRACTICE_CODE == practiceCode) && !t.DELETED);
           // }
            if (usr != null)
            {
                originalQueue.FOX_TBL_SENDER_TYPE_ID = usr.FOX_TBL_SENDER_TYPE_ID;
                var senderName = _FOX_TBL_SENDER_NAME.GetFirst(t => t.SENDER_NAME_CODE.Equals(usr.USER_NAME));
                if (senderName == null)
                {
                    senderName = new DataModels.Models.SenderName.FOX_TBL_SENDER_NAME();
                    senderName.FOX_TBL_SENDER_NAME_ID = Helper.getMaximumId("FOX_TBL_SENDER_NAME_ID");
                    senderName.FOX_TBL_SENDER_TYPE_ID = null;
                    senderName.PRACTICE_CODE = practiceCode;
                    senderName.SENDER_NAME_CODE = usr.USER_NAME;
                    senderName.SENDER_NAME_DESCRIPTION = usr.USER_DISPLAY_NAME;
                    senderName.CREATED_BY = senderName.MODIFIED_BY = userName;
                    senderName.CREATED_DATE = senderName.MODIFIED_DATE = Helper.GetCurrentDate();
                    senderName.DELETED = false;

                    _FOX_TBL_SENDER_NAME.Insert(senderName);
                    _FOX_TBL_SENDER_NAME.Save();
                }
                originalQueue.FOX_TBL_SENDER_NAME_ID = senderName?.FOX_TBL_SENDER_NAME_ID;
            }

            originalQueue.WORK_STATUS = "Created";
            originalQueue.IS_VERIFIED_BY_RECIPIENT = false;

            //_QueueRepository.Insert(originalQueue);
            //_QueueRepository.Save();

            //Get all request in single call
            ICommonServices _ICommonServices = new CommonServices.CommonServices();
            var getSenderTypes = _ICommonServices.GetSenderTypes(Profile);
            var getSenderNames = _ICommonServices.GetSenderNames(
                    new DataModels.Models.CommonModel.ReqGetSenderNamesModel()
                    {
                        SenderTypeId = originalQueue.FOX_TBL_SENDER_TYPE_ID ?? 0,
                        SearchValue = "",
                        PracticeCode = practiceCode,
                        UserName = userName
                    }
                );

            //Get UserReferralSource
            ReferralSource refSrc = null;
            if (usr.USER_TYPE.ToLower().Contains("external") && (usr.IS_APPROVED ?? false)
                && usr.ROLE_ID.HasValue && usr.ROLE_ID == 108 && usr.USR_REFERRAL_SOURCE_ID != null && usr.USR_REFERRAL_SOURCE_ID != 0)
            {
                refSrc = _InsertUpdateOrderingSourceRepository.GetFirst(e => e.SOURCE_ID == usr.USR_REFERRAL_SOURCE_ID);
                if (refSrc != null)
                {
                    if (refSrc.ACO_ID != null)
                    {
                        var acoData = _fox_tbl_identifier.GetFirst(e => e.IDENTIFIER_ID == refSrc.ACO_ID && !e.DELETED);
                        if (acoData != null)
                        {
                            System.Globalization.TextInfo strConverter = new System.Globalization.CultureInfo("en-US", false).TextInfo;
                            refSrc.ACO_NAME = strConverter.ToTitleCase(acoData.NAME?.ToLower() ?? "");
                        }
                    }

                    if (refSrc.PRACTICE_ORGANIZATION_ID != null)
                    {
                        var poData = _fox_tbl_practice_organization.GetFirst(e => e.PRACTICE_ORGANIZATION_ID == refSrc.PRACTICE_ORGANIZATION_ID && !e.DELETED);
                        if (poData != null)
                        {
                            System.Globalization.TextInfo strConverter = new System.Globalization.CultureInfo("en-US", false).TextInfo;
                            refSrc.PRACTICE_ORGANIZATION_NAME = strConverter.ToTitleCase(poData.NAME?.ToLower() ?? "");
                        }
                    }
                }
            }


            return new ResponseGeneratingWorkOrder()
            {
                WORK_ID = originalQueue.WORK_ID,
                UNIQUE_ID = originalQueue.UNIQUE_ID,
                IS_EMERGENCY_ORDER = originalQueue.IS_EMERGENCY_ORDER,
                SORCE_NAME = originalQueue.SORCE_NAME,
                SORCE_TYPE = originalQueue.SORCE_TYPE,
                FOX_TBL_SENDER_TYPE_ID = originalQueue.FOX_TBL_SENDER_TYPE_ID,
                FOX_TBL_SENDER_NAME_ID = originalQueue.FOX_TBL_SENDER_NAME_ID,
                SenderTypeList = getSenderTypes.SenderTypeList,
                SenderNameList = getSenderNames.SenderNameList,
                UserReferralSource = refSrc,
                SENDER_TYPE = senderType != null ? !string.IsNullOrWhiteSpace(senderType.SENDER_TYPE_NAME) ? senderType.SENDER_TYPE_NAME : "" : "",

            };
            //return new ResponseGeneratingWorkOrder() { WorkId = workId };
            //return new ResponseGeneratingWorkOrder() { WorkId = 544600 };
        }
        public ResponseModel SendEmail(RequestSendEmailModel requestSendEmailModel, UserProfile Profile)
        {
            try
            {
                var config = Helper.GetServiceConfiguration(Profile.PracticeCode);
                if (config.PRACTICE_CODE != null
                    && !string.IsNullOrWhiteSpace(config.ORIGINAL_FILES_PATH_DB) && !string.IsNullOrWhiteSpace(config.ORIGINAL_FILES_PATH_SERVER)
                    && !string.IsNullOrWhiteSpace(config.IMAGES_PATH_DB) && !string.IsNullOrWhiteSpace(config.IMAGES_PATH_SERVER))
                {
                    OriginalQueue originalQueue = _QueueRepository.GetFirst(t => t.WORK_ID == requestSendEmailModel.WorkId && !t.DELETED && t.PRACTICE_CODE == Profile.PracticeCode);
                    if (originalQueue != null)
                    {
                        //var orderingRefSource = _InsertUpdateOrderingSourceRepository.Get(t => t.SOURCE_ID == originalQueue.SENDER_ID && !t.DELETED && t.PRACTICE_CODE == Profile.PracticeCode);
                        var orderingRefSource = _InsertUpdateOrderingSourceRepository.GetByID(originalQueue.SENDER_ID);
                        if (orderingRefSource != null)
                        {
                            //orderingRefSourceFullName = orderingRefSource?.LAST_NAME + ", " + orderingRefSource?.FIRST_NAME;
                        }
                    }

                    //var encryptedWorkId = StringCipher.Encrypt(requestSendEmailModel.WorkId.ToString()); talkRehabLogin
                    var encryptedWorkId = requestSendEmailModel.WorkId.ToString();
                    string link = "";
                    if(Profile.isTalkRehab)
                    {
                        link = AppConfiguration.ClientURL + @"#/account/login?talkRehabEmail="+ Profile.PracticeCode;
                    }
                    else
                    {
                        link = AppConfiguration.ClientURL + @"#/VerifyWorkOrder?value=" + HttpUtility.UrlEncode(encryptedWorkId);
                        link += "&name=" + requestSendEmailModel.EmailAddress;
                    }
                    
                    //if (!string.IsNullOrWhiteSpace(orderingRefSourceFullName))
                    //{
                    //    link += "&name=" + orderingRefSourceFullName;
                    //    link += "&name=" + requestSendEmailModel.EmailAddress;
                    //}
                    
                    string linkMessage = @"
                                <p>Please <a href='" + link + @"'>click here for signing</a> to confirm that you have reviewed and are an agreement of this request.   Once you click, the document will electronically be signed by you with the current date and time.  Thank you for your confidence in our practice.
                                ";

                    ResponseHTMLToPDF responseHTMLToPDF = HTMLToPDF(config, requestSendEmailModel.AttachmentHTML, requestSendEmailModel.FileName.Replace(' ', '_'), "email", linkMessage);
                    AddHtmlToDB(requestSendEmailModel.WorkId, requestSendEmailModel.AttachmentHTML, Profile.UserName);
                    //if (responseHTMLToPDF != null && (responseHTMLToPDF?.Success ?? false))
                        if (true)
                        {
                        //string attachmentPath = responseHTMLToPDF?.FilePath + responseHTMLToPDF?.FileName;
                        string attachmentPath = "";
                        //For Live
                        List<string> _bccList = new List<string>() { "abdulsattar@carecloud.com" };

                        //For QA UAT
                        //List<string> _bccList = new List<string>() { "abdulsattar@mtbc.com" };
                        //string _body = @"
                        //        <p>***Important Message regarding your patient’s care***</p>
                        //        <p>Please login to see the request for referral from Fox Rehabilitation</p>
                        //        <p><a style='padding: 5px 15px 5px 15px;color:#fff; font-size:16px;text-decoration:none; outline:none;background-color:#ff671f;' target='_blank' href='http://172.16.0.207:8961/#/VerifyWorkOrder?value=54812928&name=1163testing'>Login To Fox Rehab Portal</a></p>
                        //        < b>FOX Rehabilitation</b>
                        //        <br />
                        //        <b>T</b> 877 407 3422 <br />
                        //        <b>F</b> 877 407 4329 Main<br />
                        //        <b>F</b> 800 597 0848 Patient Referral<br />
                        //        <b>E</b> <a href='mailTo:clientservices@foxrehab.org'>clientservices@foxrehab.org</a><br />
                        //        <br />
                        //        <b>Checkout our NEW website -</b>
                        //        <a href='https://www.foxrehab.org/'>www.foxrehab.org</a>
                        //        <br />
                        //        <br />Physical, Occupational & Speech Therapy.<br />
                        //        <b>FOX Rehabilitates Lives.</b>
                        //        ";

                        string _body = String.Empty;
                        if (!Profile.isTalkRehab)
                        {
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
                                                                    <a style='color:#fff; font-size:16px;text-decoration:none; outline:none;background-color:#ff671f;' target='_blank' href='" +link + @"'>Login To Fox Rehab Portal</a>
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
                        }
                        else
                        {
                            _body = @"
                        <table style='width:100%; padding:0px;background:#fff;font-family: sans-serif !important;' cellpadding='0' cellspacing='0'>
						<tr>
							<td style='height:15px;width:100%;'></td>
						</tr>
						<tr>
							<td align='center' style='padding: 0px 20px;'>
								<table style='width:100%;margin:0 auto;background:#fff;margin-bottom:15px;' cellpadding='0' cellspacing='0'>
									<tr>
										<td align='left'>
											<img src='https://fox.mtbc.com/assets/images/talkrehab-logo.png' alt='CC Remote logo' width='150'/>
										</td>
									</tr>
								</table>
							</td>
						</tr>
						<tr>
							<td style='height:5px;width:100%;background-color:#00a9ef; '></td>
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
                                                       Please login to see the request for referral from CareCloud Remote
													</td>
                                                </tr>
                                                <tr>
                                                    <td>&nbsp;</td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <table style='width:250px;font-family: sans-serif !important;' cellpadding='0' cellspacing='0'>
                                                            <tr>
                                                                <td style='text-align: center;background-color:#00a9ef; border:1px solid #00a9ef; vertical-align:middle; line-height:normal; padding:5px 15px 5px 15px;'>
                                                                    <a style='color:#fff; font-size:16px;text-decoration:none; outline:none;background-color:#00a9ef;' target='_blank' href='" + link + @"'>Login To CC Remote</a>
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
                                                        <strong>Regards</strong><br>
                                                        CareCloud Remote.                                                             
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
                        }

                        //Helper.Email("noreply@mtbc.com", requestSendEmailModel.EmailAddress, requestSendEmailModel.Subject, _body, null, _bccList, new List<string>() { attachmentPath });
                        //bool emailStatus = Helper.Email(requestSendEmailModel.EmailAddress, requestSendEmailModel.Subject, _body, Profile, requestSendEmailModel.WorkId, null, _bccList, new List<string>() { attachmentPath });

                        bool emailStatus = false;
                        if (Profile.isTalkRehab)
                        {
                            emailStatus = Helper.Email(requestSendEmailModel.EmailAddress, requestSendEmailModel.Subject, _body, Profile, requestSendEmailModel.WorkId, null, _bccList, new List<string>() { attachmentPath }, "NOREPLY@CARECLOUD.COM");
                        }
                        else
                        {
                            emailStatus = Helper.Email(requestSendEmailModel.EmailAddress, requestSendEmailModel.Subject, _body, Profile, requestSendEmailModel.WorkId, null, _bccList, new List<string>() { attachmentPath });
                        }

                        Helper.TokenTaskCancellationExceptionLog("RequestForOrder: In Function Queue Repository || Start Time of Finding WORK ID " + Helper.GetCurrentDate().ToLocalTime());
                       
                        var queueResult = _QueueRepository.GetFirst(s => s.WORK_ID == requestSendEmailModel.WorkId && s.DELETED == false);
                        Helper.TokenTaskCancellationExceptionLog("RequestForOrder: In Function Queue Repository || End Time of Finding WORK ID " + Helper.GetCurrentDate().ToLocalTime());
                        if (queueResult != null && emailStatus == true)
                        {
                            Helper.TokenTaskCancellationExceptionLog("RequestForOrder: In Function Queue Repository || Start Time of Saving Email Address Against the WORK ID " + Helper.GetCurrentDate().ToLocalTime());
                            queueResult.REFERRAL_EMAIL_SENT_TO = requestSendEmailModel.EmailAddress;
                            _QueueRepository.Update(queueResult);
                            _QueueRepository.Save();
                            Helper.TokenTaskCancellationExceptionLog("RequestForOrder: In Function Queue Repository || End Time of Saving Email Address Against the WORK ID " + Helper.GetCurrentDate().ToLocalTime());
                        }

                        string filePath = responseHTMLToPDF?.FilePath + responseHTMLToPDF?.FileName;
                        int numberOfPages = getNumberOfPagesOfPDF(filePath);
                        //string imagesPath = HttpContext.Current.Server.MapPath("~/" + ImgDirPath);
                        //SavePdfToImages(filePath, imagesPath, requestSendEmailModel.WorkId, numberOfPages, "Email", requestSendEmailModel.EmailAddress, Profile.UserName);
                       
                        SavePdfToImages(filePath, config, requestSendEmailModel.WorkId, numberOfPages, "Email", requestSendEmailModel.EmailAddress, Profile.UserName, requestSendEmailModel._isFromIndexInfo);

                        return new ResponseModel() { Message = "Email sent successfully, our admission team is processing your referral", ErrorMessage = "", Success = true };
                    }
                    else
                    {
                        //return new ResponseModel() { Message = "Email sent successfully, our admission team is processing your referral", ErrorMessage = responseHTMLToPDF?.ErrorMessage, Success = false };
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
                //TO DO Log exception here
                //throw exception;
                //return new ResponseModel() { Message = "Email sent successfully, our admission team is processing your referral.", ErrorMessage = exception.ToString(), Success = false };
                return new ResponseModel() { Message = "We encountered an error while processing your request.", ErrorMessage = exception.ToString(), Success = false };
            }
        }
        public ResponseModel SendFAX(RequestSendFAXModel requestSendFAXModel, UserProfile Profile)
        {
            try
            {
                string htmlstring = "";
                var config = Helper.GetServiceConfiguration(Profile.PracticeCode);
                if (config.PRACTICE_CODE != null
                    && !string.IsNullOrWhiteSpace(config.ORIGINAL_FILES_PATH_DB) && !string.IsNullOrWhiteSpace(config.ORIGINAL_FILES_PATH_SERVER)
                    && !string.IsNullOrWhiteSpace(config.IMAGES_PATH_DB) && !string.IsNullOrWhiteSpace(config.IMAGES_PATH_SERVER))
                {
                    ResponseHTMLToPDF responseHTMLToPDF = HTMLToPDF(config, requestSendFAXModel.AttachmentHTML, requestSendFAXModel.FileName, "fax");

                    if (responseHTMLToPDF != null && (responseHTMLToPDF?.Success ?? false))
                    {
                        //var resultfax = _IFaxService.SendFax(new string[] { requestSendFAXModel.ReceipientFaxNumber }, new string[] { "" }, null, responseHTMLToPDF.FileName, responseHTMLToPDF.FilePath, requestSendFAXModel.Subject, false, Profile);

                        //if (resultfax == "failed")
                        //{
                        //    htmlstring = "<html><body><h2>Delivery Report</h2><p>Subject:" + requestSendFAXModel.Subject + "</p><p>From:" + requestSendFAXModel.SenderName + "</p><p>To:" + requestSendFAXModel.ReceipientFaxNumber + "</p><p>Sent:" + DateTime.Now + "</p><br/><div style='padding:10px;background-color:#ff9999;width: 50%;'><p>Delivery report for:" + requestSendFAXModel.ReceipientFaxNumber + "</p><p>Failed:</p><p>Message failed to deliver </p></div></body></html>";
                        //}
                        //else
                        //{
                        //    htmlstring = "<html><body><h2>Delivery Report</h2><p>Subject:" + requestSendFAXModel.Subject + "</p><p>From:" + requestSendFAXModel.SenderName + "</p><p>To:" + requestSendFAXModel.ReceipientFaxNumber + "</p><p>Sent:" + DateTime.Now + "</p><br/><div style='padding:10px;width: 50%;'><p>Delivery report for:" + requestSendFAXModel.ReceipientFaxNumber + "</p><p>Delivered Successfully:</p><p>Message delivered to recipient. </p></div></body></html>";
                        //}
                        //hl
                        //ResponseHTMLToPDF responseHTMLToPDF2 = HTMLToPDF2(config, htmlstring, "tempdfdelivery");
                       
                        //string deliveryfilePath = responseHTMLToPDF2?.FilePath + responseHTMLToPDF2?.FileName;
                        string filePath = responseHTMLToPDF?.FilePath + responseHTMLToPDF?.FileName;
                        int numberOfPages = getNumberOfPagesOfPDF(filePath);
                        //string imagesPath = HttpContext.Current.Server.MapPath("~/" + ImgDirPath);
                        //SavePdfToImages(filePath, imagesPath, requestSendFAXModel.WorkId, numberOfPages, "Fax", requestSendFAXModel.ReceipientFaxNumber, Profile.UserName);

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
                        //return new ResponseModel() { Message = "We encountered an error while processing your request.", ErrorMessage = responseHTMLToPDF?.ErrorMessage, Success = false };
                    }
                }
                else
                {
                    return new ResponseModel() { Message = "Fax could not be sent.", ErrorMessage = "DB configuration for file paths not found. See service configuration.", Success = false };
                }
            }
            catch (Exception exception)
            {
                //TO DO Log exception here
                //throw exception;
                return new ResponseModel() { Message = "Fax sent successfully, our admission team is processing your referral.", ErrorMessage = exception.ToString(), Success = false };
            }
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

                if (type == "fax")
                {
                    if (!EntityHelper.isTalkRehab)
                    {
                        PdfTextSection text = new PdfTextSection(10, 10, "Please sign and return to FOX at +1 (800) 597 - 0848 or email admit@foxrehab.org",
                                           new Font("Arial", 10));

                        // footer settings
                        converter.Options.DisplayFooter = true;
                        converter.Footer.Height = 50;
                        converter.Footer.Add(text);
                    }
                }

                PdfDocument doc = converter.ConvertHtmlString(htmlDoc.DocumentNode.OuterHtml);

                //string pdfPath = HttpContext.Current.Server.MapPath("~/" + @"FoxDocumentDirectory\RequestForOrderPDF\");
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




            //SautinSoft.PdfMetamorphosis p = new SautinSoft.PdfMetamorphosis();

            ////p.Serial = "10261942764";//development
            //p.Serial = "10262870570";//server//10261435399

            //p.PageSettings.Orientation = SautinSoft.PdfMetamorphosis.PageSetting.Orientations.Portrait;
            //p.PageSettings.Size.B5Iso();
            //// Specify header in HTML format.
            ////p.PageSettings.Header.Html("<div style='margin:10px; text-align:center;'><b>Email Body</b></div>");

            //// Specify page numbers
            ////p.PageSettings.Numbering.Text = "Email Body - Page {page} of {numpages}";
            //p.PageSettings.Numbering.PosX.Mm = p.PageSettings.Size.Width.Mm / (float)2.5;
            //p.PageSettings.Numbering.PosY.Mm = p.PageSettings.Size.Height.Mm - 10;

            //if (p != null)
            //{
            //    //To Do Change Path
            //    //string pdfPath = @"E:\TempDocuments\";
            //    string pdfPath = HttpContext.Current.Server.MapPath("~/" + @"FoxDocumentDirectory\RequestForOrderPDF\");
            //    if (!Directory.Exists(pdfPath))
            //    {
            //        Directory.CreateDirectory(pdfPath);
            //    }
            //    string filename = fileName + DateTime.Now.Ticks + ".pdf";
            //    string pdfFilePath = pdfPath + filename;

            //    if (p.HtmlToPdfConvertStringToFile(htmlString, pdfFilePath) == 0)
            //    {
            //        //return pdfFilePath;
            //        return new ResponseHTMLToPDF() { FileName = filename, FilePath = pdfPath, Success = true, ErrorMessage = "" };
            //    }
            //    else
            //    {
            //        var ex = p.TraceSettings.ExceptionList.Count > 0 ? p.TraceSettings.ExceptionList[0] : null;
            //        var msg = ex != null ? ex.Message + Environment.NewLine + ex.StackTrace : "An error occured during converting HTML to PDF!";
            //        //return "";
            //        return new ResponseHTMLToPDF() { FileName = "", FilePath = "", Success = false, ErrorMessage = ex.ToString() };
            //    }
            //}
            ////return "";
            //return new ResponseHTMLToPDF() { FileName = "", FilePath = "", Success = false, ErrorMessage = "" };
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


        private void AddFilesToDatabase(string filePath, long workId, string logoPath, bool _isFromindexInfo)
        {
            try
            {
                //OriginalQueueFiles originalQueueFiles1 = _OriginalQueueFiles.GetFirst(t => t.WORK_ID == workId && !t.deleted && t.FILE_PATH1.Equals(filePath) && t.FILE_PATH.Equals(logoPath));

                //if (originalQueueFiles1 == null || _isFromindexInfo)
                //{
                //    //If Work Order files is deleted
                //    originalQueueFiles1 = _OriginalQueueFiles.Get(t => t.WORK_ID == workId && t.deleted && t.FILE_PATH1.Equals(filePath) && t.FILE_PATH.Equals(logoPath));
                //    if (originalQueueFiles1 == null)
                //    {
                //        originalQueueFiles1 = new OriginalQueueFiles();

                //        originalQueueFiles1.FILE_ID = Helper.getMaximumId("FOXREHAB_FILE_ID");
                //        originalQueueFiles1.WORK_ID = workId;
                //        originalQueueFiles1.UNIQUE_ID = workId.ToString();
                //        originalQueueFiles1.FILE_PATH1 = filePath;
                //        originalQueueFiles1.FILE_PATH = logoPath;
                //        originalQueueFiles1.deleted = false;

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
        //private void SavePdfToImages(string PdfPath, string ImageFilePath, long workId, int noOfPages, string sorcetype, string sorceName, string userName)
        //{

        //    if (!Directory.Exists(ImageFilePath))
        //    {
        //        Directory.CreateDirectory(ImageFilePath);
        //    }
        //    if (System.IO.File.Exists(PdfPath))
        //    {
        //        for (int i = 0; i < noOfPages; i++)
        //        {
        //            System.Drawing.Image img;
        //            PdfFocus f = new PdfFocus();
        //            f.Serial = "10261435399";
        //            f.OpenPdf(PdfPath);

        //            if (f.PageCount > 0)
        //            {
        //                //Save all PDF pages to jpeg images
        //                f.ImageOptions.Dpi = 120;
        //                f.ImageOptions.ImageFormat = ImageFormat.Jpeg;

        //                var image = f.ToImage(i + 1);
        //                //Next manipulate with Jpeg in memory or save to HDD, open in a viewer
        //                using (var ms = new MemoryStream(image))
        //                {
        //                    img = System.Drawing.Image.FromStream(ms);
        //                    img.Save(ImageFilePath + "\\" + workId + "_" + i + ".jpg", ImageFormat.Jpeg);
        //                    Bitmap bmp = new Bitmap(img);
        //                    ConvertPDFToImages ctp = new ConvertPDFToImages();
        //                    ctp.SaveWithNewDimention(bmp, 115, 150, 100, ImageFilePath + "\\Logo_" + workId + "_" + i + ".jpg");
        //                }
        //            }
        //            //End
        //            //string ImgDirPath = "FoxDocumentDirectory\\Fox\\Images";
        //            var imgPath = ImgDirPath + "\\" + workId + "_" + i + ".jpg";
        //            var logoImgPath = ImgDirPath + "\\Logo_" + workId + "_" + i + ".jpg";
        //            AddFilesToDatabase(imgPath, workId, logoImgPath);
        //        }
        //        AddToDatabase(PdfPath, noOfPages, workId, sorcetype, sorceName, userName);
        //    }
        //}
        //New Thread Implementation
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
                        if (_isFromIndexInfo)
                        {
                            var randomString = random.Next();
                            imgPath = config.IMAGES_PATH_DB + "\\" + deliveryReportId + "_" + i + "_" + randomString + ".jpg";
                            imgPathServer = config.IMAGES_PATH_SERVER + "\\" + deliveryReportId + "_" + i + "_" + randomString + ".jpg";
                        }
                        else
                        {
                            imgPath = config.IMAGES_PATH_DB + "\\" + deliveryReportId + "_" + i + ".jpg";
                            imgPathServer = config.IMAGES_PATH_SERVER + "\\" + deliveryReportId + "_" + i + ".jpg";
                        }
                        if (_isFromIndexInfo)
                        {
                            var randomString = random.Next();
                            logoImgPath = config.IMAGES_PATH_DB + "\\Logo_" + deliveryReportId + "_" + i + "_" + randomString + ".jpg";
                            logoImgPathServer = config.IMAGES_PATH_SERVER + "\\Logo_" + deliveryReportId + "_" + i + "_" + randomString + ".jpg";
                        }
                        else
                        {
                            logoImgPath = config.IMAGES_PATH_DB + "\\Logo_" + deliveryReportId + "_" + i + ".jpg";
                            logoImgPathServer = config.IMAGES_PATH_SERVER + "\\Logo_" + deliveryReportId + "_" + i + ".jpg";
                        }
                    }
                    else
                    {
                        if (_isFromIndexInfo)
                        {
                            var randomString = random.Next();
                            imgPath = config.IMAGES_PATH_DB + "\\" + workId + "_" + i + "_" + randomString + ".jpg";
                            imgPathServer = config.IMAGES_PATH_SERVER + "\\" + workId + "_" + i + "_" + randomString + ".jpg";
                        }
                        else if (pageCounter != 0 && _isFromIndexInfo == false && sorcetype.ToLower() == "fax")
                        {
                            imgPath = config.IMAGES_PATH_DB + "\\" + workId + "_" + pageCounter + ".jpg";
                            imgPathServer = config.IMAGES_PATH_SERVER + "\\" + workId + "_" + pageCounter + ".jpg";
                        }
                        else
                        {
                            imgPath = config.IMAGES_PATH_DB + "\\" + workId + "_" + i + ".jpg";
                            imgPathServer = config.IMAGES_PATH_SERVER + "\\" + workId + "_" + i + ".jpg";
                        }
                        if (_isFromIndexInfo)
                        {
                            var randomString = random.Next();
                            logoImgPath = config.IMAGES_PATH_DB + "\\Logo_" + workId + "_" + i + "_" + randomString + ".jpg";
                            logoImgPathServer = config.IMAGES_PATH_SERVER + "\\Logo_" + workId + "_" + i + "_" + randomString + ".jpg";
                        }
                        else if (pageCounter != 0 && _isFromIndexInfo == false && sorcetype.ToLower() == "fax")
                        {
                            logoImgPath = config.IMAGES_PATH_DB + "\\Logo_" + workId + "_" + pageCounter + ".jpg";
                            logoImgPathServer = config.IMAGES_PATH_SERVER + "\\Logo_" + workId + "_" + pageCounter + ".jpg";
                        }
                        else
                        {
                            logoImgPath = config.IMAGES_PATH_DB + "\\Logo_" + workId + "_" + i + ".jpg";
                            logoImgPathServer = config.IMAGES_PATH_SERVER + "\\Logo_" + workId + "_" + i + ".jpg";
                        }
                    }

                    Thread myThread = new Thread(() => this.newThreadImplementaion(ref threadCounter, PdfPath, i, imgPathServer, logoImgPathServer));
                    myThread.Start();
                    threadsList.Add(myThread);
                    //End
                    //string ImgDirPath = "FoxDocumentDirectory\\Fox\\Images";

                    //if (sorcetype.Split(':')?[0] == "DR")
                    //{
                    //     imgPath = config.IMAGES_PATH_DB + "\\" + deliveryReportId + "_" + i + ".jpg";
                    //     logoImgPath = config.IMAGES_PATH_DB + "\\Logo_" + deliveryReportId + "_" + i + ".jpg";
                    //}
                    //else
                    //{
                    //    imgPath = config.IMAGES_PATH_DB + "\\" + workId + "_" + i + ".jpg";
                    //    logoImgPath = config.IMAGES_PATH_DB + "\\Logo_" + workId + "_" + i + ".jpg";
                    //}
                    AddFilesToDatabase(imgPath, workId, logoImgPath, _isFromIndexInfo);
                }
                while (noOfPages > threadCounter.Count)
                {
                    //loop untill record complete
                }

                foreach (var thread in threadsList)
                {
                    thread.Abort();
                }
                //if (_isFromIndexInfo)
                //{
                //AddToDatabaseForRFO(workId, userName, _isFromIndexInfo);
                //}
                //else
                //{
                noOfPages = _OriginalQueueFiles.GetMany(t => t.WORK_ID == workId && !t.deleted)?.Count() ?? 0;
                AddToDatabase(PdfPath, noOfPages, workId, sorcetype, sorceName, userName, config.PRACTICE_CODE, _isFromIndexInfo);
                //}
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
        private int getNumberOfPagesOfPDF(string PdfPath)
        {
            iTextSharp.text.pdf.PdfReader pdfReader = new iTextSharp.text.pdf.PdfReader(PdfPath);
            return pdfReader.NumberOfPages;
        }
        private void AddToDatabase(string filePath, int noOfPages, long workId, string sorcetype, string sorceName, string userName,long? practice_code, bool fromindexinf)
        {
            try
            {
                //OriginalQueue originalQueue = _QueueRepository.Get(t => t.WORK_ID == workId && !t.DELETED);
                //if (originalQueue != null)
                //{
                //    //TO DO
                //    //originalQueue.SORCE_TYPE = sorcetype;
                //    //originalQueue.SORCE_NAME = sorceName;
                //    //originalQueue.WORK_STATUS = "Indexed";
                //    originalQueue.TOTAL_PAGES = noOfPages;
                //    //originalQueue.FILE_PATH = filePath;
                //    originalQueue.FAX_ID = "";

                //    originalQueue.MODIFIED_BY = userName;
                //    originalQueue.MODIFIED_DATE = DateTime.Now;

                //    _QueueRepository.Update(originalQueue);
                //    _QueueRepository.Save();
                //}

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
        ///<Summary>Updating number of pages in FOX_TBL_Work_Queue</Summary>
        ///<Params>workId, userName</Params>
        ///<Authour>Abdur Rafay</Authour>
        private void AddToDatabaseForRFO(long workId, string userName)
        {
            OriginalQueue originalQueue = _QueueRepository.Get(t => t.WORK_ID == workId && !t.DELETED);
            if (originalQueue != null)
            {
                if (originalQueue.TOTAL_PAGES == null)
                {
                    originalQueue.TOTAL_PAGES = 1;
                }
                else
                {
                    originalQueue.TOTAL_PAGES = originalQueue.TOTAL_PAGES + 1;
                }

                originalQueue.MODIFIED_BY = userName;
                originalQueue.MODIFIED_DATE = DateTime.Now;

                _QueueRepository.Update(originalQueue);
                _QueueRepository.Save();
            }
        }
        public bool VerifyWorkOrderByRecipient(string value)
        {
            try
            {
                //string workIdStr = StringCipher.Decrypt(value);
                string workIdStr = value;
                long workId = long.Parse(workIdStr);
                OriginalQueue originalQueue = _QueueRepository.Get(t => t.WORK_ID == workId && !t.DELETED);
                if (originalQueue != null)
                {
                    originalQueue.IS_VERIFIED_BY_RECIPIENT = true;

                    originalQueue.MODIFIED_BY = "ExternalUser";
                    originalQueue.MODIFIED_DATE = DateTime.Now;

                    _QueueRepository.Update(originalQueue);
                    _QueueRepository.Save();
                    return true;
                }
                return false;
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }
        public ResponseModel DeleteWorkOrder(RequestDeleteWorkOrder requestDeleteWorkOrder, UserProfile Profile)
        {
            try
            {
                OriginalQueue originalQueue = _QueueRepository.Get(t => t.WORK_ID == requestDeleteWorkOrder?.WorkId && !t.DELETED);

                if (originalQueue != null)
                {
                    originalQueue.DELETED = true;
                    originalQueue.MODIFIED_BY = Profile.UserName;
                    originalQueue.MODIFIED_DATE = DateTime.Now;

                    _QueueRepository.Update(originalQueue);
                    _QueueRepository.Save();
                    return new ResponseModel() { Message = "Delete work order successfully.", ErrorMessage = "", Success = true };
                }
                else
                    return new ResponseModel() { Message = "Work order not found.", ErrorMessage = "", Success = true };
            }
            catch (Exception exception)
            {
                //throw exception;
                return new ResponseModel() { Message = "We encountered an error while processing your request.", ErrorMessage = exception.ToString(), Success = false };
            }
        }

public ResponseModel DownloadPdf(RequestDownloadPdfModel requestDownloadPdfModel, UserProfile Profile)
{
    try
    {
        var config = Helper.GetServiceConfiguration(Profile.PracticeCode);
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
                //TO DO Log exception here
                //throw exception;
                return new ResponseModel() { Message = "We encountered an error while processing your request.", ErrorMessage = exception.ToString(), Success = false };
            }
        }

        public ResponseModel AddDocument_SignOrder(ReqAddDocument_SignOrder reqAddDocument_SignOrder, UserProfile Profile)
        {
            try
            {
                var config = Helper.GetServiceConfiguration(Profile.PracticeCode);
                if (config.PRACTICE_CODE != null
                    && !string.IsNullOrWhiteSpace(config.ORIGINAL_FILES_PATH_DB) && !string.IsNullOrWhiteSpace(config.ORIGINAL_FILES_PATH_SERVER)
                    && !string.IsNullOrWhiteSpace(config.IMAGES_PATH_DB) && !string.IsNullOrWhiteSpace(config.IMAGES_PATH_SERVER))
                {
                    ResponseHTMLToPDF responseHTMLToPDF = HTMLToPDF(config, reqAddDocument_SignOrder.AttachmentHTML, reqAddDocument_SignOrder.FileName.Replace(' ', '_'), "");
                    if (responseHTMLToPDF != null && (responseHTMLToPDF?.Success ?? false))
                    {
                        string filePath = responseHTMLToPDF?.FilePath + responseHTMLToPDF?.FileName;
                        int numberOfPages = getNumberOfPagesOfPDF(filePath);
                        //string imagesPath = HttpContext.Current.Server.MapPath("~/" + ImgDirPath);
                        //SavePdfToImages(filePath, imagesPath, reqAddDocument_SignOrder.WorkId, numberOfPages, "Email", Profile.UserEmailAddress, Profile.UserName);
                       
                        SavePdfToImages(filePath, config, reqAddDocument_SignOrder.WorkId, numberOfPages, "Email", Profile.UserEmailAddress, Profile.UserName, false);
                       
                        return new ResponseModel() { Message = "Add document in sign order successfully.", ErrorMessage = "", Success = true };
                    }
                    else
                    {
                        return new ResponseModel() { Message = "Add document in sign order successfully.", ErrorMessage = responseHTMLToPDF?.ErrorMessage, Success = false };
                        //return new ResponseModel() { Message = "We encountered an error while processing your request.", ErrorMessage = responseHTMLToPDF?.ErrorMessage, Success = false };
                    }
                }
                else
                {
                    return new ResponseModel() { Message = "Add document in sign order unsuccessfull.", ErrorMessage = "DB configuration for file paths not found. See service configuration.", Success = false };
                }
            }
            catch (Exception exception)
            {
                //TO DO Log exception here
                //throw exception;
                return new ResponseModel() { Message = "Email sent successfully.", ErrorMessage = exception.ToString(), Success = false };
                //return new ResponseModel() { Message = "We encountered an error while processing your request.", ErrorMessage = exception.ToString(), Success = false };
            }
        }

        public ReferralSource GetUserReferralSource(string email, long userId)
        {
            try
            {
                ReferralSource refSrc = null;

                //Get UserReferralSource
                var usr = _UserRepository.GetByID(userId);
                if (usr != null)
                {

                    if (usr.USER_TYPE.ToLower().Contains("external") && (usr.IS_APPROVED ?? false)
                        && usr.ROLE_ID.HasValue && usr.ROLE_ID == 108 && usr.USR_REFERRAL_SOURCE_ID != null && usr.USR_REFERRAL_SOURCE_ID != 0)
                    {
                        refSrc = _InsertUpdateOrderingSourceRepository.GetFirst(e => e.SOURCE_ID == usr.USR_REFERRAL_SOURCE_ID);
                        if (refSrc != null)
                        {
                            if (refSrc.ACO_ID != null)
                            {
                                var acoData = _fox_tbl_identifier.GetFirst(e => e.IDENTIFIER_ID == refSrc.ACO_ID && !e.DELETED);
                                if (acoData != null)
                                {
                                    System.Globalization.TextInfo strConverter = new System.Globalization.CultureInfo("en-US", false).TextInfo;
                                    refSrc.ACO_NAME = strConverter.ToTitleCase(acoData.NAME?.ToLower() ?? "");
                                }
                            }

                            if (refSrc.PRACTICE_ORGANIZATION_ID != null)
                            {
                                var poData = _fox_tbl_practice_organization.GetFirst(e => e.PRACTICE_ORGANIZATION_ID == refSrc.PRACTICE_ORGANIZATION_ID && !e.DELETED);
                                if (poData != null)
                                {
                                    System.Globalization.TextInfo strConverter = new System.Globalization.CultureInfo("en-US", false).TextInfo;
                                    refSrc.PRACTICE_ORGANIZATION_NAME = strConverter.ToTitleCase(poData.NAME?.ToLower() ?? "");
                                }
                            }
                        }
                    }
                }

                return refSrc;
            }
            catch (Exception ex) {
                throw ex;
            }
        }
        public void AddHtmlToDB(long WorkId, string htmll, string userName)
        {
            var work_order = _QueueRepository.GetFirst(t => t.WORK_ID == WorkId && t.DELETED == false);
            var practiceCode = work_order.PRACTICE_CODE.Value.ToString();
            OriginalQueueFiles originalQueueFiles = _OriginalQueueFiles.GetFirst(t => t.UNIQUE_ID == work_order.UNIQUE_ID && !t.deleted);

            var patient = _PatientRepository.GetFirst(x => x.Patient_Account == work_order.PATIENT_ACCOUNT && x.Practice_Code == work_order.PRACTICE_CODE && x.DELETED == false);
            var sourceDetail = _InsertSourceAddRepository.GetFirst(t => !t.DELETED && t.PRACTICE_CODE == work_order.PRACTICE_CODE && t.WORK_ID == work_order.WORK_ID && work_order.WORK_ID != 0);
            var documentType = _foxdocumenttypeRepository.GetFirst(t => t.DOCUMENT_TYPE_ID == sourceDetail.DOCUMENT_TYPE).NAME ?? "";
            var ORS = _InsertUpdateOrderingSourceRepository.GetFirst(t => t.SOURCE_ID == sourceDetail.SENDER_ID);
            var address = _PatientAddressRepository.GetMany(t => t.PATIENT_ACCOUNT == work_order.PATIENT_ACCOUNT && t.ADDRESS_TYPE == "Home Address" && !(t.DELETED ?? false)).OrderByDescending(t => t.MODIFIED_DATE).FirstOrDefault();
            var diagnosis_string = "";
            var diagnosis = _InsertDiagnosisRepository.GetMany(t => t.DELETED == false && t.WORK_ID == WorkId);
            var specialty = _InsertProceuresRepository.GetFirst(t => t.DELETED == false && t.WORK_ID == WorkId);
            if (diagnosis != null && diagnosis.Count > 0)
            {
                foreach (var item in diagnosis)
                {
                    diagnosis_string = diagnosis_string + item.DIAG_CODE + " " + item.DIAG_DESC + " <br />";
                }
            }
            //var speciality = _speciality.GetFirst(t=> t.SPECIALITY_ID == work_order.SPECIALITY_PROGRAM);
            var pri_insurance = "";
            var curr_insurances = _PatientInsuranceRepository.GetMany(x => x.Patient_Account == work_order.PATIENT_ACCOUNT && x.Pri_Sec_Oth_Type == "P" && (x.Deleted ?? false) == false && (string.IsNullOrEmpty(x.FOX_INSURANCE_STATUS) || x.FOX_INSURANCE_STATUS == "C")).OrderByDescending(t => t.Modified_Date).FirstOrDefault(); //Current
            var ins_name = "";
            if (curr_insurances != null)
            {
                ins_name = _foxInsurancePayersRepository.GetFirst(t => t.DELETED == false && t.FOX_TBL_INSURANCE_ID == curr_insurances.FOX_TBL_INSURANCE_ID).INSURANCE_NAME ?? "";
            }

            if (!String.IsNullOrWhiteSpace(ins_name))
            {
                pri_insurance = ins_name;
            }
            var file_name = patient.Last_Name + "_" + documentType;
            var Sender = _User.GetFirst(T => T.USER_NAME == sourceDetail.CREATED_BY);
            if (Sender == null && sourceDetail != null && !string.IsNullOrEmpty(sourceDetail.CREATED_BY) && sourceDetail.CREATED_BY.Equals("FOX TEAM"))
            {
                Sender = _User.GetFirst(T => T.USER_NAME == userName);
            }

            var fcClass = new FinancialClass();

            if(work_order != null && work_order.PATIENT_ACCOUNT != null && work_order.PATIENT_ACCOUNT != 0)
            {
                var pat = _FoxTblPatientRepository.GetFirst(x => x.Patient_Account == work_order.PATIENT_ACCOUNT);
                if(pat != null && pat.FINANCIAL_CLASS_ID != null)
                {
                    fcClass = _financialClassRepository.GetFirst(x => x.FINANCIAL_CLASS_ID == pat.FINANCIAL_CLASS_ID);
                    if(fcClass != null && !string.IsNullOrEmpty(fcClass.NAME) && fcClass.NAME.ToLower().Contains("sa- special account"))
                    {
                        work_order.is_strategic_account = true;
                    }
                }
            }

            var discipline = "";
            if (sourceDetail != null)
            {
                if (!string.IsNullOrEmpty(sourceDetail?.DEPARTMENT_ID))
                {
                    if (sourceDetail.DEPARTMENT_ID.Contains("1"))
                    {
                        discipline = discipline + " Occupational Therapy (OT), ";
                    }
                    if (sourceDetail.DEPARTMENT_ID.Contains("2"))
                    {
                        discipline = discipline + " Physical Therapy (PT), ";
                    }
                    if (sourceDetail.DEPARTMENT_ID.Contains("3"))
                    {
                        discipline = discipline + " Speech Therapy (ST), ";
                    }
                    if (sourceDetail.DEPARTMENT_ID == "4")
                    {
                        discipline = discipline + "Physical/Occupational/Speech Therapy(PT/OT/ST)";
                    }
                    if (sourceDetail.DEPARTMENT_ID == "5")
                    {
                        discipline = discipline + "Physical/Occupational Therapy(PT/OT)";
                    }
                    if (sourceDetail.DEPARTMENT_ID == "6")
                    {
                        discipline = discipline + "Physical/Speech Therapy(PT/ST)";
                    }
                    if (sourceDetail.DEPARTMENT_ID == "7")
                    {
                        discipline = discipline + "Occupational/Speech Therapy(OT/ST)";
                    }
                    if (sourceDetail.DEPARTMENT_ID.Contains("8"))
                    {
                        discipline = discipline + " Unknown, ";
                    }
                    if (sourceDetail.DEPARTMENT_ID.Contains("9"))
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
            //qr.SignPath = GetProfile().SIGNATURE_PATH;
            qr.AbsolutePath = System.Web.HttpContext.Current.Server.MapPath("~/" + AppConfiguration.QRCodeTempPath);
            qr.WORK_ID = WorkId;
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

                if (work_order.IS_VERBAL_ORDER == false && work_order.is_strategic_account == true)
                {
                    var VerbalOrder = htmldoc.DocumentNode.SelectSingleNode("//span[@id='VerbalOrder']");
                    VerbalOrder.Remove();
                }
                if (work_order.IS_VERBAL_ORDER == null || work_order.IS_VERBAL_ORDER == false)
                {
                    var Verbal = htmldoc.DocumentNode.SelectSingleNode("//span[@id='VERBAL']");
                    Verbal.Remove();
                }
                if (work_order.is_strategic_account)
                {
                    var Insurance = htmldoc.DocumentNode.SelectSingleNode("//span[@id='PRIMARY_INSURANCE']");
                    Insurance.Remove();
                }
                if (work_order.IS_EVALUATE_TREAT == null || work_order.IS_EVALUATE_TREAT == false)
                {
                    var evaluate = htmldoc.DocumentNode.SelectSingleNode("//span[@id='EVALUATE_TREAT']");
                    evaluate.Remove();
                }
                if (work_order.IS_EMERGENCY_ORDER == false)
                {
                    var URGENT = htmldoc.DocumentNode.SelectSingleNode("//span[@id='URGENT']");
                    URGENT.Remove();
                }
                body = htmldoc.DocumentNode.OuterHtml;

                body = body.Replace("[[PATIENT_NAME]]", patient.Last_Name + ", " + patient.First_Name + " (" + patient.Gender + ")");
                body = body.Replace("[[PATIENT_DOB]]", patient.Date_Of_Birth.ToString() ?? "");
                body = body.Replace("[[PATIENT_MRN]]", patient.Chart_Id ?? "");
                if (address != null)
                {
                    body = body.Replace("[[PATIENT_HOME_ADDRESS]]", address.ADDRESS ?? "");
                    TextInfo ti = CultureInfo.CurrentCulture.TextInfo;
                    body = body.Replace("[[PATIENT_HOME_ADDRESS_2]]", ti.ToTitleCase(address.CITY) + ", " + address.STATE + " " + address.ZIP);

                }
                else
                {
                    body = body.Replace("[[PATIENT_HOME_ADDRESS]]", "");
                    body = body.Replace("[[PATIENT_HOME_ADDRESS_2]]", "");
                }
                body = body.Replace("[[PATIENT_PRI_INS]]", pri_insurance ?? "");


                if (qrCode != null)
                {
                    body = body.Replace("[[QRCode]]", qrCode.ENCODED_IMAGE_BYTES ?? "");
                }
                body = body.Replace("[[DOCUMENT_TYPE]]", documentType ?? "");               
                if( ORS != null)
                {
                    body = body.Replace("[[ORS]]", ORS.LAST_NAME + ", " + ORS.FIRST_NAME ?? "");
                }
                body = body.Replace("[[SENDER]]", Sender == null ? "" : Sender.LAST_NAME + ", " + Sender.FIRST_NAME ?? "");
                body = body.Replace("[[TREATMENT_LOCATION]]", sourceDetail.FACILITY_NAME ?? "");


                body = body.Replace("[[Diagnosis_Information]]", diagnosis_string);

                body = body.Replace("[[discipline]]", discipline ?? "");

                if (specialty != null && !String.IsNullOrWhiteSpace(specialty.SPECIALITY_PROGRAM))
                {
                    body = body.Replace("[[specialty_program]]", specialty.SPECIALITY_PROGRAM ?? "");
                }
                else
                {
                    body = body.Replace("[[specialty_program]]", "");
                }

                if (work_order.IS_EMERGENCY_ORDER)
                {
                    body = body.Replace("[[is_urgent_Referral]]", "Yes");
                    body = body.Replace("[[reason_urgency]]", work_order.REASON_FOR_THE_URGENCY ?? "");

                }
                else
                {
                    body = body.Replace("[[is_urgent_Referral]]", "No");
                }

                if (work_order.IS_EVALUATE_TREAT != null && work_order.IS_EVALUATE_TREAT == true)
                {
                    body = body.Replace("[[HHH_NAME]]", work_order.HEALTH_NAME ?? "");
                    body = body.Replace("[[HHH_NUMBER]]", DataModels.HelperClasses.StringHelper.ApplyPhoneMask(work_order.HEALTH_NUMBER) ?? "");
                }

                if (work_order.is_strategic_account)
                {
                    var Title = "Home Health";
                    var Description = "Home Health Strategic Account";
                    body = body.Replace("[[Home_Health]]", Title ?? "");
                    body = body.Replace("[[Home_Health_Description]]", Description ?? "");
                }
                else
                {
                    body = body.Replace("[[Home_Health]]", "");
                    body = body.Replace("[[Home_Health_Description]]", "");
                }


                if (work_order.IS_VERBAL_ORDER != null && work_order.IS_VERBAL_ORDER == true)
                {
                    body = body.Replace("[[is_verbal_order]]", "Yes");
                    body = body.Replace("[[on_behalf_of]]", work_order.VO_ON_BEHALF_OF.ToString());
                    body = body.Replace("[[received_by]]", work_order.VO_RECIEVED_BY ?? "");
                    body = body.Replace("[[verbal_date]]", work_order.VO_DATE_TIME.Value.ToString("MM/dd/yyyy") ?? "");
                    body = body.Replace("[[verbal_time]]", work_order.VO_DATE_TIME.Value.ToString("hh:mm tt") ?? "");
                }
                else
                {
                    body = body.Replace("[[is_verbal_order]]", "No");
                }

                body = body.Replace("[[additional_notes]]", work_order.REASON_FOR_VISIT ?? "");
                var provider = _InsertUpdateOrderingSourceRepository.GetFirst(t => t.SOURCE_ID == sourceDetail.SENDER_ID);
                if (provider != null)
                {
                    body = body.Replace("[[provider_name]]", provider.LAST_NAME + ", " + provider.FIRST_NAME + " " + provider.REFERRAL_REGION);
                    body = body.Replace("[[provider_NPI]]", provider.NPI ?? "");
                    body = body.Replace("[[provider_phone]]", DataModels.HelperClasses.StringHelper.ApplyPhoneMask(provider.PHONE) ?? "");
                    body = body.Replace("[[provider_fax]]", DataModels.HelperClasses.StringHelper.ApplyPhoneMask(provider.FAX) ?? "");
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
            obj.WORK_ID = WorkId;
            obj.THERAPY_TREATMENT_REFERRAL_REQUEST_HTML = body;
            obj.CREATED_BY = userName;
            obj.CREATED_DATE = Helper.GetCurrentDate();
            _fox_tbl_TherapyTreatmentRequestForm.Insert(obj);
            _fox_tbl_TherapyTreatmentRequestForm.Save();

            //SqlParameter id = new SqlParameter("THERAPY_TREATMENT_REFERRAL_REQUEST_FORM_ID", Pid);
            //SqlParameter _workid = new SqlParameter("WORK_ID",  WorkId);
            //SqlParameter _html = new SqlParameter("THERAPY_TREATMENT_REFERRAL_REQUEST_HTML", html);
            //SqlParameter createdBy = new SqlParameter { ParameterName = "CREATED_BY", SqlDbType = SqlDbType.VarChar, Value = userName };
            //SqlParameter createdDate = new SqlParameter { ParameterName = "CREATED_DATE", SqlDbType = SqlDbType.DateTime, Value = Helper.GetCurrentDate() };

            //SpRepository<string>.GetListWithStoreProcedure(@"FOX_PROC_INSERT_THERAPY_TREATMENT_REFERRAL_REQUEST_FORM @THERAPY_TREATMENT_REFERRAL_REQUEST_FORM_ID, @WORK_ID, @THERAPY_TREATMENT_REFERRAL_REQUEST_HTML, @CREATED_BY, @CREATED_DATE",
            //    id, _workid, _html, createdBy, createdDate);
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
            obj.ENCODED_IMAGE_BYTES = src;
            return obj;
        }
    }
}