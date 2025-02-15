using loanLibrary;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Caching;
using System.Web.Security;
using System.Web.SessionState;

namespace abLOAN
{
    public class Global : System.Web.HttpApplication
    {
        private static CacheItemRemovedCallback OnCacheRemove = null;

        protected void Application_Start(object sender, EventArgs e)
        {
            //AddTask("DoStuff", 60);
        }

        protected void Session_End(object sender, EventArgs e)
        {
            if (Session[loanSessionsDAL.UserSession] != null)
            {
                loanUserTranDAL objUserTranDAL = new loanUserTranDAL();
                objUserTranDAL.LogoutDateTime = loanGlobalsDAL.GetCurrentDateTime();
                objUserTranDAL.SessionId = ((loanUser)Session[loanSessionsDAL.UserSession]).SessionId;
                loanRecordStatus rs = objUserTranDAL.UpdateUserTran();
            }
        }

        private void AddTask(string name, int seconds)
        {
            OnCacheRemove = new CacheItemRemovedCallback(CacheItemRemoved);
            HttpRuntime.Cache.Insert(name, seconds, null,
                DateTime.Now.AddSeconds(seconds), Cache.NoSlidingExpiration,
                CacheItemPriority.NotRemovable, OnCacheRemove);
        }

        public void CacheItemRemoved(string key, object value, CacheItemRemovedReason reason)
        {
            // do stuff here if it matches our taskname
            if (key == "DoStuff")
            {
                File.AppendAllText(System.Configuration.ConfigurationManager.AppSettings["FileSavePath"] + "bg.txt", DateTime.Now.ToString() + Environment.NewLine);
            }

            // re-add our task so it recurs
            AddTask(key, Convert.ToInt32(value));
        }
    }
}