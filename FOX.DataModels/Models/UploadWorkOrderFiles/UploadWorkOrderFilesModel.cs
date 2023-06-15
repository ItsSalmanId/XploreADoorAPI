using FOX.DataModels.Models.CommonModel;
using FOX.DataModels.Models.IndexInfo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FOX.DataModels.Models.UploadWorkOrderFiles
{
    public class UploadWorkOrderFilesModel
    {
    }

    public class ReqSaveUploadWorkOrderFiles
    {
        public long WORK_ID { get; set; }
        public List<string> FileNameList { get; set; }
    }

    public class ResSaveUploadWorkOrderFiles: ResponseModel
    {
        public long WORK_ID { get; set; }
        public List<FilePath> FilePaths { get; set; }
        public decimal fileSize { get; set; }
        public string zipFilePath { get; set; }
    }
}
