using System;
using System.IO;
using System.Web.Http.Filters;
using Newtonsoft.Json;
using FOX.DataModels.Models.Exceptions;
using FOX.BusinessOperations.CommonServices;
using FOX.BusinessOperations.CommonService;

namespace FoxRehabilitationAPI.Filters
{
    public class ExceptionHandlingFilter : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext context)
        {
            try
            {
                var excpParam = JsonConvert.SerializeObject(context.ActionContext.ActionArguments.Values);
                var uri = context.ActionContext.Request.RequestUri.OriginalString;
                var excpMsg = context.Exception.Message;
                if (excpMsg.Contains("it is being used by another process"))
                {
                    return;
                }

                if (excpMsg.Contains("The context cannot be used while the model is being created. This exception may be thrown if the context is used inside the OnModelCreating method"))
                {
                    return;
                }
                var excpStackTrace = context.Exception.StackTrace;
                var excpInnerMessage = ((context.Exception.InnerException != null && context.Exception.InnerException.Message != null) ? (context.Exception.InnerException.Message.ToLower().Contains("inner exception") ? context.Exception.InnerException.InnerException.Message : context.Exception.InnerException.Message) : "NULL");

                if (context.Exception is BusinessException)
                {
                    //string directory = System.Web.HttpContext.Current.Server.MapPath(AppConfiguration.ErrorLogPath + "\\FoxBussinessErrors");
                    string directory = System.Web.HttpContext.Current.Server.MapPath("\\FoxBussinessErrors");
                    if (!Directory.Exists(directory))
                    {
                        Directory.CreateDirectory(directory);
                    }
                    string filePath = directory + "\\errors_" + DateTime.Now.Date.ToString("MM-dd-yyyy") + ".txt";
                    using (StreamWriter writer = new StreamWriter(filePath, true))
                    {
                        writer.WriteLine("Message: " + excpMsg + Environment.NewLine + Environment.NewLine + "URI: " + uri + Environment.NewLine + Environment.NewLine + "Request parameters: " + excpParam + Environment.NewLine + Environment.NewLine + "StackTrace: " + excpStackTrace + Environment.NewLine + Environment.NewLine +
                           "///------------------Inner Exception------------------///" + Environment.NewLine + excpInnerMessage + Environment.NewLine +
                           "Date: " + DateTime.Now.ToString() + Environment.NewLine + Environment.NewLine + "-------------------------------------------------------||||||||||||---End Current Exception---||||||||||||||||-------------------------------------------------------" + Environment.NewLine);
                        writer.Close();
                    }
                }
                //Log Critical errors
                //string directoryOther = System.Web.HttpContext.Current.Server.MapPath(AppConfiguration.ErrorLogPath + "\\FoxCriticalErrors");
                string directoryOther = System.Web.HttpContext.Current.Server.MapPath("\\FoxCriticalErrors");
                if (!Directory.Exists(directoryOther))
                {
                    Directory.CreateDirectory(directoryOther);
                }
                string filePathOther = directoryOther + "\\errors_" + DateTime.Now.Date.ToString("MM-dd-yyyy") + ".txt";
                try
                {
                    using (StreamWriter writer = new StreamWriter(filePathOther, true))
                    {
                        writer.WriteLine("Message: " + excpMsg + Environment.NewLine + Environment.NewLine + "URI:  " + uri + Environment.NewLine + Environment.NewLine + "Request parameters: " + excpParam + Environment.NewLine + Environment.NewLine + "StackTrace: " + excpStackTrace + Environment.NewLine + Environment.NewLine +
                           "///------------------Inner Exception------------------///" + Environment.NewLine + excpInnerMessage + "" + Environment.NewLine +
                           "Date: " + DateTime.Now.ToString() + Environment.NewLine + Environment.NewLine + "-------------------------------------------------------||||||||||||---End Current Exception---||||||||||||||||-------------------------------------------------------" + Environment.NewLine);
                        writer.Close();
                    }
                }
                catch (Exception ex)
                {
                    Helper.SendEmailOnException(ex.Message, ex.ToString(), "Exception occurred in Exception Filter");
                }
                var expmsg = Environment.NewLine + "URI:  " + uri + Environment.NewLine + Environment.NewLine + "Request parameters: " + excpParam + Environment.NewLine + Environment.NewLine + "StackTrace: " + excpStackTrace + Environment.NewLine + Environment.NewLine +
                             "///------------------Inner Exception------------------///" + Environment.NewLine + Environment.NewLine + excpInnerMessage + Environment.NewLine + Environment.NewLine + "Date: " + DateTime.Now.ToString()
                             + Environment.NewLine + Environment.NewLine + "-------------------------------------------------------||||||||||||---End Current Exception---||||||||||||||||------------------------------" +
                             "-------------------------" + Environment.NewLine;

                Helper.SendExceptionsEmail(excpMsg, expmsg.ToString(), "Exception occurred in Exception Filter");
            }
            catch (Exception ex)
            {
                Helper.SendEmailOnException(ex.Message, ex.ToString(), "Exception occurred in Exception Filter");
            }
        }
    }
}