using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using loanLibrary;
using System.IO;
using System.Web;

namespace abLOAN
{
    public partial class contractdetails : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                loanSessionsDAL.CheckSession();

                if (!Page.IsPostBack)
                {
                    GetContractStatus();
                    GetItem();
                    GetBank();
                    GetPageDefaults();
                    if (Request.QueryString.ToString().Contains("id="))
                    {
                        int id = Convert.ToInt32(Request.QueryString["id"].ToString());
                        hdnContractMasterId.Value = id.ToString();
                    }
                    if (string.IsNullOrEmpty(hdnContractMasterId.Value))
                    {
                        //loanUser.CheckUserRights(loanUserRights.AddContract);
                        loanUser.CheckRoleRights(loanRoleRights.ViewList);

                        FillGuarantorMaster(0);
                        FillWitnessMaster(0);

                        btnNewGuarantor.Enabled = false;
                        btnNewWitness.Enabled = false;
                    }
                    else
                    {
                        //loanUser.CheckUserRights(loanUserRights.EditContract);

                        GetContractMaster(Convert.ToInt32(hdnContractMasterId.Value));
                        FillGuarantorMaster(Convert.ToInt32(hdnContractMasterId.Value));
                        FillWitnessMaster(Convert.ToInt32(hdnContractMasterId.Value));
                    }
                }
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
                if (string.IsNullOrEmpty(hdnCustomerMasterId.Value))
                {
                    loanAppGlobals.ShowMessage(null, loanMessageIcon.Warning, Resources.Messages.InputRequired + Resources.Resource.CustomerFormTitle);
                    hdnCollapseHide.Value = "collapseTwo";
                    hdnCollapseShow.Value = "collapseOne";
                    return;
                }

                loanContractMasterDAL objContractMasterDAL = new loanContractMasterDAL();
                objContractMasterDAL.linktoCompanyMasterId = ((loanUser)Session[loanSessionsDAL.UserSession]).CompanyMasterId;
                objContractMasterDAL.linktoCustomerMasterId = Convert.ToInt32(hdnCustomerMasterId.Value);
                objContractMasterDAL.ContractTitle = txtContractTitle.Text.Trim();
                if (!string.IsNullOrEmpty(ddlBank.Text))
                {
                    objContractMasterDAL.linktoBankMasterId = Convert.ToInt32(ddlBank.SelectedValue);
                }
                objContractMasterDAL.FileNo = Convert.ToInt32(txtFileNo.Text);
                objContractMasterDAL.Links = txtLinks.Text.Trim();
                objContractMasterDAL.ContractDate = DateTime.ParseExact(txtContractDate.Text, loanAppGlobals.DateFormat, System.Globalization.DateTimeFormatInfo.InvariantInfo);
                objContractMasterDAL.ContractStartDate = DateTime.ParseExact(txtContractStartDate.Text, loanAppGlobals.DateFormat, System.Globalization.DateTimeFormatInfo.InvariantInfo);
                objContractMasterDAL.InstallmentDate = DateTime.ParseExact(txtInstallmentDate.Text, loanAppGlobals.DateFormat, System.Globalization.DateTimeFormatInfo.InvariantInfo);
                objContractMasterDAL.linktoContractStatusMasterId = loanContractStatus.Active.GetHashCode();
                objContractMasterDAL.ContractAmount = Convert.ToDecimal(txtContractAmount.Text);
                objContractMasterDAL.IsBasedOnMonth = chkIsBasedOnMonth.Checked;
                objContractMasterDAL.NoOfInstallments = Convert.ToInt32(txtNoOfInstallments.Text);
                objContractMasterDAL.InstallmentAmount = Convert.ToDecimal(txtInstallmentAmount.Text);
                //objContractMasterDAL.InterestRate = Convert.ToDecimal(txtInterestRate.Text);
                objContractMasterDAL.InterestRate = 0;
                objContractMasterDAL.DownPayment = Convert.ToDecimal(txtDownPayment.Text);
                objContractMasterDAL.Notes = txtNotes.Text.Trim();
                if (!string.IsNullOrEmpty(txtSettlementAmount.Text))
                {
                    objContractMasterDAL.SettlementAmount = Convert.ToDecimal(txtSettlementAmount.Text);
                }
                if (!string.IsNullOrEmpty(ddlContractStatus.Text))
                {
                    objContractMasterDAL.linktoContractStatusMasterId = Convert.ToInt32(ddlContractStatus.SelectedValue);
                }

                objContractMasterDAL.UpdateDateTime = loanGlobalsDAL.GetCurrentDateTime();
                objContractMasterDAL.SessionId = ((loanUser)Session[loanSessionsDAL.UserSession]).SessionId;

                objContractMasterDAL.CreateDateTime = loanGlobalsDAL.GetCurrentDateTime();
                objContractMasterDAL.linktoUserMasterIdCreatedBy = ((loanUser)Session[loanSessionsDAL.UserSession]).UserMasterId;

                List<loanContractItemTranDAL> lstContractItemTranDAL = new List<loanContractItemTranDAL>();
                loanContractItemTranDAL objContractItemTranDAL = new loanContractItemTranDAL();
                objContractItemTranDAL.linktoItemMasterId = Convert.ToInt32(ddlItem.SelectedValue);
                lstContractItemTranDAL.Add(objContractItemTranDAL);

                if (string.IsNullOrEmpty(hdnContractMasterId.Value))
                {
                    loanUser.CheckRoleRights(loanRoleRights.AddRecord);

                    loanRecordStatus rsStatus = objContractMasterDAL.InsertContractMaster(lstContractItemTranDAL);
                    if (rsStatus == loanRecordStatus.Error)
                    {
                        loanAppGlobals.ShowMessage(loanMessagesDAL.InsertFail, loanMessageIcon.Error);
                        return;
                    }
                    else if (rsStatus == loanRecordStatus.RecordAlreadyExist)
                    {
                        loanAppGlobals.ShowMessage(loanMessagesDAL.AlreadyExist, loanMessageIcon.Warning);
                        return;
                    }
                    else if (rsStatus == loanRecordStatus.Success)
                    {
                        loanAppGlobals.ShowMessage(loanMessagesDAL.InsertSuccess, loanMessageIcon.Success);
                        hdnCollapseHide.Value = "collapseTwo";
                        hdnCollapseShow.Value = "collapseThree";
                        hdnContractMasterId.Value = objContractMasterDAL.ContractMasterId.ToString();
                        btnNewGuarantor.Enabled = true;
                        btnNewWitness.Enabled = true;
                    }
                }
                else
                {
                    loanUser.CheckRoleRights(loanRoleRights.EditRecord);

                    objContractMasterDAL.ContractMasterId = Convert.ToInt32(hdnContractMasterId.Value);
                    loanRecordStatus rsStatus = objContractMasterDAL.UpdateContractMaster(lstContractItemTranDAL);
                    if (rsStatus == loanRecordStatus.Error)
                    {
                        loanAppGlobals.ShowMessage(loanMessagesDAL.UpdateFail, loanMessageIcon.Error);
                        return;
                    }
                    else if (rsStatus == loanRecordStatus.RecordAlreadyExist)
                    {
                        loanAppGlobals.ShowMessage(loanMessagesDAL.AlreadyExist, loanMessageIcon.Warning);
                        return;
                    }
                    else if (rsStatus == loanRecordStatus.Success)
                    {
                        loanAppGlobals.ShowMessage(loanMessagesDAL.UpdateSuccess, loanMessageIcon.Success);
                        hdnCollapseHide.Value = "collapseTwo";
                        hdnCollapseShow.Value = "collapseThree";
                    }
                }
            }
            catch (Exception ex)
            {
                loanAppGlobals.SaveError(ex);
            }
        }

        protected void btnCustomerSave_Click(object sender, EventArgs e)
        {
            try
            {
                loanUser.CheckRoleRights(loanRoleRights.AddRecord);


                if (!string.IsNullOrEmpty(hdnCustomerMasterId.Value))
                {
                    loanAppGlobals.ShowMessage(loanMessagesDAL.InsertSuccess, loanMessageIcon.Success);
                    hdnCollapseHide.Value = "collapseOne";
                    hdnCollapseShow.Value = "collapseTwo";
                    return;
                }

                loanCustomerMasterDAL objCustomerMasterDAL = new loanCustomerMasterDAL();
                objCustomerMasterDAL.linktoCompanyMasterId = ((loanUser)Session[loanSessionsDAL.UserSession]).CompanyMasterId;
                objCustomerMasterDAL.CustomerName = txtCustomerName.Text.Trim();
                objCustomerMasterDAL.IdNo = txtIdNo.Text.Trim();
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
                objCustomerMasterDAL.Mobile1 = txtMobile1.Text.Trim();
                objCustomerMasterDAL.Mobile2 = txtMobile2.Text.Trim();
                objCustomerMasterDAL.Mobile3 = txtMobile3.Text.Trim();
                objCustomerMasterDAL.Address1 = txtAddress1.Text.Trim();
                objCustomerMasterDAL.Phone1 = txtPhone1.Text.Trim();
                objCustomerMasterDAL.ContactName1 = txtContactName1.Text.Trim();
                objCustomerMasterDAL.Address2 = txtAddress2.Text.Trim();
                objCustomerMasterDAL.Phone2 = txtPhone2.Text.Trim();
                objCustomerMasterDAL.ContactName2 = txtContactName2.Text.Trim();
                objCustomerMasterDAL.UpdateDateTime = loanGlobalsDAL.GetCurrentDateTime();
                objCustomerMasterDAL.SessionId = ((loanUser)Session[loanSessionsDAL.UserSession]).SessionId;

                loanRecordStatus rsStatus = objCustomerMasterDAL.InsertCustomerMaster();
                if (rsStatus == loanRecordStatus.Error)
                {
                    loanAppGlobals.ShowMessage(loanMessagesDAL.InsertFail, loanMessageIcon.Error);
                    return;
                }
                else if (rsStatus == loanRecordStatus.RecordAlreadyExist)
                {
                    loanAppGlobals.ShowMessage(loanMessagesDAL.AlreadyExist, loanMessageIcon.Warning);
                    return;
                }
                else if (rsStatus == loanRecordStatus.Success)
                {
                    loanAppGlobals.ShowMessage(loanMessagesDAL.InsertSuccess, loanMessageIcon.Success);
                    hdnCollapseHide.Value = "collapseOne";
                    hdnCollapseShow.Value = "collapseTwo";
                    hdnCustomerMasterId.Value = objCustomerMasterDAL.CustomerMasterId.ToString();
                }
            }
            catch (Exception ex)
            {
                loanAppGlobals.SaveError(ex);
            }
        }

        protected void btnGuarantorSave_Click(object sender, EventArgs e)
        {
            try
            {
                loanUser.CheckRoleRights(loanRoleRights.AddRecord);


                if (string.IsNullOrEmpty(hdnContractMasterId.Value))
                {
                    loanAppGlobals.ShowMessage(null, loanMessageIcon.Warning, Resources.Messages.InputRequired + Resources.Resource.ContractFormTitle);
                    return;
                }

                loanGuarantorMasterDAL objGuarantorMasterDAL = new loanGuarantorMasterDAL();
                if (!string.IsNullOrEmpty(hdnGuarantorMasterId.Value))
                {
                    objGuarantorMasterDAL.GuarantorMasterId = Convert.ToInt32(hdnGuarantorMasterId.Value);
                }
                objGuarantorMasterDAL.linktoCompanyMasterId = ((loanUser)Session[loanSessionsDAL.UserSession]).CompanyMasterId;
                objGuarantorMasterDAL.GuarantorName = txtGuarantorName.Text.Trim();
                objGuarantorMasterDAL.IdNo = txtGuarantorIdNo.Text.Trim();
                if (fuGuarantorPhotoIdImageName.HasFile)
                {
                    objGuarantorMasterDAL.PhotoIdImageName = objGuarantorMasterDAL.GuarantorMasterId + System.IO.Path.GetExtension(fuGuarantorPhotoIdImageName.FileName);
                    string ImageSavePath = System.Configuration.ConfigurationManager.AppSettings["ImageSavePath"] + "Guarantor\\";
                    if (!Directory.Exists(ImageSavePath))
                    {
                        Directory.CreateDirectory(ImageSavePath);
                    }
                    fuGuarantorPhotoIdImageName.SaveAs(ImageSavePath + objGuarantorMasterDAL.PhotoIdImageName);

                    loanGlobalsDAL.CreateThumbImages(objGuarantorMasterDAL.PhotoIdImageName, ImageSavePath);

                }
                else if (hdnGuarantorPhotoIdImageName.Value != string.Empty)
                {
                    objGuarantorMasterDAL.PhotoIdImageName = hdnGuarantorPhotoIdImageName.Value;
                }
                objGuarantorMasterDAL.Address1 = txtGuarantorAddress1.Text.Trim();
                objGuarantorMasterDAL.Phone1 = txtGuarantorPhone1.Text.Trim();
                objGuarantorMasterDAL.Mobile1 = txtGuarantorMobile1.Text.Trim();
                objGuarantorMasterDAL.Fax1 = txtGuarantorFax1.Text.Trim();
                objGuarantorMasterDAL.Address2 = txtGuarantorAddress2.Text.Trim();
                objGuarantorMasterDAL.Phone2 = txtGuarantorPhone2.Text.Trim();
                objGuarantorMasterDAL.Mobile2 = txtGuarantorMobile2.Text.Trim();
                objGuarantorMasterDAL.Fax2 = txtGuarantorFax2.Text.Trim();
                objGuarantorMasterDAL.Mobile3 = txtGuarantorMobile3.Text.Trim();
                //objGuarantorMasterDAL.City = txtGuarantorCity.Text.Trim();

                objGuarantorMasterDAL.UpdateDateTime = loanGlobalsDAL.GetCurrentDateTime();
                objGuarantorMasterDAL.SessionId = ((loanUser)Session[loanSessionsDAL.UserSession]).SessionId;

                if (string.IsNullOrEmpty(hdnActionGuarantor.Value))
                {
                    loanRecordStatus rsStatus = objGuarantorMasterDAL.InsertGuarantorMaster(Convert.ToInt32(hdnContractMasterId.Value));
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
                        if (((Button)sender).ID.Equals("btnGuarantorSaveAndNew"))
                        {
                            hdnModelGuarantor.Value = "clear";
                        }
                        else
                        {
                            hdnModelGuarantor.Value = "hide";
                            hdnCollapseHide.Value = "collapseThree";
                            hdnCollapseShow.Value = "collapseFour";
                        }
                        FillGuarantorMaster(Convert.ToInt32(hdnContractMasterId.Value));
                    }
                }
                //else
                //{
                //    objGuarantorMasterDAL.GuarantorMasterId = Convert.ToInt32(hdnGuarantorMasterId.Value);
                //    loanRecordStatus rsStatus = objGuarantorMasterDAL.UpdateGuarantorMaster();
                //    if (rsStatus == loanRecordStatus.Error)
                //    {
                //        loanAppGlobals.ShowMessage(loanMessagesDAL.UpdateFail, loanMessageIcon.Error);
                //        return;
                //    }
                //    else if (rsStatus == loanRecordStatus.RecordAlreadyExist)
                //    {
                //        loanAppGlobals.ShowMessage(loanMessagesDAL.AlreadyExist, loanMessageIcon.Warning);
                //        hdnModelGuarantor.Value = "show";
                //        return;
                //    }
                //    else if (rsStatus == loanRecordStatus.Success)
                //    {
                //        loanAppGlobals.ShowMessage(loanMessagesDAL.UpdateSuccess, loanMessageIcon.Success);
                //        hdnModelGuarantor.Value = "hide";
                //        FillGuarantorMaster(Convert.ToInt32(hdnContractMasterId.Value));
                //    }
                //}
            }
            catch (Exception ex)
            {
                loanAppGlobals.SaveError(ex);
            }
        }

        protected void btnWitnessSave_Click(object sender, EventArgs e)
        {
            try
            {
                loanUser.CheckRoleRights(loanRoleRights.AddRecord);

                if (string.IsNullOrEmpty(hdnContractMasterId.Value))
                {
                    loanAppGlobals.ShowMessage(null, loanMessageIcon.Warning, Resources.Messages.InputRequired + Resources.Resource.ContractFormTitle);
                    return;
                }

                loanWitnessMasterDAL objWitnessMasterDAL = new loanWitnessMasterDAL();
                if (!string.IsNullOrEmpty(hdnWitnessMasterId.Value))
                {
                    objWitnessMasterDAL.WitnessMasterId = Convert.ToInt32(hdnWitnessMasterId.Value);
                }
                objWitnessMasterDAL.linktoCompanyMasterId = ((loanUser)Session[loanSessionsDAL.UserSession]).CompanyMasterId;
                objWitnessMasterDAL.WitnessName = txtWitnessName.Text.Trim();
                objWitnessMasterDAL.IdNo = txtWitnessIdNo.Text.Trim();
                if (fuWitnessPhotoIdImageName.HasFile)
                {
                    objWitnessMasterDAL.PhotoIdImageName = objWitnessMasterDAL.WitnessMasterId + System.IO.Path.GetExtension(fuWitnessPhotoIdImageName.FileName);
                    string ImageSavePath = System.Configuration.ConfigurationManager.AppSettings["ImageSavePath"] + "Witness\\";
                    if (!Directory.Exists(ImageSavePath))
                    {
                        Directory.CreateDirectory(ImageSavePath);
                    }
                    fuWitnessPhotoIdImageName.SaveAs(ImageSavePath + objWitnessMasterDAL.PhotoIdImageName);

                    loanGlobalsDAL.CreateThumbImages(objWitnessMasterDAL.PhotoIdImageName, ImageSavePath);

                }
                else if (hdnWitnessPhotoIdImageName.Value != string.Empty)
                {
                    objWitnessMasterDAL.PhotoIdImageName = hdnWitnessPhotoIdImageName.Value;
                }
                objWitnessMasterDAL.Address1 = txtWitnessAddress1.Text.Trim();
                objWitnessMasterDAL.Phone1 = txtWitnessPhone1.Text.Trim();
                objWitnessMasterDAL.Mobile1 = txtWitnessMobile1.Text.Trim();
                objWitnessMasterDAL.Fax1 = txtWitnessFax1.Text.Trim();
                objWitnessMasterDAL.Address2 = txtWitnessAddress2.Text.Trim();
                objWitnessMasterDAL.Phone2 = txtWitnessPhone2.Text.Trim();
                objWitnessMasterDAL.Mobile2 = txtWitnessMobile2.Text.Trim();
                objWitnessMasterDAL.Fax2 = txtWitnessFax2.Text.Trim();
                objWitnessMasterDAL.Mobile3 = txtWitnessMobile3.Text.Trim();
                //objWitnessMasterDAL.City = txtWitnessCity.Text.Trim();

                objWitnessMasterDAL.UpdateDateTime = loanGlobalsDAL.GetCurrentDateTime();
                objWitnessMasterDAL.SessionId = ((loanUser)Session[loanSessionsDAL.UserSession]).SessionId;

                if (string.IsNullOrEmpty(hdnActionWitness.Value))
                {
                    loanRecordStatus rsStatus = objWitnessMasterDAL.InsertWitnessMaster(Convert.ToInt32(hdnContractMasterId.Value));
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
                        if (((Button)sender).ID.Equals("btnWitnessSaveAndNew"))
                        {
                            hdnModelWitness.Value = "clear";
                        }
                        else
                        {
                            hdnModelWitness.Value = "hide";
                        }
                        FillWitnessMaster(Convert.ToInt32(hdnContractMasterId.Value));
                    }
                }
                //else
                //{
                //    objWitnessMasterDAL.WitnessMasterId = Convert.ToInt32(hdnWitnessMasterId.Value);
                //    loanRecordStatus rsStatus = objWitnessMasterDAL.UpdateWitnessMaster();
                //    if (rsStatus == loanRecordStatus.Error)
                //    {
                //        loanAppGlobals.ShowMessage(loanMessagesDAL.UpdateFail, loanMessageIcon.Error);
                //        return;
                //    }
                //    else if (rsStatus == loanRecordStatus.RecordAlreadyExist)
                //    {
                //        loanAppGlobals.ShowMessage(loanMessagesDAL.AlreadyExist, loanMessageIcon.Warning);
                //        hdnModelWitness.Value = "show";
                //        return;
                //    }
                //    else if (rsStatus == loanRecordStatus.Success)
                //    {
                //        loanAppGlobals.ShowMessage(loanMessagesDAL.UpdateSuccess, loanMessageIcon.Success);
                //        hdnModelWitness.Value = "hide";
                //        FillWitnessMaster(Convert.ToInt32(hdnContractMasterId.Value));
                //    }
                //}
            }
            catch (Exception ex)
            {
                loanAppGlobals.SaveError(ex);
            }
        }

        protected void btnSearchCustomerIdNo_Click(object sender, EventArgs e)
        {
            try
            {
                hdnCollapseHide.Value = "";
                hdnCollapseShow.Value = "collapseOne";
                string IdNo = txtSearchCustomerIdNo.Text.Trim();
                if (IdNo.Contains("("))
                {
                    IdNo = IdNo.Substring(IdNo.IndexOf("(") + 1, IdNo.IndexOf(")") - IdNo.IndexOf("(") - 1);
                }
                GetCustomerMaster(IdNo, 0);
            }
            catch (Exception ex)
            {
                loanAppGlobals.SaveError(ex);
            }
        }

        protected void btnSearchGuarantorIdNo_Click(object sender, EventArgs e)
        {
            try
            {
                hdnCollapseHide.Value = "";
                hdnCollapseShow.Value = "collapseThree";
                string IdNo = txtSearchGuarantorIdNo.Text.Trim();
                if (IdNo.Contains("("))
                {
                    IdNo = IdNo.Substring(IdNo.IndexOf("(") + 1, IdNo.IndexOf(")") - IdNo.IndexOf("(") - 1);
                }
                GetGuarantorMaster(IdNo, 0);
            }
            catch (Exception ex)
            {
                loanAppGlobals.SaveError(ex);
            }
        }

        protected void btnGuarantorContinue_Click(object sender, EventArgs e)
        {
            try
            {
                hdnCollapseHide.Value = "collapseThree";
                hdnCollapseShow.Value = "collapseFour";
            }
            catch (Exception ex)
            {
                loanAppGlobals.SaveError(ex);
            }
        }

        protected void btnSearchWitnessIdNo_Click(object sender, EventArgs e)
        {
            try
            {
                hdnCollapseHide.Value = "";
                hdnCollapseShow.Value = "collapseFour";
                string IdNo = txtSearchWitnessIdNo.Text.Trim();
                if (IdNo.Contains("("))
                {
                    IdNo = IdNo.Substring(IdNo.IndexOf("(") + 1, IdNo.IndexOf(")") - IdNo.IndexOf("(") - 1);
                }
                GetWitnessMaster(IdNo, 0);
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
                    Literal ltrlMobile3 = (Literal)e.Item.FindControl("ltrlMobile3");
                    Literal ltrlFax2 = (Literal)e.Item.FindControl("ltrlFax2");
                    //Literal ltrlCity = (Literal)e.Item.FindControl("ltrlCity");

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
                    ltrlMobile3.Text = objGuarantorMasterDAL.Mobile3;
                    ltrlFax2.Text = objGuarantorMasterDAL.Fax2;
                    //ltrlCity.Text = objGuarantorMasterDAL.City;


                }
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
                loanUser.CheckRoleRights(loanRoleRights.DeleteRecord);

                if (e.CommandName.Equals("DeleteRecord", StringComparison.CurrentCultureIgnoreCase))
                {
                    loanContractGuarantorTranDAL objContractGuarantorTranDAL = new loanContractGuarantorTranDAL();
                    objContractGuarantorTranDAL.linktoContractMasterId = Convert.ToInt32(hdnContractMasterId.Value);
                    objContractGuarantorTranDAL.linktoGuarantorMasterId = Convert.ToInt32(((ListView)sender).DataKeys[e.Item.DataItemIndex].Value);
                    loanRecordStatus rsStatus = objContractGuarantorTranDAL.DeleteContractGuarantorTran();
                    if (rsStatus == loanRecordStatus.Success)
                    {
                        loanAppGlobals.ShowMessage(loanMessagesDAL.DeleteSuccess, loanMessageIcon.Success);
                        FillGuarantorMaster(Convert.ToInt32(hdnContractMasterId.Value));
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
                    Literal ltrlMobile3 = (Literal)e.Item.FindControl("ltrlMobile3");
                    Literal ltrlFax2 = (Literal)e.Item.FindControl("ltrlFax2");
                    //Literal ltrlCity = (Literal)e.Item.FindControl("ltrlCity");

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
                    ltrlMobile3.Text = objWitnessMasterDAL.Mobile3;
                    ltrlFax2.Text = objWitnessMasterDAL.Fax2;
                    //ltrlCity.Text = objWitnessMasterDAL.City;

                }
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
                loanUser.CheckRoleRights(loanRoleRights.DeleteRecord);

                if (e.CommandName.Equals("DeleteRecord", StringComparison.CurrentCultureIgnoreCase))
                {
                    loanContractWitnessTranDAL objContractWitnessTranDAL = new loanContractWitnessTranDAL();
                    objContractWitnessTranDAL.linktoContractMasterId = Convert.ToInt32(hdnContractMasterId.Value);
                    objContractWitnessTranDAL.linktoWitnessMasterId = Convert.ToInt32(((ListView)sender).DataKeys[e.Item.DataItemIndex].Value);
                    loanRecordStatus rsStatus = objContractWitnessTranDAL.DeleteContractWitnessTran();
                    if (rsStatus == loanRecordStatus.Success)
                    {
                        loanAppGlobals.ShowMessage(loanMessagesDAL.DeleteSuccess, loanMessageIcon.Success);
                        FillWitnessMaster(Convert.ToInt32(hdnContractMasterId.Value));
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
            hdnContractMasterId.Value = Convert.ToString(loanSessionsDAL.GetSessionKeyValue("ContractMasterId"));
        }

        private void GetItem()
        {
            ddlItem.Items.Clear();
            ddlItem.Items.Add(new System.Web.UI.WebControls.ListItem(loanDropDownItem.Select, ""));

            List<loanItemMasterDAL> lstItemMasterDAL = loanItemMasterDAL.SelectAllItemMasterItemName();
            if (lstItemMasterDAL == null)
            {
                loanAppGlobals.ShowMessage(loanMessagesDAL.SelectAllFail, loanMessageIcon.Error);
                return;
            }
            foreach (loanItemMasterDAL obj in lstItemMasterDAL)
            {
                ddlItem.Items.Add(new System.Web.UI.WebControls.ListItem(obj.ItemName, obj.ItemMasterId.ToString()));
            }
        }

        private void GetContractStatus()
        {
            ddlContractStatus.Items.Clear();
            ddlContractStatus.Items.Add(new System.Web.UI.WebControls.ListItem(loanDropDownItem.Select, ""));

            List<loanContractStatusMasterDAL> lstContractStatusMasterDAL = loanContractStatusMasterDAL.SelectAllContractStatusMasterContractStatus();
            if (lstContractStatusMasterDAL == null)
            {
                loanAppGlobals.ShowMessage(loanMessagesDAL.SelectAllFail, loanMessageIcon.Error);
                return;
            }
            foreach (loanContractStatusMasterDAL obj in lstContractStatusMasterDAL)
            {
                ddlContractStatus.Items.Add(new System.Web.UI.WebControls.ListItem(obj.ContractStatus, obj.ContractStatusMasterId.ToString()));
            }
        }

        private void ClearCustomerMaster()
        {
            hdnCustomerMasterId.Value = string.Empty;
            txtCustomerName.Text = string.Empty;
            txtIdNo.Text = string.Empty;
            //hdnPhotoIdImageName.Value = string.Empty;
            //hdnPhotoIdImageNameURL.Value = string.Empty;
            txtMobile1.Text = string.Empty;
            txtMobile2.Text = string.Empty;
            txtMobile3.Text = string.Empty;
            txtAddress1.Text = string.Empty;
            txtPhone1.Text = string.Empty;
            txtContactName1.Text = string.Empty;
            txtAddress2.Text = string.Empty;
            txtPhone2.Text = string.Empty;
            txtContactName2.Text = string.Empty;
        }

        private void GetContractMaster(int ContractMasterId)
        {
            loanUser.CheckRoleRights(loanRoleRights.ViewRecord);

            loanContractMasterDAL objContractMasterDAL = new loanContractMasterDAL();
            objContractMasterDAL.ContractMasterId = ContractMasterId;
            if (!objContractMasterDAL.SelectContractMaster())
            {
                loanAppGlobals.ShowMessage(loanMessagesDAL.SelectFail, loanMessageIcon.Error);
                Response.Redirect("contract.aspx");
            }
            GetCustomerMaster(null, objContractMasterDAL.linktoCustomerMasterId);

            loanContractItemTranDAL objContractItemTranDAL = new loanContractItemTranDAL();
            objContractItemTranDAL.linktoContractMasterId = objContractMasterDAL.ContractMasterId;
            List<loanContractItemTranDAL> lstContractItemTranDAL = objContractItemTranDAL.SelectAllContractItemTran();
            if (lstContractItemTranDAL == null)
            {
                loanAppGlobals.ShowMessage(loanMessagesDAL.SelectAllFail, loanMessageIcon.Error);
                Response.Redirect("contract.aspx");
            }

            hdnContractMasterId.Value = objContractMasterDAL.ContractMasterId.ToString();
            txtContractTitle.Text = objContractMasterDAL.ContractTitle;
            txtLinks.Text = objContractMasterDAL.Links;
            ddlBank.SelectedIndex = ddlBank.Items.IndexOf(ddlBank.Items.FindByValue(objContractMasterDAL.linktoBankMasterId.ToString()));
            txtFileNo.Text = objContractMasterDAL.FileNo.ToString();
            txtContractDate.Text = loanGlobalsDAL.ConvertDateTimeToString(objContractMasterDAL.ContractDate, loanAppGlobals.DateTimeFormat);
            txtContractStartDate.Text = loanGlobalsDAL.ConvertDateTimeToString(objContractMasterDAL.ContractStartDate, loanAppGlobals.DateTimeFormat);
            txtInstallmentDate.Text = loanGlobalsDAL.ConvertDateTimeToString(objContractMasterDAL.InstallmentDate, loanAppGlobals.DateTimeFormat);
            txtContractAmount.Text = objContractMasterDAL.ContractAmount.ToString("0.00");
            chkIsBasedOnMonth.Checked = objContractMasterDAL.IsBasedOnMonth;
            txtNoOfInstallments.Text = objContractMasterDAL.NoOfInstallments.ToString();
            txtInstallmentAmount.Text = objContractMasterDAL.InstallmentAmount.ToString("0.00");
            //txtInterestRate.Text = objContractMasterDAL.InterestRate.ToString("0.00");
            txtDownPayment.Text = objContractMasterDAL.DownPayment.ToString("0.00");
            txtNotes.Text = objContractMasterDAL.Notes;
            ddlItem.SelectedValue = lstContractItemTranDAL[0].linktoItemMasterId.ToString();
            ddlContractStatus.SelectedIndex = ddlContractStatus.Items.IndexOf(ddlContractStatus.Items.FindByValue(objContractMasterDAL.linktoContractStatusMasterId.ToString()));
            txtSettlementAmount.Text = objContractMasterDAL.SettlementAmount.ToString("0.00");

        }

        private void GetBank()
        {
            ddlBank.Items.Clear();
            ddlBank.Items.Add(new System.Web.UI.WebControls.ListItem(loanDropDownItem.Select, ""));

            List<loanBankMasterDAL> lstBankMasterDAL = loanBankMasterDAL.SelectAllBankMasterBankName();
            if (lstBankMasterDAL == null)
            {
                loanAppGlobals.ShowMessage(loanMessagesDAL.SelectAllFail, loanMessageIcon.Error);
                return;
            }
            foreach (loanBankMasterDAL obj in lstBankMasterDAL)
            {
                ddlBank.Items.Add(new System.Web.UI.WebControls.ListItem(obj.BankName, obj.BankMasterId.ToString()));
            }
        }

        private void GetCustomerMaster(string IdNo, int customerMasterId = 0)
        {
            loanCustomerMasterDAL objCustomerMasterDAL = new loanCustomerMasterDAL();
            objCustomerMasterDAL.CustomerMasterId = customerMasterId;
            objCustomerMasterDAL.IdNo = IdNo;
            if (!objCustomerMasterDAL.SelectCustomerMaster())
            {
                ClearCustomerMaster();
                pnlCustomer.Enabled = true;

                loanAppGlobals.ShowMessage(loanMessagesDAL.NotFound, loanMessageIcon.Warning);
                return;
            }
            hdnCustomerMasterId.Value = objCustomerMasterDAL.CustomerMasterId.ToString();
            txtCustomerName.Text = objCustomerMasterDAL.CustomerName;
            txtIdNo.Text = objCustomerMasterDAL.IdNo;
            //hdnPhotoIdImageName.Value = objCustomerMasterDAL.PhotoIdImageName;
            //hdnPhotoIdImageNameURL.Value = objCustomerMasterDAL.xsPhotoIdImageName;
            txtMobile1.Text = objCustomerMasterDAL.Mobile1;
            txtMobile2.Text = objCustomerMasterDAL.Mobile2;
            txtMobile3.Text = objCustomerMasterDAL.Mobile3;
            txtAddress1.Text = objCustomerMasterDAL.Address1;
            txtPhone1.Text = objCustomerMasterDAL.Phone1;
            txtContactName1.Text = objCustomerMasterDAL.ContactName1;
            txtAddress2.Text = objCustomerMasterDAL.Address2;
            txtPhone2.Text = objCustomerMasterDAL.Phone2;
            txtContactName2.Text = objCustomerMasterDAL.ContactName2;

            pnlCustomer.Enabled = false;
        }

        private void GetGuarantorMaster(string IdNo, int GuarantorMasterId = 0)
        {
            loanGuarantorMasterDAL objGuarantorMasterDAL = new loanGuarantorMasterDAL();
            objGuarantorMasterDAL.GuarantorMasterId = GuarantorMasterId;
            objGuarantorMasterDAL.IdNo = IdNo;
            if (!objGuarantorMasterDAL.SelectGuarantorMaster())
            {
                hdnGuarantorMasterId.Value = string.Empty;
                pnlGuarantor.Enabled = true;
                hdnModelGuarantor.Value = "clear";

                loanAppGlobals.ShowMessage(loanMessagesDAL.NotFound, loanMessageIcon.Warning);
                return;
            }
            hdnGuarantorMasterId.Value = objGuarantorMasterDAL.GuarantorMasterId.ToString();
            txtGuarantorName.Text = objGuarantorMasterDAL.GuarantorName;
            txtGuarantorIdNo.Text = objGuarantorMasterDAL.IdNo;
            hdnGuarantorPhotoIdImageName.Value = objGuarantorMasterDAL.PhotoIdImageName;
            hdnGuarantorPhotoIdImageNameURL.Value = objGuarantorMasterDAL.xsPhotoIdImageName;
            txtGuarantorAddress1.Text = objGuarantorMasterDAL.Address1;
            txtGuarantorPhone1.Text = objGuarantorMasterDAL.Phone1;
            txtGuarantorMobile1.Text = objGuarantorMasterDAL.Mobile1;
            txtGuarantorFax1.Text = objGuarantorMasterDAL.Fax1;
            txtGuarantorAddress2.Text = objGuarantorMasterDAL.Address2;
            txtGuarantorPhone2.Text = objGuarantorMasterDAL.Phone2;
            txtGuarantorMobile2.Text = objGuarantorMasterDAL.Mobile2;
            txtGuarantorMobile3.Text = objGuarantorMasterDAL.Mobile3;
            txtGuarantorFax2.Text = objGuarantorMasterDAL.Fax2;
            //txtGuarantorCity.Text = objGuarantorMasterDAL.City;

            pnlGuarantor.Enabled = false;
        }

        private void GetWitnessMaster(string IdNo, int WitnessMasterId = 0)
        {
            loanWitnessMasterDAL objWitnessMasterDAL = new loanWitnessMasterDAL();
            objWitnessMasterDAL.WitnessMasterId = WitnessMasterId;
            objWitnessMasterDAL.IdNo = IdNo;
            if (!objWitnessMasterDAL.SelectWitnessMaster())
            {
                hdnWitnessMasterId.Value = string.Empty;
                pnlWitness.Enabled = true;
                hdnModelWitness.Value = "clear";

                loanAppGlobals.ShowMessage(loanMessagesDAL.NotFound, loanMessageIcon.Warning);
                return;
            }
            hdnWitnessMasterId.Value = objWitnessMasterDAL.WitnessMasterId.ToString();
            txtWitnessName.Text = objWitnessMasterDAL.WitnessName;
            txtWitnessIdNo.Text = objWitnessMasterDAL.IdNo;
            hdnWitnessPhotoIdImageName.Value = objWitnessMasterDAL.PhotoIdImageName;
            hdnWitnessPhotoIdImageNameURL.Value = objWitnessMasterDAL.xsPhotoIdImageName;
            txtWitnessAddress1.Text = objWitnessMasterDAL.Address1;
            txtWitnessPhone1.Text = objWitnessMasterDAL.Phone1;
            txtWitnessMobile1.Text = objWitnessMasterDAL.Mobile1;
            txtWitnessFax1.Text = objWitnessMasterDAL.Fax1;
            txtWitnessAddress2.Text = objWitnessMasterDAL.Address2;
            txtWitnessPhone2.Text = objWitnessMasterDAL.Phone2;
            txtWitnessMobile2.Text = objWitnessMasterDAL.Mobile2;
            txtWitnessMobile3.Text = objWitnessMasterDAL.Mobile3;
            txtWitnessFax2.Text = objWitnessMasterDAL.Fax2;
            //txtWitnessCity.Text = objWitnessMasterDAL.City;

            pnlWitness.Enabled = false;
        }

        private void FillGuarantorMaster(int ContractMasterId)
        {

            loanGuarantorMasterDAL objGuarantorMasterDAL = new loanGuarantorMasterDAL();
            objGuarantorMasterDAL.Phone1 = string.Empty;


            int TotalRecords;
            List<loanGuarantorMasterDAL> lstGuarantorMaster = objGuarantorMasterDAL.SelectAllGuarantorMasterPageWise(0, int.MaxValue, out TotalRecords, ContractMasterId);

            if (lstGuarantorMaster == null)
            {
                loanAppGlobals.ShowMessage(loanMessagesDAL.SelectAllFail, loanMessageIcon.Error);
                return;
            }

            lvGuarantorMaster.DataSource = lstGuarantorMaster;
            lvGuarantorMaster.DataBind();

        }

        private void FillWitnessMaster(int ContractMasterId)
        {

            loanWitnessMasterDAL objWitnessMasterDAL = new loanWitnessMasterDAL();
            objWitnessMasterDAL.Phone1 = string.Empty;

            int TotalRecords;
            List<loanWitnessMasterDAL> lstWitnessMaster = objWitnessMasterDAL.SelectAllWitnessMasterPageWise(0, int.MaxValue, out TotalRecords, ContractMasterId);

            if (lstWitnessMaster == null)
            {
                loanAppGlobals.ShowMessage(loanMessagesDAL.SelectAllFail, loanMessageIcon.Error);
                return;
            }

            lvWitnessMaster.DataSource = lstWitnessMaster;
            lvWitnessMaster.DataBind();
        }
        #endregion

        #region WebMethods
        [System.Web.Services.WebMethod]
        public static List<loanCustomerMasterDAL> GetCustomerMaster(string customer)
        {

            if (HttpContext.Current.Session[loanSessionsDAL.UserSession] == null)
            {
                return null;
            }
            loanCustomerMasterDAL objCustomerMasterDAL;
            try
            {
                objCustomerMasterDAL = new loanCustomerMasterDAL();
                objCustomerMasterDAL.CustomerName = customer.Trim();
                objCustomerMasterDAL.linktoCompanyMasterId = ((loanUser)HttpContext.Current.Session[loanSessionsDAL.UserSession]).CompanyMasterId;
                int TotalRecords;
                List<loanCustomerMasterDAL> lstCustomerMasterDAL = objCustomerMasterDAL.SelectAllCustomerMasterPageWise(0, int.MaxValue, out TotalRecords);
                return lstCustomerMasterDAL;
            }
            catch (Exception ex)
            {
                loanAppGlobals.SaveError(ex);
                return null;
            }
            finally
            {
                objCustomerMasterDAL = null;
            }
        }

        [System.Web.Services.WebMethod]
        public static List<loanGuarantorMasterDAL> GetGuarantorMaster(string guarantor)
        {
            if (HttpContext.Current.Session[loanSessionsDAL.UserSession] == null)
            {
                return null;
            }
            loanGuarantorMasterDAL objGuarantorMasterDAL;
            try
            {
                objGuarantorMasterDAL = new loanGuarantorMasterDAL();
                objGuarantorMasterDAL.GuarantorName = guarantor.Trim();
                int TotalRecords;
                List<loanGuarantorMasterDAL> lstGuarantorMasterDAL = objGuarantorMasterDAL.SelectAllGuarantorMasterPageWise(0, short.MaxValue, out TotalRecords);
                return lstGuarantorMasterDAL;
            }
            catch (Exception ex)
            {
                loanAppGlobals.SaveError(ex);
                return null;
            }
            finally
            {
                objGuarantorMasterDAL = null;
            }
        }

        [System.Web.Services.WebMethod]
        public static List<loanWitnessMasterDAL> GetWitnessMaster(string witness)
        {
            if (HttpContext.Current.Session[loanSessionsDAL.UserSession] == null)
            {
                return null;
            }
            loanWitnessMasterDAL objWitnessMasterDAL;
            try
            {
                objWitnessMasterDAL = new loanWitnessMasterDAL();
                objWitnessMasterDAL.WitnessName = witness.Trim();
                int TotalRecords;
                List<loanWitnessMasterDAL> lstWitnessMasterDAL = objWitnessMasterDAL.SelectAllWitnessMasterPageWise(0, int.MaxValue, out TotalRecords);
                return lstWitnessMasterDAL;
            }
            catch (Exception ex)
            {
                loanAppGlobals.SaveError(ex);
                return null;
            }
            finally
            {
                objWitnessMasterDAL = null;
            }
        }
        #endregion
    }
}
