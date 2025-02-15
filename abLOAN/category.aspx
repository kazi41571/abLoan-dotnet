<%@ Page Language="C#" MasterPageFile="Site.Master" AutoEventWireup="true" CodeBehind="category.aspx.cs"
    Inherits="abLOAN.category" %>

<%@ Register Assembly="CustomDataPager" Namespace="CustomDataPager" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script type="text/javascript">
        $(document).ready(function () {
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequest);
            SetFilter();
        });
        function EndRequest(sender, args) {

            ShowHideDialogCategoryMaster();
            SetFilter();
        }
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphPage" runat="server">
    <div id="divcategory">
        <div class="blockui">
            <asp:UpdatePanel ID="upcategory" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <div class="row">
                        <div class="col-md-12">
                            <div class="pull-left">
                                <h4>
                                    <i class="fa fa-th"></i>&nbsp;Category&nbsp;
                                    <small>
                                        <asp:Label ID="lblRecords" runat="server" Visible="false"></asp:Label></small>
                                    &nbsp;
                                    <a id="aFilter" class="btn btn-default btn-sm" href="" style="text-decoration: none;" onclick="javascript:return ShowHideFilter('divFilter', false);">Show filter&nbsp;&nbsp;<span class="caret"></span></a>
                                </h4>
                            </div>
                            <div class="pull-right">
                                <asp:Button ID="btnNew" runat="server" Text="New" CssClass="btn btn-primary" data-toggle="modal" data-target="#divcategorydetails" OnClientClick="javascript:return false;" />
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
                                                    <label class="col-sm-5 control-label">Category Name</label>
                                                    <div class="col-sm-7">
                                                        <asp:TextBox ID="txtFilterCategoryName" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-md-4 col-sm-6">
                                                <div class="form-group">
                                                    <label class="col-sm-5 control-label">Enabled</label>
                                                    <div class="col-sm-7">
                                                        <asp:DropDownList ID="ddlFilterIsEnabled" runat="server" CssClass="form-control input-sm">
                                                            <asp:ListItem Text="Yes" Value="Yes"></asp:ListItem>
                                                            <asp:ListItem Text="No" Value="No"></asp:ListItem>
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="row">
                                            <div class="col-md-12 text-center">
                                                <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="btn btn-primary" ValidationGroup="FilterCategoryMaster" OnClick="btnSearch_Click" />
                                                &nbsp;
                                                <asp:Button ID="btnReset" runat="server" Text="Reset" CssClass="btn btn-default" CausesValidation="false" OnClick="btnReset_Click" />
                                            </div>
                                        </div>
                                    </div>
                                </asp:Panel>
                                <asp:ValidationSummary ID="vsCategoryMaster" runat="server" DisplayMode="BulletList"
                                    ShowMessageBox="true" ShowSummary="false" ValidationGroup="FilterCategoryMaster" />
                            </div>
                        </div>
                    </div>

                    <asp:ListView ID="lvCategoryMaster" runat="server" DataKeyNames="CategoryMasterId" OnItemCommand="lvCategoryMaster_ItemCommand" OnItemDataBound="lvCategoryMaster_ItemDataBound">
                        <LayoutTemplate>
                            <div class="panel panel-default">
                                <div class="table-responsive">
                                    <table id="itemPlaceholderContainer" runat="server" class="table table-hover">
                                        <tr>
                                            <th>Category Name
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
                                    <asp:Literal ID="ltrlCategoryName" runat="server"></asp:Literal>
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
                                No record found.
                            </div>
                        </EmptyDataTemplate>
                    </asp:ListView>
                    <div class="row">
                        <div class="col-md-12 text-center">
                            <cc1:DataPager ID="pgrCategoryMaster" runat="server" OnPagerCommand="pgrCategoryMaster_ItemCommand"
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

        <div class="modal fade" id="divcategorydetails">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="blockui">
                        <asp:UpdatePanel ID="upcategorydetails" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Panel ID="pnlcategorydetails" runat="server" DefaultButton="btnSaveAndNew">
                                    <asp:HiddenField ID="hdnModelCategory" runat="server"></asp:HiddenField>
                                    <asp:HiddenField ID="hdnActionCategory" runat="server"></asp:HiddenField>
                                    <div class="modal-header">
                                        <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                                        <h4 class="modal-title">Category Details</h4>
                                    </div>
                                    <div class="modal-body">
                                        <div class="form-horizontal" role="form">
                                            <asp:HiddenField ID="hdnCategoryMasterId" runat="server" Visible="false"></asp:HiddenField>
                                            <div class="form-group">
                                                <label class="col-sm-4 control-label">Category Name</label>
                                                <div class="col-sm-7">
                                                    <asp:TextBox ID="txtCategoryName" runat="server" CssClass="form-control input-sm" MaxLength="50"></asp:TextBox>
                                                    <div class="text-danger">
                                                        <asp:RequiredFieldValidator ID="rfvCategoryName" runat="server" ErrorMessage="Please enter Category Name"
                                                            ControlToValidate="txtCategoryName" Display="Dynamic" SetFocusOnError="true" Text="Please enter Category Name" ValidationGroup="categorydetails"></asp:RequiredFieldValidator>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <div class="col-sm-offset-4 col-sm-7">
                                                    <div class="checkbox">
                                                        <label>
                                                            <asp:CheckBox ID="chkIsEnabled" runat="server" Checked="true" Text="Enabled"></asp:CheckBox>
                                                        </label>
                                                    </div>
                                                </div>
                                            </div>

                                        </div>
                                    </div>
                                    <div class="modal-footer">
                                        <asp:Button ID="btnSaveAndNew" runat="server" Text="Save & New" CssClass="btn btn-primary" OnClick="btnSave_Click" ValidationGroup="categorydetails" />

                                        <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="btn btn-primary" OnClick="btnSave_Click" ValidationGroup="categorydetails" />

                                        <asp:Button ID="btnClose" runat="server" Text="Close" CssClass="btn btn-default" CausesValidation="false" data-dismiss="modal" />

                                    </div>
                                </asp:Panel>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="lvCategoryMaster" EventName="ItemCommand" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </div>
        </div>
        <asp:ValidationSummary ID="vscategorydetails" runat="server" DisplayMode="BulletList"
            ShowMessageBox="true" ShowSummary="false" ValidationGroup="categorydetails" />

    </div>
    <script type="text/javascript">
        $(document).ready(function () {
            if ($("#" + Prefix + "hdnModelCategory").val() == "") {
                ClearCategoryMaster();
            }
            SetDialogShowHideCategoryMaster();
            ShowHideDialogCategoryMaster();
        });

        function SetDialogShowHideCategoryMaster() {
            $("#divcategorydetails").on("hidden.bs.modal", function () {
                ClearCategoryMaster();
            })
            $("#divcategorydetails").on("show.bs.modal", function () {
                SetCategoryMaster();
            })
            $("#divcategorydetails").on("shown.bs.modal", function () {
                $("#" + Prefix + "txtCategoryName").focus();
            })
        }
        function ShowHideDialogCategoryMaster() {
            try {
                SetCategoryMaster();
                if ($("#" + Prefix + "hdnModelCategory").val() == "show") {
                    $("#divcategorydetails").modal("show");
                }
                else if ($("#" + Prefix + "hdnModelCategory").val() == "hide") {
                    ClearCategoryMaster();
                    $("#divcategorydetails").modal("hide");
                }
                else if ($("#" + Prefix + "hdnModelCategory").val() == "clear") {
                    ClearCategoryMaster();
                    $("#divcategorydetails").modal("show");
                    $("#" + Prefix + "txtCategoryName").focus();
                }
                $("#" + Prefix + "hdnModelCategory").val("");
                return false;
            }
            catch (ex) {
                alert(ex);
                return false;
            }
        }
        function ClearCategoryMaster() {
            for (i = 0; i < Page_Validators.length; i++) {
                Page_Validators[i].style.display = "none";
            }
            $("#" + Prefix + "hdnActionCategory").val("");
            $("#" + Prefix + "txtCategoryName").val("");
            $("#" + Prefix + "chkIsEnabled").prop("checked", true);

        }
        function SetCategoryMaster() {
            if ($("#" + Prefix + "hdnActionCategory").val() == "") {
                $("#" + Prefix + "btnSaveAndNew").css("visibility", "visible");
                $("#" + Prefix + "btnSave").val("Save");
            }
            else {
                $("#" + Prefix + "btnSaveAndNew").css("visibility", "hidden");
                $("#" + Prefix + "btnSave").val("Update");
            }
        }
    </script>

</asp:Content>
