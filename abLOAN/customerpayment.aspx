<%@ Page Language="C#" MasterPageFile="Site.Master" AutoEventWireup="true" CodeBehind="customerpayment.aspx.cs"
    Inherits="abLOAN.customerpayment" %>

<%@ Register Assembly="CustomDataPager" Namespace="CustomDataPager" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script type="text/javascript">
        $(document).ready(function () {
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequest);
            SetFilter();
        });
        function EndRequest(sender, args) {

            ShowHideDialogCustomerPaymentMaster();
            SetFilter();
            Calculate();
        }
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphPage" runat="server">
    <div id="divcustomerpayment">
        <div class="blockui">
            <asp:UpdatePanel ID="upcustomerpayment" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <div class="row">
                        <div style="justify-content: space-between; display: flex;">
                            <div style="padding: 0 15px;">
                                <h4>
                                    <i class="fa fa-th"></i>&nbsp;<%= Resources.Resource.CustomerPaymentPageTitle %>&nbsp;
                                    <small>
                                        <asp:Label ID="lblRecords" runat="server" Visible="false"></asp:Label></small>
                                    &nbsp;
                                    <a id="aFilter" class="btn btn-default btn-sm" href="" style="text-decoration: none;" onclick="javascript:return ShowHideFilter('divFilter', false);"><%= Resources.Resource.Filter %>&nbsp;&nbsp;<span class="caret"></span></a>
                                </h4>
                            </div>
                            <div style="padding: 0 15px;">
                                <asp:Button ID="btnDownload" runat="server" Text="<% $Resources:Resource, Download %>" CssClass="btn btn-info" OnClick="btnDownload_Click" />
                                &nbsp;
                                <asp:Button ID="btnNew" runat="server" Text="<% $Resources:Resource, btnNew %>" CssClass="btn btn-primary" data-toggle="modal" data-target="#divcustomerpaymentdetails" OnClientClick="javascript:return false;" />
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
                                                        <asp:TextBox ID="txtContractTitle" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-md-4 col-sm-6">
                                                <div class="form-group">
                                                    <label class="col-sm-4 control-label"><%= Resources.Resource.PaymentDate %></label>
                                                    <div class="col-sm-4">
                                                        <asp:TextBox ID="txtFromDate" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                                        <div class="text-danger">
                                                            <asp:RequiredFieldValidator ID="rfvFilterPaymentDateFrom" runat="server"
                                                                ControlToValidate="txtFromDate" Display="Dynamic"
                                                                ValidationGroup="FilterCustomerPaymentMaster"><%= Resources.Messages.InputRequired %><%=Resources.Resource.PaymentDate %></asp:RequiredFieldValidator>
                                                        </div>
                                                    </div>
                                                    <div class="col-sm-4">
                                                        <asp:TextBox ID="txtToDate" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                                        <div class="text-danger">
                                                            <asp:RequiredFieldValidator ID="rfvFilterPaymentDateTo" runat="server"
                                                                ControlToValidate="txtToDate" Display="Dynamic"
                                                                ValidationGroup="FilterCustomerPaymentMaster"><%= Resources.Messages.InputRequired %><%=Resources.Resource.PaymentDate %></asp:RequiredFieldValidator>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-md-4 col-sm-6">
                                                <div class="form-group">
                                                    <label class="col-sm-5 control-label"><%= Resources.Resource.PaymentType %></label>
                                                    <div class="col-sm-7">
                                                        <asp:DropDownList ID="ddlFilterPaymentType" runat="server" CssClass="form-control input-sm"></asp:DropDownList>
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
                                                    <label class="col-sm-5 control-label"><%= Resources.Resource.VerifiedBy %></label>
                                                    <div class="col-sm-7">
                                                        <asp:DropDownList ID="ddlFilterVerifiedBy" runat="server" CssClass="form-control input-sm">
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-md-4 col-sm-6">
                                                <div class="form-group">
                                                    <label class="col-sm-5 control-label"><%= Resources.Resource.Bank %></label>
                                                    <div class="col-sm-7">
                                                        <asp:DropDownList ID="ddlFilterBank" runat="server" CssClass="form-control input-sm"></asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-md-4 col-sm-6">
                                                <div class="form-group">
                                                    <label class="col-sm-5 control-label"><%= Resources.Resource.Amount %></label>
                                                    <div class="col-sm-7">
                                                        <asp:TextBox ID="txtFilterAmount" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                                        <asp:RegularExpressionValidator ID="revFilterAmount"
                                                            ControlToValidate="txtFilterAmount"
                                                            runat="server"
                                                            EnableClientScript="true"
                                                            ValidationExpression="^\d+([,\.]\d{1,2})?$">
                                                            <%= Resources.Messages.InputInvalid %>  <%=Resources.Resource.Amount %></asp:RegularExpressionValidator>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-md-4 col-sm-6">
                                                <div class="form-group">
                                                    <label class="col-sm-5 control-label"><%= Resources.Resource.ChequeNo %></label>
                                                    <div class="col-sm-7">
                                                        <asp:TextBox ID="txtFilterChequeNo" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row">
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
                                                    <label class="col-sm-5 control-label"><%= Resources.Resource.Notes %></label>
                                                    <div class="col-sm-7">
                                                        <asp:TextBox ID="txtFilterNotes" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-md-4 col-sm-6">
                                                <div class="form-group">
                                                    <label class="col-sm-5 control-label"><%= Resources.Resource.User %></label>
                                                    <div class="col-sm-7">
                                                        <asp:DropDownList ID="ddlFilterUser" runat="server" CssClass="form-control input-sm">
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="row">
                                            <div class="col-md-12 text-center">
                                                <asp:Button ID="btnSearch" runat="server" Text="<% $Resources:Resource, btnSearch %>" CssClass="btn btn-primary" ValidationGroup="FilterCustomerPaymentMaster" OnClick="btnSearch_Click" />
                                                &nbsp;
                                                <asp:Button ID="btnReset" runat="server" Text="<% $Resources:Resource, btnReset %>" CssClass="btn btn-default" CausesValidation="false" OnClick="btnReset_Click" />
                                            </div>
                                        </div>
                                    </div>
                                </asp:Panel>
                                <asp:ValidationSummary ID="vsCustomerPaymentMaster" runat="server" DisplayMode="BulletList"
                                    ShowMessageBox="true" ShowSummary="false" ValidationGroup="FilterCustomerPaymentMaster" />
                            </div>
                        </div>
                    </div>

                    <asp:ListView ID="lvCustomerPaymentMaster" runat="server" DataKeyNames="CustomerPaymentMasterId" OnItemCommand="lvCustomerPaymentMaster_ItemCommand" OnItemDataBound="lvCustomerPaymentMaster_ItemDataBound">
                        <LayoutTemplate>
                            <div class="panel panel-default">
                                <div class="table-responsive">
                                    <table id="itemPlaceholderContainer" runat="server" class="table table-hover">
                                        <tr>
                                            <th>
                                                <%= Resources.Resource.Customer %>
                                            </th>
                                            <th>
                                                <%= Resources.Resource.PaymentDate %>
                                            </th>
                                            <th>
                                                <%= Resources.Resource.PaymentType %>
                                            </th>
                                            <th>
                                                <%= Resources.Resource.Amount %>
                                            </th>
                                            <th>
                                                <%= Resources.Resource.ReferenceNo %>
                                            </th>
                                            <th>
                                                <%= Resources.Resource.ChequeNo %>
                                            </th>
                                            <%--   <th>
                                                <%= Resources.Resource.ChequeDate %>
                                            </th>--%>
                                            <th>
                                                <%= Resources.Resource.Bank %>
                                            </th>
                                            <th>
                                                <%= Resources.Resource.Notes %>
                                            </th>
                                            <th style="width: 25px;">
                                                <%= Resources.Resource.VerifiedBy %>
                                            </th>
                                            <th style="width: 25px;">
                                                <%= Resources.Resource.ModifiedBy %>
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
                                    <asp:Literal ID="ltrlCustomer" runat="server"></asp:Literal>
                                    <asp:HyperLink ID="hlnkCustomer" NavigateUrl="#" Target="_blank" runat="server" Visible="false">
                                    </asp:HyperLink>
                                    <br />
                                    <span class="text-muted"><%= Resources.Resource.BankAccountNumber %>:
                                    </span>
                                    <asp:Literal ID="ltrlBankAccountNumber" runat="server"></asp:Literal>
                                </td>
                                <td>
                                    <asp:Literal ID="ltrlPaymentDate" runat="server"></asp:Literal>
                                </td>
                                <td>
                                    <asp:Literal ID="ltrlPaymentType" runat="server"></asp:Literal>
                                </td>
                                <td>
                                    <asp:Literal ID="ltrlAmount" runat="server"></asp:Literal>
                                </td>

                                <td>
                                    <asp:Literal ID="ltrlReferenceNo" runat="server"></asp:Literal>
                                    <br />
                                    <asp:Literal ID="ltrlContracts" runat="server"></asp:Literal>
                                </td>
                                <td>
                                    <asp:Literal ID="ltrlChequeNo" runat="server"></asp:Literal>
                                </td>
                                <%--<td>
                                    <asp:Literal ID="ltrlChequeDate" runat="server"></asp:Literal>
                                </td>--%>
                                <td>
                                    <asp:Literal ID="ltrlBank" runat="server"></asp:Literal>
                                </td>
                                <td>
                                    <asp:Literal ID="ltrlNotes" runat="server"></asp:Literal>
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
                            <cc1:DataPager ID="pgrCustomerPaymentMaster" runat="server" OnPagerCommand="pgrCustomerPaymentMaster_ItemCommand"
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

        <div class="modal fade" id="divcustomerpaymentdetails">
            <div class="modal-dialog wide-modal-dialog">
                <div class="modal-content">
                    <div class="blockui">
                        <asp:UpdatePanel ID="upcustomerpaymentdetails" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Panel ID="pnlcustomerpaymentdetails" runat="server" DefaultButton="btnSaveAndNew">
                                    <asp:HiddenField ID="hdnModelCustomerPayment" runat="server"></asp:HiddenField>
                                    <asp:HiddenField ID="hdnActionCustomerPayment" runat="server"></asp:HiddenField>
                                    <div class="modal-header">
                                        <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                                        <h4 class="modal-title"><%= Resources.Resource.CustomerPaymentAddEditTitle %></h4>
                                    </div>
                                    <div class="modal-body">
                                        <div class="form-horizontal" role="form">
                                            <div class="row">
                                                <asp:HiddenField ID="hdnCustomerPaymentMasterId" runat="server" Visible="false"></asp:HiddenField>
                                                <asp:HiddenField ID="hdnCustomerMasterId" runat="server" Visible="false"></asp:HiddenField>

                                                <div class="form-group">
                                                    <label class="col-sm-3 control-label"><%= Resources.Resource.CustomerIdNo %></label>
                                                    <div class="col-sm-2">
                                                        <asp:TextBox ID="txtSearchCustomerIdNo" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                                        <div class="text-danger">
                                                            <asp:RequiredFieldValidator ID="rfvSearchCustomerIdNo" runat="server" ControlToValidate="txtSearchCustomerIdNo" Display="Dynamic" SetFocusOnError="true" ValidationGroup="searchcustomerdetails"><%= Resources.Messages.InputRequired %><%=Resources.Resource.CustomerIdNo %></asp:RequiredFieldValidator>
                                                        </div>
                                                    </div>
                                                    <label class="col-sm-3 control-label"><%= Resources.Resource.ContractTitle %></label>
                                                    <div class="col-sm-2">
                                                        <asp:TextBox ID="txtFilterContractTitle" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                                    </div>
                                                    <div class="col-sm-1">
                                                        <asp:Button ID="btnSearchCustomerIdNo" runat="server" CssClass="btn btn-default" Text="<% $Resources:Resource, btnSearch %>" ValidationGroup="searchcustomerdetails" OnClick="btnSearchCustomerIdNo_Click"></asp:Button>
                                                        <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="BulletList"
                                                            ShowMessageBox="true" ShowSummary="false" ValidationGroup="searchcustomerdetails" />
                                                    </div>
                                                </div>
                                                <div class="form-group">
                                                    <div class="col-lg-6">
                                                        <label class="col-sm-4 control-label"><%= Resources.Resource.IdNo %></label>
                                                        <div class="col-sm-7">
                                                            <asp:TextBox ID="txtIdNo" CssClass="form-control input-sm" runat="server" Enabled="false"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                    <div class="col-lg-6">
                                                        <div class="form-group">
                                                            <label class="col-sm-4 control-label"><%= Resources.Resource.CustomerName %></label>
                                                            <div class="col-sm-7">
                                                                <asp:TextBox ID="txtCustomerName" runat="server" CssClass="form-control input-sm" Enabled="false"></asp:TextBox>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="col-lg-12">
                                                    <div class="form-group">
                                                        <label class="col-sm-1 control-label"></label>
                                                        <div class="col-sm-11">
                                                            <asp:Button ID="btnClearList" runat="server" Style="display: none;" OnClick="btnClearList_Click" />
                                                            <asp:ListView ID="lvContractMaster" runat="server" DataKeyNames="ContractMasterId" OnItemDataBound="lvContractMaster_ItemDataBound">
                                                                <LayoutTemplate>
                                                                    <div class="panel panel-default" id="divContract">
                                                                        <div class="table-responsive">
                                                                            <table id="itemPlaceholderContainer2" runat="server" class="table table-hover">
                                                                                <tr>
                                                                                    <th style="width: 25px;">
                                                                                        <asp:CheckBox ID="chkHeader" runat="server" onclick="javascript:ListHeaderCheckChanged(this, 'itemPlaceholderContainer2');Calculate();" />
                                                                                    </th>
                                                                                    <th>
                                                                                        <%= Resources.Resource.FileNo %>
                                                                                    </th>
                                                                                    <th>
                                                                                        <%= Resources.Resource.ContractFormTitle %>
                                                                                    </th>
                                                                                    <th>
                                                                                        <%= Resources.Resource.Installment %>
                                                                                    </th>
                                                                                    <th>
                                                                                        <%= Resources.Resource.Amount %>
                                                                                    </th>
                                                                                    <th style="width: 25px;"></th>
                                                                                    <th>
                                                                                        <%= Resources.Resource.Last3Installments %>
                                                                                    </th>
                                                                                    <th></th>
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
                                                                            <asp:CheckBox ID="chkSelect" runat="server" onClick="Calculate();"></asp:CheckBox>
                                                                        </td>
                                                                        <td>
                                                                            <asp:Literal ID="ltrlFileNo" runat="server"></asp:Literal>
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
                                                                            <span class="text-muted"><%= Resources.Resource.Notes %>:
                                                                            </span>
                                                                            <asp:Literal ID="ltrlNotes" runat="server"></asp:Literal>
                                                                        </td>
                                                                        <td>
                                                                            <span class="text-muted"><%= Resources.Resource.InstallmentDate %>:
                                                                            <asp:Literal ID="ltrlInstallmentDate" runat="server"></asp:Literal>
                                                                                <br />
                                                                                <span class="text-muted"><%= Resources.Resource.PaymentAmount %>:
                                                                            <asp:Literal ID="ltrlInstallmentAmount" runat="server"></asp:Literal>
                                                                                    <br />
                                                                                    <span class="text-muted"><%= Resources.Resource.PendingAmount %>:
                                                                            <asp:Literal ID="ltrlPendingAmount" runat="server"></asp:Literal>
                                                                        </td>
                                                                        <td>
                                                                            <asp:Label ID="lblDueAmount" runat="server"></asp:Label>
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="txtLvAmount" runat="server" CssClass="chkAmountAdd" onblur="Calculate();"></asp:TextBox>
                                                                        </td>
                                                                        <td>
                                                                            <asp:Literal ID="ltrlLast3Installments" runat="server"></asp:Literal>
                                                                        </td>


                                                                    </tr>

                                                                </ItemTemplate>

                                                            </asp:ListView>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="col-lg-6">

                                                    <div class="form-group">
                                                        <label class="col-sm-4 control-label"><%= Resources.Resource.PaymentDate %></label>
                                                        <div class="col-sm-7">
                                                            <asp:TextBox ID="txtPaymentDate" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                                            <div class="text-danger">
                                                                <asp:RequiredFieldValidator ID="rfvPaymentDate" runat="server" ControlToValidate="txtPaymentDate" Display="Dynamic"
                                                                    ValidationGroup="customerpaymentdetails"><%= Resources.Messages.InputRequired %><%=Resources.Resource.PaymentDate %></asp:RequiredFieldValidator>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="form-group">
                                                        <label class="col-sm-4 control-label"><%= Resources.Resource.PaymentType %></label>
                                                        <div class="col-sm-7">
                                                            <asp:DropDownList ID="ddlPaymentType" runat="server" OnChange="Validations()" CssClass="form-control input-sm"></asp:DropDownList>
                                                            <div class="text-danger">
                                                                <asp:RequiredFieldValidator ID="rfvPaymentType" runat="server" ControlToValidate="ddlPaymentType" Display="Dynamic"
                                                                    SetFocusOnError="true" InitialValue="" ValidationGroup="customerpaymentdetails"><%= Resources.Messages.InputSelect %><%=Resources.Resource.PaymentType %></asp:RequiredFieldValidator>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div id="dvBank">
                                                        <div class="form-group">
                                                            <label class="col-sm-4 control-label"><%= Resources.Resource.ChequeNo %></label>
                                                            <div class="col-sm-7">
                                                                <asp:TextBox ID="txtChequeNo" runat="server" CssClass="form-control input-sm" MaxLength="50"></asp:TextBox>
                                                            </div>
                                                        </div>
                                                        <div class="form-group" style="display: none;">
                                                            <label class="col-sm-4 control-label"><%= Resources.Resource.ChequeDate %></label>
                                                            <div class="col-sm-7">
                                                                <asp:TextBox ID="txtChequeDate" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                                            </div>
                                                        </div>
                                                        <div class="form-group">
                                                            <label class="col-sm-4 control-label"><%= Resources.Resource.Bank %></label>
                                                            <div class="col-sm-7">
                                                                <asp:DropDownList ID="ddlBank" runat="server" CssClass="form-control input-sm"></asp:DropDownList>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div id="dvOthers">
                                                        <div class="form-group">
                                                            <label class="col-sm-4 control-label"><%= Resources.Resource.ReferenceNo %></label>
                                                            <div class="col-sm-7">
                                                                <asp:TextBox ID="txtReferenceNo" runat="server" CssClass="form-control input-sm" MaxLength="50"></asp:TextBox>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div id="dvBankAccountNumber">
                                                        <div class="form-group">
                                                            <label class="col-sm-4 control-label"><%= Resources.Resource.BankAccountNumber %></label>
                                                            <div class="col-sm-7">
                                                                <asp:DropDownList ID="ddlBankAccountNumber" runat="server" CssClass="form-control input-sm"></asp:DropDownList>
                                                                <%--<div class="text-danger">
                                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="ddlBankAccountNumber" Display="Dynamic"
                                                                        SetFocusOnError="true" InitialValue="" ValidationGroup="customerpaymentdetails"><%= Resources.Messages.InputSelect %><%=Resources.Resource.BankAccountNumber %></asp:RequiredFieldValidator>
                                                                </div>--%>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="col-lg-6">
                                                    <div class="form-group">
                                                        <label class="col-sm-4 control-label"><%= Resources.Resource.TotalAmount %></label>
                                                        <div class="col-sm-7">
                                                            <asp:TextBox ID="txtTotalLvAmount" runat="server" CssClass="TotalAdditions" Enabled="false"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                    <div class="form-group">
                                                        <label class="col-sm-4 control-label"><%= Resources.Resource.Amount %></label>
                                                        <div class="col-sm-7">
                                                            <asp:TextBox ID="txtAmount" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                                            <div class="text-danger">
                                                                <asp:RequiredFieldValidator ID="rfvAmount" runat="server" ControlToValidate="txtAmount" Display="Dynamic" SetFocusOnError="true"
                                                                    ValidationGroup="customerpaymentdetails"><%= Resources.Messages.InputRequired %><%=Resources.Resource.Amount %></asp:RequiredFieldValidator>
                                                                <asp:CompareValidator ID="cmvAmount" runat="server"
                                                                    ControlToValidate="txtAmount" Display="Dynamic" Operator="DataTypeCheck" SetFocusOnError="true"
                                                                    Type="Double" ValidationGroup="customerpaymentdetails"><%= Resources.Messages.InputInvalid %><%=Resources.Resource.Amount %></asp:CompareValidator>
                                                                <asp:CompareValidator ID="cmv2Amount" runat="server"
                                                                    ControlToValidate="txtAmount" Display="Dynamic" Operator="Equal" SetFocusOnError="true"
                                                                    ControlToCompare="txtTotalLvAmount" ValidationGroup="customerpaymentdetails"><%= Resources.Messages.InputInvalid %><%=Resources.Resource.Amount %></asp:CompareValidator>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="form-group">
                                                        <label class="col-sm-4 control-label"><%= Resources.Resource.Notes %></label>
                                                        <div class="col-sm-7">
                                                            <asp:TextBox ID="txtNotes" runat="server" Rows="3" CssClass="form-control input-sm" onKeyDown="javascript:CheckTextAreaMaxLength(this, event, 4000);" TextMode="MultiLine"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="modal-footer">
                                            <asp:Button ID="btnSaveAndNew" runat="server" Text="<% $Resources:Resource, btnSaveAndNew %>" CssClass="btn btn-primary" OnClick="btnSave_Click" ValidationGroup="customerpaymentdetails" />

                                            <asp:Button ID="btnSave" runat="server" Text="<% $Resources:Resource, btnSave %>" CssClass="btn btn-primary" OnClick="btnSave_Click" ValidationGroup="customerpaymentdetails" />

                                            <asp:Button ID="btnClose" runat="server" Text="<% $Resources:Resource, btnClose %>" CssClass="btn btn-default" CausesValidation="false" data-dismiss="modal" />

                                        </div>
                                </asp:Panel>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="lvCustomerPaymentMaster" EventName="ItemCommand" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </div>
        </div>
        <asp:ValidationSummary ID="vscustomerpaymentdetails" runat="server" DisplayMode="BulletList"
            ShowMessageBox="true" ShowSummary="false" ValidationGroup="customerpaymentdetails" />

    </div>
    <script type="text/javascript">
        $(document).ready(function () {
            SetControlsPicker();
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(SetControlsPicker);
        });

        function SetControlsPicker() {
            SetDatePicker(Prefix + "txtPaymentDate", new Date(1900, 01, 01), new Date());

            SetDatePicker(Prefix + "txtChequeDate");
            SearchCustomer();
        }
    </script>
    <script type="text/javascript">
        $(document).ready(function () {
            SetFilterControlsPicker();
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(SetFilterControlsPicker);
        });

        function SetFilterControlsPicker() {
            SetRangeDatePicker(Prefix + "txtFromDate", Prefix + "txtToDate");
        }
    </script>
    <script type="text/javascript">
        $(document).ready(function () {
            if ($("#" + Prefix + "hdnModelCustomerPayment").val() == "") {
                ClearCustomerPaymentMaster();
            }
            SetDialogShowHideCustomerPaymentMaster();
            ShowHideDialogCustomerPaymentMaster();
        });

        function SetDialogShowHideCustomerPaymentMaster() {
            $("#divcustomerpaymentdetails").on("hidden.bs.modal", function () {
                ClearCustomerPaymentMaster();
                $("#" + Prefix + "btnClearList").click();
            })
            $("#divcustomerpaymentdetails").on("show.bs.modal", function () {
                SetCustomerPaymentMaster();
            })
            $("#divcustomerpaymentdetails").on("shown.bs.modal", function () {
                $("#" + Prefix + "txtSearchCustomerIdNo").focus();
            })
        }
        function ShowHideDialogCustomerPaymentMaster() {
            try {
                SetCustomerPaymentMaster();
                if ($("#" + Prefix + "hdnModelCustomerPayment").val() == "show") {
                    $("#divcustomerpaymentdetails").modal("show");
                }
                else if ($("#" + Prefix + "hdnModelCustomerPayment").val() == "hide") {
                    ClearCustomerPaymentMaster();
                    $("#divcustomerpaymentdetails").modal("hide");
                }
                else if ($("#" + Prefix + "hdnModelCustomerPayment").val() == "clear") {
                    ClearCustomerPaymentMaster();
                    $("#divcustomerpaymentdetails").modal("show");
                    $("#" + Prefix + "txtSearchCustomerIdNo").focus();
                }
                $("#" + Prefix + "hdnModelCustomerPayment").val("");
                return false;
            }
            catch (ex) {
                alert(ex);
                return false;
            }
        }
        function ClearCustomerPaymentMaster() {
            for (i = 0; i < Page_Validators.length; i++) {
                Page_Validators[i].style.display = "none";
            }



        }
        function SetCustomerPaymentMaster() {
            for (i = 0; i < Page_Validators.length; i++) {
                Page_Validators[i].errormessage = Page_Validators[i].innerHTML;
            }
            if ($("#" + Prefix + "hdnActionCustomerPayment").val() == "") {
                $("#" + Prefix + "btnSaveAndNew").css("visibility", "visible");
                $("#" + Prefix + "btnSave").val('<%= GetGlobalResourceObject("Resource", "btnSave") %>');
            }
            else {
                $("#" + Prefix + "btnSaveAndNew").css("visibility", "hidden");
                $("#" + Prefix + "btnSave").val('<%= GetGlobalResourceObject("Resource", "btnUpdate") %>');
            }
            Validations();
        }
        function Validations() {
            var SelectedPaymentType = $("#" + Prefix + "ddlPaymentType").val();
            $("#dvBank").css("display", "none");
            $("#dvOthers").css("display", "none");
            $("#dvBankAccountNumber").css("display", "none");
            if (SelectedPaymentType == 2)    //for Bank
            {
                $("#dvBank").css("display", "block");
                $("#dvOthers").css("display", "none");
                $("#dvBankAccountNumber").css("display", "block");
            }
            else if (SelectedPaymentType > 2)   // for other types
            {
                $("#dvOthers").css("display", "block");
                $("#dvBank").css("display", "none");
                $("#dvBankAccountNumber").css("display", "block");
            }
        }
        function Calculate() {
            var txtList = $('.chkAmountAdd');
            var TotalAdditions = 0;
            for (var i = 0; i < txtList.length; i++) {
                var chk = txtList[i].id.replace("txtLvAmount", "chkSelect");
                if ($("#" + chk).prop("checked") == true) {
                    if (isNaN(parseFloat(txtList[i].value)) == false) {
                        TotalAdditions += parseFloat(txtList[i].value);
                    }
                }
            }
            $(".TotalAdditions").val(TotalAdditions.toFixed(0));
        }

        function SearchCustomer() {
            $("#" + Prefix + "txtSearchCustomerIdNo").autocomplete({
                source: function (request, response) {
                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: "contractdetails.aspx/GetCustomerMaster",
                        data: "{'customer':'" + $("#" + Prefix + "txtSearchCustomerIdNo").val() + "'}",
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
                    $("#" + Prefix + "txtSearchCustomerIdNo").val(ui.item.value);
                    $("#" + Prefix + "btnSearchCustomerIdNo").click();
                },
            });
        }
    </script>
</asp:Content>
