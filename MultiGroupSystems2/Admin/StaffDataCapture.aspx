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
            <div class="header-actions">
                <button type="button" class="cancel-button">Cancel</button>
                <button type="button" class="save-button">Save Staff Member</button>
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
                        </div>
                        <div class="form-group">
                            <label for="txtLastName">Last Name <span class="required">*</span></label>
                            <asp:TextBox ID="txtLastName" runat="server" CssClass="form-control" placeholder="Enter last name"></asp:TextBox>
                        </div>
                    </div>

                    <div class="form-row">
                        <div class="form-group">
                            <label for="txtIDNumber">ID Number <span class="required">*</span></label>
                            <asp:TextBox ID="txtIDNumber" runat="server" CssClass="form-control" placeholder="Enter ID number"></asp:TextBox>
                        </div>
                        <div class="form-group">
                            <label for="ddlGender">Gender</label>
                            <asp:DropDownList ID="ddlGender" runat="server" CssClass="form-control">
                                <asp:ListItem Value="">Select gender</asp:ListItem>
                                <asp:ListItem>Male</asp:ListItem>
                                <asp:ListItem>Female</asp:ListItem>
                                <asp:ListItem>Other</asp:ListItem>
                                <asp:ListItem>Prefer not to say</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </div>

                    <div class="form-group">
                        <label for="txtDateOfBirth">Date of Birth</label>
                        <asp:TextBox ID="txtDateOfBirth" runat="server" CssClass="form-control" TextMode="Date"></asp:TextBox>
                    </div>
                </div>
            </section>

            <!-- CONTACT INFORMATION -->
            <section class="card">
                <div class="card-header">
                    <h2>Contact Information</h2>
                    <p>Enter the staff member's contact details.</p>
                </div>
                <div class="card-body">
                    <div class="form-row">
                        <div class="form-group">
                            <label for="txtEmail">Email Address <span class="required">*</span></label>
                            <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" TextMode="Email" placeholder="example@email.com"></asp:TextBox>
                        </div>
                        <div class="form-group">
                            <label for="txtPhone">Phone Number <span class="required">*</span></label>
                            <asp:TextBox ID="txtPhone" runat="server" CssClass="form-control" placeholder="Enter phone number"></asp:TextBox>
                        </div>
                    </div>

                    <div class="form-group">
                        <label for="txtAddress">Address</label>
                        <asp:TextBox ID="txtAddress" runat="server" CssClass="description-control" TextMode="MultiLine" Rows="4" placeholder="Enter residential address"></asp:TextBox>
                    </div>
                </div>
            </section>

            <!-- EMPLOYMENT INFORMATION -->
            <section class="card">
                <div class="card-header">
                    <h2>Employment Information</h2>
                    <p>Enter the staff member's employment details.</p>
                </div>
                <div class="card-body">
                    <div class="form-row">
                        <div class="form-group">
                            <label for="txtEmployeeNumber">Employee Number <span class="required">*</span></label>
                            <asp:TextBox ID="txtEmployeeNumber" runat="server" CssClass="form-control" placeholder="e.g. EMP001"></asp:TextBox>
                        </div>
                        <div class="form-group">
                            <label for="ddlDepartment">Department <span class="required">*</span></label>
                            <asp:DropDownList ID="ddlDepartment" runat="server" CssClass="form-control">
                                <asp:ListItem Value="">Select department</asp:ListItem>
                                <asp:ListItem>Administration</asp:ListItem>
                                <asp:ListItem>Sales</asp:ListItem>
                                <asp:ListItem>Marketing</asp:ListItem>
                                <asp:ListItem>Finance</asp:ListItem>
                                <asp:ListItem>Human Resources</asp:ListItem>
                                <asp:ListItem>IT</asp:ListItem>
                                <asp:ListItem>Logistics</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </div>

                    <div class="form-row">
                        <div class="form-group">
                            <label for="txtPosition">Job Position <span class="required">*</span></label>
                            <asp:TextBox ID="txtPosition" runat="server" CssClass="form-control" placeholder="Enter job position"></asp:TextBox>
                        </div>
                        <div class="form-group">
                            <label for="ddlEmploymentType">Employment Type</label>
                            <asp:DropDownList ID="ddlEmploymentType" runat="server" CssClass="form-control">
                                <asp:ListItem>Full-Time</asp:ListItem>
                                <asp:ListItem>Part-Time</asp:ListItem>
                                <asp:ListItem>Contract</asp:ListItem>
                                <asp:ListItem>Temporary</asp:ListItem>
                                <asp:ListItem>Intern</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </div>

                    <div class="form-row">
                        <div class="form-group">
                            <label for="txtStartDate">Start Date</label>
                            <asp:TextBox ID="txtStartDate" runat="server" CssClass="form-control" TextMode="Date"></asp:TextBox>
                        </div>
                        <div class="form-group">
                            <label for="ddlStatus">Staff Status</label>
                            <asp:DropDownList ID="ddlStatus" runat="server" CssClass="form-control">
                                <asp:ListItem>Active</asp:ListItem>
                                <asp:ListItem>Inactive</asp:ListItem>
                                <asp:ListItem>On Leave</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </div>
                </div>
            </section>

            <!-- EMERGENCY CONTACT -->
            <section class="card">
                <div class="card-header">
                    <h2>Emergency Contact</h2>
                    <p>Enter an emergency contact for the staff member.</p>
                </div>
                <div class="card-body">
                    <div class="form-row">
                        <div class="form-group">
                            <label for="txtEmergencyName">Contact Name</label>
                            <asp:TextBox ID="txtEmergencyName" runat="server" CssClass="form-control" placeholder="Enter contact name"></asp:TextBox>
                        </div>
                        <div class="form-group">
                            <label for="txtEmergencyPhone">Contact Number</label>
                            <asp:TextBox ID="txtEmergencyPhone" runat="server" CssClass="form-control" placeholder="Enter contact number"></asp:TextBox>
                        </div>
                    </div>

                    <div class="form-group">
                        <label for="txtRelationship">Relationship</label>
                        <asp:TextBox ID="txtRelationship" runat="server" CssClass="form-control" placeholder="e.g. Parent, Spouse, Sibling"></asp:TextBox>
                    </div>
                </div>
            </section>

            <!-- FORM FOOTER -->
            <div class="form-footer">
                <button type="button" class="cancel-button footer-button">Cancel</button>
                <button type="button" class="save-button footer-button">Save Staff Member</button>
            </div>

        </main>

    </div>

</asp:Content>