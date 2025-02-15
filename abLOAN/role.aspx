<%@ Page Language="C#" MasterPageFile="Site.Master" AutoEventWireup="true" CodeBehind="role.aspx.cs"
    Inherits="abLOAN.role" %>

<%@ Register Assembly="CustomDataPager" Namespace="CustomDataPager" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequest);

            SetFilter();
        });
        function EndRequest(sender, args) {
            ShowHideDialogRoleMaster();
            SetFilter();
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphPage" runat="server">
    <div id="divrole">
        <asp:HiddenField ID="hdnFilter" runat="server" Value="0" ClientIDMode="Static" />
        <div class="blockui">
            <asp:UpdatePanel ID="uprole" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <div class="row">
                        <div style="justify-content: space-between; display: flex;">
                            <div style="padding: 0 15px;">
                                <h4>
                                    <i class="fa fa-th"></i>&nbsp;
                                     <%= Resources.Resource.RolePageTitle %>
                                    &nbsp; <small>
                                        <asp:Label ID="lblRecords" runat="server" Visible="False"></asp:Label></small>
                                    &nbsp; <a id="aFilter" class="btn btn-default btn-sm" href="" style="text-decoration: none;" onclick="javascript:return ShowHideFilter('divFilter', false);"><%= Resources.Resource.Filter %>&nbsp;&nbsp;<span class="caret"></span></a>
                                </h4>
                            </div>
                            <div style="padding: 0 15px;">
                                <asp:Button ID="btnNew" runat="server" Text="<%$Resources:Resource, btnNew %>" CssClass="btn btn-primary" data-toggle="modal"
                                    data-target="#divroledetails" OnClientClick="javascript:return false;" />

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
                                                        <%= Resources.Resource.Role %></label>
                                                    <div class="col-sm-7">
                                                        <asp:TextBox ID="txtFilterRole" runat="server" CssClass="form-control input-sm"></asp:TextBox>
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
                                        </div>
                                        <div class="row">
                                            <div class="col-md-12 text-center">
                                                <asp:Button ID="btnSearch" runat="server" Text="<% $Resources:Resource, btnSearch %>" CssClass="btn btn-primary"
                                                    ValidationGroup="FilterRoleMaster" OnClick="btnSearch_Click" />
                                                &nbsp;
                                                <asp:Button ID="btnReset" runat="server" Text="<% $Resources:Resource, btnReset %>" CssClass="btn btn-default"
                                                    CausesValidation="false" OnClick="btnReset_Click" />
                                            </div>
                                        </div>
                                    </div>
                                </asp:Panel>
                                <asp:ValidationSummary ID="vsRoleMaster" runat="server" DisplayMode="BulletList"
                                    ShowMessageBox="true" ShowSummary="false" ValidationGroup="FilterRoleMaster" />
                            </div>
                        </div>
                    </div>
                    <asp:ListView ID="lvRoleMaster" runat="server" DataKeyNames="RoleMasterId" OnItemCommand="lvRoleMaster_ItemCommand"
                        OnItemDataBound="lvRoleMaster_ItemDataBound">
                        <LayoutTemplate>
                            <div class="panel panel-default">
                                <div class="table-responsive">
                                    <table id="itemPlaceholderContainer" runat="server" class="table table-hover">
                                        <tr>
                                            <th><%= Resources.Resource.Role %>
                                            </th>
                                            <th><%= Resources.Resource.Description %>
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
                                    <asp:Literal ID="ltrlRole" runat="server"></asp:Literal>
                                </td>
                                <td>
                                    <asp:Literal ID="ltrlDescription" runat="server"></asp:Literal>
                                </td>
                                <td>
                                    <asp:LinkButton ID="lbtnRights" runat="server" CommandName="EditRoleRights" CssClass="btn btn-info btn-sm fa fa-key"
                                        title="Role Rights"></asp:LinkButton>
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
                            <cc1:DataPager ID="pgrRoleMaster" runat="server" OnPagerCommand="pgrRoleMaster_ItemCommand"
                                Visible="false" CssClass="pagination" CurrentPageCssClass="active"></cc1:DataPager>
                        </div>
                    </div>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnSaveAndNew" EventName="Click" />
                    <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
        <div class="modal fade" id="divroledetails">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="blockui">
                        <asp:UpdatePanel ID="uproledetails" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Panel ID="pnlroledetails" runat="server" DefaultButton="btnSaveAndNew">
                                    <asp:HiddenField ID="hdnModel" runat="server"></asp:HiddenField>
                                    <asp:HiddenField ID="hdnAction" runat="server"></asp:HiddenField>
                                    <div class="modal-header">
                                        <button type="button" class="close" data-dismiss="modal" aria-hidden="true">
                                            &times;</button>
                                        <h4 class="modal-title"><%= Resources.Resource.RoleFormTitle %></h4>
                                    </div>
                                    <div class="modal-body">
                                        <div class="form-horizontal" role="form">
                                            <asp:HiddenField ID="hdnRoleMasterId" runat="server" Visible="false"></asp:HiddenField>
                                            <div class="form-group">
                                                <label class="col-sm-4 control-label">
                                                    <%= Resources.Resource.Role %></label>
                                                <div class="col-sm-7">
                                                    <asp:TextBox ID="txtRole" runat="server" CssClass="form-control input-sm" MaxLength="50"></asp:TextBox>
                                                    <div class="text-danger">
                                                        <asp:RequiredFieldValidator ID="rfvRole" runat="server"
                                                            ControlToValidate="txtRole" Display="Dynamic" SetFocusOnError="true"
                                                            ValidationGroup="roledetails">
                                                            <%= Resources.Messages.InputRequired %> <%=Resources.Resource.Role %>
                                                        </asp:RequiredFieldValidator>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <label class="col-sm-4 control-label">
                                                    <%= Resources.Resource.Description %></label>
                                                <div class="col-sm-7">
                                                    <asp:TextBox ID="txtDescription" runat="server" CssClass="form-control input-sm"
                                                        MaxLength="100"></asp:TextBox>
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
                                        <asp:Button ID="btnSaveAndNew" runat="server" Text="<% $Resources:Resource, btnSaveAndNew %>" CssClass="btn btn-primary" OnClick="btnSave_Click" ValidationGroup="roledetails" />
                                        <asp:Button ID="btnSave" runat="server" Text="<% $Resources:Resource, btnSave %>" CssClass="btn btn-primary" OnClick="btnSave_Click" ValidationGroup="roledetails" />
                                        <asp:Button ID="btnClose" runat="server" Text="<% $Resources:Resource, btnClose %>" CssClass="btn btn-default" CausesValidation="false" data-dismiss="modal" />
                                    </div>
                                </asp:Panel>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="lvRoleMaster" EventName="ItemCommand" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </div>
        </div>
        <asp:ValidationSummary ID="vsroledetails" runat="server" DisplayMode="BulletList"
            ShowMessageBox="true" ShowSummary="false" ValidationGroup="roledetails" />
    </div>
    <script type="text/javascript">
        $(document).ready(function () {
            if ($("#" + Prefix + "hdnModel").val() == "") {
                ClearRoleMaster();
            }
            SetDialogShowHideRoleMaster();
            ShowHideDialogRoleMaster();
        });

        function SetDialogShowHideRoleMaster() {
            $("#divroledetails").on("hidden.bs.modal", function () {
                ClearRoleMaster();
            })
            $("#divroledetails").on("show.bs.modal", function () {
                SetRoleMaster();
            })
            $("#divroledetails").on("shown.bs.modal", function () {
                $("#" + Prefix + "txtRole").focus();
            })
        }
        function ShowHideDialogRoleMaster() {
            try {
                SetRoleMaster();
                if ($("#" + Prefix + "hdnModel").val() == "show") {
                    $("#divroledetails").modal("show");
                }
                else if ($("#" + Prefix + "hdnModel").val() == "hide") {
                    ClearRoleMaster();
                    $("#divroledetails").modal("hide");
                }
                else if ($("#" + Prefix + "hdnModel").val() == "clear") {
                    ClearRoleMaster();
                    $("#divroledetails").modal("show");
                    $("#" + Prefix + "txtRole").focus();
                }
                $("#" + Prefix + "hdnModel").val("");
                return false;
            }
            catch (ex) {
                alert(ex);
                return false;
            }
        }
        function ClearRoleMaster() {
            for (i = 0; i < Page_Validators.length; i++) {
                Page_Validators[i].style.display = "none";
            }
            $("#" + Prefix + "hdnAction").val("");
            $("#" + Prefix + "txtRole").val("");
            $("#" + Prefix + "txtDescription").val("");
            $("#" + Prefix + "chkIsEnabled").prop("checked", true);

        }
        function SetRoleMaster() {
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
