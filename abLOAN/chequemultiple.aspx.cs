using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using loanLibrary;

namespace abLOAN
{
    public partial class chequemultiple : BasePage
    {
        List<loanChequeMasterDAL> lstChequeMaster;
        public string selectedBankIds;

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
                    //GetCustomer();
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
                //ddlFilterBank.SelectedIndex = 0;
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
                    HiddenField hdnBankMasterId = (HiddenField)e.Item.FindControl("hdnBankMasterId");
                    Literal ltrlBank = (Literal)e.Item.FindControl("ltrlBank");
                    HiddenField hdnCustomerMasterId = (HiddenField)e.Item.FindControl("hdnCustomerMasterId");
                    Literal ltrlCustomer = (Literal)e.Item.FindControl("ltrlCustomer");
                    HyperLink hlnkCustomer = (HyperLink)e.Item.FindControl("hlnkCustomer");
                    Literal ltrlNoOfContracts = (Literal)e.Item.FindControl("ltrlNoOfContracts");
                    Literal ltrlTotalInstallmentAmount = (Literal)e.Item.FindControl("ltrlTotalInstallmentAmount");

                    TextBox txtChequeAmount = (TextBox)e.Item.FindControl("txtChequeAmount");
                    TextBox txtChequeNo = (TextBox)e.Item.FindControl("txtChequeNo");
                    TextBox txtChequeName = (TextBox)e.Item.FindControl("txtChequeName");
                    TextBox txtChequeDate = (TextBox)e.Item.FindControl("txtChequeDate");
                    TextBox txtGivenTo = (TextBox)e.Item.FindControl("txtGivenTo");
                    TextBox txtNotes = (TextBox)e.Item.FindControl("txtNotes");
                    TextBox txtOthers = (TextBox)e.Item.FindControl("txtOthers");

                    if (objChequeMasterDAL.ChequeMasterId > 0)
                    {
                        hdnChequeMasterId.Value = objChequeMasterDAL.ChequeMasterId.ToString();
                    }

                    hdnBankMasterId.Value = objChequeMasterDAL.linktoBankMasterId.ToString();
                    ltrlBank.Text = objChequeMasterDAL.Bank;
                    hdnCustomerMasterId.Value = objChequeMasterDAL.linktoCustomerMasterId.ToString();
                    if (objChequeMasterDAL.CustomerIsRedFlag == true)
                    {
                        ltrlCustomer.Text = "<span class='text-danger'>" + objChequeMasterDAL.Customer + "</span>";
                        hlnkCustomer.Text = "<span class='text-danger'>" + objChequeMasterDAL.Customer + "</span>";
                    }
                    else
                    {
                        ltrlCustomer.Text = objChequeMasterDAL.Customer;
                        hlnkCustomer.Text = objChequeMasterDAL.Customer;
                    }
                    if (objChequeMasterDAL.CustomerLinks != "")
                    {
                        hlnkCustomer.NavigateUrl = objChequeMasterDAL.CustomerLinks;
                        hlnkCustomer.ToolTip = objChequeMasterDAL.CustomerLinks;
                        hlnkCustomer.Visible = true;
                        ltrlCustomer.Visible = false;
                    }
                    if (objChequeMasterDAL.NoOfContracts > 0)
                    {
                        ltrlNoOfContracts.Text = objChequeMasterDAL.NoOfContracts.ToString();
                    }
                    ltrlTotalInstallmentAmount.Text = objChequeMasterDAL.TotalInstallmentAmount.ToString("#.##");

                    if (Session["hdnBankMasterId"] != null)
                    {
                        if (Session["hdnBankMasterId"].ToString() == hdnBankMasterId.Value && Session["hdnCustomerMasterId"].ToString() == hdnCustomerMasterId.Value)
                        {
                            ltrlBank.Visible = false;
                            ltrlCustomer.Visible = false;
                            ltrlNoOfContracts.Visible = false;
                            ltrlTotalInstallmentAmount.Visible = false;
                        }
                    }
                    Session["hdnBankMasterId"] = hdnBankMasterId.Value;
                    Session["hdnCustomerMasterId"] = hdnCustomerMasterId.Value;

                    DropDownList ddlChequeStatus = new DropDownList();
                    ddlChequeStatus = (DropDownList)e.Item.FindControl("ddlChequeStatus");

                    CheckBox chkSelect = new CheckBox();
                    chkSelect = (CheckBox)e.Item.FindControl("chkSelect");
                    chkSelect.Checked = lstChequeMaster[e.Item.DataItemIndex].IsSelected;
                    txtChequeAmount.Text = lstChequeMaster[e.Item.DataItemIndex].ChequeAmount.ToString("#.##");
                    txtChequeNo.Text = Convert.ToString(lstChequeMaster[e.Item.DataItemIndex].ChequeNo);
                    txtChequeName.Text = Convert.ToString(lstChequeMaster[e.Item.DataItemIndex].ChequeName);
                    if (chkSelect.Checked != false)
                    {
                        txtChequeDate.Text = loanGlobalsDAL.ConvertDateTimeToString(objChequeMasterDAL.ChequeDate, loanAppGlobals.DateFormat);
                    }
                    txtGivenTo.Text = Convert.ToString(lstChequeMaster[e.Item.DataItemIndex].GivenTo);
                    txtGivenTo.ToolTip = txtGivenTo.Text;
                    txtNotes.Text = Convert.ToString(lstChequeMaster[e.Item.DataItemIndex].Notes);
                    txtNotes.ToolTip = txtNotes.Text;
                    txtOthers.Text = Convert.ToString(lstChequeMaster[e.Item.DataItemIndex].Others);
                    txtOthers.ToolTip = txtOthers.Text;
                    ddlChequeStatus.DataSource = (List<loanChequeStatusMasterDAL>)Session["lstChequeStatusMasterDAL"];
                    ddlChequeStatus.DataTextField = "ChequeStatusName";
                    ddlChequeStatus.DataValueField = "ChequeStatusMasterId";
                    ddlChequeStatus.DataBind();

                    ddlChequeStatus.SelectedValue = lstChequeMaster[e.Item.DataItemIndex].linktoChequeStatusMasterId.ToString();


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
                if (e.CommandName.Equals("AddRecord", StringComparison.CurrentCultureIgnoreCase))
                {
                    // GetChequeMaster(Convert.ToInt32(((ListView)sender).DataKeys[e.Item.DataItemIndex].Value));
                    loanUser.CheckRoleRights(loanRoleRights.AddRecord);


                    AddRow(sender, e);
                }
                //else if (e.CommandName.Equals("DeleteRecord", StringComparison.CurrentCultureIgnoreCase))
                //{
                //    loanChequeMasterDAL objChequeMasterDAL = new loanChequeMasterDAL();
                //    objChequeMasterDAL.ChequeMasterId = Convert.ToInt32(((ListView)sender).DataKeys[e.Item.DataItemIndex].Value);
                //    loanRecordStatus rsStatus = objChequeMasterDAL.DeleteChequeMaster();
                //    if (rsStatus == loanRecordStatus.Success)
                //    {
                //        loanAppGlobals.ShowMessage(loanMessagesDAL.DeleteSuccess, loanMessageIcon.Success);
                //        FillChequeMaster();
                //    }
                //    else
                //    {
                //        loanAppGlobals.ShowMessage(loanMessagesDAL.DeleteFail, loanMessageIcon.Error);
                //    }
                //}
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
                //ddlFilterBank.SelectedValue = objChequeMasterDAL.linktoBankMasterId.ToString();
                txtFilterCustomer.Text = objChequeMasterDAL.Customer;
                ddlFilterChequeStatus.SelectedValue = objChequeMasterDAL.linktoChequeStatusMasterId.ToString();
            }
        }

        private void FillChequeMaster()
        {


            loanUser.CheckRoleRights(loanRoleRights.ViewRecord);
            loanChequeMasterDAL objChequeMasterDAL = new loanChequeMasterDAL();

            if (txtFilterChequeNo.Text != string.Empty)
            {
                objChequeMasterDAL.ChequeNo = txtFilterChequeNo.Text.Trim();
            }
            //if (ddlFilterBank.SelectedValue != string.Empty)
            //{
            //    objChequeMasterDAL.linktoBankMasterId = Convert.ToInt32(ddlFilterBank.SelectedValue);
            //}
            selectedBankIds = "";
            foreach (ListItem item in chkFilterBank.Items)
            {
                if (item.Selected)
                {
                    selectedBankIds += item.Value;
                    selectedBankIds += ",";
                }
            }
            if (selectedBankIds.Length > 0)
            {
                selectedBankIds = selectedBankIds.Substring(0, selectedBankIds.Length - 1);
                if (chkFilterBank.SelectedValue != string.Empty)
                {
                    objChequeMasterDAL.BankIds = selectedBankIds;
                }
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
            lstChequeMaster = objChequeMasterDAL.SelectAllChequeMasterCustomerContractPageWise(pgrChequeMaster.StartRowIndex, 100, out TotalRecords);
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
            Session["lstChequeMaster"] = lstChequeMaster;

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
            Session["hdnBankMasterId"] = null;
            Session["hdnCustomerMasterId"] = null;
        }

        private void GetBank()
        {

            //ddlFilterBank.Items.Clear();
            //ddlFilterBank.Items.Add(new System.Web.UI.WebControls.ListItem(loanDropDownItem.Select, ""));

            //List<loanBankMasterDAL> lstBankMasterDAL = loanBankMasterDAL.SelectAllBankMasterBankName();
            //if (lstBankMasterDAL == null)
            //{
            //    loanAppGlobals.ShowMessage(loanMessagesDAL.SelectAllFail, loanMessageIcon.Error);
            //    return;
            //}
            //foreach (loanBankMasterDAL obj in lstBankMasterDAL)
            //{
            //    ddlFilterBank.Items.Add(new System.Web.UI.WebControls.ListItem(obj.BankName, obj.BankMasterId.ToString()));

            //}

            chkFilterBank.Items.Clear();
            List<loanBankMasterDAL> lstBankMasterDAL = loanBankMasterDAL.SelectAllBankMasterBankName();
            if (lstBankMasterDAL == null)
            {
                loanAppGlobals.ShowMessage(loanMessagesDAL.SelectAllFail, loanMessageIcon.Error);
                return;
            }
            foreach (loanBankMasterDAL obj in lstBankMasterDAL)
            {
                chkFilterBank.Items.Add(new System.Web.UI.WebControls.ListItem(obj.BankName, obj.BankMasterId.ToString()));

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
            // ddlChequeStatus.Items.Add(new System.Web.UI.WebControls.ListItem(loanDropDownItem.Select, ""));

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

        private void AddRow(object sender, ListViewCommandEventArgs e)
        {
            lstChequeMaster = (List<loanChequeMasterDAL>)Session["lstChequeMaster"];

            int index;
            DropDownList ddl;
            //save list in session

            foreach (ListViewItem item in lvChequeMaster.Items)
            {
                CheckBox chkSelect = (CheckBox)item.FindControl("chkSelect");
                if (chkSelect != null && chkSelect.Checked)
                {
                    index = Convert.ToInt32(e.Item.DataItemIndex);
                    if (((TextBox)item.FindControl("txtChequeAmount")).Text != "")
                    {
                        lstChequeMaster[index].ChequeAmount = Convert.ToDecimal(((TextBox)item.FindControl("txtChequeAmount")).Text);
                    }
                    if (((TextBox)item.FindControl("txtChequeNo")).Text != "")
                    {
                        lstChequeMaster[index].ChequeNo = ((TextBox)item.FindControl("txtChequeNo")).Text;
                    }
                    if (((TextBox)item.FindControl("txtChequeName")).Text != "")
                    {
                        lstChequeMaster[index].ChequeName = ((TextBox)item.FindControl("txtChequeName")).Text;
                    }
                    if (((TextBox)item.FindControl("txtChequeDate")).Text != "")
                    {
                        string chequeDate = ((TextBox)item.FindControl("txtChequeDate")).Text;
                        lstChequeMaster[index].ChequeDate = DateTime.ParseExact(chequeDate, loanAppGlobals.DateFormat, System.Globalization.DateTimeFormatInfo.InvariantInfo);
                    }

                    lstChequeMaster[index].linktoBankMasterId = Convert.ToInt32(((HiddenField)item.FindControl("hdnBankMasterId")).Value);
                    lstChequeMaster[index].linktoCompanyMasterId = ((loanUser)Session[loanSessionsDAL.UserSession]).CompanyMasterId;
                    lstChequeMaster[index].linktoCustomerMasterId = Convert.ToInt32(((HiddenField)item.FindControl("hdnCustomerMasterId")).Value);
                    if (((TextBox)item.FindControl("txtGivenTo")).Text != "")
                    {
                        lstChequeMaster[index].GivenTo = ((TextBox)item.FindControl("txtGivenTo")).Text;
                    }
                    if (((TextBox)item.FindControl("txtNotes")).Text != "")
                    {
                        lstChequeMaster[index].Notes = ((TextBox)item.FindControl("txtNotes")).Text;
                    }
                    if (((TextBox)item.FindControl("txtOthers")).Text != "")
                    {
                        lstChequeMaster[index].Others = ((TextBox)item.FindControl("txtOthers")).Text;
                    }
                    ddl = (DropDownList)item.FindControl("ddlChequeStatus");
                    lstChequeMaster[index].linktoChequeStatusMasterId = Convert.ToInt32(((DropDownList)item.FindControl("ddlChequeStatus")).SelectedValue);
                    lstChequeMaster[index].IsSelected = Convert.ToBoolean(((CheckBox)item.FindControl("chkSelect")).Checked);

                    lstChequeMaster[index].UpdateDateTime = loanGlobalsDAL.GetCurrentDateTime();
                    lstChequeMaster[index].SessionId = ((loanUser)Session[loanSessionsDAL.UserSession]).SessionId;

                }
            }
            Session["lstChequeMaster"] = lstChequeMaster;
            //
            //save current row in object
            loanChequeMasterDAL objChequeMasterDAL = new loanChequeMasterDAL();


            objChequeMasterDAL.linktoBankMasterId = Convert.ToInt32(((HiddenField)e.Item.FindControl("hdnBankMasterId")).Value);
            objChequeMasterDAL.linktoCompanyMasterId = ((loanUser)Session[loanSessionsDAL.UserSession]).CompanyMasterId;
            objChequeMasterDAL.linktoCustomerMasterId = Convert.ToInt32(((HiddenField)e.Item.FindControl("hdnCustomerMasterId")).Value);
            ddl = (DropDownList)e.Item.FindControl("ddlChequeStatus");
            objChequeMasterDAL.linktoChequeStatusMasterId = Convert.ToInt32(((DropDownList)e.Item.FindControl("ddlChequeStatus")).SelectedValue);

            objChequeMasterDAL.UpdateDateTime = loanGlobalsDAL.GetCurrentDateTime();
            objChequeMasterDAL.SessionId = ((loanUser)Session[loanSessionsDAL.UserSession]).SessionId;

            //add new row
            index = Convert.ToInt32(e.Item.DataItemIndex);
            lstChequeMaster = (List<loanChequeMasterDAL>)Session["lstChequeMaster"];
            lstChequeMaster.Insert(index + 1, objChequeMasterDAL);

            lvChequeMaster.DataSource = lstChequeMaster;
            lvChequeMaster.DataBind();

            Session["lstChequeMaster"] = lstChequeMaster;

        }

        protected void btnInsert_Click(object sender, EventArgs e)
        {
            try
            {
                foreach (ListViewItem item in lvChequeMaster.Items)
                {
                    CheckBox chkSelect = (CheckBox)item.FindControl("chkSelect");
                    if (chkSelect != null && chkSelect.Checked)
                    {
                        loanChequeMasterDAL objChequeMasterDAL = new loanChequeMasterDAL();



                        string ChequeNo = ((TextBox)item.FindControl("txtChequeNo")).Text;
                        if (ChequeNo != "")
                        {
                            objChequeMasterDAL.ChequeNo = ChequeNo;
                        }




                        string ChequeAmount = ((TextBox)item.FindControl("txtChequeAmount")).Text;
                        if (ChequeAmount != "")
                        {
                            objChequeMasterDAL.ChequeAmount = Convert.ToDecimal(ChequeAmount);
                        }
                        string ChequeName = ((TextBox)item.FindControl("txtChequeName")).Text;
                        if (ChequeName != "")
                        {
                            objChequeMasterDAL.ChequeName = ((TextBox)item.FindControl("txtChequeName")).Text;
                        }
                        string ChequeDate = ((TextBox)item.FindControl("txtChequeDate")).Text;
                        if (ChequeDate != "")
                        {
                            objChequeMasterDAL.ChequeDate = DateTime.ParseExact(ChequeDate, loanAppGlobals.DateFormat, System.Globalization.DateTimeFormatInfo.InvariantInfo);
                        }
                        objChequeMasterDAL.linktoBankMasterId = Convert.ToInt32(((HiddenField)item.FindControl("hdnBankMasterId")).Value);
                        objChequeMasterDAL.linktoCompanyMasterId = ((loanUser)Session[loanSessionsDAL.UserSession]).CompanyMasterId;
                        objChequeMasterDAL.linktoCustomerMasterId = Convert.ToInt32(((HiddenField)item.FindControl("hdnCustomerMasterId")).Value);

                        string GivenTo = ((TextBox)item.FindControl("txtGivenTo")).Text;
                        if (GivenTo != "")
                        {
                            objChequeMasterDAL.GivenTo = GivenTo;
                        }
                        string Notes = ((TextBox)item.FindControl("txtNotes")).Text;
                        if (Notes != "")
                        {
                            objChequeMasterDAL.Notes = ((TextBox)item.FindControl("txtNotes")).Text;
                        }
                        string Others = ((TextBox)item.FindControl("txtOthers")).Text;
                        if (Others != "")
                        {
                            objChequeMasterDAL.Others = ((TextBox)item.FindControl("txtOthers")).Text;
                        }



                        DropDownList ddl = (DropDownList)item.FindControl("ddlChequeStatus");
                        objChequeMasterDAL.linktoChequeStatusMasterId = Convert.ToInt32(((DropDownList)item.FindControl("ddlChequeStatus")).SelectedValue);

                        objChequeMasterDAL.UpdateDateTime = loanGlobalsDAL.GetCurrentDateTime();
                        objChequeMasterDAL.SessionId = ((loanUser)Session[loanSessionsDAL.UserSession]).SessionId;


                        HiddenField hdnChequeMasterId = (HiddenField)item.FindControl("hdnChequeMasterId");
                        loanRecordStatus rsStatus;
                        if (hdnChequeMasterId.Value != "" && hdnChequeMasterId.Value != null)
                        {
                            objChequeMasterDAL.ChequeMasterId = Convert.ToInt32(hdnChequeMasterId.Value);
                            rsStatus = objChequeMasterDAL.UpdateChequeMaster();
                        }
                        else
                        {
                            rsStatus = objChequeMasterDAL.InsertChequeMaster();
                        }

                        if (rsStatus == loanRecordStatus.Error)
                        {
                            loanAppGlobals.ShowMessage(loanMessagesDAL.InsertFail, loanMessageIcon.Error);
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
                            loanAppGlobals.ShowMessage(loanMessagesDAL.InsertSuccess, loanMessageIcon.Success);
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


        #endregion


    }
}
