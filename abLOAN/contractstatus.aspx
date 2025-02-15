<%@ Page Language="C#" MasterPageFile="Site.Master" AutoEventWireup="true" CodeBehind="contractstatus.aspx.cs"
    Inherits="abLOAN.contractstatus" %>

<%@ Register Assembly="CustomDataPager" Namespace="CustomDataPager" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script type="text/javascript">
        $(document).ready(function () {
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequest);

        });
        function EndRequest(sender, args) {

            ShowHideDialogContractStatusMaster();
        }
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphPage" runat="server">
    <div id="divcontractstatus">
        <div class="blockui">
            <asp:UpdatePanel ID="upcontractstatus" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <div class="row">
                        <div style="justify-content: space-between; display: flex;">
                            <div style="padding: 0 15px;">
                                <h4>
                                    <i class="fa fa-th"></i>&nbsp;<%= Resources.Resource.ContractStatusPageTitle %>&nbsp;
                                    <small>
                                        <asp:Label ID="lblRecords" runat="server" Visible="false"></asp:Label></small>
                                    &nbsp;

                                </h4>
                            </div>
                            <div style="padding: 0 15px;">
                                <asp:Button ID="btnNew" runat="server" Text="<% $Resources:Resource, btnNew %>" CssClass="btn btn-primary" data-toggle="modal" data-target="#divcontractstatusdetails" OnClientClick="javascript:return false;" />
                                &nbsp;



                            </div>
                        </div>
                    </div>

                    <asp:ListView ID="lvContractStatusMaster" runat="server" DataKeyNames="ContractStatusMasterId" OnItemCommand="lvContractStatusMaster_ItemCommand" OnItemDataBound="lvContractStatusMaster_ItemDataBound">
                        <LayoutTemplate>
                            <div class="panel panel-default">
                                <div class="table-responsive">
                                    <table id="itemPlaceholderContainer" runat="server" class="table table-hover">
                                        <tr>
                                            <th>
                                                <%= Resources.Resource.ContractStatus %>
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
                                    <asp:Literal ID="ltrlContractStatus" runat="server"></asp:Literal>
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
                            <cc1:DataPager ID="pgrContractStatusMaster" runat="server" OnPagerCommand="pgrContractStatusMaster_ItemCommand"
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

        <div class="modal fade" id="divcontractstatusdetails">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="blockui">
                        <asp:UpdatePanel ID="upcontractstatusdetails" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Panel ID="pnlcontractstatusdetails" runat="server" DefaultButton="btnSaveAndNew">
                                    <asp:HiddenField ID="hdnModelContractStatus" runat="server"></asp:HiddenField>
                                    <asp:HiddenField ID="hdnActionContractStatus" runat="server"></asp:HiddenField>
                                    <div class="modal-header">
                                        <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                                        <h4 class="modal-title"><%= Resources.Resource.ContractStatusFormTitle %></h4>
                                    </div>
                                    <div class="modal-body">
                                        <div class="form-horizontal" role="form">
                                            <asp:HiddenField ID="hdnContractStatusMasterId" runat="server" Visible="false"></asp:HiddenField>
                                            <div class="form-group">
                                                <label class="col-sm-4 control-label"><%= Resources.Resource.ContractStatus %></label>
                                                <div class="col-sm-7">
                                                    <asp:TextBox ID="txtContractStatus" runat="server" CssClass="form-control input-sm" MaxLength="50"></asp:TextBox>
                                                    <div class="text-danger">
                                                        <asp:RequiredFieldValidator ID="rfvContractStatus" runat="server" SetFocusOnError="true" ControlToValidate="txtContractStatus"
                                                            ValidationGroup="contractstatusdetails"><%= Resources.Messages.InputRequired %> <%=Resources.Resource.ContractStatus %></asp:RequiredFieldValidator>
                                                    </div>
                                                </div>
                                            </div>

                                        </div>
                                    </div>
                                    <div class="modal-footer">
                                        <asp:Button ID="btnSaveAndNew" runat="server" Text="<% $Resources:Resource, btnSaveAndNew %>" CssClass="btn btn-primary" OnClick="btnSave_Click" ValidationGroup="contractstatusdetails" />

                                        <asp:Button ID="btnSave" runat="server" Text="<% $Resources:Resource, btnSave %>" CssClass="btn btn-primary" OnClick="btnSave_Click" ValidationGroup="contractstatusdetails" />

                                        <asp:Button ID="btnClose" runat="server" Text="<% $Resources:Resource, btnClose %>" CssClass="btn btn-default" CausesValidation="false" data-dismiss="modal" />

                                    </div>
                                </asp:Panel>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="lvContractStatusMaster" EventName="ItemCommand" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </div>
        </div>
        <asp:ValidationSummary ID="vscontractstatusdetails" runat="server" DisplayMode="BulletList"
            ShowMessageBox="true" ShowSummary="false" ValidationGroup="contractstatusdetails" />

    </div>
    <script type="text/javascript">
        $(document).ready(function () {
            if ($("#" + Prefix + "hdnModelContractStatus").val() == "") {
                ClearContractStatusMaster();
            }
            SetDialogShowHideContractStatusMaster();
            ShowHideDialogContractStatusMaster();
        });

        function SetDialogShowHideContractStatusMaster() {
            $("#divcontractstatusdetails").on("hidden.bs.modal", function () {
                ClearContractStatusMaster();
            })
            $("#divcontractstatusdetails").on("show.bs.modal", function () {
                SetContractStatusMaster();
            })
            $("#divcontractstatusdetails").on("shown.bs.modal", function () {
                $("#" + Prefix + "txtContractStatus").focus();
            })
        }
        function ShowHideDialogContractStatusMaster() {
            try {
                SetContractStatusMaster();
                if ($("#" + Prefix + "hdnModelContractStatus").val() == "show") {
                    $("#divcontractstatusdetails").modal("show");
                }
                else if ($("#" + Prefix + "hdnModelContractStatus").val() == "hide") {
                    ClearContractStatusMaster();
                    $("#divcontractstatusdetails").modal("hide");
                }
                else if ($("#" + Prefix + "hdnModelContractStatus").val() == "clear") {
                    ClearContractStatusMaster();
                    $("#divcontractstatusdetails").modal("show");
                    $("#" + Prefix + "txtContractStatus").focus();
                }
                $("#" + Prefix + "hdnModelContractStatus").val("");
                return false;
            }
            catch (ex) {
                alert(ex);
                return false;
            }
        }
        function ClearContractStatusMaster() {
            for (i = 0; i < Page_Validators.length; i++) {
                Page_Validators[i].style.display = "none";
                Page_Validators[i].errormessage = Page_Validators[i].innerHTML;
            }
            $("#" + Prefix + "hdnActionContractStatus").val("");
            $("#" + Prefix + "txtContractStatus").val("");

        }
        function SetContractStatusMaster() {
            if ($("#" + Prefix + "hdnActionContractStatus").val() == "") {
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
