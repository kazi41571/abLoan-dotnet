<%@ Page Language="C#" MasterPageFile="Site.Master" AutoEventWireup="true" CodeBehind="guarantor.aspx.cs"
    Inherits="abLOAN.guarantor" %>

<%@ Register Assembly="CustomDataPager" Namespace="CustomDataPager" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script type="text/javascript">
        $(document).ready(function () {
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequest);
            SetFilter();
        });
        function EndRequest(sender, args) {

            ShowHideDialogGuarantorMaster();
            SetFilter();
        }
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphPage" runat="server">
    <div id="divguarantor">
        <div class="blockui">
            <asp:UpdatePanel ID="upguarantor" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <div class="row">
                        <div style="justify-content: space-between; display: flex;">
                            <div style="padding: 0 15px;">
                                <h4>
                                    <i class="fa fa-th"></i>&nbsp;<%= Resources.Resource.GuarantorPageTitle %>&nbsp;
                                    <small>
                                        <asp:Label ID="lblRecords" runat="server" Visible="false"></asp:Label></small>
                                    &nbsp;
                                    <a id="aFilter" class="btn btn-default btn-sm" href="" style="text-decoration: none;" onclick="javascript:return ShowHideFilter('divFilter', false);"><%= Resources.Resource.Filter %>&nbsp;&nbsp;<span class="caret"></span></a>
                                </h4>
                            </div>
                            <div style="padding: 0 15px;">
                                <asp:Button ID="btnNew" runat="server" Text="<% $Resources:Resource, btnNew %>" CssClass="btn btn-primary" data-toggle="modal" data-target="#divguarantordetails" OnClientClick="javascript:return false;" />
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
                                                    <label class="col-sm-5 control-label"><%= Resources.Resource.GuarantorIdNo %></label>
                                                    <div class="col-sm-7">
                                                        <asp:TextBox ID="txtFilterGuarantor" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-md-4 col-sm-6">
                                                <div class="form-group">
                                                    <label class="col-sm-5 control-label"><%= Resources.Resource.PhoneMobile %></label>
                                                    <div class="col-sm-7">
                                                        <asp:TextBox ID="txtFilterPhoneMobile" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>


                                        <div class="row">
                                            <div class="col-md-12 text-center">
                                                <asp:Button ID="btnSearch" runat="server" Text="<% $Resources:Resource, btnSearch %>" CssClass="btn btn-primary" ValidationGroup="FilterGuarantorMaster" OnClick="btnSearch_Click" />
                                                &nbsp;
                                                <asp:Button ID="btnReset" runat="server" Text="<% $Resources:Resource, btnReset %>" CssClass="btn btn-default" CausesValidation="false" OnClick="btnReset_Click" />
                                            </div>
                                        </div>
                                    </div>
                                </asp:Panel>
                                <asp:ValidationSummary ID="vsGuarantorMaster" runat="server" DisplayMode="BulletList"
                                    ShowMessageBox="true" ShowSummary="false" ValidationGroup="FilterGuarantorMaster" />
                            </div>
                        </div>
                    </div>

                    <asp:ListView ID="lvGuarantorMaster" runat="server" DataKeyNames="GuarantorMasterId" OnItemCommand="lvGuarantorMaster_ItemCommand" OnItemDataBound="lvGuarantorMaster_ItemDataBound">
                        <LayoutTemplate>
                            <div class="panel panel-default">
                                <div class="table-responsive">
                                    <table id="itemPlaceholderContainer" runat="server" class="table table-hover">
                                        <tr>
                                            <th>
                                                <%= Resources.Resource.PhotoIdImageName %>
                                            </th>
                                            <th>
                                                <%= Resources.Resource.Guarantor %>
                                            </th>
                                            <th>
                                                <%= Resources.Resource.Mobile %>
                                            </th>
                                            <th>
                                                <%= Resources.Resource.Address1 %>
                                            </th>
                                            <th>
                                                <%= Resources.Resource.Address2 %>
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
                                    <asp:Image ID="imgPhotoIdImageName" runat="server" CssClass="thumb img_resize_fit"></asp:Image>
                                </td>
                                <td>
                                    <span class="text-muted"><%= Resources.Resource.Name %>:</span>
                                    <asp:Literal ID="ltrlGuarantorName" runat="server"></asp:Literal>
                                    <br />
                                    <span class="text-muted"><%= Resources.Resource.IdNo %>:</span>
                                    <asp:Literal ID="ltrlIdNo" runat="server"></asp:Literal>
                                </td>
                                <td>
                                    <span class="text-muted"><%= Resources.Resource.Mobile1 %>:</span>
                                    <asp:Literal ID="ltrlMobile1" runat="server"></asp:Literal>
                                    <br />
                                    <span class="text-muted"><%= Resources.Resource.Mobile2 %>:</span>
                                    <asp:Literal ID="ltrlMobile2" runat="server"></asp:Literal>
                                    <br />
                                    <span class="text-muted"><%= Resources.Resource.Mobile3 %>:</span>
                                    <asp:Literal ID="ltrlMobile3" runat="server"></asp:Literal>
                                </td>
                                <td>
                                    <span class="text-muted"><%= Resources.Resource.Address1 %>:</span>
                                    <asp:Literal ID="ltrlAddress1" runat="server"></asp:Literal>
                                    <br />
                                    <span class="text-muted"><%= Resources.Resource.Phone1 %>:</span>
                                    <asp:Literal ID="ltrlPhone1" runat="server"></asp:Literal>
                                    <br />
                                    <span class="text-muted"><%= Resources.Resource.Fax1 %>:</span>
                                    <asp:Literal ID="ltrlFax1" runat="server"></asp:Literal>
                                </td>
                                <td>
                                    <span class="text-muted"><%= Resources.Resource.Address2 %>:</span>
                                    <asp:Literal ID="ltrlAddress2" runat="server"></asp:Literal>
                                    <br />
                                    <span class="text-muted"><%= Resources.Resource.Phone2 %>:</span>
                                    <asp:Literal ID="ltrlPhone2" runat="server"></asp:Literal>
                                    <br />
                                    <span class="text-muted"><%= Resources.Resource.Fax2 %>:</span>
                                    <asp:Literal ID="ltrlFax2" runat="server"></asp:Literal>
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
                            <cc1:DataPager ID="pgrGuarantorMaster" runat="server" OnPagerCommand="pgrGuarantorMaster_ItemCommand"
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

        <div class="modal fade" id="divguarantordetails">
            <div class="modal-dialog wide-modal-dialog">
                <div class="modal-content">
                    <div class="blockui">
                        <asp:UpdatePanel ID="upguarantordetails" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Panel ID="pnlguarantordetails" runat="server" DefaultButton="btnSaveAndNew">
                                    <asp:HiddenField ID="hdnModelGuarantor" runat="server"></asp:HiddenField>
                                    <asp:HiddenField ID="hdnActionGuarantor" runat="server"></asp:HiddenField>
                                    <div class="modal-header">
                                        <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                                        <h4 class="modal-title"><%= Resources.Resource.GuarantorFormTitle %></h4>
                                    </div>
                                    <div class="modal-body">
                                        <div class="form-horizontal" role="form">
                                                   <div class="row">
                                                <asp:HiddenField ID="hdnGuarantorMasterId" runat="server" Visible="false"></asp:HiddenField>
                                                <div class="col-lg-6">
                                                    <div class="form-group">
                                                        <label class="col-sm-4 control-label"><%= Resources.Resource.GuarantorName %></label>
                                                        <div class="col-sm-7">
                                                            <asp:TextBox ID="txtGuarantorName" runat="server" CssClass="form-control input-sm" MaxLength="50"></asp:TextBox>
                                                            <div class="text-danger">
                                                                <asp:RequiredFieldValidator ID="rfvGuarantorName" runat="server" SetFocusOnError="true" ControlToValidate="txtGuarantorName" Display="Dynamic"
                                                                    ValidationGroup="guarantordetails"><%= Resources.Messages.InputRequired %> <%=Resources.Resource.GuarantorName %></asp:RequiredFieldValidator>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="form-group">
                                                        <label class="col-sm-4 control-label"><%= Resources.Resource.IdNo %></label>
                                                        <div class="col-sm-7">
                                                            <asp:TextBox ID="txtIdNo" runat="server" CssClass="form-control input-sm" MaxLength="10"></asp:TextBox>
                                                            <div class="text-danger">
                                                                <asp:RequiredFieldValidator ID="rfvIdNo" runat="server" SetFocusOnError="true" ControlToValidate="txtIdNo" Display="Dynamic"
                                                                    ValidationGroup="guarantordetails"><%= Resources.Messages.InputRequired %> <%=Resources.Resource.IdNo %></asp:RequiredFieldValidator>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="form-group">
                                                        <label class="col-sm-4 control-label"><%= Resources.Resource.PhotoIdImageName %></label>
                                                        <div class="col-sm-7">
                                                            <asp:FileUpload ID="fuPhotoIdImageName" runat="server" />
                                                            <asp:HiddenField ID="hdnPhotoIdImageName" runat="server" />
                                                            <asp:HiddenField ID="hdnPhotoIdImageNameURL" runat="server" />

                                                        </div>
                                                    </div>
                                                    <div class="form-group">
                                                        <label class="col-sm-4 control-label"><%= Resources.Resource.Mobile1 %></label>
                                                        <div class="col-sm-7">
                                                            <asp:TextBox ID="txtMobile1" runat="server" CssClass="form-control input-sm" MaxLength="15"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                    <div class="form-group">
                                                        <label class="col-sm-4 control-label"><%= Resources.Resource.Mobile2 %></label>
                                                        <div class="col-sm-7">
                                                            <asp:TextBox ID="txtMobile2" runat="server" CssClass="form-control input-sm" MaxLength="15"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                    <div class="form-group">
                                                        <label class="col-sm-4 control-label"><%= Resources.Resource.Mobile3 %></label>
                                                        <div class="col-sm-7">
                                                            <asp:TextBox ID="txtMobile3" runat="server" CssClass="form-control input-sm" MaxLength="15"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="col-lg-6">
                                                    <div class="form-group">
                                                        <label class="col-sm-4 control-label"><%= Resources.Resource.Address1 %></label>
                                                        <div class="col-sm-7">
                                                            <asp:TextBox ID="txtAddress1" TextMode="MultiLine" Rows="5" runat="server" CssClass="form-control input-sm" MaxLength="100"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                    <div class="form-group">
                                                        <label class="col-sm-4 control-label"><%= Resources.Resource.Phone1 %></label>
                                                        <div class="col-sm-7">
                                                            <asp:TextBox ID="txtPhone1" runat="server" CssClass="form-control input-sm" MaxLength="15"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                    <div class="form-group">
                                                        <label class="col-sm-4 control-label"><%= Resources.Resource.Fax1 %></label>
                                                        <div class="col-sm-7">
                                                            <asp:TextBox ID="txtFax1" runat="server" CssClass="form-control input-sm" MaxLength="15"></asp:TextBox>
                                                        </div>
                                                    </div>

                                                   <br />
                                                    <div class="form-group">
                                                        <label class="col-sm-4 control-label"><%= Resources.Resource.Address2 %></label>
                                                        <div class="col-sm-7">
                                                            <asp:TextBox ID="txtAddress2" TextMode="MultiLine" Rows="5" runat="server" CssClass="form-control input-sm" MaxLength="100"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                    <div class="form-group">
                                                        <label class="col-sm-4 control-label"><%= Resources.Resource.Phone2 %></label>
                                                        <div class="col-sm-7">
                                                            <asp:TextBox ID="txtPhone2" runat="server" CssClass="form-control input-sm" MaxLength="15"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                    <div class="form-group">
                                                        <label class="col-sm-4 control-label"><%= Resources.Resource.Fax2 %></label>
                                                        <div class="col-sm-7">
                                                            <asp:TextBox ID="txtFax2" runat="server" CssClass="form-control input-sm" MaxLength="15"></asp:TextBox>
                                                        </div>
                                                    </div>

                                                </div>
                                            </div>

                                        </div>
                                    </div>
                                    <div class="modal-footer">
                                        <asp:Button ID="btnSaveAndNew" runat="server" Text="<% $Resources:Resource, btnSaveAndNew %>" CssClass="btn btn-primary" OnClick="btnSave_Click" ValidationGroup="guarantordetails" />

                                        <asp:Button ID="btnSave" runat="server" Text="<% $Resources:Resource, btnSave %>" CssClass="btn btn-primary" OnClick="btnSave_Click" ValidationGroup="guarantordetails" />

                                        <asp:Button ID="btnClose" runat="server" Text="<% $Resources:Resource, btnClose %>" CssClass="btn btn-default" CausesValidation="false" data-dismiss="modal" />

                                    </div>
                                </asp:Panel>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="lvGuarantorMaster" EventName="ItemCommand" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </div>
        </div>
        <asp:ValidationSummary ID="vsguarantordetails" runat="server" DisplayMode="BulletList"
            ShowMessageBox="true" ShowSummary="false" ValidationGroup="guarantordetails" />

    </div>
    <script type="text/javascript">
        $(document).ready(function () {
            SetControlsPicker();
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(SetControlsPicker);
        });

        function SetControlsPicker() {
            SetImageUpload(Prefix + "fuPhotoIdImageName");
        }
    </script>
    <script type="text/javascript">
        $(document).ready(function () {
            if ($("#" + Prefix + "hdnModelGuarantor").val() == "") {
                ClearGuarantorMaster();
            }
            SetDialogShowHideGuarantorMaster();
            ShowHideDialogGuarantorMaster();
        });

        function SetDialogShowHideGuarantorMaster() {
            $("#divguarantordetails").on("hidden.bs.modal", function () {
                ClearGuarantorMaster();
            })
            $("#divguarantordetails").on("show.bs.modal", function () {
                SetGuarantorMaster();
            })
            $("#divguarantordetails").on("shown.bs.modal", function () {
                $("#" + Prefix + "txtGuarantorName").focus();
                if ($("#" + Prefix + "hdnPhotoIdImageName").val() != "") {
                    $("." + Prefix + "fuPhotoIdImageName").attr("src", $("#" + Prefix + "hdnPhotoIdImageNameURL").val());
                }
            })
        }
        function ShowHideDialogGuarantorMaster() {
            try {
                SetGuarantorMaster();
                if ($("#" + Prefix + "hdnModelGuarantor").val() == "show") {
                    $("#divguarantordetails").modal("show");
                }
                else if ($("#" + Prefix + "hdnModelGuarantor").val() == "hide") {
                    ClearGuarantorMaster();
                    $("#divguarantordetails").modal("hide");
                }
                else if ($("#" + Prefix + "hdnModelGuarantor").val() == "clear") {
                    ClearGuarantorMaster();
                    $("#divguarantordetails").modal("show");
                    $("#" + Prefix + "txtGuarantorName").focus();
                }
                $("#" + Prefix + "hdnModelGuarantor").val("");
                return false;
            }
            catch (ex) {
                alert(ex);
                return false;
            }
        }
        function ClearGuarantorMaster() {
            for (i = 0; i < Page_Validators.length; i++) {
                Page_Validators[i].style.display = "none";
            }
            $("#" + Prefix + "hdnActionGuarantor").val("");
            $("#" + Prefix + "txtGuarantorName").val("");
            $("#" + Prefix + "txtIdNo").val("");
            if ($("#" + Prefix + "fuPhotoIdImageName").val() != "") {
                $("#" + Prefix + "fuPhotoIdImageName").fileinput("clear");
            }
            $("#" + Prefix + "hdnPhotoIdImageName").val("");
            $("." + Prefix + "fuPhotoIdImageName").attr("src", "img/xs_NoImage.png");            
            $("#" + Prefix + "txtAddress1").val("");
            $("#" + Prefix + "txtPhone1").val("");
            $("#" + Prefix + "txtMobile1").val("");
            $("#" + Prefix + "txtFax1").val("");
            $("#" + Prefix + "txtAddress2").val("");
            $("#" + Prefix + "txtPhone2").val("");
            $("#" + Prefix + "txtMobile2").val("");
            $("#" + Prefix + "txtFax2").val("");
            $("#" + Prefix + "txtCity").val("");

        }
        function SetGuarantorMaster() {
            for (i = 0; i < Page_Validators.length; i++) {
                Page_Validators[i].errormessage = Page_Validators[i].innerHTML;
            }
            if ($("#" + Prefix + "hdnActionGuarantor").val() == "") {
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
