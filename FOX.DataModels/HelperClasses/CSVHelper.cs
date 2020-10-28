using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.IO;
using System.Web;

namespace FOX.DataModels.HelperClasses
{
    public static class CSVHelper
    {
        public static string ExportCSV<T>(this List<T> list, string fileName)
        {
            var csv = GetCSV(list);
            return CreateCsvFile(csv, fileName);
        }
        public static string GetCSV<T>(this List<T> list)
        {
            StringBuilder sb = new StringBuilder();

            //Get the properties for type T for the headers
            PropertyInfo[] propInfos = typeof(T).GetProperties();
            for (int i = 0; i <= propInfos.Length - 1; i++)
            {
                sb.Append(propInfos[i].Name);

                if (i < propInfos.Length - 1)
                {
                    sb.Append(",");
                }
            }

            sb.AppendLine();

            //Loop through the collection, then the properties and add the values
            for (int i = 0; i <= list.Count - 1; i++)
            {
                T item = list[i];
                for (int j = 0; j <= propInfos.Length - 1; j++)
                {
                    object o = item.GetType().GetProperty(propInfos[j].Name).GetValue(item, null);
                    if (o != null)
                    {
                        string value = o.ToString();

                        //Check if the value contans a comma and place it in quotes if so
                        if (value.Contains(","))
                        {
                            value = string.Concat("\"", value, "\"");
                        }

                        //Replace any \r or \n special characters from a new line with a space
                        if (value.Contains("\r"))
                        {
                            value = value.Replace("\r", " ");
                        }
                        if (value.Contains("\n"))
                        {
                            value = value.Replace("\n", " ");
                        }

                        sb.Append(value);
                    }

                    if (j < propInfos.Length - 1)
                    {
                        sb.Append(",");
                    }
                }

                sb.AppendLine();
            }

            return sb.ToString();
        }
        //public static void ExportCSV(string csv, string filename)
        //{
        //    string filePath = GetExportedDocumentDirectoryPath
        //    File.WriteAllText(filePath, sb.ToString());

        //    //HttpContext.Current.Response.Clear();
        //    //HttpContext.Current.Response.AddHeader("content-disposition", string.Format("attachment; filename={0}.csv", filename));
        //    //HttpContext.Current.Response.ContentType = "text/csv";
        //    //HttpContext.Current.Response.AddHeader("Pragma", "public");
        //    //HttpContext.Current.Response.Write(csv);
        //    //HttpContext.Current.Response.End();
        //}

        public static string CreateCsvFile(string csv, string fileName)
        {
            var filePath = "";
            try
            {
                string directoryPath = GetExportedDocumentDirectoryPath();
                if (!string.IsNullOrEmpty(directoryPath))
                {
                    filePath = Path.Combine(directoryPath, fileName);
                    File.WriteAllText(filePath, csv);
                    return fileName;
                }
            }
            catch (Exception ex)
            {
                Helper.LogException(ex);
            }
            return "";
        }

        public static string GetExportedDocumentDirectoryPath()
        {
            var path = HttpContext.Current.Server.MapPath(@"~/ExportedFiles");
            try
            {
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);
                return path;
            }
            catch (Exception ex)
            {
                Helper.LogException(ex);
                return "";
            }
        }

    }
}