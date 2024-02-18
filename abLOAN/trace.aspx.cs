using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using loanLibrary;

namespace abLOAN
{
    public partial class trace : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                //loanSessionsDAL.CheckSession();

                if (!Page.IsPostBack)
                {
                    loanUser.CheckRoleRights(loanRoleRights.ViewList);

                    GetPageDefaults();

                    //loanSessionsDAL.RemoveSessionAllKeyValue();

                    FillTraceMaster();
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
                pgrTraceMaster.CurrentPage = 1;
                FillTraceMaster();
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
                txtFilterTableName.Text = string.Empty;
                txtFilterOperationType.Text = string.Empty;
                txtFilterOperationDate.Text = loanGlobalsDAL.ConvertDateTimeToString(loanGlobalsDAL.GetCurrentDateTime().AddMonths(-1), loanAppGlobals.DateFormat);
                txtFilterOperationDateTo.Text = loanGlobalsDAL.ConvertDateTimeToString(loanGlobalsDAL.GetCurrentDateTime().AddDays(1), loanAppGlobals.DateFormat);

                pgrTraceMaster.CurrentPage = 1;
                FillTraceMaster();
            }
            catch (Exception ex)
            {
                loanAppGlobals.SaveError(ex);
            }
        }

        #region List Methods

        protected void lvTraceMaster_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            try
            {
                if (e.Item.ItemType == ListViewItemType.DataItem)
                {
                    loanTraceMasterDAL objTraceMasterDAL = (loanTraceMasterDAL)e.Item.DataItem;

                    Literal ltrlTableName = (Literal)e.Item.FindControl("ltrlTableName");
                    Literal ltrlOperationType = (Literal)e.Item.FindControl("ltrlOperationType");
                    Literal ltrlValue = (Literal)e.Item.FindControl("ltrlValue");
                    HiddenField hdnRowId = (HiddenField)e.Item.FindControl("hdnRowId");
                    Literal ltrlCreateDateTime = (Literal)e.Item.FindControl("ltrlCreateDateTime");
                    Literal ltrlModifiedBy = (Literal)e.Item.FindControl("ltrlModifiedBy");

                    ltrlModifiedBy.Text = objTraceMasterDAL.ModifiedBy;
                    ltrlTableName.Text = objTraceMasterDAL.TableName.Replace("loan", "").Replace("Master", "").Replace("Tran", "");
                    ltrlOperationType.Text = objTraceMasterDAL.OperationType;
                    hdnRowId.Value = objTraceMasterDAL.RowId.ToString();
                    ltrlValue.Text = objTraceMasterDAL.Value;
                    ltrlCreateDateTime.Text = loanGlobalsDAL.ConvertDateTimeToString(objTraceMasterDAL.CreateDateTime, loanAppGlobals.DateTimeFormat);
                    if (objTraceMasterDAL.OperationType != "D")
                    {
                        if (!objTraceMasterDAL.SelectTraceMaster())
                        {
                            if (ltrlTableName.Text.Contains("Contract"))
                            {
                                ltrlTableName.Text += "details";
                            }
                            string s = ltrlTableName.Text + ".aspx?id=" + hdnRowId.Value;
                            HiddenField hdnUrl = (HiddenField)e.Item.FindControl("hdnUrl");
                            hdnUrl.Value = s;

                            LinkButton lbtnView = (LinkButton)e.Item.FindControl("lbtnView");
                            lbtnView.Visible = true;

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                loanAppGlobals.SaveError(ex);
            }
        }

        protected void pgrTraceMaster_ItemCommand(object sender, EventArgs e)
        {
            try
            {
                FillTraceMaster();
            }
            catch (Exception ex)
            {
                loanAppGlobals.SaveError(ex);
            }
        }
        protected void lvTraceMaster_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName.Equals("ViewRecord", StringComparison.CurrentCultureIgnoreCase))
                {
                    HiddenField hdnUrl = (HiddenField)e.Item.FindControl("hdnUrl");
                    Response.Redirect(hdnUrl.Value);
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
            txtFilterOperationDate.Text = loanGlobalsDAL.ConvertDateTimeToString(loanGlobalsDAL.GetCurrentDateTime().AddMonths(-1), loanAppGlobals.DateFormat);
            txtFilterOperationDateTo.Text = loanGlobalsDAL.ConvertDateTimeToString(loanGlobalsDAL.GetCurrentDateTime().AddDays(1), loanAppGlobals.DateFormat);

            if (loanSessionsDAL.GetSessionKeyValue("CurrentPageTrace") != null)
            {
                pgrTraceMaster.CurrentPage = Convert.ToInt16(loanSessionsDAL.GetSessionKeyValue("CurrentPageTrace"));
            }
            if (loanSessionsDAL.GetSessionKeyValue("FilterTrace") != null)
            {
                loanTraceMasterDAL objTraceMasterDAL = (loanTraceMasterDAL)loanSessionsDAL.GetSessionKeyValue("FilterTrace");
                txtFilterTableName.Text = objTraceMasterDAL.TableName;
                txtFilterOperationType.Text = objTraceMasterDAL.OperationType;

            }
        }

        private void FillTraceMaster()
        {

            loanTraceMasterDAL objTraceMasterDAL = new loanTraceMasterDAL();
            objTraceMasterDAL.TableName = txtFilterTableName.Text.Trim();
            objTraceMasterDAL.OperationType = txtFilterOperationType.Text.Trim();
            DateTime? OperationDateFrom = null;
            if (!string.IsNullOrEmpty(txtFilterOperationDate.Text))
            {
                OperationDateFrom = DateTime.ParseExact(txtFilterOperationDate.Text, loanAppGlobals.DateFormat, System.Globalization.DateTimeFormatInfo.InvariantInfo);
                objTraceMasterDAL.CreateDateTime = DateTime.ParseExact(txtFilterOperationDate.Text, loanAppGlobals.DateFormat, System.Globalization.DateTimeFormatInfo.InvariantInfo);
            }
            DateTime? OperationDateTo = null;
            if (!string.IsNullOrEmpty(txtFilterOperationDateTo.Text))
            {
                OperationDateTo = DateTime.ParseExact(txtFilterOperationDateTo.Text, loanAppGlobals.DateFormat, System.Globalization.DateTimeFormatInfo.InvariantInfo);
                objTraceMasterDAL.CreateDateTime = DateTime.ParseExact(txtFilterOperationDateTo.Text, loanAppGlobals.DateFormat, System.Globalization.DateTimeFormatInfo.InvariantInfo);
            }

            loanSessionsDAL.SetSessionKeyValue("FilterTrace", objTraceMasterDAL);
            loanSessionsDAL.SetSessionKeyValue("CurrentPageTrace", pgrTraceMaster.CurrentPage);

            int TotalRecords;
            List<loanTraceMasterDAL> lstTraceMaster = objTraceMasterDAL.SelectAllTraceMasterPageWise(pgrTraceMaster.StartRowIndex, 20, out TotalRecords, OperationDateFrom, OperationDateTo);
            pgrTraceMaster.TotalRowCount = TotalRecords;

            if (lstTraceMaster == null)
            {
                loanAppGlobals.ShowMessage(loanMessagesDAL.SelectAllFail, loanMessageIcon.Error);
                return;
            }

            if (lstTraceMaster.Count == 0 && pgrTraceMaster.TotalRowCount > 0)
            {
                pgrTraceMaster_ItemCommand(pgrTraceMaster, new EventArgs());
                return;
            }

            lvTraceMaster.DataSource = lstTraceMaster;
            lvTraceMaster.DataBind();

            if (lstTraceMaster.Count > 0)
            {
                int EndiIndex = pgrTraceMaster.StartRowIndex + pgrTraceMaster.PageSize < pgrTraceMaster.TotalRowCount ? pgrTraceMaster.StartRowIndex + pgrTraceMaster.PageSize : pgrTraceMaster.TotalRowCount;
                lblRecords.Text = string.Format(Resources.Messages.ResourceManager.GetString("Records"), pgrTraceMaster.StartRowIndex + 1, EndiIndex, pgrTraceMaster.TotalRowCount);
                lblRecords.Visible = true;
            }
            else
            {
                lblRecords.Visible = false;
            }

            if (pgrTraceMaster.TotalRowCount <= pgrTraceMaster.PageSize)
            {
                pgrTraceMaster.Visible = false;
            }
            else
            {
                pgrTraceMaster.Visible = true;
            }

        }

        #endregion


    }
}
