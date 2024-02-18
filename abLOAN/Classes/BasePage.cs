using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading;
using System.Globalization;

namespace abLOAN
{
    public class BasePage : System.Web.UI.Page
    {

        protected override void InitializeCulture()
        {
            
            string Language = Convert.ToString(Session["Language"]);
            
            if (Request.Form["__EVENTTARGET"] != null && Request.Form["__EVENTTARGET"].Contains("ddlLanguage"))
            {
                if (Language.Contains("en-us"))
                {
                    Thread.CurrentThread.CurrentCulture = new CultureInfo("ar-sa");
                    Thread.CurrentThread.CurrentUICulture = new CultureInfo("ar-sa");
                    Session["Language"] = "ar-sa";
                    
                }
                else if (Language.Contains("ar-sa"))
                {
                    Thread.CurrentThread.CurrentCulture = new CultureInfo("en-us");
                    Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-us");
                    Session["Language"] = "en-us";
                }
            }
            else
            {
                if (Language.Contains("en-us"))
                {                    
                    Thread.CurrentThread.CurrentCulture = new CultureInfo("en-us");
                    Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-us");
                }
                else if (Language.Contains("ar-sa"))
                {
                    Thread.CurrentThread.CurrentCulture = new CultureInfo("ar-sa");
                    Thread.CurrentThread.CurrentUICulture = new CultureInfo("ar-sa");
                }
                else
                {
                    Thread.CurrentThread.CurrentCulture = new CultureInfo("ar-sa");
                    Thread.CurrentThread.CurrentUICulture = new CultureInfo("ar-sa");
                    Session["Language"] = "ar-sa";
                }
            }
        }
    }
}