using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FOX.DataEntities.Model.Fax
{
   
    public class FaxRequest
    {
        public string fromDate{get; set;} 
        public string toDate{get; set;} 
        public string faxStatus{get; set;} 
        public string faxID{get; set;} 
        public string resultLimit{get; set;} 
        public string nextRef { get; set; }
    }

    public class FaxInfo
    {
        public string FaxId{get; set;}
        public string CallerNumber{get; set;}
        public string FaxStatus{get; set;}
        public string FaxUserId{get; set;}
        public string Caption{get; set;}
        public string CreateTime{get; set;}
        public string CSI{get; set;}
        public string DestFaxNumber{get; set;}
        public string Duration{get; set;}
        public string ErrorCode{get; set;}
        public string ErrorText{get; set;}
        public string Note{get; set;}
        public string ImgCountry{get; set;}
        public string ImgDevice{get; set;}
        public string ImgHost{get; set;}
        public string NotifyAttachFax{get; set;}
        public string NotifyEmail{get; set;}
        public string PagesReceived{get; set;}
        public string PrintFax{get; set;}
        public string ReceivedTime{get; set;}
        public string TSI{get; set;}
        public string Viewed{get; set;}
        public string Mark{get; set;}

        public string FaxNumber { get; set; }
        public string CompleteTime { get; set; }
        public string CoverPageName { get; set; }
        public string DeleteAfterSend { get; set; }
        public string FaxDescription { get; set; }
        public string FaxHeaderEnabled { get; set; }
        public string FaxQuality  { get; set;}
        public string GroupId { get; set; }
        public string NotifyEmailAddress {get; set;}
        public string NotifyFailed { get; set; }
        public string NotifyFailedAttachFax { get; set; }
        public string NotifySuccess { get; set; }
        public string PagesSent { get; set; }
        public string PagesTotal { get; set; }
        public string PrintAfterSend { get; set; }
        public string Priority { get; set; }
        public string ProjectCode { get; set; }
        public string ProjectCode2 { get; set; }
        public string ProxyFaxUserId { get; set; }
        public string RcptInfo { get; set; }
        public string RetryCount { get; set; }
        public string RetryCountLeft { get; set; }
        public string RetryInterval { get; set; }
        public string SendAfter { get; set; }
    }

    public class ReceivedFaxResponse
    {
        public List<FaxInfo> faxInfo { get; set; }
        public string HasMoreResults { get; set; }
        public string ResultsReference { get; set; }
    }

    public class SendFaxRequest
    {
        public string[] Recp_Fax { get; set; }
        public string[] CoverLetter { get; set; }
        public string[] RecpientName { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public string Subject { get; set; }
        public bool isCallFromFax { get; set; }
    }


}
