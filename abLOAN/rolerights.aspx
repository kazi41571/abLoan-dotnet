<%@ Page Title="" Language="C#" MasterPageFile="Site.Master" AutoEventWireup="true"
    CodeBehind="rolerights.aspx.cs" Inherits="abLOAN.rolerights" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequest);
        });
        function EndRequest(sender, args) {
            EnableDisableListCheckBoxes();
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphPage" runat="server">
    <div id="divrolerights">
        <div class="blockui">
            <asp:UpdatePanel ID="uprolerights" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <div class="row">
                        <div style="justify-content: space-between; display: flex;">
                            <div style="padding: 0 15px;">
                                <h4>
                                    <i class="fa fa-th"></i>&nbsp;<%= Resources.Resource.RoleRightsPageTitle %> :
                                <asp:Label ID="lblRole" runat="server" Font-Bold="true"></asp:Label>
                                </h4>
                            </div>
                            <div style="padding: 0 15px;">
                                <asp:Button ID="btnSaveTop" runat="server" Text="<% $Resources:Resource, btnSave %>" CssClass="btn btn-primary"
                                    OnClick="btnSave_Click" />&nbsp;
                            <asp:Button ID="btnBackTop" runat="server" Text="<% $Resources:Resource, btnBack %>" CssClass="btn btn-default"
                                PostBackUrl="role.aspx" />&nbsp;
                            </div>
                        </div>
                    </div>
                    <div class="panel-group">
                        <asp:Repeater ID="rptRoleRightsGroup" runat="server" OnItemDataBound="rptRoleRightsGroup_ItemDataBound">
                            <ItemTemplate>
                                <asp:Panel ID="pnlRoleRights" runat="server">
                                    <div class="row">
                                        <div class="col-md-12">
                                            <asp:ListView ID="lvRoleRights" runat="server" DataKeyNames="RoleRightsMasterId" OnItemDataBound="lvRoleRights_ItemDataBound">
                                                <LayoutTemplate>
                                                    <div class="panel panel-default">
                                                        <div class="table-responsive">
                                                            <table id="itemPlaceholderContainer" runat="server" class="table table-hover">
                                                                <tr>
                                                                    <th style="width: 25px;">
                                                                        <asp:CheckBox ID="chkHeader" runat="server" onclick="javascript:ListHeaderCheckChanged(this, 'itemPlaceholderContainer', 'chkHeader', 'chkSelect');EnableDisableListCheckBoxes();" />
                                                                    </th>
                                                                    <th>
                                                                        <asp:Literal ID="ltrlRoleRightGroup" runat="server"></asp:Literal>
                                                                    </th>
                                                                    <th style="width: 80px;">
                                                                        <asp:CheckBox ID="chkHeaderViewList" runat="server" CssClass="checkbox-inline" Text="<% $Resources:Resource, List %>" onclick="javascript:ListHeaderCheckChanged(this, 'itemPlaceholderContainer', 'chkHeaderViewList', 'chkSelectViewList');" /></th>
                                                                    <th style="width: 80px;">
                                                                        <asp:CheckBox ID="chkHeaderViewRecord" runat="server" CssClass="checkbox-inline" Text="<% $Resources:Resource, View %>" onclick="javascript:ListHeaderCheckChanged(this, 'itemPlaceholderContainer', 'chkHeaderViewRecord', 'chkSelectViewRecord');" /></th>
                                                                    <th style="width: 80px;">
                                                                        <asp:CheckBox ID="chkHeaderAddRecord" runat="server" CssClass="checkbox-inline" Text="<% $Resources:Resource, Add %>" onclick="javascript:ListHeaderCheckChanged(this, 'itemPlaceholderContainer', 'chkHeaderAddRecord', 'chkSelectAddRecord');" /></th>
                                                                    <th style="width: 80px;">
                                                                        <asp:CheckBox ID="chkHeaderEditRecord" runat="server" CssClass="checkbox-inline" Text="<% $Resources:Resource, Edit %>" onclick="javascript:ListHeaderCheckChanged(this, 'itemPlaceholderContainer', 'chkHeaderEditRecord', 'chkSelectEditRecord');" /></th>
                                                                    <th style="width: 80px;">
                                                                        <asp:CheckBox ID="chkHeaderDeleteRecord" runat="server" CssClass="checkbox-inline" Text="<% $Resources:Resource, Delete %>" onclick="javascript:ListHeaderCheckChanged(this, 'itemPlaceholderContainer', 'chkHeaderDeleteRecord', 'chkSelectDeleteRecord');" /></th>
                                                                </tr>
                                                                <tr id="itemPlaceholder" runat="server">
                                                                </tr>
                                                            </table>
                                                        </div>
                                                    </div>
                                                    <br />
                                                </LayoutTemplate>
                                                <ItemTemplate>
                                                    <tr valign="top" id='row' runat="server" class="rolerightsrow">
                                                        <td>
                                                            <asp:CheckBox ID="chkSelect" runat="server" onclick='<%# "javascript:ListEnableDisableRowCheckBoxes(this, \"row\");" %>' />
                                                        </td>
                                                        <td>
                                                            <asp:Literal ID="ltrlRoleRight" runat="server"></asp:Literal>
                                                        </td>
                                                        <td>
                                                            <asp:CheckBox ID="chkSelectViewList" runat="server" />
                                                        </td>
                                                        <td>
                                                            <asp:CheckBox ID="chkSelectViewRecord" runat="server" />
                                                        </td>
                                                        <td>
                                                            <asp:CheckBox ID="chkSelectAddRecord" runat="server" />
                                                        </td>
                                                        <td>
                                                            <asp:CheckBox ID="chkSelectEditRecord" runat="server" />
                                                        </td>
                                                        <td>
                                                            <asp:CheckBox ID="chkSelectDeleteRecord" runat="server" />
                                                        </td>
                                                    </tr>
                                                </ItemTemplate>
                                            </asp:ListView>
                                        </div>
                                    </div>
                                </asp:Panel>
                            </ItemTemplate>
                        </asp:Repeater>
                    </div>
                    <div class="row">
                        <div style="justify-content: space-between; display: flex;">
                            <div>
                            </div>
                            <div style="padding: 0 15px;">
                                <asp:Button ID="btnSave" runat="server" Text="<% $Resources:Resource, btnSave %>" CssClass="btn btn-primary" OnClick="btnSave_Click" />&nbsp;
                                <asp:Button ID="btnBack" runat="server" Text="<% $Resources:Resource, btnBack %>" CssClass="btn btn-default" PostBackUrl="role.aspx" />&nbsp;
                            </div>
                        </div>
                    </div>

                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>
    <script type="text/javascript">
        $(document).ready(function () {
            EnableDisableListCheckBoxes();
        });
        function EnableDisableListCheckBoxes() {
            $(".rolerightsrow").each(function (index, object) {
                var CheckBoxId = object.id.replace("row", "chkSelect");
                var objCheckBox = document.getElementById(CheckBoxId);

                ListEnableDisableRowCheckBoxes(objCheckBox, "row");
            });
        }
    </script>
</asp:Content>
