<%@ Page Title="Staff Data Capture" Language="C#" AutoEventWireup="true" MasterPageFile="~/Admin/AdminMaster.master" CodeBehind="StaffDataCapture.aspx.cs" Inherits="MultiGroupSystemsTester.Admin.StaffDataCapture" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="StaffDataCapture.css" rel="stylesheet" type="text/css" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="BreadcrumbContent" runat="server">
    Staff Data Capture
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">

    <div class="page-container">

        <!-- PAGE HEADER -->
        <header class="page-header">
            <div class="header-content">
                <h1>Staff Data Capture</h1>
                <p>Capture and manage staff member information</p>
            </div>
            
        </header>

        <!-- MAIN CONTENT -->
        <main class="content-area">

            <!-- PERSONAL INFORMATION -->
            <section class="card">
                <div class="card-header">
                    <h2>Personal Information</h2>
                    <p>Enter the staff member's personal details.</p>
                </div>
                <div class="card-body">
                    <div class="form-row">
                        <div class="form-group">
                            <label for="txtFirstName">First Name <span class="required">*</span></label>
                            <asp:TextBox ID="txtFirstName" runat="server" CssClass="form-control" placeholder="Enter first name"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvFirstName" runat="server" ControlToValidate="txtFirstName"
                                ErrorMessage="First name is required." ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="revFirstName" runat="server" ControlToValidate="txtFirstName"
                                ValidationExpression="^[a-zA-Z\s'-]+$" ErrorMessage="First name can only contain letters." ForeColor="Red" Display="Dynamic"></asp:RegularExpressionValidator>
                        </div>
                        <div class="form-group">
                            <label for="txtLastName">Last Name <span class="required">*</span></label>
                            <asp:TextBox ID="txtLastName" runat="server" CssClass="form-control" placeholder="Enter last name"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvLastName" runat="server" ControlToValidate="txtLastName"
                                ErrorMessage="Last name is required." ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="revLastName" runat="server" ControlToValidate="txtLastName"
                                ValidationExpression="^[a-zA-Z\s'-]+$" ErrorMessage="Last name can only contain letters." ForeColor="Red" Display="Dynamic"></asp:RegularExpressionValidator>
                        </div>
                    </div>

                    <div class="form-row">
                        <div class="form-group">
                            <label for="txtEmail">Email Address <span class="required">*</span></label>
                            <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" TextMode="Email" placeholder="example@email.com"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvEmail" runat="server" ControlToValidate="txtEmail"
                                ErrorMessage="Email address is required." ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="revEmail" runat="server" ControlToValidate="txtEmail"
                                ValidationExpression="^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$" ErrorMessage="Invalid email address format." ForeColor="Red" Display="Dynamic"></asp:RegularExpressionValidator>
                        </div>
                         <div class="form-group">
                              <label for="txtPhone">Phone Number <span class="required">*</span> (e.g. +27 31 123 4567)</label>
                              <asp:TextBox ID="txtPhone" runat="server" CssClass="form-control" placeholder="+27 31 123 4567"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvPhone" runat="server" ControlToValidate="txtPhone"
                                         ErrorMessage="Phone number is required." ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator ID="revPhone" runat="server" ControlToValidate="txtPhone"
                                             ValidationExpression="^\+27\s\d{2}\s\d{3}\s\d{4}$" ErrorMessage="Format must be +27 XX XXX XXXX" ForeColor="Red" Display="Dynamic"></asp:RegularExpressionValidator>
                          </div>
                    </div>

                   
                </div>
            </section>


            <!-- ACCOUNT & ROLE DETAILS -->
            <section class="card">
                <div class="card-header">
                    <h2>Account & Role Details</h2>
                    <p>Assign a role and username for the staff member.</p>
                </div>
                <div class="card-body">
                    <div class="form-group mb-3">
                        <label for="ddlRole">Role <span class="required">*</span></label>
                        <asp:DropDownList ID="ddlRole" runat="server" CssClass="form-control">
                            <asp:ListItem Value="">Select role</asp:ListItem>
                            <asp:ListItem Value="Manager">Manager</asp:ListItem>
                            <asp:ListItem Value="System Administrator">System Administrator</asp:ListItem>
                            <asp:ListItem Value="Owner">Owner</asp:ListItem>
                            <asp:ListItem Value="Tester">Tester</asp:ListItem>
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="rfvRole" runat="server" ControlToValidate="ddlRole" InitialValue=""
                            ErrorMessage="Please select a role." ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>
                    </div>

                    <div class="form-row">
                        <div class="form-group">
                            <label for="txtUsername">Username <span class="required">*</span></label>
                            <asp:TextBox ID="txtUsername" runat="server" CssClass="form-control" placeholder="Enter username"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvUsername" runat="server" ControlToValidate="txtUsername"
                                ErrorMessage="Username is required." ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>
                        </div>
                    </div>

                    <p class="form-hint" style="margin: 14px 0 0; font-size: 12px; color: #64748b;">
                        No password is set here. The staff member activates their account with this username,
                        confirms a code sent by SMS to the phone number above, and then chooses their own password.
                    </p>
                </div>
            </section>


           <!-- FORM FOOTER -->
<div class="form-footer">
    <asp:Button ID="BtnCancel" runat="server" Text="Cancel" CssClass="btn btn-secondary"
        CausesValidation="false" OnClick="BtnCancel_Click" />
    <asp:Button ID="BtnSave" runat="server" Text="Save Staff Member" CssClass="btn btn-primary"
        OnClick="BtnSave_Click" />
</div>

        </main>

    </div>

</asp:Content>
