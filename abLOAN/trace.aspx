<%@ Page Language="C#" MasterPageFile="Site.Master" AutoEventWireup="true" CodeBehind="trace.aspx.cs"
    Inherits="abLOAN.trace" %>

<%@ Register Assembly="CustomDataPager" Namespace="CustomDataPager" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script type="text/javascript">
        $(document).ready(function () {
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequest);
            SetFilter();
        });
        function EndRequest(sender, args) {

            ShowHideDialogTraceMaster();
            SetFilter();
        }
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphPage" runat="server">
    <div id="divtrace">
        <div class="blockui">
            <asp:UpdatePanel ID="uptrace" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <div class="row">
                        <div style="justify-content: space-between; display: flex;">
                            <div style="padding: 0 15px;">
                                <h4>
                                    <i class="fa fa-th"></i>&nbsp;  <%= Resources.Resource.TracePageTitle %>&nbsp;
                                    <small>
                                        <asp:Label ID="lblRecords" runat="server" Visible="false"></asp:Label></small>
                                    &nbsp;
                                    <a id="aFilter" class="btn btn-default btn-sm" href="" style="text-decoration: none;" onclick="javascript:return ShowHideFilter('divFilter', false);"><%= Resources.Resource.Filter %>&nbsp;&nbsp;<span class="caret"></span></a>
                                </h4>
                            </div>
                            <div style="padding: 0 15px;">
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
                                            <div class="col-md-4 col-sm-4">
                                                <div class="form-group">
                                                    <label class="col-sm-5 control-label"><%= Resources.Resource.PageName %></label>
                                                    <div class="col-sm-5">
                                                        <asp:TextBox ID="txtFilterTableName" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-md-4 col-sm-4">
                                                <div class="form-group">
                                                    <label class="col-sm-5 control-label"><%= Resources.Resource.OperationType %></label>
                                                    <div class="col-sm-5">
                                                        <asp:TextBox ID="txtFilterOperationType" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>                                           
                                              <div class="col-md-4 col-sm-4">
                                                <div class="form-group">
                                                    <label class="col-sm-4 control-label"><%= Resources.Resource.OperationDate %></label>
                                                    <div class="col-sm-4">
                                                        <asp:TextBox ID="txtFilterOperationDate" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                                        <div class="text-danger">
                                                            <asp:RequiredFieldValidator ID="rfvFilterOperationDate" runat="server"
                                                                ControlToValidate="txtFilterOperationDate" Display="Dynamic"
                                                                ValidationGroup="FilterOperationMaster"><%= Resources.Messages.InputRequired %> <%=Resources.Resource.OperationDate %></asp:RequiredFieldValidator>
                                                        </div>
                                                    </div>
                                                     <div class="col-sm-4">
                                                        <asp:TextBox ID="txtFilterOperationDateTo" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                                        <div class="text-danger">
                                                            <asp:RequiredFieldValidator ID="rfvFilterOperationDateTo" runat="server"
                                                                ControlToValidate="txtFilterOperationDateTo" Display="Dynamic"
                                                                ValidationGroup="FilterOperationMaster"><%= Resources.Messages.InputRequired %> <%=Resources.Resource.OperationDate %></asp:RequiredFieldValidator>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                                
                                        </div>

                                        <div class="row">
                                            <div class="col-md-12 text-center">
                                                <asp:Button ID="btnSearch" runat="server" Text="<% $Resources:Resource, btnSearch %>" CssClass="btn btn-primary" ValidationGroup="FilterTraceMaster" OnClick="btnSearch_Click" />
                                                &nbsp;
                                                <asp:Button ID="btnReset" runat="server" Text="<% $Resources:Resource, btnReset %>" CssClass="btn btn-default" CausesValidation="false" OnClick="btnReset_Click" />
                                            </div>
                                        </div>
                                    </div>
                                </asp:Panel>
                                <asp:ValidationSummary ID="vsTraceMaster" runat="server" DisplayMode="BulletList"
                                    ShowMessageBox="true" ShowSummary="false" ValidationGroup="FilterTraceMaster" />
                            </div>
                        </div>
                    </div>

                    <asp:ListView ID="lvTraceMaster" runat="server" DataKeyNames="TraceMasterId" OnItemDataBound="lvTraceMaster_ItemDataBound" OnItemCommand="lvTraceMaster_ItemCommand">
                        <LayoutTemplate>
                            <div class="panel panel-default">
                                <div class="table-responsive">
                                    <table id="itemPlaceholderContainer" runat="server" class="table table-hover">
                                        <tr>
                                            <th><%= Resources.Resource.OperationDate %>
                                            </th>
                                            <th><%= Resources.Resource.PageName %>
                                            </th>
                                            <th><%= Resources.Resource.OperationType %>
                                            </th>
                                            <th>
                                                <%= Resources.Resource.Name %>
                                            </th>
                                            <th style="display: none;">Value
                                            </th>
                                            <th style="width: 25px;">

                                            </th>
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
                                    <asp:Literal ID="ltrlCreateDateTime" runat="server"></asp:Literal>
                                </td>
                                <td>
                                    <asp:Literal ID="ltrlTableName" runat="server"></asp:Literal>
                                </td>
                                <td>
                                    <asp:Literal ID="ltrlOperationType" runat="server"></asp:Literal>
                                </td>
                                <td>
                                    <asp:HiddenField ID="hdnRowId" runat="server" />
                                    <asp:HiddenField ID="hdnUrl" runat="server" />
                                    <asp:Literal ID="ltrlModifiedBy" runat="server"></asp:Literal>
                                </td>
                                <td style="display: none;">
                                    <asp:Literal ID="ltrlValue" runat="server"></asp:Literal>
                                </td>
                                 <td>
                                    <asp:LinkButton ID="lbtnView" runat="server" CommandName="ViewRecord" CssClass="btn btn-primary btn-sm fa fa-eye" title="View" Visible="false"></asp:LinkButton>
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
                            <cc1:DataPager ID="pgrTraceMaster" runat="server" OnPagerCommand="pgrTraceMaster_ItemCommand"
                                Visible="false" CssClass="pagination" CurrentPageCssClass="active"></cc1:DataPager>
                        </div>
                    </div>


                </ContentTemplate>

            </asp:UpdatePanel>
        </div>

    </div>
    <script type="text/javascript">
        $(document).ready(function () {
            SetFilterControlsPicker();
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(SetFilterControlsPicker);
        });

        function SetFilterControlsPicker() {
            SetDatePicker(Prefix + "txtFilterOperationDate");
            SetDatePicker(Prefix + "txtFilterOperationDateTo");
        }
    </script>
    <script type="text/javascript">
        $(document).ready(function () {
            if ($("#" + Prefix + "hdnModelTrace").val() == "") {
                ClearTraceMaster();
            }
            SetDialogShowHideTraceMaster();
            ShowHideDialogTraceMaster();
        });

        function SetDialogShowHideTraceMaster() {
            $("#divtracedetails").on("hidden.bs.modal", function () {
                ClearTraceMaster();
            })
            $("#divtracedetails").on("show.bs.modal", function () {
                SetTraceMaster();
            })
            $("#divtracedetails").on("shown.bs.modal", function () {
            })
        }
        function ShowHideDialogTraceMaster() {
            try {
                SetTraceMaster();
                if ($("#" + Prefix + "hdnModelTrace").val() == "show") {
                    $("#divtracedetails").modal("show");
                }
                else if ($("#" + Prefix + "hdnModelTrace").val() == "hide") {
                    ClearTraceMaster();
                    $("#divtracedetails").modal("hide");
                }
                else if ($("#" + Prefix + "hdnModelTrace").val() == "clear") {
                    ClearTraceMaster();
                    $("#divtracedetails").modal("show");
                }
                $("#" + Prefix + "hdnModelTrace").val("");
                return false;
            }
            catch (ex) {
                alert(ex);
                return false;
            }
        }
        function ClearTraceMaster() {
            for (i = 0; i < Page_Validators.length; i++) {
                Page_Validators[i].style.display = "none";
            }
            $("#" + Prefix + "hdnActionTrace").val("");

        }
        function SetTraceMaster() {
            if ($("#" + Prefix + "hdnActionTrace").val() == "") {
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
