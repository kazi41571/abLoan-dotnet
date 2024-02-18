<%@ Page Language="C#" MasterPageFile="Site.Master" AutoEventWireup="true" CodeBehind="contractamountreport.aspx.cs"
    Inherits="abLOAN.contractamountreport" %>

<%@ Register Assembly="CustomDataPager" Namespace="CustomDataPager" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphPage" runat="server">
    <div id="divcontract">
        <div class="blockui">
            <asp:UpdatePanel ID="upcontract" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <div class="row">
                        <div style="justify-content: space-between; display: flex;">
                            <div style="padding: 0 15px;">
                                <h4>
                                    <i class="fa fa-th"></i>&nbsp;<%= Resources.Resource.ContractAmountReportFormTitle %>&nbsp;
                                </h4>
                            </div>
                        </div>
                    </div>
                    <asp:ListView ID="lvContractMaster" runat="server" DataKeyNames="ContractMasterId" OnItemDataBound="lvContractMaster_ItemDataBound">
                        <LayoutTemplate>
                            <div class="panel panel-default">
                                <div class="table-responsive">
                                    <table id="itemPlaceholderContainer" runat="server" class="table table-hover">
                                        <tr>
                                            <td style="border: 1px solid; padding: 5px;">
                                                <span class="text-muted"><%= Resources.Resource.ContractAmountTotal %>
                                                </span>
                                            </td>
                                            <td style="border: 1px solid; padding: 5px;">
                                                <span class="text-muted"><%= Resources.Resource.PendingAmountTotal %>
                                                </span>
                                            </td>
                                            <td style="border: 1px solid; padding: 5px;">
                                                <span class="text-muted"><%= Resources.Resource.IncomeAmountTotal %>
                                                </span>
                                            </td>
                                            <td style="border: 1px solid; padding: 5px;">
                                                <span class="text-muted"><%= Resources.Resource.TotalInstallmentAmount %>
                                                </span>
                                            </td>
                                        </tr>
                                        <tr id="itemPlaceholder" runat="server">
                                        </tr>
                                    </table>
                                </div>
                            </div>
                        </LayoutTemplate>
                        <ItemTemplate>
                            <tr valign="top">
                                <td style="border: 1px solid; padding: 5px;">
                                    <asp:Literal ID="ltrlContractAmount" runat="server"></asp:Literal>
                                </td>
                                <td style="border: 1px solid; padding: 5px;">
                                    <asp:Literal ID="ltrlPendingAmount" runat="server"></asp:Literal>
                                </td>
                                <td style="border: 1px solid; padding: 5px;">
                                    <asp:Literal ID="ltrlIncomeAmount" runat="server"></asp:Literal>
                                </td>
                                <td style="border: 1px solid; padding: 5px;">
                                    <asp:Literal ID="ltrlInstallmentAmount" runat="server"></asp:Literal>
                                </td>
                            </tr>
                        </ItemTemplate>
                        <EmptyDataTemplate>
                            <div class="alert alert-info">
                                <%= Resources.Resource.NoRecordMessage %>
                            </div>
                        </EmptyDataTemplate>
                    </asp:ListView>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>
</asp:Content>
