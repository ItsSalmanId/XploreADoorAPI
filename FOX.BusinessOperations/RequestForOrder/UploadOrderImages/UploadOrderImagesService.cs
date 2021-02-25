using FOX.BusinessOperations.CommonService;
using FOX.DataModels.Context;
using FOX.DataModels.GenericRepository;
using FOX.DataModels.Models.OriginalQueueModel;
using FOX.DataModels.Models.RequestForOrder.UploadOrderImages;
using FOX.DataModels.Models.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using FOX.BusinessOperations.CommonServices;
using SautinSoft;
using System.Drawing.Imaging;
using System.Drawing;
using FOX.DataModels.Models.SenderName;
using FOX.DataModels.Models.ServiceConfiguration;
using FOX.DataModels.Models.IndexInfo;
using FOX.DataModels.Models.RequestForOrder;
using System.Threading;
using System.Diagnostics;
using System.Data.SqlClient;
using System.Data;

namespace FOX.BusinessOperations.RequestForOrder.UploadOrderImages
{
    public class UploadOrderImagesService : IUploadOrderImagesService
    {
        private readonly DBContextQueue _QueueContext = new DBContextQueue();
        private readonly GenericRepository<OriginalQueue> _QueueRepository;
        private readonly GenericRepository<OriginalQueueFiles> _OriginalQueueFiles;
        private readonly GenericRepository<FOX_TBL_NOTES_HISTORY> _NotesRepository;
        private readonly DbContextIndexinfo _IndexinfoContext = new DbContextIndexinfo();
        private readonly GenericRepository<OriginalQueue> _InsertSourceAddRepository;

        private readonly DbContextSecurity security = new DbContextSecurity();
        private readonly GenericRepository<User> _UserRepository;

        private readonly DbContextCommon _DbContextCommon = new DbContextCommon();
        private readonly GenericRepository<FOX_TBL_SENDER_NAME> _FOX_TBL_SENDER_NAME;
        private readonly GenericRepository<FoxDocumentType> _foxdocumenttypeRepository;
        private static List<Thread> threadsList = new List<Thread>();
        public UploadOrderImagesService()
        {
            _NotesRepository = new GenericRepository<FOX_TBL_NOTES_HISTORY>(_IndexinfoContext);
            _QueueRepository = new GenericRepository<OriginalQueue>(_QueueContext);
            _OriginalQueueFiles = new GenericRepository<OriginalQueueFiles>(_QueueContext);
            _InsertSourceAddRepository = new GenericRepository<OriginalQueue>(_IndexinfoContext);
            _UserRepository = new GenericRepository<User>(security);
            _FOX_TBL_SENDER_NAME = new GenericRepository<FOX_TBL_SENDER_NAME>(_DbContextCommon);
            _foxdocumenttypeRepository = new GenericRepository<FoxDocumentType>(_IndexinfoContext);
        }

        public ResGetSourceDataModel GetSourceData(string email, string userId, long practiceCode, string userName)
        {
            try
            {
                ResGetSourceDataModel resGetSourceDataModel = new ResGetSourceDataModel();

                resGetSourceDataModel.SORCE_NAME = email;

                long _lg;
                if (long.TryParse(userId, out _lg))
                {
                    var usr = _UserRepository.GetByID(_lg);
                    if (usr != null)
                    {
                        resGetSourceDataModel.FOX_TBL_SENDER_TYPE_ID = usr.FOX_TBL_SENDER_TYPE_ID;
                        var senderName = _FOX_TBL_SENDER_NAME.GetFirst(t => t.SENDER_NAME_CODE.Equals(usr.USER_NAME));
                        if (senderName == null)
                        {
                            senderName = new FOX_TBL_SENDER_NAME();
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
                        //resGetSourceDataModel.FOX_TBL_SENDER_NAME_ID = senderName?.FOX_TBL_SENDER_NAME_ID;
                    }
                }
                resGetSourceDataModel.Message = "Get Source Data Successfully";
                resGetSourceDataModel.Success = true;
                resGetSourceDataModel.ErrorMessage = "";
                resGetSourceDataModel.FoxDocumentTypeList = _foxdocumenttypeRepository.GetMany(d => !d.DELETED).OrderBy(o => o.NAME).ToList();

                return resGetSourceDataModel;
            }
            catch (Exception exception)
            {
                return new ResGetSourceDataModel() { Message = "We encountered an error while processing your request.", ErrorMessage = exception.ToString(), Success = false };
            }
        }

        public ResSubmitUploadOrderImagesModel SubmitUploadOrderImages(ReqSubmitUploadOrderImagesModel reqSubmitUploadOrderImagesModel, UserProfile Profile)
        {
            //try
            //{
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
            originalQueue.SORCE_NAME = Profile.UserEmailAddress;
            originalQueue.SORCE_TYPE = "Email";

            originalQueue.WORK_STATUS = "Created";
            originalQueue.IS_VERIFIED_BY_RECIPIENT = false;

            originalQueue.PATIENT_ACCOUNT = reqSubmitUploadOrderImagesModel.PATIENT_ACCOUNT;
            originalQueue.FOX_TBL_SENDER_TYPE_ID = reqSubmitUploadOrderImagesModel.FOX_TBL_SENDER_TYPE_ID;
            originalQueue.FOX_TBL_SENDER_NAME_ID = reqSubmitUploadOrderImagesModel.FOX_TBL_SENDER_NAME_ID;
            originalQueue.SENDER_ID = reqSubmitUploadOrderImagesModel.SENDER_ID;
            originalQueue.DOCUMENT_TYPE = reqSubmitUploadOrderImagesModel.DOCUMENT_TYPE;
            originalQueue.DEPARTMENT_ID = reqSubmitUploadOrderImagesModel.DEPARTMENT_ID;
            originalQueue.FACILITY_NAME = reqSubmitUploadOrderImagesModel.FACILITY_NAME;
            originalQueue.FACILITY_ID = reqSubmitUploadOrderImagesModel.FACILITY_ID;
            originalQueue.IS_EMERGENCY_ORDER = reqSubmitUploadOrderImagesModel.IS_EMERGENCY_ORDER;
            originalQueue.RFO_Type = "Upload_Images";

            originalQueue.ASSIGNED_TO = null;
            originalQueue.ASSIGNED_BY = null;
            originalQueue.ASSIGNED_DATE = null;

            _QueueRepository.Insert(originalQueue);
            _QueueRepository.Save();

            Helper.TokenTaskCancellationExceptionLog("UploadOrderImages: In Function  SubmitUploadOrderImages > GenerateAndSaveImagesOfUploadedFiles || Start Time of Function GenerateAndSaveImagesOfUploadedFiles" + Helper.GetCurrentDate().ToLocalTime());
            GenerateAndSaveImagesOfUploadedFiles(workId, reqSubmitUploadOrderImagesModel.FileNameList, Profile);
            Helper.TokenTaskCancellationExceptionLog("UploadOrderImages: In Function  SubmitUploadOrderImages > GenerateAndSaveImagesOfUploadedFiles || End Time of Function GenerateAndSaveImagesOfUploadedFiles" + Helper.GetCurrentDate().ToLocalTime());

            if (reqSubmitUploadOrderImagesModel.Is_Manual_ORS)
            {
                string body = string.Empty;
                string template_html = HttpContext.Current.Server.MapPath(@"~/HtmlTemplates/ORS_info_Template.html");
                var config = Helper.GetServiceConfiguration(Profile.PracticeCode);
                body = File.ReadAllText(template_html);
                body = body.Replace("[[provider_name]]", reqSubmitUploadOrderImagesModel.ORS_NAME ?? "");
                body = body.Replace("[[provider_NPI]]", reqSubmitUploadOrderImagesModel.ORS_NPI ?? "");
                body = body.Replace("[[provider_phone]]", DataModels.HelperClasses.StringHelper.ApplyPhoneMask(reqSubmitUploadOrderImagesModel.ORS_PHONE) ?? "");
                body = body.Replace("[[provider_fax]]", DataModels.HelperClasses.StringHelper.ApplyPhoneMask(reqSubmitUploadOrderImagesModel.ORS_FAX) ?? "");
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
            if (!String.IsNullOrWhiteSpace(reqSubmitUploadOrderImagesModel.NOTE_DESC))
            {
                var newObj = new FOX_TBL_NOTES_HISTORY()
                {
                    WORK_ID = workId,
                    NOTE_DESC = reqSubmitUploadOrderImagesModel.NOTE_DESC
                };
                InsertNotesHistory(newObj, Profile);
            }
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

        public void GenerateAndSaveImagesOfUploadedFiles(long workId, List<string> FileNameList, UserProfile profile, int originalQueueFilesCount = 0)
        {
            //try
            //{
                var config = Helper.GetServiceConfiguration(profile.PracticeCode);
                if (config.PRACTICE_CODE != null
                    && !string.IsNullOrWhiteSpace(config.ORIGINAL_FILES_PATH_DB) && !string.IsNullOrWhiteSpace(config.ORIGINAL_FILES_PATH_SERVER)
                    && !string.IsNullOrWhiteSpace(config.IMAGES_PATH_DB) && !string.IsNullOrWhiteSpace(config.IMAGES_PATH_SERVER))
                {
                    //string pdfPath = HttpContext.Current.Server.MapPath("~/" + AppConfiguration.PdfPath);
                    //string imagesPath = HttpContext.Current.Server.MapPath("~/" + AppConfiguration.ImagesPath);

                    int totalPages = 0;
                    long pageCounter = originalQueueFilesCount;
                    foreach (var filePath1 in FileNameList)
                    {
                        string filePath = HttpContext.Current.Server.MapPath("~/" + AppConfiguration.RequestForOrderUploadImages + @"\" + filePath1);
                        var ext = Path.GetExtension(filePath).ToLower();
                        if (!Directory.Exists(config.ORIGINAL_FILES_PATH_SERVER))
                        {
                            Directory.CreateDirectory(config.ORIGINAL_FILES_PATH_SERVER);
                        }
                        if (ext == ".tiff" || ext == ".tif" || ext == ".png" || ext == ".jpg" || ext == ".jpeg" || ext == ".gif")
                        {
                            ConvertPDFToImages pdfToImg = new ConvertPDFToImages();
                            int numberOfPages = pdfToImg.tifToImage(filePath, config.IMAGES_PATH_SERVER, workId, pageCounter, config.IMAGES_PATH_DB, out pageCounter, true);
                            totalPages += numberOfPages;
                        }
                        else
                        {
                            int numberOfPages = getNumberOfPagesOfPDF(filePath);
                            SavePdfToImages(filePath, config, workId, numberOfPages, Convert.ToInt32(pageCounter), out pageCounter);
                            totalPages += numberOfPages;
                        }
                    }

                AddToDatabase("", totalPages + originalQueueFilesCount, profile.UserName, workId, config.PRACTICE_CODE); 
                }
                else
                {
                    throw new Exception("DB configuration for file paths not found. See service configuration.");
                }
            //}
            //catch (Exception ex)
            //{
            //    throw ex;
            //}
        }
        private int getNumberOfPagesOfPDF(string PdfPath)
        {
            iTextSharp.text.pdf.PdfReader pdfReader = new iTextSharp.text.pdf.PdfReader(PdfPath);
            return pdfReader.NumberOfPages;
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
            catch (Exception ex)
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

        private void AddToDatabase(string filePath, int noOfPages, string userName, long workId, long? practiceCode )
        {
            try
            {
                //OriginalQueue originalQueue = _QueueRepository.Get(t => t.WORK_ID == workId && !t.DELETED);
                //if (originalQueue != null)
                //{
                //    //TO DO
                //    //originalQueue.WORK_STATUS = "Indexed";
                //    originalQueue.TOTAL_PAGES = noOfPages;
                //    originalQueue.FILE_PATH = filePath;
                //    originalQueue.FAX_ID = "";

                //    //originalQueue.MODIFIED_BY = userName;
                //    originalQueue.MODIFIED_DATE = DateTime.Now;

                //    //_QueueRepository.Update(originalQueue);
                //    //_QueueRepository.Save();
                //}


                var PracticeCode = new SqlParameter { ParameterName = "@PRACTICE_CODE", SqlDbType = SqlDbType.BigInt, Value = practiceCode };
                var workid = new SqlParameter { ParameterName = "@WORK_ID", SqlDbType = SqlDbType.BigInt, Value = workId };
                var username = new SqlParameter { ParameterName = "@USER_NAME", SqlDbType = SqlDbType.VarChar, Value = userName };
                var filePaths = new SqlParameter { ParameterName = "@FILE_PATH", SqlDbType = SqlDbType.VarChar, Value = filePath };
                var noofpages = new SqlParameter { ParameterName = "@NO_OF_PAGES", SqlDbType = SqlDbType.Int, Value = noOfPages };

                var result = SpRepository<OriginalQueue>.GetListWithStoreProcedure(@"exec FOX_PROC_ADD_TO_DB_FROM_UPLOAD_ORDER_IMAGES @PRACTICE_CODE, @WORK_ID, @USER_NAME, @FILE_PATH, @NO_OF_PAGES",
                    PracticeCode, workid, username, filePaths, noofpages);
            }
            catch (Exception exception)
            {
                //throw exception;
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