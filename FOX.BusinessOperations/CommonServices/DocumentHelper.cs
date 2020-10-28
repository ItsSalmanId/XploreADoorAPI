using FOX.DataModels.Models.CommonModel;
using FOX.DataModels.Models.Security;
using iTextSharp.text;
using iTextSharp.text.pdf;
using NReco.PdfGenerator;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace FOX.BusinessOperations.CommonServices
{
    public class DocumentHelper
    {
        public static string GetPracticeDocumentDirectoryPath(UserProfile profile, bool IsTempPath = true)
        {
            Guid guid = Guid.NewGuid();
            string DirectoryPath = "";
            if (IsTempPath)
            {
                return DirectoryPath = Path.Combine(HttpContext.Current.Server.MapPath(@"~/" + profile.PracticeDocumentDirectory + "/" + profile.PracticeCode + "/Fox/TempAttached/"), guid.ToString());
            }
            return DirectoryPath = DirectoryPath = HttpContext.Current.Server.MapPath(@"~/" + profile.PracticeDocumentDirectory + "/" + profile.PracticeCode);
        }
        public static string GetPracticeDocumentDirectory(string PracticeDocumentDirectory, string practiceCode)
        {
            return HttpContext.Current.Server.MapPath(@"~/" + PracticeDocumentDirectory + "/" + practiceCode);
        }
        public static string GetClintViewTemporarySavedFilePath(string CompleteFilePath, UserProfile profile)
        {
            string guid = GetGuidFolder(CompleteFilePath);
            string FileName = GetFileNameOrDirectoryPath(CompleteFilePath);
            if (CompleteFilePath.Contains("\\pdf"))
            {
                return profile.PracticeDocumentDirectory + "/" + profile.PracticeCode + "/Fox/TempAttached/" + guid + "/pdf/" + FileName;
            }
            return profile.PracticeDocumentDirectory + "/" + profile.PracticeCode + "/Fox/TempAttached/" + guid + "/" + FileName;
        }
        public static string GetFileNameOrDirectoryPath(string CompleteDirecotryWithFileName, bool ReturnFileName = true)
        {
            string FileName = "";
            FileName = CompleteDirecotryWithFileName.Split('\\').LastOrDefault();
            if (ReturnFileName)
            {
                return FileName;
            }
            return CompleteDirecotryWithFileName.Replace(FileName, "").TrimEnd('\\').Trim();
        }
        public static string GetGuidFolder(string DrectoryFileFullPath)
        {
            string guid = "";
            var fileName = GetFileNameOrDirectoryPath(DrectoryFileFullPath);
            if (DrectoryFileFullPath.Contains("\\pdf"))
            {
                DrectoryFileFullPath = DrectoryFileFullPath.Replace("\\pdf", "");
            }
            return guid = DrectoryFileFullPath.Replace(fileName, "").TrimEnd('\\').Split('\\').LastOrDefault().Trim();
        }
        public static bool CopyAndSaveDMSFile(string FileSourcePath, string FileDestinationPath)
        {
            if (!string.IsNullOrEmpty(FileSourcePath) && !string.IsNullOrEmpty(FileDestinationPath))
            {
                if (File.Exists(FileSourcePath))
                {
                    File.Copy(FileSourcePath, FileDestinationPath, true);
                    return true;
                }
            }
            return false;
        }
        public static string GenerateFileName(string PracticeCode, string FileExtention)
        {
            return PracticeCode + "_" + DateTime.Now.ToString("ddMMyyyHHmmssffff") + FileExtention;
        }

        public static string GenerateSignatureFileName(string username)
        {
            //return username + "_" + DateTime.Now.ToString("ddMMyyyHHmmssffff");
            return username + "_" + DateTime.Now.ToString("ddMMyyy") + "_" + DateTime.Now.Ticks;
        }
        #region Directory Delete Methods
        public static bool DeleteTempDirectory(string guid, UserProfile profile)
        {
            string DirectoryToDelete = HttpContext.Current.Server.MapPath(@"~/" + profile.PracticeDocumentDirectory + "/" + profile.PracticeCode + "/Fox/TempAttached/" + guid);
            if (!string.IsNullOrEmpty(DirectoryToDelete))
            {
                if (Directory.Exists(DirectoryToDelete))
                {

                    Directory.Delete(DirectoryToDelete, true);

                    return true;
                }
            }
            return false;
        }
        public static void DeleteTempDirectory(string directoryName, string practiceCode, string guid)
        {
            string DirectoryToDelete = HttpContext.Current.Server.MapPath(@"~/" + directoryName + "/" + practiceCode + "/Fox/TempAttached/" + guid);
            if (!string.IsNullOrEmpty(DirectoryToDelete))
            {
                if (Directory.Exists(DirectoryToDelete))
                {
                    Directory.Delete(DirectoryToDelete, true);
                }
            }
        }
        public static void DeleteTempDirectory(string practiceCode, string guid)
        {
            string DirectoryToDelete = HttpContext.Current.Server.MapPath(@"~/WebEHR_Documents/" + practiceCode + "/Fox/TempAttached/" + guid);
            if (!string.IsNullOrEmpty(DirectoryToDelete))
            {
                if (Directory.Exists(DirectoryToDelete))
                {
                    Directory.Delete(DirectoryToDelete, true);
                }
            }
        }
        public static bool DeleteTempDirectoryWithCompletePath(string DirectoryPath)
        {
            string DirectoryToDelete = HttpContext.Current.Server.MapPath(DirectoryPath);
            if (!string.IsNullOrEmpty(DirectoryToDelete))
            {
                if (Directory.Exists(DirectoryToDelete))
                {
                    Directory.Delete(DirectoryToDelete, true);
                    return true;
                }
            }
            return false;
        }
        #endregion
        public static string GetMappedFilePath(string ClintPath)
        {
            return HttpContext.Current.Server.MapPath(@"~/" + ClintPath);
        }
        public static string GetClientPathFromServerPath(string CompletServerMappedPath, string PracticeDirectoryPath)
        {
            if (string.IsNullOrEmpty(PracticeDirectoryPath))
            {
                PracticeDirectoryPath = "WebEHR_Documents";
            }
            string DirectoryPathWithFile = CompletServerMappedPath.Substring(CompletServerMappedPath.IndexOf(PracticeDirectoryPath));
            return DirectoryPathWithFile.Replace('\\', '/');
        }
        //public static string EncryptString(string PlainText)
        //{
        //    string EncryptedText = string.Empty;
        //    try
        //    {
        //        // string SecretKey = "Tigger@351$9E156";
        //        // string InitVector = "AS%o@312";
        //        byte[] Key;
        //        byte[] IV;
        //        RijndaelManaged Rjm = new RijndaelManaged();
        //        Rjm.Mode = CipherMode.CBC;
        //        Key = ASCIIEncoding.ASCII.GetBytes("MTBCTigger@351$9E156_20101212:16");
        //        IV = ASCIIEncoding.ASCII.GetBytes("MTBC_AS%o@101212");

        //        byte[] stringBytes = ASCIIEncoding.ASCII.GetBytes(PlainText.Trim());
        //        MemoryStream memStrm = new MemoryStream();
        //        using (ICryptoTransform AESDecryptor = Rjm.CreateEncryptor(Key, IV))
        //        {
        //            using (CryptoStream Cs = new CryptoStream(memStrm, AESDecryptor, CryptoStreamMode.Write))
        //            {
        //                Cs.Write(stringBytes, 0, stringBytes.Length);
        //                Cs.FlushFinalBlock();
        //                byte[] EncryptedBytes = memStrm.ToArray();
        //                memStrm.Close();
        //                EncryptedText = Convert.ToBase64String(EncryptedBytes);
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    return EncryptedText;
        //}
        public static string HashCode(byte[] bt)
        {
            string hashKey = string.Empty;
            try
            {
                hashKey = string.Empty;
                System.Security.Cryptography.SHA512 hash = System.Security.Cryptography.SHA512.Create();
                hash.ComputeHash(bt);
                hashKey = ByteArrayToString(hash.Hash);
            }
            catch (Exception ex)
            {
                System.Diagnostics.StackTrace a = new System.Diagnostics.StackTrace();
                System.Diagnostics.StackFrame stackFrame = new System.Diagnostics.StackFrame();
            }
            return hashKey;
        }
        public static string ByteArrayToString(byte[] arrInput)
        {
            int i;
            StringBuilder sOutput = new StringBuilder(arrInput.Length);
            for (i = 0; i < arrInput.Length; i++)
            {
                sOutput.Append(arrInput[i].ToString("X2"));
            }
            return sOutput.ToString();
        }
        //public static string GetClintMappedFilePath(UserProfile profile, string ServerPath)
        //{
        //    return HttpContext.Current.Server.MapPath(@"~/" + ClintPath);
        //}
        public static string CreateDocumentDirectoryAndMoveFilesFromTempPathToPermanent(string FilePath, string PracticeDocumentDirectoryName, string PracticeCode, string FolderName)
        {
            string returVal = "";
            if (!string.IsNullOrEmpty(FilePath))
            {
                if (File.Exists(FilePath))
                {
                    string DirectoryToCreate = GetPracticeDocumentDirectory(PracticeDocumentDirectoryName, PracticeCode) + "\\" + FolderName + "";
                    if (!Directory.Exists(DirectoryToCreate))
                    {
                        Directory.CreateDirectory(DirectoryToCreate);
                    }
                    string fileName = "s" + GetFileNameOrDirectoryPath(FilePath);
                    var Files = Directory.GetFiles(GetFileNameOrDirectoryPath(FilePath, false));
                    if (Files != null)
                    {
                        foreach (var source in Files)
                        {
                            //At this time we are sending one file so
                            File.Copy(source, DirectoryToCreate + "\\" + fileName, true);
                            returVal = DirectoryToCreate + "\\" + fileName;
                        }
                        // Guid delete
                        //Directory to delete
                        string guid = "";
                        if (FilePath.Contains("\\pdf"))
                        {
                            FilePath = FilePath.Replace("\\pdf", "");
                        }
                        guid = FilePath.Replace(fileName.TrimStart('s'), "").TrimEnd('\\').Split('\\').LastOrDefault().Trim();
                        DeleteTempDirectory(PracticeCode, guid);
                    }
                }
            }
            else
            {
            }
            return returVal;
        }
        public static string GetDocumentDirectoryPhysicalPathByFolder(UserProfile profile, string FolderName)
        {
            return profile.PracticeDocumentDirectory + "/" + profile.PracticeCode + "/" + FolderName + "/";
        }
        public static string CombineAllPdfFilesToOneFile(string FileDirectoryWhereAllFiles, string WhereToSaveOneCombineFile)
        {
            if (Directory.GetFiles(FileDirectoryWhereAllFiles).Count() > 0)
            {

                var ListOfAllFiles = Directory.GetFiles(FileDirectoryWhereAllFiles);
                if (ListOfAllFiles.Count() > 0)
                {
                    List<PdfReader> readerList = new List<PdfReader>();
                    foreach (var PdfFile in ListOfAllFiles)
                    {
                        PdfReader pdfReader = new PdfReader(PdfFile);
                        readerList.Add(pdfReader);
                    }
                    if (readerList.Count > 0)
                    {
                        Document document = new Document(iTextSharp.text.PageSize.A4, 0, 0, 0, 0);
                        using (var fs = new FileStream(WhereToSaveOneCombineFile, FileMode.Create))
                        {
                            PdfWriter writer = PdfWriter.GetInstance(document, fs);
                            document.Open();
                            PdfContentByte contentByte = writer.DirectContent;

                            foreach (PdfReader reader in readerList)
                            {
                                for (int i = 1; i <= reader.NumberOfPages; i++)
                                {
                                    PdfImportedPage page = writer.GetImportedPage(reader, i);
                                    if (page.Width != 595 && page.Height != 842) // page is not a4
                                    {
                                        iTextSharp.text.Rectangle pagesize = new iTextSharp.text.Rectangle(0, 0, page.Width, page.Height);
                                        document.SetPageSize(pagesize);
                                    }
                                    else
                                    { //page is A4
                                        if (page.Width <= page.Height)
                                            document.SetPageSize(iTextSharp.text.PageSize.A4);
                                        else
                                            document.SetPageSize(iTextSharp.text.PageSize.A4.Rotate());
                                    }
                                    document.NewPage();
                                    //if (addTemplate[count] == "true")
                                    //{
                                    //    contentByte.AddTemplate(page, document.PageSize.Width / reader.GetPageSize(i).Width, 0, 0, document.PageSize.Height / reader.GetPageSize(i).Height, 0, 0);
                                    //}
                                    document.Add(iTextSharp.text.Image.GetInstance(page));
                                }
                            }

                            //writer.Dispose();
                            document.Close();
                        }

                        //
                    }
                    foreach (var item in readerList)
                    {
                        item.Dispose();
                    }
                }
            }
            return WhereToSaveOneCombineFile;
        }
        public static string CombineAllPdfFilesToOneFile(string WhereToSaveOneCombineFile, List<SortedDocumentFiles> SortedFileList)
        {
            if (SortedFileList != null && SortedFileList.Count > 0)
            {
                SortedFileList = SortedFileList.OrderBy(x => x.SortNo).ToList();
                List<PdfReader> readerList = new List<PdfReader>();
                foreach (var PdfFile in SortedFileList)
                {
                    if (File.Exists(PdfFile.FilePath))
                    {
                        PdfReader pdfReader = new PdfReader(PdfFile.FilePath);
                        readerList.Add(pdfReader);
                    }
                }
                if (readerList.Count > 0)
                {
                    Document document = new Document(iTextSharp.text.PageSize.A4, 0, 0, 0, 0);
                    using (var fs = new FileStream(WhereToSaveOneCombineFile, FileMode.Create))
                    {
                        PdfWriter writer = PdfWriter.GetInstance(document, fs);
                        document.Open();
                        PdfContentByte contentByte = writer.DirectContent;

                        foreach (PdfReader reader in readerList)
                        {
                            for (int i = 1; i <= reader.NumberOfPages; i++)
                            {
                                PdfImportedPage page = writer.GetImportedPage(reader, i);
                                if (page.Width != 595 && page.Height != 842) // page is not a4
                                {
                                    iTextSharp.text.Rectangle pagesize = new iTextSharp.text.Rectangle(0, 0, page.Width, page.Height);
                                    document.SetPageSize(pagesize);
                                }
                                else
                                { //page is A4
                                    if (page.Width <= page.Height)
                                        document.SetPageSize(iTextSharp.text.PageSize.A4);
                                    else
                                        document.SetPageSize(iTextSharp.text.PageSize.A4.Rotate());
                                }
                                document.NewPage();
                                //if (addTemplate[count] == "true")
                                //{
                                //    contentByte.AddTemplate(page, document.PageSize.Width / reader.GetPageSize(i).Width, 0, 0, document.PageSize.Height / reader.GetPageSize(i).Height, 0, 0);
                                //}
                                document.Add(iTextSharp.text.Image.GetInstance(page));
                            }
                        }

                        //writer.Dispose();
                        document.Close();
                    }

                    //
                }
                foreach (var item in readerList)
                {
                    item.Dispose();
                }
            }

            return WhereToSaveOneCombineFile;
        }
        public static string GeneratePdfFromHtml(string html, string SaveDocPath, string htmlStyle = "")
        {
            try
            {
                string styleHtml = "";
                styleHtml = !string.IsNullOrEmpty(htmlStyle)
                    ? htmlStyle : "<style>body { color: #003366; background-color: #FFFFFF; font-family: Verdana, Tahoma, sans-serif; font-size: 11px; } a { /*color: #003366; background-color: #FFFFFF;*/ } ul li a { color: #003366; background-color: #FFFFFF; } ul li a:visited { color: #003366; background-color: #FFFFFF; } h1 { font-size: 12pt; font-weight: bold; } h2 { font-size: 11pt; font-weight: bold; } h3 { font-size: 10pt; font-weight: bold; } h4 { font-size: 8pt; font-weight: bold; } div { } table { width: 100%; } .header_table tr { background-color: #ccccff; } td { padding: 0.1cm 0.2cm; vertical-align: top; } .h1center { font-size: 12pt; font-weight: bold; text-align: center; width: 100%; } .header_table{ border: 1pt inset #00008b; width:100%; } .narr_table { width: 100%; } .narr_tr { background-color: #ffffcc; } .narr_th { background-color: #ffd700; } .td_label{ font-weight: bold; color: white; }</style>";
                string Generaltext = "<html><head><style type=''>table, tr, td, th, tbody, thead, tfoot {page-break-inside: avoid !important;z-index:100;}</style></head><body><div><table><tr><td>" + styleHtml + html + "</td></tr></div></table></body></html>";
                NReco.PdfGenerator.PageSize size = new NReco.PdfGenerator.PageSize();
                size = NReco.PdfGenerator.PageSize.A3;
                HtmlToPdfConverter pdf = new HtmlToPdfConverter
                {
                    PdfToolPath = Path.Combine(HttpRuntime.AppDomainAppPath, "PdfTool")
                };
                pdf.Size = size;
                pdf.Orientation = PageOrientation.Portrait;
                pdf.Margins = new PageMargins { Top = 20, Bottom = 13, Left = 13, Right = 13 };
                var pdfBytes = pdf.GeneratePdf(Generaltext);
                File.WriteAllBytes(SaveDocPath, pdfBytes);
                return SaveDocPath;
            }
            catch (Exception ex) { return ""; }
        }
    }

}