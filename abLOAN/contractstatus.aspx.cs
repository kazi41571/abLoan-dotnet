using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using loanLibrary;

namespace abLOAN
{
	public partial class contractstatus : BasePage
    {
		protected void Page_Load(object sender, EventArgs e)
		{
			try
			{
				loanSessionsDAL.CheckSession();

				if (!Page.IsPostBack)
				{
					//loanUser.CheckUserRights(loanUserRights.ViewContractStatus);

					loanSessionsDAL.RemoveSessionAllKeyValue();

					FillContractStatusMaster();
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
				loanContractStatusMasterDAL objContractStatusMasterDAL = new loanContractStatusMasterDAL();
				objContractStatusMasterDAL.ContractStatus = txtContractStatus.Text.Trim();

				objContractStatusMasterDAL.UpdateDateTime = loanGlobalsDAL.GetCurrentDateTime();
				objContractStatusMasterDAL.SessionId = ((loanUser)Session[loanSessionsDAL.UserSession]).SessionId;

				if (string.IsNullOrEmpty(hdnActionContractStatus.Value))
				{
					loanRecordStatus rsStatus = objContractStatusMasterDAL.InsertContractStatusMaster();
					if (rsStatus == loanRecordStatus.Error)
					{
						loanAppGlobals.ShowMessage(loanMessagesDAL.InsertFail, loanMessageIcon.Error);
						return;
					}
					else if (rsStatus == loanRecordStatus.RecordAlreadyExist)
					{
						loanAppGlobals.ShowMessage(loanMessagesDAL.AlreadyExist, loanMessageIcon.Warning);
						hdnModelContractStatus.Value = "show";
						return;
					}
					else if (rsStatus == loanRecordStatus.Success)
					{
						loanAppGlobals.ShowMessage(loanMessagesDAL.InsertSuccess, loanMessageIcon.Success);
						if (((Button)sender).ID.Equals("btnSaveAndNew"))
						{
							hdnModelContractStatus.Value = "clear";
						}
						else
						{
							hdnModelContractStatus.Value = "hide";
						}
						FillContractStatusMaster();
					}
				}
				else
				{
					objContractStatusMasterDAL.ContractStatusMasterId = Convert.ToInt32(hdnContractStatusMasterId.Value);
					loanRecordStatus rsStatus = objContractStatusMasterDAL.UpdateContractStatusMaster();
					if (rsStatus == loanRecordStatus.Error)
					{
						loanAppGlobals.ShowMessage(loanMessagesDAL.UpdateFail, loanMessageIcon.Error);
						return;
					}
					else if (rsStatus == loanRecordStatus.RecordAlreadyExist)
					{
						loanAppGlobals.ShowMessage(loanMessagesDAL.AlreadyExist, loanMessageIcon.Warning);
						hdnModelContractStatus.Value = "show";
						return;
					}
					else if (rsStatus == loanRecordStatus.Success)
					{
						loanAppGlobals.ShowMessage(loanMessagesDAL.UpdateSuccess, loanMessageIcon.Success);
						hdnModelContractStatus.Value = "hide";
						FillContractStatusMaster();
					}
				}
			}
			catch (Exception ex)
			{
				loanAppGlobals.SaveError(ex);
			}
		}

		#region List Methods

		protected void lvContractStatusMaster_ItemDataBound(object sender, ListViewItemEventArgs e)
		{
			try
			{
				if (e.Item.ItemType == ListViewItemType.DataItem)
				{
					loanContractStatusMasterDAL objContractStatusMasterDAL = (loanContractStatusMasterDAL)e.Item.DataItem;

					Literal ltrlContractStatus = (Literal)e.Item.FindControl("ltrlContractStatus");

					ltrlContractStatus.Text = objContractStatusMasterDAL.ContractStatus;
				}
			}
			catch (Exception ex)
			{
				loanAppGlobals.SaveError(ex);
			}
		}

		protected void pgrContractStatusMaster_ItemCommand(object sender, EventArgs e)
		{
			try
			{
				FillContractStatusMaster();
			}
			catch (Exception ex)
			{
				loanAppGlobals.SaveError(ex);
			}
		}

		protected void lvContractStatusMaster_ItemCommand(object sender, ListViewCommandEventArgs e)
		{
			try
			{
				if (e.CommandName.Equals("EditRecord", StringComparison.CurrentCultureIgnoreCase))
				{
					GetContractStatusMaster(Convert.ToInt32(((ListView)sender).DataKeys[e.Item.DataItemIndex].Value));
				}
				else if (e.CommandName.Equals("DeleteRecord", StringComparison.CurrentCultureIgnoreCase))
				{
					loanContractStatusMasterDAL objContractStatusMasterDAL = new loanContractStatusMasterDAL();
					objContractStatusMasterDAL.ContractStatusMasterId = Convert.ToInt32(((ListView)sender).DataKeys[e.Item.DataItemIndex].Value);
					loanRecordStatus rsStatus = objContractStatusMasterDAL.DeleteContractStatusMaster();
					if (rsStatus == loanRecordStatus.Success)
					{
						loanAppGlobals.ShowMessage(loanMessagesDAL.DeleteSuccess, loanMessageIcon.Success);
						FillContractStatusMaster();
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
		private void FillContractStatusMaster()
		{

			loanContractStatusMasterDAL objContractStatusMasterDAL = new loanContractStatusMasterDAL();

			short TotalRecords;
			List<loanContractStatusMasterDAL> lstContractStatusMaster =  objContractStatusMasterDAL.SelectAllContractStatusMasterPageWise(pgrContractStatusMaster.StartRowIndex, pgrContractStatusMaster.PageSize, out TotalRecords);
			pgrContractStatusMaster.TotalRowCount = TotalRecords;

			if (lstContractStatusMaster == null)
			{
				loanAppGlobals.ShowMessage(loanMessagesDAL.SelectAllFail, loanMessageIcon.Error);
				return;
			}

			if (lstContractStatusMaster.Count == 0 && pgrContractStatusMaster.TotalRowCount > 0)
			{
				pgrContractStatusMaster_ItemCommand(pgrContractStatusMaster, new EventArgs());
				return;
			}

			lvContractStatusMaster.DataSource = lstContractStatusMaster;
			lvContractStatusMaster.DataBind();

			if (lstContractStatusMaster.Count > 0)
			{
				int EndiIndex = pgrContractStatusMaster.StartRowIndex + pgrContractStatusMaster.PageSize < pgrContractStatusMaster.TotalRowCount ? pgrContractStatusMaster.StartRowIndex + pgrContractStatusMaster.PageSize : pgrContractStatusMaster.TotalRowCount;
				lblRecords.Text = string.Format(Resources.Messages.ResourceManager.GetString("Records"), pgrContractStatusMaster.StartRowIndex + 1, EndiIndex, pgrContractStatusMaster.TotalRowCount);
				lblRecords.Visible = true;
			}
			else
			{
				lblRecords.Visible = false;
			}

			if (pgrContractStatusMaster.TotalRowCount <= pgrContractStatusMaster.PageSize)
			{
				pgrContractStatusMaster.Visible = false;
			}
			else
			{
				pgrContractStatusMaster.Visible = true;
			}

		}


		private void GetContractStatusMaster(int ContractStatusMasterId)
		{
			loanContractStatusMasterDAL objContractStatusMasterDAL = new loanContractStatusMasterDAL();
			objContractStatusMasterDAL.ContractStatusMasterId = ContractStatusMasterId;
			if (!objContractStatusMasterDAL.SelectContractStatusMaster())
			{
				loanAppGlobals.ShowMessage(loanMessagesDAL.SelectFail, loanMessageIcon.Error);
				return;
			}
			hdnContractStatusMasterId.Value = objContractStatusMasterDAL.ContractStatusMasterId.ToString();
			txtContractStatus.Text = objContractStatusMasterDAL.ContractStatus;

			hdnModelContractStatus.Value = "show";
			hdnActionContractStatus.Value = "edit";
		}
		#endregion


	}
}
