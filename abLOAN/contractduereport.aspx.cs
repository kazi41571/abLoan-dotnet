using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using loanLibrary;
using System.Threading;
using System.Globalization;
using System.Data;
using System.Text;
using System.IO;
using System.Configuration;

namespace abLOAN
{
    public partial class contractduereport : BasePage
    {
        List<loanContractMasterDAL> lstContractMaster;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                loanSessionsDAL.CheckSession();

                if (!Page.IsPostBack)
                {
                    //loanUser.CheckUserRights(loanUserRights.ViewContract);
                    loanUser.CheckRoleRights(loanRoleRights.ViewList);

                    GetContractStatus();
                    GetPageDefaults();
                    GetBank();

                    loanSessionsDAL.RemoveSessionAllKeyValue();

                    FillContractMaster();
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
                pgrContractMaster.CurrentPage = 1;
                FillContractMaster();
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
                txtFilterContractTitle.Text = string.Empty;
                txtFilterFileNo.Text = string.Empty;
                txtFilterLastPaidDateFrom.Text = null;
                txtFilterLastPaidDateTo.Text = null;
                //txtFilterContractDateTo.Text = loanGlobalsDAL.ConvertDateTimeToString(loanGlobalsDAL.GetCurrentDateTime(), loanAppGlobals.DateFormat);
                txtFilterDueDateFrom.Text = null;
                txtFilterDueDateTo.Text = null;
                ddlFilterContractStatus.SelectedIndex = 0;
                ddlFilterBank.SelectedIndex = 0;

                pgrContractMaster.CurrentPage = 1;
                FillContractMaster();
            }
            catch (Exception ex)
            {
                loanAppGlobals.SaveError(ex);
            }
        }

        #region List Methods

        protected void lvContractMaster_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            try
            {
                if (e.Item.ItemType == ListViewItemType.DataItem)
                {
                    loanContractMasterDAL objContractMasterDAL = (loanContractMasterDAL)e.Item.DataItem;

                    //Literal ltrlCustomerIdNo = (Literal)e.Item.FindControl("ltrlCustomerIdNo");
                    Literal ltrlCustomer = (Literal)e.Item.FindControl("ltrlCustomer");
                    Literal ltrlBank = (Literal)e.Item.FindControl("ltrlBank");
                    Literal ltrlContractTitle = (Literal)e.Item.FindControl("ltrlContractTitle");
                    Literal ltrlFileNo = (Literal)e.Item.FindControl("ltrlFileNo");
                    Literal ltrlLastPaidDate = (Literal)e.Item.FindControl("ltrlLastPaidDate");
                    Literal ltrlDueAmount = (Literal)e.Item.FindControl("ltrlDueAmount");

                    //ltrlCustomerIdNo.Text = objContractMasterDAL.CustomerIdNo;
                    if (objContractMasterDAL.CustomerIsRedFlag == true)
                    {
                        ltrlCustomer.Text = "<span class='text-danger'>" + objContractMasterDAL.Customer + "</span>";
                    }
                    else
                    {
                        ltrlCustomer.Text = objContractMasterDAL.Customer;
                    }
                    ltrlBank.Text = objContractMasterDAL.Bank;
                    ltrlContractTitle.Text = objContractMasterDAL.ContractTitle;
                    ltrlFileNo.Text = objContractMasterDAL.FileNo.ToString();
                    ltrlLastPaidDate.Text = loanGlobalsDAL.ConvertDateTimeToString(objContractMasterDAL.LastPaidDate, loanAppGlobals.DateFormat);
                    ltrlDueAmount.Text = objContractMasterDAL.DueAmount.ToString("0.00");
                }
            }
            catch (Exception ex)
            {
                loanAppGlobals.SaveError(ex);
            }
        }

        protected void pgrContractMaster_ItemCommand(object sender, EventArgs e)
        {
            try
            {
                FillContractMaster();
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
            //txtFilterContractDate.Text = loanGlobalsDAL.ConvertDateTimeToString(loanGlobalsDAL.GetCurrentDateTime().AddYears(-20), loanAppGlobals.DateFormat);
            //txtFilterContractDateTo.Text = loanGlobalsDAL.ConvertDateTimeToString(loanGlobalsDAL.GetCurrentDateTime(), loanAppGlobals.DateFormat);

            if (loanSessionsDAL.GetSessionKeyValue("CurrentPageContract") != null)
            {
                pgrContractMaster.CurrentPage = Convert.ToInt16(loanSessionsDAL.GetSessionKeyValue("CurrentPageContract"));
            }
            if (loanSessionsDAL.GetSessionKeyValue("FilterContract") != null)
            {
                loanContractMasterDAL objContractMasterDAL = (loanContractMasterDAL)loanSessionsDAL.GetSessionKeyValue("FilterContract");
                txtFilterCustomer.Text = objContractMasterDAL.Customer;
                txtFilterContractTitle.Text = objContractMasterDAL.ContractTitle;
                if (objContractMasterDAL.FileNo > 0)
                {
                    txtFilterFileNo.Text = objContractMasterDAL.FileNo.ToString();
                }
                //if (objContractMasterDAL.ContractStartDate != new DateTime())
                //{
                //    txtFilterContractDate.Text = loanGlobalsDAL.ConvertDateTimeToString(objContractMasterDAL.ContractStartDate, loanAppGlobals.DateFormat);
                //}
                //if (objContractMasterDAL.ContractDate != new DateTime())
                //{
                //    txtFilterContractDateTo.Text = loanGlobalsDAL.ConvertDateTimeToString(objContractMasterDAL.ContractDate, loanAppGlobals.DateFormat);
                //}
                ddlFilterContractStatus.SelectedValue = objContractMasterDAL.linktoContractStatusMasterId.ToString();
                ddlFilterBank.SelectedValue = objContractMasterDAL.linktoBankMasterId.ToString();
            }
        }

        private void FillContractMaster()
        {

            loanContractMasterDAL objContractMasterDAL = new loanContractMasterDAL();
            string IdNo = txtFilterCustomer.Text.Trim();
            if (IdNo.Contains("("))
            {
                IdNo = IdNo.Substring(IdNo.IndexOf("(") + 1, IdNo.IndexOf(")") - IdNo.IndexOf("(") - 1);
            }
            objContractMasterDAL.Customer = IdNo;
            objContractMasterDAL.ContractTitle = txtFilterContractTitle.Text.Trim();
            if (!string.IsNullOrEmpty(txtFilterFileNo.Text))
            {
                objContractMasterDAL.FileNo = Convert.ToInt32(txtFilterFileNo.Text);
            }
            DateTime? InstallmentDateFrom = null;
            DateTime? InstallmentDateTo = null;
            if (!string.IsNullOrEmpty(txtFilterLastPaidDateFrom.Text))
            {
                InstallmentDateFrom = DateTime.ParseExact(txtFilterLastPaidDateFrom.Text, loanAppGlobals.DateFormat, System.Globalization.DateTimeFormatInfo.InvariantInfo);
                //objContractMasterDAL.InstallmentDate = DateTime.ParseExact(txtFilterLastPaidDateFrom.Text, loanAppGlobals.DateFormat, System.Globalization.DateTimeFormatInfo.InvariantInfo);
            }
            if (!string.IsNullOrEmpty(txtFilterLastPaidDateTo.Text))
            {
                InstallmentDateTo = DateTime.ParseExact(txtFilterLastPaidDateTo.Text, loanAppGlobals.DateFormat, System.Globalization.DateTimeFormatInfo.InvariantInfo);
                //objContractMasterDAL.InstallmentDate = DateTime.ParseExact(txtFilterLastPaidDateTo.Text, loanAppGlobals.DateFormat, System.Globalization.DateTimeFormatInfo.InvariantInfo);
            }
            //DateTime? ContractDateTo = null;
            //if (!string.IsNullOrEmpty(txtFilterContractDateTo.Text))
            //{
            //    ContractDateTo = DateTime.ParseExact(txtFilterContractDateTo.Text, loanAppGlobals.DateFormat, System.Globalization.DateTimeFormatInfo.InvariantInfo);
            //    objContractMasterDAL.ContractDate = DateTime.ParseExact(txtFilterContractDateTo.Text, loanAppGlobals.DateFormat, System.Globalization.DateTimeFormatInfo.InvariantInfo);
            //}
            if (!string.IsNullOrEmpty(txtFilterDueDateFrom.Text))
            {
                objContractMasterDAL.DueDateFrom = DateTime.ParseExact(txtFilterDueDateFrom.Text, loanAppGlobals.DateFormat, System.Globalization.DateTimeFormatInfo.InvariantInfo);
            }
            if (!string.IsNullOrEmpty(txtFilterDueDateTo.Text))
            {
                objContractMasterDAL.DueDateTo = DateTime.ParseExact(txtFilterDueDateTo.Text, loanAppGlobals.DateFormat, System.Globalization.DateTimeFormatInfo.InvariantInfo);
            }
            if (ddlFilterContractStatus.SelectedValue != string.Empty)
            {
                objContractMasterDAL.linktoContractStatusMasterId = Convert.ToInt32(ddlFilterContractStatus.SelectedValue);
            }
            objContractMasterDAL.linktoCompanyMasterId = ((loanUser)Session[loanSessionsDAL.UserSession]).CompanyMasterId;
            if (ddlFilterBank.SelectedValue != string.Empty)
            {
                objContractMasterDAL.linktoBankMasterId = Convert.ToInt32(ddlFilterBank.SelectedValue);
            }

            loanSessionsDAL.SetSessionKeyValue("FilterContract", objContractMasterDAL);
            loanSessionsDAL.SetSessionKeyValue("CurrentPageContract", pgrContractMaster.CurrentPage);

            bool WithDueAmount = true;
            int TotalRecords;
            lstContractMaster = objContractMasterDAL.SelectAllContractMasterPageWise(pgrContractMaster.StartRowIndex, pgrContractMaster.PageSize, out TotalRecords, null, null, WithDueAmount, 0, InstallmentDateFrom, InstallmentDateTo);
            pgrContractMaster.TotalRowCount = TotalRecords;

            if (lstContractMaster == null)
            {
                loanAppGlobals.ShowMessage(loanMessagesDAL.SelectAllFail, loanMessageIcon.Error);
                return;
            }

            if (lstContractMaster.Count == 0 && pgrContractMaster.TotalRowCount > 0)
            {
                pgrContractMaster_ItemCommand(pgrContractMaster, new EventArgs());
                return;
            }

            lvContractMaster.DataSource = lstContractMaster;
            lvContractMaster.DataBind();
            Session["lstContractMaster"] = lstContractMaster;

            if (lstContractMaster.Count > 0)
            {
                int EndiIndex = pgrContractMaster.StartRowIndex + pgrContractMaster.PageSize < pgrContractMaster.TotalRowCount ? pgrContractMaster.StartRowIndex + pgrContractMaster.PageSize : pgrContractMaster.TotalRowCount;
                lblRecords.Text = string.Format(Resources.Messages.ResourceManager.GetString("Records"), pgrContractMaster.StartRowIndex + 1, EndiIndex, pgrContractMaster.TotalRowCount);
                lblRecords.Visible = true;
            }
            else
            {
                lblRecords.Visible = false;
            }

            if (pgrContractMaster.TotalRowCount <= pgrContractMaster.PageSize)
            {
                pgrContractMaster.Visible = false;
            }
            else
            {
                pgrContractMaster.Visible = true;
            }
        }

        private void GetContractStatus()
        {
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

        private void GetBank()
        {
            ddlFilterBank.Items.Clear();
            ddlFilterBank.Items.Add(new System.Web.UI.WebControls.ListItem(loanDropDownItem.Select, ""));

            List<loanBankMasterDAL> lstBankMasterDAL = loanBankMasterDAL.SelectAllBankMasterBankName();
            if (lstBankMasterDAL == null)
            {
                loanAppGlobals.ShowMessage(loanMessagesDAL.SelectAllFail, loanMessageIcon.Error);
                return;
            }
            foreach (loanBankMasterDAL obj in lstBankMasterDAL)
            {
                ddlFilterBank.Items.Add(new System.Web.UI.WebControls.ListItem(obj.BankName, obj.BankMasterId.ToString()));
            }
        }

        #endregion

        protected void btnExportToExcel_Click(object sender, EventArgs e)
        {
            ExportCSVContactMaster();
        }

        protected void ExportCSVContactMaster()
        {
            StringBuilder sbCSV = new StringBuilder();
            string[] Headers = null;
            if (Session["Language"].ToString() == "en-us")
            {
                Headers = new string[] {
                    Resources.Resource.CustomerName,
                    Resources.Resource.Bank,
                    Resources.Resource.ContractTitle,
                    Resources.Resource.FileNo,
                    Resources.Resource.LastPaidDate,
                    Resources.Resource.DueAmount
                };
            }
            else
            {
                Headers = new string[] {
                    Resources.Resource.DueAmount,
                    Resources.Resource.LastPaidDate,
                    Resources.Resource.FileNo,
                    Resources.Resource.ContractTitle,
                    Resources.Resource.Bank,
                    Resources.Resource.CustomerName
                    };
            }
            for (int i = 0; i < Headers.Length; i++)
            {
                sbCSV.Append("\"" + Headers[i].Replace("\"", "\"\"") + "\",");
                if (i == Headers.Length - 1)
                {
                    sbCSV.Length -= 1;
                    sbCSV.AppendLine();
                }
            }
            List<loanContractMasterDAL> lstContractMaster = (List<loanContractMasterDAL>)Session["lstContractMaster"];
            for (int j = 0; j < lstContractMaster.Count; j++)
            {
                if (Session["Language"].ToString() == "en-us")
                {
                    sbCSV.Append("\"" + lstContractMaster[j].Customer.ToString().Replace("\"", "\"\"") + "\",");
                    sbCSV.Append("\"" + lstContractMaster[j].Bank.ToString().Replace("\"", "\"\"") + "\",");
                    sbCSV.Append("\"" + lstContractMaster[j].ContractTitle.ToString().Replace("\"", "\"\"") + "\",");
                    sbCSV.Append("\"" + lstContractMaster[j].FileNo.ToString().Replace("\"", "\"\"") + "\",");
                    sbCSV.Append("\"" + lstContractMaster[j].LastPaidDate.ToString().Replace("\"", "\"\"") + "\",");
                    sbCSV.Append("\"" + lstContractMaster[j].DueAmount.ToString("0.00").Replace("\"", "\"\"") + "\",");
                }
                else
                {
                    sbCSV.Append("\"" + lstContractMaster[j].DueAmount.ToString("0.00").Replace("\"", "\"\"") + "\",");
                    sbCSV.Append("\"" + lstContractMaster[j].LastPaidDate.ToString().Replace("\"", "\"\"") + "\",");
                    sbCSV.Append("\"" + lstContractMaster[j].FileNo.ToString().Replace("\"", "\"\"") + "\",");
                    sbCSV.Append("\"" + lstContractMaster[j].ContractTitle.ToString().Replace("\"", "\"\"") + "\",");
                    sbCSV.Append("\"" + lstContractMaster[j].Bank.ToString().Replace("\"", "\"\"") + "\",");
                    sbCSV.Append("\"" + lstContractMaster[j].Customer.ToString().Replace("\"", "\"\"") + "\",");
                }
                sbCSV.Length -= 1;
                sbCSV.AppendLine();
            }

            if (!Directory.Exists(ConfigurationManager.AppSettings["FileSavePath"]))
            {
                Directory.CreateDirectory(ConfigurationManager.AppSettings["FileSavePath"]);
            }
            File.WriteAllText(ConfigurationManager.AppSettings["FileSavePath"] + "Due" + loanGlobalsDAL.GetCurrentDateTime().ToString(" [yyyy-MM-dd HH.mm.ss]") + ".csv", sbCSV.ToString(), Encoding.UTF8);
            loanAppGlobals.SendOutFile(ConfigurationManager.AppSettings["FileSavePath"] + "Due" + loanGlobalsDAL.GetCurrentDateTime().ToString(" [yyyy-MM-dd HH.mm.ss]") + ".csv", "Due" + loanGlobalsDAL.GetCurrentDateTime().ToString(" [yyyy-MM-dd HH.mm.ss]") + ".csv");

        }
    }
}
