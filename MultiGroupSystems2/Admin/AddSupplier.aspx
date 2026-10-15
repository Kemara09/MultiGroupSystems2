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

    <section class="form-container">
        <div class="form-header">
            <div class="form-header-icon">+</div>
            <div>
                <h2>Supplier Information</h2>
                <p>Enter the supplier details below.</p>
            </div>
        </div>

        <div class="form-body">
            <div class="form-group full-width">
                <label>Supplier Name</label>               
                <input type="text" placeholder="Enter supplier name" />
            </div>

            <div class="form-row">
                <div class="form-group">
                    <label>Contact Person</label>
                    <input type="text" placeholder="Enter contact person" />
                </div>
                <div class="form-group">
                    <label>Email Address</label>
                    <input type="email" placeholder="supplier@example.com" />
                </div>
            </div>

            <div class="form-row">
                <div class="form-group">
                    <label>Phone Number</label>
                    <input type="text" placeholder="Enter phone number" />
                </div>
                <div class="form-group">
                    <label>Supplier Type</label>
                    <select>
                        <option>Select supplier type</option>
                        <option>Technology</option>
                        <option>Equipment</option>
                        <option>Services</option>
                        <option>Software</option>
                        <option>Other</option>
                    </select>
                </div>
            </div>

           <div class="form-row">
    <div class="form-group" style="flex: 0 0 25%;">
        <label>Street No.</label>
        <asp:TextBox ID="txtStreetNo" runat="server" CssClass="form-control" placeholder="No." />
    </div>
    <div class="form-group" style="flex: 1;">
        <label>Street Name</label>
        <asp:TextBox ID="txtStreetName" runat="server" CssClass="form-control" placeholder="Enter street name" />
    </div>
</div>

<div class="form-row">
    <div class="form-group">
        <label>Suburb</label>
        <asp:TextBox ID="txtSuburb" runat="server" CssClass="form-control" placeholder="Enter suburb" />
    </div>
    <div class="form-group">
        <label>City</label>
        <asp:TextBox ID="txtCity" runat="server" CssClass="form-control" placeholder="Enter city" />
    </div>
</div>
    <div class="form-row">
    <div class="form-group">
        <label>Province</label>
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
    </div>
        <div class="form-group">
        <label>Postal Code</label>
        <asp:TextBox ID="txtPostalCode" runat="server" CssClass="form-control" placeholder="Enter postal code" />
    </div>


            <div class="form-group full-width">
                <label>Additional Notes</label>
                <textarea rows="4" placeholder="Enter any additional supplier information"></textarea>
            </div>

            <div class="form-actions">
                <button type="button" class="cancel-button">Cancel</button>
                <button type="button" class="add-button">+ Add Supplier</button>
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

</asp:Content>