using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using loanLibrary;

namespace abLOAN
{
    public partial class brand : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                loanSessionsDAL.CheckSession();

                if (!Page.IsPostBack)
                {
                    //loanUser.CheckUserRights(loanUserRights.ViewBrand);

                    GetPageDefaults();

                    loanSessionsDAL.RemoveSessionAllKeyValue();

                    FillBrandMaster();
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
                pgrBrandMaster.CurrentPage = 1;
                FillBrandMaster();
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
                txtFilterBrandName.Text = string.Empty;
                ddlFilterIsEnabled.SelectedIndex = 0;

                pgrBrandMaster.CurrentPage = 1;
                FillBrandMaster();
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
                loanBrandMasterDAL objBrandMasterDAL = new loanBrandMasterDAL();
                objBrandMasterDAL.BrandName = txtBrandName.Text.Trim();
                objBrandMasterDAL.IsEnabled = chkIsEnabled.Checked;

                objBrandMasterDAL.UpdateDateTime = loanGlobalsDAL.GetCurrentDateTime();
                objBrandMasterDAL.SessionId = ((loanUser)Session[loanSessionsDAL.UserSession]).SessionId;
                if (string.IsNullOrEmpty(hdnActionBrand.Value))
                {
                    loanRecordStatus rsStatus = objBrandMasterDAL.InsertBrandMaster();
                    if (rsStatus == loanRecordStatus.Error)
                    {
                        loanAppGlobals.ShowMessage(loanMessagesDAL.InsertFail, loanMessageIcon.Error);
                        return;
                    }
                    else if (rsStatus == loanRecordStatus.RecordAlreadyExist)
                    {
                        loanAppGlobals.ShowMessage(loanMessagesDAL.AlreadyExist, loanMessageIcon.Warning);
                        hdnModelBrand.Value = "show";
                        return;
                    }
                    else if (rsStatus == loanRecordStatus.Success)
                    {
                        loanAppGlobals.ShowMessage(loanMessagesDAL.InsertSuccess, loanMessageIcon.Success);
                        if (((Button)sender).ID.Equals("btnSaveAndNew"))
                        {
                            hdnModelBrand.Value = "clear";
                        }
                        else
                        {
                            hdnModelBrand.Value = "hide";
                        }
                        FillBrandMaster();
                    }
                }
                else
                {
                    objBrandMasterDAL.UpdateDateTime = loanGlobalsDAL.GetCurrentDateTime();
                    objBrandMasterDAL.BrandMasterId = Convert.ToInt32(hdnBrandMasterId.Value);
                    loanRecordStatus rsStatus = objBrandMasterDAL.UpdateBrandMaster();
                    if (rsStatus == loanRecordStatus.Error)
                    {
                        loanAppGlobals.ShowMessage(loanMessagesDAL.UpdateFail, loanMessageIcon.Error);
                        return;
                    }
                    else if (rsStatus == loanRecordStatus.RecordAlreadyExist)
                    {
                        loanAppGlobals.ShowMessage(loanMessagesDAL.AlreadyExist, loanMessageIcon.Warning);
                        hdnModelBrand.Value = "show";
                        return;
                    }
                    else if (rsStatus == loanRecordStatus.Success)
                    {
                        loanAppGlobals.ShowMessage(loanMessagesDAL.UpdateSuccess, loanMessageIcon.Success);
                        hdnModelBrand.Value = "hide";
                        FillBrandMaster();
                    }
                }
            }
            catch (Exception ex)
            {
                loanAppGlobals.SaveError(ex);
            }
        }

        #region List Methods

        protected void lvBrandMaster_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            try
            {
                if (e.Item.ItemType == ListViewItemType.DataItem)
                {
                    loanBrandMasterDAL objBrandMasterDAL = (loanBrandMasterDAL)e.Item.DataItem;

                    Literal ltrlBrandName = (Literal)e.Item.FindControl("ltrlBrandName");

                    ltrlBrandName.Text = objBrandMasterDAL.BrandName;
                }
            }
            catch (Exception ex)
            {
                loanAppGlobals.SaveError(ex);
            }
        }

        protected void pgrBrandMaster_ItemCommand(object sender, EventArgs e)
        {
            try
            {
                FillBrandMaster();
            }
            catch (Exception ex)
            {
                loanAppGlobals.SaveError(ex);
            }
        }

        protected void lvBrandMaster_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName.Equals("EditRecord", StringComparison.CurrentCultureIgnoreCase))
                {
                    GetBrandMaster(Convert.ToInt32(((ListView)sender).DataKeys[e.Item.DataItemIndex].Value));
                }
                else if (e.CommandName.Equals("DeleteRecord", StringComparison.CurrentCultureIgnoreCase))
                {
                    loanBrandMasterDAL objBrandMasterDAL = new loanBrandMasterDAL();
                    objBrandMasterDAL.BrandMasterId = Convert.ToInt32(((ListView)sender).DataKeys[e.Item.DataItemIndex].Value);
                    loanRecordStatus rsStatus = objBrandMasterDAL.DeleteBrandMaster();
                    if (rsStatus == loanRecordStatus.Success)
                    {
                        loanAppGlobals.ShowMessage(loanMessagesDAL.DeleteSuccess, loanMessageIcon.Success);
                        FillBrandMaster();
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
            if (loanSessionsDAL.GetSessionKeyValue("CurrentPageBrand") != null)
            {
                pgrBrandMaster.CurrentPage = Convert.ToInt16(loanSessionsDAL.GetSessionKeyValue("CurrentPageBrand"));
            }
            if (loanSessionsDAL.GetSessionKeyValue("FilterBrand") != null)
            {
                loanBrandMasterDAL objBrandMasterDAL = (loanBrandMasterDAL)loanSessionsDAL.GetSessionKeyValue("FilterBrand");
                txtFilterBrandName.Text = objBrandMasterDAL.BrandName;
                if (objBrandMasterDAL.IsEnabled)
                {
                    ddlFilterIsEnabled.SelectedValue = "Yes";
                }
                else
                {
                    ddlFilterIsEnabled.SelectedValue = "No";
                }
            }
        }

        private void FillBrandMaster()
        {

            loanBrandMasterDAL objBrandMasterDAL = new loanBrandMasterDAL();
            objBrandMasterDAL.BrandName = txtFilterBrandName.Text.Trim();
            if (ddlFilterIsEnabled.SelectedValue == "Yes")
            {
                objBrandMasterDAL.IsEnabled = true;
            }
            else if (ddlFilterIsEnabled.SelectedValue == "No")
            {
                objBrandMasterDAL.IsEnabled = false;
            }

            loanSessionsDAL.SetSessionKeyValue("FilterBrand", objBrandMasterDAL);
            loanSessionsDAL.SetSessionKeyValue("CurrentPageBrand", pgrBrandMaster.CurrentPage);

            int TotalRecords;
            List<loanBrandMasterDAL> lstBrandMaster = objBrandMasterDAL.SelectAllBrandMasterPageWise(pgrBrandMaster.StartRowIndex, pgrBrandMaster.PageSize, out TotalRecords);
            pgrBrandMaster.TotalRowCount = TotalRecords;

            if (lstBrandMaster == null)
            {
                loanAppGlobals.ShowMessage(loanMessagesDAL.SelectAllFail, loanMessageIcon.Error);
                return;
            }

            if (lstBrandMaster.Count == 0 && pgrBrandMaster.TotalRowCount > 0)
            {
                pgrBrandMaster_ItemCommand(pgrBrandMaster, new EventArgs());
                return;
            }

            lvBrandMaster.DataSource = lstBrandMaster;
            lvBrandMaster.DataBind();

            if (lstBrandMaster.Count > 0)
            {
                int EndiIndex = pgrBrandMaster.StartRowIndex + pgrBrandMaster.PageSize < pgrBrandMaster.TotalRowCount ? pgrBrandMaster.StartRowIndex + pgrBrandMaster.PageSize : pgrBrandMaster.TotalRowCount;
                lblRecords.Text = string.Format(Resources.Messages.ResourceManager.GetString("Records"), pgrBrandMaster.StartRowIndex + 1, EndiIndex, pgrBrandMaster.TotalRowCount);
                lblRecords.Visible = true;
            }
            else
            {
                lblRecords.Visible = false;
            }

            if (pgrBrandMaster.TotalRowCount <= pgrBrandMaster.PageSize)
            {
                pgrBrandMaster.Visible = false;
            }
            else
            {
                pgrBrandMaster.Visible = true;
            }

        }


        private void GetBrandMaster(int BrandMasterId)
        {
            loanBrandMasterDAL objBrandMasterDAL = new loanBrandMasterDAL();
            objBrandMasterDAL.BrandMasterId = BrandMasterId;
            if (!objBrandMasterDAL.SelectBrandMaster())
            {
                loanAppGlobals.ShowMessage(loanMessagesDAL.SelectFail, loanMessageIcon.Error);
                return;
            }
            hdnBrandMasterId.Value = objBrandMasterDAL.BrandMasterId.ToString();
            txtBrandName.Text = objBrandMasterDAL.BrandName;
            chkIsEnabled.Checked = objBrandMasterDAL.IsEnabled;

            hdnModelBrand.Value = "show";
            hdnActionBrand.Value = "edit";
        }
        #endregion


    }
}
