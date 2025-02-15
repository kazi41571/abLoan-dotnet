<%@ Page Language="C#" MasterPageFile="Site.Master" AutoEventWireup="true" CodeBehind="customer.aspx.cs"
    Inherits="abLOAN.customer" %>

<%@ Register Assembly="CustomDataPager" Namespace="CustomDataPager" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script type="text/javascript">
        $(document).ready(function () {
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequest);
            SetFilter();
        });
        function EndRequest(sender, args) {

            ShowHideDialogCustomerMaster();
            SetFilter();
        }
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphPage" runat="server">
    <div id="divcustomer">
        <div class="blockui">
            <asp:UpdatePanel ID="upcustomer" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <div class="row">
                        <div style="justify-content: space-between; display: flex;">
                            <div style="padding: 0 15px;">
                                <h4>
                                    <i class="fa fa-th"></i>&nbsp;<%= Resources.Resource.CustomerPageTitle %>&nbsp;
                                    <small>
                                        <asp:Label ID="lblRecords" runat="server" Visible="false"></asp:Label></small>
                                    &nbsp;
                                    <a id="aFilter" class="btn btn-default btn-sm" href="" style="text-decoration: none;" onclick="javascript:return ShowHideFilter('divFilter', false);"><%= Resources.Resource.Filter %>&nbsp;&nbsp;<span class="caret"></span></a>
                                </h4>
                            </div>
                            <div style="padding: 0 15px;">
                                <asp:Button ID="btnDownload" runat="server" Text="<% $Resources:Resource, Download %>" CssClass="btn btn-info" OnClick="btnDownload_Click" />
                                &nbsp;
                                <asp:Button ID="btnNew" runat="server" Text="<% $Resources:Resource, btnNew %>" CssClass="btn btn-primary" data-toggle="modal" data-target="#divcustomerdetails" OnClientClick="javascript:return false;" />
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
                                                    <label class="col-sm-5 control-label"><%= Resources.Resource.CustomerIdNo %></label>
                                                    <div class="col-sm-7">
                                                        <asp:TextBox ID="txtFilterCustomer" runat="server" CssClass="form-control input-sm"></asp:TextBox>
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
                                            <div class="col-md-4 col-sm-6">
                                                <div class="form-group">
                                                    <label class="col-sm-5 control-label"><%= Resources.Resource.Verification %></label>
                                                    <div class="col-sm-7">
                                                        <asp:DropDownList ID="ddlFilterVerification" runat="server" CssClass="form-control input-sm">
                                                            <asp:ListItem Text="- SELECT -" Value=""></asp:ListItem>
                                                            <asp:ListItem Text="<% $Resources:Resource, Yes %>" Value="Yes"></asp:ListItem>
                                                            <asp:ListItem Text="<% $Resources:Resource, No %>" Value="No"></asp:ListItem>
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-md-4 col-sm-6">
                                                <div class="form-group">
                                                    <label class="col-sm-5 control-label"><%= Resources.Resource.Hasurl %></label>
                                                    <div class="col-sm-7">
                                                        <asp:DropDownList ID="ddlFilterHasurl" runat="server" CssClass="form-control input-sm">
                                                            <asp:ListItem Text="- SELECT -" Value=""></asp:ListItem>
                                                            <asp:ListItem Text="<% $Resources:Resource, Yes %>" Value="Yes"></asp:ListItem>
                                                            <asp:ListItem Text="<% $Resources:Resource, No %>" Value="No"></asp:ListItem>
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-md-4 col-sm-6">
                                                <div class="form-group">
                                                    <label class="col-sm-5 control-label"><%= Resources.Resource.InvalidMobile %></label>
                                                    <div class="col-sm-7">
                                                        <asp:DropDownList ID="ddlFilterInvalidMobile" runat="server" CssClass="form-control input-sm">
                                                            <asp:ListItem Text="- SELECT -" Value=""></asp:ListItem>
                                                            <asp:ListItem Text="<% $Resources:Resource, Yes %>" Value="Yes"></asp:ListItem>
                                                            <asp:ListItem Text="<% $Resources:Resource, No %>" Value="No"></asp:ListItem>
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-md-4 col-sm-6">
                                                <div class="form-group">
                                                    <label class="col-sm-5 control-label"><%= Resources.Resource.InvalidIdNo %></label>
                                                    <div class="col-sm-7">
                                                        <asp:DropDownList ID="ddlFilterInvalidIdNo" runat="server" CssClass="form-control input-sm">
                                                            <asp:ListItem Text="- SELECT -" Value=""></asp:ListItem>
                                                            <asp:ListItem Text="<% $Resources:Resource, Yes %>" Value="Yes"></asp:ListItem>
                                                            <asp:ListItem Text="<% $Resources:Resource, No %>" Value="No"></asp:ListItem>
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-md-4 col-sm-6">
                                                <div class="form-group">
                                                    <label class="col-sm-5 control-label"><%= Resources.Resource.ContractStatus %></label>
                                                    <div class="col-sm-7">
                                                        <asp:DropDownList ID="ddlFilterContractStatus" runat="server" CssClass="form-control input-sm"></asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-md-12 text-center">
                                                <asp:Button ID="btnSearch" runat="server" Text="<% $Resources:Resource, btnSearch %>" CssClass="btn btn-primary" ValidationGroup="FilterCustomerMaster" OnClick="btnSearch_Click" />
                                                &nbsp;
                                                <asp:Button ID="btnReset" runat="server" Text="<% $Resources:Resource, btnReset %>" CssClass="btn btn-default" CausesValidation="false" OnClick="btnReset_Click" />
                                            </div>
                                        </div>
                                    </div>
                                </asp:Panel>
                                <asp:ValidationSummary ID="vsCustomerMaster" runat="server" DisplayMode="BulletList"
                                    ShowMessageBox="true" ShowSummary="false" ValidationGroup="FilterCustomerMaster" />
                            </div>
                        </div>
                    </div>

                    <asp:ListView ID="lvCustomerMaster" runat="server" DataKeyNames="CustomerMasterId" OnItemCommand="lvCustomerMaster_ItemCommand" OnItemDataBound="lvCustomerMaster_ItemDataBound">
                        <LayoutTemplate>
                            <div class="panel panel-default">
                                <div class="table-responsive">
                                    <table id="itemPlaceholderContainer" runat="server" class="table table-hover">
                                        <tr>
                                            <%--<th>
                                                <%= Resources.Resource.PhotoIdImageName %>
                                            </th>--%>
                                            <th>
                                                <%= Resources.Resource.Customer %>
                                            </th>
                                            <th>
                                                <%= Resources.Resource.Mobile %>
                                            </th>
                                            <th>
                                                <%= Resources.Resource.Address %>
                                            </th>
                                            <th>
                                                <%= Resources.Resource.Contact %>
                                            </th>
                                            <th style="width: 100px">
                                                <%= Resources.Resource.Links %>
                                            </th>
                                            <th style="width: 25px;">
                                                <%= Resources.Resource.VerifiedBy %>
                                            </th>
                                            <th style="width: 25px;">
                                                <%= Resources.Resource.ModifiedBy %>
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
                                <%--<td>
                                    <asp:Image ID="imgPhotoIdImageName" runat="server" CssClass="thumb img_resize_fit"></asp:Image>
                                </td>--%>
                                <td>
                                    <span class="text-muted"><%= Resources.Resource.Name %>:</span>
                                    <asp:Literal ID="ltrlCustomerName" runat="server"></asp:Literal>
                                    <asp:HyperLink ID="hlnkCustomerName" NavigateUrl="#" Target="_blank" runat="server" Visible="false">
                                    </asp:HyperLink>
                                    <br />
                                    <span class="text-muted"><%= Resources.Resource.IdNo %>:</span>
                                    <asp:Literal ID="ltrlIdNo" runat="server"></asp:Literal>
                                    <br />
                                    <span class="text-muted"><%= Resources.Resource.Gender %>:</span>
                                    <asp:Literal ID="ltrlGender" runat="server"></asp:Literal>
                                    <br />
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
                                    <span class="text-muted"><%= Resources.Resource.Occupation %>:</span>
                                    <asp:Literal ID="ltrlOccupation" runat="server"></asp:Literal>
                                    <br />
                                    <span class="text-muted"><%= Resources.Resource.PlaceOfWork %>:</span>
                                    <asp:Literal ID="ltrlPlaceOfWork" runat="server"></asp:Literal>
                                    <br />
                                    <span class="text-muted"><%= Resources.Resource.CityOfResidence %>:</span>
                                    <asp:Literal ID="ltrlCityOfResidence" runat="server"></asp:Literal>
                                    <br />
                                    <span class="text-muted"><%= Resources.Resource.District %>:</span>
                                    <asp:Literal ID="ltrlDistrict" runat="server"></asp:Literal>
                                    <br />
                                    <span class="text-muted"><%= Resources.Resource.Address1 %>:</span>
                                    <asp:Literal ID="ltrlAddress1" runat="server"></asp:Literal>
                                    <br />
                                </td>
                                <td>
                                    <%--<span class="text-muted"><%= Resources.Resource.Address2 %>:</span>
                                    <asp:Literal ID="ltrlAddress2" runat="server"></asp:Literal>
                                    <br />--%>
                                    <span class="text-muted"><%= Resources.Resource.Phone1 %>:</span>
                                    <asp:Literal ID="ltrlPhone1" runat="server"></asp:Literal>
                                    <br />
                                    <span class="text-muted"><%= Resources.Resource.Relation1 %>:</span>
                                    <asp:Literal ID="ltrlRelation1" runat="server"></asp:Literal>
                                    <br />
                                    <span class="text-muted"><%= Resources.Resource.ContactPersonName1 %>:</span>
                                    <asp:Literal ID="ltrlContactName1" runat="server"></asp:Literal>
                                    <br />
                                    <span class="text-muted"><%= Resources.Resource.Phone2 %>:</span>
                                    <asp:Literal ID="ltrlPhone2" runat="server"></asp:Literal>
                                    <br />
                                    <span class="text-muted"><%= Resources.Resource.Relation2 %>:</span>
                                    <asp:Literal ID="ltrlRelation2" runat="server"></asp:Literal>
                                    <br />
                                    <span class="text-muted"><%= Resources.Resource.ContactPersonName2 %>:</span>
                                    <asp:Literal ID="ltrlContactName2" runat="server"></asp:Literal>
                                </td>
                                <td style="padding-top: 15px;">
                                    <asp:HyperLink ID="lnkLinks" NavigateUrl="#" Target="_blank" runat="server" CssClass="fa fa-link">
                                    </asp:HyperLink>
                                </td>
                                <td>
                                    <asp:LinkButton ID="lbtnVerify" runat="server" CommandName="VerifyRecord" CssClass="btn btn-primary btn-sm fa fa-check" title="Verify"></asp:LinkButton>
                                    <asp:Literal ID="ltrlVerifiedBy" runat="server"></asp:Literal>
                                    <asp:Literal ID="ltrlVerifiedDateTime" runat="server"></asp:Literal>
                                </td>
                                <td>
                                    <asp:Literal ID="ltrlModifiedBy" runat="server"></asp:Literal>
                                    <asp:Literal ID="ltrlModifiedDateTime" runat="server"></asp:Literal>
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
                            <cc1:DataPager ID="pgrCustomerMaster" runat="server" OnPagerCommand="pgrCustomerMaster_ItemCommand"
                                Visible="false" CssClass="pagination" CurrentPageCssClass="active"></cc1:DataPager>
                        </div>
                    </div>

                </ContentTemplate>
                <Triggers>
                    <asp:PostBackTrigger ControlID="btnDownload" />
                    <asp:PostBackTrigger ControlID="btnSaveAndNew" />
                    <asp:PostBackTrigger ControlID="btnSave" />
                </Triggers>
            </asp:UpdatePanel>
        </div>

        <div class="modal fade" id="divcustomerdetails">
            <div class="modal-dialog wide-modal-dialog">
                <div class="modal-content">
                    <div class="blockui">
                        <asp:UpdatePanel ID="upcustomerdetails" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Panel ID="pnlcustomerdetails" runat="server" DefaultButton="btnSaveAndNew">
                                    <asp:HiddenField ID="hdnModelCustomer" runat="server"></asp:HiddenField>
                                    <asp:HiddenField ID="hdnActionCustomer" runat="server"></asp:HiddenField>
                                    <div class="modal-header">
                                        <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                                        <h4 class="modal-title"><%= Resources.Resource.CustomerFormTitle %></h4>
                                    </div>
                                    <div class="modal-body">
                                        <div class="form-horizontal" role="form">
                                            <div class="row">
                                                <asp:HiddenField ID="hdnCustomerMasterId" runat="server" Visible="false"></asp:HiddenField>
                                                <div class="col-lg-6">
                                                    <div class="form-group">
                                                        <label class="col-sm-4 control-label"><%= Resources.Resource.CustomerName %></label>
                                                        <div class="col-sm-7">
                                                            <asp:TextBox ID="txtCustomerName" runat="server" CssClass="form-control input-sm" MaxLength="50"></asp:TextBox>
                                                            <div class="text-danger">
                                                                <asp:RequiredFieldValidator ID="rfvCustomerName" runat="server" SetFocusOnError="true" ControlToValidate="txtCustomerName" Display="Dynamic"
                                                                    ValidationGroup="customerdetails"><%= Resources.Messages.InputRequired %> <%=Resources.Resource.CustomerName %></asp:RequiredFieldValidator>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="form-group">
                                                        <label class="col-sm-4 control-label"><%= Resources.Resource.Links %></label>
                                                        <div class="col-sm-7">
                                                            <asp:TextBox ID="txtLinks" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="col-lg-6">
                                                    <div class="form-group">
                                                        <label class="col-sm-4 control-label"><%= Resources.Resource.IdNo %></label>
                                                        <div class="col-sm-7">
                                                            <asp:TextBox ID="txtIdNo" runat="server" CssClass="form-control input-sm" MaxLength="10"></asp:TextBox>
                                                            <div class="text-danger">
                                                                <asp:RequiredFieldValidator ID="rfvIdNo" runat="server" SetFocusOnError="true" ControlToValidate="txtIdNo" Display="Dynamic"
                                                                    ValidationGroup="customerdetails"><%= Resources.Messages.InputRequired %> <%=Resources.Resource.IdNo %></asp:RequiredFieldValidator>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="form-group">
                                                        <label class="col-sm-4 control-label"><%= Resources.Resource.Gender  %></label>
                                                        <div class="col-sm-7">
                                                            <asp:RadioButtonList ID="rbGender" runat="server">
                                                                <asp:ListItem Text="&nbsp;<% $Resources:Resource, Male %>" Value="Male"></asp:ListItem>
                                                                <asp:ListItem Text="&nbsp;<% $Resources:Resource, Female %>" Value="Female"></asp:ListItem>
                                                            </asp:RadioButtonList>
                                                            <div class="text-danger">
                                                                <asp:RequiredFieldValidator ID="rfvGender" runat="server" Display="Dynamic"
                                                                    ControlToValidate="rbGender" SetFocusOnError="true" ValidationGroup="customerdetails">
                                                                        <%= Resources.Messages.InputSelect %> <%=Resources.Resource.Gender %>
                                                                </asp:RequiredFieldValidator>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="col-lg-12">
                                                    <hr />
                                                </div>
                                                <div class="col-lg-6">
                                                    <div class="form-group">
                                                        <label class="col-sm-4 control-label"><%= Resources.Resource.Occupation  %></label>
                                                        <div class="col-sm-7">
                                                            <asp:TextBox ID="txtOccupation" runat="server" CssClass="form-control input-sm" MaxLength="100"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                    <div class="form-group">
                                                        <label class="col-sm-4 control-label"><%= Resources.Resource.CityOfResidence  %></label>
                                                        <div class="col-sm-7">
                                                            <asp:TextBox ID="txtCityOfResidence" runat="server" CssClass="form-control input-sm" MaxLength="100"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                    <div class="form-group">
                                                        <label class="col-sm-4 control-label"><%= Resources.Resource.Mobile1 %></label>
                                                        <div class="col-sm-7">
                                                            <asp:TextBox ID="txtMobile1" runat="server" CssClass="form-control input-sm" MaxLength="10"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                    <div class="form-group">
                                                        <label class="col-sm-4 control-label"><%= Resources.Resource.Mobile3 %></label>
                                                        <div class="col-sm-7">
                                                            <asp:TextBox ID="txtMobile3" runat="server" CssClass="form-control input-sm" MaxLength="10"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="col-lg-6">
                                                    <div class="form-group">
                                                        <label class="col-sm-4 control-label"><%= Resources.Resource.PlaceOfWork  %></label>
                                                        <div class="col-sm-7">
                                                            <asp:TextBox ID="txtPlaceOfWork" runat="server" CssClass="form-control input-sm" MaxLength="100"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                    <div class="form-group">
                                                        <label class="col-sm-4 control-label"><%= Resources.Resource.District  %></label>
                                                        <div class="col-sm-7">
                                                            <asp:TextBox ID="txtDistrict" runat="server" CssClass="form-control input-sm" MaxLength="100"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                    <div class="form-group">
                                                        <label class="col-sm-4 control-label"><%= Resources.Resource.Mobile2 %></label>
                                                        <div class="col-sm-7">
                                                            <asp:TextBox ID="txtMobile2" runat="server" CssClass="form-control input-sm" MaxLength="10"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="col-lg-12">
                                                    <hr />
                                                </div>
                                                <div class="col-lg-6">
                                                    <div class="form-group">
                                                        <label class="col-sm-4 control-label"><%= Resources.Resource.Address %></label>
                                                        <div class="col-sm-7">
                                                            <asp:TextBox ID="txtAddress1" TextMode="MultiLine" Rows="5" runat="server" CssClass="form-control input-sm" MaxLength="100"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                    <div class="form-group">
                                                        <label class="col-sm-4 control-label"><%= Resources.Resource.BankAccountNumber %></label>
                                                        <div class="col-sm-7">
                                                            <asp:TextBox ID="txtBankAccountNumber" runat="server" CssClass="form-control input-sm" MaxLength="50"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                    <div class="form-group">
                                                        <label class="col-sm-4 control-label"><%= Resources.Resource.BankAccountNumber2 %></label>
                                                        <div class="col-sm-7">
                                                            <asp:TextBox ID="txtBankAccountNumber2" runat="server" CssClass="form-control input-sm" MaxLength="50"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                    <div class="form-group">
                                                        <label class="col-sm-4 control-label"><%= Resources.Resource.BankAccountNumber3 %></label>
                                                        <div class="col-sm-7">
                                                            <asp:TextBox ID="txtBankAccountNumber3" runat="server" CssClass="form-control input-sm" MaxLength="50"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                    <div class="form-group">
                                                        <label class="col-sm-4 control-label"><%= Resources.Resource.BankAccountNumber4 %></label>
                                                        <div class="col-sm-7">
                                                            <asp:TextBox ID="txtBankAccountNumber4" runat="server" CssClass="form-control input-sm" MaxLength="50"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                    <div class="form-group">
                                                        <div class="col-sm-offset-2 col-sm-7">
                                                            <div class="checkbox">
                                                                <label>
                                                                    <asp:CheckBox ID="chkIsIsRedFlag" runat="server" Text="<% $Resources:Resource, IsRedFlag %>"></asp:CheckBox>
                                                                </label>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="col-lg-6">
                                                    <div class="form-group">
                                                        <label class="col-sm-4 control-label"><%= Resources.Resource.Contact1 %></label>
                                                        <div class="col-sm-7">
                                                            <asp:TextBox ID="txtPhone1" runat="server" CssClass="form-control input-sm" MaxLength="15"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                    <div class="form-group">
                                                        <label class="col-sm-4 control-label"><%= Resources.Resource.Relation1 %></label>
                                                        <div class="col-sm-7">
                                                            <asp:DropDownList ID="ddlRelation1" runat="server" CssClass="form-control input-sm">
                                                                <asp:ListItem Text="- SELECT -" Value=""></asp:ListItem>
                                                                <asp:ListItem Text="<% $Resources:Resource, Father %>" Value="Father"></asp:ListItem>
                                                                <asp:ListItem Text="<% $Resources:Resource, Mother %>" Value="Mother"></asp:ListItem>
                                                                <asp:ListItem Text="<% $Resources:Resource, Brother %>" Value="Brother"></asp:ListItem>
                                                                <asp:ListItem Text="<% $Resources:Resource, Sister %>" Value="Sister"></asp:ListItem>
                                                                <asp:ListItem Text="<% $Resources:Resource, Other %>" Value="Other"></asp:ListItem>
                                                            </asp:DropDownList>
                                                        </div>
                                                    </div>
                                                    <div class="form-group">
                                                        <label class="col-sm-4 control-label"><%= Resources.Resource.PersonName1 %></label>
                                                        <div class="col-sm-7">
                                                            <asp:TextBox ID="txtContactName1" runat="server" CssClass="form-control input-sm" MaxLength="50"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                    <div class="form-group">
                                                        <label class="col-sm-4 control-label"><%= Resources.Resource.Contact2 %></label>
                                                        <div class="col-sm-7">
                                                            <asp:TextBox ID="txtPhone2" runat="server" CssClass="form-control input-sm" MaxLength="15"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                    <div class="form-group">
                                                        <label class="col-sm-4 control-label"><%= Resources.Resource.Relation2 %></label>
                                                        <div class="col-sm-7">
                                                            <asp:DropDownList ID="ddlRelation2" runat="server" CssClass="form-control input-sm">
                                                                <asp:ListItem Text="- SELECT -" Value=""></asp:ListItem>
                                                                <asp:ListItem Text="<% $Resources:Resource, Father %>" Value="Father"></asp:ListItem>
                                                                <asp:ListItem Text="<% $Resources:Resource, Mother %>" Value="Mother"></asp:ListItem>
                                                                <asp:ListItem Text="<% $Resources:Resource, Brother %>" Value="Brother"></asp:ListItem>
                                                                <asp:ListItem Text="<% $Resources:Resource, Sister %>" Value="Sister"></asp:ListItem>
                                                                <asp:ListItem Text="<% $Resources:Resource, Other %>" Value="Other"></asp:ListItem>
                                                            </asp:DropDownList>
                                                        </div>
                                                    </div>
                                                    <div class="form-group">
                                                        <label class="col-sm-4 control-label"><%= Resources.Resource.PersonName2 %></label>
                                                        <div class="col-sm-7">
                                                            <asp:TextBox ID="txtContactName2" runat="server" CssClass="form-control input-sm" MaxLength="50"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="col-lg-12">
                                                    <hr />
                                                </div>
                                                <div class="col-lg-6">
                                                    <%--<div class="form-group">
                                                        <label class="col-sm-4 control-label"><%= Resources.Resource.PhotoIdImageName %></label>
                                                        <div class="col-sm-7">
                                                            <asp:FileUpload ID="fuPhotoIdImageName" runat="server" />
                                                            <asp:HiddenField ID="hdnPhotoIdImageName" runat="server" />
                                                            <asp:HiddenField ID="hdnPhotoIdImageNameURL" runat="server" />

                                                        </div>
                                                    </div>--%>
                                                    <%--<div class="form-group">
                                                        <label class="col-sm-4 control-label"><%= Resources.Resource.Address2 %></label>
                                                        <div class="col-sm-7">
                                                            <asp:TextBox ID="txtAddress2" TextMode="MultiLine" Rows="5" runat="server" CssClass="form-control input-sm" MaxLength="100"></asp:TextBox>
                                                        </div>
                                                    </div>--%>
                                                </div>
                                                <div class="col-lg-6">
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="modal-footer">
                                        <asp:Button ID="btnSaveAndNew" runat="server" Text="<% $Resources:Resource, btnSaveAndNew %>" CssClass="btn btn-primary" OnClick="btnSave_Click" ValidationGroup="customerdetails" />

                                        <asp:Button ID="btnSave" runat="server" Text="<% $Resources:Resource, btnSave %>" CssClass="btn btn-primary" OnClick="btnSave_Click" ValidationGroup="customerdetails" />

                                        <asp:Button ID="btnClose" runat="server" Text="<% $Resources:Resource, btnClose %>" CssClass="btn btn-default" CausesValidation="false" data-dismiss="modal" />

                                    </div>
                                </asp:Panel>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="lvCustomerMaster" EventName="ItemCommand" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </div>
        </div>
        <asp:ValidationSummary ID="vscustomerdetails" runat="server" DisplayMode="BulletList"
            ShowMessageBox="true" ShowSummary="false" ValidationGroup="customerdetails" />

    </div>
    <%--<script type="text/javascript">
        $(document).ready(function () {
            SetControlsPicker();
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(SetControlsPicker);
        });

        function SetControlsPicker() {
            SetImageUpload(Prefix + "fuPhotoIdImageName");
        }
    </script>--%>
    <script type="text/javascript">
        $(document).ready(function () {
            if ($("#" + Prefix + "hdnModelCustomer").val() == "") {
                ClearCustomerMaster();
            }
            SetDialogShowHideCustomerMaster();
            ShowHideDialogCustomerMaster();
        });

        function SetDialogShowHideCustomerMaster() {
            $("#divcustomerdetails").on("hidden.bs.modal", function () {
                ClearCustomerMaster();
            })
            $("#divcustomerdetails").on("show.bs.modal", function () {
                SetCustomerMaster();
            })
            $("#divcustomerdetails").on("shown.bs.modal", function () {
                $("#" + Prefix + "txtCustomerName").focus();

                //if ($("#" + Prefix + "hdnPhotoIdImageName").val() != "") {
                //    $("." + Prefix + "fuPhotoIdImageName").attr("src", $("#" + Prefix + "hdnPhotoIdImageNameURL").val());
                //}
            })
        }
        function ShowHideDialogCustomerMaster() {
            try {
                SetCustomerMaster();
                if ($("#" + Prefix + "hdnModelCustomer").val() == "show") {
                    $("#divcustomerdetails").modal("show");
                }
                else if ($("#" + Prefix + "hdnModelCustomer").val() == "hide") {
                    ClearCustomerMaster();
                    $("#divcustomerdetails").modal("hide");
                }
                else if ($("#" + Prefix + "hdnModelCustomer").val() == "clear") {
                    ClearCustomerMaster();
                    $("#divcustomerdetails").modal("show");
                    $("#" + Prefix + "txtCustomerName").focus();
                }
                $("#" + Prefix + "hdnModelCustomer").val("");
                return false;
            }
            catch (ex) {
                alert(ex);
                return false;
            }
        }
        function ClearCustomerMaster() {
            for (i = 0; i < Page_Validators.length; i++) {
                Page_Validators[i].style.display = "none";
            }
            $("#" + Prefix + "hdnActionCustomer").val("");
            $("#" + Prefix + "txtCustomerName").val("");
            $("#" + Prefix + "txtIdNo").val("");
            $("#" + Prefix + "txtLinks").val("");
            clearRadioButtonList();

            //if ($("#" + Prefix + "fuPhotoIdImageName").val() != "") {
            //    $("#" + Prefix + "fuPhotoIdImageName").fileinput("clear");
            //}
            //$("#" + Prefix + "hdnPhotoIdImageName").val("");
            //$("." + Prefix + "fuPhotoIdImageName").attr("src", "img/xs_NoImage.png");

            $("#" + Prefix + "txtOccupation").val("");
            $("#" + Prefix + "txtPlaceOfWork").val("");
            $("#" + Prefix + "txtCityOfResidence").val("");
            $("#" + Prefix + "txtDistrict").val("");
            $("#" + Prefix + "txtMobile1").val("");
            $("#" + Prefix + "txtMobile2").val("");
            $("#" + Prefix + "txtMobile3").val("");

            $("#" + Prefix + "txtAddress1").val("");
            $("#" + Prefix + "txtPhone1").val("");
            $("#" + Prefix + "ddlRelation1").prop("selectedIndex", 0);
            $("#" + Prefix + "txtContactName1").val("");
            /*$("#" + Prefix + "txtAddress2").val("");*/
            $("#" + Prefix + "txtPhone2").val("");
            $("#" + Prefix + "ddlRelation2").prop("selectedIndex", 0);
            $("#" + Prefix + "txtContactName2").val("");
            $("#" + Prefix + "txtBankAccountNumber").val("");
            $("#" + Prefix + "txtBankAccountNumber2").val("");
            $("#" + Prefix + "txtBankAccountNumber3").val("");
            $("#" + Prefix + "txtBankAccountNumber4").val("");

            $("#" + Prefix + "chkIsRedFlag").prop("checked", false);

        }
        function SetCustomerMaster() {
            for (i = 0; i < Page_Validators.length; i++) {
                Page_Validators[i].errormessage = Page_Validators[i].innerHTML;
            }
            if ($("#" + Prefix + "hdnActionCustomer").val() == "") {
                $("#" + Prefix + "btnSaveAndNew").css("visibility", "visible");
                $("#" + Prefix + "btnSave").val('<%= GetGlobalResourceObject("Resource", "btnSave") %>');
            }
            else {
                $("#" + Prefix + "btnSaveAndNew").css("visibility", "hidden");
                $("#" + Prefix + "btnSave").val('<%= GetGlobalResourceObject("Resource", "btnUpdate") %>');
            }
        }
        function clearRadioButtonList() {

            var elementRef = document.getElementById('<%= rbGender.ClientID %>');
            var inputElementArray = elementRef.getElementsByTagName('input');

            for (var i = 0; i < inputElementArray.length; i++) {
                var inputElement = inputElementArray[i];

                inputElement.checked = false;
            }
            return false;
        }
    </script>

</asp:Content>
