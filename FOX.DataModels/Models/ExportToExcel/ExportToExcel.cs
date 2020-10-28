using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace FOX.DataModels.Models.ExportToExcel
{
    public class ExportToExcel
    {
        private static void RemoveExtraColumns(DataTable dt, string CalledFrom)
        {
            DataColumnCollection dc = dt.Columns;
          

            if (CalledFrom.Equals("PHD_Special_Instructions"))
            {
                if (dc.Contains("Practice_Code"))
                {
                    dc.Remove("Practice_Code");
                }
                if (dc.Contains("Deleted"))
                {
                    dc.Remove("Deleted");
                }
            }
        }

        //private static void SetAmounts(DataTable dt)
        //{
        //    DataColumnCollection dtcol = dt.Columns;
        //    foreach (DataRow drr in dt.Rows)
        //    {
        //        #region Amounts Region
        //        if (dtcol.Contains("Billed_Amount"))
        //        {
        //            if (drr["Billed_Amount"].ToString() != string.Empty)
        //            {
        //                decimal x = Convert.ToDecimal(drr["Billed_Amount"].ToString());
        //                drr["Billed_Amount"] = x.ToString("#,##0.00");
        //            }
        //        }
        //        if (dtcol.Contains("Due_Amount"))
        //        {
        //            if (drr["Due_Amount"].ToString() != string.Empty)
        //            {
        //                decimal x = Convert.ToDecimal(drr["Due_Amount"].ToString());
        //                drr["Due_Amount"] = x.ToString("#,##0.00");
        //            }
        //        }
        //        if (dtcol.Contains("Claim_Total"))
        //        {
        //            if (drr["Claim_Total"].ToString() != string.Empty)
        //            {
        //                decimal x = Convert.ToDecimal(drr["Claim_Total"].ToString());
        //                drr["Claim_Total"] = x.ToString("#,##0.00");
        //            }
        //        }

        //        if (dtcol.Contains("Amt_Due"))
        //        {
        //            if (drr["Amt_Due"].ToString() != string.Empty)
        //            {
        //                decimal x = Convert.ToDecimal(drr["Amt_Due"].ToString());
        //                drr["Amt_Due"] = x.ToString("#,##0.00");
        //            }
        //        }

        //        if (dtcol.Contains("PaidAmount"))
        //        {
        //            if (drr["PaidAmount"].ToString() != string.Empty)
        //            {
        //                decimal x = Convert.ToDecimal(drr["PaidAmount"].ToString());
        //                drr["PaidAmount"] = x.ToString("#,##0.00");
        //            }
        //        }
        //        //ERA Submission
        //        if (dtcol.Contains("Amount"))
        //        {
        //            if (drr["Amount"].ToString() != string.Empty)
        //            {
        //                decimal x = Convert.ToDecimal(drr["Amount"].ToString());
        //                drr["Amount"] = x.ToString("#,##0.00");
        //            }
        //        }

        //        if (dtcol.Contains("TOTAL_AMOUNT"))
        //        {
        //            if (drr["TOTAL_AMOUNT"].ToString() != string.Empty)
        //            {
        //                decimal x = Convert.ToDecimal(drr["TOTAL_AMOUNT"].ToString());
        //                drr["TOTAL_AMOUNT"] = x.ToString("#,##0.00");
        //            }
        //        }

        //        if (dtcol.Contains("AMOUNT_DUE"))
        //        {
        //            if (drr["AMOUNT_DUE"].ToString() != string.Empty)
        //            {
        //                decimal x = Convert.ToDecimal(drr["AMOUNT_DUE"].ToString());
        //                drr["AMOUNT_DUE"] = x.ToString("#,##0.00");
        //            }
        //        }
        //        //ERA Submission
        //        if (dtcol.Contains("check_amount"))
        //        {
        //            if (drr["check_amount"].ToString() != string.Empty)
        //            {
        //                decimal x = Convert.ToDecimal(drr["check_amount"].ToString());
        //                drr["check_amount"] = x.ToString("#,##0.00");
        //            }
        //        }
        //        if (dtcol.Contains("payments"))
        //        {
        //            if (drr["payments"].ToString() != string.Empty)
        //            {
        //                decimal x = Convert.ToDecimal(drr["payments"].ToString());
        //                drr["payments"] = x.ToString("#,##0.00");
        //            }
        //        }
        //        if (dtcol.Contains("Days_0_30"))
        //        {
        //            if (drr["Days_0_30"].ToString() != string.Empty)
        //            {
        //                decimal x = Convert.ToDecimal(drr["Days_0_30"].ToString());
        //                drr["Days_0_30"] = x.ToString("#,##0.00");
        //            }
        //        }

        //        if (dtcol.Contains("Days_31_60"))
        //        {
        //            if (drr["Days_31_60"].ToString() != string.Empty)
        //            {
        //                decimal x = Convert.ToDecimal(drr["Days_31_60"].ToString());
        //                drr["Days_31_60"] = x.ToString("#,##0.00");
        //            }
        //        }

        //        if (dtcol.Contains("Days_61_90"))
        //        {
        //            if (drr["Days_61_90"].ToString() != string.Empty)
        //            {
        //                decimal x = Convert.ToDecimal(drr["Days_61_90"].ToString());
        //                drr["Days_61_90"] = x.ToString("#,##0.00");
        //            }
        //        }

        //        if (dtcol.Contains("Days_91_120"))
        //        {
        //            if (drr["Days_91_120"].ToString() != string.Empty)
        //            {
        //                decimal x = Convert.ToDecimal(drr["Days_91_120"].ToString());
        //                drr["Days_91_120"] = x.ToString("#,##0.00");
        //            }
        //        }

        //        if (dtcol.Contains("Days_120_Plus"))
        //        {
        //            if (drr["Days_120_Plus"].ToString() != string.Empty)
        //            {
        //                decimal x = Convert.ToDecimal(drr["Days_120_Plus"].ToString());
        //                drr["Days_120_Plus"] = x.ToString("#,##0.00");
        //            }
        //        }

        //        if (dtcol.Contains("Total_Aging"))
        //        {
        //            if (drr["Total_Aging"].ToString() != string.Empty)
        //            {
        //                decimal x = Convert.ToDecimal(drr["Total_Aging"].ToString());
        //                drr["Total_Aging"] = x.ToString("#,##0.00");
        //            }
        //        }
        //        #endregion
        //        #region Dates Region
        //        if (dtcol.Contains("DOS"))
        //        {
        //            drr["DOS"] = string.Format("{0:MM/dd/yyyy}", drr["DOS"]);
        //        }
        //        if (dtcol.Contains("Bill_Date"))
        //        {
        //            drr["Bill_Date"] = string.Format("{0:MM/dd/yyyy}", drr["Bill_Date"]);
        //        }
        //        if (dtcol.Contains("Created_Date"))
        //        {
        //            drr["Created_Date"] = string.Format("{0:MM/dd/yyyy}", drr["Created_Date"]);
        //        }
        //        if (dtcol.Contains("Applied_Date"))
        //        {
        //            drr["Applied_Date"] = string.Format("{0:MM/dd/yyyy}", drr["Applied_Date"]);
        //        }
        //        if (dtcol.Contains("Entry_Date"))
        //        {
        //            drr["Entry_Date"] = string.Format("{0:MM/dd/yyyy}", drr["Entry_Date"]);
        //        }
        //        if (dtcol.Contains("MTB_Import_Date"))
        //        {
        //            drr["MTB_Import_Date"] = string.Format("{0:MM/dd/yyyy}", drr["MTB_Import_Date"]);
        //        }
        //        if (dtcol.Contains("CS_CASE_REOPEN_DATE"))
        //        {
        //            drr["CS_CASE_REOPEN_DATE"] = string.Format("{0:MM/dd/yyyy}", drr["CS_CASE_REOPEN_DATE"]);
        //        }

        //        #endregion
        //    }
        //}
        private static void SetHeaders(DataTable dt)
        {
            DataColumnCollection dtcol = dt.Columns;

            if (dtcol.Contains("Cell_Phone"))
                dt.Columns["Cell_Phone"].ColumnName = "Cell Phone #";

            if (dtcol.Contains("CP_Event_Status"))
                dt.Columns["CP_Event_Status"].ColumnName = "Cell Phone # Status";

            if (dtcol.Contains("PAYMENT_TYPE"))
                dt.Columns["PAYMENT_TYPE"].ColumnName = "Payment_Type";
            if (dtcol.Contains("CP_Event_Status"))
                dt.Columns["CP_Event_Status"].ColumnName = "Cell Phone # Status";

            if (dtcol.Contains("PC_PracCode"))
                dt.Columns.Remove("PC_PracCode");

            if (dtcol.Contains("BATCH_NO"))
                dt.Columns["BATCH_NO"].ColumnName = "Batch_No";

            if (dtcol.Contains("BATCH_DATE"))
                dt.Columns["BATCH_DATE"].ColumnName = "Batch_Date";
            //PHD Special Instructions Report
            if (dtcol.Contains("S_No"))
                dt.Columns["S_No"].ColumnName = "S.No";

            if (dtcol.Contains("Practice_Instructions"))
                dt.Columns["Practice_Instructions"].ColumnName = "Instructions";
        }

        /// <summary>
        /// AS this Function is called after setting up the headers. So use column name that you replace in SetHeaders Section.
        /// Do not write any column that has text like 'date' in end. Function will auto handle those columns.
        /// </summary>
        /// <returns></returns>
        private static List<string> GenerateCenterAlignList()
        {
            List<string> str = new List<string>();
            str.Add("Pri. Status");
            str.Add("Sec. Status");
            str.Add("Oth. Status");
            str.Add("Pat. Status");
            str.Add("DOS");
            return str;
        }
        /// <summary>
        /// /// AS this Function is called after setting up the headers. So use column name that you replace in SetHeaders Section.
        /// Do not write any column that has text like 'date' in end. Function will auto handle those columns.
        /// </summary>
        /// <returns></returns>
        private static List<string> GenerateRightAlignList()
        {
            List<string> str = new List<string>();
            str.Add("Claim Total");
            str.Add("Paid Amount");
            str.Add("Amount Due");
            return str;
        }
    }
}