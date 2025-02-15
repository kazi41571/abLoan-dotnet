using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using loanLibrary;
using System.Drawing;
using System.Web.UI.HtmlControls;

namespace abLOAN
{
    public partial class salesreport : BasePage
    {
        decimal totalPurchaseNetAmount = 0, totalNetAmount = 0, totalVat = 0, totalFees = 0, totalSales = 0
            , totalContractAmount = 0, totalInstallmentAmount = 0;
        int totalPurchaseQuantity, totalQuantity, currentQuantity, openingQuantity;


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

                    FillPurchaseSalesTransactionsMaster();
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
                pgrPurchaseSalesTransactionsMaster.CurrentPage = 1;
                FillPurchaseSalesTransactionsMaster();
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

                txtFilterCustomer.Text = string.Empty;
                ddlFilterItem.SelectedIndex = 0;

                pgrPurchaseSalesTransactionsMaster.CurrentPage = 1;
                FillPurchaseSalesTransactionsMaster();
            }
            catch (Exception ex)
            {
                loanAppGlobals.SaveError(ex);
            }
        }

        #region List Methods

        protected void lvPurchaseSalesTransactionsMaster_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            try
            {
                if (e.Item.ItemType == ListViewItemType.DataItem)
                {
                    loanSalesMasterDAL objSalesMasterDAL = (loanSalesMasterDAL)e.Item.DataItem;
                    HtmlTableRow cell = (HtmlTableRow)e.Item.FindControl("MainTableRow");

                    Literal ltrlTranDate = (Literal)e.Item.FindControl("ltrltranDate");
                    Literal ltrlCustomerName = (Literal)e.Item.FindControl("ltrlCustomerName");
                    Literal ltrlContractStartDate = (Literal)e.Item.FindControl("ltrlContractStartDate");
                    Literal ltrlContractAmount = (Literal)e.Item.FindControl("ltrlContractAmount");
                    Literal ltrlInstallmentAmount = (Literal)e.Item.FindControl("ltrlInstallmentAmount");
                    ListView lvSalesItemTran = (ListView)e.Item.FindControl("lvSalesItemTran");

                    ltrlTranDate.Text = loanGlobalsDAL.ConvertDateTimeToString(objSalesMasterDAL.SalesDate, loanAppGlobals.DateFormat);
                    ltrlCustomerName.Text = objSalesMasterDAL.CustomerName;

                    lvSalesItemTran.DataSource = objSalesMasterDAL.lstSalesItemTranDAL;
                    lvSalesItemTran.DataBind();

                    if (objSalesMasterDAL.ContractStartDate != null)
                    {
                        ltrlContractStartDate.Text = loanGlobalsDAL.ConvertDateTimeToString(objSalesMasterDAL.ContractStartDate, loanAppGlobals.DateFormat);
                    }
                    if (objSalesMasterDAL.ContractAmount != null)
                    {
                        ltrlContractAmount.Text = objSalesMasterDAL.ContractAmount.Value.ToString("0.00");
                        if (objSalesMasterDAL.TranType.Equals("S"))
                        {
                            totalContractAmount += Convert.ToDecimal(ltrlContractAmount.Text);
                        }
                    }
                    if (objSalesMasterDAL.InstallmentAmount != null)
                    {
                        ltrlInstallmentAmount.Text = objSalesMasterDAL.InstallmentAmount.Value.ToString("0.00");
                        if (objSalesMasterDAL.TranType.Equals("S"))
                        {
                            totalInstallmentAmount += Convert.ToDecimal(ltrlInstallmentAmount.Text);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                loanAppGlobals.SaveError(ex);
            }
        }

        protected void pgrPurchaseSalesTransactionsMaster_ItemCommand(object sender, EventArgs e)
        {
            try
            {
                FillPurchaseSalesTransactionsMaster();
            }
            catch (Exception ex)
            {
                loanAppGlobals.SaveError(ex);
            }
        }

        protected void lvPurchaseSalesTransactionsMaster_DataBound(object sender, EventArgs e)
        {
            if (((ListView)sender).Items.Count > 0)
            {
                //Literal ltrlOpeningQuantity = (Literal)((ListView)sender).FindControl("ltrlOpeningQuantity");
                //ltrlOpeningQuantity.Text = openingQuantity.ToString();
                //Literal ltrlTotalPurchaseQuantity = (Literal)((ListView)sender).FindControl("ltrlTotalPurchaseQuantity");
                //ltrlTotalPurchaseQuantity.Text = totalPurchaseQuantity.ToString();

                Literal ltrlTotalQuantity = (Literal)((ListView)sender).FindControl("ltrlTotalQuantity");
                Literal ltrlTotalNetAmount = (Literal)((ListView)sender).FindControl("ltrlTotalNetAmount");
                //Literal ltrlTotalVat = (Literal)((ListView)sender).FindControl("ltrlTotalVat");
                Literal ltrlTotalFees = (Literal)((ListView)sender).FindControl("ltrlTotalFees");
                Literal ltrlTotalContractAmount = (Literal)((ListView)sender).FindControl("ltrlTotalContractAmount");
                Literal ltrlTotalInstallmentAmount = (Literal)((ListView)sender).FindControl("ltrlTotalInstallmentAmount");

                ltrlTotalQuantity.Text = totalQuantity.ToString();
                //ltrlTotalVat.Text = totalVat.ToString("0.00");
                ltrlTotalFees.Text = totalFees.ToString("0.00");
                ltrlTotalNetAmount.Text = totalNetAmount.ToString("0.00");
                ltrlTotalContractAmount.Text = totalContractAmount.ToString("0.00");
                ltrlTotalInstallmentAmount.Text = totalInstallmentAmount.ToString("0.00");

                //Literal ltrlTotalSales = (Literal)((ListView)sender).FindControl("ltrlTotalGrossAmount");
                // ltrlTotalSales.Text = totalSales.ToString("0.00");                
                //Literal ltrlCurrentQuantity = (Literal)((ListView)sender).FindControl("ltrlCurrentQuantity");
                //ltrlCurrentQuantity.Text = currentQuantity.ToString();                
                //Literal ltrlTotalPurchaseNetAmount = (Literal)((ListView)sender).FindControl("ltrlTotalPurchaseNetAmount");
                //ltrlTotalPurchaseNetAmount.Text = totalPurchaseNetAmount.ToString("0.00");


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
                    //Literal ltrlVat = (Literal)e.Item.FindControl("ltrlVat");
                    Literal ltrlFees = (Literal)e.Item.FindControl("ltrlFees");
                    Literal ltrlNetAmount = (Literal)e.Item.FindControl("ltrlNetAmount");

                    ltrlItem.Text = objSalesItemTranDAL.Item;
                    ltrlQuantity.Text = objSalesItemTranDAL.Quantity.ToString();
                    ltrlSalesRate.Text = objSalesItemTranDAL.SalesRate.ToString("0.00");
                    ltrlGrossAmount.Text = (objSalesItemTranDAL.SalesRate * objSalesItemTranDAL.Quantity).ToString("0.00");
                    //ltrlVat.Text = ((objSalesItemTranDAL.SalesRate * objSalesItemTranDAL.Quantity * objSalesItemTranDAL.Vat) / 100).ToString("0.00");
                    //ltrlFees.Text = objSalesItemTranDAL.Fees.ToString("0.00");
                    ltrlFees.Text = (((objSalesItemTranDAL.SalesRate * objSalesItemTranDAL.Quantity * objSalesItemTranDAL.Vat) / 100) + objSalesItemTranDAL.Fees).ToString("0.00");
                    ltrlNetAmount.Text = objSalesItemTranDAL.NetAmount.ToString("0.00");

                    totalQuantity += Convert.ToInt32(ltrlQuantity.Text);
                    totalSales += Convert.ToDecimal(ltrlGrossAmount.Text);
                    //totalVat += Convert.ToDecimal(ltrlVat.Text);
                    totalFees += Convert.ToDecimal(ltrlFees.Text);
                    totalNetAmount += Convert.ToDecimal(objSalesItemTranDAL.NetAmount);
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
                pgrPurchaseSalesTransactionsMaster.CurrentPage = Convert.ToInt16(loanSessionsDAL.GetSessionKeyValue("CurrentPageSales"));
            }
            if (loanSessionsDAL.GetSessionKeyValue("FilterSales") != null)
            {
                loanSalesMasterDAL objSalesMasterDAL = (loanSalesMasterDAL)loanSessionsDAL.GetSessionKeyValue("FilterSales");
                if (objSalesMasterDAL.CustomerName != "" && objSalesMasterDAL.CustomerName != null)
                {
                    txtFilterCustomer.Text = objSalesMasterDAL.CustomerName;
                }
                //ddlFilterItem.SelectedValue = objSalesMasterDAL.linktoItemMasterId.ToString();
            }
        }

        private void FillPurchaseSalesTransactionsMaster()
        {

            loanSalesMasterDAL objSalesMasterDAL = new loanSalesMasterDAL();
            if (txtFilterCustomer.Text != string.Empty)
            {
                objSalesMasterDAL.CustomerName = txtFilterCustomer.Text;
            }
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


            loanSessionsDAL.SetSessionKeyValue("FilterSales", objSalesMasterDAL);
            loanSessionsDAL.SetSessionKeyValue("CurrentPageSales", pgrPurchaseSalesTransactionsMaster.CurrentPage);

            int TotalRecords;
            List<loanSalesMasterDAL> lstSalesMaster = objSalesMasterDAL.SelectAllSalesReportPageWise(pgrPurchaseSalesTransactionsMaster.StartRowIndex, short.MaxValue, out TotalRecords, ItemMasterId);
            pgrPurchaseSalesTransactionsMaster.TotalRowCount = TotalRecords;

            if (lstSalesMaster == null)
            {
                loanAppGlobals.ShowMessage(loanMessagesDAL.SelectAllFail, loanMessageIcon.Error);
                return;
            }
            if (lstSalesMaster.Count == 0 && pgrPurchaseSalesTransactionsMaster.TotalRowCount > 0)
            {
                pgrPurchaseSalesTransactionsMaster_ItemCommand(pgrPurchaseSalesTransactionsMaster, new EventArgs());
                return;
            }
            currentQuantity = openingQuantity = 0;

            lvPurchaseSalesTransactionsMaster.DataSource = lstSalesMaster;
            lvPurchaseSalesTransactionsMaster.DataBind();

            if (lstSalesMaster.Count > 0)
            {
                int EndiIndex = pgrPurchaseSalesTransactionsMaster.StartRowIndex + short.MaxValue < pgrPurchaseSalesTransactionsMaster.TotalRowCount ? pgrPurchaseSalesTransactionsMaster.StartRowIndex + short.MaxValue : pgrPurchaseSalesTransactionsMaster.TotalRowCount;
                lblRecords.Text = string.Format(Resources.Messages.ResourceManager.GetString("Records"), pgrPurchaseSalesTransactionsMaster.StartRowIndex + 1, EndiIndex, pgrPurchaseSalesTransactionsMaster.TotalRowCount);
                lblRecords.Visible = true;
            }
            else
            {
                lblRecords.Visible = false;
            }

            if (pgrPurchaseSalesTransactionsMaster.TotalRowCount <= short.MaxValue)
            {
                pgrPurchaseSalesTransactionsMaster.Visible = false;
            }
            else
            {
                pgrPurchaseSalesTransactionsMaster.Visible = true;
            }

        }

        private void GetItem()
        {

            ddlFilterItem.Items.Clear();
            ddlFilterItem.Items.Add(new System.Web.UI.WebControls.ListItem(loanDropDownItem.Select, ""));

            List<loanItemMasterDAL> lstItemMasterDAL = loanItemMasterDAL.SelectAllItemMasterItemName();
            if (lstItemMasterDAL == null)
            {
                loanAppGlobals.ShowMessage(loanMessagesDAL.SelectAllFail, loanMessageIcon.Error);
                return;
            }
            foreach (loanItemMasterDAL obj in lstItemMasterDAL)
            {
                ddlFilterItem.Items.Add(new System.Web.UI.WebControls.ListItem(obj.ItemName, obj.ItemMasterId.ToString()));
            }
        }
        #endregion


    }
}
