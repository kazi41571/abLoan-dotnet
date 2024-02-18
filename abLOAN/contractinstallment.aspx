<%@ Page Language="C#" MasterPageFile="Site.Master" AutoEventWireup="true" CodeBehind="contractinstallment.aspx.cs"
    Inherits="abLOAN.contractinstallment" %>

<%@ Register Assembly="CustomDataPager" Namespace="CustomDataPager" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script type="text/javascript">
        $(document).ready(function () {
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequest);
            SetFilter();
        });
        function EndRequest(sender, args) {

            ShowHideDialogContractInstallmentTran();
            SetFilter();
        }
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphPage" runat="server">
    <div id="divcontractinstallment">
        <div class="blockui">
            <asp:UpdatePanel ID="upcontractinstallment" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <div class="row">
                        <div style="justify-content: space-between; display: flex;">
                            <div style="padding: 0 15px;">
                                <h4>
                                    <i class="fa fa-th"></i>&nbsp;<%= Resources.Resource.ContractInstallmentPageTitle %>&nbsp;
                                    <small>
                                        <asp:Label ID="lblRecords" runat="server" Visible="false"></asp:Label></small>
                                    &nbsp;
                                    <a id="aFilter" class="btn btn-default btn-sm" href="" style="text-decoration: none;" onclick="javascript:return ShowHideFilter('divFilter', false);"><%= Resources.Resource.Filter %>&nbsp;&nbsp;<span class="caret"></span></a>
                                </h4>
                            </div>
                            <div style="padding: 0 15px;">
                                <asp:Button ID="btnNew" runat="server" Visible="false" Text="<% $Resources:Resource, btnNew %>" CssClass="btn btn-primary" data-toggle="modal" data-target="#divcontractinstallmentdetails" OnClientClick="javascript:return false;" />
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
                                                    <label class="col-sm-5 control-label"><%= Resources.Resource.PaymentDate %></label>
                                                    <div class="col-sm-7">
                                                        <asp:TextBox ID="txtFilterInstallmentDateFrom" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                                        <div class="text-danger">
                                                            <asp:RequiredFieldValidator ID="rfvFilterInstallmentDate" runat="server"
                                                                ControlToValidate="txtFilterInstallmentDateFrom" Display="Dynamic"
                                                                ValidationGroup="FilterContractInstallmentTran"><%= Resources.Messages.InputRequired %><%=Resources.Resource.InstallmentDate %></asp:RequiredFieldValidator>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-md-4 col-sm-6">
                                                <div class="form-group">
                                                    <div class="col-sm-7">
                                                        <asp:TextBox ID="txtFilterInstallmentDateTo" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                                        <div class="text-danger">
                                                            <asp:RequiredFieldValidator ID="rfvFilterInstallmentDateTo" runat="server"
                                                                ControlToValidate="txtFilterInstallmentDateTo" Display="Dynamic"
                                                                ValidationGroup="FilterContractInstallmentTran"><%= Resources.Messages.InputRequired %><%=Resources.Resource.InstallmentDate %></asp:RequiredFieldValidator>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>

                                        </div>

                                        <div class="row">
                                            <div class="col-md-12 text-center">
                                                <asp:Button ID="btnSearch" runat="server" Text="<% $Resources:Resource, btnSearch %>" CssClass="btn btn-primary" ValidationGroup="FilterContractInstallmentTran" OnClick="btnSearch_Click" />
                                                &nbsp;
                                                <asp:Button ID="btnReset" runat="server" Text="<% $Resources:Resource, btnReset %>" CssClass="btn btn-default" CausesValidation="false" OnClick="btnReset_Click" />
                                            </div>
                                        </div>
                                    </div>
                                </asp:Panel>
                                <asp:ValidationSummary ID="vsContractInstallmentTran" runat="server" DisplayMode="BulletList"
                                    ShowMessageBox="true" ShowSummary="false" ValidationGroup="FilterContractInstallmentTran" />
                            </div>
                        </div>
                    </div>

                    <asp:ListView ID="lvContractInstallmentTran" runat="server" DataKeyNames="ContractInstallmentTranId"
                        OnItemDataBound="lvContractInstallmentTran_ItemDataBound" OnDataBound="lvContractInstallmentTran_DataBound">
                        <LayoutTemplate>
                            <div class="panel panel-default">
                                <div class="table-responsive">
                                    <table id="itemPlaceholderContainer" runat="server" class="table table-hover">
                                        <tr>
                                            <th><%= Resources.Resource.Customer %>
                                            </th>
                                            <th>
                                                <%= Resources.Resource.Contract %>
                                            </th>
                                            <th>
                                                <%= Resources.Resource.Bank %>
                                            </th>
                                            <th>
                                                <%= Resources.Resource.PaymentType %>
                                            </th>
                                            <th>
                                                <%= Resources.Resource.ChequeNo %>
                                            </th>
                                            <th>
                                                <%= Resources.Resource.ChequeDate %>
                                            </th>
                                            <th>
                                                <%= Resources.Resource.PaymentDate %>
                                            </th>
                                            <th>
                                                <%= Resources.Resource.InstallmentAmount %>
                                            </th>
                                            <th>
                                                <%= Resources.Resource.Notes %>
                                            </th>

                                        </tr>

                                        <tr id="itemPlaceholder" runat="server">
                                        </tr>


                                        <tr class="tablefooter" style='font-family: Calibri, monospace; font-size: 20px; display: none;'>


                                            <th>
                                                <%= Resources.Resource.Total %>
                                            </th>

                                            <th></th>
                                            <th></th>
                                            <th></th>
                                            <th></th>
                                            <th></th>
                                            <th></th>
                                            <th>
                                                <asp:Literal ID="ltrlTotalAmount" runat="server"></asp:Literal>
                                            </th>
                                            <th></th>

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
                                </td>
                                <td>
                                    <asp:Literal ID="ltrlContract" runat="server"></asp:Literal>
                                    <asp:HyperLink ID="hlnkContract" NavigateUrl="#" Target="_blank" runat="server" Visible="false">
                                    </asp:HyperLink>
                                </td>
                                <td>
                                    <asp:Literal ID="ltrlBank" runat="server"></asp:Literal>
                                </td>
                                <td>
                                    <asp:Literal ID="ltrlPaymentType" runat="server"></asp:Literal>
                                </td>
                                <td>
                                    <asp:Literal ID="ltrlChequeNo" runat="server"></asp:Literal>
                                </td>
                                <td>
                                    <asp:Literal ID="ltrlChequeDate" runat="server"></asp:Literal>
                                </td>
                                <td>
                                    <asp:Literal ID="ltrlInstallmentDate" runat="server"></asp:Literal>
                                </td>
                                <td>
                                    <asp:Literal ID="ltrlInstallmentAmount" runat="server"></asp:Literal>
                                </td>
                                <td>
                                    <asp:Literal ID="ltrlNotes" runat="server"></asp:Literal>
                                </td>
                                <%--<td>
                                    <asp:LinkButton ID="lbtnEdit" runat="server" CommandName="EditRecord" CssClass="btn btn-primary btn-sm fa fa-edit" title="Edit"></asp:LinkButton>
                                </td>
                                <td>
                                    <asp:LinkButton ID="lbtnDelete" runat="server" CommandName="DeleteRecord" OnClientClick="javascript:return ConfirmDelete(this);" CssClass="btn btn-danger btn-sm fa fa-trash" title="Delete"></asp:LinkButton>
                                </td>--%>
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
                            <cc1:DataPager ID="pgrContractInstallmentTran" runat="server" PageSize="50" OnPagerCommand="pgrContractInstallmentTran_ItemCommand"
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

        <div class="modal fade" id="divcontractinstallmentdetails">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="blockui">
                        <asp:UpdatePanel ID="upcontractinstallmentdetails" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Panel ID="pnlcontractinstallmentdetails" runat="server" DefaultButton="btnSaveAndNew">
                                    <asp:HiddenField ID="hdnModelContractInstallment" runat="server"></asp:HiddenField>
                                    <asp:HiddenField ID="hdnActionContractInstallment" runat="server"></asp:HiddenField>
                                    <div class="modal-header">
                                        <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                                        <h4 class="modal-title"><%= Resources.Resource.ContractInstallmentAddEditTitle %></h4>
                                    </div>
                                    <div class="modal-body">
                                        <div class="form-horizontal" role="form">
                                            <asp:HiddenField ID="hdnContractInstallmentTranId" runat="server" Visible="false"></asp:HiddenField>
                                            <div class="form-group">
                                                <label class="col-sm-4 control-label"><%= Resources.Resource.Contract %></label>
                                                <div class="col-sm-7">
                                                    <asp:DropDownList ID="ddlContract" runat="server" CssClass="form-control input-sm"></asp:DropDownList>
                                                    <div class="text-danger">
                                                        <asp:RequiredFieldValidator ID="rfvContract" runat="server" ControlToValidate="ddlContract" Display="Dynamic"
                                                            SetFocusOnError="true" InitialValue="" ValidationGroup="contractinstallmentdetails"><%= Resources.Messages.InputRequired %><%=Resources.Resource.Contract %></asp:RequiredFieldValidator>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <label class="col-sm-4 control-label"><%= Resources.Resource.InstallmentDate %></label>
                                                <div class="col-sm-7">
                                                    <asp:TextBox ID="txtInstallmentDate" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                                    <div class="text-danger">
                                                        <asp:RequiredFieldValidator ID="rfvInstallmentDate" runat="server" ControlToValidate="txtInstallmentDate" Display="Dynamic"
                                                            ValidationGroup="contractinstallmentdetails"><%= Resources.Messages.InputRequired %><%=Resources.Resource.InstallmentDate %></asp:RequiredFieldValidator>
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
                                    <div class="modal-footer">
                                        <asp:Button ID="btnSaveAndNew" runat="server" Text="<% $Resources:Resource, btnSaveAndNew %>" CssClass="btn btn-primary" OnClick="btnSave_Click" ValidationGroup="contractinstallmentdetails" />

                                        <asp:Button ID="btnSave" runat="server" Text="<% $Resources:Resource, btnSave %>" CssClass="btn btn-primary" OnClick="btnSave_Click" ValidationGroup="contractinstallmentdetails" />

                                        <asp:Button ID="btnClose" runat="server" Text="<% $Resources:Resource, btnClose %>" CssClass="btn btn-default" CausesValidation="false" data-dismiss="modal" />

                                    </div>
                                </asp:Panel>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="lvContractInstallmentTran" EventName="ItemCommand" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </div>
        </div>
        <asp:ValidationSummary ID="vscontractinstallmentdetails" runat="server" DisplayMode="BulletList"
            ShowMessageBox="true" ShowSummary="false" ValidationGroup="contractinstallmentdetails" />

    </div>
    <script type="text/javascript">
        $(document).ready(function () {
            SetControlsPicker();
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(SetControlsPicker);
        });

        function SetControlsPicker() {
            SetDatePicker(Prefix + "txtInstallmentDate");
        }
    </script>
    <script type="text/javascript">
        $(document).ready(function () {
            SetFilterControlsPicker();
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(SetFilterControlsPicker);
        });

        function SetFilterControlsPicker() {
            SetRangeDatePicker(Prefix + "txtFilterInstallmentDateFrom", Prefix + "txtFilterInstallmentDateTo");
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
    <script type="text/javascript">
        $(document).ready(function () {
            if ($("#" + Prefix + "hdnModelContractInstallment").val() == "") {
                ClearContractInstallmentTran();
            }
            SetDialogShowHideContractInstallmentTran();
            ShowHideDialogContractInstallmentTran();
        });

        function SetDialogShowHideContractInstallmentTran() {
            $("#divcontractinstallmentdetails").on("hidden.bs.modal", function () {
                ClearContractInstallmentTran();
            })
            $("#divcontractinstallmentdetails").on("show.bs.modal", function () {
                SetContractInstallmentTran();
            })
            $("#divcontractinstallmentdetails").on("shown.bs.modal", function () {
                $("#" + Prefix + "ddlContract").focus();
            })
        }
        function ShowHideDialogContractInstallmentTran() {
            try {
                SetContractInstallmentTran();
                if ($("#" + Prefix + "hdnModelContractInstallment").val() == "show") {
                    $("#divcontractinstallmentdetails").modal("show");
                }
                else if ($("#" + Prefix + "hdnModelContractInstallment").val() == "hide") {
                    ClearContractInstallmentTran();
                    $("#divcontractinstallmentdetails").modal("hide");
                }
                else if ($("#" + Prefix + "hdnModelContractInstallment").val() == "clear") {
                    ClearContractInstallmentTran();
                    $("#divcontractinstallmentdetails").modal("show");
                    $("#" + Prefix + "ddlContract").focus();
                }
                $("#" + Prefix + "hdnModelContractInstallment").val("");
                return false;
            }
            catch (ex) {
                alert(ex);
                return false;
            }
        }
        function ClearContractInstallmentTran() {
            for (i = 0; i < Page_Validators.length; i++) {
                Page_Validators[i].style.display = "none";
            }
            $("#" + Prefix + "hdnActionContractInstallment").val("");
            $("#" + Prefix + "ddlContract").prop("selectedIndex", 0);
            $("#" + Prefix + "txtInstallmentDate").val(GetDateToString(new Date()));
            $("#" + Prefix + "txtNotes").val("");

        }
        function SetContractInstallmentTran() {
            for (i = 0; i < Page_Validators.length; i++) {
                Page_Validators[i].errormessage = Page_Validators[i].innerHTML;
            }
            if ($("#" + Prefix + "hdnActionContractInstallment").val() == "") {
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
