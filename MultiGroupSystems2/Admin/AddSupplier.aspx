<%@ Page Title="Add Supplier" Language="C#" AutoEventWireup="true" MasterPageFile="~/Admin/AdminMaster.master" CodeBehind="AddSupplier.aspx.cs" Inherits="MultiGroupSystemsTester.Admin.AddSupplier" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="AddSupplier.css" rel="stylesheet" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="BreadcrumbContent" runat="server">
    Add Supplier
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">

    <section class="page-heading">
        <div>
            <h1>Add Supplier</h1>
            <p>Add a new supplier to your supplier directory.</p>
        </div>
    </section>

    <section class="form-container" style="padding: 30px 40px; margin-bottom: 30px; background-color: #ffffff; border-radius: 8px; box-shadow: 0 2px 4px rgba(0, 0, 0, 0.05);">
        <div class="form-header" style="display: flex; align-items: center; margin-bottom: 25px;">
            <div class="form-header-icon">+</div>
            <div>
                <h2>Supplier Information</h2>
                <p>Enter the supplier details below.</p>
            </div>
        </div>

        <div class="form-body">
            <div class="form-group full-width" style="margin-bottom: 20px;">      
                <asp:Label ID="lblMessage" runat="server" EnableViewState="false" />
            </div>
<!-- supplier name-->
            <div class="form-group full-width" style="margin-bottom: 20px;">
                <label style="display: block; font-weight: 600; font-size: 14px; color: #333333; margin-bottom: 8px;">Supplier Name</label>
                <asp:TextBox ID="txtSupplierName" runat="server" CssClass="form-control" placeholder="Enter supplier name" />
                <asp:RequiredFieldValidator ID="rfvSupplierName" runat="server" ControlToValidate="txtSupplierName" ErrorMessage="Supplier name is required." ForeColor="Red" Display="Dynamic" />  <!-- check not empty -->
            </div>
<!--contact person-->
            <div class="form-row" style="display: flex; gap: 20px; margin-bottom: 20px;">
                <div class="form-group" style="flex: 1; display: flex; flex-direction: column;">
                    <label style="display: block; font-weight: 600; font-size: 14px; color: #333333; margin-bottom: 8px;">Contact Person</label>
                    <asp:TextBox ID="txtContactPerson" runat="server" CssClass="form-control" placeholder="Enter contact person" />
                    <asp:RequiredFieldValidator ID="rfvContactPerson" runat="server" ControlToValidate="txtContactPerson" ErrorMessage="Contact person is required." ForeColor="Red" Display="Dynamic" />  <!-- check not empty -->
                    <asp:RegularExpressionValidator ID="revContactPerson" runat="server" ControlToValidate="txtContactPerson" ValidationExpression="^[a-zA-Z\s]+$" ErrorMessage="Name must contain letters only." ForeColor="Red" Display="Dynamic" /> <!-- check only letters -->
                </div>
<!--email-->
                <div class="form-group" style="flex: 1; display: flex; flex-direction: column;">
                    <label style="display: block; font-weight: 600; font-size: 14px; color: #333333; margin-bottom: 8px;">Email Address</label>
                    <asp:TextBox ID="txtEmailAddress" runat="server" TextMode="Email" CssClass="form-control" placeholder="supplier@example.com" />
                    <asp:RequiredFieldValidator ID="rfvEmail" runat="server" ControlToValidate="txtEmailAddress" ErrorMessage="Email is required." ForeColor="Red" Display="Dynamic" />
                    <asp:RegularExpressionValidator ID="revEmail" runat="server" ControlToValidate="txtEmailAddress" 
    ValidationExpression="^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$" 
    ErrorMessage="Please enter a valid email address." ForeColor="Red" Display="Dynamic" />       <!-- check for @ and . -->
                </div>
            </div>
<!-- phonenumber-->
           <div class="form-group" style="flex: 1; display: flex; flex-direction: column;">
    <label style="display: block; font-weight: 600; font-size: 14px; color: #333333; margin-bottom: 8px;">Phone Number</label>
    <asp:TextBox ID="txtPhoneNumber" runat="server" CssClass="form-control" placeholder="+27 31 123 4567" />
    <asp:RequiredFieldValidator ID="rfvPhone" runat="server" ControlToValidate="txtPhoneNumber" ErrorMessage="Phone number is required." ForeColor="Red" Display="Dynamic" />
    <asp:RegularExpressionValidator ID="revPhone" runat="server" ControlToValidate="txtPhoneNumber" 
        ValidationExpression="^\+27\s\d{2}\s\d{3}\s\d{4}$" 
        ErrorMessage="Phone number must be in the format: +27 XX XXX XXXX" ForeColor="Red" Display="Dynamic" />
</div>
<!-- supp type-->
                <div class="form-group" style="flex: 1; display: flex; flex-direction: column;">
                    <label style="display: block; font-weight: 600; font-size: 14px; color: #333333; margin-bottom: 8px;">Supplier Type</label>
                    <asp:DropDownList ID="ddlSupplierType" runat="server" CssClass="form-control">
                        <asp:ListItem Text="Select supplier type" Value="" />
                        <asp:ListItem Text="Technology" Value="Technology" />
                        <asp:ListItem Text="Equipment" Value="Equipment" />
                        <asp:ListItem Text="Services" Value="Services" />
                        <asp:ListItem Text="Software" Value="Software" />
                        <asp:ListItem Text="Other" Value="Other" />
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator ID="rfvSupplierType" runat="server" ControlToValidate="ddlSupplierType" InitialValue="" ErrorMessage="Please select a supplier type." ForeColor="Red" Display="Dynamic" />
                </div>
            </div>
<!-- street no-->
            <div class="form-row" style="display: flex; gap: 20px; margin-bottom: 20px;">
                <div class="form-group" style="flex: 0 0 25%; display: flex; flex-direction: column;">
                    <label style="display: block; font-weight: 600; font-size: 14px; color: #333333; margin-bottom: 8px;">Street No.</label>
                    <asp:TextBox ID="txtStreetNo" runat="server" CssClass="form-control" placeholder="No." />
                </div>
<!--street name-->
                <div class="form-group" style="flex: 1; display: flex; flex-direction: column;">
                    <label style="display: block; font-weight: 600; font-size: 14px; color: #333333; margin-bottom: 8px;">Street Name</label>
                    <asp:TextBox ID="txtStreetName" runat="server" CssClass="form-control" placeholder="Enter street name" />
                    <asp:RequiredFieldValidator ID="rfvStreetName" runat="server" ControlToValidate="txtStreetName" ErrorMessage="Street name is required." ForeColor="Red" Display="Dynamic" />
                </div>
            </div>
<!--sub-->
            <div class="form-row" style="display: flex; gap: 20px; margin-bottom: 20px;">
                <div class="form-group" style="flex: 1; display: flex; flex-direction: column;">
                    <label style="display: block; font-weight: 600; font-size: 14px; color: #333333; margin-bottom: 8px;">Suburb</label>
                    <asp:TextBox ID="txtSuburb" runat="server" CssClass="form-control" placeholder="Enter suburb" />
                </div>
<!--city-->
                <div class="form-group" style="flex: 1; display: flex; flex-direction: column;">
                    <label style="display: block; font-weight: 600; font-size: 14px; color: #333333; margin-bottom: 8px;">City</label>
                    <asp:TextBox ID="txtCity" runat="server" CssClass="form-control" placeholder="Enter city" />
                    <asp:RequiredFieldValidator ID="rfvCity" runat="server" ControlToValidate="txtCity" ErrorMessage="City is required." ForeColor="Red" Display="Dynamic" />
                </div>
            </div>
<!--province-->
            <div class="form-row" style="display: flex; gap: 20px; margin-bottom: 20px;">
                <div class="form-group" style="flex: 1; display: flex; flex-direction: column;">
                    <label style="display: block; font-weight: 600; font-size: 14px; color: #333333; margin-bottom: 8px;">Province</label>
                    <asp:DropDownList ID="ddlProvince" runat="server" CssClass="form-control">
                        <asp:ListItem Text="Select province" Value="" />
                        <asp:ListItem Text="KwaZulu-Natal" Value="KwaZulu-Natal" />
                        <asp:ListItem Text="Gauteng" Value="Gauteng" />
                        <asp:ListItem Text="Western Cape" Value="Western Cape" />
                        <asp:ListItem Text="Eastern Cape" Value="Eastern Cape" />
                        <asp:ListItem Text="Free State" Value="Free State" />
                        <asp:ListItem Text="Northern Cape" Value="Northern Cape" />
                        <asp:ListItem Text="North West" Value="North West" />
                        <asp:ListItem Text="Mpumalanga" Value="Mpumalanga" />
                        <asp:ListItem Text="Limpopo" Value="Limpopo" />
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator ID="rfvProvince" runat="server" ControlToValidate="ddlProvince" InitialValue="" ErrorMessage="Please select a province." ForeColor="Red" Display="Dynamic" />
                </div>
<!--postal-->
                <div class="form-group" style="flex: 1; display: flex; flex-direction: column;">
                    <label style="display: block; font-weight: 600; font-size: 14px; color: #333333; margin-bottom: 8px;">Postal Code</label>
                    <asp:TextBox ID="txtPostalCode" runat="server" CssClass="form-control" placeholder="Enter postal code" />
                    <asp:RequiredFieldValidator ID="rfvPostalCode" runat="server" ControlToValidate="txtPostalCode" ErrorMessage="Postal code is required." ForeColor="Red" Display="Dynamic" />
                    <asp:RegularExpressionValidator ID="revPostalCode" runat="server" ControlToValidate="txtPostalCode" ValidationExpression="^\d{4}$" ErrorMessage="Postal code must be 4 digits." ForeColor="Red" Display="Dynamic" />
                </div>
            </div>

            <div class="form-group full-width" style="margin-bottom: 20px;">
                <label style="display: block; font-weight: 600; font-size: 14px; color: #333333; margin-bottom: 8px;">Additional Notes</label>
                <asp:TextBox ID="txtNotes" runat="server" TextMode="MultiLine" Rows="4" CssClass="form-control" placeholder="Enter any additional supplier information" />
            </div>

            <div class="form-actions" style="display: flex; justify-content: flex-end; gap: 12px; margin-top: 30px;">
                <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="cancel-button" CausesValidation="false" OnClick="btnCancel_Click" style="padding: 10px 20px; border-radius: 6px; font-size: 14px; font-weight: 600; cursor: pointer; border: none; background-color: #f3f4f6; color: #374151;" />
                <asp:Button ID="btnAddSupplier" runat="server" Text="+ Add Supplier" CssClass="add-button" OnClick="btnAddSupplier_Click" style="padding: 10px 20px; border-radius: 6px; font-size: 14px; font-weight: 600; cursor: pointer; border: none; background-color: #2563eb; color: #ffffff;" />
            </div>
        </div>
    </section>

    <div class="information-box">
        <div class="information-icon">i</div>
        <div>
            <strong>Supplier Information</strong>
            <p>Make sure the supplier information is accurate before adding it to the system.</p>
        </div>
    </div>
    <!-- format code make it neater css stuff -->
</asp:Content>