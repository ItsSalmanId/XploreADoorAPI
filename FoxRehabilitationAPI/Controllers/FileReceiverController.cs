//using FOX.BusinessOperations.CommonService;
//using FOX.BusinessOperations.CommonServices;
//using FOX.DataModels.Models.CommonModel;
//using FoxRehabilitationAPI.Filters;
//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Web;
//using System.Web.Http;

//namespace FoxRehabilitationAPI.Controllers
//{
//    [Authorize]
//    [ExceptionHandlingFilter]
//    public class FileReceiverController : BaseApiController
//    {
//        #region check if file save to temprary folder or permanent
//        private bool IsToSaveAtTempraryFolder = true;
//        private bool IsToReturnServerPath = true;
//        private string PermanentFolderName = "";
//        public FileReceiverController()
//        {
//            IsToSaveAtTempraryFolder = GetHeaderValues("IsTempFolder") == "" ? true : Convert.ToBoolean(GetHeaderValues("IsTempFolder"));
//            PermanentFolderName = GetHeaderValues("Folder");
//            IsToReturnServerPath = GetHeaderValues("IsToReturnServerPath") == "" ? true : Convert.ToBoolean(GetHeaderValues("IsToReturnServerPath"));
//        }
//        #endregion
//        [HttpPost]
//        public List<FileRecieverResult> FileReceive()
//        {
//            var profile = GetProfile();
//            List<FileRecieverResult> FileRecieverResultList = new List<FileRecieverResult>();
//            string DirectoryPath = "";

//            string PracticeCode = profile.PracticeCode.ToString();
//            string PracticeDocumentServerIp = profile.PracticeDocumentServerIP;
//            string PracticeDocumentDirectoryName = profile.PracticeDocumentDirectory;
//            var fileAttachments = HttpContext.Current.Request.Files;
//            if (fileAttachments.Count > 0)
//            {
//                for (int i = 0; i < fileAttachments.Count; i++)
//                {
//                    if (fileAttachments[i] != null && fileAttachments[i].ContentLength > 0)
//                    {
//                        FileRecieverResult result = new FileRecieverResult();
//                        var fileName = string.Empty;
//                        var fileExtension = Path.GetExtension(fileAttachments[i].FileName);
//                        if (!IsToSaveAtTempraryFolder)
//                        {
//                            DirectoryPath = DocumentHelper.GetPracticeDocumentDirectoryPath(profile, false) + "\\" + PermanentFolderName;
//                        }
//                        else
//                        {
//                            DirectoryPath = DocumentHelper.GetPracticeDocumentDirectoryPath(profile, true);
//                        }
//                        if (!Directory.Exists(DirectoryPath))
//                        {
//                            Directory.CreateDirectory(DirectoryPath);
//                        }
//                        fileName = DocumentHelper.GenerateFileName(PracticeCode, fileExtension);
//                        var dirName = DirectoryPath + "\\Logo_" + fileName;
//                        DirectoryPath = DirectoryPath + "\\" + fileName;
//                        fileAttachments[i].SaveAs(DirectoryPath);
//                        fileAttachments[i].SaveAs(dirName);
//                        //Bitmap b = (Bitmap)Bitmap.FromStream(fileAttachments[i].InputStream);
//                        //using (MemoryStream ms = new MemoryStream())
//                        //{
//                        //    b.Save(ms, ImageFormat.Png);

//                        //    // use the memory stream to base64 encode.. 
//                        //}

//                        //ImageHandler handler = new ImageHandler();
//                        // handler.Save(b, 115, 150, 100, dirName);

//                        result.FileName = DocumentHelper.GetFileNameOrDirectoryPath(DirectoryPath);
//                        result.FilePath = DocumentHelper.GetFileNameOrDirectoryPath(DirectoryPath, false);
//                        if (IsToSaveAtTempraryFolder)
//                        {
//                            string CompleteDirecotryWithFileName = TiffEditor.TiffHandler(DirectoryPath, fileName);
//                            result.FileName = DocumentHelper.GetFileNameOrDirectoryPath(CompleteDirecotryWithFileName);
//                            result.FilePath = DocumentHelper.GetFileNameOrDirectoryPath(CompleteDirecotryWithFileName, false);
//                        }
//                        result.FilePath = PracticeDocumentDirectoryName + "\\" + PracticeCode + "\\Fox";
//                        FileRecieverResultList.Add(result);
//                    }
//                }
//            }
//            return FileRecieverResultList;
//        }

//        private string GetHeaderValues(string Key)
//        {
//            return HttpContext.Current.Request.Headers.Get(Key) != null ? HttpContext.Current.Request.Headers.GetValues(Key).FirstOrDefault() : "";

//        }
//    }
//}
