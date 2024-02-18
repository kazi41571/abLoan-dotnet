<%@ Page Language="C#" MasterPageFile="Site.Master" AutoEventWireup="true" CodeBehind="contractlistreport.aspx.cs"
    Inherits="abLOAN.contractlistreport" %>

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
                    <div class="row" style="display: none;">
                        <div style="justify-content: space-between; display: flex;">
                            <div style="padding: 0 15px;">
                                <h4>
                                    <i class="fa fa-th"></i>&nbsp;<%= Resources.Resource.ContractPageTitle %>&nbsp;
                                    <small>
                                        <asp:Label ID="lblRecords" runat="server" Visible="false"></asp:Label></small>
                                    &nbsp;
                                    <a id="aFilter" class="btn btn-default btn-sm" href="" style="text-decoration: none;" onclick="javascript:return ShowHideFilter('divFilter', false);"><%= Resources.Resource.Filter %>&nbsp;&nbsp;<span class="caret"></span></a>
                                </h4>
                            </div>
                            <div style="padding: 0 15px;">
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

                                            <div class="col-md-4 col-sm-6">
                                                <div class="form-group">
                                                    <label class="col-sm-5 control-label"><%= Resources.Resource.ContractStartDate %></label>
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
                                            </div>
                                            <div class="col-md-4 col-sm-6">
                                                <div class="form-group">
                                                    <label class="col-sm-5 control-label"><%= Resources.Resource.ContractStatus %></label>
                                                    <div class="col-sm-7">
                                                        <asp:DropDownList ID="ddlFilterContractStatus" runat="server" CssClass="form-control input-sm"></asp:DropDownList>
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

                    <asp:ListView ID="lvContractMaster" runat="server" DataKeyNames="ContractMasterId" OnItemCommand="lvContractMaster_ItemCommand" OnItemDataBound="lvContractMaster_ItemDataBound">
                        <LayoutTemplate>
                            <div class="panel panel-default">
                                <div class="table-responsive">
                                    <table id="itemPlaceholderContainer" runat="server" class="table table-hover">
                                        <tr>
                                            <th>
                                                <%= Resources.Resource.Name %>
                                            </th>
                                            <th>
                                                <%= Resources.Resource.IdNo %>
                                            </th>
                                            <th>
                                                <%= Resources.Resource.FileNo %>
                                            </th>
                                            <th><%= Resources.Resource.Contract %>
                                            </th>
                                            <th>
                                                <%= Resources.Resource.Bank %>
                                            </th>
                                            <th>
                                                <%= Resources.Resource.ContractDate %>:
                                            </th>
                                            <th>
                                                <%= Resources.Resource.ContractStartDate %>:
                                            </th>
                                            <th>
                                                <%= Resources.Resource.ContractEndDate %>:
                                            </th>
                                            <th>
                                                <%= Resources.Resource.Amount %>:
                                            </th>
                                            <th>
                                                <%= Resources.Resource.TotalPaidAmount %>:
                                            </th>
                                            <th>
                                                <%= Resources.Resource.Remaining %>:
                                            </th>
                                            <th>
                                                <%= Resources.Resource.DownPayment %>:
                                            </th>
                                            <th>
                                                <%= Resources.Resource.NoOfInstallments %>:
                                            </th>
                                            <th>
                                                <%= Resources.Resource.InstallmentAmount %>:
                                            </th>
                                            <th>
                                                <%= Resources.Resource.InstallmentDate %>:
                                            </th>
                                            <th>
                                                <%= Resources.Resource.DueAmount %>:
                                            </th>
                                            <th>
                                                <%= Resources.Resource.Amount %>:
                                            </th>
                                            <th>
                                                <%= Resources.Resource.SettlementReason %>:
                                            </th>
                                            <th>
                                                <%= Resources.Resource.LastPaidDate %>
                                            </th>
                                            <th style="width: 15%"><%= Resources.Resource.Notes %>
                                            </th>

                                            <th style="width: 125px"><%= Resources.Resource.ContractStatus %>
                                            </th>

                                            <th style="width: 25px;">
                                                <%= Resources.Resource.ModifiedBy %>
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
                                    <asp:HyperLink ID="ltrlCustomer" NavigateUrl="#" Target="_blank" runat="server">
                                    </asp:HyperLink>
                                </td>
                                <td>
                                    <asp:Literal ID="ltrlCustomerIdNo" runat="server"></asp:Literal>
                                </td>
                                <td>
                                    <asp:Literal ID="ltrlFileNo" runat="server"></asp:Literal>
                                </td>
                                <td>
                                    <asp:Literal ID="ltrlContractTitle" runat="server"></asp:Literal>
                                </td>
                                <td>
                                    <asp:Literal ID="ltrlBank" runat="server"></asp:Literal>
                                </td>
                                <td>
                                    <asp:Literal ID="ltrlContractDate" runat="server"></asp:Literal>
                                </td>
                                <td>
                                    <asp:Literal ID="ltrlContractStartDate" runat="server"></asp:Literal>
                                </td>
                                <td>
                                    <asp:Literal ID="ltrlContractEndDate" runat="server"></asp:Literal>
                                </td>
                                <td>
                                    <asp:Literal ID="ltrlContractAmount" runat="server"></asp:Literal>
                                </td>
                                <td>
                                    <asp:Literal ID="ltrlTotalPaidAmount" runat="server"></asp:Literal>
                                </td>
                                <td>
                                    <asp:Literal ID="ltrlRemainingAmount" runat="server"></asp:Literal>
                                </td>
                                <td>
                                    <asp:Literal ID="ltrlDownPayment" runat="server"></asp:Literal>
                                </td>
                                <td>
                                    <asp:Literal ID="ltrlNoOfInstallments" runat="server"></asp:Literal>
                                </td>
                                <td>
                                    <asp:Literal ID="ltrlInstallmentAmount" runat="server"></asp:Literal>
                                </td>
                                <td>
                                    <asp:Literal ID="ltrlInstallmentDate" runat="server"></asp:Literal>
                                </td>
                                <td>
                                    <asp:Label ID="lblDueAmount" runat="server"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="lblSettlementAmount" runat="server"></asp:Label>
                                </td>
                                <td>
                                    <asp:Literal ID="ltrlSettlementReason" runat="server"></asp:Literal>
                                </td>
                                <td>
                                    <asp:Literal ID="ltrlLastPaidDate" runat="server"></asp:Literal>
                                </td>
                                <td>
                                    <asp:Literal ID="ltrlNotes" runat="server"></asp:Literal>
                                </td>
                                <td style="padding-top: 15px;">
                                    <asp:Label ID="lblContractStatus" runat="server"></asp:Label>
                                </td>
                                <td>
                                    <asp:Literal ID="ltrlModifiedBy" runat="server"></asp:Literal>
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
    <script type="text/javascript">
        $(document).ready(function () {
            SetFilterControlsPicker();
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(SetFilterControlsPicker);
        });

        function SetFilterControlsPicker() {
            SetDatePicker(Prefix + "txtFilterContractDate");
            SetDatePicker(Prefix + "txtFilterContractDateTo");
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
