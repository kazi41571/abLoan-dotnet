using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using loanLibrary;

namespace abLOAN
{
    public partial class paymenttype : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                //loanSessionsDAL.CheckSession();

                if (!Page.IsPostBack)
                {
                    //loanUser.CheckUserRights(loanUserRights.ViewPaymentType);
                    loanUser.CheckRoleRights(loanRoleRights.ViewList);


                    //loanSessionsDAL.RemoveSessionAllKeyValue();

                    FillPaymentTypeMaster();
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
                loanPaymentTypeMasterDAL objPaymentTypeMasterDAL = new loanPaymentTypeMasterDAL();
                objPaymentTypeMasterDAL.linktoCompanyMasterId = ((loanUser)Session[loanSessionsDAL.UserSession]).CompanyMasterId;
                objPaymentTypeMasterDAL.PaymentType = txtPaymentType.Text.Trim();
                objPaymentTypeMasterDAL.UpdateDateTime = loanGlobalsDAL.GetCurrentDateTime();
                objPaymentTypeMasterDAL.SessionId = ((loanUser)Session[loanSessionsDAL.UserSession]).SessionId;

                if (string.IsNullOrEmpty(hdnActionPaymentType.Value))
                {
                    loanUser.CheckRoleRights(loanRoleRights.AddRecord);

                    loanRecordStatus rsStatus = objPaymentTypeMasterDAL.InsertPaymentTypeMaster();
                    if (rsStatus == loanRecordStatus.Error)
                    {
                        loanAppGlobals.ShowMessage(loanMessagesDAL.InsertFail, loanMessageIcon.Error);
                        return;
                    }
                    else if (rsStatus == loanRecordStatus.RecordAlreadyExist)
                    {
                        loanAppGlobals.ShowMessage(loanMessagesDAL.AlreadyExist, loanMessageIcon.Warning);
                        hdnModelPaymentType.Value = "show";
                        return;
                    }
                    else if (rsStatus == loanRecordStatus.Success)
                    {
                        loanAppGlobals.ShowMessage(loanMessagesDAL.InsertSuccess, loanMessageIcon.Success);
                        if (((Button)sender).ID.Equals("btnSaveAndNew"))
                        {
                            hdnModelPaymentType.Value = "clear";
                        }
                        else
                        {
                            hdnModelPaymentType.Value = "hide";
                        }
                        FillPaymentTypeMaster();
                    }
                }
                else
                {
                    loanUser.CheckRoleRights(loanRoleRights.EditRecord);

                    objPaymentTypeMasterDAL.UpdateDateTime = loanGlobalsDAL.GetCurrentDateTime();
                    objPaymentTypeMasterDAL.PaymentTypeMasterId = Convert.ToInt32(hdnPaymentTypeMasterId.Value);
                    loanRecordStatus rsStatus = objPaymentTypeMasterDAL.UpdatePaymentTypeMaster();
                    if (rsStatus == loanRecordStatus.Error)
                    {
                        loanAppGlobals.ShowMessage(loanMessagesDAL.UpdateFail, loanMessageIcon.Error);
                        return;
                    }
                    else if (rsStatus == loanRecordStatus.RecordAlreadyExist)
                    {
                        loanAppGlobals.ShowMessage(loanMessagesDAL.AlreadyExist, loanMessageIcon.Warning);
                        hdnModelPaymentType.Value = "show";
                        return;
                    }
                    else if (rsStatus == loanRecordStatus.Success)
                    {
                        loanAppGlobals.ShowMessage(loanMessagesDAL.UpdateSuccess, loanMessageIcon.Success);
                        hdnModelPaymentType.Value = "hide";
                        FillPaymentTypeMaster();
                    }
                }
            }
            catch (Exception ex)
            {
                loanAppGlobals.SaveError(ex);
            }
        }

        #region List Methods

        protected void lvPaymentTypeMaster_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            try
            {
                if (e.Item.ItemType == ListViewItemType.DataItem)
                {
                    loanPaymentTypeMasterDAL objPaymentTypeMasterDAL = (loanPaymentTypeMasterDAL)e.Item.DataItem;

                    Literal ltrlPaymentType = (Literal)e.Item.FindControl("ltrlPaymentType");

                    ltrlPaymentType.Text = objPaymentTypeMasterDAL.PaymentType;
                }
            }
            catch (Exception ex)
            {
                loanAppGlobals.SaveError(ex);
            }
        }

        protected void pgrPaymentTypeMaster_ItemCommand(object sender, EventArgs e)
        {
            try
            {
                FillPaymentTypeMaster();
            }
            catch (Exception ex)
            {
                loanAppGlobals.SaveError(ex);
            }
        }

        protected void lvPaymentTypeMaster_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName.Equals("EditRecord", StringComparison.CurrentCultureIgnoreCase))
                {
                    GetPaymentTypeMaster(Convert.ToInt32(((ListView)sender).DataKeys[e.Item.DataItemIndex].Value));
                }
                else if (e.CommandName.Equals("DeleteRecord", StringComparison.CurrentCultureIgnoreCase))
                {
                    loanUser.CheckRoleRights(loanRoleRights.DeleteRecord);

                    loanPaymentTypeMasterDAL objPaymentTypeMasterDAL = new loanPaymentTypeMasterDAL();
                    objPaymentTypeMasterDAL.PaymentTypeMasterId = Convert.ToInt32(((ListView)sender).DataKeys[e.Item.DataItemIndex].Value);
                    loanRecordStatus rsStatus = objPaymentTypeMasterDAL.DeletePaymentTypeMaster();
                    if (rsStatus == loanRecordStatus.Success)
                    {
                        loanAppGlobals.ShowMessage(loanMessagesDAL.DeleteSuccess, loanMessageIcon.Success);
                        FillPaymentTypeMaster();
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
        private void FillPaymentTypeMaster()
        {

            loanPaymentTypeMasterDAL objPaymentTypeMasterDAL = new loanPaymentTypeMasterDAL();

            int TotalRecords;
            List<loanPaymentTypeMasterDAL> lstPaymentTypeMaster = objPaymentTypeMasterDAL.SelectAllPaymentTypeMasterPageWise(pgrPaymentTypeMaster.StartRowIndex, pgrPaymentTypeMaster.PageSize, out TotalRecords);
            pgrPaymentTypeMaster.TotalRowCount = TotalRecords;

            if (lstPaymentTypeMaster == null)
            {
                loanAppGlobals.ShowMessage(loanMessagesDAL.SelectAllFail, loanMessageIcon.Error);
                return;
            }

            if (lstPaymentTypeMaster.Count == 0 && pgrPaymentTypeMaster.TotalRowCount > 0)
            {
                pgrPaymentTypeMaster_ItemCommand(pgrPaymentTypeMaster, new EventArgs());
                return;
            }

            lvPaymentTypeMaster.DataSource = lstPaymentTypeMaster;
            lvPaymentTypeMaster.DataBind();

            if (lstPaymentTypeMaster.Count > 0)
            {
                int EndiIndex = pgrPaymentTypeMaster.StartRowIndex + pgrPaymentTypeMaster.PageSize < pgrPaymentTypeMaster.TotalRowCount ? pgrPaymentTypeMaster.StartRowIndex + pgrPaymentTypeMaster.PageSize : pgrPaymentTypeMaster.TotalRowCount;
                lblRecords.Text = string.Format(Resources.Messages.ResourceManager.GetString("Records"), pgrPaymentTypeMaster.StartRowIndex + 1, EndiIndex, pgrPaymentTypeMaster.TotalRowCount);
                lblRecords.Visible = true;
            }
            else
            {
                lblRecords.Visible = false;
            }

            if (pgrPaymentTypeMaster.TotalRowCount <= pgrPaymentTypeMaster.PageSize)
            {
                pgrPaymentTypeMaster.Visible = false;
            }
            else
            {
                pgrPaymentTypeMaster.Visible = true;
            }

        }


        private void GetPaymentTypeMaster(int PaymentTypeMasterId)
        {
            loanUser.CheckRoleRights(loanRoleRights.ViewRecord);

            loanPaymentTypeMasterDAL objPaymentTypeMasterDAL = new loanPaymentTypeMasterDAL();
            objPaymentTypeMasterDAL.PaymentTypeMasterId = PaymentTypeMasterId;
            if (!objPaymentTypeMasterDAL.SelectPaymentTypeMaster())
            {
                loanAppGlobals.ShowMessage(loanMessagesDAL.SelectFail, loanMessageIcon.Error);
                return;
            }
            hdnPaymentTypeMasterId.Value = objPaymentTypeMasterDAL.PaymentTypeMasterId.ToString();
            txtPaymentType.Text = objPaymentTypeMasterDAL.PaymentType;

            hdnModelPaymentType.Value = "show";
            hdnActionPaymentType.Value = "edit";
        }
        #endregion


    }
}
