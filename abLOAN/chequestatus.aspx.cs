using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using loanLibrary;

namespace abLOAN
{
    public partial class chequestatus : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                loanSessionsDAL.CheckSession();

                if (!Page.IsPostBack)
                {
                    //loanUser.CheckUserRights(loanUserRights.ViewChequeStatus);
                    loanUser.CheckRoleRights(loanRoleRights.ViewList);


                    //GetCompany();

                    loanSessionsDAL.RemoveSessionAllKeyValue();

                    FillChequeStatusMaster();
                }
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
                loanChequeStatusMasterDAL objChequeStatusMasterDAL = new loanChequeStatusMasterDAL();
                objChequeStatusMasterDAL.linktoCompanyMasterId = ((loanUser)Session[loanSessionsDAL.UserSession]).CompanyMasterId;
                objChequeStatusMasterDAL.ChequeStatusName = txtChequeStatusName.Text.Trim();

                objChequeStatusMasterDAL.UpdateDateTime = loanGlobalsDAL.GetCurrentDateTime();
                objChequeStatusMasterDAL.SessionId = ((loanUser)Session[loanSessionsDAL.UserSession]).SessionId;

                if (string.IsNullOrEmpty(hdnActionChequeStatus.Value))
                {

                    loanUser.CheckRoleRights(loanRoleRights.AddRecord);

                    loanRecordStatus rsStatus = objChequeStatusMasterDAL.InsertChequeStatusMaster();
                    if (rsStatus == loanRecordStatus.Error)
                    {
                        loanAppGlobals.ShowMessage(loanMessagesDAL.InsertFail, loanMessageIcon.Error);
                        return;
                    }
                    else if (rsStatus == loanRecordStatus.RecordAlreadyExist)
                    {
                        loanAppGlobals.ShowMessage(loanMessagesDAL.AlreadyExist, loanMessageIcon.Warning);
                        hdnModelChequeStatus.Value = "show";
                        return;
                    }
                    else if (rsStatus == loanRecordStatus.Success)
                    {
                        loanAppGlobals.ShowMessage(loanMessagesDAL.InsertSuccess, loanMessageIcon.Success);
                        if (((Button)sender).ID.Equals("btnSaveAndNew"))
                        {
                            hdnModelChequeStatus.Value = "clear";
                        }
                        else
                        {
                            hdnModelChequeStatus.Value = "hide";
                        }
                        FillChequeStatusMaster();
                    }
                }
                else
                {

                    loanUser.CheckRoleRights(loanRoleRights.EditRecord);

                    objChequeStatusMasterDAL.ChequeStatusMasterId = Convert.ToInt32(hdnChequeStatusMasterId.Value);
                    loanRecordStatus rsStatus = objChequeStatusMasterDAL.UpdateChequeStatusMaster();
                    if (rsStatus == loanRecordStatus.Error)
                    {
                        loanAppGlobals.ShowMessage(loanMessagesDAL.UpdateFail, loanMessageIcon.Error);
                        return;
                    }
                    else if (rsStatus == loanRecordStatus.RecordAlreadyExist)
                    {
                        loanAppGlobals.ShowMessage(loanMessagesDAL.AlreadyExist, loanMessageIcon.Warning);
                        hdnModelChequeStatus.Value = "show";
                        return;
                    }
                    else if (rsStatus == loanRecordStatus.Success)
                    {
                        loanAppGlobals.ShowMessage(loanMessagesDAL.UpdateSuccess, loanMessageIcon.Success);
                        hdnModelChequeStatus.Value = "hide";
                        FillChequeStatusMaster();
                    }
                }
            }
            catch (Exception ex)
            {
                loanAppGlobals.SaveError(ex);
            }
        }

        #region List Methods

        protected void lvChequeStatusMaster_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            try
            {
                if (e.Item.ItemType == ListViewItemType.DataItem)
                {
                    loanChequeStatusMasterDAL objChequeStatusMasterDAL = (loanChequeStatusMasterDAL)e.Item.DataItem;


                    Literal ltrlChequeStatusName = (Literal)e.Item.FindControl("ltrlChequeStatusName");


                    ltrlChequeStatusName.Text = objChequeStatusMasterDAL.ChequeStatusName;
                }
            }
            catch (Exception ex)
            {
                loanAppGlobals.SaveError(ex);
            }
        }

        protected void pgrChequeStatusMaster_ItemCommand(object sender, EventArgs e)
        {
            try
            {
                FillChequeStatusMaster();
            }
            catch (Exception ex)
            {
                loanAppGlobals.SaveError(ex);
            }
        }

        protected void lvChequeStatusMaster_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName.Equals("EditRecord", StringComparison.CurrentCultureIgnoreCase))
                {
                    GetChequeStatusMaster(Convert.ToInt32(((ListView)sender).DataKeys[e.Item.DataItemIndex].Value));
                }
                else if (e.CommandName.Equals("DeleteRecord", StringComparison.CurrentCultureIgnoreCase))
                {
                    loanUser.CheckRoleRights(loanRoleRights.DeleteRecord);

                    loanChequeStatusMasterDAL objChequeStatusMasterDAL = new loanChequeStatusMasterDAL();
                    objChequeStatusMasterDAL.ChequeStatusMasterId = Convert.ToInt32(((ListView)sender).DataKeys[e.Item.DataItemIndex].Value);
                    loanRecordStatus rsStatus = objChequeStatusMasterDAL.DeleteChequeStatusMaster();
                    if (rsStatus == loanRecordStatus.Success)
                    {
                        loanAppGlobals.ShowMessage(loanMessagesDAL.DeleteSuccess, loanMessageIcon.Success);
                        FillChequeStatusMaster();
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
        private void FillChequeStatusMaster()
        {

            loanChequeStatusMasterDAL objChequeStatusMasterDAL = new loanChequeStatusMasterDAL();

            int TotalRecords;
            List<loanChequeStatusMasterDAL> lstChequeStatusMaster = objChequeStatusMasterDAL.SelectAllChequeStatusMasterPageWise(pgrChequeStatusMaster.StartRowIndex, pgrChequeStatusMaster.PageSize, out TotalRecords);
            pgrChequeStatusMaster.TotalRowCount = TotalRecords;

            if (lstChequeStatusMaster == null)
            {
                loanAppGlobals.ShowMessage(loanMessagesDAL.SelectAllFail, loanMessageIcon.Error);
                return;
            }

            if (lstChequeStatusMaster.Count == 0 && pgrChequeStatusMaster.TotalRowCount > 0)
            {
                pgrChequeStatusMaster_ItemCommand(pgrChequeStatusMaster, new EventArgs());
                return;
            }

            lvChequeStatusMaster.DataSource = lstChequeStatusMaster;
            lvChequeStatusMaster.DataBind();

            if (lstChequeStatusMaster.Count > 0)
            {
                int EndiIndex = pgrChequeStatusMaster.StartRowIndex + pgrChequeStatusMaster.PageSize < pgrChequeStatusMaster.TotalRowCount ? pgrChequeStatusMaster.StartRowIndex + pgrChequeStatusMaster.PageSize : pgrChequeStatusMaster.TotalRowCount;
                lblRecords.Text = "[" + (pgrChequeStatusMaster.StartRowIndex + 1) + " to " + EndiIndex + " of " + pgrChequeStatusMaster.TotalRowCount + " Records]";
                lblRecords.Visible = true;
            }
            else
            {
                lblRecords.Visible = false;
            }

            if (pgrChequeStatusMaster.TotalRowCount <= pgrChequeStatusMaster.PageSize)
            {
                pgrChequeStatusMaster.Visible = false;
            }
            else
            {
                pgrChequeStatusMaster.Visible = true;
            }

        }



        private void GetChequeStatusMaster(int ChequeStatusMasterId)
        {

            loanUser.CheckRoleRights(loanRoleRights.ViewRecord);

            loanChequeStatusMasterDAL objChequeStatusMasterDAL = new loanChequeStatusMasterDAL();
            objChequeStatusMasterDAL.ChequeStatusMasterId = ChequeStatusMasterId;
            if (!objChequeStatusMasterDAL.SelectChequeStatusMaster())
            {
                loanAppGlobals.ShowMessage(loanMessagesDAL.SelectFail, loanMessageIcon.Error);
                return;
            }
            hdnChequeStatusMasterId.Value = objChequeStatusMasterDAL.ChequeStatusMasterId.ToString();

            txtChequeStatusName.Text = objChequeStatusMasterDAL.ChequeStatusName;

            hdnModelChequeStatus.Value = "show";
            hdnActionChequeStatus.Value = "edit";
        }
        #endregion


    }
}
