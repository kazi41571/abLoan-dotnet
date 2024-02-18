using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using loanLibrary;

namespace abLOAN
{
    public partial class calling : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                loanSessionsDAL.CheckSession();

                if (!Page.IsPostBack)
                {
                    //loanUser.CheckUserRights(loanUserRights.ViewCalling);

                    GetCustomer();
                    GetBank();

                    GetPageDefaults();

                    //loanSessionsDAL.RemoveSessionAllKeyValue();

                    FillCallingMaster();
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
                pgrCallingMaster.CurrentPage = 1;
                FillCallingMaster();
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
                txtFilterCallingDate.Text = loanGlobalsDAL.ConvertDateTimeToString(loanGlobalsDAL.GetCurrentDateTime(), loanAppGlobals.DateFormat);
                txtFilterCallingName.Text = string.Empty;
                ddlFilterCustomer.SelectedIndex = 0;

                pgrCallingMaster.CurrentPage = 1;
                FillCallingMaster();
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
                loanCallingMasterDAL objCallingMasterDAL = new loanCallingMasterDAL();
                objCallingMasterDAL.linktoCompanyMasterId = ((loanUser)Session[loanSessionsDAL.UserSession]).CompanyMasterId;
                objCallingMasterDAL.CallingDate = DateTime.ParseExact(txtCallingDate.Text, loanAppGlobals.DateFormat, System.Globalization.DateTimeFormatInfo.InvariantInfo);
                objCallingMasterDAL.CallingName = txtCallingName.Text.Trim();
                objCallingMasterDAL.linktoCustomerMasterId = Convert.ToInt32(ddlCustomer.SelectedValue);
                if (!string.IsNullOrEmpty(ddlBank.Text))
                {
                    objCallingMasterDAL.linktoBankMasterId = Convert.ToInt32(ddlBank.SelectedValue);
                }
                if (!string.IsNullOrEmpty(txtAmount.Text))
                {
                    objCallingMasterDAL.Amount = Convert.ToDecimal(txtAmount.Text);
                }
                objCallingMasterDAL.Notes = txtNotes.Text.Trim();
                objCallingMasterDAL.UpdateDateTime = loanGlobalsDAL.GetCurrentDateTime();
                objCallingMasterDAL.SessionId = ((loanUser)Session[loanSessionsDAL.UserSession]).SessionId;

                if (string.IsNullOrEmpty(hdnActionCalling.Value))
                {
                    loanRecordStatus rsStatus = objCallingMasterDAL.InsertCallingMaster();
                    if (rsStatus == loanRecordStatus.Error)
                    {
                        loanAppGlobals.ShowMessage(loanMessagesDAL.InsertFail, loanMessageIcon.Error);
                        return;
                    }
                    else if (rsStatus == loanRecordStatus.RecordAlreadyExist)
                    {
                        loanAppGlobals.ShowMessage(loanMessagesDAL.AlreadyExist, loanMessageIcon.Warning);
                        hdnModelCalling.Value = "show";
                        return;
                    }
                    else if (rsStatus == loanRecordStatus.Success)
                    {
                        loanAppGlobals.ShowMessage(loanMessagesDAL.InsertSuccess, loanMessageIcon.Success);
                        if (((Button)sender).ID.Equals("btnSaveAndNew"))
                        {
                            hdnModelCalling.Value = "clear";
                        }
                        else
                        {
                            hdnModelCalling.Value = "hide";
                        }
                        FillCallingMaster();
                    }
                }
                else
                {
                    objCallingMasterDAL.UpdateDateTime = loanGlobalsDAL.GetCurrentDateTime();
                    objCallingMasterDAL.CallingMasterId = Convert.ToInt32(hdnCallingMasterId.Value);
                    loanRecordStatus rsStatus = objCallingMasterDAL.UpdateCallingMaster();
                    if (rsStatus == loanRecordStatus.Error)
                    {
                        loanAppGlobals.ShowMessage(loanMessagesDAL.UpdateFail, loanMessageIcon.Error);
                        return;
                    }
                    else if (rsStatus == loanRecordStatus.RecordAlreadyExist)
                    {
                        loanAppGlobals.ShowMessage(loanMessagesDAL.AlreadyExist, loanMessageIcon.Warning);
                        hdnModelCalling.Value = "show";
                        return;
                    }
                    else if (rsStatus == loanRecordStatus.Success)
                    {
                        loanAppGlobals.ShowMessage(loanMessagesDAL.UpdateSuccess, loanMessageIcon.Success);
                        hdnModelCalling.Value = "hide";
                        FillCallingMaster();
                    }
                }
            }
            catch (Exception ex)
            {
                loanAppGlobals.SaveError(ex);
            }
        }

        #region List Methods

        protected void lvCallingMaster_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            try
            {
                if (e.Item.ItemType == ListViewItemType.DataItem)
                {
                    loanCallingMasterDAL objCallingMasterDAL = (loanCallingMasterDAL)e.Item.DataItem;

                    Literal ltrlCallingDate = (Literal)e.Item.FindControl("ltrlCallingDate");
                    Literal ltrlCallingName = (Literal)e.Item.FindControl("ltrlCallingName");
                    Literal ltrlCustomer = (Literal)e.Item.FindControl("ltrlCustomer");
                    Literal ltrlBank = (Literal)e.Item.FindControl("ltrlBank");
                    Literal ltrlAmount = (Literal)e.Item.FindControl("ltrlAmount");
                    Literal ltrlNotes = (Literal)e.Item.FindControl("ltrlNotes");

                    ltrlCallingDate.Text = loanGlobalsDAL.ConvertDateTimeToString(objCallingMasterDAL.CallingDate, loanAppGlobals.DateFormat);
                    ltrlCallingName.Text = objCallingMasterDAL.CallingName;
                    ltrlCustomer.Text = objCallingMasterDAL.Customer;
                    ltrlBank.Text = objCallingMasterDAL.Bank;
                    if (objCallingMasterDAL.Amount != null)
                    {
                        ltrlAmount.Text = objCallingMasterDAL.Amount.Value.ToString("0.00");
                    }
                    ltrlNotes.Text = objCallingMasterDAL.Notes;
                }
            }
            catch (Exception ex)
            {
                loanAppGlobals.SaveError(ex);
            }
        }

        protected void pgrCallingMaster_ItemCommand(object sender, EventArgs e)
        {
            try
            {
                FillCallingMaster();
            }
            catch (Exception ex)
            {
                loanAppGlobals.SaveError(ex);
            }
        }

        protected void lvCallingMaster_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName.Equals("EditRecord", StringComparison.CurrentCultureIgnoreCase))
                {
                    GetCallingMaster(Convert.ToInt32(((ListView)sender).DataKeys[e.Item.DataItemIndex].Value));
                }
                else if (e.CommandName.Equals("DeleteRecord", StringComparison.CurrentCultureIgnoreCase))
                {
                    loanCallingMasterDAL objCallingMasterDAL = new loanCallingMasterDAL();
                    objCallingMasterDAL.CallingMasterId = Convert.ToInt32(((ListView)sender).DataKeys[e.Item.DataItemIndex].Value);
                    loanRecordStatus rsStatus = objCallingMasterDAL.DeleteCallingMaster();
                    if (rsStatus == loanRecordStatus.Success)
                    {
                        loanAppGlobals.ShowMessage(loanMessagesDAL.DeleteSuccess, loanMessageIcon.Success);
                        FillCallingMaster();
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
            txtFilterCallingDate.Text = loanGlobalsDAL.ConvertDateTimeToString(loanGlobalsDAL.GetCurrentDateTime(), loanAppGlobals.DateFormat);

            if (loanSessionsDAL.GetSessionKeyValue("CurrentPageCalling") != null)
            {
                pgrCallingMaster.CurrentPage = Convert.ToInt16(loanSessionsDAL.GetSessionKeyValue("CurrentPageCalling"));
            }
            if (loanSessionsDAL.GetSessionKeyValue("FilterCalling") != null)
            {
                loanCallingMasterDAL objCallingMasterDAL = (loanCallingMasterDAL)loanSessionsDAL.GetSessionKeyValue("FilterCalling");
                if (objCallingMasterDAL.CallingDate != new DateTime())
                {
                    txtFilterCallingDate.Text = loanGlobalsDAL.ConvertDateTimeToString(objCallingMasterDAL.CallingDate, loanAppGlobals.DateFormat);
                }
                txtFilterCallingName.Text = objCallingMasterDAL.CallingName;
                ddlFilterCustomer.SelectedValue = objCallingMasterDAL.linktoCustomerMasterId.ToString();
            }
        }

        private void FillCallingMaster()
        {

            loanCallingMasterDAL objCallingMasterDAL = new loanCallingMasterDAL();
            if (!string.IsNullOrEmpty(txtFilterCallingDate.Text))
            {
                objCallingMasterDAL.CallingDate = DateTime.ParseExact(txtFilterCallingDate.Text, loanAppGlobals.DateFormat, System.Globalization.DateTimeFormatInfo.InvariantInfo);
            }
            objCallingMasterDAL.CallingName = txtFilterCallingName.Text.Trim();
            if (ddlFilterCustomer.SelectedValue != string.Empty)
            {
                objCallingMasterDAL.linktoCustomerMasterId = Convert.ToInt32(ddlFilterCustomer.SelectedValue);
            }
            objCallingMasterDAL.linktoCompanyMasterId = ((loanUser)Session[loanSessionsDAL.UserSession]).CompanyMasterId;

            loanSessionsDAL.SetSessionKeyValue("FilterCalling", objCallingMasterDAL);
            loanSessionsDAL.SetSessionKeyValue("CurrentPageCalling", pgrCallingMaster.CurrentPage);

            int TotalRecords;
            List<loanCallingMasterDAL> lstCallingMaster = objCallingMasterDAL.SelectAllCallingMasterPageWise(pgrCallingMaster.StartRowIndex, pgrCallingMaster.PageSize, out TotalRecords);
            pgrCallingMaster.TotalRowCount = TotalRecords;

            if (lstCallingMaster == null)
            {
                loanAppGlobals.ShowMessage(loanMessagesDAL.SelectAllFail, loanMessageIcon.Error);
                return;
            }

            if (lstCallingMaster.Count == 0 && pgrCallingMaster.TotalRowCount > 0)
            {
                pgrCallingMaster_ItemCommand(pgrCallingMaster, new EventArgs());
                return;
            }

            lvCallingMaster.DataSource = lstCallingMaster;
            lvCallingMaster.DataBind();

            if (lstCallingMaster.Count > 0)
            {
                int EndiIndex = pgrCallingMaster.StartRowIndex + pgrCallingMaster.PageSize < pgrCallingMaster.TotalRowCount ? pgrCallingMaster.StartRowIndex + pgrCallingMaster.PageSize : pgrCallingMaster.TotalRowCount;
                lblRecords.Text = string.Format(Resources.Messages.ResourceManager.GetString("Records"), pgrCallingMaster.StartRowIndex + 1, EndiIndex, pgrCallingMaster.TotalRowCount);
                lblRecords.Visible = true;
            }
            else
            {
                lblRecords.Visible = false;
            }

            if (pgrCallingMaster.TotalRowCount <= pgrCallingMaster.PageSize)
            {
                pgrCallingMaster.Visible = false;
            }
            else
            {
                pgrCallingMaster.Visible = true;
            }

        }

        private void GetCustomer()
        {
            ddlCustomer.Items.Clear();
            ddlCustomer.Items.Add(new System.Web.UI.WebControls.ListItem(loanDropDownItem.Select, ""));

            ddlFilterCustomer.Items.Clear();
            ddlFilterCustomer.Items.Add(new System.Web.UI.WebControls.ListItem(loanDropDownItem.Select, ""));

            List<loanCustomerMasterDAL> lstCustomerMasterDAL = loanCustomerMasterDAL.SelectAllCustomerMasterCustomerName();
            if (lstCustomerMasterDAL == null)
            {
                loanAppGlobals.ShowMessage(loanMessagesDAL.SelectAllFail, loanMessageIcon.Error);
                return;
            }
            foreach (loanCustomerMasterDAL obj in lstCustomerMasterDAL)
            {
                ddlCustomer.Items.Add(new System.Web.UI.WebControls.ListItem(obj.CustomerName, obj.CustomerMasterId.ToString()));
                ddlFilterCustomer.Items.Add(new System.Web.UI.WebControls.ListItem(obj.CustomerName, obj.CustomerMasterId.ToString()));
            }
        }

        private void GetBank()
        {
            ddlBank.Items.Clear();
            ddlBank.Items.Add(new System.Web.UI.WebControls.ListItem(loanDropDownItem.Select, ""));

            List<loanBankMasterDAL> lstBankMasterDAL = loanBankMasterDAL.SelectAllBankMasterBankName();
            if (lstBankMasterDAL == null)
            {
                loanAppGlobals.ShowMessage(loanMessagesDAL.SelectAllFail, loanMessageIcon.Error);
                return;
            }
            foreach (loanBankMasterDAL obj in lstBankMasterDAL)
            {
                ddlBank.Items.Add(new System.Web.UI.WebControls.ListItem(obj.BankName, obj.BankMasterId.ToString()));
            }
        }


        private void GetCallingMaster(int CallingMasterId)
        {
            loanCallingMasterDAL objCallingMasterDAL = new loanCallingMasterDAL();
            objCallingMasterDAL.CallingMasterId = CallingMasterId;
            if (!objCallingMasterDAL.SelectCallingMaster())
            {
                loanAppGlobals.ShowMessage(loanMessagesDAL.SelectFail, loanMessageIcon.Error);
                return;
            }
            hdnCallingMasterId.Value = objCallingMasterDAL.CallingMasterId.ToString();
            txtCallingDate.Text = loanGlobalsDAL.ConvertDateTimeToString(objCallingMasterDAL.CallingDate, loanAppGlobals.DateFormat);
            txtCallingName.Text = objCallingMasterDAL.CallingName;
            ddlCustomer.SelectedIndex = ddlCustomer.Items.IndexOf(ddlCustomer.Items.FindByValue(objCallingMasterDAL.linktoCustomerMasterId.ToString()));
            ddlBank.SelectedIndex = ddlBank.Items.IndexOf(ddlBank.Items.FindByValue(objCallingMasterDAL.linktoBankMasterId.ToString()));
            if (objCallingMasterDAL.Amount != null)
            {
                txtAmount.Text = objCallingMasterDAL.Amount.Value.ToString("0.00");
            }
            txtNotes.Text = objCallingMasterDAL.Notes;

            hdnModelCalling.Value = "show";
            hdnActionCalling.Value = "edit";
        }
        #endregion


    }
}
