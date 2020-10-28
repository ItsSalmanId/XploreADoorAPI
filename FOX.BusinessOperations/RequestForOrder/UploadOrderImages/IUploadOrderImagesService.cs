using FOX.DataModels.Models.RequestForOrder.UploadOrderImages;
using FOX.DataModels.Models.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FOX.BusinessOperations.RequestForOrder.UploadOrderImages
{
    public interface IUploadOrderImagesService
    {
        ResGetSourceDataModel GetSourceData(string email, string userId, long practiceCode, string userName);
        ResSubmitUploadOrderImagesModel SubmitUploadOrderImages(ReqSubmitUploadOrderImagesModel reqSubmitUploadOrderImagesModel, UserProfile Profile);
        void GenerateAndSaveImagesOfUploadedFiles(long workId, List<string> FileNameList, UserProfile profile, int originalQueueFilesCount = 0);
    }
}
