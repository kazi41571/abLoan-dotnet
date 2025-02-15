using System;
using System.Collections.Generic;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;
using loanLibrary;

namespace abLOAN
{
    public partial class customer : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                loanSessionsDAL.CheckSession();

                if (!Page.IsPostBack)
                {
                    //loanUser.CheckUserRights(loanUserRights.ViewCustomer);
                    loanUser.CheckRoleRights(loanRoleRights.ViewList);

                    GetContractStatus();

                    GetPageDefaults();

                    //loanSessionsDAL.RemoveSessionAllKeyValue();
                    if (Request.QueryString.ToString().Contains("id="))
                    {
                        int id = Convert.ToInt32(Request.QueryString["id"].ToString());
                        GetCustomerMaster(id);
                    }
                    FillCustomerMaster();
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
                pgrCustomerMaster.CurrentPage = 1;
                FillCustomerMaster();
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
                txtFilterPhoneMobile.Text = string.Empty;
                ddlFilterVerification.SelectedIndex = 0;
                ddlFilterHasurl.SelectedIndex = 0;
                ddlFilterInvalidMobile.SelectedIndex = 0;
                ddlFilterInvalidIdNo.SelectedIndex = 0;
                ddlFilterContractStatus.SelectedIndex = 0;

                pgrCustomerMaster.CurrentPage = 1;
                FillCustomerMaster();
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
                loanCustomerMasterDAL objCustomerMasterDAL = new loanCustomerMasterDAL();
                objCustomerMasterDAL.linktoCompanyMasterId = ((loanUser)Session[loanSessionsDAL.UserSession]).CompanyMasterId;
                objCustomerMasterDAL.CustomerName = txtCustomerName.Text.Trim();
                objCustomerMasterDAL.IdNo = txtIdNo.Text.Trim();
                objCustomerMasterDAL.Links = txtLinks.Text.Trim();
                objCustomerMasterDAL.Gender = rbGender.SelectedValue;
                //if (fuPhotoIdImageName.HasFile)
                //{
                //    objCustomerMasterDAL.PhotoIdImageName = txtIdNo.Text.ToString() + "_" + fuPhotoIdImageName.FileName;
                //    string ImageSavePath = System.Configuration.ConfigurationManager.AppSettings["ImageSavePath"] + "Customer\\";
                //    if (!Directory.Exists(ImageSavePath))
                //    {
                //        Directory.CreateDirectory(ImageSavePath);
                //    }
                //    fuPhotoIdImageName.SaveAs(ImageSavePath + objCustomerMasterDAL.PhotoIdImageName);
                //    loanGlobalsDAL.CreateThumbImages(objCustomerMasterDAL.PhotoIdImageName, ImageSavePath);
                //}
                //else if (hdnPhotoIdImageName.Value != string.Empty)
                //{
                //    objCustomerMasterDAL.PhotoIdImageName = hdnPhotoIdImageName.Value;
                //}
                objCustomerMasterDAL.Occupation = txtOccupation.Text.Trim();
                objCustomerMasterDAL.PlaceOfWork = txtPlaceOfWork.Text.Trim();
                objCustomerMasterDAL.CityOfResidence = txtCityOfResidence.Text.Trim();
                objCustomerMasterDAL.District = txtDistrict.Text.Trim();
                objCustomerMasterDAL.Mobile1 = txtMobile1.Text.Trim();
                objCustomerMasterDAL.Mobile2 = txtMobile2.Text.Trim();
                objCustomerMasterDAL.Mobile3 = txtMobile3.Text.Trim();

                objCustomerMasterDAL.Address1 = txtAddress1.Text.Trim();
                objCustomerMasterDAL.Phone1 = txtPhone1.Text.Trim();
                objCustomerMasterDAL.Relation1 = ddlRelation1.SelectedValue;
                objCustomerMasterDAL.ContactName1 = txtContactName1.Text.Trim();
                //objCustomerMasterDAL.Address2 = txtAddress2.Text.Trim();
                objCustomerMasterDAL.Phone2 = txtPhone2.Text.Trim();
                objCustomerMasterDAL.Relation2 = ddlRelation2.SelectedValue;
                objCustomerMasterDAL.ContactName2 = txtContactName2.Text.Trim();
                objCustomerMasterDAL.BankAccountNumber = txtBankAccountNumber.Text.Trim();
                objCustomerMasterDAL.BankAccountNumber2 = txtBankAccountNumber2.Text.Trim();
                objCustomerMasterDAL.BankAccountNumber3 = txtBankAccountNumber3.Text.Trim();
                objCustomerMasterDAL.BankAccountNumber4 = txtBankAccountNumber4.Text.Trim();

                objCustomerMasterDAL.IsRedFlag = chkIsIsRedFlag.Checked;

                objCustomerMasterDAL.UpdateDateTime = loanGlobalsDAL.GetCurrentDateTime();
                objCustomerMasterDAL.SessionId = ((loanUser)Session[loanSessionsDAL.UserSession]).SessionId;
                if (string.IsNullOrEmpty(hdnActionCustomer.Value))
                {

                    loanUser.CheckRoleRights(loanRoleRights.AddRecord);

                    loanRecordStatus rsStatus = objCustomerMasterDAL.InsertCustomerMaster();
                    if (rsStatus == loanRecordStatus.Error)
                    {
                        loanAppGlobals.ShowMessage(loanMessagesDAL.InsertFail, loanMessageIcon.Error);
                        return;
                    }
                    else if (rsStatus == loanRecordStatus.RecordAlreadyExist)
                    {
                        loanAppGlobals.ShowMessage(loanMessagesDAL.AlreadyExist, loanMessageIcon.Warning);
                        hdnModelCustomer.Value = "show";
                        return;
                    }
                    else if (rsStatus == loanRecordStatus.Success)
                    {
                        loanAppGlobals.ShowMessage(loanMessagesDAL.InsertSuccess, loanMessageIcon.Success);
                        if (((Button)sender).ID.Equals("btnSaveAndNew"))
                        {
                            hdnModelCustomer.Value = "clear";
                        }
                        else
                        {
                            hdnModelCustomer.Value = "hide";
                        }
                        FillCustomerMaster();
                    }
                }
                else
                {
                    loanUser.CheckRoleRights(loanRoleRights.EditRecord);

                    objCustomerMasterDAL.UpdateDateTime = loanGlobalsDAL.GetCurrentDateTime();
                    objCustomerMasterDAL.CustomerMasterId = Convert.ToInt32(hdnCustomerMasterId.Value);
                    loanRecordStatus rsStatus = objCustomerMasterDAL.UpdateCustomerMaster();
                    if (rsStatus == loanRecordStatus.Error)
                    {
                        loanAppGlobals.ShowMessage(loanMessagesDAL.UpdateFail, loanMessageIcon.Error);
                        return;
                    }
                    else if (rsStatus == loanRecordStatus.RecordAlreadyExist)
                    {
                        loanAppGlobals.ShowMessage(loanMessagesDAL.AlreadyExist, loanMessageIcon.Warning);
                        hdnModelCustomer.Value = "show";
                        return;
                    }
                    else if (rsStatus == loanRecordStatus.Success)
                    {
                        loanAppGlobals.ShowMessage(loanMessagesDAL.UpdateSuccess, loanMessageIcon.Success);
                        hdnModelCustomer.Value = "hide";
                        FillCustomerMaster();
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
            loanCustomerMasterDAL objCustomerMasterDAL = new loanCustomerMasterDAL();
            objCustomerMasterDAL.CustomerName = txtFilterCustomer.Text.Trim();
            objCustomerMasterDAL.Phone1 = txtFilterPhoneMobile.Text.Trim();
            objCustomerMasterDAL.linktoCompanyMasterId = ((loanUser)Session[loanSessionsDAL.UserSession]).CompanyMasterId;
            if (ddlFilterVerification.SelectedValue == "Yes")
            {
                objCustomerMasterDAL.IsVerified = true;
            }
            else if (ddlFilterVerification.SelectedValue == "No")
            {
                objCustomerMasterDAL.IsVerified = false;
            }
            if (ddlFilterHasurl.SelectedValue == "Yes")
            {
                objCustomerMasterDAL.Hasurl = true;
            }
            else if (ddlFilterHasurl.SelectedValue == "No")
            {
                objCustomerMasterDAL.Hasurl = false;
            }
            if (ddlFilterInvalidMobile.SelectedValue == "Yes")
            {
                objCustomerMasterDAL.InvalidMobile = true;
            }
            else if (ddlFilterInvalidMobile.SelectedValue == "No")
            {
                objCustomerMasterDAL.InvalidMobile = false;
            }
            if (ddlFilterInvalidIdNo.SelectedValue == "Yes")
            {
                objCustomerMasterDAL.InvalidIdNo = true;
            }
            else if (ddlFilterInvalidIdNo.SelectedValue == "No")
            {
                objCustomerMasterDAL.InvalidIdNo = false;
            }
            if (ddlFilterContractStatus.SelectedValue != string.Empty)
            {
                objCustomerMasterDAL.linktoContractStatusMasterId = Convert.ToInt32(ddlFilterContractStatus.SelectedValue);
            }

            int TotalRecords;
            List<loanCustomerMasterDAL> lstCustomerMaster = objCustomerMasterDAL.SelectAllCustomerMasterPageWise(0, int.MaxValue, out TotalRecords);

            if (lstCustomerMaster == null)
            {
                loanAppGlobals.ShowMessage(loanMessagesDAL.SelectAllFail, loanMessageIcon.Error);
                return;
            }

            string file = System.Configuration.ConfigurationManager.AppSettings["FileSavePath"] + "Customer.csv";

            string[] headers = { "IsRedFlag" , "Gender", "Customer", "Id No", "Mobile 1", "Mobile 2", "Mobile 3", "Occupation", "Place Of Work", "City Of Residence", "District", "Address 1", "Phone 1", "Contact Person Name 1", "Address 2", "Phone 2", "Contact Person Name 2", "Links", "Verifier", "Modifier" };
            string[] columns = { "IsRedFlag", "Gender","CustomerName", "IdNo", "Mobile1", "Mobile2", "Mobile3", "Occupation", "PlaceOfWork", "CityOfResidence", "District", "Address1", "Phone1", "ContactName1", "Address2", "Phone2", "ContactName2", "Links", "VerifiedBy", "ModifiedBy" };

            bool IsSuccess = loanAppGlobals.ExportCsv(lstCustomerMaster, headers, columns, file);
            if (!IsSuccess)
            {
                loanAppGlobals.ShowMessage(loanMessagesDAL.Exception, loanMessageIcon.Error);
                return;
            }

            loanAppGlobals.SendOutFile(file, Path.GetFileName(file));
        }

        #region List Methods

        protected void lvCustomerMaster_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            try
            {
                if (e.Item.ItemType == ListViewItemType.DataItem)
                {
                    loanCustomerMasterDAL objCustomerMasterDAL = (loanCustomerMasterDAL)e.Item.DataItem;

                    //Image imgPhotoIdImageName = (Image)e.Item.FindControl("imgPhotoIdImageName");
                    Literal ltrlCustomerName = (Literal)e.Item.FindControl("ltrlCustomerName");
                    HyperLink hlnkCustomerName = (HyperLink)e.Item.FindControl("hlnkCustomerName");
                    Literal ltrlIdNo = (Literal)e.Item.FindControl("ltrlIdNo");
                    Literal ltrlMobile1 = (Literal)e.Item.FindControl("ltrlMobile1");
                    Literal ltrlMobile2 = (Literal)e.Item.FindControl("ltrlMobile2");
                    Literal ltrlMobile3 = (Literal)e.Item.FindControl("ltrlMobile3");
                    Literal ltrlOccupation = (Literal)e.Item.FindControl("ltrlOccupation");
                    Literal ltrlGender = (Literal)e.Item.FindControl("ltrlGender");
                    Literal ltrlPlaceOfWork = (Literal)e.Item.FindControl("ltrlPlaceOfWork");
                    Literal ltrlCityOfResidence = (Literal)e.Item.FindControl("ltrlCityOfResidence");
                    Literal ltrlAddress1 = (Literal)e.Item.FindControl("ltrlAddress1");
                    Literal ltrlDistrict = (Literal)e.Item.FindControl("ltrlDistrict");
                    Literal ltrlPhone1 = (Literal)e.Item.FindControl("ltrlPhone1");
                    Literal ltrlRelation1 = (Literal)e.Item.FindControl("ltrlRelation1");
                    Literal ltrlContactName1 = (Literal)e.Item.FindControl("ltrlContactName1");
                    //Literal ltrlAddress2 = (Literal)e.Item.FindControl("ltrlAddress2");
                    Literal ltrlPhone2 = (Literal)e.Item.FindControl("ltrlPhone2");
                    Literal ltrlRelation2 = (Literal)e.Item.FindControl("ltrlRelation2");
                    Literal ltrlContactName2 = (Literal)e.Item.FindControl("ltrlContactName2");
                    Literal ltrlModifiedBy = (Literal)e.Item.FindControl("ltrlModifiedBy");
                    ltrlModifiedBy.Text = objCustomerMasterDAL.ModifiedBy;
                    Literal ltrlModifiedDateTime = (Literal)e.Item.FindControl("ltrlModifiedDateTime");
                    ltrlModifiedDateTime.Text = loanGlobalsDAL.ConvertDateTimeToString(objCustomerMasterDAL.UpdateDateTime, loanAppGlobals.DateTimeFormat);

                    HyperLink lnkLinks = (HyperLink)e.Item.FindControl("lnkLinks");

                    //if (!string.IsNullOrEmpty(objCustomerMasterDAL.PhotoIdImageName))
                    //{
                    //    imgPhotoIdImageName.ImageUrl = objCustomerMasterDAL.xsPhotoIdImageName;
                    //}
                    //else
                    //{
                    //    imgPhotoIdImageName.ImageUrl = "img\\xs_NoImage.png";
                    //}
                    if (objCustomerMasterDAL.IsRedFlag == true)
                    {
                        ltrlCustomerName.Text = "<span class='text-danger'>" + objCustomerMasterDAL.CustomerName + "</span>";
                        hlnkCustomerName.Text = "<span class='text-danger'>" + objCustomerMasterDAL.CustomerName + "</span>";
                    }
                    else
                    {
                        ltrlCustomerName.Text = objCustomerMasterDAL.CustomerName;
                        hlnkCustomerName.Text = objCustomerMasterDAL.CustomerName;
                    }
                    if (objCustomerMasterDAL.Links != "")
                    {
                        hlnkCustomerName.NavigateUrl = objCustomerMasterDAL.Links;
                        hlnkCustomerName.ToolTip = objCustomerMasterDAL.Links;
                        hlnkCustomerName.Visible = true;
                        ltrlCustomerName.Visible = false;
                    }
                    ltrlIdNo.Text = objCustomerMasterDAL.IdNo;
                    ltrlMobile1.Text = objCustomerMasterDAL.Mobile1;
                    ltrlMobile2.Text = objCustomerMasterDAL.Mobile2;
                    ltrlMobile3.Text = objCustomerMasterDAL.Mobile3;
                    switch (objCustomerMasterDAL.Gender)
                    {
                        case "Male":
                            ltrlGender.Text = Resources.Resource.Male;
                            break;
                        case "Female":
                            ltrlGender.Text = Resources.Resource.Female;
                            break;
                    }
                    ltrlOccupation.Text = objCustomerMasterDAL.Occupation;
                    ltrlPlaceOfWork.Text = objCustomerMasterDAL.PlaceOfWork;
                    ltrlCityOfResidence.Text = objCustomerMasterDAL.CityOfResidence;
                    ltrlAddress1.Text = objCustomerMasterDAL.Address1;
                    ltrlDistrict.Text = objCustomerMasterDAL.District;
                    ltrlPhone1.Text = objCustomerMasterDAL.Phone1;
                    switch (objCustomerMasterDAL.Relation1)
                    {
                        case "Father":
                            ltrlRelation1.Text = Resources.Resource.Father;
                            break;
                        case "Mother":
                            ltrlRelation1.Text = Resources.Resource.Mother;
                            break;
                        case "Brother":
                            ltrlRelation1.Text = Resources.Resource.Brother;
                            break;
                        case "Sister":
                            ltrlRelation1.Text = Resources.Resource.Sister;
                            break;
                        case "Other":
                            ltrlRelation1.Text = Resources.Resource.Other;
                            break;
                    }
                    ltrlContactName1.Text = objCustomerMasterDAL.ContactName1;
                    //ltrlAddress2.Text = objCustomerMasterDAL.Address2;
                    ltrlPhone2.Text = objCustomerMasterDAL.Phone2;
                    switch (objCustomerMasterDAL.Relation2)
                    {
                        case "Father":
                            ltrlRelation2.Text = Resources.Resource.Father;
                            break;
                        case "Mother":
                            ltrlRelation2.Text = Resources.Resource.Mother;
                            break;
                        case "Brother":
                            ltrlRelation2.Text = Resources.Resource.Brother;
                            break;
                        case "Sister":
                            ltrlRelation2.Text = Resources.Resource.Sister;
                            break;
                        case "Other":
                            ltrlRelation2.Text = Resources.Resource.Other;
                            break;
                    }
                    ltrlContactName2.Text = objCustomerMasterDAL.ContactName2;
                    if (!string.IsNullOrWhiteSpace(objCustomerMasterDAL.Links))
                    {
                        lnkLinks.Visible = true;
                        lnkLinks.NavigateUrl = objCustomerMasterDAL.Links;
                        lnkLinks.ToolTip = objCustomerMasterDAL.Links;
                    }
                    else
                    {
                        lnkLinks.Visible = false;
                    }
                    if (objCustomerMasterDAL.IsVerified != null)
                    {
                        LinkButton lbtnVerify = (LinkButton)e.Item.FindControl("lbtnVerify");
                        lbtnVerify.Visible = false;
                        Literal ltrlVerifiedBy = (Literal)e.Item.FindControl("ltrlVerifiedBy");
                        ltrlVerifiedBy.Text = objCustomerMasterDAL.VerifiedBy;
                        if (objCustomerMasterDAL.VerifiedDateTime != null)
                        {
                            Literal ltrlVerifiedDateTime = (Literal)e.Item.FindControl("ltrlVerifiedDateTime");
                            ltrlVerifiedDateTime.Text = loanGlobalsDAL.ConvertDateTimeToString(objCustomerMasterDAL.VerifiedDateTime.Value, loanAppGlobals.DateTimeFormat);
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

        protected void pgrCustomerMaster_ItemCommand(object sender, EventArgs e)
        {
            try
            {
                FillCustomerMaster();
            }
            catch (Exception ex)
            {
                loanAppGlobals.SaveError(ex);
            }
        }

        protected void lvCustomerMaster_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName.Equals("EditRecord", StringComparison.CurrentCultureIgnoreCase))
                {
                    GetCustomerMaster(Convert.ToInt32(((ListView)sender).DataKeys[e.Item.DataItemIndex].Value));
                }
                else if (e.CommandName.Equals("VerifyRecord", StringComparison.CurrentCultureIgnoreCase))
                {
                    string pageName = Path.GetFileNameWithoutExtension(Page.AppRelativeVirtualPath) + "master";
                    VerifyRecord(pageName, Convert.ToInt32(((ListView)sender).DataKeys[e.Item.DataItemIndex].Value));
                }
                else if (e.CommandName.Equals("DeleteRecord", StringComparison.CurrentCultureIgnoreCase))
                {
                    loanUser.CheckRoleRights(loanRoleRights.DeleteRecord);

                    loanCustomerMasterDAL objCustomerMasterDAL = new loanCustomerMasterDAL();
                    objCustomerMasterDAL.CustomerMasterId = Convert.ToInt32(((ListView)sender).DataKeys[e.Item.DataItemIndex].Value);
                    objCustomerMasterDAL.SessionId = ((loanUser)Session[loanSessionsDAL.UserSession]).SessionId;

                    loanRecordStatus rsStatus = objCustomerMasterDAL.DeleteCustomerMaster();
                    if (rsStatus == loanRecordStatus.Success)
                    {
                        loanAppGlobals.ShowMessage(loanMessagesDAL.DeleteSuccess, loanMessageIcon.Success);
                        FillCustomerMaster();
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
            if (loanSessionsDAL.GetSessionKeyValue("CurrentPageCustomer") != null)
            {
                pgrCustomerMaster.CurrentPage = Convert.ToInt16(loanSessionsDAL.GetSessionKeyValue("CurrentPageCustomer"));
            }
            if (loanSessionsDAL.GetSessionKeyValue("FilterCustomer") != null)
            {
                loanCustomerMasterDAL objCustomerMasterDAL = (loanCustomerMasterDAL)loanSessionsDAL.GetSessionKeyValue("FilterCustomer");
                txtFilterCustomer.Text = objCustomerMasterDAL.CustomerName;
                txtFilterPhoneMobile.Text = objCustomerMasterDAL.Phone1;

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

        private void FillCustomerMaster()
        {

            loanCustomerMasterDAL objCustomerMasterDAL = new loanCustomerMasterDAL();
            objCustomerMasterDAL.CustomerName = txtFilterCustomer.Text.Trim();
            objCustomerMasterDAL.Phone1 = txtFilterPhoneMobile.Text.Trim();
            objCustomerMasterDAL.linktoCompanyMasterId = ((loanUser)Session[loanSessionsDAL.UserSession]).CompanyMasterId;
            if (ddlFilterVerification.SelectedValue == "Yes")
            {
                objCustomerMasterDAL.IsVerified = true;
            }
            else if (ddlFilterVerification.SelectedValue == "No")
            {
                objCustomerMasterDAL.IsVerified = false;
            }
            if (ddlFilterHasurl.SelectedValue == "Yes")
            {
                objCustomerMasterDAL.Hasurl = true;
            }
            else if (ddlFilterHasurl.SelectedValue == "No")
            {
                objCustomerMasterDAL.Hasurl = false;
            }
            if (ddlFilterInvalidMobile.SelectedValue == "Yes")
            {
                objCustomerMasterDAL.InvalidMobile = true;
            }
            else if (ddlFilterInvalidMobile.SelectedValue == "No")
            {
                objCustomerMasterDAL.InvalidMobile = false;
            }
            if (ddlFilterInvalidIdNo.SelectedValue == "Yes")
            {
                objCustomerMasterDAL.InvalidIdNo = true;
            }
            else if (ddlFilterInvalidIdNo.SelectedValue == "No")
            {
                objCustomerMasterDAL.InvalidIdNo = false;
            }
            if (ddlFilterContractStatus.SelectedValue != string.Empty)
            {
                objCustomerMasterDAL.linktoContractStatusMasterId = Convert.ToInt32(ddlFilterContractStatus.SelectedValue);
            }
            loanSessionsDAL.SetSessionKeyValue("FilterCustomer", objCustomerMasterDAL);
            loanSessionsDAL.SetSessionKeyValue("CurrentPageCustomer", pgrCustomerMaster.CurrentPage);

            int TotalRecords;
            List<loanCustomerMasterDAL> lstCustomerMaster = objCustomerMasterDAL.SelectAllCustomerMasterPageWise(pgrCustomerMaster.StartRowIndex, pgrCustomerMaster.PageSize, out TotalRecords);
            pgrCustomerMaster.TotalRowCount = TotalRecords;

            if (lstCustomerMaster == null)
            {
                loanAppGlobals.ShowMessage(loanMessagesDAL.SelectAllFail, loanMessageIcon.Error);
                return;
            }

            if (lstCustomerMaster.Count == 0 && pgrCustomerMaster.TotalRowCount > 0)
            {
                pgrCustomerMaster_ItemCommand(pgrCustomerMaster, new EventArgs());
                return;
            }

            lvCustomerMaster.DataSource = lstCustomerMaster;
            lvCustomerMaster.DataBind();

            if (lstCustomerMaster.Count > 0)
            {
                int EndiIndex = pgrCustomerMaster.StartRowIndex + pgrCustomerMaster.PageSize < pgrCustomerMaster.TotalRowCount ? pgrCustomerMaster.StartRowIndex + pgrCustomerMaster.PageSize : pgrCustomerMaster.TotalRowCount;
                lblRecords.Text = string.Format(Resources.Messages.ResourceManager.GetString("Records"), pgrCustomerMaster.StartRowIndex + 1, EndiIndex, pgrCustomerMaster.TotalRowCount);
                lblRecords.Visible = true;
            }
            else
            {
                lblRecords.Visible = false;
            }

            if (pgrCustomerMaster.TotalRowCount <= pgrCustomerMaster.PageSize)
            {
                pgrCustomerMaster.Visible = false;
            }
            else
            {
                pgrCustomerMaster.Visible = true;
            }

        }

        private void GetCustomerMaster(int CustomerMasterId)
        {
            loanUser.CheckRoleRights(loanRoleRights.ViewRecord);

            loanCustomerMasterDAL objCustomerMasterDAL = new loanCustomerMasterDAL();
            objCustomerMasterDAL.CustomerMasterId = CustomerMasterId;
            if (!objCustomerMasterDAL.SelectCustomerMaster())
            {
                loanAppGlobals.ShowMessage(loanMessagesDAL.SelectFail, loanMessageIcon.Error);
                return;
            }
            hdnCustomerMasterId.Value = objCustomerMasterDAL.CustomerMasterId.ToString();
            txtCustomerName.Text = objCustomerMasterDAL.CustomerName;
            txtIdNo.Text = objCustomerMasterDAL.IdNo;
            txtLinks.Text = objCustomerMasterDAL.Links;
            rbGender.SelectedValue = objCustomerMasterDAL.Gender;

            //hdnPhotoIdImageName.Value = objCustomerMasterDAL.PhotoIdImageName;
            //hdnPhotoIdImageNameURL.Value = objCustomerMasterDAL.xsPhotoIdImageName;

            txtOccupation.Text = objCustomerMasterDAL.Occupation;
            txtPlaceOfWork.Text = objCustomerMasterDAL.PlaceOfWork;
            txtCityOfResidence.Text = objCustomerMasterDAL.CityOfResidence;
            txtDistrict.Text = objCustomerMasterDAL.District;
            txtMobile1.Text = objCustomerMasterDAL.Mobile1;
            txtMobile2.Text = objCustomerMasterDAL.Mobile2;
            txtMobile3.Text = objCustomerMasterDAL.Mobile3;

            txtAddress1.Text = objCustomerMasterDAL.Address1;
            txtPhone1.Text = objCustomerMasterDAL.Phone1;
            ddlRelation1.SelectedValue = objCustomerMasterDAL.Relation1;
            txtContactName1.Text = objCustomerMasterDAL.ContactName1;
            //txtAddress2.Text = objCustomerMasterDAL.Address2;
            txtPhone2.Text = objCustomerMasterDAL.Phone2;
            ddlRelation2.SelectedValue = objCustomerMasterDAL.Relation2;
            txtContactName2.Text = objCustomerMasterDAL.ContactName2;
            txtBankAccountNumber.Text = objCustomerMasterDAL.BankAccountNumber;
            txtBankAccountNumber2.Text = objCustomerMasterDAL.BankAccountNumber2;
            txtBankAccountNumber3.Text = objCustomerMasterDAL.BankAccountNumber3;
            txtBankAccountNumber4.Text = objCustomerMasterDAL.BankAccountNumber4;

            chkIsIsRedFlag.Checked = objCustomerMasterDAL.IsRedFlag == null ? false : objCustomerMasterDAL.IsRedFlag.Value;

            hdnModelCustomer.Value = "show";
            hdnActionCustomer.Value = "edit";
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
                FillCustomerMaster();
            }
        }
        #endregion


    }
}
