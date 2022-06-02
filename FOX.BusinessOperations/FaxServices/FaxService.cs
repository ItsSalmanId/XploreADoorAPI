using FOX.DataEntities.Model.Fax;
using FOX.ExternalServices.Softlinks;
using FOX.DataModels.Context;
using FOX.DataModels.GenericRepository;
using FOX.DataModels.Models.Security;
using FOX.DataModels.Models.Settings.Practice;
using iTextSharp.text;
using iTextSharp.text.pdf;
using NReco;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using FOX.BusinessOperations.CommonServices;

namespace FOX.BusinessOperations.FaxServices
{
    public class FaxService : IFaxService
    {
        private readonly GenericRepository<InterFaxDetail> _InterFaxRep;
        private readonly DbContextSecurity _DbContextSecurity = new DbContextSecurity();

        public FaxService()
        {
            _InterFaxRep = new GenericRepository<InterFaxDetail>(_DbContextSecurity);
        }
        public ReceivedFaxResponse GetReceivedFax(FaxRequest objRequest, UserProfile profile)
        {
            ReceivedFaxResponse response = new ReceivedFaxResponse();
            try
            {
                string username = GetFaxUserDetails(profile.PracticeCode).INTERFAX_USERNAME;
                return ReplixfaxService.getRecievedFaxes(username, objRequest.fromDate, objRequest.toDate, objRequest.faxStatus, objRequest.faxID, objRequest.resultLimit, objRequest.nextRef);
            }
            catch (Exception)
            {
                return response;
            }

        }

        public ReceivedFaxResponse GetSentFax(FaxRequest objRequest, UserProfile profile)
        {
            ReceivedFaxResponse response = new ReceivedFaxResponse();
            try
            {
                string username = GetFaxUserDetails(profile.PracticeCode).INTERFAX_USERNAME;
                response = ReplixfaxService.getSentFaxes(username, objRequest.fromDate, objRequest.toDate, objRequest.faxStatus, objRequest.faxID, objRequest.resultLimit, objRequest.nextRef);
                return response;
            }
            catch (Exception)
            {
                return response;
            }
        }

        public string getSentFaxContent(string faxID, UserProfile profile)
        {
            try
            {
                Guid guid;
                guid = Guid.NewGuid();
                string newGuid = guid.ToString();

                var contentOutput = ReplixfaxService.getSentFaxPDF(faxID);
                if (contentOutput.RequestStatus.StatusCode.CompareTo("0") == 0)
                {
                    // save content to a file
                    if (contentOutput.FaxContent.ImageContent != null)
                    {

                        if (contentOutput.FaxContent.ImageContent.Length > 0)
                        {
                            var directoryPath = DocumentHelper.GetDocumentDirectoryPhysicalPathByFolder(profile, "TempFaxFolder/Fax-Content/" + guid);
                            var path = HttpContext.Current.Server.MapPath(@"~/" + directoryPath);

                            if (!Directory.Exists(path))
                            {
                                Directory.CreateDirectory(path);
                            }


                            if ((path.EndsWith("\\") == false))
                            {
                                path = path + "\\";
                            }
                            path = path + contentOutput.FaxContent.FileName;
                            directoryPath = directoryPath + contentOutput.FaxContent.FileName;

                            BinaryWriter binWriter =
                                new BinaryWriter(File.Open(path, FileMode.Create));
                            binWriter.Write(contentOutput.FaxContent.ImageContent);
                            binWriter.Flush();
                            binWriter.Close();
                            return directoryPath;
                        }
                        else
                        {
                            string msg = "StatusCode = " + contentOutput.RequestStatus.StatusCode + " Message= " + contentOutput.RequestStatus.StatusText;
                            return msg;
                        }
                    }
                    else
                    {
                        string msg = "StatusCode = " + contentOutput.RequestStatus.StatusCode + " Message= " + contentOutput.RequestStatus.StatusText;
                        return msg;
                    }
                }
                else
                {
                    string msg = "StatusCode = " + contentOutput.RequestStatus.StatusCode + " Message= " + contentOutput.RequestStatus.StatusText;
                    return msg;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        public string forwardSentFaxContent(string faxID, UserProfile profile)
        {
            try
            {
                Guid guid;
                guid = Guid.NewGuid();
                string newGuid = guid.ToString();

                var contentOutput = ReplixfaxService.getSentFaxPDF(faxID);
                if (contentOutput.RequestStatus.StatusCode.CompareTo("0") == 0)
                {
                    // save content to a file
                    if (contentOutput.FaxContent.ImageContent != null)
                    {

                        if (contentOutput.FaxContent.ImageContent.Length > 0)
                        {
                            var directoryPath = DocumentHelper.GetDocumentDirectoryPhysicalPathByFolder(profile, "TempFaxFolder/Fax-Content/" + guid);
                            var path = HttpContext.Current.Server.MapPath(@"~/" + directoryPath);

                            if (!Directory.Exists(path))
                            {
                                Directory.CreateDirectory(path);
                            }


                            if ((path.EndsWith("\\") == false))
                            {
                                path = path + "\\";
                            }
                            path = path + contentOutput.FaxContent.FileName;
                            directoryPath = directoryPath + contentOutput.FaxContent.FileName;

                            BinaryWriter binWriter =
                                new BinaryWriter(File.Open(path, FileMode.Create));
                            binWriter.Write(contentOutput.FaxContent.ImageContent);
                            binWriter.Flush();
                            binWriter.Close();
                            return path;
                        }
                        else
                        {
                            string msg = "StatusCode = " + contentOutput.RequestStatus.StatusCode + " Message= " + contentOutput.RequestStatus.StatusText;
                            return msg;
                        }
                    }
                    else
                    {
                        string msg = "StatusCode = " + contentOutput.RequestStatus.StatusCode + " Message= " + contentOutput.RequestStatus.StatusText;
                        return msg;
                    }
                }
                else
                {
                    string msg = "StatusCode = " + contentOutput.RequestStatus.StatusCode + " Message= " + contentOutput.RequestStatus.StatusText;
                    return msg;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public string getReceivedFaxContent(string faxID, UserProfile profile)
        {
            try
            {
                Guid guid;
                guid = Guid.NewGuid();
                string newGuid = guid.ToString();

                var contentOutput = ReplixfaxService.getReceivedtFaxPDF(faxID);
                if (contentOutput.RequestStatus.StatusCode.CompareTo("0") == 0)
                {
                    // save content to a file
                    if (contentOutput.FaxContent.ImageContent != null)
                    {

                        if (contentOutput.FaxContent.ImageContent.Length > 0)
                        {
                            var directoryPath = DocumentHelper.GetDocumentDirectoryPhysicalPathByFolder(profile, "TempFaxFolder/Fax-Content/" + guid);
                            var path = HttpContext.Current.Server.MapPath(@"~/" + directoryPath);

                            if (!Directory.Exists(path))
                            {
                                Directory.CreateDirectory(path);
                            }


                            if ((path.EndsWith("\\") == false))
                            {
                                path = path + "\\";
                            }
                            path = path + contentOutput.FaxContent.FileName;
                            directoryPath = directoryPath + contentOutput.FaxContent.FileName;

                            BinaryWriter binWriter =
                                new BinaryWriter(File.Open(path, FileMode.Create));
                            binWriter.Write(contentOutput.FaxContent.ImageContent);
                            binWriter.Flush();
                            binWriter.Close();
                            return directoryPath;
                        }
                        else
                        {
                            string msg = "StatusCode = " + contentOutput.RequestStatus.StatusCode + " Message= " + contentOutput.RequestStatus.StatusText;
                            return msg;
                        }
                    }
                    else
                    {
                        string msg = "StatusCode = " + contentOutput.RequestStatus.StatusCode + " Message= " + contentOutput.RequestStatus.StatusText;
                        return msg;
                    }
                }
                else
                {
                    string msg = "StatusCode = " + contentOutput.RequestStatus.StatusCode + " Message= " + contentOutput.RequestStatus.StatusText;
                    return msg;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        public string forwardReceivedFaxContent(string faxID, UserProfile profile)
        {
            try
            {
                Guid guid;
                guid = Guid.NewGuid();
                string newGuid = guid.ToString();

                var contentOutput = ReplixfaxService.getReceivedtFaxPDF(faxID);
                if (contentOutput.RequestStatus.StatusCode.CompareTo("0") == 0)
                {
                    // save content to a file
                    if (contentOutput.FaxContent.ImageContent != null)
                    {

                        if (contentOutput.FaxContent.ImageContent.Length > 0)
                        {
                            var directoryPath = DocumentHelper.GetDocumentDirectoryPhysicalPathByFolder(profile, "TempFaxFolder/Fax-Content/" + guid);
                            var path = HttpContext.Current.Server.MapPath(@"~/" + directoryPath);

                            if (!Directory.Exists(path))
                            {
                                Directory.CreateDirectory(path);
                            }


                            if ((path.EndsWith("\\") == false))
                            {
                                path = path + "\\";
                            }
                            path = path + contentOutput.FaxContent.FileName;
                            directoryPath = directoryPath + contentOutput.FaxContent.FileName;

                            BinaryWriter binWriter =
                                new BinaryWriter(File.Open(path, FileMode.Create));
                            binWriter.Write(contentOutput.FaxContent.ImageContent);
                            binWriter.Flush();
                            binWriter.Close();
                            return path;
                        }
                        else
                        {
                            string msg = "StatusCode = " + contentOutput.RequestStatus.StatusCode + " Message= " + contentOutput.RequestStatus.StatusText;
                            return msg;
                        }
                    }
                    else
                    {
                        string msg = "StatusCode = " + contentOutput.RequestStatus.StatusCode + " Message= " + contentOutput.RequestStatus.StatusText;
                        return msg;
                    }
                }
                else
                {
                    string msg = "StatusCode = " + contentOutput.RequestStatus.StatusCode + " Message= " + contentOutput.RequestStatus.StatusText;
                    return msg;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public string getMultipleSentFax(string[] faxIdList, UserProfile profile)
        {
            try
            {
                Guid guid;
                guid = Guid.NewGuid();
                string newGuid = guid.ToString();
                var OnlyPath = "";
                var path = "";
                var directoryPath = "";
                var contentOutputList = ReplixfaxService.getMultipleSentFaxPDF(faxIdList);
                foreach (var contentOutput in contentOutputList)
                {
                    if (contentOutput.RequestStatus.StatusCode.CompareTo("0") == 0)
                    {
                        // save content to a file
                        if (contentOutput.FaxContent.ImageContent != null)
                        {

                            if (contentOutput.FaxContent.ImageContent.Length > 0)
                            {
                                directoryPath = DocumentHelper.GetDocumentDirectoryPhysicalPathByFolder(profile, "TempFaxFolder/Fax-Content/" + guid);
                                path = HttpContext.Current.Server.MapPath(@"~/" + directoryPath);
                                OnlyPath = path;

                                if (!Directory.Exists(path))
                                {
                                    Directory.CreateDirectory(path);
                                }


                                if ((path.EndsWith("\\") == false))
                                {
                                    path = path + "\\";
                                }
                                path = path + contentOutput.FaxContent.FileName;
                                directoryPath = directoryPath + contentOutput.FaxContent.FileName;

                                BinaryWriter binWriter =
                                    new BinaryWriter(File.Open(path, FileMode.Create));
                                binWriter.Write(contentOutput.FaxContent.ImageContent);
                                binWriter.Flush();
                                binWriter.Close();

                            }
                        }
                    }
                }
                if (Directory.Exists(OnlyPath))
                {
                    string[] files = Directory.GetFiles(OnlyPath);
                    string mergePath = string.Empty;
                    mergePath = DocumentHelper.GetDocumentDirectoryPhysicalPathByFolder(profile, "TempFaxFolder/Fax-Content/" + guid);
                    var serverMergePath = HttpContext.Current.Server.MapPath(@"~/" + mergePath);
                    // mergePath = "\\\\" + _documentIP + "\\" + _directory + "\\" + PracticeCode + "\\Fax-Contents\\TempFaxFolder\\" + Username + "\\" + newGuid;
                    if (!Directory.Exists(serverMergePath))
                    {
                        Directory.CreateDirectory(serverMergePath);
                    }
                    mergePath += "New-MultipleMergeFile.pdf";
                    serverMergePath += "New-MultipleMergeFile.pdf";


                    Document document = new Document();
                    PdfCopy copy = new PdfCopy(document, new FileStream(serverMergePath, FileMode.Create));
                    copy.SetMergeFields();
                    document.Open();
                    foreach (var fileName in files)
                    {
                        PdfReader reader = new PdfReader(fileName);
                        copy.AddDocument(reader);
                        //reader.Close();
                    }

                    document.Close();
                    directoryPath = mergePath;
                }
                return directoryPath;
            }
            catch (Exception)
            {

                throw;
            }

        }

        public string getMultipleReceivedFax(string[] faxIdList, UserProfile profile)
        {
            try
            {
                Guid guid;
                guid = Guid.NewGuid();
                string newGuid = guid.ToString();
                var path = "";
                var OnlyPath = "";
                var directoryPath = "";
                var contentOutputList = ReplixfaxService.getMultipleReceivedtFaxPDF(faxIdList);
                foreach (var contentOutput in contentOutputList)
                {
                    if (contentOutput.RequestStatus.StatusCode.CompareTo("0") == 0)
                    {
                        // save content to a file
                        if (contentOutput.FaxContent.ImageContent != null)
                        {

                            if (contentOutput.FaxContent.ImageContent.Length > 0)
                            {
                                directoryPath = DocumentHelper.GetDocumentDirectoryPhysicalPathByFolder(profile, "TempFaxFolder/Fax-Content/" + guid);
                                path = HttpContext.Current.Server.MapPath(@"~/" + directoryPath);
                                OnlyPath = path;
                                if (!Directory.Exists(path))
                                {
                                    Directory.CreateDirectory(path);
                                }


                                if ((path.EndsWith("\\") == false))
                                {
                                    path = path + "\\";
                                }
                                path = path + contentOutput.FaxContent.FileName;
                                directoryPath = directoryPath + contentOutput.FaxContent.FileName;

                                BinaryWriter binWriter =
                                    new BinaryWriter(File.Open(path, FileMode.Create));
                                binWriter.Write(contentOutput.FaxContent.ImageContent);
                                binWriter.Flush();
                                binWriter.Close();

                            }
                        }
                    }
                }
                if (Directory.Exists(OnlyPath))
                {
                    string[] files = Directory.GetFiles(OnlyPath);
                    string mergePath = string.Empty;
                    mergePath = DocumentHelper.GetDocumentDirectoryPhysicalPathByFolder(profile, "TempFaxFolder/Fax-Content/" + guid);
                    // mergePath = "\\\\" + _documentIP + "\\" + _directory + "\\" + PracticeCode + "\\Fax-Contents\\TempFaxFolder\\" + Username + "\\" + newGuid;
                    var serverMergePath = HttpContext.Current.Server.MapPath(@"~/" + mergePath);

                    if (!Directory.Exists(serverMergePath))
                    {
                        Directory.CreateDirectory(serverMergePath);
                    }
                    mergePath += "New-MultipleMergeFile.pdf";
                    serverMergePath += "New-MultipleMergeFile.pdf";


                    Document document = new Document();
                    PdfCopy copy = new PdfCopy(document, new FileStream(serverMergePath, FileMode.Create));
                    copy.SetMergeFields();
                    document.Open();
                    foreach (var fileName in files)
                    {
                        PdfReader reader = new PdfReader(fileName);
                        copy.AddDocument(reader);
                        reader.Close();
                    }

                    document.Close();
                    directoryPath = mergePath;
                }
                return directoryPath;
            }
            catch (Exception)
            {

                throw;
            }

        }

        public InterFaxDetail GetFaxUserDetails(long practiceCode)
        {
            var pracCode = new SqlParameter("PRACTICE_CODE", SqlDbType.BigInt) { Value = practiceCode };
            var faxDetails = SpRepository<InterFaxDetail>.GetSingleObjectWithStoreProcedure(@"exec AF_PROC_GETINTERFAXDETAILS @PRACTICE_CODE", pracCode);
            return faxDetails;
        }

        public string SendFax(string[] Recp_Fax, string[] RecpName, string[] CoverLetter, string FileName, string FilePath, string Subject, bool isCallFromFax, UserProfile profile)
        {
            try
            {
                string _faxerror = "";
                bool isOK = false;
                string isok2 = "";
                long st = -24;

                string username_new = "";
                string password_new = "";
                string Fax_Num_new = "";
                string Fax_Company_new = "";

                string Prac_Code = profile.PracticeCode.ToString();
                long _Prac_Code = long.Parse(Prac_Code);
                if (_Prac_Code == 1011163)
                {
                    _Prac_Code = 1012714;
                }
                var result = GetFaxUserDetails(_Prac_Code);
                if (result != null)
                {
                    username_new = result.INTERFAX_USERNAME.ToString();
                    password_new = result.INTERFAX_PASSWORD.ToString();
                    Fax_Num_new = result.INTERFAX_NO.ToString();
                    Fax_Company_new = result.ACC_COMPANY.ToString();
                }
                for (int i = 0; i < Recp_Fax.Length; i++)
                {
                    string faxnumberME = Recp_Fax[i];
                    string recipientName = RecpName[i];
                    faxnumberME = Regex.Replace(faxnumberME, @"[^\d]", "");
                    #region Faxing logic
                    string notes = string.Empty;

                    if (CoverLetter?.Length > i)
                    {
                        notes = CoverLetter[i];
                    }

                    string[] attachfile;
                    string _subject = Subject;
                    string _PracCode = Convert.ToString(profile.PracticeCode);
                    attachfile = new string[1];

                    //////////////////////////////////
                    string practiceCode = profile.PracticeCode.ToString();
                    string savePath = string.Empty;

                    string _UserName = Convert.ToString(profile.UserName);
                    string timeStamp = DateTime.Now.ToString("ddMMyyyyHHmmss");
                    if (isCallFromFax == false)
                    {
                        //savePath = HttpContext.Current.Server.MapPath(@"~/" + FilePath + "\\" + FileName);
                        savePath = FilePath + FileName;
                        //savePath = FilePath;
                        attachfile[0] = savePath;
                    }
                    else
                    {
                        attachfile[0] = FilePath + "\\" + FileName;
                        savePath = FilePath + "\\" + timeStamp + "_1" + ".pdf";
                        string tempFolder = string.Empty;
                        tempFolder = FilePath + "\\Faxes\\";
                        if (!Directory.Exists(tempFolder))
                        {
                            Directory.CreateDirectory(tempFolder);
                        }
                    }

                    string DestinationSavePathhtml = "", DestinationSavePathhtmlPDF = "";
                    if (isCallFromFax == true)
                    {
                        DestinationSavePathhtml = FilePath + "\\Faxes\\" + timeStamp + "Destination.html";
                        DestinationSavePathhtmlPDF = FilePath + "\\Faxes\\" + timeStamp + "Destination.pdf";
                    }
                    else
                    {
                        var extension = savePath.LastIndexOf(".");
                        var filePath = savePath.Substring(0, extension) + ".html";
                        DestinationSavePathhtml = filePath;
                        DestinationSavePathhtmlPDF = savePath;
                    }
                    if (Fax_Company_new == "@rcfax.com" || Fax_Company_new == "@rpxfax.com" && CoverLetter?.Length > i)//efax issue
                    {
                        string EHTMLCode = "";
                        EHTMLCode += "<BASE href='http://TalkEHR.com'/>";
                        //EHTMLCode += "<HTML><HEAD><title>Fax</title>";
                        //EHTMLCode += "</HEAD><body>";
                        EHTMLCode += notes.Trim();
                        EHTMLCode = EHTMLCode.Replace("<BR>", "<br />");
                        EHTMLCode = EHTMLCode.Replace("<br>", "<br />");
                        //EHTMLCode += "</body></HTML>";
                        WriteHtmlFile(DestinationSavePathhtml, EHTMLCode);
                    }

                    //WEBEHR_Labs_New_Lab_Order objFaxException = new WEBEHR_Labs_New_Lab_Order();
                    switch (Fax_Company_new)
                    {
                        //case "@metrofax.com":
                        //    faxnum = "1" + faxnumberME + Fax_Company_new;
                        //    try
                        //    {
                        //        isOK = SendFax(username_new, password_new, faxnum, attachfile, Subject, notes, DestinationSavePathhtmlPDF);
                        //        if (File.Exists(DestinationSavePathhtmlPDF))
                        //            File.Delete(DestinationSavePathhtmlPDF);
                        //    }
                        //    catch (Exception ex)
                        //    {
                        //        isok2 = "false"; _faxerror = ex.Message;
                        //        //objFaxException.dumpFaxException(ex, _documentViewFullPath);
                        //    }
                        //    break;
                        //case "@efaxsend.com":
                        //    faxnum = "1" + faxnumberME + Fax_Company_new;
                        //    try
                        //    {
                        //        isOK = SendFax(username_new, password_new, faxnum, attachfile, Subject, notes, DestinationSavePathhtmlPDF);//efax issue
                        //        if (File.Exists(DestinationSavePathhtmlPDF))
                        //            File.Delete(DestinationSavePathhtmlPDF);//efax issue
                        //    }
                        //    catch (Exception ex)
                        //    {
                        //        isok2 = "false"; _faxerror = ex.Message;
                        //        //objFaxException.dumpFaxException(ex, _documentViewFullPath);
                        //    }
                        //    break;
                        //case "@rcfax.com":
                        //    faxnum = "1" + faxnumberME + Fax_Company_new;
                        //    try
                        //    {
                        //        isOK = SendFax(username_new, password_new, faxnum, attachfile, Subject, notes, DestinationSavePathhtml);
                        //        if (File.Exists(DestinationSavePathhtml))
                        //            File.Delete(DestinationSavePathhtml);
                        //    }
                        //    catch (Exception ex)
                        //    {
                        //        isok2 = "false"; _faxerror = ex.Message;
                        //        //objFaxException.dumpFaxException(ex, _documentViewFullPath);
                        //    }
                        //    break;
                        case "@rpxfax.com":
                            try
                            {
                                int arryLength = attachfile.Length;
                                if (CoverLetter != null)
                                {
                                    arryLength++;
                                }
                                var newAttachfile = new string[arryLength]; //new string[attachfile.Length + 1];
                                attachfile.CopyTo(newAttachfile, arryLength - 1);
                                if (CoverLetter != null)
                                {
                                    newAttachfile[0] = DestinationSavePathhtml;
                                }

                                isOK = ReplixfaxService.SendRpxFax(username_new, password_new, faxnumberME, recipientName, Subject, newAttachfile, savePath, profile);
                                if (!isOK)
                                {
                                    return "failed";
                                    //throw new Exception("SendRpxFax method returned false.");
                                }
                                if (isCallFromFax == true)
                                {
                                    if (File.Exists(savePath))
                                        File.Delete(savePath);
                                    if (File.Exists(DestinationSavePathhtml))
                                        File.Delete(DestinationSavePathhtml);
                                    if (File.Exists(DestinationSavePathhtmlPDF))
                                        File.Delete(DestinationSavePathhtmlPDF);
                                }
                            }
                            catch (Exception ex)
                            {
                                isok2 = "false"; _faxerror = ex.Message;
                                //objFaxException.dumpFaxException(ex, _documentViewFullPath);
                            }
                            break;
                        //case "@interfax.com":
                        //    try
                        //    {
                        //        string RecpFaxNum = "+1" + Recp_Fax[i];
                        //        replixfaxService.se //checking cover letter
                        //    }
                        //    catch (Exception ex)
                        //    {
                        //        //objFaxException.dumpFaxException(ex, _documentViewFullPath);
                        //    }
                        //    break;
                        default:
                            st = -23;
                            break;
                    }
                    #endregion
                }
                return st.ToString() + "," + isOK + "," + isok2 + "," + username_new + "," + Fax_Company_new + "," + _faxerror;
            }
            catch (Exception)
            {
                return "failed";
                //throw ex;
            }
        }

        public bool deleteSentFax(string[] faxIdList)
        {
            return ReplixfaxService.deleteSentFax(faxIdList);
        }

        public bool deleteReceivedFax(string[] faxIdList)
        {
            return ReplixfaxService.deleteReceivedFax(faxIdList);
        }
        public void WriteHtmlFile(string filePath, string contents)
        {

            try
            {
                StreamWriter StreamWriter = new StreamWriter(filePath);
                StreamWriter.WriteLine(contents);
                StreamWriter.Close();

            }
            catch (Exception)
            {

            }
        }

        public static string saveTempDoc(string html, string SaveDocPath)
        {
            string Generaltext = "<html><head><style type=''>table, tr, td, th, tbody, thead, tfoot {page-break-inside: avoid !important;z-index:100;}</style></head><body><div><table><tr><td>" + html + "</td></tr></div></table></body></html>";
            NReco.PdfGenerator.PageSize size = new NReco.PdfGenerator.PageSize();
            size = NReco.PdfGenerator.PageSize.A4;

            NReco.PdfGenerator.HtmlToPdfConverter pdf = new NReco.PdfGenerator.HtmlToPdfConverter();
            pdf.Size = size;
            pdf.Orientation = NReco.PdfGenerator.PageOrientation.Portrait;
            pdf.Margins = new NReco.PdfGenerator.PageMargins { Top = 20, Bottom = 13, Left = 13, Right = 13 };
            var pdfBytes = pdf.GeneratePdf(Generaltext);
            File.WriteAllBytes(SaveDocPath, pdfBytes);  //if you want to write on your hard disk directly
            return SaveDocPath;
        }

        public bool saveFaxSetting(InterFaxDetail objSetting, UserProfile profile)
        {
            objSetting.PRACTICE_CODE = profile.PracticeCode;
            var faxSetting = _InterFaxRep.Get(x => x.PRACTICE_CODE == objSetting.PRACTICE_CODE);
            if (faxSetting == null)
            {
                _InterFaxRep.Insert(objSetting);
            }
            else
            {
                faxSetting.ACC_COMPANY = objSetting.ACC_COMPANY;
                faxSetting.INTERFAX_NO = objSetting.INTERFAX_NO;
                faxSetting.INTERFAX_PASSWORD = objSetting.INTERFAX_PASSWORD;
                faxSetting.INTERFAX_USERNAME = objSetting.INTERFAX_USERNAME;
                _InterFaxRep.Update(faxSetting);
            }
            _InterFaxRep.Save();
            return true;
        }

    }


}
