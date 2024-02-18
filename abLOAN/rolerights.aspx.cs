using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using loanLibrary;

namespace abLOAN
{
    public partial class rolerights : BasePage
    {
        List<loanRoleRightsTranDAL> lstRoleRightsTranDAL = new List<loanRoleRightsTranDAL>();
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                loanSessionsDAL.CheckSession();

                if (!Page.IsPostBack)
                {
                    //loanUser.CheckRoleRights(loanRoleRights.Custom, loanRoleRightsName.ManagePermissions);

                    GetPageDefaults();

                    GetRoleRightsGroup();
                }
            }
            catch (Exception ex)
            {
                loanAppGlobals.SaveError(ex);
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            List<loanRoleRightsTranDAL> lstRoleRightsTranDAL = new List<loanRoleRightsTranDAL>();
            loanRoleRightsTranDAL objRoleRightsTranDAL = null;
            try
            {
                objRoleRightsTranDAL = new loanRoleRightsTranDAL();
                objRoleRightsTranDAL.linktoRoleMasterId = Convert.ToInt32(ViewState["RoleMasterId"]);
                if (objRoleRightsTranDAL.linktoRoleMasterId == 1)
                {
                    loanAppGlobals.ShowMessage(loanMessagesDAL.PermissionDenied, loanMessageIcon.Error);
                    return;
                }

                foreach (RepeaterItem rptRightsGroup in rptRoleRightsGroup.Items)
                {
                    ListView lvRoleRights = (ListView)rptRightsGroup.FindControl("lvRoleRights");

                    foreach (ListViewDataItem item in lvRoleRights.Items)
                    {
                        CheckBox chkSelect = (CheckBox)item.FindControl("chkSelect");
                        CheckBox chkSelectViewList = (CheckBox)item.FindControl("chkSelectViewList");
                        CheckBox chkSelectViewRecord = (CheckBox)item.FindControl("chkSelectViewRecord");
                        CheckBox chkSelectAddRecord = (CheckBox)item.FindControl("chkSelectAddRecord");
                        CheckBox chkSelectEditRecord = (CheckBox)item.FindControl("chkSelectEditRecord");
                        CheckBox chkSelectDeleteRecord = (CheckBox)item.FindControl("chkSelectDeleteRecord");

                        if (chkSelect.Checked)
                        {
                            objRoleRightsTranDAL = new loanRoleRightsTranDAL();
                            objRoleRightsTranDAL.linktoRoleMasterId = Convert.ToInt32(ViewState["RoleMasterId"]);
                            objRoleRightsTranDAL.linktoRoleRightsMasterId = Convert.ToInt32(lvRoleRights.DataKeys[item.DataItemIndex].Value);
                            objRoleRightsTranDAL.IsViewList = chkSelectViewList.Checked;
                            objRoleRightsTranDAL.IsViewRecord = chkSelectViewRecord.Checked;
                            objRoleRightsTranDAL.IsAddRecord = chkSelectAddRecord.Checked;
                            objRoleRightsTranDAL.IsEditRecord = chkSelectEditRecord.Checked;
                            objRoleRightsTranDAL.IsDeleteRecord = chkSelectDeleteRecord.Checked;
                            objRoleRightsTranDAL.UpdateDateTime = loanGlobalsDAL.GetCurrentDateTime();
                            objRoleRightsTranDAL.SessionId = ((loanUser)Session[loanSessionsDAL.UserSession]).SessionId;
                            lstRoleRightsTranDAL.Add(objRoleRightsTranDAL);
                        }
                    }
                }

                loanRecordStatus rsStatus = objRoleRightsTranDAL.InsertRoleRightsTran(lstRoleRightsTranDAL);
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

                    loanSessionsDAL.SetSessionKeyValue("lstRoleRightsTranDAL", lstRoleRightsTranDAL);
                    Session[loanSessionsDAL.UserAccessControl] = null;
                }
            }
            catch (Exception ex)
            {
                loanAppGlobals.SaveError(ex);
            }
            finally
            {
                objRoleRightsTranDAL = null;
            }
        }

        #region List Methods
        protected void rptRoleRightsGroup_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            loanRoleRightsMasterDAL objRoleRightsMasterDAL = null;
            List<loanRoleRightsMasterDAL> lstRoleRightsGroupMaster = null;
            try
            {
                loanRoleRightsGroupMasterDAL objRoleRightsGroupMasterDAL = (loanRoleRightsGroupMasterDAL)e.Item.DataItem;

                ListView lvRoleRights = (ListView)e.Item.FindControl("lvRoleRights");

                objRoleRightsMasterDAL = new loanRoleRightsMasterDAL();
                objRoleRightsMasterDAL.linktoRoleRightsGroupMasterId = Convert.ToInt32(objRoleRightsGroupMasterDAL.RoleRightsGroupMasterId);
                lstRoleRightsGroupMaster = objRoleRightsMasterDAL.SelectAllRoleRightsMaster();

                lvRoleRights.DataSource = lstRoleRightsGroupMaster;
                lvRoleRights.DataBind();

                Literal ltrlRoleRightGroup = (Literal)lvRoleRights.FindControl("ltrlRoleRightGroup");
                ltrlRoleRightGroup.Text = objRoleRightsGroupMasterDAL.RoleRightsGroup;
            }
            catch (Exception ex)
            {
                loanAppGlobals.SaveError(ex);
            }
            finally
            {
                objRoleRightsMasterDAL = null;
                lstRoleRightsGroupMaster = null;
            }
        }

        protected void lvRoleRights_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            List<loanRoleRightsTranDAL> lstRoleRightsTranDAL = null;
            try
            {
                if (e.Item.ItemType == ListViewItemType.DataItem)
                {
                    loanRoleRightsMasterDAL objRoleRightsMasterDAL = (loanRoleRightsMasterDAL)e.Item.DataItem;

                    CheckBox chkSelect = (CheckBox)e.Item.FindControl("chkSelect");
                    Literal ltrlRoleRight = (Literal)e.Item.FindControl("ltrlRoleRight");
                    CheckBox chkSelectViewList = (CheckBox)e.Item.FindControl("chkSelectViewList");
                    CheckBox chkSelectViewRecord = (CheckBox)e.Item.FindControl("chkSelectViewRecord");
                    CheckBox chkSelectAddRecord = (CheckBox)e.Item.FindControl("chkSelectAddRecord");
                    CheckBox chkSelectEditRecord = (CheckBox)e.Item.FindControl("chkSelectEditRecord");
                    CheckBox chkSelectDeleteRecord = (CheckBox)e.Item.FindControl("chkSelectDeleteRecord");

                    ltrlRoleRight.Text = objRoleRightsMasterDAL.RoleRight;
                    chkSelectViewList.Visible = objRoleRightsMasterDAL.IsAvailableViewList;
                    chkSelectViewRecord.Visible = objRoleRightsMasterDAL.IsAvailableViewRecord;
                    chkSelectAddRecord.Visible = objRoleRightsMasterDAL.IsAvailableAddRecord;
                    chkSelectEditRecord.Visible = objRoleRightsMasterDAL.IsAvailableEditRecord;
                    chkSelectDeleteRecord.Visible = objRoleRightsMasterDAL.IsAvailableDeleteRecord;

                    if (Convert.ToInt32(ViewState["RoleMasterId"]) == 1)
                    {
                        chkSelect.Checked = true;
                        chkSelect.Enabled = false;
                        chkSelectViewList.Checked = true;
                        chkSelectViewList.Enabled = false;
                        chkSelectViewRecord.Checked = true;
                        chkSelectViewRecord.Enabled = false;
                        chkSelectAddRecord.Checked = true;
                        chkSelectAddRecord.Enabled = false;
                        chkSelectEditRecord.Checked = true;
                        chkSelectEditRecord.Enabled = false;
                        chkSelectDeleteRecord.Checked = true;
                        chkSelectDeleteRecord.Enabled = false;
                        return;
                    }

                    lstRoleRightsTranDAL = (List<loanRoleRightsTranDAL>)loanSessionsDAL.GetSessionKeyValue("lstRoleRightsTranDAL");
                    loanRoleRightsTranDAL objRoleRightsTranDAL = lstRoleRightsTranDAL.Find(x => x.linktoRoleRightsMasterId == objRoleRightsMasterDAL.RoleRightsMasterId);
                    if (objRoleRightsTranDAL != null)
                    {
                        chkSelect.Checked = true;
                        chkSelectViewList.Checked = objRoleRightsTranDAL.IsViewList;
                        chkSelectViewRecord.Checked = objRoleRightsTranDAL.IsViewRecord;
                        chkSelectAddRecord.Checked = objRoleRightsTranDAL.IsAddRecord;
                        chkSelectEditRecord.Checked = objRoleRightsTranDAL.IsEditRecord;
                        chkSelectDeleteRecord.Checked = objRoleRightsTranDAL.IsDeleteRecord;
                    }
                }
            }
            catch (Exception ex)
            {
                loanAppGlobals.SaveError(ex);
            }
            finally
            {
                lstRoleRightsTranDAL = null;
            }
        }

        #endregion

        #region Private Methods
        private void GetPageDefaults()
        {
            loanRoleRightsTranDAL objRoleRightsTranDAL = null;
            try
            {
                if (loanSessionsDAL.GetSessionKeyValue("Role") != null)
                {
                    lblRole.Text = loanSessionsDAL.GetSessionKeyValue("Role").ToString();
                }
                if (loanSessionsDAL.GetSessionKeyValue("RoleMasterId") != null)
                {
                    ViewState["RoleMasterId"] = loanSessionsDAL.GetSessionKeyValue("RoleMasterId");
                }
                else
                {
                    Response.Redirect("role.aspx");
                    return;
                }

                objRoleRightsTranDAL = new loanRoleRightsTranDAL();
                objRoleRightsTranDAL.linktoRoleMasterId = Convert.ToInt32(ViewState["RoleMasterId"]);
                lstRoleRightsTranDAL = objRoleRightsTranDAL.SelectAllRoleRightsTran();

                loanSessionsDAL.SetSessionKeyValue("lstRoleRightsTranDAL", lstRoleRightsTranDAL);

                if (Convert.ToInt32(ViewState["RoleMasterId"]) == 1)
                {
                    btnSave.Enabled = false;
                    btnSaveTop.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                loanAppGlobals.SaveError(ex);
            }
            finally
            {
                objRoleRightsTranDAL = null;
            }
        }

        private void GetRoleRightsGroup()
        {
            loanRoleRightsGroupMasterDAL objRoleRightsGroupMasterDAL = null;
            List<loanRoleRightsGroupMasterDAL> lstRoleRightsGroupMasterDAL = null;
            try
            {
                objRoleRightsGroupMasterDAL = new loanRoleRightsGroupMasterDAL();
                lstRoleRightsGroupMasterDAL = objRoleRightsGroupMasterDAL.SelectAllRoleRightsGroupMaster();

                rptRoleRightsGroup.DataSource = lstRoleRightsGroupMasterDAL;
                rptRoleRightsGroup.DataBind();
            }
            catch (Exception ex)
            {
                loanAppGlobals.SaveError(ex);
            }
            finally
            {
                objRoleRightsGroupMasterDAL = null;
                lstRoleRightsGroupMasterDAL = null;
            }
        }
        #endregion
    }
}