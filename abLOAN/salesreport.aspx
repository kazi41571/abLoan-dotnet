<%@ Page Language="C#" MasterPageFile="Site.Master" AutoEventWireup="true" CodeBehind="salesreport.aspx.cs"
    Inherits="abLOAN.salesreport" %>

<%@ Register Assembly="CustomDataPager" Namespace="CustomDataPager" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script type="text/javascript">
        $(document).ready(function () {
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequest);

        });
        function EndRequest(sender, args) {



        }
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphPage" runat="server">
    <div id="divpurchasesalestransactions">
        <div class="blockui">
            <asp:UpdatePanel ID="uppurchasesalestransactions" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <div class="row">
                        <div style="justify-content: space-between; display: flex;">
                            <div style="padding: 0 15px;">
                                <h4>
                                    <i class="fa fa-th"></i>&nbsp;<%= Resources.Resource.SalesReportPageTitle %>&nbsp;
                                    <small>
                                        <asp:Label ID="lblRecords" runat="server" Visible="false"></asp:Label></small>
                                    &nbsp;
                                    <a id="aFilter" class="btn btn-default btn-sm" href="" style="text-decoration: none;" onclick="javascript:return ShowHideFilter('divFilter', false);"><%= Resources.Resource.Filter %>&nbsp;&nbsp;<span class="caret"></span></a>
                                </h4>
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
                                                    <label class="col-sm-4 control-label"><%= Resources.Resource.TranDate %></label>
                                                    <div class="col-sm-4">
                                                        <asp:TextBox ID="txtFromDate" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                                        <div class="text-danger">
                                                            <asp:RequiredFieldValidator ID="rfvFilterTranDateFrom" runat="server"
                                                                ControlToValidate="txtFromDate" Display="Dynamic"
                                                                ValidationGroup="FilterPurchaseSalesTransactionsMaster"><%= Resources.Messages.InputRequired %> <%=Resources.Resource.TranDate %></asp:RequiredFieldValidator>
                                                        </div>
                                                    </div>
                                                    <div class="col-sm-4">
                                                        <asp:TextBox ID="txtToDate" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                                        <div class="text-danger">
                                                            <asp:RequiredFieldValidator ID="rfvFilterTranDateTo" runat="server"
                                                                ControlToValidate="txtToDate" Display="Dynamic"
                                                                ValidationGroup="FilterPurchaseSalesTransactionsMaster"><%= Resources.Messages.InputRequired %> <%=Resources.Resource.TranDate %></asp:RequiredFieldValidator>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-md-4 col-sm-6">
                                                <div class="form-group">
                                                    <label class="col-sm-5 control-label"><%= Resources.Resource.Item %></label>
                                                    <div class="col-sm-7">
                                                        <asp:DropDownList ID="ddlFilterItem" runat="server" CssClass="form-control input-sm"></asp:DropDownList>
                                                        <div class="text-danger">
                                                            <asp:RequiredFieldValidator ID="rfvFilterItem" runat="server"
                                                                ControlToValidate="ddlFilterItem" Display="Dynamic"
                                                                ValidationGroup="FilterPurchaseSalesTransactionsMaster"><%= Resources.Messages.InputRequired %> <%=Resources.Resource.Item %></asp:RequiredFieldValidator>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-md-4 col-sm-6">
                                                <div class="form-group">
                                                    <label class="col-sm-5 control-label"><%= Resources.Resource.CustomerIdNo %></label>
                                                    <div class="col-sm-7">
                                                        <asp:TextBox ID="txtFilterCustomer" runat="server"></asp:TextBox>
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
                                <asp:ValidationSummary ID="vsPurchaseSalesTransactionsMaster" runat="server" DisplayMode="BulletList"
                                    ShowMessageBox="true" ShowSummary="false" ValidationGroup="FilterPurchaseSalesTransactionsMaster" />
                            </div>
                        </div>
                    </div>

                    <asp:ListView ID="lvPurchaseSalesTransactionsMaster" runat="server" DataKeyNames="SalesMasterId" OnDataBound="lvPurchaseSalesTransactionsMaster_DataBound" OnItemDataBound="lvPurchaseSalesTransactionsMaster_ItemDataBound">
                        <LayoutTemplate>
                            <div class="panel panel-default">
                                <div class="table-responsive">
                                    <table id="itemPlaceholderContainer" runat="server" class="table table-hover">
                                        <tr>
                                            <th>
                                                <%= Resources.Resource.CustomerName %>
                                            </th>
                                            <th>
                                                <%= Resources.Resource.TranDate %>
                                            </th>
                                            <th>
                                                <%= Resources.Resource.Item %>
                                            </th>
                                            <%--<th>
                                                <%= Resources.Resource.Quantity %> <%= Resources.Resource.PurchasePageTitle %> 
                                            </th>--%>
                                            <th>
                                                <%= Resources.Resource.Quantity %> <%= Resources.Resource.SalesPageTitle %> 
                                            </th>
                                            <%--<th>
                                                <%= Resources.Resource.CurrentQuantity %> 
                                            </th>--%>
                                            <th>
                                                <%= Resources.Resource.Rate %>
                                            </th>
                                            <th>
                                                <%= Resources.Resource.GrossAmount %>
                                            </th>
                                            <%--<th>
                                                <%= Resources.Resource.Vat %>
                                            </th>--%>
                                            <th>
                                                <%= Resources.Resource.Fees %>
                                            </th>
                                            <%--<th>
                                                <%= Resources.Resource.NetAmount %> <%= Resources.Resource.PurchasePageTitle %> 
                                            </th>--%>
                                            <th>
                                                <%= Resources.Resource.NetAmount %> <%= Resources.Resource.SalesPageTitle %> 
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

                                        </tr>

                                        <tr id="itemPlaceholder" runat="server">
                                        </tr>

                                        <tr class="tablefooter" style='font-family: Calibri, monospace; font-size: 20px;'>
                                            <th></th>

                                            <th>
                                                <%--<%= Resources.Resource.OpeningQuantity %>--%>
                                            </th>

                                            <th>
                                                <%--<asp:Literal ID="ltrlOpeningQuantity" runat="server"></asp:Literal>--%>
                                            </th>
                                            <%--<th>
                                                <asp:Literal ID="ltrlTotalPurchaseQuantity" runat="server"></asp:Literal>
                                            </th>--%>
                                            <th>
                                                <asp:Literal ID="ltrlTotalQuantity" runat="server"></asp:Literal>
                                            </th>
                                            <%--<th>
                                                <asp:Literal ID="ltrlCurrentQuantity" runat="server"></asp:Literal>
                                            </th>--%>
                                            <th>
                                                <asp:Literal ID="ltrlTotalGrossAmount" runat="server"></asp:Literal>
                                            </th>
                                            <th>
                                                <%--<asp:Literal ID="ltrlTotalVat" runat="server"></asp:Literal>--%>
                                            </th>
                                            <th>
                                                <asp:Literal ID="ltrlTotalFees" runat="server"></asp:Literal>
                                            </th>
                                            <%--<th>
                                                <asp:Literal ID="ltrlTotalPurchaseNetAmount" runat="server"></asp:Literal>
                                            </th>--%>
                                            <th>
                                                <asp:Literal ID="ltrlTotalNetAmount" runat="server"></asp:Literal>
                                            </th>
                                            <th></th>
                                            <th>
                                                <asp:Literal ID="ltrlTotalContractAmount" runat="server"></asp:Literal>
                                            </th>
                                            <th>
                                                <asp:Literal ID="ltrlTotalInstallmentAmount" runat="server"></asp:Literal>
                                            </th>
                                        </tr>

                                    </table>
                                </div>
                            </div>
                        </LayoutTemplate>
                        <ItemTemplate>

                            <tr id="MainTableRow" valign="top" runat="server">
                                <td>
                                    <asp:Literal ID="ltrlCustomerName" runat="server"></asp:Literal>
                                </td>
                                <td>
                                    <asp:Literal ID="ltrlTranDate" runat="server"></asp:Literal>
                                </td>
                                <td colspan="6">
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
                                                <%--<td>
                                                    <asp:Literal ID="ltrlVat" runat="server"></asp:Literal>
                                                </td>--%>
                                                <td>
                                                    <asp:Literal ID="ltrlFees" runat="server"></asp:Literal>
                                                </td>
                                                <td>
                                                    <asp:Literal ID="ltrlNetAmount" runat="server"></asp:Literal>
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
                                    <asp:Literal ID="ltrlContractStartDate" runat="server"></asp:Literal>
                                </td>
                                <td>
                                    <asp:Literal ID="ltrlContractAmount" runat="server"></asp:Literal>
                                </td>
                                <td>
                                    <asp:Literal ID="ltrlInstallmentAmount" runat="server"></asp:Literal>
                                </td>
                            </tr>

                        </ItemTemplate>
                        <ItemSeparatorTemplate>
                            <tr>
                                <td></td>
                            </tr>
                        </ItemSeparatorTemplate>
                        <EmptyDataTemplate>
                            <div class="alert alert-info">
                                <%= Resources.Resource.NoRecordMessage %>
                            </div>
                        </EmptyDataTemplate>
                    </asp:ListView>
                    <div class="row">
                        <div class="col-md-12 text-center">
                            <cc1:DataPager ID="pgrPurchaseSalesTransactionsMaster" runat="server" OnPagerCommand="pgrPurchaseSalesTransactionsMaster_ItemCommand"
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


</asp:Content>
