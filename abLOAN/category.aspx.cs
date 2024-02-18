using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using loanLibrary;

namespace abLOAN
{
    public partial class category : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                loanSessionsDAL.CheckSession();

                if (!Page.IsPostBack)
                {
                    //loanUser.CheckUserRights(loanUserRights.ViewCategory);

                    GetPageDefaults();

                    loanSessionsDAL.RemoveSessionAllKeyValue();

                    FillCategoryMaster();
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
                pgrCategoryMaster.CurrentPage = 1;
                FillCategoryMaster();
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
                txtFilterCategoryName.Text = string.Empty;
                ddlFilterIsEnabled.SelectedIndex = 0;

                pgrCategoryMaster.CurrentPage = 1;
                FillCategoryMaster();
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
                loanCategoryMasterDAL objCategoryMasterDAL = new loanCategoryMasterDAL();
                objCategoryMasterDAL.CategoryName = txtCategoryName.Text.Trim();
                objCategoryMasterDAL.IsEnabled = chkIsEnabled.Checked;

                objCategoryMasterDAL.UpdateDateTime = loanGlobalsDAL.GetCurrentDateTime();
                objCategoryMasterDAL.SessionId = ((loanUser)Session[loanSessionsDAL.UserSession]).SessionId;

                if (string.IsNullOrEmpty(hdnActionCategory.Value))
                {
                    loanRecordStatus rsStatus = objCategoryMasterDAL.InsertCategoryMaster();
                    if (rsStatus == loanRecordStatus.Error)
                    {
                        loanAppGlobals.ShowMessage(loanMessagesDAL.InsertFail, loanMessageIcon.Error);
                        return;
                    }
                    else if (rsStatus == loanRecordStatus.RecordAlreadyExist)
                    {
                        loanAppGlobals.ShowMessage(loanMessagesDAL.AlreadyExist, loanMessageIcon.Warning);
                        hdnModelCategory.Value = "show";
                        return;
                    }
                    else if (rsStatus == loanRecordStatus.Success)
                    {
                        loanAppGlobals.ShowMessage(loanMessagesDAL.InsertSuccess, loanMessageIcon.Success);
                        if (((Button)sender).ID.Equals("btnSaveAndNew"))
                        {
                            hdnModelCategory.Value = "clear";
                        }
                        else
                        {
                            hdnModelCategory.Value = "hide";
                        }
                        FillCategoryMaster();
                    }
                }
                else
                {
                    objCategoryMasterDAL.CategoryMasterId = Convert.ToInt32(hdnCategoryMasterId.Value);
                    loanRecordStatus rsStatus = objCategoryMasterDAL.UpdateCategoryMaster();
                    if (rsStatus == loanRecordStatus.Error)
                    {
                        loanAppGlobals.ShowMessage(loanMessagesDAL.UpdateFail, loanMessageIcon.Error);
                        return;
                    }
                    else if (rsStatus == loanRecordStatus.RecordAlreadyExist)
                    {
                        loanAppGlobals.ShowMessage(loanMessagesDAL.AlreadyExist, loanMessageIcon.Warning);
                        hdnModelCategory.Value = "show";
                        return;
                    }
                    else if (rsStatus == loanRecordStatus.Success)
                    {
                        loanAppGlobals.ShowMessage(loanMessagesDAL.UpdateSuccess, loanMessageIcon.Success);
                        hdnModelCategory.Value = "hide";
                        FillCategoryMaster();
                    }
                }
            }
            catch (Exception ex)
            {
                loanAppGlobals.SaveError(ex);
            }
        }

        #region List Methods

        protected void lvCategoryMaster_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            try
            {
                if (e.Item.ItemType == ListViewItemType.DataItem)
                {
                    loanCategoryMasterDAL objCategoryMasterDAL = (loanCategoryMasterDAL)e.Item.DataItem;

                    Literal ltrlCategoryName = (Literal)e.Item.FindControl("ltrlCategoryName");

                    ltrlCategoryName.Text = objCategoryMasterDAL.CategoryName;
                }
            }
            catch (Exception ex)
            {
                loanAppGlobals.SaveError(ex);
            }
        }

        protected void pgrCategoryMaster_ItemCommand(object sender, EventArgs e)
        {
            try
            {
                FillCategoryMaster();
            }
            catch (Exception ex)
            {
                loanAppGlobals.SaveError(ex);
            }
        }

        protected void lvCategoryMaster_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName.Equals("EditRecord", StringComparison.CurrentCultureIgnoreCase))
                {
                    GetCategoryMaster(Convert.ToInt32(((ListView)sender).DataKeys[e.Item.DataItemIndex].Value));
                }
                else if (e.CommandName.Equals("DeleteRecord", StringComparison.CurrentCultureIgnoreCase))
                {
                    loanCategoryMasterDAL objCategoryMasterDAL = new loanCategoryMasterDAL();
                    objCategoryMasterDAL.CategoryMasterId = Convert.ToInt32(((ListView)sender).DataKeys[e.Item.DataItemIndex].Value);
                    loanRecordStatus rsStatus = objCategoryMasterDAL.DeleteCategoryMaster();
                    if (rsStatus == loanRecordStatus.Success)
                    {
                        loanAppGlobals.ShowMessage(loanMessagesDAL.DeleteSuccess, loanMessageIcon.Success);
                        FillCategoryMaster();
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

        #endregion

        #region Private Methods
        private void GetPageDefaults()
        {
            if (loanSessionsDAL.GetSessionKeyValue("CurrentPageCategory") != null)
            {
                pgrCategoryMaster.CurrentPage = Convert.ToInt16(loanSessionsDAL.GetSessionKeyValue("CurrentPageCategory"));
            }
            if (loanSessionsDAL.GetSessionKeyValue("FilterCategory") != null)
            {
                loanCategoryMasterDAL objCategoryMasterDAL = (loanCategoryMasterDAL)loanSessionsDAL.GetSessionKeyValue("FilterCategory");
                txtFilterCategoryName.Text = objCategoryMasterDAL.CategoryName;
                if (objCategoryMasterDAL.IsEnabled)
                {
                    ddlFilterIsEnabled.SelectedValue = "Yes";
                }
                else
                {
                    ddlFilterIsEnabled.SelectedValue = "No";
                }
            }
        }

        private void FillCategoryMaster()
        {

            loanCategoryMasterDAL objCategoryMasterDAL = new loanCategoryMasterDAL();
            objCategoryMasterDAL.CategoryName = txtFilterCategoryName.Text.Trim();
            if (ddlFilterIsEnabled.SelectedValue == "Yes")
            {
                objCategoryMasterDAL.IsEnabled = true;
            }
            else if (ddlFilterIsEnabled.SelectedValue == "No")
            {
                objCategoryMasterDAL.IsEnabled = false;
            }

            loanSessionsDAL.SetSessionKeyValue("FilterCategory", objCategoryMasterDAL);
            loanSessionsDAL.SetSessionKeyValue("CurrentPageCategory", pgrCategoryMaster.CurrentPage);

            int TotalRecords;
            List<loanCategoryMasterDAL> lstCategoryMaster = objCategoryMasterDAL.SelectAllCategoryMasterPageWise(pgrCategoryMaster.StartRowIndex, pgrCategoryMaster.PageSize, out TotalRecords);
            pgrCategoryMaster.TotalRowCount = TotalRecords;

            if (lstCategoryMaster == null)
            {
                loanAppGlobals.ShowMessage(loanMessagesDAL.SelectAllFail, loanMessageIcon.Error);
                return;
            }

            if (lstCategoryMaster.Count == 0 && pgrCategoryMaster.TotalRowCount > 0)
            {
                pgrCategoryMaster_ItemCommand(pgrCategoryMaster, new EventArgs());
                return;
            }

            lvCategoryMaster.DataSource = lstCategoryMaster;
            lvCategoryMaster.DataBind();

            if (lstCategoryMaster.Count > 0)
            {
                int EndiIndex = pgrCategoryMaster.StartRowIndex + pgrCategoryMaster.PageSize < pgrCategoryMaster.TotalRowCount ? pgrCategoryMaster.StartRowIndex + pgrCategoryMaster.PageSize : pgrCategoryMaster.TotalRowCount;
                lblRecords.Text = "[" + (pgrCategoryMaster.StartRowIndex + 1) + " to " + EndiIndex + " of " + pgrCategoryMaster.TotalRowCount + " Records]";
                lblRecords.Visible = true;
            }
            else
            {
                lblRecords.Visible = false;
            }

            if (pgrCategoryMaster.TotalRowCount <= pgrCategoryMaster.PageSize)
            {
                pgrCategoryMaster.Visible = false;
            }
            else
            {
                pgrCategoryMaster.Visible = true;
            }

        }


        private void GetCategoryMaster(int CategoryMasterId)
        {
            loanCategoryMasterDAL objCategoryMasterDAL = new loanCategoryMasterDAL();
            objCategoryMasterDAL.CategoryMasterId = CategoryMasterId;
            if (!objCategoryMasterDAL.SelectCategoryMaster())
            {
                loanAppGlobals.ShowMessage(loanMessagesDAL.SelectFail, loanMessageIcon.Error);
                return;
            }
            hdnCategoryMasterId.Value = objCategoryMasterDAL.CategoryMasterId.ToString();
            txtCategoryName.Text = objCategoryMasterDAL.CategoryName;
            chkIsEnabled.Checked = objCategoryMasterDAL.IsEnabled;

            hdnModelCategory.Value = "show";
            hdnActionCategory.Value = "edit";
        }
        #endregion


    }
}
