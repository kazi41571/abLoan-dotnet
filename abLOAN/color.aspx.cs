using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using loanLibrary;

namespace abLOAN
{
    public partial class color : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                loanSessionsDAL.CheckSession();

                if (!Page.IsPostBack)
                {
                    //loanUser.CheckUserRights(loanUserRights.ViewColor);

                    GetPageDefaults();

                    loanSessionsDAL.RemoveSessionAllKeyValue();

                    FillColorMaster();
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
                pgrColorMaster.CurrentPage = 1;
                FillColorMaster();
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
                ddlFilterIsEnabled.SelectedIndex = 0;

                pgrColorMaster.CurrentPage = 1;
                FillColorMaster();
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
                loanColorMasterDAL objColorMasterDAL = new loanColorMasterDAL();
                objColorMasterDAL.ColorName = txtColorName.Text.Trim();
                objColorMasterDAL.IsEnabled = chkIsEnabled.Checked;
                objColorMasterDAL.UpdateDateTime = loanGlobalsDAL.GetCurrentDateTime();
                objColorMasterDAL.SessionId = ((loanUser)Session[loanSessionsDAL.UserSession]).SessionId;

                if (string.IsNullOrEmpty(hdnActionColor.Value))
                {
                    loanRecordStatus rsStatus = objColorMasterDAL.InsertColorMaster();
                    if (rsStatus == loanRecordStatus.Error)
                    {
                        loanAppGlobals.ShowMessage(loanMessagesDAL.InsertFail, loanMessageIcon.Error);
                        return;
                    }
                    else if (rsStatus == loanRecordStatus.RecordAlreadyExist)
                    {
                        loanAppGlobals.ShowMessage(loanMessagesDAL.AlreadyExist, loanMessageIcon.Warning);
                        hdnModelColor.Value = "show";
                        return;
                    }
                    else if (rsStatus == loanRecordStatus.Success)
                    {
                        loanAppGlobals.ShowMessage(loanMessagesDAL.InsertSuccess, loanMessageIcon.Success);
                        if (((Button)sender).ID.Equals("btnSaveAndNew"))
                        {
                            hdnModelColor.Value = "clear";
                        }
                        else
                        {
                            hdnModelColor.Value = "hide";
                        }
                        FillColorMaster();
                    }
                }
                else
                {
                    objColorMasterDAL.UpdateDateTime = loanGlobalsDAL.GetCurrentDateTime();
                    objColorMasterDAL.ColorMasterId = Convert.ToInt32(hdnColorMasterId.Value);
                    loanRecordStatus rsStatus = objColorMasterDAL.UpdateColorMaster();
                    if (rsStatus == loanRecordStatus.Error)
                    {
                        loanAppGlobals.ShowMessage(loanMessagesDAL.UpdateFail, loanMessageIcon.Error);
                        return;
                    }
                    else if (rsStatus == loanRecordStatus.RecordAlreadyExist)
                    {
                        loanAppGlobals.ShowMessage(loanMessagesDAL.AlreadyExist, loanMessageIcon.Warning);
                        hdnModelColor.Value = "show";
                        return;
                    }
                    else if (rsStatus == loanRecordStatus.Success)
                    {
                        loanAppGlobals.ShowMessage(loanMessagesDAL.UpdateSuccess, loanMessageIcon.Success);
                        hdnModelColor.Value = "hide";
                        FillColorMaster();
                    }
                }
            }
            catch (Exception ex)
            {
                loanAppGlobals.SaveError(ex);
            }
        }

        #region List Methods

        protected void lvColorMaster_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            try
            {
                if (e.Item.ItemType == ListViewItemType.DataItem)
                {
                    loanColorMasterDAL objColorMasterDAL = (loanColorMasterDAL)e.Item.DataItem;

                    //Label lblColorName = (Label)e.Item.FindControl("lblColorName");
                    //lblColorName.ForeColor = System.Drawing.ColorTranslator.FromHtml(objColorMasterDAL.ColorName);
                    Literal ltrlColorName = (Literal)e.Item.FindControl("ltrlColorName");

                    ltrlColorName.Text = objColorMasterDAL.ColorName;
                }
            }
            catch (Exception ex)
            {
                loanAppGlobals.SaveError(ex);
            }
        }

        protected void pgrColorMaster_ItemCommand(object sender, EventArgs e)
        {
            try
            {
                FillColorMaster();
            }
            catch (Exception ex)
            {
                loanAppGlobals.SaveError(ex);
            }
        }

        protected void lvColorMaster_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName.Equals("EditRecord", StringComparison.CurrentCultureIgnoreCase))
                {
                    GetColorMaster(Convert.ToInt32(((ListView)sender).DataKeys[e.Item.DataItemIndex].Value));
                }
                else if (e.CommandName.Equals("DeleteRecord", StringComparison.CurrentCultureIgnoreCase))
                {
                    loanColorMasterDAL objColorMasterDAL = new loanColorMasterDAL();
                    objColorMasterDAL.ColorMasterId = Convert.ToInt32(((ListView)sender).DataKeys[e.Item.DataItemIndex].Value);
                    loanRecordStatus rsStatus = objColorMasterDAL.DeleteColorMaster();
                    if (rsStatus == loanRecordStatus.Success)
                    {
                        loanAppGlobals.ShowMessage(loanMessagesDAL.DeleteSuccess, loanMessageIcon.Success);
                        FillColorMaster();
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
            if (loanSessionsDAL.GetSessionKeyValue("CurrentPageColor") != null)
            {
                pgrColorMaster.CurrentPage = Convert.ToInt16(loanSessionsDAL.GetSessionKeyValue("CurrentPageColor"));
            }
            if (loanSessionsDAL.GetSessionKeyValue("FilterColor") != null)
            {
                loanColorMasterDAL objColorMasterDAL = (loanColorMasterDAL)loanSessionsDAL.GetSessionKeyValue("FilterColor");
                if (objColorMasterDAL.IsEnabled)
                {
                    ddlFilterIsEnabled.SelectedValue = "Yes";
                }
                else
                {
                    ddlFilterIsEnabled.SelectedValue = "No";
                }
            }
        }

        private void FillColorMaster()
        {

            loanColorMasterDAL objColorMasterDAL = new loanColorMasterDAL();
            if (ddlFilterIsEnabled.SelectedValue == "Yes")
            {
                objColorMasterDAL.IsEnabled = true;
            }
            else if (ddlFilterIsEnabled.SelectedValue == "No")
            {
                objColorMasterDAL.IsEnabled = false;
            }

            loanSessionsDAL.SetSessionKeyValue("FilterColor", objColorMasterDAL);
            loanSessionsDAL.SetSessionKeyValue("CurrentPageColor", pgrColorMaster.CurrentPage);

            int TotalRecords;
            List<loanColorMasterDAL> lstColorMaster = objColorMasterDAL.SelectAllColorMasterPageWise(pgrColorMaster.StartRowIndex, pgrColorMaster.PageSize, out TotalRecords);
            pgrColorMaster.TotalRowCount = TotalRecords;

            if (lstColorMaster == null)
            {
                loanAppGlobals.ShowMessage(loanMessagesDAL.SelectAllFail, loanMessageIcon.Error);
                return;
            }

            if (lstColorMaster.Count == 0 && pgrColorMaster.TotalRowCount > 0)
            {
                pgrColorMaster_ItemCommand(pgrColorMaster, new EventArgs());
                return;
            }

            lvColorMaster.DataSource = lstColorMaster;
            lvColorMaster.DataBind();

            if (lstColorMaster.Count > 0)
            {
                int EndiIndex = pgrColorMaster.StartRowIndex + pgrColorMaster.PageSize < pgrColorMaster.TotalRowCount ? pgrColorMaster.StartRowIndex + pgrColorMaster.PageSize : pgrColorMaster.TotalRowCount;
                lblRecords.Text = string.Format(Resources.Messages.ResourceManager.GetString("Records"), pgrColorMaster.StartRowIndex + 1, EndiIndex, pgrColorMaster.TotalRowCount);
                lblRecords.Visible = true;
            }
            else
            {
                lblRecords.Visible = false;
            }

            if (pgrColorMaster.TotalRowCount <= pgrColorMaster.PageSize)
            {
                pgrColorMaster.Visible = false;
            }
            else
            {
                pgrColorMaster.Visible = true;
            }

        }


        private void GetColorMaster(int ColorMasterId)
        {
            loanColorMasterDAL objColorMasterDAL = new loanColorMasterDAL();
            objColorMasterDAL.ColorMasterId = ColorMasterId;
            if (!objColorMasterDAL.SelectColorMaster())
            {
                loanAppGlobals.ShowMessage(loanMessagesDAL.SelectFail, loanMessageIcon.Error);
                return;
            }
            hdnColorMasterId.Value = objColorMasterDAL.ColorMasterId.ToString();
            txtColorName.Text = objColorMasterDAL.ColorName;
            //lblColorName.ForeColor = System.Drawing.ColorTranslator.FromHtml(objColorMasterDAL.ColorName);
            chkIsEnabled.Checked = objColorMasterDAL.IsEnabled;

            hdnModelColor.Value = "show";
            hdnActionColor.Value = "edit";
        }
        #endregion


    }
}
