<%@ Page Language="C#" MasterPageFile="Site.Master" AutoEventWireup="true" CodeBehind="witness.aspx.cs"
    Inherits="abLOAN.witness" %>

<%@ Register Assembly="CustomDataPager" Namespace="CustomDataPager" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script type="text/javascript">
        $(document).ready(function () {
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequest);
            SetFilter();
        });
        function EndRequest(sender, args) {

            ShowHideDialogWitnessMaster();
            SetFilter();
        }
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphPage" runat="server">
    <div id="divwitness">
        <div class="blockui">
            <asp:UpdatePanel ID="upwitness" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <div class="row">
                        <div style="justify-content: space-between; display: flex;">
                            <div style="padding: 0 15px;">
                                <h4>
                                    <i class="fa fa-th"></i>&nbsp;<%= Resources.Resource.WitnessPageTitle %>&nbsp;
                                    <small>
                                        <asp:Label ID="lblRecords" runat="server" Visible="false"></asp:Label></small>
                                    &nbsp;
                                    <a id="aFilter" class="btn btn-default btn-sm" href="" style="text-decoration: none;" onclick="javascript:return ShowHideFilter('divFilter', false);"><%= Resources.Resource.Filter %>&nbsp;&nbsp;<span class="caret"></span></a>
                                </h4>
                            </div>
                            <div style="padding: 0 15px;">
                                <asp:Button ID="btnNew" runat="server" Text="<% $Resources:Resource, btnNew %>" CssClass="btn btn-primary" data-toggle="modal" data-target="#divwitnessdetails" OnClientClick="javascript:return false;" />
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
                                                    <label class="col-sm-5 control-label"><%= Resources.Resource.WitnessIdNo %></label>
                                                    <div class="col-sm-7">
                                                        <asp:TextBox ID="txtFilterWitness" runat="server" CssClass="form-control input-sm"></asp:TextBox>
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
                                                <asp:Button ID="btnSearch" runat="server" Text="<% $Resources:Resource, btnSearch %>" CssClass="btn btn-primary" ValidationGroup="FilterWitnessMaster" OnClick="btnSearch_Click" />
                                                &nbsp;
                                                <asp:Button ID="btnReset" runat="server" Text="<% $Resources:Resource, btnReset %>" CssClass="btn btn-default" CausesValidation="false" OnClick="btnReset_Click" />
                                            </div>
                                        </div>
                                    </div>
                                </asp:Panel>
                                <asp:ValidationSummary ID="vsWitnessMaster" runat="server" DisplayMode="BulletList"
                                    ShowMessageBox="true" ShowSummary="false" ValidationGroup="FilterWitnessMaster" />
                            </div>
                        </div>
                    </div>

                    <asp:ListView ID="lvWitnessMaster" runat="server" DataKeyNames="WitnessMasterId" OnItemCommand="lvWitnessMaster_ItemCommand" OnItemDataBound="lvWitnessMaster_ItemDataBound">
                        <LayoutTemplate>
                            <div class="panel panel-default">
                                <div class="table-responsive">
                                    <table id="itemPlaceholderContainer" runat="server" class="table table-hover">
                                        <tr>
                                            <th>
                                                <%= Resources.Resource.PhotoIdImageName %>
                                            </th>
                                            <th>
                                                <%= Resources.Resource.Witness %>
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
                                    <asp:Literal ID="ltrlWitnessName" runat="server"></asp:Literal>
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
                            <cc1:DataPager ID="pgrWitnessMaster" runat="server" OnPagerCommand="pgrWitnessMaster_ItemCommand"
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

        <div class="modal fade" id="divwitnessdetails">
            <div class="modal-dialog wide-modal-dialog">
                <div class="modal-content">
                    <div class="blockui">
                        <asp:UpdatePanel ID="upwitnessdetails" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Panel ID="pnlwitnessdetails" runat="server" DefaultButton="btnSaveAndNew">
                                    <asp:HiddenField ID="hdnModelWitness" runat="server"></asp:HiddenField>
                                    <asp:HiddenField ID="hdnActionWitness" runat="server"></asp:HiddenField>
                                    <div class="modal-header">
                                        <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                                        <h4 class="modal-title"><%= Resources.Resource.WitnessFormTitle %></h4>
                                    </div>
                                    <div class="modal-body">
                                        <div class="form-horizontal" role="form">
                                            <div class="row">
                                                <asp:HiddenField ID="hdnWitnessMasterId" runat="server" Visible="false"></asp:HiddenField>
                                                <div class="col-lg-6">
                                                    <div class="form-group">
                                                        <label class="col-sm-4 control-label"><%= Resources.Resource.WitnessName %></label>
                                                        <div class="col-sm-7">
                                                            <asp:TextBox ID="txtWitnessName" runat="server" CssClass="form-control input-sm" MaxLength="50"></asp:TextBox>
                                                            <div class="text-danger">
                                                                <asp:RequiredFieldValidator ID="rfvWitnessName" runat="server" SetFocusOnError="true" ControlToValidate="txtWitnessName" Display="Dynamic"
                                                                    ValidationGroup="witnessdetails"><%= Resources.Messages.InputRequired %> <%=Resources.Resource.WitnessName %></asp:RequiredFieldValidator>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="form-group">
                                                        <label class="col-sm-4 control-label"><%= Resources.Resource.IdNo %></label>
                                                        <div class="col-sm-7">
                                                            <asp:TextBox ID="txtIdNo" runat="server" CssClass="form-control input-sm" MaxLength="10"></asp:TextBox>
                                                            <div class="text-danger">
                                                                <asp:RequiredFieldValidator ID="rfvIdNo" runat="server" SetFocusOnError="true" ControlToValidate="txtIdNo" Display="Dynamic"
                                                                    ValidationGroup="witnessdetails"><%= Resources.Messages.InputRequired %> <%=Resources.Resource.IdNo %></asp:RequiredFieldValidator>
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
                                        <asp:Button ID="btnSaveAndNew" runat="server" Text="<% $Resources:Resource, btnSaveAndNew %>" CssClass="btn btn-primary" OnClick="btnSave_Click" ValidationGroup="witnessdetails" />

                                        <asp:Button ID="btnSave" runat="server" Text="<% $Resources:Resource, btnSave %>" CssClass="btn btn-primary" OnClick="btnSave_Click" ValidationGroup="witnessdetails" />

                                        <asp:Button ID="btnClose" runat="server" Text="<% $Resources:Resource, btnClose %>" CssClass="btn btn-default" CausesValidation="false" data-dismiss="modal" />

                                    </div>
                                </asp:Panel>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="lvWitnessMaster" EventName="ItemCommand" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </div>
        </div>
        <asp:ValidationSummary ID="vswitnessdetails" runat="server" DisplayMode="BulletList"
            ShowMessageBox="true" ShowSummary="false" ValidationGroup="witnessdetails" />

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
            if ($("#" + Prefix + "hdnModelWitness").val() == "") {
                ClearWitnessMaster();
            }
            SetDialogShowHideWitnessMaster();
            ShowHideDialogWitnessMaster();
        });

        function SetDialogShowHideWitnessMaster() {
            $("#divwitnessdetails").on("hidden.bs.modal", function () {
                ClearWitnessMaster();
            })
            $("#divwitnessdetails").on("show.bs.modal", function () {
                SetWitnessMaster();
            })
            $("#divwitnessdetails").on("shown.bs.modal", function () {
                $("#" + Prefix + "txtWitnessName").focus();
                if ($("#" + Prefix + "hdnPhotoIdImageName").val() != "") {
                    $("." + Prefix + "fuPhotoIdImageName").attr("src", $("#" + Prefix + "hdnPhotoIdImageNameURL").val());
                }
            })
        }
        function ShowHideDialogWitnessMaster() {
            try {
                SetWitnessMaster();
                if ($("#" + Prefix + "hdnModelWitness").val() == "show") {
                    $("#divwitnessdetails").modal("show");
                }
                else if ($("#" + Prefix + "hdnModelWitness").val() == "hide") {
                    ClearWitnessMaster();
                    $("#divwitnessdetails").modal("hide");
                }
                else if ($("#" + Prefix + "hdnModelWitness").val() == "clear") {
                    ClearWitnessMaster();
                    $("#divwitnessdetails").modal("show");
                    $("#" + Prefix + "txtWitnessName").focus();
                }
                $("#" + Prefix + "hdnModelWitness").val("");
                return false;
            }
            catch (ex) {
                alert(ex);
                return false;
            }
        }
        function ClearWitnessMaster() {
            for (i = 0; i < Page_Validators.length; i++) {
                Page_Validators[i].style.display = "none";
            }
            $("#" + Prefix + "hdnActionWitness").val("");
            $("#" + Prefix + "txtWitnessName").val("");
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
        function SetWitnessMaster() {
            for (i = 0; i < Page_Validators.length; i++) {
                Page_Validators[i].errormessage = Page_Validators[i].innerHTML;
            }
            if ($("#" + Prefix + "hdnActionWitness").val() == "") {
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
