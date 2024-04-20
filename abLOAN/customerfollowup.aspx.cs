using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using loanLibrary;
using System.Threading;
using System.Globalization;
using System.IO;
using System.Collections;
using System.Linq;
using System.Web.Hosting;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Web.UI.HtmlControls;
using System.Net.NetworkInformation;
using System.Web;
using System.Web.Services.Description;
using System.Windows.Input;

namespace abLOAN
{
    public partial class customerfollowup : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                loanSessionsDAL.CheckSession();

                if (!Page.IsPostBack)
                {

                    loanUser.CheckRoleRights(loanRoleRights.ViewList);

                    GetContractStatus();
                    GetPageDefaults();

                    GetAuditors();
                    loanSessionsDAL.RemoveSessionAllKeyValue();

                    ddlSortBy.Items.Add(new System.Web.UI.WebControls.ListItem(Resources.Resource.Customer + " " + Resources.Resource.ASC, "Customer,ASC"));
                    ddlSortBy.Items.Add(new System.Web.UI.WebControls.ListItem(Resources.Resource.Customer + " " + Resources.Resource.DESC, "Customer,DESC"));
                    ddlSortBy.Items.Add(new System.Web.UI.WebControls.ListItem(Resources.Resource.ContractStartDate + " " + Resources.Resource.ASC, "ContractStartDate,ASC"));
                    ddlSortBy.Items.Add(new System.Web.UI.WebControls.ListItem(Resources.Resource.ContractStartDate + " " + Resources.Resource.DESC, "ContractStartDate,DESC"));
                    ddlSortBy.Items.Add(new System.Web.UI.WebControls.ListItem(Resources.Resource.CreateDate + " " + Resources.Resource.ASC, "CreateDate,ASC"));
                    ddlSortBy.Items.Add(new System.Web.UI.WebControls.ListItem(Resources.Resource.CreateDate + " " + Resources.Resource.DESC, "CreateDate,DESC"));


                    FillFollowdupCustomerMaster();

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
                pgrFollowupMaster.CurrentPage = 1;
                FillFollowdupCustomerMaster();
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
                txtFilterContractDate.Text = loanGlobalsDAL.ConvertDateTimeToString(new DateTime(2000, 01, 01), loanAppGlobals.DateFormat);
                txtFilterContractDateTo.Text = loanGlobalsDAL.ConvertDateTimeToString(loanGlobalsDAL.GetCurrentDateTime().AddYears(1), loanAppGlobals.DateFormat);
                ddlFilterContractStatus.SelectedValue = loanContractStatus.Active.GetHashCode().ToString();

                ddlSortBy.SelectedValue = string.Empty;

                pgrFollowupMaster.CurrentPage = 1;
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






            string OrderBy = null;
            string OrderDir = null;
            if (ddlSortBy.SelectedValue != string.Empty)
            {
                OrderBy = ddlSortBy.SelectedValue.Split(',')[0];
                OrderDir = ddlSortBy.SelectedValue.Split(',')[1];
            }

            int TotalRecords;
            List<loanContractMasterDAL> lstContractMaster = objContractMasterDAL.SelectAllContractMasterByFollowupPageWise(0, int.MaxValue, out TotalRecords, ContractDateFrom, ContractDateTo, orderBy: OrderBy, orderDir: OrderDir);
            //pgrContractMaster.TotalRowCount = TotalRecords;

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

        protected void insertNoteBtn_Click(object sender, EventArgs e)
        {
            // Get the reference to the button that was clicked
            Button btnSubmit = (Button)sender;

            // Find the form associated with the button
            Control container = btnSubmit.NamingContainer;

            // Find the TextBox within the form
            TextBox txtInput = (TextBox)container.FindControl("ltrlNotes");
            HiddenField lblCustomerFollowupId = (HiddenField)container.FindControl("lblCustomerFollowupId");
            // Access the value of the TextBox
            string notes = txtInput.Text;

            if (String.IsNullOrEmpty(notes))
            { 
                loanAppGlobals.ShowMessage( "NoteMissing" , loanMessageIcon.Warning);
                return;
            }

            int id = int.Parse(lblCustomerFollowupId.Value);


            try
            {
                loanFollowupNoteDAL objFollowupNoteDAL = new loanFollowupNoteDAL();
                objFollowupNoteDAL.linktoCustomerFollowupId = id;
                objFollowupNoteDAL.linktoUserMasterId = ((loanUser)Session[loanSessionsDAL.UserSession]).UserMasterId;
                objFollowupNoteDAL.Notes = notes;
                objFollowupNoteDAL.UpdateDateTime = loanGlobalsDAL.GetCurrentDateTime();
                objFollowupNoteDAL.SessionId = ((loanUser)Session[loanSessionsDAL.UserSession]).SessionId;

                var rsStatus = objFollowupNoteDAL.InsertCustomerfollowupNotes();

                if (rsStatus == loanRecordStatus.Error)
                {
                    loanAppGlobals.ShowMessage(loanMessagesDAL.UpdateFail, loanMessageIcon.Error);
                    return;
                }
                else if (rsStatus == loanRecordStatus.Success)
                {
                    loanAppGlobals.ShowMessage(loanMessagesDAL.UpdateSuccess, loanMessageIcon.Success);
                    FillFollowdupCustomerMaster();
                    FillContractMaster();
                    lvCustomerFollowedup.DataBind();

                }


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
                loanCustomerFollowupDAL objCustomerFollowupDAL = new loanCustomerFollowupDAL();
                objCustomerFollowupDAL.linktoCompanyMasterId = ((loanUser)Session[loanSessionsDAL.UserSession]).CompanyMasterId;
                objCustomerFollowupDAL.linktoCustomerMasterId = Convert.ToInt32(hdnCustomerMasterId.Value);
                objCustomerFollowupDAL.linktoUserMasterId = Convert.ToInt32(ddlAuditors.SelectedValue);


                objCustomerFollowupDAL.UpdateDateTime = loanGlobalsDAL.GetCurrentDateTime();
                objCustomerFollowupDAL.SessionId = ((loanUser)Session[loanSessionsDAL.UserSession]).SessionId;





                if (string.IsNullOrEmpty(hdnActionFollowup.Value))
                {

                    loanUser.CheckRoleRights(loanRoleRights.AddRecord);

                    loanRecordStatus rsStatus = objCustomerFollowupDAL.InsertCustomerfollowup();
                    if (rsStatus == loanRecordStatus.Error)
                    {
                        loanAppGlobals.ShowMessage(loanMessagesDAL.InsertFail, loanMessageIcon.Error);
                        return;
                    }
                    else if (rsStatus == loanRecordStatus.RecordAlreadyExist)
                    {
                        loanAppGlobals.ShowMessage(loanMessagesDAL.AlreadyExist, loanMessageIcon.Warning);
                        hdnModelFollowup.Value = "show";
                        return;
                    }
                    else if (rsStatus == loanRecordStatus.Success)
                    {
                        loanAppGlobals.ShowMessage(loanMessagesDAL.InsertSuccess, loanMessageIcon.Success);
                        if (((Button)sender).ID.Equals("btnSaveAndNew"))
                        {
                            hdnModelFollowup.Value = "clear";
                           
                        }
                        else
                        {
                            hdnModelFollowup.Value = "hide";
                        }
                        FillFollowdupCustomerMaster();
                        FillContractMaster();
                        lvCustomerFollowedup.DataBind();

                    }
                }
                else
                {
                    loanUser.CheckRoleRights(loanRoleRights.EditRecord);

                    objCustomerFollowupDAL.UpdateDateTime = loanGlobalsDAL.GetCurrentDateTime();
                    objCustomerFollowupDAL.CustomerFollowupId = Convert.ToInt32(hdnCustomerFollowupId.Value);
                    loanRecordStatus rsStatus = objCustomerFollowupDAL.UpdateCustomerfollowup();
                    if (rsStatus == loanRecordStatus.Error)
                    {
                        loanAppGlobals.ShowMessage(loanMessagesDAL.UpdateFail, loanMessageIcon.Error);
                        return;
                    }
                    else if (rsStatus == loanRecordStatus.RecordAlreadyExist)
                    {
                        loanAppGlobals.ShowMessage(loanMessagesDAL.AlreadyExist, loanMessageIcon.Warning);
                        hdnModelFollowup.Value = "show";
                        return;
                    }
                    else if (rsStatus == loanRecordStatus.Success)
                    {
                        loanAppGlobals.ShowMessage(loanMessagesDAL.UpdateSuccess, loanMessageIcon.Success);
                        hdnModelFollowup.Value = "hide";
                        FillFollowdupCustomerMaster(); 
                        lvCustomerFollowedup.DataBind();
                    }
                }
            }
            catch (Exception ex)
            {
                loanAppGlobals.SaveError(ex);
            }
        }

        #region List Methods




        protected void lvCustomerFollowedup_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName.Equals("EditRecord", StringComparison.CurrentCultureIgnoreCase))
                {
                    GetFollowdupCustomer(Convert.ToInt32(((ListView)sender).DataKeys[e.Item.DataItemIndex].Value));

                }
                else if (e.CommandName.Equals("VerifyRecord", StringComparison.CurrentCultureIgnoreCase))
                {
                    string pageName = Path.GetFileNameWithoutExtension(Page.AppRelativeVirtualPath) + "master";
                    VerifyRecord(pageName, Convert.ToInt32(((ListView)sender).DataKeys[e.Item.DataItemIndex].Value));
                }
                else if (e.CommandName.Equals("DeleteRecord", StringComparison.CurrentCultureIgnoreCase))
                {
                    loanUser.CheckRoleRights(loanRoleRights.DeleteRecord);

                    loanCustomerFollowupDAL objCustomerFollowupDAL = new loanCustomerFollowupDAL();
                    objCustomerFollowupDAL.CustomerFollowupId = Convert.ToInt32(((ListView)sender).DataKeys[e.Item.DataItemIndex].Value);
                    objCustomerFollowupDAL.SessionId = ((loanUser)Session[loanSessionsDAL.UserSession]).SessionId;

                    loanRecordStatus rsStatus = objCustomerFollowupDAL.DeleteCustomerfollowup();
                    if (rsStatus == loanRecordStatus.Success)
                    {
                        loanAppGlobals.ShowMessage(loanMessagesDAL.DeleteSuccess, loanMessageIcon.Success);
                        FillContractMaster();
                        FillFollowdupCustomerMaster();
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
        protected void lvUserMaster_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName.Equals("EditRecord", StringComparison.CurrentCultureIgnoreCase))
                {
                    //  FillContractMaster(Convert.ToInt32(((ListView)sender).DataKeys[e.Item.DataItemIndex].Value));
                }
                else if (e.CommandName.Equals("VerifyRecord", StringComparison.CurrentCultureIgnoreCase))
                {
                    string pageName = Path.GetFileNameWithoutExtension(Page.AppRelativeVirtualPath) + "master";
                    VerifyRecord(pageName, Convert.ToInt32(((ListView)sender).DataKeys[e.Item.DataItemIndex].Value));
                }
                else if (e.CommandName.Equals("DeleteRecord", StringComparison.CurrentCultureIgnoreCase))
                {
                    loanUser.CheckRoleRights(loanRoleRights.DeleteRecord);

                    loanCustomerMasterDAL objCustomerMasterDAL = new loanCustomerMasterDAL();
                    objCustomerMasterDAL.CustomerMasterId = Convert.ToInt32(((ListView)sender).DataKeys[e.Item.DataItemIndex].Value);
                    objCustomerMasterDAL.SessionId = ((loanUser)Session[loanSessionsDAL.UserSession]).SessionId;

                    loanRecordStatus rsStatus = objCustomerMasterDAL.DeleteCustomerMaster();
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
                    Label lblLast3Payments = (Label)e.Item.FindControl("lblLast3Payments");
                    //Literal ltrlModifiedBy = (Literal)e.Item.FindControl("ltrlModifiedBy");
                    //ltrlModifiedBy.Text = objContractMasterDAL.ModifiedBy;
                    //Literal ltrlModifiedDateTime = (Literal)e.Item.FindControl("ltrlModifiedDateTime");
                    //ltrlModifiedDateTime.Text = loanGlobalsDAL.ConvertDateTimeToString(objContractMasterDAL.UpdateDateTime, loanAppGlobals.DateTimeFormat);

                    //Literal ltrlCreatedBy = (Literal)e.Item.FindControl("ltrlCreatedBy");
                    //ltrlCreatedBy.Text = objContractMasterDAL.CreatedBy;
                    //Literal ltrlCreatedDateTime = (Literal)e.Item.FindControl("ltrlCreatedDateTime");
                    //ltrlCreatedDateTime.Text = loanGlobalsDAL.ConvertDateTimeToString(objContractMasterDAL.CreateDateTime, loanAppGlobals.DateTimeFormat);

                    if (objContractMasterDAL.CustomerIsRedFlag == true)
                    {
                        //ltrlCustomer.Text = "<span class='text-danger'>" + objContractMasterDAL.Customer + "</span>";
                        //hlnkCustomer.Text = "<span class='text-danger'>" + objContractMasterDAL.Customer + "</span>";
                    }
                    else
                    {

                        //ltrlCustomer.Text = objContractMasterDAL.Customer;
                        //hlnkCustomer.Text = objContractMasterDAL.Customer;
                    }
                    //if (objContractMasterDAL.CustomerLinks != "")
                    //{
                    //    hlnkCustomer.NavigateUrl = objContractMasterDAL.CustomerLinks;
                    //    hlnkCustomer.ToolTip = objContractMasterDAL.CustomerLinks;
                    //    hlnkCustomer.Visible = true;
                    //    ltrlCustomer.Visible = false;
                    //}

                    //ltrlCustomerIdNo.Text = objContractMasterDAL.CustomerIdNo;
                    //ltrlGuarantorName.Text = objContractMasterDAL.GuarantorName;
                    //ltrlBankAccountNumber.Text = objContractMasterDAL.BankAccountNumber;
                    //ltrlBankAccountNumber2.Text = objContractMasterDAL.BankAccountNumber2;
                    //ltrlBankAccountNumber3.Text = objContractMasterDAL.BankAccountNumber3;
                    //ltrlBankAccountNumber4.Text = objContractMasterDAL.BankAccountNumber4;
                    ltrlContractTitle.Text = objContractMasterDAL.ContractTitle;
                    //hlnkContractTitle.Text = objContractMasterDAL.ContractTitle;
                    //if (objContractMasterDAL.Links != "")
                    //{
                    //    hlnkContractTitle.NavigateUrl = objContractMasterDAL.Links;
                    //    hlnkContractTitle.ToolTip = objContractMasterDAL.Links;
                    //    hlnkContractTitle.Visible = true;
                    //    ltrlContractTitle.Visible = false;
                    //}
                    //ltrlBank.Text = objContractMasterDAL.Bank;
                    //ltrlFileNo.Text = objContractMasterDAL.FileNo.ToString();
                    //   ltrlContractDate.Text = loanGlobalsDAL.ConvertDateTimeToString(objContractMasterDAL.ContractDate, loanAppGlobals.DateFormat);
                    //ltrlContractStartDate.Text = loanGlobalsDAL.ConvertDateTimeToString(objContractMasterDAL.ContractStartDate, loanAppGlobals.DateFormat);
                    //ltrlContractEndDate.Text = loanGlobalsDAL.ConvertDateTimeToString(objContractMasterDAL.ContractStartDate.AddMonths(objContractMasterDAL.NoOfInstallments), loanAppGlobals.DateFormat);
                    ltrlInstallmentDate.Text = loanGlobalsDAL.ConvertDateTimeToString(objContractMasterDAL.InstallmentDate, loanAppGlobals.DateFormat);
                    //ltrlContractAmount.Text = objContractMasterDAL.ContractAmount.ToString("0.00");
                    //ltrlTotalPaidAmount.Text = objContractMasterDAL.TotalPaid.ToString("0.00");
                    //ltrlRemainingAmount.Text = objContractMasterDAL.PendingAmount.ToString("0.00");// (objContractMasterDAL.ContractAmount - objContractMasterDAL.TotalPaid).ToString("0.00");
                    //ltrlNoOfInstallments.Text = objContractMasterDAL.NoOfInstallments.ToString();
                    //ltrlInstallmentAmount.Text = objContractMasterDAL.InstallmentAmount.ToString("0.00");
                    //if (objContractMasterDAL.LastPaidDate != null)
                    //{
                    //    ltrlLastPaidDate.Text = loanGlobalsDAL.ConvertDateTimeToString(objContractMasterDAL.LastPaidDate, loanAppGlobals.DateFormat); ;
                    //}
                    //if (objContractMasterDAL.LastPaidAmount != null)
                    //{
                    //    ltrlLastPaidAmount.Text = objContractMasterDAL.LastPaidAmount.Value.ToString("0.00");
                    //}
                    //lblDueAmount.Text = objContractMasterDAL.DueAmount.ToString("0.00");
                    //if (Convert.ToDecimal(lblDueAmount.Text) > 0)
                    //{
                    //    lblDueAmount.CssClass = "text-danger";
                    //}
                    //else if (Convert.ToDecimal(lblDueAmount.Text) < 0)
                    //{
                    //    lblDueAmount.CssClass = "text-success";
                    //}

                    lblLast3Payments.Text = objContractMasterDAL.Last3Installments.ToString();


                    //  ltrlDownPayment.Text = objContractMasterDAL.DownPayment.ToString("0.00");
                    //lblSettlementAmount.Text = objContractMasterDAL.SettlementAmount.ToString("0.00");
                    //if (Convert.ToDecimal(lblSettlementAmount.Text) > 0)
                    //{
                    //    lblSettlementAmount.CssClass = "text-warning";
                    //}
                    //ltrlSettlementReason.Text = objContractMasterDAL.SettlementReason;
                    //ltrlNotes.Text = objContractMasterDAL.Notes;
                    //if (objContractMasterDAL.Links != "")
                    //{
                    //    lnkLinks.Visible = true;
                    //    lnkLinks.NavigateUrl = objContractMasterDAL.Links;
                    //    lnkLinks.ToolTip = objContractMasterDAL.Links;
                    //}
                    //else
                    //{
                    //    lnkLinks.Visible = false;
                    //}
                    //if (objContractMasterDAL.CustomerLinks != "")
                    //{
                    //    lnkCustomerLinks.Visible = true;
                    //    lnkCustomerLinks.NavigateUrl = objContractMasterDAL.CustomerLinks;
                    //    lnkCustomerLinks.ToolTip = objContractMasterDAL.CustomerLinks;
                    //}
                    //else
                    //{
                    //    lnkCustomerLinks.Visible = false;
                    //}

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
                        //LinkButton lbtnVerify = (LinkButton)e.Item.FindControl("lbtnVerify");
                        //lbtnVerify.Visible = false;
                        //Literal ltrlVerifiedBy = (Literal)e.Item.FindControl("ltrlVerifiedBy");
                        //ltrlVerifiedBy.Text = objContractMasterDAL.VerifiedBy;
                        //if (objContractMasterDAL.VerifiedDateTime != null)
                        //{
                        //    Literal ltrlVerifiedDateTime = (Literal)e.Item.FindControl("ltrlVerifiedDateTime");
                        //    ltrlVerifiedDateTime.Text = loanGlobalsDAL.ConvertDateTimeToString(objContractMasterDAL.VerifiedDateTime.Value, loanAppGlobals.DateTimeFormat);
                        //}
                    }
                    if (loanUser.GetRoleRights(loanRoleRights.Custom, "VerifyRecord") == false)
                    {
                        //LinkButton lbtnVerify = (LinkButton)e.Item.FindControl("lbtnVerify");
                        //lbtnVerify.Visible = false;
                    }
                }
            }
            catch (Exception ex)
            {
                loanAppGlobals.SaveError(ex);
            }
        } 
        protected void lvFollowupNote_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            try
            {
                if (e.Item.ItemType == ListViewItemType.DataItem)
                {

                    loanFollowupNoteDAL objFollowupNoteDAL = (loanFollowupNoteDAL)e.Item.DataItem;

                    Literal ltrlFollowupNoteId = (Literal)e.Item.FindControl("ltrlFollowupNoteId");
                    ltrlFollowupNoteId.Text = objFollowupNoteDAL.FollowupNoteId.ToString();

                    Literal ltrlNote = (Literal)e.Item.FindControl("ltrlNote");
                    ltrlNote.Text = objFollowupNoteDAL.Notes.ToString();

                    //Literal ltrlUserMasterId = (Literal)e.Item.FindControl("ltrlUserMasterId");
                    //ltrlUserMasterId.Text = objFollowupNoteDAL.linktoUserMasterId.ToString();

                    Literal ltrlUsername = (Literal)e.Item.FindControl("ltrlUsername");
                    ltrlUsername.Text = objFollowupNoteDAL.Username.ToString();

                    Label lblCreateDate = (Label)e.Item.FindControl("ltrlCreateDate");
                    var d = loanGlobalsDAL.ConvertDateTimeToString(objFollowupNoteDAL.CreateDateTime, loanAppGlobals.DateFormat);
                    lblCreateDate.Text = d;

                    //Label lblNoteCount = (Label)e.Item.FindControl("lblNoteCount");
                    //lblNoteCount.Text = objFollowupNoteDAL.totalRowCount.ToString();


                }
            }
            catch (Exception ex)
            {
                loanAppGlobals.SaveError(ex);
            }
        } 
        protected void pgrFollowupMaster_ItemCommand(object sender, EventArgs e)
        {
            try
            {
                FillFollowdupCustomerMaster();
                FillContractMaster();
            }
            catch (Exception ex)
            {
                loanAppGlobals.SaveError(ex);
            }
        }
        protected void pgrFollowupNote_ItemCommand(object sender, EventArgs e)
        {
            try
            {
                //FillFollowdupCustomerMaster();
                //FillContractMaster();
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
                    GetFollowdupCustomer(Convert.ToInt32(((ListView)sender).DataKeys[e.Item.DataItemIndex].Value)); 
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
        protected void lvCustomerFollowedup_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            try
            {
                if (e.Item.ItemType == ListViewItemType.DataItem)
                {
                    loanCustomerFollowupDAL objCustomerFollowupDAL = (loanCustomerFollowupDAL)e.Item.DataItem;


                    Label lblCustomer = (Label)e.Item.FindControl("lblCustomer");
                    lblCustomer.Text = objCustomerFollowupDAL.Customer;

                    Label lblUsername = (Label)e.Item.FindControl("lblUsername");
                    lblUsername.Text = objCustomerFollowupDAL.Username.ToString();

                    Label lblCustomerIdNo = (Label)e.Item.FindControl("lblCustomerIdNo");
                    lblCustomerIdNo.Text = objCustomerFollowupDAL.CustomerIdNo.ToString();

                    Label lblMobile1 = (Label)e.Item.FindControl("lblMobile1");
                    lblMobile1.Text = objCustomerFollowupDAL.Mobile1 ?? objCustomerFollowupDAL.Mobile2;

                    Label lblMobile2 = (Label)e.Item.FindControl("lblMobile2");
                    lblMobile2.Text = objCustomerFollowupDAL.Mobile2 ?? objCustomerFollowupDAL.Mobile2;

                    Label lblMobile3 = (Label)e.Item.FindControl("lblMobile3");
                    lblMobile3.Text = objCustomerFollowupDAL.Mobile3 ?? objCustomerFollowupDAL.Mobile2;

                 

                    HiddenField lblCustomerFollowupId = (HiddenField)e.Item.FindControl("lblCustomerFollowupId");
                    lblCustomerFollowupId.Value = objCustomerFollowupDAL.CustomerFollowupId.ToString();




                    Label lblContractsCount  = (Label)e.Item.FindControl("lblContractsCount");
                    lblContractsCount.Text = objCustomerFollowupDAL.ContractsCount.ToString();



                    Label lblTotalAmount = (Label)e.Item.FindControl("lblTotalAmount");
                    lblTotalAmount.Text = objCustomerFollowupDAL.TotalAmount.ToString();



                    Label lblTotalRemains = (Label)e.Item.FindControl("lblTotalRemains");
                    lblTotalRemains.Text = objCustomerFollowupDAL.TotalRemains.ToString();




                }
            }
            catch (Exception ex)
            {


            }
        }
        public Dictionary<int, List<loanContractMasterDAL>> contracts = new Dictionary<int, List<loanContractMasterDAL>>();
        public List<loanContractMasterDAL> getContractDataSource(object idObj)
        {
            if (contracts.Count == 0)
            {
                FillContractMaster();
            }

            var data = new List<loanContractMasterDAL>();
            var id = Convert.ToInt32(idObj);
            contracts.TryGetValue(id, out data);

            return data;
        } 
        public List<loanFollowupNoteDAL> getFollowUpNoteDataSource(object idObj)
        {
            loanFollowupNoteDAL objFollowupNoteDAL = new loanFollowupNoteDAL();
            int TotalRecords;
            objFollowupNoteDAL.linktoCustomerFollowupId = Convert.ToInt32(idObj);
            List<loanFollowupNoteDAL> lstFollowupNote =
                objFollowupNoteDAL.SelectFollowupNote(out TotalRecords);
             
            return lstFollowupNote;
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
                pgrFollowupMaster.CurrentPage = Convert.ToInt16(loanSessionsDAL.GetSessionKeyValue("CurrentPageContract"));
            }
            if (loanSessionsDAL.GetSessionKeyValue("FilterContract") != null)
            {
                loanContractMasterDAL objContractMasterDAL = (loanContractMasterDAL)loanSessionsDAL.GetSessionKeyValue("FilterContract");
                txtFilterCustomer.Text = objContractMasterDAL.Customer;


                ddlFilterContractStatus.SelectedValue = objContractMasterDAL.linktoContractStatusMasterId.ToString();

            }
        }

        private void GetFollowdupCustomer(int FollowupCustomerId)
        {
            loanUser.CheckRoleRights(loanRoleRights.ViewRecord);  
            loanCustomerFollowupDAL objCustomerFollowupDAL = new loanCustomerFollowupDAL();
            objCustomerFollowupDAL.CustomerFollowupId = FollowupCustomerId;
            if (!objCustomerFollowupDAL.SelectFollowupCustomer())
            {
                loanAppGlobals.ShowMessage(loanMessagesDAL.SelectFail, loanMessageIcon.Error);
                return;
            }
            hdnCustomerFollowupId.Value =  FollowupCustomerId.ToString();
            hdnCustomerMasterId.Value = objCustomerFollowupDAL.linktoCustomerMasterId.ToString();
            txtCustomerName.Text = objCustomerFollowupDAL.Customer;
            txtIdNo.Text = objCustomerFollowupDAL.CustomerIdNo;
            txtSearchCustomerIdNo.Text = objCustomerFollowupDAL.CustomerIdNo;  
            ddlAuditors.SelectedValue = objCustomerFollowupDAL.linktoUserMasterId.ToString();

            hdnModelFollowup.Value = "show";
            hdnActionFollowup.Value = "edit";

        }

        private void FillFollowdupCustomerMaster()
        {
            loanCustomerFollowupDAL objCustomerFollowup = new loanCustomerFollowupDAL();
            string OrderBy = null;
            string OrderDir = null;


            if (ddlSortBy.SelectedValue != string.Empty)
            {
                OrderBy = ddlSortBy.SelectedValue.Split(',')[0];
                OrderDir = ddlSortBy.SelectedValue.Split(',')[1];
            }

            if (ddlAuditors.SelectedValue != string.Empty)
            {
                objCustomerFollowup.linktoUserMasterId = Convert.ToInt32(ddlAuditors.SelectedValue); 
            }


            if (ddlFilterUsername.SelectedValue != string.Empty)
            {
                objCustomerFollowup.linktoUserMasterId = Convert.ToInt32(ddlFilterUsername.SelectedValue);
            }




            if (!canCollectContracts())
            {
                objCustomerFollowup.linktoUserMasterId = ((loanUser)HttpContext.Current.Session[loanSessionsDAL.UserSession]).UserMasterId;
            }



            int TotalRecords;
            List<loanCustomerFollowupDAL> lstCustomerFollowup = objCustomerFollowup.SelectAllCustomerFollowupsPageWise
                (pgrFollowupMaster.StartRowIndex, pgrFollowupMaster.PageSize,
                out TotalRecords, orderBy: OrderBy, orderDir: OrderDir);

            pgrFollowupMaster.TotalRowCount = TotalRecords;

            lvCustomerFollowedup.DataSource = lstCustomerFollowup;
            lvCustomerFollowedup.DataBind();


            if (lstCustomerFollowup.Count == 0 && pgrFollowupMaster.TotalRowCount > 0)
            {
                pgrFollowupMaster_ItemCommand(pgrFollowupMaster, new EventArgs());
                return;
            }



            if (lstCustomerFollowup.Count > 0)
            {
                int EndiIndex = pgrFollowupMaster.StartRowIndex + pgrFollowupMaster.PageSize < pgrFollowupMaster.TotalRowCount ? pgrFollowupMaster.StartRowIndex + pgrFollowupMaster.PageSize : pgrFollowupMaster.TotalRowCount;
                lblRecords.Text = string.Format(Resources.Messages.ResourceManager.GetString("Records"), pgrFollowupMaster.StartRowIndex + 1, EndiIndex, pgrFollowupMaster.TotalRowCount);
                lblRecords.Visible = true;
            }
            else
            {
                lblRecords.Visible = false;
            }

            if (pgrFollowupMaster.TotalRowCount <= pgrFollowupMaster.PageSize)
            {
                pgrFollowupMaster.Visible = false;
            }
            else
            {
                pgrFollowupMaster.Visible = true;
            }


        }
        private void FillContractMaster()
        {

            loanContractMasterDAL objContractMasterDAL = new loanContractMasterDAL();
            string IdNo = txtFilterCustomer.Text.Trim();


            if (!canCollectContracts())
            {
                objContractMasterDAL.linktoUserMasterId = ((loanUser)HttpContext.Current.Session[loanSessionsDAL.UserSession]).UserMasterId;
            }

            if (IdNo.Contains("("))
            {
                IdNo = IdNo.Substring(IdNo.IndexOf("(") + 1, IdNo.IndexOf(")") - IdNo.IndexOf("(") - 1);
            }
            objContractMasterDAL.Customer = IdNo;



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







            loanSessionsDAL.SetSessionKeyValue("FilterContract", objContractMasterDAL);
            //loanSessionsDAL.SetSessionKeyValue("CurrentPageContract", pgrContractMaster.CurrentPage);

            string OrderBy = null;
            string OrderDir = null;
            if (ddlSortBy.SelectedValue != string.Empty)
            {
                OrderBy = ddlSortBy.SelectedValue.Split(',')[0];
                OrderDir = ddlSortBy.SelectedValue.Split(',')[1];
            }

            int TotalRecords;
            List<loanContractMasterDAL> lstContractMaster = objContractMasterDAL.SelectAllContractMasterByFollowupPageWise
                (0, 100, out TotalRecords, ContractDateFrom, ContractDateTo, orderBy: OrderBy, orderDir: OrderDir);




            var groupedContracts = lstContractMaster.GroupBy(contract => contract.linktoCustomerMasterId)
                                       .ToDictionary(group => group.Key, group => group.ToList());

            // Now, you can assign the groupedContracts to your contracts dictionary
            contracts = new Dictionary<int, List<loanContractMasterDAL>>(groupedContracts);




            //pgrContractMaster.TotalRowCount = TotalRecords;

            if (lstContractMaster == null)
            {
                loanAppGlobals.ShowMessage(loanMessagesDAL.SelectAllFail, loanMessageIcon.Error);
                return;
            }

            //if (lstContractMaster.Count == 0 && pgrContractMaster.TotalRowCount > 0)
            //{
            //    pgrContractMaster_ItemCommand(pgrContractMaster, new EventArgs());
            //    return;
            //}



            //if (lstContractMaster.Count > 0)
            //{
            //    int EndiIndex = pgrContractMaster.StartRowIndex + pgrContractMaster.PageSize < pgrContractMaster.TotalRowCount ? pgrContractMaster.StartRowIndex + pgrContractMaster.PageSize : pgrContractMaster.TotalRowCount;
            //    lblRecords.Text = string.Format(Resources.Messages.ResourceManager.GetString("Records"), pgrContractMaster.StartRowIndex + 1, EndiIndex, pgrContractMaster.TotalRowCount);
            //    lblRecords.Visible = true;
            //}
            //else
            //{
            //    lblRecords.Visible = false;
            //}

            //if (pgrContractMaster.TotalRowCount <= pgrContractMaster.PageSize)
            //{
            //    pgrContractMaster.Visible = false;
            //}
            //else
            //{
            //    pgrContractMaster.Visible = true;
            //}

            //  lvContractMaster.DataSource = contracts;
            //  lvContractMaster.DataBind = contracts;


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


        private void GetAuditors()
        {
            ddlAuditors.Items.Clear();
            ddlAuditors.Items.Add(new System.Web.UI.WebControls.ListItem(loanDropDownItem.Select, ""));

            ddlFilterUsername.Items.Clear();
            ddlFilterUsername.Items.Add(new System.Web.UI.WebControls.ListItem(loanDropDownItem.Select, ""));

            List<loanUserMasterDAL> lstUserMasterDAL = loanUserMasterDAL.SelectAllUserMaster();
            if (lstUserMasterDAL == null)
            {
                loanAppGlobals.ShowMessage(loanMessagesDAL.SelectAllFail, loanMessageIcon.Error);
                return;
            }
            foreach (loanUserMasterDAL obj in lstUserMasterDAL)
            {
                ddlAuditors.Items.Add(new System.Web.UI.WebControls.ListItem(obj.Username, obj.UserMasterId.ToString()));
                ddlFilterUsername.Items.Add(new System.Web.UI.WebControls.ListItem(obj.Username, obj.UserMasterId.ToString()));
            }
        }

        protected bool canCollectContracts()
        {

            // if Admin  
            if (loanUser.GetRoleRights(loanRoleRights.DeleteRecord, "Super"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        protected void btnClearList_Click(object sender, EventArgs e)
        {
            txtSearchCustomerIdNo.Enabled = true;
            hdnCustomerMasterId.Value = "";
            txtSearchCustomerIdNo.Text = "";
            txtIdNo.Text = "";
            txtCustomerName.Text = "";
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
                    //   btnClearList_Click(btnClearList, new EventArgs());
                    return;
                }
                hdnCustomerMasterId.Value = objCustomerMasterDAL.CustomerMasterId.ToString();
                txtIdNo.Text = IdNo;
                txtCustomerName.Text = objCustomerMasterDAL.CustomerName;
                // FillContractMaster();
                //if (sender != null)
                //{
                //    GetBankAccountNumber(objCustomerMasterDAL.CustomerMasterId);
                //}
            }
            catch (Exception ex)
            {
                loanAppGlobals.SaveError(ex);
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