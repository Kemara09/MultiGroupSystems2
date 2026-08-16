<%@ Page Title="Privacy Policy" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="PrivacyPolicy.aspx.cs" Inherits="MultiGroupSystemsTester.PrivacyPolicy" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h1>Privacy Policy</h1>
    <p class="text-muted">Last updated: <%= DateTime.Now.ToString("MMMM yyyy") %></p>

    <h4 class="mt-4">Information We Collect</h4>
    <p>When you register, place an order, or contact us, we collect personal information including your name, email address, phone number, and delivery address.</p>

    <h4 class="mt-4">How We Use Your Information</h4>
    <p>We use your information to process orders, send order confirmations and delivery updates via email and SMS, generate invoices, and respond to enquiries.</p>

    <h4 class="mt-4">Cookies</h4>
    <p>This site uses cookies to remember your login preferences (e.g. the "Remember me" option) and to determine whether you have accepted our cookie notice. No cookies are used for advertising or third-party tracking.</p>

    <h4 class="mt-4">Data Sharing</h4>
    <p>We do not sell or share your personal information with third parties, except service providers directly involved in fulfilling your order (e.g. our email and SMS notification providers).</p>

    <h4 class="mt-4">Your Rights</h4>
    <p>Under South Africa's Protection of Personal Information Act (POPIA), you have the right to access, correct, or request deletion of your personal information. You can update your details anytime via <a href="~/MyAccount.aspx" runat="server">My Account</a>, or contact us directly.</p>

    <h4 class="mt-4">Contact Us</h4>
    <p>For any privacy-related questions, please reach out via our <a href="~/Contact.aspx" runat="server">Contact page</a>.</p>
</asp:Content>