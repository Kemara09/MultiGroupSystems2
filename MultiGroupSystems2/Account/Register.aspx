<%@ Page Title="Register" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Register.aspx.cs" Inherits="MultiGroupSystemsTester.Account.Register" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h1>Create an Account</h1>
    <asp:Label ID="lblError" runat="server" CssClass="alert alert-danger d-block" Visible="false"></asp:Label>
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
            <asp:RequiredFieldValidator runat="server" ControlToValidate="txtEmail" CssClass="text-danger" Display="Dynamic" ErrorMessage="Required." />
            <asp:RegularExpressionValidator runat="server" ControlToValidate="txtEmail" CssClass="text-danger" Display="Dynamic" ErrorMessage="Invalid email." ValidationExpression="^[^@\s]+@[^@\s]+\.[^@\s]+$" />
        </div>
       <div class="col-md-6">
            <label class="form-label">Phone number</label>
            <asp:TextBox ID="txtPhone" runat="server" CssClass="form-control" />
            <asp:RequiredFieldValidator runat="server" ControlToValidate="txtPhone" CssClass="text-danger" Display="Dynamic" ErrorMessage="Required." />
            <asp:RegularExpressionValidator runat="server" ControlToValidate="txtPhone" CssClass="text-danger" Display="Dynamic" ErrorMessage="Enter a valid 10-digit number (e.g. 0312050565)." ValidationExpression="^0[0-9]{9}$" />
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
            <label class="form-label">Choose a username</label>
            <asp:TextBox ID="txtUsername" runat="server" CssClass="form-control" />
            <asp:RequiredFieldValidator runat="server" ControlToValidate="txtUsername" CssClass="text-danger" Display="Dynamic" ErrorMessage="Required." />
        </div>
        <div class="col-md-6">
            <label class="form-label">Choose a password</label>
            <asp:TextBox ID="txtPassword" runat="server" CssClass="form-control" TextMode="Password" />
            <asp:RequiredFieldValidator runat="server" ControlToValidate="txtPassword" CssClass="text-danger" Display="Dynamic" ErrorMessage="Required." />
        </div>
    </div>
    <asp:Button ID="btnRegister" runat="server" Text="Create account" CssClass="btn btn-teal mt-3" OnClick="btnRegister_Click" />
</asp:Content>