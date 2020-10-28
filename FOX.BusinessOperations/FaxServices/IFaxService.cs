using FOX.DataEntities.Model.Fax;
using FOX.DataModels.Models.Security;
using FOX.DataModels.Models.Settings.Practice;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FOX.BusinessOperations.FaxServices
{
    public interface IFaxService
    {
        ReceivedFaxResponse GetReceivedFax(FaxRequest objRequest, UserProfile profile);
        ReceivedFaxResponse GetSentFax(FaxRequest objRequest, UserProfile profile);
        InterFaxDetail GetFaxUserDetails(long practiceCode);
        string getSentFaxContent(string faxID, UserProfile profile);
        string getReceivedFaxContent(string faxID, UserProfile profile);
        string forwardSentFaxContent(string faxID, UserProfile profile);
        string forwardReceivedFaxContent(string faxID, UserProfile profile);
        string SendFax(string[] Recp_Fax, string[] RecpName, string[] CoverLetter, string FileName, string FilePath, string Subject, bool isCallFromFax, UserProfile profile);
        bool deleteSentFax(string[] faxIdList);
        bool deleteReceivedFax(string[] faxIdList);
        string getMultipleSentFax(string[] faxIdList, UserProfile profile);
        string getMultipleReceivedFax(string[] faxIdList, UserProfile profile);
        bool saveFaxSetting(InterFaxDetail objSetting, UserProfile profile);
    }
}
