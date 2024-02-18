using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using loanLibrary;

namespace abLOAN
{
    public partial class contractinstallmentreport : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                loanSessionsDAL.CheckSession();

                if (!Page.IsPostBack)
                {
                    //loanUser.CheckUserRights(loanUserRights.ViewContractInstallment);
                    loanUser.CheckRoleRights(loanRoleRights.ViewList);


                    GetBank();
                    GetPageDefaults();

                    loanSessionsDAL.RemoveSessionAllKeyValue();

                    FillContractInstallmentTran();
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
                pgrContractInstallmentTran.CurrentPage = 1;
                FillContractInstallmentTran();
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
                txtFilterContractTitle.Text = string.Empty;
                txtFilterInstallmentDateFrom.Text = loanGlobalsDAL.ConvertDateTimeToString(loanGlobalsDAL.GetCurrentDateTime().AddYears(-1), loanAppGlobals.DateFormat);
                txtFilterInstallmentDateTo.Text = loanGlobalsDAL.ConvertDateTimeToString(loanGlobalsDAL.GetCurrentDateTime(), loanAppGlobals.DateFormat);
                ddlFilterBank.SelectedIndex = 0;

                pgrContractInstallmentTran.CurrentPage = 1;
                FillContractInstallmentTran();
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

                //    loanUser.CheckRoleRights(loanRoleRights.AddRecord);


                //    loanContractInstallmentTranDAL objContractInstallmentTranDAL = new loanContractInstallmentTranDAL();
                //    objContractInstallmentTranDAL.linktoContractMasterId = Convert.ToInt32(ddlContract.SelectedValue);
                //    objContractInstallmentTranDAL.InstallmentDate = DateTime.ParseExact(txtInstallmentDate.Text, loanAppGlobals.DateFormat, System.Globalization.DateTimeFormatInfo.InvariantInfo);

                //    objContractInstallmentTranDAL.Notes = txtNotes.Text.Trim();

                //    objContractInstallmentTranDAL.UpdateDateTime = loanGlobalsDAL.GetCurrentDateTime();
                //    objContractInstallmentTranDAL.SessionId = ((loanUser)Session[loanSessionsDAL.UserSession]).SessionId;

                //    if (string.IsNullOrEmpty(hdnActionContractInstallment.Value))
                //    {
                //        loanRecordStatus rsStatus = objContractInstallmentTranDAL.InsertContractInstallmentTran();
                //        if (rsStatus == loanRecordStatus.Error)
                //        {
                //            loanAppGlobals.ShowMessage(loanMessagesDAL.InsertFail, loanMessageIcon.Error);
                //            return;
                //        }
                //        else if (rsStatus == loanRecordStatus.RecordAlreadyExist)
                //        {
                //            loanAppGlobals.ShowMessage(loanMessagesDAL.AlreadyExist, loanMessageIcon.Warning);
                //            hdnModelContractInstallment.Value = "show";
                //            return;
                //        }
                //        else if (rsStatus == loanRecordStatus.Success)
                //        {
                //            loanAppGlobals.ShowMessage(loanMessagesDAL.InsertSuccess, loanMessageIcon.Success);
                //            if (((Button)sender).ID.Equals("btnSaveAndNew"))
                //            {
                //                hdnModelContractInstallment.Value = "clear";
                //            }
                //            else
                //            {
                //                hdnModelContractInstallment.Value = "hide";
                //            }
                //            FillContractInstallmentTran();
                //        }
                //    }
                //    else
                //    {
                //        objContractInstallmentTranDAL.ContractInstallmentTranId = Convert.ToInt32(hdnContractInstallmentTranId.Value);
                //        loanRecordStatus rsStatus = objContractInstallmentTranDAL.UpdateContractInstallmentTran();
                //        if (rsStatus == loanRecordStatus.Error)
                //        {
                //            loanAppGlobals.ShowMessage(loanMessagesDAL.UpdateFail, loanMessageIcon.Error);
                //            return;
                //        }
                //        else if (rsStatus == loanRecordStatus.RecordAlreadyExist)
                //        {
                //            loanAppGlobals.ShowMessage(loanMessagesDAL.AlreadyExist, loanMessageIcon.Warning);
                //            hdnModelContractInstallment.Value = "show";
                //            return;
                //        }
                //        else if (rsStatus == loanRecordStatus.Success)
                //        {
                //            loanAppGlobals.ShowMessage(loanMessagesDAL.UpdateSuccess, loanMessageIcon.Success);
                //            hdnModelContractInstallment.Value = "hide";
                //            FillContractInstallmentTran();
                //        }
                //    }
            }
            catch (Exception ex)
            {
                loanAppGlobals.SaveError(ex);
            }
        }

        #region List Methods

        protected void lvContractInstallmentTran_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            try
            {
                if (e.Item.ItemType == ListViewItemType.DataItem)
                {
                    loanContractInstallmentTranDAL objContractInstallmentTranDAL = (loanContractInstallmentTranDAL)e.Item.DataItem;

                    Literal ltrlContract = (Literal)e.Item.FindControl("ltrlContract");
                    Literal ltrlBank = (Literal)e.Item.FindControl("ltrlBank");
                    Literal ltrlInstallmentDate = (Literal)e.Item.FindControl("ltrlInstallmentDate");
                    Literal ltrlPaymentType = (Literal)e.Item.FindControl("ltrlPaymentType");
                    Literal ltrlInstallmentAmount = (Literal)e.Item.FindControl("ltrlInstallmentAmount");

                    ltrlContract.Text = objContractInstallmentTranDAL.Contract;
                    ltrlBank.Text = objContractInstallmentTranDAL.Bank;
                    ltrlInstallmentDate.Text = loanGlobalsDAL.ConvertDateTimeToString(objContractInstallmentTranDAL.InstallmentDate, loanAppGlobals.DateFormat);
                    ltrlPaymentType.Text = objContractInstallmentTranDAL.PaymentType;
                    ltrlInstallmentAmount.Text = objContractInstallmentTranDAL.InstallmentAmount.ToString("0.00");
                }
            }
            catch (Exception ex)
            {
                loanAppGlobals.SaveError(ex);
            }
        }

        protected void pgrContractInstallmentTran_ItemCommand(object sender, EventArgs e)
        {
            try
            {
                FillContractInstallmentTran();
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
            txtFilterInstallmentDateFrom.Text = loanGlobalsDAL.ConvertDateTimeToString(loanGlobalsDAL.GetCurrentDateTime().AddYears(-1), loanAppGlobals.DateFormat);
            txtFilterInstallmentDateTo.Text = loanGlobalsDAL.ConvertDateTimeToString(loanGlobalsDAL.GetCurrentDateTime(), loanAppGlobals.DateFormat);

            if (loanSessionsDAL.GetSessionKeyValue("CurrentPageContractInstallment") != null)
            {
                pgrContractInstallmentTran.CurrentPage = Convert.ToInt16(loanSessionsDAL.GetSessionKeyValue("CurrentPageContractInstallment"));
            }
            if (loanSessionsDAL.GetSessionKeyValue("FilterContractInstallment") != null)
            {
                loanContractInstallmentTranDAL objContractInstallmentTranDAL = (loanContractInstallmentTranDAL)loanSessionsDAL.GetSessionKeyValue("FilterContractInstallment");
                txtFilterContractTitle.Text = objContractInstallmentTranDAL.ContractTitle;

                if (objContractInstallmentTranDAL.InstallmentDate != new DateTime())
                {
                    txtFilterInstallmentDateTo.Text = loanGlobalsDAL.ConvertDateTimeToString(objContractInstallmentTranDAL.InstallmentDate, loanAppGlobals.DateFormat);
                }
                ddlFilterBank.SelectedValue = objContractInstallmentTranDAL.linktoBankMasterId.ToString();
            }
        }

        private void FillContractInstallmentTran()
        {

            loanContractInstallmentTranDAL objContractInstallmentTranDAL = new loanContractInstallmentTranDAL();
            string IdNo = txtFilterCustomer.Text.Trim();
            if (IdNo.Contains("("))
            {
                IdNo = IdNo.Substring(IdNo.IndexOf("(") + 1, IdNo.IndexOf(")") - IdNo.IndexOf("(") - 1);
            }
            objContractInstallmentTranDAL.Customer = IdNo;
            objContractInstallmentTranDAL.ContractTitle = txtFilterContractTitle.Text.Trim();

            DateTime? InstallmentDateFrom = null;
            if (!string.IsNullOrEmpty(txtFilterInstallmentDateFrom.Text))
            {
                InstallmentDateFrom = DateTime.ParseExact(txtFilterInstallmentDateFrom.Text, loanAppGlobals.DateFormat, System.Globalization.DateTimeFormatInfo.InvariantInfo);
                objContractInstallmentTranDAL.InstallmentDate = DateTime.ParseExact(txtFilterInstallmentDateFrom.Text, loanAppGlobals.DateFormat, System.Globalization.DateTimeFormatInfo.InvariantInfo);
            }
            DateTime? InstallmentDateTo = null;
            if (!string.IsNullOrEmpty(txtFilterInstallmentDateTo.Text))
            {
                InstallmentDateTo = DateTime.ParseExact(txtFilterInstallmentDateTo.Text, loanAppGlobals.DateFormat, System.Globalization.DateTimeFormatInfo.InvariantInfo);
                objContractInstallmentTranDAL.InstallmentDate = DateTime.ParseExact(txtFilterInstallmentDateTo.Text, loanAppGlobals.DateFormat, System.Globalization.DateTimeFormatInfo.InvariantInfo);
            }
            objContractInstallmentTranDAL.linktoCompanyMasterId = ((loanUser)Session[loanSessionsDAL.UserSession]).CompanyMasterId;
            if (ddlFilterBank.SelectedValue != string.Empty)
            {
                objContractInstallmentTranDAL.linktoBankMasterId = Convert.ToInt32(ddlFilterBank.SelectedValue);
            }


            loanSessionsDAL.SetSessionKeyValue("FilterContractInstallment", objContractInstallmentTranDAL);
            loanSessionsDAL.SetSessionKeyValue("CurrentPageContractInstallment", pgrContractInstallmentTran.CurrentPage);

            int TotalRecords;
            List<loanContractInstallmentTranDAL> lstContractInstallmentTran = objContractInstallmentTranDAL.SelectAllContractInstallmentTranPageWise(pgrContractInstallmentTran.StartRowIndex, pgrContractInstallmentTran.PageSize, out TotalRecords, InstallmentDateFrom, InstallmentDateTo);
            pgrContractInstallmentTran.TotalRowCount = TotalRecords;

            if (lstContractInstallmentTran == null)
            {
                loanAppGlobals.ShowMessage(loanMessagesDAL.SelectAllFail, loanMessageIcon.Error);
                return;
            }

            if (lstContractInstallmentTran.Count == 0 && pgrContractInstallmentTran.TotalRowCount > 0)
            {
                pgrContractInstallmentTran_ItemCommand(pgrContractInstallmentTran, new EventArgs());
                return;
            }

            lvContractInstallmentTran.DataSource = lstContractInstallmentTran;
            lvContractInstallmentTran.DataBind();

            if (lstContractInstallmentTran.Count > 0)
            {
                int EndiIndex = pgrContractInstallmentTran.StartRowIndex + pgrContractInstallmentTran.PageSize < pgrContractInstallmentTran.TotalRowCount ? pgrContractInstallmentTran.StartRowIndex + pgrContractInstallmentTran.PageSize : pgrContractInstallmentTran.TotalRowCount;
                lblRecords.Text = string.Format(Resources.Messages.ResourceManager.GetString("Records"), pgrContractInstallmentTran.StartRowIndex + 1, EndiIndex, pgrContractInstallmentTran.TotalRowCount);
                lblRecords.Visible = true;
            }
            else
            {
                lblRecords.Visible = false;
            }

            if (pgrContractInstallmentTran.TotalRowCount <= pgrContractInstallmentTran.PageSize)
            {
                pgrContractInstallmentTran.Visible = false;
            }
            else
            {
                pgrContractInstallmentTran.Visible = true;
            }

        }


        private void GetBank()
        {
            ddlFilterBank.Items.Clear();
            ddlFilterBank.Items.Add(new System.Web.UI.WebControls.ListItem(loanDropDownItem.Select, ""));

            List<loanBankMasterDAL> lstBankMasterDAL = loanBankMasterDAL.SelectAllBankMasterBankName();
            if (lstBankMasterDAL == null)
            {
                loanAppGlobals.ShowMessage(loanMessagesDAL.SelectAllFail, loanMessageIcon.Error);
                return;
            }
            foreach (loanBankMasterDAL obj in lstBankMasterDAL)
            {
                ddlFilterBank.Items.Add(new System.Web.UI.WebControls.ListItem(obj.BankName, obj.BankMasterId.ToString()));
            }
        }

        private void GetContractInstallmentTran(int ContractInstallmentTranId)
        {
            loanUser.CheckRoleRights(loanRoleRights.ViewRecord);

            loanContractInstallmentTranDAL objContractInstallmentTranDAL = new loanContractInstallmentTranDAL();
            objContractInstallmentTranDAL.ContractInstallmentTranId = ContractInstallmentTranId;
            if (!objContractInstallmentTranDAL.SelectContractInstallmentTran())
            {
                loanAppGlobals.ShowMessage(loanMessagesDAL.SelectFail, loanMessageIcon.Error);
                return;
            }
            hdnContractInstallmentTranId.Value = objContractInstallmentTranDAL.ContractInstallmentTranId.ToString();
            ddlContract.SelectedIndex = ddlContract.Items.IndexOf(ddlContract.Items.FindByValue(objContractInstallmentTranDAL.linktoContractMasterId.ToString()));
            txtInstallmentDate.Text = loanGlobalsDAL.ConvertDateTimeToString(objContractInstallmentTranDAL.InstallmentDate, loanAppGlobals.DateFormat);

            txtNotes.Text = objContractInstallmentTranDAL.Notes;

            hdnModelContractInstallment.Value = "show";
            hdnActionContractInstallment.Value = "edit";
        }
        #endregion


    }
}
