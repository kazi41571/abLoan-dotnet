<%@ Page Language="C#" MasterPageFile="Site.Master" AutoEventWireup="true" CodeBehind="contractdetails.aspx.cs" Inherits="abLOAN.contractdetails" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script type="text/javascript">
        $(document).ready(function () {
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequest);
            SetFilter();
        });
        function EndRequest(sender, args) {

            ShowHideDialogGuarantorMaster();
            ShowHideDialogWitnessMaster();
            SetFilter();

            var collapseid = $("#hdnCollapseHide").val();
            if (collapseid != "") {
                $("#" + collapseid).collapse('hide');
            }

            collapseid = $("#hdnCollapseShow").val();
            $("#" + collapseid).collapse('show');
        }
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphPage" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:HiddenField ID="hdnCollapseHide" runat="server" ClientIDMode="Static" Value="" />
            <asp:HiddenField ID="hdnCollapseShow" runat="server" ClientIDMode="Static" Value="collapseOne" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <div class="panel-group" id="accordion" role="tablist" aria-multiselectable="true">
        <div class="panel panel-default">
            <div class="panel-heading" role="tab" id="headingOne">
                <h4 class="panel-title">
                    <div style="justify-content: space-between; display: flex;">
                        <h4 style="padding: 0 5px;">
                            <a role="button" data-toggle="collapse" data-parent="#accordion" href="#collapseOne" aria-expanded="true" aria-controls="collapseOne"><%= Resources.Resource.CustomerFormTitle %>
                            </a>
                        </h4>
                        <div style="padding: 0 5px;">
                            <asp:Button ID="btnBackTop" runat="server" CausesValidation="false" Text="<% $Resources:Resource, btnBack %>" CssClass="btn btn-default" PostBackUrl="contract.aspx" />
                        </div>
                    </div>
                </h4>
            </div>
            <div id="collapseOne" class="panel-collapse collapse in" role="tabpanel" aria-labelledby="headingOne">
                <div class="panel-body">
                    <div class="blockui">
                        <asp:UpdatePanel ID="upcustomerdetails" runat="server">
                            <ContentTemplate>
                                <asp:Panel ID="pnlcustomerdetails" runat="server" DefaultButton="btnCustomerSave">
                                    <div class="form-horizontal" role="form">
                                        <asp:HiddenField ID="hdnCustomerMasterId" runat="server" Visible="false"></asp:HiddenField>
                                        <div class="form-group">
                                            <label class="col-sm-4 control-label"><%= Resources.Resource.CustomerIdNo %></label>
                                            <div class="col-sm-2">
                                                <asp:TextBox ID="txtSearchCustomerIdNo" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                                <div class="text-danger">
                                                    <asp:RequiredFieldValidator ID="rfvSearchCustomerIdNo" runat="server" ControlToValidate="txtSearchCustomerIdNo" Display="Dynamic" SetFocusOnError="true" ValidationGroup="searchcustomerdetails"><%= Resources.Messages.InputRequired %> <%=Resources.Resource.CustomerIdNo %></asp:RequiredFieldValidator>
                                                </div>
                                            </div>
                                            <div class="col-sm-1">
                                                <asp:Button ID="btnSearchCustomerIdNo" runat="server" CssClass="btn btn-default" Text="<% $Resources:Resource, btnSearch %>" ValidationGroup="searchcustomerdetails" OnClick="btnSearchCustomerIdNo_Click"></asp:Button>
                                                <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="BulletList"
                                                    ShowMessageBox="true" ShowSummary="false" ValidationGroup="searchcustomerdetails" />
                                            </div>
                                        </div>
                                        <asp:Panel ID="pnlCustomer" runat="server" Enabled="false">
                                            <div class="row">
                                                <div class="col-md-6">
                                                    <div class="form-group">
                                                        <label class="col-sm-3 control-label"><%= Resources.Resource.IdNo %></label>
                                                        <div class="col-sm-7">
                                                            <asp:TextBox ID="txtIdNo" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                                            <div class="text-danger">
                                                                <asp:RequiredFieldValidator ID="rfvIdNo" runat="server" ControlToValidate="txtIdNo" Display="Dynamic" SetFocusOnError="true"
                                                                    ValidationGroup="customerdetails"><%= Resources.Messages.InputRequired %> <%=Resources.Resource.IdNo %></asp:RequiredFieldValidator>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="form-group">
                                                        <label class="col-sm-3 control-label"><%= Resources.Resource.CustomerName %></label>
                                                        <div class="col-sm-7">
                                                            <asp:TextBox ID="txtCustomerName" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                                            <div class="text-danger">
                                                                <asp:RequiredFieldValidator ID="rfvCustomerName" runat="server" ControlToValidate="txtCustomerName" Display="Dynamic" SetFocusOnError="true"
                                                                    ValidationGroup="customerdetails"><%= Resources.Messages.InputRequired %> <%=Resources.Resource.CustomerName %></asp:RequiredFieldValidator>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <%--<div class="form-group">
                                                        <label class="col-sm-3 control-label"><%= Resources.Resource.PhotoIdImageName %></label>
                                                        <div class="col-sm-7">
                                                            <asp:FileUpload ID="fuPhotoIdImageName" runat="server" />
                                                            <asp:HiddenField ID="hdnPhotoIdImageName" runat="server" />
                                                            <asp:HiddenField ID="hdnPhotoIdImageNameURL" runat="server" />
                                                        </div>
                                                    </div>--%>
                                                </div>
                                                <div class="col-lg-6">
                                                    <div class="form-group">
                                                        <label class="col-sm-3 control-label"><%= Resources.Resource.Mobile1 %></label>
                                                        <div class="col-sm-7">
                                                            <asp:TextBox ID="txtMobile1" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                    <div class="form-group">
                                                        <label class="col-sm-3 control-label"><%= Resources.Resource.Mobile2 %></label>
                                                        <div class="col-sm-7">
                                                            <asp:TextBox ID="txtMobile2" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                    <div class="form-group">
                                                        <label class="col-sm-3 control-label"><%= Resources.Resource.Mobile3 %></label>
                                                        <div class="col-sm-7">
                                                            <asp:TextBox ID="txtMobile3" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="col-lg-12">
                                                    <hr />
                                                </div>
                                                <div class="col-md-6">
                                                    <div class="form-group">
                                                        <label class="col-sm-3 control-label"><%= Resources.Resource.Address1 %></label>
                                                        <div class="col-sm-7">
                                                            <asp:TextBox ID="txtAddress1" runat="server" TextMode="MultiLine" CssClass="form-control input-sm"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                    <div class="form-group">
                                                        <label class="col-sm-3 control-label"><%= Resources.Resource.Phone1 %></label>
                                                        <div class="col-sm-7">
                                                            <asp:TextBox ID="txtPhone1" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                    <div class="form-group">
                                                        <label class="col-sm-3 control-label"><%= Resources.Resource.ContactPersonName1 %></label>
                                                        <div class="col-sm-7">
                                                            <asp:TextBox ID="txtContactName1" runat="server" CssClass="form-control input-sm" MaxLength="50"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="col-lg-6">
                                                    <div class="form-group">
                                                        <label class="col-sm-3 control-label"><%= Resources.Resource.Address2 %></label>
                                                        <div class="col-sm-7">
                                                            <asp:TextBox ID="txtAddress2" runat="server" TextMode="MultiLine" CssClass="form-control input-sm"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                    <div class="form-group">
                                                        <label class="col-sm-3 control-label"><%= Resources.Resource.Phone2 %></label>
                                                        <div class="col-sm-7">
                                                            <asp:TextBox ID="txtPhone2" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                                        </div>
                                                    </div>

                                                    <div class="form-group">
                                                        <label class="col-sm-3 control-label"><%= Resources.Resource.ContactPersonName2 %></label>
                                                        <div class="col-sm-7">
                                                            <asp:TextBox ID="txtContactName2" runat="server" CssClass="form-control input-sm" MaxLength="50"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </asp:Panel>
                                        <div class="modal-footer">
                                            <asp:Button ID="btnCustomerSave" runat="server" Text="<% $Resources:Resource, btnSaveAndContinue %>" CssClass="btn btn-primary" OnClick="btnCustomerSave_Click" ValidationGroup="customerdetails" />
                                        </div>
                                    </div>
                                </asp:Panel>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                    <asp:ValidationSummary ID="vscustomerdetails" runat="server" DisplayMode="BulletList"
                        ShowMessageBox="true" ShowSummary="false" ValidationGroup="customerdetails" />

                </div>
            </div>
        </div>
        <div class="panel panel-default">
            <div class="panel-heading" role="tab" id="headingTwo">
                <h4 class="panel-title">
                    <h4 style="padding: 0 5px;">
                        <a class="collapsed" role="button" data-toggle="collapse" data-parent="#accordion" href="#collapseTwo" aria-expanded="false" aria-controls="collapseTwo"><%= Resources.Resource.ContractFormTitle %>
                        </a>
                    </h4>
                </h4>
            </div>
            <div id="collapseTwo" class="panel-collapse collapse" role="tabpanel" aria-labelledby="headingTwo">
                <div class="panel-body">
                    <div class="blockui">
                        <asp:UpdatePanel ID="upcontractdetails" runat="server">
                            <ContentTemplate>
                                <asp:Panel ID="pnlcontractdetails" runat="server" DefaultButton="btnSave">
                                    <div class="form-horizontal" role="form">
                                        <asp:HiddenField ID="hdnContractMasterId" runat="server" Visible="false"></asp:HiddenField>
                                        <div class="row">
                                            <div class="col-md-6">
                                                <div class="form-group">
                                                    <label class="col-sm-3 control-label"><%= Resources.Resource.ContractTitle %></label>
                                                    <div class="col-sm-7">
                                                        <asp:TextBox ID="txtContractTitle" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                                        <div class="text-danger">
                                                            <asp:RequiredFieldValidator ID="rfvContractTitle" runat="server" ControlToValidate="txtContractTitle" Display="Dynamic" SetFocusOnError="true"
                                                                ValidationGroup="contractdetails"><%= Resources.Messages.InputRequired %> <%=Resources.Resource.ContractTitle %></asp:RequiredFieldValidator>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="form-group">
                                                    <label class="col-sm-3 control-label"><%= Resources.Resource.Bank %></label>
                                                    <div class="col-sm-7">
                                                        <asp:DropDownList ID="ddlBank" runat="server" CssClass="form-control input-sm"></asp:DropDownList>
                                                        <div class="text-danger">
                                                            <asp:RequiredFieldValidator ID="rfvBank" runat="server" ControlToValidate="ddlBank" Display="Dynamic" SetFocusOnError="true"
                                                                ValidationGroup="contractdetails"><%= Resources.Messages.InputSelect %> <%=Resources.Resource.Bank %></asp:RequiredFieldValidator>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="form-group">
                                                    <label class="col-sm-3 control-label"><%= Resources.Resource.FileNo %></label>
                                                    <div class="col-sm-7">
                                                        <asp:TextBox ID="txtFileNo" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                                        <div class="text-danger">
                                                            <asp:RequiredFieldValidator ID="rfvFileNo" runat="server" ControlToValidate="txtFileNo" Display="Dynamic" SetFocusOnError="true"
                                                                ValidationGroup="contractdetails"><%= Resources.Messages.InputRequired %> <%=Resources.Resource.FileNo %></asp:RequiredFieldValidator>
                                                            <asp:CompareValidator ID="cmvFileNo" runat="server"
                                                                ControlToValidate="txtFileNo" Display="Dynamic" Operator="DataTypeCheck" SetFocusOnError="true"
                                                                Type="Integer" ValidationGroup="contractdetails"><%= Resources.Messages.InputRequired %> <%=Resources.Resource.FileNo %></asp:CompareValidator>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="form-group">
                                                    <label class="col-sm-3 control-label"><%= Resources.Resource.Links %></label>
                                                    <div class="col-sm-7">
                                                        <asp:TextBox ID="txtLinks" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="form-group">
                                                    <label class="col-sm-3 control-label"><%= Resources.Resource.ContractDate %></label>
                                                    <div class="col-sm-7">
                                                        <asp:TextBox ID="txtContractDate" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                                        <div class="text-danger">
                                                            <asp:RequiredFieldValidator ID="rfvContractDate" runat="server" ControlToValidate="txtContractDate" Display="Dynamic"
                                                                ValidationGroup="contractdetails"><%= Resources.Messages.InputRequired %> <%=Resources.Resource.ContractDate %></asp:RequiredFieldValidator>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="form-group">
                                                    <label class="col-sm-3 control-label"><%= Resources.Resource.ContractStartDate %></label>
                                                    <div class="col-sm-7">
                                                        <asp:TextBox ID="txtContractStartDate" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                                        <div class="text-danger">
                                                            <asp:RequiredFieldValidator ID="rfvContractStartDate" runat="server" ControlToValidate="txtContractStartDate" Display="Dynamic"
                                                                ValidationGroup="contractdetails"><%= Resources.Messages.InputRequired %> <%=Resources.Resource.ContractStartDate %></asp:RequiredFieldValidator>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="form-group">
                                                    <label class="col-sm-3 control-label"><%= Resources.Resource.InstallmentDate %></label>
                                                    <div class="col-sm-7">
                                                        <asp:TextBox ID="txtInstallmentDate" runat="server" CssClass="form-control input-sm" Enabled="false"></asp:TextBox>
                                                        <div class="text-danger">
                                                            <asp:RequiredFieldValidator ID="rfvInstallmentDate" runat="server" ControlToValidate="txtInstallmentDate" Display="Dynamic"
                                                                ValidationGroup="contractdetails"><%= Resources.Messages.InputRequired %> <%=Resources.Resource.InstallmentDate %></asp:RequiredFieldValidator>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="form-group">
                                                    <label class="col-sm-3 control-label"><%= Resources.Resource.ContractAmount %></label>
                                                    <div class="col-sm-7">
                                                        <asp:TextBox ID="txtContractAmount" runat="server" CssClass="form-control input-sm" onchange="CalculateContact();"></asp:TextBox>
                                                        <div class="text-danger">
                                                            <asp:RequiredFieldValidator ID="rfvContractAmount" runat="server" ControlToValidate="txtContractAmount" Display="Dynamic" SetFocusOnError="true"
                                                                ValidationGroup="contractdetails"><%= Resources.Messages.InputRequired %> <%=Resources.Resource.ContractAmount %></asp:RequiredFieldValidator>
                                                            <asp:CompareValidator ID="cmvContractAmount" runat="server"
                                                                ControlToValidate="txtContractAmount" Display="Dynamic" Operator="DataTypeCheck" SetFocusOnError="true"
                                                                Type="Double" ValidationGroup="contractdetails"><%= Resources.Messages.InputRequired %> <%=Resources.Resource.ContractAmount %></asp:CompareValidator>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="form-group">
                                                    <label class="col-sm-3 control-label"><%= Resources.Resource.Item %></label>
                                                    <div class="col-sm-7">
                                                        <asp:DropDownList ID="ddlItem" runat="server" CssClass="form-control input-sm"></asp:DropDownList>
                                                        <div class="text-danger">
                                                            <asp:RequiredFieldValidator ID="rfvItem" runat="server"
                                                                ControlToValidate="ddlItem" Display="Dynamic" SetFocusOnError="true" InitialValue="" ValidationGroup="contractdetails"><%= Resources.Messages.InputSelect %> <%=Resources.Resource.linktoItemMasterId %></asp:RequiredFieldValidator>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-md-6">
                                                <div class="form-group">
                                                    <div class="col-sm-offset-3 col-sm-7">
                                                        <div class="checkbox">
                                                            <label>
                                                                <asp:CheckBox ID="chkIsBasedOnMonth" runat="server" Text="<% $Resources:Resource, IsBasedOnMonth %>" onclick="CalculateContact()"></asp:CheckBox>
                                                            </label>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="form-group">
                                                    <label class="col-sm-3 control-label"><%= Resources.Resource.NoOfInstallments %></label>
                                                    <div class="col-sm-7">
                                                        <asp:TextBox ID="txtNoOfInstallments" runat="server" CssClass="form-control input-sm" onchange="CalculateContact();"></asp:TextBox>
                                                        <div class="text-danger">
                                                            <asp:RequiredFieldValidator ID="rfvNoOfInstallments" runat="server" ControlToValidate="txtNoOfInstallments" Display="Dynamic" SetFocusOnError="true"
                                                                ValidationGroup="contractdetails"><%= Resources.Messages.InputRequired %> <%=Resources.Resource.NoOfInstallments %></asp:RequiredFieldValidator>
                                                            <asp:CompareValidator ID="cmvNoOfInstallments" runat="server"
                                                                ControlToValidate="txtNoOfInstallments" Display="Dynamic" Operator="DataTypeCheck" SetFocusOnError="true"
                                                                Type="Integer" ValidationGroup="contractdetails"><%= Resources.Messages.InputRequired %> <%=Resources.Resource.NoOfInstallments %></asp:CompareValidator>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="form-group">
                                                    <label class="col-sm-3 control-label"><%= Resources.Resource.InstallmentAmount %></label>
                                                    <div class="col-sm-7">
                                                        <asp:TextBox ID="txtInstallmentAmount" runat="server" CssClass="form-control input-sm" onchange="CalculateContact();"></asp:TextBox>
                                                        <div class="text-danger">
                                                            <asp:RequiredFieldValidator ID="rfvInstallmentAmount" runat="server" ControlToValidate="txtInstallmentAmount" Display="Dynamic" SetFocusOnError="true"
                                                                ValidationGroup="contractdetails"><%= Resources.Messages.InputRequired %> <%=Resources.Resource.InstallmentAmount %></asp:RequiredFieldValidator>
                                                            <asp:CompareValidator ID="cmvInstallmentAmount" runat="server"
                                                                ControlToValidate="txtInstallmentAmount" Display="Dynamic" Operator="DataTypeCheck" SetFocusOnError="true"
                                                                Type="Double" ValidationGroup="contractdetails"><%= Resources.Messages.InputRequired %> <%=Resources.Resource.InstallmentAmount %></asp:CompareValidator>
                                                        </div>
                                                    </div>
                                                </div>
                                                <%--<div class="form-group">
                                                    <label class="col-sm-3 control-label"><%= Resources.Resource.InterestRate %></label>
                                                    <div class="col-sm-7">
                                                        <asp:TextBox ID="txtInterestRate" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                                        <div class="text-danger">
                                                            <asp:RequiredFieldValidator ID="rfvInterestRate" runat="server" ControlToValidate="txtInterestRate" Display="Dynamic" SetFocusOnError="true"
                                                                ValidationGroup="contractdetails"><%= Resources.Messages.InputRequired %> <%=Resources.Resource.InterestRate %></asp:RequiredFieldValidator>
                                                            <asp:CompareValidator ID="cmvInterestRate" runat="server"
                                                                ControlToValidate="txtInterestRate" Display="Dynamic" Operator="DataTypeCheck" SetFocusOnError="true"
                                                                Type="Double" ValidationGroup="contractdetails"><%= Resources.Messages.InputRequired %> <%=Resources.Resource.InterestRate %></asp:CompareValidator>
                                                        </div>
                                                    </div>
                                                </div>--%>
                                                <div class="form-group">
                                                    <label class="col-sm-3 control-label"><%= Resources.Resource.DownPayment %></label>
                                                    <div class="col-sm-7">
                                                        <asp:TextBox ID="txtDownPayment" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                                        <div class="text-danger">
                                                            <asp:RequiredFieldValidator ID="rfvDownPayment" runat="server" ControlToValidate="txtDownPayment" Display="Dynamic" SetFocusOnError="true"
                                                                ValidationGroup="contractdetails"><%= Resources.Messages.InputRequired %> <%=Resources.Resource.DownPayment %></asp:RequiredFieldValidator>
                                                            <asp:CompareValidator ID="cmvDownPayment" runat="server"
                                                                ControlToValidate="txtDownPayment" Display="Dynamic" Operator="DataTypeCheck" SetFocusOnError="true"
                                                                Type="Double" ValidationGroup="contractdetails"><%= Resources.Messages.InputRequired %> <%=Resources.Resource.DownPayment %></asp:CompareValidator>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="form-group">
                                                    <label class="col-sm-3 control-label"><%= Resources.Resource.Notes %></label>
                                                    <div class="col-sm-7">
                                                        <asp:TextBox ID="txtNotes" runat="server" Rows="3" CssClass="form-control input-sm" onKeyDown="javascript:CheckTextAreaMaxLength(this, event, 4000);" TextMode="MultiLine"></asp:TextBox>
                                                    </div>
                                                </div>

                                                <hr />

                                                <div class="form-group">
                                                    <label class="col-sm-3 control-label"><%= Resources.Resource.ContractStatus %></label>
                                                    <div class="col-sm-7">
                                                        <asp:DropDownList ID="ddlContractStatus" runat="server" CssClass="form-control input-sm"></asp:DropDownList>
                                                    </div>
                                                </div>

                                                <div class="form-group">
                                                    <label class="col-sm-3 control-label"><%= Resources.Resource.SettlementAmount %></label>
                                                    <div class="col-sm-7">
                                                        <asp:TextBox ID="txtSettlementAmount" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                                    </div>
                                                </div>

                                            </div>
                                        </div>
                                    </div>
                                    </div>
                                    <div class="modal-footer">
                                        <asp:Button ID="btnSave" runat="server" Text="<% $Resources:Resource, btnSaveAndContinue %>" CssClass="btn btn-primary" OnClick="btnSave_Click" ValidationGroup="contractdetails" />
                                    </div>
                                </asp:Panel>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                    <asp:ValidationSummary ID="vscontractdetails" runat="server" DisplayMode="BulletList"
                        ShowMessageBox="true" ShowSummary="false" ValidationGroup="contractdetails" />
                </div>
            </div>
        </div>
        <div class="panel panel-default">
            <div class="panel-heading" role="tab" id="headingThree">
                <h4 class="panel-title">
                    <div style="justify-content: space-between; display: flex;">
                        <h4 style="padding: 0 5px;">
                            <a class="collapsed" role="button" data-toggle="collapse" data-parent="#accordion" href="#collapseThree" aria-expanded="false" aria-controls="collapseThree"><%= Resources.Resource.GuarantorFormTitle %>
                            </a>
                        </h4>
                        <div style="padding: 0 5px;">
                            <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                <ContentTemplate>
                                    <asp:Button ID="btnNewGuarantor" runat="server" Text="<% $Resources:Resource, btnNew %>" CssClass="btn btn-primary" data-toggle="modal" data-target="#divguarantordetails" OnClientClick="javascript:return false;" />
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </div>
                </h4>
            </div>
            <div id="collapseThree" class="panel-collapse collapse" role="tabpanel" aria-labelledby="headingThree">
                <div class="panel-body">
                    <div class="blockui">
                        <asp:UpdatePanel ID="upGuarantor" runat="server">
                            <ContentTemplate>
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
                                                <asp:Literal ID="ltrlGuarantorName" runat="server"></asp:Literal>
                                                <br />
                                                <asp:Literal ID="ltrlIdNo" runat="server"></asp:Literal>
                                            </td>
                                            <td>
                                                <asp:Literal ID="ltrlMobile1" runat="server"></asp:Literal>
                                                <br />
                                                <asp:Literal ID="ltrlMobile2" runat="server"></asp:Literal>
                                                <br />
                                                <asp:Literal ID="ltrlMobile3" runat="server"></asp:Literal>
                                            </td>
                                            <td>
                                                <asp:Literal ID="ltrlAddress1" runat="server"></asp:Literal>
                                                <br />
                                                <asp:Literal ID="ltrlPhone1" runat="server"></asp:Literal>
                                                <br />
                                                <asp:Literal ID="ltrlFax1" runat="server"></asp:Literal>
                                            </td>
                                            <td>
                                                <asp:Literal ID="ltrlAddress2" runat="server"></asp:Literal>
                                                <br />
                                                <asp:Literal ID="ltrlPhone2" runat="server"></asp:Literal>
                                                <br />
                                                <asp:Literal ID="ltrlFax2" runat="server"></asp:Literal>
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
                                <div style="justify-content: space-between; display: flex;">
                                    <h4 style="padding: 0 5px;"></h4>
                                    <div style="padding: 0 5px;">
                                        <br />
                                        <asp:Button ID="btnGuarantorContinue" runat="server" Text="<% $Resources:Resource, btnContinue %>" CssClass="btn btn-default" OnClick="btnGuarantorContinue_Click" />
                                    </div>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </div>
        </div>
        <div class="panel panel-default">
            <div class="panel-heading" role="tab" id="headingFour">
                <h4 class="panel-title">
                    <div style="justify-content: space-between; display: flex;">
                        <h4 style="padding: 0 5px;">
                            <a class="collapsed" role="button" data-toggle="collapse" data-parent="#accordion" href="#collapseFour" aria-expanded="false" aria-controls="collapseFour"><%= Resources.Resource.WitnessFormTitle %>
                            </a>
                        </h4>
                        <div style="padding: 0 5px;">
                            <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                <ContentTemplate>
                                    <asp:Button ID="btnNewWitness" runat="server" Text="<% $Resources:Resource, btnNew %>" CssClass="btn btn-primary" data-toggle="modal" data-target="#divwitnessdetails" OnClientClick="javascript:return false;" />
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </div>
                </h4>
            </div>
            <div id="collapseFour" class="panel-collapse collapse" role="tabpanel" aria-labelledby="headingFour">
                <div class="panel-body">
                    <div class="blockui">
                        <asp:UpdatePanel ID="upWitness" runat="server">
                            <ContentTemplate>
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
                                                <asp:Literal ID="ltrlWitnessName" runat="server"></asp:Literal>
                                                <br />
                                                <asp:Literal ID="ltrlIdNo" runat="server"></asp:Literal>
                                            </td>
                                            <td>
                                                <asp:Literal ID="ltrlMobile1" runat="server"></asp:Literal>
                                                <br />
                                                <asp:Literal ID="ltrlMobile2" runat="server"></asp:Literal>
                                                <br />
                                                <asp:Literal ID="ltrlMobile3" runat="server"></asp:Literal>
                                            </td>
                                            <td>
                                                <asp:Literal ID="ltrlAddress1" runat="server"></asp:Literal>
                                                <br />
                                                <asp:Literal ID="ltrlPhone1" runat="server"></asp:Literal>
                                                <br />
                                                <asp:Literal ID="ltrlFax1" runat="server"></asp:Literal>
                                            </td>
                                            <td>
                                                <asp:Literal ID="ltrlAddress2" runat="server"></asp:Literal>
                                                <br />
                                                <asp:Literal ID="ltrlPhone2" runat="server"></asp:Literal>
                                                <br />
                                                <asp:Literal ID="ltrlFax2" runat="server"></asp:Literal>
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
                            </ContentTemplate>
                        </asp:UpdatePanel>
                        <div style="justify-content: space-between; display: flex;">
                            <h4 style="padding: 0 5px;"></h4>
                            <div style="padding: 0 5px;">
                                <br />
                                <asp:Button ID="btnBackBottom" runat="server" CausesValidation="false" Text="<% $Resources:Resource, btnBack %>" CssClass="btn btn-default" PostBackUrl="contract.aspx" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <div class="modal fade" id="divguarantordetails">
        <div class="modal-dialog wide-modal-dialog">
            <div class="modal-content">
                <div class="blockui">
                    <asp:UpdatePanel ID="upguarantordetails" runat="server">
                        <ContentTemplate>
                            <asp:Panel ID="pnlguarantordetails" runat="server" DefaultButton="btnGuarantorSaveAndNew">
                                <asp:HiddenField ID="hdnModelGuarantor" runat="server"></asp:HiddenField>
                                <asp:HiddenField ID="hdnActionGuarantor" runat="server"></asp:HiddenField>
                                <div class="modal-header">
                                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                                    <h4 class="modal-title"><%= Resources.Resource.GuarantorFormTitle %></h4>
                                </div>
                                <div class="modal-body">
                                    <div class="form-horizontal" role="form">
                                        <asp:HiddenField ID="hdnGuarantorMasterId" runat="server" Visible="false"></asp:HiddenField>
                                        <div class="form-group">
                                            <label class="col-sm-4 control-label"><%= Resources.Resource.GuarantorIdNo %></label>
                                            <div class="col-sm-3">
                                                <asp:TextBox ID="txtSearchGuarantorIdNo" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                                <div class="text-danger">
                                                    <asp:RequiredFieldValidator ID="rfvSearchGuarantorIdNo" runat="server" SetFocusOnError="true" Display="Dynamic" ValidationGroup="searchguarantordetails" ControlToValidate="txtSearchGuarantorIdNo"><%= Resources.Messages.InputRequired %> <%=Resources.Resource.GuarantorIdNo %></asp:RequiredFieldValidator>
                                                </div>
                                            </div>
                                            <div class="col-sm-1">
                                                <asp:Button ID="btnSearchGuarantorIdNo" runat="server" CssClass="btn btn-default" ValidationGroup="searchguarantordetails" Text="<% $Resources:Resource, btnSearch %>" OnClick="btnSearchGuarantorIdNo_Click"></asp:Button>
                                                <asp:ValidationSummary ID="ValidationSummary2" runat="server" DisplayMode="BulletList"
                                                    ShowMessageBox="true" ShowSummary="false" ValidationGroup="searchguarantordetails" />
                                            </div>
                                        </div>
                                        <asp:Panel ID="pnlGuarantor" runat="server" Enabled="false">
                                            <div class="row">
                                                <div class="col-md-6">
                                                    <div class="form-group">
                                                        <label class="col-sm-4 control-label"><%= Resources.Resource.IdNo %></label>
                                                        <div class="col-sm-7">
                                                            <asp:TextBox ID="txtGuarantorIdNo" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                                            <div class="text-danger">
                                                                <asp:RequiredFieldValidator ID="rfvGuarantorIdNo" runat="server" SetFocusOnError="true" Display="Dynamic" ControlToValidate="txtGuarantorIdNo"
                                                                    ValidationGroup="guarantordetails"><%= Resources.Messages.InputRequired %> <%=Resources.Resource.IdNo %></asp:RequiredFieldValidator>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="form-group">
                                                        <label class="col-sm-4 control-label"><%= Resources.Resource.GuarantorName %></label>
                                                        <div class="col-sm-7">
                                                            <asp:TextBox ID="txtGuarantorName" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                                            <div class="text-danger">
                                                                <asp:RequiredFieldValidator ID="rfvGuarantorName" runat="server" SetFocusOnError="true" Display="Dynamic" ControlToValidate="txtGuarantorName"
                                                                    ValidationGroup="guarantordetails"><%= Resources.Messages.InputRequired %> <%=Resources.Resource.GuarantorName %></asp:RequiredFieldValidator>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="form-group">
                                                        <label class="col-sm-4 control-label"><%= Resources.Resource.PhotoIdImageName %></label>
                                                        <div class="col-sm-7">
                                                            <asp:FileUpload ID="fuGuarantorPhotoIdImageName" runat="server" />
                                                            <asp:HiddenField ID="hdnGuarantorPhotoIdImageName" runat="server" />
                                                            <asp:HiddenField ID="hdnGuarantorPhotoIdImageNameURL" runat="server" />
                                                        </div>
                                                    </div>
                                                    <div class="form-group">
                                                        <label class="col-sm-4 control-label"><%= Resources.Resource.Mobile1 %></label>
                                                        <div class="col-sm-7">
                                                            <asp:TextBox ID="txtGuarantorMobile1" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                    <div class="form-group">
                                                        <label class="col-sm-4 control-label"><%= Resources.Resource.Mobile2 %></label>
                                                        <div class="col-sm-7">
                                                            <asp:TextBox ID="txtGuarantorMobile2" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                    <div class="form-group">
                                                        <label class="col-sm-4 control-label"><%= Resources.Resource.Mobile3 %></label>
                                                        <div class="col-sm-7">
                                                            <asp:TextBox ID="txtGuarantorMobile3" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="col-md-6">
                                                    <div class="form-group">
                                                        <label class="col-sm-4 control-label"><%= Resources.Resource.Address1 %></label>
                                                        <div class="col-sm-7">
                                                            <asp:TextBox ID="txtGuarantorAddress1" runat="server" TextMode="MultiLine" CssClass="form-control input-sm" MaxLength="100"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                    <div class="form-group">
                                                        <label class="col-sm-4 control-label"><%= Resources.Resource.Phone1 %></label>
                                                        <div class="col-sm-7">
                                                            <asp:TextBox ID="txtGuarantorPhone1" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                                        </div>
                                                    </div>

                                                    <div class="form-group">
                                                        <label class="col-sm-4 control-label"><%= Resources.Resource.Fax1 %></label>
                                                        <div class="col-sm-7">
                                                            <asp:TextBox ID="txtGuarantorFax1" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                                        </div>
                                                    </div>

                                                    <div class="form-group">
                                                        <label class="col-sm-4 control-label"><%= Resources.Resource.Address2 %></label>
                                                        <div class="col-sm-7">
                                                            <asp:TextBox ID="txtGuarantorAddress2" runat="server" TextMode="MultiLine" CssClass="form-control input-sm" MaxLength="100"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                    <div class="form-group">
                                                        <label class="col-sm-4 control-label"><%= Resources.Resource.Phone2 %></label>
                                                        <div class="col-sm-7">
                                                            <asp:TextBox ID="txtGuarantorPhone2" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                    <div class="form-group">
                                                        <label class="col-sm-4 control-label"><%= Resources.Resource.Fax2 %></label>
                                                        <div class="col-sm-7">
                                                            <asp:TextBox ID="txtGuarantorFax2" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </asp:Panel>
                                    </div>
                                </div>
                                <div class="modal-footer">
                                    <asp:Button ID="btnGuarantorSaveAndNew" runat="server" Text="<% $Resources:Resource, btnSaveAndNew %>" CssClass="btn btn-primary" OnClick="btnGuarantorSave_Click" ValidationGroup="guarantordetails" />

                                    <asp:Button ID="btnGuarantorSave" runat="server" Text="<% $Resources:Resource, btnSave %>" CssClass="btn btn-primary" OnClick="btnGuarantorSave_Click" ValidationGroup="guarantordetails" />

                                    <asp:Button ID="btnGuarantorClose" runat="server" Text="<% $Resources:Resource, btnClose %>" CssClass="btn btn-default" CausesValidation="false" data-dismiss="modal" />
                                </div>
                            </asp:Panel>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>
    <asp:ValidationSummary ID="vsguarantordetails" runat="server" DisplayMode="BulletList"
        ShowMessageBox="true" ShowSummary="false" ValidationGroup="guarantordetails" />
    <div class="modal fade" id="divwitnessdetails">
        <div class="modal-dialog wide-modal-dialog">
            <div class="modal-content">
                <div class="blockui">
                    <asp:UpdatePanel ID="upwitnessdetails" runat="server">
                        <ContentTemplate>
                            <asp:Panel ID="pnlwitnessdetails" runat="server" DefaultButton="btnWitnessSaveAndNew">
                                <asp:HiddenField ID="hdnModelWitness" runat="server"></asp:HiddenField>
                                <asp:HiddenField ID="hdnActionWitness" runat="server"></asp:HiddenField>
                                <div class="modal-header">
                                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                                    <h4 class="modal-title"><%= Resources.Resource.WitnessFormTitle %></h4>
                                </div>
                                <div class="modal-body">
                                    <div class="form-horizontal" role="form">
                                        <asp:HiddenField ID="hdnWitnessMasterId" runat="server" Visible="false"></asp:HiddenField>
                                        <div class="form-group">
                                            <label class="col-sm-4 control-label"><%= Resources.Resource.WitnessIdNo %></label>
                                            <div class="col-sm-3">
                                                <asp:TextBox ID="txtSearchWitnessIdNo" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                                <div class="text-danger">
                                                    <asp:RequiredFieldValidator ID="rfvSearchWitnessIdNo" runat="server" SetFocusOnError="true" Display="Dynamic" ValidationGroup="searchwitnessdetails" ControlToValidate="txtSearchWitnessIdNo"><%= Resources.Messages.InputRequired %> <%=Resources.Resource.WitnessIdNo %></asp:RequiredFieldValidator>
                                                </div>
                                            </div>
                                            <div class="col-sm-1">
                                                <asp:Button ID="btnSearchWitnessIdNo" runat="server" CssClass="btn btn-default" ValidationGroup="searchwitnessdetails" Text="<% $Resources:Resource, btnSearch %>" OnClick="btnSearchWitnessIdNo_Click"></asp:Button>
                                                <asp:ValidationSummary ID="ValidationSummary3" runat="server" DisplayMode="BulletList"
                                                    ShowMessageBox="true" ShowSummary="false" ValidationGroup="searchwitnessdetails" />
                                            </div>
                                        </div>
                                        <asp:Panel ID="pnlWitness" runat="server" Enabled="false">
                                            <div class="row">
                                                <div class="col-md-6">
                                                    <div class="form-group">
                                                        <label class="col-sm-4 control-label"><%= Resources.Resource.IdNo %></label>
                                                        <div class="col-sm-7">
                                                            <asp:TextBox ID="txtWitnessIdNo" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                                            <div class="text-danger">
                                                                <asp:RequiredFieldValidator ID="rfvWitnessIdNo" runat="server" SetFocusOnError="true" Display="Dynamic" ControlToValidate="txtWitnessIdNo"
                                                                    ValidationGroup="witnessdetails"><%= Resources.Messages.InputRequired %> <%=Resources.Resource.IdNo %></asp:RequiredFieldValidator>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="form-group">
                                                        <label class="col-sm-4 control-label"><%= Resources.Resource.WitnessName %></label>
                                                        <div class="col-sm-7">
                                                            <asp:TextBox ID="txtWitnessName" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                                            <div class="text-danger">
                                                                <asp:RequiredFieldValidator ID="rfvWitnessName" runat="server" SetFocusOnError="true" Display="Dynamic" ControlToValidate="txtWitnessName"
                                                                    ValidationGroup="witnessdetails"><%= Resources.Messages.InputRequired %> <%=Resources.Resource.WitnessName %></asp:RequiredFieldValidator>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="form-group">
                                                        <label class="col-sm-4 control-label"><%= Resources.Resource.PhotoIdImageName %></label>
                                                        <div class="col-sm-7">
                                                            <asp:FileUpload ID="fuWitnessPhotoIdImageName" runat="server" />
                                                            <asp:HiddenField ID="hdnWitnessPhotoIdImageName" runat="server" />
                                                            <asp:HiddenField ID="hdnWitnessPhotoIdImageNameURL" runat="server" />
                                                        </div>
                                                    </div>
                                                    <div class="form-group">
                                                        <label class="col-sm-4 control-label"><%= Resources.Resource.Mobile1 %></label>
                                                        <div class="col-sm-7">
                                                            <asp:TextBox ID="txtWitnessMobile1" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                    <div class="form-group">
                                                        <label class="col-sm-4 control-label"><%= Resources.Resource.Mobile2 %></label>
                                                        <div class="col-sm-7">
                                                            <asp:TextBox ID="txtWitnessMobile2" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                    <div class="form-group">
                                                        <label class="col-sm-4 control-label"><%= Resources.Resource.Mobile3 %></label>
                                                        <div class="col-sm-7">
                                                            <asp:TextBox ID="txtWitnessMobile3" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="col-md-6">
                                                    <div class="form-group">
                                                        <label class="col-sm-4 control-label"><%= Resources.Resource.Address1 %></label>
                                                        <div class="col-sm-7">
                                                            <asp:TextBox ID="txtWitnessAddress1" runat="server" TextMode="MultiLine" CssClass="form-control input-sm"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                    <div class="form-group">
                                                        <label class="col-sm-4 control-label"><%= Resources.Resource.Phone1 %></label>
                                                        <div class="col-sm-7">
                                                            <asp:TextBox ID="txtWitnessPhone1" runat="server" CssClass="form-control input-sm" MaxLength="15"></asp:TextBox>
                                                        </div>
                                                    </div>

                                                    <div class="form-group">
                                                        <label class="col-sm-4 control-label"><%= Resources.Resource.Fax1 %></label>
                                                        <div class="col-sm-7">
                                                            <asp:TextBox ID="txtWitnessFax1" runat="server" CssClass="form-control input-sm" MaxLength="15"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                    <div class="form-group">
                                                        <label class="col-sm-4 control-label"><%= Resources.Resource.Address2 %></label>
                                                        <div class="col-sm-7">
                                                            <asp:TextBox ID="txtWitnessAddress2" runat="server" TextMode="MultiLine" CssClass="form-control input-sm" MaxLength="100"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                    <div class="form-group">
                                                        <label class="col-sm-4 control-label"><%= Resources.Resource.Phone2 %></label>
                                                        <div class="col-sm-7">
                                                            <asp:TextBox ID="txtWitnessPhone2" runat="server" CssClass="form-control input-sm" MaxLength="15"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                    <div class="form-group">
                                                        <label class="col-sm-4 control-label"><%= Resources.Resource.Fax2 %></label>
                                                        <div class="col-sm-7">
                                                            <asp:TextBox ID="txtWitnessFax2" runat="server" CssClass="form-control input-sm" MaxLength="15"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </asp:Panel>
                                    </div>
                                </div>
                                <div class="modal-footer">
                                    <asp:Button ID="btnWitnessSaveAndNew" runat="server" Text="<% $Resources:Resource, btnSaveAndNew %>" CssClass="btn btn-primary" OnClick="btnWitnessSave_Click" ValidationGroup="witnessdetails" />

                                    <asp:Button ID="btnWitnessSave" runat="server" Text="<% $Resources:Resource, btnSave %>" CssClass="btn btn-primary" OnClick="btnWitnessSave_Click" ValidationGroup="witnessdetails" />

                                    <asp:Button ID="btnWitnessClose" runat="server" Text="<% $Resources:Resource, btnClose %>" CssClass="btn btn-default" CausesValidation="false" data-dismiss="modal" />
                                </div>
                            </asp:Panel>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>
    <asp:ValidationSummary ID="vswitnessdetails" runat="server" DisplayMode="BulletList"
        ShowMessageBox="true" ShowSummary="false" ValidationGroup="witnessdetails" />
    <script type="text/javascript">
        $(document).ready(function () {
            SetControlsPicker();
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(SetControlsPicker);
        });

        function SetControlsPicker() {
            SetDatePicker(Prefix + "txtContractDate");
            SetDatePicker(Prefix + "txtContractStartDate");
            $("#" + Prefix + "txtContractStartDate").on("dp.change", function (e) {
                SetInstallmentDate();
            });
            SetDatePicker(Prefix + "txtInstallmentDate");
            //SetImageUpload(Prefix + "fuPhotoIdImageName");
            SetImageUpload(Prefix + "fuGuarantorPhotoIdImageName");
            SetImageUpload(Prefix + "fuWitnessPhotoIdImageName");
            SearchCustomer();
            SearchGuarantor();
            SearchWitness();
        }
    </script>
    <script type="text/javascript">
        $(document).ready(function () {
            for (i = 0; i < Page_Validators.length; i++) {
                Page_Validators[i].errormessage = Page_Validators[i].innerHTML;
            }
            EnableDisableContract();
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EnableDisableContract);
            SetCustomer();
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(SetCustomer);
        });
        function SearchCustomer() {
            $("#" + Prefix + "txtSearchCustomerIdNo").autocomplete({
                source: function (request, response) {
                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: "contractdetails.aspx/GetCustomerMaster",
                        data: "{'customer':'" + $("#" + Prefix + "txtSearchCustomerIdNo").val() + "'}",
                        dataType: "json",
                        success: function (data) {
                            response($.map(data.d, function (item) {
                                return {
                                    value: item.CustomerName + " (" + item.IdNo + ")"
                                }
                            }))
                        },
                    });
                },
                minLength: 3,
                select: function (event, ui) {
                    $("#" + Prefix + "txtSearchCustomerIdNo").val(ui.item.value);
                    $("#" + Prefix + "btnSearchCustomerIdNo").click();
                },
            });
        }
        //function SetCustomer() {
        //    if ($("#" + Prefix + "hdnPhotoIdImageName").val() != "") {
        //        $("." + Prefix + "fuPhotoIdImageName").attr("src", $("#" + Prefix + "hdnPhotoIdImageNameURL").val());
        //    }
        //    else {
        //        if ($("#" + Prefix + "fuPhotoIdImageName").val() != "") {
        //            $("#" + Prefix + "fuPhotoIdImageName").fileinput("clear");
        //        }
        //        $("." + Prefix + "fuPhotoIdImageName").attr("src", "img/xs_NoImage.png");
        //    }
        //}
        function CalculateContact() {
            var ContractAmount = $("#" + Prefix + "txtContractAmount").val();
            var IsBasedOnMonth = $("#" + Prefix + "chkIsBasedOnMonth").prop('checked');
            if (IsBasedOnMonth) {
                var NoOfInstallments = $("#" + Prefix + "txtNoOfInstallments").val();
                if (NoOfInstallments != "") {
                    var InstallmentAmount = ContractAmount / NoOfInstallments;
                    if (isNaN(InstallmentAmount) == false) {
                        $("#" + Prefix + "txtInstallmentAmount").val(InstallmentAmount.toFixed(2));
                    }
                    else {
                        $("#" + Prefix + "txtInstallmentAmount").val("");
                    }
                }
            }
            else {
                var InstallmentAmount = $("#" + Prefix + "txtInstallmentAmount").val();
                if (InstallmentAmount != "") {
                    var NoOfInstallments = ContractAmount / InstallmentAmount;
                    if (isNaN(InstallmentAmount) == false) {
                        $("#" + Prefix + "txtNoOfInstallments").val(Math.ceil(NoOfInstallments));
                    }
                    else {
                        $("#" + Prefix + "txtNoOfInstallments").val("");
                    }
                }
            }
            EnableDisableContract();
        }
        function EnableDisableContract() {
            var IsBasedOnMonth = $("#" + Prefix + "chkIsBasedOnMonth").prop('checked');
            if (IsBasedOnMonth) {
                $("#" + Prefix + "txtNoOfInstallments").prop('disabled', false);
                $("#" + Prefix + "txtInstallmentAmount").prop('disabled', true);
            }
            else {
                $("#" + Prefix + "txtNoOfInstallments").prop('disabled', true);
                $("#" + Prefix + "txtInstallmentAmount").prop('disabled', false);
            }
        }
        function SetInstallmentDate() {

            var ContractStartDate = $("#" + Prefix + "txtContractStartDate").val();
            var d = GetStringToDate(ContractStartDate);
            $("#" + Prefix + "txtInstallmentDate").val(GetDateToString(d));

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
        function SearchGuarantor() {
            $("#" + Prefix + "txtSearchGuarantorIdNo").autocomplete({
                source: function (request, response) {
                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: "contractdetails.aspx/GetGuarantorMaster",
                        data: "{'guarantor':'" + $("#" + Prefix + "txtSearchGuarantorIdNo").val() + "'}",
                        dataType: "json",
                        success: function (data) {
                            response($.map(data.d, function (item) {
                                return {
                                    value: item.GuarantorName + " (" + item.IdNo + ")"
                                }
                            }))
                        },
                    });
                },
                minLength: 3,
                select: function (event, ui) {
                    $("#" + Prefix + "txtSearchGuarantorIdNo").val(ui.item.value);
                    $("#" + Prefix + "btnSearchGuarantorIdNo").click();
                },
            });
        }
        function SetDialogShowHideGuarantorMaster() {
            $("#divguarantordetails").on("hidden.bs.modal", function () {
                ClearGuarantorMaster();
            })
            $("#divguarantordetails").on("show.bs.modal", function () {
                SetGuarantorMaster();
            })
            $("#divguarantordetails").on("shown.bs.modal", function () {
                $("#" + Prefix + "txtGuarantorName").focus();
                if ($("#" + Prefix + "hdnGuarantorPhotoIdImageName").val() != "") {
                    $("." + Prefix + "fuGuarantorPhotoIdImageName").attr("src", $("#" + Prefix + "hdnGuarantorPhotoIdImageNameURL").val());
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
                    $("#" + Prefix + "txtGuarantorIdNo").val("");
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
            $("#" + Prefix + "txtSearchGuarantorIdNo").val("");
            $("#" + Prefix + "txtGuarantorIdNo").val("");
            $("#" + Prefix + "txtGuarantorName").val("");
            if ($("#" + Prefix + "fuGuarantorPhotoIdImageName").val() != "") {
                $("#" + Prefix + "fuGuarantorPhotoIdImageName").fileinput("clear");
            }
            $("#" + Prefix + "hdnGuarantorPhotoIdImageName").val("");
            $("." + Prefix + "fuGuarantorPhotoIdImageName").attr("src", "img/xs_NoImage.png");
            $("#" + Prefix + "txtGuarantorAddress1").val("");
            $("#" + Prefix + "txtGuarantorPhone1").val("");
            $("#" + Prefix + "txtGuarantorMobile1").val("");
            $("#" + Prefix + "txtGuarantorFax1").val("");
            $("#" + Prefix + "txtGuarantorAddress2").val("");
            $("#" + Prefix + "txtGuarantorPhone2").val("");
            $("#" + Prefix + "txtGuarantorMobile2").val("");
            $("#" + Prefix + "txtGuarantorFax2").val("");
            $("#" + Prefix + "txtGuarantorMobile3").val("");

        }
        function SetGuarantorMaster() {
            for (i = 0; i < Page_Validators.length; i++) {
                Page_Validators[i].errormessage = Page_Validators[i].innerHTML;
            }

            if ($("#" + Prefix + "hdnActionGuarantor").val() == "") {
                $("#" + Prefix + "btnGuarantorSaveAndNew").css("visibility", "visible");
                $("#" + Prefix + "btnGuarantorSave").val('<%= GetGlobalResourceObject("Resource", "btnSave") %>');
            }
            else {
                $("#" + Prefix + "btnGuarantorSaveAndNew").css("visibility", "hidden");
                $("#" + Prefix + "btnGuarantorSave").val('<%= GetGlobalResourceObject("Resource", "btnUpdate") %>');
            }
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
        function SearchWitness() {
            $("#" + Prefix + "txtSearchWitnessIdNo").autocomplete({
                source: function (request, response) {
                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: "contractdetails.aspx/GetWitnessMaster",
                        data: "{'witness':'" + $("#" + Prefix + "txtSearchWitnessIdNo").val() + "'}",
                        dataType: "json",
                        success: function (data) {
                            response($.map(data.d, function (item) {
                                return {
                                    value: item.WitnessName + " (" + item.IdNo + ")"
                                }
                            }))
                        },
                    });
                },
                minLength: 3,
                select: function (event, ui) {
                    $("#" + Prefix + "txtSearchWitnessIdNo").val(ui.item.value);
                    $("#" + Prefix + "btnSearchWitnessIdNo").click();
                },
            });
        }
        function SetDialogShowHideWitnessMaster() {
            $("#divwitnessdetails").on("hidden.bs.modal", function () {
                ClearWitnessMaster();
            })
            $("#divwitnessdetails").on("show.bs.modal", function () {
                SetWitnessMaster();
            })
            $("#divwitnessdetails").on("shown.bs.modal", function () {
                $("#" + Prefix + "txtWitnessName").focus();
                if ($("#" + Prefix + "hdnWitnessPhotoIdImageName").val() != "") {
                    $("." + Prefix + "fuWitnessPhotoIdImageName").attr("src", $("#" + Prefix + "hdnWitnessPhotoIdImageNameURL").val());
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
                    $("#" + Prefix + "txtWitnessIdNo").val("");
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
            $("#" + Prefix + "txtSearchWitnessIdNo").val("");
            $("#" + Prefix + "txtWitnessIdNo").val("");
            $("#" + Prefix + "txtWitnessName").val("");
            if ($("#" + Prefix + "fuWitnessPhotoIdImageName").val() != "") {
                $("#" + Prefix + "fuWitnessPhotoIdImageName").fileinput("clear");
            }
            $("#" + Prefix + "hdnWitnessPhotoIdImageName").val("");
            $("." + Prefix + "fuWitnessPhotoIdImageName").attr("src", "img/xs_NoImage.png");
            $("#" + Prefix + "txtWitnessAddress1").val("");
            $("#" + Prefix + "txtWitnessPhone1").val("");
            $("#" + Prefix + "txtWitnessMobile1").val("");
            $("#" + Prefix + "txtWitnessFax1").val("");
            $("#" + Prefix + "txtWitnessAddress2").val("");
            $("#" + Prefix + "txtWitnessPhone2").val("");
            $("#" + Prefix + "txtWitnessMobile2").val("");
            $("#" + Prefix + "txtWitnessFax2").val("");
            $("#" + Prefix + "txtWitnessMobile3").val("");

        }
        function SetWitnessMaster() {
            for (i = 0; i < Page_Validators.length; i++) {
                Page_Validators[i].errormessage = Page_Validators[i].innerHTML;
            }
            if ($("#" + Prefix + "hdnActionWitness").val() == "") {
                $("#" + Prefix + "btnWitnessSaveAndNew").css("visibility", "visible");
                $("#" + Prefix + "btnWitnessSave").val('<%= GetGlobalResourceObject("Resource", "btnSave") %>');
            }
            else {
                $("#" + Prefix + "btnWitnessSaveAndNew").css("visibility", "hidden");
                $("#" + Prefix + "btnWitnessSave").val('<%= GetGlobalResourceObject("Resource", "btnUpdate") %>');
            }
        }
    </script>
</asp:Content>
