using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using loanLibrary;
using System.Drawing;
using System.Web.UI.HtmlControls;

namespace abLOAN
{
    public partial class paymentreceivedreport : BasePage
    {
        decimal totalAmount;

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                loanSessionsDAL.CheckSession();

                if (!Page.IsPostBack)
                {
                    loanUser.CheckRoleRights(loanRoleRights.ViewList);

                    GetPageDefaults();

                    loanSessionsDAL.RemoveSessionAllKeyValue();

                    FillPaymentReceivedMaster();
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
                FillPaymentReceivedMaster();
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

                FillPaymentReceivedMaster();
            }
            catch (Exception ex)
            {
                loanAppGlobals.SaveError(ex);
            }
        }

        #region List Methods
        private string getMonthName(int month)
        {
            switch (month)
            {
                case 1:
                    return "January";
                case 2:
                    return "February";
                case 3:
                    return "March";
                case 4:
                    return "April";
                case 5:
                    return "May";
                case 6:
                    return "June";
                case 7:
                    return "July";
                case 8:
                    return "August";
                case 9:
                    return "September";
                case 10:
                    return "October";
                case 11:
                    return "November";
                case 12:
                    return "December";
                default:
                    return "";

            }
        }

        protected void lvPaymentReceivedMaster_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            try
            {
                if (e.Item.ItemType == ListViewItemType.DataItem)
                {
                    loanCustomerPaymentMasterDAL objCustomerPaymentMasterDAL = (loanCustomerPaymentMasterDAL)e.Item.DataItem;

                    Literal ltrlMonthYear = (Literal)e.Item.FindControl("ltrlMonthYear");
                    Literal ltrlTotalAmount = (Literal)e.Item.FindControl("ltrlTotalAmount");

                    ltrlMonthYear.Text = getMonthName(Convert.ToInt16(objCustomerPaymentMasterDAL.Month)) + " " + objCustomerPaymentMasterDAL.Year;
                    ltrlTotalAmount.Text = objCustomerPaymentMasterDAL.Amount.ToString("0.00");

                    totalAmount += objCustomerPaymentMasterDAL.Amount;
                }
            }
            catch (Exception ex)
            {
                loanAppGlobals.SaveError(ex);
            }
        }

        protected void lvPaymentReceivedMaster_DataBound(object sender, EventArgs e)
        {
            if (((ListView)sender).Items.Count > 0)
            {
                Literal ltrlTotalAmount = (Literal)((ListView)sender).FindControl("ltrlTotalAmount");
                ltrlTotalAmount.Text = totalAmount.ToString("0.00");
            }
        }
        #endregion

        #region Private Methods
        private void GetPageDefaults()
        {
            txtFromDate.Text = loanGlobalsDAL.ConvertDateTimeToString(loanGlobalsDAL.GetCurrentDateTime().AddYears(-1), loanAppGlobals.DateFormat);
            txtToDate.Text = loanGlobalsDAL.ConvertDateTimeToString(loanGlobalsDAL.GetCurrentDateTime(), loanAppGlobals.DateFormat);
        }

        private void FillPaymentReceivedMaster()
        {
            loanCustomerPaymentMasterDAL objCustomerPaymentMasterDAL = new loanCustomerPaymentMasterDAL();
            if (!string.IsNullOrEmpty(txtFromDate.Text))
            {
                objCustomerPaymentMasterDAL.FromDate = DateTime.ParseExact(txtFromDate.Text, loanAppGlobals.DateFormat, System.Globalization.DateTimeFormatInfo.InvariantInfo);
            }
            if (!string.IsNullOrEmpty(txtToDate.Text))
            {
                objCustomerPaymentMasterDAL.ToDate = DateTime.ParseExact(txtToDate.Text, loanAppGlobals.DateFormat, System.Globalization.DateTimeFormatInfo.InvariantInfo);
            }
            objCustomerPaymentMasterDAL.linktoCompanyMasterId = ((loanUser)Session[loanSessionsDAL.UserSession]).CompanyMasterId;

            List<loanCustomerPaymentMasterDAL> lstPaymentAmount = objCustomerPaymentMasterDAL.SelectMonthWisePaymentReceivedReport();

            lvPaymentAmount.DataSource = lstPaymentAmount;
            lvPaymentAmount.DataBind();
        }
        #endregion
    }
}
