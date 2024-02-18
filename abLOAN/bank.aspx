<%@ Page Language="C#" MasterPageFile="Site.Master" AutoEventWireup="true" CodeBehind="bank.aspx.cs"
    Inherits="abLOAN.bank" %>

<%@ Register Assembly="CustomDataPager" Namespace="CustomDataPager" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script type="text/javascript">
        $(document).ready(function () {
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequest);
            SetFilter();
        });

        function EndRequest(sender, args) {

            ShowHideDialogBankMaster();
            SetFilter();
        }
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphPage" runat="server">
    <div id="divbank">
        <div class="blockui">
            <asp:UpdatePanel ID="upbank" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <div class="row">
                        <div style="justify-content: space-between; display: flex;">
                            <div style="padding: 0 15px;">
                                <h4>
                                    <i class="fa fa-th"></i>&nbsp;
                                     <%= Resources.Resource.BankPageTitle %>
                                    &nbsp; <small>
                                        <asp:Label ID="lblRecords" runat="server" Visible="False"></asp:Label></small>
                                    &nbsp; <a id="aFilter" class="btn btn-default btn-sm" href="" style="text-decoration: none;" onclick="javascript:return ShowHideFilter('divFilter', false);"><%= Resources.Resource.Filter %>&nbsp;&nbsp;<span class="caret"></span></a>
                                </h4>
                            </div>
                            <div style="padding: 0 15px;">

                                <asp:Button ID="btnNew" runat="server" Text="<%$Resources:Resource, btnNew %>" CssClass="btn btn-primary" data-toggle="modal"
                                    data-target="#divbankdetails" OnClientClick="javascript:return false;" />

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
                                                    <label class="col-sm-5 control-label"><%= Resources.Resource.BankName %></label>
                                                    <div class="col-sm-7">
                                                        <asp:TextBox ID="txtFilterBankName" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-md-4 col-sm-6">
                                                <div class="form-group">
                                                    <label class="col-sm-5 control-label"><%= Resources.Resource.IsEnabled %></label>
                                                    <div class="col-sm-7">
                                                        <asp:DropDownList ID="ddlFilterIsEnabled" runat="server" CssClass="form-control input-sm">
                                                            <asp:ListItem Text="<% $Resources:Resource, Yes %>" Value="Yes"></asp:ListItem>
                                                            <asp:ListItem Text="<% $Resources:Resource, No %>" Value="No"></asp:ListItem>
                                                        </asp:DropDownList>
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
                                                <asp:Button ID="btnSearch" runat="server" Text="<% $Resources:Resource, btnSearch %>" CssClass="btn btn-primary" ValidationGroup="FilterBankMaster" OnClick="btnSearch_Click" />
                                                &nbsp;
                                                <asp:Button ID="btnReset" runat="server" Text="<% $Resources:Resource, btnReset %>" CssClass="btn btn-default" CausesValidation="false" OnClick="btnReset_Click" />
                                            </div>
                                        </div>
                                    </div>
                                </asp:Panel>
                                <asp:ValidationSummary ID="vsBankMaster" runat="server" DisplayMode="BulletList"
                                    ShowMessageBox="true" ShowSummary="false" ValidationGroup="FilterBankMaster" />
                            </div>
                        </div>
                    </div>

                    <asp:ListView ID="lvBankMaster" runat="server" DataKeyNames="BankMasterId" OnItemCommand="lvBankMaster_ItemCommand" OnItemDataBound="lvBankMaster_ItemDataBound">
                        <LayoutTemplate>
                            <div class="panel panel-default">
                                <div class="table-responsive">
                                    <table id="itemPlaceholderContainer" runat="server" class="table table-hover">
                                        <tr>
                                            <th>
                                                <%= Resources.Resource.BankName %>
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
                                    </table>
                                </div>
                            </div>
                        </LayoutTemplate>
                        <ItemTemplate>
                            <tr valign="top">
                                <td>
                                    <asp:Literal ID="ltrlBankName" runat="server"></asp:Literal>
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
                            <cc1:DataPager ID="pgrBankMaster" runat="server" OnPagerCommand="pgrBankMaster_ItemCommand"
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

        <div class="modal fade" id="divbankdetails">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="blockui">
                        <asp:UpdatePanel ID="upbankdetails" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Panel ID="pnlbankdetails" runat="server" DefaultButton="btnSaveAndNew">
                                    <asp:HiddenField ID="hdnModelBank" runat="server"></asp:HiddenField>
                                    <asp:HiddenField ID="hdnActionBank" runat="server"></asp:HiddenField>
                                    <div class="modal-header">
                                        <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                                        <h4 class="modal-title"><%= Resources.Resource.BankFormTitle %></h4>
                                    </div>
                                    <div class="modal-body">
                                        <div class="form-horizontal" role="form">
                                            <asp:HiddenField ID="hdnBankMasterId" runat="server" Visible="false"></asp:HiddenField>
                                            <div class="form-group">
                                                <label class="col-sm-4 control-label"><%= Resources.Resource.BankName %></label>
                                                <div class="col-sm-7">
                                                    <asp:TextBox ID="txtBankName" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                                    <div class="text-danger">
                                                        <asp:RequiredFieldValidator ID="rfvBankName" runat="server"
                                                            ControlToValidate="txtBankName" Display="Dynamic" SetFocusOnError="true" ValidationGroup="bankdetails">
                                                            <%= Resources.Messages.InputRequired %> <%=Resources.Resource.BankName %>
                                                        </asp:RequiredFieldValidator>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <div class="col-sm-offset-4 col-sm-7">
                                                    <div class="checkbox">
                                                        <label>
                                                            <asp:CheckBox ID="chkIsEnabled" runat="server" Checked="true" Text="<% $Resources:Resource, IsEnabled %>"></asp:CheckBox>
                                                        </label>
                                                    </div>
                                                </div>
                                            </div>

                                        </div>
                                    </div>
                                    <div class="modal-footer">
                                        <asp:Button ID="btnSaveAndNew" runat="server" Text="<% $Resources:Resource, btnSaveAndNew %>" CssClass="btn btn-primary" OnClick="btnSave_Click" ValidationGroup="bankdetails" />

                                        <asp:Button ID="btnSave" runat="server" Text="<% $Resources:Resource, btnSave %>" CssClass="btn btn-primary" OnClick="btnSave_Click" ValidationGroup="bankdetails" />

                                        <asp:Button ID="btnClose" runat="server" Text="<% $Resources:Resource, btnClose %>" CssClass="btn btn-default" CausesValidation="false" data-dismiss="modal" />

                                    </div>
                                </asp:Panel>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="lvBankMaster" EventName="ItemCommand" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </div>
        </div>
        <asp:ValidationSummary ID="vsbankdetails" runat="server" DisplayMode="BulletList"
            ShowMessageBox="true" ShowSummary="false" ValidationGroup="bankdetails" />

    </div>
    <script type="text/javascript">
        $(document).ready(function () {
            if ($("#" + Prefix + "hdnModelBank").val() == "") {
                ClearBankMaster();
            }
            SetDialogShowHideBankMaster();
            ShowHideDialogBankMaster();
        });

        function SetDialogShowHideBankMaster() {
            $("#divbankdetails").on("hidden.bs.modal", function () {
                ClearBankMaster();
            })
            $("#divbankdetails").on("show.bs.modal", function () {
                SetBankMaster();
            })
            $("#divbankdetails").on("shown.bs.modal", function () {
                $("#" + Prefix + "txtBankName").focus();
            })
        }
        function ShowHideDialogBankMaster() {
            try {
                SetBankMaster();
                if ($("#" + Prefix + "hdnModelBank").val() == "show") {
                    $("#divbankdetails").modal("show");
                }
                else if ($("#" + Prefix + "hdnModelBank").val() == "hide") {
                    ClearBankMaster();
                    $("#divbankdetails").modal("hide");
                }
                else if ($("#" + Prefix + "hdnModelBank").val() == "clear") {
                    ClearBankMaster();
                    $("#divbankdetails").modal("show");
                    $("#" + Prefix + "txtBankName").focus();
                }
                $("#" + Prefix + "hdnModelBank").val("");
                return false;
            }
            catch (ex) {
                alert(ex);
                return false;
            }
        }
        function ClearBankMaster() {
            for (i = 0; i < Page_Validators.length; i++) {
                Page_Validators[i].style.display = "none";
            }
            $("#" + Prefix + "hdnActionBank").val("");
            $("#" + Prefix + "txtBankName").val("");
            $("#" + Prefix + "chkIsEnabled").prop("checked", true);

        }
        function SetBankMaster() {
            for (i = 0; i < Page_Validators.length; i++) {
                Page_Validators[i].errormessage = Page_Validators[i].innerHTML;
            }
            if ($("#" + Prefix + "hdnActionBank").val() == "") {
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
