<%@ Page Title="Forgot Password" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ForgotPassword.aspx.cs" Inherits="MultiGroupSystemsTester.Account.ForgotPassword" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h1>Forgot Password</h1>
    <div class="row"><div class="col-md-5">

        <asp:Panel ID="pnlForm" runat="server">
            <p class="text-muted">Enter the email address on your account and we'll email you a link to choose a new password.</p>
            <div class="mb-3">
                <label class="form-label">Email Address</label>
                <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" TextMode="Email"></asp:TextBox>
            </div>
            <asp:Label ID="lblError" runat="server" CssClass="text-danger d-block mb-3" />
            <asp:Button ID="btnSend" runat="server" Text="Send Reset Link" CssClass="btn btn-navy" OnClick="btnSend_Click" />
            <p class="mt-3"><a href="CustomerLogin.aspx">Back to log in</a></p>
        </asp:Panel>

        <asp:Panel ID="pnlSent" runat="server" Visible="false">
            <div class="alert alert-success">
                If an account exists for that email address, we've sent a link to reset your password.
                It expires in 30 minutes. If you can't see it, check your spam folder.
            </div>
            <p><a href="CustomerLogin.aspx">Back to log in</a></p>
        </asp:Panel>

    </div></div>
</asp:Content>
