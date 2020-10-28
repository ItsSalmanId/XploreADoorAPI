

using Microsoft.Win32;
using SubSystems.HT;
using SubSystems.TE;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Net;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace WEBEHRUtil.Generics
{
    public class RtfConvertor
    {
        internal static bool BadLicNbr = false;
        internal int ConvType;
        internal string DefBasePictFile = "HTS_";
        internal static bool eval = true;
        internal static int EvalCount = 5;
        internal static int EvalDaysRemaining = 15;
        internal static int EvalDaysUsed = 0;
        internal static bool exceeded = false;
        internal static bool expired = false;
        private StrHdrFtr[] HdrFtr;
        public const int HR_FIRST_FTR = 1;
        public const int HR_FIRST_HDR = 0;
        public const int HR_FTR = 3;
        public const int HR_HDR = 2;
        public const int HRFLAG_DONT_HIDE_HIDDEN_CHAR = 0x80000;
        public const int HRFLAG_EMBED_PICTURE = 4;
        public const int HRFLAG_FIELD_TO_TEXT_BOX = 0x200000;
        public const int HRFLAG_FLASH_SUPPORT = 0x200;
        public const int HRFLAG_LIST_TO_TEXT = 0x20;
        public const int HRFLAG_NO_CRLF = 0x20000;
        public const int HRFLAG_NO_FIXED_TABLE_LAYOUT = 0x40000;
        public const int HRFLAG_NO_FLOATING_OBJECTS = 0x800000;
        public const int HRFLAG_NO_FONT = 0x100;
        public const int HRFLAG_NO_INTERNET = 0x100000;
        public const int HRFLAG_NO_PICT_PATH = 8;
        public const int HRFLAG_NO_SPAN_TAG = 0x80;
        public const int HRFLAG_RETURN_MSG_ID = 1;
        public const int HRFLAG_SAVE_PICT_AS_BMP = 0x800;
        public const int HRFLAG_SAVE_PICT_AS_JPG = 0x400000;
        public const int HRFLAG_SEGMENT_ONLY = 2;
        public const int HRFLAG_SKIP_DELETED_TEXT = 0x8000;
        public const int HRFLAG_WORDPAD_COMPATIBLE_PICT = 0x10;
        public const int HRFLAG_WRITE_HTML_BULLET_CHAR = 0x4000;
        public const int HRFLAG_XLATE_SINGLE_QUOTE = 0x400;
        internal int HrsFlags = 0;
        internal string HrsLastDebugMsg = "";
        internal int HrsLastMsg = 0;
        internal string HrsLastMsgText = "";
        public const int HRTYPE_HTML = 2;
        public const int HRTYPE_RTF = 1;
        public const int HRTYPE_TEXT = 0;
        internal static string HtnKey = "372HS-42G8Z-RBS79";
        internal static bool InIE = false;
        internal bool InServer = false;
        internal static bool IsServerLicense = true;
        internal static string LicenseCompany = "";
        internal static int LicenseCount = -1;
        internal static string LicenseFile = "";
        internal static string LicenseKey = "PE670-G8R30-9NE71";
        internal static DateTime LicensePulseDate = DateTime.Today;
        internal static string LicenseWord = "";
        internal static string MacAddr = "";
        internal int MargBot;
        internal bool MarginSet;
        internal int MargLeft;
        internal int MargRight;
        internal int MargTop;
        internal int MaxPicts;
        public const int MSG_BAD_HTML_FILE = 8;
        public const int MSG_BAD_LICENSE_KEY = 14;
        public const int MSG_BAD_LICENSE_NUMBER = 13;
        public const int MSG_BAD_RTF_FILE = 7;
        public const int MSG_EXPIRED = 9;
        public const int MSG_FILE_NOT_FOUND = 1;
        public const int MSG_INVALID_SERVER_LICENSE = 15;
        public const int MSG_LICENSE_EXCEEDED = 12;
        public const int MSG_MEMORY_LOCK = 5;
        public const int MSG_NO_EDITOR_INSTANCE = 2;
        public const int MSG_NO_FILE_SIGNATURE = 6;
        public const int MSG_OTHER = 0x63;
        public const int MSG_READ_ERROR = 3;
        public const int MSG_WRITE_ERROR = 4;
        internal bool OrientSet;
        internal int PaperHeight;
        internal bool PaperIsPortrait;
        internal bool PaperSet;
        internal int PaperWidth;
        internal string[] PictName;
        internal PaperKind PprKind;
        internal string ProjectDir;
        internal string SrcTagDir;
        internal static string TernKey = "H83KS-27B5Z-GL723";
        internal int TotalPicts;
        internal string WebDir;
        public const int XLATE_HTML_TO_RTF = 1;
        public const int XLATE_RTF_TO_HTML = 2;

        public RtfConvertor()
        {
            this.WebFolder = "";
            this.SrcTagFolder = "";
            this.ProjectDir = "";
            this.PaperSet = this.OrientSet = this.MarginSet = false;
            this.HdrFtr = new StrHdrFtr[4];
            for (int i = 0; i < 4; i++)
            {
                this.HdrFtr[i] = new StrHdrFtr();
            }
            this.HdrFtr[0].HdrFtrChar = '\x0019';
            this.HdrFtr[1].HdrFtrChar = '\x001a';
            this.HdrFtr[2].HdrFtrChar = '\x0011';
            this.HdrFtr[3].HdrFtrChar = '\x0010';
        }

        internal bool ApplyHdrFtrText(Tern tern)
        {
            for (int i = 0; i < 4; i++)
            {
                if ((this.HdrFtr[i].ptr != null) && (this.HdrFtr[i].ptr.Length != 0))
                {
                    tern.TerPosHdrFtrEx(0, this.HdrFtr[i].HdrFtrChar, 0, false);
                    if (this.HdrFtr[i].type == 0)
                    {
                        tern.InsertTerText(this.HdrFtr[i].ptr, false);
                    }
                    else
                    {
                        tern.InsertRtfBuf(this.HdrFtr[i].ptr, -1, -1, false);
                    }
                }
            }
            return true;
        }

        internal static bool CheckEval(bool KeyProvided, string nbr, string key)
        {
            bool flag = false;
            if (!eval)
            {
                return true;
            }
            if (!KeyProvided && !InIE)
            {
                string path = @"k:\hrn\key.txt";
                bool flag2 = false;
                if (File.Exists(path))
                {
                    flag2 = true;
                }
                else
                {
                    path = "key_hrn7.txt";
                    if (File.Exists(path))
                    {
                        flag2 = true;
                    }
                }
                if (flag2)
                {
                    StreamReader reader = null;
                    reader = new StreamReader(path);
                    if (reader != null)
                    {
                        key = reader.ReadToEnd();
                        reader.Close();
                        if (key.Length > LicenseKey.Length)
                        {
                            key = key.Substring(0, LicenseKey.Length);
                        }
                        flag = true;
                    }
                }
            }
            IsServerLicense = true;
            if (((key.Length == LicenseKey.Length) && (key.Substring(0, 4) == LicenseKey.Substring(0, 4))) && ((key.Substring(5, 5) == LicenseKey.Substring(5, 5)) && (key.Substring(11, 5) == LicenseKey.Substring(11, 5))))
            {
                if (!(flag || !(key.Substring(4, 1) != "S")))
                {
                    IsServerLicense = false;
                }
                eval = false;
                expired = false;
                LicenseCount = -1;
                exceeded = false;
                BadLicNbr = false;
                if ((nbr != "donthave") && IsServerLicense)
                {
                    CheckLicenseCount("Hrn7", nbr, key);
                }
                return true;
            }
            if (!InIE)
            {
                string name = @"Software\SubSystems\Hrn7\Eval";
                RegistryKey currentUser = Registry.CurrentUser;
                RegistryKey key3 = null;
                object obj2 = null;
                DateTime now = DateTime.Now;
                try
                {
                    key3 = currentUser.OpenSubKey(name, true);
                    obj2 = key3.GetValue("Date");
                    now = new DateTime(Convert.ToInt64(obj2.ToString()));
                }
                catch (Exception)
                {
                }
                if ((key3 == null) || (obj2 == null))
                {
                    key3 = currentUser.CreateSubKey(name);
                    now = DateTime.Today;
                    key3.SetValue("Date", now.Ticks);
                }
                key3.Close();
                TimeSpan span = (TimeSpan)(DateTime.Today - now);
                EvalDaysUsed = span.Days;
                EvalDaysRemaining = 15 - EvalDaysUsed;
                expired = EvalDaysRemaining <= 0;
            }
            return false;
        }

        internal static bool CheckLicenseCount(string product, string nbr, string key)
        {
            int index;
            byte[] buffer;
            string str = "";
            exceeded = false;
            BadLicNbr = false;
            LicenseCount = -1;
            LicenseFile = "";
            LicenseWord = "";
            MacAddr = "";
            if (key.Substring(4, 1) != "S")
            {
                return true;
            }
            str = key.Substring(10, 1);
            switch (str)
            {
                case "U":
                case "Z":
                    return true;
            }
            if (str == "1")
            {
                LicenseCount = 1;
            }
            else if (str == "A")
            {
                LicenseCount = 5;
            }
            else if (str == "B")
            {
                LicenseCount = 10;
            }
            else if (str == "C")
            {
                LicenseCount = 20;
            }
            else if (str == "D")
            {
                LicenseCount = 50;
            }
            else if (str == "E")
            {
                LicenseCount = 100;
            }
            else
            {
                LicenseCount = 1;
            }
            LicenseCount += 2;
            if (LicenseCompany.Length > 10)
            {
                LicenseCompany = LicenseCompany.Substring(0, 10);
            }
            for (index = 0; index < LicenseCompany.Length; index++)
            {
                char oldChar = LicenseCompany[index];
                if ((((oldChar < 'a') || (oldChar > 'z')) && ((oldChar < 'A') || (oldChar > 'Z'))) && ((oldChar < '0') || (oldChar > '9')))
                {
                    LicenseCompany = LicenseCompany.Replace(oldChar, '-');
                }
            }
            if (!(nbr.StartsWith("srab") || nbr.StartsWith("smo")))
            {
                BadLicNbr = true;
                return false;
            }
            if (nbr.IndexOf("-") < 0)
            {
                BadLicNbr = true;
                return false;
            }
            if (nbr.StartsWith("srab"))
            {
                int num2 = 0;
                try
                {
                    num2 = Convert.ToInt32(nbr.Substring(4, 5));
                }
                catch
                {
                    BadLicNbr = true;
                    return false;
                }
                if ((num2 < 0x7530) || (num2 > 0x124f8))
                {
                    BadLicNbr = true;
                    return false;
                }
            }
            LicenseFile = nbr + "_" + LicenseCompany + "_" + product + ".htm";
            MacAddr = "";
            try
            {
                if (!NetworkInterface.GetIsNetworkAvailable())
                {
                    LicenseCount = -1;
                    return true;
                }
                NetworkInterface[] allNetworkInterfaces = NetworkInterface.GetAllNetworkInterfaces();
                if ((allNetworkInterfaces == null) || (allNetworkInterfaces.Length == 0))
                {
                    LicenseCount = -1;
                    return true;
                }
                MacAddr = allNetworkInterfaces[0].GetPhysicalAddress().ToString();
                if (MacAddr.Length > 5)
                {
                    MacAddr = MacAddr.Substring(5) + MacAddr.Substring(0, 5);
                }
            }
            catch (Exception)
            {
                LicenseCount = -1;
                return true;
            }
            WebClient client = new WebClient();
            try
            {
                buffer = client.DownloadData("http://www.subsysctl.com/word.htm");
            }
            catch (Exception)
            {
                LicenseCount = -1;
                return true;
            }
            client.Dispose();
            LicenseWord = new UTF8Encoding().GetString(buffer);
            index = LicenseWord.IndexOf('\r');
            if (index > 0)
            {
                LicenseWord = LicenseWord.Substring(0, index);
            }
            return CheckLicenseCount2();
        }

        internal static bool CheckLicenseCount2()
        {
            string address = "";
            string strB = "";
            char ch = ',';
            int num2 = 2;
            exceeded = false;
            if (LicenseCount >= 0)
            {
                int num;
                char[] chArray2;
                strB = DateTime.Today.ToString("yyyyMMdd");
                address = "http://www.subsysctl.com/license/" + LicenseFile;
                string[] strArray = null;
                bool flag = true;
                byte[] buffer = null;
                string str3 = "";
                WebClient client = new WebClient();
                try
                {
                    buffer = client.DownloadData(address);
                }
                catch (Exception)
                {
                    flag = false;
                }
                client.Dispose();
                if ((flag && (buffer != null)) && (buffer.Length > 0))
                {
                    char[] chArray = new char[buffer.Length];
                    for (num = 0; num < buffer.Length; num++)
                    {
                        chArray[num] = (char)buffer[num];
                    }
                    str3 = new string(chArray);
                    str3 = str3.Replace("<div>", "").Replace("</div>", "");
                    chArray2 = new char[] { '\r', '\n' };
                    strArray = str3.Split(chArray2, StringSplitOptions.RemoveEmptyEntries);
                }
                int length = 0;
                if ((strArray != null) && (strArray.Length > 0))
                {
                    length = strArray.Length;
                    chArray2 = new char[] { ch };
                    for (num = 0; num < strArray.Length; num++)
                    {
                        string[] strArray2 = strArray[num].Split(chArray2);
                        if ((strArray2 != null) && (strArray2.Length > 0))
                        {
                            string str4 = strArray2[0];
                            if (((str4 == MacAddr) || (strArray2.Length != num2)) || (string.Compare(strArray2[1], strB) < 0))
                            {
                                strArray[num] = "";
                                length--;
                            }
                        }
                    }
                }
                if (length >= LicenseCount)
                {
                    exceeded = true;
                    return false;
                }
                char ch2 = '\r';
                ch2 = '\n';
                string str5 = ch2.ToString() + ch2.ToString();
                str3 = "";
                if (strArray != null)
                {
                    for (num = 0; num < strArray.Length; num++)
                    {
                        if (strArray[num] != "")
                        {
                            if (str3.Length > 0)
                            {
                                str3 = str3 + str5;
                            }
                            str3 = str3 + "<div>" + strArray[num] + "</div>";
                        }
                    }
                }
                if (str3.Length > 0)
                {
                    str3 = str3 + str5;
                }
                object obj2 = str3;
                str3 = string.Concat(new object[] { obj2, "<div>", MacAddr, ch, strB, "</div>" });
                buffer = new byte[str3.Length];
                for (num = 0; num < str3.Length; num++)
                {
                    buffer[num] = (byte)str3[num];
                }
                try
                {
                    FtpWebRequest request = (FtpWebRequest)WebRequest.Create("ftp://www.subsysctl.com/license/" + LicenseFile);
                    request.Method = "STOR";
                    request.Credentials = new NetworkCredential("u37854439", LicenseWord + LicenseWord + "b");
                    request.UsePassive = true;
                    request.UseBinary = true;
                    request.KeepAlive = false;
                    Stream requestStream = request.GetRequestStream();
                    requestStream.Write(buffer, 0, buffer.Length);
                    requestStream.Close();
                }
                catch (Exception)
                {
                }
                LicensePulseDate = DateTime.Today;
            }
            return true;
        }

        internal bool DrawEval()
        {
            string str2 = "";
            if (eval)
            {
                CheckEval(false, "donthave", "");
            }
            if (!eval)
            {
                if ((IsServerLicense && (LicenseCount >= 0)) && (DateTime.Today > LicensePulseDate))
                {
                    CheckLicenseCount2();
                    LicensePulseDate = DateTime.Today;
                }
                if (!(IsServerLicense || !this.InServer))
                {
                    this.HrsLastMsg = 15;
                    this.HrsLastMsgText = "Your license is not valid for Server use!";
                    return false;
                }
                if (IsServerLicense && (LicenseCount >= 0))
                {
                    if (exceeded)
                    {
                        this.HrsLastMsg = 12;
                        this.HrsLastMsgText = "No more licenses available! Please consider purchasing addional licenses.";
                        return false;
                    }
                    if (BadLicNbr)
                    {
                        this.HrsLastMsg = 13;
                        this.HrsLastMsgText = "Invalid License Number.  Please refer to your product delivery email for your valid Licnese# starting with a 'srab' or 'smo' prefix.";
                        return false;
                    }
                }
                return true;
            }
            EvalCount++;
            if (!((EvalCount >= 3) || expired))
            {
                return true;
            }
            EvalCount = 0;
            string text = "";
            text = ((((text + "Please visit our web site: www.subsystems.com to purchase the product.\n") + "\n" + "If you have already purchased a license for HTML - RTF Converter, v7, please set the license \n") + "using the HrsSetLicenseKey method.  Your license key is available in a \n" + "distribution file called key.txt.") + "\n" + "\n") + "\n" + "Thank you.";
            if (EvalDaysRemaining == 1)
            {
                str2 = "EVALUATION EXPIRING TODAY!";
            }
            else
            {
                str2 = "EVALUATION DAYS REMAINING: " + EvalDaysRemaining.ToString();
            }
            if (this.True(this.HrsFlags & 1) || this.InServer)
            {
                if (expired)
                {
                    this.HrsLastMsg = 9;
                    this.HrsLastMsgText = "EVALUATION EXPIRED!";
                }
            }
            else
            {
                MessageBox.Show(text, expired ? "EVALUATION EXPIRED!" : str2, MessageBoxButtons.OK);
            }
            return !expired;
        }

        internal int EvPicture(object Sender, ref string PictName)
        {
            this.SavePictName(PictName);
            return 0;
        }

        internal void EvSavePicture(object Sender, ref string PictName)
        {
            this.SavePictName(PictName);
        }

        internal bool False(int val)
        {
            return (val == 0);
        }

        internal bool FreePict()
        {
            this.PictName = null;
            this.TotalPicts = this.MaxPicts = 0;
            return true;
        }

        public string HrsConvertBuffer(string InData, int type)
        {
            InData = string.IsNullOrEmpty(InData) ? "" : InData;
            string str;
            string terBuffer = null;
            Tern tern;
            Htn htn;
            //if (!this.DrawEval())
            //{
            //    return null;
            //}
            if ((type != 2) && (type != 1))
            {
                return null;
            }
            if (InData.Length < 5)
            {
                str = InData;
            }
            else
            {
                str = InData.Substring(0, 5);
            }
            if (!this.InitConversion(out tern, out htn))
            {
                return null;
            }
            this.ConvType = type;
            if (type == 2)
            {
                if (str != @"{\rtf")
                {
                    this.PrintError(7, "Input string is not an RTF string", null);
                    return null;
                }
                if (!tern.SetTerBuffer(InData, ""))
                {
                    this.PrintError(3, "Error reading input file", null);
                    return null;
                }
                if (this.True(this.HrsFlags & 0x200000))
                {
                    tern.TerInputFieldToTextBox(true);
                }
                tern.TerSetOutputFormat(4);
                terBuffer = tern.GetTerBuffer();
                switch (terBuffer)
                {
                    case null:
                    case "":
                        this.PrintError(4, "Error retrieving output string", null);
                        return null;
                }
            }
            if (type == 1)
            {
                if (str == @"{\rtf")
                {
                    this.PrintError(8, "Input string is not an HTML string", null);
                    return null;
                }
                if (!htn.HtsRead(1, "", InData, ""))
                {
                    this.PrintError(3, null, null);
                    return null;
                }
                this.SetPaper(tern);
                this.ApplyHdrFtrText(tern);
                tern.TerSetOutputFormat(2);
                terBuffer = tern.GetTerBuffer();
                switch (terBuffer)
                {
                    case null:
                    case "":
                        this.PrintError(4, "Error retrieving output data", null);
                        return null;
                }
            }
            htn.TernDispose();
            return terBuffer;
        }

        public bool HrsConvertFile(string InputFile, string OutFile, int type)
        {
            Tern tern;
            Htn htn;
            bool flag = false;
            string pSign = "";
            string path = "";
            if (!this.DrawEval())
            {
                return false;
            }
            if ((type != 2) && (type != 1))
            {
                return false;
            }
            path = "";
            if (InputFile.IndexOf("www.") == 0)
            {
                path = path + "http://";
            }
            path = path + InputFile;
            if ((path.IndexOf("http://") != 0) && (path.IndexOf("https://") != 0))
            {
                if (!File.Exists(path))
                {
                    return this.PrintError(1, "Input File Not Found!", "HtsConvertFile");
                }
                if (!this.ReadFileSignature(path, out pSign))
                {
                    return this.PrintError(6, "Error reading file signature", null);
                }
            }
            if (!this.InitConversion(out tern, out htn))
            {
                return false;
            }
            this.ConvType = type;
            if (type == 2)
            {
                if (pSign != @"{\rtf")
                {
                    return this.PrintError(7, "Input file is not an RTF file", null);
                }
                if (!tern.ReadTerFile(path))
                {
                    return this.PrintError(3, "Error reading input file", null);
                }
                if (this.True(this.HrsFlags & 0x200000))
                {
                    tern.TerInputFieldToTextBox(true);
                }
                tern.TerSetOutputFormat(4);
                if (!tern.SaveTerFile(OutFile))
                {
                    return this.PrintError(4, "Error writing output file", null);
                }
                flag = true;
            }
            if (type == 1)
            {
                if (pSign == @"{\rtf")
                {
                    return this.PrintError(8, "Input file is not an HTML file", null);
                }
                if (!htn.HtsRead(0, path, "", ""))
                {
                    return this.PrintError(3, null, null);
                }
                this.SetPaper(tern);
                this.ApplyHdrFtrText(tern);
                tern.TerSetOutputFormat(2);
                if (!tern.SaveTerFile(OutFile))
                {
                    return this.PrintError(4, null, null);
                }
                flag = true;
            }
            htn.TernDispose();
            return flag;
        }

        public byte[] HrsFileToBytes(string file)
        {
            byte[] buffer = null;
            BinaryReader reader = null;
            try
            {
                FileStream input = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.Read);
                int length = (int)input.Length;
                buffer = new byte[length];
                reader = new BinaryReader(input);
                if (reader != null)
                {
                    buffer = reader.ReadBytes(length);
                    reader.Close();
                }
            }
            catch (Exception)
            {
                buffer = null;
            }
            return buffer;
        }

        public string HrsFileToStr(string file)
        {
            if (file.Length == 0)
            {
                return "";
            }
            if (!File.Exists(file))
            {
                return "";
            }
            byte[] buffer = this.HrsFileToBytes(file);
            if (buffer == null)
            {
                return "";
            }
            int length = buffer.Length;
            char[] chArray = new char[length];
            for (int i = 0; i < length; i++)
            {
                chArray[i] = (char)buffer[i];
            }
            return new string(chArray, 0, length);
        }

        public int HrsGetLastMessage(out string message, out string DebugMsg)
        {
            message = this.HrsLastMsgText;
            DebugMsg = this.HrsLastDebugMsg;
            return this.HrsLastMsg;
        }

        public static int HrsGetLicenseStatus()
        {
            if (eval)
            {
                if (expired)
                {
                    return 4;
                }
                return 1;
            }
            if (BadLicNbr)
            {
                return 2;
            }
            if (exceeded)
            {
                return 3;
            }
            return 0;
        }

        public int HrsGetPictCount()
        {
            return this.TotalPicts;
        }

        public string HrsGetPictName(int idx)
        {
            if ((idx < 0) || (idx >= this.TotalPicts))
            {
                return "";
            }
            return this.PictName[idx];
        }

        public bool HrsResetLastMessage()
        {
            this.HrsLastMsg = 0;
            this.HrsLastDebugMsg = "";
            this.HrsLastMsgText = "";
            return true;
        }

        public int HrsSetFlags(bool set, int flags)
        {
            if (set)
            {
                this.HrsFlags |= flags;
            }
            else
            {
                this.ResetUintFlag(ref this.HrsFlags, flags);
            }
            return this.HrsFlags;
        }

        public bool HrsSetHdrFtrText(int HfId, int type, string text)
        {
            if ((HfId < 0) || (HfId >= 4))
            {
                return false;
            }
            this.HdrFtr[HfId].type = type;
            this.HdrFtr[HfId].ptr = text;
            return true;
        }

        public static int HrsSetLicenseInfo(string LicKey, string LicNbr, string CompanyName)
        {
            LicenseCompany = CompanyName;
            CheckEval(true, LicNbr, LicKey);
            if (eval)
            {
                if (((LicKey != null) && (LicKey.Length >= 5)) && (LicKey[4] != 'S'))
                {
                    MessageBox.Show("Invalid Product License Key!");
                }
                return 1;
            }
            if (BadLicNbr)
            {
                return 2;
            }
            if (exceeded)
            {
                return 3;
            }
            return 0;
        }

        internal static bool HrsSetLicenseKey(string key)
        {
            CheckEval(true, "donthave", key);
            if (eval)
            {
                MessageBox.Show("Invalid Product License Key!");
            }
            return !eval;
        }

        public bool HrsSetPageMargin(int left, int right, int top, int bot)
        {
            this.MarginSet = true;
            this.MargLeft = left;
            this.MargRight = right;
            this.MargTop = top;
            this.MargBot = bot;
            return true;
        }

        public bool HrsSetPaperOrient(bool IsPortrait)
        {
            this.OrientSet = true;
            this.PaperIsPortrait = IsPortrait;
            return true;
        }

        public bool HrsSetPaperSize(PaperKind kind, int width, int height)
        {
            this.PaperSet = true;
            this.PprKind = kind;
            this.PaperWidth = width;
            this.PaperHeight = height;
            return true;
        }

        public byte[] HrsStrToBytes(string str)
        {
            if (str == null)
            {
                return null;
            }
            int length = str.Length;
            byte[] buffer = new byte[length];
            for (int i = 0; i < length; i++)
            {
                buffer[i] = (byte)str[i];
            }
            return buffer;
        }

        internal bool InitConversion(out Tern tern, out Htn htn)
        {
            this.FreePict();
            Tern.TerSetLicenseKey(TernKey);
            Htn.HtsSetLicenseKey(HtnKey);
            tern = Htn.HtnTern();
            tern.Width = 0x240;
            tern.UseWindow = false;
            tern.TerSetFlags(true, 0x100004);
            tern.TerSetFlags2(true, 0x800);
            tern.TerSetFlags5(true, 0x48000000);
            tern.TerSetFlags7(this.True(this.HrsFlags & 0x100000), 0x10000000);
            tern.TerSetFlags8(true, 0x8000);
            if (this.True(this.HrsFlags & 4))
            {
                tern.TerSetFlags(true, 0x10);
            }
            if (this.True(this.HrsFlags & 0x10))
            {
                tern.TerSetFlags6(true, 0x40);
            }
            if (this.True(this.HrsFlags & 0x20))
            {
                tern.TerSetFlags6(true, 0x8000);
            }
            if (this.True(this.HrsFlags & 0x4000))
            {
                tern.TerSetFlags8(true, 0x20000);
            }
            if (this.True(this.HrsFlags & 0x80000))
            {
                tern.TerSetFlags8(true, 0x1000000);
            }
            tern.InServer = true;
            if (this.WebDir.Length > 0)
            {
                tern.TerSetWebFolder(this.WebDir);
            }
            else if (this.WebDir.Length > 0)
            {
                tern.TerSetWebFolder(this.ProjectDir);
            }
            htn = new Htn(tern);
            if ((this.HrsFlags & 2) != 0)
            {
                htn.HtsSetFlags(true, 0x818000);
            }
            if ((this.HrsFlags & 8) != 0)
            {
                htn.HtsSetFlags(true, 4);
            }
            if (this.True(this.HrsFlags & 0x80))
            {
                htn.HtsSetFlags(true, 0x1000);
            }
            if (this.True(this.HrsFlags & 0x100))
            {
                htn.HtsSetFlags(true, 0x20000);
            }
            if (this.True(this.HrsFlags & 0x200))
            {
                htn.HtsSetFlags2(true, 0x40);
            }
            if (this.True(this.HrsFlags & 0x400))
            {
                htn.HtsSetFlags2(true, 0x100);
            }
            if (this.True(this.HrsFlags & 0x800))
            {
                htn.HtsSetFlags2(true, 0x800);
            }
            if (this.True(this.HrsFlags & 0x400000))
            {
                htn.HtsSetFlags2(true, 0x100000);
            }
            if (this.True(this.HrsFlags & 0x40000))
            {
                htn.HtsSetFlags2(true, 0x40000);
            }
            if (this.True(this.HrsFlags & 0x800000))
            {
                htn.HtsSetFlags2(true, 0x400000);
            }
            htn.HtsSetFlags2(true, 0x8000);
            htn.HtsSetFlags2(this.True(this.HrsFlags & 0x8000), 0x10000);
            htn.HtsSetFlags2(this.True(this.HrsFlags & 0x20000), 0x4000);
            htn.HtsSetFlags(true, 0x21);
            htn.SrcTagFolder = this.SrcTagDir;
            htn.ProjectFolder = this.ProjectDir;
            htn.PictureNamePrefix = this.DefBasePictFile;
            htn.HtsSetDownloadDir(this.WebDir);
            Assembly assembly = Assembly.GetAssembly(htn.GetType());
            tern.TerSetHtnAssembly(assembly);
            tern.TerSetHtnObject(htn);
            tern.TerSetDefTabWidth(720, false);
            tern.SetTerDefaultFont("Times New Roman", 10, 0, Color.Black, false);
            tern.TerSetMargin(0x438, 0x438, 0x438, 0x438, false);
            //htn.SavePicture += (new Htn.EventSavePicture(this, (IntPtr)this.EvSavePicture));
            //htn.Picture += (new Htn.EventPicture(this, (IntPtr)this.EvPicture));

            htn.SavePicture += (new Htn.EventSavePicture(EvSavePicture));
            htn.Picture += (new Htn.EventPicture(EvPicture));

            for (int i = 0; i < 4; i++)
            {
                if (((this.HdrFtr[i].ptr != null) && (this.HdrFtr[i].ptr.Length != 0)) && (this.HdrFtr[i].type == 2))
                {
                    htn.HtsRead(1, "", this.HdrFtr[i].ptr, "");
                    this.HdrFtr[i].ptr = null;
                    tern.TerSetOutputFormat(2);
                    this.HdrFtr[i].ptr = tern.Data;
                    if (this.HdrFtr[i].ptr != null)
                    {
                        this.HdrFtr[i].type = 1;
                    }
                    tern.Command = (0x271);
                    tern.Command = (0x25e);
                    tern.TerSetModify(false);
                }
            }
            return true;
        }

        public bool LogPrintf(params object[] msg)
        {
            string str = "";
            foreach (object obj2 in msg)
            {
                if (str.Length > 0)
                {
                    str = str + " ";
                }
                if (obj2 != null)
                {
                    str = str + obj2.ToString();
                }
                else
                {
                    str = str + "null object";
                }
            }
            return OurPrintf(str, true);
        }

        internal int MulDiv(int x, int y, int z)
        {
            long num3 = x * y;
            long num2 = num3 / ((long)z);
            long num = num3 % ((long)z);
            if (num >= (z >> 1))
            {
                num2 += 1L;
            }
            return (int)num2;
        }

        public bool OurPrintf(params object[] msg)
        {
            string str = "";
            foreach (object obj2 in msg)
            {
                if (str.Length > 0)
                {
                    str = str + " ";
                }
                if (obj2 != null)
                {
                    str = str + obj2.ToString();
                }
                else
                {
                    str = str + "null object";
                }
            }
            return OurPrintf(str, false);
        }

        private static bool OurPrintf(object msg, bool LogIt)
        {
            IntPtr ptr;
            string path = @"c:\temp\hrn.log";
            string str = msg.ToString();
            if (LogIt)
            {
                StreamWriter writer = null;
                try
                {
                    writer = new StreamWriter(path, true, Encoding.ASCII);
                }
                catch (Exception)
                {
                    return false;
                }
                string str3 = DateTime.Now.ToString();
                writer.Write(str3);
                writer.Write(" --> ");
                writer.WriteLine(str);
                writer.Close();
                return true;
            }
            if (!(IntPtr.Zero != (ptr = Win32.FindWindow("DBWin", null))))
            {
                MessageBox.Show(str, "", MessageBoxButtons.OK);
            }
            else if (IntPtr.Zero != (ptr = Win32.GetWindow(ptr, 5)))
            {
                bool flag = false;
                int length = str.Length;
                int num = 0;
                while (num < length)
                {
                    if (str[num] < ' ')
                    {
                        break;
                    }
                    num++;
                }
                if (num < 0x20)
                {
                    flag = true;
                }
                if (!flag)
                {
                    str = str + "\r\n";
                }
                Win32.SendMessage(ptr, 0xb1, (IntPtr)0x7fff, (IntPtr)0x7fff);
                Win32.SendMessage(ptr, 0xc2, IntPtr.Zero, str);
                Win32.SendMessage(ptr, 0xb7, IntPtr.Zero, IntPtr.Zero);
                if (flag)
                {
                    str = "\r\n";
                    Win32.SendMessage(ptr, 0xb1, (IntPtr)0x7fff, (IntPtr)0x7fff);
                    Win32.SendMessage(ptr, 0xc2, IntPtr.Zero, str);
                    Win32.SendMessage(ptr, 0xb7, IntPtr.Zero, IntPtr.Zero);
                }
            }
            return true;
        }

        internal bool PrintError(int MsgId, string msg, string DebugMsg)
        {
            bool flag = false;
            if (((this.HrsFlags & 1) != 0) || this.InServer)
            {
                flag = true;
            }
            this.HrsLastMsg = MsgId;
            if (flag)
            {
                this.HrsLastDebugMsg = DebugMsg;
                this.HrsLastMsgText = msg;
            }
            else
            {
                MessageBox.Show(msg, DebugMsg, MessageBoxButtons.OK);
            }
            return false;
        }

        internal bool ReadFileSignature(string InpFile, out string pSign)
        {
            int count = 5;
            char[] buffer = new char[count];
            pSign = "";
            if (File.Exists(InpFile))
            {
                StreamReader reader = null;
                reader = new StreamReader(InpFile);
                if (reader != null)
                {
                    if (count != reader.Read(buffer, 0, count))
                    {
                        reader.Close();
                        return false;
                    }
                    pSign = new string(buffer, 0, count);
                    reader.Close();
                    return true;
                }
            }
            return false;
        }

        internal int ResetUintFlag(ref int flags, int flag)
        {
            return (flags &= ~flag);
        }

        internal bool SavePictName(string pict)
        {
            if ((this.TotalPicts + 1) > this.MaxPicts)
            {
                int num = this.TotalPicts + 10;
                if (this.PictName == null)
                {
                    this.PictName = new string[num + 1];
                }
                else
                {
                    string[] strArray = new string[num + 1];
                    for (int i = 0; i < this.TotalPicts; i++)
                    {
                        strArray[i] = this.PictName[i];
                    }
                    this.PictName = strArray;
                }
                this.MaxPicts = num;
            }
            this.PictName[this.TotalPicts] = pict;
            this.TotalPicts++;
            return true;
        }

        internal bool SetPaper(Tern tern)
        {
            if (this.PaperSet)
            {
                tern.TerSetSectPageSize(-1, this.PprKind, this.PaperWidth, this.PaperHeight, false);
            }
            if (this.OrientSet)
            {
                tern.TerSetSectOrient(this.PaperIsPortrait, false);
            }
            if (this.MarginSet)
            {
                tern.TerSetMargin(this.MargLeft, this.MargRight, this.MargTop, this.MargBot, false);
            }
            if ((this.PaperSet || this.OrientSet) || this.MarginSet)
            {
                tern.TerAdjustHtmlTable();
            }
            if (this.ConvType == 1)
            {
                tern.TerRepaginate(false);
            }
            return true;
        }

        public static bool StcPrintf(params object[] msg)
        {
            string str = "";
            foreach (object obj2 in msg)
            {
                if (str.Length > 0)
                {
                    str = str + " ";
                }
                if (obj2 != null)
                {
                    str = str + obj2.ToString();
                }
                else
                {
                    str = str + "null object";
                }
            }
            return OurPrintf(str, false);
        }

        internal bool True(int val)
        {
            return (val != 0);
        }

        [Description("Control to be hosted in a server app"), Category("Operating Mode")]
        public bool InWebServer
        {
            get
            {
                return this.InServer;
            }
            set
            {
                this.InServer = value;
            }
        }

        [Description("Prefix string to add to the SRC tag file name."), Category("Operating Mode")]
        public string PictureNamePrefix
        {
            get
            {
                return this.DefBasePictFile;
            }
            set
            {
                this.DefBasePictFile = value;
            }
        }

        [Description(@"Project folder, example: c:\inetpub\wwwroot\MyProject"), Category("Operating Environment")]
        public string ProjectFolder
        {
            get
            {
                return this.ProjectDir;
            }
            set
            {
                this.ProjectDir = value;
            }
        }

        [Category("Operating Mode"), Description("Folder name to add to the 'src' tag file name")]
        public string SrcTagFolder
        {
            get
            {
                return this.SrcTagDir;
            }
            set
            {
                this.SrcTagDir = value;
            }
        }

        [Category("Operating Mode"), Description("Web folder where the html pictures should be saved")]
        public string WebFolder
        {
            get
            {
                return this.WebDir;
            }
            set
            {
                this.WebDir = value;
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct StrHdrFtr
        {
            internal char HdrFtrChar;
            internal int type;
            internal string ptr;
        }

        internal class Win32
        {
            internal const int EM_REPLACESEL = 0xc2;
            internal const int EM_SCROLLCARET = 0xb7;
            internal const int EM_SETSEL = 0xb1;
            internal const int GW_CHILD = 5;

            [DllImport("user32.dll", CharSet = CharSet.Auto)]
            internal static extern IntPtr FindWindow(string ClsName, string WinName);
            [DllImport("user32.dll", CharSet = CharSet.Unicode)]
            internal static extern IntPtr GetWindow(IntPtr hWnd, int cmd);
            [DllImport("user32.dll", CharSet = CharSet.Auto)]
            internal static extern bool SendMessage(IntPtr hWnd, [MarshalAs(UnmanagedType.U4)] int Msg, IntPtr wParam, IntPtr lParam);
            [DllImport("user32.dll", CharSet = CharSet.Auto)]
            internal static extern bool SendMessage(IntPtr hWnd, [MarshalAs(UnmanagedType.U4)] int Msg, IntPtr wParam, string lParam);
        }
    }
}
