<%@ Page Title="Reset Password" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ResetPassword.aspx.cs" Inherits="MultiGroupSystemsTester.Account.ResetPassword" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h1>Reset Password</h1>
    <div class="row"><div class="col-md-5">

        <asp:Panel ID="pnlInvalid" runat="server" Visible="false">
            <div class="alert alert-danger">
                This reset link has expired or has already been used.
            </div>
            <p><a href="ForgotPassword.aspx">Request a new reset link</a></p>
        </asp:Panel>

        <asp:Panel ID="pnlForm" runat="server" Visible="false">
            <p class="text-muted">Choose a new password for your account.</p>
            <div class="mb-3">
                <label class="form-label">New Password</label>
                <asp:TextBox ID="txtPassword" runat="server" CssClass="form-control" TextMode="Password" data-pw-checklist="pwChecklist"></asp:TextBox>
                <ul id="pwChecklist" class="list-unstyled small mb-0 mt-2">
                    <li data-rule="length">At least 8 characters</li>
                    <li data-rule="upper">An uppercase letter (A-Z)</li>
                    <li data-rule="lower">A lowercase letter (a-z)</li>
                    <li data-rule="number">A number (0-9)</li>
                    <li data-rule="special">A special character (for example ! @ # $ %)</li>
                </ul>
            </div>
            <div class="mb-3">
                <label class="form-label">Confirm Password</label>
                <asp:TextBox ID="txtConfirmPassword" runat="server" CssClass="form-control" TextMode="Password"></asp:TextBox>
            </div>
            <asp:Label ID="lblError" runat="server" CssClass="text-danger d-block mb-3" />
            <asp:Button ID="btnReset" runat="server" Text="Reset Password" CssClass="btn btn-navy" OnClick="btnReset_Click" />
        </asp:Panel>

        <asp:Panel ID="pnlDone" runat="server" Visible="false">
            <div class="alert alert-success">Your password has been reset.</div>
            <a href="CustomerLogin.aspx" class="btn btn-teal">Log in</a>
        </asp:Panel>

    </div></div>

    <script src="<%= ResolveUrl("~/Scripts/password-rules.js") %>"></script>
</asp:Content>
