using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using loanLibrary;
using System.Threading;
using System.IO;

namespace abLOAN
{
    public partial class customerpayment : BasePage
    {
        //Double TotalAmount;
        List<loanContractInstallmentTranDAL> lstContractInstallmentTran;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                loanSessionsDAL.CheckSession();

                if (!Page.IsPostBack)
                {
                    //loanUser.CheckUserRights(loanUserRights.ViewCustomerPayment);
                    loanUser.CheckRoleRights(loanRoleRights.ViewList);

                    GetUser();
                    GetPaymentType();
                    GetBank();
                    GetBankAccountNumber(0);

                    GetPageDefaults();

                    //loanSessionsDAL.RemoveSessionAllKeyValue();
                    if (Request.QueryString.ToString().Contains("id="))
                    {
                        int id = Convert.ToInt32(Request.QueryString["id"].ToString());
                        GetCustomerPaymentMaster(id);
                    }
                    FillCustomerPaymentMaster();
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
                pgrCustomerPaymentMaster.CurrentPage = 1;
                FillCustomerPaymentMaster();
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
                txtContractTitle.Text = string.Empty;
                txtFromDate.Text = loanGlobalsDAL.ConvertDateTimeToString(new DateTime(2000, 01, 01), loanAppGlobals.DateFormat);
                txtToDate.Text = loanGlobalsDAL.ConvertDateTimeToString(loanGlobalsDAL.GetCurrentDateTime(), loanAppGlobals.DateFormat);
                ddlFilterPaymentType.SelectedIndex = 0;
                ddlFilterUser.SelectedIndex = 0;
                ddlFilterVerifiedBy.SelectedIndex = 0;
                ddlFilterBank.SelectedIndex = 0;
                txtFilterAmount.Text = string.Empty;
                txtFilterChequeNo.Text = string.Empty;
                txtFilterNotes.Text = string.Empty;
                txtFilterBankAccountNumber.Text = string.Empty;

                pgrCustomerPaymentMaster.CurrentPage = 1;
                FillCustomerPaymentMaster();
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
                loanCustomerPaymentMasterDAL objCustomerPaymentMasterDAL = new loanCustomerPaymentMasterDAL();
                objCustomerPaymentMasterDAL.linktoCompanyMasterId = ((loanUser)Session[loanSessionsDAL.UserSession]).CompanyMasterId;
                objCustomerPaymentMasterDAL.linktoCustomerMasterId = Convert.ToInt32(hdnCustomerMasterId.Value);
                objCustomerPaymentMasterDAL.PaymentDate = DateTime.ParseExact(txtPaymentDate.Text, loanAppGlobals.DateFormat, System.Globalization.DateTimeFormatInfo.InvariantInfo);
                objCustomerPaymentMasterDAL.linktoPaymentTypeMasterId = Convert.ToInt32(ddlPaymentType.SelectedValue);
                objCustomerPaymentMasterDAL.Amount = Convert.ToDecimal(txtAmount.Text);
                objCustomerPaymentMasterDAL.ReferenceNo = txtReferenceNo.Text.Trim();
                objCustomerPaymentMasterDAL.ChequeNo = txtChequeNo.Text.Trim();
                objCustomerPaymentMasterDAL.BankAccountNumber = ddlBankAccountNumber.SelectedValue == "" ? "" : ddlBankAccountNumber.SelectedItem.Text;
                if (!string.IsNullOrEmpty(txtChequeDate.Text))
                {
                    objCustomerPaymentMasterDAL.ChequeDate = DateTime.ParseExact(txtChequeDate.Text, loanAppGlobals.DateFormat, System.Globalization.DateTimeFormatInfo.InvariantInfo);
                }
                if (!string.IsNullOrEmpty(ddlBank.Text))
                {
                    objCustomerPaymentMasterDAL.linktoBankMasterId = Convert.ToInt32(ddlBank.SelectedValue);
                }
                objCustomerPaymentMasterDAL.Notes = txtNotes.Text.Trim();

                objCustomerPaymentMasterDAL.UpdateDateTime = loanGlobalsDAL.GetCurrentDateTime();
                objCustomerPaymentMasterDAL.SessionId = ((loanUser)Session[loanSessionsDAL.UserSession]).SessionId;

                CheckBox chkSelect;
                TextBox txtLvAmount;
                List<loanContractInstallmentTranDAL> lstContractInstallmentTranDAL = new List<loanContractInstallmentTranDAL>();

                foreach (ListViewItem item in lvContractMaster.Items)
                {
                    txtLvAmount = (TextBox)item.FindControl("txtLvAmount");
                    chkSelect = (CheckBox)item.FindControl("chkSelect");
                    if (chkSelect != null && chkSelect.Checked)
                    {
                        loanContractInstallmentTranDAL objContractInstallmentTranDAL = new loanContractInstallmentTranDAL();
                        objContractInstallmentTranDAL.InstallmentDate = objCustomerPaymentMasterDAL.PaymentDate;
                        objContractInstallmentTranDAL.InstallmentAmount = Convert.ToDecimal(txtLvAmount.Text);
                        objContractInstallmentTranDAL.linktoCompanyMasterId = objCustomerPaymentMasterDAL.linktoCompanyMasterId;
                        objContractInstallmentTranDAL.linktoContractMasterId = Convert.ToInt32(lvContractMaster.DataKeys[item.DisplayIndex].Value);
                        objContractInstallmentTranDAL.linktoCustomerPaymentMasterId = objCustomerPaymentMasterDAL.CustomerPaymentMasterId;
                        objContractInstallmentTranDAL.Notes = objCustomerPaymentMasterDAL.Notes;
                        objContractInstallmentTranDAL.UpdateDateTime = objCustomerPaymentMasterDAL.UpdateDateTime;
                        objContractInstallmentTranDAL.SessionId = objCustomerPaymentMasterDAL.SessionId;
                        lstContractInstallmentTranDAL.Add(objContractInstallmentTranDAL);
                    }
                }

                if (string.IsNullOrEmpty(hdnActionCustomerPayment.Value))
                {
                    loanUser.CheckRoleRights(loanRoleRights.AddRecord);

                    loanRecordStatus rsStatus = objCustomerPaymentMasterDAL.InsertCustomerPaymentMaster(lstContractInstallmentTranDAL);
                    if (rsStatus == loanRecordStatus.Error)
                    {
                        loanAppGlobals.ShowMessage(loanMessagesDAL.InsertFail, loanMessageIcon.Error);
                        return;
                    }
                    else if (rsStatus == loanRecordStatus.RecordAlreadyExist)
                    {
                        loanAppGlobals.ShowMessage(loanMessagesDAL.AlreadyExist, loanMessageIcon.Warning);
                        hdnModelCustomerPayment.Value = "show";
                        return;
                    }
                    else if (rsStatus == loanRecordStatus.Success)
                    {
                        loanAppGlobals.ShowMessage(loanMessagesDAL.InsertSuccess, loanMessageIcon.Success);
                        if (((Button)sender).ID.Equals("btnSaveAndNew"))
                        {
                            hdnModelCustomerPayment.Value = "clear";
                            btnClearList_Click(btnClearList, new EventArgs());
                        }
                        else
                        {
                            hdnModelCustomerPayment.Value = "hide";
                        }
                        FillCustomerPaymentMaster();
                    }
                }
                else
                {
                    loanUser.CheckRoleRights(loanRoleRights.EditRecord);

                    objCustomerPaymentMasterDAL.CustomerPaymentMasterId = Convert.ToInt32(hdnCustomerPaymentMasterId.Value);
                    loanRecordStatus rsStatus = objCustomerPaymentMasterDAL.UpdateCustomerPaymentMaster(lstContractInstallmentTranDAL);
                    if (rsStatus == loanRecordStatus.Error)
                    {
                        loanAppGlobals.ShowMessage(loanMessagesDAL.UpdateFail, loanMessageIcon.Error);
                        return;
                    }
                    else if (rsStatus == loanRecordStatus.RecordAlreadyExist)
                    {
                        loanAppGlobals.ShowMessage(loanMessagesDAL.AlreadyExist, loanMessageIcon.Warning);
                        hdnModelCustomerPayment.Value = "show";
                        return;
                    }
                    else if (rsStatus == loanRecordStatus.Success)
                    {
                        loanAppGlobals.ShowMessage(loanMessagesDAL.UpdateSuccess, loanMessageIcon.Success);
                        hdnModelCustomerPayment.Value = "hide";
                        FillCustomerPaymentMaster();
                    }
                }


            }

            catch (Exception ex)
            {
                loanAppGlobals.SaveError(ex);
            }
        }

        protected void btnDownload_Click(object sender, EventArgs e)
        {
            loanCustomerPaymentMasterDAL objCustomerPaymentMasterDAL = new loanCustomerPaymentMasterDAL();

            if (txtFilterCustomer.Text != string.Empty)
            {
                objCustomerPaymentMasterDAL.Customer = txtFilterCustomer.Text;
            }
            if (!string.IsNullOrEmpty(txtFromDate.Text))
            {
                objCustomerPaymentMasterDAL.FromDate = DateTime.ParseExact(txtFromDate.Text, loanAppGlobals.DateFormat, System.Globalization.DateTimeFormatInfo.InvariantInfo);
            }
            if (!string.IsNullOrEmpty(txtToDate.Text))
            {
                objCustomerPaymentMasterDAL.ToDate = DateTime.ParseExact(txtToDate.Text, loanAppGlobals.DateFormat, System.Globalization.DateTimeFormatInfo.InvariantInfo);
            }
            if (ddlFilterPaymentType.SelectedValue != string.Empty)
            {
                objCustomerPaymentMasterDAL.linktoPaymentTypeMasterId = Convert.ToInt32(ddlFilterPaymentType.SelectedValue);
            }
            objCustomerPaymentMasterDAL.linktoCompanyMasterId = ((loanUser)Session[loanSessionsDAL.UserSession]).CompanyMasterId;
            objCustomerPaymentMasterDAL.ContractTitle = txtContractTitle.Text.Trim();
            if (ddlFilterVerification.SelectedValue == "Yes")
            {
                objCustomerPaymentMasterDAL.IsVerified = true;
            }
            else if (ddlFilterVerification.SelectedValue == "No")
            {
                objCustomerPaymentMasterDAL.IsVerified = false;
            }
            if (!string.IsNullOrEmpty(ddlFilterUser.SelectedValue))
            {
                objCustomerPaymentMasterDAL.linktoUserMasterId = Convert.ToInt32(ddlFilterUser.SelectedValue);
            }
            if (!string.IsNullOrEmpty(ddlFilterVerifiedBy.SelectedValue))
            {
                objCustomerPaymentMasterDAL.linktoUserMasterIdVerifier = Convert.ToInt32(ddlFilterVerifiedBy.SelectedValue);
            }
            if (!string.IsNullOrEmpty(ddlFilterBank.SelectedValue))
            {
                objCustomerPaymentMasterDAL.linktoBankMasterId = Convert.ToInt32(ddlFilterBank.SelectedValue);
            }
            objCustomerPaymentMasterDAL.Amount = !string.IsNullOrWhiteSpace(txtFilterAmount.Text) ? Convert.ToDecimal(txtFilterAmount.Text.Trim()) : 0;
            objCustomerPaymentMasterDAL.ChequeNo = txtFilterChequeNo.Text.Trim();
            objCustomerPaymentMasterDAL.Notes = txtFilterNotes.Text.Trim();

            int TotalRecords;
            List<loanCustomerPaymentMasterDAL> lstCustomerPaymentMaster = objCustomerPaymentMasterDAL.SelectAllCustomerPaymentMasterPageWise(0, int.MaxValue, out TotalRecords);

            if (lstCustomerPaymentMaster == null)
            {
                loanAppGlobals.ShowMessage(loanMessagesDAL.SelectAllFail, loanMessageIcon.Error);
                return;
            }

            string file = System.Configuration.ConfigurationManager.AppSettings["FileSavePath"] + "CustomerPayment.csv";

            string[] headers = { "Customer", "Payment Date", "Payment Type", "Amount", "Reference No", "Cheque No", "Bank", "Notes", "Verifier", "Modifier" };
            string[] columns = { "Customer", "PaymentDate", "PaymentType", "Amount", "ReferenceNo", "ChequeNo", "Bank", "Notes", "VerifiedBy", "ModifiedBy" };

            bool IsSuccess = loanAppGlobals.ExportCsv(lstCustomerPaymentMaster, headers, columns, file);
            if (!IsSuccess)
            {
                loanAppGlobals.ShowMessage(loanMessagesDAL.Exception, loanMessageIcon.Error);
                return;
            }

            loanAppGlobals.SendOutFile(file, Path.GetFileName(file));
        }

        #region List Methods

        protected void lvCustomerPaymentMaster_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            try
            {
                if (e.Item.ItemType == ListViewItemType.DataItem)
                {
                    loanCustomerPaymentMasterDAL objCustomerPaymentMasterDAL = (loanCustomerPaymentMasterDAL)e.Item.DataItem;

                    Literal ltrlCustomer = (Literal)e.Item.FindControl("ltrlCustomer");
                    HyperLink hlnkCustomer = (HyperLink)e.Item.FindControl("hlnkCustomer");
                    Literal ltrlBankAccountNumber = (Literal)e.Item.FindControl("ltrlBankAccountNumber");
                    Literal ltrlPaymentDate = (Literal)e.Item.FindControl("ltrlPaymentDate");
                    Literal ltrlPaymentType = (Literal)e.Item.FindControl("ltrlPaymentType");
                    Literal ltrlAmount = (Literal)e.Item.FindControl("ltrlAmount");
                    Literal ltrlReferenceNo = (Literal)e.Item.FindControl("ltrlReferenceNo");
                    Literal ltrlContracts = (Literal)e.Item.FindControl("ltrlContracts");
                    Literal ltrlChequeNo = (Literal)e.Item.FindControl("ltrlChequeNo");
                    Literal ltrlChequeDate = (Literal)e.Item.FindControl("ltrlChequeDate");
                    Literal ltrlBank = (Literal)e.Item.FindControl("ltrlBank");
                    Literal ltrlNotes = (Literal)e.Item.FindControl("ltrlNotes");
                    Literal ltrlModifiedBy = (Literal)e.Item.FindControl("ltrlModifiedBy");
                    ltrlModifiedBy.Text = objCustomerPaymentMasterDAL.ModifiedBy;
                    Literal ltrlModifiedDateTime = (Literal)e.Item.FindControl("ltrlModifiedDateTime");
                    ltrlModifiedDateTime.Text = loanGlobalsDAL.ConvertDateTimeToString(objCustomerPaymentMasterDAL.UpdateDateTime, loanAppGlobals.DateTimeFormat);

                    if (objCustomerPaymentMasterDAL.CustomerIsRedFlag == true)
                    {
                        ltrlCustomer.Text = "<span class='text-danger'>" + objCustomerPaymentMasterDAL.Customer + "</span>";
                        hlnkCustomer.Text = "<span class='text-danger'>" + objCustomerPaymentMasterDAL.Customer + "</span>";
                    }
                    else
                    {
                        ltrlCustomer.Text = objCustomerPaymentMasterDAL.Customer;
                        hlnkCustomer.Text = objCustomerPaymentMasterDAL.Customer;
                    }
                    if (objCustomerPaymentMasterDAL.Links != "")
                    {
                        hlnkCustomer.NavigateUrl = objCustomerPaymentMasterDAL.Links;
                        hlnkCustomer.ToolTip = objCustomerPaymentMasterDAL.Links;
                        hlnkCustomer.Visible = true;
                        ltrlCustomer.Visible = false;
                    }
                    ltrlBankAccountNumber.Text = objCustomerPaymentMasterDAL.BankAccountNumber;
                    ltrlPaymentDate.Text = loanGlobalsDAL.ConvertDateTimeToString(objCustomerPaymentMasterDAL.PaymentDate, loanAppGlobals.DateFormat);
                    ltrlPaymentType.Text = objCustomerPaymentMasterDAL.PaymentType;
                    ltrlAmount.Text = objCustomerPaymentMasterDAL.Amount.ToString("0.00");
                    ltrlReferenceNo.Text = objCustomerPaymentMasterDAL.ReferenceNo;
                    ltrlContracts.Text = objCustomerPaymentMasterDAL.Contracts;
                    ltrlChequeNo.Text = objCustomerPaymentMasterDAL.ChequeNo;
                    if (objCustomerPaymentMasterDAL.ChequeDate != null)
                    {
                        ltrlChequeDate.Text = loanGlobalsDAL.ConvertDateTimeToString(objCustomerPaymentMasterDAL.ChequeDate.Value, loanAppGlobals.DateFormat);
                    }
                    ltrlBank.Text = objCustomerPaymentMasterDAL.Bank;
                    ltrlNotes.Text = objCustomerPaymentMasterDAL.Notes;
                    if (objCustomerPaymentMasterDAL.IsVerified != null)
                    {
                        LinkButton lbtnVerify = (LinkButton)e.Item.FindControl("lbtnVerify");
                        lbtnVerify.Visible = false;
                        Literal ltrlVerifiedBy = (Literal)e.Item.FindControl("ltrlVerifiedBy");
                        ltrlVerifiedBy.Text = objCustomerPaymentMasterDAL.VerifiedBy;
                        if (objCustomerPaymentMasterDAL.VerifiedDateTime != null)
                        {
                            Literal ltrlVerifiedDateTime = (Literal)e.Item.FindControl("ltrlVerifiedDateTime");
                            ltrlVerifiedDateTime.Text = loanGlobalsDAL.ConvertDateTimeToString(objCustomerPaymentMasterDAL.VerifiedDateTime.Value, loanAppGlobals.DateTimeFormat);
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

        protected void pgrCustomerPaymentMaster_ItemCommand(object sender, EventArgs e)
        {
            try
            {
                FillCustomerPaymentMaster();
            }
            catch (Exception ex)
            {
                loanAppGlobals.SaveError(ex);
            }
        }

        protected void lvCustomerPaymentMaster_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName.Equals("EditRecord", StringComparison.CurrentCultureIgnoreCase))
                {
                    GetCustomerPaymentMaster(Convert.ToInt32(((ListView)sender).DataKeys[e.Item.DataItemIndex].Value));
                }
                else if (e.CommandName.Equals("VerifyRecord", StringComparison.CurrentCultureIgnoreCase))
                {
                    string pageName = Path.GetFileNameWithoutExtension(Page.AppRelativeVirtualPath) + "master";
                    VerifyRecord(pageName, Convert.ToInt32(((ListView)sender).DataKeys[e.Item.DataItemIndex].Value));
                }
                else if (e.CommandName.Equals("DeleteRecord", StringComparison.CurrentCultureIgnoreCase))
                {
                    loanUser.CheckRoleRights(loanRoleRights.DeleteRecord);

                    loanCustomerPaymentMasterDAL objCustomerPaymentMasterDAL = new loanCustomerPaymentMasterDAL();
                    objCustomerPaymentMasterDAL.CustomerPaymentMasterId = Convert.ToInt32(((ListView)sender).DataKeys[e.Item.DataItemIndex].Value);
                    objCustomerPaymentMasterDAL.SessionId = ((loanUser)Session[loanSessionsDAL.UserSession]).SessionId;

                    loanRecordStatus rsStatus = objCustomerPaymentMasterDAL.DeleteCustomerPaymentMaster();
                    if (rsStatus == loanRecordStatus.Success)
                    {
                        loanAppGlobals.ShowMessage(loanMessagesDAL.DeleteSuccess, loanMessageIcon.Success);
                        FillCustomerPaymentMaster();
                        btnClearList_Click(btnClearList, new EventArgs());
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

        protected void lvContractMaster_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            try
            {
                if (e.Item.ItemType == ListViewItemType.DataItem)
                {
                    loanContractMasterDAL objContractMasterDAL = (loanContractMasterDAL)e.Item.DataItem;

                    Literal ltrlFileNo = (Literal)e.Item.FindControl("ltrlFileNo");
                    Literal ltrlContractTitle = (Literal)e.Item.FindControl("ltrlContractTitle");
                    HyperLink hlnkContractTitle = (HyperLink)e.Item.FindControl("hlnkContractTitle");
                    Literal ltrlBank = (Literal)e.Item.FindControl("ltrlBank");
                    Literal ltrlContractDate = (Literal)e.Item.FindControl("ltrlContractDate");
                    Literal ltrlInstallmentDate = (Literal)e.Item.FindControl("ltrlInstallmentDate");
                    Literal ltrlInstallmentAmount = (Literal)e.Item.FindControl("ltrlInstallmentAmount");
                    Literal ltrlPendingAmount = (Literal)e.Item.FindControl("ltrlPendingAmount");
                    Literal ltrlNotes = (Literal)e.Item.FindControl("ltrlNotes");
                    Label lblDueAmount = (Label)e.Item.FindControl("lblDueAmount");
                    TextBox txtLvAmount = (TextBox)e.Item.FindControl("txtLvAmount");
                    Literal ltrlLast3Installments = (Literal)e.Item.FindControl("ltrlLast3Installments");
                    CheckBox chkSelect = (CheckBox)e.Item.FindControl("chkSelect");

                    ltrlFileNo.Text = objContractMasterDAL.FileNo.ToString();
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
                    ltrlContractDate.Text = loanGlobalsDAL.ConvertDateTimeToString(objContractMasterDAL.ContractDate, loanAppGlobals.DateFormat);
                    ltrlInstallmentDate.Text = loanGlobalsDAL.ConvertDateTimeToString(objContractMasterDAL.InstallmentDate, loanAppGlobals.DateFormat);
                    ltrlInstallmentAmount.Text = objContractMasterDAL.InstallmentAmount.ToString("0.00");
                    ltrlLast3Installments.Text = objContractMasterDAL.Last3Installments;
                    lblDueAmount.Text = objContractMasterDAL.DueAmount.ToString("0.00");
                    ltrlPendingAmount.Text = objContractMasterDAL.PendingAmount.ToString("0.00");
                    ltrlNotes.Text = objContractMasterDAL.Notes;
                    if (objContractMasterDAL.DueAmount > 0)
                    {
                        lblDueAmount.CssClass = "text-danger";
                    }
                    else if (objContractMasterDAL.DueAmount < 0)
                    {
                        lblDueAmount.CssClass = "text-success";
                    }

                    txtLvAmount.Text = lblDueAmount.Text;
                    // TotalAmount += Convert.ToDouble(txtLvAmount.Text);

                    if (!string.IsNullOrEmpty(hdnActionCustomerPayment.Value))
                    {
                        loanContractInstallmentTranDAL objContractInstallmentTranDAL = lstContractInstallmentTran.Find(f => f.linktoContractMasterId == objContractMasterDAL.ContractMasterId);
                        if (objContractInstallmentTranDAL != null)
                        {
                            //Changed on clients request to keep all checkboxes as un-selected.
                            chkSelect.Checked = true;
                            txtLvAmount.Text = objContractInstallmentTranDAL.InstallmentAmount.ToString("0.00");
                        }
                    }
                    else
                    {
                        //Changed on clients request to keep all checkboxes as un-selected.
                        chkSelect.Checked = false;
                        //if (objContractMasterDAL.DueAmount > 0)
                        //{
                        //    chkSelect.Checked = true;
                        //}
                        //else
                        //{
                        //    chkSelect.Checked = false;
                        //}
                    }
                }
                //txtTotalLvAmount.Text = TotalAmount.ToString();
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
            txtFromDate.Text = loanGlobalsDAL.ConvertDateTimeToString(new DateTime(2000, 01, 01), loanAppGlobals.DateFormat);
            txtToDate.Text = loanGlobalsDAL.ConvertDateTimeToString(loanGlobalsDAL.GetCurrentDateTime(), loanAppGlobals.DateFormat);


            if (loanSessionsDAL.GetSessionKeyValue("CurrentPageCustomerPayment") != null)
            {
                pgrCustomerPaymentMaster.CurrentPage = Convert.ToInt16(loanSessionsDAL.GetSessionKeyValue("CurrentPageCustomerPayment"));
            }
            if (loanSessionsDAL.GetSessionKeyValue("FilterCustomerPayment") != null)
            {
                loanCustomerPaymentMasterDAL objCustomerPaymentMasterDAL = (loanCustomerPaymentMasterDAL)loanSessionsDAL.GetSessionKeyValue("FilterCustomerPayment");
                txtContractTitle.Text = objCustomerPaymentMasterDAL.ContractTitle;
                if (objCustomerPaymentMasterDAL.linktoCustomerMasterId > 0)
                {
                    txtFilterCustomer.Text = objCustomerPaymentMasterDAL.linktoCustomerMasterId.ToString();
                }
                //if (objCustomerPaymentMasterDAL.PaymentDate != new DateTime())
                //{
                //    txtFromDate.Text = objCustomerPaymentMasterDAL.PaymentDate.ToString(loanAppGlobals.DateFormat);
                //    txtToDate.Text = objCustomerPaymentMasterDAL.PaymentDate.ToString(loanAppGlobals.DateFormat);
                //}

                ddlFilterPaymentType.SelectedValue = objCustomerPaymentMasterDAL.linktoPaymentTypeMasterId.ToString();

            }
        }

        private void FillCustomerPaymentMaster()
        {

            loanCustomerPaymentMasterDAL objCustomerPaymentMasterDAL = new loanCustomerPaymentMasterDAL();

            if (txtFilterCustomer.Text != string.Empty)
            {
                objCustomerPaymentMasterDAL.Customer = txtFilterCustomer.Text;
            }
            if (!string.IsNullOrEmpty(txtFilterBankAccountNumber.Text))
            {
                objCustomerPaymentMasterDAL.BankAccountNumber = txtFilterBankAccountNumber.Text.Trim();
            }
            if (!string.IsNullOrEmpty(txtFromDate.Text))
            {
                objCustomerPaymentMasterDAL.FromDate = DateTime.ParseExact(txtFromDate.Text, loanAppGlobals.DateFormat, System.Globalization.DateTimeFormatInfo.InvariantInfo);
            }
            if (!string.IsNullOrEmpty(txtToDate.Text))
            {
                objCustomerPaymentMasterDAL.ToDate = DateTime.ParseExact(txtToDate.Text, loanAppGlobals.DateFormat, System.Globalization.DateTimeFormatInfo.InvariantInfo);
            }
            if (ddlFilterPaymentType.SelectedValue != string.Empty)
            {
                objCustomerPaymentMasterDAL.linktoPaymentTypeMasterId = Convert.ToInt32(ddlFilterPaymentType.SelectedValue);
            }
            objCustomerPaymentMasterDAL.linktoCompanyMasterId = ((loanUser)Session[loanSessionsDAL.UserSession]).CompanyMasterId;
            objCustomerPaymentMasterDAL.ContractTitle = txtContractTitle.Text.Trim();
            if (ddlFilterVerification.SelectedValue == "Yes")
            {
                objCustomerPaymentMasterDAL.IsVerified = true;
            }
            else if (ddlFilterVerification.SelectedValue == "No")
            {
                objCustomerPaymentMasterDAL.IsVerified = false;
            }
            if (!string.IsNullOrEmpty(ddlFilterUser.SelectedValue))
            {
                objCustomerPaymentMasterDAL.linktoUserMasterId = Convert.ToInt32(ddlFilterUser.SelectedValue);
            }
            if (!string.IsNullOrEmpty(ddlFilterVerifiedBy.SelectedValue))
            {
                objCustomerPaymentMasterDAL.linktoUserMasterIdVerifier = Convert.ToInt32(ddlFilterVerifiedBy.SelectedValue);
            }
            if (!string.IsNullOrEmpty(ddlFilterBank.SelectedValue))
            {
                objCustomerPaymentMasterDAL.linktoBankMasterId = Convert.ToInt32(ddlFilterBank.SelectedValue);
            }
            objCustomerPaymentMasterDAL.Amount = !string.IsNullOrWhiteSpace(txtFilterAmount.Text) ? Convert.ToDecimal(txtFilterAmount.Text.Trim()) : 0;
            objCustomerPaymentMasterDAL.ChequeNo = txtFilterChequeNo.Text.Trim();
            objCustomerPaymentMasterDAL.Notes = txtFilterNotes.Text.Trim();

            loanSessionsDAL.SetSessionKeyValue("FilterCustomerPayment", objCustomerPaymentMasterDAL);
            loanSessionsDAL.SetSessionKeyValue("CurrentPageCustomerPayment", pgrCustomerPaymentMaster.CurrentPage);

            int TotalRecords;
            List<loanCustomerPaymentMasterDAL> lstCustomerPaymentMaster = objCustomerPaymentMasterDAL.SelectAllCustomerPaymentMasterPageWise(pgrCustomerPaymentMaster.StartRowIndex, 20, out TotalRecords);
            pgrCustomerPaymentMaster.TotalRowCount = TotalRecords;

            if (lstCustomerPaymentMaster == null)
            {
                loanAppGlobals.ShowMessage(loanMessagesDAL.SelectAllFail, loanMessageIcon.Error);
                return;
            }

            if (lstCustomerPaymentMaster.Count == 0 && pgrCustomerPaymentMaster.TotalRowCount > 0)
            {
                pgrCustomerPaymentMaster_ItemCommand(pgrCustomerPaymentMaster, new EventArgs());
                return;
            }

            lvCustomerPaymentMaster.DataSource = lstCustomerPaymentMaster;
            lvCustomerPaymentMaster.DataBind();

            if (lstCustomerPaymentMaster.Count > 0)
            {
                int EndiIndex = pgrCustomerPaymentMaster.StartRowIndex + pgrCustomerPaymentMaster.PageSize < pgrCustomerPaymentMaster.TotalRowCount ? pgrCustomerPaymentMaster.StartRowIndex + pgrCustomerPaymentMaster.PageSize : pgrCustomerPaymentMaster.TotalRowCount;
                lblRecords.Text = string.Format(Resources.Messages.ResourceManager.GetString("Records"), pgrCustomerPaymentMaster.StartRowIndex + 1, EndiIndex, pgrCustomerPaymentMaster.TotalRowCount);
                lblRecords.Visible = true;
            }
            else
            {
                lblRecords.Visible = false;
            }

            if (pgrCustomerPaymentMaster.TotalRowCount <= pgrCustomerPaymentMaster.PageSize)
            {
                pgrCustomerPaymentMaster.Visible = false;
            }
            else
            {
                pgrCustomerPaymentMaster.Visible = true;
            }

        }

        private void GetPaymentType()
        {
            loanPaymentTypeMasterDAL loanPaymentTypeMasterDAL = new loanPaymentTypeMasterDAL();

            ddlPaymentType.Items.Clear();
            ddlPaymentType.Items.Add(new System.Web.UI.WebControls.ListItem(loanDropDownItem.Select, ""));

            ddlFilterPaymentType.Items.Clear();
            ddlFilterPaymentType.Items.Add(new System.Web.UI.WebControls.ListItem(loanDropDownItem.Select, ""));
            int totalRecords = 0;
            List<loanPaymentTypeMasterDAL> lstPaymentTypeMasterDAL = loanPaymentTypeMasterDAL.SelectAllPaymentTypeMasterPageWise(0, int.MaxValue, out totalRecords);
            if (lstPaymentTypeMasterDAL == null)
            {
                loanAppGlobals.ShowMessage(loanMessagesDAL.SelectAllFail, loanMessageIcon.Error);
                return;
            }
            foreach (loanPaymentTypeMasterDAL obj in lstPaymentTypeMasterDAL)
            {
                ddlPaymentType.Items.Add(new System.Web.UI.WebControls.ListItem(obj.PaymentType, obj.PaymentTypeMasterId.ToString()));
                ddlFilterPaymentType.Items.Add(new System.Web.UI.WebControls.ListItem(obj.PaymentType, obj.PaymentTypeMasterId.ToString()));
            }
        }

        private void GetBank()
        {
            ddlBank.Items.Clear();
            ddlBank.Items.Add(new System.Web.UI.WebControls.ListItem(loanDropDownItem.Select, ""));

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
                ddlBank.Items.Add(new System.Web.UI.WebControls.ListItem(obj.BankName, obj.BankMasterId.ToString()));
                ddlFilterBank.Items.Add(new System.Web.UI.WebControls.ListItem(obj.BankName, obj.BankMasterId.ToString()));
            }
        }

        private void GetBankAccountNumber(int customerMasterId)
        {
            ddlBankAccountNumber.Items.Clear();
            ddlBankAccountNumber.Items.Add(new System.Web.UI.WebControls.ListItem(loanDropDownItem.Select, ""));

            if (customerMasterId > 0)
            {
                List<loanCustomerMasterDAL> lstCustomerMasterDAL = loanCustomerMasterDAL.SelectAllCustomerMasterBankAccountNumbers(customerMasterId);
                if (lstCustomerMasterDAL == null)
                {
                    loanAppGlobals.ShowMessage(loanMessagesDAL.SelectAllFail, loanMessageIcon.Error);
                    return;
                }
                int i = 1;
                foreach (loanCustomerMasterDAL obj in lstCustomerMasterDAL)
                {
                    ddlBankAccountNumber.Items.Add(new System.Web.UI.WebControls.ListItem(obj.BankAccountNumber, i.ToString()));
                    i++;
                }
            }
        }

        private void GetCustomerPaymentMaster(int CustomerPaymentMasterId)
        {
            loanUser.CheckRoleRights(loanRoleRights.ViewRecord);
            hdnModelCustomerPayment.Value = "show";
            hdnActionCustomerPayment.Value = "edit";

            loanCustomerPaymentMasterDAL objCustomerPaymentMasterDAL = new loanCustomerPaymentMasterDAL();
            objCustomerPaymentMasterDAL.CustomerPaymentMasterId = CustomerPaymentMasterId;
            if (!objCustomerPaymentMasterDAL.SelectCustomerPaymentMaster())
            {
                loanAppGlobals.ShowMessage(loanMessagesDAL.SelectFail, loanMessageIcon.Error);
                return;
            }
            hdnCustomerPaymentMasterId.Value = objCustomerPaymentMasterDAL.CustomerPaymentMasterId.ToString();

            hdnCustomerMasterId.Value = objCustomerPaymentMasterDAL.linktoCustomerMasterId.ToString();
            GetBankAccountNumber(objCustomerPaymentMasterDAL.linktoCustomerMasterId);
            ddlBankAccountNumber.SelectedIndex = ddlBankAccountNumber.Items.IndexOf(ddlBankAccountNumber.Items.FindByText(objCustomerPaymentMasterDAL.BankAccountNumber?.ToString()));

            txtPaymentDate.Text = loanGlobalsDAL.ConvertDateTimeToString(objCustomerPaymentMasterDAL.PaymentDate, loanAppGlobals.DateFormat);
            ddlPaymentType.SelectedIndex = ddlPaymentType.Items.IndexOf(ddlPaymentType.Items.FindByValue(objCustomerPaymentMasterDAL.linktoPaymentTypeMasterId.ToString()));
            txtAmount.Text = objCustomerPaymentMasterDAL.Amount.ToString("0");
            txtReferenceNo.Text = objCustomerPaymentMasterDAL.ReferenceNo;
            txtChequeNo.Text = objCustomerPaymentMasterDAL.ChequeNo;
            if (objCustomerPaymentMasterDAL.ChequeDate != null)
            {
                txtChequeDate.Text = loanGlobalsDAL.ConvertDateTimeToString(objCustomerPaymentMasterDAL.ChequeDate, loanAppGlobals.DateFormat);
            }
            ddlBank.SelectedIndex = ddlBank.Items.IndexOf(ddlBank.Items.FindByValue(objCustomerPaymentMasterDAL.linktoBankMasterId.ToString()));
            txtNotes.Text = objCustomerPaymentMasterDAL.Notes;
            txtCustomerName.Text = objCustomerPaymentMasterDAL.Customer;
            txtSearchCustomerIdNo.Text = objCustomerPaymentMasterDAL.IdNo;
            txtIdNo.Text = objCustomerPaymentMasterDAL.IdNo;

            FillContractInstallmentTran(CustomerPaymentMasterId);
            btnSearchCustomerIdNo_Click(null, null);
            txtSearchCustomerIdNo.Enabled = false;
            txtFilterContractTitle.Enabled = false;


        }

        #endregion

        private void FillContractMaster()
        {

            loanContractMasterDAL objContractMasterDAL = new loanContractMasterDAL();
            if (txtIdNo.Text != "")
            {
                objContractMasterDAL.linktoCustomerMasterId = Convert.ToInt32(hdnCustomerMasterId.Value);
                objContractMasterDAL.linktoCompanyMasterId = ((loanUser)Session[loanSessionsDAL.UserSession]).CompanyMasterId;
                objContractMasterDAL.ContractTitle = txtFilterContractTitle.Text.Trim();
                objContractMasterDAL.linktoContractStatusMasterId = loanContractStatus.Active.GetHashCode();
                int CustomerPaymentContractMasterId = 0;
                if (!string.IsNullOrEmpty(hdnActionCustomerPayment.Value))
                {
                    ;// objContractMasterDAL.ContractMasterId=
                    if (loanSessionsDAL.GetSessionKeyValue("CustomerPaymentContractMasterId") != null)
                    {
                        CustomerPaymentContractMasterId = (int)loanSessionsDAL.GetSessionKeyValue("CustomerPaymentContractMasterId");
                        //objContractMasterDAL.ContractMasterId = CustomerPaymentContractMasterId;
                    }
                }

                loanSessionsDAL.SetSessionKeyValue("FilterContract", objContractMasterDAL);

                int TotalRecords;
                List<loanContractMasterDAL> lstContractMaster = objContractMasterDAL.SelectAllContractMasterPageWise(0, int.MaxValue, out TotalRecords, null, null, false, CustomerPaymentContractMasterId);

                if (lstContractMaster == null)
                {
                    loanAppGlobals.ShowMessage(loanMessagesDAL.SelectAllFail, loanMessageIcon.Error);
                    return;
                }
                lvContractMaster.DataSource = lstContractMaster;
                lvContractMaster.DataBind();
            }
        }

        private void FillContractInstallmentTran(int linktoCustomerPaymentMasterId)
        {

            loanContractInstallmentTranDAL objContractInstallmentTranDAL = new loanContractInstallmentTranDAL();
            objContractInstallmentTranDAL.linktoCompanyMasterId = ((loanUser)Session[loanSessionsDAL.UserSession]).CompanyMasterId;

            objContractInstallmentTranDAL.linktoCustomerPaymentMasterId = linktoCustomerPaymentMasterId;
            loanSessionsDAL.SetSessionKeyValue("FilterContractInstallment", objContractInstallmentTranDAL);

            int TotalRecords;
            lstContractInstallmentTran = objContractInstallmentTranDAL.SelectAllContractInstallmentTranPageWise(0, int.MaxValue, out TotalRecords);
            loanSessionsDAL.SetSessionKeyValue("CustomerPaymentContractMasterId", lstContractInstallmentTran[0].linktoCustomerPaymentMasterId);
            if (lstContractInstallmentTran == null)
            {
                loanAppGlobals.ShowMessage(loanMessagesDAL.SelectAllFail, loanMessageIcon.Error);
                return;
            }
            //lvContractMaster.DataSource = lstContractInstallmentTran;
            //lvContractMaster.DataBind();

        }

        protected void btnSearchCustomerIdNo_Click(object sender, EventArgs e)
        {
            try
            {
                string IdNo = txtSearchCustomerIdNo.Text.Trim();
                if (IdNo.Contains("("))
                {
                    IdNo = IdNo.Substring(IdNo.IndexOf("(") + 1, IdNo.IndexOf(")") - IdNo.IndexOf("(") - 1);
                }
                loanCustomerMasterDAL objCustomerMasterDAL = new loanCustomerMasterDAL();
                objCustomerMasterDAL.IdNo = IdNo;
                if (!objCustomerMasterDAL.SelectCustomerMaster())
                {
                    loanAppGlobals.ShowMessage(loanMessagesDAL.NotFound, loanMessageIcon.Warning);
                    btnClearList_Click(btnClearList, new EventArgs());
                    return;
                }
                hdnCustomerMasterId.Value = objCustomerMasterDAL.CustomerMasterId.ToString();
                txtIdNo.Text = IdNo;
                txtCustomerName.Text = objCustomerMasterDAL.CustomerName;
                FillContractMaster();
                if (sender != null)
                {
                    GetBankAccountNumber(objCustomerMasterDAL.CustomerMasterId);
                }
            }
            catch (Exception ex)
            {
                loanAppGlobals.SaveError(ex);
            }
        }

        protected void btnClearList_Click(object sender, EventArgs e)
        {
            lvContractMaster.DataSource = null;
            lvContractMaster.DataBind();
            txtSearchCustomerIdNo.Enabled = true;
            txtFilterContractTitle.Enabled = true;

            hdnActionCustomerPayment.Value = "";
            hdnCustomerMasterId.Value = "";

            txtPaymentDate.Text = loanGlobalsDAL.ConvertDateTimeToString(loanGlobalsDAL.GetCurrentDateTime(), loanAppGlobals.DateFormat);
            ddlPaymentType.SelectedIndex = 0;
            txtSearchCustomerIdNo.Text = "";
            txtFilterContractTitle.Text = "";
            txtIdNo.Text = "";
            txtCustomerName.Text = "";
            txtAmount.Text = "";
            txtTotalLvAmount.Text = "";
            txtReferenceNo.Text = "";
            txtChequeNo.Text = "";
            txtChequeDate.Text = "";
            ddlBank.SelectedIndex = 0;
            txtNotes.Text = "";

            GetBankAccountNumber(0);
            ddlBankAccountNumber.SelectedIndex = 0;
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
                FillCustomerPaymentMaster();
            }
        }

        private void GetUser()
        {
            ddlFilterUser.Items.Clear();
            ddlFilterUser.Items.Add(new System.Web.UI.WebControls.ListItem(loanDropDownItem.Select, ""));
            ddlFilterVerifiedBy.Items.Clear();
            ddlFilterVerifiedBy.Items.Add(new System.Web.UI.WebControls.ListItem(loanDropDownItem.Select, ""));

            List<loanUserMasterDAL> lstUserMasterDAL = loanUserMasterDAL.SelectAllUserMaster();
            if (lstUserMasterDAL == null)
            {
                loanAppGlobals.ShowMessage(loanMessagesDAL.SelectAllFail, loanMessageIcon.Error);
                return;
            }

            foreach (loanUserMasterDAL obj in lstUserMasterDAL)
            {
                ddlFilterUser.Items.Add(new System.Web.UI.WebControls.ListItem(obj.Username, obj.UserMasterId.ToString()));
                ddlFilterVerifiedBy.Items.Add(new System.Web.UI.WebControls.ListItem(obj.Username, obj.UserMasterId.ToString()));
            }
        }
    }
}
