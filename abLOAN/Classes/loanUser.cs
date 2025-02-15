using loanLibrary;
using System;
using System.Collections.Generic;
using System.Web;

namespace abLOAN
{
    public class loanUser
    {
        #region Properties
        public int UserMasterId { get; set; }
        public string Username { get; set; }
        public int linktoRoleMasterId { get; set; }
        public string SessionId { get; set; }
        public int CompanyMasterId { get; set; }
        #endregion

        public loanUser(int userMasterId, string userName, int roleMasterId, string sessionId, int companyMasterId)
        {
            this.UserMasterId = userMasterId;
            this.Username = userName;
            this.linktoRoleMasterId = roleMasterId;
            this.SessionId = sessionId;
            this.CompanyMasterId = companyMasterId;
        }

        public static void CheckRoleRights(loanRoleRights roleRight, string name = null) /// TO DENY ACCESS
        {
            try
            {
                if (GetRoleRights(roleRight, name) == false)
                {
                    if (HttpContext.Current.Request.UrlReferrer == null)
                    {
                        loanAppGlobals.ShowMessage(loanMessagesDAL.PermissionDenied, loanMessageIcon.Error);
                        HttpContext.Current.Response.Redirect("default.aspx"); // DO NOT ADD false HERE
                    }
                    else if (HttpContext.Current.Request.UrlReferrer.ToString().Contains("login.aspx"))
                    {
                        HttpContext.Current.Response.Redirect("default.aspx"); // DO NOT ADD false HERE
                    }
                    else
                    {
                        loanAppGlobals.ShowMessage(loanMessagesDAL.PermissionDenied, loanMessageIcon.Error);
                        HttpContext.Current.Response.Redirect(HttpContext.Current.Request.UrlReferrer.ToString()); // DO NOT ADD false HERE
                    }
                }
            }
            catch { }
        }

        public static bool GetRoleRights(loanRoleRights roleRight, string name = null) /// TO USE IN CONDITION ONLY
        {
            loanRoleRightsTranDAL objRoleRightsTranDAL;
            List<loanRoleRightsTranDAL> lstRoleRightsTranDAL;
            try
            {
                if (HttpContext.Current.Session[loanSessionsDAL.UserSession] == null)
                {
                    return false;
                }
                if (((loanUser)HttpContext.Current.Session[loanSessionsDAL.UserSession]).linktoRoleMasterId == 1)
                {
                    return true;
                }
                lstRoleRightsTranDAL = (List<loanRoleRightsTranDAL>)HttpContext.Current.Session[loanSessionsDAL.UserAccessControl];
                if (lstRoleRightsTranDAL == null)
                {
                    objRoleRightsTranDAL = new loanRoleRightsTranDAL();
                    objRoleRightsTranDAL.linktoRoleMasterId = ((loanUser)HttpContext.Current.Session[loanSessionsDAL.UserSession]).linktoRoleMasterId;
                    lstRoleRightsTranDAL = objRoleRightsTranDAL.SelectAllRoleRightsTran();
                    if (lstRoleRightsTranDAL == null)
                    {
                        loanAppGlobals.ShowMessage(loanMessagesDAL.SelectFail, loanMessageIcon.Error);
                        return false;
                    }
                    HttpContext.Current.Session[loanSessionsDAL.UserAccessControl] = lstRoleRightsTranDAL;
                }

                string PageName = name;
                if (string.IsNullOrEmpty(PageName))
                {
                    PageName = HttpContext.Current.Request.RawUrl.Substring(HttpContext.Current.Request.RawUrl.LastIndexOf('/') + 1);
                    if (PageName.Contains("?"))
                    {
                        PageName = PageName.Substring(0, HttpContext.Current.Request.RawUrl.IndexOf('?') - 1);
                    }
                }

                if (roleRight == loanRoleRights.Custom && lstRoleRightsTranDAL.Find(x => x.PageName.Equals(PageName, StringComparison.InvariantCultureIgnoreCase)) != null)
                {
                    return true;
                }
                else if (roleRight == loanRoleRights.ViewList && lstRoleRightsTranDAL.Find(x => x.PageName.Equals(PageName, StringComparison.InvariantCultureIgnoreCase) && x.IsViewList == true) != null)
                {
                    return true;
                }
                else if (roleRight == loanRoleRights.ViewRecord && lstRoleRightsTranDAL.Find(x => x.PageName.Equals(PageName, StringComparison.InvariantCultureIgnoreCase) && x.IsViewRecord == true) != null)
                {
                    return true;
                }
                else if (roleRight == loanRoleRights.AddRecord && lstRoleRightsTranDAL.Find(x => x.PageName.Equals(PageName, StringComparison.InvariantCultureIgnoreCase) && x.IsAddRecord == true) != null)
                {
                    return true;
                }
                else if (roleRight == loanRoleRights.EditRecord && lstRoleRightsTranDAL.Find(x => x.PageName.Equals(PageName, StringComparison.InvariantCultureIgnoreCase) && x.IsEditRecord == true) != null)
                {
                    return true;
                }
                else if (roleRight == loanRoleRights.DeleteRecord && lstRoleRightsTranDAL.Find(x => x.PageName.Equals(PageName, StringComparison.InvariantCultureIgnoreCase) && x.IsDeleteRecord == true) != null)
                {
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                loanAppGlobals.SaveError(ex);
                return false;
            }
            finally
            {
                lstRoleRightsTranDAL = null;
                objRoleRightsTranDAL = null;
            }
        }
    }
}
