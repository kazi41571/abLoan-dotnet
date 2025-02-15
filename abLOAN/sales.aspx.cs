using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using loanLibrary;
using System.IO;
using System.Linq;
using NumberToWord;
using System.Web;
using QRCoder;
using System.Drawing;

namespace abLOAN
{
    public partial class sales : BasePage
    {
        decimal totalNetAmount = 0, totalDiscount = 0, totalVat = 0, totalFees = 0, totalSales = 0;
        int totalQuantity;

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                loanSessionsDAL.CheckSession();

                if (!Page.IsPostBack)
                {
                    //loanUser.CheckUserRights(loanUserRights.ViewSales);
                    loanUser.CheckRoleRights(loanRoleRights.ViewList);

                    GetItem();

                    GetPageDefaults();

                    loanSessionsDAL.RemoveSessionAllKeyValue();
                    if (Request.QueryString.ToString().Contains("id="))
                    {
                        int id = Convert.ToInt32(Request.QueryString["id"].ToString());
                        GetSalesMaster(id);
                    }
                    FillSalesMaster();
                    FillSalesItems();
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
                pgrSalesMaster.CurrentPage = 1;
                FillSalesMaster();
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
                //if (loanGlobalsDAL.GetCurrentDateTime().Day < 25)
                //{
                //    txtFromDate.Text = loanGlobalsDAL.ConvertDateTimeToString(loanGlobalsDAL.GetCurrentDateTime().AddMonths(-1), loanAppGlobals.DateFormat);
                //    txtFromDate.Text = "25" + txtFromDate.Text.Substring(2);
                //    txtToDate.Text = loanGlobalsDAL.ConvertDateTimeToString(loanGlobalsDAL.GetCurrentDateTime(), loanAppGlobals.DateFormat);
                //    txtToDate.Text = "24" + txtToDate.Text.Substring(2);
                //}
                //else
                //{
                //    txtFromDate.Text = loanGlobalsDAL.ConvertDateTimeToString(loanGlobalsDAL.GetCurrentDateTime(), loanAppGlobals.DateFormat);
                //    txtFromDate.Text = "25" + txtFromDate.Text.Substring(2);
                //    txtToDate.Text = loanGlobalsDAL.ConvertDateTimeToString(loanGlobalsDAL.GetCurrentDateTime().AddMonths(1), loanAppGlobals.DateFormat);
                //    txtToDate.Text = "24" + txtToDate.Text.Substring(2);
                //}

                txtFromDate.Text = loanGlobalsDAL.ConvertDateTimeToString(loanGlobalsDAL.GetCurrentDateTime(), loanAppGlobals.DateFormat);
                txtFromDate.Text = "01" + txtFromDate.Text.Substring(2);
                txtToDate.Text = loanGlobalsDAL.ConvertDateTimeToString(loanGlobalsDAL.GetCurrentDateTime(), loanAppGlobals.DateFormat);
                txtToDate.Text = DateTime.DaysInMonth(loanGlobalsDAL.GetCurrentDateTime().Year, loanGlobalsDAL.GetCurrentDateTime().Month).ToString("00") + txtToDate.Text.Substring(2);

                txtFilterBillNo.Text = string.Empty;
                txtFilterReceiptNo.Text = string.Empty;
                txtFilterCustomer.Text = string.Empty;

                ddlFilterItem.SelectedIndex = 0;

                pgrSalesMaster.CurrentPage = 1;
                FillSalesMaster();
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
                loanSalesMasterDAL objSalesMasterDAL = new loanSalesMasterDAL();
                objSalesMasterDAL.linktoCompanyMasterId = ((loanUser)Session[loanSessionsDAL.UserSession]).CompanyMasterId;
                objSalesMasterDAL.CustomerName = txtCustomerName.Text.Trim();
                objSalesMasterDAL.CustomerIdNo = txtCustomerIdNo.Text.Trim();
                objSalesMasterDAL.CustomerAddress = txtCustomerAddress.Text.Trim();
                objSalesMasterDAL.SalesDate = DateTime.ParseExact(txtSalesDate.Text, loanAppGlobals.DateFormat, System.Globalization.DateTimeFormatInfo.InvariantInfo);
                objSalesMasterDAL.Notes = txtNotes.Text.Trim();
                if (txtContractStartDate.Text != "")
                {
                    objSalesMasterDAL.ContractStartDate = DateTime.ParseExact(txtContractStartDate.Text, loanAppGlobals.DateFormat, System.Globalization.DateTimeFormatInfo.InvariantInfo);
                }
                objSalesMasterDAL.ContractAmount = Convert.ToDecimal(txtContractAmount.Text);
                objSalesMasterDAL.InstallmentAmount = Convert.ToDecimal(txtInstallmentAmount.Text);
                objSalesMasterDAL.BillNo = txtBillNo.Text.Trim();
                objSalesMasterDAL.ReceiptNo = txtReceiptNo.Text.Trim();

                objSalesMasterDAL.UpdateDateTime = loanGlobalsDAL.GetCurrentDateTime();
                objSalesMasterDAL.SessionId = ((loanUser)Session[loanSessionsDAL.UserSession]).SessionId;

                List<loanSalesItemTranDAL> lstSalesItemTranDAL = (List<loanSalesItemTranDAL>)ViewState["lvSalesItems"];

                if (lstSalesItemTranDAL.Count == 0)
                {
                    loanAppGlobals.ShowMessage(loanMessagesDAL.NotFound, loanMessageIcon.Warning);
                    return;
                }

                if (string.IsNullOrEmpty(hdnActionSales.Value))
                {
                    loanUser.CheckRoleRights(loanRoleRights.AddRecord);

                    loanRecordStatus rsStatus = objSalesMasterDAL.InsertSalesMaster(lstSalesItemTranDAL);
                    if (rsStatus == loanRecordStatus.Error)
                    {
                        loanAppGlobals.ShowMessage(loanMessagesDAL.InsertFail, loanMessageIcon.Error);
                        return;
                    }
                    else if (rsStatus == loanRecordStatus.RecordAlreadyExist)
                    {
                        loanAppGlobals.ShowMessage(loanMessagesDAL.AlreadyExist, loanMessageIcon.Warning);
                        hdnModelSales.Value = "show";
                        return;
                    }
                    else if (rsStatus == loanRecordStatus.Success)
                    {
                        loanAppGlobals.ShowMessage(loanMessagesDAL.InsertSuccess, loanMessageIcon.Success);
                        if (((Button)sender).ID.Equals("btnSaveAndNew"))
                        {
                            hdnModelSales.Value = "clear";
                        }
                        else if (((Button)sender).ID.Equals("btnSaveAndPrint"))
                        {
                            PrintReceipt(objSalesMasterDAL.SalesMasterId);
                        }
                        else
                        {
                            hdnModelSales.Value = "hide";
                        }
                        btnClearList_Click(btnClearList, new EventArgs());
                        FillSalesMaster();
                    }
                }
                else
                {
                    loanUser.CheckRoleRights(loanRoleRights.EditRecord);

                    objSalesMasterDAL.SalesMasterId = Convert.ToInt32(hdnSalesMasterId.Value);
                    loanRecordStatus rsStatus = objSalesMasterDAL.UpdateSalesMaster(lstSalesItemTranDAL);
                    if (rsStatus == loanRecordStatus.Error)
                    {
                        loanAppGlobals.ShowMessage(loanMessagesDAL.UpdateFail, loanMessageIcon.Error);
                        return;
                    }
                    else if (rsStatus == loanRecordStatus.RecordAlreadyExist)
                    {
                        loanAppGlobals.ShowMessage(loanMessagesDAL.AlreadyExist, loanMessageIcon.Warning);
                        hdnModelSales.Value = "show";
                        return;
                    }
                    else if (rsStatus == loanRecordStatus.Success)
                    {
                        loanAppGlobals.ShowMessage(loanMessagesDAL.UpdateSuccess, loanMessageIcon.Success);
                        hdnModelSales.Value = "hide";
                        btnClearList_Click(btnClearList, new EventArgs());
                        FillSalesMaster();
                    }
                }
            }
            catch (Exception ex)
            {
                loanAppGlobals.SaveError(ex);
            }
        }

        protected void btnSaveAndPrint_Click(object sender, EventArgs e)
        {
            try
            {
                btnSave_Click(sender, e);

                PrintReceipt(Convert.ToInt32(hdnSalesMasterId.Value));
            }
            catch (Exception ex)
            {
                loanAppGlobals.SaveError(ex);
            }
        }

        protected void ddlItem_SelectedIndexChanged(object sender, EventArgs e)
        {
            loanItemMasterDAL objItemMasterDAL = new loanItemMasterDAL();
            if (ddlItem.SelectedIndex > 0)
            {
                objItemMasterDAL.ItemMasterId = Convert.ToInt32(ddlItem.SelectedValue);
                bool isSelected = objItemMasterDAL.SelectItemMaster();
                if (isSelected)
                {
                    txtSalesRate.Text = objItemMasterDAL.SalesPrice.ToString("0.00");
                    txtDiscountAmount.Text = "0.00";
                    txtVat.Text = objItemMasterDAL.Vat.ToString("0.00");
                    txtVatAmount.Text = ((objItemMasterDAL.SalesPrice * Convert.ToInt32(txtQuantity.Text)) * (Convert.ToDecimal(txtVat.Text) / 100)).ToString("0.00");
                    txtNetAmount.Text = ((objItemMasterDAL.SalesPrice * Convert.ToInt32(txtQuantity.Text)) + (objItemMasterDAL.SalesPrice * Convert.ToInt32(txtQuantity.Text)) * (Convert.ToDecimal(txtVat.Text) / 100)).ToString("0.00");
                }
            }
            else
            {
                txtQuantity.Text = "1";
                txtSalesRate.Text = "0.00";
                txtDiscountAmount.Text = "0.00";
                txtVat.Text = "0.00";
                txtVatAmount.Text = "0.00";
                txtFees.Text = "0.00";
                txtNetAmount.Text = "0.00";
            }
        }

        protected void btnClearList_Click(object sender, EventArgs e)
        {
            ViewState["lvSalesItems"] = new List<loanSalesItemTranDAL>();

            FillSalesItems();

            ddlItem.SelectedIndex = 0;
            ddlItem_SelectedIndexChanged(ddlItem, new EventArgs());
        }

        protected void btnDownload_Click(object sender, EventArgs e)
        {
            loanSalesMasterDAL objSalesMasterDAL = new loanSalesMasterDAL();
            if (!string.IsNullOrEmpty(txtFromDate.Text))
            {
                objSalesMasterDAL.FromDate = DateTime.ParseExact(txtFromDate.Text, loanAppGlobals.DateFormat, System.Globalization.DateTimeFormatInfo.InvariantInfo);
            }
            if (!string.IsNullOrEmpty(txtToDate.Text))
            {
                objSalesMasterDAL.ToDate = DateTime.ParseExact(txtToDate.Text, loanAppGlobals.DateFormat, System.Globalization.DateTimeFormatInfo.InvariantInfo);
            }
            int ItemMasterId = 0;
            if (ddlFilterItem.SelectedValue != string.Empty)
            {
                ItemMasterId = Convert.ToInt32(ddlFilterItem.SelectedValue);
            }
            objSalesMasterDAL.linktoCompanyMasterId = ((loanUser)Session[loanSessionsDAL.UserSession]).CompanyMasterId;
            if (ddlFilterVerification.SelectedValue == "Yes")
            {
                objSalesMasterDAL.IsVerified = true;
            }
            else if (ddlFilterVerification.SelectedValue == "No")
            {
                objSalesMasterDAL.IsVerified = false;
            }

            objSalesMasterDAL.BillNo = txtFilterBillNo.Text;
            objSalesMasterDAL.ReceiptNo = txtReceiptNo.Text;
            objSalesMasterDAL.CustomerName = txtFilterCustomer.Text;

            int TotalRecords;
            List<loanSalesMasterDAL> lstSalesMaster = objSalesMasterDAL.SelectAllSalesMasterPageWise(0, int.MaxValue, out TotalRecords, ItemMasterId);

            if (lstSalesMaster == null)
            {
                loanAppGlobals.ShowMessage(loanMessagesDAL.SelectAllFail, loanMessageIcon.Error);
                return;
            }

            List<loanSalesMasterPrint> lstSalesMasterPrint = new List<loanSalesMasterPrint>();
            loanSalesMasterPrint objSalesMasterPrint;

            lstSalesMaster.ForEach(f =>
            {
                //public string ReceiptNo { get; set; }
                //public DateTime UpdateDateTime { get; set; }
                //public string SessionId { get; set; }

                ///// Extra
                //public DateTime? FromDate { get; set; }
                //public DateTime? ToDate { get; set; }
                //public string TranType { get; set; }
                //public string VerifiedBy { get; set; }
                //public string ModifiedBy { get; set; }
                //public int linktoCategoryMasterId { get; set; }

                objSalesMasterPrint = new loanSalesMasterPrint();
                objSalesMasterPrint.SalesMasterId = f.SalesMasterId;
                objSalesMasterPrint.linktoCompanyMasterId = f.linktoCompanyMasterId;
                objSalesMasterPrint.CustomerName = f.CustomerName;
                objSalesMasterPrint.CustomerIdNo = f.CustomerIdNo;
                objSalesMasterPrint.CustomerAddress = f.CustomerAddress;
                objSalesMasterPrint.SalesDate = f.SalesDate;
                objSalesMasterPrint.Notes = f.Notes;
                objSalesMasterPrint.IsVerified = f.IsVerified;
                objSalesMasterPrint.VerifiedDateTime = f.VerifiedDateTime;
                objSalesMasterPrint.ContractStartDate = f.ContractStartDate;
                objSalesMasterPrint.ContractAmount = f.ContractAmount;
                objSalesMasterPrint.InstallmentAmount = f.InstallmentAmount;
                objSalesMasterPrint.BillNo = f.BillNo;
                objSalesMasterPrint.ReceiptNo = f.ReceiptNo;
                objSalesMasterPrint.UpdateDateTime = f.UpdateDateTime;
                objSalesMasterPrint.SessionId = f.SessionId;
                objSalesMasterPrint.FromDate = f.FromDate;
                objSalesMasterPrint.ToDate = f.ToDate;
                objSalesMasterPrint.TranType = f.TranType;
                objSalesMasterPrint.VerifiedBy = f.VerifiedBy;
                objSalesMasterPrint.ModifiedBy = f.ModifiedBy;
                objSalesMasterPrint.linktoCategoryMasterId = f.linktoCategoryMasterId;

                bool IsNew = false;
                f.lstSalesItemTranDAL.ForEach(a =>
                {
                    if (IsNew)
                    {
                        objSalesMasterPrint = new loanSalesMasterPrint();
                    }

                    objSalesMasterPrint.SalesItemTranId = a.SalesItemTranId;
                    objSalesMasterPrint.linktoSalesMasterId = a.linktoSalesMasterId;
                    objSalesMasterPrint.linktoItemMasterId = a.linktoItemMasterId;
                    objSalesMasterPrint.Quantity = a.Quantity;
                    objSalesMasterPrint.SalesRate = a.SalesRate;
                    objSalesMasterPrint.GrossAmount = a.SalesRate * a.Quantity;
                    objSalesMasterPrint.DiscountAmount = a.DiscountAmount;
                    objSalesMasterPrint.Vat = a.Vat;
                    objSalesMasterPrint.VatAmount = a.VatAmount;
                    objSalesMasterPrint.Fees = a.Fees;
                    objSalesMasterPrint.NetAmount = a.NetAmount;
                    objSalesMasterPrint.Item = a.Item;
                    objSalesMasterPrint.ItemCode = a.ItemCode;

                    lstSalesMasterPrint.Add(objSalesMasterPrint);
                    IsNew = true;
                });
            });

            string file = System.Configuration.ConfigurationManager.AppSettings["FileSavePath"] + "Sales.csv";

            string[] headers = { "Customer Name", "Sales Date", "Item", "Quantity", "Sales Rate", "Gross Amount", "Discount Amount", "Vat Amount", "Net Amount", "Fees", "Notes", "Bill No", "Receipt No", "Installment Start", "Contract Amount", "Installment Amount", "Verifier", "Modifier" };
            string[] columns = { "CustomerName", "SalesDate", "Item", "Quantity", "SalesRate", "GrossAmount", "DiscountAmount", "VatAmount", "NetAmount", "Fees", "Notes", "BillNo", "ReceiptNo", "ContractStartDate", "ContractAmount", "InstallmentAmount", "VerifiedBy", "ModifiedBy" };

            bool IsSuccess = loanAppGlobals.ExportCsv(lstSalesMasterPrint, headers, columns, file);
            if (!IsSuccess)
            {
                loanAppGlobals.ShowMessage(loanMessagesDAL.Exception, loanMessageIcon.Error);
                return;
            }

            loanAppGlobals.SendOutFile(file, Path.GetFileName(file));
        }

        #region List Methods

        protected void lvSalesMaster_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            try
            {
                if (e.Item.ItemType == ListViewItemType.DataItem)
                {
                    loanSalesMasterDAL objSalesMasterDAL = (loanSalesMasterDAL)e.Item.DataItem;

                    Literal ltrlSalesDate = (Literal)e.Item.FindControl("ltrlSalesDate");
                    Literal ltrlCustomerName = (Literal)e.Item.FindControl("ltrlCustomerName");
                    Literal ltrlNotes = (Literal)e.Item.FindControl("ltrlNotes");
                    Literal ltrlBillNo = (Literal)e.Item.FindControl("ltrlBillNo");
                    Literal ltrlReceiptNo = (Literal)e.Item.FindControl("ltrlReceiptNo");
                    Literal ltrlContractStartDate = (Literal)e.Item.FindControl("ltrlContractStartDate");
                    Literal ltrlContractAmount = (Literal)e.Item.FindControl("ltrlContractAmount");
                    Literal ltrlInstallmentAmount = (Literal)e.Item.FindControl("ltrlInstallmentAmount");
                    ListView lvSalesItemTran = (ListView)e.Item.FindControl("lvSalesItemTran");

                    Literal ltrlModifiedBy = (Literal)e.Item.FindControl("ltrlModifiedBy");
                    ltrlModifiedBy.Text = objSalesMasterDAL.ModifiedBy;
                    Literal ltrlModifiedDateTime = (Literal)e.Item.FindControl("ltrlModifiedDateTime");
                    ltrlModifiedDateTime.Text = loanGlobalsDAL.ConvertDateTimeToString(objSalesMasterDAL.UpdateDateTime, loanAppGlobals.DateTimeFormat);

                    ltrlSalesDate.Text = loanGlobalsDAL.ConvertDateTimeToString(objSalesMasterDAL.SalesDate, loanAppGlobals.DateFormat);
                    ltrlCustomerName.Text = objSalesMasterDAL.CustomerName;
                    ltrlNotes.Text = objSalesMasterDAL.Notes;
                    ltrlBillNo.Text = objSalesMasterDAL.BillNo;
                    ltrlReceiptNo.Text = objSalesMasterDAL.ReceiptNo;
                    if (objSalesMasterDAL.ContractStartDate != null)
                    {
                        ltrlContractStartDate.Text = loanGlobalsDAL.ConvertDateTimeToString(objSalesMasterDAL.ContractStartDate, loanAppGlobals.DateFormat);
                    }
                    if (objSalesMasterDAL.ContractAmount != null)
                    {
                        ltrlContractAmount.Text = objSalesMasterDAL.ContractAmount.Value.ToString("0.00");
                    }
                    if (objSalesMasterDAL.InstallmentAmount != null)
                    {
                        ltrlInstallmentAmount.Text = objSalesMasterDAL.InstallmentAmount.Value.ToString("0.00");
                    }

                    lvSalesItemTran.DataSource = objSalesMasterDAL.lstSalesItemTranDAL;
                    lvSalesItemTran.DataBind();

                    if (objSalesMasterDAL.IsVerified != null)
                    {
                        LinkButton lbtnVerify = (LinkButton)e.Item.FindControl("lbtnVerify");
                        lbtnVerify.Visible = false;
                        Literal ltrlVerifiedBy = (Literal)e.Item.FindControl("ltrlVerifiedBy");
                        ltrlVerifiedBy.Text = objSalesMasterDAL.VerifiedBy;
                        if (objSalesMasterDAL.VerifiedDateTime != null)
                        {
                            Literal ltrlVerifiedDateTime = (Literal)e.Item.FindControl("ltrlVerifiedDateTime");
                            ltrlVerifiedDateTime.Text = loanGlobalsDAL.ConvertDateTimeToString(objSalesMasterDAL.VerifiedDateTime.Value, loanAppGlobals.DateTimeFormat);
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

        protected void pgrSalesMaster_ItemCommand(object sender, EventArgs e)
        {
            try
            {
                FillSalesMaster();
            }
            catch (Exception ex)
            {
                loanAppGlobals.SaveError(ex);
            }
        }

        protected void lvSalesMaster_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName.Equals("EditRecord", StringComparison.CurrentCultureIgnoreCase))
                {
                    GetSalesMaster(Convert.ToInt32(((ListView)sender).DataKeys[e.Item.DataItemIndex].Value));
                }
                else if (e.CommandName.Equals("VerifyRecord", StringComparison.CurrentCultureIgnoreCase))
                {
                    string pageName = Path.GetFileNameWithoutExtension(Page.AppRelativeVirtualPath) + "master";
                    VerifyRecord(pageName, Convert.ToInt32(((ListView)sender).DataKeys[e.Item.DataItemIndex].Value));
                }
                else if (e.CommandName.Equals("DeleteRecord", StringComparison.CurrentCultureIgnoreCase))
                {
                    loanUser.CheckRoleRights(loanRoleRights.DeleteRecord);

                    loanSalesMasterDAL objSalesMasterDAL = new loanSalesMasterDAL();
                    objSalesMasterDAL.SalesMasterId = Convert.ToInt32(((ListView)sender).DataKeys[e.Item.DataItemIndex].Value);
                    objSalesMasterDAL.SessionId = ((loanUser)Session[loanSessionsDAL.UserSession]).SessionId;

                    loanRecordStatus rsStatus = objSalesMasterDAL.DeleteSalesMaster();
                    if (rsStatus == loanRecordStatus.Success)
                    {
                        loanAppGlobals.ShowMessage(loanMessagesDAL.DeleteSuccess, loanMessageIcon.Success);
                        FillSalesMaster();
                    }
                    else
                    {
                        loanAppGlobals.ShowMessage(loanMessagesDAL.DeleteFail, loanMessageIcon.Error);
                    }
                }
                else if (e.CommandName.Equals("PrintReceipt", StringComparison.CurrentCultureIgnoreCase))
                {
                    PrintReceipt(Convert.ToInt32(((ListView)sender).DataKeys[e.Item.DataItemIndex].Value));
                }
            }
            catch (Exception ex)
            {
                loanAppGlobals.SaveError(ex);
            }
        }

        protected void lvSalesMaster_DataBound(object sender, EventArgs e)
        {
            if (((ListView)sender).Items.Count > 0)
            {
                Literal ltrlTotalQuantity = (Literal)((ListView)sender).FindControl("ltrlTotalQuantity");
                ltrlTotalQuantity.Text = totalQuantity.ToString("0.00");
                Literal ltrlTotalSales = (Literal)((ListView)sender).FindControl("ltrlTotalGrossAmount");
                ltrlTotalSales.Text = totalSales.ToString("0.00");
                Literal ltrlTotalDiscountAmount = (Literal)((ListView)sender).FindControl("ltrlTotalDiscountAmount");
                ltrlTotalDiscountAmount.Text = totalDiscount.ToString("0.00");
                Literal ltrlTotalNetAmount = (Literal)((ListView)sender).FindControl("ltrlTotalNetAmount");
                ltrlTotalNetAmount.Text = totalNetAmount.ToString("0.00");

                Literal ltrlTotalVat = (Literal)((ListView)sender).FindControl("ltrlTotalVat");
                ltrlTotalVat.Text = totalVat.ToString("0.00");
                Literal ltrlTotalFees = (Literal)((ListView)sender).FindControl("ltrlTotalFees");
                ltrlTotalFees.Text = totalFees.ToString("0.00");
            }
        }

        protected void lvSalesItemTran_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            try
            {
                if (e.Item.ItemType == ListViewItemType.DataItem)
                {
                    loanSalesItemTranDAL objSalesItemTranDAL = (loanSalesItemTranDAL)e.Item.DataItem;

                    Literal ltrlItem = (Literal)e.Item.FindControl("ltrlItem");
                    Literal ltrlQuantity = (Literal)e.Item.FindControl("ltrlQuantity");
                    Literal ltrlSalesRate = (Literal)e.Item.FindControl("ltrlSalesRate");
                    Literal ltrlGrossAmount = (Literal)e.Item.FindControl("ltrlGrossAmount");
                    Literal ltrlDiscountAmount = (Literal)e.Item.FindControl("ltrlDiscountAmount");
                    Literal ltrlVat = (Literal)e.Item.FindControl("ltrlVat");
                    Literal ltrlFees = (Literal)e.Item.FindControl("ltrlFees");
                    Literal ltrlNetAmount = (Literal)e.Item.FindControl("ltrlNetAmount");

                    ltrlItem.Text = objSalesItemTranDAL.Item;
                    ltrlQuantity.Text = objSalesItemTranDAL.Quantity.ToString();
                    ltrlSalesRate.Text = objSalesItemTranDAL.SalesRate.ToString("0.00");
                    ltrlGrossAmount.Text = (objSalesItemTranDAL.SalesRate * objSalesItemTranDAL.Quantity).ToString("0.00");
                    ltrlDiscountAmount.Text = objSalesItemTranDAL.DiscountAmount.ToString("0.00");
                    ltrlVat.Text = (objSalesItemTranDAL.VatAmount).ToString("0.00");
                    ltrlFees.Text = objSalesItemTranDAL.Fees.ToString("0.00");
                    //ltrlFees.Text = (((objSalesItemTranDAL.SalesRate * objSalesItemTranDAL.Quantity * objSalesItemTranDAL.Vat) / 100) + objSalesItemTranDAL.Fees).ToString("0.00");
                    ltrlNetAmount.Text = objSalesItemTranDAL.NetAmount.ToString("0.00");

                    totalQuantity += Convert.ToInt32(ltrlQuantity.Text);
                    totalSales += Convert.ToDecimal(ltrlGrossAmount.Text);
                    totalDiscount += Convert.ToDecimal(ltrlDiscountAmount.Text);
                    totalVat += Convert.ToDecimal(ltrlVat.Text);
                    totalFees += Convert.ToDecimal(ltrlFees.Text);
                    totalNetAmount += Convert.ToDecimal(objSalesItemTranDAL.NetAmount);
                }
            }
            catch (Exception ex)
            {
                loanAppGlobals.SaveError(ex);
            }
        }

        protected void lvSalesItems_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            try
            {
                if (e.Item.ItemType == ListViewItemType.DataItem)
                {
                    loanSalesItemTranDAL objSalesItemTranDAL = (loanSalesItemTranDAL)e.Item.DataItem;

                    Literal ltrlItem = (Literal)e.Item.FindControl("ltrlItem");
                    Literal ltrlQuantity = (Literal)e.Item.FindControl("ltrlQuantity");
                    Literal ltrlSalesRate = (Literal)e.Item.FindControl("ltrlSalesRate");
                    Literal ltrlGrossAmount = (Literal)e.Item.FindControl("ltrlGrossAmount");
                    Literal ltrlDiscountAmount = (Literal)e.Item.FindControl("ltrlDiscountAmount");
                    Literal ltrlVat = (Literal)e.Item.FindControl("ltrlVat");
                    Literal ltrlFees = (Literal)e.Item.FindControl("ltrlFees");
                    Literal ltrlNetAmount = (Literal)e.Item.FindControl("ltrlNetAmount");

                    ltrlItem.Text = objSalesItemTranDAL.Item;
                    ltrlQuantity.Text = objSalesItemTranDAL.Quantity.ToString();
                    ltrlSalesRate.Text = objSalesItemTranDAL.SalesRate.ToString("0.00");
                    ltrlGrossAmount.Text = (objSalesItemTranDAL.SalesRate * objSalesItemTranDAL.Quantity).ToString("0.00");
                    ltrlDiscountAmount.Text = objSalesItemTranDAL.DiscountAmount.ToString("0.00");
                    ltrlVat.Text = (objSalesItemTranDAL.VatAmount).ToString("0.00");
                    ltrlFees.Text = objSalesItemTranDAL.Fees.ToString("0.00");
                    //ltrlFees.Text = (((objSalesItemTranDAL.SalesRate * objSalesItemTranDAL.Quantity * objSalesItemTranDAL.Vat) / 100) + objSalesItemTranDAL.Fees).ToString("0.00");
                    ltrlNetAmount.Text = objSalesItemTranDAL.NetAmount.ToString("0.00");

                    totalQuantity += objSalesItemTranDAL.Quantity;
                    totalSales += Convert.ToDecimal(ltrlGrossAmount.Text);
                    totalDiscount += Convert.ToDecimal(ltrlDiscountAmount.Text);
                    totalVat += Convert.ToDecimal(ltrlVat.Text);
                    totalFees += objSalesItemTranDAL.Fees;
                    totalNetAmount += objSalesItemTranDAL.NetAmount;
                }
            }
            catch (Exception ex)
            {
                loanAppGlobals.SaveError(ex);
            }
        }

        protected void lvSalesItems_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName.Equals("DeleteRecord", StringComparison.CurrentCultureIgnoreCase))
                {
                    List<loanSalesItemTranDAL> lstSalesItems = (List<loanSalesItemTranDAL>)ViewState["lvSalesItems"];

                    lstSalesItems.RemoveAt(e.Item.DataItemIndex);

                    ViewState["lvSalesItems"] = lstSalesItems;

                    FillSalesItems();
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
            //if (loanGlobalsDAL.GetCurrentDateTime().Day < 25)
            //{
            //    txtFromDate.Text = loanGlobalsDAL.ConvertDateTimeToString(loanGlobalsDAL.GetCurrentDateTime().AddMonths(-1), loanAppGlobals.DateFormat);
            //    txtFromDate.Text = "25" + txtFromDate.Text.Substring(2);
            //    txtToDate.Text = loanGlobalsDAL.ConvertDateTimeToString(loanGlobalsDAL.GetCurrentDateTime(), loanAppGlobals.DateFormat);
            //    txtToDate.Text = "24" + txtToDate.Text.Substring(2);
            //}
            //else
            //{
            //    txtFromDate.Text = loanGlobalsDAL.ConvertDateTimeToString(loanGlobalsDAL.GetCurrentDateTime(), loanAppGlobals.DateFormat);
            //    txtFromDate.Text = "25" + txtFromDate.Text.Substring(2);
            //    txtToDate.Text = loanGlobalsDAL.ConvertDateTimeToString(loanGlobalsDAL.GetCurrentDateTime().AddMonths(1), loanAppGlobals.DateFormat);
            //    txtToDate.Text = "24" + txtToDate.Text.Substring(2);
            //}

            txtFromDate.Text = loanGlobalsDAL.ConvertDateTimeToString(loanGlobalsDAL.GetCurrentDateTime(), loanAppGlobals.DateFormat);
            txtFromDate.Text = "01" + txtFromDate.Text.Substring(2);
            txtToDate.Text = loanGlobalsDAL.ConvertDateTimeToString(loanGlobalsDAL.GetCurrentDateTime(), loanAppGlobals.DateFormat);
            txtToDate.Text = DateTime.DaysInMonth(loanGlobalsDAL.GetCurrentDateTime().Year, loanGlobalsDAL.GetCurrentDateTime().Month).ToString("00") + txtToDate.Text.Substring(2);

            if (loanSessionsDAL.GetSessionKeyValue("CurrentPageSales") != null)
            {
                pgrSalesMaster.CurrentPage = Convert.ToInt16(loanSessionsDAL.GetSessionKeyValue("CurrentPageSales"));
            }
            if (loanSessionsDAL.GetSessionKeyValue("FilterSales") != null)
            {
                loanSalesMasterDAL objSalesMasterDAL = (loanSalesMasterDAL)loanSessionsDAL.GetSessionKeyValue("FilterSales");

                //ddlFilterItem.SelectedValue = objSalesMasterDAL.linktoItemMasterId.ToString();
            }

            ViewState["lvSalesItems"] = new List<loanSalesItemTranDAL>();
        }

        private void FillSalesMaster()
        {
            loanSalesMasterDAL objSalesMasterDAL = new loanSalesMasterDAL();
            if (!string.IsNullOrEmpty(txtFromDate.Text))
            {
                objSalesMasterDAL.FromDate = DateTime.ParseExact(txtFromDate.Text, loanAppGlobals.DateFormat, System.Globalization.DateTimeFormatInfo.InvariantInfo);
            }
            if (!string.IsNullOrEmpty(txtToDate.Text))
            {
                objSalesMasterDAL.ToDate = DateTime.ParseExact(txtToDate.Text, loanAppGlobals.DateFormat, System.Globalization.DateTimeFormatInfo.InvariantInfo);
            }
            int ItemMasterId = 0;
            if (ddlFilterItem.SelectedValue != string.Empty)
            {
                ItemMasterId = Convert.ToInt32(ddlFilterItem.SelectedValue);
            }
            objSalesMasterDAL.linktoCompanyMasterId = ((loanUser)Session[loanSessionsDAL.UserSession]).CompanyMasterId;
            if (ddlFilterVerification.SelectedValue == "Yes")
            {
                objSalesMasterDAL.IsVerified = true;
            }
            else if (ddlFilterVerification.SelectedValue == "No")
            {
                objSalesMasterDAL.IsVerified = false;
            }

            objSalesMasterDAL.BillNo = txtFilterBillNo.Text;
            objSalesMasterDAL.ReceiptNo = txtReceiptNo.Text;
            objSalesMasterDAL.CustomerName = txtFilterCustomer.Text;

            loanSessionsDAL.SetSessionKeyValue("FilterSales", objSalesMasterDAL);
            loanSessionsDAL.SetSessionKeyValue("CurrentPageSales", pgrSalesMaster.CurrentPage);

            int TotalRecords;
            List<loanSalesMasterDAL> lstSalesMaster = objSalesMasterDAL.SelectAllSalesMasterPageWise(pgrSalesMaster.StartRowIndex, pgrSalesMaster.PageSize, out TotalRecords, ItemMasterId);
            pgrSalesMaster.TotalRowCount = TotalRecords;

            if (lstSalesMaster == null)
            {
                loanAppGlobals.ShowMessage(loanMessagesDAL.SelectAllFail, loanMessageIcon.Error);
                return;
            }

            if (lstSalesMaster.Count == 0 && pgrSalesMaster.TotalRowCount > 0)
            {
                pgrSalesMaster_ItemCommand(pgrSalesMaster, new EventArgs());
                return;
            }

            lvSalesMaster.DataSource = lstSalesMaster;
            lvSalesMaster.DataBind();

            if (lstSalesMaster.Count > 0)
            {
                int EndiIndex = pgrSalesMaster.StartRowIndex + pgrSalesMaster.PageSize < pgrSalesMaster.TotalRowCount ? pgrSalesMaster.StartRowIndex + pgrSalesMaster.PageSize : pgrSalesMaster.TotalRowCount;
                lblRecords.Text = string.Format(Resources.Messages.ResourceManager.GetString("Records"), pgrSalesMaster.StartRowIndex + 1, EndiIndex, pgrSalesMaster.TotalRowCount);
                lblRecords.Visible = true;
            }
            else
            {
                lblRecords.Visible = false;
            }

            if (pgrSalesMaster.TotalRowCount <= pgrSalesMaster.PageSize)
            {
                pgrSalesMaster.Visible = false;
            }
            else
            {
                pgrSalesMaster.Visible = true;
            }

        }

        private void FillSalesItems()
        {
            List<loanSalesItemTranDAL> lstSalesItems = (List<loanSalesItemTranDAL>)ViewState["lvSalesItems"];

            lvSalesItems.DataSource = lstSalesItems;
            lvSalesItems.DataBind();
        }

        protected void lbtnAdd_Click(object sender, EventArgs e)
        {
            List<loanSalesItemTranDAL> lstSalesItems = (List<loanSalesItemTranDAL>)ViewState["lvSalesItems"];

            if (ddlItem.SelectedIndex == 0)
            {
                loanAppGlobals.ShowMessage(loanMessagesDAL.NotFound, loanMessageIcon.Warning);
                return;
            }
            if (lstSalesItems.Find(f => f.linktoItemMasterId == Convert.ToInt32(ddlItem.SelectedValue)) != null)
            {
                loanAppGlobals.ShowMessage(loanMessagesDAL.AlreadyExist, loanMessageIcon.Warning);
                return;
            }

            bool IsStockAvailable = GetItemStock(Convert.ToInt32(ddlItem.SelectedValue));
            if (IsStockAvailable == false)
            {
                loanAppGlobals.ShowMessage(loanMessagesDAL.NotFound, loanMessageIcon.Warning);
                return;
            }

            loanSalesItemTranDAL objSalesItemTranDAL = new loanSalesItemTranDAL();
            objSalesItemTranDAL.linktoItemMasterId = Convert.ToInt32(ddlItem.SelectedValue);
            objSalesItemTranDAL.Item = ddlItem.SelectedItem.Text;
            objSalesItemTranDAL.Quantity = Convert.ToInt32(txtQuantity.Text);
            objSalesItemTranDAL.SalesRate = Convert.ToDecimal(txtSalesRate.Text);
            objSalesItemTranDAL.DiscountAmount = Convert.ToDecimal(txtDiscountAmount.Text);
            objSalesItemTranDAL.Vat = Convert.ToDecimal(txtVat.Text);
            objSalesItemTranDAL.VatAmount = Convert.ToDecimal(txtVatAmount.Text);
            objSalesItemTranDAL.Fees = Convert.ToDecimal(txtFees.Text);
            objSalesItemTranDAL.NetAmount = Convert.ToDecimal(txtNetAmount.Text);

            lstSalesItems.Add(objSalesItemTranDAL);

            ViewState["lvSalesItems"] = lstSalesItems;

            FillSalesItems();

            ddlItem.SelectedIndex = 0;
            ddlItem_SelectedIndexChanged(ddlItem, new EventArgs());
        }

        private void GetItem()
        {
            ddlItem.Items.Clear();
            ddlItem.Items.Add(new System.Web.UI.WebControls.ListItem(loanDropDownItem.Select, ""));

            ddlFilterItem.Items.Clear();
            ddlFilterItem.Items.Add(new System.Web.UI.WebControls.ListItem(loanDropDownItem.Select, ""));

            List<loanItemMasterDAL> lstItemMasterDAL = loanItemMasterDAL.SelectAllItemMasterItemNameBrand();
            if (lstItemMasterDAL == null)
            {
                loanAppGlobals.ShowMessage(loanMessagesDAL.SelectAllFail, loanMessageIcon.Error);
                return;
            }
            foreach (loanItemMasterDAL obj in lstItemMasterDAL)
            {
                ddlItem.Items.Add(new System.Web.UI.WebControls.ListItem(obj.ItemName, obj.ItemMasterId.ToString()));
                ddlFilterItem.Items.Add(new System.Web.UI.WebControls.ListItem(obj.ItemName, obj.ItemMasterId.ToString()));
            }
        }

        private void GetSalesMaster(int SalesMasterId)
        {
            loanUser.CheckRoleRights(loanRoleRights.ViewRecord);

            loanSalesMasterDAL objSalesMasterDAL = new loanSalesMasterDAL();
            objSalesMasterDAL.SalesMasterId = SalesMasterId;
            if (!objSalesMasterDAL.SelectSalesMaster())
            {
                loanAppGlobals.ShowMessage(loanMessagesDAL.SelectFail, loanMessageIcon.Error);
                return;
            }
            txtCustomerName.Text = objSalesMasterDAL.CustomerName;
            txtCustomerIdNo.Text = objSalesMasterDAL.CustomerIdNo;
            txtCustomerAddress.Text = objSalesMasterDAL.CustomerAddress;
            hdnSalesMasterId.Value = objSalesMasterDAL.SalesMasterId.ToString();
            txtSalesDate.Text = loanGlobalsDAL.ConvertDateTimeToString(objSalesMasterDAL.SalesDate, loanAppGlobals.DateFormat);
            txtNotes.Text = objSalesMasterDAL.Notes;
            txtContractStartDate.Text = loanGlobalsDAL.ConvertDateTimeToString(objSalesMasterDAL.ContractStartDate, loanAppGlobals.DateFormat);
            if (objSalesMasterDAL.ContractAmount != null)
            {
                txtContractAmount.Text = objSalesMasterDAL.ContractAmount.Value.ToString("0.00");
            }
            if (objSalesMasterDAL.InstallmentAmount != null)
            {
                txtInstallmentAmount.Text = objSalesMasterDAL.InstallmentAmount.Value.ToString("0.00");
            }
            txtBillNo.Text = objSalesMasterDAL.BillNo;
            txtReceiptNo.Text = objSalesMasterDAL.ReceiptNo;

            ViewState["lvSalesItems"] = objSalesMasterDAL.lstSalesItemTranDAL;

            FillSalesItems();

            hdnModelSales.Value = "show";
            hdnActionSales.Value = "edit";
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
                FillSalesMaster();
            }
        }

        private bool GetItemStock(int itemMasterId)
        {
            loanItemMasterDAL objItemMasterDAL = new loanItemMasterDAL();
            DateTime FromDate = new DateTime();
            if (!string.IsNullOrEmpty(txtSalesDate.Text))
            {
                FromDate = DateTime.ParseExact(txtSalesDate.Text, loanAppGlobals.DateFormat, System.Globalization.DateTimeFormatInfo.InvariantInfo);
            }
            DateTime ToDate = new DateTime();
            if (!string.IsNullOrEmpty(txtSalesDate.Text))
            {
                ToDate = DateTime.ParseExact(txtSalesDate.Text, loanAppGlobals.DateFormat, System.Globalization.DateTimeFormatInfo.InvariantInfo);
            }
            objItemMasterDAL.linktoCompanyMasterId = ((loanUser)Session[loanSessionsDAL.UserSession]).CompanyMasterId;

            List<loanItemMasterDAL> lstItemMasterDAL = objItemMasterDAL.SelectAllItemStockReportPageWise(FromDate, ToDate, itemMasterId);

            if (lstItemMasterDAL == null || lstItemMasterDAL.Count == 0)
            {
                return false;
            }

            if (lstItemMasterDAL[0].CurrentQuantity < Convert.ToInt32(txtQuantity.Text))
            {
                return false;
            }
            return true;
        }

        private void PrintReceipt(int salesMasterID)
        {
            loanUser.CheckRoleRights(loanRoleRights.ViewRecord);

            loanSalesMasterDAL objSalesMasterDAL = new loanSalesMasterDAL();
            objSalesMasterDAL.SalesMasterId = salesMasterID;
            if (!objSalesMasterDAL.SelectSalesMaster())
            {
                loanAppGlobals.ShowMessage(loanMessagesDAL.SelectFail, loanMessageIcon.Error);
                return;
            }

            string HtmlReceipt = File.ReadAllText(Path.Combine(System.Configuration.ConfigurationManager.AppSettings["FileSavePath"], "ReceiptTemplate/", "receipt.html"));

            HtmlReceipt = HtmlReceipt.Replace("[#CUSTOMERNAME#]", objSalesMasterDAL.CustomerName);
            HtmlReceipt = HtmlReceipt.Replace("[#CUSTOMERIDNO#]", objSalesMasterDAL.CustomerIdNo);
            HtmlReceipt = HtmlReceipt.Replace("[#BILLNO#]", objSalesMasterDAL.BillNo);
            HtmlReceipt = HtmlReceipt.Replace("[#DATE#]", loanGlobalsDAL.ConvertDateTimeToString(objSalesMasterDAL.SalesDate, loanAppGlobals.DateFormat));
            HtmlReceipt = HtmlReceipt.Replace("[#TIME#]", loanGlobalsDAL.ConvertDateTimeToString(objSalesMasterDAL.SalesDate, loanAppGlobals.TimeFormat));
            HtmlReceipt = HtmlReceipt.Replace("[#ADDRESS#]", objSalesMasterDAL.CustomerAddress);

            string QRCodeText = "مؤسسة عمق نجد للتجارة" + Environment.NewLine + "الرقم الضريبى:302241766300003";
            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(QRCodeText, QRCodeGenerator.ECCLevel.Q);
            QRCode qrCode = new QRCode(qrCodeData);
            using (Bitmap qrCodeImage = qrCode.GetGraphic(20))
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    qrCodeImage.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                    byte[] byteImage = ms.ToArray();
                    HtmlReceipt = HtmlReceipt.Replace("[#QRCODESRC#]", "data:image/png;base64," + Convert.ToBase64String(byteImage));
                }
            }

            string HtmlReceiptRow = File.ReadAllText(Path.Combine(System.Configuration.ConfigurationManager.AppSettings["FileSavePath"], "ReceiptTemplate/", "receiptrow.html"));
            string row = "";
            int sr = 1;
            int rowheight = 300;
            foreach (loanSalesItemTranDAL obj in objSalesMasterDAL.lstSalesItemTranDAL)
            {
                row += HtmlReceiptRow;
                row = row.Replace("[#ITEMAMOUNT#]", (obj.Quantity * obj.SalesRate).ToString("0.00"));
                row = row.Replace("[#ITEMVAT#]", obj.VatAmount.ToString("0.00"));
                row = row.Replace("[#ITEMTAXTYPE#]", obj.Vat.ToString("0.00"));
                row = row.Replace("[#ITEMRATE#]", obj.SalesRate.ToString("0.00"));
                row = row.Replace("[#ITEMQTY#]", obj.Quantity.ToString());
                row = row.Replace("[#ITEMDESC#]", obj.Item);
                row = row.Replace("[#ITEMCODE#]", obj.ItemCode);
                row = row.Replace("[#ITEMSR#]", (sr++).ToString());
                rowheight -= 50;
            }

            HtmlReceipt = HtmlReceipt.Replace("[#RECEIPTROW#]", row);
            HtmlReceipt = HtmlReceipt.Replace("[#ROWHEIGHT#]", rowheight.ToString());

            decimal GrossAmount = 0;
            objSalesMasterDAL.lstSalesItemTranDAL.ForEach(f =>
            {
                GrossAmount += (f.Quantity * f.SalesRate) - f.DiscountAmount;
            });

            HtmlReceipt = HtmlReceipt.Replace("[#TOTAL#]", GrossAmount.ToString("0.00"));
            HtmlReceipt = HtmlReceipt.Replace("[#QUANTITY#]", objSalesMasterDAL.lstSalesItemTranDAL.Sum(a => a.Quantity).ToString());
            HtmlReceipt = HtmlReceipt.Replace("[#DISCOUNT#]", objSalesMasterDAL.lstSalesItemTranDAL.Sum(a => a.DiscountAmount).ToString("0.00"));
            HtmlReceipt = HtmlReceipt.Replace("[#TAX#]", objSalesMasterDAL.lstSalesItemTranDAL.Sum(a => a.VatAmount).ToString("0.00"));
            HtmlReceipt = HtmlReceipt.Replace("[#TAXPER#]", objSalesMasterDAL.lstSalesItemTranDAL.Count == 1 ? objSalesMasterDAL.lstSalesItemTranDAL[0].Vat.ToString("0") + "%" : "15%");
            HtmlReceipt = HtmlReceipt.Replace("[#NETAMOUNT#]", objSalesMasterDAL.lstSalesItemTranDAL.Sum(a => a.NetAmount).ToString("0.00"));

            ToWord toWord = new ToWord(Convert.ToDecimal(objSalesMasterDAL.lstSalesItemTranDAL.Sum(a => a.NetAmount).ToString("0.00")), new CurrencyInfo(CurrencyInfo.Currencies.SaudiArabia));
            string NetAmountText = toWord.ConvertToArabic();

            HtmlReceipt = HtmlReceipt.Replace("[#NETAMOUNTTEXT#]", NetAmountText);

            HtmlReceipt = HtmlReceipt.Replace("[#SOLDBY#]", objSalesMasterDAL.ModifiedBy);

            //HtmlToPdf converter = new HtmlToPdf();

            //converter.Options.PdfPageSize = (PdfPageSize)Enum.Parse(typeof(PdfPageSize), "A4", true);
            //converter.Options.MarginLeft = 25;
            //converter.Options.MarginRight = 25;
            //converter.Options.MarginTop = 25;
            //converter.Options.MarginBottom = 25;

            //PdfDocument doc = converter.ConvertHtmlString(HtmlReceipt);
            //doc.Save(FileSavePathWithFileName);
            //doc.Close();

            string ReceiptPath = Path.Combine(System.Configuration.ConfigurationManager.AppSettings["FileSavePath"], "Receipt/", "receipt.html");
            File.WriteAllText(ReceiptPath, HtmlReceipt);

            string url = System.Configuration.ConfigurationManager.AppSettings["FileRetrievePath"] + "Receipt/receipt.html";
            string s = "var win = window.open('" + url + "', 'popup_window', 'width=1024,height=720,left=100,top=100,resizable=yes'); win.focus(); win.print();";
            ClientScript.RegisterStartupScript(this.GetType(), "script", s, true);
        }
        #endregion

        #region WebMethods
        [System.Web.Services.WebMethod]
        public static loanSalesMasterDAL GetMaxBillNo()
        {
            if (HttpContext.Current.Session[loanSessionsDAL.UserSession] == null)
            {
                return null;
            }
            try
            {
                loanSalesMasterDAL objSalesMasterDAL = new loanSalesMasterDAL();
                if (objSalesMasterDAL.SelectMaxBillNo())
                {
                    return objSalesMasterDAL;
                }
                return null;
            }
            catch (Exception ex)
            {
                loanAppGlobals.SaveError(ex);
                return null;
            }
        }
        #endregion
    }

    public class loanSalesMasterPrint : loanSalesItemTranDAL
    {
        #region Properties
        public int? SalesMasterId { get; set; }
        public int? linktoCompanyMasterId { get; set; }
        public string CustomerName { get; set; }
        public string CustomerIdNo { get; set; }
        public string CustomerAddress { get; set; }
        public DateTime? SalesDate { get; set; }
        public string Notes { get; set; }
        public bool? IsVerified { get; set; }
        public DateTime? VerifiedDateTime { get; set; }
        public DateTime? ContractStartDate { get; set; }
        public decimal? ContractAmount { get; set; }
        public decimal? InstallmentAmount { get; set; }
        public string BillNo { get; set; }
        public string ReceiptNo { get; set; }
        public DateTime? UpdateDateTime { get; set; }
        public string SessionId { get; set; }

        /// Extra
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public string TranType { get; set; }
        public string VerifiedBy { get; set; }
        public string ModifiedBy { get; set; }
        public int? linktoCategoryMasterId { get; set; }
        public decimal? GrossAmount { get; set; }
        #endregion
    }
}
