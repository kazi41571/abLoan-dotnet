using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using loanLibrary;

namespace abLOAN
{
    public partial class role : BasePage
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

                    //      loanSessionDAL.RemoveSessionAllKeyValue();

                    FillRoleMaster();
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
                pgrRoleMaster.CurrentPage = 1;
                FillRoleMaster();
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
                txtFilterRole.Text = string.Empty;
                ddlFilterIsEnabled.SelectedIndex = 0;

                pgrRoleMaster.CurrentPage = 1;
                FillRoleMaster();
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
                loanRoleMasterDAL objRoleMasterDAL = new loanRoleMasterDAL();
                objRoleMasterDAL.linktoCompanyMasterId = ((loanUser)Session[loanSessionsDAL.UserSession]).CompanyMasterId;
                objRoleMasterDAL.Role = txtRole.Text.Trim();
                objRoleMasterDAL.Description = txtDescription.Text.Trim();
                objRoleMasterDAL.IsEnabled = chkIsEnabled.Checked;
                objRoleMasterDAL.UpdateDateTime = loanGlobalsDAL.GetCurrentDateTime();
                objRoleMasterDAL.SessionId = ((loanUser)Session[loanSessionsDAL.UserSession]).SessionId;

                if (string.IsNullOrEmpty(hdnAction.Value))
                {
                    loanUser.CheckRoleRights(loanRoleRights.AddRecord);

                    loanRecordStatus rsStatus = objRoleMasterDAL.InsertRoleMaster();
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
                        FillRoleMaster();
                    }
                }
                else
                {
                    objRoleMasterDAL.RoleMasterId = Convert.ToInt32(hdnRoleMasterId.Value);

                    if (objRoleMasterDAL.RoleMasterId == 1)
                    {
                        loanAppGlobals.ShowMessage(loanMessagesDAL.PermissionDenied, loanMessageIcon.Error);
                        return;
                    }
                    loanUser.CheckRoleRights(loanRoleRights.EditRecord);
                    loanRecordStatus rsStatus = objRoleMasterDAL.UpdateRoleMaster();
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
                        FillRoleMaster();
                    }
                }
            }
            catch (Exception ex)
            {
                loanAppGlobals.SaveError(ex);
            }
        }

        #region List Methods

        protected void lvRoleMaster_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            try
            {
                if (e.Item.ItemType == ListViewItemType.DataItem)
                {
                    loanRoleMasterDAL objRoleMasterDAL = (loanRoleMasterDAL)e.Item.DataItem;

                    Literal ltrlRole = (Literal)e.Item.FindControl("ltrlRole");
                    Literal ltrlDescription = (Literal)e.Item.FindControl("ltrlDescription");

                    ltrlRole.Text = objRoleMasterDAL.Role;
                    ltrlDescription.Text = objRoleMasterDAL.Description;

                    if (objRoleMasterDAL.RoleMasterId == 1)
                    {
                        LinkButton lbtnEdit = (LinkButton)e.Item.FindControl("lbtnEdit");
                        LinkButton lbtnDelete = (LinkButton)e.Item.FindControl("lbtnDelete");

                        lbtnEdit.Enabled = false;
                        lbtnDelete.Enabled = false;
                    }
                }
            }
            catch (Exception ex)
            {
                loanAppGlobals.SaveError(ex);
            }
        }

        protected void pgrRoleMaster_ItemCommand(object sender, EventArgs e)
        {
            try
            {
                FillRoleMaster();
            }
            catch (Exception ex)
            {
                loanAppGlobals.SaveError(ex);
            }
        }

        protected void lvRoleMaster_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName.Equals("EditRecord", StringComparison.CurrentCultureIgnoreCase))
                {
                    GetRoleMaster(Convert.ToInt32(((ListView)sender).DataKeys[e.Item.DataItemIndex].Value));
                }
                else if (e.CommandName.Equals("DeleteRecord", StringComparison.CurrentCultureIgnoreCase))
                {
                    loanRoleMasterDAL objRoleMasterDAL = new loanRoleMasterDAL();
                    objRoleMasterDAL.RoleMasterId = Convert.ToInt32(((ListView)sender).DataKeys[e.Item.DataItemIndex].Value);
                    objRoleMasterDAL.SessionId = ((loanUser)Session[loanSessionsDAL.UserSession]).SessionId;

                    if (objRoleMasterDAL.RoleMasterId == 1)
                    {
                        loanAppGlobals.ShowMessage(loanMessagesDAL.PermissionDenied, loanMessageIcon.Error);
                        return;
                    }
                    loanUser.CheckRoleRights(loanRoleRights.DeleteRecord);
                    loanRecordStatus rsStatus = objRoleMasterDAL.DeleteRoleMaster();
                    if (rsStatus == loanRecordStatus.Success)
                    {
                        loanAppGlobals.ShowMessage(loanMessagesDAL.DeleteSuccess, loanMessageIcon.Success);
                        FillRoleMaster();
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
                else if (e.CommandName.Equals("EditRoleRights", StringComparison.CurrentCultureIgnoreCase))
                {
                    Literal ltrlRole = (Literal)e.Item.FindControl("ltrlRole");
                    loanSessionsDAL.SetSessionKeyValue("Role", ltrlRole.Text);
                    loanSessionsDAL.SetSessionKeyValue("RoleMasterId", ((ListView)sender).DataKeys[e.Item.DataItemIndex].Value.ToString());
                    Response.Redirect("rolerights.aspx");
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
            if (loanSessionsDAL.GetSessionKeyValue("CurrentPageRole") != null)
            {
                pgrRoleMaster.CurrentPage = Convert.ToInt16(loanSessionsDAL.GetSessionKeyValue("CurrentPageRole"));
            }
            if (loanSessionsDAL.GetSessionKeyValue("FilterRole") != null)
            {
                loanRoleMasterDAL objRoleMasterDAL = (loanRoleMasterDAL)loanSessionsDAL.GetSessionKeyValue("FilterRole");
                txtFilterRole.Text = objRoleMasterDAL.Role;
                if (objRoleMasterDAL.IsEnabled)
                {
                    ddlFilterIsEnabled.SelectedValue = "Yes";
                }
                else
                {
                    ddlFilterIsEnabled.SelectedValue = "No";
                }
            }
        }

        private void FillRoleMaster()
        {

            loanRoleMasterDAL objRoleMasterDAL = new loanRoleMasterDAL();
            objRoleMasterDAL.Role = txtFilterRole.Text.Trim();
            if (ddlFilterIsEnabled.SelectedValue == "Yes")
            {
                objRoleMasterDAL.IsEnabled = true;
            }
            else if (ddlFilterIsEnabled.SelectedValue == "No")
            {
                objRoleMasterDAL.IsEnabled = false;
            }

            loanSessionsDAL.SetSessionKeyValue("FilterRole", objRoleMasterDAL);
            loanSessionsDAL.SetSessionKeyValue("CurrentPageRole", pgrRoleMaster.CurrentPage);

            int TotalRecords;
            List<loanRoleMasterDAL> lstRoleMaster = objRoleMasterDAL.SelectAllRoleMasterPageWise(pgrRoleMaster.StartRowIndex, pgrRoleMaster.PageSize, out TotalRecords);
            pgrRoleMaster.TotalRowCount = TotalRecords;

            if (lstRoleMaster == null)
            {
                loanAppGlobals.ShowMessage(loanMessagesDAL.SelectAllFail, loanMessageIcon.Error);
                return;
            }

            if (lstRoleMaster.Count == 0 && pgrRoleMaster.TotalRowCount > 0)
            {
                pgrRoleMaster_ItemCommand(pgrRoleMaster, new EventArgs());
                return;
            }

            lvRoleMaster.DataSource = lstRoleMaster;
            lvRoleMaster.DataBind();

            if (lstRoleMaster.Count > 0)
            {
                int EndiIndex = pgrRoleMaster.StartRowIndex + pgrRoleMaster.PageSize < pgrRoleMaster.TotalRowCount ? pgrRoleMaster.StartRowIndex + pgrRoleMaster.PageSize : pgrRoleMaster.TotalRowCount;
                lblRecords.Text = string.Format(Resources.Messages.ResourceManager.GetString("Records"), pgrRoleMaster.StartRowIndex + 1, EndiIndex, pgrRoleMaster.TotalRowCount);
                lblRecords.Visible = true;
            }
            else
            {
                lblRecords.Visible = false;
            }

            if (pgrRoleMaster.TotalRowCount <= pgrRoleMaster.PageSize)
            {
                pgrRoleMaster.Visible = false;
            }
            else
            {
                pgrRoleMaster.Visible = true;
            }

        }


        private void GetRoleMaster(int RoleMasterId)
        {
            loanUser.CheckRoleRights(loanRoleRights.ViewRecord);

            loanRoleMasterDAL objRoleMasterDAL = new loanRoleMasterDAL();
            objRoleMasterDAL.RoleMasterId = RoleMasterId;
            if (!objRoleMasterDAL.SelectRoleMaster())
            {
                loanAppGlobals.ShowMessage(loanMessagesDAL.SelectFail, loanMessageIcon.Error);
                return;
            }
            hdnRoleMasterId.Value = objRoleMasterDAL.RoleMasterId.ToString();
            txtRole.Text = objRoleMasterDAL.Role;
            txtDescription.Text = objRoleMasterDAL.Description;
            chkIsEnabled.Checked = objRoleMasterDAL.IsEnabled;

            hdnModel.Value = "show";
            hdnAction.Value = "edit";
        }
        #endregion


    }
}
