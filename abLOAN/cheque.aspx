<%@ Page Language="C#" MasterPageFile="Site.Master" AutoEventWireup="true" CodeBehind="cheque.aspx.cs"
    Inherits="abLOAN.cheque" %>

<%@ Register Assembly="CustomDataPager" Namespace="CustomDataPager" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script type="text/javascript">
        $(document).ready(function () {
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequest);
            SetFilter();
        });
        function EndRequest(sender, args) {

            ShowHideDialogChequeMaster();
            SetFilter();
            DisableCheckBoxes();
        }
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphPage" runat="server">
    <div id="divcheque">
        <div class="blockui">
            <asp:UpdatePanel ID="upcheque" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <div class="row">
                        <div style="justify-content: space-between; display: flex;">
                            <div style="padding: 0 15px;">
                                <h4>
                                    <i class="fa fa-th"></i>
                                    <%= Resources.Resource.ChequePageTitle %>
                                    <small>
                                        <asp:Label ID="lblRecords" runat="server" Visible="false"></asp:Label></small>
                                    &nbsp;
                                    <a id="aFilter" class="btn btn-default btn-sm" style="text-decoration: none;" onclick="javascript:return ShowHideFilter('divFilter', false);"><%= Resources.Resource.Filter %>&nbsp;&nbsp;<span class="caret"></span></a>
                                </h4>
                            </div>
                            <div style="padding: 0 15px;">
                                <asp:Button ID="btnUpdate" runat="server" Text="<%$Resources:Resource, btnSave %>" CssClass="btn btn-primary" OnClick="btnUpdate_Click" />
                                &nbsp;

                              <%--  <asp:Button ID="btnNew" runat="server" Text="New" CssClass="btn btn-primary" data-toggle="modal" data-target="#divchequedetails" OnClientClick="javascript:return false;" />
                                &nbsp;--%>
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
                                                    <label class="col-sm-5 control-label"><%= Resources.Resource.ChequeNo %></label>
                                                    <div class="col-sm-7">
                                                        <asp:TextBox ID="txtFilterChequeNo" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-md-4 col-sm-6">
                                                <div class="form-group">
                                                    <label class="col-sm-5 control-label"><%= Resources.Resource.BankName %></label>
                                                    <div class="col-sm-7">
                                                        <asp:DropDownList ID="ddlFilterBank" runat="server" CssClass="form-control input-sm"></asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>
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
                                                    <label class="col-sm-5 control-label"><%= Resources.Resource.ChequeStatus %></label>
                                                    <div class="col-sm-7">
                                                        <asp:DropDownList ID="ddlFilterChequeStatus" runat="server" CssClass="form-control input-sm"></asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="row">
                                            <div class="col-md-12 text-center">
                                                <asp:Button ID="btnSearch" runat="server" Text="<% $Resources:Resource, btnSearch %>" CssClass="btn btn-primary" ValidationGroup="FilterChequeMaster" OnClick="btnSearch_Click" />
                                                &nbsp;
                                                <asp:Button ID="btnReset" runat="server" Text="<% $Resources:Resource, btnReset %>" CssClass="btn btn-default" CausesValidation="false" OnClick="btnReset_Click" />
                                            </div>
                                        </div>
                                    </div>
                                </asp:Panel>
                                <asp:ValidationSummary ID="vsChequeMaster" runat="server" DisplayMode="BulletList"
                                    ShowMessageBox="true" ShowSummary="false" ValidationGroup="FilterChequeMaster" />
                            </div>
                        </div>
                    </div>

                    <asp:ListView ID="lvChequeMaster" runat="server" DataKeyNames="ChequeMasterId" OnItemCommand="lvChequeMaster_ItemCommand" OnItemDataBound="lvChequeMaster_ItemDataBound">
                        <LayoutTemplate>
                            <div class="panel panel-default">
                                <div class="table-responsive">
                                    <table id="itemPlaceholderContainer" runat="server" class="table table-hover">
                                        <tr>
                                            <th><%= Resources.Resource.ChequeName %>
                                            </th>
                                            <th><%= Resources.Resource.ChequeDate %>
                                            </th>
                                            <th><%= Resources.Resource.Bank %>
                                            </th>

                                            <th><%= Resources.Resource.CustomerName %>
                                            </th>
                                           

                                            <th><%= Resources.Resource.GivenTo %>
                                            </th>
                                            <th><%= Resources.Resource.Notes %>
                                            </th>
                                            <th><%= Resources.Resource.EntryDate %>
                                            </th>
                                            <th><%= Resources.Resource.ChequeAmount %>
                                            </th>
                                            <th><%= Resources.Resource.ChequeNo %>
                                            </th>
                                            <th><%= Resources.Resource.ChequeStatus %>
                                            </th>
                                            <th></th>


                                            <th style="width: 25px;"></th>
                                        </tr>

                                        <tr id="itemPlaceholder" runat="server">
                                        </tr>
                                    </table>
                                </div>
                            </div>
                        </LayoutTemplate>
                        <ItemTemplate>
                            <tr valign="top" id='row' runat="server">
                                <td>
                                    <asp:HiddenField ID="hdnChequeMasterId" runat="server" />
                                    <asp:Literal ID="ltrlChequeName" runat="server"></asp:Literal>
                                </td>

                                <td>
                                    <asp:Literal ID="ltrlChequeDate" runat="server"></asp:Literal>
                                </td>
                                <td>
                                    <asp:HiddenField ID="hdnBankMasterId" runat="server" />
                                    <asp:Literal ID="ltrlBank" runat="server"></asp:Literal>
                                </td>

                                <td>
                                    <asp:HiddenField ID="hdnCustomerMasterId" runat="server" />
                                    <asp:Literal ID="ltrlCustomer" runat="server"></asp:Literal>
                                </td>
                              

                                <td>
                                    <asp:Literal ID="ltrlGivenTo" runat="server"></asp:Literal>
                                </td>
                                <td>
                                    <asp:Literal ID="ltrlNotes" runat="server"></asp:Literal>
                                </td>
                                <td>
                                    <asp:Literal ID="ltrlEntryDate" runat="server"></asp:Literal>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtChequeAmount" CssClass="txtChequeAmount" runat="server" Width="50"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtChequeNo" runat="server" Width="50"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlChequeStatus" runat="server"></asp:DropDownList>
                                </td>
                                <td>
                                    <asp:CheckBox ID="chkSelect" runat="server" CssClass="chkSelect" onclick='<%# "javascript:ListEnableDisableRowTextBoxes(this, \"row\");" %>'></asp:CheckBox>
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
                            <cc1:DataPager ID="pgrChequeMaster" runat="server" OnPagerCommand="pgrChequeMaster_ItemCommand"
                                Visible="false" CssClass="pagination" CurrentPageCssClass="active"></cc1:DataPager>
                        </div>
                    </div>


                </ContentTemplate>
                <Triggers>
                </Triggers>
            </asp:UpdatePanel>
        </div>

        <%-- <div class="modal fade" id="divchequedetails">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="blockui">
                        <asp:UpdatePanel ID="upchequedetails" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Panel ID="pnlchequedetails" runat="server" DefaultButton="btnSaveAndNew">
                                    <asp:HiddenField ID="hdnModelCheque" runat="server"></asp:HiddenField>
                                    <asp:HiddenField ID="hdnActionCheque" runat="server"></asp:HiddenField>
                                    <div class="modal-header">
                                        <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                                        <h4 class="modal-title">Cheque Details</h4>
                                    </div>
                                    <div class="modal-body">
                                        <div class="form-horizontal" role="form">
                                            <asp:HiddenField ID="hdnChequeMasterId" runat="server" Visible="false"></asp:HiddenField>
                                            <asp:HiddenField ID="hdnCustomerMasterId" runat="server" Visible="false"></asp:HiddenField>
                                            <div class="form-group">
                                                <label class="col-sm-4 control-label">Cheque Name</label>
                                                <div class="col-sm-7">
                                                    <asp:TextBox ID="txtChequeName" runat="server" CssClass="form-control input-sm" MaxLength="100"></asp:TextBox>
                                                    <div class="text-danger">
                                                        <asp:RequiredFieldValidator ID="rfvChequeName" runat="server" ErrorMessage="Please enter Cheque Name"
                                                            ControlToValidate="txtChequeName" Display="Dynamic" SetFocusOnError="true" Text="Please enter Cheque Name" ValidationGroup="chequedetails"></asp:RequiredFieldValidator>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <label class="col-sm-4 control-label">Cheque No</label>
                                                <div class="col-sm-7">
                                                    <asp:TextBox ID="txtChequeNo" runat="server" CssClass="form-control input-sm" MaxLength="50"></asp:TextBox>
                                                    <div class="text-danger">
                                                        <asp:RequiredFieldValidator ID="rfvChequeNo" runat="server" ErrorMessage="Please enter Cheque No"
                                                            ControlToValidate="txtChequeNo" Display="Dynamic" SetFocusOnError="true" Text="Please enter Cheque No" ValidationGroup="chequedetails"></asp:RequiredFieldValidator>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <label class="col-sm-4 control-label">Cheque Date</label>
                                                <div class="col-sm-7">
                                                    <asp:TextBox ID="txtChequeDate" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                                    <div class="text-danger">
                                                        <asp:RequiredFieldValidator ID="rfvChequeDate" runat="server" ErrorMessage="Please enter Cheque Date"
                                                            ControlToValidate="txtChequeDate" Display="Dynamic" Text="Cheque Date" ValidationGroup="chequedetails"></asp:RequiredFieldValidator>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <label class="col-sm-4 control-label">Bank</label>
                                                <div class="col-sm-7">
                                                    <asp:DropDownList ID="ddlBank" runat="server" CssClass="form-control input-sm"></asp:DropDownList>
                                                    <div class="text-danger">
                                                        <asp:RequiredFieldValidator ID="rfvBank" runat="server" ErrorMessage="Please select Bank"
                                                            ControlToValidate="ddlBank" Display="Dynamic" SetFocusOnError="true" Text="Please select Bank" InitialValue="" ValidationGroup="chequedetails"></asp:RequiredFieldValidator>
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="form-group">
                                                <label class="col-sm-4 control-label">Customer</label>
                                                <div class="col-sm-7">
                                                    <asp:DropDownList ID="ddlCustomer" runat="server" CssClass="form-control input-sm"></asp:DropDownList>
                                                    <div class="text-danger">
                                                        <asp:RequiredFieldValidator ID="rfvCustomer" runat="server" ErrorMessage="Please select Customer"
                                                            ControlToValidate="ddlCustomer" Display="Dynamic" SetFocusOnError="true" Text="Please select Customer" InitialValue="" ValidationGroup="chequedetails"></asp:RequiredFieldValidator>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <label class="col-sm-4 control-label">Installment Amount</label>
                                                <div class="col-sm-7">
                                                    <asp:TextBox ID="txtInstallmentAmount" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                                    <div class="text-danger">
                                                        <asp:RequiredFieldValidator ID="rfvInstallmentAmount" runat="server" ErrorMessage="Please enter Installment Amount"
                                                            ControlToValidate="txtInstallmentAmount" Display="Dynamic" SetFocusOnError="true" Text="Please enter Installment Amount" ValidationGroup="chequedetails"></asp:RequiredFieldValidator>
                                                        <asp:CompareValidator ID="cmvInstallmentAmount" runat="server" ErrorMessage="Please enter valid Installment Amount"
                                                            ControlToValidate="txtInstallmentAmount" Display="Dynamic" Operator="DataTypeCheck" SetFocusOnError="true"
                                                            Text="Please enter valid Installment Amount" Type="Double" ValidationGroup="chequedetails"></asp:CompareValidator>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <label class="col-sm-4 control-label">Cheque Amount</label>
                                                <div class="col-sm-7">
                                                    <asp:TextBox ID="txtChequeAmount" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                                    <div class="text-danger">
                                                        <asp:RequiredFieldValidator ID="rfvChequeAmount" runat="server" ErrorMessage="Please enter Cheque Amount"
                                                            ControlToValidate="txtChequeAmount" Display="Dynamic" SetFocusOnError="true" Text="Please enter Cheque Amount" ValidationGroup="chequedetails"></asp:RequiredFieldValidator>
                                                        <asp:CompareValidator ID="cmvChequeAmount" runat="server" ErrorMessage="Please enter valid Cheque Amount"
                                                            ControlToValidate="txtChequeAmount" Display="Dynamic" Operator="DataTypeCheck" SetFocusOnError="true"
                                                            Text="Please enter valid Cheque Amount" Type="Double" ValidationGroup="chequedetails"></asp:CompareValidator>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <label class="col-sm-4 control-label">Given To</label>
                                                <div class="col-sm-7">
                                                    <asp:TextBox ID="txtGivenTo" runat="server" CssClass="form-control input-sm" MaxLength="100"></asp:TextBox>
                                                    <div class="text-danger">
                                                        <asp:RequiredFieldValidator ID="rfvGivenTo" runat="server" ErrorMessage="Please enter Given To"
                                                            ControlToValidate="txtGivenTo" Display="Dynamic" SetFocusOnError="true" Text="Please enter Given To" ValidationGroup="chequedetails"></asp:RequiredFieldValidator>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <label class="col-sm-4 control-label">Notes</label>
                                                <div class="col-sm-7">
                                                    <asp:TextBox ID="txtNotes" runat="server" Rows="3" CssClass="form-control input-sm" onKeyDown="javascript:CheckTextAreaMaxLength(this, event, 500);" TextMode="MultiLine"></asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <label class="col-sm-4 control-label">Entry Date</label>
                                                <div class="col-sm-7">
                                                    <asp:TextBox ID="txtEntryDate" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                                    <div class="text-danger">
                                                        <asp:RequiredFieldValidator ID="rfvEntryDate" runat="server" ErrorMessage="Please enter Entry Date"
                                                            ControlToValidate="txtEntryDate" Display="Dynamic" Text="Entry Date" ValidationGroup="chequedetails"></asp:RequiredFieldValidator>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <label class="col-sm-4 control-label">Cheque Status</label>
                                                <div class="col-sm-7">
                                                    <asp:DropDownList ID="ddlChequeStatus" runat="server" CssClass="form-control input-sm"></asp:DropDownList>
                                                    <div class="text-danger">
                                                        <asp:RequiredFieldValidator ID="rfvChequeStatus" runat="server" ErrorMessage="Please select Cheque Status"
                                                            ControlToValidate="ddlChequeStatus" Display="Dynamic" SetFocusOnError="true" Text="Please select Cheque Status" InitialValue="" ValidationGroup="chequedetails"></asp:RequiredFieldValidator>
                                                    </div>
                                                </div>
                                            </div>


                                        </div>
                                    </div>
                                    <div class="modal-footer">
                                        <asp:Button ID="btnSaveAndNew" runat="server" Text="Save & New" CssClass="btn btn-primary" OnClick="btnSave_Click" ValidationGroup="chequedetails" />

                                        <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="btn btn-primary" OnClick="btnSave_Click" ValidationGroup="chequedetails" />

                                        <asp:Button ID="btnClose" runat="server" Text="Close" CssClass="btn btn-default" CausesValidation="false" data-dismiss="modal" />

                                    </div>
                                </asp:Panel>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="lvChequeMaster" EventName="ItemCommand" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </div>
        </div>--%>

        <asp:ValidationSummary ID="vschequedetails" runat="server" DisplayMode="BulletList"
            ShowMessageBox="true" ShowSummary="false" ValidationGroup="chequedetails" />

    </div>
    <script type="text/javascript">
        $(document).ready(function () {
            SetControlsPicker();
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(SetControlsPicker);
            SetFilterControlsPicker();
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(SetFilterControlsPicker);
            
        });

        function SetControlsPicker() {
            //SetDatePicker(Prefix + "txtChequeDate");
            //SetDatePicker(Prefix + "txtEntryDate");
        }
        function SetFilterControlsPicker() {
            //    SetDatePicker(Prefix + "txtFilterContractDate");
            //    SetDatePicker(Prefix + "txtFilterContractDateTo");
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
            if ($("#" + Prefix + "hdnModelCheque").val() == "") {
                ClearChequeMaster();
            }
            SetDialogShowHideChequeMaster();
            ShowHideDialogChequeMaster();
            DisableCheckBoxes();
        });

        function SetDialogShowHideChequeMaster() {
            $("#divchequedetails").on("hidden.bs.modal", function () {
                ClearChequeMaster();
            })
            $("#divchequedetails").on("show.bs.modal", function () {
               // SetChequeMaster();
            })
            $("#divchequedetails").on("shown.bs.modal", function () {
                $("#" + Prefix + "txtChequeName").focus();
            })
        }
        function ShowHideDialogChequeMaster() {
            try {
              //  SetChequeMaster();
                if ($("#" + Prefix + "hdnModelCheque").val() == "show") {
                    $("#divchequedetails").modal("show");
                }
                else if ($("#" + Prefix + "hdnModelCheque").val() == "hide") {
                    ClearChequeMaster();
                    $("#divchequedetails").modal("hide");
                }
                else if ($("#" + Prefix + "hdnModelCheque").val() == "clear") {
                    ClearChequeMaster();
                    $("#divchequedetails").modal("show");
                    $("#" + Prefix + "txtChequeName").focus();
                }
                $("#" + Prefix + "hdnModelCheque").val("");
                return false;
            }
            catch (ex) {
                alert(ex);
                return false;
            }
        }
        function ClearChequeMaster() {
            for (i = 0; i < Page_Validators.length; i++) {
                Page_Validators[i].style.display = "none";
            }
            $("#" + Prefix + "hdnActionCheque").val("");
            $("#" + Prefix + "txtChequeName").val("");
            $("#" + Prefix + "txtChequeNo").val("");
            $("#" + Prefix + "txtChequeDate").val(new Date().format("MM/dd/yyyy"));
            $("#" + Prefix + "ddlBank").prop("selectedIndex", 0);

            $("#" + Prefix + "ddlCustomer").prop("selectedIndex", 0);
           
            $("#" + Prefix + "txtChequeAmount").val("");
            $("#" + Prefix + "txtGivenTo").val("");
            $("#" + Prefix + "txtNotes").val("");
            $("#" + Prefix + "txtEntryDate").val(new Date().format("MM/dd/yyyy"));
            $("#" + Prefix + "ddlChequeStatus").prop("selectedIndex", 0);


        }
        function DisableCheckBoxes() {
            var txtList = $('.txtChequeAmount');

            for (var i = 0; i < txtList.length; i++) {
                
                var chk = txtList[i].id.replace("txtChequeAmount", "chkSelect");
                if ($("#" + chk).prop("checked") == false) {
                    var txtChequeNo = txtList[i].id.replace("txtChequeAmount", "txtChequeNo");
                    $("#" + txtChequeNo).attr("disabled", "disabled");
                    $("#" + txtList[i].id).attr("disabled", "disabled");
                    $("#" + txtList[i].id).val = "";

                   
                    //var ddlChequeStatus = txtList[i].id.replace("txtChequeAmount", "ddlChequeStatus");
                    //$("#" + ddlChequeStatus).attr("disabled", "true");

                }
            }
        }
        
    </script>

</asp:Content>
