using System;
using System.Collections.Generic;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;
using loanLibrary;

namespace abLOAN
{
    public partial class witness : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                loanSessionsDAL.CheckSession();

                if (!Page.IsPostBack)
                {
                    //loanUser.CheckUserRights(loanUserRights.ViewWitness);
                    loanUser.CheckRoleRights(loanRoleRights.ViewList);


                    GetPageDefaults();

                    loanSessionsDAL.RemoveSessionAllKeyValue();

                    FillWitnessMaster();
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
                pgrWitnessMaster.CurrentPage = 1;
                FillWitnessMaster();
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
                txtFilterWitness.Text = string.Empty;
                txtFilterPhoneMobile.Text = string.Empty;

                pgrWitnessMaster.CurrentPage = 1;
                FillWitnessMaster();
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
                loanWitnessMasterDAL objWitnessMasterDAL = new loanWitnessMasterDAL();
                objWitnessMasterDAL.linktoCompanyMasterId = ((loanUser)Session[loanSessionsDAL.UserSession]).CompanyMasterId;
                objWitnessMasterDAL.WitnessName = txtWitnessName.Text.Trim();
                objWitnessMasterDAL.IdNo = txtIdNo.Text.Trim();
                if (fuPhotoIdImageName.HasFile)
                {
                    objWitnessMasterDAL.PhotoIdImageName = objWitnessMasterDAL.WitnessMasterId + System.IO.Path.GetExtension(fuPhotoIdImageName.FileName);
                    string ImageSavePath = System.Configuration.ConfigurationManager.AppSettings["ImageSavePath"] + "Witness\\";
                    if (!Directory.Exists(ImageSavePath))
                    {
                        Directory.CreateDirectory(ImageSavePath);
                    }
                    fuPhotoIdImageName.SaveAs(ImageSavePath + objWitnessMasterDAL.PhotoIdImageName);

                    loanGlobalsDAL.CreateThumbImages(objWitnessMasterDAL.PhotoIdImageName, ImageSavePath);

                }
                else if (hdnPhotoIdImageName.Value != string.Empty)
                {
                    objWitnessMasterDAL.PhotoIdImageName = hdnPhotoIdImageName.Value;
                }
                objWitnessMasterDAL.Address1 = txtAddress1.Text.Trim();
                objWitnessMasterDAL.Phone1 = txtPhone1.Text.Trim();
                objWitnessMasterDAL.Mobile1 = txtMobile1.Text.Trim();
                objWitnessMasterDAL.Fax1 = txtFax1.Text.Trim();
                objWitnessMasterDAL.Address2 = txtAddress2.Text.Trim();
                objWitnessMasterDAL.Phone2 = txtPhone2.Text.Trim();
                objWitnessMasterDAL.Mobile2 = txtMobile2.Text.Trim();
                objWitnessMasterDAL.Fax2 = txtFax2.Text.Trim();


                objWitnessMasterDAL.UpdateDateTime = loanGlobalsDAL.GetCurrentDateTime();
                objWitnessMasterDAL.SessionId = ((loanUser)Session[loanSessionsDAL.UserSession]).SessionId;

                if (string.IsNullOrEmpty(hdnActionWitness.Value))
                {
                    loanUser.CheckRoleRights(loanRoleRights.AddRecord);

                    loanRecordStatus rsStatus = objWitnessMasterDAL.InsertWitnessMaster();
                    if (rsStatus == loanRecordStatus.Error)
                    {
                        loanAppGlobals.ShowMessage(loanMessagesDAL.InsertFail, loanMessageIcon.Error);
                        return;
                    }
                    else if (rsStatus == loanRecordStatus.RecordAlreadyExist)
                    {
                        loanAppGlobals.ShowMessage(loanMessagesDAL.AlreadyExist, loanMessageIcon.Warning);
                        hdnModelWitness.Value = "show";
                        return;
                    }
                    else if (rsStatus == loanRecordStatus.Success)
                    {
                        loanAppGlobals.ShowMessage(loanMessagesDAL.InsertSuccess, loanMessageIcon.Success);
                        if (((Button)sender).ID.Equals("btnSaveAndNew"))
                        {
                            hdnModelWitness.Value = "clear";
                        }
                        else
                        {
                            hdnModelWitness.Value = "hide";
                        }
                        FillWitnessMaster();
                    }
                }
                else
                {
                    loanUser.CheckRoleRights(loanRoleRights.EditRecord);

                    objWitnessMasterDAL.WitnessMasterId = Convert.ToInt32(hdnWitnessMasterId.Value);
                    loanRecordStatus rsStatus = objWitnessMasterDAL.UpdateWitnessMaster();
                    if (rsStatus == loanRecordStatus.Error)
                    {
                        loanAppGlobals.ShowMessage(loanMessagesDAL.UpdateFail, loanMessageIcon.Error);
                        return;
                    }
                    else if (rsStatus == loanRecordStatus.RecordAlreadyExist)
                    {
                        loanAppGlobals.ShowMessage(loanMessagesDAL.AlreadyExist, loanMessageIcon.Warning);
                        hdnModelWitness.Value = "show";
                        return;
                    }
                    else if (rsStatus == loanRecordStatus.Success)
                    {
                        loanAppGlobals.ShowMessage(loanMessagesDAL.UpdateSuccess, loanMessageIcon.Success);
                        hdnModelWitness.Value = "hide";
                        FillWitnessMaster();
                    }
                }
            }
            catch (Exception ex)
            {
                loanAppGlobals.SaveError(ex);
            }
        }

        #region List Methods

        protected void lvWitnessMaster_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            try
            {
                if (e.Item.ItemType == ListViewItemType.DataItem)
                {
                    loanWitnessMasterDAL objWitnessMasterDAL = (loanWitnessMasterDAL)e.Item.DataItem;

                    Image imgPhotoIdImageName = (Image)e.Item.FindControl("imgPhotoIdImageName");
                    Literal ltrlWitnessName = (Literal)e.Item.FindControl("ltrlWitnessName");
                    Literal ltrlIdNo = (Literal)e.Item.FindControl("ltrlIdNo");
                    Literal ltrlAddress1 = (Literal)e.Item.FindControl("ltrlAddress1");
                    Literal ltrlPhone1 = (Literal)e.Item.FindControl("ltrlPhone1");
                    Literal ltrlMobile1 = (Literal)e.Item.FindControl("ltrlMobile1");
                    Literal ltrlFax1 = (Literal)e.Item.FindControl("ltrlFax1");
                    Literal ltrlAddress2 = (Literal)e.Item.FindControl("ltrlAddress2");
                    Literal ltrlPhone2 = (Literal)e.Item.FindControl("ltrlPhone2");
                    Literal ltrlMobile2 = (Literal)e.Item.FindControl("ltrlMobile2");
                    Literal ltrlFax2 = (Literal)e.Item.FindControl("ltrlFax2");
                    Literal ltrlCity = (Literal)e.Item.FindControl("ltrlCity");

                    if (!string.IsNullOrEmpty(objWitnessMasterDAL.PhotoIdImageName))
                    {
                        imgPhotoIdImageName.ImageUrl = objWitnessMasterDAL.xsPhotoIdImageName;
                    }
                    else
                    {
                        imgPhotoIdImageName.ImageUrl = "img\\xs_NoImage.png";
                    }
                    ltrlWitnessName.Text = objWitnessMasterDAL.WitnessName;
                    ltrlIdNo.Text = objWitnessMasterDAL.IdNo;
                    ltrlAddress1.Text = objWitnessMasterDAL.Address1;
                    ltrlPhone1.Text = objWitnessMasterDAL.Phone1;
                    ltrlMobile1.Text = objWitnessMasterDAL.Mobile1;
                    ltrlFax1.Text = objWitnessMasterDAL.Fax1;
                    ltrlAddress2.Text = objWitnessMasterDAL.Address2;
                    ltrlPhone2.Text = objWitnessMasterDAL.Phone2;
                    ltrlMobile2.Text = objWitnessMasterDAL.Mobile2;
                    ltrlFax2.Text = objWitnessMasterDAL.Fax2;

                }
            }
            catch (Exception ex)
            {
                loanAppGlobals.SaveError(ex);
            }
        }

        protected void pgrWitnessMaster_ItemCommand(object sender, EventArgs e)
        {
            try
            {
                FillWitnessMaster();
            }
            catch (Exception ex)
            {
                loanAppGlobals.SaveError(ex);
            }
        }

        protected void lvWitnessMaster_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName.Equals("EditRecord", StringComparison.CurrentCultureIgnoreCase))
                {
                    GetWitnessMaster(Convert.ToInt32(((ListView)sender).DataKeys[e.Item.DataItemIndex].Value));
                }
                else if (e.CommandName.Equals("DeleteRecord", StringComparison.CurrentCultureIgnoreCase))
                {
                    loanUser.CheckRoleRights(loanRoleRights.DeleteRecord);

                    loanWitnessMasterDAL objWitnessMasterDAL = new loanWitnessMasterDAL();
                    objWitnessMasterDAL.WitnessMasterId = Convert.ToInt32(((ListView)sender).DataKeys[e.Item.DataItemIndex].Value);
                    loanRecordStatus rsStatus = objWitnessMasterDAL.DeleteWitnessMaster();
                    if (rsStatus == loanRecordStatus.Success)
                    {
                        loanAppGlobals.ShowMessage(loanMessagesDAL.DeleteSuccess, loanMessageIcon.Success);
                        FillWitnessMaster();
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
            if (loanSessionsDAL.GetSessionKeyValue("CurrentPageWitness") != null)
            {
                pgrWitnessMaster.CurrentPage = Convert.ToInt16(loanSessionsDAL.GetSessionKeyValue("CurrentPageWitness"));
            }
            if (loanSessionsDAL.GetSessionKeyValue("FilterWitness") != null)
            {
                loanWitnessMasterDAL objWitnessMasterDAL = (loanWitnessMasterDAL)loanSessionsDAL.GetSessionKeyValue("FilterWitness");
                txtFilterWitness.Text = objWitnessMasterDAL.WitnessName;
                txtFilterPhoneMobile.Text = objWitnessMasterDAL.Phone1;

            }
        }

        private void FillWitnessMaster()
        {

            loanWitnessMasterDAL objWitnessMasterDAL = new loanWitnessMasterDAL();
            objWitnessMasterDAL.WitnessName = txtFilterWitness.Text.Trim();
            objWitnessMasterDAL.Phone1 = txtFilterPhoneMobile.Text.Trim();


            loanSessionsDAL.SetSessionKeyValue("FilterWitness", objWitnessMasterDAL);
            loanSessionsDAL.SetSessionKeyValue("CurrentPageWitness", pgrWitnessMaster.CurrentPage);

            int TotalRecords;
            List<loanWitnessMasterDAL> lstWitnessMaster = objWitnessMasterDAL.SelectAllWitnessMasterPageWise(pgrWitnessMaster.StartRowIndex, pgrWitnessMaster.PageSize, out TotalRecords);
            pgrWitnessMaster.TotalRowCount = TotalRecords;

            if (lstWitnessMaster == null)
            {
                loanAppGlobals.ShowMessage(loanMessagesDAL.SelectAllFail, loanMessageIcon.Error);
                return;
            }

            if (lstWitnessMaster.Count == 0 && pgrWitnessMaster.TotalRowCount > 0)
            {
                pgrWitnessMaster_ItemCommand(pgrWitnessMaster, new EventArgs());
                return;
            }

            lvWitnessMaster.DataSource = lstWitnessMaster;
            lvWitnessMaster.DataBind();

            if (lstWitnessMaster.Count > 0)
            {
                int EndiIndex = pgrWitnessMaster.StartRowIndex + pgrWitnessMaster.PageSize < pgrWitnessMaster.TotalRowCount ? pgrWitnessMaster.StartRowIndex + pgrWitnessMaster.PageSize : pgrWitnessMaster.TotalRowCount;
                lblRecords.Text = string.Format(Resources.Messages.ResourceManager.GetString("Records"), pgrWitnessMaster.StartRowIndex + 1, EndiIndex, pgrWitnessMaster.TotalRowCount);
                lblRecords.Visible = true;
            }
            else
            {
                lblRecords.Visible = false;
            }

            if (pgrWitnessMaster.TotalRowCount <= pgrWitnessMaster.PageSize)
            {
                pgrWitnessMaster.Visible = false;
            }
            else
            {
                pgrWitnessMaster.Visible = true;
            }

        }


        private void GetWitnessMaster(int WitnessMasterId)
        {
            loanUser.CheckRoleRights(loanRoleRights.ViewRecord);

            loanWitnessMasterDAL objWitnessMasterDAL = new loanWitnessMasterDAL();
            objWitnessMasterDAL.WitnessMasterId = WitnessMasterId;
            if (!objWitnessMasterDAL.SelectWitnessMaster())
            {
                loanAppGlobals.ShowMessage(loanMessagesDAL.SelectFail, loanMessageIcon.Error);
                return;
            }
            hdnWitnessMasterId.Value = objWitnessMasterDAL.WitnessMasterId.ToString();
            txtWitnessName.Text = objWitnessMasterDAL.WitnessName;
            txtIdNo.Text = objWitnessMasterDAL.IdNo;
            hdnPhotoIdImageName.Value = objWitnessMasterDAL.PhotoIdImageName;
            hdnPhotoIdImageNameURL.Value = objWitnessMasterDAL.xsPhotoIdImageName;
            txtAddress1.Text = objWitnessMasterDAL.Address1;
            txtPhone1.Text = objWitnessMasterDAL.Phone1;
            txtMobile1.Text = objWitnessMasterDAL.Mobile1;
            txtFax1.Text = objWitnessMasterDAL.Fax1;
            txtAddress2.Text = objWitnessMasterDAL.Address2;
            txtPhone2.Text = objWitnessMasterDAL.Phone2;
            txtMobile2.Text = objWitnessMasterDAL.Mobile2;
            txtFax2.Text = objWitnessMasterDAL.Fax2;


            hdnModelWitness.Value = "show";
            hdnActionWitness.Value = "edit";
        }
        #endregion


    }
}
