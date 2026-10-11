<%@ Page Title="Activate Your Account" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Activate.aspx.cs" Inherits="MultiGroupSystemsTester.Account.Activate" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container my-5" style="max-width: 480px;">
        <div class="card shadow-sm border-0">
            <div class="card-body p-4" style="border-top: 4px solid #0F6E56;">
                <h3 class="mb-3" style="color:#0C2A44;">Activate Your Account</h3>
                <p class="text-muted mb-4">
                    If you're an existing Multigroup customer, enter your email address below to set up online access.
                </p>

                <!-- Step 1: Find account -->
                <asp:Panel ID="pnlFindAccount" runat="server">
                    <div class="mb-3">
                        <label for="txtEmail" class="form-label">Email Address</label>
                        <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" TextMode="Email" placeholder="you@example.com" />
                    </div>
                    <asp:Label ID="lblFindError" runat="server" CssClass="text-danger d-block mb-3" />
                    <asp:Button ID="btnFindAccount" runat="server" Text="Find My Account" CssClass="btn w-100" 
                        Style="background-color:#0F6E56; color:#fff;" OnClick="btnFindAccount_Click" />
                </asp:Panel>

                <!-- Step 2: Set password -->
                <asp:Panel ID="pnlSetPassword" runat="server" Visible="false">
                    <asp:HiddenField ID="hdnEmail" runat="server" />
                    <p class="mb-3">Account found. Create a password to finish activating your account.</p>

                    <div class="mb-3">
                        <label for="txtPassword" class="form-label">New Password</label>
                        <asp:TextBox ID="txtPassword" runat="server" CssClass="form-control" TextMode="Password" />
                    </div>
                    <div class="mb-3">
                        <label for="txtConfirmPassword" class="form-label">Confirm Password</label>
                        <asp:TextBox ID="txtConfirmPassword" runat="server" CssClass="form-control" TextMode="Password" />
                    </div>
                    <asp:Label ID="lblPasswordError" runat="server" CssClass="text-danger d-block mb-3" />
                    <asp:Button ID="btnSetPassword" runat="server" Text="Activate Account" CssClass="btn w-100" 
                        Style="background-color:#0C2A44; color:#fff;" OnClick="btnSetPassword_Click" />
                </asp:Panel>

                <div class="text-center mt-3">
                    <small>Already activated? <a href="~/Account/CustomerLogin.aspx" runat="server">Log in</a></small>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
