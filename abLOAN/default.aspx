<%@ Page Title="" Language="C#" MasterPageFile="Site.Master" AutoEventWireup="true"
    CodeBehind="default.aspx.cs" Inherits="abLOAN._default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphPage" runat="server">
    <asp:ListView ID="lvVerifyMaster" runat="server" OnItemDataBound="lvVerifyMaster_ItemDataBound">
        <LayoutTemplate>
            <div class="panel panel-default">
                <div class="table-responsive">
                    <table id="itemPlaceholderContainer" runat="server" class="table table-hover">
                        <tr>
                            <th>
                                <%= Resources.Resource.Links %>
                            </th>
                            <th>
                                <%= Resources.Resource.Count %>
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
                    <asp:HyperLink ID="ltrlUrl" NavigateUrl="#" Target="_blank" runat="server">
                    </asp:HyperLink>
                </td>
                <td>
                    <asp:Literal ID="ltrlTotal" runat="server"></asp:Literal>
                </td>

            </tr>

        </ItemTemplate>
        <EmptyDataTemplate>
            <div class="alert alert-info">
                <%= Resources.Resource.NoRecordMessage %>
            </div>
        </EmptyDataTemplate>
    </asp:ListView>
</asp:Content>
