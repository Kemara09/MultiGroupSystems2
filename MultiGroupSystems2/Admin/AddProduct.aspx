<%@ Page Title="Add Product" Language="C#" AutoEventWireup="true"
MasterPageFile="~/Admin/AdminMaster.master"
CodeBehind="AddProduct.aspx.cs"
Inherits="MultiGroupSystemsTester.Admin.AddProduct" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadContent" runat="server"> <link href="AddProduct.css" rel="stylesheet" type="text/css" />
</asp:Content>

<asp:Content ID="BreadcrumbContent" ContentPlaceHolderID="BreadcrumbContent" runat="server">
Add Product
</asp:Content>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" runat="server">
<header class="page-header">
            <div>
                <h1>Add Product</h1>
                <p>Add a new product to your store</p>
            </div>
        </header>

        <!-- Main Content -->
        <main class="content-area">

            <!-- Left Column -->
            <section class="left-column">

                <!-- Basic Information -->
                <div class="card">
                    <div class="card-header">
                        <h2>Product Information</h2>
                        <p>Enter the basic information about your product.</p>
                    </div>

                    <div class="card-body">

                        <!-- Product Name -->
                        <div class="form-group">
                            <label for="txtProductName">
                                Product Name
                                <span class="required">*</span>
                            </label>
                            <asp:TextBox
                                ID="txtProductName"
                                runat="server"
                                CssClass="form-control"
                                placeholder="Enter product name">
                            </asp:TextBox>
                        </div>

                        <!-- Category and Price -->
                        <div class="form-row">
                            <div class="form-group">
                                <label for="ddlCategory">
                                    Category
                                    <span class="required">*</span>
                                </label>
                                <asp:DropDownList
                                    ID="ddlCategory"
                                    runat="server"
                                    CssClass="form-control">
                                    <asp:ListItem Value="">Select category</asp:ListItem>
                                    <asp:ListItem>Accessories</asp:ListItem>
                                    <asp:ListItem>Electronics</asp:ListItem>
                                    <asp:ListItem>Rail</asp:ListItem>
                                    <asp:ListItem>Livestock Inventory</asp:ListItem>
                                    <asp:ListItem>Audio Visual Equipment</asp:ListItem>
                                    <asp:ListItem>IT Equipment</asp:ListItem>
                                    <asp:ListItem>High Security Locking Systems</asp:ListItem>
                                    <asp:ListItem>Security Equipment</asp:ListItem>
                                </asp:DropDownList>
                            </div>

                            <div class="form-group">
                                <label for="txtPrice">
                                    Price
                                    <span class="required">*</span>
                                </label>
                                <div class="price-input">
                                    <span class="currency">R</span>
                                    <asp:TextBox
                                        ID="txtPrice"
                                        runat="server"
                                        CssClass="price-control"
                                        placeholder="0.00">
                                    </asp:TextBox>
                                </div>
                            </div>
                        </div>

                        <!-- Description -->
                        <div class="form-group">
                            <label for="txtDescription">
                                Product Description
                            </label>
                            <asp:TextBox
                                ID="txtDescription"
                                runat="server"
                                CssClass="description-control"
                                TextMode="MultiLine"
                                Rows="6"
                                placeholder="Describe your product...">
                            </asp:TextBox>
                            <span class="field-help">
                                Provide a clear description of the product.
                            </span>
                        </div>

                    </div>
                </div>

                <!-- Inventory & Supply -->
                <div class="card">
                    <div class="card-header">
                        <h2>Inventory &amp; Supply</h2>
                        <p>Manage your product stock, reorder levels, and supplier information.</p>
                    </div>

                    <div class="card-body">

                        <!-- Row 1: Stock Quantity & Re-order Level -->
                        <div class="form-row">
                            <div class="form-group">
                                <label for="txtStock">
                                    Quantity in stock
                                    <span class="required">*</span>
                                </label>
                                <asp:TextBox
                                    ID="txtStock"
                                    runat="server"
                                    CssClass="form-control"
                                    placeholder="e.g. 50">
                                </asp:TextBox>
                                <asp:RegularExpressionValidator
                                    ID="revStock"
                                    runat="server"
                                    ControlToValidate="txtStock"
                                    ValidationExpression="^\d+$"
                                    ErrorMessage="Only numbers are allowed."
                                    ForeColor="Red"
                                    Display="Dynamic" />
                            </div>

                            <div class="form-group">
                                <label for="txtReorderLevel">
                                    Re-order Level
                                    <span class="required">*</span>
                                </label>
                                <asp:TextBox
                                    ID="txtReorderLevel"
                                    runat="server"
                                    CssClass="form-control"
                                    placeholder="e.g. 5">
                                </asp:TextBox>
                                <asp:RegularExpressionValidator
                                    ID="revReorderLevel"
                                    runat="server"
                                    ControlToValidate="txtReorderLevel"
                                    ValidationExpression="^\d+$"
                                    ErrorMessage="Only numbers are allowed."
                                    ForeColor="Red"
                                    Display="Dynamic" />
                            </div>
                        </div>

                        <!-- Row 2: Supplier & Product Status -->
                        <div class="form-row">
                            <div class="form-group">
                                <label for="ddlSupplier">
                                    Supplier
                                    <span class="required">*</span>
                                </label>
                                <asp:DropDownList
                                    ID="ddlSupplier"
                                    runat="server"
                                    CssClass="form-control"
                                    AppendDataBoundItems="true">
                                    <asp:ListItem Value="">Select supplier</asp:ListItem>
                                </asp:DropDownList>
                            </div>

                           
                        </div>

                    </div>
                </div>

            </section>

            <!-- Right Column -->
            <aside class="right-column">

                <!-- Product Image -->
                <div class="card">
                    <div class="card-header">
                        <h2>Product Image</h2>
                        <p>Upload an image of your product.</p>
                    </div>
                    <div class="card-body">
                        <div class="image-upload">
                            <div class="upload-icon">+</div>
                            <h3>Upload Product Image</h3>
                            <p>Click below to select an image</p>
                            <asp:FileUpload
                                ID="fileProductImage"
                                runat="server"
                                CssClass="file-upload" />
                            <span class="upload-help">JPG, PNG or JPEG</span>
                        </div>
                    </div>
                </div>

                <!-- Product Summary -->
               
<div class="card summary-card">
    <div class="card-header">
        <h2>Product Summary</h2>
        <p>Review your product information.</p>
    </div>
    <div class="card-body">
        <div class="summary-item">
            <span>Product</span>
            <strong id="lblSumProduct">New Product</strong>
        </div>
        <div class="summary-item">
            <span>Category</span>
            <strong id="lblSumCategory">Not selected</strong>
        </div>
        <div class="summary-item">
            <span> Unit Price</span>
            <strong id="lblSumPrice">R 0.00</strong>
        </div>
        <div class="summary-item">
            <span>Stock</span>
            <strong id="lblSumStock">0 units</strong>
        </div>
    </div>
</div>

            </aside>

        </main>

        <!-- Bottom Form Actions Footer -->
        <div class="form-footer-actions" style="margin-top: 30px; display: flex; justify-content: flex-end; gap: 15px;">
            <asp:Button
    ID="btnCancel"
    runat="server"
    Text="Cancel"
    CssClass="cancel-button"
    CausesValidation="false"
    OnClientClick="window.location.href='Dashboard.aspx'; return false;" />

            <asp:Button
                ID="btnSaveProduct"
                runat="server"
                Text="Add Product"
                CssClass="save-button"
                OnClick="btnSaveProduct_Click" />
        </div>
    <script type="text/javascript">
    document.addEventListener("DOMContentLoaded", function () {
        const txtName = document.getElementById('<%= txtProductName.ClientID %>');
        const ddlCat = document.getElementById('<%= ddlCategory.ClientID %>');
        const txtPr = document.getElementById('<%= txtPrice.ClientID %>');
        const txtStk = document.getElementById('<%= txtStock.ClientID %>');

        const sumProd = document.getElementById('lblSumProduct');
        const sumCat = document.getElementById('lblSumCategory');
        const sumPr = document.getElementById('lblSumPrice');
        const sumStk = document.getElementById('lblSumStock');

        if (txtName) {
            txtName.addEventListener('input', function () {
                sumProd.textContent = txtName.value.trim() || 'New Product';
            });
        }

        if (ddlCat) {
            ddlCat.addEventListener('change', function () {
                sumCat.textContent = ddlCat.value ? ddlCat.options[ddlCat.selectedIndex].text : 'Not selected';
            });
        }

        if (txtPr) {
            txtPr.addEventListener('input', function () {
                let val = txtPr.value.trim();
                sumPr.textContent = val ? 'R ' + val : 'R 0.00';
            });
        }

        if (txtStk) {
            txtStk.addEventListener('input', function () {
                let val = txtStk.value.trim();
                sumStk.textContent = val ? val + ' units' : '0 units';
            });
        }
    });
    </script>
    
</asp:Content>
