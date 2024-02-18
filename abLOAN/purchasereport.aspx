<%@ Page Language="C#" MasterPageFile="Site.Master" AutoEventWireup="true" CodeBehind="purchasereport.aspx.cs"
    Inherits="abLOAN.purchasereport" %>

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
                                    <i class="fa fa-th"></i>&nbsp;<%= Resources.Resource.PurchaseReportPageTitle %>&nbsp;
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

                    <asp:ListView ID="lvPurchaseSalesTransactionsMaster" runat="server" DataKeyNames="TranId" OnDataBound="lvPurchaseSalesTransactionsMaster_DataBound" OnItemDataBound="lvPurchaseSalesTransactionsMaster_ItemDataBound">
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
                                            <th>
                                                <%= Resources.Resource.Quantity %> <%= Resources.Resource.PurchasePageTitle %> 
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
                                            <th>
                                                <%= Resources.Resource.Vat %>
                                            </th>
                                            <th>
                                                <%= Resources.Resource.Fees %>
                                            </th>
                                            <th>
                                                <%= Resources.Resource.NetAmount %> <%= Resources.Resource.PurchasePageTitle %> 
                                            </th>


                                        </tr>

                                        <tr id="itemPlaceholder" runat="server">
                                        </tr>

                                        <tr class="tablefooter" style='font-family: Calibri, monospace; font-size: 20px;'>
                                            <th></th>

                                           <%-- <th >
                                                <%= Resources.Resource.OpeningQuantity %>
                                            </th>

                                            <th>
                                                <asp:Literal ID="ltrlOpeningQuantity" runat="server"></asp:Literal>
                                            </th>--%>
                                           <th></th>
                                            <th></th>
                                             <th>
                                                <asp:Literal ID="ltrlTotalPurchaseQuantity" runat="server"></asp:Literal>
                                            </th>
                                            <%--<th>
                                                <asp:Literal ID="ltrlCurrentQuantity" runat="server"></asp:Literal>
                                            </th>--%>
                                            <th>
                                                <asp:Literal ID="ltrlTotalGrossAmount" runat="server"></asp:Literal>
                                            </th>
                                            <th></th>
                                            <th>
                                                <asp:Literal ID="ltrlTotalVat" runat="server"></asp:Literal>
                                            </th>
                                            <th>
                                                <asp:Literal ID="ltrlTotalFees" runat="server"></asp:Literal>
                                            </th>
                                            <th>
                                                <asp:Literal ID="ltrlTotalPurchaseNetAmount" runat="server"></asp:Literal>
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
                                <td>
                                    <asp:Literal ID="ltrlItem" runat="server"></asp:Literal>
                                </td>
                                <td>
                                    <asp:Literal ID="ltrlPurchaseQuantity" runat="server"></asp:Literal>
                                </td>
                                <%--<td>
                                    <asp:Literal ID="ltrlCurrentQuantity" runat="server"></asp:Literal>
                                </td>--%>
                                <td>
                                    <asp:Literal ID="ltrlRate" runat="server"></asp:Literal>
                                </td>
                                <td>
                                    <asp:Literal ID="ltrlGrossAmount" runat="server"></asp:Literal>
                                </td>
                                <td>
                                    <asp:Literal ID="ltrlVat" runat="server"></asp:Literal>
                                </td>
                                <td>
                                    <asp:Literal ID="ltrlFees" runat="server"></asp:Literal>
                                </td>

                                <td>
                                    <asp:Literal ID="ltrlPurchaseNetAmount" runat="server"></asp:Literal>
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
