
<%@ Page Title="Register" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Register.aspx.cs" Inherits="MultiGroupSystemsTester.Account.Register" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

<style>
    .register-page {
        --reg-bg: #f1efe8;
        --reg-card: #ffffff;
        --reg-text: #172b3a;
        --reg-muted: #64748b;
        --reg-border: #d7dfe7;
        --reg-input: #ffffff;
        --reg-accent: #0f6e56;
        --reg-soft: #f3f7f6;
        --reg-shadow: 0 12px 32px rgba(0,0,0,.08);
        background: var(--reg-bg);
        color: var(--reg-text);
        padding: 38px 16px 65px;
        min-height: 80vh;
    }

    [data-theme="dark"] .register-page {
        --reg-bg: #151b24;
        --reg-card: #202936;
        --reg-text: #f1f5f9;
        --reg-muted: #b7c2d0;
        --reg-border: #405064;
        --reg-input: #263444;
        --reg-accent: #20b89a;
        --reg-soft: #253443;
        --reg-shadow: 0 12px 32px rgba(0,0,0,.25);
    }

    .register-container {
        max-width: 1000px;
        margin: 0 auto;
    }

    .register-header {
        text-align: center;
        margin-bottom: 28px;
    }

    .register-header h1 {
        color: var(--reg-text);
        font-size: 30px;
        font-weight: 750;
        margin-bottom: 8px;
    }

    .register-header p {
        color: var(--reg-muted);
        font-size: 14px;
        margin: 0;
    }

    .register-card {
        background: var(--reg-card);
        border: 1px solid var(--reg-border);
        border-radius: 16px;
        box-shadow: var(--reg-shadow);
        padding: 30px;
    }

    .register-section {
        margin-bottom: 30px;
    }

    .register-section:last-child {
        margin-bottom: 0;
    }

    .register-section-title {
        color: var(--reg-text);
        font-size: 16px;
        font-weight: 700;
        padding-bottom: 12px;
        margin-bottom: 20px;
        border-bottom: 1px solid var(--reg-border);
    }

    .register-page .form-label {
        color: var(--reg-text);
        font-size: 13px;
        font-weight: 600;
        margin-bottom: 7px;
    }

    .register-page .form-control,
    .register-page .form-select {
        width: 100%;
        min-height: 44px;
        color: var(--reg-text);
        background: var(--reg-input);
        border: 1px solid var(--reg-border);
        border-radius: 8px;
        padding: 10px 12px;
        font-size: 14px;
    }

    .register-page .form-control:focus,
    .register-page .form-select:focus {
        color: var(--reg-text);
        background: var(--reg-input);
        border-color: var(--reg-accent);
        box-shadow: 0 0 0 3px rgba(15,110,86,.15);
    }

    .register-page .form-control::placeholder {
        color: var(--reg-muted);
        opacity: .75;
    }

    .register-page .form-text,
    .register-page .text-muted {
        color: var(--reg-muted) !important;
        font-size: 12px;
        line-height: 1.5;
    }

    .register-page .text-danger {
        color: #e45b65 !important;
        font-size: 12px;
        display: block;
        margin-top: 5px;
    }

    .register-page .field-group {
        margin-bottom: 4px;
    }

    .register-page .password-rules {
        margin-top: 12px;
        padding: 14px 16px;
        background: var(--reg-soft);
        border: 1px solid var(--reg-border);
        border-radius: 10px;
    }

    .register-page .password-rules-title {
        color: var(--reg-text);
        font-size: 12px;
        font-weight: 700;
        margin-bottom: 9px;
    }

    .register-page #pwChecklist {
        margin: 0;
        padding: 0;
        list-style: none;
    }

    .register-page #pwChecklist li {
        color: var(--reg-muted);
        font-size: 12px;
        padding: 3px 0 3px 22px;
        position: relative;
        line-height: 1.5;
    }

    .register-page #pwChecklist li::before {
        content: "○";
        position: absolute;
        left: 2px;
        color: var(--reg-muted);
        font-weight: 700;
    }

    .register-page #pwChecklist li.valid,
    .register-page #pwChecklist li.met,
    .register-page #pwChecklist li.passed {
        color: #16a085;
    }

    .register-page #pwChecklist li.valid::before,
    .register-page #pwChecklist li.met::before,
    .register-page #pwChecklist li.passed::before {
        content: "✓";
        color: #16a085;
    }

    .register-page .register-actions {
        border-top: 1px solid var(--reg-border);
        padding-top: 22px;
        margin-top: 12px;
    }

    .register-page .register-submit {
        background: #0f6e56;
        border: 1px solid #0f6e56;
        color: #ffffff;
        border-radius: 9px;
        padding: 12px 28px;
        font-size: 14px;
        font-weight: 700;
        min-height: 46px;
        cursor: pointer;
    }

    .register-page .register-submit:hover {
        background: #0b5744;
        color: #ffffff;
    }

    .register-page .register-footer {
        margin-top: 15px;
        color: var(--reg-muted);
        font-size: 13px;
    }

    .register-page .register-footer a,
    .register-page .register-link {
        color: var(--reg-accent);
        font-weight: 600;
    }

    .register-page .verification-card {
        max-width: 560px;
        margin: 0 auto;
    }

    .register-page .verification-heading {
        color: var(--reg-text);
        font-size: 22px;
        font-weight: 700;
        margin-bottom: 12px;
    }

    .register-page .verification-description {
        color: var(--reg-muted);
        font-size: 14px;
        line-height: 1.7;
        margin-bottom: 22px;
    }

    .register-page .verification-code {
        font-size: 21px;
        font-weight: 700;
        letter-spacing: 6px;
        text-align: center;
        max-width: 260px;
    }

    .register-page .verification-actions {
        display: flex;
        flex-wrap: wrap;
        align-items: center;
        gap: 15px;
        margin-top: 20px;
    }

    .register-page .alert {
        border-radius: 9px;
        font-size: 13px;
        margin-bottom: 20px;
    }

    @media (max-width: 767px) {
        .register-page {
            padding: 22px 10px 40px;
        }

        .register-card {
            padding: 20px 16px;
        }

        .register-header h1 {
            font-size: 25px;
        }

        .register-page .register-submit {
            width: 100%;
        }

        .register-page .verification-actions {
            align-items: stretch;
            flex-direction: column;
        }

        .register-page .verification-actions .register-submit {
            width: 100%;
        }
    }
</style>

<div class="register-page">
    <div class="register-container">

        <div class="register-header">
            <h1>Create an Account</h1>
            <p>Join Multigroup Inc. to shop, manage orders and track deliveries.</p>
        </div>

        <asp:Label
            ID="lblError"
            runat="server"
            CssClass="alert alert-danger d-block"
            Visible="false" />

        <!-- STEP 1: REGISTRATION DETAILS -->
        <asp:Panel ID="pnlForm" runat="server">

            <div class="register-card">

                <!-- PERSONAL INFORMATION -->
                <div class="register-section">

                    <div class="register-section-title">
                        Personal Information
                    </div>

                    <div class="row g-3">

                        <div class="col-md-6 field-group">
                            <label class="form-label">First name</label>
                            <asp:TextBox
                                ID="txtFirstName"
                                runat="server"
                                CssClass="form-control"
                                placeholder="Enter your first name" />
                            <asp:RequiredFieldValidator
                                runat="server"
                                ControlToValidate="txtFirstName"
                                CssClass="text-danger"
                                Display="Dynamic"
                                ErrorMessage="Required." />
                        </div>

                        <div class="col-md-6 field-group">
                            <label class="form-label">Last name</label>
                            <asp:TextBox
                                ID="txtLastName"
                                runat="server"
                                CssClass="form-control"
                                placeholder="Enter your last name" />
                            <asp:RequiredFieldValidator
                                runat="server"
                                ControlToValidate="txtLastName"
                                CssClass="text-danger"
                                Display="Dynamic"
                                ErrorMessage="Required." />
                        </div>

                        <div class="col-md-6 field-group">
                            <label class="form-label">Email address</label>
                            <asp:TextBox
                                ID="txtEmail"
                                runat="server"
                                CssClass="form-control"
                                TextMode="Email"
                                placeholder="name@example.com" />
                            <div class="form-text">
                                This is what you'll use to log in.
                            </div>
                            <asp:RequiredFieldValidator
                                runat="server"
                                ControlToValidate="txtEmail"
                                CssClass="text-danger"
                                Display="Dynamic"
                                ErrorMessage="Required." />
                            <asp:RegularExpressionValidator
                                runat="server"
                                ControlToValidate="txtEmail"
                                CssClass="text-danger"
                                Display="Dynamic"
                                ErrorMessage="Invalid email."
                                ValidationExpression="^[^@\s]+@[^@\s]+\.[^@\s]+$" />
                        </div>

                        <div class="col-md-6 field-group">
                            <label class="form-label">Mobile number</label>
                            <asp:TextBox
                                ID="txtPhone"
                                runat="server"
                                CssClass="form-control"
                                MaxLength="10"
                                placeholder="0821234567" />
                            <div class="form-text">
                                We'll text a verification code to this number.
                            </div>
                            <asp:RequiredFieldValidator
                                runat="server"
                                ControlToValidate="txtPhone"
                                CssClass="text-danger"
                                Display="Dynamic"
                                ErrorMessage="Required." />
                            <asp:RegularExpressionValidator
                                runat="server"
                                ControlToValidate="txtPhone"
                                CssClass="text-danger"
                                Display="Dynamic"
                                ErrorMessage="Enter a valid 10-digit mobile number (e.g. 0821234567)."
                                ValidationExpression="^0[6-8][0-9]{8}$" />
                        </div>

                    </div>
                </div>

                <!-- ADDRESS INFORMATION -->
                <div class="register-section">

                    <div class="register-section-title">
                        Address Information
                    </div>

                    <div class="row g-3">

                        <div class="col-md-3 field-group">
                            <label class="form-label">Street number</label>
                            <asp:TextBox
                                ID="txtStreetNumber"
                                runat="server"
                                CssClass="form-control"
                                placeholder="12" />
                            <asp:RequiredFieldValidator
                                runat="server"
                                ControlToValidate="txtStreetNumber"
                                CssClass="text-danger"
                                Display="Dynamic"
                                ErrorMessage="Required." />
                            <asp:RegularExpressionValidator
                                runat="server"
                                ControlToValidate="txtStreetNumber"
                                CssClass="text-danger"
                                Display="Dynamic"
                                ErrorMessage="Digits only."
                                ValidationExpression="^[0-9]+$" />
                        </div>

                        <div class="col-md-9 field-group">
                            <label class="form-label">Street name</label>
                            <asp:TextBox
                                ID="txtStreetName"
                                runat="server"
                                CssClass="form-control"
                                placeholder="Enter your street name" />
                        </div>

                        <div class="col-md-6 field-group">
                            <label class="form-label">Suburb</label>
                            <asp:TextBox
                                ID="txtSuburb"
                                runat="server"
                                CssClass="form-control"
                                placeholder="Enter your suburb" />
                        </div>

                        <div class="col-md-6 field-group">
                            <label class="form-label">City</label>
                            <asp:TextBox
                                ID="txtCity"
                                runat="server"
                                CssClass="form-control"
                                placeholder="Enter your city" />
                        </div>

                        <div class="col-md-6 field-group">
                            <label class="form-label">Province</label>
                            <asp:DropDownList
                                ID="ddlProvince"
                                runat="server"
                                CssClass="form-select">
                                <asp:ListItem Text="-- Select province --" Value="" />
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
                            <asp:RequiredFieldValidator
                                runat="server"
                                ControlToValidate="ddlProvince"
                                InitialValue=""
                                CssClass="text-danger"
                                Display="Dynamic"
                                ErrorMessage="Please select a province." />
                        </div>

                        <div class="col-md-6 field-group">
                            <label class="form-label">Postal code</label>
                            <asp:TextBox
                                ID="txtPostalCode"
                                runat="server"
                                CssClass="form-control"
                                MaxLength="4"
                                placeholder="4001" />
                            <asp:RequiredFieldValidator
                                runat="server"
                                ControlToValidate="txtPostalCode"
                                CssClass="text-danger"
                                Display="Dynamic"
                                ErrorMessage="Required." />
                            <asp:RegularExpressionValidator
                                runat="server"
                                ControlToValidate="txtPostalCode"
                                CssClass="text-danger"
                                Display="Dynamic"
                                ErrorMessage="Enter a valid 4-digit postal code."
                                ValidationExpression="^[0-9]{4}$" />
                        </div>

                    </div>
                </div>

                <!-- ACCOUNT SECURITY -->
                <div class="register-section">

                    <div class="register-section-title">
                        Account Security
                    </div>

                    <div class="row g-3">

                        <div class="col-md-6 field-group">
                            <label class="form-label">Choose a password</label>
                            <asp:TextBox
                                ID="txtPassword"
                                runat="server"
                                CssClass="form-control"
                                TextMode="Password"
                                data-pw-checklist="pwChecklist"
                                placeholder="Create a strong password" />

                            <asp:RequiredFieldValidator
                                runat="server"
                                ControlToValidate="txtPassword"
                                CssClass="text-danger"
                                Display="Dynamic"
                                ErrorMessage="Required." />

                            <div class="password-rules">
                                <div class="password-rules-title">
                                    Password requirements
                                </div>
                                <ul id="pwChecklist" class="list-unstyled small mb-0 mt-2">
                                    <li data-rule="length">At least 8 characters</li>
                                    <li data-rule="upper">An uppercase letter (A-Z)</li>
                                    <li data-rule="lower">A lowercase letter (a-z)</li>
                                    <li data-rule="number">A number (0-9)</li>
                                    <li data-rule="special">A special character (for example ! @ # $ %)</li>
                                </ul>
                            </div>
                        </div>

                        <div class="col-md-6 field-group">
                            <label class="form-label">Confirm password</label>
                            <asp:TextBox
                                ID="txtConfirmPassword"
                                runat="server"
                                CssClass="form-control"
                                TextMode="Password"
                                placeholder="Re-enter your password" />

                            <asp:RequiredFieldValidator
                                runat="server"
                                ControlToValidate="txtConfirmPassword"
                                CssClass="text-danger"
                                Display="Dynamic"
                                ErrorMessage="Required." />

                            <asp:CompareValidator
                                runat="server"
                                ControlToValidate="txtConfirmPassword"
                                ControlToCompare="txtPassword"
                                CssClass="text-danger"
                                Display="Dynamic"
                                ErrorMessage="Passwords do not match." />
                        </div>

                    </div>
                </div>

                <!-- SUBMIT -->
                <div class="register-actions">

                    <asp:Button
                        ID="btnRegister"
                        runat="server"
                        Text="Send verification code"
                        CssClass="register-submit"
                        OnClick="btnRegister_Click" />

                    <div class="register-footer">
                        Already have an account?
                        <a href="<%= ResolveUrl("~/Account/CustomerLogin.aspx") %>">
                            Log in
                        </a>
                    </div>

                </div>

            </div>

        </asp:Panel>

        <!-- STEP 2: SMS VERIFICATION -->
        <asp:Panel ID="pnlVerify" runat="server" Visible="false">

            <div class="register-card verification-card">

                <h2 class="verification-heading">
                    Verify your phone number
                </h2>

                <p class="verification-description">
                    We sent a 6-digit code by SMS to your phone ending in
                    <strong>
                        <asp:Label ID="lblLastDigits" runat="server" />
                    </strong>.
                    Enter it below to create your account.
                    The code expires in 5 minutes.
                </p>

                <asp:Label
                    ID="lblCodeInfo"
                    runat="server"
                    CssClass="alert alert-success d-block"
                    Visible="false" />

                <asp:Label
                    ID="lblCodeError"
                    runat="server"
                    CssClass="alert alert-danger d-block"
                    Visible="false" />

                <div class="mb-3">
                    <label class="form-label">Verification code</label>
                    <asp:TextBox
                        ID="txtCode"
                        runat="server"
                        CssClass="form-control verification-code"
                        MaxLength="6"
                        inputmode="numeric"
                        autocomplete="one-time-code"
                        placeholder="123456" />
                </div>

                <div class="verification-actions">

                    <asp:Button
                        ID="btnVerifyCode"
                        runat="server"
                        Text="Verify and create account"
                        CssClass="register-submit"
                        CausesValidation="false"
                        OnClick="btnVerifyCode_Click" />

                    <asp:LinkButton
                        ID="btnResendCode"
                        runat="server"
                        CssClass="register-link"
                        CausesValidation="false"
                        OnClick="btnResendCode_Click">
                        Send a new code
                    </asp:LinkButton>

                </div>

                <p class="mt-4 mb-0">
                    <asp:LinkButton
                        ID="btnBack"
                        runat="server"
                        CssClass="register-link"
                        CausesValidation="false"
                        OnClick="btnBack_Click">
                        Change my details
                    </asp:LinkButton>
                </p>

            </div>

        </asp:Panel>

    </div>
</div>

<script src="<%= ResolveUrl("~/Scripts/password-rules.js") %>"></script>

</asp:Content>
