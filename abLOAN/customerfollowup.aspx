<%@ Page Language="C#" MasterPageFile="Site.Master" AutoEventWireup="true" CodeBehind="customerfollowup.aspx.cs"
    Inherits="abLOAN.customerfollowup" %>

<%@ Register Assembly="CustomDataPager" Namespace="CustomDataPager" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script type="text/javascript">
        $(document).ready(function () {
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequest);
            SetFilter();
        });
        function EndRequest(sender, args) {
            ShowHideDialogCustomerFollowup();
            SetFilter();
        }
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphPage" runat="server">
    <div id="divcontract">
        <div class="blockui">
            <asp:UpdatePanel ID="upcontract" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <div class="row">
                        <div style="justify-content: space-between; display: flex;">
                            <div style="padding: 0 15px;">
                                <h4>
                                    <i class="fa fa-th"></i>&nbsp;<%= Resources.Resource.ContractPageTitle %>&nbsp;
                                    <small>
                                        <asp:Label ID="lblRecords" runat="server" Visible="false"></asp:Label></small>
                                    &nbsp;
                                    <a id="aFilter" class="btn btn-default btn-sm" href="" style="text-decoration: none;" onclick="javascript:return ShowHideFilter('divFilter', false);"><%= Resources.Resource.Filter %>&nbsp;&nbsp;<span class="caret"></span></a>
                                </h4>
                            </div>
                            <div style="padding: 0 15px;">
                                <asp:Button ID="btnDownload" runat="server" Text="<% $Resources:Resource, Download %>" CssClass="btn btn-info" OnClick="btnDownload_Click" />
                                &nbsp;
                                <asp:Button ID="btnNew" runat="server" Text="<% $Resources:Resource, btnNew %>" 
                                    CssClass="btn btn-primary" data-toggle="modal" data-target="#divfollowupdetails" 
                                    OnClientClick="javascript:return false;"
                                    />
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
                                                    <label class="col-sm-5 control-label"><%= Resources.Resource.Username %></label>
                                                    <div class="col-sm-7">
                                                        <asp:TextBox ID="txtFilterUsername" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>


                                        </div>
                                        <div class="row">


                                            <div class="col-md-4 col-sm-6">
                                                <div class="form-group">
                                                    <label class="col-sm-5 control-label"><%= Resources.Resource.ContractStartDate %></label>
                                                    <div class="col-sm-7">
                                                        <asp:TextBox ID="txtFilterContractDate" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                                        <div class="text-danger">
                                                            <asp:RequiredFieldValidator ID="rfvFilterContractDate" runat="server"
                                                                ControlToValidate="txtFilterContractDate" Display="Dynamic"
                                                                ValidationGroup="FilterContractMaster"><%= Resources.Messages.InputRequired %> <%=Resources.Resource.ContractDate %></asp:RequiredFieldValidator>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-md-4 col-sm-6">
                                                <div class="form-group">
                                                    <div class="col-sm-7">
                                                        <asp:TextBox ID="txtFilterContractDateTo" runat="server" CssClass="form-control input-sm">

                                                        </asp:TextBox>
                                                        <div class="text-danger">
                                                            <asp:RequiredFieldValidator ID="rfvFilterContractDateTo" runat="server"
                                                                ControlToValidate="txtFilterContractDateTo" Display="Dynamic"
                                                                ValidationGroup="FilterContractMaster"><%= Resources.Messages.InputRequired %> <%=Resources.Resource.ContractDate %></asp:RequiredFieldValidator>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-md-4 col-sm-6">
                                                <div class="form-group">
                                                    <label class="col-sm-5 control-label"><%= Resources.Resource.ContractStatus %></label>
                                                    <div class="col-sm-7">
                                                        <asp:DropDownList ID="ddlFilterContractStatus" runat="server" CssClass="form-control input-sm"></asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>





                                            <div class="col-md-4 col-sm-6">
                                                <div class="form-group">
                                                    <label class="col-sm-5 control-label"><%= Resources.Resource.SortBy %></label>
                                                    <div class="col-sm-7">
                                                        <asp:DropDownList ID="ddlSortBy" runat="server" CssClass="form-control input-sm">
                                                            <asp:ListItem Text="- SELECT -" Value=""></asp:ListItem>
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-md-12 text-center">
                                                <asp:Button ID="btnSearch" runat="server" Text="<% $Resources:Resource, btnSearch %>" CssClass="btn btn-primary" ValidationGroup="FilterContractMaster" OnClick="btnSearch_Click" />
                                                &nbsp;
                                                <asp:Button ID="btnReset" runat="server" Text="<% $Resources:Resource, btnReset %>" CssClass="btn btn-default" CausesValidation="false" OnClick="btnReset_Click" />
                                            </div>
                                        </div>
                                    </div>
                                </asp:Panel>
                                <asp:ValidationSummary ID="vsContractMaster" runat="server" DisplayMode="BulletList"
                                    ShowMessageBox="true" ShowSummary="false" ValidationGroup="FilterContractMaster" />
                            </div>
                        </div>
                    </div>

                    <asp:ListView ID="lvCustomerFollowedup" 
                        DataKeyNames="CustomerFollowupId"
                        OnItemCommand="lvCustomerFollowedup_ItemCommand"
                        OnItemDataBound="lvCustomerFollowedup_ItemDataBound" runat="server">
                        <ItemTemplate>

                            <asp:HiddenField ID="lblCustomerFollowupId" runat="server" />
                            <div class="card-container">


                                <div class="container row">

                                    <div class="col-md-8">
                                        <div>

                                            <asp:Label class="primary-card" ID="lblUsername" runat="server"></asp:Label>
                                        </div>
                                        <h4>
                                            <b>
                                                 <asp:Label class="" ID="lblCustomerIdNo" runat="server"></asp:Label> -
                                                <asp:Label ID="lblCustomer" runat="server"></asp:Label>
                                            </b>

                                        </h4>
                                        <h5>

                                            <asp:Label ID="lblMobile1" runat="server"></asp:Label>
                                        </h5>

                                        <h5>

                                            <asp:Label ID="lblMobile2" runat="server"></asp:Label>
                                        </h5>

                                        <h5>

                                            <asp:Label ID="lblMobile3" runat="server"></asp:Label>
                                        </h5>
                                    </div>

                                    <div class="col-md-4">
                                        <div>

                                           

                                             <asp:LinkButton ID="lbtnEdit" 
                                                 runat="server" CommandName="EditRecord"
                                                 CssClass="btn btn-primary btn-sm fa fa-edit"  
                                                 title="Edit"></asp:LinkButton>

                                            <asp:LinkButton ID="lbtnDelete" runat="server" 
                                                CommandName="DeleteRecord" 
                                                OnClientClick="javascript:return ConfirmDelete(this);" 
                                                CssClass="btn btn-danger btn-sm fa fa-trash" 
                                                title="Delete"></asp:LinkButton>
                                        </div>



                                    </div>

                                </div>


                                <div class="container row" runat="server">
                                    <!-- Your form inputs here -->


                                    <div class="col-md-8">
                                        <asp:TextBox CssClass=" form-control " ID="ltrlNotes"
                                            runat="server" placeholder="Add Your Note"></asp:TextBox>
                                    </div>

                                    <div>

                                        <asp:Button CssClass="btn btn-primary"
                                            runat="server" Text="Add Note" OnClick="insertNoteBtn_Click" />
                                    </div>


                                </div>


                            </div>

                            <div class="card-container"> 
                               
                                    <div class="card card-body"> 

                                                                          <p> 
              <button class="btn btn-primary" type="button" data-toggle="collapse"
 data-target='<%# "#collapseExample" + Container.DataItemIndex %>' aria-expanded="false"
 aria-controls='<%# "collapseExample" + Container.DataItemIndex %>'>
        Show Note  
     </button>
</p>           

<div class="collapse" id='<%# "collapseExample" + Container.DataItemIndex %>'>
                                        <asp:ListView ID="lvFollowupNote" runat="server"
                                            DataKeyNames="FollowupNoteId"
                                            DataSource='<%# getFollowUpNoteDataSource(Eval("CustomerFollowupId"))  %>'
                                            OnItemCommand="pgrFollowupNote_ItemCommand"
                                            OnItemDataBound="lvFollowupNote_ItemDataBound">
                        

                                            <LayoutTemplate>
                                                <div class="panel panel-default">
                                                    <div class="table-responsive">
                                                        <table id="itemPlaceholderContainer" runat="server" class="table table-hover">
                                                            <tr>

                                                                <th>ID
                                                                </th>

                                                                <th>Note
                                                                </th>
                                                                <th>Username
                                                                </th>


                                                                <th style="width: 125px">Datetime
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

                                                        <asp:Literal ID="ltrlFollowupNoteId" runat="server"></asp:Literal>
                                                    </td>
                                                    <td>

                                                        <asp:Literal ID="ltrlNote" runat="server"></asp:Literal>
                                                    </td>
                                                    <td>

                                                        <asp:Literal ID="ltrlUsername" runat="server"></asp:Literal>


                                                    </td>
                                                    <td>

                                                        <asp:Label ID="ltrlCreateDate" runat="server"></asp:Label>

                                                    </td>

                                                    <td style="padding-top: 15px;"></td>

                                                </tr>

                                            </ItemTemplate>
                                            <EmptyDataTemplate>
                                                <div class="alert alert-info">
                                                    <%= Resources.Resource.NoRecordMessage %>
                                                </div>
                                            </EmptyDataTemplate>


                                        </asp:ListView> 
          </div>
                                         
                                    </div>
                             
                            </div>

                            <div class="card-container">

                                <asp:ListView ID="lvContractMaster" runat="server"
                                    DataKeyNames="ContractMasterId"
                                    DataSource='<%# getContractDataSource(Eval("linktoCustomerMasterId"))  %>'
                                    OnItemCommand="lvContractMaster_ItemCommand"
                                    OnItemDataBound="lvContractMaster_ItemDataBound">

                                    <LayoutTemplate>
                                        <div class="panel panel-default">
                                            <div class="table-responsive">
                                                <table id="itemPlaceholderContainer" runat="server" class="table table-hover">
                                                    <tr>

                                                        <th><%= Resources.Resource.ContractFormTitle %>
                                                        </th>
                                                        <th><%= Resources.Resource.Installment %>
                                                        </th>
                                                        <th><%= "Last 3 Installments" %>
                                                        </th>

                                                        <th style="width: 125px"><%= Resources.Resource.ContractStatus %>
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
                                                <span class="text-muted"><%= Resources.Resource.Contract %>:
                                                </span>
                                                <asp:Literal ID="ltrlContractTitle" runat="server"></asp:Literal>
                                            </td>
                                            <td>

                                                <span class="text-muted"><%= Resources.Resource.InstallmentDate %>:
                                                </span>
                                                <asp:Literal ID="ltrlInstallmentDate" runat="server"></asp:Literal>


                                            </td>
                                            <td>

                                                <asp:Label ID="lblLast3Payments" runat="server"></asp:Label>

                                            </td>

                                            <td style="padding-top: 15px;">
                                                <asp:Label ID="lblContractStatus" runat="server"></asp:Label>
                                            </td>

                                        </tr>

                                    </ItemTemplate>
                                    <EmptyDataTemplate>
                                        <div class="alert alert-info">
                                            <%= Resources.Resource.NoRecordMessage %>
                                        </div>
                                    </EmptyDataTemplate>
                                </asp:ListView>

                            </div>
                        </ItemTemplate>
                    </asp:ListView>


                    <div class="row">
                        <div class="col-md-12 text-center">
                            <cc1:DataPager ID="pgrFollowupMaster" runat="server" OnPagerCommand="pgrFollowupMaster_ItemCommand"
                                Visible="false" CssClass="pagination" CurrentPageCssClass="active"></cc1:DataPager>
                        </div>
                    </div>


                </ContentTemplate>
                <Triggers>
                    <asp:PostBackTrigger ControlID="btnDownload" />
                </Triggers>
            </asp:UpdatePanel>
        </div>

    </div>


    <div class="modal fade" id="divfollowupdetails">
        <div class="modal-dialog wide-modal-dialog">
            <div class="modal-content">
                <div class="blockui">
                    <asp:UpdatePanel ID="upfollowupdetails" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:Panel ID="pnlfollowupdetails" runat="server" DefaultButton="btnSaveAndNew">
                                <asp:HiddenField ID="hdnModelFollowup" runat="server"></asp:HiddenField>
                                <asp:HiddenField ID="hdnActionFollowup" runat="server"></asp:HiddenField>
                                <div class="modal-header">
                                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                                    <h4 class="modal-title"><%= Resources.Resource.CustomerFormTitle %></h4>
                                </div>
                                <div class="modal-body">
                                    <div class="form-horizontal" role="form">
                                        <div class="row">
                                            <asp:HiddenField ID="hdnCustomerFollowupId" runat="server" Visible="false"></asp:HiddenField>

                                            <asp:HiddenField ID="hdnCustomerMasterId" runat="server" Visible="false"></asp:HiddenField>
                                            <div class="col-lg-6">
                                                <div class="form-group">
                                                    <label class="col-sm-4 control-label"><%= Resources.Resource.Username %></label>


                                                    <div class="col-sm-7">
                                                        <asp:DropDownList ID="ddlAuditors" runat="server" CssClass="form-control input-sm"></asp:DropDownList>
                                                    </div>
                                                </div>

                                            </div>

                                        </div>


                                        <div class="row">
                                            <asp:HiddenField ID="HiddenField1" runat="server" Visible="false"></asp:HiddenField>
                                            <div class="col-lg-12">
                                                <div class="form-group">
                                                    <label class="col-sm-4   control-label"><%= Resources.Resource.CustomerName %></label>


                                                    <div class="col-sm-8">
                                                        <div class="col-sm-10">
                                                            <asp:TextBox ID="txtSearchCustomerIdNo" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                                            <div class="text-danger">
                                                                <asp:RequiredFieldValidator ID="rfvSearchCustomerIdNo" runat="server" ControlToValidate="txtSearchCustomerIdNo" Display="Dynamic" SetFocusOnError="true" ValidationGroup="searchcustomerdetails"><%= Resources.Messages.InputRequired %><%=Resources.Resource.CustomerIdNo %></asp:RequiredFieldValidator>
                                                            </div>
                                                        </div>

                                                        <div class="col-sm-1">
                                                            <asp:Button ID="btnSearchCustomerIdNo" runat="server" CssClass="btn btn-default" Text="<% $Resources:Resource, btnSearch %>" ValidationGroup="searchcustomerdetails" OnClick="btnSearchCustomerIdNo_Click"></asp:Button>
                                                            <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="BulletList"
                                                                ShowMessageBox="true" ShowSummary="false" ValidationGroup="searchcustomerdetails" />
                                                        </div>
                                                    </div>
                                                </div>




                                            </div>

                                        </div>

                                        <div class="row">

                                            <div class="form-group">
                                                <div class="col-lg-6">
                                                    <label class="col-sm-4 control-label"><%= Resources.Resource.IdNo %></label>
                                                    <div class="col-sm-7">
                                                        <asp:TextBox ID="txtIdNo" CssClass="form-control input-sm" runat="server" Enabled="false"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="col-lg-6">
                                                    <div class="form-group">
                                                        <label class="col-sm-4 control-label"><%= Resources.Resource.CustomerName %></label>
                                                        <div class="col-sm-7">
                                                            <asp:TextBox ID="txtCustomerName" runat="server" CssClass="form-control input-sm" Enabled="false"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="modal-footer">
                                    <asp:Button ID="btnSaveAndNew" runat="server" Text="<% $Resources:Resource, btnSaveAndNew %>" CssClass="btn btn-primary" OnClick="btnSave_Click" ValidationGroup="followupdetails" />

                                    <asp:Button ID="btnSave" runat="server" Text="<% $Resources:Resource, btnSave %>" CssClass="btn btn-primary" OnClick="btnSave_Click" ValidationGroup="followupdetails" />


                                    <asp:Button ID="btnClose" runat="server" Text="<% $Resources:Resource, btnClose %>" CssClass="btn btn-default" CausesValidation="false" data-dismiss="modal" />

                                </div>
                            </asp:Panel>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="lvCustomerFollowedup" EventName="ItemCommand" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>






     <script type="text/javascript">
        $(document).ready(function () {
            SetFilterControlsPicker();
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(SetFilterControlsPicker);
        });

        function SetFilterControlsPicker() {
            SetDatePicker(Prefix + "txtFilterContractDate");
            SetDatePicker(Prefix + "txtFilterContractDateTo");
            SearchCustomer();
        }

        function SearchCustomer() {
            $("#" + Prefix + "txtFilterCustomer").autocomplete({
                source: function (request, response) {
                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: "contractdetails.aspx/GetCustomerMaster",
                        data: "{'customer':'" + $("#" + Prefix + "txtFilterCustomer").val() + "'}",
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
                    $("#" + Prefix + "txtFilterCustomer").val(ui.item.value);
                    $("#" + Prefix + "btnSearch").click();
                },
            });
        }
    </script> 


   


     <script type="text/javascript">
         $(document).ready(function () {
             if ($("#" + Prefix + "hdnModelFollowup").val() == "") {
                 ClearCustomerFollowup();
             }
             SetDialogShowHideCustomerFollowup();
             ShowHideDialogCustomerFollowup();
         });

         function SetDialogShowHideCustomerFollowup() {

             $("#divfollowupdetails").on("hidden.bs.modal", function () {
                 ClearCustomerFollowup();
             
             })
             $("#divfollowupdetails").on("show.bs.modal", function () {
                 SetCustomerFollowup();
             })
             $("#divfollowupdetails").on("shown.bs.modal", function () {
                 $("#" + Prefix + "txtCustomerName").focus();
             })
         }
         function ShowHideDialogCustomerFollowup() {
             try {
                 console.log("dialog in")
                 SetCustomerFollowup();
                 if ($("#" + Prefix + "hdnModelFollowup").val() == "show") {
                     console.log("Show")
                     $("#divfollowupdetails").modal("show");
                 }
                 else if ($("#" + Prefix + "hdnModelFollowup").val() == "hide") {
                     console.log("hide")
                     ClearCustomerFollowup();
                     $("#divfollowupdetails").modal("hide");
                 }
                 else if ($("#" + Prefix + "hdnModelFollowup").val() == "clear") {
                     console.log("Clear")
                     ClearCustomerFollowup();
                     $("#divfollowupdetails").modal("show");
                     $("#" + Prefix + "txtSearchCustomerIdNo").focus();
                 }
                 $("#" + Prefix + "hdnModelFollowup").val("");
                 return false;
             }
             catch (ex) {
                 alert(ex);
                 return false;
             }
         }
         function ClearCustomerFollowup() {
             for (i = 0; i < Page_Validators.length; i++) {
                 Page_Validators[i].style.display = "none";
             }
             $("#" + Prefix + "hdnActionFollowup").val("");
             $("#" + Prefix + "txtCustomerName").val("");
             $("#" + Prefix + "txtIdNo").val(""); 
             $("#" + Prefix + "txtSearchCustomerIdNo").val("");  
             $("#" + Prefix + "ddlAuditors").prop("selectedIndex", 0);
             



         }


         function SetCustomerFollowup() {
             for (i = 0; i < Page_Validators.length; i++) {
                 Page_Validators[i].errormessage = Page_Validators[i].innerHTML;
             }
             if ($("#" + Prefix + "hdnActionFollowup").val() == "") {
                 $("#" + Prefix + "btnSaveAndNew").css("visibility", "visible");
                 $("#" + Prefix + "btnSave").val('<%= GetGlobalResourceObject("Resource", "btnSave") %>');
          }
          else {
              $("#" + Prefix + "btnSaveAndNew").css("visibility", "hidden");
                   $("#" + Prefix + "btnSave").val('<%= GetGlobalResourceObject("Resource", "btnUpdate") %>');
               }
           }


 
 

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
     </script>


</asp:Content>
