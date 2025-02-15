using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using loanLibrary;

namespace abLOAN
{
	public partial class installmentstatus : BasePage
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			try
			{
				loanSessionsDAL.CheckSession();

				if (!Page.IsPostBack)
				{
					//loanUser.CheckUserRights(loanUserRights.ViewInstallmentStatus);

					//loanSessionsDAL.RemoveSessionAllKeyValue();

					FillInstallmentStatusMaster();
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
				loanInstallmentStatusMasterDAL objInstallmentStatusMasterDAL = new loanInstallmentStatusMasterDAL();
				objInstallmentStatusMasterDAL.InstallmentStatus = txtInstallmentStatus.Text.Trim();
                objInstallmentStatusMasterDAL.UpdateDateTime = loanGlobalsDAL.GetCurrentDateTime();
                objInstallmentStatusMasterDAL.SessionId = ((loanUser)Session[loanSessionsDAL.UserSession]).SessionId;

                if (string.IsNullOrEmpty(hdnActionInstallmentStatus.Value))
				{
					loanRecordStatus rsStatus = objInstallmentStatusMasterDAL.InsertInstallmentStatusMaster();
					if (rsStatus == loanRecordStatus.Error)
					{
						loanAppGlobals.ShowMessage(loanMessagesDAL.InsertFail, loanMessageIcon.Error);
						return;
					}
					else if (rsStatus == loanRecordStatus.RecordAlreadyExist)
					{
						loanAppGlobals.ShowMessage(loanMessagesDAL.AlreadyExist, loanMessageIcon.Warning);
						hdnModelInstallmentStatus.Value = "show";
						return;
					}
					else if (rsStatus == loanRecordStatus.Success)
					{
						loanAppGlobals.ShowMessage(loanMessagesDAL.InsertSuccess, loanMessageIcon.Success);
						if (((Button)sender).ID.Equals("btnSaveAndNew"))
						{
							hdnModelInstallmentStatus.Value = "clear";
						}
						else
						{
							hdnModelInstallmentStatus.Value = "hide";
						}
						FillInstallmentStatusMaster();
					}
				}
				else
				{
					objInstallmentStatusMasterDAL.UpdateDateTime = loanGlobalsDAL.GetCurrentDateTime();
					objInstallmentStatusMasterDAL.InstallmentStatusMasterId = Convert.ToInt32(hdnInstallmentStatusMasterId.Value);
					loanRecordStatus rsStatus = objInstallmentStatusMasterDAL.UpdateInstallmentStatusMaster();
					if (rsStatus == loanRecordStatus.Error)
					{
						loanAppGlobals.ShowMessage(loanMessagesDAL.UpdateFail, loanMessageIcon.Error);
						return;
					}
					else if (rsStatus == loanRecordStatus.RecordAlreadyExist)
					{
						loanAppGlobals.ShowMessage(loanMessagesDAL.AlreadyExist, loanMessageIcon.Warning);
						hdnModelInstallmentStatus.Value = "show";
						return;
					}
					else if (rsStatus == loanRecordStatus.Success)
					{
						loanAppGlobals.ShowMessage(loanMessagesDAL.UpdateSuccess, loanMessageIcon.Success);
						hdnModelInstallmentStatus.Value = "hide";
						FillInstallmentStatusMaster();
					}
				}
			}
			catch (Exception ex)
			{
				loanAppGlobals.SaveError(ex);
			}
		}

		#region List Methods

		protected void lvInstallmentStatusMaster_ItemDataBound(object sender, ListViewItemEventArgs e)
		{
			try
			{
				if (e.Item.ItemType == ListViewItemType.DataItem)
				{
					loanInstallmentStatusMasterDAL objInstallmentStatusMasterDAL = (loanInstallmentStatusMasterDAL)e.Item.DataItem;

					Literal ltrlInstallmentStatus = (Literal)e.Item.FindControl("ltrlInstallmentStatus");

					ltrlInstallmentStatus.Text = objInstallmentStatusMasterDAL.InstallmentStatus;
				}
			}
			catch (Exception ex)
			{
				loanAppGlobals.SaveError(ex);
			}
		}

		protected void pgrInstallmentStatusMaster_ItemCommand(object sender, EventArgs e)
		{
			try
			{
				FillInstallmentStatusMaster();
			}
			catch (Exception ex)
			{
				loanAppGlobals.SaveError(ex);
			}
		}

		protected void lvInstallmentStatusMaster_ItemCommand(object sender, ListViewCommandEventArgs e)
		{
			try
			{
				if (e.CommandName.Equals("EditRecord", StringComparison.CurrentCultureIgnoreCase))
				{
					GetInstallmentStatusMaster(Convert.ToInt32(((ListView)sender).DataKeys[e.Item.DataItemIndex].Value));
				}
				else if (e.CommandName.Equals("DeleteRecord", StringComparison.CurrentCultureIgnoreCase))
				{
					loanInstallmentStatusMasterDAL objInstallmentStatusMasterDAL = new loanInstallmentStatusMasterDAL();
					objInstallmentStatusMasterDAL.InstallmentStatusMasterId = Convert.ToInt32(((ListView)sender).DataKeys[e.Item.DataItemIndex].Value);
					loanRecordStatus rsStatus = objInstallmentStatusMasterDAL.DeleteInstallmentStatusMaster();
					if (rsStatus == loanRecordStatus.Success)
					{
						loanAppGlobals.ShowMessage(loanMessagesDAL.DeleteSuccess, loanMessageIcon.Success);
						FillInstallmentStatusMaster();
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
		private void FillInstallmentStatusMaster()
		{

			loanInstallmentStatusMasterDAL objInstallmentStatusMasterDAL = new loanInstallmentStatusMasterDAL();

			short TotalRecords;
			List<loanInstallmentStatusMasterDAL> lstInstallmentStatusMaster =  objInstallmentStatusMasterDAL.SelectAllInstallmentStatusMasterPageWise(pgrInstallmentStatusMaster.StartRowIndex, pgrInstallmentStatusMaster.PageSize, out TotalRecords);
			pgrInstallmentStatusMaster.TotalRowCount = TotalRecords;

			if (lstInstallmentStatusMaster == null)
			{
				loanAppGlobals.ShowMessage(loanMessagesDAL.SelectAllFail, loanMessageIcon.Error);
				return;
			}

			if (lstInstallmentStatusMaster.Count == 0 && pgrInstallmentStatusMaster.TotalRowCount > 0)
			{
				pgrInstallmentStatusMaster_ItemCommand(pgrInstallmentStatusMaster, new EventArgs());
				return;
			}

			lvInstallmentStatusMaster.DataSource = lstInstallmentStatusMaster;
			lvInstallmentStatusMaster.DataBind();

			if (lstInstallmentStatusMaster.Count > 0)
			{
				int EndiIndex = pgrInstallmentStatusMaster.StartRowIndex + pgrInstallmentStatusMaster.PageSize < pgrInstallmentStatusMaster.TotalRowCount ? pgrInstallmentStatusMaster.StartRowIndex + pgrInstallmentStatusMaster.PageSize : pgrInstallmentStatusMaster.TotalRowCount;
                lblRecords.Text = string.Format(Resources.Messages.ResourceManager.GetString("Records"), pgrInstallmentStatusMaster.StartRowIndex + 1, EndiIndex, pgrInstallmentStatusMaster.TotalRowCount);
                lblRecords.Visible = true;
			}
			else
			{
				lblRecords.Visible = false;
			}

			if (pgrInstallmentStatusMaster.TotalRowCount <= pgrInstallmentStatusMaster.PageSize)
			{
				pgrInstallmentStatusMaster.Visible = false;
			}
			else
			{
				pgrInstallmentStatusMaster.Visible = true;
			}

		}


		private void GetInstallmentStatusMaster(int InstallmentStatusMasterId)
		{
			loanInstallmentStatusMasterDAL objInstallmentStatusMasterDAL = new loanInstallmentStatusMasterDAL();
			objInstallmentStatusMasterDAL.InstallmentStatusMasterId = InstallmentStatusMasterId;
			if (!objInstallmentStatusMasterDAL.SelectInstallmentStatusMaster())
			{
				loanAppGlobals.ShowMessage(loanMessagesDAL.SelectFail, loanMessageIcon.Error);
				return;
			}
			hdnInstallmentStatusMasterId.Value = objInstallmentStatusMasterDAL.InstallmentStatusMasterId.ToString();
			txtInstallmentStatus.Text = objInstallmentStatusMasterDAL.InstallmentStatus;

			hdnModelInstallmentStatus.Value = "show";
			hdnActionInstallmentStatus.Value = "edit";
		}
		#endregion


	}
}
