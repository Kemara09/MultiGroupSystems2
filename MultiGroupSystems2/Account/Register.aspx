<%@ Page Title="Register" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Register.aspx.cs" Inherits="MultiGroupSystemsTester.Account.Register" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h1>Create an Account</h1>
    <asp:Label ID="lblError" runat="server" CssClass="alert alert-danger d-block" Visible="false"></asp:Label>

    <!-- Step 1: your details -->
    <asp:Panel ID="pnlForm" runat="server">
        <div class="row g-3">
            <div class="col-md-6">
                <label class="form-label">First name</label>
                <asp:TextBox ID="txtFirstName" runat="server" CssClass="form-control" />
                <asp:RequiredFieldValidator runat="server" ControlToValidate="txtFirstName" CssClass="text-danger" Display="Dynamic" ErrorMessage="Required." />
            </div>
            <div class="col-md-6">
                <label class="form-label">Last name</label>
                <asp:TextBox ID="txtLastName" runat="server" CssClass="form-control" />
                <asp:RequiredFieldValidator runat="server" ControlToValidate="txtLastName" CssClass="text-danger" Display="Dynamic" ErrorMessage="Required." />
            </div>
            <div class="col-md-6">
                <label class="form-label">Email</label>
                <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" />
                <div class="form-text">This is what you'll use to log in.</div>
                <asp:RequiredFieldValidator runat="server" ControlToValidate="txtEmail" CssClass="text-danger" Display="Dynamic" ErrorMessage="Required." />
                <asp:RegularExpressionValidator runat="server" ControlToValidate="txtEmail" CssClass="text-danger" Display="Dynamic" ErrorMessage="Invalid email." ValidationExpression="^[^@\s]+@[^@\s]+\.[^@\s]+$" />
            </div>
            <div class="col-md-6">
                <label class="form-label">Mobile number</label>
                <asp:TextBox ID="txtPhone" runat="server" CssClass="form-control" />
                <div class="form-text">We'll text a verification code to this number.</div>
                <asp:RequiredFieldValidator runat="server" ControlToValidate="txtPhone" CssClass="text-danger" Display="Dynamic" ErrorMessage="Required." />
                <asp:RegularExpressionValidator runat="server" ControlToValidate="txtPhone" CssClass="text-danger" Display="Dynamic" ErrorMessage="Enter a valid 10-digit mobile number (e.g. 0821234567)." ValidationExpression="^0[6-8][0-9]{8}$" />
            </div>
            <div class="col-md-3">
                <label class="form-label">Street number</label>
                <asp:TextBox ID="txtStreetNumber" runat="server" CssClass="form-control" />
                <asp:RequiredFieldValidator runat="server" ControlToValidate="txtStreetNumber" CssClass="text-danger" Display="Dynamic" ErrorMessage="Required." />
                <asp:RegularExpressionValidator runat="server" ControlToValidate="txtStreetNumber" CssClass="text-danger" Display="Dynamic" ErrorMessage="Digits only." ValidationExpression="^[0-9]+$" />
            </div>
            <div class="col-md-9">
                <label class="form-label">Street name</label>
                <asp:TextBox ID="txtStreetName" runat="server" CssClass="form-control" />
            </div>
            <div class="col-md-4">
                <label class="form-label">Suburb</label>
                <asp:TextBox ID="txtSuburb" runat="server" CssClass="form-control" />
            </div>
            <div class="col-md-4">
                <label class="form-label">City</label>
                <asp:TextBox ID="txtCity" runat="server" CssClass="form-control" />
            </div>
            <div class="col-md-4">
                <label class="form-label">Province</label>
                <asp:DropDownList ID="ddlProvince" runat="server" CssClass="form-select">
                    <asp:ListItem Text="-- Select --" Value="" />
                    <asp:ListItem Text="Eastern Cape" Value="Eastern Cape" />
                    <asp:ListItem Text="Free State" Value="Free State" />
                    <asp:ListItem Text="Gauteng" Value="Gauteng" />
                    <asp:ListItem Text="KwaZulu-Natal" Value="KwaZulu-Natal" />
                    <asp:ListItem Text="Limpopo" Value="Limpopo" />
                    <asp:ListItem Text="Mpumalanga" Value="Mpumalanga" />
                    <asp:ListItem Text="North West" Value="North West" />
                    <asp:ListItem Text="Northern Cape" Value="Northern Cape" />
                    <asp:ListItem Text="Western Cape" Value="Western Cape" />
                </asp:DropDownList>
                <asp:RequiredFieldValidator runat="server" ControlToValidate="ddlProvince" InitialValue="" CssClass="text-danger" Display="Dynamic" ErrorMessage="Please select a province." />
            </div>
            <div class="col-md-4">
                <label class="form-label">Postal code</label>
                <asp:TextBox ID="txtPostalCode" runat="server" CssClass="form-control" />
                <asp:RequiredFieldValidator runat="server" ControlToValidate="txtPostalCode" CssClass="text-danger" Display="Dynamic" ErrorMessage="Required." />
                <asp:RegularExpressionValidator runat="server" ControlToValidate="txtPostalCode" CssClass="text-danger" Display="Dynamic" ErrorMessage="Enter a valid 4-digit postal code." ValidationExpression="^[0-9]{4}$" />
            </div>
            <div class="col-md-6">
                <label class="form-label">Choose a password</label>
                <asp:TextBox ID="txtPassword" runat="server" CssClass="form-control" TextMode="Password" data-pw-checklist="pwChecklist" />
                <ul id="pwChecklist" class="list-unstyled small mb-0 mt-2">
                    <li data-rule="length">At least 8 characters</li>
                    <li data-rule="upper">An uppercase letter (A-Z)</li>
                    <li data-rule="lower">A lowercase letter (a-z)</li>
                    <li data-rule="number">A number (0-9)</li>
                    <li data-rule="special">A special character (for example ! @ # $ %)</li>
                </ul>
                <asp:RequiredFieldValidator runat="server" ControlToValidate="txtPassword" CssClass="text-danger" Display="Dynamic" ErrorMessage="Required." />
            </div>
            <div class="col-md-6">
                <label class="form-label">Confirm password</label>
                <asp:TextBox ID="txtConfirmPassword" runat="server" CssClass="form-control" TextMode="Password" />
                <asp:RequiredFieldValidator runat="server" ControlToValidate="txtConfirmPassword" CssClass="text-danger" Display="Dynamic" ErrorMessage="Required." />
            </div>
        </div>
        <asp:Button ID="btnRegister" runat="server" Text="Send verification code" CssClass="btn btn-teal mt-3" OnClick="btnRegister_Click" />
    </asp:Panel>

    <!-- Step 2: verify the phone number, then the account is created -->
    <asp:Panel ID="pnlVerify" runat="server" Visible="false">
        <div class="row"><div class="col-md-5">
            <h4>Verify your phone number</h4>
            <p class="text-muted">
                We sent a 6-digit code by SMS to your phone ending in <strong><asp:Label ID="lblLastDigits" runat="server" /></strong>.
                Enter it below to create your account. It expires in 5 minutes.
            </p>

            <asp:Label ID="lblCodeInfo" runat="server" CssClass="alert alert-success d-block" Visible="false"></asp:Label>
            <asp:Label ID="lblCodeError" runat="server" CssClass="alert alert-danger d-block" Visible="false"></asp:Label>

            <div class="mb-3">
                <label class="form-label">Verification code</label>
                <asp:TextBox ID="txtCode" runat="server" CssClass="form-control" MaxLength="6"
                    inputmode="numeric" autocomplete="one-time-code" placeholder="123456" />
            </div>

            <asp:Button ID="btnVerifyCode" runat="server" Text="Verify and create account" CssClass="btn btn-teal" CausesValidation="false" OnClick="btnVerifyCode_Click" />
            <asp:LinkButton ID="btnResendCode" runat="server" CssClass="ms-3" CausesValidation="false" OnClick="btnResendCode_Click">Send a new code</asp:LinkButton>

            <p class="mt-3">
                <asp:LinkButton ID="btnBack" runat="server" CausesValidation="false" OnClick="btnBack_Click">Change my details</asp:LinkButton>
            </p>
        </div></div>
    </asp:Panel>

    <script src="<%= ResolveUrl("~/Scripts/password-rules.js") %>"></script>
</asp:Content>
