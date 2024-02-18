<%@ Page Language="C#" MasterPageFile="Site.Master" AutoEventWireup="true" CodeBehind="calling.aspx.cs"
    Inherits="abLOAN.calling" %>

<%@ Register Assembly="CustomDataPager" Namespace="CustomDataPager" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script type="text/javascript">
        $(document).ready(function () {
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequest);
            SetFilter();
        });
        function EndRequest(sender, args) {

            ShowHideDialogCallingMaster();
            SetFilter();
        }
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphPage" runat="server">
    <div id="divcalling">
        <div class="blockui">
            <asp:UpdatePanel ID="upcalling" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <div class="row">
                        <div style="justify-content: space-between; display: flex;">
                            <div style="padding: 0 15px;">
                                <h4>
                                    <i class="fa fa-th"></i>&nbsp;<%= Resources.Resource.CallingPageTitle %>&nbsp;
                                    <small>
                                        <asp:Label ID="lblRecords" runat="server" Visible="false"></asp:Label></small>
                                    &nbsp;
                                    <a id="aFilter" class="btn btn-default btn-sm" href="" style="text-decoration: none;" onclick="javascript:return ShowHideFilter('divFilter', false);"><%= Resources.Resource.Filter %>&nbsp;&nbsp;<span class="caret"></span></a>
                                </h4>
                            </div>
                            <div style="padding: 0 15px;">
                                <asp:Button ID="btnNew" runat="server" Text="<% $Resources:Resource, btnNew %>" CssClass="btn btn-primary" data-toggle="modal" data-target="#divcallingdetails" OnClientClick="javascript:return false;" />
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
                                                    <label class="col-sm-5 control-label"><%= Resources.Resource.CallingDate %></label>
                                                    <div class="col-sm-7">
                                                        <asp:TextBox ID="txtFilterCallingDate" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                                        <div class="text-danger">
                                                            <asp:RequiredFieldValidator ID="rfvFilterCallingDate" runat="server"
                                                                ControlToValidate="txtFilterCallingDate" Display="Dynamic"
                                                                ValidationGroup="FilterCallingMaster"><%= Resources.Messages.InputRequired %> <%=Resources.Resource.CallingDate %></asp:RequiredFieldValidator>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-md-4 col-sm-6">
                                                <div class="form-group">
                                                    <label class="col-sm-5 control-label"><%= Resources.Resource.CallingName %></label>
                                                    <div class="col-sm-7">
                                                        <asp:TextBox ID="txtFilterCallingName" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-md-4 col-sm-6">
                                                <div class="form-group">
                                                    <label class="col-sm-5 control-label"><%= Resources.Resource.Customer %></label>
                                                    <div class="col-sm-7">
                                                        <asp:DropDownList ID="ddlFilterCustomer" runat="server" CssClass="form-control input-sm"></asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="row">
                                            <div class="col-md-12 text-center">
                                                <asp:Button ID="btnSearch" runat="server" Text="<% $Resources:Resource, btnSearch %>" CssClass="btn btn-primary" ValidationGroup="FilterCallingMaster" OnClick="btnSearch_Click" />
                                                &nbsp;
                                                <asp:Button ID="btnReset" runat="server" Text="<% $Resources:Resource, btnReset %>" CssClass="btn btn-default" CausesValidation="false" OnClick="btnReset_Click" />
                                            </div>
                                        </div>
                                    </div>
                                </asp:Panel>
                                <asp:ValidationSummary ID="vsCallingMaster" runat="server" DisplayMode="BulletList"
                                    ShowMessageBox="true" ShowSummary="false" ValidationGroup="FilterCallingMaster" />
                            </div>
                        </div>
                    </div>

                    <asp:ListView ID="lvCallingMaster" runat="server" DataKeyNames="CallingMasterId" OnItemCommand="lvCallingMaster_ItemCommand" OnItemDataBound="lvCallingMaster_ItemDataBound">
                        <LayoutTemplate>
                            <div class="panel panel-default">
                                <div class="table-responsive">
                                    <table id="itemPlaceholderContainer" runat="server" class="table table-hover">
                                        <tr>
                                            <th>
                                                <%= Resources.Resource.CallingDate %>
                                            </th>
                                            <th>
                                                <%= Resources.Resource.CallingName %>
                                            </th>
                                            <th>
                                                <%= Resources.Resource.Customer %>
                                            </th>
                                            <th>
                                                <%= Resources.Resource.Bank %>
                                            </th>
                                            <th>
                                                <%= Resources.Resource.Amount %>
                                            </th>
                                            <th>
                                                <%= Resources.Resource.Notes %>
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
                                    <asp:Literal ID="ltrlCallingDate" runat="server"></asp:Literal>
                                </td>
                                <td>
                                    <asp:Literal ID="ltrlCallingName" runat="server"></asp:Literal>
                                </td>
                                <td>
                                    <asp:Literal ID="ltrlCustomer" runat="server"></asp:Literal>
                                </td>
                                <td>
                                    <asp:Literal ID="ltrlBank" runat="server"></asp:Literal>
                                </td>
                                <td>
                                    <asp:Literal ID="ltrlAmount" runat="server"></asp:Literal>
                                </td>
                                <td>
                                    <asp:Literal ID="ltrlNotes" runat="server"></asp:Literal>
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
                            <cc1:DataPager ID="pgrCallingMaster" runat="server" OnPagerCommand="pgrCallingMaster_ItemCommand"
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

        <div class="modal fade" id="divcallingdetails">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="blockui">
                        <asp:UpdatePanel ID="upcallingdetails" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Panel ID="pnlcallingdetails" runat="server" DefaultButton="btnSaveAndNew">
                                    <asp:HiddenField ID="hdnModelCalling" runat="server"></asp:HiddenField>
                                    <asp:HiddenField ID="hdnActionCalling" runat="server"></asp:HiddenField>
                                    <div class="modal-header">
                                        <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                                        <h4 class="modal-title"><%= Resources.Resource.CallingFormTitle %></h4>
                                    </div>
                                    <div class="modal-body">
                                        <div class="form-horizontal" role="form">
                                            <asp:HiddenField ID="hdnCallingMasterId" runat="server" Visible="false"></asp:HiddenField>
                                            <div class="form-group">
                                                <label class="col-sm-4 control-label"><%= Resources.Resource.CallingDate %></label>
                                                <div class="col-sm-7">
                                                    <asp:TextBox ID="txtCallingDate" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                                    <div class="text-danger">
                                                        <asp:RequiredFieldValidator ID="rfvCallingDate" runat="server" ControlToValidate="txtCallingDate" Display="Dynamic"
                                                            ValidationGroup="callingdetails"><%= Resources.Messages.InputRequired %> <%=Resources.Resource.CallingDate %></asp:RequiredFieldValidator>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <label class="col-sm-4 control-label"><%= Resources.Resource.CallingName %></label>
                                                <div class="col-sm-7">
                                                    <asp:TextBox ID="txtCallingName" runat="server" CssClass="form-control input-sm" ></asp:TextBox>
                                                    <div class="text-danger">
                                                        <asp:RequiredFieldValidator ID="rfvCallingName" runat="server" SetFocusOnError="true" ControlToValidate="txtCallingName"
                                                            ValidationGroup="callingdetails"><%= Resources.Messages.InputRequired %> <%=Resources.Resource.CallingName %></asp:RequiredFieldValidator>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <label class="col-sm-4 control-label"><%= Resources.Resource.Customer %></label>
                                                <div class="col-sm-7">
                                                    <asp:DropDownList ID="ddlCustomer" runat="server" CssClass="form-control input-sm"></asp:DropDownList>
                                                    <div class="text-danger">
                                                        <asp:RequiredFieldValidator ID="rfvCustomer" runat="server" Display="Dynamic"
                                                            ControlToValidate="ddlCustomer" SetFocusOnError="true" InitialValue="" ValidationGroup="callingdetails">
                                                            <%= Resources.Messages.InputRequired %> <%=Resources.Resource.linktoCustomerMasterId %>
                                                        </asp:RequiredFieldValidator>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <label class="col-sm-4 control-label"><%= Resources.Resource.Bank %></label>
                                                <div class="col-sm-7">
                                                    <asp:DropDownList ID="ddlBank" runat="server" CssClass="form-control input-sm"></asp:DropDownList>
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <label class="col-sm-4 control-label"><%= Resources.Resource.Amount %></label>
                                                <div class="col-sm-7">
                                                    <asp:TextBox ID="txtAmount" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                                    <div class="text-danger">
                                                        <asp:CompareValidator ID="cmvAmount" runat="server"
                                                            ControlToValidate="txtAmount" Display="Dynamic" Operator="DataTypeCheck" SetFocusOnError="true"
                                                            Type="Double" ValidationGroup="callingdetails"><%= Resources.Messages.InputInvalid %> <%=Resources.Resource.Amount %></asp:CompareValidator>
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
                                        <asp:Button ID="btnSaveAndNew" runat="server" Text="<% $Resources:Resource, btnSaveAndNew %>" CssClass="btn btn-primary" OnClick="btnSave_Click" ValidationGroup="callingdetails" />

                                        <asp:Button ID="btnSave" runat="server" Text="<% $Resources:Resource, btnSave %>" CssClass="btn btn-primary" OnClick="btnSave_Click" ValidationGroup="callingdetails" />

                                        <asp:Button ID="btnClose" runat="server" Text="<% $Resources:Resource, btnClose %>" CssClass="btn btn-default" CausesValidation="false" data-dismiss="modal" />

                                    </div>
                                </asp:Panel>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="lvCallingMaster" EventName="ItemCommand" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </div>
        </div>
        <asp:ValidationSummary ID="vscallingdetails" runat="server" DisplayMode="BulletList"
            ShowMessageBox="true" ShowSummary="false" ValidationGroup="callingdetails" />

    </div>
    <script type="text/javascript">
        $(document).ready(function () {
            SetControlsPicker();
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(SetControlsPicker);
        });

        function SetControlsPicker() {
            SetDatePicker(Prefix + "txtCallingDate");
        }
    </script>
    <script type="text/javascript">
        $(document).ready(function () {
            SetFilterControlsPicker();
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(SetFilterControlsPicker);
        });

        function SetFilterControlsPicker() {
            SetDatePicker(Prefix + "txtFilterCallingDate");
        }
    </script>
    <script type="text/javascript">
        $(document).ready(function () {
            if ($("#" + Prefix + "hdnModelCalling").val() == "") {
                ClearCallingMaster();
            }
            SetDialogShowHideCallingMaster();
            ShowHideDialogCallingMaster();
        });

        function SetDialogShowHideCallingMaster() {
            $("#divcallingdetails").on("hidden.bs.modal", function () {
                ClearCallingMaster();
            })
            $("#divcallingdetails").on("show.bs.modal", function () {
                SetCallingMaster();
            })
            $("#divcallingdetails").on("shown.bs.modal", function () {
                $("#" + Prefix + "txtCallingDate").focus();
            })
        }
        function ShowHideDialogCallingMaster() {
            try {
                SetCallingMaster();
                if ($("#" + Prefix + "hdnModelCalling").val() == "show") {
                    $("#divcallingdetails").modal("show");
                }
                else if ($("#" + Prefix + "hdnModelCalling").val() == "hide") {
                    ClearCallingMaster();
                    $("#divcallingdetails").modal("hide");
                }
                else if ($("#" + Prefix + "hdnModelCalling").val() == "clear") {
                    ClearCallingMaster();
                    $("#divcallingdetails").modal("show");
                    $("#" + Prefix + "txtCallingDate").focus();
                }
                $("#" + Prefix + "hdnModelCalling").val("");
                return false;
            }
            catch (ex) {
                alert(ex);
                return false;
            }
        }
        function ClearCallingMaster() {
            for (i = 0; i < Page_Validators.length; i++) {
                Page_Validators[i].style.display = "none";
            }
            $("#" + Prefix + "hdnActionCalling").val("");
            $("#" + Prefix + "txtCallingDate").val(GetDateToString(new Date()));
            $("#" + Prefix + "txtCallingName").val("");
            $("#" + Prefix + "ddlCustomer").prop("selectedIndex", 0);
            $("#" + Prefix + "ddlBank").prop("selectedIndex", 0);
            $("#" + Prefix + "txtAmount").val("");
            $("#" + Prefix + "txtNotes").val("");

        }
        function SetCallingMaster() {
            for (i = 0; i < Page_Validators.length; i++) {
                Page_Validators[i].errormessage = Page_Validators[i].innerHTML;
            }
            if ($("#" + Prefix + "hdnActionCalling").val() == "") {
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
