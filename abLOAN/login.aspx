<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="login.aspx.cs" Inherits="abLOAN.login"  Async="true" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title><%= Resources.Resource.ProjectName %></title>
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <link href="https://cdnjs.cloudflare.com/ajax/libs/twitter-bootstrap/3.3.7/css/bootstrap.min.css" rel="stylesheet" type="text/css" id="lnkCss" />
    <link href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/4.7.0/css/font-awesome.min.css" rel="stylesheet" type="text/css" />
    <link href="https://cdnjs.cloudflare.com/ajax/libs/admin-lte/2.3.11/css/AdminLTE.min.css" rel="stylesheet" type="text/css" id="lnkAdminCss" />
    <link href="css/style.css" rel="stylesheet" type="text/css" />

    <script src="https://cdnjs.cloudflare.com/ajax/libs/jquery/2.2.4/jquery.min.js" type="text/javascript"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/twitter-bootstrap/3.3.7/js/bootstrap.min.js" type="text/javascript"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/jquery.blockUI/2.70/jquery.blockUI.min.js" type="text/javascript"></script>
    <script src="js/scripts.js" type="text/javascript"></script>
    <!-- HTML5 Shim and Respond.js IE8 support of HTML5 elements and media queries -->
    <!-- WARNING: Respond.js doesn't work if you view the page via file:// -->
    <!--[if lt IE 9]>
      <script src="https://oss.maxcdn.com/libs/html5shiv/3.7.0/html5shiv.js"></script>
      <script src="https://oss.maxcdn.com/libs/respond.js/1.3.0/respond.min.js"></script>
    <![endif]-->
</head>
<body class="hold-transition login-page" style="height: 84%;">
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <div id="divPageDirection" runat="server">
            <div class="login-box">
                <div class="login-logo">
                    <img src="img/arraybit-logo.png" height="45" alt="ArrayBit" />
                </div>
                <!-- /.login-logo -->
                <div class="login-box-body">
                    <h4 class="login-box-msg"><%= Resources.Resource.ProjectLogin %></h4>

                    <asp:UpdatePanel ID="upLogin" runat="server">
                        <ContentTemplate>
                            <asp:PlaceHolder ID="phLogin" runat="server">
                                <div class="form-group">
                                    <div class="input-group">
                                        <div class="input-group-addon">
                                            <i class="fa fa-language"></i>
                                        </div>
                                        <asp:HiddenField ID="hdnLang" runat="server" />
                                        <asp:DropDownList ID="ddlLanguage" runat="server" CssClass="form-control input-sm" OnSelectedIndexChanged="ddlLanguage_SelectedIndexChanged" AutoPostBack="true">
                                            <asp:ListItem Text="<%$ Resources:Resource, Arabic %>" Value="ar-sa" />
                                            <asp:ListItem Text="<%$ Resources:Resource, English %>" Value="en-us" />
                                        </asp:DropDownList>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <div class="input-group">
                                        <div class="input-group-addon">
                                            <i class="fa fa-user"></i>
                                        </div>
                                        <asp:TextBox ID="txtUsername" runat="server" class="form-control" placeholder="Username"
                                            MaxLength="50" required autofocus></asp:TextBox>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <div class="input-group">
                                        <div class="input-group-addon">
                                            <i class="fa fa-asterisk"></i>
                                        </div>
                                        <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" class="form-control"
                                            MaxLength="25" placeholder="Password" required></asp:TextBox>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-xs-12">
                                        <asp:Button ID="btnLogin" runat="server" Text="<%$Resources:Resource, btnLogin %>" class="btn btn-primary btn-flat btn-block"
                                            OnClick="btnLogin_Click" />
                                    </div>
                                </div>
                            </asp:PlaceHolder>
                            <asp:PlaceHolder ID="phVerify" runat="server" Visible="false">
                                <div class="form-group">
                                    <div class="input-group">
                                        <div class="input-group-addon">
                                            <i class="fa fa-lock"></i>
                                        </div>
                                        <asp:TextBox ID="txtVerificationCode" runat="server" class="form-control" placeholder="Verification Code"
                                            MaxLength="8" autofocus></asp:TextBox>
                                    </div>
                                    <div class="text-danger">
                                        <asp:RequiredFieldValidator ID="rfvVerificationCode" runat="server" ControlToValidate="txtVerificationCode" Display="Dynamic" SetFocusOnError="true"><%= Resources.Messages.InputRequired %> <%=Resources.Resource.Verification %></asp:RequiredFieldValidator>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-xs-12">
                                        <asp:Button ID="btnVerify" runat="server" Text="<%$Resources:Resource, btnVerify %>" class="btn btn-primary btn-flat btn-block"
                                            OnClick="btnVerify_Click" />
                                        <asp:Button ID="btnBack" runat="server" CausesValidation="false" Text="<%$Resources:Resource, btnBack %>" class="btn btn-default btn-flat btn-block"
                                            OnClick="btnBack_Click" />
                                    </div>
                                </div>
                            </asp:PlaceHolder>
                            <div class="row">
                                <div class="col-xs-12">
                                    <br />
                                    <asp:Panel ID="pnlMessage" runat="server" Visible="false" CssClass="alert alert-danger alert-dismissable">
                                        <button type="button" class="close" data-dismiss="alert" aria-hidden="true">
                                            &times;</button>
                                        <i class="fa fa-ban"></i>&nbsp;&nbsp;
                                        <asp:Label ID="lblMessage" runat="server"></asp:Label>
                                    </asp:Panel>
                                </div>
                                <!-- /.col -->
                            </div>
                        </ContentTemplate>
                        <Triggers>
                            <asp:PostBackTrigger ControlID="ddlLanguage" />
                        </Triggers>
                    </asp:UpdatePanel>

                </div>
                <!-- /.login-box-body -->
            </div>
            <!-- /.login-box -->
        </div>
    </form>
</body>
</html>
