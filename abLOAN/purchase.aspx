<%@ Page Language="C#" MasterPageFile="Site.Master" AutoEventWireup="true" CodeBehind="purchase.aspx.cs"
    Inherits="abLOAN.purchase" %>

<%@ Register Assembly="CustomDataPager" Namespace="CustomDataPager" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script type="text/javascript">
        $(document).ready(function () {
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequest);
            SetFilter();
        });
        function EndRequest(sender, args) {

            ShowHideDialogPurchaseMaster();
            SetFilter();
        }
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphPage" runat="server">
    <div id="divpurchase">
        <div class="blockui">
            <asp:UpdatePanel ID="uppurchase" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <div class="row">
                        <div style="justify-content: space-between; display: flex;">
                            <div style="padding: 0 15px;">
                                <h4>
                                    <i class="fa fa-th"></i>&nbsp;<%= Resources.Resource.PurchasePageTitle %>&nbsp;
                                    <small>
                                        <asp:Label ID="lblRecords" runat="server" Visible="false"></asp:Label></small>
                                    &nbsp;
                                    <a id="aFilter" class="btn btn-default btn-sm" href="" style="text-decoration: none;" onclick="javascript:return ShowHideFilter('divFilter', false);"><%= Resources.Resource.Filter %>&nbsp;&nbsp;<span class="caret"></span></a>
                                </h4>
                            </div>
                            <div style="padding: 0 15px;">
                                <asp:Button ID="btnDownload" runat="server" Text="<% $Resources:Resource, Download %>" CssClass="btn btn-info" OnClick="btnDownload_Click" />
                                &nbsp;
                                <asp:Button ID="btnNew" runat="server" Text="<% $Resources:Resource, btnNew %>" CssClass="btn btn-primary" data-toggle="modal" data-target="#divpurchasedetails" OnClientClick="javascript:return false;" />
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
                                                    <label class="col-sm-4 control-label"><%= Resources.Resource.PurchaseDate %></label>
                                                    <div class="col-sm-4">
                                                        <asp:TextBox ID="txtFromDate" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                                        <div class="text-danger">
                                                            <asp:RequiredFieldValidator ID="rfvFilterPurchaseDateFrom" runat="server"
                                                                ControlToValidate="txtFromDate" Display="Dynamic"
                                                                ValidationGroup="FilterPurchaseMaster"><%= Resources.Messages.InputRequired %> <%=Resources.Resource.PurchaseDate %></asp:RequiredFieldValidator>
                                                        </div>
                                                    </div>
                                                    <div class="col-sm-4">
                                                        <asp:TextBox ID="txtToDate" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                                        <div class="text-danger">
                                                            <asp:RequiredFieldValidator ID="rfvFilterPurchaseDateTo" runat="server"
                                                                ControlToValidate="txtToDate" Display="Dynamic"
                                                                ValidationGroup="FilterPurchaseMaster"><%= Resources.Messages.InputRequired %> <%=Resources.Resource.PurchaseDate %></asp:RequiredFieldValidator>
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
                                        </div>

                                        <div class="row">
                                            <div class="col-md-12 text-center">
                                                <asp:Button ID="btnSearch" runat="server" Text="<% $Resources:Resource, btnSearch %>" CssClass="btn btn-primary" ValidationGroup="FilterPurchaseMaster" OnClick="btnSearch_Click" />
                                                &nbsp;
                                                <asp:Button ID="btnReset" runat="server" Text="<% $Resources:Resource, btnReset %>" CssClass="btn btn-default" CausesValidation="false" OnClick="btnReset_Click" />
                                            </div>
                                        </div>
                                    </div>
                                </asp:Panel>
                                <asp:ValidationSummary ID="vsPurchaseMaster" runat="server" DisplayMode="BulletList"
                                    ShowMessageBox="true" ShowSummary="false" ValidationGroup="FilterPurchaseMaster" />
                            </div>
                        </div>
                    </div>

                    <asp:ListView ID="lvPurchaseMaster" runat="server" DataKeyNames="PurchaseMasterId" OnDataBound="lvPurchaseMaster_DataBound" OnItemCommand="lvPurchaseMaster_ItemCommand" OnItemDataBound="lvPurchaseMaster_ItemDataBound">
                        <LayoutTemplate>
                            <div class="panel panel-default">
                                <div class="table-responsive">
                                    <table id="itemPlaceholderContainer" runat="server" class="table table-hover">
                                        <tr>
                                            <th>
                                                <%= Resources.Resource.PurchaseDate %>
                                            </th>
                                            <th>
                                                <%= Resources.Resource.Item %>
                                            </th>
                                            <th>
                                                <%= Resources.Resource.Quantity %>
                                            </th>
                                            <th>
                                                <%= Resources.Resource.PurchaseRate %>
                                            </th>
                                            <th style="display: none;">
                                                <%= Resources.Resource.Vat %>
                                            </th>
                                            <th>
                                                <%= Resources.Resource.NetAmount %>
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
                                        <tr class="tablefooter" style='font-family: Calibri, monospace; font-size: 20px;'>

                                            <th></th>
                                            <th></th>
                                            <th></th>
                                            <th>Total</th>
                                            <th>
                                                <asp:Literal ID="ltrlTotalNetAmount" runat="server"></asp:Literal>
                                            </th>
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
                                    <asp:Literal ID="ltrlPurchaseDate" runat="server"></asp:Literal>
                                </td>
                                <td>
                                    <asp:Literal ID="ltrlItem" runat="server"></asp:Literal>
                                </td>
                                <td>
                                    <asp:Literal ID="ltrlQuantity" runat="server"></asp:Literal>
                                </td>
                                <td>
                                    <asp:Literal ID="ltrlPurchaseRate" runat="server"></asp:Literal>
                                </td>
                                <td style="display: none;">
                                    <asp:Literal ID="ltrlVat" runat="server"></asp:Literal>
                                </td>

                                <td>
                                    <asp:Literal ID="ltrlNetAmount" runat="server"></asp:Literal>
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
                            <cc1:DataPager ID="pgrPurchaseMaster" runat="server" OnPagerCommand="pgrPurchaseMaster_ItemCommand"
                                Visible="false" CssClass="pagination" CurrentPageCssClass="active"></cc1:DataPager>
                        </div>
                    </div>


                </ContentTemplate>
                <Triggers>
                    <asp:PostBackTrigger ControlID="btnDownload" />
                    <asp:PostBackTrigger ControlID="btnSaveAndNew" />
                    <asp:PostBackTrigger ControlID="btnSave" />
                </Triggers>
            </asp:UpdatePanel>
        </div>

        <div class="modal fade" id="divpurchasedetails">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="blockui">
                        <asp:UpdatePanel ID="uppurchasedetails" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Panel ID="pnlpurchasedetails" runat="server" DefaultButton="btnSaveAndNew">
                                    <asp:HiddenField ID="hdnModelPurchase" runat="server"></asp:HiddenField>
                                    <asp:HiddenField ID="hdnActionPurchase" runat="server"></asp:HiddenField>
                                    <div class="modal-header">
                                        <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                                        <h4 class="modal-title"><%= Resources.Resource.PurchaseFormTitle %></h4>
                                    </div>
                                    <div class="modal-body">
                                        <div class="form-horizontal" role="form">
                                            <asp:HiddenField ID="hdnPurchaseMasterId" runat="server" Visible="false"></asp:HiddenField>
                                            <div class="form-group">
                                                <label class="col-sm-4 control-label"><%= Resources.Resource.PurchaseDate %></label>
                                                <div class="col-sm-7">
                                                    <asp:TextBox ID="txtPurchaseDate" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                                    <div class="text-danger">
                                                        <asp:RequiredFieldValidator ID="rfvPurchaseDate" runat="server" ControlToValidate="txtPurchaseDate" Display="Dynamic"
                                                            ValidationGroup="purchasedetails"><%= Resources.Messages.InputRequired %>  <%=Resources.Resource.PurchaseDate %></asp:RequiredFieldValidator>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <label class="col-sm-4 control-label"><%= Resources.Resource.CategoryName %></label>
                                                <div class="col-sm-7">
                                                    <asp:DropDownList ID="ddlCategory" AutoPostBack="true" OnSelectedIndexChanged="ddlCategory_SelectedIndexChanged" runat="server" CssClass="form-control input-sm"></asp:DropDownList>
                                                    <div class="text-danger">
                                                        <asp:RequiredFieldValidator ID="rfvCategory" runat="server"
                                                            ControlToValidate="ddlCategory" Display="Dynamic" SetFocusOnError="true" InitialValue="" ValidationGroup="purchasedetails"><%= Resources.Messages.InputRequired %>      <%=Resources.Resource.CategoryName %></asp:RequiredFieldValidator>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <label class="col-sm-4 control-label"><%= Resources.Resource.Item %></label>
                                                <div class="col-sm-7">
                                                    <asp:DropDownList ID="ddlItem" AutoPostBack="true" OnSelectedIndexChanged="ddlItem_SelectedIndexChanged" runat="server" CssClass="form-control input-sm"></asp:DropDownList>
                                                    <div class="text-danger">
                                                        <asp:RequiredFieldValidator ID="rfvItem" runat="server"
                                                            ControlToValidate="ddlItem" Display="Dynamic" SetFocusOnError="true" InitialValue="" ValidationGroup="purchasedetails"><%= Resources.Messages.InputRequired %>      <%=Resources.Resource.linktoItemMasterId %></asp:RequiredFieldValidator>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <label class="col-sm-4 control-label"><%= Resources.Resource.Quantity %></label>
                                                <div class="col-sm-7">
                                                    <asp:TextBox ID="txtQuantity" onchange="javascript:Calculate();" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                                    <div class="text-danger">
                                                        <asp:RequiredFieldValidator ID="rfvQuantity" runat="server" SetFocusOnError="true" ControlToValidate="txtQuantity"
                                                            ValidationGroup="purchasedetails"><%= Resources.Messages.InputRequired %> <%=Resources.Resource.Quantity %></asp:RequiredFieldValidator>
                                                        <asp:CompareValidator ID="cmvQuantity" runat="server"
                                                            ControlToValidate="txtQuantity" Display="Dynamic" Operator="DataTypeCheck" SetFocusOnError="true"
                                                            Type="Integer" ValidationGroup="purchasedetails"><%= Resources.Messages.InputInvalid %>  <%=Resources.Resource.Quantity %></asp:CompareValidator>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <label class="col-sm-4 control-label"><%= Resources.Resource.PurchaseRate %></label>
                                                <div class="col-sm-7">
                                                    <asp:TextBox ID="txtPurchaseRate" onchange="javascript:Calculate();" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                                    <div class="text-danger">
                                                        <asp:RequiredFieldValidator ID="rfvPurchaseRate" runat="server" SetFocusOnError="true" ControlToValidate="txtPurchaseRate"
                                                            ValidationGroup="purchasedetails"><%= Resources.Messages.InputRequired %>  <%=Resources.Resource.PurchaseRate %></asp:RequiredFieldValidator>
                                                        <asp:CompareValidator ID="cmvPurchaseRate" runat="server"
                                                            ControlToValidate="txtPurchaseRate" Display="Dynamic" Operator="DataTypeCheck" SetFocusOnError="true"
                                                            Type="Double" ValidationGroup="purchasedetails"><%= Resources.Messages.InputInvalid %> <%=Resources.Resource.PurchaseRate %></asp:CompareValidator>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="form-group" style="display: none;">
                                                <label class="col-sm-4 control-label"><%= Resources.Resource.Vat %></label>
                                                <div class="col-sm-7">
                                                    <asp:TextBox ID="txtVat" onchange="javascript:Calculate();" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                                    <div class="text-danger">
                                                        <asp:RequiredFieldValidator ID="rfvVat" runat="server" SetFocusOnError="true" ControlToValidate="txtVat"
                                                            ValidationGroup="purchasedetails"><%= Resources.Messages.InputRequired %>  <%=Resources.Resource.Vat %></asp:RequiredFieldValidator>
                                                        <asp:CompareValidator ID="cvVat" runat="server"
                                                            ControlToValidate="txtVat" Display="Dynamic" Operator="DataTypeCheck" SetFocusOnError="true"
                                                            Type="Double" ValidationGroup="purchasedetails"><%= Resources.Messages.InputInvalid %> <%=Resources.Resource.Vat %></asp:CompareValidator>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <label class="col-sm-4 control-label"><%= Resources.Resource.NetAmount %></label>
                                                <div class="col-sm-7">
                                                    <asp:TextBox ID="txtNetAmount" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                                    <div class="text-danger">
                                                        <asp:RequiredFieldValidator ID="rfvNetAmount" runat="server" SetFocusOnError="true" ControlToValidate="txtNetAmount"
                                                            ValidationGroup="purchasedetails"><%= Resources.Messages.InputRequired %>  <%=Resources.Resource.NetAmount %></asp:RequiredFieldValidator>
                                                        <asp:CompareValidator ID="cvNetAmount" runat="server"
                                                            ControlToValidate="txtNetAmount" Display="Dynamic" Operator="DataTypeCheck" SetFocusOnError="true"
                                                            Type="Double" ValidationGroup="purchasedetails"><%= Resources.Messages.InputInvalid %> <%=Resources.Resource.NetAmount %></asp:CompareValidator>
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
                                        <asp:Button ID="btnSaveAndNew" runat="server" Text="<% $Resources:Resource, btnSaveAndNew %>" CssClass="btn btn-primary" OnClick="btnSave_Click" ValidationGroup="purchasedetails" />

                                        <asp:Button ID="btnSave" runat="server" Text="<% $Resources:Resource, btnSave %>" CssClass="btn btn-primary" OnClick="btnSave_Click" ValidationGroup="purchasedetails" />

                                        <asp:Button ID="btnClose" runat="server" Text="<% $Resources:Resource, btnClose %>" CssClass="btn btn-default" CausesValidation="false" data-dismiss="modal" />

                                    </div>
                                </asp:Panel>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="lvPurchaseMaster" EventName="ItemCommand" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </div>
        </div>
        <asp:ValidationSummary ID="vspurchasedetails" runat="server" DisplayMode="BulletList"
            ShowMessageBox="true" ShowSummary="false" ValidationGroup="purchasedetails" />

    </div>
    <script type="text/javascript">
        $(document).ready(function () {
            SetControlsPicker();
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(SetControlsPicker);
        });

        function SetControlsPicker() {
            SetDatePicker(Prefix + "txtPurchaseDate");
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
            if ($("#" + Prefix + "hdnModelPurchase").val() == "") {
                ClearPurchaseMaster();
            }
            SetDialogShowHidePurchaseMaster();
            ShowHideDialogPurchaseMaster();
        });
        function Calculate() {
            var purchaseRate = parseFloat($("#" + Prefix + "txtPurchaseRate").val());
            var quantity = parseFloat($("#" + Prefix + "txtQuantity").val());
            var vat = parseFloat($("#" + Prefix + "txtVat").val());
            //var netAmount = purchaseRate * quantity;
            //netAmount = netAmount * vat;
            //netAmount = netAmount / 100.0;
            var netAmount = purchaseRate * quantity;


            $("#" + Prefix + "txtNetAmount").val(netAmount);
        }
        function SetDialogShowHidePurchaseMaster() {
            $("#divpurchasedetails").on("hidden.bs.modal", function () {
                ClearPurchaseMaster();
            })
            $("#divpurchasedetails").on("show.bs.modal", function () {
                SetPurchaseMaster();
            })
            $("#divpurchasedetails").on("shown.bs.modal", function () {
                $("#" + Prefix + "txtPurchaseDate").focus();
            })
        }
        function ShowHideDialogPurchaseMaster() {
            try {
                SetPurchaseMaster();
                if ($("#" + Prefix + "hdnModelPurchase").val() == "show") {
                    $("#divpurchasedetails").modal("show");
                }
                else if ($("#" + Prefix + "hdnModelPurchase").val() == "hide") {
                    ClearPurchaseMaster();
                    $("#divpurchasedetails").modal("hide");
                }
                else if ($("#" + Prefix + "hdnModelPurchase").val() == "clear") {
                    ClearPurchaseMaster();
                    $("#divpurchasedetails").modal("show");
                    $("#" + Prefix + "txtPurchaseDate").focus();
                }
                $("#" + Prefix + "hdnModelPurchase").val("");
                return false;
            }
            catch (ex) {
                alert(ex);
                return false;
            }
        }
        function ClearPurchaseMaster() {
            for (i = 0; i < Page_Validators.length; i++) {
                Page_Validators[i].style.display = "none";
            }
            $("#" + Prefix + "hdnActionPurchase").val("");
            $("#" + Prefix + "txtPurchaseDate").val(GetDateToString(new Date()));
            $("#" + Prefix + "ddlItem").prop("selectedIndex", 0);
            $("#" + Prefix + "ddlCategory").prop("selectedIndex", 0);
            $("#" + Prefix + "txtQuantity").val("1");
            $("#" + Prefix + "txtPurchaseRate").val("");
            $("#" + Prefix + "txtVat").val("0.00");
            $("#" + Prefix + "txtNetAmount").val("");
            $("#" + Prefix + "txtNotes").val("");

        }
        function SetPurchaseMaster() {
            for (i = 0; i < Page_Validators.length; i++) {
                Page_Validators[i].errormessage = Page_Validators[i].innerHTML;
            }
            if ($("#" + Prefix + "hdnActionPurchase").val() == "") {
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
