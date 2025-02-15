using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using loanLibrary;

namespace abLOAN
{
    public partial class changepassword : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                loanSessionsDAL.CheckSession();
            }
            catch (Exception ex)
            {
                loanAppGlobals.SaveError(ex);
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            loanUserMasterDAL objUserMasterDAL;
            try
            {
                objUserMasterDAL = new loanUserMasterDAL();
                objUserMasterDAL.Username = ((loanUser)Session[loanSessionsDAL.UserSession]).Username;

                if (objUserMasterDAL.SelectUserMasterByUsername())
                {
                    if (!objUserMasterDAL.Password.Equals(txtOldPassword.Text, StringComparison.InvariantCulture))
                    {
                        loanAppGlobals.ShowMessage("Invalid Old Password, Try again!", loanMessageIcon.Error);
                        return;
                    }
                    else
                    {
                        objUserMasterDAL.Password = txtNewPassword.Text.Trim();
                        objUserMasterDAL.UpdateDateTime = loanGlobalsDAL.GetCurrentDateTime();
                        objUserMasterDAL.SessionId = ((loanUser)Session[loanSessionsDAL.UserSession]).SessionId;

                        loanRecordStatus rstatus = objUserMasterDAL.UpdateUserMasterPassword();
                        if (rstatus == loanRecordStatus.Error)
                        {
                            loanAppGlobals.ShowMessage(loanMessagesDAL.UpdateFail, loanMessageIcon.Error);
                            return;
                        }
                        else if (rstatus == loanRecordStatus.Success)
                        {
                            loanAppGlobals.ShowMessage("Password changed successfully.", loanMessageIcon.Success);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                loanAppGlobals.SaveError(ex);
            }
            finally
            {
                objUserMasterDAL = null;
            }
        }
    }
}