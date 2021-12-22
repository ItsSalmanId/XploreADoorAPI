using FOX.DataModels.Models.Security;
using FOX.DataModels.Models.UploadWorkOrderFiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FOX.BusinessOperations.IndexInfoServices.UploadWorkOrderFiles
{
    public interface IUploadWorkOrderFilesService
    {
        ResSaveUploadWorkOrderFiles SaveUploadWorkOrderFiles(ReqSaveUploadWorkOrderFiles reqSaveUploadWorkOrderFiles, UserProfile Profile);
        ResSaveUploadWorkOrderFiles saveUploadAdditionalWorkOrderFiles(ReqSaveUploadWorkOrderFiles reqSaveUploadWorkOrderFiles, UserProfile Profile);
    }
}
