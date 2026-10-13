<%@ Page Title="Login" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="CustomerLogin.aspx.cs" Inherits="MultiGroupSystemsTester.Account.CustomerLogin" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h1>Log In</h1>
    <div class="row"><div class="col-md-5">
        <asp:Label ID="lblError" runat="server" CssClass="alert alert-danger d-block" Visible="false"></asp:Label>
        <div class="mb-3">
            <label class="form-label">Email Address</label>
            <asp:TextBox ID="txtEmailAddress" runat="server" CssClass="form-control"></asp:TextBox>
        </div>
        <div class="mb-3">
            <label class="form-label">Password</label>
            <asp:TextBox ID="txtPassword" runat="server" CssClass="form-control" TextMode="Password"></asp:TextBox>
        </div>
        <div class="mb-3 form-check">
            <asp:CheckBox ID="chkRememberMe" runat="server" CssClass="form-check-input" />
            <label class="form-check-label">Remember me</label>
        </div>
        <div class="mb-3">
            <div class="g-recaptcha" data-sitekey="<%= System.Configuration.ConfigurationManager.AppSettings["RecaptchaSiteKey"] %>"></div>
        </div>
        <asp:Button ID="btnLogin" runat="server" Text="Log In" CssClass="btn btn-navy" OnClick="btnLogin_Click" />
        <a href="ForgotPassword.aspx" class="ms-3">Forgot password?</a>
        <p class="mt-3"><a href="Register.aspx" runat="server">Don't have an account? Register</a></p>
    </div></div>

    
    <div class="modal fade" id="activationModal" tabindex="-1" aria-hidden="true">
      <div class="modal-dialog modal-dialog-centered">
        <div class="modal-content">
          <div class="modal-header" style="background-color:#0C2A44;">
            <h5 class="modal-title text-white">Account Not Activated</h5>
            <button type="button" class="btn-close btn-close-white" data-bs-dismiss="modal" aria-label="Close"></button>
          </div>
          <div class="modal-body">
            <p class="mb-0">We found an account with this email, but it hasn't been activated for online access yet. Set up a password to continue.</p>
          </div>
          <div class="modal-footer">
            <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Cancel</button>
            <a id="lnkActivateNow" href="#" class="btn" style="background-color:#0F6E56; color:#fff;">Activate Account</a>
          </div>
        </div>
      </div>
    </div>

    <script src="https://www.google.com/recaptcha/api.js" async defer></script>
    <script>
        window.addEventListener('load', function () {
            var emailInput = document.getElementById('<%= txtEmailAddress.ClientID %>');
            var modalEl = document.getElementById('activationModal');
            var activateLink = document.getElementById('lnkActivateNow');
            var bsModal = new bootstrap.Modal(modalEl);

            emailInput.addEventListener('blur', function () {
                var email = emailInput.value.trim();
                if (!email) return;

                fetch('CheckAccountStatus.ashx?email=' + encodeURIComponent(email))
                    .then(function (response) { return response.json(); })
                    .then(function (data) {
                        if (data.exists && !data.activated) {
                            activateLink.href = 'Activate.aspx?email=' + encodeURIComponent(email);
                            bsModal.show();
                        }
                    })
                    .catch(function (err) { console.error('Account status check failed:', err); });
            });
        });
    </script>
</asp:Content>
