using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.IO;
//using FOX.DataModels.Models.Exceptions;
//using FOX.DataModels.Models.Security;
//using FOX.DataModels.Context;

namespace FOX.DataModels.HelperClasses
{
    public class Web_GetMaxColumnID
    {
        public string MaxID { get; set; }
    }
    public class PatAppMaxColumnID {
        public string MaxColumnID { get; set; }
    }

    [Table("Maintenance_Counter")]
    public class Maintenance_Counter
    {
        [Key]
        public string Col_Name { get; set; }
        public long Col_Counter { get; set; }
        //public long SUPPORT_TRAINING_ID { get; set; }
        //public long SUPPORT_CALL_ID { get; set; }
    }

    public class Helper
    {

        public static void LogException(Exception ex)
        {
            if (!string.IsNullOrEmpty(ex.Message))
            {
                string directory = @"e:\\Errors";
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }
                string filePath = directory + "\\EligibilityErrors.txt";
                using (StreamWriter writer = new StreamWriter(filePath, true))
                {
                    writer.WriteLine("Message :" + ex.Message + "<br/>" + Environment.NewLine + "StackTrace :" + ex.StackTrace + Environment.NewLine + Environment.NewLine +
                        "///------------------Inner Exception------------------///" + Environment.NewLine + ((ex.InnerException != null && ex.InnerException.Message != null) ? ex.InnerException.Message : "NULL") + Environment.NewLine +
                        "Date :" + DateTime.Now.ToString() + Environment.NewLine + Environment.NewLine + "-------------------------------------------------------||||||||||||---End Current Exception---||||||||||||||||-------------------------------------------------------" + Environment.NewLine);
                }
            }
        }
        public static void LogException(string text)
        {
            string directory = @"e:\\Errors";
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
            string filePath = directory + "\\EligibilityErrors.txt";
            using (StreamWriter writer = new StreamWriter(filePath, true))
            {
                writer.WriteLine("Message :" + text + "<br/>" + "Date :" + DateTime.Now.ToString() + Environment.NewLine);
            }
        }
    }

    public static class StringHelper
    {
        public static string ToTitleCase(this string title)
        {
            return !string.IsNullOrEmpty(title) ? System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(title) : string.Empty;
        }

        public static string ApplyPhoneMask(this string phone)
        {
            if (!string.IsNullOrWhiteSpace(phone))
            {
                if (!phone.Contains("(") || !phone.Contains(")") || !phone.Contains("-"))
                {
                    if (phone.Length == 10)
                    {
                        return Convert.ToInt64(phone.Substring(0, 10)).ToString("(000) 000-0000");
                    }
                    else if (phone.Length == 9)
                    {
                        return Convert.ToInt64(phone.Substring(0, 9)).ToString("(000) 000-000");
                    }
                    else if (phone.Length == 8)
                    {
                        return Convert.ToInt64(phone.Substring(0, 8)).ToString("(000) 000-00");
                    }
                    else if (phone.Length == 7)
                    {
                        return Convert.ToInt64(phone.Substring(0, 7)).ToString("(000) 000-0");
                    }
                    else if (phone.Length == 6)
                    {
                        return Convert.ToInt64(phone.Substring(0, 6)).ToString("(000) 000");
                    }
                    else if (phone.Length == 5)
                    {
                        return Convert.ToInt64(phone.Substring(0, 5)).ToString("(000) 00");
                    }
                    else if (phone.Length == 4)
                    {
                        return Convert.ToInt64(phone.Substring(0, 4)).ToString("(000) 0");
                    }
                    else if (phone.Length == 3)
                    {
                        return Convert.ToInt64(phone.Substring(0, 3)).ToString("(000)");
                    }
                    else
                    {
                        return phone;
                    }
                }
                else
                    return phone;
            }
            else
            {
                return string.Empty;
            }
        }
    }
}
