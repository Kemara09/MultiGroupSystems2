<%@ Page Title="Activate Staff Account" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="StaffActivate.aspx.cs" Inherits="MultiGroupSystemsTester.Account.StaffActivate" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container my-5" style="max-width: 480px;">
        <div class="card shadow-sm border-0">
            <div class="card-body p-4" style="border-top: 4px solid #0F6E56;">
                <h3 class="mb-3" style="color:#0C2A44;">Activate Your Staff Account</h3>
                <p class="text-muted mb-4">
                    Enter the username your manager registered for you to set up your login.
                    We'll text a verification code to the mobile number on file for you.
                </p>

                <!-- Step 1: Find account -->
                <asp:Panel ID="pnlFindAccount" runat="server">
                    <div class="mb-3">
                        <label for="txtUsername" class="form-label">Username</label>
                        <asp:TextBox ID="txtUsername" runat="server" CssClass="form-control"
                            autocomplete="username" placeholder="Your staff username" />
                    </div>
                    <asp:Label ID="lblFindError" runat="server" CssClass="text-danger d-block mb-3" />
                    <asp:Button ID="btnFindAccount" runat="server" Text="Send Verification Code" CssClass="btn w-100"
                        Style="background-color:#0F6E56; color:#fff;" OnClick="btnFindAccount_Click" />
                </asp:Panel>

                <!-- Step 2: Enter the SMS code -->
                <asp:Panel ID="pnlVerifyCode" runat="server" Visible="false">
                    <p class="mb-3">
                        We sent a 6-digit code by SMS to your phone ending in
                        <strong><asp:Label ID="lblLastDigits" runat="server" /></strong>. It expires in 5 minutes.
                    </p>
                    <div class="mb-3">
                        <label for="txtCode" class="form-label">Verification code</label>
                        <asp:TextBox ID="txtCode" runat="server" CssClass="form-control" MaxLength="6"
                            inputmode="numeric" autocomplete="one-time-code" placeholder="123456" />
                    </div>
                    <asp:Label ID="lblCodeInfo" runat="server" CssClass="text-success d-block mb-3" />
                    <asp:Label ID="lblCodeError" runat="server" CssClass="text-danger d-block mb-3" />
                    <asp:Button ID="btnVerifyCode" runat="server" Text="Verify" CssClass="btn w-100"
                        Style="background-color:#0C2A44; color:#fff;" OnClick="btnVerifyCode_Click" />
                    <div class="text-center mt-3">
                        <asp:LinkButton ID="btnResendCode" runat="server" CausesValidation="false" OnClick="btnResendCode_Click">Send a new code</asp:LinkButton>
                        <span class="text-muted mx-2">|</span>
                        <a href="StaffActivate.aspx">Start over</a>
                    </div>
                </asp:Panel>

                <!-- Step 3: Set password (only reachable after the code is verified) -->
                <asp:Panel ID="pnlSetPassword" runat="server" Visible="false">
                    <p class="mb-3">Your phone is verified. Create a password to finish activating your account.</p>

                    <div class="mb-3">
                        <label for="txtPassword" class="form-label">New Password</label>
                        <asp:TextBox ID="txtPassword" runat="server" CssClass="form-control" TextMode="Password" data-pw-checklist="pwChecklist" />
                        <ul id="pwChecklist" class="list-unstyled small mb-0 mt-2">
                            <li data-rule="length">At least 8 characters</li>
                            <li data-rule="upper">An uppercase letter (A-Z)</li>
                            <li data-rule="lower">A lowercase letter (a-z)</li>
                            <li data-rule="number">A number (0-9)</li>
                            <li data-rule="special">A special character (for example ! @ # $ %)</li>
                        </ul>
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
                    <small>Already activated? <a href="~/Account/Login.aspx" runat="server">Staff log in</a></small>
                </div>
            </div>
        </div>
    </div>

    <script src="<%= ResolveUrl("~/Scripts/password-rules.js") %>"></script>
</asp:Content>