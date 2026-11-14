<%@ Page Title="Staff Login" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="MultiGroupSystemsTester.Account.Login" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h1>Staff Login</h1>
    <div class="row"><div class="col-md-5">
        <asp:Label ID="lblError" runat="server" CssClass="alert alert-danger d-block" Visible="false"></asp:Label>
        <asp:Label ID="lblInfo" runat="server" CssClass="alert alert-info d-block" Visible="false"></asp:Label>

        <%-- STEP 1: username + password --%>
        <asp:Panel ID="pnlCredentials" runat="server">
            <div class="mb-3">
                <label class="form-label">Username</label>
                <asp:TextBox ID="txtUsername" runat="server" CssClass="form-control" ValidationGroup="Creds"></asp:TextBox>
                <asp:RequiredFieldValidator runat="server" ControlToValidate="txtUsername" CssClass="text-danger" Display="Dynamic" ErrorMessage="Required." ValidationGroup="Creds" />
            </div>
            <div class="mb-3">
                <label class="form-label">Password</label>
                <asp:TextBox ID="txtPassword" runat="server" CssClass="form-control" TextMode="Password" ValidationGroup="Creds"></asp:TextBox>
                <asp:RequiredFieldValidator runat="server" ControlToValidate="txtPassword" CssClass="text-danger" Display="Dynamic" ErrorMessage="Required." ValidationGroup="Creds" />
            </div>
            <asp:Button ID="btnLogin" runat="server" Text="Log In" CssClass="btn btn-navy" OnClick="btnLogin_Click" ValidationGroup="Creds" />
            <p class="text-muted small mt-3 mb-0">Forgot your password? Please contact a manager to have it reset.</p>
        </asp:Panel>

        <%-- STEP 2: SMS verification code (second factor) --%>
        <asp:Panel ID="pnlOtp" runat="server" Visible="false">
            <p class="mb-3">Enter the 6-digit code we sent to the mobile number ending in <strong><asp:Label ID="lblLastDigits" runat="server" /></strong>.</p>
            <div class="mb-3">
                <label class="form-label">Verification code</label>
                <asp:TextBox ID="txtOtp" runat="server" CssClass="form-control" MaxLength="6" autocomplete="one-time-code" inputmode="numeric" ValidationGroup="Otp"></asp:TextBox>
                <asp:RequiredFieldValidator runat="server" ControlToValidate="txtOtp" CssClass="text-danger" Display="Dynamic" ErrorMessage="Required." ValidationGroup="Otp" />
                <asp:RegularExpressionValidator runat="server" ControlToValidate="txtOtp" ValidationExpression="^\d{6}$" CssClass="text-danger" Display="Dynamic" ErrorMessage="Enter the 6-digit code." ValidationGroup="Otp" />
            </div>
            <asp:Button ID="btnVerify" runat="server" Text="Verify" CssClass="btn btn-navy" OnClick="btnVerify_Click" ValidationGroup="Otp" />
            <asp:Button ID="btnResend" runat="server" Text="Resend code" CssClass="btn btn-outline-secondary" OnClick="btnResend_Click" CausesValidation="false" />
            <asp:Button ID="btnCancel" runat="server" Text="Start over" CssClass="btn btn-link" OnClick="btnCancel_Click" CausesValidation="false" />
            <p class="text-muted small mt-3 mb-0">The code expires after 5 minutes. Never share it with anyone.</p>
        </asp:Panel>
    </div></div>
<script>
    (function () {
        console.log("[staff-check] script loaded");

        var user = document.getElementById("<%= txtUsername.ClientID %>");
        var pass = document.getElementById("<%= txtPassword.ClientID %>");
        console.log("[staff-check] username box:", user, "password box:", pass);
        if (!user || !pass) return;

        var checkUrl = "<%= ResolveUrl("~/Account/StaffStatus.ashx") %>";
        var activateUrl = "<%= ResolveUrl("~/Account/StaffActivate.aspx") %>";
        console.log("[staff-check] checkUrl:", checkUrl);

        var notice = document.createElement("div");
        notice.className = "alert alert-warning mt-2";
        notice.setAttribute("role", "alert");
        notice.style.display = "none";
        user.parentNode.appendChild(notice);

        var lastChecked = "";

        function showNotice(name) {
            notice.textContent = "";
            notice.appendChild(document.createTextNode(
                "This staff account hasn't been activated yet. You need to create your password first. "));

            var link = document.createElement("a");
            link.href = activateUrl + "?username=" + encodeURIComponent(name);
            link.textContent = "Activate your staff account";
            notice.appendChild(link);

            notice.style.display = "block";
            pass.disabled = true;
        }

        function hideNotice() {
            notice.style.display = "none";
            pass.disabled = false;
        }

        function check() {
            var name = user.value.trim();
            console.log("[staff-check] checking:", name);

            if (name === "") { lastChecked = ""; hideNotice(); return; }
            if (name === lastChecked) { console.log("[staff-check] already checked"); return; }
            lastChecked = name;

            fetch(checkUrl, {
                method: "POST",
                credentials: "same-origin",
                headers: { "Content-Type": "application/x-www-form-urlencoded" },
                body: "username=" + encodeURIComponent(name)
            })
                .then(function (r) {
                    console.log("[staff-check] response status:", r.status);
                    if (!r.ok) throw new Error("HTTP " + r.status);
                    return r.json();
                })
                .then(function (data) {
                    console.log("[staff-check] response data:", data);
                    if (user.value.trim() !== name) return;
                    if (data.needsActivation) showNotice(name); else hideNotice();
                })
                .catch(function (err) {
                    console.log("[staff-check] FAILED:", err);
                    hideNotice();
                });
        }

        user.addEventListener("blur", check);
        pass.addEventListener("focus", check);
        user.addEventListener("input", function () { lastChecked = ""; hideNotice(); });

        if (user.value.trim() !== "") check();
    })();
</script>
</asp:Content>
