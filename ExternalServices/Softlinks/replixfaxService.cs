using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.IO;
using iTextSharp.text;
using FOX.ExternalServices.replifax;
using FOX.DataEntities.Model.Fax;
using FOX.DataModels.Models.Security;
using System.Net;

namespace FOX.ExternalServices.Softlinks
{
    public class ReplixfaxService
    {

        private const string _true = "true";
        private const string _false = "false";
        private const string _pdf = "pdf";
        private const string _all = "all";
        private const string _portrait = "portrait";
        private const string _qualityHigh = "high";

        private AuthenticationToken _authToken = null;
        protected void connectReplifax(object sender, EventArgs e)
        {
            try
            {
                System.ServiceModel.BasicHttpBinding bind = null;
                ReplixFaxPortClient client = null;
                Authentication auth = null;
                initServiceCall(ref bind, ref client, ref auth);
                LoginInput input = new LoginInput();
                input.Authentication = auth;
                LoginOutput result = client.Login(input);
                if (result.RequestStatus.StatusCode.Equals("0"))
                {
                    AuthenticationToken authtoken = new AuthenticationToken();
                    authtoken.AuthToken = result.AuthToken;
                    _authToken = authtoken;
                }
            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }
        }
        private void initServiceCall(ref System.ServiceModel.BasicHttpBinding bind, ref ReplixFaxPortClient client, ref Authentication auth)
        {
            bind = new System.ServiceModel.BasicHttpBinding();

            // HERE'S THE IMPORTANT BIT FOR SSL
            bind.Security.Mode = BasicHttpSecurityMode.Transport;
            bind.Security.Transport.ClientCredentialType = System.ServiceModel.HttpClientCredentialType.Basic;
            bind.MessageEncoding = WSMessageEncoding.Mtom;
            bind.MaxReceivedMessageSize = 104857600;
            bind.SendTimeout = TimeSpan.FromMinutes(2);
            bind.BypassProxyOnLocal = false;
            string uriLink = "https://api.rpxfax.com/softlinx/replixfax/wsapi";
            EndpointAddress endpoint = new EndpointAddress(uriLink);

            const System.Security.Authentication.SslProtocols _Tls12 = (System.Security.Authentication.SslProtocols)0x00000C00;
            const SecurityProtocolType Tls12 = (SecurityProtocolType)_Tls12;
            ServicePointManager.SecurityProtocol = Tls12;

            bind.HostNameComparisonMode = HostNameComparisonMode.Exact;
            bind.Security.Transport.ProxyCredentialType = HttpProxyCredentialType.Basic;
            client = new ReplixFaxPortClient(bind, endpoint);
            client.ClientCredentials.UserName.UserName = "anonymous";
            auth = new Authentication();
            auth.Login = "admin";           
            auth.Password = EncodeTo64("admin");
            auth.PasswordSecurity = "base64";
            auth.Realm = "mtbc";
        }
        private string EncodeTo64(string toEncode)
        {
            byte[] toEncodeAsBytes = System.Text.ASCIIEncoding.ASCII.GetBytes(toEncode);
            string returnValue = System.Convert.ToBase64String(toEncodeAsBytes);
            return returnValue;
        }
        public static ReceivedFaxResponse getRecievedFaxes(string username, string fromDate, string toDate, string faxStatus, string faxID, string resultLimit, string nextRef)
        {
            ReplixfaxService obj = new ReplixfaxService();
            BasicHttpBinding bind = null;
            ReplixFaxPortClient client = null;
            Authentication auth = null;
            try
            {
                obj.initServiceCall(ref bind, ref client, ref auth);
                LoginInput loginInput = new LoginInput();
                loginInput.Authentication = auth;
                LoginOutput result = client.Login(loginInput);
                if (result.RequestStatus.StatusCode.Equals("0"))
                {
                    AuthenticationToken authtoken = new AuthenticationToken();
                    authtoken.AuthToken = result.AuthToken;
                    obj._authToken = authtoken;

                }
            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }
            QueryReceiveFaxInput input = new QueryReceiveFaxInput();

            input.Authentication = auth;
            input.AuthenticationToken = obj._authToken;
            input.FaxUserId = username;

            if (faxStatus != "all")
            {
                input.FaxStatus = faxStatus;
            }

            if (faxID != string.Empty && faxID != null)
            {
                input.FaxId = faxID;
            }

            if (fromDate != "" && toDate != "")
            {
                DateTime dtFrom = Convert.ToDateTime(fromDate);
                DateTimeOffset doFrom = new DateTimeOffset(dtFrom,
                                            TimeZoneInfo.Local.GetUtcOffset(dtFrom));

                input.DatetimeAfter = doFrom.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'sszzz");


                DateTime dtTo = Convert.ToDateTime(toDate).AddDays(1);
                DateTimeOffset doTo = new DateTimeOffset(dtTo,
                                           TimeZoneInfo.Local.GetUtcOffset(dtTo));
                input.DatetimeBefore = doTo.ToString("yyyy'-'MM'-'dd'T'23':'59':'59zzz");

            }
            if (resultLimit != string.Empty)
            {
                input.ResultLimit = resultLimit;
            }
            if (nextRef != string.Empty)
            {
                input.GetNextResults = nextRef;
            }
            QueryReceiveFaxOutput output = null;
            try
            {
                output = client.QueryReceiveFax(input);
            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }
            ReceivedFaxResponse resp = new ReceivedFaxResponse();
            resp.faxInfo = new List<FaxInfo>();
            if (output != null && output.FaxInfo != null)
            {
                if (output.FaxInfo.Length == 0)
                {
                    return resp;
                }

                resp.HasMoreResults = output.HasMoreResults;
                resp.ResultsReference = output.ResultsReference;

                for (int n = 0; n < output.FaxInfo.Length; n++)
                {

                    FaxInfo objFax = new FaxInfo();
                    objFax.FaxId = output.FaxInfo[n].FaxId;
                    if (output.FaxInfo[n].CallerNumber.Length > 10)
                        objFax.CallerNumber = output.FaxInfo[n].CallerNumber.Substring(output.FaxInfo[n].CallerNumber.Length - 10);
                    else
                        objFax.CallerNumber = output.FaxInfo[n].CallerNumber;
                    objFax.FaxStatus = output.FaxInfo[n].FaxStatus;
                    objFax.FaxUserId = output.FaxInfo[n].FaxUserId;
                    objFax.Caption = output.FaxInfo[n].Caption;
                    objFax.CreateTime = output.FaxInfo[n].CreateTime;
                    objFax.CSI = output.FaxInfo[n].CSI;
                    objFax.DestFaxNumber = output.FaxInfo[n].DestFaxNumber;
                    objFax.Duration = output.FaxInfo[n].Duration;
                    objFax.ErrorCode = output.FaxInfo[n].ErrorCode;
                    objFax.ErrorText = output.FaxInfo[n].ErrorText;
                    objFax.Note = output.FaxInfo[n].Note;
                    objFax.ImgCountry = output.FaxInfo[n].ImgCountry;
                    objFax.ImgDevice = output.FaxInfo[n].ImgDevice;
                    objFax.ImgHost = output.FaxInfo[n].ImgHost;
                    objFax.NotifyAttachFax = output.FaxInfo[n].NotifyAttachFax;
                    objFax.NotifyEmail = output.FaxInfo[n].NotifyEmail;
                    objFax.PagesReceived = output.FaxInfo[n].PagesReceived;
                    objFax.PrintFax = output.FaxInfo[n].PrintFax;
                    objFax.ReceivedTime = output.FaxInfo[n].ReceivedTime;
                    objFax.TSI = output.FaxInfo[n].TSI;
                    objFax.Viewed = output.FaxInfo[n].Viewed;
                    objFax.Mark = output.FaxInfo[n].Mark;
                    resp.faxInfo.Add(objFax);
                }
                return resp;
            }

            return resp;
        }
        public static ReceivedFaxResponse getSentFaxes(string username, string fromDate, string toDate, string faxStatus, string faxID, string resultLimit, string nextRef)
        {
            ReplixfaxService obj = new ReplixfaxService();
            System.ServiceModel.BasicHttpBinding bind = null;
            ReplixFaxPortClient client = null;
            Authentication auth = null;

            try
            {
                obj.initServiceCall(ref bind, ref client, ref auth);
                LoginInput loginInput = new LoginInput();
                loginInput.Authentication = auth;
                LoginOutput result = client.Login(loginInput);
                if (result.RequestStatus.StatusCode.Equals("0"))
                {
                    AuthenticationToken authtoken = new AuthenticationToken();
                    authtoken.AuthToken = result.AuthToken;
                    obj._authToken = authtoken;

                }
            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }

            QuerySendFaxInput input = new QuerySendFaxInput();

            input.Authentication = auth;
            input.AuthenticationToken = obj._authToken;

            input.FaxUserId = username;
            input.Mark = _false;
            if (faxStatus != "all")
            {
                input.FaxStatus = faxStatus;
            }
            if (faxID != string.Empty && faxID != null)
            {
                input.FaxId = faxID;
            }

            if (fromDate != "" && toDate != "")
            {
                DateTime dtFrom = Convert.ToDateTime(fromDate);
                DateTimeOffset doFrom = new DateTimeOffset(dtFrom,
                                            TimeZoneInfo.Local.GetUtcOffset(dtFrom));

                input.DatetimeAfter = doFrom.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'sszzz");


                DateTime dtTo = Convert.ToDateTime(toDate);
                DateTimeOffset doTo = new DateTimeOffset(dtTo,
                                           TimeZoneInfo.Local.GetUtcOffset(dtTo));
                input.DatetimeBefore = doTo.ToString("yyyy'-'MM'-'dd'T'23':'59':'59zzz");
            }
            if (resultLimit != string.Empty)
            {
                input.ResultLimit = resultLimit;
            }
            if (nextRef != string.Empty)
            {
                input.GetNextResults = nextRef;
            }
            QuerySendFaxOutput output = null;
            try
            {
                output = client.QuerySendFax(input);
            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }

            ReceivedFaxResponse resp = new ReceivedFaxResponse();
            resp.faxInfo = new List<FaxInfo>();
            // fill the result grid
            if (output != null && output.FaxInfo != null)
            {
                if (output.FaxInfo.Length == 0)
                {
                    return resp;
                }

                resp.HasMoreResults = output.HasMoreResults;
                resp.ResultsReference = output.ResultsReference;

                for (int n = 0; n < output.FaxInfo.Length; n++)
                {

                    FaxInfo objFax = new FaxInfo();
                    objFax.FaxId = output.FaxInfo[n].FaxId;
                    objFax.FaxNumber = output.FaxInfo[n].FaxNumber;
                    objFax.FaxStatus = output.FaxInfo[n].FaxStatus;
                    objFax.FaxUserId = output.FaxInfo[n].FaxUserId;
                    objFax.CompleteTime = output.FaxInfo[n].CompleteTime;
                    objFax.CoverPageName = output.FaxInfo[n].CoverPageName;
                    objFax.CreateTime = output.FaxInfo[n].CreateTime;
                    objFax.CSI = output.FaxInfo[n].CSI;
                    objFax.DeleteAfterSend = output.FaxInfo[n].DeleteAfterSend;
                    objFax.Duration = output.FaxInfo[n].Duration;
                    objFax.ErrorCode = output.FaxInfo[n].ErrorCode;
                    objFax.ErrorText = output.FaxInfo[n].ErrorText;
                    objFax.FaxDescription = output.FaxInfo[n].FaxDescription;
                    objFax.FaxHeaderEnabled = output.FaxInfo[n].FaxHeaderEnabled;
                    objFax.FaxQuality = output.FaxInfo[n].FaxQuality;
                    objFax.GroupId = output.FaxInfo[n].GroupId;
                    objFax.ImgCountry = output.FaxInfo[n].ImgCountry;
                    objFax.ImgDevice = output.FaxInfo[n].ImgDevice;
                    objFax.ImgHost = output.FaxInfo[n].ImgHost;
                    objFax.NotifyEmailAddress = output.FaxInfo[n].NotifyEmailAddress;
                    objFax.NotifyFailed = output.FaxInfo[n].NotifyFailed;
                    objFax.NotifyFailedAttachFax = output.FaxInfo[n].NotifyFailedAttachFax;
                    objFax.NotifySuccess = output.FaxInfo[n].NotifySuccess;
                    objFax.PagesSent = output.FaxInfo[n].PagesSent;
                    objFax.PagesTotal = output.FaxInfo[n].PagesTotal;
                    objFax.PrintAfterSend = output.FaxInfo[n].PrintAfterSend;
                    objFax.Priority = output.FaxInfo[n].Priority;
                    objFax.ProjectCode = output.FaxInfo[n].ProjectCode;
                    objFax.ProjectCode2 = output.FaxInfo[n].ProjectCode2;
                    objFax.ProxyFaxUserId = output.FaxInfo[n].ProxyFaxUserId;
                    objFax.RcptInfo = output.FaxInfo[n].RcptInfo;
                    objFax.RetryCount = output.FaxInfo[n].RetryCount;
                    objFax.RetryCountLeft = output.FaxInfo[n].RetryCountLeft;
                    objFax.RetryInterval = output.FaxInfo[n].RetryInterval;
                    objFax.SendAfter = output.FaxInfo[n].SendAfter;
                    objFax.TSI = output.FaxInfo[n].TSI;
                    objFax.Mark = output.FaxInfo[n].Mark;
                    resp.faxInfo.Add(objFax);
                }
                return resp;
            }
            return resp;
        }
        public static bool deleteSentFax(string[] faxIdList)
        {
            ReplixfaxService obj = new ReplixfaxService();
            System.ServiceModel.BasicHttpBinding bind = null;
            ReplixFaxPortClient client = null;
            Authentication auth = null;
            try
            {
                obj.initServiceCall(ref bind, ref client, ref auth);
                LoginInput loginInput = new LoginInput();
                loginInput.Authentication = auth;
                LoginOutput result = client.Login(loginInput);
                if (result.RequestStatus.StatusCode.Equals("0"))
                {
                    AuthenticationToken authtoken = new AuthenticationToken();
                    authtoken.AuthToken = result.AuthToken;
                    obj._authToken = authtoken;
                }
                DeleteSendFaxInput input = new DeleteSendFaxInput();
                DeleteSendFaxOutput output = null;
                input.Authentication = auth;
                input.AuthenticationToken = obj._authToken;
                input.FaxId = faxIdList;
                try
                {
                    output = client.DeleteSendFax(input);
                    if (output.RequestStatus.StatusCode.CompareTo("0") == 0)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                catch
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }
        }
        public static bool deleteReceivedFax(string[] faxIdList)
        {
            string returnValue = string.Empty;
            ReplixfaxService obj = new ReplixfaxService();
            System.ServiceModel.BasicHttpBinding bind = null;
            ReplixFaxPortClient client = null;
            Authentication auth = null;

            try
            {
                obj.initServiceCall(ref bind, ref client, ref auth);
                LoginInput loginInput = new LoginInput();
                loginInput.Authentication = auth;
                LoginOutput result = client.Login(loginInput);
                if (result.RequestStatus.StatusCode.Equals("0"))
                {
                    AuthenticationToken authtoken = new AuthenticationToken();
                    authtoken.AuthToken = result.AuthToken;
                    obj._authToken = authtoken;
                }
                DeleteReceiveFaxInput input = new DeleteReceiveFaxInput();
                DeleteReceiveFaxOutput output = null;
                input.Authentication = auth;
                input.AuthenticationToken = obj._authToken;
                input.FaxId = faxIdList;
                try
                {
                    output = client.DeleteReceiveFax(input);
                    if (output.RequestStatus.StatusCode.CompareTo("0") == 0)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                catch (Exception ex)
                {
                    returnValue = Convert.ToString(ex.Message);
                    return false;
                }
            }
            catch (Exception ex)
            {
                returnValue = Convert.ToString(ex.Message);
                return false;
            }
        }
        public static GetReceiveFaxContentOutput downloadRecievedFax(string faxID)
        {
            ReplixfaxService obj = new ReplixfaxService();
            System.ServiceModel.BasicHttpBinding bind = null;
            ReplixFaxPortClient client = null;
            Authentication auth = null;

            try
            {
                obj.initServiceCall(ref bind, ref client, ref auth);
                LoginInput loginInput = new LoginInput();
                loginInput.Authentication = auth;
                LoginOutput result = client.Login(loginInput);
                if (result.RequestStatus.StatusCode.Equals("0"))
                {
                    AuthenticationToken authtoken = new AuthenticationToken();
                    authtoken.AuthToken = result.AuthToken;
                    obj._authToken = authtoken;
                }

                GetReceiveFaxContentInput contentInput = new GetReceiveFaxContentInput();
                contentInput.Authentication = auth;
                contentInput.AuthenticationToken = obj._authToken;

                contentInput.FaxId = faxID;
                contentInput.FaxContentType = _pdf;
                GetReceiveFaxContentOutput contentOutput = null;
                try
                {
                    contentOutput = client.GetReceiveFaxContent(contentInput);
                }
                catch (Exception ex)
                {
                    throw ex.InnerException;
                }
                return contentOutput;
            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }
        }
        public static GetSendFaxContentOutput downloadSentFax(string faxID)
        {
            ReplixfaxService obj = new ReplixfaxService();
            System.ServiceModel.BasicHttpBinding bind = null;
            ReplixFaxPortClient client = null;
            Authentication auth = null;

            try
            {
                obj.initServiceCall(ref bind, ref client, ref auth);
                LoginInput loginInput = new LoginInput();
                loginInput.Authentication = auth;
                LoginOutput result = client.Login(loginInput);
                if (result.RequestStatus.StatusCode.Equals("0"))
                {
                    AuthenticationToken authtoken = new AuthenticationToken();
                    authtoken.AuthToken = result.AuthToken;
                    obj._authToken = authtoken;
                }

                GetSendFaxContentInput contentInput = new GetSendFaxContentInput();
                contentInput.Authentication = auth;
                contentInput.AuthenticationToken = obj._authToken;

                contentInput.FaxId = faxID;
                contentInput.FaxContentSelector = _all;
                contentInput.FaxContentType = _pdf;
                GetSendFaxContentOutput contentOutput = null;
                try
                {
                    contentOutput = client.GetSendFaxContent(contentInput);
                }
                catch (Exception ex)
                {
                    throw ex.InnerException;
                }

                return contentOutput;
            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }
        }
        public static GetSendFaxContentOutput getSentFaxPDF(string faxID)
        {
            ReplixfaxService obj = new ReplixfaxService();
            System.ServiceModel.BasicHttpBinding bind = null;
            ReplixFaxPortClient client = null;
            Authentication auth = null;

            try
            {
                obj.initServiceCall(ref bind, ref client, ref auth);
                LoginInput loginInput = new LoginInput();
                loginInput.Authentication = auth;
                LoginOutput result = client.Login(loginInput);
                if (result.RequestStatus.StatusCode.Equals("0"))
                {
                    AuthenticationToken authtoken = new AuthenticationToken();
                    authtoken.AuthToken = result.AuthToken;
                    obj._authToken = authtoken;
                }
                GetSendFaxContentInput contentInput = new GetSendFaxContentInput();
                contentInput.Authentication = auth;
                contentInput.AuthenticationToken = obj._authToken;

                contentInput.FaxId = faxID;
                contentInput.FaxContentType = _pdf;
                GetSendFaxContentOutput contentOutput = null;
                try
                {
                    contentOutput = client.GetSendFaxContent(contentInput);
                }
                catch (Exception ex)
                {
                    throw ex.InnerException;
                }

                return contentOutput;

            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }
        }
        public static GetReceiveFaxContentOutput getReceivedtFaxPDF(string faxID)
        {
            ReplixfaxService obj = new ReplixfaxService();
            System.ServiceModel.BasicHttpBinding bind = null;
            ReplixFaxPortClient client = null;
            Authentication auth = null;
            try
            {
                obj.initServiceCall(ref bind, ref client, ref auth);
                LoginInput loginInput = new LoginInput();
                loginInput.Authentication = auth;
                LoginOutput result = client.Login(loginInput);
                if (result.RequestStatus.StatusCode.Equals("0"))
                {
                    AuthenticationToken authtoken = new AuthenticationToken();
                    authtoken.AuthToken = result.AuthToken;
                    obj._authToken = authtoken;
                }
                GetReceiveFaxContentInput contentInput = new GetReceiveFaxContentInput();
                contentInput.Authentication = auth;
                contentInput.AuthenticationToken = obj._authToken;

                contentInput.FaxId = faxID;
                contentInput.FaxContentType = _pdf;
                GetReceiveFaxContentOutput contentOutput = null;
                try
                {
                    contentOutput = client.GetReceiveFaxContent(contentInput);
                    markReceivedFax(faxID);
                }
                catch (Exception ex)
                {
                    throw ex.InnerException;
                }
                return contentOutput;

            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }
        }
        public static List<GetSendFaxContentOutput> getMultipleSentFaxPDF(string[] faxIdList)
        {
            ReplixfaxService obj = new ReplixfaxService();
            System.ServiceModel.BasicHttpBinding bind = null;
            ReplixFaxPortClient client = null;
            Authentication auth = null;

            try
            {
                obj.initServiceCall(ref bind, ref client, ref auth);
                LoginInput loginInput = new LoginInput();
                loginInput.Authentication = auth;
                LoginOutput result = client.Login(loginInput);
                if (result.RequestStatus.StatusCode.Equals("0"))
                {
                    AuthenticationToken authtoken = new AuthenticationToken();
                    authtoken.AuthToken = result.AuthToken;
                    obj._authToken = authtoken;
                }
                List<GetSendFaxContentOutput> contentOutputList = new List<GetSendFaxContentOutput>();
                for (var i = 0; i < faxIdList.Length; i++)
                {
                    GetSendFaxContentInput contentInput = new GetSendFaxContentInput();
                    contentInput.Authentication = auth;
                    contentInput.AuthenticationToken = obj._authToken;

                    contentInput.FaxId = faxIdList[i];
                    contentInput.FaxContentType = _pdf;


                    GetSendFaxContentOutput contentOutput = null;
                    try
                    {
                        contentOutput = client.GetSendFaxContent(contentInput);
                        contentOutputList.Add(contentOutput);
                    }
                    catch (Exception ex)
                    {
                        throw ex.InnerException;
                    }

                }
                return contentOutputList;
            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }
        }
        public static List<GetReceiveFaxContentOutput> getMultipleReceivedtFaxPDF(string[] faxIdList)
        {
            ReplixfaxService obj = new ReplixfaxService();
            System.ServiceModel.BasicHttpBinding bind = null;
            ReplixFaxPortClient client = null;
            Authentication auth = null;
            List<GetReceiveFaxContentOutput> contentOutputList = new List<GetReceiveFaxContentOutput>();

            try
            {
                obj.initServiceCall(ref bind, ref client, ref auth);
                LoginInput loginInput = new LoginInput();
                loginInput.Authentication = auth;
                LoginOutput result = client.Login(loginInput);
                if (result.RequestStatus.StatusCode.Equals("0"))
                {
                    AuthenticationToken authtoken = new AuthenticationToken();
                    authtoken.AuthToken = result.AuthToken;
                    obj._authToken = authtoken;
                }
                for (var i = 0; i < faxIdList.Length; i++)
                {
                    GetReceiveFaxContentInput contentInput = new GetReceiveFaxContentInput();
                    contentInput.Authentication = auth;
                    contentInput.AuthenticationToken = obj._authToken;

                    contentInput.FaxId = faxIdList[i];
                    contentInput.FaxContentType = _pdf;
                    GetReceiveFaxContentOutput contentOutput = null;
                    try
                    {
                        contentOutput = client.GetReceiveFaxContent(contentInput);
                        contentOutputList.Add(contentOutput);
                    }
                    catch (Exception ex)
                    {
                        throw ex.InnerException;
                    }

                    markReceivedFax(faxIdList[i]);
                }
                return contentOutputList;
            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }
        }
        public static void markReceivedFax(string FaxID)
        {
            ReplixfaxService obj = new ReplixfaxService();
            System.ServiceModel.BasicHttpBinding bind = null;
            ReplixFaxPortClient client = null;
            Authentication auth = null;

            try
            {
                obj.initServiceCall(ref bind, ref client, ref auth);

                LoginInput loginInput = new LoginInput();
                loginInput.Authentication = auth;
                LoginOutput result = client.Login(loginInput);
                if (result.RequestStatus.StatusCode.Equals("0"))
                {
                    AuthenticationToken authtoken = new AuthenticationToken();
                    authtoken.AuthToken = result.AuthToken;
                    obj._authToken = authtoken;
                }
                ModifyReceiveFaxInput input = new ModifyReceiveFaxInput();
                string faxToModify = FaxID;

                input.AuthenticationToken = obj._authToken;
                input.FaxId = faxToModify;

                input.Mark = _true;

                try
                {
                    client.ModifyReceiveFax(input);
                }
                catch (Exception ex)
                {
                    throw ex.InnerException;
                }
            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }

        }
        public QueryReceiveFaxOutput updateUnreadFaxes(string username)
        {
            ReplixfaxService obj = new ReplixfaxService();
            System.ServiceModel.BasicHttpBinding bind = null;
            ReplixFaxPortClient client = null;
            Authentication auth = null;

            try
            {
                obj.initServiceCall(ref bind, ref client, ref auth);
                LoginInput loginInput = new LoginInput();
                loginInput.Authentication = auth;
                LoginOutput result = client.Login(loginInput);
                if (result.RequestStatus.StatusCode.Equals("0"))
                {
                    AuthenticationToken authtoken = new AuthenticationToken();
                    authtoken.AuthToken = result.AuthToken;
                    obj._authToken = authtoken;

                }
            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }
            QueryReceiveFaxInput input = new QueryReceiveFaxInput();

            input.Authentication = auth;
            input.AuthenticationToken = obj._authToken;
            input.FaxUserId = username;


            input.Mark = _false;

            QueryReceiveFaxOutput output = null;
            try
            {
                output = client.QueryReceiveFax(input);
            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }
            if (output != null && output.FaxInfo != null)
            {
                return output;
            }

            return null;
        }
        public static QueryReceiveFaxOutput getUnreadFaxes(string username)
        {
            ReplixfaxService obj = new ReplixfaxService();
            System.ServiceModel.BasicHttpBinding bind = null;
            ReplixFaxPortClient client = null;
            Authentication auth = null;

            try
            {
                obj.initServiceCall(ref bind, ref client, ref auth);
                LoginInput loginInput = new LoginInput();
                loginInput.Authentication = auth;
                LoginOutput result = client.Login(loginInput);
                if (result.RequestStatus.StatusCode.Equals("0"))
                {
                    AuthenticationToken authtoken = new AuthenticationToken();
                    authtoken.AuthToken = result.AuthToken;
                    obj._authToken = authtoken;
                }
            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }
            QueryReceiveFaxInput input = new QueryReceiveFaxInput();

            input.Authentication = auth;
            input.AuthenticationToken = obj._authToken;
            input.FaxUserId = username;


            input.Mark = _false;

            QueryReceiveFaxOutput output = null;
            try
            {
                output = client.QueryReceiveFax(input);
            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }
            if (output != null && output.FaxInfo != null)
            {
                return output;
            }
            return null;
        }

        public static bool SendRpxFax(string username, string pass, string recepientNumber, string recipientName, string coverMsg, string[] DestinationSavePathhtml, string pdfFullPath, UserProfile profile)
        {
            ReplixfaxService obj = new ReplixfaxService();
            try
            {
                System.ServiceModel.BasicHttpBinding bind = null;
                ReplixFaxPortClient client = null;
                Authentication auth = null;
                obj.initServiceCall(ref bind, ref client, ref auth);


                SendFaxInput input = new SendFaxInput();
                input.Authentication = auth;
                // get recipients
                FaxRecipient[] recipients = new FaxRecipient[1];

                recipients[0] = new FaxRecipient();
                recipients[0].FaxNumber = recepientNumber;
                recipients[0].RcptName = recipientName;
                recipients[0].RcptTitle = "MTBC";
                recipients[0].RcptCompany = "MTBC";
                recipients[0].RcptVoice = "";
                recipients[0].RcptFax = "";

                // there must be at least one recipient, min already validated
                input.FaxRecipient = recipients;


                // add attachments
                Attachment[] attachments = null;
                attachments = new Attachment[DestinationSavePathhtml.Length];

                for (var n = 0; n < DestinationSavePathhtml.Length; n++)
                {
                    if (File.Exists(DestinationSavePathhtml[n]))
                    {
                        FileInfo fileinfo = new FileInfo(DestinationSavePathhtml[n]);
                        string extension = fileinfo.Extension.ToLower();

                        if (extension == "tiff" || extension == "tif" || extension == ".tiff" || extension == ".tif")
                        {

                            string gPath = "";
                            gPath = DestinationSavePathhtml[n];
                            // creation of the document with a certain size and certain margins  
                            iTextSharp.text.Document document = new iTextSharp.text.Document(iTextSharp.text.PageSize.A4, 0, 0, 0, 0);
                            // creation of the different writers  
                            iTextSharp.text.pdf.PdfWriter writer = iTextSharp.text.pdf.PdfWriter.GetInstance(document, new System.IO.FileStream(pdfFullPath, System.IO.FileMode.Create));
                            // load the tiff image and count the total pages  
                            System.Drawing.Bitmap bm = new System.Drawing.Bitmap(gPath);
                            int pageCount = bm.GetFrameCount(System.Drawing.Imaging.FrameDimension.Page);
                            document.Open();
                            iTextSharp.text.pdf.PdfContentByte cb = writer.DirectContent;
                            for (int k = 0; k < pageCount; ++k)
                            {
                                bm.SelectActiveFrame(System.Drawing.Imaging.FrameDimension.Page, k);
                                iTextSharp.text.Image img = iTextSharp.text.Image.GetInstance(bm, System.Drawing.Imaging.ImageFormat.Bmp);
                                if (img.DpiX != 0 && img.DpiY != 0 && img.DpiX != img.DpiY)
                                {
                                    Rectangle pdfPageSize = new Rectangle(0, 0, 1700, 2800);
                                    img.ScalePercent(100f);
                                    float percentX = (pdfPageSize.Width * 100) / img.ScaledWidth;
                                    float percentY = (pdfPageSize.Height * 100) / img.ScaledHeight;
                                    img.ScalePercent(percentX, percentY);
                                    img.ScaleAbsoluteHeight(img.ScaledHeight);
                                    img.ScaleAbsoluteWidth(img.ScaledWidth);
                                    img.SetAbsolutePosition(0, 0);
                                    iTextSharp.text.Rectangle pageRect = new iTextSharp.text.Rectangle(0, 0, img.ScaledWidth, img.ScaledHeight);
                                    document.SetPageSize(pageRect);
                                    document.SetMargins(0, 0, 0, 0);
                                    document.NewPage();
                                    cb.AddImage(img);
                                }
                                else
                                {
                                    img.ScalePercent(60f / img.DpiX * 100);
                                    img.ScaleAbsoluteHeight(bm.Height);
                                    img.ScaleAbsoluteWidth(bm.Width);
                                    img.SetAbsolutePosition(0, 0);
                                    cb.AddImage(img);
                                    document.NewPage();
                                }
                            }
                            document.Close();
                            FileInfo fn = new FileInfo(pdfFullPath);
                            attachments[n] = new Attachment();
                            attachments[n].ContentType = obj.ReturnExtension(fn.Extension.ToLower());
                            attachments[n].AttachmentContent = File.ReadAllBytes(pdfFullPath);

                            attachments[n].FileName = pdfFullPath;
                            attachments[n] = new Attachment();
                            attachments[n].ContentType = obj.ReturnExtension(fn.Extension.ToLower());
                            attachments[n].AttachmentContent = File.ReadAllBytes(pdfFullPath);

                            attachments[n].FileName = pdfFullPath;
                        }
                        else
                        {
                            attachments[n] = new Attachment();
                            attachments[n].ContentType = obj.ReturnExtension(fileinfo.Extension.ToLower());
                            attachments[n].AttachmentContent = File.ReadAllBytes(DestinationSavePathhtml[n]);
                            attachments[n].FileName = DestinationSavePathhtml[n];
                        }
                    }
                }
                if (attachments.Length > 0)
                {
                    input.Attachment = attachments;
                }

                // if the coverpage name is selected or if the default is selected, then assume
                // we need to pass all coverpage options


                input.CoverPageEnabled = _true;
                input.CoverMessage = coverMsg;

                if (username != "")
                {
                    input.FaxUserId = username;
                }

                input.NotifyEmailAddress = "adnanshah3@carecloud.com";

                input.NotifyFailed = _false;

                input.NotifySuccess = _false;

                input.NotifyFailedAttachFax = _false;

                input.DeleteAfterSend = _false;
                input.PageOrientation = _portrait;

                input.FaxQuality = _qualityHigh;

                input.FaxHeaderEnabled = _false;
                input.FaxHeader = "MTBC Fax Service";

                input.Priority = "10";
                input.TSI = profile.LastName + ", " + profile.FirstName;


                try
                {

                    SendFaxOutput output = client.SendFax(input);

                    if (output.FaxInfo != null && output.RequestStatus.StatusCode == "0")
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                }
                catch (Exception ex)
                {
                    throw ex.InnerException;
                }
            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }
        }


        private string ReturnExtension(string fileExtension)
        {
            //mime-type fix by Arshad Mehmood - 4187
            switch (fileExtension)
            {
                case ".htm":
                case ".html":
                case ".log":
                    return "text/HTML";
                case ".txt":
                    return "text/plain";
                case ".doc":
                    return "application/ms-word";
                case ".tiff":
                case ".tif":
                    return "image/tiff";
                case ".asf":
                    return "video/x-ms-asf";
                case ".avi":
                    return "video/avi";
                case ".zip":
                    return "application/zip";
                case ".xls":
                case ".csv":
                    return "application/vnd.ms-excel";
                case ".gif":
                    return "image/gif";
                case ".jpg":
                    return "image/jpeg";
                case "jpeg":
                    return "image/jpeg";
                case ".bmp":
                    return "image/bmp";
                case ".wav":
                    return "audio/wav";
                case ".mp3":
                    return "audio/mpeg3";
                case ".mpg":
                case "mpeg":
                    return "video/mpeg";
                case ".rtf":
                    return "application/rtf";
                case ".asp":
                    return "text/asp";
                case ".pdf":
                    return "application/pdf";
                case ".fdf":
                    return "application/vnd.fdf";
                case ".ppt":
                    return "application/mspowerpoint";
                case ".dwg":
                    return "image/vnd.dwg";
                case ".msg":
                    return "application/msoutlook";
                case ".xml":
                case ".sdxl":
                    return "application/xml";
                case ".xdp":
                    return "application/vnd.adobe.xdp+xml";
                default:
                    return "application/octet-stream";
            }
        }


    }
}
