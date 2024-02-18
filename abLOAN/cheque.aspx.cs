using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using loanLibrary;

namespace abLOAN
{
    public partial class cheque : BasePage
    {
        //List<loanChequeStatusMasterDAL> lstLvChequeStatusMasterDAL;

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                loanSessionsDAL.CheckSession();

                if (!Page.IsPostBack)
                {
                    //loanUser.CheckUserRights(loanUserRights.ViewCheque);
                    loanUser.CheckRoleRights(loanRoleRights.ViewList);

                    GetBank();
                    //GetCompany();
                    // GetCustomer();
                    GetChequeStatus();

                    GetPageDefaults();

                    loanSessionsDAL.RemoveSessionAllKeyValue();

                    FillChequeMaster();
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
                pgrChequeMaster.CurrentPage = 1;
                FillChequeMaster();
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
                txtFilterChequeNo.Text = string.Empty;
                ddlFilterBank.SelectedIndex = 0;
                txtFilterCustomer.Text = string.Empty;
                ddlFilterChequeStatus.SelectedIndex = 0;

                pgrChequeMaster.CurrentPage = 1;
                FillChequeMaster();
            }
            catch (Exception ex)
            {
                loanAppGlobals.SaveError(ex);
            }
        }

        //protected void btnSave_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        loanChequeMasterDAL objChequeMasterDAL = new loanChequeMasterDAL();
        //        objChequeMasterDAL.ChequeName = txtChequeName.Text.Trim();
        //        objChequeMasterDAL.ChequeNo = txtChequeNo.Text.Trim();
        //        objChequeMasterDAL.ChequeDate = DateTime.ParseExact(txtChequeDate.Text, loanAppGlobals.DateFormat, System.Globalization.DateTimeFormatInfo.InvariantInfo);
        //        objChequeMasterDAL.linktoBankMasterId = Convert.ToInt32(ddlBank.SelectedValue);
        //        objChequeMasterDAL.linktoCompanyMasterId = ((loanUser)Session[loanSessionsDAL.UserSession]).CompanyMasterId;
        //        objChequeMasterDAL.linktoCustomerMasterId = Convert.ToInt32(hdnCustomerMasterId.Value); //Convert.ToInt32(ddlCustomer.SelectedValue);
        //        objChequeMasterDAL.InstallmentAmount = Convert.ToDecimal(txtInstallmentAmount.Text);
        //        objChequeMasterDAL.ChequeAmount = Convert.ToDecimal(txtChequeAmount.Text);
        //        objChequeMasterDAL.GivenTo = txtGivenTo.Text.Trim();
        //        objChequeMasterDAL.Notes = txtNotes.Text.Trim();

        //        objChequeMasterDAL.linktoChequeStatusMasterId = Convert.ToInt32(ddlChequeStatus.SelectedValue);


        //        objChequeMasterDAL.UpdateDateTime = loanGlobalsDAL.GetCurrentDateTime();
        //        objChequeMasterDAL.SessionId = ((loanUser)Session[loanSessionsDAL.UserSession]).SessionId;

        //        if (string.IsNullOrEmpty(hdnActionCheque.Value))
        //        {
        //            loanRecordStatus rsStatus = objChequeMasterDAL.InsertChequeMaster();
        //            if (rsStatus == loanRecordStatus.Error)
        //            {
        //                loanAppGlobals.ShowMessage(loanMessagesDAL.InsertFail, loanMessageIcon.Error);
        //                return;
        //            }
        //            else if (rsStatus == loanRecordStatus.RecordAlreadyExist)
        //            {
        //                loanAppGlobals.ShowMessage(loanMessagesDAL.AlreadyExist, loanMessageIcon.Warning);
        //                hdnModelCheque.Value = "show";
        //                return;
        //            }
        //            else if (rsStatus == loanRecordStatus.Success)
        //            {
        //                loanAppGlobals.ShowMessage(loanMessagesDAL.InsertSuccess, loanMessageIcon.Success);
        //                if (((Button)sender).ID.Equals("btnSaveAndNew"))
        //                {
        //                    hdnModelCheque.Value = "clear";
        //                }
        //                else
        //                {
        //                    hdnModelCheque.Value = "hide";
        //                }
        //                FillChequeMaster();
        //            }
        //        }
        //        else
        //        {
        //            objChequeMasterDAL.ChequeMasterId = Convert.ToInt32(hdnChequeMasterId.Value);
        //            loanRecordStatus rsStatus = objChequeMasterDAL.UpdateChequeMaster();
        //            if (rsStatus == loanRecordStatus.Error)
        //            {
        //                loanAppGlobals.ShowMessage(loanMessagesDAL.UpdateFail, loanMessageIcon.Error);
        //                return;
        //            }
        //            else if (rsStatus == loanRecordStatus.RecordAlreadyExist)
        //            {
        //                loanAppGlobals.ShowMessage(loanMessagesDAL.AlreadyExist, loanMessageIcon.Warning);
        //                hdnModelCheque.Value = "show";
        //                return;
        //            }
        //            else if (rsStatus == loanRecordStatus.Success)
        //            {
        //                loanAppGlobals.ShowMessage(loanMessagesDAL.UpdateSuccess, loanMessageIcon.Success);
        //                hdnModelCheque.Value = "hide";
        //                FillChequeMaster();
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        loanAppGlobals.SaveError(ex);
        //    }
        //}

        #region List Methods

        protected void lvChequeMaster_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            try
            {
                if (e.Item.ItemType == ListViewItemType.DataItem)
                {
                    loanChequeMasterDAL objChequeMasterDAL = (loanChequeMasterDAL)e.Item.DataItem;

                    HiddenField hdnChequeMasterId = (HiddenField)e.Item.FindControl("hdnChequeMasterId");
                    Literal ltrlChequeName = (Literal)e.Item.FindControl("ltrlChequeName");
                    Literal ltrlChequeDate = (Literal)e.Item.FindControl("ltrlChequeDate");
                    HiddenField hdnBankMasterId = (HiddenField)e.Item.FindControl("hdnBankMasterId");
                    Literal ltrlBank = (Literal)e.Item.FindControl("ltrlBank");
                    HiddenField hdnCustomerMasterId = (HiddenField)e.Item.FindControl("hdnCustomerMasterId");
                    Literal ltrlCustomer = (Literal)e.Item.FindControl("ltrlCustomer");
                    Literal ltrlInstallmentAmount = (Literal)e.Item.FindControl("ltrlInstallmentAmount");
                    Literal ltrlGivenTo = (Literal)e.Item.FindControl("ltrlGivenTo");
                    Literal ltrlNotes = (Literal)e.Item.FindControl("ltrlNotes");

                    TextBox txtChequeAmount = (TextBox)e.Item.FindControl("txtChequeAmount");
                    TextBox txtChequeNo = (TextBox)e.Item.FindControl("txtChequeNo");

                    DropDownList ddlChequeStatus = new DropDownList();
                    ddlChequeStatus = (DropDownList)e.Item.FindControl("ddlChequeStatus");

                    ddlChequeStatus.DataSource = (List<loanChequeStatusMasterDAL>)Session["lstChequeStatusMasterDAL"];
                    ddlChequeStatus.DataTextField = "ChequeStatusName";
                    ddlChequeStatus.DataValueField = "ChequeStatusMasterId";
                    ddlChequeStatus.DataBind();

                    hdnChequeMasterId.Value = objChequeMasterDAL.ChequeMasterId.ToString();
                    ltrlChequeName.Text = objChequeMasterDAL.ChequeName;
                    ltrlChequeDate.Text = loanGlobalsDAL.ConvertDateTimeToString(objChequeMasterDAL.ChequeDate, loanAppGlobals.DateFormat);
                    hdnBankMasterId.Value = objChequeMasterDAL.linktoBankMasterId.ToString();
                    ltrlBank.Text = objChequeMasterDAL.Bank;
                    hdnCustomerMasterId.Value = objChequeMasterDAL.linktoCustomerMasterId.ToString();
                    ltrlCustomer.Text = objChequeMasterDAL.Customer;

                    ltrlGivenTo.Text = objChequeMasterDAL.GivenTo;
                    ltrlNotes.Text = objChequeMasterDAL.Notes;

                    txtChequeAmount.Text = objChequeMasterDAL.ChequeAmount.ToString("0.00");
                    txtChequeNo.Text = objChequeMasterDAL.ChequeNo;
                    ddlChequeStatus.SelectedValue = objChequeMasterDAL.linktoChequeStatusMasterId.ToString();


                }
            }
            catch (Exception ex)
            {
                loanAppGlobals.SaveError(ex);
            }
        }

        protected void pgrChequeMaster_ItemCommand(object sender, EventArgs e)
        {
            try
            {
                FillChequeMaster();
            }
            catch (Exception ex)
            {
                loanAppGlobals.SaveError(ex);
            }
        }

        protected void lvChequeMaster_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName.Equals("EditRecord", StringComparison.CurrentCultureIgnoreCase))
                {
                    //GetChequeMaster(Convert.ToInt32(((ListView)sender).DataKeys[e.Item.DataItemIndex].Value));
                }
                else if (e.CommandName.Equals("DeleteRecord", StringComparison.CurrentCultureIgnoreCase))
                {
                    loanChequeMasterDAL objChequeMasterDAL = new loanChequeMasterDAL();
                    objChequeMasterDAL.ChequeMasterId = Convert.ToInt32(((ListView)sender).DataKeys[e.Item.DataItemIndex].Value);
                    loanRecordStatus rsStatus = objChequeMasterDAL.DeleteChequeMaster();
                    if (rsStatus == loanRecordStatus.Success)
                    {
                        loanAppGlobals.ShowMessage(loanMessagesDAL.DeleteSuccess, loanMessageIcon.Success);
                        FillChequeMaster();
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
            if (loanSessionsDAL.GetSessionKeyValue("CurrentPageCheque") != null)
            {
                pgrChequeMaster.CurrentPage = Convert.ToInt16(loanSessionsDAL.GetSessionKeyValue("CurrentPageCheque"));
            }
            if (loanSessionsDAL.GetSessionKeyValue("FilterCheque") != null)
            {
                loanChequeMasterDAL objChequeMasterDAL = (loanChequeMasterDAL)loanSessionsDAL.GetSessionKeyValue("FilterCheque");
                txtFilterChequeNo.Text = objChequeMasterDAL.ChequeNo;
                ddlFilterBank.SelectedValue = objChequeMasterDAL.linktoBankMasterId.ToString();
                txtFilterCustomer.Text = objChequeMasterDAL.Customer;
                ddlFilterChequeStatus.SelectedValue = objChequeMasterDAL.linktoChequeStatusMasterId.ToString();
            }
        }

        private void FillChequeMaster()
        {

            loanChequeMasterDAL objChequeMasterDAL = new loanChequeMasterDAL();
            objChequeMasterDAL.ChequeNo = txtFilterChequeNo.Text.Trim();
            if (ddlFilterBank.SelectedValue != string.Empty)
            {
                objChequeMasterDAL.linktoBankMasterId = Convert.ToInt32(ddlFilterBank.SelectedValue);
            }
            string IdNo = txtFilterCustomer.Text.Trim();
            if (IdNo.Contains("("))
            {
                IdNo = IdNo.Substring(IdNo.IndexOf("(") + 1, IdNo.IndexOf(")") - IdNo.IndexOf("(") - 1);
            }
            objChequeMasterDAL.Customer = IdNo;
            if (ddlFilterChequeStatus.SelectedValue != string.Empty)
            {
                objChequeMasterDAL.linktoChequeStatusMasterId = Convert.ToInt32(ddlFilterChequeStatus.SelectedValue);
            }

            loanSessionsDAL.SetSessionKeyValue("FilterCheque", objChequeMasterDAL);
            loanSessionsDAL.SetSessionKeyValue("CurrentPageCheque", pgrChequeMaster.CurrentPage);

            int TotalRecords;
            List<loanChequeMasterDAL> lstChequeMaster = objChequeMasterDAL.SelectAllChequeMasterPageWise(pgrChequeMaster.StartRowIndex, pgrChequeMaster.PageSize, out TotalRecords);
            pgrChequeMaster.TotalRowCount = TotalRecords;

            if (lstChequeMaster == null)
            {
                loanAppGlobals.ShowMessage(loanMessagesDAL.SelectAllFail, loanMessageIcon.Error);
                return;
            }

            if (lstChequeMaster.Count == 0 && pgrChequeMaster.TotalRowCount > 0)
            {
                pgrChequeMaster_ItemCommand(pgrChequeMaster, new EventArgs());
                return;
            }

            lvChequeMaster.DataSource = lstChequeMaster;
            lvChequeMaster.DataBind();

            if (lstChequeMaster.Count > 0)
            {
                int EndiIndex = pgrChequeMaster.StartRowIndex + pgrChequeMaster.PageSize < pgrChequeMaster.TotalRowCount ? pgrChequeMaster.StartRowIndex + pgrChequeMaster.PageSize : pgrChequeMaster.TotalRowCount;
                lblRecords.Text = "[" + (pgrChequeMaster.StartRowIndex + 1) + " to " + EndiIndex + " of " + pgrChequeMaster.TotalRowCount + " Records]";
                lblRecords.Visible = true;
            }
            else
            {
                lblRecords.Visible = false;
            }

            if (pgrChequeMaster.TotalRowCount <= pgrChequeMaster.PageSize)
            {
                pgrChequeMaster.Visible = false;
            }
            else
            {
                pgrChequeMaster.Visible = true;
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



        //private void GetCustomer()
        //{
        //    ddlCustomer.Items.Clear();
        //    ddlCustomer.Items.Add(new System.Web.UI.WebControls.ListItem(loanDropDownItem.Select, ""));


        //    List<loanCustomerMasterDAL> lstCustomerMasterDAL = loanCustomerMasterDAL.SelectAllCustomerMasterCustomerName();
        //    if (lstCustomerMasterDAL == null)
        //    {
        //        loanAppGlobals.ShowMessage(loanMessagesDAL.SelectAllFail, loanMessageIcon.Error);
        //        return;
        //    }
        //    foreach (loanCustomerMasterDAL obj in lstCustomerMasterDAL)
        //    {
        //        ddlCustomer.Items.Add(new System.Web.UI.WebControls.ListItem(obj.CustomerName, obj.CustomerMasterId.ToString()));

        //    }
        //}

        private void GetChequeStatus()
        {
            //ddlChequeStatus.Items.Clear();
            //ddlChequeStatus.Items.Add(new System.Web.UI.WebControls.ListItem(loanDropDownItem.Select, ""));

            ddlFilterChequeStatus.Items.Clear();
            ddlFilterChequeStatus.Items.Add(new System.Web.UI.WebControls.ListItem(loanDropDownItem.Select, ""));

            List<loanChequeStatusMasterDAL> lstChequeStatusMasterDAL = loanChequeStatusMasterDAL.SelectAllChequeStatusMasterChequeStatusName();
            if (lstChequeStatusMasterDAL == null)
            {
                loanAppGlobals.ShowMessage(loanMessagesDAL.SelectAllFail, loanMessageIcon.Error);
                return;
            }
            foreach (loanChequeStatusMasterDAL obj in lstChequeStatusMasterDAL)
            {
                //ddlChequeStatus.Items.Add(new System.Web.UI.WebControls.ListItem(obj.ChequeStatusName, obj.ChequeStatusMasterId.ToString()));
                ddlFilterChequeStatus.Items.Add(new System.Web.UI.WebControls.ListItem(obj.ChequeStatusName, obj.ChequeStatusMasterId.ToString()));

            }
            Session["lstChequeStatusMasterDAL"] = lstChequeStatusMasterDAL;
        }


        //private void GetChequeMaster(int ChequeMasterId)
        //{
        //    loanChequeMasterDAL objChequeMasterDAL = new loanChequeMasterDAL();
        //    objChequeMasterDAL.ChequeMasterId = ChequeMasterId;
        //    if (!objChequeMasterDAL.SelectChequeMaster())
        //    {
        //        loanAppGlobals.ShowMessage(loanMessagesDAL.SelectFail, loanMessageIcon.Error);
        //        return;
        //    }
        //    hdnChequeMasterId.Value = objChequeMasterDAL.ChequeMasterId.ToString();
        //    txtChequeName.Text = objChequeMasterDAL.ChequeName;
        //    txtChequeNo.Text = objChequeMasterDAL.ChequeNo;
        //    txtChequeDate.Text = objChequeMasterDAL.ChequeDate.ToString(loanAppGlobals.DateFormat);
        //    ddlBank.SelectedIndex = ddlBank.Items.IndexOf(ddlBank.Items.FindByValue(objChequeMasterDAL.linktoBankMasterId.ToString()));

        //    hdnCustomerMasterId.Value = objChequeMasterDAL.linktoCustomerMasterId.ToString();
        //    ddlCustomer.SelectedIndex = ddlCustomer.Items.IndexOf(ddlCustomer.Items.FindByValue(objChequeMasterDAL.linktoCustomerMasterId.ToString()));
        //    txtInstallmentAmount.Text = objChequeMasterDAL.InstallmentAmount.ToString("0.00");
        //    txtChequeAmount.Text = objChequeMasterDAL.ChequeAmount.ToString("0.00");
        //    txtGivenTo.Text = objChequeMasterDAL.GivenTo;
        //    txtNotes.Text = objChequeMasterDAL.Notes;

        //    ddlChequeStatus.SelectedIndex = ddlChequeStatus.Items.IndexOf(ddlChequeStatus.Items.FindByValue(objChequeMasterDAL.linktoChequeStatusMasterId.ToString()));


        //    hdnModelCheque.Value = "show";
        //    hdnActionCheque.Value = "edit";
        //}

        #endregion

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {

                loanUser.CheckRoleRights(loanRoleRights.AddRecord);

                foreach (ListViewItem item in lvChequeMaster.Items)
                {

                    CheckBox chkSelect = (CheckBox)item.FindControl("chkSelect");
                    if (chkSelect != null && chkSelect.Checked)
                    {
                        loanChequeMasterDAL objChequeMasterDAL = new loanChequeMasterDAL();
                        objChequeMasterDAL.ChequeMasterId = Convert.ToInt32(((HiddenField)item.FindControl("hdnChequeMasterId")).Value);
                        objChequeMasterDAL.ChequeNo = ((TextBox)item.FindControl("txtChequeNo")).Text;
                        objChequeMasterDAL.ChequeAmount = Convert.ToDecimal(((TextBox)item.FindControl("txtChequeAmount")).Text);
                        objChequeMasterDAL.ChequeName = ((Literal)item.FindControl("ltrlChequeName")).Text;
                        string chequeDate = ((Literal)item.FindControl("ltrlChequeDate")).Text;
                        objChequeMasterDAL.ChequeDate = DateTime.ParseExact(chequeDate, loanAppGlobals.DateFormat, System.Globalization.DateTimeFormatInfo.InvariantInfo);
                        objChequeMasterDAL.linktoBankMasterId = Convert.ToInt32(((HiddenField)item.FindControl("hdnBankMasterId")).Value);
                        objChequeMasterDAL.linktoCompanyMasterId = ((loanUser)Session[loanSessionsDAL.UserSession]).CompanyMasterId;
                        objChequeMasterDAL.linktoCustomerMasterId = Convert.ToInt32(((HiddenField)item.FindControl("hdnCustomerMasterId")).Value);



                        objChequeMasterDAL.GivenTo = ((Literal)item.FindControl("ltrlGivenTo")).Text;
                        objChequeMasterDAL.Notes = ((Literal)item.FindControl("ltrlNotes")).Text;


                        DropDownList ddl = (DropDownList)item.FindControl("ddlChequeStatus");
                        objChequeMasterDAL.linktoChequeStatusMasterId = Convert.ToInt32(((DropDownList)item.FindControl("ddlChequeStatus")).SelectedValue);

                        objChequeMasterDAL.UpdateDateTime = loanGlobalsDAL.GetCurrentDateTime();
                        objChequeMasterDAL.SessionId = ((loanUser)Session[loanSessionsDAL.UserSession]).SessionId;


                        loanRecordStatus rsStatus = objChequeMasterDAL.UpdateChequeMaster();
                        if (rsStatus == loanRecordStatus.Error)
                        {
                            loanAppGlobals.ShowMessage(loanMessagesDAL.UpdateFail, loanMessageIcon.Error);
                            return;
                        }
                        else if (rsStatus == loanRecordStatus.RecordAlreadyExist)
                        {
                            loanAppGlobals.ShowMessage(loanMessagesDAL.AlreadyExist, loanMessageIcon.Warning);
                            //hdnModelCheque.Value = "show";
                            return;
                        }
                        else if (rsStatus == loanRecordStatus.Success)
                        {
                            loanAppGlobals.ShowMessage(loanMessagesDAL.UpdateSuccess, loanMessageIcon.Success);
                            // hdnModelCheque.Value = "hide";

                        }
                    }
                }
                FillChequeMaster();
            }
            catch (Exception ex)
            {
                loanAppGlobals.SaveError(ex);
            }
        }
    }
}
