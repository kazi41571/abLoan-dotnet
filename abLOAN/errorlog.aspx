<%@ Page Language="C#" MasterPageFile="Site.Master" AutoEventWireup="true" CodeBehind="errorlog.aspx.cs"
    Inherits="abLOAN.errorlog" %>

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
    <div id="diverrorlog">
        <asp:HiddenField ID="hdnFilter" runat="server" Value="0" ClientIDMode="Static" />
        <div class="blockui">
            <asp:UpdatePanel ID="uperrorlog" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <div class="row">
                        <div class="col-xs-9">
                            <h4>
                                <i class="fa fa-th"></i>&nbsp;Error Log <small>
                                    <asp:Label ID="lblRecords" runat="server" Visible="false"></asp:Label></small>
                                &nbsp; <a id="aFilter" class="btn btn-default btn-sm" href="" style="text-decoration: none;"
                                    onclick="javascript:return ShowHideFilter('divFilter', false);">Show filter&nbsp;&nbsp;<span
                                        class="caret"></span></a>
                            </h4>
                        </div>
                        <div class="col-xs-3 text-right">
                            <asp:Button ID="btnDelete" runat="server" Text="Delete" CssClass="btn btn-danger"
                                OnClick="btnDelete_Click" OnClientClick="javascript:return ConfirmDeleteSelected(this, 'lvErrorLog');" />&nbsp;
                        </div>
                    </div>
                    <div id="divFilter" style="display: none;">
                        <div class="panel panel-default">
                            <div class="panel-body">
                                <asp:Panel ID="pnlFilter" runat="server" DefaultButton="btnSearch">
                                    <div class="form-horizontal">
                                        <div class="row">
                                            <div class="col-md-4 col-sm-6">
                                                <div class="form-group">
                                                    <label for="inputErrorLogId" class="col-sm-5 control-label">
                                                        Error ID</label>
                                                    <div class="col-sm-7">
                                                        <asp:TextBox ID="txtFilterErrorLogId" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                                        <div class="text-danger">
                                                            <asp:CompareValidator ID="cmvFilterErrorLogId" runat="server" ErrorMessage="Please enter valid Error Log Id"
                                                                ControlToValidate="txtFilterErrorLogId" Display="Dynamic" Operator="DataTypeCheck"
                                                                SetFocusOnError="true" Text="Please enter valid Error Log Id" Type="Integer"
                                                                ValidationGroup="FilterErrorLog"></asp:CompareValidator>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-md-4 col-sm-6">
                                                <div class="form-group">
                                                    <label for="inputErrorDateTime" class="col-sm-5 control-label">
                                                        Error Date Time</label>
                                                    <div class="col-sm-7">
                                                        <asp:TextBox ID="txtFilterErrorDateTime" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-md-4 col-sm-6">
                                                <div class="form-group">
                                                    <label for="inputErrorMessage" class="col-sm-5 control-label">
                                                        Error Message</label>
                                                    <div class="col-sm-7">
                                                        <asp:TextBox ID="txtFilterErrorMessage" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-md-12 text-center">
                                                <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="btn btn-primary"
                                                    ValidationGroup="FilterErrorLog" OnClick="btnSearch_Click" />
                                                &nbsp;
                                                <asp:Button ID="btnReset" runat="server" Text="Reset" CssClass="btn btn-default"
                                                    CausesValidation="false" OnClick="btnReset_Click" />
                                            </div>
                                        </div>
                                    </div>
                                </asp:Panel>
                                <asp:ValidationSummary ID="vsErrorLog" runat="server" DisplayMode="BulletList" ShowMessageBox="true"
                                    ShowSummary="false" ValidationGroup="FilterErrorLog" />
                            </div>
                        </div>
                    </div>
                    <asp:ListView ID="lvErrorLog" runat="server" DataKeyNames="ErrorLogId" OnItemCommand="lvErrorLog_ItemCommand"
                        OnItemDataBound="lvErrorLog_ItemDataBound">
                        <LayoutTemplate>
                            <div class="panel panel-default">
                                <div class="table-responsive">
                                    <table id="itemPlaceholderContainer" runat="server" class="table table-hover">
                                        <tr>
                                            <th style="width: 25px;">
                                                <asp:CheckBox ID="chkHeader" runat="server" onclick="javascript:ListHeaderCheckChanged(this, 'itemPlaceholderContainer');" />
                                            </th>
                                            <th style="width: 70px;">Error ID
                                            </th>
                                            <th style="width: 100px;">DateTime
                                            </th>
                                            <th>Error Details
                                            </th>
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
                                    <asp:CheckBox ID="chkSelect" runat="server" onclick="javascript:ListSelectCheckChanged(this, 'itemPlaceholderContainer');" />
                                </td>
                                <td>
                                    <asp:Label ID="lblErrorLogId" runat="server" Font-Bold="true" CssClass="text-primary"></asp:Label>
                                </td>
                                <td>
                                    <asp:Literal ID="ltrlErrorDateTime" runat="server"></asp:Literal>
                                </td>
                                <td>
                                    <asp:Label ID="lblErrorMessage" runat="server" CssClass="text-danger"></asp:Label>
                                    <br />
                                    <asp:Literal ID="ltrlErrorStackTrace" runat="server"></asp:Literal>
                                </td>
                                <td>
                                    <asp:LinkButton ID="lbtnDelete" runat="server" CommandName="DeleteRecord" OnClientClick="javascript:return ConfirmDelete(this);"
                                        CssClass="btn btn-danger btn-sm fa fa-trash" title="Delete"></asp:LinkButton>
                                </td>
                            </tr>
                        </ItemTemplate>
                        <EmptyDataTemplate>
                            <div class="alert alert-info">
                                No record found.
                            </div>
                        </EmptyDataTemplate>
                    </asp:ListView>
                    <div class="row">
                        <div class="col-md-12 text-center">
                            <cc1:DataPager ID="pgrErrorLog" runat="server" OnPagerCommand="pgrErrorLog_ItemCommand"
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
            SetDatePicker(Prefix + "txtFilterErrorDateTime");
        }
    </script>
</asp:Content>
