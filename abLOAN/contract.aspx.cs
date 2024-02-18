using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using loanLibrary;
using System.Threading;
using System.Globalization;
using System.IO;

namespace abLOAN
{
    public partial class contract : BasePage
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

                    ddlSortBy.Items.Add(new System.Web.UI.WebControls.ListItem(Resources.Resource.Customer + " " + Resources.Resource.ASC, "Customer,ASC"));
                    ddlSortBy.Items.Add(new System.Web.UI.WebControls.ListItem(Resources.Resource.Customer + " " + Resources.Resource.DESC, "Customer,DESC"));
                    ddlSortBy.Items.Add(new System.Web.UI.WebControls.ListItem(Resources.Resource.ContractStartDate + " " + Resources.Resource.ASC, "ContractStartDate,ASC"));
                    ddlSortBy.Items.Add(new System.Web.UI.WebControls.ListItem(Resources.Resource.ContractStartDate + " " + Resources.Resource.DESC, "ContractStartDate,DESC"));
                    ddlSortBy.Items.Add(new System.Web.UI.WebControls.ListItem(Resources.Resource.CreateDate + " " + Resources.Resource.ASC, "CreateDate,ASC"));
                    ddlSortBy.Items.Add(new System.Web.UI.WebControls.ListItem(Resources.Resource.CreateDate + " " + Resources.Resource.DESC, "CreateDate,DESC"));

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
                txtFilterContractTitle.Text = string.Empty;
                txtFilterFileNo.Text = string.Empty;
                txtFilterContractDate.Text = loanGlobalsDAL.ConvertDateTimeToString(new DateTime(2000, 01, 01), loanAppGlobals.DateFormat);
                txtFilterContractDateTo.Text = loanGlobalsDAL.ConvertDateTimeToString(loanGlobalsDAL.GetCurrentDateTime().AddYears(1), loanAppGlobals.DateFormat);
                ddlFilterContractStatus.SelectedValue = loanContractStatus.Active.GetHashCode().ToString();
                ddlFilterBank.SelectedValue = string.Empty;
                ddlFilterHasurl.SelectedValue = string.Empty;
                ddlFilterVerification.SelectedValue = string.Empty;
                txtFilterDueInstallments.Text = string.Empty;
                txtFilterBankAccountNumber.Text = string.Empty;
                ddlSortBy.SelectedValue = string.Empty;

                pgrContractMaster.CurrentPage = 1;
                FillContractMaster();
            }
            catch (Exception ex)
            {
                loanAppGlobals.SaveError(ex);
            }
        }

        protected void btnDownload_Click(object sender, EventArgs e)
        {
            loanContractMasterDAL objContractMasterDAL = new loanContractMasterDAL();
            string IdNo = txtFilterCustomer.Text.Trim();
            if (IdNo.Contains("("))
            {
                IdNo = IdNo.Substring(IdNo.IndexOf("(") + 1, IdNo.IndexOf(")") - IdNo.IndexOf("(") - 1);
            }
            objContractMasterDAL.Customer = IdNo;
            objContractMasterDAL.ContractTitle = txtFilterContractTitle.Text.Trim();
            if (!string.IsNullOrEmpty(txtFilterFileNo.Text))
            {
                objContractMasterDAL.FileNo = Convert.ToInt32(txtFilterFileNo.Text);
            }
            DateTime? ContractDateFrom = null;
            if (!string.IsNullOrEmpty(txtFilterContractDate.Text))
            {
                ContractDateFrom = DateTime.ParseExact(txtFilterContractDate.Text, loanAppGlobals.DateFormat, System.Globalization.DateTimeFormatInfo.InvariantInfo);
                objContractMasterDAL.ContractStartDate = DateTime.ParseExact(txtFilterContractDate.Text, loanAppGlobals.DateFormat, System.Globalization.DateTimeFormatInfo.InvariantInfo);
            }
            DateTime? ContractDateTo = null;
            if (!string.IsNullOrEmpty(txtFilterContractDateTo.Text))
            {
                ContractDateTo = DateTime.ParseExact(txtFilterContractDateTo.Text, loanAppGlobals.DateFormat, System.Globalization.DateTimeFormatInfo.InvariantInfo);
                objContractMasterDAL.ContractDate = DateTime.ParseExact(txtFilterContractDateTo.Text, loanAppGlobals.DateFormat, System.Globalization.DateTimeFormatInfo.InvariantInfo);
            }
            if (ddlFilterContractStatus.SelectedValue != string.Empty)
            {
                objContractMasterDAL.linktoContractStatusMasterId = Convert.ToInt32(ddlFilterContractStatus.SelectedValue);
            }
            objContractMasterDAL.linktoCompanyMasterId = ((loanUser)Session[loanSessionsDAL.UserSession]).CompanyMasterId;
            if (ddlFilterBank.SelectedValue != string.Empty)
            {
                objContractMasterDAL.linktoBankMasterId = Convert.ToInt32(ddlFilterBank.SelectedValue);
            }
            if (ddlFilterVerification.SelectedValue == "Yes")
            {
                objContractMasterDAL.IsVerified = true;
            }
            else if (ddlFilterVerification.SelectedValue == "No")
            {
                objContractMasterDAL.IsVerified = false;
            }
            if (ddlFilterHasurl.SelectedValue == "Yes")
            {
                objContractMasterDAL.HasURL = true;
            }
            else if (ddlFilterHasurl.SelectedValue == "No")
            {
                objContractMasterDAL.HasURL = false;
            }
            if (!string.IsNullOrEmpty(txtFilterDueInstallments.Text))
            {
                objContractMasterDAL.DueInstallments = Convert.ToInt32(txtFilterDueInstallments.Text);
            }
            string OrderBy = null;
            string OrderDir = null;
            if (ddlSortBy.SelectedValue != string.Empty)
            {
                OrderBy = ddlSortBy.SelectedValue.Split(',')[0];
                OrderDir = ddlSortBy.SelectedValue.Split(',')[1];
            }

            int TotalRecords;
            List<loanContractMasterDAL> lstContractMaster = objContractMasterDAL.SelectAllContractMasterPageWise(0, int.MaxValue, out TotalRecords, ContractDateFrom, ContractDateTo, orderBy: OrderBy, orderDir: OrderDir);
            pgrContractMaster.TotalRowCount = TotalRecords;

            if (lstContractMaster == null)
            {
                loanAppGlobals.ShowMessage(loanMessagesDAL.SelectAllFail, loanMessageIcon.Error);
                return;
            }

            string file = System.Configuration.ConfigurationManager.AppSettings["FileSavePath"] + "Contract.csv";

            string[] headers = { "Customer", "Id No", "Customer Address", "File No", "Guarantor Name", "Contract", "Bank", "Contract Date", "Contract Start Date", "Contract Amount", "Total Paid", "Remaining", "Down Payment", "No Of Installments", "Installment Amount", "Installment Date", "Last Paid Amount", "Last Paid Date", "Due Amount", "Settlement Amount", "Settlement Reason", "Notes", "Customer Links", "Contract Links", "Contract Status", "Verifier", "Modifier", "Creator" };
            string[] columns = { "Customer", "CustomerIdNo", "CustomerAddress", "FileNo", "GuarantorName", "ContractTitle", "Bank", "ContractDate", "ContractStartDate", "ContractAmount", "TotalPaid", "PendingAmount", "DownPayment", "NoOfInstallments", "InstallmentAmount", "InstallmentDate", "LastPaidAmount", "LastPaidDate", "DueAmount", "SettlementAmount", "SettlementReason", "Notes", "CustomerLinks", "Links", "ContractStatus", "VerifiedBy", "ModifiedBy", "CreatedBy" };

            bool IsSuccess = loanAppGlobals.ExportCsv(lstContractMaster, headers, columns, file);
            if (!IsSuccess)
            {
                loanAppGlobals.ShowMessage(loanMessagesDAL.Exception, loanMessageIcon.Error);
                return;
            }

            loanAppGlobals.SendOutFile(file, Path.GetFileName(file));
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
                    HyperLink hlnkCustomer = (HyperLink)e.Item.FindControl("hlnkCustomer");
                    Literal ltrlCustomerIdNo = (Literal)e.Item.FindControl("ltrlCustomerIdNo");
                    Literal ltrlGuarantorName = (Literal)e.Item.FindControl("ltrlGuarantorName");
                    Literal ltrlBankAccountNumber = (Literal)e.Item.FindControl("ltrlBankAccountNumber");
                    Literal ltrlBankAccountNumber2 = (Literal)e.Item.FindControl("ltrlBankAccountNumber2");
                    Literal ltrlBankAccountNumber3 = (Literal)e.Item.FindControl("ltrlBankAccountNumber3");
                    Literal ltrlBankAccountNumber4 = (Literal)e.Item.FindControl("ltrlBankAccountNumber4");
                    Literal ltrlContractTitle = (Literal)e.Item.FindControl("ltrlContractTitle");
                    HyperLink hlnkContractTitle = (HyperLink)e.Item.FindControl("hlnkContractTitle");
                    Literal ltrlBank = (Literal)e.Item.FindControl("ltrlBank");
                    Literal ltrlFileNo = (Literal)e.Item.FindControl("ltrlFileNo");
                    Literal ltrlContractDate = (Literal)e.Item.FindControl("ltrlContractDate");
                    Literal ltrlContractStartDate = (Literal)e.Item.FindControl("ltrlContractStartDate");
                    Literal ltrlContractEndDate = (Literal)e.Item.FindControl("ltrlContractEndDate");
                    Literal ltrlInstallmentDate = (Literal)e.Item.FindControl("ltrlInstallmentDate");
                    Literal ltrlContractAmount = (Literal)e.Item.FindControl("ltrlContractAmount");
                    Literal ltrlTotalPaidAmount = (Literal)e.Item.FindControl("ltrlTotalPaidAmount");
                    Literal ltrlRemainingAmount = (Literal)e.Item.FindControl("ltrlRemainingAmount");
                    Literal ltrlNoOfInstallments = (Literal)e.Item.FindControl("ltrlNoOfInstallments");
                    Literal ltrlInstallmentAmount = (Literal)e.Item.FindControl("ltrlInstallmentAmount");
                    Literal ltrlLastPaidDate = (Literal)e.Item.FindControl("ltrlLastPaidDate");
                    Literal ltrlLastPaidAmount = (Literal)e.Item.FindControl("ltrlLastPaidAmount");
                    Label lblDueAmount = (Label)e.Item.FindControl("lblDueAmount");
                    Literal ltrlDownPayment = (Literal)e.Item.FindControl("ltrlDownPayment");
                    Label lblSettlementAmount = (Label)e.Item.FindControl("lblSettlementAmount");
                    Literal ltrlSettlementReason = (Literal)e.Item.FindControl("ltrlSettlementReason");
                    Literal ltrlNotes = (Literal)e.Item.FindControl("ltrlNotes");
                    HyperLink lnkLinks = (HyperLink)e.Item.FindControl("lnkLinks");
                    HyperLink lnkCustomerLinks = (HyperLink)e.Item.FindControl("lnkCustomerLinks");
                    Label lblContractStatus = (Label)e.Item.FindControl("lblContractStatus");

                    Literal ltrlModifiedBy = (Literal)e.Item.FindControl("ltrlModifiedBy");
                    ltrlModifiedBy.Text = objContractMasterDAL.ModifiedBy;
                    Literal ltrlModifiedDateTime = (Literal)e.Item.FindControl("ltrlModifiedDateTime");
                    ltrlModifiedDateTime.Text = loanGlobalsDAL.ConvertDateTimeToString(objContractMasterDAL.UpdateDateTime, loanAppGlobals.DateTimeFormat);

                    Literal ltrlCreatedBy = (Literal)e.Item.FindControl("ltrlCreatedBy");
                    ltrlCreatedBy.Text = objContractMasterDAL.CreatedBy;
                    Literal ltrlCreatedDateTime = (Literal)e.Item.FindControl("ltrlCreatedDateTime");
                    ltrlCreatedDateTime.Text = loanGlobalsDAL.ConvertDateTimeToString(objContractMasterDAL.CreateDateTime, loanAppGlobals.DateTimeFormat);

                    if (objContractMasterDAL.CustomerIsRedFlag == true)
                    {
                        ltrlCustomer.Text = "<span class='text-danger'>" + objContractMasterDAL.Customer + "</span>";
                        hlnkCustomer.Text = "<span class='text-danger'>" + objContractMasterDAL.Customer + "</span>";
                    }
                    else
                    {
                        ltrlCustomer.Text = objContractMasterDAL.Customer;
                        hlnkCustomer.Text = objContractMasterDAL.Customer;
                    }
                    if (objContractMasterDAL.CustomerLinks != "")
                    {
                        hlnkCustomer.NavigateUrl = objContractMasterDAL.CustomerLinks;
                        hlnkCustomer.ToolTip = objContractMasterDAL.CustomerLinks;
                        hlnkCustomer.Visible = true;
                        ltrlCustomer.Visible = false;
                    }

                    ltrlCustomerIdNo.Text = objContractMasterDAL.CustomerIdNo;
                    ltrlGuarantorName.Text = objContractMasterDAL.GuarantorName;
                    ltrlBankAccountNumber.Text = objContractMasterDAL.BankAccountNumber;
                    ltrlBankAccountNumber2.Text = objContractMasterDAL.BankAccountNumber2;
                    ltrlBankAccountNumber3.Text = objContractMasterDAL.BankAccountNumber3;
                    ltrlBankAccountNumber4.Text = objContractMasterDAL.BankAccountNumber4;
                    ltrlContractTitle.Text = objContractMasterDAL.ContractTitle;
                    hlnkContractTitle.Text = objContractMasterDAL.ContractTitle;
                    if (objContractMasterDAL.Links != "")
                    {
                        hlnkContractTitle.NavigateUrl = objContractMasterDAL.Links;
                        hlnkContractTitle.ToolTip = objContractMasterDAL.Links;
                        hlnkContractTitle.Visible = true;
                        ltrlContractTitle.Visible = false;
                    }
                    ltrlBank.Text = objContractMasterDAL.Bank;
                    ltrlFileNo.Text = objContractMasterDAL.FileNo.ToString();
                    ltrlContractDate.Text = loanGlobalsDAL.ConvertDateTimeToString(objContractMasterDAL.ContractDate, loanAppGlobals.DateFormat);
                    ltrlContractStartDate.Text = loanGlobalsDAL.ConvertDateTimeToString(objContractMasterDAL.ContractStartDate, loanAppGlobals.DateFormat);
                    ltrlContractEndDate.Text = loanGlobalsDAL.ConvertDateTimeToString(objContractMasterDAL.ContractStartDate.AddMonths(objContractMasterDAL.NoOfInstallments), loanAppGlobals.DateFormat);
                    ltrlInstallmentDate.Text = loanGlobalsDAL.ConvertDateTimeToString(objContractMasterDAL.InstallmentDate, loanAppGlobals.DateFormat);
                    ltrlContractAmount.Text = objContractMasterDAL.ContractAmount.ToString("0.00");
                    ltrlTotalPaidAmount.Text = objContractMasterDAL.TotalPaid.ToString("0.00");
                    ltrlRemainingAmount.Text = objContractMasterDAL.PendingAmount.ToString("0.00");// (objContractMasterDAL.ContractAmount - objContractMasterDAL.TotalPaid).ToString("0.00");
                    ltrlNoOfInstallments.Text = objContractMasterDAL.NoOfInstallments.ToString();
                    ltrlInstallmentAmount.Text = objContractMasterDAL.InstallmentAmount.ToString("0.00");
                    if (objContractMasterDAL.LastPaidDate != null)
                    {
                        ltrlLastPaidDate.Text = loanGlobalsDAL.ConvertDateTimeToString(objContractMasterDAL.LastPaidDate, loanAppGlobals.DateFormat); ;
                    }
                    if (objContractMasterDAL.LastPaidAmount != null)
                    {
                        ltrlLastPaidAmount.Text = objContractMasterDAL.LastPaidAmount.Value.ToString("0.00");
                    }
                    lblDueAmount.Text = objContractMasterDAL.DueAmount.ToString("0.00");
                    if (Convert.ToDecimal(lblDueAmount.Text) > 0)
                    {
                        lblDueAmount.CssClass = "text-danger";
                    }
                    else if (Convert.ToDecimal(lblDueAmount.Text) < 0)
                    {
                        lblDueAmount.CssClass = "text-success";
                    }
                    ltrlDownPayment.Text = objContractMasterDAL.DownPayment.ToString("0.00");
                    lblSettlementAmount.Text = objContractMasterDAL.SettlementAmount.ToString("0.00");
                    if (Convert.ToDecimal(lblSettlementAmount.Text) > 0)
                    {
                        lblSettlementAmount.CssClass = "text-warning";
                    }
                    ltrlSettlementReason.Text = objContractMasterDAL.SettlementReason;
                    ltrlNotes.Text = objContractMasterDAL.Notes;
                    if (objContractMasterDAL.Links != "")
                    {
                        lnkLinks.Visible = true;
                        lnkLinks.NavigateUrl = objContractMasterDAL.Links;
                        lnkLinks.ToolTip = objContractMasterDAL.Links;
                    }
                    else
                    {
                        lnkLinks.Visible = false;
                    }
                    if (objContractMasterDAL.CustomerLinks != "")
                    {
                        lnkCustomerLinks.Visible = true;
                        lnkCustomerLinks.NavigateUrl = objContractMasterDAL.CustomerLinks;
                        lnkCustomerLinks.ToolTip = objContractMasterDAL.CustomerLinks;
                    }
                    else
                    {
                        lnkCustomerLinks.Visible = false;
                    }

                    if (objContractMasterDAL.ContractStatus == loanContractStatus.Active.ToString())
                    {
                        if (Session["Language"] != null && Session["Language"].ToString() == "ar-sa")
                        {
                            lblContractStatus.Text = "مفتوح";
                        }
                        else
                        {
                            lblContractStatus.Text = objContractMasterDAL.ContractStatus;
                        }
                        lblContractStatus.CssClass = "label label-success";
                    }
                    else if (objContractMasterDAL.ContractStatus == loanContractStatus.Cancelled.ToString())
                    {
                        if (Session["Language"] != null && Session["Language"].ToString() == "ar-sa")
                        {
                            lblContractStatus.Text = "تم إلغاء";
                        }
                        else
                        {
                            lblContractStatus.Text = objContractMasterDAL.ContractStatus;
                        }
                        lblContractStatus.CssClass = "label label-danger";
                    }
                    else if (objContractMasterDAL.ContractStatus == loanContractStatus.Closed.ToString())
                    {
                        if (Session["Language"] != null && Session["Language"].ToString() == "ar-sa")
                        {
                            lblContractStatus.Text = "مغلق";
                        }
                        else
                        {
                            lblContractStatus.Text = objContractMasterDAL.ContractStatus;
                        }
                        lblContractStatus.CssClass = "label label-default";
                    }
                    if (objContractMasterDAL.IsVerified != null)
                    {
                        LinkButton lbtnVerify = (LinkButton)e.Item.FindControl("lbtnVerify");
                        lbtnVerify.Visible = false;
                        Literal ltrlVerifiedBy = (Literal)e.Item.FindControl("ltrlVerifiedBy");
                        ltrlVerifiedBy.Text = objContractMasterDAL.VerifiedBy;
                        if (objContractMasterDAL.VerifiedDateTime != null)
                        {
                            Literal ltrlVerifiedDateTime = (Literal)e.Item.FindControl("ltrlVerifiedDateTime");
                            ltrlVerifiedDateTime.Text = loanGlobalsDAL.ConvertDateTimeToString(objContractMasterDAL.VerifiedDateTime.Value, loanAppGlobals.DateTimeFormat);
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

        protected void lvContractMaster_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName.Equals("EditRecord", StringComparison.CurrentCultureIgnoreCase))
                {
                    loanSessionsDAL.SetSessionKeyValue("ContractMasterId", ((ListView)sender).DataKeys[e.Item.DataItemIndex].Value);
                    Response.Redirect("contractdetails.aspx");
                }
                else if (e.CommandName.Equals("VerifyRecord", StringComparison.CurrentCultureIgnoreCase))
                {
                    string pageName = Path.GetFileNameWithoutExtension(Page.AppRelativeVirtualPath) + "master";
                    VerifyRecord(pageName, Convert.ToInt32(((ListView)sender).DataKeys[e.Item.DataItemIndex].Value));
                }
                else if (e.CommandName.Equals("DeleteRecord", StringComparison.CurrentCultureIgnoreCase))
                {
                    loanUser.CheckRoleRights(loanRoleRights.DeleteRecord);

                    loanContractMasterDAL objContractMasterDAL = new loanContractMasterDAL();
                    objContractMasterDAL.ContractMasterId = Convert.ToInt32(((ListView)sender).DataKeys[e.Item.DataItemIndex].Value);
                    objContractMasterDAL.SessionId = ((loanUser)Session[loanSessionsDAL.UserSession]).SessionId;

                    loanRecordStatus rsStatus = objContractMasterDAL.DeleteContractMaster();
                    if (rsStatus == loanRecordStatus.Success)
                    {
                        loanAppGlobals.ShowMessage(loanMessagesDAL.DeleteSuccess, loanMessageIcon.Success);
                        FillContractMaster();
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
            txtFilterContractDate.Text = loanGlobalsDAL.ConvertDateTimeToString(new DateTime(2000, 01, 01), loanAppGlobals.DateFormat);
            txtFilterContractDateTo.Text = loanGlobalsDAL.ConvertDateTimeToString(loanGlobalsDAL.GetCurrentDateTime().AddYears(1), loanAppGlobals.DateFormat);
            ddlFilterContractStatus.SelectedValue = loanContractStatus.Active.GetHashCode().ToString();

            if (loanSessionsDAL.GetSessionKeyValue("CurrentPageContract") != null)
            {
                pgrContractMaster.CurrentPage = Convert.ToInt16(loanSessionsDAL.GetSessionKeyValue("CurrentPageContract"));
            }
            if (loanSessionsDAL.GetSessionKeyValue("FilterContract") != null)
            {
                loanContractMasterDAL objContractMasterDAL = (loanContractMasterDAL)loanSessionsDAL.GetSessionKeyValue("FilterContract");
                txtFilterCustomer.Text = objContractMasterDAL.Customer;
                txtFilterContractTitle.Text = objContractMasterDAL.ContractTitle;
                if (objContractMasterDAL.FileNo > 0)
                {
                    txtFilterFileNo.Text = objContractMasterDAL.FileNo.ToString();
                }
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
            objContractMasterDAL.ContractTitle = txtFilterContractTitle.Text.Trim();
            if (!string.IsNullOrEmpty(txtFilterFileNo.Text))
            {
                objContractMasterDAL.FileNo = Convert.ToInt32(txtFilterFileNo.Text);
            }
            if (!string.IsNullOrEmpty(txtFilterBankAccountNumber.Text))
            {
                objContractMasterDAL.BankAccountNumber = txtFilterBankAccountNumber.Text.Trim();
            }
            DateTime? ContractDateFrom = null;
            if (!string.IsNullOrEmpty(txtFilterContractDate.Text))
            {
                ContractDateFrom = DateTime.ParseExact(txtFilterContractDate.Text, loanAppGlobals.DateFormat, System.Globalization.DateTimeFormatInfo.InvariantInfo);
                objContractMasterDAL.ContractStartDate = DateTime.ParseExact(txtFilterContractDate.Text, loanAppGlobals.DateFormat, System.Globalization.DateTimeFormatInfo.InvariantInfo);
            }
            DateTime? ContractDateTo = null;
            if (!string.IsNullOrEmpty(txtFilterContractDateTo.Text))
            {
                ContractDateTo = DateTime.ParseExact(txtFilterContractDateTo.Text, loanAppGlobals.DateFormat, System.Globalization.DateTimeFormatInfo.InvariantInfo);
                objContractMasterDAL.ContractDate = DateTime.ParseExact(txtFilterContractDateTo.Text, loanAppGlobals.DateFormat, System.Globalization.DateTimeFormatInfo.InvariantInfo);
            }
            if (ddlFilterContractStatus.SelectedValue != string.Empty)
            {
                objContractMasterDAL.linktoContractStatusMasterId = Convert.ToInt32(ddlFilterContractStatus.SelectedValue);
            }
            objContractMasterDAL.linktoCompanyMasterId = ((loanUser)Session[loanSessionsDAL.UserSession]).CompanyMasterId;
            if (ddlFilterBank.SelectedValue != string.Empty)
            {
                objContractMasterDAL.linktoBankMasterId = Convert.ToInt32(ddlFilterBank.SelectedValue);
            }
            if (ddlFilterVerification.SelectedValue == "Yes")
            {
                objContractMasterDAL.IsVerified = true;
            }
            else if (ddlFilterVerification.SelectedValue == "No")
            {
                objContractMasterDAL.IsVerified = false;
            }
            if (ddlFilterHasurl.SelectedValue == "Yes")
            {
                objContractMasterDAL.HasURL = true;
            }
            else if (ddlFilterHasurl.SelectedValue == "No")
            {
                objContractMasterDAL.HasURL = false;
            }
            if (!string.IsNullOrEmpty(txtFilterDueInstallments.Text))
            {
                objContractMasterDAL.DueInstallments = Convert.ToInt32(txtFilterDueInstallments.Text);
            }
            loanSessionsDAL.SetSessionKeyValue("FilterContract", objContractMasterDAL);
            loanSessionsDAL.SetSessionKeyValue("CurrentPageContract", pgrContractMaster.CurrentPage);

            string OrderBy = null;
            string OrderDir = null;
            if (ddlSortBy.SelectedValue != string.Empty)
            {
                OrderBy = ddlSortBy.SelectedValue.Split(',')[0];
                OrderDir = ddlSortBy.SelectedValue.Split(',')[1];
            }

            int TotalRecords;
            List<loanContractMasterDAL> lstContractMaster = objContractMasterDAL.SelectAllContractMasterPageWise(pgrContractMaster.StartRowIndex, pgrContractMaster.PageSize, out TotalRecords, ContractDateFrom, ContractDateTo, orderBy: OrderBy, orderDir: OrderDir);
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

            loanUser.CheckRoleRights(loanRoleRights.ViewRecord);

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
                FillContractMaster();
            }
        }
        #endregion


    }
}
