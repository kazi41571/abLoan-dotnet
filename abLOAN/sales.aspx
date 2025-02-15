<%@ Page Language="C#" MasterPageFile="Site.Master" AutoEventWireup="true" CodeBehind="sales.aspx.cs"
    Inherits="abLOAN.sales" %>

<%@ Register Assembly="CustomDataPager" Namespace="CustomDataPager" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script type="text/javascript">
        $(document).ready(function () {
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequest);
        });
        function EndRequest(sender, args) {
            ShowHideDialogSalesMaster();
        }
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphPage" runat="server">
    <div id="divsales">
        <div class="blockui">
            <asp:UpdatePanel ID="upsales" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <div class="row">
                        <div style="justify-content: space-between; display: flex;">
                            <div style="padding: 0 15px;">
                                <h4>
                                    <i class="fa fa-th"></i>&nbsp;<%= Resources.Resource.SalesPageTitle %>&nbsp;
                                    <small>
                                        <asp:Label ID="lblRecords" runat="server" Visible="false"></asp:Label></small>
                                    &nbsp;
                                    <a id="aFilter" class="btn btn-default btn-sm" href="" style="text-decoration: none;" onclick="javascript:return ShowHideFilter('divFilter', false);"><%= Resources.Resource.Filter %>&nbsp;&nbsp;<span class="caret"></span></a>
                                </h4>
                            </div>
                            <div style="padding: 0 15px;">
                                <asp:Button ID="btnDownload" runat="server" Text="<% $Resources:Resource, Download %>" CssClass="btn btn-info" OnClick="btnDownload_Click" />
                                &nbsp;
                                <asp:Button ID="btnNew" runat="server" Text="<% $Resources:Resource, btnNew %>" CssClass="btn btn-primary" data-toggle="modal" data-target="#divsalesdetails" OnClientClick="javascript:return GetMaxBillNo();" />
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
                                                    <label class="col-sm-4 control-label"><%= Resources.Resource.SalesDate %></label>
                                                    <div class="col-sm-4">
                                                        <asp:TextBox ID="txtFromDate" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                                        <div class="text-danger">
                                                            <asp:RequiredFieldValidator ID="rfvFilterSalesDateFrom" runat="server"
                                                                ControlToValidate="txtFromDate" Display="Dynamic"
                                                                ValidationGroup="FilterSalesMaster"><%= Resources.Messages.InputRequired %> <%=Resources.Resource.SalesDate %></asp:RequiredFieldValidator>
                                                        </div>
                                                    </div>
                                                    <div class="col-sm-4">
                                                        <asp:TextBox ID="txtToDate" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                                        <div class="text-danger">
                                                            <asp:RequiredFieldValidator ID="rfvFilterSalesDateTo" runat="server"
                                                                ControlToValidate="txtToDate" Display="Dynamic"
                                                                ValidationGroup="FilterSalesMaster"><%= Resources.Messages.InputRequired %> <%=Resources.Resource.SalesDate %></asp:RequiredFieldValidator>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-md-4 col-sm-6">
                                                <div class="form-group">
                                                    <label class="col-sm-5 control-label"><%= Resources.Resource.Item %></label>
                                                    <div class="col-sm-7">
                                                        <asp:DropDownList ID="ddlFilterItem" runat="server" CssClass="form-control input-sm"></asp:DropDownList>
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
                                                    <label class="col-sm-5 control-label"><%= Resources.Resource.BillNo %></label>
                                                    <div class="col-sm-7">
                                                        <asp:TextBox ID="txtFilterBillNo" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-md-4 col-sm-6">
                                                <div class="form-group">
                                                    <label class="col-sm-5 control-label"><%= Resources.Resource.ReceiptNo %></label>
                                                    <div class="col-sm-7">
                                                        <asp:TextBox ID="txtFilterReceiptNo" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="row">
                                            <div class="col-md-12 text-center">
                                                <asp:Button ID="btnSearch" runat="server" Text="<% $Resources:Resource, btnSearch %>" CssClass="btn btn-primary" ValidationGroup="FilterSalesMaster" OnClick="btnSearch_Click" />
                                                &nbsp;
                                                <asp:Button ID="btnReset" runat="server" Text="<% $Resources:Resource, btnReset %>" CssClass="btn btn-default" CausesValidation="false" OnClick="btnReset_Click" />
                                            </div>
                                        </div>
                                    </div>
                                </asp:Panel>
                                <asp:ValidationSummary ID="vsSalesMaster" runat="server" DisplayMode="BulletList"
                                    ShowMessageBox="true" ShowSummary="false" ValidationGroup="FilterSalesMaster" />
                            </div>
                        </div>
                    </div>

                    <asp:ListView ID="lvSalesMaster" runat="server" DataKeyNames="SalesMasterId" OnDataBound="lvSalesMaster_DataBound" OnItemCommand="lvSalesMaster_ItemCommand" OnItemDataBound="lvSalesMaster_ItemDataBound">
                        <LayoutTemplate>
                            <div class="panel panel-default">
                                <div class="table-responsive">
                                    <table id="itemPlaceholderContainer" runat="server" class="table table-hover">
                                        <tr>
                                            <th>
                                                <%= Resources.Resource.CustomerName %>
                                            </th>
                                            <th>
                                                <%= Resources.Resource.SalesDate %>
                                            </th>
                                            <th style="width: 100px;">
                                                <%= Resources.Resource.Item %>
                                            </th>
                                            <th style="width: 50px;">
                                                <%= Resources.Resource.Quantity %>
                                            </th>
                                            <th style="width: 50px;">
                                                <%= Resources.Resource.SalesRate %>
                                            </th>
                                            <th style="width: 50px;">
                                                <%= Resources.Resource.GrossAmount %>
                                            </th>
                                            <th style="width: 50px;">
                                                <%= Resources.Resource.DiscountAmount %>
                                            </th>
                                            <th style="width: 50px;">
                                                <%= Resources.Resource.VatAmount %>
                                            </th>
                                            <th style="width: 50px;">
                                                <%= Resources.Resource.NetAmount %>
                                            </th>
                                            <th style="width: 50px;">
                                                <%= Resources.Resource.Fees %>
                                            </th>
                                            <th>
                                                <%= Resources.Resource.Notes %>
                                            </th>
                                            <th>
                                                <%= Resources.Resource.BillNo %>
                                            </th>
                                            <th>
                                                <%= Resources.Resource.ReceiptNo %>
                                            </th>
                                            <th>
                                                <%= Resources.Resource.InstallmentStart %>
                                            </th>
                                            <th>
                                                <%= Resources.Resource.ContractAmount %>
                                            </th>
                                            <th>
                                                <%= Resources.Resource.InstallmentAmount %>
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
                                        <tr class="tablefooter" style='font-family: Calibri, monospace; font-size: 20px;'>
                                            <th></th>
                                            <th></th>
                                            <th>Total</th>
                                            <th>
                                                <asp:Literal ID="ltrlTotalQuantity" runat="server"></asp:Literal>
                                            </th>
                                            <th></th>
                                            <th>
                                                <asp:Literal ID="ltrlTotalGrossAmount" runat="server"></asp:Literal>
                                            </th>
                                            <th>
                                                <asp:Literal ID="ltrlTotalDiscountAmount" runat="server"></asp:Literal>
                                            </th>
                                            <th>
                                                <asp:Literal ID="ltrlTotalVat" runat="server"></asp:Literal>
                                            </th>
                                            <th>
                                                <asp:Literal ID="ltrlTotalNetAmount" runat="server"></asp:Literal>
                                            </th>
                                            <th>
                                                <asp:Literal ID="ltrlTotalFees" runat="server"></asp:Literal>
                                            </th>
                                            <th></th>
                                            <th></th>
                                            <th></th>
                                            <th></th>
                                            <th></th>
                                            <th></th>
                                            <th></th>
                                            <th></th>
                                            <th></th>
                                            <th></th>
                                            <th></th>
                                            <th></th>
                                        </tr>
                                    </table>
                                </div>
                            </div>
                        </LayoutTemplate>
                        <ItemTemplate>
                            <tr valign="top">
                                <td>
                                    <asp:Literal ID="ltrlCustomerName" runat="server"></asp:Literal>
                                </td>
                                <td>
                                    <asp:Literal ID="ltrlSalesDate" runat="server"></asp:Literal>
                                </td>
                                <td colspan="7">
                                    <asp:ListView ID="lvSalesItemTran" runat="server" OnItemDataBound="lvSalesItemTran_ItemDataBound">
                                        <LayoutTemplate>
                                            <div class="panel panel-default">
                                                <div class="table-responsive">
                                                    <table id="itemPlaceholderContainer" runat="server" class="table table-hover">
                                                        <tr id="itemPlaceholder" runat="server">
                                                        </tr>
                                                    </table>
                                                </div>
                                            </div>
                                        </LayoutTemplate>
                                        <ItemTemplate>
                                            <tr valign="top">
                                                <td style="width: 100px;">
                                                    <asp:Literal ID="ltrlItem" runat="server"></asp:Literal>
                                                </td>
                                                <td style="width: 50px;">
                                                    <asp:Literal ID="ltrlQuantity" runat="server"></asp:Literal>
                                                </td>
                                                <td style="width: 50px;">
                                                    <asp:Literal ID="ltrlSalesRate" runat="server"></asp:Literal>
                                                </td>
                                                <td style="width: 50px;">
                                                    <asp:Literal ID="ltrlGrossAmount" runat="server"></asp:Literal>
                                                </td>
                                                <td style="width: 50px;">
                                                    <asp:Literal ID="ltrlDiscountAmount" runat="server"></asp:Literal>
                                                </td>
                                                <td style="width: 50px;">
                                                    <asp:Literal ID="ltrlVat" runat="server"></asp:Literal>
                                                </td>
                                                <td style="width: 50px;">
                                                    <asp:Literal ID="ltrlNetAmount" runat="server"></asp:Literal>
                                                </td>
                                                <td style="width: 50px;">
                                                    <asp:Literal ID="ltrlFees" runat="server"></asp:Literal>
                                                </td>
                                            </tr>

                                        </ItemTemplate>
                                        <EmptyDataTemplate>
                                            <div class="alert alert-info">
                                                No record found.
                                            </div>
                                        </EmptyDataTemplate>
                                    </asp:ListView>
                                </td>
                                <td>
                                    <asp:Literal ID="ltrlNotes" runat="server"></asp:Literal>
                                </td>
                                <td>
                                    <asp:Literal ID="ltrlBillNo" runat="server"></asp:Literal>
                                </td>
                                <td>
                                    <asp:Literal ID="ltrlReceiptNo" runat="server"></asp:Literal>
                                </td>
                                <td>
                                    <asp:Literal ID="ltrlContractStartDate" runat="server"></asp:Literal>
                                </td>
                                <td>
                                    <asp:Literal ID="ltrlContractAmount" runat="server"></asp:Literal>
                                </td>
                                <td>
                                    <asp:Literal ID="ltrlInstallmentAmount" runat="server"></asp:Literal>
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
                                    <asp:LinkButton ID="lbtnPrint" runat="server" CommandName="PrintReceipt" CssClass="btn btn-info btn-sm fa fa-print" title="Print"></asp:LinkButton>
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
                            <cc1:DataPager ID="pgrSalesMaster" runat="server" OnPagerCommand="pgrSalesMaster_ItemCommand"
                                Visible="false" CssClass="pagination" CurrentPageCssClass="active"></cc1:DataPager>
                        </div>
                    </div>
                </ContentTemplate>
                <Triggers>
                    <%--<asp:AsyncPostBackTrigger ControlID="btnSaveAndNew" EventName="Click" />--%>
                    <asp:PostBackTrigger ControlID="btnDownload" />
                    <asp:PostBackTrigger ControlID="btnSave" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
        <div class="modal fade" id="divsalesdetails">
            <div class="modal-dialog modal-lg">
                <div class="modal-content">
                    <div class="blockui">
                        <asp:UpdatePanel ID="upsalesdetails" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Panel ID="pnlsalesdetails" runat="server" DefaultButton="btnSave">
                                    <asp:HiddenField ID="hdnModelSales" runat="server"></asp:HiddenField>
                                    <asp:HiddenField ID="hdnActionSales" runat="server"></asp:HiddenField>
                                    <div class="modal-header">
                                        <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                                        <h4 class="modal-title"><%= Resources.Resource.SalesFormTitle %></h4>
                                    </div>
                                    <div class="modal-body">
                                        <div class="form-horizontal" role="form">
                                            <asp:HiddenField ID="hdnSalesMasterId" runat="server" Visible="false"></asp:HiddenField>
                                            <div class="form-group">
                                                <label class="col-sm-4 control-label"><%= Resources.Resource.SalesDate %></label>
                                                <div class="col-sm-7">
                                                    <asp:TextBox ID="txtSalesDate" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                                    <div class="text-danger">
                                                        <asp:RequiredFieldValidator ID="rfvSalesDate" runat="server" ControlToValidate="txtSalesDate" Display="Dynamic"
                                                            ValidationGroup="salesdetails"><%= Resources.Messages.InputRequired %>  <%=Resources.Resource.SalesDate %></asp:RequiredFieldValidator>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <label class="col-sm-4 control-label"><%= Resources.Resource.CustomerName %></label>
                                                <div class="col-sm-7">
                                                    <asp:TextBox ID="txtCustomerName" runat="server" Rows="3" CssClass="form-control input-sm"></asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <label class="col-sm-4 control-label"><%= Resources.Resource.IdNo %></label>
                                                <div class="col-sm-7">
                                                    <asp:TextBox ID="txtCustomerIdNo" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <label class="col-sm-4 control-label"><%= Resources.Resource.Address %></label>
                                                <div class="col-sm-7">
                                                    <asp:TextBox ID="txtCustomerAddress" runat="server" Rows="3" CssClass="form-control input-sm"></asp:TextBox>
                                                </div>
                                            </div>
                                            <table>
                                                <tr>
                                                    <td>
                                                        <label class="col-sm-11 control-label"><%= Resources.Resource.Item %></label></td>
                                                    <td>
                                                        <label class="col-sm-11 control-label"><%= Resources.Resource.Quantity %></label></td>
                                                    <td>
                                                        <label class="col-sm-11 control-label"><%= Resources.Resource.SalesRate %></label></td>
                                                    <td>
                                                        <label class="col-sm-11 control-label"><%= Resources.Resource.DiscountAmount %></label></td>
                                                    <td style="display: none;">
                                                        <label class="col-sm-11 control-label"><%= Resources.Resource.Vat %></label></td>
                                                    <td>
                                                        <label class="col-sm-11 control-label"><%= Resources.Resource.VatAmount %></label></td>
                                                    <td>
                                                        <label class="col-sm-11 control-label"><%= Resources.Resource.NetAmount %></label></td>
                                                    <td>
                                                        <label class="col-sm-11 control-label"><%= Resources.Resource.Fees %></label></td>
                                                    <td style="width: 25px;"></td>
                                                </tr>
                                                <tr>
                                                    <td style="display: block;">
                                                        <div class="form-group">
                                                            <div class="col-sm-11" valign="top">
                                                                <asp:DropDownList ID="ddlItem" AutoPostBack="true" OnSelectedIndexChanged="ddlItem_SelectedIndexChanged" runat="server" CssClass="form-control input-sm"></asp:DropDownList>
                                                                <%--<div class="text-danger">
                                                                    <asp:RequiredFieldValidator ID="rfvItem" runat="server"
                                                                        ControlToValidate="ddlItem" Display="Dynamic" SetFocusOnError="true" InitialValue="" ValidationGroup="salesdetails"><%= Resources.Messages.InputRequired %>      <%=Resources.Resource.linktoItemMasterId %></asp:RequiredFieldValidator>
                                                                </div>--%>
                                                            </div>
                                                        </div>
                                                    </td>
                                                    <td>
                                                        <div class="form-group">
                                                            <div class="col-sm-11">
                                                                <asp:TextBox ID="txtQuantity" onchange="javascript:Calculate();" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                                                <div class="text-danger">
                                                                    <asp:RequiredFieldValidator ID="rfvQuantity" runat="server" SetFocusOnError="true" ControlToValidate="txtQuantity"
                                                                        ValidationGroup="salesdetails"><%= Resources.Messages.InputRequired %> <%=Resources.Resource.Quantity %></asp:RequiredFieldValidator>
                                                                    <asp:CompareValidator ID="cmvQuantity" runat="server"
                                                                        ControlToValidate="txtQuantity" Display="Dynamic" Operator="DataTypeCheck" SetFocusOnError="true"
                                                                        Type="Integer" ValidationGroup="salesdetails"><%= Resources.Messages.InputInvalid %>  <%=Resources.Resource.Quantity %></asp:CompareValidator>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </td>
                                                    <td>
                                                        <div class="form-group">
                                                            <div class="col-sm-11">
                                                                <asp:TextBox ID="txtSalesRate" onchange="javascript:Calculate();" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                                                <div class="text-danger">
                                                                    <asp:RequiredFieldValidator ID="rfvSalesRate" runat="server" SetFocusOnError="true" ControlToValidate="txtSalesRate"
                                                                        ValidationGroup="salesdetails"><%= Resources.Messages.InputRequired %>  <%=Resources.Resource.SalesRate %></asp:RequiredFieldValidator>
                                                                    <asp:CompareValidator ID="cmvSalesRate" runat="server"
                                                                        ControlToValidate="txtSalesRate" Display="Dynamic" Operator="DataTypeCheck" SetFocusOnError="true"
                                                                        Type="Double" ValidationGroup="salesdetails"><%= Resources.Messages.InputInvalid %> <%=Resources.Resource.SalesRate %></asp:CompareValidator>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </td>
                                                    <td>
                                                        <div class="form-group">
                                                            <div class="col-sm-11">
                                                                <asp:TextBox ID="txtDiscountAmount" onchange="javascript:Calculate();" runat="server" CssClass="form-control input-sm" Text="0.00"></asp:TextBox>
                                                                <div class="text-danger">
                                                                    <asp:RequiredFieldValidator ID="rfvDiscountAmount" runat="server" SetFocusOnError="true" ControlToValidate="txtDiscountAmount"
                                                                        ValidationGroup="salesdetails"><%= Resources.Messages.InputRequired %>  <%=Resources.Resource.DiscountAmount %></asp:RequiredFieldValidator>
                                                                    <asp:CompareValidator ID="cmvDiscountAmount" runat="server"
                                                                        ControlToValidate="txtDiscountAmount" Display="Dynamic" Operator="DataTypeCheck" SetFocusOnError="true"
                                                                        Type="Double" ValidationGroup="salesdetails"><%= Resources.Messages.InputInvalid %> <%=Resources.Resource.DiscountAmount %></asp:CompareValidator>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </td>
                                                    <td style="display: none;">
                                                        <div class="form-group">
                                                            <div class="col-sm-11">
                                                                <asp:TextBox ID="txtVat" onchange="javascript:Calculate();" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                                                <div class="text-danger">
                                                                    <asp:RequiredFieldValidator ID="rfvVat" runat="server" SetFocusOnError="true" ControlToValidate="txtVat"
                                                                        ValidationGroup="salesdetails"><%= Resources.Messages.InputRequired %>  <%=Resources.Resource.Vat %></asp:RequiredFieldValidator>
                                                                    <asp:CompareValidator ID="cvVat" runat="server"
                                                                        ControlToValidate="txtVat" Display="Dynamic" Operator="DataTypeCheck" SetFocusOnError="true"
                                                                        Type="Double" ValidationGroup="salesdetails"><%= Resources.Messages.InputInvalid %> <%=Resources.Resource.Vat %></asp:CompareValidator>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </td>
                                                    <td>
                                                        <div class="form-group">
                                                            <div class="col-sm-11">
                                                                <asp:TextBox ID="txtVatAmount" onchange="javascript:CalculateVat();" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                                                <div class="text-danger">
                                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" SetFocusOnError="true" ControlToValidate="txtVatAmount"
                                                                        ValidationGroup="salesdetails"><%= Resources.Messages.InputRequired %>  <%=Resources.Resource.Vat %></asp:RequiredFieldValidator>
                                                                    <asp:CompareValidator ID="CompareValidator1" runat="server"
                                                                        ControlToValidate="txtVatAmount" Display="Dynamic" Operator="DataTypeCheck" SetFocusOnError="true"
                                                                        Type="Double" ValidationGroup="salesdetails"><%= Resources.Messages.InputInvalid %> <%=Resources.Resource.Vat %></asp:CompareValidator>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </td>
                                                    <td>
                                                        <div class="form-group">
                                                            <div class="col-sm-11">
                                                                <asp:TextBox ID="txtNetAmount" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                                                <div class="text-danger">
                                                                    <asp:RequiredFieldValidator ID="rfvNetAmount" runat="server" SetFocusOnError="true" ControlToValidate="txtNetAmount"
                                                                        ValidationGroup="salesdetails"><%= Resources.Messages.InputRequired %>  <%=Resources.Resource.NetAmount %></asp:RequiredFieldValidator>
                                                                    <asp:CompareValidator ID="cvNetAmount" runat="server"
                                                                        ControlToValidate="txtNetAmount" Display="Dynamic" Operator="DataTypeCheck" SetFocusOnError="true"
                                                                        Type="Double" ValidationGroup="salesdetails"><%= Resources.Messages.InputInvalid %> <%=Resources.Resource.NetAmount %></asp:CompareValidator>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </td>
                                                    <td>
                                                        <div class="form-group">
                                                            <div class="col-sm-11">
                                                                <asp:TextBox ID="txtFees" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                                                <div class="text-danger">
                                                                    <asp:RequiredFieldValidator ID="rfvFees" runat="server" SetFocusOnError="true" ControlToValidate="txtFees"
                                                                        ValidationGroup="salesdetails"><%= Resources.Messages.InputRequired %>  <%=Resources.Resource.Fees %></asp:RequiredFieldValidator>
                                                                    <asp:CompareValidator ID="cvFees" runat="server"
                                                                        ControlToValidate="txtFees" Display="Dynamic" Operator="DataTypeCheck" SetFocusOnError="true"
                                                                        Type="Double" ValidationGroup="salesdetails"><%= Resources.Messages.InputInvalid %> <%=Resources.Resource.Fees %></asp:CompareValidator>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </td>
                                                    <td style="display: block;">
                                                        <div class="form-group">
                                                            <div class="col-sm-11">
                                                                <asp:LinkButton ID="lbtnAdd" runat="server" CommandName="Add" CssClass="btn btn-primary btn-sm fa fa-plus" title="Add" OnClick="lbtnAdd_Click"></asp:LinkButton>
                                                            </div>
                                                        </div>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="7">
                                                        <asp:Button ID="btnClearList" runat="server" Style="display: none;" OnClick="btnClearList_Click" />
                                                        <asp:ListView ID="lvSalesItems" runat="server" OnItemDataBound="lvSalesItems_ItemDataBound" OnItemCommand="lvSalesItems_ItemCommand">
                                                            <LayoutTemplate>
                                                                <div class="panel panel-default">
                                                                    <div class="table-responsive">
                                                                        <table id="itemPlaceholderContainer" runat="server" class="table table-hover">
                                                                            <tr id="itemPlaceholder" runat="server">
                                                                            </tr>
                                                                        </table>
                                                                    </div>
                                                                </div>
                                                            </LayoutTemplate>
                                                            <ItemTemplate>
                                                                <tr valign="top">
                                                                    <td>
                                                                        <asp:Literal ID="ltrlItem" runat="server"></asp:Literal>
                                                                    </td>
                                                                    <td>
                                                                        <asp:Literal ID="ltrlQuantity" runat="server"></asp:Literal>
                                                                    </td>
                                                                    <td>
                                                                        <asp:Literal ID="ltrlSalesRate" runat="server"></asp:Literal>
                                                                    </td>
                                                                    <td>
                                                                        <asp:Literal ID="ltrlGrossAmount" runat="server"></asp:Literal>
                                                                    </td>
                                                                    <td>
                                                                        <asp:Literal ID="ltrlDiscountAmount" runat="server"></asp:Literal>
                                                                    </td>
                                                                    <td>
                                                                        <asp:Literal ID="ltrlVat" runat="server"></asp:Literal>
                                                                    </td>
                                                                    <td>
                                                                        <asp:Literal ID="ltrlNetAmount" runat="server"></asp:Literal>
                                                                    </td>
                                                                    <td>
                                                                        <asp:Literal ID="ltrlFees" runat="server"></asp:Literal>
                                                                    </td>
                                                                    <td style="width: 25px;">
                                                                        <asp:LinkButton ID="lbtnDelete" runat="server" CommandName="DeleteRecord" OnClientClick="javascript:return ConfirmDelete(this);" CssClass="btn btn-danger btn-sm fa fa-trash" title="Delete"></asp:LinkButton>
                                                                    </td>
                                                                </tr>

                                                            </ItemTemplate>
                                                            <EmptyDataTemplate>
                                                                <div class="alert alert-info">
                                                                    No record found.
                                                                </div>
                                                            </EmptyDataTemplate>
                                                        </asp:ListView>
                                                    </td>
                                                </tr>
                                            </table>
                                            <div class="form-group">
                                                <label class="col-sm-4 control-label"><%= Resources.Resource.Notes %></label>
                                                <div class="col-sm-7">
                                                    <asp:TextBox ID="txtNotes" runat="server" Rows="3" CssClass="form-control input-sm" onKeyDown="javascript:CheckTextAreaMaxLength(this, event, 4000);" TextMode="MultiLine"></asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <label class="col-sm-4 control-label"><%= Resources.Resource.InstallmentStart %></label>
                                                <div class="col-sm-7">
                                                    <asp:TextBox ID="txtContractStartDate" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <label class="col-sm-4 control-label"><%= Resources.Resource.ContractAmount %></label>
                                                <div class="col-sm-7">
                                                    <asp:TextBox ID="txtContractAmount" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                                    <div class="text-danger">
                                                        <asp:CompareValidator ID="cvContractAmount" runat="server"
                                                            ControlToValidate="txtContractAmount" Display="Dynamic" Operator="DataTypeCheck" SetFocusOnError="true"
                                                            Type="Double" ValidationGroup="salesdetails"><%= Resources.Messages.InputInvalid %> <%=Resources.Resource.ContractAmount %></asp:CompareValidator>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <label class="col-sm-4 control-label"><%= Resources.Resource.InstallmentAmount %></label>
                                                <div class="col-sm-7">
                                                    <asp:TextBox ID="txtInstallmentAmount" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                                    <div class="text-danger">
                                                        <asp:CompareValidator ID="cvInstallmentAmount" runat="server"
                                                            ControlToValidate="txtInstallmentAmount" Display="Dynamic" Operator="DataTypeCheck" SetFocusOnError="true"
                                                            Type="Double" ValidationGroup="salesdetails"><%= Resources.Messages.InputInvalid %> <%=Resources.Resource.InstallmentAmount %></asp:CompareValidator>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <label class="col-sm-4 control-label"><%= Resources.Resource.BillNo %></label>
                                                <div class="col-sm-7">
                                                    <asp:TextBox ID="txtBillNo" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <label class="col-sm-4 control-label"><%= Resources.Resource.ReceiptNo %></label>
                                                <div class="col-sm-7">
                                                    <asp:TextBox ID="txtReceiptNo" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="modal-footer">
                                        <asp:Button ID="btnSaveAndPrint" runat="server" Text="<% $Resources:Resource, btnSaveAndPrint %>" CssClass="btn btn-primary" OnClick="btnSaveAndPrint_Click" ValidationGroup="salesdetails" />

                                        <%--<asp:Button ID="btnSaveAndNew" runat="server" Text="<% $Resources:Resource, btnSaveAndNew %>" CssClass="btn btn-primary" OnClick="btnSave_Click" ValidationGroup="salesdetails" />--%>

                                        <asp:Button ID="btnSave" runat="server" Text="<% $Resources:Resource, btnSave %>" CssClass="btn btn-primary" OnClick="btnSave_Click" ValidationGroup="salesdetails" />

                                        <asp:Button ID="btnClose" runat="server" Text="<% $Resources:Resource, btnClose %>" CssClass="btn btn-default" CausesValidation="false" data-dismiss="modal" />

                                    </div>
                                </asp:Panel>
                            </ContentTemplate>
                            <Triggers>
                                <asp:PostBackTrigger ControlID="lvSalesMaster" />
                                <asp:PostBackTrigger ControlID="btnSaveAndPrint" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </div>
        </div>
        <asp:ValidationSummary ID="vssalesdetails" runat="server" DisplayMode="BulletList" ShowMessageBox="true" ShowSummary="false" ValidationGroup="salesdetails" />

    </div>
    <script type="text/javascript">
        $(document).ready(function () {
            SetControlsPicker();
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(SetControlsPicker);
        });

        function SetControlsPicker() {
            SetDatePicker(Prefix + "txtSalesDate");
            SetDatePicker(Prefix + "txtContractStartDate");
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
            if ($("#" + Prefix + "hdnModelSales").val() == "") {
                ClearSalesMaster();
            }
            SetDialogShowHideSalesMaster();
            ShowHideDialogSalesMaster();
            SearchCustomer();
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(SearchCustomer);
        });

        function CalculateVat() {
            var salesRate = parseFloat($("#" + Prefix + "txtSalesRate").val());
            var quantity = parseFloat($("#" + Prefix + "txtQuantity").val());
            var vatAmount = parseFloat($("#" + Prefix + "txtVatAmount").val());

            var grossAmount = salesRate * quantity;
            var vat = (vatAmount * 100) / grossAmount;

            $("#" + Prefix + "txtVat").val(vat.toFixed(2));

            Calculate();
        }

        function Calculate() {
            var salesRate = parseFloat($("#" + Prefix + "txtSalesRate").val());
            var quantity = parseFloat($("#" + Prefix + "txtQuantity").val());
            var discount = parseFloat($("#" + Prefix + "txtDiscountAmount").val());
            var vat = parseFloat($("#" + Prefix + "txtVat").val());
            //var fees = parseFloat($("#" + Prefix + "txtFees").val());

            var grossAmount = (salesRate * quantity) - discount;
            var vatAmount = grossAmount * vat;
            vatAmount = vatAmount / 100.0;

            $("#" + Prefix + "txtVatAmount").val(vatAmount.toFixed(2));

            //var netAmount = grossAmount - fees - vatAmount;
            var netAmount = grossAmount + vatAmount;

            $("#" + Prefix + "txtNetAmount").val(netAmount.toFixed(2));
        }

        function SetDialogShowHideSalesMaster() {
            $("#divsalesdetails").on("hidden.bs.modal", function () {
                ClearSalesMaster();
                $("#" + Prefix + "btnClearList").click();
            })
            $("#divsalesdetails").on("show.bs.modal", function () {
                SetSalesMaster();
            })
            $("#divsalesdetails").on("shown.bs.modal", function () {
                //$("#" + Prefix + "txtSalesDate").focus();
            })
        }

        function ShowHideDialogSalesMaster() {
            try {
                SetSalesMaster();
                if ($("#" + Prefix + "hdnModelSales").val() == "show") {
                    $("#divsalesdetails").modal("show");
                }
                else if ($("#" + Prefix + "hdnModelSales").val() == "hide") {
                    ClearSalesMaster();
                    $("#divsalesdetails").modal("hide");
                }
                else if ($("#" + Prefix + "hdnModelSales").val() == "clear") {
                    ClearSalesMaster();
                    $("#divsalesdetails").modal("show");
                    //$("#" + Prefix + "txtSalesDate").focus();
                }
                $("#" + Prefix + "hdnModelSales").val("");
                return false;
            }
            catch (ex) {
                alert(ex);
                return false;
            }
        }

        function ClearSalesMaster() {
            for (i = 0; i < Page_Validators.length; i++) {
                Page_Validators[i].style.display = "none";
            }
            $("#" + Prefix + "hdnActionSales").val("");
            $("#" + Prefix + "txtCustomerName").val("");
            $("#" + Prefix + "txtCustomerIdNo").val("");
            $("#" + Prefix + "txtCustomerAddress").val("");
            $("#" + Prefix + "txtSalesDate").val(GetDateToString(new Date()));
            $("#" + Prefix + "ddlItem").prop("selectedIndex", 0);
            $("#" + Prefix + "txtQuantity").val("1");
            $("#" + Prefix + "txtSalesRate").val("0.00");
            $("#" + Prefix + "txtVat").val("0.00");
            $("#" + Prefix + "txtVatAmount").val("0.00");
            $("#" + Prefix + "txtFees").val("0.00");
            $("#" + Prefix + "txtNetAmount").val("0.00");
            $("#" + Prefix + "txtNotes").val("");
            $("#" + Prefix + "txtContractStartDate").val("");
            $("#" + Prefix + "txtContractAmount").val("0.00");
            $("#" + Prefix + "txtInstallmentAmount").val("0.00");
            $("#" + Prefix + "txtBillNo").val("");
            $("#" + Prefix + "txtReceiptNo").val("");
        }

        function SetSalesMaster() {
            for (i = 0; i < Page_Validators.length; i++) {
                Page_Validators[i].errormessage = Page_Validators[i].innerHTML;
            }
            if ($("#" + Prefix + "hdnActionSales").val() == "") {
                $("#" + Prefix + "btnSaveAndNew").css("visibility", "visible");
                $("#" + Prefix + "btnSave").val('<%= GetGlobalResourceObject("Resource", "btnSave") %>');
            }
            else {
                $("#" + Prefix + "btnSaveAndNew").css("visibility", "hidden");
                $("#" + Prefix + "btnSave").val('<%= GetGlobalResourceObject("Resource", "btnUpdate") %>');
            }
        }

        function SearchCustomer() {
            $("#" + Prefix + "txtCustomerName").autocomplete({
                source: function (request, response) {
                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: "contractdetails.aspx/GetCustomerMaster",
                        data: "{'customer':'" + $("#" + Prefix + "txtCustomerName").val() + "'}",
                        dataType: "json",
                        success: function (data) {
                            response($.map(data.d, function (item) {
                                return {
                                    value: item.CustomerName
                                }
                            }))
                        },
                    });
                },
                minLength: 3,
                select: function (event, ui) {
                    $("#" + Prefix + "txtCustomerName").val(ui.item.value);
                },
            });
        }

        function GetMaxBillNo() {
            try {
                PageMethods.GetMaxBillNo(OnSuccessGetBillNo, OnErrorGetBillNo);
                return false;

            } catch (ex) {
                alert(ex);
                return false;
            }
        }
        function OnSuccessGetBillNo(result) {
            try {
                if (result == null) {
                    alert(ErrorMessage);
                    return false;
                }

                $("#" + Prefix + "txtBillNo").val(result.BillNo);
            }
            catch (ex) {
                alert(ex);
                return false;
            }
        }
        function OnErrorGetBillNo(error) {
            alert(ErrorMessage + "\n\nError: " + error._statusCode + " - " + error._message);
        }
    </script>

</asp:Content>
