<%@ Page Title="Staff Login" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="MultiGroupSystemsTester.Account.Login" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h1>Staff Login</h1>
    <div class="row"><div class="col-md-5">
        <asp:Label ID="lblError" runat="server" CssClass="alert alert-danger d-block" Visible="false"></asp:Label>
        <div class="mb-3">
            <label class="form-label">Username</label>
            <asp:TextBox ID="txtUsername" runat="server" CssClass="form-control"></asp:TextBox>
            <asp:RequiredFieldValidator runat="server" ControlToValidate="txtUsername" CssClass="text-danger" Display="Dynamic" ErrorMessage="Required." />
        </div>
        <div class="mb-3">
            <label class="form-label">Password</label>
            <asp:TextBox ID="txtPassword" runat="server" CssClass="form-control" TextMode="Password"></asp:TextBox>
            <asp:RequiredFieldValidator runat="server" ControlToValidate="txtPassword" CssClass="text-danger" Display="Dynamic" ErrorMessage="Required." />
        </div>
        <asp:Button ID="btnLogin" runat="server" Text="Log In" CssClass="btn btn-navy" OnClick="btnLogin_Click" />
    </div></div>
</asp:Content>