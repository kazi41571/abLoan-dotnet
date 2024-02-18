using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using loanLibrary;
using System.Linq;
using System.Runtime.InteropServices;

namespace abLOAN
{
    public partial class zakaahpayment : BasePage
    {
        //Double TotalAmount;
        List<loanContractMasterDAL> lstContractMaster;

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                loanSessionsDAL.CheckSession();

                if (!Page.IsPostBack)
                {
                    //loanUser.CheckUserRights(loanUserRights.ViewZakaahPayment);
                    loanUser.CheckRoleRights(loanRoleRights.ViewList);


                    GetPageDefaults();

                    //loanSessionsDAL.RemoveSessionAllKeyValue();

                    FillZakaahPaymentMaster();
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
                pgrZakaahPaymentMaster.CurrentPage = 1;
                FillZakaahPaymentMaster();
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
                txtFromDate.Text = loanGlobalsDAL.ConvertDateTimeToString(loanGlobalsDAL.GetCurrentDateTime().AddYears(-1), loanAppGlobals.DateFormat);
                txtToDate.Text = loanGlobalsDAL.ConvertDateTimeToString(loanGlobalsDAL.GetCurrentDateTime(), loanAppGlobals.DateFormat);

                pgrZakaahPaymentMaster.CurrentPage = 1;
                Session["lstContractMaster"] = null;
                FillZakaahPaymentMaster();
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
                loanZakaahPaymentMasterDAL objZakaahPaymentMasterDAL = new loanZakaahPaymentMasterDAL();
                objZakaahPaymentMasterDAL.linktoCompanyMasterId = ((loanUser)Session[loanSessionsDAL.UserSession]).CompanyMasterId;
                objZakaahPaymentMasterDAL.PaymentDate = DateTime.ParseExact(txtPaymentDate.Text, loanAppGlobals.DateFormat, System.Globalization.DateTimeFormatInfo.InvariantInfo);
                if (!string.IsNullOrEmpty(txtFromDate.Text))
                {
                    objZakaahPaymentMasterDAL.FromDate = DateTime.ParseExact(txtFromDate.Text, loanAppGlobals.DateFormat, System.Globalization.DateTimeFormatInfo.InvariantInfo);
                }
                if (!string.IsNullOrEmpty(txtToDate.Text))
                {
                    objZakaahPaymentMasterDAL.ToDate = DateTime.ParseExact(txtToDate.Text, loanAppGlobals.DateFormat, System.Globalization.DateTimeFormatInfo.InvariantInfo);
                }
                if (!string.IsNullOrEmpty(txtActiveDate.Text))
                {
                    objZakaahPaymentMasterDAL.ActiveDate = DateTime.ParseExact(txtActiveDate.Text, loanAppGlobals.DateFormat, System.Globalization.DateTimeFormatInfo.InvariantInfo);
                }
                objZakaahPaymentMasterDAL.PendingAmount = Convert.ToDecimal(txtPendingAmount.Text);
                objZakaahPaymentMasterDAL.ZakaahAmount = Convert.ToDecimal(txtZakaahAmount.Text);

                objZakaahPaymentMasterDAL.UpdateDateTime = loanGlobalsDAL.GetCurrentDateTime();
                objZakaahPaymentMasterDAL.SessionId = ((loanUser)Session[loanSessionsDAL.UserSession]).SessionId;

                if (string.IsNullOrEmpty(hdnActionZakaahPayment.Value))
                {
                    loanUser.CheckRoleRights(loanRoleRights.AddRecord);

                    loanRecordStatus rsStatus = objZakaahPaymentMasterDAL.InsertZakaahPaymentMaster();
                    if (rsStatus == loanRecordStatus.Error)
                    {
                        loanAppGlobals.ShowMessage(loanMessagesDAL.InsertFail, loanMessageIcon.Error);
                        return;
                    }
                    else if (rsStatus == loanRecordStatus.RecordAlreadyExist)
                    {
                        loanAppGlobals.ShowMessage(loanMessagesDAL.AlreadyExist, loanMessageIcon.Warning);
                        hdnModelZakaahPayment.Value = "show";
                        return;
                    }
                    else if (rsStatus == loanRecordStatus.Success)
                    {
                        loanAppGlobals.ShowMessage(loanMessagesDAL.InsertSuccess, loanMessageIcon.Success);
                        if (((Button)sender).ID.Equals("btnSaveAndNew"))
                        {
                            hdnModelZakaahPayment.Value = "clear";
                            btnClearList_Click(btnClearList, new EventArgs());
                        }
                        else
                        {
                            hdnModelZakaahPayment.Value = "hide";
                        }
                        FillZakaahPaymentMaster();
                    }
                }
                else
                {
                    loanUser.CheckRoleRights(loanRoleRights.EditRecord);

                    objZakaahPaymentMasterDAL.ZakaahPaymentMasterId = Convert.ToInt32(hdnZakaahPaymentMasterId.Value);
                    loanRecordStatus rsStatus = objZakaahPaymentMasterDAL.UpdateZakaahPaymentMaster();
                    if (rsStatus == loanRecordStatus.Error)
                    {
                        loanAppGlobals.ShowMessage(loanMessagesDAL.UpdateFail, loanMessageIcon.Error);
                        return;
                    }
                    else if (rsStatus == loanRecordStatus.RecordAlreadyExist)
                    {
                        loanAppGlobals.ShowMessage(loanMessagesDAL.AlreadyExist, loanMessageIcon.Warning);
                        hdnModelZakaahPayment.Value = "show";
                        return;
                    }
                    else if (rsStatus == loanRecordStatus.Success)
                    {
                        loanAppGlobals.ShowMessage(loanMessagesDAL.UpdateSuccess, loanMessageIcon.Success);
                        hdnModelZakaahPayment.Value = "hide";
                        FillZakaahPaymentMaster();
                    }
                }


            }

            catch (Exception ex)
            {
                loanAppGlobals.SaveError(ex);
            }
        }

        //protected void ddlMonths_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    FillContractMaster();
        //    txtFromDate.Text = loanGlobalsDAL.ConvertDateTimeToString(loanGlobalsDAL.GetCurrentDateTime().AddMonths(-(Convert.ToInt32(ddlMonths.SelectedValue))), loanAppGlobals.DateFormat);
        //    txtToDate.Text = loanGlobalsDAL.ConvertDateTimeToString(loanGlobalsDAL.GetCurrentDateTime(), loanAppGlobals.DateFormat);
        //}

        protected void btnClearList_Click(object sender, EventArgs e)
        {
            lvContractMaster.DataSource = null;
            lvContractMaster.DataBind();

            hdnActionZakaahPayment.Value = "";

            txtPaymentDate.Text = loanGlobalsDAL.ConvertDateTimeToString(loanGlobalsDAL.GetCurrentDateTime(), loanAppGlobals.DateFormat);
            txtFromDate.Text = loanGlobalsDAL.ConvertDateTimeToString(loanGlobalsDAL.GetCurrentDateTime().AddYears(-1), loanAppGlobals.DateFormat);
            txtToDate.Text = loanGlobalsDAL.ConvertDateTimeToString(loanGlobalsDAL.GetCurrentDateTime(), loanAppGlobals.DateFormat);
            txtActiveDate.Text = loanGlobalsDAL.ConvertDateTimeToString(loanGlobalsDAL.GetCurrentDateTime().AddYears(-1), loanAppGlobals.DateFormat);
            txtPendingAmount.Text = "";
            //ddlMonths.SelectedIndex = 0;
            txtZakaahAmount.Text = "";
        }

        protected void lbtnShowList_Click(object sender, EventArgs e)
        {
            FillContractMaster();

            lstContractMaster = (List<loanContractMasterDAL>)Session["lstContractMaster"];
            lvContractMaster.DataSource = lstContractMaster;
            lvContractMaster.DataBind();

        }

        #region List Methods
        protected void lvZakaahPaymentMaster_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            try
            {
                if (e.Item.ItemType == ListViewItemType.DataItem)
                {
                    loanZakaahPaymentMasterDAL objZakaahPaymentMasterDAL = (loanZakaahPaymentMasterDAL)e.Item.DataItem;

                    Literal ltrlZakaah = (Literal)e.Item.FindControl("ltrlZakaah");
                    Literal ltrlPaymentDate = (Literal)e.Item.FindControl("ltrlPaymentDate");
                    Literal ltrlFromDate = (Literal)e.Item.FindControl("ltrlFromDate");
                    Literal ltrlToDate = (Literal)e.Item.FindControl("ltrlToDate");
                    Literal ltrlPendingAmount = (Literal)e.Item.FindControl("ltrlPendingAmount");
                    Literal ltrlZakaahAmount = (Literal)e.Item.FindControl("ltrlZakaahAmount");

                    ltrlPaymentDate.Text = loanGlobalsDAL.ConvertDateTimeToString(objZakaahPaymentMasterDAL.PaymentDate, loanAppGlobals.DateFormat);
                    ltrlFromDate.Text = loanGlobalsDAL.ConvertDateTimeToString(objZakaahPaymentMasterDAL.FromDate, loanAppGlobals.DateFormat);
                    ltrlToDate.Text = loanGlobalsDAL.ConvertDateTimeToString(objZakaahPaymentMasterDAL.ToDate, loanAppGlobals.DateFormat);
                    ltrlPendingAmount.Text = objZakaahPaymentMasterDAL.PendingAmount.ToString("0.00");
                    ltrlZakaahAmount.Text = objZakaahPaymentMasterDAL.ZakaahAmount.ToString("0.00");
                }
            }
            catch (Exception ex)
            {
                loanAppGlobals.SaveError(ex);
            }
        }

        protected void pgrZakaahPaymentMaster_ItemCommand(object sender, EventArgs e)
        {
            try
            {
                FillZakaahPaymentMaster();
            }
            catch (Exception ex)
            {
                loanAppGlobals.SaveError(ex);
            }
        }

        protected void lvZakaahPaymentMaster_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName.Equals("EditRecord", StringComparison.CurrentCultureIgnoreCase))
                {
                    GetZakaahPaymentMaster(Convert.ToInt32(((ListView)sender).DataKeys[e.Item.DataItemIndex].Value));
                }
                else if (e.CommandName.Equals("DeleteRecord", StringComparison.CurrentCultureIgnoreCase))
                {
                    loanUser.CheckRoleRights(loanRoleRights.DeleteRecord);

                    loanZakaahPaymentMasterDAL objZakaahPaymentMasterDAL = new loanZakaahPaymentMasterDAL();
                    objZakaahPaymentMasterDAL.ZakaahPaymentMasterId = Convert.ToInt32(((ListView)sender).DataKeys[e.Item.DataItemIndex].Value);
                    loanRecordStatus rsStatus = objZakaahPaymentMasterDAL.DeleteZakaahPaymentMaster();
                    if (rsStatus == loanRecordStatus.Success)
                    {
                        loanAppGlobals.ShowMessage(loanMessagesDAL.DeleteSuccess, loanMessageIcon.Success);
                        FillZakaahPaymentMaster();
                        btnClearList_Click(btnClearList, new EventArgs());
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

        protected void lvContractMaster_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            try
            {
                if (e.Item.ItemType == ListViewItemType.DataItem)
                {
                    loanContractMasterDAL objContractMasterDAL = (loanContractMasterDAL)e.Item.DataItem;

                    Literal ltrlCustomerName = (Literal)e.Item.FindControl("ltrlCustomerName");
                    Literal ltrlCustomerIdNo = (Literal)e.Item.FindControl("ltrlCustomerIdNo");
                    Literal ltrlFileNo = (Literal)e.Item.FindControl("ltrlFileNo");
                    Literal ltrlContractTitle = (Literal)e.Item.FindControl("ltrlContractTitle");
                    Literal ltrlBank = (Literal)e.Item.FindControl("ltrlBank");
                    Literal ltrlContractDate = (Literal)e.Item.FindControl("ltrlContractDate");
                    Literal ltrlContractStartDate = (Literal)e.Item.FindControl("ltrlContractStartDate");
                    Literal ltrlContractEndDate = (Literal)e.Item.FindControl("ltrlContractEndDate");
                    Literal ltrlContractAmount = (Literal)e.Item.FindControl("ltrlContractAmount");
                    Literal ltrlPaidAmount = (Literal)e.Item.FindControl("ltrlPaidAmount");
                    Literal ltrlPendingAmount = (Literal)e.Item.FindControl("ltrlPendingAmount");
                    Literal ltrlDownPayment = (Literal)e.Item.FindControl("ltrlDownPayment");
                    Literal ltrlNoOfInstallments = (Literal)e.Item.FindControl("ltrlNoOfInstallments");
                    Literal ltrlInstallmentAmount = (Literal)e.Item.FindControl("ltrlInstallmentAmount");
                    Literal ltrlInstallmentDate = (Literal)e.Item.FindControl("ltrlInstallmentDate");
                    Label lblDueAmount = (Label)e.Item.FindControl("lblDueAmount");
                    Label lblSettlementAmount = (Label)e.Item.FindControl("lblSettlementAmount");
                    Literal ltrlSettlementReason = (Literal)e.Item.FindControl("ltrlSettlementReason");
                    Literal ltrlNotes = (Literal)e.Item.FindControl("ltrlNotes");
                    Literal ltrlLastPaidDate = (Literal)e.Item.FindControl("ltrlLastPaidDate");

                    ltrlCustomerName.Text = objContractMasterDAL.Customer;
                    ltrlCustomerIdNo.Text = objContractMasterDAL.CustomerIdNo;
                    ltrlFileNo.Text = objContractMasterDAL.FileNo.ToString();
                    ltrlContractTitle.Text = objContractMasterDAL.ContractTitle;
                    ltrlBank.Text = objContractMasterDAL.Bank;
                    ltrlContractDate.Text = loanGlobalsDAL.ConvertDateTimeToString(objContractMasterDAL.ContractDate, loanAppGlobals.DateFormat);
                    ltrlContractStartDate.Text = loanGlobalsDAL.ConvertDateTimeToString(objContractMasterDAL.ContractStartDate, loanAppGlobals.DateFormat);
                    ltrlContractEndDate.Text = loanGlobalsDAL.ConvertDateTimeToString(objContractMasterDAL.ContractStartDate.AddMonths(objContractMasterDAL.NoOfInstallments), loanAppGlobals.DateFormat);
                    ltrlContractAmount.Text = objContractMasterDAL.ContractAmount.ToString("0.00");
                    ltrlPaidAmount.Text = objContractMasterDAL.TotalPaid.ToString("0.00");
                    ltrlPendingAmount.Text = objContractMasterDAL.PendingAmount.ToString("0.00");
                    ltrlDownPayment.Text = objContractMasterDAL.DownPayment.ToString("0.00");
                    ltrlNoOfInstallments.Text = objContractMasterDAL.NoOfInstallments.ToString();
                    ltrlInstallmentAmount.Text = objContractMasterDAL.InstallmentAmount.ToString("0.00");
                    ltrlInstallmentDate.Text = loanGlobalsDAL.ConvertDateTimeToString(objContractMasterDAL.InstallmentDate, loanAppGlobals.DateFormat);
                    lblDueAmount.Text = objContractMasterDAL.DueAmount.ToString("0.00");
                    lblSettlementAmount.Text = objContractMasterDAL.SettlementAmount.ToString("0.00");
                    ltrlSettlementReason.Text = objContractMasterDAL.SettlementReason;
                    ltrlNotes.Text = objContractMasterDAL.Notes;
                    ltrlLastPaidDate.Text = loanGlobalsDAL.ConvertDateTimeToString(objContractMasterDAL.LastPaidDate, loanAppGlobals.DateFormat);

                    // TotalAmount += Convert.ToDouble(ltrlPendingAmount.Text);
                }
                //  txtPendingAmount.Text = TotalAmount.ToString("0.00");
                // txtZakaahAmount.Text = (TotalAmount / 40).ToString("0.00");
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
            txtFilterFromDate.Text = loanGlobalsDAL.ConvertDateTimeToString(loanGlobalsDAL.GetCurrentDateTime().AddYears(-1), loanAppGlobals.DateFormat);
            txtFilterToDate.Text = loanGlobalsDAL.ConvertDateTimeToString(loanGlobalsDAL.GetCurrentDateTime(), loanAppGlobals.DateFormat);

            btnClearList_Click(btnClearList, new EventArgs());

            if (loanSessionsDAL.GetSessionKeyValue("CurrentPageZakaahPayment") != null)
            {
                pgrZakaahPaymentMaster.CurrentPage = Convert.ToInt16(loanSessionsDAL.GetSessionKeyValue("CurrentPageZakaahPayment"));
            }
            if (loanSessionsDAL.GetSessionKeyValue("FilterZakaahPayment") != null)
            {
                loanZakaahPaymentMasterDAL objZakaahPaymentMasterDAL = (loanZakaahPaymentMasterDAL)loanSessionsDAL.GetSessionKeyValue("FilterZakaahPayment");
            }
        }

        private void FillZakaahPaymentMaster()
        {

            loanZakaahPaymentMasterDAL objZakaahPaymentMasterDAL = new loanZakaahPaymentMasterDAL();
            objZakaahPaymentMasterDAL.linktoCompanyMasterId = ((loanUser)Session[loanSessionsDAL.UserSession]).CompanyMasterId;
            if (!string.IsNullOrEmpty(txtFilterFromDate.Text))
            {
                objZakaahPaymentMasterDAL.FromDate = DateTime.ParseExact(txtFilterFromDate.Text, loanAppGlobals.DateFormat, System.Globalization.DateTimeFormatInfo.InvariantInfo);
            }
            if (!string.IsNullOrEmpty(txtFilterToDate.Text))
            {
                objZakaahPaymentMasterDAL.ToDate = DateTime.ParseExact(txtFilterToDate.Text, loanAppGlobals.DateFormat, System.Globalization.DateTimeFormatInfo.InvariantInfo);
            }
            loanSessionsDAL.SetSessionKeyValue("FilterZakaahPayment", objZakaahPaymentMasterDAL);
            loanSessionsDAL.SetSessionKeyValue("CurrentPageZakaahPayment", pgrZakaahPaymentMaster.CurrentPage);

            int TotalRecords;
            List<loanZakaahPaymentMasterDAL> lstZakaahPaymentMaster = objZakaahPaymentMasterDAL.SelectAllZakaahPaymentMasterPageWise(pgrZakaahPaymentMaster.StartRowIndex, pgrZakaahPaymentMaster.PageSize, out TotalRecords);
            pgrZakaahPaymentMaster.TotalRowCount = TotalRecords;

            if (lstZakaahPaymentMaster == null)
            {
                loanAppGlobals.ShowMessage(loanMessagesDAL.SelectAllFail, loanMessageIcon.Error);
                return;
            }

            if (lstZakaahPaymentMaster.Count == 0 && pgrZakaahPaymentMaster.TotalRowCount > 0)
            {
                pgrZakaahPaymentMaster_ItemCommand(pgrZakaahPaymentMaster, new EventArgs());
                return;
            }

            lvZakaahPaymentMaster.DataSource = lstZakaahPaymentMaster;
            lvZakaahPaymentMaster.DataBind();

            if (lstZakaahPaymentMaster.Count > 0)
            {
                int EndiIndex = pgrZakaahPaymentMaster.StartRowIndex + pgrZakaahPaymentMaster.PageSize < pgrZakaahPaymentMaster.TotalRowCount ? pgrZakaahPaymentMaster.StartRowIndex + pgrZakaahPaymentMaster.PageSize : pgrZakaahPaymentMaster.TotalRowCount;
                lblRecords.Text = string.Format(Resources.Messages.ResourceManager.GetString("Records"), pgrZakaahPaymentMaster.StartRowIndex + 1, EndiIndex, pgrZakaahPaymentMaster.TotalRowCount);
                lblRecords.Visible = true;
            }
            else
            {
                lblRecords.Visible = false;
            }

            if (pgrZakaahPaymentMaster.TotalRowCount <= pgrZakaahPaymentMaster.PageSize)
            {
                pgrZakaahPaymentMaster.Visible = false;
            }
            else
            {
                pgrZakaahPaymentMaster.Visible = true;
            }

        }

        private void GetZakaahPaymentMaster(int ZakaahPaymentMasterId)
        {
            loanUser.CheckRoleRights(loanRoleRights.ViewRecord);

            loanZakaahPaymentMasterDAL objZakaahPaymentMasterDAL = new loanZakaahPaymentMasterDAL();
            objZakaahPaymentMasterDAL.ZakaahPaymentMasterId = ZakaahPaymentMasterId;
            if (!objZakaahPaymentMasterDAL.SelectZakaahPaymentMaster())
            {
                loanAppGlobals.ShowMessage(loanMessagesDAL.SelectFail, loanMessageIcon.Error);
                return;
            }
            hdnZakaahPaymentMasterId.Value = objZakaahPaymentMasterDAL.ZakaahPaymentMasterId.ToString();
            txtPaymentDate.Text = loanGlobalsDAL.ConvertDateTimeToString(objZakaahPaymentMasterDAL.PaymentDate, loanAppGlobals.DateFormat);
            txtFromDate.Text = loanGlobalsDAL.ConvertDateTimeToString(objZakaahPaymentMasterDAL.FromDate, loanAppGlobals.DateFormat);
            txtToDate.Text = loanGlobalsDAL.ConvertDateTimeToString(objZakaahPaymentMasterDAL.ToDate, loanAppGlobals.DateFormat);
            //ddlMonths.SelectedValue = objZakaahPaymentMasterDAL.ActiveSinceMonths.ToString();
            txtPendingAmount.Text = objZakaahPaymentMasterDAL.PendingAmount.ToString("0.00");
            txtZakaahAmount.Text = objZakaahPaymentMasterDAL.ZakaahAmount.ToString("0.00");

            hdnModelZakaahPayment.Value = "show";
            hdnActionZakaahPayment.Value = "edit";
        }

        private void FillContractMaster()
        {
            lstContractMaster = null;
            Session["lstContractMaster"] = null;
            lvContractMaster.DataSource = lstContractMaster;
            lvContractMaster.DataBind();

            loanContractMasterDAL objContractMasterDAL = new loanContractMasterDAL();

            //objContractMasterDAL.linktoCustomerMasterId = Convert.ToInt32(hdnCustomer.Value);
            //objContractMasterDAL.LastPaidDate = DateTime.ParseExact(loanGlobalsDAL.ConvertDateTimeToString(loanGlobalsDAL.GetCurrentDateTime().AddMonths(-(Convert.ToInt32(ddlMonths.SelectedValue))), loanAppGlobals.DateFormat), loanAppGlobals.DateFormat, System.Globalization.DateTimeFormatInfo.InvariantInfo);
            //objContractMasterDAL.LastPaidDate = DateTime.ParseExact(txtToDate.Text, loanAppGlobals.DateFormat, System.Globalization.DateTimeFormatInfo.InvariantInfo).AddMonths(-Convert.ToInt32(ddlMonths.SelectedValue));

            DateTime ContractDateFrom = DateTime.ParseExact(txtFromDate.Text, loanAppGlobals.DateFormat, System.Globalization.DateTimeFormatInfo.InvariantInfo);
            DateTime ContractDateTo = DateTime.ParseExact(txtToDate.Text, loanAppGlobals.DateFormat, System.Globalization.DateTimeFormatInfo.InvariantInfo);
            //objContractMasterDAL.InstallmentDate = DateTime.ParseExact(txtToDate.Text, loanAppGlobals.DateFormat, System.Globalization.DateTimeFormatInfo.InvariantInfo);
            objContractMasterDAL.LastPaidDate = DateTime.ParseExact(txtActiveDate.Text, loanAppGlobals.DateFormat, System.Globalization.DateTimeFormatInfo.InvariantInfo);
            objContractMasterDAL.linktoCompanyMasterId = ((loanUser)Session[loanSessionsDAL.UserSession]).CompanyMasterId;

            loanSessionsDAL.SetSessionKeyValue("FilterContract", objContractMasterDAL);

            int TotalRecords;
            lstContractMaster = objContractMasterDAL.SelectAllContractMasterPageWise(0, int.MaxValue, out TotalRecords, ContractDateFrom, ContractDateTo, false, 0, ContractDateFrom, ContractDateTo);

            if (lstContractMaster == null)
            {
                loanAppGlobals.ShowMessage(loanMessagesDAL.SelectAllFail, loanMessageIcon.Error);
                return;
            }

            Session["lstContractMaster"] = lstContractMaster;

            var totalPending = lstContractMaster.Sum(x => x.PendingAmount);
            txtPendingAmount.Text = totalPending.ToString("0.00");
            txtZakaahAmount.Text = (totalPending / 40).ToString("0.00");
        }

        #endregion
    }
}
