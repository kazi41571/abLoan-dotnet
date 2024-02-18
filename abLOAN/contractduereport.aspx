<%@ Page Language="C#" MasterPageFile="Site.Master" AutoEventWireup="true" CodeBehind="contractduereport.aspx.cs"
    Inherits="abLOAN.contractduereport" %>

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
    <div id="divcontract">
        <div class="blockui">
            <asp:UpdatePanel ID="upcontract" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <div class="row">
                        <div style="justify-content: space-between; display: flex;">
                            <div style="padding: 0 15px;">
                                <h4>
                                    <i class="fa fa-th"></i>&nbsp;<%= Resources.Resource.ContractDueReportFormTitle %>&nbsp;
                                    <small>
                                        <asp:Label ID="lblRecords" runat="server" Visible="false"></asp:Label></small>
                                    &nbsp;
                                    <a id="aFilter" class="btn btn-default btn-sm" href="" style="text-decoration: none;" onclick="javascript:return ShowHideFilter('divFilter', false);"><%= Resources.Resource.Filter %>&nbsp;&nbsp;<span class="caret"></span></a>
                                </h4>
                            </div>
                            <div style="padding: 0 15px;">
                                <%--<asp:Button ID="btnNew" runat="server" Text="<% $Resources:Resource, btnNew %>" CssClass="btn btn-primary" PostBackUrl="contractdetails.aspx" />--%>
                                <asp:Button ID="btnExportToExcel" runat="server" OnClick="btnExportToExcel_Click" Text="<% $Resources:Resource, btnExportToExcel %>" CssClass="btn btn-primary" />
                                &nbsp;
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
                                                    <label class="col-sm-5 control-label"><%= Resources.Resource.CustomerIdNo %></label>
                                                    <div class="col-sm-7">
                                                        <asp:TextBox ID="txtFilterCustomer" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-md-4 col-sm-6">
                                                <div class="form-group">
                                                    <label class="col-sm-5 control-label"><%= Resources.Resource.ContractTitle %></label>
                                                    <div class="col-sm-7">
                                                        <asp:TextBox ID="txtFilterContractTitle" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-md-4 col-sm-6">
                                                <div class="form-group">
                                                    <label class="col-sm-5 control-label"><%= Resources.Resource.Bank %></label>
                                                    <div class="col-sm-7">
                                                        <asp:DropDownList ID="ddlFilterBank" runat="server" CssClass="form-control input-sm"></asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-md-4 col-sm-6">
                                                <div class="form-group">
                                                    <label class="col-sm-5 control-label"><%= Resources.Resource.FileNo %></label>
                                                    <div class="col-sm-7">
                                                        <asp:TextBox ID="txtFilterFileNo" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                                        <div class="text-danger">
                                                            <asp:CompareValidator ID="cmvFilterFileNo" runat="server"
                                                                ControlToValidate="txtFilterFileNo" Display="Dynamic" Operator="DataTypeCheck" SetFocusOnError="true"
                                                                Type="Integer" ValidationGroup="FilterContractMaster"><%= Resources.Messages.InputRequired %> <%=Resources.Resource.FileNo %></asp:CompareValidator>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <%--<div class="col-md-4 col-sm-6">
                                                <div class="form-group">
                                                    <div class="col-sm-7">
                                                        <asp:TextBox ID="txtFilterContractDateTo" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                                        <div class="text-danger">
                                                            <asp:RequiredFieldValidator ID="rfvFilterContractDateTo" runat="server"
                                                                ControlToValidate="txtFilterContractDateTo" Display="Dynamic"
                                                                ValidationGroup="FilterContractMaster"><%= Resources.Messages.InputRequired %> <%=Resources.Resource.ContractDate %></asp:RequiredFieldValidator>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>--%>
                                            <div class="col-md-4 col-sm-6">
                                                <div class="form-group">
                                                    <label class="col-sm-5 control-label"><%= Resources.Resource.ContractStatus %></label>
                                                    <div class="col-sm-7">
                                                        <asp:DropDownList ID="ddlFilterContractStatus" runat="server" CssClass="form-control input-sm"></asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-md-4 col-sm-6">
                                                <div class="form-group">
                                                    <label class="col-sm-5 control-label"><%= Resources.Resource.LastPaidDate %></label>
                                                    <div class="col-sm-7">
                                                        <asp:TextBox ID="txtFilterLastPaidDateFrom" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-md-4 col-sm-6">
                                                <div class="form-group">
                                                    <div class="col-sm-7">
                                                        <asp:TextBox ID="txtFilterLastPaidDateTo" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-md-4 col-sm-6">
                                                <div class="form-group">
                                                    <label class="col-sm-5 control-label"><%= Resources.Resource.FilterDueDate %></label>
                                                    <div class="col-sm-7">
                                                        <asp:TextBox ID="txtFilterDueDateFrom" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-md-4 col-sm-6">
                                                <div class="form-group">
                                                    <div class="col-sm-7">
                                                        <asp:TextBox ID="txtFilterDueDateTo" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-md-12 text-center">
                                                <asp:Button ID="btnSearch" runat="server" Text="<% $Resources:Resource, btnSearch %>" CssClass="btn btn-primary" ValidationGroup="FilterContractMaster" OnClick="btnSearch_Click" />
                                                &nbsp;
                                                <asp:Button ID="btnReset" runat="server" Text="<% $Resources:Resource, btnReset %>" CssClass="btn btn-default" CausesValidation="false" OnClick="btnReset_Click" />
                                            </div>
                                        </div>
                                    </div>
                                </asp:Panel>
                                <asp:ValidationSummary ID="vsContractMaster" runat="server" DisplayMode="BulletList"
                                    ShowMessageBox="true" ShowSummary="false" ValidationGroup="FilterContractMaster" />
                            </div>
                        </div>
                    </div>

                    <asp:ListView ID="lvContractMaster" runat="server" DataKeyNames="ContractMasterId" OnItemDataBound="lvContractMaster_ItemDataBound">
                        <LayoutTemplate>
                            <div class="panel panel-default">
                                <div class="table-responsive">
                                    <table id="itemPlaceholderContainer" runat="server" class="table table-hover">
                                        <tr>
                                            <%--<td style="border: 1px solid; padding: 5px;">
                                                <span class="text-muted"><%= Resources.Resource.IdNo %>
                                                </span>
                                            </td>--%>
                                            <td style="border: 1px solid; padding: 5px;" colspan="2">
                                                <span class="text-muted"><%= Resources.Resource.Name %>
                                                </span>
                                            </td>
                                            <td style="border: 1px solid; padding: 5px;">
                                                <span class="text-muted"><%= Resources.Resource.Bank %>
                                                </span>
                                            </td>
                                            <td style="border: 1px solid; padding: 5px;">
                                                <span class="text-muted"><%= Resources.Resource.ContractTitle %>
                                                </span>
                                            </td>
                                            <td style="border: 1px solid; padding: 5px;">
                                                <span class="text-muted"><%= Resources.Resource.FileNo %>
                                                </span>
                                            </td>
                                            <td style="border: 1px solid; padding: 5px;">
                                                <span class="text-muted"><%= Resources.Resource.LastPaidDate %>
                                                </span>
                                            </td>
                                            <td style="border: 1px solid; padding: 5px;">
                                                <span class="text-muted"><%= Resources.Resource.AmountDue %>
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

                                <%--<td style="border: 1px solid; padding: 5px;">
                                    <asp:Literal ID="ltrlCustomerIdNo" runat="server"></asp:Literal>
                                </td>--%>
                                <td style="border: 1px solid; padding: 5px;" colspan="2">
                                    <asp:Literal ID="ltrlCustomer" runat="server"></asp:Literal>
                                </td>
                                <td style="border: 1px solid; padding: 5px;">
                                    <asp:Literal ID="ltrlBank" runat="server"></asp:Literal>
                                </td>
                                <td style="border: 1px solid; padding: 5px;">
                                    <asp:Literal ID="ltrlContractTitle" runat="server"></asp:Literal>
                                </td>
                                <td style="border: 1px solid; padding: 5px;">
                                    <asp:Literal ID="ltrlFileNo" runat="server"></asp:Literal>
                                </td>
                                <td style="border: 1px solid; padding: 5px;">
                                    <asp:Literal ID="ltrlLastPaidDate" runat="server"></asp:Literal>
                                </td>
                                <td style="border: 1px solid; padding: 5px;">
                                    <asp:Literal ID="ltrlDueAmount" runat="server"></asp:Literal>
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
                            <cc1:DataPager ID="pgrContractMaster" runat="server" OnPagerCommand="pgrContractMaster_ItemCommand"
                                Visible="false" CssClass="pagination" CurrentPageCssClass="active" PageSize="500"></cc1:DataPager>
                        </div>
                    </div>
                </ContentTemplate>
                <Triggers>
                    <asp:PostBackTrigger ControlID="btnExportToExcel" />

                </Triggers>
            </asp:UpdatePanel>
        </div>

    </div>
    <script type="text/javascript">
        $(document).ready(function () {
            SetFilterControlsPicker();
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(SetFilterControlsPicker);
        });

        function SetFilterControlsPicker() {
            //SetDatePicker(Prefix + "txtFilterLastPaidDate");
            SetRangeDatePicker(Prefix + "txtFilterLastPaidDateFrom", Prefix + "txtFilterLastPaidDateTo");
            //SetDatePicker(Prefix + "txtFilterContractDateTo");
            SetRangeDatePicker(Prefix + "txtFilterDueDateFrom", Prefix + "txtFilterDueDateTo");
            SearchCustomer();
        }

        function SearchCustomer() {
            $("#" + Prefix + "txtFilterCustomer").autocomplete({
                source: function (request, response) {
                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: "contractdetails.aspx/GetCustomerMaster",
                        data: "{'customer':'" + $("#" + Prefix + "txtFilterCustomer").val() + "'}",
                        dataType: "json",
                        success: function (data) {
                            response($.map(data.d, function (item) {
                                return {
                                    value: item.CustomerName + " (" + item.IdNo + ")"
                                }
                            }))
                        },
                    });
                },
                minLength: 3,
                select: function (event, ui) {
                    $("#" + Prefix + "txtFilterCustomer").val(ui.item.value);
                    $("#" + Prefix + "btnSearch").click();
                },
            });
        }
    </script>

</asp:Content>
