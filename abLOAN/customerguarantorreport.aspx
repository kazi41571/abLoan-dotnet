<%@ Page Language="C#" MasterPageFile="Site.Master" AutoEventWireup="true" CodeBehind="customerguarantorreport.aspx.cs"
    Inherits="abLOAN.customerguarantorreport" %>

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
    <div id="divcustomer">
        <div class="blockui">
            <asp:UpdatePanel ID="upcustomer" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <div class="row">
                        <div style="justify-content: space-between; display: flex;">
                            <div style="padding: 0 15px;">
                                <h4>
                                    <i class="fa fa-th"></i>&nbsp;<%= Resources.Resource.CustomerGuarantorReportFormTitle %>&nbsp;
                                    <small>
                                        <asp:Label ID="lblRecords" runat="server" Visible="false"></asp:Label></small>
                                    &nbsp;
                                    <a id="aFilter" class="btn btn-default btn-sm" href="" style="text-decoration: none;" onclick="javascript:return ShowHideFilter('divFilter', false);"><%= Resources.Resource.Filter %>&nbsp;&nbsp;<span class="caret"></span></a>
                                </h4>
                            </div>
                            <div style="padding: 0 15px;">
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
                                                    <label class="col-sm-5 control-label"><%= Resources.Resource.PhoneMobile %></label>
                                                    <div class="col-sm-7">
                                                        <asp:TextBox ID="txtFilterPhoneMobile" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-md-4 col-sm-6">
                                                <div class="form-group">
                                                    <label class="col-sm-5 control-label"><%= Resources.Resource.Guarantor %></label>
                                                    <div class="col-sm-7">
                                                        <asp:TextBox ID="txtFilterGuarantors" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="row">
                                            <div class="col-md-12 text-center">
                                                <asp:Button ID="btnSearch" runat="server" Text="<% $Resources:Resource, btnSearch %>" CssClass="btn btn-primary" ValidationGroup="FilterCustomerMaster" OnClick="btnSearch_Click" />
                                                &nbsp;
                                                <asp:Button ID="btnReset" runat="server" Text="<% $Resources:Resource, btnReset %>" CssClass="btn btn-default" CausesValidation="false" OnClick="btnReset_Click" />
                                            </div>
                                        </div>
                                    </div>
                                </asp:Panel>
                                <asp:ValidationSummary ID="vsCustomerMaster" runat="server" DisplayMode="BulletList"
                                    ShowMessageBox="true" ShowSummary="false" ValidationGroup="FilterCustomerMaster" />
                            </div>
                        </div>
                    </div>

                    <asp:ListView ID="lvCustomerMaster" runat="server" DataKeyNames="CustomerMasterId" OnItemDataBound="lvCustomerMaster_ItemDataBound">
                        <LayoutTemplate>
                            <div class="panel panel-default">
                                <div class="table-responsive">
                                    <table id="itemPlaceholderContainer" runat="server" class="table table-hover">
                                        <tr>
                                            <th>
                                                <%= Resources.Resource.CustomerName %>
                                            </th>
                                            <th>
                                                <%= Resources.Resource.Mobile %>
                                            </th>
                                            <th>
                                                <%= Resources.Resource.Guarantor %>
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
                                    <span class="text-muted"><%= Resources.Resource.Name %>:</span>
                                    <asp:Literal ID="ltrlCustomerName" runat="server"></asp:Literal>
                                </td>
                                <td>
                                    <span class="text-muted"><%= Resources.Resource.Mobile1 %>:</span>
                                    <asp:Literal ID="ltrlMobile1" runat="server"></asp:Literal>
                                    <br />
                                    <span class="text-muted"><%= Resources.Resource.Mobile2 %>:</span>
                                    <asp:Literal ID="ltrlMobile2" runat="server"></asp:Literal>
                                    <br />
                                    <span class="text-muted"><%= Resources.Resource.Mobile3 %>:</span>
                                    <asp:Literal ID="ltrlMobile3" runat="server"></asp:Literal>
                                </td>
                                <td>
                                    <asp:Literal ID="ltrlGuarantors" runat="server"></asp:Literal>
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
                            <cc1:DataPager ID="pgrCustomerMaster" runat="server" OnPagerCommand="pgrCustomerMaster_ItemCommand"
                                Visible="false" CssClass="pagination" PageSize="20" CurrentPageCssClass="active"></cc1:DataPager>
                        </div>
                    </div>

                </ContentTemplate>

            </asp:UpdatePanel>
        </div>




    </div>



</asp:Content>
