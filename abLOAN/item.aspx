<%@ Page Language="C#" MasterPageFile="Site.Master" AutoEventWireup="true" CodeBehind="item.aspx.cs"
    Inherits="abLOAN.item" %>

<%@ Register Assembly="CustomDataPager" Namespace="CustomDataPager" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script type="text/javascript">
        $(document).ready(function () {
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequest);
            SetFilter();
        });
        function EndRequest(sender, args) {

            ShowHideDialogItemMaster();
            SetFilter();
        }
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphPage" runat="server">
    <div id="divitem">
        <div class="blockui">
            <asp:UpdatePanel ID="upitem" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <div class="row">
                        <div style="justify-content: space-between; display: flex;">
                            <div style="padding: 0 15px;">
                                <h4>
                                    <i class="fa fa-th"></i>&nbsp;<%= Resources.Resource.ItemPageTitle %>&nbsp;
                                    <small>
                                        <asp:Label ID="lblRecords" runat="server" Visible="false"></asp:Label></small>
                                    &nbsp;
                                    <a id="aFilter" class="btn btn-default btn-sm" href="" style="text-decoration: none;" onclick="javascript:return ShowHideFilter('divFilter', false);"><%= Resources.Resource.Filter %>&nbsp;&nbsp;<span class="caret"></span></a>
                                </h4>
                            </div>
                            <div style="padding: 0 15px;">
                                <asp:Button ID="btnNew" runat="server" Text="<% $Resources:Resource, btnNew %>" CssClass="btn btn-primary" data-toggle="modal" data-target="#divitemdetails" OnClientClick="javascript:return false;" />
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
                                                    <label class="col-sm-5 control-label"><%= Resources.Resource.ItemName %></label>
                                                    <div class="col-sm-7">
                                                        <asp:TextBox ID="txtFilterItemName" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-md-4 col-sm-6">
                                                <div class="form-group">
                                                    <label class="col-sm-5 control-label"><%= Resources.Resource.ItemCode %></label>
                                                    <div class="col-sm-7">
                                                        <asp:TextBox ID="txtFilterItemCode" runat="server" CssClass="form-control input-sm"></asp:TextBox>
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
                                        </div>

                                        <div class="row">
                                            <div class="col-md-12 text-center">
                                                <asp:Button ID="btnSearch" runat="server" Text="<% $Resources:Resource, btnSearch %>" CssClass="btn btn-primary" ValidationGroup="FilterItemMaster" OnClick="btnSearch_Click" />
                                                &nbsp;
                                                <asp:Button ID="btnReset" runat="server" Text="<% $Resources:Resource, btnReset %>" CssClass="btn btn-default" CausesValidation="false" OnClick="btnReset_Click" />
                                            </div>
                                        </div>
                                    </div>
                                </asp:Panel>
                                <asp:ValidationSummary ID="vsItemMaster" runat="server" DisplayMode="BulletList"
                                    ShowMessageBox="true" ShowSummary="false" ValidationGroup="FilterItemMaster" />
                            </div>
                        </div>
                    </div>

                    <asp:ListView ID="lvItemMaster" runat="server" DataKeyNames="ItemMasterId" OnItemCommand="lvItemMaster_ItemCommand" OnItemDataBound="lvItemMaster_ItemDataBound">
                        <LayoutTemplate>
                            <div class="panel panel-default">
                                <div class="table-responsive">
                                    <table id="itemPlaceholderContainer" runat="server" class="table table-hover">
                                        <tr>
                                            <th>
                                                <%= Resources.Resource.ItemName %>
                                            </th>
                                            <th>
                                                <%= Resources.Resource.ItemCode %>
                                            </th>
                                            <th>
                                                <%= Resources.Resource.CategoryName %>
                                            </th>
                                            <th>
                                                <%= Resources.Resource.BrandName %>
                                            </th>
                                            <th>
                                                <%= Resources.Resource.ColorName %>
                                            </th>
                                            <th>
                                                <%= Resources.Resource.CurrentQuantity %>
                                            </th>
                                            <th>
                                                <%= Resources.Resource.Price %>
                                            </th>
                                            <th>
                                                <%= Resources.Resource.SalesPrice %>
                                            </th>
                                            <th>
                                                <%= Resources.Resource.Vat %>
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
                                    <asp:Literal ID="ltrlItemName" runat="server"></asp:Literal>
                                </td>
                                <td>
                                    <asp:Literal ID="ltrlItemCode" runat="server"></asp:Literal>
                                </td>
                                <td>
                                    <asp:Literal ID="ltrlCategory" runat="server"></asp:Literal>
                                </td>
                                <td>
                                    <asp:Literal ID="ltrlBrand" runat="server"></asp:Literal>
                                </td>
                                <td>
                                    <asp:Literal ID="ltrlColor" runat="server"></asp:Literal>
                                </td>
                                <td>
                                    <asp:Literal ID="ltrlCurrentQuantity" runat="server"></asp:Literal>
                                </td>
                                <td>
                                    <asp:Literal ID="ltrlPrice" runat="server"></asp:Literal>
                                </td>
                                <td>
                                    <asp:Literal ID="ltrlSalesPrice" runat="server"></asp:Literal>
                                </td>
                                <td>
                                    <asp:Literal ID="ltrlVat" runat="server"></asp:Literal>
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
                            <cc1:DataPager ID="pgrItemMaster" runat="server" OnPagerCommand="pgrItemMaster_ItemCommand"
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

        <div class="modal fade" id="divitemdetails">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="blockui">
                        <asp:UpdatePanel ID="upitemdetails" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Panel ID="pnlitemdetails" runat="server" DefaultButton="btnSaveAndNew">
                                    <asp:HiddenField ID="hdnModelItem" runat="server"></asp:HiddenField>
                                    <asp:HiddenField ID="hdnActionItem" runat="server"></asp:HiddenField>
                                    <div class="modal-header">
                                        <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                                        <h4 class="modal-title"><%= Resources.Resource.ItemFormTitle %></h4>
                                    </div>
                                    <div class="modal-body">
                                        <div class="form-horizontal" role="form">
                                            <asp:HiddenField ID="hdnItemMasterId" runat="server" Visible="false"></asp:HiddenField>
                                            <div class="form-group">
                                                <label class="col-sm-4 control-label"><%= Resources.Resource.ItemName %></label>
                                                <div class="col-sm-7">
                                                    <asp:TextBox ID="txtItemName" runat="server" CssClass="form-control input-sm" MaxLength="100"></asp:TextBox>
                                                    <div class="text-danger">
                                                        <asp:RequiredFieldValidator ID="rfvItemName" runat="server" SetFocusOnError="true" ControlToValidate="txtItemName" Display="Dynamic"
                                                            ValidationGroup="itemdetails"><%= Resources.Messages.InputRequired %> <%=Resources.Resource.ItemName %></asp:RequiredFieldValidator>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <label class="col-sm-4 control-label"><%= Resources.Resource.ItemCode %></label>
                                                <div class="col-sm-7">
                                                    <asp:TextBox ID="txtItemCode" runat="server" CssClass="form-control input-sm" MaxLength="50"></asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <label class="col-sm-4 control-label"><%= Resources.Resource.ItemDescription %></label>
                                                <div class="col-sm-7">
                                                    <asp:TextBox ID="txtItemDescription" runat="server" Rows="3" CssClass="form-control input-sm" onKeyDown="javascript:CheckTextAreaMaxLength(this, event, 4000);" TextMode="MultiLine"></asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <label class="col-sm-4 control-label"><%= Resources.Resource.CategoryName %></label>
                                                <div class="col-sm-7">
                                                    <asp:DropDownList ID="ddlCategory" runat="server" CssClass="form-control input-sm"></asp:DropDownList>
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <label class="col-sm-4 control-label"><%= Resources.Resource.BrandName %></label>
                                                <div class="col-sm-7">
                                                    <asp:DropDownList ID="ddlBrand" runat="server" CssClass="form-control input-sm"></asp:DropDownList>
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <label class="col-sm-4 control-label"><%= Resources.Resource.ColorName %></label>
                                                <div class="col-sm-7">
                                                    <asp:DropDownList ID="ddlColor" runat="server" CssClass="form-control input-sm"></asp:DropDownList>
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <label class="col-sm-4 control-label"><%= Resources.Resource.Price %></label>
                                                <div class="col-sm-7">
                                                    <asp:TextBox ID="txtPrice" runat="server" Rows="3" CssClass="form-control input-sm"></asp:TextBox>
                                                    <div class="text-danger">
                                                        <asp:RequiredFieldValidator ID="rfvPrice" runat="server" SetFocusOnError="true" ControlToValidate="txtPrice" Display="Dynamic"
                                                            ValidationGroup="itemdetails"><%= Resources.Messages.InputRequired %> <%=Resources.Resource.Price %></asp:RequiredFieldValidator>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <label class="col-sm-4 control-label"><%= Resources.Resource.SalesPrice %></label>
                                                <div class="col-sm-7">
                                                    <asp:TextBox ID="txtSalesPrice" runat="server" Rows="3" CssClass="form-control input-sm"></asp:TextBox>
                                                    <div class="text-danger">
                                                        <asp:RequiredFieldValidator ID="rfvSalesPrice" runat="server" SetFocusOnError="true" ControlToValidate="txtSalesPrice" Display="Dynamic"
                                                            ValidationGroup="itemdetails"><%= Resources.Messages.InputRequired %> <%=Resources.Resource.SalesPrice %></asp:RequiredFieldValidator>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <label class="col-sm-4 control-label"><%= Resources.Resource.Vat %></label>
                                                <div class="col-sm-7">
                                                    <asp:TextBox ID="txtVat" runat="server" Rows="3" CssClass="form-control input-sm"></asp:TextBox>
                                                    <div class="text-danger">
                                                        <asp:RequiredFieldValidator ID="rfvVat" runat="server" SetFocusOnError="true" ControlToValidate="txtVat" Display="Dynamic"
                                                            ValidationGroup="itemdetails"><%= Resources.Messages.InputRequired %> <%=Resources.Resource.Vat %></asp:RequiredFieldValidator>
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
                                        <asp:Button ID="btnSaveAndNew" runat="server" Text="<% $Resources:Resource, btnSaveAndNew %>" CssClass="btn btn-primary" OnClick="btnSave_Click" ValidationGroup="itemdetails" />

                                        <asp:Button ID="btnSave" runat="server" Text="<% $Resources:Resource, btnSave %>" CssClass="btn btn-primary" OnClick="btnSave_Click" ValidationGroup="itemdetails" />

                                        <asp:Button ID="btnClose" runat="server" Text="<% $Resources:Resource, btnClose %>" CssClass="btn btn-default" CausesValidation="false" data-dismiss="modal" />

                                    </div>
                                </asp:Panel>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="lvItemMaster" EventName="ItemCommand" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </div>
        </div>
        <asp:ValidationSummary ID="vsitemdetails" runat="server" DisplayMode="BulletList"
            ShowMessageBox="true" ShowSummary="false" ValidationGroup="itemdetails" />

    </div>
    <script type="text/javascript">
        $(document).ready(function () {
            if ($("#" + Prefix + "hdnModelItem").val() == "") {
                ClearItemMaster();
            }
            SetDialogShowHideItemMaster();
            ShowHideDialogItemMaster();
        });

        function SetDialogShowHideItemMaster() {
            $("#divitemdetails").on("hidden.bs.modal", function () {
                ClearItemMaster();
            })
            $("#divitemdetails").on("show.bs.modal", function () {
                SetItemMaster();
            })
            $("#divitemdetails").on("shown.bs.modal", function () {
                $("#" + Prefix + "txtItemName").focus();
            })
        }
        function ShowHideDialogItemMaster() {
            try {
                SetItemMaster();
                if ($("#" + Prefix + "hdnModelItem").val() == "show") {
                    $("#divitemdetails").modal("show");
                }
                else if ($("#" + Prefix + "hdnModelItem").val() == "hide") {
                    ClearItemMaster();
                    $("#divitemdetails").modal("hide");
                }
                else if ($("#" + Prefix + "hdnModelItem").val() == "clear") {
                    ClearItemMaster();
                    $("#divitemdetails").modal("show");
                    $("#" + Prefix + "txtItemName").focus();
                }
                $("#" + Prefix + "hdnModelItem").val("");
                return false;
            }
            catch (ex) {
                alert(ex);
                return false;
            }
        }
        function ClearItemMaster() {
            for (i = 0; i < Page_Validators.length; i++) {
                Page_Validators[i].style.display = "none";
            }
            $("#" + Prefix + "hdnActionItem").val("");
            $("#" + Prefix + "txtItemName").val("");
            $("#" + Prefix + "txtItemCode").val("");
            $("#" + Prefix + "txtItemDescription").val("");
            $("#" + Prefix + "ddlBrand").prop("selectedIndex", 0);
            $("#" + Prefix + "ddlColor").prop("selectedIndex", 0);
            $("#" + Prefix + "txtPrice").val("");
            $("#" + Prefix + "txtSalesPrice").val("");
            $("#" + Prefix + "chkIsEnabled").prop("checked", true);

        }
        function SetItemMaster() {
            for (i = 0; i < Page_Validators.length; i++) {
                Page_Validators[i].errormessage = Page_Validators[i].innerHTML;
            }
            if ($("#" + Prefix + "hdnActionItem").val() == "") {
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
