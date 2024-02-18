using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using loanLibrary;

namespace abLOAN
{
    public partial class item : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                loanSessionsDAL.CheckSession();

                if (!Page.IsPostBack)
                {
                    //loanUser.CheckUserRights(loanUserRights.ViewItem);
                    loanUser.CheckRoleRights(loanRoleRights.ViewList);

                    GetCategory();
                    GetBrand();
                    GetColor();
                    GetPageDefaults();

                    loanSessionsDAL.RemoveSessionAllKeyValue();

                    FillItemMaster();
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
                pgrItemMaster.CurrentPage = 1;
                FillItemMaster();
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
                txtFilterItemName.Text = string.Empty;
                txtFilterItemCode.Text = string.Empty;
                ddlFilterIsEnabled.SelectedIndex = 0;

                pgrItemMaster.CurrentPage = 1;
                FillItemMaster();
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
                loanItemMasterDAL objItemMasterDAL = new loanItemMasterDAL();
                objItemMasterDAL.linktoCompanyMasterId = ((loanUser)Session[loanSessionsDAL.UserSession]).CompanyMasterId;
                objItemMasterDAL.ItemName = txtItemName.Text.Trim();
                objItemMasterDAL.ItemCode = txtItemCode.Text.Trim();
                objItemMasterDAL.ItemDescription = txtItemDescription.Text.Trim();
                if (!string.IsNullOrEmpty(ddlCategory.Text))
                {
                    objItemMasterDAL.linktoCategoryMasterId = Convert.ToInt32(ddlCategory.SelectedValue);
                }
                if (!string.IsNullOrEmpty(ddlBrand.Text))
                {
                    objItemMasterDAL.linktoBrandMasterId = Convert.ToInt32(ddlBrand.SelectedValue);
                }
                if (!string.IsNullOrEmpty(ddlColor.Text))
                {
                    objItemMasterDAL.linktoColorMasterId = Convert.ToInt32(ddlColor.SelectedValue);
                }
                objItemMasterDAL.Price = Convert.ToDecimal(txtPrice.Text.Trim());
                objItemMasterDAL.SalesPrice = Convert.ToDecimal(txtSalesPrice.Text.Trim());
                objItemMasterDAL.Vat = Convert.ToDecimal(txtVat.Text.Trim());
                objItemMasterDAL.IsEnabled = chkIsEnabled.Checked;

                objItemMasterDAL.UpdateDateTime = loanGlobalsDAL.GetCurrentDateTime();
                objItemMasterDAL.SessionId = ((loanUser)Session[loanSessionsDAL.UserSession]).SessionId;

                if (string.IsNullOrEmpty(hdnActionItem.Value))
                {
                    loanUser.CheckRoleRights(loanRoleRights.AddRecord);

                    loanRecordStatus rsStatus = objItemMasterDAL.InsertItemMaster();
                    if (rsStatus == loanRecordStatus.Error)
                    {
                        loanAppGlobals.ShowMessage(loanMessagesDAL.InsertFail, loanMessageIcon.Error);
                        return;
                    }
                    else if (rsStatus == loanRecordStatus.RecordAlreadyExist)
                    {
                        loanAppGlobals.ShowMessage(loanMessagesDAL.AlreadyExist, loanMessageIcon.Warning);
                        hdnModelItem.Value = "show";
                        return;
                    }
                    else if (rsStatus == loanRecordStatus.Success)
                    {
                        loanAppGlobals.ShowMessage(loanMessagesDAL.InsertSuccess, loanMessageIcon.Success);
                        if (((Button)sender).ID.Equals("btnSaveAndNew"))
                        {
                            hdnModelItem.Value = "clear";
                        }
                        else
                        {
                            hdnModelItem.Value = "hide";
                        }
                        FillItemMaster();
                    }
                }
                else
                {
                    loanUser.CheckRoleRights(loanRoleRights.EditRecord);

                    objItemMasterDAL.ItemMasterId = Convert.ToInt32(hdnItemMasterId.Value);
                    loanRecordStatus rsStatus = objItemMasterDAL.UpdateItemMaster();
                    if (rsStatus == loanRecordStatus.Error)
                    {
                        loanAppGlobals.ShowMessage(loanMessagesDAL.UpdateFail, loanMessageIcon.Error);
                        return;
                    }
                    else if (rsStatus == loanRecordStatus.RecordAlreadyExist)
                    {
                        loanAppGlobals.ShowMessage(loanMessagesDAL.AlreadyExist, loanMessageIcon.Warning);
                        hdnModelItem.Value = "show";
                        return;
                    }
                    else if (rsStatus == loanRecordStatus.Success)
                    {
                        loanAppGlobals.ShowMessage(loanMessagesDAL.UpdateSuccess, loanMessageIcon.Success);
                        hdnModelItem.Value = "hide";
                        FillItemMaster();
                    }
                }
            }
            catch (Exception ex)
            {
                loanAppGlobals.SaveError(ex);
            }
        }

        #region List Methods

        protected void lvItemMaster_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            try
            {
                if (e.Item.ItemType == ListViewItemType.DataItem)
                {
                    loanItemMasterDAL objItemMasterDAL = (loanItemMasterDAL)e.Item.DataItem;

                    Literal ltrlItemName = (Literal)e.Item.FindControl("ltrlItemName");
                    Literal ltrlItemCode = (Literal)e.Item.FindControl("ltrlItemCode");
                    Literal ltrlCategory = (Literal)e.Item.FindControl("ltrlCategory");
                    Literal ltrlBrand = (Literal)e.Item.FindControl("ltrlBrand");
                    Literal ltrlColor = (Literal)e.Item.FindControl("ltrlColor");
                    Literal ltrlPrice = (Literal)e.Item.FindControl("ltrlPrice");
                    Literal ltrlSalesPrice = (Literal)e.Item.FindControl("ltrlSalesPrice");
                    Literal ltrlVat = (Literal)e.Item.FindControl("ltrlVat");
                    Literal ltrlCurrentQuantity = (Literal)e.Item.FindControl("ltrlCurrentQuantity");

                    ltrlItemName.Text = objItemMasterDAL.ItemName;
                    ltrlItemCode.Text = objItemMasterDAL.ItemCode;
                    ltrlCategory.Text = objItemMasterDAL.Category;
                    if (objItemMasterDAL.linktoBrandMasterId != null)
                    {
                        ltrlBrand.Text = objItemMasterDAL.Brand;
                    }
                    if (objItemMasterDAL.linktoColorMasterId != null)
                    {
                        ltrlColor.Text = objItemMasterDAL.Color;
                    }
                    ltrlPrice.Text = objItemMasterDAL.Price.ToString("0.00");
                    ltrlSalesPrice.Text = objItemMasterDAL.SalesPrice.ToString("0.00");
                    ltrlVat.Text = objItemMasterDAL.Vat.ToString("0.00");
                    ltrlCurrentQuantity.Text = objItemMasterDAL.CurrentQuantity.ToString();
                }
            }
            catch (Exception ex)
            {
                loanAppGlobals.SaveError(ex);
            }
        }

        protected void pgrItemMaster_ItemCommand(object sender, EventArgs e)
        {
            try
            {
                FillItemMaster();
            }
            catch (Exception ex)
            {
                loanAppGlobals.SaveError(ex);
            }
        }

        protected void lvItemMaster_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName.Equals("EditRecord", StringComparison.CurrentCultureIgnoreCase))
                {
                    GetItemMaster(Convert.ToInt32(((ListView)sender).DataKeys[e.Item.DataItemIndex].Value));
                }
                else if (e.CommandName.Equals("DeleteRecord", StringComparison.CurrentCultureIgnoreCase))
                {
                    loanUser.CheckRoleRights(loanRoleRights.DeleteRecord);

                    loanItemMasterDAL objItemMasterDAL = new loanItemMasterDAL();
                    objItemMasterDAL.ItemMasterId = Convert.ToInt32(((ListView)sender).DataKeys[e.Item.DataItemIndex].Value);
                    loanRecordStatus rsStatus = objItemMasterDAL.DeleteItemMaster();
                    if (rsStatus == loanRecordStatus.Success)
                    {
                        loanAppGlobals.ShowMessage(loanMessagesDAL.DeleteSuccess, loanMessageIcon.Success);
                        FillItemMaster();
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
            if (loanSessionsDAL.GetSessionKeyValue("CurrentPageItem") != null)
            {
                pgrItemMaster.CurrentPage = Convert.ToInt16(loanSessionsDAL.GetSessionKeyValue("CurrentPageItem"));
            }
            if (loanSessionsDAL.GetSessionKeyValue("FilterItem") != null)
            {
                loanItemMasterDAL objItemMasterDAL = (loanItemMasterDAL)loanSessionsDAL.GetSessionKeyValue("FilterItem");
                txtFilterItemName.Text = objItemMasterDAL.ItemName;
                if (objItemMasterDAL.IsEnabled)
                {
                    ddlFilterIsEnabled.SelectedValue = "Yes";
                }
                else
                {
                    ddlFilterIsEnabled.SelectedValue = "No";
                }
            }
        }

        private void FillItemMaster()
        {

            loanItemMasterDAL objItemMasterDAL = new loanItemMasterDAL();
            objItemMasterDAL.ItemName = txtFilterItemName.Text.Trim();
            objItemMasterDAL.ItemCode = txtFilterItemCode.Text.Trim();
            if (ddlFilterIsEnabled.SelectedValue == "Yes")
            {
                objItemMasterDAL.IsEnabled = true;
            }
            else if (ddlFilterIsEnabled.SelectedValue == "No")
            {
                objItemMasterDAL.IsEnabled = false;
            }
            objItemMasterDAL.linktoCompanyMasterId = ((loanUser)Session[loanSessionsDAL.UserSession]).CompanyMasterId;

            loanSessionsDAL.SetSessionKeyValue("FilterItem", objItemMasterDAL);
            loanSessionsDAL.SetSessionKeyValue("CurrentPageItem", pgrItemMaster.CurrentPage);

            int TotalRecords;
            List<loanItemMasterDAL> lstItemMaster = objItemMasterDAL.SelectAllItemMasterPageWise(pgrItemMaster.StartRowIndex, pgrItemMaster.PageSize, out TotalRecords);
            pgrItemMaster.TotalRowCount = TotalRecords;

            if (lstItemMaster == null)
            {
                loanAppGlobals.ShowMessage(loanMessagesDAL.SelectAllFail, loanMessageIcon.Error);
                return;
            }

            if (lstItemMaster.Count == 0 && pgrItemMaster.TotalRowCount > 0)
            {
                pgrItemMaster_ItemCommand(pgrItemMaster, new EventArgs());
                return;
            }

            lvItemMaster.DataSource = lstItemMaster;
            lvItemMaster.DataBind();

            if (lstItemMaster.Count > 0)
            {
                int EndiIndex = pgrItemMaster.StartRowIndex + pgrItemMaster.PageSize < pgrItemMaster.TotalRowCount ? pgrItemMaster.StartRowIndex + pgrItemMaster.PageSize : pgrItemMaster.TotalRowCount;
                lblRecords.Text = string.Format(Resources.Messages.ResourceManager.GetString("Records"), pgrItemMaster.StartRowIndex + 1, EndiIndex, pgrItemMaster.TotalRowCount);
                lblRecords.Visible = true;
            }
            else
            {
                lblRecords.Visible = false;
            }

            if (pgrItemMaster.TotalRowCount <= pgrItemMaster.PageSize)
            {
                pgrItemMaster.Visible = false;
            }
            else
            {
                pgrItemMaster.Visible = true;
            }

        }

        private void GetCategory()
        {
            ddlCategory.Items.Clear();
            ddlCategory.Items.Add(new System.Web.UI.WebControls.ListItem(loanDropDownItem.Select, ""));

            //ddlFilterCategory.Items.Clear();
            //ddlFilterCategory.Items.Add(new System.Web.UI.WebControls.ListItem(loanDropDownItem.Select, ""));

            List<loanCategoryMasterDAL> lstCategoryMasterDAL = loanCategoryMasterDAL.SelectAllCategoryMasterCategoryName();
            if (lstCategoryMasterDAL == null)
            {
                loanAppGlobals.ShowMessage(loanMessagesDAL.SelectAllFail, loanMessageIcon.Error);
                return;
            }
            foreach (loanCategoryMasterDAL obj in lstCategoryMasterDAL)
            {
                ddlCategory.Items.Add(new System.Web.UI.WebControls.ListItem(obj.CategoryName, obj.CategoryMasterId.ToString()));
                //ddlFilterCategory.Items.Add(new System.Web.UI.WebControls.ListItem(obj.CategoryName, obj.CategoryMasterId.ToString()));
            }
        }

        private void GetBrand()
        {
            ddlBrand.Items.Clear();
            ddlBrand.Items.Add(new System.Web.UI.WebControls.ListItem(loanDropDownItem.Select, ""));

            List<loanBrandMasterDAL> lstBrandMasterDAL = loanBrandMasterDAL.SelectAllBrandMasterBrandName();
            if (lstBrandMasterDAL == null)
            {
                loanAppGlobals.ShowMessage(loanMessagesDAL.SelectAllFail, loanMessageIcon.Error);
                return;
            }
            foreach (loanBrandMasterDAL obj in lstBrandMasterDAL)
            {
                ddlBrand.Items.Add(new System.Web.UI.WebControls.ListItem(obj.BrandName, obj.BrandMasterId.ToString()));
            }
        }

        private void GetColor()
        {
            ddlColor.Items.Clear();
            ddlColor.Items.Add(new System.Web.UI.WebControls.ListItem(loanDropDownItem.Select, ""));

            List<loanColorMasterDAL> lstColorMasterDAL = loanColorMasterDAL.SelectAllColorMasterColorName();
            if (lstColorMasterDAL == null)
            {
                loanAppGlobals.ShowMessage(loanMessagesDAL.SelectAllFail, loanMessageIcon.Error);
                return;
            }
            foreach (loanColorMasterDAL obj in lstColorMasterDAL)
            {
                ddlColor.Items.Add(new System.Web.UI.WebControls.ListItem(obj.ColorName, obj.ColorMasterId.ToString()));
            }
        }

        private void GetItemMaster(int ItemMasterId)
        {
            loanUser.CheckRoleRights(loanRoleRights.ViewRecord);

            loanItemMasterDAL objItemMasterDAL = new loanItemMasterDAL();
            objItemMasterDAL.ItemMasterId = ItemMasterId;
            if (!objItemMasterDAL.SelectItemMaster())
            {
                loanAppGlobals.ShowMessage(loanMessagesDAL.SelectFail, loanMessageIcon.Error);
                return;
            }
            hdnItemMasterId.Value = objItemMasterDAL.ItemMasterId.ToString();
            txtItemName.Text = objItemMasterDAL.ItemName;
            txtItemCode.Text = objItemMasterDAL.ItemCode;
            txtItemDescription.Text = objItemMasterDAL.ItemDescription;
            if (objItemMasterDAL.linktoBrandMasterId != null)
            {
                ddlBrand.Text = objItemMasterDAL.linktoBrandMasterId.Value.ToString();
            }
            if (objItemMasterDAL.linktoColorMasterId != null)
            {
                ddlColor.Text = objItemMasterDAL.linktoColorMasterId.Value.ToString();
            }
            txtPrice.Text = objItemMasterDAL.Price.ToString("0.00");
            txtSalesPrice.Text = objItemMasterDAL.SalesPrice.ToString("0.00");
            txtVat.Text = objItemMasterDAL.Vat.ToString("0.00");
            chkIsEnabled.Checked = objItemMasterDAL.IsEnabled;

            hdnModelItem.Value = "show";
            hdnActionItem.Value = "edit";
        }
        #endregion


    }
}
