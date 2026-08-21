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

            <div class="form-group full-width">
                <label>Address</label>
                <textarea rows="4" placeholder="Enter supplier address"></textarea>
            </div>

            <div class="form-row">
                <div class="form-group">
                    <label>City</label>
                    <input type="text" placeholder="Enter city" />
                </div>
                <div class="form-group">
                    <label>Country</label>
                    <select>
                        <option>Select country</option>
                        <option>South Africa</option>
                        <option>United Kingdom</option>
                        <option>United States</option>
                        <option>Australia</option>
                        <option>Other</option>
                    </select>
                </div>
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