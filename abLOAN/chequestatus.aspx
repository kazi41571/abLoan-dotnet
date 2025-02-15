<%@ Page Language="C#" MasterPageFile="Site.Master" AutoEventWireup="true" CodeBehind="chequestatus.aspx.cs"
    Inherits="abLOAN.chequestatus" %>

<%@ Register Assembly="CustomDataPager" Namespace="CustomDataPager" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script type="text/javascript">
        $(document).ready(function () {
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequest);

        });
        function EndRequest(sender, args) {

            ShowHideDialogChequeStatusMaster();
        }
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphPage" runat="server">
    <div id="divchequestatus">
        <div class="blockui">
            <asp:UpdatePanel ID="upchequestatus" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <div class="row">
                        <div style="justify-content: space-between; display: flex;">
                            <div style="padding: 0 15px;">
                                <h4>
                                    <i class="fa fa-th"></i>&nbsp;  <%= Resources.Resource.ChequeStatusPageTitle %>&nbsp;
                                    <small>
                                        <asp:Label ID="lblRecords" runat="server" Visible="false"></asp:Label></small>
                                    &nbsp;

                                </h4>
                            </div>
                            <div style="padding: 0 15px;">
                                <asp:Button ID="btnNew" runat="server" Text="<%$Resources:Resource, btnNew %>" CssClass="btn btn-primary" data-toggle="modal" data-target="#divchequestatusdetails" OnClientClick="javascript:return false;" />
                                &nbsp;



                            </div>
                        </div>
                    </div>

                    <asp:ListView ID="lvChequeStatusMaster" runat="server" DataKeyNames="ChequeStatusMasterId" OnItemCommand="lvChequeStatusMaster_ItemCommand" OnItemDataBound="lvChequeStatusMaster_ItemDataBound">
                        <LayoutTemplate>
                            <div class="panel panel-default">
                                <div class="table-responsive">
                                    <table id="itemPlaceholderContainer" runat="server" class="table table-hover">
                                        <tr>
                                            
                                            <th><%= Resources.Resource.ChequeStatusName %>
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
                                    <asp:Literal ID="ltrlChequeStatusName" runat="server"></asp:Literal>
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
                            <cc1:DataPager ID="pgrChequeStatusMaster" runat="server" OnPagerCommand="pgrChequeStatusMaster_ItemCommand"
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

        <div class="modal fade" id="divchequestatusdetails">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="blockui">
                        <asp:UpdatePanel ID="upchequestatusdetails" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Panel ID="pnlchequestatusdetails" runat="server" DefaultButton="btnSaveAndNew">
                                    <asp:HiddenField ID="hdnModelChequeStatus" runat="server"></asp:HiddenField>
                                    <asp:HiddenField ID="hdnActionChequeStatus" runat="server"></asp:HiddenField>
                                    <div class="modal-header">
                                        <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                                        <h4 class="modal-title"><%= Resources.Resource.ChequeStatusFormTitle %></h4>
                                    </div>
                                    <div class="modal-body">
                                        <div class="form-horizontal" role="form">
                                            <asp:HiddenField ID="hdnChequeStatusMasterId" runat="server" Visible="false"></asp:HiddenField>
                                           
                                            <div class="form-group">
                                                <label class="col-sm-4 control-label"><%= Resources.Resource.ChequeStatusName %></label>
                                                <div class="col-sm-7">
                                                    <asp:TextBox ID="txtChequeStatusName" runat="server" CssClass="form-control input-sm" MaxLength="50"></asp:TextBox>
                                                    <div class="text-danger">
                                                        <asp:RequiredFieldValidator ID="rfvChequeStatusName" runat="server" 
                                                           
                                                            ControlToValidate="txtChequeStatusName" Display="Dynamic" 
                                                            SetFocusOnError="true"
                                                            ValidationGroup="chequestatusdetails">
                                                             <%= Resources.Messages.InputRequired %> <%=Resources.Resource.ChequeStatusName %>
                                                        </asp:RequiredFieldValidator>
                                                    </div>
                                                </div>
                                            </div>

                                        </div>
                                    </div>
                                    <div class="modal-footer">
                                        <asp:Button ID="btnSaveAndNew" runat="server" Text="<% $Resources:Resource, btnSaveAndNew %>" CssClass="btn btn-primary" OnClick="btnSave_Click" ValidationGroup="chequestatusdetails" />

                                        <asp:Button ID="btnSave" runat="server" Text="<% $Resources:Resource, btnSave %>" CssClass="btn btn-primary" OnClick="btnSave_Click" ValidationGroup="chequestatusdetails" />

                                        <asp:Button ID="btnClose" runat="server" Text="<% $Resources:Resource, btnClose %>" CssClass="btn btn-default" CausesValidation="false" data-dismiss="modal" />

                                    </div>
                                </asp:Panel>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="lvChequeStatusMaster" EventName="ItemCommand" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </div>
        </div>
        <asp:ValidationSummary ID="vschequestatusdetails" runat="server" DisplayMode="BulletList"
            ShowMessageBox="true" ShowSummary="false" ValidationGroup="chequestatusdetails" />

    </div>
    <script type="text/javascript">
        $(document).ready(function () {
            if ($("#" + Prefix + "hdnModelChequeStatus").val() == "") {
                ClearChequeStatusMaster();
            }
            SetDialogShowHideChequeStatusMaster();
            ShowHideDialogChequeStatusMaster();
        });

        function SetDialogShowHideChequeStatusMaster() {
            $("#divchequestatusdetails").on("hidden.bs.modal", function () {
                ClearChequeStatusMaster();
            })
            $("#divchequestatusdetails").on("show.bs.modal", function () {
                SetChequeStatusMaster();
            })
            $("#divchequestatusdetails").on("shown.bs.modal", function () {
                $("#" + Prefix + "txtChequeStatusName").focus();
            })
        }
        function ShowHideDialogChequeStatusMaster() {
            try {
                SetChequeStatusMaster();
                if ($("#" + Prefix + "hdnModelChequeStatus").val() == "show") {
                    $("#divchequestatusdetails").modal("show");
                }
                else if ($("#" + Prefix + "hdnModelChequeStatus").val() == "hide") {
                    ClearChequeStatusMaster();
                    $("#divchequestatusdetails").modal("hide");
                }
                else if ($("#" + Prefix + "hdnModelChequeStatus").val() == "clear") {
                    ClearChequeStatusMaster();
                    $("#divchequestatusdetails").modal("show");
                    $("#" + Prefix + "txtChequeStatusName").focus();
                }
                $("#" + Prefix + "hdnModelChequeStatus").val("");
                return false;
            }
            catch (ex) {
                alert(ex);
                return false;
            }
        }
        function ClearChequeStatusMaster() {
            for (i = 0; i < Page_Validators.length; i++) {
                Page_Validators[i].style.display = "none";
            }
            $("#" + Prefix + "hdnActionChequeStatus").val("");
           
            $("#" + Prefix + "txtChequeStatusName").val("");

        }
        function SetChequeStatusMaster() {
            if ($("#" + Prefix + "hdnActionChequeStatus").val() == "") {
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
