using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using loanLibrary;

namespace abLOAN
{
    public partial class company : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                //loanSessionsDAL.CheckSession();
                if (!Page.IsPostBack)
                {
                    //loanUser.CheckUserRights(loanUserRights.ViewCompany);
                    loanUser.CheckRoleRights(loanRoleRights.ViewList);

                    //loanSessionsDAL.RemoveSessionAllKeyValue();
                    FillCompanyMaster();
                }
            }
            catch (Exception ex)
            {
                loanAppGlobals.SaveError(ex);
            }
        }

        protected void btnDownload_Click(object sender, EventArgs e)
        {
            loanCompanyMasterDAL objCompanyMasterDAL = new loanCompanyMasterDAL();
            objCompanyMasterDAL.CompanyName = txtCompanyName.Text.Trim();
            objCompanyMasterDAL.CompanyDetails = txtCompanyDetails.Text.Trim();
            objCompanyMasterDAL.Address = txtAddress.Text.Trim();
            objCompanyMasterDAL.ContactNo = txtContactNo.Text.Trim() ; 


            int TotalRecords;
            List<loanCompanyMasterDAL> lstCompanyMaster = objCompanyMasterDAL.SelectAllCompanyMasterPageWise(0, int.MaxValue, out TotalRecords);

            if (lstCompanyMaster == null)
            {
                loanAppGlobals.ShowMessage(loanMessagesDAL.SelectAllFail, loanMessageIcon.Error);
                return;
            }



            string file = System.Configuration.ConfigurationManager.AppSettings["FileSavePath"] + "Guarantor.csv";




            var parameterNames = typeof(loanCompanyMasterDAL).GetProperties()
            .Where(p => !p.GetGetMethod().IsVirtual)
            .Select(p => p.Name)
            .ToArray();



            bool IsSuccess = loanAppGlobals.ExportCsv(lstCompanyMaster, parameterNames, parameterNames, file);
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


        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                loanCompanyMasterDAL objCompanyMasterDAL = new loanCompanyMasterDAL();
                objCompanyMasterDAL.CompanyName = txtCompanyName.Text.Trim();
                objCompanyMasterDAL.CompanyDetails = txtCompanyDetails.Text.Trim();
                objCompanyMasterDAL.Address = txtAddress.Text.Trim();
                objCompanyMasterDAL.ContactNo = txtContactNo.Text.Trim();
                if (fuLogoImageName.HasFile)
                {
                    objCompanyMasterDAL.LogoImageName = DateTime.Now.ToString(loanGlobalsDAL.UniqueKey) + "_" + fuLogoImageName.FileName;
                    string ImageSavePath = System.Configuration.ConfigurationManager.AppSettings["ImageSavePath"] + "Company\\";
                    if (!Directory.Exists(ImageSavePath))
                    {
                        Directory.CreateDirectory(ImageSavePath);
                    }
                    fuLogoImageName.SaveAs(ImageSavePath + objCompanyMasterDAL.LogoImageName);

                    loanGlobalsDAL.CreateThumbImages(objCompanyMasterDAL.LogoImageName, ImageSavePath);

                }
                else if (hdnLogoImageName.Value != string.Empty)
                {
                    objCompanyMasterDAL.LogoImageName = hdnLogoImageName.Value;
                }
                objCompanyMasterDAL.UpdateDateTime = loanGlobalsDAL.GetCurrentDateTime();
                objCompanyMasterDAL.SessionId = ((loanUser)Session[loanSessionsDAL.UserSession]).SessionId;
                if (string.IsNullOrEmpty(hdnActionCompany.Value))
                {

                    loanUser.CheckRoleRights(loanRoleRights.AddRecord);

                    loanRecordStatus rsStatus = objCompanyMasterDAL.InsertCompanyMaster();
                    if (rsStatus == loanRecordStatus.Error)
                    {
                        loanAppGlobals.ShowMessage(loanMessagesDAL.InsertFail, loanMessageIcon.Error);
                        return;
                    }
                    else if (rsStatus == loanRecordStatus.RecordAlreadyExist)
                    {
                        loanAppGlobals.ShowMessage(loanMessagesDAL.AlreadyExist, loanMessageIcon.Warning);
                        hdnModelCompany.Value = "show";
                        return;
                    }
                    else if (rsStatus == loanRecordStatus.Success)
                    {
                        loanAppGlobals.ShowMessage(loanMessagesDAL.InsertSuccess, loanMessageIcon.Success);
                        if (((Button)sender).ID.Equals("btnSaveAndNew"))
                        {
                            hdnModelCompany.Value = "clear";
                        }
                        else
                        {
                            hdnModelCompany.Value = "hide";
                        }
                        FillCompanyMaster();
                    }
                }
                else
                {
                    loanUser.CheckRoleRights(loanRoleRights.EditRecord);

                    objCompanyMasterDAL.UpdateDateTime = loanGlobalsDAL.GetCurrentDateTime();
                    objCompanyMasterDAL.CompanyMasterId = Convert.ToInt32(hdnCompanyMasterId.Value);
                    loanRecordStatus rsStatus = objCompanyMasterDAL.UpdateCompanyMaster();
                    if (rsStatus == loanRecordStatus.Error)
                    {
                        loanAppGlobals.ShowMessage(loanMessagesDAL.UpdateFail, loanMessageIcon.Error);
                        return;
                    }
                    else if (rsStatus == loanRecordStatus.RecordAlreadyExist)
                    {
                        loanAppGlobals.ShowMessage(loanMessagesDAL.AlreadyExist, loanMessageIcon.Warning);
                        hdnModelCompany.Value = "show";
                        return;
                    }
                    else if (rsStatus == loanRecordStatus.Success)
                    {
                        loanAppGlobals.ShowMessage(loanMessagesDAL.UpdateSuccess, loanMessageIcon.Success);
                        hdnModelCompany.Value = "hide";
                        FillCompanyMaster();
                    }
                }
            }
            catch (Exception ex)
            {
                loanAppGlobals.SaveError(ex);
            }
        }

        #region List Methods

        protected void lvCompanyMaster_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            try
            {
                if (e.Item.ItemType == ListViewItemType.DataItem)
                {
                    loanCompanyMasterDAL objCompanyMasterDAL = (loanCompanyMasterDAL)e.Item.DataItem;

                    Image imgLogoImageName = (Image)e.Item.FindControl("imgLogoImageName");
                    Literal ltrlCompanyName = (Literal)e.Item.FindControl("ltrlCompanyName");
                    Literal ltrlCompanyDetails = (Literal)e.Item.FindControl("ltrlCompanyDetails");
                    Literal ltrlAddress = (Literal)e.Item.FindControl("ltrlAddress");
                    Literal ltrlContactNo = (Literal)e.Item.FindControl("ltrlContactNo");

                    if (!string.IsNullOrEmpty(objCompanyMasterDAL.LogoImageName))
                    {
                        imgLogoImageName.ImageUrl = objCompanyMasterDAL.xsLogoImageName;
                    }
                    else
                    {
                        imgLogoImageName.ImageUrl = "img\\xs_NoImage.png";
                    }
                    ltrlCompanyName.Text = objCompanyMasterDAL.CompanyName;
                    ltrlCompanyDetails.Text = objCompanyMasterDAL.CompanyDetails;
                    ltrlAddress.Text = objCompanyMasterDAL.Address;
                    ltrlContactNo.Text = objCompanyMasterDAL.ContactNo;
                }
            }
            catch (Exception ex)
            {
                loanAppGlobals.SaveError(ex);
            }
        }

        protected void pgrCompanyMaster_ItemCommand(object sender, EventArgs e)
        {
            try
            {
                FillCompanyMaster();
            }
            catch (Exception ex)
            {
                loanAppGlobals.SaveError(ex);
            }
        }

        protected void lvCompanyMaster_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName.Equals("EditRecord", StringComparison.CurrentCultureIgnoreCase))
                {
                    GetCompanyMaster(Convert.ToInt32(((ListView)sender).DataKeys[e.Item.DataItemIndex].Value));
                }
                else if (e.CommandName.Equals("DeleteRecord", StringComparison.CurrentCultureIgnoreCase))
                {

                    loanUser.CheckRoleRights(loanRoleRights.DeleteRecord);


                    loanCompanyMasterDAL objCompanyMasterDAL = new loanCompanyMasterDAL();
                    objCompanyMasterDAL.CompanyMasterId = Convert.ToInt32(((ListView)sender).DataKeys[e.Item.DataItemIndex].Value);
                    loanRecordStatus rsStatus = objCompanyMasterDAL.DeleteCompanyMaster();
                    if (rsStatus == loanRecordStatus.Success)
                    {
                        loanAppGlobals.ShowMessage(loanMessagesDAL.DeleteSuccess, loanMessageIcon.Success);
                        FillCompanyMaster();
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
        private void FillCompanyMaster()
        {

            loanCompanyMasterDAL objCompanyMasterDAL = new loanCompanyMasterDAL();

            int TotalRecords;
            List<loanCompanyMasterDAL> lstCompanyMaster = objCompanyMasterDAL.SelectAllCompanyMasterPageWise(pgrCompanyMaster.StartRowIndex, pgrCompanyMaster.PageSize, out TotalRecords);
            pgrCompanyMaster.TotalRowCount = TotalRecords;

            if (lstCompanyMaster == null)
            {
                loanAppGlobals.ShowMessage(loanMessagesDAL.SelectAllFail, loanMessageIcon.Error);
                return;
            }

            if (lstCompanyMaster.Count == 0 && pgrCompanyMaster.TotalRowCount > 0)
            {
                pgrCompanyMaster_ItemCommand(pgrCompanyMaster, new EventArgs());
                return;
            }

            lvCompanyMaster.DataSource = lstCompanyMaster;
            lvCompanyMaster.DataBind();

            if (lstCompanyMaster.Count > 0)
            {
                int EndiIndex = pgrCompanyMaster.StartRowIndex + pgrCompanyMaster.PageSize < pgrCompanyMaster.TotalRowCount ? pgrCompanyMaster.StartRowIndex + pgrCompanyMaster.PageSize : pgrCompanyMaster.TotalRowCount;
                lblRecords.Text = string.Format(Resources.Messages.ResourceManager.GetString("Records"), pgrCompanyMaster.StartRowIndex + 1, EndiIndex, pgrCompanyMaster.TotalRowCount);
                lblRecords.Visible = true;
            }
            else
            {
                lblRecords.Visible = false;
            }

            if (pgrCompanyMaster.TotalRowCount <= pgrCompanyMaster.PageSize)
            {
                pgrCompanyMaster.Visible = false;
            }
            else
            {
                pgrCompanyMaster.Visible = true;
            }

        }


        private void GetCompanyMaster(int CompanyMasterId)
        {
            loanUser.CheckRoleRights(loanRoleRights.ViewRecord);

            loanCompanyMasterDAL objCompanyMasterDAL = new loanCompanyMasterDAL();
            objCompanyMasterDAL.CompanyMasterId = CompanyMasterId;
            if (!objCompanyMasterDAL.SelectCompanyMaster())
            {
                loanAppGlobals.ShowMessage(loanMessagesDAL.SelectFail, loanMessageIcon.Error);
                return;
            }
            hdnCompanyMasterId.Value = objCompanyMasterDAL.CompanyMasterId.ToString();
            txtCompanyName.Text = objCompanyMasterDAL.CompanyName;
            txtCompanyDetails.Text = objCompanyMasterDAL.CompanyDetails;
            txtAddress.Text = objCompanyMasterDAL.Address;
            txtContactNo.Text = objCompanyMasterDAL.ContactNo;
            hdnLogoImageName.Value = objCompanyMasterDAL.LogoImageName;
            hdnLogoImageNameURL.Value = objCompanyMasterDAL.xsLogoImageName;

            hdnModelCompany.Value = "show";
            hdnActionCompany.Value = "edit";
        }
        #endregion


    }
}
