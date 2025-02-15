using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using loanLibrary;
using System.Threading;
using System.Globalization;
using System.Data;
using System.Text;
using System.IO;
using System.Configuration;

namespace abLOAN
{
    public partial class contractamountreport : BasePage
    {
        List<loanContractMasterDAL> lstContractMaster;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                loanSessionsDAL.CheckSession();

                if (!Page.IsPostBack)
                {
                    //loanUser.CheckUserRights(loanUserRights.ViewContract);
                    loanUser.CheckRoleRights(loanRoleRights.ViewList);


                    loanSessionsDAL.RemoveSessionAllKeyValue();

                    FillContractMaster();
                }
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

                    Literal ltrlContractAmount = (Literal)e.Item.FindControl("ltrlContractAmount");
                    Literal ltrlPendingAmount = (Literal)e.Item.FindControl("ltrlPendingAmount");
                    Literal ltrlIncomeAmount = (Literal)e.Item.FindControl("ltrlIncomeAmount");
                    Literal ltrlInstallmentAmount = (Literal)e.Item.FindControl("ltrlInstallmentAmount");

                    ltrlContractAmount.Text = objContractMasterDAL.ContractAmount.ToString("0.00");
                    ltrlPendingAmount.Text = objContractMasterDAL.PendingAmount.ToString("0.00");
                    ltrlIncomeAmount.Text = objContractMasterDAL.IncomeAmount.ToString("0.00");
                    ltrlInstallmentAmount.Text = objContractMasterDAL.InstallmentAmount.ToString("0.00");
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
        private void FillContractMaster()
        {

            loanContractMasterDAL objContractMasterDAL = new loanContractMasterDAL();
            objContractMasterDAL.SelectContractMasterAmount();

            lstContractMaster = new List<loanContractMasterDAL>();
            lstContractMaster.Add(objContractMasterDAL);

            lvContractMaster.DataSource = lstContractMaster;
            lvContractMaster.DataBind();

        }
        #endregion
    }
}
