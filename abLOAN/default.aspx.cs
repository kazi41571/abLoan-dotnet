using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using loanLibrary;
using System.Globalization;

namespace abLOAN
{
    public partial class _default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                loanSessionsDAL.CheckSession();

                if (!Page.IsPostBack)
                {
                    loanSessionsDAL.RemoveSessionAllKeyValue();
                    if (loanUser.GetRoleRights(loanRoleRights.Custom, "VerifyRecord") == true)                    
                    {
                        FillVerifyRecord();
                    }
                }
            }
            catch (Exception ex)
            {
                loanAppGlobals.SaveError(ex);
            }
        }
        protected void lvVerifyMaster_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            try
            {
                if (e.Item.ItemType == ListViewItemType.DataItem)
                {
                    loanGlobalsDAL objGlobalsDAL = (loanGlobalsDAL)e.Item.DataItem;

                    HyperLink ltrlUrl = (HyperLink)e.Item.FindControl("ltrlUrl");
                    Literal ltrlTotal = (Literal)e.Item.FindControl("ltrlTotal");

                    ltrlUrl.Text = objGlobalsDAL.Page;
                    ltrlUrl.NavigateUrl = objGlobalsDAL.Url;
                    ltrlTotal.Text = objGlobalsDAL.Total;
                }
            }
            catch (Exception ex)
            {
                loanAppGlobals.SaveError(ex);
            }
        }
        private void FillVerifyRecord()
        {

            loanGlobalsDAL objGlobalsDAL = new loanGlobalsDAL();

            string Language = Convert.ToString(Session["Language"]);
            List<loanGlobalsDAL> lstGlobals = objGlobalsDAL.SelectAllGetUnVerifiedRecordsPageWise(Language);

            if (lstGlobals == null)
            {
                loanAppGlobals.ShowMessage(loanMessagesDAL.SelectAllFail, loanMessageIcon.Error);
                return;
            }
            lvVerifyMaster.DataSource = lstGlobals;
            lvVerifyMaster.DataBind();

        }
    }
}