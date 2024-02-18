<%@ Page Language="C#" MasterPageFile="Site.Master" AutoEventWireup="true" CodeBehind="installmentstatus.aspx.cs"
    Inherits="abLOAN.installmentstatus" %>

<%@ Register Assembly="CustomDataPager" Namespace="CustomDataPager" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script type="text/javascript">
        $(document).ready(function () {
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequest);

        });
        function EndRequest(sender, args) {

            ShowHideDialogInstallmentStatusMaster();
        }
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphPage" runat="server">
    <div id="divinstallmentstatus">
        <div class="blockui">
            <asp:UpdatePanel ID="upinstallmentstatus" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <div class="row">
                        <div style="justify-content: space-between; display: flex;">
                            <div style="padding: 0 15px;">
                                <h4>
                                    <i class="fa fa-th"></i>&nbsp;<%= Resources.Resource.InstallmentStatusPageTitle %>&nbsp;
                                    <small>
                                        <asp:Label ID="lblRecords" runat="server" Visible="false"></asp:Label></small>
                                    &nbsp;

                                </h4>
                            </div>
                            <div style="padding: 0 15px;">
                                <asp:Button ID="btnNew" runat="server" Text="<% $Resources:Resource, btnNew %>" CssClass="btn btn-primary" data-toggle="modal" data-target="#divinstallmentstatusdetails" OnClientClick="javascript:return false;" />
                                &nbsp;



                            </div>
                        </div>
                    </div>

                    <asp:ListView ID="lvInstallmentStatusMaster" runat="server" DataKeyNames="InstallmentStatusMasterId" OnItemCommand="lvInstallmentStatusMaster_ItemCommand" OnItemDataBound="lvInstallmentStatusMaster_ItemDataBound">
                        <LayoutTemplate>
                            <div class="panel panel-default">
                                <div class="table-responsive">
                                    <table id="itemPlaceholderContainer" runat="server" class="table table-hover">
                                        <tr>
                                            <th>
                                                <%= Resources.Resource.InstallmentStatus %>
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
                                    <asp:Literal ID="ltrlInstallmentStatus" runat="server"></asp:Literal>
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
                            <cc1:DataPager ID="pgrInstallmentStatusMaster" runat="server" OnPagerCommand="pgrInstallmentStatusMaster_ItemCommand"
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

        <div class="modal fade" id="divinstallmentstatusdetails">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="blockui">
                        <asp:UpdatePanel ID="upinstallmentstatusdetails" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Panel ID="pnlinstallmentstatusdetails" runat="server" DefaultButton="btnSaveAndNew">
                                    <asp:HiddenField ID="hdnModelInstallmentStatus" runat="server"></asp:HiddenField>
                                    <asp:HiddenField ID="hdnActionInstallmentStatus" runat="server"></asp:HiddenField>
                                    <div class="modal-header">
                                        <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                                        <h4 class="modal-title"><%= Resources.Resource.InstallmentStatusFormTitle %></h4>
                                    </div>
                                    <div class="modal-body">
                                        <div class="form-horizontal" role="form">
                                            <asp:HiddenField ID="hdnInstallmentStatusMasterId" runat="server" Visible="false"></asp:HiddenField>
                                            <div class="form-group">
                                                <label class="col-sm-4 control-label"><%= Resources.Resource.InstallmentStatus %></label>
                                                <div class="col-sm-7">
                                                    <asp:TextBox ID="txtInstallmentStatus" runat="server" CssClass="form-control input-sm" MaxLength="50"></asp:TextBox>
                                                    <div class="text-danger">
                                                        <asp:RequiredFieldValidator ID="rfvInstallmentStatus" runat="server" SetFocusOnError="true" ControlToValidate="txtInstallmentStatus"
                                                            ValidationGroup="installmentstatusdetails"><%= Resources.Messages.InputRequired %> <%=Resources.Resource.InstallmentStatus %></asp:RequiredFieldValidator>
                                                    </div>
                                                </div>
                                            </div>

                                        </div>
                                    </div>
                                    <div class="modal-footer">
                                        <asp:Button ID="btnSaveAndNew" runat="server" Text="<% $Resources:Resource, btnSaveAndNew %>" CssClass="btn btn-primary" OnClick="btnSave_Click" ValidationGroup="installmentstatusdetails" />

                                        <asp:Button ID="btnSave" runat="server" Text="<% $Resources:Resource, btnSave %>" CssClass="btn btn-primary" OnClick="btnSave_Click" ValidationGroup="installmentstatusdetails" />

                                        <asp:Button ID="btnClose" runat="server" Text="<% $Resources:Resource, btnClose %>" CssClass="btn btn-default" CausesValidation="false" data-dismiss="modal" />

                                    </div>
                                </asp:Panel>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="lvInstallmentStatusMaster" EventName="ItemCommand" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </div>
        </div>
        <asp:ValidationSummary ID="vsinstallmentstatusdetails" runat="server" DisplayMode="BulletList"
            ShowMessageBox="true" ShowSummary="false" ValidationGroup="installmentstatusdetails" />

    </div>
    <script type="text/javascript">
        $(document).ready(function () {
            if ($("#" + Prefix + "hdnModelInstallmentStatus").val() == "") {
                ClearInstallmentStatusMaster();
            }
            SetDialogShowHideInstallmentStatusMaster();
            ShowHideDialogInstallmentStatusMaster();
        });

        function SetDialogShowHideInstallmentStatusMaster() {
            $("#divinstallmentstatusdetails").on("hidden.bs.modal", function () {
                ClearInstallmentStatusMaster();
            })
            $("#divinstallmentstatusdetails").on("show.bs.modal", function () {
                SetInstallmentStatusMaster();
            })
            $("#divinstallmentstatusdetails").on("shown.bs.modal", function () {
                $("#" + Prefix + "txtInstallmentStatus").focus();
            })
        }
        function ShowHideDialogInstallmentStatusMaster() {
            try {
                SetInstallmentStatusMaster();
                if ($("#" + Prefix + "hdnModelInstallmentStatus").val() == "show") {
                    $("#divinstallmentstatusdetails").modal("show");
                }
                else if ($("#" + Prefix + "hdnModelInstallmentStatus").val() == "hide") {
                    ClearInstallmentStatusMaster();
                    $("#divinstallmentstatusdetails").modal("hide");
                }
                else if ($("#" + Prefix + "hdnModelInstallmentStatus").val() == "clear") {
                    ClearInstallmentStatusMaster();
                    $("#divinstallmentstatusdetails").modal("show");
                    $("#" + Prefix + "txtInstallmentStatus").focus();
                }
                $("#" + Prefix + "hdnModelInstallmentStatus").val("");
                return false;
            }
            catch (ex) {
                alert(ex);
                return false;
            }
        }
        function ClearInstallmentStatusMaster() {
            for (i = 0; i < Page_Validators.length; i++) {
                Page_Validators[i].style.display = "none";
                Page_Validators[i].errormessage = Page_Validators[i].innerHTML;
            }
            $("#" + Prefix + "hdnActionInstallmentStatus").val("");
            $("#" + Prefix + "txtInstallmentStatus").val("");

        }
        function SetInstallmentStatusMaster() {
            if ($("#" + Prefix + "hdnActionInstallmentStatus").val() == "") {
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
