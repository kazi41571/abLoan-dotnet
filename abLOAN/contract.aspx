<%@ Page Language="C#" MasterPageFile="Site.Master" AutoEventWireup="true" CodeBehind="contract.aspx.cs"
    Inherits="abLOAN.contract" %>

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
                                    <i class="fa fa-th"></i>&nbsp;<%= Resources.Resource.ContractPageTitle %>&nbsp;
                                    <small>
                                        <asp:Label ID="lblRecords" runat="server" Visible="false"></asp:Label></small>
                                    &nbsp;
                                    <a id="aFilter" class="btn btn-default btn-sm" href="" style="text-decoration: none;" onclick="javascript:return ShowHideFilter('divFilter', false);"><%= Resources.Resource.Filter %>&nbsp;&nbsp;<span class="caret"></span></a>
                                </h4>
                            </div>
                            <div style="padding: 0 15px;">
                                <asp:Button ID="btnDownload" runat="server" Text="<% $Resources:Resource, Download %>" CssClass="btn btn-info" OnClick="btnDownload_Click" />
                                &nbsp;
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
                                            <div class="col-md-4 col-sm-6">
                                                <div class="form-group">
                                                    <label class="col-sm-5 control-label"><%= Resources.Resource.Hasurl %></label>
                                                    <div class="col-sm-7">
                                                        <asp:DropDownList ID="ddlFilterHasurl" runat="server" CssClass="form-control input-sm">
                                                            <asp:ListItem Text="- SELECT -" Value=""></asp:ListItem>
                                                            <asp:ListItem Text="<% $Resources:Resource, Yes %>" Value="Yes"></asp:ListItem>
                                                            <asp:ListItem Text="<% $Resources:Resource, No %>" Value="No"></asp:ListItem>
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-md-4 col-sm-6">
                                                <div class="form-group">
                                                    <label class="col-sm-5 control-label"><%= Resources.Resource.DueInstallments %></label>
                                                    <div class="col-sm-7">
                                                        <asp:TextBox ID="txtFilterDueInstallments" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                                        <div class="text-danger">
                                                            <asp:CompareValidator ID="cvFilterDueInstallments" runat="server"
                                                                ControlToValidate="txtFilterDueInstallments" Display="Dynamic" Operator="DataTypeCheck" SetFocusOnError="true"
                                                                Type="Integer" ValidationGroup="FilterContractMaster"><%= Resources.Messages.InputRequired %> <%=Resources.Resource.DueInstallments %></asp:CompareValidator>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-md-4 col-sm-6">
                                                <div class="form-group">
                                                    <label class="col-sm-5 control-label"><%= Resources.Resource.BankAccountNumber %></label>
                                                    <div class="col-sm-7">
                                                        <asp:TextBox ID="txtFilterBankAccountNumber" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-md-4 col-sm-6">
                                                <div class="form-group">
                                                    <label class="col-sm-5 control-label"><%= Resources.Resource.SortBy %></label>
                                                    <div class="col-sm-7">
                                                        <asp:DropDownList ID="ddlSortBy" runat="server" CssClass="form-control input-sm">
                                                            <asp:ListItem Text="- SELECT -" Value=""></asp:ListItem>
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
                                            <th><%= Resources.Resource.Customer %>
                                            </th>
                                            <th><%= Resources.Resource.ContractFormTitle %>
                                            </th>
                                            <th><%= Resources.Resource.Installment %>
                                            </th>
                                            <th><%= Resources.Resource.Settlement %></th>
                                            <th style="width: 15%"><%= Resources.Resource.Notes %>
                                            </th>
                                            <th style="width: 100px"><%= Resources.Resource.Links %>
                                            </th>
                                            <th style="width: 125px"><%= Resources.Resource.ContractStatus %>
                                            </th>
                                            <th style="width: 25px;">
                                                <%= Resources.Resource.VerifiedBy %>
                                            </th>
                                            <th style="width: 25px;">
                                                <%= Resources.Resource.ModifiedBy %>
                                            </th>
                                            <th style="width: 25px;">
                                                <%= Resources.Resource.CreatedBy %>
                                            </th>
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
                                    <span class="text-muted"><%= Resources.Resource.Name %>:
                                    </span>
                                    <asp:Literal ID="ltrlCustomer" runat="server"></asp:Literal>
                                    <asp:HyperLink ID="hlnkCustomer" NavigateUrl="#" Target="_blank" runat="server" Visible="false">
                                    </asp:HyperLink>
                                    <br />
                                    <span class="text-muted"><%= Resources.Resource.IdNo %>:
                                    </span>
                                    <asp:Literal ID="ltrlCustomerIdNo" runat="server"></asp:Literal>
                                    <br />
                                    <span class="text-muted"><%= Resources.Resource.FileNo %>:
                                    </span>
                                    <asp:Literal ID="ltrlFileNo" runat="server"></asp:Literal>
                                    <br />
                                    <span class="text-muted"><%= Resources.Resource.GuarantorName %>:
                                    </span>
                                    <asp:Literal ID="ltrlGuarantorName" runat="server"></asp:Literal>
                                    <br />
                                    <span class="text-muted"><%= Resources.Resource.BankAccountNumber %>:
                                    </span>
                                    <asp:Literal ID="ltrlBankAccountNumber" runat="server"></asp:Literal>
                                    <br />
                                    <span class="text-muted"><%= Resources.Resource.BankAccountNumber2 %>:
                                    </span>
                                    <asp:Literal ID="ltrlBankAccountNumber2" runat="server"></asp:Literal>
                                    <br />
                                    <span class="text-muted"><%= Resources.Resource.BankAccountNumber3 %>:
                                    </span>
                                    <asp:Literal ID="ltrlBankAccountNumber3" runat="server"></asp:Literal>
                                    <br />
                                    <span class="text-muted"><%= Resources.Resource.BankAccountNumber4 %>:
                                    </span>
                                    <asp:Literal ID="ltrlBankAccountNumber4" runat="server"></asp:Literal>
                                </td>
                                <td>
                                    <span class="text-muted"><%= Resources.Resource.Contract %>:
                                    </span>
                                    <asp:Literal ID="ltrlContractTitle" runat="server"></asp:Literal>
                                    <asp:HyperLink ID="hlnkContractTitle" NavigateUrl="#" Target="_blank" runat="server" Visible="false">
                                    </asp:HyperLink>
                                    <br />
                                    <span class="text-muted"><%= Resources.Resource.Bank %>:
                                    </span>
                                    <asp:Literal ID="ltrlBank" runat="server"></asp:Literal>
                                    <br />
                                    <span class="text-muted"><%= Resources.Resource.ContractDate %>:
                                    </span>
                                    <asp:Literal ID="ltrlContractDate" runat="server"></asp:Literal>
                                    <br />
                                    <span class="text-muted"><%= Resources.Resource.ContractStartDate %>:
                                    </span>
                                    <asp:Literal ID="ltrlContractStartDate" runat="server"></asp:Literal>
                                    <br />
                                    <span class="text-muted"><%= Resources.Resource.ContractEndDate %>:
                                    </span>
                                    <asp:Literal ID="ltrlContractEndDate" runat="server"></asp:Literal>
                                    <br />
                                    <span class="text-muted"><%= Resources.Resource.Amount %>:
                                    </span>
                                    <asp:Literal ID="ltrlContractAmount" runat="server"></asp:Literal>
                                    <br />
                                    <span class="text-muted"><%= Resources.Resource.TotalPaidAmount %>:
                                    </span>
                                    <asp:Literal ID="ltrlTotalPaidAmount" runat="server"></asp:Literal>
                                    <br />
                                    <span class="text-muted"><%= Resources.Resource.Remaining %>:
                                    </span>
                                    <asp:Literal ID="ltrlRemainingAmount" runat="server"></asp:Literal>
                                </td>
                                <td>
                                    <span class="text-muted"><%= Resources.Resource.DownPayment %>:
                                    </span>
                                    <asp:Literal ID="ltrlDownPayment" runat="server"></asp:Literal>
                                    <br />
                                    <span class="text-muted"><%= Resources.Resource.NoOfInstallments %>:
                                    </span>
                                    <asp:Literal ID="ltrlNoOfInstallments" runat="server"></asp:Literal>
                                    <br />
                                    <span class="text-muted"><%= Resources.Resource.InstallmentAmount %>:
                                    </span>
                                    <asp:Literal ID="ltrlInstallmentAmount" runat="server"></asp:Literal>
                                    <br />
                                    <span class="text-muted"><%= Resources.Resource.InstallmentDate %>:
                                    </span>
                                    <asp:Literal ID="ltrlInstallmentDate" runat="server"></asp:Literal>
                                    <br />
                                    <span class="text-muted"><%= Resources.Resource.LastPaidAmount %>:
                                    </span>
                                    <asp:Literal ID="ltrlLastPaidAmount" runat="server"></asp:Literal>
                                    <br />
                                    <span class="text-muted"><%= Resources.Resource.LastPaidDate %>:
                                    </span>
                                    <asp:Literal ID="ltrlLastPaidDate" runat="server"></asp:Literal>
                                    <br />
                                    <span class="text-muted"><%= Resources.Resource.DueAmount %>:
                                    </span>
                                    <asp:Label ID="lblDueAmount" runat="server"></asp:Label>
                                </td>
                                <td><span class="text-muted"><%= Resources.Resource.Amount %>:
                                </span>
                                    <asp:Label ID="lblSettlementAmount" runat="server"></asp:Label>
                                    <br />
                                    <span class="text-muted"><%= Resources.Resource.SettlementReason %>:
                                    </span>
                                    <asp:Literal ID="ltrlSettlementReason" runat="server"></asp:Literal>
                                </td>
                                <td>
                                    <asp:Literal ID="ltrlNotes" runat="server"></asp:Literal>
                                </td>
                                <td style="padding-top: 15px;">
                                    <asp:HyperLink ID="lnkLinks" NavigateUrl="#" Target="_blank" runat="server" CssClass="fa fa-link">&nbsp;<%= Resources.Resource.Contract %>
                                    </asp:HyperLink>
                                    <br />
                                    <asp:HyperLink ID="lnkCustomerLinks" NavigateUrl="#" Target="_blank" runat="server" CssClass="fa fa-link">&nbsp;<%= Resources.Resource.Customer %>
                                    </asp:HyperLink>
                                </td>
                                <td style="padding-top: 15px;">
                                    <asp:Label ID="lblContractStatus" runat="server"></asp:Label>
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
                                    <asp:Literal ID="ltrlCreatedBy" runat="server"></asp:Literal>
                                    <asp:Literal ID="ltrlCreatedDateTime" runat="server"></asp:Literal>
                                </td>
                                <td>
                                    <asp:LinkButton ID="lbtnEdit" runat="server" CommandName="EditRecord" CssClass="btn btn-primary btn-sm fa fa-edit" title="Edit"></asp:LinkButton>
                                </td>
                                <td>
                                    <asp:LinkButton ID="lbtnDelete" runat="server" CommandName="DeleteRecord" OnClientClick="javascript:return ConfirmDelete(this);" CssClass="btn btn-danger btn-sm fa fa-trash" title="Delete"></asp:LinkButton>
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
                                Visible="false" CssClass="pagination" CurrentPageCssClass="active"></cc1:DataPager>
                        </div>
                    </div>


                </ContentTemplate>
                <Triggers>
                    <asp:PostBackTrigger ControlID="btnDownload" />
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
