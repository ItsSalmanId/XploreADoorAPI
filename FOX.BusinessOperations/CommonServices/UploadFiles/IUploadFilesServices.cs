using FOX.DataModels.Models.CommonModel;
using FOX.DataModels.Models.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FOX.BusinessOperations.CommonServices.UploadFiles
{
    public interface IUploadFilesServices
    {
        ResponseUploadFilesModel UploadFiles(RequestUploadFilesModel requestUploadFilesAPIModel);
        ResponseLedgerUploadFilesModel UploadReconsiliationLedger(RequestUploadFilesModel requestUploadFilesAPIModel, string reconsiliationId);
    }
}
