using System;
using System.Collections.Generic;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;
using loanLibrary;

namespace abLOAN
{
    public partial class customerguarantorreport : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                loanSessionsDAL.CheckSession();

                if (!Page.IsPostBack)
                {
                    //loanUser.CheckUserRights(loanUserRights.ViewCustomer);
                    loanUser.CheckRoleRights(loanRoleRights.ViewList);


                    GetPageDefaults();

                    //loanSessionsDAL.RemoveSessionAllKeyValue();

                    FillCustomerMaster();
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
                pgrCustomerMaster.CurrentPage = 1;
                FillCustomerMaster();
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
                txtFilterPhoneMobile.Text = string.Empty;
                txtFilterGuarantors.Text = string.Empty;

                pgrCustomerMaster.CurrentPage = 1;
                FillCustomerMaster();
            }
            catch (Exception ex)
            {
                loanAppGlobals.SaveError(ex);
            }
        }



        #region List Methods

        protected void lvCustomerMaster_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            try
            {
                if (e.Item.ItemType == ListViewItemType.DataItem)
                {
                    loanCustomerMasterDAL objCustomerMasterDAL = (loanCustomerMasterDAL)e.Item.DataItem;

                    Literal ltrlCustomerName = (Literal)e.Item.FindControl("ltrlCustomerName");
                    Literal ltrlMobile1 = (Literal)e.Item.FindControl("ltrlMobile1");
                    Literal ltrlMobile2 = (Literal)e.Item.FindControl("ltrlMobile2");
                    Literal ltrlMobile3 = (Literal)e.Item.FindControl("ltrlMobile3");
                    Literal ltrlGuarantors = (Literal)e.Item.FindControl("ltrlGuarantors");

                    if (objCustomerMasterDAL.IsRedFlag == true)
                    {
                        ltrlCustomerName.Text = "<span class='text-danger'>" + objCustomerMasterDAL.CustomerName + "</span>";
                    }
                    else
                    {
                        ltrlCustomerName.Text = objCustomerMasterDAL.CustomerName;
                    }
                    ltrlMobile1.Text = objCustomerMasterDAL.Mobile1;
                    ltrlMobile2.Text = objCustomerMasterDAL.Mobile2;
                    ltrlMobile3.Text = objCustomerMasterDAL.Mobile3;
                    ltrlGuarantors.Text = objCustomerMasterDAL.Guarantors;
                }
            }
            catch (Exception ex)
            {
                loanAppGlobals.SaveError(ex);
            }
        }

        protected void pgrCustomerMaster_ItemCommand(object sender, EventArgs e)
        {
            try
            {
                FillCustomerMaster();
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
            if (loanSessionsDAL.GetSessionKeyValue("CurrentPageCustomer") != null)
            {
                pgrCustomerMaster.CurrentPage = Convert.ToInt16(loanSessionsDAL.GetSessionKeyValue("CurrentPageCustomer"));
            }
            if (loanSessionsDAL.GetSessionKeyValue("FilterCustomer") != null)
            {
                loanCustomerMasterDAL objCustomerMasterDAL = (loanCustomerMasterDAL)loanSessionsDAL.GetSessionKeyValue("FilterCustomer");
                txtFilterCustomer.Text = objCustomerMasterDAL.CustomerName;
                txtFilterPhoneMobile.Text = objCustomerMasterDAL.Phone1;

            }
        }

        private void FillCustomerMaster()
        {

            loanCustomerMasterDAL objCustomerMasterDAL = new loanCustomerMasterDAL();
            objCustomerMasterDAL.CustomerName = txtFilterCustomer.Text.Trim();
            objCustomerMasterDAL.Phone1 = txtFilterPhoneMobile.Text.Trim();
            objCustomerMasterDAL.Guarantors = txtFilterGuarantors.Text.Trim();
            objCustomerMasterDAL.linktoCompanyMasterId = ((loanUser)Session[loanSessionsDAL.UserSession]).CompanyMasterId;

            loanSessionsDAL.SetSessionKeyValue("FilterCustomer", objCustomerMasterDAL);
            loanSessionsDAL.SetSessionKeyValue("CurrentPageCustomer", pgrCustomerMaster.CurrentPage);

            int TotalRecords;
            List<loanCustomerMasterDAL> lstCustomerMaster = objCustomerMasterDAL.SelectAllCustomerMasterPageWise(pgrCustomerMaster.StartRowIndex, pgrCustomerMaster.PageSize, out TotalRecords);
            pgrCustomerMaster.TotalRowCount = TotalRecords;

            if (lstCustomerMaster == null)
            {
                loanAppGlobals.ShowMessage(loanMessagesDAL.SelectAllFail, loanMessageIcon.Error);
                return;
            }

            if (lstCustomerMaster.Count == 0 && pgrCustomerMaster.TotalRowCount > 0)
            {
                pgrCustomerMaster_ItemCommand(pgrCustomerMaster, new EventArgs());
                return;
            }

            lvCustomerMaster.DataSource = lstCustomerMaster;
            lvCustomerMaster.DataBind();

            if (lstCustomerMaster.Count > 0)
            {
                int EndiIndex = pgrCustomerMaster.StartRowIndex + pgrCustomerMaster.PageSize < pgrCustomerMaster.TotalRowCount ? pgrCustomerMaster.StartRowIndex + pgrCustomerMaster.PageSize : pgrCustomerMaster.TotalRowCount;
                lblRecords.Text = string.Format(Resources.Messages.ResourceManager.GetString("Records"), pgrCustomerMaster.StartRowIndex + 1, EndiIndex, pgrCustomerMaster.TotalRowCount);
                lblRecords.Visible = true;
            }
            else
            {
                lblRecords.Visible = false;
            }

            if (pgrCustomerMaster.TotalRowCount <= pgrCustomerMaster.PageSize)
            {
                pgrCustomerMaster.Visible = false;
            }
            else
            {
                pgrCustomerMaster.Visible = true;
            }

        }
        #endregion


    }
}
