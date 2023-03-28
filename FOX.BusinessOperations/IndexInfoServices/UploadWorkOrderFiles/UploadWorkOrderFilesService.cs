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
            Helper.TokenTaskCancellationExceptionLog("UploadWorkOrderFiles: In Function  SaveUploadAdditionalWorkOrderFiles > GenerateAndSaveImagesOfUploadedFiles || Start Time of Function GenerateAndSaveImagesOfUploadedFiles" + Helper.GetCurrentDate().ToLocalTime());
            _IUploadOrderImagesService.GenerateAndSaveImagesOfUploadedFiles(workId, reqSaveUploadWorkOrderFiles.FileNameList, Profile, originalQueueFilesCount);
            Helper.TokenTaskCancellationExceptionLog("UploadWorkOrderFiles: In Function  SaveUploadAdditionalWorkOrderFiles > GenerateAndSaveImagesOfUploadedFiles || End Time of Function GenerateAndSaveImagesOfUploadedFiles" + Helper.GetCurrentDate().ToLocalTime());

            result.WORK_ID = workId;
            result.FilePaths = SpRepository<FilePath>.GetListWithStoreProcedure(@"exec FOX_GET_File_PAGES  @WORK_ID", new SqlParameter("WORK_ID ", SqlDbType.BigInt) { Value = workId });
            result.Message = $"Upload Work Order Files Successfully. WorkId = { workId }";
            result.ErrorMessage = "";
            result.Success = true;

            return result;
        }
        public ResSaveUploadWorkOrderFiles SaveUploadWorkOrderFiles(ReqSaveUploadWorkOrderFiles reqSaveUploadWorkOrderFiles, UserProfile Profile)
        {
            var result = new ResSaveUploadWorkOrderFiles();
            long workId = reqSaveUploadWorkOrderFiles.WORK_ID;
            string zipFilePath = "";

            var originalQueueFilesCount = _OriginalQueueFiles.GetMany(t => t.WORK_ID == workId && !t.deleted)?.Count() ?? 0;
            Helper.TokenTaskCancellationExceptionLog("UploadWorkOrderFiles: In Function  SaveUploadWorkOrderFiles > GenerateAndSaveImagesOfUploadedFiles || Start Time of Function GenerateAndSaveImagesOfUploadedFiles" + Helper.GetCurrentDate().ToLocalTime());
            zipFilePath = _IUploadOrderImagesService.GenerateAndSaveImagesOfUploadedFilesZip(workId, reqSaveUploadWorkOrderFiles.FileNameList, Profile, originalQueueFilesCount);
            Helper.TokenTaskCancellationExceptionLog("UploadWorkOrderFiles: In Function  SaveUploadWorkOrderFiles > GenerateAndSaveImagesOfUploadedFiles || End Time of Function GenerateAndSaveImagesOfUploadedFiles" + Helper.GetCurrentDate().ToLocalTime());

            result.WORK_ID = workId;
            result.zipFilePath = zipFilePath;
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
