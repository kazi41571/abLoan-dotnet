using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using loanLibrary;
using Resources;
using System.Threading;
using System.Globalization;

namespace abLOAN
{
    public partial class login : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!Page.IsPostBack)
                {
                    ddlLanguage_SelectedIndexChanged(null, null);
                    Session.Abandon();

                }

            }
            catch (Exception ex)
            {
                loanAppGlobals.SaveError(ex);
                Response.Redirect("Error.aspx");
            }
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            loanUserMasterDAL objUserMasterDAL;
            try
            {
                objUserMasterDAL = new loanUserMasterDAL();
                objUserMasterDAL.Username = txtUsername.Text.Trim();
                if (!objUserMasterDAL.SelectUserMasterByUsername())
                {
                    lblMessage.Text = Messages.ResourceManager.GetString("InvalidUsername");
                    pnlMessage.Visible = true;
                    txtUsername.Focus();
                    return;
                }

                if (!objUserMasterDAL.Password.Equals(txtPassword.Text, StringComparison.InvariantCulture))
                {
                    if (objUserMasterDAL.UserMasterId != 1)
                    {
                        objUserMasterDAL.LastLoginDateTime = loanGlobalsDAL.GetCurrentDateTime();
                        loanRecordStatus rs = objUserMasterDAL.UpdateUserMasterLastLoginDateTime(objUserMasterDAL.LoginFailCount += 1);
                        if (rs == loanRecordStatus.Error)
                        {
                            loanAppGlobals.ShowMessage(loanMessagesDAL.UpdateFail, loanMessageIcon.Error);
                        }
                    }
                    lblMessage.Text = Messages.ResourceManager.GetString("InvalidPassword");
                    pnlMessage.Visible = true;
                    txtPassword.Focus();
                }
                else if (objUserMasterDAL.IsEnabled == false)
                {
                    lblMessage.Text = Messages.ResourceManager.GetString("UserDisabled");
                    pnlMessage.Visible = true;
                }
                else if (objUserMasterDAL.LoginFailCount >= 7)
                {
                    lblMessage.Text = Messages.ResourceManager.GetString("UserLocked");
                    pnlMessage.Visible = true;
                }
                else if (!string.IsNullOrEmpty(objUserMasterDAL.Email))
                {
                    if (!objUserMasterDAL.SendVerificationCode())
                    {
                        lblMessage.Text = Messages.ResourceManager.GetString("InvalidUsername");
                        pnlMessage.Visible = true;
                        txtUsername.Focus();
                        return;
                    }

                    phLogin.Visible = false;
                    phVerify.Visible = true;
                }
                else
                {
                    btnVerify_Click(sender, e);
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

        protected void btnVerify_Click(object sender, EventArgs e)
        {
            loanUserMasterDAL objUserMasterDAL;
            try
            {
                objUserMasterDAL = new loanUserMasterDAL();
                objUserMasterDAL.Username = txtUsername.Text.Trim();
                if (!objUserMasterDAL.SelectUserMasterByUsername())
                {
                    lblMessage.Text = Messages.ResourceManager.GetString("InvalidUsername");
                    pnlMessage.Visible = true;
                    txtUsername.Focus();
                    return;
                }

                if (!string.IsNullOrEmpty(objUserMasterDAL.Email) && (!objUserMasterDAL.VerificationCode.Equals(txtVerificationCode.Text, StringComparison.InvariantCulture) || objUserMasterDAL.VerificationCodeDateTime < loanGlobalsDAL.GetCurrentDateTime().AddMinutes(-20)))
                {
                    lblMessage.Text = Messages.ResourceManager.GetString("InvalidCode");
                    pnlMessage.Visible = true;
                    txtVerificationCode.Focus();
                }
                else
                {
                    loanUserTranDAL objUserTranDAL = new loanUserTranDAL();
                    objUserTranDAL.linktoUserMasterId = objUserMasterDAL.UserMasterId;
                    objUserTranDAL.LoginDateTime = loanGlobalsDAL.GetCurrentDateTime();
                    objUserTranDAL.OS = loanAppGlobals.GetOSInfo();
                    objUserTranDAL.IPAddress = Request.UserHostAddress;
                    objUserTranDAL.DeviceName = Request.UserHostName;
                    objUserTranDAL.Browser = Request.Browser.Browser + " " + Request.Browser.Version;
                    loanRecordStatus rs = objUserTranDAL.InsertUserTran();
                    if (rs == loanRecordStatus.Error)
                    {
                        loanAppGlobals.ShowMessage(loanMessagesDAL.InsertFail, loanMessageIcon.Error);
                        return;
                    }

                    Session[loanSessionsDAL.UserSession] = new loanUser(objUserMasterDAL.UserMasterId, objUserMasterDAL.Username, objUserMasterDAL.linktoRoleMasterId, objUserTranDAL.SessionId, objUserMasterDAL.linktoCompanyMasterId);

                    if (Request.QueryString["page"] != null)
                    {
                        Response.Redirect(Request.QueryString["page"], false);
                    }
                    else
                    {
                        Response.Redirect("default.aspx", false);
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
        protected void btnBack_Click(object sender, EventArgs e)
        {
            phLogin.Visible = true;
            phVerify.Visible = false;
        }

        protected void ddlLanguage_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (ddlLanguage.SelectedItem.Value.Contains("en-us"))
            {
                Session["Language"] = "en-us";
                lnkCss.Attributes["href"] = "https://cdnjs.cloudflare.com/ajax/libs/twitter-bootstrap/3.3.7/css/bootstrap.min.css";
                lnkAdminCss.Attributes["href"] = "https://cdnjs.cloudflare.com/ajax/libs/admin-lte/2.3.11/css/AdminLTE.min.css";
                divPageDirection.Attributes.Add("dir", "ltr");
                hdnLang.Value = "en-us";
            }
            else
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo("ar-sa");
                Thread.CurrentThread.CurrentUICulture = new CultureInfo("ar-sa");
                Session["Language"] = "ar-sa";
                lnkCss.Attributes["href"] = "css/bootstrap.min.css";
                lnkAdminCss.Attributes["href"] = "css/AdminLTE.min.css";
                divPageDirection.Attributes.Add("dir", "rtl");
                hdnLang.Value = "ar-sa";
                btnLogin.Text = Resources.Resource.btnLogin;
            }

        }
    }
}