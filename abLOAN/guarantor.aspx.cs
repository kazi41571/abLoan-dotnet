using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Web.UI;
using System.Web.UI.WebControls;
using loanLibrary;

namespace abLOAN
{
    public partial class guarantor : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                loanSessionsDAL.CheckSession();

                if (!Page.IsPostBack)
                {
                    //loanUser.CheckUserRights(loanUserRights.ViewGuarantor);
                    loanUser.CheckRoleRights(loanRoleRights.ViewList);

                    GetPageDefaults();

                    loanSessionsDAL.RemoveSessionAllKeyValue();

                    FillGuarantorMaster();
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
                pgrGuarantorMaster.CurrentPage = 1;
                FillGuarantorMaster();
            }
            catch (Exception ex)
            {
                loanAppGlobals.SaveError(ex);
            }
        }


        protected void btnDownload_Click(object sender, EventArgs e)
        {
            loanGuarantorMasterDAL objGuarantorMasterDAL = new loanGuarantorMasterDAL();
            objGuarantorMasterDAL.GuarantorName = txtFilterGuarantor.Text.Trim();
            objGuarantorMasterDAL.Phone1 = txtFilterPhoneMobile.Text.Trim();
            objGuarantorMasterDAL.linktoCompanyMasterId = ((loanUser)Session[loanSessionsDAL.UserSession]).CompanyMasterId;
          

            int TotalRecords;
            List<loanGuarantorMasterDAL> lstGuarantorMaster = objGuarantorMasterDAL.SelectAllGuarantorMasterPageWise(0, int.MaxValue, out TotalRecords);

            if (lstGuarantorMaster == null)
            {
                loanAppGlobals.ShowMessage(loanMessagesDAL.SelectAllFail, loanMessageIcon.Error);
                return;
            }

       

            string file = System.Configuration.ConfigurationManager.AppSettings["FileSavePath"] + "Guarantor.csv"; 




            var parameterNames = typeof(loanGuarantorMasterDAL).GetProperties()
            .Where(p => !p.GetGetMethod().IsVirtual)
            .Select(p => p.Name) 
            .ToArray();
 


            bool IsSuccess = loanAppGlobals.ExportCsv(lstGuarantorMaster, parameterNames, parameterNames, file);
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


        protected void btnReset_Click(object sender, EventArgs e)
        {
            try
            {
                txtFilterGuarantor.Text = string.Empty;
                txtFilterPhoneMobile.Text = string.Empty;


                pgrGuarantorMaster.CurrentPage = 1;
                FillGuarantorMaster();
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
                loanGuarantorMasterDAL objGuarantorMasterDAL = new loanGuarantorMasterDAL();
                objGuarantorMasterDAL.linktoCompanyMasterId = ((loanUser)Session[loanSessionsDAL.UserSession]).CompanyMasterId;
                objGuarantorMasterDAL.GuarantorName = txtGuarantorName.Text.Trim();
                objGuarantorMasterDAL.IdNo = txtIdNo.Text.Trim();
                if (fuPhotoIdImageName.HasFile)
                {
                    objGuarantorMasterDAL.PhotoIdImageName = objGuarantorMasterDAL.GuarantorMasterId + System.IO.Path.GetExtension(fuPhotoIdImageName.FileName);
                    string ImageSavePath = System.Configuration.ConfigurationManager.AppSettings["ImageSavePath"] + "Guarantor\\";
                    if (!Directory.Exists(ImageSavePath))
                    {
                        Directory.CreateDirectory(ImageSavePath);
                    }
                    fuPhotoIdImageName.SaveAs(ImageSavePath + objGuarantorMasterDAL.PhotoIdImageName);

                    loanGlobalsDAL.CreateThumbImages(objGuarantorMasterDAL.PhotoIdImageName, ImageSavePath);

                }
                else if (hdnPhotoIdImageName.Value != string.Empty)
                {
                    objGuarantorMasterDAL.PhotoIdImageName = hdnPhotoIdImageName.Value;
                }
                objGuarantorMasterDAL.Address1 = txtAddress1.Text.Trim();
                objGuarantorMasterDAL.Phone1 = txtPhone1.Text.Trim();
                objGuarantorMasterDAL.Mobile1 = txtMobile1.Text.Trim();
                objGuarantorMasterDAL.Fax1 = txtFax1.Text.Trim();
                objGuarantorMasterDAL.Address2 = txtAddress2.Text.Trim();
                objGuarantorMasterDAL.Phone2 = txtPhone2.Text.Trim();
                objGuarantorMasterDAL.Mobile2 = txtMobile2.Text.Trim();
                objGuarantorMasterDAL.Fax2 = txtFax2.Text.Trim();


                objGuarantorMasterDAL.UpdateDateTime = loanGlobalsDAL.GetCurrentDateTime();
                objGuarantorMasterDAL.SessionId = ((loanUser)Session[loanSessionsDAL.UserSession]).SessionId;

                if (string.IsNullOrEmpty(hdnActionGuarantor.Value))
                {
                    loanUser.CheckRoleRights(loanRoleRights.AddRecord);

                    loanRecordStatus rsStatus = objGuarantorMasterDAL.InsertGuarantorMaster();
                    if (rsStatus == loanRecordStatus.Error)
                    {
                        loanAppGlobals.ShowMessage(loanMessagesDAL.InsertFail, loanMessageIcon.Error);
                        return;
                    }
                    else if (rsStatus == loanRecordStatus.RecordAlreadyExist)
                    {
                        loanAppGlobals.ShowMessage(loanMessagesDAL.AlreadyExist, loanMessageIcon.Warning);
                        hdnModelGuarantor.Value = "show";
                        return;
                    }
                    else if (rsStatus == loanRecordStatus.Success)
                    {
                        loanAppGlobals.ShowMessage(loanMessagesDAL.InsertSuccess, loanMessageIcon.Success);
                        if (((Button)sender).ID.Equals("btnSaveAndNew"))
                        {
                            hdnModelGuarantor.Value = "clear";
                        }
                        else
                        {
                            hdnModelGuarantor.Value = "hide";
                        }
                        FillGuarantorMaster();
                    }
                }
                else
                {
                    loanUser.CheckRoleRights(loanRoleRights.EditRecord);

                    objGuarantorMasterDAL.GuarantorMasterId = Convert.ToInt32(hdnGuarantorMasterId.Value);
                    loanRecordStatus rsStatus = objGuarantorMasterDAL.UpdateGuarantorMaster();
                    if (rsStatus == loanRecordStatus.Error)
                    {
                        loanAppGlobals.ShowMessage(loanMessagesDAL.UpdateFail, loanMessageIcon.Error);
                        return;
                    }
                    else if (rsStatus == loanRecordStatus.RecordAlreadyExist)
                    {
                        loanAppGlobals.ShowMessage(loanMessagesDAL.AlreadyExist, loanMessageIcon.Warning);
                        hdnModelGuarantor.Value = "show";
                        return;
                    }
                    else if (rsStatus == loanRecordStatus.Success)
                    {
                        loanAppGlobals.ShowMessage(loanMessagesDAL.UpdateSuccess, loanMessageIcon.Success);
                        hdnModelGuarantor.Value = "hide";
                        FillGuarantorMaster();
                    }
                }
            }
            catch (Exception ex)
            {
                loanAppGlobals.SaveError(ex);
            }
        }

        #region List Methods

        protected void lvGuarantorMaster_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            try
            {
                if (e.Item.ItemType == ListViewItemType.DataItem)
                {
                    loanGuarantorMasterDAL objGuarantorMasterDAL = (loanGuarantorMasterDAL)e.Item.DataItem;

                    Image imgPhotoIdImageName = (Image)e.Item.FindControl("imgPhotoIdImageName");
                    Literal ltrlGuarantorName = (Literal)e.Item.FindControl("ltrlGuarantorName");
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

                    if (!string.IsNullOrEmpty(objGuarantorMasterDAL.PhotoIdImageName))
                    {
                        imgPhotoIdImageName.ImageUrl = objGuarantorMasterDAL.xsPhotoIdImageName;
                    }
                    else
                    {
                        imgPhotoIdImageName.ImageUrl = "img\\xs_NoImage.png";
                    }
                    ltrlGuarantorName.Text = objGuarantorMasterDAL.GuarantorName;
                    ltrlIdNo.Text = objGuarantorMasterDAL.IdNo;
                    ltrlAddress1.Text = objGuarantorMasterDAL.Address1;
                    ltrlPhone1.Text = objGuarantorMasterDAL.Phone1;
                    ltrlMobile1.Text = objGuarantorMasterDAL.Mobile1;
                    ltrlFax1.Text = objGuarantorMasterDAL.Fax1;
                    ltrlAddress2.Text = objGuarantorMasterDAL.Address2;
                    ltrlPhone2.Text = objGuarantorMasterDAL.Phone2;
                    ltrlMobile2.Text = objGuarantorMasterDAL.Mobile2;
                    ltrlFax2.Text = objGuarantorMasterDAL.Fax2;

                }
            }
            catch (Exception ex)
            {
                loanAppGlobals.SaveError(ex);
            }
        }

        protected void pgrGuarantorMaster_ItemCommand(object sender, EventArgs e)
        {
            try
            {
                FillGuarantorMaster();
            }
            catch (Exception ex)
            {
                loanAppGlobals.SaveError(ex);
            }
        }

        protected void lvGuarantorMaster_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName.Equals("EditRecord", StringComparison.CurrentCultureIgnoreCase))
                {
                    GetGuarantorMaster(Convert.ToInt32(((ListView)sender).DataKeys[e.Item.DataItemIndex].Value));
                }
                else if (e.CommandName.Equals("DeleteRecord", StringComparison.CurrentCultureIgnoreCase))
                {
                    loanUser.CheckRoleRights(loanRoleRights.DeleteRecord);

                    loanGuarantorMasterDAL objGuarantorMasterDAL = new loanGuarantorMasterDAL();
                    objGuarantorMasterDAL.GuarantorMasterId = Convert.ToInt32(((ListView)sender).DataKeys[e.Item.DataItemIndex].Value);
                    loanRecordStatus rsStatus = objGuarantorMasterDAL.DeleteGuarantorMaster();
                    if (rsStatus == loanRecordStatus.Success)
                    {
                        loanAppGlobals.ShowMessage(loanMessagesDAL.DeleteSuccess, loanMessageIcon.Success);
                        FillGuarantorMaster();
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
            if (loanSessionsDAL.GetSessionKeyValue("CurrentPageGuarantor") != null)
            {
                pgrGuarantorMaster.CurrentPage = Convert.ToInt16(loanSessionsDAL.GetSessionKeyValue("CurrentPageGuarantor"));
            }
            if (loanSessionsDAL.GetSessionKeyValue("FilterGuarantor") != null)
            {
                loanGuarantorMasterDAL objGuarantorMasterDAL = (loanGuarantorMasterDAL)loanSessionsDAL.GetSessionKeyValue("FilterGuarantor");
                txtFilterGuarantor.Text = objGuarantorMasterDAL.GuarantorName;
                txtFilterPhoneMobile.Text = objGuarantorMasterDAL.Phone1;

            }
        }

        private void FillGuarantorMaster()
        {

            loanGuarantorMasterDAL objGuarantorMasterDAL = new loanGuarantorMasterDAL();
            objGuarantorMasterDAL.GuarantorName = txtFilterGuarantor.Text.Trim();
            objGuarantorMasterDAL.Phone1 = txtFilterPhoneMobile.Text.Trim();

            loanSessionsDAL.SetSessionKeyValue("FilterGuarantor", objGuarantorMasterDAL);
            loanSessionsDAL.SetSessionKeyValue("CurrentPageGuarantor", pgrGuarantorMaster.CurrentPage);

            int TotalRecords;
            List<loanGuarantorMasterDAL> lstGuarantorMaster = objGuarantorMasterDAL.SelectAllGuarantorMasterPageWise(pgrGuarantorMaster.StartRowIndex, pgrGuarantorMaster.PageSize, out TotalRecords);
            pgrGuarantorMaster.TotalRowCount = TotalRecords;

            if (lstGuarantorMaster == null)
            {
                loanAppGlobals.ShowMessage(loanMessagesDAL.SelectAllFail, loanMessageIcon.Error);
                return;
            }

            if (lstGuarantorMaster.Count == 0 && pgrGuarantorMaster.TotalRowCount > 0)
            {
                pgrGuarantorMaster_ItemCommand(pgrGuarantorMaster, new EventArgs());
                return;
            }

            lvGuarantorMaster.DataSource = lstGuarantorMaster;
            lvGuarantorMaster.DataBind();

            if (lstGuarantorMaster.Count > 0)
            {
                int EndiIndex = pgrGuarantorMaster.StartRowIndex + pgrGuarantorMaster.PageSize < pgrGuarantorMaster.TotalRowCount ? pgrGuarantorMaster.StartRowIndex + pgrGuarantorMaster.PageSize : pgrGuarantorMaster.TotalRowCount;
                lblRecords.Text = string.Format(Resources.Messages.ResourceManager.GetString("Records"), pgrGuarantorMaster.StartRowIndex + 1, EndiIndex, pgrGuarantorMaster.TotalRowCount);
                lblRecords.Visible = true;
            }
            else
            {
                lblRecords.Visible = false;
            }

            if (pgrGuarantorMaster.TotalRowCount <= pgrGuarantorMaster.PageSize)
            {
                pgrGuarantorMaster.Visible = false;
            }
            else
            {
                pgrGuarantorMaster.Visible = true;
            }

        }


        private void GetGuarantorMaster(int GuarantorMasterId)
        {
            loanUser.CheckRoleRights(loanRoleRights.ViewRecord);

            loanGuarantorMasterDAL objGuarantorMasterDAL = new loanGuarantorMasterDAL();
            objGuarantorMasterDAL.GuarantorMasterId = GuarantorMasterId;
            if (!objGuarantorMasterDAL.SelectGuarantorMaster())
            {
                loanAppGlobals.ShowMessage(loanMessagesDAL.SelectFail, loanMessageIcon.Error);
                return;
            }
            hdnGuarantorMasterId.Value = objGuarantorMasterDAL.GuarantorMasterId.ToString();
            txtGuarantorName.Text = objGuarantorMasterDAL.GuarantorName;
            txtIdNo.Text = objGuarantorMasterDAL.IdNo;
            hdnPhotoIdImageName.Value = objGuarantorMasterDAL.PhotoIdImageName;
            hdnPhotoIdImageNameURL.Value = objGuarantorMasterDAL.xsPhotoIdImageName;
            txtAddress1.Text = objGuarantorMasterDAL.Address1;
            txtPhone1.Text = objGuarantorMasterDAL.Phone1;
            txtMobile1.Text = objGuarantorMasterDAL.Mobile1;
            txtFax1.Text = objGuarantorMasterDAL.Fax1;
            txtAddress2.Text = objGuarantorMasterDAL.Address2;
            txtPhone2.Text = objGuarantorMasterDAL.Phone2;
            txtMobile2.Text = objGuarantorMasterDAL.Mobile2;
            txtFax2.Text = objGuarantorMasterDAL.Fax2;


            hdnModelGuarantor.Value = "show";
            hdnActionGuarantor.Value = "edit";
        }
        #endregion


    }
}
