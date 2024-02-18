using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using loanLibrary;
using System.Drawing;
using System.Web.UI.HtmlControls;

namespace abLOAN
{
    public partial class purchasereport : BasePage
    {
        decimal totalPurchaseNetAmount = 0, totalVat = 0, totalFees = 0, totalSales = 0;
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
                    loanPurchaseMasterDAL objPurchaseMasterDAL = (loanPurchaseMasterDAL)e.Item.DataItem;
                    HtmlTableRow cell = (HtmlTableRow)e.Item.FindControl("MainTableRow");
                    Literal ltrlTranDate = (Literal)e.Item.FindControl("ltrltranDate");
                    Literal ltrlCustomerName = (Literal)e.Item.FindControl("ltrlCustomerName");
                    Literal ltrlItem = (Literal)e.Item.FindControl("ltrlItem");
                    Literal ltrlPurchaseQuantity = (Literal)e.Item.FindControl("ltrlPurchaseQuantity");
                    //Literal ltrlQuantity = (Literal)e.Item.FindControl("ltrlQuantity");
                    //Literal ltrlCurrentQuantity = (Literal)e.Item.FindControl("ltrlCurrentQuantity");
                    Literal ltrlRate = (Literal)e.Item.FindControl("ltrlRate");
                    Literal ltrlGrossAmount = (Literal)e.Item.FindControl("ltrlGrossAmount");
                    Literal ltrlVat = (Literal)e.Item.FindControl("ltrlVat");
                    Literal ltrlFees = (Literal)e.Item.FindControl("ltrlFees");
                    Literal ltrlPurchaseNetAmount = (Literal)e.Item.FindControl("ltrlPurchaseNetAmount");
                    //Literal ltrlNetAmount = (Literal)e.Item.FindControl("ltrlNetAmount");


                    ltrlTranDate.Text = loanGlobalsDAL.ConvertDateTimeToString(objPurchaseMasterDAL.TranDate, loanAppGlobals.DateFormat);
                    ltrlCustomerName.Text = objPurchaseMasterDAL.CustomerName;
                    ltrlItem.Text = objPurchaseMasterDAL.Item;
                    if (objPurchaseMasterDAL.TranType.Equals("S"))
                    {
                        //ltrlQuantity.Text = objSalesMasterDAL.Quantity.ToString();
                        //currentQuantity = currentQuantity - objSalesMasterDAL.Quantity;
                    }
                    else
                    {
                        ltrlPurchaseQuantity.Text = objPurchaseMasterDAL.Quantity.ToString();
                        currentQuantity = currentQuantity + objPurchaseMasterDAL.Quantity;
                        ltrlCustomerName.Text = Resources.Resource.PurchasePageTitle;
                    }
                    //ltrlCurrentQuantity.Text = currentQuantity.ToString();
                    ltrlRate.Text = objPurchaseMasterDAL.Rate.ToString("0.00");
                    ltrlGrossAmount.Text = (objPurchaseMasterDAL.Rate * objPurchaseMasterDAL.Quantity).ToString("0.00");

                    ltrlVat.Text = ((objPurchaseMasterDAL.Rate * objPurchaseMasterDAL.Quantity * objPurchaseMasterDAL.Vat) / 100).ToString("0.00");
                    ltrlFees.Text = objPurchaseMasterDAL.Fees.ToString("0.00");
                    if (objPurchaseMasterDAL.TranType.Equals("S"))
                    {
                        //ltrlNetAmount.Text = objSalesMasterDAL.NetAmount.ToString("0.00");
                    }
                    else
                    {
                        ltrlPurchaseNetAmount.Text = objPurchaseMasterDAL.NetAmount.ToString("0.00");
                    }

                    if (objPurchaseMasterDAL.TranType.Equals("S"))
                    {
                        totalQuantity += objPurchaseMasterDAL.Quantity;
                    }
                    else
                    {
                        totalPurchaseQuantity += objPurchaseMasterDAL.Quantity;
                    }

                    if (objPurchaseMasterDAL.TranType.Equals("S"))
                    {
                        totalSales += Convert.ToDecimal(ltrlGrossAmount.Text);
                    }
                    else
                    {
                        totalSales += Convert.ToDecimal(ltrlGrossAmount.Text);
                    }
                    totalVat += Convert.ToDecimal(ltrlVat.Text);
                    totalFees += objPurchaseMasterDAL.Fees;

                    if (objPurchaseMasterDAL.TranType.Equals("S"))
                    {
                        //totalNetAmount += Convert.ToDecimal(ltrlNetAmount.Text);
                    }
                    else
                    {
                        totalPurchaseNetAmount += Convert.ToDecimal(ltrlPurchaseNetAmount.Text);
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
                Literal ltrlTotalPurchaseQuantity = (Literal)((ListView)sender).FindControl("ltrlTotalPurchaseQuantity");
                ltrlTotalPurchaseQuantity.Text = totalPurchaseQuantity.ToString();
                //Literal ltrlTotalQuantity = (Literal)((ListView)sender).FindControl("ltrlTotalQuantity");
                //ltrlTotalQuantity.Text = totalQuantity.ToString();
                //Literal ltrlTotalSales = (Literal)((ListView)sender).FindControl("ltrlTotalGrossAmount");
                // ltrlTotalSales.Text = totalSales.ToString("0.00");                
                //Literal ltrlCurrentQuantity = (Literal)((ListView)sender).FindControl("ltrlCurrentQuantity");
                //ltrlCurrentQuantity.Text = currentQuantity.ToString();

                Literal ltrlTotalVat = (Literal)((ListView)sender).FindControl("ltrlTotalVat");
                ltrlTotalVat.Text = totalVat.ToString("0.00");
                Literal ltrlTotalFees = (Literal)((ListView)sender).FindControl("ltrlTotalFees");
                ltrlTotalFees.Text = totalFees.ToString("0.00");
                Literal ltrlTotalPurchaseNetAmount = (Literal)((ListView)sender).FindControl("ltrlTotalPurchaseNetAmount");
                ltrlTotalPurchaseNetAmount.Text = totalPurchaseNetAmount.ToString("0.00");
                //Literal ltrlTotalNetAmount = (Literal)((ListView)sender).FindControl("ltrlTotalNetAmount");
                //ltrlTotalNetAmount.Text = totalNetAmount.ToString("0.00");
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

            if (loanSessionsDAL.GetSessionKeyValue("CurrentPagePurchase") != null)
            {
                pgrPurchaseSalesTransactionsMaster.CurrentPage = Convert.ToInt16(loanSessionsDAL.GetSessionKeyValue("CurrentPagePurchase"));
            }
            if (loanSessionsDAL.GetSessionKeyValue("FilterPurchase") != null)
            {
                loanPurchaseMasterDAL objPurchaseMasterDAL = (loanPurchaseMasterDAL)loanSessionsDAL.GetSessionKeyValue("FilterPurchase");

                ddlFilterItem.SelectedValue = objPurchaseMasterDAL.linktoItemMasterId.ToString();
            }
        }


        private void FillPurchaseSalesTransactionsMaster()
        {

            loanPurchaseMasterDAL objPurchaseMasterDAL = new loanPurchaseMasterDAL();
            if (!string.IsNullOrEmpty(txtFromDate.Text))
            {
                objPurchaseMasterDAL.FromDate = DateTime.ParseExact(txtFromDate.Text, loanAppGlobals.DateFormat, System.Globalization.DateTimeFormatInfo.InvariantInfo);
            }
            if (!string.IsNullOrEmpty(txtToDate.Text))
            {
                objPurchaseMasterDAL.ToDate = DateTime.ParseExact(txtToDate.Text, loanAppGlobals.DateFormat, System.Globalization.DateTimeFormatInfo.InvariantInfo);
            }
            if (ddlFilterItem.SelectedValue != string.Empty)
            {
                objPurchaseMasterDAL.linktoItemMasterId = Convert.ToInt32(ddlFilterItem.SelectedValue);
            }
            objPurchaseMasterDAL.linktoCompanyMasterId = ((loanUser)Session[loanSessionsDAL.UserSession]).CompanyMasterId;


            loanSessionsDAL.SetSessionKeyValue("FilterPurchase", objPurchaseMasterDAL);
            loanSessionsDAL.SetSessionKeyValue("CurrentPagePurchase", pgrPurchaseSalesTransactionsMaster.CurrentPage);

            int TotalRecords;
            List<loanPurchaseMasterDAL> lstSalesMaster = objPurchaseMasterDAL.SelectAllPurchaseReportPageWise(pgrPurchaseSalesTransactionsMaster.StartRowIndex, int.MaxValue, out TotalRecords);
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
            if (lstSalesMaster.Count > 0 && lstSalesMaster[0].TotalQuantity > 0)
            {
                currentQuantity = openingQuantity = Convert.ToInt32(lstSalesMaster[0].TotalQuantity);
            }

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
