using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq.Expressions;
using System.Net;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace loanLibrary
{
    #region  loanGlobal
    public class loanGlobalsDAL
    {
        public const string UniqueKey = "yyyy-MM-dd_HH.mm.ss.ffff";
        public const string EncryptionKey = "31BF3856AD364E35";

        public string Page { get; set; }
        public string Url { get; set; }
        public string Total { get; set; }

        internal static void SaveError(Exception ex)
        {
            System.Threading.ThreadAbortException tae = ex as System.Threading.ThreadAbortException;
            if (tae != null)
            {
                return;
            }
            try
            {
                /// Insert error into ErrorLog table
                loanErrorLogDAL _objErrorLogDAL = new loanErrorLogDAL();
                _objErrorLogDAL.ErrorDateTime = loanGlobalsDAL.GetCurrentDateTime();
                _objErrorLogDAL.ErrorMessage = ex.Message;
                _objErrorLogDAL.ErrorStackTrace = ex.StackTrace;

                if (_objErrorLogDAL.InsertErrorLog() == loanRecordStatus.Error)
                {
                    /// Write error into ErrorLog file if error not inserted
                    string _strErrorLogFile = System.Configuration.ConfigurationManager.AppSettings["rootpath"] + System.Configuration.ConfigurationManager.AppSettings["ErrorFilePath"];
                    StringBuilder _sb = new StringBuilder();
                    _sb.AppendLine("Error DateTime    : " + loanGlobalsDAL.GetCurrentDateTime().ToString("s"));
                    _sb.AppendLine("Error Message     : " + ex.Message);
                    _sb.AppendLine("Error StackTrace  : " + ex.StackTrace);
                    _sb.AppendLine(string.Empty.PadRight(100, '-'));
                    File.WriteAllText(_strErrorLogFile, _sb.ToString());
                }
            }
            catch
            {
            }
        }

        public static string GetCleanFileName(string fileName)
        {
            string FileName = Regex.Replace(fileName, "[^a-zA-Z0-9_.]", "_");
            FileName = Path.GetFileNameWithoutExtension(FileName) + "_" + GetRandomString(6) + Path.GetExtension(FileName);
            return FileName;
        }
        #region Culture
        public static void SwitchCulture(string culture)
        {
            if (!string.IsNullOrEmpty(culture))
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo(culture);
                Thread.CurrentThread.CurrentUICulture = new CultureInfo(culture);
            }
        }
        #endregion
        public static string GetRandomString(int length)
        {
            //It will generate string with combination of small,capital letters and numbers
            char[] charArr = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz".ToCharArray();
            string randomString = string.Empty;
            Random objRandom = new Random();
            for (int i = 0; i < length; i++)
            {
                //Don't Allow Repetation of Characters
                int x = objRandom.Next(1, charArr.Length);
                if (!randomString.Contains(charArr.GetValue(x).ToString()))
                {
                    randomString += charArr.GetValue(x);
                }
                else
                {
                    i--;
                }
            }
            return randomString;
        }
        public static string GetRandomNumber(int length)
        {
            //It will generate string with combination of numbers
            char[] charArr = "0123456789".ToCharArray();
            string randomString = string.Empty;
            Random objRandom = new Random();
            for (int i = 0; i < length; i++)
            {
                //Don't Allow Repetation of Characters
                int x = objRandom.Next(1, charArr.Length);
                if (!randomString.Contains(charArr.GetValue(x).ToString()))
                {
                    randomString += charArr.GetValue(x);
                }
                else
                {
                    i--;
                }
            }
            return randomString;
        }

        #region Images
        public static bool CreateThumbImages(string imageName, string imageSavePath, string watermarkImageNameWithPath = null)
        {
            try
            {
                abHelper.ImageProcessing img = new abHelper.ImageProcessing();

                string ImageName = imageName;
                if (!string.IsNullOrEmpty(watermarkImageNameWithPath))
                {
                    ImageName = Path.GetFileNameWithoutExtension(imageName) + "_w" + Path.GetExtension(imageName);
                    //  img.AddWatermark(imageSavePath + imageName, imageSavePath + ImageName, watermarkImageNameWithPath, abHelper.ImageProcessing.WatermarkPosition.BottomRight, 50);
                }

                // for extra small device 
                int Width = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["xsWidth"]);
                int Height = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["xsHeight"]);
                img.CreateThumbnail(imageSavePath + ImageName, imageSavePath + "xs_" + imageName, Width, Height, false);

                // for small device 
                Width = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["smWidth"]);
                Height = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["smHeight"]);
                img.CreateThumbnail(imageSavePath + ImageName, imageSavePath + "sm_" + imageName, Width, Height, false);

                // for medium device 
                Width = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["mdWidth"]);
                Height = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["mdHeight"]);
                img.CreateThumbnail(imageSavePath + ImageName, imageSavePath + "md_" + imageName, Width, Height, false);

                // for large device 
                Width = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["lgWidth"]);
                Height = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["lgHeight"]);
                img.CreateThumbnail(imageSavePath + ImageName, imageSavePath + "lg_" + imageName, Width, Height, false);

                // for extra large device 
                Width = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["xlWidth"]);
                Height = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["xlHeight"]);
                img.CreateThumbnail(imageSavePath + ImageName, imageSavePath + "xl_" + imageName, Width, Height, false);

                File.Delete(imageSavePath + imageName);
                if (!string.IsNullOrEmpty(watermarkImageNameWithPath))
                {
                    File.Delete(imageSavePath + ImageName);
                }
                return true;
            }
            catch (Exception ex)
            {
                loanGlobalsDAL.SaveError(ex);
                return false;
            }
        }

        public static bool DeleteThumbImages(string imageName, string imageSavePath)
        {
            try
            {
                // for extra small device 
                if (File.Exists(imageSavePath + "xs_" + imageName))
                {
                    File.Delete(imageSavePath + "xs_" + imageName);
                }

                // for small device 
                if (File.Exists(imageSavePath + "sm_" + imageName))
                {
                    File.Delete(imageSavePath + "sm_" + imageName);
                }

                // for medium device 
                if (File.Exists(imageSavePath + "md_" + imageName))
                {
                    File.Delete(imageSavePath + "md_" + imageName);
                }

                // for large device 
                if (File.Exists(imageSavePath + "lg_" + imageName))
                {
                    File.Delete(imageSavePath + "lg_" + imageName);
                }

                // for extra large device 
                if (File.Exists(imageSavePath + "xl_" + imageName))
                {
                    File.Delete(imageSavePath + "xl_" + imageName);
                }

                return true;
            }
            catch (Exception ex)
            {
                loanGlobalsDAL.SaveError(ex);
                return false;
            }
        }
        #endregion

        public static DateTime GetCurrentDateTime()
        {
            TimeZoneInfo tzLocal = TimeZoneInfo.FindSystemTimeZoneById("Arabic Standard Time");
            return TimeZoneInfo.ConvertTimeFromUtc(DateTime.Now.ToUniversalTime(), tzLocal);
        }

        public static string ConvertDateTimeToString(DateTime? dt, string format)
        {
            if (dt != null)
            {
                return dt.Value.ToString(format, new CultureInfo("en-us"));
                //return dt.Value.ToString(format, new CultureInfo("ar-sa"));
            }
            return string.Empty;
        }

        public static DataTable ToDataTable<T>(List<T> items, List<string> ExtraCollumns)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);

            //Get all the properties
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in Props)
            {
                //Setting column names as Property names
                dataTable.Columns.Add(prop.Name);
            }
            foreach (T item in items)
            {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {
                    //inserting property values to datatable rows
                    values[i] = Props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }

            foreach (string extraCollumn in ExtraCollumns)
            {
                dataTable.Columns.Remove(extraCollumn);
            }

            //put a breakpoint here and check datatable
            return dataTable;
        }

        public static string GetPropertyName<T>(Expression<Func<T, object>> expression)
        {
            var body = expression.Body as MemberExpression;

            if (body == null)
            {
                body = ((UnaryExpression)expression.Body).Operand as MemberExpression;
            }

            return body.Member.Name;
        }

        #region Email
        public static bool SendEmail(string ToEmail, string sub, string msg)
        {
            return SendEmail(ToEmail, sub, msg, null);
        }

        public static bool SendEmail(string ToEmail, string sub, string msg, string attachmentFileName)
        {
            if (!string.IsNullOrEmpty(ToEmail))
            {
                abHelper.Mail sendmail = new abHelper.Mail();
                sendmail.Subject = sub;
                sendmail.ToMailAddresses = ToEmail;
                sendmail.FromMailAddress = System.Configuration.ConfigurationManager.AppSettings["FromMailAddress"];
                sendmail.UserName = System.Configuration.ConfigurationManager.AppSettings["FromMailAddress"];
                sendmail.Password = System.Configuration.ConfigurationManager.AppSettings["FromMailPassword"];
                sendmail.IsUseCredentials = true;
                if (!string.IsNullOrEmpty(attachmentFileName))
                {
                    sendmail.AttachmentFilesWithPath = attachmentFileName;
                }

                sendmail.Host = System.Configuration.ConfigurationManager.AppSettings["EmailSMTP"];
                sendmail.Port = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["EmailPort"]);
                sendmail.IsSSL = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["EmailIsSSL"]);
                sendmail.IsBodyHTML = true;
                sendmail.Body = msg;
                sendmail.Send();
            }
            return true;
        }
        #endregion

        #region VerifyRecord
        public loanRecordStatus VerifyRecord(string tableName, int masterId, int linktoUserMaster)
        {
            SqlConnection SqlCon = null;
            SqlCommand SqlCmd = null;
            try
            {
                SqlCon = loanObjectFactoryDAL.CreateConnection();
                SqlCmd = new SqlCommand("loanVerifyRecord_Update", SqlCon);
                SqlCmd.CommandType = CommandType.StoredProcedure;
                SqlCmd.Parameters.Add("@TableName", SqlDbType.VarChar).Value = tableName;
                SqlCmd.Parameters.Add("@MasterId", SqlDbType.Int).Value = masterId;
                SqlCmd.Parameters.Add("@VerifiedDateTime", SqlDbType.DateTime).Value = loanGlobalsDAL.GetCurrentDateTime();
                SqlCmd.Parameters.Add("@linktoUserMasterId", SqlDbType.Int).Value = linktoUserMaster;
                SqlCmd.Parameters.Add("@Status", SqlDbType.SmallInt).Direction = ParameterDirection.Output;
                SqlCon.Open();
                SqlCmd.ExecuteNonQuery();
                SqlCon.Close();
                loanRecordStatus rs = (loanRecordStatus)(short)SqlCmd.Parameters["@Status"].Value;
                return rs;
            }
            catch (Exception ex)
            {
                loanGlobalsDAL.SaveError(ex);
                return loanRecordStatus.Error;
            }
            finally
            {
                loanObjectFactoryDAL.DisposeCommand(SqlCmd);
                loanObjectFactoryDAL.DisposeConnection(SqlCon);
            }
        }
        private List<loanGlobalsDAL> SetListPropertiesFromSqlDataReader(SqlDataReader sqlRdr)
        {
            List<loanGlobalsDAL> lstGlobals = new List<loanGlobalsDAL>();
            loanGlobalsDAL objGlobals = null;
            while (sqlRdr.Read())
            {
                objGlobals = new loanGlobalsDAL();
                objGlobals.Page = Convert.ToString(sqlRdr["Page"]);
                objGlobals.Url = Convert.ToString(sqlRdr["Url"]);
                objGlobals.Total = Convert.ToString(sqlRdr["Total"]);

                /// Extra
                lstGlobals.Add(objGlobals);
            }
            return lstGlobals;
        }
        public List<loanGlobalsDAL> SelectAllGetUnVerifiedRecordsPageWise(string Language)
        {
            SqlConnection SqlCon = null;
            SqlCommand SqlCmd = null;
            SqlDataReader SqlRdr = null;
            try
            {
                SqlCon = loanObjectFactoryDAL.CreateConnection();
                SqlCmd = new SqlCommand("loanGetUnverifiedRecordsPageWise_SelectAll", SqlCon);
                SqlCmd.CommandType = CommandType.StoredProcedure;
                SqlCmd.Parameters.Add("@Language", SqlDbType.NVarChar).Value = Language;
                SqlCon.Open();
                SqlRdr = SqlCmd.ExecuteReader();
                List<loanGlobalsDAL> lstGlobalsDAL = SetListPropertiesFromSqlDataReader(SqlRdr);
                SqlRdr.Close();
                SqlCon.Close();


                return lstGlobalsDAL;
            }
            catch (Exception ex)
            {

                loanGlobalsDAL.SaveError(ex);
                return null;
            }
            finally
            {
                loanObjectFactoryDAL.DisposeDataReader(SqlRdr);
                loanObjectFactoryDAL.DisposeCommand(SqlCmd);
                loanObjectFactoryDAL.DisposeConnection(SqlCon);
            }
        }

        #endregion
    }

    #endregion

    #region loanMessageDAL
    public class loanMessagesDAL
    {
        public const string NotLoggedIn = "NotLoggedIn";
        public const string Exception = "Exception";
        public const string PermissionDenied = "PermissionDenied";
        public const string AlreadyInUse = "AlreadyInUse";
        public const string AlreadyExist = "AlreadyExist";
        public const string NotFound = "NotFound";
        public const string InsertSuccess = "InsertSuccess";
        public const string InsertFail = "InsertFail";
        public const string UpdateSuccess = "UpdateSuccess";
        public const string UpdateFail = "UpdateFail";
        public const string DeleteSuccess = "DeleteSuccess";
        public const string DeleteFail = "DeleteFail";
        public const string SelectFail = "SelectFail";
        public const string SelectAllFail = "SelectAllFail";
        public const string InsertAllSuccess = "InsertAllSuccess";
        public const string InsertAllFail = "InsertAllFail";
        public const string UpdateAllSuccess = "UpdateAllSuccess";
        public const string UpdateAllFail = "UpdateAllFail";
        public const string DeleteAllSuccess = "DeleteAllSuccess";
        public const string DeleteAllFail = "DeleteAllFail";
        public const string SelectRecord = "SelectRecord";
        public const string DeleteRecord = "DeleteRecord";
        public const string DeleteAllRecords = "DeleteAllRecords";
        public const string DataLostWarning = "DataLostWarning";
    }
    #endregion

    #region Record Status
    public enum loanRecordStatus
    {
        RecordNotFound = -3,
        RecordAlreadyExist = -2,
        Error = -1,
        Success = 0
    }
    #endregion

    #region Role Rights
    public enum loanRoleRights
    {
        Custom,
        ViewList,
        ViewRecord,
        AddRecord,
        EditRecord,
        DeleteRecord

    }
    #endregion

    #region Enum
    public enum loanMessageIcon
    {
        None,
        Success,
        Error,
        Information,
        Warning
    }
    public enum loanContractStatus
    {
        Active = 1,
        Closed = 2,
        Cancelled = 3,
    }
    #endregion

    #region DropDownItem
    public class loanDropDownItem
    {
        public const string Select = "- SELECT -";
        public const string All = "- ALL -";
    }
    #endregion


}
