<%@ Page Language="C#" MasterPageFile="Site.Master" AutoEventWireup="true" CodeBehind="customerreport.aspx.cs"
    Inherits="abLOAN.customerreport" %>

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
                                    <i class="fa fa-th"></i>&nbsp;<%= Resources.Resource.CustomerReportFormTitle %>&nbsp;
                                    <small>
                                        <asp:Label ID="lblRecords" runat="server" Visible="false"></asp:Label></small>
                                    &nbsp;
                                    <a id="aFilter" class="btn btn-default btn-sm" href="" style="text-decoration: none;" onclick="javascript:return ShowHideFilter('divFilter', false);"><%= Resources.Resource.Filter %>&nbsp;&nbsp;<span class="caret"></span></a>
                                </h4>
                            </div>
                            <div style="padding: 0 15px; display: none;">
                                <asp:Button ID="btnNew" runat="server" Text="<% $Resources:Resource, btnNew %>" CssClass="btn btn-primary" PostBackUrl="contractdetails.aspx" />
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
                                                    <label class="col-sm-5 control-label"><%= Resources.Resource.Bank %></label>
                                                    <div class="col-sm-7">
                                                        <asp:DropDownList ID="ddlFilterBank" runat="server" CssClass="form-control input-sm"></asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>
                                            <%--</div>
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
                                            </div>--%>

                                            <%--<div class="col-md-4 col-sm-6">
                                                <div class="form-group">
                                                    <label class="col-sm-5 control-label"><%= Resources.Resource.ContractDateFrom %></label>
                                                    <div class="col-sm-7">
                                                        <asp:TextBox ID="txtFilterContractDate" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                                        <div class="text-danger">
                                                            <asp:RequiredFieldValidator ID="rfvFilterContractDate" runat="server"
                                                                ControlToValidate="txtFilterContractDate" Display="Dynamic"
                                                                ValidationGroup="FilterContractMaster"><%= Resources.Messages.InputRequired %> <%=Resources.Resource.ContractDate %></asp:RequiredFieldValidator>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-md-4 col-sm-6">
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
                                        <%--<tr>
                                            <th>
                                                <%= Resources.Resource.CustomerFormTitle %>
                                            </th>

                                        </tr>--%>

                                        <tr id="itemPlaceholder" runat="server">
                                        </tr>
                                    </table>
                                </div>
                            </div>
                        </LayoutTemplate>
                        <ItemTemplate>
                            <tr valign="top">
                                <td>
                                    <table width="100%">
                                        <tr>
                                            <td style="border: 1px solid; padding: 5px;">
                                                <span class="text-muted"><%= Resources.Resource.Name %>
                                                </span>
                                            </td>
                                            <td style="border: 1px solid; padding: 5px;" >
                                                <asp:Literal ID="ltrlCustomer" runat="server"></asp:Literal>
                                            </td>
                                             <td style="border: 1px solid; padding: 5px;">
                                                <span class="text-muted"><%= Resources.Resource.Guarantor %>
                                                </span>
                                            </td>
                                            <td style="border: 1px solid; padding: 5px;">
                                                <asp:Literal ID="ltrlGuarantorName" runat="server"></asp:Literal>
                                            </td>

                                            <td style="border: 1px solid; padding: 5px;">
                                                <span class="text-muted"><%= Resources.Resource.IdNo %>
                                                </span>
                                            </td>
                                            <td style="border: 1px solid; padding: 5px;">
                                                <asp:Literal ID="ltrlCustomerIdNo" runat="server"></asp:Literal>
                                            </td>
                                        </tr>
                                        <tr>
                                            <%--<td style="border: 1px solid; padding: 5px;">
                                                <span class="text-muted"><%= Resources.Resource.ContractTitle %>
                                                </span>
                                            </td>--%>
                                            <td style="border: 1px solid; padding: 5px;">
                                                <span class="text-muted"><%= Resources.Resource.ContractAmount %>
                                                </span>
                                            </td>
                                            <td style="border: 1px solid; padding: 5px;">
                                                <span class="text-muted"><%= Resources.Resource.InstallmentStart %>
                                                </span>
                                            </td>

                                            <td style="border: 1px solid; padding: 5px;">
                                                <span class="text-muted"><%= Resources.Resource.InstallmentAmount %>
                                                </span>
                                            </td>
                                            <td style="border: 1px solid; padding: 5px;">
                                                <span class="text-muted"><%= Resources.Resource.Remaining %>
                                                </span>
                                            </td>
                                            <td style="border: 1px solid; padding: 5px;text-align:center" colspan="2">
                                                <span class="text-muted"><%= Resources.Resource.Notes %>
                                                </span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <%--<td style="border: 1px solid; padding: 5px;">
                                                <asp:Literal ID="ltrlContractTitle" runat="server"></asp:Literal>
                                            </td>--%>
                                            <td style="border: 1px solid; padding: 5px;">
                                                <asp:Literal ID="ltrlContractAmount" runat="server"></asp:Literal>
                                            </td>
                                            <td style="border: 1px solid; padding: 5px;">
                                                <asp:Literal ID="ltrlContractStartDate" runat="server"></asp:Literal>
                                            </td>
                                            <td style="border: 1px solid; padding: 5px;">
                                                <asp:Literal ID="ltrlInstallmentAmount" runat="server"></asp:Literal>
                                            </td>
                                            <td style="border: 1px solid; padding: 5px;">
                                                <asp:Literal ID="ltrlRemainingAmount" runat="server"></asp:Literal>
                                            </td>
                                            <td style="border: 1px solid; padding: 5px;" rowspan="3" colspan="2">
                                                <asp:Literal ID="ltrlNotes" runat="server"></asp:Literal>
                                            </td>
                                        </tr>
                                        <tr>
                                            <%--<td style="border: 1px solid; padding: 5px;">
                                                <span class="text-muted"><%= Resources.Resource.FileNo %>
                                                </span>
                                            </td>--%>
                                            <td style="border: 1px solid; padding: 5px;">
                                                <span class="text-muted"><%= Resources.Resource.InstallmentDate %>
                                                </span>
                                            </td>
                                            <td style="border: 1px solid; padding: 5px;">
                                                <span class="text-muted"><%= Resources.Resource.LastPaidDate %>
                                                </span>
                                            </td>
                                            <td style="border: 1px solid; padding: 5px;">
                                                <span class="text-muted"><%= Resources.Resource.TotalPaidAmount %>
                                                </span>
                                            </td>
                                            <td style="border: 1px solid; padding: 5px;">
                                                <span class="text-muted"><%= Resources.Resource.AmountDue %>                                                
                                                </span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <%--<td style="border: 1px solid; padding: 5px;">
                                                <asp:Literal ID="ltrlFileNo" runat="server"></asp:Literal>
                                            </td>--%>
                                            <td style="border: 1px solid; padding: 5px;">
                                                <asp:Literal ID="ltrlInstallmentDate" runat="server"></asp:Literal>
                                            </td>
                                            <td style="border: 1px solid; padding: 5px;">
                                                <asp:Literal ID="ltrlLastPaidDate" runat="server"></asp:Literal>
                                            </td>
                                            <td style="border: 1px solid; padding: 5px;">
                                                <asp:Literal ID="ltrlTotalPaidAmount" runat="server"></asp:Literal>
                                            </td>
                                            <td style="border: 1px solid; padding: 5px;">
                                                <asp:Label ID="lblDueAmount" runat="server"></asp:Label>
                                            </td>

                                        </tr>
                                    </table>
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
                                Visible="false" CssClass="pagination" CurrentPageCssClass="active" PageSize="100"></cc1:DataPager>
                        </div>
                    </div>


                </ContentTemplate>

            </asp:UpdatePanel>
        </div>

    </div>
    <script type="text/javascript">
        $(document).ready(function () {
            SetFilterControlsPicker();
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(SetFilterControlsPicker);
        });

        function SetFilterControlsPicker() {
            //    SetDatePicker(Prefix + "txtFilterContractDate");
            //    SetDatePicker(Prefix + "txtFilterContractDateTo");
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
