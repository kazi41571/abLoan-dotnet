using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using loanLibrary;
using System.Text;
using System.Data;
using System.Web.Configuration;
using System.Globalization;
using System.Threading;

namespace abLOAN
{
    public partial class Site : System.Web.UI.MasterPage
    {
        protected void Page_PreRender(object sender, EventArgs e)
        {
            try
            {
                string Language = Convert.ToString(Session["Language"]);
                if (Language.Contains("ar-sa"))
                {
                    Thread.CurrentThread.CurrentCulture = new CultureInfo("ar-sa");
                    Thread.CurrentThread.CurrentUICulture = new CultureInfo("ar-sa");
                }
                else
                {
                    Thread.CurrentThread.CurrentCulture = new CultureInfo("en-us");
                    Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-us");
                }

                if (Session[loanSessionsDAL.MessageSession] != null)
                {
                    pnlMessage.Visible = true;
                    Hashtable htMessage = (Hashtable)Session[loanSessionsDAL.MessageSession];
                    lblMessage.Text = htMessage["Message"].ToString();
                    switch ((loanMessageIcon)htMessage["MessageIcon"])
                    {
                        case loanMessageIcon.Information:
                            pnlMessage.CssClass = "alert alert-info alert-dismissable";
                            lblIcon.CssClass = "fa fa-info-circle";
                            break;
                        case loanMessageIcon.Warning:
                            pnlMessage.CssClass = "alert alert-warning alert-dismissable";
                            lblIcon.CssClass = "fa fa-exclamation-circle";
                            break;
                        case loanMessageIcon.Error:
                            pnlMessage.CssClass = "alert alert-danger alert-dismissable";
                            lblIcon.CssClass = "fa fa-times-circle";
                            break;
                        case loanMessageIcon.Success:
                            pnlMessage.CssClass = "alert alert-success alert-dismissable";
                            lblIcon.CssClass = "fa fa-check-circle";
                            break;
                    }
                    Session[loanSessionsDAL.MessageSession] = null;
                }
                else
                {
                    pnlMessage.Visible = false;
                }
            }
            catch (Exception ex)
            {
                loanAppGlobals.SaveError(ex);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Response.Cache.SetExpires(DateTime.Now.AddSeconds(-1));
                Response.Cache.SetNoStore();

                if (Session[loanSessionsDAL.UserSession] == null)
                {
                    Response.Redirect("login.aspx");
                }
                else
                {
                    if (Session["Language"] != null && Session["Language"].ToString() == "ar-sa")
                    {
                        lnkCss.Attributes["href"] = "css/bootstrap.min.css";
                        lnkAdminCss.Attributes["href"] = "css/AdminLTE.min.css";
                        //lnkStyleCss.Attributes["href"] = "css/style-ar.css";
                        divPageDirection.Attributes.Add("dir", "rtl");
                        hdnLang.Value = "ar-sa";
                    }
                    else
                    {
                        lnkCss.Attributes["href"] = "https://cdnjs.cloudflare.com/ajax/libs/twitter-bootstrap/3.3.7/css/bootstrap.min.css";
                        lnkAdminCss.Attributes["href"] = "https://cdnjs.cloudflare.com/ajax/libs/admin-lte/2.3.11/css/AdminLTE.min.css";
                        divPageDirection.Attributes.Add("dir", "ltr");
                        //lnkStyleCss.Attributes["href"] = "css/style.css?20171026";
                        hdnLang.Value = "en-us";
                    }

                    if (!IsPostBack)
                    {
                        ddlLanguage.SelectedValue = hdnLang.Value;

                        loanUser objUser = (loanUser)Session[loanSessionsDAL.UserSession];
                        lblUsernameTop.Text = lblUsername.Text = objUser.Username;
                        if (!string.IsNullOrEmpty(objUser.Username))
                        {
                            lblUsernameTop.Text = lblUsername.Text = objUser.Username;
                        }

                        FillMenu();
                    }
                }
            }
            catch (Exception ex)
            {
                loanAppGlobals.SaveError(ex);
            }

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
                Session["Language"] = "ar-sa";
                lnkCss.Attributes["href"] = "css/bootstrap.min.css";
                lnkAdminCss.Attributes["href"] = "css/AdminLTE.min.css";
                divPageDirection.Attributes.Add("dir", "rtl");
                hdnLang.Value = "ar-sa";
            }

            FillMenu();
            Response.Redirect(Request.RawUrl);
        }

        #region Private Methods
        private void FillMenu()
        {
            DataSet dsMenu = new DataSet();
            if (Session["Language"] != null && Session["Language"].ToString() == "ar-sa")
            {
                dsMenu.ReadXml(Server.MapPath("Web-ar.sitemap"));
            }
            else
            {
                dsMenu.ReadXml(Server.MapPath("Web.sitemap"));
            }

            dsMenu.Tables["siteMapNode"].DefaultView.RowFilter = "siteMapNode_Id_0 = 0";
            DataTable dtMenu = dsMenu.Tables["siteMapNode"].DefaultView.ToTable();
            DataTable dtChild;
            DataTable dtChild1;

            StringBuilder sbMenu = new StringBuilder();
            StringBuilder sbTemp = null;
            bool IsAdd = true;
            bool IsActive = false;
            bool IsChildActive = false;
            foreach (DataRow drMenu in dtMenu.Rows)
            {
                dsMenu.Tables["siteMapNode"].DefaultView.RowFilter = "siteMapNode_Id_0 = " + drMenu["siteMapNode_Id"];
                dtChild = dsMenu.Tables["siteMapNode"].DefaultView.ToTable();

                IsAdd = true;
                sbTemp = new StringBuilder();

                if (dtChild.Rows.Count > 0)
                {
                    sbTemp.Append("<li class=\"[~CLASS~]\">");

                    sbTemp.Append("<a href=\"" + Page.ResolveClientUrl(drMenu["url"].ToString()) + "\">");
                    if (drMenu["icon"] != DBNull.Value)
                    {
                        sbTemp.Append("<i class=\"" + drMenu["icon"].ToString() + "\"></i> <span>" + drMenu["title"].ToString() + "</span>");
                        sbTemp.Append("<span class=\"pull-right-container\"><i class=\"fa fa-angle-left pull-right\"></i></span>");
                        sbTemp.Append("</a>");
                    }
                    else
                    {
                        sbTemp.Append("<i class=\"fa fa-link\"></i> <span>" + drMenu["title"].ToString() + "</span>");
                        sbTemp.Append("<span class=\"pull-right-container\"><i class=\"fa fa-angle-left pull-right\"></i></span>");
                        sbTemp.Append("</a>");
                    }
                    sbTemp.Append("<ul class=\"treeview-menu\">");

                    IsAdd = false;
                    IsActive = false;
                    foreach (DataRow drChild in dtChild.Rows)
                    {
                        IsAdd = true;

                        dsMenu.Tables["siteMapNode"].DefaultView.RowFilter = "siteMapNode_Id_0 = " + drChild["siteMapNode_Id"];
                        dtChild1 = dsMenu.Tables["siteMapNode"].DefaultView.ToTable();

                        if (dtChild1.Rows.Count == 0)
                        {
                            sbTemp.Append("<li");
                            if (Request.QueryString["Page"] != null && drChild["url"].ToString().Contains(Request.QueryString["Page"]))
                            {
                                sbTemp.Append(" class=\"active\" ");
                                IsActive = true;
                            }
                            if (Request.Url.PathAndQuery.Contains("/" + drChild["url"].ToString()))
                            {
                                sbTemp.Append(" class=\"active\" ");
                                IsActive = true;
                            }
                            sbTemp.Append(">");
                            sbTemp.Append("<a href=\"" + Page.ResolveClientUrl(drChild["url"].ToString()) + "\">" + drChild["title"].ToString() + "</a>");
                            sbTemp.Append("</li>");
                        }
                        else
                        {
                            sbTemp.Append("<li class=\"[~CHILDCLASS~]\">");

                            sbTemp.Append("<a href=\"" + Page.ResolveClientUrl(drChild["url"].ToString()) + "\">");
                            if (drMenu["icon"] != DBNull.Value)
                            {
                                sbTemp.Append("<i class=\"" + drChild["icon"].ToString() + "\"></i> <span>" + drChild["title"].ToString() + "</span>");
                                sbTemp.Append("<span class=\"pull-right-container\"><i class=\"fa fa-angle-left pull-right\"></i></span>");
                                sbTemp.Append("</a>");
                            }
                            else
                            {
                                sbTemp.Append("<i class=\"fa fa-link\"></i> <span>" + drChild["title"].ToString() + "</span>");
                                sbTemp.Append("<span class=\"pull-right-container\"><i class=\"fa fa-angle-left pull-right\"></i></span>");
                                sbTemp.Append("</a>");
                            }
                            sbTemp.Append("<ul class=\"treeview-menu\">");

                            IsChildActive = false;
                            foreach (DataRow drChild1 in dtChild1.Rows)
                            {
                                sbTemp.Append("<li");
                                if (Request.QueryString["Page"] != null && drChild1["url"].ToString().Contains(Request.QueryString["Page"]))
                                {
                                    sbTemp.Append(" class=\"active\" ");
                                    IsChildActive = true;
                                }
                                else if (Request.Url.PathAndQuery.Contains("/" + drChild1["url"].ToString()))
                                {
                                    sbTemp.Append(" class=\"active\" ");
                                    IsChildActive = true;
                                }
                                sbTemp.Append(">");
                                sbTemp.Append("<a href=\"" + Page.ResolveClientUrl(drChild1["url"].ToString()) + "\">" + drChild1["title"].ToString() + "</a>");
                                sbTemp.Append("</li>");
                            }

                            sbTemp.Append("</ul>");
                        }
                    }

                    sbTemp.Append("</ul>");
                }
                else
                {
                    sbTemp.Append("<li");
                    if (Request.QueryString["Page"] != null && drMenu["url"].ToString().Contains(Request.QueryString["Page"]))
                    {
                        sbTemp.Append(" class=\"active\" ");
                    }
                    else if (Request.Url.AbsolutePath.Contains("/" + drMenu["url"].ToString()))
                    {
                        sbTemp.Append(" class=\"active\" ");
                    }
                    sbTemp.Append(">");
                    sbTemp.Append("<a href=\"" + Page.ResolveClientUrl(drMenu["url"].ToString()) + "\">");
                    if (drMenu["icon"] != DBNull.Value)
                    {
                        sbTemp.Append("<i class=\"" + drMenu["icon"].ToString() + "\"></i> <span>" + drMenu["title"].ToString() + "</span></a>");
                    }
                    else
                    {
                        sbTemp.Append("<i class=\"fa fa-link\"></i> <span>" + drMenu["title"].ToString() + "</span></a>");
                    }
                }

                if (IsAdd)
                {
                    if (IsActive || IsChildActive)
                    {
                        sbTemp.Replace("[~CLASS~]", "treeview active");
                    }
                    else
                    {
                        sbTemp.Replace("[~CLASS~]", "treeview");
                    }
                    if (IsChildActive)
                    {
                        sbTemp.Replace("[~CHILDCLASS~]", "active");
                        IsChildActive = false;
                    }
                    else
                    {
                        sbTemp.Replace("[~CHILDCLASS~]", "");
                    }
                    sbTemp.Append("</li>");
                    sbMenu.Append(sbTemp);
                }
            }

            ltrlMenu.Text = sbMenu.ToString();
        }
        #endregion
    }
}