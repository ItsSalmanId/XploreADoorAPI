using FOX.DataModels.Models.CommonModel;
using FOX.DataModels.Models.Security;
using System;
using System.IO;
using FOX.DataModels.Models.HrAutoEmail;
using FOX.DataModels.GenericRepository;
using FOX.DataModels.Context;
using FOX.BusinessOperations.CommonService;
using FOX.BusinessOperations.Security;

namespace FOX.BusinessOperations.CommonServices.UploadFiles
{
    public class UploadFilesServices : IUploadFilesServices
    {
        private readonly DBContextHrAutoEmail _dbContextHrAutoEmail = new DBContextHrAutoEmail();
        private readonly GenericRepository<HrAutoEmailConfigure> _hrAutoEmailRepository;
        private readonly GenericRepository<HrEmailDocumentFileAll> _hrEmailDocumentFileAllRepository;
        public UploadFilesServices()
        {
            _hrAutoEmailRepository = new GenericRepository<HrAutoEmailConfigure>(_dbContextHrAutoEmail);
            _hrEmailDocumentFileAllRepository = new GenericRepository<HrEmailDocumentFileAll>(_dbContextHrAutoEmail);
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
                        //else if (postedFile.ContentLength > MaxContentLength)
                        //{
                        //    var message = string.Format("Please Upload a file upto 5MB.");
                        //    dict.Add("error", message);
                        //    return Request.CreateResponse(HttpStatusCode.BadRequest, dict);
                        //}
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
                            responseUploadFilesModel.FileName = Encrypt.EncryptionForClient(filePath);
                            responseUploadFilesModel.Message = "File Uploaded Successfully.";
                            responseUploadFilesModel.Success = true;
                            responseUploadFilesModel.ErrorMessage = "" ;
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
                responseUploadFilesModel.FileName = "";
                return responseUploadFilesModel;
            }
        }

        public ResponseLedgerUploadFilesModel UploadReconsiliationLedger(RequestUploadFilesModel requestUploadFilesModel, string reconsiliationId)
        {
            ResponseLedgerUploadFilesModel responseUploadFilesModel = new ResponseLedgerUploadFilesModel();
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
                        //else if (postedFile.ContentLength > MaxContentLength)
                        //{
                        //    var message = string.Format("Please Upload a file upto 5MB.");
                        //    dict.Add("error", message);
                        //    return Request.CreateResponse(HttpStatusCode.BadRequest, dict);
                        //}
                        else
                        {
                            string uploadFilesPath = requestUploadFilesModel?.UploadFilesPath;
                            if (!Directory.Exists(uploadFilesPath))
                            {
                                Directory.CreateDirectory(uploadFilesPath);
                            }
                            fileName = reconsiliationId + "_" + fileName + "_" + DateTime.Now.Ticks + fileExtension;
                            string filePath = uploadFilesPath + @"\" + fileName;
                            responseUploadFilesModel.AbsolutePathWithFileName = filePath;
                            responseUploadFilesModel.OrignalFileName = postedFile.FileName;
                            responseUploadFilesModel.FilePath = FOX.BusinessOperations.CommonServices.AppConfiguration.ReconciliationOriginalFilesDirectory + @"\" + fileName;
                            responseUploadFilesModel.Message = "File Uploaded Successfully.";
                            responseUploadFilesModel.Success = true;
                            responseUploadFilesModel.ErrorMessage = "";
                            responseUploadFilesModel.AbsolutePathWithFileName = string.Empty;
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
        /// <summary>
        /// This Function is used to Upload File
        /// </summary>
        /// <param name="requestUploadFilesModel"></param>
        /// <param name="userProfile"></param>
        /// <returns></returns>
        public ResponseUploadFilesModel UploadHAutoEmailFiles(RequestUploadFilesModel requestUploadFilesModel, UserProfile userProfile)
        {
            if (requestUploadFilesModel != null)
            {
                var hrAutoResponseDetail = _hrAutoEmailRepository.GetFirst(s => s.HR_CONFIGURE_ID == requestUploadFilesModel.HR_CONFIGURE_ID && !s.DELETED);
                if (hrAutoResponseDetail != null)
                {
                    hrAutoResponseDetail.FILE_PATH = requestUploadFilesModel.UploadFilesPath;

                    _hrAutoEmailRepository.Update(hrAutoResponseDetail);
                    _hrAutoEmailRepository.Save();
                }
            }
            ResponseUploadFilesModel responseUploadFilesModel = new ResponseUploadFilesModel();
            string message = "Please upload file of type " + String.Join(", ", requestUploadFilesModel?.AllowedFileExtensions) + ".";
            try
            {
                foreach (string file in requestUploadFilesModel?.Files)
                {
                    var postedFile = requestUploadFilesModel?.Files[file];
                    if (postedFile != null && postedFile.ContentLength > 0)
                    {
                        string fileName = Path.GetFileNameWithoutExtension(postedFile.FileName);
                        if (fileName?.Length > 30)
                            fileName = fileName.Substring(0, 30);

                        string fileExtension = Path.GetExtension(postedFile.FileName);

                        if (!(requestUploadFilesModel?.AllowedFileExtensions.Contains(fileExtension?.ToLower()) ?? false))
                        {
                            responseUploadFilesModel.FilePath = "";
                            responseUploadFilesModel.Message = message;
                            responseUploadFilesModel.Success = false;
                            responseUploadFilesModel.ErrorMessage = "";
                            return responseUploadFilesModel;
                        }
                        else
                        {
                            var result = _hrAutoEmailRepository.GetFirst(f => f.HR_CONFIGURE_ID == requestUploadFilesModel.HR_CONFIGURE_ID && !f.DELETED).NAME;
                            string uploadFilesPath = requestUploadFilesModel?.UploadFilesPath + "\\" + result;

                            if (!Directory.Exists(uploadFilesPath))
                            {
                                Directory.CreateDirectory(uploadFilesPath);
                            }

                            fileName += "_" + DateTime.Now.Ticks + fileExtension;
                            string filePath = uploadFilesPath + @"\" + fileName;

                            responseUploadFilesModel.FilePath = fileName;

                            var getfileName = responseUploadFilesModel.FilePath;
                            var fileNameSubstring = getfileName.Split('_');
                            responseUploadFilesModel.FilePath = fileNameSubstring[0].ToString();
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
    }
}