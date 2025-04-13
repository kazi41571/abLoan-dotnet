using loanLibrary;
using Resources;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Web;

namespace abLOAN
{
    public class loanAppGlobals
    {
        public static string DateFormat = "dd'/'MM'/'yyyy";
        public static string TimeFormat = "hh':'mm tt";
        public static string Time24HourFormat = "HH':'mm";
        public static string DateTimeFormat = "dd'/'MM'/'yyyy hh':'mm tt";

        public static void SaveError(Exception ex)
        {
            System.Threading.ThreadAbortException tae = ex as System.Threading.ThreadAbortException;
            if (tae != null)
            {
                return;
            }
            try
            {
                /// Insert error into ErrorLog table
                loanErrorLogDAL objErrorLogDAL = new loanErrorLogDAL();
                objErrorLogDAL.ErrorDateTime = loanGlobalsDAL.GetCurrentDateTime();
                objErrorLogDAL.ErrorMessage = ex.Message;
                objErrorLogDAL.ErrorStackTrace = ex.StackTrace;

                if (objErrorLogDAL.InsertErrorLog() == loanRecordStatus.Error)
                {
                    /// Write error into ErrorLog file if error not inserted
                    string ErrorLogPath = System.Configuration.ConfigurationManager.AppSettings["FileSavePath"];
                    if (!Directory.Exists(ErrorLogPath))
                    {
                        Directory.CreateDirectory(ErrorLogPath);
                    }
                    StringBuilder sbError = new StringBuilder();
                    sbError.AppendLine("Error DateTime    : " + loanGlobalsDAL.GetCurrentDateTime().ToString("s"));
                    sbError.AppendLine("Error Message     : " + ex.Message);
                    sbError.AppendLine("Error StackTrace  : " + ex.StackTrace);
                    sbError.AppendLine(string.Empty.PadRight(100, '-'));
                    File.WriteAllText(ErrorLogPath + "ErrorLog.txt", sbError.ToString());
                }
            }
            catch
            {
            }

            ShowMessage(loanMessagesDAL.Exception, loanMessageIcon.Error);
        }

        public static void ShowMessage(string key, loanMessageIcon messageIcon, string message = null)
        {
            Hashtable htSession = new Hashtable();
            if (!string.IsNullOrEmpty(key))
            {
                message = Messages.ResourceManager.GetString(key);
            }
            htSession.Add("Message", message);
            htSession.Add("MessageIcon", messageIcon);
            HttpContext.Current.Session[loanSessionsDAL.MessageSession] = htSession;
        }

        public static void SendOutFile(string filePathWithName, string fileName)
        {
            try
            {
                if (File.Exists(filePathWithName) == false)
                {
                    return;
                }
                if (string.IsNullOrEmpty(fileName))
                {
                    fileName = Path.GetFileName(filePathWithName);
                }
                var response = HttpContext.Current.Response;
                response.Clear();
                response.ClearContent();
                response.ClearHeaders();
                response.ContentType = GetMimeType(fileName);
                response.Charset = string.Empty;
                response.Cache.SetCacheability(HttpCacheability.Public);
                response.AddHeader("Content-Disposition", "attachment; filename=\"" + fileName + "\"");
                response.TransmitFile(filePathWithName);
                response.Flush();
                HttpContext.Current.ApplicationInstance.CompleteRequest();
            }
            catch (ThreadAbortException)
            {
                // This is expected, do nothing
            }
            catch (Exception ex)
            {
                SaveError(ex);
            }
        }

        public static bool ExportCsv<T>(List<T> list, string[] headers, string[] columns, string filePathWithName)
        {
            try
            {
                var sb = new StringBuilder();
                var header = "";
                var info = typeof(T).GetProperties();
                if (!File.Exists(filePathWithName))
                {
                    if (!Directory.Exists(Path.GetDirectoryName(filePathWithName)))
                    {
                        Directory.CreateDirectory(Path.GetDirectoryName(filePathWithName));
                    }
                    var file = File.Create(filePathWithName);
                    file.Close();
                }
                else
                {
                    File.Delete(filePathWithName);
                }
                foreach (var prop in headers)
                {
                    header += "\"" + prop + "\";";
                }
                header = header.Substring(0, header.Length - 1);
                sb.AppendLine(header);
                TextWriter sw = new StreamWriter(filePathWithName, true, Encoding.UTF8);
                sw.Write(sb.ToString());
                sw.Close();

                foreach (var obj in list)
                {
                    sb = new StringBuilder();
                    var line = "";
                    System.Reflection.PropertyInfo pro;
                    foreach (var prop in columns)
                    {
                        pro = info.First(f => f.Name == prop);
                        if (pro != null)
                        {
                            if (pro.GetValue(obj, null) != null && (pro.PropertyType == typeof(DateTime) || pro.PropertyType == typeof(DateTime?)))
                            {
                                if (((DateTime)pro.GetValue(obj, null)).ToString("HHmmss") == "000000")
                                {
                                    line += "\"" + loanGlobalsDAL.ConvertDateTimeToString((DateTime)pro.GetValue(obj, null), loanAppGlobals.DateFormat)?.Replace(Environment.NewLine, " ") + "\";";
                                }
                                else
                                {
                                    line += "\"" + loanGlobalsDAL.ConvertDateTimeToString((DateTime)pro.GetValue(obj, null), loanAppGlobals.DateTimeFormat)?.Replace(Environment.NewLine, " ") + "\";";
                                }
                            }
                            else
                            {
                                //line += "\"" + pro.GetValue(obj, null)?.ToString().Replace(Environment.NewLine, " ") + "\";";
                                line += "\"" + pro.GetValue(obj, null)?
                                                    .ToString()
                                                    .Replace("\r\n", " ")
                                                    .Replace("\n", " ")
                                                    .Replace("\r", " ") + "\";";
                            }
                        }
                    }
                    line = line.Substring(0, line.Length - 1);
                    sb.AppendLine(line);
                    sw = new StreamWriter(filePathWithName, true, Encoding.UTF8);
                    sw.Write(sb.ToString());
                    sw.Close();
                }
                return true;
            }
            catch (Exception ex)
            {
                loanAppGlobals.SaveError(ex);
                return false;
            }
        }

        public static string GetOSInfo()
        {
            var ua = HttpContext.Current.Request.UserAgent;

            if (ua.Contains("Android"))
            {
                return string.Format("Android {0}", GetOSVersion(ua, "Android"));
            }
            if (ua.Contains("iPad"))
            {
                return string.Format("iPad OS {0}", GetOSVersion(ua, "OS"));
            }
            if (ua.Contains("iPhone"))
            {
                return string.Format("iPhone OS {0}", GetOSVersion(ua, "OS"));
            }
            if (ua.Contains("Linux") && ua.Contains("KFAPWI"))
            {
                return "Kindle Fire";
            }
            if (ua.Contains("RIM Tablet") || (ua.Contains("BB") && ua.Contains("Mobile")))
            {
                return "Black Berry";
            }
            if (ua.Contains("Windows Phone"))
            {
                return string.Format("Windows Phone {0}", GetOSVersion(ua, "Windows Phone"));
            }
            if (ua.Contains("Mac OS"))
            {
                return "Mac OS";
            }
            if (ua.Contains("Windows NT 5.1") || ua.Contains("Windows NT 5.2"))
            {
                return "Windows XP";
            }
            if (ua.Contains("Windows NT 6.0"))
            {
                return "Windows Vista";
            }
            if (ua.Contains("Windows NT 6.1"))
            {
                return "Windows 7";
            }
            if (ua.Contains("Windows NT 6.2"))
            {
                return "Windows 8";
            }
            if (ua.Contains("Windows NT 6.3"))
            {
                return "Windows 8.1";
            }
            if (ua.Contains("Windows NT 10"))
            {
                return "Windows 10";
            }
            //fallback to basic platform:
            return HttpContext.Current.Request.Browser.Platform + (ua.Contains("Mobile") ? " Mobile " : "");
        }

        #region Private Methods
        private static string GetMimeType(string fileName)
        {
            string mimeType = "application/unknown";
            string ext = System.IO.Path.GetExtension(fileName).ToLower();
            Microsoft.Win32.RegistryKey regKey = Microsoft.Win32.Registry.ClassesRoot.OpenSubKey(ext);
            if (regKey != null && regKey.GetValue("Content Type") != null)
            {
                mimeType = regKey.GetValue("Content Type").ToString();
            }
            return mimeType;
        }

        private static string GetOSVersion(string userAgent, string device)
        {
            var temp = userAgent.Substring(userAgent.IndexOf(device) + device.Length).TrimStart();
            var version = string.Empty;

            foreach (var character in temp)
            {
                var validCharacter = false;
                int test = 0;

                if (Int32.TryParse(character.ToString(), out test))
                {
                    version += character;
                    validCharacter = true;
                }

                if (character == '.' || character == '_')
                {
                    version += '.';
                    validCharacter = true;
                }

                if (validCharacter == false)
                {
                    break;
                }
            }

            return version;
        }
        #endregion
    }

    public class loanSessionsDAL
    {
        public const string UserSession = "UserSession";
        public const string MessageSession = "MessageSession";
        public const string DataSession = "DataSession";
        public const string UserAccessControl = "lstAccessControl";

        #region Session Methods
        public static void CheckSession()
        {
            if (HttpContext.Current.Session[loanSessionsDAL.UserSession] == null)
            {
                if (!HttpContext.Current.Request.Url.AbsolutePath.Contains("default.aspx"))
                {
                    HttpContext.Current.Response.Redirect("login.aspx?page=" + HttpContext.Current.Request.Url.AbsolutePath); // DO NOT ADD false HERE
                }
                else
                {
                    HttpContext.Current.Response.Redirect("login.aspx"); // DO NOT ADD false HERE
                }
            }
        }

        public static void SetSessionKeyValue(string key, object value)
        {
            Hashtable htSession;
            if (HttpContext.Current.Session[loanSessionsDAL.DataSession] != null)
            {
                htSession = (Hashtable)HttpContext.Current.Session[loanSessionsDAL.DataSession];
            }
            else
            {
                htSession = new Hashtable();
                HttpContext.Current.Session[loanSessionsDAL.DataSession] = htSession;
            }
            if (htSession.ContainsKey(key))
            {
                htSession[key] = value;
            }
            else
            {
                htSession.Add(key, value);
            }
        }

        public static object GetSessionKeyValue(string key)
        {
            if (HttpContext.Current.Session[loanSessionsDAL.DataSession] != null)
            {
                Hashtable htSession = (Hashtable)HttpContext.Current.Session[loanSessionsDAL.DataSession];

                if (htSession.ContainsKey(key))
                {
                    return htSession[key];
                }
            }
            return null;
        }

        public static void RemoveSessionKeyValue(string key)
        {
            if (HttpContext.Current.Session[loanSessionsDAL.DataSession] != null)
            {
                Hashtable htSession = (Hashtable)HttpContext.Current.Session[loanSessionsDAL.DataSession];

                if (htSession.ContainsKey(key))
                {
                    htSession.Remove(key);
                }
            }
        }

        public static void RemoveSessionAllKeyValue()
        {
            HttpContext.Current.Session[loanSessionsDAL.DataSession] = null;
        }
        #endregion
    }
}
