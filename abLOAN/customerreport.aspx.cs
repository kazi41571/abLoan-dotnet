using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using loanLibrary;
using System.Threading;
using System.Globalization;

namespace abLOAN
{
    public partial class customerreport : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                loanSessionsDAL.CheckSession();

                if (!Page.IsPostBack)
                {
                    //loanUser.CheckUserRights(loanUserRights.ViewContract);
                    loanUser.CheckRoleRights(loanRoleRights.ViewList);

                    GetContractStatus();
                    GetPageDefaults();
                    GetBank();

                    loanSessionsDAL.RemoveSessionAllKeyValue();

                    FillContractMaster();
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
                pgrContractMaster.CurrentPage = 1;
                FillContractMaster();
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
                txtFilterCustomer.Text = string.Empty;
                //txtFilterContractTitle.Text = string.Empty;
                //txtFilterFileNo.Text = string.Empty;
                //txtFilterContractDate.Text = loanGlobalsDAL.ConvertDateTimeToString(loanGlobalsDAL.GetCurrentDateTime().AddYears(-1), loanAppGlobals.DateFormat);
                //txtFilterContractDateTo.Text = loanGlobalsDAL.ConvertDateTimeToString(loanGlobalsDAL.GetCurrentDateTime(), loanAppGlobals.DateFormat);
                ddlFilterContractStatus.SelectedIndex = 0;
                ddlFilterBank.SelectedIndex = 0;

                pgrContractMaster.CurrentPage = 1;
                FillContractMaster();
            }
            catch (Exception ex)
            {
                loanAppGlobals.SaveError(ex);
            }
        }

        #region List Methods

        protected void lvContractMaster_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            try
            {
                if (e.Item.ItemType == ListViewItemType.DataItem)
                {
                    loanContractMasterDAL objContractMasterDAL = (loanContractMasterDAL)e.Item.DataItem;

                    Literal ltrlCustomer = (Literal)e.Item.FindControl("ltrlCustomer");
                    Literal ltrlCustomerIdNo = (Literal)e.Item.FindControl("ltrlCustomerIdNo");
                    Literal ltrlGuarantorName = (Literal)e.Item.FindControl("ltrlGuarantorName");
                    //Literal ltrlContractTitle = (Literal)e.Item.FindControl("ltrlContractTitle");
                    //Literal ltrlFileNo = (Literal)e.Item.FindControl("ltrlFileNo");
                    Literal ltrlContractStartDate = (Literal)e.Item.FindControl("ltrlContractStartDate");
                    Literal ltrlInstallmentDate = (Literal)e.Item.FindControl("ltrlInstallmentDate");
                    Literal ltrlContractAmount = (Literal)e.Item.FindControl("ltrlContractAmount");
                    Literal ltrlTotalPaidAmount = (Literal)e.Item.FindControl("ltrlTotalPaidAmount");
                    Literal ltrlRemainingAmount = (Literal)e.Item.FindControl("ltrlRemainingAmount");
                    Literal ltrlLastPaidDate = (Literal)e.Item.FindControl("ltrlLastPaidDate");
                    Literal ltrlInstallmentAmount = (Literal)e.Item.FindControl("ltrlInstallmentAmount");
                    Label lblDueAmount = (Label)e.Item.FindControl("lblDueAmount");
                    Literal ltrlDownPayment = (Literal)e.Item.FindControl("ltrlDownPayment");
                    Literal ltrlNotes = (Literal)e.Item.FindControl("ltrlNotes");


                    if (objContractMasterDAL.CustomerIsRedFlag == true)
                    {
                        ltrlCustomer.Text = "<span class='text-danger'>" + objContractMasterDAL.Customer + "</span>";
                    }
                    else
                    {
                        ltrlCustomer.Text = objContractMasterDAL.Customer;
                    }
                    ltrlCustomerIdNo.Text = objContractMasterDAL.CustomerIdNo;
                    ltrlGuarantorName.Text = objContractMasterDAL.GuarantorName;
                    //ltrlContractTitle.Text = objContractMasterDAL.ContractTitle;
                    //ltrlFileNo.Text = objContractMasterDAL.FileNo.ToString();

                    ltrlContractStartDate.Text = loanGlobalsDAL.ConvertDateTimeToString(objContractMasterDAL.ContractStartDate, loanAppGlobals.DateFormat);
                    ltrlInstallmentDate.Text = loanGlobalsDAL.ConvertDateTimeToString(objContractMasterDAL.InstallmentDate, loanAppGlobals.DateFormat);
                    ltrlContractAmount.Text = objContractMasterDAL.ContractAmount.ToString("0.00");
                    ltrlTotalPaidAmount.Text = objContractMasterDAL.TotalPaid.ToString("0.00");
                    ltrlRemainingAmount.Text = objContractMasterDAL.PendingAmount.ToString("0.00"); //(objContractMasterDAL.ContractAmount - objContractMasterDAL.TotalPaid).ToString("0.00");
                    ltrlLastPaidDate.Text = loanGlobalsDAL.ConvertDateTimeToString(objContractMasterDAL.LastPaidDate, loanAppGlobals.DateFormat);
                    ltrlInstallmentAmount.Text = objContractMasterDAL.InstallmentAmount.ToString("0.00");
                    lblDueAmount.Text = objContractMasterDAL.DueAmount.ToString("0.00");
                    if (objContractMasterDAL.DueAmount > 0)
                    {
                        lblDueAmount.CssClass = "text-danger";
                    }
                    else if (objContractMasterDAL.DueAmount < 0)
                    {
                        lblDueAmount.CssClass = "text-success";
                    }
                    ltrlNotes.Text = objContractMasterDAL.Notes;
                }
            }
            catch (Exception ex)
            {
                loanAppGlobals.SaveError(ex);
            }
        }

        protected void pgrContractMaster_ItemCommand(object sender, EventArgs e)
        {
            try
            {
                FillContractMaster();
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
            //txtFilterContractDate.Text = loanGlobalsDAL.ConvertDateTimeToString(loanGlobalsDAL.GetCurrentDateTime().AddYears(-1), loanAppGlobals.DateFormat);
            //txtFilterContractDateTo.Text = loanGlobalsDAL.ConvertDateTimeToString(loanGlobalsDAL.GetCurrentDateTime(), loanAppGlobals.DateFormat);

            if (loanSessionsDAL.GetSessionKeyValue("CurrentPageContract") != null)
            {
                pgrContractMaster.CurrentPage = Convert.ToInt16(loanSessionsDAL.GetSessionKeyValue("CurrentPageContract"));
            }
            if (loanSessionsDAL.GetSessionKeyValue("FilterContract") != null)
            {
                loanContractMasterDAL objContractMasterDAL = (loanContractMasterDAL)loanSessionsDAL.GetSessionKeyValue("FilterContract");
                txtFilterCustomer.Text = objContractMasterDAL.Customer;
                //txtFilterContractTitle.Text = objContractMasterDAL.ContractTitle;
                //if (objContractMasterDAL.FileNo > 0)
                //{
                //    txtFilterFileNo.Text = objContractMasterDAL.FileNo.ToString();
                //}
                //if (objContractMasterDAL.ContractStartDate != new DateTime())
                //{
                //    txtFilterContractDate.Text = loanGlobalsDAL.ConvertDateTimeToString(objContractMasterDAL.ContractStartDate, loanAppGlobals.DateFormat);
                //}
                //if (objContractMasterDAL.ContractDate != new DateTime())
                //{
                //    txtFilterContractDateTo.Text = loanGlobalsDAL.ConvertDateTimeToString(objContractMasterDAL.ContractDate, loanAppGlobals.DateFormat);
                //}
                ddlFilterContractStatus.SelectedValue = objContractMasterDAL.linktoContractStatusMasterId.ToString();
                ddlFilterBank.SelectedValue = objContractMasterDAL.linktoBankMasterId.ToString();
            }
        }

        private void FillContractMaster()
        {

            loanContractMasterDAL objContractMasterDAL = new loanContractMasterDAL();
            string IdNo = txtFilterCustomer.Text.Trim();
            if (IdNo.Contains("("))
            {
                IdNo = IdNo.Substring(IdNo.IndexOf("(") + 1, IdNo.IndexOf(")") - IdNo.IndexOf("(") - 1);
            }
            objContractMasterDAL.Customer = IdNo;
            //objContractMasterDAL.ContractTitle = txtFilterContractTitle.Text.Trim();
            //if (!string.IsNullOrEmpty(txtFilterFileNo.Text))
            //{
            //    objContractMasterDAL.FileNo = Convert.ToInt32(txtFilterFileNo.Text);
            //}
            DateTime? ContractDateFrom = null;
            //if (!string.IsNullOrEmpty(txtFilterContractDate.Text))
            //{
            //    ContractDateFrom = DateTime.ParseExact(txtFilterContractDate.Text, loanAppGlobals.DateFormat, System.Globalization.DateTimeFormatInfo.InvariantInfo);
            //    objContractMasterDAL.ContractStartDate = DateTime.ParseExact(txtFilterContractDate.Text, loanAppGlobals.DateFormat, System.Globalization.DateTimeFormatInfo.InvariantInfo);
            //}
            DateTime? ContractDateTo = null;
            //if (!string.IsNullOrEmpty(txtFilterContractDateTo.Text))
            //{
            //    ContractDateTo = DateTime.ParseExact(txtFilterContractDateTo.Text, loanAppGlobals.DateFormat, System.Globalization.DateTimeFormatInfo.InvariantInfo);
            //    objContractMasterDAL.ContractDate = DateTime.ParseExact(txtFilterContractDateTo.Text, loanAppGlobals.DateFormat, System.Globalization.DateTimeFormatInfo.InvariantInfo);
            //}
            if (ddlFilterContractStatus.SelectedValue != string.Empty)
            {
                objContractMasterDAL.linktoContractStatusMasterId = Convert.ToInt32(ddlFilterContractStatus.SelectedValue);
            }
            objContractMasterDAL.linktoCompanyMasterId = ((loanUser)Session[loanSessionsDAL.UserSession]).CompanyMasterId;
            if (ddlFilterBank.SelectedValue != string.Empty)
            {
                objContractMasterDAL.linktoBankMasterId = Convert.ToInt32(ddlFilterBank.SelectedValue);
            }

            loanSessionsDAL.SetSessionKeyValue("FilterContract", objContractMasterDAL);
            loanSessionsDAL.SetSessionKeyValue("CurrentPageContract", pgrContractMaster.CurrentPage);

            int TotalRecords;
            List<loanContractMasterDAL> lstContractMaster = objContractMasterDAL.SelectAllContractMasterPageWise(pgrContractMaster.StartRowIndex, pgrContractMaster.PageSize, out TotalRecords, ContractDateFrom, ContractDateTo);
            pgrContractMaster.TotalRowCount = TotalRecords;

            if (lstContractMaster == null)
            {
                loanAppGlobals.ShowMessage(loanMessagesDAL.SelectAllFail, loanMessageIcon.Error);
                return;
            }

            if (lstContractMaster.Count == 0 && pgrContractMaster.TotalRowCount > 0)
            {
                pgrContractMaster_ItemCommand(pgrContractMaster, new EventArgs());
                return;
            }

            lvContractMaster.DataSource = lstContractMaster;
            lvContractMaster.DataBind();

            if (lstContractMaster.Count > 0)
            {
                int EndiIndex = pgrContractMaster.StartRowIndex + pgrContractMaster.PageSize < pgrContractMaster.TotalRowCount ? pgrContractMaster.StartRowIndex + pgrContractMaster.PageSize : pgrContractMaster.TotalRowCount;
                lblRecords.Text = string.Format(Resources.Messages.ResourceManager.GetString("Records"), pgrContractMaster.StartRowIndex + 1, EndiIndex, pgrContractMaster.TotalRowCount);
                lblRecords.Visible = true;
            }
            else
            {
                lblRecords.Visible = false;
            }

            if (pgrContractMaster.TotalRowCount <= pgrContractMaster.PageSize)
            {
                pgrContractMaster.Visible = false;
            }
            else
            {
                pgrContractMaster.Visible = true;
            }

        }

        private void GetContractStatus()
        {
            ddlFilterContractStatus.Items.Clear();
            ddlFilterContractStatus.Items.Add(new System.Web.UI.WebControls.ListItem(loanDropDownItem.Select, ""));

            List<loanContractStatusMasterDAL> lstContractStatusMasterDAL = loanContractStatusMasterDAL.SelectAllContractStatusMasterContractStatus();
            if (lstContractStatusMasterDAL == null)
            {
                loanAppGlobals.ShowMessage(loanMessagesDAL.SelectAllFail, loanMessageIcon.Error);
                return;
            }

            foreach (loanContractStatusMasterDAL obj in lstContractStatusMasterDAL)
            {
                ddlFilterContractStatus.Items.Add(new System.Web.UI.WebControls.ListItem(obj.ContractStatus, obj.ContractStatusMasterId.ToString()));
            }
        }
        private void GetBank()
        {
            ddlFilterBank.Items.Clear();
            ddlFilterBank.Items.Add(new System.Web.UI.WebControls.ListItem(loanDropDownItem.Select, ""));

            List<loanBankMasterDAL> lstBankMasterDAL = loanBankMasterDAL.SelectAllBankMasterBankName();
            if (lstBankMasterDAL == null)
            {
                loanAppGlobals.ShowMessage(loanMessagesDAL.SelectAllFail, loanMessageIcon.Error);
                return;
            }
            foreach (loanBankMasterDAL obj in lstBankMasterDAL)
            {
                ddlFilterBank.Items.Add(new System.Web.UI.WebControls.ListItem(obj.BankName, obj.BankMasterId.ToString()));
            }
        }
        #endregion


    }
}
