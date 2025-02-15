using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using loanLibrary;
using System.Web.UI.HtmlControls;
using Resources;
using System.IO;
using System.Linq;

namespace abLOAN
{
    public partial class user : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                loanSessionsDAL.CheckSession();

                if (!Page.IsPostBack)
                {
                    loanUser.CheckRoleRights(loanRoleRights.ViewList);

                    loanSessionsDAL.RemoveSessionAllKeyValue();

                    GetRole();
                    GetPageDefaults();
                    if (Request.QueryString.ToString().Contains("id="))
                    {
                        int id = Convert.ToInt32(Request.QueryString["id"].ToString());
                        GetUserMaster(id);
                    }
                    FillUserMaster();
                }
            }
            catch (Exception ex)
            {
                loanAppGlobals.SaveError(ex);
            }
        }

        protected void btnDownload_Click(object sender, EventArgs e)
        {
            loanUserMasterDAL objUserMasterDAL = new loanUserMasterDAL();
            objUserMasterDAL.Username = txtFilterUsername.Text.Trim();
            objUserMasterDAL.Role = ddlFilterRole.SelectedItem.Text.Trim();
            var IsVerified = ddlFilterVerification.SelectedItem.Text.Trim();
            if (IsVerified == "Yes")
            {
                objUserMasterDAL.IsVerified = true;
            }
            else
            {
                objUserMasterDAL.IsVerified = false;
            }
            objUserMasterDAL.linktoCompanyMasterId = ((loanUser)Session[loanSessionsDAL.UserSession]).CompanyMasterId;
          

            int TotalRecords;
            List<loanUserMasterDAL> lstUserMaster = objUserMasterDAL.SelectAllUserMasterPageWise(0, int.MaxValue, out TotalRecords);

            if (lstUserMaster == null)
            {
                loanAppGlobals.ShowMessage(loanMessagesDAL.SelectAllFail, loanMessageIcon.Error);
                return;
            }

       

            string file = System.Configuration.ConfigurationManager.AppSettings["FileSavePath"] + "User.csv"; 




            var parameterNames = typeof(loanUserMasterDAL).GetProperties()
            .Where(p => !p.GetGetMethod().IsVirtual && p.Name != "Password" && p.Name != "VerificationCode")
            .Select(p => p.Name) 
            .ToArray();
 


            bool IsSuccess = loanAppGlobals.ExportCsv(lstUserMaster, parameterNames, parameterNames, file);
            if (!IsSuccess)
            {
                loanAppGlobals.ShowMessage(loanMessagesDAL.Exception, loanMessageIcon.Error);
                return;
            }

            try
            {
                loanAppGlobals.SendOutFile(file, Path.GetFileName(file));
            }
            catch (System.Threading.ThreadAbortException)
            {
                // This is expected when downloading files - ignore it
            }
            catch (Exception ex)
            {
                loanAppGlobals.SaveError(ex);
                loanAppGlobals.ShowMessage(loanMessagesDAL.Exception, loanMessageIcon.Error);
            }
        }


        protected void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                pgrUserMaster.CurrentPage = 1;
                FillUserMaster();
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
                txtFilterUsername.Text = string.Empty;
                ddlFilterRole.SelectedIndex = 0;
                ddlFilterIsEnabled.SelectedIndex = 0;

                pgrUserMaster.CurrentPage = 1;
                FillUserMaster();
            }
            catch (Exception ex)
            {
                loanAppGlobals.SaveError(ex);
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                loanUserMasterDAL objUserMasterDAL = new loanUserMasterDAL();
                objUserMasterDAL.Username = txtUsername.Text.Trim();
                objUserMasterDAL.Password = txtPassword.Text.Trim();
                objUserMasterDAL.Email = txtEmail.Text.Trim();
                objUserMasterDAL.linktoRoleMasterId = Convert.ToInt32(ddlRole.SelectedValue);
                objUserMasterDAL.linktoCompanyMasterId = ((loanUser)Session[loanSessionsDAL.UserSession]).CompanyMasterId;
                objUserMasterDAL.Comment = txtComment.Text.Trim();
                objUserMasterDAL.IsEnabled = chkIsEnabled.Checked;

                objUserMasterDAL.UpdateDateTime = loanGlobalsDAL.GetCurrentDateTime();
                objUserMasterDAL.SessionId = ((loanUser)Session[loanSessionsDAL.UserSession]).SessionId;
                if (string.IsNullOrEmpty(hdnAction.Value))
                {
                    loanUser.CheckRoleRights(loanRoleRights.AddRecord);

                    loanRecordStatus rsStatus = objUserMasterDAL.InsertUserMaster(null, null);
                    if (rsStatus == loanRecordStatus.Error)
                    {
                        loanAppGlobals.ShowMessage(loanMessagesDAL.InsertFail, loanMessageIcon.Error);
                        return;
                    }
                    else if (rsStatus == loanRecordStatus.RecordAlreadyExist)
                    {
                        loanAppGlobals.ShowMessage(loanMessagesDAL.AlreadyExist, loanMessageIcon.Warning);
                        hdnModel.Value = "show";
                        return;
                    }
                    else if (rsStatus == loanRecordStatus.Success)
                    {
                        loanAppGlobals.ShowMessage(loanMessagesDAL.InsertSuccess, loanMessageIcon.Success);
                        if (((Button)sender).ID.Equals("btnSaveAndNew"))
                        {
                            hdnModel.Value = "clear";
                        }
                        else
                        {
                            hdnModel.Value = "hide";
                        }
                        FillUserMaster();
                    }
                }
                else
                {
                    loanUser.CheckRoleRights(loanRoleRights.EditRecord);

                    objUserMasterDAL.UserMasterId = Convert.ToInt32(hdnUserMasterId.Value);
                    if (objUserMasterDAL.UserMasterId == 1)
                    {
                        objUserMasterDAL.linktoRoleMasterId = 1;
                        objUserMasterDAL.IsEnabled = true;
                    }
                    loanRecordStatus rsStatus = objUserMasterDAL.UpdateUserMaster(null, null);
                    if (rsStatus == loanRecordStatus.Error)
                    {
                        loanAppGlobals.ShowMessage(loanMessagesDAL.UpdateFail, loanMessageIcon.Error);
                        return;
                    }
                    else if (rsStatus == loanRecordStatus.RecordAlreadyExist)
                    {
                        loanAppGlobals.ShowMessage(loanMessagesDAL.AlreadyExist, loanMessageIcon.Warning);
                        hdnModel.Value = "show";
                        return;
                    }
                    else if (rsStatus == loanRecordStatus.Success)
                    {
                        loanAppGlobals.ShowMessage(loanMessagesDAL.UpdateSuccess, loanMessageIcon.Success);
                        hdnModel.Value = "hide";
                        FillUserMaster();
                    }
                }
            }
            catch (Exception ex)
            {
                loanAppGlobals.SaveError(ex);
            }
        }

        protected void btnShowPassword_Click(object sender, EventArgs e)
        {
            // System.Resources.ResourceManager RM = new System.Resources.ResourceManager("abLoan.Resource", this.GetType().Assembly);
            //System.Resources.ResourceManager RM = new System.Resources.ResourceManager("abLoan.Resource", Assembly.GetExecutingAssembly());

            var showPassword = Resource.ResourceManager.GetString("btnShowPassword");
            var hidePassword = Resource.ResourceManager.GetString("btnHidePassword");

            HtmlTableCell thPassword = (HtmlTableCell)lvUserMaster.FindControl("thPassword");
            if (btnShowPassword.Text.Equals(showPassword))
            {
                //loanUser.CheckRoleRights(loanRoleRights.Custom, loanRoleRightsName.ShowUsersPassword);

                ViewState["ShowPassword"] = true;
                btnShowPassword.Text = hidePassword;
                btnShowPassword.CssClass = "btn btn-info";
                thPassword.Visible = true;
            }
            else
            {
                ViewState["ShowPassword"] = false;
                btnShowPassword.Text = showPassword;
                btnShowPassword.CssClass = "btn btn-info btn-outline";
                thPassword.Visible = false;
            }
            FillUserMaster();
        }

        #region List Methods

        protected void lvUserMaster_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            try
            {
                if (e.Item.ItemType == ListViewItemType.DataItem)
                {
                    loanUserMasterDAL objUserMasterDAL = (loanUserMasterDAL)e.Item.DataItem;

                    Literal ltrlUsername = (Literal)e.Item.FindControl("ltrlUsername");
                    Literal ltrlPassword = (Literal)e.Item.FindControl("ltrlPassword");
                    Literal ltrlRole = (Literal)e.Item.FindControl("ltrlRole");
                    Literal ltrlLastLoginDateTime = (Literal)e.Item.FindControl("ltrlLastLoginDateTime");
                    HtmlTableCell tdPassword = (HtmlTableCell)e.Item.FindControl("tdPassword");
                    LinkButton lbtnUnlock = (LinkButton)e.Item.FindControl("lbtnUnlock");
                    LinkButton lbtnEdit = (LinkButton)e.Item.FindControl("lbtnEdit");
                    LinkButton lbtnDelete = (LinkButton)e.Item.FindControl("lbtnDelete");
                    Literal ltrlModifiedBy = (Literal)e.Item.FindControl("ltrlModifiedBy");
                    ltrlModifiedBy.Text = objUserMasterDAL.ModifiedBy;
                    Literal ltrlModifiedDateTime = (Literal)e.Item.FindControl("ltrlModifiedDateTime");
                    ltrlModifiedDateTime.Text = loanGlobalsDAL.ConvertDateTimeToString(objUserMasterDAL.UpdateDateTime, loanAppGlobals.DateTimeFormat);

                    ltrlUsername.Text = objUserMasterDAL.Username;
                    ltrlPassword.Text = objUserMasterDAL.Password;
                    if (ViewState["ShowPassword"] != null && Convert.ToBoolean(ViewState["ShowPassword"]) == true)
                    {
                        tdPassword.Visible = true;
                    }
                    else
                    {
                        tdPassword.Visible = false;
                    }
                    ltrlRole.Text = objUserMasterDAL.Role;
                    if (objUserMasterDAL.LastLoginDateTime != null)
                    {
                        ltrlLastLoginDateTime.Text = loanGlobalsDAL.ConvertDateTimeToString(objUserMasterDAL.LastLoginDateTime, loanAppGlobals.DateTimeFormat); ;
                    }
                    if (objUserMasterDAL.LoginFailCount >= 7)
                    {
                        lbtnUnlock.Visible = true;
                    }

                    if (objUserMasterDAL.IsVerified != null)
                    {
                        LinkButton lbtnVerify = (LinkButton)e.Item.FindControl("lbtnVerify");
                        lbtnVerify.Visible = false;
                        Literal ltrlVerifiedBy = (Literal)e.Item.FindControl("ltrlVerifiedBy");
                        ltrlVerifiedBy.Text = objUserMasterDAL.VerifiedBy;
                        if (objUserMasterDAL.VerifiedDateTime != null)
                        {
                            Literal ltrlVerifiedDateTime = (Literal)e.Item.FindControl("ltrlVerifiedDateTime");
                            ltrlVerifiedDateTime.Text = loanGlobalsDAL.ConvertDateTimeToString(objUserMasterDAL.VerifiedDateTime.Value, loanAppGlobals.DateTimeFormat);
                        }
                    }
                    if (loanUser.GetRoleRights(loanRoleRights.Custom, "VerifyRecord") == false)
                    {
                        LinkButton lbtnVerify = (LinkButton)e.Item.FindControl("lbtnVerify");
                        lbtnVerify.Visible = false;
                    }
                }
            }
            catch (Exception ex)
            {
                loanAppGlobals.SaveError(ex);
            }
        }

        protected void pgrUserMaster_ItemCommand(object sender, EventArgs e)
        {
            try
            {
                FillUserMaster();
            }
            catch (Exception ex)
            {
                loanAppGlobals.SaveError(ex);
            }
        }

        protected void lvUserMaster_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName.Equals("UnlockRecord", StringComparison.CurrentCultureIgnoreCase))
                {
                    if (((loanUser)Session[loanSessionsDAL.UserSession]).linktoRoleMasterId != 1)
                    {
                        loanAppGlobals.ShowMessage(loanMessagesDAL.PermissionDenied, loanMessageIcon.Error);
                        return;
                    }

                    loanUserMasterDAL objUserMasterDAL = new loanUserMasterDAL();
                    objUserMasterDAL.UserMasterId = Convert.ToInt32(((ListView)sender).DataKeys[e.Item.DataItemIndex].Value);
                    objUserMasterDAL.UpdateDateTime = loanGlobalsDAL.GetCurrentDateTime();
                    objUserMasterDAL.SessionId = ((loanUser)Session[loanSessionsDAL.UserSession]).SessionId;
                    loanRecordStatus rsStatus = objUserMasterDAL.UpdateUserMasterUnlock();
                    if (rsStatus == loanRecordStatus.Success)
                    {
                        loanAppGlobals.ShowMessage(loanMessagesDAL.UpdateSuccess, loanMessageIcon.Success);
                        FillUserMaster();
                    }
                    else
                    {
                        loanAppGlobals.ShowMessage(loanMessagesDAL.UpdateFail, loanMessageIcon.Error);
                    }
                }
                else if (e.CommandName.Equals("VerifyRecord", StringComparison.CurrentCultureIgnoreCase))
                {
                    string pageName = Path.GetFileNameWithoutExtension(Page.AppRelativeVirtualPath) + "master";
                    VerifyRecord(pageName, Convert.ToInt32(((ListView)sender).DataKeys[e.Item.DataItemIndex].Value));
                }
                else if (e.CommandName.Equals("EditRecord", StringComparison.CurrentCultureIgnoreCase))
                {
                    GetUserMaster(Convert.ToInt32(((ListView)sender).DataKeys[e.Item.DataItemIndex].Value));
                }
                else if (e.CommandName.Equals("DeleteRecord", StringComparison.CurrentCultureIgnoreCase))
                {
                    loanUser.CheckRoleRights(loanRoleRights.DeleteRecord);

                    loanUserMasterDAL objUserMasterDAL = new loanUserMasterDAL();
                    objUserMasterDAL.UserMasterId = Convert.ToInt32(((ListView)sender).DataKeys[e.Item.DataItemIndex].Value);
                    objUserMasterDAL.SessionId = ((loanUser)Session[loanSessionsDAL.UserSession]).SessionId;

                    if (objUserMasterDAL.UserMasterId == 1)
                    {
                        loanAppGlobals.ShowMessage(loanMessagesDAL.AlreadyInUse, loanMessageIcon.Warning);
                        return;
                    }
                    objUserMasterDAL.SessionId = ((loanUser)Session[loanSessionsDAL.UserSession]).SessionId;
                    loanRecordStatus rsStatus = objUserMasterDAL.DeleteUserMaster();
                    if (rsStatus == loanRecordStatus.Success)
                    {
                        loanAppGlobals.ShowMessage(loanMessagesDAL.DeleteSuccess, loanMessageIcon.Success);
                        FillUserMaster();
                    }
                    else if (rsStatus == loanRecordStatus.RecordAlreadyExist)
                    {
                        loanAppGlobals.ShowMessage(loanMessagesDAL.AlreadyInUse, loanMessageIcon.Warning);
                    }
                    else
                    {
                        loanAppGlobals.ShowMessage(loanMessagesDAL.DeleteFail, loanMessageIcon.Error);
                    }
                }
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
                pgrUserMaster.CurrentPage = Convert.ToInt16(loanSessionsDAL.GetSessionKeyValue("CurrentPageUser"));
            }
            if (loanSessionsDAL.GetSessionKeyValue("FilterUser") != null)
            {
                loanUserMasterDAL objUserMasterDAL = (loanUserMasterDAL)loanSessionsDAL.GetSessionKeyValue("FilterUser");
                txtFilterUsername.Text = objUserMasterDAL.Username;
                ddlFilterRole.SelectedValue = objUserMasterDAL.linktoRoleMasterId.ToString();
                if (objUserMasterDAL.IsEnabled)
                {
                    ddlFilterIsEnabled.SelectedValue = "Yes";
                }
                else
                {
                    ddlFilterIsEnabled.SelectedValue = "No";
                }
            }
        }

        private void FillUserMaster()
        {

            loanUserMasterDAL objUserMasterDAL = new loanUserMasterDAL();
            objUserMasterDAL.linktoCompanyMasterId = ((loanUser)Session[loanSessionsDAL.UserSession]).CompanyMasterId;
            if (txtFilterUsername.Text != "")
            {
                objUserMasterDAL.Username = txtFilterUsername.Text.Trim();
            }
            if (ddlFilterRole.SelectedValue != string.Empty)
            {
                objUserMasterDAL.linktoRoleMasterId = Convert.ToInt32(ddlFilterRole.SelectedValue);
            }
            if (ddlFilterIsEnabled.SelectedValue == "Yes")
            {
                objUserMasterDAL.IsEnabled = true;
            }
            else if (ddlFilterIsEnabled.SelectedValue == "No")
            {
                objUserMasterDAL.IsEnabled = false;
            }
            if (ddlFilterVerification.SelectedValue == "Yes")
            {
                objUserMasterDAL.IsVerified = true;
            }
            else if (ddlFilterVerification.SelectedValue == "No")
            {
                objUserMasterDAL.IsVerified = false;
            }
            loanSessionsDAL.SetSessionKeyValue("FilterUser", objUserMasterDAL);
            loanSessionsDAL.SetSessionKeyValue("CurrentPageUser", pgrUserMaster.CurrentPage);

            int TotalRecords;
            List<loanUserMasterDAL> lstUserMaster = objUserMasterDAL.SelectAllUserMasterPageWise(pgrUserMaster.StartRowIndex, pgrUserMaster.PageSize, out TotalRecords);
            pgrUserMaster.TotalRowCount = TotalRecords;

            if (lstUserMaster == null)
            {
                loanAppGlobals.ShowMessage(loanMessagesDAL.SelectAllFail, loanMessageIcon.Error);
                return;
            }

            if (lstUserMaster.Count == 0 && pgrUserMaster.TotalRowCount > 0)
            {
                pgrUserMaster_ItemCommand(pgrUserMaster, new EventArgs());
                return;
            }

            lvUserMaster.DataSource = lstUserMaster;
            lvUserMaster.DataBind();

            if (lstUserMaster.Count > 0)
            {
                int EndiIndex = pgrUserMaster.StartRowIndex + pgrUserMaster.PageSize < pgrUserMaster.TotalRowCount ? pgrUserMaster.StartRowIndex + pgrUserMaster.PageSize : pgrUserMaster.TotalRowCount;
                lblRecords.Text = string.Format(Resources.Messages.ResourceManager.GetString("Records"), pgrUserMaster.StartRowIndex + 1, EndiIndex, pgrUserMaster.TotalRowCount);
                lblRecords.Visible = true;
            }
            else
            {
                lblRecords.Visible = false;
            }

            if (pgrUserMaster.TotalRowCount <= pgrUserMaster.PageSize)
            {
                pgrUserMaster.Visible = false;
            }
            else
            {
                pgrUserMaster.Visible = true;
            }

        }

        private void GetRole()
        {
            ddlRole.Items.Clear();
            ddlRole.Items.Add(new System.Web.UI.WebControls.ListItem(loanDropDownItem.Select, ""));

            ddlFilterRole.Items.Clear();
            ddlFilterRole.Items.Add(new System.Web.UI.WebControls.ListItem(loanDropDownItem.Select, ""));

            List<loanRoleMasterDAL> lstRoleMasterDAL = loanRoleMasterDAL.SelectAllRoleMasterRole();
            if (lstRoleMasterDAL == null)
            {
                loanAppGlobals.ShowMessage(loanMessagesDAL.SelectAllFail, loanMessageIcon.Error);
                return;
            }

            foreach (loanRoleMasterDAL obj in lstRoleMasterDAL)
            {
                ddlRole.Items.Add(new System.Web.UI.WebControls.ListItem(obj.Role, obj.RoleMasterId.ToString()));
                ddlFilterRole.Items.Add(new System.Web.UI.WebControls.ListItem(obj.Role, obj.RoleMasterId.ToString()));
            }
        }

        private void GetUserMaster(int UserMasterId)
        {
            loanUser.CheckRoleRights(loanRoleRights.ViewRecord);

            loanUserMasterDAL objUserMasterDAL = new loanUserMasterDAL();
            objUserMasterDAL.UserMasterId = UserMasterId;
            if (!objUserMasterDAL.SelectUserMaster())
            {
                loanAppGlobals.ShowMessage(loanMessagesDAL.SelectFail, loanMessageIcon.Error);
                return;
            }
            hdnUserMasterId.Value = objUserMasterDAL.UserMasterId.ToString();
            txtUsername.Text = objUserMasterDAL.Username;
            txtPassword.Attributes["value"] = objUserMasterDAL.Password;
            txtEmail.Text = objUserMasterDAL.Email;
            ddlRole.SelectedIndex = ddlRole.Items.IndexOf(ddlRole.Items.FindByValue(objUserMasterDAL.linktoRoleMasterId.ToString()));
            txtComment.Text = objUserMasterDAL.Comment;
            chkIsEnabled.Checked = objUserMasterDAL.IsEnabled;

            hdnModel.Value = "show";
            hdnAction.Value = "edit";
        }

        private void VerifyRecord(string pageName, int masterId)
        {
            loanUser.CheckRoleRights(loanRoleRights.Custom, "VerifyRecord");
            loanGlobalsDAL objGlobasDAL = new loanGlobalsDAL();
            int linktoUserMasterId = ((loanUser)Session[loanSessionsDAL.UserSession]).UserMasterId;

            loanRecordStatus rsStatus = objGlobasDAL.VerifyRecord(pageName, masterId, linktoUserMasterId);
            if (rsStatus == loanRecordStatus.Error)
            {
                loanAppGlobals.ShowMessage(loanMessagesDAL.UpdateFail, loanMessageIcon.Error);
                return;
            }
            else if (rsStatus == loanRecordStatus.Success)
            {
                loanAppGlobals.ShowMessage(loanMessagesDAL.UpdateSuccess, loanMessageIcon.Success);
                FillUserMaster();
            }
        }
        #endregion
    }
}
