using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using loanLibrary;
using System.Web;
using System.IO;

namespace abLOAN
{
    public partial class bank : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
              //  loanSessionDAL.CheckSession();

                if (!Page.IsPostBack)
                {
                    //loanUser.CheckUserRights(loanUserRights.ViewBank);
                    loanUser.CheckRoleRights(loanRoleRights.ViewList);
                    GetPageDefaults();

                    //					loanSessionDAL.RemoveSessionAllKeyValue();

                    if (Request.QueryString.ToString().Contains("id="))
                    {
                        int id = Convert.ToInt32(Request.QueryString["id"].ToString());
                        GetBankMaster(id);
                    }

                    FillBankMaster();
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
                pgrBankMaster.CurrentPage = 1;
                FillBankMaster();
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
                txtFilterBankName.Text = string.Empty;
                ddlFilterIsEnabled.SelectedIndex = 0;
                ddlFilterVerification.SelectedIndex = 0;

                pgrBankMaster.CurrentPage = 1;
                FillBankMaster();
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
                loanBankMasterDAL objBankMasterDAL = new loanBankMasterDAL();
                objBankMasterDAL.linktoCompanyMasterId = ((loanUser)Session[loanSessionsDAL.UserSession]).CompanyMasterId;
                objBankMasterDAL.BankName = txtBankName.Text.Trim();
                objBankMasterDAL.IsEnabled = chkIsEnabled.Checked;
                objBankMasterDAL.UpdateDateTime = loanGlobalsDAL.GetCurrentDateTime();
                objBankMasterDAL.SessionId = ((loanUser)Session[loanSessionsDAL.UserSession]).SessionId;

                if (string.IsNullOrEmpty(hdnActionBank.Value))
                {
                    loanUser.CheckRoleRights(loanRoleRights.AddRecord);

                    loanRecordStatus rsStatus = objBankMasterDAL.InsertBankMaster();
                    if (rsStatus == loanRecordStatus.Error)
                    {
                        loanAppGlobals.ShowMessage(loanMessagesDAL.InsertFail, loanMessageIcon.Error);
                        return;
                    }
                    else if (rsStatus == loanRecordStatus.RecordAlreadyExist)
                    {
                        loanAppGlobals.ShowMessage(loanMessagesDAL.AlreadyExist, loanMessageIcon.Warning);
                        hdnModelBank.Value = "show";
                        return;
                    }
                    else if (rsStatus == loanRecordStatus.Success)
                    {
                        loanAppGlobals.ShowMessage(loanMessagesDAL.InsertSuccess, loanMessageIcon.Success);
                        if (((Button)sender).ID.Equals("btnSaveAndNew"))
                        {
                            hdnModelBank.Value = "clear";
                        }
                        else
                        {
                            hdnModelBank.Value = "hide";
                        }
                        FillBankMaster();
                    }
                }
                else
                {
                    loanUser.CheckRoleRights(loanRoleRights.EditRecord);
                    objBankMasterDAL.UpdateDateTime = loanGlobalsDAL.GetCurrentDateTime();
                    objBankMasterDAL.BankMasterId = Convert.ToInt32(hdnBankMasterId.Value);
                    loanRecordStatus rsStatus = objBankMasterDAL.UpdateBankMaster();
                    if (rsStatus == loanRecordStatus.Error)
                    {
                        loanAppGlobals.ShowMessage(loanMessagesDAL.UpdateFail, loanMessageIcon.Error);
                        return;
                    }
                    else if (rsStatus == loanRecordStatus.RecordAlreadyExist)
                    {
                        loanAppGlobals.ShowMessage(loanMessagesDAL.AlreadyExist, loanMessageIcon.Warning);
                        hdnModelBank.Value = "show";
                        return;
                    }
                    else if (rsStatus == loanRecordStatus.Success)
                    {
                        loanAppGlobals.ShowMessage(loanMessagesDAL.UpdateSuccess, loanMessageIcon.Success);
                        hdnModelBank.Value = "hide";
                        FillBankMaster();
                    }
                }
            }
            catch (Exception ex)
            {
                loanAppGlobals.SaveError(ex);
            }
        }

        #region List Methods

        protected void lvBankMaster_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            try
            {
                if (e.Item.ItemType == ListViewItemType.DataItem)
                {
                    loanBankMasterDAL objBankMasterDAL = (loanBankMasterDAL)e.Item.DataItem;
                    Literal ltrlBankName = (Literal)e.Item.FindControl("ltrlBankName");
                    ltrlBankName.Text = objBankMasterDAL.BankName;

                    Literal ltrlModifiedBy = (Literal)e.Item.FindControl("ltrlModifiedBy");
                    ltrlModifiedBy.Text = objBankMasterDAL.ModifiedBy;
                    Literal ltrlModifiedDateTime = (Literal)e.Item.FindControl("ltrlModifiedDateTime");
                    ltrlModifiedDateTime.Text = loanGlobalsDAL.ConvertDateTimeToString(objBankMasterDAL.UpdateDateTime, loanAppGlobals.DateTimeFormat);

                    if (objBankMasterDAL.IsVerified != null)
                    {
                        LinkButton lbtnVerify = (LinkButton)e.Item.FindControl("lbtnVerify");
                        lbtnVerify.Visible = false;
                        Literal ltrlVerifiedBy = (Literal)e.Item.FindControl("ltrlVerifiedBy");
                        ltrlVerifiedBy.Text = objBankMasterDAL.VerifiedBy;
                        if (objBankMasterDAL.VerifiedDateTime != null)
                        {
                            Literal ltrlVerifiedDateTime = (Literal)e.Item.FindControl("ltrlVerifiedDateTime");
                            ltrlVerifiedDateTime.Text = loanGlobalsDAL.ConvertDateTimeToString(objBankMasterDAL.VerifiedDateTime.Value, loanAppGlobals.DateTimeFormat);
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

        protected void pgrBankMaster_ItemCommand(object sender, EventArgs e)
        {
            try
            {
                FillBankMaster();
            }
            catch (Exception ex)
            {
                loanAppGlobals.SaveError(ex);
            }
        }

        protected void lvBankMaster_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName.Equals("EditRecord", StringComparison.CurrentCultureIgnoreCase))
                {
                    GetBankMaster(Convert.ToInt32(((ListView)sender).DataKeys[e.Item.DataItemIndex].Value));
                }
                else if (e.CommandName.Equals("VerifyRecord", StringComparison.CurrentCultureIgnoreCase))
                {
                    string pageName = Path.GetFileNameWithoutExtension(Page.AppRelativeVirtualPath) + "master";
                    VerifyRecord(pageName, Convert.ToInt32(((ListView)sender).DataKeys[e.Item.DataItemIndex].Value));
                }
                else if (e.CommandName.Equals("DeleteRecord", StringComparison.CurrentCultureIgnoreCase))
                {
                    loanUser.CheckRoleRights(loanRoleRights.DeleteRecord);
                    loanBankMasterDAL objBankMasterDAL = new loanBankMasterDAL();
                    objBankMasterDAL.BankMasterId = Convert.ToInt32(((ListView)sender).DataKeys[e.Item.DataItemIndex].Value);
                    objBankMasterDAL.SessionId = ((loanUser)Session[loanSessionsDAL.UserSession]).SessionId;

                    loanRecordStatus rsStatus = objBankMasterDAL.DeleteBankMaster();
                    if (rsStatus == loanRecordStatus.Success)
                    {
                        loanAppGlobals.ShowMessage(loanMessagesDAL.DeleteSuccess, loanMessageIcon.Success);
                        FillBankMaster();
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
            if (loanSessionsDAL.GetSessionKeyValue("CurrentPageBank") != null)
            {
                pgrBankMaster.CurrentPage = Convert.ToInt16(loanSessionsDAL.GetSessionKeyValue("CurrentPageBank"));
            }
            if (loanSessionsDAL.GetSessionKeyValue("FilterBank") != null)
            {
                loanBankMasterDAL objBankMasterDAL = (loanBankMasterDAL)loanSessionsDAL.GetSessionKeyValue("FilterBank");
                txtFilterBankName.Text = objBankMasterDAL.BankName;
                if (objBankMasterDAL.IsEnabled)
                {
                    ddlFilterIsEnabled.SelectedValue = "Yes";
                }
                else
                {
                    ddlFilterIsEnabled.SelectedValue = "No";
                }
            }
        }

        private void FillBankMaster()
        {

            loanBankMasterDAL objBankMasterDAL = new loanBankMasterDAL();
            objBankMasterDAL.BankName = txtFilterBankName.Text.Trim();
            if (ddlFilterIsEnabled.SelectedValue == "Yes")
            {
                objBankMasterDAL.IsEnabled = true;
            }
            else if (ddlFilterIsEnabled.SelectedValue == "No")
            {
                objBankMasterDAL.IsEnabled = false;
            }
            if (ddlFilterVerification.SelectedValue == "Yes")
            {
                objBankMasterDAL.IsVerified = true;
            }
            else if (ddlFilterVerification.SelectedValue == "No")
            {
                objBankMasterDAL.IsVerified = false;
            }
            loanSessionsDAL.SetSessionKeyValue("FilterBank", objBankMasterDAL);
            loanSessionsDAL.SetSessionKeyValue("CurrentPageBank", pgrBankMaster.CurrentPage);

            int TotalRecords;
            List<loanBankMasterDAL> lstBankMaster = objBankMasterDAL.SelectAllBankMasterPageWise(pgrBankMaster.StartRowIndex, pgrBankMaster.PageSize, out TotalRecords);
            pgrBankMaster.TotalRowCount = TotalRecords;

            if (lstBankMaster == null)
            {
                loanAppGlobals.ShowMessage(loanMessagesDAL.SelectAllFail, loanMessageIcon.Error);
                return;
            }

            if (lstBankMaster.Count == 0 && pgrBankMaster.TotalRowCount > 0)
            {
                pgrBankMaster_ItemCommand(pgrBankMaster, new EventArgs());
                return;
            }

            lvBankMaster.DataSource = lstBankMaster;
            lvBankMaster.DataBind();

            if (lstBankMaster.Count > 0)
            {
                int EndiIndex = pgrBankMaster.StartRowIndex + pgrBankMaster.PageSize < pgrBankMaster.TotalRowCount ? pgrBankMaster.StartRowIndex + pgrBankMaster.PageSize : pgrBankMaster.TotalRowCount;
                lblRecords.Text = string.Format(Resources.Messages.ResourceManager.GetString("Records"), pgrBankMaster.StartRowIndex + 1, EndiIndex, pgrBankMaster.TotalRowCount);
                lblRecords.Visible = true;
            }
            else
            {
                lblRecords.Visible = false;
            }

            if (pgrBankMaster.TotalRowCount <= pgrBankMaster.PageSize)
            {
                pgrBankMaster.Visible = false;
            }
            else
            {
                pgrBankMaster.Visible = true;
            }

        }


        private void GetBankMaster(int BankMasterId)
        {
            loanUser.CheckRoleRights(loanRoleRights.ViewRecord);
            loanBankMasterDAL objBankMasterDAL = new loanBankMasterDAL();
            objBankMasterDAL.BankMasterId = BankMasterId;
            if (!objBankMasterDAL.SelectBankMaster())
            {
                loanAppGlobals.ShowMessage(loanMessagesDAL.SelectFail, loanMessageIcon.Error);
                return;
            }
            hdnBankMasterId.Value = objBankMasterDAL.BankMasterId.ToString();
            txtBankName.Text = objBankMasterDAL.BankName;
            chkIsEnabled.Checked = objBankMasterDAL.IsEnabled;

            hdnModelBank.Value = "show";
            hdnActionBank.Value = "edit";
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
                FillBankMaster();
            }
        }

        #endregion


    }
}
