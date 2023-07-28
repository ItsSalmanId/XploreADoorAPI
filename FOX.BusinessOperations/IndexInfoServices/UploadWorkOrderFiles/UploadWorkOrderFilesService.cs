using FOX.DataModels.Models.UploadWorkOrderFiles;
using FOX.BusinessOperations.RequestForOrder.UploadOrderImages;
using FOX.DataModels.Models.Security;
using FOX.DataModels.GenericRepository;
using FOX.DataModels.Models.IndexInfo;
using System.Data.SqlClient;
using System.Data;
using FOX.DataModels.Context;
using FOX.DataModels.Models.OriginalQueueModel;
using System.Linq;
using System.Web;
using System.IO;
using System;
using FOX.BusinessOperations.CommonService;
using FOX.BusinessOperations.CommonServices;
using System.Collections.Generic;
using ICSharpCode.SharpZipLib.Zip;
using ICSharpCode.SharpZipLib.Core;
using FOX.DataModels.Models.CommonModel;

namespace FOX.BusinessOperations.IndexInfoServices.UploadWorkOrderFiles
{
    public class UploadWorkOrderFilesService : IUploadWorkOrderFilesService
    {
        private readonly IUploadOrderImagesService _IUploadOrderImagesService;

        private readonly DBContextQueue _QueueContext = new DBContextQueue();
        //private readonly GenericRepository<OriginalQueue> _QueueRepository;
        private readonly GenericRepository<OriginalQueueFiles> _OriginalQueueFiles;

        public UploadWorkOrderFilesService()
        {
            _OriginalQueueFiles = new GenericRepository<OriginalQueueFiles>(_QueueContext);
            _IUploadOrderImagesService = new UploadOrderImagesService();
        }
        public ResSaveUploadWorkOrderFiles saveUploadAdditionalWorkOrderFiles(ReqSaveUploadWorkOrderFiles reqSaveUploadWorkOrderFiles, UserProfile Profile)
        {
            var result = new ResSaveUploadWorkOrderFiles();
            long workId = reqSaveUploadWorkOrderFiles.WORK_ID;

            var originalQueueFilesCount = _OriginalQueueFiles.GetMany(t => t.WORK_ID == workId && !t.deleted)?.Count() ?? 0;
            _IUploadOrderImagesService.GenerateAndSaveImagesOfUploadedFiles(workId, reqSaveUploadWorkOrderFiles.FileNameList, Profile, originalQueueFilesCount);

            result.WORK_ID = workId;
            result.FilePaths = SpRepository<FilePath>.GetListWithStoreProcedure(@"exec FOX_GET_File_PAGES  @WORK_ID", new SqlParameter("WORK_ID ", SqlDbType.BigInt) { Value = workId });
            result.Message = $"Upload Work Order Files Successfully. WorkId = { workId }";
            result.ErrorMessage = "";
            result.Success = true;

            return result;
        }
        public ResponseModel GenerateAndSaveImagesOfUploadedFilesZip(ReqSaveUploadWorkOrderFiles reqSaveUploadWorkOrderFiles, UserProfile Profile)
        {
            ResponseModel response = new ResponseModel();
            var config = Helper.GetServiceConfiguration(Profile.PracticeCode);
            List<string> filePathsZip = new List<string>();
            SqlParameter refWorkIdd = new SqlParameter { ParameterName = "@WORK_ID", SqlDbType = SqlDbType.BigInt, Value = reqSaveUploadWorkOrderFiles.WORK_ID };
            var originalQueueData = SpRepository<OriginalQueue>.GetSingleObjectWithStoreProcedure(@"exec FOX_PROC_GET_WORK_QUEUE_FILE_ALL_RECORD @WORK_ID", refWorkIdd);
            foreach (var item in reqSaveUploadWorkOrderFiles.FileNameList)
            {
                filePathsZip.Add(HttpContext.Current.Server.MapPath("~/" + AppConfiguration.RequestForOrderUploadImages + @"\" + item));
            }
            string zipfolderpath = config.ORIGINAL_FILES_PATH_SERVER;
            string FileName = reqSaveUploadWorkOrderFiles.WORK_ID + "_" + DateTime.Now.Ticks + ".zip";
            var filePath2 = @"" + zipfolderpath + "\\" + FileName;
            var newZipFilePath = @"" + zipfolderpath + "\\NewZipFile";
            if (!Directory.Exists(newZipFilePath))
            {
                Directory.CreateDirectory(newZipFilePath);
            }
            ZipFile file = null;
            if (!string.IsNullOrEmpty(originalQueueData.FILE_PATH))
            {
                try
                {
                    FileStream fs = File.OpenRead(originalQueueData.FILE_PATH);
                    file = new ZipFile(fs);
                    foreach (ZipEntry zipEntry in file)
                    {
                        if (!zipEntry.IsFile)
                        {
                            // Ignore directories
                            continue;
                        }
                        String entryFileName = zipEntry.Name;
                        // to remove the folder from the entry:- entryFileName = Path.GetFileName(entryFileName);
                        // Optionally match entrynames against a selection list here to skip as desired.
                        // The unpacked length is available in the zipEntry.Size property.

                        // 4K is optimum
                        byte[] buffer = new byte[4096];
                        Stream zipStream = file.GetInputStream(zipEntry);

                        // Manipulate the output filename here as desired.
                        String fullZipToPath = Path.Combine(newZipFilePath, entryFileName);
                        string directoryName = Path.GetDirectoryName(fullZipToPath);
                        filePathsZip.Add(fullZipToPath);
                        if (directoryName.Length > 0)
                        {
                            Directory.CreateDirectory(directoryName);
                        }
                        // Unzip file in buffered chunks. This is just as fast as unpacking to a buffer the full size
                        // of the file, but does not waste memory.
                        // The "using" will close the stream even if an exception occurs.
                        using (FileStream streamWriter = File.Create(fullZipToPath))
                        {
                            StreamUtils.Copy(zipStream, streamWriter, buffer);
                        }
                    }
                }
                finally
                {
                    if (file != null)
                    {
                        file.IsStreamOwner = true; // Makes close also shut the underlying stream
                        file.Close(); // Ensure we release resources
                    }
                }
            }
            using (var zipStream = new ZipOutputStream(File.Create(filePath2)))
            {
                foreach (string filePaths in filePathsZip)
                {
                    byte[] fileBytes = System.IO.File.ReadAllBytes(filePaths);
                    var fileEntry = new ZipEntry(Path.GetFileName(filePaths))
                    {
                        Size = fileBytes.Length
                    };
                    zipStream.PutNextEntry(fileEntry);
                    zipStream.Write(fileBytes, 0, fileBytes.Length);
                }
                zipStream.Flush();
                zipStream.Close();
            }
            UploadOrderImagesService uploadOrderImagesServiceObj = new UploadOrderImagesService();
            int totalPages = 0;
            long workId = reqSaveUploadWorkOrderFiles.WORK_ID;
            SqlParameter refWorkId = new SqlParameter { ParameterName = "@WORK_ID", SqlDbType = SqlDbType.BigInt, Value = workId };
            var originalQueueFilesCount = Convert.ToInt32(SpRepository<string>.GetSingleObjectWithStoreProcedure(@"exec FOX_PROC_GET_WORK_QUEUE_FILE_ALL_LIST @WORK_ID", refWorkId));
            uploadOrderImagesServiceObj.AddToDatabase(filePath2, totalPages + originalQueueFilesCount, Profile.UserName, workId, config.PRACTICE_CODE);
            string zipFilePath = filePath2.ToString();
            response.Message = zipFilePath;
            return response;
        }
        public ResSaveUploadWorkOrderFiles SaveUploadWorkOrderFiles(ReqSaveUploadWorkOrderFiles reqSaveUploadWorkOrderFiles, UserProfile Profile)
        {
            var result = new ResSaveUploadWorkOrderFiles();
            long workId = reqSaveUploadWorkOrderFiles.WORK_ID;
            UploadOrderImagesService uploadOrderImagesServiceObj = new UploadOrderImagesService();
            int totalPages = 0;
            SqlParameter refWorkId = new SqlParameter { ParameterName = "@WORK_ID", SqlDbType = SqlDbType.BigInt, Value = workId };
            var originalQueueFilesCount = Convert.ToInt32(SpRepository<string>.GetSingleObjectWithStoreProcedure(@"exec FOX_PROC_GET_WORK_QUEUE_FILE_ALL_LIST @WORK_ID", refWorkId));
            var config = Helper.GetServiceConfiguration(Profile.PracticeCode);
            if (config.PRACTICE_CODE != null
                && !string.IsNullOrWhiteSpace(config.ORIGINAL_FILES_PATH_DB) && !string.IsNullOrWhiteSpace(config.ORIGINAL_FILES_PATH_SERVER)
                && !string.IsNullOrWhiteSpace(config.IMAGES_PATH_DB) && !string.IsNullOrWhiteSpace(config.IMAGES_PATH_SERVER))
            {
                //string pdfPath = HttpContext.Current.Server.MapPath("~/" + AppConfiguration.PdfPath);
                //string imagesPath = HttpContext.Current.Server.MapPath("~/" + AppConfiguration.ImagesPath);


                long pageCounter = originalQueueFilesCount;
                foreach (var filePath1 in reqSaveUploadWorkOrderFiles.FileNameList)
                {
                    string filePath = HttpContext.Current.Server.MapPath("~/" + AppConfiguration.RequestForOrderUploadImages + @"\" + filePath1);
                    var ext = Path.GetExtension(filePath).ToLower();
                    if (!Directory.Exists(config.ORIGINAL_FILES_PATH_SERVER))
                    {
                        Directory.CreateDirectory(config.ORIGINAL_FILES_PATH_SERVER);
                    }
                    if (ext == ".tiff" || ext == ".tif" || ext == ".png" || ext == ".jpg" || ext == ".jpeg" || ext == ".gif" || ext == ".bmp")
                    {
                        ConvertPDFToImages pdfToImg = new ConvertPDFToImages();
                        int numberOfPages = pdfToImg.tifToImage(filePath, config.IMAGES_PATH_SERVER, workId, pageCounter, config.IMAGES_PATH_DB, out pageCounter, true);
                        totalPages += numberOfPages;
                    }
                    else
                    {
                        int numberOfPages = uploadOrderImagesServiceObj.getNumberOfPagesOfPDF(filePath);
                        uploadOrderImagesServiceObj.SavePdfToImages(filePath, config, workId, numberOfPages, Convert.ToInt32(pageCounter), out pageCounter);
                        totalPages += numberOfPages;
                    }
                }
            }
            string zipfolderpath = config.ORIGINAL_FILES_PATH_SERVER;
            string FileName = workId + "_" + DateTime.Now.Ticks + ".zip";
            var filePathWithFolderPath = @"" + zipfolderpath + "\\" + FileName;
            result.WORK_ID = workId;
            result.zipFilePath = filePathWithFolderPath;
            result.FilePaths = SpRepository<FilePath>.GetListWithStoreProcedure(@"exec FOX_GET_File_PAGES  @WORK_ID", new SqlParameter("WORK_ID ", SqlDbType.BigInt) { Value = workId });
            result.Message = $"Upload Work Order Files Successfully. WorkId = { workId }";
            result.ErrorMessage = "";
            result.Success = true;

            decimal size = 0;
            decimal byteCount = 0;
            foreach (var list in result.FilePaths.ToList())
            {
                string virtualPath = @"/" + list.file_path1;
                string orignalPath = HttpContext.Current.Server.MapPath("~" + virtualPath);
                FileInfo file = new FileInfo(orignalPath);
                bool exists = file.Exists;
                if (file.Exists)
                {
                    byteCount = file.Length;
                    size += byteCount;
                }
            }
            result.fileSize = Convert.ToDecimal(string.Format("{0:0.00}", size / 1048576));

            return result;
        }
    }
}
