<%@ Page Language="C#" MasterPageFile="Site.Master" AutoEventWireup="true" CodeBehind="zakaahpayment.aspx.cs"
    Inherits="abLOAN.zakaahpayment" %>

<%@ Register Assembly="CustomDataPager" Namespace="CustomDataPager" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script type="text/javascript">
        $(document).ready(function () {
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequest);
            SetFilter();
        });
        function EndRequest(sender, args) {

            ShowHideDialogZakaahPaymentMaster();
            SetFilter();
        }
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphPage" runat="server">
    <div id="divzakaahpayment">
        <div class="blockui">
            <asp:UpdatePanel ID="upzakaahpayment" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <div class="row">
                        <div style="justify-content: space-between; display: flex;">
                            <div style="padding: 0 15px;">
                                <h4>
                                    <i class="fa fa-th"></i>&nbsp;<%= Resources.Resource.ZakaahPaymentPageTitle %>&nbsp;
                                    <small>
                                        <asp:Label ID="lblRecords" runat="server" Visible="false"></asp:Label></small>
                                    &nbsp;
                                    <a id="aFilter" class="btn btn-default btn-sm" href="" style="text-decoration: none;" onclick="javascript:return ShowHideFilter('divFilter', false);"><%= Resources.Resource.Filter %>&nbsp;&nbsp;<span class="caret"></span></a>
                                </h4>
                            </div>
                            <div style="padding: 0 15px;">
                                <asp:Button ID="btnNew" runat="server" Text="<% $Resources:Resource, btnNew %>" CssClass="btn btn-primary" data-toggle="modal" data-target="#divzakaahpaymentdetails" OnClientClick="javascript:return false;" />
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
                                                    <label class="col-sm-4 control-label"><%= Resources.Resource.PaymentDate %></label>
                                                    <div class="col-sm-4">
                                                        <asp:TextBox ID="txtFilterFromDate" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                                        <div class="text-danger">
                                                            <asp:RequiredFieldValidator ID="rfvFilterPaymentDateFrom" runat="server"
                                                                ControlToValidate="txtFilterFromDate" Display="Dynamic"
                                                                ValidationGroup="FilterZakaahPaymentMaster"><%= Resources.Messages.InputRequired %> <%=Resources.Resource.FromDate %></asp:RequiredFieldValidator>
                                                        </div>
                                                    </div>
                                                    <div class="col-sm-4">
                                                        <asp:TextBox ID="txtFilterToDate" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                                        <div class="text-danger">
                                                            <asp:RequiredFieldValidator ID="rfvFilterPaymentDateTo" runat="server"
                                                                ControlToValidate="txtFilterToDate" Display="Dynamic"
                                                                ValidationGroup="FilterZakaahPaymentMaster"><%= Resources.Messages.InputRequired %> <%=Resources.Resource.ToDate %></asp:RequiredFieldValidator>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="row">
                                            <div class="col-md-12 text-center">
                                                <asp:Button ID="btnSearch" runat="server" Text="<% $Resources:Resource, btnSearch %>" CssClass="btn btn-primary" ValidationGroup="FilterZakaahPaymentMaster" OnClick="btnSearch_Click" />
                                                &nbsp;
                                                <asp:Button ID="btnReset" runat="server" Text="<% $Resources:Resource, btnReset %>" CssClass="btn btn-default" CausesValidation="false" OnClick="btnReset_Click" />
                                            </div>
                                        </div>
                                    </div>
                                </asp:Panel>
                                <asp:ValidationSummary ID="vsZakaahPaymentMaster" runat="server" DisplayMode="BulletList"
                                    ShowMessageBox="true" ShowSummary="false" ValidationGroup="FilterZakaahPaymentMaster" />
                            </div>
                        </div>
                    </div>

                    <asp:ListView ID="lvZakaahPaymentMaster" runat="server" DataKeyNames="ZakaahPaymentMasterId" OnItemCommand="lvZakaahPaymentMaster_ItemCommand" OnItemDataBound="lvZakaahPaymentMaster_ItemDataBound">
                        <LayoutTemplate>
                            <div class="panel panel-default">
                                <div class="table-responsive">
                                    <table id="itemPlaceholderContainer" runat="server" class="table table-hover">
                                        <tr>
                                            <th>
                                                <%= Resources.Resource.PaymentDate %>
                                            </th>
                                            <th>
                                                <%= Resources.Resource.FromDate %>
                                            </th>
                                            <th>
                                                <%= Resources.Resource.ToDate %>
                                            </th>
                                            <th>
                                                <%= Resources.Resource.PendingAmount %>
                                            </th>
                                            <th>
                                                <%= Resources.Resource.ZakaahAmount %>
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
                                    <asp:Literal ID="ltrlPaymentDate" runat="server"></asp:Literal>
                                </td>
                                <td>
                                    <asp:Literal ID="ltrlFromDate" runat="server"></asp:Literal>
                                </td>
                                <td>
                                    <asp:Literal ID="ltrlToDate" runat="server"></asp:Literal>
                                </td>
                                <td>
                                    <asp:Literal ID="ltrlPendingAmount" runat="server"></asp:Literal>
                                </td>
                                <td>
                                    <asp:Literal ID="ltrlZakaahAmount" runat="server"></asp:Literal>
                                </td>
                                <td>
                                    <asp:LinkButton ID="lbtnEdit" runat="server" Visible="false" CommandName="EditRecord" CssClass="btn btn-primary btn-sm fa fa-edit" title="Edit"></asp:LinkButton>
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
                            <cc1:DataPager ID="pgrZakaahPaymentMaster" runat="server" OnPagerCommand="pgrZakaahPaymentMaster_ItemCommand"
                                Visible="false" CssClass="pagination" CurrentPageCssClass="active"></cc1:DataPager>
                        </div>
                    </div>


                </ContentTemplate>
                <Triggers>

                    <asp:AsyncPostBackTrigger ControlID="btnSaveAndNew" EventName="Click" />
                    <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                </Triggers>
            </asp:UpdatePanel>
        </div>

        <div class="modal fade" id="divzakaahpaymentdetails">
            <div class="modal-dialog wide-modal-dialog">
                <div class="modal-content">
                    <div class="blockui">
                        <asp:UpdatePanel ID="upzakaahpaymentdetails" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Panel ID="pnlzakaahpaymentdetails" runat="server" DefaultButton="btnSaveAndNew">
                                    <asp:HiddenField ID="hdnModelZakaahPayment" runat="server"></asp:HiddenField>
                                    <asp:HiddenField ID="hdnActionZakaahPayment" runat="server"></asp:HiddenField>
                                    <div class="modal-header">
                                        <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                                        <h4 class="modal-title"><%= Resources.Resource.ZakaahPaymentAddEditTitle %></h4>
                                    </div>
                                    <div class="modal-body">
                                        <div class="form-horizontal" role="form">
                                            <div class="row">
                                                <asp:HiddenField ID="hdnZakaahPaymentMasterId" runat="server" Visible="false"></asp:HiddenField>

                                                <div class="col-lg-6">
                                                    <div class="form-group">
                                                        <label class="col-sm-4 control-label"><%= Resources.Resource.PaymentDate %></label>
                                                        <div class="col-sm-7">
                                                            <asp:TextBox ID="txtPaymentDate" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                                            <div class="text-danger">
                                                                <asp:RequiredFieldValidator ID="rfvPaymentDate" runat="server" ControlToValidate="txtPaymentDate" Display="Dynamic"
                                                                    ValidationGroup="zakaahpaymentdetails"><%= Resources.Messages.InputRequired %> <%=Resources.Resource.PaymentDate %></asp:RequiredFieldValidator>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="form-group">
                                                        <label class="col-sm-4 control-label"><%= Resources.Resource.FromDate %></label>
                                                        <div class="col-sm-7">
                                                            <asp:TextBox ID="txtFromDate" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                                            <div class="text-danger">
                                                                <asp:RequiredFieldValidator ID="rfvFromDate" runat="server" ControlToValidate="txtFromDate" Display="Dynamic"
                                                                    ValidationGroup="zakaahpaymentdetails"><%= Resources.Messages.InputRequired %> <%=Resources.Resource.FromDate %></asp:RequiredFieldValidator>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="form-group">
                                                        <label class="col-sm-4 control-label"><%= Resources.Resource.ToDate %></label>
                                                        <div class="col-sm-7">
                                                            <asp:TextBox ID="txtToDate" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                                            <div class="text-danger">
                                                                <asp:RequiredFieldValidator ID="rfvToDate" runat="server" ControlToValidate="txtToDate" Display="Dynamic"
                                                                    ValidationGroup="zakaahpaymentdetails"><%= Resources.Messages.InputRequired %> <%=Resources.Resource.ToDate %></asp:RequiredFieldValidator>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="form-group">
                                                        <label class="col-sm-4 control-label"><%= Resources.Resource.ActiveSinceDate %></label>
                                                        <div class="col-sm-7">
                                                            <asp:TextBox ID="txtActiveDate" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                                            <div class="text-danger">
                                                                <asp:RequiredFieldValidator ID="rfvActiveDate" runat="server" ControlToValidate="txtActiveDate" Display="Dynamic"
                                                                    ValidationGroup="zakaahpaymentdetails"><%= Resources.Messages.InputRequired %> <%=Resources.Resource.ActiveSinceDate %></asp:RequiredFieldValidator>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <%--<div class="form-group">
                                                        <label class="col-sm-4 control-label"><%= Resources.Resource.ActiveSinceMonths %></label>
                                                        <div class="col-sm-7">
                                                            <asp:DropDownList ID="ddlMonths" CssClass="form-control" runat="server">
                                                                <asp:ListItem Text="1" Value="1"></asp:ListItem>
                                                                <asp:ListItem Text="2" Value="2"></asp:ListItem>
                                                                <asp:ListItem Text="3" Value="3"></asp:ListItem>
                                                                <asp:ListItem Text="4" Value="4"></asp:ListItem>
                                                                <asp:ListItem Text="5" Value="5"></asp:ListItem>
                                                                <asp:ListItem Text="6" Value="6"></asp:ListItem>
                                                                <asp:ListItem Text="7" Value="7"></asp:ListItem>
                                                                <asp:ListItem Text="8" Value="8"></asp:ListItem>
                                                                <asp:ListItem Text="9" Value="9"></asp:ListItem>
                                                                <asp:ListItem Text="10" Value="10"></asp:ListItem>
                                                                <asp:ListItem Text="11" Value="11"></asp:ListItem>
                                                                <asp:ListItem Text="12" Value="12"></asp:ListItem>
                                                            </asp:DropDownList>
                                                        </div>
                                                    </div>--%>
                                                </div>
                                                <div class="col-lg-6">
                                                    <div class="form-group">
                                                        <label class="col-sm-4 control-label"><%= Resources.Resource.PendingAmount %></label>
                                                        <div class="col-sm-7">
                                                            <asp:TextBox ID="txtPendingAmount" runat="server" Enabled="false" CssClass="form-control input-sm"></asp:TextBox>
                                                            <div class="text-danger">
                                                                <asp:RequiredFieldValidator ID="rfvPendingAmount" runat="server" ControlToValidate="txtPendingAmount" Display="Dynamic" SetFocusOnError="true"
                                                                    ValidationGroup="zakaahpaymentdetails"><%= Resources.Messages.InputRequired %> <%=Resources.Resource.PendingAmount %></asp:RequiredFieldValidator>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="form-group">
                                                        <label class="col-sm-4 control-label"><%= Resources.Resource.ZakaahAmount %></label>
                                                        <div class="col-sm-7">
                                                            <asp:TextBox ID="txtZakaahAmount" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                                            <div class="text-danger">
                                                                <asp:RequiredFieldValidator ID="rfvZakaahAmount" runat="server" ControlToValidate="txtZakaahAmount" Display="Dynamic" SetFocusOnError="true"
                                                                    ValidationGroup="zakaahpaymentdetails"><%= Resources.Messages.InputRequired %> <%=Resources.Resource.ZakaahAmount %></asp:RequiredFieldValidator>
                                                                <asp:CompareValidator ID="cmvZakaahAmount" runat="server"
                                                                    ControlToValidate="txtZakaahAmount" Display="Dynamic" Operator="DataTypeCheck" SetFocusOnError="true"
                                                                    Type="Double" ValidationGroup="zakaahpaymentdetails"><%= Resources.Messages.InputInvalid %> <%=Resources.Resource.ZakaahAmount %></asp:CompareValidator>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="form-group">
                                                        <label class="col-sm-4 control-label"></label>
                                                        <div class="col-sm-7">
                                                        </div>
                                                    </div>
                                                    <div class="form-group">
                                                        <label class="col-sm-4 control-label"></label>
                                                        <div class="col-sm-7">
                                                            <asp:LinkButton ID="lbtnShowList" OnClick="lbtnShowList_Click" runat="server" CssClass="btn btn-info btn-sm" Text="<% $Resources:Resource, View %>"></asp:LinkButton>
                                                        </div>
                                                    </div>


                                                </div>


                                                <div class="col-sm-12">
                                                    <asp:Button ID="btnClearList" runat="server" Style="display: none;" OnClick="btnClearList_Click" />
                                                    <asp:ListView ID="lvContractMaster" runat="server" DataKeyNames="ContractMasterId" OnItemDataBound="lvContractMaster_ItemDataBound">
                                                        <LayoutTemplate>
                                                            <div class="panel panel-default" id="divContract">
                                                                <div class="table-responsive" style="overflow: scroll; max-height: 800px;">
                                                                    <table id="itemPlaceholderContainer" runat="server" class="table table-hover">
                                                                        <tr>
                                                                            <th>
                                                                                <%= Resources.Resource.CustomerName %>
                                                                            </th>
                                                                            <th>
                                                                                <%= Resources.Resource.IdNo %>
                                                                            </th>
                                                                            <th>
                                                                                <%= Resources.Resource.FileNo %>
                                                                            </th>
                                                                            <th>
                                                                                <%= Resources.Resource.ContractTitle %>
                                                                            </th>
                                                                            <th>
                                                                                <%= Resources.Resource.Bank %>
                                                                            </th>
                                                                            <th>
                                                                                <%= Resources.Resource.ContractDate %>
                                                                            </th>
                                                                            <th>
                                                                                <%= Resources.Resource.ContractStartDate %>
                                                                            </th>
                                                                            <th>
                                                                                <%= Resources.Resource.ContractEndDate %>
                                                                            </th>
                                                                            <th>
                                                                                <%= Resources.Resource.ContractAmount %>
                                                                            </th>
                                                                            <th>
                                                                                <%= Resources.Resource.PaidAmount %>
                                                                            </th>
                                                                            <th>
                                                                                <%= Resources.Resource.PendingAmount %>
                                                                            </th>
                                                                            <th>
                                                                                <%= Resources.Resource.DownPayment %>
                                                                            </th>
                                                                            <th>
                                                                                <%= Resources.Resource.NoOfInstallments %>
                                                                            </th>
                                                                            <th>
                                                                                <%= Resources.Resource.InstallmentAmount %>
                                                                            </th>
                                                                            <th>
                                                                                <%= Resources.Resource.InstallmentDate %>
                                                                            </th>
                                                                            <th>
                                                                                <%= Resources.Resource.DueAmount %>
                                                                            </th>
                                                                            <th>
                                                                                <%= Resources.Resource.SettlementAmount %>
                                                                            </th>
                                                                            <th>
                                                                                <%= Resources.Resource.SettlementReason %>
                                                                            </th>
                                                                            <th>
                                                                                <%= Resources.Resource.Notes %>
                                                                            </th>
                                                                            <th>
                                                                                <%= Resources.Resource.LastPaidDate %>
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
                                                                    <asp:Literal ID="ltrlCustomerName" runat="server"></asp:Literal>
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
                                                                    <asp:Literal ID="ltrlPaidAmount" runat="server"></asp:Literal>
                                                                </td>
                                                                <td>
                                                                    <asp:Literal ID="ltrlPendingAmount" runat="server"></asp:Literal>
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
                                                                    <asp:Literal ID="ltrlNotes" runat="server"></asp:Literal>
                                                                </td>
                                                                <td>
                                                                    <asp:Literal ID="ltrlLastPaidDate" runat="server"></asp:Literal>
                                                                </td>
                                                            </tr>
                                                        </ItemTemplate>
                                                    </asp:ListView>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="modal-footer">
                                            <asp:Button ID="btnSaveAndNew" runat="server" Text="<% $Resources:Resource, btnSaveAndNew %>" CssClass="btn btn-primary" OnClick="btnSave_Click" ValidationGroup="zakaahpaymentdetails" />

                                            <asp:Button ID="btnSave" runat="server" Text="<% $Resources:Resource, btnSave %>" CssClass="btn btn-primary" OnClick="btnSave_Click" ValidationGroup="zakaahpaymentdetails" />

                                            <asp:Button ID="btnClose" runat="server" Text="<% $Resources:Resource, btnClose %>" CssClass="btn btn-default" CausesValidation="false" data-dismiss="modal" />

                                        </div>
                                </asp:Panel>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="lvZakaahPaymentMaster" EventName="ItemCommand" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </div>
        </div>
        <asp:ValidationSummary ID="vszakaahpaymentdetails" runat="server" DisplayMode="BulletList"
            ShowMessageBox="true" ShowSummary="false" ValidationGroup="zakaahpaymentdetails" />

    </div>
    <script type="text/javascript">
        $(document).ready(function () {
            SetControlsPicker();
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(SetControlsPicker);
        });

        function SetControlsPicker() {
            SetDatePicker(Prefix + "txtPaymentDate");
            SetDatePicker(Prefix + "txtFilterFromDate");
            SetDatePicker(Prefix + "txtFilterToDate");
            SetDatePicker(Prefix + "txtFromDate");
            SetDatePicker(Prefix + "txtToDate");
            SetDatePicker(Prefix + "txtActiveDate");
        }
    </script>
    <script type="text/javascript">
        $(document).ready(function () {
            SetFilterControlsPicker();
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(SetFilterControlsPicker);
        });

        function SetFilterControlsPicker() {
            SetRangeDatePicker(Prefix + "txtFromDate", Prefix + "txtToDate");
            SetRangeDatePicker(Prefix + "txtFilterFromDate", Prefix + "txtFilterToDate");
        }
    </script>
    <script type="text/javascript">
        $(document).ready(function () {
            if ($("#" + Prefix + "hdnModelZakaahPayment").val() == "") {
                ClearZakaahPaymentMaster();
            }
            SetDialogShowHideZakaahPaymentMaster();
            ShowHideDialogZakaahPaymentMaster();
        });

        function SetDialogShowHideZakaahPaymentMaster() {
            $("#divzakaahpaymentdetails").on("hidden.bs.modal", function () {
                ClearZakaahPaymentMaster();
                $("#" + Prefix + "btnClearList").click();
            })
            $("#divzakaahpaymentdetails").on("show.bs.modal", function () {
                SetZakaahPaymentMaster();
            })
            $("#divzakaahpaymentdetails").on("shown.bs.modal", function () {
                $("#" + Prefix + "ddlZakaah").focus();
            })
        }
        function ShowHideDialogZakaahPaymentMaster() {
            try {
                SetZakaahPaymentMaster();
                if ($("#" + Prefix + "hdnModelZakaahPayment").val() == "show") {
                    $("#divzakaahpaymentdetails").modal("show");
                }
                else if ($("#" + Prefix + "hdnModelZakaahPayment").val() == "hide") {
                    ClearZakaahPaymentMaster();
                    $("#divzakaahpaymentdetails").modal("hide");
                }
                else if ($("#" + Prefix + "hdnModelZakaahPayment").val() == "clear") {
                    ClearZakaahPaymentMaster();
                    $("#divzakaahpaymentdetails").modal("show");
                    $("#" + Prefix + "ddlZakaah").focus();
                }
                $("#" + Prefix + "hdnModelZakaahPayment").val("");
                return false;
            }
            catch (ex) {
                alert(ex);
                return false;
            }
        }
        function ClearZakaahPaymentMaster() {
            for (i = 0; i < Page_Validators.length; i++) {
                Page_Validators[i].style.display = "none";
            }


        }
        function SetZakaahPaymentMaster() {
            for (i = 0; i < Page_Validators.length; i++) {
                Page_Validators[i].errormessage = Page_Validators[i].innerHTML;
            }
            if ($("#" + Prefix + "hdnActionZakaahPayment").val() == "") {
                $("#" + Prefix + "btnSaveAndNew").css("visibility", "visible");
                $("#" + Prefix + "btnSave").val('<%= GetGlobalResourceObject("Resource", "btnSave") %>');
            }
            else {
                $("#" + Prefix + "btnSaveAndNew").css("visibility", "hidden");
                $("#" + Prefix + "btnSave").val('<%= GetGlobalResourceObject("Resource", "btnUpdate") %>');
            }

        }


    </script>
</asp:Content>
