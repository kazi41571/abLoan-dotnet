<%@ Page Language="C#" MasterPageFile="Site.Master" AutoEventWireup="true" CodeBehind="company.aspx.cs"
    Inherits="abLOAN.company" %>

<%@ Register Assembly="CustomDataPager" Namespace="CustomDataPager" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script type="text/javascript">
        $(document).ready(function () {
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequest);

        });
        function EndRequest(sender, args) {

            ShowHideDialogCompanyMaster();
        }
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphPage" runat="server">
    <div id="divcompany">
        <div class="blockui">
            <asp:UpdatePanel ID="upcompany" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <div class="row">
                        <div style="justify-content: space-between; display: flex;">
                            <div style="padding: 0 15px;">
                                <h4>
                                    <i class="fa fa-th"></i>&nbsp;<%= Resources.Resource.CompanyPageTitle %>&nbsp;
                                    <small>
                                        <asp:Label ID="lblRecords" runat="server" Visible="false"></asp:Label></small>
                                    &nbsp;

                                </h4>
                            </div>
                            <div style="padding: 0 15px;">
                            </div>
                        </div>
                    </div>

                    <asp:ListView ID="lvCompanyMaster" runat="server" DataKeyNames="CompanyMasterId" OnItemCommand="lvCompanyMaster_ItemCommand" OnItemDataBound="lvCompanyMaster_ItemDataBound">
                        <LayoutTemplate>
                            <div class="panel panel-default">
                                <div class="table-responsive">
                                    <table id="itemPlaceholderContainer" runat="server" class="table table-hover">
                                        <tr>
                                            <th>
                                                <%= Resources.Resource.LogoImageName %>
                                            </th>
                                            <th>
                                                <%= Resources.Resource.CompanyName %>
                                            </th>
                                            <th>
                                                <%= Resources.Resource.CompanyDetails %>
                                            </th>
                                            <th>
                                                <%= Resources.Resource.Address %>
                                            </th>
                                            <th>
                                                <%= Resources.Resource.ContactNo %>
                                            </th>
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
                                    <asp:Image ID="imgLogoImageName" runat="server" CssClass="thumb img_resize_fit"></asp:Image>
                                </td>
                                <td>
                                    <asp:Literal ID="ltrlCompanyName" runat="server"></asp:Literal>
                                </td>
                                <td>
                                    <asp:Literal ID="ltrlCompanyDetails" runat="server"></asp:Literal>
                                </td>
                                <td>
                                    <asp:Literal ID="ltrlAddress" runat="server"></asp:Literal>
                                </td>
                                <td>
                                    <asp:Literal ID="ltrlContactNo" runat="server"></asp:Literal>
                                </td>
                                <td>
                                    <asp:LinkButton ID="lbtnEdit" runat="server" CommandName="EditRecord" CssClass="btn btn-primary btn-sm fa fa-edit" title="Edit"></asp:LinkButton>
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
                            <cc1:DataPager ID="pgrCompanyMaster" runat="server" OnPagerCommand="pgrCompanyMaster_ItemCommand"
                                Visible="false" CssClass="pagination" CurrentPageCssClass="active"></cc1:DataPager>
                        </div>
                    </div>


                </ContentTemplate>
                <Triggers>

                    <asp:PostBackTrigger ControlID="btnSaveAndNew" />
                    <asp:PostBackTrigger ControlID="btnSave" />
                </Triggers>
            </asp:UpdatePanel>
        </div>

        <div class="modal fade" id="divcompanydetails">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="blockui">
                        <asp:UpdatePanel ID="upcompanydetails" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Panel ID="pnlcompanydetails" runat="server" DefaultButton="btnSaveAndNew">
                                    <asp:HiddenField ID="hdnModelCompany" runat="server"></asp:HiddenField>
                                    <asp:HiddenField ID="hdnActionCompany" runat="server"></asp:HiddenField>
                                    <div class="modal-header">
                                        <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                                        <h4 class="modal-title"><%= Resources.Resource.CompanyFormTitle %></h4>
                                    </div>
                                    <div class="modal-body">
                                        <div class="form-horizontal" role="form">
                                            <asp:HiddenField ID="hdnCompanyMasterId" runat="server" Visible="false"></asp:HiddenField>
                                            <div class="form-group">
                                                <label class="col-sm-4 control-label"><%= Resources.Resource.CompanyName %></label>
                                                <div class="col-sm-7">
                                                    <asp:TextBox ID="txtCompanyName" runat="server" CssClass="form-control input-sm" MaxLength="100"></asp:TextBox>
                                                    <div class="text-danger">
                                                        <asp:RequiredFieldValidator ID="rfvCompanyName" runat="server"
                                                            ControlToValidate="txtCompanyName" Display="Dynamic" SetFocusOnError="true" ValidationGroup="companydetails">
                                                            <%= Resources.Messages.InputRequired %> <%=Resources.Resource.CompanyName %>
                                                        </asp:RequiredFieldValidator>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <label class="col-sm-4 control-label"><%= Resources.Resource.CompanyDetails %></label>
                                                <div class="col-sm-7">
                                                    <asp:TextBox ID="txtCompanyDetails" runat="server" Rows="3" CssClass="form-control input-sm" onKeyDown="javascript:CheckTextAreaMaxLength(this, event, 500);" TextMode="MultiLine"></asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <label class="col-sm-4 control-label"><%= Resources.Resource.Address %></label>
                                                <div class="col-sm-7">
                                                    <asp:TextBox ID="txtAddress" runat="server" Rows="3" CssClass="form-control input-sm" onKeyDown="javascript:CheckTextAreaMaxLength(this, event, 500);" TextMode="MultiLine"></asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <label class="col-sm-4 control-label"><%= Resources.Resource.ContactNo %></label>
                                                <div class="col-sm-7">
                                                    <asp:TextBox ID="txtContactNo" runat="server" CssClass="form-control input-sm" MaxLength="100"></asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <label class="col-sm-4 control-label"><%= Resources.Resource.LogoImageName %></label>
                                                <div class="col-sm-7">
                                                    <asp:FileUpload ID="fuLogoImageName" runat="server" />
                                                    <asp:HiddenField ID="hdnLogoImageName" runat="server" />
                                                    <asp:HiddenField ID="hdnLogoImageNameURL" runat="server" />
                                                </div>
                                            </div>

                                        </div>
                                    </div>
                                    <div class="modal-footer">
                                        <asp:Button ID="btnSaveAndNew" runat="server" Text="<% $Resources:Resource, btnSaveAndNew %>" CssClass="btn btn-primary" OnClick="btnSave_Click" ValidationGroup="companydetails" />

                                        <asp:Button ID="btnSave" runat="server" Text="<% $Resources:Resource, btnSave %>" CssClass="btn btn-primary" OnClick="btnSave_Click" ValidationGroup="companydetails" />

                                        <asp:Button ID="btnClose" runat="server" Text="<% $Resources:Resource, btnClose %>" CssClass="btn btn-default" CausesValidation="false" data-dismiss="modal" />

                                    </div>
                                </asp:Panel>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="lvCompanyMaster" EventName="ItemCommand" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </div>
        </div>
        <asp:ValidationSummary ID="vscompanydetails" runat="server" DisplayMode="BulletList"
            ShowMessageBox="true" ShowSummary="false" ValidationGroup="companydetails" />

    </div>
    <script type="text/javascript">
        $(document).ready(function () {
            SetControlsPicker();
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(SetControlsPicker);
        });

        function SetControlsPicker() {
            SetImageUpload(Prefix + "fuLogoImageName");
        }
    </script>
    <script type="text/javascript">
        $(document).ready(function () {
            if ($("#" + Prefix + "hdnModelCompany").val() == "") {
                ClearCompanyMaster();
            }
            SetDialogShowHideCompanyMaster();
            ShowHideDialogCompanyMaster();
        });

        function SetDialogShowHideCompanyMaster() {
            $("#divcompanydetails").on("hidden.bs.modal", function () {
                ClearCompanyMaster();
            })
            $("#divcompanydetails").on("show.bs.modal", function () {
                SetCompanyMaster();
            })
            $("#divcompanydetails").on("shown.bs.modal", function () {
                for (i = 0; i < Page_Validators.length; i++) {
                    Page_Validators[i].style.display = "none";
                }
                $("#" + Prefix + "txtCompanyName").focus();
                if ($("#" + Prefix + "hdnLogoImageName").val() != "") {
                    $("." + Prefix + "fuLogoImageName").attr("src", $("#" + Prefix + "hdnLogoImageNameURL").val());
                }
            })
        }
        function ShowHideDialogCompanyMaster() {
            try {
                SetCompanyMaster();
                if ($("#" + Prefix + "hdnModelCompany").val() == "show") {
                    $("#divcompanydetails").modal("show");
                }
                else if ($("#" + Prefix + "hdnModelCompany").val() == "hide") {
                    ClearCompanyMaster();
                    $("#divcompanydetails").modal("hide");
                }
                else if ($("#" + Prefix + "hdnModelCompany").val() == "clear") {
                    ClearCompanyMaster();
                    $("#divcompanydetails").modal("show");
                    $("#" + Prefix + "txtCompanyName").focus();
                }
                $("#" + Prefix + "hdnModelCompany").val("");
                return false;
            }
            catch (ex) {
                alert(ex);
                return false;
            }
        }
        function ClearCompanyMaster() {
            for (i = 0; i < Page_Validators.length; i++) {
                Page_Validators[i].style.display = "none";
            }
            $("#" + Prefix + "hdnActionCompany").val("");
            $("#" + Prefix + "txtCompanyName").val("");
            $("#" + Prefix + "txtCompanyDetails").val("");
            $("#" + Prefix + "txtAddress").val("");
            $("#" + Prefix + "txtContactNo").val("");
            if ($("#" + Prefix + "fuLogoImageName").val() != "") {
                $("#" + Prefix + "fuLogoImageName").fileinput("clear");
            }
            $("#" + Prefix + "hdnLogoImageName").val("");
            $("." + Prefix + "fuLogoImageName").attr("src", "img/xs_NoImage.png");

        }
        function SetCompanyMaster() {
            for (i = 0; i < Page_Validators.length; i++) {
                Page_Validators[i].errormessage = Page_Validators[i].innerHTML;
            }
            if ($("#" + Prefix + "hdnActionCompany").val() == "") {
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
