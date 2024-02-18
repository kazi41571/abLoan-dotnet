using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using loanLibrary;
using System.IO;

namespace abLOAN
{
    public partial class purchase : BasePage
    {
        decimal totalNetAmount = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                loanSessionsDAL.CheckSession();

                if (!Page.IsPostBack)
                {
                    //loanUser.CheckUserRights(loanUserRights.ViewPurchase);
                    loanUser.CheckRoleRights(loanRoleRights.ViewList);


                    GetItem();
                    GetCategory();

                    GetPageDefaults();

                    loanSessionsDAL.RemoveSessionAllKeyValue();
                    if (Request.QueryString.ToString().Contains("id="))
                    {
                        int id = Convert.ToInt32(Request.QueryString["id"].ToString());
                        GetPurchaseMaster(id);
                    }
                    FillPurchaseMaster();
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
                pgrPurchaseMaster.CurrentPage = 1;
                FillPurchaseMaster();
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
                //if (loanGlobalsDAL.GetCurrentDateTime().Day < 25)
                //{
                //    txtFromDate.Text = loanGlobalsDAL.ConvertDateTimeToString(loanGlobalsDAL.GetCurrentDateTime().AddMonths(-1), loanAppGlobals.DateFormat);
                //    txtFromDate.Text = "25" + txtFromDate.Text.Substring(2);
                //    txtToDate.Text = loanGlobalsDAL.ConvertDateTimeToString(loanGlobalsDAL.GetCurrentDateTime(), loanAppGlobals.DateFormat);
                //    txtToDate.Text = "24" + txtToDate.Text.Substring(2);
                //}
                //else
                //{
                //    txtFromDate.Text = loanGlobalsDAL.ConvertDateTimeToString(loanGlobalsDAL.GetCurrentDateTime(), loanAppGlobals.DateFormat);
                //    txtFromDate.Text = "25" + txtFromDate.Text.Substring(2);
                //    txtToDate.Text = loanGlobalsDAL.ConvertDateTimeToString(loanGlobalsDAL.GetCurrentDateTime().AddMonths(1), loanAppGlobals.DateFormat);
                //    txtToDate.Text = "24" + txtToDate.Text.Substring(2);
                //}

                txtFromDate.Text = loanGlobalsDAL.ConvertDateTimeToString(loanGlobalsDAL.GetCurrentDateTime(), loanAppGlobals.DateFormat);
                txtFromDate.Text = "01" + txtFromDate.Text.Substring(2);
                txtToDate.Text = loanGlobalsDAL.ConvertDateTimeToString(loanGlobalsDAL.GetCurrentDateTime(), loanAppGlobals.DateFormat);
                txtToDate.Text = DateTime.DaysInMonth(loanGlobalsDAL.GetCurrentDateTime().Year, loanGlobalsDAL.GetCurrentDateTime().Month).ToString("00") + txtToDate.Text.Substring(2);

                ddlFilterItem.SelectedIndex = 0;

                pgrPurchaseMaster.CurrentPage = 1;
                FillPurchaseMaster();
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
                loanPurchaseMasterDAL objPurchaseMasterDAL = new loanPurchaseMasterDAL();
                objPurchaseMasterDAL.linktoCompanyMasterId = ((loanUser)Session[loanSessionsDAL.UserSession]).CompanyMasterId;
                objPurchaseMasterDAL.PurchaseDate = DateTime.ParseExact(txtPurchaseDate.Text, loanAppGlobals.DateFormat, System.Globalization.DateTimeFormatInfo.InvariantInfo);
                objPurchaseMasterDAL.linktoItemMasterId = Convert.ToInt32(ddlItem.SelectedValue);
                objPurchaseMasterDAL.Quantity = Convert.ToInt32(txtQuantity.Text);
                objPurchaseMasterDAL.PurchaseRate = Convert.ToDecimal(txtPurchaseRate.Text);
                objPurchaseMasterDAL.Vat = Convert.ToDecimal(txtVat.Text);
                objPurchaseMasterDAL.NetAmount = Convert.ToDecimal(txtNetAmount.Text);
                objPurchaseMasterDAL.Notes = txtNotes.Text.Trim();

                objPurchaseMasterDAL.UpdateDateTime = loanGlobalsDAL.GetCurrentDateTime();
                objPurchaseMasterDAL.SessionId = ((loanUser)Session[loanSessionsDAL.UserSession]).SessionId;

                if (string.IsNullOrEmpty(hdnActionPurchase.Value))
                {
                    loanUser.CheckRoleRights(loanRoleRights.AddRecord);

                    loanRecordStatus rsStatus = objPurchaseMasterDAL.InsertPurchaseMaster();
                    if (rsStatus == loanRecordStatus.Error)
                    {
                        loanAppGlobals.ShowMessage(loanMessagesDAL.InsertFail, loanMessageIcon.Error);
                        return;
                    }
                    else if (rsStatus == loanRecordStatus.RecordAlreadyExist)
                    {
                        loanAppGlobals.ShowMessage(loanMessagesDAL.AlreadyExist, loanMessageIcon.Warning);
                        hdnModelPurchase.Value = "show";
                        return;
                    }
                    else if (rsStatus == loanRecordStatus.Success)
                    {
                        loanAppGlobals.ShowMessage(loanMessagesDAL.InsertSuccess, loanMessageIcon.Success);
                        if (((Button)sender).ID.Equals("btnSaveAndNew"))
                        {
                            hdnModelPurchase.Value = "clear";
                        }
                        else
                        {
                            hdnModelPurchase.Value = "hide";
                        }
                        FillPurchaseMaster();
                    }
                }
                else
                {
                    loanUser.CheckRoleRights(loanRoleRights.EditRecord);

                    objPurchaseMasterDAL.PurchaseMasterId = Convert.ToInt32(hdnPurchaseMasterId.Value);
                    loanRecordStatus rsStatus = objPurchaseMasterDAL.UpdatePurchaseMaster();
                    if (rsStatus == loanRecordStatus.Error)
                    {
                        loanAppGlobals.ShowMessage(loanMessagesDAL.UpdateFail, loanMessageIcon.Error);
                        return;
                    }
                    else if (rsStatus == loanRecordStatus.RecordAlreadyExist)
                    {
                        loanAppGlobals.ShowMessage(loanMessagesDAL.AlreadyExist, loanMessageIcon.Warning);
                        hdnModelPurchase.Value = "show";
                        return;
                    }
                    else if (rsStatus == loanRecordStatus.Success)
                    {
                        loanAppGlobals.ShowMessage(loanMessagesDAL.UpdateSuccess, loanMessageIcon.Success);
                        hdnModelPurchase.Value = "hide";
                        FillPurchaseMaster();
                    }
                }
            }
            catch (Exception ex)
            {
                loanAppGlobals.SaveError(ex);
            }
        }

        protected void btnDownload_Click(object sender, EventArgs e)
        {
            loanPurchaseMasterDAL objPurchaseMasterDAL = new loanPurchaseMasterDAL();
            if (!string.IsNullOrEmpty(txtFromDate.Text))
            {
                objPurchaseMasterDAL.FromDate = DateTime.ParseExact(txtFromDate.Text, loanAppGlobals.DateFormat, System.Globalization.DateTimeFormatInfo.InvariantInfo);
            }
            if (!string.IsNullOrEmpty(txtToDate.Text))
            {
                objPurchaseMasterDAL.ToDate = DateTime.ParseExact(txtToDate.Text, loanAppGlobals.DateFormat, System.Globalization.DateTimeFormatInfo.InvariantInfo);
            }
            if (ddlFilterItem.SelectedValue != string.Empty)
            {
                objPurchaseMasterDAL.linktoItemMasterId = Convert.ToInt32(ddlFilterItem.SelectedValue);
            }
            objPurchaseMasterDAL.linktoCompanyMasterId = ((loanUser)Session[loanSessionsDAL.UserSession]).CompanyMasterId;
            if (ddlFilterVerification.SelectedValue == "Yes")
            {
                objPurchaseMasterDAL.IsVerified = true;
            }
            else if (ddlFilterVerification.SelectedValue == "No")
            {
                objPurchaseMasterDAL.IsVerified = false;
            }

            int TotalRecords;
            List<loanPurchaseMasterDAL> lstPurchaseMaster = objPurchaseMasterDAL.SelectAllPurchaseMasterPageWise(0, int.MaxValue, out TotalRecords);

            if (lstPurchaseMaster == null)
            {
                loanAppGlobals.ShowMessage(loanMessagesDAL.SelectAllFail, loanMessageIcon.Error);
                return;
            }

            string file = System.Configuration.ConfigurationManager.AppSettings["FileSavePath"] + "Purchase.csv";

            string[] headers = { "Purchase Date", "Item", "Quantity", "Purchase Rate", "Net Amount", "Notes", "Verifier", "Modifier" };
            string[] columns = { "PurchaseDate", "Item", "Quantity", "PurchaseRate", "NetAmount", "Notes", "VerifiedBy", "ModifiedBy" };

            bool IsSuccess = loanAppGlobals.ExportCsv(lstPurchaseMaster, headers, columns, file);
            if (!IsSuccess)
            {
                loanAppGlobals.ShowMessage(loanMessagesDAL.Exception, loanMessageIcon.Error);
                return;
            }

            loanAppGlobals.SendOutFile(file, Path.GetFileName(file));
        }

        #region List Methods

        protected void lvPurchaseMaster_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            try
            {
                if (e.Item.ItemType == ListViewItemType.DataItem)
                {
                    loanPurchaseMasterDAL objPurchaseMasterDAL = (loanPurchaseMasterDAL)e.Item.DataItem;

                    Literal ltrlPurchaseDate = (Literal)e.Item.FindControl("ltrlPurchaseDate");
                    Literal ltrlItem = (Literal)e.Item.FindControl("ltrlItem");
                    Literal ltrlQuantity = (Literal)e.Item.FindControl("ltrlQuantity");
                    Literal ltrlPurchaseRate = (Literal)e.Item.FindControl("ltrlPurchaseRate");
                    Literal ltrlVat = (Literal)e.Item.FindControl("ltrlVat");
                    Literal ltrlNetAmount = (Literal)e.Item.FindControl("ltrlNetAmount");
                    Literal ltrlNotes = (Literal)e.Item.FindControl("ltrlNotes");
                    Literal ltrlModifiedBy = (Literal)e.Item.FindControl("ltrlModifiedBy");
                    ltrlModifiedBy.Text = objPurchaseMasterDAL.ModifiedBy;
                    Literal ltrlModifiedDateTime = (Literal)e.Item.FindControl("ltrlModifiedDateTime");
                    ltrlModifiedDateTime.Text = loanGlobalsDAL.ConvertDateTimeToString(objPurchaseMasterDAL.UpdateDateTime, loanAppGlobals.DateTimeFormat);

                    ltrlPurchaseDate.Text = loanGlobalsDAL.ConvertDateTimeToString(objPurchaseMasterDAL.PurchaseDate, loanAppGlobals.DateFormat);
                    ltrlItem.Text = objPurchaseMasterDAL.Item;
                    ltrlQuantity.Text = objPurchaseMasterDAL.Quantity.ToString();
                    ltrlPurchaseRate.Text = objPurchaseMasterDAL.PurchaseRate.ToString("0.00");
                    ltrlVat.Text = objPurchaseMasterDAL.Vat.ToString("0.00");
                    ltrlNetAmount.Text = objPurchaseMasterDAL.NetAmount.ToString("0.00");
                    ltrlNotes.Text = objPurchaseMasterDAL.Notes;

                    totalNetAmount += objPurchaseMasterDAL.NetAmount;

                    if (objPurchaseMasterDAL.IsVerified != null)
                    {
                        LinkButton lbtnVerify = (LinkButton)e.Item.FindControl("lbtnVerify");
                        lbtnVerify.Visible = false;
                        Literal ltrlVerifiedBy = (Literal)e.Item.FindControl("ltrlVerifiedBy");
                        ltrlVerifiedBy.Text = objPurchaseMasterDAL.VerifiedBy;
                        if (objPurchaseMasterDAL.VerifiedDateTime != null)
                        {
                            Literal ltrlVerifiedDateTime = (Literal)e.Item.FindControl("ltrlVerifiedDateTime");
                            ltrlVerifiedDateTime.Text = loanGlobalsDAL.ConvertDateTimeToString(objPurchaseMasterDAL.VerifiedDateTime.Value, loanAppGlobals.DateTimeFormat);
                        }
                    }
                    if (loanUser.GetRoleRights(loanRoleRights.Custom, "VerifyRecord") == false)
                    {
                        LinkButton lbtnVerify = (LinkButton)e.Item.FindControl("lbtnVerify");
                        lbtnVerify.Visible = false;
                    }

                }
            }
            catch (Exception ex)
            {
                loanAppGlobals.SaveError(ex);
            }
        }

        protected void pgrPurchaseMaster_ItemCommand(object sender, EventArgs e)
        {
            try
            {
                FillPurchaseMaster();
            }
            catch (Exception ex)
            {
                loanAppGlobals.SaveError(ex);
            }
        }

        protected void lvPurchaseMaster_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName.Equals("EditRecord", StringComparison.CurrentCultureIgnoreCase))
                {
                    GetPurchaseMaster(Convert.ToInt32(((ListView)sender).DataKeys[e.Item.DataItemIndex].Value));
                }
                else if (e.CommandName.Equals("VerifyRecord", StringComparison.CurrentCultureIgnoreCase))
                {
                    string pageName = Path.GetFileNameWithoutExtension(Page.AppRelativeVirtualPath) + "master";
                    VerifyRecord(pageName, Convert.ToInt32(((ListView)sender).DataKeys[e.Item.DataItemIndex].Value));
                }
                else if (e.CommandName.Equals("DeleteRecord", StringComparison.CurrentCultureIgnoreCase))
                {
                    loanUser.CheckRoleRights(loanRoleRights.DeleteRecord);

                    loanPurchaseMasterDAL objPurchaseMasterDAL = new loanPurchaseMasterDAL();
                    objPurchaseMasterDAL.PurchaseMasterId = Convert.ToInt32(((ListView)sender).DataKeys[e.Item.DataItemIndex].Value);
                    objPurchaseMasterDAL.SessionId = ((loanUser)Session[loanSessionsDAL.UserSession]).SessionId;

                    loanRecordStatus rsStatus = objPurchaseMasterDAL.DeletePurchaseMaster();
                    if (rsStatus == loanRecordStatus.Success)
                    {
                        loanAppGlobals.ShowMessage(loanMessagesDAL.DeleteSuccess, loanMessageIcon.Success);
                        FillPurchaseMaster();
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

        protected void lvPurchaseMaster_DataBound(object sender, EventArgs e)
        {
            if (((ListView)sender).Items.Count > 0)
            {
                Literal ltrlTotalNetAmount = (Literal)((ListView)sender).FindControl("ltrlTotalNetAmount");
                ltrlTotalNetAmount.Text = totalNetAmount.ToString("0.00");
            }
        }

        #endregion


        #region Private Methods
        private void GetPageDefaults()
        {
            //if (loanGlobalsDAL.GetCurrentDateTime().Day < 25)
            //{
            //    txtFromDate.Text = loanGlobalsDAL.ConvertDateTimeToString(loanGlobalsDAL.GetCurrentDateTime().AddMonths(-1), loanAppGlobals.DateFormat);
            //    txtFromDate.Text = "25" + txtFromDate.Text.Substring(2);
            //    txtToDate.Text = loanGlobalsDAL.ConvertDateTimeToString(loanGlobalsDAL.GetCurrentDateTime(), loanAppGlobals.DateFormat);
            //    txtToDate.Text = "24" + txtToDate.Text.Substring(2);
            //}
            //else
            //{
            //    txtFromDate.Text = loanGlobalsDAL.ConvertDateTimeToString(loanGlobalsDAL.GetCurrentDateTime(), loanAppGlobals.DateFormat);
            //    txtFromDate.Text = "25" + txtFromDate.Text.Substring(2);
            //    txtToDate.Text = loanGlobalsDAL.ConvertDateTimeToString(loanGlobalsDAL.GetCurrentDateTime().AddMonths(1), loanAppGlobals.DateFormat);
            //    txtToDate.Text = "24" + txtToDate.Text.Substring(2);
            //}

            txtFromDate.Text = loanGlobalsDAL.ConvertDateTimeToString(loanGlobalsDAL.GetCurrentDateTime(), loanAppGlobals.DateFormat);
            txtFromDate.Text = "01" + txtFromDate.Text.Substring(2);
            txtToDate.Text = loanGlobalsDAL.ConvertDateTimeToString(loanGlobalsDAL.GetCurrentDateTime(), loanAppGlobals.DateFormat);
            txtToDate.Text = DateTime.DaysInMonth(loanGlobalsDAL.GetCurrentDateTime().Year, loanGlobalsDAL.GetCurrentDateTime().Month).ToString("00") + txtToDate.Text.Substring(2);

            if (loanSessionsDAL.GetSessionKeyValue("CurrentPagePurchase") != null)
            {
                pgrPurchaseMaster.CurrentPage = Convert.ToInt16(loanSessionsDAL.GetSessionKeyValue("CurrentPagePurchase"));
            }
            if (loanSessionsDAL.GetSessionKeyValue("FilterPurchase") != null)
            {
                loanPurchaseMasterDAL objPurchaseMasterDAL = (loanPurchaseMasterDAL)loanSessionsDAL.GetSessionKeyValue("FilterPurchase");

                ddlFilterItem.SelectedValue = objPurchaseMasterDAL.linktoItemMasterId.ToString();
            }
        }

        private void FillPurchaseMaster()
        {
            loanPurchaseMasterDAL objPurchaseMasterDAL = new loanPurchaseMasterDAL();
            if (!string.IsNullOrEmpty(txtFromDate.Text))
            {
                objPurchaseMasterDAL.FromDate = DateTime.ParseExact(txtFromDate.Text, loanAppGlobals.DateFormat, System.Globalization.DateTimeFormatInfo.InvariantInfo);
            }
            if (!string.IsNullOrEmpty(txtToDate.Text))
            {
                objPurchaseMasterDAL.ToDate = DateTime.ParseExact(txtToDate.Text, loanAppGlobals.DateFormat, System.Globalization.DateTimeFormatInfo.InvariantInfo);
            }
            if (ddlFilterItem.SelectedValue != string.Empty)
            {
                objPurchaseMasterDAL.linktoItemMasterId = Convert.ToInt32(ddlFilterItem.SelectedValue);
            }
            objPurchaseMasterDAL.linktoCompanyMasterId = ((loanUser)Session[loanSessionsDAL.UserSession]).CompanyMasterId;
            if (ddlFilterVerification.SelectedValue == "Yes")
            {
                objPurchaseMasterDAL.IsVerified = true;
            }
            else if (ddlFilterVerification.SelectedValue == "No")
            {
                objPurchaseMasterDAL.IsVerified = false;
            }

            loanSessionsDAL.SetSessionKeyValue("FilterPurchase", objPurchaseMasterDAL);
            loanSessionsDAL.SetSessionKeyValue("CurrentPagePurchase", pgrPurchaseMaster.CurrentPage);

            int TotalRecords;
            List<loanPurchaseMasterDAL> lstPurchaseMaster = objPurchaseMasterDAL.SelectAllPurchaseMasterPageWise(pgrPurchaseMaster.StartRowIndex, pgrPurchaseMaster.PageSize, out TotalRecords);
            pgrPurchaseMaster.TotalRowCount = TotalRecords;

            if (lstPurchaseMaster == null)
            {
                loanAppGlobals.ShowMessage(loanMessagesDAL.SelectAllFail, loanMessageIcon.Error);
                return;
            }

            if (lstPurchaseMaster.Count == 0 && pgrPurchaseMaster.TotalRowCount > 0)
            {
                pgrPurchaseMaster_ItemCommand(pgrPurchaseMaster, new EventArgs());
                return;
            }

            lvPurchaseMaster.DataSource = lstPurchaseMaster;
            lvPurchaseMaster.DataBind();

            if (lstPurchaseMaster.Count > 0)
            {
                int EndiIndex = pgrPurchaseMaster.StartRowIndex + pgrPurchaseMaster.PageSize < pgrPurchaseMaster.TotalRowCount ? pgrPurchaseMaster.StartRowIndex + pgrPurchaseMaster.PageSize : pgrPurchaseMaster.TotalRowCount;
                lblRecords.Text = string.Format(Resources.Messages.ResourceManager.GetString("Records"), pgrPurchaseMaster.StartRowIndex + 1, EndiIndex, pgrPurchaseMaster.TotalRowCount);
                lblRecords.Visible = true;
            }
            else
            {
                lblRecords.Visible = false;
            }

            if (pgrPurchaseMaster.TotalRowCount <= pgrPurchaseMaster.PageSize)
            {
                pgrPurchaseMaster.Visible = false;
            }
            else
            {
                pgrPurchaseMaster.Visible = true;
            }

        }

        private void GetItem()
        {
            ddlItem.Items.Clear();
            ddlItem.Items.Add(new System.Web.UI.WebControls.ListItem(loanDropDownItem.Select, ""));

            ddlFilterItem.Items.Clear();
            ddlFilterItem.Items.Add(new System.Web.UI.WebControls.ListItem(loanDropDownItem.Select, ""));

            List<loanItemMasterDAL> lstItemMasterDAL = loanItemMasterDAL.SelectAllItemMasterItemNameBrand();
            if (lstItemMasterDAL == null)
            {
                loanAppGlobals.ShowMessage(loanMessagesDAL.SelectAllFail, loanMessageIcon.Error);
                return;
            }
            foreach (loanItemMasterDAL obj in lstItemMasterDAL)
            {
                ddlItem.Items.Add(new System.Web.UI.WebControls.ListItem(obj.ItemName, obj.ItemMasterId.ToString()));
                ddlFilterItem.Items.Add(new System.Web.UI.WebControls.ListItem(obj.ItemName, obj.ItemMasterId.ToString()));
            }
        }

        private void GetCategory()
        {
            ddlCategory.Items.Clear();
            ddlCategory.Items.Add(new System.Web.UI.WebControls.ListItem(loanDropDownItem.Select, ""));

            List<loanCategoryMasterDAL> lstCategoryMasterDAL = loanCategoryMasterDAL.SelectAllCategoryMasterCategoryName();
            if (lstCategoryMasterDAL == null)
            {
                loanAppGlobals.ShowMessage(loanMessagesDAL.SelectAllFail, loanMessageIcon.Error);
                return;
            }
            foreach (loanCategoryMasterDAL obj in lstCategoryMasterDAL)
            {
                ddlCategory.Items.Add(new System.Web.UI.WebControls.ListItem(obj.CategoryName, obj.CategoryMasterId.ToString()));
            }
        }

        private void GetPurchaseMaster(int PurchaseMasterId)
        {
            loanUser.CheckRoleRights(loanRoleRights.ViewRecord);

            loanPurchaseMasterDAL objPurchaseMasterDAL = new loanPurchaseMasterDAL();
            objPurchaseMasterDAL.PurchaseMasterId = PurchaseMasterId;
            if (!objPurchaseMasterDAL.SelectPurchaseMaster())
            {
                loanAppGlobals.ShowMessage(loanMessagesDAL.SelectFail, loanMessageIcon.Error);
                return;
            }
            hdnPurchaseMasterId.Value = objPurchaseMasterDAL.PurchaseMasterId.ToString();
            txtPurchaseDate.Text = loanGlobalsDAL.ConvertDateTimeToString(objPurchaseMasterDAL.PurchaseDate, loanAppGlobals.DateFormat);
            ddlItem.SelectedIndex = ddlItem.Items.IndexOf(ddlItem.Items.FindByValue(objPurchaseMasterDAL.linktoItemMasterId.ToString()));
            txtQuantity.Text = objPurchaseMasterDAL.Quantity.ToString();
            txtPurchaseRate.Text = objPurchaseMasterDAL.PurchaseRate.ToString("0.00");
            txtVat.Text = objPurchaseMasterDAL.Vat.ToString("0.00");
            txtNetAmount.Text = objPurchaseMasterDAL.NetAmount.ToString("0.00");
            txtNotes.Text = objPurchaseMasterDAL.Notes;

            hdnModelPurchase.Value = "show";
            hdnActionPurchase.Value = "edit";
        }

        private void VerifyRecord(string pageName, int masterId)
        {
            loanUser.CheckRoleRights(loanRoleRights.Custom, "VerifyRecord");
            loanGlobalsDAL objGlobasDAL = new loanGlobalsDAL();
            int linktoUserMasterId = ((loanUser)Session[loanSessionsDAL.UserSession]).UserMasterId;

            loanRecordStatus rsStatus = objGlobasDAL.VerifyRecord(pageName, masterId, linktoUserMasterId);
            if (rsStatus == loanRecordStatus.Error)
            {
                loanAppGlobals.ShowMessage(loanMessagesDAL.UpdateFail, loanMessageIcon.Error);
                return;
            }
            else if (rsStatus == loanRecordStatus.Success)
            {
                loanAppGlobals.ShowMessage(loanMessagesDAL.UpdateSuccess, loanMessageIcon.Success);
                FillPurchaseMaster();
            }
        }

        #endregion

        protected void ddlItem_SelectedIndexChanged(object sender, EventArgs e)
        {
            loanItemMasterDAL objItemMasterDAL = new loanItemMasterDAL();
            if (ddlFilterItem.SelectedIndex > 0)
            {
                objItemMasterDAL.ItemMasterId = Convert.ToInt32(ddlItem.SelectedValue);
                bool isSelected = objItemMasterDAL.SelectItemMaster();
                if (isSelected)
                {
                    txtPurchaseRate.Text = objItemMasterDAL.Price.ToString("0.00");
                    txtVat.Text = "0.00";// objItemMasterDAL.Vat.ToString("0.00");
                    txtNetAmount.Text = ((objItemMasterDAL.Price * Convert.ToInt32(txtQuantity.Text)) +
                        (objItemMasterDAL.Price * Convert.ToInt32(txtQuantity.Text)) * (Convert.ToDecimal(txtVat.Text) / 100)).ToString("0.00");

                }
            }
        }

        protected void ddlCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlItem.Items.Clear();
            ddlItem.Items.Add(new System.Web.UI.WebControls.ListItem(loanDropDownItem.Select, ""));
            int linktoCategoryMasterId = 0;
            if (ddlCategory.SelectedIndex > 0)
            {
                linktoCategoryMasterId = Convert.ToInt32(ddlCategory.SelectedValue);
            }
            List<loanItemMasterDAL> lstItemMasterDAL = loanItemMasterDAL.SelectAllItemMasterItemNameBrand(linktoCategoryMasterId);
            if (lstItemMasterDAL == null)
            {
                loanAppGlobals.ShowMessage(loanMessagesDAL.SelectAllFail, loanMessageIcon.Error);
                return;
            }
            foreach (loanItemMasterDAL obj in lstItemMasterDAL)
            {
                ddlItem.Items.Add(new System.Web.UI.WebControls.ListItem(obj.ItemName, obj.ItemMasterId.ToString()));
                ddlFilterItem.Items.Add(new System.Web.UI.WebControls.ListItem(obj.ItemName, obj.ItemMasterId.ToString()));
            }
        }


    }
}
