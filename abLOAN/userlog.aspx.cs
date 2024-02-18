using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using loanLibrary;

namespace abLOAN
{
    public partial class userlog : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                loanSessionsDAL.CheckSession();

                if (!Page.IsPostBack)
                {
                    loanUser.CheckRoleRights(loanRoleRights.ViewList);

                    GetPageDefaults();

                    GetUser();

                    //       loanSessionDAL.RemoveSessionAllKeyValue();

                    FillUserTran();
                }
            }
            catch (Exception ex)
            {
                loanAppGlobals.SaveError(ex);
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                pgrUserTran.CurrentPage = 1;
                FillUserTran();
            }
            catch (Exception ex)
            {
                loanAppGlobals.SaveError(ex);
            }
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            try
            {
                ddlFilterUser.SelectedIndex = 0;

                pgrUserTran.CurrentPage = 1;
                FillUserTran();
            }
            catch (Exception ex)
            {
                loanAppGlobals.SaveError(ex);
            }
        }

        #region List Methods

        protected void lvUserTran_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            try
            {
                if (e.Item.ItemType == ListViewItemType.DataItem)
                {
                    loanUserTranDAL objUserTranDAL = (loanUserTranDAL)e.Item.DataItem;

                    Literal ltrlUsername = (Literal)e.Item.FindControl("ltrlUsername");
                    Literal ltrlSessionId = (Literal)e.Item.FindControl("ltrlSessionId");
                    Literal ltrlLoginDateTime = (Literal)e.Item.FindControl("ltrlLoginDateTime");
                    Literal ltrlLogoutDateTime = (Literal)e.Item.FindControl("ltrlLogoutDateTime");
                    Literal ltrlOS = (Literal)e.Item.FindControl("ltrlOS");
                    Literal ltrlIPAddress = (Literal)e.Item.FindControl("ltrlIPAddress");
                    Literal ltrlDeviceName = (Literal)e.Item.FindControl("ltrlDeviceName");
                    Literal ltrlBrowser = (Literal)e.Item.FindControl("ltrlBrowser");

                    ltrlUsername.Text = objUserTranDAL.Username;
                    ltrlSessionId.Text = objUserTranDAL.SessionId;
                    ltrlLoginDateTime.Text = loanGlobalsDAL.ConvertDateTimeToString(objUserTranDAL.LoginDateTime, loanAppGlobals.DateTimeFormat); ;
                    if (objUserTranDAL.LogoutDateTime != null)
                    {
                        ltrlLogoutDateTime.Text = loanGlobalsDAL.ConvertDateTimeToString(objUserTranDAL.LogoutDateTime, loanAppGlobals.DateTimeFormat); ;
                    }
                    ltrlOS.Text = objUserTranDAL.OS;
                    ltrlIPAddress.Text = objUserTranDAL.IPAddress;
                    ltrlDeviceName.Text = objUserTranDAL.DeviceName;
                    ltrlBrowser.Text = objUserTranDAL.Browser;
                }
            }
            catch (Exception ex)
            {
                loanAppGlobals.SaveError(ex);
            }
        }

        protected void pgrUserTran_ItemCommand(object sender, EventArgs e)
        {
            try
            {
                FillUserTran();
            }
            catch (Exception ex)
            {
                loanAppGlobals.SaveError(ex);
            }
        }

        #endregion

        #region Private Methods
        private void GetPageDefaults()
        {
            if (loanSessionsDAL.GetSessionKeyValue("CurrentPageUser") != null)
            {
                pgrUserTran.CurrentPage = Convert.ToInt16(loanSessionsDAL.GetSessionKeyValue("CurrentPageUser"));
            }
            if (loanSessionsDAL.GetSessionKeyValue("FilterUserLog") != null)
            {
                loanUserTranDAL objUserTranDAL = (loanUserTranDAL)loanSessionsDAL.GetSessionKeyValue("FilterUserLog");
                ddlFilterUser.SelectedValue = objUserTranDAL.linktoUserMasterId.ToString();
            }
        }

        private void FillUserTran()
        {

            loanUserTranDAL objUserTranDAL = new loanUserTranDAL();
            if (ddlFilterUser.SelectedValue != string.Empty)
            {
                objUserTranDAL.linktoUserMasterId = Convert.ToInt32(ddlFilterUser.SelectedValue);
            }

            loanSessionsDAL.SetSessionKeyValue("FilterUserLog", objUserTranDAL);
            loanSessionsDAL.SetSessionKeyValue("CurrentPageUser", pgrUserTran.CurrentPage);

            int TotalRecords;
            List<loanUserTranDAL> lstUserTran = objUserTranDAL.SelectAllUserTranPageWise(pgrUserTran.StartRowIndex, pgrUserTran.PageSize, out TotalRecords);
            pgrUserTran.TotalRowCount = TotalRecords;

            if (lstUserTran == null)
            {
                loanAppGlobals.ShowMessage(loanMessagesDAL.SelectAllFail, loanMessageIcon.Error);
                return;
            }

            if (lstUserTran.Count == 0 && pgrUserTran.TotalRowCount > 0)
            {
                pgrUserTran_ItemCommand(pgrUserTran, new EventArgs());
                return;
            }

            lvUserTran.DataSource = lstUserTran;
            lvUserTran.DataBind();

            if (lstUserTran.Count > 0)
            {
                int EndiIndex = pgrUserTran.StartRowIndex + pgrUserTran.PageSize < pgrUserTran.TotalRowCount ? pgrUserTran.StartRowIndex + pgrUserTran.PageSize : pgrUserTran.TotalRowCount;
                lblRecords.Text = string.Format(Resources.Messages.ResourceManager.GetString("Records"), pgrUserTran.StartRowIndex + 1, EndiIndex, pgrUserTran.TotalRowCount);
                lblRecords.Visible = true;
            }
            else
            {
                lblRecords.Visible = false;
            }

            if (pgrUserTran.TotalRowCount <= pgrUserTran.PageSize)
            {
                pgrUserTran.Visible = false;
            }
            else
            {
                pgrUserTran.Visible = true;
            }

        }

        private void GetUser()
        {
            ddlFilterUser.Items.Clear();
            ddlFilterUser.Items.Add(new System.Web.UI.WebControls.ListItem(loanDropDownItem.Select, ""));

            loanUserMasterDAL objUserMasterDAL = new loanUserMasterDAL();
            objUserMasterDAL.linktoCompanyMasterId = ((loanUser)Session[loanSessionsDAL.UserSession]).CompanyMasterId;
            int TotalRecords;
            List<loanUserMasterDAL> lstUserMaster = objUserMasterDAL.SelectAllUserMasterPageWise(pgrUserTran.StartRowIndex, 20, out TotalRecords);
            pgrUserTran.TotalRowCount = TotalRecords;

            if (lstUserMaster == null)
            {
                loanAppGlobals.ShowMessage(loanMessagesDAL.SelectAllFail, loanMessageIcon.Error);
                return;
            }
            foreach (loanUserMasterDAL obj in lstUserMaster)
            {
                ddlFilterUser.Items.Add(new System.Web.UI.WebControls.ListItem(obj.Username, obj.UserMasterId.ToString()));
            }
        }
        #endregion


    }
}
