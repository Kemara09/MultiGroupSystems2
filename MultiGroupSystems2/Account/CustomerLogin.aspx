<%@ Page Title="Login" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="CustomerLogin.aspx.cs" Inherits="MultiGroupSystemsTester.Account.CustomerLogin" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h1>Log In</h1>
    <div class="row"><div class="col-md-5">
        <asp:Label ID="lblError" runat="server" CssClass="alert alert-danger d-block" Visible="false"></asp:Label>
        <div class="mb-3">
            <label class="form-label">Username</label>
            <asp:TextBox ID="txtUsername" runat="server" CssClass="form-control"></asp:TextBox>
        </div>
        <div class="mb-3">
            <label class="form-label">Password</label>
            <asp:TextBox ID="txtPassword" runat="server" CssClass="form-control" TextMode="Password"></asp:TextBox>
        </div>
        <asp:Button ID="btnLogin" runat="server" Text="Log In" CssClass="btn btn-navy" OnClick="btnLogin_Click" />
        <p class="mt-3"><a href="Register.aspx" runat="server">Don't have an account? Register</a></p>
    </div></div>
</asp:Content>