<%@ Page Language="C#" MasterPageFile="Site.Master" AutoEventWireup="true" CodeBehind="user.aspx.cs"
    Inherits="abLOAN.user" %>

<%@ Register Assembly="CustomDataPager" Namespace="CustomDataPager" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequest);

            SetFilter();
        });
        function EndRequest(sender, args) {
            ShowHideDialogUserMaster();
            SetFilter();
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphPage" runat="server">
    <div id="divuser">
        <asp:HiddenField ID="hdnFilter" runat="server" Value="0" ClientIDMode="Static" />
        <div class="blockui">
            <asp:UpdatePanel ID="upuser" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <div class="row">
                        <div style="justify-content: space-between; display: flex;">
                            <div style="padding: 0 15px;">
                                <h4>
                                    <i class="fa fa-th"></i>&nbsp;<%= Resources.Resource.UserPageTitle %>&nbsp; <small>
                                        <asp:Label ID="lblRecords" runat="server" Visible="false"></asp:Label></small>
                                    &nbsp; <a id="aFilter" class="btn btn-default btn-sm" href="" style="text-decoration: none;" onclick="javascript:return ShowHideFilter('divFilter', false);"><%= Resources.Resource.Filter %>&nbsp;&nbsp;<span class="caret"></span></a>
                                </h4>
                            </div>
                            <div style="padding: 0 15px;">
                                <asp:Button ID="btnDownload" runat="server" Text="<%$Resources:Resource, Download %>" CssClass="btn btn-info" OnClick="btnDownload_Click" />
                                &nbsp;
                                <asp:Button ID="btnShowPassword" runat="server" Text="<%$Resources:Resource, btnShowPassword %>" CssClass="btn btn-info btn-outline"
                                    OnClick="btnShowPassword_Click" />
                                <asp:Button ID="btnNew" runat="server" Text="<%$Resources:Resource, btnNew %>" CssClass="btn btn-primary" data-toggle="modal"
                                    data-target="#divuserdetails" OnClientClick="javascript:return false;" />
                                &nbsp;
                            </div>
                        </div>
                    </div>
                    <div id="divFilter" style="display: none;">
                        <div class="panel panel-default">
                            <div class="panel-body">
                                <asp:Panel ID="pnlFilter" runat="server" DefaultButton="btnSearch">
                                    <div class="form-horizontal">
                                        <div class="row">
                                            <div class="col-md-4 col-sm-6">
                                                <div class="form-group">
                                                    <label class="col-sm-5 control-label">
                                                        <%= Resources.Resource.Username %></label>
                                                    <div class="col-sm-7">
                                                        <asp:TextBox ID="txtFilterUsername" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-md-4 col-sm-6">
                                                <div class="form-group">
                                                    <label class="col-sm-5 control-label">
                                                        <%= Resources.Resource.Role %></label>
                                                    <div class="col-sm-7">
                                                        <asp:DropDownList ID="ddlFilterRole" runat="server" CssClass="form-control input-sm">
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-md-4 col-sm-6">
                                                <div class="form-group">
                                                    <label class="col-sm-5 control-label">
                                                        <%= Resources.Resource.IsEnabled %></label>
                                                    <div class="col-sm-7">
                                                        <asp:DropDownList ID="ddlFilterIsEnabled" runat="server" CssClass="form-control input-sm">
                                                            <asp:ListItem Text="<% $Resources:Resource, Yes %>" Value="Yes"></asp:ListItem>
                                                            <asp:ListItem Text="<% $Resources:Resource, No %>" Value="No"></asp:ListItem>
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-md-4 col-sm-6">
                                                <div class="form-group">
                                                    <label class="col-sm-5 control-label"><%= Resources.Resource.Verification %></label>
                                                    <div class="col-sm-7">
                                                        <asp:DropDownList ID="ddlFilterVerification" runat="server" CssClass="form-control input-sm">
                                                            <asp:ListItem Text="- SELECT -" Value=""></asp:ListItem>
                                                            <asp:ListItem Text="<% $Resources:Resource, Yes %>" Value="Yes"></asp:ListItem>
                                                            <asp:ListItem Text="<% $Resources:Resource, No %>" Value="No"></asp:ListItem>
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-md-12 text-center">
                                                <asp:Button ID="btnSearch" runat="server" Text="<% $Resources:Resource, btnSearch %>" CssClass="btn btn-primary"
                                                    ValidationGroup="FilterUserMaster" OnClick="btnSearch_Click" />
                                                &nbsp;
                                                <asp:Button ID="btnReset" runat="server" Text="<% $Resources:Resource, btnReset %>" CssClass="btn btn-default"
                                                    CausesValidation="false" OnClick="btnReset_Click" />
                                            </div>
                                        </div>
                                    </div>
                                </asp:Panel>
                                <asp:ValidationSummary ID="vsUserMaster" runat="server" DisplayMode="BulletList"
                                    ShowMessageBox="true" ShowSummary="false" ValidationGroup="FilterUserMaster" />
                            </div>
                        </div>
                    </div>
                    <asp:ListView ID="lvUserMaster" runat="server" DataKeyNames="UserMasterId" OnItemCommand="lvUserMaster_ItemCommand"
                        OnItemDataBound="lvUserMaster_ItemDataBound">
                        <LayoutTemplate>
                            <div class="panel panel-default">
                                <div class="table-responsive">
                                    <table id="itemPlaceholderContainer" runat="server" class="table table-hover">
                                        <tr>
                                            <th>
                                                <%= Resources.Resource.Username %>
                                            </th>
                                            <th id="thPassword" runat="server" visible="false">
                                                <%= Resources.Resource.Password %>
                                            </th>
                                            <th>
                                                <%= Resources.Resource.Role %>
                                            </th>
                                            <th>
                                                <%= Resources.Resource.LastLoginDateTime %>
                                            </th>
                                            <th style="width: 25px;">
                                                <%= Resources.Resource.VerifiedBy %>
                                            </th>
                                            <th style="width: 25px;">
                                                <%= Resources.Resource.ModifiedBy %>
                                            </th>
                                            <th style="width: 25px;"></th>
                                            <th style="width: 25px;"></th>
                                            <th style="width: 25px;"></th>
                                        </tr>
                                        <tr id="itemPlaceholder" runat="server">
                                        </tr>
                                    </table>
                                </div>
                            </div>
                        </LayoutTemplate>
                        <ItemTemplate>
                            <tr valign="top">
                                <td>
                                    <asp:Literal ID="ltrlUsername" runat="server"></asp:Literal>
                                </td>
                                <td id="tdPassword" runat="server" visible="false">
                                    <asp:Literal ID="ltrlPassword" runat="server"></asp:Literal>
                                </td>
                                <td>
                                    <asp:Literal ID="ltrlRole" runat="server"></asp:Literal>
                                </td>
                                <td>
                                    <asp:Literal ID="ltrlLastLoginDateTime" runat="server"></asp:Literal>
                                </td>
                                <td>
                                    <asp:LinkButton ID="lbtnVerify" runat="server" CommandName="VerifyRecord" CssClass="btn btn-primary btn-sm fa fa-check" title="Verify"></asp:LinkButton>
                                    <asp:Literal ID="ltrlVerifiedBy" runat="server"></asp:Literal>
                                    <asp:Literal ID="ltrlVerifiedDateTime" runat="server"></asp:Literal>
                                </td>
                                <td>
                                    <asp:Literal ID="ltrlModifiedBy" runat="server"></asp:Literal>
                                    <asp:Literal ID="ltrlModifiedDateTime" runat="server"></asp:Literal>
                                </td>
                                <td>
                                    <asp:LinkButton ID="lbtnUnlock" runat="server" CommandName="UnlockRecord" CssClass="btn btn-info btn-sm fa fa-unlock"
                                        title="Unlock" Visible="false"></asp:LinkButton>
                                </td>
                                <td>
                                    <asp:LinkButton ID="lbtnEdit" runat="server" CommandName="EditRecord" CssClass="btn btn-primary btn-sm fa fa-edit"
                                        title="Edit"></asp:LinkButton>
                                </td>
                                <td>
                                    <asp:LinkButton ID="lbtnDelete" runat="server" CommandName="DeleteRecord" OnClientClick="javascript:return ConfirmDelete(this);"
                                        CssClass="btn btn-danger btn-sm fa fa-trash" title="Delete"></asp:LinkButton>
                                </td>
                            </tr>
                        </ItemTemplate>
                        <EmptyDataTemplate>
                            <div class="alert alert-info">
                                <%= Resources.Resource.NoRecordMessage %>
                            </div>
                        </EmptyDataTemplate>
                    </asp:ListView>
                    <div class="row">
                        <div class="col-md-12 text-center">
                            <cc1:DataPager ID="pgrUserMaster" runat="server" OnPagerCommand="pgrUserMaster_ItemCommand"
                                Visible="false" CssClass="pagination" CurrentPageCssClass="active"></cc1:DataPager>
                        </div>
                    </div>
                </ContentTemplate>
                <Triggers>
                    <asp:PostBackTrigger ControlID="btnDownload" />
                    <asp:AsyncPostBackTrigger ControlID="btnSaveAndNew" EventName="Click" />
                    <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
        <div class="modal fade" id="divuserdetails">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="blockui">
                        <asp:UpdatePanel ID="upuserdetails" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Panel ID="pnluserdetails" runat="server" DefaultButton="btnSaveAndNew">
                                    <asp:HiddenField ID="hdnModel" runat="server"></asp:HiddenField>
                                    <asp:HiddenField ID="hdnAction" runat="server"></asp:HiddenField>
                                    <div class="modal-header">
                                        <button type="button" class="close" data-dismiss="modal" aria-hidden="true">
                                            &times;</button>
                                        <h4 class="modal-title"><%= Resources.Resource.UserFormTitle %></h4>
                                    </div>
                                    <div class="modal-body">
                                        <div class="form-horizontal" role="form">
                                            <asp:HiddenField ID="hdnUserMasterId" runat="server" Visible="false"></asp:HiddenField>
                                            <div class="form-group">
                                                <label class="col-sm-4 control-label">
                                                    <%= Resources.Resource.Username %></label>
                                                <div class="col-sm-7">
                                                    <%-- start - for firefox and chrome disable autofill --%>
                                                    <input name="fakeUsername" style="display: none;" />
                                                    <input name="fakePassword" style="display: none;" type="password" />
                                                    <input name="fakeEmail" style="display: none;" />
                                                    <%-- end --%>
                                                    <asp:TextBox ID="txtUsername" runat="server" CssClass="form-control input-sm" MaxLength="50"></asp:TextBox>
                                                    <div class="text-danger">
                                                        <asp:RequiredFieldValidator ID="rfvUsername" runat="server"
                                                            ControlToValidate="txtUsername" Display="Dynamic" SetFocusOnError="true"
                                                            ValidationGroup="userdetails">
                                                            <%= Resources.Messages.InputRequired %> <%=Resources.Resource.Username %>
                                                        </asp:RequiredFieldValidator>
                                                        <asp:RegularExpressionValidator ID="revUsername" runat="server"
                                                            ControlToValidate="txtUsername" Display="Dynamic" SetFocusOnError="true"
                                                            ValidationGroup="userdetails" ValidationExpression="^[a-zA-Z0-9_@.]*$">
                                                            <%= Resources.Messages.InputInvalid %> <%=Resources.Resource.Username %>
                                                        </asp:RegularExpressionValidator>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <label class="col-sm-4 control-label">
                                                    <%= Resources.Resource.Password %></label>
                                                <div class="col-sm-7">
                                                    <asp:TextBox ID="txtPassword" runat="server" CssClass="form-control input-sm" MaxLength="50"
                                                        TextMode="Password" autocomplete="new-password"></asp:TextBox>
                                                    <%-- for chrome autocomplete="new-password" --%>
                                                    <div class="text-danger">
                                                        <asp:RequiredFieldValidator ID="rfvPassword" runat="server"
                                                            ControlToValidate="txtPassword" Display="Dynamic" SetFocusOnError="true"
                                                            ValidationGroup="userdetails">
                                                            <%= Resources.Messages.InputRequired %> <%=Resources.Resource.Password %>
                                                        </asp:RequiredFieldValidator>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <label class="col-sm-4 control-label">
                                                    <%= Resources.Resource.Email %></label>
                                                <div class="col-sm-7">
                                                    <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control input-sm" MaxLength="100"></asp:TextBox>
                                                    <div class="text-danger">
                                                        <%--<asp:RequiredFieldValidator ID="rfvEmail" runat="server"
                                                            ControlToValidate="txtEmail" Display="Dynamic" SetFocusOnError="true"
                                                            ValidationGroup="userdetails">
                                                            <%= Resources.Messages.InputRequired %> <%=Resources.Resource.Email %>
                                                        </asp:RequiredFieldValidator>--%>
                                                        <asp:RegularExpressionValidator ID="revEmail" runat="server"
                                                            ControlToValidate="txtEmail" Display="Dynamic" SetFocusOnError="true"
                                                            ValidationGroup="userdetails" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*">
                                                            <%= Resources.Messages.InputInvalid %> <%=Resources.Resource.Email %>
                                                        </asp:RegularExpressionValidator>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <label class="col-sm-4 control-label">
                                                    <%= Resources.Resource.Role %></label>
                                                <div class="col-sm-7">
                                                    <asp:DropDownList ID="ddlRole" runat="server" CssClass="form-control input-sm">
                                                    </asp:DropDownList>
                                                    <div class="text-danger">
                                                        <asp:RequiredFieldValidator ID="rfvRole" runat="server"
                                                            ControlToValidate="ddlRole" Display="Dynamic" SetFocusOnError="true"
                                                            InitialValue="" ValidationGroup="userdetails">
                                                            <%= Resources.Messages.InputRequired %> <%=Resources.Resource.Role %>
                                                        </asp:RequiredFieldValidator>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <label class="col-sm-4 control-label">
                                                    <%= Resources.Resource.Comment %></label>
                                                <div class="col-sm-7">
                                                    <asp:TextBox ID="txtComment" runat="server" CssClass="form-control input-sm" MaxLength="100"></asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <div class="col-sm-offset-4 col-sm-7">
                                                    <div class="checkbox">
                                                        <label>
                                                            <asp:CheckBox ID="chkIsEnabled" runat="server" Checked="true" Text="<% $Resources:Resource, IsEnabled %>"></asp:CheckBox>
                                                        </label>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="modal-footer">
                                        <asp:Button ID="btnSaveAndNew" runat="server" Text="<% $Resources:Resource, btnSaveAndNew %>" CssClass="btn btn-primary" OnClick="btnSave_Click" ValidationGroup="userdetails" />
                                        <asp:Button ID="btnSave" runat="server" Text="<% $Resources:Resource, btnSave %>" CssClass="btn btn-primary" OnClick="btnSave_Click" ValidationGroup="userdetails" />
                                        <asp:Button ID="btnClose" runat="server" Text="<% $Resources:Resource, btnClose %>" CssClass="btn btn-default" CausesValidation="false" data-dismiss="modal" />
                                    </div>
                                </asp:Panel>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="lvUserMaster" EventName="ItemCommand" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </div>
        </div>
        <asp:ValidationSummary ID="vsuserdetails" runat="server" DisplayMode="BulletList"
            ShowMessageBox="true" ShowSummary="false" ValidationGroup="userdetails" />
    </div>
    <script type="text/javascript">
        $(document).ready(function () {
            if ($("#" + Prefix + "hdnModel").val() == "") {
                ClearUserMaster();
            }
            SetDialogShowHideUserMaster();
            ShowHideDialogUserMaster();
        });

        function SetDialogShowHideUserMaster() {
            $("#divuserdetails").on("hidden.bs.modal", function () {
                ClearUserMaster();
            })
            $("#divuserdetails").on("show.bs.modal", function () {
                SetUserMaster();
            })
            $("#divuserdetails").on("shown.bs.modal", function () {
                $("#" + Prefix + "txtUsername").focus();
            })
        }
        function ShowHideDialogUserMaster() {
            try {
                SetUserMaster();
                if ($("#" + Prefix + "hdnModel").val() == "show") {
                    $("#divuserdetails").modal("show");
                }
                else if ($("#" + Prefix + "hdnModel").val() == "hide") {
                    ClearUserMaster();
                    $("#divuserdetails").modal("hide");
                }
                else if ($("#" + Prefix + "hdnModel").val() == "clear") {
                    ClearUserMaster();
                    $("#divuserdetails").modal("show");
                    $("#" + Prefix + "txtUsername").focus();
                }
                $("#" + Prefix + "hdnModel").val("");
                return false;
            }
            catch (ex) {
                alert(ex);
                return false;
            }
        }
        function ClearUserMaster() {
            for (i = 0; i < Page_Validators.length; i++) {
                Page_Validators[i].style.display = "none";
            }
            $("#" + Prefix + "hdnAction").val("");
            $("#" + Prefix + "txtUsername").val("");
            $("#" + Prefix + "txtPassword").val("");
            $("#" + Prefix + "txtEmail").val("");
            $("#" + Prefix + "ddlRole").prop("disabled", false);
            $("#" + Prefix + "ddlRole").prop("selectedIndex", 0);
            $("#" + Prefix + "txtComment").val("");
            $("#" + Prefix + "chkIsEnabled").prop("disabled", false);
            $("#" + Prefix + "chkIsEnabled").prop("checked", true);

        }
        function SetUserMaster() {
            for (i = 0; i < Page_Validators.length; i++) {
                Page_Validators[i].errormessage = Page_Validators[i].innerHTML;
            }
            if ($("#" + Prefix + "hdnAction").val() == "") {
                $("#" + Prefix + "btnSaveAndNew").css("visibility", "visible");
                $("#" + Prefix + "btnSave").val('<%= GetGlobalResourceObject("Resource", "btnSave") %>');
            }
            else {
                $("#" + Prefix + "btnSaveAndNew").css("visibility", "hidden");
                $("#" + Prefix + "btnSave").val('<%= GetGlobalResourceObject("Resource", "btnUpdate") %>');
            }
        }
    </script>
</asp:Content>
