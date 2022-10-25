using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FOX.ExternalServices.SundrySmsService;
using Org.BouncyCastle.Asn1.Ocsp;
using System.Web;
using System.Net;
using System.Configuration;

namespace FOX.ExternalServices
{
    public static class SmsService
    {
        // FOX.ExternalServices.TelenorSmsService
        public static string NJSmsService(string CellPhone, string SmsBody)
        {
            
            const System.Security.Authentication.SslProtocols _Tls12 = (System.Security.Authentication.SslProtocols)0x00000C00;
            const SecurityProtocolType Tls12 = (SecurityProtocolType)_Tls12;
            ServicePointManager.SecurityProtocol = Tls12;

            Sundry obj = new ExternalServices.SundrySmsService.Sundry();
            ValidationSoapHeader header = new ValidationSoapHeader();
            string statusValue = "phrInprogress";
            string userID = "6KjP6ha7N", Password = ConfigurationManager.AppSettings["ExternalServiceSMSPassword"];
            header.ValidUserID = userID;
            header.ValidPassword = Password;
            header.DeviceInfo = HttpContext.Current.Request.UserAgent;
            header.ApplicationName = "Fox Por";
            header.MachineName = "ClnjWeb";
            obj.ValidationSoapHeaderValue = header;
            CSSendSMS abcd = new CSSendSMS();
            abcd.smsBody = SmsBody;
            abcd.userPhone = CellPhone;
            if (CellPhone != "")
            {
                try
                {
                    System.Data.DataTable returningTest = obj.sendSMS(abcd);
                    /*SMS Code Here End*/
                    for (int i = 0; i < returningTest.Rows.Count; i++)
                    {
                        statusValue = (returningTest.Rows[i]["Error"]).ToString();
                        if (statusValue == "")
                        {
                            statusValue = "Success";
                        }
                        else
                        {
                            statusValue = "Error";
                        }
                    }
                }
                catch (Exception)
                {
                    return statusValue;
                }

            }
            return statusValue;
        }
        public static string LOCALSmsService(string Reciepent_Name, string PhoneNo, string SMSText, string Team)
        {

            try
            {
                TelenorSmsService.Service1 obj = new FOX.ExternalServices.TelenorSmsService.Service1();
                var Result = obj.SendTelenorSMS(Reciepent_Name, PhoneNo, SMSText, Team);
                return Result;
            }
            catch (Exception)
            {
                return null;
            }

        }
        /// <summary>
        /// NJ SMS Service is used for sending SMS to USA Clients.
        /// </summary>
        /// <param name="number"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public static string SMSTwilio(string number, string message)
        {
            try
            {
                string SmsUrl = "http://clnjacu-srv1/ACU_SMS/ACU_Twilio_Service.asmx/ACUSendSMS";
                WebRequest request = WebRequest.Create($"{SmsUrl}?Number={number}&Message={message}");
                request.Method = "GET";
                WebResponse response = request.GetResponse();
                return "Success";
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
