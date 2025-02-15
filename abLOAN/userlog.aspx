<%@ Page Language="C#" MasterPageFile="Site.Master" AutoEventWireup="true" CodeBehind="userlog.aspx.cs"
    Inherits="abLOAN.userlog" %>

<%@ Register Assembly="CustomDataPager" Namespace="CustomDataPager" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script type="text/javascript">
        $(document).ready(function () {
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequest);
            SetFilter();
        });
        function EndRequest(sender, args) {
            SetFilter();
        }
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphPage" runat="server">
    <div id="divuserlog">
        <div class="blockui">
            <asp:UpdatePanel ID="upuserlog" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <div class="row">
                        <div style="justify-content: space-between; display: flex;">
                            <div style="padding: 0 15px;">
                                <h4>
                                    <i class="fa fa-th"></i>&nbsp;
                                     <%= Resources.Resource.UserlogPageTitle %>
                                    &nbsp; <small>
                                        <asp:Label ID="lblRecords" runat="server" Visible="False"></asp:Label></small>
                                    &nbsp; <a id="aFilter" class="btn btn-default btn-sm" href="" style="text-decoration: none;" onclick="javascript:return ShowHideFilter('divFilter', false);"><%= Resources.Resource.Filter %>&nbsp;&nbsp;<span class="caret"></span></a>
                                </h4>
                            </div>
                            <div style="padding: 0 15px;">
                            </div>
                        </div>
                    </div>
                    <div id="divFilter" style="display: none;">
                        <asp:HiddenField ID="hdnFilter" runat="server" Value="0" ClientIDMode="Static" />
                        <div class="panel panel-default">
                            <div class="panel-body">
                                <asp:Panel ID="pnlFilter" runat="server" DefaultButton="btnSearch">
                                    <div class="form-horizontal">
                                        <div class="row">
                                            <div class="col-md-4 col-sm-6">
                                                <div class="form-group">
                                                    <label class="col-sm-5 control-label"><%= Resources.Resource.User %></label>
                                                    <div class="col-sm-7">
                                                        <asp:DropDownList ID="ddlFilterUser" runat="server" CssClass="form-control input-sm"></asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="row">
                                            <div class="col-md-12 text-center">
                                                <asp:Button ID="btnSearch" runat="server" Text="<% $Resources:Resource, btnSearch %>" CssClass="btn btn-primary" ValidationGroup="FilterUserTran" OnClick="btnSearch_Click" />
                                                &nbsp;
                                                <asp:Button ID="btnReset" runat="server" Text="<% $Resources:Resource, btnReset %>" CssClass="btn btn-default" CausesValidation="false" OnClick="btnReset_Click" />
                                            </div>
                                        </div>
                                    </div>
                                </asp:Panel>
                                <asp:ValidationSummary ID="vsUserTran" runat="server" DisplayMode="BulletList"
                                    ShowMessageBox="true" ShowSummary="false" ValidationGroup="FilterUserTran" />
                            </div>
                        </div>
                    </div>

                    <asp:ListView ID="lvUserTran" runat="server" DataKeyNames="UserTranId" OnItemDataBound="lvUserTran_ItemDataBound">
                        <LayoutTemplate>
                            <div class="panel panel-default">
                                <div class="table-responsive">
                                    <table id="itemPlaceholderContainer" runat="server" class="table table-hover">
                                        <tr>
                                            <th><%= Resources.Resource.User %>
                                            </th>
                                            <th><%= Resources.Resource.SessionId %>
                                            </th>
                                            <th><%= Resources.Resource.LastLoginDateTime %>
                                            </th>
                                            <th><%= Resources.Resource.LastLogoutDateTime %>
                                            </th>
                                            <th><%= Resources.Resource.Os %>
                                            </th>
                                            <th><%= Resources.Resource.IpAddress %>
                                            </th>
                                            <th><%= Resources.Resource.DeviceName %>
                                            </th>
                                            <th><%= Resources.Resource.Browser %>
                                            </th>

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
                                <td>
                                    <asp:Literal ID="ltrlSessionId" runat="server"></asp:Literal>
                                </td>
                                <td>
                                    <asp:Literal ID="ltrlLoginDateTime" runat="server"></asp:Literal>
                                </td>
                                <td>
                                    <asp:Literal ID="ltrlLogoutDateTime" runat="server"></asp:Literal>
                                </td>
                                <td>
                                    <asp:Literal ID="ltrlOS" runat="server"></asp:Literal>
                                </td>
                                <td>
                                    <asp:Literal ID="ltrlIPAddress" runat="server"></asp:Literal>
                                </td>
                                <td>
                                    <asp:Literal ID="ltrlDeviceName" runat="server"></asp:Literal>
                                </td>
                                <td>
                                    <asp:Literal ID="ltrlBrowser" runat="server"></asp:Literal>
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
                            <cc1:DataPager ID="pgrUserTran" runat="server" OnPagerCommand="pgrUserTran_ItemCommand"
                                Visible="false" CssClass="pagination" CurrentPageCssClass="active"></cc1:DataPager>
                        </div>
                    </div>


                </ContentTemplate>

            </asp:UpdatePanel>
        </div>

    </div>
    <script type="text/javascript">
        $(document).ready(function () {
            SetControlsPicker();
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(SetControlsPicker);
        });

        function SetControlsPicker() {
            SetDatePicker(Prefix + "txtLoginDateTime");
            SetDatePicker(Prefix + "txtLogoutDateTime");
        }
    </script>

</asp:Content>
