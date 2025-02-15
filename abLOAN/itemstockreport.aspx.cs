using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using loanLibrary;
using System.Drawing;
using System.Web.UI.HtmlControls;

namespace abLOAN
{
    public partial class itemstockreport : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                loanSessionsDAL.CheckSession();

                if (!Page.IsPostBack)
                {
                    loanUser.CheckRoleRights(loanRoleRights.ViewList);

                    GetItem();

                    GetPageDefaults();

                    loanSessionsDAL.RemoveSessionAllKeyValue();

                    FillItemStock();
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
                FillItemStock();
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
                txtFromDate.Text = loanGlobalsDAL.ConvertDateTimeToString(loanGlobalsDAL.GetCurrentDateTime().AddYears(-1), loanAppGlobals.DateFormat);
                txtToDate.Text = loanGlobalsDAL.ConvertDateTimeToString(loanGlobalsDAL.GetCurrentDateTime(), loanAppGlobals.DateFormat);
                ddlFilterItem.SelectedIndex = 0;
                ddlFilterShowZeroValue.SelectedIndex = 0;

                FillItemStock();
            }
            catch (Exception ex)
            {
                loanAppGlobals.SaveError(ex);
            }
        }

        #region List Methods
        protected void lvItemStock_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            try
            {
                if (e.Item.ItemType == ListViewItemType.DataItem)
                {
                    loanItemMasterDAL objItemMasterDAL = (loanItemMasterDAL)e.Item.DataItem;

                    Literal ltrlItem = (Literal)e.Item.FindControl("ltrlItem");
                    Literal ltrlOpeningQuantity = (Literal)e.Item.FindControl("ltrlOpeningQuantity");
                    Literal ltrlPurchaseQuantity = (Literal)e.Item.FindControl("ltrlPurchaseQuantity");
                    Literal ltrlSalesQuantity = (Literal)e.Item.FindControl("ltrlSalesQuantity");
                    Literal ltrlClosingQuantity = (Literal)e.Item.FindControl("ltrlClosingQuantity");

                    ltrlItem.Text = objItemMasterDAL.ItemName;
                    ltrlOpeningQuantity.Text = objItemMasterDAL.OpeningQuantity.ToString();
                    ltrlPurchaseQuantity.Text = objItemMasterDAL.PurchaseQuantity.ToString();
                    ltrlSalesQuantity.Text = objItemMasterDAL.SalesQuantity.ToString();
                    ltrlClosingQuantity.Text = objItemMasterDAL.CurrentQuantity.ToString();
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
            txtFromDate.Text = loanGlobalsDAL.ConvertDateTimeToString(loanGlobalsDAL.GetCurrentDateTime().AddYears(-1), loanAppGlobals.DateFormat);
            txtToDate.Text = loanGlobalsDAL.ConvertDateTimeToString(loanGlobalsDAL.GetCurrentDateTime(), loanAppGlobals.DateFormat);

        }

        private void FillItemStock()
        {
            loanItemMasterDAL objItemMasterDAL = new loanItemMasterDAL();
            DateTime FromDate = new DateTime();
            if (!string.IsNullOrEmpty(txtFromDate.Text))
            {
                FromDate = DateTime.ParseExact(txtFromDate.Text, loanAppGlobals.DateFormat, System.Globalization.DateTimeFormatInfo.InvariantInfo);
            }
            DateTime ToDate = new DateTime();
            if (!string.IsNullOrEmpty(txtToDate.Text))
            {
                ToDate = DateTime.ParseExact(txtToDate.Text, loanAppGlobals.DateFormat, System.Globalization.DateTimeFormatInfo.InvariantInfo);
            }
            objItemMasterDAL.linktoCompanyMasterId = ((loanUser)Session[loanSessionsDAL.UserSession]).CompanyMasterId;
            int ItemMasterId = 0;
            if (ddlFilterItem.SelectedValue != string.Empty)
            {
                ItemMasterId = Convert.ToInt32(ddlFilterItem.SelectedValue);
            }

            List<loanItemMasterDAL> lstItemMasterDAL = objItemMasterDAL.SelectAllItemStockReportPageWise(FromDate, ToDate, ItemMasterId);

            if (lstItemMasterDAL == null)
            {
                loanAppGlobals.ShowMessage(loanMessagesDAL.SelectAllFail, loanMessageIcon.Error);
                return;
            }
            if (ddlFilterShowZeroValue.SelectedValue == "No")
            {
                lstItemMasterDAL = lstItemMasterDAL.FindAll(f => f.CurrentQuantity > 0);
            }

            lvItemStock.DataSource = lstItemMasterDAL;
            lvItemStock.DataBind();
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
