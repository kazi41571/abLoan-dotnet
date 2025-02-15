using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using loanLibrary;

namespace abLOAN
{
    public partial class contractinstallment : BasePage
    {
        decimal totalAmount;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                loanSessionsDAL.CheckSession();

                if (!Page.IsPostBack)
                {
                    //loanUser.CheckUserRights(loanUserRights.ViewContractInstallment);
                    loanUser.CheckRoleRights(loanRoleRights.ViewList);

                    GetContractStatus();
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

        protected void btnDownload_Click(object sender, EventArgs e)
        {
            loanContractInstallmentTranDAL objContractInstallmentTranDAL = new loanContractInstallmentTranDAL();
            objContractInstallmentTranDAL.Customer = txtFilterCustomer.Text.Trim();
            objContractInstallmentTranDAL.ContractTitle = txtFilterContractTitle.Text.Trim();
            objContractInstallmentTranDAL.InstallmentDate = DateTime.ParseExact(txtFilterInstallmentDateFrom.Text, loanAppGlobals.DateFormat, System.Globalization.DateTimeFormatInfo.InvariantInfo);
           
            if(ddlFilterBank.SelectedValue    != string.Empty)
            {
                objContractInstallmentTranDAL.linktoBankMasterId = Convert.ToInt32(ddlFilterBank.SelectedValue);
            }
          

            objContractInstallmentTranDAL.linktoCompanyMasterId = ((loanUser)Session[loanSessionsDAL.UserSession]).CompanyMasterId;

            if (ddlFilterContractStatus.SelectedValue != string.Empty)
            {
                objContractInstallmentTranDAL.linktoContractStatusMasterId = Convert.ToInt32(ddlFilterContractStatus.SelectedValue);
            }

            int TotalRecords;
            List<loanContractInstallmentTranDAL> lstContractInstallmentTran = objContractInstallmentTranDAL.SelectAllContractInstallmentTranPageWise(0, int.MaxValue, out TotalRecords);

            if (lstContractInstallmentTran == null)
            {
                loanAppGlobals.ShowMessage(loanMessagesDAL.SelectAllFail, loanMessageIcon.Error);
                return;
            }

            string file = System.Configuration.ConfigurationManager.AppSettings["FileSavePath"] + "ContractInstallment.csv";

            var parameterNames = typeof(loanContractInstallmentTranDAL).GetProperties()
                .Where(p => !p.GetGetMethod().IsVirtual)
                .Select(p => p.Name)
                .ToArray();

            bool IsSuccess = loanAppGlobals.ExportCsv(lstContractInstallmentTran, parameterNames, parameterNames, file);
            if (!IsSuccess)
            {
                loanAppGlobals.ShowMessage(loanMessagesDAL.Exception, loanMessageIcon.Error);
                return;
            }

            try
            {
                loanAppGlobals.SendOutFile(file, Path.GetFileName(file));
            }
            catch (System.Threading.ThreadAbortException)
            {
                // This is expected when downloading files - ignore it
            }
            catch (Exception ex)
            {
                loanAppGlobals.SaveError(ex);
                loanAppGlobals.ShowMessage(loanMessagesDAL.Exception, loanMessageIcon.Error);
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
                txtFilterInstallmentDateFrom.Text = loanGlobalsDAL.ConvertDateTimeToString(new DateTime(2000, 01, 01), loanAppGlobals.DateFormat);
                txtFilterInstallmentDateTo.Text = loanGlobalsDAL.ConvertDateTimeToString(loanGlobalsDAL.GetCurrentDateTime(), loanAppGlobals.DateFormat);
                ddlFilterBank.SelectedIndex = 0;
                ddlFilterContractStatus.SelectedValue = loanContractStatus.Active.GetHashCode().ToString();

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
                loanContractInstallmentTranDAL objContractInstallmentTranDAL = new loanContractInstallmentTranDAL();
                objContractInstallmentTranDAL.linktoContractMasterId = Convert.ToInt32(ddlContract.SelectedValue);
                objContractInstallmentTranDAL.InstallmentDate = DateTime.ParseExact(txtInstallmentDate.Text, loanAppGlobals.DateFormat, System.Globalization.DateTimeFormatInfo.InvariantInfo);

                objContractInstallmentTranDAL.Notes = txtNotes.Text.Trim();

                objContractInstallmentTranDAL.UpdateDateTime = loanGlobalsDAL.GetCurrentDateTime();
                objContractInstallmentTranDAL.SessionId = ((loanUser)Session[loanSessionsDAL.UserSession]).SessionId;

                if (string.IsNullOrEmpty(hdnActionContractInstallment.Value))
                {

                    loanUser.CheckRoleRights(loanRoleRights.AddRecord);

                    loanRecordStatus rsStatus = objContractInstallmentTranDAL.InsertContractInstallmentTran();
                    if (rsStatus == loanRecordStatus.Error)
                    {
                        loanAppGlobals.ShowMessage(loanMessagesDAL.InsertFail, loanMessageIcon.Error);
                        return;
                    }
                    else if (rsStatus == loanRecordStatus.RecordAlreadyExist)
                    {
                        loanAppGlobals.ShowMessage(loanMessagesDAL.AlreadyExist, loanMessageIcon.Warning);
                        hdnModelContractInstallment.Value = "show";
                        return;
                    }
                    else if (rsStatus == loanRecordStatus.Success)
                    {
                        loanAppGlobals.ShowMessage(loanMessagesDAL.InsertSuccess, loanMessageIcon.Success);
                        if (((Button)sender).ID.Equals("btnSaveAndNew"))
                        {
                            hdnModelContractInstallment.Value = "clear";
                        }
                        else
                        {
                            hdnModelContractInstallment.Value = "hide";
                        }
                        FillContractInstallmentTran();
                    }
                }
                else
                {
                    loanUser.CheckRoleRights(loanRoleRights.EditRecord);

                    objContractInstallmentTranDAL.ContractInstallmentTranId = Convert.ToInt32(hdnContractInstallmentTranId.Value);
                    loanRecordStatus rsStatus = objContractInstallmentTranDAL.UpdateContractInstallmentTran();
                    if (rsStatus == loanRecordStatus.Error)
                    {
                        loanAppGlobals.ShowMessage(loanMessagesDAL.UpdateFail, loanMessageIcon.Error);
                        return;
                    }
                    else if (rsStatus == loanRecordStatus.RecordAlreadyExist)
                    {
                        loanAppGlobals.ShowMessage(loanMessagesDAL.AlreadyExist, loanMessageIcon.Warning);
                        hdnModelContractInstallment.Value = "show";
                        return;
                    }
                    else if (rsStatus == loanRecordStatus.Success)
                    {
                        loanAppGlobals.ShowMessage(loanMessagesDAL.UpdateSuccess, loanMessageIcon.Success);
                        hdnModelContractInstallment.Value = "hide";
                        FillContractInstallmentTran();
                    }
                }
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

                    Literal ltrlCustomer = (Literal)e.Item.FindControl("ltrlCustomer");
                    HyperLink hlnkCustomer = (HyperLink)e.Item.FindControl("hlnkCustomer");
                    Literal ltrlContract = (Literal)e.Item.FindControl("ltrlContract");
                    HyperLink hlnkContract = (HyperLink)e.Item.FindControl("hlnkContract");
                    Literal ltrlBank = (Literal)e.Item.FindControl("ltrlBank");
                    Literal ltrlPaymentType = (Literal)e.Item.FindControl("ltrlPaymentType");
                    Literal ltrlChequeNo = (Literal)e.Item.FindControl("ltrlChequeNo");
                    Literal ltrlChequeDate = (Literal)e.Item.FindControl("ltrlChequeDate");
                    Literal ltrlInstallmentDate = (Literal)e.Item.FindControl("ltrlInstallmentDate");
                    Literal ltrlInstallmentAmount = (Literal)e.Item.FindControl("ltrlInstallmentAmount");
                    Literal ltrlNotes = (Literal)e.Item.FindControl("ltrlNotes");

                    if (objContractInstallmentTranDAL.CustomerIsRedFlag == true)
                    {
                        ltrlCustomer.Text = "<span class='text-danger'>" + objContractInstallmentTranDAL.Customer + "</span>";
                        hlnkCustomer.Text = "<span class='text-danger'>" + objContractInstallmentTranDAL.Customer + "</span>";
                    }
                    else
                    {
                        ltrlCustomer.Text = objContractInstallmentTranDAL.Customer;
                        hlnkCustomer.Text = objContractInstallmentTranDAL.Customer;
                    }
                    if (objContractInstallmentTranDAL.CustomerLinks != "")
                    {
                        hlnkCustomer.NavigateUrl = objContractInstallmentTranDAL.CustomerLinks;
                        hlnkCustomer.ToolTip = objContractInstallmentTranDAL.CustomerLinks;
                        hlnkCustomer.Visible = true;
                        ltrlCustomer.Visible = false;
                    }
                    ltrlContract.Text = objContractInstallmentTranDAL.Contract;
                    hlnkContract.Text = objContractInstallmentTranDAL.Contract;
                    if (objContractInstallmentTranDAL.ContractLinks != "")
                    {
                        hlnkContract.NavigateUrl = objContractInstallmentTranDAL.ContractLinks;
                        hlnkContract.ToolTip = objContractInstallmentTranDAL.ContractLinks;
                        hlnkContract.Visible = true;
                        ltrlContract.Visible = false;
                    }
                    ltrlBank.Text = objContractInstallmentTranDAL.Bank;
                    ltrlPaymentType.Text = objContractInstallmentTranDAL.PaymentType;
                    ltrlChequeNo.Text = objContractInstallmentTranDAL.ChequeNo;
                    if (objContractInstallmentTranDAL.ChequeDate != null)
                    {
                        ltrlChequeDate.Text = loanGlobalsDAL.ConvertDateTimeToString(objContractInstallmentTranDAL.ChequeDate.Value, loanAppGlobals.DateFormat);
                    }
                    ltrlInstallmentDate.Text = loanGlobalsDAL.ConvertDateTimeToString(objContractInstallmentTranDAL.InstallmentDate, loanAppGlobals.DateFormat);
                    ltrlInstallmentAmount.Text = objContractInstallmentTranDAL.InstallmentAmount.ToString("0.00");
                    totalAmount += objContractInstallmentTranDAL.InstallmentAmount;
                    ltrlNotes.Text = objContractInstallmentTranDAL.Notes;
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

        //protected void lvContractInstallmentTran_ItemCommand(object sender, ListViewCommandEventArgs e)
        //{
        //    try
        //    {
        //        if (e.CommandName.Equals("EditRecord", StringComparison.CurrentCultureIgnoreCase))
        //        {
        //            GetContractInstallmentTran(Convert.ToInt32(((ListView)sender).DataKeys[e.Item.DataItemIndex].Value));
        //        }
        //        else if (e.CommandName.Equals("DeleteRecord", StringComparison.CurrentCultureIgnoreCase))
        //        {
        //            loanContractInstallmentTranDAL objContractInstallmentTranDAL = new loanContractInstallmentTranDAL();
        //            objContractInstallmentTranDAL.ContractInstallmentTranId = Convert.ToInt32(((ListView)sender).DataKeys[e.Item.DataItemIndex].Value);
        //            loanRecordStatus rsStatus = objContractInstallmentTranDAL.DeleteContractInstallmentTran();
        //            if (rsStatus == loanRecordStatus.Success)
        //            {
        //                loanAppGlobals.ShowMessage(loanMessagesDAL.DeleteSuccess, loanMessageIcon.Success);
        //                FillContractInstallmentTran();
        //            }
        //            else
        //            {
        //                loanAppGlobals.ShowMessage(loanMessagesDAL.DeleteFail, loanMessageIcon.Error);
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        loanAppGlobals.SaveError(ex);
        //    }
        //}

        #endregion

        #region Private Methods
        private void GetPageDefaults()
        {
            txtFilterInstallmentDateFrom.Text = loanGlobalsDAL.ConvertDateTimeToString(new DateTime(2000, 01, 01), loanAppGlobals.DateFormat);
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
                ddlFilterContractStatus.SelectedValue = objContractInstallmentTranDAL.linktoContractStatusMasterId.ToString();
            }
        }

     private void GetContractStatus()
        {

            loanUser.CheckRoleRights(loanRoleRights.ViewRecord);

            ddlFilterContractStatus.Items.Clear();
            ddlFilterContractStatus.Items.Add(new System.Web.UI.WebControls.ListItem(loanDropDownItem.Select, ""));

            List<loanContractStatusMasterDAL> lstContractStatusMasterDAL = loanContractStatusMasterDAL.SelectAllContractStatusMasterContractStatus();
            if (lstContractStatusMasterDAL == null)
            {
                loanAppGlobals.ShowMessage(loanMessagesDAL.SelectAllFail, loanMessageIcon.Error);
                return;
            }

            foreach (loanContractStatusMasterDAL obj in lstContractStatusMasterDAL)
            {
                ddlFilterContractStatus.Items.Add(new System.Web.UI.WebControls.ListItem(obj.ContractStatus, obj.ContractStatusMasterId.ToString()));
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

            if (ddlFilterContractStatus.SelectedValue != string.Empty)
            {
                objContractInstallmentTranDAL.linktoContractStatusMasterId = Convert.ToInt32(ddlFilterContractStatus.SelectedValue);
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

        protected void lvContractInstallmentTran_DataBound(object sender, EventArgs e)
        {
            if (((ListView)sender).Items.Count > 0)
            {

                Literal ltrlTotalAmount = (Literal)((ListView)sender).FindControl("ltrlTotalAmount");
                ltrlTotalAmount.Text = totalAmount.ToString("0.00");
            }
        }
    }
}
