using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using loanLibrary;

namespace abLOAN
{
    public partial class errorlog : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                loanSessionsDAL.CheckSession();

                if (!Page.IsPostBack)
                {
                    //  loanSessionDAL.RemoveSessionAllKeyValue();
                    FillErrorLog();
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
                pgrErrorLog.CurrentPage = 1;
                FillErrorLog();
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
                txtFilterErrorLogId.Text = string.Empty;
                txtFilterErrorDateTime.Text = string.Empty;
                txtFilterErrorMessage.Text = string.Empty;

                pgrErrorLog.CurrentPage = 1;
                FillErrorLog();
            }
            catch (Exception ex)
            {
                loanAppGlobals.SaveError(ex);
            }
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            CheckBox chkSelect;
            StringBuilder ids;
            try
            {
                ids = new StringBuilder();
                foreach (ListViewDataItem item in lvErrorLog.Items)
                {
                    chkSelect = (CheckBox)item.FindControl("chkSelect");
                    if (chkSelect != null && chkSelect.Checked)
                    {
                        ids.Append(lvErrorLog.DataKeys[item.DisplayIndex].Value + ",");
                    }
                }
                if (ids.Length > 0)
                {
                    ids.Length -= 1;
                }

                loanRecordStatus rsStatus = loanErrorLogDAL.DeleteAllErrorLog(ids.ToString());
                if (rsStatus == loanRecordStatus.Success)
                {
                    loanAppGlobals.ShowMessage(loanMessagesDAL.DeleteAllSuccess, loanMessageIcon.Success);
                    FillErrorLog();
                }
                else
                {
                    loanAppGlobals.ShowMessage(loanMessagesDAL.DeleteAllFail, loanMessageIcon.Error);
                }
            }
            catch (Exception ex)
            {
                loanAppGlobals.SaveError(ex);
            }
            finally
            {
                ids = null;
            }
        }

        #region List Methods
        protected void lvErrorLog_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            try
            {
                if (e.Item.ItemType == ListViewItemType.DataItem)
                {
                    loanErrorLogDAL objErrorLogDAL = (loanErrorLogDAL)e.Item.DataItem;

                    Label lblErrorLogId = (Label)e.Item.FindControl("lblErrorLogId");
                    Literal ltrlErrorDateTime = (Literal)e.Item.FindControl("ltrlErrorDateTime");
                    Label lblErrorMessage = (Label)e.Item.FindControl("lblErrorMessage");
                    Literal ltrlErrorStackTrace = (Literal)e.Item.FindControl("ltrlErrorStackTrace");

                    lblErrorLogId.Text = objErrorLogDAL.ErrorLogId.ToString();
                    ltrlErrorDateTime.Text = loanGlobalsDAL.ConvertDateTimeToString(objErrorLogDAL.ErrorDateTime, loanAppGlobals.DateTimeFormat);
                    lblErrorMessage.Text = objErrorLogDAL.ErrorMessage;
                    ltrlErrorStackTrace.Text = objErrorLogDAL.ErrorStackTrace;
                }
            }
            catch (Exception ex)
            {
                loanAppGlobals.SaveError(ex);
            }
        }

        protected void pgrErrorLog_ItemCommand(object sender, EventArgs e)
        {
            try
            {
                FillErrorLog();
            }
            catch (Exception ex)
            {
                loanAppGlobals.SaveError(ex);
            }
        }

        protected void lvErrorLog_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
            loanErrorLogDAL objErrorLogDAL;
            try
            {
                if (e.CommandName.Equals("DeleteRecord", StringComparison.CurrentCultureIgnoreCase))
                {
                    objErrorLogDAL = new loanErrorLogDAL();
                    objErrorLogDAL.ErrorLogId = Convert.ToInt32(((ListView)sender).DataKeys[e.Item.DataItemIndex].Value);
                    loanRecordStatus rsStatus = objErrorLogDAL.DeleteErrorLog();
                    if (rsStatus == loanRecordStatus.Success)
                    {
                        loanAppGlobals.ShowMessage(loanMessagesDAL.DeleteSuccess, loanMessageIcon.Success);
                        FillErrorLog();
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
            finally
            {
                objErrorLogDAL = null;
            }
        }
        #endregion

        #region Private Methods

        private void FillErrorLog()
        {
            loanErrorLogDAL objErrorLogDAL;
            List<loanErrorLogDAL> lstErrorLog;
            try
            {
                btnDelete.Visible = false;

                objErrorLogDAL = new loanErrorLogDAL();
                if (!string.IsNullOrEmpty(txtFilterErrorLogId.Text))
                {
                    objErrorLogDAL.ErrorLogId = Convert.ToInt32(txtFilterErrorLogId.Text);
                }
                if (!string.IsNullOrEmpty(txtFilterErrorDateTime.Text))
                {
                    objErrorLogDAL.ErrorDateTime = DateTime.ParseExact(txtFilterErrorDateTime.Text, loanAppGlobals.DateFormat, System.Globalization.DateTimeFormatInfo.InvariantInfo);
                }
                objErrorLogDAL.ErrorMessage = txtFilterErrorMessage.Text.Trim();

                int TotalRecords;
                lstErrorLog = objErrorLogDAL.SelectAllErrorLogPageWise(pgrErrorLog.StartRowIndex, pgrErrorLog.PageSize, out TotalRecords);
                pgrErrorLog.TotalRowCount = TotalRecords;

                if (lstErrorLog == null)
                {
                    loanAppGlobals.ShowMessage(loanMessagesDAL.SelectAllFail, loanMessageIcon.Error);
                    return;
                }

                if (lstErrorLog.Count == 0 && pgrErrorLog.TotalRowCount > 0)
                {
                    pgrErrorLog_ItemCommand(pgrErrorLog, new EventArgs());
                    return;
                }

                lvErrorLog.DataSource = lstErrorLog;
                lvErrorLog.DataBind();

                if (lstErrorLog.Count > 0)
                {
                    int EndiIndex = pgrErrorLog.StartRowIndex + pgrErrorLog.PageSize < pgrErrorLog.TotalRowCount ? pgrErrorLog.StartRowIndex + pgrErrorLog.PageSize : pgrErrorLog.TotalRowCount;
                    lblRecords.Text = "[" + (pgrErrorLog.StartRowIndex + 1) + " to " + EndiIndex + " of " + pgrErrorLog.TotalRowCount + " Records]";
                    ((CheckBox)lvErrorLog.FindControl("chkHeader")).Checked = false;
                    lblRecords.Visible = true;
                    btnDelete.Visible = true;
                }
                else
                {
                    lblRecords.Visible = false;
                }

                if (pgrErrorLog.TotalRowCount <= pgrErrorLog.PageSize)
                {
                    pgrErrorLog.Visible = false;
                }
                else
                {
                    pgrErrorLog.Visible = true;
                }
            }
            catch (Exception ex)
            {
                loanAppGlobals.SaveError(ex);
            }
            finally
            {
                objErrorLogDAL = null;
                lstErrorLog = null;
            }
        }
        #endregion
    }
}
