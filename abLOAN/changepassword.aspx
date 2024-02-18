<%@ Page Title="" Language="C#" MasterPageFile="Site.Master" AutoEventWireup="true"
    CodeBehind="changepassword.aspx.cs" Inherits="abLOAN.changepassword" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphPage" runat="server">
    <div class="row">
        <div class="col-md-3">
        </div>
        <div class="col-md-6">
            <div id="divchangepassword">
                <div class="blockui">
                    <asp:UpdatePanel ID="upchangePassword" runat="server">
                        <ContentTemplate>
                            <asp:Panel ID="pnlChangePassword" runat="server" DefaultButton="btnSave">
                                <div class="panel panel-default">
                                    <div class="panel-heading">
                                        <h5><%= Resources.Resource.ChangePasswordPageTitle %></h5>
                                    </div>
                                    <div class="panel-body">
                                        <div class="form-horizontal" role="form">
                                            <div class="form-group">
                                                <label class="col-sm-4 control-label">
                                                    <%= Resources.Resource.OldPassword %></label>
                                                <div class="col-sm-7">
                                                    <%-- start - for firefox and chrome disable autofill --%>
                                                    <input name="fakePassword" style="display: none;" type="password" />
                                                    <%-- end --%>
                                                    <asp:TextBox ID="txtOldPassword" runat="server" CssClass="form-control input-sm"
                                                        MaxLength="25" TextMode="Password"></asp:TextBox>
                                                    <div class="text-danger">
                                                        <asp:HiddenField ID="hdnChangePassword" runat="server" />
                                                        <asp:Label ID="lblpasswordmessage" runat="server" Visible="false" Text=""></asp:Label>
                                                        <asp:RequiredFieldValidator ID="rfvOldPassword" runat="server"
                                                            ControlToValidate="txtOldPassword" Display="Dynamic" SetFocusOnError="true" ValidationGroup="changepassword">
                                                            <%= Resources.Messages.InputRequired %> <%=Resources.Resource.OldPassword %>
                                                        </asp:RequiredFieldValidator>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <label class="col-sm-4 control-label">
                                                    <%= Resources.Resource.NewPassword %></label>
                                                <div class="col-sm-7">
                                                    <asp:TextBox ID="txtNewPassword" runat="server" CssClass="form-control input-sm"
                                                        MaxLength="25" TextMode="Password"></asp:TextBox>
                                                    <div class="text-danger">
                                                        <asp:RequiredFieldValidator ID="rfvNewPassword" runat="server" ControlToValidate="txtNewPassword" Display="Dynamic" SetFocusOnError="true"
                                                            ValidationGroup="changepassword">
                                                             <%= Resources.Messages.InputRequired %> <%=Resources.Resource.NewPassword %>
                                                        </asp:RequiredFieldValidator>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <label class="col-sm-4 control-label">
                                                    <%= Resources.Resource.ConfirmPassword %></label>
                                                <div class="col-sm-7">
                                                    <asp:TextBox ID="txtConfirmPassword" runat="server" CssClass="form-control input-sm"
                                                        MaxLength="25" TextMode="Password"></asp:TextBox>
                                                    <div class="text-danger">
                                                        <asp:RequiredFieldValidator ID="rfvConfirmPassword" runat="server"
                                                            ControlToValidate="txtConfirmPassword" Display="Dynamic" SetFocusOnError="true"
                                                            ValidationGroup="changepassword">
                                                            <%= Resources.Messages.InputRequired %> <%=Resources.Resource.ConfirmPassword %>
                                                        </asp:RequiredFieldValidator>
                                                        <asp:CompareValidator ID="cvMatchPassword" runat="server" ValidationGroup="changepassword"
                                                            Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtConfirmPassword"
                                                            ControlToCompare="txtNewPassword">
                                                            <%=Resources.Resource.ConfirmPassword %> <%= Resources.Messages.CompareValidator %>
                                                        </asp:CompareValidator>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="modal-footer">

                                        <asp:Button ID="btnSave" runat="server" Text="<% $Resources:Resource, btnSave %>" CssClass="btn btn-primary" ValidationGroup="changepassword"
                                            OnClick="btnSave_Click" />
                                        &nbsp;
                                            <asp:Button ID="btnClose" runat="server" Text="<% $Resources:Resource, btnClose %>" CssClass="btn btn-default"
                                                CausesValidation="false" PostBackUrl="default.aspx" />

                                    </div>
                                </div>
                            </asp:Panel>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
        <div class="col-md-3">
        </div>
    </div>
    <asp:ValidationSummary ID="vschangepassword" runat="server" DisplayMode="BulletList"
        ShowMessageBox="true" ShowSummary="false" ValidationGroup="changepassword" />
    <script type="text/javascript">
        $(document).ready(function () {
            $("#" + Prefix + "txtOldPassword").focus();
        });
    </script>
</asp:Content>
