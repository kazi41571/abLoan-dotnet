<%@ Page Language="C#" MasterPageFile="Site.Master" AutoEventWireup="true" CodeBehind="chequemultiple.aspx.cs"
    Inherits="abLOAN.chequemultiple" %>

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
            SetDates();
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
                                    <i class="fa fa-th"></i><%= Resources.Resource.ChequeMultiplePageTitle %>
                                    <small>
                                        <asp:Label ID="lblRecords" runat="server" Visible="false"></asp:Label></small>
                                    &nbsp;
                                    <a id="aFilter" class="btn btn-default btn-sm" style="text-decoration: none;" onclick="javascript:return ShowHideFilter('divFilter', false);"><%= Resources.Resource.Filter %>&nbsp;&nbsp;<span class="caret"></span></a>
                                </h4>
                            </div>
                            <div style="padding: 0 15px;">


                                <asp:Button ID="btnInsert" OnClick="btnInsert_Click" runat="server" Text="<%$Resources:Resource, btnSave %>" CssClass="btn btn-primary" />



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
                                                    <label class="col-sm-3 control-label"><%= Resources.Resource.ChequeNo %></label>
                                                    <div class="col-sm-7">
                                                        <asp:TextBox ID="txtFilterChequeNo" runat="server" CssClass="form-control input-sm"></asp:TextBox>
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
                                                    <label class="col-sm-4 control-label"><%= Resources.Resource.ChequeStatus %></label>
                                                    <div class="col-sm-7">
                                                        <asp:DropDownList ID="ddlFilterChequeStatus" runat="server" CssClass="form-control input-sm"></asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="col-md-12 col-sm-12">
                                                <div class="form-group">
                                                    <label class="col-sm-1 control-label"><%= Resources.Resource.BankName %></label>
                                                    <div class="col-sm-10">
                                                        <%--<asp:DropDownList ID="ddlFilterBank" runat="server" CssClass="form-control input-sm"></asp:DropDownList>--%>
                                                        <asp:CheckBoxList AutoPostBack="true" ID="chkFilterBank" CssClass="checkboxlist" RepeatColumns="7" RepeatLayout="Table" RepeatDirection="Horizontal" runat="server"></asp:CheckBoxList>
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
                                            <th><%= Resources.Resource.Bank %>
                                            </th>
                                            <th><%= Resources.Resource.CustomerName %>
                                            </th>
                                            <th><%= Resources.Resource.NoOfContracts %>
                                            </th>
                                            <th><%= Resources.Resource.TotalInstallmentAmount %>
                                            </th>


                                            <th><%= Resources.Resource.ChequeNo %>
                                            </th>
                                            <th><%= Resources.Resource.ChequeAmount %>
                                            </th>
                                            <th><%= Resources.Resource.ChequeName %>
                                            </th>
                                            <th><%= Resources.Resource.ChequeDate %>
                                                <br />
                                                <asp:CheckBox ID="chkSelectDate" runat="server" onclick="SetDatesValue();" />
                                                <asp:TextBox ID="txtChequeDateMain" runat="server" CssClass="txtDate" Width="80"></asp:TextBox>
                                            </th>

                                            <th style="display: none;"><%= Resources.Resource.GivenTo %>
                                            </th>
                                            <th><%= Resources.Resource.Notes %>
                                            </th>
                                            <th style="display: none;"><%= Resources.Resource.Others %>
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
                                    <asp:HiddenField ID="hdnBankMasterId" runat="server" />
                                    <asp:Literal ID="ltrlBank" runat="server"></asp:Literal>
                                </td>

                                <td>
                                    <asp:HiddenField ID="hdnCustomerMasterId" runat="server" />
                                    <asp:Literal ID="ltrlCustomer" runat="server"></asp:Literal>
                                    <asp:HyperLink ID="hlnkCustomer" NavigateUrl="#" Target="_blank" runat="server" Visible="false">
                                    </asp:HyperLink>
                                </td>
                                <td>
                                    <asp:Literal ID="ltrlNoOfContracts" runat="server"></asp:Literal>
                                </td>
                                <td>
                                    <asp:Literal ID="ltrlTotalInstallmentAmount" runat="server"></asp:Literal>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtChequeNo" runat="server" Width="50"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtChequeAmount" runat="server" Width="50"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtChequeName" runat="server" Width="100" TextMode="MultiLine" Rows="2"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtChequeDate" runat="server" CssClass="txtDate" Width="100"></asp:TextBox>
                                </td>
                                <td style="display: none;">
                                    <asp:TextBox ID="txtGivenTo" runat="server" Width="100" TextMode="MultiLine" Rows="2"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtNotes" runat="server" Width="100" TextMode="MultiLine" Rows="2"></asp:TextBox>
                                </td>
                                <td style="display: none;">
                                    <asp:TextBox ID="txtOthers" runat="server" Width="100" TextMode="MultiLine" Rows="2"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlChequeStatus" runat="server"></asp:DropDownList>
                                </td>
                                <td>
                                    <asp:CheckBox ID="chkSelect" runat="server" onclick='<%# "javascript:ListEnableDisableRowTextBoxes(this, \"row\");" %>'></asp:CheckBox>
                                </td>

                                <td>
                                    <%--<asp:LinkButton ID="lbtnEdit" runat="server" CommandName="EditRecord" CssClass="btn btn-primary btn-sm fa fa-edit" title="Edit"></asp:LinkButton>--%>
                                    <asp:LinkButton ID="btnAdd" runat="server" CommandName="AddRecord" CssClass="btn btn-primary btn-sm fa fa-plus" title="Add"></asp:LinkButton>
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
            SetDatePicker(Prefix + "txtChequeDate");

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
            SetDates();
        });

        function SetDialogShowHideChequeMaster() {
            $("#divchequedetails").on("hidden.bs.modal", function () {
                ClearChequeMaster();
            })
            $("#divchequedetails").on("show.bs.modal", function () {
                SetChequeMaster();
            })
            $("#divchequedetails").on("shown.bs.modal", function () {
                $("#" + Prefix + "txtChequeName").focus();
            })
        }
        function ShowHideDialogChequeMaster() {
            try {
                SetChequeMaster();
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
            //for (i = 0; i < Page_Validators.length; i++) {
            //    Page_Validators[i].style.display = "none";
            //}
            //$("#" + Prefix + "hdnActionCheque").val("");
            //$("#" + Prefix + "txtChequeName").val("");
            //$("#" + Prefix + "txtChequeNo").val("");
            //$("#" + Prefix + "txtChequeDate").val(new Date().format("MM/dd/yyyy"));
            //$("#" + Prefix + "ddlBank").prop("selectedIndex", 0);

            //$("#" + Prefix + "ddlCustomer").prop("selectedIndex", 0);

            //$("#" + Prefix + "txtChequeAmount").val("");
            //$("#" + Prefix + "txtGivenTo").val("");
            //$("#" + Prefix + "txtNotes").val("");

            //$("#" + Prefix + "ddlChequeStatus").prop("selectedIndex", 0);


        }
        function SetChequeMaster() {
            if ($("#" + Prefix + "hdnActionCheque").val() == "") {
                $("#" + Prefix + "btnSaveAndNew").css("visibility", "visible");
                $("#" + Prefix + "btnSave").val('<%= GetGlobalResourceObject("Resource", "btnSave") %>');
            }
            else {
                $("#" + Prefix + "btnSaveAndNew").css("visibility", "hidden");
                $("#" + Prefix + "btnSave").val('<%= GetGlobalResourceObject("Resource", "btnUpdate") %>');
            }
        }

        function SetDates() {
            var txtList = $('.txtDate');

            for (var i = 0; i < txtList.length; i++) {
                SetDatePicker(txtList[i].id);
                var chk = txtList[i].id.replace("txtChequeDate", "chkSelect");
                if ($("#" + chk).prop("checked") == false) {
                    var txtChequeNo = txtList[i].id.replace("txtChequeDate", "txtChequeNo");
                    $("#" + txtChequeNo).attr("disabled", "disabled");
                    var txtChequeAmount = txtList[i].id.replace("txtChequeDate", "txtChequeAmount");
                    $("#" + txtChequeAmount).attr("disabled", "disabled");
                    var txtChequeName = txtList[i].id.replace("txtChequeDate", "txtChequeName");
                    $("#" + txtChequeName).attr("disabled", "disabled");

                    $("#" + txtList[i].id).attr("disabled", "disabled");
                    $("#" + txtList[i].id).val = "";

                    var txtGivenTo = txtList[i].id.replace("txtChequeDate", "txtGivenTo");
                    $("#" + txtGivenTo).attr("disabled", "disabled");
                    var txtNotes = txtList[i].id.replace("txtChequeDate", "txtNotes");
                    $("#" + txtNotes).attr("disabled", "disabled");
                    var txtOthers = txtList[i].id.replace("txtChequeDate", "txtOthers");
                    $("#" + txtOthers).attr("disabled", "disabled");
                    var ddlChequeStatus = txtList[i].id.replace("txtChequeDate", "ddlChequeStatus");
                    //$("#" + ddlChequeStatus).attr("disabled", "true");

                }



            }
        }
        function SetDatesValue() {
            var txtList = $('.txtDate');

            if ($("#" + Prefix + "lvChequeMaster_chkSelectDate").prop("checked") == true) {
                for (var i = 0; i < txtList.length; i++) {
                    var chk = txtList[i].id.replace("txtChequeDate", "chkSelect");
                    if ($("#" + chk).prop("checked") == true) {
                        //  alert($("#" + Prefix + "lvChequeMaster_txtChequeDateMain").val());                       
                        $("#" + txtList[i].id).val($("#" + Prefix + "lvChequeMaster_txtChequeDateMain").val());
                    }
                }
            }
        }

    </script>

</asp:Content>
